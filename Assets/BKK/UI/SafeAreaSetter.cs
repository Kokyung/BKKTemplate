using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BKK.UI
{
    /// <summary>
    /// 모바일 디바이스의 Safe Area에 맞춰 UI를 조정해주는 컴포넌트입니다.
    /// </summary>
    public sealed class SafeAreaSetter : MonoBehaviour
    {
        private Canvas canvas;
        private RectTransform panelSafeArea;
        private Rect currentSafeArea = new Rect();
        private ScreenOrientation currentOrientation = ScreenOrientation.AutoRotation;
        
        private void Start()
        {
            Init();
        }

        private void OnValidate()
        {
            Init();
        }

        private void Init()
        {
            if (!canvas) canvas = GetComponent<Canvas>();
            if (!panelSafeArea) panelSafeArea = GetComponent<RectTransform>();

            ApplySafeArea();
        }

        private void ApplySafeArea()
        {
            if (!Application.isMobilePlatform) return;
            
            if (currentOrientation == Screen.orientation && currentSafeArea == Screen.safeArea) return;
            
            currentOrientation = Screen.orientation;
            currentSafeArea = Screen.safeArea;
            
            if (panelSafeArea == null)
            {
                return;
            }

            Rect safeArea = Screen.safeArea;

            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= canvas.pixelRect.width;
            anchorMin.y /= canvas.pixelRect.height;

            anchorMax.x /= canvas.pixelRect.width;
            anchorMax.y /= canvas.pixelRect.height;

            panelSafeArea.anchorMin = anchorMin;
            panelSafeArea.anchorMax = anchorMax;
        }
    }
}
