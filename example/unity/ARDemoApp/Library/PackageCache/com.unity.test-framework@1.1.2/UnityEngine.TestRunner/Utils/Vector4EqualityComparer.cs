using System.Collections.Generic;

namespace UnityEngine.TestTools.Utils
{
    public class Vector4EqualityComparer : IEqualityComparer<Vector4>
    {
        private const float k_DefaultError = 0.0001f;
        private readonly float AllowedError;

        private static readonly Vector4EqualityComparer m_Instance = new Vector4EqualityComparer();
        public static Vector4EqualityComparer Instance { get { return m_Instance; } }

        private Vector4EqualityComparer() : this(k_DefaultError) {}
        public Vector4EqualityComparer(float allowedError)
        {
            this.AllowedError = allowedError;
        }

        public bool Equals(Vector4 expected, Vector4 actual)
        {
            return Utils.AreFloatsEqual(expected.x, actual.x, AllowedError) &&
                Utils.AreFloatsEqual(expected.y, actual.y, AllowedError) &&
                Utils.AreFloatsEqual(expected.z, actual.z, AllowedError) &&
                Utils.AreFloatsEqual(expected.w, actual.w, AllowedError);
        }

        public int GetHashCode(Vector4 vec4)
        {
            return 0;
        }
    }
}
