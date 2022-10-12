using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace BKK.Tools
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
        
        public static bool IsPointerOverUI(int layer = 5)
        {
            return IsPointerOverUIElement(GetEventSystemRaycastResults(), layer);
        }
        
        private static bool IsPointerOverUIElement(IReadOnlyList<RaycastResult> eventSystemRaycastResults, int layer = 5)
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
            
            eventData.position =  Input.mousePosition;
            EventSystem.current.RaycastAll( eventData, raycastResults );
            return raycastResults;
        }
        
        public static bool IsMissingReference(this UnityEngine.Object unknown) {
            try {
                unknown.GetInstanceID();
                return false;
            }
            catch (System.Exception e) {
                return true;
            }
        }
    }
    
    public static class AnimationUtility
    {
        public static AnimatorControllerParameterType GetAnimatorParameterType(this Animator animator,
            string parameterName)
        {
            for (var index = 0; index < animator.parameters.Length; index++)
            {
                var param = animator.GetParameter(index);

                if (param.name == parameterName)
                {
                    return param.type;
                }
            }

            return (AnimatorControllerParameterType) (-1);
        }

        public static bool CheckParameter(this Animator animator, string paramName)
        {
            return animator.parameters.Any(param => param.name == paramName);
        }
    }
    
    public static class FileUtility
    {
        public static void CreateFile(string _fullPath, byte[] _bytes, bool addNumber = false)
        {
            var directoryPath = Path.GetDirectoryName(_fullPath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (addNumber)
            {
                if (File.Exists(_fullPath))
                {
                    var index = 1;
                    do
                    {
                        index++;

                        _fullPath = _fullPath.Insert(_fullPath.LastIndexOf('.'), $" ({index})");
                    } while (File.Exists(_fullPath));
                }
            }

            File.WriteAllBytes(_fullPath, _bytes);
        }
        
        public static byte[] LoadFile(string _filePath)
        {
            if (File.Exists(_filePath))
            {
                return File.ReadAllBytes(_filePath);
            }
            else
            {
                Debug.LogError($"FileUtility: 파일이 존재하지 않습니다. / {_filePath}");
                return null;
            }
        }

        public static void DeleteFile(string _filePath)
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
        }

        /// <summary>
        /// PNG 파일을 가져와서 Texture2D로 변환하여 리턴합니다.
        /// </summary>
        /// <param name="name">불러올 파일 이름</param>
        /// <param name="folderPath">Application.persistentDataPath내의 폴더 경로.</param>
        /// <returns></returns>
        public static Texture2D LoadPngToTexture(string name, string folderPath = "/Image",
            TextureFormat format = TextureFormat.RGBA32)
        {
            if (folderPath[0] != '/') folderPath = '/' + folderPath;

            var loadPath = Application.persistentDataPath + folderPath + $"/{name}.png";

            var pngBytes = LoadFile(loadPath);
            
            var tex = new Texture2D(1, 1, format, false, false);

            tex.LoadImage(pngBytes);

            tex.Apply();

            return tex;
        }

        /// <summary>
        /// JPG 파일을 가져와서 Texture2D로 변환하여 리턴합니다.
        /// </summary>
        /// <param name="name">불러올 파일 이름</param>
        /// <param name="folderPath">Application.persistentDataPath내의 폴더 경로.</param>
        /// <returns></returns>
        public static Texture2D LoadJpgToTexture(string name, string folderPath = "/Image")
        {
            if (folderPath[0] != '/') folderPath = '/' + folderPath;

            var loadPath = Application.persistentDataPath + folderPath + $"/{name}.jpg";

            var jpgBytes = LoadFile(loadPath);

            var tex = new Texture2D(1, 1, TextureFormat.Alpha8, false);

            tex.LoadImage(jpgBytes);

            tex.Apply();

            return tex;
        }
    }
    
    public static class GraphicsUtility
    {
        public static T ChangeAlpha<T>(this T g, float _alpha)
            where T : Graphic
        {
            var color = g.color;
            color.a = _alpha;
            g.color = color;
            return g;
        }
        
        public static void ChangeAlpha(this Renderer renderer, float _alpha)
        {
            foreach (var material in renderer.materials)
            {
                material.ChangeAlpha(_alpha);
            }
        }

        public static void ChangeAlpha(this Material material, float _alpha)
        {
            var color = material.color;
            color.a = _alpha;
            material.color = color;
        }

        public static void ChangeColor(this Renderer renderer, Color _color)
        {
            foreach (var material in renderer.materials)
            {
                material.ChangeColor(_color);
            }
        }

        public static void ChangeColor(this Material material, Color _color)
        {
            material.color = _color;
        }
        
        public static GraphicsFormat GetSupportedGraphicsFormat(GraphicsDeviceType type)
        {
            if (QualitySettings.activeColorSpace == ColorSpace.Linear)
            {
                switch (type)
                {
                    case GraphicsDeviceType.Direct3D11:
                    case GraphicsDeviceType.Direct3D12:
                    case GraphicsDeviceType.Vulkan:
                        return GraphicsFormat.B8G8R8A8_SRGB;
                    case GraphicsDeviceType.OpenGLCore:
                    case GraphicsDeviceType.OpenGLES2:
                    case GraphicsDeviceType.OpenGLES3:
                        return GraphicsFormat.R8G8B8A8_SRGB;
                    case GraphicsDeviceType.Metal:
                        return GraphicsFormat.B8G8R8A8_SRGB;
                }
            }
            else
            {
                switch (type)
                {
                    case GraphicsDeviceType.Direct3D11:
                    case GraphicsDeviceType.Direct3D12:
                    case GraphicsDeviceType.Vulkan:
                        return GraphicsFormat.B8G8R8A8_UNorm;
                    case GraphicsDeviceType.OpenGLCore:
                    case GraphicsDeviceType.OpenGLES2:
                    case GraphicsDeviceType.OpenGLES3:
                        return GraphicsFormat.R8G8B8A8_UNorm;
                    case GraphicsDeviceType.Metal:
                        return GraphicsFormat.B8G8R8A8_UNorm;
                }
            }

            throw new ArgumentException($"Graphics device type {type} not supported");
        }
    
        public static RenderTextureFormat GetSupportedRenderTextureFormat(GraphicsDeviceType type)
        {
            var graphicsFormat = GetSupportedGraphicsFormat(type);
            return GraphicsFormatUtility.GetRenderTextureFormat(graphicsFormat);
        }

        public static TextureFormat GetSupportedTextureFormat(GraphicsDeviceType type)
        {
            var graphicsFormat = GetSupportedGraphicsFormat(type);
            return GraphicsFormatUtility.GetTextureFormat(graphicsFormat);
        }
    }
    
    public static class StringUtility
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }

        public static string EraseWhiteSpace(this string source)
        {
            var erased = string.Concat(source.Where(c => !char.IsWhiteSpace(c)));
            return erased;
        }
    
        public static string GetPath(this Component component) {
            return component.transform.GetPath() + "/" + component.GetType().ToString();
        }
    
        public static string GetPath(this Transform current) {
            if (current.parent == null) return current.name;
            return current.parent.GetPath() + "/" + current.name;
        }
        
        public static string[] GetStringArrayFromEnum(this Type t)
        {
            return Enum.GetNames(t);
        }
        
        public static int GetEnumCount(this Type t)
        {
            return Enum.GetNames(t).Length;
        }
        
        public static T ConvertToEnum<T>(this string _str)
        {
            try { return (T)Enum.Parse(typeof(T), _str); }
            catch { return (T)Enum.Parse(typeof(T), "none"); }
        }
        
        public static string ConvertToString<T>(this T _enum) where T : Enum
        {
            try { return Enum.GetName(typeof(T), _enum); }
            catch { return String.Empty; }
        }
    }
}
