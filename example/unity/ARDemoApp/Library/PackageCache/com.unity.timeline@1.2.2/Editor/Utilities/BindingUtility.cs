using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Object = UnityEngine.Object;

namespace UnityEditor.Timeline
{
    static class BindingUtility
    {
        public static Type GetRequiredBindingType(PlayableBinding binding)
        {
            return binding.outputTargetType;
        }

        public static void Bind(PlayableDirector director, TrackAsset bindTo, Object objectToBind)
        {
            if (director == null || bindTo == null  || TimelineWindow.instance == null)
                return;

            TimelineWindow.instance.state.previewMode = false; // returns all objects to previous state
            TimelineUndo.PushUndo(director, "PlayableDirector Binding");
            director.SetGenericBinding(bindTo, objectToBind);
            TimelineWindow.instance.state.rebuildGraph = true;
        }

        public static BindingAction GetBindingAction(Type requiredBindingType, Object objectToBind)
        {
            if (requiredBindingType == null || objectToBind == null)
                return BindingAction.DoNotBind;

            // prevent drag and drop of prefab assets
            if (PrefabUtility.IsPartOfPrefabAsset(objectToBind))
                return BindingAction.DoNotBind;

            if (requiredBindingType.IsInstanceOfType(objectToBind))
                return BindingAction.BindDirectly;

            var draggedGameObject = objectToBind as GameObject;

            if (!typeof(Component).IsAssignableFrom(requiredBindingType) || draggedGameObject == null)
                return BindingAction.DoNotBind;

            if (draggedGameObject.GetComponent(requiredBindingType) == null)
                return BindingAction.BindToMissingComponent;

            return BindingAction.BindToExistingComponent;
        }
    }

    enum BindingAction
    {
        DoNotBind,
        BindDirectly,
        BindToExistingComponent,
        BindToMissingComponent
    }
}
