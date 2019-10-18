using System.Collections.Generic;


namespace UnityEngine.TestTools.Utils
{
    public class Vector3EqualityComparer : IEqualityComparer<Vector3>
    {
        private const float k_DefaultError = 0.0001f;
        private readonly float AllowedError;

        private static readonly Vector3EqualityComparer m_Instance = new Vector3EqualityComparer();
        public static Vector3EqualityComparer Instance { get { return m_Instance; } }

        private Vector3EqualityComparer() : this(k_DefaultError) {}
        public Vector3EqualityComparer(float allowedError)
        {
            this.AllowedError = allowedError;
        }

        public bool Equals(Vector3 expected, Vector3 actual)
        {
            return Utils.AreFloatsEqual(expected.x, actual.x, AllowedError) &&
                Utils.AreFloatsEqual(expected.y, actual.y, AllowedError) &&
                Utils.AreFloatsEqual(expected.z, actual.z, AllowedError);
        }

        public int GetHashCode(Vector3 vec3)
        {
            return 0;
        }
    }
}
