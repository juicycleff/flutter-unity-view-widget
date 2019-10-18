using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityObject = UnityEngine.Object;

namespace UnityEditor
{
    class TimelineDragging : TreeViewDragging
    {
        public delegate bool TypeResolver(IEnumerable<Type> types, Action<Type> onComplete, string format);

        private static readonly string k_SelectTrackWithBinding = LocalizationDatabase.GetLocalizedString("Add {0}");
        private static readonly string k_SelectTrackWithClip = LocalizationDatabase.GetLocalizedString("Add Clip With {0}");
        private static readonly string k_SelectClip = LocalizationDatabase.GetLocalizedString("Add {0}");


        const string k_GenericDragId = "TimelineDragging";
        readonly int kDragSensitivity = 2;
        readonly TimelineAsset m_Timeline;
        readonly TimelineWindow m_Window;

        class TimelineDragData
        {
            public TimelineDragData(List<TreeViewItem> draggedItems)
            {
                this.draggedItems = draggedItems;
            }

            public readonly List<TreeViewItem> draggedItems;
        }

        public TimelineDragging(TreeViewController treeView, TimelineWindow window, TimelineAsset data)
            : base(treeView)
        {
            m_Timeline = data;
            m_Window = window;
        }

        public override bool CanStartDrag(TreeViewItem targetItem, List<int> draggedItemIDs, Vector2 mouseDownPosition)
        {
            if (Event.current.modifiers != EventModifiers.None)
                return false;

            // Can only drag when starting in the track header area
            if (mouseDownPosition.x > m_Window.sequenceHeaderRect.xMax)
                return false;

            var trackBaseGUI = targetItem as TimelineTrackBaseGUI;

            if (trackBaseGUI == null || trackBaseGUI.track == null)
                return false;

            if (trackBaseGUI.track.lockedInHierarchy)
                return false;

            if (Event.current.type == EventType.MouseDrag && Mathf.Abs(Event.current.delta.y) < kDragSensitivity)
                return false;

            // Make sure dragged items are selected
            // TODO Use similar system than the SceneHierarchyWindow in order to handle selection between treeView and tracks.
            SelectionManager.Clear();
            var draggedTrackGUIs = m_Window.allTracks.Where(t => draggedItemIDs.Contains(t.id));
            foreach (var trackGUI in draggedTrackGUIs)
                SelectionManager.Add(trackGUI.track);

            return true;
        }

        public override void StartDrag(TreeViewItem draggedNode, List<int> draggedItemIDs)
        {
            DragAndDrop.PrepareStartDrag();
            var tvItems = SelectionManager.SelectedTrackGUI().Cast<TreeViewItem>().ToList();
            DragAndDrop.SetGenericData(k_GenericDragId, new TimelineDragData(tvItems));
            DragAndDrop.objectReferences = new UnityObject[] {};  // this IS required for dragging to work

            string title = draggedItemIDs.Count + (draggedItemIDs.Count > 1 ? "s" : ""); // title is only shown on OSX (at the cursor)

            TimelineGroupGUI groupGui = draggedNode as TimelineGroupGUI;
            if (groupGui != null)
            {
                title = groupGui.displayName;
            }
            DragAndDrop.StartDrag(title);
        }

        public static bool IsDraggingEvent()
        {
            return Event.current.type == EventType.DragUpdated ||
                Event.current.type == EventType.DragExited ||
                Event.current.type == EventType.DragPerform;
        }

        public static bool ResolveType(IEnumerable<System.Type> types, Action<Type> onComplete, string formatString)
        {
            if (!types.Any() || onComplete == null)
                return false;

            if (types.Count() == 1)
            {
                onComplete(types.First());
                return true;
            }

            var menu = new GenericMenu();

            var builtInTypes = types.Where(TypeUtility.IsBuiltIn).OrderBy(TypeUtility.GetDisplayName).ToArray();
            var customTypes = types.Where(x => !TypeUtility.IsBuiltIn(x)).OrderBy(TypeUtility.GetDisplayName).ToArray();

            foreach (var t in builtInTypes)
            {
                menu.AddItem(new GUIContent(string.Format(formatString, TypeUtility.GetDisplayName(t))), false, s => onComplete((System.Type)s), t);
            }

            if (builtInTypes.Length != 0 && customTypes.Length != 0)
                menu.AddSeparator(string.Empty);

            foreach (var t in customTypes)
            {
                menu.AddItem(new GUIContent(string.Format(formatString, TypeUtility.GetDisplayName(t))), false, s => onComplete((System.Type)s), t);
            }

            menu.ShowAsContext();
            return true;
        }

