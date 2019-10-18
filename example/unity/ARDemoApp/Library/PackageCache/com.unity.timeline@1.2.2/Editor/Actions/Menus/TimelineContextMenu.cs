using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Object = UnityEngine.Object;


namespace UnityEditor.Timeline
{
    static class SequencerContextMenu
    {
        private static readonly TimelineAction[] MarkerHeaderCommonOperations =
        {
            new PasteAction()
        };

        public static readonly TimelineAction[] MarkerHeaderMenuItems =
            TimelineAction.AllActions.OfType<MarkerHeaderAction>().
                Where(a => a.showInMenu).
                Union(MarkerHeaderCommonOperations).
                ToArray();


        static class Styles
        {
            public static readonly string addItemFromAssetTemplate       = L10n.Tr("Add {0} From {1}");
            public static readonly string addSingleItemFromAssetTemplate = L10n.Tr("Add From {1}");
            public static readonly string addItemTemplate                = L10n.Tr("Add {0}");
            public static readonly string typeSelectorTemplate           = L10n.Tr("Select {0}");
            public static readonly string trackGroup                     = L10n.Tr("Track Group");
            public static readonly string trackSubGroup                  = L10n.Tr("Track Sub-Group");
            public static readonly string addTrackLayer                  = L10n.Tr("Add Layer");
            public static readonly string layerName                      = L10n.Tr("Layer {0}");
        }

        public static void ShowMarkerHeaderContextMenu(Vector2? mousePosition, WindowState state)
        {
            var menu = new GenericMenu();
            List<MenuActionItem> items = new List<MenuActionItem>(100);
            BuildMarkerHeaderContextMenu(items, mousePosition, state);
            MenuItemActionBase.BuildMenu(menu, items);
            menu.ShowAsContext();
        }

        public static void ShowNewTracksContextMenu(ICollection<TrackAsset> tracks, WindowState state)
        {
            var menu = new GenericMenu();
            List<MenuActionItem> items = new List<MenuActionItem>(100);
            BuildNewTracksContextMenu(items, tracks, state);
            MenuItemActionBase.BuildMenu(menu, items);
            menu.ShowAsContext();
        }

        public static void ShowTrackContextMenu(TrackAsset[] tracks, Vector2? mousePosition)
        {
            if (tracks == null || tracks.Length == 0)
                return;

            var items = new List<MenuActionItem>();
            var menu = new GenericMenu();
            BuildTrackContextMenu(items, tracks, mousePosition);
            MenuItemActionBase.BuildMenu(menu, items);
            menu.ShowAsContext();
        }

        public static void ShowItemContextMenu(Vector2 mousePosition, TimelineClip[] clips, IMarker[] markers)
        {
            var menu = new GenericMenu();
            var items = new List<MenuActionItem>();
            BuildItemContextMenu(items, mousePosition, clips, markers);
            MenuItemActionBase.BuildMenu(menu, items);
            menu.ShowAsContext();
        }

        internal static void BuildItemContextMenu(List<MenuActionItem> items, Vector2 mousePosition, TimelineClip[] clips, IMarker[] markers)
        {
            var state = TimelineWindow.instance.state;

            TimelineAction.GetMenuEntries(TimelineAction.MenuActions, mousePosition, items);
            ItemAction<TimelineClip>.GetMenuEntries(clips, items);
            ItemAction<IMarker>.GetMenuEntries(markers, items);

            if (clips.Length > 0)
                AddMarkerMenuCommands(items, clips.Select(c => c.parentTrack).Distinct().ToList(), TimelineHelpers.GetCandidateTime(state, mousePosition));
        }

        internal static void BuildNewTracksContextMenu(List<MenuActionItem> menuItems, ICollection<TrackAsset> parentTracks, WindowState state, string format = null)
        {
            if (parentTracks == null)
                parentTracks = new TrackAsset[0];

            if (string.IsNullOrEmpty(format))
                format = "{0}";

            // Add Group or SubGroup
            var title = string.Format(format, parentTracks.Any(t => t != null) ? Styles.trackSubGroup : Styles.trackGroup);
            var menuState = MenuActionDisplayState.Visible;
            if (state.editSequence.isReadOnly)
                menuState = MenuActionDisplayState.Disabled;
            if (parentTracks.Any() && parentTracks.Any(t => t != null && t.lockedInHierarchy))
                menuState = MenuActionDisplayState.Disabled;

            GenericMenu.MenuFunction command = () =>
            {
                SelectionManager.Clear();
                if (parentTracks.Count == 0)
                    Selection.Add(TimelineHelpers.CreateTrack<GroupTrack>(null, title));

                foreach (var parentTrack in parentTracks)
                    Selection.Add(TimelineHelpers.CreateTrack<GroupTrack>(parentTrack, title));

                TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved);
            };

