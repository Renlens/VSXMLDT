using System.Collections.Generic;

namespace Renlen.FileTranslator
{
    public interface IWillTranslateFile
    {
        bool IsPause { get; }
        bool IsContinuous { get; }
        bool IsFile { get; }
        string FullPath { get; }
        IEnumerable<ITranslatingLine> GetTranslatingLines();
        long GetFileSize();
    }
}
