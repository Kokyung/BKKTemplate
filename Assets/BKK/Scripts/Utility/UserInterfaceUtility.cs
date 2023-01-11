using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BKK.Utility
{
    public class UserInterfaceUtility : MonoBehaviour
    {
        public static bool IsPointerOverUI(int layer = 5)
        {
            var raycastResult = GetEventSystemRaycastResults(Input.mousePosition);
            return IsPointerOverUIElement(raycastResult, layer);
        }

        public static bool IsPointerOverUIElement(IReadOnlyList<RaycastResult> eventSystemRaycastResults,
            int layer = 5)
        {
            foreach (var curRaycastResult in eventSystemRaycastResults)
            {
                if (curRaycastResult.gameObject.layer == layer) return true;
            }

            return false;
        }
        
        public static List<RaycastResult> GetEventSystemRaycastResults(Vector2 pos)
        {
            var eventData = new PointerEventData(EventSystem.current);
            var raycastResults = new List<RaycastResult>();

            eventData.position = pos;
            EventSystem.current.RaycastAll(eventData, raycastResults);
            return raycastResults;
        }

        #region Select

        public static void SelectDown<T>() where T : Selectable
        {
            var currentSelected = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
            SelectDown<T>(currentSelected);
        }

        public static void SelectUp<T>() where T : Selectable
        {
            var currentSelected = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
            SelectUp<T>(currentSelected);
        }

        public static void SelectDown<T1, T2>()
            where T1 : Selectable
            where T2 : Selectable
        {
            var currentSelected = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
            SelectDown<T1, T2>(currentSelected);
        }

        public static void SelectUp<T1, T2>()
            where T1 : Selectable
            where T2 : Selectable
        {
            var currentSelected = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
            SelectUp<T1,T2>(currentSelected);
        }

        public static void SelectDown<T1, T2, T3>()
            where T1 : Selectable
            where T2 : Selectable
            where T3 : Selectable
        {
            var currentSelected = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
            SelectDown<T1, T2, T3>(currentSelected);
        }

        public static void SelectUp<T1, T2, T3>()
            where T1 : Selectable
            where T2 : Selectable
            where T3 : Selectable
        {
            var currentSelected = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
            SelectUp<T1,T2, T3>(currentSelected);
        }
        
        public static void SelectDown<T>(Selectable selectable) where T : Selectable
        {
            var next = selectable.FindSelectableOnDown();
            var isValid = next is T;

            if (next != null && isValid)
            {
                next.Select();
            }
        }

        public static void SelectUp<T>(Selectable selectable) where T : Selectable
        {
            var next = selectable.FindSelectableOnUp();
            var isValid = next is T;

            if (next != null && isValid)
            {
                next.Select();
            }
        }

        public static void SelectDown<T1, T2>(Selectable selectable)
            where T1 : Selectable
            where T2 : Selectable
        {
            var next = selectable.FindSelectableOnDown();
            var isValid = next is T1 || next is T2;

            if (next != null && isValid)
            {
                next.Select();
            }
        }

        public static void SelectUp<T1, T2>(Selectable selectable)
            where T1 : Selectable
            where T2 : Selectable
        {
            var next = selectable.FindSelectableOnUp();
            var isValid = next is T1 || next is T2;

            if (next != null && isValid)
            {
                next.Select();
            }
        }

        public static void SelectDown<T1, T2, T3>(Selectable selectable)
            where T1 : Selectable
            where T2 : Selectable
            where T3 : Selectable
        {
            var next = selectable.FindSelectableOnDown();
            var isValid = next is T1 || next is T2 || next is T3;

            if (next != null && isValid)
            {
                next.Select();
            }
        }

        public static void SelectUp<T1, T2, T3>(Selectable selectable)
            where T1 : Selectable
            where T2 : Selectable
            where T3 : Selectable
        {
            var next = selectable.FindSelectableOnUp();
            var isValid = next is T1 || next is T2 || next is T3;

            if (next != null && isValid)
            {
                next.Select();
            }
        }

        #endregion
        
    }
}

