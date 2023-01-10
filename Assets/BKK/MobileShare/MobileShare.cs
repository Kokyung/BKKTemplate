using UnityEngine;

namespace BKK.Native
{
    public static class MobileShareModule
    {
        public static void ShareTextMessage(string title, string message, NativeShare.ShareResultCallback callback = null)
        {
            NativeShare share = new NativeShare();
        
            callback += OnShareResult;
        
            share.SetTitle(title);
            share.SetText(message);
            share.SetCallback(callback);
            share.Share();
        }
    
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