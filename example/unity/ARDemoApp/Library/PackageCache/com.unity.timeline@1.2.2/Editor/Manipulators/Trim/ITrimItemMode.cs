using UnityEngine;

namespace UnityEditor.Timeline
{
    enum TrimEdge
    {
        Start,
        End
    }

    interface ITrimItemMode
    {
        void OnBeforeTrim(ITrimmable item, TrimEdge trimDirection);

        void TrimStart(ITrimmable item, double time);
        void TrimEnd(ITrimmable item, double time, bool affectTimeScale);
    }

    interface ITrimItemDrawer
    {
        void DrawGUI(WindowState state, Rect bounds, Color color, TrimEdge edge);
    }
}
