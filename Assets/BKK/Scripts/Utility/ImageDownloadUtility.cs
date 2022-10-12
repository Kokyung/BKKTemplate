using System;
using System.Collections;
using BKK.Utility;
using UnityEngine;
using UnityEngine.Networking;

namespace BKK.Tools
{
    /// <summary>
    /// 이미지 다운로드 및 저장 유틸리티 클래스입니다.
    /// 
    /// 작성자: 변고경
    /// </summary>
    public sealed class ImageDownloadUtility
    {
        private static int timeOut = 10;

        /// <summary>
        /// 이미지를 다운로드 합니다.
        /// 코루틴이므로 MonoBehaviour 클래스 내에서 StartCoroutine을 이용해서 호출해야합니다.
        /// </summary>
        /// <param name="url">이미지 링크. 링크 마지막이 파일 확장자명으로 끝나야합니다.</param>
        /// <param name="resultAction">다운로드 받은 텍스쳐 받아서 실행할 액션</param>
        /// <returns></returns>
        public static IEnumerator DownloadTexture(string url, Action<Texture> resultAction)
        {
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogWarning($"[ImageDownloadUtility] 링크가 존재하지 않습니다.");
                yield break;
            }

            var request = UnityWebRequestTexture.GetTexture(url);

            request.timeout = timeOut;

            yield return request.SendWebRequest();

            Debug.Log($"[ImageDownloadUtility] 데이터 요청 결과: {request.result}");

            switch (request.result)
            {
                case UnityWebRequest.Result.Success:
                {
                    Debug.LogWarning($"[ImageDownloadUtility] 다운로드 성공하였습니다. / {request.error} / {url}");
                    var tex = ((DownloadHandlerTexture) request.downloadHandler).texture;
                    // var tex = new Texture2D(8, 8);
                    // tex.LoadImage(request.downloadHandler.data);
                    resultAction(tex);
                    break;
                }
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogWarning($"[ImageDownloadUtility] 연결이 끊겼으므로 재다운로드를 진행합니다. / {request.error} / {url}");
                    //CoroutineHelper.StartCoroutine(DownloadTexture(url, resultAction));
                    //yield break;
                    break;
                case UnityWebRequest.Result.InProgress:
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogWarning($"[ImageDownloadUtility] URL에 문제가 있습니다. / {request.error} / {url}");
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    break;
                default:
                    Debug.LogWarning($"[ImageDownloadUtility] 이미지 다운로드를 실패하였습니다. / {request.error} / {url}");
                    break;
            }
        }

        /// <summary>
        /// 텍스쳐를 PNG 파일로 저장합니다.
        /// </summary>
        /// <param name="tex">타겟 텍스쳐</param>
        /// <param name="name">저장시 파일 이름</param>
        /// <param name="folderPath">Application.persistentDataPath내의 폴더 경로.</param>
        public static void SaveTextureToPNG(Texture2D tex, string name, string folderPath = "/Downloaded Image",
            bool addNumber = false)
        {
            if (folderPath[0] != '/') folderPath = '/' + folderPath;

            var bin = tex.EncodeToPNG();

            var savePath = Application.persistentDataPath + folderPath + $"/{name}.png";

            FileUtility.CreateFile(savePath, bin, addNumber);
        }

        /// <summary>
        /// 텍스쳐를 JPG 파일로 저장합니다.
        /// </summary>
        /// <param name="tex">타겟 텍스쳐</param>
        /// <param name="name">저장시 파일 이름</param>
        /// <param name="folderPath">Application.persistentDataPath 내의 폴더 경로.</param>
        public static void SaveTextureToJPG(Texture2D tex, string name, string folderPath = "/Downloaded Image",
            bool addNumber = false)
        {
            if (folderPath[0] != '/') folderPath = '/' + folderPath;

            var bin = tex.EncodeToJPG();

            var savePath = Application.persistentDataPath + folderPath + $"/{name}.jpg";

            FileUtility.CreateFile(savePath, bin, addNumber);
        }
    }
}
