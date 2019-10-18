using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.XR.ARKit.FaceTracking.Tests
{
    [TestFixture]
    public class RuntimeExampleTest
    {
        [Test]
        public void PlayModeSampleTestSimplePasses()
        {
            // Use the Assert class to test conditions.
        }

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest]
        public IEnumerator PlayModeSampleTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // yield to skip a frame
            yield return null;
        }
    }
}
