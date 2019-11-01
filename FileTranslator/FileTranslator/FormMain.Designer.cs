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
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFileSize = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTextLength = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFullPath = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSourceLanguage = new DevExpress.XtraGrid.Columns.GridColumn();
            this.upEditLanguage = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.viewUpEditLanguage = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colTargetLanguage = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOutput = new DevExpress.XtraGrid.Columns.GridColumn();
            this.upEditOutput = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.repositoryItemGridLookUpEdit2View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colProgress = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemProgressBar1 = new DevExpress.XtraEditors.Repository.RepositoryItemProgressBar();
            this.colState = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOperation = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.colTargetLangu = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upEditLanguage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewUpEditLanguage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upEditOutput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit2View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemProgressBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(12, 12);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.upEditLanguage,
            this.upEditOutput,
            this.repositoryItemProgressBar1,
            this.repositoryItemButtonEdit1});
            this.gridControl1.Size = new System.Drawing.Size(1207, 614);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colName,
            this.colFileSize,
            this.colTextLength,
            this.colFullPath,
            this.colSourceLanguage,
            this.colTargetLanguage,
            this.colOutput,
            this.colProgress,
            this.colState,
            this.colOperation});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // colName
            // 
            this.colName.Caption = "File Name";
            this.colName.FieldName = "Name";
            this.colName.Name = "colName";
            this.colName.OptionsColumn.AllowEdit = false;
            this.colName.OptionsColumn.ReadOnly = true;
            this.colName.OptionsFilter.AllowAutoFilter = false;
            this.colName.Visible = true;
            this.colName.VisibleIndex = 0;
            this.colName.Width = 90;
            // 
            // colFileSize
            // 
            this.colFileSize.Caption = "File Size";
            this.colFileSize.DisplayFormat.FormatString = "S5";
            this.colFileSize.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.colFileSize.FieldName = "FileSize";
            this.colFileSize.Name = "colFileSize";
            this.colFileSize.OptionsColumn.AllowEdit = false;
            this.colFileSize.OptionsColumn.ReadOnly = true;
            this.colFileSize.OptionsFilter.AllowAutoFilter = false;
            this.colFileSize.Visible = true;
            this.colFileSize.VisibleIndex = 1;
            this.colFileSize.Width = 83;
            // 
            // colTextLength
            // 
            this.colTextLength.Caption = "Text Length";
            this.colTextLength.DisplayFormat.FormatString = "Z5";
            this.colTextLength.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.colTextLength.FieldName = "TextLength";
            this.colTextLength.Name = "colTextLength";
            this.colTextLength.OptionsColumn.AllowEdit = false;
            this.colTextLength.OptionsColumn.ReadOnly = true;
            this.colTextLength.OptionsFilter.AllowAutoFilter = false;
            this.colTextLength.Visible = true;
            this.colTextLength.VisibleIndex = 2;
            this.colTextLength.Width = 83;
            // 
            // colFullPath
            // 
            this.colFullPath.Caption = "Full Path";
            this.colFullPath.FieldName = "FullPath";
            this.colFullPath.Name = "colFullPath";
            this.colFullPath.OptionsColumn.AllowEdit = false;
            this.colFullPath.OptionsColumn.ReadOnly = true;
            this.colFullPath.OptionsFilter.AllowAutoFilter = false;
            this.colFullPath.Visible = true;
            this.colFullPath.VisibleIndex = 3;
            this.colFullPath.Width = 82;
            // 
            // colSourceLanguage
            // 
            this.colSourceLanguage.Caption = "Source Language";
            this.colSourceLanguage.ColumnEdit = this.upEditLanguage;
            this.colSourceLanguage.FieldName = "SourceLanguage";
            this.colSourceLanguage.Name = "colSourceLanguage";
            this.colSourceLanguage.Visible = true;
            this.colSourceLanguage.VisibleIndex = 4;
            this.colSourceLanguage.Width = 136;
            // 
            // upEditLanguage
            // 
            this.upEditLanguage.AutoHeight = false;
            this.upEditLanguage.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.upEditLanguage.DisplayMember = "Caption";
            this.upEditLanguage.Name = "upEditLanguage";
            this.upEditLanguage.NullText = "Default";
            this.upEditLanguage.PopupView = this.viewUpEditLanguage;
            this.upEditLanguage.ValueMember = "Value";
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
            this.colTargetLanguage.ColumnEdit = this.upEditLanguage;
            this.colTargetLanguage.FieldName = "TargetLanguage";
            this.colTargetLanguage.Name = "colTargetLanguage";
            this.colTargetLanguage.Visible = true;
            this.colTargetLanguage.VisibleIndex = 5;
            this.colTargetLanguage.Width = 135;
            // 
            // colOutput
            // 
            this.colOutput.Caption = "Output";
            this.colOutput.ColumnEdit = this.upEditOutput;
            this.colOutput.FieldName = "Output";
            this.colOutput.Name = "colOutput";
            this.colOutput.Visible = true;
            this.colOutput.VisibleIndex = 6;
            this.colOutput.Width = 113;
            // 
            // upEditOutput
            // 
            this.upEditOutput.AutoHeight = false;
            this.upEditOutput.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.upEditOutput.Name = "upEditOutput";
            this.upEditOutput.NullText = "Default";
            this.upEditOutput.PopupView = this.repositoryItemGridLookUpEdit2View;
            // 
            // repositoryItemGridLookUpEdit2View
            // 
            this.repositoryItemGridLookUpEdit2View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.repositoryItemGridLookUpEdit2View.Name = "repositoryItemGridLookUpEdit2View";
            this.repositoryItemGridLookUpEdit2View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.repositoryItemGridLookUpEdit2View.OptionsView.ShowGroupPanel = false;
            // 
            // colProgress
            // 
            this.colProgress.Caption = "Progress";
            this.colProgress.ColumnEdit = this.repositoryItemProgressBar1;
            this.colProgress.FieldName = "Progress";
            this.colProgress.Name = "colProgress";
            this.colProgress.Visible = true;
            this.colProgress.VisibleIndex = 7;
            this.colProgress.Width = 134;
            // 
            // repositoryItemProgressBar1
            // 
            this.repositoryItemProgressBar1.Name = "repositoryItemProgressBar1";
            // 
            // colState
            // 
            this.colState.Caption = "State";
            this.colState.FieldName = "State";
            this.colState.Name = "colState";
            this.colState.Visible = true;
            this.colState.VisibleIndex = 8;
            this.colState.Width = 92;
            // 
            // colOperation
            // 
            this.colOperation.Caption = "Operation";
            this.colOperation.ColumnEdit = this.repositoryItemButtonEdit1;
            this.colOperation.Name = "colOperation";
            this.colOperation.Visible = true;
            this.colOperation.VisibleIndex = 9;
            this.colOperation.Width = 152;
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
            this.colTargetLangu.ColumnEdit = this.upEditLanguage;
            this.colTargetLangu.Name = "colTargetLangu";
            this.colTargetLangu.Visible = true;
            this.colTargetLangu.VisibleIndex = 5;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1231, 638);
            this.Controls.Add(this.gridControl1);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "文件翻译";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upEditLanguage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewUpEditLanguage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upEditOutput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit2View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemProgressBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colName;
        private DevExpress.XtraGrid.Columns.GridColumn colFileSize;
        private DevExpress.XtraGrid.Columns.GridColumn colTextLength;
        private DevExpress.XtraGrid.Columns.GridColumn colFullPath;
        private DevExpress.XtraGrid.Columns.GridColumn colSourceLanguage;
        private DevExpress.XtraGrid.Columns.GridColumn colTargetLanguage;
        private DevExpress.XtraGrid.Columns.GridColumn colOutput;
        private DevExpress.XtraGrid.Columns.GridColumn colProgress;
        private DevExpress.XtraGrid.Columns.GridColumn colState;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit upEditLanguage;
        private DevExpress.XtraGrid.Views.Grid.GridView viewUpEditLanguage;
        private DevExpress.XtraGrid.Columns.GridColumn colTargetLangu;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit upEditOutput;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemGridLookUpEdit2View;
        private DevExpress.XtraEditors.Repository.RepositoryItemProgressBar repositoryItemProgressBar1;
        private DevExpress.XtraGrid.Columns.GridColumn colOperation;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
    }
}

