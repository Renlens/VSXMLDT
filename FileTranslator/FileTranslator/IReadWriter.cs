using System.IO;

namespace Renlen.FileTranslator
{
    /// <summary>
    /// 指定类型读写到指定流的读写器实现
    /// </summary>
    /// <typeparam name="T">要读写对象的类型</typeparam>
    public interface IReadWriter<T>
    {
        /// <summary>
        /// 将一个 <typeparamref name="T"/> 类型的数据写入到指定流的当前位置，并将位置移动到写入后的末尾。
        /// </summary>
        /// <param name="stream">要写入的流</param>
        /// <param name="o">要写入流的对象</param>
        void Write(Stream stream, T o);
        /// <summary>
        /// 从指定流的当前的位置读取一个 <typeparamref name="T"/> 类型的对象，并将位置移动到读取后的位置。
        /// </summary>
        /// <param name="stream">要读取的流</param>
        /// <returns>从流中读取的对象</returns>
        T Read(Stream stream);
    }
}
