function materialPrintPreview(LODOP, rows, imgUrl) {
    LODOP.PRINT_INITA(0, 0, 397, 280, "物料标签打印");
    LODOP.SET_PRINT_PAGESIZE(1, 1050, 740, "");
    //标题
    LODOP.ADD_PRINT_IMAGE(7, 0, 123, 42, "<img border='0' src='" + imgUrl + "'/>");
    LODOP.SET_PRINT_STYLEA(0, "ItemType", 1);
    LODOP.ADD_PRINT_TEXT(16, 137, 229, 35, "物  料  标  签");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 18);
    LODOP.SET_PRINT_STYLEA(0, "Alignment", 2);
    LODOP.SET_PRINT_STYLEA(0, "Bold", 1);
    LODOP.SET_PRINT_STYLEA(0, "ItemType", 1);
    //矩形
    LODOP.ADD_PRINT_RECT(52, 2, 364, 196, 0, 1);
    LODOP.SET_PRINT_STYLEA(0, "ItemType", 1);
    //横线
    LODOP.ADD_PRINT_LINE(113, 2, 113, 364, 0, 1);
    LODOP.SET_PRINT_STYLEA(0, "ItemType", 1);
    LODOP.ADD_PRINT_LINE(174, 2, 174, 364, 0, 1);
    LODOP.SET_PRINT_STYLEA(0, "ItemType", 1);
    LODOP.ADD_PRINT_LINE(210, 2, 210, 364, 0, 1);
    LODOP.SET_PRINT_STYLEA(0, "ItemType", 1);
    //竖线
    LODOP.ADD_PRINT_LINE(246, 93, 52, 93, 0, 1);
    LODOP.SET_PRINT_STYLEA(0, "ItemType", 1);
    LODOP.ADD_PRINT_LINE(210, 194, 174, 194, 0, 1);
    LODOP.SET_PRINT_STYLEA(0, "ItemType", 1);
    LODOP.ADD_PRINT_LINE(210, 250, 174, 250, 0, 1);
    LODOP.SET_PRINT_STYLEA(0, "ItemType", 1);
    //名目
    LODOP.ADD_PRINT_TEXT(75, 4, 90, 30, "物料编码");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 14);
    LODOP.SET_PRINT_STYLEA(0, "Alignment", 2);
    LODOP.SET_PRINT_STYLEA(0, "Bold", 1);
    LODOP.SET_PRINT_STYLEA(0, "ItemType", 1);
    LODOP.ADD_PRINT_TEXT(134, 4, 90, 30, "物料描述");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 14);
    LODOP.SET_PRINT_STYLEA(0, "Alignment", 2);
    LODOP.SET_PRINT_STYLEA(0, "Bold", 1);
    LODOP.SET_PRINT_STYLEA(0, "ItemType", 1);
    LODOP.ADD_PRINT_TEXT(183, 8, 80, 30, "数量");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 13);
    LODOP.SET_PRINT_STYLEA(0, "Alignment", 2);
    LODOP.SET_PRINT_STYLEA(0, "Bold", 1);
    LODOP.SET_PRINT_STYLEA(0, "ItemType", 1);
    LODOP.ADD_PRINT_TEXT(183, 197, 50, 30, "子库");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 13);
    LODOP.SET_PRINT_STYLEA(0, "Alignment", 2);
    LODOP.SET_PRINT_STYLEA(0, "Bold", 1);
    LODOP.SET_PRINT_STYLEA(0, "ItemType", 1);
    LODOP.ADD_PRINT_TEXT(220, 8, 80, 30, "供应商");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 13);
    LODOP.SET_PRINT_STYLEA(0, "Alignment", 2);
    LODOP.SET_PRINT_STYLEA(0, "Bold", 1);
    LODOP.SET_PRINT_STYLEA(0, "ItemType", 1);
    //值
    for (var i = 0; i < rows.length; i++) {
        if (i > 0)
            LODOP.NewPage();
        if (rows[i].materialDocNumber) {
            LODOP.ADD_PRINT_TEXT(65, 102, 260, 40, rows[i].materialDocNumber);
            LODOP.SET_PRINT_STYLEA(0, "FontSize", 31);
            LODOP.SET_PRINT_STYLEA(0, "Alignment", 2);
        }
        if (rows[i].materialDescription) {
            LODOP.ADD_PRINT_TEXT(130, 95, 260, 40, rows[i].materialDescription);
            LODOP.SET_PRINT_STYLEA(0, "FontSize", 12);
        }
        if (rows[i].assignQuantity) {
            LODOP.ADD_PRINT_TEXT(183, 95, 98, 30, rows[i].assignQuantity);
            LODOP.SET_PRINT_STYLEA(0, "FontSize", 12);
            LODOP.SET_PRINT_STYLEA(0, "Alignment", 2);
        }
        if (rows[i].warehouseId) {
            LODOP.ADD_PRINT_TEXT(183, 252, 110, 30, rows[i].warehouseId);
            LODOP.SET_PRINT_STYLEA(0, "FontSize", 12);
            LODOP.SET_PRINT_STYLEA(0, "Alignment", 2);
        }
        if (rows[i].supplierShortName) {
            LODOP.ADD_PRINT_TEXT(220, 97, 262, 30, rows[i].supplierShortName);
            LODOP.SET_PRINT_STYLEA(0, "FontSize", 12);
        }
    }

    LODOP.SET_SHOW_MODE("HIDE_PAPER_BOARD", 1);
    LODOP.PREVIEW();
}