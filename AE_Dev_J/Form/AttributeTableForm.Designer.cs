namespace AE_Dev_J.Form
{
    partial class AttributeTableForm
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
            this.att_gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.attForm_dockManager = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.filter_dockPanel = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.tool_dockPanel = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel2_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            ((System.ComponentModel.ISupportInitialize)(this.att_gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.attForm_dockManager)).BeginInit();
            this.filter_dockPanel.SuspendLayout();
            this.tool_dockPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // att_gridControl
            // 
            this.att_gridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.att_gridControl.Location = new System.Drawing.Point(0, 0);
            this.att_gridControl.MainView = this.gridView1;
            this.att_gridControl.Name = "att_gridControl";
            this.att_gridControl.Size = new System.Drawing.Size(627, 355);
            this.att_gridControl.TabIndex = 8;
            this.att_gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.att_gridControl;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            // 
            // splitterControl1
            // 
            this.splitterControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitterControl1.Location = new System.Drawing.Point(144, 139);
            this.splitterControl1.MinSize = 20;
            this.splitterControl1.Name = "splitterControl1";
            this.splitterControl1.Size = new System.Drawing.Size(633, 5);
            this.splitterControl1.TabIndex = 1;
            this.splitterControl1.TabStop = false;
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "ribbonPage1";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.ShowCaptionButton = false;
            this.ribbonPageGroup1.Text = "ribbonPageGroup1";
            // 
            // attForm_dockManager
            // 
            this.attForm_dockManager.Form = this;
            this.attForm_dockManager.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.filter_dockPanel,
            this.tool_dockPanel});
            this.attForm_dockManager.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl"});
            // 
            // filter_dockPanel
            // 
            this.filter_dockPanel.Controls.Add(this.dockPanel1_Container);
            this.filter_dockPanel.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.filter_dockPanel.ID = new System.Guid("172969ba-b28a-4096-8127-f70c9b6f0b96");
            this.filter_dockPanel.Location = new System.Drawing.Point(0, 0);
            this.filter_dockPanel.Name = "filter_dockPanel";
            this.filter_dockPanel.OriginalSize = new System.Drawing.Size(144, 200);
            this.filter_dockPanel.Size = new System.Drawing.Size(144, 523);
            this.filter_dockPanel.Text = "Filter";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Location = new System.Drawing.Point(4, 23);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(136, 496);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // tool_dockPanel
            // 
            this.tool_dockPanel.Controls.Add(this.dockPanel2_Container);
            this.tool_dockPanel.Dock = DevExpress.XtraBars.Docking.DockingStyle.Top;
            this.tool_dockPanel.ID = new System.Guid("d57e8816-6859-4077-be05-c7787179e0ac");
            this.tool_dockPanel.Location = new System.Drawing.Point(144, 0);
            this.tool_dockPanel.Name = "tool_dockPanel";
            this.tool_dockPanel.OriginalSize = new System.Drawing.Size(200, 139);
            this.tool_dockPanel.Size = new System.Drawing.Size(633, 139);
            this.tool_dockPanel.Text = "Tool";
            // 
            // dockPanel2_Container
            // 
            this.dockPanel2_Container.Location = new System.Drawing.Point(4, 23);
            this.dockPanel2_Container.Name = "dockPanel2_Container";
            this.dockPanel2_Container.Size = new System.Drawing.Size(625, 112);
            this.dockPanel2_Container.TabIndex = 0;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(144, 139);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(633, 384);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.att_gridControl);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(627, 355);
            this.xtraTabPage1.Text = "xtraTabPage1";
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(627, 355);
            this.xtraTabPage2.Text = "xtraTabPage2";
            // 
            // AttributeTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 523);
            this.Controls.Add(this.splitterControl1);
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.tool_dockPanel);
            this.Controls.Add(this.filter_dockPanel);
            this.IsMdiContainer = true;
            this.Name = "AttributeTableForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AttributeTableForm";
            this.Load += new System.EventHandler(this.AttributeTableForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.att_gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.attForm_dockManager)).EndInit();
            this.filter_dockPanel.ResumeLayout(false);
            this.tool_dockPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SplitterControl splitterControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.Docking.DockManager attForm_dockManager;
        private DevExpress.XtraBars.Docking.DockPanel tool_dockPanel;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel2_Container;
        private DevExpress.XtraBars.Docking.DockPanel filter_dockPanel;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraGrid.GridControl att_gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;

    }
}