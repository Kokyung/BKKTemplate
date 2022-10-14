using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ParticleSound : MonoBehaviour
{
    private AudioSource source;

    [SerializeField] private AudioClip clip;
    
    private void Awake()
    {
        Init();
    }

    private void OnValidate()
    {
        Init();
    }

    private void Init()
    {
        if(!source) source = GetComponent<AudioSource>();
        source.loop = false;
        source.spatialBlend = 1f;
        source.playOnAwake = false;
    }

    public void Play()
    {
        source.PlayOneShot(clip);
    }

    public void Play(AudioClip _clip)
    {
        if (!_clip) return;
        
        source.PlayOneShot(_clip);
    }
}
