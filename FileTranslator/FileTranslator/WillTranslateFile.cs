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

        public bool IsFromFile => true;

        public bool IsFromStream => true;

        public FileAbout About => null;

        public IWillTranslateFile FromFile(string path)
        {
            return new WillTranslateFile();
        }

        public IWillTranslateFile FromStream(Stream stream)
        {
            return new WillTranslateFile();
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
            return FileSize.NoSize;
        }
    }
}
