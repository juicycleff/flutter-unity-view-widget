using AOT;
using System;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// ARKit implementation of the <c>XRSessionSubsystem</c>. Do not create this directly. Use the <c>SubsystemManager</c> instead.
    /// </summary>
    [Preserve]
    public sealed class ARKitSessionSubsystem : XRSessionSubsystem
    {
        /// <summary>
        /// <c>true</c> if [Coaching Overlay](https://developer.apple.com/documentation/arkit/arcoachingoverlayview) is supported, otherwise <c>false</c>.
        /// </summary>
        public static bool coachingOverlaySupported
        {
            get
            {
#if UNITY_IOS && !UNITY_EDITOR
                return NativeApi.UnityARKit_coachingOverlay_isSupported();
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Whether the [Coaching Overlay](https://developer.apple.com/documentation/arkit/arcoachingoverlayview)
        /// activates automatically or not. By default, it does not.
        /// </summary>
        public bool coachingActivatesAutomatically
        {
            get => NativeApi.UnityARKit_coachingOverlay_getActivatesAutomatically();
            set => NativeApi.UnityARKit_coachingOverlay_setActivatesAutomatically(value);
        }

        /// <summary>
        /// Defines the [Coaching Goal](https://developer.apple.com/documentation/arkit/arcoachingoverlayview/3192180-goal).
        /// This should be based on your app's tracking requirements and affects the UI that the coaching overlay presents.
        /// </summary>
        public ARCoachingGoal coachingGoal
        {
            get => NativeApi.UnityARKit_coachingOverlay_getGoal();
            set => NativeApi.UnityARKit_coachingOverlay_setGoal(value);
        }

        /// <summary>
        /// <c>true</c> if the [Coaching Overlay](https://developer.apple.com/documentation/arkit/arcoachingoverlayview) is active.
        /// </summary>
        public bool coachingActive => NativeApi.UnityARKit_coachingOverlay_isActive();

        /// <summary>
        /// Activates or deactivates the [Coaching Overlay](https://developer.apple.com/documentation/arkit/arcoachingoverlayview)
        /// </summary>
        /// <param name="active">Whether the coaching overlay should be active or not.</param>
        /// <param name="animate">The type of transition to use when showing or hiding the coaching overlay.</param>
        public void SetCoachingActive(bool active, ARCoachingOverlayTransition transition)
        {
            NativeApi.UnityARKit_coachingOverlay_setActive(active, transition == ARCoachingOverlayTransition.Animated);
        }

        /// <summary>
        /// <para>Asynchronously create an <see cref="ARWorldMap"/>. An <c>ARWorldMap</c>
        /// represents the state of the session and can be serialized to a byte
        /// array to persist the session data, or send it to another device for
        /// shared AR experiences.</para>
        /// <para>It is a wrapper for <a href="https://developer.apple.com/documentation/arkit/arworldmap">ARKit's ARWorldMap</a>.</para>
        /// </summary>
        /// <returns>An <see cref="ARWorldMapRequest"/> which can be used to determine the status
        /// of the request and get the <c>ARWorldMap</c> when complete.</returns>
        /// <seealso cref="ApplyWorldMap(ARWorldMap)"/>
        /// <seealso cref="worldMapSupported"/>
        public ARWorldMapRequest GetARWorldMapAsync()
        {
            var requestId = NativeApi.UnityARKit_createWorldMapRequest();
            return new ARWorldMapRequest(requestId);
        }

        /// <summary>
        /// <para>
        /// Asynchronously create an <see cref="ARWorldMap"/>. An <c>ARWorldMap</c>
        /// represents the state of the session and can be serialized to a byte
        /// array to persist the session data, or send it to another device for
        /// shared AR experiences.
        /// </para>
        /// <para>
        /// It is a wrapper for <a href="https://developer.apple.com/documentation/arkit/arworldmap">ARKit's ARWorldMap</a>.
        /// </para>
        /// <para>
        /// If the <see cref="ARWorldMapRequestStatus"/> is <see cref="ARWorldMapRequestStatus.Success"/>, then
        /// the resulting <see cref="ARWorldMap"/> must be disposed to avoid leaking native resources. Otherwise,
        /// the <see cref="ARWorldMap"/> is not valid, and need not be disposed.
        /// </para>
        /// </summary>
        /// <param name="onComplete">A method to invoke when the world map has either been created, or determined
        /// that it could not be created. Check the value of the <see cref="ARWorldMapRequestStatus"/> parameter
        /// to determine whether the world map was successfully created.</param>
        /// <seealso cref="ApplyWorldMap(ARWorldMap)"/>
        /// <seealso cref="worldMapSupported"/>
        public void GetARWorldMapAsync(
            Action<ARWorldMapRequestStatus, ARWorldMap> onComplete)
        {
            var handle = GCHandle.Alloc(onComplete);
            var context = GCHandle.ToIntPtr(handle);

            NativeApi.UnityARKit_createWorldMapRequestWithCallback(s_OnAsyncWorldMapCompleted, context);
        }

        /// <summary>
        /// Detect <see cref="ARWorldMap"/> support. <c>ARWorldMap</c> requires iOS 12 or greater.
        /// </summary>
        /// <returns><c>true</c> if <c>ARWorldMap</c>s are supported, otherwise <c>false</c>.</returns>
        /// <seealso cref="GetARWorldMapAsync()"/>
        public static bool worldMapSupported
        {
            get
            {
#if UNITY_IOS && !UNITY_EDITOR
                return NativeApi.UnityARKit_worldMapSupported();
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Get the world mapping status. Used to determine the suitability of the current session for
        /// creating an <see cref="ARWorldMap"/>.
        /// </summary>
        /// <returns>The <see cref="ARWorldMappingStatus"/> of the session.</returns>
        public ARWorldMappingStatus worldMappingStatus => NativeApi.UnityARKit_session_getWorldMappingStatus();

        /// <summary>
        /// Apply an existing <see cref="ARWorldMap"/> to the session. This will attempt
        /// to relocalize the current session to the given <paramref name="worldMap"/>.
        /// If relocalization is successful, the stored planes & reference points from
        /// the <paramref name="worldMap"/> will be added to the current session.
        /// This is equivalent to setting the <a href="https://developer.apple.com/documentation/arkit/arworldtrackingconfiguration/2968180-initialworldmap">initialWorldMap</a>
        /// property on the session's <a href="https://developer.apple.com/documentation/arkit/arworldtrackingconfiguration">ARWorldTrackingConfiguration</a>.
        /// </summary>
        /// <param name="worldMap">An <see cref="ARWorldMap"/> with which to relocalize the session.</param>
        public void ApplyWorldMap(ARWorldMap worldMap)
        {
            if (worldMap.nativeHandle == ARWorldMap.k_InvalidHandle)
                throw new InvalidOperationException("ARWorldMap has been disposed.");

            NativeApi.UnityARKit_applyWorldMap(worldMap.nativeHandle);
        }

        /// <summary>
        /// Get or set whether collaboration is enabled. When collaboration is enabled, collaboration
        /// data is accumulated by the subsystem until you read it out with <see cref="DequeueCollaborationData"/>.
        /// </summary>
        /// <remarks>
        /// Note: If you change this value, the new value may not be reflected until the next frame.
        /// </remarks>
        /// <seealso cref="ARCollaborationData"/>
        /// <seealso cref="DequeueCollaborationData"/>
        /// <seealso cref="collaborationDataCount"/>
        public bool collaborationEnabled
        {
            get => NativeApi.UnityARKit_session_getCollaborationEnabled();
            set
            {
                if (supportsCollaboration)
                {
                    NativeApi.UnityARKit_session_setCollaborationRequested(value);
                }
                else if (value)
                {
                    throw new NotSupportedException("ARCollaborationData is not supported by this version of iOS.");
                }
            }
        }

        /// <summary>
        /// True if collaboration is supported. Collaboration is only supported on iOS versions 13.0 and later.
        /// </summary>
        /// <seealso cref="ARCollaborationData"/>
        public static bool supportsCollaboration => s_SupportsCollaboration;

        /// <summary>
        /// The number of <see cref="ARCollaborationData"/>s in the queue. Obtain <see cref="ARCollaborationData"/>
        /// with <see cref="DequeueCollaborationData"/>.
        /// </summary>
        /// <seealso cref="ARCollaborationData"/>
        /// <seealso cref="DequeueCollaborationData"/>
        public int collaborationDataCount => NativeApi.UnityARKit_session_getCollaborationDataQueueSize();

        /// <summary>
        /// Dequeues the oldest collaboration data in the queue. After calling this method, <see cref="collaborationDataCount"/>
        /// will be decremented by one.
        /// </summary>
        /// <exception cref="System.NotSupportedException">Thrown if <see cref="supportsCollaboration"/> is false.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if <see cref="collaborationDataCount"/> is zero.</exception>
        /// <seealso cref="ARCollaborationData"/>
        public ARCollaborationData DequeueCollaborationData()
        {
            if (!supportsCollaboration)
                throw new NotSupportedException("ARCollaborationData is not supported by this version of iOS.");

            if (collaborationDataCount == 0)
                throw new InvalidOperationException("There is no collaboration data to dequeue.");

            return new ARCollaborationData(NativeApi.UnityARKit_session_dequeueCollaborationData());
        }

        /// <summary>
        /// Applies <see cref="ARCollaborationData"/> to the session.
        /// </summary>
        /// <exception cref="System.NotSupportedException">Thrown if <see cref="supportsCollaboration"/> is false.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if <paramref name="collaborationData"/> is not valid.</exception>
        public void UpdateWithCollaborationData(ARCollaborationData collaborationData)
        {
            if (!supportsCollaboration)
                throw new NotSupportedException("ARCollaborationData is not supported by this version of iOS.");

            if (!collaborationData.valid)
                throw new InvalidOperationException("Invalid collaboration data.");

            NativeApi.UnityARKit_session_updateWithCollaborationData(collaborationData.m_NativePtr);
        }

        /// <summary>
        /// Creates the provider interface.
        /// </summary>
        /// <returns>The provider interface for ARKit</returns>
        protected override Provider CreateProvider() => new ARKitProvider();

        static ARKitSessionSubsystem()
        {
            s_OnAsyncWorldMapCompleted = OnAsyncConversionComplete;
#if UNITY_IOS && !UNITY_EDITOR
            s_SupportsCollaboration = NativeApi.UnityARKit_session_getCollaborationSupported();
#else
            s_SupportsCollaboration = false;
#endif
        }

        static NativeApi.OnAsyncConversionCompleteDelegate s_OnAsyncWorldMapCompleted;

        static bool s_SupportsCollaboration;

        [MonoPInvokeCallback(typeof(NativeApi.OnAsyncConversionCompleteDelegate))]
        static unsafe void OnAsyncConversionComplete(ARWorldMapRequestStatus status, int worldMapId, IntPtr context)
        {
            var handle = GCHandle.FromIntPtr(context);
            var onComplete = (Action<ARWorldMapRequestStatus, ARWorldMap>)handle.Target;

            if (status.IsError())
            {
                onComplete(status, default(ARWorldMap));
            }
            else
            {
                var worldMap = new ARWorldMap(worldMapId);
                onComplete(status, worldMap);
            }

            handle.Free();
        }

        class ARKitProvider : Provider
        {
            public ARKitProvider() => NativeApi.UnityARKit_session_construct();

            public override void Resume() => NativeApi.UnityARKit_session_resume();

            public override void Pause() => NativeApi.UnityARKit_session_pause();

            public override void Update(XRSessionUpdateParams updateParams) => NativeApi.UnityARKit_session_update();

            public override void Destroy() => NativeApi.UnityARKit_session_destroy();

            public override void Reset() => NativeApi.UnityARKit_session_reset();

            public override Promise<SessionAvailability> GetAvailabilityAsync()
            {
                var result = NativeApi.UnityARKit_session_getAvailability();
                var retVal = SessionAvailability.None;
                if (result == NativeApi.Availability.Supported)
                    retVal = SessionAvailability.Installed | SessionAvailability.Supported;

                return Promise<SessionAvailability>.CreateResolvedPromise(retVal);
            }

            public override Promise<SessionInstallationStatus> InstallAsync() =>
                throw new NotSupportedException("ARKit cannot be installed.");

            public override IntPtr nativePtr => NativeApi.UnityARKit_session_getNativePtr();

            public override TrackingState trackingState => NativeApi.UnityARKit_session_getTrackingState();

            public override NotTrackingReason notTrackingReason => NativeApi.UnityARKit_session_getNotTrackingReason();

            public override Guid sessionId => NativeApi.UnityARKit_session_getSessionId();

            public override bool matchFrameRate
            {
                get => NativeApi.UnityARKit_session_getMatchFrameRateEnabled();
                set => NativeApi.UnityARKit_session_setMatchFrameRateEnabled(value);
            }

            public override int frameRate => NativeApi.UnityARKit_Session_GetFrameRate();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RegisterDescriptor()
        {
#if UNITY_IOS && !UNITY_EDITOR
            NativeApi.UnityARKit_ensureRootViewIsSetup();
            XRSessionSubsystemDescriptor.RegisterDescriptor(new XRSessionSubsystemDescriptor.Cinfo
            {
                id = "ARKit-Session",
                subsystemImplementationType = typeof(ARKitSessionSubsystem),
                supportsInstall = false,
                supportsMatchFrameRate = true
            });
#endif
        }

        static class NativeApi
        {
            // Should match ARKitAvailability in ARKitXRSessionProvider.mm
            public enum Availability
            {
                None,
                Supported
            }

            public delegate void OnAsyncConversionCompleteDelegate(
                ARWorldMapRequestStatus status,
                int worldMapId,
                IntPtr context);

            [DllImport("__Internal")]
            public static extern int UnityARKit_createWorldMapRequest();

            [DllImport("__Internal")]
            public static extern void UnityARKit_createWorldMapRequestWithCallback(
                OnAsyncConversionCompleteDelegate callback,
                IntPtr context);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_worldMapSupported();

            [DllImport("__Internal")]
            public static extern ARWorldMappingStatus UnityARKit_session_getWorldMappingStatus();

            [DllImport("__Internal")]
            public static extern void UnityARKit_applyWorldMap(int worldMapId);

            [DllImport("__Internal")]
            public static extern IntPtr UnityARKit_session_getNativePtr();

            [DllImport("__Internal")]
            public static extern Availability UnityARKit_session_getAvailability();

            [DllImport("__Internal")]
            public static extern void UnityARKit_session_update();

            [DllImport("__Internal")]
            public static extern void UnityARKit_session_construct();

            [DllImport("__Internal")]
            public static extern void UnityARKit_session_destroy();

            [DllImport("__Internal")]
            public static extern void UnityARKit_session_resume();

            [DllImport("__Internal")]
            public static extern void UnityARKit_session_pause();

            [DllImport("__Internal")]
            public static extern void UnityARKit_session_reset();

            [DllImport("__Internal")]
            public static extern TrackingState UnityARKit_session_getTrackingState();

            [DllImport("__Internal")]
            public static extern NotTrackingReason UnityARKit_session_getNotTrackingReason();

            [DllImport("__Internal")]
            public static extern bool UnityARKit_session_getCollaborationSupported();

            [DllImport("__Internal")]
            public static extern IntPtr UnityARKit_session_dequeueCollaborationData();

            [DllImport("__Internal")]
            public static extern int UnityARKit_session_getCollaborationDataQueueSize();

            [DllImport("__Internal")]
            public static extern void UnityARKit_session_updateWithCollaborationData(IntPtr data);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_session_getCollaborationEnabled();

            [DllImport("__Internal")]
            public static extern void UnityARKit_session_setCollaborationRequested(bool requested);

            [DllImport("__Internal")]
            public static extern Guid UnityARKit_session_getSessionId();

            [DllImport("__Internal")]
            public static extern bool UnityARKit_session_getMatchFrameRateEnabled();

            [DllImport("__Internal")]
            public static extern void UnityARKit_session_setMatchFrameRateEnabled(bool enabled);

            [DllImport("__Internal")]
            public static extern int UnityARKit_Session_GetFrameRate();

            [DllImport("__Internal")]
            public static extern bool UnityARKit_coachingOverlay_getActivatesAutomatically();

            [DllImport("__Internal")]
            public static extern void UnityARKit_coachingOverlay_setActivatesAutomatically(bool activatesAutomatically);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_coachingOverlay_isActive();

            [DllImport("__Internal")]
            public static extern void UnityARKit_coachingOverlay_setGoal(ARCoachingGoal goal);

            [DllImport("__Internal")]
            public static extern ARCoachingGoal UnityARKit_coachingOverlay_getGoal();

            [DllImport("__Internal")]
            public static extern void UnityARKit_coachingOverlay_setActive(bool active, bool animated);

            [DllImport("__Internal")]
            public static extern void UnityARKit_ensureRootViewIsSetup();

            [DllImport("__Internal")]
            public static extern bool UnityARKit_coachingOverlay_isSupported();
        }
    }
}
