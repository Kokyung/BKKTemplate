using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BKK.Extension
{
    public static class UIExtension
    {
        public static bool UIExist(this Vector2 point, GraphicRaycaster graphicRaycaster)
        {
            var uiExists = false;
            var eventData = new PointerEventData(EventSystem.current);
            var results = new List<RaycastResult>();

            eventData.position = point;

            graphicRaycaster.Raycast(eventData, results);

            uiExists = results.Count > 0;

            return uiExists;
        }
    }
}
