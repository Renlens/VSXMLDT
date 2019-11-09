using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Renlen.FileTranslator
{
    public partial class XmlFileOfVS : IWillTranslateFile, IWillTranslateFileReadWrite
    {
        private const string Multielement = "Multielement";
        private const string Skip = "Skip";
        private const string Link = "Link";

        private const string InElementIndex = "Index";
        private const string LinkIndex = "LinkIndex";

        private const string SegmentCount = "SegmentCount";

        private long segmentIndex = 0;

        private readonly ReadWriter readWriter = new ReadWriter();
        private FileSize fileSize = FileSize.Uninit;
        internal XmlDocument xml = new XmlDocument();

        public bool IsPause => true;
        public bool IsContinuous => false;
        public bool IsFile { get; private set; }
        public string FullPath { get; private set; }

        public XmlFileOfVS()
        {
            fileSize = FileSize.Uninit;
            IsFile = false;
            FullPath = "";
        }
        public XmlFileOfVS(string file)
        {
            IsFile = true;
            FullPath = Path.GetFullPath(file);
            using FileStream stream = new FileStream(FullPath, FileMode.OpenOrCreate, FileAccess.Read);
            xml.Load(stream);
            fileSize = (FileSize)stream.Length;
        }
        public XmlFileOfVS(Stream stream)
        {
            IsFile = false;
            xml.Load(stream);
            fileSize = (FileSize)stream.Length;
        }

        public long GetFileSize()
        {
            return (long)fileSize;
        }

        /// <summary>
        /// test <see cref="GetTranslatingLines"/> test2
        /// <para>
        /// test<see cref="DTSubString"/> test 
        ///     <para>
        ///     testtest
        ///     </para>
        /// </para>
        /// test
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ITranslatingLine> GetTranslatingLines()
        {
            XmlNodeList members = xml.GetElementsByTagName("member");
            foreach (XmlNode member in members)
            {
                foreach (XmlNode node in member.ChildNodes)
                {
                    foreach (ITranslatingLine line in Analysis(node))
                    {
                        yield return line;
                    }
                }
            }
        }

        /// <summary>
        /// 分析节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private IEnumerable<ITranslatingLine> Analysis(XmlNode node, long parentIndex = -1)
        {
            if (node.ChildNodes.Count == 0)
            {
                if (parentIndex != -1)
                {
                    yield return new XmlFileOfVSLine(null, this.segmentIndex++, -1, parentIndex);
                }
                yield break;
            }
            else if (node.ChildNodes.Count == 1)
            {
                XmlNode line = node.FirstChild;
                if (line.NodeType == XmlNodeType.Text)
                {
                    yield return new XmlFileOfVSLine(line, this.segmentIndex++, -1, parentIndex);
                }
                else if (Enum.TryParse(line.Name, out ElementType type))
                {
                    if (ElementType.Skip.HasFlag(type) || ElementType.Insert.HasFlag(type))
                    {
                        if (parentIndex != -1)
                        {
                            yield return new XmlFileOfVSLine(null, this.segmentIndex++, -1, parentIndex);
                        }
                        yield break;
                    }
                    else if (ElementType.Separate.HasFlag(type))
                    {
                        foreach (ITranslatingLine item in Analysis(line))
                        {
                            yield return item;
                        }
                    }
                }
            }
            else
            {
                long segmentIndex = this.segmentIndex++;
                List<XmlElement> elements = new List<XmlElement>();

                int index = 0;
                int linkIndex = 0;

                XmlElement element = null;
                XmlAttribute attribute;
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    if (node.ChildNodes[i].NodeType == XmlNodeType.Text)
                    {
                        if (element == null)
                        {
                            element = node.OwnerDocument.CreateElement(Multielement);
                            attribute = node.OwnerDocument.CreateAttribute(InElementIndex);
                            attribute.Value = index++.ToString();
                            element.Attributes.Append(attribute);
                        }
                        element.AppendChild(node.ChildNodes[i]);
                        continue;
                    }
                    else if (Enum.TryParse(node.ChildNodes[i].Name, out ElementType type))
                    {
                        if (ElementType.Insert.HasFlag(type))
                        {
                            if (element == null)
                            {
                                element = node.OwnerDocument.CreateElement(Multielement);
                                attribute = node.OwnerDocument.CreateAttribute(InElementIndex);
                                attribute.Value = index++.ToString();
                                element.Attributes.Append(attribute);
                            }
                            element.AppendChild(node.ChildNodes[i]);
                            continue;
                        }
                        else if (ElementType.Skip.HasFlag(type))
                        {
                            if (element != null && element.ChildNodes.Count > 0)
                            {
                                elements.Add(element);
                            }
                            element = node.OwnerDocument.CreateElement(Skip);
                            attribute = node.OwnerDocument.CreateAttribute(InElementIndex);
                            attribute.Value = index++.ToString();
                            element.Attributes.Append(attribute);
                            element.AppendChild(node.ChildNodes[i]);
                            elements.Add(element);
                            element = null;
                        }
                        else if (ElementType.Separate.HasFlag(type))
                        {
                            if (element != null && element.ChildNodes.Count > 0)
                            {
                                elements.Add(element);
                            }
                            element = node.OwnerDocument.CreateElement(Link);
                            attribute = node.OwnerDocument.CreateAttribute(InElementIndex);
                            attribute.Value = index++.ToString();
                            element.Attributes.Append(attribute);
                            attribute = node.OwnerDocument.CreateAttribute(LinkIndex);
                            attribute.Value = linkIndex++.ToString();
                            element.Attributes.Append(attribute);
                            element.AppendChild(node.ChildNodes[i]);
                            Analysis(node.ChildNodes[i], segmentIndex);
                            elements.Add(element);
                            element = null;
                        }
                    }
                }
                if (element != null && element.ChildNodes.Count > 0)
                {
                    elements.Add(element);
                    //element = null;
                }
                foreach (XmlElement item in elements)
                {
                    attribute = node.OwnerDocument.CreateAttribute(SegmentCount);
                    attribute.Value = elements.Count.ToString();
                    item.Attributes.Append(attribute);
                    yield return new XmlFileOfVSLine(item, segmentIndex, elements.Count, parentIndex);
                }
            }
        }

        public IReadWriter<IWillTranslateFile> GetReadWriter()
        {
            return readWriter;
        }
    }
}
