namespace Renlen.TranslateFile
{
    /// <summary>
    /// 表示翻译文件的一次翻译对象
    /// </summary>
    public interface ITranslatingLine
    {
        /// <summary>
        /// 即将要翻译的文本。
        /// </summary>
        string Text { get; }
        /// <summary>
        /// 结果代码
        /// </summary>
        int Code { get; set; }
        /// <summary>
        /// 翻译结果。
        /// </summary>
        string Result { get; set; }
        /// <summary>
        /// 翻译后调用此方法，用于提交结果
        /// </summary>
        void CommitResult();
    }
}
