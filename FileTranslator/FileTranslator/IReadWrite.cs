namespace Renlen.FileTranslator
{
    public interface IReadWrite<T>
    {
        IReadWriter<T> GetReadWriter();
    }
}
