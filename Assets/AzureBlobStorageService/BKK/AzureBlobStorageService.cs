using System;
using System.IO;
using System.Threading.Tasks;
using Azure.StorageServices;
using Cysharp.Threading.Tasks;
using RESTClient;
using UnityEngine;

namespace Azure.Custom
{
    /// <summary>
    /// Azure Blob Storage에 데이터를 업로드/다운로드할 수 있게 해주는 컴포넌트
    ///
    /// 다음 링크 내용을 참고하였습니다.
    /// https://answers.unity.com/questions/1833982/how-to-get-a-text-file-video-from-azure-blob-stora.html
    /// </summary>
    public class AzureBlobStorageService : MonoBehaviour
    {
        [Tooltip("스토리지 계정 이름.\nAzure Portal에서 해당 [스토리지 계정 -> 엑세스 키]에서 확인 가능합니다.")]
        [SerializeField] private string storageAccountName = "BKK";
        [Tooltip("스토리지 계정의 엑세스 키.\nAzure Portal에서 해당 [스토리지 계정 -> 엑세스 키]에서 확인 가능합니다.")]
        [SerializeField] private string accessKey = "";

        [Tooltip("기본 컨테이너 경로")]
        [SerializeField] private string defaultContainerPath = "Container/Cache";
        [Tooltip("비디오 컨테이너 경로")]
        [SerializeField] private string videoContainerPath = "Container/Cache/Video";
        [Tooltip("오디오 컨테이너 경로")]
        [SerializeField] private string audioContainerPath = "Container/Cache/Audio";
        [Tooltip("이미지 컨테이너 경로")]
        [SerializeField] private string imageContainerPath = "Container/Cache/Image";

        private StorageServiceClient client;
        private BlobService blobService;
        private FilePicker filePicker;

        #region Debug

        private string tempFileName;
        private string tempPath;

        [Tooltip("모바일 디버깅용 GUI 활성화 여부")]
        [SerializeField] private bool debug = false;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            Init();
        }

        private void OnGUI()
        {
            if(!debug) return;
            
            GUI.Label(new Rect(Screen.width/2 - 200, 50,400,50), "본인 Azure Storage를 이용하여 설정하시기 바랍니다.");
            
            if (GUI.Button(new Rect(50, 50, 100, 40), "비디오 업로드"))
            {
                PickAndUploadVideoFile();
            }
            if (GUI.Button(new Rect(50, 100, 100, 40), "오디오 업로드"))
            {
                PickAndUploadAudioFile();
            }
            if (GUI.Button(new Rect(50, 150, 100, 40), "이미지 업로드"))
            {
                PickAndUploadImageFile();
            }
            if (GUI.Button(new Rect(50, 200, 200, 40), "직전 업로드된 파일 다운로드"))
            {
                DownloadFileDemo();
            }
            if (GUI.Button(new Rect(50, 250, 200, 40), "직전 업로드된 파일 삭제"))
            {
                DeleteVideoFileDemo();
            }
        }

        #endregion

        #region Initialize

        private void Init()
        {
            client = StorageServiceClient.Create(storageAccountName, accessKey);
            blobService = client.GetBlobService();

            filePicker = FindObjectOfType<FilePicker>();

            if (!filePicker) filePicker = this.gameObject.AddComponent<FilePicker>();
        }

        #endregion

        #region Main Function
        
        /// <summary>
        /// 파일 폴더에서 선택한 비디오 파일을 업로드한다.
        /// </summary>
        public void PickAndUploadVideoFile()
        {
            filePicker.PickVideoFile(async delegate(string[] paths)
            {
                foreach (var path in paths)
                {
                    var fileName = path.Substring(path.LastIndexOf('/') + 1);
                    var fileFormat = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();
                    await UploadVideoFile(path, videoContainerPath, fileName, fileFormat);
                    
                    tempFileName = fileName;
                    tempPath = videoContainerPath;
                }
            });
        }
        
