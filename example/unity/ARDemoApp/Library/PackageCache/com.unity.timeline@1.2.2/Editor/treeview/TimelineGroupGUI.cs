using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    class TimelineGroupGUI : TimelineTrackBaseGUI
    {
        protected DirectorStyles m_Styles;
        protected Rect m_TreeViewRect = new Rect(0, 0, 0, 0);
        protected GUIContent m_ProblemIcon = new GUIContent();

        bool m_MustRecomputeUnions = true;
        int m_GroupDepth;
        readonly bool m_IsReferencedTrack;
        readonly List<TimelineClipUnion> m_Unions = new List<TimelineClipUnion>();

        public override Rect boundingRect
        {
            get { return ToWindowSpace(m_TreeViewRect); }
        }

        public Rect ToWindowSpace(Rect localRect)
        {
            localRect.position += treeViewToWindowTransformation;
            return localRect;
        }

        public override bool expandable
        {
            get { return !m_IsRoot; }
        }

        // The expanded rectangle (contains children) as calculated by the the tree gui
        public Rect expandedRect { get; set; }
        // The row rectangle (header only) as calculated by the tree gui
        public Rect rowRect { get; set; }
        // the drop rectangle as set by the tree gui when targetted by a drag and drop
        public Rect dropRect { get; set; }

        public TimelineGroupGUI(TreeViewController treeview, TimelineTreeViewGUI treeviewGUI, int id, int depth, TreeViewItem parent, string displayName, TrackAsset trackAsset, bool isRoot)
            : base(id, depth, parent, displayName, trackAsset, treeview, treeviewGUI)
        {
            m_Styles = DirectorStyles.Instance;
            m_IsRoot = isRoot;

            var trackPath = AssetDatabase.GetAssetPath(trackAsset);
            var sequencePath = AssetDatabase.GetAssetPath(treeviewGUI.TimelineWindow.state.editSequence.asset);
            if (trackPath != sequencePath)
                m_IsReferencedTrack = true;

            m_GroupDepth = CalculateGroupDepth(parent);
        }

        public virtual float GetHeight(WindowState state)
        {
            // group tracks don't scale in height
            return TrackEditor.DefaultTrackHeight;
        }

        public override void OnGraphRebuilt() {}

        static int CalculateGroupDepth(TreeViewItem parent)
        {
            int depth = 0;

            bool done = false;
            do
            {
                var gui = parent as TimelineGroupGUI;
                if (gui == null || gui.track == null)
                    done = true;
                else
                {
                    if (gui.track is GroupTrack)
                        depth++;

                    parent = parent.parent;
                }
            }
            while (!done);

            return depth;
        }

        internal static float Spaced(float width)
        {
            return width > 0 ? width + WindowConstants.trackHeaderButtonSpacing : 0;
        }

        private void DrawTrackButtons(Rect headerRect, WindowState state)
        {
            const float buttonSize = WindowConstants.trackHeaderButtonSize;
            const float padding = WindowConstants.trackHeaderButtonPadding;

            var buttonRect = new Rect(headerRect.xMax - buttonSize - padding, headerRect.y + ((headerRect.height - buttonSize) / 2f), buttonSize, buttonSize);

            if (GUI.Button(buttonRect, EditorGUIUtility.IconContent("CreateAddNew"), m_Styles.trackGroupAddButton))
            {
                // the drop down will apply to all selected tracks
                if (!SelectionManager.Contains(track))
                {
                    SelectionManager.Clear();
                    SelectionManager.Add(track);
                }

                SequencerContextMenu.ShowNewTracksContextMenu(SelectionManager.SelectedTracks().ToArray(), TimelineWindow.state);
            }
            buttonRect.x -= buttonSize;

            buttonRect.x -= Spaced(DrawMuteButton(buttonRect, state));
            buttonRect.x -= Spaced(DrawLockButton(buttonRect, state));
        }

        public void SetExpanded(bool expanded)
        {
            var collapseChanged = expanded != isExpanded;
            isExpanded = expanded;
            if (collapseChanged)
            {
                track.SetCollapsed(!expanded);
                m_MustRecomputeUnions = true;
            }
        }

        public override void Draw(Rect headerRect, Rect contentRect, WindowState state)
        {
            if (track == null)
                return;

            if (m_IsRoot)
                return;

            if (m_MustRecomputeUnions)
                RecomputeRectUnions();

            if (depth == 1)
                Graphics.DrawBackgroundRect(state, headerRect);

            var background = headerRect;
            background.height = expandedRect.height;

            var groupColor = TrackResourceCache.GetTrackColor(track);

            m_TreeViewRect = contentRect;

            var col = groupColor;

            var isSelected = SelectionManager.Contains(track);

            if (isSelected)
                col = DirectorStyles.Instance.customSkin.colorSelection;
            else if (isDropTarget)
                col = DirectorStyles.Instance.customSkin.colorDropTarget;
            else
            {
                if (m_GroupDepth % 2 == 1)
                {
                    float h, s, v;
                    Color.RGBToHSV(col, out h, out s, out v);
                    v += 0.06f;
                    col = Color.HSVToRGB(h, s, v);
                }
            }

            // Draw Rounded Rectangle of the group...
            using (new GUIColorOverride(col))
                GUI.Box(background, GUIContent.none, m_Styles.groupBackground);

            var trackRectBackground = headerRect;
            trackRectBackground.xMin += background.width;
            trackRectBackground.width = contentRect.width;
            trackRectBackground.height = background.height;

            if (isSelected)
            {
                col = state.IsEditingASubTimeline()
                    ? m_Styles.customSkin.colorTrackSubSequenceBackgroundSelected
                    : m_Styles.customSkin.colorTrackBackgroundSelected;
            }
            else
            {
                col = m_Styles.customSkin.colorGroupTrackBackground;
            }

            EditorGUI.DrawRect(trackRectBackground, col);
            if (!isExpanded && children != null && children.Count > 0)
            {
                var collapsedTrackRect = contentRect;

                foreach (var u in m_Unions)
                    u.Draw(collapsedTrackRect, state);
            }

            // Draw the name of the Group...
            var labelRect = headerRect;
            labelRect.xMin += 20;
            var actorName = track != null ? track.name : "missing";
            labelRect.width = m_Styles.groupFont.CalcSize(new GUIContent(actorName)).x;
            labelRect.width = Math.Max(labelRect.width, 50.0f);

            // if we aren't bound to anything, we show a text field that allows to rename the actor
            // otherwise we show a ObjectField to allow binding to a go
            if (track != null && track is GroupTrack)
            {
                var textColor = m_Styles.groupFont.normal.textColor;

                if (isSelected)
                    textColor = Color.white;

                string newName;

                EditorGUI.BeginChangeCheck();
                using (new StyleNormalColorOverride(m_Styles.groupFont, textColor))
                {
                    newName = EditorGUI.DelayedTextField(labelRect, GUIContent.none, track.GetInstanceID(), track.name, m_Styles.groupFont);
                }

                if (EditorGUI.EndChangeCheck() && !string.IsNullOrEmpty(newName))
                {
                    track.name = newName;
                    displayName = track.name;
                }
            }

            DrawTrackButtons(headerRect, state);

            if (IsTrackRecording(state))
            {
                using (new GUIColorOverride(DirectorStyles.Instance.customSkin.colorTrackBackgroundRecording))
                    GUI.Label(background, GUIContent.none, m_Styles.displayBackground);
            }

            // is this a referenced track?
            if (m_IsReferencedTrack)
            {
                var refRect = contentRect;
                refRect.x = state.timeAreaRect.xMax - 20.0f;
                refRect.y += 5.0f;
                refRect.width = 30.0f;
                GUI.Label(refRect, DirectorStyles.referenceTrackLabel, EditorStyles.label);
            }

            Rect bgRect = contentRect;
            if (track as GroupTrack != null || AllChildrenMuted(this))
                bgRect.height = expandedRect.height;
            DrawTrackState(contentRect, bgRect, track);
        }

        protected bool IsSubTrack()
        {
            if (track == null)
                return false;

            var parentTrack = track.parent as TrackAsset;
            if (parentTrack == null)
                return false;

            return parentTrack.GetType() != typeof(GroupTrack);
        }

        protected TrackAsset ParentTrack()
        {
            if (IsSubTrack())
                return track.parent as TrackAsset;
            return null;
        }

        // is there currently a recording track
        bool IsTrackRecording(WindowState state)
        {
            if (!state.recording)
                return false;
            if (track.GetType() != typeof(GroupTrack))
                return false;

            return state.GetArmedTrack(track) != null;
        }

        void RecomputeRectUnions()
        {
            m_MustRecomputeUnions = false;
            m_Unions.Clear();
            if (children == null)
                return;

            foreach (var c in children.OfType<TimelineTrackGUI>())
            {
                c.RebuildGUICacheIfNecessary();
                m_Unions.AddRange(TimelineClipUnion.Build(c.clips));
            }
        }

        bool AllChildrenMuted(TimelineGroupGUI groupGui)
        {
            if (!groupGui.track.muted)
                return false;
            if (groupGui.children == null)
                return true;
            return groupGui.children.OfType<TimelineGroupGUI>().All(AllChildrenMuted);
        }
    }
}
