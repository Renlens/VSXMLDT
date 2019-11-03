using System;
using System.Collections.Generic;
using System.Data;

namespace Renlen.FileTranslator
{
    public class WillTranslateFile : IWillTranslateFile
    {
        public string FullPath { get; protected set; }

        public bool IsPause { get; protected set; }

        public bool IsContinuous { get; protected set; }

        public bool IsFile { get; protected set; }

        public virtual long GetFileSize()
        {
            return 0;
        }

        public virtual IEnumerable<ITranslatingLine> GetTranslatingLines()
        {
            yield break;
        }
    }
}
