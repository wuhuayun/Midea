<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	厂房管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!--厂房-->
    <div region="west" split="false" border="false" title="" style="width:480px;padding:0px 2px 0px 0px;overflow:auto;">
		<div class="easyui-panel" title="" fit="true" border="true">
			<div class="easyui-layout" fit="true" style="" border="false">
			    <div region="center" title="" border="false">
					<table id="dgWorkshopBuilding" title="厂房管理" style="" border="false" fit="true" singleSelect="true"
			            idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb">
		            </table>
                    <div id="tb">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="Reload();">刷新</a>
                        <% if (ynWebRight.rightAdd){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="Add();">增加</a>
                        <%} %>
                        <% if (ynWebRight.rightEdit){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="Edit();">修改</a>
                        <%} %>
                        <% if (ynWebRight.rightDelete){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="Delete();">删除</a>
                        <%} %>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="LookMap();">区域图</a>
                    </div>
                    <div id="editWorkshopBuilding" class="easyui-window" title="厂房信息" style="padding: 5px;width:540px;height:420px;"
			            iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			            <div class="easyui-layout" fit="true">
				            <div region="center" border="false" style="padding:5px;background:#fff;border:1px solid #ccc;">
                                <form id="editWorkshopBuildingForm" method="post" style="">
				                    <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>名称：</span>
						                    </td>
						                    <td style="width:70%">
							                    <input class="easyui-validatebox" id="name" name="name" type="text" style="width:120px;" /><span style="color:Red;">*</span>
						                    </td>
                                            <td style="width: 10%; text-align:left;" nowrap>
						                    </td>
					                    </tr>
                                        <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>代号：</span>
						                    </td>
						                    <td style="width:70%">
							                    <input class="easyui-validatebox" id="code" name="code" data-options="validType:'charCode'" type="text" style="width:120px;" /><span style="color:Red;">*</span>
						                    </td>
                                            <td style="width: 10%; text-align:left;" nowrap>
						                    </td>
					                    </tr>
                                        <tr style="height:45px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>水平厂房划分：</span>
						                    </td>
						                    <td style="width:70%; padding-left:5px;">
							                    <input class="easyui-slider" id="horizontal" name="horizontal" style="width:300px" 
                                                    data-options="showTip:true,
                                                                  min:1,
                                                                  max:9,
                                                                  tipFormatter:hTipFormatter,
                                                                  onChange: function(newValue, oldValue) {
                                                                      $('#spanHShow').html('(' + newValue + '列)');
                                                                  }"/>
						                    </td>
                                            <td style="width: 10%; text-align:left;" nowrap>
                                                <span id="spanHShow"></span>
						                    </td>
					                    </tr>
                                        <tr style="height:45px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>垂直厂房划分：</span>
						                    </td>
						                    <td style="width:70%; padding-left:5px;">
							                    <input class="easyui-slider" id="vertical" name="vertical" style="width:300px" 
                                                    data-options="showTip:true,
                                                                  min:65,
                                                                  max:90,
                                                                  tipFormatter: function(value) {
                                                                      return String.fromCharCode(value);
                                                                  },
                                                                  onChange: function(newValue, oldValue) {
                                                                      $('#spanVShow').html('(' + (newValue - 64) + '行)');
                                                                  }"/>  
						                    </td>
                                            <td style="width: 10%; text-align:left;" nowrap>
                                                <span id="spanVShow"></span>
						                    </td>
					                    </tr>
                                        <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>描述：</span>
						                    </td>
						                    <td style="width:70%">
							                    <input class="easyui-validatebox" id="description" name="description" type="text" style="width:300px;" />
						                    </td>
                                            <td style="width: 10%; text-align:left;" nowrap>
						                    </td>
					                    </tr>
				                    </table>
			                    </form>
				            </div>
				            <div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                                <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
					            <a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="Save()">保存</a>
                                <%} %>
					            <a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editWorkshopBuilding').window('close');">取消</a>
				            </div>
			            </div>
		            </div>
			    </div>
			</div>
		</div>
	</div>
    <!--关联子库-->
    <div region="center" title="" border="false" style="padding:0px 0px 0px 0px;">
		<div class="easyui-panel" fit="true" border="false">
			<div class="easyui-layout" fit="true" style="">
			    <div region="center" border="true" fit="true" style="padding:0px 0px 0px 0px;overflow:auto;">
					<table id="dgBuildingWarehouseLink" title="关联子库" style="" border="false" fit="true" singleSelect="true"
			            idField="id" sortName="" sortOrder="" striped="true" toolbar="#tb2">
		            </table>
                    <div id="tb2">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="LinkReload();">刷新</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <%--厂房--%>
    <script type="text/javascript">
        $.extend($.fn.validatebox.defaults.rules,{
            charCode: {
                validator: function(value, param) {
                    var reg = new RegExp("^([0-9]|[a-z]|[A-Z])$");
                    return reg.test(value);
                },
                message: '必须输入单个数字或字母.'
            }
        });
        $(function () {
            $('#dgWorkshopBuilding').datagrid({
                rownumbers: true,
                loadMsg: '更新数据......',
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WorkshopBuildingList/',
                frozenColumns: [[
                    { field: 'id', hidden: true },
                    { field: 'name', title: '厂房名称', width: 140, align: 'left' }
                ]],
                columns: [[
                    { field: 'code', title: '代号', width: 60, align: 'center' },
                    { field: 'description', title: '描述', width: 220, align: 'left' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId = rowData.id;
                    LinkReload();
                },
                onDblClickRow: function (rowIndex, rowData) {
                    <% if (ynWebRight.rightEdit){ %>
                    Edit();
                    <%} %>
                },
                onLoadSuccess: function (data) {
                    $(this).datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    } else {
                        LinkReload();
                    }
                }
            });
        });
        function hTipFormatter (value) {
            if (value > 9) { 
                value = value + 87;
                return String.fromCharCode(value);  
            } 
            return value;
        }
        function Reload() {
            $('#dgWorkshopBuilding').datagrid('reload');
        }
        var currentId = null;
        function Add() {
            $('#editWorkshopBuilding').window('open');
            $("#editWorkshopBuildingForm").form('clear');

            $('#horizontal').slider('setValue', 1);
            $('#spanHShow').html('(1列)');
            $('#vertical').slider('setValue', 1);
            $('#spanVShow').html('(1行)');
            $('#name').focus();
            currentId = "";
        }
        function Edit() {
            var selectRow = $('#dgWorkshopBuilding').datagrid('getSelected');
            if (selectRow) {
                $('#editWorkshopBuilding').window('open');
                $.ajax({
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WorkshopBuildingEdit/' + selectRow.id,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        if (r) {
                            $("#editWorkshopBuildingForm").form('clear');
                            $('#name').val(r.name);
                            $('#code').val(r.code);
                            $('#description').val(r.description);
                            $('#horizontal').slider('setValue', r.horizontal);
                            $('#vertical').slider('setValue', r.vertical);

                            $('#name').focus();
                        }
                    }
                });
                currentId = selectRow.id;
            } else {
                $.messager.alert('提示', '请选择厂房', 'info');
            }
        }
        function Save() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    $('#editWorkshopBuildingForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WorkshopBuildingSave/' + currentId,
                        onSubmit: function () {
                            return $('#editWorkshopBuildingForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editWorkshopBuilding').window('close');
                                currentId = rVal.id;
                                Reload();
                            } else {
                                $.messager.alert('错误', '保存失败:' + rVal.message, 'error');
                            }
                        }
                    });
                }
            });
        }
        function Delete() {
            var selectRow = $('#dgWorkshopBuilding').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm('确认', '确认删除厂房[<font color="red">' + selectRow.name + '</font>]？', function (result) {
                    if (result) {
                        $.ajax({
                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WorkshopBuildingDelete/' + selectRow.id,
                            type: "post",
                            dataType: "json",
                            success: function (r) {
                                if (r.result) {
                                    currentId = null;
                                    Reload();
                                } else {
                                    $.messager.alert('确认', '删除失败:' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
            } else {
                $.messager.alert('提示', '请选择要删除厂房', 'info');
            }
        }
        function LookMap() {
            var selectRow = $('#dgWorkshopBuilding').datagrid('getSelected');
            if (selectRow) {
                var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WorkshopBuildingMap/' + selectRow.id + "?mi=<%=Request["mi"].ToString() %>";
                parent.openTab('厂房_' + selectRow.name, url);
            } else {
                $.messager.alert('提示', '请选择厂房！', 'info');
            }
        }
    </script>
    <%--厂房与子库关联--%>
    <script type="text/javascript">
        $(function () {
            $('#dgBuildingWarehouseLink').datagrid({
                idField: 'id',
                checkOnSelect: false,
                selectOnCheck: false,
                loadMsg: '更新数据......',
                columns: [[
                    { field: 'id', hidden: 'true' },
                    { field: 'warehouseId', title: '子库名称', width: 100, align: 'center' },
                    { field: 'warehouseName', title: '子库描述', width: 300, align: 'left' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentLinkId=rowData.id;
                },
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
                    if (currentLinkId) {
                        $(this).datagrid('selectRecord', currentLinkId);
                    }
                }
            });
        });
        var currentLinkId = null;
        function LinkReload() {
            var options = $('#dgBuildingWarehouseLink').datagrid('options');
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/BuildingWarehouseLinkList/';
            options.queryParams.buildingId = currentId;    
            $('#dgBuildingWarehouseLink').datagrid('reload');
        }
    </script>
</asp:Content>
