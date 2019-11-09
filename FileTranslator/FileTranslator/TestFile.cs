using System.Collections.Generic;
using static Renlen.FileTranslator.Global;

namespace Renlen.FileTranslator
{
    class TestFile : WillTranslateFile
    {
        public static TestFile[] CreateTestFiles()
        {
            return CreateTestFiles(GRandom.Next(5, 50));
        }
        public static TestFile[] CreateTestFiles(int n)
        {
            if (n < 0) n = 10;
            int length = n.ToString().Length;
            TestFile[] files = new TestFile[n];
            for (int i = 0; i < n; i++)
            {
                files[i] = new TestFile($"TestFile{(i + 1).ToString().PadLeft(length, '0')}") { IsFile = true };
            }
            return files;
        }
        public TestFile(string path)
        {
            FullPath = path;
        }
        public override long GetFileSize()
        {
            return GRandom.Next();
        }

        public override IEnumerable<ITranslatingLine> GetTranslatingLines()
        {
            yield return new TranslatingLine("test");
            yield return new TranslatingLine("text");
            yield return new TranslatingLine("end");
        }
    }
}
