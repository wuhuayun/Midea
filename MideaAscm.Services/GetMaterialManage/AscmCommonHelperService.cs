using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using MideaAscm;
using MideaAscm.Dal.FromErp.Entities;

namespace MideaAscm.Services.GetMaterialManage
{
    public class AscmCommonHelperService
    {
        private static AscmCommonHelperService service;
        public static AscmCommonHelperService GetInstance()
        {
            if (service == null)
                service = new AscmCommonHelperService();
            return service;
        }

        /// <summary>特殊子库配置参数</summary>
        public string SpecWareHouseConfigParam = ConfigurationManager.AppSettings["SpecWareHouseConfigParam"].ToString();
        /// <summary>任务开头字母配置参数</summary>
        public string TaskStartWordsConfigParam = ConfigurationManager.AppSettings["TaskStartWordsConfigParam"].ToString();
        /// <summary>兼容上传排产单数值参数</summary>
        public static string DiscreteJobsImportParam = ConfigurationManager.AppSettings["DiscreteJobsImportParam"].ToString();
        /// <summary>特殊子库关联物料参数</summary>
        public string SpecWmRelatedMaterialParam = ConfigurationManager.AppSettings["SpecWmRelatedMaterialParam"].ToString();
        /// <summary>排产车间参数配置</summary>
        public string LogisticClassConfigParam = ConfigurationManager.AppSettings["LogisticClassConfigParam"].ToString();
        /// <summary>兼容上传物料备料形式数值参数</summary>
        public static string MaterialStockFormatImportParam = ConfigurationManager.AppSettings["MaterialStockFormatImportParam"].ToString();

