<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="MideaAscm.Dal.Vehicle.Entities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	卸货点状态监视
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="north" title="" border="false" style="height:30px;overflow:hidden;padding:0px 0px 2px 0px;">
        <div class="easyui-panel" title="" fit="true" border="true" style="overflow:hidden;">
            <div class="div_toolbar" style="float:left; height:26px; width:100%;padding:2px;">
			    <span>仓库选择：</span>
                <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseSelectCombo.ascx", 
                      new MideaAscm.Code.SelectCombo { queryParams = "'inUnloadingPoint':1", onChange = "warehouseOnChange" }); %>
	        </div>
        </div>
    </div>
    <div region="center" id="centerDiv" title="" border="true" style="padding:0px 5px 5px 5px;overflow:auto;">
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <style type="text/css">
        .warehouse { float:left; width:100%; border:1px solid #c9c9cb; overflow:hidden; margin:5px 0px 0px; padding-bottom:10px; }
        .warehouse h3 { position:relative; border-bottom:1px solid #c9c9cb; height:25px; line-height:25px; font-size:12px; font-weight:bold; text-indent:15px; background:#cedff1; margin-bottom:5px; z-index:0 }
        
        .unloadingPoint { float:left; padding-left:10px; padding-top:6px; padding-bottom:0px; width:100%; overflow:hidden; z-index:-6 }
        .unloadingPoint dl { float:left; width:140px; overflow:hidden; margin-bottom:0px; z-index:-7; border:0px solid #c9c9c9; }
        .unloadingPoint dl dd { float:left; clear:both; width:130px; z-index:-8;line-height:18px; height:20px; text-align:center }
        .unloadingPoint dl dd a { text-decoration:none; }
        .unloadingPoint dl dd span { color:#cc0000; }
        .unloadingPoint dl dt { float:left; width:128px; height:88px; position:relative; border:1px solid #c9c9c9; margin-bottom:3px; }
        .unloadingPoint dl dt.red{ background:#FF6600;  }
        .unloadingPoint dl dt.blue{ background:#6699CC;  }
        .unloadingPoint dl dt a { width:128px; height:88px; display:table-cell; text-align:center; vertical-align:middle; padding:0px; }
        .unloadingPoint dl dt a img { vertical-align:middle; z-index:-9 }
        .unloadingPoint dl dt img { max-height:80px; max-width:120px; border:medium none; display:block; margin:0 auto; margin-top:4px; padding:0px; }

        .unloadingPoint dl dd a:hover { color:#ff6600; text-decoration:underline; }
        
        .warehouse_legend { float:left; width:100%; border:0px solid #c9c9cb; overflow:hidden; margin:5px 0px 0px; padding-bottom:0px; }
        .warehouse_legend dl { float:left; width:120px; overflow:hidden; margin-bottom:0px; z-index:-7; border:0px solid #c9c9c9; }
        .warehouse_legend dl dd { line-height:18px; height:20px; margin-top:8px;margin-left:48px; border:0px solid red; }
        .warehouse_legend dl dt { float:left; width:44px; height:32px; border:1px solid #c9c9c9;margin-bottom:0px;}
        .warehouse_legend dl dt.red{ background:#FF6600;  }
        .warehouse_legend dl dt.blue{ background:#6699CC;  }
        .warehouse_legend dl dt img { max-height:32px; max-width:40px; border:medium none; display:block; margin:0 auto; margin-top:2px; padding:0px; }
    </style>
    <script type="text/javascript">
        var warehouseId = "";
        $(function(){
            setInterval(LoadUnloadingPointMonitor, 5000);
            LoadUnloadingPointMonitor();
        })
        function warehouseOnChange(newVal, oldVal) {
            warehouseId = newVal;
            LoadUnloadingPointMonitor();
        }
        function LoadUnloadingPointMonitor() {
            $.ajax({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Vehicle/LoadUnloadingPointMonitor/',
                type: "post",
                data: { "warehouseId" : warehouseId },
                success: function (r) {
                    if (r) {
                        $('#centerDiv').empty();
                        <% if (ViewData.ContainsKey("warehouseLegend")){ %>
                        $('#centerDiv').append('<%=ViewData["warehouseLegend"] %>');
                        <%} %>
                        $('#centerDiv').append(r);
                    }
                }
            })
        }
    </script>
</asp:Content>
