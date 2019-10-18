using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityObject = UnityEngine.Object;

namespace UnityEditor.Timeline
{
    static class EditorClipFactory
    {
        static Dictionary<TimelineClip, EditorClip> s_EditorCache = new Dictionary<TimelineClip, EditorClip>();

        public static EditorClip GetEditorClip(TimelineClip clip)
        {
            if (clip == null)
                throw new ArgumentException("parameter cannot be null");

            if (s_EditorCache.ContainsKey(clip))
            {
                var editorClip = s_EditorCache[clip];
                if (editorClip != null)
                    return editorClip;
            }

            var editor = ScriptableObject.CreateInstance<EditorClip>();
            editor.hideFlags |= HideFlags.HideInHierarchy | HideFlags.DontSaveInEditor;
            editor.lastHash = -1;
            editor.clip = clip;
            s_EditorCache[clip] = editor;

            return editor;
        }

        public static void RemoveEditorClip(TimelineClip clip)
        {
            if (clip == null)
                return;

            if (s_EditorCache.ContainsKey(clip))
            {
                var obj = s_EditorCache[clip];
                if (obj != null)
                    UnityObject.DestroyImmediate(obj);
                s_EditorCache.Remove(clip);
            }
        }

        public static bool Contains(TimelineClip clip)
        {
            return clip != null && s_EditorCache.ContainsKey(clip);
        }
    }
}
