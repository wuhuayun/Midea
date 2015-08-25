<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<YnFrame.Dal.Entities.YnUser>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	YnUser
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!--用户-->
    <div region="center" title="" border="true" style="padding:0px 0px 0px 0px;overflow:hidden;">
        <div title="" class="easyui-layout" fit="true" style="background:#fafafa;">
            <div region="north" title="" border="false" style="height:30px;overflow:hidden;">
                <div class="div_toolbar" style="float:left; height:26px; width:100%;border-bottom:1px solid #99BBE8; padding:1px;">
    				<a class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="userSave()">保存</a>
	            </div>
            </div>
            <div region="center" title="" border="false" style="padding:5px 5px 5px 5px;overflow:hidden;">
		        <form id="editUserForm" method="post">
			        <table style="width:500px;" border="0" cellpadding="0" cellspacing="0">
				        <tr style="height:32px">
					        <td style="width: 80px; text-align:right;" nowrap>
						        <span>用户账号：</span>
					        </td>
					        <td style="">
                            
						        <input class="easyui-validatebox" id="userId" name="userId" type="text" style="width:200px;background-color:#CCCCCC;" value="<%=Model.userId %>" readonly="readonly"/>
					        </td>
				        </tr>
				        <tr style="height:32px">
					        <td style="text-align:right;" nowrap>
						        <span>用户姓名：</span>
					        </td>
					        <td style="">
						        <input class="easyui-validatebox" required="true" id="userName" name="userName" type="text" style="width:200px;" value="<%=Model.userName %>"/>
					        </td>
				        </tr>
				        <tr style="height:32px">
					        <td style="text-align:right;" nowrap>
						        <span>性别：</span>
					        </td>
					        <td>
						        <select id="sex" name="sex" style="width:100px;">
							        <option value="男" <%=(Model.sex=="男")?"selected":"" %>>男</option>
							        <option value="女" <%=(Model.sex=="女")?"selected":"" %>>女</option>
						        </select>
					        </td>
				        </tr>
				        <tr style="height:32px">
					        <td style="text-align:right;" nowrap>
						        <span>EMail：</span>
					        </td>
					        <td>
						        <input class="easyui-validatebox" id="email" name="email" type="text" style="width:200px;" value="<%=Model.email %>"/>
					        </td>
				        </tr>
				        <tr style="height:24px">
					        <td style="text-align:right;" nowrap>
						        <span>办公电话：</span>
					        </td>
					        <td>
						        <input class="easyui-validatebox" id="officeTel" name="officeTel" type="text" style="width:200px;" value="<%=Model.officeTel %>"/>
					        </td>
				        </tr>
				        <tr style="height:24px">
					        <td style="text-align:right;" nowrap>
						        <span>移动电话：</span>
					        </td>
					        <td>
						        <input class="easyui-validatebox" id="mobileTel" name="mobileTel" type="text" style="width:200px;" value="<%=Model.mobileTel %>"/>
					        </td>
				        </tr>
				        <tr style="height:24px">
					        <td style="text-align:right;" nowrap>
						        <span>账号锁定：</span>
					        </td>
					        <td>
						        <input class="easyui-validatebox" type="checkbox" id="isAccountLocked" name="isAccountLocked" value="" <%=(Model.isAccountLocked)?"checked":"" %>/>
					        </td>
				        </tr>
			        </table>
		        </form>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
		function userSave(){
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
		            $('#editUserForm').form('submit', {
		                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnUser/UserSave/' + encodeURIComponent("<%=Model.userId %>"),
		                onSubmit: function () {
		                    $("#isAccountLocked").val($("#isAccountLocked").prop("checked"));
		                    return $('#editUserForm').form('validate');
		                },
		                success: function (r) {
		                    var rVal = $.parseJSON(r);
		                    if (rVal.result) {
		                    } else {
		                        $.messager.alert('确认', '修改信息失败:' + rVal.message, 'error');
		                    }
		                }
		            });
                }
            })

        }
    </script>
</asp:Content>
