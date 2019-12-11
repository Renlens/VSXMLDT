using System;
using System.IO;
using System.Text.RegularExpressions;
using Renlen.TranslateFile;
using static Renlen.FileTranslator.Global;

namespace Renlen.FileTranslator
{
    public class TypeRef
    {
        public IWillTranslateFile NullFile { get; }
        public string Name { get; }
        public string Caption { get; }
        public string Auther { get; }
        public string FullName { get; }
        public Type Type { get; }
        public string FileFilter { get; }
        public bool IsFromFile { get; }
        public bool IsFromStream { get; }

        public TypeRef(Type type)
        {
            if (type.IsInterface)
                throw new ArgumentException("接口类型无效！", nameof(type));
            if (type.IsAbstract)
                throw new ArgumentException("抽象类型无效！", nameof(type));
            //if (FileInterface.IsAssignableFrom(type))
            //    throw new ArgumentException("类型未直接或间接实现 Renlen.IWillTranslateFile 接口。", nameof(type));
            if (type.GetConstructor(Array.Empty<Type>()) == null)
                throw new ArgumentException("未找到此类型的无参数公共构造函数。", nameof(type));

            Type = type;
            FullName = Type.FullName;

            NullFile = (IWillTranslateFile)Type.Assembly.CreateInstance(FullName);
            FileAbout about = NullFile.About;
            if (about == null)
            {
                Name = Type.Name;
                FileFilter = $"{Name}|*.*";
                Auther = "";
            }
            else
            {
                Name = string.IsNullOrWhiteSpace(about.Name) ? type.Name : about.Name;
                FileFilter = about.FileFilter ?? $"{Name}|*.*";
                if (!Regex.IsMatch(FileFilter, @"^[^\|\r\n]+ *\| *(?:[^\|\r\n]+)(?:;[^\|\r\n]+)*$"))
                {
                    FileFilter = $"{Name}|*.*";
                }
                Auther = about.Auther ?? "";
                IsFromFile = about.IsFromFile ?? false;
                IsFromStream = about.IsFromStream ?? false;
            }
        }

        public IWillTranslateFile Create(string path)
        {
            if (IsFromFile)
            {
                return NullFile.FromFile(path);
            }
            else if (IsFromStream)
            {
                FileStream stream = new FileStream(path, FileMode.Open);
                return NullFile.FromStream(stream);
            }
            else
            {
                return null;
            }
        }

        public static bool operator ==(TypeRef x, TypeRef y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;
            return x.FullName == y.FullName;
        }
        public static bool operator !=(TypeRef x, TypeRef y)
        {
            if (x == null && y == null) return false;
            if (x == null || y == null) return true;
            return x.FullName != y.FullName;
        }
        public override bool Equals(object obj)
        {
            TypeRef other = obj as TypeRef;
            return this == other;
        }
        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }
    }
}
