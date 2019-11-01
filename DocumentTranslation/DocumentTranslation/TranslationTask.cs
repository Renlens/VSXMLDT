using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentTranslation
{
    internal class TranslationTask
    {
        private int size = -1;
        public VSXMLData XML { get; set; }
        public int Size
        {
            get
            {
                if (size == -1)
                {
                    size = XML.GetSize();
                }
                return size;
            }
        }
    }
}
