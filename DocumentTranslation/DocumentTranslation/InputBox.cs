using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocumentTranslation
{
  /// <summary>
  /// 提供输入对话框相关的方法。
  /// </summary>
  public sealed class Input : Form
  {
    /// <summary>
    /// 对话框中的确认按钮
    /// </summary>
    public Button OK = new Button();
    /// <summary>
    /// 对话框中的取消按钮
    /// </summary>
    public Button Cancel = new Button();
    /// <summary>
    /// 对话框中的提示标签
    /// </summary>
    public Label Caption = new Label();
    /// <summary>
    /// 对话框请用户输入的文本框
    /// </summary>
    public TextBox txtValue = new TextBox();
    /// <summary>
    /// 获取文本框中的值
    /// </summary>
    public string Value = "";
    /// <summary>
    /// 该值指示使用InputBox()方法后是否自动进行初始化。
    /// </summary>
    public static bool AutoInitialization = true;
    /// <summary>
    /// 该值指示InputBox()方法中当文本框为空时确定按钮是否有效。
    /// </summary>
    public static bool NullOKEnabled = false;
    bool CloseEnabled = false;

    static int PromptLengthMAX = 1024;
    static int CaptionLengthMAX = 256;
    static int DefaultValueLengthMAX = 1024;
    static Input fi = new Input();

    /// <summary>
    /// 指示InputBox()方法中的取消按钮是否可用
    /// </summary>
    public enum CancelButtonEnabled
    {
      /// <summary>
      /// 表示取消按钮可用
      /// </summary>
      Visible,
      /// <summary>
      /// 表示取消按钮不可用
      /// </summary>
      Invisible
    }
    /// <summary>
    /// 指示按钮所使用的语言
    /// </summary>
    public enum ButtonLanguage
    {
      ///// <summary>
      ///// 获取系统所使用的语言
      ///// </summary>
      //System,
      /// <summary>
      /// 简体中文
      /// </summary>
      Chinese,
      /// <summary>
      /// 英语
      /// </summary>
      English,
      ///// <summary>
      ///// 日语
      ///// </summary>
      //Japanese
    }


    /// <summary>
    /// 当参数不符合规定形式时引发的异常
    /// </summary>
    [Serializable]
    public class InputBoxParameterException : Exception
    {
      /// <summary>
      /// 
      /// </summary>
      public InputBoxParameterException() { }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="message"></param>
      public InputBoxParameterException(string message) : base(message) { }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="message"></param>
      /// <param name="inner"></param>
      public InputBoxParameterException(string message, Exception inner) : base(message, inner) { }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="info"></param>
      /// <param name="context"></param>
      protected InputBoxParameterException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context)
      { }
    }

    private Input()
    {
      Name = "fms";
      MaximizeBox = false;
      MinimizeBox = false;
      FormBorderStyle = FormBorderStyle.FixedDialog;
      ControlBox = false;
      Text = "输入";
      Width = 640;

      Controls.Add(OK);
      Controls.Add(Cancel);
      Controls.Add(Caption);
      Controls.Add(txtValue);

      OK.Name = "OK";
      OK.Size = new Size(75, 23);
      OK.Text = "确定(&O)";
      OK.TabIndex = 1;
      OK.Location = new Point(OK.Location.X + 530, OK.Location.Y + 20);
      OK.Click += new EventHandler(this.OK_Click);

      Cancel.Name = "Cancel";
      Cancel.Size = new Size(75, 23);
      Cancel.Text = "取消(&C)";
      Cancel.TabIndex = 2;
      Cancel.Location = new Point(Cancel.Location.X + 530, Cancel.Location.Y + 50);
      Cancel.Click += new EventHandler(this.Cancel_Click);

      Caption.Name = "Caption";
      Caption.AutoSize = true;
      //Caption.BorderStyle = BorderStyle.Fixed3D;
      Caption.MaximumSize = new Size(432, 600);
      Caption.Text = "";
      Caption.TabIndex = 3;
      Caption.Location = new Point(Caption.Location.X + 20, Caption.Location.Y + 20);

      txtValue.Name = "Value";
      txtValue.Width = 585;
      txtValue.Anchor = AnchorStyles.Bottom;
      txtValue.Text = Value;
      txtValue.TabIndex = 0;
      txtValue.Location = new Point(txtValue.Location.X + 20, txtValue.Location.Y + 160);
      txtValue.TextChanged += new EventHandler(txtValue_TextChanged);

      Height = Math.Max(240, Caption.Height + 100);
      txtValue.Location = new Point(txtValue.Location.X, Height - 70);
      StartPosition = FormStartPosition.CenterParent;
      AcceptButton = OK;

      this.FormClosing += new FormClosingEventHandler(Form_FormClosing);

      txtValue_TextChanged(null, null);
    }
    private void OK_Click(object sender, EventArgs e)
    {
      Value = txtValue.Text;
      CloseEnabled = true;
      Close();
    }
    private void txtValue_TextChanged(object sender, EventArgs e)
    {
      if (NullOKEnabled)
      {
        OK.Enabled = true;
        return;
      }
      if (txtValue.Text.Trim() == "")
      {
        OK.Enabled = false;
      }
      else
      {
        OK.Enabled = true;
      }
    }
    private void Cancel_Click(object sender, EventArgs e)
    {
      Value = "";
      CloseEnabled = true;
      Close();
    }
    private void Form_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!CloseEnabled)
      {
        e.Cancel = true;
      }
      CloseEnabled = false;
    }
    /// <summary>
    /// 在对话框中显示提示，等待用户输入文本或单击按钮，然后返回包含文本框内容的字符串。
    /// </summary>
    /// <param name="Prompt">String类型的变量或表达式，作为消息显示在对话框中</param>
    /// <returns>返回值为字符串</returns>
    public static string InputBox(string Prompt)
    {
      if (Prompt.Length > PromptLengthMAX)
      {
        InputBoxParameterException iex = new InputBoxParameterException("参数错误。参数Prompt的长度不可超过" + PromptLengthMAX + "个字符");
        throw iex;
      }
      string Value;
      Input fi = Input.fi;

      fi.Caption.Text = Prompt;
      string name = Application.ExecutablePath.Replace("/", "\\");
      while (name.IndexOf("\\") > 0)
      {
        name = name.Substring(name.IndexOf("\\") + 1);
      }
      fi.Text = name;//Caption;
      fi.Value = "";// DefaultValue;
      fi.txtValue.Text = fi.Value;

      fi.Height = Math.Max(240, fi.Caption.Height + 100);
      fi.ShowDialog();
      Value = fi.Value;
      if (AutoInitialization) Initialization();
      return Value;
    }
    /// <summary>
    /// 在对话框中显示提示，等待用户输入文本或单击按钮，然后返回包含文本框内容的字符串。
    /// </summary>
    /// <param name="Prompt">String类型的变量或表达式，作为消息显示在对话框中</param>
    /// <param name="Caption">String类型的变量或表达式，作为对话框的标题</param>
    /// <returns>返回值为字符串</returns>
    public static string InputBox(string Prompt, string Caption)
    {
      if (Prompt.Length > PromptLengthMAX)
      {
        InputBoxParameterException iex = new InputBoxParameterException("参数错误。参数Prompt的长度不可超过" + PromptLengthMAX + "个字符");
        throw iex;
      }
      if (Caption.Length > CaptionLengthMAX)
      {
        InputBoxParameterException iex = new InputBoxParameterException("参数错误。参数Caption的长度不可超过" + CaptionLengthMAX + "个字符");
        throw iex;
      }
      string Value;
      Input fi = Input.fi;

      fi.Caption.Text = Prompt;
      fi.Text = Caption;
      fi.Value = "";// DefaultValue;
      fi.txtValue.Text = fi.Value;

      fi.Height = Math.Max(240, fi.Caption.Height + 100);
      fi.ShowDialog();
      Value = fi.Value;
      if (AutoInitialization) Initialization();
      return Value;
    }
    /// <summary>
    /// 在对话框中显示提示，等待用户输入文本或单击按钮，然后返回包含文本框内容的字符串。
    /// </summary>
    /// <param name="Prompt">String类型的变量或表达式，作为消息显示在对话框中</param>
    /// <param name="Caption">String类型的变量或表达式，作为对话框的标题</param>
    /// <param name="DefaultValue">String类型的变量或表达式，作为文本框的默认值</param>
    /// <returns>返回值为字符串</returns>
    public static string InputBox(string Prompt, string Caption, string DefaultValue)
    {
      if (Prompt.Length > PromptLengthMAX)
      {
        InputBoxParameterException iex = new InputBoxParameterException("参数错误。参数Prompt的长度不可超过" + PromptLengthMAX + "个字符");
        throw iex;
      }
      if (Caption.Length > CaptionLengthMAX)
      {
        InputBoxParameterException iex = new InputBoxParameterException("参数错误。参数Caption的长度不可超过" + CaptionLengthMAX + "个字符");
        throw iex;
      }
      if (DefaultValue.Length > DefaultValueLengthMAX)
      {
        InputBoxParameterException iex = new InputBoxParameterException("参数错误。参数DefaultValue的长度不可超过" + DefaultValueLengthMAX + "个字符");
        throw iex;
      }
      string Value;
      Input fi = Input.fi;

      fi.Caption.Text = Prompt;
      fi.Text = Caption;
      fi.Value = DefaultValue;
      fi.txtValue.Text = fi.Value;

      fi.Height = Math.Max(240, fi.Caption.Height + 100);
      fi.ShowDialog();
      Value = fi.Value;
      if (AutoInitialization) Initialization();
      return Value;
    }
    /// <summary>
    /// 在对话框中显示提示，等待用户输入文本或单击按钮，然后返回包含文本框内容的字符串。
    /// </summary>
    /// <param name="Prompt">String类型的变量或表达式，作为消息显示在对话框中</param>
    /// <param name="Caption">String类型的变量或表达式，作为对话框的标题</param>
    /// <param name="DefaultValue">String类型的变量或表达式，作为文本框的默认值</param>
    /// <param name="CancelButtonEnabled">指示取消按钮是否可用</param>
    /// <returns>返回值为字符串</returns>
    public static string InputBox(string Prompt, string Caption, string DefaultValue, CancelButtonEnabled CancelButtonEnabled)
    {
      if (Prompt.Length > PromptLengthMAX)
      {
        InputBoxParameterException iex = new InputBoxParameterException("参数错误。参数Prompt的长度不可超过" + PromptLengthMAX + "个字符");
        throw iex;
      }
      if (Caption.Length > CaptionLengthMAX)
      {
        InputBoxParameterException iex = new InputBoxParameterException("参数错误。参数Caption的长度不可超过" + CaptionLengthMAX + "个字符");
        throw iex;
      }
      if (DefaultValue.Length > DefaultValueLengthMAX)
      {
        InputBoxParameterException iex = new InputBoxParameterException("参数错误。参数DefaultValue的长度不可超过" + DefaultValueLengthMAX + "个字符");
        throw iex;
      }
      string Value;
      Input fi = Input.fi;

      fi.Caption.Text = Prompt;
      fi.Text = Caption;
      fi.Value = DefaultValue;
      fi.txtValue.Text = fi.Value;
      if (CancelButtonEnabled == CancelButtonEnabled.Invisible)
      {
        fi.Cancel.Enabled = false;
      }

      fi.Height = Math.Max(240, fi.Caption.Height + 100);
      fi.ShowDialog();
      Value = fi.Value;
      if (AutoInitialization) Initialization();
      return Value;
    }
    /// <summary>
    /// 在对话框中显示提示，等待用户输入文本或单击按钮，然后返回包含文本框内容的字符串。
    /// </summary>
    /// <param name="Prompt">String类型的变量或表达式，作为消息显示在对话框中</param>
    /// <param name="Caption">String类型的变量或表达式，作为对话框的标题</param>
    /// <param name="DefaultValue">String类型的变量或表达式，作为文本框的默认值</param>
    /// <param name="CancelButtonEnabled">指示取消按钮是否可用</param>
    /// <param name="ButtonLanguage">指示按钮所使用的语言</param>
    /// <returns>返回值为字符串</returns>
    public static string InputBox(string Prompt, string Caption, string DefaultValue, CancelButtonEnabled CancelButtonEnabled, ButtonLanguage ButtonLanguage)
    {
      if (Prompt.Length > PromptLengthMAX)
      {
        InputBoxParameterException iex = new InputBoxParameterException("参数错误。参数Prompt的长度不可超过" + PromptLengthMAX + "个字符");
        throw iex;
      }
      if (Caption.Length > CaptionLengthMAX)
      {
        InputBoxParameterException iex = new InputBoxParameterException("参数错误。参数Caption的长度不可超过" + CaptionLengthMAX + "个字符");
        throw iex;
      }
      if (DefaultValue.Length > DefaultValueLengthMAX)
      {
        InputBoxParameterException iex = new InputBoxParameterException("参数错误。参数DefaultValue的长度不可超过" + DefaultValueLengthMAX + "个字符");
        throw iex;
      }
      string Value;
      Input fi = Input.fi;

      fi.Caption.Text = Prompt;
      fi.Text = Caption;
      fi.Value = DefaultValue;
      fi.txtValue.Text = fi.Value;
      if (CancelButtonEnabled == CancelButtonEnabled.Invisible)
      {
        fi.Cancel.Enabled = false;
      }
      if (ButtonLanguage == ButtonLanguage.Chinese)
      {
        fi.OK.Text = "确定(&O)";
        fi.Cancel.Text = "取消(&C)";
      }
      else if (ButtonLanguage == ButtonLanguage.English)
      {
        fi.OK.Text = "&OK";
        fi.Cancel.Text = "&Cancel";
      }
      //else if (ButtonLanguage == ButtonLanguage.Japanese)
      //{
      //    fi.OK.Text = "かくてい";
      //    fi.Cancel.Text = "キヤソセル";
      //}
      fi.Height = Math.Max(240, fi.Caption.Height + 100);
      fi.ShowDialog();
      Value = fi.Value;
      if (AutoInitialization) Initialization();
      return Value;
    }
    /// <summary>
    /// 内部函数。使用内部指令更改窗体容易出错的参数。
    /// </summary>
    /// <param name="Command">窗体参数更改指令</param>
    public static void SetParameter(string Command)
    {
      if (Command.Length > 10 && Command.Substring(0, 7) == "Command")
      {
        try
        {
          if (Command.Substring(7, 3) == "PPM")
          {
            PromptLengthMAX = Convert.ToInt32(Command.Substring(10));
          }
          else if (Command.Substring(7, 3) == "PCM")
          {
            CaptionLengthMAX = Convert.ToInt32(Command.Substring(10));
          }
          else if (Command.Substring(7, 3) == "PDM")
          {
            DefaultValueLengthMAX = Convert.ToInt32(Command.Substring(10));
          }
          else
          {
            int c = 0;
            c = 1 / c;
          }
        }
        catch
        {
          InputBoxParameterException iex = new InputBoxParameterException("参数错误。参数Command不符合规则");
          throw iex;
        }
      }
      else
      {
        InputBoxParameterException iex = new InputBoxParameterException("参数错误。参数Command不符合规则");
        throw iex;
      }
    }
    /// <summary>
    /// 获取当前窗体的参数
    /// </summary>
    /// <returns></returns>
    public static Input GetFormParameter()
    {
      return fi;
    }
    /// <summary>
    /// 将当前参数设置为窗体的参数。可使用Initialization()方法初始化参数。
    /// </summary>
    public void SetFormParameter()
    {
      fi = this;
    }
    /// <summary>
    /// 设置窗体的参数。可使用Initialization()方法初始化参数。
    /// </summary>
    /// <param name="Parameter">窗体的参数</param>
    public static void SetFormParameter(Input Parameter)
    {
      fi = Parameter;
    }
    /// <summary>
    /// 初始化窗体参数
    /// </summary>
    public static void Initialization()
    {
      fi = new Input();
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      // 
      // Input
      // 
      this.ClientSize = new System.Drawing.Size(284, 262);
      this.Name = "Input";
      this.ResumeLayout(false);

    }
    ///// <summary>
    ///// 初始化TAB顺序，如果不进行此初始化，对话框的默认焦点将不再文本框中，文本框中的默认值也不会被选中。使用Initialization()方法初始化也可以。
    ///// </summary>
    //public static void InitializationIndex()
    //{
    //    fi.Focus();
    //    fi.txtValue.SelectAll();
    //}
  }
}
