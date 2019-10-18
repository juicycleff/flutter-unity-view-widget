using System;

namespace UnityEditor.TestTools.TestRunner.Api
{
    public class ExecutionSettings
    {
        public ExecutionSettings(params Filter[] filtersToExecute)
        {
            filters = filtersToExecute;
        }
        
        internal BuildTarget? targetPlatform;
        public ITestRunSettings overloadTestRunSettings;
        internal Filter filter;
        public Filter[] filters;
    }
}
