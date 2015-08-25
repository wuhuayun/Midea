using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Xml;

namespace MideaAscm.AM.Soft
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Assembly _assembly = Assembly.GetExecutingAssembly();

                #region db初始化
                string file1 = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"YnFrame.Dal.dll");
                string file2 = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"MideaAscm.Dal.dll");
                System.Reflection.Assembly _assembly1 = System.Reflection.Assembly.LoadFile(file1);
                System.Reflection.Assembly _assembly2 = System.Reflection.Assembly.LoadFile(file2);
                XmlReader xmlReader1 = XmlReader.Create(_assembly.GetManifestResourceStream("MideaAscm.AM.Soft.hibernate.cfg.xml"));
                XmlReader xmlReader2 = XmlReader.Create(_assembly.GetManifestResourceStream("MideaAscm.AM.Soft.hibernate.cfg.xml"));
                //string connection_string = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SID=XE)));User Id=mideaascm;Password=123;";
                YnFrame.Dal.YnDaoHelper.Init(YnBaseDal.NHibernateType.WinForm, null, xmlReader1, null, new System.Reflection.Assembly[] { _assembly1 });
                MideaAscm.Dal.YnDaoHelper.Init(YnBaseDal.NHibernateType.WinForm, null, xmlReader2, null, new System.Reflection.Assembly[] { _assembly2 });

                #endregion

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain());
            }
            catch (Exception ex)
            {
                throw ex;
                //MessageBox.Show("初始化失败：" + ex.Message, "提示....", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
