using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEditor.StyleSheets;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Object = UnityEngine.Object;

namespace UnityEditor.Timeline
{
    class TimelineTrackGUI : TimelineGroupGUI, IClipCurveEditorOwner, IRowGUI
    {
        struct TrackDrawData
        {
            public bool                 m_AllowsRecording;
            public bool                 m_ShowTrackBindings;
            public bool                 m_HasBinding;
            public bool                 m_IsSubTrack;
            public PlayableBinding      m_Binding;
            public UnityEngine.Object   m_TrackBinding;
            public Texture              m_TrackIcon;
        }

        static class Styles
        {
            public static readonly string kArmForRecordDisabled = L10n.Tr("Recording is not permitted when Track Offsets are set to Auto. Track Offset settings can be changed in the track menu of the base track.");
            public static Texture2D kProblemIcon = DirectorStyles.GetBackgroundImage(DirectorStyles.Instance.warning);
        }

        static GUIContent s_ArmForRecordContentOn;
        static GUIContent s_ArmForRecordContentOff;
        static GUIContent s_ArmForRecordDisabled;

        bool m_InlineCurvesSkipped;
        int m_TrackHash = -1;
        int m_BlendHash = -1;
        int m_LastDirtyIndex = -1;

        readonly InfiniteTrackDrawer    m_InfiniteTrackDrawer;
        TrackItemsDrawer                m_ItemsDrawer;
        TrackDrawData                   m_TrackDrawData;
        TrackDrawOptions                m_TrackDrawOptions;
        readonly TrackEditor            m_TrackEditor;
        readonly GUIContent             m_DefaultTrackIcon;


        public override bool expandable
        {
            get { return hasChildren; }
        }

        internal InlineCurveEditor inlineCurveEditor { get; set; }

        public ClipCurveEditor clipCurveEditor { get; private set; }

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

        bool IClipCurveEditorOwner.showLoops
        {
            get { return false; }
        }

        TrackAsset IClipCurveEditorOwner.owner
        {
            get { return track; }
        }

        static bool DoesTrackAllowsRecording(TrackAsset track)
        {
            // if the root animation track is in auto mode, recording is not allowed
            var animTrack = TimelineUtility.GetSceneReferenceTrack(track) as AnimationTrack;
            if (animTrack != null)
                return animTrack.trackOffset != TrackOffset.Auto;

            return false;
        }

        bool? m_TrackHasAnimatableParameters;
        bool trackHasAnimatableParameters
        {
            get
            {
                // cache this value to avoid the recomputation
                if (!m_TrackHasAnimatableParameters.HasValue)
                    m_TrackHasAnimatableParameters = track.HasAnyAnimatableParameters() ||
                        track.clips.Any(c => c.HasAnyAnimatableParameters());

                return m_TrackHasAnimatableParameters.Value;
            }
        }

        public bool locked
        {
            get { return track.lockedInHierarchy; }
        }

        public bool showMarkers
        {
            get { return track.GetShowMarkers(); }
        }

        public bool muted
        {
            get { return track.muted; }
        }

        public List<TimelineClipGUI> clips
        {
            get
            {
                return m_ItemsDrawer.clips == null ? new List<TimelineClipGUI>(0) : m_ItemsDrawer.clips.ToList();
            }
        }

        TrackAsset IRowGUI.asset { get { return track; } }

        bool showTrackRecordingDisabled
        {
            get
            {
                // if the root animation track is in auto mode, recording is not allowed
                var animTrack = TimelineUtility.GetSceneReferenceTrack(track) as AnimationTrack;
                return animTrack != null && animTrack.trackOffset == TrackOffset.Auto;
            }
        }

