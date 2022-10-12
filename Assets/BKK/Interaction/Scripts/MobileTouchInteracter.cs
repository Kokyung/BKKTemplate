using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


    /// <summary>
    /// Touch Interactable 컴포넌트가 있는 게임 오브젝트와 상호작용 할 수 있게 해주는 컴포넌트입니다.
    /// BKK
    /// 작성자: 변고경
    /// 작성일자: 2021/8/5
    /// </summary>
    public class MobileTouchInteracter : BaseTouchInteracter
    {
        protected override void Awake()
        {
            base.Awake();
            triggerInteraction = QueryTriggerInteraction.Ignore;
        }

        protected override void OnClickPerformed(InputAction.CallbackContext callback)
        {
            if(!IsPointerOverUI()) Interact();
        }

        /// <summary>
        /// Touch Interactable과 상호작용
        /// </summary>
        private void Interact()
        {
            var hit = GetWorldPositionHitData((Vector2)pointPos);

            if (hit.collider == null) return;
            if (hit.collider.TryGetComponent(out TouchInteractable interactable))
            {
                if (interactable.CheckInteraction(hit.distance))
                {
                    interactable.Interact();
                }
            }
        }
    }

