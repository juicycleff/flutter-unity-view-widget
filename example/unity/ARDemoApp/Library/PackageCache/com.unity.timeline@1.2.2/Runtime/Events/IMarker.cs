namespace UnityEngine.Timeline
{
    /// <summary>
    /// Interface implemented by markers.
    /// </summary>
    /// <remarks>
    /// A marker is a point in time.
    /// </remarks>
    /// <seealso cref="UnityEngine.Timeline.Marker"/>
    public interface IMarker
    {
        /// <summary>
        /// The time set for the marker, in seconds.
        /// </summary>
        double time { get; set; }

        /// <summary>
        /// The track that contains the marker.
        /// </summary>
        TrackAsset parent { get; }

        /// <summary>
        /// This method is called when the marker is initialized.
        /// </summary>
        /// <param name="parent">The track that contains the marker.</param>
        /// <remarks>
        /// This method is called after each deserialization of the Timeline Asset.
        /// </remarks>
        void Initialize(TrackAsset parent);
    }
}
