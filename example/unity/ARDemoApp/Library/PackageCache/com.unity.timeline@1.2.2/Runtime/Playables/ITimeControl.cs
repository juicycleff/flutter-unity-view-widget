namespace UnityEngine.Timeline
{
    /// <summary>
    /// Interface that can be implemented by MonoBehaviours indicating that they receive time-related control calls from a PlayableGraph.
    /// </summary>
    /// <remarks>
    /// Implementing this interface on MonoBehaviours attached to GameObjects under control by control-tracks will cause them to be notified when associated Timeline clips are active.
    /// </remarks>
    public interface ITimeControl
    {
        /// <summary>
        /// Called each frame the Timeline clip is active.
        /// </summary>
        /// <param name="time">The local time of the associated Timeline clip.</param>
        void SetTime(double time);

        /// <summary>
        /// Called when the associated Timeline clip becomes active.
        /// </summary>
        void OnControlTimeStart();

        /// <summary>
        /// Called when the associated Timeline clip becomes deactivated.
        /// </summary>
        void OnControlTimeStop();
    }
}
