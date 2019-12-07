using System.Collections.Generic;

namespace Renlen.FileTranslator
{
    /// <summary>
    /// 表示一个将要翻译的文件。
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
        /// 如果此文件是一个硬盘的文件，则此属性返回文件全路径。
        /// </summary>
        string FullPath { get; }
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
