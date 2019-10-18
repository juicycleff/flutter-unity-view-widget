using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace UnityEngine.TestTools
{
    internal class BeforeAfterTestCommandState : ScriptableObject
    {
        public int NextBeforeStepIndex;
        public int NextBeforeStepPc;
        public int NextAfterStepIndex;
        public int NextAfterStepPc;
        public bool TestHasRun;
        public TestStatus CurrentTestResultStatus;
        public string CurrentTestResultLabel;
        public FailureSite CurrentTestResultSite;
        public string CurrentTestMessage;
        public string CurrentTestStrackTrace;
        public bool TestAfterStarted;

        public void Reset()
        {
            NextBeforeStepIndex = 0;
            NextBeforeStepPc = 0;
            NextAfterStepIndex = 0;
            NextAfterStepPc = 0;
            TestHasRun = false;
            CurrentTestResultStatus = TestStatus.Inconclusive;
            CurrentTestResultLabel = null;
            CurrentTestResultSite = default(FailureSite);
            CurrentTestMessage = null;
            CurrentTestStrackTrace = null;
            TestAfterStarted = false;
        }

        public void StoreTestResult(TestResult result)
        {
            CurrentTestResultStatus = result.ResultState.Status;
            CurrentTestResultLabel = result.ResultState.Label;
            CurrentTestResultSite = result.ResultState.Site;
            CurrentTestMessage = result.Message;
            CurrentTestStrackTrace = result.StackTrace;
        }

        public void ApplyTestResult(TestResult result)
        {
            result.SetResult(new ResultState(CurrentTestResultStatus, CurrentTestResultLabel, CurrentTestResultSite), CurrentTestMessage, CurrentTestStrackTrace);
        }
    }
}
