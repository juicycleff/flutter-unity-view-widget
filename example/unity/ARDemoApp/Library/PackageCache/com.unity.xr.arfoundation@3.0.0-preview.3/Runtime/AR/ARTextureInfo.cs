using System;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

using Object = UnityEngine.Object;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Container that pairs a <see cref="Unity.XR.ARSubsystems.XRTextureDescriptor"/> that wraps a native texture
    /// object and a <c>Texture2D</c> that is created for the native texture object.
    /// </summary>
    internal struct ARTextureInfo : IEquatable<ARTextureInfo>, IDisposable
    {
        /// <summary>
        /// Constant for whether the texture is in a linear color space.
        /// </summary>
        /// <value>
        /// Constant for whether the texture is in a linear color space.
        /// </value>
        const bool k_TextureHasLinearColorSpace = false;

        /// <summary>
        /// The texture descriptor describing the metadata for the native texture object.
        /// </summary>
        /// <value>
        /// The texture descriptor describing the metadata for the native texture object.
        /// </value>
        public XRTextureDescriptor descriptor
        {
            get { return m_Descriptor; }
        }
        XRTextureDescriptor m_Descriptor;

        /// <summary>
        /// The Unity <c>Texture2D</c> object for the native texture.
        /// </summary>
        /// <value>
        /// The Unity <c>Texture2D</c> object for the native texture.
        /// </value>
        public Texture2D texture
        {
            get { return m_Texture; }
        }
        Texture2D m_Texture;

        /// <summary>
        /// Constructs the texture info with the given descriptor and material.
        /// </summary>
        /// <param name="descriptor">The texture descriptor wrapping a native texture object.</param>
        public ARTextureInfo(XRTextureDescriptor descriptor)
        {
            m_Descriptor = descriptor;
            m_Texture = CreateTexture(m_Descriptor);
        }

        /// <summary>
        /// Resets the texture info back to the default state destroying the texture game object, if one exists.
        /// </summary>
        public void Reset()
        {
            m_Descriptor.Reset();
            DestroyTexture();
        }

        /// <summary>
        /// Destroys the texture, and sets the property to <c>null</c>.
        /// </summary>
        void DestroyTexture()
        {
            if (m_Texture != null)
            {
                UnityEngine.Object.Destroy(m_Texture);
                m_Texture = null;
            }
        }

        /// <summary>
        /// Sets the current descriptor, and creates/updates the associated texture as appropriate.
        /// </summary>
        /// <param name="textureInfo">The texture info to update.</param>
        /// <param name="descriptor">The texture descriptor wrapping a native texture object.</param>
        /// <returns>
        /// The updated texture information.
        /// </returns>
        public static ARTextureInfo GetUpdatedTextureInfo(ARTextureInfo textureInfo, XRTextureDescriptor descriptor)
        {
            // If the current and given descriptors are equal, exit early from this method.
            if (textureInfo.m_Descriptor.Equals(descriptor))
            {
                return textureInfo;
            }

            // If the given descriptor is invalid, destroy any existing texture, and return the default texture
            // info.
            if (!descriptor.valid)
            {
                textureInfo.DestroyTexture();
                return default(ARTextureInfo);
            }

            // If there is a texture already and if the descriptors have identical texture metadata, we only need
            // to update the existing texture with the given native texture object.
            if ((textureInfo.m_Texture != null) && textureInfo.m_Descriptor.hasIdenticalTextureMetadata(descriptor))
            {
                // Update the current descriptor with the given descriptor.
                textureInfo.m_Descriptor = descriptor;

                // Update the current texture with the native texture object.
                textureInfo.m_Texture.UpdateExternalTexture(textureInfo.m_Descriptor.nativeTexture);
            }
            // Else, we need to destroy the existing texture object and create a new texture object.
            else
            {
                // Update the current descriptor with the given descriptor.
                textureInfo.m_Descriptor = descriptor;

                // Replace the current texture with a newly created texture, and update the material.
                textureInfo.DestroyTexture();
                textureInfo.m_Texture = CreateTexture(textureInfo.m_Descriptor);
            }

            return textureInfo;
        }

        /// <summary>
        /// Create the texture object for the native texture wrapped by the valid descriptor.
        /// </summary>
        /// <param name="descriptor">The texture descriptor wrapping a native texture object.</param>
        /// <returns>
        /// If the descriptor is valid, the <c>Texture2D</c> object created from the texture descriptor. Otherwise,
        /// <c>null</c>.
        /// </returns>
        static Texture2D CreateTexture(XRTextureDescriptor descriptor)
        {
            if (!descriptor.valid)
            {
                return null;
            }

            Texture2D texture = Texture2D.CreateExternalTexture(descriptor.width, descriptor.height,
                                                                descriptor.format, (descriptor.mipmapCount != 0),
                                                                k_TextureHasLinearColorSpace,
                                                                descriptor.nativeTexture);

            // NB: SetWrapMode needs to be the first call here, and the value passed
            //     needs to be kTexWrapClamp - this is due to limitations of what
            //     wrap modes are allowed for external textures in OpenGL (which are
            //     used for ARCore), as Texture::ApplySettings will eventually hit
            //     an assert about an invalid enum (see calls to glTexParameteri
            //     towards the top of ApiGLES::TextureSampler)
            // reference: "3.7.14 External Textures" section of
            // https://www.khronos.org/registry/OpenGL/extensions/OES/OES_EGL_image_external.txt
            // (it shouldn't ever matter what the wrap mode is set to normally, since
            // this is for a pass-through video texture, so we shouldn't ever need to
            // worry about the wrap mode as textures should never "wrap")
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;
            texture.hideFlags = HideFlags.HideAndDontSave;

            return texture;
        }

        public void Dispose()
        {
            DestroyTexture();
        }

        public override int GetHashCode()
        {
            int hash = 486187739;
            unchecked
            {
                hash = hash * 486187739 + m_Descriptor.GetHashCode();
                hash = hash * 486187739 + ((m_Texture == null) ? 0 : m_Texture.GetHashCode());
            }
            return hash;
        }

        public bool Equals(ARTextureInfo other)
        {
            return m_Descriptor.Equals(other) && (m_Texture == other.m_Texture);
        }

        public override bool Equals(System.Object obj)
        {
            return ((obj is ARTextureInfo) && Equals((ARTextureInfo)obj));
        }

        public static bool operator ==(ARTextureInfo lhs, ARTextureInfo rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ARTextureInfo lhs, ARTextureInfo rhs)
        {
            return !lhs.Equals(rhs);
        }
    }
}
