namespace UnityEditor.Timeline
{
    enum AttractedEdge
    {
        None,
        Left,
        Right
    }

    interface IAttractable
    {
        bool ShouldSnapTo(ISnappable snappable);
        double start { get; }
        double end { get; }
    }

    interface IAttractionHandler
    {
        void OnAttractedEdge(IAttractable attractable, ManipulateEdges manipulateEdges, AttractedEdge edge, double time);
    }
}
