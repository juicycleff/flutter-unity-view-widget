using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.TestTools.TestRunner;

namespace UnityEngine.TestTools.Logging
{
    sealed class LogScope : IDisposable
    {
        static List<LogScope> s_ActiveScopes = new List<LogScope>();
        
        readonly object m_Lock = new object();
        bool m_Disposed;
        bool m_NeedToProcessLogs;

        public Queue<LogMatch> ExpectedLogs { get; set; }
        public List<LogEvent> AllLogs { get; }
        public List<LogEvent> FailingLogs { get; }
        public bool IgnoreFailingMessages { get; set; }
        public bool IsNUnitException { get; private set; }
        public bool IsNUnitSuccessException { get; private set; }
        public bool IsNUnitInconclusiveException { get; private set; }
        public bool IsNUnitIgnoreException { get; private set; }
        public string NUnitExceptionMessage { get; private set; }
        
        public static LogScope Current
        {
            get
            {
                if (s_ActiveScopes.Count == 0)
                    throw new InvalidOperationException("No log scope is available");
                return s_ActiveScopes[0];
            }
        }

        public static bool HasCurrentLogScope()
        {
            return s_ActiveScopes.Count > 0;
        }

        public LogScope()
        {
            AllLogs = new List<LogEvent>();
            FailingLogs = new List<LogEvent>();
            ExpectedLogs = new Queue<LogMatch>();
            IgnoreFailingMessages = false;
            Activate();
        }

        void Activate()
        {
            s_ActiveScopes.Insert(0, this);
            RegisterScope(this);
            Application.logMessageReceivedThreaded -= AddLog;
            Application.logMessageReceivedThreaded += AddLog;
        }

        void Deactivate()
        {
            Application.logMessageReceivedThreaded -= AddLog;
            s_ActiveScopes.Remove(this);
            UnregisterScope(this);
        }

        static void RegisterScope(LogScope logScope)
        {
            Application.logMessageReceivedThreaded += logScope.AddLog;
        }

        static void UnregisterScope(LogScope logScope)
        {
            Application.logMessageReceivedThreaded -= logScope.AddLog;
        }

        public void AddLog(string message, string stacktrace, LogType type)
        {
            lock (m_Lock)
            {
                m_NeedToProcessLogs = true;
                var log = new LogEvent
                {
                    LogType = type,
                    Message = message,
                    StackTrace = stacktrace,
                };

                AllLogs.Add(log);

                if (IsNUnitResultStateException(stacktrace, type))
                {
                    if (message.StartsWith("SuccessException"))
                    {
                        IsNUnitException = true;
                        IsNUnitSuccessException = true;
                        if (message.StartsWith("SuccessException: "))
                        {
                            NUnitExceptionMessage = message.Substring("SuccessException: ".Length);
                            return;
                        }
                    }
                    else if (message.StartsWith("InconclusiveException"))
                    {
                        IsNUnitException = true;
                        IsNUnitInconclusiveException = true;
                        if (message.StartsWith("InconclusiveException: "))
                        {
                            NUnitExceptionMessage = message.Substring("InconclusiveException: ".Length);
                            return;
                        }
                    }
                    else if (message.StartsWith("IgnoreException"))
                    {
                        IsNUnitException = true;
                        IsNUnitIgnoreException = true;
                        if (message.StartsWith("IgnoreException: "))
                        {
                            NUnitExceptionMessage = message.Substring("IgnoreException: ".Length);
                            return;
                        }
                    }
                }

                if (IsFailingLog(type) && !IgnoreFailingMessages)
                {
                    FailingLogs.Add(log);
                }
            }
        }

        static bool IsNUnitResultStateException(string stacktrace, LogType logType)
        {
            if (logType != LogType.Exception)
                return false;

            return string.IsNullOrEmpty(stacktrace) || stacktrace.StartsWith("NUnit.Framework.Assert.");
        }

        static bool IsFailingLog(LogType type)
        {
            switch (type)
            {
                case LogType.Assert:
                case LogType.Error:
                case LogType.Exception:
                    return true;
                default:
                    return false;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (m_Disposed)
            {
                return;
            }

            m_Disposed = true;

            if (disposing)
            {
                Deactivate();
            }
        }

        public bool AnyFailingLogs()
        {
            ProcessExpectedLogs();
            return FailingLogs.Any();
        }

        public void ProcessExpectedLogs()
        {
            lock (m_Lock)
            {
                if (!m_NeedToProcessLogs || !ExpectedLogs.Any())
                    return;

                LogMatch expectedLog = null;
                foreach (var logEvent in AllLogs)
                {
                    if (!ExpectedLogs.Any())
                        break;
                    if (expectedLog == null && ExpectedLogs.Any())
                        expectedLog = ExpectedLogs.Peek();

                    if (expectedLog != null && expectedLog.Matches(logEvent))
                    {
                        ExpectedLogs.Dequeue();
                        logEvent.IsHandled = true;
                        if (FailingLogs.Any(expectedLog.Matches))
                        {
                            var failingLog = FailingLogs.First(expectedLog.Matches);
                            FailingLogs.Remove(failingLog);
                        }
                        expectedLog = null;
                    }
                }
                m_NeedToProcessLogs = false;
            }
        }

        public void NoUnexpectedReceived()
        {
            lock (m_Lock)
            {
                ProcessExpectedLogs();

                var unhandledLog = AllLogs.FirstOrDefault(x => !x.IsHandled);
                if (unhandledLog != null)
                {
                    throw new UnhandledLogMessageException(unhandledLog);
                }
            }
        }
    }
}
