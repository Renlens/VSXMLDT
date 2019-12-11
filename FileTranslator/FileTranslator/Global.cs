using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
                            if (!type.IsInterface && !type.IsAbstract && FileInterface.IsAssignableFrom(type))
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

    public class TypeRef
    {
        public string Name { get; }
        public string Caption { get; }
        public string FullName { get; }
        public Type Type { get; }
        public string FileFilter { get; }

        public TypeRef(Type type)
        {
            if (type.IsInterface)
                throw new ArgumentException("接口类型无效！", nameof(type));
            if (type.IsAbstract)
                throw new ArgumentException("抽象类型无效！", nameof(type));
            if (typeof(IWillTranslateFile).IsAssignableFrom(type))
                throw new ArgumentException("类型未直接或间接实现 Renlen.IWillTranslateFile 接口。", nameof(type));
            if (type.GetConstructor(Array.Empty<Type>()) == null) 
                throw new ArgumentException("未找到此类型的无参数公共构造函数。", nameof(type));

            Type = type;
            FullName = Type.FullName;


            IWillTranslateFile file = (IWillTranslateFile)Type.Assembly.CreateInstance(FullName);
            Name = string.IsNullOrWhiteSpace(file.Name) ? type.Name : file.Name;
            FileFilter = file.FileFilter ?? $"{Name}|*.*";
            Regex.IsMatch(FileFilter, @"^[^\|\r\n]\|(?:[^\|\r\n])+$");
        }
    }
}
