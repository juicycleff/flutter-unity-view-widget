using System;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// Use Marker as a base class when creating a custom marker.
    /// </summary>
    /// <remarks>
    /// A marker is a point in time.
    /// </remarks>
    public abstract class Marker : ScriptableObject, IMarker
    {
        [SerializeField, TimeField, Tooltip("Time for the marker")] double m_Time;

        /// <inheritdoc/>
        public TrackAsset parent { get; private set; }

        /// <inheritdoc/>
        /// <remarks>
        /// The marker time cannot be negative.
        /// </remarks>
        public double time
        {
            get { return m_Time; }
            set { m_Time = Math.Max(value, 0); }
        }

        void IMarker.Initialize(TrackAsset parentTrack)
        {
            // We only really want to update the parent when the object is first deserialized
            // If not a cloned track would "steal" the source's markers
            if (parent == null)
            {
                parent = parentTrack;
                try
                {
                    OnInitialize(parentTrack);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message, this);
                }
            }
        }

        /// <summary>
        /// Override this method to receive a callback when the marker is initialized.
        /// </summary>
        public virtual void OnInitialize(TrackAsset aPent)
        {
        }
    }
}
