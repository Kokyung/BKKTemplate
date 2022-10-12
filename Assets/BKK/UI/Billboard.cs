using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BKK.UI
{
    /// <summary>
    /// UI 빌보드 컴포넌트입니다. 
    /// 
    /// 작성자: 변고경
    /// 작성일자: 2021/8/5
    /// </summary>
    public sealed class Billboard : MonoBehaviour
    {
        [HideInInspector] public Camera cam;

        [Tooltip("회전할 축")] public RotationOnly rotationOnly = RotationOnly.None;

        private Vector3 v;
    
        private void Update()
        {
            Look();
        }

        /// <summary>
        /// 메인 카메라(플레이어)를 바라본다.
        /// </summary>
        private void Look()
        {
            if (cam == null)
            {
                if (Camera.main == null)
                {
                    Debug.LogWarning("빌보드: 메인 카메라가 존재하지 않습니다.");
                    return;
                }
                
                else cam = Camera.main;
            }

            v = cam.transform.position - transform.position;

            switch (rotationOnly)
            {
                case RotationOnly.X:
                    v.y = 0;
                    v.z = 0;
                    break;
                case RotationOnly.Y:
                    v.x = 0;
                    v.z = 0;
                    break;
                case RotationOnly.Z:
                    v.x = 0;
                    v.y = 0;
                    break;
                case RotationOnly.None:
                    v = Vector3.zero;
                    break;
                default:
                    v = Vector3.zero;
                    break;
            }

            transform.LookAt(cam.transform.position - v);
        }
    }

    public enum RotationOnly
    {
        None,
        X,
        Y,
        Z
    }
}