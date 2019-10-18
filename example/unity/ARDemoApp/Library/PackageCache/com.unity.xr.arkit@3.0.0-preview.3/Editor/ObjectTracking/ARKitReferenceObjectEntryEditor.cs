using System;
using UnityEditor.XR.ARSubsystems;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARKit;

namespace UnityEditor.XR.ARKit
{
    [CustomEditor(typeof(ARKitReferenceObjectEntry))]
    class ARKitReferenceObjectEntryEditor : Editor
    {
        ARObject? m_ARObject;

        bool m_ShouldReadARObject;

        static class Content
        {
            public static readonly GUIContent info = new GUIContent(
                "Metadata",
                "Metadata included with the object scan");

            public static readonly GUIContent referenceOrigin = new GUIContent(
                "Reference Origin",
                "The reference origin of the scanned object.");

            public static readonly GUIContent infoRotation = new GUIContent(
                "Origin Rotation",
                "The rotation of the origin for the scanned object.");

            public static readonly GUIContent infoPosition = new GUIContent(
                "Origin Position",
                "The position of the origin for the scanned object.");

            public static readonly GUIContent infoVersion = new GUIContent(
                "Version",
                "The version of the scanned object file.");
        }

        void OnEnable()
        {
            m_ShouldReadARObject = true;
        }

        public override void OnInspectorGUI()
        {
            var entry = target as ARKitReferenceObjectEntry;

            if (m_ShouldReadARObject)
            {
                var path = AssetDatabase.GetAssetPath(entry);
                m_ARObject = ARObjectImporter.ReadARObject(path);
                m_ShouldReadARObject = false;
            }

            if (m_ARObject.HasValue)
            {
                var info = m_ARObject.Value.info;
                EditorGUILayout.LabelField(Content.info);
                using (new EditorGUI.DisabledScope(true))
                {
                    EditorGUILayout.IntField(Content.infoVersion, info.version);
                    EditorGUILayout.LabelField(Content.referenceOrigin);
                    using (new EditorGUI.IndentLevelScope())
                    {
                        EditorGUILayout.Vector3Field(Content.infoPosition, info.referenceOrigin.position);
                        EditorGUILayout.Vector3Field(Content.infoRotation, info.referenceOrigin.rotation.eulerAngles);
                    }
                    TextureField(m_ARObject.Value.preview);
                }
            }
            else
            {
                EditorGUILayout.LabelField("Could not read object info.");
            }
        }

        static int s_LastNonZeroWidth;

        static void TextureField(Texture2D texture)
        {
            using (var h = new EditorGUILayout.HorizontalScope())
            {
                // During other event types, the width is zero.
                if (Event.current.type == EventType.Repaint)
                {
                    s_LastNonZeroWidth = (int)h.rect.width;
                }

                int maxWidth = Mathf.Max(s_LastNonZeroWidth, 64);
                int width = maxWidth, height = maxWidth;
                if (texture != null)
                {
                    height = width * texture.height / texture.width;
                }

                EditorGUILayout.ObjectField(
                    texture,
                    typeof(Texture2D),
                    false,
                    GUILayout.Width(width),
                    GUILayout.Height(height));
            }
        }
    }
}
