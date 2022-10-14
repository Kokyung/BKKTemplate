using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BKK.EditorCustomUtility
{
    [InitializeOnLoad]
    public class GameObjectStealthModeMenu : Editor
    {
        private const string MenuName = "BKK/하이러키에서 싱글턴 숨기기";

        private const string prefsName = "HierarchystealthMode";
 
        public static bool isEnabled = false;

        static GameObjectStealthModeMenu()
        {
            isEnabled = EditorPrefs.GetBool(prefsName, false);
        }

        [MenuItem(MenuName)]
        private static void ToggleAction()
        {
            isEnabled = !isEnabled;
            EditorPrefs.SetBool(prefsName, isEnabled);
        
            var modeList = FindObjectsOfType<GameObjectStealthMode>();

            foreach (var mode in modeList)
            {
                mode.HideInInspector(isEnabled);
            }
            EditorApplication.DirtyHierarchyWindowSorting();// 이걸 호출해야 하이러키에 반영됨
        }
 
        [MenuItem(MenuName, true)]
        private static bool ToggleActionValidate()
        {
            Menu.SetChecked(MenuName, isEnabled);

            return true;
        }
    }
}
