namespace DocumentTranslation
{
  partial class Form_Output
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.radioButton1 = new System.Windows.Forms.RadioButton();
      this.radioButton2 = new System.Windows.Forms.RadioButton();
      this.label1 = new System.Windows.Forms.Label();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.radioButton3 = new System.Windows.Forms.RadioButton();
      this.radioButton4 = new System.Windows.Forms.RadioButton();
      this.label2 = new System.Windows.Forms.Label();
      this.textBox2 = new System.Windows.Forms.TextBox();
      this.checkBox1 = new System.Windows.Forms.CheckBox();
      this.button1 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.panel1 = new System.Windows.Forms.Panel();
      this.button3 = new System.Windows.Forms.Button();
      this.radioButton6 = new System.Windows.Forms.RadioButton();
      this.radioButton5 = new System.Windows.Forms.RadioButton();
      this.button4 = new System.Windows.Forms.Button();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // radioButton1
      // 
      this.radioButton1.AutoSize = true;
      this.radioButton1.Checked = true;
      this.radioButton1.Location = new System.Drawing.Point(27, 23);
      this.radioButton1.Name = "radioButton1";
      this.radioButton1.Size = new System.Drawing.Size(71, 16);
      this.radioButton1.TabIndex = 0;
      this.radioButton1.TabStop = true;
      this.radioButton1.Text = "每次询问";
      this.radioButton1.UseVisualStyleBackColor = true;
      this.radioButton1.Click += new System.EventHandler(this.RadioButton1_CheckedChanged);
      // 
      // radioButton2
      // 
      this.radioButton2.AutoSize = true;
      this.radioButton2.Location = new System.Drawing.Point(27, 54);
      this.radioButton2.Name = "radioButton2";
      this.radioButton2.Size = new System.Drawing.Size(119, 16);
      this.radioButton2.TabIndex = 0;
      this.radioButton2.TabStop = true;
      this.radioButton2.Text = "备份并替换源文件";
      this.radioButton2.UseVisualStyleBackColor = true;
      this.radioButton2.Click += new System.EventHandler(this.RadioButton1_CheckedChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(15, 10);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(89, 12);
      this.label1.TabIndex = 1;
      this.label1.Text = "备份文件保存到";
      // 
      // textBox1
      // 
      this.textBox1.Location = new System.Drawing.Point(132, 49);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(380, 21);
      this.textBox1.TabIndex = 2;
      this.textBox1.Text = "Backups";
      // 
      // radioButton3
      // 
      this.radioButton3.AutoSize = true;
      this.radioButton3.Location = new System.Drawing.Point(27, 149);
      this.radioButton3.Name = "radioButton3";
      this.radioButton3.Size = new System.Drawing.Size(83, 16);
      this.radioButton3.TabIndex = 0;
      this.radioButton3.Text = "覆盖源文件";
      this.radioButton3.UseVisualStyleBackColor = true;
      this.radioButton3.Click += new System.EventHandler(this.RadioButton1_CheckedChanged);
      // 
      // radioButton4
      // 
      this.radioButton4.AutoSize = true;
      this.radioButton4.Location = new System.Drawing.Point(27, 181);
      this.radioButton4.Name = "radioButton4";
      this.radioButton4.Size = new System.Drawing.Size(107, 16);
      this.radioButton4.TabIndex = 0;
      this.radioButton4.Text = "输出到指定位置";
      this.radioButton4.UseVisualStyleBackColor = true;
      this.radioButton4.Click += new System.EventHandler(this.RadioButton1_CheckedChanged);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(74, 211);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(53, 12);
      this.label2.TabIndex = 1;
      this.label2.Text = "输出目录";
      // 
      // textBox2
      // 
      this.textBox2.Location = new System.Drawing.Point(133, 208);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new System.Drawing.Size(438, 21);
      this.textBox2.TabIndex = 2;
      this.textBox2.Text = "Output";
      // 
      // checkBox1
      // 
      this.checkBox1.AutoSize = true;
      this.checkBox1.Location = new System.Drawing.Point(27, 257);
      this.checkBox1.Name = "checkBox1";
      this.checkBox1.Size = new System.Drawing.Size(96, 16);
      this.checkBox1.TabIndex = 3;
      this.checkBox1.Text = "记住我的选择";
      this.checkBox1.UseVisualStyleBackColor = true;
      // 
      // button1
      // 
      this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.button1.Location = new System.Drawing.Point(463, 297);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(88, 31);
      this.button1.TabIndex = 4;
      this.button1.Text = "确定(&O)";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.Button1_Click);
      // 
      // button2
      // 
      this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.button2.Location = new System.Drawing.Point(557, 297);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(88, 31);
      this.button2.TabIndex = 4;
      this.button2.Text = "取消(&C)";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.Button2_Click);
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.button3);
      this.panel1.Controls.Add(this.textBox1);
      this.panel1.Controls.Add(this.radioButton6);
      this.panel1.Controls.Add(this.radioButton5);
      this.panel1.Controls.Add(this.label1);
      this.panel1.Location = new System.Drawing.Point(59, 73);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(596, 76);
      this.panel1.TabIndex = 5;
      // 
      // button3
      // 
      this.button3.Location = new System.Drawing.Point(518, 47);
      this.button3.Name = "button3";
      this.button3.Size = new System.Drawing.Size(75, 23);
      this.button3.TabIndex = 3;
      this.button3.Text = "浏览";
      this.button3.UseVisualStyleBackColor = true;
      this.button3.Click += new System.EventHandler(this.Button3_Click);
      // 
      // radioButton6
      // 
      this.radioButton6.AutoSize = true;
      this.radioButton6.Location = new System.Drawing.Point(55, 50);
      this.radioButton6.Name = "radioButton6";
      this.radioButton6.Size = new System.Drawing.Size(71, 16);
      this.radioButton6.TabIndex = 2;
      this.radioButton6.TabStop = true;
      this.radioButton6.Text = "指定目录";
      this.radioButton6.UseVisualStyleBackColor = true;
      // 
      // radioButton5
      // 
      this.radioButton5.AutoSize = true;
      this.radioButton5.Checked = true;
      this.radioButton5.Location = new System.Drawing.Point(55, 28);
      this.radioButton5.Name = "radioButton5";
      this.radioButton5.Size = new System.Drawing.Size(95, 16);
      this.radioButton5.TabIndex = 2;
      this.radioButton5.TabStop = true;
      this.radioButton5.Text = "文件所在目录";
      this.radioButton5.UseVisualStyleBackColor = true;
      // 
      // button4
      // 
      this.button4.Location = new System.Drawing.Point(580, 206);
      this.button4.Name = "button4";
      this.button4.Size = new System.Drawing.Size(75, 23);
      this.button4.TabIndex = 3;
      this.button4.Text = "浏览";
      this.button4.UseVisualStyleBackColor = true;
      this.button4.Click += new System.EventHandler(this.Button4_Click);
      // 
      // Form_Output
      // 
      this.AcceptButton = this.button1;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(661, 340);
      this.Controls.Add(this.button4);
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.checkBox1);
      this.Controls.Add(this.textBox2);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.radioButton4);
      this.Controls.Add(this.radioButton3);
      this.Controls.Add(this.radioButton2);
      this.Controls.Add(this.radioButton1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "Form_Output";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "输出选项";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Output_FormClosing);
      this.Load += new System.EventHandler(this.Form_Output_Load);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.RadioButton radioButton1;
    private System.Windows.Forms.RadioButton radioButton2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.RadioButton radioButton3;
    private System.Windows.Forms.RadioButton radioButton4;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox textBox2;
    private System.Windows.Forms.CheckBox checkBox1;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.RadioButton radioButton6;
    private System.Windows.Forms.RadioButton radioButton5;
    private System.Windows.Forms.Button button3;
    private System.Windows.Forms.Button button4;
  }
}