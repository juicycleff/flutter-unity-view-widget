using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;

#if UNITY_2019_3_OR_NEWER
using LegacyMeshId = UnityEngine.XR.MeshId;
#else
using LegacyMeshId = UnityEngine.Experimental.XR.TrackableId;
using MeshInfo = UnityEngine.Experimental.XR.MeshInfo;
#endif

namespace UnityEngine.XR.ARFoundation
{
    internal class MeshInfoComparer : IComparer<MeshInfo>
    {
        /// <summary>
        /// Mesh infos are stored last first so that the dequeue operation is fast
        /// - < 0 <paramref name="infoA"/> will appear before infoB in the list
        /// - 0 <paramref name="infoA"/> has the same priority as <paramref name="infoB"/>
        /// - > 0 <paramref name="infoB"/> will appear before infoA in the list
        /// </summary>
        public int Compare(MeshInfo infoA, MeshInfo infoB)
        {
            // Prioritize 'added' over 'updated'
            if (infoA.ChangeState < infoB.ChangeState)
            {
                return 1;
            }
            else if (infoB.ChangeState < infoA.ChangeState)
            {
                return -1;
            }
            else
            {
                // If 'A' has a high priority, then we return a positive number
                // which puts A last in the list. This means we can dequeue
                // the next mesh to generate by taking the last element.
                return (infoA.PriorityHint - infoB.PriorityHint);
            }
        }
    }

    internal class MeshQueue
    {
        public void EnqueueUnique(MeshInfo meshInfo)
        {
            if (m_MeshSet.Contains(meshInfo.MeshId))
            {
                UpdateExisting(meshInfo);
            }
            else
            {
                InsertNew(meshInfo);
            }
        }

        public int count
        {
            get { return m_Queue.Count; }
        }

        public bool TryDequeue(IReadOnlyDictionary<LegacyMeshId, MeshInfo> generating, out MeshInfo meshInfo)
        {
            for (int i = m_Queue.Count - 1; i >= 0; --i)
            {
                meshInfo = m_Queue[i];
                if (!generating.ContainsKey(meshInfo.MeshId))
                {
                    m_Queue.RemoveAt(i);
                    m_MeshSet.Remove(meshInfo.MeshId);
                    return true;
                }
            }

            meshInfo = default;
            return false;
        }

        public bool Remove(LegacyMeshId meshId)
        {
            // It is relatively rare to remove an existing mesh
            // (this means it was removed while awaiting generation).
            // So it most cases we should be able to early out.
            if (!m_MeshSet.Remove(meshId))
                return false;

            // Otherwise, perform a linear search and remove it.
            for (int i = 0; i < m_Queue.Count; ++i)
            {
                if (m_Queue[i].MeshId.Equals(meshId))
                {
                    m_Queue.RemoveAt(i);
                    break;
                }
            }

            return true;
        }

        void InsertNew(MeshInfo meshInfo)
        {
            int index = m_Queue.BinarySearch(meshInfo, s_MeshInfoComparer);
            if (index < 0)
                index = ~index;

            m_Queue.Insert(index, meshInfo);
            m_MeshSet.Add(meshInfo.MeshId);
        }

        void UpdateExisting(MeshInfo meshInfo)
        {
            for (int i = 0; i < m_Queue.Count; ++i)
            {
                var existing = m_Queue[i];
                if (existing.MeshId.Equals(meshInfo.MeshId))
                {
                    // Only need to do anything if they are not equal
                    if (existing.PriorityHint != meshInfo.PriorityHint)
                    {
                        existing.PriorityHint = meshInfo.PriorityHint;
                        m_Queue[i] = existing;
                        m_Queue.Sort(s_MeshInfoComparer);
                    }
                    break;
                }
            }
        }

        public void Clear()
        {
            m_Queue.Clear();
            m_MeshSet.Clear();
        }

        // This list is kept sorted according to MeshInfoComparer
        List<MeshInfo> m_Queue = new List<MeshInfo>();

        HashSet<LegacyMeshId> m_MeshSet = new HashSet<LegacyMeshId>();

        MeshInfoComparer s_MeshInfoComparer = new MeshInfoComparer();
    }
}
