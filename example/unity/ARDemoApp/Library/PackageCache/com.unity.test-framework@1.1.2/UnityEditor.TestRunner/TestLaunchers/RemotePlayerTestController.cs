using System;
using UnityEditor.Networking.PlayerConnection;
using UnityEditor.TestTools.TestRunner;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEditor.TestTools.TestRunner.UnityTestProtocol;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;
using UnityEngine.TestRunner.TestLaunchers;

namespace UnityEditor.TestRunner.TestLaunchers
{
    [Serializable]
    internal class RemoteTestRunController : ScriptableSingleton<RemoteTestRunController>
    {
        internal const int k_HeartbeatTimeout = 60 * 10;
        
        [SerializeField]
        private RemoteTestResultReciever m_RemoteTestResultReciever;

        [SerializeField]
        private PlatformSpecificSetup m_PlatformSpecificSetup;

        [SerializeField]
        private bool m_RegisteredConnectionCallbacks;

        private IDelayedCallback m_TimeoutCallback;
        
        public void Init(BuildTarget buildTarget)
        {
            m_PlatformSpecificSetup = new PlatformSpecificSetup(buildTarget);
            m_PlatformSpecificSetup.Setup();
            m_RemoteTestResultReciever = new RemoteTestResultReciever();
            EditorConnection.instance.Initialize();
            if (!m_RegisteredConnectionCallbacks)
            {
                EditorConnection.instance.Initialize();
                DelegateEditorConnectionEvents();
            }
            
            m_TimeoutCallback = new DelayedCallback(TimeoutCallback, k_HeartbeatTimeout);
        }

        private void DelegateEditorConnectionEvents()
        {
            m_RegisteredConnectionCallbacks = true;
            //This is needed because RemoteTestResultReciever is not a ScriptableObject
            EditorConnection.instance.Register(PlayerConnectionMessageIds.runStartedMessageId, RunStarted);
            EditorConnection.instance.Register(PlayerConnectionMessageIds.runFinishedMessageId, RunFinished);
            EditorConnection.instance.Register(PlayerConnectionMessageIds.testStartedMessageId, TestStarted);
            EditorConnection.instance.Register(PlayerConnectionMessageIds.testFinishedMessageId, TestFinished);
            EditorConnection.instance.Register(PlayerConnectionMessageIds.playerAliveHeartbeat, PlayerAliveHearbeat);
        }

        private void RunStarted(MessageEventArgs messageEventArgs)
        {
            m_TimeoutCallback.Reset();
            m_RemoteTestResultReciever.RunStarted(messageEventArgs);
            CallbacksDelegator.instance.RunStartedRemotely(messageEventArgs.data);
        }

        private void RunFinished(MessageEventArgs messageEventArgs)
        {
            m_TimeoutCallback.Reset();
            m_RemoteTestResultReciever.RunFinished(messageEventArgs);
            m_PlatformSpecificSetup.CleanUp();

            CallbacksDelegator.instance.RunFinishedRemotely(messageEventArgs.data);
        }

        private void TestStarted(MessageEventArgs messageEventArgs)
        {
            m_TimeoutCallback.Reset();
            CallbacksDelegator.instance.TestStartedRemotely(messageEventArgs.data);
        }

        private void TestFinished(MessageEventArgs messageEventArgs)
        {
            m_TimeoutCallback.Reset();
            CallbacksDelegator.instance.TestFinishedRemotely(messageEventArgs.data);
        }
        
        private void PlayerAliveHearbeat(MessageEventArgs messageEventArgs)
        {
            m_TimeoutCallback.Reset();
        }

        private void TimeoutCallback()
        {
            CallbacksDelegator.instance.RunFailed($"Test execution timed out. No activity received from the player in {k_HeartbeatTimeout/60} minutes.");
        }

        public void PostBuildAction()
        {
            m_PlatformSpecificSetup.PostBuildAction();
        }

        public void PostSuccessfulBuildAction()
        {
            m_PlatformSpecificSetup.PostSuccessfulBuildAction();
        }
    }
}