            menuItems.Add(
                new MenuActionItem()
                {
                    category = string.Empty,
                    entryName = title,
                    shortCut = string.Empty,
                    isActiveInMode = true,
                    isChecked = false,
                    priority =  MenuOrder.AddGroupItemStart,
                    state = menuState,
                    callback = command
                }
            );


            var allTypes = TypeUtility.AllTrackTypes().Where(x => x != typeof(GroupTrack) && !TypeUtility.IsHiddenInMenu(x)).ToList();

            int builtInPriority = MenuOrder.AddTrackItemStart;
            int customPriority = MenuOrder.AddCustomTrackItemStart;
            foreach (var trackType in allTypes)
            {
                var trackItemType = trackType;

                command = () =>
                {
                    SelectionManager.Clear();

                    if (parentTracks.Count == 0)
                        SelectionManager.Add(TimelineHelpers.CreateTrack((Type)trackItemType, null));

                    foreach (var parentTrack in parentTracks)
                        SelectionManager.Add(TimelineHelpers.CreateTrack((Type)trackItemType, parentTrack));
                };

                menuItems.Add(
                    new MenuActionItem()
                    {
                        category = TimelineHelpers.GetTrackCategoryName(trackType),
                        entryName = string.Format(format, TimelineHelpers.GetTrackMenuName(trackItemType)),
                        shortCut = string.Empty,
                        isActiveInMode = true,
                        isChecked = false,
                        priority = TypeUtility.IsBuiltIn(trackType) ? builtInPriority++ : customPriority++,
                        state = menuState,
                        callback = command
                    }
                );
            }
        }

        internal static void BuildMarkerHeaderContextMenu(List<MenuActionItem> menu, Vector2? mousePosition, WindowState state)
        {
            TimelineAction.GetMenuEntries(MarkerHeaderMenuItems, null, menu);

            var timeline = state.editSequence.asset;
            var time = TimelineHelpers.GetCandidateTime(state, mousePosition);
            var enabled = timeline.markerTrack == null || !timeline.markerTrack.lockedInHierarchy;

            var addMarkerCommand = new Action<Type, Object>
                (
                (type, obj) => AddSingleMarkerCallback(type, time, timeline, state.editSequence.director, obj)
                );

            AddMarkerMenuCommands(menu, new TrackAsset[] {timeline.markerTrack}, addMarkerCommand, enabled);
        }

        internal static void BuildTrackContextMenu(List<MenuActionItem> items, TrackAsset[] tracks, Vector2? mousePosition)
        {
            if (tracks == null || tracks.Length == 0)
                return;

            TimelineAction.GetMenuEntries(TimelineAction.MenuActions, mousePosition, items);
            TrackAction.GetMenuEntries(TimelineWindow.instance.state, mousePosition, tracks, items);
            AddLayeredTrackCommands(items, tracks);

            var first = tracks.First().GetType();
            var allTheSame = tracks.All(t => t.GetType() == first);
            if (allTheSame)
            {
                if (first != typeof(GroupTrack))
                {
                    var candidateTime = TimelineHelpers.GetCandidateTime(TimelineWindow.instance.state, mousePosition, tracks);
                    AddClipMenuCommands(items, tracks, candidateTime);
                    AddMarkerMenuCommands(items, tracks, candidateTime);
                }
                else
                {
                    BuildNewTracksContextMenu(items, tracks, TimelineWindow.instance.state, Styles.addItemTemplate);
                }
            }
        }

