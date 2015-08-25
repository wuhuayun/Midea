<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<object  id="LODOP" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width="0" height="0"> 
    <embed id="LODOP_EM" type="application/x-print-lodop" width="0" height="0"></embed>
</object>
<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="print();">打印送货合单</a>
<!--进度条-->           
<div id="wLoadPrintProgressbar" class="easyui-window" title="加载打印数据..." style="padding: 10px;width:440px;height:80px;"
    data-options="collapsible:false,minimizable:false,maximizable:false,resizable:false,modal:true,shadow:true,closed:true">
    <div id="pLoadPrint" class="easyui-progressbar" style="width:400px;"></div>
</div>
<script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/lodop/LodopFuncs_6.0.js"></script>
<script type="text/javascript">
    <%--打印预览--%>
    function printPreview(printParams) {
        $.ajax({
            type: "POST",
            url: '<%=Url.Action("SupplierDeliveryBatSumPrint", "SupplierPreparation", new { Area = "Ascm" })%>',
            cache: false,
            dataType: 'json',
            data: printParams,
           beforeSend: function () {
                $('#wLoadPrintProgressbar').window('open');
                $('#pLoadPrint').progressbar('setValue', 0);
                setInterval(updateProgress, 100);
                return true;
            },
            success: function (result) {
                $('#pLoadPrint').progressbar('setValue', 100);
                $('#wLoadPrintProgressbar').window('close');
                if (result.success) {
                    
                    /* 判断合单是否有容器，有则多打印合单最后一页的容器入库单*/
                    if(result.data.listContainerSpec.length>0)
                        createPrintContainerPage(result.data);
                    else
                        createPrintPage(result.data);
                    /* 打印预览 */
                    LODOP.SET_SHOW_MODE("HIDE_PAPER_BOARD",1);
                    LODOP.PREVIEW();
                }
            }
        });
    }
    /*
    *  无容器的合单
    */
    function createPrintPage(data){
        //初次下载插件时，脚本会提示“没有权限”,需将其移到aspx页面中
        //最好用IE浏览器，谷歌无法显示下载插件页面   by余峰嘉
        //var downloadUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/_data/install_lodop.exe';
        //var LODOP = getLodop(document.getElementById('LODOP'), document.getElementById('LODOP_EM'), downloadUrl);
        LODOP.PRINT_INITA(0,0,835,525,"供应商送货合单打印");
        LODOP.SET_PRINT_PAGESIZE(1,2210,1390,"");
        /* 页眉 */
        LODOP.ADD_PRINT_IMAGE(6,5,194,56,"<img border='0' src='<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/lodop/MideaLogo.jpg'/>");
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
        LODOP.ADD_PRINT_TEXT(21,263,308,32,"美的中央空调供应商送货合单");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"FontSize",14);
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
        
        LODOP.SET_PRINT_STYLEA(0,"Horient",2);
        LODOP.SET_PRINT_STYLEA(0,"LetterSpacing",2);
      

        LODOP.ADD_PRINT_TEXT(14,610,70,20,"合单号：\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
        
        LODOP.ADD_PRINT_TEXT(69,5,75,20,"供方简称：\n\n\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         
        LODOP.ADD_PRINT_TEXT(69,176,75,20,"供方编码：\n\n\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         
        LODOP.ADD_PRINT_TEXT(69,313,79,20,"司机编号：\n\n\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         
         LODOP.ADD_PRINT_TEXT(69,447,75,20,"出货子库：\n\n\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         
        LODOP.ADD_PRINT_TEXT(69,566,75,20,"收货仓库：\n\n\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         
        
        /*LODOP.ADD_PRINT_TEXT(69,685,50,20,"状态：\n\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);*/
        /*LODOP.ADD_PRINT_TEXT(69,685,75,20,"送货地点：\n\n\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);*/
        
        LODOP.ADD_PRINT_TEXT(90,5,104,20,"预约开始时间：\n\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         
        LODOP.ADD_PRINT_TEXT(90,200,104,20,"预约结束时间：\n\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         
        /*LODOP.ADD_PRINT_TEXT(90,385,51,20,"容器：\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);*/
        LODOP.ADD_PRINT_TEXT(90,490,50,20,"状态：\n\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         
        LODOP.ADD_PRINT_TEXT(90,610,75,20,"送货地点：\n\n\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         
        /*LODOP.ADD_PRINT_TEXT(90,710,50,20,"状态：\n\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);*/

        var rows;
        if (data != null && data != "" && data != undefined) {
            rows = data.listDetail;

            LODOP.ADD_PRINT_TEXT(14,662,95,20,data.docNumber);
            //LODOP.ADD_PRINT_TEXT(14,687,95,20,data.docNumber);
            LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
            LODOP.ADD_PRINT_TEXT(69,69,110,20,data.supplierShortName);
            LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
             
            LODOP.ADD_PRINT_TEXT(69,240,77,20,data.supplierDocNumber);
            LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
             
            LODOP.ADD_PRINT_TEXT(69,377,75,20,data.driverSn);
            LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
             
            LODOP.ADD_PRINT_TEXT(69,511,60,20,data.supplyWarehouse);
            LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
             
            LODOP.ADD_PRINT_TEXT(69,630,60,20,data.warehouse);
            LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
             
            //LODOP.ADD_PRINT_TEXT(69,720,50,20,data.status);
            //LODOP.ADD_PRINT_TEXT(69,749,75,20,data.deliveryPlace);
            //LODOP.SET_PRINT_STYLEA(0,"ItemType",1);

            LODOP.ADD_PRINT_TEXT(90,91,105,20,data.appointmentStartTime);
            LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
             
            LODOP.ADD_PRINT_TEXT(90,286,115,20,data.appointmentEndTime);
            LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
             
            //LODOP.ADD_PRINT_TEXT(90,420,289,20,data.containers);
            LODOP.ADD_PRINT_TEXT(90,525,50,20,data.status);
            LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
             
            LODOP.ADD_PRINT_TEXT(90,712,70,20,data.deliveryPlace);
            //LODOP.ADD_PRINT_TEXT(90,749,75,20,data.status);
            LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
             
        }
        /* 页脚 */
        LODOP.ADD_PRINT_TEXT(454,7,190,20,"供方经办人：\n\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         
        LODOP.ADD_PRINT_TEXT(454,213,190,20,"需方经办人：\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         
        LODOP.ADD_PRINT_TEXT(454,419,190,20,"需方报检时间：\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         
        LODOP.ADD_PRINT_TEXT(454,625,190,20,"需方检验员：\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         

        LODOP.ADD_PRINT_TEXT(472,7,150,20,"第1联：白色-需方仓库\n");
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         
        LODOP.ADD_PRINT_TEXT(472,161,150,20,"第2联：红色-需方检验\n");
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         
        LODOP.ADD_PRINT_TEXT(472,315,150,20,"第3联：黄色-需方记帐\n");
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         
        LODOP.ADD_PRINT_TEXT(472,469,150,20,"第4联：蓝色-供方财务\n");
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         
        LODOP.ADD_PRINT_TEXT(472,623,150,20,"第5联：绿色-供方制单\n");
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         
        /* 主体 */
        barcodes = new Array();
        LODOP.ADD_PRINT_TABLE(110,-6,756,314,getTable(rows));
        /* 条码 */
        for(var i = 0;i < barcodes.length; i++) {
            if (i > 0)
                LODOP.NewPage();
            LODOP.ADD_PRINT_BARCODE(29,635,180,26,"128B",barcodes[i]);
            LODOP.SET_PRINT_STYLEA(0,"FontSize",8);
            LODOP.SET_PRINT_STYLEA(0,"ShowBarText",0);
            LODOP.SET_PRINT_STYLEA(0,"Horient",1);
            //LODOP.ADD_PRINT_TEXT(54,675,100,20,barcodes[i]);
            LODOP.ADD_PRINT_TEXT(54,650,100,20,barcodes[i]);
        }
   }



    /*
     *   有容器的合单
     */
    function createPrintContainerPage(data){
        LODOP.PRINT_INITA(0,0,835,525,"供应商送货合单打印");
        LODOP.SET_PRINT_PAGESIZE(1,2210,1390,"");
        /* 页眉 */
        LODOP.ADD_PRINT_IMAGE(6,5,194,56,"<img border='0' src='<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/lodop/MideaLogo.jpg'/>");
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
        LODOP.ADD_PRINT_TEXT(21,263,308,32,"美的中央空调供应商送货合单");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"FontSize",14);
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
        LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示
        LODOP.SET_PRINT_STYLEA(0,"Horient",2);
        LODOP.SET_PRINT_STYLEA(0,"LetterSpacing",2);
      

        LODOP.ADD_PRINT_TEXT(14,610,70,20,"合单号：\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
        LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示
        
        LODOP.ADD_PRINT_TEXT(69,5,75,20,"供方简称：\n\n\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示
        LODOP.ADD_PRINT_TEXT(69,176,75,20,"供方编码：\n\n\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示
        LODOP.ADD_PRINT_TEXT(69,313,79,20,"司机编号：\n\n\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示
         LODOP.ADD_PRINT_TEXT(69,447,75,20,"出货子库：\n\n\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示
        LODOP.ADD_PRINT_TEXT(69,566,75,20,"收货仓库：\n\n\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示
        
        LODOP.ADD_PRINT_TEXT(90,5,104,20,"预约开始时间：\n\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示
        LODOP.ADD_PRINT_TEXT(90,200,104,20,"预约结束时间：\n\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示
        LODOP.ADD_PRINT_TEXT(90,490,50,20,"状态：\n\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示
        LODOP.ADD_PRINT_TEXT(90,610,75,20,"送货地点：\n\n\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示
        var rows;
        if (data != null && data != "" && data != undefined) {
            rows = data.listDetail;

            LODOP.ADD_PRINT_TEXT(14,662,95,20,data.docNumber);
            LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
             LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示
            LODOP.ADD_PRINT_TEXT(69,69,110,20,data.supplierShortName);
            LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
             LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示
            LODOP.ADD_PRINT_TEXT(69,240,77,20,data.supplierDocNumber);
            LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
             LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示
            LODOP.ADD_PRINT_TEXT(69,377,75,20,data.driverSn);
            LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
             LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示
            LODOP.ADD_PRINT_TEXT(69,511,60,20,data.supplyWarehouse);
            LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
             LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示
            LODOP.ADD_PRINT_TEXT(69,630,60,20,data.warehouse);
            LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
             LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示

            LODOP.ADD_PRINT_TEXT(90,91,105,20,data.appointmentStartTime);
            LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
             LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示
            LODOP.ADD_PRINT_TEXT(90,286,115,20,data.appointmentEndTime);
            LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
             LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示
            LODOP.ADD_PRINT_TEXT(90,525,50,20,data.status);
            LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
             LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示
            LODOP.ADD_PRINT_TEXT(90,712,70,20,data.deliveryPlace);
            LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
             LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示
        }
        /* 页脚 */
        LODOP.ADD_PRINT_TEXT(454,7,190,20,"供方经办人：\n\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示
        LODOP.ADD_PRINT_TEXT(454,213,190,20,"需方经办人：\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示
        LODOP.ADD_PRINT_TEXT(454,419,190,20,"需方报检时间：\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示
        LODOP.ADD_PRINT_TEXT(454,625,190,20,"需方检验员：\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示

        LODOP.ADD_PRINT_TEXT(472,7,150,20,"第1联：白色-需方仓库\n");
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示
        LODOP.ADD_PRINT_TEXT(472,161,150,20,"第2联：红色-需方检验\n");
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示
        LODOP.ADD_PRINT_TEXT(472,315,150,20,"第3联：黄色-需方记帐\n");
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示
        LODOP.ADD_PRINT_TEXT(472,469,150,20,"第4联：蓝色-供方财务\n");
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示
        LODOP.ADD_PRINT_TEXT(472,623,150,20,"第5联：绿色-供方制单\n");
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         LODOP.SET_PRINT_STYLEA(0,"PageUnIndex","Last");	//控制页眉页脚不显示
        /* 主体 */
        barcodes = new Array();
        LODOP.ADD_PRINT_TABLE(110,-6,756,314,getTable(rows));
        /* 条码 */
        for(var i = 0;i < barcodes.length; i++) {
            if (i > 0)
                LODOP.NewPage();
            LODOP.ADD_PRINT_BARCODE(29,635,180,26,"128B",barcodes[i]);
            LODOP.SET_PRINT_STYLEA(0,"FontSize",8);
            LODOP.SET_PRINT_STYLEA(0,"ShowBarText",0);
            LODOP.SET_PRINT_STYLEA(0,"Horient",1);
            LODOP.ADD_PRINT_TEXT(54,650,100,20,barcodes[i]);
        }           

        /*
         *   标准物流器具入库通知单
         */
        LODOP.NewPage();
         /* 页眉 */
        LODOP.ADD_PRINT_IMAGE(6,5,194,56,"<img border='0' src='<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/lodop/MideaLogo.jpg'/>");
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
        LODOP.ADD_PRINT_TEXT(11,233,348,32,"中央空调事业部顺德工厂");
        LODOP.SET_PRINT_STYLEA(0,"FontSize",15);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
        LODOP.SET_PRINT_STYLEA(0,"PageIndex","Last");//控制最后一页的页眉页脚输出
        LODOP.SET_PRINT_STYLEA(0,"Horient",2);
        LODOP.SET_PRINT_STYLEA(0,"LetterSpacing",10);

        LODOP.ADD_PRINT_TEXT(41,273,309,32,"标准物流器具入库通知单");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"FontSize",15);
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         LODOP.SET_PRINT_STYLEA(0,"PageIndex","Last");//控制最后一页的页眉页脚输出
        LODOP.SET_PRINT_STYLEA(0,"Horient",2);
        LODOP.SET_PRINT_STYLEA(0,"LetterSpacing",1);

        /*LODOP.ADD_PRINT_TEXT(14,610,70,20,"合单号：\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);*/
        
        LODOP.ADD_PRINT_TEXT(69,5,170,20,"开单日期:"+LODOP.FORMAT("TIME:YYYY年MM月DD日","DATE"));
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
        LODOP.SET_PRINT_STYLEA(0,"PageIndex","Last");//控制最后一页的页眉页脚输出
        
        LODOP.ADD_PRINT_TEXT(90,5,75,20,"供方简称：\n\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         LODOP.SET_PRINT_STYLEA(0,"PageIndex","Last");//控制最后一页的页眉页脚输出
        LODOP.ADD_PRINT_TEXT(90,470,75,20,"收货仓库：\n\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         LODOP.SET_PRINT_STYLEA(0,"PageIndex","Last");//控制最后一页的页眉页脚输出
        LODOP.ADD_PRINT_TEXT(90,610,75,20,"送货地点：\n\n\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         LODOP.SET_PRINT_STYLEA(0,"PageIndex","Last");//控制最后一页的页眉页脚输出

        
        if (data != null && data != "" && data != undefined) {
            
            /*LODOP.ADD_PRINT_TEXT(14,662,95,20,data.docNumber);
            LODOP.SET_PRINT_STYLEA(0,"ItemType",1);*/                    

            LODOP.ADD_PRINT_TEXT(90,71,105,20,data.supplierShortName);
            LODOP.SET_PRINT_STYLEA(0,"ItemType",1);   
             LODOP.SET_PRINT_STYLEA(0,"PageIndex","Last");//控制最后一页的页眉页脚输出        
            LODOP.ADD_PRINT_TEXT(90,530,90,20,data.warehouse);
            LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
             LODOP.SET_PRINT_STYLEA(0,"PageIndex","Last");//控制最后一页的页眉页脚输出
            LODOP.ADD_PRINT_TEXT(90,712,70,20,data.deliveryPlace);
            LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
             LODOP.SET_PRINT_STYLEA(0,"PageIndex","Last");//控制最后一页的页眉页脚输出
        }
        /* 页脚 */
        LODOP.ADD_PRINT_TEXT(454,120,190,20,"制单：\n\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         LODOP.SET_PRINT_STYLEA(0,"PageIndex","Last");//控制最后一页的页眉页脚输出
        LODOP.ADD_PRINT_TEXT(454,313,190,20,"审核：\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         LODOP.SET_PRINT_STYLEA(0,"PageIndex","Last");//控制最后一页的页眉页脚输出
        LODOP.ADD_PRINT_TEXT(454,503,190,20,"收货方经办人：\n");
        LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
        LODOP.SET_PRINT_STYLEA(0,"Bold",1);
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
         LODOP.SET_PRINT_STYLEA(0,"PageIndex","Last");//控制最后一页的页眉页脚输出

        LODOP.ADD_PRINT_TEXT(472,160,150,20,"第一联：存根联\n");
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
        LODOP.SET_PRINT_STYLEA(0,"PageIndex","Last");//控制最后一页的页眉页脚输出
        LODOP.ADD_PRINT_TEXT(472,260,150,20,"第二联：记账联\n");
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
        LODOP.SET_PRINT_STYLEA(0,"PageIndex","Last");//控制最后一页的页眉页脚输出
        LODOP.ADD_PRINT_TEXT(472,360,150,20,"第三联：收货方联\n");
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
        LODOP.SET_PRINT_STYLEA(0,"PageIndex","Last");//控制最后一页的页眉页脚输出
        LODOP.ADD_PRINT_TEXT(472,478,150,20,"第四联：放行联\n");
        LODOP.SET_PRINT_STYLEA(0,"ItemType",1);
        LODOP.SET_PRINT_STYLEA(0,"PageIndex","Last");//控制最后一页的页眉页脚输出 
         /* 主体 */
        LODOP.ADD_PRINT_TABLE(110,-6,756,314,getContainerTable(data));
    }

    /*
     *  获取容器的具体规格  2014.11.17 by余峰嘉
     */
    function getSpecDetail(containerSpec){
        switch(containerSpec){
            case "A":
                return "300mm*200mm*148mm";
            case "B":
                return "400mm*300mm*148mm";
            case "D":
                return "600mm*400mm*280mm";
            case "H":
                return "600mm*400mm*148mm";
            default:
                return "";
        }
    }
    /*
     *   容器单表格
     */
    var containerCount;
    function getContainerTable(data) {            
        var style = "<style type='text/css'>";
        var table = "<table border=\"1\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" style=\"border-collapse:collapse;border-color:#000000;\">";
        table += "<thead>";
        table += "<tr style=\"height:26px;font-size:15px;font-weight:bold;text-align:center;\">";
        table += "<th style=\"width:200px;\">器具名称</th>";
        table += "<th style=\"width:310px;\">器具规格</th>";
        table += "<th style=\"width:52px;\">单位</th>";
        table += "<th style=\"width:57px;\">数量</th>";
        table += "<th style=\"width:104px;\">备注</th>";
        table += "</tr>";
        table += "</thead>";
        for(var containerCount = 0; containerCount<data.listContainerSpec.length;containerCount++){
            table += "<tr style=\"height:23px;font-size:14px;text-align:center;\">";
            table += "<td>" + data.listContainerDescription[containerCount] + "</td>";
            table += "<td>" + getSpecDetail(data.listContainerSpec[containerCount]) + "</td>";
            table += "<td>个</td>";
            table += "<td>"  + data.listContainerQuantity[containerCount] +"</td>";
            table += "<td></td>";
            table += "</tr>";
        }
        table += "<tr style=\"height:22px;font-size:14px;text-align:center;\">"
        table += "<td colspan=\"2\"><b>合计</b></td>"
        table += "<td></td>"
        table += "<td>"+data.containers+"</td>"
        table += "<td></td>"
        table += "</tr>"
        table += "</table>";
        return table;
    }
    /*
     *  合单表格
     */
    var barcodes;
    function getTable(rows) {
        var style = "<style type='text/css'>";
        var table = "<table border=\"1\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" style=\"border-collapse:collapse;border-color:#000000;\">";
        table += "<thead>";
        table += "<tr style=\"height:24px;font-size:12px;font-weight:bold;text-align:center;\">";
        table += "<th style=\"width:94px;\">物料编码</th>";
        table += "<th style=\"width:63px;\">送货单<br/>条码</th>";
        table += "<th style=\"width:362px;\">物料描述</th>";
        table += "<th style=\"width:63px;\">送货量</th>";
        table += "<th style=\"width:32px;\">单位</th>";
        table += "<th style=\"width:47px;\">容器绑<br/>定数量</th>";
        table += "<th style=\"width:31px;\">容器<br/>数量</th>";
        table += "<th style=\"width:94px;\">备注</th>";
        table += "</tr>";
        table += "</thead>";
        if (rows != null && rows != "" && rows != undefined) {
            $.each(rows, function(i, item) {
                if ($.inArray(item.barcode, barcodes) == -1)
                    barcodes.push(item.barcode);
                table += "<tr style=\"height:22px;font-size:12px;text-align:center;\">";
                table += "<td>" + item.materialDocNumber + "</td>";
                table += "<td>" + item.batchBarCode + "</td>";
                table += "<td style=\"text-align:left;\">" + item.materialDescription + "</td>";
                table += "<td>" + item.deliveryNumber + "</td>";
                table += "<td>" + item.materialUnit + "</td>";
                table += "<td>" + item.containerBindNumber + "</td>";
                table += "<td>" + item.containerNumber + "</td>";
                table += "<td>" + item.comment + "</td>";
                table += "</tr>";
                table += "<tr style=\"height:22px;font-size:12px;\">";
                var totalQuantity = item.totalQuantity > 0 ? "<b>总数：</b>" + item.totalQuantity : "";
                table += "<td style=\"text-align:left;\">" + totalQuantity + "</td>";
                table += "<td colspan=\"7\">" + item.batchDocNumber + "&nbsp;&nbsp;<b>作业号：</b>" + item.wipEntities + "</td>";
                table += "</tr>";
            });
        }
        table += "</table>";
        return table;
    }

    function updateProgress() {
        var value = $('#pLoadPrint').progressbar('getValue');
        if (value < 99) {
            $('#pLoadPrint').progressbar('setValue', value + 1);
        }
    }
</script>

