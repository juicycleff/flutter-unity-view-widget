using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Timeline
{
    /// <summary>
    /// Used to indicate path and priority of classes that are auto added to the menu
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal class MenuEntryAttribute : Attribute
    {
        public readonly int priority;
        public readonly string name;
        public readonly string subMenuPath;

        public MenuEntryAttribute(string path, int priority)
        {
            path = path ?? string.Empty;
            path = L10n.Tr(path);
            this.priority = priority;

            int index = path.LastIndexOf('/');
            if (index >= 0)
            {
                name = (index == path.Length - 1) ? string.Empty : path.Substring(index + 1);
                subMenuPath = path.Substring(0, index + 1);
            }
            else
            {
                name = path;
                subMenuPath = string.Empty;
            }
        }
    }
}
