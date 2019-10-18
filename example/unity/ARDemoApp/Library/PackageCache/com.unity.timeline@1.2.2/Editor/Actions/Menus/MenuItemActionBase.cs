using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityEditor.Timeline
{
    enum MenuActionDisplayState
    {
        Visible,
        Disabled,
        Hidden
    }

    struct MenuActionItem
    {
        public string category;
        public string entryName;
        public string shortCut;
        public int priority;
        public bool isChecked;
        public bool isActiveInMode;
        public MenuActionDisplayState state;
        public GenericMenu.MenuFunction callback;
    }

    class MenuItemActionBase
    {
        public Vector2? mousePosition { get; set; }

        protected static bool s_ShowActionTriggeredByShortcut = false;

        private static MenuEntryAttribute NoMenu = new MenuEntryAttribute(null, MenuOrder.DefaultPriority);
        private MenuEntryAttribute m_MenuInfo;
        private string m_ShortCut = null;


        public static IEnumerable<Type> GetActionsOfType(Type actionType)
        {
            var query = TypeCache.GetTypesDerivedFrom(actionType).Where(type => !type.IsGenericType && !type.IsNested && !type.IsAbstract);
            return query;
        }

        public static ShortcutAttribute GetShortcutAttributeForAction(MenuItemActionBase action)
        {
            var shortcutAttributes = action.GetType()
                .GetCustomAttributes(typeof(ShortcutAttribute), true)
                .Cast<ShortcutAttribute>();

            foreach (var shortcutAttribute in shortcutAttributes)
            {
                var shortcutOverride = shortcutAttribute as ShortcutPlatformOverrideAttribute;
                if (shortcutOverride != null)
                {
                    if (shortcutOverride.MatchesCurrentPlatform())
                        return shortcutOverride;
                }
                else
                {
                    return shortcutAttribute;
                }
            }

            return null;
        }

        public static void BuildMenu(GenericMenu menu, List<MenuActionItem> items)
        {
            // sorted the outer menu by priority, then sort the innermenu by priority
            var sortedItems =
                items.GroupBy(x => string.IsNullOrEmpty(x.category) ? x.entryName : x.category).
                    OrderBy(x => x.Min(y => y.priority)).
                    SelectMany(x => x.OrderBy(z => z.priority));

            int lastPriority = Int32.MinValue;
            string lastCategory = string.Empty;

            foreach (var s in sortedItems)
            {
                if (s.state == MenuActionDisplayState.Hidden)
                    continue;

                var priority = s.priority;
                if (lastPriority == Int32.MinValue)
                {
                    lastPriority = priority;
                }
                else if ((priority / MenuOrder.SeparatorAt) > (lastPriority / MenuOrder.SeparatorAt))
                {
                    string path = String.Empty;
                    if (lastCategory == s.category)
                        path = s.category;
                    menu.AddSeparator(path);
                }

                lastPriority = priority;
                lastCategory = s.category;

                string entry = s.category + s.entryName;
                if (!string.IsNullOrEmpty(s.shortCut))
                    entry += " " + s.shortCut;

                if (s.state == MenuActionDisplayState.Visible && s.isActiveInMode)
                    menu.AddItem(new GUIContent(entry), s.isChecked, s.callback);
                else
                    menu.AddDisabledItem(new GUIContent(entry));
            }
        }

        public static ActiveInModeAttribute GetActiveInModeAttribute(MenuItemActionBase action)
        {
            var attr = action.GetType().GetCustomAttributes(typeof(ActiveInModeAttribute), true);

            if (attr.Length > 0)
                return (attr[0] as ActiveInModeAttribute);

            return null;
        }

        public static bool IsActionActiveInMode(MenuItemActionBase action, TimelineModes mode)
        {
            ActiveInModeAttribute attr = GetActiveInModeAttribute(action);
            return attr != null && (attr.modes & mode) != 0;
        }

        public int priority
        {
            get { return menuInfo.priority; }
        }

        public string category
        {
            get { return menuInfo.subMenuPath; }
        }

        public string menuName
        {
            get
            {
                if (string.IsNullOrEmpty(menuInfo.name))
                    return L10n.Tr(GetType().Name);
                return menuInfo.name;
            }
        }

        // shortcut used by the menu
        public string shortCut
        {
            get
            {
                if (m_ShortCut == null)
                {
                    var shortcutAttribute = GetShortcutAttributeForAction(this);
                    m_ShortCut = shortcutAttribute == null ? string.Empty : shortcutAttribute.GetMenuShortcut();
                }
                return m_ShortCut;
            }
        }

        public bool showInMenu
        {
            get { return menuInfo != NoMenu; }
        }

        private MenuEntryAttribute menuInfo
        {
            get
            {
                if (m_MenuInfo == null)
                    m_MenuInfo = GetType().GetCustomAttributes(typeof(MenuEntryAttribute), false).OfType<MenuEntryAttribute>().DefaultIfEmpty(NoMenu).First();
                return m_MenuInfo;
            }
        }
    }
}
