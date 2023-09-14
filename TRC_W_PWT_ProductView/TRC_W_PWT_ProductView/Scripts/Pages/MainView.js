///////////////////////////////////////////////////////////////////////////////////////////////
//メイン画面(MainView)用制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
//メイン処理からの各画面派生呼び出し(PageLoad)
function MasterMainPageLoaded() {
    MainView.Initialize();
}
//メイン処理からの各画面派生呼び出し(Resize)
function MasterMainPageResized() {
    //    MainView.ResizeGridArea();
    MainView.ResizeGridArea2();
}

MainView = {
    //定数
    CSS_DIV: "div-auto-scroll"
    ,
    GRID_MAIN_VIEW: "divMainViewScroll"
    ,
    MIN_HEIGHT: 70
    ,
    PARENT_DIV_W_MARGIN: 6
    ,
    INNER_DIV_W_MARGIN: 2
    ,
    INNER_DIV_H_MARGIN: 3
    ,
    PRODUCT_KIND_NUM: 3
    ,
    GRID_LT: "divLTScroll"
    ,
    GRID_RT: "divRTScroll"
    ,
    GRID_LB: "divLBScroll"
    ,
    GRID_RB: "divRBScroll"
    ,
    // 非表示ラインコードID
    HIDDENFIELD_LINE_CD: "MasterBody_hdnLineCd"
    ,
    // 非表示工程コードID
    HIDDENFIELD_PROCESS_CD: "MasterBody_hdnProcessCd"
    ,
    // 非表示作業コードID
    HIDDENFIELD_WORK_CD: "MasterBody_hdnWorkCd"
    ,
    // 工程名ID
    TEXT_PROCESS_NM: "MasterBody_txtProcessNm"
    ,
    // 非表示工程名ID
    HIDDENFIELD_PROCESS_NM: "MasterBody_hdnProcessNm"
    ,
    // 作業名ID
    TEXT_WORK_NM: "MasterBody_txtWorkNm"
    ,
    // 非表示作業名ID
    HIDDENFIELD_WORK_NM: "MasterBody_hdnWorkNm"
    ,
    // 非表示検索対象フラグID
    HIDDENFIELD_SEARCH_TARGET_FLG: "MasterBody_hdnSearchTargetFlg"
    ,
    // 親画面：工程変更ボタン
    PARENT_CHANGE_PROCESS: "MasterBody_btnChangeProcess"
    ,
    PRODUCT_KIND: "MasterBody_rblProductKind_"
    ,

    ResizeGridArea: function () {

        //コンテンツタグの位置を取得
        var containtsBottom = 0;
        var containtsArea = $("#" + MasterMain.CONTAINTS_AREA);
        if (null != containtsArea.position()) {
            containtsBottom = containtsArea.position().top + containtsArea.height();
            if (null == containtsBottom) {
                containtsBottom = 0;
            }
        }

        //グリッドサイズ
        var gridSize = 0;
        //グリッド位置
        var gridTop = 0;

        //グリッド用DIVサイズ変更
        var gridArea = $("#" + MainView.GRID_MAIN_VIEW);
        //if (0 < gridArea.outerHeight(true)) {
        if (0 < gridArea.length) {
            //グリッドのDIVを高さ変更する
            if (null != gridArea.position()) {
                gridTop = gridArea.position().top;
            }
            try {
                gridSize = containtsBottom - gridTop - MainView.INNER_DIV_H_MARGIN;
                if (gridSize < MainView.MIN_HEIGHT) {
                    //gridSize = MainView.MIN_HEIGHT;
                }
                gridArea.height(gridSize);
            } catch (e) {
            }
        }
    }
    ,
    ResizeGridArea2: function () {

        //コンテンツタグの位置を取得
        var containtsBottom = 0;
        var containtsArea = $("#" + MasterMain.CONTAINTS_AREA);
        var containtsBottomArea = $("#" + MasterMain.CONTAINTS_AREA_BOTTOM);
        if (null != containtsArea.position()) {
            containtsBottom = containtsBottomArea.position().top - containtsArea.position().top + MainView.MIN_HEIGHT;
            if (null == containtsBottom) {
                containtsBottom = 0;
            }
        }

        //ウィンドウ幅
        var windowWidth = $(document).innerWidth();

        //グリッド用DIVサイズ変更
        var gridLT = $("#" + this.GRID_LT);
        var gridRT = $("#" + this.GRID_RT);
        var gridLB = $("#" + this.GRID_LB);
        var gridRB = $("#" + this.GRID_RB);

        //Table Header幅
        var tblHeaders_Width = 0;

        //Div(右)幅
        var divR_Width = 0;
        //Div(右)位置(横)
        var divR_Left = 0;

        //Div(下)位置(縦)
        var divMain_Top = 0;
        //Div(下)高さ
        var divMain_Height = 0;

        if (0 < gridLT.length) {

            //グリッドのDIVを幅変更する
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

            //グリッドのDIVを高さ変更する
            if (null != gridLB.position()) {
                divMain_Top = gridLB.position().top;
            }
            try {
                divMain_Height = containtsBottom - divMain_Top - MainView.INNER_DIV_H_MARGIN;
                gridLB.height(divMain_Height);
                gridRB.height(divMain_Height);
            } catch (e) {
            }
        }
    }
    ,
    Initialize: function () {
        $("#" + MainView.GRID_RB).scroll(function () {
            var gridRT = $("#" + MainView.GRID_RT);
            var gridLB = $("#" + MainView.GRID_LB);
            var gridRB = $("#" + MainView.GRID_RB);
            if (gridRB.scrollTop() != gridLB.scrollTop()) {
                gridLB.scrollTop(gridRB.scrollTop());
            }
            if (gridRB.scrollLeft() != gridRT.scrollLeft()) {
                gridRT.scrollLeft(gridRB.scrollLeft());
            }
        });
        $("#" + MainView.GRID_LB).scroll(function () {
            var gridLB = $("#" + MainView.GRID_LB);
            var gridRB = $("#" + MainView.GRID_RB);
            if (gridLB.scrollTop() != gridRB.scrollTop()) {
                gridRB.scrollTop(gridLB.scrollTop());
            }
        });
        $("#" + MainView.GRID_LB).on(Common.GetWheelEvent(), function (e) {
            var gridLB = $("#" + MainView.GRID_LB);
            gridLB.scrollTop(gridLB.scrollTop() - Common.GetWheelDelta(e));
        });
        $("#" + MainView.GRID_RB).on(Common.GetWheelEvent(), function (e) {
            var gridRB = $("#" + MainView.GRID_RB);
            gridRB.scrollTop(gridRB.scrollTop() - Common.GetWheelDelta(e));
        });
    }
    ,
    // 工程絞込検索表示
    ShowProcessFilteringView: function () {
        var productKind;

        for (var i = 0; i < this.PRODUCT_KIND_NUM; i++) {
            if (true == $("#" + this.PRODUCT_KIND + i)[0].checked) {
                productKind = $("#" + this.PRODUCT_KIND + i)[0].value;
                break;
            }
        }

        window.open("ProcessFilteringView.aspx?productKind=" + productKind, null, "menubar=0,toolbar=0,status=0");
    }
    ,
    // 工程・作業クリア
    ClearProcessAndWork: function () {
        $("#" + this.HIDDENFIELD_LINE_CD)[0].value = "";
        $("#" + this.HIDDENFIELD_PROCESS_CD)[0].value = "";
        $("#" + this.HIDDENFIELD_WORK_CD)[0].value = "";
        $("#" + this.TEXT_PROCESS_NM)[0].value = "";
        $("#" + this.HIDDENFIELD_PROCESS_NM)[0].value = "";
        $("#" + this.TEXT_WORK_NM)[0].value = "";
        $("#" + this.HIDDENFIELD_WORK_NM)[0].value = "";
        $("#" + this.HIDDENFIELD_SEARCH_TARGET_FLG)[0].value = "";
        $("#" + this.PARENT_CHANGE_PROCESS)[0].click();
    }
    ,
    // ページロード後に必ず実行する処理
    DoAfterLoad: function () {
        $("#" + this.TEXT_PROCESS_NM)[0].value = $("#" + this.HIDDENFIELD_PROCESS_NM)[0].value;
        $("#" + this.TEXT_WORK_NM)[0].value = $("#" + this.HIDDENFIELD_WORK_NM)[0].value;
    }
}