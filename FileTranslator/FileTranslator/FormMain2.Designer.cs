namespace Renlen.FileTranslator
{
    partial class FormMain2
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gridView1 = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFileSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTextLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDirectoryPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSourceLanguage = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colTargetLanguage = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colOutput = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colProgress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridView1
            // 
            this.gridView1.AllowUserToAddRows = false;
            this.gridView1.AllowUserToDeleteRows = false;
            this.gridView1.AllowUserToOrderColumns = true;
            this.gridView1.AllowUserToResizeRows = false;
            this.gridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colFileSize,
            this.colTextLength,
            this.colDirectoryPath,
            this.colSourceLanguage,
            this.colTargetLanguage,
            this.colOutput,
            this.colProgress,
            this.colState});
            this.gridView1.GridColor = System.Drawing.SystemColors.Control;
            this.gridView1.Location = new System.Drawing.Point(12, 44);
            this.gridView1.Name = "gridView1";
            this.gridView1.Size = new System.Drawing.Size(1100, 494);
            this.gridView1.TabIndex = 0;
            this.gridView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.gridControl1_MouseClick);
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colName.DataPropertyName = "Name";
            dataGridViewCellStyle2.NullValue = null;
            this.colName.DefaultCellStyle = dataGridViewCellStyle2;
            this.colName.FillWeight = 150F;
            this.colName.HeaderText = "File Name";
            this.colName.MinimumWidth = 50;
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Width = 125;
            // 
            // colFileSize
            // 
            this.colFileSize.DataPropertyName = "FileSize";
            this.colFileSize.FillWeight = 40F;
            this.colFileSize.HeaderText = "File Size";
            this.colFileSize.MinimumWidth = 20;
            this.colFileSize.Name = "colFileSize";
            this.colFileSize.ReadOnly = true;
            // 
            // colTextLength
            // 
            this.colTextLength.DataPropertyName = "TextLength";
            this.colTextLength.FillWeight = 40F;
            this.colTextLength.HeaderText = "Text Length";
            this.colTextLength.Name = "colTextLength";
            // 
            // colDirectoryPath
            // 
            this.colDirectoryPath.DataPropertyName = "DirectoryPath";
            this.colDirectoryPath.FillWeight = 50F;
            this.colDirectoryPath.HeaderText = "Directory Path";
            this.colDirectoryPath.Name = "colDirectoryPath";
            // 
            // colSourceLanguage
            // 
            this.colSourceLanguage.DataPropertyName = "SourceLanguage";
            this.colSourceLanguage.FillWeight = 50F;
            this.colSourceLanguage.HeaderText = "Source Language";
            this.colSourceLanguage.Name = "colSourceLanguage";
            // 
            // colTargetLanguage
            // 
            this.colTargetLanguage.DataPropertyName = "TargetLanguage";
            this.colTargetLanguage.FillWeight = 50F;
            this.colTargetLanguage.HeaderText = "Target Language";
            this.colTargetLanguage.Name = "colTargetLanguage";
            // 
            // colOutput
            // 
            this.colOutput.DataPropertyName = "Output";
            this.colOutput.FillWeight = 50F;
            this.colOutput.HeaderText = "Output";
            this.colOutput.Name = "colOutput";
            // 
            // colProgress
            // 
            this.colProgress.DataPropertyName = "Progress";
            this.colProgress.FillWeight = 70F;
            this.colProgress.HeaderText = "Progress";
            this.colProgress.Name = "colProgress";
            // 
            // colState
            // 
            this.colState.DataPropertyName = "State";
            this.colState.FillWeight = 40F;
            this.colState.HeaderText = "State";
            this.colState.Name = "colState";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1121, 550);
            this.Controls.Add(this.gridView1);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "File Translator";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFileSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTextLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDirectoryPath;
        private System.Windows.Forms.DataGridViewComboBoxColumn colSourceLanguage;
        private System.Windows.Forms.DataGridViewComboBoxColumn colTargetLanguage;
        private System.Windows.Forms.DataGridViewComboBoxColumn colOutput;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProgress;
        private System.Windows.Forms.DataGridViewTextBoxColumn colState;
    }
}

