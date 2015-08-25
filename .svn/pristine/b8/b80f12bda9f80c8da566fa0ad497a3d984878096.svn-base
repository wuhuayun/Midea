using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using System.IO;
using MideaAscm.Dal.GetMaterialManage.Entities;
using MideaAscm.Dal.FromErp.Entities;
using MideaAscm.Services.GetMaterialManage;
using MideaAscm.Services.Base;
using NHibernate;
using MideaAscm.Services.FromErp;
using MideaAscm.Dal;
using System.Collections;
using MideaAscm.Dal.IEntity;
using MideaAscm.Services.IEntity;

namespace MideaAscm.AM.Soft
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        bool result = false;
        private void frmMain_Load(object sender, EventArgs e)
        {
            
        }

        

        #region 自动生成任务
        public void AutoGenerateTask()
        {
            try
            {
                //获取数据源
                string whereOther = "", whereQueryWord = "", whereBomOther = "";

                string generateTaskDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
                whereQueryWord = "time like '" + generateTaskDate + "%'";
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                List<AscmDiscreteJobs> list_jobs = AscmDiscreteJobsService.GetInstance().GetList(null, "", "", "", whereOther, false);
                if (list_jobs == null || list_jobs.Count == 0)
                    throw new Exception("满足条件的排产单(作业)不存在或者作业数量为0 ！");

                string ids_jobs = string.Empty;
                if (list_jobs != null && list_jobs.Count > 0)
                {
                    foreach (AscmDiscreteJobs ascmDiscreteJobs in list_jobs)
                    {
                        if (!string.IsNullOrEmpty(ids_jobs) && ascmDiscreteJobs.wipEntityId > 0)
                            ids_jobs += ",";
                        if (ascmDiscreteJobs.wipEntityId > 0)
                            ids_jobs += ascmDiscreteJobs.wipEntityId;
                    }
                }

                whereQueryWord = "ami.wipSupplyType = 1";
                whereBomOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereBomOther, whereQueryWord);

                whereQueryWord = "awro.taskId = 0";
                whereBomOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereBomOther, whereQueryWord);

                List<AscmWipRequirementOperations> list_operations = AscmWipRequirementOperationsService.GetInstance().GetList(null, "", "", ids_jobs, whereBomOther);
                if (list_operations != null && list_operations.Count > 0)
                {
                    //生成领料任务
                    List<AscmGetMaterialTask> list = GenerateTask(list_operations, generateTaskDate);

                    if (list != null && list.Count > 0)
                    {
                        //保存任务
                        SaveTask(list, "", list_jobs);
                    }

                    //分配任务
                    AllocateTask("");
                }
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }

        //生成任务
        public List<AscmGetMaterialTask> GenerateTask(List<AscmWipRequirementOperations> list, string generateTaskDate)
        {
            List<AscmGetMaterialTask> listTask = new List<AscmGetMaterialTask>();

            try
            {
                List<AscmGenerateTaskRule> list_rule = AscmGenerateTaskRuleService.GetInstance().GetList(null, "identificationId,priority", "", "", "");
                if (list_rule == null || list_rule.Count == 0)
                    throw new Exception("生成领料任务规则不存在！");

                generateTaskDate = Convert.ToDateTime(generateTaskDate).ToString("yyyy-MM-dd");
                int iCount = YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmGetMaterialTask where uploadDate = '" + generateTaskDate + "'");

                foreach (AscmWipRequirementOperations ascmWipRequirmentOperations in list)
                {
                    foreach (AscmGenerateTaskRule ascmGenerateTaskRule in list_rule)
                    {
                        AscmGetMaterialTask ascmGetMaterialTask = new AscmGetMaterialTask();
                        ascmGetMaterialTask.taskId = getTaskId(iCount + listTask.Count + 1);
                        ascmGetMaterialTask.productLine = ascmWipRequirmentOperations.productLine;
                        ascmGetMaterialTask.IdentificationId = ascmWipRequirmentOperations.identificationId;
                        ascmGetMaterialTask.warehouserId = ascmWipRequirmentOperations.supplySubinventory;
                        ascmGetMaterialTask.materialDocNumber = ascmWipRequirmentOperations.docNumber;
                        ascmGetMaterialTask.materialType = ascmWipRequirmentOperations.wipSupplyType;
                        ascmGetMaterialTask.dateReleased = ascmWipRequirmentOperations.jobDate;

                        switch (ascmGetMaterialTask.IdentificationId)
                        {
                            case 1://总装
                                ascmGetMaterialTask.mtlCategoryStatus = ascmWipRequirmentOperations.zMtlCategoryStatus;
                                break;
                            case 2://电装
                                ascmGetMaterialTask.mtlCategoryStatus = ascmWipRequirmentOperations.dMtlCategoryStatus;
                                break;
                        }
                        ascmGetMaterialTask.uploadDate = generateTaskDate;
                        ascmGetMaterialTask.which = ascmWipRequirmentOperations.which;
                        ascmGetMaterialTask.rankerId = ascmWipRequirmentOperations.workerId;
                        ascmGetMaterialTask.taskTime = ascmWipRequirmentOperations.onlineTime;
                        ascmGetMaterialTask.status = AscmGetMaterialTask.StatusDefine.notAllocate;

                        if (ascmGenerateTaskRule.identificationId == ascmGetMaterialTask.IdentificationId)
                        {
                            string warehouseId = ascmWipRequirmentOperations.supplySubinventory.Substring(0, 4);
                            string docnumber = ascmWipRequirmentOperations.docNumber;
                            switch (ascmGenerateTaskRule.ruleType)
                            {
                                case AscmGenerateTaskRule.RuleTypeDefine.typeofPreStock:
                                    {
                                        if (ascmGetMaterialTask.mtlCategoryStatus == MtlCategoryStatusDefine.preStock)
                                        {
                                            bool isOk = IsJudgeCodeAndRanker(ascmGenerateTaskRule, ascmGetMaterialTask, warehouseId, docnumber, false, false);

                                            if (isOk)
                                            {
                                                ascmGetMaterialTask.ruleType = ascmGenerateTaskRule.ruleType;
                                                // 任务不存在
                                                if (ContainsTask(listTask, ascmGetMaterialTask) == null)
                                                {
                                                    if (ascmGetMaterialTask.listAscmWipRequirementOperations == null)
                                                        ascmGetMaterialTask.listAscmWipRequirementOperations = new List<AscmWipRequirementOperations>();
                                                    ascmGetMaterialTask.listAscmWipRequirementOperations.Add(ascmWipRequirmentOperations);

                                                    listTask.Add(ascmGetMaterialTask);
                                                }
                                                else
                                                {
                                                    //任务存在,判断是否包含该BOM
                                                    AscmGetMaterialTask task = ContainsTask(listTask, ascmGetMaterialTask);
                                                    if (ContainsOperations(task.listAscmWipRequirementOperations, ascmWipRequirmentOperations) == null)
                                                    {
                                                        task.listAscmWipRequirementOperations.Add(ascmWipRequirmentOperations);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case AscmGenerateTaskRule.RuleTypeDefine.typeofMixStock:
                                    {
                                        if (ascmGetMaterialTask.mtlCategoryStatus == MtlCategoryStatusDefine.mixStock)
                                        {
                                            bool isOk = IsJudgeCodeAndRanker(ascmGenerateTaskRule, ascmGetMaterialTask, warehouseId, docnumber, false, false);

                                            if (isOk)
                                            {
                                                ascmGetMaterialTask.ruleType = ascmGenerateTaskRule.ruleType;
                                                // 任务不存在
                                                if (ContainsTask(listTask, ascmGetMaterialTask) == null)
                                                {
                                                    if (ascmGetMaterialTask.listAscmWipRequirementOperations == null)
                                                        ascmGetMaterialTask.listAscmWipRequirementOperations = new List<AscmWipRequirementOperations>();
                                                    ascmGetMaterialTask.listAscmWipRequirementOperations.Add(ascmWipRequirmentOperations);

                                                    ascmGetMaterialTask.materialDocNumber = "";
                                                    listTask.Add(ascmGetMaterialTask);
                                                }
                                                else
                                                {
                                                    //任务存在,判断是否包含该BOM
                                                    AscmGetMaterialTask task = ContainsTask(listTask, ascmGetMaterialTask);
                                                    if (ContainsOperations(task.listAscmWipRequirementOperations, ascmWipRequirmentOperations) == null)
                                                    {
                                                        task.listAscmWipRequirementOperations.Add(ascmWipRequirmentOperations);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case AscmGenerateTaskRule.RuleTypeDefine.typeofWarehouse:
                                    {
                                        bool isOk = IsJudgeCodeAndRanker(ascmGenerateTaskRule, ascmGetMaterialTask, warehouseId, docnumber, false, false);

                                        if (isOk)
                                        {
                                            ascmGetMaterialTask.ruleType = ascmGenerateTaskRule.ruleType;
                                            // 任务不存在
                                            if (ContainsTask(listTask, ascmGetMaterialTask) == null)
                                            {
                                                if (ascmGetMaterialTask.listAscmWipRequirementOperations == null)
                                                    ascmGetMaterialTask.listAscmWipRequirementOperations = new List<AscmWipRequirementOperations>();
                                                ascmGetMaterialTask.listAscmWipRequirementOperations.Add(ascmWipRequirmentOperations);

                                                ascmGetMaterialTask.mtlCategoryStatus = "";
                                                ascmGetMaterialTask.materialDocNumber = "";
                                                listTask.Add(ascmGetMaterialTask);
                                            }
                                            else
                                            {
                                                //任务存在,判断是否包含该BOM
                                                AscmGetMaterialTask task = ContainsTask(listTask, ascmGetMaterialTask);
                                                if (ContainsOperations(task.listAscmWipRequirementOperations, ascmWipRequirmentOperations) == null)
                                                {
                                                    task.listAscmWipRequirementOperations.Add(ascmWipRequirmentOperations);
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case AscmGenerateTaskRule.RuleTypeDefine.typeofMaterial:
                                    {
                                        bool isOk = IsJudgeCodeAndRanker(ascmGenerateTaskRule, ascmGetMaterialTask, warehouseId, docnumber, false, false);

                                        if (isOk)
                                        {
                                            ascmGetMaterialTask.ruleType = ascmGenerateTaskRule.ruleType;
                                            // 任务不存在
                                            if (ContainsTask(listTask, ascmGetMaterialTask) == null)
                                            {
                                                if (ascmGetMaterialTask.listAscmWipRequirementOperations == null)
                                                    ascmGetMaterialTask.listAscmWipRequirementOperations = new List<AscmWipRequirementOperations>();
                                                ascmGetMaterialTask.listAscmWipRequirementOperations.Add(ascmWipRequirmentOperations);

                                                listTask.Add(ascmGetMaterialTask);
                                            }
                                            else
                                            {
                                                //任务存在,判断是否包含该BOM
                                                AscmGetMaterialTask task = ContainsTask(listTask, ascmGetMaterialTask);
                                                if (ContainsOperations(task.listAscmWipRequirementOperations, ascmWipRequirmentOperations) == null)
                                                {
                                                    task.listAscmWipRequirementOperations.Add(ascmWipRequirmentOperations);
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case AscmGenerateTaskRule.RuleTypeDefine.typeofProductLine:
                                    {
                                        bool isOk = IsJudgeCodeAndRanker(ascmGenerateTaskRule, ascmGetMaterialTask, warehouseId, docnumber, false, false);

                                        if (isOk)
                                        {
                                            ascmGetMaterialTask.ruleType = ascmGenerateTaskRule.ruleType;
                                            // 任务不存在
                                            if (ContainsTask(listTask, ascmGetMaterialTask) == null)
                                            {
                                                if (ascmGetMaterialTask.listAscmWipRequirementOperations == null)
                                                    ascmGetMaterialTask.listAscmWipRequirementOperations = new List<AscmWipRequirementOperations>();
                                                ascmGetMaterialTask.listAscmWipRequirementOperations.Add(ascmWipRequirmentOperations);

                                                ascmGetMaterialTask.materialDocNumber = "";
                                                listTask.Add(ascmGetMaterialTask);
                                            }
                                            else
                                            {
                                                //任务存在,判断是否包含该BOM
                                                AscmGetMaterialTask task = ContainsTask(listTask, ascmGetMaterialTask);
                                                if (ContainsOperations(task.listAscmWipRequirementOperations, ascmWipRequirmentOperations) == null)
                                                {
                                                    task.listAscmWipRequirementOperations.Add(ascmWipRequirmentOperations);
                                                }
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("生成任务失败(Generate GetMaterialTask) ", ex);
                throw ex;
            }

            return listTask;
        }
        #region 生成任务相关方法
        //判断规则及指定关系人
        public bool IsJudgeCodeAndRanker(AscmGenerateTaskRule ascmGenerateTaskRule, AscmGetMaterialTask ascmGetMaterialTask, string warehouseId, string docnumber, bool isRule, bool isRaner)
        {
            if (!string.IsNullOrEmpty(ascmGenerateTaskRule.ruleCode))
            {
                string[] myArray = ascmGenerateTaskRule.ruleCode.Split('&');
                string warehouseString = myArray[0].Substring(myArray[0].IndexOf("(") + 1, myArray[0].IndexOf(")") - myArray[0].IndexOf("(") - 1);
                string materialString = myArray[1].Substring(myArray[1].IndexOf("(") + 1, myArray[1].IndexOf(")") - myArray[1].IndexOf("(") - 1);

                if (warehouseString.Length > 0 && warehouseString.IndexOf(warehouseId) > -1)
                {
                    #region
                    if (materialString.Length > 0)
                    {
                        if (materialString.IndexOf("|") > -1)
                        {
                            string[] mtlArray = materialString.Split('|');
                            foreach (string mtl in mtlArray)
                            {
                                if (mtl.IndexOf(warehouseId) > -1)
                                {
                                    string material = mtl.Substring(mtl.IndexOf(":") + 1, mtl.Length - mtl.IndexOf(":") - 1);
                                    if (material.IndexOf("%") > -1)
                                    {
                                        string[] materialArray = material.Split('%');
                                        foreach (string item in materialArray)
                                        {
                                            if (item == docnumber.Substring(0, item.Length))
                                            {
                                                isRule = true;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (material == docnumber.Substring(0, material.Length))
                                        {
                                            isRule = true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (materialString.IndexOf(warehouseId) > -1)
                            {
                                string material = materialString.Substring(materialString.IndexOf(":") + 1, materialString.Length - materialString.IndexOf(":") - 1);
                                if (material.IndexOf("%") > -1)
                                {
                                    string[] materialArray = material.Split('%');
                                    foreach (string item in materialArray)
                                    {
                                        if (item == docnumber.Substring(0, item.Length))
                                        {
                                            isRule = true;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (material == docnumber.Substring(0, material.Length))
                                    {
                                        isRule = true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        isRule = true;
                    }
                    #endregion
                }
            }
            else
            {
                isRule = true;
            }

            if (!string.IsNullOrEmpty(ascmGenerateTaskRule.relatedRanker))
            {
                if (ascmGenerateTaskRule.relatedRanker == ascmGetMaterialTask.rankerId)
                    isRaner = true;
            }
            else
            {
                isRaner = true;
            }

            return (isRule && isRaner);
        }
        //判断是否包含任务
        public AscmGetMaterialTask ContainsTask(List<AscmGetMaterialTask> listTask, AscmGetMaterialTask ascmGetMaterialTask)
        {
            if (listTask.Count == 0)
                return null;

            switch (ascmGetMaterialTask.ruleType)
            {
                case AscmGenerateTaskRule.RuleTypeDefine.typeofPreStock:
                    {
                        foreach (AscmGetMaterialTask item in listTask)
                        {
                            if (item.rankerId == ascmGetMaterialTask.rankerId && item.uploadDate == ascmGetMaterialTask.uploadDate && item.IdentificationId == ascmGetMaterialTask.IdentificationId && item.warehouserId == ascmGetMaterialTask.warehouserId && item.materialDocNumber == ascmGetMaterialTask.materialDocNumber && item.mtlCategoryStatus == ascmGetMaterialTask.mtlCategoryStatus && item.productLine == ascmGetMaterialTask.productLine && item.taskTime == ascmGetMaterialTask.taskTime && item.which == ascmGetMaterialTask.which && item.dateReleased == ascmGetMaterialTask.dateReleased)
                                return item;
                        }
                    }
                    break;
                case AscmGenerateTaskRule.RuleTypeDefine.typeofMixStock:
                    {
                        foreach (AscmGetMaterialTask item in listTask)
                        {
                            if (item.rankerId == ascmGetMaterialTask.rankerId && item.uploadDate == ascmGetMaterialTask.uploadDate && item.IdentificationId == ascmGetMaterialTask.IdentificationId && item.warehouserId == ascmGetMaterialTask.warehouserId && item.mtlCategoryStatus == ascmGetMaterialTask.mtlCategoryStatus && item.productLine == ascmGetMaterialTask.productLine && item.taskTime == ascmGetMaterialTask.taskTime && item.which == ascmGetMaterialTask.which && item.dateReleased == ascmGetMaterialTask.dateReleased)
                                return item;
                        }
                    }
                    break;
                case AscmGenerateTaskRule.RuleTypeDefine.typeofWarehouse:
                    {
                        foreach (AscmGetMaterialTask item in listTask)
                        {
                            if (item.rankerId == ascmGetMaterialTask.rankerId && item.uploadDate == ascmGetMaterialTask.uploadDate && item.IdentificationId == ascmGetMaterialTask.IdentificationId && item.warehouserId == ascmGetMaterialTask.warehouserId && item.productLine == ascmGetMaterialTask.productLine && item.taskTime == ascmGetMaterialTask.taskTime && item.which == ascmGetMaterialTask.which && item.dateReleased == ascmGetMaterialTask.dateReleased)
                                return item;
                        }
                    }
                    break;
                case AscmGenerateTaskRule.RuleTypeDefine.typeofMaterial:
                    {
                        foreach (AscmGetMaterialTask item in listTask)
                        {
                            if (item.rankerId == ascmGetMaterialTask.rankerId && item.uploadDate == ascmGetMaterialTask.uploadDate && item.IdentificationId == ascmGetMaterialTask.IdentificationId && item.warehouserId == ascmGetMaterialTask.warehouserId && item.productLine == ascmGetMaterialTask.productLine && item.taskTime == ascmGetMaterialTask.taskTime && item.materialDocNumber == ascmGetMaterialTask.materialDocNumber && item.which == ascmGetMaterialTask.which && item.dateReleased == ascmGetMaterialTask.dateReleased)
                                return item;
                        }
                    }
                    break;
                case AscmGenerateTaskRule.RuleTypeDefine.typeofProductLine:
                    {
                        foreach (AscmGetMaterialTask item in listTask)
                        {
                            if (item.rankerId == ascmGetMaterialTask.rankerId && item.uploadDate == ascmGetMaterialTask.uploadDate && item.IdentificationId == ascmGetMaterialTask.IdentificationId && item.warehouserId == ascmGetMaterialTask.warehouserId && item.productLine == ascmGetMaterialTask.productLine && item.taskTime == ascmGetMaterialTask.taskTime && item.which == ascmGetMaterialTask.which && item.dateReleased == ascmGetMaterialTask.dateReleased)
                                return item;
                        }
                    }
                    break;

            }

            return null;
        }
        //判断是否包含BOM
        public AscmWipRequirementOperations ContainsOperations(List<AscmWipRequirementOperations> listAscmWipRequirementOperations, AscmWipRequirementOperations ascmWipRequirementOperations)
        {
            if (listAscmWipRequirementOperations.Count == 0)
                return null;

            foreach (AscmWipRequirementOperations item in listAscmWipRequirementOperations)
            {
                if (item.id == ascmWipRequirementOperations.id)
                    return item;
            }

            return null;
        }

        public string getTaskId(int number)
        {
            string autoTask = "T";

            int a = number / 1000;
            int b = (number - a * 1000) / 100;
            int c = (number - a * 1000 - b * 100) / 10;
            int d = (number - a * 1000 - b * 100 - c * 10);

            return autoTask + a.ToString() + b.ToString() + c.ToString() + d.ToString();
        }
        #endregion

        //保存任务
        public void SaveTask(List<AscmGetMaterialTask> list, string userName, List<AscmDiscreteJobs> list_jobs)
        {
            int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmGetMaterialTask");

            List<AscmWipRequirementOperations> listAscmWipRequirementOperations = new List<AscmWipRequirementOperations>();
            int count = 0;
            foreach (AscmGetMaterialTask ascmGetMaterialTask in list)
            {
                ascmGetMaterialTask.id = maxId + 1 + count;
                ascmGetMaterialTask.createUser = userName;
                ascmGetMaterialTask.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                ascmGetMaterialTask.modifyUser = userName;
                ascmGetMaterialTask.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                foreach (AscmWipRequirementOperations ascmWipRequirementOperations in ascmGetMaterialTask.listAscmWipRequirementOperations)
                {
                    ascmWipRequirementOperations.taskId = ascmGetMaterialTask.id;
                    ascmWipRequirementOperations.modifyUser = userName;
                    ascmWipRequirementOperations.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                    listAscmWipRequirementOperations.Add(ascmWipRequirementOperations);
                }
                count++;
            }

            foreach (AscmDiscreteJobs ascmDiscreteJobs in list_jobs)
            {
                ascmDiscreteJobs.status = 2;
                ascmDiscreteJobs.modifyUser = userName;
                ascmDiscreteJobs.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            }

            //执行事务
            ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
            session.Clear();
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    //保存生成任务
                    if (list != null)
                        AscmGetMaterialTaskService.GetInstance().Save(list);
                    //更新关联作业BOM
                    if (listAscmWipRequirementOperations != null)
                        listAscmWipRequirementOperations.ForEach(P => session.Merge(P));
                    //更新关联作业状态
                    if (list_jobs != null)
                        AscmDiscreteJobsService.GetInstance().Update(list_jobs);
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    YnBaseClass2.Helper.LogHelper.GetLog().Error("生成任务失败(Generate AscmGetMateiralTask)", ex);
                }
            }
        }

        //分配任务
        public void AllocateTask(string userName)
        {
            try
            {
                string whereTaskOther = "", whereQueryWord = "", whereRuleOther = "";

                whereQueryWord = "status = '" + AscmGetMaterialTask.StatusDefine.notAllocate + "'";
                whereTaskOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereTaskOther, whereQueryWord);

                List<AscmGetMaterialTask> listAscmGetMaterialTask = AscmGetMaterialTaskService.GetInstance().GetList(null, "", "", "", whereTaskOther);
                if (listAscmGetMaterialTask == null || listAscmGetMaterialTask.Count == 0)
                    throw new Exception("须分配任务不存在！");

                List<AscmAllocateRule> listAscmAllocateRule = AscmAllocateRuleService.GetInstance().GetList(null, "", "", "", whereRuleOther);

                if ((listAscmGetMaterialTask != null && listAscmGetMaterialTask.Count > 0) && (listAscmAllocateRule != null && listAscmAllocateRule.Count > 0))
                {
                    Allocate(listAscmGetMaterialTask, listAscmAllocateRule, userName);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("分配任务失败(Allocate GetMaterialTask) ", ex);
                throw ex;
            }
        }
        public void Allocate(List<AscmGetMaterialTask> listTask, List<AscmAllocateRule> listRule, string userName)
        {
            List<AscmAllocateRule> listNewRule = null;

            foreach (AscmGetMaterialTask ascmGetMaterialTask in listTask)
            {
                #region 查找符合领料任务条件的领料员
                List<string> namelist = new List<string>();
                foreach (AscmAllocateRule ascmAllocateRule in listRule)
                {
                    string taskType = ascmGetMaterialTask.taskId.Substring(0, 1);
                    string sName = string.Empty;
                    if (taskType == "T")
                    {
                        //自动任务分配
                        if (!string.IsNullOrEmpty(ascmAllocateRule.ruleCode))
                        {
                            if (ascmGetMaterialTask.IdentificationId == 1)//总装
                            {
                                if (!string.IsNullOrEmpty(ascmAllocateRule.zRankerName))//对接总装排产员的领料员
                                {
                                    if (ascmAllocateRule.zRankerName == ascmGetMaterialTask.rankerId)
                                    {
                                        string[] strArr = ascmAllocateRule.ruleCode.Split('&');
                                        if (string.IsNullOrEmpty(ascmGetMaterialTask.mtlCategoryStatus))
                                        {
                                            #region 特殊子库
                                            string ruleCode = strArr[2].ToString();
                                            if (ruleCode.Length > 2)
                                            {
                                                string WarehouseStr = ruleCode.Substring(ruleCode.IndexOf('(') + 1, ruleCode.IndexOf(')') - ruleCode.IndexOf('(') - 1);
                                                string MaterialStr = ruleCode.Substring(ruleCode.LastIndexOf('(') + 1, ruleCode.LastIndexOf(')') - ruleCode.LastIndexOf('(') - 1);
                                                string Warehouse = ascmGetMaterialTask.warehouserId.Substring(0, 4);
                                                if (WarehouseStr.IndexOf(Warehouse) > -1)
                                                {
                                                    sName = ascmAllocateRule.id.ToString();
                                                    namelist.Add(sName);
                                                }
                                            }
                                            #endregion
                                        }
                                        else if (ascmGetMaterialTask.mtlCategoryStatus == "PRESTOCK")
                                        {
                                            #region 须备料
                                            string ruleCode = strArr[1].ToString();
                                            if (ruleCode.Length > 2)
                                            {
                                                string Status = ruleCode.Substring(ruleCode.IndexOf('[') + 1, ruleCode.IndexOf('(') - 1);
                                                string WarehouseStr = ruleCode.Substring(ruleCode.IndexOf('(') + 1, ruleCode.IndexOf(')') - ruleCode.IndexOf('(') - 1);
                                                string MaterialStr = ruleCode.Substring(ruleCode.LastIndexOf('(') + 1, ruleCode.LastIndexOf(')') - ruleCode.LastIndexOf('(') - 1);
                                                if (Status == "须备料")
                                                {
                                                    string Warehouse = ascmGetMaterialTask.warehouserId.Substring(0, 4);
                                                    if (WarehouseStr.IndexOf(Warehouse) > -1)
                                                    {
                                                        if (MaterialStr.Length > 2)
                                                        {
                                                            if (MaterialStr.IndexOf('|') > -1)
                                                            {
                                                                #region 有分割子库筛选物料
                                                                string[] MaterialArray = MaterialStr.Split('|');
                                                                foreach (string item1 in MaterialArray)
                                                                {
                                                                    string MaterialDocnumberStr = item1.Substring(item1.IndexOf(':') + 1, item1.Length - item1.IndexOf(':') - 1);
                                                                    string MaterialWarehouseStr = item1.Substring(0, item1.IndexOf(':'));
                                                                    if (MaterialDocnumberStr.IndexOf('%') > -1)
                                                                    {
                                                                        string[] MaterialDocnumberArray = MaterialDocnumberStr.Split('%');
                                                                        foreach (string item2 in MaterialDocnumberArray)
                                                                        {
                                                                            if (ascmGetMaterialTask.materialDocNumber.IndexOf(item2) > -1 && MaterialWarehouseStr == Warehouse)
                                                                            {
                                                                                sName = ascmAllocateRule.id.ToString();
                                                                                namelist.Add(sName);
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (ascmGetMaterialTask.materialDocNumber.IndexOf(MaterialDocnumberStr) > -1 && MaterialWarehouseStr == Warehouse)
                                                                        {
                                                                            sName = ascmAllocateRule.id.ToString();
                                                                            namelist.Add(sName);
                                                                        }
                                                                    }
                                                                }
                                                                #endregion
                                                            }
                                                            else
                                                            {
                                                                #region 无分割子库筛选物料
                                                                string MaterialDocnumberStr = MaterialStr.Substring(MaterialStr.IndexOf(':') + 1, MaterialStr.Length - MaterialStr.IndexOf(':') - 1);
                                                                string MaterialWarehouseStr = MaterialStr.Substring(0, MaterialStr.IndexOf(':'));
                                                                if (MaterialDocnumberStr.IndexOf('%') > -1)
                                                                {
                                                                    string[] MaterialDocnumberArray = MaterialDocnumberStr.Split('%');
                                                                    foreach (string item2 in MaterialDocnumberArray)
                                                                    {
                                                                        if (ascmGetMaterialTask.materialDocNumber.IndexOf(item2) > -1 && MaterialWarehouseStr == Warehouse)
                                                                        {
                                                                            sName = ascmAllocateRule.id.ToString();
                                                                            namelist.Add(sName);
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (ascmGetMaterialTask.materialDocNumber.IndexOf(MaterialDocnumberStr) > -1 && MaterialWarehouseStr == Warehouse)
                                                                    {
                                                                        sName = ascmAllocateRule.id.ToString();
                                                                        namelist.Add(sName);
                                                                    }
                                                                }
                                                                #endregion
                                                            }
                                                        }
                                                        else
                                                        {
                                                            sName = ascmAllocateRule.id.ToString();
                                                            namelist.Add(sName);
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                        else if (ascmGetMaterialTask.mtlCategoryStatus == "MIXSTOCK")
                                        {
                                            #region 须配料
                                            string ruleCode = strArr[0].ToString();
                                            if (ruleCode.Length > 2)
                                            {
                                                string Status = ruleCode.Substring(ruleCode.IndexOf('[') + 1, ruleCode.IndexOf('(') - 1);
                                                string WarehouseStr = ruleCode.Substring(ruleCode.IndexOf('(') + 1, ruleCode.IndexOf(')') - ruleCode.IndexOf('(') - 1);
                                                string MaterialStr = ruleCode.Substring(ruleCode.LastIndexOf('(') + 1, ruleCode.LastIndexOf(')') - ruleCode.LastIndexOf('(') - 1);
                                                if (Status == "须配料")
                                                {
                                                    string Warehouse = ascmGetMaterialTask.warehouserId.Substring(0, 4);
                                                    if (WarehouseStr.IndexOf(Warehouse) > -1)
                                                    {
                                                        sName = ascmAllocateRule.id.ToString();
                                                        namelist.Add(sName);
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(ascmAllocateRule.dRankerName))//对接电装排产员的领料员
                                {
                                    if (ascmAllocateRule.dRankerName == ascmGetMaterialTask.rankerId)
                                    {
                                        string[] strArr = ascmAllocateRule.ruleCode.Split('&');
                                        if (string.IsNullOrEmpty(ascmGetMaterialTask.mtlCategoryStatus))
                                        {
                                            #region 特殊子库
                                            string ruleCode = strArr[2].ToString();
                                            if (ruleCode.Length > 2)
                                            {
                                                string WarehouseStr = ruleCode.Substring(ruleCode.IndexOf('(') + 1, ruleCode.IndexOf(')') - ruleCode.IndexOf('(') - 1);
                                                string MaterialStr = ruleCode.Substring(ruleCode.LastIndexOf('(') + 1, ruleCode.LastIndexOf(')') - ruleCode.LastIndexOf('(') - 1);
                                                string Warehouse = ascmGetMaterialTask.warehouserId.Substring(0, 4);
                                                if (WarehouseStr.IndexOf(Warehouse) > -1)
                                                {
                                                    sName = ascmAllocateRule.id.ToString();
                                                    namelist.Add(sName);
                                                }
                                            }
                                            #endregion
                                        }
                                        else if (ascmGetMaterialTask.mtlCategoryStatus == "MIXSTOCK")
                                        {
                                            #region 须配料
                                            string ruleCode = strArr[0].ToString();
                                            if (ruleCode.Length > 2)
                                            {
                                                string Status = ruleCode.Substring(ruleCode.IndexOf('[') + 1, ruleCode.IndexOf('(') - 1);
                                                string WarehouseStr = ruleCode.Substring(ruleCode.IndexOf('(') + 1, ruleCode.IndexOf(')') - ruleCode.IndexOf('(') - 1);
                                                string MaterialStr = ruleCode.Substring(ruleCode.LastIndexOf('(') + 1, ruleCode.LastIndexOf(')') - ruleCode.LastIndexOf('(') - 1);
                                                if (Status == "须配料")
                                                {
                                                    string Warehouse = ascmGetMaterialTask.warehouserId.Substring(0, 4);
                                                    if (WarehouseStr.IndexOf(Warehouse) > -1)
                                                    {
                                                        sName = ascmAllocateRule.id.ToString();
                                                        namelist.Add(sName);
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (taskType == "L")
                    {
                        if (!string.IsNullOrEmpty(ascmGetMaterialTask.tip))
                        {
                            if (!string.IsNullOrEmpty(ascmAllocateRule.other))
                            {
                                if (ascmAllocateRule.other.IndexOf(ascmGetMaterialTask.tip.ToString()) > -1)
                                {
                                    sName = ascmAllocateRule.id.ToString();
                                    namelist.Add(sName);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region 按平衡原则指定责任人
                if (namelist != null && namelist.Count > 0)
                {
                    string ids = string.Empty, id = string.Empty, count = string.Empty, worker = string.Empty;
                    foreach (string name in namelist)
                    {
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids += name;
                    }

                    string where = "id in (" + ids + ")";
                    if (!string.IsNullOrEmpty(where))
                    {
                        IList<AscmAllocateRule> ilistAllocateRule = AscmAllocateRuleService.GetInstance().GetList(null, "taskCount,id", "", "", where);
                        if (ilistAllocateRule != null && ilistAllocateRule.Count > 0)
                        {
                            List<AscmAllocateRule> listAllocateRule = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmAllocateRule>(ilistAllocateRule);

                            ids = listAllocateRule[0].id.ToString();
                            count = listAllocateRule[0].taskCount.ToString();
                            worker = listAllocateRule[0].workerName.ToString();

                            //获取责任人的物流组
                            string userLogisticsClass = AscmUserInfoService.GetInstance().GetUserLogisticsName(worker);

                            ascmGetMaterialTask.workerId = worker;
                            ascmGetMaterialTask.status = "NOTEXECUTE";
                            ascmGetMaterialTask.logisticsClass = userLogisticsClass;
                            ascmGetMaterialTask.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            ascmGetMaterialTask.modifyUser = userName;

                            AscmAllocateRule ascmAllocateRule = AscmAllocateRuleService.GetInstance().Get(int.Parse(ids));
                            ascmAllocateRule.taskCount = int.Parse(count) + 1;
                            ascmAllocateRule.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            ascmAllocateRule.modifyUser = userName;
                            AscmAllocateRuleService.GetInstance().Update(ascmAllocateRule);
                        }
                    }
                }
                #endregion
            }

            //执行事务
            ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
            session.Clear();
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    //更新任务列表
                    if (listTask != null)
                        AscmGetMaterialTaskService.GetInstance().Update(listTask);
                    //更新分配规则列表
                    //if (listNewRule != null)
                    //    listNewRule.ForEach(P => session.Merge(P));
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    YnBaseClass2.Helper.LogHelper.GetLog().Error("分配任务失败(Allocate AscmGetMateiralTask)", ex);
                }
            }
        }
        #endregion

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (!timer1.Enabled)
                timer1.Enabled = true;

            toolStripStatusLabel1.Text = "已开启服务！";
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
                timer1.Enabled = false;

            toolStripStatusLabel1.Text = "已关闭服务！";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string nowString = DateTime.Now.ToShortTimeString();

            if (nowString == "23:50")
                AutoGenerateTask();

            if (InvokeModulInterface())
                result = false;
        }

        public bool InvokeModulInterface()
        {
            try
            {
                string whereOther = "", whereQueryWord = "";

                whereQueryWord = "status = " + AscmGetMaterialLog.StatusDefine.normalStatus;
                whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, whereQueryWord);

                List<AscmGetMaterialLog> list = AscmGetMaterialLogService.GetInstance().GetList(null, "", "", "", whereOther);
                if (list != null && list.Count > 0)
                {
                    List<WmsAndLogistics> InterfaceList = new List<WmsAndLogistics>();
                    foreach (AscmGetMaterialLog ascmGetMaterialLog in list)
                    {
                        WmsAndLogistics wmsAndLogistics = new WmsAndLogistics();
                        wmsAndLogistics.wipEntityId = ascmGetMaterialLog.wipEntityId;
                        wmsAndLogistics.materialId = ascmGetMaterialLog.materialId;
                        wmsAndLogistics.warehouseId = ascmGetMaterialLog.warehouseId;
                        wmsAndLogistics.quantity = ascmGetMaterialLog.quantity;
                        wmsAndLogistics.preparationString = ascmGetMaterialLog.preparationString;
                        wmsAndLogistics.workerId = ascmGetMaterialLog.workerId;

                        InterfaceList.Add(wmsAndLogistics);
                    }

                    //调用接口
                    WmsAndLogisticsService.GetInstance().DoMaterialRequisition(InterfaceList);
                    foreach (AscmGetMaterialLog ascmGetMaterialLog in list)
                    {
                        ascmGetMaterialLog.status = AscmGetMaterialLog.StatusDefine.abnormalSatus;
                    }
                    //更新领料信息
                    AscmGetMaterialLogService.GetInstance().Update(list);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("确认领料失败！(Confrim AscmGetMateiralTask)", ex);
                throw ex;
            }

            return result;
        }
    }
}
