using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SpatialTracking;
using System.Linq;
using UnityEngine.Experimental.XR.Interaction;

namespace UnityEngine.SpatialTracking
{
    [TestFixture]
    public class TrackedPoseDriverTests
    {
        public class TestTrackedPoseDriverWrapper : TrackedPoseDriver
        {
            public void FakeUpdate()
            {
                Update();
            }

            public void FakeOnBeforeRender()
            {
                OnBeforeRender();
            }
        }

        static Vector3 testpos = new Vector3(1.0f, 2.0f, 3.0f);
        static Quaternion testrot = new Quaternion(0.09853293f, 0.09853293f, 0.09853293f, 0.9853293f);


        internal class TestPoseProvider : BasePoseProvider
        {
            public PoseDataFlags flags = PoseDataFlags.Position | PoseDataFlags.Rotation;

            public override PoseDataFlags GetPoseFromProvider(out Pose output)
            {
                Pose tmp = new Pose();                
                tmp.position = testpos;
                tmp.rotation = testrot;
                output = tmp;
                return flags;
            }
        }

        internal static TestTrackedPoseDriverWrapper CreateGameObjectWithTPD()
        {
            GameObject go = new GameObject();
            TestTrackedPoseDriverWrapper tpd = go.AddComponent<TestTrackedPoseDriverWrapper>();
            return tpd;
        }

        internal static BasePoseProvider CreatePoseProviderOnTPD(TestTrackedPoseDriverWrapper tpd)
        {
            TestPoseProvider tpp = tpd.gameObject.AddComponent<TestPoseProvider>();
            tpd.poseProviderComponent = tpp;
            return tpp;
        }

        [Test]
        public void TPDApiSetTest()
        {
            TestTrackedPoseDriverWrapper tpd = CreateGameObjectWithTPD();

            bool ret = tpd.SetPoseSource(TrackedPoseDriver.DeviceType.GenericXRDevice, TrackedPoseDriver.TrackedPose.Head);
            Assert.That(ret,  Is.EqualTo(true));
            Assert.That(tpd.poseSource, Is.EqualTo(TrackedPoseDriver.TrackedPose.Head));

            ret = tpd.SetPoseSource(TrackedPoseDriver.DeviceType.GenericXRDevice, TrackedPoseDriver.TrackedPose.LeftPose);
            Assert.That(ret, Is.EqualTo(false));
            Assert.That(tpd.poseSource, Is.EqualTo(TrackedPoseDriver.TrackedPose.Head));

            ret = tpd.SetPoseSource(TrackedPoseDriver.DeviceType.GenericXRController, TrackedPoseDriver.TrackedPose.RightPose);
            Assert.That(ret, Is.EqualTo(true));
            Assert.That(tpd.poseSource, Is.EqualTo(TrackedPoseDriver.TrackedPose.RightPose));
        }

        [Test]
        public void TPDPoseProviderTest()
        {
            TestTrackedPoseDriverWrapper tpd = CreateGameObjectWithTPD();
            BasePoseProvider pp = CreatePoseProviderOnTPD(tpd);

            Assert.That(tpd.poseProviderComponent, Is.EqualTo(pp));

            tpd.FakeUpdate();
            Assert.That(tpd.gameObject.transform.position, Is.EqualTo(testpos));
            Assert.That(tpd.gameObject.transform.rotation.Equals(testrot));

        }

        public void Reset(GameObject go)
        {
            go.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
            go.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        }

        [Test]
        public void TPDUpdateOptionTest()
        {
            TestTrackedPoseDriverWrapper tpd = CreateGameObjectWithTPD();
            BasePoseProvider pp = CreatePoseProviderOnTPD(tpd);

            Assert.That(tpd.poseProviderComponent, Is.EqualTo(pp));

            // check the update/before render case
            tpd.updateType = TrackedPoseDriver.UpdateType.BeforeRender;
            tpd.trackingType = TrackedPoseDriver.TrackingType.RotationAndPosition;
            Reset(tpd.gameObject);
            tpd.FakeUpdate();
            Assert.That(tpd.gameObject.transform.position, Is.Not.EqualTo(testpos));
            Assert.That(!tpd.gameObject.transform.rotation.Equals(testrot));

            tpd.FakeOnBeforeRender();
            Assert.That(tpd.gameObject.transform.position, Is.EqualTo(testpos));
            Assert.That(tpd.gameObject.transform.rotation.Equals(testrot));

            Reset(tpd.gameObject);

            tpd.updateType = TrackedPoseDriver.UpdateType.Update;
            tpd.trackingType = TrackedPoseDriver.TrackingType.RotationAndPosition;
            tpd.FakeOnBeforeRender();
            Assert.That(tpd.gameObject.transform.position, Is.Not.EqualTo(testpos));
            Assert.That(!tpd.gameObject.transform.rotation.Equals(testrot));

            tpd.FakeUpdate(); 
            Assert.That(tpd.gameObject.transform.position, Is.EqualTo(testpos));
            Assert.That(tpd.gameObject.transform.rotation.Equals(testrot));

            // check the rot/pos case
            tpd.updateType = TrackedPoseDriver.UpdateType.UpdateAndBeforeRender;

            tpd.trackingType = TrackedPoseDriver.TrackingType.PositionOnly;
            Reset(tpd.gameObject);
            tpd.FakeUpdate();
            Assert.That(tpd.gameObject.transform.position, Is.EqualTo(testpos));
            Assert.That(!tpd.gameObject.transform.rotation.Equals(testrot));

            tpd.trackingType = TrackedPoseDriver.TrackingType.RotationOnly;
            Reset(tpd.gameObject);
            tpd.FakeUpdate();
            Assert.That(tpd.gameObject.transform.position, Is.Not.EqualTo(testpos));
            Assert.That(tpd.gameObject.transform.rotation.Equals(testrot));
        }

        [Test]
        public void TPDPartialUpdateDataTest()
        {
            TestTrackedPoseDriverWrapper tpd = CreateGameObjectWithTPD();
            BasePoseProvider pp = CreatePoseProviderOnTPD(tpd);
            TestPoseProvider tpp = pp as TestPoseProvider;

            Assert.That(tpd.poseProviderComponent, Is.EqualTo(pp));

            tpp.flags = PoseDataFlags.Position;
            tpd.FakeUpdate();
            Assert.That(tpd.gameObject.transform.position, Is.EqualTo(testpos));
            Assert.That(!tpd.gameObject.transform.rotation.Equals(testrot));

            Reset(tpd.gameObject);
            tpp.flags = PoseDataFlags.Rotation;
            tpd.FakeUpdate();
            Assert.That(tpd.gameObject.transform.position, Is.Not.EqualTo(testpos));
            Assert.That(tpd.gameObject.transform.rotation.Equals(testrot));

            Reset(tpd.gameObject);
            tpp.flags = PoseDataFlags.Position | PoseDataFlags.Rotation;
            tpd.FakeUpdate();
            Assert.That(tpd.gameObject.transform.position, Is.EqualTo(testpos));
            Assert.That(tpd.gameObject.transform.rotation.Equals(testrot));
        }

    }
}
    