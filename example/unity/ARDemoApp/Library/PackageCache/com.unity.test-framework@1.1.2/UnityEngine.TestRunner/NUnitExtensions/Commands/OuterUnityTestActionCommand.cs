using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;
using UnityEngine.TestRunner.NUnitExtensions.Runner;

namespace UnityEngine.TestTools
{
    internal class OuterUnityTestActionCommand : BeforeAfterTestCommandBase<IOuterUnityTestAction>
    {
        public OuterUnityTestActionCommand(TestCommand innerCommand)
            : base(innerCommand, "BeforeTest", "AfterTest")
        {
            if (Test.TypeInfo.Type != null)
            {
                BeforeActions = GetUnityTestActionsFromMethod(Test.Method.MethodInfo);
                AfterActions = BeforeActions;
            }
        }

        private static IOuterUnityTestAction[] GetUnityTestActionsFromMethod(MethodInfo method)
        {
            var attributes = method.GetCustomAttributes(false);
            List<IOuterUnityTestAction> actions = new List<IOuterUnityTestAction>();
            foreach (var attribute in attributes)
            {
                if (attribute is IOuterUnityTestAction)
                    actions.Add(attribute as IOuterUnityTestAction);
            }
            return actions.ToArray();
        }

        protected override IEnumerator InvokeBefore(IOuterUnityTestAction action, Test test, UnityTestExecutionContext context)
        {
            return action.BeforeTest(test);
        }

        protected override IEnumerator InvokeAfter(IOuterUnityTestAction action, Test test, UnityTestExecutionContext context)
        {
            return action.AfterTest(test);
        }

        protected override BeforeAfterTestCommandState GetState(UnityTestExecutionContext context)
        {
            return context.OuterUnityTestActionState;
        }
    }
}
