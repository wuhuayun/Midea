<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<MideaAscm.Dal.Warehouse.Entities.AscmWorkshopBuilding>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    厂房区域图
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="north" title="" border="false" style="height: 30px; overflow: hidden;
        padding: 0px 0px 2px 0px;">
        <div class="easyui-panel" title="" fit="true" border="true" style="overflow: hidden;">
            <div class="div_toolbar" style="float: left; height: 28px; width: 100%; padding: 1px;">
                <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
			    <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="AddWarelocation();">增加区域货位</a>
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-remove" onclick="BuildingAreaLinkWarehouse();">关联子库</a>
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-remove" onclick="BuildingAreaLinkUser();">关联用户</a>
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="BuildingAreaSetCategory();">设置物料大类</a>
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="BuildingAreaCancelCheckAll()">取消选择</a>
                <%} %>
            </div>
        </div>
    </div>
    <div region="center" id="centerDiv" title="" border="false" style="padding: 0px; overflow: hidden;">
        <div class="easyui-layout" data-options="fit:true">
            <div region="west" split="false" border="false" title="" style="width:850px;padding:0px 2px 0px 0px;overflow:auto;">
                <div class="easyui-panel" id="pBuildingArea" title="<%=Model.name %>" style="padding: 5px; overflow:auto;";
                    data-options="border:true,fit:true,noheader:true">
                </div>
            </div>
            <div region="center" title="" border="false" style="padding:0px 0px 0px 0px;">
                <div class="easyui-panel" style="padding: 0px; overflow:auto;";
                    data-options="border:true,fit:true,noheader:true">
                    <div style="height:50%;">
                        <table id="dgLocationMaterial" title="货位管理" style="" border="false" fit="true" singleSelect="true"
			                idField="id" sortOrder="asc" striped="true" toolbar="#tb_dgLocationMaterial">
		                </table>
                        <div id="tb_dgLocationMaterial">
                            <input id="txtMaterial" type="text" style="width:100px;" />
                            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="MaterialReload();"></a>
                            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="MaterialReload();">刷新</a>
                        </div>
                    </div>

                    <div class="easyui-panel" id='wBuildingLocationInfo' style="padding: 0px; overflow:auto;";
                         data-options="border:true,noheader:true">
                    </div>
                </div>
            </div>
        </div>
        <div id="wBuildingLocationMap" class="easyui-window" title="厂房区域货位图" style="padding: 5px;width:720px;height:480px;"
            data-options="iconCls:'icon-edit',closed:true,maximizable:false,minimizable:false,resizable:false,collapsible:false,modal:true,shadow:true">
            <div class="easyui-panel" title="" fit="true" border="true" style="overflow:hidden;">
                <div class="easyui-layout" fit="true">
                    <div region="north" title="" border="false" style="height:30px;overflow:hidden;">
                        <div class="div_toolbar" style="float:left; height:26px; width:100%; border-bottom:1px solid #99BBE8; padding:1px;">
                            <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
			                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-remove" onclick="LocationLinkWarehouse();">关联子库</a>
                            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-remove" onclick="LocationLinkUser();">关联用户</a>
                            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="LocationSetCategory();">设置物料大类</a>
                            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="LocationCancelCheckAll()">取消选择</a>
                            <%} %>
	                    </div>
                    </div>
                    <div region="center" id="rBuildingLocationMap"  border="false" style="overflow:auto;">
                    </div>
                </div>
            </div>
        </div>
        <div id="wAddWarelocation" class="easyui-window" title="添加区域货位" style="padding:10px;width:500px;height:300px;"
			iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
			    <div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
			        <form id="fAddWarelocation" method="post" style="">
				        <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					        <tr style="height:45px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>水平区域划分：</span>
						        </td>
						        <td style="width:70%; padding-left:5px;">
							        <input class="easyui-slider" id="horizontal" name="horizontal" style="width:240px" 
                                        data-options="showTip:true,
                                                      min:1,
                                                      max:9,
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
							        <span>垂直区域划分：</span>
						        </td>
						        <td style="width:70%; padding-left:5px;">
							        <input class="easyui-slider" id="vertical" name="vertical" style="width:240px" 
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
							        <span>货架层数：</span>
						        </td>
						        <td style="width:80%">
                                    <input class="easyui-numberbox" id="layer" type="text" style="width:120px;" data-options="min:1,max:9" /><span style="color:Red;">*</span>
						        </td>
                                <td style="width: 10%;" nowrap></td>
					        </tr>
				        </table>
			        </form>
				</div>
				<div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
					<a class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="SaveWarelocation()">确定</a>
                    <%} %>
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#wAddWarelocation').window('close');">取消</a>
				</div>
			</div>
		</div>
        <%-- 选择子库 --%>
        <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseSelect.ascx"); %>
        <%-- 选择仓管员 --%>
        <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseUserInfoSelect.ascx"); %>
        <div id="wSetCategory" class="easyui-window" title="设置物料大类" style="padding:10px;width:460px;height:260px;"
			iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
			    <div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
				    <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					    <tr style="height:24px">
						    <td style="width: 20%; text-align:right;" nowrap>
							    <span>物料大类：</span>
						    </td>
						    <td style="width:80%">
                                <input class="easyui-validatebox" id="categoryCode" name="categoryCode" type="text" data-options="validType:'checkNumber[4]'" style="width:120px;" /><span>（物料编码前四位）</span><span style="color:Red;">*</span>
						    </td>
					    </tr>
				    </table>
				</div>
				<div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
					<a class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="SetCategoryOK()">确定</a>
                    <%} %>
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#wSetCategory').window('close');">取消</a>
				</div>
			</div>
		</div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <style type="text/css">
        table.buildingAreaTable
        {
        	border-spacing:20px;
        	border-collapse:separate;
        	width:auto;
        	height:auto;
        }
        table.buildingAreaTable tr
        {
            height:100px;	
        }
        table.buildingAreaTable tr td
        {
        	/*-webkit-box-shadow:1px 1px 1px #000;
        	-moz-box-shadow:1px 1px 1px #000;
        	box-shadow:1px 1px 1px 1px #000;*/
        	background:#007799;
        	color:#ffffff;
        	padding:10px;
        	text-align:center;
        	vertical-align:middle;
        	width:100px;
        	font-family: "Times New Roman", Times, serif; 
            font-size: 23px;
            cursor:pointer;  
        }
        table.buildingAreaLocationTable
        {
        	border-spacing:5px;
        	border-collapse:separate;
        	width:auto;
        	height:auto;
        }
        table.buildingAreaLocationTable tr td
        {
        	-webkit-box-shadow:1px 1px 1px #000;
        	-moz-box-shadow:1px 1px 1px #000;
        	box-shadow:1px 1px 1px #000;
        } 
        table.buildingAreaLocationTable tr td div
        {
            -webkit-box-shadow:1px 1px 1px #000;
        	-moz-box-shadow:1px 1px 1px #000;
        	box-shadow:1px 1px 1px 1px #000;
        	background:#008866;
        	color:#ffffff;
        	text-align:center;
        	width:100px;
        	height:60px; 
            line-height:60px; 
            cursor:pointer;
            margin:5px;
            font-family: "Times New Roman", Times, serif; 
        }
    </style>
    <%--厂房区域图、区域货位图--%>
    <script type="text/javascript">
        $.extend($.fn.validatebox.defaults.rules, {
            checkNumber: {
                validator: function (value, param) {
                    var reg = new RegExp("^([0-9]|[a-z]|[A-Z]){" + param[0] + "}$");
                    return reg.test(value);
                },
                message: '必须输入{0}位字符.'
            }
        });
        var arraySelectArea = [];
        var arraySelectNull = [];
        $(function () {
            LoadWorkshopBuildingMap();
        })
        function BuildingAreaCancelCheckAll() {
            arraySelectArea = [];
            arraySelectNull = [];
            $('#tBuildingArea tr td').each(function(){
                $(this).css({ "background":"#007799", "color":"#ffffff" });
            });
        }
        function AddWarelocation() {
            if (arraySelectArea.length == 0){
                $.messager.alert('提示', '请先选择厂房区域', 'info');
                return;
            }
            $('#wAddWarelocation').window('open');
            $('#fAddWarelocation').form('clear');
            $('#horizontal').slider('setValue', 1);
            $('#spanHShow').html('(1列)');
            $('#vertical').slider('setValue', 1);
            $('#spanVShow').html('(1行)');
        }
        function SaveWarelocation() {
            $.messager.confirm("确认", "确认提交保存？", function (r) {
                if (r) {
                    var options = {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/BuildingAreaWarelocationSave/<%=Model.id %>',
                        type: 'POST',
                        dataType: 'json',
                        data: { 
                            "buildingAreas": arraySelectArea.toString(),
                            "horizontal": $('#horizontal').slider('getValue'),
                            "vertical": $('#vertical').slider('getValue'),
                            "layer": $('#layer').numberbox('getValue')
                        },
                        beforeSubmit: function () {
                            return $('#fAddWarelocation').form('validate');
                        },
                        success: function (r) {
                            if (r.result) {
                                LoadWorkshopBuildingMap();

                                $('#wAddWarelocation').window('close');
                                $.messager.alert('确认', '区域货位添加成功', 'info');
                            } else {
                                $.messager.alert('错误', '区域货位添加失败:' + r.message, 'error');
                            }
                        }
                    };
                    $('#fAddWarelocation').ajaxForm(options);
                    $('#fAddWarelocation').submit();
                }
            });
        }
        //加载厂房区域
        function LoadWorkshopBuildingMap() {
            $.ajax({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/LoadWorkshopBuildingMap/<%=Model.id %>',
                type: 'post',
                dataType: 'json',
                success: function (r) {
                    if (r.result) {
                        $('#pBuildingArea').empty();
                        $('#pBuildingArea').append(r.entity);
                        arraySelectArea = [];
                        arraySelectNull = [];
                        $('#tBuildingArea tr td').bind({
                            click: function () {
                                // 支持区域多选
                                var index = $.inArray($(this).attr('id'), arraySelectArea);
                                //判断是否有选择空货位的厂房区域
                                if ($(this).children('div').eq(1).text().length == 1) {
                                    if (index == -1) {
                                        arraySelectNull.push($(this).attr('id'));
                                    } else {
                                        arraySelectNull.splice(0 , 1);
                                    }
                                }
                                if (index == -1) {
                                    $(this).css({ "background": "#ffaa00", "color": "#000000" });
                                    arraySelectArea.push($(this).attr('id'));
                                } else {
                                    arraySelectArea.splice(index, 1);
                                    $(this).css({ "background": "#007799", "color": "#ffffff" });
                                }
                                BuildingAreaInfo();
                            },
                            dblclick: function () {
                                if ($.inArray($(this).attr('id'), arraySelectArea) == -1) {
                                    arraySelectArea.push($(this).attr('id'));
                                }
                                $(this).css({ "background": "#ffaa00", "color": "#000000" });
                                $('#wBuildingLocationMap').window('open');
                                loadBuildingLocationMap();
                            },
                            mouseover: function () {
                                //$(this).css({ "background":"#ffaa00", "color":"#000000" });
                                $(this).css({ "color": "#FF0000" });
                            },
                            mouseout: function () {
                                if ($.inArray($(this).attr('id'), arraySelectArea) == -1) {
                                    $(this).css({ "background": "#007799", "color": "#ffffff" });
                                } else {
                                    $(this).css({ "color": "#ffffff" });
                                }
                            }
                        })
                    } else {
                        $.messager.alert('错误', '获取厂房区域货位失败:' + r.message, 'error');
                    }
                }
            })
        }
        //加载区域货位
        function loadBuildingLocationMap(){
            $.ajax({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/LoadBuildingLocationMap/<%=Model.id %>',
                type: 'post',
                dataType: 'json',
                data: { "buildingArea": arraySelectArea[arraySelectArea.length-1] },
                success: function (r) {
                    if (r.result) {
                        $('#rBuildingLocationMap').empty();
                        $('#rBuildingLocationMap').append(r.entity);
                        arraySelectLocation = [];
                        $("#tBuildingAreaLocation tr td div").hover(
                            function () {
                                $(this).css({ "background": "#ffaa00", "color": "#000000" });
                            },
                            function () {
                                if ($.inArray($(this).attr('id'), arraySelectLocation) == -1) {
                                    $(this).css({ "background": "#008866", "color": "#ffffff" });
                                }
                            }
                        );
                    } else {
                        $.messager.alert('错误', '获取厂房区域失败:' + r.message, 'error');
                    }
                }
            })
        }
        function BuildingAreaInfo() {
            $.ajax({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/LoadBuildingAreaInfo/<%=Model.id %>',
                type: 'post',
                dataType: 'json',
                data: { "buildingAreas": arraySelectArea.toString() },
                success: function (r) {
                    if (r.result) {
                        $('#wBuildingLocationInfo').empty();
                        $('#wBuildingLocationInfo').append(r.entity);
                    } else {
                        $.messager.alert('错误', '获取厂房区域信息失败:' + r.message, 'error');
                    }
                }
            })
        }
    </script>
    <%--厂房区域关联子库--%>
    <script type="text/javascript">
        var operatorType = "";
        function BuildingAreaLinkWarehouse() {
            if (arraySelectArea.length == 0) {
                $.messager.alert('提示', '请先选择厂房区域', 'info');
                return;
            } else if (arraySelectNull.length > 0) {
                $.messager.alert('提示', '请先为该区域添加货位', 'info');
                return;
            }
            operatorType = "BuildingArea";
            SelectWarehouse();
        }
        function WarehouseSelected(selectRow) {
            if (selectRow) {
                if (operatorType == "BuildingArea") {
                    $.messager.confirm('确认', '确认厂房区域与子库关联？', function (result) {
                        if (result) {
                            $.ajax({
                                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/BuildingWarehouseLinkAdd/<%=Model.id %>',
                                type: "post",
                                dataType: "json",
                                data: { "buildingAreas": arraySelectArea.toString(), "warehouseId": selectRow.id },
                                success: function (r) {
                                    if (r.result) {
                                        $.messager.alert('确认', '厂房区域与子库关联成功', 'info');
                                    } else {
                                        $.messager.alert('错误', '关联失败:' + r.message, 'error');
                                    }
                                }
                            });
                        }
                    });
                } else if (operatorType == "Warelocation") {
                    $.messager.confirm('确认', '确认货位与子库关联？', function (result) {
                        if (result) {
                            $.ajax({
                                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/LocationWarehouseLinkAdd/<%=Model.id %>',
                                type: "post",
                                dataType: "json",
                                data: { "locationIds": arraySelectLocation.toString(), "warehouseId": selectRow.id },
                                success: function (r) {
                                    if (r.result) {
                                        loadBuildingLocationMap();
                                        $.messager.alert('确认', '货位与子库关联成功', 'info');
                                    } else {
                                        $.messager.alert('错误', '关联失败:' + r.message, 'error');
                                    }
                                }
                            });
                        }
                    });
                }
            }
        }
        function BuildingAreaSetCategory() {
            if (arraySelectArea.length == 0) {
                $.messager.alert('提示', '请先选择厂房区域', 'info');
                return;
            }else if (arraySelectNull.length > 0) {
                $.messager.alert('提示', '请先为该区域添加货位', 'info');
                return;
            }
            operatorType = "BuildingArea";
            $('#wSetCategory').window('open');
            $('#categoryCode').val("");
        }
        function SetCategoryOK() {
            if (operatorType == "BuildingArea") {
                $.messager.confirm("确认", "确认提交保存？", function (r) {
                    if (r) {
                        $.ajax({
                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/SetBuildingAreaCategory/<%=Model.id %>',
                            type: "post",
                            dataType: "json",
                            data: { "buildingAreas": arraySelectArea.toString(), "categoryCode": $('#categoryCode').val() },
                            beforeSend: function () {
                                return $('#categoryCode').validatebox('isValid');
                            },
                            success: function (r) {
                                if (r.result) {
                                    $('#wSetCategory').window('close');
                                    $.messager.alert('确认', '物料大类设置成功', 'info');
                                } else {
                                    $.messager.alert('错误', '物料大类设置失败:' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
            } else if (operatorType == "Warelocation") {
                $.messager.confirm("确认", "确认提交保存？", function (r) {
                    if (r) {
                        $.ajax({
                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/SetLocationCategory/',
                            type: "post",
                            dataType: "json",
                            data: { "locationIds": arraySelectLocation.toString(), "categoryCode": $('#categoryCode').val() },
                            beforeSend: function () {
                                return $('#categoryCode').validatebox('isValid');
                            },
                            success: function (r) {
                                if (r.result) {
                                    loadBuildingLocationMap();
                                    $('#wSetCategory').window('close');
                                    $.messager.alert('确认', '物料大类设置成功', 'info');
                                } else {
                                    $.messager.alert('错误', '设置失败:' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
            }
        }
    </script>
    <%--区域货位关联子库--%>
    <script type="text/javascript">
        var arraySelectLocation = [];
        $(function () {
            $('#tBuildingAreaLocation tr td div').live({
                click: function () {
                    // 支持货位多选
                    var index = $.inArray($(this).attr('id'), arraySelectLocation);
                    if (index == -1) {
                        $(this).css({ "background": "#ffaa00", "color": "#000000" });
                        arraySelectLocation.push($(this).attr('id'));
                    } else {
                        arraySelectLocation.splice(index, 1);
                        $(this).css({ "background": "#008866", "color": "#ffffff" });
                    }
                }
            });
        })
        function LocationLinkWarehouse() {
            if (arraySelectLocation.length == 0) {
                $.messager.alert('提示', '请先选择货位', 'info');
                return;
            }
            operatorType = "Warelocation";
            SelectWarehouse();
        }
        function LocationSetCategory() {
            if (arraySelectLocation.length == 0) {
                $.messager.alert('提示', '请先选择货位', 'info');
                return;
            }
            operatorType = "Warelocation";
            $('#wSetCategory').window('open');
            $('#categoryCode').val("");
        }
        function LocationCancelCheckAll() {
            arraySelectLocation = [];
            $('#tBuildingAreaLocation tr td div').each(function () {
                $(this).css({ "background": "#008866", "color": "#ffffff" });
            });
        }
    </script>
    <%--厂房区域关联用户--%>
    <script type="text/javascript">
        function BuildingAreaLinkUser() {
            if (arraySelectArea.length == 0) {
                $.messager.alert('提示', '请先选择厂房区域', 'info');
                return;
            } else if (arraySelectNull.length > 0) {
                $.messager.alert('提示', '请先为该区域添加货位', 'info');
                return;
            }

            operatorType = "BuildingArea";
            SelectUserInfo();
        }
        function UserInfoSelected(selectRow) {
            if (selectRow) {
                if (operatorType == "BuildingArea") {
                    $.messager.confirm('确认', '确认厂房区域与子库关联？', function (result) {
                        if (result) {
                            $.ajax({
                                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/BuildingAreaUserInfoAdd/<%=Model.id %>',
                                type: "post",
                                dataType: "json",
                                data: { "buildingAreas": arraySelectArea.toString(), "userId": selectRow.userId },
                                success: function (r) {
                                    if (r.result) {
                                        $.messager.alert('确认', '厂房区域与仓管员关联成功', 'info');
                                    } else {
                                        $.messager.alert('错误', '关联失败:' + r.message, 'error');
                                    }
                                }
                            });
                        }
                    });
                } else if (operatorType == "Warelocation") {
                    $.messager.confirm('确认', '确认货位与子库关联？', function (result) {
                        if (result) {
                            $.ajax({
                                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/LocationUserInfoAdd/<%=Model.id %>',
                                type: "post",
                                dataType: "json",
                                data: { "locationIds": arraySelectLocation.toString(), "userId": selectRow.userId },
                                success: function (r) {
                                    if (r.result) {
                                        $.messager.alert('确认', '货位与仓管员关联成功', 'info');
                                    } else {
                                        $.messager.alert('错误', '关联失败:' + r.message, 'error');
                                    }
                                }
                            });
                        }
                    });
                }
            }
        }
    </script>
    <%--区域货位关联用户--%>
    <script type="text/javascript">
        function LocationLinkUser() {
            if (arraySelectLocation.length == 0) {
                $.messager.alert('提示', '请先选择货位', 'info');
                return;
            }
            operatorType = "Warelocation";
            SelectUserInfo();
        }
    </script>

    <%-- 货位-物料-查询 --%>
    <script type="text/javascript">
        $(function () {
            $('#dgLocationMaterial').datagrid({
                loadMsg: '更新数据......',
                columns: [[
                    { field: 'id', hidden: 'true' },
                    { field: 'materialDocNumber', title: '物料编码', width: 100, align: 'center' },
                    { field: 'quantity', title: '物料数量', width: 70, align: 'right' },
                    { field: 'locationDocNumber', title: '货位编码', width: 70, align: 'center' }
                ]]
            });
        });

        function MaterialReload() {
            var options = $('#dgLocationMaterial').datagrid('options');
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/LocationMaterialList/';
            options.queryParams.queryWord = $('#txtMaterial').val();
            options.queryParams.buildingId = "<%=Model.id %>";
            $('#dgLocationMaterial').datagrid('reload');
        }

    </script>
</asp:Content>
