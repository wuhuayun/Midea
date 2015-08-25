using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using YnBaseDal;

namespace MideaAscm
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // 参数默认值
            );

        }

        protected void Application_Start()
        {
            //日志
            log4net.Config.XmlConfigurator.Configure();
            new YnBaseClass2.Helper.LogHelper();
            //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            //log.Error("Error信息");
            //log.Debug("Debug信息");
            //FATAL>ERROR >WARN>INFO>DEBUG
            YnBaseClass2.Helper.LogHelper.GetLog().Info("程序启动...");
            //注册路由
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);

            //初始化
            //取连接字符串
            //string dbFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "YnDbConfig.ini");
            string dbFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\YnDbConfig.ini");
            string connection_string = "";
            if (System.IO.File.Exists(dbFile))
            {
                System.IO.StreamReader objReader = new System.IO.StreamReader(dbFile);
                string key = "youneng";
                string sLine = objReader.ReadLine();
                objReader.Close();
                connection_string = YnBaseClass2.Helper.DESEncryptHelper.Decrypt(sLine, key);
            }

            string file1 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\YnFrame.Dal.dll");

            string file2 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\MideaAscm.Dal.dll");
            System.Reflection.Assembly _assembly = System.Reflection.Assembly.LoadFile(file1);
            YnFrame.Dal.YnDaoHelper.Init(NHibernateType.Web, null, null, connection_string, new System.Reflection.Assembly[] { _assembly });
            _assembly = System.Reflection.Assembly.LoadFile(file2);
            MideaAscm.Dal.YnDaoHelper.Init(NHibernateType.Web, null, null, connection_string, new System.Reflection.Assembly[] { _assembly });

            string file3 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\MideaAscm.Job.Dal.dll");
            _assembly = System.Reflection.Assembly.LoadFile(file3);
            string configFileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\hibernate2.cfg.xml");
            MideaAscm.Job.Dal.YnDaoHelper.Init(NHibernateType.Web, configFileName, null, connection_string, new System.Reflection.Assembly[] { _assembly });

            string file4 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\MideaAscm.View.Dal.dll");
            _assembly = System.Reflection.Assembly.LoadFile(file4);
            configFileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\hibernate3.cfg.xml");
            MideaAscm.View.Dal.YnDaoHelper.Init(NHibernateType.Web, configFileName, null, connection_string, new System.Reflection.Assembly[] { _assembly });
        }
        protected void Application_End(Object sender, EventArgs e)
        {
            //初始化,奇怪没有执行？
            //YnStructShared.Dal.YnDaoHelper.GetInstance().nHibernateHelper.CloseSessionFactory();
            YnFrame.Dal.YnDaoHelper.GetInstance().nHibernateHelper.CloseSessionFactory();
            MideaAscm.Dal.YnDaoHelper.GetInstance().nHibernateHelper.CloseSessionFactory();
        }
    }
}