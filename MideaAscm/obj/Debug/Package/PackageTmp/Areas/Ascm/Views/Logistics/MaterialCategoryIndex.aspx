<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	物料类别状态管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="west" split="true" border="false" title="" style="width: 250px; padding: 0px 2px 0px 0px;
        overflow: auto;">
        <div class="easyui-panel" title="" fit="true" border="true">
            <div class="easyui-layout" fit="true" style="" border="false">
                <div region="center" title="" border="false">
                    <table id="dgMaterialCategory" title="物料大类管理" class="easyui-datagrid" style="" border="false"
                        data-options="fit: true,
                                      rownumbers: true,
                                      singleSelect : true,
                                      idField: 'id',
                                      sortName: 'id',
                                      sortOrder: 'asc',
                                      striped: true,
                                      toolbar: '#tb1',
                                      pagination: true,
                                      pageSize: 50,
                                      loadMsg: '更新数据......',
                                      url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/MaterialCategoryList',
                                      onSelect: function(rowIndex, rowRec){
                                          currentId = rowRec.id;
                                          GetCategoryCode();
                                          LoadMaterialSubCategoryList();
                                      },
                                      onLoadSuccess: function(){
                                          $(this).datagrid('clearSelections');
                                          if (currentId) {
                                              $(this).datagrid('selectRecord', currentId);
                                          }
                                      }">
                        <thead data-options="frozen:true">
                            <tr>
                                <th data-options="field:'id',hidden:true"></th>
                                <th data-options="field:'categoryCode',width:70,align:'center'">
                                    大类编码
                                </th>
                            </tr>
                        </thead>
                        <thead>
                            <tr>
                                <th data-options="field:'description',width:100,align:'center'">
                                    物料大类描述
                                </th>
                            </tr>
                        </thead>
                    </table>
                    <div id="tb1" style="padding: 5px; height: auto;">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload"
                            onclick="MaterialCategoryReload();">刷新</a>
                        <input id="search_category" type="text" style="width: 100px;" />
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search"
                            onclick="Query_MaterialCategory();">查询</a>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div region="center" split="true" title="" border="false" style="padding: 0px 2px 0px 0px;">
        <div class="easyui-panel" fit="true" border="false" >
            <div class="easyui-layout" fit="true" style="">
                <div region="center" border="true" fit="true" style="padding: 0px 0px 0px 0px; overflow: false;">
                    <table id="dgMaterialSubCategory" title="物料子类管理" class="easyui-datagrid" style="" border="false"
                        data-options="fit: true,
                          rownumbers: true,  
                          singleSelect: false,
                          checkOnSelect: true,
                          selectOnCheck: true,
                          idField: 'id',
                          sortName: 'id',
                          sortOrder: 'asc',
                          striped: true,
                          toolbar: '#tb2',
                          pagination: true,
                          pageSize: 50,
                          loadMsg: '更新数据...',
                          <%--url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/MaterialSubCategoryList',--%>
                          onClickRow: function(rowIndex, rowRec){
                              currentId = rowRec.id;
                              GetSubCategoryCode();
                              LoadMaterialItemList();
                          },
                          onDblClickRow: function(rowIndex, rowRec){
                              <% if (ynWebRight.rightEdit){ %>
                              <%--Edit_MaterialSubCategory();--%>
                              <%} %>
                          },
                          onLoadSuccess: function(){
                              $(this).datagrid('clearSelections');
                              <%--if (currentId) {
                                  $(this).datagrid('selectRecord', currentId);
                              }--%>
                          }">
                        <thead data-options="frozen:true">
                            <tr>
                                <th data-options="checkbox:true"></th>
                                <th data-options="field:'id',hidden:true"></th>
                                <th data-options="field:'subCategoryCode',width:55,align:'center'">
                                    子类编码
                                </th>
                            </tr>
                        </thead>
                        <thead>
                            <tr>
                                <th data-options="field:'combinationCode',width:65,align:'center'">
                                    组合码
                                </th>
                                <th data-options="field:'_zMtlCategoryStatus',width:80,align:'center'">
                                    总装备料形式
                                </th>
                                <th data-options="field:'_dMtlCategoryStatus',width:80,align:'center'">
                                    电装备料形式
                                </th>
                                <th data-options="field:'_wMtlCategoryStatus',width:80,align:'center'">
                                    其他备料形式
                                </th>
                                <th data-options="field:'description',width:150,align:'center'">
                                    物料子类别描述
                                </th>
                            </tr>
                        </thead>
                    </table>
                    <div id="tb2" style="padding: 5px; height: auto;">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload"
                            onclick="LoadMaterialSubCategoryList();">刷新</a>
                        <input id="categorycode" type="text" style="width: 35px; background-color:#E8E8E8;" readonly="readonly" />
                        <span>小类段</span>
                        <input id="start_subcategory" type="text" style="width: 35px;" maxlength="3" onblur="rightPadding('#start_subcategory',3);" />-
                        <input id="end_subcategory" type="text" style="width: 35px;" maxlength="3" onblur="rightPadding('#end_subcategory',3);" />
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search"
                            onclick="Query_MaterialSubCategory();">查询</a>
                        <% if (ynWebRight.rightEdit)
                           { %>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit"
                            onclick="Edit_MaterialSubCategory();">修改</a>
                        <%} %>

                        <% if (ynWebRight.rightEdit)
                           { %>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit"
                            onclick="Edit_Choice_MaterialSubCategory();">选择修改</a>
                        <%} %>
                    </div>
                    <div id="editMaterialSubCategory" class="easyui-window" title="修改物料子类" style="padding: 10px;
                        width: 540px; height: 420px;" iconcls="icon-edit" closed="true" maximizable="false"
                        minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
                        <div class="easyui-layout" fit="true">
                            <div region="center" border="false" style="padding: 10px; background: #fff; border: 1px solid #ccc;">
                                <form id="editMaterialSubCategoryForm" method="post" style="">
                                <table style="width: 100%;" border="0" cellpadding="0" cellspacing="0">
                                    <tr style="height: 24px">
                                        <td style="width: 20%; text-align: right;" nowrap>
                                            <span>子类：</span>
                                        </td>
                                        <td style="width: 80%">
                                            <input class="easyui-validatebox" required="true" id="subCategoryCode" name="subCategoryCode" type="text"
                                                style="width: 140px;" style="width: 108px; background-color: #CCCCCC;" readonly="readonly" /><span style="color: Red;">*</span>
                                        </td>
                                    </tr>
                                    <tr style="height: 24px">
                                        <td style="width: 20%; text-align: right;" nowrap>
                                            <span>组合码：</span>
                                        </td>
                                        <td style="width: 80%">
                                            <input class="easyui-validatebox" required="true" id="combinationCode" name="combinationCode" type="text"
                                                style="width: 140px;" style="width: 108px; background-color: #CCCCCC;" readonly="readonly" /><span style="color: Red;">*</span>
                                        </td>
                                    </tr>
                                    <tr style="height: 24px">
                                        <td style="text-align: right;" nowrap>
                                            <span>总装备料形式：</span>
                                        </td>
                                        <td>
                                            <select id="zMtlCategoryStatus" name="zMtlCategoryStatus" style="width: 145px;" class="easyui-combobox" data-options="panelHeight:'auto'">
                                                <% List<string> listMtlCategoryStatusDefine = MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.GetList(); %>
                                                <% if (listMtlCategoryStatusDefine != null && listMtlCategoryStatusDefine.Count > 0)
                                                   { %>
                                                <% foreach (string CategoryStatusDefine in listMtlCategoryStatusDefine)
                                                   { %>
                                                <option value="<%=CategoryStatusDefine %>"><%=MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.DisplayText(CategoryStatusDefine)%></option>
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
                                            <select id="dMtlCategoryStatus" name="dMtlCategoryStatus" style="width: 145px;" class="easyui-combobox" data-options="panelHeight:'auto'">
                                                <% listMtlCategoryStatusDefine = MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.GetList(); %>
                                                <% if (listMtlCategoryStatusDefine != null && listMtlCategoryStatusDefine.Count > 0)
                                                   { %>
                                                <% foreach (string CategoryStatusDefine in listMtlCategoryStatusDefine)
                                                   { %>
                                                <option value="<%=CategoryStatusDefine %>"><%=MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.DisplayText(CategoryStatusDefine)%></option>
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
                                            <select id="wMtlCategoryStatus" name="wMtlCategoryStatus" style="width: 145px;" class="easyui-combobox" data-options="panelHeight:'auto'">                                            <% listMtlCategoryStatusDefine = MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.GetList(); %>
                                                <% if (listMtlCategoryStatusDefine != null && listMtlCategoryStatusDefine.Count > 0)
                                                   { %>
                                                <% foreach (string CategoryStatusDefine in listMtlCategoryStatusDefine)
                                                   { %>
                                                <option value="<%=CategoryStatusDefine %>"><%=MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.DisplayText(CategoryStatusDefine)%></option>
                                                <% } %>
                                                <% } %>
                                            </select>
                                        </td>
                                    </tr>
                                    <tr style="height: 24px">
                                        <td style="text-align: right;" nowrap>
                                            <span>备注：</span>
                                        </td>
                                        <td>
                                            <textarea class="easyui-validatebox" id="tip" name="tip" rows="3" cols="342" 
                                                style="width:300px;"></textarea>
                                        </td>
                                    </tr>
                                </table>
                                </form>
                            </div>
                            <div region="south" border="false" style="text-align: right; height: 30px; line-height: 30px;">
                                <% if (ynWebRight.rightEdit)
                                   { %>
                                <a id="btnSave" class="easyui-linkbutton" iconcls="icon-ok" href="javascript:void(0)"
                                    onclick="Save_MaterialSubCategory()">保存</a>
                                <%} %>
                                <a class="easyui-linkbutton" iconcls="icon-cancel" href="javascript:void(0)" onclick="$('#editMaterialSubCategory').window('close');">
                                    取消</a>
                            </div>
                        </div>
                    </div>
                    <div id="editChoiceSubCategory" class="easyui-window" title="批量修改物料子类" style="padding: 10px;
                        width: 540px; height: 420px;" iconcls="icon-edit" closed="true" maximizable="false"
                        minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
                        <div class="easyui-layout" fit="true">
                            <div region="center" border="false" style="padding: 10px; background: #fff; border: 1px solid #ccc;">
                                <form id="editChoiceSubCategoryForm" method="post" style="">
                                <table style="width: 100%;" border="0" cellpadding="0" cellspacing="0">
                                    <tr style="height: 24px">
                                        <td style="text-align: right;" nowrap>
                                            <span>总装备料形式：</span>
                                        </td>
                                        <td>
                                            <select id="cZMtlCategoryStatus" name="zMtlCategoryStatus" style="width: 145px;" class="easyui-combobox" data-options="panelHeight:'auto'">
                                                <option value="kz">空值</option>
                                                <% listMtlCategoryStatusDefine = MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.GetList(); %>
                                                <% if (listMtlCategoryStatusDefine != null && listMtlCategoryStatusDefine.Count > 0)
                                                   { %>
                                                <% foreach (string CategoryStatusDefine in listMtlCategoryStatusDefine)
                                                   { %>
                                                <option value="<%=CategoryStatusDefine %>"><%=MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.DisplayText(CategoryStatusDefine)%></option>
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
                                            <select id="cDMtlCategoryStatus" name="dMtlCategoryStatus" style="width: 145px;" class="easyui-combobox" data-options="panelHeight:'auto'">
                                                <option value="kz">空值</option>
                                                <% listMtlCategoryStatusDefine = MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.GetList(); %>
                                                <% if (listMtlCategoryStatusDefine != null && listMtlCategoryStatusDefine.Count > 0)
                                                   { %>
                                                <% foreach (string CategoryStatusDefine in listMtlCategoryStatusDefine)
                                                   { %>
                                                <option value="<%=CategoryStatusDefine %>"><%=MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.DisplayText(CategoryStatusDefine)%></option>
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
                                            <select id="cWMtlCategoryStatus" name="wMtlCategoryStatus" style="width: 145px;" class="easyui-combobox" data-options="panelHeight:'auto'">
                                                <option value="kz">空值</option>
                                                <% listMtlCategoryStatusDefine = MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.GetList(); %>
                                                <% if (listMtlCategoryStatusDefine != null && listMtlCategoryStatusDefine.Count > 0)
                                                   { %>
                                                <% foreach (string CategoryStatusDefine in listMtlCategoryStatusDefine)
                                                   { %>
                                                <option value="<%=CategoryStatusDefine %>"><%=MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.DisplayText(CategoryStatusDefine)%></option>
                                                <% } %>
                                                <% } %>
                                            </select>
                                        </td>
                                    </tr>
                                </table>
                                </form>
                            </div>
                            <div region="south" border="false" style="text-align: right; height: 30px; line-height: 30px;">
                                <% if (ynWebRight.rightEdit)
                                   { %>
                                <a id="A2" class="easyui-linkbutton" iconcls="icon-ok" href="javascript:void(0)"
                                    onclick="Save_Choice_MaterialSubCategory()">保存</a>
                                <%} %>
                                <a class="easyui-linkbutton" iconcls="icon-cancel" href="javascript:void(0)" onclick="$('#editChoiceSubCategory').window('close');">
                                    取消</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div region="east" title="" split="true" border="false" style="width: 580px; padding: 0px 0px 0px 0px;
        overflow: auto;">
        <div class="easyui-panel" fit="true" border="false" >
            <div class="easyui-layout" fit="true" style="">
                <div region="center" border="true" fit="true" style="padding: 0px 0px 0px 0px; overflow: false;">
                    <table id="dgMaterialItem" title="物料管理" class="easyui-datagrid" style="" border="false"
                        data-options="fit: true,
                          rownumbers: true,  
                          singleSelect: true,
                          idField: 'id',
                          striped: true,
                          toolbar: '#tb3',
                          pagination: true,
                          pageSize: 200,
                          pageList:[50,100,150,200,250],
                          loadMsg: '更新数据...',
                          <%--url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/MaterialItemList',--%>
                          onSelect: function(rowIndex, rowRec){
                              currentId = rowRec.id;
                          },
                          onDblClickRow: function(rowIndex, rowRec){
                              <% if (ynWebRight.rightEdit){ %>
                              Edit_MaterialItem();
                              <%} %>
                          },
                          onLoadSuccess: function(){
                              $(this).datagrid('clearSelections');
                              <%--if (currentId) {
                                  $(this).datagrid('selectRecord', currentId);
                              }  --%>                   
                          }">
                        <thead data-options="frozen:true">
                            <tr>
                                <th data-options="field:'id',hidden:true"></th>
                                <th data-options="field:'docNumber',width:100,align:'center'">
                                    物料编码
                                </th>
                            </tr>
                        </thead>
                        <thead>
                            <tr>
                                <th data-options="field:'wipSupplyTypeCn',width:60,align:'center'">
                                    供应类型
                                </th>
                                <th data-options="field:'_zMtlCategoryStatus',width:80,align:'center'">
                                    总装备料形式
                                </th>
                                <th data-options="field:'_dMtlCategoryStatus',width:80,align:'center'">
                                    电装备料形式
                                </th>
                                <th data-options="field:'_wMtlCategoryStatus',width:80,align:'center'">
                                    其他备料形式
                                </th>
                                <th data-options="field:'description',width:400,align:'left'">
                                    描述
                                </th>
                            </tr>
                        </thead>
                    </table>
                    <div id="tb3" style="padding: 5px; height: auto;">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload"
                            onclick="LoadMaterialItemList();">刷新</a>
                        <input id="comcode" type="text" style="width: 60px; background-color:#E8E8E8;" readonly="readonly" />
                        <span>编码段：</span>
                        <input id="start_info" type="text" maxlength="5" style="width: 70px;" onblur="rightPadding('#start_info',5);" />-
                        <input id="end_info" type="text" maxlength="5" style="width: 70px;" onblur="rightPadding('#end_info',5);" />
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search"
                            onclick="Query_MaterialItem();">查询</a>
                        <% if (ynWebRight.rightEdit)
                           { %>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit"
                            onclick="Edit_MaterialItem();">修改</a>
                        <%} %>
                    </div>
                    <div id="editMaterialItem" class="easyui-window" title="物料管理" style="padding: 10px;
                        width: 540px; height: 420px;" iconcls="icon-edit" closed="true" maximizable="false"
                        minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
                        <div class="easyui-layout" fit="true">
                            <div region="center" border="false" style="padding: 10px; background: #fff; border: 1px solid #ccc;">
                                <form id="editMaterialItemForm" method="post" style="">
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
                                            <select id="sZMtlCategoryStatus" name="zMtlCategoryStatus" style="width: 145px;" class="easyui-combobox" data-options="panelHeight:'auto'">
                                                <% listMtlCategoryStatusDefine = MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.GetList(); %>
                                                <% if (listMtlCategoryStatusDefine != null && listMtlCategoryStatusDefine.Count > 0)
                                                   { %>
                                                <% foreach (string CategoryStatusDefine in listMtlCategoryStatusDefine)
                                                   { %>
                                                <option value="<%=CategoryStatusDefine %>"><%=MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.DisplayText(CategoryStatusDefine)%></option>
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
                                            <select id="sDMtlCategoryStatus" name="dMtlCategoryStatus" style="width: 145px;" class="easyui-combobox" data-options="panelHeight:'auto'">
                                                <% listMtlCategoryStatusDefine = MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.GetList(); %>
                                                <% if (listMtlCategoryStatusDefine != null && listMtlCategoryStatusDefine.Count > 0)
                                                   { %>
                                                <% foreach (string CategoryStatusDefine in listMtlCategoryStatusDefine)
                                                   { %>
                                                <option value="<%=CategoryStatusDefine %>"><%=MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.DisplayText(CategoryStatusDefine)%></option>
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
                                            <select id="sWMtlCategoryStatus" name="wMtlCategoryStatus" style="width: 145px;" class="easyui-combobox" data-options="panelHeight:'auto'">
                                                <% listMtlCategoryStatusDefine = MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.GetList(); %>
                                                <% if (listMtlCategoryStatusDefine != null && listMtlCategoryStatusDefine.Count > 0)
                                                   { %>
                                                <% foreach (string CategoryStatusDefine in listMtlCategoryStatusDefine)
                                                   { %>
                                                <option value="<%=CategoryStatusDefine %>"><%=MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.DisplayText(CategoryStatusDefine)%></option>
                                                <% } %>
                                                <% } %>
                                            </select>
                                        </td>
                                    </tr>
                                </table>
                                </form>
                            </div>
                            <div region="south" border="false" style="text-align: right; height: 30px; line-height: 30px;">
                                <% if (ynWebRight.rightEdit)
                                   { %>
                                <a id="A1" class="easyui-linkbutton" iconcls="icon-ok" href="javascript:void(0)"
                                    onclick="Save_MaterialItem()">保存</a>
                                <%} %>
                                <a class="easyui-linkbutton" iconcls="icon-cancel" href="javascript:void(0)" onclick="$('#editMaterialItem').window('close');">
                                    取消</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
<script type="text/javascript">
    var currentId = null;
    //物料大类操作
    function Query_MaterialCategory() {
        var options = $('#dgMaterialCategory').datagrid('options');
        options.queryParams.queryWord = $('#search_category').val();
        $('#dgMaterialCategory').datagrid('reload');
    }
    function MaterialCategoryReload() {
        $('#dgMaterialCategory').datagrid('reload');
    }
</script>
<script type="text/javascript">
    //物料子类操作
    function Query_MaterialSubCategory() {
        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/MaterialSubCategoryList/';
        var options = $('#dgMaterialSubCategory').datagrid('options');
        options.url = sUrl;
        options.queryParams.queryWord = $('#start_subcategory').val();
        options.queryParams.queryOtherWord = $('#end_subcategory').val();
        options.queryParams.categoryCode = $('#categorycode').val();

        $('#dgMaterialSubCategory').datagrid('reload');
    }
    function MaterialSubCategoryReload() {
        $('#dgMaterialSubCategory').datagrid('reload');
    }
    function Edit_MaterialSubCategory() {
        var selectRow = $('#dgMaterialSubCategory').datagrid('getSelected');
        if (selectRow) {
            $('#editMaterialSubCategory').window('open');
            var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/MaterialSubCategoryEdit/' + selectRow.id;
            $.ajax({
                url: sUrl,
                type: "post",
                dataType: "json",
                success: function (r) {
                    if (r) {
                        $("#editMaterialSubCategoryForm")[0].reset();

                        $('#subCategoryCode').val(r.subCategoryCode);
                        $('#combinationCode').val(r.combinationCode);
                        $('#zMtlCategoryStatus').combobox('setValue', r.zMtlCategoryStatus);
                        $('#dMtlCategoryStatus').combobox('setValue', r.dMtlCategoryStatus);
                        $('#wMtlCategoryStatus').combobox('setValue', r.wMtlCategoryStatus);
                        $('#tip').val(r.tip);
                    }
                }
            });
            currentId = selectRow.id;
        } else {
            $.messager.alert('提示', '请选择物料子类信息', 'info');
        }
    }
    function Save_MaterialSubCategory() {
        $.messager.confirm("确认", "确认提交保存?", function (r) {
            if (r) {
                $('#editMaterialSubCategoryForm').form('submit', {
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/MaterialSubCategorySave/' + currentId,
                    onSubmit: function () {
                        return $('#editMaterialSubCategoryForm').form('validate');
                    },
                    success: function (r) {
                        var rVal = $.parseJSON(r);
                        if (rVal.result) {
                            $('#editMaterialSubCategory').window('close');
                            currentId = rVal.id;
                            Query_MaterialSubCategory();
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
    //物料操作
    function Query_MaterialItem() {
        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/MaterialItemList/';
        var options = $('#dgMaterialItem').datagrid('options');
        options.url = sUrl;
        options.queryParams.queryWord = $('#start_info').val();
        options.queryParams.queryOtherWord = $('#end_info').val();
        options.queryParams.comCode = $('#comcode').val();

        $('#dgMaterialItem').datagrid('reload');
    }
    function MaterialItemReload() {
        $('#dgMaterialItem').datagrid('reload');
    }
    function Edit_MaterialItem() {
        var selectRow = $('#dgMaterialItem').datagrid('getSelected');
        if (selectRow) {
            $('#editMaterialItem').window('open');
            var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/MaterialItemEdit/' + selectRow.id;
            $.ajax({
                url: sUrl,
                type: "post",
                dataType: "json",
                success: function (r) {
                    if (r) {
                        $("#editMaterialItemForm")[0].reset();

                        $('#docNumber').val(r.docNumber);
                        $('#sZMtlCategoryStatus').combobox('setValue', r.zMtlCategoryStatus);
                        $('#sDMtlCategoryStatus').combobox('setValue', r.dMtlCategoryStatus);
                        $('#sWMtlCategoryStatus').combobox('setValue', r.wMtlCategoryStatus);
                    }
                }
            });
            currentId = selectRow.id;
        } else {
            $.messager.alert('提示', '请选择物料子类信息', 'info');
        }
    }
    function Save_MaterialItem() {
        $.messager.confirm("确认", "确认提交保存?", function (r) {
            if (r) {
                $('#editMaterialItemForm').form('submit', {
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/MaterialItemSave/' + currentId,
                    onSubmit: function () {
                        return $('#editMaterialItemForm').form('validate');
                    },
                    success: function (r) {
                        var rVal = $.parseJSON(r);
                        if (rVal.result) {
                            $('#editMaterialItem').window('close');
                            currentId = rVal.id;
                            Query_MaterialItem();
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
    function LoadMaterialSubCategoryList() {
        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/LoadMaterialSubCategoryList/';
        var options = $('#dgMaterialSubCategory').datagrid('options');
        options.url = sUrl;
        options.queryParams.categoryId = currentId;
        $('#dgMaterialSubCategory').datagrid('reload');
    }
    function LoadMaterialItemList() {
        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/LoadMaterialItemList/';
        var options = $('#dgMaterialItem').datagrid('options');
        options.url = sUrl;
        options.queryParams.subCategoryId = currentId;
        $('#dgMaterialItem').datagrid('reload');
    }
</script>
<script type="text/javascript">
    function GetCategoryCode() {
        var selectRow = $('#dgMaterialCategory').datagrid('getSelected');
        if (selectRow) {
            var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/MaterialCategoryEdit/' + selectRow.id;
            $.ajax({
                url: sUrl,
                type: "post",
                dataType: "json",
                success: function (r) {
                    if (r) {
                        $("#categorycode").val();

                        $("#categorycode").val(r.categoryCode);
                    }
                }
            });
            currentId = selectRow.id;
        } 
//        else {
//            $.messager.alert('提示', '请选择物料大类', 'info');
//        }
    }
    function GetSubCategoryCode() {
        var selectRow = $('#dgMaterialSubCategory').datagrid('getSelected');
        if (selectRow) {
            var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/MaterialSubCategoryEdit/' + selectRow.id;
            $.ajax({
                url: sUrl,
                type: "post",
                dataType: "json",
                success: function (r) {
                    if (r) {
                        $('#comcode').val();

                        $('#comcode').val(r.combinationCode);
                    }
                }
            });
            currentId = selectRow.id;
        } 
//        else {
//            $.messager.alert('提示', '请选择物料子类信息', 'info');
//        }
    }
</script>
<script type="text/javascript">
    function rightPadding(obj, length) {
        var temp = $(obj).val();
        var nI = length;
        if (temp != "" && temp != null) {
            if (temp.length < nI) {
                var str = "";
                for (var i = 0; i < nI - temp.length; i++) {
                    str += "0";
                }
                temp += str;
            }
        }
        $(obj).val(temp);
    }
</script>
<script type="text/javascript">
    var releaseHeaderIds = null;
    function Edit_Choice_MaterialSubCategory() {
        releaseHeaderIds = "";
        var checkRows = $('#dgMaterialSubCategory').datagrid('getChecked');
        if (checkRows.length == 0) {
            $.messager.alert("提示", "请选择勾选的物料！", "info");
            return;
        }
        $.each(checkRows, function (i, item) {
            if (releaseHeaderIds != "")
                releaseHeaderIds += ",";
            releaseHeaderIds += item.id;
        });

        $('#editChoiceSubCategory').window('open');
        currentId = null;
        $("#editChoiceSubCategoryForm")[0].reset();
    }
    function Save_Choice_MaterialSubCategory() {
        $.messager.confirm('确认', '确认修改选择的物料子类？', function (result) {
            if (result) {
                var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/ChoiceMaterialSubCategorySave/';
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    data: { "releaseHeaderIds": releaseHeaderIds,
                        "zmtlCategoryStatus": $('#cZMtlCategoryStatus').combobox('getValue'),
                        "dmtlCategoryStatus": $('#cDMtlCategoryStatus').combobox('getValue'),
                        "wmtlCategoryStatus": $('#cWMtlCategoryStatus').combobox('getValue')
                    },
                    success: function (r) {
                        if (r.result) {
                            $('#editChoiceSubCategory').window('close');
                            Query_MaterialSubCategory();
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
</asp:Content>
