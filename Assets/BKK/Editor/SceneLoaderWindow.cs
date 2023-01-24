/*******************************************************************************************************
 * 
 *		SceneLoaderWindow.cs
 *			- 특정 폴더안의 scene 을 오픈시키기 위한 EditorWindow
 * 
 *******************************************************************************************************/

using System;
using System.Text;
using BKK.UI;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class SceneLoaderWindow : EditorWindow
{
	private SceneLoaderData sceneLoaderDataInstance = default;
	public SceneLoaderData sceneLoaderData
	{
		get
		{
			if ( sceneLoaderDataInstance == null )
				sceneLoaderDataInstance = SceneLoaderData.Load();

			return sceneLoaderDataInstance;
		}
	}

	void OnGUI()
	{
		CustomEditorUtility.DrawUILine( Color.gray );
		
		GUILayout.BeginHorizontal();
		GUILayout.Label( "Scene 폴더 지정" );
		
		if ( GUILayout.Button( new GUIContent( "폴더 지정", "사용 하려고 하는 폴더의 Scene 을 지정합니다" ) ) )
		{
			if ( sceneLoaderData != null )
			{
				CustomEditorUtility.FileBrowser(ref sceneLoaderData.currentPath, "Scene 폴더 지정");

				if ( sceneLoaderData.GetSceneNameListCount() == 0 )
					EditorUtility.DisplayDialog( "안내 팝업", "폴더 내에 scene 파일이 없습니다.", "확인" );
			}
		}
		GUILayout.EndHorizontal();

		// 지정된 폴더 경로의 Scene 에셋 리스트 갱신
		RefreshSceneList(sceneLoaderData.currentPath);
		
		if ( sceneLoaderData != null )
			sceneLoaderData.DrawSceneList();

		CustomEditorUtility.DrawUILine( Color.gray );

		sceneLoaderData.loadSceneWhenPlay = GUILayout.Toggle(sceneLoaderData.loadSceneWhenPlay, "플레이시 로드할 Default Scene 지정");
		
		if (sceneLoaderData.loadSceneWhenPlay)
		{
			sceneLoaderData.defaultSceneIndex = EditorGUILayout.Popup("Scene", sceneLoaderData.defaultSceneIndex, sceneLoaderData.sceneNameList.ToArray());

			sceneLoaderData.mode = (LoadSceneMode)EditorGUILayout.Popup("Load Mode", (int)sceneLoaderData.mode, Enum.GetNames(typeof(LoadSceneMode)));
		}
		
		CustomEditorUtility.DrawUILine( Color.gray );
	}

	private void RefreshSceneList(string folderPath)
	{
		if (folderPath == null) return;
		
		StringBuilder sb = new StringBuilder();
		System.IO.DirectoryInfo di = new System.IO.DirectoryInfo( folderPath );
		
		sceneLoaderData.ClearList();
		
		foreach ( System.IO.FileInfo file in di.GetFiles() )
		{
			string extension = file.Name.Substring( file.Name.Length - 5, 5 );
			if ( extension.Equals( "unity" ) == false )
				continue;

			// 지정한 폴더 경로 + scene 파일명 조합하기
			sb.Length = 0;

			folderPath = folderPath.Substring(folderPath.IndexOf("Assets"));
					
			sb.Append( folderPath );
			sb.Append( "/" );
			sb.Append( file.Name );

			sceneLoaderData.AddSceneName( file.Name );
			sceneLoaderData.AddScenePath( sb.ToString() );
		}
	}
	
	/// <summary>
	/// 유니티 에디터에 메뉴 추가하기
	/// </summary>
	[MenuItem( "BKK/Scene/Scene Loader" )]

	public static void ShowWindow()
	{
		SceneLoaderWindow window = EditorWindow.GetWindow<SceneLoaderWindow>( false, " Scene Loader ", true );
		window.titleContent = new GUIContent( "Scene Loader" );
	}
}

