using System;
using NUnit.Framework;
using NUnit.Framework.Internal.Commands;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;
using UnityEngine.TestRunner.NUnitExtensions.Runner;

namespace UnityEngine.TestTools
{
    [AttributeUsage(AttributeTargets.Method)]
    public class UnityTestAttribute : CombiningStrategyAttribute, ISimpleTestBuilder, IImplyFixture
    {
        public UnityTestAttribute() : base(new UnityCombinatorialStrategy(), new ParameterDataSourceProvider()) {}

        private readonly NUnitTestCaseBuilder _builder = new NUnitTestCaseBuilder();

        TestMethod ISimpleTestBuilder.BuildFrom(IMethodInfo method, Test suite)
        {
            TestCaseParameters parms = new TestCaseParameters
            {
                ExpectedResult = new object(),
                HasExpectedResult = true
            };

            var t = _builder.BuildTestMethod(method, suite, parms);

            if (t.parms != null)
                t.parms.HasExpectedResult = false;
            return t;
        }
    }
}
