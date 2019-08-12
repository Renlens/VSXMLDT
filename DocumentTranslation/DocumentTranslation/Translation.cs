using Renlen.BaseLibrary.Secret;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Renlen.BaseLibrary
{
  /// <summary>
  /// 百度翻译网络API。此类不能被继承。
  /// </summary>
  public sealed class Translation
  {
    public event EventHandler LengthChange;
    public event EventHandler Canceled;
    private static Random Random
    {
      get
      {
        if (_Random == null) _Random = new Random();
        return _Random;
      }
    }
    private static Random _Random;
    private static Regex RegexAppID
    {
      get
      {
        if (_RegexAppID == null) _RegexAppID = new Regex(@"^\d+$");
        return _RegexAppID;
      }
    }
    private static Regex _RegexAppID;
    private static Regex RegexSecretKey
    {
      get
      {
        if (_RegexSecretKey == null) _RegexSecretKey = new Regex(@"^[a-zA-Z0-9]+$");
        return _RegexSecretKey;
      }
    }
    private static Regex _RegexSecretKey;
    //private static Regex RegexErrorCode
    //{
    //  get
    //  {
    //    if (_RegexErrorCode == null) _RegexErrorCode = new Regex(@"^\d{5}$");
    //    return _RegexErrorCode;
    //  }
    //}
    //private static Regex _RegexErrorCode;
    private static Uri UriHttp { get; }
      = new Uri(@"http://api.fanyi.baidu.com/api/trans/vip/translate");
    private static Uri UriHttps { get; }
      = new Uri(@"https://fanyi-api.baidu.com/api/trans/vip/translate");
    private string AppID { get; set; }
    private string SecretKey { get; set; }
    public Language SourceLanguage { get; set; } = Language.auto;
    public Language TargetLanguage { get; set; } = Language.zh;
    private long _Length;
    public long Length
    {
      get => _Length;
      set
      {
        _Length = value;
        LengthChange?.Invoke(this, new EventArgs());
      }
    }
    public bool Cancel { get; set; }

    /******************************华丽的分割线******************************/

    public Translation()
    {
      AppID = "";
      SecretKey = "";
    }
    public Translation(string appID, string secretKey, bool isCheck = true)
    {
      if (appID == null)
        throw new ArgumentNullException("appID", "appID 不能为空。");
      if (secretKey == null)
        throw new ArgumentNullException("secure", "secure 不能为空。");
      if (isCheck && !CheckAppID(appID))
        throw new ArgumentException("appID 不合法。", "appID");
      if (isCheck && !CheckSecretKey(secretKey))
        throw new ArgumentException("secure 不合法。", "secure");
      AppID = appID;
      SecretKey = secretKey;
    }

    /******************************华丽的分割线******************************/

    public static bool CheckAppID(string appID) => RegexAppID.IsMatch(appID);
    public static bool CheckSecretKey(string secretKey) => RegexSecretKey.IsMatch(secretKey);

    public void SetAppID(string appID, bool isCheck = true)
    {
      if (appID == null)
        throw new ArgumentNullException("appID", "appID 不能为空。");
      if (isCheck && !RegexAppID.IsMatch(appID))
        throw new ArgumentException("appID 不合法。", "appID");
      AppID = appID;
    }
    public void SetSecure(string secure, bool isCheck = true)
    {
      if (secure == null)
        throw new ArgumentNullException("secure", "secure 不能为空。");
      if (isCheck && !RegexSecretKey.IsMatch(secure))
        throw new ArgumentException("secure 不合法。", "secure");
      SecretKey = secure;
    }

    public string TranslationString(string str)
    {
      return TranslationString(str, SourceLanguage, TargetLanguage);
    }
    public string TranslationString(string str, Language sourceLanguage, Language targetLanguage)
    {
      //if (targetLanguage == Language.auto)
      //  throw new ArgumentException("目标语言不能是 auto 。", "targetLanguage");
      if (string.IsNullOrWhiteSpace(str))
      {
        return $@"{{""from"":""{sourceLanguage.ToString()}"",""to"":""{targetLanguage.ToString()}"",""trans_result"":[{{""src"":"""",""dst"":""""}}]}}";
      }
      Length += str.Length;
      if (Cancel)
      {
        //[Dim: System.Caption]
        //Caption_MAX=0=每月翻译字数将要达到上限，用户选择停止。
        Canceled?.Invoke(this, new EventArgs());
        Cancel = false;
        return null;
      }
      string r;
      string salt = Random.Next().ToString();

      string get = string.Format(
      "?q={0}&from={1}&to={2}&appid={3}&salt={4}&sign={5}"
      , Uri.EscapeDataString(str)
      , sourceLanguage.ToString()
      , targetLanguage.ToString()
      , AppID
      , salt
      , $"{AppID}{str}{salt}{SecretKey}".GetMD5());

      HttpWebRequest web = (HttpWebRequest)WebRequest.Create(UriHttp + get);

      //r = @"{ ""error_code"" : ""333"" }";
      //r = @"{""from"":""en"",""to"":""zh"",""trans_result"":[{""src"":""Test"",""dst"":""\u8bd5\u9a8c""}]}";
      try
      {
        using (WebResponse ls = web.GetResponse())
        using (StreamReader sr = new StreamReader(ls.GetResponseStream(), Encoding.UTF8))
        {
          r = sr.ReadToEnd();
        }
      }
      catch
      {
        r = @"{ ""error_code"" : ""333"" }";
      }

      return r;
      //return JsonDocument.LoadJson(r);
    }


    //public async Task<Dictionary<string, string>> TranslationStringAsync(string str)
    //{
    //  return await TranslationStringAsync(str, SourceLanguage, TargetLanguage);
    //}
    //public async Task<Dictionary<string, string>> TranslationStringAsync(string str, Language sourceLanguage, Language targetLanguage)
    //{
    //  //if (targetLanguage == Language.auto)
    //  //  throw new ArgumentException("目标语言不能是 auto 。", "targetLanguage");

    //  string salt = Random.Next().ToString();
    //  string r = "";

    //  string get = string.Format(
    //  "?q={0}&from={1}&to={2}&appid={3}&salt={4}&sign={5}"
    //  , Uri.EscapeDataString(str)
    //  , sourceLanguage.ToString()
    //  , targetLanguage.ToString()
    //  , AppID
    //  , salt
    //  , $"{AppID}{str}{salt}{SecretKey}".GetMD5());

    //  HttpWebRequest web = (HttpWebRequest)WebRequest.Create(UriHttp + get);

    //  using (StreamReader sr = new StreamReader((await web.GetResponseAsync()).GetResponseStream(), Encoding.UTF8))
    //  {
    //    r = await sr.ReadToEndAsync();
    //  }

    //  Dictionary<string, string> value = new Dictionary<string, string>();
    //  foreach (var item in RegexSplit.Split(r))
    //  {
    //    if (!RegexKeyValue.IsMatch(item)) continue;
    //    string ls = item.Replace("\"", "").Replace(" ", "");
    //    int index = ls.IndexOf(':');
    //    value.Add(ls.Substring(0, index), Uri.EscapeUriString(ls.Substring(index + 1)));
    //  }
    //  return value;
    //}
  }

  /// <summary>
  /// 支持翻译的语言列表，目标语言不可使用 <see cref="Language.auto"/> .
  /// </summary>
  public enum Language
  {
    /// <summary>
    /// 自动检测
    /// </summary>
    auto,
    /// <summary>
    /// 中文
    /// </summary>
    zh,
    /// <summary>
    ///	 英语
    /// </summary>
    en,
    /// <summary>
    ///	粤语
    /// </summary>
    yue,
    /// <summary>
    ///文言文
    /// </summary>
    wyw,
    /// <summary>
    /// 日语
    /// </summary>
    jp,
    /// <summary>
    ///	韩语
    /// </summary>
    kor,
    /// <summary>
    ///法语
    /// </summary>
    fra,
    /// <summary>
    ///西班牙语
    /// </summary>
    spa,
    /// <summary>
    /// 泰语
    /// </summary>
    th,
    /// <summary>
    ///	阿拉伯语
    /// </summary>
    ara,
    /// <summary>
    /// 俄语
    /// </summary>
    ru,
    /// <summary>
    ///	 葡萄牙语
    /// </summary>
    pt,
    /// <summary>
    ///	德语
    /// </summary>
    de,
    /// <summary>
    /// 意大利语
    /// </summary>
    it,
    /// <summary>
    /// 希腊语
    /// </summary>
    el,
    /// <summary>
    ///  荷兰语
    /// </summary>
    nl,
    /// <summary>
    ///	 波兰语
    /// </summary>
    pl,
    /// <summary>
    ///	保加利亚语
    /// </summary>
    bul,
    /// <summary>
    ///爱沙尼亚语
    /// </summary>
    est,
    /// <summary>
    ///丹麦语
    /// </summary>
    dan,
    /// <summary>
    ///芬兰语
    /// </summary>
    fin,
    /// <summary>
    ///捷克语
    /// </summary>
    cs,
    /// <summary>
    /// 罗马尼亚语
    /// </summary>
    rom,
    /// <summary>
    ///斯洛文尼亚语
    /// </summary>
    slo,
    /// <summary>
    ///瑞典语
    /// </summary>
    swe,
    /// <summary>
    /// 匈牙利语
    /// </summary>
    hu,
    /// <summary>
    ///	繁体中文
    /// </summary>
    cht,
    /// <summary>
    ///越南语
    /// </summary>
    vie
  }

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

  /// <summary>
  /// MD5扩展
  /// </summary>
  public static class EXMD5
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
