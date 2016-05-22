namespace AE_Dev_J.Form
{
    partial class AddFeatureClassForm
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
            this.AddFeatrueClass_filename = new DevExpress.XtraEditors.TextEdit();
            this.addfeatureclass_namelabel = new DevExpress.XtraEditors.LabelControl();
            this.addfeatureclass_locationlabel = new DevExpress.XtraEditors.LabelControl();
            this.AddFeatrueClass_locationbutton = new DevExpress.XtraEditors.ButtonEdit();
            this.AddFeatrueClass_geometrytype = new DevExpress.XtraEditors.ComboBoxEdit();
            this.addfeatureclass_typelabel = new DevExpress.XtraEditors.LabelControl();
            this.AddFeatrueClass_OKbutton = new DevExpress.XtraEditors.SimpleButton();
            this.AddFeatrueClass_Cancelbutton = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.AddFeatrueClass_radioGroup.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddFeatureClass_groupControl)).BeginInit();
            this.AddFeatureClass_groupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AddFeatrueClass_filename.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddFeatrueClass_locationbutton.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddFeatrueClass_geometrytype.Properties)).BeginInit();
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
            this.AddFeatureClass_groupControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.AddFeatureClass_groupControl.Controls.Add(this.AddFeatrueClass_filename);
            this.AddFeatureClass_groupControl.Controls.Add(this.addfeatureclass_namelabel);
            this.AddFeatureClass_groupControl.Controls.Add(this.addfeatureclass_locationlabel);
            this.AddFeatureClass_groupControl.Controls.Add(this.AddFeatrueClass_locationbutton);
            this.AddFeatureClass_groupControl.Location = new System.Drawing.Point(12, 110);
            this.AddFeatureClass_groupControl.Name = "AddFeatureClass_groupControl";
            this.AddFeatureClass_groupControl.ShowCaption = false;
            this.AddFeatureClass_groupControl.Size = new System.Drawing.Size(508, 111);
            this.AddFeatureClass_groupControl.TabIndex = 1;
            this.AddFeatureClass_groupControl.Text = "groupControl1";
            // 
            // AddFeatrueClass_filename
            // 
            this.AddFeatrueClass_filename.Location = new System.Drawing.Point(14, 74);
            this.AddFeatrueClass_filename.Name = "AddFeatrueClass_filename";
            this.AddFeatrueClass_filename.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.AddFeatrueClass_filename.Size = new System.Drawing.Size(482, 20);
            this.AddFeatrueClass_filename.TabIndex = 3;
            // 
            // addfeatureclass_namelabel
            // 
            this.addfeatureclass_namelabel.Location = new System.Drawing.Point(14, 53);
            this.addfeatureclass_namelabel.Name = "addfeatureclass_namelabel";
            this.addfeatureclass_namelabel.Size = new System.Drawing.Size(106, 14);
            this.addfeatureclass_namelabel.TabIndex = 2;
            this.addfeatureclass_namelabel.Text = "Feature Class Name";
            // 
            // addfeatureclass_locationlabel
            // 
            this.addfeatureclass_locationlabel.Location = new System.Drawing.Point(14, 6);
            this.addfeatureclass_locationlabel.Name = "addfeatureclass_locationlabel";
            this.addfeatureclass_locationlabel.Size = new System.Drawing.Size(121, 14);
            this.addfeatureclass_locationlabel.TabIndex = 1;
            this.addfeatureclass_locationlabel.Text = "Feature Class Location";
            // 
            // AddFeatrueClass_locationbutton
            // 
            this.AddFeatrueClass_locationbutton.Location = new System.Drawing.Point(14, 26);
            this.AddFeatrueClass_locationbutton.Name = "AddFeatrueClass_locationbutton";
            this.AddFeatrueClass_locationbutton.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.AddFeatrueClass_locationbutton.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.AddFeatrueClass_locationbutton.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.AddFeatrueClass_locationbutton.Size = new System.Drawing.Size(482, 20);
            this.AddFeatrueClass_locationbutton.TabIndex = 0;
            this.AddFeatrueClass_locationbutton.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.AddFeatrueClass_buttonEdit1_ButtonClick);
            // 
            // AddFeatrueClass_geometrytype
            // 
            this.AddFeatrueClass_geometrytype.Location = new System.Drawing.Point(26, 84);
            this.AddFeatrueClass_geometrytype.Name = "AddFeatrueClass_geometrytype";
            this.AddFeatrueClass_geometrytype.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.AddFeatrueClass_geometrytype.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.AddFeatrueClass_geometrytype.Properties.Items.AddRange(new object[] {
            "POINT",
            "POLYLINE",
            "POLYGON"});
            this.AddFeatrueClass_geometrytype.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.AddFeatrueClass_geometrytype.Size = new System.Drawing.Size(482, 20);
            this.AddFeatrueClass_geometrytype.TabIndex = 5;
            // 
            // addfeatureclass_typelabel
            // 
            this.addfeatureclass_typelabel.Location = new System.Drawing.Point(26, 64);
            this.addfeatureclass_typelabel.Name = "addfeatureclass_typelabel";
            this.addfeatureclass_typelabel.Size = new System.Drawing.Size(86, 14);
            this.addfeatureclass_typelabel.TabIndex = 4;
            this.addfeatureclass_typelabel.Text = "Geometry Type";
            // 
            // AddFeatrueClass_OKbutton
            // 
            this.AddFeatrueClass_OKbutton.Location = new System.Drawing.Point(322, 227);
            this.AddFeatrueClass_OKbutton.Name = "AddFeatrueClass_OKbutton";
            this.AddFeatrueClass_OKbutton.Size = new System.Drawing.Size(96, 23);
            this.AddFeatrueClass_OKbutton.TabIndex = 2;
            this.AddFeatrueClass_OKbutton.Text = "OK";
            this.AddFeatrueClass_OKbutton.Click += new System.EventHandler(this.AddFeatrueClass_OKbutton_Click);
            // 
            // AddFeatrueClass_Cancelbutton
            // 
            this.AddFeatrueClass_Cancelbutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.AddFeatrueClass_Cancelbutton.Location = new System.Drawing.Point(424, 227);
            this.AddFeatrueClass_Cancelbutton.Name = "AddFeatrueClass_Cancelbutton";
            this.AddFeatrueClass_Cancelbutton.Size = new System.Drawing.Size(96, 23);
            this.AddFeatrueClass_Cancelbutton.TabIndex = 3;
            this.AddFeatrueClass_Cancelbutton.Text = "Cancel";
            this.AddFeatrueClass_Cancelbutton.Click += new System.EventHandler(this.AddFeatrueClass_Cancelbutton_Click);
            // 
            // AddFeatureClassForm
            // 
            this.AcceptButton = this.AddFeatrueClass_OKbutton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.AddFeatrueClass_Cancelbutton;
            this.ClientSize = new System.Drawing.Size(532, 262);
            this.Controls.Add(this.addfeatureclass_typelabel);
            this.Controls.Add(this.AddFeatrueClass_Cancelbutton);
            this.Controls.Add(this.AddFeatrueClass_OKbutton);
            this.Controls.Add(this.AddFeatrueClass_geometrytype);
            this.Controls.Add(this.AddFeatureClass_groupControl);
            this.Controls.Add(this.AddFeatrueClass_radioGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AddFeatureClassForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddFeatureClass";
            ((System.ComponentModel.ISupportInitialize)(this.AddFeatrueClass_radioGroup.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddFeatureClass_groupControl)).EndInit();
            this.AddFeatureClass_groupControl.ResumeLayout(false);
            this.AddFeatureClass_groupControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AddFeatrueClass_filename.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddFeatrueClass_locationbutton.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddFeatrueClass_geometrytype.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.RadioGroup AddFeatrueClass_radioGroup;
        private DevExpress.XtraEditors.GroupControl AddFeatureClass_groupControl;
        private DevExpress.XtraEditors.ButtonEdit AddFeatrueClass_locationbutton;
        private DevExpress.XtraEditors.LabelControl addfeatureclass_typelabel;
        private DevExpress.XtraEditors.TextEdit AddFeatrueClass_filename;
        private DevExpress.XtraEditors.LabelControl addfeatureclass_namelabel;
        private DevExpress.XtraEditors.LabelControl addfeatureclass_locationlabel;
        private DevExpress.XtraEditors.ComboBoxEdit AddFeatrueClass_geometrytype;
        private DevExpress.XtraEditors.SimpleButton AddFeatrueClass_OKbutton;
        private DevExpress.XtraEditors.SimpleButton AddFeatrueClass_Cancelbutton;

    }
}