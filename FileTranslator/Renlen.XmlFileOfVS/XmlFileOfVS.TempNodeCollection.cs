using System;
using System.Collections;
using System.Collections.Generic;

namespace Renlen.FileTranslator
{
    public partial class XmlFileOfVS
    {
        /// <summary>
        /// 表示节点
        /// </summary>
        private class TempNodeCollection : ICollection<TempNode>, IEnumerable<TempNode>
        {
            private const int TempNodeMax = 10;

            private TempNode[] nodes = new TempNode[TempNodeMax];
            private int head = -1;
            private int count = 0;
            private XmlFileOfVS xml;
            public TempNodeCollection(XmlFileOfVS file)
            {
                xml = file;
            }
            public TempNode this[string name]
            {
                get
                {
                    int index;
                    for (int i = 0; i < count; i++)
                    {
                        index = (head + TempNodeMax - i) % TempNodeMax;
                        if (nodes[index].Name == name)
                        {
                            return nodes[index];
                        }
                    }
                    TempNode tempNode = xml.GetTempNode(name);
                    if (tempNode != null)
                    {
                        Add(tempNode);
                    }
                    return tempNode;
                }
            }

            public int Count => count;

            public bool IsReadOnly => false;

            public void Add(TempNode item)
            {
                head++;
                head %= TempNodeMax;
                nodes[head] = item;
                count++;
                //缓存数量不能超过 TempNodeMax 个
                if (count > TempNodeMax)
                {
                    count = TempNodeMax;
                }
            }

            public void Clear()
            {
                nodes.Initialize();
                count = 0;
                head = -1;
            }

            public bool Contains(TempNode item)
            {
                int index;
                for (int i = 0; i < count; i++)
                {
                    index = (head + TempNodeMax - i) % TempNodeMax;
                    if (nodes[index].Name == item.Name)
                    {
                        return true;
                    }
                }
                return false;
            }

            public void CopyTo(TempNode[] array, int arrayIndex)
            {
                int index = arrayIndex;
                foreach (TempNode item in this)
                {
                    array[index++] = item;
                }
            }

            public IEnumerator<TempNode> GetEnumerator()
            {
                int index;
                for (int i = 0; i < count; i++)
                {
                    index = (head + TempNodeMax - i) % TempNodeMax;
                    yield return nodes[index];
                }
            }

            /// <summary>
            /// 始终抛出 <see cref="NotImplementedException"/> 异常
            /// </summary>
            /// <param name="item"></param>
            /// <returns></returns>
            public bool Remove(TempNode item)
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
