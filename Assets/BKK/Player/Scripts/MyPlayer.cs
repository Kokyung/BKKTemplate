using System;
using System.Collections;
using UnityEngine;

public class MyPlayer : MonoBehaviour
{
	private static MyPlayer _instance;
	public static MyPlayer Instance
	{
		get
		{
			if (!_instance) return _instance;
			
			Destroy(_instance.gameObject);
			Debug.LogWarning("로컬 플레이어 인스턴스가 하나 이상입니다. 이 인스턴스를 삭제합니다.");
			return null;
		}
	}

	[SerializeField] private Camera playerCamera;
	[SerializeField] private AudioListener audioListener;

	public bool CameraAndAudioActive => playerCamera && audioListener;

	private void Start()
	{
		InitializePlayerComponents();
	}

	private void OnValidate()
	{
		GetPlayerComponents();
	}

	/// <summary>
	/// 플레이어의 컴포넌트들을 가져온다.
	/// </summary>
	private void GetPlayerComponents()
	{
		if(!playerCamera) playerCamera = Camera.main;
		if (!audioListener) audioListener = FindObjectOfType<AudioListener>();
	}
	
	/// <summary>
	/// 플레이어 컴포넌트들을 초기화합니다.
	/// </summary>
	private void InitializePlayerComponents()
	{
		// 여기에 초기화 코드를 작성해주세요.
		EnableCameraAndAudio();
	}
	
	/// <summary>
	/// 지정 트랜스폼으로 텔레포트 합니다.
	/// </summary>
	/// <param name="newPlayerTr">새로운 플레이어 위치 트랜스폼</param>
	public void Teleport(Transform newPlayerTr)
	{
		StartCoroutine(SetPlayerPosition(newPlayerTr));
	}

	/// <summary>
	/// 플레이어의 Transform을 변경할 때 호출하면됨
	/// </summary>
	/// <param name="newPlayerTr">새로운 플레이어 위치 트랜스폼</param>
	/// <returns></returns>
	private IEnumerator SetPlayerPosition(Transform newPlayerTr)
	{
		// 한 프레임 동안 카메라 위치 및 기타 설정해줘야하는 요소들을 실행한다.
		
		yield return null;
		
		// 위치 변경
		transform.position = newPlayerTr.position;
		transform.rotation = newPlayerTr.rotation;
		
		yield return null;
	}

	/// <summary>
	/// Scene 로드 후 원하는 타이밍에 카메라와 오디오리스너를 활성화시키기 위해 작성하였습니다.
	/// </summary>
	/// <param name="enable">활성화 여부</param>
	public void EnableCameraAndAudio(bool enable = true)
	{
		playerCamera.enabled = enable;
		audioListener.enabled = enable;
	}
}

