using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.SupplierPreparation.Entities;
using MideaAscm.Services.Vehicle;
using MideaAscm.Dal.Vehicle.Entities;
using MideaAscm.Dal.Base;
using YnFrame.Services;

namespace MideaAscm.Services.SupplierPreparation
{
    public class AscmAppointmentDistributionService
    {
        #region base
        private static AscmAppointmentDistributionService ascmAppointmentDistributionServices;
        public static AscmAppointmentDistributionService GetInstance()
        {
            if (ascmAppointmentDistributionServices == null)
                ascmAppointmentDistributionServices = new AscmAppointmentDistributionService();
            return ascmAppointmentDistributionServices;
        }
        public AscmAppointmentDistribution Get(int id)
        {
            AscmAppointmentDistribution ascmAppointmentDistribution = null;
            try
            {
                ascmAppointmentDistribution = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmAppointmentDistribution>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmAppointmentDistribution)", ex);
                throw ex;
            }
            return ascmAppointmentDistribution;
        }
        public List<AscmAppointmentDistribution> GetList(string sql)
        {
            List<AscmAppointmentDistribution> list = null;
            try
            {
                IList<AscmAppointmentDistribution> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmAppointmentDistribution>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmAppointmentDistribution>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmAppointmentDistribution)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmAppointmentDistribution> listAscmAppointmentDistribution)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmAppointmentDistribution);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmAppointmentDistribution)", ex);
                throw ex;
            }
        }
        public void Save(AscmAppointmentDistribution ascmAppointmentDistribution)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmAppointmentDistribution);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmAppointmentDistribution)", ex);
                throw ex;
            }
        }
        public void Update(AscmAppointmentDistribution ascmAppointmentDistribution)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmAppointmentDistribution>(ascmAppointmentDistribution);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmAppointmentDistribution)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmAppointmentDistribution)", ex);
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmAppointmentDistribution ascmAppointmentDistribution = Get(id);
                Delete(ascmAppointmentDistribution);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmAppointmentDistribution ascmAppointmentDistribution)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmAppointmentDistribution>(ascmAppointmentDistribution);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmAppointmentDistribution)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmAppointmentDistribution> listAscmAppointmentDistribution)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmAppointmentDistribution);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmAppointmentDistribution)", ex);
                throw ex;
            }
        }
        #endregion

        #region 业务
        public void GetAppointmentDistribution(double _supplierPassDuration, List<AscmDeliBatSumMain> listAscmDeliBatSumMain, ref List<AscmAppointmentDistribution> listAscmAppointmentDistribution)
        {
            try
            {
                if (listAscmDeliBatSumMain == null || listAscmDeliBatSumMain.Count == 0)
                    return;
                DateTime dtAppointmentStartTime, dtAppointmentEndTime;
                if (listAscmDeliBatSumMain[0].appointmentStartTime == null || !DateTime.TryParse(listAscmDeliBatSumMain[0].appointmentStartTime, out dtAppointmentStartTime) ||
                    listAscmDeliBatSumMain[0].appointmentEndTime == null || !DateTime.TryParse(listAscmDeliBatSumMain[0].appointmentEndTime, out dtAppointmentEndTime))
                    return;
                foreach (AscmDeliBatSumMain ascmDeliBatSumMain in listAscmDeliBatSumMain)
                {
                    //2013.11.28改为司机合单确认（多个合单）
                    //取交集
                    DateTime _dtAppointmentStartTime, _dtAppointmentEndTime;
                    if (ascmDeliBatSumMain.appointmentStartTime == null || !DateTime.TryParse(ascmDeliBatSumMain.appointmentStartTime, out _dtAppointmentStartTime) ||
                        ascmDeliBatSumMain.appointmentEndTime == null || !DateTime.TryParse(ascmDeliBatSumMain.appointmentEndTime, out _dtAppointmentEndTime))
                        return;
                    if (dtAppointmentStartTime < _dtAppointmentStartTime)
                        dtAppointmentStartTime = _dtAppointmentStartTime;
                    //5)最晚到货时间早于当前时间的批单，该批单总是可以勾选入合单，但不参加合单生成预约到货时间计算
                    if (_dtAppointmentEndTime > DateTime.Now.AddMinutes(30)&&dtAppointmentEndTime > _dtAppointmentEndTime)
                        dtAppointmentEndTime = _dtAppointmentEndTime;
                }
                if (listAscmDeliBatSumMain.Count > 1)
                {
                    if (dtAppointmentEndTime > DateTime.Now.AddMinutes(30) && dtAppointmentEndTime < dtAppointmentStartTime)
                        throw new Exception("司机绑定多张合单预约时间不一致，不能合并送货");
                }
                double supplierPassDuration = (double)0.5;
                double.TryParse(YnParameterService.GetInstance().GetValue(MyParameter.supplierPassDuration), out supplierPassDuration);
                if (supplierPassDuration < 0.5)
                    supplierPassDuration = 0.5;

                if (_supplierPassDuration > 0)
                {
                    supplierPassDuration = _supplierPassDuration;
                }

                //如到货时间早于当前时间,时间为当前时间向后取半点(最晚最早一样处理)
                DateTime now = DateTime.Now;
                if (now.Minute > 30)
                    now = now.AddMinutes(30);
                if (dtAppointmentStartTime < now)
                    dtAppointmentStartTime = now;
                if (dtAppointmentEndTime < now)
                {
                    dtAppointmentStartTime = now;
                    dtAppointmentEndTime = dtAppointmentStartTime.AddMinutes((int)supplierPassDuration * 60);//-30
                    TimeSpan ts = dtAppointmentEndTime - dtAppointmentStartTime;
                    if (dtAppointmentStartTime.Hour < 8)
                    {
                        dtAppointmentStartTime = Convert.ToDateTime(dtAppointmentStartTime.ToString("yyyy-MM-dd 08:00"));
                        dtAppointmentEndTime = dtAppointmentStartTime.AddMinutes(ts.TotalMinutes);
                    }
                    if (dtAppointmentStartTime.Hour > 17)
                    {
                        dtAppointmentStartTime = Convert.ToDateTime(dtAppointmentStartTime.ToString("yyyy-MM-dd 08:00")).AddDays(1);
                        dtAppointmentEndTime = dtAppointmentStartTime.AddMinutes(ts.TotalMinutes);
                    }
                    if (dtAppointmentEndTime.Hour > 17)
                    {
                        dtAppointmentEndTime = Convert.ToDateTime(dtAppointmentEndTime.ToString("yyyy-MM-dd 17:00"));
                        dtAppointmentStartTime = dtAppointmentEndTime.AddMinutes(-ts.TotalMinutes);
                    }
                }
                //按30分钟取整
                //6)仓库收料时间为8：00 – 11：20，13：00 - 17：00，合单生成的预约到货时间为8：00 -17：00。
                string appointmentStartTime = dtAppointmentStartTime.ToString("yyyy-MM-dd HH:") + (dtAppointmentStartTime.Minute >= 30 ? "30" : "00");
                string appointmentEndTime = dtAppointmentEndTime.ToString("yyyy-MM-dd HH:") + (dtAppointmentEndTime.Minute >= 30 ? "30" : "00");
                DateTime.TryParse(appointmentStartTime, out dtAppointmentStartTime);
                DateTime.TryParse(appointmentEndTime, out dtAppointmentEndTime);

                if (dtAppointmentStartTime.ToString("yyyy-MM-dd") != dtAppointmentEndTime.ToString("yyyy-MM-dd"))
                {
                    dtAppointmentEndTime = dtAppointmentStartTime.AddMinutes(supplierPassDuration*60);
                    if (dtAppointmentStartTime.ToString("yyyy-MM-dd") != dtAppointmentEndTime.ToString("yyyy-MM-dd"))
                    {
                        dtAppointmentEndTime = Convert.ToDateTime(dtAppointmentEndTime.ToString("yyyy-MM-dd 23:30:00"));
                    }
                }
                
                //1.将一天8：00 – 18：00每半小时分为一段,此半小时为固定值
                listAscmAppointmentDistribution = GetList("from AscmAppointmentDistribution where distributionDate='" + dtAppointmentEndTime .ToString("yyyy-MM-dd")+ "'");
                DateTime dtStart = Convert.ToDateTime("2000-01-01 08:00");
                int timeMinuteStep = 30;//此半小时为固定值
                for (int iTimePoint = 0; iTimePoint < 20; iTimePoint++)
                {
                    AscmAppointmentDistribution ascmAppointmentDistribution = listAscmAppointmentDistribution.Find(p=>p.timeId==iTimePoint);
                    if (ascmAppointmentDistribution == null)
                    {
                        ascmAppointmentDistribution = new AscmAppointmentDistribution();
                        ascmAppointmentDistribution.id = 0;
                        ascmAppointmentDistribution.distributionDate = dtAppointmentEndTime.ToString("yyyy-MM-dd");
                        ascmAppointmentDistribution.timeId = iTimePoint;
                        ascmAppointmentDistribution.startTime = dtStart.AddMinutes((iTimePoint) * timeMinuteStep).ToString("HH:mm");
                        ascmAppointmentDistribution.endTime = dtStart.AddMinutes((iTimePoint + 1) * timeMinuteStep).ToString("HH:mm");
                        ascmAppointmentDistribution.count = 0;
                        listAscmAppointmentDistribution.Add(ascmAppointmentDistribution);
                    }
                }
                //2.对于合单生成可选时间,计算合单在T0~T19上每个区间的值
                List<AscmAppointmentDistribution> listAscmAppointmentDistribution_Tmp = new List<AscmAppointmentDistribution>();
                for (int iTime = 0; iTime < 20; iTime++)
                {
                    AscmAppointmentDistribution ascmAppointmentDistribution = listAscmAppointmentDistribution_Tmp.Find(p => p.timeId == iTime);
                    if (ascmAppointmentDistribution == null)
                    {
                        ascmAppointmentDistribution = new AscmAppointmentDistribution();
                        ascmAppointmentDistribution.id = 0;
                        ascmAppointmentDistribution.distributionDate = dtAppointmentEndTime.ToString("yyyy-MM-dd");
                        ascmAppointmentDistribution.timeId = iTime;
                        ascmAppointmentDistribution.startTime = dtStart.AddMinutes((iTime) * timeMinuteStep).ToString("HH:mm");
                        ascmAppointmentDistribution.endTime = dtStart.AddMinutes((iTime + 1) * timeMinuteStep).ToString("HH:mm");
                        ascmAppointmentDistribution.count = 0;
                        listAscmAppointmentDistribution_Tmp.Add(ascmAppointmentDistribution);
                    }
                    if (GetMinutes(dtAppointmentStartTime.ToString("HH:mm")) <= GetMinutes(ascmAppointmentDistribution.startTime) &&
                        GetMinutes(dtAppointmentEndTime.ToString("HH:mm")) > GetMinutes(ascmAppointmentDistribution.endTime))
                    {
                        ascmAppointmentDistribution.count += 1;
                    }
                }
                ////取均值 此功能在此暂时意义不大，但不要删除
                //int sum1 = (int)listAscmAppointmentDistribution_Tmp.Sum(p => p.count);
                //foreach (AscmAppointmentDistribution ascmAppointmentDistribution in listAscmAppointmentDistribution_Tmp)
                //{
                //    ascmAppointmentDistribution.count = ascmAppointmentDistribution.count / sum1;
                //}
                //3.比较合单可选时间段不同方案的最优选择，得出合单最早到货时间和最晚到货时间
                //供应商到厂放行时长
                List<AppointmentSection> listAppointmentSection = new List<AppointmentSection>();

                for (int itime = GetMinutes(dtAppointmentStartTime.ToString("HH:mm")); itime < GetMinutes(dtAppointmentEndTime.ToString("HH:mm")); itime += (int)(supplierPassDuration * 60))
                {
                    int item_start=itime;
                    int item_end=itime+ (int)(supplierPassDuration * 60);
                    AppointmentSection appointmentSection = new AppointmentSection();
                    appointmentSection.startTime = GetMinutes(itime);
                    appointmentSection.endTime = GetMinutes(item_end);
                    appointmentSection.value = 0;
                    for (int iTime = 0; iTime < 20; iTime++)
                    {
                        AscmAppointmentDistribution ascmAppointmentDistribution = listAscmAppointmentDistribution.Find(p=>p.timeId==iTime);
                        AscmAppointmentDistribution ascmAppointmentDistribution_tmp = listAscmAppointmentDistribution_Tmp.Find(p=>p.timeId==iTime);
                        if (item_start <= GetMinutes(ascmAppointmentDistribution.startTime) && item_end >= GetMinutes(ascmAppointmentDistribution.endTime))
                        {
                            appointmentSection.value += ascmAppointmentDistribution.count + ascmAppointmentDistribution_tmp.count;
                        }
                    }

                    listAppointmentSection.Add(appointmentSection);
                }
                //2014.3.5根据补充规则，多个合单时间不能超出交集范围 2014.3.9一张合单也这样处理
                //if (listAscmDeliBatSumMain.Count > 1)
                //{
                    if (listAppointmentSection.Count == 1)
                    {
                        if (GetMinutes(listAppointmentSection[0].endTime) > GetMinutes(dtAppointmentEndTime.ToString("HH:mm")))
                        {
                            listAppointmentSection[0].endTime = dtAppointmentEndTime.ToString("HH:mm");
                        }
                    }
                //}
                //4.取最小值
                AppointmentSection appointmentSection_min=null;
                foreach (AppointmentSection _appointmentSection in listAppointmentSection)
                {
                    if (appointmentSection_min == null)
                        appointmentSection_min = _appointmentSection;

                    if(appointmentSection_min.value>_appointmentSection.value)
                        appointmentSection_min = _appointmentSection;
                }
                //
                if (appointmentSection_min != null)
                {
                    //appointmentSection_min为供应商计算得出的最小落点时间
                    int appointmentSection_min_timePoint_start = GetTime(dtStart, appointmentSection_min.startTime, timeMinuteStep);
                    int appointmentSection_min_timePoint_end = GetTime(dtStart, appointmentSection_min.endTime, timeMinuteStep);
                    if (appointmentSection_min_timePoint_start != appointmentSection_min_timePoint_end && appointmentSection_min_timePoint_start < appointmentSection_min_timePoint_end)
                    {
                        decimal f1 = (decimal)1 / (appointmentSection_min_timePoint_end - appointmentSection_min_timePoint_start);
                        for (int appointmentSection_min_time = appointmentSection_min_timePoint_start; appointmentSection_min_time < appointmentSection_min_timePoint_end; appointmentSection_min_time++)
                        {
                            AscmAppointmentDistribution ascmAppointmentDistribution = listAscmAppointmentDistribution.Find(p => p.timeId == appointmentSection_min_time);
                            if (ascmAppointmentDistribution != null)
                            {
                                ascmAppointmentDistribution.count += f1;
                            }
                        }
                    }
                    //ascmDeliBatSumMain.appointmentStartTime = dtAppointmentStartTime.ToString("yyyy-MM-dd") + " " + appointmentSection_min.startTime;
                    //ascmDeliBatSumMain.appointmentEndTime = dtAppointmentStartTime.ToString("yyyy-MM-dd") + " " + appointmentSection_min.endTime;
                    //2013.11.28改为司机合单确认（多个合单）
                    //2014.3.24改为不替换时间
                    //foreach (AscmDeliBatSumMain ascmDeliBatSumMain in listAscmDeliBatSumMain)
                    //{
                    //    ascmDeliBatSumMain.appointmentStartTime = dtAppointmentStartTime.ToString("yyyy-MM-dd") + " " + appointmentSection_min.startTime;
                    //    ascmDeliBatSumMain.appointmentEndTime = dtAppointmentStartTime.ToString("yyyy-MM-dd") + " " + appointmentSection_min.endTime;
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public class AppointmentSection
        {
            ///<summary>开始时间</summary>
            public virtual string startTime { get; set; }
            ///<summary>结束时间</summary>
            public virtual string endTime { get; set; }
            ///<summary>值</summary>
            public virtual decimal value { get; set; }

            //辅助属性
        }
        private int GetTime(DateTime dtStart,string time,int timeMinuteStep)
        {
            int time1 = GetMinutes(time) - GetMinutes(dtStart.ToString("HH:mm"));
            int time2 = time1 / timeMinuteStep;

            //for (int iTimePoint = 0; iTimePoint < 20; iTimePoint++)
            //{
            //    if (dtStart.AddMinutes((iTimePoint) * timeStep).ToString("HH:mm") == time)
            //    {
            //        return iTimePoint;
            //    }
            //}
            return time2;
        }
        private int GetMinutes(string time)
        {
            string hour = time.Split(':')[0];
            string minute = time.Split(':')[1];
            return Convert.ToInt32(hour) * 60 + Convert.ToInt32(minute);
        }
        private string GetMinutes(int time)
        {
            int hour = time / 60;//取整时不进行四舍五入只取整数部分
            int minute = time % 60;//取模
            return YnBaseClass2.Helper.StringHelper.Repeat('0', 2 - hour.ToString().Length) +  hour + ":" + YnBaseClass2.Helper.StringHelper.Repeat('0', 2 - minute.ToString().Length)+ minute;
        }
        #endregion
    }
}
