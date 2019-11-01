namespace Renlen.FileTranslator
{
    public enum TranslateState
    {
        Create,
        Ready, 
        Statistics,
        Waiting,
        Translating,
        Pause,
        Completed,
        Interruption,
        Error,
        Cancel
    }
}
