using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// 변고경 BKK
/// 상호작용 가능한 영역을 설정해주는 컴포넌트입니다.
/// </summary>
[RequireComponent(typeof(Collider))]
public class InteractionArea : MonoBehaviour
{
    public UnityEvent _ontriggerEnter;
    public UnityEvent _ontriggerExit;

    public string CollisionTag = "Player";

    public bool entered = false;

    public bool playOnce = false;

    private bool checkPlayOnce = false;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playOnce)
        {
            if(checkPlayOnce) return;
            checkPlayOnce = true;
        }
        
        if (other.tag == CollisionTag)
        {
            _ontriggerEnter?.Invoke();
            entered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == CollisionTag)
        {
            _ontriggerExit?.Invoke();
            entered = false;
        }
    }
}

