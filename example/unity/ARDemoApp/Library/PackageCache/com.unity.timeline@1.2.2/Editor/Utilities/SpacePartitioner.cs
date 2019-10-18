using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    interface IBounds
    {
        Rect boundingRect { get; }
    }

    class SpacePartitioner
    {
        internal class CachedList<T>
        {
            public static readonly List<T> Instance = new List<T>(1000);
        }

        struct Entry : IInterval
        {
            public object item { get; set; }
            public long intervalStart { get; set; }
            public long intervalEnd { get; set; }
            public Rect bounds { get; set; }

            private const float kPrecision = 100.0f;
            private const float kMaxFloat = (float)long.MaxValue;
            private const float kMinFloat = (float)long.MinValue;

            static public Int64 FromFloat(float f)
            {
                if (Single.IsPositiveInfinity(f))
                    return long.MaxValue;
                if (Single.IsNegativeInfinity(f))
                    return long.MinValue;

                f = Mathf.Clamp(f, kMinFloat, kMaxFloat);              // prevent overflow of floats
                f = Mathf.Clamp(f * kPrecision, kMinFloat, kMaxFloat); // clamp to 'long' range
                return (long)(f);
            }
        }

        const EventType k_GuiEventLock = EventType.Repaint;

        IntervalTree<Entry> m_Tree = new IntervalTree<Entry>();
        List<Entry> m_CacheList = new List<Entry>();

        public void Clear()
        {
            m_Tree.Clear();
        }

        public void AddBounds(IBounds bounds)
        {
            AddBounds(bounds, bounds.boundingRect);
        }

        public void AddBounds(object item, Rect rect)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            m_Tree.Add(new Entry()
            {
                intervalStart = Entry.FromFloat(rect.yMin),
                intervalEnd = Entry.FromFloat(rect.yMax),
                bounds = rect,
                item = item
            }
            );
        }

        /// <summary>
        /// Get items of type T at a given position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="inClipSpace"></param>
        /// <typeparam name="T"></typeparam>
        /// <remarks>
        /// Uses a (1,1) sized box
        /// Use .ToList() or .ToArray() when not enumerating the result immediately
        /// </remarks>
        /// <returns></returns>
        public IEnumerable<T> GetItemsAtPosition<T>(Vector2 position)
        {
            return GetItemsInArea<T>(new Rect(position.x, position.y, 1, 1));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="area"></param>
        /// <param name="inClipSpace"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetItemsInArea<T>(Rect area)
        {
            m_CacheList.Clear();
            m_Tree.IntersectsWithRange(long.MinValue, long.MaxValue, m_CacheList);

            var list = CachedList<T>.Instance;
            list.Clear();
            foreach (var i in m_CacheList)
            {
                if (i.item is T && i.bounds.Overlaps(area))
                    list.Add((T)i.item);
            }
            return list;
        }

        public void DebugDraw()
        {
            var kFillColor = new Color(1.0f, 1.0f, 1.0f, 0.1f);
            var kOutlineColor = Color.yellow;

            m_CacheList.Clear();
            m_Tree.IntersectsWithRange(long.MinValue, long.MaxValue, m_CacheList);
            HandleUtility.ApplyWireMaterial();

            foreach (var item in m_CacheList)
            {
                Handles.DrawSolidRectangleWithOutline(item.bounds, kFillColor, kOutlineColor);
            }
        }
    }
}
