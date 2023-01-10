using UnityEngine;

namespace BKK.GameEventArchitecture
{
    public interface IGameEvent
    {
        public void Register(IGameEventListener listener);
        public void Deregister(IGameEventListener listener);

        public void Raise();
        public void Cancel();

        public bool HasListeners();
    }
    
    public interface IGameEvent<Ttype>
    {
        public void Register(IGameEventListener<Ttype> listener);
        public void Deregister(IGameEventListener<Ttype> listener);

        public void Raise(Ttype value);
        public void Cancel(Ttype Value);

        public bool HasListeners();
    }
}
