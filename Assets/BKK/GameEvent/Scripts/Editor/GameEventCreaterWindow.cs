using System;
using BKK.GameEventArchitecture.Editor;
using UnityEditor;
using UnityEngine;
using Debug = BKK.Debugging.Debug;

namespace BKK.GameEventArchitecture.Editor
{
    public class GameEventCreaterWindow : EditorWindow
    {
        private string className;
        private string creationPath = "Assets/CustomGameEvent";
        private MonoScript script = null;

        private void OnGUI()
        {
            Draw();
        }

        private void OnDestroy()
        {
            ResetProperties();
        }

        private void Draw()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("타겟 클래스 지정", "파라미터로 받을 타겟 클래스를 지정합니다."));
            script = EditorGUILayout.ObjectField(script, typeof(MonoScript), false) as MonoScript;

            if (script != null)
            {
                if (script.GetClass() != null)
                {
                    className = script.GetClass().FullName;
                }
                else
                {
                    Debug.LogError("접근 할 수 없는 클래스입니다. Assets 폴더 내에 있는지 확인해주세요");
                }
            }
            EditorGUILayout.EndHorizontal();
            
            className = EditorGUILayout.TextField(new GUIContent("클래스 이름", "파라미터로 받을 클래스 이름. 네임스페이스도 포함되어야합니다.\n예) UnityEngine.Quaternion"), className);
            creationPath = EditorGUILayout.TextField(new GUIContent("폴더 경로", "생성된 게임 이벤트가 저장될 폴더 경로"), creationPath);

            if (GUILayout.Button(new GUIContent("폴더 지정", "게임 이벤트가 생성될 폴더를 지정합니다.")))
            {
                // 폴더 지정 파일 다이얼로그 띄우기
                string folderPath = EditorUtility.SaveFolderPanel("게임 이벤트 폴더 지정", Application.dataPath, "");

                if (string.IsNullOrEmpty(folderPath)) return;

                creationPath = folderPath;
            }

            if (GUILayout.Button(new GUIContent("생성", "게임 이벤트를 생성합니다.")))
            {
                GameEventCreater.CreateAll(className, creationPath);
            }
            
            EditorUtility.SetDirty(this);
        }

        private void ResetProperties(bool withPath = false)
        {
            if(withPath) creationPath = "Assets/BKK/GameEvent/CustomGameEvent";
            className = string.Empty;
            script = null;
        }
    }
}
