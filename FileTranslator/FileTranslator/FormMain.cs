using DevExpress.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Renlen.FileTranslator.Global;

namespace Renlen.FileTranslator
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }
        private void InitializeControls()
        {
            //
            // colFileSize
            //
            colFileSize.DisplayFormat.Format = NumberFormatter.Formatter;
            //
            // colTextLength
            //
            colTextLength.DisplayFormat.Format = NumberFormatter.Formatter;
        }
        private void SetDataSource()
        {
            //
            // upEditLanguage
            //
            upEditLanguage.DataSource = Language.GetLanguagesAnyDefault();
            //
            // gridControl1
            //
            gridControl1.DataSource = Files;
        }
        private void GetConfig()
        {

        }
        private void LoadFiles()
        {
            Files.AddRange(TestFile.CreateTestFiles(100).Select(file => new TranslatingFile(file)));
        }
        private void RefreshFiles()
        {
            Files.ForEach(file =>
            {
                file.Statistics();
                file.Progress = GRandom.Next(0, 100);
                file.State = (TranslateState)GRandom.Next(0, 10);
                if (file.State == TranslateState.Completed)
                {
                    file.Progress = 100;
                }
            });
        }

        private readonly List<TranslatingFile> Files = new List<TranslatingFile>();
        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeControls();
            SetDataSource();
            GetConfig();
            LoadFiles();
            RefreshFiles();
        }
    }


    class TestFile : WillTranslateFile
    {
        public static TestFile[] CreateTestFiles()
        {
            return CreateTestFiles(GRandom.Next(5, 50));
        }
        public static TestFile[] CreateTestFiles(int n)
        {
            if (n <= 0) n = 10;
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
