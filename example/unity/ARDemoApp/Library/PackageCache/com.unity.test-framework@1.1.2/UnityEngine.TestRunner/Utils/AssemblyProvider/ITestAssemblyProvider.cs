using NUnit.Framework.Interfaces;

namespace UnityEngine.TestTools.Utils
{
    internal interface ITestAssemblyProvider
    {
        ITest GetTestsWithNUnit();
        IAssemblyWrapper[] GetUserAssemblies();
    }
}
