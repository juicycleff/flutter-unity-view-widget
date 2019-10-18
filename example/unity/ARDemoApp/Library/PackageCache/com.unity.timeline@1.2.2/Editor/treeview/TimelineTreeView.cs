using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditor.Timeline
{
    class TimelineTreeView : ITreeViewGUI
    {
        float m_FoldoutWidth;
        Rect m_DraggingInsertionMarkerRect;
        readonly TreeViewController m_TreeView;

        List<Rect> m_RowRects = new List<Rect>();
        List<Rect> m_ExpandedRowRects = new List<Rect>();

        float m_MaxWidthOfRows;
        readonly WindowState m_State;

        static readonly float kMinTrackHeight = 25.0f;
        static readonly float kFoldOutOffset = 14.0f;

        static DirectorStyles m_Styles;

        public bool showInsertionMarker { get; set; }
        public virtual float topRowMargin { get; private set; }
        public virtual float bottomRowMargin { get; private set; }

        public TimelineTreeView(TimelineWindow sequencerWindow, TreeViewController treeView)
        {
            m_TreeView = treeView;
            m_TreeView.useExpansionAnimation = true;

            m_TreeView.selectionChangedCallback += SelectionChangedCallback;
            m_TreeView.contextClickOutsideItemsCallback += ContextClickOutsideItemsCallback;
            m_TreeView.itemDoubleClickedCallback += ItemDoubleClickedCallback;
            m_TreeView.contextClickItemCallback += ContextClickItemCallback;

            m_TreeView.SetConsumeKeyDownEvents(false);
            m_Styles = DirectorStyles.Instance;
            m_State = sequencerWindow.state;

            m_FoldoutWidth = DirectorStyles.Instance.foldout.fixedWidth;
        }

        void ItemDoubleClickedCallback(int id)
        {
            var trackGUI = m_TreeView.FindItem(id) as TimelineTrackGUI;
            if (trackGUI == null)
                return;

            if (trackGUI.track == null || trackGUI.track.lockedInHierarchy)
                return;

            var selection = SelectionManager.SelectedItems().ToList();
            var items = ItemsUtils.GetItems(trackGUI.track).ToList();
            var addToSelection = !selection.SequenceEqual(items);

            foreach (var i in items)
            {
                if (addToSelection)
                    SelectionManager.Add(i);
                else
                    SelectionManager.Remove(i);
            }
        }

        void ContextClickOutsideItemsCallback()
        {
            SequencerContextMenu.ShowNewTracksContextMenu(null, m_State);
            Event.current.Use();
        }

        void ContextClickItemCallback(int id)
        {
            // may not occur if another menu is active
            if (!m_TreeView.IsSelected(id))
                SelectionChangedCallback(new[] {id});

            SequencerContextMenu.ShowTrackContextMenu(SelectionManager.SelectedTracks().ToArray(), Event.current.mousePosition);

            Event.current.Use();
        }

        void SelectionChangedCallback(int[] ids)
        {
            if (Event.current.button == 1 && PickerUtils.PickedLayerableOfType<ISelectable>() != null)
                return;

            if (Event.current.command || Event.current.control || Event.current.shift)
                SelectionManager.UnSelectTracks();
            else
                SelectionManager.Clear();

            foreach (var id in ids)
            {
                var trackGUI = (TimelineTrackBaseGUI)m_TreeView.FindItem(id);
                SelectionManager.Add(trackGUI.track);
            }

            m_State.GetWindow().Repaint();
        }

        public void OnInitialize() {}

        public Rect GetRectForFraming(int row)
        {
            return GetRowRect(row, 1); // We ignore width by default when framing (only y scroll is affected)
        }

        protected virtual Vector2 GetSizeOfRow(TreeViewItem item)
        {
            if (item.displayName == "root")
                return new Vector2(m_TreeView.GetTotalRect().width, 0.0f);

            var trackGroupGui = item as TimelineGroupGUI;
            if (trackGroupGui != null)
            {
                return new Vector2(m_TreeView.GetTotalRect().width, trackGroupGui.GetHeight(m_State));
            }

            float height = TrackEditor.DefaultTrackHeight;
            if (item.hasChildren && m_TreeView.data.IsExpanded(item))
            {
                height = Mathf.Min(TrackEditor.DefaultTrackHeight, kMinTrackHeight);
            }

            return new Vector2(m_TreeView.GetTotalRect().width, height);
        }

        public virtual void BeginRowGUI()
        {
            if (m_TreeView.GetTotalRect().width != GetRowRect(0).width)
            {
                CalculateRowRects();
            }

            m_DraggingInsertionMarkerRect.x = -1;

            m_TreeView.SetSelection(SelectionManager.SelectedTrackGUI().Select(t => t.id).ToArray(), false);
        }

        public virtual void EndRowGUI()
        {
            // Draw row marker when dragging
            if (m_DraggingInsertionMarkerRect.x >= 0 && Event.current.type == EventType.Repaint)
            {
                Rect insertionRect = m_DraggingInsertionMarkerRect;
                const float insertionHeight = 1.0f;
                insertionRect.height = insertionHeight;

                if (m_TreeView.dragging.drawRowMarkerAbove)
                    insertionRect.y -= insertionHeight * 0.5f + 2.0f;
                else
                    insertionRect.y += m_DraggingInsertionMarkerRect.height - insertionHeight * 0.5f + 1.0f;

                EditorGUI.DrawRect(insertionRect, Color.white);
            }
        }

        public virtual void OnRowGUI(Rect rowRect, TreeViewItem item, int row, bool selected, bool focused)
        {
            using (new EditorGUI.DisabledScope(TimelineWindow.instance.currentMode.TrackState(TimelineWindow.instance.state) == TimelineModeGUIState.Disabled))
            {
                var sqvi = (TimelineTrackBaseGUI)item;
                sqvi.treeViewToWindowTransformation = m_TreeView.GetTotalRect().position - m_TreeView.state.scrollPos;

                // this may be called because an encompassing parent is visible
                if (!sqvi.visibleExpanded)
                    return;

                Rect headerRect = rowRect;
                Rect contentRect = rowRect;

                headerRect.width = m_State.sequencerHeaderWidth - 2.0f;
                contentRect.xMin += m_State.sequencerHeaderWidth;
                contentRect.width = rowRect.width - m_State.sequencerHeaderWidth - 1.0f;

                Rect foldoutRect = rowRect;

                var indent = GetFoldoutIndent(item);
                var headerRectWithIndent = headerRect;
                headerRectWithIndent.xMin = indent;
                var rowRectWithIndent = new Rect(rowRect.x + indent, rowRect.y, rowRect.width - indent, rowRect.height);
                sqvi.Draw(headerRectWithIndent, contentRect, m_State);
                sqvi.DrawInsertionMarkers(rowRectWithIndent);

                if (Event.current.type == EventType.Repaint)
                {
                    m_State.spacePartitioner.AddBounds(sqvi);

                    // Show marker below this Item
                    if (showInsertionMarker)
                    {
                        if (m_TreeView.dragging != null && m_TreeView.dragging.GetRowMarkerControlID() == TreeViewController.GetItemControlID(item))
                            m_DraggingInsertionMarkerRect = rowRectWithIndent;
                    }
                }

                // Draw foldout (after text content above to ensure drop down icon is rendered above selection highlight)
                DrawFoldout(item, foldoutRect, indent);

                sqvi.ClearDrawFlags();
            }
        }

        private void DrawFoldout(TreeViewItem item, Rect foldoutRect, float indent)
        {
            bool showFoldout = m_TreeView.data.IsExpandable(item);
            if (showFoldout)
            {
                foldoutRect.x = indent - kFoldOutOffset;
                foldoutRect.width = m_FoldoutWidth;
                EditorGUI.BeginChangeCheck();
                float foldoutIconHeight = DirectorStyles.Instance.foldout.fixedHeight;
                foldoutRect.y += foldoutIconHeight / 2.0f;
                foldoutRect.height = foldoutIconHeight;

                //Override Disable state for TrakGroup toggle button to expand/collapse group.
                bool previousEnableState = GUI.enabled;
                GUI.enabled = true;
                bool newExpandedValue = GUI.Toggle(foldoutRect, m_TreeView.data.IsExpanded(item), GUIContent.none, m_Styles.foldout);
                GUI.enabled = previousEnableState;

                if (EditorGUI.EndChangeCheck())
                {
                    if (Event.current.alt)
                        m_TreeView.data.SetExpandedWithChildren(item, newExpandedValue);
                    else
                        m_TreeView.data.SetExpanded(item, newExpandedValue);
                }
            }
        }

        public Rect GetRenameRect(Rect rowRect, int row, TreeViewItem item)
        {
            return rowRect;
        }

        public void BeginPingItem(TreeViewItem item, float topPixelOfRow, float availableWidth) {}
        public void EndPingItem() {}

        public Rect GetRowRect(int row,  float rowWidth)
        {
            return GetRowRect(row);
        }

        public Rect GetRowRect(int row)
        {
            if (m_RowRects.Count == 0)
                return new Rect();

            if (row >= m_RowRects.Count)
                return new Rect();

            return m_RowRects[row];
        }

        static float GetSpacing(TreeViewItem item)
        {
            var trackBase = item as TimelineTrackBaseGUI;
            if (trackBase != null)
                return trackBase.GetVerticalSpacingBetweenTracks();

            return 3.0f;
        }

        public void CalculateRowRects()
        {
            if (m_TreeView.isSearching)
                return;

            const float startY = 6.0f;
            IList<TreeViewItem> rows = m_TreeView.data.GetRows();
            m_RowRects = new List<Rect>(rows.Count);
            m_ExpandedRowRects = new List<Rect>(rows.Count);

            float curY = startY;
            m_MaxWidthOfRows = 1f;

            // first pass compute the row rects
            for (int i = 0; i < rows.Count; ++i)
            {
                var item = rows[i];

                if (i != 0)
                    curY += GetSpacing(item);

                Vector2 rowSize = GetSizeOfRow(item);
                m_RowRects.Add(new Rect(0, curY, rowSize.x, rowSize.y));
                m_ExpandedRowRects.Add(m_RowRects[i]);

                curY += rowSize.y;

                if (rowSize.x > m_MaxWidthOfRows)
                    m_MaxWidthOfRows = rowSize.x;

                // updated the expanded state
                var groupGUI = item as TimelineGroupGUI;
                if (groupGUI != null)
                    groupGUI.SetExpanded(m_TreeView.data.IsExpanded(item));
            }

            float halfHeight = halfDropBetweenHeight;
            const float kGroupPad = 1.0f;
            const float kSkinPadding = 5.0f * 0.6f;
            // work bottom up and compute visible regions for groups
            for (int i = rows.Count - 1; i > 0; i--)
            {
                float height = 0;
                TimelineTrackBaseGUI item = (TimelineTrackBaseGUI)rows[i];
                if (item.isExpanded && item.children != null && item.children.Count > 0)
                {
                    for (var j = 0; j < item.children.Count; j++)
                    {
                        var child = item.children[j];
                        int index = rows.IndexOf(child);
                        if (index > i)
                            height += m_ExpandedRowRects[index].height + kSkinPadding;
                    }

                    height += kGroupPad;
                }
                m_ExpandedRowRects[i] = new Rect(m_RowRects[i].x, m_RowRects[i].y, m_RowRects[i].width, m_RowRects[i].height + height);

                var groupGUI = item as TimelineGroupGUI;
                if (groupGUI != null)
                {
                    var spacing = GetSpacing(item) + 1;
                    groupGUI.expandedRect = m_ExpandedRowRects[i];
                    groupGUI.rowRect = m_RowRects[i];
                    groupGUI.dropRect = new Rect(m_RowRects[i].x, m_RowRects[i].y - spacing, m_RowRects[i].width, m_RowRects[i].height + Mathf.Max(halfHeight, spacing));
                }
            }
        }

        public virtual bool BeginRename(TreeViewItem item, float delay)
        {
            return false;
        }

        public virtual void EndRename() {}

        protected virtual float GetFoldoutIndent(TreeViewItem item)
        {
            // Ignore depth when showing search results
            if (item.depth <= 1 || m_TreeView.isSearching)
                return DirectorStyles.kBaseIndent;

            int depth = item.depth;
            var trackGUI = item as TimelineTrackGUI;

            // first level subtracks are not indented
            if (trackGUI != null && trackGUI.track != null && trackGUI.track.isSubTrack)
                depth--;

            return depth * DirectorStyles.kBaseIndent;
        }

        public virtual float GetContentIndent(TreeViewItem item)
        {
            return GetFoldoutIndent(item);
        }

        public int GetNumRowsOnPageUpDown(TreeViewItem fromItem, bool pageUp, float heightOfTreeView)
        {
            return (int)Mathf.Floor(heightOfTreeView / 30); // return something
        }

        // Should return the row number of the first and last row thats fits in the pixel rect defined by top and height
        public void GetFirstAndLastRowVisible(out int firstRowVisible, out int lastRowVisible)
        {
            int rowCount = m_TreeView.data.rowCount;
            if (rowCount == 0)
            {
                firstRowVisible = lastRowVisible = -1;
                return;
            }

            if (rowCount != m_ExpandedRowRects.Count)
            {
                Debug.LogError("Mismatch in state: rows vs cached rects. Did you remember to hook up: dataSource.onVisibleRowsChanged += gui.CalculateRowRects ?");
                CalculateRowRects();
            }

            float topPixel = m_TreeView.state.scrollPos.y;
            float heightInPixels = m_TreeView.GetTotalRect().height;

            int firstVisible = -1;
            int lastVisible = -1;

            Rect visibleRect = new Rect(0, topPixel, m_ExpandedRowRects[0].width, heightInPixels);
            for (int i = 0; i < m_ExpandedRowRects.Count; ++i)
            {
                bool visible = visibleRect.Overlaps(m_ExpandedRowRects[i]);
                if (visible)
                {
                    if (firstVisible == -1)
                        firstVisible = i;
                    lastVisible = i;
                }

                TimelineTrackBaseGUI gui = m_TreeView.data.GetItem(i) as TimelineTrackBaseGUI;
                if (gui != null)
                {
                    gui.visibleExpanded = visible;
                    gui.visibleRow = visibleRect.Overlaps(m_RowRects[i]);
                }
            }

            if (firstVisible != -1 && lastVisible != -1)
            {
                firstRowVisible = firstVisible;
                lastRowVisible = lastVisible;
            }
            else
            {
                firstRowVisible = 0;
                lastRowVisible = rowCount - 1;
            }
        }

        public Vector2 GetTotalSize()
        {
            if (m_RowRects.Count == 0)
                return new Vector2(0, 0);

            return new Vector2(m_MaxWidthOfRows, m_RowRects[m_RowRects.Count - 1].yMax);
        }

        public virtual float halfDropBetweenHeight
        {
            get { return 8f; }
        }
    }
}
