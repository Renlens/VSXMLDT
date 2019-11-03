using System.Text.RegularExpressions;

namespace Renlen.FileTranslator
{
    public class BaiduUser
    {
        /// <summary>
        /// 用于验证 AppID 的正则表达式
        /// </summary>
        private static Regex RegexAppID
        {
            get
            {
                if (_RegexAppID == null) _RegexAppID = new Regex(@"^\d+$");
                return _RegexAppID;
            }
        }
        private static Regex _RegexAppID;
        /// <summary>
        /// 用于验证 SecretKey 的正则表达式
        /// </summary>
        private static Regex RegexSecretKey
        {
            get
            {
                if (_RegexSecretKey == null) _RegexSecretKey = new Regex(@"^[a-zA-Z0-9]+$");
                return _RegexSecretKey;
            }
        }
        private static Regex _RegexSecretKey;
        /// <summary>
        /// 验证 AppID 是否符合格式
        /// </summary>
        /// <param name="appID"></param>
        /// <returns></returns>
        public static bool CheckAppID(string appID) => RegexAppID.IsMatch(appID);
        /// <summary>
        /// 验证 SecretKey 是否符合格式
        /// </summary>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        public static bool CheckSecretKey(string secretKey) => RegexSecretKey.IsMatch(secretKey);
        /// <summary>
        /// 由百度开发平台分配的 AppID
        /// </summary>
        public string AppID { get; private set; }
        /// <summary>
        /// 由百度开发平台分配的 SecretKey
        /// </summary>
        public string SecretKey { get; private set; }
        /// <summary>
        /// 当前服务类型。此类不验证服务类型是否正确，因服务类型设置出错导致的额外支出概不负责。
        /// </summary>
        public BaiduUserType Type { get; set; }
    }
}
