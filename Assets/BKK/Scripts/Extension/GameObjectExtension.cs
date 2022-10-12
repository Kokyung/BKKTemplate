using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BKK.Extension
{
    public static class GameObjectUtility
    {
        public static bool IncludedInLayerMask(this GameObject target, LayerMask layerMask)
        {
            return ((1 << target.gameObject.layer) & layerMask) != 0;
        }

        public static bool ExcludedInLayerMask(this GameObject target, LayerMask layerMask)
        {
            return ((1 << target.gameObject.layer) & layerMask) == 0;
        }

        public static Transform FindInAllChildren(this Transform target, string name)
        {
            if (target.name == name) return target;

            for (var i = 0; i < target.childCount; ++i)
            {
                var result = FindInAllChildren(target.GetChild(i), name);

                if (result != null) return result;
            }

            return null;
        }

        public static T GetComponentByGameObjectName<T>(this GameObject target, string name)
        {
            var result = target.transform.FindInAllChildren(name);

            return result.GetComponent<T>();
        }

        public static bool IsMissingReference(this UnityEngine.Object unknown)
        {
            try
            {
                unknown.GetInstanceID();
                return false;
            }
            catch (System.Exception e)
            {
                return true;
            }
        }

        public static void SetChildren(this Transform target, Transform[] children)
        {
            foreach (var child in children)
            {
                target.SetChild(child);
            }
        }

        public static void SetChild(this Transform target, Transform child)
        {
            child.SetParent(target);
        }

        public static Vector3 InsertX(this Vector3 pos, float value)
        {
            pos.x = value;
            return pos;
        }
        
        public static Vector3 InsertY(this Vector3 pos, float value)
        {
            pos.y = value;
            return pos;
        }
        
        public static Vector3 InsertZ(this Vector3 pos, float value)
        {
            pos.z = value;
            return pos;
        }
        
        public static Vector3 InsertXY(this Vector3 pos, float x, float y)
        {
            pos.x = x;
            pos.y = y;
            return pos;
        }
        
        public static Vector3 InsertYZ(this Vector3 pos, float y, float z)
        {
            pos.y = y;
            pos.z = z;
            return pos;
        }
        
        public static Vector3 InsertXZ(this Vector3 pos, float x, float z)
        {
            pos.x = x;
            pos.z = z;
            return pos;
        }

        public static Transform InsertX(this Transform tr, float value)
        {
            tr.position = tr.position.InsertX(value);
            return tr;
        }
        
        public static Transform InsertY(this Transform tr, float value)
        {
            tr.position = tr.position.InsertY(value);
            return tr;
        }
        
        public static Transform InsertZ(this Transform tr, float value)
        {
            tr.position = tr.position.InsertZ(value);
            return tr;
        }
        
        public static Transform InsertXY(this Transform tr, float x, float y)
        {
            tr.position = tr.position.InsertXY(x, y);
            return tr;
        }
        
        public static Transform InsertYZ(this Transform tr, float y, float z)
        {
            tr.position = tr.position.InsertYZ(y, z);
            return tr;
        }
        
        public static Transform InsertXZ(this Transform tr, float x, float z)
        {
            tr.position = tr.position.InsertXZ(x, z);
            return tr;
        }




        public static bool ContainsScreenPoint(this RectTransform rectTransform, Vector3 pos)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, pos);
        }
        
        public static bool ContainsScreenPoint(this RectTransform rectTransform, Vector3 pos, Camera eventCamera)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, pos, eventCamera);
        }
        
        public static bool ContainsScreenPoint(this RectTransform rectTransform, Vector3 pos, Camera eventCamera, Vector4 offset)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, pos, eventCamera, offset);
        }
        
        public static Vector2 WorldToScreenPoint(this Vector3 worldPoint, Camera eventCamera)
        {
            return RectTransformUtility.WorldToScreenPoint(eventCamera, worldPoint);
        }
    }
}

