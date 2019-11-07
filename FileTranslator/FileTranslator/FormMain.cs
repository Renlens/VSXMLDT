using DevExpress.Utils;
using DevExpress.XtraGrid.Columns;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
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
            Files.AddRange(TestFile.CreateTestFiles(30).Select(file => new TranslatingFile(file)));
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
            XmlFileOfVS xml = Files.Last().File as XmlFileOfVS;
            foreach (var item in xml.xml.Descendants("member"))
            {
                var v1 = item.Elements();
                var v2 = item.Descendants();
                var v3 = item.Nodes();
                var v4 = item.Value;
                var v41 = v1.First().Elements();
                var v5 = item.ToString();
                item.SetValue("翻译结果");
                using MemoryStream stream = new MemoryStream();
                item.Save(stream);
                stream.Position = 0;
                using StreamReader reader = new StreamReader(stream);
                var v6 = reader.ReadToEnd();
            }
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                FileName = ""
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                XmlFileOfVS xml = new XmlFileOfVS(openFileDialog.FileName);
                Files.Add(new TranslatingFile(xml));
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

    public class XmlFileOfVS : IWillTranslateFile, IWillTranslateFileReadWrite
    {
        private readonly ReadWriter readWriter = new ReadWriter();
        private long fileSize = (long)FileSize.Uninit;
        internal XmlDocument xml = new XmlDocument();

        public bool IsPause => true;
        public bool IsContinuous => false;
        public bool IsFile { get; private set; }
        public string FullPath { get; private set; }

        public XmlFileOfVS()
        {
            fileSize = (long)FileSize.Uninit;
            IsFile = false;
            FullPath = "";
        }
        public XmlFileOfVS(string file)
        {
            IsFile = true;
            FullPath = Path.GetFullPath(file);
            using FileStream stream = new FileStream(FullPath, FileMode.OpenOrCreate, FileAccess.Read);
            xml.Load(stream);
            fileSize = stream.Length;
        }
        public XmlFileOfVS(Stream stream)
        {
            IsFile = false;
            xml.Load(stream);
            fileSize = stream.Length;
        }

        public long GetFileSize()
        {
            return fileSize;
        }

        public IEnumerable<ITranslatingLine> GetTranslatingLines()
        {
            XmlFileOfVSLine line;
            XmlNodeList members = xml.GetElementsByTagName("member");
            for (int i = 0; i < members.Count; i++)
            {
                if (members[i].NodeType == XmlNodeType.Element)
                {

                }

            }
            yield break;
        }

        public IReadWriter<IWillTranslateFile> GetReadWriter()
        {
            return readWriter;
        }

        private class ReadWriter : IReadWriter<IWillTranslateFile>
        {
            public IWillTranslateFile Read(Stream stream)
            {
                return new XmlFileOfVS(stream);
            }

            public void Write(Stream stream, IWillTranslateFile o)
            {
                (o as XmlFileOfVS).xml.Save(stream);
            }
        }
    }

    public class XmlFileOfVSLine : ITranslatingLine, ITranslatingLineReadWrite
    {
        private XElement element;
        public ResultCode Code { get; set; }
        public string Text { get; set; }
        public string Result { get; set; }

        public XmlFileOfVSLine(XElement element)
        {
            this.element = element;
        }

        public void CommitResult()
        {

        }


        public IReadWriter<ITranslatingLine> GetReadWriter()
        {
            throw new NotImplementedException();
        }

        private class ReadWriter : IReadWriter<ITranslatingLine>
        {
            public ITranslatingLine Read(Stream stream)
            {
                throw new NotImplementedException();
            }

            public void Write(Stream stream, ITranslatingLine o)
            {
                throw new NotImplementedException();
            }
        }
    }

    public interface IWillTranslateFileReadWrite : IReadWrite<IWillTranslateFile>
    {

    }
    public interface ITranslatingLineReadWrite : IReadWrite<ITranslatingLine>
    {

    }
}
