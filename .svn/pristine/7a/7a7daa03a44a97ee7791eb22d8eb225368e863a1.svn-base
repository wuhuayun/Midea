using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MideaAscm.Pad
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (new frmLoginView().ShowDialog() == DialogResult.OK)
            {
                Application.Run(new frmMainView());
            }
            else
            {
                return;
            }
        }
    }
}
