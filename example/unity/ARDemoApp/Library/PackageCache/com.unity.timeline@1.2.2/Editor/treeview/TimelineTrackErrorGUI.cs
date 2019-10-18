using System;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Playables;

namespace UnityEditor.Timeline
{
    class TimelineTrackErrorGUI : TimelineTrackBaseGUI
    {
        static class Styles
        {
            public static readonly GUIContent ErrorText = EditorGUIUtility.TrTextContent("Track cannot be loaded.", "Please fix any compile errors in the script for this track");
            public static readonly Texture2D IconWarn = EditorGUIUtility.LoadIconRequired("console.warnicon.inactive.sml");
            public static readonly GUIContent RemoveTrack = EditorGUIUtility.TrTextContent("Delete");

            public static readonly Color WarningBoxBackgroundColor = new Color(115.0f / 255.0f, 115.0f / 255.0f, 115.0f / 255.0f); // approved for both skins
            public static readonly Color WarningBoxHighlightColor = new Color(229 / 255.0f, 208 / 255.0f, 54 / 255.0f); // brigher than standard warning color for contrast
        }

        Rect m_TrackRect;
        ScriptableObject m_ScriptableObject;
        PlayableAsset m_Owner;
        static GUIContent s_GUIContent = new GUIContent();

        public TimelineTrackErrorGUI(TreeViewController treeview, TimelineTreeViewGUI treeviewGUI, int id, int depth, TreeViewItem parent, string displayName, ScriptableObject track, PlayableAsset owner)
            : base(id, depth, parent, displayName, null, treeview, treeviewGUI)
        {
            m_ScriptableObject = track;
            m_Owner = owner;
        }

        public override Rect boundingRect
        {
            get { return m_TrackRect; }
        }

        public override bool expandable
        {
            get { return false; }
        }

        public override void Draw(Rect headerRect, Rect contentRect, WindowState state)
        {
            m_TrackRect = contentRect;

            DrawMissingTrackHeader(headerRect, state);
            DrawMissingTrackBody(contentRect);
        }

        void DrawMissingTrackHeader(Rect headerRect, WindowState state)
        {
            var styles = DirectorStyles.Instance;

            // Draw a header
            Color backgroundColor = styles.customSkin.colorTrackHeaderBackground;
            var bgRect = headerRect;
            bgRect.x += styles.trackSwatchStyle.fixedWidth;
            bgRect.width -= styles.trackSwatchStyle.fixedWidth;
            EditorGUI.DrawRect(bgRect, backgroundColor);

            // draw the warning icon
            var errorIcon = Styles.IconWarn;
            Rect iconRect = new Rect(headerRect.xMin + styles.trackSwatchStyle.fixedWidth, headerRect.yMin + 0.5f * (headerRect.height - errorIcon.height), errorIcon.width, errorIcon.height);
            if (iconRect.width > 0 && iconRect.height > 0)
            {
                GUI.DrawTexture(iconRect, errorIcon, ScaleMode.ScaleAndCrop, true, 0,  DirectorStyles.kClipErrorColor, 0, 0);
            }

            // Draw the name

            // m_ScriptableObject == null will return true because the script can't be loaded. so this checks
            // to make sure it is actually not null so we can grab the name
            object o = m_ScriptableObject;
            if (o != null)
            {
                s_GUIContent.text = m_ScriptableObject.name;
                var textStyle = styles.trackHeaderFont;
                textStyle.normal.textColor = styles.customSkin.colorTrackFont; // TODO -- we shouldn't modify the style like this. track header does it though :(
                Rect textRect = headerRect;
                textRect.xMin = iconRect.xMax + 1;
                textRect.xMax = Math.Min(textRect.xMin + styles.trackHeaderFont.CalcSize(s_GUIContent).x, headerRect.xMax - 1);
                EditorGUI.LabelField(textRect, s_GUIContent, textStyle);
            }


            // Draw the color swatch to the left of the track, darkened by the mute
            var color = Color.Lerp(DirectorStyles.kClipErrorColor, styles.customSkin.colorTrackDarken, styles.customSkin.colorTrackDarken.a);
            color.a = 1;
            using (new GUIColorOverride(color))
            {
                var colorSwatchRect = headerRect;
                colorSwatchRect.width = styles.trackSwatchStyle.fixedWidth;
                GUI.Label(colorSwatchRect, GUIContent.none, styles.trackSwatchStyle);
            }

            // draw darken overlay
            EditorGUI.DrawRect(bgRect, styles.customSkin.colorTrackDarken);

            DrawRemoveMenu(headerRect, state);
        }

