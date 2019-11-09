using System.IO;

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
                (o as XmlFileOfVS).xml.Save(stream);
            }
        }
    }

}
