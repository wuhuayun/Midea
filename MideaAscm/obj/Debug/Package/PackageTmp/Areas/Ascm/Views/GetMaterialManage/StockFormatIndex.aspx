<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	备料形式批量维护
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding:0px;overflow:false;">
        <table id="dgStockFormat" title="备料形式批量维护" class="easyui-datagrid" style="" border="false"
                        data-options="fit: true,
                          rownumbers: true,  
                          singleSelect: false,
                          idField: 'id',
                          striped: true,
                          toolbar: '#tb1',
                          pagination: true,
                          pageSize: 500,
                          pageList:[100,200,300,400,500],
                          loadMsg: '更新数据...',
                          url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/StockFormatInfoList',
                          onSelect: function(rowIndex, rowRec){
                              currentId = rowRec.id;
                          },
                          onDblClickRow: function(rowIndex, rowRec){
                              <% if (ynWebRight.rightEdit){ %>
                              Edit_Single();
                              <%} %>
                          },
                          onLoadSuccess: function(){
                              $(this).datagrid('clearSelections');
                              if (currentId) {
                                  $(this).datagrid('selectRecord', currentId);
                              }                     
                          }">
                        <thead>
                            <tr>
                                <th data-options="checkbox:true"></th>
                                <th data-options="field:'id',width:80,align:'center',hiddren:true">
                                    ID
                                </th>
                                <th data-options="field:'docNumber',width:100,align:'center'">
                                    编号
                                </th>
                                <th data-options="field:'unit',width:80,align:'center'">
                                    单位
                                </th>
                                <th data-options="field:'wipSupplyTypeCn',width:80,align:'center'">
                                    供应类型
                                </th>
                                <th data-options="field:'_zMtlCategoryStatus',width:100,align:'center'">
                                    总装备料形式
                                </th>
                                <th data-options="field:'_dMtlCategoryStatus',width:100,align:'center'">
                                    电装备料形式
                                </th>
                                <th data-options="field:'_wMtlCategoryStatus',width:100,align:'center'">
                                    其他备料形式
                                </th>
                                <th data-options="field:'description',width:250,align:'left'">
                                    描述
                                </th>
                            </tr>
                        </thead>
                    </table>
        <div id="tb1">
            <span>供应类型：</span>
            <select id="queryType" name="queryType" style="width: 80px;">
                <option value=""></option>
                <% List<int> listTypeDefine = MideaAscm.Dal.Base.Entities.AscmMaterialItem.WipSupplyTypeDefine.GetList(); %>
                <% if (listTypeDefine != null && listTypeDefine.Count > 0)
                    { %>
                <% foreach (int typeDefine in listTypeDefine)
                    { %>
                    <%if (typeDefine <= 3 && typeDefine >= 1)
                      { %>
                    <option value="<%=typeDefine %>">
                    <%=MideaAscm.Dal.Base.Entities.AscmMaterialItem.WipSupplyTypeDefine.DisplayText(typeDefine)%></option>
                    <% } %>
                <% } %>
                <% } %>
            </select>
            <span>总装备料形式：</span>
            <select id="zStatus" name="zStatus" style="width: 80px;">
                <option value="">未维护</option>
                <option value="kz">空值</option>
                <option value="qb">全部</option>
                <% List<string> mtlStatusDefine = MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.GetList(); %>
                <% if (mtlStatusDefine != null && mtlStatusDefine.Count > 0)
                    { %>
                <% foreach (string statusDefine in mtlStatusDefine)
                    { %>
                    <option value="<%=statusDefine %>">
                    <%=MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.DisplayText(statusDefine)%></option>
                <% } %>
                <% } %>
            </select>
            <span>电装备料形式：</span>
            <select id="dStatus" name="dStatus" style="width: 80px;">
                <option value="">未维护</option>
                <option value="kz">空值</option>
                <option value="qb">全部</option>
                <% mtlStatusDefine = MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.GetList(); %>
                <% if (mtlStatusDefine != null && mtlStatusDefine.Count > 0)
                    { %>
                <% foreach (string statusDefine in mtlStatusDefine)
                    { %>
                    <option value="<%=statusDefine %>">
                    <%=MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.DisplayText(statusDefine)%></option>
                <% } %>
                <% } %>
            </select>
            <span>其他备料形式：</span>
            <select id="wStatus" name="wStatus" style="width: 80px;">
                <option value="">未维护</option>
                <option value="kz">空值</option>
                <option value="qb">全部</option>
                <% mtlStatusDefine = MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.GetList(); %>
                <% if (mtlStatusDefine != null && mtlStatusDefine.Count > 0)
                    { %>
                <% foreach (string statusDefine in mtlStatusDefine)
                    { %>
                    <option value="<%=statusDefine %>">
                    <%=MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.DisplayText(statusDefine)%></option>
                <% } %>
                <% } %>
            </select>
            <span>编码段：</span>
            <input id="queryStarDocnumber" type="text" style="width: 100px;" maxlength="12" onblur="rightPadding('#queryStarDocnumber');" />-
            <input id="queryEndDocnumber" type="text" style="width: 100px;" maxlength="12" onblur="rightPadding('#queryEndDocnumber');" />
            <span>描述：</span>
            <input id="queryDescribe" type="text" style="width:100px;" />
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a> 
            <% if (ynWebRight.rightEdit)
               { %>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit"
                onclick="Edit_Single();">修改</a>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit"
                onclick="Edit_Choice();">选择修改</a>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit"
                onclick="Edit_Batch();">批量修改</a>
            <%} %>
            <% if (ynWebRight.rightAdd)
               { %>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add"
                onclick="DataRepair();">维护数据</a>
            <%} %>
            <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="ImportExcel();">导入</a>
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="ExportExcel();">导出</a>
            <% } %>
        </div>
        <div id="popImport" class="easyui-window" title="数据导入" style="padding: 10px;width:380px;height:200px;"
            iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
            <div class="easyui-layout" fit="true">
                <div region="center" id="FileUpload" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
					<form id="FormUpload" enctype="multipart/form-data" method="post" ><br />
                        <p><input type="file" id="fileImport" name="fileImport" size="35" value=''/>&nbsp;&nbsp;</p>
                    </form>
                </div>
				<div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
					<a class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="ImportOk()">保存</a>
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#popImport').window('close');">取消</a>
				</div>
            </div>
        </div>
        <div id="progressbarDiv" class="easyui-window" title="" style="padding: 12px;width:460px;height:90px;"
            closed="true" closable="false" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
            <div id="p" class="easyui-progressbar" style="width:400px;"></div> 
        </div>
        <div id="editStockFormat" class="easyui-window" title="维护备料形式" style="padding: 10px;
            width: 540px; height: 420px;" iconcls="icon-edit" closed="true" maximizable="false"
            minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
            <div class="easyui-layout" fit="true">
                <div region="center" border="false" style="padding: 10px; background: #fff; border: 1px solid #ccc;">
                    <form id="editStockFormatForm" method="post">
                    <table style="width: 100%;" border="0" cellpadding="0" cellspacing="0">
                        <tr style="height: 24px">
                            <td style="width: 20%; text-align: right;" nowrap>
                                <span>物料编码：</span>
                            </td>
                            <td style="width: 80%">
                                <input class="easyui-validatebox" required="true" id="docNumber" name="docNumber" type="text"
                                    style="width: 140px;" style="width: 108px; background-color: #CCCCCC;" readonly="readonly" /><span style="color: Red;">*</span>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>总装备料形式：</span>
                            </td>
                            <td>
                                <select id="sZMtlCategoryStatus" name="zMtlCategoryStatus" style="width: 145px;">
                                    <option value=""></option>
                                    <% List<string> listMtlCategoryStatusDefine = MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.GetList(); %>
                                    <% if (listMtlCategoryStatusDefine != null && listMtlCategoryStatusDefine.Count > 0)
                                        { %>
                                    <% foreach (string CategoryStatusDefine in listMtlCategoryStatusDefine)
                                        { %>
                                    <option value="<%=CategoryStatusDefine %>">
                                        <%=MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.DisplayText(CategoryStatusDefine)%></option>
                                    <% } %>
                                    <% } %>
                                </select>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>电装备料形式：</span>
                            </td>
                            <td>
                                <select id="sDMtlCategoryStatus" name="dMtlCategoryStatus" style="width: 145px;">
                                    <option value=""></option>
                                    <% listMtlCategoryStatusDefine = MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.GetList(); %>
                                    <% if (listMtlCategoryStatusDefine != null && listMtlCategoryStatusDefine.Count > 0)
                                        { %>
                                    <% foreach (string CategoryStatusDefine in listMtlCategoryStatusDefine)
                                        { %>
                                    <option value="<%=CategoryStatusDefine %>">
                                        <%=MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.DisplayText(CategoryStatusDefine)%></option>
                                    <% } %>
                                    <% } %>
                                </select>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>其他备料形式：</span>
                            </td>
                            <td>
                                <select id="sWMtlCategoryStatus" name="wMtlCategoryStatus" style="width: 145px;">
                                    <option value=""></option>
                                    <% listMtlCategoryStatusDefine = MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.GetList(); %>
                                    <% if (listMtlCategoryStatusDefine != null && listMtlCategoryStatusDefine.Count > 0)
                                        { %>
                                    <% foreach (string CategoryStatusDefine in listMtlCategoryStatusDefine)
                                        { %>
                                    <option value="<%=CategoryStatusDefine %>">
                                        <%=MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.DisplayText(CategoryStatusDefine)%></option>
                                    <% } %>
                                    <% } %>
                                </select>
                            </td>
                        </tr>
                    </table>
                    </form>
                </div>
                <div region="south" border="false" style="text-align: right; height: 30px; line-height: 30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit)
                       { %>
                    <a id="btnSave" class="easyui-linkbutton" iconcls="icon-ok" href="javascript:void(0)"
                        onclick="Save_Single();">保存</a>
                    <%} %>
                    <a class="easyui-linkbutton" iconcls="icon-cancel" href="javascript:void(0)" onclick="$('#editStockFormat').window('close');">
                        取消</a>
                </div>
            </div>
        </div>
        <div id="editBathStockFormat" class="easyui-window" title="批量修改数据" style="padding: 10px;
            width: 540px; height: 420px;" iconcls="icon-edit" closed="true" maximizable="false"
            minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
            <div class="easyui-layout" fit="true">
                <div region="center" border="false" style="padding: 10px; background: #fff; border: 1px solid #ccc;">
                    <form id="editBathStockFormatForm" method="post">
                    <table style="width: 100%;" border="0" cellpadding="0" cellspacing="0">
                        <tr style="height: 24px">
                            <td style="width: 20%; text-align: right;" nowrap>
                                <span>编码段：</span>
                            </td>
                            <td style="width: 80%">
                                <input id="sDocnumber" name="sDocnumber" type="text" style="width: 100px;" maxlength="12" onblur="rightPadding('#sDocnumber');" />-
                                <input id="eDocnumber" name="eDocnumber" type="text" style="width: 100px;" maxlength="12" onblur="rightPadding('#eDocnumber');" />
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>供应类型：</span>
                            </td>
                            <td>
                                <select id="wipSupplyType" name="wipSupplyType" style="width: 80px;">
                                    <option value=""></option>
                                    <% listTypeDefine = MideaAscm.Dal.Base.Entities.AscmMaterialItem.WipSupplyTypeDefine.GetList(); %>
                                    <% if (listTypeDefine != null && listTypeDefine.Count > 0)
                                        { %>
                                    <% foreach (int typeDefine in listTypeDefine)
                                        { %>
                                        <%if (typeDefine <= 3 && typeDefine >= 1)
                                          { %>
                                        <option value="<%=typeDefine %>">
                                        <%=MideaAscm.Dal.Base.Entities.AscmMaterialItem.WipSupplyTypeDefine.DisplayText(typeDefine)%></option>
                                        <% } %>
                                    <% } %>
                                    <% } %>
                                </select>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>总装备料形式：</span>
                            </td>
                            <td>
                                <select id="b_zMtlCategoryStatus" name="zMtlCategoryStatus" style="width: 145px;">
                                    <option value=""></option>
                                    <% listMtlCategoryStatusDefine = MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.GetList(); %>
                                    <% if (listMtlCategoryStatusDefine != null && listMtlCategoryStatusDefine.Count > 0)
                                        { %>
                                    <% foreach (string CategoryStatusDefine in listMtlCategoryStatusDefine)
                                        { %>
                                    <option value="<%=CategoryStatusDefine %>">
                                        <%=MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.DisplayText(CategoryStatusDefine)%></option>
                                    <% } %>
                                    <% } %>
                                </select>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>电装备料形式：</span>
                            </td>
                            <td>
                                <select id="b_dMtlCategoryStatus" name="dMtlCategoryStatus" style="width: 145px;">
                                    <option value=""></option>
                                    <% listMtlCategoryStatusDefine = MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.GetList(); %>
                                    <% if (listMtlCategoryStatusDefine != null && listMtlCategoryStatusDefine.Count > 0)
                                        { %>
                                    <% foreach (string CategoryStatusDefine in listMtlCategoryStatusDefine)
                                        { %>
                                    <option value="<%=CategoryStatusDefine %>">
                                        <%=MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.DisplayText(CategoryStatusDefine)%></option>
                                    <% } %>
                                    <% } %>
                                </select>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>其他备料形式：</span>
                            </td>
                            <td>
                                <select id="b_wMtlCategoryStatus" name="wMtlCategoryStatus" style="width: 145px;">
                                    <option value=""></option>
                                    <% listMtlCategoryStatusDefine = MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.GetList(); %>
                                    <% if (listMtlCategoryStatusDefine != null && listMtlCategoryStatusDefine.Count > 0)
                                        { %>
                                    <% foreach (string CategoryStatusDefine in listMtlCategoryStatusDefine)
                                        { %>
                                    <option value="<%=CategoryStatusDefine %>">
                                        <%=MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.DisplayText(CategoryStatusDefine)%></option>
                                    <% } %>
                                    <% } %>
                                </select>
                            </td>
                        </tr>
                    </table>
                    </form>
                </div>
                <div region="south" border="false" style="text-align: right; height: 30px; line-height: 30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit)
                       { %>
                    <a id="btnBathSave" class="easyui-linkbutton" iconcls="icon-ok" href="javascript:void(0)"
                        onclick="Save_Batch();">保存</a>
                    <%} %>
                    <a class="easyui-linkbutton" iconcls="icon-cancel" href="javascript:void(0)" onclick="$('#editBathStockFormat').window('close');">
                        取消</a>
                </div>
            </div>
        </div>
        <div id="editChoiceStockFormat" class="easyui-window" title="多选修改数据" style="padding: 10px;
            width: 540px; height: 420px;" iconcls="icon-edit" closed="true" maximizable="false"
            minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
            <div class="easyui-layout" fit="true">
                <div region="center" border="false" style="padding: 10px; background: #fff; border: 1px solid #ccc;">
                    <form id="editChoiceStockFormatForm" method="post">
                    <table style="width: 100%;" border="0" cellpadding="0" cellspacing="0">
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>总装备料形式：</span>
                            </td>
                            <td>
                                <select id="c_zMtlCategoryStatus" name="zMtlCategoryStatus" style="width: 145px;">
                                    <option value=""></option>
                                    <% listMtlCategoryStatusDefine = MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.GetList(); %>
                                    <% if (listMtlCategoryStatusDefine != null && listMtlCategoryStatusDefine.Count > 0)
                                        { %>
                                    <% foreach (string CategoryStatusDefine in listMtlCategoryStatusDefine)
                                        { %>
                                    <option value="<%=CategoryStatusDefine %>">
                                        <%=MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.DisplayText(CategoryStatusDefine)%></option>
                                    <% } %>
                                    <% } %>
                                </select>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>电装备料形式：</span>
                            </td>
                            <td>
                                <select id="c_dMtlCategoryStatus" name="dMtlCategoryStatus" style="width: 145px;">
                                    <option value=""></option>
                                    <% listMtlCategoryStatusDefine = MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.GetList(); %>
                                    <% if (listMtlCategoryStatusDefine != null && listMtlCategoryStatusDefine.Count > 0)
                                        { %>
                                    <% foreach (string CategoryStatusDefine in listMtlCategoryStatusDefine)
                                        { %>
                                    <option value="<%=CategoryStatusDefine %>">
                                        <%=MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.DisplayText(CategoryStatusDefine)%></option>
                                    <% } %>
                                    <% } %>
                                </select>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>其他备料形式：</span>
                            </td>
                            <td>
                                <select id="c_wMtlCategoryStatus" name="wMtlCategoryStatus" style="width: 145px;">
                                    <option value=""></option>
                                    <% listMtlCategoryStatusDefine = MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.GetList(); %>
                                    <% if (listMtlCategoryStatusDefine != null && listMtlCategoryStatusDefine.Count > 0)
                                        { %>
                                    <% foreach (string CategoryStatusDefine in listMtlCategoryStatusDefine)
                                        { %>
                                    <option value="<%=CategoryStatusDefine %>">
                                        <%=MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.DisplayText(CategoryStatusDefine)%></option>
                                    <% } %>
                                    <% } %>
                                </select>
                            </td>
                        </tr>
                    </table>
                    </form>
                </div>
                <div region="south" border="false" style="text-align: right; height: 30px; line-height: 30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit)
                       { %>
                    <a id="A1" class="easyui-linkbutton" iconcls="icon-ok" href="javascript:void(0)"
                        onclick="Save_Choice();">保存</a>
                    <%} %>
                    <a class="easyui-linkbutton" iconcls="icon-cancel" href="javascript:void(0)" onclick="$('#editChoiceStockFormat').window('close');">
                        取消</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
