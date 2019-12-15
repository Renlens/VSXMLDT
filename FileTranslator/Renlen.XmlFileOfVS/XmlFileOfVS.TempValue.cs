using System.Text;
using System.Xml;

namespace Renlen.FileTranslator
{
    public partial class XmlFileOfVS
    {
        /// <summary>
        /// 表示缓存节点值，它可以是单个xml节点，也可以是多个xml节点
        /// </summary>
        private class TempValue
        {
            private XmlNode node;
            private int startIndex;
            private int length;
            public bool IsSingle { get; }

            public TempValue(XmlNode node)
            {
                this.node = node;
                IsSingle = true;
            }

            public TempValue(XmlNode node, int start, int length)
            {
                this.node = node;
                startIndex = start;
                this.length = length;
                IsSingle = false;
            }

            public void Commit(string result)
            {
                if (IsSingle)
                {
                    string originalText;
                    if (node.NodeType == XmlNodeType.Text)
                    {
                        originalText = node.Value;
                        node.Value = $"{result} Original : {originalText}";
                    }
                    else
                    {
                        originalText = node.InnerXml;
                        node.InnerXml = $"{result} Original : {originalText}";
                    }
                }
                else
                {
                    StringBuilder originalText = new StringBuilder();
                    for (int i = 0; i < length; i++)
                    {
                        if (node.ChildNodes[startIndex].NodeType == XmlNodeType.Text)
                        {
                            originalText.Append(node.ChildNodes[startIndex].Value);
                        }
                        else
                        {
                            originalText.Append(node.ChildNodes[startIndex].OuterXml);
                        }
                        node.RemoveChild(node.ChildNodes[startIndex]);
                    }
                    XmlNode xmlNode = node.OwnerDocument.CreateElement("resulttemp");
                    xmlNode.InnerXml = $"{result} Original : {originalText.ToString()}";
                    for (int i = startIndex; xmlNode.ChildNodes.Count > 0; i++)
                    {
                        node.InsertBefore(xmlNode.ChildNodes[0], node.ChildNodes[i]);
                    }
                }
            }
        }
    }
}
