using System;
using UnityEngine;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// Specifies the type of PlayableAsset that a TrackAsset derived class can create clips of.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class TrackClipTypeAttribute : Attribute
    {
        /// <summary>
        /// The type of the clip class associate with this track
        /// </summary>
        public readonly Type inspectedType;

        /// <summary>
        /// Whether to allow automatic creation of these types.
        /// </summary>
        public readonly bool allowAutoCreate; // true will make it show up in menus

        /// <summary>
        /// </summary>
        /// <param name="clipClass">The type of the clip class to associate with this track. Must derive from PlayableAsset.</param>
        public TrackClipTypeAttribute(Type clipClass)
        {
            inspectedType = clipClass;
            allowAutoCreate = true;
        }

        /// <summary>
        /// </summary>
        /// <param name="clipClass">The type of the clip class to associate with this track. Must derive from PlayableAsset.</param>
        /// <param name="allowAutoCreate">Whether to allow automatic creation of these types. Default value is true.</param>
        /// <remarks>Setting allowAutoCreate to false will cause Timeline to not show menu items for creating clips of this type.</remarks>
        public TrackClipTypeAttribute(Type clipClass, bool allowAutoCreate)
        {
            inspectedType = clipClass;
            allowAutoCreate = false;
        }
    }

    /// <summary>
    /// Apply this to a PlayableBehaviour class or field to indicate that it is not animatable.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class)]
    public class NotKeyableAttribute : Attribute
    {
    }


    /// <summary>
    /// Options for track binding
    /// </summary>
    [Flags]
    public enum TrackBindingFlags
    {
        /// <summary>
        /// No options specified
        /// </summary>
        None = 0,

        /// <summary>
        /// Allow automatic creating of component during gameObject drag and drop
        /// </summary>
        AllowCreateComponent = 1,

        /// <summary>
        /// All options specified
        /// </summary>
        All = AllowCreateComponent
    }

    /// <summary>
    /// Specifies the type of object that should be bound to a TrackAsset.
    /// </summary>
    /// <example>
    /// <code>
    /// using UnityEngine;
    /// using UnityEngine.Timeline;
    /// [TrackBindingType(typeof(Light), TrackBindingFlags.AllowCreateComponent)]
    /// public class LightTrack : TrackAsset
    /// {
    /// }
    /// </code>
    /// </example>
    /// <remarks>
    /// Use this attribute when creating Custom Tracks to specify the type of object the track requires a binding to.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public class TrackBindingTypeAttribute : Attribute
    {
        /// <summary>
        /// The type of binding for the associate track
        /// </summary>
        public readonly Type type;

        /// <summary>
        /// Options for the the track binding
        /// </summary>
        public readonly TrackBindingFlags flags;

        public TrackBindingTypeAttribute(Type type)
        {
            this.type = type;
            this.flags = TrackBindingFlags.All;
        }

        public TrackBindingTypeAttribute(Type type, TrackBindingFlags flags)
        {
            this.type = type;
            this.flags = flags;
        }
    }

    // indicates that child tracks are permitted on a track
    //  internal because not fully supported on custom classes yet
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    class SupportsChildTracksAttribute : Attribute
    {
        public readonly Type childType;
        public readonly int levels;

        public SupportsChildTracksAttribute(Type childType = null, int levels = Int32.MaxValue)
        {
            this.childType = childType;
            this.levels = levels;
        }
    }

    // indicates that the type should not be put on a PlayableTrack
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    class IgnoreOnPlayableTrackAttribute : System.Attribute {}

    // used to flag properties as using a time field (second/frames) display
    class TimeFieldAttribute : PropertyAttribute
    {
        public enum UseEditMode
        {
            None,
            ApplyEditMode
        }
        public UseEditMode useEditMode { get; }

        public TimeFieldAttribute(UseEditMode useEditMode = UseEditMode.ApplyEditMode)
        {
            this.useEditMode = useEditMode;
        }
    }

    /// <summary>
    /// Use this attribute to hide a class from Timeline menus.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class HideInMenuAttribute : Attribute {}

    ///<summary>
    /// Use this attribute to customize the appearance of a Marker.
    /// </summary>
    /// Specify the style to use to draw a Marker.
    /// <example>
    /// [CustomStyle("MyStyle")]
    /// public class MyMarker : UnityEngine.Timeline.Marker {}
    /// </example>
    /// How to create a custom style rule:
    /// 1) Create a 'common.uss' USS file in an Editor folder in a StyleSheets/Extensions folder hierarchy.
    /// Example of valid folder paths:
    /// - Assets/Editor/StyleSheets/Extensions
    /// - Assets/Editor/Markers/StyleSheets/Extensions
    /// - Assets/Timeline/Editor/MyMarkers/StyleSheets/Extensions
    /// Rules in 'dark.uss' are used if you use the Pro Skin and rules in 'light.uss' are used otherwise.
    ///
    /// 2)In the USS file, create a styling rule to customize the appearance of the marker.
    /// <example>
    /// MyStyle
    /// {
    ///   /* Specify the appearance of the marker in the collapsed state here. */
    /// }
    ///
    /// MyStyle:checked
    /// {
    ///   /* Specify the appearance of the marker in the expanded state here. */
    /// }
    ///
    /// MyStyle:focused:checked
    /// {
    ///   /* Specify the appearance of the marker in the selected state here. */
    /// }
    /// </example>
    /// <seealso cref="UnityEngine.Timeline.Marker"/>
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomStyleAttribute : Attribute
    {
        /// <summary>
        /// The name of the USS style.
        /// </summary>
        public readonly string ussStyle;

        /// <param name="ussStyle">The name of the USS style.</param>
        public CustomStyleAttribute(string ussStyle)
        {
            this.ussStyle = ussStyle;
        }
    }

    /// <summary>
    /// Use this attribute to assign a clip, marker or track to a category in a submenu
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal class MenuCategoryAttribute : Attribute
    {
        /// <summary>
        /// The menu name of the category
        /// </summary>
        public readonly string category;

        public MenuCategoryAttribute(string category)
        {
            this.category = category ?? string.Empty;
        }
    }
}
