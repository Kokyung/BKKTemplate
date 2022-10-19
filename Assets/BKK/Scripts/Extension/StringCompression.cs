using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace BKK.Extension
{
    /// <summary>
    /// DeflateStream을 이용한 스트링 압축 함수
    /// Base64 방식으로 인코딩합니다.
    /// https://learn.microsoft.com/en-us/answers/questions/226531/c-best-method-to-reduce-size-of-large-string-data.html
    /// </summary>
    public static class StringCompression
    {
        /// <summary>
        /// 문자열을 압축하여 Base64 문자열로 반환합니다.
        /// </summary>
        /// <param name="uncompressedString">String to compress</param>
        public static string Compress(this string uncompressedString)
        {
            var compressedBytes = Encoding.UTF8.GetBytes(uncompressedString).Compress();

            return Convert.ToBase64String(compressedBytes);
        }

        /// <summary>
        /// Base64 형태로 압축된 문자열을 압축 해제합니다.
        /// </summary>
        /// <param name="compressedString">String to decompress.</param>
        public static string Decompress(this string compressedString)
        {
            var decompressedBytes = Convert.FromBase64String(compressedString).Decompress();

            return Encoding.UTF8.GetString(decompressedBytes);
        }
    }
    
    public static class ByteArrayCompression
    {
        /// <summary>
        /// 바이트 배열을 DeflateStream을 이용하여 압축합니다.
        /// </summary>
        /// <param name="uncompressedBytes">Byte Array to compress</param>
        public static byte[] Compress(this byte[] uncompressedBytes)
        {
            byte[] compressedBytes;
    
            using (var uncompressedStream = new MemoryStream(uncompressedBytes))
            {
                using (var compressedStream = new MemoryStream())
                {
                    using (var compressorStream = new DeflateStream(compressedStream, CompressionMode.Compress, true))
                    {
                        uncompressedStream.CopyTo(compressorStream);
                    }
                    
                    // ToArray()가 호출되려면 DeflateStream이 닫히고 버퍼가 플러쉬 되야 가능하다.
                    compressedBytes = compressedStream.ToArray();
                }
            }
    
            return compressedBytes;
        }
    
        /// <summary>
        /// DeflateStream을 이용하여 압축한 바이트 배열을 압축 해제합니다.
        /// </summary>
        /// <param name="compressedBytes">Byte Array to decompress.</param>
        public static byte[] Decompress(this byte[] compressedBytes)
        {
            byte[] decompressedBytes;
    
            var compressedStream = new MemoryStream(compressedBytes);
    
            using (var decompressorStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
            {
                using (var decompressedStream = new MemoryStream())
                {
                    decompressorStream.CopyTo(decompressedStream);
    
                    decompressedBytes = decompressedStream.ToArray();
                }
            }
    
            return decompressedBytes;
        }
    }
}