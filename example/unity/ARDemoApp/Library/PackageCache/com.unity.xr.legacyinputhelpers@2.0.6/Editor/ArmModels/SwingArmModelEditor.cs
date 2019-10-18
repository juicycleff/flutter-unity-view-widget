using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.LegacyInputHelpers;

namespace UnityEditor.XR.LegacyInputHelpers
{
    [CustomEditor(typeof(SwingArmModel))]
    internal class SwingArmModelEditor : ArmModelEditor
    {

        protected static class SwingArmModelStyles
        {
            public static GUIContent rotationRatioLabel = EditorGUIUtility.TrTextContent("Rotation Ratio");
            public static GUIContent shoulderRotationRatioLabel = EditorGUIUtility.TrTextContent("Shoulder", "Portion of controller rotation applied to the shoulder joint");
            public static GUIContent elbowRotationRatioLabel = EditorGUIUtility.TrTextContent("Elbow", "Portion of controller rotation applied to the elbow joint");
            public static GUIContent wristRotationRatioLabel = EditorGUIUtility.TrTextContent("Wrist", "Portion of controller rotation applied to the wrist joint");

            public static GUIContent shiftedRotationRatioLabel = EditorGUIUtility.TrTextContent("Shifted Rotation Ratio");
            public static GUIContent shiftedShoulderRotationRatioLabel = EditorGUIUtility.TrTextContent("Shifted Shoulder","Portion of controller rotation applied to the shoulder joint when the controller is backwards");
            public static GUIContent shiftedElbowRotationRatioLabel = EditorGUIUtility.TrTextContent("Shifted Elbow", "Portion of controller rotation applied to the elbow joint when the controller is backwards");
            public static GUIContent shiftedWristRotationRatioLabel = EditorGUIUtility.TrTextContent("Shifted Wrist", "Portion of controller rotation applied to the wrist joint when the controller is backwards");

            public static GUIContent jointShiftAngleLabel = EditorGUIUtility.TrTextContent("Joint Shift Angle", "The min/max angle of the controller before starting to lerp towards the shifted joint ratios");
            public static GUIContent jointShiftExponentLabel = EditorGUIUtility.TrTextContent("Joint Shift Exponent", "Exponent applied to the joint shift ratio to control the curve of the shift");
        }

        SerializedProperty m_ShoulderRotationRatioProp = null;
        SerializedProperty m_EblowRotationRatioProp = null;
        SerializedProperty m_WristRotationRatioProp = null;
        SerializedProperty m_ShiftedShoulderRotationRatioProp = null;
        SerializedProperty m_ShiftedEblowRotationRatioProp = null;
        SerializedProperty m_ShiftedWristRotationRatioProp = null;
        SerializedProperty m_JointShiftAngleProp = null;
        SerializedProperty m_JointShiftExponentProp = null;

        bool m_ExpandShoulder = false;
        bool m_ExpandShiftedShoulder = false;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_ShoulderRotationRatioProp = this.serializedObject.FindProperty("m_ShoulderRotationRatio");
            m_EblowRotationRatioProp = this.serializedObject.FindProperty("m_ElbowRotationRatio");
            m_WristRotationRatioProp = this.serializedObject.FindProperty("m_WristRotationRatio");
            m_ShiftedShoulderRotationRatioProp = this.serializedObject.FindProperty("m_ShiftedShoulderRotationRatio");
            m_ShiftedEblowRotationRatioProp = this.serializedObject.FindProperty("m_ShiftedElbowRotationRatio");
            m_ShiftedWristRotationRatioProp = this.serializedObject.FindProperty("m_ShiftedWristRotationRatio");
            m_JointShiftAngleProp = this.serializedObject.FindProperty("m_JointShiftAngle");
            m_JointShiftExponentProp = this.serializedObject.FindProperty("m_JointShiftExponent");
        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();            
            EditorGUILayout.PropertyField(m_PoseSourceProp, ArmModelStyles.poseSourceLabel);
            EditorGUILayout.PropertyField(m_HeadGameObjectProp, ArmModelStyles.headPositionSourceLabel);
            EditorGUILayout.PropertyField(m_ArmExtensionOffsetProp, ArmModelStyles.armExtensionOffsetLabel);
            EditorGUILayout.PropertyField(m_JointShiftAngleProp, SwingArmModelStyles.jointShiftAngleLabel);
            EditorGUILayout.PropertyField(m_JointShiftExponentProp, SwingArmModelStyles.jointShiftExponentLabel);
            EditorGUILayout.PropertyField(m_ElbowBendRatioProp, ArmModelStyles.elbowBendRatioLabel);
            EditorGUILayout.PropertyField(m_IsLockedToNeckProp, ArmModelStyles.isLockedToNeckLabel);         
          
            m_ExpandRestPosition = EditorGUILayout.Foldout(m_ExpandRestPosition, ArmModelStyles.restPositionLabel);
            if (m_ExpandRestPosition)
            {
                using (EditorGUI.IndentLevelScope indent = new EditorGUI.IndentLevelScope())
                {
                    EditorGUILayout.PropertyField(m_EblowRestPositionProp, ArmModelStyles.elbowRestPositionLabel);
                    EditorGUILayout.PropertyField(m_WristRestPositionProp, ArmModelStyles.wristRestPositionLabel);
                    EditorGUILayout.PropertyField(m_ControllerRestPositionProp, ArmModelStyles.controllerRestPositionLabel);
                }

            }            
            m_ExpandShoulder = EditorGUILayout.Foldout(m_ExpandShoulder, SwingArmModelStyles.rotationRatioLabel);
            if (m_ExpandShoulder)
            {
                using (EditorGUI.IndentLevelScope indent = new EditorGUI.IndentLevelScope())
                {
                    EditorGUILayout.PropertyField(m_ShoulderRotationRatioProp, SwingArmModelStyles.shoulderRotationRatioLabel);
                    EditorGUILayout.PropertyField(m_EblowRotationRatioProp, SwingArmModelStyles.elbowRotationRatioLabel);
                    EditorGUILayout.PropertyField(m_WristRotationRatioProp, SwingArmModelStyles.wristRotationRatioLabel);
                }
            }            
            m_ExpandShiftedShoulder = EditorGUILayout.Foldout(m_ExpandShiftedShoulder, SwingArmModelStyles.shiftedRotationRatioLabel);
            if (m_ExpandShiftedShoulder)
            {
                using (EditorGUI.IndentLevelScope indent = new EditorGUI.IndentLevelScope())
                {
                    EditorGUILayout.PropertyField(m_ShiftedShoulderRotationRatioProp, SwingArmModelStyles.shiftedShoulderRotationRatioLabel);
                    EditorGUILayout.PropertyField(m_ShiftedEblowRotationRatioProp, SwingArmModelStyles.shiftedElbowRotationRatioLabel);
                    EditorGUILayout.PropertyField(m_ShiftedWristRotationRatioProp, SwingArmModelStyles.shiftedWristRotationRatioLabel);
                }
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}