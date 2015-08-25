<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	车门车道管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!--车门管理-->
    <div region="west" split="false" border="false" title="" style="width:450px;padding:0px 2px 0px 0px;overflow:auto;">
		<div class="easyui-panel" title="" fit="true" border="true">
			<div class="easyui-layout" fit="true" style="" border="false">
			    <div region="center" title="" border="false">
					<table id="dataGridDoor" title="车门管理" style="" border="false" fit="true" singleSelect="true"
			            idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb">
		            </table>
                    <div id="tb">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="DoorReload();">刷新</a>
                        <% if (ynWebRight.rightAdd){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="DoorAdd();">增加</a>
                        <%} %>
                        <% if (ynWebRight.rightEdit){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="DoorEdit();">修改</a>
                        <%} %>
                        <% if (ynWebRight.rightDelete){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="DoorDelete();">删除</a>
                        <%} %>
                        <input id="accMenuSearch" type="text" style="width:100px;" />
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="DoorSearch();"></a> 
                    </div>
			        <div id="editDoor" class="easyui-window" title="车门管理" style="padding: 10px;width:500px;height:300px;"
				        iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
				        <div class="easyui-layout" fit="true">
			                <div region="center" id="editDoorContent" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
			                    <form id="editDoorForm" method="post" style="">
				                    <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>名称：</span>
						                    </td>
						                    <td style="width:80%">
							                    <input class="easyui-validatebox" id="Text1" name="startId" type="text" style="width:100px;" />
						                    </td>
					                    </tr>
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>序号：</span>
						                    </td>
						                    <td style="width:80%">
							                    <input class="easyui-validatebox" id="Text2" name="startId" type="text" style="width:100px;" />
						                    </td>
					                    </tr>
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>方向：</span>
						                    </td>
						                    <td style="width:80%">
							                    <select id="Select2" name="gg" style="width:120px;"><option selected="selected" value=""></option></select>
						                    </td>
					                    </tr>
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>类型：</span>
						                    </td>
						                    <td style="width:80%">
							                    <select id="Select4" name="gg" style="width:120px;"><option selected="selected" value=""></option></select>
						                    </td>
					                    </tr>
					                    <tr style="height:24px">
						                    <td style="text-align:right;" nowrap>
							                    <span>备注：</span>
						                    </td>
						                    <td>
							                    <input class="easyui-validatebox" id="description" name="description" type="text" style="width:300px;" />
						                    </td>
					                    </tr>
				                    </table>
			                    </form>
					        </div>
					        <div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                                <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
						        <a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="DoorSave()">保存</a>
                                <%} %>
						        <a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editDoor').window('close');">取消</a>
					        </div>
				        </div>
			        </div>
			    </div>
			</div>
		</div>
	</div>
    <!--车道管理-->
    <div region="center" title="" border="false" style="padding:0px 0px 0px 0px;">
		<div class="easyui-panel" fit="true" border="false">
			<div class="easyui-layout" fit="true" style="">
			    <div region="center" border="true" fit="true" style="padding:0px 0px 0px 0px;overflow:auto;">
					<table id="dataGridRoadway" title="车道管理" style="" border="false" fit="true" singleSelect="true"
			            idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb2">
		            </table>
                    <div id="tb2">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="RoadwayReload();">刷新</a>
                        <% if (ynWebRight.rightAdd){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="RoadwayAdd('root');">增加</a>
                        <%} %>
                        <% if (ynWebRight.rightEdit){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="RoadwayEdit();">修改</a>
                        <%} %>
                        <% if (ynWebRight.rightDelete){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="RoadwayDelete();">删除</a>
                        <%} %>
                    </div>
			        <div id="editRoadway" class="easyui-window" title="车道管理" style="padding: 10px;width:500px;height:300px;"
				        iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
				        <div class="easyui-layout" fit="true">
			                <div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
			                    <form id="editRoadwayForm" method="post" style="">
				                    <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>名称：</span>
						                    </td>
						                    <td style="width:80%">
							                    <input class="easyui-validatebox" id="Text3" name="startId" type="text" style="width:100px;" />
						                    </td>
					                    </tr>
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>序号：</span>
						                    </td>
						                    <td style="width:80%">
							                    <input class="easyui-validatebox" id="Text4" name="startId" type="text" style="width:100px;" />
						                    </td>
					                    </tr>
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>方向：</span>
						                    </td>
						                    <td style="width:80%">
							                    <select id="Select1" name="gg" style="width:120px;"><option selected="selected" value=""></option></select>
						                    </td>
					                    </tr>
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>类型：</span>
						                    </td>
						                    <td style="width:80%">
							                    <select id="Select3" name="gg" style="width:120px;"><option selected="selected" value=""></option></select>
						                    </td>
					                    </tr>
					                    <tr style="height:24px">
						                    <td style="text-align:right;" nowrap>
							                    <span>备注：</span>
						                    </td>
						                    <td>
							                    <input class="easyui-validatebox" id="Text5" name="description" type="text" style="width:300px;" />
						                    </td>
					                    </tr>
				                    </table>
			                    </form>
					        </div>
					        <div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                                <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
						        <a class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="RoadwaySave()">保存</a>
                                <%} %>
						        <a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editRoadway').window('close');">取消</a>
					        </div>
				        </div>
			        </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        $(function () {
            $('#dataGridDoor').datagrid({
                url: '',
//                pagination: true,
//                pageSize: 50,
                loadMsg: '更新数据......',
                columns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'name', title: '大门名称', width: 100, align: 'center' },
                    { field: 'name', title: '进出方向', width: 80, align: 'center' },
                    { field: 'name', title: '大门类型', width: 80, align: 'center' },
                    { field: 'name', title: '备注', width: 120, align: 'left' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentDoorId=rowData.id;
                },
                onDblClickRow: function (rowIndex, rowData) {
                    <% if (ynWebRight.rightEdit){ %>
                    Edit();
                    <%} %>
                },
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
                    if (currentDoorId) {
                        $(this).datagrid('selectRecord', currentDoorId);
                    }
                }
            });
        });
        function DoorReload() {
            $('#dataGridDoor').datagrid('reload');
        }
        var currentDoorId = null;
        function DoorAdd() {
            $("#editDoor").window("open");
            $("#editDoorForm")[0].reset();

            $("#editDoorForm input[name='name']").focus();
            currentDoorId = null;
        }
    </script>
    <script type="text/javascript">
        $(function () {
            $('#dataGridRoadway').datagrid({
                url: '',
//                pagination: true,
//                pageSize: 50,
                loadMsg: '更新数据......',
                columns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'name', title: '车道名称', width: 150, align: 'center' },
                    { field: 'name', title: '进出方向', width: 80, align: 'center' },
                    { field: 'name', title: '车道类型', width: 80, align: 'center' },
                    { field: 'name', title: '备注', width: 120, align: 'left' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentRoadwayId=rowData.id;
                },
                onDblClickRow: function (rowIndex, rowData) {
                    <% if (ynWebRight.rightEdit){ %>
                    Edit();
                    <%} %>
                },
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
                    if (currentRoadwayId) {
                        $(this).datagrid('selectRecord', currentRoadwayId);
                    }
                }
            });
        });
        function RoadwayReload() {
            $('#dataGridRoadway').datagrid('reload');
        }
        var currentRoadwayId = null;
        function RoadwayAdd() {
            $("#editRoadway").window("open");
            $("#editRoadwayForm")[0].reset();

            $("#editRoadwayForm input[name='name']").focus();
            currentRoadwayId = null;
        }
    </script>
</asp:Content>