        public void PickAndUploadVideoFile(Action<string> url)
        {
            filePicker.PickVideoFile(async delegate(string[] paths)
            {
                foreach (var path in paths)
                {
                    var fileName = path.Substring(path.LastIndexOf('/') + 1);
                    var fileFormat = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();
                    await UploadVideoFile(path, videoContainerPath, fileName, fileFormat, delegate(RestResponse response)
                    {
                        if (!response.IsError)
                        {
                            url.Invoke(GetVideoFileUrl(fileName));
                        }
                    });
                    
                    tempFileName = fileName;
                    tempPath = videoContainerPath;
                }
            });
        }
        
        /// <summary>
        /// 파일 폴더에서 선택한 오디오 파일을 업로드한다.
        /// </summary>
        public void PickAndUploadAudioFile()
        {
            filePicker.PickAudioFile(async delegate(string[] paths)
            {
                foreach (var path in paths)
                {
                    var fileName = path.Substring(path.LastIndexOf('/') + 1);
                    var fileFormat = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();
                    await UploadAudioFile(path, audioContainerPath, fileName, fileFormat);
                    
                    tempFileName = fileName;
                    tempPath = audioContainerPath;
                }
            });
        }
        
        /// <summary>
        /// 파일 폴더에서 선택한 이미지 파일을 업로드한다.
        /// </summary>
        public void PickAndUploadImageFile()
        {
            filePicker.PickImageFile(async delegate(string[] paths)
            {
                foreach (var path in paths)
                {
                    var fileName = path.Substring(path.LastIndexOf('/') + 1);
                    var fileFormat = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();
                    await UploadImageFile(path, imageContainerPath, fileName, fileFormat);
                    
                    tempFileName = fileName;
                    tempPath = imageContainerPath;
                }
            });
        }
        
        /// <summary>
        /// 컨테이너에 비디오 파일을 다운로드한다.
        /// </summary>
        /// <param name="fileName">파일명</param>
        public async void DownloadVideoFile(string fileName)
        {
            var blobFilePath = $"{videoContainerPath}/{fileName}";

            await DownloadVideoFile(delegate(IRestResponse<byte[]> response)
            {
                if (!response.IsError)
                {
                    var path = $"{Application.persistentDataPath}/{blobFilePath}";

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    File.WriteAllBytes($"{Application.persistentDataPath}/{blobFilePath}", response.Data);
                }
            }, blobFilePath);
        }
        
        /// <summary>
        /// 컨테이너에 오디오 파일을 다운로드한다.
        /// </summary>
        /// <param name="fileName">파일명</param>
        /// <param name="afterAction">다운로드 후 액션</param>
        public async void DownloadAudioFile(string fileName, Action<IRestResponse<AudioClip>> afterAction)
        {
            var blobFilePath = $"{audioContainerPath}/{fileName}";

            await DownloadAudioFile(afterAction, blobFilePath);
        }
        
        /// <summary>
        /// 컨테이너에 이미지 파일을 다운로드한다.
        /// </summary>
        /// <param name="fileName">파일명</param>
        /// <param name="afterAction">다운로드 후 액션</param>
        public async void DownloadImageFile(string fileName, Action<IRestResponse<Texture>> afterAction)
        {
            var blobFilePath = $"{imageContainerPath}/{fileName}";

            await DownloadImageFile(afterAction, blobFilePath);
        }

        /// <summary>
        /// 컨테이너의 비디오 파일을 삭제한다.
        /// </summary>
        /// <param name="fileName">파일명</param>
        public void DeleteRemoteVideoFile(string fileName)
        {
            DeleteFile(videoContainerPath, fileName);
        }
        
        /// <summary>
        /// 컨테이너의 오디오 파일을 삭제한다.
        /// </summary>
        /// <param name="fileName">파일명</param>
        public void DeleteRemoteAudioFile(string fileName)
        {
            DeleteFile(audioContainerPath, fileName);
        }
        
        /// <summary>
        /// 컨테이너의 이미지 파일을 삭제한다.
        /// </summary>
        /// <param name="fileName"></param>
        public void DeleteRemoteImageFile(string fileName)
        {
            DeleteFile(imageContainerPath, fileName);
        }
        
