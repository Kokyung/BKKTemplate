using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BKK.Utility
{
    /// <summary>
    /// 모바일 파일 폴더에 접근하여 선택한 파일의 경로를 가져올 수 있게 해줍니다.
    ///
    /// 변고경
    /// </summary>
    public class FilePicker : MonoBehaviour
    {
        [SerializeField] private List<string> fileTypeList = new List<string>() {"pdf"};
        private List<string> fileMIMEList = new List<string>();
        private List<string> filePathList = new List<string>();

        private bool completed = false;

        public bool canMultipleSelect = false;

        private void Start()
        {
            ConvertExtensionToFileTypeList(fileTypeList.ToArray());
        }

        /// <summary>
        /// PDF 파일을 가져와 WebviewController를 통해 웹뷰에 표시한다.
        /// </summary>
        public void PickPdfFile(Action<string[]> afterAction)
        {
            PickFile(new[] {"pdf"}, afterAction);
        }

        /// <summary>
        /// 비디오 파일을 가져온다.
        /// </summary>
        /// <param name="afterAction"></param>
        public void PickVideoFile(Action<string[]> afterAction)
        {
            PickFile(new[] {"mp4", "avi", "mkv", "mov"}, afterAction);
        }

        /// <summary>
        /// 폴더 내 선택한 파일 경로 가져오기.
        /// FilePicker 컴포넌트의 인스펙터 변수를 사용.
        /// </summary>
        /// <param name="afterAction"></param>
        public void PickFile(Action<string[]> afterAction)
        {
            PickFile(fileTypeList.ToArray(), afterAction);
        }

        /// <summary>
        /// 폴더 내 선택한 파일 경로 가져오기.
        /// </summary>
        /// <param name="fileTypes">파일 포맷.</param>
        /// <param name="afterAction">가져온 파일 경로로 실행할 이벤트</param>
        public async void PickFile(string[] fileTypes, Action<string[]> afterAction)
        {
            ConvertExtensionToFileTypeList(fileTypes);

            PickPath(canMultipleSelect);

            await UniTask.WaitUntil(() => completed);

            afterAction?.Invoke(filePathList.ToArray());
        }

        /// <summary>
        /// 파일 폴더를 열고 선택한 파일들의 경로를 가져온다.
        /// </summary>
        /// <param name="multipleSelect">다중 선택 여부. 모바일만 사용 가능</param>
        public void PickPath(bool multipleSelect = false)
        {
            completed = false;

            if (multipleSelect)
            {
                if (!NativeFilePicker.CanPickMultipleFiles())
                {
                    Debug.LogWarning("파일 다중 선택은 PC 및 에디터에서 지원하지 않습니다.");
                    return;
                }

                NativeFilePicker.PickMultipleFiles(delegate(string[] paths)
                {
                    filePathList.Clear();
                    SetMultiplePaths(paths);
                    completed = true;
                }, fileMIMEList.ToArray());
            }
            else
            {
                NativeFilePicker.PickFile(delegate(string path)
                {
                    filePathList.Clear();
                    SetPath(path);
                    completed = true;
                }, fileMIMEList.ToArray());
            }
        }

        /// <summary>
        /// 가져온 파일 경로를 리스트에 등록한다.
        /// </summary>
        /// <param name="path">파일 경로</param>
        private void SetPath(string path)
        {
            if (path == null)
            {
                Debug.Log("파일 가져오기를 취소합니다.");
            }
            else
            {
                Debug.Log($"가져온 파일: {path}");
                filePathList.Add(path);
            }
        }

        /// <summary>
        /// 가져온 여러개의 파일 경로를 리스트에 등록한다.
        /// </summary>
        /// <param name="paths">파일 경로들</param>
        private void SetMultiplePaths(string[] paths)
        {
            foreach (var path in paths)
            {
                SetPath(path);
            }
        }

        /// <summary>
        /// 파일 타입을 MIME/UTI로 변환한다.
        /// </summary>
        /// <param name="fileTypes">파일 포맷들</param>
        public void ConvertExtensionToFileTypeList(string[] fileTypes)
        {
            fileMIMEList.Clear();
            foreach (var fileType in fileTypes)
            {
                var extension = NativeFilePicker.ConvertExtensionToFileType(fileType);
                fileMIMEList.Add(extension);

                Debug.Log($"{fileType}의 MIME/UTI: {extension}");
            }
        }
    }
}