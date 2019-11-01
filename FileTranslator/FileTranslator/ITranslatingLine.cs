namespace Renlen.FileTranslator
{
    public interface ITranslatingLine
    {
        string Text { get; }
        string Result { get; set; }
        void CommitResult();
    }
}
