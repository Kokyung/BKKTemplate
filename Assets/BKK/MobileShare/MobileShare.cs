using UnityEngine;

namespace BKK.Native
{
    public static class MobileShare
    {
        /// <summary>
        /// 문자 메시지를 공유합니다.(모바일 전용)
        /// </summary>
        /// <param name="title">제목</param>
        /// <param name="message">메세지</param>
        /// <param name="callback">공유 성공시 콜백</param>
        public static void ShareTextMessage(string title, string message, NativeShare.ShareResultCallback callback = null)
        {
            NativeShare share = new NativeShare();
        
            callback += OnShareResult;
        
            share.SetTitle(title);
            share.SetText(message);
            share.SetCallback(callback);
            share.Share();
        }
    
        /// <summary>
        /// Texture2D를 공유합니다.(모바일 전용)
        /// </summary>
        /// <param name="texture">이미지</param>
        /// <param name="title">제목</param>
        /// <param name="message">메세지</param>
        /// <param name="callback">공유 성공시 콜백</param>
        public static void ShareTexture2D(Texture2D texture, string title, string message, NativeShare.ShareResultCallback callback = null)
        {
            NativeShare share = new NativeShare();
        
            callback += OnShareResult;

            share.AddFile(texture);
            share.SetTitle(title);
            share.SetText(message);
            share.SetCallback(callback);
            share.Share();
        }

        private static void OnShareResult(NativeShare.ShareResult result, string shareTarget)
        {
            Debug.Log("Share result: " + result + ", selected app: " + shareTarget);
        }
    }
}