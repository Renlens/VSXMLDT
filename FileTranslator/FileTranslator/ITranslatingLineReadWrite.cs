using System;

namespace Renlen.FileTranslator
{
    /// <summary>
    /// 保存或读取翻译的行数据。如果待翻译文件支持保存进度下次继续翻译，则翻译的行必须实现此接口。
    /// </summary>
    public interface ITranslatingLineReadWrite : IReadWrite<ITranslatingLine>
    {

    }
}
