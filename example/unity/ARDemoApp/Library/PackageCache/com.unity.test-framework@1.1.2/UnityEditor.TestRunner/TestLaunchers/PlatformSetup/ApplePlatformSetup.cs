using System;
using System.Diagnostics;
using UnityEngine;

namespace UnityEditor.TestTools.TestRunner
{
    [Serializable]
    internal class ApplePlatformSetup : IPlatformSetup
    {
        [SerializeField]
        private bool m_Stripping;

        public ApplePlatformSetup(BuildTarget buildTarget)
        {
        }

        public void Setup()
        {
            // Camera and fonts are stripped out and app crashes on iOS when test runner is trying to add a scene with... camera and text
            m_Stripping = PlayerSettings.stripEngineCode;
            PlayerSettings.stripEngineCode = false;
        }

        public void PostBuildAction()
        {
            // Restoring player setting as early as possible
            PlayerSettings.stripEngineCode = m_Stripping;
        }

        public void PostSuccessfulBuildAction()
        {
        }

        public void CleanUp()
        {
        }
    }
}
