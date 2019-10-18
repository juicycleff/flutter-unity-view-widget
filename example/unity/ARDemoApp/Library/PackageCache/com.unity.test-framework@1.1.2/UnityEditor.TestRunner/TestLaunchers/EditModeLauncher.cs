using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using UnityEditor.SceneManagement;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestRunner.Utils;
using UnityEngine.TestTools;
using UnityEngine.TestTools.TestRunner;
using UnityEngine.TestTools.TestRunner.GUI;

namespace UnityEditor.TestTools.TestRunner
{
    internal class EditModeLauncher : TestLauncherBase
    {
        public static bool IsRunning;
        private readonly EditModeRunner m_EditModeRunner;

        // provided for backward compatibility with Rider UnitTesting prior to Rider package v.1.1.1
        public EditModeLauncher(TestRunnerFilter filter, TestPlatform platform) : this(new[]
        {
            new Filter()
            {
                testNames = filter.testNames,
                categoryNames = filter.categoryNames,
                groupNames = filter.groupNames,
                assemblyNames = filter.assemblyNames
            }
        }, platform)
        {
        }

        public EditModeLauncher(Filter[] filters, TestPlatform platform)
        {
            m_EditModeRunner = ScriptableObject.CreateInstance<EditModeRunner>();
            m_EditModeRunner.UnityTestAssemblyRunnerFactory = new UnityTestAssemblyRunnerFactory();
            m_EditModeRunner.Init(filters, platform);
        }

        public override void Run()
        {
            // Give user chance to save the changes to their currently open scene because we close it and load our own
            var cancelled = !EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            if (cancelled)
                return;

            IsRunning = true;
            var exceptionThrown = ExecutePreBuildSetupMethods(m_EditModeRunner.GetLoadedTests(), m_EditModeRunner.GetFilter());
            if (exceptionThrown)
            {
                CallbacksDelegator.instance.RunFailed("Run Failed: One or more errors in a prebuild setup. See the editor log for details.");
                return;
            }

            var undoGroup = Undo.GetCurrentGroup();
            SceneSetup[] previousSceneSetup;
            if (!OpenNewScene(out previousSceneSetup))
                return;

            var callback = AddEventHandler<EditModeRunnerCallback>();
            callback.previousSceneSetup = previousSceneSetup;
            callback.undoGroup = undoGroup;
            callback.runner = m_EditModeRunner;
            AddEventHandler<CallbacksDelegatorListener>();

            m_EditModeRunner.Run();
            AddEventHandler<BackgroundListener>();
            AddEventHandler<TestRunCallbackListener>();
        }

        private static bool OpenNewScene(out SceneSetup[] previousSceneSetup)
        {
            previousSceneSetup = null;

            var sceneCount = SceneManager.sceneCount;

            var scene = SceneManager.GetSceneAt(0);
            var isSceneNotPersisted = string.IsNullOrEmpty(scene.path);

            if (sceneCount == 1 && isSceneNotPersisted)
            {
                EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
                return true;
            }
            RemoveUntitledScenes();

            // In case the user chose not to save the dirty scenes we reload them
            ReloadUnsavedDirtyScene();

            previousSceneSetup = EditorSceneManager.GetSceneManagerSetup();

            scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
            SceneManager.SetActiveScene(scene);

            return true;
        }

        private static void ReloadUnsavedDirtyScene()
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                var isSceneNotPersisted = string.IsNullOrEmpty(scene.path);
                var isSceneDirty = scene.isDirty;
                if (isSceneNotPersisted && isSceneDirty)
                {
                    EditorSceneManager.ReloadScene(scene);
                }
            }
        }

        private static void RemoveUntitledScenes()
        {
            int sceneCount = SceneManager.sceneCount;

            var scenesToClose = new List<Scene>();
            for (var i = 0; i < sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                var isSceneNotPersisted = string.IsNullOrEmpty(scene.path);
                if (isSceneNotPersisted)
                {
                    scenesToClose.Add(scene);
                }
            }
            foreach (Scene scene in scenesToClose)
            {
                EditorSceneManager.CloseScene(scene, true);
            }
        }

        public class BackgroundListener : ScriptableObject, ITestRunnerListener
        {
            public void RunStarted(ITest testsToRun)
            {
            }

            public void RunFinished(ITestResult testResults)
            {
                IsRunning = false;
            }

            public void TestStarted(ITest test)
            {
            }

            public void TestFinished(ITestResult result)
            {
            }
        }

        public T AddEventHandler<T>() where T : ScriptableObject, ITestRunnerListener
        {
            return m_EditModeRunner.AddEventHandler<T>();
        }
    }
}
