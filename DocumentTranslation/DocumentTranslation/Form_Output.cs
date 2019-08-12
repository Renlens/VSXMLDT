using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DocumentTranslation.GlobalData;

namespace DocumentTranslation
{
  public partial class Form_Output : Form
  {
    public OutputConfig Value { get; private set; }
    public Form_Output(bool isConfig)
    {
      InitializeComponent();
      if (isConfig)
      {
        checkBox1.Visible = false;
      }
      else
      {
        radioButton1.Visible = false;
      }
    }

    public void CancelRegisterLanguageDataChange()
    {
      GData.LanguageDataChange -= GData_LanguageDataChange;
    }
    private void Form_Output_Load(object sender, EventArgs e)
    {
      GData.LanguageDataChange += GData_LanguageDataChange;
      GData_LanguageDataChange(GData, new EventArgs());
      if (radioButton1.Visible)
      {
        switch (GData.Config.OutType)
        {
          case 0:
            radioButton1.Checked = true;
            break;
          case 1:
            radioButton2.Checked = true;
            break;
          case 2:
            radioButton3.Checked = true;
            break;
          case 3:
            radioButton4.Checked = true;
            break;
          default:
            radioButton1.Checked = true;
            break;
        }
        radioButton6.Checked = !GData.Config.BackupSaveType;
        textBox1.Text = GData.Config.BackupSavePath;
        textBox2.Text = GData.Config.OutputPath;
        if (checkBox1.Visible && !(radioButton1.Checked || radioButton1.Checked || radioButton1.Checked || radioButton1.Checked))
          button1.Enabled = false;
      }
      else
      {
        switch (GlobalData.GData.Config.OutTypeSave)
        {
          case 1:
            radioButton2.Checked = true;
            break;
          case 2:
            radioButton3.Checked = true;
            break;
          case 3:
            radioButton4.Checked = true;
            break;
          default:
            radioButton1.Checked = true;
            break;
        }
        radioButton6.Checked = !GData.Config.BackupSaveType;
        textBox1.Text = GData.Config.BackupSavePath;
        textBox2.Text = GData.Config.OutputPath;
      }
    }

    private void GData_LanguageDataChange(GlobalData sender, EventArgs e)
    {
      GData.Data.LoadForm(this
        , typeof(Button)
        , typeof(Label)
        , typeof(RadioButton)
        , typeof(CheckBox)
        );
    }

    private void Button3_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog dialog = new FolderBrowserDialog();
      if (dialog.ShowDialog() == DialogResult.OK)
      {
        textBox1.Text = dialog.SelectedPath;
      }
      dialog.Dispose();
    }

    private void Button2_Click(object sender, EventArgs e)
    {
      Value = null;
      Close();
    }

    private void Button4_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog dialog = new FolderBrowserDialog();
      if (dialog.ShowDialog() == DialogResult.OK)
      {
        textBox2.Text = dialog.SelectedPath;
      }
      dialog.Dispose();
    }

    private void Button1_Click(object sender, EventArgs e)
    {
      if (radioButton1.Visible || checkBox1.Checked)
      {
        GlobalData.GData.Config.OutType =
          radioButton1.Checked ? 0 :
          radioButton2.Checked ? 1 :
          radioButton3.Checked ? 2 :
          radioButton4.Checked ? 3 : 0;
        GlobalData.GData.Config.OutTypeSave = 1;
        //GlobalData.GData.Config.IsAsked = radioButton1.Checked;
        GlobalData.GData.Config.BackupSaveType = radioButton5.Checked;
        GlobalData.GData.Config.BackupSavePath = textBox1.Text;
        GlobalData.GData.Config.OutputPath = textBox2.Text;
      }
      else
      {
        GlobalData.GData.Config.OutType = 0;
        GlobalData.GData.Config.OutTypeSave =
          radioButton1.Checked ? 0 :
          radioButton2.Checked ? 1 :
          radioButton3.Checked ? 2 :
          radioButton4.Checked ? 3 : 1;
        //GlobalData.GData.Config.IsAsked = radioButton1.Checked;
        GlobalData.GData.Config.BackupSaveType = radioButton5.Checked;
        GlobalData.GData.Config.BackupSavePath = textBox1.Text;
        GlobalData.GData.Config.OutputPath = textBox2.Text;
      }
      Value = new OutputConfig(
        radioButton1.Checked ? 0 : radioButton2.Checked ? 1 : radioButton3.Checked ? 2 : radioButton4.Checked ? 3 : 0,
        radioButton5.Checked,
        textBox1.Text,
        textBox2.Text
        );
    }

    private void RadioButton1_CheckedChanged(object sender, EventArgs e)
    {
      if (checkBox1.Visible && !(radioButton1.Checked || radioButton1.Checked || radioButton1.Checked || radioButton1.Checked))
        button1.Enabled = false;
      else
        button1.Enabled = true;
    }

    private void Form_Output_FormClosing(object sender, FormClosingEventArgs e)
    {
      CancelRegisterLanguageDataChange();
    }
  }

  public class OutputConfig
  {
    public OutputConfig(int outType, bool backupSaveType, string backupSavePath, string outputPath)
    {
      OutType = outType;
      BackupSaveType = backupSaveType;
      BackupSavePath = backupSavePath;
      OutputPath = outputPath;
    }
    public int OutType { get; }
    public bool BackupSaveType { get; }
    public string BackupSavePath { get; }
    public string OutputPath { get; }


    private bool init = true;
    private bool _IsBackup;
    private bool IsBackup
    {
      get
      {
        if (init)
        {
          Init();
        }
        return _IsBackup;
      }
    }
    private Func<string, string> _ToOutPath;
    private Func<string, string> ToOutPath
    {
      get
      {
        if (init)
        {
          Init();
        }
        return _ToOutPath;
      }
    }
    private Func<string, string> _ToBackupPath;
    private Func<string, string> ToBackupPath
    {
      get
      {
        if (init)
        {
          Init();
        }
        return _ToBackupPath;
      }
    }
    public void Check(string path, out bool isBackup, out string outpath, out string backuppath)
    {
      isBackup = IsBackup;
      outpath = ToOutPath(path);
      backuppath = ToBackupPath(path);
    }

    private void Init()
    {
      _IsBackup = OutType == 1;
      Func<string, string> me = new Func<string, string>(p => p);
      switch (OutType)
      {
        case 1:
          if (!BackupSaveType) Directory.CreateDirectory(BackupSavePath);
          _ToBackupPath = BackupSaveType ? new Func<string, string>(p => Path.ChangeExtension(p, ".backup.xml")) : new Func<string, string>(p => Path.Combine(BackupSavePath, Path.GetFileNameWithoutExtension(p) + ".backup.xml"));
          _ToOutPath = me;
          break;
        case 2:
          _ToBackupPath = me;
          _ToOutPath = me;
          break;
        case 3:
          _ToBackupPath = me;
          Directory.CreateDirectory(OutputPath);
          _ToOutPath = new Func<string, string>(p => Path.Combine(OutputPath, Path.GetFileName(p)));
          break;
        default:
          _ToBackupPath = me;
          _ToOutPath = me;
          break;
      }
    }
  }
}
