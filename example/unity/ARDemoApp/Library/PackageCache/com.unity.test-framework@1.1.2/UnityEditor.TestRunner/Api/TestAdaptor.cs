using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using UnityEngine.TestRunner.NUnitExtensions;
using UnityEngine.TestRunner.NUnitExtensions.Runner;
using UnityEngine.TestRunner.TestLaunchers;
using UnityEngine.TestTools.Utils;

namespace UnityEditor.TestTools.TestRunner.Api
{
    internal class TestAdaptor : ITestAdaptor
    {
        internal TestAdaptor(ITest test, ITestAdaptor[] children = null)
        {
            Id = test.Id;
            Name = test.Name;
            var childIndex = -1;
            if (test.Properties["childIndex"].Count > 0)
            {
                childIndex = (int)test.Properties["childIndex"][0];
            }
            FullName = childIndex != -1 ? GetIndexedTestCaseName(test.FullName, childIndex) : test.FullName;
            TestCaseCount = test.TestCaseCount;
            HasChildren = test.HasChildren;
            IsSuite = test.IsSuite;
            if (UnityTestExecutionContext.CurrentContext != null)
            {
                TestCaseTimeout = UnityTestExecutionContext.CurrentContext.TestCaseTimeout;
            }
            else
            {
                TestCaseTimeout = CoroutineRunner.k_DefaultTimeout;
            }

            TypeInfo = test.TypeInfo;
            Method = test.Method;
            Categories = test.GetAllCategoriesFromTest().Distinct().ToArray();
            IsTestAssembly = test is TestAssembly;
            RunState = (RunState)Enum.Parse(typeof(RunState), test.RunState.ToString());
            Description = (string)test.Properties.Get(PropertyNames.Description);
            SkipReason = test.GetSkipReason();
            ParentId = test.GetParentId();
            ParentFullName = test.GetParentFullName();
            UniqueName = test.GetUniqueName();
            ParentUniqueName = test.GetParentUniqueName();
            ChildIndex = childIndex;
            
            if (test.Parent != null)
            {
                if (test.Parent.Parent == null) // Assembly level
                {
                    TestMode = (TestMode)Enum.Parse(typeof(TestMode),test.Properties.Get("platform").ToString());        
                }
            }

            Children = children;
        }

        public void SetParent(ITestAdaptor parent)
        {
            Parent = parent;
            if (parent != null)
            {
                TestMode = parent.TestMode;
            }
        }

        internal TestAdaptor(RemoteTestData test)
        {
            Id = test.id;
            Name = test.name;
            FullName = test.ChildIndex != -1 ? GetIndexedTestCaseName(test.fullName, test.ChildIndex) : test.fullName;
            TestCaseCount = test.testCaseCount;
            HasChildren = test.hasChildren;
            IsSuite = test.isSuite;
            m_ChildrenIds = test.childrenIds;
            TestCaseTimeout = test.testCaseTimeout;
            Categories = test.Categories;
            IsTestAssembly = test.IsTestAssembly;
            RunState = (RunState)Enum.Parse(typeof(RunState), test.RunState.ToString());
            Description = test.Description;
            SkipReason = test.SkipReason;
            ParentId = test.ParentId;
            UniqueName = test.UniqueName;
            ParentUniqueName = test.ParentUniqueName;
            ParentFullName = test.ParentFullName;
            ChildIndex = test.ChildIndex;
            TestMode = TestMode.PlayMode;
        }

        internal void ApplyChildren(IEnumerable<TestAdaptor> allTests)
        {
            Children = m_ChildrenIds.Select(id => allTests.First(t => t.Id == id)).ToArray();
            if (!string.IsNullOrEmpty(ParentId))
            {
                Parent = allTests.FirstOrDefault(t => t.Id == ParentId);
            }
        }

        public string Id { get; private set; }
        public string Name { get; private set; }
        public string FullName { get; private set; }
        public int TestCaseCount { get; private set; }
        public bool HasChildren { get; private set; }
        public bool IsSuite { get; private set; }
        public IEnumerable<ITestAdaptor> Children { get; private set; }
        public ITestAdaptor Parent { get; private set; }
        public int TestCaseTimeout { get; private set; }
        public ITypeInfo TypeInfo { get; private set; }
        public IMethodInfo Method { get; private set; }
        private string[] m_ChildrenIds;
        public string[] Categories { get; private set; }
        public bool IsTestAssembly { get; private set; }
        public RunState RunState { get; }
        public string Description { get; }
        public string SkipReason { get; }
        public string ParentId { get; }
        public string ParentFullName { get; }
        public string UniqueName { get; }
        public string ParentUniqueName { get; }
        public int ChildIndex { get; }
        public TestMode TestMode { get; private set; }
        
        private static string GetIndexedTestCaseName(string fullName, int index)
        {
            var generatedTestSuffix = " GeneratedTestCase" + index;
            if (fullName.EndsWith(")"))
            {
                // Test names from generated TestCaseSource look like Test(TestCaseSourceType)
                // This inserts a unique test case index in the name, so that it becomes Test(TestCaseSourceType GeneratedTestCase0)
                return fullName.Substring(0, fullName.Length - 1) + generatedTestSuffix + fullName[fullName.Length - 1];
            }

            // In some cases there can be tests with duplicate names generated in other ways and they won't have () in their name
            // We just append a suffix at the end of the name in that case
            return fullName + generatedTestSuffix;
        }
    }
}
