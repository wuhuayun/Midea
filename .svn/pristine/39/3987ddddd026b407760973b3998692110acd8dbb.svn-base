<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	任务生成规则管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding: 0px; overflow: false;">
        <table id="dgTaskRule" title="任务生成规则管理" class="easyui-datagrid" style="" border="false"
                        data-options="fit: true,
                          rownumbers: true,  
                          singleSelect: true,
                          idField: 'id',
                          striped: true,
                          toolbar: '#tb1',
                          pagination: true,
                          pageSize: 50,
                          loadMsg: '更新数据...',
                          url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/LogisticsGenerateTaskRuleList',
                          onSelect: function(rowIndex, rowRec){
                              currentId = rowRec.id;
                          },
                          onDblClickRow: function(rowIndex, rowRec){
                              Edit();
                          },
                          onLoadSuccess: function(){
                              $(this).datagrid('clearSelections');
                              if (currentId) {
                                  $(this).datagrid('selectRecord', currentId);
                              }                     
                          }">
                        <thead data-options="frozen:true">
                            <tr>
                                <%--<th data-options="checkbox:true"></th>--%>
                                <th data-options="field:'id',width:40,align:'center'">
                                    序号
                                </th>
                                <th data-options="field:'identificationIdCn',width:80,align:'center'">
                                    类型
                                </th>
                                <th data-options="field:'ruleTypeCn',width:80,align:'center'">
                                    规则类型
                                </th>
                            </tr>
                        </thead>
                        <thead>
                            <tr>
                                <th data-options="field:'ruleCode',width:240,align:'center'">
                                    生成规则
                                </th>
                                <th data-options="field:'ascmUserInfo_Name',width:90,align:'center'">
                                    指定排产员
                                </th>
                                <th data-options="field:'isEnable',width:70,align:'left'">
                                    是否启用
                                </th>
                                <th data-options="field:'priority',width:60,align:'left'">
                                    优先级
                                </th>
                                <th data-options="field:'description',width:250,align:'left'">
                                    描述
                                </th>
                                <th data-options="field:'others',width:60,align:'center'">
                                    其他
                                </th>
                                <th data-options="field:'tip',width:80,align:'center'">
                                    备注
                                </th>
                                <th data-options="field:'_createTime',width:120,align:'center'">
                                    创建时间
                                </th>
                                <th data-options="field:'_modifyTime',width:120,align:'center'">
                                    最后更新时间
                                </th>
                            </tr>
                        </thead>
                    </table>
        <div id="tb1">
            
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add"
                onclick="Add();">增加</a>

            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit"
                onclick="Edit();">修改</a>

            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel"
                onclick="Delete();">删除</a>

            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-unlock"
                onclick="Enable();">启用</a>

            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-lock"
                onclick="DisEnable();">禁用</a>
        </div>
        <div id="editTaskRule" class="easyui-window" title="任务生成规则管理" style="padding: 10px;
            width: 540px; height: 420px;" iconcls="icon-edit" closed="true" maximizable="false"
            minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
            <div class="easyui-layout" fit="true">
                <div region="center" border="false" style="padding: 10px; background: #fff; border: 1px solid #ccc;">
                    <form id="editTaskRuleForm" method="post">
                    <table style="width: 100%;" border="0" cellpadding="0" cellspacing="0">
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>类　　型：</span>
                            </td>
                            <td>
                                <select id="identificationId" class="easyui-combobox" name="identificationId" style="width:200px;" data-options="panelHeight:'auto'">
                                    <% List<int> listIdentificationDefine = MideaAscm.Dal.GetMaterialManage.Entities.AscmDiscreteJobs.IdentificationIdDefine.GetList(); %>
                                    <% if (listIdentificationDefine != null && listIdentificationDefine.Count > 0)
                                        { %>
                                    <% foreach (int IdentificationDefine in listIdentificationDefine)
                                        { %>
                                    <option value="<%=IdentificationDefine %>"><%=MideaAscm.Dal.GetMaterialManage.Entities.AscmDiscreteJobs.IdentificationIdDefine.DisplayText(IdentificationDefine).Trim()%></option>
                                    <% } %>
                                    <% } %>
                                </select>
                                <span style="color: Red;">*</span>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="width: 20%; text-align: right;" nowrap>
                                <span>任务类型：</span>
                            </td>
                            <td style="width: 80%">
                                <select id="ruleType" class="easyui-combobox" name="ruleType" style="width:200px;" data-options="panelHeight:'auto'">
                                    <% List<string> listTypeDefine = MideaAscm.Dal.GetMaterialManage.Entities.AscmGenerateTaskRule.RuleTypeDefine.GetList(); %>
                                    <% if (listTypeDefine != null && listTypeDefine.Count > 0)
                                        { %>
                                    <% foreach (string typeDefine in listTypeDefine)
                                        { %>
                                    <option value="<%=typeDefine %>"><%=MideaAscm.Dal.GetMaterialManage.Entities.AscmGenerateTaskRule.RuleTypeDefine.DisplayText(typeDefine).Trim() %></option>
                                    <% } %>
                                    <% } %>
                                </select>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>生成规则：</span>
                            </td>
                            <td>
                                <input class="easyui-validatebox" id="ruleCode" name="ruleCode" type="text" style="width: 300px;" />
								<a href="javascript:void(0);" id="btnEditRuleCode" class="easyui-linkbutton" 
								plain="true" iconCls="icon-extract" title="编辑详细规则" onclick="btnEditRuleCode_click();"></a>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>指定排产员：</span>
                            </td>
                            <td>
                                <input class="easyui-validatebox" id="relatedRanker" name="relatedRanker" type="text"
                                    style="width: 300px;" />
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>其他：</span>
                            </td>
                            <td>
                                <input class="easyui-validatebox" id="others" name="others" type="text"
                                    style="width: 300px;" />
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>备注：</span>
                            </td>
                            <td>
                                <textarea class="easyui-validatebox" id="tip" name="tip" rows="3"
                                    cols="342" style="width: 342px;"></textarea>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <%--<span>备注：</span>--%>
                            </td>
                            <td>
                            <br /><br />
                                <div style="font-size:14px; color:Red">*任务生成规则的生成方式及优先级根据任务类型改变，且优先级的数值越低优先级别越高</div>
                            </td>
                        </tr>
                    </table>
                    </form>
                </div>
                <div region="south" border="false" style="text-align: right; height: 30px; line-height: 30px;">

                    <a id="btnSave" class="easyui-linkbutton" iconcls="icon-ok" href="javascript:void(0)"
                        onclick="Save()">保存</a>

                    <a class="easyui-linkbutton" iconcls="icon-cancel" href="javascript:void(0)" onclick="$('#editTaskRule').window('close');">
                        取消</a>
                </div>
            </div>
        </div>

		<div id="winEditRuleCode" class="easyui-window" title="生成规则详细编辑" style="padding: 5px;
            width: 560px; height: 440px;" iconcls="icon-edit" closed="true" maximizable="false"
            minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
            <div class="easyui-layout" fit="true">
                <div region="center" border="false" style="padding: 0px; background: #fff; border: 1px solid #ccc;">
                    <table style="width: 520px;" border="0" cellpadding="0" cellspacing="0">
						<tr style="height: 30px">
                            <td style="width: 70px; text-align:left;" nowrap>
                                子库：
                            </td>
                            <td style="width: 190px">
                                <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseSelectCombo.ascx",
									  new MideaAscm.Code.SelectCombo { id = "cmbSubInventory", width = "150px" });%>
                            </td>

							<td style="width: 70px; text-align: left;" nowrap>
                                物料：
                            </td>
                            <td style="width: 190px">
                                <%Html.RenderPartial("~/Areas/Ascm/Views/Public/MaterialItemSelectCombo.ascx",
									  new MideaAscm.Code.SelectCombo { id = "cmbMateriel", width = "150px" }); %>
								<a id="A2" class="easyui-linkbutton" iconcls="icon-save" plain="true"
								href="javascript:void(0)" onclick="SaveMaterielNO()"></a>
                            </td>
                        </tr>

                        <tr>
                            <td style="width: 80px; height: 5px;text-align: right;">
                                &nbsp;
                            </td>
                        </tr>

						<tr>
                            <td style="width: 260px;text-align: right; vertical-align:top" colspan="2" nowrap>
                                <table id="dgWarehouse" style="" border="true" singleSelect="true"
									idField="warehouseCode" striped="true" toolbar="#toolbar_dgWarehouse">
								</table>
								<div id="toolbar_dgWarehouse">
									<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="dgSub_Delete('#dgWarehouse');">删除</a>
								</div>
                            </td>
							<td style="width: 260px;text-align: right; vertical-align:top" colspan="2" nowrap>
                                <table id="dgMaterial" style="" border="true" singleSelect="true"
									idField="materialCode" striped="true" toolbar="#toolbar_dgMaterial">
								</table>
								<div id="toolbar_dgMaterial">
									<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="dgSub_Delete('#dgMaterial');">删除</a>
								</div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div region="south" border="false" style="text-align: right; height: 30px; line-height: 30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit)
                       { %>
                    <a id="A1" class="easyui-linkbutton" iconcls="icon-ok" href="javascript:void(0)"
                        onclick="SaveEditRuleCode()">保存</a>
                    <%} %>
                    <a class="easyui-linkbutton" iconcls="icon-cancel" href="javascript:void(0)" onclick="$('#winEditRuleCode').window('close');">
                        取消
					</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>

	<script type="text/javascript">
		var ruleEntity = {};

		$(function ()
		{
			$('#dgWarehouse').datagrid({
				rownumbers: true,
				width: 250,
				height: 270,
				columns: [[
                { field: 'warehouseCode', title: '子库代码', width: 100, align: 'left' }
            ]],
				onSelect: function (rowIndex, rowData)
				{
					deleteAllRows("#dgMaterial");
					var warehouse = rowData.warehouseCode;
					if (!warehouse) return;

					if (ruleEntity[warehouse])
					{
						var entityMaterial = ruleEntity[warehouse];
						for (var material in entityMaterial)
						{
							$("#dgMaterial").datagrid("appendRow", { materialCode: material });
						}
					}
				}
			});

			$('#dgMaterial').datagrid({
				rownumbers: true,
				width: 250,
				height: 270,
				columns: [[
                { field: 'materialCode', title: '物料编号', width: 150, align: 'left' }
            ]],
				onSelect: function (rowIndex, rowData)
				{
					$('#cmbMateriel').combogrid('setText', rowData.materialCode);
				}
			});

			$('#cmbMateriel').combogrid({ idField: 'docNumber' });

			$('#cmbSubInventory').combogrid({ onSelect: cmbSubInventory_select });
			$('#cmbMateriel').combogrid({ onSelect: cmbMateriel_select });
		});

		function dgSub_Delete(ctrlID)
		{
			var row = $(ctrlID).datagrid('getSelected');
			if (!row) return;
			var rowIndex = $(ctrlID).datagrid('getRowIndex', row);

			var warehouse = null;
			var rowWarehouse = $('#dgWarehouse').datagrid("getSelected");
			if (rowWarehouse) warehouse = rowWarehouse.warehouseCode;
			if (!warehouse)
			{
				alert("请选择【子库】！");
				return;
			}

			if (ctrlID == "#dgMaterial")
			{
				var materiel = row.materialCode;
				delete ruleEntity[warehouse][materiel];
			}
			else
			{
				delete ruleEntity[warehouse];
				deleteAllRows("#dgMaterial");
			}

			$(ctrlID).datagrid('deleteRow', rowIndex);
		}

		function cmbSubInventory_select(selIndex, selRow)
		{
			//$('#cmbMateriel').combogrid({ queryParams: { subInventory: newValue} });
			if (!selRow || !selRow.id) return;

			var newValue = selRow.id.toString();
			var warehouse = newValue;
			if (newValue && newValue.length >= 4)
			{
				warehouse = newValue.substr(0, 4);
			}

			if (ruleEntity[warehouse])
			{
				alert("【子库】已经存在！");
				return;
			}

			$("#dgWarehouse").datagrid("appendRow", { warehouseCode: warehouse });
			ruleEntity[warehouse] = {};
		}

		function cmbMateriel_select(selIndex, selRow)
		{
			if (!selRow || !selRow.docNumber) return;

			var materiel = selRow.docNumber.toString();

			var warehouse = null;
			var row = $('#dgWarehouse').datagrid("getSelected");
			if (row) warehouse = row.warehouseCode;
			if (!warehouse)
			{
				alert("请选择【子库】！");
				$('#cmbMateriel').combogrid("clear");
				return;
			}

			if (ruleEntity[warehouse] && ruleEntity[warehouse][materiel])
			{
				alert("【物料】已经存在！");
				return;
			}

			$("#dgMaterial").datagrid("appendRow", { materialCode: materiel });
			ruleEntity[warehouse][materiel] = "rc";
		}

		function deleteAllRows(ctrlID)
		{
			var rows = $(ctrlID).datagrid("getRows");
			if (rows && rows.length > 0)
			{
				for (var i = rows.length - 1; i >= 0; i--)
				{
					$(ctrlID).datagrid("deleteRow", i);
				}
			}
		}

		function SaveMaterielNO()
		{
			var warehouse = null;
			var row = $('#dgWarehouse').datagrid("getSelected");
			if (row) warehouse = row.warehouseCode;
			if (!warehouse)
			{
				alert("请选择列表中的【子库】！");
				return;
			}

			var material = null;
			var row = $('#dgMaterial').datagrid("getSelected");
			var rowIndex = $('#dgMaterial').datagrid("getRowIndex", row);
			if (row && row.materialCode)
			{
				material = row.materialCode;
			}
			if (!material)
			{
				alert("请选择列表中的【物料】！");
				return;
			}

			var newMaterial = $('#cmbMateriel').combogrid('getText');
			if (newMaterial) newMaterial = newMaterial.replace(" ", "");
			if (!newMaterial || newMaterial.length == 0)
			{
				alert("【物料】为空！");
				return;
			}

			if (ruleEntity[warehouse] && ruleEntity[warehouse][material])
			{
				delete ruleEntity[warehouse][material];
			}

			ruleEntity[warehouse][newMaterial] = "rc";
			$('#dgMaterial').datagrid('updateRow', {
				index: rowIndex,
				row: { materialCode: newMaterial }
			});

			alert("已保存！");
		}

		function SaveEditRuleCode()
		{
			var textRule = "";
			var arrWarehouse = [];
			var arrWarehouseMateriel = [];
			for (var warehouse in ruleEntity)
			{
				arrWarehouse.push(warehouse);

				var arrMateriel = [];
				for (var materiel in ruleEntity[warehouse])
				{
					arrMateriel.push(materiel);
				}
				if (arrMateriel.length > 0)
				{
					arrWarehouseMateriel.push(warehouse + ":" + arrMateriel.join("%"));
				}
			}

			textRule = "[()&()]";
			if (arrWarehouse.length > 0)
			{
				var textRule = "[(" + arrWarehouse.join("|") + ")&(" + arrWarehouseMateriel.join("|") + ")]";
			}

			$('#ruleCode').val(textRule);
			$('#winEditRuleCode').window('close');
		}

		function btnEditRuleCode_click()
		{
			deleteAllRows("#dgWarehouse");
			deleteAllRows("#dgMaterial");
			$('#cmbSubInventory').combogrid("clear");
			$('#cmbMateriel').combogrid("clear");

			setRuleEntity($('#ruleCode').val());
			$('#winEditRuleCode').window('open');
		}

		function setRuleEntity(strRuleCode)
		{
			ruleEntity = {};
			if (!strRuleCode) return;

			var strSubCodes = strRuleCode.replace("[(", "").replace(")]", "").split(")&(");
			if (strSubCodes && strSubCodes.length == 2)
			{
				var strWarehouses = strSubCodes[0].split('|');
				if (strWarehouses && strWarehouses.length > 0)
				{
					for (var i = 0; i < strWarehouses.length; i++)
					{
						ruleEntity[strWarehouses[i]] = {};
					}

					var strWHMateriels = strSubCodes[1].split('|');
					if (strWHMateriels && strWHMateriels.length > 0)
					{
						for (var j = 0; j < strWHMateriels.length; j++)
						{
							var strSubWHMateriels = strWHMateriels[j].split(':');
							if (strSubWHMateriels && strSubWHMateriels.length == 2)
							{
								var subWHName = strSubWHMateriels[0];
								var subMateriels = strSubWHMateriels[1].split('%');
								if (subMateriels && subMateriels.length > 0)
								{
									for (var k = 0; k < subMateriels.length; k++)
									{
										if (ruleEntity[subWHName])
										{
											ruleEntity[subWHName][subMateriels[k]] = "rc";
										}
									}
								}
							}
						}
					}
				}
			}

			for (var warehouse in ruleEntity)
			{
				$("#dgWarehouse").datagrid("appendRow", { warehouseCode: warehouse });
			}
		}

