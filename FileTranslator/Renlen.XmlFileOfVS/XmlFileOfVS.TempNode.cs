using System.Collections.Generic;
using System.Xml;

namespace Renlen.FileTranslator
{
    public partial class XmlFileOfVS
    {
        /// <summary>
        /// 表示缓存的一个成员，通过索引器可以根据节点路径获取表示行的节点值
        /// </summary>
        private class TempNode
        {
            private readonly Dictionary<string, TempValue> temp = new Dictionary<string, TempValue>();
            public string Name { get; }
            public TempValue this[string path]
            {
                get => temp.TryGetValue(path, out TempValue value) ? value : null;
            }
            public TempNode(string name)
            {
                Name = name;
            }
            public void Add(string path, XmlNode node)
            {
                try
                {
                    temp.Add(path, new TempValue(node));
                }
                catch
                {
                }
            }
            public void Add(string path, XmlNode node, int start, int length)
            {
                temp.Add(path, new TempValue(node, start, length));
            }
        }
    }
}
