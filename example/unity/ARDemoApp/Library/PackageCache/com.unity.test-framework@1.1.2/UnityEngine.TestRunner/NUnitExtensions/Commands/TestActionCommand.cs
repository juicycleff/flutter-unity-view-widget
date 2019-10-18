using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;
using UnityEngine.TestRunner.NUnitExtensions.Runner;

namespace UnityEngine.TestTools
{
    internal class TestActionCommand : BeforeAfterTestCommandBase<ITestAction>
    {
        public TestActionCommand(TestCommand innerCommand)
            : base(innerCommand, "BeforeTest", "AfterTest", true)
        {
            if (Test.TypeInfo.Type != null)
            {
                BeforeActions = GetTestActionsFromMethod(Test.Method.MethodInfo);
                AfterActions = BeforeActions;
            }
        }

        private static ITestAction[] GetTestActionsFromMethod(MethodInfo method)
        {
            var attributes = method.GetCustomAttributes(false);
            List<ITestAction> actions = new List<ITestAction>();
            foreach (var attribute in attributes)
            {
                if (attribute is ITestAction)
                    actions.Add(attribute as ITestAction);
            }
            return actions.ToArray();
        }

        protected override IEnumerator InvokeBefore(ITestAction action, Test test, UnityTestExecutionContext context)
        {
            action.BeforeTest(test);
            yield return null;
        }

        protected override IEnumerator InvokeAfter(ITestAction action, Test test, UnityTestExecutionContext context)
        {
            action.AfterTest(test);
            yield return null;
        }

        protected override BeforeAfterTestCommandState GetState(UnityTestExecutionContext context)
        {
            return null;
        }
    }
}
