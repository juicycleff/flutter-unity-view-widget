using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Manages an <c>XRRaycastSubsystem</c>, exposing raycast functionality in ARFoundation. Use this component
    /// to raycast against trackables (i.e., detected features in the physical environment) when they do not have
    /// a presence in the Physics world.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ARSessionOrigin))]
    [HelpURL("https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@latest?preview=1&subfolder=/api/UnityEngine.XR.ARFoundation.ARRaycastManager.html")]
    public sealed class ARRaycastManager : SubsystemLifecycleManager<XRRaycastSubsystem, XRRaycastSubsystemDescriptor>
    {
        /// <summary>
        /// Cast a ray from a point in screen space against trackables, i.e., detected features such as planes.
        /// </summary>
        /// <param name="screenPoint">The point, in device screen pixels, from which to cast.</param>
        /// <param name="hitResults">Contents are replaced with the raycast results, if successful.</param>
        /// <param name="trackableTypes">(Optional) The types of trackables to cast against.</param>
        /// <returns>True if the raycast hit a trackable in the <paramref name="trackableTypes"/></returns>
        public bool Raycast(
            Vector2 screenPoint,
            List<ARRaycastHit> hitResults,
            TrackableType trackableTypes = TrackableType.All)
        {
            if (subsystem == null)
                return false;

            if (hitResults == null)
                throw new ArgumentNullException("hitResults");

            var nativeHits = m_RaycastViewportDelegate(screenPoint, trackableTypes, Allocator.Temp);
            var originTransform = m_SessionOrigin.camera != null ? m_SessionOrigin.camera.transform : m_SessionOrigin.trackablesParent;
            return TransformAndDisposeNativeHitResults(nativeHits, hitResults, originTransform.position);
        }

        /// <summary>
        /// Cast a <c>Ray</c> against trackables, i.e., detected features such as planes.
        /// </summary>
        /// <param name="ray">The <c>Ray</c>, in Unity world space, to cast.</param>
        /// <param name="hitResults">Contents are replaced with the raycast results, if successful.</param>
        /// <param name="trackableTypes">(Optional) The types of trackables to cast against.</param>
        /// <returns>True if the raycast hit a trackable in the <paramref name="trackableTypes"/></returns>
        public bool Raycast(
            Ray ray,
            List<ARRaycastHit> hitResults,
            TrackableType trackableTypes = TrackableType.All)
        {
            if (subsystem == null)
                return false;

            if (hitResults == null)
                throw new ArgumentNullException("hitResults");

            var sessionSpaceRay = m_SessionOrigin.trackablesParent.InverseTransformRay(ray);
            var nativeHits = m_RaycastRayDelegate(sessionSpaceRay, trackableTypes, Allocator.Temp);
            return TransformAndDisposeNativeHitResults(nativeHits, hitResults, ray.origin);
        }

        static void TransformAndSortRaycastResults(
            Transform transform,
            NativeArray<XRRaycastHit> nativeHits,
            List<ARRaycastHit> managedHits,
            Vector3 rayOrigin)
        {
            foreach (var nativeHit in nativeHits)
            {
                float distanceInWorldSpace = (nativeHit.pose.position - rayOrigin).magnitude;
                managedHits.Add(new ARRaycastHit(nativeHit, distanceInWorldSpace, transform));
            }
        }

        /// <summary>
        /// Allows AR managers to register themselves as a raycaster.
        /// Raycasters be used as a fallback method if the AR platform does
        /// not support raycasting using arbitrary <c>Ray</c>s.
        /// </summary>
        /// <param name="raycaster">A raycaster implementing the IRaycast interface.</param>
        internal void RegisterRaycaster(IRaycaster raycaster)
        {
            ConstructIfNecessary();
            if (!m_Raycasters.Contains(raycaster))
                m_Raycasters.Add(raycaster);
        }

        /// <summary>
        /// Unregisters a raycaster previously registered with <see cref="RegisterRaycaster(IRaycaster)"/>.
        /// </summary>
        /// <param name="raycaster">A raycaster to use in the fallback case.</param>
        internal void UnregisterRaycaster(IRaycaster raycaster)
        {
            if (m_Raycasters != null)
                m_Raycasters.Remove(raycaster);
        }

        protected override void OnAfterStart()
        {
            if (subsystem.SubsystemDescriptor.supportsViewportBasedRaycast)
            {
                m_RaycastViewportDelegate = RaycastViewport;
            }
            else
            {
                m_RaycastViewportDelegate = RaycastViewportAsRay;
            }

            if (subsystem.SubsystemDescriptor.supportsWorldBasedRaycast)
            {
                m_RaycastRayDelegate = RaycastRay;
            }
            else
            {
                m_RaycastRayDelegate = RaycastFallback;
            }

            var raycasters = GetComponents(typeof(IRaycaster));
            foreach (var raycaster in raycasters)
                RegisterRaycaster((IRaycaster)raycaster);
        }

        NativeArray<XRRaycastHit> RaycastViewportAsRay(
            Vector2 screenPoint,
            TrackableType trackableTypeMask,
            Allocator allocator)
        {
            if (m_SessionOrigin.camera == null)
                return new NativeArray<XRRaycastHit>(0, allocator);

            var worldSpaceRay = m_SessionOrigin.camera.ScreenPointToRay(screenPoint);
            var sessionSpaceRay = m_SessionOrigin.trackablesParent.InverseTransformRay(worldSpaceRay);
            return m_RaycastRayDelegate(sessionSpaceRay, trackableTypeMask, allocator);
        }

        NativeArray<XRRaycastHit> RaycastViewport(
            Vector2 screenPoint,
            TrackableType trackableTypeMask,
            Allocator allocator)
        {
            screenPoint.x = Mathf.Clamp01(screenPoint.x / Screen.width);
            screenPoint.y = Mathf.Clamp01(screenPoint.y / Screen.height);
            return subsystem.Raycast(screenPoint, trackableTypeMask, allocator);
        }

        NativeArray<XRRaycastHit> RaycastRay(
            Ray ray,
            TrackableType trackableTypeMask,
            Allocator allocator)
        {
            return subsystem.Raycast(ray, trackableTypeMask, allocator);
        }

        static int RaycastHitComparer(ARRaycastHit lhs, ARRaycastHit rhs)
        {
            return lhs.CompareTo(rhs);
        }

        NativeArray<XRRaycastHit> RaycastFallback(
            Ray ray,
            TrackableType trackableTypeMask,
            Allocator allocator)
        {
            s_NativeRaycastHits.Clear();
            int count = 0;
            foreach (var raycaster in m_Raycasters)
            {
                var hits = raycaster.Raycast(ray, trackableTypeMask, Allocator.Temp);
                if (hits.IsCreated)
                {
                    s_NativeRaycastHits.Add(hits);
                    count += hits.Length;
                }
            }

            var allHits = new NativeArray<XRRaycastHit>(count, allocator);
            int dstIndex = 0;
            foreach (var hitArray in s_NativeRaycastHits)
            {
                NativeArray<XRRaycastHit>.Copy(hitArray, 0, allHits, dstIndex, hitArray.Length);
                hitArray.Dispose();
                dstIndex += hitArray.Length;
            }

            return allHits;
        }

        bool TransformAndDisposeNativeHitResults(
            NativeArray<XRRaycastHit> nativeHits,
            List<ARRaycastHit> managedHits,
            Vector3 rayOrigin)
        {
            managedHits.Clear();
            if (!nativeHits.IsCreated)
                return false;

            try
            {
                // Results are in "trackables space", so transform results back into world space
                TransformAndSortRaycastResults(m_SessionOrigin.trackablesParent, nativeHits, managedHits, rayOrigin);
                managedHits.Sort(s_RaycastHitComparer);
                return managedHits.Count > 0;
            }
            finally
            {
                nativeHits.Dispose();
            }
        }

        void ConstructIfNecessary()
        {
            if (m_Raycasters == null)
                m_Raycasters = new List<IRaycaster>();
        }

        void Awake()
        {
            m_SessionOrigin = GetComponent<ARSessionOrigin>();
            ConstructIfNecessary();
        }

        static Comparison<ARRaycastHit> s_RaycastHitComparer = RaycastHitComparer;

        static List<NativeArray<XRRaycastHit>> s_NativeRaycastHits = new List<NativeArray<XRRaycastHit>>();

        ARSessionOrigin m_SessionOrigin;

        Func<Vector2, TrackableType, Allocator, NativeArray<XRRaycastHit>> m_RaycastViewportDelegate;

        Func<Ray, TrackableType, Allocator, NativeArray<XRRaycastHit>> m_RaycastRayDelegate;

        List<IRaycaster> m_Raycasters;
    }
}
