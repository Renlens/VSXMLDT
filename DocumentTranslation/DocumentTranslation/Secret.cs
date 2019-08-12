using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.IO;
using static Renlen.BaseLibrary.Secret.RandomNumber;
using Microsoft.Win32;

namespace Renlen.BaseLibrary.Secret
{
  /// <summary>
  /// 加密规则。针对长度较短的字符串。此类不能被继承。
  /// </summary>
  public sealed class Secret
  {
    /// <summary>
    /// 获取与此实例绑定的密钥。
    /// </summary>
    public SecretKey SecretKey { get; }
    /// <summary>
    /// 与此实例关联的字节序列。
    /// </summary>
    private byte[] Key;
    /// <summary>
    /// 与此实例关联的反字节序列。
    /// </summary>
    private byte[] ReverseKey;
    /// <summary>
    /// 使用一个密钥初始化一个 <see cref="Secret"/> 实例。
    /// <para>您可以通过 <see langword="new"/> <see cref="Renlen.Public.Secret.SecretKey.SecretKey(byte[])"/> 新建一个密钥或使用 <see langword="static"/> <see cref="SecretKey.ReadSecretKey(string, byte[], bool)"/> 从文件读取一个密钥 , 使用 <see langword="static"/> <see cref="SecretKey.ReadSecretKey(string, string, byte[], bool)"/> 从注册表读取一个密钥。</para>
    /// </summary>
    /// <param name="secretkey">用来初始化的密钥</param>
    public Secret(SecretKey secretkey)
    {
      SecretKey = secretkey;
      Key = SecretKey._SecretKeyID.GetNewSoftArray();
      ReverseKey = Key.GetReverseKey();
    }

