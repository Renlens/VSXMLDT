using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renlen.FileTranslator
{
    internal static class Global
    {
        /// <summary>
        /// 全局唯一随机函数（项目内）
        /// </summary>
        public static Random GRandom { get; } = new Random();
    }
}
