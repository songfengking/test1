///////////////////////////////////////////////////////////////////////////////////////////////
//詳細画面 トラクタ刻印用コントロール制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
//メイン処理からの各画面派生呼び出し(PageLoad)
function DetailFramePageLoaded() {
    Sheel.Initialize();
}
//メイン処理からの各画面派生呼び出し(Resize)
function DetailFramePageResized() {
    Sheel.ResizeDivArea();
}

Sheel = {
    //定数
    PARENT_DIV_W_MARGIN: 6
    ,
    INNER_DIV_W_MARGIN: 2
    ,
    INNER_DIV_H_MARGIN: 3
    ,
    GRID_LT: "divLTScroll"
    ,
    GRID_RT: "divRTScroll"
    ,
    GRID_LB: "divLBScroll"
    ,
    GRID_RB: "divRBScroll"
    ,
    GRID_LT_MS: "divLTScroll_MS"
    ,
    GRID_RT_MS: "divRTScroll_MS"
    ,
    GRID_LB_MS: "divLBScroll_MS"
    ,
    GRID_RB_MS: "divRBScroll_MS"
    ,
    GRID_LT_MID: "divLTScroll_MID"
    ,
    GRID_RT_MID: "divRTScroll_MID"
    ,
    GRID_LB_MID: "divLBScroll_MID"
    ,
    GRID_RB_MID: "divRBScroll_MID"
    ,
    ResizeDivArea: function () {

        //詳細表示エリアDIVの位置を取得
        var containtsBottom = 0;
        var containtsWidth = 0;
        var containtsArea = $("#" + DetailFrame.DIV_VIEW_BOX);
        var containtsBottomArea = $("#" + MasterMain.CONTAINTS_AREA_BOTTOM);
        if (null != containtsArea.position()) {
            //            containtsBottom = containtsArea.position().top + containtsArea.height();
            containtsBottom = containtsBottomArea.position().top - containtsArea.position().top + 100;

            if (null == containtsBottom) {
                containtsBottom = 0;
            }
            containtsWidth = containtsArea.width();
            if (null == containtsWidth) {
                containtsWidth = 0;
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

        //グリッド用DIVサイズ変更
        var gridLT = $("#" + Sheel.GRID_LT);
        var gridRT = $("#" + Sheel.GRID_RT);
        var gridLB = $("#" + Sheel.GRID_LB);
        var gridRB = $("#" + Sheel.GRID_RB);

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
        }


        //ミッションケース、MIDケース
        //グリッド用DIVサイズ変更
        var gridLT_MS = $("#" + Sheel.GRID_LT_MS);
        var gridRT_MS = $("#" + Sheel.GRID_RT_MS);
        var gridRB_MS = $("#" + Sheel.GRID_RB_MS);

        var gridLT_MID = $("#" + Sheel.GRID_LT_MID);
        var gridRT_MID = $("#" + Sheel.GRID_RT_MID);
        var gridRB_MID = $("#" + Sheel.GRID_RB_MID);

        if (0 < gridLT_MS.length) {

            //グリッドのDIVを幅変更する
            if (null != gridLT_MS.position()) {
                divR_Left = gridLT_MS.position().left + gridLT_MS.width();
            }

            try {
                tblHeaders_Width = windowWidth - this.PARENT_DIV_W_MARGIN;
                divR_Width = tblHeaders_Width - divR_Left;
                gridRT_MS.width(divR_Width);
                gridRB_MS.width(divR_Width);
                gridRT_MID.width(divR_Width);
                gridRB_MID.width(divR_Width);
            } catch (e) {
            }
        }


    }
    ,
    Initialize: function () {
        //前車軸フレーム
        $("#" + Sheel.GRID_RB).scroll(function () {
            var gridRT = $("#" + Sheel.GRID_RT);
            var gridLB = $("#" + Sheel.GRID_LB);
            var gridRB = $("#" + Sheel.GRID_RB);
            if (gridRB.scrollTop() != gridLB.scrollTop()) {
                gridLB.scrollTop(gridRB.scrollTop());
            }
            if (gridRB.scrollLeft() != gridRT.scrollLeft()) {
                gridRT.scrollLeft(gridRB.scrollLeft());
            }
        });
        $("#" + Sheel.GRID_LB).scroll(function () {
            var gridLB = $("#" + Sheel.GRID_LB);
            var gridRB = $("#" + Sheel.GRID_RB);
            if (gridLB.scrollTop() != gridRB.scrollTop()) {
                gridRB.scrollTop(gridLB.scrollTop());
            }
        });
        $("#" + Sheel.GRID_LB).on(Common.GetWheelEvent(), function (e) {
            var gridLB = $("#" + Sheel.GRID_LB);
            gridLB.scrollTop(gridLB.scrollTop() - Common.GetWheelDelta(e));
        });
        $("#" + Sheel.GRID_RB).on(Common.GetWheelEvent(), function (e) {
            var gridRB = $("#" + Sheel.GRID_RB);
            gridRB.scrollTop(gridRB.scrollTop() - Common.GetWheelDelta(e));
        });

        //ミッションケース
        $("#" + Sheel.GRID_RB_MS).scroll(function () {
            var gridRT_MS = $("#" + Sheel.GRID_RT_MS);
            var gridLB_MS = $("#" + Sheel.GRID_LB_MS);
            var gridRB_MS = $("#" + Sheel.GRID_RB_MS);
            if (gridRB_MS.scrollTop() != gridLB_MS.scrollTop()) {
                gridLB_MS.scrollTop(gridRB_MS.scrollTop());
            }
            if (gridRB_MS.scrollLeft() != gridRT_MS.scrollLeft()) {
                gridRT_MS.scrollLeft(gridRB_MS.scrollLeft());
            }
        });
        $("#" + Sheel.GRID_LB_MS).scroll(function () {
            var gridLB_MS = $("#" + Sheel.GRID_LB_MS);
            var gridRB_MS = $("#" + Sheel.GRID_RB_MS);
            if (gridLB_MS.scrollTop() != gridRB_MS.scrollTop()) {
                gridRB_MS.scrollTop(gridLB_MS.scrollTop());
            }
        });
        $("#" + Sheel.GRID_LB_MS).on(Common.GetWheelEvent(), function (e) {
            var gridLB_MS = $("#" + Sheel.GRID_LB_MS);
            gridLB_MS.scrollTop(gridLB_MS.scrollTop() - Common.GetWheelDelta(e));
        });
        $("#" + Sheel.GRID_RB_MS).on(Common.GetWheelEvent(), function (e) {
            var gridRB_MS = $("#" + Sheel.GRID_RB_MS);
            gridRB_MS.scrollTop(gridRB_MS.scrollTop() - Common.GetWheelDelta(e));
        });
        //MIDケース
        $("#" + Sheel.GRID_RB_MID).scroll(function () {
            var gridRT_MID = $("#" + Sheel.GRID_RT_MID);
            var gridLB_MID = $("#" + Sheel.GRID_LB_MID);
            var gridRB_MID = $("#" + Sheel.GRID_RB_MID);
            if (gridRB_MID.scrollTop() != gridLB_MID.scrollTop()) {
                gridLB_MID.scrollTop(gridRB_MID.scrollTop());
            }
            if (gridRB_MID.scrollLeft() != gridRT_MID.scrollLeft()) {
                gridRT_MID.scrollLeft(gridRB_MID.scrollLeft());
            }
        });
        $("#" + Sheel.GRID_LB_MID).scroll(function () {
            var gridLB_MID = $("#" + Sheel.GRID_LB_MID);
            var gridRB_MID = $("#" + Sheel.GRID_RB_MID);
            if (gridLB_MID.scrollTop() != gridRB_MID.scrollTop()) {
                gridRB_MID.scrollTop(gridLB_MID.scrollTop());
            }
        });
        $("#" + Sheel.GRID_LB_MID).on(Common.GetWheelEvent(), function (e) {
            var gridLB_MID = $("#" + Sheel.GRID_LB_MID);
            gridLB_MID.scrollTop(gridLB_MID.scrollTop() - Common.GetWheelDelta(e));
        });
        $("#" + Sheel.GRID_RB_MID).on(Common.GetWheelEvent(), function (e) {
            var gridRB_MID = $("#" + Sheel.GRID_RB_MID);
            gridRB_MID.scrollTop(gridRB_MID.scrollTop() - Common.GetWheelDelta(e));
        });
    }
}