using NUnit.Framework;
using Unity.Collections;

namespace UnityEngine.XR.ARSubsystems.Tests
{
    public class XRReferencePointSubsystemImpl : XRReferencePointSubsystem
    {
        protected override Provider CreateProvider() => new TestProvider();

        class TestProvider : Provider
        {
            public override TrackableChanges<XRReferencePoint> GetChanges(XRReferencePoint defaultReferencePoint, Allocator allocator) => default;
        }
    }

    [TestFixture]
    public class XRReferencePointSubsystemTestFixture
    {
        [Test]
        public void RunningStateTests()
        {
            XRReferencePointSubsystem subsystem = new XRReferencePointSubsystemImpl();

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