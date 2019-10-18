using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    static class AnimationOffsetMenu
    {
        public static GUIContent MatchPreviousMenuItem = EditorGUIUtility.TrTextContent("Match Offsets To Previous Clip");
        public static GUIContent MatchNextMenuItem = EditorGUIUtility.TrTextContent("Match Offsets To Next Clip");
        public static string MatchFieldsPrefix = "Match Offsets Fields/";
        public static GUIContent ResetOffsetMenuItem = EditorGUIUtility.TrTextContent("Reset Offsets");

        static bool EnforcePreviewMode(WindowState state)
        {
            state.previewMode = true; // try and set the preview mode
            if (!state.previewMode)
            {
                Debug.LogError("Match clips cannot be completed because preview mode cannot be enabed");
                return false;
            }
            return true;
        }

        internal static void MatchClipsToPrevious(WindowState state, TimelineClip[] clips)
        {
            if (!EnforcePreviewMode(state))
                return;

            clips = clips.OrderBy(x => x.start).ToArray();
            foreach (var clip in clips)
            {
                var sceneObject = TimelineUtility.GetSceneGameObject(state.editSequence.director, clip.parentTrack);
                if (sceneObject != null)
                {
                    TimelineUndo.PushUndo(clip.asset, "Match Clip");
                    TimelineAnimationUtilities.MatchPrevious(clip, sceneObject.transform, state.editSequence.director);
                }
            }

            InspectorWindow.RepaintAllInspectors();
            TimelineEditor.Refresh(RefreshReason.ContentsModified);
        }

        internal static void MatchClipsToNext(WindowState state, TimelineClip[] clips)
        {
            if (!EnforcePreviewMode(state))
                return;

            clips = clips.OrderByDescending(x => x.start).ToArray();
            foreach (var clip in clips)
            {
                var sceneObject = TimelineUtility.GetSceneGameObject(state.editSequence.director, clip.parentTrack);
                if (sceneObject != null)
                {
                    TimelineUndo.PushUndo(clip.asset, "Match Clip");
                    TimelineAnimationUtilities.MatchNext(clip, sceneObject.transform, state.editSequence.director);
                }
            }

            InspectorWindow.RepaintAllInspectors();
            TimelineEditor.Refresh(RefreshReason.ContentsModified);
        }

        public static void ResetClipOffsets(WindowState state, TimelineClip[] clips)
        {
            foreach (var clip in clips)
            {
                if (clip.asset is AnimationPlayableAsset)
                {
                    TimelineUndo.PushUndo(clip.asset, "Reset Offsets");
                    var playableAsset = (AnimationPlayableAsset)clip.asset;
                    playableAsset.ResetOffsets();
                }
            }
            state.rebuildGraph = true;

            InspectorWindow.RepaintAllInspectors();
            TimelineEditor.Refresh(RefreshReason.SceneNeedsUpdate);
        }
    }
}
