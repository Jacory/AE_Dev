namespace AE_Dev_J.Form
{
    partial class StatisticsAndChartForm
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
            this.StatisticsAndChart_vGridControl = new DevExpress.XtraVerticalGrid.VGridControl();
            this.row = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            ((System.ComponentModel.ISupportInitialize)(this.StatisticsAndChart_vGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.dockPanel1.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatisticsAndChart_vGridControl
            // 
            this.StatisticsAndChart_vGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StatisticsAndChart_vGridControl.LayoutStyle = DevExpress.XtraVerticalGrid.LayoutViewStyle.BandsView;
            this.StatisticsAndChart_vGridControl.Location = new System.Drawing.Point(0, 0);
            this.StatisticsAndChart_vGridControl.Name = "StatisticsAndChart_vGridControl";
            this.StatisticsAndChart_vGridControl.OptionsView.AutoScaleBands = true;
            this.StatisticsAndChart_vGridControl.Rows.AddRange(new DevExpress.XtraVerticalGrid.Rows.BaseRow[] {
            this.row});
            this.StatisticsAndChart_vGridControl.Size = new System.Drawing.Size(331, 353);
            this.StatisticsAndChart_vGridControl.TabIndex = 0;
            // 
            // row
            // 
            this.row.Appearance.Options.UseTextOptions = true;
            this.row.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.row.Height = 17;
            this.row.Name = "row";
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.HiddenPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanel1});
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl"});
            // 
            // dockPanel1
            // 
            this.dockPanel1.Controls.Add(this.dockPanel1_Container);
            this.dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.dockPanel1.ID = new System.Guid("9efd1292-eb9a-4199-8b16-fbf90959178d");
            this.dockPanel1.Location = new System.Drawing.Point(0, 0);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.OriginalSize = new System.Drawing.Size(339, 200);
            this.dockPanel1.SavedDock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.dockPanel1.SavedIndex = 0;
            this.dockPanel1.Size = new System.Drawing.Size(339, 380);
            this.dockPanel1.Text = "dockPanel1";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.StatisticsAndChart_vGridControl);
            this.dockPanel1_Container.Location = new System.Drawing.Point(4, 23);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(331, 353);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.Location = new System.Drawing.Point(339, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(323, 380);
            this.splitContainerControl1.SplitterPosition = 135;
            this.splitContainerControl1.TabIndex = 3;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // StatisticsAndChartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 380);
            this.Controls.Add(this.splitContainerControl1);
            this.Controls.Add(this.dockPanel1);
            this.Name = "StatisticsAndChartForm";
            this.Text = "StatisticsAndChart";
            this.Load += new System.EventHandler(this.StatisticsAndChartForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.StatisticsAndChart_vGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.dockPanel1.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraVerticalGrid.VGridControl StatisticsAndChart_vGridControl;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow row;


    }
}