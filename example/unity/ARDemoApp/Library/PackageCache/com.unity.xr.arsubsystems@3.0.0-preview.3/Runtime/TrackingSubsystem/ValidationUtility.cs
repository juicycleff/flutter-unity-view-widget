using System;
using System.Collections.Generic;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// A utility to validate data from certain types of <c>Subsystem</c>s.
    /// </summary>
    /// <typeparam name="T">The <see cref="ITrackable"/> managed by the subsystem.</typeparam>
    /// <seealso cref="XRDepthSubsystem"/>
    /// <seealso cref="XRPlaneSubsystem"/>
    /// <seealso cref="XRReferencePointSubsystem"/>
    public class ValidationUtility<T>
        where T : struct, ITrackable
    {
        /// <summary>
        /// Performs validation checks that ensure a trackable does not exist in multiple lists
        /// simultaneously, e.g., added and removed. Also ensures that a trackable cannot be
        /// removed before being added.
        /// </summary>
        /// <param name="changes">A set of trackable changes (added, updated & removed)</param>
        public void ValidateAndThrow(TrackableChanges<T> changes)
        {
            s_IdSet.Clear();
            foreach (var trackable in changes.added)
            {
                AddToSetAndThrowIfDuplicate(trackable.trackableId, false, k_AddedAction);
                m_Trackables.Add(trackable.trackableId);
            }

            foreach (var trackable in changes.updated)
                AddToSetAndThrowIfDuplicate(trackable.trackableId, true, k_UpdatedAction);

            foreach (var trackableId in changes.removed)
            {
                AddToSetAndThrowIfDuplicate(trackableId, true, k_RemovedAction);
                m_Trackables.Remove(trackableId);
            }
        }

        /// <summary>
        /// Same as <see cref="ValidateAndThrow(TrackableChanges{T})"/> but also disposes the <paramref name="changes"/>.
        /// </summary>
        /// <param name="changes">A set of trackable changes (added, updated & removed)</param>
        public void ValidateAndDisposeIfThrown(TrackableChanges<T> changes)
        {
            try
            {
                ValidateAndThrow(changes);
            }
            catch
            {
                changes.Dispose();
                throw;
            }
        }

        void AddToSetAndThrowIfDuplicate(
            TrackableId trackableId,
            bool shouldBeInDictionary,
            string action)
        {
            if (!s_IdSet.Add(trackableId))
                    throw new InvalidOperationException(
                        string.Format(
                            "Trackable {0} being {1} this frame has at least one other action associated with it.",
                            trackableId, action));

            if (m_Trackables.Contains(trackableId) != shouldBeInDictionary)
                throw new InvalidOperationException(string.Format(
                    "Trackable {0} is being {1} but is {2} in the list of trackables.",
                    trackableId, action, shouldBeInDictionary ? "not" : "already"));
        }

        static HashSet<TrackableId> s_IdSet = new HashSet<TrackableId>();

        static readonly string k_AddedAction = "added";

        static readonly string k_UpdatedAction = "updated";

        static readonly string k_RemovedAction = "removed";

        HashSet<TrackableId> m_Trackables = new HashSet<TrackableId>();
    }
}
