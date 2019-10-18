using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UnityObject = UnityEngine.Object;

namespace UnityEditor.Timeline
{
    class TimelineMarkerClusterGUI : TimelineItemGUI
    {
        readonly List<TimelineMarkerGUI> m_MarkerGUIs;
        readonly IZOrderProvider m_ZOrderProvider;

        public TimelineMarkerGUI topMarker
        {
            get { return m_MarkerGUIs.LastOrDefault(); }
        }

        TimelineMarkerGUI m_ManipulatedMarker;

        public TimelineMarkerClusterGUI(List<TimelineMarkerGUI> guis, IRowGUI parent,
                                        IZOrderProvider zOrderProvider, LayerZOrder layerZOrder)
            : base(parent)
        {
            m_MarkerGUIs = guis;
            m_ZOrderProvider = zOrderProvider;
            zOrder = layerZOrder;
            SortMarkers();
            topMarker.onStartDrag += OnDragTopMarker;
        }

        public override double start
        {
            get { return topMarker.start; }
        }

        public override double end
        {
            get { return topMarker.end; }
        }

        public override ITimelineItem item
        {
            get { return topMarker.item; }
        }

        public override void Select()
        {
            foreach (var marker in m_MarkerGUIs)
            {
                if (!marker.IsSelected())
                    marker.Select();
            }
        }

        public override void Deselect()
        {
            foreach (var marker in m_MarkerGUIs)
            {
                if (marker.IsSelected())
                    marker.Deselect();
            }
        }

        public override void Draw(Rect trackRect, bool trackRectChanged, WindowState state)
        {
            RegisterRect(state);

            topMarker.Draw(trackRect, trackRectChanged, state);

            if (m_MarkerGUIs.Count > 1)
                GUI.Box(treeViewRect, String.Empty, DirectorStyles.Instance.markerMultiOverlay);

            if (m_ManipulatedMarker != null)
                m_ManipulatedMarker.Draw(trackRect, trackRectChanged, state);
        }

        public override Rect RectToTimeline(Rect trackRect, WindowState state)
        {
            return topMarker.RectToTimeline(trackRect, state);
        }

        public void CycleTop()
        {
            if (m_MarkerGUIs.Count < 2)
                return;

            topMarker.onStartDrag -= OnDragTopMarker;

            var last = topMarker;
            for (int i = 0; i < m_MarkerGUIs.Count; ++i)
            {
                var next = m_MarkerGUIs[i];
                m_MarkerGUIs[i] = last;
                last = next;
            }

            topMarker.zOrder = m_ZOrderProvider.Next();

            topMarker.onStartDrag += OnDragTopMarker;
        }

        void OnDragTopMarker()
        {
            m_ManipulatedMarker = topMarker;
            m_ManipulatedMarker.onStartDrag -= OnDragTopMarker;
            m_MarkerGUIs.RemoveAt(m_MarkerGUIs.Count - 1);
        }

        void SortMarkers()
        {
            m_MarkerGUIs.Sort((lhs, rhs) => lhs.zOrder.CompareTo(rhs.zOrder));
        }

        void RegisterRect(WindowState state)
        {
            treeViewRect = topMarker.treeViewRect;

            if (Event.current.type == EventType.Repaint && !parent.locked)
                state.spacePartitioner.AddBounds(this, rect);
        }

        public static bool CanCycleMarkers()
        {
            if (!SelectionManager.SelectedMarkers().Any())
                return false;

            var cluster = PickerUtils.PickedLayerableOfType<TimelineMarkerClusterGUI>();

            if (cluster == null)
                return false;

            // Only cycle if the marker is selected and nothing else is selected
            return cluster.topMarker.IsSelected() && SelectionManager.Count() == 1;
        }

        public static void CycleMarkers()
        {
            var cluster = PickerUtils.PickedLayerableOfType<TimelineMarkerClusterGUI>();

            if (cluster == null)
                return;

            cluster.topMarker.Deselect();
            cluster.CycleTop();
            cluster.topMarker.Select();
        }
    }
}
