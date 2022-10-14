using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BKK.EditorCustomUtility
{
    public class GameObjectStealthMode : MonoBehaviour
    {
        private const string prefsName = "HierarchystealthMode";

        private void Awake()
        {
#if UNITY_EDITOR
            HideInInspector(EditorPrefs.GetBool(prefsName));
#endif
            HideThis();
        }

        public void HideInInspector(bool enable)
        {
            this.gameObject.hideFlags = enable ? HideFlags.HideInHierarchy : HideFlags.None;
            HideThis();
        }

        private void HideThis()
        {
            GetComponent<GameObjectStealthMode>().hideFlags = HideFlags.HideInInspector;
            //this.hideFlags = HideFlags.HideInInspector; // <- 이렇게 하면 안숨겨짐
        }
    }
}