        public TimelineTrackGUI(TreeViewController tv, TimelineTreeViewGUI w, int id, int depth, TreeViewItem parent, string displayName, TrackAsset sequenceActor)
            : base(tv, w, id, depth, parent, displayName, sequenceActor, false)
        {
            AnimationTrack animationTrack = sequenceActor as AnimationTrack;
            if (animationTrack != null)
                m_InfiniteTrackDrawer = new InfiniteTrackDrawer(new AnimationTrackKeyDataSource(animationTrack));
            else if (sequenceActor.HasAnyAnimatableParameters() && !sequenceActor.clips.Any())
                m_InfiniteTrackDrawer = new InfiniteTrackDrawer(new TrackPropertyCurvesDataSource(sequenceActor));

            UpdateInfiniteClipEditor(w.TimelineWindow);

            var bindings = track.outputs.ToArray();
            m_TrackDrawData.m_HasBinding = bindings.Length > 0;
            if (m_TrackDrawData.m_HasBinding)
                m_TrackDrawData.m_Binding = bindings[0];
            m_TrackDrawData.m_IsSubTrack = IsSubTrack();
            m_TrackDrawData.m_AllowsRecording = DoesTrackAllowsRecording(sequenceActor);
            m_DefaultTrackIcon = TrackResourceCache.GetTrackIcon(track);


            m_TrackEditor = CustomTimelineEditorCache.GetTrackEditor(sequenceActor);

            try
            {
                m_TrackDrawOptions = m_TrackEditor.GetTrackOptions(track, null);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                m_TrackDrawOptions = CustomTimelineEditorCache.GetDefaultTrackEditor().GetTrackOptions(track, null);
            }

            m_TrackDrawOptions.errorText = null; // explicitly setting to null for an uninitialized state
        }

        public override float GetVerticalSpacingBetweenTracks()
        {
            if (track != null && track.isSubTrack)
                return 1.0f; // subtracks have less of a gap than tracks
            return base.GetVerticalSpacingBetweenTracks();
        }

        void UpdateInfiniteClipEditor(TimelineWindow window)
        {
            if (clipCurveEditor != null || track == null || !track.ShouldShowInfiniteClipEditor())
                return;

            var dataSource = CurveDataSource.Create(this);
            clipCurveEditor = new ClipCurveEditor(dataSource, window, track);
        }

