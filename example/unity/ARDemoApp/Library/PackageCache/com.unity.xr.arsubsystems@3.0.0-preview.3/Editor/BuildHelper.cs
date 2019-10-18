using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build;
using UnityEngine;

using Object = UnityEngine.Object;

namespace UnityEditor.XR.ARSubsystems
{
    /// <summary>
    /// Class with helper methods for interacting with the build process.
    /// </summary>
    public static class BuildHelper
    {
        /// <summary>
        /// Adds a background shader with the given name to the project as a preloaded asset.
        /// </summary>
        /// <param name="shaderName">The name of a shader to add to the project.</param>
        /// <exception cref="UnityEditor.Build.BuildFailedException">Thrown if a shader with the given name cannot be
        /// found.</exception>
        public static void AddBackgroundShaderToProject(string shaderName)
        {
            if (string.IsNullOrEmpty(shaderName))
            {
                Debug.LogWarning("Incompatible render pipeline in GraphicsSettings.renderPipelineAsset. Background "
                                 + "rendering may not operate properly.");
            }
            else
            {
                Shader shader = FindShaderOrFailBuild(shaderName);

                Object[] preloadedAssets = PlayerSettings.GetPreloadedAssets();

                var shaderAssets = (from preloadedAsset in preloadedAssets where shader.Equals(preloadedAsset)
                                    select preloadedAsset);
                if ((shaderAssets == null) || !shaderAssets.Any())
                {
                    List<Object> preloadedAssetsList = preloadedAssets.ToList();
                    preloadedAssetsList.Add(shader);
                    PlayerSettings.SetPreloadedAssets(preloadedAssetsList.ToArray());
                }
            }
        }

        /// <summary>
        /// Removes a shader with the given name from the preloaded assets of the project.
        /// </summary>
        /// <param name="shaderName">The name of a shader to remove from the project.</param>
        /// <exception cref="UnityEditor.Build.BuildFailedException">Thrown if a shader with the given name cannot be
        /// found.</exception>
        public static void RemoveShaderFromProject(string shaderName)
        {
            if (!string.IsNullOrEmpty(shaderName))
            {
                Shader shader = FindShaderOrFailBuild(shaderName);

                Object[] preloadedAssets = PlayerSettings.GetPreloadedAssets();

                var nonShaderAssets = (from preloadedAsset in preloadedAssets where !shader.Equals(preloadedAsset)
                                       select preloadedAsset);
                PlayerSettings.SetPreloadedAssets(nonShaderAssets.ToArray());
            }
        }

        /// <summary>
        /// Finds a shader with the given name, or fail the build, if no shader is found.
        /// </summary>
        /// <param name="shaderName">The name of a shader to find.</param>
        /// <returns>
        /// The shader with the given name.
        /// </returns>
        /// <exception cref="UnityEditor.Build.BuildFailedException">Thrown if a shader with the given name cannot be
        /// found.</exception>
        static Shader FindShaderOrFailBuild(string shaderName)
        {
            var shader = Shader.Find(shaderName);
            if (shader == null)
            {
                throw new BuildFailedException($"Cannot find shader '{shaderName}'");
            }

            return shader;
        }
    }
}
