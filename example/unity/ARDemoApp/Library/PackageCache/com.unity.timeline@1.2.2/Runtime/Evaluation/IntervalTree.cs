using System;
using System.Collections.Generic;

namespace UnityEngine.Timeline
{
    interface IInterval
    {
        Int64 intervalStart { get; }
        Int64 intervalEnd { get; }
    }

    struct IntervalTreeNode         // interval node,
    {
        public Int64 center;        // midpoint for this node
        public int first;           // index of first element of this node in m_Entries
        public int last;            // index of the last element of this node in m_Entries
        public int left;            // index in m_Nodes of the left subnode
        public int right;           // index in m_Nodes of the right subnode
    }

    class IntervalTree<T> where T : IInterval
    {
        internal struct Entry
        {
            public Int64 intervalStart;
            public Int64 intervalEnd;
            public T item;
        }

        const int kMinNodeSize = 10;     // the minimum number of entries to have subnodes
        const int kInvalidNode = -1;
        const Int64 kCenterUnknown = Int64.MaxValue; // center hasn't been calculated. indicates no children

        readonly List<Entry> m_Entries = new List<Entry>();
        readonly List<IntervalTreeNode> m_Nodes = new List<IntervalTreeNode>();

        /// <summary>
        /// Whether the tree will be rebuilt on the next query
        /// </summary>
        public bool dirty { get; internal set; }

        /// <summary>
        /// Add an IInterval to the tree
        /// </summary>
        public void Add(T item)
        {
            if (item == null)
                return;

            m_Entries.Add(
                new Entry()
                {
                    intervalStart = item.intervalStart,
                    intervalEnd = item.intervalEnd,
                    item = item
                }
            );
            dirty = true;
        }

        /// <summary>
        /// Query the tree at a particular time
        /// </summary>
        /// <param name="value"></param>
        /// <param name="results"></param>
        public void IntersectsWith(Int64 value, List<T> results)
        {
            if (m_Entries.Count == 0)
                return;

            if (dirty)
            {
                Rebuild();
                dirty = false;
            }

            if (m_Nodes.Count > 0)
                Query(m_Nodes[0], value, results);
        }

        /// <summary>
        /// Query the tree at a particular range of time
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="results"></param>
        public void IntersectsWithRange(Int64 start, Int64 end, List<T> results)
        {
            if (start > end)
                return;

            if (m_Entries.Count == 0)
                return;

            if (dirty)
            {
                Rebuild();
                dirty = false;
            }

            if (m_Nodes.Count > 0)
                QueryRange(m_Nodes[0], start, end, results);
        }

        /// <summary>
        /// Updates the intervals from their source. Use this to detect if the data in the tree
        /// has changed.
        /// </summary>
        public void UpdateIntervals()
        {
            bool isDirty = false;
            for (int i = 0; i < m_Entries.Count; i++)
            {
                var n = m_Entries[i];
                var s = n.item.intervalStart;
                var e = n.item.intervalEnd;

                isDirty |= n.intervalStart != s;
                isDirty |= n.intervalEnd != e;

                m_Entries[i] = new Entry()
                {
                    intervalStart = s,
                    intervalEnd = e,
                    item = n.item
                };
            }

            dirty |= isDirty;
        }

        private void Query(IntervalTreeNode intervalTreeNode, Int64 value, List<T> results)
        {
            for (int i = intervalTreeNode.first; i <= intervalTreeNode.last; i++)
            {
                var entry = m_Entries[i];
                if (value >= entry.intervalStart && value < entry.intervalEnd)
                {
                    results.Add(entry.item);
                }
            }

            if (intervalTreeNode.center == kCenterUnknown)
                return;
            if (intervalTreeNode.left != kInvalidNode && value < intervalTreeNode.center)
                Query(m_Nodes[intervalTreeNode.left], value, results);
            if (intervalTreeNode.right != kInvalidNode && value > intervalTreeNode.center)
                Query(m_Nodes[intervalTreeNode.right], value, results);
        }

        private void QueryRange(IntervalTreeNode intervalTreeNode, Int64 start, Int64 end, List<T> results)
        {
            for (int i = intervalTreeNode.first; i <= intervalTreeNode.last; i++)
            {
                var entry = m_Entries[i];
                if (end >= entry.intervalStart && start < entry.intervalEnd)
                {
                    results.Add(entry.item);
                }
            }

            if (intervalTreeNode.center == kCenterUnknown)
                return;
            if (intervalTreeNode.left != kInvalidNode && start < intervalTreeNode.center)
                QueryRange(m_Nodes[intervalTreeNode.left], start, end, results);
            if (intervalTreeNode.right != kInvalidNode && end > intervalTreeNode.center)
                QueryRange(m_Nodes[intervalTreeNode.right], start, end, results);
        }

        private void Rebuild()
        {
            m_Nodes.Clear();
            m_Nodes.Capacity = m_Entries.Capacity;
            Rebuild(0, m_Entries.Count - 1);
        }

        private int Rebuild(int start, int end)
        {
            IntervalTreeNode intervalTreeNode = new IntervalTreeNode();

            // minimum size, don't subdivide
            int count = end - start + 1;
            if (count < kMinNodeSize)
            {
                intervalTreeNode = new IntervalTreeNode() {center = kCenterUnknown, first = start, last = end, left = kInvalidNode, right = kInvalidNode};
                m_Nodes.Add(intervalTreeNode);
                return m_Nodes.Count - 1;
            }

            var min = Int64.MaxValue;
            var max = Int64.MinValue;

            for (int i = start; i <= end; i++)
            {
                var o = m_Entries[i];
                min = Math.Min(min, o.intervalStart);
                max = Math.Max(max, o.intervalEnd);
            }

            var center = (max + min) / 2;
            intervalTreeNode.center = center;

            // first pass, put every thing left of center, left
            int x = start;
            int y = end;
            while (true)
            {
                while (x <= end && m_Entries[x].intervalEnd < center)
                    x++;

                while (y >= start && m_Entries[y].intervalEnd >= center)
                    y--;

                if (x > y)
                    break;

                var nodeX = m_Entries[x];
                var nodeY = m_Entries[y];

                m_Entries[y] = nodeX;
                m_Entries[x] = nodeY;
            }

            intervalTreeNode.first = x;

            // second pass, put every start passed the center right
            y = end;
            while (true)
            {
                while (x <= end && m_Entries[x].intervalStart <= center)
                    x++;

                while (y >= start && m_Entries[y].intervalStart > center)
                    y--;

                if (x > y)
                    break;

                var nodeX = m_Entries[x];
                var nodeY = m_Entries[y];

                m_Entries[y] = nodeX;
                m_Entries[x] = nodeY;
            }

            intervalTreeNode.last = y;

            // reserve a place
            m_Nodes.Add(new IntervalTreeNode());
            int index = m_Nodes.Count - 1;

            intervalTreeNode.left = kInvalidNode;
            intervalTreeNode.right = kInvalidNode;

            if (start < intervalTreeNode.first)
                intervalTreeNode.left = Rebuild(start, intervalTreeNode.first - 1);

            if (end > intervalTreeNode.last)
                intervalTreeNode.right = Rebuild(intervalTreeNode.last + 1, end);

            m_Nodes[index] = intervalTreeNode;
            return index;
        }

        public void Clear()
        {
            m_Entries.Clear();
            m_Nodes.Clear();
        }
    }
}
