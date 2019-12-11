using System;
using System.IO;
using Renlen.TranslateFile;

namespace Renlen.FileTranslator
{

    public partial class XmlFileOfVS
    {
        private class ReadWriter : IReadWriter<IWillTranslateFile>
        {
            public IWillTranslateFile Read(Stream stream)
            {
                return new XmlFileOfVS(stream);
            }

            public void Write(Stream stream, IWillTranslateFile o)
            {
                if (o is XmlFileOfVS xml)
                {
                    xml.Save(stream);
                }
                else
                {
                    throw new ArgumentException("无法接受的对象类型。",nameof(o));
                }
            }
        }
    }

}