    /// <summary>
    /// 使用该密钥加密一个字节数组。此方法加密后可解密，加密结果唯一。此方法只适合长度较短的字符串，否则将消耗较长时间。
    /// </summary>
    /// <param name="sourusdata"></param>
    /// <returns></returns>
    internal byte[] EncryptionByte(byte[] sourusdata)
    {
      return sourusdata.Encryption(Key);
    }
    /// <summary>
    /// 使用该密钥加密一串字符串。此方法加密的字符串可解密，加密结果唯一。此方法只适合长度较短的字符串，否则将消耗较长时间。
    /// </summary>
    /// <param name="sourusstring"></param>
    /// <returns></returns>
    internal byte[] EncryptionByte(string sourusstring)
    {
      return Encoding.UTF8.GetBytes(sourusstring).Encryption(Key);
    }
    /// <summary>
    /// 使用该密钥解密一个字节数组。
    /// </summary>
    /// <param name="ciphertext"></param>
    /// <returns></returns>
    internal byte[] DecryptByteToByte(byte[] ciphertext)
    {
      return ciphertext.Decrypt(ReverseKey);
    }
    /// <summary>
    /// 使用该密钥解密一个字节数组。并按UTF-8格式解码。
    /// </summary>
    /// <param name="ciphertext"></param>
    /// <returns></returns>
    internal string DecryptByteToString(byte[] ciphertext)
    {
      return Encoding.UTF8.GetString(ciphertext.Decrypt(ReverseKey));
    }
    /// <summary>
    /// 使用该密钥加密一个字节数组。此方法加密后可解密，加密结果不唯一，不能作为字典的键值。此方法只适合长度较短的字符串，否则将消耗较长时间。
    /// </summary>
    /// <param name="sourusdata"></param>
    /// <returns></returns>
    public string EncryptionRandom(byte[] sourusdata)
    {
      if (sourusdata?.Length == 0) return "";
      string head = ((char)Next(65, 91)).ToString();
      string order = head[0].GetOrderString();
      var data = EncryptionByte(sourusdata);
      var key = SecretKey._SecretKeyID.GetMappingRule();
      return head + data.EncryptionMapping(order, key);
    }
    /// <summary>
    /// 使用该密钥加密一串字符串。此方法加密的字符串可解密，加密结果不唯一，不能作为字典的键值。此方法只适合长度较短的字符串，否则将消耗较长时间。
    /// </summary>
    /// <param name="sourusstring"></param>
    /// <returns></returns>
    public string EncryptionRandom(string sourusstring)
    {
      if (sourusstring == "") return "";
      string head = ((char)Next(65, 91)).ToString();
      string order = head[0].GetOrderString();
      var data = EncryptionByte(sourusstring);
      var key = SecretKey._SecretKeyID.GetMappingRule();
      return head + data.EncryptionMapping(order, key);
    }
    /// <summary>
    /// 使用该密钥加密指定的文件，加密后可解密，加密后覆盖原文件。
    /// </summary>
    /// <param name="filepath">需要加密的文件的路径</param>
    public void EncryptionFile(string filepath)
    {
      using (Stream stream = new FileStream(filepath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
      {
        using (BufferedStream bs = new BufferedStream(stream))
        {
          byte[] buffer = new byte[4096];
          int length = 1;
          bs.Seek(0, SeekOrigin.Begin);
          while (length > 0)
          {
            length = bs.Read(buffer, 0, buffer.Length);
            buffer.EncryptionOverwrite(Key);
            bs.Seek(0 - length, SeekOrigin.Current);
            bs.Write(buffer, 0, length);
          }
        }
      }
    }
    /// <summary>
    /// 使用该密钥加密一个流，加密后可解密，加密后覆盖原流。
    /// </summary>
    /// <param name="stream">需要加密的流</param>
    public void EncryptionStream(Stream stream)
    {
      using (BufferedStream bs = new BufferedStream(stream))
      {
        byte[] buffer = new byte[4096];
        int length = 1;
        bs.Seek(0, SeekOrigin.Begin);
        while (length > 0)
        {
          length = bs.Read(buffer, 0, buffer.Length);
          buffer.EncryptionOverwrite(Key);
          bs.Seek(0 - length, SeekOrigin.Current);
          bs.Write(buffer, 0, length);
        }
      }
    }
    /// <summary>
    /// 使用该密钥计算输入的哈希值，其结果为长度为十六的字节数组。
    /// </summary>
    /// <returns></returns>
    public byte[] EncryptionHash(byte[] input)
    {
      return input.MD5().Encryption(Key).MD5();
    }
    /// <summary>
    /// 使用该密钥计算输入的哈希值，其结果为长度为十六的字节数组。
    /// </summary>
    /// <returns></returns>
    public byte[] EncryptionHash(Stream input)
    {
      return input.MD5().Encryption(Key).MD5();
    }
    /// <summary>
    /// 使用该密钥解密一串字符串。结果为字节数组。
    /// </summary>
    /// <param name="ciphertext"></param>
    /// <returns></returns>
    public byte[] DecryptRandomToByte(string ciphertext)
    {
      if (ciphertext?.Length <= 1) return new byte[0];
      return ciphertext.Substring(1).DecryptMapping(ciphertext[0].GetOrderString(), SecretKey._SecretKeyID.GetMappingRule()).Decrypt(ReverseKey);
    }
    /// <summary>
    /// 使用该密钥解密一串字符串。结果为字符串。
    /// </summary>
    /// <param name="ciphertext"></param>
    /// <returns></returns>
    public string DecryptRandomToString(string ciphertext)
    {
      if (ciphertext?.Length <= 1) return "";
      return Encoding.UTF8.GetString(ciphertext.Substring(1).DecryptMapping(ciphertext[0].GetOrderString(), SecretKey._SecretKeyID.GetMappingRule()).Decrypt(ReverseKey));
    }
    /// <summary>
    /// 使用该密钥解密一个流，解密后覆盖原流。
    /// </summary>
    /// <param name="stream">需要加密的流</param>
    public void DecryptStream(Stream stream)
    {
      using (BufferedStream bs = new BufferedStream(stream))
      {
        byte[] buffer = new byte[4096];
        int length = 1;
        bs.Seek(0, SeekOrigin.Begin);
        while (length > 0)
        {
          length = bs.Read(buffer, 0, buffer.Length);
          buffer.DecryptOverwrite(ReverseKey);
          bs.Seek(0 - length, SeekOrigin.Current);
          bs.Write(buffer, 0, length);
        }
      }
    }
    /// <summary>
    /// 使用该密钥解密指定的文件，解密后覆盖原文件。
    /// </summary>
    /// <param name="filepath">需要加密的文件的路径</param>
    public void DecryptFile(string filepath)
    {
      using (Stream stream = new FileStream(filepath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
      {
        using (BufferedStream bs = new BufferedStream(stream))
        {
          byte[] buffer = new byte[4096];
          int length = 1;
          bs.Seek(0, SeekOrigin.Begin);
          while (length > 0)
          {
            length = bs.Read(buffer, 0, buffer.Length);
            buffer.DecryptOverwrite(ReverseKey);
            bs.Seek(0 - length, SeekOrigin.Current);
            bs.Write(buffer, 0, length);
          }
        }
      }
    }
    /// <summary>
    /// 使用该密钥解密指定的文件。
    /// </summary>
    /// <param name="filepath">需要加密的文件的路径</param>
    public MemoryStream DecryptFileToStream(string filepath)
    {
      using (FileStream stream = new FileStream(filepath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
      {
        MemoryStream bs = new MemoryStream((int)stream.Length);
        byte[] buffer = new byte[4096];
        int length = 1;
        stream.Seek(0, SeekOrigin.Begin);
        bs.Seek(0, SeekOrigin.Begin);
        while (length > 0)
        {
          length = stream.Read(buffer, 0, buffer.Length);
          buffer.DecryptOverwrite(ReverseKey);
          bs.Write(buffer, 0, length);
        }
        return bs;
      }

    }
  }

  /// <summary>
  /// 加密密钥。此类不能被继承。
  /// </summary>
  [Serializable]
  public sealed class SecretKey
  {
    /// <summary>
    /// 密钥序列。即加解密的凭证。
    /// </summary>
    internal byte[] _SecretKeyID;
    /// <summary>
    /// 获取密钥的特征码。
    /// </summary>
    public string SecretKeyID => _SecretKeyID.MD5().ToStringNoSymbol();
    /// <summary>
    /// 密码特征
    /// </summary>
    private byte[] _Password;
    /// <summary>
    /// 使用新密码创建一个新密钥。请将密钥保存到文件或注册表，这将是解密的唯一凭证。如果您想要任何人都可以使用此密钥，请将密码指定为 <see langword="null"/> 。
    /// <para>保存到文件请使用 <see cref="SaveTo(string)"/> , 保存到注册表请使用 <see cref="SaveTo(string, string)"/> 。</para>
    /// </summary>
    /// <param name="password">密码序列</param>
    public SecretKey(byte[] password = null, bool isfixed = false)
    {
      password = password ?? new byte[0];
      _Password = password.MD5();
      if (isfixed)
      {
        List<byte> sk = new List<byte>();
        sk.AddRange(_Password);
        sk.AddRange(Encoding.UTF8.GetBytes("|Renlen.BaseLibrary.Secret|★"));
        sk.AddRange(Encoding.UTF8.GetBytes("|201906190207@China"));
        sk.Add(60);
        sk.Add(255);
        sk.Add(0);
        sk.Add(18);
        _SecretKeyID = sk.ToArray().MD5();
      }
      else
      {
        List<byte> sk = new List<byte>(14 * 4);
        sk.AddRange(BitConverter.GetBytes(Environment.TickCount));
        var now = DateTime.Now;
        sk.AddRange(BitConverter.GetBytes(now.Year));
        sk.AddRange(BitConverter.GetBytes(now.Month));
        sk.AddRange(BitConverter.GetBytes(now.Day));
        sk.AddRange(BitConverter.GetBytes(now.Hour));
        sk.AddRange(BitConverter.GetBytes(now.Minute));
        sk.AddRange(BitConverter.GetBytes(now.Second));
        sk.AddRange(BitConverter.GetBytes(now.Millisecond));
        sk.AddRange(_Password);
        sk.AddRange(BitConverter.GetBytes(NextDouble()));
        _SecretKeyID = sk.ToArray().MD5();
      }
    }
    /// <summary>
    /// 使用新密码创建一个新密钥。请将密钥保存到文件或注册表，这将是解密的唯一凭证。如果您想要任何人都可以使用此密钥，请将密码指定为 <see langword="null"/> 。
    /// <para>保存到文件请使用 <see cref="SaveTo(string)"/> , 保存到注册表请使用 <see cref="SaveTo(string, string)"/> 。</para>
    /// </summary>
    /// <param name="password">密码字符串</param>
    /// <param name="encoding">将字符串转换为字节序列时使用的编码。如果您不知道什么是编码，推荐您选择 <see cref="Encoding.UTF8"/> 。注意，不同的字符串或不同的编码可以会有同样的字节序列。</param>
    public SecretKey(string password, Encoding encoding, bool isfixed = false)
      : this(encoding.GetBytes(password), isfixed)
    {

    }

    /// <summary>
    /// 使用指定密码从文件加载一个密钥。
    /// </summary>
    /// <param name="path">文件所在路径</param>
    /// <param name="password">打开文件使用的密码</param>
    /// <param name="istrynull">指示使用指定密码打开文件失败时，是否用空密码尝试打开</param>
    /// <returns></returns>
    public static SecretKey ReadSecretKey(string path, byte[] password = null, bool istrynull = false)
    {
      var oldpassword = password ?? new byte[0];
      IFormatter formattable = new BinaryFormatter();
      var stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
      byte[] data = new byte[stream.Length];
      stream.Seek(0, SeekOrigin.Begin);
      stream.Read(data, 0, data.Length);
      var soudata = data.Decrypt(oldpassword.MD5().GetNewSoftArray().GetReverseKey());
      stream.Seek(0, SeekOrigin.Begin);
      stream.Write(soudata, 0, soudata.Length);
      stream.Seek(0, SeekOrigin.Begin);
      SecretKey sk;
      try
      {
        sk = (SecretKey)formattable.Deserialize(stream);
      }
      catch
      {
        stream.Seek(0, SeekOrigin.Begin);
        stream.Write(data, 0, data.Length);
        try { stream.Close(); } catch { }
        if (istrynull && oldpassword.Length != 0) return ReadSecretKey(path);
        throw new ArgumentException("密码不正确。");
      }
      stream.Seek(0, SeekOrigin.Begin);
      stream.Write(data, 0, data.Length);
      try { stream.Close(); } catch { }
      return sk;
    }
    /// <summary>
    /// 使用指定密码从注册表加载一个密钥。
    /// </summary>
    /// <param name="registrykey">注册表项，包含其路径</param>
    /// <param name="valuename">注册表值的名称</param>
    /// <param name="password">打开文件使用的密码</param>
    /// <param name="istrynull">指示使用指定密码打开文件失败时，是否用空密码尝试打开</param>
    /// <returns></returns>
    public static SecretKey ReadSecretKey(string registrykey, string valuename, byte[] password = null, bool istrynull = false)
    {
      var oldpassword = password ?? new byte[0];
      IFormatter formattable = new BinaryFormatter();
      byte[] data = Registry.GetValue(registrykey, valuename, null) as byte[] ?? throw new ArgumentException("指定的注册表项或值不存在。");
      string path = Path.GetTempFileName();
      var stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
      data = data.Decrypt(oldpassword.MD5().GetNewSoftArray().GetReverseKey());
      stream.Seek(0, SeekOrigin.Begin);
      stream.Write(data, 0, data.Length);
      stream.Seek(0, SeekOrigin.Begin);
      SecretKey sk;
      try
      {
        sk = (SecretKey)formattable.Deserialize(stream);
      }
      catch
      {
        try { stream.Close(); } catch { }
        try { File.Delete(path); } catch { }
        if (istrynull && oldpassword.Length != 0) return ReadSecretKey(registrykey, valuename);
        throw new ArgumentException("密码不正确。");
      }
      try { stream.Close(); } catch { }
      try { File.Delete(path); } catch { }
      return sk;
    }


    /// <summary>
    /// 将此密钥保存到文件。如果文件已存在将被覆盖。
    /// </summary>
    /// <param name="path"></param> 
    public void SaveTo(string path)
    {
      IFormatter formattable = new BinaryFormatter();
      var stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
      formattable.Serialize(stream, this);
      byte[] data = new byte[stream.Length];
      stream.Seek(0, SeekOrigin.Begin);
      stream.Read(data, 0, data.Length);
      data = data.Encryption(_Password.GetNewSoftArray());
      stream.Seek(0, SeekOrigin.Begin);
      stream.Write(data, 0, data.Length);
      try { stream.Close(); } catch { }
    }
    /// <summary>
    /// 将此密钥保存到注册表。如果注册表值已存在将被覆盖。使用此方法时要保证有足够的权限，否则将引发 <see cref="UnauthorizedAccessException"/> 异常。
    /// </summary>
    /// <param name="registrykey">注册表项，包含其路径</param>
    /// <param name="valuename">注册表值</param>
    public void SaveTo(string registrykey, string valuename)
    {
      string path = Path.GetTempFileName();
      IFormatter formattable = new BinaryFormatter();
      var stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
      formattable.Serialize(stream, this);
      byte[] data = new byte[stream.Length];
      stream.Seek(0, SeekOrigin.Begin);
      stream.Read(data, 0, data.Length);
      data = data.Encryption(_Password.GetNewSoftArray());
      try { stream.Close(); } catch { }
      try { File.Delete(path); } catch { }
      Registry.SetValue(registrykey, valuename, data, RegistryValueKind.Binary);
    }

    /// <summary>
    /// 为此密钥设置新密码。设置新密码后注意使用 <see cref="SaveTo(string)"/> 更新文件。
    /// </summary>
    /// <param name="oldpassword">原密码</param>
    /// <param name="newpassword">新密码</param>
    public void SetNewPassword(byte[] oldpassword, byte[] newpassword)
    {
      var password = oldpassword ?? new byte[0];
      if (password.MD5().ToStringNoSymbol() == _Password.ToStringNoSymbol())
      {
        _Password = newpassword.MD5();
      }
      else
      {
        throw new ArgumentException("原密码不正确。");
      }
    }

    /// <summary>
    /// 判断两个密钥的加密过程是否一样。如果都为空，则返回 <see langword="true"/> 。
    /// </summary>
    /// <param name="sk1">要判断的密钥</param>
    /// <param name="sk2">用来比较的密钥</param>
    /// <returns></returns>
    public static bool operator ==(SecretKey sk1, SecretKey sk2)
    {
      if ((object)sk1 == null && (object)sk2 == null) return true;
      if ((object)sk1 == null || (object)sk2 == null) return false;
      return sk1.SecretKeyID == sk2.SecretKeyID;
    }
    /// <summary>
    /// 判断两个密钥的加密过程是否不一样。如果都为空，则返回 <see langword="false"/> 。
    /// </summary>
    /// <param name="sk1"></param>
    /// <param name="sk2"></param>
    /// <returns></returns>
    public static bool operator !=(SecretKey sk1, SecretKey sk2)
    {
      if ((object)sk1 == null && (object)sk2 == null) return false;
      if ((object)sk1 == null || (object)sk2 == null) return true;
      return sk1.SecretKeyID != sk2.SecretKeyID;
    }
    /// <summary>
    /// 确定指定的对象的加密过程是否与当前对象一样。不推荐使用，请使用 <see langword="=="/> 运算符和 <see langword="!="/> 运算符。
    /// </summary>
    /// <param name="obj">要与当前对象进行比较的对象</param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
      SecretKey sk2 = obj as SecretKey;
      return this == sk2;
    }
    /// <summary>
    /// 获取密钥特征码的哈希代码。
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
      return SecretKeyID.GetHashCode();
    }
  }

  /// <summary>
  /// 基础方法。其他地方也能用到的方法。
  /// </summary>
  internal static class Visbek
  {
    /// <summary>
    /// 计算指定 <see cref="Stream"/> 对象的哈希值。
    /// </summary>
    /// <param name="inputtream">要计算其哈希代码的输入。</param>
    /// <returns></returns>
    internal static byte[] MD5(this Stream inputtream)
    {
      return new MD5CryptoServiceProvider().ComputeHash(inputtream);
    }

    /// <summary>
    /// 计算指定字节数组的哈希值。
    /// </summary>
    /// <param name="buffer">要计算其哈希代码的输入。</param>
    /// <returns></returns>
    internal static byte[] MD5(this byte[] buffer)
    {
      return new MD5CryptoServiceProvider().ComputeHash(buffer);
    }

    /// <summary>
    /// 取出位于指定索引处的元素。
    /// </summary>
    /// <param name="list">元素列表</param>
    /// <param name="index">从零开始的索引</param>
    /// <returns></returns>
    internal static T GetAtIndexValue<T>(this List<T> list, int index)
    {
      T b = list[index];
      list.RemoveAt(index);
      return b;
    }

    /// <summary>
    /// 与操作数进行And(按位与)运算后比较是否与操作数相等。通常用于取位。
    /// </summary>
    /// <param name="data">数据</param>
    /// <param name="operationnumber">操作数</param>
    /// <returns></returns>
    internal static bool IsAndTrue(this int data, int operationnumber)
    {
      return (data & operationnumber) == operationnumber;
    }

    /// <summary>
    /// 将字符串的顺序随机打乱。
    /// </summary>
    /// <param name="data">要打乱的字符串</param>
    /// <returns></returns>
    internal static string OrderByRandom(this string data)
    {
      StringBuilder sb = new StringBuilder(data.Length);
      List<char> list = data.ToList();
      while (list.Count > 0)
      {
        sb.Append(list.GetAtIndexValue(Next(0, list.Count)));
      }
      return sb.ToString();
    }
  }

  /// <summary>
  /// 特有方法。为此程序集专门设计的方法。
  /// </summary>
  internal static class Special
  {
    /// <summary>
    /// 按照一个长度为十六的字节数组把字节序列打乱。
    /// </summary>
    /// <param name="arr"></param>
    /// <returns></returns>
    internal static byte[] GetNewSoftArray(this byte[] arr)
    {
      List<byte> souarr = new List<byte>(256);
      for (byte i = 0; i < 255; i++)
      {
        souarr.Add(i);
      }
      souarr.Add(255);
      List<byte> newarr = new List<byte>(256);
      var ls = arr.ToArray();
      for (int i = 0; i < 16; i++)
      {
        for (int j = 0; j < 16; j++)
        {
          while (souarr.Count <= ls[j])
          {
            if (ls[j] % 2 == 0)
              ls[j] /= 2;
            else
              ls[j] = Convert.ToByte((ls[j] - 1) / 2);
          }
          newarr.Add(souarr.GetAtIndexValue(ls[j]));
        }
      }
      return newarr.ToArray();
    }

    /// <summary>
    /// 按照指定字节序列加密数据。
    /// </summary>
    /// <param name="data">要加密的数据</param>
    /// <param name="key">字节序列</param>
    /// <returns></returns>
    internal static void EncryptionOverwrite(this byte[] data, byte[] key)
    {

      for (int i = 0; i < data.Length; i++)
        data[i] = key[data[i]];
    }

    /// <summary>
    /// 按照指定字节序列加密数据。
    /// </summary>
    /// <param name="data">要加密的数据</param>
    /// <param name="key">字节序列</param>
    /// <returns></returns>
    internal static byte[] Encryption(this byte[] data, byte[] key)
    {
      byte[] arr = new byte[data.Length];
      for (int i = 0; i < data.Length; i++)
        arr[i] = key[data[i]];
      return arr;
    }

    /// <summary>
    /// 按照指定反字节序列解密数据。
    /// </summary>
    /// <param name="data">要解密的数据</param>
    /// <param name="reversekey">反字节序列</param>
    /// <returns></returns>
    internal static void DecryptOverwrite(this byte[] data, byte[] reversekey)
    {
      for (int i = 0; i < data.Length; i++)
        data[i] = (reversekey[(data[i])]);
    }

    /// <summary>
    /// 按照指定反字节序列解密数据。
    /// </summary>
    /// <param name="data">要解密的数据</param>
    /// <param name="reversekey">反字节序列</param>
    /// <returns></returns>
    internal static byte[] Decrypt(this byte[] data, byte[] reversekey)
    {
      List<byte> arr = new List<byte>(data.Length);
      for (int i = 0; i < data.Length; i++)
        arr.Add(reversekey[data[i]]);
      return arr.ToArray();
    }

    /// <summary>
    /// 创建基于此字符的字节序列。
    /// </summary>
    /// <param name="data">字符</param>
    /// <returns></returns>
    internal static string GetOrderString(this char data)
    {
      string order = "";
      int speed = new int[] { 3, 5, 7, 9, 11 }[(data - 64) % 5];
      for (int i = 1; i <= 26; i++)
      {
        order += (char)((i * speed - 1) % 26 + 65);
      }
      return order;
    }

    /// <summary>
    /// 获取与此字节序列相对应的偏移规则。
    /// </summary>
    /// <param name="data">字节序列</param>
    /// <returns></returns>
    internal static int[] GetMappingRule(this byte[] data)
    {
      int[] result = new int[3];
      result.Initialize();
      foreach (var i in data)
      {
        result[i % 3]++;
      }
      return result;
    }

    /// <summary>
    /// 按照指定的字节序列与偏移规则加密数据。
    /// </summary>
    /// <param name="data"></param>
    /// <param name="order"></param>
    /// <param name="deviation"></param>
    /// <returns></returns>
    internal static string EncryptionMapping(this byte[] data, string order, int[] deviation)
    {
      StringBuilder sb = new StringBuilder(data.Length * 16);
      var devdata = data.Select(d => (d + deviation[0]) * deviation[1] - deviation[2] + 16);
      var bit = new int[] { 0x1, 0x2, 0x4, 0x8, 0x10, 0x20, 0x40, 0x80, 0x100, 0x200, 0x400, 0x800, 0x1000 };//, 0x2000, 0x4000, 0x8000 };
                                                                                                             //              =2的   0    1    2    3    4     5     6      7     8      9     10      11     12      13      14      15   次方。
      string bytestr;
      foreach (var c in devdata)
      {
        bytestr = "";
        for (int i = 0; i < bit.Length; i++)
        {
          if (c.IsAndTrue(bit[i])) bytestr += order[i];
        }
        sb.Append(bytestr.PadToFull(order).OrderByRandom());
      }
      return sb.ToString();
    }

    /// <summary>
    /// 按照指定的字节序列与偏移规则解密数据。
    /// </summary>
    /// <param name="data"></param>
    /// <param name="order"></param>
    /// <param name="deviation"></param>
    /// <returns></returns>
    internal static byte[] DecryptMapping(this string data, string order, int[] deviation)
    {
      List<byte> list = new List<byte>(data.Length / 16);
      if (data.Length % 16 != 0) throw new FormatException("需要解密的字符串格式不正确。");
      data = data.ToUpper();
      var deorder = order.GetReverseMappingRule();
      var bit = new int[] { 0x1, 0x2, 0x4, 0x8, 0x10, 0x20, 0x40, 0x80, 0x100, 0x200, 0x400, 0x800, 0x1000 };//, 0x2000, 0x4000, 0x8000 };
                                                                                                             //              =2的   0    1    2    3    4     5     6      7     8      9     10      11     12      13      14      15   次方。
      string c;
      int ascii;
      for (int i = 0; i < data.Length / 16; i++)
      {
        c = data.Substring(i * 16, 16);
        ascii = 0;
        for (int j = 0; j < 16; j++)
        {
          try { ascii |= bit[deorder[c[j] - 65]]; } catch { }
        }
        ascii = (ascii - 16 + deviation[2]) / deviation[1] - deviation[0];
        list.Add(Convert.ToByte(ascii));
      }
      return list.ToArray();
    }

    /// <summary>
    /// 按照指定字节序列将字符串填满。
    /// </summary>
    /// <param name="data">要填满的字符串</param>
    /// <param name="order">字节序列</param>
    /// <returns></returns>
    internal static string PadToFull(this string data, string order)
    {
      while (data.Length < 16)
      {
        data += order[Next(13, 26)];
      }
      return data;
    }

    /// <summary>
    /// 获取相对于此字节序列的反字节序列。
    /// </summary>
    /// <param name="order">此字节序列</param>
    /// <returns></returns>
    internal static int[] GetReverseMappingRule(this string order)
    {
      int[] result = new int[26];
      for (int i = 0; i < 26; i++)
      {
        result[order[i] - 65] = i;
      }
      return result;
    }
    /// <summary>
    /// 获取相对于此字节序列的反字节序列。
    /// </summary>
    /// <param name="key">此字节序列</param>
    /// <returns></returns>
    internal static byte[] GetReverseKey(this byte[] key)
    {
      byte[] result = new byte[256];
      for (int i = 0; i < 256; i++)
      {
        result[key[i]] = (byte)i;
      }
      return result;
    }
  }

  /// <summary>
  /// 伪随机数类。
  /// </summary>
  internal static class RandomNumber
  {
    /// <summary>
    /// 随机数生成器。
    /// </summary>
    private static Random random = new Random();
    /// <summary>
    /// 返回一个非负随机整数。
    /// </summary>
    /// <returns></returns>
    internal static int Next()
    {
      return random.Next();
    }
    /// <summary>
    /// 返回一个小于所指定最大值的非负随机整数。
    /// </summary>
    /// <param name="maxvalue">要生成的随机数字上限包含上限，必须大于或等于零</param>
    /// <returns></returns>
    internal static int Next(int maxvalue)
    {
      return random.Next(maxvalue);
    }
    /// <summary>
    /// 返回在指定范围内的任意整数。
    /// </summary>
    /// <param name="minvalue">返回的随机数下限包含下限</param>
    /// <param name="maxvalue">返回的随机数上限不包含上限，必须大于或等于下限</param>
    /// <returns></returns>
    internal static int Next(int minvalue, int maxvalue)
    {
      return random.Next(minvalue, maxvalue);
    }
    /// <summary>
    /// 返回一个大于或等于0.0且小于1.0的随机浮点数。
    /// </summary>
    /// <returns></returns>
    internal static double NextDouble()
    {
      return random.NextDouble();
    }
  }

  /// <summary>
  /// 提供 <see langword="byte"/>[] 的扩展方法。
  /// </summary>
  public static class _Extend
  {
    /// <summary>
    /// 将字节数组转换为没有分隔符的十六进制字符串。
    /// </summary>
    /// <param name="array">要转换的字节数组</param>
    /// <returns></returns>
    public static string ToStringNoSymbol(this byte[] array)
    {
      return BitConverter.ToString(array).Replace("-", "");
    }
  }
}