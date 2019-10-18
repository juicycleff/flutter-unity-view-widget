using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    [CustomEditor(typeof(ActivationTrack))]
    class ActivationTrackInspector : TrackAssetInspector
    {
        static class Styles
        {
            public static readonly GUIContent PostPlaybackStateText = EditorGUIUtility.TrTextContent("Post-playback state");
        }

        SerializedProperty m_PostPlaybackProperty;

        public override void OnInspectorGUI()
        {
            using (new EditorGUI.DisabledScope(IsTrackLocked()))
            {
                serializedObject.Update();

                EditorGUI.BeginChangeCheck();

                if (m_PostPlaybackProperty != null)
                    EditorGUILayout.PropertyField(m_PostPlaybackProperty, Styles.PostPlaybackStateText);

                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    var activationTrack = target as ActivationTrack;
                    if (activationTrack != null)
                        activationTrack.UpdateTrackMode();
                }
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            m_PostPlaybackProperty = serializedObject.FindProperty("m_PostPlaybackState");
        }
    }
}
