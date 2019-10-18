using System;
using UnityEngine;
using UnityEditor.XR.ARSubsystems.InternalBridge;
using UnityEngine.XR.ARSubsystems;

namespace UnityEditor.XR.ARSubsystems
{
    [CustomEditor(typeof(XRReferenceImageLibrary))]
    class XRReferenceImageLibraryEditor : Editor
    {
        static class Content
        {
            static readonly int s_AddImageControlId;
            static readonly GUIContent s_AddButtonContent;
            static readonly GUIContent s_RemoveButtonContent;

            static Content()
            {
                s_AddButtonContent = new GUIContent("Add Image");
                s_AddImageControlId = GUIUtility.GetControlID(s_AddButtonContent, FocusType.Keyboard);

                s_RemoveButtonContent = new GUIContent(
                    string.Empty,
                    EditorGUIUtility.FindTexture(EditorGUIUtility.isProSkin ? "d_LookDevClose" : "LookDevClose"),
                    "Remove this image from the database");
            }

            public static readonly GUIContent keepTexture = new GUIContent(
                "Keep Texture at Runtime",
                "If enabled, the texture will be available in the Player. Otherwise, the texture will be null in the Player.");

            public static readonly GUIContent name = new GUIContent(
                "Name",
                "The name of the reference image. This can useful for matching detected images with their reference image at runtime.");

            public static readonly GUIContent specifySize = new GUIContent(
                "Specify Size",
                "If enabled, you can specify the physical dimensions of the image in meters. Some platforms require this.");

            public static readonly GUIContent sizePixels = new GUIContent(
                "Texture Size (pixels)",
                "The texture dimensions, in pixels.");

            public static readonly GUIContent sizeMeters = new GUIContent(
                "Physical Size (meters)",
                "The dimensions of the physical image, in meters.");

            public static int addImageControlId
            {
                get
                {
                    return s_AddImageControlId;
                }
            }

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
        }

        SerializedProperty m_ReferenceImages;

        void OnEnable()
        {
            m_ReferenceImages = serializedObject.FindProperty("m_Images");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            int indexToRemove = -1;
            for (int i = 0; i < m_ReferenceImages.arraySize; ++i)
            {
                var shouldRemove = ReferenceImageField(i);
                if (shouldRemove)
                {
                    indexToRemove = i;
                }

                EditorGUILayout.Separator();

                if (i < m_ReferenceImages.arraySize - 1)
                    EditorGUILayout.LabelField(string.Empty, GUI.skin.horizontalSlider);
            }

            if (indexToRemove > -1)
                m_ReferenceImages.DeleteArrayElementAtIndex(indexToRemove);

            serializedObject.ApplyModifiedProperties();

            if (Content.addButton)
            {
                Undo.RecordObject(target, "Add reference image");
                (target as XRReferenceImageLibrary).Add();
                EditorUtility.SetDirty(target);
            }
        }

        /// <summary>
        /// Generates the GUI for a reference image at index <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the reference image in the <see cref="XRReferenceImageLibrary"/>.</param>
        /// <returns>True if the image should be removed.</returns>
        bool ReferenceImageField(int index)
        {
            var library = target as XRReferenceImageLibrary;
            var referenceImageProperty = m_ReferenceImages.GetArrayElementAtIndex(index);
            var sizeProperty = referenceImageProperty.FindPropertyRelative("m_Size");
            var specifySizeProperty = referenceImageProperty.FindPropertyRelative("m_SpecifySize");
            var nameProperty = referenceImageProperty.FindPropertyRelative("m_Name");

            var referenceImage = library[index];
            var texturePath = AssetDatabase.GUIDToAssetPath(referenceImage.textureGuid.ToString("N"));
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);

            bool shouldRemove = false;
            bool wasTextureUpdated = false;

            using (new EditorGUILayout.HorizontalScope())
            {
                using (var textureCheck = new EditorGUI.ChangeCheckScope())
                {
                    texture = TextureField(texture);
                    wasTextureUpdated = textureCheck.changed;
                }
                shouldRemove = Content.removeButton;
            }

            EditorGUILayout.PropertyField(nameProperty, Content.name);
            EditorGUILayout.PropertyField(specifySizeProperty, Content.specifySize);

            if (specifySizeProperty.boolValue)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    using (new EditorGUI.DisabledScope(true))
                    {
                        var imageDimensions = (texture == null) ? Vector2Int.zero : GetTextureSize(texture);
                        EditorGUILayout.Vector2IntField(Content.sizePixels, imageDimensions);
                    }

