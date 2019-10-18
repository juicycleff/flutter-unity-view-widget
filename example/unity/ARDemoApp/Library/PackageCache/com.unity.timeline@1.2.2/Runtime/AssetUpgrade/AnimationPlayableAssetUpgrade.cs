using System;

namespace UnityEngine.Timeline
{
    partial class AnimationPlayableAsset : ISerializationCallbackReceiver
    {
        enum Versions
        {
            Initial = 0,
            RotationAsEuler = 1,
        }
        static readonly int k_LatestVersion = (int)Versions.RotationAsEuler;
        [SerializeField, HideInInspector] int m_Version;

        [SerializeField, Obsolete("Use m_RotationEuler Instead", false), HideInInspector]
        private Quaternion m_Rotation = Quaternion.identity;  // deprecated. now saves in euler angles

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            m_Version = k_LatestVersion;
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (m_Version < k_LatestVersion)
            {
                OnUpgradeFromVersion(m_Version); //upgrade derived classes
            }
        }

        void OnUpgradeFromVersion(int oldVersion)
        {
            if (oldVersion < (int)Versions.RotationAsEuler)
                AnimationPlayableAssetUpgrade.ConvertRotationToEuler(this);
        }

        static class AnimationPlayableAssetUpgrade
        {
            public static void ConvertRotationToEuler(AnimationPlayableAsset asset)
            {
#pragma warning disable 618
                asset.m_EulerAngles = asset.m_Rotation.eulerAngles;
#pragma warning restore 618
            }
        }
    }
}
