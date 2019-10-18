using System;
using System.Collections.Generic;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Several method extensions to <c>Pose</c> for inverse-transforming additional Unity types.
    /// </summary>
    public static class PoseExtensions
    {
        /// <summary>
        /// Inversely transform the <paramref name="position"/> by <c>Pose</c>.
        /// </summary>
        /// <param name="pose">The <c>Pose</c> to use.</param>
        /// <param name="position">A position to inversely transform</param>
        /// <returns>An position inversely transformed by the <paramref name="pose"/>.</returns>
        public static Vector3 InverseTransformPosition(this Pose pose, Vector3 position)
        {
            return Quaternion.Inverse(pose.rotation) * (position - pose.position);
        }

        /// <summary>
        /// Inversely transform the <paramref name="direction"/> by <c>Pose</c>.
        /// </summary>
        /// <param name="pose">The <c>Pose</c> to use.</param>
        /// <param name="direction">A direction to inversely transform</param>
        /// <returns>An direction inversely transformed by the <paramref name="pose"/>.</returns>
        public static Vector3 InverseTransformDirection(this Pose pose, Vector3 direction)
        {
            return Quaternion.Inverse(pose.rotation) * direction;
        }

        /// <summary>
        /// Inversely transform the <paramref name="positions"/> by <c>Pose</c>. The transform is made in-place.
        /// </summary>
        /// <param name="pose">The <c>Pose</c> to use.</param>
        /// <param name="positions">A <c>List</c> of positions to inversely transform</param>
        public static void InverseTransformPositions(this Pose pose, List<Vector3> positions)
        {
            if (positions == null)
                throw new ArgumentNullException("positions");

            for (int i = 0; i < positions.Count; ++i)
            {
                positions[i] = pose.InverseTransformPosition(positions[i]);
            }
        }

    }
}
