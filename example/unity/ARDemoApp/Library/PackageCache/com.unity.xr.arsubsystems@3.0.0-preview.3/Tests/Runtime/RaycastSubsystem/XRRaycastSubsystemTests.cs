using NUnit.Framework;

namespace UnityEngine.XR.ARSubsystems.Tests
{
    public class XRRaycastSubsystemImpl : XRRaycastSubsystem
    {
        protected override Provider CreateProvider() => new Provider();
    }

    [TestFixture]
    public class XRRaycastSubsystemTestFixture
    {
         [Test]
        public void RunningStateTests()
        {
            XRRaycastSubsystem subsystem = new XRRaycastSubsystemImpl();

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