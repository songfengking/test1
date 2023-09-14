﻿///////////////////////////////////////////////////////////////////////////////////////////////
// 製品別通過実績検索画面(SearchProductOrder)用制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
// メイン処理からの各画面派生呼び出し(PageLoad)
function MasterMainPageLoaded() {
    SearchProductOrder.Initialize();
}
// メイン処理からの各画面派生呼び出し(Resize)
function MasterMainPageResized() {
    SearchProductOrder.ResizeGridArea2();
}

SearchProductOrder = {
    // 定数
    MIN_HEIGHT: 70,
    PARENT_DIV_W_MARGIN: 6,
    INNER_DIV_H_MARGIN: 3,
    GRID_LT: "divLTScroll",
    GRID_RT: "divRTScroll",
    GRID_LB: "divLBScroll",
    GRID_RB: "divRBScroll",

    // メソッド
    ResizeGridArea2: function () {
        // コンテンツタグの位置を取得
        var containtsBottom = 0;
        var containtsArea = $("#" + MasterMain.CONTAINTS_AREA);
        var containtsBottomArea = $("#" + MasterMain.CONTAINTS_AREA_BOTTOM);
        if (null != containtsArea.position()) {
            containtsBottom = containtsBottomArea.position().top - containtsArea.position().top + SearchProductOrder.MIN_HEIGHT;
            if (null == containtsBottom) {
                containtsBottom = 0;
            }
        }
        // ウィンドウ幅
        var windowWidth = $(document).innerWidth();
        // グリッド用DIVサイズ変更
        var gridLT = $("#" + this.GRID_LT);
        var gridRT = $("#" + this.GRID_RT);
        var gridLB = $("#" + this.GRID_LB);
        var gridRB = $("#" + this.GRID_RB);
        // Table Header幅
        var tblHeaders_Width = 0;
        // Div(右)幅
        var divR_Width = 0;
        // Div(右)位置(横)
        var divR_Left = 0;
        // Div(下)位置(縦)
        var divMain_Top = 0;
        // Div(下)高さ
        var divMain_Height = 0;
        if (0 < gridLT.length) {
            // グリッドのDIVを幅変更する
            if (null != gridLT.position()) {
                divR_Left = gridLT.position().left + gridLT.width();
            }
            try {
                tblHeaders_Width = windowWidth - this.PARENT_DIV_W_MARGIN;
                divR_Width = tblHeaders_Width - divR_Left;
                gridRT.width(divR_Width);
                gridRB.width(divR_Width);
            } catch (e) {
            }
            // グリッドのDIVを高さ変更する
            if (null != gridLB.position()) {
                divMain_Top = gridLB.position().top;
            }
            try {
                divMain_Height = containtsBottom - divMain_Top - SearchProductOrder.INNER_DIV_H_MARGIN;
                gridLB.height(divMain_Height);
                gridRB.height(divMain_Height);
            } catch (e) {
            }
        }
    }
    ,
    Initialize: function () {
        $("#" + SearchProductOrder.GRID_RB).scroll(function () {
            var gridRT = $("#" + SearchProductOrder.GRID_RT);
            var gridLB = $("#" + SearchProductOrder.GRID_LB);
            var gridRB = $("#" + SearchProductOrder.GRID_RB);
            if (gridRB.scrollTop() != gridLB.scrollTop()) {
                gridLB.scrollTop(gridRB.scrollTop());
            }
            if (gridRB.scrollLeft() != gridRT.scrollLeft()) {
                gridRT.scrollLeft(gridRB.scrollLeft());
            }
        });
        $("#" + SearchProductOrder.GRID_LB).scroll(function () {
            var gridLB = $("#" + SearchProductOrder.GRID_LB);
            var gridRB = $("#" + SearchProductOrder.GRID_RB);
            if (gridLB.scrollTop() != gridRB.scrollTop()) {
                gridRB.scrollTop(gridLB.scrollTop());
            }
        });
        $("#" + SearchProductOrder.GRID_LB).on(Common.GetWheelEvent(), function (e) {
            var gridLB = $("#" + SearchProductOrder.GRID_LB);
            gridLB.scrollTop(gridLB.scrollTop() - Common.GetWheelDelta(e));
        });
        $("#" + SearchProductOrder.GRID_RB).on(Common.GetWheelEvent(), function (e) {
            var gridRB = $("#" + SearchProductOrder.GRID_RB);
            gridRB.scrollTop(gridRB.scrollTop() - Common.GetWheelDelta(e));
        });
    }
    ,
    // 大文字に変換
    toUpper: function (ele) {
        var textval = ele.value;
        textval = textval.toUpperCase();
        ele.value = textval;
    }
}