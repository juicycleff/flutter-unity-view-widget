using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    [CustomEditor(typeof(Marker), true)]
    [CanEditMultipleObjects]
    class MarkerInspector : BasicAssetInspector
    {
        static class Styles
        {
            public static readonly string MultipleMarkerSelectionTitle = L10n.Tr("{0} Markers");
            public static readonly string UndoCommand = L10n.Tr("Rename marker");
        }

        internal override bool IsEnabled()
        {
            if (!TimelineUtility.IsCurrentSequenceValid() || IsCurrentSequenceReadOnly())
                return false;
            var marker = target as Marker;
            if (marker != null)
            {
                if (!marker.parent.GetShowMarkers())
                    return false;
            }
            return base.IsEnabled();
        }

        internal override void OnHeaderTitleGUI(Rect titleRect, string header)
        {
            if (targets.Length > 1)
            {
                var multiSelectTitle = string.Format(Styles.MultipleMarkerSelectionTitle, targets.Length);
                base.OnHeaderTitleGUI(titleRect, multiSelectTitle);
                return;
            }

            var marker = target as Marker;
            if (marker != null)
            {
                if (marker.parent.GetShowMarkers() && TimelineUtility.IsCurrentSequenceValid() && !IsCurrentSequenceReadOnly())
                {
                    EditorGUI.BeginChangeCheck();
                    var newName = EditorGUI.DelayedTextField(titleRect, marker.name);
                    if (EditorGUI.EndChangeCheck())
                    {
                        TimelineUndo.PushUndo(marker, Styles.UndoCommand);
                        marker.name = newName;
                    }
                }
                else
                {
                    base.OnHeaderTitleGUI(titleRect, marker.name);
                }
            }
            else
            {
                var typeName = TypeUtility.GetDisplayName(target.GetType());
                EditorGUILayout.LabelField(typeName);
            }
        }

        static bool IsCurrentSequenceReadOnly()
        {
            return TimelineWindow.instance.state.editSequence.isReadOnly;
        }
    }
}
