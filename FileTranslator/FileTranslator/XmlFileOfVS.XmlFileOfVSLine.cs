using System;
using System.Xml;

namespace Renlen.FileTranslator
{

    public partial class XmlFileOfVS
    {
        private partial class XmlFileOfVSLine : ITranslatingLine, ITranslatingLineReadWrite
        {
            private readonly string hash;
            private readonly string memberName;
            private readonly string path;
            private readonly int index;
            private readonly LineType type;
            public ResultCode Code { get; set; }
            public string Text { get; set; }
            private string result;
            private string finalResult;
            public string Result
            {
                get
                {
                    if (finalResult != null)
                    {
                        return finalResult;
                    }
                    if (string.IsNullOrWhiteSpace(result))
                    {
                        finalResult = "";
                        return finalResult;
                    }
                    if (type == LineType.Text)
                    {
                        finalResult = result;
                    }
                    else if (type == LineType.Element)
                    {

                    }
                    return finalResult;
                }
                set
                {
                    result = value;
                }
            }
            public XmlFileOfVSLine(string text, string hash, string memberName, string path, int index, LineType type)
            {
                this.Text = text;
                this.hash = hash;
                this.memberName = memberName;
                this.path = path;
                this.index = index;
                this.type = type;
            }

            public void CommitResult()
            {

            }


            public IReadWriter<ITranslatingLine> GetReadWriter()
            {
                throw new NotImplementedException();
            }
        }
    }

}
