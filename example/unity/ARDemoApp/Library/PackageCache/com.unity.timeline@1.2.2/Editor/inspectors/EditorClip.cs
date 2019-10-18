using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    class EditorClip : ScriptableObject
    {
        [SerializeField] TimelineClip m_Clip;

        public TimelineClip clip
        {
            get { return m_Clip; }
            set { m_Clip = value; }
        }

        public int lastHash { get; set; }

        public override int GetHashCode()
        {
            return clip.Hash();
        }
    }
}
