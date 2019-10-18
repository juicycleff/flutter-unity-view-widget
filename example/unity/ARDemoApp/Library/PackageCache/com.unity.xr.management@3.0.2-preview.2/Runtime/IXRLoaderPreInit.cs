#if UNITY_EDITOR
using UnityEditor;

namespace UnityEngine.XR.Management
{
    /// <summary>
    /// XRLoader interface for retrieving the XR SDK PreInit library name from an XRLoader instance
    /// </summary>
    public interface IXRLoaderPreInit
    {
        /// <summary>
        /// Get the library name, if any, to use for XR SDK PreInit.
        /// </summary>
        ///
        /// <param name="buildTarget">An enum specifying which platform this build is for.</param>
        /// <param name="buildTargetGroup">An enum specifying which platform group this build is for.</param>
        /// <returns>A string specifying the library name used for XR SDK PreInit.</returns>
        string GetPreInitLibraryName(BuildTarget buildTarget, BuildTargetGroup buildTargetGroup);
    }
}
#endif
