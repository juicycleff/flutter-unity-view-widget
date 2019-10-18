using System;

namespace UnityEditor.TestTools.TestRunner
{
    internal interface ITestSettings : IDisposable
    {
        ScriptingImplementation? scriptingBackend { get; set; }

        string Architecture { get; set; }

        ApiCompatibilityLevel? apiProfile { get; set; }

        bool? appleEnableAutomaticSigning { get; set; }
        string appleDeveloperTeamID { get; set; }
        ProvisioningProfileType? iOSManualProvisioningProfileType { get; set; }
        string iOSManualProvisioningProfileID { get; set; }
        ProvisioningProfileType? tvOSManualProvisioningProfileType { get; set; }
        string tvOSManualProvisioningProfileID { get; set; }

        void SetupProjectParameters();
    }
}
