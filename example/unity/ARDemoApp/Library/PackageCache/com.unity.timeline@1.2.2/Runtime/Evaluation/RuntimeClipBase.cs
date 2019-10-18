using System;
using UnityEngine;

namespace UnityEngine.Timeline
{
    internal abstract class RuntimeClipBase : RuntimeElement
    {
        public abstract double start { get; }
        public abstract double duration { get; }

        public override Int64 intervalStart
        {
            get { return DiscreteTime.GetNearestTick(start); }
        }

        public override Int64 intervalEnd
        {
            get { return DiscreteTime.GetNearestTick(start + duration); }
        }
    }
}