        void DrawRemoveMenu(Rect headerRect, WindowState state)
        {
            const float pad = 3;
            const float buttonSize = 16;
            var buttonRect = new Rect(headerRect.xMax - buttonSize - pad, headerRect.y + ((headerRect.height - buttonSize) / 2f) + 2, buttonSize, buttonSize);

            if (GUI.Button(buttonRect, GUIContent.none, DirectorStyles.Instance.trackOptions))
            {
                GenericMenu menu = new GenericMenu();

                var owner = m_Owner;
                var scriptableObject = m_ScriptableObject;

                menu.AddItem(Styles.RemoveTrack, false, () =>
                {
                    if (TrackExtensions.RemoveBrokenTrack(owner, scriptableObject))
                        state.Refresh();
                }
                );

                menu.ShowAsContext();
            }
        }

        static void DrawMissingTrackBody(Rect contentRect)
        {
            if (contentRect.width < 0)
                return;

            var styles = DirectorStyles.Instance;

            // draw a track rectangle
            EditorGUI.DrawRect(contentRect, styles.customSkin.colorTrackDarken);
            // draw the warning box
            DrawScriptWarningBox(contentRect, Styles.ErrorText);
        }

        static void DrawScriptWarningBox(Rect trackRect, GUIContent content)
        {
            var styles = DirectorStyles.Instance;
            const float kTextPadding = 52f;

            var errorIcon = Styles.IconWarn;
            float textWidth = styles.fontClip.CalcSize(content).x;

            var outerRect = trackRect;
            outerRect.width = textWidth + kTextPadding + errorIcon.width;
            outerRect.x += (trackRect.width - outerRect.width) / 2f;
            outerRect.height -= 4f;
            outerRect.y += 1f;

            bool drawText = true;
            if (outerRect.width > trackRect.width)
            {
                outerRect.x = trackRect.x;
                outerRect.width = trackRect.width;
                drawText = false;
            }

            var innerRect = new Rect(outerRect.x + 2, outerRect.y + 2, outerRect.width - 4, outerRect.height - 4);
            using (new GUIColorOverride(Styles.WarningBoxHighlightColor))
                GUI.Box(outerRect, GUIContent.none, styles.displayBackground);
            using (new GUIColorOverride(Styles.WarningBoxBackgroundColor))
                GUI.Box(innerRect, GUIContent.none, styles.displayBackground);


            if (drawText)
            {
                var iconRect = new Rect(outerRect.x + kTextPadding / 2.0f - 4.0f, outerRect.y + (outerRect.height - errorIcon.height) / 2.0f, errorIcon.width, errorIcon.height);
                var textRect = new Rect(iconRect.xMax + 4.0f, outerRect.y, textWidth, outerRect.height);

                GUI.DrawTexture(iconRect, errorIcon, ScaleMode.ScaleAndCrop, true, 0, Styles.WarningBoxHighlightColor, 0, 0);
                Graphics.ShadowLabel(textRect, content, styles.fontClip, Color.white, Color.black);
            }
            else if (errorIcon.width > innerRect.width)
            {
                var iconRect = new Rect(outerRect.x + (outerRect.width - errorIcon.width) / 2.0f, outerRect.y + (outerRect.height - errorIcon.height) / 2.0f, errorIcon.width, errorIcon.height);
                GUI.DrawTexture(iconRect, errorIcon, ScaleMode.ScaleAndCrop, true, 0, Styles.WarningBoxHighlightColor, 0, 0);
            }
        }

        public override void OnGraphRebuilt() {}
    }
}
