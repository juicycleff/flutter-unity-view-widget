using System;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    class ClipItem : IBlendable, ITrimmable
    {
        readonly TimelineClip m_Clip;

        public TimelineClip clip
        {
            get { return m_Clip; }
        }

        public ClipItem(TimelineClip clip)
        {
            m_Clip = clip;
        }

        public TrackAsset parentTrack
        {
            get { return m_Clip.parentTrack; }
            set { m_Clip.parentTrack = value; }
        }

        public double start
        {
            get { return m_Clip.start; }
            set { m_Clip.start = value; }
        }

        public double end
        {
            get { return m_Clip.end; }
        }

        public double duration
        {
            get { return m_Clip.duration; }
        }

        public bool IsCompatibleWithTrack(TrackAsset track)
        {
            return track.IsCompatibleWithClip(m_Clip);
        }

        public void PushUndo(string operation)
        {
            TimelineUndo.PushUndo(m_Clip.parentTrack, operation);
        }

        public TimelineItemGUI gui
        {
            get { return ItemToItemGui.GetGuiForClip(m_Clip); }
        }

        public bool supportsBlending
        {
            get { return m_Clip.SupportsBlending(); }
        }

        public bool hasLeftBlend
        {
            get { return m_Clip.hasBlendIn; }
        }

        public bool hasRightBlend
        {
            get { return m_Clip.hasBlendOut; }
        }

        public double leftBlendDuration
        {
            get { return m_Clip.hasBlendIn ? m_Clip.blendInDuration : m_Clip.easeInDuration; }
        }

        public double rightBlendDuration
        {
            get { return m_Clip.hasBlendOut ? m_Clip.blendOutDuration : m_Clip.easeOutDuration; }
        }

        public void SetStart(double time)
        {
            ClipModifier.SetStart(m_Clip, time);
        }

        public void SetEnd(double time, bool affectTimeScale)
        {
            ClipModifier.SetEnd(m_Clip, time, affectTimeScale);
        }

        public void Delete()
        {
            EditorClipFactory.RemoveEditorClip(m_Clip);
            ClipModifier.Delete(m_Clip.parentTrack.timelineAsset, m_Clip);
        }

        public void TrimStart(double time)
        {
            ClipModifier.TrimStart(m_Clip, time);
        }

        public void TrimEnd(double time)
        {
            ClipModifier.TrimEnd(m_Clip, time);
        }

        public ITimelineItem CloneTo(TrackAsset parent, double time)
        {
            return new ClipItem(TimelineHelpers.Clone(m_Clip, TimelineEditor.inspectedDirector, TimelineEditor.inspectedDirector, time, parent));
        }

        public override string ToString()
        {
            return m_Clip.ToString();
        }

        public bool Equals(ClipItem otherClip)
        {
            if (ReferenceEquals(null, otherClip)) return false;
            if (ReferenceEquals(this, otherClip)) return true;
            return Equals(m_Clip, otherClip.m_Clip);
        }

        public override int GetHashCode()
        {
            return (m_Clip != null ? m_Clip.GetHashCode() : 0);
        }

        public bool Equals(ITimelineItem other)
        {
            return Equals((object)other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as ClipItem;
            return other != null && Equals(other);
        }
    }
}
