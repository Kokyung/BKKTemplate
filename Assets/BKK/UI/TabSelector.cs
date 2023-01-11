using System.Collections;
using System.Collections.Generic;
using BKK.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TabSelector : MonoBehaviour
{
    public UnityEvent onSubmit = new UnityEvent();

    protected virtual void Update()
    {
        KeyboardInput<InputField, TMP_InputField>();
    }

    protected void KeyboardInput<T>() where T : Selectable
    {
        if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
        {
            // Tab + LeftShift는 위의 Selectable를 선택
            UIUtility.SelectUp<T>();
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Tab은 아래의 Selectable를 선택
            UIUtility.SelectDown<T>();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            onSubmit?.Invoke();
        }
    }

    protected void KeyboardInput<T1, T2>()
        where T1 : Selectable
        where T2 : Selectable
    {
        if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
        {
            // Tab + LeftShift는 위의 Selectable를 선택
            UIUtility.SelectUp<T1, T2>();
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Tab은 아래의 Selectable를 선택
            UIUtility.SelectDown<T1, T2>();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            onSubmit?.Invoke();
        }
    }
    
    protected void KeyboardInput<T1, T2, T3>()
        where T1 : Selectable
        where T2 : Selectable
        where T3 : Selectable
    {
        if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
        {
            // Tab + LeftShift는 위의 Selectable를 선택
            UIUtility.SelectUp<T1, T2, T3>();
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Tab은 아래의 Selectable를 선택
            UIUtility.SelectDown<T1, T2, T3>();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            onSubmit?.Invoke();
        }
    }
}
