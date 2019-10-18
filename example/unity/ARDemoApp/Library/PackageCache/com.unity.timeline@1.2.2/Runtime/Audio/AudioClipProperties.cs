using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    [Serializable]
    [NotKeyable]
    class AudioClipProperties : PlayableBehaviour
    {
        [Range(0.0f, 1.0f)]
        public float volume = 1.0f;
    }
}
