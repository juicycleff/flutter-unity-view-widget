using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    partial class TimelineWindow
    {
        [SerializeField]
        SequencePath m_SequencePath;

        void OnSelectionChange()
        {
            RefreshSelection(false);
        }

        void RefreshSelection(bool forceRebuild)
        {
            // if we're in Locked mode, keep current selection - don't use locked property because the
            // sequence hierarchy may need to be rebuilt and it assumes no asset == unlocked
            if (m_LockTracker.isLocked || (state != null && state.recording))
            {
                RestoreLastSelection(forceRebuild);
                return;
            }

            // selection is a TimelineAsset
            Object selectedObject = Selection.activeObject as TimelineAsset;
            if (selectedObject != null)
            {
                SetCurrentSelection(Selection.activeObject);
                return;
            }

            // selection is a GameObject, or a prefab with a director
            var selectedGO = Selection.activeGameObject;
            if (selectedGO != null)
            {
                bool isSceneObject = !PrefabUtility.IsPartOfPrefabAsset(selectedGO);
                bool hasDirector = selectedGO.GetComponent<PlayableDirector>() != null;
                if (isSceneObject || hasDirector)
                {
                    SetCurrentSelection(selectedGO);
                    return;
                }
            }

            // otherwise, keep the same selection.
            RestoreLastSelection(forceRebuild);
        }

        void RestoreLastSelection(bool forceRebuild)
        {
            state.SetCurrentSequencePath(m_SequencePath, forceRebuild);
        }

        void SetCurrentSelection(Object obj)
        {
            var selectedGameObject = obj as GameObject;
            if (selectedGameObject != null)
            {
                PlayableDirector director = TimelineUtility.GetDirectorComponentForGameObject(selectedGameObject);
                SetCurrentTimeline(director);
            }
            else
            {
                var selectedSequenceAsset = obj as TimelineAsset;
                if (selectedSequenceAsset != null)
                {
                    SetCurrentTimeline(selectedSequenceAsset);
                }
            }

            Repaint();
        }
    }
}
