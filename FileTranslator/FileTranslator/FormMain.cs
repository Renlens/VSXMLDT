using DevExpress.Utils;
using DevExpress.XtraGrid.Columns;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
            // 格式化器
            //
            colFileSize.DisplayFormat.Format = NumberFormatter.Formatter;
            colTextLength.DisplayFormat.Format = NumberFormatter.Formatter;
            //
            // 设置 viewUpEditLanguage 的列
            //

            #region 定义列
            GridColumn colLanguageValue = new GridColumn
            {
                FieldName = "Value",
                Caption = "Value",
                VisibleIndex = 0
            };
            GridColumn colLanguageName = new GridColumn
            {
                FieldName = "Name",
                Caption = "Name",
                VisibleIndex = 1
            };
            GridColumn colLanguageCaption = new GridColumn
            {
                FieldName = "Caption",
                Caption = "Caption",
                VisibleIndex = 2
            };
            #endregion

            viewUpEditLanguage.Columns.AddRange(new GridColumn[]
            {
                colLanguageValue,
                colLanguageName,
                colLanguageCaption
            });
        }
        private void SetDataSource()
        {
            upEditSourceLanguage.DataSource = Language.GetLanguages();
            upEditTargetLanguage.DataSource = Language.GetLanguages(LanguageFiter.RemoveAuto);
            gridControl1.DataSource = Files;
        }
        private void GetConfig()
        {

        }
        private void LoadFiles()
        {
            Files.AddRange(TestFile.CreateTestFiles(20).Select(file => new TranslatingFile(file)));
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

        private readonly List<TranslatingFile> Files = new List<TranslatingFile>();
        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeControls();
            GetConfig();
            SetDataSource();
            LoadFiles();
            RefreshFiles();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("在此停顿!");
        }
    }

}
