using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 파티클 순차 처리해주는 컴포넌트입니다.
///
/// 작성자: 변고경
/// </summary>
public class ParticleSequencer : MonoBehaviour
{
    [Tooltip("파티클 시퀀스 리스트")]
    public List<ParticleSequence> sequences = new List<ParticleSequence>();

    [Tooltip("Play() 호출시 무한 반복 여부")]
    public bool loop;
    
    private void Awake()
    {
        Stop();
    }

    /// <summary>
    /// 파티클 시퀀스 재생
    /// </summary>
    public void Play()
    {
        StartCoroutine(Co_Play());
    }

    /// <summary>
    /// 파티클 시퀀스 정지
    /// </summary>
    public void Stop()
    {
        if (loop)
        {
            Debug.LogWarning("루프를 해제하고 정지해주세요.");
        }
        
        foreach (var sequence in sequences)
        {
            sequence.particle.Stop();
        }
    }
    
    /// <summary>
    /// 파티클 시퀀스 일시정지
    /// </summary>
    public void Pause()
    {
        foreach (var sequence in sequences)
        {
            sequence.particle.Pause();
        }
    }
    
    private IEnumerator Co_Play()
    {
        do
        {
            foreach (var sequence in sequences)
            {
                sequence.onSequenceStart.Invoke();
                yield return new WaitForSeconds(sequence.beforePlayDelay);
                sequence.particle.Play();
                yield return new WaitForSeconds(sequence.afterPlayDelay);
                sequence.onSequenceEnd.Invoke();
            }
        } while (loop);
    }
}

[System.Serializable]
public class ParticleSequence
{
    [Tooltip("재생할 파티클")] public ParticleSystem particle;
    [Tooltip("파티클 재생 전 딜레이")] public float beforePlayDelay;
    [Tooltip("파티클 재생 후 다음 시퀀스까지 딜레이")] public float afterPlayDelay;

    [Tooltip("파티클 재생 전 이벤트")] public UnityEvent onSequenceStart = new UnityEvent();
    [Tooltip("파티클 재생 후 이벤트")] public UnityEvent onSequenceEnd = new UnityEvent();

    public ParticleSequence(ParticleSystem _particle, float _beforePlayDelay = 0, float _afterPlayDelay = 0,
        UnityAction _onSequenceStart = null, UnityAction _onSequenceEnd = null)
    {
        particle = _particle;
        beforePlayDelay = _beforePlayDelay;
        afterPlayDelay = _afterPlayDelay;
        onSequenceStart.AddListener(_onSequenceStart);
        onSequenceEnd.AddListener(_onSequenceEnd);
    }
}