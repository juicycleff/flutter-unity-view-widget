using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    class TimelineTreeViewGUI
    {
        readonly TimelineAsset m_Timeline;
        readonly TreeViewController m_TreeView;
        readonly TimelineTreeView m_TimelineTreeView;
        readonly TimelineWindow m_Window;
        readonly TimelineDataSource m_DataSource;

        TreeViewItem root
        {
            get { return m_DataSource.root; }
        }

        TimelineTrackBaseGUI[] visibleTrackGuis
        {
            get
            {
                int firstRow;
                int lastRow;
                var visibleRows = new List<TimelineTrackBaseGUI>();
                m_TreeView.gui.GetFirstAndLastRowVisible(out firstRow, out lastRow);

                for (int r = firstRow; r <= lastRow; r++)
                {
                    var track = m_TreeView.data.GetItem(r) as TimelineTrackBaseGUI;
                    if (track != null && track != root)
                    {
                        AddVisibleTrackRecursive(ref visibleRows, track);
                    }
                }
                return visibleRows.ToArray();
            }
        }

        public TrackAsset[] visibleTracks
        {
            get { return visibleTrackGuis.Select(x => x.track).ToArray(); }
        }

        public List<TimelineClipGUI> allClipGuis
        {
            get
            {
                TimelineDataSource dataSource = m_TreeView.data as TimelineDataSource;
                if (dataSource != null && dataSource.allTrackGuis != null)
                    return dataSource.allTrackGuis.OfType<TimelineTrackGUI>().SelectMany(x => x.clips).ToList();

                return null;
            }
        }

        public List<TimelineTrackBaseGUI> allTrackGuis
        {
            get
            {
                var dataSource = m_TreeView.data as TimelineDataSource;
                if (dataSource != null)
                    return dataSource.allTrackGuis;
                return null;
            }
        }

        public Vector2 contentSize
        {
            get { return m_TreeView.GetContentSize(); }
        }

        public Vector2 scrollPosition
        {
            get { return m_TreeView.state.scrollPos; }
            set
            {
                Rect r = m_TreeView.GetTotalRect();
                Vector2 visibleContent = m_TreeView.GetContentSize();
                m_TreeView.state.scrollPos = new Vector2(value.x, Mathf.Min(Mathf.Clamp(value.y, 0.0f, visibleContent.y - r.height)));
            }
        }

        public bool showingVerticalScrollBar
        {
            get { return m_TreeView.showingVerticalScrollBar; }
        }

        public void FrameItem(TreeViewItem item)
        {
            m_TreeView.Frame(item.id, true, false, true);
        }

        public TimelineDragging timelineDragging { get {return m_TreeView.dragging as TimelineDragging; }}

        public TimelineTreeViewGUI(TimelineWindow sequencerWindow, TimelineAsset timeline,  Rect rect)
        {
            m_Timeline = timeline;
            m_Window = sequencerWindow;

            var treeviewState = new TreeViewState();
            treeviewState.scrollPos = new Vector2(treeviewState.scrollPos.x, TimelineWindowViewPrefs.GetOrCreateViewModel(m_Timeline).verticalScroll);

            m_TreeView = new TreeViewController(sequencerWindow, treeviewState);
            m_TreeView.horizontalScrollbarStyle = GUIStyle.none;
            m_TreeView.scrollViewStyle = GUI.skin.scrollView;
            m_TreeView.keyboardInputCallback = sequencerWindow.TreeViewKeyboardCallback;


            m_TimelineTreeView = new TimelineTreeView(sequencerWindow, m_TreeView);
            var dragging = new TimelineDragging(m_TreeView, m_Window, m_Timeline);
            m_DataSource = new TimelineDataSource(this, m_TreeView, sequencerWindow);

            m_DataSource.onVisibleRowsChanged += m_TimelineTreeView.CalculateRowRects;
            m_TreeView.Init(rect, m_DataSource, m_TimelineTreeView, dragging);

            m_DataSource.ExpandItems(m_DataSource.root);
        }

        public ITreeViewGUI gui
        {
            get { return m_TimelineTreeView; }
        }
        public ITreeViewDataSource data
        {
            get { return m_TreeView == null ? null : m_TreeView.data; }
        }

        public TimelineWindow TimelineWindow
        {
            get { return m_Window; }
        }

        public void CalculateRowRects()
        {
            m_TimelineTreeView.CalculateRowRects();
        }

        public void Reload()
        {
            m_TreeView.ReloadData();
            m_DataSource.ExpandItems(m_DataSource.root);
            m_TimelineTreeView.CalculateRowRects();
        }

        public void OnGUI(Rect rect)
        {
            int keyboardControl = GUIUtility.GetControlID(FocusType.Passive, rect);
            m_TreeView.OnGUI(rect, keyboardControl);
            TimelineWindowViewPrefs.GetOrCreateViewModel(m_Timeline).verticalScroll = m_TreeView.state.scrollPos.y;
        }

        public Rect GetRowRect(int row)
        {
            return m_TimelineTreeView.GetRowRect(row);
        }

        static void AddVisibleTrackRecursive(ref List<TimelineTrackBaseGUI> list, TimelineTrackBaseGUI track)
        {
            if (track == null)
                return;

            list.Add(track);

            if (!track.isExpanded)
                return;

            if (track.children != null)
            {
                foreach (var c in track.children)
                {
                    AddVisibleTrackRecursive(ref list, c as TimelineTrackBaseGUI);
                }
            }
        }
    }
}
