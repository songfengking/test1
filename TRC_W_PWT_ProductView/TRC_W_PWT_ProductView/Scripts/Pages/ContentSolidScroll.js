///////////////////////////////////////////////////////////////////////////////////////////////
//詳細画面 固定配置用コントロール制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
//メイン処理からの各画面派生呼び出し(PageLoad)
function DetailFramePageLoaded() {
    ContentSolidScroll.Initialize();
    ContentSolidScroll.ResizeDivArea();
}
//メイン処理からの各画面派生呼び出し(Resize)
function DetailFramePageResized() {
    ContentSolidScroll.ResizeDivArea();
}

ContentSolidScroll = {
    //定数
    DIV_DETAIL_AREA: "divMainListArea"
    ,
    PARENT_DIV_W_MARGIN: 6
    ,
    INNER_DIV_W_MARGIN: 2
    ,
    INNER_DIV_H_MARGIN: 3
    ,
    GRID_MAIN_VIEW: "divMainViewScroll"
    ,
    GRID_LT: "divLTScroll"
    ,
    GRID_RT: "divRTScroll"
    ,
    GRID_LB: "divLBScroll"
    ,
    GRID_RB: "divRBScroll"
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
        var divParentArea = $("#" + this.DIV_BODY);
        var divMainArea = $("#" + this.DIV_MAIN_AREA);
        var divSubArea = $("#" + this.DIV_SUB_AREA);
        //グリッド用DIVサイズ変更
        var gridLT = $("#" + ContentSolidScroll.GRID_LT);
        var gridRT = $("#" + ContentSolidScroll.GRID_RT);
        var gridLB = $("#" + ContentSolidScroll.GRID_LB);
        var gridRB = $("#" + ContentSolidScroll.GRID_RB);

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
                divMain_Height = containtsBottomArea.position().top - divMain_Top - 20;
                gridLB.height(divMain_Height);
                gridRB.height(divMain_Height);
            } catch (e) {
            }
        }

    }
    ,
    Initialize: function () {
        $("#" + ContentSolidScroll.GRID_RB).scroll(function () {
            var gridRT = $("#" + ContentSolidScroll.GRID_RT);
            var gridLB = $("#" + ContentSolidScroll.GRID_LB);
            var gridRB = $("#" + ContentSolidScroll.GRID_RB);
            if (gridRB.scrollTop() != gridLB.scrollTop()) {
                gridLB.scrollTop(gridRB.scrollTop());
            }
            if (gridRB.scrollLeft() != gridRT.scrollLeft()) {
                gridRT.scrollLeft(gridRB.scrollLeft());
            }
        });
        $("#" + ContentSolidScroll.GRID_LB).scroll(function () {
            var gridLB = $("#" + ContentSolidScroll.GRID_LB);
            var gridRB = $("#" + ContentSolidScroll.GRID_RB);
            if (gridLB.scrollTop() != gridRB.scrollTop()) {
                gridRB.scrollTop(gridLB.scrollTop());
            }
        });
        $("#" + ContentSolidScroll.GRID_LB).on(Common.GetWheelEvent(), function (e) {
            var gridLB = $("#" + ContentSolidScroll.GRID_LB);
            gridLB.scrollTop(gridLB.scrollTop() - Common.GetWheelDelta(e));
        });
        $("#" + ContentSolidScroll.GRID_RB).on(Common.GetWheelEvent(), function (e) {
            var gridRB = $("#" + ContentSolidScroll.GRID_RB);
            gridRB.scrollTop(gridRB.scrollTop() - Common.GetWheelDelta(e));
        });
    }
}