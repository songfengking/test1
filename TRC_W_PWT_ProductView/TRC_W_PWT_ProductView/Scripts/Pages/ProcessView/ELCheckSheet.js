///////////////////////////////////////////////////////////////////////////////////////////////
//詳細画面 電子チェックシート(ELCheckSheet)用コントロール制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
/* 電子チェックシートイメージ画像用制御 */
//メイン処理からの各画面派生呼び出し(PageLoad)
function DetailFramePageLoaded() {
    var viewer = new ScrollViewer("#" + ELCheckSheetDef.DIV_VIEW_BOX);
    ELCheckSheetDef.AfterImageLoad();
    ELCheckSheetDef.Initialize();
}
//メイン処理からの各画面派生呼び出し(Resize)
function DetailFramePageResized() {
    ELCheckSheetDef.ResizeDivArea();
}
ELCheckSheetDef = {
    //定数
    DIV_BODY: "tabResult"
    ,
    DIV_LIST_AREA: "divNGListArea"
    ,
    DIV_VIEW_AREA: "divNGViewArea"
    ,
    DIV_VIEW_BOX: "divNGViewBox"
    ,
    DIV_CHK_LIST_AREA: "divChkListArea"
    ,
    DIV_CHK_VIEW_AREA: "divChkViewArea"
    ,
    DIV_CHK_VIEW_BOX: "divChkViewBox"
    ,
    DIV_MAIN_AREA: "divMainListArea"
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
    GRID_LT: "divLTScroll"
    ,
    GRID_RT: "divRTScroll"
    ,
    GRID_LB: "divLBScroll"
    ,
    GRID_RB: "divRBScroll"
    ,
    GRID_NG_LT: "divNGLTScroll"
    ,
    GRID_NG_RT: "divNGRTScroll"
    ,
    GRID_NG_LB: "divNGLBScroll"
    ,
    GRID_NG_RB: "divNGRBScroll"
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

        var divChkListArea = $("#" + this.DIV_CHK_LIST_AREA);
        var divChkViewArea = $("#" + this.DIV_CHK_VIEW_AREA);
        var divChkDetailBoxArea = $("#" + this.DIV_CHK_VIEW_BOX);

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
                divHSize = containtsBottom - divParentArea.position().top - this.PARENT_DIV_H_MARGIN;
                divListArea.height(divHSize);
                divViewArea.height(divHSize);
                divDetailBoxArea.height(divHSize);

                divChkListArea.height(divHSize);
                divChkViewArea.height(divHSize);
                divChkDetailBoxArea.height(divHSize);

            } catch (e) {
            }

            //各タブの高さ設定
            divTop = divParentArea.position().top;
            divHSize = containtsBottom - divTop - this.INNER_DIV_MARGIN_30 - this.INNER_DIV_MARGIN_3;
            var listItems = $("#" + this.DIV_BODY).find("." + this.SELECTED_TAB_DISP);
            $(listItems).height(divHSize);

        }

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

        //DIVサイズ変更
        ///検査情報タブ
        var gridLT = $("#" + this.GRID_LT);
        var gridRT = $("#" + this.GRID_RT);
        var gridLB = $("#" + this.GRID_LB);
        var gridRB = $("#" + this.GRID_RB);
        ///NGタブ
        var gridNGLT = $("#" + this.GRID_NG_LT);
        var gridNGRT = $("#" + this.GRID_NG_RT);
        var gridNGLB = $("#" + this.GRID_NG_LB);
        var gridNGRB = $("#" + this.GRID_NG_RB);


        ///メインタブ
        var gridLTMain = $("#" + this.GRID_MAIN_LT);
        var gridRTMain = $("#" + this.GRID_MAIN_RT);
        var gridLBMain = $("#" + this.GRID_MAIN_LB);
        var gridRBMain = $("#" + this.GRID_MAIN_RB);

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
                divR_Width = tblHeaders_Width - divR_Left + ELCheckSheetDef.INNER_DIV_MARGIN_3;
                gridRTMain.width(divR_Width);
                gridRBMain.width(divR_Width);
            } catch (e) {
            }

            //グリッドのDIVを高さ変更する
            if (null != gridLB.position()) {
                divMain_Top = gridLBMain.position().top;
            }
            try {
//                divMain_Height = containtsBottom - divMain_Top - ELCheckSheetDef.INNER_DIV_MARGIN_12;
                divMain_Height = divParentArea.position().top - gridRTMain.position().top - 30;
//                gridLBMain.height(divMain_Height);
//                gridRBMain.height(divMain_Height);
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
        
        if (0 < gridLT.length) {

            //グリッドのDIVを幅変更する
            if (null != gridLT.position()) {
                divR_Left = gridLT.position().left + gridLT.width();
            }

            try {
                tblHeaders_Width = windowWidth - this.PARENT_DIV_W_MARGIN;
                divR_Width = tblHeaders_Width - divR_Left + ELCheckSheetDef.INNER_DIV_MARGIN_3;
                gridRT.width(divR_Width);
                gridRB.width(divR_Width);
            } catch (e) {
            }

            //グリッドのDIVを高さ変更する
            if (null != gridLB.position()) {
                divMain_Top = gridLB.position().top;
            }
            try {
                divMain_Height = containtsBottom - divMain_Top - ELCheckSheetDef.INNER_DIV_MARGIN_12;
                gridLB.height(divMain_Height);
                gridRB.height(divMain_Height);
            } catch (e) {
            }
        }
        
        //DIVサイズ変更
        ///NGタブ
        var gridNGLT = $("#" + this.GRID_NG_LT);
        var gridNGRT = $("#" + this.GRID_NG_RT);
        var gridNGLB = $("#" + this.GRID_NG_LB);
        var gridNGRB = $("#" + this.GRID_NG_RB);

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
            if (null != gridNGLT.position()) {
                divR_Left = gridNGLT.position().left + gridNGLT.width();
            }

            try {
                tblHeaders_Width = windowWidth - this.PARENT_DIV_W_MARGIN;
                divR_Width = tblHeaders_Width - divR_Left + ELCheckSheetDef.INNER_DIV_MARGIN_3;
                gridNGRT.width(divR_Width);
                gridNGRB.width(divR_Width);
            } catch (e) {
            }

            //グリッドのDIVを高さ変更する
            if (null != gridNGLB.position()) {
                divMain_Top = gridNGLB.position().top;
            }
            try {
                divMain_Height = containtsBottom - divMain_Top - ELCheckSheetDef.INNER_DIV_MARGIN_12;
                gridNGLB.height(divMain_Height);
                gridNGRB.height(divMain_Height);
            } catch (e) {
            }
        }


    }
    ,
    //メインエリア画像変更(不具合画像)
    ChangeMainAreaImage: function (ctrlId, url) {

        var mainArea = $("#" + ctrlId);
        if (mainArea.attr("src") == url) {
            return;
        }

        $("#" + ctrlId).attr("src", url);
        $("#" + this.DIV_VIEW_BOX).scrollTop(0);
        $("#" + this.DIV_VIEW_BOX).scrollLeft(0);

        //選択行変更
        var ele = $(window.event.srcElement);

        var listItems = $("#" + this.DIV_LIST_AREA).find("." + this.CSS_LIST_ITEM_DIV);
        var selectRow = ele.closest("." + this.CSS_LIST_ITEM_DIV);
        if (0 < selectRow.length) {
            $(listItems).removeClass(this.CSS_SELECTED_DIV);
            $(selectRow).addClass(this.CSS_SELECTED_DIV);
        }
    }
    ,
    //メインエリア画像変更(検査画像)
    ChangeMainAreaImage2: function (ctrlId, url) {

        var mainArea = $("#" + ctrlId);
        if (mainArea.attr("src") == url) {
            return;
        }

        $("#" + ctrlId).attr("src", url);
        $("#" + this.DIV_CHK_VIEW_BOX).scrollTop(0);
        $("#" + this.DIV_CHK_VIEW_BOX).scrollLeft(0);

        //選択行変更
        var ele = $(window.event.srcElement);

        var listItems = $("#" + this.DIV_CHK_LIST_AREA).find("." + this.CSS_LIST_ITEM_DIV);
        var selectRow = ele.closest("." + this.CSS_LIST_ITEM_DIV);
        if (0 < selectRow.length) {
            $(listItems).removeClass(this.CSS_SELECTED_DIV);
            $(selectRow).addClass(this.CSS_SELECTED_DIV);
        }
    }
    ,
    //タブコントロール
    ChangeTab: function (tabName) {

        var tmp;    //制御用tmp変数

        //タブの表示制御
        var listItems = $("#" + this.DIV_BODY).find("." + this.SELECTED_TAB_DISP);
        $(listItems).addClass(this.CSS_UNSELECTED_DISP);

        //タブの色制御
        var lsTabColor = $("." + this.SELECTED_TAB_COLOR);
        $(lsTabColor).addClass(this.CSS_UNSELECTED_TAB);


        // 指定タブのみ表示と色を変更
        if (tabName) {
            tmp = $("#" + tabName);
            tmp.removeClass(this.CSS_UNSELECTED_DISP);

            tmp = $("." + tabName);
            tmp.removeClass(this.CSS_UNSELECTED_TAB);
            tmp.addClass(this.CSS_SELECTED_TAB);

        }

        //画面DIV制御
        ELCheckSheetDef.ResizeDivArea();
    }
    ,
    //選択行連動
    Initialize: function () {

        //メインタブ
        $("#" + ELCheckSheetDef.GRID_MAIN_RB).scroll(function () {
            var gridRTMain = $("#" + ELCheckSheetDef.GRID_MAIN_RT);
            var gridLBMain = $("#" + ELCheckSheetDef.GRID_MAIN_LB);
            var gridRBMain = $("#" + ELCheckSheetDef.GRID_MAIN_RB);
            if (gridRBMain.scrollTop() != gridLBMain.scrollTop()) {
                gridLBMain.scrollTop(gridRBMain.scrollTop());
            }
            if (gridRBMain.scrollLeft() != gridRTMain.scrollLeft()) {
                gridRTMain.scrollLeft(gridRBMain.scrollLeft());
            }
        });
        $("#" + ELCheckSheetDef.GRID_MAIN_LB).scroll(function () {
            var gridLBMain = $("#" + ELCheckSheetDef.GRID_MAIN_LB);
            var gridRBMain = $("#" + ELCheckSheetDef.GRID_MAIN_RB);
            if (gridLBMain.scrollTop() != gridRBMain.scrollTop()) {
                gridRBMain.scrollTop(gridLBMain.scrollTop());
            }
        });
        $("#" + ELCheckSheetDef.GRID_MAIN_LB).on(Common.GetWheelEvent(), function (e) {
            var gridLBMain = $("#" + ELCheckSheetDef.GRID_MAIN_LB);
            gridLBMain.scrollTop(gridLBMain.scrollTop() - Common.GetWheelDelta(e));
        });
        $("#" + ELCheckSheetDef.GRID_MAIN_RB).on(Common.GetWheelEvent(), function (e) {
            var gridRBMain = $("#" + ELCheckSheetDef.GRID_MAIN_RB);
            gridRBMain.scrollTop(gridRBMain.scrollTop() - Common.GetWheelDelta(e));
        });


        //検査タブ
        $("#" + ELCheckSheetDef.GRID_RB).scroll(function () {
            var gridRT = $("#" + ELCheckSheetDef.GRID_RT);
            var gridLB = $("#" + ELCheckSheetDef.GRID_LB);
            var gridRB = $("#" + ELCheckSheetDef.GRID_RB);
            if (gridRB.scrollTop() != gridLB.scrollTop()) {
                gridLB.scrollTop(gridRB.scrollTop());
            }
            if (gridRB.scrollLeft() != gridRT.scrollLeft()) {
                gridRT.scrollLeft(gridRB.scrollLeft());
            }
        });
        $("#" + ELCheckSheetDef.GRID_LB).scroll(function () {
            var gridLB = $("#" + ELCheckSheetDef.GRID_LB);
            var gridRB = $("#" + ELCheckSheetDef.GRID_RB);
            if (gridLB.scrollTop() != gridRB.scrollTop()) {
                gridRB.scrollTop(gridLB.scrollTop());
            }
        });
        $("#" + ELCheckSheetDef.GRID_LB).on(Common.GetWheelEvent(), function (e) {
            var gridLB = $("#" + ELCheckSheetDef.GRID_LB);
            gridLB.scrollTop(gridLB.scrollTop() - Common.GetWheelDelta(e));
        });
        $("#" + ELCheckSheetDef.GRID_RB).on(Common.GetWheelEvent(), function (e) {
            var gridRB = $("#" + ELCheckSheetDef.GRID_RB);
            gridRB.scrollTop(gridRB.scrollTop() - Common.GetWheelDelta(e));
        });

        //NGタブ
        //検査タブ
        $("#" + ELCheckSheetDef.GRID_NG_RB).scroll(function () {
            var gridNGRT = $("#" + ELCheckSheetDef.GRID_NG_RT);
            var gridNGLB = $("#" + ELCheckSheetDef.GRID_NG_LB);
            var gridNGRB = $("#" + ELCheckSheetDef.GRID_NG_RB);
            if (gridNGRB.scrollTop() != gridNGLB.scrollTop()) {
                gridNGLB.scrollTop(gridNGRB.scrollTop());
            }
            if (gridNGRB.scrollLeft() != gridNGRT.scrollLeft()) {
                gridNGRT.scrollLeft(gridNGRB.scrollLeft());
            }
        });
        $("#" + ELCheckSheetDef.GRID_NG_LB).scroll(function () {
            var gridNGLB = $("#" + ELCheckSheetDef.GRID_NG_LB);
            var gridNGRB = $("#" + ELCheckSheetDef.GRID_NG_RB);
            if (gridNGLB.scrollTop() != gridNGRB.scrollTop()) {
                gridNGRB.scrollTop(gridNGLB.scrollTop());
            }
        });
        $("#" + ELCheckSheetDef.GRID_NG_LB).on(Common.GetWheelEvent(), function (e) {
            var gridNGLB = $("#" + ELCheckSheetDef.GRID_NG_LB);
            gridNGLB.scrollTop(gridNGLB.scrollTop() - Common.GetWheelDelta(e));
        });
        $("#" + ELCheckSheetDef.GRID_RB).on(Common.GetWheelEvent(), function (e) {
            var gridRB = $("#" + ELCheckSheetDef.GRID_RB);
            gridRB.scrollTop(gridRB.scrollTop() - Common.GetWheelDelta(e));
        });

    }
    ,
    //リストビュー行選択(検査情報)
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
        if (0 < selectRow[0].id.indexOf("lstChkInfo2")) {
            blLR = true;
        }

        if (0 < selectRow.length) {
            //選択行を全てクリア
            var listItems = $("#tabChkInfo").find(".listview-row");
            $(listItems).removeClass(ControlCommon.SELECT_ROW);
            $(selectRow).addClass(ControlCommon.SELECT_ROW);

            if (true == blLR) {
                //連動させる側が左
                var tmp = $(selectRow)[0].id;
                tmp = tmp.replace("lstChkInfo2", "lstChkInfo");
                var obj = $("#" + tmp);
                $(obj).addClass(ControlCommon.SELECT_ROW);
            } else {
                //連動させる側が右
                var tmp = $(selectRow)[0].id;
                tmp = tmp.replace("lstChkInfo", "lstChkInfo2");
                var obj = $("#" + tmp);
                $(obj).addClass(ControlCommon.SELECT_ROW);
            }
        }
    }
    ,
    //リストビュー行選択(NGリスト)
    SelectNGListViewRow: function (ele, index, selectBtnId) {

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
        if (0 < selectRow[0].id.indexOf("lstNGRB")) {
            blLR = true;
        }

        if (0 < selectRow.length) {
            //選択行を全てクリア
            var listItems = $("#tabNGList").find(".listview-row");
            $(listItems).removeClass(ControlCommon.SELECT_ROW);
            $(selectRow).addClass(ControlCommon.SELECT_ROW);

            if (true == blLR) {
                //連動させる側が左
                var tmp = $(selectRow)[0].id;
                tmp = tmp.replace("lstNGRB", "lstNG");
                var obj = $("#" + tmp);
                $(obj).addClass(ControlCommon.SELECT_ROW);
            } else {
                //連動させる側が右
                var tmp = $(selectRow)[0].id;
                tmp = tmp.replace("lstNG", "lstNGRB");
                var obj = $("#" + tmp);
                $(obj).addClass(ControlCommon.SELECT_ROW);
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
}

