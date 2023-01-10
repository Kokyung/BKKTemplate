using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

/// <summary>
/// https://docs.unity3d.com/Manual/upm-api.html
/// </summary>
public static class UpmRequest
{
    private static AddRequest addRequest;
    private static ListRequest listRequest;
    private static RemoveRequest removeRequest;
    
    [MenuItem("BKK/패키지/Rider Flow 패키지 설치")]
    private static void AddRiderFlow()
    {
        bool ok = EditorUtility.DisplayDialog("안내",
            "Rider Flow는 효과적인 Scene 관리를 가능하게 해주는 Unity Editor용 무료 플러그인입니다.\nRider Flow를 설치하시겠습니까?", "OK", "Cancel");
        
        if(ok) Add("com.jetbrains.riderflow");
    }

    private static void Add(string identifier)
    {
        // Add a package to the project
        addRequest = Client.Add(identifier);
        //EditorApplication.update += OnProgressAdd;
    }
    
    private static void Remove(string identifier)
    {
        // Add a package to the project
        removeRequest = Client.Remove(identifier);
    }
    
    private static void List()
    {
        // List packages installed for the project
        listRequest = Client.List();    
        EditorApplication.update += OnProgressList;
    }

    private static async Task<bool> HasPackage(string identifier)
    {
        listRequest = Client.List();

        while (!listRequest.IsCompleted) await Task.Delay(200);

        var exist = listRequest.Result.FirstOrDefault(p => p.name == identifier);

        return exist != null;
    }

    private static void OnProgressAdd()
    {
        if (addRequest.IsCompleted)
        {
            if (addRequest.Status == StatusCode.Success)
                Debug.Log("Installed: " + addRequest.Result.packageId);
            else if (addRequest.Status >= StatusCode.Failure)
                Debug.Log(addRequest.Error.message);

            EditorApplication.update -= OnProgressAdd;
        }
    }
    
    private static void OnProgressList()
    {
        if (listRequest.IsCompleted)
        {
            if (listRequest.Status == StatusCode.Success)
                foreach (var package in listRequest.Result)
                    Debug.Log("Package name: " + package.name);
            else if (listRequest.Status >= StatusCode.Failure)
                Debug.Log(listRequest.Error.message);

            EditorApplication.update -= OnProgressList;
        }
    }
}
