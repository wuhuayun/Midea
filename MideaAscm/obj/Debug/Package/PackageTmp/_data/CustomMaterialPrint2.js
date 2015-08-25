﻿function materialPrintPreview(LODOP, rows) {
    LODOP.PRINT_INITA(0, 0, 397, 298, "物料标签打印");
    LODOP.SET_PRINT_PAGESIZE(1, 1050, 850, "");
    //标题
    LODOP.ADD_PRINT_TEXT(17, 18, 165, 35, "物 料 标 签");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 18);
    LODOP.SET_PRINT_STYLEA(0, "Alignment", 2);
    LODOP.SET_PRINT_STYLEA(0, "Bold", 1);
    LODOP.SET_PRINT_STYLEA(0, "ItemType", 1);
    //矩形
    LODOP.ADD_PRINT_RECT(50, 16, 365, 240, 0, 1);
    LODOP.SET_PRINT_STYLEA(0, "ItemType", 1);
    LODOP.ADD_PRINT_RECT(213, 291, 87, 70, 0, 1);
    LODOP.SET_PRINT_STYLEA(0, "ItemType", 1);
    //名目
    LODOP.ADD_PRINT_TEXT(55, 19, 63, 30, "子 库");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 13);
    LODOP.SET_PRINT_STYLEA(0, "Bold", 1);
    LODOP.SET_PRINT_STYLEA(0, "ItemType", 1);
    LODOP.ADD_PRINT_TEXT(55, 192, 81, 30, "作业日期");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 13);
    LODOP.SET_PRINT_STYLEA(0, "Bold", 1);
    LODOP.SET_PRINT_STYLEA(0, "ItemType", 1);
    LODOP.ADD_PRINT_TEXT(86, 19, 63, 30, "作业号");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 13);
    LODOP.SET_PRINT_STYLEA(0, "Bold", 1);
    LODOP.SET_PRINT_STYLEA(0, "ItemType", 1);
    LODOP.ADD_PRINT_TEXT(86, 237, 65, 30, "作业数");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 13);
    LODOP.SET_PRINT_STYLEA(0, "Bold", 1);
    LODOP.SET_PRINT_STYLEA(0, "ItemType", 1);
    LODOP.ADD_PRINT_TEXT(117, 19, 75, 30, "供应商");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 13);
    LODOP.SET_PRINT_STYLEA(0, "Bold", 1);
    LODOP.SET_PRINT_STYLEA(0, "ItemType", 1);
    LODOP.ADD_PRINT_TEXT(183, 265, 45, 30, "数量");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 13);
    LODOP.SET_PRINT_STYLEA(0, "Bold", 1);
    LODOP.SET_PRINT_STYLEA(0, "ItemType", 1);
    LODOP.ADD_PRINT_TEXT(148, 19, 85, 32, "入库日期");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 13);
    LODOP.SET_PRINT_STYLEA(0, "Bold", 1);
    LODOP.SET_PRINT_STYLEA(0, "ItemType", 1);
    LODOP.ADD_PRINT_TEXT(183, 19, 85, 30, "物料编码");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 13);
    LODOP.SET_PRINT_STYLEA(0, "Bold", 1);
    LODOP.SET_PRINT_STYLEA(0, "ItemType", 1);
    LODOP.ADD_PRINT_TEXT(213, 19, 20, 73, "物料描述");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 10);
    LODOP.SET_PRINT_STYLEA(0, "Bold", 1);
    LODOP.SET_PRINT_STYLEA(0, "ItemType", 1);
    LODOP.ADD_PRINT_TEXT(148, 215, 56, 30, "货位");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 13);
    LODOP.SET_PRINT_STYLEA(0, "Bold", 1);
    LODOP.SET_PRINT_STYLEA(0, "ItemType", 1);

    //值
    for (var i = 0; i < rows.length; i++) {
        if (i > 0)
            LODOP.NewPage();
        //作业数
        if (rows[i].wipEntityQuantity && rows[i].wipEntityQuantity > 0) {
            LODOP.ADD_PRINT_TEXT(86, 301, 80, 30, rows[i].wipEntityQuantity);
            LODOP.SET_PRINT_STYLEA(0, "FontSize", 13);
        }
        //条码
        if (rows[i].labelNo)
            LODOP.ADD_PRINT_BARCODE(18, 220, 146, 32, "", rows[i].labelNo);
        //作业号
        if (rows[i].wipEntityName) {
            LODOP.ADD_PRINT_TEXT(86, 81, 165, 30, rows[i].wipEntityName);
            LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
            LODOP.SET_PRINT_STYLEA(0, "FontSize", 13);
        }
        //作业日期
        if (rows[i]._wipEntityDate) {
            LODOP.ADD_PRINT_TEXT(55, 270, 115, 30, rows[i]._wipEntityDate);
            LODOP.SET_PRINT_STYLEA(0, "FontSize", 14);
        }
        //入库日期
        if (rows[i]._enterWarehouseDate) {
            LODOP.ADD_PRINT_TEXT(148, 100, 115, 30, rows[i]._enterWarehouseDate);
            LODOP.SET_PRINT_STYLEA(0, "FontSize", 14);
        }
        //物料编码
        if (rows[i].materialDocNumber) {
            //LODOP.ADD_PRINT_IMAGE(176, 100, 250, 40, rows[i].materialDocNumber);
            //LODOP.SET_PRINT_STYLEA(0, "Stretch", 1);

            LODOP.ADD_PRINT_TEXT(180, 95, 182, 30, rows[i].materialDocNumber);
            LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
            LODOP.SET_PRINT_STYLEA(0, "FontSize", 17);
        }
        //物料描述
        if (rows[i].materialDescription) {
            LODOP.ADD_PRINT_TEXT(213, 37, 254, 70, rows[i].materialDescription);
            LODOP.SET_PRINT_STYLEA(0, "FontSize", 12);
            LODOP.SET_PRINT_STYLEA(0, "Alignment", 2);
        }
        //数量
        if (rows[i].quantity) {
            LODOP.ADD_PRINT_TEXT(182, 310, 85, 30, rows[i].quantity);
            LODOP.SET_PRINT_STYLEA(0, "FontSize", 15);
        }
        //子库
        if (rows[i].warehouseId) {
            LODOP.ADD_PRINT_TEXT(55, 81, 115, 30, rows[i].warehouseId);
            LODOP.SET_PRINT_STYLEA(0, "FontSize", 13);
        }
        //供应商
        if (rows[i].supplierShortName) {
            LODOP.ADD_PRINT_TEXT(117, 81, 298, 30, rows[i].supplierShortName);
            LODOP.SET_PRINT_STYLEA(0, "FontSize", 15);
        }
        //未检预留_czy
        if (rows[i].checkResultCn) {
            LODOP.ADD_PRINT_TEXT(229, 299, 70, 45, rows[i].checkResultCn);
            LODOP.SET_PRINT_STYLEA(0, "FontSize", 20);
            LODOP.SET_PRINT_STYLEA(0, "Alignment", 2);
        }
        //货位编码_czy
        if (rows[i].locationDocNumber) {
            LODOP.ADD_PRINT_TEXT(145, 270, 110, 30, rows[i].locationDocNumber);
            LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
            LODOP.SET_PRINT_STYLEA(0, "FontSize", 17);
        }
    }

    LODOP.SET_SHOW_MODE("HIDE_PAPER_BOARD", 1);
    LODOP.PREVIEW();
    //LODOP.PRINT_DESIGN();
}