        public override bool DragElement(TreeViewItem targetItem, Rect targetItemRect, int row)
        {
            if (TimelineWindow.instance.state.editSequence.isReadOnly)
                return false;
            // the drop rect contains the row rect plus additional spacing. The base drag element overlaps 1/2 the height of the next track
            // which interferes with track bindings
            var targetTrack = targetItem as TimelineGroupGUI;
            if (row > 0 && targetTrack != null && !targetTrack.dropRect.Contains(Event.current.mousePosition))
                return false;

            return base.DragElement(targetItem, targetItemRect, row);
        }

        TreeViewItem GetNextItem(TreeViewItem item)
        {
            if (item == null)
                return null;

            if (item.parent == null)
            {
                int row = m_Window.treeView.data.GetRow(item.id);
                var items = m_Window.treeView.data.GetRows();
                if (items.Count > row + 1)
                    return items[row + 1];
                return null;
            }

            var children = item.parent.children;
            if (children == null)
                return null;

            for (int i = 0; i < children.Count - 1; i++)
            {
                if (children[i] == item)
                    return children[i + 1];
            }
            return null;
        }

        private static TrackAsset GetTrack(TreeViewItem item)
        {
            TimelineTrackBaseGUI baseGui = item as TimelineTrackBaseGUI;
            if (baseGui == null)
                return null;
            return baseGui.track;
        }

        // The drag and drop may be over an expanded group but might be between tracks
        private void HandleNestedItemGUI(ref TreeViewItem parentItem, ref TreeViewItem targetItem, ref TreeViewItem insertBefore)
        {
            const float kTopPad = 5;
            const float kBottomPad = 5;

            insertBefore = null;

            if (!ShouldUseHierarchyDragAndDrop())
                return;

            var targetTrack = targetItem as TimelineGroupGUI;
            if (targetTrack == null)
                return;

            var mousePosition = Event.current.mousePosition;

            var dropBefore = targetTrack.rowRect.yMin + kTopPad > mousePosition.y;
            var dropAfter = !(targetTrack.track is GroupTrack) && (targetTrack.rowRect.yMax - kBottomPad < mousePosition.y);

            targetTrack.drawInsertionMarkerBefore = dropBefore;
            targetTrack.drawInsertionMarkerAfter = dropAfter;

            if (dropBefore)
            {
                targetItem = parentItem;
                parentItem = targetItem != null ? targetItem.parent : null;
                insertBefore = targetTrack;
            }
            else if (dropAfter)
            {
                targetItem = parentItem;
                parentItem = targetItem != null ? targetItem.parent : null;
                insertBefore = GetNextItem(targetTrack);
            }
            else if (targetTrack.track is GroupTrack)
            {
                targetTrack.isDropTarget = true;
            }
        }

        public override DragAndDropVisualMode DoDrag(TreeViewItem parentItem, TreeViewItem targetItem, bool perform, DropPosition dropPos)
        {
            m_Window.isDragging = false;

            var retMode = DragAndDropVisualMode.None;

            var trackDragData = DragAndDrop.GetGenericData(k_GenericDragId) as TimelineDragData;

            if (trackDragData != null)
            {
                retMode = HandleTrackDrop(parentItem, targetItem, perform, dropPos);
                if (retMode == DragAndDropVisualMode.Copy && targetItem != null && Event.current.type == EventType.DragUpdated)
                {
                    var targetActor = targetItem as TimelineGroupGUI;
                    if (targetActor != null)
                        targetActor.isDropTarget = true;
                }
            }
            else if (DragAndDrop.objectReferences.Any())
            {
                var objectsBeingDropped = DragAndDrop.objectReferences.OfType<UnityObject>();
                var director = m_Window.state.editSequence.director;

                if (ShouldUseHierarchyDragAndDrop())
                {
                    // for object drawing
                    var originalTarget = targetItem;
                    TreeViewItem insertBeforeItem = null;
                    HandleNestedItemGUI(ref parentItem, ref targetItem, ref insertBeforeItem);
                    var track = GetTrack(targetItem);
                    var parent = GetTrack(parentItem);
                    var insertBefore = GetTrack(insertBeforeItem);
                    retMode = HandleHierarchyPaneDragAndDrop(objectsBeingDropped, track, perform, m_Timeline, director, ResolveType, insertBefore);

                    // fallback to old clip behaviour
                    if (retMode == DragAndDropVisualMode.None)
                    {
                        retMode = HandleClipPaneObjectDragAndDrop(objectsBeingDropped, track, perform, m_Timeline, parent, director, m_Window.state.timeAreaShownRange.x, ResolveType, insertBefore);
                    }

                    // if we are rejected, clear any drop markers
                    if (retMode == DragAndDropVisualMode.Rejected && targetItem != null)
                    {
                        ClearInsertionMarkers(originalTarget);
                        ClearInsertionMarkers(targetItem);
                        ClearInsertionMarkers(parentItem);
                        ClearInsertionMarkers(insertBeforeItem);
                    }
                }
                else
                {
                    var candidateTime = TimelineHelpers.GetCandidateTime(m_Window.state, Event.current.mousePosition);
                    retMode = HandleClipPaneObjectDragAndDrop(objectsBeingDropped, GetTrack(targetItem), perform, m_Timeline, GetTrack(parentItem), director, candidateTime, ResolveType);
                }
            }

            m_Window.isDragging = false;

            return retMode;
        }