<script type="text/javascript">
    function rightPadding(obj) {
        var temp = $(obj).val();
        if (temp != "" && temp != null) {
            if (temp.length < 12) {
                var str = "";
                for (var i = 0; i < 12 - temp.length; i++) {
                    str += "0";
                }
                temp += str;
            }
        }
        $(obj).val(temp);
    }
</script>
<script type="text/javascript">
    var currentId = null;
    function Query() {
        var queryParams = $('#dgStockFormat').datagrid('options').queryParams;
        queryParams.queryType = $('#queryType').val();
        queryParams.queryStarDocnumber = $('#queryStarDocnumber').val();
        queryParams.queryEndDocnumber = $('#queryEndDocnumber').val();
        queryParams.zStatus = $('#zStatus').val();
        queryParams.dStatus = $('#dStatus').val();
        queryParams.wStatus = $('#wStatus').val();
        queryParams.queryDescribe = $('#queryDescribe').val();

        $('#dgStockFormat').datagrid('reload');
    }
</script>
<script type="text/javascript">
    function DataRepair() {
        $.messager.confirm('确认', '是否维护物料大类、物料小类及物料信息？', function (result) {
            if (result) {
                $.ajax({
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/DataRepair',
                    type: "post",
                    success: function (r) {
                        if (r.result) {
                            if (r.message != "" && r.message != null) {
                                $.messager.alert('确认', r.message, 'info');
                            }
                            Query();
                        } else {
                            $.messager.alert('确认', '维护物料信息失败:' + r.message, 'error');
                        }
                    }
                });
            }
        });
    }
