///////////////////////////////////////////////////////////////////////////////////////////////
//詳細画面 AI画像解析(AiImageDef)用コントロール制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
/* 電子チェックシートイメージ画像用制御 */
//メイン処理からの各画面派生呼び出し(PageLoad)
function DetailFramePageLoaded() {
    var viewer = new ScrollViewer("#" + AiImageDef.DIV_VIEW_BOX);
    AiImageDef.AfterImageLoad();
}
//メイン処理からの各画面派生呼び出し(Resize)
function DetailFramePageResized() {
    AiImageDef.ResizeDivArea();
}
AiImageDef = {
    //定数
    DIV_BODY: "tabResult"
    ,
    DIV_BODY_RESULT: "divDetailBodyScroll"
    ,
    DIV_LIST_AREA: "divAnlListArea"
    ,
    DIV_VIEW_AREA: "divAnlViewArea"
    ,
    DIV_VIEW_BOX: "divAnlViewBox"
    ,
    DIV_MAIN_AREA: "divMainListArea"
    ,
    UL_STATION_TABS: "stationTabs"
    ,
    PARENT_DIV_W_MARGIN: 20
    ,
    PARENT_DIV_H_MARGIN: 34
    ,
    INNER_DIV_MARGIN_3: 3
    ,
    INNER_DIV_MARGIN_12: 12
    ,
    CSS_LIST_ITEM_DIV: "div-list-view-item"
    ,
    CSS_SELECTED_DIV: "div-item-selected"
    ,
    ATTR_ORIGINAL_SRC: "data-original"
    ,
    CSS_SELECTED_TAB: "selected-tab-color"
    ,
    CSS_UNSELECTED_TAB: "unselected-tab-color"
    ,
    CSS_UNSELECTED_DISP: "unselected-tab-disp"
    ,
    SELECTED_TAB_DISP: "tabDiv"
    ,
    SELECTED_TAB_COLOR: "tabColor"
    ,
    INNER_DIV_MARGIN_25: 25
    ,
    INNER_DIV_MARGIN_30: 30
    ,
    INNER_DIV_MARGIN_40: 40
    ,
    INNER_DIV_MARGIN_70: 70
    ,
    GRID_MAIN_LT: "divLTMainScroll"
    ,
    GRID_MAIN_RT: "divRTMainScroll"
    ,
    GRID_MAIN_LB: "divLBMainScroll"
    ,
    GRID_MAIN_RB: "divRBMainScroll"
    ,
    CURRENT_TAB_ID: ""
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
    ///画面リサイズ
    ResizeDivArea: function () {

        //詳細表示エリアDIVの位置を取得
        var containtsBottom = 0;
        var containtsWidth = 0;
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
        var divListArea = $("#" + this.DIV_LIST_AREA + this.CURRENT_TAB_ID);
        var divViewArea = $("#" + this.DIV_VIEW_AREA + this.CURRENT_TAB_ID);
        var divDetailBoxArea = $("#" + this.DIV_VIEW_BOX + this.CURRENT_TAB_ID);
        var divBodyResult = $("#" + this.DIV_BODY_RESULT + this.CURRENT_TAB_ID);
        var ulStationTabs = $("#" + this.UL_STATION_TABS);

        //上位DIVを高さ変更
        if (0 < divParentArea.length) {
            //親DIVを幅変更する
            try {
                divWSize = containtsWidth - this.PARENT_DIV_W_MARGIN;
                divParentArea.width(divWSize);
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
            //リスト側、詳細側DIVを高さ変更する
            if (null != divListArea.position()) {
                divTop = divListArea.position().top;
            }
            try {
                divHSize = containtsBottom - divParentArea.position().top - ulStationTabs.height() - divBodyResult.height() - this.INNER_DIV_MARGIN_70;
                divListArea.height(divHSize);
                divViewArea.height(divHSize);
                divDetailBoxArea.height(divHSize);
            } catch (e) {
            }

            //各タブの高さ設定
            divTop = divParentArea.position().top;
            divHSize = containtsBottom - divTop - this.INNER_DIV_MARGIN_30 - this.INNER_DIV_MARGIN_3;
            var listItems = $("#" + this.DIV_BODY).find("." + this.SELECTED_TAB_DISP);
            $(listItems).height(divHSize);
        }

        //ウィンドウ幅
        var windowWidth = $(document).innerWidth();

        //メインタブ
        var gridLTMain = $("#" + this.GRID_MAIN_LT + this.CURRENT_TAB_ID);
        var gridRTMain = $("#" + this.GRID_MAIN_RT + this.CURRENT_TAB_ID);
        var gridLBMain = $("#" + this.GRID_MAIN_LB + this.CURRENT_TAB_ID);
        var gridRBMain = $("#" + this.GRID_MAIN_RB + this.CURRENT_TAB_ID);

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

        if (0 < gridLTMain.length) {

            //グリッドのDIVを幅変更する
            if (null != gridLTMain.position()) {
                divR_Left = gridLTMain.position().left + gridLTMain.width();
            }

            try {
                tblHeaders_Width = windowWidth - this.PARENT_DIV_W_MARGIN;
                divR_Width = tblHeaders_Width - divR_Left + AiImageDef.INNER_DIV_MARGIN_3;
                gridRTMain.width(divR_Width);
                gridRBMain.width(divR_Width);
            } catch (e) {
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
        $("#" + this.DIV_VIEW_BOX + this.CURRENT_TAB_ID).scrollTop(0);
        $("#" + this.DIV_VIEW_BOX + this.CURRENT_TAB_ID).scrollLeft(0);

        //選択行変更
        var ele = $(window.event.srcElement);

        var listItems = $("#" + this.DIV_LIST_AREA + this.CURRENT_TAB_ID).find("." + this.CSS_LIST_ITEM_DIV);
        var selectRow = ele.closest("." + this.CSS_LIST_ITEM_DIV);
        if (0 < selectRow.length) {
            $(listItems).removeClass(this.CSS_SELECTED_DIV);
            $(selectRow).addClass(this.CSS_SELECTED_DIV);
        }
    }
    ,
    //タブコントロール
    ChangeTab: function (stationCd) {
        var tmp;    //制御用tmp変数

        //タブの表示制御
        var listItems = $("#" + this.DIV_BODY).find("." + this.SELECTED_TAB_DISP);
        $(listItems).addClass(this.CSS_UNSELECTED_DISP);

        //タブの色制御
        var lsTabColor = $("." + this.SELECTED_TAB_COLOR);
        $(lsTabColor).addClass(this.CSS_UNSELECTED_TAB);

        // 指定タブのみ表示と色を変更
        this.CURRENT_TAB_ID = stationCd;

        tmp = $("#tabStation" + stationCd);
        tmp.removeClass(this.CSS_UNSELECTED_DISP);

        tmp = $(".tabStation" + stationCd);
        tmp.removeClass(this.CSS_UNSELECTED_TAB);
        tmp.addClass(this.CSS_SELECTED_TAB);

        //画面DIV制御
        this.ResizeDivArea();
    }
    ,
    //スクロールバー連動
    InitializeTab: function (stationCd) {
        $("#" + AiImageDef.GRID_MAIN_RB + stationCd).scroll(function () {
            var gridRTMain = $("#" + AiImageDef.GRID_MAIN_RT + stationCd);
            var gridLBMain = $("#" + AiImageDef.GRID_MAIN_LB + stationCd);
            var gridRBMain = $("#" + AiImageDef.GRID_MAIN_RB + stationCd);
            if (gridRBMain.scrollTop() != gridLBMain.scrollTop()) {
                gridLBMain.scrollTop(gridRBMain.scrollTop());
            }
            if (gridRBMain.scrollLeft() != gridRTMain.scrollLeft()) {
                gridRTMain.scrollLeft(gridRBMain.scrollLeft());
            }
        });
        $("#" + AiImageDef.GRID_MAIN_LB + stationCd).scroll(function () {
            var gridLBMain = $("#" + AiImageDef.GRID_MAIN_LB + stationCd);
            var gridRBMain = $("#" + AiImageDef.GRID_MAIN_RB + stationCd);
            if (gridLBMain.scrollTop() != gridRBMain.scrollTop()) {
                gridRBMain.scrollTop(gridLBMain.scrollTop());
            }
        });
        $("#" + AiImageDef.GRID_MAIN_LB + stationCd).on(Common.GetWheelEvent(), function (e) {
            var gridLBMain = $("#" + AiImageDef.GRID_MAIN_LB + stationCd);
            gridLBMain.scrollTop(gridLBMain.scrollTop() - Common.GetWheelDelta(e));
        });
        $("#" + AiImageDef.GRID_MAIN_RB + stationCd).on(Common.GetWheelEvent(), function (e) {
            var gridRBMain = $("#" + AiImageDef.GRID_MAIN_RB + stationCd);
            gridRBMain.scrollTop(gridRBMain.scrollTop() - Common.GetWheelDelta(e));
        });
    }
    ,
    SelectMainViewRow: function (ele, stationCd) {

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
        if (0 < selectRow[0].id.indexOf("lstMainListRB" + stationCd)) {
            blLR = true;
        }

        if (0 < selectRow.length) {
            //選択行を全てクリア
            var listItems = $("#divMainListArea" + stationCd).find(".listview-row");
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
        }
    }
}