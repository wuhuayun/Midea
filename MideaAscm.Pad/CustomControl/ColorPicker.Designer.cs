namespace MideaAscm.Pad.CustomControl
{
    partial class ColorPicker
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.colorPanel = new System.Windows.Forms.Panel();
            this.listWeb = new System.Windows.Forms.ListBox();
            this.listSystem = new System.Windows.Forms.ListBox();
            this.tabStrip1 = new DevComponents.DotNetBar.TabStrip();
            this.tabWeb = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabCustom = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabSystem = new DevComponents.DotNetBar.TabItem(this.components);
            this.colorPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // colorPanel
            // 
            this.colorPanel.Controls.Add(this.listWeb);
            this.colorPanel.Controls.Add(this.listSystem);
            this.colorPanel.Controls.Add(this.tabStrip1);
            this.colorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.colorPanel.Location = new System.Drawing.Point(0, 0);
            this.colorPanel.Name = "colorPanel";
            this.colorPanel.Size = new System.Drawing.Size(200, 182);
            this.colorPanel.TabIndex = 2;
            // 
            // listWeb
            // 
            this.listWeb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listWeb.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listWeb.IntegralHeight = false;
            this.listWeb.Location = new System.Drawing.Point(0, 0);
            this.listWeb.Name = "listWeb";
            this.listWeb.Size = new System.Drawing.Size(200, 158);
            this.listWeb.TabIndex = 0;
            this.listWeb.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.DrawWebItem);
            this.listWeb.SelectedIndexChanged += new System.EventHandler(this.WebSelectionChange);
            // 
            // listSystem
            // 
            this.listSystem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listSystem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listSystem.IntegralHeight = false;
            this.listSystem.Location = new System.Drawing.Point(0, 0);
            this.listSystem.Name = "listSystem";
            this.listSystem.Size = new System.Drawing.Size(200, 158);
            this.listSystem.TabIndex = 2;
            this.listSystem.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.DrawSystemItem);
            this.listSystem.SelectedIndexChanged += new System.EventHandler(this.SystemSelectionChange);
            // 
            // tabStrip1
            // 
            this.tabStrip1.AutoHideSystemBox = true;
            this.tabStrip1.AutoSelectAttachedControl = true;
            this.tabStrip1.CanReorderTabs = true;
            this.tabStrip1.CloseButtonVisible = false;
            this.tabStrip1.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabStrip1.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.tabStrip1.Location = new System.Drawing.Point(0, 158);
            this.tabStrip1.Name = "tabStrip1";
            this.tabStrip1.SelectedTab = this.tabSystem;
            this.tabStrip1.ShowFocusRectangle = false;
            this.tabStrip1.Size = new System.Drawing.Size(200, 24);
            this.tabStrip1.Style = DevComponents.DotNetBar.eTabStripStyle.Office2003;
            this.tabStrip1.TabIndex = 2;
            this.tabStrip1.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabStrip1.Tabs.Add(this.tabCustom);
            this.tabStrip1.Tabs.Add(this.tabWeb);
            this.tabStrip1.Tabs.Add(this.tabSystem);
            this.tabStrip1.Text = "tabStrip1";
            // 
            // tabWeb
            // 
            this.tabWeb.AttachedControl = this.listWeb;
            this.tabWeb.Name = "tabWeb";
            this.tabWeb.Text = "Web";
            // 
            // tabCustom
            // 
            this.tabCustom.AttachedControl = this.colorPanel;
            this.tabCustom.Name = "tabCustom";
            this.tabCustom.Text = "Custom";
            // 
            // tabSystem
            // 
            this.tabSystem.AttachedControl = this.listSystem;
            this.tabSystem.Name = "tabSystem";
            this.tabSystem.Text = "System";
            // 
            // ColorPicker
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.colorPanel);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "ColorPicker";
            this.Size = new System.Drawing.Size(200, 182);
            this.colorPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel colorPanel;
        internal DevComponents.DotNetBar.TabStrip tabStrip1;
        private DevComponents.DotNetBar.TabItem tabSystem;
        private DevComponents.DotNetBar.TabItem tabCustom;
        private DevComponents.DotNetBar.TabItem tabWeb;
        private System.Windows.Forms.ListBox listSystem;
        private System.Windows.Forms.ListBox listWeb;
    }
}
