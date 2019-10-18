using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    /// <summary>
    /// Information currently being edited in the Timeline Editor Window.
    /// </summary>
    public static class TimelineEditor
    {
        /// <summary>
        /// The PlayableDirector associated with the timeline currently being shown in the Timeline window.
        /// </summary>
        public static PlayableDirector inspectedDirector { get { return state == null ? null : state.editSequence.director; } }

        /// <summary>
        /// The PlayableDirector responsible for the playback of the timeline currently being shown in the Timeline window.
        /// </summary>
        public static PlayableDirector masterDirector { get { return state == null ? null : state.masterSequence.director; } }

        /// <summary>
        /// The TimelineAsset currently being shown in the Timeline window.
        /// </summary>
        public static TimelineAsset inspectedAsset { get { return state == null ? null : state.editSequence.asset; } }

        /// <summary>
        /// The TimelineAsset at the root of the hierarchy currently being shown in the Timeline window.
        /// </summary>
        public static TimelineAsset masterAsset { get { return state == null ? null : state.masterSequence.asset; } }

        /// <summary>
        /// The PlayableDirector currently being shown in the Timeline Editor Window.
        /// </summary>
        [Obsolete("playableDirector is ambiguous. Please select either inspectedDirector or masterDirector instead.", false)]
        public static PlayableDirector playableDirector { get { return inspectedDirector; } }

        /// <summary>
        /// The TimelineAsset currently being shown in the Timeline Editor Window.
        /// </summary>
        [Obsolete("timelineAsset is ambiguous. Please select either inspectedAsset or masterAsset instead.", false)]
        public static TimelineAsset timelineAsset { get { return inspectedAsset; } }


        /// <summary>
        /// <para>
        /// Refreshes the different components affected by the currently inspected
        /// <see cref="UnityEngine.Timeline.TimelineAsset"/>, based on the <see cref="RefreshReason"/> provided.
        /// </para>
        /// <para>
        /// For better performance, it is recommended that you invoke this method once, after you modify the
        /// <see cref="UnityEngine.Timeline.TimelineAsset"/>. You should also combine reasons using the <c>|</c> operator.
        /// </para>
        /// </summary>
        /// <remarks>
        /// Note: This operation is not synchronous. It is performed during the next GUI loop.
        /// </remarks>
        /// <param name="reason">The reason why a refresh should be performed.</param>
        public static void Refresh(RefreshReason reason)
        {
            if (state == null)
                return;

            if ((reason & RefreshReason.ContentsAddedOrRemoved) != 0)
            {
                state.Refresh();
            }
            else if ((reason & RefreshReason.ContentsModified) != 0)
            {
                state.rebuildGraph = true;
            }
            else if ((reason & RefreshReason.SceneNeedsUpdate) != 0)
            {
                state.Evaluate();
            }

            window.Repaint();
        }

        static TimelineWindow window { get { return TimelineWindow.instance; } }
        static WindowState state { get { return window == null ? null : window.state; } }

        internal static readonly Clipboard clipboard = new Clipboard();

        /// <summary>
        /// The list of clips selected in the TimelineEditor.
        /// </summary>
        public static TimelineClip[] selectedClips
        {
            get { return Selection.GetFiltered<EditorClip>(SelectionMode.Unfiltered).Select(e => e.clip).Where(x => x != null).ToArray();  }
            set
            {
                if (value == null || value.Length == 0)
                {
                    Selection.objects = null;
                }
                else
                {
                    var objects = new List<UnityEngine.Object>();
                    foreach (var clip in value)
                    {
                        if (clip == null)
                            continue;

                        var editorClip = EditorClipFactory.GetEditorClip(clip);
                        if (editorClip != null)
                            objects.Add(editorClip);
                    }

                    Selection.objects = objects.ToArray();
                }
            }
        }

        /// <summary>
        /// The clip selected in the TimelineEditor.
        /// </summary>
        /// <remarks>
        /// If there are multiple clips selected, this property returns the first clip.
        /// </remarks>
        public static TimelineClip selectedClip
        {
            get
            {
                var editorClip = Selection.activeObject as EditorClip;
                if (editorClip != null)
                    return editorClip.clip;
                return null;
            }
            set
            {
                var editorClip = (value != null) ? EditorClipFactory.GetEditorClip(value) : null;
                Selection.activeObject = editorClip;
            }
        }
    }


    /// <summary>
    /// <see cref="TimelineEditor.Refresh"/> uses these flags to determine what needs to be refreshed or updated.
    /// </summary>
    /// <remarks>
    /// Use the <c>|</c> operator to combine flags.
    /// <example>
    /// <c>TimelineEditor.Refresh(RefreshReason.ContentsModified | RefreshReason.SceneNeedsUpdate);</c>
    /// </example>
    /// </remarks>
    [Flags]
    public enum RefreshReason
    {
        /// <summary>
        /// Use this flag when a change to the Timeline requires that the Timeline window be redrawn.
        /// </summary>
        WindowNeedsRedraw        = 1 << 0,

        /// <summary>
        /// Use this flag when a change to the Timeline requires that the Scene be updated.
        /// </summary>
        SceneNeedsUpdate         = 1 << 1,

        /// <summary>
        /// Use this flag when a Timeline element was modified.
        /// </summary>
        ContentsModified         = 1 << 2,

        /// <summary>
        /// Use this flag when an element was added to or removed from the Timeline.
        /// </summary>
        ContentsAddedOrRemoved   = 1 << 3
    }
}
