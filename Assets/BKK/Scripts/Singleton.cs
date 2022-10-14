using BKK.EditorCustomUtility;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    private static object _lock = new object();

    public static T Instance => GetInstance();

    private static T GetInstance()
    {
        if (applicationIsQuitting)
        {
            Debug.LogWarning($"[Singleton] 앱 종료 중이므로 {typeof(T)} 싱글턴 인스턴스를 생성하지 않았습니다.");
            return null;
        }

        lock (_lock)
        {
            if (_instance == null)
            {
                _instance = (T) FindObjectOfType(typeof(T));

                if (FindObjectsOfType(typeof(T)).Length > 1)
                {
                    Debug.LogError($"{typeof(T)} 싱글턴 인스턴스가 둘 이상 존재합니다.");
                    return _instance;
                }

                if (_instance == null)
                {
                    var singleton = new GameObject();
                    _instance = singleton.AddComponent<T>();
                    singleton.name = typeof(T).ToString();
                    
                    #if UNITY_EDITOR
                    singleton.AddComponent<GameObjectStealthMode>();
                    #endif
                    
                    DontDestroyOnLoad(singleton);
                }
                else
                {
                    Debug.Log($"{typeof(T)} 싱글턴 인스턴스가 이미 존재합니다. {_instance.gameObject.name}");
                }
            }

            return _instance;
        }

    }

    private static bool applicationIsQuitting = false;

    public void OnDestroy()
    {
        applicationIsQuitting = true;
    }
}
