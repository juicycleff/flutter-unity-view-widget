using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    partial class TimelineWindow
    {
        private List<BreadCrumbTitle> m_BreadCrumbLabels = new List<BreadCrumbTitle>(100);

        // Class used for uniquely format names used in the GenericMenu. We can't add duplicate MenuItem in GenericMenu
        // so that's why we need to keep informations about the text we want to uniquely format.
        class SequenceMenuNameFormater
        {
            Dictionary<int, int> m_UniqueItem = new Dictionary<int, int>();

            public string Format(string text)
            {
                int key = text.GetHashCode();
                int index = 0;

                if (m_UniqueItem.ContainsKey(key))
                {
                    index = m_UniqueItem[key];
                    index++;
                    m_UniqueItem[key] = index;
                }
                else
                {
                    m_UniqueItem.Add(key, index);
                    return text;
                }

                return string.Format("{0}{1}", text, index);
            }
        }


        static TitleMode GetTitleMode(ISequenceState sequence)
        {
            var prefabStage = UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
            // Top level
            if (sequence.hostClip == null)
            {
                if (sequence.director != null && prefabStage != null && prefabStage.IsPartOfPrefabContents(sequence.director.gameObject))
                    return TitleMode.Prefab;
                if (sequence.director != null && PrefabUtility.IsPartOfPrefabAsset(sequence.director))
                    return TitleMode.PrefabOutOfContext;
                if (sequence.director != null && !sequence.director.isActiveAndEnabled)
                    return TitleMode.DisabledComponent;
                if (sequence.director != null)
                    return TitleMode.GameObject;
                if (sequence.asset != null)
                    return TitleMode.Asset;
            }
            // Subtimelines only get an error icon
            else if (sequence.director != null && !sequence.director.isActiveAndEnabled && !PrefabUtility.IsPartOfPrefabAsset(sequence.director))
                return TitleMode.DisabledComponent;

            return TitleMode.None;
        }

        void DoBreadcrumbGUI()
        {
            if (state == null)
                return;
            int count = 0;
            foreach (var sequence in state.GetAllSequences())
            {
                BreadCrumbTitle title = new BreadCrumbTitle()
                {
                    name = DisplayNameHelper.GetDisplayName(sequence),
                    mode = GetTitleMode(sequence)
                };
                if (count >= m_BreadCrumbLabels.Count)
                    m_BreadCrumbLabels.Add(title);
                else
                    m_BreadCrumbLabels[count] = title;
                count++;
            }

            if (m_BreadCrumbLabels.Count > count)
                m_BreadCrumbLabels.RemoveRange(count, m_BreadCrumbLabels.Count - count);

            using (new EditorGUI.DisabledScope(currentMode.headerState.breadCrumb == TimelineModeGUIState.Disabled))
            {
                BreadcrumbDrawer.Draw(breadCrumbAreaWidth, m_BreadCrumbLabels, NavigateToBreadcrumbIndex);
            }
        }

        void NavigateToBreadcrumbIndex(int index)
        {
            state.PopSequencesUntilCount(index + 1);
        }

        void DoSequenceSelectorGUI()
        {
            using (new EditorGUI.DisabledScope(currentMode.headerState.sequenceSelector == TimelineModeGUIState.Disabled))
            {
                if (GUILayout.Button(DirectorStyles.sequenceSelectorIcon, EditorStyles.toolbarPopup, GUILayout.Width(WindowConstants.selectorWidth)))
                {
                    var allDirectors = TimelineUtility.GetDirectorsInSceneUsingAsset(null);

                    GenericMenu sequenceMenu = new GenericMenu();
                    SequenceMenuNameFormater formater = new SequenceMenuNameFormater();

                    foreach (var d in allDirectors)
                    {
                        if (d.playableAsset is TimelineAsset)
                        {
                            string text = formater.Format(DisplayNameHelper.GetDisplayName(d));
                            bool isCurrent = state.masterSequence.director == d;
                            sequenceMenu.AddItem(new GUIContent(text), isCurrent, arg =>
                            {
                                var directorToBindTo = (PlayableDirector)arg;
                                if (directorToBindTo)
                                {
                                    // don't just select the object, it may already be selected.
                                    SetCurrentTimeline(directorToBindTo);
                                }
                            }, d);
                        }
                    }

                    if (allDirectors.Length == 0)
                        sequenceMenu.AddDisabledItem(DirectorStyles.noTimelinesInScene);

                    sequenceMenu.ShowAsContext();
                }

                GUILayout.Space(10f);
            }
        }
    }
}