        void ClearInsertionMarkers(TreeViewItem item)
        {
            var trackGUI = item as TimelineTrackBaseGUI;
            if (trackGUI != null)
            {
                trackGUI.drawInsertionMarkerAfter = false;
                trackGUI.drawInsertionMarkerBefore = false;
                trackGUI.isDropTarget = false;
            }
        }

        bool ShouldUseHierarchyDragAndDrop()
        {
            return m_Window.state.IsEditingAnEmptyTimeline() || m_Window.state.sequencerHeaderWidth > Event.current.mousePosition.x;
        }

        public static DragAndDropVisualMode HandleHierarchyPaneDragAndDrop(IEnumerable<UnityObject> objectsBeingDropped, TrackAsset targetTrack, bool perform, TimelineAsset timeline, PlayableDirector director, TypeResolver typeResolver, TrackAsset insertBefore = null)
        {
            if (timeline == null)
                return DragAndDropVisualMode.Rejected;

            // if we are over a target track, defer to track binding system (implemented in TrackGUIs), unless we are a groupTrack
            if (targetTrack != null && (targetTrack as GroupTrack) == null)
                return DragAndDropVisualMode.None;

            if (targetTrack != null && targetTrack.lockedInHierarchy)
                return DragAndDropVisualMode.Rejected;

            var tracksWithBinding = objectsBeingDropped.SelectMany(TypeUtility.GetTracksCreatableFromObject).Distinct();
            if (!tracksWithBinding.Any())
                return DragAndDropVisualMode.None;

            if (perform)
            {
                System.Action<Type> onResolve = trackType =>
                {
                    foreach (var obj in objectsBeingDropped)
                    {
                        if (!obj.IsPrefab() && TypeUtility.IsTrackCreatableFromObject(obj, trackType))
                        {
                            var newTrack = TimelineHelpers.CreateTrack(timeline, trackType, targetTrack, string.Empty);
                            if (insertBefore != null)
                            {
                                if (targetTrack != null)
                                    targetTrack.MoveLastTrackBefore(insertBefore);
                                else
                                    timeline.MoveLastTrackBefore(insertBefore);
                            }

                            TimelineHelpers.Bind(newTrack, obj, director);
                        }
                    }
                    TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved);
                };
                typeResolver(tracksWithBinding, onResolve, k_SelectTrackWithBinding);
            }

            return DragAndDropVisualMode.Copy;
        }

