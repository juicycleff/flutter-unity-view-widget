using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.Timeline
{
    class TimelineClipUnion
    {
        List<TimelineClipGUI> m_Members = new List<TimelineClipGUI>();
        Rect m_BoundingRect;
        Rect m_Union;
        double m_Start;
        double m_Duration;
        bool m_InitUnionRect = true;

        void Add(TimelineClipGUI clip)
        {
            m_Members.Add(clip);
            if (m_Members.Count == 1)
            {
                m_BoundingRect = clip.clippedRect;
            }
            else
            {
                m_BoundingRect = Encompass(m_BoundingRect, clip.rect);
            }
        }

        public void Draw(Rect parentRect, WindowState state)
        {
            if (m_InitUnionRect)
            {
                m_Start = m_Members.OrderBy(c => c.clip.start).First().clip.start;
                m_Duration = m_Members.Sum(c => c.clip.duration);
                m_InitUnionRect = false;
            }

            m_Union = new Rect((float)(m_Start) * state.timeAreaScale.x, 0, (float)m_Duration * state.timeAreaScale.x, 0);

            // transform clipRect into pixel-space
            m_Union.xMin += state.timeAreaTranslation.x + parentRect.x;
            m_Union.xMax += state.timeAreaTranslation.x + parentRect.x;
            m_Union.y = parentRect.y + 4.0f;
            m_Union.height = parentRect.height - 8.0f;

            // calculate clipped rect
            if (m_Union.x < parentRect.xMin)
            {
                var overflow = parentRect.xMin - m_Union.x;
                m_Union.x = parentRect.xMin;
                m_Union.width -= overflow;
            }

            // bail out if completely clipped
            if (m_Union.xMax < parentRect.xMin)
                return;
            if (m_Union.xMin > parentRect.xMax)
                return;

            EditorGUI.DrawRect(m_Union, DirectorStyles.Instance.customSkin.colorClipUnion);
        }

        public static List<TimelineClipUnion> Build(List<TimelineClipGUI> clips)
        {
            var unions = new List<TimelineClipUnion>();
            if (clips == null)
                return unions;

            TimelineClipUnion currentUnion = null;
            foreach (var c in clips)
            {
                if (currentUnion == null)
                {
                    currentUnion = new TimelineClipUnion();
                    currentUnion.Add(c);
                    unions.Add(currentUnion);
                }
                else
                {
                    Rect result;
                    if (Intersection(c.rect, currentUnion.m_BoundingRect, out result))
                    {
                        currentUnion.Add(c);
                    }
                    else
                    {
                        currentUnion = new TimelineClipUnion();
                        currentUnion.Add(c);
                        unions.Add(currentUnion);
                    }
                }
            }

            return unions;
        }

        public static Rect Encompass(Rect a, Rect b)
        {
            Rect newRect = a;
            newRect.xMin = Mathf.Min(a.xMin, b.xMin);
            newRect.yMin = Mathf.Min(a.yMin, b.yMin);
            newRect.xMax = Mathf.Max(a.xMax, b.xMax);
            newRect.yMax = Mathf.Max(a.yMax, b.yMax);
            return newRect;
        }

        public static bool Intersection(Rect r1, Rect r2, out Rect intersection)
        {
            if (!r1.Overlaps(r2) && !r2.Overlaps(r1))
            {
                intersection = new Rect(0, 0, 0, 0);
                return false;
            }

            float left = Mathf.Max(r1.xMin, r2.xMin);
            float top = Mathf.Max(r1.yMin, r2.yMin);

            float right = Mathf.Min(r1.xMax, r2.xMax);
            float bottom = Mathf.Min(r1.yMax, r2.yMax);

            intersection = new Rect(left, top, right - left, bottom - top);
            return true;
        }
    }
}
