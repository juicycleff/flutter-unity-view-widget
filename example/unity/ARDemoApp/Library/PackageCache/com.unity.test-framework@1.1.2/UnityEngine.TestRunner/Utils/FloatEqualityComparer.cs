using System.Collections.Generic;

namespace UnityEngine.TestTools.Utils
{
    public class FloatEqualityComparer : IEqualityComparer<float>
    {
        private const float k_DefaultError = 0.0001f;
        private readonly float AllowedError;

        private static readonly  FloatEqualityComparer m_Instance = new FloatEqualityComparer();
        public static FloatEqualityComparer Instance { get { return m_Instance; } }

        private FloatEqualityComparer() : this(k_DefaultError) {}

        public FloatEqualityComparer(float allowedError)
        {
            this.AllowedError = allowedError;
        }

        public bool Equals(float expected, float actual)
        {
            return Utils.AreFloatsEqual(expected, actual, AllowedError);
        }

        public int GetHashCode(float value)
        {
            return 0;
        }
    }
}
