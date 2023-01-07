using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WebViewExtension
{
    public static string GetUserAgent()
    {
        string platform = "Android 9";

#if UNITY_IOS
        platform = "iOS 12";
#endif
        string userAgent = $"{Application.productName}/{Application.version} ({platform})";

        return userAgent;
    }
}
