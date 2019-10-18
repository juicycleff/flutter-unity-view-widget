using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;
using UnityEngine.TestTools.TestRunner.GUI;

namespace UnityEditor.TestTools.TestRunner
{
    internal class RerunCallbackData : ScriptableSingleton<RerunCallbackData>
    {
        [SerializeField]
        internal TestRunnerFilter[] runFilters;

        [SerializeField]
        internal TestMode testMode;
    }
}
