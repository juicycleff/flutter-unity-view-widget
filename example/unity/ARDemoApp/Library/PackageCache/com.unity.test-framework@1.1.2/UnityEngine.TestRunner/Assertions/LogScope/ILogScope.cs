using System;
using System.Collections.Generic;

namespace UnityEngine.TestTools.Logging
{
    internal interface ILogScope : IDisposable
    {
        List<LogEvent> LogEvents { get; }
    }
}