        void DetectTrackChanged()
        {
            if (Event.current.type == EventType.Layout)
            {
                // incremented when a track or it's clips changed
                if (m_LastDirtyIndex != track.DirtyIndex)
                {
                    try
                    {
                        m_TrackEditor.OnTrackChanged(track);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                    m_LastDirtyIndex = track.DirtyIndex;
                }
                OnTrackChanged();
            }
        }

        // Called when the source track data, including it's clips have changed has changed.
        void OnTrackChanged()
        {
            // recompute blends if necessary
            int newBlendHash = BlendHash();
            if (m_BlendHash != newBlendHash)
            {
                UpdateClipOverlaps();
                m_BlendHash = newBlendHash;
            }

            RebuildGUICacheIfNecessary();
        }

        void UpdateDrawData(WindowState state)
        {
            if (Event.current.type == EventType.Layout)
            {
                m_TrackDrawData.m_ShowTrackBindings = false;
                m_TrackDrawData.m_TrackBinding = null;


                if (state.editSequence.director != null && showSceneReference)
                {
                    m_TrackDrawData.m_ShowTrackBindings = state.GetWindow().currentMode.ShouldShowTrackBindings(state);
                    m_TrackDrawData.m_TrackBinding = state.editSequence.director.GetGenericBinding(track);
                }

                var lastError = m_TrackDrawOptions.errorText;
                var lastHeight = m_TrackDrawOptions.minimumHeight;
                try
                {
                    m_TrackDrawOptions = m_TrackEditor.GetTrackOptions(track, m_TrackDrawData.m_TrackBinding);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    m_TrackDrawOptions = CustomTimelineEditorCache.GetDefaultTrackEditor().GetTrackOptions(track, m_TrackDrawData.m_TrackBinding);
                }

                m_TrackDrawData.m_AllowsRecording = DoesTrackAllowsRecording(track);
                m_TrackDrawData.m_TrackIcon = m_TrackDrawOptions.icon;
                if (m_TrackDrawData.m_TrackIcon == null)
                    m_TrackDrawData.m_TrackIcon = m_DefaultTrackIcon.image;

                // track height has changed. need to update gui
                if (!Mathf.Approximately(lastHeight, m_TrackDrawOptions.minimumHeight))
                    state.Refresh();
            }
        }

        public override void Draw(Rect headerRect, Rect contentRect, WindowState state)
        {
            DetectTrackChanged();
            UpdateDrawData(state);

            UpdateInfiniteClipEditor(state.GetWindow());

            var trackHeaderRect = headerRect;
            var trackContentRect = contentRect;

            float inlineCurveHeight = contentRect.height - GetTrackContentHeight(state);
            bool hasInlineCurve = inlineCurveHeight > 0.0f;

            if (hasInlineCurve)
            {
                trackHeaderRect.height -= inlineCurveHeight;
                trackContentRect.height -= inlineCurveHeight;
            }

            if (Event.current.type == EventType.Repaint)
            {
                m_TreeViewRect = trackContentRect;
            }

            if (s_ArmForRecordContentOn == null)
                s_ArmForRecordContentOn = new GUIContent(DirectorStyles.GetBackgroundImage(DirectorStyles.Instance.autoKey, StyleState.active));

            if (s_ArmForRecordContentOff == null)
                s_ArmForRecordContentOff = new GUIContent(DirectorStyles.GetBackgroundImage(DirectorStyles.Instance.autoKey));

            if (s_ArmForRecordDisabled == null)
                s_ArmForRecordDisabled = new GUIContent(DirectorStyles.GetBackgroundImage(DirectorStyles.Instance.autoKey), Styles.kArmForRecordDisabled);

            track.SetCollapsed(!isExpanded);

            RebuildGUICacheIfNecessary();

            // Prevents from drawing outside of bounds, but does not effect layout or markers
            bool isOwnerDrawSucceed = false;

            Vector2 visibleTime = state.timeAreaShownRange;

            if (drawer != null)
                isOwnerDrawSucceed = drawer.DrawTrack(trackContentRect, track, visibleTime, state);

            if (!isOwnerDrawSucceed)
            {
                using (new GUIViewportScope(trackContentRect))
                    DrawBackground(trackContentRect, track, visibleTime, state);

                if (m_InfiniteTrackDrawer != null)
                    m_InfiniteTrackDrawer.DrawTrack(trackContentRect, track, visibleTime, state);

                // draw after user customization so overlay text shows up
                using (new GUIViewportScope(trackContentRect))
                    m_ItemsDrawer.Draw(trackContentRect, state);
            }

            DrawTrackHeader(trackHeaderRect, state);

            if (hasInlineCurve)
            {
                var curvesHeaderRect = headerRect;
                curvesHeaderRect.yMin = trackHeaderRect.yMax;

                var curvesContentRect = contentRect;
                curvesContentRect.yMin = trackContentRect.yMax;

                DrawInlineCurves(curvesHeaderRect, curvesContentRect, state);
            }

            DrawTrackColorKind(headerRect);
            DrawTrackState(contentRect, contentRect, track);
        }

        void DrawInlineCurves(Rect curvesHeaderRect, Rect curvesContentRect, WindowState state)
        {
            if (!track.GetShowInlineCurves())
                return;

            // Inline curves are not within the editor window -- case 952571
            if (!IsInlineCurvesEditorInBounds(ToWindowSpace(curvesHeaderRect), curvesContentRect.height, state))
            {
                m_InlineCurvesSkipped = true;
                return;
            }

            // If inline curves were skipped during the last event; we want to avoid rendering them until
            // the next Layout event. Otherwise, we still get the RTE prevented above when the user resizes
            // the timeline window very fast. -- case 952571
            if (m_InlineCurvesSkipped && Event.current.type != EventType.Layout)
                return;

            m_InlineCurvesSkipped = false;

            if (inlineCurveEditor == null)
                inlineCurveEditor = new InlineCurveEditor(this);


            curvesHeaderRect.x += DirectorStyles.kBaseIndent;
            curvesHeaderRect.width -= DirectorStyles.kBaseIndent;

            inlineCurveEditor.Draw(curvesHeaderRect, curvesContentRect, state);
        }

        static bool IsInlineCurvesEditorInBounds(Rect windowSpaceTrackRect, float inlineCurveHeight, WindowState state)
        {
            var legalHeight = state.windowHeight;
            var trackTop = windowSpaceTrackRect.y;
            var inlineCurveOffset = windowSpaceTrackRect.height - inlineCurveHeight;
            return legalHeight - trackTop - inlineCurveOffset > 0;
        }

        void DrawErrorIcon(Rect position, WindowState state)
        {
            Rect bindingLabel = position;
            bindingLabel.x = position.xMax + 3;
            bindingLabel.width = state.bindingAreaWidth;
            EditorGUI.LabelField(position, m_ProblemIcon);
        }

        void DrawBackground(Rect trackRect, TrackAsset trackAsset, Vector2 visibleTime, WindowState state)
        {
            bool canDrawRecordBackground = IsRecording(state);
            if (canDrawRecordBackground)
            {
                DrawRecordingTrackBackground(trackRect, trackAsset, visibleTime, state);
            }
            else
            {
                Color trackBackgroundColor;

                if (SelectionManager.Contains(track))
                {
                    trackBackgroundColor = state.IsEditingASubTimeline() ?
                        DirectorStyles.Instance.customSkin.colorTrackSubSequenceBackgroundSelected :
                        DirectorStyles.Instance.customSkin.colorTrackBackgroundSelected;
                }
                else
                {
                    trackBackgroundColor = state.IsEditingASubTimeline() ?
                        DirectorStyles.Instance.customSkin.colorTrackSubSequenceBackground :
                        DirectorStyles.Instance.customSkin.colorTrackBackground;
                }

                EditorGUI.DrawRect(trackRect, trackBackgroundColor);
            }
        }

        float InlineCurveHeight()
        {
            return track.GetShowInlineCurves() && CanDrawInlineCurve()
                ? TimelineWindowViewPrefs.GetInlineCurveHeight(track)
                : 0.0f;
        }

        public override float GetHeight(WindowState state)
        {
            var height = GetTrackContentHeight(state);

            if (CanDrawInlineCurve())
                height += InlineCurveHeight();

            return height;
        }

        float GetTrackContentHeight(WindowState state)
        {
            float height = m_TrackDrawOptions.minimumHeight;
            if (height <= 0.0f)
                height = TrackEditor.DefaultTrackHeight;
            height = Mathf.Clamp(height, TrackEditor.MinimumTrackHeight, TrackEditor.MaximumTrackHeight);

            return height * state.trackScale;
        }

        static bool CanDrawIcon(GUIContent icon)
        {
            return icon != null && icon != GUIContent.none && icon.image != null;
        }

        bool showSceneReference
        {
            get
            {
                return track != null &&
                    m_TrackDrawData.m_HasBinding &&
                    !m_TrackDrawData.m_IsSubTrack &&
                    m_TrackDrawData.m_Binding.sourceObject != null &&
                    m_TrackDrawData.m_Binding.outputTargetType != null &&
                    typeof(Object).IsAssignableFrom(m_TrackDrawData.m_Binding.outputTargetType);
            }
        }

        void DrawTrackHeader(Rect trackHeaderRect, WindowState state)
        {
            using (new GUIViewportScope(trackHeaderRect))
            {
                Rect rect = trackHeaderRect;

                DrawHeaderBackground(trackHeaderRect);
                rect.x += m_Styles.trackSwatchStyle.fixedWidth;

                const float buttonSize = WindowConstants.trackHeaderButtonSize;
                const float padding = WindowConstants.trackHeaderButtonPadding;
                var buttonRect = new Rect(trackHeaderRect.xMax - buttonSize - padding, rect.y + ((rect.height - buttonSize) / 2f), buttonSize, buttonSize);

                rect.x += DrawTrackIconKind(rect, state);
                DrawTrackBinding(rect, trackHeaderRect);

                if (track is GroupTrack)
                    return;

                buttonRect.x -= Spaced(DrawTrackDropDownMenu(buttonRect));
                buttonRect.x -= Spaced(DrawLockMarkersButton(buttonRect, state));
                buttonRect.x -= Spaced(DrawInlineCurveButton(buttonRect, state));
                buttonRect.x -= Spaced(DrawMuteButton(buttonRect, state));
                buttonRect.x -= Spaced(DrawLockButton(buttonRect, state));
                buttonRect.x -= Spaced(DrawRecordButton(buttonRect, state));
                buttonRect.x -= Spaced(DrawCustomTrackButton(buttonRect, state));
            }
        }

        void DrawHeaderBackground(Rect headerRect)
        {
            Color backgroundColor = SelectionManager.Contains(track)
                ? DirectorStyles.Instance.customSkin.colorSelection
                : DirectorStyles.Instance.customSkin.colorTrackHeaderBackground;

            var bgRect = headerRect;
            bgRect.x += m_Styles.trackSwatchStyle.fixedWidth;
            bgRect.width -= m_Styles.trackSwatchStyle.fixedWidth;

            EditorGUI.DrawRect(bgRect, backgroundColor);
        }

        void DrawTrackColorKind(Rect rect)
        {
            // subtracks don't draw the color, the parent does that.
            if (track != null && track.isSubTrack)
                return;

            using (new GUIColorOverride(m_TrackDrawOptions.trackColor))
            {
                rect.width = m_Styles.trackSwatchStyle.fixedWidth;
                GUI.Label(rect, GUIContent.none, m_Styles.trackSwatchStyle);
            }
        }

        float DrawTrackIconKind(Rect rect, WindowState state)
        {
            // no icons on subtracks
            if (track != null && track.isSubTrack)
                return 0.0f;

            rect.yMin += (rect.height - 16f) / 2f;
            rect.width = 16.0f;
            rect.height = 16.0f;

            if (!string.IsNullOrEmpty(m_TrackDrawOptions.errorText))
            {
                m_ProblemIcon.image = Styles.kProblemIcon;
                m_ProblemIcon.tooltip = m_TrackDrawOptions.errorText;

                if (CanDrawIcon(m_ProblemIcon))
                    DrawErrorIcon(rect, state);
            }
            else
            {
                var content = GUIContent.Temp(m_TrackDrawData.m_TrackIcon, m_DefaultTrackIcon.tooltip);
                if (CanDrawIcon(content))
                    GUI.Box(rect, content, GUIStyle.none);
            }

            return rect.width;
        }

        void DrawTrackBinding(Rect rect, Rect headerRect)
        {
            if (m_TrackDrawData.m_ShowTrackBindings)
            {
                DoTrackBindingGUI(rect, headerRect);
                return;
            }

            var textStyle = m_Styles.trackHeaderFont;
            textStyle.normal.textColor = SelectionManager.Contains(track) ? Color.white : m_Styles.customSkin.colorTrackFont;

            string trackName = track.name;

            EditorGUI.BeginChangeCheck();

            // by default the size is just the width of the string (for selection purposes)
            rect.width = m_Styles.trackHeaderFont.CalcSize(new GUIContent(trackName)).x;

            // if we are editing, supply the entire width of the header
            if (GUIUtility.keyboardControl == track.GetInstanceID())
                rect.width = (headerRect.xMax - rect.xMin) - (5 * WindowConstants.trackHeaderButtonSize);

            trackName = EditorGUI.DelayedTextField(rect, GUIContent.none, track.GetInstanceID(), track.name, textStyle);

            if (EditorGUI.EndChangeCheck())
            {
                TimelineUndo.PushUndo(track, "Rename Track");
                track.name = trackName;
            }
        }

        float DrawTrackDropDownMenu(Rect rect)
        {
            rect.y += WindowConstants.trackOptionButtonVerticalPadding;

            if (GUI.Button(rect, GUIContent.none, m_Styles.trackOptions))
            {
                // the drop down will apply to all selected tracks
                if (!SelectionManager.Contains(track))
                {
                    SelectionManager.Clear();
                    SelectionManager.Add(track);
                }

                SequencerContextMenu.ShowTrackContextMenu(SelectionManager.SelectedTracks().ToArray(), null);
            }

            return WindowConstants.trackHeaderButtonSize;
        }

        bool CanDrawInlineCurve()
        {
            // Note: A track with animatable parameters always has inline curves.
            return trackHasAnimatableParameters || TimelineUtility.TrackHasAnimationCurves(track);
        }

        float DrawInlineCurveButton(Rect rect, WindowState state)
        {
            if (!CanDrawInlineCurve())
            {
                return 0.0f;
            }

            // Override enable state to display "Show Inline Curves" button in disabled state.
            bool prevEnabledState = GUI.enabled;
            GUI.enabled = true;
            var newValue = GUI.Toggle(rect, track.GetShowInlineCurves(), GUIContent.none, DirectorStyles.Instance.curves);
            GUI.enabled = prevEnabledState;

            if (newValue != track.GetShowInlineCurves())
            {
                if (!state.editSequence.isReadOnly)
                    TimelineUndo.PushUndo(track, newValue ? "Show Inline Curves" : "Hide Inline Curves");
                track.SetShowInlineCurves(newValue);
                state.GetWindow().treeView.CalculateRowRects();
            }

            return WindowConstants.trackHeaderButtonSize;
        }

        float DrawRecordButton(Rect rect, WindowState state)
        {
            if (m_TrackDrawData.m_AllowsRecording)
            {
                bool isPlayerDisabled = state.editSequence.director != null && !state.editSequence.director.isActiveAndEnabled;
                using (new EditorGUI.DisabledScope(track.lockedInHierarchy || isPlayerDisabled || !string.IsNullOrEmpty(m_TrackDrawOptions.errorText)))
                {
                    if (IsRecording(state))
                    {
                        state.editorWindow.Repaint();
                        float remainder = Time.realtimeSinceStartup % 1;

                        var animatedContent = s_ArmForRecordContentOn;
                        if (remainder < 0.22f)
                        {
                            animatedContent = GUIContent.none;
                        }
                        if (GUI.Button(rect, animatedContent, GUIStyle.none) || isPlayerDisabled)
                        {
                            state.UnarmForRecord(track);
                        }
                    }
                    else
                    {
                        if (GUI.Button(rect, s_ArmForRecordContentOff, GUIStyle.none))
                        {
                            state.ArmForRecord(track);
                        }
                    }
                    return WindowConstants.trackHeaderButtonSize;
                }
            }

            if (showTrackRecordingDisabled)
            {
                using (new EditorGUI.DisabledScope(true))
                    GUI.Button(rect, s_ArmForRecordDisabled, GUIStyle.none);
                return k_ButtonSize;
            }

            return 0.0f;
        }

        float DrawCustomTrackButton(Rect rect, WindowState state)
        {
            if (drawer.DrawTrackHeaderButton(rect, track, state))
            {
                return WindowConstants.trackHeaderButtonSize;
            }
            return 0.0f;
        }

        float DrawLockMarkersButton(Rect rect, WindowState state)
        {
            if (track.GetMarkerCount() == 0)
                return 0.0f;

            var markersShown = showMarkers;
            var style = TimelineWindow.styles.collapseMarkers;
            if (Event.current.type == EventType.Repaint)
                style.Draw(rect, GUIContent.none, false, false, markersShown, false);

            // Override enable state to display "Show Marker" button in disabled state.
            bool prevEnabledState = GUI.enabled;
            GUI.enabled = true;
            if (GUI.Button(rect, DirectorStyles.markerCollapseButton, GUIStyle.none))
            {
                state.GetWindow().SetShowTrackMarkers(track, !markersShown);
            }
            GUI.enabled = prevEnabledState;
            return WindowConstants.trackHeaderButtonSize;
        }

        static void ObjectBindingField(Rect position, Object obj, PlayableBinding binding)
        {
            bool allowScene =
                typeof(GameObject).IsAssignableFrom(binding.outputTargetType) ||
                typeof(Component).IsAssignableFrom(binding.outputTargetType);

            EditorGUI.BeginChangeCheck();
            // FocusType.Passive so it never gets focused when pressing tab
            int controlId = GUIUtility.GetControlID("s_ObjectFieldHash".GetHashCode(), FocusType.Passive, position);
            var newObject = EditorGUI.DoObjectField(EditorGUI.IndentedRect(position), EditorGUI.IndentedRect(position), controlId, obj, binding.outputTargetType, null, null, allowScene, EditorStyles.objectField);
            if (EditorGUI.EndChangeCheck())
            {
                BindingUtility.Bind(TimelineEditor.inspectedDirector, binding.sourceObject as TrackAsset, newObject);
            }
        }

        void DoTrackBindingGUI(Rect rect, Rect headerRect)
        {
            var bindingRect = new Rect(
                rect.xMin,
                rect.y + (rect.height - WindowConstants.trackHeaderButtonSize) / 2f,
                headerRect.xMax - WindowConstants.trackHeaderMaxButtonsWidth - rect.xMin,
                WindowConstants.trackHeaderButtonSize);

            if (bindingRect.Contains(Event.current.mousePosition) && TimelineDragging.IsDraggingEvent() && DragAndDrop.objectReferences.Length == 1)
            {
                TimelineDragging.HandleBindingDragAndDrop(track, BindingUtility.GetRequiredBindingType(m_TrackDrawData.m_Binding));
                Event.current.Use();
            }
            else
            {
                if (m_TrackDrawData.m_Binding.outputTargetType != null && typeof(Object).IsAssignableFrom(m_TrackDrawData.m_Binding.outputTargetType))
                {
                    ObjectBindingField(bindingRect, m_TrackDrawData.m_TrackBinding, m_TrackDrawData.m_Binding);
                }
            }
        }

        bool IsRecording(WindowState state)
        {
            return state.recording && state.IsArmedForRecord(track);
        }

        // background to draw during recording
        void DrawRecordingTrackBackground(Rect trackRect, TrackAsset trackAsset, Vector2 visibleTime, WindowState state)
        {
            if (drawer != null)
                drawer.DrawRecordingBackground(trackRect, trackAsset, visibleTime, state);
        }

        void UpdateClipOverlaps()
        {
            TrackExtensions.ComputeBlendsFromOverlaps(track.clips);
        }

        internal void RebuildGUICacheIfNecessary()
        {
            if (m_TrackHash == track.Hash())
                return;

            m_ItemsDrawer = new TrackItemsDrawer(this);
            m_TrackHash = track.Hash();
        }

        int BlendHash()
        {
            var hash = 0;
            foreach (var clip in track.clips)
            {
                hash = HashUtility.CombineHash(hash,
                    (clip.duration - clip.start).GetHashCode(),
                    ((int)clip.blendInCurveMode).GetHashCode(),
                    ((int)clip.blendOutCurveMode).GetHashCode());
            }
            return hash;
        }

        // callback when the corresponding graph is rebuilt. This can happen, but not have the GUI rebuilt.
        public override void OnGraphRebuilt()
        {
            RefreshCurveEditor();
        }

        void RefreshCurveEditor()
        {
            var window = TimelineWindow.instance;
            if (track != null && window != null && window.state != null)
            {
                bool hasEditor = clipCurveEditor != null;
                bool shouldHaveEditor = track.ShouldShowInfiniteClipEditor();
                if (hasEditor != shouldHaveEditor)
                    window.state.AddEndFrameDelegate((x, currentEvent) =>
                    {
                        x.Refresh();
                        return true;
                    });
            }
        }
    }
}
