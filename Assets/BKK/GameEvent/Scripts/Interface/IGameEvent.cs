using UnityEngine;

namespace BKK.GameEventArchitecture
{
    public abstract class IGameEvent : ScriptableObject
    {
        public abstract void Register(IGameEventListener listener);
        public abstract void Deregister(IGameEventListener listener);
        public abstract void Raise();
        public abstract void Cancel();
        public abstract bool HasListeners();
    }
    
    public abstract class IGameEvent<Ttype> : ScriptableObject
    {
        public abstract void Register(IGameEventListener<Ttype> listener);
        public abstract void Deregister(IGameEventListener<Ttype> listener);
        public abstract void Raise(Ttype value);
        public abstract void Cancel(Ttype Value);
        public abstract bool HasListeners();
    }
}
