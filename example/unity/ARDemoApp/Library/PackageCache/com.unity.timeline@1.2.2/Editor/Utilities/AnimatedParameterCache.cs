using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityEditor.Timeline
{
    static class AnimatedParameterCache
    {
        static readonly Dictionary<Type, FieldInfo[]> k_ScriptPlayableFieldsCache = new Dictionary<Type, FieldInfo[]>();
        static readonly Dictionary<PropertyKey, FieldInfo> k_PropertyFieldInfoCache = new Dictionary<PropertyKey, FieldInfo>();
        static readonly Dictionary<PropertyKey, bool> k_PropertyIsAnimatableCache = new Dictionary<PropertyKey, bool>();
        static readonly Dictionary<PropertyKey, string> k_BindingNameCache = new Dictionary<PropertyKey, string>();

        public static bool TryGetScriptPlayableFields(Type type, out FieldInfo[] scriptPlayableFields)
        {
            return k_ScriptPlayableFieldsCache.TryGetValue(type, out scriptPlayableFields);
        }

        public static void SetScriptPlayableFields(Type type, FieldInfo[] scriptPlayableFields)
        {
            k_ScriptPlayableFieldsCache[type] = scriptPlayableFields;
        }

        public static bool TryGetFieldInfoForProperty(SerializedProperty property, out FieldInfo fieldInfo)
        {
            return k_PropertyFieldInfoCache.TryGetValue(new PropertyKey(property), out fieldInfo);
        }

        public static void SetFieldInfoForProperty(SerializedProperty property, FieldInfo fieldInfo)
        {
            k_PropertyFieldInfoCache[new PropertyKey(property)] = fieldInfo;
        }

        public static bool TryGetIsPropertyAnimatable(SerializedProperty property, out bool isAnimatable)
        {
            return k_PropertyIsAnimatableCache.TryGetValue(new PropertyKey(property), out isAnimatable);
        }

        public static void SetIsPropertyAnimatable(SerializedProperty property, bool isAnimatable)
        {
            k_PropertyIsAnimatableCache[new PropertyKey(property)] = isAnimatable;
        }

        public static bool TryGetBindingName(Type type, string path, out string bindingName)
        {
            return k_BindingNameCache.TryGetValue(new PropertyKey(type, path), out bindingName);
        }

        public static void SetBindingName(Type type, string path, string bindingName)
        {
            k_BindingNameCache[new PropertyKey(type, path)] = bindingName;
        }
    }

    struct PropertyKey : IEquatable<PropertyKey>
    {
        readonly Type m_Type;
        readonly string m_Path;

        public PropertyKey(SerializedProperty property)
        {
            m_Type = property.serializedObject.targetObject.GetType();
            m_Path = property.propertyPath;
        }

        public PropertyKey(Type type, string path)
        {
            m_Type = type;
            m_Path = path;
        }

        public bool Equals(PropertyKey other)
        {
            return m_Type == other.m_Type && string.Equals(m_Path, other.m_Path);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is PropertyKey && Equals((PropertyKey)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((m_Type != null ? m_Type.GetHashCode() : 0) * 397) ^ (m_Path != null ? m_Path.GetHashCode() : 0);
            }
        }
    }
}
