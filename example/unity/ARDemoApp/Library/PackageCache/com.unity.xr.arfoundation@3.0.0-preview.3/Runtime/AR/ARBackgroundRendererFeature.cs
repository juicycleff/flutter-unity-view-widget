using UnityEngine;
using UnityEngine.Rendering;
#if MODULE_URP_ENABLED
using UnityEngine.Rendering.Universal;
#elif MODULE_LWRP_ENABLED
using UnityEngine.Rendering.LWRP;
#else
using ScriptableRendererFeature = UnityEngine.ScriptableObject;
#endif

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// A render feature for rendering the camera background for AR devies.
    /// </summary>
    public class ARBackgroundRendererFeature : ScriptableRendererFeature
    {
#if MODULE_URP_ENABLED || MODULE_LWRP_ENABLED
        /// <summary>
        /// The scriptable render pass to be added to the renderer when the camera background is to be rendered.
        /// </summary>
        CustomRenderPass m_ScriptablePass;

        /// <summary>
        /// Create the scriptable render pass.
        /// </summary>
        public override void Create()
        {
#if !UNITY_EDITOR
            m_ScriptablePass = new CustomRenderPass(RenderPassEvent.BeforeRenderingOpaques);
#endif // !UNITY_EDITOR
        }

        /// <summary>
        /// Add the background rendering pass when rendering a game camera with an enabled AR camera background component.
        /// </summary>
        /// <param name="renderer">The sriptable renderer in which to enqueue the render pass.</param>
        /// <param name="renderingData">Additional rendering data about the current state of rendering.</param>
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
#if !UNITY_EDITOR
            Camera currentCamera = renderingData.cameraData.camera;
            if ((currentCamera != null) && (currentCamera.cameraType == CameraType.Game))
            {
                ARCameraBackground cameraBackground = currentCamera.gameObject.GetComponent<ARCameraBackground>();
                if ((cameraBackground != null) && cameraBackground.backgroundRenderingEnabled && (cameraBackground.material != null))
                {
                    m_ScriptablePass.Setup(cameraBackground.material, renderer.cameraColorTarget, renderer.cameraDepth);
                    renderer.EnqueuePass(m_ScriptablePass);
                }
            }
#endif // !UNITY_EDITOR
        }

        /// <summary>
        /// The custom render pass to render the camera background.
        /// </summary>
        class CustomRenderPass : ScriptableRenderPass
        {
            /// <summary>
            /// The name for the custom render pass which will be display in graphics debugging tools.
            /// </summary>
            const string k_CustomRenderPassName = "AR Background Pass (URP)";

            /// <summary>
            /// The material used for blitting the camera video texture to the device background.
            /// </summary>
            Material m_Material;

            /// <summary>
            /// The camera video texture used for blitting to the device background.
            /// </summary>
            Texture m_Texture;

            /// <summary>
            /// The destination render target identifier for blitting the background color.
            /// </summary>
            RenderTargetIdentifier m_ColorTargetIdentifier;

            /// <summary>
            /// The destination render target identifier for blitting the background depth.
            /// </summary>
            RenderTargetIdentifier m_DepthTargetIdentifier;

            /// <summary>
            /// Constructs the background render pass.
            /// </summary>
            /// <param name="renderPassEvent">The render pass event when this pass should be rendered.</param>
            public CustomRenderPass(RenderPassEvent renderPassEvent)
            {
                this.renderPassEvent = renderPassEvent;
            }

            /// <summary>
            /// Setup the background render pass.
            /// </summary>
            /// <param name="material">The material to use when blitting the background texture.</param>
            /// <param name="colorTargetIdentifier">The color target to which to blit the background texture.</param>
            /// <param name="depthTargetIdentifier">The depth target to which to blit the background texture.</param>

            public void Setup(Material material, RenderTargetIdentifier colorTargetIdentifier,
                              RenderTargetIdentifier depthTargetIdentifier)
            {
                m_Material = material;
                m_Texture = !m_Material.HasProperty(ARCameraBackground.k_MainTexName) ? null : m_Material.GetTexture(ARCameraBackground.k_MainTexName);
                m_ColorTargetIdentifier = colorTargetIdentifier;
                m_DepthTargetIdentifier = depthTargetIdentifier;
            }

            /// <summary>
            /// Configure the render pass by configuring the render target and clear values.
            /// </summary>
            /// <param name="commandBuffer">The command buffer for configuration.</param>
            /// <param name="renderTextureDescriptor">The descriptor of the target render texture.</param>
            public override void Configure(CommandBuffer commandBuffer, RenderTextureDescriptor renderTextureDescriptor)
            {
                ConfigureTarget(m_ColorTargetIdentifier, m_DepthTargetIdentifier);
                ConfigureClear(ClearFlag.Depth, Color.clear);
            }

            /// <summary>
            /// Execute the render commands to blit the camera background texture.
            /// </summary>
            /// <param name="context">The render context for executing the render commands.</param>
            /// <param name="renderingData">Additional rendering data about the current state of rendering.</param>
            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                var cmd = CommandBufferPool.Get(k_CustomRenderPassName);

                Blit(cmd, m_Texture, m_ColorTargetIdentifier, m_Material);
                context.ExecuteCommandBuffer(cmd);

                CommandBufferPool.Release(cmd);
            }

            /// <summary>
            /// Clean up any resources for the render pass.
            /// </summary>
            /// <param name="commandBuffer">The command buffer for frame cleanup.</param>
            public override void FrameCleanup(CommandBuffer commandBuffer)
            {
            }
        }
#endif // MODULE_URP_ENABLED || MODULE_LWRP_ENABLED
    }
}
