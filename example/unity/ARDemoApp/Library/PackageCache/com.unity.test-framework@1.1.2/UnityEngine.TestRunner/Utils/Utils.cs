using System;

namespace UnityEngine.TestTools.Utils
{
    public static class Utils
    {
        public static bool AreFloatsEqual(float expected, float actual, float epsilon)
        {
            // special case for infinity
            if (expected == Mathf.Infinity || actual == Mathf.Infinity || expected == Mathf.NegativeInfinity || actual == Mathf.NegativeInfinity)
                return expected == actual;

            // we cover both relative and absolute tolerance with this check
            // which is better than just relative in case of small (in abs value) args
            // please note that "usually" approximation is used [i.e. abs(x)+abs(y)+1]
            // but we speak about test code so we dont care that much about performance
            // but we do care about checks being more precise
            return Math.Abs(actual - expected) <= epsilon * Mathf.Max(Mathf.Max(Mathf.Abs(actual), Mathf.Abs(expected)), 1.0f);
        }

        public static bool AreFloatsEqualAbsoluteError(float expected, float actual, float allowedAbsoluteError)
        {
            return Math.Abs(actual - expected) <= allowedAbsoluteError;
        }

        /// <summary>
        /// Analogous to GameObject.CreatePrimitive, but creates a primitive mesh renderer with fast shader instead of a default builtin shader.
        /// Optimized for testing performance.
        /// </summary>
        /// <returns>A GameObject with primitive mesh renderer and collider.</returns>
        public static GameObject CreatePrimitive(PrimitiveType type)
        {
            var prim = GameObject.CreatePrimitive(type);
            var renderer = prim.GetComponent<Renderer>();
            if (renderer)
                renderer.sharedMaterial = new Material(Shader.Find("VertexLit"));
            return prim;
        }
    }
}
