using System;
using System.Collections;
using NUnit.Framework.Internal;
using UnityEngine.TestRunner.NUnitExtensions.Runner;

namespace UnityEngine.TestTools.Utils
{
    internal class CoroutineRunner
    {
        private bool m_Running;
        private bool m_TestFailed;
        private bool m_Timeout;
        private readonly MonoBehaviour m_Controller;
        private readonly UnityTestExecutionContext m_Context;
        private Coroutine m_TimeOutCoroutine;
        private IEnumerator m_TestCoroutine;

        internal const int k_DefaultTimeout = 1000 * 180;

        public CoroutineRunner(MonoBehaviour playmodeTestsController, UnityTestExecutionContext context)
        {
            m_Controller = playmodeTestsController;
            m_Context = context;
        }

        public IEnumerator HandleEnumerableTest(IEnumerator testEnumerator)
        {
            if (m_Context.TestCaseTimeout == 0)
            {
                m_Context.TestCaseTimeout = k_DefaultTimeout;
            }
            do
            {
                if (!m_Running)
                {
                    m_Running = true;
                    m_TestCoroutine = ExMethod(testEnumerator, m_Context.TestCaseTimeout);
                    m_Controller.StartCoroutine(m_TestCoroutine);
                }
                if (m_TestFailed)
                {
                    StopAllRunningCoroutines();
                    yield break;
                }

                if (m_Context.ExecutionStatus == TestExecutionStatus.StopRequested || m_Context.ExecutionStatus == TestExecutionStatus.AbortRequested)
                {
                    StopAllRunningCoroutines();
                    yield break;
                }
                yield return null;
            }
            while (m_Running);
        }

        private void StopAllRunningCoroutines()
        {
            if (m_TimeOutCoroutine != null)
            {
                m_Controller.StopCoroutine(m_TimeOutCoroutine);
            }

            if (m_TestCoroutine != null)
            {
                m_Controller.StopCoroutine(m_TestCoroutine);
            }
        }

        private IEnumerator ExMethod(IEnumerator e, int timeout)
        {
            m_TimeOutCoroutine = m_Controller.StartCoroutine(StartTimer(e, timeout,
                () =>
                {
                    m_TestFailed = true;
                    m_Timeout = true;
                    m_Running = false;
                }));

            yield return m_Controller.StartCoroutine(e);
            m_Controller.StopCoroutine(m_TimeOutCoroutine);
            m_Running = false;
        }

        private IEnumerator StartTimer(IEnumerator coroutineToBeKilled, int timeout, Action onTimeout)
        {
            yield return new WaitForSecondsRealtime(timeout / 1000f);
            if (coroutineToBeKilled != null)
                m_Controller.StopCoroutine(coroutineToBeKilled);
            if (onTimeout != null)
                onTimeout();
        }

        public bool HasFailedWithTimeout()
        {
            return m_Timeout;
        }

        public int GetDefaultTimeout()
        {
            return k_DefaultTimeout;
        }
    }
}
