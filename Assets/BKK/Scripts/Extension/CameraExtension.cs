using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BKK.Extension
{
    public static class CameraExtension
    {
        public static bool IsInCamera(this Renderer renderer, Camera cam)
        {
            return renderer.bounds.IsInCamera(cam);
        }

        public static bool IsInCamera(this Collider collider, Camera cam)
        {
            return collider.bounds.IsInCamera(cam);
        }
    
        public static bool IsInCamera(this Bounds bounds, Camera cam)
        {
            var planes = GeometryUtility.CalculateFrustumPlanes(cam);

            return GeometryUtility.TestPlanesAABB(planes, bounds);
        }
    }
}