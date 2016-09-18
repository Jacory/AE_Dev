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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AttributeTableForm));
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.attForm_dockManager = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.tool_dockPanel = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel2_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.att_dockPaneltools = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.navBarControl1 = new DevExpress.XtraNavBar.NavBarControl();
            this.att_navBarGroup1 = new DevExpress.XtraNavBar.NavBarGroup();
            this.att_chartbaritem = new DevExpress.XtraNavBar.NavBarItem();
            this.att_navBarGroup2 = new DevExpress.XtraNavBar.NavBarGroup();
            this.att_xtraTabControl = new DevExpress.XtraTab.XtraTabControl();
            ((System.ComponentModel.ISupportInitialize)(this.attForm_dockManager)).BeginInit();
            this.tool_dockPanel.SuspendLayout();
            this.att_dockPaneltools.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.att_xtraTabControl)).BeginInit();
            this.SuspendLayout();
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
            this.attForm_dockManager.HiddenPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.tool_dockPanel});
            this.attForm_dockManager.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.att_dockPaneltools});
            this.attForm_dockManager.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl"});
            // 
            // tool_dockPanel
            // 
            this.tool_dockPanel.Controls.Add(this.dockPanel2_Container);
            this.tool_dockPanel.Dock = DevExpress.XtraBars.Docking.DockingStyle.Top;
            this.tool_dockPanel.FloatVertical = true;
            this.tool_dockPanel.ID = new System.Guid("d57e8816-6859-4077-be05-c7787179e0ac");
            this.tool_dockPanel.Location = new System.Drawing.Point(97, 0);
            this.tool_dockPanel.Name = "tool_dockPanel";
            this.tool_dockPanel.OriginalSize = new System.Drawing.Size(200, 265);
            this.tool_dockPanel.SavedDock = DevExpress.XtraBars.Docking.DockingStyle.Top;
            this.tool_dockPanel.SavedIndex = 1;
            this.tool_dockPanel.Size = new System.Drawing.Size(680, 265);
            this.tool_dockPanel.Text = "Tool";
            this.tool_dockPanel.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
            // 
            // dockPanel2_Container
            // 
            this.dockPanel2_Container.Location = new System.Drawing.Point(4, 23);
            this.dockPanel2_Container.Name = "dockPanel2_Container";
            this.dockPanel2_Container.Size = new System.Drawing.Size(672, 238);
            this.dockPanel2_Container.TabIndex = 0;
            // 
            // att_dockPaneltools
            // 
            this.att_dockPaneltools.Controls.Add(this.dockPanel1_Container);
            this.att_dockPaneltools.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.att_dockPaneltools.ID = new System.Guid("172969ba-b28a-4096-8127-f70c9b6f0b96");
            this.att_dockPaneltools.Location = new System.Drawing.Point(0, 0);
            this.att_dockPaneltools.Name = "att_dockPaneltools";
            this.att_dockPaneltools.Options.ShowCloseButton = false;
            this.att_dockPaneltools.OriginalSize = new System.Drawing.Size(193, 200);
            this.att_dockPaneltools.Size = new System.Drawing.Size(193, 523);
            this.att_dockPaneltools.Text = "Tools";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.navBarControl1);
            this.dockPanel1_Container.Location = new System.Drawing.Point(4, 23);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(185, 496);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // navBarControl1
            // 
            this.navBarControl1.ActiveGroup = this.att_navBarGroup1;
            this.navBarControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navBarControl1.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.att_navBarGroup1,
            this.att_navBarGroup2});
            this.navBarControl1.Items.AddRange(new DevExpress.XtraNavBar.NavBarItem[] {
            this.att_chartbaritem});
            this.navBarControl1.Location = new System.Drawing.Point(0, 0);
            this.navBarControl1.Name = "navBarControl1";
            this.navBarControl1.OptionsNavPane.ExpandedWidth = 185;
            this.navBarControl1.Size = new System.Drawing.Size(185, 496);
            this.navBarControl1.TabIndex = 0;
            this.navBarControl1.Text = "navBarControl1";
            // 
            // att_navBarGroup1
            // 
            this.att_navBarGroup1.Caption = "";
            this.att_navBarGroup1.Expanded = true;
            this.att_navBarGroup1.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.att_chartbaritem)});
            this.att_navBarGroup1.Name = "att_navBarGroup1";
            // 
            // att_chartbaritem
            // 
            this.att_chartbaritem.Caption = "Statistics And Chart";
            this.att_chartbaritem.Name = "att_chartbaritem";
            this.att_chartbaritem.SmallImage = ((System.Drawing.Image)(resources.GetObject("att_chartbaritem.SmallImage")));
            this.att_chartbaritem.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.att_chartbaritem_LinkClicked);
            // 
            // att_navBarGroup2
            // 
            this.att_navBarGroup2.Caption = "";
            this.att_navBarGroup2.Expanded = true;
            this.att_navBarGroup2.Name = "att_navBarGroup2";
            // 
            // att_xtraTabControl
            // 
            this.att_xtraTabControl.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InAllTabPageHeaders;
            this.att_xtraTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.att_xtraTabControl.HeaderLocation = DevExpress.XtraTab.TabHeaderLocation.Bottom;
            this.att_xtraTabControl.Location = new System.Drawing.Point(193, 0);
            this.att_xtraTabControl.Name = "att_xtraTabControl";
            this.att_xtraTabControl.Size = new System.Drawing.Size(584, 523);
            this.att_xtraTabControl.TabIndex = 0;
            this.att_xtraTabControl.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.att_xtraTabControl_SelectedPageChanged);
            this.att_xtraTabControl.CloseButtonClick += new System.EventHandler(this.xtraTabControl_CloseButtonClick);
            // 
            // AttributeTableForm
            // 
            this.ActiveGlowColor = System.Drawing.Color.White;
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 523);
            this.Controls.Add(this.att_xtraTabControl);
            this.Controls.Add(this.att_dockPaneltools);
            this.Name = "AttributeTableForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AttributeTableForm";
            this.Load += new System.EventHandler(this.AttributeTableForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.attForm_dockManager)).EndInit();
            this.tool_dockPanel.ResumeLayout(false);
            this.att_dockPaneltools.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.att_xtraTabControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.Docking.DockManager attForm_dockManager;
        private DevExpress.XtraBars.Docking.DockPanel tool_dockPanel;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel2_Container;
        private DevExpress.XtraBars.Docking.DockPanel att_dockPaneltools;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraTab.XtraTabControl att_xtraTabControl;
        private DevExpress.XtraNavBar.NavBarControl navBarControl1;
        private DevExpress.XtraNavBar.NavBarGroup att_navBarGroup1;
        private DevExpress.XtraNavBar.NavBarGroup att_navBarGroup2;
        private DevExpress.XtraNavBar.NavBarItem att_chartbaritem;

    }
}