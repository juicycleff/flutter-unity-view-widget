using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Object = UnityEngine.Object;

namespace UnityEditor.Timeline
{
    class TimelineMarkerHeaderContextMenu : Manipulator
    {
        protected override bool ContextClick(Event evt, WindowState state)
        {
            if (!state.showMarkerHeader)
                return false;

            if (!(state.GetWindow().markerHeaderRect.Contains(evt.mousePosition)
                  || state.GetWindow().markerContentRect.Contains(evt.mousePosition)))
                return false;

            SequencerContextMenu.ShowMarkerHeaderContextMenu(evt.mousePosition, state);
            return true;
        }
    }
}
