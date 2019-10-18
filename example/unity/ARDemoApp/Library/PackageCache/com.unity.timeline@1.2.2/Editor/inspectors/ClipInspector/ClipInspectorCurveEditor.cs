using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.Timeline
{
    class ClipInspectorCurveEditor
    {
        CurveEditor m_CurveEditor;

        AnimationCurve[] m_Curves;
        CurveWrapper[] m_CurveWrappers;

        const float k_HeaderHeight = 30;
        const float k_PresetHeight = 30;

        Action<AnimationCurve, EditorCurveBinding> m_CurveUpdatedCallback;
        GUIContent m_TextContent = new GUIContent();

        GUIStyle m_LabelStyle;
        GUIStyle m_LegendStyle;

        // Track time. controls the position of the track head
        public static readonly double kDisableTrackTime = double.NaN;
        double m_trackTime = kDisableTrackTime;
        public double trackTime { get { return m_trackTime; } set { m_trackTime = value; } }

        public string headerString { get; set; }

        public ClipInspectorCurveEditor()
        {
            var curveEditorSettings = new CurveEditorSettings
            {
                allowDeleteLastKeyInCurve = false,
                allowDraggingCurvesAndRegions = true,
                hTickLabelOffset = 0.1f,
                showAxisLabels = true,
                useFocusColors = false,
                wrapColor = new EditorGUIUtility.SkinnedColor(Color.black),
                hSlider = false,
                hRangeMin = 0.0f,
                vRangeMin = 0.0F,
                vRangeMax = 1.0f,
                hRangeMax = 1.0F,
                vSlider = false,
                hRangeLocked = false,
                vRangeLocked = false,

                hTickStyle = new TickStyle
                {
                    tickColor = new EditorGUIUtility.SkinnedColor(new Color(0.0f, 0.0f, 0.0f, 0.2f)),
                    distLabel = 30,
                    stubs = false,
                    centerLabel = true
                },

                vTickStyle = new TickStyle
                {
                    tickColor = new EditorGUIUtility.SkinnedColor(new Color(1.0f, 0.0f, 0.0f, 0.2f)),
                    distLabel = 20,
                    stubs = false,
                    centerLabel = true
                }
            };

            m_CurveEditor = new CurveEditor(new Rect(0, 0, 1000, 100), new CurveWrapper[0], true)
            {
                settings = curveEditorSettings,
                ignoreScrollWheelUntilClicked = true
            };
        }

        internal bool InitStyles()
        {
            if (EditorStyles.s_Current == null)
                return false;

            if (m_LabelStyle == null)
            {
                m_LabelStyle = new GUIStyle(EditorStyles.whiteLargeLabel);
                m_LegendStyle = new GUIStyle(EditorStyles.miniBoldLabel);

                m_LabelStyle.alignment = TextAnchor.MiddleCenter;
                m_LegendStyle.alignment = TextAnchor.MiddleCenter;
            }
            return true;
        }

        internal void OnGUI(Rect clientRect, CurvePresetLibrary presets)
        {
            const float presetPad = 30.0f;

            if (!InitStyles())
                return;

            if (m_Curves == null || m_Curves.Length == 0)
                return;

            // regions
            var headerRect = new Rect(clientRect.x, clientRect.y, clientRect.width, k_HeaderHeight);
            var curveRect = new Rect(clientRect.x, clientRect.y + headerRect.height, clientRect.width, clientRect.height - k_HeaderHeight - k_PresetHeight);
            var presetRect = new Rect(clientRect.x + presetPad, clientRect.y + curveRect.height + k_HeaderHeight, clientRect.width - presetPad, k_PresetHeight);

            GUI.Box(headerRect, headerString, m_LabelStyle);

            m_CurveEditor.rect = curveRect;
            m_CurveEditor.shownAreaInsideMargins = new Rect(0, 0, 1, 1);
            m_CurveEditor.animationCurves = m_CurveWrappers;
            UpdateSelectionColors();

            DrawTrackHead(curveRect);

            EditorGUI.BeginChangeCheck();

            m_CurveEditor.OnGUI();
            DrawPresets(presetRect, presets);

            bool hasChanged = EditorGUI.EndChangeCheck();

            if (presets == null)
                DrawLegend(presetRect);

            if (hasChanged)
                ProcessUpdates();
        }

        void DrawPresets(Rect position, PresetLibrary curveLibrary)
        {
            if (curveLibrary == null || curveLibrary.Count() == 0)
                return;

            const int maxNumPresets = 9;
            int numPresets = curveLibrary.Count();
            int showNumPresets = Mathf.Min(numPresets, maxNumPresets);

            const float swatchWidth = 30;
            const float swatchHeight = 15;
            const float spaceBetweenSwatches = 10;
            float presetButtonsWidth = showNumPresets * swatchWidth + (showNumPresets - 1) * spaceBetweenSwatches;
            float flexWidth = (position.width - presetButtonsWidth) * 0.5f;

            // Preset swatch area
            float curY = (position.height - swatchHeight) * 0.5f;
            float curX = 3.0f;
            if (flexWidth > 0)
                curX = flexWidth;

            GUI.BeginGroup(position);

            for (int i = 0; i < showNumPresets; i++)
            {
                if (i > 0)
                    curX += spaceBetweenSwatches;

                var swatchRect = new Rect(curX, curY, swatchWidth, swatchHeight);
                m_TextContent.tooltip = curveLibrary.GetName(i);
                if (GUI.Button(swatchRect, m_TextContent, GUIStyle.none))
                {
                    // if there is only 1, no need to specify
                    IEnumerable<CurveWrapper> wrappers = m_CurveWrappers;
                    if (m_CurveWrappers.Length > 1)
                        wrappers = m_CurveWrappers.Where(x => x.selected == CurveWrapper.SelectionMode.Selected);

                    foreach (var wrapper in wrappers)
                    {
                        var presetCurve = (AnimationCurve)curveLibrary.GetPreset(i);
                        wrapper.curve.keys = (Keyframe[])presetCurve.keys.Clone();
                        wrapper.changed = true;
                    }
                }

                if (Event.current.type == EventType.Repaint)
                    curveLibrary.Draw(swatchRect, i);

                curX += swatchWidth;
            }

            GUI.EndGroup();
        }

        // draw a line representing where in the current clip we are
        void DrawTrackHead(Rect clientRect)
        {
            DirectorStyles styles = TimelineWindow.styles;
            if (styles == null)
                return;

            if (!double.IsNaN(m_trackTime))
            {
                float x = m_CurveEditor.TimeToPixel((float)m_trackTime, clientRect);
                x = Mathf.Clamp(x, clientRect.xMin, clientRect.xMax);
                var p1 = new Vector2(x, clientRect.yMin);
                var p2 = new Vector2(x, clientRect.yMax);
                Graphics.DrawLine(p1, p2, DirectorStyles.Instance.customSkin.colorPlayhead);
            }
        }

        // Draws a legend for the displayed curves
        void DrawLegend(Rect r)
        {
            if (m_CurveWrappers == null || m_CurveWrappers.Length == 0)
                return;

            Color c = GUI.color;
            float boxWidth = r.width / m_CurveWrappers.Length;
            for (int i = 0; i < m_CurveWrappers.Length; i++)
            {
                CurveWrapper cw = m_CurveWrappers[i];
                if (cw != null)
                {
                    var pos = new Rect(r.x + i * boxWidth, r.y, boxWidth, r.height);
                    var textColor = cw.color;
                    textColor.a = 1;
                    GUI.color = textColor;
                    string name = LabelName(cw.binding.propertyName);
                    EditorGUI.LabelField(pos, name, m_LegendStyle);
                }
            }
            GUI.color = c;
        }

        // Helper for making label name appropriately small
        static char[] s_LabelMarkers = { '_' };

        static string LabelName(string propertyName)
        {
            propertyName = AnimationWindowUtility.GetPropertyDisplayName(propertyName);
            int index = propertyName.LastIndexOfAny(s_LabelMarkers);
            if (index >= 0)
                propertyName = propertyName.Substring(index);
            return propertyName;
        }

        public void SetCurves(AnimationCurve[] curves, EditorCurveBinding[] bindings)
        {
            m_Curves = curves;
            if (m_Curves != null && m_Curves.Length > 0)
            {
                m_CurveWrappers = new CurveWrapper[m_Curves.Length];
                for (int i = 0; i < m_Curves.Length; i++)
                {
                    var cw = new CurveWrapper
                    {
                        renderer = new NormalCurveRenderer(m_Curves[i]),
                        readOnly = false,
                        color = EditorGUI.kCurveColor,
                        id = curves[i].GetHashCode(),
                        hidden = false,
                        regionId = -1
                    };

                    cw.renderer.SetWrap(WrapMode.Clamp, WrapMode.Clamp);
                    cw.renderer.SetCustomRange(0, 1);

                    if (bindings != null)
                    {
                        cw.binding = bindings[i];
                        cw.color = CurveUtility.GetPropertyColor(bindings[i].propertyName);
                        cw.id = bindings[i].GetHashCode();
                    }

                    m_CurveWrappers[i] = cw;
                }

                UpdateSelectionColors();
                m_CurveEditor.animationCurves = m_CurveWrappers;
            }
        }

        internal void SetUpdateCurveCallback(Action<AnimationCurve, EditorCurveBinding> callback)
        {
            m_CurveUpdatedCallback = callback;
        }

        void ProcessUpdates()
        {
            foreach (var cw in m_CurveWrappers)
            {
                if (cw.changed)
                {
                    cw.changed = false;

                    if (m_CurveUpdatedCallback != null)
                        m_CurveUpdatedCallback(cw.curve, cw.binding);
                }
            }
        }

        public void SetSelected(AnimationCurve curve)
        {
            m_CurveEditor.SelectNone();
            for (int i = 0; i < m_Curves.Length; i++)
            {
                if (curve == m_Curves[i])
                {
                    m_CurveWrappers[i].selected = CurveWrapper.SelectionMode.Selected;
                    m_CurveEditor.AddSelection(new CurveSelection(m_CurveWrappers[i].id, 0));
                }
            }

            UpdateSelectionColors();
        }

        void UpdateSelectionColors()
        {
            if (m_CurveWrappers == null)
                return;

            // manually manage selection colors
            foreach (var cw in m_CurveWrappers)
            {
                Color c = cw.color;
                if (cw.readOnly)
                    c.a = 0.75f;
                else if (cw.selected != CurveWrapper.SelectionMode.None)
                    c.a = 1.0f;
                else
                    c.a = 0.5f;
                cw.color = c;
            }
        }

        public static void CurveField(GUIContent title, SerializedProperty property, Action<SerializedProperty> onClick)
        {
            Rect controlRect = EditorGUILayout.GetControlRect(GUILayout.MinWidth(20));
            EditorGUI.BeginProperty(controlRect, title, property);
            DrawCurve(controlRect, property, onClick, EditorGUI.kCurveColor, EditorGUI.kCurveBGColor);
            EditorGUI.EndProperty();
        }

        static Rect DrawCurve(Rect controlRect, SerializedProperty property, Action<SerializedProperty> onClick, Color fgColor, Color bgColor)
        {
            if (GUI.Button(controlRect, GUIContent.none))
            {
                if (onClick != null)
                    onClick(property);
            }
            EditorGUIUtility.DrawCurveSwatch(controlRect, null, property, fgColor, bgColor);
            return controlRect;
        }
    }
}
