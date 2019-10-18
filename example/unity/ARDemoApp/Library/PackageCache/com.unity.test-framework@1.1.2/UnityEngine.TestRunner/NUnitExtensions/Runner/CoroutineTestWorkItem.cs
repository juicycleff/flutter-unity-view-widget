using System;
using System.Collections;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;
using NUnit.Framework.Internal.Execution;
using UnityEngine.TestTools.Utils;

namespace UnityEngine.TestRunner.NUnitExtensions.Runner
{
    internal class CoroutineTestWorkItem : UnityWorkItem
    {
        private static MonoBehaviour m_MonoBehaviourCoroutineRunner;
        private TestCommand m_Command;

        public static MonoBehaviour monoBehaviourCoroutineRunner
        {
            get
            {
                if (m_MonoBehaviourCoroutineRunner == null)
                {
                    throw new NullReferenceException("MonoBehaviour coroutine runner not set");
                }
                return m_MonoBehaviourCoroutineRunner;
            }
            set { m_MonoBehaviourCoroutineRunner = value; }
        }

        public CoroutineTestWorkItem(TestMethod test, ITestFilter filter)
            : base(test, null)
        {
            m_Command = m_Command = TestCommandBuilder.BuildTestCommand(test, filter);
        }

        protected override IEnumerable PerformWork()
        {
            if (m_Command is SkipCommand)
            {
                m_Command.Execute(Context);
                Result = Context.CurrentResult;
                WorkItemComplete();
                yield break;
            }

            if (m_Command is ApplyChangesToContextCommand)
            {
                var applyChangesToContextCommand = (ApplyChangesToContextCommand)m_Command;
                applyChangesToContextCommand.ApplyChanges(Context);
                m_Command = applyChangesToContextCommand.GetInnerCommand();
            }

            var enumerableTestMethodCommand = (IEnumerableTestMethodCommand)m_Command;
            try
            {
                var executeEnumerable = enumerableTestMethodCommand.ExecuteEnumerable(Context).GetEnumerator();

                var coroutineRunner = new CoroutineRunner(monoBehaviourCoroutineRunner, Context);
                yield return coroutineRunner.HandleEnumerableTest(executeEnumerable);

                if (coroutineRunner.HasFailedWithTimeout())
                {
                    Context.CurrentResult.SetResult(ResultState.Failure, string.Format("Test exceeded Timeout value of {0}ms", Context.TestCaseTimeout));
                }

                while (executeEnumerable.MoveNext()) {}

                Result = Context.CurrentResult;
            }
            finally
            {
                WorkItemComplete();
            }
        }
    }
}
