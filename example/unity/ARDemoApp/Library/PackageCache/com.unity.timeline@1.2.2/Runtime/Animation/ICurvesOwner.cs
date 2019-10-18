namespace UnityEngine.Timeline
{
    interface ICurvesOwner
    {
        AnimationClip curves { get; }
        bool hasCurves { get; }
        double duration { get; }
        void CreateCurves(string curvesClipName);

        string defaultCurvesName { get; }
        Object asset { get; }
        Object assetOwner { get; }
        TrackAsset targetTrack { get; }
    }
}
