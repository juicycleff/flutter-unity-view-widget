using System;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    interface ITimelineItem : IEquatable<ITimelineItem>
    {
        double start { get; set; }
        double end { get; }
        double duration { get; }

        TrackAsset parentTrack { get; set; }
        bool IsCompatibleWithTrack(TrackAsset track);

        void Delete();
        ITimelineItem CloneTo(TrackAsset parent, double time);
        void PushUndo(string operation);

        TimelineItemGUI gui { get; }
    }

    interface ITrimmable : ITimelineItem
    {
        void SetStart(double time);
        void SetEnd(double time, bool affectTimeScale);
        void TrimStart(double time);
        void TrimEnd(double time);
    }

    interface IBlendable : ITimelineItem
    {
        bool supportsBlending { get; }
        bool hasLeftBlend { get; }
        bool hasRightBlend { get; }

        double leftBlendDuration { get; }
        double rightBlendDuration { get; }
    }
}
