using System;

namespace Renlen.FileTranslator
{
    public partial class XmlFileOfVS
    {
        [Flags]
        private enum ElementType
        {
            c = 0b_00000000_00000000_00000001,
            para = 0b_00000000_00000000_00000010,
            see = 0b_00000000_00000000_00000100,
            code = 0b_00000000_00000000_00001000,
            param = 0b_00000000_00000000_00010000,
            seealso = 0b_00000000_00000000_00100000,
            example = 0b_00000000_00000000_01000000,
            paramref = 0b_00000000_00000000_10000000,
            summary = 0b_00000000_00000001_00000000,
            exception = 0b_00000000_00000010_00000000,
            permission = 0b_00000000_00000100_00000000,
            typeparam = 0b_00000000_00001000_00000000,
            include = 0b_00000000_00010000_00000000,
            remarks = 0b_00000000_00100000_00000000,
            typeparamref = 0b_00000000_01000000_00000000,
            list = 0b_00000000_10000000_00000000,
            returns = 0b_00000001_00000000_00000000,
            value = 0b_00000010_00000000_00000000,
            /// <summary>
            /// 表示不是一个元素，而是作为一个类别
            /// </summary>
            NoElement = 0b_10000000_00000000_00000000,
            /// <summary>
            /// 表示此元素应插入到文本中翻译
            /// </summary>
            Insert = NoElement | c | see | code | seealso | example | paramref | permission | typeparamref,
            /// <summary>
            /// 表示此元素应单独翻译
            /// </summary>
            Separate = NoElement | para | param | summary | exception | typeparam | remarks | returns | value,
            /// <summary>
            /// 表示此元素应保持原样
            /// </summary>
            Skip = NoElement | include,
            /// <summary>
            /// 表示此元素应单独处理，并将前后元素分开
            /// </summary>
            Breakpoint = NoElement | Separate | Skip,
            TrimSpeac = NoElement | 0
        }
    }
}
