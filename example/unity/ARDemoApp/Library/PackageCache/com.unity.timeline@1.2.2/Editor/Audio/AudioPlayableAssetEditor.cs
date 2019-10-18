using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    [CustomTimelineEditor(typeof(AudioPlayableAsset)), UsedImplicitly]
    class AudioPlayableAssetEditor : ClipEditor
    {
        readonly string k_NoClipAssignedError = LocalizationDatabase.GetLocalizedString("No audio clip assigned");
        readonly Dictionary<TimelineClip, WaveformPreview> m_PersistentPreviews = new Dictionary<TimelineClip, WaveformPreview>();
        ColorSpace m_ColorSpace = ColorSpace.Uninitialized;

        /// <inheritdoc/>
        public override ClipDrawOptions GetClipOptions(TimelineClip clip)
        {
            var clipOptions = base.GetClipOptions(clip);
            var audioAsset = clip.asset as AudioPlayableAsset;
            if (audioAsset != null && audioAsset.clip == null)
                clipOptions.errorText = k_NoClipAssignedError;
            return clipOptions;
        }

        /// <inheritdoc/>
        public override void DrawBackground(TimelineClip clip, ClipBackgroundRegion region)
        {
            if (!TimelineWindow.instance.state.showAudioWaveform)
                return;

            var rect = region.position;
            if (rect.width <= 0)
                return;

            var audioClip = clip.asset as AudioClip;
            if (audioClip == null)
            {
                var audioPlayableAsset = clip.asset as AudioPlayableAsset;
                if (audioPlayableAsset != null)
                    audioClip = audioPlayableAsset.clip;
            }

            if (audioClip == null)
                return;

            var quantizedRect = new Rect(Mathf.Ceil(rect.x), Mathf.Ceil(rect.y), Mathf.Ceil(rect.width), Mathf.Ceil(rect.height));
            WaveformPreview preview;

            if (QualitySettings.activeColorSpace != m_ColorSpace)
            {
                m_ColorSpace = QualitySettings.activeColorSpace;
                m_PersistentPreviews.Clear();
            }

            if (!m_PersistentPreviews.TryGetValue(clip, out preview) || audioClip != preview.presentedObject)
            {
                preview = m_PersistentPreviews[clip] = WaveformPreviewFactory.Create((int)quantizedRect.width, audioClip);
                Color waveColour = GammaCorrect(DirectorStyles.Instance.customSkin.colorAudioWaveform);
                Color transparent = waveColour;
                transparent.a = 0;
                preview.backgroundColor = transparent;
                preview.waveColor = waveColour;
                preview.SetChannelMode(WaveformPreview.ChannelMode.MonoSum);
                preview.updated += () => TimelineEditor.Refresh(RefreshReason.WindowNeedsRedraw);
            }

            preview.looping = clip.SupportsLooping();
            preview.SetTimeInfo(region.startTime, region.endTime - region.startTime);
            preview.OptimizeForSize(quantizedRect.size);

            if (Event.current.type == EventType.Repaint)
            {
                preview.ApplyModifications();
                preview.Render(quantizedRect);
            }
        }

        static Color GammaCorrect(Color color)
        {
            return (QualitySettings.activeColorSpace == ColorSpace.Linear) ? color.gamma : color;
        }
    }
}
