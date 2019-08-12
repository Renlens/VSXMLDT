using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocumentTranslation
{
  static class Program
  {
    /// <summary>
    /// 应用程序的主入口点。
    /// </summary>
    [STAThread]
    static void Main()
    {
      global::System.Environment.CurrentDirectory = global::System.Windows.Forms.Application.StartupPath;
      Application.ThreadException += Application_ThreadException;

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      //GlobalData.Init();
      Application.Run(GlobalData.GData.Form_Main);

      GlobalData.GData.Data.Lock();
//#if DEBUG
//      GlobalData.GData.Data.SaveFileIni(@"Data\Temp.ini");
//#endif
      GlobalData.GData.Data.SaveFileRLDF(@"Data\Temp.rldf");
    }

    private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
    {
      string exceptionMessage = 
$@"错误消息:{e.Exception.Message}

详细信息：
出错方法：{e.Exception.TargetSite}
调用堆栈：
{e.Exception.StackTrace}";
      if (MessageBox.Show(
$@"系统发生了未知异常，是否将错误消息复制到剪贴板？

{exceptionMessage}"
         , "错误"
         , MessageBoxButtons.YesNo
         , MessageBoxIcon.Error) == DialogResult.Yes)
      {
        Clipboard.SetText(exceptionMessage);
      }
      Application.Exit();
    }
  }
}
