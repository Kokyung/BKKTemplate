using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 캔버스가 가려졌을때 캔버스 그룹의 Interactable을 비활성화 해주는 컴포넌트
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
[DisallowMultipleComponent]
public class PanelVisibility : MonoBehaviour
{
    [SerializeField] MaskableGraphic maskable;
    [SerializeField] private CanvasGroup cg;// 타겟 캔버스 그룹 
 
    void Awake()
    {
        maskable.onCullStateChanged.AddListener(CullStateChanged);// MaskableGraphic의 컬링 상태 변화 체크
    }

    private void OnValidate()
    {
        if(!cg) cg = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// MaskableGraphic의 컬링 상태 체크해서 보이면 활성화, 보이지 않으면 비활성화한다.
    /// </summary>
    /// <param name="hidden"></param>
    private void CullStateChanged(bool hidden)
    {
        cg.interactable = !hidden;
    }
}
