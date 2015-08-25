using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Xml;

namespace MideaAscm.Server
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
                YnFrame.Client.YnClientApp.GetInstance().registryKey = @"Software\YouNengSoft\MideaAscmServer";
                //日志f
                Assembly _assembly = Assembly.GetExecutingAssembly();
                if (YnBaseClass2.Helper.LogHelper.GetLog() == null)
                {
                    //StreamReader _textStreamReader = new StreamReader(_assembly.GetManifestResourceStream("log4net.config"));
                    Stream stream = _assembly.GetManifestResourceStream("MideaAscm.Server.log4net.config");
                    log4net.Config.XmlConfigurator.Configure(stream);
                    new YnBaseClass2.Helper.LogHelper();
                    //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                    //log.Error("Error信息");
                    //log.Debug("Debug信息");
                    //FATAL>ERROR >WARN>INFO>DEBUG
                }
                YnBaseClass2.Helper.LogHelper.GetLog().Info("启动美的中央空调ASCM中间件服务系统...");

                #region db初始化
                string file1 = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"YnFrame.Dal.dll");
                string file2 = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"MideaAscm.Dal.dll");
                System.Reflection.Assembly _assembly1 = System.Reflection.Assembly.LoadFile(file1);
                System.Reflection.Assembly _assembly2 = System.Reflection.Assembly.LoadFile(file2);
                XmlReader xmlReader1 = XmlReader.Create(_assembly.GetManifestResourceStream("MideaAscm.Server.hibernate.cfg.xml"));
                XmlReader xmlReader2 = XmlReader.Create(_assembly.GetManifestResourceStream("MideaAscm.Server.hibernate.cfg.xml"));
                //string connection_string = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SID=XE)));User Id=mideaascm;Password=123;";
                YnFrame.Dal.YnDaoHelper.Init(YnBaseDal.NHibernateType.WinForm, null, xmlReader1, null, new System.Reflection.Assembly[] { _assembly1 });
                MideaAscm.Dal.YnDaoHelper.Init(YnBaseDal.NHibernateType.WinForm, null, xmlReader2, null, new System.Reflection.Assembly[] { _assembly2 });
                
                #endregion

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show("初始化失败：" + ex.Message, "提示....", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //YnBaseClass2.Helper.LogHelper.GetLog().Error("初始化错误:", ex);
                //throw ex;
            }

        }
    }
}
