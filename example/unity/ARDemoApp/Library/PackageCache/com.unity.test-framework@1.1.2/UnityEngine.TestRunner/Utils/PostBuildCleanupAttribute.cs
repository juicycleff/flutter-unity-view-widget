using System;

namespace UnityEngine.TestTools
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
    public class PostBuildCleanupAttribute : Attribute
    {
        public PostBuildCleanupAttribute(Type targetClass)
        {
            TargetClass = targetClass;
        }

        public PostBuildCleanupAttribute(string targetClassName)
        {
            TargetClass = AttributeHelper.GetTargetClassFromName(targetClassName, typeof(IPostBuildCleanup));
        }

        internal Type TargetClass { get; private set; }
    }
}
