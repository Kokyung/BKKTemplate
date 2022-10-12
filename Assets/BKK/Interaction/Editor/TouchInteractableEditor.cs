using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BKK
{
    [CustomEditor(typeof(TouchInteractable))]
    public class TouchInteractableEditor : Editor
    {
        private TouchInteractable interactable;

        private bool showOntouch = true;

        private SerializedProperty _onTouch;
        private SerializedProperty _onTouchTiming;
        private SerializedProperty _onTouchDelay;
        private SerializedProperty _interactionCheck;
        private SerializedProperty _interactDistance;
        private SerializedProperty _interactionArea;

        private InteractionCheck check;

        private GUIContent _onTouchLabel;
        private GUIContent _onTouchTimingLabel;
        private GUIContent _onTouchDelayLabel;
        private GUIContent _interactionCheckLabel;
        private GUIContent _interactDistanceLabel;
        private GUIContent _interactionAreaLabel;

        private void OnEnable()
        {
            interactable = (TouchInteractable) target;

            _onTouch = serializedObject.FindProperty("onTouch");
            _onTouchTiming = serializedObject.FindProperty("onTouchTiming");
            _onTouchDelay = serializedObject.FindProperty("onTouchDelay");
            _interactionCheck = serializedObject.FindProperty("interactionCheck");
            _interactDistance = serializedObject.FindProperty("interactDistance");
            _interactionArea = serializedObject.FindProperty("interactionArea");

            _onTouchLabel = new GUIContent("OnTouch 이벤트");
            _onTouchTimingLabel = new GUIContent("OnTouch 타이밍");
            _onTouchDelayLabel = new GUIContent("OnTouch 딜레이");
            _interactionCheckLabel = new GUIContent("상호작용 체크 방식");
            _interactDistanceLabel = new GUIContent("상호작용 거리");
            _interactionAreaLabel = new GUIContent("상호작용 영역");
        }

        public override void OnInspectorGUI()
        {
            showOntouch = EditorGUILayout.Foldout(showOntouch, "터치 이벤트");

            if (showOntouch)
            {
                EditorGUILayout.PropertyField(_onTouch, _onTouchLabel);
                EditorGUILayout.PropertyField(_onTouchTiming, _onTouchTimingLabel);
                EditorGUILayout.PropertyField(_onTouchDelay, _onTouchDelayLabel);
            }
            
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_interactionCheck, _interactionCheckLabel);
            switch (_interactionCheck.enumValueIndex)
            {
                case (int)InteractionCheck.PlayerDistance:
                    EditorGUILayout.PropertyField(_interactDistance, _interactDistanceLabel);
                    break;
                case (int)InteractionCheck.CameraDistance:
                    EditorGUILayout.PropertyField(_interactDistance, _interactDistanceLabel);
                    break;
                case (int)InteractionCheck.Collision:
                    EditorGUILayout.PropertyField(_interactionArea, _interactionAreaLabel);
                    break;
                case (int)InteractionCheck.PlayerDistanceAndCollision:
                    EditorGUILayout.PropertyField(_interactDistance, _interactDistanceLabel);
                    EditorGUILayout.PropertyField(_interactionArea, _interactionAreaLabel);
                    break;
                case (int)InteractionCheck.CameraDistanceAndCollision:
                    EditorGUILayout.PropertyField(_interactDistance, _interactDistanceLabel);
                    EditorGUILayout.PropertyField(_interactionArea, _interactionAreaLabel);
                    break;
                default:
                    break;
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
