namespace MideaAscm.Server
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.button_setup = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusCompanyInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripStatusTel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSplitButton2 = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripStatusSystemTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.listBoxLog = new System.Windows.Forms.ListBox();
            this.button_RfidReader_Run = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.timer_unloadingPoint = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_RfidReader_Stop = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_unloadingPoint_Stop = new System.Windows.Forms.Button();
            this.button_unloadingPoint_Run = new System.Windows.Forms.Button();
            this.timer_readingHead = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_setup
            // 
            this.button_setup.Location = new System.Drawing.Point(20, 364);
            this.button_setup.Name = "button_setup";
            this.button_setup.Size = new System.Drawing.Size(64, 24);
            this.button_setup.TabIndex = 36;
            this.button_setup.Text = "配置";
            this.button_setup.Click += new System.EventHandler(this.button_setup_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusCompanyInfo,
            this.toolStripSplitButton1,
            this.toolStripStatusTel,
            this.toolStripSplitButton2,
            this.toolStripStatusSystemTime});
            this.statusStrip1.Location = new System.Drawing.Point(0, 400);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(560, 22);
            this.statusStrip1.TabIndex = 37;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusCompanyInfo
            // 
            this.toolStripStatusCompanyInfo.Name = "toolStripStatusCompanyInfo";
            this.toolStripStatusCompanyInfo.Size = new System.Drawing.Size(56, 17);
            this.toolStripStatusCompanyInfo.Text = "东莞思谷";
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(16, 20);
            this.toolStripSplitButton1.Text = "toolStripSplitButton1";
            // 
            // toolStripStatusTel
            // 
            this.toolStripStatusTel.Name = "toolStripStatusTel";
            this.toolStripStatusTel.Size = new System.Drawing.Size(64, 17);
            this.toolStripStatusTel.Text = "12345678";
            // 
            // toolStripSplitButton2
            // 
            this.toolStripSplitButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton2.Name = "toolStripSplitButton2";
            this.toolStripSplitButton2.Size = new System.Drawing.Size(16, 20);
            this.toolStripSplitButton2.Text = "toolStripSplitButton2";
            // 
            // toolStripStatusSystemTime
            // 
            this.toolStripStatusSystemTime.Name = "toolStripStatusSystemTime";
            this.toolStripStatusSystemTime.Size = new System.Drawing.Size(0, 17);
            this.toolStripStatusSystemTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // listBoxLog
            // 
            this.listBoxLog.Dock = System.Windows.Forms.DockStyle.Top;
            this.listBoxLog.FormattingEnabled = true;
            this.listBoxLog.ItemHeight = 12;
            this.listBoxLog.Location = new System.Drawing.Point(0, 0);
            this.listBoxLog.Name = "listBoxLog";
            this.listBoxLog.Size = new System.Drawing.Size(560, 208);
            this.listBoxLog.TabIndex = 38;
            // 
            // button_RfidReader_Run
            // 
            this.button_RfidReader_Run.Image = ((System.Drawing.Image)(resources.GetObject("button_RfidReader_Run.Image")));
            this.button_RfidReader_Run.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_RfidReader_Run.Location = new System.Drawing.Point(12, 24);
            this.button_RfidReader_Run.Name = "button_RfidReader_Run";
            this.button_RfidReader_Run.Size = new System.Drawing.Size(148, 32);
            this.button_RfidReader_Run.TabIndex = 40;
            this.button_RfidReader_Run.Text = "开启RFID读写器服务";
            this.button_RfidReader_Run.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_RfidReader_Run.UseVisualStyleBackColor = true;
            this.button_RfidReader_Run.Click += new System.EventHandler(this.button_RfidReader_Run_Click);
            // 
            // btnExit
            // 
            this.btnExit.Image = ((System.Drawing.Image)(resources.GetObject("btnExit.Image")));
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(476, 372);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(79, 24);
            this.btnExit.TabIndex = 39;
            this.btnExit.Text = "退出服务";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // timer_unloadingPoint
            // 
            this.timer_unloadingPoint.Enabled = true;
            this.timer_unloadingPoint.Interval = 10000;
            this.timer_unloadingPoint.Tick += new System.EventHandler(this.timer_unloadingPoint_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button_RfidReader_Stop);
            this.groupBox1.Controls.Add(this.button_RfidReader_Run);
            this.groupBox1.Location = new System.Drawing.Point(8, 216);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(544, 60);
            this.groupBox1.TabIndex = 41;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "RFID读写器";
            // 
            // button_RfidReader_Stop
            // 
            this.button_RfidReader_Stop.Enabled = false;
            this.button_RfidReader_Stop.Image = ((System.Drawing.Image)(resources.GetObject("button_RfidReader_Stop.Image")));
            this.button_RfidReader_Stop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_RfidReader_Stop.Location = new System.Drawing.Point(164, 24);
            this.button_RfidReader_Stop.Name = "button_RfidReader_Stop";
            this.button_RfidReader_Stop.Size = new System.Drawing.Size(144, 32);
            this.button_RfidReader_Stop.TabIndex = 41;
            this.button_RfidReader_Stop.Text = "停止RFID读写器服务";
            this.button_RfidReader_Stop.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_RfidReader_Stop.UseVisualStyleBackColor = true;
            this.button_RfidReader_Stop.Click += new System.EventHandler(this.button_RfidReader_Stop_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button_unloadingPoint_Stop);
            this.groupBox2.Controls.Add(this.button_unloadingPoint_Run);
            this.groupBox2.Location = new System.Drawing.Point(8, 284);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(544, 60);
            this.groupBox2.TabIndex = 42;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "卸货点";
            // 
            // button_unloadingPoint_Stop
            // 
            this.button_unloadingPoint_Stop.Enabled = false;
            this.button_unloadingPoint_Stop.Image = ((System.Drawing.Image)(resources.GetObject("button_unloadingPoint_Stop.Image")));
            this.button_unloadingPoint_Stop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_unloadingPoint_Stop.Location = new System.Drawing.Point(164, 24);
            this.button_unloadingPoint_Stop.Name = "button_unloadingPoint_Stop";
            this.button_unloadingPoint_Stop.Size = new System.Drawing.Size(144, 32);
            this.button_unloadingPoint_Stop.TabIndex = 42;
            this.button_unloadingPoint_Stop.Text = "停止卸货点服务";
            this.button_unloadingPoint_Stop.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_unloadingPoint_Stop.UseVisualStyleBackColor = true;
            this.button_unloadingPoint_Stop.Click += new System.EventHandler(this.button_unloadingPoint_Stop_Click);
            // 
            // button_unloadingPoint_Run
            // 
            this.button_unloadingPoint_Run.Image = ((System.Drawing.Image)(resources.GetObject("button_unloadingPoint_Run.Image")));
            this.button_unloadingPoint_Run.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_unloadingPoint_Run.Location = new System.Drawing.Point(12, 24);
            this.button_unloadingPoint_Run.Name = "button_unloadingPoint_Run";
            this.button_unloadingPoint_Run.Size = new System.Drawing.Size(148, 32);
            this.button_unloadingPoint_Run.TabIndex = 41;
            this.button_unloadingPoint_Run.Text = "开启卸货点服务";
            this.button_unloadingPoint_Run.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_unloadingPoint_Run.UseVisualStyleBackColor = true;
            this.button_unloadingPoint_Run.Click += new System.EventHandler(this.button_unloadingPoint_Run_Click);
            // 
            // timer_readingHead
            // 
            this.timer_readingHead.Interval = 10000;
            this.timer_readingHead.Tick += new System.EventHandler(this.timer_readingHead_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 422);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.listBoxLog);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.button_setup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ASCM服务程序";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_setup;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusCompanyInfo;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusTel;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusSystemTime;
        private System.Windows.Forms.ListBox listBoxLog;
        private System.Windows.Forms.Button button_RfidReader_Run;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Timer timer_unloadingPoint;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_RfidReader_Stop;
        private System.Windows.Forms.Timer timer_readingHead;
        private System.Windows.Forms.Button button_unloadingPoint_Stop;
        private System.Windows.Forms.Button button_unloadingPoint_Run;
    }
}