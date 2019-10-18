using System;
using System.Globalization;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    [CustomEditor(typeof(AudioTrack))]
    [CanEditMultipleObjects]
    class AudioTrackInspector : TrackAssetInspector
    {
        [UsedImplicitly] // Also used by tests
        internal static class Styles
        {
            public const string VolumeControl = "AudioTrackInspector.volume";
            public const string StereoPanControl = "AudioTrackInspector.stereoPan";
            public const string SpatialBlendControl = "AudioTrackInspector.spatialBlend";

            const string k_Indent = "    ";
            public const string valuesFormatter = "0.###";
            public const string mixInfoSectionSeparator = "\n\n";
            public static string mixedPropertiesInfo = L10n.Tr("The final {3} is {0}\n" +
                "Calculated from:\n" +
                k_Indent + "Track: {1}\n" +
                k_Indent + "AudioSource: {2}");
        }

        static StringBuilder s_MixInfoBuilder = new StringBuilder();

        SerializedProperty m_VolumeProperty;
        SerializedProperty m_StereoPanProperty;
        SerializedProperty m_SpatialBlendProperty;
        PlayableDirector m_Director;

        public override void OnEnable()
        {
            base.OnEnable();

            if (((AudioTrack)target).timelineAsset == TimelineEditor.inspectedAsset)
                m_Director = TimelineEditor.inspectedDirector;

            m_VolumeProperty = serializedObject.FindProperty("m_TrackProperties.volume");
            m_StereoPanProperty = serializedObject.FindProperty("m_TrackProperties.stereoPan");
            m_SpatialBlendProperty = serializedObject.FindProperty("m_TrackProperties.spatialBlend");
        }

        protected override void DrawTrackProperties()
        {
            // Volume
            GUI.SetNextControlName(Styles.VolumeControl);
            EditorGUILayout.Slider(m_VolumeProperty, 0.0f, 1.0f, AudioSourceInspector.Styles.volumeLabel);
            EditorGUILayout.Space();

            // Stereo Pan
            GUI.SetNextControlName(Styles.StereoPanControl);
            EditorGUIUtility.sliderLabels.SetLabels(AudioSourceInspector.Styles.panLeftLabel, AudioSourceInspector.Styles.panRightLabel);
            EditorGUILayout.Slider(m_StereoPanProperty, -1.0f, 1.0f, AudioSourceInspector.Styles.panStereoLabel);
            EditorGUIUtility.sliderLabels.SetLabels(null, null);
            EditorGUILayout.Space();

            // Spatial Blend
            using (new EditorGUI.DisabledScope(ShouldDisableSpatialBlend()))
            {
                GUI.SetNextControlName(Styles.SpatialBlendControl);
                EditorGUIUtility.sliderLabels.SetLabels(AudioSourceInspector.Styles.spatialLeftLabel, AudioSourceInspector.Styles.spatialRightLabel);
                EditorGUILayout.Slider(m_SpatialBlendProperty, 0.0f, 1.0f, AudioSourceInspector.Styles.spatialBlendLabel);
                EditorGUIUtility.sliderLabels.SetLabels(null, null);
            }

            DrawMixInfoSection();
        }

        void DrawMixInfoSection()
        {
            if (m_Director == null || targets.Length > 1)
                return;

            var binding = m_Director.GetGenericBinding(target) as AudioSource;
            if (binding == null)
                return;

            var audioSourceVolume = binding.volume;
            var audioSourcePan = binding.panStereo;
            var audioSourceBlend = binding.spatialBlend;

            var trackVolume = m_VolumeProperty.floatValue;
            var trackPan = m_StereoPanProperty.floatValue;
            var trackBlend = m_SpatialBlendProperty.floatValue;

            // Skip sections when result is obvious

            var skipVolumeInfo = Math.Abs(audioSourceVolume) < float.Epsilon && Math.Abs(trackVolume) < float.Epsilon ||            // All muted
                Math.Abs(audioSourceVolume - 1) < float.Epsilon && Math.Abs(trackVolume - 1) < float.Epsilon;                       // All max volume

            var skipPanInfo = Math.Abs(audioSourcePan) < float.Epsilon && Math.Abs(trackPan) < float.Epsilon ||                     // All centered
                Math.Abs(audioSourcePan - 1) < float.Epsilon && Math.Abs(trackPan - 1) < float.Epsilon ||                           // All right
                Math.Abs(audioSourcePan - (-1.0f)) < float.Epsilon && Math.Abs(trackPan - (-1.0f)) < float.Epsilon;                 // All left

            var skipBlendInfo = Math.Abs(audioSourceBlend) < float.Epsilon && Math.Abs(trackBlend) < float.Epsilon ||               // All 2D
                Math.Abs(audioSourceBlend - 1) < float.Epsilon && Math.Abs(trackBlend - 1) < float.Epsilon;                         // All 3D

            if (skipVolumeInfo && skipPanInfo && skipBlendInfo)
                return;

            s_MixInfoBuilder.Length = 0;

            if (!skipVolumeInfo)
                s_MixInfoBuilder.AppendFormat(
                    Styles.mixedPropertiesInfo,
                    (audioSourceVolume * trackVolume).ToString(Styles.valuesFormatter, CultureInfo.InvariantCulture),
                    trackVolume.ToString(Styles.valuesFormatter, CultureInfo.InvariantCulture),
                    audioSourceVolume.ToString(Styles.valuesFormatter, CultureInfo.InvariantCulture),
                    AudioSourceInspector.Styles.volumeLabel.text);

            if (!skipVolumeInfo && !skipPanInfo)
                s_MixInfoBuilder.Append(Styles.mixInfoSectionSeparator);

            if (!skipPanInfo)
                s_MixInfoBuilder.AppendFormat(
                    Styles.mixedPropertiesInfo,
                    Mathf.Clamp(audioSourcePan + trackPan, -1.0f, 1.0f).ToString(Styles.valuesFormatter, CultureInfo.InvariantCulture),
                    trackPan.ToString(Styles.valuesFormatter, CultureInfo.InvariantCulture),
                    audioSourcePan.ToString(Styles.valuesFormatter, CultureInfo.InvariantCulture),
                    AudioSourceInspector.Styles.panStereoLabel.text);

            if ((!skipVolumeInfo || !skipPanInfo) && !skipBlendInfo)
                s_MixInfoBuilder.Append(Styles.mixInfoSectionSeparator);

            if (!skipBlendInfo)
                s_MixInfoBuilder.AppendFormat(
                    Styles.mixedPropertiesInfo,
                    Mathf.Clamp01(audioSourceBlend + trackBlend).ToString(Styles.valuesFormatter, CultureInfo.InvariantCulture),
                    trackBlend.ToString(Styles.valuesFormatter, CultureInfo.InvariantCulture),
                    audioSourceBlend.ToString(Styles.valuesFormatter, CultureInfo.InvariantCulture),
                    AudioSourceInspector.Styles.spatialBlendLabel.text);

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox(new GUIContent(s_MixInfoBuilder.ToString()));
        }

        protected override void ApplyChanges()
        {
            var track = (AudioTrack)target;

            if (TimelineEditor.inspectedAsset != track.timelineAsset || TimelineEditor.inspectedDirector == null)
                return;

            if (TimelineEditor.inspectedDirector.state == PlayState.Playing)
                track.LiveLink();
            else
                TimelineEditor.Refresh(RefreshReason.ContentsModified);
        }

        bool ShouldDisableSpatialBlend()
        {
            return m_Director == null ||
                targets.Any(selectedTrack => m_Director.GetGenericBinding(selectedTrack) == null);
        }
    }
}
