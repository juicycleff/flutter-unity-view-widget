namespace UnityEngine.Timeline
{
    /// <summary>
    /// Implement this interface to change the behaviour of an INotification.
    /// </summary>
    /// This interface must be implemented along with <see cref="UnityEngine.Playables.INotification"/> to modify the default behaviour of a notification.
    /// <seealso cref="UnityEngine.Timeline.NotificationFlags"/>
    public interface INotificationOptionProvider
    {
        /// <summary>
        /// The flags that change the triggering behaviour.
        /// </summary>
        NotificationFlags flags { get; }
    }
}
