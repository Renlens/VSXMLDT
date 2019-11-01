namespace Renlen.FileTranslator
{
    /// <summary>
    /// 结果代码
    /// </summary>
    public enum ResultCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 52000,
        /// <summary>
        /// 网络异常。请检查是否连接到网络。
        /// </summary>
        NetworkError = 333,
        /// <summary>
        /// 请求超时。请重试。
        /// </summary>
        QequestTimedOut = 52001,
        /// <summary>
        /// 系统错误。请重试。
        /// </summary>
        SystemError = 52002,
        /// <summary>
        /// 未授权用户。检查您的 appid 是否正确，或者服务是否开通。
        /// </summary>
        UnauthorizedUser = 52003,
        /// <summary>
        /// 必填参数为空。检查是否少传参数。
        /// </summary>
        ArgumentIsNull = 54000,
        /// <summary>
        /// 签名错误。请检查您的签名生成方法。
        /// </summary>
        SignatureError = 54001,
        /// <summary>
        /// 访问频率受限。请降低您的调用频率。
        /// </summary>
        LimitedAccessFrequency = 54003,
        /// <summary>
        /// 账户余额不足。请前往管理控制台为账户充值。
        /// </summary>
        InsufficientAccountBalance = 54004,
        /// <summary>
        /// 长 Query 请求频繁。请降低长 Query 的发送频率，3s后再试。
        /// </summary>
        LongRequestFrequent = 54005,
        /// <summary>
        /// 客户端IP非法。
        /// 检查个人资料里填写的 IP地址 是否正确
        /// 可前往管理控制平台修改，
        /// IP限制，IP可留空。
        /// </summary>
        ClientIPIsIllegal = 58000,
        /// <summary>
        /// 译文语言方向不支持。检查译文语言是否在语言列表里。
        /// </summary>
        LanguageIsNonSupported = 58001,
        /// <summary>
        /// 服务当前已关闭。请前往管理控制台开启服务。
        /// </summary>
        ServiceIsCurrentlyClosed = 58002
    }
}
