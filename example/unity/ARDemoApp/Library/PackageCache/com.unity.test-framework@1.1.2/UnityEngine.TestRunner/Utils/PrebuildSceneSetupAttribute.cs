using System;

namespace UnityEngine.TestTools
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
    public class PrebuildSetupAttribute : Attribute
    {
        public PrebuildSetupAttribute(Type targetClass)
        {
            TargetClass = targetClass;
        }

        public PrebuildSetupAttribute(string targetClassName)
        {
            TargetClass = AttributeHelper.GetTargetClassFromName(targetClassName, typeof(IPrebuildSetup));
        }

        internal Type TargetClass { get; private set; }
    }
}
