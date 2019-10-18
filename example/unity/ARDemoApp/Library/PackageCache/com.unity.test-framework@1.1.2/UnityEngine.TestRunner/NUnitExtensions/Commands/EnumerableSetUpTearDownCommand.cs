using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;
using UnityEngine.TestRunner.NUnitExtensions.Runner;

namespace UnityEngine.TestTools
{
    internal class EnumerableSetUpTearDownCommand : BeforeAfterTestCommandBase<MethodInfo>
    {
        public EnumerableSetUpTearDownCommand(TestCommand innerCommand)
            : base(innerCommand, "SetUp", "TearDown")
        {
            if (Test.TypeInfo.Type != null)
            {
                BeforeActions = GetMethodsWithAttributeFromFixture(Test.TypeInfo.Type, typeof(UnitySetUpAttribute));
                AfterActions = GetMethodsWithAttributeFromFixture(Test.TypeInfo.Type, typeof(UnityTearDownAttribute)).Reverse().ToArray();
            }
        }

        private static MethodInfo[] GetMethodsWithAttributeFromFixture(Type fixtureType, Type setUpType)
        {
            MethodInfo[] methodsWithAttribute = Reflect.GetMethodsWithAttribute(fixtureType, setUpType, true);
            return methodsWithAttribute.Where(x => x.ReturnType == typeof(IEnumerator)).ToArray();
        }

        protected override IEnumerator InvokeBefore(MethodInfo action, Test test, UnityTestExecutionContext context)
        {
            return (IEnumerator)Reflect.InvokeMethod(action, context.TestObject);
        }

        protected override IEnumerator InvokeAfter(MethodInfo action, Test test, UnityTestExecutionContext context)
        {
            return (IEnumerator)Reflect.InvokeMethod(action, context.TestObject);
        }

        protected override BeforeAfterTestCommandState GetState(UnityTestExecutionContext context)
        {
            return context.SetUpTearDownState;
        }
    }
}
