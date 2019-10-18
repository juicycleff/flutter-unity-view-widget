using System;
using Unity.Collections;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Defines an interface for interacting with environment probe functionality for creating realistic lighting and
    /// environment texturing in AR scenes.
    /// </summary>
    public abstract class XREnvironmentProbeSubsystem : TrackingSubsystem<XREnvironmentProbe, XREnvironmentProbeSubsystemDescriptor>
    {
        /// <summary>
        /// Constructs an <see cref="XREnvironmentProbeSubsystem"/>.
        /// Do not create this directly.
        /// Call <c>Create</c> on an <see cref="XREnvironmentProbeSubsystemDescriptor"/> obtained from the <c>SubsystemManager</c>.
        /// </summary>
        public XREnvironmentProbeSubsystem() => m_Provider = CreateProvider();

        /// <summary>
        /// Specifies whether the AR session should automatically place environment probes in the scene.
        /// </summary>
        /// <value>
        /// <c>true</c> if automatic placement of environment probes is enabled. Otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// If both manual and automatic placement of environment probes are supported, manually placed environment
        /// probes may be specified via <see cref="AddEnvironmentProbe"/> regardless of whether automatic placement is
        /// enabled or not.
        /// </remarks>
        /// <exception cref="System.NotSupportedException">Thrown when setting this value to <c>true</c> for
        /// implementations that do not support automatic placement.</exception>
        public bool automaticPlacement
        {
            get { return m_AutomaticPlacement; }
            set
            {
                if (value && !SubsystemDescriptor.supportsAutomaticPlacement)
                    throw new NotSupportedException("subsystem does not support automatic placement of environment probes.");

                if (m_AutomaticPlacement != value && running)
                {
                    m_Provider.SetAutomaticPlacement(value);
                }

                m_AutomaticPlacement = value;
            }
        }
        bool m_AutomaticPlacement;

        /// <summary>
        /// Specifies whether the environment textures should be returned as HDR textures.
        /// </summary>
        /// <value>
        /// <c>true</c> if the environment textures should be returned as HDR textures. Otherwise, <c>false</c>.
        /// </value>
        public bool environmentTextureHDR
        {
            get { return m_EnvironmentTextureHDR; }
            set
            {
                if ((m_EnvironmentTextureHDR != value) && m_Provider.TrySetEnvironmentTextureHDREnabled(value))
                {
                    m_EnvironmentTextureHDR = value;
                }
            }
        }
        bool m_EnvironmentTextureHDR = true;

        /// <summary>
        ///
        /// </summary>
        /// <param name="allocator"></param>
        /// <returns></returns>
        public override TrackableChanges<XREnvironmentProbe> GetChanges(Allocator allocator)
        {
            return m_Provider.GetChanges(XREnvironmentProbe.defaultValue, allocator);
        }

        /// <summary>
        /// Starts the subsystem. If <see cref="automaticPlacement"/> is <c>true</c>, environment probes will be created automatically.
        /// </summary>
        protected sealed override void OnStart()
        {
            m_Provider.SetAutomaticPlacement(automaticPlacement);
            m_Provider.Start();
        }

        /// <summary>
        /// Stops the subsystem. This does not remove existing environment probes, but it stops automatically placing them, and manually placed probes will not be updated
        /// until <see cref="OnStart"/> is called again.
        /// </summary>
        protected sealed override void OnStop() => m_Provider.Stop();

        /// <summary>
        /// Destroys the subsystem and any internal state.
        /// </summary>
        protected sealed override void OnDestroyed() => m_Provider.Destroy();

        /// <summary>
        /// Tries to create an environment probe.
        /// </summary>
        /// <param name="pose">The position and rotation at which to create the environment probe.</param>
        /// <param name="scale">The scale at which to create the environment probe.</param>
        /// <param name="size">The size (dimensions) of the environment probe to create.</param>
        /// <param name="environmentProbe">If successful, populated with the newly created environment probe. Otherwise, it will contain default values.</param>
        /// <returns>
        /// <c>true</c> if the environment probe was successfully added, otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="InvalidOperationException">Thrown when the environment probe subsystem is not running and
        /// this method is called to an add environment probe.</exception>
        /// <exception cref="NotSupportedException">Thrown for platforms that do not support manual placement of
        /// environment probes.</exception>
        public bool TryAddEnvironmentProbe(Pose pose, Vector3 scale, Vector3 size, out XREnvironmentProbe environmentProbe)
        {
            if (!running)
            {
                throw new InvalidOperationException("cannot add environment probes when environment probe system is not running");
            }

            environmentProbe = XREnvironmentProbe.defaultValue;
            return m_Provider.TryAddEnvironmentProbe(pose, scale, size, out environmentProbe);
        }

        /// <summary>
        /// Asynchronously removes the environment probe matching the trackable ID from the AR session.
        /// </summary>
        /// <param name='trackableId'>Trackable ID of the environment probe to be removed from the AR session.</param>
        /// <returns>
        /// <c>true</c> if the environment probe is found in the current AR session and will be removed. Otherwise,
        /// <c>false</c>.
        /// </returns>
        /// <remarks>
        /// <c>RemoveEnvironmentProbe</c> may be used to remove both manually-placed and automatically-placed
        /// environment probes if the implementation supports such removals, as indicated by the descriptor properties
        /// <see cref="XREnvironmentProbeSubsystemDescriptor.supportsRemovalOfManual"/> and
        /// <see cref="XREnvironmentProbeSubsystemDescriptor.supportsRemovalOfAutomatic"/>.
        /// </remarks>
        /// <exception cref="InvalidOperationException">Thrown when the environment probe subsystem is not running and
        /// this method is called to an add environment probe.</exception>
        /// <exception cref="System.NotSupportedException">Thrown for platforms that do not support removal of the
        /// type of environment probe.</exception>
        public bool RemoveEnvironmentProbe(TrackableId trackableId)
        {
            if (!running)
            {
                throw new InvalidOperationException("cannot remove environment probes when environment probe system is not running");
            }

            return m_Provider.RemoveEnvironmentProbe(trackableId);
        }

        /// <summary>
        /// Must create an implementation of <see cref="Provider"/>, the provider-specific implementation of this subsystem.
        /// </summary>
        protected abstract Provider CreateProvider();

        /// <summary>
        /// The class for providers to implement to support the <see cref="XREnvironmentProbeSubsystem"/>.
        /// </summary>
        protected abstract class Provider
        {
            /// <summary>
            /// Starts the subsystem. Will only be invoked if <see cref="running"/> is <c>false</c>.
            /// </summary>
            public virtual void Start()
            { }

            /// <summary>
            /// Stops the subsystem. Will only be invoked if <see cref="running"/> is <c>true</c>.
            /// </summary>
            public virtual void Stop()
            { }

            /// <summary>
            /// Invoked when the <see cref="XREnvironmentProbeSubsystem"/> is about to be destroyed.
            /// </summary>
            public virtual void Destroy()
            { }

            /// <summary>
            /// Overridden by the provider implementation to set the automatic placement state for the environment probe
            /// subsystem.
            /// </summary>
            /// <param name='value'>Whether automatic placement of environment probes should be enabled (<c>true</c>) or
            /// disabled (<c>false</c>).</param>
            /// <exception cref="System.NotSupportedException">Thrown in the defualt implementation if
            /// <paramref name="value"/> is <c>true</c>.</exception>
            public virtual void SetAutomaticPlacement(bool value)
            {
                if (value)
                {
                    throw new NotSupportedException("automatic placement of environment probes is not supported by this implementation");
                }
            }

            /// <summary>
            /// Overridden by the provider implementation to set the state of HDR environment texture generation.
            /// </summary>
            /// <param name="value">Whether HDR environment texture generation is enabled (<c>true</c>) or disabled
            /// (<c>false</c>).</param>
            /// <returns>
            /// Whether the HDR environment texture generation state was set.
            /// </returns>
            /// <exception cref="System.NotSupportedException">Thrown if the implementation does not support HDR
            /// environment textures if the state is being enabled.</exception>
            public virtual bool TrySetEnvironmentTextureHDREnabled(bool value)
            {
                if (value)
                {
                    throw new NotSupportedException("HDR environment textures are not supported by this implementation");
                }
                return false;
            }

            /// <summary>
            /// Overridden by the provider implementation to manually add an environment probe to the AR session.
            /// </summary>
            /// <param name='pose'>The position and rotation at which to create the new environment probe.</param>
            /// <param name='scale'>The scale of the new environment probe.</param>
            /// <param name='size'>The size (dimensions) of the new environment probe.</param>
            /// <param name='environmentProbe'>If successful, should be populated with the newly created environment probe.</param>
            /// <returns>
            /// <c>true</c> if a new environment probe was created, otherwise <c>false</c>.
            /// </returns>
            /// <exception cref="System.NotSupportedException">Thrown in the default implementation of this method.</exception>
            public virtual bool TryAddEnvironmentProbe(Pose pose, Vector3 scale, Vector3 size, out XREnvironmentProbe environmentProbe)
            {
                throw new NotSupportedException("manual placement of environment probes is not supported by this implementation");
            }

            /// <summary>
            /// Overridden by the provider to remove the environment probe matching the trackable ID from
            /// the AR session.
            /// </summary>
            /// <param name='trackableId'>Trackable ID of the environment probe to be removed from the AR session.</param>
            /// <returns>
            /// <c>true</c> whether the environment probe is found in the current AR session and will be removed.
            /// Otherwise, <c>false</c>.
            /// </returns>
            /// <remarks>
            /// This method may be used to remove both manually-placed and automatically-placed environment probes if the
            /// implementation supports such removals. Providers should implement this method remove environment probes of
            /// the allowed types and to throw a <c>System.NotSupportedException</c> for removals of environment probes of
            /// disallowed types.
            /// </remarks>
            /// <exception cref="System.NotSupportedException">Thrown in the default implementation.</exception>
            public virtual bool RemoveEnvironmentProbe(TrackableId trackableId)
            {
                throw new NotSupportedException("removal of environment probes is not supported by this implementation");
            }

            /// <summary>
            /// Get changes (added, updated, and removed) in environment probes since the last call to this method.
            /// </summary>
            /// <param name="defaultEnvironmentProbe">A default value for environment probes. Implementations should first fill their output
            /// arrays with copies of this value, then copy in their own. See the <see cref="NativeCopyUtility"/>.
            /// This allows additional fields to be added to the <see cref="XREnvironmentProbe"/> in the future.</param>
            /// <param name="allocator">The allocator to use for the <c>NativeArray</c>s in the returned <see cref="TrackableChanges{T}"/>.</param>
            /// <returns>The environment probes which have been added, updated, and removed since the last call to this method.</returns>
            /// <seealso cref="NativeCopyUtility"/>
            public abstract TrackableChanges<XREnvironmentProbe> GetChanges(XREnvironmentProbe defaultEnvironmentProbe, Allocator allocator);
        }

        /// <summary>
        /// Registers a subsystem implementation based on the given subystem parameters.
        /// </summary>
        /// <param name='environmentProbeSubsystemCinfo'>The parameters defining the environment probe functionality
        /// implemented by the subsystem provider.</param>
        /// <returns>
        /// <c>true</c> if the subsystem implementation is registered. Otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the values specified in the
        /// <paramref name="environmentProbeSubsystemCinfo"/> parameter are invalid. Typically, this will occur
        /// <list type="bullet">
        /// <item>
        /// <description>if <see cref="XREnvironmentProbeSubsystemCinfo.id"/> is <c>null</c> or empty</description>
        /// </item>
        /// <item>
        /// <description>if <see cref="XREnvironmentProbeSubsystemCinfo.implementationType"/> is <c>null</c>
        /// </description>
        /// </item>
        /// <item>
        /// <description>if <see cref="XREnvironmentProbeSubsystemCinfo.implementationType"/> does not derive from the
        /// <c>XREnvironmentProbeSubsystem</c> class
        /// </description>
        /// </item>
        /// </list>
        /// </exception>
        public static bool Register(XREnvironmentProbeSubsystemCinfo environmentProbeSubsystemCinfo)
        {
            XREnvironmentProbeSubsystemDescriptor environmentProbeSubsystemDescriptor = XREnvironmentProbeSubsystemDescriptor.Create(environmentProbeSubsystemCinfo);

            return SubsystemRegistration.CreateDescriptor(environmentProbeSubsystemDescriptor);
        }

        Provider m_Provider;
    }
}
