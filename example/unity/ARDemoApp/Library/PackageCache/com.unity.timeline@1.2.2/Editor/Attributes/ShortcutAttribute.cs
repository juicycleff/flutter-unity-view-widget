using System;
using System.Linq;
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace UnityEditor.Timeline
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    class ShortcutAttribute : Attribute
    {
        readonly string m_Identifier;
        readonly string m_EventCommandName;
        readonly string m_MenuShortcut;

        public ShortcutAttribute(string identifier)
        {
            m_Identifier = identifier;
            m_EventCommandName = identifier;
        }

        public ShortcutAttribute(string identifier, string commandName)
        {
            m_Identifier = identifier;
            m_EventCommandName = commandName;
        }

        public ShortcutAttribute(KeyCode key, ShortcutModifiers modifiers = ShortcutModifiers.None)
        {
            m_MenuShortcut = new KeyCombination(key, modifiers).ToMenuShortcutString();
        }

        public string GetMenuShortcut()
        {
            if (m_MenuShortcut != null)
                return m_MenuShortcut;

            //find the mapped shortcut in the shortcut manager
            var shortcut = ShortcutIntegration.instance.directory.FindShortcutEntry(m_Identifier);
            if (shortcut != null && shortcut.combinations.Any())
            {
                return KeyCombination.SequenceToMenuString(shortcut.combinations);
            }

            return string.Empty;
        }

        public bool MatchesEvent(Event evt)
        {
            if (evt.type != EventType.ExecuteCommand)
                return false;
            return evt.commandName == m_EventCommandName;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    class ShortcutPlatformOverrideAttribute : ShortcutAttribute
    {
        RuntimePlatform platform { get; }

        public ShortcutPlatformOverrideAttribute(RuntimePlatform platform, KeyCode key, ShortcutModifiers modifiers = ShortcutModifiers.None)
            : base(key, modifiers)
        {
            this.platform = platform;
        }

        public bool MatchesCurrentPlatform()
        {
            return Application.platform == platform;
        }
    }
}
