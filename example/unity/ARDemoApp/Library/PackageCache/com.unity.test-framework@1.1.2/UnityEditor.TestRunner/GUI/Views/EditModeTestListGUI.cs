using System;
using System.Linq;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.TestRunner.GUI;

namespace UnityEditor.TestTools.TestRunner.GUI
{
    [Serializable]
    internal class EditModeTestListGUI : TestListGUI
    {
        public override TestMode TestMode
        {
            get { return TestMode.EditMode; }
        }

        public override void RenderNoTestsInfo()
        {
            if (!TestListGUIHelper.SelectedFolderContainsTestAssembly())
            {
                var noTestText = "No tests to show";

                if (!PlayerSettings.playModeTestRunnerEnabled)
                {
                    const string testsArePulledFromCustomAssemblies =
                        "EditMode tests can be in Editor only Assemblies, either in the editor special folder or Editor only Assembly Definitions with added Unity References \"Test Assemblies\".";
                    noTestText += Environment.NewLine + testsArePulledFromCustomAssemblies;
                }

                EditorGUILayout.HelpBox(noTestText, MessageType.Info);
                if (GUILayout.Button("Create EditMode Test Assembly Folder"))
                {
                    TestListGUIHelper.AddFolderAndAsmDefForTesting(isEditorOnly: true);
                }
            }

            if (!TestListGUIHelper.CanAddEditModeTestScriptAndItWillCompile())
            {
                UnityEngine.GUI.enabled = false;
                EditorGUILayout.HelpBox("EditMode test scripts can only be created in editor test assemblies.", MessageType.Warning);
            }
            if (GUILayout.Button("Create Test Script in current folder"))
            {
                TestListGUIHelper.AddTest();
            }
            UnityEngine.GUI.enabled = true;
        }

        public override void PrintHeadPanel()
        {
            base.PrintHeadPanel();
            DrawFilters();
        }

        protected override void RunTests(params TestRunnerFilter[] filters)
        {
            if (EditorUtility.scriptCompilationFailed)
            {
                Debug.LogError("Fix compilation issues before running tests");
                return;
            }

            foreach (var filter in filters)
            {
                filter.ClearResults(newResultList.OfType<TestRunnerFilter.IClearableResult>().ToList());                
            }

            RerunCallbackData.instance.runFilters = filters;
            RerunCallbackData.instance.testMode = TestMode.EditMode;

            var testRunnerApi = ScriptableObject.CreateInstance<TestRunnerApi>();
            testRunnerApi.Execute(new ExecutionSettings()
            {
                filters = filters.Select(filter => new Filter()
                {
                    assemblyNames = filter.assemblyNames,
                    categoryNames = filter.categoryNames,
                    groupNames =  filter.groupNames,
                    testMode = TestMode,
                    testNames = filter.testNames
                }).ToArray()
            });
        }

        public override TestPlatform TestPlatform { get { return TestPlatform.EditMode; } }
        protected override bool IsBusy()
        {
            return EditModeLauncher.IsRunning || EditorApplication.isCompiling || EditorApplication.isPlaying;
        }
    }
}
