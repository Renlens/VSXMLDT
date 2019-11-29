namespace Renlen.FileTranslator
{
    /// <summary>
    /// 保存或读取待翻译翻译的文件。如果待翻译文件支持保存进度下次继续翻译，则待翻译文件必须实现此接口。
    /// </summary>
    public interface IWillTranslateFileReadWrite : IReadWrite<IWillTranslateFile>
    {

    }
}