        #region 任务开头字母参数配置
        public List<string> GetTaskConfigList()
        {
            List<string> list = new List<string>();
            try
            {
                if (!string.IsNullOrEmpty(TaskStartWordsConfigParam))
                {
                    if (TaskStartWordsConfigParam.IndexOf("|") > -1)
                    {
                        string[] taskConfigArray = TaskStartWordsConfigParam.Split('|');
                        foreach (string item in taskConfigArray)
                        {
                            list.Add(item);
                        }
                    }
                    else
                    {
                        list.Add(TaskStartWordsConfigParam);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return list;
        }
        public string GetConfigTaskWords(int value)
        {
            try
            {
                List<string> list = GetTaskConfigList();
                if (list != null && list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (i == value)
                        {
                            return list[i].Substring(0, 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "";
        }
        public int GetConfigTaskLength(string TaskWords)
        {
            int Length = 0;
            try
            {
                List<string> list = GetTaskConfigList();
                if (list != null && list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].IndexOf(TaskWords) > -1)
                        { 
                            Length = Convert.ToInt16(list[i].Substring(list[i].IndexOf(TaskWords) + 1, 1)); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
                
            return Length;
        }
        #endregion

        #region 特殊子库配置参数
        public List<string> GetSpecWareHouseArray()
        {
            List<string> list = new List<string>();
            try
            {
                if (!string.IsNullOrEmpty(SpecWareHouseConfigParam))
                {
                    if (SpecWareHouseConfigParam.IndexOf("|") > -1)
                    {
                        string[] warehouseConfigArray = SpecWareHouseConfigParam.Split('|');
                        foreach (string item in warehouseConfigArray)
                        {
                            list.Add(item);
                        }
                    }
                    else
                    {
                        list.Add(SpecWareHouseConfigParam);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return list;
        }
        public bool IsJudgeSpecWareHouse(string value)
        {
            List<string> list = GetSpecWareHouseArray();
            if (list != null && list.Count > 0)
            {
                foreach (string item in list)
                {
                    if (item == value)
                        return true;
                }
            }

            return false;
        }
        #endregion

        #region 特殊子库关联物料参数配置
        public string GetSpecWmRelatedMaterialParam()
        {
            if (!string.IsNullOrEmpty(SpecWmRelatedMaterialParam))
                return SpecWmRelatedMaterialParam.Trim();
            return "";
        }
        /// <summary>
        /// 获取参数List
        /// </summary>
        /// <returns></returns>
        public List<string> GetSpecWmRelatedMaterialParamList()
        {
            List<string> list = new List<string>();
            string param = GetSpecWmRelatedMaterialParam();
            if (!string.IsNullOrEmpty(param))
            {
                if (param.IndexOf("|") > -1)
                {
                    string[] paramArray = param.Split('|');
                    foreach (string item in paramArray)
                    {
                        list.Add(item);
                    }
                }
                else
                {
                    list.Add(param);
                }
            }

            return list;
        }
        /// <summary>
        /// 获取物料所有特殊子库
        /// </summary>
        /// <returns></returns>
        public string GetSpecMaterialRelatedWm()
        {
            string param = GetSpecWmRelatedMaterialParam();
            string specWmString = "";
            if (!string.IsNullOrEmpty(param))
            {
                if (param.IndexOf('|') > -1)
                {
                    string[] specWmArray = param.Split('|');
                    foreach (string item in specWmArray)
                    {
                        if (!string.IsNullOrEmpty(specWmString))
                            specWmString += ",";
                        specWmString += item.Substring(param.IndexOf("[") + 1, 4);
                    }
                }
                else
                {
                    specWmString = param.Substring(param.IndexOf("[") + 1, 4);
                }
            }

            return specWmString;
        }
        /// <summary>
        /// 获取该特殊子库的特殊物料
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        public string GetSpecWmRelatedMaterial(string warehouseId)
        {
            string warehouse = warehouseId.Substring(0, 4);
            List<string> list = GetSpecWmRelatedMaterialParamList();
            string specMaterial = "";
            if (list != null && list.Count > 0)
            {
                foreach (string item in list)
                {
                    if (item.IndexOf(warehouse) > -1)
                    {
                        specMaterial = item.Substring(item.IndexOf(":") + 1, item.IndexOf("]") - item.IndexOf(":") - 1);
                    }
                }
            }

            return specMaterial;
        }
        /// <summary>
        /// 判断特殊子库与特殊物料是否存在
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="docnumber"></param>
        /// <returns></returns>
        public bool JudgeSpecWmRelatedMaterial(string warehouseId, string docnumber)
        {
            if (string.IsNullOrEmpty(warehouseId))
                return false;
            if (string.IsNullOrEmpty(docnumber))
                return false;

            string warehouse = warehouseId.Substring(0, 4);
            List<string> list = GetSpecWmRelatedMaterialParamList();
            if (list != null && list.Count > 0)
            {
                foreach (string item in list)
                {
                    if (item.IndexOf(warehouse) > -1)
                    {
                        string materialString = item.Substring(item.IndexOf(":") + 1, item.IndexOf("]") - item.IndexOf(":") - 1);
                        if (materialString.IndexOf("*") > -1)
                        {
                            string[] materialArray = materialString.Split('*');
                            foreach (string material in materialArray)
                            {
                                if (material == docnumber.Substring(0, material.Length))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }
        #endregion

        #region 排产车间参数配置
        /// <summary>
        /// 获取配置参数物流组List
        /// </summary>
        /// <returns></returns>
        public List<string> GetLogisticsClassList()
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(LogisticClassConfigParam))
            {
                if (LogisticClassConfigParam.IndexOf("|") > -1)
                {
                    string[] logisticClassArray = LogisticClassConfigParam.Split('|');
                    foreach (string item in logisticClassArray)
                    {
                        list.Add(item);
                    }
                }
                else
                {
                    list.Add(LogisticClassConfigParam);
                }
            }

            return list;
        }
        /// <summary>
        /// 获取英文物流组List
        /// </summary>
        /// <returns></returns>
        public List<string> GetLogisticsClassRelfectList()
        {
            List<string> list = GetLogisticsClassList();
            List<string> relfectList = new List<string>();
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    relfectList.Add("CLASS" + (i + 1).ToString());
                }
            }

            return relfectList;
        }
        /// <summary>
        /// 中文显示物流班组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string DisplayLogisticsClass(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                List<string> list = GetLogisticsClassList();
                if (list != null && list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(value) && value.Substring(5, value.Length - 5)  == (i + 1).ToString())
                        {
                            return list[i].ToString();
                        }
                    }
                }
            }

            return "";
        }
        /// <summary>
        /// 英文显示物流组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string DisplayEnglishLogisticsClass(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                List<string> list = GetLogisticsClassList();
                if (list != null && list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(value) && value == list[i].ToString())
                        {
                            List<string> relfectList = GetLogisticsClassRelfectList();
                            if (relfectList != null && relfectList.Count > 0)
                                return relfectList[i].ToString();
                        }
                    }
                }
            }

            return "";
        }

        /// <summary>
        /// 获取物流组名称的长度
        /// </summary>
        /// <returns></returns>
        public int GetLogisticsClassCount()
        {
            List<string> list = GetLogisticsClassList();
            if (list != null && list.Count > 0)
            {
                int temp = 0;
                int times = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    if (i == 0)
                    {
                        temp = list[i].Length;
                    }
                    else
                    {
                        if (temp == list[i].Length)
                        {
                            times++;
                        }
                    }
                }

                if (times == (list.Count - 1))
                {
                    return temp;
                }
            }

            return 0;
        }
        #endregion

        #region sql拼接字符串公用方法
        /// <summary>
        /// sql拼接字符串公用方法
        /// </summary>
        /// <param name="ids">拼接字符串</param>
        /// <param name="condition">条件范围</param>
        /// <returns></returns>
        public string IsJudgeListCount(string ids, string condition)
        {
            bool flag = false;
            string where = "", whereQueryWord = "";
            if (!string.IsNullOrEmpty(ids))
            {
                if (ids.IndexOf(',') > -1)
                {
                    string[] strArray = ids.Split(',');


                    if (strArray.Length >= 1000)
                    {
                        decimal temp = Convert.ToDecimal(strArray.Length) / 1000;
                        int count = Convert.ToInt16(Math.Ceiling(temp));
                        for (int i = 0; i < count; i++)
                        {
                            string str = string.Empty;
                            for (int j = i * 1000; j < strArray.Length; j++)
                            {
                                if (j < (i + 1) * 1000 && j >= i * 1000)
                                {
                                    if (!string.IsNullOrEmpty(str))
                                        str += ",";
                                    str += strArray[j];
                                    flag = true;
                                }
                            }
                            if (flag)
                            {
                                whereQueryWord = condition + " in (" + str + ")";
                                where = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(where, whereQueryWord);
                            }
                        }
                    }
                    else
                    {
                        whereQueryWord = condition + " in (" + ids + ")";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                    }
                }
                else
                {

                    whereQueryWord = condition + " = " + ids.Trim().ToString();
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
            }

            return where;
        }
        #endregion

        public string getTaskId(int number)
        {
            string autoTask = AscmCommonHelperService.GetInstance().GetConfigTaskWords(0);

            int a = number / 1000;
            int b = (number - a * 1000) / 100;
            int c = (number - a * 1000 - b * 100) / 10;
            int d = (number - a * 1000 - b * 100 - c * 10);

            return autoTask + a.ToString() + b.ToString() + c.ToString() + d.ToString();
        }

        public void SetTotalSum(List<AscmWipRequirementOperations> listAscmWipRequirementOperations, List<AscmWipDiscreteJobs> listAscmWipDiscreteJobs)
        {
            foreach (AscmWipDiscreteJobs ascmWipDiscreteJobs in listAscmWipDiscreteJobs)
            {
                foreach (AscmWipRequirementOperations ascmWipRequirementOperations in listAscmWipRequirementOperations)
                {
                    if (ascmWipRequirementOperations.wipEntityId.ToString() == ascmWipDiscreteJobs.wipEntityId.ToString())
                    {
                        ascmWipDiscreteJobs.totalRequiredQuantity = Convert.ToDecimal(ascmWipRequirementOperations.requiredQuantity);
                        ascmWipDiscreteJobs.totalGetMaterialQuantity = Convert.ToDecimal(ascmWipRequirementOperations.getMaterialQuantity);
                        ascmWipDiscreteJobs.totalPreparationQuantity = Convert.ToDecimal(ascmWipRequirementOperations.wmsPreparationQuantity);
                    }
                }
            }
        }
    }
}
