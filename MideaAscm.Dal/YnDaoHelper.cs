using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YnBaseDal;
using System.Xml;
using System.Reflection;

namespace MideaAscm.Dal
{
    public class YnDaoHelper
    {
        private static YnDaoHelper ynDaoHelper;
        public NHibernateHelper nHibernateHelper { get; set; }
        public YnDaoHelper()
        {
        }
        public static YnDaoHelper GetInstance()
        {
            if (ynDaoHelper == null)
                ynDaoHelper = new YnDaoHelper();
            return ynDaoHelper;
        }
        private YnDaoHelper(NHibernateType nHibernateType, string configFileName)
        {
            try
            {
                nHibernateHelper = new NHibernateHelper(nHibernateType, configFileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private YnDaoHelper(NHibernateType nHibernateType,string configFileName, Assembly[] assemblys)
        {
            try
            {
                nHibernateHelper = new NHibernateHelper(nHibernateType,configFileName, null, null, assemblys);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private YnDaoHelper(NHibernateType nHibernateType, XmlReader xmlReader, string connection_string)
        {
            try
            {
                nHibernateHelper = new NHibernateHelper(nHibernateType, xmlReader, connection_string, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private YnDaoHelper(NHibernateType nHibernateType, string configFileName, XmlReader xmlReader, string connection_string, Assembly[] assemblys)
        {
            try
            {
                nHibernateHelper = new NHibernateHelper(nHibernateType,configFileName, xmlReader, connection_string, assemblys);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void Init(NHibernateType nHibernateType)
        {
            try
            {
                Init(nHibernateType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void Init(NHibernateType nHibernateType, Assembly[] assemblys)
        {
            try
            {
                Init(nHibernateType, null, null, null, assemblys);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void Init(NHibernateType nHibernateType, string configFileName, Assembly[] assemblys)
        {
            try
            {
                Init(nHibernateType,configFileName, null, null, assemblys);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void Init(NHibernateType nHibernateType, string configFileName)
        {
            try
            {
                if (ynDaoHelper == null)
                {
                    ynDaoHelper = new YnDaoHelper(nHibernateType, configFileName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void Init(NHibernateType nHibernateType, XmlReader xmlReader, string connection_string)
        {
            try
            {
                if (ynDaoHelper == null)
                {
                    ynDaoHelper = new YnDaoHelper(nHibernateType, xmlReader, connection_string);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void Init(NHibernateType nHibernateType,string configFileName, XmlReader xmlReader, string connection_string, Assembly[] assemblys)
        {
            try
            {
                if (ynDaoHelper == null)
                {
                    ynDaoHelper = new YnDaoHelper(nHibernateType,configFileName, xmlReader, connection_string, assemblys);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
