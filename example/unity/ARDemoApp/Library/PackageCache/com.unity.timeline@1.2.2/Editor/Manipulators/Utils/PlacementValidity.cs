namespace UnityEditor.Timeline
{
    enum PlacementValidity
    {
        Valid,
        InvalidContains,
        InvalidIsWithin,
        InvalidStartsInBlend,
        InvalidEndsInBlend,
        InvalidContainsBlend,
        InvalidOverlapWithNonBlendableClip
    }
}
