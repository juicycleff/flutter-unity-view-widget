using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    abstract class RuntimeElement : IInterval
    {
        public abstract Int64 intervalStart { get; }
        public abstract Int64 intervalEnd { get; }
        public int intervalBit { get; set; }

        public abstract bool enable { set; }
        public abstract void EvaluateAt(double localTime, FrameData frameData);
    }
}
