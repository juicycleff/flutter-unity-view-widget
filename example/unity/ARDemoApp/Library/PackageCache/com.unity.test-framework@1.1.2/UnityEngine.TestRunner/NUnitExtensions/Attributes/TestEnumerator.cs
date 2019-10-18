using System;
using System.Collections;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace UnityEngine.TestTools
{
    internal class TestEnumerator
    {
        private readonly ITestExecutionContext m_Context;
        private static IEnumerator m_TestEnumerator;

        public static IEnumerator Enumerator { get { return m_TestEnumerator; } }

        public TestEnumerator(ITestExecutionContext context, IEnumerator testEnumerator)
        {
            m_Context = context;
            m_TestEnumerator = testEnumerator;
        }

        public IEnumerator Execute()
        {
            m_Context.CurrentResult.SetResult(ResultState.Success);

            while (true)
            {
                object current = null;
                try
                {
                    if (!m_TestEnumerator.MoveNext())
                    {
                        yield break;
                    }

                    if (!m_Context.CurrentResult.ResultState.Equals(ResultState.Success))
                    {
                        yield break;
                    }

                    current = m_TestEnumerator.Current;
                }
                catch (Exception exception)
                {
                    m_Context.CurrentResult.RecordException(exception);
                    yield break;
                }
                yield return current;
            }
        }
    }
}
