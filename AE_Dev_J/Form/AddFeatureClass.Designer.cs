namespace AE_Dev_J.Form
{
    partial class AddFeatureClass
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
            this.AddFeatrueClass_radioGroup = new DevExpress.XtraEditors.RadioGroup();
            this.AddFeatureClass_groupControl = new DevExpress.XtraEditors.GroupControl();
            this.AddFeatrueClass_geometrytype = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.AddFeatrueClass_filename = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.AddFeatrueClass_locationbutton = new DevExpress.XtraEditors.ButtonEdit();
            this.AddFeatrueClass_OKbutton = new DevExpress.XtraEditors.SimpleButton();
            this.AddFeatrueClass_Cancelbutton = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.AddFeatrueClass_radioGroup.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddFeatureClass_groupControl)).BeginInit();
            this.AddFeatureClass_groupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AddFeatrueClass_geometrytype.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddFeatrueClass_filename.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddFeatrueClass_locationbutton.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // AddFeatrueClass_radioGroup
            // 
            this.AddFeatrueClass_radioGroup.Location = new System.Drawing.Point(6, 5);
            this.AddFeatrueClass_radioGroup.Name = "AddFeatrueClass_radioGroup";
            this.AddFeatrueClass_radioGroup.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.AddFeatrueClass_radioGroup.Properties.Appearance.Options.UseBackColor = true;
            this.AddFeatrueClass_radioGroup.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.AddFeatrueClass_radioGroup.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "临时图层"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "自定义图层")});
            this.AddFeatrueClass_radioGroup.Size = new System.Drawing.Size(361, 59);
            this.AddFeatrueClass_radioGroup.TabIndex = 0;
            this.AddFeatrueClass_radioGroup.SelectedIndexChanged += new System.EventHandler(this.AddFeatrueClass_radioGroup_SelectedIndexChanged);
            // 
            // AddFeatureClass_groupControl
            // 
            this.AddFeatureClass_groupControl.Controls.Add(this.AddFeatrueClass_geometrytype);
            this.AddFeatureClass_groupControl.Controls.Add(this.labelControl3);
            this.AddFeatureClass_groupControl.Controls.Add(this.AddFeatrueClass_filename);
            this.AddFeatureClass_groupControl.Controls.Add(this.labelControl2);
            this.AddFeatureClass_groupControl.Controls.Add(this.labelControl1);
            this.AddFeatureClass_groupControl.Controls.Add(this.AddFeatrueClass_locationbutton);
            this.AddFeatureClass_groupControl.Location = new System.Drawing.Point(12, 61);
            this.AddFeatureClass_groupControl.Name = "AddFeatureClass_groupControl";
            this.AddFeatureClass_groupControl.ShowCaption = false;
            this.AddFeatureClass_groupControl.Size = new System.Drawing.Size(508, 160);
            this.AddFeatureClass_groupControl.TabIndex = 1;
            this.AddFeatureClass_groupControl.Text = "groupControl1";
            // 
            // AddFeatrueClass_geometrytype
            // 
            this.AddFeatrueClass_geometrytype.Location = new System.Drawing.Point(12, 125);
            this.AddFeatrueClass_geometrytype.Name = "AddFeatrueClass_geometrytype";
            this.AddFeatrueClass_geometrytype.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.AddFeatrueClass_geometrytype.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.AddFeatrueClass_geometrytype.Properties.Items.AddRange(new object[] {
            "POINT",
            "POLYLINE",
            "POLYGON"});
            this.AddFeatrueClass_geometrytype.Size = new System.Drawing.Size(482, 20);
            this.AddFeatrueClass_geometrytype.TabIndex = 5;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(12, 104);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(86, 14);
            this.labelControl3.TabIndex = 4;
            this.labelControl3.Text = "Geometry Type";
            // 
            // AddFeatrueClass_filename
            // 
            this.AddFeatrueClass_filename.Location = new System.Drawing.Point(12, 77);
            this.AddFeatrueClass_filename.Name = "AddFeatrueClass_filename";
            this.AddFeatrueClass_filename.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.AddFeatrueClass_filename.Size = new System.Drawing.Size(482, 20);
            this.AddFeatrueClass_filename.TabIndex = 3;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 56);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(106, 14);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "Feature Class Name";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 9);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(121, 14);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "Feature Class Location";
            // 
            // AddFeatrueClass_locationbutton
            // 
            this.AddFeatrueClass_locationbutton.Location = new System.Drawing.Point(12, 29);
            this.AddFeatrueClass_locationbutton.Name = "AddFeatrueClass_locationbutton";
            this.AddFeatrueClass_locationbutton.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.AddFeatrueClass_locationbutton.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.AddFeatrueClass_locationbutton.Size = new System.Drawing.Size(482, 20);
            this.AddFeatrueClass_locationbutton.TabIndex = 0;
            this.AddFeatrueClass_locationbutton.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.AddFeatrueClass_buttonEdit1_ButtonClick);
            // 
            // AddFeatrueClass_OKbutton
            // 
            this.AddFeatrueClass_OKbutton.Location = new System.Drawing.Point(304, 227);
            this.AddFeatrueClass_OKbutton.Name = "AddFeatrueClass_OKbutton";
            this.AddFeatrueClass_OKbutton.Size = new System.Drawing.Size(96, 23);
            this.AddFeatrueClass_OKbutton.TabIndex = 2;
            this.AddFeatrueClass_OKbutton.Text = "OK";
            this.AddFeatrueClass_OKbutton.Click += new System.EventHandler(this.AddFeatrueClass_OKbutton_Click);
            // 
            // AddFeatrueClass_Cancelbutton
            // 
            this.AddFeatrueClass_Cancelbutton.Location = new System.Drawing.Point(424, 227);
            this.AddFeatrueClass_Cancelbutton.Name = "AddFeatrueClass_Cancelbutton";
            this.AddFeatrueClass_Cancelbutton.Size = new System.Drawing.Size(96, 23);
            this.AddFeatrueClass_Cancelbutton.TabIndex = 3;
            this.AddFeatrueClass_Cancelbutton.Text = "Cancel";
            this.AddFeatrueClass_Cancelbutton.Click += new System.EventHandler(this.AddFeatrueClass_Cancelbutton_Click);
            // 
            // AddFeatureClass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 262);
            this.Controls.Add(this.AddFeatrueClass_Cancelbutton);
            this.Controls.Add(this.AddFeatrueClass_OKbutton);
            this.Controls.Add(this.AddFeatureClass_groupControl);
            this.Controls.Add(this.AddFeatrueClass_radioGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AddFeatureClass";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddFeatureClass";
            ((System.ComponentModel.ISupportInitialize)(this.AddFeatrueClass_radioGroup.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddFeatureClass_groupControl)).EndInit();
            this.AddFeatureClass_groupControl.ResumeLayout(false);
            this.AddFeatureClass_groupControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AddFeatrueClass_geometrytype.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddFeatrueClass_filename.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddFeatrueClass_locationbutton.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.RadioGroup AddFeatrueClass_radioGroup;
        private DevExpress.XtraEditors.GroupControl AddFeatureClass_groupControl;
        private DevExpress.XtraEditors.ButtonEdit AddFeatrueClass_locationbutton;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit AddFeatrueClass_filename;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit AddFeatrueClass_geometrytype;
        private DevExpress.XtraEditors.SimpleButton AddFeatrueClass_OKbutton;
        private DevExpress.XtraEditors.SimpleButton AddFeatrueClass_Cancelbutton;

    }
}