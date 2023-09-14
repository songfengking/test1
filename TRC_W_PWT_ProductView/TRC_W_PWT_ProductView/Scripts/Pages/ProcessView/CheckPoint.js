///////////////////////////////////////////////////////////////////////////////////////////////
//詳細画面 電子チェックシート(ELCheckSheet)用コントロール制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
/* 電子チェックシートイメージ画像用制御 */
//メイン処理からの各画面派生呼び出し(PageLoad)
function DetailFramePageLoaded() {
    CheckPoint.Initialize();
    CheckPoint.SelectedRowAutoScroller();
}
//メイン処理からの各画面派生呼び出し(Resize)
function DetailFramePageResized() {
    CheckPoint.ResizeDivArea();
}
CheckPoint = {
    //定数
    DIV_BODY: "divDetailBodyScroll"
    ,
    DIV_LIST_AREA: "divDetailListArea"
    ,
    DIV_WORK_AREA: "divWorkArea"
    ,
    DIV_VIEW_BOX: "divDetailViewBox"
    ,
    DIV_PROCESS_AREA: "divProcessArea"
    ,
    PARENT_DIV_H_MARGIN: 2
    ,
    INNER_DIV_MARGIN_3: 3
    ,
    PARENT_DIV_W_MARGIN: 6
    ,
    INNER_DIV_W_MARGIN: 2
    ,
    INNER_DIV_H_MARGIN: 3
    ,
    GRID_PROCESS_LT: "divProcessLTScroll"
    ,
    GRID_PROCESS_RT: "divProcessRTScroll"
    ,
    GRID_PROCESS_LB: "divProcessLBScroll"
    ,
    GRID_PROCESS_RB: "divProcessRBScroll"
    ,
    DIFFERENCE_ROW: "cell-cnt-difference"
    ,
    ROW_OFFSET_TOP: 1
    ,

    ///画面リサイズ
    ResizeDivArea: function () {

        //詳細表示エリアDIVの位置を取得
        var containtsBottom = 0;
        var containtsWidth = 0;
        var divProcessArea = $("#" + this.DIV_PROCESS_AREA);
        var containtsArea = $("#" + DetailFrame.DIV_VIEW_BOX);
        if (null != containtsArea.position()) {
            containtsBottom = containtsArea.position().top + containtsArea.height();
            if (null == containtsBottom) {
                containtsBottom = 0;
            }
            containtsWidth = containtsArea.width();
            if (null == containtsWidth) {
                containtsWidth = 0;
            }
        }

        //Div高さサイズ
        var divHSize = 0;
        //Div幅サイズ
        var divWSize = 0;
        //Div縦位置
        var divTop = 0;
        //Div横位置
        var divLeft = 0;

        //グリッド用DIVサイズ変更
        var divParentArea = $("#" + this.DIV_BODY);
        var divListArea = $("#" + this.DIV_LIST_AREA);
        var divWorkArea = $("#" + this.DIV_WORK_AREA);
        var divDetailBoxArea = $("#" + this.DIV_VIEW_BOX);
        var divProcessArea = $("#" + this.DIV_PROCESS_AREA);

        //上位DIVを高さ変更
        if (0 < divParentArea.length) {
            //親DIVを幅変更する
            try {
                divWSize = containtsWidth - this.PARENT_DIV_W_MARGIN;
                divParentArea.width(divWSize);
            } catch (e) {
            }
            //詳細側DIVを幅変更する
            if (null != divListArea.position()) {
                divLeft = divListArea.width();
            }
            try {
                divWSize = containtsWidth - divLeft - this.PARENT_DIV_W_MARGIN - this.INNER_DIV_W_MARGIN;
                divWorkArea.width(divWSize);
            } catch (e) {
            }
            //親DIVを高さ変更する
            if (null != divParentArea.position()) {
                divTop = divParentArea.position().top;
            }
            try {
                divHSize = containtsBottom - divTop - this.INNER_DIV_H_MARGIN;
                divParentArea.height(divHSize);
            } catch (e) {
            }
            //リスト側、詳細側DIVを高さ変更する
            if (null != divListArea.position()) {
                divTop = divListArea.position().top;
            }
            try {
                divHSize = containtsBottom - divTop - this.INNER_DIV_H_MARGIN;
                divListArea.height(divHSize);
                divWorkArea.height(divHSize);
            } catch (e) {
            }
            //内部DIVサイズを変更する
            //詳細子画面表示欄
            if (null != divDetailBoxArea.position()) {
                divTop = divDetailBoxArea.position().top;
            }
            try {
                divHSize = containtsBottom - divTop - this.INNER_DIV_H_MARGIN;
                divDetailBoxArea.height(divHSize);
            } catch (e) {
            }
        }

        //ウィンドウ幅
        var windowWidth = $(document).innerWidth();

        //Div高さサイズ
        var divHSize = 0;
        //Div幅サイズ
        var divWSize = 0;
        //Div縦位置
        var divTop = 0;
        //Div横位置
        var divLeft = 0;

        ///メインタブ
        var gridLTMain = $("#" + this.GRID_PROCESS_LT);
        var gridRTMain = $("#" + this.GRID_PROCESS_RT);
        var gridLBMain = $("#" + this.GRID_PROCESS_LB);
        var gridRBMain = $("#" + this.GRID_PROCESS_RB);

        if (0 < gridLTMain.length) {

            //グリッドのDIVを幅変更する
            if (null != gridLTMain.position()) {
                divR_Left = gridLTMain.position().left + gridLTMain.width();
            }

            try {
                tblHeaders_Width = windowWidth - this.PARENT_DIV_W_MARGIN;
                divR_Width = tblHeaders_Width - divR_Left + CheckPoint.INNER_DIV_MARGIN_3;
                gridRTMain.width(divR_Width);
                gridRBMain.width(divR_Width);
            } catch (e) {
            }

            try {
                divMain_Height = divParentArea.position().top - gridRTMain.position().top - 30;
            } catch (e) {
            }
        }
    }
    ,

    //選択行連動
    Initialize: function () {

        //メインタブ
        $("#" + CheckPoint.GRID_PROCESS_RB).scroll(function () {
            var gridRTMain = $("#" + CheckPoint.GRID_PROCESS_RT);
            var gridLBMain = $("#" + CheckPoint.GRID_PROCESS_LB);
            var gridRBMain = $("#" + CheckPoint.GRID_PROCESS_RB);
            if (gridRBMain.scrollTop() != gridLBMain.scrollTop()) {
                gridLBMain.scrollTop(gridRBMain.scrollTop());
            }
            if (gridRBMain.scrollLeft() != gridRTMain.scrollLeft()) {
                gridRTMain.scrollLeft(gridRBMain.scrollLeft());
            }
        });
        $("#" + CheckPoint.GRID_PROCESS_LB).scroll(function () {
            var gridLBMain = $("#" + CheckPoint.GRID_PROCESS_LB);
            var gridRBMain = $("#" + CheckPoint.GRID_PROCESS_RB);
            if (gridLBMain.scrollTop() != gridRBMain.scrollTop()) {
                gridRBMain.scrollTop(gridLBMain.scrollTop());
            }
        });
        $("#" + CheckPoint.GRID_PROCESS_LB).on(Common.GetWheelEvent(), function (e) {
            var gridLBMain = $("#" + CheckPoint.GRID_PROCESS_LB);
            gridLBMain.scrollTop(gridLBMain.scrollTop() - Common.GetWheelDelta(e));
        });
        $("#" + CheckPoint.GRID_PROCESS_RB).on(Common.GetWheelEvent(), function (e) {
            var gridRBMain = $("#" + CheckPoint.GRID_PROCESS_RB);
            gridRBMain.scrollTop(gridRBMain.scrollTop() - Common.GetWheelDelta(e));
        });
    }
    ,
 
    //メインビュー行選択(メイン)
    SelectMainViewRow: function (ele, index, selectBtnId) {

        var gridview = $(ele).closest("." + ControlCommon.GRID_VIEW);
        var preSelect = $(gridview).find("." + ControlCommon.SELECT_ROW);
        var gridrows = $(gridview).find("." + ControlCommon.LIST_VIEW_ROW);

        var selectRow = $(ele);

        if (0 < preSelect.length
            && 0 < selectRow.length
            && preSelect[0] == selectRow[0]) {
            return;
        }
        var blLR = false;
        if (0 < selectRow[0].id.indexOf("listProcessRB")) {
            blLR = true;
        }

        if (0 < selectRow.length) {
            //選択行を全てクリア
            var listItems = $("#divProcessArea").find(".listview-row");
            $(listItems).removeClass(ControlCommon.SELECT_ROW);
            $(selectRow).removeClass(CheckPoint.DIFFERENCE_ROW);
            $(selectRow).addClass(ControlCommon.SELECT_ROW);

            if (true == blLR) {
                //連動させる側が左
                var tmp = $(selectRow)[0].id;
                tmp = tmp.replace("listProcessRB", "listProcessLB");
                var obj = $("#" + tmp);
                $(obj).removeClass(CheckPoint.DIFFERENCE_ROW);
                $(obj).addClass(ControlCommon.SELECT_ROW);
            } else {
                //連動させる側が右
                var tmp = $(selectRow)[0].id;
                tmp = tmp.replace("listProcessLB", "listProcessRB");
                var obj = $("#" + tmp);
                $(obj).removeClass(CheckPoint.DIFFERENCE_ROW);
                $(obj).addClass(ControlCommon.SELECT_ROW);
            }

            if (null != selectBtnId && undefined != selectBtnId && "" != selectBtnId) {
                __doPostBack(selectBtnId, '');
            }
        }
    }
    ,
    SelectedRowAutoScroller: function () {
        var gridLBMain = $("#" + CheckPoint.GRID_PROCESS_LB);
        var gridRBMain = $("#" + CheckPoint.GRID_PROCESS_RB);
        var selectLBMain = gridLBMain.find("." + ControlCommon.SELECT_ROW);
        var selectRBMain = gridRBMain.find("." + ControlCommon.SELECT_ROW);
        if (0 < selectLBMain.length) {
            if (selectLBMain[0].offsetTop == CheckPoint.ROW_OFFSET_TOP) {
                gridLBMain.animate({ scrollTop: 0 });
            } else {
                gridLBMain.animate({ scrollTop: selectLBMain[0].offsetTop });
            }
        }
        if (0 < selectRBMain.length) {
            if (selectRBMain[0].offsetTop == CheckPoint.ROW_OFFSET_TOP) {
                gridRBMain.animate({ scrollTop: 0 });
            } else {
                gridRBMain.animate({ scrollTop: selectRBMain[0].offsetTop });
            }
        }
    }
}

