///////////////////////////////////////////////////////////////////////////////////////////////
//詳細画面 エンジン運転測定(03エンジン EngineTest03)用コントロール制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
//メイン処理からの各画面派生呼び出し(PageLoad)
function DetailFramePageLoaded() {
    EngineTest03.Initialize();
}
//メイン処理からの各画面派生呼び出し(Resize)
function DetailFramePageResized() {
    EngineTest03.ResizeDivArea();
}

EngineTest03 = {
    //定数
    DIV_BODY: "divDetailBodyScroll"
    ,
    DIV_MAIN_AREA: "divMainListArea"
    ,
    DIV_SUB_AREA: "divSubListArea"
    ,
    PARENT_DIV_W_MARGIN: 6
    ,
    INNER_DIV_W_MARGIN: 2
    ,
    INNER_DIV_MARGIN_3: 3
    ,
    INNER_DIV_MARGIN_40: 40
    ,
    GRID_LT: "divLTScroll"
    ,
    GRID_RT: "divRTScroll"
    ,
    GRID_LB: "divLBScroll"
    ,
    GRID_RB: "divRBScroll"
    ,
    SUB_GRID_LT: "divLTSubScroll"
    ,
    SUB_GRID_RT: "divRTSubScroll"
    ,
    SUB_GRID_LB: "divLBSubScroll"
    ,
    SUB_GRID_RB: "divRBSubScroll"
    ,
    ResizeDivArea: function () {

        //詳細表示エリアDIVの位置を取得
        var containtsBottom = 0;
        var containtsWidth = 0;
        var containtsArea = $("#" + DetailFrame.DIV_VIEW_BOX);
        var containtsBottomArea = $("#" + MasterMain.CONTAINTS_AREA_BOTTOM);
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
        var divMainArea = $("#" + this.DIV_MAIN_AREA);
        var divSubArea = $("#" + this.DIV_SUB_AREA);

        //上位DIVを高さ変更
        if (0 < divParentArea.length) {
            //親DIVを幅変更する
            try {
                divWSize = containtsWidth - this.PARENT_DIV_W_MARGIN;
                divParentArea.width(divWSize);
            } catch (e) {
            }
            //内部DIVを幅変更する
            try {
                divWSize = divWSize - this.INNER_DIV_W_MARGIN;
                divMainArea.width(divWSize);
//                divSubArea.width(divWSize);
            } catch (e) {
            }
            //親DIVを高さ変更する
            if (null != divParentArea.position()) {
                divTop = divParentArea.position().top;
            }
            try {
                divHSize = containtsBottom - divTop - this.INNER_DIV_MARGIN_3;
                divParentArea.height(divHSize);
            } catch (e) {
            }
            //サブ側DIVを高さ変更する
            if (null != divSubArea.position()) {
                divTop = divSubArea.position().top;
            }
            try {
                divHSize = containtsBottom - divTop - this.INNER_DIV_MARGIN_3;
//                divSubArea.height(divHSize);
            } catch (e) {
            }
        }

        //固定対応
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

        //グリッド用DIVサイズ変更
        var divParentArea = $("#" + this.DIV_BODY);
        var divMainArea = $("#" + this.DIV_MAIN_AREA);
        var divSubArea = $("#" + this.DIV_SUB_AREA);
        //グリッド用DIVサイズ変更
        var gridLT = $("#" + this.GRID_LT);
        var gridRT = $("#" + this.GRID_RT);
        var gridLB = $("#" + this.GRID_LB);
        var gridRB = $("#" + this.GRID_RB);

        var gridSubLT = $("#" + EngineTest03.SUB_GRID_LT);
        var gridSubRT = $("#" + EngineTest03.SUB_GRID_RT);
        var gridSubLB = $("#" + EngineTest03.SUB_GRID_LB);
        var gridSubRB = $("#" + EngineTest03.SUB_GRID_RB);


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

            //mainグリッドのDIVを高さ変更する
            if (null != gridLB.position()) {
                divTop = gridLB.position().top;
            }
            try {
                divHSize = gridSubLT.position().top - divTop - this.INNER_DIV_MARGIN_40;
                gridLB.height(divHSize);
                gridRB.height(divHSize);
            } catch (e) {
            }
        }



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

        if (0 < gridSubLT.length) {

            //グリッドのDIVを幅変更する
            if (null != gridSubLT.position()) {
                divR_Left = gridSubLT.position().left + gridSubLT.width();
            }

            try {
                tblHeaders_Width = windowWidth - this.PARENT_DIV_W_MARGIN;
                divR_Width = tblHeaders_Width - divR_Left;
                gridSubRT.width(divR_Width);
                gridSubRB.width(divR_Width);
            } catch (e) {
            }

            //グリッドのDIVを高さ変更する
            if (null != gridSubLB.position()) {
                divMain_Top = gridSubLB.position().top;
            }
            try {
                divMain_Height = containtsBottomArea.position().top - divMain_Top - 20;
                gridSubLB.height(divMain_Height);
                gridSubRB.height(divMain_Height);
            } catch (e) {
            }
        }
    }
    ,
    Initialize: function () {

        //メイン
        $("#" + EngineTest03.GRID_RB).scroll(function () {
            var gridRT = $("#" + EngineTest03.GRID_RT);
            var gridLB = $("#" + EngineTest03.GRID_LB);
            var gridRB = $("#" + EngineTest03.GRID_RB);
            if (gridRB.scrollTop() != gridLB.scrollTop()) {
                gridLB.scrollTop(gridRB.scrollTop());
            }
            if (gridRB.scrollLeft() != gridRT.scrollLeft()) {
                gridRT.scrollLeft(gridRB.scrollLeft());
            }
        });
        $("#" + EngineTest03.GRID_LB).scroll(function () {
            var gridLB = $("#" + EngineTest03.GRID_LB);
            var gridRB = $("#" + EngineTest03.GRID_RB);
            if (gridLB.scrollTop() != gridRB.scrollTop()) {
                gridRB.scrollTop(gridLB.scrollTop());
            }
        });
        $("#" + EngineTest03.GRID_LB).on(Common.GetWheelEvent(), function (e) {
            var gridLB = $("#" + EngineTest03.GRID_LB);
            gridLB.scrollTop(gridLB.scrollTop() - Common.GetWheelDelta(e));
        });
        $("#" + EngineTest03.GRID_RB).on(Common.GetWheelEvent(), function (e) {
            var gridRB = $("#" + EngineTest03.GRID_RB);
            gridRB.scrollTop(gridRB.scrollTop() - Common.GetWheelDelta(e));
        });

        //サブ
        $("#" + EngineTest03.SUB_GRID_RB).scroll(function () {
            var gridRT = $("#" + EngineTest03.SUB_GRID_RT);
            var gridLB = $("#" + EngineTest03.SUB_GRID_LB);
            var gridRB = $("#" + EngineTest03.SUB_GRID_RB);
            if (gridRB.scrollTop() != gridLB.scrollTop()) {
                gridLB.scrollTop(gridRB.scrollTop());
            }
            if (gridRB.scrollLeft() != gridRT.scrollLeft()) {
                gridRT.scrollLeft(gridRB.scrollLeft());
            }
        });
        $("#" + EngineTest03.SUB_GRID_LB).scroll(function () {
            var gridLB = $("#" + EngineTest03.SUB_GRID_LB);
            var gridRB = $("#" + EngineTest03.SUB_GRID_RB);
            if (gridLB.scrollTop() != gridRB.scrollTop()) {
                gridRB.scrollTop(gridLB.scrollTop());
            }
        });
        $("#" + EngineTest03.SUB_GRID_LB).on(Common.GetWheelEvent(), function (e) {
            var gridLB = $("#" + EngineTest03.SUB_GRID_LB);
            gridLB.scrollTop(gridLB.scrollTop() - Common.GetWheelDelta(e));
        });
        $("#" + EngineTest03.SUB_GRID_RB).on(Common.GetWheelEvent(), function (e) {
            var gridRB = $("#" + EngineTest03.SUB_GRID_RB);
            gridRB.scrollTop(gridRB.scrollTop() - Common.GetWheelDelta(e));
        });
    }
    ,
    //リストビュー行選択
    SelectListViewRow: function (ele, index, selectBtnId) {

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
        if (0 < selectRow[0].id.indexOf("lstMainListLB")) {
            blLR = true;
        }

        if (0 < selectRow.length) {
            //選択行を全てクリア
            var listItems = $("#divMainListArea").find(".listview-row");
            $(listItems).removeClass(ControlCommon.SELECT_ROW);
            $(selectRow).addClass(ControlCommon.SELECT_ROW);

            if (true == blLR) {
                //連動させる側が左
                var tmp = $(selectRow)[0].id;
                tmp = tmp.replace("lstMainListLB", "lstMainListRB");
                var obj = $("#" + tmp);
                $(obj).addClass(ControlCommon.SELECT_ROW);
            } else {
                //連動させる側が右
                var tmp = $(selectRow)[0].id;
                tmp = tmp.replace("lstMainListRB", "lstMainListLB");
                var obj = $("#" + tmp);
                $(obj).addClass(ControlCommon.SELECT_ROW);
            }

            if (null != selectBtnId && undefined != selectBtnId && "" != selectBtnId) {
                __doPostBack(selectBtnId, '');
            }
        }
    }
    ,
    //サブリストビュー行選択
    SelectSubListViewRow: function (ele, index, selectBtnId) {

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
        if (0 < selectRow[0].id.indexOf("lstSubListLB")) {
            blLR = true;
        }

        if (0 < selectRow.length) {
            //選択行を全てクリア
            var listItems = $("#divSubListArea").find(".listview-row");
            $(listItems).removeClass(ControlCommon.SELECT_ROW);
            $(selectRow).addClass(ControlCommon.SELECT_ROW);

            if (true == blLR) {
                //連動させる側が左
                var tmp = $(selectRow)[0].id;
                tmp = tmp.replace("lstSubListLB", "lstSubListRB");
                var obj = $("#" + tmp);
                $(obj).addClass(ControlCommon.SELECT_ROW);
            } else {
                //連動させる側が右
                var tmp = $(selectRow)[0].id;
                tmp = tmp.replace("lstSubListRB", "lstSubListLB");
                var obj = $("#" + tmp);
                $(obj).addClass(ControlCommon.SELECT_ROW);
            }

            if (null != selectBtnId && undefined != selectBtnId && "" != selectBtnId) {
                __doPostBack(selectBtnId, '');
            }
        }
    }
}