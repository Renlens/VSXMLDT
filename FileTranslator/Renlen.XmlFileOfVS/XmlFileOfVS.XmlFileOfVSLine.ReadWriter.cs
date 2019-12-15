using System;
using System.IO;
using Renlen.TranslateFile;

namespace Renlen.FileTranslator
{

    public partial class XmlFileOfVS
    {
        private partial class XmlFileOfVSLine
        {
            private class ReadWriter : IReadWriter<XmlFileOfVSLine>
            {
                private readonly XmlFileOfVS xml;
                public ReadWriter(XmlFileOfVS xml)
                {
                    this.xml = xml;
                }
                public XmlFileOfVSLine Read(Stream stream)
                {
                    return new XmlFileOfVSLine(stream) { File = xml };
                }

                public void Write(Stream stream, XmlFileOfVSLine line)
                {
                    line.Save(stream);
                }
            }
        }
    }

}
