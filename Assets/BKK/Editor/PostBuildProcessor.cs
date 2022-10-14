using System.Collections;
using System.Collections.Generic;
using BKK.EditorCustomUtility;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class PostBuildProcessor : MonoBehaviour
{
    [PostProcessBuild(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
        Debug.Log( pathToBuiltProject );
    }
}
