//#define PERF_PROFILE

using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    [CustomEditor(typeof(GroupTrack)), CanEditMultipleObjects]
    class GroupTrackInspector : TrackAssetInspector
    {
        static class Styles
        {
            public static readonly GUIContent GroupSubTrackHeaderName     = EditorGUIUtility.TrTextContent("Name");
            public static readonly GUIContent GroupSubTrackHeaderType     = EditorGUIUtility.TrTextContent("Type");
            public static readonly GUIContent GroupSubTrackHeaderDuration = EditorGUIUtility.TrTextContent("Duration");
            public static readonly GUIContent GroupSubTrackHeaderFrames   = EditorGUIUtility.TrTextContent("Frames");
            public static readonly GUIContent GroupInvalidTrack           = EditorGUIUtility.TrTextContent("Invalid Track");
        }

        ReorderableList m_SubTracks;

        public override void OnInspectorGUI()
        {
            foreach (var group in targets)
            {
                var groupTrack = group as GroupTrack;
                if (groupTrack == null) return;

                var childrenTracks = groupTrack.GetChildTracks();
                var groupTrackName = groupTrack.name;

                GUILayout.Label(childrenTracks.Count() > 0
                    ? groupTrackName + " (" + childrenTracks.Count() + ")"
                    : groupTrackName, EditorStyles.boldLabel);
                GUILayout.Space(3.0f);

                // the subTrackObjects is used because it's the internal list
                m_SubTracks.list = groupTrack.subTracksObjects;
                m_SubTracks.DoLayoutList();
                m_SubTracks.index = -1;
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();

            m_SubTracks = new ReorderableList(new string[] {}, typeof(string), false, true, false, false)
            {
                drawElementCallback = OnDrawSubTrack,
                drawHeaderCallback = OnDrawHeader,
                showDefaultBackground = true,
                index = 0,
                elementHeight = 20
            };
        }

        static void OnDrawHeader(Rect rect)
        {
            int sections = 4;
            float sectionWidth = rect.width / sections;

            rect.width = sectionWidth;
            GUI.Label(rect, Styles.GroupSubTrackHeaderName, EditorStyles.label);
            rect.x += sectionWidth;
            GUI.Label(rect, Styles.GroupSubTrackHeaderType, EditorStyles.label);
            rect.x += sectionWidth;
            GUI.Label(rect, Styles.GroupSubTrackHeaderDuration, EditorStyles.label);
            rect.x += sectionWidth;
            GUI.Label(rect, Styles.GroupSubTrackHeaderFrames, EditorStyles.label);
        }

        void OnDrawSubTrack(Rect rect, int index, bool selected, bool focused)
        {
            int sections = 4;
            float sectionWidth = rect.width / sections;

            var childrenTrack = m_SubTracks.list[index] as TrackAsset;
            if (childrenTrack == null)
            {
                object o = m_SubTracks.list[index];
                rect.width = sectionWidth;
                if (o != null) // track is loaded, but has broken script
                {
                    string name = ((UnityEngine.Object)m_SubTracks.list[index]).name;
                    GUI.Label(rect, name, EditorStyles.label);
                }
                rect.x += sectionWidth;
                using (new GUIColorOverride(DirectorStyles.kClipErrorColor))
                    GUI.Label(rect, Styles.GroupInvalidTrack.text, EditorStyles.label);
                return;
            }

            rect.width = sectionWidth;
            GUI.Label(rect, childrenTrack.name, EditorStyles.label);
            rect.x += sectionWidth;
            GUI.Label(rect, childrenTrack.GetType().Name, EditorStyles.label);
            rect.x += sectionWidth;
            GUI.Label(rect, childrenTrack.duration.ToString(), EditorStyles.label);
            rect.x += sectionWidth;
            double exactFrames = TimeUtility.ToExactFrames(childrenTrack.duration, TimelineWindow.instance.state.referenceSequence.frameRate);
            GUI.Label(rect, exactFrames.ToString(), EditorStyles.label);
        }
    }
}
