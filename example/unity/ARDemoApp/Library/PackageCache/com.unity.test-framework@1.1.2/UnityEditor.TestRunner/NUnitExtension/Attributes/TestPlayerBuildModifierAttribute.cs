using System;

namespace UnityEditor.TestTools
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class TestPlayerBuildModifierAttribute : Attribute
    {
        private Type m_Type;
        public TestPlayerBuildModifierAttribute(Type type)
        {
            var interfaceType = typeof(ITestPlayerBuildModifier);
            if (!interfaceType.IsAssignableFrom(type))
            {
                throw new ArgumentException(string.Format("Type provided to {0} does not implement {1}", this.GetType().Name, interfaceType.Name));
            }
            m_Type = type;
        }

        internal ITestPlayerBuildModifier ConstructModifier()
        {
            return Activator.CreateInstance(m_Type) as ITestPlayerBuildModifier;
        }
    }
}

