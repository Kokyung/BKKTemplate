/*******************************************************************************************************
 * 
 *		SceneLoaderData.cs
 *			- SceneLoaderWindow 에서 설정한 폴더의 scene list 를 보관할 asset data
 * 
 *******************************************************************************************************/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Unity.Collections;

[CreateAssetMenu( fileName = "SceneLoaderData", menuName = "BKK/Scene Loader Data" )]
public sealed class SceneLoaderData : ScriptableObject
{
	public string currentPath;
	
	[ReadOnly]
	public List<string> sceneNameList = new List<string>();

	[ReadOnly]
	public List<string> scenePathList = new List<string>();

	private const string assetPath = "SceneLoader/";
	private const string assetName = "SceneLoaderData";
	
	[ReadOnly]
	public bool loadSceneWhenPlay = false;

	[ReadOnly]
	public int defaultSceneIndex = 0;

	[ReadOnly]
	public LoadSceneMode mode = LoadSceneMode.Single;

	private void OnDisable()
	{
		EditorUtility.SetDirty(this);
	}

	public static SceneLoaderData Load()
	{
		string dataPath = string.Format( "{0}{1}", assetPath, assetName );
		SceneLoaderData asset = Resources.Load<SceneLoaderData>( dataPath );
		return asset;
	}

	public void ClearList()
	{
		sceneNameList.Clear();
		scenePathList.Clear();
	}

	public void AddSceneName( string name )
	{
		sceneNameList.Add( name );
	}

	public void AddScenePath( string name )
	{
		scenePathList.Add( name );
	}

	public int GetSceneNameListCount()
	{
		return sceneNameList.Count;
	}

	public void DrawSceneList()
	{
		int index = 0;
		foreach ( string path in scenePathList )
		{
			GUILayout.BeginHorizontal();
			if ( GUILayout.Button( sceneNameList[index] ) )
			{
				EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
				EditorSceneManager.OpenScene( path );
			}
			GUILayout.EndHorizontal();

			index += 1;
		}
	}
}
