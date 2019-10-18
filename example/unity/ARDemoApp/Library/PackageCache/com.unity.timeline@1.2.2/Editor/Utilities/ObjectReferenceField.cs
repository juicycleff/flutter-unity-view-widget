using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.Playables;
using Object = UnityEngine.Object;

namespace UnityEditor.Timeline
{
    // Describes the object references on a ScriptableObject, ignoring script fields
    struct ObjectReferenceField
    {
        public string propertyPath;
        public bool isSceneReference;
        public System.Type type;

        private readonly static ObjectReferenceField[] None = new ObjectReferenceField[0];
        private readonly static Dictionary<System.Type, ObjectReferenceField[]> s_Cache = new Dictionary<System.Type, ObjectReferenceField[]>();

        public static ObjectReferenceField[] FindObjectReferences(System.Type type)
        {
            if (type == null)
                return None;

            if (type.IsAbstract || type.IsInterface)
                return None;

            if (!typeof(ScriptableObject).IsAssignableFrom(type))
                return None;

            ObjectReferenceField[] result = null;
            if (s_Cache.TryGetValue(type, out result))
                return result;

            result = SearchForFields(type);
            s_Cache[type] = result;
            return result;
        }

        public static ObjectReferenceField[] FindObjectReferences<T>() where T : ScriptableObject, new()
        {
            return FindObjectReferences(typeof(T));
        }

        private static ObjectReferenceField[] SearchForFields(System.Type t)
        {
            Object instance = ScriptableObject.CreateInstance(t);
            var list = new List<ObjectReferenceField>();

            var serializableObject = new SerializedObject(instance);
            var prop = serializableObject.GetIterator();
            bool enterChildren = true;
            while (prop.NextVisible(enterChildren))
            {
                enterChildren = true;
                var ppath = prop.propertyPath;
                if (ppath == "m_Script")
                {
                    enterChildren = false;
                }
                else if (prop.propertyType == SerializedPropertyType.ObjectReference || prop.propertyType == SerializedPropertyType.ExposedReference)
                {
                    enterChildren = false;
                    var exposedType = GetTypeFromPath(t, prop.propertyPath);
                    if (exposedType != null && typeof(Object).IsAssignableFrom(exposedType))
                    {
                        bool isSceneRef = prop.propertyType == SerializedPropertyType.ExposedReference;
                        list.Add(
                            new ObjectReferenceField() {propertyPath = prop.propertyPath, isSceneReference = isSceneRef, type = exposedType}
                        );
                    }
                }
            }

            Object.DestroyImmediate(instance);
            if (list.Count == 0)
                return None;
            return list.ToArray();
        }

        private static System.Type GetTypeFromPath(System.Type baseType, string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            System.Type parentType = baseType;
            FieldInfo field = null;
            var pathTo = path.Split(new char[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
            var flags = BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance;
            foreach (string s in pathTo)
            {
                field = parentType.GetField(s, flags);
                while (field == null)
                {
                    if (parentType.BaseType == null)
                        return null; // Should not happen really. Means SerializedObject got the property, but the reflection missed it
                    parentType = parentType.BaseType;
                    field = parentType.GetField(s, flags);
                }

                parentType = field.FieldType;
            }

            // dig out exposed reference types
            if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(ExposedReference<Object>).GetGenericTypeDefinition())
            {
                return field.FieldType.GetGenericArguments()[0];
            }

            return field.FieldType;
        }

        public Object Find(ScriptableObject sourceObject, Object context = null)
        {
            if (sourceObject == null)
                return null;

            SerializedObject obj = new SerializedObject(sourceObject, context);
            var prop = obj.FindProperty(propertyPath);
            if (prop == null)
                throw new InvalidOperationException("sourceObject is not of the proper type. It does not contain a path to " + propertyPath);

            Object result = null;
            if (isSceneReference)
            {
                if (prop.propertyType != SerializedPropertyType.ExposedReference)
                    throw new InvalidOperationException(propertyPath + " is marked as a Scene Reference, but is not an exposed reference type");
                if (context == null)
                    Debug.LogWarning("ObjectReferenceField.Find " + " is called on a scene reference without a context, will always be null");

                result = prop.exposedReferenceValue;
            }
            else
            {
                if (prop.propertyType != SerializedPropertyType.ObjectReference)
                    throw new InvalidOperationException(propertyPath + "is marked as an asset reference, but is not an object reference type");
                result = prop.objectReferenceValue;
            }

            return result;
        }

        /// <summary>
        /// Check if an Object satisfies this field, including components
        /// </summary>
        public bool IsAssignable(Object obj)
        {
            if (obj == null)
                return false;

            // types match
            bool potentialMatch = type.IsAssignableFrom(obj.GetType());

            // field is component, and it exists on the gameObject
            if (!potentialMatch && typeof(Component).IsAssignableFrom(type) && obj is GameObject)
                potentialMatch = ((GameObject)obj).GetComponent(type) != null;

            return potentialMatch && isSceneReference == obj.IsSceneObject();
        }

        /// <summary>
        /// Assigns a value to the field
        /// </summary>
        public bool Assign(ScriptableObject scriptableObject, Object value, IExposedPropertyTable exposedTable = null)
        {
            var serializedObject = new SerializedObject(scriptableObject, exposedTable as Object);
            var property = serializedObject.FindProperty(propertyPath);
            if (property == null)
                return false;

            // if the value is a game object, but the field is a component
            if (value is GameObject && typeof(Component).IsAssignableFrom(type))
                value = ((GameObject)value).GetComponent(type);

            if (isSceneReference)
            {
                property.exposedReferenceValue = value;

                // the object gets dirtied, but not the scene which is where the reference is stored
                var component = exposedTable as Component;
                if (component != null && !EditorApplication.isPlaying)
                    EditorSceneManager.MarkSceneDirty(component.gameObject.scene);
            }
            else
                property.objectReferenceValue = value;

            serializedObject.ApplyModifiedProperties();
            return true;
        }
    }
}
