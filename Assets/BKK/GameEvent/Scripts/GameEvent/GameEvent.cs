using System.Collections.Generic;
using BKK.Extension;
using UnityEngine;
using UnityEngine.Events;
using Debug = BKK.Debugging.Debug;

namespace BKK.GameEventArchitecture
{
    [CreateAssetMenu(menuName = "BKK/Game Event Architecture/Game Event", fileName = "New Game Event",order = 0)]
    public class GameEvent : ScriptableObject
    {
        private readonly HashSet<GameEventListener> listeners = new HashSet<GameEventListener>();

#if UNITY_EDITOR
        [HideInInspector]
        public string description;
#endif

        /// <summary>
        /// 게임 이벤트에 등록된 모든 게임 이벤트 리스너의 유니티 이벤트들을 호출합니다.
        /// </summary>
        public virtual void Invoke()
        {
            foreach (var listener in listeners)
            {
                listener.RaiseEvent();
                Debug.Log($"{this.name} 이벤트가 실행되었습니다.\n경로: {listener.GetListenerPath()}");
            }
        }

        public virtual void Cancel()
        {
            foreach (var listener in listeners)
            {
                listener.StopEvent();
                Debug.Log($"{this.name} 이벤트가 취소되었습니다.\n경로: {listener.GetListenerPath()}");
            }
        }

        /// <summary>
        /// 게임 이벤트 리스너를 등록합니다.
        /// </summary>
        /// <param name="listener">등록할 게임 이벤트 리스너</param>
        public void Register(GameEventListener listener) => listeners.Add(listener);

        /// <summary>
        /// 게임 이벤트 리스너를 해지합니다.
        /// </summary>
        /// <param name="listener">해지할 게임 이벤트 리스너</param>
        public void Deregister(GameEventListener listener) => listeners.Remove(listener);

        /// <summary>
        /// 게임 이벤트에 등록된 게임 이벤트 리스너가 있는지 체크합니다.
        /// </summary>
        /// <returns></returns>
        public bool HasListeners()
        {
            return listeners.Count > 0;
        }
    }
    
    public class GameEvent<T> : ScriptableObject
    {
        private readonly HashSet<IGameEventListener<T>> listeners = new HashSet<IGameEventListener<T>>();

#if UNITY_EDITOR
        [HideInInspector]
        public string description;
#endif

        /// <summary>
        /// 게임 이벤트에 등록된 모든 게임 이벤트 리스너의 유니티 이벤트들을 호출합니다.
        /// </summary>
        public virtual void Invoke(T value)
        {
            foreach (var listener in listeners)
            {
                listener.RaiseEvent(value);
                Debug.Log($"{this.name} 이벤트가 실행되었습니다.\n경로: {listener.GetListenerPath()}");
            }
        }

        public virtual void Cancel(T value)
        {
            foreach (var listener in listeners)
            {
                listener.StopEvent(value);
                Debug.Log($"{this.name} 이벤트가 취소되었습니다.\n경로: {listener.GetListenerPath()}");
            }
        }

        /// <summary>
        /// 게임 이벤트 리스너를 등록합니다.
        /// </summary>
        /// <param name="listener">등록할 게임 이벤트 리스너</param>
        public void Register(IGameEventListener<T>listener) => listeners.Add(listener);

        /// <summary>
        /// 게임 이벤트 리스너를 해지합니다.
        /// </summary>
        /// <param name="listener">해지할 게임 이벤트 리스너</param>
        public void Deregister(IGameEventListener<T> listener) => listeners.Remove(listener);

        /// <summary>
        /// 게임 이벤트에 등록된 게임 이벤트 리스너가 있는지 체크합니다.
        /// </summary>
        /// <returns></returns>
        public bool HasListeners()
        {
            return listeners.Count > 0;
        }
    }
}
