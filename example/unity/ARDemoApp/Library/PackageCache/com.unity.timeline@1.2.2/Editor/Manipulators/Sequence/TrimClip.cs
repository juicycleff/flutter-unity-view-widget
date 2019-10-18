using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    class TrimClip : Manipulator
    {
        class TrimClipAttractionHandler : IAttractionHandler
        {
            public void OnAttractedEdge(IAttractable attractable, ManipulateEdges manipulateEdges, AttractedEdge edge, double time)
            {
                var clipGUI = attractable as TimelineClipGUI;
                if (clipGUI == null)
                    return;

                var clipItem = ItemsUtils.ToItem(clipGUI.clip);
                if (manipulateEdges == ManipulateEdges.Right)
                {
                    bool affectTimeScale = Event.current.modifiers == EventModifiers.Shift; // TODO Do not use Event.current from here.
                    EditMode.TrimEnd(clipItem, time, affectTimeScale);
                }
                else if (manipulateEdges == ManipulateEdges.Left)
                {
                    EditMode.TrimStart(clipItem, time);
                }
            }
        }

        bool m_IsCaptured;
        TimelineClipHandle m_TrimClipHandler;

        double m_OriginalDuration;
        double m_OriginalTimeScale;
        bool m_UndoSaved;
        SnapEngine m_SnapEngine;

        readonly StringBuilder m_OverlayText = new StringBuilder();
        readonly List<string> m_OverlayStrings = new List<string>();

        static readonly double kEpsilon = 0.0000001;

        protected override bool MouseDown(Event evt, WindowState state)
        {
            var handle = PickerUtils.PickedLayerableOfType<TimelineClipHandle>();
            if (handle == null)
                return false;

            if (handle.clipGUI.clip.parentTrack != null && handle.clipGUI.clip.parentTrack.lockedInHierarchy)
                return false;

            if (ItemSelection.CanClearSelection(evt))
                SelectionManager.Clear();

            if (!SelectionManager.Contains(handle.clipGUI.clip))
                SelectionManager.Add(handle.clipGUI.clip);

            m_TrimClipHandler = handle;

            m_IsCaptured = true;
            state.AddCaptured(this);

            m_UndoSaved = false;

            var clip = m_TrimClipHandler.clipGUI.clip;

            m_OriginalDuration = clip.duration;
            m_OriginalTimeScale = clip.timeScale;

            RefreshOverlayStrings(m_TrimClipHandler, state);

            // in ripple trim, the right edge moves and needs to snap
            var edges = ManipulateEdges.Right;
            if (EditMode.editType != EditMode.EditType.Ripple && m_TrimClipHandler.trimDirection == TrimEdge.Start)
                edges = ManipulateEdges.Left;
            m_SnapEngine = new SnapEngine(m_TrimClipHandler.clipGUI, new TrimClipAttractionHandler(), edges, state,
                evt.mousePosition);

            EditMode.BeginTrim(ItemsUtils.ToItem(clip), m_TrimClipHandler.trimDirection);

            return true;
        }

        protected override bool MouseUp(Event evt, WindowState state)
        {
            if (!m_IsCaptured)
                return false;

            m_IsCaptured = false;
            m_TrimClipHandler = null;
            m_UndoSaved = false;
            m_SnapEngine = null;
            EditMode.FinishTrim();

            state.captured.Clear();

            return true;
        }

        protected override bool MouseDrag(Event evt, WindowState state)
        {
            if (state.editSequence.isReadOnly)
                return false;

            if (!m_IsCaptured)
                return false;

            if (!m_UndoSaved)
            {
                var uiClip = m_TrimClipHandler.clipGUI;
                TimelineUndo.PushUndo(uiClip.clip.parentTrack, "Trim Clip");
                if (TimelineUtility.IsRecordableAnimationClip(uiClip.clip))
                {
                    TimelineUndo.PushUndo(uiClip.clip.animationClip, "Trim Clip");
                }

                m_UndoSaved = true;
            }

            if (m_SnapEngine != null)
                m_SnapEngine.Snap(evt.mousePosition, evt.modifiers);

            RefreshOverlayStrings(m_TrimClipHandler, state);

            if (Selection.activeObject != null)
                EditorUtility.SetDirty(Selection.activeObject);

            // updates the duration of the graph without rebuilding
            state.UpdateRootPlayableDuration(state.editSequence.duration);

            return true;
        }

        public override void Overlay(Event evt, WindowState state)
        {
            if (!m_IsCaptured)
                return;

            EditMode.DrawTrimGUI(state, m_TrimClipHandler.clipGUI, m_TrimClipHandler.trimDirection);

            bool trimStart = m_TrimClipHandler.trimDirection == TrimEdge.Start;

            TimeIndicator.Draw(state, trimStart ? m_TrimClipHandler.clipGUI.start : m_TrimClipHandler.clipGUI.end);

            if (m_SnapEngine != null)
                m_SnapEngine.OnGUI(trimStart, !trimStart);

            if (m_OverlayStrings.Count > 0)
            {
                const float padding = 4.0f;
                var labelStyle = TimelineWindow.styles.tinyFont;
                var longestLine = labelStyle.CalcSize(
                    new GUIContent(m_OverlayStrings.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur)));
                var stringLength = longestLine.x + padding;
                var lineHeight = longestLine.y + padding;

                var r = new Rect(evt.mousePosition.x - (stringLength / 2.0f),
                    m_TrimClipHandler.clipGUI.rect.yMax,
                    stringLength, lineHeight);

                foreach (var s in m_OverlayStrings)
                {
                    GUI.Label(r, s, labelStyle);
                    r.y += lineHeight;
                }
            }
        }

        void RefreshOverlayStrings(TimelineClipHandle handle, WindowState state)
        {
            m_OverlayStrings.Clear();

            m_OverlayText.Length = 0;

            var differenceDuration = handle.clipGUI.clip.duration - m_OriginalDuration;
            bool hasDurationDelta = Math.Abs(differenceDuration) > kEpsilon;

            if (state.timeInFrames)
            {
                var durationInFrame = handle.clipGUI.clip.duration * state.referenceSequence.frameRate;
                m_OverlayText.Append("duration: ").Append(durationInFrame.ToString("f2")).Append(" frames");

                if (hasDurationDelta)
                {
                    m_OverlayText.Append(" (");

                    if (differenceDuration > 0.0)
                        m_OverlayText.Append("+");

                    var valueInFrame = differenceDuration * state.referenceSequence.frameRate;
                    m_OverlayText.Append(valueInFrame.ToString("f2")).Append(" frames)");
                }
            }
            else
            {
                m_OverlayText.Append("duration: ").Append(handle.clipGUI.clip.duration.ToString("f2")).Append("s");

                if (hasDurationDelta)
                {
                    m_OverlayText.Append(" (");

                    if (differenceDuration > 0.0)
                        m_OverlayText.Append("+");

                    m_OverlayText.Append(differenceDuration.ToString("f2")).Append("s)");
                }
            }

            m_OverlayStrings.Add(m_OverlayText.ToString());

            m_OverlayText.Length = 0;

            var differenceSpeed = m_OriginalTimeScale - handle.clipGUI.clip.timeScale;
            if (Math.Abs(differenceSpeed) > kEpsilon)
            {
                m_OverlayText.Append("speed: ").Append(handle.clipGUI.clip.timeScale.ToString("p2"));

                m_OverlayText.Append(" (");

                if (differenceSpeed > 0.0)
                    m_OverlayText.Append("+");

                m_OverlayText.Append(differenceSpeed.ToString("p2")).Append(")");

                m_OverlayStrings.Add(m_OverlayText.ToString());
            }
        }
    }
}
