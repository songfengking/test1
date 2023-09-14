///////////////////////////////////////////////////////////////////////////////////////////////
// 画像解析グループ一覧画面(AnlGroupList)用制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
// 画像解析グループ一覧画面派生呼び出し(PageLoad)
function MasterMainPageLoaded() {
    AnlGroupList.Initialize();
}
// メイン処理からの各画面派生呼び出し(Resize)
function MasterMainPageResized() {
    AnlGroupList.ResizeGridArea2();
}

AnlGroupList = {
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
    TXT_TEST: "MasterBody_txtCorrectiveComment"
    ,
    TH_JYOHO: "thJyoho1"
    ,
    TD_JYOHO: "tdJyoho"
    ,
    IDX: "MasterBody_hiddenNum"
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
        var gridArea = $("#" + AnlGroupList.GRID_MAIN_VIEW);
        //if (0 < gridArea.outerHeight(true)) {
        if (0 < gridArea.length) {
            //グリッドのDIVを高さ変更する
            if (null != gridArea.position()) {
                gridTop = gridArea.position().top;
            }
            try {
                gridSize = containtsBottom - gridTop - AnlGroupList.INNER_DIV_H_MARGIN;
                if (gridSize < AnlGroupList.MIN_HEIGHT) {
                    //gridSize = AnlGroupList.MIN_HEIGHT;
                }
                gridArea.height(gridSize);
            } catch (e) {
            }
        }
    }
    ,
    ResizeGridArea2: function () {
        //$("#" + this.TXT_TEST).text("88888888");
        //コンテンツタグの位置を取得
        var containtsBottom = 0;
        var containtsArea = $("#" + MasterMain.CONTAINTS_AREA);
        var containtsBottomArea = $("#" + MasterMain.CONTAINTS_AREA_BOTTOM);
        if (null != containtsArea.position()) {
            containtsBottom = containtsBottomArea.position().top - containtsArea.position().top + AnlGroupList.MIN_HEIGHT;
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
                divMain_Height = containtsBottom - divMain_Top - AnlGroupList.INNER_DIV_H_MARGIN;
                gridLB.height(divMain_Height);
                gridRB.height(divMain_Height);
            } catch (e) {
            }
        }
    }
    ,
    Initialize: function () {
        $("#" + AnlGroupList.GRID_RB).scroll(function () {
            var gridRT = $("#" + AnlGroupList.GRID_RT);
            var gridLB = $("#" + AnlGroupList.GRID_LB);
            var gridRB = $("#" + AnlGroupList.GRID_RB);
            if (gridRB.scrollTop() != gridLB.scrollTop()) {
                gridLB.scrollTop(gridRB.scrollTop());
            }
            if (gridRB.scrollLeft() != gridRT.scrollLeft()) {
                gridRT.scrollLeft(gridRB.scrollLeft());
            }
        });
        $("#" + AnlGroupList.GRID_LB).scroll(function () {
            var gridLB = $("#" + AnlGroupList.GRID_LB);
            var gridRB = $("#" + AnlGroupList.GRID_RB);
            if (gridLB.scrollTop() != gridRB.scrollTop()) {
                gridRB.scrollTop(gridLB.scrollTop());
            }
        });
        $("#" + AnlGroupList.GRID_LB).on(Common.GetWheelEvent(), function (e) {
            var gridLB = $("#" + AnlGroupList.GRID_LB);
            gridLB.scrollTop(gridLB.scrollTop() - Common.GetWheelDelta(e));
        });
        $("#" + AnlGroupList.GRID_RB).on(Common.GetWheelEvent(), function (e) {
            var gridRB = $("#" + AnlGroupList.GRID_RB);
            gridRB.scrollTop(gridRB.scrollTop() - Common.GetWheelDelta(e));
        });
    }
    ,
    TxtWrite: function (txtGroupCd) {
        var tmpTxt = $("#" + this.TXT_TEST);
        $(tmpTxt).val(txtGroupCd);
        return false;
    }
    ,
    RefreshColumn: function (colId, colNm, colVals) {
        var tmpCol = $("#MasterBody_lJyoho" + colId);
        var tmpColVal = $("#MasterBody_txtJyoho" + colId);
        $(tmpCol).text(colNm);
        $(tmpColVal).val(colVals);
        return false;
    }
    ,
    setDisplay: function () {
        var idx = $("#" + this.IDX).val();
        for (let i = 1; i <= idx; i++) {
            var thJyoho = $("#" + this.TH_JYOHO + i);
            var tdJyoho = $("#" + this.TD_JYOHO + i);
            thJyoho.removeClass('print-show');
            tdJyoho.removeClass('print-show');
        }
        return false;
    }
    ,
    setFocus: function (colId) {
        var tmpCol = $("#MasterBody_" + colId);
        $(tmpCol).focus();
        return false;
    }
    ,
    AllCheck: function () {
        var allCheckState = $('#' + 'MasterBody_allCheckState');
        var allCheckStateVal = $(allCheckState).val()
        if (allCheckStateVal == 'false') {
            $("input[type='checkbox']").prop("checked", true);
            $(allCheckState).val('true');
        } else {
            $("input[type='checkbox']").prop("checked", false);
            $(allCheckState).val('false');
        }
        return false;
    }
}