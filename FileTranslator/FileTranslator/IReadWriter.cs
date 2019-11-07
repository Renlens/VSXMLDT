using System.IO;

namespace Renlen.FileTranslator
{
    public interface IReadWriter<T>
    {
        void Write(Stream stream, T o);
        T Read(Stream stream);
    }
}
