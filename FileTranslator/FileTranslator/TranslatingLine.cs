namespace Renlen.FileTranslator
{
    public class TranslatingLine : ITranslatingLine
    {
        public string Text { get; protected set; }
        public string Result { get; set; }

        protected TranslatingLine()
        {

        }
        public TranslatingLine(string text)
        {
            Text = text;
        }

        public virtual void CommitResult()
        {

        }
    }
}
