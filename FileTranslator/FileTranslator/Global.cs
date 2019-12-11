using System;
using System.Collections;
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
        public static Type FileInterface { get; } = typeof(IWillTranslateFile);
        /// <summary>
        /// 全局唯一随机函数（项目内）
        /// </summary>
        public static Random GRandom { get; } = new Random();

        public static TypeList FileTypes = new TypeList();

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
                                FileTypes.Add(new TypeRef(type));
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

    public class TypeList : IEnumerable<TypeRef>
    {
        private List<TypeRef> types = new List<TypeRef>();

        public TypeRef this[int index]
        {
            get => types[index];
        }
        public TypeRef this[string fullName]
        {
            get => types.FirstOrDefault(t => t.FullName == fullName);
        }

        public void Add(TypeRef type)
        {
            if (!Contains(type))
            {
                types.Add(type);
            }
        }

        public void Remove(TypeRef type)
        {
            types.Remove(type);
        }

        public bool Contains(TypeRef type)
        {
            return types.Contains(type);
        }

        public IEnumerator<TypeRef> GetEnumerator()
        {
            return types.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
