using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static Renlen.FileTranslator.Global;
using Microsoft.VisualBasic.FileIO;
using Microsoft.VisualBasic;

namespace Renlen.FileTranslator
{
    public partial class FormMain2 : Form
    {
        public FormMain2()
        {
            InitializeComponent();
            gridView1.AutoGenerateColumns = false;
        }
        private void SetDataSource()
        {
            BindingFiles.DataSource = Files;
            gridView1.DataSource = BindingFiles;
            colSourceLanguage.DataSource = Language.GetLanguages();
            colSourceLanguage.DisplayMember = "SourceLanguage";
            colSourceLanguage.ValueMember = "This";
            colSourceLanguage.DisplayMember = "Caption";
            colTargetLanguage.DataSource = Language.GetLanguages(LanguageFiter.RemoveAuto);
            colTargetLanguage.DisplayMember = "TargetLanguage";
            colTargetLanguage.ValueMember = "This";
            colTargetLanguage.DisplayMember = "Caption";
            colFileSize.DefaultCellStyle.FormatProvider = FileSizeFormatter.Formatter;
            colFileSize.DefaultCellStyle.Format = "S5";
        }

        private void GetConfig()
        {

        }
        private void LoadFiles()
        {
            Files.AddRange(TestFile.CreateTestFiles(10).Select(file => new TranslatingFile(file)));
        }
        private void RefreshFiles()
        {
            Language[] languages = Language.GetLanguages();
            Files.ForEach(file =>
            {
                file.Statistics();
                file.Progress = GRandom.Next(0, 100);
                file.State = (TranslateState)GRandom.Next(0, 10);
                if (file.State == TranslateState.Completed)
                {
                    file.Progress = 100;
                }
                file.SourceLanguage = languages[GRandom.Next(0, languages.Length)];
                file.TargetLanguage = languages[GRandom.Next(2, languages.Length)];
                if (file.SourceLanguage == file.TargetLanguage)
                {
                    file.SourceLanguage = languages[1];
                }
            });
        }

        private readonly BindingSource BindingFiles = new BindingSource();
        private readonly List<TranslatingFile> Files = new List<TranslatingFile>();
        private void Form1_Load(object sender, EventArgs e)
        {
            GetConfig();
            SetDataSource();
            LoadFiles();
            RefreshFiles();
            BindingFiles.ResetBindings(false);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("在此停顿!");
        }

        private void GridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

            }
        }

        private void GridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (!e.FormattingApplied && e.CellStyle.FormatProvider is IFormatProvider provider)
            {
                object result = provider.GetFormat(typeof(ICustomFormatter));
                if (result != null && result is ICustomFormatter formatter)
                {
                    e.Value = formatter.Format(e.CellStyle.Format, e.Value, provider);
                    e.FormattingApplied = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
