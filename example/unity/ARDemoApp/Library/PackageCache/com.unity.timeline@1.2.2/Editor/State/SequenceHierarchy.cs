using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    class SequenceHierarchy : ScriptableObject
    {
        readonly List<ISequenceState> m_Sequences = new List<ISequenceState>();

        WindowState m_WindowState;

        [SerializeField]
        SequencePath m_SerializedPath;

        public ISequenceState masterSequence
        {
            get { return m_Sequences.FirstOrDefault(); }
        }

        public ISequenceState editSequence
        {
            get { return m_Sequences.LastOrDefault(); }
        }

        public int count
        {
            get { return m_Sequences.Count; }
        }

        public IEnumerable<ISequenceState> allSequences
        {
            get { return m_Sequences; }
        }

        public static SequenceHierarchy CreateInstance()
        {
            var hierarchy = ScriptableObject.CreateInstance<SequenceHierarchy>();
            hierarchy.hideFlags = HideFlags.HideAndDontSave;
            return hierarchy;
        }

        public void Init(WindowState owner)
        {
            m_WindowState = owner;
        }

        // This is called when performing Undo operations.
        // It needs to be called here since some operations are not
        // allowed (EditorUtility.InstanceIDToObject, for example)
        // during the ISerializationCallbackReceiver methods.
        void OnValidate()
        {
            if (m_SerializedPath == null || m_WindowState == null || m_WindowState.GetWindow() == null)
                return;

            m_WindowState.SetCurrentSequencePath(m_SerializedPath, true);
        }

        public void Add(TimelineAsset asset, PlayableDirector director, TimelineClip hostClip)
        {
            if (hostClip == null)
                AddToCurrentUndoGroup(this); // Merge with selection undo
            else
                TimelineUndo.PushUndo(this, "Edit Sub-Timeline");

            Add_Internal(asset, director, hostClip);

            UpdateSerializedPath();
        }

        public void Remove()
        {
            if (m_Sequences.Count == 0) return;

            TimelineUndo.PushUndo(this, "Go to Sub-Timeline");

            Remove_Internal();

            UpdateSerializedPath();
        }

        public ISequenceState GetStateAtIndex(int index)
        {
            return m_Sequences[index];
        }

        public void RemoveUntilCount(int expectedCount)
        {
            if (expectedCount < 0 || m_Sequences.Count <= expectedCount) return;

            TimelineUndo.PushUndo(this, "Go to Sub-Timeline");

            RemoveUntilCount_Internal(expectedCount);

            UpdateSerializedPath();
        }

        public void Clear()
        {
            if (m_Sequences.Count == 0) return;

            AddToCurrentUndoGroup(this);
            Clear_Internal();
            UpdateSerializedPath();
        }

        public SequencePath ToSequencePath()
        {
            var path = new SequencePath();

            if (m_Sequences.Count == 0)
                return path;

            var rootSequence = m_Sequences[0];
            var root = 0;
            if (rootSequence.director != null && rootSequence.director.gameObject != null)
                root = rootSequence.director.gameObject.GetInstanceID();
            else if (rootSequence.asset != null)
                root = rootSequence.asset.GetInstanceID();

            path.SetSelectionRoot(root);

            var resolver = rootSequence.director;

            if (m_Sequences.Count > 1)
            {
                for (int i = 1, n = m_Sequences.Count; i < n; ++i)
                {
                    path.AddSubSequence(m_Sequences[i], resolver);
                    resolver = m_Sequences[i].director;
                }
            }

            return path;
        }

        public bool NeedsUpdate(SequencePath path, bool forceRebuild)
        {
            return forceRebuild || !SequencePath.AreEqual(m_SerializedPath, path);
        }

        public void FromSequencePath(SequencePath path, bool forceRebuild)
        {
            if (!NeedsUpdate(path, forceRebuild))
                return;

            Clear_Internal();

            var rootObject = EditorUtility.InstanceIDToObject(path.selectionRoot);
            if (rootObject == null)
            {
                UpdateSerializedPath();
                return;
            }

            var candidateAsset = rootObject as TimelineAsset;
            if (candidateAsset != null)
            {
                Add_Internal(candidateAsset, null, null);
                UpdateSerializedPath();
                return;
            }

            var candidateGameObject = rootObject as GameObject;
            if (candidateGameObject == null)
            {
                UpdateSerializedPath();
                return;
            }

            var director = TimelineUtility.GetDirectorComponentForGameObject(candidateGameObject);
            var asset = TimelineUtility.GetTimelineAssetForDirectorComponent(director);
            Add_Internal(asset, director, null);

            if (!path.subElements.Any())
            {
                UpdateSerializedPath();
                return;
            }

            List<SequenceBuildingBlock> buildingBlocks;
            if (ValidateSubElements(path.subElements, director, out buildingBlocks))
            {
                foreach (var buildingBlock in buildingBlocks)
                    Add_Internal(buildingBlock.asset, buildingBlock.director, buildingBlock.hostClip);
            }

            UpdateSerializedPath();
        }

        void Add_Internal(TimelineAsset asset, PlayableDirector director, TimelineClip hostClip)
        {
            if (hostClip == null)
                Clear_Internal();

            var parent = m_Sequences.Count > 0 ? editSequence : null;
            m_Sequences.Add(new SequenceState(m_WindowState, asset, director, hostClip, (SequenceState)parent));
        }

        void Remove_Internal()
        {
            m_Sequences.Last().Dispose();
            m_Sequences.RemoveAt(m_Sequences.Count - 1);
        }

        void RemoveUntilCount_Internal(int expectedCount)
        {
            while (m_Sequences.Count > expectedCount)
            {
                Remove_Internal();
            }
        }

        void Clear_Internal()
        {
            RemoveUntilCount_Internal(0);
        }

        void UpdateSerializedPath()
        {
            m_SerializedPath = ToSequencePath();
        }

        static bool ValidateSubElements(List<SequencePathSubElement> subElements, PlayableDirector director, out List<SequenceBuildingBlock> buildingBlocks)
        {
            buildingBlocks = new List<SequenceBuildingBlock>(subElements.Count);
            var currentDirector = director;

            foreach (var element in subElements)
            {
                var timeline = currentDirector.playableAsset as TimelineAsset;
                if (timeline == null)
                    return false;
                if (timeline.trackObjects == null)
                    return false;

                var track = timeline.GetOutputTracks().FirstOrDefault(t => t.GetInstanceID() == element.trackInstanceID);
                if (track == null)
                    return false;
                if (track.Hash() != element.trackHash)
                    return false;
                if (track.clips == null)
                    return false;
                if (track.clips.Length <= element.clipIndex)
                    return false;

                var clip = track.clips[element.clipIndex];
                if (clip == null)
                    return false;
                if (clip.Hash() != element.clipHash)
                    return false;

                var candidateDirectors = TimelineUtility.GetSubTimelines(clip, director);

                if (element.subDirectorIndex < 0 || element.subDirectorIndex >= candidateDirectors.Count)
                    return false;

                var candidateDirector = candidateDirectors[element.subDirectorIndex];

                if (candidateDirector == null || !(candidateDirector.playableAsset is TimelineAsset))
                    return false;

                currentDirector = candidateDirector;

                buildingBlocks.Add(
                    new SequenceBuildingBlock
                    {
                        asset = currentDirector.playableAsset as TimelineAsset,
                        director = currentDirector,
                        hostClip = clip
                    });
            }

            return true;
        }

        struct SequenceBuildingBlock
        {
            public TimelineAsset asset;
            public PlayableDirector director;
            public TimelineClip hostClip;
        }

        static void AddToCurrentUndoGroup(Object target)
        {
            if (target == null) return;

            var group = Undo.GetCurrentGroup();
            var groupName = Undo.GetCurrentGroupName();
            EditorUtility.SetDirty(target);
            Undo.RegisterCompleteObjectUndo(target, groupName);
            Undo.CollapseUndoOperations(group);
        }
    }
}
