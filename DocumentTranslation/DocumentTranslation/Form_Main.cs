using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Renlen.BaseLibrary;
using static DocumentTranslation.GlobalData;
using Renlen.BaseLibrary.Secret;
using System.Xml;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace DocumentTranslation
{
  public partial class Form_Main : Form
  {
    /// <summary>
    /// 在当前窗体上显示对话框。
    /// </summary>
    /// <param name="text">要显示的消息</param>
    /// <param name="caption">对话框的标题。如果为 <see langword="null"/> ,标题则为消息(享受语言转换)。</param>
    /// <param name="buttons">对话框的按钮。默认为 <see cref="MessageBoxButtons.OK"/> 。</param>
    /// <param name="icon">对话框的图标。默认为 <see cref="MessageBoxIcon.Information"/> 。</param>
    /// <returns></returns>
    public DialogResult ShowDialog(string text, string caption = null, MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Information)
    {
      caption = caption ?? GData.Data["Dim:System.Other", "Message", "消息"];
      return (DialogResult)this.Invoke(new Func<DialogResult>(() =>
       {
         return MessageBox.Show(text, caption, buttons, icon);
       }));
    }

    public Form_Main()
    {
      InitializeComponent();
    }

    private void Form_Main_Load(object sender, EventArgs e)
    {
      GData.LanguageDataChange += GData_LanguageDataChange;

      //var v = LanguageData.LoadFile(@"Language\简体中文(编辑).ini");
      //Form_Output fo = new Form_Output(true);
      //Form_Config fc = new Form_Config();
      //v.AddForm(this
      //  , typeof(Button)
      //  , typeof(Label)
      //);
      //v.AddForm(fc
      //  , typeof(Button)
      //  , typeof(Label)
      //  , typeof(GroupBox)
      //  , typeof(RadioButton)
      //  , typeof(CheckBox)
      //  , typeof(LinkLabel)
      //);
      //v.AddForm(fo
      //  , typeof(Button)
      //  , typeof(Label)
      //  , typeof(RadioButton)
      //  , typeof(CheckBox)
      //);
      //fc.Dispose();
      //fo.Dispose();
      //v.SaveFileIni(@"Language\简体中文(系统).ini");
      ////v.SaveFileIni(@"Language\English.ini");
      //v.Lock();
      //v.SaveFileRLDF(@"Data\zh.rldf");
      ////v = LanguageData.LoadFile(@"Language\English.ini");
      ////GData.Data = v;

      GData_LanguageDataChange(GData, null);

      bool flag = true;
      foreach (string item in Enum.GetNames(typeof(Language)))
      {
        comboBox1.Items.Add(item);
        if (flag)
        {
          flag = false;
          continue;
        }
        comboBox2.Items.Add(item);
      }
      //comboBox1.DataSource = Enum.GetNames(typeof(Language));
      //comboBox2.DataSource = comboBox1.DataSource;
      comboBox1.SelectedIndex = 0;
      comboBox2.SelectedIndex = 0;


      //listBox1.Items.Add(@"E:\Renlen\Desktop\SlimDX.xml");

      //VSXMLData xml = new VSXMLData();
      //xml.Load(@"Test\System.Collections.xml");
      //xml.Load(@"Test\System.Null.xml");
      //xml.StartTranslation();
      //xml.Save(@"Test\System.Collections2.xml");

      //GData.Config.SaveConfig();
      //GData.Baidu = new Translation("20190614000307590", "FzpVNpx5OVA5lCc2Dq7I")
      //{
      //  Length = 100000,
      //  SourceLanguage = Language.auto,
      //  TargetLanguage = Language.zh
      //};

    }

    private void GData_LanguageDataChange(GlobalData sender, EventArgs e)
    {
      GData.Data.LoadForm(this, typeof(Button), typeof(Label));
    }

    private void Button1_Click(object sender, EventArgs e)
    {
      if (GData.OpenFile("VS成员注释文档|*.xml", out var files))
      {
        listBox1.Items.AddRange(files);
      }
    }

    private void Button4_Click(object sender, EventArgs e)
    {
      new Form_Config().ShowDialog();
    }

    private void Button3_Click(object sender, EventArgs e)
    {
      listBox1.Items.Clear();
    }

    public void RefreshFormat()
    {
      comboBox1.Format -= ComboBox1_Format;
      comboBox1.Format += ComboBox1_Format;
      comboBox2.Format -= ComboBox1_Format;
      comboBox2.Format += ComboBox1_Format;
    }
    private void ComboBox1_Format(object sender, ListControlConvertEventArgs e)
    {
      string value = e.ListItem.ToString();
      e.Value = GData.Data["Enum:Renlen.BaseLibrary.Language", value, value];
    }

    VSXMLData data = null;
    private void Start(OutputConfig oc)
    {
      if (listBox1.Items.Count > 0)
      {
        string[] task = new string[listBox1.Items.Count];
        listBox1.Items.CopyTo(task, 0);
        int current = -1;
        data = new VSXMLData();
        data.ProgressChange += VSXMLData_ProgressChange;

        data.Canceled += (sender, e) =>
          {
            this.Invoke(new Action(() =>
            {
              label1.Text = GData.Data["Dim:System.Caption", "Message_Main_Completed", "已完成"];
              textBox1.Text = GData.Data["Dim:System.Caption", "Message_Main_Completed", "已完成"];
              //progressBar1.Maximum = 1;
              progressBar1.Value = progressBar1.Maximum;
              progressBar2.Maximum = 1;
              progressBar2.Value = 1;
              button5.Enabled = true;
              state = 3;
              if (data != null) data.Pause = false;
              button5.Text = GData.Data["Dim:System.Button", "Start", "开始(&S)"];
            }));
          };
        VSXMLData_ProgressChange(data, (true, 1, 1, ""));
        void VSXMLData_ProgressChange(VSXMLData sender, (bool IsCompleted, int Progress, int MaxValue, string Member) e)
        {
          if (e.Progress > e.MaxValue) e.Progress = e.MaxValue;
          if (e.IsCompleted)
          {
            current++;
            if (current == task.Length)
            {
              this.Invoke(new Action(() =>
              {
                label1.Text = GData.Data["Dim:System.Caption", "Message_Main_Completed", "已完成"];
                textBox1.Text = GData.Data["Dim:System.Caption", "Message_Main_Completed", "已完成"];
                //progressBar1.Maximum = 1;
                progressBar1.Value = progressBar1.Maximum;
                progressBar2.Maximum = 1;
                progressBar2.Value = 1;
                button5.Enabled = true;
                state = 3;
                if (data != null) data.Pause = false;
                button5.Text = GData.Data["Dim:System.Button", "Start", "开始(&S)"];
              }));
              return;
            }
            data.Load(task[current]);
            data.CreateTranslationProgress();
            data.StartTranslation(oc);
            this.Invoke(new Action(() =>
            {
              //progressBar1.Maximum = task.Length;
              double value = ((double)current / (double)task.Length) + ((double)progressBar2.Value / (double)progressBar2.Maximum) / (double)task.Length;
              progressBar1.Value = (int)Math.Floor(value * progressBar1.Maximum);
              label1.Text = $"{GData.Data["Dim:System.Caption", "Message_Main_CurrentTranslation", "正在翻译："]}({progressBar1.Value}/{progressBar1.Maximum}) {task[current]}";
            }));
          }
          else
          {
            this.Invoke(new Action(() =>
            {
              progressBar2.Maximum = e.MaxValue;
              progressBar2.Value = e.Progress;
              textBox1.Text = $"{GData.Data["Dim:System.Caption", "Message_Main_CurrentTranslation", "正在翻译："]}({progressBar2.Value}/{progressBar2.Maximum}) {e.Member}";
              //progressBar1.Maximum = task.Length;
              double value = ((double)current / (double)task.Length) + ((double)progressBar2.Value / (double)progressBar2.Maximum) / (double)task.Length;
              progressBar1.Value = (int)Math.Floor(value * progressBar1.Maximum);
              label1.Text = $"{GData.Data["Dim:System.Caption", "Message_Main_CurrentTranslation", "正在翻译："]}({current}/{task.Length}) {task[current]}";
            }));
          }
        }
      }
      else
      {
        button5.Enabled = true;
      }
    }
    int state = 0; //0未开始 1工作 2暂停 3结束
    public void Pause()
    {
      this.Invoke(new Action(() =>
      {
        if (data != null) data.Pause = true;
        state = 2;
        button5.Text = GData.Data["Dim:System.Button", "Continue", "继续(&C)"];
      }));
    }
    private void Button5_Click(object sender, EventArgs e)
    {
      switch (state)
      {
        case 0:
        case 3:
          if (GData.Config.User == null)
          {
            MessageBox.Show
              (GData.Data["Dim:System.Caption", "Caption_NonUser", "请先登录！输入用户信息后保存设置！"]
              , GData.Data["Dim:System.Other", "Message", "消息"]);
            Button4_Click(button4, new EventArgs());
            return;
          }
          OutputConfig oc = null;
          if (GData.Config.OutType == 0)
          {
            Form_Output fo = new Form_Output(false);
            if (fo.ShowDialog() != DialogResult.OK)
            {
              return;
            }
            oc = fo.Value;
          }
          if (oc == null)
          {
            oc = new OutputConfig(
              GData.Config.OutType,
              GData.Config.BackupSaveType,
              GData.Config.BackupSavePath,
              GData.Config.OutputPath
              );
          }
          state = 1;
          if (data != null) data.Pause = false;
          button5.Text = GData.Data["Dim:System.Button", "Pause", "暂停(&P)"];
          GData.Baidu.SourceLanguage = (Language)Enum.Parse(typeof(Language), comboBox1.SelectedItem.ToString());
          GData.Baidu.TargetLanguage = (Language)Enum.Parse(typeof(Language), comboBox2.SelectedItem.ToString());
          Start(oc);
          break;
        case 1:
          Pause();
          break;
        case 2:
          if (data != null) data.Pause = false;
          state = 1;
          button5.Text = GData.Data["Dim:System.Button", "Pause", "暂停(&P)"];
          break;
        default:
          break;
      }

    }

    private void Button2_Click(object sender, EventArgs e)
    {
      if (listBox1.SelectedIndices.Count > 0)
      {
        int[] indexs = new int[listBox1.SelectedIndices.Count];
        listBox1.SelectedIndices.CopyTo(indexs, 0);
        for (int i = indexs.Length - 1; i >= 0; i--)
        {
          listBox1.Items.RemoveAt(indexs[i]);
        }
      }
    }

    private void Timer1_Tick(object sender, EventArgs e)
    {

    }

    private void Translation(LanguageData ld, Language language)
    {
      Translation translation = GData.Baidu;
      string str;
      JObject result;
      LKey[] keys = (from item in ld
                     select item.Key).ToArray();
      foreach (var key in keys)
      {
        str = ld[key];
        str = translation.TranslationString(str, Language.auto, language);
        result = JsonDocument.LoadJson(str);
        str = result.Last.Last.Last["dst"].ToString();
        ld[key] = str;
        System.Threading.Thread.Sleep(3000);
      }
    }
    private void Translation(LanguageData ld, Language language, string node)
    {
      Translation translation = GData.Baidu;
      string str;
      JObject result;
      LKey[] keys = (from item in ld
                     where item.Key.Name == node
                     select item.Key).ToArray();
      foreach (var key in keys)
      {
        str = ld[key];
        str = translation.TranslationString(str, Language.auto, language);
        result = JsonDocument.LoadJson(str);
        str = result.Last.Last.Last["dst"].ToString();
        ld[key] = str;
        System.Threading.Thread.Sleep(3000);
      }
    }

    private void Button6_Click(object sender, EventArgs e)
    {
      Language language = (Language)Enum.Parse(typeof(Language), comboBox2.SelectedItem.ToString());
      Translation(GData.Data, language);
      GData.Data.SaveFileIni(@"Language\1.ini");
    }

    private void Button7_Click(object sender, EventArgs e)
    {
      Language language = (Language)Enum.Parse(typeof(Language), comboBox2.SelectedItem.ToString());
      Translation(GData.Data, language, "Form:Form_Output");
      GData.Data.SaveFileIni(@"Language\1.ini");
    }
  }
}