</script>
<script type="text/javascript">
    function Edit_Single() {
        var selectRow = $('#dgStockFormat').datagrid('getSelected');
        if (selectRow) {
            $('#editStockFormat').window('open');
            var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/StockFormatEdit/' + selectRow.id;
            $.ajax({
                url: sUrl,
                type: "post",
                dataType: "json",
                success: function (r) {
                    if (r) {
                        $("#editStockFormatForm")[0].reset();

                        $('#docNumber').val(r.docNumber);
                        $('#sZMtlCategoryStatus').val(r.zMtlCategoryStatus);
                        $('#sDMtlCategoryStatus').val(r.dMtlCategoryStatus);
                        $('#sWMtlCategoryStatus').val(r.wMtlCategoryStatus);
                    }
                }
            });
            currentId = selectRow.id;
        } else {
            $.messager.alert('提示', '请选择物料信息！', 'info');
        }
    }
    function Save_Single() {
        $.messager.confirm("确认", "确认提交保存?", function (r) {
            if (r) {
                $('#editStockFormatForm').form('submit', {
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/StockFormatSave/' + currentId,
                    onSubmit: function () {
                        return $('#editStockFormatForm').form('validate');
                    },
                    success: function (r) {
                        var rVal = $.parseJSON(r);
                        if (rVal.result) {
                            $('#editStockFormat').window('close');
                            currentId = rVal.id;
                            if (rVal.message != null && rVal.message != "") {
                                $.message.alert('提示', rVal.message, 'info');
                            }
                            Query();
                        } else {
                            $.messager.alert('确认', '修改信息失败:' + rVal.message, 'error');
                        }
                    }
                });
            }
        });
    }
