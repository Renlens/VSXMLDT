using Renlen.BaseLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DocumentTranslation.GlobalData;

namespace DocumentTranslation
{
  public partial class Form_Config : Form
  {
    private bool init = true;
    public Form_Config()
    {
      InitializeComponent();
    }
    public void CancelRegisterLanguageDataChange()
    {
      GData.LanguageDataChange -= GData_LanguageDataChange;
    }
    private void GData_LanguageDataChange(GlobalData sender, EventArgs e)
    {
      GData.Data.LoadForm(this
        , typeof(Button)
        , typeof(Label)
        , typeof(GroupBox)
        , typeof(RadioButton)
        , typeof(CheckBox)
        , typeof(LinkLabel)
        );
    }

    private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      System.Diagnostics.Process.Start(@"http://api.fanyi.baidu.com/api/trans/product/prodinfo");
    }
    private void LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      System.Diagnostics.Process.Start(@"http://api.fanyi.baidu.com/api/trans/product/desktop?req=developer");
    }
    private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      MessageBox.Show(
@"注册个人开发者免费，使用通用翻译API即可。
通用翻译API每月前200万字每月前200万字每月前200万字免费，
超出部分超出部分超出部分按49.00元/百万字收费。
（价格仅供参考，实际价格以官网为准！）
（价格仅供参考，实际价格以官网为准！）
（价格仅供参考，实际价格以官网为准！）
因为不是完全免费的，所以只能使用者自己注册。

重要的事情说三遍！重要的事情说三遍！重要的事情说三遍！
"
      , "消息"
      , MessageBoxButtons.OK
      , MessageBoxIcon.Information);
    }

    private void Button1_Click(object sender, EventArgs e)
    {
      User user = new User()
      {
        AppID = textBox1.Text.Trim(),
        SecretKey = textBox2.Text.Trim(),
        Password = User.GetPassword(textBox3.Text),
        Length = long.TryParse(textBox5.Text.Trim(), out long r) ? r : 0,
        Name = textBox4.Text.Trim(),
        IsCheck = false,
        Stop = checkBox4.Checked
      };
      if (user.AppID == "" || !Translation.CheckAppID(user.AppID))
      {
        MessageBox.Show("APPID格式不正确。", "消息");
      }
      else if (user.SecretKey == "" || !Translation.CheckSecretKey(user.SecretKey))
      {
        MessageBox.Show("密钥格式不正确。", "消息");
      }
      else if (user.Password != User.GetPassword(textBox6.Text))
      {
        MessageBox.Show("两次输入的密码不一致。", "消息");
      }
      else
      {
        bool flag = true;
        if (GData.Config.User != null && GData.Config.User.AppID == user.AppID)
        {
          if (GData.Config.User.Password != User.GetPassword("") && GData.Config.User.Password != user.Password)
          {
            string password;
            do
            {
              password = InputBoxPassWord();
              if (password == "")
              {
                flag = false;
                break;
              }
            } while (GData.Config.User.Password != User.GetPassword(password));
          }
        }
        else if (File.Exists($@"User\{user.AppID}.bins"))
        {
          flag = User.LoadUser($@"User\{user.AppID}.bins") != null;
        }
        if (flag)
        {
          user.SaveUser($@"User\{user.AppID}.bins");
          GData.Config.User = user;
          GData.Config.SaveConfig();
          GData.Baidu = new Translation(user.AppID, user.SecretKey);
        }
      }
    }

    /**
     * 错误消息:未将对象引用设置到对象的实例。
     * 
     * 详细信息：
     * 出错方法：Void Form_Config_Load(System.Object, System.EventArgs)
     * 调用堆栈：
     *    在 DocumentTranslation.Form_Config.Form_Config_Load(Object sender, EventArgs e) 位置 E:\Renlen\项目\C#\小工具\DocumentTranslation\DocumentTranslation\Form_Config.cs:行号 102
     *    在 System.Windows.Forms.Form.OnLoad(EventArgs e)
     *    在 System.Windows.Forms.Form.OnCreateControl()
     *    在 System.Windows.Forms.Control.CreateControl(Boolean fIgnoreVisible)
     *    在 System.Windows.Forms.Control.CreateControl()
     *    在 System.Windows.Forms.Control.WmShowWindow(Message& m)
     *    在 System.Windows.Forms.Control.WndProc(Message& m)
     *    在 System.Windows.Forms.Form.WmShowWindow(Message& m)
     *    在 System.Windows.Forms.NativeWindow.Callback(IntPtr hWnd, Int32 msg, IntPtr wparam, IntPtr lparam)
     * 
     */
    private void Form_Config_Load(object sender, EventArgs e)
    {
      GData.LanguageDataChange += GData_LanguageDataChange;
      //listBox1.FormatString = GData.Data["Dim:System.Caption", "Caption_LDH_NoName", "(未命名)"];
      GData_LanguageDataChange(null, null);
      if (GData.Config == null)
      {
        //GData.Config = Only.Environment.LoadConfigOrDefault("NonExists");
      }
      if (GData.Config.User != null)
      {
        textBox1.Text = GData.Config.User.AppID;
        textBox2.Text = GData.Config.User.SecretKey;
        textBox4.Text = GData.Config.User.Name;
        textBox5.Text = GData.Config.User.Length.ToString();
        checkBox4.Checked = GData.Config.User.Stop;
      }
      comboBox1.BeginUpdate();
      try
      {
        foreach (var item in from file in Directory.GetFiles(@"Language")
                             where file.EndsWith(".ini", StringComparison.OrdinalIgnoreCase)
                             || file.EndsWith(".rldf", StringComparison.OrdinalIgnoreCase)
                             select LanguageDataHead.LoadFileHead(file))
        {
          if (item != null) comboBox1.Items.Add(item);
        }
      }
      catch { }
      finally { comboBox1.EndUpdate(); }
      textBox1.BeginUpdate();
      try
      {
        foreach (var item in from file in Directory.GetFiles(@"User")
                             where file.EndsWith(".bins", StringComparison.OrdinalIgnoreCase)
                             select Path.GetFileNameWithoutExtension(file))
        {
          textBox1.Items.Add(item);
        }
      }
      catch { }
      finally { textBox1.EndUpdate(); }

      if (GData.Config.SelectData != "")
      {
        string path;
        try { path = Path.GetFullPath(GData.Config.SelectData).ToLower(); }
        catch { path = ""; }
        if (path != "")
        {
          foreach (LanguageDataHead ldh in comboBox1.Items)
          {
            if (path == Path.GetFullPath(ldh.Path).ToLower())
            {
              comboBox1.SelectedItem = ldh;
              break;
            }
          }
        }
        if (comboBox1.SelectedItem == null)
        {
          GData.Config.UseLanguage = false;
          GData.Config.SelectData = "";
        }
      }
      radioButton1.Checked = !GData.Config.UseLanguage;
      radioButton2.Checked = GData.Config.UseLanguage;

      checkBox2.Checked = GData.Config.Original;

      init = false;
    }

    private void LinkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      System.Diagnostics.Process.Start(@"http://api.fanyi.baidu.com/api/trans/product/desktop");
    }

    private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      listBox1.Items.Clear();
      if (comboBox1.SelectedItem is LanguageDataHead ldh)
      {
        string value = ldh.GetSystemInfo("Name");
        if (value == "") value = GData.Data["Dim:System.Caption", "Caption_LDH_NonName", "(未读取到名称信息)"];
        listBox1.Items.Add(value);
        value = ldh.GetSystemInfo("Type");
        if (value == "") value = GData.Data["Dim:System.Caption", "Caption_LDH_NonKnow", "(未知)"];
        listBox1.Items.Add($"{GData.Data["Dim:System.Caption", "Caption_LDH_Type", "语言类型："]}{value}");
        value = ldh.GetSystemInfo("Author");
        if (value == "") value = GData.Data["Dim:System.Caption", "Caption_LDH_NonKnow", "(未知)"];
        listBox1.Items.Add($"{GData.Data["Dim:System.Caption", "Caption_LDH_Author", "文件作者："]}{value}");
        value = ldh.GetSystemInfo("UseSoftware");
        if (value == "") value = GData.Data["Dim:System.Caption", "Caption_LDH_NonKnow", "(未知)"];
        listBox1.Items.Add($"{GData.Data["Dim:System.Caption", "Caption_LDH_UseSoftware", "可用软件："]}{value}");
        value = ldh.GetSystemInfo("Caption");
        if (value == "") value = "";
        listBox1.Items.Add($"{GData.Data["Dim:System.Caption", "Caption_LDH_Caption", "文件说明："]}{value}");
        listBox1.Items.Add("------------------------------------------");
        foreach (var head in ldh.GetHeadInfoReadOnly())
        {
          listBox1.Items.Add($"{head.Key}:{head.Value}");
        }
        if (init) return;
        init = true;
        radioButton2.Checked = true;
        init = false;
        GData.Config.SelectData = ldh.Path;
        GData.Data = ldh.ToLanguageData();
        this.RefreshFormat();
        GData.Form_Main.RefreshFormat();
      }
      else
      {
        if (init) return;
        GData.Config.UseLanguage = false;
        GData.Config.SelectData = "";
      }
      GData.Config.SaveConfig();
    }

    private void Form_Config_FormClosing(object sender, FormClosingEventArgs e)
    {
      CancelRegisterLanguageDataChange();
    }

    private void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
      if (init) return;
      GData.Config.UseLanguage = radioButton2.Checked;
      if (radioButton2.Checked)
      {
        ComboBox1_SelectedIndexChanged(null, null);
      }
      else
      {
        GData.Data = GData.LoadLanguageDataDefault();
        GData.Config.SaveConfig();
      }
    }

    private void CheckBox2_CheckedChanged(object sender, EventArgs e)
    {
      if (init) return;
      GData.Config.Original = checkBox2.Checked;
      GData.Config.SaveConfig();
    }

    public void RefreshFormat()
    {
      init = true;
      comboBox1.Format -= ComboBox1_Format;
      comboBox1.Format += ComboBox1_Format;
      init = false;
    }
    private void ComboBox1_Format(object sender, ListControlConvertEventArgs e)
    {
      e.Value = (e.ListItem as LanguageDataHead).ToString(GData.Data["Dim:System.Caption", "Caption_LDH_NoName", "(未命名)"]);
    }

    private void Button4_Click(object sender, EventArgs e)
    {
      new Form_Output(true).ShowDialog();
    }

    private void TextBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      string path = $@"User\{textBox1.Text}.bins";
      if (File.Exists(path))
      {
        User user = User.LoadUser(path);
        if (user == null)
        {
          //user = null;
          //textBox1.Text = GData.Config.User.AppID;
          textBox2.Text = "";
          textBox4.Text = "";
          textBox5.Text = "";
          textBox3.Text = "";
          textBox6.Text = "";
          //checkBox4.Checked = GData.Config.User.Stop;
        }
        else
        {
          GData.Config.User = user;
          textBox1.Text = GData.Config.User.AppID;
          textBox2.Text = GData.Config.User.SecretKey;
          textBox4.Text = GData.Config.User.Name;
          textBox5.Text = GData.Config.User.Length.ToString();
          checkBox4.Checked = GData.Config.User.Stop;
          textBox3.Text = "";
          textBox6.Text = "";
        }
      }
    }

    private void Button2_Click(object sender, EventArgs e)
    {
      string id = textBox1.Text;
      if (File.Exists($@"User\{id}.bins"))
      {
        File.Delete($@"User\{id}.bins");
      }
      if (File.Exists($@"User\{id}.check"))
      {
        File.Delete($@"User\{id}.check");
      }
    }
  }
}