        public static DragAndDropVisualMode HandleClipPaneObjectDragAndDrop(IEnumerable<UnityObject> objectsBeingDropped, TrackAsset targetTrack, bool perform, TimelineAsset timeline, TrackAsset parent, PlayableDirector director, double candidateTime, TypeResolver typeResolver, TrackAsset insertBefore = null)
        {
            if (timeline == null)
                return DragAndDropVisualMode.Rejected;

            // locked tracks always reject
            if (targetTrack != null && targetTrack.lockedInHierarchy)
                return DragAndDropVisualMode.Rejected;

            // treat group tracks as having no track
            if (targetTrack is GroupTrack)
            {
                parent = targetTrack;
                targetTrack = null;
            }

            // Special case for monoscripts, since they describe the type
            if (objectsBeingDropped.Any(o => o is MonoScript))
                return HandleClipPaneMonoScriptDragAndDrop(objectsBeingDropped.OfType<MonoScript>(), targetTrack, perform, timeline, parent, director, candidateTime);

            // no unity objects, or explicit exceptions
            if (!objectsBeingDropped.Any() || objectsBeingDropped.Any(o => !ValidateObjectDrop(o)))
                return DragAndDropVisualMode.Rejected;

            // reject scene references if we have no context
            if (director == null && objectsBeingDropped.Any(o => o.IsSceneObject()))
                return DragAndDropVisualMode.Rejected;

            var validTrackTypes = objectsBeingDropped.SelectMany(o => TypeUtility.GetTrackTypesForObject(o)).Distinct().ToList();
            // special case for playable assets
            if (objectsBeingDropped.Any(o => TypeUtility.IsConcretePlayableAsset(o.GetType())))
            {
                var playableAssets = objectsBeingDropped.OfType<IPlayableAsset>().Where(o => TypeUtility.IsConcretePlayableAsset(o.GetType()));
                return HandleClipPanePlayableAssetDragAndDrop(playableAssets, targetTrack, perform, timeline, parent, director, candidateTime, typeResolver);
            }

            var markerTypes = objectsBeingDropped.SelectMany(o => TypeUtility.MarkerTypesWithFieldForObject(o)).Distinct();

            // Markers support all tracks
            if (!markerTypes.Any())
            {
                // No tracks support this object
                if (!validTrackTypes.Any())
                    return DragAndDropVisualMode.Rejected;

                // no tracks for this object
                if (targetTrack != null && !validTrackTypes.Contains(targetTrack.GetType()))
                    return DragAndDropVisualMode.Rejected;
            }

            // there is no target track, dropping to empty space, or onto a group
            if (perform)
            {
                // choose track and then clip
                if (targetTrack == null)
                {
                    var createdTrack = HandleTrackAndItemCreation(objectsBeingDropped, candidateTime, typeResolver, timeline, parent, validTrackTypes, insertBefore);
                    if (!createdTrack)
                    {
                        timeline.CreateMarkerTrack();
                        HandleItemCreation(objectsBeingDropped, timeline.markerTrack, candidateTime, typeResolver, true); // menu is always popped if ambiguous choice
                    }
                }
                // just choose clip/marker
                else
                {
                    HandleItemCreation(objectsBeingDropped, targetTrack, candidateTime, typeResolver, true); // menu is always popped if ambiguous choice
                }
            }

            return DragAndDropVisualMode.Copy;
        }

        static bool HandleTrackAndItemCreation(IEnumerable<UnityEngine.Object> objectsBeingDropped, double candidateTime, TypeResolver typeResolver, TimelineAsset timeline, TrackAsset parent, IEnumerable<Type> validTrackTypes, TrackAsset insertBefore = null)
        {
            Action<Type> onResolved = t =>
            {
                var newTrack = TimelineHelpers.CreateTrack(timeline, t, parent, string.Empty);
                if (insertBefore != null)
                {
                    if (parent != null)
                        parent.MoveLastTrackBefore(insertBefore);
                    else
                        timeline.MoveLastTrackBefore(insertBefore);
                }
                HandleItemCreation(objectsBeingDropped, newTrack, candidateTime, typeResolver, validTrackTypes.Count() == 1); // menu is popped if ambiguous clip choice and unambiguous track choice
            };
            return typeResolver(validTrackTypes, t => onResolved(t), k_SelectTrackWithClip); // Did it create a track
        }

        static void HandleItemCreation(IEnumerable<UnityEngine.Object> objectsBeingDropped, TrackAsset targetTrack, double candidateTime, TypeResolver typeResolver, bool allowMenu)
        {
            var assetTypes = objectsBeingDropped.Select(o =>
                TypeUtility.GetAssetTypesForObject(targetTrack.GetType(), o)
                    .Union(TypeUtility.MarkerTypesWithFieldForObject(o))).ToList();
            Action<Type> onCreateItem = assetType =>
            {
                if (typeof(PlayableAsset).IsAssignableFrom(assetType))
                {
                    TimelineHelpers.CreateClipsFromObjects(assetType, targetTrack, candidateTime,
                        objectsBeingDropped);
                }
                else
                {
                    TimelineHelpers.CreateMarkersFromObjects(assetType, targetTrack, candidateTime, objectsBeingDropped);
                }
            };

            var flatAssetTypes = assetTypes.SelectMany(x => x).Distinct();
            // If there is a one to one mapping between assets and timeline types, no need to go through the type resolution, not ambiguous.
            if (assetTypes.All(x => x.Count() <= 1))
            {
                foreach (var type in flatAssetTypes)
                {
                    onCreateItem(type);
                }
            }
            else
            {
                if (!allowMenu) // If we already popped a menu, and are presented with an ambiguous choice, take the first entry
                {
                    flatAssetTypes = new[] {flatAssetTypes.First()};
                }

                typeResolver(flatAssetTypes, onCreateItem, k_SelectClip);
            }
        }

