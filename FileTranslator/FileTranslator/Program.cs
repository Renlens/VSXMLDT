using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Renlen.FileTranslator.Global;

namespace Renlen.FileTranslator
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppDomain.CurrentDomain.AssemblyResolve += (sender, re) =>
            {
                if (re.Name.Contains("Renlen.TranslateFile"))
                {
                    return Assembly.LoadFrom(@"Renlen.TranslateFile.dll");
                }
                else
                {
                    return null;
                }
            };
            string delPath = @"FileType\Renlen.TranslateFile.dll";
            if (File.Exists(delPath))
            {
                File.Delete(delPath);
            }
            Global.LoadFileTypes("FileType", out string msg);
            if (!string.IsNullOrWhiteSpace(msg))
            {
                string err = $"加载扩展类型时出错：\r\n{msg}";
                if (MessageBox.Show($"{err}\r\n\r\n是否将错误信息复制到剪贴板？", "错误", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Clipboard.SetText(err);
                }
            }

            Application.Run(MainForm);
        }
    }
}
