namespace UnityEngine.Timeline
{
    partial class TimelineAsset
    {
        enum Versions
        {
            Initial = 0
        }
        const int k_LatestVersion = (int)Versions.Initial;
        [SerializeField, HideInInspector] int m_Version;

        //fields that are used for upgrading should be put here, ideally as read-only

        void UpgradeToLatestVersion()
        {}

        //upgrade code should go into this class
        static class TimelineAssetUpgrade
        {}
    }
}
