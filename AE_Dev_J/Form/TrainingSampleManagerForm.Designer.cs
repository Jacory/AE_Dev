namespace AE_Dev_J.Form
{
    partial class TrainingSampleManagerForm
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
            this.TrainingSample_BarManager = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.TrainingSample_Bar = new DevExpress.XtraBars.Bar();
            this.Sample_Clear = new DevExpress.XtraBars.BarButtonItem();
            this.Sample_Open = new DevExpress.XtraBars.BarButtonItem();
            this.Sample_Save = new DevExpress.XtraBars.BarButtonItem();
            this.Sample_Merge = new DevExpress.XtraBars.BarButtonItem();
            this.Sample_Split = new DevExpress.XtraBars.BarButtonItem();
            this.Sample_Delet = new DevExpress.XtraBars.BarButtonItem();
            this.Sample_CreateFile = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.TrainingSample_BarManager)).BeginInit();
            this.SuspendLayout();
            // 
            // TrainingSample_BarManager
            // 
            this.TrainingSample_BarManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.TrainingSample_Bar});
            this.TrainingSample_BarManager.DockControls.Add(this.barDockControlTop);
            this.TrainingSample_BarManager.DockControls.Add(this.barDockControlBottom);
            this.TrainingSample_BarManager.DockControls.Add(this.barDockControlLeft);
            this.TrainingSample_BarManager.DockControls.Add(this.barDockControlRight);
            this.TrainingSample_BarManager.Form = this;
            this.TrainingSample_BarManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.Sample_Clear,
            this.Sample_Open,
            this.Sample_Save,
            this.Sample_Merge,
            this.Sample_Split,
            this.Sample_Delet,
            this.Sample_CreateFile});
            this.TrainingSample_BarManager.MaxItemId = 7;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(508, 31);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 307);
            this.barDockControlBottom.Size = new System.Drawing.Size(508, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 31);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 276);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(508, 31);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 276);
            // 
            // TrainingSample_Bar
            // 
            this.TrainingSample_Bar.BarName = "Tools";
            this.TrainingSample_Bar.DockCol = 0;
            this.TrainingSample_Bar.DockRow = 0;
            this.TrainingSample_Bar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.TrainingSample_Bar.FloatLocation = new System.Drawing.Point(81, 150);
            this.TrainingSample_Bar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.Sample_Clear),
            new DevExpress.XtraBars.LinkPersistInfo(this.Sample_Open),
            new DevExpress.XtraBars.LinkPersistInfo(this.Sample_Save),
            new DevExpress.XtraBars.LinkPersistInfo(this.Sample_Merge),
            new DevExpress.XtraBars.LinkPersistInfo(this.Sample_Split),
            new DevExpress.XtraBars.LinkPersistInfo(this.Sample_Delet),
            new DevExpress.XtraBars.LinkPersistInfo(this.Sample_CreateFile)});
            this.TrainingSample_Bar.Text = "Tools";
            // 
            // Sample_Clear
            // 
            this.Sample_Clear.Caption = "清空";
            this.Sample_Clear.Id = 0;
            this.Sample_Clear.Name = "Sample_Clear";
            this.Sample_Clear.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Sample_Clear_ItemClick);
            // 
            // Sample_Open
            // 
            this.Sample_Open.Caption = "打开";
            this.Sample_Open.Id = 1;
            this.Sample_Open.Name = "Sample_Open";
            // 
            // Sample_Save
            // 
            this.Sample_Save.Caption = "保存";
            this.Sample_Save.Id = 2;
            this.Sample_Save.Name = "Sample_Save";
            // 
            // Sample_Merge
            // 
            this.Sample_Merge.Caption = "合并";
            this.Sample_Merge.Id = 3;
            this.Sample_Merge.Name = "Sample_Merge";
            // 
            // Sample_Split
            // 
            this.Sample_Split.Caption = "分离";
            this.Sample_Split.Id = 4;
            this.Sample_Split.Name = "Sample_Split";
            // 
            // Sample_Delet
            // 
            this.Sample_Delet.Caption = "删除";
            this.Sample_Delet.Id = 5;
            this.Sample_Delet.Name = "Sample_Delet";
            // 
            // Sample_CreateFile
            // 
            this.Sample_CreateFile.Caption = "生成文件";
            this.Sample_CreateFile.Id = 6;
            this.Sample_CreateFile.Name = "Sample_CreateFile";
            // 
            // TrainingSampleManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 307);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "TrainingSampleManagerForm";
            this.Text = "TrainingSampleManagerForm";
            ((System.ComponentModel.ISupportInitialize)(this.TrainingSample_BarManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager TrainingSample_BarManager;
        private DevExpress.XtraBars.Bar TrainingSample_Bar;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem Sample_Clear;
        private DevExpress.XtraBars.BarButtonItem Sample_Open;
        private DevExpress.XtraBars.BarButtonItem Sample_Save;
        private DevExpress.XtraBars.BarButtonItem Sample_Merge;
        private DevExpress.XtraBars.BarButtonItem Sample_Split;
        private DevExpress.XtraBars.BarButtonItem Sample_Delet;
        private DevExpress.XtraBars.BarButtonItem Sample_CreateFile;
    }
}