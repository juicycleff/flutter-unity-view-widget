using UnityEngine;

namespace UnityEditor.Timeline
{
    class TimelineClipHandle : ILayerable
    {
        Rect m_Rect;
        readonly TimelineClipGUI m_ClipGUI;
        readonly TrimEdge m_TrimDirection;
        readonly LayerZOrder m_ZOrder;

        public Rect boundingRect
        {
            get { return m_ClipGUI.parent.ToWindowSpace(m_Rect); }
        }

        public TrimEdge trimDirection
        {
            get { return m_TrimDirection; }
        }

        public TimelineClipGUI clipGUI
        {
            get { return m_ClipGUI; }
        }

        public LayerZOrder zOrder
        {
            get { return m_ZOrder; }
        }

        public TimelineClipHandle(TimelineClipGUI theClipGUI, TrimEdge trimDirection)
        {
            m_TrimDirection = trimDirection;
            m_ClipGUI = theClipGUI;
            m_ZOrder = theClipGUI.zOrder.ChangeLayer(Layer.ClipHandles);
        }

        public void Draw(Rect clientRect, float width, WindowState state)
        {
            var handleRect = clientRect;
            handleRect.width = width;

            if (m_TrimDirection == TrimEdge.End)
                handleRect.x = clientRect.xMax - width;

            m_Rect = handleRect;
            if (!TimelineWindow.instance.state.editSequence.isReadOnly)
                EditorGUIUtility.AddCursorRect(handleRect, MouseCursor.SplitResizeLeftRight);
            state.spacePartitioner.AddBounds(this, boundingRect);
        }
    }
}
