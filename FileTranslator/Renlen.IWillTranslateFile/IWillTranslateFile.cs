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
        /// 获取指示此文件是否可以保存，下次继续翻译的值。
        /// </summary>
        bool IsPause { get; }
        /// <summary>
        /// 获取指示此文件是否必须按固定顺序提交翻译结果的值。
        /// </summary>
        bool IsContinuous { get; }
        /// <summary>
        /// 获取指示此文件的源是否是一个磁盘中的文件的值。
        /// </summary>
        bool IsFile { get; }
        /// <summary>
        /// 如果此文件是一个硬盘的文件，则此属性返回文件全路径。否则，应返回文件的标识，以路径的格式。
        /// </summary>
        string FullPath { get; }
        /// <summary>
        /// 表示此类可以从文件创建
        /// </summary>
        bool IsFromFile { get; }
        /// <summary>
        /// 表示此类可以从流创建
        /// </summary>
        bool IsFromStream { get; }
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
}
