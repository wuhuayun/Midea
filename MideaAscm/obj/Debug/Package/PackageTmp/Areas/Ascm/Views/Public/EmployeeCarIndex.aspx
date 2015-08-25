<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	美的员工车辆基本信息管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataGridEmployeeCar" title="美的员工车辆信息管理" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb1">
		</table>
        <div id="tb1">
            <input id="employeeCarSearch" type="text" style="width:100px;" />
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a>
            <% if (ynWebRight.rightAdd){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="Add();">增加</a>
            <%} %>
            <% if (ynWebRight.rightEdit){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="Edit();">修改</a>
            <%} %>
            <% if (ynWebRight.rightDelete){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="Delete();">删除</a>
            <%} %>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="Print();">打印</a>
        </div>
		<div id="editEmployeeCar" class="easyui-window" title="员工车辆信息修改" style="padding: 10px;width:540px;height:460px;"
			iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
				<div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
                    <form id="editEmployeeCarForm" method="post" style="">
				        <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
                            <%-- 
				            <tr style="height:24px">
					            <td style="width: 20%; text-align:right;" nowrap>
						            <span>员工编号：</span>
					            </td>
					            <td style="width:80%">
						            <input class="easyui-validatebox" id="employeeDocNumber" name="employeeDocNumber" type="text" style="width:120px; background-color:#CCCCCC;" readonly="readonly"/>
							        <input class="easyui-validatebox" id="employeeId" name="employeeId" type="text" style="width:10px;display:none"/>
							        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" iconCls="icon-extract" title="选择员工" onclick="SelectEmployee()"></a>
							        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" iconCls="icon-remove" title="移除员工" onclick="RemoveEmployee()"></a>
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="width: 20%; text-align:right;" nowrap>
						            <span>员工姓名：</span>
					            </td>
					            <td style="width:80%">
						            <input class="easyui-validatebox" id="employeeName" name="employeeName" type="text" style="width:120px; background-color:#CCCCCC;" readonly="readonly" />
					            </td>
				            </tr>--%>
				            <tr style="height:24px">
					            <td style="width: 20%; text-align:right;" nowrap>
						            <span>员工编号：</span>
					            </td>
					            <td style="width:80%">
						            <input class="easyui-validatebox" id="employeeDocNumber" name="employeeDocNumber" type="text" style="width:120px;" />
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="width: 20%; text-align:right;" nowrap>
						            <span>员工姓名：</span>
					            </td>
					            <td style="width:80%">
						            <input class="easyui-validatebox" id="employeeName" name="employeeName" type="text" style="width:120px;" />
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="text-align:right;" nowrap>
						            <span>性别：</span>
					            </td>
					            <td>
						            <select id="employeeSex" name="employeeSex" style="width:124px;">
							            <option value="男">男</option>
							            <option value="女">女</option>
						            </select>
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="text-align:right;" nowrap>
						            <span>身份证号：</span>
					            </td>
					            <td>
						            <input class="easyui-validatebox" id="employeeIdNumber" name="employeeIdNumber" type="text" style="width:120px;"/>
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="text-align:right;" nowrap>
						            <span>办公电话：</span>
					            </td>
					            <td>
						            <input class="easyui-validatebox" id="employeeOfficeTel" name="employeeOfficeTel" type="text" style="width:120px;"/>
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="text-align:right;" nowrap>
						            <span>移动电话：</span>
					            </td>
					            <td>
						            <input class="easyui-validatebox" id="employeeMobileTel" name="employeeMobileTel" type="text" style="width:120px;"/>
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="text-align:right;" nowrap>
						            <span>车牌号：</span>
					            </td>
					            <td>
						            <input class="easyui-validatebox" id="plateNumber" name="plateNumber" type="text" style="width:120px;"/><span style="color:Red;">*</span>
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="text-align:right;" nowrap>
						            <span>型号：</span>
					            </td>
					            <td>
						            <input class="easyui-validatebox" id="spec" name="spec" type="text" style="width:120px;"/>
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="text-align:right;" nowrap>
						            <span>颜色：</span>
					            </td>
					            <td>
						            <input class="easyui-validatebox" id="color" name="color" type="text" style="width:120px;"/>
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="text-align:right;" nowrap>
						            <span>座位数量：</span>
					            </td>
					            <td>
						            <input class="easyui-validatebox" id="seatCount" name="seatCount" type="text" validType="number" style="width:120px;"/>
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="text-align:right;" nowrap>
						            <span>RFID：</span>
					            </td>
					            <td>
			                        <select id="rfid1" name="rfid1" style="width:80px;">
                                        <% List<string> listPre24G = MideaAscm.Dal.Base.Entities.AscmRfid.Pre24G.GetList(); %>
                                        <% if (listPre24G != null && listPre24G.Count > 0)
                                            { %>
                                        <% foreach (string pre24G in listPre24G)
                                            { %>
                                        <option value="<%=pre24G %>"><%=pre24G%></option>
                                        <% } %>
                                        <% } %>
                                    </select>
						            <input class="easyui-validatebox" id="rfid2" name="rfid2" type="text" style="width:120px;"/>
                                    <input class="easyui-validatebox" id="rfid" name="rfid" type="text" style="display:none;"/>
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="text-align:right;" nowrap>
						            <span>免检：</span>
					            </td>
					            <td>
						            <input class="easyui-validatebox" type="checkbox" id="exemption" name="exemption" value=""/>
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="text-align:right;" nowrap>
						            <span>员工级别：</span>
					            </td>
					            <td>
			                        <select id="employeeLevel" name="employeeLevel" style="width:80px;">
                                        <% List<string> listEmployeeLevelDefine = MideaAscm.Dal.Base.Entities.AscmEmployeeCar.EmployeeLevelDefine.GetList(); %>
                                        <% if (listEmployeeLevelDefine != null && listEmployeeLevelDefine.Count > 0)
                                            { %>
                                        <% foreach (string employeeLevelDefine in listEmployeeLevelDefine)
                                            { %>
                                        <option value="<%=employeeLevelDefine %>"><%=employeeLevelDefine%></option>
                                        <% } %>
                                        <% } %>
                                    </select>
					            </td>
				            </tr>
					        <tr style="height:24px">
						        <td style="text-align:right;" nowrap>
							        <span>备注：</span>
						        </td>
						        <td>
							        <input class="easyui-validatebox" id="memo" name="memo" type="text" style="width:300px;" />
						        </td>
					        </tr>
				        </table>
			        </form>
                    <%-- 选择员工 
                    <%Html.RenderPartial("~/Areas/Ascm/Views/Public/EmployeeSelect.ascx"); %>--%>
				</div>
				<div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
					<a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="Save()">保存</a>
                    <%} %>
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editEmployeeCar').window('close');">取消</a>
				</div>
			</div>
		</div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        $(function () {
            $('#dataGridEmployeeCar').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/EmployeeCarList',
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'plateNumber', title: '车牌号', width: 80, align: 'center' },
                ]],
                columns: [[
					{ field: 'employeeDocNumber', title: '员工编号', width: 80, align: 'center' },
					{ field: 'employeeName', title: '员工姓名', width: 80, align: 'center' },
					{ field: 'employeeSex', title: '性别', width: 50, align: 'center' },
					{ field: 'employeeIdNumber', title: '身份证号', width: 100, align: 'center' },
					{ field: 'employeeOfficeTel', title: '办公电话', width: 100, align: 'center' },
					{ field: 'employeeMobileTel', title: '移动电话', width: 100, align: 'center' },

					{ field: 'spec', title: '型号', width: 150, align: 'left' },
					{ field: 'color', title: '颜色', width: 80, align: 'center' },
					{ field: 'seatCount', title: '座位数量', width: 80, align: 'center' },
					{ field: 'status', title: '状态', width: 60, align: 'center' },
					{ field: 'rfid', title: 'RFID', width: 120, align: 'center' },
                    { field: 'exemption', title: '免检', width: 60, align: 'center' },
                    { field: 'employeeLevel', title: '员工级别', width: 60, align: 'center' },
                    { field: 'memo', title: '备注', width: 200, align: 'left' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId=rowData.id;
                },
                onDblClickRow: function (rowIndex, rowData) {
                    <% if (ynWebRight.rightEdit){ %>
                    Edit();
                    <%} %>
                },
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                }
            });
        });
        function Reload() {
            $('#dataGridEmployeeCar').datagrid('reload');
        }
		function Query(){
			var queryParams = $('#dataGridEmployeeCar').datagrid('options').queryParams;
			queryParams.queryWord = $('#employeeCarSearch').val();
			$('#dataGridEmployeeCar').datagrid('reload');
		}
        var currentId = null;
        function Add() {
            $('#editEmployeeCar').window('open');
            $("#editEmployeeCarForm")[0].reset();
            $('#rfid1').val("<%=MideaAscm.Dal.Base.Entities.AscmRfid.Pre24G.GetList()[0] %>");

            $('#plateNumber').focus();
            currentId = "";
        }
        function Edit() {
            var selectRow = $('#dataGridEmployeeCar').datagrid('getSelected');
            if (selectRow) {
                $('#editEmployeeCar').window('open');
                var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/EmployeeCarEdit/' + selectRow.id;
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        $("#editEmployeeCarForm")[0].reset();

				        $('#plateNumber').val(r.plateNumber);
                        $('#spec').val(r.spec);
				        $('#color').val(r.color);
				        $('#seatCount').val(r.seatCount);
                        $('#rfid1').val(r.rfid1);
				        $('#rfid2').val(r.rfid2);
                        $('#memo').val(r.memo);

//                        $('#employeeId').val(r.employeeId);
                        $('#employeeName').val(r.employeeName);
                        $('#employeeDocNumber').val(r.employeeDocNumber);
                        $('#employeeSex').val(r.employeeSex);
                        $('#employeeIdNumber').val(r.employeeIdNumber);
				        $('#employeeOfficeTel').val(r.employeeOfficeTel);
				        $('#employeeMobileTel').val(r.employeeMobileTel);
                        $("#exemption").prop("checked", r.exemption);
                        $('#employeeLevel').val(r.employeeLevel);

                        $('#plateNumber').focus();
                    }
                });
                currentId = selectRow.id;
            } else {
                $.messager.alert('提示', '请选择员工车辆', 'info');
            }
        }
        function Save() {
			var reg = new RegExp("^[A-Fa-f0-9]+$");
			var txtRfid2 = $('#rfid2').val();
			if(!reg.test(txtRfid2)){
				$.messager.alert('确认', 'RFID输入有误!（允许输入范围是：数字0-9，字母A-F或a-f）', 'error');
				return;
			}

            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    $('#editEmployeeCarForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/EmployeeCarSave/' + currentId,
                        onSubmit: function () {
                            $('#rfid').val($('#rfid1').val()+$('#rfid2').val());
                            $("#exemption").val($("#exemption").prop("checked"));
                            return $('#editEmployeeCarForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editEmployeeCar').window('close');
                                currentId = rVal.id;
                                Reload();
                            } else {
                                $.messager.alert('确认', '修改信息失败:' + rVal.message, 'error');
                            }
                        }
                    });
			    }
			});
        }
        function Delete() {
            var selectRow = $('#dataGridEmployeeCar').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm('确认', '确认删除员工车辆[<font color="red">' + selectRow.plateNumber + '</font>]？', function (result) {
                    if (result) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/EmployeeCarDelete/' + selectRow.id;
                        $.ajax({
                            url: sUrl,
                            type: "post",
                            dataType: "json",
                            success: function (r) {
                                if (r.result) {
                                    Reload();
                                } else {
                                    $.messager.alert('确认', '删除失败:' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
            } else {
                $.messager.alert('提示', '请选择要删除的员工车辆', 'info');
            }
        }
        function Print() {
            var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/Ascm/ReportViewer/EmployeeCarPrint.aspx';
            parent.openTab('员工车辆统计打印', url);
        }
//        function EmployeeSelected(selectRow) {
//            if (selectRow) {
//                $('#employeeId').val(selectRow.id);
//                $('#employeeName').val(selectRow.name);
//                $('#employeeDocNumber').val(selectRow.docNumber);
//            }
//        }
//		function RemoveEmployee(){
//			$('#employeeId').val("");
//			$('#employeeName').val("");
//			$('#employeeDocNumber').val("");
//		}
    </script>
</asp:Content>
