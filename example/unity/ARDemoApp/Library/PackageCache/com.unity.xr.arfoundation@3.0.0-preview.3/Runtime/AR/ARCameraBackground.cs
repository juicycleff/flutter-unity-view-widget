using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// <para>Add this component to a <c>Camera</c> to copy the color camera's texture onto the background.</para>
    /// <para>If you are using the Lightweight Render Pipeline (version 5.7.2 or later) or the Univerisal Render
    /// Pipeline (version 7.0.0 or later), you must also add the <see cref="ARBackgroundRendererFeature"/> to the list
    /// of render features for the scriptable renderer.</para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// To add the <see cref="ARBackgroundRendererFeature"/> to the list of render features for the scriptable
    /// renderer:
    /// <list type="bullet">
    /// <item><description>In Project Settings -> Graphics, select the render pipeline asset (either
    /// <c>LightweightRenderPipelineAsset</c> or <c>UniversalRenderPipelineAsset</c>) that is in the Scriptable Render
    /// Pipeline Settings field.</description></item>
    /// <item><description>In the Inspector with the render pipeline asset selected, ensure that the Render Type is set
    /// to Custom.</description></item>
    /// <item><description>In the Inspector with the render pipeline asset selected, select the Render Type -> Data
    /// asset which would be of type <c>ForwardRendererData</c>.</description></item>
    /// <item><description>In the Inspector with the forward renderer data selected, ensure the Render Features list
    /// contains a <see cref="ARBackgroundRendererFeature"/>.</description></item>
    /// </list>
    /// </para>
    /// <para>To customize background rendering with the legacy render pipeline, override both the
    /// <see cref="ConfigureLegacyRenderPipelineBackgroundRendering"/> method and the
    /// <see cref="TeardownLegacyRenderPipelineBackgroundRendering"/> method to modify the given
    /// <c>CommandBuffer</c> with rendering commands and to inject the given <c>CommandBuffer</c> into the camera's
    /// rendering.</para>
    /// <para>To customize background rendering with a scriptable render pipeline, create a
    /// <c>ScriptableRendererFeature</c> with the background rendering commands, and insert the
    /// <c>ScriptableRendererFeature</c> into the list of render features for the scriptable renderer.</para>
    /// </remarks>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(ARCameraManager))]
    [HelpURL("https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@latest?preview=1&subfolder=/api/UnityEngine.XR.ARFoundation.ARCameraBackground.html")]
    public class ARCameraBackground : MonoBehaviour
    {
        /// <summary>
        /// Name for the custom rendering command buffer.
        /// </summary>
        const string k_CustomRenderPassName = "AR Background Pass (LegacyRP)";

        /// <summary>
        /// Name of the main texture parameter for the material
        /// </summary>
        internal const string k_MainTexName = "_MainTex";

        /// <summary>
        /// Name of the shader parameter for the display transform matrix.
        /// </summary>
        const string k_DisplayTransformName = "_UnityDisplayTransform";

        /// <summary>
        /// Property ID for the shader parameter for the display transform matrix.
        /// </summary>
        static readonly int k_DisplayTransformId = Shader.PropertyToID(k_DisplayTransformName);

        /// <summary>
        /// The camera to which the projection matrix is set on each frame event.
        /// </summary>
        Camera m_Camera;

        /// <summary>
        /// The camera manager from which frame information is pulled.
        /// </summary>
        ARCameraManager m_CameraManager;

        /// <summary>
        /// Command buffer for any custom rendering commands.
        /// </summary>
        CommandBuffer m_CommandBuffer;

        /// <summary>
        /// Whether to use the custom material for rendering the background.
        /// </summary>
        [SerializeField, FormerlySerializedAs("m_OverrideMaterial")]
        bool m_UseCustomMaterial;

        /// <summary>
        /// A custom material for rendering the background.
        /// </summary>
        [SerializeField, FormerlySerializedAs("m_Material")]
        Material m_CustomMaterial;

        /// <summary>
        /// The default material for rendering the background.
        /// </summary>
        Material m_DefaultMaterial;

        /// <summary>
        /// The previous clear flags for the camera, if any.
        /// </summary>
        CameraClearFlags? m_PreviousCameraClearFlags;

        /// <summary>
        /// Whether background rendering is enabled.
        /// </summary>
        bool m_BackgroundRenderingEnabled;

        /// <summary>
        /// The camera to which the projection matrix is set on each frame event.
        /// </summary>
        /// <value>
        /// The camera to which the projection matrix is set on each frame event.
        /// </value>
#if UNITY_EDITOR
        protected new Camera camera { get => m_Camera; }
#else // UNITY_EDITOR
        protected Camera camera { get => m_Camera; }
#endif // UNITY_EDITOR

        /// <summary>
        /// The camera manager from which frame information is pulled.
        /// </summary>
        /// <value>
        /// The camera manager from which frame information is pulled.
        /// </value>
        protected ARCameraManager cameraManager { get => m_CameraManager; }

        /// <summary>
        /// The current <c>Material</c> used for background rendering.
        /// </summary>
        public Material material
        {
            get { return (useCustomMaterial && (customMaterial != null)) ? customMaterial : defaultMaterial; }
        }

        /// <summary>
        /// Whether to use the custom material for rendering the background.
        /// </summary>
        /// <value>
        /// <c>true</c> if the custom material should be used for rendering the camera background. Otherwise,
        /// <c>false</c>.
        /// </value>
        public bool useCustomMaterial { get => m_UseCustomMaterial; set => m_UseCustomMaterial = value; }

        /// <summary>
        /// A custom material for rendering the background.
        /// </summary>
        /// <value>
        /// A custom material for rendering the background.
        /// </value>
        public Material customMaterial { get => m_CustomMaterial; set => m_CustomMaterial = value; }

        /// <summary>
        /// Whether background rendering is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if background rendering is enabled and if at least one camera frame has been received.
        /// Otherwise, <c>false</c>.
        /// </value>
        public bool backgroundRenderingEnabled => m_BackgroundRenderingEnabled;

        /// <summary>
        /// The default material for rendering the background.
        /// </summary>
        /// <value>
        /// The default material for rendering the background.
        /// </value>
        Material defaultMaterial { get => cameraManager.cameraMaterial; }

        /// <summary>
        /// Whether to use the legacy rendering pipeline.
        /// </summary>
        /// <value>
        /// <c>true</c> fi the legacy render pipeline is in use. Otherwise, <c>false</c>.
        /// </value>
        bool useLegacyRenderPipeline { get => GraphicsSettings.renderPipelineAsset == null; }

        void Awake()
        {
            m_Camera = GetComponent<Camera>();
            m_CameraManager = GetComponent<ARCameraManager>();
        }

        void OnEnable()
        {
            // Ensure that background rendering is disabled until the first camera frame is received.
            m_BackgroundRenderingEnabled = false;
            cameraManager.frameReceived += OnCameraFrameReceived;
        }

        void OnDisable()
        {
            cameraManager.frameReceived -= OnCameraFrameReceived;
            DisableBackgroundRendering();
        }

        /// <summary>
        /// Enable background rendering by disabling the camera's clear flags, and enabling the legacy RP background
        /// rendering if we are in legacy RP mode.
        /// </summary>
        void EnableBackgroundRendering()
        {
            m_BackgroundRenderingEnabled = true;

            DisableBackgroundClearFlags();

            Material material = defaultMaterial;
            if (useLegacyRenderPipeline && (material != null))
            {
                EnableLegacyRenderPipelineBackgroundRendering();
            }
        }

        /// <summary>
        /// Disable background rendering by disabling the legacy RP background rendering if we are in legacy RP mode
        /// and restoring the camera's clear flags.
        /// </summary>
        void DisableBackgroundRendering()
        {
            m_BackgroundRenderingEnabled = false;

            DisableLegacyRenderPipelineBackgroundRendering();

            RestoreBackgroundClearFlags();

            // We are no longer setting the projection matrix so tell the camera to resume its normal projection matrix
            // calculations.
            camera.ResetProjectionMatrix();
        }

        /// <summary>
        /// Set the camera's clear flags to do nothing while preserving the previous camera clear flags.
        /// </summary>
        void DisableBackgroundClearFlags()
        {
            m_PreviousCameraClearFlags = m_Camera.clearFlags;
            m_Camera.clearFlags = CameraClearFlags.Nothing;
        }

        /// <summary>
        /// Restore the previous camera's clear flags, if any.
        /// </summary>
        void RestoreBackgroundClearFlags()
        {
            if (m_PreviousCameraClearFlags != null)
            {
                m_Camera.clearFlags = m_PreviousCameraClearFlags.Value;
            }
        }

        /// <summary>
        /// Enable background rendering getting a command buffer, and configure it for rendering the background.
        /// </summary>
        void EnableLegacyRenderPipelineBackgroundRendering()
        {
            if (m_CommandBuffer == null)
            {
                m_CommandBuffer = new CommandBuffer();
                m_CommandBuffer.name = k_CustomRenderPassName;

                ConfigureLegacyRenderPipelineBackgroundRendering(m_CommandBuffer);
            }
        }

        /// <summary>
        /// Disable background rendering by removing the command buffer from the camera.
        /// </summary>
        void DisableLegacyRenderPipelineBackgroundRendering()
        {
            if (m_CommandBuffer != null)
            {
                TeardownLegacyRenderPipelineBackgroundRendering(m_CommandBuffer);

                m_CommandBuffer = null;
            }
        }

        /// <summary>
        /// Configure the command buffer for background rendering by inserting the blit, and adding the command buffer
        /// into the camera.
        /// </summary>
        /// <param name="commandBuffer">The command buffer to configure.</param>
        protected virtual void ConfigureLegacyRenderPipelineBackgroundRendering(CommandBuffer commandBuffer)
        {
            Texture texture = !material.HasProperty(k_MainTexName) ? null : material.GetTexture(k_MainTexName);

            commandBuffer.ClearRenderTarget(true, false, Color.clear);
            commandBuffer.Blit(texture, BuiltinRenderTextureType.CameraTarget, material);
            camera.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, m_CommandBuffer);
            camera.AddCommandBuffer(CameraEvent.BeforeGBuffer, m_CommandBuffer);
        }

        /// <summary>
        /// Teardown the command buffer that was configured for background rendering by removing the command buffer
        /// from the camera.
        /// </summary>
        /// <param name="commandBuffer">The command buffer to teaerdown.</param>
        protected virtual void TeardownLegacyRenderPipelineBackgroundRendering(CommandBuffer commandBuffer)
        {
            camera.RemoveCommandBuffer(CameraEvent.BeforeForwardOpaque, m_CommandBuffer);
            camera.RemoveCommandBuffer(CameraEvent.BeforeGBuffer, m_CommandBuffer);
        }

        /// <summary>
        /// Callback for the camera frame event.
        /// </summary>
        /// <param name="eventArgs">The camera event arguments.</param>
        void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
        {
            // Enable background rendering when first frame is received.
            if (!m_BackgroundRenderingEnabled)
            {
                EnableBackgroundRendering();
            }

            Material material = this.material;
            if (material != null)
            {
                var count = eventArgs.textures.Count;
                for (int i = 0; i < count; ++i)
                {
                    material.SetTexture(eventArgs.propertyNameIds[i], eventArgs.textures[i]);
                }

                if (eventArgs.displayMatrix.HasValue)
                {
                    material.SetMatrix(k_DisplayTransformId, eventArgs.displayMatrix.Value);
                }
            }

            if (eventArgs.projectionMatrix.HasValue)
            {
                camera.projectionMatrix = eventArgs.projectionMatrix.Value;
            }
        }
    }
}
