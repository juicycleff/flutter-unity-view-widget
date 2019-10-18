using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
    struct Range
    {
        public double start;
        public double end;
        public double length { get { return end - start; } }

        public static Range Union(Range lhs, Range rhs)
        {
            return new Range
            {
                start = Math.Min(lhs.start, rhs.start),
                end = Math.Max(lhs.end, rhs.end)
            };
        }

        public static Range Intersection(Range lhs, Range rhs)
        {
            var s = Math.Max(lhs.start, rhs.start);
            var e = Math.Min(lhs.end, rhs.end);

            if (s > e)
            {
                // No intersection returns a 0-length range from 0 to 0
                return new Range();
            }

            return new Range
            {
                start = s,
                end = e
            };
        }

        public override string ToString()
        {
            return ToString("F3");
        }

        public string ToString(string format)
        {
            return UnityString.Format("({0}, {1})", start.ToString(format), end.ToString(format));
        }
    }
}
