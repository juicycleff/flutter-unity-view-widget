using System.Collections.Generic;

namespace UnityEngine.TestTools.Utils
{
    public class Vector3ComparerWithEqualsOperator : IEqualityComparer<Vector3>
    {
        private static readonly Vector3ComparerWithEqualsOperator m_Instance = new Vector3ComparerWithEqualsOperator();
        public static Vector3ComparerWithEqualsOperator Instance { get { return m_Instance; } }

        private Vector3ComparerWithEqualsOperator() {}

        public bool Equals(Vector3 expected, Vector3 actual)
        {
            return expected == actual;
        }

        public int GetHashCode(Vector3 vec3)
        {
            return 0;
        }
    }
}
