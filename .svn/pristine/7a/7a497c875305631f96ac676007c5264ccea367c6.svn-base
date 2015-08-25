<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	卸货位地图状态监视
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div region="west" title="" border="false" style="width:400px;overflow:hidden;padding:0px 0px 2px 0px;">
        <div class="easyui-layout" fit="true" style="" border="false">
            <div region="north" title="" border="false" style="height:400px;overflow:hidden;padding:padding:5px 5px 5px 5px;">
                <div class="easyui-panel" id="mq" class="dhMarquee" title="" fit="true" border="false" onmouseover="iScrollAmount=0" onmouseout="iScrollAmount=1" style="overflow:hidden;">
                    <div id="unloadingPoint_Prompt" class="mqdemo">
                        <ul>
                            <li>粤X123456请前往#1厂房A123位&nbsp;&nbsp;&nbsp;&nbsp;</li>
                            <li>联系方式：12345678</li>
                            <li>美的短号：12345</li>
                            <li>请联系仓管员：某某某</li>
                            <li>&nbsp;</li>
                            <li>粤X123456请前往#1厂房A123位&nbsp;&nbsp;&nbsp;&nbsp;</li>
                            <li>联系方式：12345678</li>
                            <li>美的短号：12345</li>
                            <li>请联系仓管员：某某某</li>
                        </ul>
                    </div>
                </div>
            </div>
            <div region="center" id="unloadingPoint_Info" title="" border="false" style="padding:5px 5px 5px 5px;overflow:auto;">
                <ul>
                    <li>#1厂房共有停车位<span>100</span></li>
                    <li>#1厂房空余停车位<span>50</span></li>
                    <li>#1厂房预约停车位<span>10</span></li>
                    <li>#1厂房故障停车位<span>10</span></li>
                </ul>
            </div>
        </div>
    </div>
    <div region="center" title="" border="false" style="padding:0px 0px 0px 0px;overflow:auto;">
        <div class="warehouseMap">
            <div style="position: relative;top: 100px;left: 20px;">
                <dl>
                    <dt>
                        <img src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Content/Images/Vehicle.jpg" border="0" alt="#123" />
                    </dt>
                    <dd> #123</dd>
                </dl>
            </div>
            <div style="position: relative;top: 100px;left: 70px;">
                <dl>
                    <dt>
                        
                    </dt>
                    <dd> #123</dd>
                </dl>
            </div>
            <div style="position: relative;top: 100px;left: 120px;">
                <dl>
                    <dt class="blue">
                        <img1 src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Content/Images/Vehicle.jpg" border="0" alt="#123" />
                    </dt>
                    <dd> #123</dd>
                </dl>
            </div>
            <div style="position: relative;top: 370px;left: 850px;">
                <dl>
                    <dt>
                        <img src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Content/Images/Vehicle.jpg" border="0" alt="#123" />
                    </dt>
                    <dd> #123</dd>
                </dl>
            </div>
            <div style="position: relative;top: 370px;left: 900px;">
                <dl>
                    <dt class="red">
                        <img2 src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Content/Images/Vehicle.jpg" border="0" alt="#123" />
                    </dt>
                    <dd> #123</dd>
                </dl>
            </div>
            <div style="position: relative;top: 370px;left: 950px;">
                <dl>
                    <dt>
                        <img src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Content/Images/Vehicle.jpg" border="0" alt="#123" />
                    </dt>
                    <dd> #123</dd>
                </dl>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <style type="text/css">
        /*.dhMarquee1 {width:100px;height:108px;text-align:left;margin:0px;padding:0px;border:1px solid #000;overflow:hidden;white-space:nowrap;} 
        .mqdemo {margin:0px;padding:0px;border:0px;}
        .dhScrollA {font-size:12px;display:block;padding:2px;}*/
        #unloadingPoint_Prompt ul { width:100%; overflow:hidden; }
        #unloadingPoint_Prompt ul li { display:inline; float:left; width:355px; height:32px; overflow:hidden; margin:0 5px 0 5px;padding-left:12px; line-height:32px; font-size:18px; }
        #unloadingPoint_Prompt ul li span { float:right; padding-left:10px; }
        
        #unloadingPoint_Info ul { width:100%; overflow:hidden; }
        #unloadingPoint_Info ul li { display:inline; float:left; width:355px; height:32px; overflow:hidden; margin:0 5px 0 5px;padding-left:12px; line-height:32px; font-size:18px; }
        #unloadingPoint_Info ul li span { float:right; padding-left:10px; }
    </style>
    <style type="text/css">
        .warehouseMap { top: 0px;left: 0px;position: relative;width:1199px; height:835px; border:1px solid #c9c9cb; overflow:hidden;z-index:1; background: url(<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/_data/UnloadingPointMap/full.png) 0px 0px no-repeat;}
        
        .unloadingPoint1 { float:left; padding-left:10px; padding-top:6px; padding-bottom:0px; width:100%; overflow:hidden; z-index:-6 }
        .warehouseMap dl {position: absolute; width:46px; overflow:hidden; margin-bottom:0px; z-index:-7; border:0px solid #c9c9c9; }
        .warehouseMap dl dd { float:left; clear:both; width:40px; z-index:-8;line-height:18px; height:20px; text-align:center }
        .warehouseMap dl dd a { text-decoration:none; }
        .warehouseMap dl dd span { color:#cc0000; }
        .warehouseMap dl dt { float:left; width:44px; height:32px; position:relative; border:1px solid #c9c9c9; margin-bottom:3px; }
        .warehouseMap dl dt.red{ background:#FF6600;  }
        .warehouseMap dl dt.blue{ background:#6699CC;  }
        .warehouseMap dl dt img { max-height:32px; max-width:40px; border:medium none; display:block; margin:0 auto; margin-top:2px; padding:0px; }

        .warehouse_legend { float:left; width:100%; border:0px solid #c9c9cb; overflow:hidden; margin:5px 0px 0px; padding-bottom:5px; }
        .warehouse_legend dl { float:left; width:120px; overflow:hidden; margin-bottom:0px; z-index:-7; border:0px solid #c9c9c9; }
        .warehouse_legend dl dd { line-height:18px; height:20px; margin-top:8px;margin-left:48px; border:0px solid red; }
        .warehouse_legend dl dt { float:left; width:44px; height:32px; border:1px solid #c9c9c9;margin-bottom:0px;}
        .warehouse_legend dl dt.red{ background:#FF6600;  }
        .warehouse_legend dl dt.blue{ background:#6699CC;  }
        .warehouse_legend dl dt img { max-height:32px; max-width:40px; border:medium none; display:block; margin:0 auto; margin-top:2px; padding:0px; }
        
    </style>

    <script type="text/javascript">
        $(function () {
            //$("#unloadingPoint_Info").rollList({ direction: "up", step: 1, time:23 });
        });
        (function ($) {
            $.fn.extend({
                rollList: function (option) {
                    option = $.extend({
                        direction: "up",
                        step: 1,
                        time: 23
                    }, option);
                    var step_coe, scroll_coe, score_coe;
                    if (option.direction == "up") {
                        step_coe = 1;
                        scroll_coe = 1;
                        score_coe = 1;
                    } else {
                        step_coe = -1;
                        scroll_coe = -1;
                        score_coe = 0;
                    }
                    return this.each(function () {
                        var $this = $(this);
                        var _this = this;
                        var itemHeight;
                        var temp = $("<DIV> </DIV>");
                        $this.css("overflow", "hidden").children().appendTo(temp);
                        $this.append(temp.clone(true)).append(temp);
                        itemHeight = $this.children();
                        itemHeight = itemHeight.eq(1).offset().top - itemHeight.eq(0).offset().top;
                        while ($this.children(":last").offset().top - $this.offset().top <= $this.height())
                            $this.append(temp.clone(true));
                        var roll;
                        this.scrollTop = itemHeight * (1 - score_coe);
                        roll = function () {
                            temp = setInterval(function () {
                                if (_this.scrollTop * scroll_coe >= itemHeight * score_coe) {
                                    _this.scrollTop = (_this.scrollTop - itemHeight) * scroll_coe;
                                }
                                _this.scrollTop += option.step * step_coe;

                            }, option.time);
                        }
                        $this.hover(function () {
                            clearInterval(temp);
                        }, function () {
                            roll();
                        });
                        roll();
                    });
                }
            })
        } (jQuery));
    </script>
    <script language="javascript">
        $(function () {
            /*var speed = 30;
            //滚动对象 
            var oMarquee = document.getElementById("mq");
            //内容对象 
            var omqdemo = document.getElementById("mqdemo");
            var h = oMarquee.offsetHeight;
            var odl = omqdemo.offsetHeight;
            var x = parseInt(h / odl) + 1;
            for (var i = 0; i < x; i++) {
                var o = omqdemo.cloneNode(true);
                oMarquee.appendChild(o);
            }
            var iScrollAmount = 1
            var myMar;
            function scroll() {
                oMarquee.scrollTop += iScrollAmount;
                var ol = oMarquee.scrollTop;
                if (odl - ol <= 0) {
                    window.clearTimeout(myMar);
                    oMarquee.scrollTop = 0;
                } else {
                    if (ol % h != 0) {
                        myMar = window.setTimeout(scroll, speed);
                    } else {
                        window.clearTimeout(myMar);
                    }
                }
            }
            var t = 3000;
            function clipShow() {
                scroll();
                window.setTimeout(clipShow, t);
            }
            window.setTimeout(clipShow, t);*/
        });

 </script>
</asp:Content>
