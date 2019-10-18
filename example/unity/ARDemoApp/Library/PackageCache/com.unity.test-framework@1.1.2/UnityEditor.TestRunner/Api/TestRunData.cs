using System;
using UnityEngine;

namespace UnityEditor.TestTools.TestRunner.Api
{
    [Serializable]
    internal class TestRunData : ScriptableSingleton<TestRunData>
    {
        [SerializeField]
        public ExecutionSettings executionSettings;
    }
}
