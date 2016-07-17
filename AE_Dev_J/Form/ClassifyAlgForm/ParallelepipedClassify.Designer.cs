namespace AE_Dev_J.Form.ClassifyAlgForm
{
    partial class ParallelepipedClassify
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParallelepipedClassify));
            this.label1 = new System.Windows.Forms.Label();
            this.paralle_thresh_trackBarControl = new DevExpress.XtraEditors.TrackBarControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.paralle_introduction_memoEdit = new DevExpress.XtraEditors.MemoEdit();
            this.paralle_thresh_radioGroup = new DevExpress.XtraEditors.RadioGroup();
            this.panel1 = new System.Windows.Forms.Panel();
            this.parallel_thresh_textEdit = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.paralle_thresh_trackBarControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paralle_thresh_trackBarControl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paralle_introduction_memoEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paralle_thresh_radioGroup.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.parallel_thresh_textEdit.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 14);
            this.label1.TabIndex = 7;
            this.label1.Text = "阈值";
            // 
            // paralle_thresh_trackBarControl
            // 
            this.paralle_thresh_trackBarControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.paralle_thresh_trackBarControl.EditValue = null;
            this.paralle_thresh_trackBarControl.Enabled = false;
            this.paralle_thresh_trackBarControl.Location = new System.Drawing.Point(93, 149);
            this.paralle_thresh_trackBarControl.Name = "paralle_thresh_trackBarControl";
            this.paralle_thresh_trackBarControl.Properties.LabelAppearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.paralle_thresh_trackBarControl.Properties.LabelAppearance.Options.UseFont = true;
            this.paralle_thresh_trackBarControl.Properties.LabelAppearance.Options.UseTextOptions = true;
            this.paralle_thresh_trackBarControl.Properties.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.paralle_thresh_trackBarControl.Properties.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.paralle_thresh_trackBarControl.Properties.TickStyle = System.Windows.Forms.TickStyle.None;
            this.paralle_thresh_trackBarControl.Size = new System.Drawing.Size(45, 181);
            this.paralle_thresh_trackBarControl.TabIndex = 5;
            this.paralle_thresh_trackBarControl.EditValueChanged += new System.EventHandler(this.paralle_thresh_trackBarControl_EditValueChanged);
            this.paralle_thresh_trackBarControl.DoubleClick += new System.EventHandler(this.paralle_thresh_trackBarControl_DoubleClick);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Location = new System.Drawing.Point(12, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Padding = new System.Windows.Forms.Padding(10);
            this.labelControl1.Size = new System.Drawing.Size(139, 41);
            this.labelControl1.TabIndex = 10;
            this.labelControl1.Text = "设置标准差参数";
            // 
            // paralle_introduction_memoEdit
            // 
            this.paralle_introduction_memoEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paralle_introduction_memoEdit.EditValue = resources.GetString("paralle_introduction_memoEdit.EditValue");
            this.paralle_introduction_memoEdit.Location = new System.Drawing.Point(174, 0);
            this.paralle_introduction_memoEdit.Name = "paralle_introduction_memoEdit";
            this.paralle_introduction_memoEdit.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.paralle_introduction_memoEdit.Properties.Appearance.Options.UseFont = true;
            this.paralle_introduction_memoEdit.Properties.ReadOnly = true;
            this.paralle_introduction_memoEdit.Size = new System.Drawing.Size(275, 362);
            this.paralle_introduction_memoEdit.TabIndex = 11;
            this.paralle_introduction_memoEdit.UseOptimizedRendering = true;
            // 
            // paralle_thresh_radioGroup
            // 
            this.paralle_thresh_radioGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.paralle_thresh_radioGroup.Location = new System.Drawing.Point(21, 50);
            this.paralle_thresh_radioGroup.Name = "paralle_thresh_radioGroup";
            this.paralle_thresh_radioGroup.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.paralle_thresh_radioGroup.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.paralle_thresh_radioGroup.Properties.Appearance.Options.UseBackColor = true;
            this.paralle_thresh_radioGroup.Properties.Appearance.Options.UseFont = true;
            this.paralle_thresh_radioGroup.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.paralle_thresh_radioGroup.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "默认"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "设置单一阈值")});
            this.paralle_thresh_radioGroup.Size = new System.Drawing.Size(142, 67);
            this.paralle_thresh_radioGroup.TabIndex = 9;
            this.paralle_thresh_radioGroup.SelectedIndexChanged += new System.EventHandler(this.paralle_thresh_radioGroup_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.parallel_thresh_textEdit);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.paralle_thresh_trackBarControl);
            this.panel1.Controls.Add(this.paralle_thresh_radioGroup);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.MinimumSize = new System.Drawing.Size(174, 393);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(174, 393);
            this.panel1.TabIndex = 12;
            // 
            // parallel_thresh_textEdit
            // 
            this.parallel_thresh_textEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.parallel_thresh_textEdit.EditValue = "";
            this.parallel_thresh_textEdit.Enabled = false;
            this.parallel_thresh_textEdit.Location = new System.Drawing.Point(96, 123);
            this.parallel_thresh_textEdit.Name = "parallel_thresh_textEdit";
            this.parallel_thresh_textEdit.Properties.Appearance.Options.UseTextOptions = true;
            this.parallel_thresh_textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.parallel_thresh_textEdit.Properties.ValidateOnEnterKey = true;
            this.parallel_thresh_textEdit.Properties.Validating += new System.ComponentModel.CancelEventHandler(this.parallel_thresh_textEdit_Properties_Validating);
            this.parallel_thresh_textEdit.Size = new System.Drawing.Size(42, 20);
            this.parallel_thresh_textEdit.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(73, 308);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 14);
            this.label3.TabIndex = 11;
            this.label3.Text = "0";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(73, 155);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 14);
            this.label2.TabIndex = 11;
            this.label2.Text = "1";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(67, 233);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 14);
            this.label4.TabIndex = 11;
            this.label4.Text = "0.5";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 232);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 14);
            this.label5.TabIndex = 11;
            this.label5.Text = "默认值";
            // 
            // ParallelepipedClassify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 362);
            this.Controls.Add(this.paralle_introduction_memoEdit);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(16, 401);
            this.Name = "ParallelepipedClassify";
            ((System.ComponentModel.ISupportInitialize)(this.paralle_thresh_trackBarControl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paralle_thresh_trackBarControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paralle_introduction_memoEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paralle_thresh_radioGroup.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.parallel_thresh_textEdit.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TrackBarControl paralle_thresh_trackBarControl;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.MemoEdit paralle_introduction_memoEdit;
        private DevExpress.XtraEditors.RadioGroup paralle_thresh_radioGroup;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.TextEdit parallel_thresh_textEdit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
    }
}