</script>
    <script type="text/javascript">
    	var currentId = null;
    	function Reload()
    	{
    		$('#dgTaskRule').datagrid('reload');
    	}

    	function Add()
    	{
    		$('#editTaskRule').window('open');
    		$("#editTaskRuleForm")[0].reset();

    		currentId = null;
    	}
    	function Edit()
    	{
    		var selectRow = $('#dgTaskRule').datagrid('getSelected');
    		if (selectRow)
    		{
    			$('#editTaskRule').window('open');
    			var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/LogisticsGenerateTaskRuleEdit/' + selectRow.id;
    			$.ajax({
    				url: sUrl,
    				type: "post",
    				dataType: "json",
    				success: function (r)
    				{
    					if (r)
    					{
    						$("#editTaskRuleForm")[0].reset();

    						$('#identificationId').combobox('setValue', r.identificationId);
    						$('#ruleType').combobox('setValue', r.ruleType);
    						$('#ruleCode').val(r.ruleCode);
    						$('#relatedRanker').val(r.relatedRanker);
    						$('#others').val(r.others);
    						$('#tip').val(r.tip);
    					}
    				}
    			});
    			currentId = selectRow.id;
    		} else
    		{
    			$.messager.alert('提示', '请填写必填项', 'info');
    		}
    	}
    	function Save()
    	{
    		$.messager.confirm("确认", "确认提交保存?", function (r)
    		{
    			if (r)
    			{
    				$('#editTaskRuleForm').form('submit', {
    					url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/LogisticsGenerateTaskRuleSave/' + currentId,
    					onSubmit: function ()
    					{
    						return $('#editTaskRuleForm').form('validate');
    					},
    					success: function (r)
    					{
    						var rVal = $.parseJSON(r);
    						if (rVal.result)
    						{
    							$('#editTaskRule').window('close');
    							Reload();
    							currentId = rVal.id;
    						} else
    						{
    							$.messager.alert('确认', '修改信息失败:' + rVal.message, 'error');
    						}
    					}
    				});
    			}
    		});
    	}
    	function Delete()
    	{
    		var selectRow = $('#dgTaskRule').datagrid('getSelected');
    		if (selectRow)
    		{
    			$.messager.confirm('确认', '确认删除该任务生成规则[<font color="red">' + selectRow.id + '</font>]？', function (result)
    			{
    				if (result)
    				{
    					var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/LogisticsGenerateTaskRuleDelete/' + selectRow.id;
    					$.ajax({
    						url: sUrl,
    						type: "post",
    						dataType: "json",
    						success: function (r)
    						{
    							if (r.result)
    							{
    								Reload();
    							} else
    							{
    								$.messager.alert('确认', '删除失败:' + r.message, 'error');
    							}
    						}
    					});
    				}
    			});
    		} else
    		{
    			$.messager.alert('提示', '请选择要删除的领料排配规则', 'info');
    		}
    	}
    </script>
    <script type="text/javascript">
    	function Enable()
    	{
    		var selectRow = $('#dgTaskRule').datagrid('getSelected');
    		if (selectRow)
    		{
    			$.messager.confirm('确认', '确认启用该任务生成规则[<font color="red">' + selectRow.id + '</font>]？', function (result)
    			{
    				if (result)
    				{
    					var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/LogisticsGenerateTaskRuleEnable/' + selectRow.id;
    					$.ajax({
    						url: sUrl,
    						type: "post",
    						dataType: "json",
    						success: function (r)
    						{
    							if (r.result)
    							{
    								Reload();
    							} else
    							{
    								$.messager.alert('确认', '删除失败:' + r.message, 'error');
    							}
    						}
    					});
    				}
    			});
    		} else
    		{
    			$.messager.alert('提示', '请选择要删除的领料排配规则', 'info');
    		}
    	}
    	function DisEnable()
    	{
    		var selectRow = $('#dgTaskRule').datagrid('getSelected');
    		if (selectRow)
    		{
    			$.messager.confirm('确认', '确认禁用该任务生成规则[<font color="red">' + selectRow.id + '</font>]？', function (result)
    			{
    				if (result)
    				{
    					var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/LogisticsGenerateTaskRuleDisEnable/' + selectRow.id;
    					$.ajax({
    						url: sUrl,
    						type: "post",
    						dataType: "json",
    						success: function (r)
    						{
    							if (r.result)
    							{
    								Reload();
    							} else
    							{
    								$.messager.alert('确认', '删除失败:' + r.message, 'error');
    							}
    						}
    					});
    				}
    			});
    		} else
    		{
    			$.messager.alert('提示', '请选择要删除的领料排配规则', 'info');
    		}
    	}
    </script>
</asp:Content>
