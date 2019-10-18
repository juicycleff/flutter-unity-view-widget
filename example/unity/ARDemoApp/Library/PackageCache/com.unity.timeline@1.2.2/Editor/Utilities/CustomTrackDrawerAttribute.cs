using System;

namespace UnityEditor.Timeline
{
    // Tells a custom [[TrackDrawer]] which [[TrackAsset]] it's a drawer for.
    sealed class CustomTrackDrawerAttribute : Attribute
    {
        public Type assetType;
        public CustomTrackDrawerAttribute(Type type)
        {
            assetType = type;
        }
    }

    /// <summary>
    /// Attribute that specifies a class as an editor for an extended Timeline type.
    /// </summary>
    /// <remarks>
    /// Use this attribute on a class that extends ClipEditor, TrackEditor, or MarkerEditor to specify either the PlayableAsset, Marker, or TrackAsset derived classes for associated customization.
    /// </remarks>
    /// <example>
    /// [CustomTimelineEditor(typeof(LightControlClip))]
    /// class LightControlClipEditor : ClipEditor
    /// {
    /// }
    /// </example>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class CustomTimelineEditorAttribute : Attribute
    {
        /// <summary>
        /// The type that that this editor applies to.
        /// </summary>
        public Type classToEdit { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="type"> The type that that this editor applies to.</param>
        /// <exception cref="ArgumentNullException">Thrown if type is null</exception>
        public CustomTimelineEditorAttribute(Type type)
        {
            if (type == null)
                throw new System.ArgumentNullException(nameof(type));
            classToEdit = type;
        }
    }
}
