namespace MideaAscm.Pad
{
    partial class frmRelateDiscreteJobsView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.bar1 = new DevComponents.DotNetBar.Bar();
            this.labelItem1 = new DevComponents.DotNetBar.LabelItem();
            this.tbItemStart = new DevComponents.DotNetBar.TextBoxItem();
            this.labelItem2 = new DevComponents.DotNetBar.LabelItem();
            this.tbItemEnd = new DevComponents.DotNetBar.TextBoxItem();
            this.btnItemSearch = new DevComponents.DotNetBar.ButtonItem();
            this.pagerControlJobs = new MideaAscm.Pad.CustomControl.PagerControl();
            this.dgViewDiscreteJobsList = new DevComponents.DotNetBar.Controls.DataGridViewX();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgViewDiscreteJobsList)).BeginInit();
            this.SuspendLayout();
            // 
            // bar1
            // 
            this.bar1.AntiAlias = true;
            this.bar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.bar1.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.labelItem1,
            this.tbItemStart,
            this.labelItem2,
            this.tbItemEnd,
            this.btnItemSearch});
            this.bar1.Location = new System.Drawing.Point(0, 0);
            this.bar1.Name = "bar1";
            this.bar1.Size = new System.Drawing.Size(797, 30);
            this.bar1.Stretch = true;
            this.bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.bar1.TabIndex = 0;
            this.bar1.TabStop = false;
            this.bar1.Text = "bar1";
            // 
            // labelItem1
            // 
            this.labelItem1.Name = "labelItem1";
            this.labelItem1.Text = "起止日期：";
            // 
            // tbItemStart
            // 
            this.tbItemStart.Name = "tbItemStart";
            this.tbItemStart.TextBoxWidth = 90;
            this.tbItemStart.WatermarkColor = System.Drawing.SystemColors.GrayText;
            this.tbItemStart.GotFocus += new System.EventHandler(this.textBoxItem_GotFocus);
            // 
            // labelItem2
            // 
            this.labelItem2.Name = "labelItem2";
            this.labelItem2.Text = "~";
            // 
            // tbItemEnd
            // 
            this.tbItemEnd.Name = "tbItemEnd";
            this.tbItemEnd.TextBoxWidth = 90;
            this.tbItemEnd.WatermarkColor = System.Drawing.SystemColors.GrayText;
            this.tbItemEnd.GotFocus += new System.EventHandler(this.textBoxItem_GotFocus);
            // 
            // btnItemSearch
            // 
            this.btnItemSearch.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btnItemSearch.Image = global::MideaAscm.Pad.Properties.Resources.search;
            this.btnItemSearch.Name = "btnItemSearch";
            this.btnItemSearch.Text = "查询";
            this.btnItemSearch.Click += new System.EventHandler(this.btnItemSearch_Click);
            // 
            // pagerControlJobs
            // 
            this.pagerControlJobs.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pagerControlJobs.Location = new System.Drawing.Point(0, 371);
            this.pagerControlJobs.Name = "pagerControlJobs";
            this.pagerControlJobs.NMax = 0;
            this.pagerControlJobs.PageCount = 0;
            this.pagerControlJobs.PageCurrent = 0;
            this.pagerControlJobs.PageSize = 30;
            this.pagerControlJobs.Size = new System.Drawing.Size(797, 25);
            this.pagerControlJobs.TabIndex = 1;
            this.pagerControlJobs.EventPaging += new MideaAscm.Pad.CustomControl.PagerControl.EventPagingHandler(this.pagerControlJobs_EventPaging);
            // 
            // dgViewDiscreteJobsList
            // 
            this.dgViewDiscreteJobsList.AllowUserToAddRows = false;
            this.dgViewDiscreteJobsList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgViewDiscreteJobsList.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgViewDiscreteJobsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgViewDiscreteJobsList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.dgViewDiscreteJobsList.Location = new System.Drawing.Point(0, 30);
            this.dgViewDiscreteJobsList.Name = "dgViewDiscreteJobsList";
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgViewDiscreteJobsList.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgViewDiscreteJobsList.RowTemplate.Height = 23;
            this.dgViewDiscreteJobsList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgViewDiscreteJobsList.Size = new System.Drawing.Size(797, 341);
            this.dgViewDiscreteJobsList.TabIndex = 2;
            // 
            // frmRelateDiscreteJobsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 396);
            this.Controls.Add(this.dgViewDiscreteJobsList);
            this.Controls.Add(this.pagerControlJobs);
            this.Controls.Add(this.bar1);
            this.DoubleBuffered = true;
            this.Name = "frmRelateDiscreteJobsView";
            this.Text = "关联作业";
            this.Load += new System.EventHandler(this.frmRelateDiscreteJobsView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgViewDiscreteJobsList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Bar bar1;
        private CustomControl.PagerControl pagerControlJobs;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgViewDiscreteJobsList;
        private DevComponents.DotNetBar.LabelItem labelItem1;
        private DevComponents.DotNetBar.TextBoxItem tbItemStart;
        private DevComponents.DotNetBar.LabelItem labelItem2;
        private DevComponents.DotNetBar.TextBoxItem tbItemEnd;
        private DevComponents.DotNetBar.ButtonItem btnItemSearch;
    }
}