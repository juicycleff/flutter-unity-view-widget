using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using UnityEngine.TestRunner.TestLaunchers;
using UnityEngine.TestTools;

namespace UnityEditor.TestTools.TestRunner
{
    interface ITestListCacheData
    {
        List<TestPlatform> platforms { get; }
        List<ITest> cachedData { get; }
    }
}
