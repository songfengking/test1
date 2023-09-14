///////////////////////////////////////////////////////////////////////////////////////////////
//クライアント用コントロール制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
ControlCommon = {
    //定数
    HDN_FOCUS_ID: "hdnFocus"
    ,
    GRID_ROW: "grid-row"
    ,
    GRID_VIEW: "grid-layout"
    ,
    SELECT_ROW: "ui-state-highlight"
    ,
    LIST_VIEW_ROW: "listview-row"
    ,
    LIST_VIEW_SELECT_CTRL: "btnSelectSubmit"
    ,
    METHOD_TYPE_WINDOW: 1
    ,
    QUERY_STR_TOKEN: "?Token="
    ,
    QUERY_STR_INDEX: "&Index="
    ,
    QUERY_STR_GROUP: "&GroupCd="
    ,
    QUERY_STR_CLASS: "&ClassCd="
    //,
    //QUERY_STR_COOP: "?Coop="
    ,
    QUERY_STR_MODEL_CD: "&ModelCd="
    ,
    QUERY_STR_COUNTRY_CD: "&CountryCd="
    ,
    QUERY_STR_SERIAL: "&Serial="
    ,
    ATTR_GROUP_CD: "group_cd"
    ,
    ATTR_GRID_INDEX: "grid_index"
    ,
    ATTR_DATA_INDEX: "data_index"
    ,
    GRID_VIEW2: "grid-layout2"
    ,
    //グリッドビュー行選択
    SelectGridRow: function (ele,index) {

        var gridview = $(ele).closest("." + ControlCommon.GRID_VIEW);
        var gridrows = $(gridview).find("." + ControlCommon.GRID_ROW);
        var selectRow = $(ele);

        if (0 < selectRow.length) {
            $(gridrows).removeClass(ControlCommon.SELECT_ROW);
            $(selectRow).addClass(ControlCommon.SELECT_ROW);
        }
    }
    ,
    //グリッドビュー行ダブルクリック
    DoubleClickGridRow: function (index, methodType, param1, param2) {
        if (this.METHOD_TYPE_WINDOW == methodType) {
            this.WindowOpen(index, param1, param2);
        }
    },
    //グリッドビュー行ダブルクリック
    DoubleClickGridRowWithCaller: function (index, methodType, param1, param2, param3) {
        if (this.METHOD_TYPE_WINDOW == methodType) {
            this.WindowOpenWithCaller(index, param1, param2, param3);
        }
    },
    //リストビュー行選択
    SelectListViewRow: function (ele,index, selectBtnId) {

        var gridview = $(ele).closest("." + ControlCommon.GRID_VIEW);
        var preSelect = $(gridview).find("." + ControlCommon.SELECT_ROW);
        var gridrows = $(gridview).find("." + ControlCommon.LIST_VIEW_ROW);

        var selectRow = $(ele);

        if (0 < preSelect.length
            && 0 < selectRow.length
            && preSelect[0] == selectRow[0]) {
            return;
        }

        if (0 < selectRow.length) {
            $(gridrows).removeClass(ControlCommon.SELECT_ROW);
            $(selectRow).addClass(ControlCommon.SELECT_ROW);
            
            if (null != selectBtnId && undefined != selectBtnId && "" != selectBtnId) {
                __doPostBack(selectBtnId, '');
            }
        }
    }
    ,
    //グリッドビュー行選択
    SelectGridRowSynchro: function (ele, index) {

        var selectRow = $(ele);
        var attrGrpCd = selectRow.attr(this.ATTR_GROUP_CD);
        if (false == Common.IsBlank(attrGrpCd)) {
            var gridIndex = selectRow.attr(this.ATTR_GRID_INDEX);

            var selectRows = $("[" + this.ATTR_GROUP_CD + "='" + attrGrpCd + "']" + "[" + this.ATTR_GRID_INDEX + "='" + gridIndex + "']");

            for (var idx = 0; idx < selectRows.length; idx++) {
                var gridview = $(selectRows[idx]).closest("." + ControlCommon.GRID_VIEW2);
                var gridrows = $(gridview).find("." + ControlCommon.GRID_ROW);
                $(gridrows).removeClass(ControlCommon.SELECT_ROW);
            }
            $(selectRows).addClass(ControlCommon.SELECT_ROW);
        } else {
            var gridview = $(ele).closest("." + ControlCommon.GRID_VIEW2);
            var gridrows = $(gridview).find("." + ControlCommon.GRID_ROW);
            $(gridrows).removeClass(ControlCommon.SELECT_ROW);
            $(selectRow).addClass(ControlCommon.SELECT_ROW);
        }
    }
    ,
    //ウィンドウオープン(セッションインデックス指定渡し)
    WindowOpen: function (index, url, token) {
        var unixTs = new Date().getTime();
        var subWindow = window.open(url + this.QUERY_STR_TOKEN + token + this.QUERY_STR_INDEX + index + "&uts=" + unixTs, '_blank');
    }
    ,
    //ウィンドウオープン(セッションインデックス、呼び出し元ページ指定渡し)
    WindowOpenWithCaller: function (index, url, token, callerPageId) {
        var unixTs = new Date().getTime();
        var subWindow = window.open(url + this.QUERY_STR_TOKEN + token + this.QUERY_STR_INDEX + index + "&uts=" + unixTs + "&CallerPageID=" + callerPageId, '_blank');
    }
    ,
    //ウィンドウオープン(型式、国、機番 指定)
    WindowOpenChangeDetail: function (url, token, modelCd, countryCd, serial) {
        var unixTs = new Date().getTime();
        var subWindow = window.open(url + this.QUERY_STR_TOKEN + token + this.QUERY_STR_MODEL_CD + modelCd + this.QUERY_STR_COUNTRY_CD + countryCd + this.QUERY_STR_SERIAL + serial + "&uts=" + unixTs, '_blank');
        return false;
    }
    ,
    //ウィンドウクローズ前処理
    BeforeWindowClose: function () {

        //終了ローディング
        SubmitControl.SetLoadingType(SubmitControl.TYPE_EXIT);

        var browserType = Common.GetBrowserType();
        //IE系(EDGEは都度確認要)
        browserType = Common.BROWSER_IE8;
        if (Common.BROWSER_IE8 == browserType
            || Common.BROWSER_IE9 == browserType
            || Common.BROWSER_IE_EDGE == browserType) {
            //サーバからセッション削除後終了
            return true;
        //IE以外(終了確認メッセージとかはブラウザ依存)
        } else if (Common.BROWSER_CHROME == browserType
            || Common.BROWSER_NO_SUPPORT == browserType) {
            //即、ウィンドウクローズ セッションはタイムアウトから破棄
            this.WindowClose();
            return false;
        }
    }
    ,
    //ウィンドウクローズ
    WindowClose: function () {

        var browserType = Common.GetBrowserType();
        //IE系(EDGEは都度確認要)
        browserType = Common.BROWSER_IE8;
        if (Common.BROWSER_IE8 == browserType
            || Common.BROWSER_IE9 == browserType
            || Common.BROWSER_IE_EDGE == browserType) {
            window.open('about:blank', '_self').close();
        //IE以外(終了確認メッセージとかはブラウザ依存)
        } else if (Common.BROWSER_CHROME == browserType
            || Common.BROWSER_NO_SUPPORT == browserType) {
            window.opener = window;
            window.close();
        }
    }
    ,
    //初期フォーカスセット
    SetInitializeFocus: function () {
        var hdnFocus = $("#" + this.HDN_FOCUS_ID);
        if (0 < hdnFocus.length) {
            var focusCtrlId = hdnFocus.val();
            if ("" != focusCtrlId) {
                ControlCommon.SetFocus(focusCtrlId);
            }
        }
    }
    ,
    //最終フォーカス保持
    KeepFocus: function () {
        var hdnFocus = $("#" + this.HDN_FOCUS_ID);
        if (0 < hdnFocus.length) {
            var autoFocusElementId = document.activeElement.id;
            if (null != autoFocusElementId && undefined != autoFocusElementId && "" != autoFocusElementId) {
                hdnFocus.val(autoFocusElementId);
            }            
        }
    }
    ,
    //フォーカスセット
    SetFocus: function( ctrlID ) {
        try {
            setTimeout(function () {
                //フォーカス設定
                $('#' + ctrlID).focus();
                if (document.activeElement.id != ctrlID) {
                    $('#' + ctrlID).focus().focus();
                }
            }, 0);
        } catch (ex) {
            //alert(ex.message);
        }
    }
}