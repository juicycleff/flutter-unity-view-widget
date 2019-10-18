using NUnit.Framework;
using Unity.Collections;

namespace UnityEngine.XR.ARSubsystems.Tests
{
    public class XRPlaneSubsystemImpl : XRPlaneSubsystem
    {
        protected override Provider CreateProvider() => new TestProvider();

        class TestProvider : Provider
        {
            public override TrackableChanges<BoundedPlane> GetChanges(BoundedPlane defaultPlane, Allocator allocator)
            {
                return default;
            }
        }
    }

    [TestFixture]
    public class XRPlaneSubsystemTestFixture
    {
        [Test]
        public void RunningStateTests()
        {
            XRPlaneSubsystem subsystem = new XRPlaneSubsystemImpl();

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