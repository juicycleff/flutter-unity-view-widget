using System;

namespace UnityEditor.TestTools.TestRunner.Api
{
    [Flags]
    public enum TestMode
    {
        EditMode = 1 << 0,
        PlayMode = 1 << 1
    }
}
