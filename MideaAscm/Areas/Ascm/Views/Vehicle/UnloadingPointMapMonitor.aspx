<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	卸货位地图状态监视
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="north" title="" border="false" style="height:30px;overflow:hidden;padding:0px 0px 2px 0px;">
        <div class="easyui-panel" title="" fit="true" border="true" style="overflow:hidden;">
            <div class="div_toolbar" style="float:left; height:26px; width:100%;padding:2px;">
			    <span>仓库选择：</span>
                <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseSelectCombo.ascx",
                      new MideaAscm.Code.SelectCombo { queryParams = "'inUnloadingPointMap':1", onChange = "warehouseOnChange" }); %>
                <span>卸货位地图选择：</span>
                <select id="unloadingMapSelect" onchange="unloadingMapOnChange();">
                    <option value="">选择地图</option>
                </select>
	        </div>
        </div>
    </div>
    <div region="center" id="centerDiv" title="" border="true" style="padding:5px 5px 5px 5px;overflow:auto;">
        <% if (ViewData.ContainsKey("warehouseLegend")){ %>
        <%=ViewData["warehouseLegend"] %>
        <%} %>
        <div class="warehouseMap"></div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <style type="text/css">
        /*.warehouseMap { top: 0px;left: 0px;position: relative;width:1199px; height:835px; border:1px solid #c9c9cb; overflow:hidden;z-index:1; background: url(<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/_data/UnloadingPointMap/full.png) 0px 0px no-repeat;}*/
        .warehouseMap { top: 0px;left: 0px;position: relative; border:1px solid #c9c9cb; overflow:hidden;z-index:1;}
        
        /*.unloadingPoint1 { float:left; padding-left:10px; padding-top:6px; padding-bottom:0px; width:100%; overflow:hidden; z-index:-6 }*/
        /*.warehouseMap dl { position: absolute; width:46px; overflow:hidden; margin-bottom:0px; z-index:-7; border:0px solid #c9c9c9; }*/
        .warehouseMap dl { position: absolute; overflow:hidden; margin-bottom:0px; z-index:-7; border:0px solid #c9c9c9; }
        /*.warehouseMap dl dd { float:left; clear:both; width:40px; z-index:-8;line-height:18px; height:20px; text-align:center }*/
        .warehouseMap dl dd { float:left; clear:both; z-index:-8; text-align:center }
        /*.warehouseMap dl dd a { text-decoration:none; }
        .warehouseMap dl dd span { color:#cc0000; }*/
        /*.warehouseMap dl dt { float:left; width:44px; height:32px; position:relative; border:1px solid #c9c9c9; margin-bottom:3px; }*/
        .warehouseMap dl dt { float:left; position:relative; border:1px solid #c9c9c9; margin-bottom:3px; }
        .warehouseMap dl dt.red{ background:#FF6600;  }
        .warehouseMap dl dt.blue{ background:#6699CC;  }
        /*.warehouseMap dl dt img { max-height:32px; max-width:40px; border:medium none; display:block; margin:0 auto; margin-top:2px; padding:0px; }*/
        .warehouseMap dl dt img { border:medium none; display:block; margin:0 auto; margin-top:2px; padding:0px; }

        .warehouse_legend { float:left; width:100%; border:0px solid #c9c9cb; overflow:hidden; margin:5px 0px 0px; padding-bottom:5px; }
        .warehouse_legend dl { float:left; width:120px; overflow:hidden; margin-bottom:0px; z-index:-7; border:0px solid #c9c9c9; }
        .warehouse_legend dl dd { line-height:18px; height:20px; margin-top:8px;margin-left:48px; border:0px solid red; }
        .warehouse_legend dl dt { float:left; width:44px; height:32px; border:1px solid #c9c9c9;margin-bottom:0px;}
        .warehouse_legend dl dt.red{ background:#FF6600;  }
        .warehouse_legend dl dt.blue{ background:#6699CC;  }
        .warehouse_legend dl dt img { max-height:32px; max-width:40px; border:medium none; display:block; margin:0 auto; margin-top:2px; padding:0px; }
    </style>
    <script type="text/javascript">
        var warehouseId = null, mapId = null;
        $(function(){
            setInterval(LoadUnloadingPointMapMonitor, 5000);
        })
        function unloadingMapOnChange() {
            var mapId = $('#unloadingMapSelect').val();
            LoadUnloadingPointMapMonitor();
        }
        function warehouseOnChange(newVal, oldVal) {
            mapId = null;
            $('#unloadingMapSelect').empty();
            if (newVal) {
                $.ajax({
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Vehicle/GetUnloadingPointMapList/',
                    type: "post",
                    dataType: "json",
                    async: false,
                    data: { "warehouseId" : newVal },
                    success: function (r) {
                        if (r) {
                            $.each(r, function (index, item) {
                                if (index == 0)
                                    mapId = item.id;
                                $('#unloadingMapSelect').append('<option value="' + item.id + '">' + item.name + '</option>');
                            })
                        }
                    }
                })
            }
            warehouseId = newVal;
            LoadUnloadingPointMapMonitor();
        }
        function LoadUnloadingPointMapMonitor() {
            $.ajax({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Vehicle/LoadUnloadingPointMapMonitor/',
                type: 'post',
                dataType: 'json',
                data: { "warehouseId" : warehouseId, "mapId" : mapId },
                success: function (r) {
                    if (r.result) {
                        $('.warehouseMap').empty();
                        $('.warehouseMap').append(r.entity.display);

                        $('.warehouseMap').css($.parseJSON(r.entity.warehouseMapCss));
                        $('.warehouseMap dl').css($.parseJSON(r.entity.dlCss));
                        $('.warehouseMap dl dd').css($.parseJSON(r.entity.ddCss));
                        $('.warehouseMap dl dt').css($.parseJSON(r.entity.dtCss));
                        $('.warehouseMap dl dt img').css($.parseJSON(r.entity.dtImgCss));
                    }
                }
            })
        }
    </script>
</asp:Content>
