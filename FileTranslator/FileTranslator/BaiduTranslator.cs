using System;
using System.IO;
using System.Net;
using System.Text;

namespace Renlen.FileTranslator
{
    /// <summary>
    /// 表示翻译器的一组实现。
    /// </summary>
    public class BaiduTranslator
    {
        private const string HTTP = @"http://api.fanyi.baidu.com/api/trans/vip/translate";
        private const string HTTPS = @"https://fanyi-api.baidu.com/api/trans/vip/translate";

        /// <summary>
        /// 翻译所需随机数生成器
        /// </summary>
        private static Random Random
        {
            get
            {
                if (_Random == null) _Random = new Random();
                return _Random;
            }
        }
        private static Random _Random;

        private static string uri = HTTP;

        private static TransferProtocolType transferProtocol = TransferProtocolType.Http;
        public static TransferProtocolType TransferProtocol
        {
            get => transferProtocol;
            set
            {
                uri = value switch
                {
                    TransferProtocolType.Http => HTTP,
                    TransferProtocolType.Https => HTTPS,
                    _ => throw new ArgumentException("无效的传输协议类型", nameof(value)),
                };
                transferProtocol = value;
            }
        } 

        /// <summary>
        /// 实现类型的名称
        /// </summary>
        public string Name { get; } = "Baidu";
        /// <summary>
        /// 翻译一串字符串，以指定的用户和语言
        /// </summary>
        /// <param name="str"></param>
        /// <param name="user"></param>
        /// <param name="sourceLanguage"></param>
        /// <param name="targetLanguage"></param>
        /// <returns></returns>
        public string TranslationString(string str, BaiduUser user, Language sourceLanguage, Language targetLanguage)
        {
            //if (targetLanguage == Language.auto)
            //  throw new ArgumentException("目标语言不能是 auto 。", "targetLanguage");
            if (string.IsNullOrWhiteSpace(str))
            {
                return $@"{{""from"":""{sourceLanguage.Value}"",""to"":""{targetLanguage.Value}"",""trans_result"":[{{""src"":"""",""dst"":""""}}]}}";
            }
            string r;
            string salt = Random.Next().ToString();

            string get = string.Format(
            "?q={0}&from={1}&to={2}&appid={3}&salt={4}&sign={5}"
            , Uri.EscapeDataString(str)
            , sourceLanguage.Value
            , targetLanguage.Value
            , user.AppID
            , salt
            , $"{user.AppID}{str}{salt}{user.SecretKey}".GetMD5());

            HttpWebRequest web = (HttpWebRequest)WebRequest.Create($"{uri}{get}");

            //测试数据
            //r = @"{ ""error_code"" : ""333"" }";
            //r = @"{""from"":""en"",""to"":""zh"",""trans_result"":[{""src"":""Test"",""dst"":""\u8bd5\u9a8c""}]}";
            try
            {
                using WebResponse ls = web.GetResponse();
                using StreamReader sr = new StreamReader(ls.GetResponseStream(), Encoding.UTF8);
                r = sr.ReadToEnd();
            }
            catch
            {
                r = @"{ ""error_code"" : ""333"" }";
            }

            return r;
            //return JsonDocument.LoadJson(r);
        }
    }

    public enum TransferProtocolType
    {
        Http,
        Https
    }
}
