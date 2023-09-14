///////////////////////////////////////////////////////////////////////////////////////////////
//詳細画面 光軸検査(Optaxis)用コントロール制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
/* 光軸検査用制御 */
//メイン処理からの各画面派生呼び出し(PageLoad)
function DetailFramePageLoaded() {
    var viewer = new ScrollViewer("#" + Optaxis.DIV_VIEW_BOX);
    Optaxis.AfterImageLoad();
    Optaxis.Initialize();
}
//メイン処理からの各画面派生呼び出し(Resize)
function DetailFramePageResized() {
    Optaxis.ResizeDivArea();
}
Optaxis = {
    //定数
    DIV_BODY: "divDetailBodyScroll"
    ,
    DIV_LIST_AREA: "divDetailListArea"
    ,
    DIV_VIEW_AREA: "divDetailViewArea"
    ,
    DIV_VIEW_BOX: "divDetailViewBox"
    ,
    DIV_MAIN_AREA: "divMainListArea"
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
    CSS_LIST_ITEM_DIV: "div-list-view-item"
    ,
    CSS_SELECTED_DIV: "div-item-selected"
    ,
    ATTR_ORIGINAL_SRC: "data-original"
    ,
    DIV_MAIN_AREA: "divMainListArea"
    ,
    GRID_MAIN_LT: "divLTMainScroll"
    ,
    GRID_MAIN_RT: "divRTMainScroll"
    ,
    GRID_MAIN_LB: "divLBMainScroll"
    ,
    GRID_MAIN_RB: "divRBMainScroll"
    ,
    AfterImageLoad: function () {
        var parentDiv = $("#" + this.DIV_BODY);
        var images = $(parentDiv).find("[" + this.ATTR_ORIGINAL_SRC + "]");
        var loop = 0;
        for (loop = 0; loop < images.length; loop++) {
            var imgTag = $(images[loop]);
            var originalSrc = imgTag.attr(this.ATTR_ORIGINAL_SRC);
            imgTag.attr("src", originalSrc);
        }
    }
    ,
    ResizeDivArea: function () {

        //詳細表示エリアDIVの位置を取得
        var containtsBottom = 0;
        var containtsWidth = 0;
        var divMainArea = $("#" + this.DIV_MAIN_AREA);
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
        var divViewArea = $("#" + this.DIV_VIEW_AREA);
        var divDetailBoxArea = $("#" + this.DIV_VIEW_BOX);
        var divMainArea = $("#" + this.DIV_MAIN_AREA);

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
                divViewArea.width(divWSize);
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
                divViewArea.height(divHSize);
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
        var gridLTMain = $("#" + this.GRID_MAIN_LT);
        var gridRTMain = $("#" + this.GRID_MAIN_RT);
        var gridLBMain = $("#" + this.GRID_MAIN_LB);
        var gridRBMain = $("#" + this.GRID_MAIN_RB);

        if (0 < gridLTMain.length) {

            //グリッドのDIVを幅変更する
            if (null != gridLTMain.position()) {
                divR_Left = gridLTMain.position().left + gridLTMain.width();
            }

            try {
                tblHeaders_Width = windowWidth - this.PARENT_DIV_W_MARGIN;
                divR_Width = tblHeaders_Width - divR_Left + Optaxis.INNER_DIV_MARGIN_3;
                gridRTMain.width(divR_Width);
                gridRBMain.width(divR_Width);
            } catch (e) {
            }

            try {
                //                divMain_Height = containtsBottom - divMain_Top - Optaxis.INNER_DIV_MARGIN_12;
                divMain_Height = divParentArea.position().top - gridRTMain.position().top - 30;
                //                gridLBMain.height(divMain_Height);
                //                gridRBMain.height(divMain_Height);
            } catch (e) {
            }
        }
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
        if (0 < selectRow[0].id.indexOf("lstMainListRB")) {
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
                tmp = tmp.replace("lstMainListRB", "lstMainListLB");
                var obj = $("#" + tmp);
                $(obj).addClass(ControlCommon.SELECT_ROW);
            } else {
                //連動させる側が右
                var tmp = $(selectRow)[0].id;
                tmp = tmp.replace("lstMainListLB", "lstMainListRB");
                var obj = $("#" + tmp);
                $(obj).addClass(ControlCommon.SELECT_ROW);
            }

            if (null != selectBtnId && undefined != selectBtnId && "" != selectBtnId) {
                __doPostBack(selectBtnId, '');
            }
        }
    }
    ,
    //メインエリア画像変更
    ChangeMainAreaImage: function (ctrlId, url) {

        var mainArea = $("#" + ctrlId);
        if (mainArea.attr("src") == url) {
            return;
        }

        $("#" + ctrlId).attr("src", url);
        $("#" + Optaxis.DIV_VIEW_BOX).scrollTop(0);
        $("#" + Optaxis.DIV_VIEW_BOX).scrollLeft(0);

        //選択行変更
        var ele = $(window.event.srcElement);

        var listItems = $("#" + Optaxis.DIV_LIST_AREA).find("." + Optaxis.CSS_LIST_ITEM_DIV);
        var selectRow = ele.closest("." + Optaxis.CSS_LIST_ITEM_DIV);
        if (0 < selectRow.length) {
            $(listItems).removeClass(Optaxis.CSS_SELECTED_DIV);
            $(selectRow).addClass(Optaxis.CSS_SELECTED_DIV);
        }
    }
    ,
    //選択行連動
    Initialize: function () {

        //メインタブ
        $("#" + Optaxis.GRID_MAIN_RB).scroll(function () {
            var gridRTMain = $("#" + Optaxis.GRID_MAIN_RT);
            var gridLBMain = $("#" + Optaxis.GRID_MAIN_LB);
            var gridRBMain = $("#" + Optaxis.GRID_MAIN_RB);
            if (gridRBMain.scrollTop() != gridLBMain.scrollTop()) {
                gridLBMain.scrollTop(gridRBMain.scrollTop());
            }
            if (gridRBMain.scrollLeft() != gridRTMain.scrollLeft()) {
                gridRTMain.scrollLeft(gridRBMain.scrollLeft());
            }
        });
        $("#" + Optaxis.GRID_MAIN_LB).scroll(function () {
            var gridLBMain = $("#" + Optaxis.GRID_MAIN_LB);
            var gridRBMain = $("#" + Optaxis.GRID_MAIN_RB);
            if (gridLBMain.scrollTop() != gridRBMain.scrollTop()) {
                gridRBMain.scrollTop(gridLBMain.scrollTop());
            }
        });
        $("#" + Optaxis.GRID_MAIN_LB).on(Common.GetWheelEvent(), function (e) {
            var gridLBMain = $("#" + Optaxis.GRID_MAIN_LB);
            gridLBMain.scrollTop(gridLBMain.scrollTop() - Common.GetWheelDelta(e));
        });
        $("#" + Optaxis.GRID_MAIN_RB).on(Common.GetWheelEvent(), function (e) {
            var gridRBMain = $("#" + Optaxis.GRID_MAIN_RB);
            gridRBMain.scrollTop(gridRBMain.scrollTop() - Common.GetWheelDelta(e));
        });
    }
}