</script>
<script type="text/javascript">
    function Edit_Batch() {
        $('#editBathStockFormat').window('open');
        currentId = null;
        $("#editBathStockFormatForm")[0].reset();
    }
    function Save_Batch() {
        $.messager.confirm("确认", "确认提交批量修改?(若确认提交批量修改，请稍等...)", function (r) {
            if (r) {
                $('#editBathStockFormatForm').form('submit', {
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/BatchStockFormatSave/',
                    onSubmit: function () {
                        return $('#editBathStockFormatForm').form('validate');
                    },
                    success: function (r) {
                        var rVal = $.parseJSON(r);
                        if (rVal.result) {
                            $('#editBathStockFormat').window('close');
                            Query();
                        } else {
                            $.messager.alert('确认', '修改信息失败:' + rVal.message, 'error');
                        }
                    }
                });
            }
        });
    }
</script>
<script type="text/javascript">
    var releaseHeaderIds = null;
    function Edit_Choice() {
        releaseHeaderIds = "";
        var checkRows = $('#dgStockFormat').datagrid('getChecked');
        if (checkRows.length == 0) {
            $.messager.alert("提示", "请选择勾选的物料！", "info");
            return;
        }
        $.each(checkRows, function (i, item) {
            if (releaseHeaderIds != "")
                releaseHeaderIds += ",";
            releaseHeaderIds += item.id;
        });

        $('#editChoiceStockFormat').window('open');
        currentId = null;
        $("#editChoiceStockFormatForm")[0].reset();
    }
    function Save_Choice() {
        $.messager.confirm('确认', '确认修改选择的物料？', function (result) {
            if (result) {
                var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/ChoiceStockFormatSave/';
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    data: { "releaseHeaderIds": releaseHeaderIds,
                            "zmtlCategoryStatus": $('#c_zMtlCategoryStatus').val(),
                            "dmtlCategoryStatus": $('#c_dMtlCategoryStatus').val(),
                            "wmtlCategoryStatus": $('#c_wMtlCategoryStatus').val()
                    },
                    success: function (r) {
                        if (r.result) {
                            $('#editChoiceStockFormat').window('close');
                            Query();
                            releaseHeaderIds = "";
                        } else {
                            $.messager.alert('确认', '确认领料失败:' + r.message, 'error');
                        }
                    }
                });
            }
        });
    }
