using System;

namespace UnityEditor.Timeline
{
    [AttributeUsage(AttributeTargets.Class)]
    class ActiveInModeAttribute : Attribute
    {
        public TimelineModes modes { get; private set; }
        public ActiveInModeAttribute(TimelineModes timelineModes)
        {
            modes = timelineModes;
        }
    }
}
