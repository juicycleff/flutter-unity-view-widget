using System;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEngine.Profiling;

namespace UnityEngine.TestTools.Constraints
{
    public class AllocatingGCMemoryConstraint : Constraint
    {
        private class AllocatingGCMemoryResult : ConstraintResult
        {
            private readonly int diff;
            public AllocatingGCMemoryResult(IConstraint constraint, object actualValue, int diff) : base(constraint, actualValue, diff > 0)
            {
                this.diff = diff;
            }

            public override void WriteMessageTo(MessageWriter writer)
            {
                if (diff == 0)
                    writer.WriteMessageLine("The provided delegate did not make any GC allocations.");
                else
                    writer.WriteMessageLine("The provided delegate made {0} GC allocation(s).", diff);
            }
        }

        private ConstraintResult ApplyTo(Action action, object original)
        {
            var recorder = Recorder.Get("GC.Alloc");

            // The recorder was created enabled, which means it captured the creation of the Recorder object itself, etc.
            // Disabling it flushes its data, so that we can retrieve the sample block count and have it correctly account
            // for these initial allocations.
            recorder.enabled = false;

#if !UNITY_WEBGL
            recorder.FilterToCurrentThread();
#endif

            recorder.enabled = true;

            try
            {
                action();
            }
            finally
            {
                recorder.enabled = false;
#if !UNITY_WEBGL
                recorder.CollectFromAllThreads();
#endif
            }

            return new AllocatingGCMemoryResult(this, original, recorder.sampleBlockCount);
        }

        public override ConstraintResult ApplyTo(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException();

            TestDelegate d = obj as TestDelegate;
            if (d == null)
                throw new ArgumentException(string.Format("The actual value must be a TestDelegate but was {0}",
                    obj.GetType()));

            return ApplyTo(() => d.Invoke(), obj);
        }

        public override ConstraintResult ApplyTo<TActual>(ActualValueDelegate<TActual> del)
        {
            if (del == null)
                throw new ArgumentNullException();

            return ApplyTo(() => del.Invoke(), del);
        }

        public override string Description
        {
            get { return "allocates GC memory"; }
        }
    }
}