                    using (var changeCheck = new EditorGUI.ChangeCheckScope())
                    {
                        EditorGUILayout.PropertyField(sizeProperty, Content.sizeMeters);

                        // Prevent dimensions from going below zero.
                        var size = new Vector2(
                            Mathf.Max(0f, sizeProperty.vector2Value.x),
                            Mathf.Max(0f, sizeProperty.vector2Value.y));

                        if ((sizeProperty.vector2Value.x < 0f) ||
                            (sizeProperty.vector2Value.y < 0f))
                        {
                            sizeProperty.vector2Value = size;
                        }

                        if (changeCheck.changed)
                        {
                            if (texture == null)
                            {
                                // If the texture is null, then we just set whatever the user specifies
                                sizeProperty.vector2Value = size;
                            }
                            else
                            {
                                // Otherwise, maintain the aspect ratio
                                var delta = referenceImage.size - size;
                                delta = new Vector2(Mathf.Abs(delta.x), Mathf.Abs(delta.y));

                                // Determine which dimension has changed and compute the unchanged dimension
                                if (delta.x > delta.y)
                                {
                                    sizeProperty.vector2Value = SizeFromWidth(texture, size);
                                }
                                else if (delta.y > 0f)
                                {
                                    sizeProperty.vector2Value = SizeFromHeight(texture, size);
                                }
                            }
                        }
                        else if (wasTextureUpdated && texture != null)
                        {
                            // If the texture changed, re-compute width / height
                            if (size.x == 0f)
                            {
                                sizeProperty.vector2Value = SizeFromHeight(texture, size);
                            }
                            else
                            {
                                sizeProperty.vector2Value = SizeFromWidth(texture, size);
                            }
                        }
                    }

                    if ((sizeProperty.vector2Value.x <= 0f) || (sizeProperty.vector2Value.y <= 0f))
                    {
                        EditorGUILayout.HelpBox("Dimensions must be greater than zero.", MessageType.Warning);
                    }
                }
            }

            using (new EditorGUI.DisabledScope(texture == null))
            using (var changeCheck = new EditorGUI.ChangeCheckScope())
            {
                bool keepTexture = EditorGUILayout.Toggle(Content.keepTexture, referenceImage.texture != null);
                if (changeCheck.changed || wasTextureUpdated)
                {
                    // Auto-populate the name from the texture's name if it is not set already.
                    if (string.IsNullOrEmpty(nameProperty.stringValue) && texture != null)
                    {
                        nameProperty.stringValue = texture.name;
                    }

                    // Apply properties for anything that may have been modified, otherwise
                    // the texture change may be overwritten.
                    serializedObject.ApplyModifiedProperties();

                    // Create an undo entry, modify, set dirty
                    Undo.RecordObject(target, "Update reference image texture");
                    library.SetTexture(index, texture, keepTexture);
                    EditorUtility.SetDirty(target);
                }
            }

            return shouldRemove;
        }

        /// <summary>
        /// Computes a new size using the width and aspect ratio from the Texture2D.
        /// Width remains the same as before; height is recalculated.
        /// </summary>
        static Vector2 SizeFromWidth(Texture2D texture, Vector2 size)
        {
            var textureSize = GetTextureSize(texture);
            return new Vector2(size.x, size.x * (float)textureSize.y / (float)textureSize.x);
        }

        /// <summary>
        /// Computes a new size using the height and aspect ratio from the Texture2D.
        /// Height remains the same as before; width is recalculated.
        /// </summary>
        static Vector2 SizeFromHeight(Texture2D texture, Vector2 size)
        {
            var textureSize = GetTextureSize(texture);
            return new Vector2(size.y * (float)textureSize.x / (float)textureSize.y, size.y);
        }

        static Vector2Int GetTextureSize(Texture2D texture)
        {
            if (texture == null)
                throw new ArgumentNullException("texture");

            var textureImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(texture)) as TextureImporter;
            if (textureImporter == null)
            {
                return new Vector2Int(texture.width, texture.height);
            }
            else
            {
                return TextureImporterInternals.GetSourceTextureDimensions(textureImporter);
            }
        }

        static Texture2D TextureField(Texture2D texture)
        {
            const int k_MaxSideLength = 64;
            int width = k_MaxSideLength, height = k_MaxSideLength;
            if (texture != null)
            {
                var textureSize = GetTextureSize(texture);

                if (textureSize.x > textureSize.y)
                    height = width * textureSize.y / textureSize.x;
                else
                    width = height * textureSize.x / textureSize.y;
            }

            return (Texture2D)EditorGUILayout.ObjectField(
                texture,
                typeof(Texture2D),
                true,
                GUILayout.Width(width),
                GUILayout.Height(height));
        }
    }
}
