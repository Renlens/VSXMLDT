using System;
using System.IO;

namespace Renlen.FileTranslator
{

    public partial class XmlFileOfVS
    {


        private partial class XmlFileOfVSLine
        {
            private class ReadWriter : IReadWriter<ITranslatingLine>
            {
                public ITranslatingLine Read(Stream stream)
                {
                    throw new NotImplementedException();
                }

                public void Write(Stream stream, ITranslatingLine o)
                {
                    throw new NotImplementedException();
                }
            }
        }
    }

}
