#if DEV
using DevExpress.Utils;
using DevExpress.XtraGrid.Columns;
using Renlen.TranslateFile;
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

        private OpenFileDialog addFileConrtol;
        private bool flagUpdate = false;
        private bool flagFilter = false;
        private bool SelectFile()
        {
            if (FileTypes.Count == 0)
            {
                MessageBox.Show("未找到扩展类型，请至少成功加载一个扩展类型后再使用。", "消息");
                return false;
            }
            if (addFileConrtol == null)
            {
                addFileConrtol = new OpenFileDialog()
                {
                    FileName = "",
                    Multiselect = true
                };
                flagFilter = true;
            }
            if (flagFilter)
            {
                StringBuilder fileFilter = new StringBuilder("所有文件|*.*");
                foreach (TypeRef type in FileTypes)
                {
                    fileFilter.Append("|");
                    fileFilter.Append(type.FileFilter);
                }
                addFileConrtol.Filter = fileFilter.ToString();
                flagFilter = false;
            }
            return addFileConrtol.ShowDialog() == DialogResult.OK;
        }

        #region Init
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
        private void InitializeConfig()
        {
            UpdateSpen = 0;
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
        #endregion

        private readonly List<TranslatingFile> Files = new List<TranslatingFile>();
        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeControls();
            SetDataSource();
            GetConfig();
            InitializeConfig();
            LoadFiles();
            RefreshFiles();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("在此停顿!");
        }

        private async void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (SelectFile())
            {
                if (addFileConrtol.FileNames.Length > 0)
                {
                    int index = 0;
                    if (addFileConrtol.FilterIndex > 1)
                    {
                        index = addFileConrtol.FilterIndex - 2;
                    }
                    List<Task> tasks = new List<Task>(addFileConrtol.FileNames.Length);
                    foreach (string path in addFileConrtol.FileNames)
                    {
                        IWillTranslateFile file = FileTypes[index].Create(path);
                        if (file == null)
                        {
                            continue;
                        }
                        Files.Add(new TranslatingFile(file));
                        OnUpdateData();
                        tasks.Add(Files.Last().StatisticsAsync());
                    }
                    if (tasks.Count > 0)
                    {
                        await Task.Factory.ContinueWhenAny(tasks.ToArray(), task => OnUpdateData());
                        await Task.WhenAll(tasks);
                        OnUpdateData(true);
                    }
                }
            }
        }
        private async void btnStart_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TranslateState[] end = {
                 TranslateState.Cancel,
                 TranslateState.Completed,
                 TranslateState.Error,
                 TranslateState.Pause
            };
            await Files.FirstOrDefault(file => !end.Contains(file.State))?.StartTranslateAsync();
        }

        private void gridControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                menuMain.ShowPopup(MousePosition);
            }
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            if (flagUpdate)
            {
                gridControl1.RefreshDataSource();
                flagUpdate = false;
            }
        }

        public void OnUpdateData(bool isnow = false)
        {
            Log($"请求更新数据: {isnow}");
            if (isnow || UpdateSpen == 0)
            {
                gridControl1.RefreshDataSource();
                flagUpdate = false;
            }
            else
            {
                flagUpdate = true;
            }
        }
        public void OnUpdateFilter()
        {
            flagFilter = true;
        }
        public static void Log(string msg)
        {
#if DEBUG
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} - DEBUG - {msg}");
#endif
        }
    }
}
#endif