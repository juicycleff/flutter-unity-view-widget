using System.Collections.Generic;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// Interface used to inform the Timeline Editor about potential property modifications that may occur while previewing.
    /// </summary>
    public interface IPropertyCollector
    {
        /// <summary>
        /// Sets the active game object for subsequent property modifications.
        /// </summary>
        /// <param name="gameObject">The GameObject to push.</param>
        void PushActiveGameObject(GameObject gameObject);

        /// <summary>
        /// Removes the active GameObject from the modification stack, restoring the previous value.
        /// </summary>
        void PopActiveGameObject();

        /// <summary>
        /// Add properties modified by an animation clip.
        /// </summary>
        /// <param name="clip">The animation clip that contains the properties</param>
        void AddFromClip(AnimationClip clip);

        /// <summary>
        /// Add property modifications specified by a list of animation clips.
        /// </summary>
        /// <param name="clips">The list of animation clips used to determine which property modifications to apply.</param>
        void AddFromClips(IEnumerable<AnimationClip> clips);

        /// <summary>
        /// Add property modifications using the serialized property name.
        /// </summary>
        /// <param name="name">The name of the serialized property</param>
        /// <typeparam name="T">The type of the component the property exists on</typeparam>
        /// <remarks>
        /// This method uses the most recent gameObject from PushActiveGameObject
        /// </remarks>
        void AddFromName<T>(string name) where T : Component;

        /// <summary>
        /// Add property modifications using the serialized property name.
        /// </summary>
        /// <param name="name">The name of the serialized property</param>
        /// <remarks>
        /// This method uses the most recent gameObject from PushActiveGameObject
        /// </remarks>
        void AddFromName(string name);

        /// <summary>
        /// Add property modifications modified by an animation clip.
        /// </summary>
        /// <param name="obj">The GameObject where the properties exist</param>
        /// <param name="clip">The animation clip that contains the properties</param>
        void AddFromClip(GameObject obj, AnimationClip clip);

        /// <summary>
        /// Add property modifications specified by a list of animation clips.
        /// </summary>
        /// <param name="obj">The gameObject that will be animated</param>
        /// <param name="clips">The list of animation clips used to determine which property modifications to apply.</param>
        void AddFromClips(GameObject obj, IEnumerable<AnimationClip> clips);

        /// <summary>
        /// Add property modifications using the serialized property name.
        /// </summary>
        /// <param name="name">The name of the serialized property</param>
        /// <param name="obj">The gameObject where the properties exist</param>
        /// <typeparam name="T">The type of the component the property exists on</typeparam>>
        void AddFromName<T>(GameObject obj, string name) where T : Component;

        /// <summary>
        /// Add property modifications using the serialized property name.
        /// </summary>
        /// <param name="obj">The gameObject where the properties exist</param>
        /// <param name="name">The name of the serialized property</param>
        void AddFromName(GameObject obj, string name);

        /// <summary>
        /// Add property modifications using the serialized property name.
        /// </summary>
        /// <param name="name">The name of the serialized property</param>
        /// <param name="component">The component where the properties exist</param>
        void AddFromName(Component component, string name);

        /// <summary>
        /// Set all serializable properties on a component to be under preview control.
        /// </summary>
        /// <param name="obj">The gameObject where the properties exist</param>
        /// <param name="component">The component to set in preview mode</param>
        void AddFromComponent(GameObject obj, Component component);

        /// <summary>
        /// Add property modifications modified by an animation clip.
        /// </summary>
        /// <param name="obj">The Object where the properties exist</param>
        /// <param name="clip">The animation clip that contains the properties</param>
        void AddObjectProperties(Object obj, AnimationClip clip);
    }
}
