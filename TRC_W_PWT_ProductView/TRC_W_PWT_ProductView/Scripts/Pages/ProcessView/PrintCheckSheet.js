///////////////////////////////////////////////////////////////////////////////////////////////
// チェックシート(CheckSheet)印刷用コントロール制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
$(document).on('mousewheel', function (event) {
    // Ctrl + wheel による拡大縮小禁止
    return !event.ctrlKey;
});
//メイン処理からの各画面派生呼び出し(PageLoad)
function MasterMainPageLoaded() {
    PrintCheckSheet.AfterImageLoad();
    PrintCheckSheet.SetCheckBox();
    PrintCheckSheet.DefaultCheckBox();
}
//メイン処理からの各画面派生呼び出し(Resize)
function MasterMainPageResized() {
    PrintCheckSheet.ResizeDivArea();
}
PrintCheckSheet = {
    //定数
    DIV_BODY: "divDetailBodyScroll"
    ,
    DIV_LIST_AREA: "PrintCheckSheet_divListArea"
    ,
    DIV_VIEW_AREA: "divViewArea"
    ,
    DIV_VIEW_BOX: "PrintCheckSheet_divViewBox"
    ,
    DIV_PRINT_AREA: "PrintCheckSheet_divPrintArea"
    ,
    LST_CHECK_SHEET: "lstCheckSheetList"
    ,
    LST_PRINT_LIST: "lstPrintList"
    ,
    BTN_PRINT: "btnPrint"
    ,
    CSS_LIST_ITEM_DIV: "div-list-view-item"
    ,
    CSS_SELECTED_DIV: "div-item-selected"
    ,
    CSS_PRINT_DIV_LIST: "print-div-list"
    ,
    CSS_PRINT_SHOW: "print-show"
    ,
    ATTR_ORIGINAL_SRC: "data-original"
    ,
    PARENT_DIV_W_MARGIN: 8
    ,
    INNER_DIV_H_MARGIN: 3
    ,

    //画像設定
    AfterImageLoad: function () {

        var parentDiv = $("#" + PrintCheckSheet.DIV_BODY);
        var images = $(parentDiv).find("[" + PrintCheckSheet.ATTR_ORIGINAL_SRC + "]");
        var loop = 0;
        for (loop = 0; loop < images.length; loop++) {
            var imgTag = $(images[loop]);
            var originalSrc = imgTag.attr(PrintCheckSheet.ATTR_ORIGINAL_SRC);
            imgTag.attr("src", originalSrc);
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
        $("#" + PrintCheckSheet.DIV_VIEW_BOX).scrollTop(0);
        $("#" + PrintCheckSheet.DIV_VIEW_BOX).scrollLeft(0);

        //選択行変更
        var ele = $(window.event.srcElement);

        var listItems = $("#" + PrintCheckSheet.DIV_LIST_AREA).find("." + PrintCheckSheet.CSS_LIST_ITEM_DIV);
        var selectRow = ele.closest("." + PrintCheckSheet.CSS_LIST_ITEM_DIV);
        if (0 < selectRow.length) {
            $(listItems).removeClass(PrintCheckSheet.CSS_SELECTED_DIV);
            $(selectRow).addClass(PrintCheckSheet.CSS_SELECTED_DIV);
        }
    }
    ,
    //リサイズ
    ResizeDivArea: function () {

        //詳細表示エリアDIVの位置を取得
        var containtsBottom = 0;
        var containtsWidth = 0;
        var containtsArea = $("#" + PrintCheckSheet.DIV_LIST_AREA);
        if (null != containtsArea.position()) {
            containtsBottom = containtsArea.position().top + $(window).height() - PrintCheckSheet.INNER_DIV_H_MARGIN;
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
        var divParentArea = $("#" + PrintCheckSheet.DIV_BODY);
        var divListArea = $("#" + PrintCheckSheet.DIV_LIST_AREA);
        var divViewArea = $("#" + PrintCheckSheet.DIV_VIEW_AREA);
        var divViewBox = $("#" + PrintCheckSheet.DIV_VIEW_BOX);

        //上位DIVを高さ変更
        if (0 < divParentArea.length) {

            //リスト側、詳細側DIVを高さ変更する
            if (null != divListArea.position()) {
                divTop = divListArea.position().top;
            }
            try {
                divHSize = containtsBottom - divTop - PrintCheckSheet.INNER_DIV_H_MARGIN - 45;
                divListArea.height(divHSize);
                divListArea.position().top = 0;

                divHSize = divHSize + 6;
                divViewBox.height(divHSize);
                //                divViewArea.height(divHSize);

            } catch (e) {
            }

            //詳細側DIVを幅変更する
            if (null != divListArea.position()) {
                divLeft = divListArea.width();
            }
            try {
                divWSize = $(window).width() - divLeft - PrintCheckSheet.PARENT_DIV_W_MARGIN;
                divViewBox.width(divWSize);
                //                divViewArea.width(divWSize);
            } catch (e) {
            }
        }
    }
    ,
    //チェックボックス選択
    SetCheckBox: function () {

        $("#btnPrint").prop("disabled", false);

        $('table tr').click(function () {

            var ele = window.event.srcElement;
            //チェックボックス以外は処理しない
            if (ele.type != "checkbox") {
                return;
            }

            var listItems = $("#" + PrintCheckSheet.DIV_LIST_AREA).find(".print-div-list");
            var node = ele.parentNode.parentNode.children[1].children[0];
            var selectRow = $(node).closest(".print-div-list");

            if (ele.checked) {
                //印刷対象外→印刷対象

                var tmp = $(selectRow)[0].id;
                tmp = tmp.replace("lstCheckSheetList", "lstPrintList");
                var obj = $("#" + tmp);
                $(obj).removeClass(PrintCheckSheet.CSS_PRINT_SHOW);
                //等比だと切れることがあるのでやや縮小
                $(obj).width(794 * 0.95);
                $(obj).height(1123 * 0.95);


            } else {
                //印刷対象→印刷対象外
                var tmp = $(selectRow)[0].id;
                tmp = tmp.replace("lstCheckSheetList", "lstPrintList");
                var obj = $("#" + tmp);
                $(obj).addClass(PrintCheckSheet.CSS_PRINT_SHOW);

            }

            //印刷ボタン制御
            if (listItems.length == $("#" + PrintCheckSheet.DIV_PRINT_AREA).find(".print-show").length) {
                $("#" + PrintCheckSheet.BTN_PRINT).prop("disabled", true);
            } else {
                $("#" + PrintCheckSheet.BTN_PRINT).prop("disabled", false);
            }
        });
    }
    ,
    //デフォルトチェックBOX
    DefaultCheckBox: function () {
        var listItems = $("#" + PrintCheckSheet.DIV_LIST_AREA).find(".print-div-list");
        for (var idx = 0; idx < listItems.length; ++idx) {
            var tmp = listItems[idx].id.replace("lstCheckSheetList", "lstPrintList");
            var obj = $("#" + tmp);
            $(obj).removeClass(PrintCheckSheet.CSS_PRINT_SHOW);
            //等比だと切れることがあるのでやや縮小
            $(obj).width(794 * 0.95);
            $(obj).height(1123 * 0.95);
        }
    }
}