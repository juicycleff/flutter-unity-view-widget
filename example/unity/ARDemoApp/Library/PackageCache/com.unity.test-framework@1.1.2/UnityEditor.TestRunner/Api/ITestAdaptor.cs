using System.Collections.Generic;
using NUnit.Framework.Interfaces;

namespace UnityEditor.TestTools.TestRunner.Api
{
    public interface ITestAdaptor
    {
        string Id { get; }
        string Name { get; }
        string FullName { get; }
        int TestCaseCount { get; }
        bool HasChildren { get; }
        bool IsSuite { get; }
        IEnumerable<ITestAdaptor> Children { get; }
        ITestAdaptor Parent { get; }
        int TestCaseTimeout { get; }
        ITypeInfo TypeInfo { get; }
        IMethodInfo Method { get; }
        string[] Categories { get; }
        bool IsTestAssembly { get; }
        RunState RunState { get; }
        string Description { get; }
        string SkipReason { get; }
        string ParentId { get; }
        string ParentFullName { get; }
        string UniqueName { get; }
        string ParentUniqueName { get; }
        int ChildIndex { get; }
        TestMode TestMode { get; }
    }
}
