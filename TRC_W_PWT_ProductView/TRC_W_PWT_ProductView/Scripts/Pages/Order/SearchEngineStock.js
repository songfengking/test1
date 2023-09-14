///////////////////////////////////////////////////////////////////////////////////////////////
// エンジン立体倉庫在庫検索画面(SearchEngineStock)用制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
// メイン処理からの各画面派生呼び出し(PageLoad)
function MasterMainPageLoaded() {
    SearchEngineStock.Initialize();
}
// メイン処理からの各画面派生呼び出し(Resize)
function MasterMainPageResized() {
    SearchEngineStock.ResizeGridArea2();
}

SearchEngineStock = {
    // 定数
    MIN_HEIGHT: 70,
    PARENT_DIV_W_MARGIN: 6,
    INNER_DIV_H_MARGIN: 3,
    GRID_LT: "divLTScroll",
    GRID_RT: "divRTScroll",
    GRID_LB: "divLBScroll",
    GRID_RB: "divRBScroll",
    GV_RB: "MasterBody_gvSearchEngineStockRB",
    TARGET_STR: "ui-state-highlight",
    DIALOG_URL: "ProCodeIdnoDialog.aspx",
    KATASHIKICD_URL_PRM: "?katashikiCd=",
    KATASHIKINM_URL_PRM: "&katashikiNm=",

    // メソッド
    ResizeGridArea2: function () {
        // コンテンツタグの位置を取得
        var containtsBottom = 0;
        var containtsArea = $("#" + MasterMain.CONTAINTS_AREA);
        var containtsBottomArea = $("#" + MasterMain.CONTAINTS_AREA_BOTTOM);
        if (null != containtsArea.position()) {
            containtsBottom = containtsBottomArea.position().top - containtsArea.position().top + SearchEngineStock.MIN_HEIGHT;
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
                divMain_Height = containtsBottom - divMain_Top - SearchEngineStock.INNER_DIV_H_MARGIN;
                gridLB.height(divMain_Height);
                gridRB.height(divMain_Height);
            } catch (e) {
            }
        }
    }
    ,
    Initialize: function () {
        $("#" + SearchEngineStock.GRID_RB).scroll(function () {
            var gridRT = $("#" + SearchEngineStock.GRID_RT);
            var gridLB = $("#" + SearchEngineStock.GRID_LB);
            var gridRB = $("#" + SearchEngineStock.GRID_RB);
            if (gridRB.scrollTop() != gridLB.scrollTop()) {
                gridLB.scrollTop(gridRB.scrollTop());
            }
            if (gridRB.scrollLeft() != gridRT.scrollLeft()) {
                gridRT.scrollLeft(gridRB.scrollLeft());
            }
        });
        $("#" + SearchEngineStock.GRID_LB).scroll(function () {
            var gridLB = $("#" + SearchEngineStock.GRID_LB);
            var gridRB = $("#" + SearchEngineStock.GRID_RB);
            if (gridLB.scrollTop() != gridRB.scrollTop()) {
                gridRB.scrollTop(gridLB.scrollTop());
            }
        });
        $("#" + SearchEngineStock.GRID_LB).on(Common.GetWheelEvent(), function (e) {
            var gridLB = $("#" + SearchEngineStock.GRID_LB);
            gridLB.scrollTop(gridLB.scrollTop() - Common.GetWheelDelta(e));
        });
        $("#" + SearchEngineStock.GRID_RB).on(Common.GetWheelEvent(), function (e) {
            var gridRB = $("#" + SearchEngineStock.GRID_RB);
            gridRB.scrollTop(gridRB.scrollTop() - Common.GetWheelDelta(e));
        });
    }
    ,
    dialogOpen: function () {
        var katashikiCd;
        var katashikiNm;
        // 一覧行をすべて取得
        var row = $("#" + SearchEngineStock.GV_RB)[0].rows;
        for (let idx = 0; idx < row.length; idx++) {
            // 行をすべて検査
            if (-1 < row[idx].className.indexOf(SearchEngineStock.TARGET_STR)) {
                // 選択行の場合、型式コードと型式名を取得
                katashikiCd = row[idx].childNodes[1].innerText;
                katashikiNm = row[idx].childNodes[3].innerText;
                break;
            }
        }
        // 遷移先URLの設定
        var url = SearchEngineStock.DIALOG_URL + SearchEngineStock.KATASHIKICD_URL_PRM + katashikiCd + SearchEngineStock.KATASHIKINM_URL_PRM + katashikiNm;
        window.open(url, null, "menubar=0,toolbar=0,status=0");
    },
}