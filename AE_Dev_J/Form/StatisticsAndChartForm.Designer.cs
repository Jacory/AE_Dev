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
            this.repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.StatisticsChart_dockManager = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.chart_split = new DevExpress.XtraEditors.SplitContainerControl();
            this.StatisticsChart_chart1 = new DevExpress.XtraCharts.ChartControl();
            ((System.ComponentModel.ISupportInitialize)(this.StatisticsAndChart_vGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StatisticsChart_dockManager)).BeginInit();
            this.dockPanel1.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_split)).BeginInit();
            this.chart_split.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StatisticsChart_chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // StatisticsAndChart_vGridControl
            // 
            this.StatisticsAndChart_vGridControl.Cursor = System.Windows.Forms.Cursors.Hand;
            this.StatisticsAndChart_vGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StatisticsAndChart_vGridControl.LayoutStyle = DevExpress.XtraVerticalGrid.LayoutViewStyle.BandsView;
            this.StatisticsAndChart_vGridControl.Location = new System.Drawing.Point(0, 0);
            this.StatisticsAndChart_vGridControl.Name = "StatisticsAndChart_vGridControl";
            this.StatisticsAndChart_vGridControl.OptionsBehavior.Editable = false;
            this.StatisticsAndChart_vGridControl.OptionsBehavior.ShowEditorOnMouseUp = true;
            this.StatisticsAndChart_vGridControl.OptionsMenu.EnableContextMenu = true;
            this.StatisticsAndChart_vGridControl.OptionsView.AutoScaleBands = true;
            this.StatisticsAndChart_vGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemButtonEdit1});
            this.StatisticsAndChart_vGridControl.ShowButtonMode = DevExpress.XtraVerticalGrid.ShowButtonModeEnum.ShowAlways;
            this.StatisticsAndChart_vGridControl.Size = new System.Drawing.Size(238, 353);
            this.StatisticsAndChart_vGridControl.TabIndex = 0;
            this.StatisticsAndChart_vGridControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.StatisticsAndChart_vGridControl_MouseClick);
            // 
            // repositoryItemButtonEdit1
            // 
            this.repositoryItemButtonEdit1.AutoHeight = false;
            this.repositoryItemButtonEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
            // 
            // StatisticsChart_dockManager
            // 
            this.StatisticsChart_dockManager.Form = this;
            this.StatisticsChart_dockManager.HiddenPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanel1});
            this.StatisticsChart_dockManager.TopZIndexControls.AddRange(new string[] {
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
            this.dockPanel1.OriginalSize = new System.Drawing.Size(246, 200);
            this.dockPanel1.SavedDock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.dockPanel1.SavedIndex = 0;
            this.dockPanel1.Size = new System.Drawing.Size(246, 380);
            this.dockPanel1.Text = "dockPanel1";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.StatisticsAndChart_vGridControl);
            this.dockPanel1_Container.Location = new System.Drawing.Point(4, 23);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(238, 353);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // chart_split
            // 
            this.chart_split.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart_split.Horizontal = false;
            this.chart_split.Location = new System.Drawing.Point(246, 0);
            this.chart_split.Name = "chart_split";
            this.chart_split.Panel1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.chart_split.Panel1.Controls.Add(this.StatisticsChart_chart1);
            this.chart_split.Panel1.Text = "Panel1";
            this.chart_split.Panel2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.chart_split.Panel2.Text = "Panel2";
            this.chart_split.Size = new System.Drawing.Size(416, 380);
            this.chart_split.SplitterPosition = 106;
            this.chart_split.TabIndex = 3;
            this.chart_split.Text = "splitContainerControl1";
            // 
            // StatisticsChart_chart1
            // 
            this.StatisticsChart_chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StatisticsChart_chart1.Location = new System.Drawing.Point(0, 0);
            this.StatisticsChart_chart1.Name = "StatisticsChart_chart1";
            this.StatisticsChart_chart1.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.StatisticsChart_chart1.Size = new System.Drawing.Size(412, 102);
            this.StatisticsChart_chart1.TabIndex = 0;
            // 
            // StatisticsAndChartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 380);
            this.Controls.Add(this.chart_split);
            this.Controls.Add(this.dockPanel1);
            this.Name = "StatisticsAndChartForm";
            this.Text = "StatisticsAndChart";
            this.Load += new System.EventHandler(this.StatisticsAndChartForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.StatisticsAndChart_vGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StatisticsChart_dockManager)).EndInit();
            this.dockPanel1.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart_split)).EndInit();
            this.chart_split.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.StatisticsChart_chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraVerticalGrid.VGridControl StatisticsAndChart_vGridControl;
        private DevExpress.XtraBars.Docking.DockManager StatisticsChart_dockManager;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraEditors.SplitContainerControl chart_split;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
        private DevExpress.XtraCharts.ChartControl StatisticsChart_chart1;


    }
}