<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    容器盘点
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding: 0px; overflow: auto;">
        <table id="dgContainerCheck" title="容器盘点" style="" border="false" fit="true" singleselect="true"
            idfield="id" sortname="id" sortorder="asc" striped="true" toolbar="#tb1">
        </table>
    </div>
        <div id="tb1">
        <span>供应商：</span>
                &nbsp;&nbsp;<%Html.RenderPartial("~/Areas/Ascm/Views/Purchase/SupplierSelectCombo.ascx", new MideaAscm.Code.SelectCombo { width = "222px" }); %>
                <span>规格：</span>
                <input id="ecmb_spec" class="easyui-combobox" name="cmb_spec" style="width: 90px"  data-options="panelHeight:'400px',valueField:'id',textField:'spec',url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/RfidSpeac'"/>
                 <span>位置：</span>
                <input id="cmb_place" class="easyui-combobox" name="cmb_place" style="width: 140px"  data-options="panelHeight:'400px',valueField:'id',textField:'address',url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/ContainerPlace'"/>
                 <span>时间：</span>
                 <input type="text" class="easyui-numberbox" style="width: 70px" value="90" data-options="min:1,max:1000" id="txtDays" />
            <a id="Expromt" class="easyui-linkbutton" iconcls="icon-download" href="#" onclick ="ExpoerSW()"
                >导出异常容器数据</a>
         <br />
        <span>供应商名称：</span>
        <input id="userSearch" type="text" style="width: 100px;" />
        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search"
            onclick="Search();">查询</a> <a href="javascript:void(0);" class="easyui-linkbutton"
                id="btnBegin" plain="true" icon="icon-run" onclick="checkBegin()">开始盘点</a>
        <%--<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" onclick="checkSave('TEMPE');"
            id="btnTempe" style="display: none">暂存盘点</a>--%>
        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-stop"
            onclick="checkSave('END');" id="btnEnd" style="display: none">盘点结束</a>
    </div>
    <div id="ContainerDetail" class="easyui-dialog" title="详情" style="width:950px; height: 400px;"
        data-options="resizable:true,modal:true, closed: true, onBeforeClose:function(){$('#ContainerSn').val('');$('#place').combobox('clear');}">
        <table id="dgContainerDetail" title="" style="" border="false" fit="true" singleselect="true"
            idfield="sn" sortname="sn" sortorder="asc" striped="true" toolbar="#detailToolbar">
        </table>
        <div id="detailToolbar" style="padding: 5px; height: auto;">
            <span>容器编号：</span>
            <input id="ContainerSn" type="text" style="width: 100px;" />
            <span>规格：</span>
            <input id="cmb_spec" class="easyui-combobox" name="cmb_spec" style="width: 90px"  data-options="panelHeight:'400px',valueField:'id',textField:'spec',url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/RfidSpeac'"/>
            <span>位置：</span>
            <input id="place" name="place" value="" />
            <span>状态：</span>
            <select id="queryStatus" name="queryStatus"  style="width: 80px;">
                    <option value=""></option>
                    <% List<int> listStatusDefine = MideaAscm.Dal.SupplierPreparation.Entities.AscmContainer.StatusDefine.GetListII(); %>
                    <% if (listStatusDefine != null && listStatusDefine.Count > 0)
                       {
                           listStatusDefine.Remove(1); listStatusDefine.Remove(4); listStatusDefine.Remove(5); listStatusDefine.Remove(6); listStatusDefine.Remove(7);
                          %>
                    <% foreach (int statusDefine in listStatusDefine)
                       { %>
                    <option value="<%=statusDefine %>">
                        <%=MideaAscm.Dal.SupplierPreparation.Entities.AscmContainer.StatusDefine.DisplayText(statusDefine)%></option>
                    <% } %>
                    <% } %>
                </select>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search"
                onclick="DetailSearch();">查询</a>
            <br />
            <a id="btnLostSave" href="#" class="easyui-linkbutton" onclick="save('lost')">已丢失</a>
            <a id="btnFindSave" href="#" class="easyui-linkbutton" onclick="save('find')">已找回</a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/easyui/datagrid-detailview.js"></script>
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        var supplierId = null;
        var specid = null;
        var currentId = null;
        var Ischeck = false; //是否开启了盘点
        var checkLost = new Array(); //盘点的临时数据  
        var checkRow = new Object();
        $(function () {
            $('#dgContainerCheck').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/SupplierList',
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'id', title: '编号', width: 10, align: 'center', hidden: 'true' },
                    { field: 'docNumber', title: '供应商编号', width: 110, align: 'center' }
                ]],
                columns: [[
                    { field: 'name', title: '供应商全称', width: 250, align: 'left' },
                    { field: 'containerAmount', title: '容器数量', width: 70, align: 'left' },
                    { field: 'ExceptionCount', title: '异常数量', width: 70, align: 'left' },
                    { field: 'operator', title: '详情', width: 70, align: 'left', formatter: function (value, row, index) { var e = '<a href="#" onclick="UserQuery(' + row.id + ')" style="text-decoration: none;color: #800080;">详情</a>'; return e; } }

                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId = rowData.id;
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
            $('#dgContainerCheck').datagrid('reload');
        }
        function Search() {
            var queryParams = $('#dgContainerCheck').datagrid('options').queryParams;
            queryParams.queryWord = $('#userSearch').val();
            $('#dgContainerCheck').datagrid('reload');
        }
        function DetailSearch() {
            var queryParams = $('#dgContainerDetail').datagrid('options').queryParams;
            queryParams.supplierId = supplierId;
            queryParams.sn = $('#ContainerSn').val();
            queryParams.place = $('#place').combobox('getValue');
            queryParams.specid = $('#cmb_spec').combobox('getValue');
            queryParams.status = $('#queryStatus').val();
            $('#dgContainerDetail').datagrid('reload');
        }

        //详细的表格显示
        function detaiShow(id, specId) {
            supplierId = id;
            specid = specId;
            var prama = "supplierId=" + id + "";
            $.ajax({ 
                type: "GET",
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/GetContainerPlaceListBy2Id/?' + prama,
                async: true, //异步 
                timeout: 20000,
                success: function (data) {
                    var rVal = data;
                    if (rVal.rows) {
                        var feiquarr = new Array();
                        feiquarr = rVal.rows; //返回的json格式数据                         
                        if (feiquarr.length > 0) {
                            //feiquarr[0].selected = true;
                        }
                        $("#place").combobox({
                            data: feiquarr, //数据源     
                            valueField: 'id', //主键      
                            textField: 'address', //文本                           
                            onSelect: function (record) {     
                            }
                        });
                    }
                    else {
                        alert("错误： " + data.message); //显示错误信息                    
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("失败" + XMLHttpRequest.responseText);
                }
            });

            $('#dgContainerDetail').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/GetContainerListBy2Id',
                queryParams: {
                    supplierId: id,
                    specid: specId
                },
                singleSelect: true,
                checkOnSelect: false,
                selectOnCheck: false,
                striped: true,
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                columns: [[
                    { field: 'IsCheck', title: '', width: 50, align: 'center', checkbox: true },
                    { field: 'sn', title: '容器编号', width: 110, align: 'center' },
                    { field: 'rfid', title: 'EPC', width: 110, align: 'center' },
                    { field: 'spec', title: '规格', width: 110, align: 'center' },
                    { field: 'supplierName', title: '供应商名称', width: 110, align: 'center' },
                    { field: 'ReadingHeadAddress', title: '位置', width: 110, align: 'center' },
//                    { field: 'description', title: '描述', width: 200, align: 'center' },
                    { field: 'statusCn', title: '状态', width: 70, align: 'center' },
                    { field: 'isCheck', title: '是否已盘点', width: 70, align: 'center', formatter: IsChecked }

                ]],
                onClickRow: function (rowIndex, rowData) {
                    $('#dgContainerDetail').datagrid('beginEdit', rowIndex);
                }
            });
            //判断是否开始盘点
            if (!Ischeck) {
                $('#btnLostSave').hide();
                $('#btnFindSave').hide();
                $('#btnIschecked').hide();
                $('#dgContainerDetail').datagrid('hideColumn', 'IsCheck');
            }
            else {
                $('#btnLostSave').show();
                $('#btnFindSave').show();
                $('#btnIschecked').show();
                $('#dgContainerDetail').datagrid('showColumn', 'IsCheck');
            }
            $('#ContainerDetail').dialog('open');
        }
        //格式化输出是否盘点数据
        function IsChecked(value, row, index) {
            var e;
            if (value == 0) {
                e = "否";
            }
            else {
                e = "是";
            }
            return e;
        }
        function UserQuery(id, specId) {
            if (Ischeck) {
                $.messager.confirm('是否需要更改盘点信息', '如果盘点信息没有变化无需设置，</br>请点击【取消】退出，系统将设置该供应商的容器全部盘点，</br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;否则点击【确定】进入详情修改！', function (r) {
                    if (r) {
                        detaiShow(id, specId);
                    }
                    else {
                        //关窗不做任何操作，所以用户选择确定或者取消都要执行添加
                        if ($.inArray(new checkInfo("", id, "", ""), checkLost) == -1) {
                            checkLost.push(new checkInfo("", id, "", ""));
                        }
                    }

                });
            }
            else {
                detaiShow(id, specId);
            }

        }
        function checkBegin() {
            if (jQuery.isEmptyObject($('#dgContainerCheck').datagrid('getRows'))) {
                $.messager.alert('错误', '没有容器不能盘点！');
                return;
            }
            var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/CheckBegin';
            $.ajax({
                url: url,
                type: "post",
                dataType: "json",
                data: null,
                success: function (rVal) {
                    //var rVal = $.evalJSON(r);
                    if (rVal.toString() == "2") {
                        $.messager.alert('确认', '开始新的盘点');
                    } else {
                        $.messager.show({
                            title: '继续盘点',
                            msg: '继续上次未完成的盘点！',
                            showType: 'slide',
                            timeout: 6000,
                            style: {
                                right: '',
                                bottom: ''
                            }
                        });
                    }
                    Ischeck = true;
                    $('#btnBegin').linkbutton('disable');
                    $('#btnEnd').show();
                },
                error: function () {
                    Ischeck = false;
                    $.messager.alert('新建盘点失败', '失败:请重试', 'error');
                }
            });
        }
        //默认保持原有数据不变的情况下提交数据
        function defEnd() {
            $.messager.confirm('未完成盘点', '有未完成盘点的容器，如果继续提交系统将默认原来的信息不变，并完成盘点点击【确定】完成，【取消】继续盘点', function (r) {
                if (r) {
                    $.ajax({
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/EndCheck',
                        type: "post",
                        success: function (rVal) {
                            if (rVal.toString() == "1") {
                                $.messager.alert('确认', '提交信息成功!');
                                Ischeck = false;
                                checkLost = new Array();
                                checkRow = new Object();
                                Reload();
                                $('#btnBegin').linkbutton('enable');
                                $('#btnEnd').hide();
                                Reload();
                            }
                            else {
                                $.messager.alert('确认', '提交信息失败!');
                            }
                        }
                    });
                }

            });
        }

        //点击结束盘点
        function checkSave(status) {
            if (Ischeck) {
                var url = "";
                if (status == "END") {
                    url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/CheckEndSave';
                }
                else {
                    url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/CheckTempeSave';
                }
                if (!jQuery.isEmptyObject(checkLost)) {
                    checkRow["checkInfo"] = $.toJSON(checkLost);
                    $.ajax({
                        url: url,
                        type: "post",
                        dataType: "json",
                        data: checkRow,
                        success: function (rVal) {
                            if (rVal.id.toString() == "2") {
                                defEnd();
                                return;
                            }
                            if (rVal.result) {
                                $.messager.alert('确认', '提交信息成功:' + rVal.message);
                                if (status == "END") {
                                    Ischeck = false;
                                    $('#btnBegin').linkbutton('enable');
                                    $('#btnEnd').hide();
                                }
                                checkLost = new Array();
                                checkRow = new Object();
                                Reload();
                                $('#dgContainerDetail').datagrid('reload');
                            } else {
                                $.messager.alert('确认', '提交信息失败:' + rVal.message, 'error');
                            }
                        },
                        error: function (e) {
                            $.messager.alert('确认', '提交信息失败' + e, 'error');
                        }
                    });
                }
                else {
                    if (status == "END") {
                        $.messager.confirm('警告', '确定保持现有盘点数据不变化，完成盘点！', function (r) {
                            if (r) {
                                $.ajax({
                                    url: url,
                                    type: "post",
                                    dataType: "json",
                                    success: function (rVal) {
                                        if (rVal.id.toString() == "2") {
                                            defEnd();
                                            return;
                                        }
                                        if (rVal.result) {
                                            $.messager.alert('确认', '提交信息成功:' + rVal.message);
                                            if (status == "END") {
                                                Ischeck = false;
                                                $('#btnBegin').linkbutton('enable');
                                                $('#btnEnd').hide();
                                            }
                                            checkLost = new Array();
                                            checkRow = new Object();
                                            Reload();
                                        } else {
                                            $.messager.alert('确认', '提交信息失败:' + rVal.message, 'error');
                                        }
                                    },
                                    error: function (e) {
                                        $.messager.alert('确认', '提交信息失败' + e, 'error');
                                    }
                                });
                            }
                            return;
                        });
                    }
                    else {
                        $.messager.alert('确认', '没有盘点数据不能提交', 'error');
                    }
                }
            }
            else {
                $.messager.alert('确认', '没有开始盘点无法提交，请点击开始盘点后开始提交！', 'error');
            }
        }
        //简单的消息提交类
        function checkInfo(id, supplierId, status, count) {
            this.id = id;
            this.supplierId = supplierId;
            this.status = status;
            this.count = count;
        }
        //保存方法
        function save(status) {
            if (status == "lost") {
                $.each($('#dgContainerDetail').datagrid('getChecked'), function (key, val) {
                    //alert(val.status);
                    if ($.inArray(new checkInfo(val.sn, "", "FIND", ""), checkLost) != -1 || $.inArray(new checkInfo(val.sn, "", "", ""), checkLost) != -1) {
                        $.each(checkLost, function (key, chval) {
                            if (chval.sn == val.sn) {
                                chval.status = "LOST";
                            }
                        }
                    );
                    }
                    else {
                        if ($.inArray(new checkInfo(val.sn, "", "LOST", ""), checkLost) == -1) {
                            checkLost.push(new checkInfo(val.sn, "", "LOST", ""));
                        }
                    }
                });

            }
            if (status == "find") {
                $.each($('#dgContainerDetail').datagrid('getChecked'), function (key, val) {
                    //alert(val.status);
                    if ($.inArray(new checkInfo(val.sn, "", "LOST", ""), checkLost) != -1 || $.inArray(new checkInfo(val.sn, "", "", ""), checkLost) != -1) {
                        $.each(checkLost, function (key, chval) {
                            if (chval.sn == val.sn) {
                                chval.status = "FIND";
                            }
                        }
                    );
                    }
                    else {
                        if ($.inArray(new checkInfo(val.sn, "", "FIND", ""), checkLost) == -1) {
                            checkLost.push(new checkInfo(val.sn, "", "FIND", ""));
                        }
                    }
                });
            }
            checkSave("T");
            $('#dgContainerDetail').datagrid('clearSelections');
            $('#dgContainerDetail').datagrid('clearChecked');
        }
        function ExpoerSW() {
        if($('#txtDays').val()==null)
        {
        $.messager.alert('提示', '时间必填！', 'info');
        return;
        }
            var adress="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/ExportExcell/?days="+$('#txtDays').val()+"&supplierId="+$('#supplierSelect').combogrid('getValue')+"&spec="+ $('#ecmb_spec').combobox('getValue')+"&place="+$('#cmb_place').combobox('getValue')+"";
            $('#Expromt').attr('href', adress);
        }
    </script>
</asp:Content>
