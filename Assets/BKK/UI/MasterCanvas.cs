using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BKK.UI
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(CanvasScaler))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(SafeAreaSetter))]
    [RequireComponent(typeof(CanvasScalerCalculator))]
    public sealed class MasterCanvas : MonoBehaviour
    {
        private Canvas _canvas;
        private SafeAreaSetter _safeAreaSetter;

        public CanvasGroup _canvasGroup;

        public bool manuallySetVisible = false;

        public int controllerSiblingIndex = 0;

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            if (!manuallySetVisible) SetMasterCanvasVisible(true);
        }

        private void Init()
        {
            GetCanvasComponents();

            SetMasterCanvasVisible(false);
        }
        
        private void GetCanvasComponents()
        {
            if (!_canvas) _canvas = GetComponent<Canvas>();
            if (!_canvasGroup) _canvasGroup = GetComponent<CanvasGroup>();
            if (!_safeAreaSetter) _safeAreaSetter = GetComponent<SafeAreaSetter>();
        }

        public void SetController(bool enable)
        {
            SetServantUIVisible(controllerSiblingIndex, enable);
        }

        public void SetMasterCanvasVisible(bool enable)
        {
            _canvasGroup.alpha = enable ? 1 : 0;
            _canvasGroup.interactable = enable;
        }
        
        public void SetServantUIVisible(int siblingIndex, bool enable)
        {
            var child = transform.GetChild(siblingIndex).GetComponent<CanvasGroup>();
            if (child != null)
            {
                child.interactable = enable;
                child.alpha = enable ? 1 : 0;
            }
            else
            {
                Debug.Log($"{siblingIndex}번째 Child에 CanvasGroup이 존재하지 않습니다.");
            }
        }
    }
}
