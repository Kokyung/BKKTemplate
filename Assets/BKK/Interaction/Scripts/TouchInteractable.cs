using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum InteractionCheck
{
    [InspectorName("없음")] None,
    [InspectorName("플레이어 거리")] PlayerDistance,
    [InspectorName("카메라 거리")] CameraDistance,
    [InspectorName("상호작용 영역")] Collision,
    [InspectorName("플레이어 거리 + 상호작용 영역")] PlayerDistanceAndCollision,
    [InspectorName("카메라 거리 + 상호작용 영역")] CameraDistanceAndCollision,
}

/// <summary>
/// 변고경 BKK
/// 콜라이더가 있는 게임 오브젝트를 터치시 등록한 이벤트를 호출해주는 컴포넌트입니다.
/// </summary>
[RequireComponent(typeof(Collider))]
public class TouchInteractable : MonoBehaviour
{
    [SerializeField] private UnityEvent onTouch;
    public float onTouchTiming = 0;
    public float onTouchDelay = 1;
    private bool delayed = false;

    public InteractionCheck interactionCheck = InteractionCheck.PlayerDistance;
    public float interactDistance = 8f;
    public InteractionArea interactionArea;

    public bool CheckInteraction(float distance = 0)
    {
        switch (interactionCheck)
        {
            case InteractionCheck.None:
                return true;
            case InteractionCheck.PlayerDistance:
                if (!MyPlayer.Instance)
                {
                    Debug.LogError("MyPlayer 컴포넌트가 존재하지 않습니다.");
                    return false;
                }
                if (Vector3.Distance(MyPlayer.Instance.transform.position, transform.position) <= interactDistance) return true;
                break;
            case InteractionCheck.CameraDistance:
                if (distance <= interactDistance) return true;
                break;
            case InteractionCheck.Collision:
                if (!interactionArea)
                {
                    Debug.LogError($"{this.gameObject.name}의 Touch Interactable에 참조된 Interaction Area가 존재하지 않습니다.");
                    return false;
                }
                if (interactionArea.entered) return true;
                break;
            case InteractionCheck.PlayerDistanceAndCollision:
                if (!interactionArea)
                {
                    Debug.LogError($"{this.gameObject.name}의 Touch Interactable에 참조된 Interaction Area가 존재하지 않습니다.");
                    return false;
                }
                if (!MyPlayer.Instance)
                {
                    Debug.LogError("MyPlayer 컴포넌트가 존재하지 않습니다.");
                    return false;
                }
                if (Vector3.Distance(MyPlayer.Instance.transform.position, transform.position) <= interactDistance && (interactionArea.entered)) return true;
                break;
            case InteractionCheck.CameraDistanceAndCollision:
                if (!interactionArea)
                {
                    Debug.LogError($"{this.gameObject.name}의 Touch Interactable에 참조된 Interaction Area가 존재하지 않습니다.");
                    return false;
                }
                if ((distance <= interactDistance) && (interactionArea.entered)) return true;
                break;
            default:
                return false;
        }

        return false;
    }

    public void Interact()
    {
        StartCoroutine(Co_Interact());
    }

    IEnumerator Co_Interact()
    {
        if (delayed) yield break;
        yield return new WaitForSeconds(onTouchTiming);
        onTouch.Invoke();
        delayed = true;
        yield return new WaitForSeconds(onTouchDelay);
        delayed = false;
    }

    public void AddEvent(UnityAction action)
    {
        onTouch.AddListener(action);
    }

    public void RemoveEvent(UnityAction action)
    {
        onTouch.RemoveListener(action);
    }

    public void RemoveAllEvent()
    {
        onTouch.RemoveAllListeners();
    }

    public void StopAllEvent()
    {
        StopAllCoroutines();
    }
}
