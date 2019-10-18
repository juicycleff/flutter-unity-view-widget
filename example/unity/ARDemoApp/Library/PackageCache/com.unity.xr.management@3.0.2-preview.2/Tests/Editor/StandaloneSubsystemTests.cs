using System;
using System.Collections;

using NUnit.Framework;

using UnityEditor;

using UnityEngine;
using UnityEngine.TestTools;

using UnityEngine.XR.Management.Tests.Standalone;
using UnityEngine.XR.Management.Tests.Standalone.Providing;

namespace UnityEditor.XR.Management.Tests
{
    class EditorTests
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            StandaloneSubsystemParams parms = new StandaloneSubsystemParams("Standalone Subsystem", typeof(StandaloneSubsystem));
            StandaloneSubsystemDescriptor.Create(parms);
        }

        StandaloneLoader loader;

        [SetUp]
        public void SetUp()
        {
            loader = ScriptableObject.CreateInstance<StandaloneLoader>() as StandaloneLoader;
        }

        [TearDown]
        public void TearDown()
        {
            UnityEngine.Object.DestroyImmediate(loader);
            loader = null;
        }

        [Test]
        public void StandaloneLoaderCreateTest()
        {
            Assert.IsTrue(loader.Initialize());
        }

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest]
        public IEnumerator StandaloneLoaderLifecycleTest()
        {
            Assert.IsTrue(loader.Initialize());
            yield return null;

            Assert.IsTrue(loader.Start());
            Assert.IsTrue(loader.started);
            yield return null;


            Assert.IsTrue(loader.Stop());
            Assert.IsTrue(loader.stopped);
            yield return null;


            Assert.IsTrue(loader.Deinitialize());
            Assert.IsTrue(loader.deInitialized);
            yield return null;

        }
    }
}
