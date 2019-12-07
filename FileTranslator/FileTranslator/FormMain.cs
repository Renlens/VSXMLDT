#if DEV
using DevExpress.Utils;
using DevExpress.XtraGrid.Columns;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
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
            colFileSize.DisplayFormat.Format = FileSizeFormatter.Formatter;
            colTextLength.DisplayFormat.Format = FileSizeFormatter.Formatter;
            //
            // 设置 viewUpEditLanguage 的列
            //

#region 定义列
            GridColumn colLanguageName = new GridColumn
            {
                FieldName = "Name",
                Caption = "Name",
                VisibleIndex = 1,
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
            Files.AddRange(TestFile.CreateTestFiles(0).Select(file => new TranslatingFile(file)));
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
            XmlFileOfVS xml = new XmlFileOfVS(@"Renlen.FileTranslator.xml");
            TranslatingFile file = new TranslatingFile(xml);
            file.Statistics();
            file.SourceLanguage = Language.English;
            file.TargetLanguage = Language.Chinese;
            Files.Add(file);
        }

        private async void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                FileName = ""
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                XmlFileOfVS xml = new XmlFileOfVS(openFileDialog.FileName);
                Files.Add(new TranslatingFile(xml));
                await Files.Last().StatisticsAsync();
                gridControl1.RefreshDataSource();
            }
        }

        private void gridControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                menuMain.ShowPopup(MousePosition);
            }
        }
    }
}
#endif