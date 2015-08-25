<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    系统作业
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="north" title="" border="false" style="height:250px; padding:0px 0px 2px 0px; overflow:hidden;">
        <div class="easyui-panel" title="" fit="true" border="true" style="overflow:hidden;">
            <div class="easyui-layout" fit="true">
                <div region="north" title="" border="false" style="height:30px;overflow:hidden;">
                    <div class="div_toolbar" style="float:left; height:26px; width:100%; border-bottom:1px solid #99BBE8; padding:1px;">
                        <input id="search" type="text" style="width: 100px;" />
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="query()">查询</a>
                        <% if (ynWebRight.rightAdd){ %>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-run" onclick="run()">运行</a>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-stop" onclick="stop()">停止</a>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="add()">增加</a>
                        <%} %>
                        <% if (ynWebRight.rightDelete){ %>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="drop()">删除</a>
                        <%} %>
	                </div>
                </div>
                <div region="center" border="false" style="padding:0px;overflow:hidden;">
                    <table id="dgJob" title="系统作业" class="easyui-datagrid" style="" border="false" 
                          data-options="rownumbers: true,
                                        noheader: true,
                                        fit: true,
                                        singleSelect: true,
                                        striped: true,
                                        idField: 'jobName',
                                        sortName: '',
                                        sortOrder: '',
                                        pagination: true,
                                        pageSize: 10,
                                        loadMsg: '更新数据...',
                                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/JobList',
                                        onSelect: function(rowIndex, rowRec){
                                            currentId = rowRec.jobName;
                                            loadJob();
                                        },
                                        onDblClickRow: function(rowIndex, rowRec){

                                        },
                                        onLoadSuccess: function(){
                                            $(this).datagrid('clearSelections');
                                            if (currentId) {
                                                $(this).datagrid('selectRecord', currentId);
                                            }                     
                                        }
                                        ">
                        <thead data-options="frozen:true">
                            <tr>
                                <th data-options="field:'jobName',width:160,align:'left'">名称</th>
                                <th data-options="field:'comments',width:120,align:'left'">描述</th>
                            </tr>
                        </thead>
                        <thead>
                            <tr>
                                <th data-options="field:'jobCreator',width:60,align:'center',hidden:true">创建者</th>
                                <th data-options="field:'jobTypeCn',width:70,align:'center',hidden:true">类型</th>
                                <th data-options="field:'jobAction',width:230,align:'left'">动作</th>
                                <th data-options="field:'stateCn',width:80,align:'center'">状态</th>
                                <th data-options="field:'numberOfArguments',width:70,align:'center'">参数数量</th>
                                <th data-options="field:'_startDate',width:120,align:'left'">开始时间</th>
                                <th data-options="field:'enabledCn',width:60,align:'center'">启用</th>
                                <th data-options="field:'runCount',width:70,align:'center'">运行次数</th>
                                <th data-options="field:'failureCount',width:70,align:'center'">失败次数</th>
                                <th data-options="field:'_lastStartDate',width:120,align:'left'">上次执行时间</th>
                                <th data-options="field:'_nextRunDate',width:120,align:'left'">下次执行时间</th>
                                <th data-options="field:'autoDrop',width:50,align:'center',formatter:jobLogFormat">日志</th>
                            </tr>
                        </thead>
                    </table>
                    <!--日志查询-->
                    <div id="wJobLog" class="easyui-window" title="日志查询" style="padding: 5px;width:640px;height:480px;"
                        data-options="iconCls: 'icon-search',
                                      closed: true,
                                      maximizable: false,
                                      minimizable: false,
                                      resizable: false,
                                      collapsible: false,
                                      modal: true,
                                      shadow: true">
                        <div class="easyui-layout" fit="true">
                            <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
                                <table id="dgJobLog" class="easyui-datagrid" title="作业日志" style="" border="false" 
                                    data-options="rownumbers: true,
                                                  noheader: true,
                                                  fit: true,
                                                  singleSelect: true,
                                                  idField: 'logId',
                                                  sortName: '',
                                                  sortOrder: '',
                                                  striped: true,
                                                  pagination: true,
                                                  pageSize: 30,
                                                  toolbar: '#tbJobLog',
                                                  loadMsg: '加载数据...'">
                                    <thead>
                                        <tr>
                                            <th data-options="field:'logId',hidden:true"></th>
                                            <th data-options="field:'_logDate',width:120,align:'left'">日期</th>
                                            <th data-options="field:'operationCn',width:80,align:'center'">操作</th>
                                            <th data-options="field:'statusCn',width:100,align:'center'">状态</th>
                                        </tr>
                                    </thead>
		                        </table>
                                <div id="tbJobLog">
                                    <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="jobLogReload();">刷新</a>
                                    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#wJobLog').window('close');">关闭</a> 
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div region="center" title="" border="false" style="padding:0px 0px 0px 0px;">
        <form id="editJobForm" method="post" style="height:100%;width:100%;">
        <div id="jobsEditTabs" class="easyui-tabs" fit="true" data-options="border:true,tools:'#tab-tools'">
            <div title="作业属性" id="jobPropertiesTab" style="padding:5px;overflow:auto;background:#fafafa;">
                <div class="easyui-panel" title="" fit="true" border="false" style="overflow:hidden;">
                    <div class="easyui-layout" fit="true">
                        <div region="west" title="" border="false" style="width:320px;overflow:hidden;">
                            <div class="fitem">
                                <label>名称：</label>
                                <input id="jobName" name="jobName" /><span style="color:red;">*</span>
                            </div>
                            <div class="fitem">
                                <label>描述：</label>
                                <input id="comments" name="comments"/>
                            </div>
                            <div class="fitem">
                                <label>启用：</label>
                                <input type="checkbox" id="enabledYes" name="enabled" value="TRUE"/>是
                                <input type="checkbox" id="enabledNo" name="enabled" value="FALSE"/>否
                            </div>
                            <div class="fitem">
                                <label>自动删除：</label>
                                <input type="checkbox" id="autoDropYes" name="autoDrop" value="TRUE"/>是
                                <input type="checkbox" id="autoDropNo" name="autoDrop" value="FALSE"/>否
                            </div>
                            <div class="fitem">
                                <label>开始时间：</label>
                                <input id="startDate" name="startDate" class="easyui-datetimebox" data-options="showSeconds:true" style="width:150px"/>
                            </div>
                        </div>
                        <div region="center" border="false" style="padding:0px;overflow:hidden;">
                            <div class="fitem">
                                <label>重复执行：</label>
                                <input type="checkbox" id="none" name="freq" value=""/>不重复
                                <input type="checkbox" id="secondly" name="freq" value="SECONDLY"/>秒
                                <input type="checkbox" id="minutely" name="freq" value="MINUTELY"/>分
                                <input type="checkbox" id="hourly" name="freq" value="HOURLY"/>时
                                <input type="checkbox" id="daily" name="freq" value="DAILY"/>天
                                <input type="checkbox" id="weekly" name="freq" value="WEEKLY"/>周
                                <input type="checkbox" id="monthly" name="freq" value="MONTHLY"/>月
                            </div>
                            <div class="fitem">
                                <div class="subfitem"></div>
                            </div>
                        </div>   
                    </div>
                </div>
            </div>
            <div title="存储过程" id="procedureTab" style="padding:5px;overflow:hidden;background:#fafafa;">
                <div class="easyui-panel" title="" fit="true" border="false" style="overflow:hidden;">
                    <div class="easyui-layout" fit="true">
                        <div region="north" title="" border="false" style="height:30px;overflow:hidden;">
                            <label>选择存储过程：</label>
                            <select id="jobAction" name="jobAction" class="easyui-combogrid" style="width:480px;"
                                data-options="panelWidth: 500,
                                              rownumbers: true,
                                              idField: '_procedureName',
                                              textField: '_procedureName',
                                              mode: 'remote',
                                              fitColumns: true,
                                              delay: 500,
                                              url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/ProcedureList',
                                              columns: [[
                                                { field:'id',hidden:true },
                                                { field:'_procedureName',title:'过程名',width:460,align:'left' },
                                                { field:'objectName',hidden:true },
                                                { field:'procedureName',hidden:true },
                                                { field:'objectType',hidden:true }
                                              ]],
                                              onChange: function(newValue, oldValue){
                                                  if (newValue) {
                                                      var procedureRow = $(this).combogrid('grid').datagrid('getSelected');
                                                      if (procedureRow) {
                                                          var jobRow = $('#dgJob').datagrid('getSelected');
                                                          var options = $('#dgProcedureArgument').datagrid('options');
                                                          options.queryParams.jobName = currentId;
                                                          options.queryParams.procedureJson = $.toJSON(procedureRow);
                                                          if (jobRow)
                                                              options.queryParams.jobAction = jobRow.jobAction;
                                                          options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/ProcedureArgumentList';    
                                                          $('#dgProcedureArgument').datagrid('reload');
                                                      } 
                                                  }
                                              }">
                            </select>
                        </div> 
                        <div region="center" border="false" style="padding:0px;overflow:auto;">
                            <table id="dgProcedureArgument" title="过程参数" class="easyui-datagrid" style="overflow:auto;" border="true" 
                                data-options="rownumbers: true,
                                              noheader: true,
                                              fit: true,
                                              singleSelect: true,
                                              striped: true,
                                              logMsg: '加载中...',
                                              onClickRow: clickProcedureArgumentRow,
                                              onLoadSuccess: function(){
                                                  editIndex = undefined; 
                                              }">
                                <thead>
                                    <tr>
                                        <th data-options="field:'position',hidden:true"></th>
                                        <th data-options="field:'argumentName',width:160,align:'left'">参数名</th>
                                        <th data-options="field:'dataType',width:180,align:'left'">数据类型</th>
                                        <th data-options="field:'value',width:230,align:'left',editor:'text'">值</th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-tools">
            <% if (ynWebRight.rightEdit){ %>           
		    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-save'" onclick="save()">保存</a>
		    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-cancel'" onclick="cancel()">取消</a>
            <%} %>
	    </div>
        </form>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <style type="text/css">
        .fitem{ margin-bottom:5px; }
        .fitem label{ display:inline-block;width:100px; }
        .fitem div.subfitem{ padding:5px; }
        .fitem div.subfitem div{ padding-bottom:5px; }  
    </style>
    <%-- init --%>
    <script type="text/javascript">
        var currentId = null;
        var preEnabled = null;
        var preAutoDrop = null;
        var preFreq = null;
        $(function () {
            $("[name='enabled']").each(function () {
                $(this).click(function () {
                    if ($(this).is(':checked')) {
                        if (preEnabled != null) {
                            preEnabled.removeAttr("checked"); 
                        }
                        preEnabled = $(this);
                        preEnabled.attr("checked", "true");
                    }
                })
            })
            $("[name='autoDrop']").each(function () {
                $(this).click(function () {
                    if ($(this).is(':checked')) {
                        if (preAutoDrop != null) {
                            preAutoDrop.removeAttr("checked");
                        }
                        preAutoDrop = $(this);
                        preAutoDrop.attr("checked", "true");
                    }
                })
            })
            $("[name='freq']").each(function () {
                $(this).click(function () {
                    $('.subfitem').empty();
                    if ($(this).is(':checked')) {
                        displayRepeatInterval($(this).val());
                        if (preFreq != null) {
                            preFreq.removeAttr("checked"); 
                        }
                        preFreq = $(this);
                        preFreq.attr("checked", "true");
                    }
                })
            })
        })
        function jobLogFormat(value, row, index) {
            var imgUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/images/log.png';
            return "<a href='javascript:void(0)' onclick='loadJobLog(\""+row.jobName+"\");'><img src='" + imgUrl + "' alt='' /></a>";
        }
        function loadJobLog(jobName) {
            $('#wJobLog').window('open');
            var options = $('#dgJobLog').datagrid('options');
            options.queryParams.jobName = jobName;
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/LoadJobLog';
            $('#dgJobLog').datagrid('reload');
        }
    </script>
    <%-- business --%>
    <script type="text/javascript">
        function query() {
            var queryParams = $('#dgJob').datagrid('options').queryParams;
            queryParams.queryWord = $('#search').val();
            $('#dgJob').datagrid('reload');
        }
        function run() {
            if (currentId == null) {
                $.messager.alert('提示', '请选择作业', 'info');
                return;
            }
            $.ajax({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/RunJob/',
                cache: false,
                dataType: 'json',
                data: { "jobName": decodeURIComponent(currentId) },
                success: function (r) {
                    if (r.result) {
                        query();
                        $.messager.alert('确认', r.message, 'info');
                    } else {
                        $.messager.alert('错误', '运行失败:' + r.message, 'error');
                    }
                }
            })
        }
        function stop() {
            if (currentId == null) {
                $.messager.alert('提示', '请选择作业', 'info');
                return;
            }
            $.ajax({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/StopJob/',
                cache: false,
                dataType: 'json',
                data: { "jobName": decodeURIComponent(currentId) },
                success: function (r) {
                    if (r.result) {
                        query();
                        $.messager.alert('确认', r.message, 'info');
                    } else {
                        $.messager.alert('错误', '停止失败:' + r.message, 'error');
                    }
                }
            })
        }
        function add() {
            currentId = null;
            $('#jobName').attr('readonly', false);
            $('#dgJob').datagrid('clearSelections');
            clearJob();
        }
        function drop() {
            if (currentId == null) {
                $.messager.alert('提示', '请选择作业', 'info');
                return;
            }
            $.ajax({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/DropJob/',
                cache: false,
                dataType: 'json',
                data: { "jobName": decodeURIComponent(currentId) },
                success: function (r) {
                    if (r.result) {
                        query();
                        clearJob();
                        $.messager.alert('确认', r.message, 'info');
                    } else {
                        $.messager.alert('错误', '删除失败:' + r.message, 'error');
                    }
                }
            })
        }
        function loadJob() {
            if (currentId == null) {
                $.messager.alert('提示', '请选择作业', 'info');
                return;
            }
            $.ajax({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/LoadJob/',
                cache: false,
                dataType: 'json',
                data: { "jobName": currentId },
                success: function (r) {
                    if (r.result) {
                        clearJob();
                        if (r.entity)
                            displayJob(r.entity);
                    } else {
                        $.messager.alert('错误', '加载失败:' + r.message, 'error');
                    }
                }
            })
        }
        function clearJob() {
            $('.subfitem').empty();
            $('#dgProcedureArgument').datagrid('load', []);
            $('#editJobForm').form('clear');
        }
        function displayJob(r) {
            $('#jobName').val(r.jobName);
            $('#jobName').attr('readonly', true);
            $('#comments').val(r.comments);
            $("[name='enabled']").each(function () {
                if ($(this).val() == r.enabled) {
                    if (preEnabled != null) {
                        preEnabled.removeAttr("checked");
                    }
                    $(this).attr('checked', 'true');
                    preEnabled = $(this);
                    preEnabled.attr("checked", "true");
                }
            })
            $("[name='autoDrop']").each(function () {
                if ($(this).val() == r.autoDrop) {
                    if (preAutoDrop != null) {
                        preAutoDrop.removeAttr("checked");
                    }
                    $(this).attr('checked', 'true');
                    preAutoDrop = $(this);
                    preAutoDrop.attr("checked", "true");
                }
            })
            $('#startDate').datetimebox('setValue', r.startDate);
            if (r.repeatInterval && r.repeatInterval.length > 0) {
                repeatInterval.init(r.repeatInterval);
                repeatInterval.display();
            }
            if (r.jobAction) {
                $('#jobAction').combogrid('setValue', r.jobAction);
                //$('#jobAction').combogrid('grid').datagrid('reload', { '_procedureName': r.jobAction });
                //setTimeout(function () { $('#jobAction').combogrid('setValue', r.jobAction); }, 100);
            }
        }
        function save() {
            var data = {};
            acceptProcedureArgument();
            var procedureArgument = $('#dgProcedureArgument').datagrid('getData');
            if (procedureArgument.rows.length > 0) {
                $.extend(data, { "argumentJson": $.toJSON(procedureArgument.rows) });
            }
            var options = {
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/SaveJob/'+ encodeURIComponent(currentId),
                type: 'POST',
                dataType: 'json',
                data: data,
                beforeSubmit: function () {
                    return $('#editJobForm').form('validate');
                },
                success: function (r) {
                    if (r.result) {
                        currentId = r.id;
                        query();
                        $.messager.alert('确认', r.message, 'info');
                    } else {
                        $.messager.alert('错误', '保存失败:' + r.message, 'error');
                    }
                }
            };
            $('#editJobForm').ajaxForm(options);
            $('#editJobForm').submit();
        }
        function cancel() {
            if (currentId == null)
                clearJob();
            else
                loadJob();   
        }
    </script>
    <%-- dynamic tags --%>
    <script type="text/javascript">
        /*
         * set repeatInterval tags
         */
        var repeatInterval = {
            freq: "",
            interval: 0,
            byMonthDay: "",
            byDay: "",
            byHour: 0,
            byMinute: 0,
            bySecond: 0,
            init: function (s) {
                var array = s.split(';');
                for (var i = 0; i < array.length; i++) {
                    var subArray = array[i].split('=');
                    if (subArray.length > 0) {
                        switch (subArray[0]) {
                            case "FREQ": this.freq = subArray[1]; break;
                            case "INTERVAL": this.interval = subArray[1]; break;
                            case "BYMONTHDAY": this.byMonthDay = subArray[1]; break;
                            case "BYDAY": this.byDay = subArray[1]; break;
                            case "BYHOUR": this.byHour = subArray[1]; break;
                            case "BYMINUTE": this.byMinute = subArray[1]; break;
                            case "BYSECOND": this.bySecond = subArray[1]; break;
                        }
                    }
                }
            },
            display: function () {
                $('.subfitem').empty();
                displayRepeatInterval(this.freq);
                displayRepeat(this.freq);
                displayInterval(this.interval);
                displayByMonthDay(this.byMonthDay);
                displayByDay(this.byDay);
                displayByTime(this.byHour, this.byMinute, this.bySecond);
            }
        };
        function displayRepeat(freq) {
            $("[name='freq']").each(function () {
                if ($(this).val() == freq) {
                    if (preFreq != null) {
                        preFreq.removeAttr("checked");
                    }
                    $(this).attr('checked', 'true');
                    preFreq = $(this);
                    preFreq.attr("checked", "true");
                }
            })
        }
        function displayInterval(interval) {
            if (interval > 1)
                $('#interval').val(interval);
        }
        function displayByMonthDay(byMonthDay) {
            if (byMonthDay != "") {
                var array = byMonthDay.split(',');
                $("[name='byMonthDay']").each(function () {
                    for (var i = 0; i < array.length; i++) {
                        if ($(this).val() == array[i]) {
                            $(this).attr('checked', 'true');
                        }    
                    }
                })
            }
        }
        function displayByDay(byDay) {
            if (byDay != "") {
                var array = byDay.split(',');
                $("[name='byDay']").each(function () {
                    for (var i = 0; i < array.length; i++) {
                        if ($(this).val() == array[i]) {
                            $(this).attr('checked', 'true');
                        }
                    }
                })
            }
        }
        function displayByTime(byHour, byMinute, bySecond) {
            $('#byTime').timespinner('setValue', byHour + ':' + byMinute + ":" + bySecond);  
        }
        /*
         * display repeatInterval tags
         */
        function displayRepeatInterval(item) {
            var flag = true;
            switch (item) {
                case "SECONDLY": displayRepeatBySecondly(); break;
                case "MINUTELY": displayRepeatByMinutely(); break;
                case "HOURLY": displayRepeatByHourly(); break;
                case "DAILY": displayRepeatByDaily(); break;
                case "WEEKLY": displayRepeatByWeekly(); break;
                case "MONTHLY": displayRepeatByMonthly(); break;
                default: flag = false; break;
            }
            if (flag)
                $.parser.parse('.subfitem');
        }
        function getIntervalTag(min) {
            min = min ? min : 1;
            return $('<input type="text" class="easyui-numberbox" id="interval" name="interval" data-options="min:' + min + ',precision:0"/>');
        }
        function getByTimeTag() {
            return $('<input type="text" class="easyui-timespinner" id="byTime" name="byTime" data-options="showSeconds:true" value="00:00:00"/>');
        } 
        function displayRepeatBySecondly() {
            $('.subfitem').append('<label>时间间隔(秒)：</label>');
            $('.subfitem').append(getIntervalTag(5));
        }
        function displayRepeatByMinutely() {
            $('.subfitem').append('<label>时间间隔(分)：</label>');
            $('.subfitem').append(getIntervalTag());
        }
        function displayRepeatByHourly() {
            $('.subfitem').append('<label>时间间隔(时)：</label>');
            $('.subfitem').append(getIntervalTag());
        }
        function displayRepeatByDaily() {
            $('.subfitem').append('<div>');
            $('.subfitem').append('<label>时间间隔(天)：</label>');
            $('.subfitem').append(getIntervalTag());
            $('.subfitem').append('</div>');
            $('.subfitem').append('<div>');
            $('.subfitem').append('<label>时间：</label>');
            $('.subfitem').append(getByTimeTag());
            $('.subfitem').append('</div>');
        }
        function displayRepeatByWeekly() {
            $('.subfitem').append('<div>');
            $('.subfitem').append('<label>时间间隔(周)：</label>');
            $('.subfitem').append(getIntervalTag());
            $('.subfitem').append('</div>');
            $('.subfitem').append('<div>');
            $('.subfitem').append('<label>每周星期几：</label>');
            $('.subfitem').append('<input type="checkbox" id="chkMon" name="byDay" value="MON"/>星期一');
            $('.subfitem').append('<input type="checkbox" id="chkTue" name="byDay" value="TUE"/>星期二');
            $('.subfitem').append('<input type="checkbox" id="chkWed" name="byDay" value="WED"/>星期三');
            $('.subfitem').append('<input type="checkbox" id="chkThu" name="byDay" value="THU"/>星期四');
            $('.subfitem').append('<input type="checkbox" id="chkFri" name="byDay" value="FRI"/>星期五');
            $('.subfitem').append('<input type="checkbox" id="chkSat" name="byDay" value="SAT"/>星期六');
            $('.subfitem').append('<input type="checkbox" id="chkSun" name="byDay" value="SUN"/>星期日');
            $('.subfitem').append('</div>');
            $('.subfitem').append('<div>');
            $('.subfitem').append('<label>时间：</label>');
            $('.subfitem').append(getByTimeTag());
            $('.subfitem').append('</div>');
        }
        function displayRepeatByMonthly() {
            $('.subfitem').append('<div>');
            $('.subfitem').append('<label>时间间隔(月)：</label>');
            $('.subfitem').append(getIntervalTag());
            $('.subfitem').append('</div>');
            $('.subfitem').append('<div>');
            $('.subfitem').append('<label>每月几号：</label>');
            for (var i = 0; i < 31; i++) {
                var flag = i % 7 == 0;
                if (flag)
                    $('.subfitem').append('<div>');
                var value = i + 1;
                var text = value < 10 ? '0' + value : value;
                $('.subfitem').append('<input type="checkbox" id="chk' + value + '" name="byMonthDay" value="' + value + '"/>' + text);
                if (i == 30)
                    $('.subfitem').append('<input type="checkbox" id="chkLast" name="byMonthDay" value="-1"/>月底');
                if (flag)
                    $('.subfitem').append('</div>');        
            }
            $('.subfitem').append('</div>');
            $('.subfitem').append('<div>');
            $('.subfitem').append('<label>时间：</label>');
            $('.subfitem').append(getByTimeTag());
            $('.subfitem').append('</div>');
        }
    </script>
    <%-- job arguments --%>
    <script type="text/javascript">
        var editIndex = undefined;
        function endEditing() {
            if (editIndex == undefined) { return true }
            if ($('#dgProcedureArgument').datagrid('validateRow', editIndex)) {
                $('#dgProcedureArgument').datagrid('endEdit', editIndex);
                editIndex = undefined;
                return true;
            } else {
                return false;
            }
        }
        function clickProcedureArgumentRow(index) {
            if (editIndex != index) {
                if (endEditing()) {
                    $('#dgProcedureArgument').datagrid('selectRow', index)
							.datagrid('beginEdit', index);
                    editIndex = index;
                } else {
                    $('#dgProcedureArgument').datagrid('selectRow', editIndex);
                }
            }
        }
        function acceptProcedureArgument() {
            if (endEditing()) {
                $('#dgProcedureArgument').datagrid('acceptChanges');
            }
        }
    </script>
</asp:Content>
