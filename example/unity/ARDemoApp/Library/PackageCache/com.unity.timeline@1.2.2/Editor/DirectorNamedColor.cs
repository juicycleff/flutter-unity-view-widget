using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor.Timeline;

namespace UnityEngine.Timeline
{
    [Serializable]
    class DirectorNamedColor : ScriptableObject
    {
        [SerializeField]
        public Color colorPlayhead;

        [SerializeField]
        public Color colorSelection;

        [SerializeField]
        public Color colorEndmarker;

        [SerializeField]
        public Color colorGroup;

        [SerializeField]
        public Color colorGroupTrackBackground;

        [SerializeField]
        public Color colorAnimation;

        [SerializeField]
        public Color colorAnimationRecorded;

        [SerializeField]
        public Color colorAudio;

        [SerializeField]
        public Color colorAudioWaveform;

        [SerializeField]
        public Color colorActivation;

        [SerializeField]
        public Color colorDropTarget;

        [SerializeField]
        public Color colorClipFont;

        [SerializeField]
        public Color colorInvalidClipOverlay;

        [SerializeField]
        public Color colorClipBlendYin;

        [SerializeField]
        public Color colorClipBlendYang;

        [SerializeField]
        public Color colorClipBlendLines;

        [SerializeField]
        public Color colorTrackBackground;

        [SerializeField]
        public Color colorTrackHeaderBackground;

        [SerializeField]
        public Color colorTrackDarken;

        [SerializeField]
        public Color colorTrackBackgroundRecording;

        [SerializeField]
        public Color colorInfiniteTrackBackgroundRecording;

        [SerializeField]
        public Color colorTrackBackgroundSelected;

        [SerializeField]
        public Color colorTrackFont;

        [SerializeField]
        public Color colorClipUnion;

        [SerializeField]
        public Color colorTopOutline3;

        [SerializeField]
        public Color colorDurationLine;

        [SerializeField]
        public Color colorRange;

        [SerializeField]
        public Color colorSequenceBackground;

        [SerializeField]
        public Color colorTooltipBackground;

        [SerializeField]
        public Color colorInfiniteClipLine;

        [SerializeField]
        public Color colorDefaultTrackDrawer;

        [SerializeField]
        public Color colorDuration = new Color(0.66f, 0.66f, 0.66f, 1.0f);

        [SerializeField]
        public Color colorRecordingClipOutline = new Color(1, 0, 0, 0.9f);

        [SerializeField]
        public Color colorAnimEditorBinding = new Color(54.0f / 255.0f, 54.0f / 255.0f, 54.0f / 255.0f);

        [SerializeField]
        public Color colorTimelineBackground = new Color(0.2f, 0.2f, 0.2f, 1.0f);

        [SerializeField]
        public Color colorLockTextBG = Color.red;

        [SerializeField]
        public Color colorInlineCurveVerticalLines = new Color(1.0f, 1.0f, 1.0f, 0.2f);

        [SerializeField]
        public Color colorInlineCurveOutOfRangeOverlay = new Color(0.0f, 0.0f, 0.0f, 0.5f);

        [SerializeField]
        public Color colorInlineCurvesBackground;

        [SerializeField]
        public Color markerDrawerBackgroundColor = new Color(0.4f, 0.4f, 0.4f , 1.0f);

        [SerializeField]
        public Color markerHeaderDrawerBackgroundColor = new Color(0.5f, 0.5f, 0.5f , 1.0f);

        [SerializeField]
        public Color colorControl = new Color(0.2313f, 0.6353f, 0.5843f, 1.0f);

        [SerializeField]
        public Color colorSubSequenceBackground = new Color(0.1294118f, 0.1764706f, 0.1764706f, 1.0f);

        [SerializeField]
        public Color colorTrackSubSequenceBackground = new Color(0.1607843f, 0.2156863f, 0.2156863f, 1.0f);

        [SerializeField]
        public Color colorTrackSubSequenceBackgroundSelected = new Color(0.0726923f, 0.252f, 0.252f, 1.0f);

        [SerializeField]
        public Color colorSubSequenceOverlay = new Color(0.02f, 0.025f, 0.025f, 0.30f);

        [SerializeField]
        public Color colorSubSequenceDurationLine = new Color(0.0f, 1.0f, 0.88f, 0.46f);

