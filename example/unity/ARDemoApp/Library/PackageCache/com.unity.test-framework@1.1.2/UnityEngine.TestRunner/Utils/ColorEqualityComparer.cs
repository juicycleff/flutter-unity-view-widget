using System.Collections.Generic;

namespace UnityEngine.TestTools.Utils
{
    public class ColorEqualityComparer : IEqualityComparer<Color>
    {
        private const float k_DefaultError = 0.01f;
        private readonly float AllowedError;


        private static readonly ColorEqualityComparer m_Instance = new ColorEqualityComparer();
        public static ColorEqualityComparer Instance { get { return m_Instance; } }

        private ColorEqualityComparer() : this(k_DefaultError)
        {
        }

        public ColorEqualityComparer(float error)
        {
            this.AllowedError = error;
        }

        public bool Equals(Color expected, Color actual)
        {
            return Utils.AreFloatsEqualAbsoluteError(expected.r, actual.r, AllowedError) &&
                Utils.AreFloatsEqualAbsoluteError(expected.g, actual.g, AllowedError) &&
                Utils.AreFloatsEqualAbsoluteError(expected.b, actual.b, AllowedError) &&
                Utils.AreFloatsEqualAbsoluteError(expected.a, actual.a, AllowedError);
        }

        public int GetHashCode(Color color)
        {
            return 0;
        }
    }
}
