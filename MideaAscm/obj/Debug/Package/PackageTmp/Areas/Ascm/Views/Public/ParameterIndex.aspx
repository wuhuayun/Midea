<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	参数管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div region="center" title="" border="true" style="padding:0px 0px 0px 0px;overflow:hidden;">
        <div title="" class="easyui-layout" fit="true" style="background:#fafafa;">
            <div region="north" title="" border="false" style="height:30px;overflow:hidden;">
                <div class="div_toolbar" style="float:left; height:26px; width:100%;border-bottom:1px solid #99BBE8; padding:1px;">
			        <a href="#" class="easyui-linkbutton" plain="true" icon="icon-save" onclick="parameterSave();">保存</a>
	            </div>
            </div>
            <div region="center" title="" border="false" style="padding:5px 5px 5px 5px;overflow:hidden;">
                <form id="editParameterForm" method="post" style="height:100%;">
				<table style="width:800px" border="0" cellpadding="0" cellspacing="0">
					<tr style="height:24px">
						<td nowrap="nowrap">
                            <fieldset style="padding:10px;">
                                <legend>公共参数</legend>
				                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr style="height:24px">
						                <td style="text-align:right;width:100px;" nowrap>
							                <span>大门LED名称：</span>
						                </td>
						                <td style="text-align:left;width:200px;" colspan="3">
                                            <input class="easyui-validatebox" id="doorLedTitle" name="doorLedTitle" type="text" style="width:400px;" value="<%=Model.doorLedTitle%>"/>
						                </td>
					                </tr>
                                    <tr style="height:24px">
						                <td style="text-align:right;width:100px;" nowrap>
							                <span>卸货点预约失效时长：</span>
						                </td>
						                <td style="text-align:left;width:200px;" colspan="3">
                                            <input class="easyui-numberbox" id="reserveInvalid" name="reserveInvalid" type="text" style="width:100px;text-align:right;" data-options="min:0" value="<%=Model.reserveInvalid%>"/><span>(分钟)</span>
						                </td>
					                </tr>
                                    <tr style="height:24px">
						                <td style="text-align:right;width:100px;" nowrap>
							                <span>供应商到厂放行时长：</span>
						                </td>
						                <td style="text-align:left;width:200px;" colspan="3">
                                            <input class="easyui-numberbox" id="supplierPassDuration" name="supplierPassDuration" type="text" style="width:100px;text-align:right;" data-options="min:0,max:24,precision:1" max="24"  value="<%=Model.supplierPassDuration%>"/><span>(小时)</span>
						                </td>
					                </tr>
                                    <tr style="height:24px">
						                <td style="text-align:right;width:100px;" nowrap>
							                <span>员工车辆免检级别：</span>
						                </td>
						                <td style="text-align:left;width:200px;" colspan="3">
                                            <input class="easyui-validatebox" id="employeeCarExemptionLevel" name="employeeCarExemptionLevel" type="text" style="width:400px;" value="<%=Model.employeeCarExemptionLevel%>"/>
                                            不同级别请用半角逗号分隔
						                </td>
					                </tr>
				                </table>
                            </fieldset>
						</td>
                    </tr>
                </table>
                </form>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript">
        function parameterSave() {
            $.messager.confirm("确认", "确认保存参数设置？", function (r) {
                if (r) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/ParameterSave';
                    $('#editParameterForm').form('submit', {
                        url: sUrl,
                        onSubmit: function () {

                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $.messager.alert('确认', '保存完成!', 'info');
                                document.location.reload();
                            } else {
                                $.messager.alert('确认', '保存失败:' + rVal.message, 'error');
                            }
                        }
                    })
                }
            })
        }
    </script>
</asp:Content>
