using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    class TimelineDataSource : TreeViewDataSource
    {
        readonly TimelineWindow m_TimelineWindow;
        readonly TimelineTreeViewGUI m_ParentGUI;

        public List<TimelineTrackBaseGUI> allTrackGuis { get; private set; }

        TreeViewItem treeroot
        {
            get { return m_RootItem; }
        }

        public TimelineDataSource(TimelineTreeViewGUI parentGUI, TreeViewController treeView, TimelineWindow sequencerWindow)
            : base(treeView)
        {
            m_TreeView.useExpansionAnimation = false;
            m_TimelineWindow = sequencerWindow;
            m_ParentGUI = parentGUI;
            FetchData();
        }

        public override bool IsExpanded(TreeViewItem item)
        {
            if (!IsExpandable(item))
                return true;

            return IsExpanded(item.id);
        }

        public override bool IsExpandable(TreeViewItem item)
        {
            var expandable = false;

            var track = item as TimelineTrackBaseGUI;

            if (track != null)
                expandable =  track.expandable;

            return expandable && item.hasChildren;
        }

        public sealed override void FetchData()
        {
            // create root item
            m_RootItem = new TimelineGroupGUI(m_TreeView, m_ParentGUI, 1, 0, null, "root", null, true);

            var tree = new Dictionary<TrackAsset, TimelineTrackBaseGUI>();

            var filteredView = m_TimelineWindow.state.editSequence.asset.trackObjects;
            allTrackGuis = new List<TimelineTrackBaseGUI>(filteredView.Count());

            foreach (var t in filteredView)
            {
                CreateItem(t, ref tree, filteredView.OfType<TrackAsset>(), m_RootItem);
            }

            m_NeedRefreshRows = true;

            SetExpanded(m_RootItem, true);
        }

        TimelineTrackBaseGUI CreateItem(ScriptableObject scriptableObject, ref Dictionary<TrackAsset, TimelineTrackBaseGUI> tree, IEnumerable<TrackAsset> selectedRows, TreeViewItem parentTreeViewItem)
        {
            // if a script doesn't load correctly, the trackAsset will be NULL, but the scriptableObject __should_ be intact (but == null will be true)
            var trackAsset = scriptableObject as TrackAsset;

            if (tree == null)
                throw new ArgumentNullException("tree");

            if (selectedRows == null)
                throw new ArgumentNullException("selectedRows");

            if (trackAsset != null && tree.ContainsKey(trackAsset))
                return tree[trackAsset];

            TimelineTrackBaseGUI parentItem = parentTreeViewItem as TimelineTrackBaseGUI;

            // should we create the parent?
            TrackAsset parentTrack = trackAsset != null ? (trackAsset.parent as TrackAsset) : null;
            if (trackAsset != null && parentTrack != null && selectedRows.Contains(parentTrack))
            {
                parentItem = CreateItem(parentTrack, ref tree, selectedRows, parentTreeViewItem);
            }

            int theDepth = -1;
            if (parentItem != null)
                theDepth = parentItem.depth;
            theDepth++;

            TimelineTrackBaseGUI newItem;
            if (trackAsset == null)
            {
                PlayableAsset parent = m_TimelineWindow.state.editSequence.asset;
                if (parentItem != null && parentItem.track != null)
                    parent = parentItem.track;

                newItem = new TimelineTrackErrorGUI(m_TreeView, m_ParentGUI, 0, theDepth, parentItem, "ERROR", scriptableObject, parent);
            }
            else if (trackAsset.GetType() != typeof(GroupTrack))
            {
                newItem = new TimelineTrackGUI(m_TreeView, m_ParentGUI, trackAsset.GetInstanceID(), theDepth, parentItem, trackAsset.name, trackAsset);
            }
            else
            {
                newItem = new TimelineGroupGUI(m_TreeView, m_ParentGUI, trackAsset.GetInstanceID(), theDepth, parentItem, trackAsset.name, trackAsset, false);
            }

            allTrackGuis.Add(newItem);

            if (parentItem != null)
            {
                if (parentItem.children == null)
                    parentItem.children = new List<TreeViewItem>();
                parentItem.children.Add(newItem);
            }
            else
            {
                m_RootItem = newItem;
                SetExpanded(m_RootItem, true);
            }

            if (trackAsset != null)
                tree[trackAsset] = newItem;

            var actorAsAnimTrack = newItem.track as AnimationTrack;
            bool isEditableInfiniteClip = actorAsAnimTrack != null && actorAsAnimTrack.ShouldShowInfiniteClipEditor();
            if (isEditableInfiniteClip)
            {
                if (newItem.children == null)
                    newItem.children = new List<TreeViewItem>();
            }
            else if (trackAsset != null)
            {
                // check if clips on this track have animation, if so we inline a animationEditorTrack
                bool clipHasAnimatableAnimationCurves = false;

                for (var i = 0; i != newItem.track.clips.Length; ++i)
                {
                    var curveClip = newItem.track.clips[i].curves;
                    var animationClip = newItem.track.clips[i].animationClip;

                    // prune out clip with zero curves
                    if (curveClip != null && curveClip.empty)
                        curveClip = null;

                    if (animationClip != null && animationClip.empty)
                        animationClip = null;

                    // prune out clips coming from FBX
                    if (animationClip != null && ((animationClip.hideFlags & HideFlags.NotEditable) != 0))
                        animationClip = null;

                    if (!newItem.track.clips[i].recordable)
                        animationClip = null;

                    clipHasAnimatableAnimationCurves = (curveClip != null) || (animationClip != null);
                    if (clipHasAnimatableAnimationCurves)
                        break;
                }

                if (clipHasAnimatableAnimationCurves)
                {
                    if (newItem.children == null)
                        newItem.children = new List<TreeViewItem>();
                }
            }

            if (trackAsset != null)
            {
                // Here we are using the internal subTrackObject so we can properly handle tracks whose script
                //  can't load (via ScriptableObject)
                foreach (var subTrack in trackAsset.subTracksObjects)
                {
                    CreateItem(subTrack, ref tree, selectedRows, newItem);
                }
            }
            return newItem;
        }

        public override bool CanBeParent(TreeViewItem item)
        {
            // will prevent track becoming subtracks via dragging
            TimelineTrackGUI track = item as TimelineTrackGUI;
            if (track != null)
                return false;

            return true;
        }

        public void ExpandItems(TreeViewItem item)
        {
            if (treeroot == item)
            {
                SetExpanded(treeroot, true);
            }

            TimelineGroupGUI gui = item as TimelineGroupGUI;
            if (gui != null && gui.track != null)
            {
                SetExpanded(item, !gui.track.GetCollapsed());
            }

            if (item.children != null)
            {
                for (int c = 0; c < item.children.Count; c++)
                {
                    ExpandItems(item.children[c]);
                }
            }
        }
    }
}
