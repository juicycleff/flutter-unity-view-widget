using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Object = UnityEngine.Object;

namespace UnityEditor.Timeline
{
    class PropertyCollector : IPropertyCollector
    {
        readonly Stack<GameObject> m_ObjectStack = new Stack<GameObject>();

        // Call immediately before use
        public void Reset()
        {
            m_ObjectStack.Clear();
        }

        // call to reset caches. should be called when switching master timelines
        public void Clear()
        {
            m_ObjectStack.Clear();
            AnimationPreviewUtilities.ClearCaches();
        }

        public void PushActiveGameObject(GameObject gameObject)
        {
            m_ObjectStack.Push(gameObject);
        }

        public void PopActiveGameObject()
        {
            m_ObjectStack.Pop();
        }

        public void AddFromClip(AnimationClip clip)
        {
            var go = m_ObjectStack.Peek(); // allow it to throw if empty
            if (go != null && clip != null) // null game object is allowed for calls to be ignored
                AddFromClip(go, clip);
        }

        public void AddFromClips(IEnumerable<AnimationClip> clips)
        {
            var go = m_ObjectStack.Peek();
            if (go != null)
                AddFromClips(go, clips);
        }

        public void AddFromName<T>(string name) where T : Component
        {
            var go = m_ObjectStack.Peek(); // allow it to throw if empty
            if (go != null) // null game object is allowed for calls to be ignored
                AddFromName<T>(go, name);
        }

        public void AddFromName(string name)
        {
            var go = m_ObjectStack.Peek(); // allow it to throw if empty
            if (go != null) // null game object is allowed for calls to be ignored
                AddFromName(go, name);
        }

        public void AddFromClip(GameObject obj, AnimationClip clip)
        {
            if (!Application.isPlaying)
                AddPropertiesFromClip(obj, clip);
        }

        public void AddFromClips(GameObject animatorRoot, IEnumerable<AnimationClip> clips)
        {
            if (Application.isPlaying)
                return;
            
            AnimationPreviewUtilities.PreviewFromCurves(animatorRoot, AnimationPreviewUtilities.GetBindings(animatorRoot, clips));
        }

        public void AddFromName<T>(GameObject obj, string name) where T : Component
        {
            if (!Application.isPlaying)
                AddPropertiesFromName(obj, typeof(T), name);
        }

        public void AddFromName(GameObject obj, string name)
        {
            if (!Application.isPlaying)
                AddPropertiesFromName(obj, name);
        }

        public void AddFromName(Component component, string name)
        {
            if (!Application.isPlaying)
                AddPropertyModification(component, name);
        }

        public void AddFromComponent(GameObject obj, Component component)
        {
            if (Application.isPlaying)
                return;

            if (obj == null || component == null)
                return;

            var serializedObject = new SerializedObject(component);
            SerializedProperty property = serializedObject.GetIterator();

            while (property.NextVisible(true))
            {
                if (property.hasVisibleChildren || !AnimatedParameterUtility.IsTypeAnimatable(property.propertyType))
                    continue;

                AddPropertyModification(component, property.propertyPath);
            }
        }

        void AddPropertiesFromClip(GameObject go, AnimationClip clip)
        {
            if (go != null && clip != null)
            {
                AnimationMode.InitializePropertyModificationForGameObject(go, clip);
            }
        }

        static void AddPropertiesFromName(GameObject go, string property)
        {
            if (go == null)
                return;

            AddPropertyModification(go, property);
        }

        static void AddPropertiesFromName(GameObject go, Type compType, string property)
        {
            if (go == null)
                return;
            var comp = go.GetComponent(compType);
            if (comp == null)
                return;

            AddPropertyModification(comp, property);
        }

        public void AddObjectProperties(Object obj, AnimationClip clip)
        {
            if (obj == null || clip == null)
                return;

            IPlayableAsset asset = obj as IPlayableAsset;
            IPlayableBehaviour playable = obj as IPlayableBehaviour;

            // special case for assets that contain animated script playables.
            // The paths in the clip start from the field with the templated playable
            if (asset != null)
            {
                if (playable == null)
                {
                    AddSerializedPlayableModifications(asset, clip);
                }
                else
                {
                    // in this case the asset is the playable. The clip applies directly
                    AnimationMode.InitializePropertyModificationForObject(obj, clip);
                }
            }
        }

        void AddSerializedPlayableModifications(IPlayableAsset asset, AnimationClip clip)
        {
            var obj = asset as Object;
            if (obj == null)
                return;

            var driver = WindowState.previewDriver;
            if (driver == null || !AnimationMode.InAnimationMode(driver))
                return;

            var serializedObj = new SerializedObject(obj);
            var bindings = AnimationClipCurveCache.Instance.GetCurveInfo(clip).bindings;
            var fields = AnimatedParameterUtility.GetScriptPlayableFields(asset);

            // go through each binding and offset using the field name
            //  so the modification system can find the particle object using the asset as a root
            foreach (var b in bindings)
            {
                foreach (var f in fields)
                {
                    var propertyPath = f.Name + "." + b.propertyName;
                    if (serializedObj.FindProperty(propertyPath) != null)
                    {
                        DrivenPropertyManager.RegisterProperty(driver, obj, propertyPath);
                        break;
                    }
                }
            }
        }

        private static void AddPropertyModification(GameObject obj, string propertyName)
        {
            var driver = WindowState.previewDriver;
            if (driver == null || !AnimationMode.InAnimationMode(driver))
                return;

            DrivenPropertyManager.RegisterProperty(driver, obj, propertyName);
        }

        private static void AddPropertyModification(Component comp, string name)
        {
            if (comp == null)
                return;

            var driver = WindowState.previewDriver;
            if (driver == null || !AnimationMode.InAnimationMode(driver))
                return;

            // Register Property will display an error if a property doesn't exist (wanted behaviour)
            // However, it also displays an error on Monobehaviour m_Script property, since it can't be driven. (not wanted behaviour)
            // case 967026
            if (name == "m_Script" && (comp as MonoBehaviour) != null)
                return;

            DrivenPropertyManager.RegisterProperty(driver, comp, name);
        }
    }
}
