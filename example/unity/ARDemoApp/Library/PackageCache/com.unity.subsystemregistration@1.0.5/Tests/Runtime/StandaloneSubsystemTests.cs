using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using NUnit.Framework;
using Unity.Subsystem.Registration;

using UnityEngine;
using UnityEngine.TestTools;

#if !UNITY_2019_2_OR_NEWER
using UnityEngine.Experimental;
#endif

namespace Unity.Subsystem.Registration
{
    [TestFixture]
    public class StandaloneSubsystemTestFixture
    {
        public class TestSubsystemDescriptor : SubsystemDescriptor<TestSubsystem>
        {
            public bool holdsThings { get; set; }
        }

        public abstract class TestSubsystem : Subsystem<TestSubsystemDescriptor>
        {
            public bool StartCalled { get; set; }
            public bool StopCalled { get; set; }
            public bool DestroyCalled { get; set; }
            public bool IsRunning { get; set; }
            public abstract int GetNumThings();
        }

        public class ConcreteTestSubsystem : TestSubsystem
        {
            public override void Destroy() { DestroyCalled = true; }

            public override void Start() { StartCalled = true; IsRunning = true; }

            public override void Stop() { StopCalled = true; IsRunning = false; }

#if UNITY_2019_2_OR_NEWER
            public override bool running { get { return IsRunning; } }
#else
            public bool running { get { return IsRunning; } }
#endif

            public override int GetNumThings()
            {
                return 66;
            }
        }

        [Test, Order(2)]
        public void UseSubsystemTest()
        {
            List<TestSubsystemDescriptor> descriptors = new List<TestSubsystemDescriptor>();

            SubsystemManager.GetSubsystemDescriptors<TestSubsystemDescriptor>(descriptors);
            Assert.That(1 == descriptors.Count, "TestSubsystemDescriptor not registered.");

            Assert.That("RuntimeTestSubsystem" == descriptors[0].id, "Subsystem ID doesn't match registered ID.");

            TestSubsystem subsystem = descriptors[0].Create();
            Assert.That(null != subsystem, "Create() failed in test subsystem descriptor.");

            // Method call works
            Assert.That(66 == subsystem.GetNumThings(), "Test method on TestSubsystem failed.");
        }

        [Test, Order(1)]
        public void RegisterSubsystemTest()
        {
            TestSubsystemDescriptor descriptor = new TestSubsystemDescriptor();
            List<TestSubsystemDescriptor> descriptors = new List<TestSubsystemDescriptor>();

            SubsystemManager.GetSubsystemDescriptors<TestSubsystemDescriptor>(descriptors);

            Assert.That(0 == descriptors.Count, "TestSubsystemDescriptor already registered.");
            
            // Populate the descriptor object
            descriptor.holdsThings = true;
            descriptor.id = "RuntimeTestSubsystem";
            descriptor.subsystemImplementationType = typeof(ConcreteTestSubsystem);

            // Register the descriptor
            Assert.That(true == SubsystemRegistration.CreateDescriptor(descriptor), "Descriptor not added.");
            Assert.That(false == SubsystemRegistration.CreateDescriptor(descriptor), "Descriptor added twice.");

            SubsystemManager.GetSubsystemDescriptors<TestSubsystemDescriptor>(descriptors);
            Assert.That(1 == descriptors.Count, "TestSubsystemDescriptor not registered.");
        }
    }
}