using System;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace UnityEditor.TestTools
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
    internal class RequireApiProfileAttribute : NUnitAttribute, IApplyToTest
    {
        public ApiCompatibilityLevel[] apiProfiles { get; private set; }

        public RequireApiProfileAttribute(params ApiCompatibilityLevel[] apiProfiles)
        {
            this.apiProfiles = apiProfiles;
        }

        void IApplyToTest.ApplyToTest(Test test)
        {
            test.Properties.Add(PropertyNames.Category, string.Format("ApiProfile({0})", string.Join(", ", apiProfiles.Select(p => p.ToString()).OrderBy(p => p).ToArray())));
            ApiCompatibilityLevel testProfile = PlayerSettings.GetApiCompatibilityLevel(EditorUserBuildSettings.activeBuildTargetGroup);

            if (!apiProfiles.Contains(testProfile))
            {
                string skipReason = "Skipping test as it requires a compatible api profile set: " + string.Join(", ", apiProfiles.Select(p => p.ToString()).ToArray());
                test.RunState = RunState.Skipped;
                test.Properties.Add(PropertyNames.SkipReason, skipReason);
            }
        }
    }
}
