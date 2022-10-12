using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoadAttribute]
public static class DefaultSceneLoader
{
    static DefaultSceneLoader(){
        EditorApplication.playModeStateChanged += LoadDefaultScene;
    }

    static void LoadDefaultScene(PlayModeStateChange state)
    {
        var sceneData = SceneLoaderData.Load();
        
        if (!sceneData.loadSceneWhenPlay) return;
		
        if (state == PlayModeStateChange.ExitingEditMode) 
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo ();
        }

        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            string sceneName = sceneData.sceneNameList[sceneData.defaultSceneIndex].Substring(0, sceneData.sceneNameList[sceneData.defaultSceneIndex].Length - 6);
            EditorSceneManager.LoadScene(sceneName, sceneData.mode);
        }
    }
}
