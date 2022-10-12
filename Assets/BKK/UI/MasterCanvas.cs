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
    [RequireComponent(typeof(MobileSafeAreaSetting))]
    [RequireComponent(typeof(CanvasScalerCalculator))]
    public sealed class MasterCanvas : MonoBehaviour
    {
        private Canvas _canvas;

        public CanvasGroup _canvasGroup;

        public bool setVisibleOnStart = true;

        public int controllerSiblingIndex = 0;

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            SetMasterCanvasVisible(setVisibleOnStart);
        }

        private void Init()
        {
            GetCanvasComponents();
        }
        
        private void GetCanvasComponents()
        {
            if (!_canvas) _canvas = GetComponent<Canvas>();
            if (!_canvasGroup) _canvasGroup = GetComponent<CanvasGroup>();
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
            var child = transform.GetChild(siblingIndex);

            if (!child)
            {
                Debug.Log($"{siblingIndex}번째 Child가 존재하지 않습니다.");
                return;
            }
            
            var cg = child.GetComponent<CanvasGroup>();

            if (!cg)
            {
                Debug.Log($"{siblingIndex}번째 Child에 CanvasGroup이 존재하지 않습니다.");
                return;
            }
            
            cg.interactable = enable;
            cg.alpha = enable ? 1 : 0;
        }
    }
}