        /// <summary>
        /// 파일의 경로를 리턴한다.
        /// </summary>
        /// <param name="containerPath">컨테이너 경로</param>
        /// <param name="fileName">파일명</param>
        /// <returns></returns>
        public string GetFileUrl(string containerPath, string fileName)
        {
            return $"https://{storageAccountName}.blob.core.windows.net/{containerPath}/{fileName}";
        }
        
        /// <summary>
        /// 기본 컨테이너에 있는 파일 경로를 리턴한다.
        /// </summary>
        /// <param name="fileName">파일명</param>
        /// <returns></returns>
        public string GetFileUrl(string fileName)
        {
            return GetFileUrl(defaultContainerPath, fileName);
        }
        
        /// <summary>
        /// 비디오 컨테이너에 있는 파일 경로를 리턴한다.
        /// </summary>
        /// <param name="fileName">파일명</param>
        /// <returns></returns>
        public string GetVideoFileUrl(string fileName)
        {
            return GetFileUrl(videoContainerPath, fileName);
        }
        
        /// <summary>
        /// 오디오 컨테이너에 있는 파일 경로를 리턴한다.
        /// </summary>
        /// <param name="fileName">파일명</param>
        /// <returns></returns>
        public string GetAudioFileUrl(string fileName)
        {
            return GetFileUrl(audioContainerPath, fileName);
        }
        
        /// <summary>
        /// 이미지 컨테이너에 있는 파일 경로를 리턴한다.
        /// </summary>
        /// <param name="fileName">파일명</param>
        /// <returns></returns>
        public string GetImageFileUrl(string fileName)
        {
            return GetFileUrl(imageContainerPath, fileName);
        }

        #endregion
        
        #region Upload

        public async Task UploadVideoFile(string filePath, string blobPath, string fileNameOnBlob, string fileType = "mp4", Action<RestResponse> response = null)
        {
            var data = File.ReadAllBytes(filePath);
            var contentType = $"video/{fileType}";
            
            await UploadFile(response, data, blobPath, fileNameOnBlob, contentType);
        }
        
        public async Task UploadAudioFile(string filePath, string blobPath, string fileNameOnBlob, string fileType = "wav", Action<RestResponse> response = null)
        {
            var data = File.ReadAllBytes(filePath);
            var contentType = $"audio/{fileType}";
            
            await UploadFile(response, data, blobPath, fileNameOnBlob, contentType);
        }
        
        public async Task UploadImageFile(string filePath, string blobPath, string fileNameOnBlob, string fileType = "png", Action<RestResponse> response = null)
        {
            var data = File.ReadAllBytes(filePath);
            var contentType = $"image/{fileType}";
            
            await UploadFile(response, data, blobPath, fileNameOnBlob, contentType);
        }
        
        public async Task UploadText(string filePath, string blobPath, string fileNameOnBlob, Action<RestResponse> response = null)
        {
            var data = File.ReadAllBytes(filePath);
            var contentType = "text/plain; charset=UTF-8";
            
            await UploadFile(response, data, blobPath, fileNameOnBlob, contentType);
        }
        
        public async Task UploadAssetBundle(string filePath, string blobPath, string fileNameOnBlob, Action<RestResponse> response = null)
        {
            var data = File.ReadAllBytes(filePath);
            var contentType = "application/octet-stream";
            
            await UploadFile(response, data, blobPath, fileNameOnBlob, contentType);
        }
        
        public async Task UploadFile(Action<RestResponse> response, byte[] data, string blobPath, string fileNameOnBlob, string contentType)
        {
            var convertedName = Uri.EscapeUriString(fileNameOnBlob);
            
            response += OnPutBlobResponse;

            var putRoutine = blobService.PutBlob(response, data, blobPath, convertedName, contentType);

            await putRoutine;
        }

        #endregion
        
        #region Download
        
        public async Task DownloadVideoFile(Action<IRestResponse<byte[]>> responseAction, string blobFilePath)
        {
            responseAction += OnGetBlobResponse;

            var getRoutine = blobService.GetBlob(responseAction, Uri.EscapeUriString(blobFilePath));

            await getRoutine;
        }
        
