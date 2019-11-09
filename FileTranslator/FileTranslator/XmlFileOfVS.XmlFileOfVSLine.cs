using System;
using System.Xml;

namespace Renlen.FileTranslator
{

    public partial class XmlFileOfVS
    {
        private partial class XmlFileOfVSLine : ITranslatingLine, ITranslatingLineReadWrite
        {
            private readonly XmlNode node;
            private readonly LineType type;
            private readonly long index;
            private readonly int count;
            private readonly long parentIndex;
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
                    switch (type)
                    {
                        case LineType.Text:
                            finalResult = result;
                            break;
                        case LineType.SingleElement:
                            break;
                        case LineType.Multielement:
                            break;
                        default:
                            break;
                    }
                    return finalResult;
                }
                set
                {
                    result = value;
                }
            }
            public XmlFileOfVSLine(XmlNode node, long index, int count = -1, long parentIndex = -1)
            {
                this.node = node;
                this.type = (parentIndex == -1 && count == -1) ?
                    LineType.Text :
                    parentIndex == -1 ?
                    LineType.Link :
                    LineType.Multielement;
                this.index = index;
                this.count = count;
                this.node = node;
                this.parentIndex = parentIndex;


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
