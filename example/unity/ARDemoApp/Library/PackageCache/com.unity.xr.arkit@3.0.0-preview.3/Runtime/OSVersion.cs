using System;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// Represents a version number consisting of major, minor, and point compnents.
    /// Version numbers are often written as <c>Major.Minor.Point</c>.
    /// </summary>
    public struct OSVersion : IEquatable<OSVersion>, IComparable<OSVersion>
    {
        /// <summary>
        /// The major version component.
        /// </summary>
        public int major { get; private set; }

        /// <summary>
        /// The minor version component.
        /// </summary>
        public int minor { get; private set; }

        /// <summary>
        /// The point version component.
        /// </summary>
        public int point { get; private set; }

        /// <summary>
        /// Constructs a new version number.
        /// </summary>
        /// <param name="major">The major version component</param>
        /// <param name="minor">The minor version component</param>
        /// <param name="point">The point version component</param>
        public OSVersion(int major, int minor = 0, int point = 0)
        {
            if (major < 0)
                throw new ArgumentOutOfRangeException("major", "Version component must be greater than or equal to 0.");

            if (minor < 0)
                throw new ArgumentOutOfRangeException("minor", "Version component must be greater than or equal to 0.");

            if (point < 0)
                throw new ArgumentOutOfRangeException("point", "Version component must be greater than or equal to 0.");

            this.major = major;
            this.minor = minor;
            this.point = point;
        }

        /// <summary>
        /// Parses a string which contains a version number of the form X.Y.Z somewhere in the string.
        /// If multiple such substrings exists, the first is used. The parser stops when either
        /// 3 components have been identified, or when less than 3 components have been identified
        /// and the next character is not a period (".") or a digit (0-9). If <paramref name="version"/>
        /// is <c>null</c> or the empty string, this method returns the version 0.0.0
        /// </summary>
        /// <param name="version">The string to parse.</param>
        /// <returns>A new <c>OSVersion</c> representing <paramref name="version"/>.</returns>
        public static OSVersion Parse(string version)
        {
            if (string.IsNullOrEmpty(version))
                return new OSVersion(0);

            int index = IndexOfFirstDigit(version);
            return new OSVersion
            {
                major = ParseNextComponent(version, ref index),
                minor = ParseNextComponent(version, ref index),
                point = ParseNextComponent(version, ref index)
            };
        }

        static int IndexOfFirstDigit(string version)
        {
            for (int index = 0; index < version.Length; ++index)
            {
                var digit = (int)version[index] - 48;
                if (digit >= 0 && digit <= 9)
                    return index;
            }

            // Return one past the end of the string so that
            // ParseNextComponent will early out
            return version.Length;
        }

        static int ParseNextComponent(string version, ref int index)
        {
            const int periodCode = -2;
            int number = 0;
            for (; index < version.Length; ++index)
            {
                var digit = (int)version[index] - 48;
                if (digit == 0 && number == 0)
                {
                    // Ignore leading zeroes
                    continue;
                }
                else if (digit >= 0 && digit <= 9)
                {
                    number = number * 10 + digit;
                }
                else if (digit == periodCode)
                {
                    ++index;
                    break;
                }
                else
                {
                    // This is no longer a version string.
                    index = version.Length;
                    break;
                }
            }

            return number;
        }

        /// <summary>
        /// Generates a hash code suitable for use in a HashSet or Dictionary.
        /// </summary>
        /// <returns>A hash code. The same <c>OSVersion<c/> will produce the same hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = major.GetHashCode();
                hash = hash * 486187739 + minor.GetHashCode();
                hash = hash * 486187739 + point.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// IComparable interface. This is useful for sorting routines.
        /// </summary>
        /// <param name="version">The other version to compare to.</param>
        /// <returns>-1 if this OSVersion is less than <paramref name="version"/>, 0 if they are equal, or 1 if this is greater.</returns>
        public int CompareTo(OSVersion version)
        {
            if (this < version)
                return -1;
            if (this > version)
                return 1;

            return 0;
        }

        /// <summary>
        /// Compares for equality.
        /// </summary>
        /// <param name="obj">The object to compare against.</param>
        /// <returns>Returns false if <paramref name="obj"/> is null or is not of type <c>OSVersion</c>. Otherwise, returns the same value as <see cref="Equals(OSVersion)"/>.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is OSVersion))
                return false;

            return Equals((OSVersion)obj);
        }

        /// <summary>
        /// Compares for equality. The three version components are compared against <paramref name="other"/>'s.
        /// </summary>
        /// <param name="other">The OSVersion to compare for equality.</param>
        /// <returns>True if <paramref name="other"/> has the same <see cref="major"/>, <see cref="minor"/>, and <see cref="point"/> values.</returns>
        public bool Equals(OSVersion other)
        {
            return
                (major == other.major) &&
                (minor == other.minor) &&
                (point == other.point);
        }

        /// <summary>
        /// Tests whether <paramref name="lhs"/> is an earlier version compared to <paramref name="rhs"/>.
        /// </summary>
        /// <param name="lhs">The left-hand side of the comparison.</param>
        /// <param name="rhs">The right-hand side of the comparison.</param>
        /// <returns><c>true</c> if <paramref name="lhs"/> is an earlier version compared to <paramref name="rhs"/>; otherwise, <c>false</c>.</returns>
        public static bool operator <(OSVersion lhs, OSVersion rhs)
        {
            if (lhs.major < rhs.major)
                return true;
            if (lhs.major > rhs.major)
                return false;

            if (lhs.minor < rhs.minor)
                return true;
            if (lhs.minor > rhs.minor)
                return false;

            return lhs.point < rhs.point;
        }

        /// <summary>
        /// Tests whether <paramref name="lhs"/> is a later version compared to <paramref name="rhs"/>.
        /// </summary>
        /// <param name="lhs">The left-hand side of the comparison.</param>
        /// <param name="rhs">The right-hand side of the comparison.</param>
        /// <returns><c>true</c> if <paramref name="lhs"/> is a later version compared to <paramref name="rhs"/>; otherwise, <c>false</c>.</returns>
        public static bool operator >(OSVersion lhs, OSVersion rhs)
        {
            if (lhs.major > rhs.major)
                return true;
            if (lhs.major < rhs.major)
                return false;

            if (lhs.minor > rhs.minor)
                return true;
            if (lhs.minor < rhs.minor)
                return false;

            return lhs.point > rhs.point;
        }

        /// <summary>
        /// Tests whether <paramref name="lhs"/> is the same version as <paramref name="rhs"/>. This is the same as <see cref="Equals(OSVersion)"/>.
        /// </summary>
        /// <param name="lhs">The left-hand side of the comparison.</param>
        /// <param name="rhs">The right-hand side of the comparison.</param>
        /// <returns><c>true</c> if <paramref name="lhs"/> is the same version as <paramref name="rhs"/>; otherwise, <c>false</c>.</returns>
        public static bool operator ==(OSVersion lhs, OSVersion rhs)
        {
            return lhs.Equals(rhs);
        }

        /// <summary>
        /// Tests whether <paramref name="lhs"/> is a different version from <paramref name="rhs"/>.
        /// </summary>
        /// <param name="lhs">The left-hand side of the comparison.</param>
        /// <param name="rhs">The right-hand side of the comparison.</param>
        /// <returns><c>true</c> if <paramref name="lhs"/> is a different version from <paramref name="rhs"/>; otherwise, <c>false</c>.</returns>
        public static bool operator !=(OSVersion lhs, OSVersion rhs)
        {
            return !lhs.Equals(rhs);
        }

        /// <summary>
        /// Tests whether <paramref name="lhs"/> is the same or a later version compared to <paramref name="rhs"/>.
        /// </summary>
        /// <param name="lhs">The left-hand side of the comparison.</param>
        /// <param name="rhs">The right-hand side of the comparison.</param>
        /// <returns><c>true</c> if <paramref name="lhs"/> is the same or a later version compared to <paramref name="rhs"/>; otherwise, <c>false</c>.</returns>
        public static bool operator >=(OSVersion lhs, OSVersion rhs)
        {
            return (lhs > rhs) || (lhs == rhs);
        }

        /// <summary>
        /// Tests whether <paramref name="lhs"/> is the same or an earlier version compared to <paramref name="rhs"/>.
        /// </summary>
        /// <param name="lhs">The left-hand side of the comparison.</param>
        /// <param name="rhs">The right-hand side of the comparison.</param>
        /// <returns><c>true</c> if <paramref name="lhs"/> is the same or an earlier version compared to <paramref name="rhs"/>; otherwise, <c>false</c>.</returns>
        public static bool operator <=(OSVersion lhs, OSVersion rhs)
        {
            return (lhs < rhs) || (lhs == rhs);
        }

        /// <summary>
        /// Generates a string representation of the version.
        /// </summary>
        /// <returns>A string in the form <see cref="major"/>.<see cref="minor"/>.<see cref="point"/>.</returns>
        public override string ToString()
        {
            return $"{major}.{minor}.{point}";
        }
    }
}