</script>
<script type="text/javascript">
    function updateProgress() {
        var value = $('#p').progressbar('getValue');
        if (value < 99) {
            $('#p').progressbar('setValue', value + 1);
        }
    }
    function ImportExcel() {
        $('#popImport').window('open');
    }
    function ImportOk() {
        var _selFile = $('#FormUpload input[Name=fileImport]').val();
        if (_selFile == "") {
            $.messager.alert('警告', "请选择文件！", 'warning');
            return;
        }
        $('#progressbarDiv').window({ title: '导入进度' });
        $('#progressbarDiv').window('open');
        $('#p').progressbar('setValue', 0);
        setInterval(updateProgress, 600);
        $('#FormUpload').form('submit', {
            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/MaterialImport/',
            onSubmit: function () {
                return $('#FormUpload').form('validate');
            },
            success: function (result) {
                $('#p').progressbar('setValue', 100);
                $('#progressbarDiv').window('close');
                var retValue = eval('(' + result + ')');
                $('#popImport').window('close');
                if (retValue.result) {
                    if (retValue.message != null && retValue.message != "") {
                        $.messager.alert('确认', retValue.message, '');
                    }
                    Query();
                }
                else {
                    $.messager.alert('错误', retValue.message, '');
                }
            }
        });
    }
    function ExportExcel() {
        var data = { "queryWord": "Export",
            "queryType": $('#queryType').val(),
            "queryStarDocnumber": $('#queryStarDocnumber').val(),
            "queryEndDocnumber": $('#queryEndDocnumber').val(),
            "zStatus": $('#zStatus').val(),
            "dStatus": $('#dStatus').val(),
            "wStatus": $('#wStatus').val(),
            "queryDescribe": $('#queryDescribe').val()
        };
        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/MaterialExport/';
        sUrl += "?"+$.param(data,true);
        window.location.href = sUrl;  
    }
</script>
</asp:Content>
