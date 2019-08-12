using Renlen.BaseLibrary;
using Renlen.BaseLibrary.Secret;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DocumentTranslation.GlobalData;

namespace DocumentTranslation
{
  internal class GlobalData
  {
    public event LanguageDataChangeEventHandler LanguageDataChange;
    internal Form_Main Form_Main { get; set; }
    private LanguageData _Data;
    public LanguageData Data
    {
      get
      {
        if (_Data == null)
          Data = LoadLanguageDataDefault();
        return _Data;
      }
      set
      {
        _Data = value;
        LanguageDataChange?.Invoke(this, new EventArgs());
      }
    }
    public Translation Baidu { get; set; }
    public string ConfigFilePath { get; set; }
    public StreamWriter Log { get; set; }
    public static void Init()
    {
      //GData.LoadEnvironment();
      //GData.Form_Main = new Form_Main();
      //Directory.CreateDirectory(@"Log");
      //GData.Log = new StreamWriter(new FileStream($@"Log\log_{DateTime.Now.ToString("yyyyMMdd")}.log", FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read));
    }

    public Only.Environment Config { get; set; }

    private GlobalData()
    {
      ConfigFilePath = @"Config.bin";
      Directory.CreateDirectory(@"Data");
      Directory.CreateDirectory(@"User");
      Directory.CreateDirectory(@"Language");

      Config = Only.Environment.LoadConfigOrDefault(ConfigFilePath);
      if (Config.UseLanguage)
      {
        Data = LanguageData.LoadFile(Config.SelectData);
      }
      //if (Data == null)
      //{
      //  Data = LoadLanguageDataDefault();
      //}
      if (Config.User != null) Baidu = Config.User.GetTranslation();

      Form_Main = new Form_Main();
      Directory.CreateDirectory(@"Log");
      Log = new StreamWriter(new FileStream($@"Log\log_{DateTime.Now.ToString("yyyyMMdd")}.log", FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read));

    }

    public void LoadEnvironment()
    {
      Config = Only.Environment.LoadConfigOrDefault(ConfigFilePath);
      if (Config.User != null) GData.Baidu = Config.User.GetTranslation();
    }

    public static string InputBoxPassWord()
    {
      //[Dim: System.Words]
      //OK = 确定(&O)
      //Cancel = 取消(&C)
      Input input = Input.GetFormParameter();
      input.OK.Text = GData.Data["Dim:System.Button", "OK", "确定(&O)"];
      input.Cancel.Text = GData.Data["Dim:System.Button", "Cancel", "取消(&C)"];
      input.txtValue.UseSystemPasswordChar = true;
      return Input.InputBox
         (GData.Data["Dim:System.Caption", "Input_Password_Caption", "请输入当前用户设置的密码："]
        , GData.Data["Dim:System.Caption", "Input_Password_Title", "输入密码"]
      );
    }
    private OpenFileDialog _OpenFileDialog;
    private OpenFileDialog OpenFileDialog
    {
      get
      {
        if (_OpenFileDialog == null)
          _OpenFileDialog = new OpenFileDialog() { FileName = "", Multiselect = true/*, InitialDirectory = @"C:\Program Files\dotnet\sdk\NuGetFallbackFolder"*/ };
        return _OpenFileDialog;
      }
    }
    private SaveFileDialog _SaveFileDialog;
    private SaveFileDialog SaveFileDialog
    {
      get
      {
        if (_SaveFileDialog == null)
          _SaveFileDialog = new SaveFileDialog() { FileName = "" };
        return _SaveFileDialog;
      }
    }

