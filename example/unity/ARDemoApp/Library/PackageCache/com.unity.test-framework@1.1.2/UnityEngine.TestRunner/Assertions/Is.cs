namespace UnityEngine.TestTools.Constraints
{
    public class Is : NUnit.Framework.Is
    {
        public static AllocatingGCMemoryConstraint AllocatingGCMemory()
        {
            return new AllocatingGCMemoryConstraint();
        }
    }
}
