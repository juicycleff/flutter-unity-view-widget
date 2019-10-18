using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;
using NUnit.Framework.Internal.Execution;
using UnityEngine.TestRunner.NUnitExtensions.Runner;

namespace UnityEngine.TestTools
{
    internal class SetUpTearDownCommand : BeforeAfterTestCommandBase<MethodInfo>
    {
        public SetUpTearDownCommand(TestCommand innerCommand)
            : base(innerCommand, "SetUp", "TearDown", true)
        {
            if (Test.TypeInfo.Type != null)
            {
                BeforeActions = GetMethodsWithAttributeFromFixture(Test.TypeInfo.Type, typeof(SetUpAttribute));
                AfterActions = GetMethodsWithAttributeFromFixture(Test.TypeInfo.Type, typeof(TearDownAttribute)).Reverse().ToArray();
            }
        }

        private static MethodInfo[] GetMethodsWithAttributeFromFixture(Type fixtureType, Type setUpType)
        {
            MethodInfo[] methodsWithAttribute = Reflect.GetMethodsWithAttribute(fixtureType, setUpType, true);
            return methodsWithAttribute.Where(x => x.ReturnType == typeof(void)).ToArray();
        }

        protected override IEnumerator InvokeBefore(MethodInfo action, Test test, UnityTestExecutionContext context)
        {
            Reflect.InvokeMethod(action, context.TestObject);
            yield return null;
        }

        protected override IEnumerator InvokeAfter(MethodInfo action, Test test, UnityTestExecutionContext context)
        {
            Reflect.InvokeMethod(action, context.TestObject);
            yield return null;
        }

        protected override BeforeAfterTestCommandState GetState(UnityTestExecutionContext context)
        {
            return null;
        }
    }
}
