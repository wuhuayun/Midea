using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YnBaseDal;
using NHibernate;
using Oracle.DataAccess.Client;
using System.Data;
using MideaAscm.Dal;
using MideaAscm.Dal.Warehouse.Entities;
using MideaAscm.Dal.SupplierPreparation.Entities;

namespace MideaAscm.Services.MesInterface
{
    public class AscmMesService
    {
        /// <summary>中央空调事业部库存组织ID</summary>
        private static readonly int orgId = 775;
		private static Dictionary<string, string> AscmKeys = new Dictionary<string, string>();
		private static object objLockKey = new object();
        private static AscmMesService service;
        public static AscmMesService GetInstance()
        {
            if (service == null)
                service = new AscmMesService();
            return service;
        }

        /// <summary>MES闭环单号类型</summary>
        public struct BillTypeDefine
        {
            /// <summary>MES到货接收手工单单号</summary>
            public const string sg = "SG";
            /// <summary>MES领料单单号</summary>
            public const string ll = "LL";
            /// <summary>MES退料单单号</summary>
            public const string tl = "TL";
            /// <summary>MES供应商退货单单号</summary>
            public const string th = "TH";
            /// <summary>MES杂项事务单单号</summary>
            public const string zx = "ZX";
            /// <summary>MES子库转移单单号</summary>
            public const string zy = "ZY";
        }

