using System.Linq;
using UnityEditor.Build;

namespace UnityEditor.TestRunner
{
    internal class TestBuildAssemblyFilter : IFilterBuildAssemblies
    {
        private const string nunitAssemblyName = "nunit.framework";
        private const string unityTestRunnerAssemblyName = "UnityEngine.TestRunner";

        public int callbackOrder { get; }
        public string[] OnFilterAssemblies(BuildOptions buildOptions, string[] assemblies)
        {
            if ((buildOptions & BuildOptions.IncludeTestAssemblies) == BuildOptions.IncludeTestAssemblies || PlayerSettings.playModeTestRunnerEnabled)
            {
                return assemblies;
            }
            return assemblies.Where(x => !x.Contains(nunitAssemblyName) && !x.Contains(unityTestRunnerAssemblyName)).ToArray();
        }
    }
}
