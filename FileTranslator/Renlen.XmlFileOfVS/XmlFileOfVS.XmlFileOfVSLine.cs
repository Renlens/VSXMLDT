using System;
using System.Xml;
using Renlen.TranslateFile;

namespace Renlen.FileTranslator
{

    public partial class XmlFileOfVS
    {
        private partial class XmlFileOfVSLine : ITranslatingLine
        {
            public static IReadWriter<ITranslatingLine> LineReadWriter { get; } = new ReadWriter();

            internal XmlFileOfVS File { get; set; }
            private readonly string memberName;
            private readonly string path;
            private readonly int index;
            private readonly LineType type;
            public int Code { get; set; }
            public string Text { get; }
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
                        finalResult = result;
                    }
                    return finalResult;
                }
                set
                {
                    result = value;
                }
            }

            public XmlFileOfVSLine(XmlFileOfVS file, string text, string memberName, string path, int index, LineType type)
            {
                this.File = file;
                this.Text = text;
                this.memberName = memberName;
                this.path = path;
                this.index = index;
                this.type = type;
            }

            public void CommitResult()
            {
                TempNode tempNode = File.TempNodes[memberName];
                TempValue tempValue = tempNode?[$"{path}/{index}"];
                tempValue?.Commit(Result);
            }

            public override string ToString()
            {
                return Text?.ToString() ?? "";
            }
        }
    }

}
