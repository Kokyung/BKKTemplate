using System;
using System.Collections;
using System.Collections.Generic;
using BKK.Tools;
using BKK.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace BKK
{
    public class NonePlayableCharacter : MonoBehaviour
    {
        [Header("캐릭터 애니메이션")]
        [SerializeField] private Animator animator;
    
        public bool loopAnimation;
        public string touchParameterValue = "Emote01_Hi";
        public string endParameterValue = "Emote01_Hi";
        public string loopParameterValue = "Emote10_Cute";
        [Range(0.00f, 1.00f)] public float touchDelay = 1f;
        [Range(0.00f, 1.00f)] public float endDelay = 1f;
        public float loopDelay = 10f;

        [SerializeField] private UnityEvent onLoopAnimation = new UnityEvent();
    
        private bool touched = false;
        private bool ended = false;
        private Coroutine touchRoutine;
        private Coroutine loopRoutine;
    
        [Header("사운드")] 
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private List<AudioClip> clips;

        public PlayOrder clipPlayOrder = PlayOrder.Random;

        [Header("상호작용")]
        [SerializeField] private TouchInteractable interactable;

        [SerializeField] private UnityEvent onTouchNPC = new UnityEvent();
        [SerializeField] private UnityEvent onEndNPC = new UnityEvent();

        [Header("말풍선 대사")] 
        [SerializeField] private Transform balloonPrefab;
        [SerializeField] private Transform balloonTarget;
        [SerializeField] private TMP_Text balloonText;

        public Vector3 balloonPositionOffset;

        public List<string> dialogueList = new List<string>();

        public PlayOrder dialogueOrder = PlayOrder.Descending;

        public DialoguePop dialoguePop = DialoguePop.Always;

        public TouchInteractable Interactable { get => interactable; }

        public bool LoopAnimation
        {
            get => loopAnimation;
            set
            {
                loopAnimation = value;
                if(loopAnimation) PlayLoopAnimation();
                else StopLoopAnimation();
            }
        }

        #region Monobehaviour

        private void Start()
        {
            Init();
        }

        private void OnEnable()
        {
            PlayLoopAnimation();
        }

        private void OnDisable()
        {
            StopLoopAnimation();
        }

        private void OnValidate()
        {
            if (!this.gameObject.activeInHierarchy) return;
        
            FindOrCreateComponents();
            LoopAnimation = loopAnimation;
        }

        #endregion

        #region Initialize

        private void Init()
        {
            if (interactable)
            {
                interactable.AddEvent(TouchNPC);
                interactable.AddEvent(PlaySound);
            }
            touched = false;
            ended = false;
        
            SetBalloonDialogue();
        }

        private void SetBalloonDialogue()
        {
            InstantiateBalloonDialogue();
        
            switch (dialoguePop)
            {
                case DialoguePop.Always:
                    break;
                case DialoguePop.OnTouch:
                
                
                
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void InstantiateBalloonDialogue()
        {
            if (!balloonPrefab) return;

            if(!balloonTarget) balloonTarget = 
                Instantiate(balloonPrefab, Vector3.zero + balloonPositionOffset, Quaternion.identity, this.transform);

            var billboard = balloonTarget.GetComponentInChildren<Billboard>();

            if (!billboard)
            {
                billboard = balloonTarget.gameObject.AddComponent<Billboard>();
                billboard.rotationOnly = RotationOnly.Y;
            }
        }

        private int i = 0;
    
        public void PlaySound()
        {
            if (!audioSource || clips.Count == 0) return;

            switch (clipPlayOrder)
            {
                case PlayOrder.Ascending:
                    if (i == 0) i = clips.Count;
                    audioSource.PlayOneShot(clips[i--]);
                    break;
                case PlayOrder.Descending:
                    if (i == clips.Count - 1) i = 0;
                    audioSource.PlayOneShot(clips[i++]);
                    break;
                case PlayOrder.Random:
                    Random.InitState((int) (Time.time * 100));
                    audioSource.PlayOneShot(clips[Random.Range(0, clips.Count - 1)]);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void FindOrCreateComponents()
        {
            if (!animator) animator = GetComponentInChildren<Animator>();
        
            interactable = GetComponentInChildren<TouchInteractable>();
            if (!interactable)
            {
                var go = new GameObject("TouchInteractable");
                go.transform.SetParent(this.transform);
                go.transform.SetSiblingIndex(0);
            
                var col = go.AddComponent<BoxCollider>();
                col.center = Vector3.up;
                col.size = Vector3.one + Vector3.up;
                col.isTrigger = true;
            
                interactable = go.AddComponent<TouchInteractable>();
            }

            audioSource = GetComponent<AudioSource>();

            if (!audioSource)
            {
                audioSource = this.gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.spatialBlend = 0.7f;
                audioSource.rolloffMode = AudioRolloffMode.Linear;
            }
        }

        #endregion
    
        public void TouchNPC()
        {
            TouchNPC(touchParameterValue);
        }

        public void TouchNPC(string parameter)
        {
            StartCoroutine(Co_TouchNPC(parameter));
        }
    
        public void EndNPC()
        {
            EndNPC(endParameterValue);
        }
    
        public void EndNPC(string parameter)
        {
            StartCoroutine(Co_EndNPC(parameter));
        }

        public void PlayLoopAnimation()
        {
            loopRoutine = StartCoroutine(Co_PlayLoopAnimation(loopParameterValue));
        }
    
        public void PlayLoopAnimation(string parameter)
        {
            loopRoutine = StartCoroutine(Co_PlayLoopAnimation(parameter));
        }
    
        public void StopLoopAnimation()
        {
            StopCoroutine(loopRoutine);
        }

        #region Coroutine

        private IEnumerator Co_TouchNPC(string parameterValue)
        {
            if (touched) yield break;
        
            touched = true;
        
            onTouchNPC.Invoke();

            StartCoroutine(Co_PlayAnimation(parameterValue));
        
            yield return new WaitForSeconds(touchDelay);
        
            touched = false;
        }
    
        private IEnumerator Co_EndNPC(string parameterValue)
        {
            if (ended) yield break;
        
            ended = true;
        
            onEndNPC.Invoke();

            StartCoroutine(Co_PlayAnimation(parameterValue));
        
            yield return new WaitForSeconds(endDelay);
        
            ended = false;
        }
    
        private IEnumerator Co_PlayLoopAnimation(string parameterValue)
        {
            if(!loopAnimation) yield break;

            do
            {
                if (touched)
                {
                    yield return null;
                    continue;
                }
                StartCoroutine(Co_PlayAnimation(parameterValue));
                onLoopAnimation.Invoke();
                yield return new WaitForSeconds(loopDelay);
            } while (loopAnimation);
        }
    
        private IEnumerator Co_PlayAnimation(string parameterValue)
        {
            if (!animator)
            {
                Debug.LogWarning($"{this.name}에 애니메이터가 존재하지 않습니다.");
                yield break;
            }
        
            animator.SetBool(parameterValue, true);

            yield return new WaitWhile(() => animator.IsInTransition(0));
        
            if (animator.GetAnimatorParameterType(parameterValue) == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(parameterValue, false);
            }
        }
    
        #endregion
    }

    public enum PlayOrder
    {
        Ascending,
        Descending,
        Random
    }

    public enum DialoguePop
    {
        Always,
        OnTouch,
    }
}