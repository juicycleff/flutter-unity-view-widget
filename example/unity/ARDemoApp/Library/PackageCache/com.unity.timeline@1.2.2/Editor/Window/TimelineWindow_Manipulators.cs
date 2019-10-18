using UnityEngine;

namespace UnityEditor.Timeline
{
    partial class TimelineWindow
    {
        readonly Control m_PreTreeViewControl = new Control();
        readonly Control m_PostTreeViewControl = new Control();

        readonly RectangleSelect m_RectangleSelect = new RectangleSelect();
        readonly RectangleZoom m_RectangleZoom = new RectangleZoom();

        void InitializeManipulators()
        {
            // Order is important!

            // Manipulators that needs to be processed BEFORE the treeView (mainly anything clip related)
            m_PreTreeViewControl.AddManipulator(new TimelinePanManipulator());
            m_PreTreeViewControl.AddManipulator(new InlineCurveResize());
            m_PreTreeViewControl.AddManipulator(new TrackZoom());
            m_PreTreeViewControl.AddManipulator(new Jog());
            m_PreTreeViewControl.AddManipulator(TimelineZoomManipulator.Instance);
            m_PreTreeViewControl.AddManipulator(new ContextMenuManipulator());
            m_PreTreeViewControl.AddManipulator(new TimelineMarkerHeaderContextMenu());

            m_PreTreeViewControl.AddManipulator(new EaseClip());
            m_PreTreeViewControl.AddManipulator(new TrimClip());
            m_PreTreeViewControl.AddManipulator(new SelectAndMoveItem());
            m_PreTreeViewControl.AddManipulator(new TrackDoubleClick());
            m_PreTreeViewControl.AddManipulator(new DrillIntoClip());
            m_PreTreeViewControl.AddManipulator(new ItemActionShortcutManipulator());
            m_PreTreeViewControl.AddManipulator(new InlineCurvesShortcutManipulator());

            // Manipulators that needs to be processed AFTER the treeView or any GUI element able to use event (like inline curves)
            m_PostTreeViewControl.AddManipulator(new TimeAreaContextMenu());
            m_PostTreeViewControl.AddManipulator(new TrackShortcutManipulator());
            m_PostTreeViewControl.AddManipulator(new TimelineShortcutManipulator());
            m_PostTreeViewControl.AddManipulator(new ClearSelection());
        }
    }
}