    public LanguageData LoadLanguageDataDefault()
    {
      return LanguageData.LoadFile(@"Data\zh.rldf")
        ?? LanguageData.LoadFile(@"Data\Temp.rldf")
        ?? LanguageData.Create();
    }
    public bool OpenFile(string filter, out string[] files)
    {
      OpenFileDialog.Filter = filter;
      if (OpenFileDialog.ShowDialog() == DialogResult.OK)
      {
        files = OpenFileDialog.FileNames;
        return true;
      }
      else
      {
        files = null;
        return false;
      }
    }
    public bool SaveFile(string filter, out string file)
    {
      SaveFileDialog.Filter = filter;
      if (SaveFileDialog.ShowDialog() == DialogResult.OK)
      {
        file = SaveFileDialog.FileName;
        return true;
      }
      else
      {
        file = null;
        return false;
      }
    }
    private static GlobalData _GData;
    public static GlobalData GData
    {
      get
      {
        if (_GData == null)
          _GData = new GlobalData();
        return _GData;
      }
    }
  }

  internal delegate void LanguageDataChangeEventHandler(GlobalData sender, EventArgs e);

  namespace Only
  {
    internal class Environment
    {
      public bool UseLanguage { get; set; }
      public string SelectData { get; set; }
      public User User { get; set; }
      public bool Original { get; set; }

      public int OutType { get; set; }
      public int OutTypeSave { get; set; }
      public bool BackupSaveType { get; set; }
      public string BackupSavePath { get; set; } = "Backups";
      public string OutputPath { get; set; } = "Output";

      private Environment()
      {
        UseLanguage = false;

      }

      public void SaveConfig()
      {
        SaveConfig(GData.ConfigFilePath);
        //string path = GData.ConfigFilePath;
        //using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
        //using (BinaryWriter bw = new BinaryWriter(fs, Encoding.UTF8))
        //{
        //  string check = "DocumentTranslationConfig";
        //  bw.Write(check.ToCharArray());
        //  bw.Write(UseLanguage);
        //  bw.Write(SelectData ?? "");
        //  bw.Write(User == null ? "" : User.AppID);
        //  bw.Write(Original);
        //}
      }
      public void SaveConfig(string path)
      {
        using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
        using (BinaryWriter bw = new BinaryWriter(fs, Encoding.UTF8))
        {
          string check = "DocumentTranslationConfig";
          bw.Write(check.ToCharArray());
          bw.Write(UseLanguage);
          bw.Write(SelectData ?? "");
          bw.Write(User == null ? "" : User.AppID);
          bw.Write(Original);
          bw.Write(OutType);
          bw.Write(OutTypeSave);
          bw.Write(BackupSaveType);
          bw.Write(BackupSavePath);
          bw.Write(OutputPath);
        }
      }
      public static Environment LoadConfigOrDefault(string path)
      {
        Environment config;
        if (!File.Exists(path))
        {
          config = new Environment();
          config.SaveConfig(path);
          return config;
        }
        config = LoadConfig(path);
        if (config == null)
        {
          config = new Environment();
          config.SaveConfig(path);
          return config;
        }
        return config;
      }
      public static Environment LoadConfig(string path)
      {
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
        using (BinaryReader br = new BinaryReader(fs, Encoding.UTF8))
        {
          string check = "DocumentTranslationConfig";
          if (check != new string(br.ReadChars(check.Length)))
          {
            return null;
          }
          Environment config = new Environment();
          string stringValue;
          config.UseLanguage = br.ReadBoolean();
          config.SelectData = br.ReadString();
          //if (config.UseLanguage)
          //{
          //  GData.Data = LanguageData.LoadFile(config.SelectData);
          //}
          //if (GData.Data == null)
          //{
          //  config.LoadLanguageDataDefault();
          //}
          stringValue = br.ReadString();
          if (stringValue != "" && Translation.CheckAppID(stringValue))
          {
            config.User = User.LoadUser($@"User\{stringValue}.bins");
          }
          try
          {
            config.Original = br.ReadBoolean();
            config.OutType = br.ReadInt32();
            config.OutTypeSave = br.ReadInt32();
            config.BackupSaveType = br.ReadBoolean();
            config.BackupSavePath = br.ReadString();
            config.OutputPath = br.ReadString();
          }
          catch { }
          return config;
        }
      }
    }
  }

