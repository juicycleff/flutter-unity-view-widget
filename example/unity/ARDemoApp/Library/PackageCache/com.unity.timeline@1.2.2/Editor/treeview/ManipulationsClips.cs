using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    class ItemActionShortcutManipulator : Manipulator
    {
        protected override bool ExecuteCommand(Event evt, WindowState state)
        {
            if (state.IsEditingASubItem())
                return false;

            var consumed = false;
            var clips = SelectionManager.SelectedClips();
            foreach (var clip in clips)
                consumed |= ItemAction<TimelineClip>.HandleShortcut(state, evt, clip);

            var markers = SelectionManager.SelectedMarkers();
            foreach (var marker in markers)
                consumed |= ItemAction<IMarker>.HandleShortcut(state, evt, marker);

            return consumed;
        }
    }

    class DrillIntoClip : Manipulator
    {
        protected override bool DoubleClick(Event evt, WindowState state)
        {
            if (evt.button != 0)
                return false;

            var guiClip = PickerUtils.PickedLayerableOfType<TimelineClipGUI>();

            if (guiClip == null)
                return false;

            if (!TimelineWindow.instance.state.editSequence.isReadOnly && (guiClip.clip.curves != null || guiClip.clip.animationClip != null))
                ItemAction<TimelineClip>.Invoke<EditClipInAnimationWindow>(state, guiClip.clip);

            if (guiClip.supportsSubTimelines)
                ItemAction<TimelineClip>.Invoke<EditSubTimeline>(state, guiClip.clip);

            return true;
        }
    }

    class ContextMenuManipulator : Manipulator
    {
        protected override bool MouseDown(Event evt, WindowState state)
        {
            if (evt.button == 1)
                ItemSelection.HandleSingleSelection(evt);

            return false;
        }

        protected override bool ContextClick(Event evt, WindowState state)
        {
            if (evt.alt)
                return false;

            var selectable = PickerUtils.PickedLayerableOfType<ISelectable>();

            if (selectable != null && selectable.IsSelected())
            {
                SequencerContextMenu.ShowItemContextMenu(evt.mousePosition,
                    SelectionManager.SelectedClips().ToArray(),
                    SelectionManager.SelectedMarkers().ToArray());
                return true;
            }

            return false;
        }
    }
}