        /// Handles drag and drop of a mono script.
        public static DragAndDropVisualMode HandleClipPaneMonoScriptDragAndDrop(IEnumerable<MonoScript> scriptsBeingDropped, TrackAsset targetTrack, bool perform, TimelineAsset timeline, TrackAsset parent, PlayableDirector director, double candidateTime)
        {
            var playableAssetTypes = scriptsBeingDropped.Select(s => s.GetClass()).Where(TypeUtility.IsConcretePlayableAsset).Distinct();
            if (!playableAssetTypes.Any())
                return DragAndDropVisualMode.Rejected;

            var targetTrackType = typeof(PlayableTrack);
            if (targetTrack != null)
                targetTrackType = targetTrack.GetType();

            var trackAssetsTypes = TypeUtility.GetPlayableAssetsHandledByTrack(targetTrackType);
            var supportedTypes = trackAssetsTypes.Intersect(playableAssetTypes);
            if (!supportedTypes.Any())
                return DragAndDropVisualMode.Rejected;

            if (perform)
            {
                if (targetTrack == null)
                    targetTrack = TimelineHelpers.CreateTrack(timeline, targetTrackType, parent, string.Empty);
                TimelineHelpers.CreateClipsFromTypes(supportedTypes, targetTrack, candidateTime);
            }

            return DragAndDropVisualMode.Copy;
        }

        public static DragAndDropVisualMode HandleClipPanePlayableAssetDragAndDrop(IEnumerable<IPlayableAsset> assetsBeingDropped, TrackAsset targetTrack, bool perform, TimelineAsset timeline, TrackAsset parent, PlayableDirector director, double candidateTime, TypeResolver typeResolver)
        {
            // get the list of supported track types
            var assetTypes = assetsBeingDropped.Select(x => x.GetType()).Distinct();
            IEnumerable<Type> supportedTypes = null;
            if (targetTrack == null)
            {
                supportedTypes = TypeUtility.AllTrackTypes().Where(t => TypeUtility.GetPlayableAssetsHandledByTrack(t).Intersect(assetTypes).Any()).ToList();
            }
            else
            {
                supportedTypes = Enumerable.Empty<Type>();
                var trackAssetTypes = TypeUtility.GetPlayableAssetsHandledByTrack(targetTrack.GetType());
                if (trackAssetTypes.Intersect(assetTypes).Any())
                    supportedTypes = new[] {targetTrack.GetType()};
            }

            if (!supportedTypes.Any())
                return DragAndDropVisualMode.Rejected;

            if (perform)
            {
                Action<Type> onResolved = (t) =>
                {
                    if (targetTrack == null)
                        targetTrack = TimelineHelpers.CreateTrack(timeline, t, parent, string.Empty);

                    var clipTypes = TypeUtility.GetPlayableAssetsHandledByTrack(targetTrack.GetType());
                    foreach (var asset in assetsBeingDropped)
                    {
                        if (clipTypes.Contains(asset.GetType()))
                            TimelineHelpers.CreateClipOnTrackFromPlayableAsset(asset, targetTrack, candidateTime);
                    }
                };

                typeResolver(supportedTypes, onResolved, k_SelectTrackWithClip);
            }


            return DragAndDropVisualMode.Copy;
        }

        static bool ValidateObjectDrop(UnityObject obj)
        {
            // legacy animation clips are not supported at all
            AnimationClip clip = obj as AnimationClip;
            if (clip != null && clip.legacy)
                return false;

            return !(obj is TimelineAsset);
        }

