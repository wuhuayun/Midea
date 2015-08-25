using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DevComponents.DotNetBar;
using Microsoft.Win32;

namespace WinForm
{
    public partial class Login : Office2007Form
    {
        public Login()
        {
            InitializeComponent();
            lblUsername.BackColor = Color.Transparent;
            lblPassword.BackColor = Color.Transparent;
            cbUserPwd.BackColor = Color.Transparent;
        }

        /// <summary>
        /// 登录按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = "";
            string password = "";

            #region 用户登录检测

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
            #region 用户登录判断

            WinForm.AscmWebService.AscmWebService service = new AscmWebService.AscmWebService();
            string message = "";
            string encryptTicket = "";
            try
            {
                if (service.UserAuthentication(username, password, YnBaseClass2.Helper.HostHelper.GetHostIP(), ref message, ref encryptTicket))
                {
                    //Main_Form.encryptTicket = encryptTicket;
                    //Main_Form.userName = username;
                    //this.DialogResult = DialogResult.OK;
                    //this.Close();

                    if (cbUserPwd.Checked == true)
                    {
                        Remember(true, username, password);
                    }
                    else
                    {
                        Remember(false, username, password);
                    }

                    frmMain.encryptTicket = encryptTicket;
                    frmMain.userName = username;
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
        /// 用户名文本框的回车事件,把焦点给txtPassword控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUsername_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                txtPassword.Focus();
            }
        }

        /// <summary>
        /// 密码文本框的回车事件,把焦点给btnLogin控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                //btnLogin.Focus();

                btnLogin_Click(sender, e);
            }
        }

        /// <summary>
        /// 取消按钮事件,退出系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// 记录用户名及密码
        /// </summary>
        /// <param name="remeber">是否记录</param>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        private void Remember(bool remeber, string userName, string passWord)
        {
            try
            {
                //RegistryKey key = Registry.LocalMachine.OpenSubKey("SYGOLE.PAD.CLIENT", true);
                //key = key.CreateSubKey("CheckRemeberPwdManage", RegistryKeyPermissionCheck.Default);
                RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE", true);
                key = key.CreateSubKey("CheckManage", RegistryKeyPermissionCheck.Default);
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
        /// 判断是否记住密码
        /// </summary>
        /// <returns></returns>
        private bool HasRemember()
        {
            try
            {
                //RegistryKey key = Registry.LocalMachine.OpenSubKey("SYGOLE.PAD.CLIENT\\CheckRemeberPwdManage", true);
                RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\CheckManage", true);
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

        private void Login_Load(object sender, EventArgs e)
        {
            this.cbUserPwd.Checked = false;
            HasRemember();
        }


    }
}
