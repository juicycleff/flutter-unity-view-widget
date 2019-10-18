using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// Listens for emitted signals and reacts depending on its defined reactions.
    /// </summary>
    /// A SignalReceiver contains a list of reactions. Each reaction is bound to a SignalAsset.
    /// When a SignalEmitter emits a signal, the SignalReceiver invokes the corresponding reaction.
    /// <seealso cref="UnityEngine.Timeline.SignalEmitter"/>
    /// <seealso cref="UnityEngine.Timeline.SignalAsset"/>
    public class SignalReceiver : MonoBehaviour, INotificationReceiver
    {
        [SerializeField]
        EventKeyValue m_Events = new EventKeyValue();

        /// <summary>
        /// Called when a notification is sent.
        /// </summary>
        public void OnNotify(Playable origin, INotification notification, object context)
        {
            var signal = notification as SignalEmitter;
            if (signal != null && signal.asset != null)
            {
                UnityEvent evt;
                if (m_Events.TryGetValue(signal.asset, out evt) && evt != null)
                {
                    evt.Invoke();
                }
            }
        }

        /// <summary>
        /// Defines a new reaction for a SignalAsset.
        /// </summary>
        /// <param name="asset">The SignalAsset for which the reaction is being defined.</param>
        /// <param name="reaction">The UnityEvent that describes the reaction.</param>
        /// <exception cref="ArgumentNullException">Thrown when the asset is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the SignalAsset is already registered with this receiver.</exception>
        public void AddReaction(SignalAsset asset, UnityEvent reaction)
        {
            if (asset == null)
                throw new ArgumentNullException("asset");

            if (m_Events.signals.Contains(asset))
                throw new ArgumentException("SignalAsset already used.");
            m_Events.Append(asset, reaction);
        }

        /// <summary>
        /// Appends a null SignalAsset with a reaction specified by the UnityEvent.
        /// </summary>
        /// <param name="reaction">The new reaction to be appended.</param>
        /// <returns>The index of the appended reaction.</returns>
        /// <remarks>Multiple null assets are valid.</remarks>
        public int AddEmptyReaction(UnityEvent reaction)
        {
            m_Events.Append(null, reaction);
            return m_Events.events.Count - 1;
        }

        /// <summary>
        /// Removes the first occurrence of a SignalAsset.
        /// </summary>
        /// <param name="asset">The SignalAsset to be removed.</param>
        public void Remove(SignalAsset asset)
        {
            if (!m_Events.signals.Contains(asset))
            {
                throw new ArgumentException("The SignalAsset is not registered with this receiver.");
            }

            m_Events.Remove(asset);
        }

        /// <summary>
        /// Gets a list of all registered SignalAssets.
        /// </summary>
        /// <returns>Returns a list of SignalAssets.</returns>
        public IEnumerable<SignalAsset> GetRegisteredSignals()
        {
            return m_Events.signals;
        }

        /// <summary>
        /// Gets the first UnityEvent associated with a SignalAsset.
        /// </summary>
        /// <param name="key">A SignalAsset defining the signal.</param>
        /// <returns>Returns the reaction associated with a SignalAsset. Returns null if the signal asset does not exist.</returns>
        public UnityEvent GetReaction(SignalAsset key)
        {
            UnityEvent ret;
            if (m_Events.TryGetValue(key, out ret))
            {
                return ret;
            }

            return null;
        }

        /// <summary>
        /// Returns the count of registered SignalAssets.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return m_Events.signals.Count;
        }

        /// <summary>
        /// Replaces the SignalAsset associated with a reaction at a specific index.
        /// </summary>
        /// <param name="idx">The index of the reaction.</param>
        /// <param name="newKey">The replacement SignalAsset.</param>
        /// <exception cref="ArgumentException">Thrown when the replacement SignalAsset is already registered to this SignalReceiver.</exception>
        /// <remarks>The new SignalAsset can be null.</remarks>
        public void ChangeSignalAtIndex(int idx, SignalAsset newKey)
        {
            if (idx < 0 || idx > m_Events.signals.Count - 1)
                throw new IndexOutOfRangeException();

            if (m_Events.signals[idx] == newKey)
                return;
            var alreadyUsed = m_Events.signals.Contains(newKey);
            if (newKey == null || m_Events.signals[idx]  == null || !alreadyUsed)
                m_Events.signals[idx] = newKey;

            if (alreadyUsed)
                throw new ArgumentException("SignalAsset already used.");
        }

        /// <summary>
        /// Removes the SignalAsset and reaction at a specific index.
        /// </summary>
        /// <param name="idx">The index of the SignalAsset to be removed.</param>
        public void RemoveAtIndex(int idx)
        {
            if (idx < 0 || idx > m_Events.signals.Count - 1)
                throw new IndexOutOfRangeException();
            m_Events.Remove(idx);
        }

        /// <summary>
        /// Replaces the reaction at a specific index with a new UnityEvent.
        /// </summary>
        /// <param name="idx">The index of the reaction to be replaced.</param>
        /// <param name="reaction">The replacement reaction.</param>
        /// <exception cref="ArgumentNullException">Thrown when the replacement reaction is null.</exception>
        public void ChangeReactionAtIndex(int idx, UnityEvent reaction)
        {
            if (idx < 0 || idx > m_Events.events.Count - 1)
                throw new IndexOutOfRangeException();

            m_Events.events[idx] = reaction;
        }

        /// <summary>
        /// Gets the reaction at a specific index.
        /// </summary>
        /// <param name="idx">The index of the reaction.</param>
        /// <returns>Returns a reaction.</returns>
        public UnityEvent GetReactionAtIndex(int idx)
        {
            if (idx < 0 || idx > m_Events.events.Count - 1)
                throw new IndexOutOfRangeException();
            return m_Events.events[idx];
        }

        /// <summary>
        /// Gets the SignalAsset at a specific index
        /// </summary>
        /// <param name="idx">The index of the SignalAsset.</param>
        /// <returns>Returns a SignalAsset.</returns>
        public SignalAsset GetSignalAssetAtIndex(int idx)
        {
            if (idx < 0 || idx > m_Events.signals.Count - 1)
                throw new IndexOutOfRangeException();
            return m_Events.signals[idx];
        }

        // Required by Unity for the MonoBehaviour to have an enabled state
        private void OnEnable()
        {
        }

        [Serializable]
        class EventKeyValue
        {
            [SerializeField]
            List<SignalAsset> m_Signals = new List<SignalAsset>();

            [SerializeField, CustomSignalEventDrawer]
            List<UnityEvent> m_Events = new List<UnityEvent>();

            public bool TryGetValue(SignalAsset key, out UnityEvent value)
            {
                var index = m_Signals.IndexOf(key);
                if (index != -1)
                {
                    value = m_Events[index];
                    return true;
                }

                value = null;
                return false;
            }

            public void Append(SignalAsset key, UnityEvent value)
            {
                m_Signals.Add(key);
                m_Events.Add(value);
            }

            public void Remove(int idx)
            {
                if (idx != -1)
                {
                    m_Signals.RemoveAt(idx);
                    m_Events.RemoveAt(idx);
                }
            }

            public void Remove(SignalAsset key)
            {
                var idx = m_Signals.IndexOf(key);
                if (idx != -1)
                {
                    m_Signals.RemoveAt(idx);
                    m_Events.RemoveAt(idx);
                }
            }

            public List<SignalAsset> signals
            {
                get { return m_Signals; }
            }

            public List<UnityEvent> events
            {
                get { return m_Events; }
            }
        }
    }
}
