using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;
using Object = UnityEngine.Object;

namespace UnityEditor.Timeline
{
    class TimelineMarkerHeaderGUI : IRowGUI, ILayerable
    {
        int m_TrackHash;
        TimelineAsset timeline { get; }
        WindowState state { get; }
        MarkersLayer m_Layer;
        LayerZOrder m_ZOrder = new LayerZOrder(Layer.MarkerHeaderTrack, 0);

        struct DrawData
        {
            public Rect headerRect;
            public Rect contentRect;
            public GUIStyle trackSwatchStyle;
            public GUIStyle trackHeaderFont;
            public Color colorTrackFont;
            public bool showLockButton;
            public bool showMuteButton;
        }

        public TimelineMarkerHeaderGUI(TimelineAsset asset, WindowState state)
        {
            m_TrackHash = -1;
            timeline = asset;
            this.state = state;
        }

        public TrackAsset asset { get { return timeline.markerTrack; } }
        public Rect boundingRect { get; private set; }
        public bool locked { get { return !state.showMarkerHeader; } }

        public bool showMarkers
        {
            get { return state.showMarkerHeader; }
        }

        public bool muted
        {
            get { return timeline.markerTrack != null && timeline.markerTrack.muted; }
        }

        Rect IRowGUI.ToWindowSpace(Rect rect)
        {
            //header gui is already in global coordinates
            return rect;
        }

        public void Draw(Rect markerHeaderRect, Rect markerContentRect, WindowState state)
        {
            boundingRect = markerContentRect;
            var data = new DrawData()
            {
                headerRect = markerHeaderRect,
                contentRect = markerContentRect,
                trackSwatchStyle = new GUIStyle(),
                trackHeaderFont = DirectorStyles.Instance.trackHeaderFont,
                colorTrackFont = DirectorStyles.Instance.customSkin.colorTrackFont,
                showLockButton = locked,
                showMuteButton = muted
            };

            if (state.showMarkerHeader)
            {
                DrawMarkerDrawer(data, state);
                if (Event.current.type == EventType.Repaint)
                    state.spacePartitioner.AddBounds(this, boundingRect);
            }

            if (asset != null && Hash() != m_TrackHash)
                Rebuild();

            var rect = state.showMarkerHeader ? markerContentRect : state.timeAreaRect;
            using (new GUIViewportScope(rect))
            {
                if (m_Layer != null)
                    m_Layer.Draw(rect, state);

                HandleDragAndDrop();
            }
        }

        public void Rebuild()
        {
            if (asset == null)
                return;

            m_Layer = new MarkersLayer(Layer.MarkersOnHeader, this);
            m_TrackHash = Hash();
        }

        void HandleDragAndDrop()
        {
            if (TimelineWindow.instance.state.editSequence.isReadOnly)
                return;

            if (Event.current == null || Event.current.type != EventType.DragUpdated &&
                Event.current.type != EventType.DragPerform && Event.current.type != EventType.DragExited)
                return;

            timeline.CreateMarkerTrack(); // Ensure Marker track is created.
            var objectsBeingDropped = DragAndDrop.objectReferences.OfType<Object>();
            var candidateTime = TimelineHelpers.GetCandidateTime(TimelineWindow.instance.state, Event.current.mousePosition);
            var perform = Event.current.type == EventType.DragPerform;
            var director = state.editSequence != null ? state.editSequence.director : null;
            DragAndDrop.visualMode = TimelineDragging.HandleClipPaneObjectDragAndDrop(objectsBeingDropped, timeline.markerTrack, perform,
                timeline, null, director, candidateTime, TimelineDragging.ResolveType);
            if (perform && DragAndDrop.visualMode == DragAndDropVisualMode.Copy)
            {
                DragAndDrop.AcceptDrag();
            }
        }

        int Hash()
        {
            return timeline.markerTrack == null ? 0 : timeline.markerTrack.Hash();
        }

        static void DrawMarkerDrawer(DrawData data, WindowState state)
        {
            DrawMarkerDrawerHeaderBackground(data);
            DrawMarkerDrawerHeader(data, state);
            DrawMarkerDrawerContentBackground(data);
        }

        static void DrawMarkerDrawerHeaderBackground(DrawData data)
        {
            var backgroundColor = DirectorStyles.Instance.customSkin.markerHeaderDrawerBackgroundColor;
            var bgRect = data.headerRect;
            bgRect.x += data.trackSwatchStyle.fixedWidth;
            bgRect.width -= data.trackSwatchStyle.fixedWidth;
            EditorGUI.DrawRect(bgRect, backgroundColor);
        }

        static void DrawMarkerDrawerHeader(DrawData data, WindowState state)
        {
            var textStyle = data.trackHeaderFont;
            textStyle.normal.textColor = data.colorTrackFont;
            var labelRect = data.headerRect;
            labelRect.x += DirectorStyles.kBaseIndent;

            EditorGUI.LabelField(labelRect, DirectorStyles.timelineMarkerTrackHeader);

            const float buttonSize = WindowConstants.trackHeaderButtonSize;
            const float padding = WindowConstants.trackHeaderButtonPadding;
            var x = data.headerRect.xMax - buttonSize - padding - 2f;
            var y = data.headerRect.y + (data.headerRect.height - buttonSize) / 2.0f;
            var buttonRect = new Rect(x, y, buttonSize, buttonSize);

            DrawTrackDropDownMenu(buttonRect, state);
            buttonRect.x -= 16.0f;

            if (data.showMuteButton)
            {
                DrawMuteButton(buttonRect, state);
                buttonRect.x -= 16.0f;
            }

            if (data.showLockButton)
            {
                DrawLockButton(buttonRect, state);
            }
        }

        static void DrawMarkerDrawerContentBackground(DrawData data)
        {
            var trackBackgroundColor = DirectorStyles.Instance.customSkin.markerDrawerBackgroundColor;
            EditorGUI.DrawRect(data.contentRect, trackBackgroundColor);
        }

        static void DrawLockButton(Rect rect, WindowState state)
        {
            if (GUI.Button(rect, GUIContent.none, TimelineWindow.styles.locked))
                TimelineAction.Invoke<ToggleShowMarkersOnTimeline>(state);
        }

        static void DrawTrackDropDownMenu(Rect rect, WindowState state)
        {
            rect.y += WindowConstants.trackOptionButtonVerticalPadding;
            if (GUI.Button(rect, GUIContent.none, DirectorStyles.Instance.trackOptions))
                SequencerContextMenu.ShowMarkerHeaderContextMenu(null, state);
        }

        static void DrawMuteButton(Rect rect, WindowState state)
        {
            if (GUI.Button(rect, GUIContent.none, TimelineWindow.styles.mute))
                TimelineAction.Invoke<ToggleMuteMarkersOnTimeline>(state);
        }

        public LayerZOrder zOrder
        {
            get { return m_ZOrder; }
        }
    }
}
