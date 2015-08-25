using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal;
using MideaAscm.Dal.Warehouse.Entities;

namespace MideaAscm.Services.Warehouse
{
    public class AscmWmsMaterialLabelService
    {
        private static AscmWmsMaterialLabelService service;
        public static AscmWmsMaterialLabelService GetInstance()
        {
            if (service == null)
                service = new AscmWmsMaterialLabelService();
            return service;
        }

        public AscmWmsMaterialLabel Get(int id)
        {
            AscmWmsMaterialLabel materialLabel = null;
            try
            {
                materialLabel = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmWmsMaterialLabel>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWmsMaterialLabel)", ex);
                throw ex;
            }
            return materialLabel;
        }
        public List<AscmWmsMaterialLabel> GetList(string sql)
        {
            List<AscmWmsMaterialLabel> list = null;
            try
            {
                IList<AscmWmsMaterialLabel> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsMaterialLabel>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsMaterialLabel>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsMaterialLabel)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmWmsMaterialLabel> listWmsMaterialLabel)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listWmsMaterialLabel);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsMaterialLabel)", ex);
                throw ex;
            }
        }
        public void Save(AscmWmsMaterialLabel materialLabel)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(materialLabel);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsMaterialLabel)", ex);
                throw ex;
            }
        }
        public void Update(AscmWmsMaterialLabel materialLabel)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmWmsMaterialLabel>(materialLabel);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmWmsMaterialLabel)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmWmsMaterialLabel)", ex);
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmWmsMaterialLabel materialLabel = Get(id);
                Delete(materialLabel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmWmsMaterialLabel materialLabel)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmWmsMaterialLabel>(materialLabel);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWmsMaterialLabel)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmWmsMaterialLabel> listWmsMaterialLabel)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listWmsMaterialLabel);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWmsMaterialLabel)", ex);
                throw ex;
            }
        }
    }
}
