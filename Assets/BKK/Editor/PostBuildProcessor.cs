using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BKK.EditorCustomUtility;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class PostBuildProcessor : MonoBehaviour
{
    [PostProcessBuild(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        Debug.Log(pathToBuiltProject);
        BuildCompleteSound();
    }

    private static async void BuildCompleteSound()
    {
        await Task.Delay(2000);
        EditorSFX.PlayClip(Resources.Load<AudioClip>("BuildComplete-HyunJu"));
    }
}
