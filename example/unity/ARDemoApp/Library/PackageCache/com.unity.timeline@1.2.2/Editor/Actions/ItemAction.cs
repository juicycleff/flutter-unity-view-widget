using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    [ActiveInMode(TimelineModes.Default)]
    abstract class ItemAction<T> : MenuItemActionBase where T : class
    {
        public abstract bool Execute(WindowState state, T[] items);

        protected virtual MenuActionDisplayState GetDisplayState(WindowState state, T[] items)
        {
            return items.Length > 0 ? MenuActionDisplayState.Visible : MenuActionDisplayState.Disabled;
        }

        protected virtual string GetDisplayName(T[] items)
        {
            return menuName;
        }

        public bool CanExecute(WindowState state, T[] items)
        {
            return GetDisplayState(state, items) == MenuActionDisplayState.Visible;
        }

        protected virtual void AddMenuItem(WindowState state, T[] items, List<MenuActionItem> menuItem)
        {
            var mode = TimelineWindow.instance.currentMode.mode;
            menuItem.Add(
                new MenuActionItem()
                {
                    category = category,
                    entryName = GetDisplayName(items),
                    shortCut = this.shortCut,
                    isChecked = false,
                    isActiveInMode = IsActionActiveInMode(this, mode),
                    priority = priority,
                    state = GetDisplayState(state, items),
                    callback = () => Execute(state, items)
                }
            );
        }

        public static bool HandleShortcut(WindowState state, Event evt, T item)
        {
            T[] items = { item };

            foreach (ItemAction<T> action in actions)
            {
                var attr = action.GetType().GetCustomAttributes(typeof(ShortcutAttribute), true);

                foreach (ShortcutAttribute shortcut in attr)
                {
                    if (shortcut.MatchesEvent(evt))
                    {
                        if (s_ShowActionTriggeredByShortcut)
                            Debug.Log(action.GetType().Name);

                        if (!IsActionActiveInMode(action, TimelineWindow.instance.currentMode.mode))
                            return false;

                        var result = action.Execute(state, items);
                        state.Refresh();
                        state.Evaluate();
                        return result;
                    }
                }
            }

            return false;
        }

        static List<ItemAction<T>> s_ActionClasses;

        static List<ItemAction<T>> actions
        {
            get
            {
                if (s_ActionClasses == null)
                {
                    s_ActionClasses = GetActionsOfType(typeof(ItemAction<T>)).Select(x => (ItemAction<T>)x.GetConstructors()[0].Invoke(null)).ToList();
                }

                return s_ActionClasses;
            }
        }

        public static void GetMenuEntries(T[] items, List<MenuActionItem> menuItems)
        {
            if (items == null || items.Length == 0)
                return;

            foreach (var action in actions)
            {
                if (action.showInMenu)
                    action.AddMenuItem(TimelineWindow.instance.state, items, menuItems);
            }
        }

        public static bool Invoke<TAction>(WindowState state, T[] items)
            where TAction : ItemAction<T>
        {
            var itemsDerived = items.ToArray();

            if (!itemsDerived.Any())
                return false;

            var action = actions.FirstOrDefault(x => x.GetType() == typeof(TAction));

            if (action != null)
                return action.Execute(state, itemsDerived);

            return false;
        }

        public static bool Invoke<TAction>(WindowState state, T item)
            where TAction : ItemAction<T>
        {
            return Invoke<TAction>(state, new[] {item});
        }
    }
}
