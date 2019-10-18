using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Experimental.XR.Interaction;

namespace UnityEngine.SpatialTracking
{
    [CustomEditor(typeof(TrackedPoseDriver))]
    internal class TrackedPoseDriverEditor : Editor
    {
        static class Styles
        {
            public static GUIContent deviceLabel = EditorGUIUtility.TrTextContent("Device", "The Device to read tracking data from ");
            public static GUIContent poseLabel = EditorGUIUtility.TrTextContent("Pose Source", "The end point on the device to read tracking data from");
            public static GUIContent trackingLabel = EditorGUIUtility.TrTextContent("Tracking Type", "Whether Rotation or Position, or Both are applied from the source pose");
            public static GUIContent updateLabel = EditorGUIUtility.TrTextContent("Update Type", "Whether the Tracked Pose Driver updates in update, and/or just before rendering");
            public static GUIContent relativeLabel = EditorGUIUtility.TrTextContent("Use Relative Transform", "When this is set, the Tracked Pose Driver will use the original position of the object as a reference. This option will be deprecated in future releases");
            public static GUIContent poseProviderLabel = EditorGUIUtility.TrTextContent("Use Pose Provider", " [Optional] when a PoseProvider object is attached here, the pose provider will be used as the data source, not the Device/Pose settings on the Tracked Pose Driver");
            public static readonly string poseProviderWarning = "This Tracked Pose Driver is using an external component as its Pose Source.";
            public static readonly string devicePropWarning = "The selected Pose Source is not valid, please pick a different pose";
            public static readonly string cameraWarning = "The Tracked Pose Driver is attached to a camera, but is not tracking the Center Eye / HMD Reference. This may cause tracking problems if this camera is intended to track the headset.";
        }

        SerializedProperty m_DeviceProp = null;
        SerializedProperty m_PoseLabelProp = null;
        SerializedProperty m_TrackingTypeProp = null;
        SerializedProperty m_UpdateTypeProp = null;
        SerializedProperty m_UseRelativeTransformProp = null;
        SerializedProperty m_PoseProviderProp = null;

        void OnEnable()
        {
            m_DeviceProp = this.serializedObject.FindProperty("m_Device");
            m_PoseLabelProp = this.serializedObject.FindProperty("m_PoseSource");
            m_TrackingTypeProp = this.serializedObject.FindProperty("m_TrackingType");
            m_UpdateTypeProp = this.serializedObject.FindProperty("m_UpdateType");
            m_UseRelativeTransformProp = this.serializedObject.FindProperty("m_UseRelativeTransform");
            m_PoseProviderProp = this.serializedObject.FindProperty("m_PoseProviderComponent");
        }

        public override void OnInspectorGUI()
        {
       
            TrackedPoseDriver tpd = target as TrackedPoseDriver;
            serializedObject.Update();

            if (m_PoseProviderProp.objectReferenceValue == null)
            {
                EditorGUILayout.PropertyField(m_DeviceProp, Styles.deviceLabel);

                int selectedIndex = -1;
                for (int i = 0; i < TrackedPoseDriverDataDescription.DeviceData[m_DeviceProp.enumValueIndex].Poses.Count; ++i)
                {
                    if ((int)TrackedPoseDriverDataDescription.DeviceData[m_DeviceProp.enumValueIndex].Poses[i] == m_PoseLabelProp.enumValueIndex)
                    {
                        selectedIndex = i;
                        break;
                    }
                }
                Rect rect = EditorGUILayout.GetControlRect();
                EditorGUI.LabelField(rect, Styles.poseLabel);
                rect.xMin += EditorGUIUtility.labelWidth;

                if (selectedIndex != -1)
                {
                    int index = EditorGUI.Popup(rect, selectedIndex, TrackedPoseDriverDataDescription.DeviceData[m_DeviceProp.enumValueIndex].PoseNames.ToArray());
                    m_PoseLabelProp.enumValueIndex = (int)TrackedPoseDriverDataDescription.DeviceData[m_DeviceProp.enumValueIndex].Poses[index];
                    if(tpd && 
                        (m_DeviceProp.enumValueIndex == 0 && m_PoseLabelProp.enumValueIndex !=  (int)(TrackedPoseDriver.TrackedPose.Center)))
                    {
                        Camera camera = tpd.GetComponent<Camera>();
                        if(camera != null)
                        {
                            EditorGUILayout.HelpBox(Styles.cameraWarning, MessageType.Warning, true);
                        }
                    }
                }
                else
                {
                    int index = EditorGUI.Popup(rect, 0, TrackedPoseDriverDataDescription.DeviceData[m_DeviceProp.enumValueIndex].PoseNames.ToArray());
                    m_PoseLabelProp.enumValueIndex = (int)TrackedPoseDriverDataDescription.DeviceData[m_DeviceProp.enumValueIndex].Poses[index];
                    EditorGUILayout.HelpBox(Styles.devicePropWarning, MessageType.Warning, true);
                }
            }
            else
            {
                EditorGUILayout.HelpBox(Styles.poseProviderWarning, MessageType.Info, true);
            }

            EditorGUILayout.PropertyField(m_TrackingTypeProp, Styles.trackingLabel);
            EditorGUILayout.PropertyField(m_UpdateTypeProp, Styles.updateLabel);
            EditorGUILayout.PropertyField(m_UseRelativeTransformProp, Styles.relativeLabel);

            EditorGUILayout.PropertyField(m_PoseProviderProp, Styles.poseProviderLabel);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
