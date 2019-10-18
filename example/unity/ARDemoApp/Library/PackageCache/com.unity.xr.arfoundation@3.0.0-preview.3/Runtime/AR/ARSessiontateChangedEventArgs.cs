using System;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Container for SystemState event arguments. Used by the <see cref="ARSubsystemManager"/>.
    /// </summary>
    public struct ARSessionStateChangedEventArgs : IEquatable<ARSessionStateChangedEventArgs>
    {
        /// <summary>
        /// The new session state.
        /// </summary>
        public ARSessionState state { get; private set; }

        /// <summary>
        /// Constructor for these event arguments.
        /// </summary>
        /// <param name="state">The new session state.</param>
        public ARSessionStateChangedEventArgs(ARSessionState state)
        {
            this.state = state;
        }

        public override int GetHashCode()
        {
            return ((int)state).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ARSessionStateChangedEventArgs))
                return false;

            return Equals((ARSessionStateChangedEventArgs)obj);
        }

        public override string ToString()
        {
            return state.ToString();
        }

        public bool Equals(ARSessionStateChangedEventArgs other)
        {
            return state == other.state;
        }

        public static bool operator ==(ARSessionStateChangedEventArgs lhs, ARSessionStateChangedEventArgs rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ARSessionStateChangedEventArgs lhs, ARSessionStateChangedEventArgs rhs)
        {
            return !lhs.Equals(rhs);
        }
    }
}
