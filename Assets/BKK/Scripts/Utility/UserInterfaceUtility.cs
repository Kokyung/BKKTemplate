using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BKK.Utility
{
    public class UserInterfaceUtility : MonoBehaviour
    {
        public static bool IsPointerOverUI(int layer = 5)
        {
            return IsPointerOverUIElement(GetEventSystemRaycastResults(), layer);
        }

        private static bool IsPointerOverUIElement(IReadOnlyList<RaycastResult> eventSystemRaycastResults,
            int layer = 5)
        {
            foreach (var curRaycastResult in eventSystemRaycastResults)
            {
                if (curRaycastResult.gameObject.layer == layer) return true;
            }

            return false;
        }

        private static List<RaycastResult> GetEventSystemRaycastResults()
        {
            var eventData = new PointerEventData(EventSystem.current);
            var raycastResults = new List<RaycastResult>();

            eventData.position = Input.mousePosition;
            EventSystem.current.RaycastAll(eventData, raycastResults);
            return raycastResults;
        }
    }
}

