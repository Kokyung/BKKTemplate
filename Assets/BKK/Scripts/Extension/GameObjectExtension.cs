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
    }
}

