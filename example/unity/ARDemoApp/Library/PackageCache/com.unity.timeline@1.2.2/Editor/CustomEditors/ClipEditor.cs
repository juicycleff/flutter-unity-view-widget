using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    /// <summary>
    /// Description of the on-screen area where a clip is drawn
    /// </summary>
    public struct ClipBackgroundRegion
    {
        /// <summary>
        /// The rectangle where the background of the clip is drawn.
        /// </summary>
        /// <remarks>
        /// The rectangle is clipped to the screen. The rectangle does not include clip borders.
        /// </remarks>
        public Rect position { get; private set; }

        /// <summary>
        /// The start time of the region, relative to the clip.
        /// </summary>
        public double startTime { get; private set; }

        /// <summary>
        /// The end time of the region, relative to the clip.
        /// </summary>
        public double endTime { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_position"></param>
        /// <param name="_startTime"></param>
        /// <param name="_endTime"></param>
        public ClipBackgroundRegion(Rect _position, double _startTime, double _endTime)
        {
            position = _position;
            startTime = _startTime;
            endTime = _endTime;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ClipBackgroundRegion))
                return false;

            return Equals((ClipBackgroundRegion)obj);
        }

        public bool Equals(ClipBackgroundRegion other)
        {
            return position.Equals(other.position) &&
                startTime == other.startTime &&
                endTime == other.endTime;
        }

        public override int GetHashCode()
        {
            return HashUtility.CombineHash(
                position.GetHashCode(),
                startTime.GetHashCode(),
                endTime.GetHashCode()
            );
        }

        public static bool operator==(ClipBackgroundRegion region1, ClipBackgroundRegion region2)
        {
            return region1.Equals(region2);
        }

        public static bool operator!=(ClipBackgroundRegion region1, ClipBackgroundRegion region2)
        {
            return !region1.Equals(region2);
        }
    }

    /// <summary>
    /// The user-defined options for drawing a clip.
    /// </summary>
    public struct ClipDrawOptions
    {
        private IEnumerable<Texture2D> m_Icons;

        /// <summary>
        /// Text that indicates if the clip should display an error.
        /// </summary>
        /// <remarks>
        /// If the error text is not empty or null, then the clip displays a warning. The error text is used as the tooltip.
        /// </remarks>
        public string errorText { get; set; }

        /// <summary>
        /// The tooltip to show for the clip.
        /// </summary>
        public string tooltip { get; set; }

        /// <summary>
        /// The color drawn under the clip. By default, the color is the same as the track color.
        /// </summary>
        public Color highlightColor { get; set; }


        /// <summary>
        /// Icons to display on the clip.
        /// </summary>
        public IEnumerable<Texture2D> icons
        {
            get { return m_Icons ?? System.Linq.Enumerable.Empty<Texture2D>(); }
            set { m_Icons = value;}
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ClipDrawOptions))
                return false;

            return Equals((ClipDrawOptions)obj);
        }

        public bool Equals(ClipDrawOptions other)
        {
            return errorText == other.errorText &&
                tooltip == other.tooltip &&
                highlightColor == other.highlightColor &&
                System.Linq.Enumerable.SequenceEqual(icons, other.icons);
        }

        public override int GetHashCode()
        {
            return HashUtility.CombineHash(
                errorText != null ? errorText.GetHashCode() : 0,
                tooltip != null ? tooltip.GetHashCode() : 0,
                highlightColor.GetHashCode(),
                icons != null ? icons.GetHashCode() : 0
            );
        }

        public static bool operator==(ClipDrawOptions options1, ClipDrawOptions options2)
        {
            return options1.Equals(options2);
        }

        public static bool operator!=(ClipDrawOptions options1, ClipDrawOptions options2)
        {
            return !options1.Equals(options2);
        }
    }


    /// <summary>
    /// Use this class to customize clip types in the TimelineEditor.
    /// </summary>
    public class ClipEditor
    {
        static readonly string k_NoPlayableAssetError = LocalizationDatabase.GetLocalizedString("This clip does not contain a valid playable asset");
        static readonly string k_ScriptLoadError = LocalizationDatabase.GetLocalizedString("The associated script can not be loaded");

        internal readonly bool supportsSubTimelines;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ClipEditor()
        {
            supportsSubTimelines = TypeUtility.HasOverrideMethod(GetType(), nameof(GetSubTimelines));
        }

        /// <summary>
        /// Implement this method to override the default options for drawing a clip.
        /// </summary>
        /// <param name="clip">The clip being drawn.</param>
        /// <returns>The options for drawing a clip.</returns>
        public virtual ClipDrawOptions GetClipOptions(TimelineClip clip)
        {
            return new ClipDrawOptions()
            {
                errorText = GetErrorText(clip),
                tooltip = string.Empty,
                highlightColor = GetDefaultHighlightColor(clip),
                icons = System.Linq.Enumerable.Empty<Texture2D>()
            };
        }

        /// <summary>
        /// Override this method to draw a background for a clip .
        /// </summary>
        /// <param name="clip">The clip being drawn.</param>
        /// <param name="region">The on-screen area where the clip is drawn.</param>
        public virtual void DrawBackground(TimelineClip clip, ClipBackgroundRegion region)
        {
        }

        /// <summary>
        /// Called when a clip is created.
        /// </summary>
        /// <param name="clip">The newly created clip.</param>
        /// <param name="track">The track that the clip is assigned to.</param>
        /// <param name="clonedFrom">The source that the clip was copied from. This can be set to null if the clip is not a copy.</param>
        /// <remarks>
        /// The callback occurs before the clip is assigned to the track.
        /// </remarks>
        public virtual void OnCreate(TimelineClip clip, TrackAsset track, TimelineClip clonedFrom)
        {
        }

        /// <summary>
        /// Gets the error text for the specified clip.
        /// </summary>
        /// <param name="clip">The clip being drawn.</param>
        /// <returns>Returns the error text to be displayed as the tool tip for the clip. If there is no error to be displayed, this method returns string.Empty.</returns>
        public string GetErrorText(TimelineClip clip)
        {
            if (clip == null || clip.asset == null)
                return k_NoPlayableAssetError;

            var playableAsset = clip.asset as ScriptableObject;
            if (playableAsset == null || MonoScript.FromScriptableObject(playableAsset) == null)
                return k_ScriptLoadError;

            return string.Empty;
        }

        /// <summary>
        /// The color drawn under the clip. By default, the color is the same as the track color.
        /// </summary>
        /// <param name="clip">The clip being drawn.</param>
        /// <returns>Returns the highlight color of the clip being drawn.</returns>
        public Color GetDefaultHighlightColor(TimelineClip clip)
        {
            if (clip == null)
                return Color.white;

            return TrackResourceCache.GetTrackColor(clip.parentTrack);
        }

        /// <summary>
        /// Called when a clip is changed by the Editor.
        /// </summary>
        /// <param name="clip">The clip that changed.</param>
        public virtual void OnClipChanged(TimelineClip clip)
        {
        }

        /// <summary>
        /// Gets the sub-timelines for a specific clip. Implement this method if your clip supports playing nested timelines.
        /// </summary>
        /// <param name="clip">The clip with the ControlPlayableAsset.</param>
        /// <param name="director">The playable director driving the Timeline Clip. This may not be the same as TimelineEditor.inspectedDirector.</param>
        /// <param name="subTimelines">Specify the sub-timelines to control.</param>
        public virtual void GetSubTimelines(TimelineClip clip, PlayableDirector director, List<PlayableDirector> subTimelines)
        {
        }
    }
}
