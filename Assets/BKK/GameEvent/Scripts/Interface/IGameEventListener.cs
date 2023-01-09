using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BKK.GameEventArchitecture
{
    public interface IGameEventListener<Ttype>
    {
        public void RaiseEvent(Ttype value);

        public void StopEvent(Ttype value);

        public string GetListenerPath();
    }
    
    public interface IGameEventListener
    {
        public void RaiseEvent();

        public void StopEvent();

        public string GetListenerPath();
    }
}
