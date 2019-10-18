using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline.Utilities
{
    class KeyTraverser
    {
        float[] m_KeyCache;
        int m_DirtyStamp = -1;
        int m_LastHash = -1;
        readonly TimelineAsset m_Asset;
        readonly float m_Epsilon;
        int m_LastIndex = -1;

        public int lastIndex
        {
            get { return m_LastIndex; }
        }

        public static IEnumerable<float> GetClipKeyTimes(TimelineClip clip)
        {
            if (clip == null || clip.animationClip == null || clip.animationClip.empty)
                return new float[0];

            return AnimationClipCurveCache.Instance.GetCurveInfo(clip.animationClip).keyTimes.
                Select(k => (float)clip.FromLocalTimeUnbound(k)).    // convert to sequence time
                Where(k => k >= clip.start && k <= clip.end);    // remove non visible keys
        }

        public static IEnumerable<float> GetTrackKeyTimes(AnimationTrack track)
        {
            if (track != null)
            {
                if (track.inClipMode)
                    return track.clips.Where(c => c.recordable).
                        SelectMany(x => GetClipKeyTimes(x));
                if (track.infiniteClip != null && !track.infiniteClip.empty)
                    return AnimationClipCurveCache.Instance.GetCurveInfo(track.infiniteClip).keyTimes;
            }
            return new float[0];
        }

        static int CalcAnimClipHash(TrackAsset asset)
        {
            int hash = 0;
            if (asset != null)
            {
                AnimationTrack animTrack = asset as AnimationTrack;
                if (animTrack != null)
                {
                    for (var i = 0; i != animTrack.clips.Length; ++i)
                    {
                        hash ^= (animTrack.clips[i]).Hash();
                    }
                }
                foreach (var subTrack in asset.GetChildTracks())
                {
                    if (subTrack != null)
                        hash ^= CalcAnimClipHash(subTrack);
                }
            }
            return hash;
        }

        internal static int CalcAnimClipHash(TimelineAsset asset)
        {
            int hash = 0;
            foreach (var t in asset.GetRootTracks())
            {
                if (t != null)
                    hash ^= CalcAnimClipHash(t);
            }
            return hash;
        }

        void RebuildKeyCache()
        {
            m_KeyCache = m_Asset.flattenedTracks.Where(x => (x as AnimationTrack) != null)
                .Cast<AnimationTrack>()
                .SelectMany(t => GetTrackKeyTimes(t)).
                OrderBy(x => x).ToArray();

            if (m_KeyCache.Length > 0)
            {
                float[] unique = new float[m_KeyCache.Length];
                unique[0] = m_KeyCache[0];
                int index = 0;
                for (int i = 1; i < m_KeyCache.Length; i++)
                {
                    if (m_KeyCache[i] - unique[index] > m_Epsilon)
                    {
                        index++;
                        unique[index] = m_KeyCache[i];
                    }
                }
                m_KeyCache = unique;
                Array.Resize(ref m_KeyCache, index + 1);
            }
        }

        public KeyTraverser(TimelineAsset timeline, float epsilon)
        {
            m_Asset = timeline;
            m_Epsilon = epsilon;
        }

        void CheckCache(int dirtyStamp)
        {
            int hash = CalcAnimClipHash(m_Asset);
            if (dirtyStamp != m_DirtyStamp || hash != m_LastHash)
            {
                RebuildKeyCache();
                m_DirtyStamp = dirtyStamp;
                m_LastHash = hash;
            }
        }

        public float GetNextKey(float key, int dirtyStamp)
        {
            CheckCache(dirtyStamp);
            if (m_KeyCache.Length > 0)
            {
                if (key < m_KeyCache.Last() - m_Epsilon)
                {
                    if (key > m_KeyCache[0] - m_Epsilon)
                    {
                        float t = key + m_Epsilon;
                        // binary search
                        int max = m_KeyCache.Length - 1;
                        int min = 0;
                        while (max - min > 1)
                        {
                            int imid = (min + max) / 2;
                            if (t > m_KeyCache[imid])
                                min = imid;
                            else
                                max = imid;
                        }
                        m_LastIndex = max;
                        return m_KeyCache[max];
                    }

                    m_LastIndex = 0;
                    return m_KeyCache[0];
                }
                if (key < m_KeyCache.Last() + m_Epsilon)
                {
                    m_LastIndex = m_KeyCache.Length - 1;
                    return Mathf.Max(key, m_KeyCache.Last());
                }
            }
            m_LastIndex = -1;
            return key;
        }

        public float GetPrevKey(float key, int dirtyStamp)
        {
            CheckCache(dirtyStamp);
            if (m_KeyCache.Length > 0)
            {
                if (key > m_KeyCache[0] + m_Epsilon)
                {
                    if (key < m_KeyCache.Last() + m_Epsilon)
                    {
                        float t = key - m_Epsilon;

                        // binary search
                        int max = m_KeyCache.Length - 1;
                        int min = 0;
                        while (max - min > 1)
                        {
                            int imid = (min + max) / 2;
                            if (t < m_KeyCache[imid])
                                max = imid;
                            else
                                min = imid;
                        }
                        m_LastIndex = min;
                        return m_KeyCache[min];
                    }
                    m_LastIndex = m_KeyCache.Length - 1;
                    return m_KeyCache.Last();
                }
                if (key >= m_KeyCache[0] - m_Epsilon)
                {
                    m_LastIndex = 0;
                    return Mathf.Min(key, m_KeyCache[0]);
                }
            }
            m_LastIndex = -1;
            return key;
        }

        public int GetKeyCount(int dirtyStamp)
        {
            CheckCache(dirtyStamp);
            return m_KeyCache.Length;
        }
    }
}
