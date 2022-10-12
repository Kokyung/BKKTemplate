using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

[RequireComponent(typeof(InputSystemUIInputModule))]
public class BKKInputSystem : MonoBehaviour
{
    public static BKKInputs inputs;

    public static bool Initialized { get; private set; }

    private void Awake()
    {
        inputs ??= new BKKInputs();

        Initialized = true;
    }

    private void OnEnable()
    {
        inputs?.Enable();
    }

    private void OnDisable()
    {
        inputs?.Disable();
    }
}
