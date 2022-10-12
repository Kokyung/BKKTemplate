using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

namespace BKK.Utility
{
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

        public static Texture2D ToTexture2D(this RenderTexture renderTexture)
        {
            var tex = new Texture2D(renderTexture.width, renderTexture.height, renderTexture.graphicsFormat,
                renderTexture.mipmapCount, TextureCreationFlags.None);// 렌더 텍스쳐와 mipmap Count를 일치시키지 않으면 Graphics.CopyTexture()가 동작하지 않습니다.
            
            // 구글링하면 나오는 texture.ReadPixels()을 이용한 예제는 퍼포먼스 문제가 있으므로 Graphics.CopyTexture()를 사용했습니다.
            Graphics.CopyTexture(renderTexture,tex);
            return tex;
        }

        public static RenderTexture ToRenderTexture(this Texture2D texture2D)
        {
            var renderTexture = new RenderTexture((int) texture2D.width, (int) texture2D.height, 0,
                texture2D.graphicsFormat,
                texture2D.mipmapCount)
            {
                filterMode = FilterMode.Trilinear,
                anisoLevel = 8,
                useMipMap = true,
                wrapMode = TextureWrapMode.Repeat,
            };
            Graphics.Blit(texture2D, renderTexture);
            return renderTexture;
        }
    }
}
