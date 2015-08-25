<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<YnFrame.Areas.YnPublic.Controllers.YnSharedController.ParameterModel>" %>

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
				    <table style="width:800px;" border="0" cellpadding="0" cellspacing="0">
					    <tr style="height:24px">
						    <td nowrap="nowrap">
                                <fieldset style="padding:10px;">
                                    <legend>公共参数</legend>
				                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr style="height:24px">
						                    <td style="text-align:right;width:100px;" nowrap>
							                    <span>框架主页标题：</span>
						                    </td>
						                    <td style="text-align:left;width:400px;">
                                                <input class="easyui-validatebox" id="frameHomeTitle" name="frameHomeTitle" type="text" style="width:180px;" value="<%=Model.frameHomeTitle%>"/>
						                    </td>
                                        </tr>
                                        <tr style="height:24px">
						                    <td style="text-align:right;width:100px;" nowrap>
							                    <span>框架主页Url：</span>
						                    </td>
						                    <td style="text-align:left;width:400px;">
                                                <input class="easyui-validatebox" id="frameHomeUrl" name="frameHomeUrl" type="text" style="width:280px;" value="<%=Model.frameHomeUrl%>"/>
						                    </td>
					                    </tr>
				                    </table>
                                </fieldset>
						    </td>
                        </tr>
					    <tr style="height:24px">
						    <td nowrap="nowrap">
                                <fieldset style="padding:10px;">
                                    <legend>其他参数</legend>
				                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr style="height:24px">
						                    <td style="text-align:right;width:100px;" nowrap>
							                    <span></span>
						                    </td>
						                    <td style="text-align:left;width:200px;">
                                                    
						                    </td>
						                    <td style="text-align:right;width:100px;" nowrap>
							                    <span></span>
						                    </td>
						                    <td style="text-align:left;width:200px;">
                                                    
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
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnShared/ParameterSave';
                    $('#editParameterForm').form('submit', {
                        url: sUrl,
                        onSubmit: function () {
                            //return $('#editGoodsPriceForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                //$('#editParameterForm').window('close');
                                $.messager.alert('确认', '保存完成!', 'info');
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
