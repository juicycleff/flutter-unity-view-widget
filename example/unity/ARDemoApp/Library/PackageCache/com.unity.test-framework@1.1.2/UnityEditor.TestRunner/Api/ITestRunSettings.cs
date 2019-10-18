using System;

namespace UnityEditor.TestTools.TestRunner.Api
{
    public interface ITestRunSettings : IDisposable
    {
        void Apply();
    }
}
