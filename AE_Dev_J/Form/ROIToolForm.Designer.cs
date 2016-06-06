namespace AE_Dev_J.Form
{
    partial class ROIToolForm
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
            this.components = new System.ComponentModel.Container();
            this.SC_barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.SC_ToolBar = new DevExpress.XtraBars.Bar();
            this.SC_LayerName = new DevExpress.XtraBars.BarStaticItem();
            this.SampleLayerCombox = new DevExpress.XtraBars.BarEditItem();
            this.LayerCombox = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            this.SC_SelectSampleButton = new DevExpress.XtraBars.BarButtonItem();
            this.SC_OpenSamplefile = new DevExpress.XtraBars.BarButtonItem();
            this.SC_SaveSample = new DevExpress.XtraBars.BarButtonItem();
            this.SC_MergeSample = new DevExpress.XtraBars.BarButtonItem();
            this.SC_SplitSample = new DevExpress.XtraBars.BarButtonItem();
            this.SC_RemoveSample = new DevExpress.XtraBars.BarButtonItem();
            this.SC_ClearSamples = new DevExpress.XtraBars.BarButtonItem();
            this.SC_CreateSampleFiles = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.SC_dataGridView = new System.Windows.Forms.DataGridView();
            this.count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.color = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.SC_barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LayerCombox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SC_dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // SC_barManager
            // 
            this.SC_barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.SC_ToolBar});
            this.SC_barManager.DockControls.Add(this.barDockControlTop);
            this.SC_barManager.DockControls.Add(this.barDockControlBottom);
            this.SC_barManager.DockControls.Add(this.barDockControlLeft);
            this.SC_barManager.DockControls.Add(this.barDockControlRight);
            this.SC_barManager.Form = this;
            this.SC_barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.SC_SelectSampleButton,
            this.SC_OpenSamplefile,
            this.SC_SaveSample,
            this.SC_MergeSample,
            this.SC_SplitSample,
            this.SC_RemoveSample,
            this.SC_ClearSamples,
            this.SC_CreateSampleFiles,
            this.SampleLayerCombox,
            this.SC_LayerName});
            this.SC_barManager.MaxItemId = 10;
            this.SC_barManager.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.LayerCombox});
            // 
            // SC_ToolBar
            // 
            this.SC_ToolBar.BarName = "SC_Tools";
            this.SC_ToolBar.DockCol = 0;
            this.SC_ToolBar.DockRow = 0;
            this.SC_ToolBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.SC_ToolBar.FloatLocation = new System.Drawing.Point(453, 164);
            this.SC_ToolBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.SC_LayerName),
            new DevExpress.XtraBars.LinkPersistInfo(this.SampleLayerCombox),
            new DevExpress.XtraBars.LinkPersistInfo(this.SC_SelectSampleButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.SC_OpenSamplefile, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.SC_SaveSample),
            new DevExpress.XtraBars.LinkPersistInfo(this.SC_MergeSample),
            new DevExpress.XtraBars.LinkPersistInfo(this.SC_SplitSample),
            new DevExpress.XtraBars.LinkPersistInfo(this.SC_RemoveSample),
            new DevExpress.XtraBars.LinkPersistInfo(this.SC_ClearSamples),
            new DevExpress.XtraBars.LinkPersistInfo(this.SC_CreateSampleFiles)});
            this.SC_ToolBar.Text = "Tools";
            // 
            // SC_LayerName
            // 
            this.SC_LayerName.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.SC_LayerName.Caption = "图层：";
            this.SC_LayerName.Id = 9;
            this.SC_LayerName.Name = "SC_LayerName";
            this.SC_LayerName.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // SampleLayerCombox
            // 
            this.SampleLayerCombox.Caption = "SampleLayerCombox";
            this.SampleLayerCombox.Edit = this.LayerCombox;
            this.SampleLayerCombox.Id = 8;
            this.SampleLayerCombox.Name = "SampleLayerCombox";
            this.SampleLayerCombox.Width = 114;
            this.SampleLayerCombox.ShowingEditor += new DevExpress.XtraBars.ItemCancelEventHandler(this.SampleLayerCombox_ShowingEditor);
            // 
            // LayerCombox
            // 
            this.LayerCombox.AutoHeight = false;
            this.LayerCombox.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LayerCombox.Name = "LayerCombox";
            this.LayerCombox.SelectedIndexChanged += new System.EventHandler(this.LayerCombox_SelectedIndexChanged);
            // 
            // SC_SelectSampleButton
            // 
            this.SC_SelectSampleButton.Caption = "采集";
            this.SC_SelectSampleButton.Id = 0;
            this.SC_SelectSampleButton.Name = "SC_SelectSampleButton";
            this.SC_SelectSampleButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.SC_SelectSampleButton_ItemClick);
            // 
            // SC_OpenSamplefile
            // 
            this.SC_OpenSamplefile.Caption = "打开样本文件";
            this.SC_OpenSamplefile.Id = 1;
            this.SC_OpenSamplefile.Name = "SC_OpenSamplefile";
            // 
            // SC_SaveSample
            // 
            this.SC_SaveSample.Caption = "保存样本区域";
            this.SC_SaveSample.Id = 2;
            this.SC_SaveSample.Name = "SC_SaveSample";
            // 
            // SC_MergeSample
            // 
            this.SC_MergeSample.Caption = "合并";
            this.SC_MergeSample.Id = 3;
            this.SC_MergeSample.Name = "SC_MergeSample";
            // 
            // SC_SplitSample
            // 
            this.SC_SplitSample.Caption = "分离";
            this.SC_SplitSample.Id = 4;
            this.SC_SplitSample.Name = "SC_SplitSample";
            // 
            // SC_RemoveSample
            // 
            this.SC_RemoveSample.Caption = "删除";
            this.SC_RemoveSample.Id = 5;
            this.SC_RemoveSample.Name = "SC_RemoveSample";
            // 
            // SC_ClearSamples
            // 
            this.SC_ClearSamples.Caption = "清空";
            this.SC_ClearSamples.Id = 6;
            this.SC_ClearSamples.Name = "SC_ClearSamples";
            // 
            // SC_CreateSampleFiles
            // 
            this.SC_CreateSampleFiles.Caption = "生成文件";
            this.SC_CreateSampleFiles.Id = 7;
            this.SC_CreateSampleFiles.Name = "SC_CreateSampleFiles";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(690, 31);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 376);
            this.barDockControlBottom.Size = new System.Drawing.Size(690, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 31);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 345);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(690, 31);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 345);
            // 
            // SC_dataGridView
            // 
            this.SC_dataGridView.AllowUserToAddRows = false;
            this.SC_dataGridView.AllowUserToOrderColumns = true;
            this.SC_dataGridView.AllowUserToResizeRows = false;
            this.SC_dataGridView.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.SC_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.SC_dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.name,
            this.value,
            this.color,
            this.count});
            this.SC_dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SC_dataGridView.Location = new System.Drawing.Point(0, 31);
            this.SC_dataGridView.Name = "SC_dataGridView";
            this.SC_dataGridView.RowTemplate.Height = 23;
            this.SC_dataGridView.Size = new System.Drawing.Size(690, 345);
            this.SC_dataGridView.TabIndex = 9;
            // 
            // count
            // 
            this.count.HeaderText = "像元数（近似值）";
            this.count.Name = "count";
            this.count.ReadOnly = true;
            this.count.Width = 129;
            // 
            // color
            // 
            this.color.HeaderText = "样本颜色";
            this.color.Name = "color";
            this.color.ReadOnly = true;
            this.color.Width = 130;
            // 
            // value
            // 
            this.value.HeaderText = "样本值";
            this.value.Name = "value";
            this.value.ReadOnly = true;
            this.value.Width = 129;
            // 
            // name
            // 
            this.name.HeaderText = "样本名称";
            this.name.Name = "name";
            this.name.Width = 130;
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Width = 129;
            // 
            // ROIToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 376);
            this.Controls.Add(this.SC_dataGridView);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "ROIToolForm";
            this.Text = "SupervisedClassification";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SupervisedClassification_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.SC_barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LayerCombox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SC_dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager SC_barManager;
        private DevExpress.XtraBars.Bar SC_ToolBar;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem SC_SelectSampleButton;
        private DevExpress.XtraBars.BarButtonItem SC_OpenSamplefile;
        private DevExpress.XtraBars.BarButtonItem SC_SaveSample;
        private DevExpress.XtraBars.BarButtonItem SC_MergeSample;
        private DevExpress.XtraBars.BarButtonItem SC_SplitSample;
        private DevExpress.XtraBars.BarButtonItem SC_RemoveSample;
        private DevExpress.XtraBars.BarButtonItem SC_ClearSamples;
        private DevExpress.XtraBars.BarButtonItem SC_CreateSampleFiles;
        private DevExpress.XtraBars.BarEditItem SampleLayerCombox;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox LayerCombox;
        private DevExpress.XtraBars.BarStaticItem SC_LayerName;
        private System.Windows.Forms.DataGridView SC_dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn value;
        private System.Windows.Forms.DataGridViewTextBoxColumn color;
        private System.Windows.Forms.DataGridViewTextBoxColumn count;
    }
}