        static void AddLayeredTrackCommands(List<MenuActionItem> menuItems, ICollection<TrackAsset> tracks)
        {
            if (tracks.Count == 0)
                return;

            var layeredType = tracks.First().GetType();
            // animation tracks have a special menu.
            if (layeredType == typeof(AnimationTrack))
                return;

            // must implement ILayerable
            if (!typeof(UnityEngine.Timeline.ILayerable).IsAssignableFrom(layeredType))
                return;

            if (tracks.Any(t => t.GetType() != layeredType))
                return;

            // only supported on the master track no nesting.
            if (tracks.Any(t => t.isSubTrack))
                return;

            var enabled = tracks.All(t => t != null && !t.lockedInHierarchy) && !TimelineWindow.instance.state.editSequence.isReadOnly;
            int priority = MenuOrder.TrackAddMenu.AddLayerTrack;
            GenericMenu.MenuFunction menuCallback = () =>
            {
                foreach (var track in tracks)
                    TimelineHelpers.CreateTrack(layeredType, track, string.Format(Styles.layerName, track.GetChildTracks().Count() + 1));
            };

            var entryName = Styles.addTrackLayer;
            menuItems.Add(
                new MenuActionItem()
                {
                    category = string.Empty,
                    entryName = entryName,
                    shortCut = string.Empty,
                    isActiveInMode = true,
                    isChecked = false,
                    priority = priority++,
                    state = enabled ? MenuActionDisplayState.Visible : MenuActionDisplayState.Disabled,
                    callback = menuCallback
                }
            );
        }

        static void AddClipMenuCommands(List<MenuActionItem> menuItems, ICollection<TrackAsset> tracks, double candidateTime)
        {
            if (!tracks.Any())
                return;

            var trackAsset = tracks.First();
            var trackType = trackAsset.GetType();
            if (tracks.Any(t => t.GetType() != trackType))
                return;

            var enabled = tracks.All(t => t != null && !t.lockedInHierarchy) && !TimelineWindow.instance.state.editSequence.isReadOnly;
            var assetTypes = TypeUtility.GetPlayableAssetsHandledByTrack(trackType);
            var visibleAssetTypes = TypeUtility.GetVisiblePlayableAssetsHandledByTrack(trackType);

            // skips the name if there is only a single type
            var commandNameTemplate = assetTypes.Count() == 1 ? Styles.addSingleItemFromAssetTemplate : Styles.addItemFromAssetTemplate;
            int builtInPriority = MenuOrder.AddClipItemStart;
            int customPriority = MenuOrder.AddCustomClipItemStart;
            foreach (var assetType in assetTypes)
            {
                var assetItemType = assetType;
                var category = TimelineHelpers.GetItemCategoryName(assetType);
                Action<Object> onObjectChanged = obj =>
                {
                    if (obj != null)
                    {
                        foreach (var t in tracks)
                        {
                            TimelineHelpers.CreateClipOnTrack(assetItemType, obj, t, candidateTime);
                        }
                    }
                };

                foreach (var objectReference in TypeUtility.ObjectReferencesForType(assetType))
                {
                    var isSceneReference = objectReference.isSceneReference;
                    var dataType = objectReference.type;
                    GenericMenu.MenuFunction menuCallback = () =>
                    {
                        ObjectSelector.get.Show(null, dataType, null, isSceneReference, null, (obj) => onObjectChanged(obj), null);
                        ObjectSelector.get.titleContent = EditorGUIUtility.TrTextContent(string.Format(Styles.typeSelectorTemplate, TypeUtility.GetDisplayName(dataType)));
                    };

                    menuItems.Add(
                        new MenuActionItem()
                        {
                            category = category,
                            entryName = string.Format(commandNameTemplate, TypeUtility.GetDisplayName(assetType), TypeUtility.GetDisplayName(objectReference.type)),
                            shortCut = string.Empty,
                            isActiveInMode = true,
                            isChecked = false,
                            priority = TypeUtility.IsBuiltIn(assetType) ? builtInPriority++ : customPriority++,
                            state = enabled ? MenuActionDisplayState.Visible : MenuActionDisplayState.Disabled,
                            callback = menuCallback
                        }
                    );
                }
            }

            foreach (var assetType in visibleAssetTypes)
            {
                var assetItemType = assetType;
                var category = TimelineHelpers.GetItemCategoryName(assetType);
                var commandName = string.Format(Styles.addItemTemplate, TypeUtility.GetDisplayName(assetType));
                GenericMenu.MenuFunction command = () =>
                {
                    foreach (var t in tracks)
                    {
                        TimelineHelpers.CreateClipOnTrack(assetItemType, t, candidateTime);
                    }
                };

                menuItems.Add(
                    new MenuActionItem()
                    {
                        category = category,
                        entryName = commandName,
                        shortCut = string.Empty,
                        isActiveInMode = true,
                        isChecked = false,
                        priority = TypeUtility.IsBuiltIn(assetItemType) ? builtInPriority++ : customPriority++,
                        state = enabled ? MenuActionDisplayState.Visible : MenuActionDisplayState.Disabled,
                        callback = command
                    }
                );
            }
        }

