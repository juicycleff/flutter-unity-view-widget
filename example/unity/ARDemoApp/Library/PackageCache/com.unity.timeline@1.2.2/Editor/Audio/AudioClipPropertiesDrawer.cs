using System;
using System.Globalization;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.Timeline;

namespace UnityEngine.Timeline
{
    [CustomPropertyDrawer(typeof(AudioClipProperties))]
    class AudioClipPropertiesDrawer : PropertyDrawer
    {
        [UsedImplicitly] // Also used by tests
        internal static class Styles
        {
            public const string VolumeControl = "AudioClipPropertiesDrawer.volume";

            const string k_Indent = "    ";
            public const string valuesFormatter = "0.###";
            public static string mixedPropertiesInfo = L10n.Tr("The final {3} is {0}\n" +
                "Calculated from:\n" +
                k_Indent + "Clip: {1}\n" +
                k_Indent + "Track: {2}");

            public static string audioSourceContribution = L10n.Tr(k_Indent + "AudioSource: {0}");
        }

        static StringBuilder s_MixInfoBuilder = new StringBuilder();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var volumeProp = property.FindPropertyRelative("volume");

            GUI.SetNextControlName(Styles.VolumeControl);
            EditorGUI.Slider(position, volumeProp, 0.0f, 1.0f, AudioSourceInspector.Styles.volumeLabel);

            if (TimelineEditor.inspectedDirector == null)
                // Nothing more to do in asset mode
                return;

            var clip = SelectionManager.SelectedClips().FirstOrDefault(c => c.asset == property.serializedObject.targetObject);

            if (clip == null || clip.parentTrack == null)
                return;

            var clipVolume = volumeProp.floatValue;
            var trackVolume = new SerializedObject(clip.parentTrack).FindProperty("m_TrackProperties.volume").floatValue;
            var binding = TimelineEditor.inspectedDirector.GetGenericBinding(clip.parentTrack) as AudioSource;

            if (Math.Abs(clipVolume) < float.Epsilon &&
                Math.Abs(trackVolume) < float.Epsilon &&
                (binding == null || Math.Abs(binding.volume) < float.Epsilon))
                return;

            if (Math.Abs(clipVolume - 1) < float.Epsilon &&
                Math.Abs(trackVolume - 1) < float.Epsilon &&
                (binding == null || Math.Abs(binding.volume - 1) < float.Epsilon))
                return;

            s_MixInfoBuilder.Length = 0;

            var audioSourceVolume = binding == null ? 1.0f : binding.volume;

            s_MixInfoBuilder.AppendFormat(
                Styles.mixedPropertiesInfo,
                (clipVolume * trackVolume * audioSourceVolume).ToString(Styles.valuesFormatter, CultureInfo.InvariantCulture),
                clipVolume.ToString(Styles.valuesFormatter, CultureInfo.InvariantCulture),
                trackVolume.ToString(Styles.valuesFormatter, CultureInfo.InvariantCulture),
                AudioSourceInspector.Styles.volumeLabel.text);

            if (binding != null)
                s_MixInfoBuilder.Append("\n")
                    .AppendFormat(Styles.audioSourceContribution,
                        audioSourceVolume.ToString(Styles.valuesFormatter, CultureInfo.InvariantCulture));

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox(new GUIContent(s_MixInfoBuilder.ToString()));
        }
    }
}
