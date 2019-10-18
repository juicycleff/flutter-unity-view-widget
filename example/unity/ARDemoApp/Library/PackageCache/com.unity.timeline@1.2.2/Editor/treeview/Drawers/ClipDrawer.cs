using System;
using UnityEngine.Timeline;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace UnityEditor.Timeline
{
    enum BlendKind
    {
        None,
        Ease,
        Mix
    }

    struct IconData
    {
        public enum Side { Left = -1, Right = 1 }

        public Texture2D icon;
        public Color tint;

        public float width { get { return 16; } }
        public float height { get { return 16; } }

        public IconData(Texture2D icon, Color tint)
        {
            this.icon = icon;
            this.tint = tint;
        }

        public IconData(Texture2D icon)
        {
            this.icon = icon;
            this.tint = Color.white;
        }
    }

    class ClipBorder
    {
        public readonly Color color;
        public readonly float thickness;

        ClipBorder(Color color, float thickness)
        {
            this.color = color;
            this.thickness = thickness;
        }

        const float k_ClipSelectionBorder = 1.0f;
        const float k_ClipRecordingBorder = 2.0f;

        public static readonly ClipBorder kSelection = new ClipBorder(Color.white, k_ClipSelectionBorder);
        public static readonly ClipBorder kRecording = new ClipBorder(DirectorStyles.Instance.customSkin.colorRecordingClipOutline, k_ClipRecordingBorder);
    }

    struct ClipBlends
    {
        public readonly BlendKind inKind;
        public readonly Rect inRect;

        public readonly BlendKind outKind;
        public readonly Rect outRect;

        public ClipBlends(BlendKind inKind, Rect inRect, BlendKind outKind, Rect outRect)
        {
            this.inKind = inKind;
            this.inRect = inRect;
            this.outKind = outKind;
            this.outRect = outRect;
        }

        public static readonly ClipBlends kNone = new ClipBlends(BlendKind.None, Rect.zero, BlendKind.None, Rect.zero);
    }

    struct ClipDrawData
    {
        public TimelineClip clip;             // clip being drawn
        public Rect targetRect;               // rectangle to draw to
        public Rect unclippedRect;            // the clip's unclipped rect
        public Rect clippedRect;              // the clip's clipped rect to the visible time area
        public Rect clipCenterSection;        // clip center section
        public string title;                  // clip title
        public bool selected;                 // is the clip selected
        public bool inlineCurvesSelected;     // is the inline curve of the clip selected
        public double localVisibleStartTime;
        public double localVisibleEndTime;
        public IconData[] leftIcons;
        public IconData[] rightIcons;
        public TimelineClip previousClip;
        public bool supportsLooping;
        public int minLoopIndex;
        public List<Rect> loopRects;
        public ClipBlends clipBlends;
        public ClipDrawOptions ClipDrawOptions;
        public ClipEditor clipEditor;
    }

    static class ClipDrawer
    {
        public static class Styles
        {
            public static readonly Texture2D iconWarn = EditorGUIUtility.LoadIconRequired("console.warnicon.inactive.sml");
            public static readonly GUIContent addClipContent = EditorGUIUtility.TrTextContent("Add From");
            public static readonly string HoldText = LocalizationDatabase.GetLocalizedString("HOLD");
            public static readonly Texture2D s_IconNoRecord = EditorGUIUtility.LoadIcon("console.erroricon.sml");
            public static readonly GUIContent s_ClipNotRecorable = EditorGUIUtility.TrTextContent("", "This clip is not recordable");
            public static readonly GUIContent s_ClipNoRecordInBlend = EditorGUIUtility.TrTextContent("", "Recording in blends in prohibited");

            public static readonly Texture blendMixInBckg = DirectorStyles.GetBackgroundImage(DirectorStyles.Instance.blendMixIn);
            public static readonly Texture blendEaseInBckg = DirectorStyles.GetBackgroundImage(DirectorStyles.Instance.blendEaseIn);
            public static readonly Texture blendMixOutBckg = DirectorStyles.GetBackgroundImage(DirectorStyles.Instance.blendMixOut);
            public static readonly Texture blendEaseOutBckg = DirectorStyles.GetBackgroundImage(DirectorStyles.Instance.blendEaseOut);
            
        }

        const float k_ClipSwatchLineThickness = 4.0f;
        const float k_MinClipWidth = 7.0f;
        const float k_ClipInOutMinWidth = 15.0f;
        const float k_ClipLoopsMinWidth = 20.0f;
        const float k_ClipLabelPadding = 6.0f;
        const float k_ClipLabelMinWidth = 10.0f;
        const float k_IconsPadding = 1.0f;
        const float k_ClipInlineWidth = 2.0f;

        static readonly GUIContent s_TitleContent = new GUIContent();
        static readonly IconData[] k_ClipErrorIcons =  { new IconData {icon = Styles.iconWarn, tint = DirectorStyles.kClipErrorColor} };
        static readonly Color s_InlineLightColor = new Color(1.0f, 1.0f, 1.0f, 0.2f);
        static readonly Color s_InlineShadowColor = new Color(0.0f, 0.0f, 0.0f, 0.2f);
        static readonly Dictionary<int, string> s_LoopStringCache = new Dictionary<int, string>(100);


        // caches the loopstring to avoid allocation from string concats
        static string GetLoopString(int loopIndex)
        {
            string loopString = null;
            if (!s_LoopStringCache.TryGetValue(loopIndex, out loopString))
            {
                loopString = "L" + loopIndex;
                s_LoopStringCache[loopIndex] = loopString;
            }
            return loopString;
        }

        static void DrawLoops(ClipDrawData drawData)
        {
            if (drawData.loopRects == null || drawData.loopRects.Count == 0)
                return;

            var oldColor = GUI.color;

            int loopIndex = drawData.minLoopIndex;
            for (int l = 0; l < drawData.loopRects.Count; l++)
            {
                Rect theRect = drawData.loopRects[l];
                theRect.x -= drawData.unclippedRect.x;
                theRect.x += 1;
                theRect.width -= 2.0f;
                theRect.y = 5.0f;
                theRect.height -= 4.0f;
                theRect.xMin -= 4f;

                if (theRect.width >= 5f)
                {
                    GUI.color = new Color(0.0f, 0.0f, 0.0f, 0.2f);
                    GUI.Label(theRect, GUIContent.none, DirectorStyles.Instance.displayBackground);

                    if (theRect.width > 36.0f)
                    {
                        var style = DirectorStyles.Instance.fontClipLoop;
                        GUI.color = new Color(0.0f, 0.0f, 0.0f, 0.3f);
                        var loopContent = new GUIContent(drawData.supportsLooping ? GetLoopString(loopIndex) : Styles.HoldText);
                        GUI.Label(theRect, loopContent, style);
                    }
                }

                loopIndex++;

                if (!drawData.supportsLooping)
                    break;
            }

            GUI.color = oldColor;
        }

        static void DrawClipBorder(ClipDrawData drawData)
        {
            ClipBorder border = null;
            var animTrack = drawData.clip.parentTrack as AnimationTrack;
            if (TimelineWindow.instance.state.recording && animTrack == null && drawData.clip.parentTrack.IsRecordingToClip(drawData.clip))
            {
                border = ClipBorder.kRecording;
            }
            else if (drawData.selected)
            {
                border = ClipBorder.kSelection;
            }

            if (border != null)
                DrawBorder(drawData.clipCenterSection, border, drawData.clipBlends, drawData.previousClip);
        }

        static void DrawClipTimescale(Rect targetRect, double timeScale)
        {
            if (timeScale != 1.0)
            {
                const float xOffset = 4.0f;
                const float yOffset = 6.0f;
                var segmentLength = timeScale > 1.0f ? 5.0f : 15.0f;
                var start = new Vector3(targetRect.min.x + xOffset, targetRect.min.y + yOffset, 0.0f);
                var end = new Vector3(targetRect.max.x - xOffset, targetRect.min.y + yOffset, 0.0f);

                Graphics.DrawDottedLine(start, end, segmentLength, DirectorStyles.Instance.customSkin.colorClipFont);
                Graphics.DrawDottedLine(start + new Vector3(0.0f, 1.0f, 0.0f), end + new Vector3(0.0f, 1.0f, 0.0f), segmentLength, DirectorStyles.Instance.customSkin.colorClipFont);
            }
        }

        static void DrawClipInOut(Rect targetRect, TimelineClip clip)
        {
            var assetDuration = TimelineHelpers.GetClipAssetEndTime(clip);

            bool drawClipOut = assetDuration<double.MaxValue &&
                                             assetDuration - clip.end> TimeUtility.kTimeEpsilon;

            bool drawClipIn = clip.clipIn > 0.0;

            if (!drawClipIn && !drawClipOut)
                return;

            var rect = targetRect;

            if (drawClipOut)
            {
                var icon = DirectorStyles.Instance.clipOut;
                var iconRect = new Rect(rect.xMax - icon.fixedWidth - 2.0f,
                    rect.yMin + (rect.height - icon.fixedHeight) * 0.5f,
                    icon.fixedWidth, icon.fixedHeight);

                GUI.Label(iconRect, GUIContent.none, icon);
            }

            if (drawClipIn)
            {
                var icon = DirectorStyles.Instance.clipIn;
                var iconRect = new Rect(2.0f + rect.xMin,
                    rect.yMin + (rect.height - icon.fixedHeight) * 0.5f,
                    icon.fixedWidth, icon.fixedHeight);

                GUI.Label(iconRect, GUIContent.none, icon);
            }
        }

        static void DrawClipLabel(ClipDrawData data, Rect availableRect, Color color)
        {
            var errorText = data.ClipDrawOptions.errorText;
            var hasError = !string.IsNullOrEmpty(errorText);
            var textColor = hasError ? DirectorStyles.kClipErrorColor : color;
            var tooltip = hasError ? errorText : data.ClipDrawOptions.tooltip;

            if (hasError)
                DrawClipLabel(data.title, availableRect, textColor, k_ClipErrorIcons, null, tooltip);
            else
                DrawClipLabel(data.title, availableRect, textColor, data.leftIcons, data.rightIcons, tooltip);
        }

        static void DrawClipLabel(string title, Rect availableRect, Color color, string errorText = "")
        {
            var hasError = !string.IsNullOrEmpty(errorText);
            var textColor = hasError ? DirectorStyles.kClipErrorColor : color;

            if (hasError)
                DrawClipLabel(title, availableRect, textColor, k_ClipErrorIcons, null, errorText);
            else
                DrawClipLabel(title, availableRect, textColor, null, null, errorText);
        }

        static void DrawClipLabel(string title, Rect availableRect, Color textColor, IconData[] leftIcons, IconData[] rightIcons, string tooltipMessage = "")
        {
            s_TitleContent.text = title;
            var neededTextWidth = DirectorStyles.Instance.fontClip.CalcSize(s_TitleContent).x;
            var neededIconWidthLeft = 0.0f;
            var neededIconWidthRight = 0.0f;

            if (leftIcons != null)
                for (int i = 0, n = leftIcons.Length; i < n; ++i)
                    neededIconWidthLeft += leftIcons[i].width + k_IconsPadding;

            if (rightIcons != null)
                for (int i = 0, n = rightIcons.Length; i < n; ++i)
                    neededIconWidthRight += rightIcons[i].width + k_IconsPadding;

            var neededIconWidth = Mathf.Max(neededIconWidthLeft, neededIconWidthRight);

            // Atomic operation: We either show all icons or no icons at all
            var showIcons = neededTextWidth / 2.0f + neededIconWidth < availableRect.width / 2.0f;

            if (showIcons)
            {
                if (leftIcons != null)
                    DrawClipIcons(leftIcons, IconData.Side.Left, neededTextWidth, availableRect);

                if (rightIcons != null)
                    DrawClipIcons(rightIcons, IconData.Side.Right, neededTextWidth, availableRect);
            }

            if (neededTextWidth > availableRect.width)
                s_TitleContent.text = DirectorStyles.Elipsify(title, availableRect.width, neededTextWidth);

            s_TitleContent.tooltip = tooltipMessage;
            DrawClipName(availableRect, s_TitleContent, textColor);
        }

        static void DrawClipIcons(IconData[] icons, IconData.Side side, float labelWidth, Rect availableRect)
        {
            var halfText = labelWidth / 2.0f;
            var offset = halfText + k_IconsPadding;

            foreach (var iconData in icons)
            {
                offset += iconData.width / 2.0f + k_IconsPadding;

                var iconRect =
                    new Rect(0.0f, 0.0f, iconData.width, iconData.height)
                {
                    center = new Vector2(availableRect.center.x + offset * (int)side, availableRect.center.y)
                };

                DrawIcon(iconRect, iconData.tint, iconData.icon);

                offset += iconData.width / 2.0f;
            }
        }

        static void DrawClipName(Rect rect, GUIContent content, Color textColor)
        {
            Graphics.ShadowLabel(rect, content, DirectorStyles.Instance.fontClip, textColor, Color.black);
        }

        static void DrawIcon(Rect imageRect, Color color, Texture2D icon)
        {
            GUI.DrawTexture(imageRect, icon, ScaleMode.ScaleAndCrop, true, 0, color, 0, 0);
        }

        static void DrawClipBackground(Rect clipCenterSection, ClipBlends blends, bool selected)
        {
            var clipStyle = selected ? DirectorStyles.Instance.timelineClipSelected : DirectorStyles.Instance.timelineClip;

            var texture = DirectorStyles.GetBackgroundImage(clipStyle);
            var lineColor = DirectorStyles.Instance.customSkin.colorClipBlendLines;

            // Center body
            GUI.Label(clipCenterSection, GUIContent.none, clipStyle);

            // Blend/Mix In
            if (blends.inKind != BlendKind.None)
            {
                var mixInRect = blends.inRect;

                if (blends.inKind == BlendKind.Ease)
                {
                    ClipRenderer.RenderTexture(mixInRect, texture, Styles.blendMixInBckg , Color.black);

                    if (!selected)
                        Graphics.DrawLineAA(2.5f, new Vector3(mixInRect.xMin, mixInRect.yMax - 1f, 0), new Vector3(mixInRect.xMax, mixInRect.yMin + 1f, 0), lineColor);
                }
                else
                {
                    var blendInColor = selected ? Color.black : DirectorStyles.Instance.customSkin.colorClipBlendYin;
                    ClipRenderer.RenderTexture(mixInRect, texture, Styles.blendEaseInBckg, blendInColor);

                    if (!selected)
                        Graphics.DrawLineAA(2.0f, new Vector3(mixInRect.xMin, mixInRect.yMin + 1f, 0), new Vector3(mixInRect.xMax, mixInRect.yMax - 1f, 0), lineColor);
                }

                Graphics.DrawLineAA(2.0f, mixInRect.max, new Vector2(mixInRect.xMax, mixInRect.yMin), lineColor);
            }

            // Blend/Mix Out
            if (blends.outKind != BlendKind.None)
            {
                var mixOutRect = blends.outRect;

                if (blends.outKind == BlendKind.Ease)
                {
                    ClipRenderer.RenderTexture(mixOutRect, texture, Styles.blendMixOutBckg, Color.black);

                    if (!selected)
                        Graphics.DrawLineAA(2.5f, new Vector3(mixOutRect.xMin, mixOutRect.yMin + 1f, 0), new Vector3(mixOutRect.xMax, mixOutRect.yMax - 1f, 0), lineColor);
                }
                else
                {
                    var blendOutColor = selected ? Color.black : DirectorStyles.Instance.customSkin.colorClipBlendYang;
                    ClipRenderer.RenderTexture(mixOutRect, texture, Styles.blendEaseOutBckg, blendOutColor);

                    if (!selected)
                        Graphics.DrawLineAA(2.0f, new Vector3(mixOutRect.xMin, mixOutRect.yMin + 1f, 0), new Vector3(mixOutRect.xMax, mixOutRect.yMax - 1f, 0), lineColor);
                }

                Graphics.DrawLineAA(2.0f, mixOutRect.min, new Vector2(mixOutRect.xMin, mixOutRect.yMax), lineColor);
            }
        }

        static void DrawClipEdges(Rect targetRect, Color swatchColor, Color lightColor, Color shadowColor, bool drawLeftEdge, bool drawRightEdge)
        {
            // Draw Colored Line at the bottom.
            var colorRect = targetRect;
            colorRect.yMin = colorRect.yMax - k_ClipSwatchLineThickness;

            EditorGUI.DrawRect(colorRect, swatchColor);

            // Draw Highlighted line at the top
            EditorGUI.DrawRect(
                new Rect(targetRect.xMin, targetRect.yMin, targetRect.width - k_ClipInlineWidth, k_ClipInlineWidth),
                lightColor);

            if (drawLeftEdge)
            {
                // Draw Highlighted line at the left
                EditorGUI.DrawRect(
                    new Rect(targetRect.xMin, targetRect.yMin + k_ClipInlineWidth, k_ClipInlineWidth,
                        targetRect.height),
                    lightColor);
            }

            if (drawRightEdge)
            {
                // Draw darker vertical line at the right of the clip
                EditorGUI.DrawRect(
                    new Rect(targetRect.xMax - k_ClipInlineWidth, targetRect.yMin, k_ClipInlineWidth,
                        targetRect.height),
                    shadowColor);
            }

            // Draw darker vertical line at the bottom of the clip
            EditorGUI.DrawRect(
                new Rect(targetRect.xMin, targetRect.yMax - k_ClipInlineWidth, targetRect.width, k_ClipInlineWidth),
                shadowColor);
        }

        public static void DrawBorder(Rect centerRect, ClipBorder border, ClipBlends blends, TimelineClip prevClip = null)
        {
            var thickness = border.thickness;
            var color = border.color;

            // Draw top selected lines.
            EditorGUI.DrawRect(new Rect(centerRect.xMin, centerRect.yMin, centerRect.width, thickness), color);

            // Draw bottom selected lines.
            EditorGUI.DrawRect(new Rect(centerRect.xMin, centerRect.yMax - thickness, centerRect.width, thickness), color);

            // Draw Left Selected Lines
            if (blends.inKind == BlendKind.None)
            {
                EditorGUI.DrawRect(new Rect(centerRect.xMin, centerRect.yMin, thickness, centerRect.height), color);
            }
            else
            {
                var mixInRect = blends.inRect;

                if (blends.inKind == BlendKind.Ease)
                {
                    EditorGUI.DrawRect(new Rect(mixInRect.xMin, mixInRect.yMax - thickness, mixInRect.width, thickness), color);

                    EditorGUI.DrawRect(new Rect(mixInRect.xMin, mixInRect.yMin, thickness, mixInRect.height), color);

                    Graphics.DrawLineAA(2.0f * thickness, new Vector3(mixInRect.xMin, mixInRect.yMax - 1f, 0), new Vector3(mixInRect.xMax, mixInRect.yMin + 1f, 0), color);
                }
                else if (blends.inKind == BlendKind.Mix)
                {
                    EditorGUI.DrawRect(new Rect(mixInRect.xMin, mixInRect.yMin, mixInRect.width, thickness), color);

                    // If there's another clip in the left, draw the blend.
                    if (prevClip != null && SelectionManager.Contains(prevClip))
                        EditorGUI.DrawRect(new Rect(mixInRect.xMin, mixInRect.yMax - thickness, mixInRect.width, thickness), color); //  Bottom

                    Graphics.DrawLineAA(2.0f * thickness, new Vector3(mixInRect.xMin, mixInRect.yMin + 1f, 0), new Vector3(mixInRect.xMax, mixInRect.yMax - 1f, 0), color);
                }
            }

            // Draw Right Selected Lines
            if (blends.outKind == BlendKind.None)
            {
                EditorGUI.DrawRect(new Rect(centerRect.xMax - thickness, centerRect.yMin, thickness, centerRect.height), color);
            }
            else
            {
                var mixOutRect = blends.outRect;
                EditorGUI.DrawRect(new Rect(mixOutRect.xMin, mixOutRect.yMax - thickness, mixOutRect.width, thickness), color);

                if (blends.outKind == BlendKind.Ease)
                    EditorGUI.DrawRect(new Rect(mixOutRect.xMax - thickness, mixOutRect.yMin, thickness, mixOutRect.height), color);

                Graphics.DrawLineAA(2.0f * thickness, new Vector3(mixOutRect.xMin, mixOutRect.yMin + 1f, 0), new Vector3(mixOutRect.xMax, mixOutRect.yMax - 1f, 0), color);
            }
        }

        public static void DrawSimpleClip(TimelineClip clip, Rect targetRect, ClipBorder border, Color overlay, ClipDrawOptions drawOptions, ClipBlends blends)
        {
            GUI.BeginClip(targetRect);

            var clipRect = new Rect(0.0f, 0.0f, targetRect.width, targetRect.height);

            var orgColor = GUI.color;
            GUI.color = overlay;

            DrawClipBackground(clipRect, ClipBlends.kNone, false);
            GUI.color = orgColor;

            if (clipRect.width <= k_MinClipWidth)
            {
                clipRect.width = k_MinClipWidth;
            }

            DrawClipEdges(clipRect, drawOptions.highlightColor * overlay, s_InlineLightColor * overlay, s_InlineShadowColor * overlay,
                blends.inKind != BlendKind.Mix, blends.outKind != BlendKind.Mix);

            DrawClipTimescale(clipRect, clip.timeScale);

            if (targetRect.width >= k_ClipInOutMinWidth)
                DrawClipInOut(clipRect, clip);

            var textRect = clipRect;

            textRect.xMin += k_ClipLabelPadding;
            textRect.xMax -= k_ClipLabelPadding;

            if (textRect.width > k_ClipLabelMinWidth)
                DrawClipLabel(clip.displayName, textRect, Color.white, drawOptions.errorText);

            if (border != null)
                DrawBorder(clipRect, border, ClipBlends.kNone);

            GUI.EndClip();
        }

        public static void DrawDefaultClip(ClipDrawData drawData)
        {
            DrawClipBackground(drawData.clipCenterSection, drawData.clipBlends, drawData.selected);

            if (drawData.targetRect.width > k_MinClipWidth)
            {
                var isRepaint = (Event.current.type == EventType.Repaint);
                if (isRepaint && drawData.clipEditor != null)
                {
                    var customBodyRect = drawData.clippedRect;
                    customBodyRect.yMin += k_ClipInlineWidth;
                    customBodyRect.yMax -= k_ClipSwatchLineThickness;
                    var region = new ClipBackgroundRegion(customBodyRect, drawData.localVisibleStartTime, drawData.localVisibleEndTime);
                    try
                    {
                        drawData.clipEditor.DrawBackground(drawData.clip, region);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }
            else
            {
                drawData.targetRect.width = k_MinClipWidth;
                drawData.clipCenterSection.width = k_MinClipWidth;
            }

            var overlapRect = drawData.targetRect;
            if (drawData.previousClip != null && SelectionManager.Contains(drawData.previousClip) &&
                drawData.clipBlends.inRect.width != 0)
            {
                var mixInRect = drawData.clipBlends.inRect;
                overlapRect.xMin +=  mixInRect.width;
                Graphics.DrawLineAA(2.0f, new Vector3(mixInRect.xMin, mixInRect.yMin + 1f, 0),
                    new Vector3(mixInRect.xMax, mixInRect.yMax - 1f, 0), ClipBorder.kSelection.color);
            }
            DrawClipEdges(overlapRect, drawData.ClipDrawOptions.highlightColor, s_InlineLightColor, s_InlineShadowColor,
                drawData.clipBlends.inKind != BlendKind.Mix,
                drawData.clipBlends.outKind != BlendKind.Mix);

            DrawClipTimescale(drawData.targetRect, drawData.clip.timeScale);

            if (drawData.targetRect.width >= k_ClipInOutMinWidth)
                DrawClipInOut(drawData.targetRect, drawData.clip);

            var labelRect = drawData.clipCenterSection;

            if (drawData.targetRect.width >= k_ClipLoopsMinWidth)
            {
                bool selected = drawData.selected || drawData.inlineCurvesSelected;

                if (selected)
                {
                    if (drawData.loopRects != null && drawData.loopRects.Any())
                    {
                        DrawLoops(drawData);

                        var l = drawData.loopRects[0];
                        labelRect.xMax = Math.Min(labelRect.xMax, l.x - drawData.unclippedRect.x);
                    }
                }
            }

            labelRect.xMin += k_ClipLabelPadding;
            labelRect.xMax -= k_ClipLabelPadding;

            if (labelRect.width > k_ClipLabelMinWidth)
            {
                DrawClipLabel(drawData, labelRect, Color.white);
            }

            DrawClipBorder(drawData);
        }

        public static void DrawAnimationRecordBorder(ClipDrawData drawData)
        {
            if (!drawData.clip.parentTrack.IsRecordingToClip(drawData.clip))
                return;

            double time = TimelineWindow.instance.state.editSequence.time;
            if (time < drawData.clip.start + drawData.clip.mixInDuration || time > drawData.clip.end - drawData.clip.mixOutDuration)
                return;

            ClipDrawer.DrawBorder(drawData.clipCenterSection, ClipBorder.kRecording, ClipBlends.kNone);
        }

        public static void DrawRecordProhibited(ClipDrawData drawData)
        {
            DrawRecordInvalidClip(drawData);
            DrawRecordOnBlend(drawData);
        }

        public static void DrawRecordOnBlend(ClipDrawData drawData)
        {
            double time = TimelineWindow.instance.state.editSequence.time;
            if (time >= drawData.clip.start && time < drawData.clip.start + drawData.clip.mixInDuration)
            {
                Rect r = Rect.MinMaxRect(drawData.clippedRect.xMin, drawData.clippedRect.yMin, drawData.clipCenterSection.xMin, drawData.clippedRect.yMax);
                DrawInvalidRecordIcon(r, Styles.s_ClipNoRecordInBlend);
            }

            if (time <= drawData.clip.end && time > drawData.clip.end - drawData.clip.mixOutDuration)
            {
                Rect r = Rect.MinMaxRect(drawData.clipCenterSection.xMax, drawData.clippedRect.yMin, drawData.clippedRect.xMax, drawData.clippedRect.yMax);
                DrawInvalidRecordIcon(r, Styles.s_ClipNoRecordInBlend);
            }
        }

        public static void DrawRecordInvalidClip(ClipDrawData drawData)
        {
            if (drawData.clip.recordable)
                return;

            double time = TimelineWindow.instance.state.editSequence.time;
            if (time < drawData.clip.start + drawData.clip.mixInDuration || time > drawData.clip.end - drawData.clip.mixOutDuration)
                return;

            DrawInvalidRecordIcon(drawData.clipCenterSection, Styles.s_ClipNotRecorable);
        }

        public static void DrawInvalidRecordIcon(Rect rect, GUIContent helpText)
        {
            EditorGUI.DrawRect(rect, new Color(0, 0, 0, 0.30f));

            var icon = Styles.s_IconNoRecord;
            if (rect.width < icon.width || rect.height < icon.height)
                return;

            float x = rect.x + (rect.width - icon.width) * 0.5f;
            float y = rect.y + (rect.height - icon.height) * 0.5f;
            Rect r = new Rect(x, y, icon.width, icon.height);
            GUI.Label(r, helpText);
            GUI.DrawTexture(r, icon, ScaleMode.ScaleAndCrop, true, 0, Color.white, 0, 0);
        }
    }
}
