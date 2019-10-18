using System;
using UnityEngine.Scripting;

namespace UnityEngine.XR.ARCore
{
    /// <summary>
    /// Manages Android permissions for the application.
    /// Allows you to determine whether a permission has been
    /// granted and request permission.
    /// </summary>
    public class ARCorePermissionManager : AndroidJavaProxy
    {
        /// <summary>
        /// Checks if an Android permission is granted to the application.
        /// </summary>
        /// <param name="permissionName">The full name of the Android permission to check (e.g.
        /// android.permission.CAMERA).</param>
        /// <returns><c>true</c> if <c>permissionName</c> is granted to the application, otherwise
        /// <c>false</c>.</returns>
        public static bool IsPermissionGranted(string permissionName)
        {
            if (Application.isEditor)
                return true;

            return permissionsService.Call<bool>("IsPermissionGranted", activity, permissionName);
        }

        /// <summary>
        /// Requests an Android permission from the user.
        /// </summary>
        /// <param name="permissionName">The permission to be requested (e.g. android.permission.CAMERA).</param>
        /// <param name="callback">A delegate to invoke when the permission has been granted or denied. The
        /// parameters of the delegate are the <paramref name="permissionName"/> being requested and a <c>bool</c>
        /// indicating whether permission was granted.</param>
        public static void RequestPermission(string permissionName, Action<string, bool> callback)
        {
            if (callback == null)
                throw new ArgumentNullException("callback");

            if (IsPermissionGranted(permissionName))
            {
                callback(permissionName, true);
                return;
            }

            if (s_CurrentCallback != null)
                throw new InvalidOperationException("Cannot start a new permissions request before the current one finishes.");

            permissionsService.Call("RequestPermissionAsync", activity, new[] { permissionName }, instance);
            s_CurrentCallback = callback;
        }

        // UnityAndroidPermissions interface
        [Preserve]
        void OnPermissionGranted(string permissionName)
        {
            s_CurrentCallback(permissionName, true);
            s_CurrentCallback = null;
        }

        // UnityAndroidPermissions interface
        [Preserve]
        void OnPermissionDenied(string permissionName)
        {
            s_CurrentCallback(permissionName, false);
            s_CurrentCallback = null;
        }

        // UnityAndroidPermissions interface (unused)
        [Preserve]
        void OnActivityResult() { }

        ARCorePermissionManager()
            : base(k_AndroidPermissionsClass)
        { }

        static ARCorePermissionManager instance
        {
            get
            {
                if (s_Instance == null)
                    s_Instance = new ARCorePermissionManager();

                return s_Instance;
            }
        }

        static AndroidJavaObject activity
        {
            get
            {
                if (s_Activity == null)
                {
                    var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    s_Activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                }

                return s_Activity;
            }
        }

        static AndroidJavaObject permissionsService
        {
            get
            {
                if (s_PermissionService == null)
                    s_PermissionService = new AndroidJavaObject(k_AndroidPermissionService);

                return s_PermissionService;
            }
        }

        static ARCorePermissionManager s_Instance;

        static AndroidJavaObject s_Activity;

        static AndroidJavaObject s_PermissionService;

        static Action<string, bool> s_CurrentCallback;

        static readonly string k_AndroidPermissionsClass = "com.unity3d.plugin.UnityAndroidPermissions$IPermissionRequestResult";

        static readonly string k_AndroidPermissionService = "com.unity3d.plugin.UnityAndroidPermissions";
    }
}
