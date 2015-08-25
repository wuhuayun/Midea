using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using YnBaseDal;

namespace MideaAscm.WebService
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            //日志
            log4net.Config.XmlConfigurator.Configure();
            new YnBaseClass2.Helper.LogHelper();
            YnBaseClass2.Helper.LogHelper.GetLog().Info("程序启动...");

            string file1 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\YnFrame.Dal.dll");
            string file2 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\MideaAscm.Dal.dll");
            string file3 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\MideaAscm.View.Dal.dll");
            System.Reflection.Assembly _assembly = System.Reflection.Assembly.LoadFile(file1);
            YnFrame.Dal.YnDaoHelper.Init(NHibernateType.Web, new System.Reflection.Assembly[] { _assembly });
            _assembly = System.Reflection.Assembly.LoadFile(file2);
            MideaAscm.Dal.YnDaoHelper.Init(NHibernateType.Web, new System.Reflection.Assembly[] { _assembly });
            _assembly = System.Reflection.Assembly.LoadFile(file3);
            MideaAscm.View.Dal.YnDaoHelper.Init(NHibernateType.Web, new System.Reflection.Assembly[] { _assembly });
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            YnFrame.Dal.YnDaoHelper.GetInstance().nHibernateHelper.CloseSessionFactory();
            MideaAscm.Dal.YnDaoHelper.GetInstance().nHibernateHelper.CloseSessionFactory();
            MideaAscm.View.Dal.YnDaoHelper.GetInstance().nHibernateHelper.CloseSessionFactory();
        }
    }
}