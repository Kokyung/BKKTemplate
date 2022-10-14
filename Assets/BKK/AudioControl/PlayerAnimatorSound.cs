using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class PlayerAnimatorSound : MonoBehaviour
{
    private AudioSource source;
    
    public AudioPreset jump = new AudioPreset(null, "jump2", 0, 0.5f);
    
    public AudioPreset step = new AudioPreset(null, "footstep", 0, 0.15f);
    
    public UnityEvent onFootStep = new UnityEvent();
    public UnityEvent onJump = new UnityEvent();
    
    public float checkWalkWeight = 0.3f;
    public float checkRunWeight = 0.3f; 
    public float checkJumpWeight = 0.3f;
    public float checkJumpRunWeight = 0.1f;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
    }

    public void Step_Walk(AnimationEvent evt)
    {
        if (!this.enabled) return;
        
        if (evt.animatorClipInfo.weight < checkWalkWeight) return;
        
        onFootStep?.Invoke();
        StepSound();
    }

    public void Step_Run(AnimationEvent evt)
    {
        if (!this.enabled) return;

        if (evt.animatorClipInfo.weight < checkRunWeight) return;
        
        onFootStep?.Invoke();
        StepSound();
    }

    public void Jump(AnimationEvent evt)
    {
        if (!this.enabled) return;

        if (evt.animatorClipInfo.weight < checkJumpWeight) return;
        
        onJump?.Invoke();
        JumpSound();
    }
    
    public void Jump_Run(AnimationEvent evt)
    {
        if (!this.enabled) return;

        if (evt.animatorClipInfo.weight < checkJumpRunWeight) return;
        
        onJump?.Invoke();
        JumpSound();
    }

    private void JumpSound()
    {
        StartCoroutine(Co_PlaySound(jump));
    }
    
    private void StepSound()
    {
        StartCoroutine(Co_PlaySound(step));
    }

    private IEnumerator Co_PlaySound(AudioPreset preset)
    {
        yield return new WaitForSeconds(preset.playAfter);

        source.PlayOneShot(preset.clip, preset.volume);
    }
}

[System.Serializable]
public class AudioPreset
{
    public AudioClip clip;
    public string clipName;
    public float playAfter = 0.0f;
    public float volume = 0.5f;

    public AudioPreset(AudioClip _clip, string _clipName, float _playAfter, float _volume)
    {
        clip = _clip;
        clipName = _clipName;
        playAfter = _playAfter;
        volume = _volume;
    }
}