        #region 获取MES闭环单号
        /// <summary>
        /// 获取MES到货接收手工单单号
        /// </summary>
        /// <returns>返回MES到货接收手工单单号</returns>
        public string GetMesManualBillNo()
        {
            return GetMesBillNo(orgId, "SG");
        }
        /// <summary>
        /// 获取MES领料单单号
        /// </summary>
        /// <returns>返回MES领料单单号</returns>
        public string GetMesRequisitionBillNo()
        {
            return GetMesBillNo(orgId, "LL");
        }
        /// <summary>
        /// 获取MES退料单单号
        /// </summary>
        /// <returns>返回MES退料单单号</returns>
        public string GetMesReturnedBillNo()
        {
            return GetMesBillNo(orgId, "TL");
        }
        /// <summary>
        /// 获取MES供应商退货单单号
        /// </summary>
        /// <returns>返回MES供应商退货单单号</returns>
        public string GetMesRejectBillNo()
        {
            return GetMesBillNo(orgId, "TH");
        }
        /// <summary>
        /// 获取MES杂项事务单单号
        /// </summary>
        /// <returns>返回MES杂项事务单单号</returns>
        public string GetMesMiscBillNo()
        {
            return GetMesBillNo(orgId, "ZX");
        }
        /// <summary>
        /// 获取MES子库转移单单号
        /// </summary>
        /// <returns>返回MES子库转移单单号</returns>
        public string GetMesTransferBillNo()
        {
            return GetMesBillNo(orgId, "ZY");
        }
        /// <summary>
        /// 获取MES闭环单号
        /// </summary>
        /// <param name="organizationId">库存组织ID</param>
        /// <param name="billType">单据类型</param>
        /// <returns>返回MES闭环单号</returns>
        public string GetMesBillNo(int organizationId, string billType)
        {
            string billNo = "";
            try
            {
                OracleParameter[] commandParameters = new OracleParameter[] {
                    //库存组织
                    new OracleParameter {
                        ParameterName = "i_org_id",
                        OracleDbType = OracleDbType.Int32,
                        Value = organizationId,
                        Direction = ParameterDirection.Input
                    },
                    //MES闭环单据类型
                    new OracleParameter {
                        ParameterName = "i_type",
                        OracleDbType = OracleDbType.Varchar2,
                        Size = 80,
                        Value = billType,
                        Direction = ParameterDirection.Input
                    },
                    //MES生成单据号
                    new OracleParameter {
                        ParameterName = "o_returnno",
                        OracleDbType = OracleDbType.Varchar2,
                        Size = 80,
                        Direction = ParameterDirection.Output
                    }
                };
                //ExecuteOraProcedure("cux_mes_ascm_interface_pkg.ascm_get_mes_billno", ref commandParameters);

                ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
                OracleCommand command = (OracleCommand)session.Connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "cux_mes_ascm_interface_pkg.ascm_get_mes_billno";
                Array.ForEach<OracleParameter>(commandParameters, P => command.Parameters.Add(P));
                command.ExecuteNonQuery();

                billNo = commandParameters[commandParameters.Length - 1].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return billNo;
        }
        #endregion

		#region 到货接收系统单
		public void DoSysReceive(List<AscmDeliBatOrderLink> listDeliBatOrderLink, string userId, AscmMesInteractiveLog ascmMesInteractiveLog)
        {
            try
            {
				string pKey = "";
                AscmDeliBatOrderLink ascmDeliBatOrderLink = listDeliBatOrderLink.First();
				OracleParameter[] sysReceiveParams = GetSysReceiveParams(listDeliBatOrderLink, ascmDeliBatOrderLink, userId, out pKey);
				lock (objLockKey) 
				{
					if (AscmKeys.ContainsKey(pKey)) 
					{
						return;
					}

					AscmKeys[pKey] = "";
				}

                //不能放在事务中执行，否则调用的事务处理无法释放资源
                //ExecuteOraProcedure("cux_mes_ascm_interface_pkg.ascm_do_sys_receive", ref sysReceiveParams);
                ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
                OracleCommand command = (OracleCommand)session.Connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "cux_mes_ascm_interface_pkg.ascm_do_sys_receive";
                Array.ForEach<OracleParameter>(sysReceiveParams, P => command.Parameters.Add(P));
                command.ExecuteNonQuery();

                ascmMesInteractiveLog.returnCode = sysReceiveParams[sysReceiveParams.Length - 2].Value.ToString();
                ascmMesInteractiveLog.returnMessage = sysReceiveParams[sysReceiveParams.Length - 1].Value.ToString();
				if (AscmKeys.ContainsKey(pKey))
				{
					AscmKeys.Remove(pKey);
				}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public OracleParameter[] GetSysReceiveParams(List<AscmDeliBatOrderLink> listDeliBatOrderLink, AscmDeliBatOrderLink ascmDeliBatOrderLink, string userId,out string paramsKey)
        {
            //注意MES存储过程中要求字符串数组长度一致，并且一一对应
            string[] headIds = listDeliBatOrderLink.Select(P => P.mainId.ToString()).ToArray();
            string[] lineIds = listDeliBatOrderLink.Select(P => P.id.ToString()).ToArray();
            string[] itemlocs = new string[lineIds.Length]; 
            string[] recqtys = listDeliBatOrderLink.Select(P => P.receivedQuantity.ToString()).ToArray();
            string[] mrecqtys = listDeliBatOrderLink.Select(P => P.deliveryQuantity.ToString()).ToArray();
            string[] priorities = new string[lineIds.Length];
            string[] istries = new string[lineIds.Length];
            string[] tryitemdesc = new string[lineIds.Length];
            //为数组元素指定默认值
            for (int i = 0; i < lineIds.Length; i++)
            {
                itemlocs[i] = "";
                priorities[i] = "IQC_Normal";
                istries[i] = "N";
                tryitemdesc[i] = "";
            }

			StringBuilder strKey = new StringBuilder();
			foreach (var item in headIds)
			{
				strKey.Append(item + ",");
			}
			foreach (var item in lineIds)
			{
				strKey.Append(item + ",");
			}
			paramsKey = strKey.ToString();

            return new OracleParameter[] {
                //库存组织
                new OracleParameter{
                    ParameterName = "i_org_id",
                    OracleDbType = OracleDbType.Int32,
                    Value = orgId,
                    Direction = ParameterDirection.Input
                },
                //送货批条码号
                new OracleParameter{
                    ParameterName = "i_batch_bar_code",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmDeliBatOrderLink.batchBarCode,
                    Direction = ParameterDirection.Input
                },
                //自动返还，默认值FALSE
                new OracleParameter{
                    ParameterName = "i_autoreturnitem",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = "FALSE",
                    Direction = ParameterDirection.Input
                },
                //收货仓库编码
                new OracleParameter{
                    ParameterName = "i_inv_code",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmDeliBatOrderLink.warehouseId,
                    Direction = ParameterDirection.Input
                },
                //是否外租仓，默认值N
                new OracleParameter{
                    ParameterName = "i_isrentedinv",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = "N",
                    Direction = ParameterDirection.Input
                },
                //是否外检合格，默认值N
                new OracleParameter{
                    ParameterName = "i_ischeckedokinvendor",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = "N",
                    Direction = ParameterDirection.Input
                },
                //送货单备注
                new OracleParameter{
                    ParameterName = "i_memo",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 240,
                    Value = "",
                    Direction = ParameterDirection.Input
                },
                //送货单头ID（字符串数组）
                new OracleParameter{
                    ParameterName = "i_headid",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = headIds,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //送货单行ID（字符串数组）
                new OracleParameter{
                    ParameterName = "i_lineid",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = lineIds,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //位置，支持多位置存储，多位置存储时用英文逗号隔开（字符串数组）
                new OracleParameter{
                    ParameterName = "i_itemloc",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = itemlocs,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //实收数量（字符串数组），不可为空
                new OracleParameter{
                    ParameterName = "i_recqty",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = recqtys,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //送货数量，根据多位置一一对应，当非多位置存储时默认为0（字符串数组）
                new OracleParameter{
                    ParameterName = "i_mrecqty",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = mrecqtys,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //紧急程度，默认值为：IQC_Normal（字符串数组）
                new OracleParameter{
                    ParameterName = "i_priority",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = priorities,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //试流，默认值为：N（字符串数组）
                new OracleParameter{
                    ParameterName = "i_istry",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = istries,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //试流物料，默认值为空（字符串数组）
                new OracleParameter{
                    ParameterName = "i_tryitemdesc",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = tryitemdesc,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //用户名称
                new OracleParameter{
                    ParameterName = "i_user_code",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = userId,
                    Direction = ParameterDirection.Input
                },
                //返回参(值为0时表示成功)
                new OracleParameter{
                    ParameterName = "o_ret_code",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 4000,
                    Direction = ParameterDirection.Output
                },
                //返回参(错误信息)
                new OracleParameter{
                    ParameterName = "o_ret_msg",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 4000,
                    Direction = ParameterDirection.Output
                }
            };
        }
        #endregion

        #region 手工单接收
        public void DoManualReceive(AscmWmsIncManAccMain ascmWmsIncManAccMain, List<AscmWmsIncManAccDetail> listAscmWmsIncManAccDetail, string userId, AscmMesInteractiveLog ascmMesInteractiveLog)
        {
            try
            {
				string pKey = "";
				OracleParameter[] manualReceiveParams = GetManualReceiveParams(ascmWmsIncManAccMain, listAscmWmsIncManAccDetail, userId, out pKey);
				lock (objLockKey)
				{
					if (AscmKeys.ContainsKey(pKey))
					{
						return;
					}

					AscmKeys[pKey] = "";
				}

                ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
                OracleCommand command = (OracleCommand)session.Connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "cux_mes_ascm_interface_pkg.ascm_do_manual_receive";
                Array.ForEach<OracleParameter>(manualReceiveParams, P => command.Parameters.Add(P));
                command.ExecuteNonQuery();

                ascmMesInteractiveLog.returnCode = manualReceiveParams[manualReceiveParams.Length - 2].Value.ToString();
                ascmMesInteractiveLog.returnMessage = manualReceiveParams[manualReceiveParams.Length - 1].Value.ToString();

				if (ascmWmsIncManAccMain != null) 
				{
					ascmWmsIncManAccMain.returnCode = ascmMesInteractiveLog.returnCode;
					ascmWmsIncManAccMain.returnMessage = ascmMesInteractiveLog.returnMessage;
					ascmWmsIncManAccMain.uploadTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				}

				if (AscmKeys.ContainsKey(pKey))
				{
					AscmKeys.Remove(pKey);
				}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
		public OracleParameter[] GetManualReceiveParams(AscmWmsIncManAccMain ascmWmsIncManAccMain, List<AscmWmsIncManAccDetail> listAscmWmsIncManAccDetail, string userId, out string paramsKey)
        {
            string[] items = listAscmWmsIncManAccDetail.Select(P => P.materialId.ToString()).ToArray();
            string[] itemlocs = new string[items.Length];
            string[] recqtys = listAscmWmsIncManAccDetail.Select(P => P.receivedQuantity.ToString()).ToArray();
            string[] mobatchnos = new string[items.Length];
            string[] priorities = new string[items.Length];
            string[] istries = new string[items.Length];
            string[] tryitemdesc = new string[items.Length];
            //为数组元素指定默认值
            for (int i = 0; i < items.Length; i++)
            {
                itemlocs[i] = "";
                mobatchnos[i] = "";
                priorities[i] = "IQC_Normal";
                istries[i] = "N";
                tryitemdesc[i] = "";
            }

			StringBuilder strKey = new StringBuilder();
			foreach (var item in items)
			{
				strKey.Append(item + ",");
			}
			paramsKey = strKey.ToString();

            return new OracleParameter[] {
                //库存组织
                new OracleParameter{
                    ParameterName = "i_org_id",
                    OracleDbType = OracleDbType.Int32,
                    Value = orgId,
                    Direction = ParameterDirection.Input
                },
                //闭环单号
                new OracleParameter{
                    ParameterName = "i_bar_code",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmWmsIncManAccMain.docNumber,
                    Direction = ParameterDirection.Input
                },
                //是否外租仓，默认值N
                new OracleParameter{
                    ParameterName = "i_isrentedinv",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = "N",
                    Direction = ParameterDirection.Input
                },
                //收货仓库编码
                new OracleParameter{
                    ParameterName = "i_inv_code",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmWmsIncManAccMain.warehouseId,
                    Direction = ParameterDirection.Input
                },
                //作业号
                new OracleParameter{
                    ParameterName = "i_mo_name",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = "",
                    Direction = ParameterDirection.Input
                },
                //供应商ID
                new OracleParameter{
                    ParameterName = "i_supplierid",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmWmsIncManAccMain.supplierId,
                    Direction = ParameterDirection.Input
                },
                //供应商地址ID
                new OracleParameter{
                    ParameterName = "i_suppliersetid",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmWmsIncManAccMain.supplierAddressId,
                    Direction = ParameterDirection.Input
                },
                //车牌号码
                new OracleParameter{
                    ParameterName = "i_trucknum",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmWmsIncManAccMain.supperPlateNumber,
                    Direction = ParameterDirection.Input
                },
                //出货子库
                new OracleParameter{
                    ParameterName = "i_from_inv_code",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmWmsIncManAccMain.supperWarehouse,
                    Direction = ParameterDirection.Input
                },
                //联系号码
                new OracleParameter{
                    ParameterName = "i_tel_phone",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmWmsIncManAccMain.supperTelephone,
                    Direction = ParameterDirection.Input
                },
                //备注
                new OracleParameter{
                    ParameterName = "i_memo",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 240,
                    Value = ascmWmsIncManAccMain.memo,
                    Direction = ParameterDirection.Input
                },
                //物料编码（字符串数组）
                new OracleParameter{
                    ParameterName = "i_item",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = items,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //位置，支持多位置存储，多位置存储时用英文逗号隔开（字符串数组）
                new OracleParameter{
                    ParameterName = "i_itemloc",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = itemlocs,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //实收数量（字符串数组），不可为空
                new OracleParameter{
                    ParameterName = "i_recqty",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = recqtys,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //生产批次（字符串数组）
                new OracleParameter{
                    ParameterName = "i_mobatchno",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = mobatchnos,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //紧急程度，默认值为：IQC_Normal（字符串数组）
                new OracleParameter{
                    ParameterName = "i_priority",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = priorities,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //试流，默认值为：N（字符串数组）
                new OracleParameter{
                    ParameterName = "i_istry",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = istries,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //试流物料，默认值为空（字符串数组）
                new OracleParameter{
                    ParameterName = "i_tryitemdesc",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = tryitemdesc,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //用户名称
                new OracleParameter{
                    ParameterName = "i_user_code",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = userId,
                    Direction = ParameterDirection.Input
                },
                //返回参(值为0时表示成功)
                new OracleParameter{
                    ParameterName = "o_ret_code",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 4000,
                    Direction = ParameterDirection.Output
                },
                //返回参(错误信息)
                new OracleParameter{
                    ParameterName = "o_ret_msg",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 4000,
                    Direction = ParameterDirection.Output
                }
            };
        }
        #endregion

        #region 供应商退货
        public void DoGoodsReject(AscmWmsBackInvoiceMain ascmWmsBackInvoiceMain, List<AscmWmsBackInvoiceLink> listWmsBackInvoiceLink, string userId, AscmMesInteractiveLog ascmMesInteractiveLog)
        {
            try
            {
                OracleParameter[] goodsRejectParams = GetGoodsRejectParams(ascmWmsBackInvoiceMain, listWmsBackInvoiceLink, userId);
                ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
                OracleCommand command = (OracleCommand)session.Connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "cux_mes_ascm_interface_pkg.ascm_do_goods_reject";
                Array.ForEach<OracleParameter>(goodsRejectParams, P => command.Parameters.Add(P));
                command.ExecuteNonQuery();

                ascmMesInteractiveLog.returnCode = goodsRejectParams[goodsRejectParams.Length - 2].Value.ToString();
                ascmMesInteractiveLog.returnMessage = goodsRejectParams[goodsRejectParams.Length - 1].Value.ToString();

				if (ascmWmsBackInvoiceMain != null)
				{
					ascmWmsBackInvoiceMain.returnCode = ascmMesInteractiveLog.returnCode;
					ascmWmsBackInvoiceMain.returnMessage = ascmMesInteractiveLog.returnMessage;
					ascmWmsBackInvoiceMain.uploadTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public OracleParameter[] GetGoodsRejectParams(AscmWmsBackInvoiceMain ascmWmsBackInvoiceMain, List<AscmWmsBackInvoiceLink> listWmsBackInvoiceLink, string userId)
        {
            //注意MES存储过程中要求字符串数组长度一致，并且一一对应
            string[] batchDocNumbers = listWmsBackInvoiceLink.Select(P => P.barCode).ToArray();
            string[] labelIds = new string[batchDocNumbers.Length];
            string[] materialIds = listWmsBackInvoiceLink.Select(P => P.materialId.ToString()).ToArray();
            string[] warehouseIds = listWmsBackInvoiceLink.Select(P => P.warehouseId).ToArray();
            string[] locIds = new string[batchDocNumbers.Length];
            string[] quantities = listWmsBackInvoiceLink.Select(P => P.rejectQuantity.ToString()).ToArray();
            string[] accountStatuss = new string[batchDocNumbers.Length];
            //为数组元素指定默认值
            for (int i = 0; i < batchDocNumbers.Length; i++)
            {
                labelIds[i] = "";
                locIds[i] = "";
                accountStatuss[i] = ascmWmsBackInvoiceMain.accountStatus;
            }

            return new OracleParameter[] {
                //库存组织
                new OracleParameter{
                    ParameterName = "i_org_id",
                    OracleDbType = OracleDbType.Int32,
                    Value = orgId,
                    Direction = ParameterDirection.Input
                },
                //MES闭环退货单号
                new OracleParameter{
                    ParameterName = "i_rejectno",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmWmsBackInvoiceMain.docNumber,
                    Direction = ParameterDirection.Input
                },
                //手工退货单号
                new OracleParameter{
                    ParameterName = "i_manuno",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmWmsBackInvoiceMain.manualDocNumber,
                    Direction = ParameterDirection.Input
                },
                //系统收据号
                new OracleParameter{
                    ParameterName = "i_systemno",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = null,
                    Direction = ParameterDirection.Input
                },
                //制单人
                new OracleParameter{
                    ParameterName = "i_maker",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmWmsBackInvoiceMain.createUser,
                    Direction = ParameterDirection.Input
                },
                //退货原因，ERP事务原因id
                new OracleParameter{
                    ParameterName = "i_resion_id",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmWmsBackInvoiceMain.reasonId,
                    Direction = ParameterDirection.Input
                },
                //送货单备注
                new OracleParameter{
                    ParameterName = "i_memo",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 240,
                    Value = ascmWmsBackInvoiceMain.memo,
                    Direction = ParameterDirection.Input
                },
                //标签号（字符串数组）
                new OracleParameter{
                    ParameterName = "i_labelid",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = labelIds,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //送货批单条码号（字符串数组）
                new OracleParameter{
                    ParameterName = "i_deliveryno",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = batchDocNumbers,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //物料编码（字符串数组）
                new OracleParameter{
                    ParameterName = "i_itemid",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = materialIds,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //仓库（字符串数组），不可为空
                new OracleParameter{
                    ParameterName = "i_invcode",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = warehouseIds,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //位置（字符串数组）
                new OracleParameter{
                    ParameterName = "i_locid",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = locIds,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //退货数量（字符串数组）
                new OracleParameter{
                    ParameterName = "i_quantity",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = quantities,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //账务状态（字符串数组）
                new OracleParameter{
                    ParameterName = "i_accountstatus",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = accountStatuss,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //用户名称
                new OracleParameter{
                    ParameterName = "i_user_code",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = userId,
                    Direction = ParameterDirection.Input
                },
                //返回参(值为0时表示成功)
                new OracleParameter{
                    ParameterName = "o_ret_code",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 4000,
                    Direction = ParameterDirection.Output
                },
                //返回参(错误信息)
                new OracleParameter{
                    ParameterName = "o_ret_msg",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 4000,
                    Direction = ParameterDirection.Output
                }
            };
        }
        #endregion

        #region 子库存转移
        public void DoStockTrans(AscmWmsStockTransMain ascmWmsStockTransMain, List<AscmWmsStockTransDetail> listWmsStockTransDetail, string userId, AscmMesInteractiveLog ascmMesInteractiveLog)
        {
            try
            {
                OracleParameter[] stockTransParams = GetStockTransParams(ascmWmsStockTransMain, listWmsStockTransDetail, userId);
                ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
                OracleCommand command = (OracleCommand)session.Connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "cux_mes_ascm_interface_pkg.ascm_do_stock_trans";
                Array.ForEach<OracleParameter>(stockTransParams, P => command.Parameters.Add(P));
                command.ExecuteNonQuery();

                ascmMesInteractiveLog.returnCode = stockTransParams[stockTransParams.Length - 2].Value.ToString();
                ascmMesInteractiveLog.returnMessage = stockTransParams[stockTransParams.Length - 1].Value.ToString();

				if (ascmWmsStockTransMain != null)
				{
					ascmWmsStockTransMain.returnCode = ascmMesInteractiveLog.returnCode;
					ascmWmsStockTransMain.returnMessage = ascmMesInteractiveLog.returnMessage;
					ascmWmsStockTransMain.uploadTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public OracleParameter[] GetStockTransParams(AscmWmsStockTransMain ascmWmsStockTransMain, List<AscmWmsStockTransDetail> listWmsStockTransDetail, string userId)
        {
            //备注
            string memo = "";
            if (!string.IsNullOrEmpty(ascmWmsStockTransMain.memo))
                memo = ascmWmsStockTransMain.memo;
            //注意MES存储过程中要求字符串数组长度一致，并且一一对应
            string[] itemIds = listWmsStockTransDetail.Select(P => P.materialId.ToString()).ToArray();
            string[] labelIds = new string[itemIds.Length];
            string[] fromInvCodes = new string[itemIds.Length];
            string[] fromLocIds = new string[itemIds.Length];
            string[] toInvCodes = new string[itemIds.Length];
            string[] toLocIds = new string[itemIds.Length];
            string[] quantities = listWmsStockTransDetail.Select(P => P.quantity.ToString()).ToArray();
            string[] descriptions = new string[itemIds.Length];
            string[] references = listWmsStockTransDetail.Select(P => P.reference.ToString()).ToArray();
            string[] moBatchNos = new string[itemIds.Length];
            string[] vendorIds = new string[itemIds.Length];
            string[] accountStatuss = new string[itemIds.Length];
            string[] deliveryNos = new string[itemIds.Length];
            //为数组元素指定默认值
            for (int i = 0; i < itemIds.Length; i++)
            {
                labelIds[i] = "";
                fromInvCodes[i] = ascmWmsStockTransMain.fromWarehouseId;
                fromLocIds[i] = "";
                toInvCodes[i] = ascmWmsStockTransMain.toWarehouseId;
                toLocIds[i] = "";
                descriptions[i] = "";
                moBatchNos[i] = "";
                vendorIds[i] = "";
                accountStatuss[i] = "lm_deliveried";
                deliveryNos[i] = "";
            }

            return new OracleParameter[] {
                //库存组织
                new OracleParameter{
                    ParameterName = "i_org_id",
                    OracleDbType = OracleDbType.Int32,
                    Value = orgId,
                    Direction = ParameterDirection.Input
                },
                //MES闭环单号
                new OracleParameter{
                    ParameterName = "i_invtrano",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmWmsStockTransMain.docNumber,
                    Direction = ParameterDirection.Input
                },
                //手工单号
                new OracleParameter{
                    ParameterName = "i_manuno",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmWmsStockTransMain.manualDocNumber,
                    Direction = ParameterDirection.Input
                },
                //MES事务类型
                new OracleParameter{
                    ParameterName = "i_cbxtype",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmWmsStockTransMain.transType,
                    Direction = ParameterDirection.Input
                },
                //接收人
                new OracleParameter{
                    ParameterName = "i_recemp",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmWmsStockTransMain.createUser,
                    Direction = ParameterDirection.Input
                },
                //自动报检
                new OracleParameter{
                    ParameterName = "i_autosendcheck",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = "FALSE",
                    Direction = ParameterDirection.Input
                },
                //退货原因，ERP事务原因id
                new OracleParameter{
                    ParameterName = "i_resion_id",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmWmsStockTransMain.reasonId,
                    Direction = ParameterDirection.Input
                },
                //标签号（字符串数组）
                new OracleParameter{
                    ParameterName = "i_labelid",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = labelIds,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //物料编码（字符串数组）
                new OracleParameter{
                    ParameterName = "i_itemid",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = itemIds,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //来源仓库（字符串数组），不可为空
                new OracleParameter{
                    ParameterName = "i_frominvcode",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = fromInvCodes,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //来源货位（字符串数组）
                new OracleParameter{
                    ParameterName = "i_fromlocid",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = fromLocIds,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //目标仓库（字符串数组），不可为空
                new OracleParameter{
                    ParameterName = "i_toinvcode",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = toInvCodes,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //目标货位（字符串数组）
                new OracleParameter{
                    ParameterName = "i_tolocid",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = toLocIds,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //数量（字符串数组）
                new OracleParameter{
                    ParameterName = "i_quantity",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = quantities,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //备注（字符串数组）
                new OracleParameter{
                    ParameterName = "i_description",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = descriptions,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //参考（字符串数组）
                new OracleParameter{
                    ParameterName = "i_reference",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = references,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //生产批次（字符串数组）
                new OracleParameter{
                    ParameterName = "i_mobatchno",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = moBatchNos,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //供应商ID（字符串数组）
                new OracleParameter{
                    ParameterName = "i_vendorid",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = vendorIds,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //账务状态（字符串数组）
                new OracleParameter{
                    ParameterName = "i_accountstatus",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = accountStatuss,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //送货单号（字符串数组）
                new OracleParameter{
                    ParameterName = "i_deliveryno",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = deliveryNos,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //用户名称
                new OracleParameter{
                    ParameterName = "i_user_code",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = userId,
                    Direction = ParameterDirection.Input
                },
                //返回参(值为0时表示成功)
                new OracleParameter{
                    ParameterName = "o_ret_code",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 4000,
                    Direction = ParameterDirection.Output
                },
                //返回参(错误信息)
                new OracleParameter{
                    ParameterName = "o_ret_msg",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 4000,
                    Direction = ParameterDirection.Output
                }
            };
        }
        #endregion

        #region 作业领料
        public void DoMtlRequisition(AscmWmsMtlRequisitionMain ascmWmsMtlRequisitionMain, List<AscmWmsMtlRequisitionDetail> listWmsMtlRequisitionDetail, string userId, AscmMesInteractiveLog ascmMesInteractiveLog)
        {
            try
            {
                OracleParameter[] mtlRequisitionParams = GetMaterialRequisitionParams(ascmWmsMtlRequisitionMain, listWmsMtlRequisitionDetail, userId);
                ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
                OracleCommand command = (OracleCommand)session.Connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "cux_mes_ascm_interface_pkg.ascm_do_mtl_requisition";
                Array.ForEach<OracleParameter>(mtlRequisitionParams, P => command.Parameters.Add(P));
                command.ExecuteNonQuery();

                ascmMesInteractiveLog.returnCode = mtlRequisitionParams[mtlRequisitionParams.Length - 2].Value.ToString();
                ascmMesInteractiveLog.returnMessage = mtlRequisitionParams[mtlRequisitionParams.Length - 1].Value.ToString();

				if (ascmWmsMtlRequisitionMain != null)
				{
					ascmWmsMtlRequisitionMain.returnCode = ascmMesInteractiveLog.returnCode;
					ascmWmsMtlRequisitionMain.returnMessage = ascmMesInteractiveLog.returnMessage;
					ascmWmsMtlRequisitionMain.uploadTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public OracleParameter[] GetMaterialRequisitionParams(AscmWmsMtlRequisitionMain ascmWmsMtlRequisitionMain, List<AscmWmsMtlRequisitionDetail> listWmsMtlRequisitionDetail, string userId)
        {
            //注意MES存储过程中要求字符串数组长度一致，并且一一对应
            string[] itemIds = listWmsMtlRequisitionDetail.Select(P => P.materialId.ToString()).ToArray();
            string[] deliveryBarCodes = new string[itemIds.Length];
            string[] accStatuss = new string[itemIds.Length];
            string[] remarks = new string[itemIds.Length];
            string[] billNos = listWmsMtlRequisitionDetail.Select(P => P.wipEntityName).ToArray();
            string[] itemGetQtys = listWmsMtlRequisitionDetail.Select(P => P.quantity.ToString()).ToArray();
            string[] invCodes = listWmsMtlRequisitionDetail.Select(P=>P.warehouseId).ToArray();
            string[] locIds = new string[itemIds.Length];
            //为数组元素指定默认值
            for (int i = 0; i < itemIds.Length; i++)
            {
                deliveryBarCodes[i] = "";
                accStatuss[i] = "lm_deliveried";
                remarks[i] = "";
                locIds[i] = "";
            }

            return new OracleParameter[] {
                //库存组织
                new OracleParameter{
                    ParameterName = "i_org_id",
                    OracleDbType = OracleDbType.Int32,
                    Value = orgId,
                    Direction = ParameterDirection.Input
                },
                //MES闭环单号
                new OracleParameter{
                    ParameterName = "i_mesno",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmWmsMtlRequisitionMain.docNumber,
                    Direction = ParameterDirection.Input
                },
                //领料批号
                new OracleParameter{
                    ParameterName = "i_batchbillno",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = "",
                    Direction = ParameterDirection.Input
                },
                //单据类型
                new OracleParameter{
                    ParameterName = "i_gettype",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = "mocode",
                    Direction = ParameterDirection.Input
                },
                //手工单号(作业领料必须输入)
                new OracleParameter{
                    ParameterName = "i_sgcode",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmWmsMtlRequisitionMain.manualDocNumber,
                    Direction = ParameterDirection.Input
                },
                //领大单
                new OracleParameter{
                    ParameterName = "i_isbig",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = "N",
                    Direction = ParameterDirection.Input
                },
                //自动配送
                new OracleParameter{
                    ParameterName = "i_isauto",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = "N",
                    Direction = ParameterDirection.Input
                },
                //是否自动匹配
                new OracleParameter{
                    ParameterName = "i_automatch",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = "N",
                    Direction = ParameterDirection.Input
                },
                //接收位置
                new OracleParameter{
                    ParameterName = "i_loc_area",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = "",
                    Direction = ParameterDirection.Input
                },
                //接收区域
                new OracleParameter{
                    ParameterName = "i_to_area",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = "OnWip",
                    Direction = ParameterDirection.Input
                },
                //制单人
                new OracleParameter{
                    ParameterName = "i_person",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = "",
                    Direction = ParameterDirection.Input
                },
                //领用部门
                new OracleParameter{
                    ParameterName = "i_dept",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = "",
                    Direction = ParameterDirection.Input
                },
                //交接人
                new OracleParameter{
                    ParameterName = "i_recman",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = "",
                    Direction = ParameterDirection.Input
                },
                //送货单号（字符串数组）
                new OracleParameter{
                    ParameterName = "i_deliverybarcode",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = deliveryBarCodes,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //帐务状态（字符串数组）
                new OracleParameter{
                    ParameterName = "i_accstatus",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = accStatuss,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //备注（字符串数组）
                new OracleParameter{
                    ParameterName = "i_remark",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = remarks,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //作业号（字符串数组）
                new OracleParameter{
                    ParameterName = "i_billno",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = billNos,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //物料编码（字符串数组）
                new OracleParameter{
                    ParameterName = "i_itemid",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = itemIds,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //实际数量（字符串数组）
                new OracleParameter{
                    ParameterName = "i_itemgetqty",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = itemGetQtys,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //仓库（字符串数组）
                new OracleParameter{
                    ParameterName = "i_invcode",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = invCodes,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //位置（字符串数组）
                new OracleParameter{
                    ParameterName = "i_locid",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = locIds,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //用户名称
                new OracleParameter{
                    ParameterName = "i_user_code",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = userId,
                    Direction = ParameterDirection.Input
                },
                //返回参(值为0时表示成功)
                new OracleParameter{
                    ParameterName = "o_ret_code",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 4000,
                    Direction = ParameterDirection.Output
                },
                //返回参(错误信息)
                new OracleParameter{
                    ParameterName = "o_ret_msg",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 4000,
                    Direction = ParameterDirection.Output
                }
            };
        }
        #endregion

        #region 作业退料
        public void DoMaterialReturn(AscmWmsMtlReturnMain ascmWmsMtlReturnMain, List<AscmWmsMtlReturnDetail> listWmsMtlReturnDetail, string userId, AscmMesInteractiveLog ascmMesInteractiveLog)
        {
            try
            {
                OracleParameter[] stockTransParams = GetMaterialReturnParams(ascmWmsMtlReturnMain, listWmsMtlReturnDetail, userId);
                ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
                OracleCommand command = (OracleCommand)session.Connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "cux_mes_ascm_interface_pkg.ascm_do_manual_return";
                Array.ForEach<OracleParameter>(stockTransParams, P => command.Parameters.Add(P));
                command.ExecuteNonQuery();

                ascmMesInteractiveLog.returnCode = stockTransParams[stockTransParams.Length - 2].Value.ToString();
                ascmMesInteractiveLog.returnMessage = stockTransParams[stockTransParams.Length - 1].Value.ToString();

				if (ascmWmsMtlReturnMain != null)
				{
					ascmWmsMtlReturnMain.returnCode = ascmMesInteractiveLog.returnCode;
					ascmWmsMtlReturnMain.returnMessage = ascmMesInteractiveLog.returnMessage;
					ascmWmsMtlReturnMain.uploadTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public OracleParameter[] GetMaterialReturnParams(AscmWmsMtlReturnMain ascmWmsMtlReturnMain, List<AscmWmsMtlReturnDetail> listWmsMtlReturnDetail, string userId)
        {
            //注意MES存储过程中要求字符串数组长度一致，并且一一对应
            string[] itemCodeLst = listWmsMtlReturnDetail.Select(P => P.materialId.ToString()).ToArray();
            string[] locIdLst = new string[itemCodeLst.Length];
            string[] backQtyLst = listWmsMtlReturnDetail.Select(P => P.quantity.ToString()).ToArray();
            string[] moBatchNoLst = new string[itemCodeLst.Length];
            string[] labelIds = new string[itemCodeLst.Length];
            //为数组元素指定默认值
            for (int i = 0; i < itemCodeLst.Length; i++)
            {
                locIdLst[i] = "";
                moBatchNoLst[i] = "";
                labelIds[i] = "";
            }

            return new OracleParameter[] {
                //库存组织
                new OracleParameter{
                    ParameterName = "i_org_id",
                    OracleDbType = OracleDbType.Int32,
                    Value = orgId,
                    Direction = ParameterDirection.Input
                },
                //MES闭环单号
                new OracleParameter{
                    ParameterName = "i_bill_no",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmWmsMtlReturnMain.docNumber,
                    Direction = ParameterDirection.Input
                },
                //作业号
                new OracleParameter{
                    ParameterName = "i_mo_code",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmWmsMtlReturnMain.wipEntityId,
                    Direction = ParameterDirection.Input
                },
                //仓库
                new OracleParameter{
                    ParameterName = "i_inv_code",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmWmsMtlReturnMain.warehouseId,
                    Direction = ParameterDirection.Input
                },
                //退货区域
                new OracleParameter{
                    ParameterName = "i_return_area",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmWmsMtlReturnMain.returnArea,
                    Direction = ParameterDirection.Input
                },
                //交接人
                new OracleParameter{
                    ParameterName = "i_conn_user",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = "",
                    Direction = ParameterDirection.Input
                },
                //备注
                new OracleParameter{
                    ParameterName = "i_remark",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmWmsMtlReturnMain.memo,
                    Direction = ParameterDirection.Input
                },
                //领料交易取消上传
                new OracleParameter{
                    ParameterName = "i_chk_all_flag",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = "N",
                    Direction = ParameterDirection.Input
                },
                //用户名称
                new OracleParameter{
                    ParameterName = "i_user_code",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = userId,
                    Direction = ParameterDirection.Input
                },
                //物料编码（字符串数组）
                new OracleParameter{
                    ParameterName = "i_item_code_lst",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = itemCodeLst,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //位置（字符串数组）
                new OracleParameter{
                    ParameterName = "i_loc_id_lst",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = locIdLst,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //退料数量（字符串数组）
                new OracleParameter{
                    ParameterName = "i_back_qty_lst",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = backQtyLst,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //生产批次（字符串数组）
                new OracleParameter{
                    ParameterName = "i_mo_batch_no_lst",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = moBatchNoLst,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //标签号（字符串数组）
                new OracleParameter{
                    ParameterName = "i_label_id",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = labelIds,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //退料原因
                new OracleParameter{
                    ParameterName = "i_reason_id",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmWmsMtlReturnMain.reasonId,
                    Direction = ParameterDirection.Input
                },
                //返回参(值为0时表示成功)
                new OracleParameter{
                    ParameterName = "o_ret_code",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 4000,
                    Direction = ParameterDirection.Output
                },
                //返回参(错误信息)
                new OracleParameter{
                    ParameterName = "o_ret_msg",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 4000,
                    Direction = ParameterDirection.Output
                }
            };
        }
        #endregion

        #region 退料单退料
        public void DoSysReturn(AscmWmsMtlReturnMain ascmWmsMtlReturnMain, List<AscmWmsMtlReturnDetail> listWmsMtlReturnDetail, string userId, AscmMesInteractiveLog ascmMesInteractiveLog)
        {
            try
            {
                OracleParameter[] stockTransParams = GetSysReturnParams(ascmWmsMtlReturnMain, listWmsMtlReturnDetail, userId);
                ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
                OracleCommand command = (OracleCommand)session.Connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "cux_mes_ascm_interface_pkg.ascm_do_sys_return";
                Array.ForEach<OracleParameter>(stockTransParams, P => command.Parameters.Add(P));
                command.ExecuteNonQuery();

                ascmMesInteractiveLog.returnCode = stockTransParams[stockTransParams.Length - 2].Value.ToString();
                ascmMesInteractiveLog.returnMessage = stockTransParams[stockTransParams.Length - 1].Value.ToString();

				if (ascmWmsMtlReturnMain != null)
				{
					ascmWmsMtlReturnMain.returnCode = ascmMesInteractiveLog.returnCode;
					ascmWmsMtlReturnMain.returnMessage = ascmMesInteractiveLog.returnMessage;
					ascmWmsMtlReturnMain.uploadTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public OracleParameter[] GetSysReturnParams(AscmWmsMtlReturnMain ascmWmsMtlReturnMain, List<AscmWmsMtlReturnDetail> listWmsMtlReturnDetail, string userId)
        {
            //注意MES存储过程中要求字符串数组长度一致，并且一一对应
            string[] itemCodeLst = listWmsMtlReturnDetail.Select(P => P.materialId.ToString()).ToArray();
            string[] invCodeLst = listWmsMtlReturnDetail.Select(P => P.warehouseId).ToArray();
            string[] locIdLst = new string[itemCodeLst.Length];
            string[] delBarCodeLst = new string[itemCodeLst.Length];
            string[] backQtyLst = listWmsMtlReturnDetail.Select(P => P.quantity.ToString()).ToArray();
            string[] moBatchNoLst = new string[itemCodeLst.Length];
            string[] vendorIdLst = new string[itemCodeLst.Length];
            string[] labelIds = new string[itemCodeLst.Length];
            //为数组元素指定默认值
            for (int i = 0; i < itemCodeLst.Length; i++)
            {
                locIdLst[i] = "";
                delBarCodeLst[i] = "";
                moBatchNoLst[i] = "";
                vendorIdLst[i] = "";
                labelIds[i] = "";
            }

            return new OracleParameter[] {
                //库存组织
                new OracleParameter{
                    ParameterName = "i_org_id",
                    OracleDbType = OracleDbType.Int32,
                    Value = orgId,
                    Direction = ParameterDirection.Input
                },
                //MES闭环单号
                new OracleParameter{
                    ParameterName = "i_bill_no",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmWmsMtlReturnMain.docNumber,
                    Direction = ParameterDirection.Input
                },
                //退料单号
                new OracleParameter{
                    ParameterName = "i_matereturn_no",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmWmsMtlReturnMain.releaseNumber,
                    Direction = ParameterDirection.Input
                },
                //退料原因
                new OracleParameter{
                    ParameterName = "i_reason_id",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmWmsMtlReturnMain.reasonId,
                    Direction = ParameterDirection.Input
                },
                //退货区域
                new OracleParameter{
                    ParameterName = "i_return_area",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = ascmWmsMtlReturnMain.returnArea,
                    Direction = ParameterDirection.Input
                },
                //备注
                new OracleParameter{
                    ParameterName = "i_remark",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 240,
                    Value = ascmWmsMtlReturnMain.memo,
                    Direction = ParameterDirection.Input
                },
                //交接人
                new OracleParameter{
                    ParameterName = "i_conn_user",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = "",
                    Direction = ParameterDirection.Input
                },
                //领料交易取消上传
                new OracleParameter{
                    ParameterName = "i_chk_all_flag",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = "N",
                    Direction = ParameterDirection.Input
                },
                //用户名称
                new OracleParameter{
                    ParameterName = "i_user_code",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 80,
                    Value = userId,
                    Direction = ParameterDirection.Input
                },
                //物料编码（字符串数组）
                new OracleParameter{
                    ParameterName = "i_item_code_lst",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = itemCodeLst,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //仓库（字符串数组）
                new OracleParameter{
                    ParameterName = "i_inv_code_lst",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = invCodeLst,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //位置（字符串数组）
                new OracleParameter{
                    ParameterName = "i_loc_id_lst",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = locIdLst,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //送货单号（字符串数组）
                new OracleParameter{
                    ParameterName = "i_del_bar_code_lst",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = delBarCodeLst,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //退料数量（字符串数组）
                new OracleParameter{
                    ParameterName = "i_back_qty_lst",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = backQtyLst,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //生产批次（字符串数组）
                new OracleParameter{
                    ParameterName = "i_mo_batch_no_lst",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = moBatchNoLst,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //供应商ID（字符串数组）
                new OracleParameter{
                    ParameterName = "i_vendor_id_lst",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = vendorIdLst,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //标签号（字符串数组）
                new OracleParameter{
                    ParameterName = "i_label_id",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = labelIds,
                    Direction = ParameterDirection.Input,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray
                },
                //返回参(值为0时表示成功)
                new OracleParameter{
                    ParameterName = "o_ret_code",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 4000,
                    Direction = ParameterDirection.Output
                },
                //返回参(错误信息)
                new OracleParameter{
                    ParameterName = "o_ret_msg",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 4000,
                    Direction = ParameterDirection.Output
                }
            };
        }
        #endregion

        public void ExecuteOraProcedure(string commandText, ref OracleParameter[] commandParameters)
        {
            ISession session = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession();
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    OracleCommand command = (OracleCommand)session.Connection.CreateCommand();
                    tx.Enlist(command);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = commandText;
                    Array.ForEach<OracleParameter>(commandParameters, P => command.Parameters.Add(P));
                    command.ExecuteNonQuery();

                    tx.Commit();//正确执行提交
                }
                catch (Exception ex)
                {
                    tx.Rollback();//回滚
                    YnBaseClass2.Helper.LogHelper.GetLog().Error("执行存储过程失败", ex);
                    throw ex;
                }
            }
        }
    }
}
