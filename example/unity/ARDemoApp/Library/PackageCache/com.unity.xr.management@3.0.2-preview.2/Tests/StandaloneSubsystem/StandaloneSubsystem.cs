using System;

using UnityEngine;
using UnityEngine.XR;

#if !UNITY_2019_3_OR_NEWER
using UnityEngine.Experimental;
using UnityEngine.Experimental.XR;
#endif

namespace UnityEngine.XR.Management.Tests.Standalone
{
    public class StandaloneSubsystem : Subsystem
    {
        private bool isRunning = false;
        public override bool running
        {
            get
            {
                return isRunning;
            }
        }

        public event Action startCalled;
        public event Action stopCalled;
        public event Action destroyCalled;


        public override void Start()
        {
            isRunning = true;
            if (startCalled != null)
                startCalled.Invoke();
        }

        public override void Stop()
        {
            isRunning = false;
            if (stopCalled != null)
                stopCalled.Invoke();
        }

        public override void Destroy()
        {
            isRunning = false;
            if (destroyCalled != null)
                destroyCalled.Invoke();
        }
    }
}
