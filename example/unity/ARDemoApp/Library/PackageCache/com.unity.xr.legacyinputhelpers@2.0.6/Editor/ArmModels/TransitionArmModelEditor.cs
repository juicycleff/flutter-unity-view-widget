using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.LegacyInputHelpers;

namespace UnityEditor.XR.LegacyInputHelpers
{
      
    [CustomEditor(typeof(TransitionArmModel))]
    internal class TransitionArmModelEditor : Editor
    {
        static class Styles
        {
            public static GUIContent poseSourceLabel = EditorGUIUtility.TrTextContent("Angular Velocity Source", "The source of angular velocity which is used to transition to queued arm models");
            public static GUIContent armModelSourceLabel = EditorGUIUtility.TrTextContent("Current Arm Model", "The current arm model ");
            public static GUIContent armModelTransitions = EditorGUIUtility.TrTextContent("Configuration", "Arm models that the transition arm model can blend to when receiving the event corresponding to the transition");
        }

        SerializedProperty m_PoseSourceProp = null;
        SerializedProperty m_ArmModelProp = null;
        SerializedProperty m_ArmModelTransitions = null;

        void OnEnable()
        {
            m_PoseSourceProp = this.serializedObject.FindProperty("m_PoseSource");
            m_ArmModelProp = this.serializedObject.FindProperty("m_CurrentArmModelComponent");
            m_ArmModelTransitions = this.serializedObject.FindProperty("m_ArmModelTransitions");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_PoseSourceProp, Styles.poseSourceLabel);
            EditorGUILayout.PropertyField(m_ArmModelProp, Styles.armModelSourceLabel);
            EditorGUILayout.PropertyField(m_ArmModelTransitions, Styles.armModelTransitions,true);            
        
            serializedObject.ApplyModifiedProperties();            
        }
    }
}
