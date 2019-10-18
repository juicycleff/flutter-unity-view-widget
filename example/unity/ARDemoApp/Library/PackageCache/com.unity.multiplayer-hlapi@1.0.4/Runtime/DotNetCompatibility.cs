using System;
using System.Collections.Generic;
using System.Reflection;
using System.Net.Sockets;

namespace UnityEngine.Networking
{
    internal static class DotNetCompatibility
    {
        internal static string GetMethodName(this Delegate func)
        {
            return func.Method.Name;
        }

        internal static Type GetBaseType(this Type type)
        {
            return type.BaseType;
        }

        internal static string GetErrorCode(this SocketException e)
        {
            return e.ErrorCode.ToString();
        }
    }
}
