namespace UnityEditor.TestTools.TestRunner.Api
{
    internal interface ITestLauncherFactory
    {
        TestLauncherBase GetLauncher(ExecutionSettings executionSettings);
    }
}