        static void AddMarkerMenuCommands(List<MenuActionItem> menu, IEnumerable<Type> markerTypes, Action<Type, Object> addMarkerCommand, bool enabled)
        {
            int builtInPriority = MenuOrder.AddMarkerItemStart;
            int customPriority = MenuOrder.AddCustomMarkerItemStart;
            foreach (var markerType in markerTypes)
            {
                var markerItemType = markerType;
                string category = TimelineHelpers.GetItemCategoryName(markerItemType);
                menu.Add(
                    new MenuActionItem()
                    {
                        category = category,
                        entryName = string.Format(Styles.addItemTemplate, TypeUtility.GetDisplayName(markerType)),
                        shortCut = string.Empty,
                        isActiveInMode = true,
                        isChecked = false,
                        priority = TypeUtility.IsBuiltIn(markerType) ? builtInPriority++ : customPriority++,
                        state = enabled ? MenuActionDisplayState.Visible : MenuActionDisplayState.Disabled,
                        callback = () => addMarkerCommand(markerItemType, null)
                    }
                );

                foreach (var objectReference in TypeUtility.ObjectReferencesForType(markerType))
                {
                    var isSceneReference = objectReference.isSceneReference;
                    GenericMenu.MenuFunction menuCallback = () =>
                    {
                        var dataType = markerItemType;
                        ObjectSelector.get.Show(null, dataType, null, isSceneReference, null, (obj) => addMarkerCommand(markerItemType, obj), null);
                        ObjectSelector.get.titleContent = EditorGUIUtility.TrTextContent(string.Format(Styles.typeSelectorTemplate, TypeUtility.GetDisplayName(dataType)));
                    };

                    menu.Add(
                        new MenuActionItem()
                        {
                            category = TimelineHelpers.GetItemCategoryName(markerItemType),
                            entryName = string.Format(Styles.addItemFromAssetTemplate, TypeUtility.GetDisplayName(markerType), TypeUtility.GetDisplayName(objectReference.type)),
                            shortCut = string.Empty,
                            isActiveInMode = true,
                            isChecked = false,
                            priority = TypeUtility.IsBuiltIn(markerType) ? builtInPriority++ : customPriority++,
                            state = enabled ? MenuActionDisplayState.Visible : MenuActionDisplayState.Disabled,
                            callback = menuCallback
                        }
                    );
                }
            }
        }

        static void AddMarkerMenuCommands(List<MenuActionItem> menuItems, ICollection<TrackAsset> tracks, double candidateTime)
        {
            if (tracks.Count == 0)
                return;

            var enabled = tracks.All(t => !t.lockedInHierarchy) && !TimelineWindow.instance.state.editSequence.isReadOnly;
            var addMarkerCommand = new Action<Type, Object>((type, obj) => AddMarkersCallback(tracks, type, candidateTime, obj));

            AddMarkerMenuCommands(menuItems, tracks, addMarkerCommand, enabled);
        }

        static void AddMarkerMenuCommands(List<MenuActionItem> menuItems, ICollection<TrackAsset> tracks, Action<Type, Object> command, bool enabled)
        {
            var markerTypes = TypeUtility.GetBuiltInMarkerTypes().Union(TypeUtility.GetUserMarkerTypes());
            if (tracks != null)
                markerTypes = markerTypes.Where(x => tracks.All(track => (track == null) || TypeUtility.DoesTrackSupportMarkerType(track, x))); // null track indicates marker track to be created

            AddMarkerMenuCommands(menuItems, markerTypes, command, enabled);
        }

        static void AddMarkersCallback(ICollection<TrackAsset> targets, Type markerType, double time, Object obj)
        {
            SelectionManager.Clear();
            foreach (var target in targets)
            {
                var marker = TimelineHelpers.CreateMarkerOnTrack(markerType, obj, target, time);
                SelectionManager.Add(marker);
            }
            TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved);
        }

        static void AddSingleMarkerCallback(Type markerType, double time, TimelineAsset timeline, PlayableDirector director, Object assignableObject)
        {
            timeline.CreateMarkerTrack();
            var markerTrack = timeline.markerTrack;

            SelectionManager.Clear();
            var marker = TimelineHelpers.CreateMarkerOnTrack(markerType, assignableObject, markerTrack, time);
            SelectionManager.Add(marker);

            if (typeof(INotification).IsAssignableFrom(markerType) && director != null)
            {
                if (director != null && director.GetGenericBinding(markerTrack) == null)
                    director.SetGenericBinding(markerTrack, director.gameObject);
            }

            TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved);
        }
    }
}
