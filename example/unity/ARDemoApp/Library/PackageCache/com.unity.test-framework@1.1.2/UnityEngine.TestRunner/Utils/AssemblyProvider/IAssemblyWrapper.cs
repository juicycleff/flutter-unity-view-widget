using System.Reflection;

namespace UnityEngine.TestTools.Utils
{
    internal interface IAssemblyWrapper
    {
        Assembly Assembly { get; }
        string Location { get; }
        AssemblyName[] GetReferencedAssemblies();
    }
}
