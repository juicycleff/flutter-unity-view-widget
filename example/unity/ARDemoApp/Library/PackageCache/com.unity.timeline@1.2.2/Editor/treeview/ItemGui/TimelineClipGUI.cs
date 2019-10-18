using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    class TimelineClipGUI : TimelineItemGUI, IClipCurveEditorOwner, ISnappable, IAttractable
    {
        EditorClip m_EditorItem;

        Rect m_ClipCenterSection;
        readonly List<Rect> m_LoopRects = new List<Rect>();

        ClipDrawData m_ClipDrawData;
        Rect m_MixOutRect = new Rect();
        Rect m_MixInRect = new Rect();
        int m_MinLoopIndex = 1;

        // clip dirty detection
        int m_LastDirtyIndex = Int32.MinValue;
        bool m_ClipViewDirty = true;

        bool supportResize { get; }
        public ClipCurveEditor clipCurveEditor { get; set; }
        public TimelineClipGUI previousClip { get; set; }
        public TimelineClipGUI nextClip { get; set; }

        static readonly float k_MinMixWidth = 2;
        static readonly float k_MaxHandleWidth = 10f;
        static readonly float k_MinHandleWidth = 1f;

        bool? m_ShowDrillIcon;
        ClipEditor m_ClipEditor;

        static List<PlayableDirector> s_TempSubDirectors = new List<PlayableDirector>();

        static readonly IconData k_DiggableClipIcon = new IconData(DirectorStyles.LoadIcon("TimelineDigIn"));

        string name
        {
            get
            {
                if (string.IsNullOrEmpty(clip.displayName))
                    return "(Empty)";

                return clip.displayName;
            }
        }

        public bool inlineCurvesSelected
        {
            get { return SelectionManager.IsCurveEditorFocused(this); }
            set
            {
                if (!value && SelectionManager.IsCurveEditorFocused(this))
                    SelectionManager.SelectInlineCurveEditor(null);
                else
                    SelectionManager.SelectInlineCurveEditor(this);
            }
        }

        public Rect mixOutRect
        {
            get
            {
                float percent = clip.mixOutPercentage;
                m_MixOutRect.Set(treeViewRect.width * (1 - percent), 0.0f, treeViewRect.width * percent, treeViewRect.height);
                return m_MixOutRect;
            }
        }

        public Rect mixInRect
        {
            get
            {
                m_MixInRect.Set(0.0f, 0.0f, treeViewRect.width * clip.mixInPercentage, treeViewRect.height);
                return m_MixInRect;
            }
        }

        public ClipBlends GetClipBlends()
        {
            var _mixInRect = mixInRect;
            var _mixOutRect = mixOutRect;

            var blendInKind = BlendKind.None;
            if (_mixInRect.width > k_MinMixWidth && clip.hasBlendIn)
                blendInKind = BlendKind.Mix;
            else if (_mixInRect.width > k_MinMixWidth)
                blendInKind = BlendKind.Ease;

            var blendOutKind = BlendKind.None;
            if (_mixOutRect.width > k_MinMixWidth && clip.hasBlendOut)
                blendOutKind = BlendKind.Mix;
            else if (_mixOutRect.width > k_MinMixWidth)
                blendOutKind = BlendKind.Ease;

            return new ClipBlends(blendInKind, _mixInRect, blendOutKind, _mixOutRect);
        }

        public override double start
        {
            get { return clip.start; }
        }

        public override double end
        {
            get { return clip.end; }
        }

        public bool supportsLooping
        {
            get { return clip.SupportsLooping(); }
        }

        // for the inline curve editor, only show loops if we recorded the asset
        bool IClipCurveEditorOwner.showLoops
        {
            get { return clip.SupportsLooping() && (clip.asset is AnimationPlayableAsset);  }
        }

        TrackAsset IClipCurveEditorOwner.owner
        {
            get { return clip.parentTrack; }
        }

        public bool supportsSubTimelines
        {
            get { return m_ClipEditor.supportsSubTimelines; }
        }


        public int minLoopIndex
        {
            get { return m_MinLoopIndex; }
        }

        public TrackDrawer drawer
        {
            get { return ((TimelineTrackGUI)parent).drawer; }
        }

        public Rect clippedRect { get; private set; }

        public override void Select()
        {
            zOrder = zOrderProvider.Next();
            SelectionManager.Add(clip);
        }

        public override bool IsSelected()
        {
            return SelectionManager.Contains(clip);
        }

        public override void Deselect()
        {
            SelectionManager.Remove(clip);
        }

        public override ITimelineItem item
        {
            get { return ItemsUtils.ToItem(clip); }
        }

        IZOrderProvider zOrderProvider { get; }

        public TimelineClipHandle leftHandle { get; }
        public TimelineClipHandle rightHandle { get; }

        public TimelineClipGUI(TimelineClip clip, IRowGUI parent, IZOrderProvider provider) : base(parent)
        {
            zOrderProvider = provider;
            zOrder = provider.Next();

            m_EditorItem = EditorClipFactory.GetEditorClip(clip);
            m_ClipEditor = CustomTimelineEditorCache.GetClipEditor(clip);

            supportResize = true;

            leftHandle = new TimelineClipHandle(this, TrimEdge.Start);
            rightHandle = new TimelineClipHandle(this, TrimEdge.End);

            ItemToItemGui.Add(clip, this);
        }

        void CreateInlineCurveEditor(WindowState state)
        {
            if (clipCurveEditor != null)
                return;

            var animationClip = clip.animationClip;

            if (animationClip != null && animationClip.empty)
                animationClip = null;

            // prune out clips coming from FBX
            if (animationClip != null && !clip.recordable)
                return; // don't show, even if there are curves

            if (animationClip == null && !clip.HasAnyAnimatableParameters())
                return; // nothing to show

            state.AddEndFrameDelegate((istate, currentEvent) =>
            {
                clipCurveEditor = new ClipCurveEditor(CurveDataSource.Create(this), TimelineWindow.instance, clip.parentTrack);
                return true;
            });
        }

        public TimelineClip clip
        {
            get { return m_EditorItem.clip; }
        }

        // Draw the actual clip. Defers to the track drawer for customization
        void UpdateDrawData(WindowState state, Rect drawRect, string title, bool selected, float rectXOffset)
        {
            m_ClipDrawData.clip = clip;
            m_ClipDrawData.targetRect = drawRect;
            m_ClipDrawData.clipCenterSection = m_ClipCenterSection;
            m_ClipDrawData.unclippedRect = treeViewRect;
            m_ClipDrawData.title = title;
            m_ClipDrawData.selected = selected;
            m_ClipDrawData.inlineCurvesSelected = inlineCurvesSelected;
            m_ClipDrawData.previousClip = previousClip != null ? previousClip.clip : null;

            Vector3 shownAreaTime = state.timeAreaShownRange;
            m_ClipDrawData.localVisibleStartTime = clip.ToLocalTimeUnbound(Math.Max(clip.start, shownAreaTime.x));
            m_ClipDrawData.localVisibleEndTime = clip.ToLocalTimeUnbound(Math.Min(clip.end, shownAreaTime.y));

            m_ClipDrawData.clippedRect = new Rect(clippedRect.x - rectXOffset, 0.0f, clippedRect.width, clippedRect.height);

            m_ClipDrawData.minLoopIndex = minLoopIndex;
            m_ClipDrawData.loopRects = m_LoopRects;
            m_ClipDrawData.supportsLooping = supportsLooping;
            m_ClipDrawData.clipBlends = GetClipBlends();
            m_ClipDrawData.clipEditor = m_ClipEditor;
            m_ClipDrawData.ClipDrawOptions = UpdateClipDrawOptions(m_ClipEditor, clip);

            UpdateClipIcons(state);
        }

        void UpdateClipIcons(WindowState state)
        {
            // Pass 1 - gather size
            int required = 0;
            bool requiresDigIn = ShowDrillIcon(state.editSequence.director);
            if (requiresDigIn)
                required++;

            var icons = m_ClipDrawData.ClipDrawOptions.icons;
            foreach (var icon in icons)
            {
                if (icon != null)
                    required++;
            }

            // Pass 2 - copy icon data
            if (required == 0)
            {
                m_ClipDrawData.rightIcons = null;
                return;
            }

            if (m_ClipDrawData.rightIcons == null || m_ClipDrawData.rightIcons.Length != required)
                m_ClipDrawData.rightIcons = new IconData[required];

            int index = 0;
            if (requiresDigIn)
                m_ClipDrawData.rightIcons[index++] = k_DiggableClipIcon;

            foreach (var icon in icons)
            {
                if (icon != null)
                    m_ClipDrawData.rightIcons[index++] = new IconData(icon);
            }
        }

        static ClipDrawOptions UpdateClipDrawOptions(ClipEditor clipEditor, TimelineClip clip)
        {
            try
            {
                return clipEditor.GetClipOptions(clip);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return CustomTimelineEditorCache.GetDefaultClipEditor().GetClipOptions(clip);
        }

        static void DrawClip(ClipDrawData drawData)
        {
            ClipDrawer.DrawDefaultClip(drawData);

            if (drawData.clip.asset is AnimationPlayableAsset)
            {
                var state = TimelineWindow.instance.state;
                if (state.recording && state.IsArmedForRecord(drawData.clip.parentTrack))
                {
                    ClipDrawer.DrawAnimationRecordBorder(drawData);
                    ClipDrawer.DrawRecordProhibited(drawData);
                }
            }
        }

        public void DrawGhostClip(Rect targetRect)
        {
            DrawSimpleClip(targetRect, ClipBorder.kSelection, new Color(1.0f, 1.0f, 1.0f, 0.5f));
        }

        public void DrawInvalidClip(Rect targetRect)
        {
            DrawSimpleClip(targetRect, ClipBorder.kSelection, DirectorStyles.Instance.customSkin.colorInvalidClipOverlay);
        }

        void DrawSimpleClip(Rect targetRect, ClipBorder border, Color overlay)
        {
            var drawOptions = UpdateClipDrawOptions(CustomTimelineEditorCache.GetClipEditor(clip), clip);
            var blends = GetClipBlends();
            ClipDrawer.DrawSimpleClip(clip, targetRect, border, overlay, drawOptions, blends);
        }

        void DrawInto(Rect drawRect, WindowState state)
        {
            if (Event.current.type != EventType.Repaint)
                return;

            // create the inline curve editor if not already created
            CreateInlineCurveEditor(state);

            // @todo optimization, most of the calculations (rect, offsets, colors, etc.) could be cached
            // and rebuilt when the hash of the clip changes.

            if (isInvalid)
            {
                DrawInvalidClip(treeViewRect);
                return;
            }

            GUI.BeginClip(drawRect);

            var originRect = new Rect(0.0f, 0.0f, drawRect.width, drawRect.height);
            string clipLabel = name;
            bool selected = SelectionManager.Contains(clip);

            if (selected && 1.0 != clip.timeScale)
                clipLabel += " " + clip.timeScale.ToString("F2") + "x";

            UpdateDrawData(state, originRect, clipLabel, selected, drawRect.x);
            DrawClip(m_ClipDrawData);

            GUI.EndClip();

            if (clip.parentTrack != null && !clip.parentTrack.lockedInHierarchy)
            {
                if (selected && supportResize)
                {
                    var cursorRect = rect;
                    cursorRect.xMin += leftHandle.boundingRect.width;
                    cursorRect.xMax -= rightHandle.boundingRect.width;
                    EditorGUIUtility.AddCursorRect(cursorRect, MouseCursor.MoveArrow);
                }

                if (supportResize)
                {
                    var handleWidth = Mathf.Clamp(drawRect.width * 0.3f, k_MinHandleWidth, k_MaxHandleWidth);

                    leftHandle.Draw(drawRect, handleWidth, state);
                    rightHandle.Draw(drawRect, handleWidth, state);
                }
            }
        }

        void CalculateClipRectangle(Rect trackRect, WindowState state)
        {
            if (m_ClipViewDirty)
            {
                var clipRect = RectToTimeline(trackRect, state);
                treeViewRect = clipRect;

                // calculate clipped rect
                clipRect.xMin = Mathf.Max(clipRect.xMin, trackRect.xMin);
                clipRect.xMax = Mathf.Min(clipRect.xMax, trackRect.xMax);

                if (clipRect.width > 0 && clipRect.width < 2)
                {
                    clipRect.width = 5.0f;
                }

                clippedRect = clipRect;
            }
        }

        void AddToSpacePartitioner(WindowState state)
        {
            if (Event.current.type == EventType.Repaint && !parent.locked)
                state.spacePartitioner.AddBounds(this, rect);
        }

        void CalculateBlendRect()
        {
            m_ClipCenterSection = treeViewRect;
            m_ClipCenterSection.x = 0;
            m_ClipCenterSection.y = 0;

            m_ClipCenterSection.xMin = treeViewRect.width * clip.mixInPercentage;

            m_ClipCenterSection.width = treeViewRect.width;
            m_ClipCenterSection.xMax -= mixOutRect.width;
            m_ClipCenterSection.xMax -= (treeViewRect.width * clip.mixInPercentage);
        }

        // Entry point to the Clip Drawing...
        public override void Draw(Rect trackRect, bool trackRectChanged, WindowState state)
        {
            // if the clip has changed, fire the appropriate callback
            DetectClipChanged(trackRectChanged);

            // update the clip projected rectangle on the timeline
            CalculateClipRectangle(trackRect, state);

            AddToSpacePartitioner(state);

            // update the blend rects (when clip overlaps with others)
            CalculateBlendRect();

            // update the loop rects (when clip loops)
            CalculateLoopRects(trackRect, state);

            DrawExtrapolation(trackRect, treeViewRect);

            DrawInto(treeViewRect, state);

            ResetClipChanged();
        }

        void DetectClipChanged(bool trackRectChanged)
        {
            if (Event.current.type == EventType.Layout)
            {
                if (clip.DirtyIndex != m_LastDirtyIndex)
                {
                    m_ClipViewDirty = true;

                    try
                    {
                        m_ClipEditor.OnClipChanged(clip);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }

                    m_LastDirtyIndex = clip.DirtyIndex;
                }
                m_ClipViewDirty |= trackRectChanged;
            }
        }

        void ResetClipChanged()
        {
            if (Event.current.type == EventType.Repaint)
                m_ClipViewDirty = false;
        }

        GUIStyle GetExtrapolationIcon(TimelineClip.ClipExtrapolation mode)
        {
            GUIStyle extrapolationIcon = null;

            switch (mode)
            {
                case TimelineClip.ClipExtrapolation.None: return null;
                case TimelineClip.ClipExtrapolation.Hold: extrapolationIcon = m_Styles.extrapolationHold; break;
                case TimelineClip.ClipExtrapolation.Loop: extrapolationIcon = m_Styles.extrapolationLoop; break;
                case TimelineClip.ClipExtrapolation.PingPong: extrapolationIcon = m_Styles.extrapolationPingPong; break;
                case TimelineClip.ClipExtrapolation.Continue: extrapolationIcon = m_Styles.extrapolationContinue; break;
            }

            return extrapolationIcon;
        }

        Rect GetPreExtrapolationBounds(Rect trackRect, Rect clipRect, GUIStyle icon)
        {
            float x = clipRect.xMin - (icon.fixedWidth + 10.0f);
            float y = trackRect.yMin + (trackRect.height - icon.fixedHeight) / 2.0f;

            if (previousClip != null)
            {
                float distance = Mathf.Abs(treeViewRect.xMin - previousClip.treeViewRect.xMax);

                if (distance < icon.fixedWidth)
                    return new Rect(0.0f, 0.0f, 0.0f, 0.0f);

                if (distance < icon.fixedWidth + 20.0f)
                {
                    float delta = (distance - icon.fixedWidth) / 2.0f;
                    x = clipRect.xMin - (icon.fixedWidth + delta);
                }
            }

            return new Rect(x, y, icon.fixedWidth, icon.fixedHeight);
        }

        Rect GetPostExtrapolationBounds(Rect trackRect, Rect clipRect, GUIStyle icon)
        {
            float x = clipRect.xMax + 10.0f;
            float y = trackRect.yMin + (trackRect.height - icon.fixedHeight) / 2.0f;

            if (nextClip != null)
            {
                float distance = Mathf.Abs(nextClip.treeViewRect.xMin - treeViewRect.xMax);

                if (distance < icon.fixedWidth)
                    return new Rect(0.0f, 0.0f, 0.0f, 0.0f);

                if (distance < icon.fixedWidth + 20.0f)
                {
                    float delta = (distance - icon.fixedWidth) / 2.0f;
                    x = clipRect.xMax + delta;
                }
            }

            return new Rect(x, y, icon.fixedWidth, icon.fixedHeight);
        }

        static void DrawExtrapolationIcon(Rect rect, GUIStyle icon)
        {
            GUI.Label(rect, GUIContent.none, icon);
        }

        void DrawExtrapolation(Rect trackRect, Rect clipRect)
        {
            if (clip.hasPreExtrapolation)
            {
                GUIStyle icon = GetExtrapolationIcon(clip.preExtrapolationMode);

                if (icon != null)
                {
                    Rect iconBounds = GetPreExtrapolationBounds(trackRect, clipRect, icon);

                    if (iconBounds.width > 1 && iconBounds.height > 1)
                        DrawExtrapolationIcon(iconBounds, icon);
                }
            }

            if (clip.hasPostExtrapolation)
            {
                GUIStyle icon = GetExtrapolationIcon(clip.postExtrapolationMode);

                if (icon != null)
                {
                    Rect iconBounds = GetPostExtrapolationBounds(trackRect, clipRect, icon);

                    if (iconBounds.width > 1 && iconBounds.height > 1)
                        DrawExtrapolationIcon(iconBounds, icon);
                }
            }
        }

        static Rect ProjectRectOnTimeline(Rect rect, Rect trackRect, WindowState state)
        {
            Rect newRect = rect;
            // transform clipRect into pixel-space
            newRect.x *= state.timeAreaScale.x;
            newRect.width *= state.timeAreaScale.x;

            newRect.x += state.timeAreaTranslation.x + trackRect.xMin;

            // adjust clipRect height and vertical centering
            const int clipPadding = 2;
            newRect.y = trackRect.y + clipPadding;
            newRect.height = trackRect.height - (2 * clipPadding);
            return newRect;
        }

        void CalculateLoopRects(Rect trackRect, WindowState state)
        {
            if (!m_ClipViewDirty)
                return;

            m_LoopRects.Clear();
            if (clip.duration < WindowState.kTimeEpsilon)
                return;

            var times = TimelineHelpers.GetLoopTimes(clip);
            var loopDuration = TimelineHelpers.GetLoopDuration(clip);
            m_MinLoopIndex = -1;

            // we have a hold, no need to compute all loops
            if (!supportsLooping)
            {
                if (times.Length > 1)
                {
                    var t = times[1];
                    float loopTime = (float)(clip.duration - t);
                    m_LoopRects.Add(ProjectRectOnTimeline(new Rect((float)(t + clip.start), 0, loopTime, 0), trackRect, state));
                }
                return;
            }

            var range = state.timeAreaShownRange;
            var visibleStartTime = range.x - clip.start;
            var visibleEndTime = range.y - clip.start;

            for (int i = 1; i < times.Length; i++)
            {
                var t = times[i];

                // don't draw off screen loops
                if (t > visibleEndTime)
                    break;

                float loopTime = Mathf.Min((float)(clip.duration - t), (float)loopDuration);
                var loopEnd = t + loopTime;

                if (loopEnd < visibleStartTime)
                    continue;

                m_LoopRects.Add(ProjectRectOnTimeline(new Rect((float)(t + clip.start), 0, loopTime, 0), trackRect, state));

                if (m_MinLoopIndex == -1)
                    m_MinLoopIndex = i;
            }
        }

        public override Rect RectToTimeline(Rect trackRect, WindowState state)
        {
            var offsetFromTimeSpaceToPixelSpace = state.timeAreaTranslation.x + trackRect.xMin;

            var start = (float)(DiscreteTime)clip.start;
            var end = (float)(DiscreteTime)clip.end;

            return Rect.MinMaxRect(
                Mathf.Round(start * state.timeAreaScale.x + offsetFromTimeSpaceToPixelSpace), Mathf.Round(trackRect.yMin),
                Mathf.Round(end * state.timeAreaScale.x + offsetFromTimeSpaceToPixelSpace), Mathf.Round(trackRect.yMax)
            );
        }

        public IEnumerable<Edge> SnappableEdgesFor(IAttractable attractable, ManipulateEdges manipulateEdges)
        {
            var edges = new List<Edge>();

            bool canAddEdges = !parent.muted;

            if (canAddEdges)
            {
                // Hack: Trim Start in Ripple mode should not have any snap point added
                if (EditMode.editType == EditMode.EditType.Ripple && manipulateEdges == ManipulateEdges.Left)
                    return edges;

                if (attractable != this)
                {
                    if (EditMode.editType == EditMode.EditType.Ripple)
                    {
                        bool skip = false;

                        // Hack: Since Trim End and Move in Ripple mode causes other snap point to move on the same track (which is not supported), disable snapping for this special cases...
                        // TODO Find a proper way to have different snap edges for each edit mode.
                        if (manipulateEdges == ManipulateEdges.Right)
                        {
                            var otherClipGUI = attractable as TimelineClipGUI;
                            skip = otherClipGUI != null && otherClipGUI.parent == parent;
                        }
                        else if (manipulateEdges == ManipulateEdges.Both)
                        {
                            var moveHandler = attractable as MoveItemHandler;
                            skip = moveHandler != null && moveHandler.movingItems.Any(clips => clips.targetTrack == clip.parentTrack && clip.start >= clips.start);
                        }

                        if (skip)
                            return edges;
                    }

                    AddEdge(edges, clip.start);
                    AddEdge(edges, clip.end);
                }
                else
                {
                    if (manipulateEdges == ManipulateEdges.Right)
                    {
                        var d = TimelineHelpers.GetClipAssetEndTime(clip);

                        if (d < double.MaxValue)
                        {
                            if (clip.SupportsLooping())
                            {
                                var l = TimelineHelpers.GetLoopDuration(clip);

                                var shownTime = TimelineWindow.instance.state.timeAreaShownRange;
                                do
                                {
                                    AddEdge(edges, d, false);
                                    d += l;
                                }
                                while (d < shownTime.y);
                            }
                            else
                            {
                                AddEdge(edges, d, false);
                            }
                        }
                    }

                    if (manipulateEdges == ManipulateEdges.Left)
                    {
                        var clipInfo = AnimationClipCurveCache.Instance.GetCurveInfo(clip.animationClip);
                        if (clipInfo != null && clipInfo.keyTimes.Any())
                            AddEdge(edges, clip.FromLocalTimeUnbound(clipInfo.keyTimes.Min()), false);
                    }
                }
            }
            return edges;
        }

        public bool ShouldSnapTo(ISnappable snappable)
        {
            return true;
        }

        bool ShowDrillIcon(PlayableDirector resolver)
        {
            if (!m_ShowDrillIcon.HasValue || TimelineWindow.instance.hierarchyChangedThisFrame)
            {
                var nestable = m_ClipEditor.supportsSubTimelines;
                m_ShowDrillIcon = nestable && resolver != null;
                if (m_ShowDrillIcon.Value)
                {
                    s_TempSubDirectors.Clear();
                    try
                    {
                        m_ClipEditor.GetSubTimelines(clip, resolver, s_TempSubDirectors);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }

                    m_ShowDrillIcon &= s_TempSubDirectors.Count > 0;
                }
            }

            return m_ShowDrillIcon.Value;
        }

        static void AddEdge(List<Edge> edges, double time, bool showEdgeHint = true)
        {
            var shownTime = TimelineWindow.instance.state.timeAreaShownRange;
            if (time >= shownTime.x && time <= shownTime.y)
                edges.Add(new Edge(time, showEdgeHint));
        }
    }
}
