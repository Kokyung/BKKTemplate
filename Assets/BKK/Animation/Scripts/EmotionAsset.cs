using System;
using System.Collections;
using System.Collections.Generic;
using BKK.Tools;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif

[CreateAssetMenu(fileName = "Emotions", menuName = "BKK/Emotion Asset")]
public sealed class EmotionAsset : ScriptableObject
{
    public List<Emotion> emotions = new List<Emotion>();
}

#if UNITY_EDITOR
[CustomEditor(typeof(EmotionAsset))]
public class EmotionAsset_Editor : Editor
{
    private EmotionAsset asset;

    public void OnEnable ()
    {
        asset = (EmotionAsset) target;
    }
 
    public override void OnInspectorGUI ()
    {
        DrawDefaultInspector ();
        EditorGUILayout.Space ();
        DropAreaGUI ();
    }
 
    public void DropAreaGUI ()
    {
        var evt = Event.current;
        var style = new GUIStyle
        {
            border = new RectOffset(5, 5, 5, 5),
            normal =
            {
                background = (Texture2D) AssetDatabase.LoadAssetAtPath<Texture>("Assets/_DEV/BKK/Editor/Cat.jpg"),
                textColor = Color.magenta
            },
            alignment = TextAnchor.MiddleCenter
        };
        if(!style.normal.background) style.normal.background = Texture2D.grayTexture;
        var isCat = style.normal.background.name.Contains("Cat", StringComparison.OrdinalIgnoreCase);
        var fieldWidth = EditorGUIUtility.currentViewWidth - 25;
        var ratio = isCat ? (float) style.normal.background.width / (float) style.normal.background.height : 0;
        var dropArea = isCat
            ? GUILayoutUtility.GetRect(0.0f, 0.0f, GUILayout.Width(fieldWidth), GUILayout.Height(fieldWidth / ratio))
            : GUILayoutUtility.GetRect(0.0f, EditorGUIUtility.singleLineHeight * 2, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, isCat ? "\n\n\n\n\n애니메이터 파라미터 가져오라냥\n드래그 앤 드롭하라냥!" : "애니메이터 파라미터 가져오기\n드래그 앤 드롭!", style);

        switch (evt.type) {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!dropArea.Contains (evt.mousePosition))
                    return;
             
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
         
                if (evt.type == EventType.DragPerform) {
                    DragAndDrop.AcceptDrag ();
             
                    foreach (var draggedObject in DragAndDrop.objectReferences)
                    {
                        // 드래그 시 처리할 코드를 여기에 작성합니다.
                        switch (draggedObject)
                        {
                            case AnimatorController controller:
                                AddEmotions(controller);
                                break;
                            case Animator animator:
                                AddEmotions(animator);
                                break;
                            case GameObject go:
                                var anim = go.GetComponent<Animator>();
                                if (anim) AddEmotions(anim);
                                break;
                            default:
                                Debug.Log("애니메이션 컨트롤러 에셋만 가능합니다.");
                                continue;
                        }
                    }
                }
                break;
        }
    }

    public void AddEmotions(AnimatorController animatorController)
    {
        foreach (var parameter in animatorController.parameters)
        {
            asset.emotions.Add(new Emotion("",parameter.name));
        }
    }
    
    public void AddEmotions(Animator animatorController)
    {
        foreach (var parameter in animatorController.parameters)
        {
            asset.emotions.Add(new Emotion("",parameter.name));
        }
    }
}
#endif
