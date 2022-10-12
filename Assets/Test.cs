using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using BKK.Tools;
using BKK.UI;
using UnityEngine;

public class Test : MonoBehaviour
{
    public RotationOnly rotationOnly = RotationOnly.Y;
    
    private void Start()
    {
        CoroutineHelper.StartCoroutine(TP());
        var d = new byte[1];
        d[0] = 10;
        FileUtility.CreateFile("C:/Users/kikik/Desktop/t.mp4", d, true);
        Debug.Log(FileUtility.LoadFile("C:/Users/kikik/Desktop/t.mp4")[0]);
    }

    private IEnumerator TP()
    {
        yield return null;
        Debug.Log("!?");
    }
}
