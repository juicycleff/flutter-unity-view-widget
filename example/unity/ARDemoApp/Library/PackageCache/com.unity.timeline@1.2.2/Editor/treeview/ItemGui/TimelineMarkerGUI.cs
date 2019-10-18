using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityObject = UnityEngine.Object;

namespace UnityEditor.Timeline
{
    class TimelineMarkerGUI : TimelineItemGUI, ISnappable, IAttractable
    {
        public event Action onStartDrag;

        int m_ProjectedClipHash;
        int m_MarkerHash;
        bool m_Selectable;

        MarkerDrawOptions m_MarkerDrawOptions;
        MarkerEditor m_Editor;

        IMarker marker { get; }

        bool selectable
        {
            get { return m_Selectable; }
        }

        public double time
        {
            get { return marker.time; }
        }

        public override double start
        {
            get { return time; }
        }

        public override double end
        {
            get { return time; }
        }

        public override void Select()
        {
            zOrder = zOrderProvider.Next();
            SelectionManager.Add(marker);
            TimelineWindowViewPrefs.GetTrackViewModelData(parent.asset).markerTimeStamps[m_MarkerHash] = DateTime.UtcNow.Ticks;
        }

        public override bool IsSelected()
        {
            return SelectionManager.Contains(marker);
        }

        public override void Deselect()
        {
            SelectionManager.Remove(marker);
        }

        public override ITimelineItem item
        {
            get { return ItemsUtils.ToItem(marker); }
        }

        IZOrderProvider zOrderProvider { get; }

        public TimelineMarkerGUI(IMarker theMarker, IRowGUI parent, IZOrderProvider provider) : base(parent)
        {
            marker = theMarker;
            m_Selectable = marker.GetType().IsSubclassOf(typeof(UnityObject));

            m_MarkerHash = 0;
            var o = marker as object;
            if (!o.Equals(null))
                m_MarkerHash = o.GetHashCode();

            zOrderProvider = provider;
            zOrder = zOrderProvider.Next();
            ItemToItemGui.Add(marker, this);
            m_Editor = CustomTimelineEditorCache.GetMarkerEditor(theMarker);
        }

        int ComputeDirtyHash()
        {
            return time.GetHashCode();
        }

        static void DrawMarker(Rect drawRect, Type type, bool isSelected, bool isCollapsed, MarkerDrawOptions options)
        {
            if (Event.current.type == EventType.Repaint)
            {
                bool hasError = !string.IsNullOrEmpty(options.errorText);

                var style = StyleManager.UssStyleForType(type);
                style.Draw(drawRect, GUIContent.none, false, false, !isCollapsed, isSelected);

                // case1141836: Use of GUI.Box instead of GUI.Label causes desync in UI controlID
                if (hasError)
                    GUI.Label(drawRect, String.Empty, DirectorStyles.Instance.markerWarning);

                var tooltip = hasError ? options.errorText : options.tooltip;
                if (!string.IsNullOrEmpty(tooltip) && drawRect.Contains(Event.current.mousePosition))
                {
                    GUIStyle.SetMouseTooltip(tooltip, drawRect);
                }
            }
        }

        void UpdateDrawData()
        {
            if (Event.current.type == EventType.Layout)
            {
                try
                {
                    m_MarkerDrawOptions = m_Editor.GetMarkerOptions(marker);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    m_MarkerDrawOptions = CustomTimelineEditorCache.GetDefaultMarkerEditor().GetMarkerOptions(marker);
                }
            }
        }

        public override void Draw(Rect trackRect, bool trackRectChanged, WindowState state)
        {
            UpdateDrawData();

            // compute marker hash
            var currentMarkerHash = ComputeDirtyHash();

            // update the clip projected rectangle on the timeline
            CalculateClipRectangle(trackRect, state, currentMarkerHash, trackRectChanged);

            var isSelected = selectable && SelectionManager.Contains(marker);
            var showMarkers = parent.showMarkers;

            QueueOverlay(treeViewRect, isSelected, !showMarkers);
            DrawMarker(treeViewRect, marker.GetType(), isSelected, !showMarkers, m_MarkerDrawOptions);

            if (Event.current.type == EventType.Repaint && showMarkers && !parent.locked)
                state.spacePartitioner.AddBounds(this, rect);
        }

        public void QueueOverlay(Rect rect, bool isSelected, bool isCollapsed)
        {
            if (Event.current.type == EventType.Repaint && m_Editor.supportsDrawOverlay)
            {
                rect = GUIClip.Unclip(rect);
                TimelineWindow.instance.AddUserOverlay(marker, rect, m_Editor, isCollapsed, isSelected);
            }
        }

        public override void StartDrag()
        {
            if (onStartDrag != null)
                onStartDrag.Invoke();
        }

        void CalculateClipRectangle(Rect trackRect, WindowState state, int projectedClipHash, bool trackRectChanged)
        {
            if (m_ProjectedClipHash == projectedClipHash && !trackRectChanged)
                return;

            m_ProjectedClipHash = projectedClipHash;
            treeViewRect = RectToTimeline(trackRect, state);
        }

        public override Rect RectToTimeline(Rect trackRect, WindowState state)
        {
            var style = StyleManager.UssStyleForType(marker.GetType());
            var width = style.fixedWidth;
            var height = style.fixedHeight;
            var x = ((float)marker.time * state.timeAreaScale.x) + state.timeAreaTranslation.x + trackRect.xMin;
            x -= 0.5f * width;
            return new Rect(x, trackRect.y, width, height);
        }

        public IEnumerable<Edge> SnappableEdgesFor(IAttractable attractable, ManipulateEdges manipulateEdges)
        {
            var edges = new List<Edge>();
            var attractableGUI = attractable as TimelineMarkerGUI;
            var canAddEdges = !(attractableGUI != null && attractableGUI.parent == parent);
            if (canAddEdges)
                edges.Add(new Edge(time));
            return edges;
        }

        public bool ShouldSnapTo(ISnappable snappable)
        {
            return snappable != this;
        }
    }
}
