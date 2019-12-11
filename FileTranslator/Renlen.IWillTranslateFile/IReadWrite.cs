namespace Renlen.TranslateFile
{
    /// <summary>
    /// 读写接口，表示此类支持到流的自定义读写
    /// </summary>
    /// <typeparam name="T">读写类型，通常是此类</typeparam>
    public interface IReadWrite<T>
    {
        /// <summary>
        /// 获取与此类关联的读写器
        /// </summary>
        /// <returns></returns>
        IReadWriter<T> GetReadWriter();
    }
}