  internal class User
  {
    public event EventHandler Canceling;
    public string AppID { get; set; }
    public string SecretKey { get; set; }
    public string Password { get; set; }
    public bool IsCheck { get; set; }
    public string Name { get; set; }
    public long Length { get; set; }
    public bool Stop { get; set; }
    private Translation translation;
    public Translation GetTranslation()
    {
      if (translation == null)
      {
        translation = new Translation(AppID, SecretKey)
        {
          Length = Length,
        };
        translation.LengthChange += (sender, e) =>
          {
            Length = translation.Length;
            if (Length >= 1998620)
            {
              translation.Cancel = Stop;
            }
          };
        translation.Canceled += (sender, e) =>
          {
            Canceling?.Invoke(this, new EventArgs());
          };
      }
      return translation;
    }
    public void SaveUser(string path)
    {
      using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
      using (BinaryWriter bw = new BinaryWriter(fs, Encoding.Unicode, true))
      {
        fs.Seek(0, SeekOrigin.Begin);
        bw.Write(Password);
        bw.Write(AppID);
        bw.Write(SecretKey);
        bw.Write(IsCheck);
        bw.Write(Name);
        bw.Write(Length);
        bw.Write(Stop);
      }
      using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
      {
        stream.Seek(0, SeekOrigin.Begin);
        byte[] data = new byte[stream.Length];
        stream.Read(data, 0, data.Length);
        string md5 = data.GetMD5();
        File.WriteAllText($@"User\{AppID}.check", md5);
      }
      new Secret(GetSecretKey(Password)).EncryptionFile(path);
    }
    public static User LoadUser(string path)
    {
      if (!File.Exists(path))
      {
        if (File.Exists(Path.ChangeExtension(path, "check")))
        {
          File.Delete(Path.ChangeExtension(path, "check"));
        }
        return null;
      }
      if (!File.Exists(Path.ChangeExtension(path, "check")))
      {
        if (File.Exists(path))
        {
          File.Delete(path);
        }
        return null;
      }
      string pwd = GetPassword("");
      MemoryStream ms;
      while (!Check(pwd, out ms))
      {
        pwd = InputBoxPassWord();
        if (pwd == "")
        {
          break;
        }
        pwd = GetPassword(pwd);
      }

      bool Check(string password, out MemoryStream stream)
      {
        Secret s = new Secret(GetSecretKey(password));
        try
        {
          stream = s.DecryptFileToStream(path);
        }
        catch
        {
          stream = null;
          return false;
        }
        stream.Seek(0, SeekOrigin.Begin);
        byte[] data = new byte[stream.Length];
        stream.Read(data, 0, data.Length);
        stream.Seek(0, SeekOrigin.Begin);
        string md5 = data.GetMD5();
        string soumd5 = File.ReadAllText(Path.ChangeExtension(path, "check")).Trim();
        if (md5 == soumd5)
        {
          return true;
        }
        else
        {
          //stream = null;
          return false;
        }
      }
      try
      {
        using (BinaryReader br = new BinaryReader(ms, Encoding.Unicode))
        {
          User user = new User
          {
            Password = br.ReadString(),
            AppID = br.ReadString(),
            SecretKey = br.ReadString(),
            IsCheck = br.ReadBoolean(),
            Name = br.ReadString(),
            Length = br.ReadInt64(),
            Stop = br.ReadBoolean()
          };
          return user;
        }
      }
      catch
      {
        return null;
      }
      finally
      {
        //GData.Secret.EncryptionFile(path);
      }
    }

    internal static SecretKey GetSecretKey(string password)
    {
      if (password == null) password = "";
      string s = $"{password.GetMD5()}|SecretKey|★|Renlen".GetMD5();
      return new SecretKey(s, Encoding.Unicode, true);
    }
    internal static string GetPassword(string password)
    {
      if (password == null) password = "";
      return $"{password.GetMD5()}|DocumentTranslation.User|★|Renlen".GetMD5();
    }
  }
}
