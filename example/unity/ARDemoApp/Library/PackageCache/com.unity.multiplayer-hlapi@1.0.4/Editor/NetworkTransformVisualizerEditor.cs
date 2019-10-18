using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityEditor
{
    [CustomEditor(typeof(NetworkTransformVisualizer), true)]
    [CanEditMultipleObjects]
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class NetworkTransformVisualizerEditor : NetworkBehaviourInspector
    {
        internal override bool hideScriptField
        {
            get
            {
                return true;
            }
        }
    }
}
