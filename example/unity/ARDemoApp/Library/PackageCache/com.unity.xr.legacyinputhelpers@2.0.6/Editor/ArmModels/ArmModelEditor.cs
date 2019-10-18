using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.LegacyInputHelpers;

namespace UnityEditor.XR.LegacyInputHelpers
{
    [CustomEditor(typeof(ArmModel))]
    internal class ArmModelEditor : Editor
    {

        protected static class ArmModelStyles
        {
            public static GUIContent poseSourceLabel = EditorGUIUtility.TrTextContent("Input Pose Source", "The source of the 3dof controller data");
            public static GUIContent headPositionSourceLabel = EditorGUIUtility.TrTextContent("Head Position Source", "The source of head position data used by the arm model");
            public static GUIContent isLockedToNeckLabel = EditorGUIUtility.TrTextContent("Lock To Neck", "If true, the root of the pose is locked to the local position of the player's neck");
            public static GUIContent armExtensionOffsetLabel = EditorGUIUtility.TrTextContent("Arm Extension Offset", "Offset applied to the elbow position as the controller is rotated upwards");
            public static GUIContent elbowBendRatioLabel = EditorGUIUtility.TrTextContent("Elbow Bend Ratio", "Amount of the controller's rotation to apply to the elbow");

            public static GUIContent elbowRestPositionLabel = EditorGUIUtility.TrTextContent("Elbow","The Elbow's Position relative to the users head");
            public static GUIContent wristRestPositionLabel = EditorGUIUtility.TrTextContent("Wrist","The Wrist's Position relative to the users head");
            public static GUIContent controllerRestPositionLabel = EditorGUIUtility.TrTextContent("Controller", "The Controller position relative to the users head");

            public static GUIContent restPositionLabel = EditorGUIUtility.TrTextContent("Rest Position");
        }
         
        protected SerializedProperty m_PoseSourceProp = null;
        protected SerializedProperty m_HeadGameObjectProp = null;    
        protected SerializedProperty m_IsLockedToNeckProp = null;    
        protected SerializedProperty m_ArmExtensionOffsetProp = null;
        protected SerializedProperty m_EblowRestPositionProp = null;
        protected SerializedProperty m_WristRestPositionProp = null;
        protected SerializedProperty m_ControllerRestPositionProp = null;
        protected SerializedProperty m_ElbowBendRatioProp = null;

        protected bool m_ExpandRestPosition = false;
        
        protected virtual void OnEnable()
        {
        
            m_PoseSourceProp = this.serializedObject.FindProperty("m_PoseSource");
            m_HeadGameObjectProp = this.serializedObject.FindProperty("m_HeadPoseSource");
            m_IsLockedToNeckProp = this.serializedObject.FindProperty("m_IsLockedToNeck");        
            m_ArmExtensionOffsetProp = this.serializedObject.FindProperty("m_ElbowRestPosition");
            m_EblowRestPositionProp = this.serializedObject.FindProperty("m_ElbowRestPosition");
            m_WristRestPositionProp = this.serializedObject.FindProperty("m_WristRestPosition");
            m_ControllerRestPositionProp = this.serializedObject.FindProperty("m_ControllerRestPosition");
            m_ElbowBendRatioProp = this.serializedObject.FindProperty("m_ElbowBendRatio");
        }

        

        public override void OnInspectorGUI()
        {
            serializedObject.Update();            
            EditorGUILayout.PropertyField(m_PoseSourceProp, ArmModelStyles.poseSourceLabel);
            EditorGUILayout.PropertyField(m_HeadGameObjectProp, ArmModelStyles.headPositionSourceLabel);
            EditorGUILayout.PropertyField(m_ArmExtensionOffsetProp, ArmModelStyles.armExtensionOffsetLabel);
            EditorGUILayout.PropertyField(m_ElbowBendRatioProp, ArmModelStyles.elbowBendRatioLabel);   
            EditorGUILayout.PropertyField(m_IsLockedToNeckProp, ArmModelStyles.isLockedToNeckLabel);         
            m_ExpandRestPosition = EditorGUILayout.Foldout(m_ExpandRestPosition,ArmModelStyles.restPositionLabel);
            if (m_ExpandRestPosition)
            {
                using (EditorGUI.IndentLevelScope indent = new EditorGUI.IndentLevelScope())
                {
                    EditorGUILayout.PropertyField(m_EblowRestPositionProp, ArmModelStyles.elbowRestPositionLabel);
                    EditorGUILayout.PropertyField(m_WristRestPositionProp, ArmModelStyles.wristRestPositionLabel);
                    EditorGUILayout.PropertyField(m_ControllerRestPositionProp, ArmModelStyles.controllerRestPositionLabel);
                }

            }
            serializedObject.ApplyModifiedProperties();            
        }
    }
}
 