        public DragAndDropVisualMode HandleTrackDrop(TreeViewItem parentItem, TreeViewItem targetItem, bool perform, DropPosition dropPos)
        {
            ((TimelineTreeView)m_Window.treeView.gui).showInsertionMarker = false;
            var trackDragData = (TimelineDragData)DragAndDrop.GetGenericData(k_GenericDragId);
            bool validDrag = ValidDrag(targetItem, trackDragData.draggedItems);
            if (!validDrag)
                return DragAndDropVisualMode.None;


            var draggedTracks = trackDragData.draggedItems.OfType<TimelineGroupGUI>().Select(x => x.track).ToList();
            if (draggedTracks.Count == 0)
                return DragAndDropVisualMode.None;

            if (parentItem != null)
            {
                var parentActor = parentItem as TimelineGroupGUI;
                if (parentActor != null && parentActor.track != null)
                {
                    if (parentActor.track.lockedInHierarchy)
                        return DragAndDropVisualMode.Rejected;

                    if (draggedTracks.Any(x => !TimelineCreateUtilities.ValidateParentTrack(parentActor.track, x.GetType())))
                        return DragAndDropVisualMode.Rejected;
                }
            }

            var insertAfterItem = targetItem as TimelineGroupGUI;
            if (insertAfterItem != null && insertAfterItem.track != null)
            {
                ((TimelineTreeView)m_Window.treeView.gui).showInsertionMarker = true;
            }

            if (dropPos == DropPosition.Upon)
            {
                var groupGUI = targetItem as TimelineGroupGUI;
                if (groupGUI != null)
                    groupGUI.isDropTarget = true;
            }

            if (perform)
            {
                PlayableAsset targetParent = m_Timeline;
                var parentActor = parentItem as TimelineGroupGUI;

                if (parentActor != null && parentActor.track != null)
                    targetParent = parentActor.track;

                TrackAsset siblingTrack = insertAfterItem != null ? insertAfterItem.track : null;

                // where the user drops after the last track, make sure to place it after all the tracks
                if (targetParent == m_Timeline && dropPos == DropPosition.Below && siblingTrack == null)
                {
                    siblingTrack = m_Timeline.GetRootTracks().LastOrDefault(x => !draggedTracks.Contains(x));
                }

                if (TrackExtensions.ReparentTracks(TrackExtensions.FilterTracks(draggedTracks).ToList(), targetParent, siblingTrack, dropPos == DropPosition.Above))
                {
                    m_Window.state.Refresh();
                }
            }

            return DragAndDropVisualMode.Move;
        }

        public static void HandleBindingDragAndDrop(TrackAsset dropTarget, Type requiredBindingType)
        {
            var objectBeingDragged = DragAndDrop.objectReferences[0];

            var action = BindingUtility.GetBindingAction(requiredBindingType, objectBeingDragged);
            DragAndDrop.visualMode = action == BindingAction.DoNotBind
                ? DragAndDropVisualMode.Rejected
                : DragAndDropVisualMode.Link;

            if (action == BindingAction.DoNotBind || Event.current.type != EventType.DragPerform)
                return;

            var director = TimelineEditor.inspectedDirector;

            switch (action)
            {
                case BindingAction.BindDirectly:
                {
                    BindingUtility.Bind(director, dropTarget, objectBeingDragged);
                    break;
                }
                case BindingAction.BindToExistingComponent:
                {
                    var gameObjectBeingDragged = objectBeingDragged as GameObject;
                    Debug.Assert(gameObjectBeingDragged != null, "The object being dragged was detected as being a GameObject");

                    BindingUtility.Bind(director, dropTarget, gameObjectBeingDragged.GetComponent(requiredBindingType));
                    break;
                }
                case BindingAction.BindToMissingComponent:
                {
                    var gameObjectBeingDragged = objectBeingDragged as GameObject;
                    Debug.Assert(gameObjectBeingDragged != null, "The object being dragged was detected as being a GameObject");

                    var typeNameOfComponent = requiredBindingType.ToString().Split(".".ToCharArray()).Last();
                    var bindMenu = new GenericMenu();

                    bindMenu.AddItem(
                        EditorGUIUtility.TextContent("Create " + typeNameOfComponent + " on " + gameObjectBeingDragged.name),
                        false,
                        nullParam => BindingUtility.Bind(director, dropTarget, Undo.AddComponent(gameObjectBeingDragged, requiredBindingType)),
                        null);

                    bindMenu.AddSeparator("");
                    bindMenu.AddItem(EditorGUIUtility.TrTextContent("Cancel"), false, userData => {}, null);
                    bindMenu.ShowAsContext();

                    break;
                }
                default:
                {
                    //no-op
                    return;
                }
            }

            DragAndDrop.AcceptDrag();
        }

        static bool ValidDrag(TreeViewItem target, List<TreeViewItem> draggedItems)
        {
            TreeViewItem currentParent = target;
            while (currentParent != null)
            {
                if (draggedItems.Contains(currentParent))
                    return false;
                currentParent = currentParent.parent;
            }

            // dragging into the sequence itself
            return true;
        }
    }
}
