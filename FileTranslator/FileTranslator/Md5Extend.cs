using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Renlen.FileTranslator
{
    /// <summary>
    /// MD5扩展
    /// </summary>
    public static class Md5Extend
    {
        private static MD5 MD5
        {
            get
            {
                if (_MD5 == null) _MD5 = MD5.Create();
                return _MD5;
            }
        }
        private static MD5 _MD5;
        /// <summary>
        /// 获取指定字符串的MD5校验值，默认编码为 UTF-8 。
        /// </summary>
        /// <param name="data">要校验的字符串</param>
        /// <param name="encoding">字符串编码，默认为 UTF-8 。</param>
        /// <returns>字符串的校验值，为32位小写字母数字组合。</returns>
        public static string GetMD5(this string data, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            return encoding.GetBytes(data).GetMD5();
        }
        /// <summary>
        /// 获取指定字节数组的MD5校验值。
        /// </summary>
        /// <param name="data">要校验的字节数组</param>
        /// <returns>字节数组的校验值，为32位小写字母数字组合。</returns>
        public static string GetMD5(this byte[] data)
        {
            return MD5.ComputeHash(data).ToShow();
        }
        /// <summary>
        /// 获取指定流的MD5校验值。
        /// </summary>
        /// <param name="stream">要校验的流</param>
        /// <returns>流的校验值，为32位小写字母数字组合。</returns>
        public static string GetMD5(this Stream stream)
        {
            return MD5.ComputeHash(stream).ToShow();
        }
        /// <summary>
        /// 将字节数组转换为可见字符串，默认格式为十六进制小写字符串。
        /// </summary>
        /// <param name="data">要转换的字节数组</param>
        /// <param name="formt">转换格式，即 <see cref="byte.ToString(string)"/> 的参数。</param>
        /// <returns></returns>
        public static string ToShow(this byte[] data, string formt = "x2")
        {
            StringBuilder str = new StringBuilder();
            foreach (byte d in data)
            {
                str.Append(d.ToString(formt));
            }
            return str.ToString();
        }
    }
}
