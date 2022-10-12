using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Monobehaviour를 상속받지 않은 클래스에서 코루틴을 호출할 수 있도록 해주는 클래스입니다.
///
/// 작성자: 변고경
/// </summary>
public sealed class CoroutineHelper : MonoBehaviour
{
    private static MonoBehaviour monoInstance;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initializer()
    {
        var go = new GameObject($"[{nameof(CoroutineHelper)}]", typeof(CoroutineHelper));
        go.hideFlags = HideFlags.HideInHierarchy;
        monoInstance = go.GetComponent<CoroutineHelper>();
        DontDestroyOnLoad(monoInstance.gameObject);
    }
    
    public new static Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return monoInstance.StartCoroutine(coroutine);
    }
    
    public new static void StopCoroutine(Coroutine coroutine)
    {
        monoInstance.StopCoroutine(coroutine);
    }

    public new static void StopAllCoroutines()
    {
        monoInstance.StopAllCoroutines();
    }
}
