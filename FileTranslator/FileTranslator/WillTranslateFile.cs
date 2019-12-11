using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Renlen.TranslateFile;

namespace Renlen.FileTranslator
{
    public class WillTranslateFile : IWillTranslateFile
    {
        public string FullPath { get; protected set; }

        public bool IsPause { get; protected set; }

        public bool IsContinuous { get; protected set; }

        public bool IsFile { get; protected set; }

        public bool IsFromFile => throw new NotImplementedException();

        public bool IsFromStream => throw new NotImplementedException();

        public IWillTranslateFile FromFile(string path)
        {
            throw new NotImplementedException();
        }

        public IWillTranslateFile FromStream(Stream stream)
        {
            throw new NotImplementedException();
        }

        public virtual long GetFileSize()
        {
            return 0;
        }

        public virtual IEnumerable<ITranslatingLine> GetTranslatingLines()
        {
            yield break;
        }

        FileSize IWillTranslateFile.GetFileSize()
        {
            throw new NotImplementedException();
        }
    }
}
