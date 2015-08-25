using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Rendering;
using System.Globalization;
using System.Runtime.InteropServices;

namespace MideaAscm.Pad
{
    public partial class frmMainView : DevComponents.DotNetBar.RibbonForm
    {
        public static string encryptTicket = string.Empty;   //Permissions authentication ticket
        public static string userName = string.Empty;   //Login username

        public frmMainView()
        {
            InitializeComponent();
        }

        [DllImport("user32")]
        public static extern int SetParent(int hWndChild, int hWndNewParent);

        private void frmMainView_Load(object sender, EventArgs e)
        {
            UpdateStatusBar();
        }

        private void frmMainView_MdiChildActivate(object sender, EventArgs e)
        {
            EnableFileItems();
        }

        /// <summary>
        /// Verifies current context and enables/disables menu items...
        /// </summary>
        private void EnableFileItems()
        {
            // Accessing items through the Items collection and setting the properties on them
            // will propagate certain properties to all items with the same name...

            if (this.ActiveMdiChild != null)
            {
                if (this.ActiveMdiChild is frmLogisticsMonitoringView)
                    ((frmLogisticsMonitoringView)this.ActiveMdiChild).FormActivated();
                else if (this.ActiveMdiChild is frmTaskManagementView)
                    ((frmTaskManagementView)this.ActiveMdiChild).FormActivated();
                else if (this.ActiveMdiChild is frmWareHouseSearchView)
                    ((frmWareHouseSearchView)this.ActiveMdiChild).FormActivated();
                else if (this.ActiveMdiChild is frmDiscreteJobsSearchView)
                    ((frmDiscreteJobsSearchView)this.ActiveMdiChild).FormActivated();
            }
        }


        #region Automatically change the form style
        #region Automatic Color Scheme creation based on the selected color table
        private bool m_ColorSelected = false;
        private eStyle m_BaseStyle = eStyle.Office2010Silver;
        private void buttonStyleCustom_ExpandChange(object sender, System.EventArgs e)
        {
            if (buttonStyleCustom.Expanded)
            {
                // Remember the starting color scheme to apply if no color is selected during live-preview
                m_ColorSelected = false;
                m_BaseStyle = StyleManager.Style;
            }
            else
            {
                if (!m_ColorSelected)
                {
                    if (StyleManager.IsMetro(StyleManager.Style))
                        StyleManager.MetroColorGeneratorParameters = DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters.Default;
                    else
                        StyleManager.ChangeStyle(m_BaseStyle, Color.Empty);
                }
            }
        }

        private void buttonStyleCustom_ColorPreview(object sender, DevComponents.DotNetBar.ColorPreviewEventArgs e)
        {
            if (StyleManager.IsMetro(StyleManager.Style))
            {
                Color baseColor = e.Color;
                StyleManager.MetroColorGeneratorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(Color.White, baseColor);
            }
            else
                StyleManager.ColorTint = e.Color;
        }

        private void buttonStyleCustom_SelectedColorChanged(object sender, System.EventArgs e)
        {
            m_ColorSelected = true; // Indicate that color was selected for buttonStyleCustom_ExpandChange method
            buttonStyleCustom.CommandParameter = buttonStyleCustom.SelectedColor;
        }
        #endregion

        private void AppCommandTheme_Executed(object sender, EventArgs e)
        {
            ICommandSource source = sender as ICommandSource;
            if (source.CommandParameter is string)
            {
                eStyle style = (eStyle)Enum.Parse(typeof(eStyle), source.CommandParameter.ToString());
                // Using StyleManager change the style and color tinting
                if (StyleManager.IsMetro(style))
                {
                    ribbonControl1.RibbonStripFont = new System.Drawing.Font("Segoe UI", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    if (style == eStyle.Metro)
                        StyleManager.MetroColorGeneratorParameters = DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters.DarkBlue;

                    // Adjust size of switch button to match Metro styling
                    switchButtonItem1.SwitchWidth = 12;
                    switchButtonItem1.ButtonWidth = 42;
                    switchButtonItem1.ButtonHeight = 19;

                    // Adjust tab strip style
                    tabStrip1.Style = eTabStripStyle.Metro;

                    StyleManager.Style = style; // BOOM
                }
                else
                {
                    // If previous style was Metro we need to update other properties as well
                    if (StyleManager.IsMetro(StyleManager.Style))
                    {
                        ribbonControl1.RibbonStripFont = null;

                        // Adjust size of switch button to match Office styling
                        switchButtonItem1.SwitchWidth = 28;
                        switchButtonItem1.ButtonWidth = 62;
                        switchButtonItem1.ButtonHeight = 20;
                    }
                    // Adjust tab strip style
                    tabStrip1.Style = eTabStripStyle.Office2007Document;
                    StyleManager.ChangeStyle(style, Color.Empty);
                }
            }
            else if (source.CommandParameter is Color)
            {
                if (StyleManager.IsMetro(StyleManager.Style))
                    StyleManager.MetroColorGeneratorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(Color.White, (Color)source.CommandParameter);
                else
                    StyleManager.ColorTint = (Color)source.CommandParameter;
            }
        }
        #endregion

        /// <summary>
        /// Expand or shrink menu
        /// </summary>
        private void RibbonStateCommand_Executed(object sender, EventArgs e)
        {
            ribbonControl1.Expanded = RibbonStateCommand.Checked;
            RibbonStateCommand.Checked = !RibbonStateCommand.Checked;
        }

        /// <summary>
        /// Open MDI ChildForm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppCommandNew_Executed(object sender, EventArgs e)
        {
            ICommandSource source = sender as ICommandSource;
            if (source.CommandParameter is string)
            {
                //If the child form is opened, bring to front for the form;
                bool isValid = false;
                foreach (Form form in Application.OpenForms)
                {
                    if (source.CommandParameter.ToString() == form.Name)
                    {
                        //form.Activate();
                        //form.WindowState = FormWindowState.Maximized;
                        //form.Show();
                        //form.BringToFront();
                        //form.Update();
                        form.Focus();
                        return;
                    }
                }
                if (!isValid)
                {
                    string frmName = source.CommandParameter.ToString();
                    switch (frmName)
                    {
                        case "frmLogisticsMonitoringView":
                            {
                                frmLogisticsMonitoringView frm = new frmLogisticsMonitoringView();
                                frm.MdiParent = this;
                                frm.WindowState = FormWindowState.Maximized;
                                frm.BringToFront();
                                frm.Show();
                                frm.Update();
                            }
                            break;
                        case "frmTaskManagementView":
                            {
                                frmTaskManagementView frm = new frmTaskManagementView();
                                frm.MdiParent = this;
                                frm.WindowState = FormWindowState.Maximized;
                                frm.BringToFront();
                                frm.Show();
                                frm.Update();
                            }
                            break;
                        case "frmWareHouseSearchView":
                            {
                                frmWareHouseSearchView frm = new frmWareHouseSearchView();
                                frm.MdiParent = this;
                                frm.WindowState = FormWindowState.Maximized;
                                frm.BringToFront();
                                frm.Show();
                                frm.Update();
                            }
                            break;
                        case "frmDiscreteJobsSearchView":
                            {
                                frmDiscreteJobsSearchView frm = new frmDiscreteJobsSearchView();
                                frm.MdiParent = this;
                                frm.WindowState = FormWindowState.Maximized;
                                frm.BringToFront();
                                frm.Show();
                                frm.Update();
                            }
                            break;
                    }
                }
                
            }
        }

        /// <summary>
        /// Update the status bar
        /// </summary>
        private void UpdateStatusBar()
        {
            labelStatus.Text = "欢迎您，" + userName;
        }
    }
}
