#nullable enable

using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;

namespace Renlen.TranslateFile
{
    /// <summary>
    /// 表示一个将要翻译的文件。此类必须有公有无参数构造函数用来辅助创建实例。
    /// </summary>
    public interface IWillTranslateFile
    {
        /// <summary>
        /// 获取类本身的相关信息。
        /// </summary>
        FileAbout? About { get; }
        /// <summary>
        /// 获取指示此文件的源是否是一个磁盘中的文件的值。
        /// </summary>
        bool IsFile { get; }
        /// <summary>
        /// 如果此文件是一个硬盘的文件，则此属性返回文件全路径。否则，应返回文件的标识，以路径的格式。
        /// </summary>
        string FullPath { get; }
        /// <summary>
        /// 从文件创建一个实例
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        IWillTranslateFile FromFile(string path);
        /// <summary>
        /// 从流创建一个实例
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        IWillTranslateFile FromStream(Stream stream);
        /// <summary>
        /// 获取此文件要翻译的所有行。
        /// </summary>
        /// <returns></returns>
        IEnumerable<ITranslatingLine> GetTranslatingLines();
        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <returns></returns>
        FileSize GetFileSize();
    }

    /// <summary>
    /// 存储与类实例无关只与类本身有关的信息。注意：所有属性均在 <see langword="null"/> 值时才接受赋值。
    /// </summary>
    public class FileAbout
    {
        private string? name;
        private string? caption;
        private string? fileFilter;
        private string? auther;
        private bool? isPause;
        private bool? isContinuous;
        private bool? isFromFile;
        private bool? isFromStream;

        /// <summary>
        /// 对文件的简单描述。
        /// <para>例如：Visual Studio 成员注释文档</para>
        /// </summary>
        public string? Name
        {
            get => name;
            set => name ??= value;
        }
        /// <summary>
        /// 对文件的说明。
        /// <para>例如：对 Visual Studio 内程序集内的成员 XML 注释文档的翻译文件。必须遵循 XML 文档规范，否则结果可能有误。</para>
        /// </summary>
        public string? Caption
        {
            get => caption;
            set => caption ??= value;
        }
        /// <summary>
        /// 文件筛选器。用于打开文件对话框中文件的筛选。建议只包含一个 "|" 。虽然在赋值时并不会对其格式进行检查，但有的软件可能会忽略错误的格式。
        /// <para>例如：Visual Studio 成员注释文档|*.xml</para>
        /// </summary>
        public string? FileFilter
        {
            get => fileFilter;
            set => fileFilter ??= value;
        }
        /// <summary>
        /// 作者。允许包含联系方式等。
        /// </summary>
        public string? Auther
        {
            get => auther;
            set => auther ??= value;
        }
        /// <summary>
        /// 获取指示此文件是否可以保存，下次继续翻译的值。
        /// </summary>
        public bool? IsPause { get => isPause; set => isPause ??= value; }
        /// <summary>
        /// 获取指示此文件是否必须按固定顺序提交翻译结果的值。
        /// </summary>
        public bool? IsContinuous
        {
            get => isContinuous;
            set => isContinuous ??= value;
        }
        /// <summary>
        /// 表示此类可以从文件创建
        /// </summary>
        public bool? IsFromFile
        {
            get => isFromFile;
            set => isFromFile ??= value;
        }
        /// <summary>
        /// 表示此类可以从流创建
        /// </summary>
        public bool? IsFromStream
        {
            get => isFromStream;
            set => isFromStream ??= value;
        }
    }
}
