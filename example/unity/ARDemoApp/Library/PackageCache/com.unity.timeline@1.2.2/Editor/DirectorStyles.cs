using UnityEditor.Experimental;
using UnityEditor.StyleSheets;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    class DirectorStyles
    {
        const string k_Elipsis = "…";
        const string k_ImagePath = "Packages/com.unity.timeline/Editor/StyleSheets/Images/Icons/{0}.png";

        //Timeline resources
        public static readonly GUIContent referenceTrackLabel = TrTextContent("R", "This track references an external asset");
        public static readonly GUIContent recordingLabel = TrTextContent("Recording...");
        public static readonly GUIContent sequenceSelectorIcon = IconContent("TimelineSelector");
        public static readonly GUIContent noTimelineAssetSelected = TrTextContent("To start creating a timeline, select a GameObject");
        public static readonly GUIContent createTimelineOnSelection = TrTextContent("To begin a new timeline with {0}, create {1}");
        public static readonly GUIContent noTimelinesInScene = TrTextContent("No timeline found in the scene");
        public static readonly GUIContent createNewTimelineText = TrTextContent("Create a new Timeline and Director Component for Game Object");
        public static readonly GUIContent previewContent = TrTextContent("Preview", "Enable/disable scene preview mode");
        public static readonly GUIContent mixOff = TrIconContent("TimelineEditModeMixOFF", "Mix Mode (1)");
        public static readonly GUIContent mixOn = TrIconContent("TimelineEditModeMixON", "Mix Mode (1)");
        public static readonly GUIContent rippleOff = TrIconContent("TimelineEditModeRippleOFF", "Ripple Mode (2)");
        public static readonly GUIContent rippleOn = TrIconContent("TimelineEditModeRippleON", "Ripple Mode (2)");
        public static readonly GUIContent replaceOff = TrIconContent("TimelineEditModeReplaceOFF", "Replace Mode (3)");
        public static readonly GUIContent replaceOn = TrIconContent("TimelineEditModeReplaceON", "Replace Mode (3)");
        public static readonly GUIContent showMarkersOn = TrIconContent("TimelineMarkerAreaButtonEnabled", "Show / Hide Timeline Markers");
        public static readonly GUIContent showMarkersOff = TrIconContent("TimelineMarkerAreaButtonDisabled", "Show / Hide Timeline Markers");
        public static readonly GUIContent showMarkersOnTimeline = TrTextContent("Show markers");
        public static readonly GUIContent timelineMarkerTrackHeader = TrTextContentWithIcon("Markers", string.Empty, "TimelineHeaderMarkerIcon");
        public static readonly GUIContent markerCollapseButton = TrTextContent(string.Empty, "Expand / Collapse Track Markers");
        public static readonly GUIContent signalTrackIcon = IconContent("TimelineSignal");

        //Unity Default Resources
        public static readonly GUIContent playContent = EditorGUIUtility.TrIconContent("Animation.Play", "Play the timeline (Space)");
        public static readonly GUIContent gotoBeginingContent = EditorGUIUtility.TrIconContent("Animation.FirstKey", "Go to the beginning of the timeline (Shift+<)");
        public static readonly GUIContent gotoEndContent = EditorGUIUtility.TrIconContent("Animation.LastKey", "Go to the end of the timeline (Shift+>)");
        public static readonly GUIContent nextFrameContent = EditorGUIUtility.TrIconContent("Animation.NextKey", "Go to the next frame");
        public static readonly GUIContent previousFrameContent = EditorGUIUtility.TrIconContent("Animation.PrevKey", "Go to the previous frame");
        public static readonly GUIContent newContent = EditorGUIUtility.IconContent("CreateAddNew", "Add new tracks.");
        public static readonly GUIContent cogIcon = EditorGUIUtility.IconContent("_Popup");
        public static readonly GUIContent animationTrackIcon = EditorGUIUtility.IconContent("AnimationClip Icon");
        public static readonly GUIContent audioTrackIcon = EditorGUIUtility.IconContent("AudioSource Icon");
        public static readonly GUIContent playableTrackIcon = EditorGUIUtility.IconContent("cs Script Icon");

        public GUIContent playrangeContent;

        public static readonly float kBaseIndent = 15.0f;
        public static readonly float kDurationGuiThickness = 5.0f;

        // matches dark skin warning color.
        public static readonly Color kClipErrorColor = new Color(0.957f, 0.737f, 0.008f, 1f);

        // TODO: Make skinnable? If we do, we should probably also make the associated cursors skinnable...
        public static readonly Color kMixToolColor = Color.white;
        public static readonly Color kRippleToolColor = new Color(255f / 255f, 210f / 255f, 51f / 255f);
        public static readonly Color kReplaceToolColor = new Color(165f / 255f, 30f / 255f, 30f / 255f);

        public const string markerDefaultStyle = "MarkerItem";

        public GUIStyle groupBackground;
        public GUIStyle displayBackground;
        public GUIStyle fontClip;
        public GUIStyle fontClipLoop;
        public GUIStyle trackHeaderFont;
        public GUIStyle trackGroupAddButton;
        public GUIStyle groupFont;
        public GUIStyle timeCursor;
        public GUIStyle endmarker;
        public GUIStyle tinyFont;
        public GUIStyle foldout;
        public GUIStyle mute;
        public GUIStyle locked;
        public GUIStyle autoKey;
        public GUIStyle playTimeRangeStart;
        public GUIStyle playTimeRangeEnd;
        public GUIStyle selectedStyle;
        public GUIStyle trackSwatchStyle;
        public GUIStyle connector;
        public GUIStyle keyframe;
        public GUIStyle warning;
        public GUIStyle extrapolationHold;
        public GUIStyle extrapolationLoop;
        public GUIStyle extrapolationPingPong;
        public GUIStyle extrapolationContinue;
        public GUIStyle collapseMarkers;
        public GUIStyle markerMultiOverlay;
        public GUIStyle timelineClip;
        public GUIStyle timelineClipSelected;
        public GUIStyle bottomShadow;
        public GUIStyle trackOptions;
        public GUIStyle infiniteTrack;
        public GUIStyle blendMixIn;
        public GUIStyle blendMixOut;
        public GUIStyle blendEaseIn;
        public GUIStyle blendEaseOut;
        public GUIStyle clipOut;
        public GUIStyle clipIn;
        public GUIStyle curves;
        public GUIStyle lockedBG;
        public GUIStyle activation;
        public GUIStyle playrange;
        public GUIStyle lockButton;
        public GUIStyle avatarMaskOn;
        public GUIStyle avatarMaskOff;
        public GUIStyle markerWarning;
        public GUIStyle editModeMixBtn;
        public GUIStyle editModeRippleBtn;
        public GUIStyle editModeReplaceBtn;
        public GUIStyle showMarkersBtn;

        static internal DirectorStyles s_Instance;

        DirectorNamedColor m_DarkSkinColors;
        DirectorNamedColor m_LightSkinColors;
        DirectorNamedColor m_DefaultSkinColors;

        static readonly string s_DarkSkinPath = "Editors/TimelineWindow/Timeline_DarkSkin.txt";
        static readonly string s_LightSkinPath = "Editors/TimelineWindow/Timeline_LightSkin.txt";

        static readonly GUIContent s_TempContent = new GUIContent();

        public static bool IsInitialized
        {
            get { return s_Instance != null; }
        }

        public static DirectorStyles Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = new DirectorStyles();
                    s_Instance.Initialize();
                }

                return s_Instance;
            }
        }

        public static void ReloadStylesIfNeeded()
        {
            if (Instance.ShouldLoadStyles())
            {
                Instance.LoadStyles();
                if (!Instance.ShouldLoadStyles())
                    Instance.Initialize();
            }
        }

        public DirectorNamedColor customSkin
        {
            get { return EditorGUIUtility.isProSkin ? m_DarkSkinColors : m_LightSkinColors; }
            internal set
            {
                if (EditorGUIUtility.isProSkin)
                    m_DarkSkinColors = value;
                else
                    m_LightSkinColors = value;
            }
        }

        DirectorNamedColor LoadColorSkin(string path)
        {
            var asset = EditorGUIUtility.LoadRequired(path) as TextAsset;

            if (asset != null && !string.IsNullOrEmpty(asset.text))
            {
                return DirectorNamedColor.CreateAndLoadFromText(asset.text);
            }

            return m_DefaultSkinColors;
        }

        static DirectorNamedColor CreateDefaultSkin()
        {
            var nc = ScriptableObject.CreateInstance<DirectorNamedColor>();
            nc.SetDefault();
            return nc;
        }

        public void ExportSkinToFile()
        {
            if (customSkin == m_DarkSkinColors)
                customSkin.ToText(s_DarkSkinPath);

            if (customSkin == m_LightSkinColors)
                customSkin.ToText(s_LightSkinPath);
        }

        public void ReloadSkin()
        {
            if (customSkin == m_DarkSkinColors)
            {
                m_DarkSkinColors = LoadColorSkin(s_DarkSkinPath);
            }
            else if (customSkin == m_LightSkinColors)
            {
                m_LightSkinColors = LoadColorSkin(s_LightSkinPath);
            }
        }

        public void Initialize()
        {
            m_DefaultSkinColors = CreateDefaultSkin();
            m_DarkSkinColors = LoadColorSkin(s_DarkSkinPath);
            m_LightSkinColors = LoadColorSkin(s_LightSkinPath);

            // add the built in colors (control track uses attribute)
            TrackResourceCache.ClearTrackColorCache();
            TrackResourceCache.SetTrackColor<AnimationTrack>(customSkin.colorAnimation);
            TrackResourceCache.SetTrackColor<PlayableTrack>(Color.white);
            TrackResourceCache.SetTrackColor<AudioTrack>(customSkin.colorAudio);
            TrackResourceCache.SetTrackColor<ActivationTrack>(customSkin.colorActivation);
            TrackResourceCache.SetTrackColor<GroupTrack>(customSkin.colorGroup);
            TrackResourceCache.SetTrackColor<ControlTrack>(customSkin.colorControl);

            // add default icons
            TrackResourceCache.ClearTrackIconCache();
            TrackResourceCache.SetTrackIcon<AnimationTrack>(animationTrackIcon);
            TrackResourceCache.SetTrackIcon<AudioTrack>(audioTrackIcon);
            TrackResourceCache.SetTrackIcon<PlayableTrack>(playableTrackIcon);
            TrackResourceCache.SetTrackIcon<ActivationTrack>(new GUIContent(GetBackgroundImage(activation)));
            TrackResourceCache.SetTrackIcon<SignalTrack>(signalTrackIcon);
        }

        DirectorStyles()
        {
            LoadStyles();
        }

        bool ShouldLoadStyles()
        {
            return endmarker == null ||
                endmarker.name == GUISkin.error.name;
        }

        void LoadStyles()
        {
            endmarker = GetGUIStyle("Icon-Endmarker");
            groupBackground = GetGUIStyle("groupBackground");
            displayBackground = GetGUIStyle("sequenceClip");
            fontClip = GetGUIStyle("Font-Clip");
            trackHeaderFont = GetGUIStyle("sequenceTrackHeaderFont");
            trackGroupAddButton = GetGUIStyle("sequenceTrackGroupAddButton");
            groupFont = GetGUIStyle("sequenceGroupFont");
            timeCursor = GetGUIStyle("Icon-TimeCursor");
            tinyFont = GetGUIStyle("tinyFont");
            foldout = GetGUIStyle("Icon-Foldout");
            mute = GetGUIStyle("Icon-Mute");
            locked = GetGUIStyle("Icon-Locked");
            autoKey = GetGUIStyle("Icon-AutoKey");
            playTimeRangeStart = GetGUIStyle("Icon-PlayAreaStart");
            playTimeRangeEnd = GetGUIStyle("Icon-PlayAreaEnd");
            selectedStyle = GetGUIStyle("Color-Selected");
            trackSwatchStyle = GetGUIStyle("Icon-TrackHeaderSwatch");
            connector = GetGUIStyle("Icon-Connector");
            keyframe = GetGUIStyle("Icon-Keyframe");
            warning = GetGUIStyle("Icon-Warning");
            extrapolationHold = GetGUIStyle("Icon-ExtrapolationHold");
            extrapolationLoop = GetGUIStyle("Icon-ExtrapolationLoop");
            extrapolationPingPong = GetGUIStyle("Icon-ExtrapolationPingPong");
            extrapolationContinue = GetGUIStyle("Icon-ExtrapolationContinue");
            timelineClip = GetGUIStyle("Icon-Clip");
            timelineClipSelected = GetGUIStyle("Icon-ClipSelected");
            bottomShadow = GetGUIStyle("Icon-Shadow");
            trackOptions = GetGUIStyle("Icon-TrackOptions");
            infiniteTrack = GetGUIStyle("Icon-InfiniteTrack");
            blendMixIn = GetGUIStyle("Icon-BlendMixIn");
            blendMixOut = GetGUIStyle("Icon-BlendMixOut");
            blendEaseIn = GetGUIStyle("Icon-BlendEaseIn");
            blendEaseOut = GetGUIStyle("Icon-BlendEaseOut");
            clipOut = GetGUIStyle("Icon-ClipOut");
            clipIn = GetGUIStyle("Icon-ClipIn");
            curves = GetGUIStyle("Icon-Curves");
            lockedBG = GetGUIStyle("Icon-LockedBG");
            activation = GetGUIStyle("Icon-Activation");
            playrange = GetGUIStyle("Icon-Playrange");
            lockButton = GetGUIStyle("IN LockButton");
            avatarMaskOn = GetGUIStyle("Icon-AvatarMaskOn");
            avatarMaskOff = GetGUIStyle("Icon-AvatarMaskOff");
            collapseMarkers = GetGUIStyle("TrackCollapseMarkerButton");
            markerMultiOverlay = GetGUIStyle("MarkerMultiOverlay");
            editModeMixBtn = GetGUIStyle("editModeMixBtn");
            editModeRippleBtn = GetGUIStyle("editModeRippleBtn");
            editModeReplaceBtn = GetGUIStyle("editModeReplaceBtn");
            showMarkersBtn = GetGUIStyle("showMarkerBtn");
            markerWarning = GetGUIStyle("markerWarningOverlay");

            playrangeContent = new GUIContent(GetBackgroundImage(playrange)) { tooltip = "Toggle play range markers." };

            fontClipLoop = new GUIStyle(fontClip) { fontStyle = FontStyle.Bold };
        }

        public static GUIStyle GetGUIStyle(string s)
        {
            return EditorStyles.FromUSS(s);
        }

        public static GUIContent TrIconContent(string iconName, string tooltip = null)
        {
            return EditorGUIUtility.TrIconContent(iconName == null ? null : ResolveIcon(iconName), tooltip);
        }

        public static GUIContent IconContent(string iconName)
        {
            return EditorGUIUtility.IconContent(iconName == null ? null : ResolveIcon(iconName));
        }

        public static GUIContent TrTextContentWithIcon(string text, string tooltip, string iconName)
        {
            return EditorGUIUtility.TrTextContentWithIcon(text, tooltip, iconName == null ? null : ResolveIcon(iconName));
        }

        public static GUIContent TrTextContent(string text, string tooltip = null)
        {
            return EditorGUIUtility.TrTextContent(text, tooltip);
        }

        public static Texture2D LoadIcon(string iconName)
        {
            return EditorGUIUtility.LoadIconRequired(iconName == null ? null : ResolveIcon(iconName));
        }

        static string ResolveIcon(string icon)
        {
            return string.Format(k_ImagePath, icon);
        }

        public static string Elipsify(string label, Rect rect, GUIStyle style)
        {
            var ret = label;

            if (label.Length == 0)
                return ret;

            s_TempContent.text = label;
            float neededWidth = style.CalcSize(s_TempContent).x;

            return Elipsify(label, rect.width, neededWidth);
        }

        public static string Elipsify(string label, float destinationWidth, float neededWidth)
        {
            var ret = label;

            if (label.Length == 0)
                return ret;

            if (destinationWidth < neededWidth)
            {
                float averageWidthOfOneChar = neededWidth / label.Length;
                int floor = Mathf.Max((int)Mathf.Floor(destinationWidth / averageWidthOfOneChar), 0);

                if (floor < k_Elipsis.Length)
                    ret = string.Empty;
                else if (floor == k_Elipsis.Length)
                    ret = k_Elipsis;
                else if (floor < label.Length)
                    ret = label.Substring(0, floor - k_Elipsis.Length) + k_Elipsis;
            }

            return ret;
        }

        public static Texture2D GetBackgroundImage(GUIStyle style, StyleState state = StyleState.normal)
        {
            var blockName = GUIStyleExtensions.StyleNameToBlockName(style.name, false);
            var styleBlock = EditorResources.GetStyle(blockName, state);
            return styleBlock.GetTexture(StyleCatalogKeyword.backgroundImage);
        }
    }
}
