using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace Renlen.FileTranslator
{
    public partial class XmlFileOfVS : IWillTranslateFile, IWillTranslateFileReadWrite
    {
        private static readonly ReadWriter readWriter = new ReadWriter();

        private FileSize fileSize = FileSize.Uninit;
        internal readonly XmlDocument xml = new XmlDocument();

        /// <summary>
        /// 获取指示此文件是否可以保存，下次继续翻译的值。始终为 True 。
        /// </summary>
        public bool IsPause => true;
        /// <summary>
        /// 获取指示此文件是否必须按固定顺序提交翻译结果的值。始终为 False 。
        /// </summary>
        public bool IsContinuous => false;
        /// <summary>
        /// 获取指示此文件的源是否是一个磁盘中的文件的值。
        /// </summary>
        public bool IsFile { get; private set; }
        /// <summary>
        /// 如果此文件是一个硬盘的文件，则此属性返回文件全路径。
        /// </summary>
        public string FullPath { get; private set; }

        /// <summary>
        /// 初始化一个新实例。
        /// </summary>
        private XmlFileOfVS()
        {
            fileSize = FileSize.Uninit;
            IsFile = false;
            FullPath = "";
        }
        /// <summary>
        /// 使用指定的 XML 文件初始化一个 <see cref="XmlFileOfVS"/> 对象。
        /// </summary>
        /// <param name="file"></param>
        public XmlFileOfVS(string file)
        {
            IsFile = true;
            FullPath = Path.GetFullPath(file);
            using FileStream stream = new FileStream(FullPath, FileMode.OpenOrCreate, FileAccess.Read);
            xml.Load(stream);
            fileSize = (FileSize)stream.Length;
            //Load(stream);
        }
        /// <summary>
        /// 从指定的流的当前位置加载一个 <see cref="XmlFileOfVS"/> 对象。
        /// </summary>
        /// <param name="stream"></param>
        private XmlFileOfVS(Stream stream)
        {
            Load(stream);
        }

        /// <summary>
        /// 从指定的流的当前位置加载数据。
        /// </summary>
        /// <param name="stream"></param>
        private void Load(Stream stream)
        {
            using BinaryReader br = new BinaryReader(stream, Encoding.UTF8, true);
            FullPath = br.ReadString();
            IsFile = !string.IsNullOrWhiteSpace(FullPath);
            fileSize = (FileSize)br.ReadInt64();
            int length = br.ReadInt32();
            if (length == 0)
            {
                fileSize = 0;
            }
            else
            {
                byte[] date = new byte[length];
                stream.Read(date, 0, date.Length);
                using MemoryStream xmlStream = new MemoryStream(date);
                xml.Load(xmlStream);
                fileSize = (FileSize)stream.Length;
            }
            bool isnull = br.ReadBoolean();
            if (isnull)
            {
                lines = null;
            }
            else
            {
                length = br.ReadInt32();
                lines = new List<ITranslatingLine>(length);
                IReadWriter<ITranslatingLine> readWriter = XmlFileOfVSLine.LineReadWriter;
                for (int i = 0; i < length; i++)
                {
                    lines.Add(readWriter.Read(stream));
                }
            }
        }
        private void Save(Stream stream)
        {
            using BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8, true);
            bw.Write(FullPath ?? "");
            bw.Write((long)fileSize);
            //保存xml文件
            bw.Write(0);
            long start = stream.Position;
            xml.Save(stream);
            long end = stream.Position;
            int length = Convert.ToInt32(end - start);
            if (length > 0)
            {
                stream.Position = start - 4;
                bw.Write(length);
                stream.Position = end;
            }
            bool isnull = lines == null;
            bw.Write(isnull);
            if (!isnull)
            {
                bw.Write(lines.Count);
                IReadWriter<ITranslatingLine> readWriter = XmlFileOfVSLine.LineReadWriter;
                foreach (ITranslatingLine line in lines)
                {
                    readWriter.Write(stream, line);
                }
            }
            bw.Flush();
        }

        /// <summary>
        /// 获取相关联的 XML 文件的大小
        /// </summary>
        /// <returns></returns>
        public long GetFileSize()
        {
            return (long)fileSize;
        }

        private List<ITranslatingLine> lines;

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
            if (lines != null)
            {
                foreach (ITranslatingLine line in lines)
                {
                    yield return line;
                }
                yield break;
            }
            XmlNodeList members = xml.GetElementsByTagName("member");
            lines = new List<ITranslatingLine>(Math.Max(4, members.Count));
            foreach (XmlNode member in members)
            {
                string memberName = GetMemberName(member);
                if (string.IsNullOrWhiteSpace(memberName))
                {
                    continue;
                }
                foreach (XmlNode node in member.ChildNodes)
                {
                    foreach (ITranslatingLine line in Analysis(node, memberName, node.Name))
                    {
                        lines.Add(line);
                        yield return line;
                    }
                }
            }
            lines.TrimExcess();
        }

        /// <summary>
        /// 分析节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="memberName"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private IEnumerable<ITranslatingLine> Analysis(XmlNode node, string memberName, string path)
        {
            if (node.ChildNodes.Count == 0)
            {
                yield break;
            }
            else if (node.ChildNodes.Count == 1)
            {
                string name = GetMemberName(node.ParentNode);
                if (name == null)
                {
                    yield break;
                }
                XmlNode line = node.FirstChild;
                if (line.NodeType == XmlNodeType.Text)
                {
                    yield return new XmlFileOfVSLine(this, line.Value, memberName, path, 0, LineType.Text);
                }
                else if (Enum.TryParse(line.Name, out ElementType type))
                {
                    if (ElementType.Skip.HasFlag(type) || ElementType.Insert.HasFlag(type))
                    {
                        yield break;
                    }
                    else if (ElementType.Separate.HasFlag(type))
                    {
                        foreach (ITranslatingLine item in Analysis(line, memberName, Combine(path, 0)))
                        {
                            yield return item;
                        }
                    }
                }
            }
            else
            {
                int index = 0;
                StringBuilder textBuilder = new StringBuilder();
                bool check = false;
                string nodeValue, nodeCheck;
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    if (node.ChildNodes[i].NodeType == XmlNodeType.Text)
                    {
                        nodeValue = node.ChildNodes[i].Value;
                        nodeCheck = nodeValue;
                        textBuilder.Append(node.ChildNodes[i].Value);
                        check = check || !string.IsNullOrWhiteSpace(nodeCheck);
                    }
                    else if (Enum.TryParse(node.ChildNodes[i].Name, out ElementType type))
                    {
                        if (ElementType.Insert.HasFlag(type))
                        {
                            nodeValue = node.ChildNodes[i].OuterXml;
                            nodeCheck = node.ChildNodes[i].InnerText;
                            textBuilder.Append(nodeValue);
                            check = check || !string.IsNullOrWhiteSpace(nodeCheck);
                        }
                        else if (ElementType.Skip.HasFlag(type))
                        {
                            if (check)
                            {
                                yield return new XmlFileOfVSLine(this, textBuilder.ToString(), memberName, path, index++, LineType.Element);
                            }
                            textBuilder.Clear();
                            check = false;
                        }
                        else if (ElementType.Separate.HasFlag(type))
                        {
                            if (check)
                            {
                                yield return new XmlFileOfVSLine(this, textBuilder.ToString(), memberName, path, index++, LineType.Element);
                            }
                            textBuilder.Clear();
                            check = false;
                            foreach (ITranslatingLine line in Analysis(node.ChildNodes[i], memberName, Combine(path, index)))
                            {
                                yield return line;
                            }
                        }
                    }
                }
            }
        }

        private string GetMemberName(XmlNode node)
        {
            return node?.Attributes["name"]?.Value;
        }

        private string Combine(string path, int index)
        {
            return $"{path}/{index}";
        }

        //private string GetNodePath(XmlNode node)
        //{
        //    Stack<string> nodeName = new Stack<string>();
        //    while (node.Name != "member")
        //    {
        //        nodeName.Push(node.Name);
        //        node = node.ParentNode;
        //    }
        //    string name = nodeName.Pop();
        //    StringBuilder path = new StringBuilder(name);
        //    while (nodeName.Count > 0)
        //    {
        //        name = nodeName.Pop();
        //        path.Append("/");
        //        path.Append(name);
        //    }
        //    return path.ToString();
        //}

        /// <summary>
        /// 获取可以读写此对象的读写器。
        /// </summary>
        /// <returns></returns>
        public IReadWriter<IWillTranslateFile> GetReadWriter()
        {
            return readWriter;
        }

    }
}
