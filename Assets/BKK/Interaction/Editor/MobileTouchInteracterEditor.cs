using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BKK
{
    [CustomEditor(typeof(MobileTouchInteracter))]
    public class MobileTouchInteracterEditor : Editor
    {
        private MobileTouchInteracter _interacter;

        //private SerializedProperty _eventChecker;
        private SerializedProperty _triggerInteraction;
        private SerializedProperty _ignoreLayerMask;
        private SerializedProperty _collideMaxDistance;

        private GUIContent _eventCheckerLabel;
        private GUIContent _triggerInteractionLabel;
        private GUIContent _ignoreLayerMaskLabel;
        private GUIContent _collideMaxDistanceLabel;
        
        private void OnEnable()
        {
            //_eventChecker = serializedObject.FindProperty("eventChecker");
            _triggerInteraction = serializedObject.FindProperty("triggerInteraction");
            _ignoreLayerMask = serializedObject.FindProperty("ignoreLayerMask");
            _collideMaxDistance = serializedObject.FindProperty("collideMaxDistance");

            _eventCheckerLabel = new GUIContent("UI Event Checker");
            _triggerInteractionLabel = new GUIContent("트리거 상호작용 여부");
            _ignoreLayerMaskLabel = new GUIContent("무시할 레이어마스크");
            _collideMaxDistanceLabel = new GUIContent("최대 상호작용 거리");
        }

        public override void OnInspectorGUI()
        {
            //EditorGUILayout.PropertyField(_eventChecker, _eventCheckerLabel);
            EditorGUILayout.PropertyField(_triggerInteraction, _triggerInteractionLabel);
            EditorGUILayout.PropertyField(_ignoreLayerMask, _ignoreLayerMaskLabel);
            EditorGUILayout.PropertyField(_collideMaxDistance, _collideMaxDistanceLabel);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
