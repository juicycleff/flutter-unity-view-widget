using System.Collections.Generic;

namespace UnityEditor.Timeline
{
    struct Edge
    {
        public double time { get; set; }

        public bool showSnapHint { get; set; }

        public Edge(double edgeTime, bool snapHint = true) : this()
        {
            time = edgeTime;
            showSnapHint = snapHint;
        }
    }

    interface ISnappable
    {
        IEnumerable<Edge> SnappableEdgesFor(IAttractable attractable, ManipulateEdges manipulateEdges);
    }
}
