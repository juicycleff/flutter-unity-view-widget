using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework.Interfaces;
using UnityEngine.TestTools.NUnitExtensions;

namespace UnityEngine.TestTools.Utils
{
    internal class PlayerTestAssemblyProvider
    {
        private IAssemblyLoadProxy m_AssemblyLoadProxy;
        private readonly List<string> m_AssembliesToLoad;

        //Cached until domain reload
        private static List<IAssemblyWrapper> m_LoadedAssemblies;

        internal PlayerTestAssemblyProvider(IAssemblyLoadProxy assemblyLoadProxy, List<string> assembliesToLoad)
        {
            m_AssemblyLoadProxy = assemblyLoadProxy;
            m_AssembliesToLoad = assembliesToLoad;
            LoadAssemblies();
        }

        public ITest GetTestsWithNUnit()
        {
            return BuildTests(TestPlatform.PlayMode, m_LoadedAssemblies.ToArray());
        }

        public List<IAssemblyWrapper> GetUserAssemblies()
        {
            return m_LoadedAssemblies;
        }

        protected static ITest BuildTests(TestPlatform testPlatform, IAssemblyWrapper[] assemblies)
        {
            var settings = UnityTestAssemblyBuilder.GetNUnitTestBuilderSettings(testPlatform);
            var builder = new UnityTestAssemblyBuilder();
            return builder.Build(assemblies.Select(a => a.Assembly).ToArray(), Enumerable.Repeat(testPlatform, assemblies.Length).ToArray(), settings);
        }

        private void LoadAssemblies()
        {
            if (m_LoadedAssemblies != null)
            {
                return;
            }

            m_LoadedAssemblies = new List<IAssemblyWrapper>();

            foreach (var userAssembly in m_AssembliesToLoad)
            {
                IAssemblyWrapper a;
                try
                {
                    a = m_AssemblyLoadProxy.Load(userAssembly);
                }
                catch (FileNotFoundException)
                {
                    continue;
                }
                if (a != null)
                    m_LoadedAssemblies.Add(a);
            }
        }
    }
}
