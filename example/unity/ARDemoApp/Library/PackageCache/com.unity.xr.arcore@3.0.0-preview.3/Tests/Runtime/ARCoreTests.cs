using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARCore.Tests
{
    [TestFixture]
    public class ARCoreTestFixture
    {
        [Test]
        public void DepthSubsystemRegistered()
        {
#if !UNITY_EDITOR
            Assert.That(SubsystemDescriptorRegistered<XRDepthSubsystemDescriptor>("ARCore-Depth"));
#endif
        }

        [Test]
        public void SessionSubsystemRegistered()
        {
#if !UNITY_EDITOR
            Assert.That(SubsystemDescriptorRegistered<XRSessionSubsystemDescriptor>("ARCore-Session"));
#endif
        }

        [Test]
        public void PlaneSubsystemRegistered()
        {
#if !UNITY_EDITOR
            Assert.That(SubsystemDescriptorRegistered<XRPlaneSubsystemDescriptor>("ARCore-Plane"));
#endif
        }

        [Test]
        public void RaycastSubsystemRegistered()
        {
#if !UNITY_EDITOR
            Assert.That(SubsystemDescriptorRegistered<XRRaycastSubsystemDescriptor>("ARCore-Raycast"));
#endif
        }

        [Test]
        public void ReferencePointSubsystemRegistered()
        {
#if !UNITY_EDITOR
            Assert.That(SubsystemDescriptorRegistered<XRReferencePointSubsystemDescriptor>("ARCore-ReferencePoint"));
#endif
        }

        [Test]
        public void CameraSubsystemRegistered()
        {
#if !UNITY_EDITOR
            Assert.That(SubsystemDescriptorRegistered<XRCameraSubsystemDescriptor>("ARCore-Camera"));
#endif
        }
        bool SubsystemDescriptorRegistered<T>(string id) where T : SubsystemDescriptor
        {
            List<T> descriptors = new List<T>();

            SubsystemManager.GetSubsystemDescriptors<T>(descriptors);

            foreach(T descriptor in descriptors)
            {
                if (descriptor.id == id)
                {
                    return true;
                }
            }

            return false;
        }
    }
}