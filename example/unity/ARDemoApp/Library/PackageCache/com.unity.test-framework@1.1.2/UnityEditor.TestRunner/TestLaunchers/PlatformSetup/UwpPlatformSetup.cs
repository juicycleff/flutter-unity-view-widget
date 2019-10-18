using System;

namespace UnityEditor.TestTools.TestRunner
{
    internal class UwpPlatformSetup : IPlatformSetup
    {
        private const string k_SettingsBuildConfiguration = "BuildConfiguration";
        private bool m_InternetClientServer;
        private bool m_PrivateNetworkClientServer;

        public void Setup()
        {
            m_InternetClientServer = PlayerSettings.WSA.GetCapability(PlayerSettings.WSACapability.InternetClientServer);
            m_PrivateNetworkClientServer = PlayerSettings.WSA.GetCapability(PlayerSettings.WSACapability.PrivateNetworkClientServer);
            PlayerSettings.WSA.SetCapability(PlayerSettings.WSACapability.InternetClientServer, true);
            PlayerSettings.WSA.SetCapability(PlayerSettings.WSACapability.PrivateNetworkClientServer, true);

            // This setting is initialized only when Window Store App is selected from the Build Settings window, and
            // is typically an empty strings when running tests via UTR on the command-line.
            bool wsaSettingNotInitialized = string.IsNullOrEmpty(EditorUserBuildSettings.wsaArchitecture);

            // If WSA build settings aren't fully initialized or running from a build machine, specify a default build configuration.
            // Otherwise we can use the existing configuration specified by the user in Build Settings.
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("UNITY_THISISABUILDMACHINE")) || wsaSettingNotInitialized)
            {
                EditorUserBuildSettings.wsaSubtarget = WSASubtarget.PC;
                EditorUserBuildSettings.wsaArchitecture = "x64";
                EditorUserBuildSettings.SetPlatformSettings(BuildPipeline.GetBuildTargetName(BuildTarget.WSAPlayer), k_SettingsBuildConfiguration, WSABuildType.Debug.ToString());
                EditorUserBuildSettings.wsaUWPBuildType = WSAUWPBuildType.ExecutableOnly;
                PlayerSettings.SetIl2CppCompilerConfiguration(BuildTargetGroup.WSA, Il2CppCompilerConfiguration.Debug);
            }
        }

        public void PostBuildAction()
        {
        }

        public void PostSuccessfulBuildAction()
        {
        }

        public void CleanUp()
        {
            PlayerSettings.WSA.SetCapability(PlayerSettings.WSACapability.InternetClientServer, m_InternetClientServer);
            PlayerSettings.WSA.SetCapability(PlayerSettings.WSACapability.PrivateNetworkClientServer, m_PrivateNetworkClientServer);
        }
    }
}
