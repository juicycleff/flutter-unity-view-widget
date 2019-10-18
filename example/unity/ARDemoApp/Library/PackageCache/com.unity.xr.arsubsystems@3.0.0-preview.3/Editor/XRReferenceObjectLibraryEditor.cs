using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

namespace UnityEditor.XR.ARSubsystems
{
    [CustomEditor(typeof(XRReferenceObjectLibrary))]
    class XRReferenceObjectLibraryEditor : Editor
    {
        static class Content
        {
            static Content()
            {
                s_AddButtonContent = new GUIContent("Add Reference Object", "Adds a reference object to the library.");

                s_RemoveButtonContent = new GUIContent(
                    string.Empty,
                    EditorGUIUtility.FindTexture("d_LookDevClose"),
                    "Remove this image from the library.");
            }

            public static readonly GUIContent name = new GUIContent(
                "Name",
                "The name assigned to this reference object.");

            public static readonly GUIContent types = new GUIContent(
                "Reference Object Assets",
                "An asset for each object tracking provider representing this entry. The number of available entries depends on the number of supporting packages installed.");

            public static bool addButton
            {
                get
                {
                    return GUILayout.Button(s_AddButtonContent);
                }
            }

            public static bool removeButton
            {
                get
                {
                    return GUI.Button(
                        GUILayoutUtility.GetRect(s_RemoveButtonContent, GUI.skin.button, GUILayout.ExpandWidth(false)),
                        s_RemoveButtonContent,
                        GUI.skin.button);
                }
            }

            static readonly GUIContent s_AddButtonContent;
            static readonly GUIContent s_RemoveButtonContent;
        }

        class AssemblyHelper
        {
            public AssemblyHelper()
            {
                AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
                OnAfterAssemblyReload();
            }

            public List<Type> types { get { return m_Types; } }

            void OnAfterAssemblyReload()
            {
                m_Types.Clear();
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach(var type in assembly.GetTypes())
                    {
                        if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(XRReferenceObjectEntry)))
                            m_Types.Add(type);
                    }
                }
            }

            List<Type> m_Types = new List<Type>();
        }

        static AssemblyHelper s_AssemblyHelper;

        static SerializedProperty m_ReferenceObjects;

        void OnEnable()
        {
            m_ReferenceObjects = serializedObject.FindProperty("m_ReferenceObjects");
        }

        public override void OnInspectorGUI()
        {
            if (s_AssemblyHelper == null)
                s_AssemblyHelper = new AssemblyHelper();

            serializedObject.Update();
            var library = target as XRReferenceObjectLibrary;

            int indexToRemove = -1;
            for (int i = 0; i < m_ReferenceObjects.arraySize; ++i)
            {
                bool shouldRemove = ReferenceObjectField(i);
                if (shouldRemove)
                {
                    indexToRemove = i;
                }

                EditorGUILayout.Separator();

                if (i < m_ReferenceObjects.arraySize - 1)
                    EditorGUILayout.LabelField(string.Empty, GUI.skin.horizontalSlider);
            }

            if (indexToRemove > -1)
                m_ReferenceObjects.DeleteArrayElementAtIndex(indexToRemove);

            serializedObject.ApplyModifiedProperties();

            if (Content.addButton)
            {
                Undo.RecordObject(target, "Add reference object");
                library.Add();
                EditorUtility.SetDirty(target);
            }
        }

        bool ReferenceObjectField(int index)
        {
            var library = target as XRReferenceObjectLibrary;
            var referenceObject = library.m_ReferenceObjects[index];
            var referenceObjectProperty = m_ReferenceObjects.GetArrayElementAtIndex(index);
            var nameProperty = referenceObjectProperty.FindPropertyRelative("m_Name");

            bool remove = false;
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.PropertyField(nameProperty, Content.name);
                remove = Content.removeButton;
            }

            EditorGUILayout.LabelField(Content.types);
            EditorGUILayout.Separator();

            foreach (var type in s_AssemblyHelper.types)
            {
                using (var changeCheck = new EditorGUI.ChangeCheckScope())
                {
                    var entry = EditorGUILayout.ObjectField(referenceObject.FindEntry(type), type, false) as XRReferenceObjectEntry;
                    if (changeCheck.changed)
                    {
                        serializedObject.ApplyModifiedProperties();
                        Undo.RecordObject(target, "Change reference object entry");
                        library.SetReferenceObjectEntry(index, type, entry);
                        EditorUtility.SetDirty(target);
                    }
                }
            }

            return remove;
        }
    }
}
