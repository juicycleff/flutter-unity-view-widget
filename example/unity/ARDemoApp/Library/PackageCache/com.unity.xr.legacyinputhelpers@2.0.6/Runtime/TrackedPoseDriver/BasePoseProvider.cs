using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;

#if ENABLE_VR
using UnityEngine.XR;
#endif

namespace UnityEngine.Experimental.XR.Interaction
{
    /// <summary>
    /// The BasePoseProvider type is used as the base interface for all "Pose Providers"
    /// Implementing this abstract class will allow the Pose Provider to be linked to a Tracked Pose Driver.
    /// </summary>
    [Serializable]
    public abstract class BasePoseProvider : MonoBehaviour
    {
        /// <summary> Gets the Pose value from the Pose Provider. returns NoData as this is a default implementation. Specalizations shoudl return the correct bitflags relating to the Pose data they are returning</summary>
        public virtual PoseDataFlags GetPoseFromProvider(out Pose output)
        {          
            output = Pose.identity;
            return PoseDataFlags.NoData;
        }
    }
}
