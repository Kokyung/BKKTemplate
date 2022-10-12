using UnityEngine;

namespace BKK.Debugging
{
    /// <summary>
    /// 빌드시 디버그 로그 처리를 제외시킵니다.
    /// </summary>
    public static class Debug
    {
        public static void Log(object message)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log(message);
#endif
        }

        public static void Log(object message, Object context)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log(message, context);
#endif
        }

        public static void LogWarning(object message)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning(message);
#endif
        }

        public static void LogWarning(object message, Object context)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning(message, context);
#endif
        }

        public static void LogError(object message)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogError(message);
#endif
        }

        public static void LogError(object message, Object context)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogError(message, context);
#endif
        }
    }
}

