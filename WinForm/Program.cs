﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WinForm
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (new Login().ShowDialog() == DialogResult.OK)
            {
                //Application.Run(new Main_Form());
                Application.Run(new frmMain());
            }
            else
            {
                return;
            }
        }
    }
}
