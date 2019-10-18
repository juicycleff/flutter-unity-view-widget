using System;
using System.Collections.Generic;
using System.IO;

using UnityEditor;

using UnityEngine;
using UnityEngine.UIElements;

#if XR_MANAGEMENT

namespace UnityEngine.SpatialTracking
{
    internal class TPDConfigurationProvider : SettingsProvider
    {        
        readonly static string s_SettingsRootTitle = "Project/XR Plugin Management/XR Tracking";
        readonly static string s_TPDCameraReason = "Cameras no longer automatically follow the HMD in VR. To have cameras track the position of the HMD use the following button to attach a Tracked Pose Driver to all cameras tagged as 'Main Camera' in the current scene.";
        readonly static GUIContent s_AddTPDToMainCameras = new GUIContent("Add Tracked Pose Driver To Cameras with 'Main Camera' Tag in the scene");
        readonly static GUIContent s_AddTPDToStereoCameras = new GUIContent("Add Tracked Pose Driver To All Stereo Cameras in the scene");
        readonly static GUIContent s_AddTPDToAllCameras = new GUIContent("Add Tracked Pose Driver To All Cameras In the scene");

        static TPDConfigurationProvider s_TPDSettings = null;        

        public TPDConfigurationProvider(string path, SettingsScope scopes = SettingsScope.Project) : base(path, scopes)
        {
        }

        [SettingsProvider]
        [UnityEngine.Internal.ExcludeFromDocs]
        static SettingsProvider Create()
        {
            if (s_TPDSettings == null)
            {
                s_TPDSettings = new TPDConfigurationProvider(s_SettingsRootTitle);
            }

            return s_TPDSettings;
        }

        private void AddTPDToCamera(Camera camera)
        {
            TrackedPoseDriver tpd = camera.gameObject.AddComponent<TrackedPoseDriver>();
            tpd.SetPoseSource(TrackedPoseDriver.DeviceType.GenericXRDevice, TrackedPoseDriver.TrackedPose.Center);
            tpd.UseRelativeTransform = false;
            Debug.Log("Added Tracked Pose Driver to the camera named " + camera.name, camera);
        }

        private void AddTPDToMainCamerasInScene()
        {
            var cameras = Object.FindObjectsOfType<Camera>();
            foreach(var camera in cameras)
            {
                if(camera.CompareTag("MainCamera"))
                {
                    if(camera.GetComponent<TrackedPoseDriver>() == null)
                    {
                        AddTPDToCamera(camera);
                    }
                }
            }            
        }

        private void AddTPDToStereoCamerasInScene()
        {
            var cameras = Object.FindObjectsOfType<Camera>();
            foreach (var camera in cameras)
            {
                if (camera.stereoEnabled == true)
                {
                    if (camera.GetComponent<TrackedPoseDriver>() == null)
                    {
                        AddTPDToCamera(camera);
                    }
                }
            }
        }
        
        private void AddToAllCamerasInScene()
        {
            var cameras = Object.FindObjectsOfType<Camera>();
            foreach (var camera in cameras)
            {              
                if (camera.GetComponent<TrackedPoseDriver>() == null)
                {
                    AddTPDToCamera(camera);
                }            
            }
        }

        public override void OnGUI(string searchContext)
        {
            GUI.skin.label.wordWrap = true;
            GUILayout.Label(s_TPDCameraReason);
            if (GUILayout.Button(s_AddTPDToMainCameras, GUILayout.ExpandWidth(false)))
            {
                AddTPDToMainCamerasInScene();
            }
            if (GUILayout.Button(s_AddTPDToStereoCameras, GUILayout.ExpandWidth(false)))
            {
                AddTPDToStereoCamerasInScene();
            }
            if (GUILayout.Button(s_AddTPDToAllCameras, GUILayout.ExpandWidth(false)))
            {
                AddToAllCamerasInScene();
            }
            base.OnGUI(searchContext);
        }        
    }
}
#endif
