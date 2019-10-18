using System;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Serialization;

namespace UnityEditor.XR.Management
{
    [Serializable]
    internal class CuratedInfo
    {
        [SerializeField]
        internal string MenuTitle = "";
        [SerializeField]
        internal string PackageName = "";
        [SerializeField]
        internal string LoaderTypeInfo = "";
    }

    internal sealed class XRCuratedPackages : ScriptableObject
    {
        [SerializeField]
        internal CuratedInfo[] CuratedPackages = null;
    }
}
