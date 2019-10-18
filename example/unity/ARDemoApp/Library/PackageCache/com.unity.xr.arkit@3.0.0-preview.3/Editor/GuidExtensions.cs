using System;

namespace UnityEditor.XR.ARKit
{
    internal static class GuidExtensions
    {
        /// <summary>
        /// Assumes the guid is an NSUUID and returns a string in the same format as
        /// [NSUUID.UUIDString](https://developer.apple.com/documentation/foundation/nsuuid/1416585-uuidstring)
        /// </summary>
        /// <param name="guid">The guid to convert to a string</param>
        /// <returns>A string representation of the GUID in "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX" format.</returns>
        public static string ToUUIDString(this Guid guid)
        {
            // When converting to a string representation,
            // C# reverses the gender of the last 2 integral components
            // while Core Foundation's NSUUID.UUIDString reverses all 5.
            // We want to generate a string that will match what NSUUID.UUIDString
            // will produce, so we need to reverse the first 3 components.
            var bytes = guid.ToByteArray();
            Array.Reverse(bytes, 0, 4);
            Array.Reverse(bytes, 4, 2);
            Array.Reverse(bytes, 6, 2);
            return new Guid(bytes).ToString("D").ToUpper();
        }
    }
}
