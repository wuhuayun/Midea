using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.IO;
using MideaAscm.Dal.GetMaterialManage.Entities;
using MideaAscm.Dal.Base.Entities;
using Newtonsoft.Json;

namespace WinForm
{
    public partial class frmMain : Office2007Form
    {
        /// <summary>权限验证票证</summary>
        public static string encryptTicket = string.Empty;
        /// <summary>登陆用户名</summary>
        public static string userName = string.Empty;
        
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            int nI = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form.Name == "frmMonitorView")
                {
                    form.Activate();
                    form.BringToFront();
                    nI++;
                }
            }
            if (nI == 0)
            {
                frmMonitorView MonitorView = new frmMonitorView();
                MonitorView.MdiParent = this;
                this.splitContainer2.Panel2.Controls.Add(MonitorView);
                MonitorView.Dock = DockStyle.Fill;
                MonitorView.Show();
            }
        }

        private void btnTask_Click(object sender, EventArgs e)
        {
            int nI = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form.Name == "frmTasksView")
                {
                    form.Activate();
                    form.BringToFront();
                    nI++;
                }
            }
            if (nI == 0)
            {
                frmTasksView TasksView = new frmTasksView();
                TasksView.MdiParent = this;
                this.splitContainer2.Panel2.Controls.Add(TasksView);
                TasksView.Dock = DockStyle.Fill;
                TasksView.Show();
            }
        }

        private void btnMaterial_Click(object sender, EventArgs e)
        {
            int nI = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form.Name == "frmMaterialView")
                {
                    form.Activate();
                    form.BringToFront();
                    nI++;
                }
            }
            if (nI == 0)
            {
                frmMaterialView MaterialView = new frmMaterialView();
                MaterialView.MdiParent = this;
                this.splitContainer2.Panel2.Controls.Add(MaterialView);
                MaterialView.Dock = DockStyle.Fill;
                MaterialView.Show();
            }
        }

        /// <summary>
        /// 退出系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnexit_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.Show("确定退出本系统?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = userName.ToString();

            int nI = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form.Name == "frmMonitorView")
                {
                    form.Activate();
                    form.BringToFront();
                    nI++;
                }
            }
            if (nI == 0)
            {
                frmMonitorView MonitorView = new frmMonitorView();
                MonitorView.MdiParent = this;
                this.splitContainer2.Panel2.Controls.Add(MonitorView);
                MonitorView.Dock = DockStyle.Fill;
                MonitorView.Show();
            }
        }
    }
}
