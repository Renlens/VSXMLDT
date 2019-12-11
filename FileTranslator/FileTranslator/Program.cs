using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            Global.LoadFileTypes("FileType", out string msg);
            if (!string.IsNullOrWhiteSpace(msg))
            {
                MessageBox.Show(msg);
            }
#if DEV
            Application.Run(new FormMain());
#else
            Application.Run(new FormMain2());
#endif
        }
    }
}
