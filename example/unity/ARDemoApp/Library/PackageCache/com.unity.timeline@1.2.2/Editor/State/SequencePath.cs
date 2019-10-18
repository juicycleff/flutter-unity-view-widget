using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    [Serializable]
    class SequencePath
    {
        [SerializeField] int m_SelectionRoot;

        public int selectionRoot
        {
            get { return m_SelectionRoot; }
        }

        [SerializeField] List<SequencePathSubElement> m_SubElements;

        public List<SequencePathSubElement> subElements
        {
            get { return m_SubElements ?? (m_SubElements = new List<SequencePathSubElement>()); }
        }

        public void SetSelectionRoot(int instanceID)
        {
            m_SelectionRoot = instanceID;
            subElements.Clear();
        }

        public void AddSubSequence(ISequenceState state, IExposedPropertyTable resolver)
        {
            subElements.Add(SequencePathSubElement.Create(state, resolver));
        }

        public void Clear()
        {
            m_SelectionRoot = 0;
            subElements.Clear();
        }

        public static bool AreEqual(SequencePath lhs, SequencePath rhs)
        {
            if (ReferenceEquals(lhs, null) && ReferenceEquals(rhs, null)) return true;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;
            if (ReferenceEquals(lhs, rhs)) return true;

            var result = lhs.selectionRoot == rhs.selectionRoot &&
                lhs.subElements.Count == rhs.subElements.Count;

            if (!result)
                return false;

            for (int i = 0, n = lhs.subElements.Count; i < n; ++i)
                result = result && SequencePathSubElement.AreEqual(lhs.subElements[i], rhs.subElements[i]);

            return result;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendFormat("[{0}]", m_SelectionRoot.ToString());

            if (m_SubElements != null && m_SubElements.Count > 0)
            {
                foreach (var element in m_SubElements)
                {
                    sb.Append(" > ");
                    sb.Append(element.ToString());
                }
            }

            return sb.ToString();
        }
    }

    [Serializable]
    class SequencePathSubElement
    {
        public int trackInstanceID;
        public int trackHash;
        public int clipIndex;
        public int clipHash;
        public int subDirectorIndex;

        public static SequencePathSubElement Create(ISequenceState state, IExposedPropertyTable resolver)
        {
            var clip = state.hostClip;
            Debug.Assert(clip != null);
            var track = clip.parentTrack;
            Debug.Assert(track != null);
            var asset = track.timelineAsset;
            Debug.Assert(asset != null);
            var directors = TimelineUtility.GetSubTimelines(clip, resolver as PlayableDirector);

            return new SequencePathSubElement
            {
                trackInstanceID = track.GetInstanceID(),
                trackHash = track.Hash(),
                clipIndex = Array.IndexOf(track.clips, clip),
                clipHash = clip.Hash(),
                subDirectorIndex = directors.IndexOf(state.director)
            };
        }

        public static bool AreEqual(SequencePathSubElement lhs, SequencePathSubElement rhs)
        {
            if (ReferenceEquals(lhs, null) && ReferenceEquals(rhs, null)) return true;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;
            if (ReferenceEquals(lhs, rhs)) return true;

            return lhs.trackInstanceID  == rhs.trackInstanceID &&
                lhs.trackHash        == rhs.trackHash &&
                lhs.clipIndex        == rhs.clipIndex &&
                lhs.clipHash         == rhs.clipHash &&
                lhs.subDirectorIndex == rhs.subDirectorIndex;
        }

        public override string ToString()
        {
            return string.Format(
                "[track[{0}] ({1}) > clip[{2}] ({3})]",
                trackInstanceID.ToString(), trackHash.ToString(),
                clipIndex.ToString(), clipHash.ToString());
        }
    }
}
