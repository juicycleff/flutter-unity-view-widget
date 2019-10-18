using UnityEngine;

namespace UnityEditor.Timeline
{
    static class ManipulatorsUtils
    {
        public static EventModifiers actionModifier
        {
            get
            {
                if (Application.platform == RuntimePlatform.OSXEditor ||
                    Application.platform == RuntimePlatform.OSXPlayer)
                    return EventModifiers.Command;

                return EventModifiers.Control;
            }
        }
    }
}
