using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace UnityEngine.TestTools
{
    public class ConditionalIgnoreAttribute : NUnitAttribute, IApplyToTest
    {
        string m_ConditionKey;
        string m_IgnoreReason;

        public ConditionalIgnoreAttribute(string conditionKey, string ignoreReason)
        {
            m_ConditionKey = conditionKey;
            m_IgnoreReason = ignoreReason;
        }

        public void ApplyToTest(Test test)
        {
            var key = m_ConditionKey.ToLowerInvariant();
            if (m_ConditionMap.ContainsKey(key) && m_ConditionMap[key])
            {
                test.RunState = RunState.Ignored;
                string skipReason = string.Format(m_IgnoreReason);
                test.Properties.Add(PropertyNames.SkipReason, skipReason);
            }
        }

        static Dictionary<string, bool> m_ConditionMap = new Dictionary<string, bool>();
        public static void AddConditionalIgnoreMapping(string key, bool value)
        {
            m_ConditionMap.Add(key.ToLowerInvariant(), value);
        }
    }
}