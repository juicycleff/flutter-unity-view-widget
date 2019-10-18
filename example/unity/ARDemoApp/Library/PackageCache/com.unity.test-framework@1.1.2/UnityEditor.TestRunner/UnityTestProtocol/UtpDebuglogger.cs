using UnityEngine;

namespace UnityEditor.TestTools.TestRunner.UnityTestProtocol
{
    class UtpDebugLogger : IUtpLogger
    {
        public void Log(Message msg)
        {
            var msgJson = JsonUtility.ToJson(msg);
            Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, "\n##utp:{0}", msgJson);
        }
    }
}
