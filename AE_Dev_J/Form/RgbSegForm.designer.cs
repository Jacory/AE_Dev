namespace AE_Dev_J
{
    partial class RgbSegForm
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RgbSegForm));
            this.AutoSegTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.singleTab = new DevExpress.XtraTab.XtraTabPage();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.browseOutFileBtn = new DevExpress.XtraEditors.SimpleButton();
            this.outputTB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.browseinputFilebtn = new DevExpress.XtraEditors.SimpleButton();
            this.inputTB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.multiTab = new DevExpress.XtraTab.XtraTabPage();
            this.groupControl4 = new DevExpress.XtraEditors.GroupControl();
            this.browseOutFolderBtn = new DevExpress.XtraEditors.SimpleButton();
            this.outputFolderTB = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.browseinputFolderBtn = new DevExpress.XtraEditors.SimpleButton();
            this.inputFolderTB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.okBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.AutoSegTabControl1)).BeginInit();
            this.AutoSegTabControl1.SuspendLayout();
            this.singleTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.multiTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).BeginInit();
            this.groupControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            this.SuspendLayout();
            // 
            // AutoSegTabControl1
            // 
            this.AutoSegTabControl1.CustomHeaderButtons.AddRange(new DevExpress.XtraTab.Buttons.CustomHeaderButton[] {
            new DevExpress.XtraTab.Buttons.CustomHeaderButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "单个影像处理", -1, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, serializableAppearanceObject3, "", null, null, true),
            new DevExpress.XtraTab.Buttons.CustomHeaderButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "文件夹批处理", -1, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, serializableAppearanceObject4, "", null, null, true)});
            this.AutoSegTabControl1.Location = new System.Drawing.Point(0, 0);
            this.AutoSegTabControl1.Name = "AutoSegTabControl1";
            this.AutoSegTabControl1.SelectedTabPage = this.singleTab;
            this.AutoSegTabControl1.Size = new System.Drawing.Size(417, 193);
            this.AutoSegTabControl1.TabIndex = 0;
            this.AutoSegTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.singleTab,
            this.multiTab});
            // 
            // singleTab
            // 
            this.singleTab.Controls.Add(this.groupControl2);
            this.singleTab.Controls.Add(this.groupControl1);
            this.singleTab.Name = "singleTab";
            this.singleTab.Size = new System.Drawing.Size(411, 164);
            this.singleTab.Text = "单个影像处理";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.browseOutFileBtn);
            this.groupControl2.Controls.Add(this.outputTB);
            this.groupControl2.Controls.Add(this.label2);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupControl2.Location = new System.Drawing.Point(0, 81);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(411, 83);
            this.groupControl2.TabIndex = 3;
            this.groupControl2.Text = "输出";
            // 
            // browseOutFileBtn
            // 
            this.browseOutFileBtn.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.browseOutFileBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.browseOutFileBtn.Image = ((System.Drawing.Image)(resources.GetObject("browseOutFileBtn.Image")));
            this.browseOutFileBtn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.browseOutFileBtn.Location = new System.Drawing.Point(366, 22);
            this.browseOutFileBtn.Name = "browseOutFileBtn";
            this.browseOutFileBtn.Size = new System.Drawing.Size(43, 59);
            this.browseOutFileBtn.TabIndex = 2;
            this.browseOutFileBtn.Text = "浏览";
            this.browseOutFileBtn.Click += new System.EventHandler(this.browseOutFileBtn_Click);
            // 
            // outputTB
            // 
            this.outputTB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.outputTB.Location = new System.Drawing.Point(124, 41);
            this.outputTB.Name = "outputTB";
            this.outputTB.ReadOnly = true;
            this.outputTB.Size = new System.Drawing.Size(236, 25);
            this.outputTB.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "输入影像路径";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.browseinputFilebtn);
            this.groupControl1.Controls.Add(this.inputTB);
            this.groupControl1.Controls.Add(this.label1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(411, 83);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "输入";
            // 
            // browseinputFilebtn
            // 
            this.browseinputFilebtn.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.browseinputFilebtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.browseinputFilebtn.Image = ((System.Drawing.Image)(resources.GetObject("browseinputFilebtn.Image")));
            this.browseinputFilebtn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.browseinputFilebtn.Location = new System.Drawing.Point(366, 22);
            this.browseinputFilebtn.Name = "browseinputFilebtn";
            this.browseinputFilebtn.Size = new System.Drawing.Size(43, 59);
            this.browseinputFilebtn.TabIndex = 2;
            this.browseinputFilebtn.Text = "浏览";
            this.browseinputFilebtn.Click += new System.EventHandler(this.browseinputFilebtn_Click);
            // 
            // inputTB
            // 
            this.inputTB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.inputTB.Location = new System.Drawing.Point(124, 37);
            this.inputTB.Name = "inputTB";
            this.inputTB.ReadOnly = true;
            this.inputTB.Size = new System.Drawing.Size(236, 25);
            this.inputTB.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "输入影像路径";
            // 
            // multiTab
            // 
            this.multiTab.Controls.Add(this.groupControl4);
            this.multiTab.Controls.Add(this.groupControl3);
            this.multiTab.Name = "multiTab";
            this.multiTab.Size = new System.Drawing.Size(411, 164);
            this.multiTab.Text = "文件夹批处理";
            // 
            // groupControl4
            // 
            this.groupControl4.Controls.Add(this.browseOutFolderBtn);
            this.groupControl4.Controls.Add(this.outputFolderTB);
            this.groupControl4.Controls.Add(this.label4);
            this.groupControl4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupControl4.Location = new System.Drawing.Point(0, 81);
            this.groupControl4.Name = "groupControl4";
            this.groupControl4.Size = new System.Drawing.Size(411, 83);
            this.groupControl4.TabIndex = 4;
            this.groupControl4.Text = "输出";
            // 
            // browseOutFolderBtn
            // 
            this.browseOutFolderBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.browseOutFolderBtn.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.browseOutFolderBtn.Image = ((System.Drawing.Image)(resources.GetObject("browseOutFolderBtn.Image")));
            this.browseOutFolderBtn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.browseOutFolderBtn.Location = new System.Drawing.Point(365, 25);
            this.browseOutFolderBtn.Name = "browseOutFolderBtn";
            this.browseOutFolderBtn.Size = new System.Drawing.Size(44, 56);
            this.browseOutFolderBtn.TabIndex = 2;
            this.browseOutFolderBtn.Text = "浏览";
            this.browseOutFolderBtn.Click += new System.EventHandler(this.browseOutFolderBtn_Click);
            // 
            // outputFolderTB
            // 
            this.outputFolderTB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.outputFolderTB.Location = new System.Drawing.Point(124, 40);
            this.outputFolderTB.Name = "outputFolderTB";
            this.outputFolderTB.ReadOnly = true;
            this.outputFolderTB.Size = new System.Drawing.Size(235, 25);
            this.outputFolderTB.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "输出文件夹路径";
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.browseinputFolderBtn);
            this.groupControl3.Controls.Add(this.inputFolderTB);
            this.groupControl3.Controls.Add(this.label3);
            this.groupControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl3.Location = new System.Drawing.Point(0, 0);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(411, 83);
            this.groupControl3.TabIndex = 1;
            this.groupControl3.Text = "输入";
            // 
            // browseinputFolderBtn
            // 
            this.browseinputFolderBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseinputFolderBtn.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.browseinputFolderBtn.Image = ((System.Drawing.Image)(resources.GetObject("browseinputFolderBtn.Image")));
            this.browseinputFolderBtn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.browseinputFolderBtn.Location = new System.Drawing.Point(367, 21);
            this.browseinputFolderBtn.Name = "browseinputFolderBtn";
            this.browseinputFolderBtn.Size = new System.Drawing.Size(44, 60);
            this.browseinputFolderBtn.TabIndex = 2;
            this.browseinputFolderBtn.Text = "浏览";
            this.browseinputFolderBtn.Click += new System.EventHandler(this.browseinputFolderBtn_Click_1);
            // 
            // inputFolderTB
            // 
            this.inputFolderTB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.inputFolderTB.Location = new System.Drawing.Point(124, 37);
            this.inputFolderTB.Name = "inputFolderTB";
            this.inputFolderTB.ReadOnly = true;
            this.inputFolderTB.Size = new System.Drawing.Size(235, 25);
            this.inputFolderTB.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "输入文件夹路径";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.Location = new System.Drawing.Point(336, 195);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 28);
            this.cancelBtn.TabIndex = 6;
            this.cancelBtn.Text = "取消";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // okBtn
            // 
            this.okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBtn.Location = new System.Drawing.Point(255, 195);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 28);
            this.okBtn.TabIndex = 5;
            this.okBtn.Text = "运行";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // RgbSegForm
            // 
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 238);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.AutoSegTabControl1);
            this.MaximizeBox = false;
            this.Name = "RgbSegForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "自动影像分割";
            this.Load += new System.EventHandler(this.FrmAutoSeg_Load);
            ((System.ComponentModel.ISupportInitialize)(this.AutoSegTabControl1)).EndInit();
            this.AutoSegTabControl1.ResumeLayout(false);
            this.singleTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            this.multiTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).EndInit();
            this.groupControl4.ResumeLayout(false);
            this.groupControl4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            this.groupControl3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl AutoSegTabControl1;
        private DevExpress.XtraTab.XtraTabPage singleTab;
        private DevExpress.XtraTab.XtraTabPage multiTab;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private System.Windows.Forms.TextBox inputTB;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SimpleButton browseinputFilebtn;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.SimpleButton browseOutFileBtn;
        private System.Windows.Forms.TextBox outputTB;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.GroupControl groupControl4;
        private DevExpress.XtraEditors.SimpleButton browseOutFolderBtn;
        private System.Windows.Forms.TextBox outputFolderTB;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.SimpleButton browseinputFolderBtn;
        private System.Windows.Forms.TextBox inputFolderTB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button okBtn;
    }
}