using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BKK.Utility
{
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
}

