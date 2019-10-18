using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Filters;
using UnityEngine;
using UnityEngine.TestRunner.NUnitExtensions.Filters;
using UnityEngine.TestTools.TestRunner.GUI;

namespace UnityEditor.TestTools.TestRunner.Api
{
    [Serializable]
    public class Filter
    {
        [SerializeField]
        public TestMode testMode;
        [SerializeField]
        public string[] testNames;
        [SerializeField]
        public string[] groupNames;
        [SerializeField]
        public string[] categoryNames;
        [SerializeField]
        public string[] assemblyNames;
        [SerializeField]
        public BuildTarget? targetPlatform;

        internal TestRunnerFilter ToTestRunnerFilter()
        {
            return new TestRunnerFilter()
            {
                testNames = testNames,
                categoryNames = categoryNames,
                groupNames = groupNames,
                assemblyNames = assemblyNames
            };
        }
        
        internal ITestFilter BuildNUnitFilter()
        {
            var filters = new List<ITestFilter>();

            if (testNames != null && testNames.Length != 0)
            {
                var nameFilter = new OrFilter(testNames.Select(n => new FullNameFilter(n)).ToArray());
                filters.Add(nameFilter);
            }

            if (groupNames != null && groupNames.Length != 0)
            {
                var exactNamesFilter = new OrFilter(groupNames.Select(n =>
                {
                    var f = new FullNameFilter(n);
                    f.IsRegex = true;
                    return f;
                }).ToArray());
                filters.Add(exactNamesFilter);
            }

            if (assemblyNames != null && assemblyNames.Length != 0)
            {
                var assemblyFilter = new OrFilter(assemblyNames.Select(c => new AssemblyNameFilter(c)).ToArray());
                filters.Add(assemblyFilter);
            }

            if (categoryNames != null && categoryNames.Length != 0)
            {
                var categoryFilter = new OrFilter(categoryNames.Select(c => new CategoryFilterExtended(c) {IsRegex = true}).ToArray());
                filters.Add(categoryFilter);
            }

            return filters.Count == 0 ? TestFilter.Empty : new AndFilter(filters.ToArray());
        }
    }
}
