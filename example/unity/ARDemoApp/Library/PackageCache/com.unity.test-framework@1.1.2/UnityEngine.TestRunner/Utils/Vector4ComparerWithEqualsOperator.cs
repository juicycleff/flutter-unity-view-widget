using System.Collections.Generic;

namespace UnityEngine.TestTools.Utils
{
    public class Vector4ComparerWithEqualsOperator : IEqualityComparer<Vector4>
    {
        private static readonly Vector4ComparerWithEqualsOperator m_Instance = new Vector4ComparerWithEqualsOperator();
        public static Vector4ComparerWithEqualsOperator Instance { get { return m_Instance; } }

        private Vector4ComparerWithEqualsOperator() {}

        public bool Equals(Vector4 expected, Vector4 actual)
        {
            return expected == actual;
        }

        public int GetHashCode(Vector4 vec4)
        {
            return 0;
        }
    }
}
