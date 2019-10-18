using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Interfaces;
using UnityEngine.TestRunner.NUnitExtensions.Runner;

namespace UnityEngine.TestRunner.TestLaunchers
{
    [Serializable]
    internal class RemoteTestResultDataWithTestData
    {
        public RemoteTestResultData[] results;
        public RemoteTestData[] tests;
    }
}
