using System;

namespace UnityEditor.TestTools.TestRunner
{
    internal class TestSettings : ITestSettings
    {
        private readonly TestSetting[] m_Settings =
        {
            new TestSetting<ScriptingImplementation?>(
                settings => settings.scriptingBackend,
                () => PlayerSettings.GetScriptingBackend(EditorUserBuildSettings.activeBuildTargetGroup),
                implementation => PlayerSettings.SetScriptingBackend(EditorUserBuildSettings.activeBuildTargetGroup, implementation.Value)),
            new TestSetting<string>(
                settings => settings.Architecture,
                () => EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android ? PlayerSettings.Android.targetArchitectures.ToString() : null,
                architecture =>
                {
                    if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
                    {
                        if (!string.IsNullOrEmpty(architecture))
                        {
                            var targetArchitectures = (AndroidArchitecture)Enum.Parse(typeof(AndroidArchitecture), architecture, true);
                            PlayerSettings.Android.targetArchitectures = targetArchitectures;
                        }
                    }
                }),
            new TestSetting<ApiCompatibilityLevel?>(
                settings => settings.apiProfile,
                () => PlayerSettings.GetApiCompatibilityLevel(EditorUserBuildSettings.activeBuildTargetGroup),
                implementation =>
                {
                    if (Enum.IsDefined(typeof(ApiCompatibilityLevel), implementation.Value))
                    {
                        PlayerSettings.SetApiCompatibilityLevel(EditorUserBuildSettings.activeBuildTargetGroup,
                            implementation.Value);
                    }
                }),
            new TestSetting<bool?>(
                settings => settings.appleEnableAutomaticSigning,
                () => PlayerSettings.iOS.appleEnableAutomaticSigning,
                enableAutomaticSigning =>
                {
                    if (enableAutomaticSigning != null)
                        PlayerSettings.iOS.appleEnableAutomaticSigning = enableAutomaticSigning.Value;
                }),
            new TestSetting<string>(
                settings => settings.appleDeveloperTeamID,
                () => PlayerSettings.iOS.appleDeveloperTeamID,
                developerTeam =>
                {
                    if (developerTeam != null)
                        PlayerSettings.iOS.appleDeveloperTeamID = developerTeam;
                }),
            new TestSetting<ProvisioningProfileType?>(
                settings => settings.iOSManualProvisioningProfileType,
                () => PlayerSettings.iOS.iOSManualProvisioningProfileType,
                profileType =>
                {
                    if (profileType != null)
                        PlayerSettings.iOS.iOSManualProvisioningProfileType = profileType.Value;
                }),
            new TestSetting<string>(
                settings => settings.iOSManualProvisioningProfileID,
                () => PlayerSettings.iOS.iOSManualProvisioningProfileID,
                provisioningUUID =>
                {
                    if (provisioningUUID != null)
                        PlayerSettings.iOS.iOSManualProvisioningProfileID = provisioningUUID;
                }),
            new TestSetting<ProvisioningProfileType?>(
                settings => settings.tvOSManualProvisioningProfileType,
                () => PlayerSettings.iOS.tvOSManualProvisioningProfileType,
                profileType =>
                {
                    if (profileType != null)
                        PlayerSettings.iOS.tvOSManualProvisioningProfileType = profileType.Value;
                }),
            new TestSetting<string>(
                settings => settings.tvOSManualProvisioningProfileID,
                () => PlayerSettings.iOS.tvOSManualProvisioningProfileID,
                provisioningUUID =>
                {
                    if (provisioningUUID != null)
                        PlayerSettings.iOS.tvOSManualProvisioningProfileID = provisioningUUID;
                }),
        };

        private bool m_Disposed;

        public ScriptingImplementation? scriptingBackend { get; set; }

        public string Architecture { get; set; }

        public ApiCompatibilityLevel? apiProfile { get; set; }

        public bool? appleEnableAutomaticSigning { get; set; }
        public string appleDeveloperTeamID { get; set; }
        public ProvisioningProfileType? iOSManualProvisioningProfileType { get; set; }
        public string iOSManualProvisioningProfileID { get; set; }
        public ProvisioningProfileType? tvOSManualProvisioningProfileType { get; set; }
        public string tvOSManualProvisioningProfileID { get; set; }

        public void Dispose()
        {
            if (!m_Disposed)
            {
                foreach (var testSetting in m_Settings)
                {
                    testSetting.Cleanup();
                }

                m_Disposed = true;
            }
        }

        public void SetupProjectParameters()
        {
            foreach (var testSetting in m_Settings)
            {
                testSetting.Setup(this);
            }
        }

        private abstract class TestSetting
        {
            public abstract void Setup(TestSettings settings);
            public abstract void Cleanup();
        }

        private class TestSetting<T> : TestSetting
        {
            private T m_ValueBeforeSetup;
            private Func<TestSettings, T> m_GetFromSettings;
            private Func<T> m_GetCurrentValue;
            private Action<T> m_SetValue;

            public TestSetting(Func<TestSettings, T> getFromSettings, Func<T> getCurrentValue, Action<T> setValue)
            {
                m_GetFromSettings = getFromSettings;
                m_GetCurrentValue = getCurrentValue;
                m_SetValue = setValue;
            }

            public override void Setup(TestSettings settings)
            {
                m_ValueBeforeSetup = m_GetCurrentValue();
                var newValue = m_GetFromSettings(settings);
                if (newValue != null)
                {
                    m_SetValue(newValue);
                }
            }

            public override void Cleanup()
            {
                m_SetValue(m_ValueBeforeSetup);
            }
        }
    }
}
