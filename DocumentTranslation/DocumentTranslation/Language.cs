using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Renlen.BaseLibrary
{
  /*
   * Renlen
   */
  /// <summary>
  /// 语言数据文件。用于需要设置多语言的软件。
  /// </summary>
  public sealed class LanguageData : IEnumerable<KeyValuePair<LKey, string>>
  {
    /// <summary>
    /// 创建一个新语言数据文件。
    /// </summary>
    /// <returns></returns>
    public static LanguageData Create()
    {
      return new LanguageData();
    }
    /// <summary>
    /// 私有成员。存储语言设定。
    /// </summary>
    private readonly Dictionary<LKey, string> Data;
    /// <summary>
    /// 私有成员。存储文件头信息。
    /// </summary>
    private readonly Dictionary<string, string> head;
    /// <summary>
    /// 此对象是否可以更改。其值为字符串类型的 <see langword="true"/> 或者 <see langword="false"/> 。
    /// </summary>
    public string IsReadOnly { get; private set; } = "false";
    /// <summary>
    /// 获取读取时或最后一次保存的文件类型。如果是新创建的对象，其值为空字符串。
    /// </summary>
    public string FileType { get; private set; } = "";
    /// <summary>
    /// 获取读取时或最后一次保存的文件路径。如果是新创建的对象，其值为空字符串。
    /// </summary>
    public string Path { get; private set; } = "";
    /// <summary>
    /// 此对象的名称。
    /// </summary>
    public string Name { get; set; } = "";
    /// <summary>
    /// 文件类型。即语言类型。
    /// </summary>
    public string Type { get; set; } = "";
    /// <summary>
    /// 此文件对应的软件。建议格式 软件程序集名称@软件作者 。
    /// </summary>
    public string UseSoftware { get; set; } = "";
    /// <summary>
    /// 文件作者。
    /// </summary>
    public string Author { get; set; } = "";
    /// <summary>
    /// 文件注释。
    /// </summary>
    public string Caption { get; set; } = "";
    /// <summary>
    /// 根据属性名称设置或获取值。
    /// </summary>
    /// <param name="propertyName">属性名称</param>
    /// <returns></returns>
    public object this[string propertyName]
    {
      get
      {
        return GetType().GetProperty(propertyName)?.GetValue(this) ?? "";
      }
      set
      {
        GetType().GetProperty(propertyName)?.SetValue(this, value ?? "");
      }
    }
    /// <summary>
    /// 设置或获取指定数据。
    /// </summary>
    /// <param name="name">节点名称</param>
    /// <param name="key">键值</param>
    /// <param name="defaultvalue">获取失败时的默认值</param>
    /// <returns></returns>
    public string this[string name, string key, string defaultvalue = ""]
    {
      get
      {
        LKey lkey = new LKey(name, key);
        return this[lkey, defaultvalue];
      }
      set
      {
        if (IsReadOnly == "true") return;
        LKey lkey = new LKey(name, key);
        this[lkey] = value ?? "";
      }
    }
    /// <summary>
    /// 设置或获取指定数据。
    /// </summary>
    /// <param name="key">键信息</param>
    /// <param name="defaultvalue">获取失败时的默认值</param>
    /// <returns></returns>
    public string this[LKey key, string defaultvalue = ""]
    {
      get
      {
        bool b = Data.TryGetValue(key, out string value);
        if (b) return value;
        if (IsReadOnly != "true")
        {
          Data.Add(key, defaultvalue);
        }
        return defaultvalue;
      }
      set
      {
        if (Data.ContainsKey(key))
        {
          Data[key] = value ?? "";
        }
        else
        {
          Data.Add(key, value ?? "");
        }
      }
    }

    /// <summary>
    /// 初始化 <see cref="LanguageData"/> 类的新实例。
    /// </summary>
    public LanguageData()
    {

      Data = new Dictionary<LKey, string>();

      head = new Dictionary<string, string>();

      SetHeadInfo("FileVersion", "1.0.0");
    }
    /// <summary>
    /// 设置版本号。保存在头信息，其键值为 version
    /// </summary>
    /// <param name="version"></param>
    public void SetVersion(string version)
    {
      SetHeadInfo("version", version);
    }
    /// <summary>
    /// 设置头信息
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetHeadInfo(string key, string value)
    {
      if (head.ContainsKey(key))
        head[key] = value;
      else
        head.Add(key, value);
    }
    /// <summary>
    /// 获取头信息
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetHeadInfo(string key)
    {
      return head.TryGetValue(key, out string value) ? value : null;
    }
    /// <summary>
    /// 获取头信息集合的只读封装。用于遍历头信息。
    /// </summary>
    /// <returns></returns>
    public ReadOnlyDictionary<string, string> GetHeadInfoReadOnly()
    {
      return new ReadOnlyDictionary<string, string>(head);
    }
    /// <summary>
    /// 获取枚举器。它仅遍历设置的语言信息，不遍历属性和头信息。
    /// </summary>
    /// <returns></returns>
    public IEnumerator<KeyValuePair<LKey, string>> GetEnumerator()
    {
      return Data.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    /// <summary>
    /// 添加一系列枚举类型，其初始值为空字符串
    /// </summary>
    /// <param name="enumType"></param>
    public void AddEnumOfNull(params Type[] enumType)
    {
      string prefix = "Enum:";
      foreach (Type e in enumType)
      {
        foreach (string name in Enum.GetNames(e))
        {
          this[$"{prefix}{e.ToString()}", name] = "";
        }
      }
    }
    /// <summary>
    /// 添加一系列枚举类型，其初始值为枚举名称
    /// </summary>
    /// <param name="enumType"></param>
    public void AddEnumOfName(params Type[] enumType)
    {
      string prefix = "Enum:";
      string t;
      foreach (Type type in enumType)
      {
        t = type.ToString();
        foreach (string name in Enum.GetNames(type))
        {
          this[$"{prefix}{t}", name] = name;
        }
      }
    }
    /// <summary>
    /// 添加一系列枚举类型，其初始值自定义
    /// </summary>
    /// <param name="FuncNameToValue"></param>
    /// <param name="enumType"></param>
    public void AddEnum(Func<string, string> FuncNameToValue, params Type[] enumType)
    {
      if (FuncNameToValue == null) FuncNameToValue = d => d;
      string prefix = "Enum:";
      foreach (Type type in enumType)
      {
        foreach (string name in Enum.GetNames(type))
        {
          this[$"{prefix}{type.ToString()}", name] = FuncNameToValue(name);
        }
      }
    }
    /// <summary>
    /// 添加一系列枚举类型，其初始值自定义
    /// </summary>
    /// <param name="FuncNameToValue"></param>
    /// <param name="enumType"></param>
    public void AddEnum(Func<string, string> FuncNameToValue, IEnumerable<Type> enumType)
    {
      if (FuncNameToValue == null) FuncNameToValue = d => d;
      string prefix = "Enum:";
      foreach (Type type in enumType)
      {
        foreach (string name in Enum.GetNames(type))
        {
          this[$"{prefix}{type.ToString()}", name] = FuncNameToValue(name);
        }
      }
    }
    /// <summary>
    /// 向指定结点添加 Windows 控件的集合，默认初始值为控件的 Text 属性
    /// </summary>
    /// <param name="name">结点名称</param>
    /// <param name="controls"></param>
    /// <param name="FuncToValue"></param>
    public void AddControl(string name, IEnumerable<Control> controls, Func<Control, string> FuncToValue = null)
    {
      if (FuncToValue == null) FuncToValue = c => c.Text;
      string prefix = "Controls:";
      foreach (Control c in controls)
      {
        this[$"{prefix}{name}", c.Name] = FuncToValue(c);
      }
    }
    /// <summary>
    /// 添加 Windows 控件的集合，默认结点名为控件顶级控件(通常是窗体)的名称，默认键值为控件的名称，默认初始值为控件的 Text 属性
    /// </summary>
    /// <param name="controls"></param>
    /// <param name="FuncToName"></param>
    /// <param name="FuncToKey"></param>
    /// <param name="FuncToValue"></param>
    public void AddControlOfFunc(IEnumerable<Control> controls, Func<Control, string> FuncToName = null, Func<Control, string> FuncToKey = null, Func<Control, string> FuncToValue = null)
    {
      if (FuncToName == null) FuncToName = c => c.TopLevelControl.Name;
      if (FuncToKey == null) FuncToKey = c => c.Name;
      if (FuncToValue == null) FuncToValue = c => c.Text;
      string prefix = "Controls:";
      foreach (Control c in controls)
      {
        this[$"{prefix}{FuncToName(c)}", FuncToKey(c)] = FuncToValue(c);
      }
    }
    /// <summary>
    /// 向指定结点添加 Windows 控件的集合，初始值为控件的 Text 属性
    /// </summary>
    /// <param name="name"></param>
    /// <param name="controls"></param>
    public void AddControl(string name, params Control[] controls)
    {
      string prefix = "Controls:";
      foreach (Control c in controls)
      {
        this[$"{prefix}{name}", c.Name] = c.Text;
      }
    }
    /// <summary>
    /// 向指定结点添加 Windows 控件的集合，结点名为控件顶级控件(通常是窗体)的名称，键值为控件的名称，初始值为控件的 Text 属性
    /// </summary>
    /// <param name="controls"></param>
    public void AddControl(params Control[] controls)
    {
      string prefix = "Controls:";
      foreach (Control c in controls)
      {
        this[$"{prefix}{c.TopLevelControl.Name}", c.Name] = c.Text;
      }
    }
    /// <summary>
    /// 向指定结点添加 Windows 控件的集合，默认结点名为控件顶级控件(通常是窗体)的名称，默认键值为控件的名称，默认初始值为控件的 Text 属性
    /// </summary>
    /// <param name="FuncToName"></param>
    /// <param name="FuncToKey"></param>
    /// <param name="FuncToValue"></param>
    /// <param name="controls"></param>
    public void AddControlOfFunc(Func<Control, string> FuncToName, Func<Control, string> FuncToKey, Func<Control, string> FuncToValue, params Control[] controls)
    {
      if (FuncToName == null) FuncToName = c => c.TopLevelControl.Name;
      if (FuncToKey == null) FuncToKey = c => c.Name;
      if (FuncToValue == null) FuncToValue = c => c.Text;
      string prefix = "Controls:";
      foreach (Control c in controls)
      {
        this[$"{prefix}{FuncToName(c)}", FuncToKey(c)] = FuncToValue(c);
      }
    }
    /// <summary>
    /// 添加指定 Windows 窗体。并设置需要设置语言信息的控件类型。
    /// </summary>
    /// <param name="form">窗体</param>
    /// <param name="ControlType">类型列表</param>
    public void AddForm(Form form, params Type[] ControlType)
    {
      string prefix = "Form:";
      this[$"{prefix}{form.Name}", form.Name] = form.Text;
      AddSubControl(form, IsAdd);

      bool IsAdd(Control control)
      {
        foreach (Type type in ControlType)
        {
          if (type == control.GetType())
          {
            return true;
          }
        }
        return false;
      }
      void AddSubControl(Control control, Func<Control, bool> func)
      {
        foreach (Control item in control.Controls)
        {
          if (func(item))
          {
            this[$"{prefix}{control.TopLevelControl.Name}", item.Name] = item.Text;
            //AddControlOfFunc(null, null, null, item);
          }
          AddSubControl(item, func);
        }
      }
    }
    /// <summary>
    /// 加载指定 Windows 窗体。并设置需要加载语言信息的控件类型。
    /// </summary>
    /// <param name="form">窗体</param>
    /// <param name="ControlType">类型列表</param>
    public void LoadForm(Form form, params Type[] ControlType)
    {
      string prefix = "Form:";
      form.Text = this[$"{prefix}{form.Name}", form.Name, defaultvalue: form.Text ?? ""];
      LoadSubControl(form, IsAdd);

      bool IsAdd(Control control)
      {
        foreach (Type type in ControlType)
        {
          if (type == control.GetType())
          {
            return true;
          }
        }
        return false;
      }
      void LoadSubControl(Control control, Func<Control, bool> func)
      {
        foreach (Control item in control.Controls)
        {
          if (func(item))
          {
            item.Text = this[$"{prefix}{control.TopLevelControl.Name}", item.Name,  item.Text ?? ""];
            //AddControlOfFunc(null, null, null, item);
          }
          LoadSubControl(item, func);
        }
      }
    }

    /// <summary>
    /// 清空语言信息集合。
    /// </summary>
    public void Clear()
    {
      if (IsReadOnly == "true") return;
      Data.Clear();
    }
    /// <summary>
    /// 清空头信息集合。
    /// </summary>
    public void ClearHead()
    {
      if (IsReadOnly == "true") return;
      head.Clear();
    }
    /// <summary>
    /// 移除指定键。
    /// </summary>
    /// <param name="name">节点名称</param>
    /// <param name="key">键</param>
    public void RemoveKey(string name, string key)
    {
      if (IsReadOnly == "true") return;
      LKey lkey = new LKey(name, key);
      if (Data.ContainsKey(lkey))
      {
        Data.Remove(lkey);
      }
    }
    /// <summary>
    /// 锁定对象。将此对象设为只读。
    /// </summary>
    public void Lock()
    {
      IsReadOnly = "true";
    }
    /// <summary>
    /// 保存文件到指定位置。
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <param name="type">保存文件类型</param>
    public void SaveFile(string path, LanguageFileType type = LanguageFileType.RLanguageDataFile)
    {
      switch (type)
      {
        case LanguageFileType.Auto:
        case LanguageFileType.RLanguageDataFile:
          SaveFileRLDF(path);
          break;
        case LanguageFileType.InitializationFile:
          SaveFileIni(path);
          break;
        default:
          SaveFileRLDF(path);
          break;
      }
    }
    /// <summary>
    /// 保存文件到指定位置。以INI格式保存。
    /// </summary>
    /// <param name="path">文件路径</param>
    public void SaveFileIni(string path)
    {
      FileType = "INI";
      Path = "";
      using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
      {
        sw.WriteLine(";I");
        sw.WriteLine(";请尽量不要修改:SYSTEM:节的内容，并将其放置在第一个节点(否则读取文件列表时无法读取文件基本属性信息，如Name、Type等)；");
        sw.WriteLine(";如果您必须定义一些自己的字段，请将其放置在;[Head]的后方以key1=value1&key2=value2&...的形式");
        sw.Write(";[Head]");
        foreach (var item in head)
        {
          sw.Write(item.Key);
          sw.Write('=');
          sw.Write(item.Value?.Escape());
          sw.Write('&');
        }
        sw.WriteLine();
        sw.WriteLine();
        sw.WriteLine("[:SYSTEM:]");
        foreach (var item in GetType().GetProperties())
        {
          if (item.Name == "Item") continue;
          sw.Write(item.Name);
          sw.Write('=');
          sw.WriteLine(item.GetValue(this).ToString().Escape());
        }
        //sw.WriteLine();
        var groups = from d in Data
                     group d
                     by d.Key.Name;
        foreach (var group in groups)
        {
          if (group.Count() < 0) continue;
          sw.WriteLine();
          sw.Write("[");
          sw.Write(group.Key);
          sw.WriteLine("]");
          foreach (var item in group)
          {
            sw.Write(item.Key.Key);
            sw.Write('=');
            //sw.Write(item.Key.MaxLength);
            //sw.Write('=');
            sw.WriteLine(item.Value?.Escape());
          }
        }
        sw.WriteLine();
      }
      Path = path;
    }
    /// <summary>
    /// 保存文件到指定位置。以RLDF格式保存。
    /// </summary>
    /// <param name="path">文件路径</param>
    public void SaveFileRLDF(string path)
    {
      FileType = "RLDF";
      Path = "";
      using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
      using (BinaryWriter bw = new BinaryWriter(fs, Encoding.UTF8))
      {
        bw.Write("RLDF".ToCharArray());
        if (IsReadOnly == "true")
        {
          bw.Write("此文件会进行校验，请不要修改此文件，否则将导致文件损坏。");
        }
        else
        {
          bw.Write("此文件不会进行校验，您可以随意修改此文件，只要您有那个本事。");
        }
        foreach (var item in head)
        {
          bw.Write(item.Key);
          bw.Write(item.Value ?? "");
        }
        bw.Write("");
        foreach (var item in GetType().GetProperties())
        {
          if (item.Name == "Item") continue;
          bw.Write(item.Name);
          bw.Write(item.GetValue(this).ToString());
        }
        bw.Write("");
        var groups = from d in Data
                     group d
                     by d.Key.Name;
        foreach (var group in groups)
        {
          if (group.Count() < 0) continue;
          bw.Write(group.Key);
          bw.Write(group.Count());
          foreach (var item in group)
          {
            bw.Write(item.Key.Key);
            //bw.Write(item.Key.MaxLength);
            bw.Write(item.Value ?? "");
          }
        }
        bw.Write("");
        if (IsReadOnly != "true")
        {
          bw.Write(-1L);
          bw.Write("");
          bw.Write("End");
        }
      }
      if (IsReadOnly == "true")
      {
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
        {
          string md5 = $"{fs.GetMD5()}|Renlen.BaseLibrary.LanguageData|FileAuthor:{Author}|★".GetMD5();
          fs.Seek(0, SeekOrigin.End);
          using (BinaryWriter bw = new BinaryWriter(fs, Encoding.UTF8))
          {
            bw.Write(fs.Length);
            bw.Write(md5);
            bw.Write("End");
            //null=fa3e177d0ba1f73cf21361b16c9c544a
            //have=97b0f1731ab0d46deba114e4d6249156
          }
        }
      }
      Path = path;
    }
    /// <summary>
    /// 从指定文件加载语言数据。加载失败返回 <see langword="null"/> 。
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <param name="fileType">文件类型</param>
    /// <returns></returns>
    public static LanguageData LoadFile(string path, LanguageFileType fileType = LanguageFileType.Auto)
    {

      switch (fileType)
      {
        case LanguageFileType.Auto:
          return (TryLoadFileRLDF(path, out var r, out _) ? r : null) ?? LoadFileIni(path);
        case LanguageFileType.RLanguageDataFile:
          return TryLoadFileRLDF(path, out r, out _) ? r : null;
        case LanguageFileType.InitializationFile:
          return LoadFileIni(path);
        default:
          break;
      }
      return null;
    }
    /// <summary>
    /// 加载INI文件。加载失败返回 <see langword="null"/> 。
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <returns></returns>
    public static LanguageData LoadFileIni(string path)
    {
      if (!File.Exists(path)) return null;
      using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
      {
        if (!sr.EndOfStream)
        {
          string check = sr.ReadLine();
          if (check.Length < 2 || check[0] != ';' || check[1] != 'I')
            return null;
        }
        string line, node = null;
        string key, maxLength, value;
        int index;
        Regex regexComment = new Regex(@"^[\s]*;");
        Regex regexHead = new Regex(@"^;[\s]*\[Head]", RegexOptions.IgnoreCase);
        Regex regexNode = new Regex(@"^\[.+]$");
        Regex regexKey = new Regex(@"^[^[=].*=.*$");
        Regex regexLength = new Regex(@"^[\d]+=[^\r\n]*$");
        Regex regexChar = new Regex(@"[\s-[\t ]]");
        LanguageData ld = new LanguageData();
        ld.Path = path;
        ld.FileType = "INI";
        string isReadOnly = "false";
        bool headflag = true;
        while (!sr.EndOfStream)
        {
          line = regexChar.Replace(sr.ReadLine(), "");
          if (headflag)
          {
            while (true)
            {
              if (line.Length == 0) continue;
              if (line[0] != ';') break;
              if (regexHead.IsMatch(line))
              {
                index = line.IndexOf(']');
                line = line.Substring(index + 1);
                foreach (var item in line.Split(new char[] { '&' }
                , StringSplitOptions.RemoveEmptyEntries))
                {
                  index = item.IndexOf('=');
                  if (index > 0)
                  {
                    ld.SetHeadInfo(item.Substring(0, index), item.Substring(index + 1).Unescape());
                  }
                }
                if (sr.EndOfStream) return ld;
                else line = regexChar.Replace(sr.ReadLine(), "");
                break;
              }
              if (sr.EndOfStream) return ld;
              else line = regexChar.Replace(sr.ReadLine(), "");
            }
            headflag = false;
          }
          if (line.Length == 0 || regexComment.IsMatch(line))
          {
            continue;
          }
          else if (regexNode.IsMatch(line))
          {
            node = line.Substring(1, line.Length - 2);
            if (node == ":SYSTEM:")
            {
              while (!sr.EndOfStream)
              {
                line = regexChar.Replace(sr.ReadLine(), "");
                if (regexComment.IsMatch(line))
                {
                  continue;
                }
                else if (regexNode.IsMatch(line))
                {
                  node = line.Substring(1, line.Length - 2);
                  break;
                }
                else if (regexKey.IsMatch(line))
                {
                  index = line.IndexOf('=');
                  key = line.Substring(0, index);
                  value = line.Substring(index + 1).Unescape();
                  if (key == "IsReadOnly")
                    isReadOnly = value;
                  else
                    ld[key] = value;
                }
              }
              ld.FileType = "INI";
            }
            //continue;
          }
          else if (regexKey.IsMatch(line))
          {
            if (node != null)
            {
              index = line.IndexOf('=');
              key = line.Substring(0, index);
              line = line.Substring(index + 1);
              if (regexLength.IsMatch(line))
              {
                index = line.IndexOf('=');
                maxLength = line.Substring(0, index);
                value = line.Substring(index + 1).Unescape();
              }
              else
              {
                maxLength = "0";
                value = line.Unescape();
              }
              ld[node, key] = value;
            }
          }
        }
        ld.IsReadOnly = isReadOnly;
        return ld;
      }
    }
    /// <summary>
    /// 加载RLDF文件。加载失败返回 <see langword="null"/> 。
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <returns></returns>
    public static LanguageData LoadFileRLDF(string path)
    {
      if (!File.Exists(path)) return null;
      int intValue;
      string stringValue;
      using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
      {
        LanguageData ld = new LanguageData();
        ld.Path = path;
        long length;
        string md5;
        string isReadOnly;
        try
        {
          using (BinaryReader br = new BinaryReader(fs, Encoding.UTF8, true))
          {
            stringValue = new string(br.ReadChars(4));
            if (stringValue != "RLDF")
            {
              return null;
            }
            br.ReadString();
            while (true)
            {
              stringValue = br.ReadString();
              if (stringValue == "") break;
              string value = br.ReadString();
              ld.SetHeadInfo(stringValue, value);
            }
            while (true)
            {
              stringValue = br.ReadString();
              if (stringValue == "") break;
              string value = br.ReadString();
              if (stringValue == "IsReadOnly")
                isReadOnly = stringValue;
              else
                ld[stringValue] = value;
            }
            ld.FileType = "RLDF";
            string node;
            string key;
            int maxLength;
            while (true)
            {
              node = br.ReadString();
              if (node == "") break;
              intValue = br.ReadInt32();
              for (int i = 0; i < intValue; i++)
              {
                key = br.ReadString();
                if (key == "") break;
                maxLength = br.ReadInt32();
                stringValue = br.ReadString();
                ld[node, key] = stringValue;
              }
            }
            length = br.ReadInt64();
            md5 = br.ReadString();
          }
        }
        catch
        {
          throw new Exception("无法识别的文件格式。文件可能已损坏或者已锁定的文件被篡改。");
        }
        if (length > -1)
        {
          ld.Lock();
          fs.Seek(0, SeekOrigin.Begin);
          byte[] data = new byte[length];
          fs.Read(data, 0, (int)length);
          string checkmd5 = $"{data.GetMD5()}|Renlen.BaseLibrary.LanguageData|FileAuthor:{ld.Author}|★".GetMD5();
          if (md5 != checkmd5)
          {
            throw new Exception("无法识别的文件格式。文件可能已损坏或者已锁定的文件被篡改。");
          }
        }
        return ld;
      }
    }
    /// <summary>
    /// 尝试读取RLDF文件。
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <param name="languageData">返回结果</param>
    /// <param name="errormessage">错误消息</param>
    /// <returns></returns>
    public static bool TryLoadFileRLDF(string path, out LanguageData languageData, out string errormessage)
    {
      if (!File.Exists(path))
      {
        errormessage = "指定的文件不存在。";
        languageData = null;
        return false;
      }
      try
      {
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
        {
          int intValue;
          string stringValue;
          LanguageData ld = new LanguageData();
          ld.Path = path;
          long length;
          string md5;
          using (BinaryReader br = new BinaryReader(fs, Encoding.UTF8, true))
          {
            stringValue = new string(br.ReadChars(4));
            if (stringValue != "RLDF")
            {
              errormessage = "无法识别的文件格式。";
              languageData = null;
              return false;
            }
            br.ReadString();
            while (true)
            {
              stringValue = br.ReadString();
              if (stringValue == "") break;
              string value = br.ReadString();
              ld.SetHeadInfo(stringValue, value);
            }
            while (true)
            {
              stringValue = br.ReadString();
              if (stringValue == "") break;
              string value = br.ReadString();
              if (stringValue != "IsReadOnly")
                ld[stringValue] = value;
            }
            ld.FileType = "RLDF";
            string node;
            string key;
            //int maxLength;
            while (true)
            {
              node = br.ReadString();
              if (node == "") break;
              intValue = br.ReadInt32();
              for (int i = 0; i < intValue; i++)
              {
                key = br.ReadString();
                if (key == "") break;
                //maxLength = br.ReadInt32();
                stringValue = br.ReadString();
                ld[node, key] = stringValue;
              }
            }
            length = br.ReadInt64();
            md5 = br.ReadString();
          }
          if (length > -1)
          {
            ld.Lock();
            fs.Seek(0, SeekOrigin.Begin);
            byte[] data = new byte[length];
            fs.Read(data, 0, (int)length);
            string checkmd5 = $"{data.GetMD5()}|Renlen.BaseLibrary.LanguageData|FileAuthor:{ld.Author}|★".GetMD5();
            if (md5 != checkmd5)
            {
              errormessage = "无法识别的文件格式。文件可能已损坏或者已锁定的文件被篡改。";
              languageData = null;
              return false;
            }
          }
          errormessage = null;
          languageData = ld;
          return true;
        }
      }
      catch (EndOfStreamException)
      {
        errormessage = "无法识别的文件格式。文件可能已损坏或者已锁定的文件被篡改。";
        languageData = null;
        return false;
      }
      catch (IOException ioex)
      {
        errormessage = ioex.Message;
        languageData = null;
        return false;
      }
      catch (Exception ex)
      {
        errormessage = ex.Message;
        languageData = null;
        return false;
      }
    }

  }
  /// <summary>
  /// 语言数据文件的文件头信息。用于显示语言选择列表。与 <see cref="LanguageData"/> 差不多。
  /// </summary>
  public sealed class LanguageDataHead : IEnumerable<KeyValuePair<string, string>>, IEnumerable
  {
    private readonly Dictionary<string, string> head = new Dictionary<string, string>();
    private readonly Dictionary<string, string> system = new Dictionary<string, string>();
    public string Path { get; }
    /// <summary>
    /// 以字节为单位。
    /// </summary>
    public long Length { get; private set; }
    public string FileType { get; private set; }

    private LanguageDataHead(string path)
    {
      Path = path;
    }

    public void SetVersion(string version)
    {
      SetHeadInfo("version", version);
    }
    public void SetHeadInfo(string key, string value)
    {
      if (head.ContainsKey(key))
        head[key] = value;
      else
        head.Add(key, value);
    }
    public string GetHeadInfo(string key)
    {
      return head.TryGetValue(key, out string value) ? value : null;
    }
    public ReadOnlyDictionary<string, string> GetHeadInfoReadOnly()
    {
      return new ReadOnlyDictionary<string, string>(head);
    }
    public void SetSystemInfo(string key, string value)
    {
      if (system.ContainsKey(key))
        system[key] = value;
      else
        system.Add(key, value);
    }
    public string GetSystemInfo(string key)
    {
      return system.TryGetValue(key, out string value) ? value : "";
    }
    public ReadOnlyDictionary<string, string> GetSystemInfoReadOnly()
    {
      return new ReadOnlyDictionary<string, string>(system);
    }

    public LanguageData ToLanguageData()
    {
      switch (FileType)
      {
        case "RLDF":
          return LanguageData.TryLoadFileRLDF(Path, out var ld, out _) ? ld : null;
        case "INI":
          return LanguageData.LoadFileIni(Path);
        default:
          return LanguageData.LoadFile(Path);
      }
    }

    public static LanguageDataHead LoadFileHead(string path, LanguageFileType fileType = LanguageFileType.Auto)
    {
      switch (fileType)
      {
        case LanguageFileType.Auto:
          return LoadFileHeadRLDF(path) ?? LoadFileHeadIni(path);
        case LanguageFileType.RLanguageDataFile:
          return LoadFileHeadRLDF(path);
        case LanguageFileType.InitializationFile:
          return LoadFileHeadIni(path);
        default:
          break;
      }
      return null;
    }
    public static LanguageDataHead LoadFileHeadIni(string path)
    {
      using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
      {
        if (!sr.EndOfStream)
        {
          string check = sr.ReadLine();
          if (check.Length < 2 || check[0] != ';' || check[1] != 'I')
            return null;
        }
        string line, node = null;
        string key, value;
        int index;
        Regex regexComment = new Regex(@"^[\s]*;");
        Regex regexHead = new Regex(@"^;[\s]*\[Head]", RegexOptions.IgnoreCase);
        Regex regexNode = new Regex(@"^\[.+]$");
        Regex regexKey = new Regex(@"^[^[=].*=.*$");
        Regex regexLength = new Regex(@"^[\d]+=[^\r\n]*$");
        Regex regexChar = new Regex(@"[\s-[\t ]]");
        LanguageDataHead ldh = new LanguageDataHead(path)
        { Length = sr.BaseStream.Length, FileType = "INI" };
        bool headflag = true;
        while (!sr.EndOfStream)
        {
          line = regexChar.Replace(sr.ReadLine(), "");
          if (headflag)
          {
            while (true)
            {
              if (line.Length == 0) continue;
              if (line[0] != ';') break;
              if (regexHead.IsMatch(line))
              {
                index = line.IndexOf(']');
                line = line.Substring(index + 1);
                foreach (var item in line.Split(new char[] { '&' }
                , StringSplitOptions.RemoveEmptyEntries))
                {
                  index = item.IndexOf('=');
                  if (index > 0)
                  {
                    ldh.SetHeadInfo(item.Substring(0, index), item.Substring(index + 1).Unescape());
                  }
                }
                if (sr.EndOfStream) return ldh;
                else line = regexChar.Replace(sr.ReadLine(), "");
                break;
              }
              if (sr.EndOfStream) return ldh;
              else line = regexChar.Replace(sr.ReadLine(), "");
            }
            headflag = false;
          }
          if (line.Length == 0 || regexComment.IsMatch(line))
          {
            continue;
          }
          else if (regexNode.IsMatch(line))
          {
            node = line.Substring(1, line.Length - 2).Unescape();
            if (node == ":SYSTEM:")
            {
              while (!sr.EndOfStream)
              {
                line = regexChar.Replace(sr.ReadLine(), "");
                if (regexComment.IsMatch(line))
                {
                  continue;
                }
                else if (regexNode.IsMatch(line))
                {
                  return ldh;
                }
                else if (regexKey.IsMatch(line))
                {
                  index = line.IndexOf('=');
                  key = line.Substring(0, index);
                  value = line.Substring(index + 1).Unescape();
                  ldh.SetSystemInfo(key, value);
                }
              }
              ldh.FileType = "INI";
            }
            else
              break;
            //continue;
          }
        }
        return ldh;
      }
    }
    public static LanguageDataHead LoadFileHeadRLDF(string path)
    {
      string stringValue;
      using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
      {
        try
        {
          using (BinaryReader br = new BinaryReader(fs, Encoding.UTF8, true))
          {
            stringValue = new string(br.ReadChars(4));
            if (stringValue != "RLDF")
            {
              return null;
            }
            LanguageDataHead ldh = new LanguageDataHead(path)
            { Length = fs.Length, FileType = "RLDF" };
            while (true)
            {
              stringValue = br.ReadString();
              if (stringValue == "") break;
              string value = br.ReadString();
              ldh.SetHeadInfo(stringValue, value);
            }
            while (true)
            {
              stringValue = br.ReadString();
              if (stringValue == "") break;
              string value = br.ReadString();
              ldh.SetSystemInfo(stringValue, value);
            }
            ldh.FileType = "RLDF";
            return ldh;
          }
        }
        catch
        {
          throw new Exception("无法识别的文件格式。文件可能已损坏或者已锁定的文件被篡改。");
        }
      }
    }
    public static bool TryLoadFileHeadRLDF(string path, out LanguageDataHead languageData, out string errormessage)
    {
      string stringValue;
      try
      {
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
        using (BinaryReader br = new BinaryReader(fs, Encoding.UTF8, true))
        {
          stringValue = new string(br.ReadChars(4));
          if (stringValue != "RLDF")
          {
            errormessage = "无法识别的文件格式。";
            languageData = null;
            return false;
          }
          LanguageDataHead ldh = new LanguageDataHead(path)
          { Length = fs.Length, FileType = "RLDF" };
          while (true)
          {
            stringValue = br.ReadString();
            if (stringValue == "") break;
            string value = br.ReadString();
            ldh.SetHeadInfo(stringValue, value);
          }
          while (true)
          {
            stringValue = br.ReadString();
            if (stringValue == "") break;
            string value = br.ReadString();
            ldh.SetSystemInfo(stringValue, value);
          }
          errormessage = null;
          ldh.FileType = "RLDF";
          languageData = ldh;
          return true;
        }
      }
      catch (EndOfStreamException)
      {
        errormessage = "无法识别的文件格式。文件可能已损坏或者已锁定的文件被篡改。";
        languageData = null;
        return false;
      }
      catch (IOException ioex)
      {
        errormessage = ioex.Message;
        languageData = null;
        return false;
      }
      catch (Exception ex)
      {
        errormessage = ex.Message;
        languageData = null;
        return false;
      }
    }

    /// <summary>
    /// 此对象的字符串表现形式。
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      string value = GetSystemInfo("Name") ?? "";
      if (value == "")
      {
        value = GetSystemInfo("Type") ?? "";
        if (value != "") value = $"({value})";
        value = $"(未命名){value}";
      }
      return $"{value}({((Path[0] != '\\' && Path[0] != '/' && Path[1] != ':') ? "...\\" : "")}{Path})";
    }
    public string ToString(string noname)
    {
      string value = GetSystemInfo("Name") ?? "";
      if (value == "")
      {
        value = GetSystemInfo("Type") ?? "";
        if (value != "") value = $"({value})";
        value = $"{noname}{value}";
      }
      return $"{value}({((Path[0] != '\\' && Path[0] != '/' && Path[1] != ':') ? "...\\" : "")}{Path})";
    }
    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
      foreach (var item in head)
        yield return item;
      foreach (var item in system)
        yield return item;
      yield break;
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

  }

  [Serializable]
  public struct LKey : IComparable<LKey>, IComparable
  {
    public string Name { get; }
    public string Key { get; }
    public LKey(string name, string key)
    {
      Name = name;
      Key = key;
    }

    public int CompareTo(object obj)
    {
      return CompareTo((LKey)obj);
    }
    public int CompareTo(LKey other)
    {
      return this.Name == other.Name
        ? this.Key.CompareTo(other.Key)
        : this.Name.CompareTo(other.Name);
    }

    public override string ToString()
    {
      return $"Name={Name};Key={Key}";
    }
    public override bool Equals(object obj)
    {
      return obj is LKey key ? this == key : false;
    }
    public override int GetHashCode()
    {
      return Name.GetHashCode() ^ Key.GetHashCode();
    }
    public static bool operator ==(LKey lkey, LKey rkey)
    {
      return lkey.Name == rkey.Name && lkey.Key == rkey.Key;
    }
    public static bool operator !=(LKey lkey, LKey rkey)
    {
      return lkey.Name != rkey.Name || lkey.Key != rkey.Key;
    }
  }

  [Serializable]
  public enum LanguageFileType
  {
    Auto = 0,
    RLanguageDataFile = 1,
    InitializationFile = 2
  }

  public static class LanguageDataEX
  {
    public static string Escape(this string str)
    {
      return Regex.Escape(str);
    }
    public static string Unescape(this string str)
    {
      return Regex.Unescape(str);
    }

    /// <summary>
    ///  <see cref="GetLanguageValue(string, int)"/> 的简写。
    /// </summary>
    /// <param name="str"></param>
    /// <param name="maxLength"></param>
    /// <returns></returns>
    public static string GLV(this string str, int maxLength)
    {
      return str.GetLanguageValue(maxLength);
    }
    public static string GetLanguageValue(this string str, int maxLength)
    {
      if (maxLength == 0 || str.Length <= maxLength) return str;
      else return str.Substring(0, maxLength);
    }
  }
}
