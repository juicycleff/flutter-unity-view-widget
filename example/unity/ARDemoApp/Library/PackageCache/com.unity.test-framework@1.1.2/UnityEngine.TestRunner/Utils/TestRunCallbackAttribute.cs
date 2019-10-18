using System;

namespace UnityEngine.TestRunner
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class TestRunCallbackAttribute : Attribute
    {
        private Type m_Type;
        public TestRunCallbackAttribute(Type type)
        {
            var interfaceType = typeof(ITestRunCallback);
            if (!interfaceType.IsAssignableFrom(type))
            {
                throw new ArgumentException(string.Format("Type provided to {0} does not implement {1}", this.GetType().Name, interfaceType.Name));
            }
            m_Type = type;
        }

        internal ITestRunCallback ConstructCallback()
        {
            return Activator.CreateInstance(m_Type) as ITestRunCallback;
        }
    }
}
