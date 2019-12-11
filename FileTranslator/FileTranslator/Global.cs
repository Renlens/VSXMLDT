using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Renlen.TranslateFile;

namespace Renlen.FileTranslator
{
    internal static class Global
    {
        private static Type FileInterface { get; } = typeof(IWillTranslateFile);
        /// <summary>
        /// 全局唯一随机函数（项目内）
        /// </summary>
        public static Random GRandom { get; } = new Random();

        public static List<Type> FileTypes = new List<Type>();

        public static bool LoadFileTypes(string path, out string message)
        {
            FileTypes.Add(typeof(XmlFileOfVS));
            if (Directory.Exists(path))
            {
                string[] dllFiles;
                try
                {
                    dllFiles = Directory.GetFiles(path, "*.dll");
                }
                catch (IOException)
                {
                    message = "加载反射文件列表：读取反射文件列表失败！";
                    return false;
                }
                StringBuilder msg = new StringBuilder();
                foreach (string file in dllFiles)
                {
                    Assembly dll;
                    try
                    {
                        dll = Assembly.LoadFrom(file);
                        foreach (Type type in dll.GetTypes())
                        {
                            if (FileInterface.IsAssignableFrom(type))
                            {
                                FileTypes.Add(type);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        msg.Append("异常：");
                        msg.Append(ex.Message);
                        msg.Append(" 文件：");
                        msg.Append(file);
                        msg.AppendLine();
                    }
                }
                message = msg.ToString();
                return string.IsNullOrWhiteSpace(message);
            }
            else
            {
                message = "加载反射文件列表：指定的路径不存在！";
                return false;
            }
        }
    }
}
