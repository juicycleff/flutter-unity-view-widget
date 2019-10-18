namespace UnityEditor.TestTools.TestRunner.Api
{
    public interface ICallbacks
    {
        void RunStarted(ITestAdaptor testsToRun);
        void RunFinished(ITestResultAdaptor result);
        void TestStarted(ITestAdaptor test);
        void TestFinished(ITestResultAdaptor result);
    }
}
