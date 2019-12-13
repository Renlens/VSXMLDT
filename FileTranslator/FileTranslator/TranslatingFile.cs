using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using Renlen.TranslateFile;

namespace Renlen.FileTranslator
{
    public class TranslatingFile
    {
        public IWillTranslateFile File { get; }
        public bool IsStatisticed { get; private set; }
        public string Name { get; private set; }
        public long FileSize { get; private set; } = -3;
        public long TextLength { get; private set; } = -3;
        public string DirectoryPath { get; private set; }
        public string FullPath { get; private set; }
        public Language SourceLanguage { get; set; } = Language.Default;
        public Language TargetLanguage { get; set; } = Language.Default;
        public string Output { get; set; } = "输出到 Output";
        public double Progress { get; set; }
        public TranslateState State { get; internal set; }
        private ConcurrentQueue<ITranslatingLine> Lines { get; set; } = new ConcurrentQueue<ITranslatingLine>();
        private int lineCount;
        public TranslatingFile(IWillTranslateFile file)
        {
            File = file;
            if (file.IsFile)
            {
                FullPath = Path.GetFullPath(file.FullPath);
            }
            else
            {
                FullPath = file.FullPath;
            }
            Name = Path.GetFileName(FullPath);
            DirectoryPath = Path.GetDirectoryName(FullPath);
        }
        public void Statistics()
        {
            if (IsStatisticed) return;
            FileSize = -2;// "Loading...";
            TextLength = -2;//"Loading...";
            FileSize = (long)File.GetFileSize();//.ToShow("S5");
            long textLength = 0;
            foreach (ITranslatingLine line in File.GetTranslatingLines())
            {
                if (string.IsNullOrWhiteSpace(line.Text))
                {
                    continue;
                }
                textLength += line.Text.Length;
                Lines.Enqueue(line);
            }
            TextLength = textLength;//.ToShow("Z5");
            lineCount = Lines.Count;
            IsStatisticed = true;
        }
        public async Task StatisticsAsync()
        {
            await Task.Run(Statistics);
        }

        public void StartTranslate()
        {
            Translate();
        }

        private void Translate()
        {
            while (Lines.Count > 0)
            {
                Lines.TryDequeue(out ITranslatingLine line);
                line.Result = BaiduTranslator.TestTranslation(line.Text);
                line.CommitResult();
            }
        }
    }
}
