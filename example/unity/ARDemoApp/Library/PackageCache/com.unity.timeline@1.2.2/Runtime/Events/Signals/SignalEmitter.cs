using System;
using UnityEngine;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    /// <inheritdoc cref="UnityEngine.Timeline.IMarker" />
    /// <summary>
    /// Marker that emits a signal to a SignalReceiver.
    /// </summary>
    /// A SignalEmitter emits a notification through the playable system. A SignalEmitter is used with a SignalReceiver and a SignalAsset.
    /// <seealso cref="UnityEngine.Timeline.SignalAsset"/>
    /// <seealso cref="UnityEngine.Timeline.SignalReceiver"/>
    [Serializable]
    [CustomStyle("SignalEmitter")]
    public class SignalEmitter : Marker, INotification, INotificationOptionProvider
    {
        [SerializeField] bool m_Retroactive;
        [SerializeField] bool m_EmitOnce;
        [SerializeField] SignalAsset m_Asset;

        /// <summary>
        /// Use retroactive to emit the signal if playback starts after the SignalEmitter time.
        /// </summary>
        public bool retroactive
        {
            get { return m_Retroactive; }
            set { m_Retroactive = value; }
        }

        /// <summary>
        /// Use emitOnce to emit this signal once during loops.
        /// </summary>
        public bool emitOnce
        {
            get { return m_EmitOnce; }
            set { m_EmitOnce = value; }
        }

        /// <summary>
        /// Asset representing the signal being emitted.
        /// </summary>
        public SignalAsset asset
        {
            get { return m_Asset; }
            set { m_Asset = value; }
        }

        PropertyName INotification.id
        {
            get
            {
                if (m_Asset != null)
                {
                    return new PropertyName(m_Asset.name);
                }
                return new PropertyName(string.Empty);
            }
        }

        NotificationFlags INotificationOptionProvider.flags
        {
            get
            {
                return (retroactive ? NotificationFlags.Retroactive : default(NotificationFlags)) |
                    (emitOnce ? NotificationFlags.TriggerOnce : default(NotificationFlags)) |
                    NotificationFlags.TriggerInEditMode;
            }
        }
    }
}
