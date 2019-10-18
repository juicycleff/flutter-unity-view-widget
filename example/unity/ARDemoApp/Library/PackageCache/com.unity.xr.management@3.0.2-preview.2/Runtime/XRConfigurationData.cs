using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace UnityEngine.XR.Management
{
    /// <summary>
    /// This attribute is used to tag classes as providing build settings support for an XR SDK provider. The unified setting system
    /// will present the settings as an inspectable object in the Unified Settings window using the built-in inspector UI.
    ///
    /// The implementor of the settings is able to create their own custom UI and the Unified Settings system will use that UI in
    /// place of the build in inspector. See the &lt;a href="https://docs.unity3d.com/Manual/ExtendingTheEditor.html">&gt;Extending the Editor&lt;/a&gt;
    /// portion of the Unity documentation for information and instructions on doing this.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class XRConfigurationDataAttribute : Attribute
    {
        /// <summary>
        /// The display name to be presented to the user in the Unified Settings window.
        /// </summary>
        public string displayName { get; set; }

        /// <summary>
        /// The key that will be used to store the singleton instance of these settings within EditorBuildSettings.
        ///
        /// See &lt;a href="https://docs.unity3d.com/ScriptReference/EditorBuildSettings.html"&gt;EditorBuildSettings&lt;/a&gt; scripting
        /// API documentation on how this is beign done.
        /// </summary>
        public string buildSettingsKey { get; set; }

        private XRConfigurationDataAttribute() {}

        /// <summary>Constructor for attribute</summary>
        /// <param name="displayName">The display name to use in the Project Settings window.</param>
        /// <param name="buildSettingsKey">The key to use to get/set build settings with.</param>
        public XRConfigurationDataAttribute(string displayName, string buildSettingsKey)
        {
            this.displayName = displayName;
            this.buildSettingsKey = buildSettingsKey;
        }
    }
}
