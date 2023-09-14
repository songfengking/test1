///////////////////////////////////////////////////////////////////////////////////////////////
//詳細画面 出荷部品(ShipmentParts)用コントロール制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
//メイン処理からの各画面派生呼び出し(PageLoad)
function DetailFramePageLoaded() {
    var viewer = new ScrollViewer("#" + ShipmentParts.DIV_VIEW_BOX);
    ShipmentParts.AfterImageLoad();
}
//メイン処理からの各画面派生呼び出し(Resize)
function DetailFramePageResized() {
    ShipmentParts.ResizeDivArea();
}

ShipmentParts = {
    //定数
    DIV_BODY: "divDetailBodyScroll"
    ,
    DIV_LIST_AREA: "divDetailListArea"
    ,
    DIV_VIEW_AREA: "divDetailViewArea"
    ,
    DIV_VIEW_BOX: "divDetailViewBox"
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
    }
    ,
    //メインエリア画像変更
    ChangeMainAreaImage: function (ctrlId, url) {

        var mainArea = $("#" + ctrlId);
        if (mainArea.attr("src") == url) {
            return;
        }

        $("#" + ctrlId).attr("src", url);
        $("#" + ShipmentParts.DIV_VIEW_BOX).scrollTop(0);
        $("#" + ShipmentParts.DIV_VIEW_BOX).scrollLeft(0);

        //選択行変更
        var ele = $(window.event.srcElement);

        var listItems = $("#" + ShipmentParts.DIV_LIST_AREA).find("." + ShipmentParts.CSS_LIST_ITEM_DIV);
        var selectRow = ele.closest("." + ShipmentParts.CSS_LIST_ITEM_DIV);
        if (0 < selectRow.length) {
            $(listItems).removeClass(ShipmentParts.CSS_SELECTED_DIV);
            $(selectRow).addClass(ShipmentParts.CSS_SELECTED_DIV);
        }
    }
}