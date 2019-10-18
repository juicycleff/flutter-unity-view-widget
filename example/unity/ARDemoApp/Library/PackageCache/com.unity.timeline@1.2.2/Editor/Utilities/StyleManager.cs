using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental;
using UnityEditor.StyleSheets;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    static class StyleManager
    {
        static readonly StyleState[] k_StyleStates = { StyleState.any };
        static readonly string k_ErrorCannotFindStyle = L10n.Tr("Cannot find style {0} for {1}");

        static Dictionary<Type, GUIStyle> s_CustomStyles = new Dictionary<Type, GUIStyle>();
        static GUISkin s_CurrentSkin;

        public static GUIStyle UssStyleForType(Type type)
        {
            ClearCacheIfInvalid();

            GUIStyle cachedStyle;
            if (s_CustomStyles.TryGetValue(type, out cachedStyle))
                return cachedStyle;

            var style = DirectorStyles.GetGUIStyle(DirectorStyles.markerDefaultStyle);

            var customStyleForType = CustomStyleForType(type);
            if (customStyleForType != null)
            {
                if (IsStyleValid(customStyleForType))
                    style = DirectorStyles.GetGUIStyle(customStyleForType);
                else
                    Debug.LogWarningFormat(k_ErrorCannotFindStyle, customStyleForType, type.Name);
            }

            s_CustomStyles.Add(type, style);
            return style;
        }

        static string CustomStyleForType(Type type)
        {
            var attr = (CustomStyleAttribute)type.GetCustomAttributes(typeof(CustomStyleAttribute), true).FirstOrDefault();
            return attr != null ? attr.ussStyle : null;
        }

        static bool IsStyleValid(string ussStyle)
        {
            return GUISkin.current.FindStyle(ussStyle) != null || EditorResources.styleCatalog.GetStyle(ussStyle, k_StyleStates).IsValid();
        }

        static void ClearCacheIfInvalid()
        {
            if (s_CurrentSkin != GUISkin.current)
                s_CustomStyles.Clear();
            s_CurrentSkin = GUISkin.current;
        }
    }
}
