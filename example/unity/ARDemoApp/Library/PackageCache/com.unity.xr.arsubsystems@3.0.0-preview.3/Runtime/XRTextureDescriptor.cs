using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Encapsulates a native texture object and includes various metadata about the texture.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct XRTextureDescriptor : IEquatable<XRTextureDescriptor>
    {
        /// <summary>
        /// A pointer to the native texture object.
        /// </summary>
        /// <value>
        /// A pointer to the native texture object.
        /// </value>
        public IntPtr nativeTexture
        {
            get { return m_NativeTexture; }
            private set { m_NativeTexture = value; }
        }
        IntPtr m_NativeTexture;

        /// <summary>
        /// Specifies the width dimension of the native texture object.
        /// </summary>
        /// <value>
        /// The width of the native texture object.
        /// </value>
        public int width
        {
            get { return m_Width; }
            private set { m_Width = value; }
        }
        int m_Width;

        /// <summary>
        /// Specifies the height dimension of the native texture object.
        /// </summary>
        /// <value>
        /// The height of the native texture object.
        /// </value>
        public int height
        {
            get { return m_Height; }
            private set { m_Height = value; }
        }
        int m_Height;

        /// <summary>
        /// Specifies the number of mipmap levels in the native texture object.
        /// </summary>
        /// <value>
        /// The number of mipmap levels in the native texture object.
        /// </value>
        public int mipmapCount
        {
            get { return m_MipmapCount; }
            private set { m_MipmapCount = value; }
        }
        int m_MipmapCount;

        /// <summary>
        /// Specifies the texture format of the native texture object.
        /// </summary>
        /// <value>
        /// The format of the native texture object.
        /// </value>
        public TextureFormat format
        {
            get { return m_Format; }
            private set { m_Format = value; }
        }
        TextureFormat m_Format;

        /// <summary>
        /// Specifies the unique shader property name ID for the material shader texture.
        /// </summary>
        /// <value>
        /// The unique shader property name ID for the material shader texture.
        /// </value>
        /// <remarks>
        /// Use the static method <c>Shader.PropertyToID(string name)</c> to get the unique identifier.
        /// </remarks>
        public int propertyNameId
        {
            get { return m_PropertyNameId; }
            private set { m_PropertyNameId = value; }
        }
        int m_PropertyNameId;

        /// <summary>
        /// Determines whether the texture data references a valid texture object with positive width and height.
        /// </summary>
        /// <value>
        /// <c>true</c> if the texture data references a valid texture object with positive width and height.
        /// Otherwise, <c>false</c>.
        /// </value>
        public bool valid
        {
            get { return (m_NativeTexture != IntPtr.Zero) && (m_Width > 0) && (m_Height > 0); }
        }

        /// <summary>
        /// Determines whether the given texture descriptor has identical texture metadata (dimension, mipmap count,
        /// and format).
        /// </summary>
        /// <param name="other">The given texture descriptor with which to compare.</param>
        /// <returns>
        /// <c>true</c> if the texture metadata (dimension, mipmap count, and format) are identical between  the
        /// current and other texture descriptors. Otherwise, <c>false</c>.
        /// </returns>
        public bool hasIdenticalTextureMetadata(XRTextureDescriptor other)
        {
            return
                m_Width.Equals(other.m_Width) &&
                m_Height.Equals(other.m_Height) &&
                m_MipmapCount.Equals(other.m_MipmapCount) &&
                (m_Format == other.m_Format);
        }

        /// <summary>
        /// Reset the texture descriptor back to default values.
        /// </summary>
        public void Reset()
        {
            m_NativeTexture = IntPtr.Zero;
            m_Width = 0;
            m_Height = 0;
            m_MipmapCount = 0;
            m_Format = (TextureFormat)0;
            m_PropertyNameId = 0;
        }

        public bool Equals(XRTextureDescriptor other)
        {
            return
                hasIdenticalTextureMetadata(other) &&
                m_PropertyNameId.Equals(other.m_PropertyNameId) &&
                (m_NativeTexture == other.m_NativeTexture);
        }

        public override bool Equals(System.Object obj)
        {
            return ((obj is XRTextureDescriptor) && Equals((XRTextureDescriptor)obj));
        }

        public static bool operator ==(XRTextureDescriptor lhs, XRTextureDescriptor rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(XRTextureDescriptor lhs, XRTextureDescriptor rhs)
        {
            return !lhs.Equals(rhs);
        }

        public override int GetHashCode()
        {
            int hashCode = 486187739;
            unchecked
            {
                hashCode = (hashCode * 486187739) + m_NativeTexture.GetHashCode();
                hashCode = (hashCode * 486187739) + m_Width.GetHashCode();
                hashCode = (hashCode * 486187739) + m_Height.GetHashCode();
                hashCode = (hashCode * 486187739) + m_MipmapCount.GetHashCode();
                hashCode = (hashCode * 486187739) + ((int)m_Format).GetHashCode();
                hashCode = (hashCode * 486187739) + m_PropertyNameId.GetHashCode();
            }
            return hashCode;
        }

        public override string ToString()
        {
            return string.Format("0x{0} {1}x{2}-{3} format:{4} propertyNameId:{5}", m_NativeTexture.ToString("X16"),
                                 m_Width.ToString(), m_Height.ToString(), m_MipmapCount.ToString(),
                                 m_Format.ToString(), m_PropertyNameId.ToString());
        }
    }
}
