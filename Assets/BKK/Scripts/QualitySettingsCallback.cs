using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QualitySettingsCallback : MonoBehaviour
{
    /// <summary>
    /// 퀄리티 레벨이 변경되었을때 호출합니다.<br /><br />
    /// 첫번째 매개변수: 변경된 레벨
    /// </summary>
    public static UnityEvent<int> onQualityLevelChanged = new UnityEvent<int>();

    private int beforeLevel = 0;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initializer()
    {
        var go = new GameObject($"[{nameof(QualitySettingsCallback)}]", typeof(QualitySettingsCallback))
        {
            hideFlags = HideFlags.HideInHierarchy
        };
        
        DontDestroyOnLoad(go);
    }

    private void Awake()
    {
        beforeLevel = QualitySettings.GetQualityLevel();
    }

    private void FixedUpdate()
    {
        var currentLevel = QualitySettings.GetQualityLevel();
        if (beforeLevel != currentLevel)
        {
            onQualityLevelChanged.Invoke(currentLevel);
            beforeLevel = currentLevel;
        }
    }
}
