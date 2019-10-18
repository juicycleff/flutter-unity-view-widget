using System;
using UnityEngine;
using System.Net;

namespace UnityEditor.TestTools.TestRunner
{
    internal class AndroidPlatformSetup : IPlatformSetup
    {
        private string m_oldApplicationIdentifier;
        private string m_oldDeviceSocketAddress;
        [SerializeField]
        private bool m_Stripping;

        public void Setup()
        {
            m_oldApplicationIdentifier = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android);
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.UnityTestRunner.UnityTestRunner");

            m_oldDeviceSocketAddress = EditorUserBuildSettings.androidDeviceSocketAddress;
            var androidDeviceConnection = Environment.GetEnvironmentVariable("ANDROID_DEVICE_CONNECTION");
            EditorUserBuildSettings.waitForPlayerConnection = true;
            if (androidDeviceConnection != null)
            {
                EditorUserBuildSettings.androidDeviceSocketAddress = androidDeviceConnection;
            }
            m_Stripping = PlayerSettings.stripEngineCode;
            PlayerSettings.stripEngineCode = false;
        }

        public void PostBuildAction()
        {
            PlayerSettings.stripEngineCode = m_Stripping;
        }

        public void PostSuccessfulBuildAction()
        {
            var connectionResult = -1;
            var maxTryCount = 10;
            var tryCount = maxTryCount;
            while (tryCount-- > 0 && connectionResult == -1)
            {
                connectionResult = EditorConnectionInternal.ConnectPlayerProxy(IPAddress.Loopback.ToString(), 34999);
                if (EditorUtility.DisplayCancelableProgressBar("Editor Connection", "Connecting to the player",
                    1 - ((float)tryCount / maxTryCount)))
                {
                    EditorUtility.ClearProgressBar();
                    throw new TestLaunchFailedException();
                }
            }
            EditorUtility.ClearProgressBar();
            if (connectionResult == -1)
                throw new TestLaunchFailedException(
                    "Timed out trying to connect to the player. Player failed to launch or crashed soon after launching");
        }

        public void CleanUp()
        {
            EditorUserBuildSettings.androidDeviceSocketAddress = m_oldDeviceSocketAddress;
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, m_oldApplicationIdentifier);
        }
    }
}
