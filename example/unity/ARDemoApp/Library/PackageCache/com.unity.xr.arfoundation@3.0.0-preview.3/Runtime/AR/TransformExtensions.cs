using System;
using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Several method extensions to <c>Transform</c> for transforming and inverse-transforming additional Unity types.
    /// </summary>
    public static class TransformExtensions
    {
        /// <summary>
        /// Transforms a <c>Ray</c>
        /// </summary>
        /// <param name="transform">The <c>Transform</c> component</param>
        /// <param name="ray">The <c>Ray</c> to transform</param>
        /// <returns>A new <c>Ray</c> representing the transformed <paramref name="ray"/></returns>
        public static Ray TransformRay(this Transform transform, Ray ray)
        {
            if (transform == null)
                throw new ArgumentNullException("transform");

            return new Ray(
                transform.TransformPoint(ray.origin),
                transform.TransformDirection(ray.direction));
        }

        /// <summary>
        /// Inverse transforms a <c>Ray</c>
        /// </summary>
        /// <param name="transform">The <c>Transform</c> component</param>
        /// <param name="ray">The <c>Ray</c> to inversely transform</param>
        /// <returns>A new <c>Ray</c> representing the inversely transformed <paramref name="ray"/></returns>
        public static Ray InverseTransformRay(this Transform transform, Ray ray)
        {
            if (transform == null)
                throw new ArgumentNullException("transform");

            return new Ray(
                transform.InverseTransformPoint(ray.origin),
                transform.InverseTransformDirection(ray.direction));
        }

        /// <summary>
        /// Transforms a <c>Pose</c>
        /// </summary>
        /// <param name="transform">The <c>Transform</c> component</param>
        /// <param name="pose">The <c>Pose</c> to transform</param>
        /// <returns>A new <c>Pose</c> representing the transformed <paramref name="pose"/></returns>
        public static Pose TransformPose(this Transform transform, Pose pose)
        {
            return pose.GetTransformedBy(transform);
        }

        /// <summary>
        /// Inverse transforms a <c>Pose</c>
        /// </summary>
        /// <param name="transform">The <c>Transform</c> component</param>
        /// <param name="pose">The <c>Pose</c> to inversely transform</param>
        /// <returns>A new <c>Pose</c> representing the inversely transformed <paramref name="pose"/></returns>
        public static Pose InverseTransformPose(this Transform transform, Pose pose)
        {
            if (transform == null)
                throw new ArgumentNullException("transform");

            return new Pose
            {
                position = transform.InverseTransformPoint(pose.position),
                rotation = Quaternion.Inverse(transform.rotation) * pose.rotation
            };
        }

        /// <summary>
        /// Transforms a <c>List</c> of positions.
        /// </summary>
        /// <param name="transform">The <c>Transform</c> component</param>
        /// <param name="points">The points to transform. The points are transformed in-place.</param>
        public static void TransformPointList(this Transform transform, List<Vector3> points)
        {
            if (transform == null)
                throw new ArgumentNullException("transform");

            if (points == null)
                throw new ArgumentNullException("points");

            for (int i = 0; i < points.Count; ++i)
            {
                points[i] = transform.TransformPoint(points[i]);
            }
        }

        /// <summary>
        /// Inverse transforms a <c>List</c> of <c>Vector3</c>s.
        /// </summary>
        /// <param name="transform">The <c>Transform</c> component</param>
        /// <param name="points">The points to inverse transform. This is done in-place.</param>
        public static void InverseTransformPointList(this Transform transform, List<Vector3> points)
        {
            if (transform == null)
                throw new ArgumentNullException("transform");

            if (points == null)
                throw new ArgumentNullException("points");

            for (int i = 0; i < points.Count; ++i)
            {
                points[i] = transform.InverseTransformPoint(points[i]);
            }
        }

        /// <summary>
        /// Sets the layer for the <c>GameObject</c> associated with <paramref name="transform"/> and all its children.
        /// </summary>
        /// <param name="transform">The <c>Transform</c> component</param>
        /// <param name="layer">The layer in which the game object should be.</param>
        public static void SetLayerRecursively(this Transform transform, int layer)
        {
            if (transform == null)
                throw new ArgumentNullException("transform");

            // Set self
            transform.gameObject.layer = layer;

            // Set all child layers recursively
            for (var i = 0; i < transform.childCount; ++i)
            {
                var child = transform.GetChild(i);
                child.SetLayerRecursively(layer);
            }
        }
    }
}
