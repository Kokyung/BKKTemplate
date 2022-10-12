using System;
using System.Collections;
using System.Collections.Generic;
using BKK.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmotionController : MonoBehaviour
{
    public Animator emotionAnimator;

    public EmotionAsset emotionAsset;

    public int emotionLayer = 1;

    public Button activeListButton;
    
    public Transform listRoot;

    public Transform listContent;
    
    public Transform listItemOriginal;
    
    public List<Emotion> emotions = new List<Emotion>();

    public List<Transform> emotionButtons = new List<Transform>();

    private const string state_Idle = "Idle";

    private Coroutine emotionRoutine;

    private string currentParameter;

    private void Start()
    {
        FindAnimator();
        CreateEmotionButtonItem();
        SetActiveListButtonListener();
    }
    
    private void OnValidate()
    {
        if (Application.isPlaying) return;
        
        FindValue();
    }
    
    private void FindAnimator()
    {
        if(!emotionAnimator) emotionAnimator = GetComponent<Animator>();
        emotionAnimator.SetLayerWeight(0,0);
    }

    private void AddAllEmotions()
    {
        if (emotionAsset)
        {
            emotions = emotionAsset.emotions;
        }
        else
        {
            emotions.Add(new Emotion("안녕!", "Emote01_Hi"));
            emotions.Add(new Emotion("화이팅!", "Emote02_Cheer"));
            emotions.Add(new Emotion("박수!", "Emote03_Clap"));
            emotions.Add(new Emotion("안돼!", "Emote04_No"));
            emotions.Add(new Emotion("깜짝이야!", "Emote05_Suprised"));
            emotions.Add(new Emotion("제발!", "Emote06_Emotion"));
            emotions.Add(new Emotion("힘내!", "Emote07_Encourage"));
            emotions.Add(new Emotion("슬퍼!", "Emote08_Sad"));
            emotions.Add(new Emotion("날따라와!", "Emote09_Followme"));
            emotions.Add(new Emotion("아잉!", "Emote10_Cute"));
        }
    }

    private void FindValue()
    {
        if (!activeListButton) activeListButton = GetComponent<Button>();

        if(!listRoot) listRoot = GetComponentInChildren<ScrollRect>(true).transform;
        if(!listContent) listContent = GetComponentInChildren<ContentSizeFitter>(true).transform;
        if (!listItemOriginal) listItemOriginal = listContent.Find("Btn_ani");
        listRoot.gameObject.SetActive(false);
        listItemOriginal.gameObject.SetActive(false);
    }

    private void SetActiveListButtonListener()
    {
        activeListButton.onClick.AddListener(() => listRoot.gameObject.SetActive(!listRoot.gameObject.activeSelf));
    }
    
    public void CreateEmotionButtonItem()
    {
        AddAllEmotions();
        
        ClearEmotionItem();
        
        foreach (var emotion in emotions)
        {
            var item = Instantiate(listItemOriginal, listContent);
            var nameText = item.GetComponentInChildren<TMP_Text>();
            var button = item.GetComponentInChildren<Button>();
            nameText.text = emotion.emotionName;

            button.onClick.AddListener(() => PlayEmotionAnimation(emotion.animatorParameterName));
            
            emotionButtons.Add(item);
            
            item.gameObject.SetActive(true);
        }
    }

    public void ClearEmotionItem()
    {
        foreach (var emotionButton in emotionButtons)
        {
            Destroy(emotionButton.gameObject);
        }
        
        emotionButtons.Clear();
    }

    public void PlayEmotionAnimation(string parameterName)
    {
        if (emotionRoutine != null)
        {
            StopCoroutine(emotionRoutine);
            //emotionAnimator.SetBool(parameterName, false);
            //emotionAnimator.CrossFade(state_Idle, 0.1f, emotionLayer);
        }
        
        emotionRoutine = StartCoroutine(Co_PlayEmotionAnimation(parameterName));
    }

    private IEnumerator Co_PlayEmotionAnimation(string parameterName)
    {
        // 여기서 플레이어 움직임 체크해서 움직이고 있으면 리턴

        if (!emotionAnimator.CheckParameter(parameterName))
        {
            Debug.LogWarning($"{emotionAnimator.name} 애니메이터에 {parameterName} 파라미터가 존재하지 않습니다.");
            yield break;
        }

        if(!emotionAnimator.GetCurrentAnimatorStateInfo(emotionLayer).IsName(state_Idle)) yield break;

        emotionAnimator.SetBool(parameterName, true);

        StartCoroutine(ResetBool(parameterName, 0.15f));
    }

    private IEnumerator ResetBool(string parameterName, float sec)
    {
        yield return null;// 다음 스테이트로 넘어갈 시간 여유를 준다.
        yield return new WaitForSeconds(sec);// 네트워크 애니메이터에서 체크할 시간 여유를 준다.

        if (emotionAnimator.GetAnimatorParameterType(parameterName) == AnimatorControllerParameterType.Bool)
        {
            emotionAnimator.SetBool(parameterName, false);
        }
    }
}

[System.Serializable]
public class Emotion
{
    public string emotionName;
    public string animatorParameterName;

    public Emotion(string _emotionName, string _animatorParameterName)
    {
        emotionName = _emotionName;
        animatorParameterName = _animatorParameterName;
    }
}