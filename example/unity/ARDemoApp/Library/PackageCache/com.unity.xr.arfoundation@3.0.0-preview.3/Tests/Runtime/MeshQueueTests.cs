using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.XR.ARSubsystems;

#if UNITY_2019_3_OR_NEWER
using LegacyMeshId = UnityEngine.XR.MeshId;
#else
using LegacyMeshId = UnityEngine.Experimental.XR.TrackableId;
using MeshInfo = UnityEngine.Experimental.XR.MeshInfo;
using MeshChangeState = UnityEngine.Experimental.XR.MeshChangeState;
#endif

namespace UnityEngine.XR.ARFoundation
{
    [TestFixture]
    public class MeshQueueTestFixture
    {
        [Test]
        public void HighPriorityItemsTakePrecedence()
        {
            var emptyDict = new Dictionary<LegacyMeshId, MeshInfo>();
            var queue = new MeshQueue();
            for (int i = 0; i < 100; ++i)
            {
                queue.EnqueueUnique(new MeshInfo
                {
                    MeshId = GetRandomMeshId(),
                    ChangeState = MeshChangeState.Added,
                    PriorityHint = Random.Range(0, 100)
                });
            }

            int? lastPriorityHint = null;
            while (queue.count > 0)
            {
                bool dequeued = queue.TryDequeue(emptyDict, out MeshInfo meshInfo);
                Assert.That(dequeued, "Could not dequeue even with an empty dictionary");

                if (lastPriorityHint.HasValue)
                {
                    Assert.That(meshInfo.PriorityHint <= lastPriorityHint.Value);
                }

                lastPriorityHint = meshInfo.PriorityHint;
            }
        }

        [Test]
        public void AddedMeshesTakePrecedence()
        {
            var emptyDict = new Dictionary<LegacyMeshId, MeshInfo>();
            var queue = new MeshQueue();
            for (int i = 0; i < 100; ++i)
            {
                queue.EnqueueUnique(new MeshInfo
                {
                    MeshId = GetRandomMeshId(),
                    ChangeState = (Random.Range(0f, 1f) < .5f || i == 0) ? MeshChangeState.Added : MeshChangeState.Updated,
                    PriorityHint = Random.Range(0, 100)
                });
            }

            MeshChangeState? lastChangeState = null;
            while (queue.count > 0)
            {
                bool dequeued = queue.TryDequeue(emptyDict, out MeshInfo meshInfo);
                Assert.That(dequeued, "Could not dequeue even with an empty dictionary");

                if (lastChangeState.HasValue)
                {
                    Assert.That((meshInfo.ChangeState == lastChangeState.Value) || (meshInfo.ChangeState == MeshChangeState.Updated && lastChangeState.Value == MeshChangeState.Added),
                        "All added meshes did not come first");
                }

                lastChangeState = meshInfo.ChangeState;
            }
        }

        [Test]
        public void GeneratingMeshesAreNotDequeued()
        {
            var generating = new Dictionary<LegacyMeshId, MeshInfo>();
            var queue = new MeshQueue();
            for (int i = 0; i < 100; ++i)
            {
                var meshId = GetRandomMeshId();
                var meshInfo = new MeshInfo
                {
                    MeshId = meshId,
                    ChangeState = MeshChangeState.Added,
                    PriorityHint = Random.Range(0, 100)
                };

                queue.EnqueueUnique(meshInfo);

                if (Random.Range(0f, 1f) < .5f)
                {
                    generating[meshId] = meshInfo;
                }
            }

            while (generating.Count < queue.count)
            {
                bool result = queue.TryDequeue(generating, out MeshInfo meshInfo);
                Assert.That(result, "Could not dequeue a mesh info even though there are more items to dequeue.");
                Assert.That(!generating.ContainsKey(meshInfo.MeshId), "Should not dequeue a mesh info while it is generating.");
            }
        }

        [Test]
        public void QueueIsUnique()
        {
            var generating = new Dictionary<LegacyMeshId, MeshInfo>();
            var queue = new MeshQueue();
            var uniqueMeshIds = new List<LegacyMeshId>();

            for (int i = 0; i < 100; ++i)
            {
                LegacyMeshId meshId;
                if (i == 0 || Random.Range(0f, 1f) < .5f)
                {
                    meshId = MakeMeshId((ulong)i, (ulong)i);
                    uniqueMeshIds.Add(meshId);
                }
                else
                {
                    meshId = uniqueMeshIds[Random.Range(0, uniqueMeshIds.Count - 1)];
                }

                var meshInfo = new MeshInfo
                {
                    MeshId = meshId,
                    ChangeState = MeshChangeState.Added,
                    PriorityHint = Random.Range(0, 100)
                };

                queue.EnqueueUnique(meshInfo);
                Assert.That(uniqueMeshIds.Count == queue.count);
            }
        }

        LegacyMeshId MakeMeshId(ulong a, ulong b)
        {
            return ARMeshManager.GetLegacyMeshId(new TrackableId(a, b));
        }

        LegacyMeshId GetRandomMeshId()
        {
            return MakeMeshId(
                (ulong)Random.Range(0, int.MaxValue),
                (ulong)Random.Range(0, int.MaxValue));
        }
    }
}
