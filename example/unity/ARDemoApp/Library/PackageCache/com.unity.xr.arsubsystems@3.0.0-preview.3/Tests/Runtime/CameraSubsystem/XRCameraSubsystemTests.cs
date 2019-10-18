using NUnit.Framework;

namespace UnityEngine.XR.ARSubsystems.Tests
{
    public class XRCameraSubsystemImpl : XRCameraSubsystem
    {
        protected override Provider CreateProvider() => new Provider();
    }

    [TestFixture]
    public class XRCameraSubsystemTestFixture
    {
        [Test]
        public void RunningStateTests()
        {
            XRCameraSubsystem subsystem = new XRCameraSubsystemImpl();

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
