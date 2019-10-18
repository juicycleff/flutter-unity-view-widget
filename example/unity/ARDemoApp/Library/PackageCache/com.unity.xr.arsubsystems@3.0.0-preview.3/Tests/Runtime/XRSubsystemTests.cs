using NUnit.Framework;
using System.Collections.Generic;

namespace UnityEngine.XR.ARSubsystems.Tests
{
    public class XRTestSubsystemDescriptor : SubsystemDescriptor<XRTestSubsystem>
    { }

    public class XRTestSubsystem : XRSubsystem<XRTestSubsystemDescriptor>
    {
        public int startCount { get; private set; }
        public int stopCount { get; private set; }
        public int destroyCount { get; private set; }

        protected override void OnStart() => ++startCount;
        protected override void OnStop() => ++stopCount;
        protected override void OnDestroyed() => ++destroyCount;
    }

    [TestFixture]
    public class XRSubsystemTestFixture
    {
        static XRTestSubsystem RegisterAndCreate()
        {
            SubsystemRegistration.CreateDescriptor(new XRTestSubsystemDescriptor
            {
                id = "Test Subsystem",
                subsystemImplementationType = typeof(XRTestSubsystem),
            });

            var descriptors = new List<XRTestSubsystemDescriptor>();
            SubsystemManager.GetSubsystemDescriptors<XRTestSubsystemDescriptor>(descriptors);
            return descriptors[0].Create();
        }

        [Test]
        public void IsRunningAfterStartCalled()
        {
            var subsystem = RegisterAndCreate();
            subsystem.Start();
            Assert.IsTrue(subsystem.running);
            subsystem.Destroy();
        }

        [Test]
        public void IsNotRunningAfterStopCalled()
        {
            var subsystem = RegisterAndCreate();
            subsystem.Start();
            subsystem.Stop();
            Assert.IsFalse(subsystem.running);
            subsystem.Destroy();
        }

        [Test]
        public void DestroyCallsStopWhenRunning()
        {
            var subsystem = RegisterAndCreate();
            subsystem.Start();
            Assert.AreEqual(0, subsystem.stopCount);
            subsystem.Destroy();
            Assert.IsFalse(subsystem.running);
            Assert.AreEqual(1, subsystem.stopCount);
        }

        [Test]
        public void DestroyDoesNotCallStopWhenNotRunning()
        {
            var subsystem = RegisterAndCreate();
            subsystem.Start();
            subsystem.Stop();
            Assert.IsFalse(subsystem.running);
            Assert.AreEqual(1, subsystem.stopCount);
            subsystem.Destroy();
            Assert.AreEqual(1, subsystem.stopCount);
        }

        [Test]
        public void DestroyOnlyCalledOnce()
        {
            var subsystem = RegisterAndCreate();
            Assert.AreEqual(0, subsystem.destroyCount);

            subsystem.Destroy();
            Assert.AreEqual(1, subsystem.destroyCount);
            subsystem.Destroy();
            subsystem.Destroy();
            subsystem.Destroy();
            subsystem.Destroy();
            Assert.AreEqual(1, subsystem.destroyCount);
        }

        [Test]
        public void StartOnlyCalledWhenNotRunning()
        {
            var subsystem = RegisterAndCreate();
            Assert.AreEqual(0, subsystem.startCount);
            subsystem.Start();
            Assert.IsTrue(subsystem.running);
            Assert.AreEqual(1, subsystem.startCount);
            subsystem.Start();
            Assert.IsTrue(subsystem.running);
            Assert.AreEqual(1, subsystem.startCount);
            subsystem.Stop();
            Assert.IsFalse(subsystem.running);
            subsystem.Start();
            Assert.IsTrue(subsystem.running);
            Assert.AreEqual(2, subsystem.startCount);
            subsystem.Destroy();
        }

        [Test]
        public void StopOnlyCalledWhenRunning()
        {
            var subsystem = RegisterAndCreate();

            subsystem.Stop();
            Assert.AreEqual(0, subsystem.stopCount);

            subsystem.Start();
            Assert.IsTrue(subsystem.running);
            Assert.AreEqual(0, subsystem.stopCount);

            subsystem.Stop();
            Assert.IsFalse(subsystem.running);
            Assert.AreEqual(1, subsystem.stopCount);

            subsystem.Stop();
            Assert.AreEqual(1, subsystem.startCount);

            subsystem.Start();
            Assert.IsTrue(subsystem.running);
            subsystem.Stop();
            Assert.IsFalse(subsystem.running);
            Assert.AreEqual(2, subsystem.stopCount);
            subsystem.Destroy();
        }
    }
}
