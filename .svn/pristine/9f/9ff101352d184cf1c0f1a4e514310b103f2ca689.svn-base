using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using Microsoft.Win32;
using System.Security.Permissions;
using System.Reflection;

namespace MideaAscm.Pad
{
    public partial class frmLoginView : Form
    {
        public frmLoginView()
        {
            InitializeComponent();
        }
        
        
        /// <summary>
        /// Form Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmLoginView_Load(object sender, EventArgs e)
        {
            this.cbUserPwd.Checked = false;
            this.Text += this.GetRunningVersion().ToString();
            HasRemember();
        }

        /// <summary>
        /// Login Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUserLogin_Click(object sender, EventArgs e)
        {
            string username = "";
            string password = "";

            #region User login authentication

            if (!String.IsNullOrEmpty(txtUsername.Text.Trim()))
            {
                username = txtUsername.Text.Trim();
            }
            else
            {
                MessageBoxEx.Show("请输入登录用户名", "登录提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!String.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                password = txtPassword.Text.Trim();
            }
            else if (username.ToLower() == "admin")//其它用户允许空密码
            {

                MessageBoxEx.Show("请输入登录密码", "登录提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;

            }

            #endregion

            #region Verify that the user exists

            AscmWebService.AscmWebService service = new AscmWebService.AscmWebService();
            string message = "";
            string encryptTicket = "";
            try
            {
                if (service.UserAuthentication(username, password, YnBaseClass2.Helper.HostHelper.GetHostIP(), ref message, ref encryptTicket))
                {
                    if (cbUserPwd.Checked == true)
                    {
                        Remember(true, username, password);
                    }
                    else
                    {
                        Remember(false, username, password);
                    }

                    frmMainView.encryptTicket = encryptTicket;
                    frmMainView.userName = username;

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBoxEx.Show(message, "登录失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                MessageBoxEx.Show(message, "登录失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            #endregion
        }

        /// <summary>
        /// Reset Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUserReset_Click(object sender, EventArgs e)
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
        }

        /// <summary>
        /// Remeber User'name And Password
        /// </summary>
        /// <param name="remeber">Whether Remeber</param>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        private void Remember(bool remeber, string userName, string passWord)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
                key = key.CreateSubKey("LoginApp", RegistryKeyPermissionCheck.Default);
                if (remeber)
                {
                    key.SetValue("username", userName);
                    key.SetValue("password", passWord);
                }
                else
                {
                    if (key.GetValue("username") != null) key.DeleteValue("username");
                    if (key.GetValue("password") != null) key.DeleteValue("password");
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Determine whether the Remember PassWord
        /// </summary>
        /// <returns></returns>
        private bool HasRemember()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\LoginApp", true);
                object objuser = key.GetValue("username");
                if (objuser != null)
                {
                    this.txtUsername.Text = objuser.ToString();
                    this.cbUserPwd.Checked = true;
                }
                else
                {
                    this.cbUserPwd.Checked = false;
                }
                object objpassword = key.GetValue("password");
                if (objpassword != null) this.txtPassword.Text = objpassword.ToString();

                return objuser != null && objpassword != null;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get deployed version
        /// </summary>
        /// <returns></returns>
        private Version GetRunningVersion()
        {
            try
            {
                return System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;
            }
            catch
            {
                return Assembly.GetExecutingAssembly().GetName().Version;
            }
        }
    }
}
