namespace UnityEngine.Timeline
{
    partial class TimelineClip
    {
        enum Versions
        {
            Initial = 0,
            ClipInFromGlobalToLocal = 1
        }
        const int k_LatestVersion = (int)Versions.ClipInFromGlobalToLocal;
        [SerializeField, HideInInspector] int m_Version;

        //fields that are used for upgrading should be put here, ideally as read-only

        void UpgradeToLatestVersion()
        {
            if (m_Version < (int)Versions.ClipInFromGlobalToLocal)
            {
                TimelineClipUpgrade.UpgradeClipInFromGlobalToLocal(this);
            }
        }

        static class TimelineClipUpgrade
        {
            // version 0->1, clipIn move from global to local
            public static void UpgradeClipInFromGlobalToLocal(TimelineClip clip)
            {
                // case 936751 -- clipIn was serialized in global, not local offset
                if (clip.m_ClipIn > 0 && clip.m_TimeScale > float.Epsilon)
                    clip.m_ClipIn *= clip.m_TimeScale;
            }
        }
    }
}