        public async Task DownloadAudioFile(Action<IRestResponse<AudioClip>> responseAction, string blobFilePath)
        {
            responseAction += OnGetBlobResponse;

            var getRoutine = blobService.GetAudioBlob(responseAction, Uri.EscapeUriString(blobFilePath));

            await getRoutine;
        }
        
        public async Task DownloadImageFile(Action<IRestResponse<Texture>> responseAction, string blobFilePath)
        {
            responseAction += OnGetBlobResponse;

            var getRoutine = blobService.GetImageBlob(responseAction, Uri.EscapeUriString(blobFilePath));

            await getRoutine;
        }
        
        public async Task DownloadTextFile(Action<RestResponse> responseAction, string blobFilePath)
        {
            responseAction += OnGetBlobResponse;

            var getRoutine = blobService.GetTextBlob(responseAction, Uri.EscapeUriString(blobFilePath));

            await getRoutine;
        }
        
        public async Task DownloadAssetBundle(Action<IRestResponse<AssetBundle>> responseAction, string blobFilePath)
        {
            responseAction += OnGetBlobResponse;

            var getRoutine = blobService.GetAssetBundle(responseAction, Uri.EscapeUriString(blobFilePath));

            await getRoutine;
        }

        public async Task DownloadBlob(Action<IRestResponse<byte[]>> responseAction, string blobFilePath)
        {
            responseAction += OnGetBlobResponse;

            var getRoutine = blobService.GetBlob(responseAction, Uri.EscapeUriString(blobFilePath));

            await getRoutine;
        }

        #endregion

        #region Delete

        public async void DeleteFile(string blobPath, string fileName)
        {
            await DeleteBlob(delegate {  }, blobPath, fileName);
        }

        private async Task DeleteBlob(Action<RestResponse> response, string blobPath, string fileName)
        {
            response += OnDeleteBlobResponse;
            await blobService.DeleteBlob(response, blobPath, Uri.EscapeUriString(fileName));
        }

        #endregion
        
        #region Callback

        private void OnPutBlobResponse(RestResponse response)
        {
            Debug.Log($"애저 Blob 업로드: {!response.IsError} / {response.ErrorMessage} / {response.Url}");

            HandleError(response.ErrorMessage);
        }

        private void OnGetBlobResponse<T>(IRestResponse<T> response)
        {
            Debug.Log($"애저 Blob 다운로드: {!response.IsError} / {response.ErrorMessage} / {response.Url}");
            
            HandleError(response.ErrorMessage);
        }

        private void OnGetBlobResponse(RestResponse response)
        {
            Debug.Log($"애저 Blob 다운로드: {!response.IsError} / {response.ErrorMessage} / {response.Url}");
            
            HandleError(response.ErrorMessage);
        }
        
        private void OnDeleteBlobResponse(RestResponse response)
        {
            Debug.Log($"애저 Blob 삭제: {!response.IsError} / {response.ErrorMessage} / {response.Url}");

            HandleError(response.ErrorMessage);
        }

        private void HandleError(string errorMessage)
        {
            if (string.IsNullOrEmpty(errorMessage)) return;

            if (errorMessage.Contains("RequestEntityTooLarge"))
            {
                Debug.LogWarning(
                    "Azure Blob Storage에 업로드 용량 제한이 걸려있습니다.\n" +
                    "버전 2016-05-31 ~ 버전 2019-07-07에서는 일반적으로 256Mb 이상의 데이터는 업로드 불가합니다.\n" +
                    "업로드를 원한다면 (스토리지 계정->설정->구성->대용량 파일 공유)를 체크하세요.");
            }
        }

        #endregion

        #region Demo

        private async void DownloadFileDemo()
        {
            var blobFilePath = $"{tempPath}/{tempFileName}";

            await DownloadVideoFile(delegate(IRestResponse<byte[]> response)
            {
                if (!response.IsError)
                {
                    var path = $"{Application.persistentDataPath}/{tempPath}/";

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    File.WriteAllBytes($"{path + tempFileName}", response.Data);
                }
            }, blobFilePath);
        }
        
        private async void DeleteVideoFileDemo()
        {
            await DeleteBlob(delegate {  }, tempPath, tempFileName);
        }

        #endregion
    }
}