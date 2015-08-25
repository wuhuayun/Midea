namespace MideaAscm.AM.IoC
{
    partial class frmMain
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
            this.superTabControl2 = new DevComponents.DotNetBar.SuperTabControl();
            this.superTabControlPanel2 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listViewMsg = new System.Windows.Forms.ListView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnIoCStop = new System.Windows.Forms.Button();
            this.btnIoCPlay = new System.Windows.Forms.Button();
            this.superTabItemIoC = new DevComponents.DotNetBar.SuperTabItem();
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl2)).BeginInit();
            this.superTabControl2.SuspendLayout();
            this.superTabControlPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // superTabControl2
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.superTabControl2.ControlBox.CloseBox.Name = "";
            // 
            // 
            // 
            this.superTabControl2.ControlBox.MenuBox.Name = "";
            this.superTabControl2.ControlBox.Name = "";
            this.superTabControl2.ControlBox.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabControl2.ControlBox.MenuBox,
            this.superTabControl2.ControlBox.CloseBox});
            this.superTabControl2.Controls.Add(this.superTabControlPanel2);
            this.superTabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControl2.FixedTabSize = new System.Drawing.Size(150, 0);
            this.superTabControl2.Location = new System.Drawing.Point(0, 0);
            this.superTabControl2.Name = "superTabControl2";
            this.superTabControl2.ReorderTabsEnabled = true;
            this.superTabControl2.SelectedTabFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.superTabControl2.SelectedTabIndex = 0;
            this.superTabControl2.Size = new System.Drawing.Size(814, 444);
            this.superTabControl2.TabAlignment = DevComponents.DotNetBar.eTabStripAlignment.Left;
            this.superTabControl2.TabFont = new System.Drawing.Font("Segoe UI", 9F);
            this.superTabControl2.TabIndex = 2;
            this.superTabControl2.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabItemIoC});
            this.superTabControl2.TabStyle = DevComponents.DotNetBar.eSuperTabStyle.Office2010BackstageBlue;
            this.superTabControl2.TabVerticalSpacing = 3;
            this.superTabControl2.Text = "superTabControl2";
            // 
            // superTabControlPanel2
            // 
            this.superTabControlPanel2.Controls.Add(this.splitContainer1);
            this.superTabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel2.Location = new System.Drawing.Point(152, 0);
            this.superTabControlPanel2.Name = "superTabControlPanel2";
            this.superTabControlPanel2.Size = new System.Drawing.Size(662, 444);
            this.superTabControlPanel2.TabIndex = 1;
            this.superTabControlPanel2.TabItem = this.superTabItemIoC;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listViewMsg);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(662, 444);
            this.splitContainer1.SplitterDistance = 335;
            this.splitContainer1.TabIndex = 0;
            // 
            // listViewMsg
            // 
            this.listViewMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewMsg.Location = new System.Drawing.Point(0, 0);
            this.listViewMsg.Name = "listViewMsg";
            this.listViewMsg.Size = new System.Drawing.Size(662, 335);
            this.listViewMsg.TabIndex = 0;
            this.listViewMsg.UseCompatibleStateImageBehavior = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnIoCStop);
            this.groupBox1.Controls.Add(this.btnIoCPlay);
            this.groupBox1.Location = new System.Drawing.Point(3, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(656, 97);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "卸货点服务";
            // 
            // btnIoCStop
            // 
            this.btnIoCStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIoCStop.Location = new System.Drawing.Point(237, 37);
            this.btnIoCStop.Name = "btnIoCStop";
            this.btnIoCStop.Size = new System.Drawing.Size(167, 42);
            this.btnIoCStop.TabIndex = 1;
            this.btnIoCStop.Text = "关闭卸货点服务";
            this.btnIoCStop.UseVisualStyleBackColor = true;
            // 
            // btnIoCPlay
            // 
            this.btnIoCPlay.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnIoCPlay.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIoCPlay.Location = new System.Drawing.Point(42, 37);
            this.btnIoCPlay.Name = "btnIoCPlay";
            this.btnIoCPlay.Size = new System.Drawing.Size(167, 42);
            this.btnIoCPlay.TabIndex = 0;
            this.btnIoCPlay.Text = "开启卸货点服务";
            this.btnIoCPlay.UseVisualStyleBackColor = true;
            // 
            // superTabItemIoC
            // 
            this.superTabItemIoC.AttachedControl = this.superTabControlPanel2;
            this.superTabItemIoC.GlobalItem = false;
            this.superTabItemIoC.Name = "superTabItemIoC";
            this.superTabItemIoC.Text = "卸货点控制";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 444);
            this.Controls.Add(this.superTabControl2);
            this.Name = "frmMain";
            this.Text = "Ascmx卸货点控制服务程序";
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl2)).EndInit();
            this.superTabControl2.ResumeLayout(false);
            this.superTabControlPanel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.SuperTabControl superTabControl2;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel2;
        private DevComponents.DotNetBar.SuperTabItem superTabItemIoC;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView listViewMsg;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnIoCStop;
        private System.Windows.Forms.Button btnIoCPlay;
    }
}