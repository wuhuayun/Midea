using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal.GetMaterialManage.Entities;

namespace MideaAscm.Dal.FromErp.Entities
{
    ///<summary>计划组，车间 </summary>
    public class AscmWipScheduleGroups
    {
        ///<summary>id</summary>
        public virtual int scheduleGroupId { get; set; }
        ///<summary>库存组织ID</summary>
        public virtual int organizationId { get; set; }
        ///<summary>创建人</summary>
        public virtual string createUser { get; set; }
        ///<summary>创建时间</summary>
        public virtual string createTime { get; set; }
        ///<summary>最后更新人</summary>
        public virtual string modifyUser { get; set; }
        ///<summary>最后更新时间</summary>
        public virtual string modifyTime { get; set; }
        ///<summary>名称</summary>
        public virtual string scheduleGroupName { get; set; }

        public AscmWipScheduleGroups GetOwner()
        {
            return (AscmWipScheduleGroups)this.MemberwiseClone();
        }

        //辅助信息
        public class TypeDefine
        {
            /// <summary>总装</summary>
            public const string GA = "GA";
            /// <summary>电装</summary>
            public const string QC = "QC";

            public static string DisplayText(string typeDefine)
            {
                switch (typeDefine)
                {
                    case GA: return "总装";
                    case QC: return "电装";
                    default: return typeDefine;
                }
            }
            public static List<string> GetList()
            {
                List<string> list = new List<string>();
                list.Add(GA);
                list.Add(QC);
                return list;
            }

            public static List<string> GetWipScheduleGroupNames(string typeDefine)
            {
                List<string> listScheduleGroupNames = new List<string>();
                if (typeDefine == GA)
                {
                    listScheduleGroupNames.Add("S多总一");
                    listScheduleGroupNames.Add("S多总二");
                    listScheduleGroupNames.Add("S多总三");
                    listScheduleGroupNames.Add("S热总一");
                    listScheduleGroupNames.Add("S水箱总");
                }
                else if (typeDefine == QC)
                {
                    listScheduleGroupNames.Add("S电装一");
                    listScheduleGroupNames.Add("S电装二");
                    listScheduleGroupNames.Add("S电装三");
                    listScheduleGroupNames.Add("S电装四");
                    listScheduleGroupNames.Add("S电装五");
                    listScheduleGroupNames.Add("S电集一");
                }
                return listScheduleGroupNames;
            }
            public static List<string> GetMtlPrepareTypes(string typeDefine)
            {
                List<string> listMtlPrepareType = new List<string>();
                if (typeDefine == GA || typeDefine == QC)
                {
                    List<string> listMtlCategoryStatus = MtlCategoryStatusDefine.GetList();
                    if (listMtlCategoryStatus != null)
                    {
                        foreach (string mtlCategoryStatus in listMtlCategoryStatus)
                        {
                            listMtlPrepareType.Add(typeDefine + "_" + mtlCategoryStatus);
                        }
                    }
                }
                return listMtlPrepareType;
            }
            public static string GetMtlPrepareTypeText(string mtlPrepareType)
            {
                string mtlPrepareTypeText = string.Empty;
                if (!string.IsNullOrEmpty(mtlPrepareType))
                {
                    int index = mtlPrepareType.IndexOf('_');
                    if (index != -1)
                    {
                        mtlPrepareTypeText += DisplayText(mtlPrepareType.Substring(0, index));
                        mtlPrepareTypeText += "_";
                        mtlPrepareTypeText += MtlCategoryStatusDefine.DisplayText(mtlPrepareType.Substring(index + 1));
                    }
                }
                return mtlPrepareTypeText;
            }
            public static KeyValuePair<string, string> GetMtlPrepareType(string mtlPrepareType)
            {
                KeyValuePair<string, string> kvp = new KeyValuePair<string, string>();
                if (!string.IsNullOrEmpty(mtlPrepareType))
                { 
                    int index = mtlPrepareType.IndexOf('_');
                    if (index != -1)
                    {
                        string typeDefine = mtlPrepareType.Substring(0, index);
                        string mtlCategoryStatus = mtlPrepareType.Substring(index + 1);
                        if (GetList().Contains(typeDefine) && MtlCategoryStatusDefine.GetList().Contains(mtlCategoryStatus))
                        {
                            kvp = new KeyValuePair<string, string>(typeDefine, mtlCategoryStatus);
                        }
                    }
                }
                return kvp;
            }
        }      
    }
}
