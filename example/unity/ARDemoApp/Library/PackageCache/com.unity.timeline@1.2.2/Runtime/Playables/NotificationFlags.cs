using System;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// Use these flags to specify the notification behaviour.
    /// </summary>
    /// <see cref="UnityEngine.Playables.INotification"/>
    [Flags]
    [Serializable]
    public enum NotificationFlags : short
    {
        /// <summary>
        /// Use this flag to send the notification in Edit Mode.
        /// </summary>
        /// <remarks>
        /// Sent on discontinuous jumps in time.
        /// </remarks>
        TriggerInEditMode = 1 << 0,

        /// <summary>
        /// Use this flag to send the notification if playback starts after the notification time.
        /// </summary>
        Retroactive = 1 << 1,

        /// <summary>
        /// Use this flag to send the notification only once when looping.
        /// </summary>
        TriggerOnce = 1 << 2,
    }
}
