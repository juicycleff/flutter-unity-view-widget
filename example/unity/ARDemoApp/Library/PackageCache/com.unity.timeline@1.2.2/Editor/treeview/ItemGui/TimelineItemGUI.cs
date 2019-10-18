using System;
using System.Collections.Generic;
using UnityEngine.Timeline;
using UnityEngine;

namespace UnityEditor.Timeline
{
    static class ItemToItemGui
    {
        static Dictionary<object, TimelineItemGUI> s_ItemToItemGUI =
            new Dictionary<object, TimelineItemGUI>();

        public static void Add(TimelineClip clip, TimelineItemGUI gui)
        {
            s_ItemToItemGUI[clip] = gui;
        }

        public static void Add(IMarker marker, TimelineItemGUI gui)
        {
            s_ItemToItemGUI[marker] = gui;
        }

        public static TimelineClipGUI GetGuiForClip(TimelineClip clip)
        {
            return GetGuiForItem(clip) as TimelineClipGUI;
        }

        public static TimelineMarkerGUI GetGuiForMarker(IMarker marker)
        {
            return GetGuiForItem(marker) as TimelineMarkerGUI;
        }

        static TimelineItemGUI GetGuiForItem(object item)
        {
            if (item == null)
                return null;

            TimelineItemGUI gui;
            s_ItemToItemGUI.TryGetValue(item, out gui);
            return gui;
        }
    }

    abstract class TimelineItemGUI : ISelectable
    {
        protected readonly DirectorStyles m_Styles;

        public abstract ITimelineItem item { get; }
        public abstract double start { get; }
        public abstract double end { get; }
        public abstract void Draw(Rect rect, bool rectChanged, WindowState state);
        public abstract Rect RectToTimeline(Rect trackRect, WindowState state);

        public virtual void Select() {}
        public virtual bool IsSelected() { return false; }
        public virtual void Deselect() {}

        public virtual void StartDrag() {}
        public virtual void StopDrag() {}

        public LayerZOrder zOrder { get; set; }

        public bool visible { get; set; }
        public bool isInvalid { get; set; }

        public IRowGUI parent { get; }

        public Rect rect
        {
            get { return parent.ToWindowSpace(treeViewRect); }
        }

        public Rect treeViewRect
        {
            get { return m_TreeViewRect; }
            protected set
            {
                m_TreeViewRect = value;
                if (value.width < 0.0f)
                    m_TreeViewRect.width = 1.0f;
            }
        }

        Rect m_TreeViewRect;

        protected TimelineItemGUI(IRowGUI parent)
        {
            this.parent = parent;
            m_Styles = DirectorStyles.Instance;
        }
    }
}
