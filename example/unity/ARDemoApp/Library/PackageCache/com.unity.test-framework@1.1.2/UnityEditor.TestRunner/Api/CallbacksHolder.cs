using System.Collections.Generic;
using System.Linq;

namespace UnityEditor.TestTools.TestRunner.Api
{
    internal class CallbacksHolder : ScriptableSingleton<CallbacksHolder>, ICallbacksHolder
    {
        private List<CallbackWithPriority> m_Callbacks = new List<CallbackWithPriority>();
        public void Add(ICallbacks callback, int priority)
        {
            m_Callbacks.Add(new CallbackWithPriority(callback, priority));
        }

        public void Remove(ICallbacks callback)
        {
            m_Callbacks.RemoveAll(callbackWithPriority => callbackWithPriority.Callback == callback);
        }

        public ICallbacks[] GetAll()
        {
            return m_Callbacks.OrderByDescending(callback => callback.Priority).Select(callback => callback.Callback).ToArray();
        }

        public void Clear()
        {
            m_Callbacks.Clear();
        }

        private struct CallbackWithPriority
        {
            public ICallbacks Callback;
            public int Priority;
            public CallbackWithPriority(ICallbacks callback, int priority)
            {
                Callback = callback;
                Priority = priority;
            }
        }
    }
}
