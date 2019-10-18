using NUnit.Framework;
using Unity.Collections;

namespace UnityEngine.XR.ARSubsystems.Tests
{
    public class XRDepthSubsystemImpl : XRDepthSubsystem
    {
        protected override Provider CreateProvider() => new TestProvider();

        class TestProvider : Provider
        {
            public override TrackableChanges<XRPointCloud> GetChanges(XRPointCloud defaultPointCloud, Allocator allocator) => default;
            public override XRPointCloudData GetPointCloudData(TrackableId trackableId, Allocator allocator) => default;
        }
    }

    [TestFixture]
    public class XRDepthSubsystemTestFixture
    {
        [Test]
        public void RunningStateTests()
        {
            XRDepthSubsystem subsystem = new XRDepthSubsystemImpl();

            // Initial state is not running
            Assert.That(subsystem.running == false);

            // After start subsystem is running
            subsystem.Start();
            Assert.That(subsystem.running == true);

            // After start subsystem is running
            subsystem.Stop();
            Assert.That(subsystem.running == false);
        }
    }
}