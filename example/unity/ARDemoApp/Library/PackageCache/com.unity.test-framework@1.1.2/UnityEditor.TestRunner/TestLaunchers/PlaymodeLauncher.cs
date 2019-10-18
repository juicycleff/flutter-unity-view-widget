using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal.Filters;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestRunner.Utils;
using UnityEngine.TestTools.TestRunner;
using UnityEngine.TestTools.TestRunner.Callbacks;

namespace UnityEditor.TestTools.TestRunner
{
    internal class PlaymodeLauncher : RuntimeTestLauncherBase
    {
        public static bool IsRunning;
        private Scene m_Scene;
        private bool m_IsTestSetupPerformed;
        private readonly PlaymodeTestsControllerSettings m_Settings;
        private ITestFilter testFilter;

        [SerializeField]
        private List<Type> m_EventHandlers = new List<Type>();

        public PlaymodeLauncher(PlaymodeTestsControllerSettings settings)
        {
            m_Settings = settings;
        }

        public override void Run()
        {
            IsRunning = true;
            ConsoleWindow.SetConsoleErrorPause(false);
            Application.runInBackground = true;

            var sceneName = CreateSceneName();
            m_Scene = CreateBootstrapScene(sceneName, runner =>
            {
                runner.AddEventHandlerMonoBehaviour<PlayModeRunnerCallback>();
                runner.AddEventHandlerScriptableObject<TestRunnerCallback>();
                runner.AddEventHandlerScriptableObject<CallbacksDelegatorListener>();
                runner.AddEventHandlerScriptableObject<TestRunCallbackListener>();

                foreach (var eventHandler in m_EventHandlers)
                {
                    var obj = ScriptableObject.CreateInstance(eventHandler);
                    runner.AddEventHandlerScriptableObject(obj as ITestRunnerListener);
                }

                runner.settings = m_Settings;
            });

            if (m_Settings.sceneBased)
            {
                var newListOfScenes =
                    new List<EditorBuildSettingsScene> {new EditorBuildSettingsScene(sceneName, true)};
                newListOfScenes.AddRange(EditorBuildSettings.scenes);
                EditorBuildSettings.scenes = newListOfScenes.ToArray();
            }

            EditorApplication.update += UpdateCallback;
        }

        public void UpdateCallback()
        {
            if (m_IsTestSetupPerformed)
            {
                if (m_Scene.IsValid())
                    SceneManager.SetActiveScene(m_Scene);
                EditorApplication.update -= UpdateCallback;
                EditorApplication.isPlaying = true;
            }
            else
            {
                testFilter = m_Settings.BuildNUnitFilter();
                var runner = LoadTests(testFilter);

                var exceptionThrown = ExecutePreBuildSetupMethods(runner.LoadedTest, testFilter);
                if (exceptionThrown)
                {
                    EditorApplication.update -= UpdateCallback;
                    IsRunning = false;
                    var controller = PlaymodeTestsController.GetController();
                    ReopenOriginalScene(controller);
                    AssetDatabase.DeleteAsset(controller.settings.bootstrapScene);
                    CallbacksDelegator.instance.RunFailed("Run Failed: One or more errors in a prebuild setup. See the editor log for details.");
                    return;
                }
                m_IsTestSetupPerformed = true;
            }
        }

        [InitializeOnLoad]
        public class BackgroundWatcher
        {
            static BackgroundWatcher()
            {
                EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            }

            private static void OnPlayModeStateChanged(PlayModeStateChange state)
            {
                if (!PlaymodeTestsController.IsControllerOnScene())
                    return;
                var runner = PlaymodeTestsController.GetController();
                if (runner == null)
                    return;
                if (state == PlayModeStateChange.ExitingPlayMode)
                {
                    AssetDatabase.DeleteAsset(runner.settings.bootstrapScene);
                    ExecutePostBuildCleanupMethods(runner.m_Runner.LoadedTest, runner.settings.BuildNUnitFilter(), Application.platform);
                    IsRunning = false;
                }
                else if (state == PlayModeStateChange.EnteredEditMode)
                {
                    //reopen the original scene once we exit playmode
                    ReopenOriginalScene(runner);
                }
            }
        }

        protected static void ReopenOriginalScene(PlaymodeTestsController runner)
        {
            ReopenOriginalScene(runner.settings.originalScene);
        }

        public void AddEventHandler<T>() where T : ScriptableObject, ITestRunnerListener
        {
            m_EventHandlers.Add(typeof(T));
        }
    }
}
