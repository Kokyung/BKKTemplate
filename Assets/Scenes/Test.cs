using System;
using System.Collections;
using BKK.Extension;
using BKK.UI;
using BKK.Utility;
using UnityEngine;

public class Test : MonoBehaviour
{
    public RotationOnly rotationOnly = RotationOnly.Y;

    public RenderTexture rt1;
    public RenderTexture rt2;
    
    public Texture2D tex1;
    
    public Texture2D tex2;

    private Coroutine routine;
    
    private void Start()
    {
        routine = CoroutineHelper.StartCoroutine(TP());
        // var d = new byte[1];
        // d[0] = 10;
        // FileUtility.CreateFile("C:/Users/kikik/Desktop/t.mp4", d, true);
        // Debug.Log(FileUtility.LoadFile("C:/Users/kikik/Desktop/t.mp4")[0]);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Rotate(Vector3.left * Time.deltaTime * 30f);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(Vector3.right * Time.deltaTime * 30f);
        }

        transform.InsertXY(0, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CoroutineHelper.StopCoroutine(routine);
        }
    }

    private IEnumerator TP()
    {
        yield return null;
        Debug.Log("!?");

        // while (true)
        // {
        //     Debug.Log("!!!");
        //     yield return null;
        // }
    }
}
