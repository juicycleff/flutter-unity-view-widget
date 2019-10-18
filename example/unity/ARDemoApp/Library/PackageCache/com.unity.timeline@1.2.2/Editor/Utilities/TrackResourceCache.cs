using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    static class TrackResourceCache
    {
        private static Dictionary<System.Type, GUIContent> s_TrackIconCache = new Dictionary<Type, GUIContent>(10);
        private static Dictionary<System.Type, Color> s_TrackColorCache = new Dictionary<Type, Color>(10);
        public static GUIContent s_DefaultIcon = EditorGUIUtility.IconContent("UnityEngine/ScriptableObject Icon");

        public static GUIContent GetTrackIcon(TrackAsset track)
        {
            if (track == null)
                return s_DefaultIcon;

            GUIContent content = null;
            if (!s_TrackIconCache.TryGetValue(track.GetType(), out content))
            {
                content = FindTrackIcon(track);
                s_TrackIconCache[track.GetType()] = content;
            }
            return content;
        }

        public static Color GetTrackColor(TrackAsset track)
        {
            if (track == null)
                return Color.white;

            // Try to ensure DirectorStyles is initialized first
            // Note: GUISkin.current must exist to be able do so
            if (!DirectorStyles.IsInitialized && GUISkin.current != null)
                DirectorStyles.ReloadStylesIfNeeded();

            Color color;
            if (!s_TrackColorCache.TryGetValue(track.GetType(), out color))
            {
                var attr = track.GetType().GetCustomAttributes(typeof(TrackColorAttribute), true);
                if (attr.Length > 0)
                {
                    color = ((TrackColorAttribute)attr[0]).color;
                }
                else
                {
                    // case 1141958
                    // There was an error initializing DirectorStyles
                    if (!DirectorStyles.IsInitialized)
                        return Color.white;

                    color = DirectorStyles.Instance.customSkin.colorDefaultTrackDrawer;
                }

                s_TrackColorCache[track.GetType()] = color;
            }
            return color;
        }

        public static void ClearTrackIconCache()
        {
            s_TrackIconCache.Clear();
        }

        public static void SetTrackIcon<T>(GUIContent content) where T : TrackAsset
        {
            s_TrackIconCache[typeof(T)] = content;
        }

        public static void ClearTrackColorCache()
        {
            s_TrackColorCache.Clear();
        }

        public static void SetTrackColor<T>(Color c) where T : TrackAsset
        {
            s_TrackColorCache[typeof(T)] = c;
        }

        private static GUIContent FindTrackIcon(TrackAsset track)
        {
            // backwards compatible -- try to load from Gizmos folder
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Gizmos/" + track.GetType().Name + ".png");
            if (texture != null)
                return new GUIContent(texture);

            // try to load based on the binding type
            var binding = track.outputs.FirstOrDefault();
            if (binding.outputTargetType != null)
            {
                // Type calls don't properly handle monobehaviours, because an instance is required to
                //  get the monoscript icons
                if (typeof(MonoBehaviour).IsAssignableFrom(binding.outputTargetType))
                {
                    texture = null;
                    var scripts = UnityEngine.Resources.FindObjectsOfTypeAll<MonoScript>();
                    foreach (var script in scripts)
                    {
                        if (script.GetClass() == binding.outputTargetType)
                        {
                            texture = AssetPreview.GetMiniThumbnail(script);
                            break;
                        }
                    }
                }
                else
                {
                    texture = EditorGUIUtility.FindTexture(binding.outputTargetType);
                }

                if (texture != null)
                    return new GUIContent(texture);
            }

            // default to the scriptable object icon
            return s_DefaultIcon;
        }
    }
}
