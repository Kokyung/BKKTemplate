using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

namespace BKK.Utility
{
    public static class GraphicsExperimentalUtility
    {
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
}
