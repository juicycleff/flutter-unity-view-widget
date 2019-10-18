using System;
using System.Runtime.InteropServices;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// Enum values that represent face action units that affect the expression on the face
    /// </summary>
    public enum ARKitBlendShapeLocation
    {
        BrowDownLeft        ,
        BrowDownRight       ,
        BrowInnerUp         ,
        BrowOuterUpLeft     ,
        BrowOuterUpRight    ,
        CheekPuff           ,
        CheekSquintLeft     ,
        CheekSquintRight    ,
        EyeBlinkLeft        ,
        EyeBlinkRight       ,
        EyeLookDownLeft     ,
        EyeLookDownRight    ,
        EyeLookInLeft       ,
        EyeLookInRight      ,
        EyeLookOutLeft      ,
        EyeLookOutRight     ,
        EyeLookUpLeft       ,
        EyeLookUpRight      ,
        EyeSquintLeft       ,
        EyeSquintRight      ,
        EyeWideLeft         ,
        EyeWideRight        ,
        JawForward          ,
        JawLeft             ,
        JawOpen             ,
        JawRight            ,
        MouthClose          ,
        MouthDimpleLeft     ,
        MouthDimpleRight    ,
        MouthFrownLeft      ,
        MouthFrownRight     ,
        MouthFunnel         ,
        MouthLeft           ,
        MouthLowerDownLeft  ,
        MouthLowerDownRight ,
        MouthPressLeft      ,
        MouthPressRight     ,
        MouthPucker         ,
        MouthRight          ,
        MouthRollLower      ,
        MouthRollUpper      ,
        MouthShrugLower     ,
        MouthShrugUpper     ,
        MouthSmileLeft      ,
        MouthSmileRight     ,
        MouthStretchLeft    ,
        MouthStretchRight   ,
        MouthUpperUpLeft    ,
        MouthUpperUpRight   ,
        NoseSneerLeft       ,
        NoseSneerRight      ,
        TongueOut
    }

    /// <summary>
    /// An entry that specifies how much of a specific <see cref="XRArkitBlendShapeLocation"/> is present in the current expression on the face.
    /// </summary>
    /// <remarks>
    /// You get a list of these for every expression a face makes.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct ARKitBlendShapeCoefficient : IEquatable<ARKitBlendShapeCoefficient>
    {
        // Fields to marshall/serialize from native code
        ARKitBlendShapeLocation m_BlendShapeLocation;
        float m_Coefficient;

        /// <summary>
        /// The specific <see cref="ARKitBlendShapeLocation"/> being examined.
        /// </summary>
        public ARKitBlendShapeLocation blendShapeLocation
        {
            get { return m_BlendShapeLocation; }
        }

        /// <summary>
        /// A value from 0.0 to 1.0 that specifies how active the associated <see cref="ARKitBlendShapeLocation"/> is in this expression.
        /// </summary>
        public float coefficient
        {
            get { return m_Coefficient; }
        }

        public bool Equals(ARKitBlendShapeCoefficient other)
        {
            return
                (blendShapeLocation == other.blendShapeLocation) &&
                coefficient.Equals(other.coefficient);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return (obj is ARKitBlendShapeCoefficient) && Equals((ARKitBlendShapeCoefficient)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = ((int)m_BlendShapeLocation).GetHashCode();
                hash = hash * 486187739 + coefficient.GetHashCode();
                return hash;
            }
        }

        public static bool operator==(ARKitBlendShapeCoefficient left, ARKitBlendShapeCoefficient right)
        {
            return left.Equals(right);
        }

        public static bool operator!=(ARKitBlendShapeCoefficient left, ARKitBlendShapeCoefficient right)
        {
            return !left.Equals(right);
        }
    }
}
