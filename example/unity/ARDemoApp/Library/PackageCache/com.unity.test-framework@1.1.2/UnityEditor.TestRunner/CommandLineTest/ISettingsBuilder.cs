using UnityEditor.TestTools.TestRunner.Api;

namespace UnityEditor.TestTools.TestRunner.CommandLineTest
{
    interface ISettingsBuilder
    {
        Api.ExecutionSettings BuildApiExecutionSettings(string[] commandLineArgs);
        ExecutionSettings BuildExecutionSettings(string[] commandLineArgs);
    }
}
