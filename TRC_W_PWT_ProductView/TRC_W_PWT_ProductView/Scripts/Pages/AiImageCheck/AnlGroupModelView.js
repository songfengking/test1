﻿///////////////////////////////////////////////////////////////////////////////////////////////
// 型式紐づくグループ検索画面(AnlGroupModelView)用制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
// 型式紐づくグループ検索画面派生呼び出し(PageLoad)
function MasterMainPageLoaded() {
    AnlGroupModelView.Initialize();
}
// メイン処理からの各画面派生呼び出し(Resize)
function MasterMainPageResized() {
    AnlGroupModelView.ResizeGridArea2();
}

AnlGroupModelView = {
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
    TXT_TEST: "MasterBody_txtComment"
    ,
    CONST_BUTTON_VIEW_DISP_TMP: "MasterBody_btnModalDispTmp"
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
        var gridArea = $("#" + AnlGroupModelView.GRID_MAIN_VIEW);
        //if (0 < gridArea.outerHeight(true)) {
        if (0 < gridArea.length) {
            //グリッドのDIVを高さ変更する
            if (null != gridArea.position()) {
                gridTop = gridArea.position().top;
            }
            try {
                gridSize = containtsBottom - gridTop - AnlGroupModelView.INNER_DIV_H_MARGIN;
                if (gridSize < AnlGroupModelView.MIN_HEIGHT) {
                    //gridSize = AnlGroupModelView.MIN_HEIGHT;
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
            containtsBottom = containtsBottomArea.position().top - containtsArea.position().top + AnlGroupModelView.MIN_HEIGHT;
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
                divMain_Height = containtsBottom - divMain_Top - AnlGroupModelView.INNER_DIV_H_MARGIN;
                gridLB.height(divMain_Height);
                gridRB.height(divMain_Height);
            } catch (e) {
            }
        }
    }
    ,
    Initialize: function () {
        $("#" + AnlGroupModelView.GRID_RB).scroll(function () {
            var gridRT = $("#" + AnlGroupModelView.GRID_RT);
            var gridLB = $("#" + AnlGroupModelView.GRID_LB);
            var gridRB = $("#" + AnlGroupModelView.GRID_RB);
            if (gridRB.scrollTop() != gridLB.scrollTop()) {
                gridLB.scrollTop(gridRB.scrollTop());
            }
            if (gridRB.scrollLeft() != gridRT.scrollLeft()) {
                gridRT.scrollLeft(gridRB.scrollLeft());
            }
        });
        $("#" + AnlGroupModelView.GRID_LB).scroll(function () {
            var gridLB = $("#" + AnlGroupModelView.GRID_LB);
            var gridRB = $("#" + AnlGroupModelView.GRID_RB);
            if (gridLB.scrollTop() != gridRB.scrollTop()) {
                gridRB.scrollTop(gridLB.scrollTop());
            }
        });
        $("#" + AnlGroupModelView.GRID_LB).on(Common.GetWheelEvent(), function (e) {
            var gridLB = $("#" + AnlGroupModelView.GRID_LB);
            gridLB.scrollTop(gridLB.scrollTop() - Common.GetWheelDelta(e));
        });
        $("#" + AnlGroupModelView.GRID_RB).on(Common.GetWheelEvent(), function (e) {
            var gridRB = $("#" + AnlGroupModelView.GRID_RB);
            gridRB.scrollTop(gridRB.scrollTop() - Common.GetWheelDelta(e));
        });
    }
    ,
    TxtWrite: function (selecedRow) {
        var tmpTxt = $("#" + this.TXT_TEST);
        $(tmpTxt).val(selecedRow);
        AnlGroupModelView.BthClick();
        return true;
    }
    ,
    BthClick: function () {
        parent.$("#" + this.CONST_BUTTON_VIEW_DISP_TMP).trigger('click');
        //ControlCommon.DoubleClickGridRow(0, 1, 'AnlGroupModelList.aspx', token);
        return true;
    }
}