        public void SetDefault()
        {
            colorPlayhead = DirectorStyles.Instance.timeCursor.normal.textColor;
            colorSelection = DirectorStyles.Instance.selectedStyle.normal.textColor;
            colorEndmarker = DirectorStyles.Instance.endmarker.normal.textColor;

            colorGroup = new Color(0.094f, 0.357f, 0.384f, 0.310f);
            colorGroupTrackBackground = new Color(0f, 0f, 0f, 1f);
            colorAnimation = new Color(0.3f, 0.39f, 0.46f, 1.0f);
            colorAnimationRecorded = new Color(colorAnimation.r * 0.75f, colorAnimation.g * 0.75f, colorAnimation.b * 0.75f, 1.0f);
            colorAudio = new Color(1f, 0.635f, 0f);
            colorAudioWaveform = new Color(0.129f, 0.164f, 0.254f);
            colorActivation = Color.green;

            colorDropTarget = new Color(0.514f, 0.627f, 0.827f);
            colorClipFont = DirectorStyles.Instance.fontClip.normal.textColor;
            colorTrackBackground = new Color(0.2f, 0.2f, 0.2f, 1.0f);
            colorTrackBackgroundSelected = new Color(1f, 1f, 1f, 0.33f);

            colorInlineCurvesBackground = new Color(0.25f, 0.25f, 0.25f, 0.6f);

            colorTrackFont = DirectorStyles.Instance.trackHeaderFont.normal.textColor;

            colorClipUnion = new Color(0.72f, 0.72f, 0.72f, 0.8f);
            colorTopOutline3 = new Color(0.274f, 0.274f, 0.274f, 1.0f);

            colorDurationLine = new Color(33.0f / 255.0f, 109.0f / 255.0f, 120.0f / 255.0f);

            colorRange = new Color(0.733f, 0.733f, 0.733f, 0.70f);

            colorSequenceBackground = new Color(0.16f, 0.16f, 0.16f, 1.0f);

            colorTooltipBackground = new Color(29.0f / 255.0f, 32.0f / 255.0f, 33.0f / 255.0f);

            colorInfiniteClipLine = new Color(72.0f / 255.0f, 78.0f / 255.0f, 82.0f / 255.0f);

            colorTrackBackgroundRecording = new Color(1, 0, 0, 0.1f);

            colorTrackDarken = new Color(0.0f, 0.0f, 0.0f, 0.4f);

            colorTrackHeaderBackground = new Color(51.0f / 255.0f, 51.0f / 255.0f, 51.0f / 255.0f, 1.0f);

            colorDefaultTrackDrawer = new Color(218.0f / 255.0f, 220.0f / 255.0f, 222.0f / 255.0f);

            colorRecordingClipOutline = new Color(1, 0, 0, 0.9f);
            colorInlineCurveVerticalLines = new Color(1.0f, 1.0f, 1.0f, 0.2f);
            colorInlineCurveOutOfRangeOverlay = new Color(0.0f, 0.0f, 0.0f, 0.5f);
        }

        public void ToText(string path)
        {
            StringBuilder builder = new StringBuilder();

            var fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach (var f in fields)
            {
                if (f.FieldType != typeof(Color))
                    continue;

                Color c = (Color)f.GetValue(this);
                builder.AppendLine(f.Name + "," + c);
            }

            string filePath = Application.dataPath + "/Editor Default Resources/" + path;
            File.WriteAllText(filePath, builder.ToString());
        }

        public void FromText(string text)
        {
            // parse to a map
            string[] lines = text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            var map = new Dictionary<string, Color>();
            foreach (var line in lines)
            {
                var pieces = line.Replace("RGBA(", "").Replace(")", "").Split(',');
                if (pieces.Length == 5)
                {
                    string name = pieces[0].Trim();
                    Color c = Color.black;
                    bool b = ParseFloat(pieces[1], out c.r) &&
                        ParseFloat(pieces[2], out c.g) &&
                        ParseFloat(pieces[3], out c.b) &&
                        ParseFloat(pieces[4], out c.a);

                    if (b)
                    {
                        map[name] = c;
                    }
                }
            }

            var fields = typeof(DirectorNamedColor).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach (var f in fields)
            {
                if (f.FieldType != typeof(Color))
                    continue;

                Color c = Color.black;
                if (map.TryGetValue(f.Name, out c))
                {
                    f.SetValue(this, c);
                }
            }
        }

        // Case 938534 - Timeline window has white background when running on .NET 4.6 depending on the set system language
        // Make sure we're using an invariant culture so "0.35" is parsed as 0.35 and not 35
        static bool ParseFloat(string str, out float f)
        {
            return float.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out f);
        }

        public static DirectorNamedColor CreateAndLoadFromText(string text)
        {
            DirectorNamedColor instance = CreateInstance<DirectorNamedColor>();
            instance.FromText(text);
            return instance;
        }
    }
}
