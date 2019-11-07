namespace Renlen.FileTranslator
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFileSize = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTextLength = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDirectoryPath = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSourceLanguage = new DevExpress.XtraGrid.Columns.GridColumn();
            this.upEditSourceLanguage = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.viewUpEditLanguage = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colTargetLanguage = new DevExpress.XtraGrid.Columns.GridColumn();
            this.upEditTargetLanguage = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.colOutput = new DevExpress.XtraGrid.Columns.GridColumn();
            this.upEditOutput = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.viewUpEditOutput = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colProgress = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemProgressBar1 = new DevExpress.XtraEditors.Repository.RepositoryItemProgressBar();
            this.colState = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOperation = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.colTargetLangu = new DevExpress.XtraGrid.Columns.GridColumn();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.menuMain = new DevExpress.XtraBars.PopupMenu(this.components);
            this.btnAdd = new DevExpress.XtraBars.BarButtonItem();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upEditSourceLanguage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewUpEditLanguage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upEditTargetLanguage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upEditOutput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewUpEditOutput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemProgressBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gridControl1.Location = new System.Drawing.Point(16, 55);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(4);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.upEditSourceLanguage,
            this.upEditOutput,
            this.repositoryItemProgressBar1,
            this.repositoryItemButtonEdit1,
            this.upEditTargetLanguage});
            this.gridControl1.Size = new System.Drawing.Size(1466, 618);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            this.gridControl1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.gridControl1_MouseClick);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colName,
            this.colFileSize,
            this.colTextLength,
            this.colDirectoryPath,
            this.colSourceLanguage,
            this.colTargetLanguage,
            this.colOutput,
            this.colProgress,
            this.colState,
            this.colOperation});
            this.gridView1.DetailHeight = 437;
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // colName
            // 
            this.colName.Caption = "File Name";
            this.colName.FieldName = "Name";
            this.colName.MinWidth = 27;
            this.colName.Name = "colName";
            this.colName.OptionsColumn.AllowEdit = false;
            this.colName.OptionsColumn.ReadOnly = true;
            this.colName.OptionsFilter.AllowAutoFilter = false;
            this.colName.Visible = true;
            this.colName.VisibleIndex = 0;
            this.colName.Width = 125;
            // 
            // colFileSize
            // 
            this.colFileSize.Caption = "File Size";
            this.colFileSize.DisplayFormat.FormatString = "S5";
            this.colFileSize.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.colFileSize.FieldName = "FileSize";
            this.colFileSize.MinWidth = 27;
            this.colFileSize.Name = "colFileSize";
            this.colFileSize.OptionsColumn.AllowEdit = false;
            this.colFileSize.OptionsColumn.ReadOnly = true;
            this.colFileSize.OptionsFilter.AllowAutoFilter = false;
            this.colFileSize.Visible = true;
            this.colFileSize.VisibleIndex = 1;
            this.colFileSize.Width = 115;
            // 
            // colTextLength
            // 
            this.colTextLength.Caption = "Text Length";
            this.colTextLength.DisplayFormat.FormatString = "Z5";
            this.colTextLength.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.colTextLength.FieldName = "TextLength";
            this.colTextLength.MinWidth = 27;
            this.colTextLength.Name = "colTextLength";
            this.colTextLength.OptionsColumn.AllowEdit = false;
            this.colTextLength.OptionsColumn.ReadOnly = true;
            this.colTextLength.OptionsFilter.AllowAutoFilter = false;
            this.colTextLength.Visible = true;
            this.colTextLength.VisibleIndex = 2;
            this.colTextLength.Width = 115;
            // 
            // colDirectoryPath
            // 
            this.colDirectoryPath.Caption = "Directory Path";
            this.colDirectoryPath.FieldName = "DirectoryPath";
            this.colDirectoryPath.MinWidth = 27;
            this.colDirectoryPath.Name = "colDirectoryPath";
            this.colDirectoryPath.OptionsColumn.AllowEdit = false;
            this.colDirectoryPath.OptionsColumn.ReadOnly = true;
            this.colDirectoryPath.OptionsFilter.AllowAutoFilter = false;
            this.colDirectoryPath.Visible = true;
            this.colDirectoryPath.VisibleIndex = 3;
            this.colDirectoryPath.Width = 113;
            // 
            // colSourceLanguage
            // 
            this.colSourceLanguage.Caption = "Source Language";
            this.colSourceLanguage.ColumnEdit = this.upEditSourceLanguage;
            this.colSourceLanguage.FieldName = "SourceLanguage";
            this.colSourceLanguage.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.colSourceLanguage.MinWidth = 27;
            this.colSourceLanguage.Name = "colSourceLanguage";
            this.colSourceLanguage.Visible = true;
            this.colSourceLanguage.VisibleIndex = 4;
            this.colSourceLanguage.Width = 188;
            // 
            // upEditSourceLanguage
            // 
            this.upEditSourceLanguage.AutoHeight = false;
            this.upEditSourceLanguage.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.upEditSourceLanguage.DisplayMember = "This";
            this.upEditSourceLanguage.Name = "upEditSourceLanguage";
            this.upEditSourceLanguage.NullText = "Null";
            this.upEditSourceLanguage.PopupView = this.viewUpEditLanguage;
            this.upEditSourceLanguage.ValueMember = "This";
            // 
            // viewUpEditLanguage
            // 
            this.viewUpEditLanguage.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.viewUpEditLanguage.Name = "viewUpEditLanguage";
            this.viewUpEditLanguage.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.viewUpEditLanguage.OptionsView.ShowGroupPanel = false;
            // 
            // colTargetLanguage
            // 
            this.colTargetLanguage.Caption = "Target Language";
            this.colTargetLanguage.ColumnEdit = this.upEditTargetLanguage;
            this.colTargetLanguage.FieldName = "TargetLanguage";
            this.colTargetLanguage.MinWidth = 27;
            this.colTargetLanguage.Name = "colTargetLanguage";
            this.colTargetLanguage.Visible = true;
            this.colTargetLanguage.VisibleIndex = 5;
            this.colTargetLanguage.Width = 187;
            // 
            // upEditTargetLanguage
            // 
            this.upEditTargetLanguage.AutoHeight = false;
            this.upEditTargetLanguage.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.upEditTargetLanguage.Name = "upEditTargetLanguage";
            this.upEditTargetLanguage.NullText = "Null";
            this.upEditTargetLanguage.PopupView = this.viewUpEditLanguage;
            // 
            // colOutput
            // 
            this.colOutput.Caption = "Output";
            this.colOutput.ColumnEdit = this.upEditOutput;
            this.colOutput.FieldName = "Output";
            this.colOutput.MinWidth = 27;
            this.colOutput.Name = "colOutput";
            this.colOutput.Visible = true;
            this.colOutput.VisibleIndex = 6;
            this.colOutput.Width = 157;
            // 
            // upEditOutput
            // 
            this.upEditOutput.AutoHeight = false;
            this.upEditOutput.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.upEditOutput.Name = "upEditOutput";
            this.upEditOutput.NullText = "Null";
            this.upEditOutput.PopupView = this.viewUpEditOutput;
            // 
            // viewUpEditOutput
            // 
            this.viewUpEditOutput.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.viewUpEditOutput.Name = "viewUpEditOutput";
            this.viewUpEditOutput.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.viewUpEditOutput.OptionsView.ShowGroupPanel = false;
            // 
            // colProgress
            // 
            this.colProgress.Caption = "Progress";
            this.colProgress.ColumnEdit = this.repositoryItemProgressBar1;
            this.colProgress.FieldName = "Progress";
            this.colProgress.MinWidth = 27;
            this.colProgress.Name = "colProgress";
            this.colProgress.Visible = true;
            this.colProgress.VisibleIndex = 7;
            this.colProgress.Width = 230;
            // 
            // repositoryItemProgressBar1
            // 
            this.repositoryItemProgressBar1.Name = "repositoryItemProgressBar1";
            // 
            // colState
            // 
            this.colState.Caption = "State";
            this.colState.FieldName = "State";
            this.colState.MinWidth = 27;
            this.colState.Name = "colState";
            this.colState.Visible = true;
            this.colState.VisibleIndex = 8;
            this.colState.Width = 100;
            // 
            // colOperation
            // 
            this.colOperation.Caption = "Operation";
            this.colOperation.ColumnEdit = this.repositoryItemButtonEdit1;
            this.colOperation.MinWidth = 27;
            this.colOperation.Name = "colOperation";
            this.colOperation.Visible = true;
            this.colOperation.VisibleIndex = 9;
            this.colOperation.Width = 200;
            // 
            // repositoryItemButtonEdit1
            // 
            this.repositoryItemButtonEdit1.AutoHeight = false;
            this.repositoryItemButtonEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
            // 
            // colTargetLangu
            // 
            this.colTargetLangu.Caption = "gridColumn6";
            this.colTargetLangu.ColumnEdit = this.upEditSourceLanguage;
            this.colTargetLangu.Name = "colTargetLangu";
            this.colTargetLangu.Visible = true;
            this.colTargetLangu.VisibleIndex = 5;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(1362, 12);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(81, 29);
            this.simpleButton1.TabIndex = 1;
            this.simpleButton1.Text = "Test";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // menuMain
            // 
            this.menuMain.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnAdd)});
            this.menuMain.Manager = this.barManager1;
            this.menuMain.Name = "menuMain";
            // 
            // btnAdd
            // 
            this.btnAdd.Caption = "&Add";
            this.btnAdd.Id = 0;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAdd_ItemClick);
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnAdd});
            this.barManager1.MaxItemId = 1;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(1495, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 688);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1495, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 688);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1495, 0);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 688);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1495, 688);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "File Translator";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upEditSourceLanguage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewUpEditLanguage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upEditTargetLanguage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upEditOutput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewUpEditOutput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemProgressBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colName;
        private DevExpress.XtraGrid.Columns.GridColumn colFileSize;
        private DevExpress.XtraGrid.Columns.GridColumn colTextLength;
        private DevExpress.XtraGrid.Columns.GridColumn colDirectoryPath;
        private DevExpress.XtraGrid.Columns.GridColumn colSourceLanguage;
        private DevExpress.XtraGrid.Columns.GridColumn colTargetLanguage;
        private DevExpress.XtraGrid.Columns.GridColumn colOutput;
        private DevExpress.XtraGrid.Columns.GridColumn colProgress;
        private DevExpress.XtraGrid.Columns.GridColumn colState;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit upEditSourceLanguage;
        private DevExpress.XtraGrid.Views.Grid.GridView viewUpEditLanguage;
        private DevExpress.XtraGrid.Columns.GridColumn colTargetLangu;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit upEditOutput;
        private DevExpress.XtraGrid.Views.Grid.GridView viewUpEditOutput;
        private DevExpress.XtraEditors.Repository.RepositoryItemProgressBar repositoryItemProgressBar1;
        private DevExpress.XtraGrid.Columns.GridColumn colOperation;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit upEditTargetLanguage;
        private DevExpress.XtraBars.PopupMenu menuMain;
        private DevExpress.XtraBars.BarButtonItem btnAdd;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
    }
}

