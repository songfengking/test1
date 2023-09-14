//////////////////////////////////////////////////////////////////////////////////////////////
//詳細画面 外枠(DetailPartsFrame)用コントロール制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
//メイン処理からの各画面派生呼び出し(PageLoad)
function MasterMainPageLoaded() {
    DetailPartsFrame.PreLoadCSSImage();
    if (typeof DetailPartsFramePageLoaded == "function") {
        DetailPartsFramePageLoaded();
    }
}
//メイン処理からの各画面派生呼び出し(Resize)
function MasterMainPageResized() {
    DetailPartsFrame.ResizeDivArea();
    if (typeof DetailPartsFramePageResized == "function") {
        DetailPartsFramePageResized();
    }
}

DetailPartsFrame = {
    //定数
    DIV_BODY: "divBodyScroll"
    ,
    DIV_LIST_AREA: "divListArea"
    ,
    DIV_LIST_CONTENT: "divContentList"
    ,
    DIV_VIEW_AREA: "divViewArea"
    ,
    DIV_VIEW_BOX: "divViewBox" 
    ,
    PARENT_DIV_W_MARGIN: 6
    ,
    INNER_DIV_W_MARGIN: 2
    ,
    INNER_DIV_H_MARGIN: 3
    ,
    LIST_ROW_CSS: "btn-list-content"
    ,
    SELECT_ROW_CSS: "btn-list-content-selected"
    ,
    ResizeDivArea: function () {

        //コンテンツタグの位置を取得
        var containtsBottom = 0;
        var containtsArea = $("#" + MasterMain.CONTAINTS_AREA);
        if (null != containtsArea.position()) {
            containtsBottom = containtsArea.position().top + containtsArea.height() -10;
            if (null == containtsBottom) {
                containtsBottom = 0;
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
        var divListArea = $("#" + this.DIV_LIST_AREA);
        var divListContentArea = $("#" + this.DIV_LIST_CONTENT);
        var divViewArea = $("#" + this.DIV_VIEW_AREA);
        var divDetailBoxArea = $("#" + this.DIV_VIEW_BOX);
                        
        //上位DIVを高さ変更
        if (0 < divParentArea.length) {
            //親DIVを幅変更する
            try {
                divWSize = windowWidth - this.PARENT_DIV_W_MARGIN;
                divParentArea.width(divWSize);
            } catch (e) {
            }
            //詳細側DIVを幅変更する
            if (null != divListArea.position()) {
                divLeft = divListArea.position().left + divListArea.width();
            }
            try {
                divWSize = windowWidth - divLeft - this.PARENT_DIV_W_MARGIN;
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
            //内部DIVサイズを高さ変更する
            //リスト側一覧コンテンツ欄
            if (null != divListContentArea.position()) {
                divTop = divListContentArea.position().top;
            }
            try {
                divHSize = containtsBottom - divTop - this.INNER_DIV_H_MARGIN - this.INNER_DIV_H_MARGIN;
                divListContentArea.height(divHSize);
            } catch (e) {
            }
            //詳細子画面表示欄
            if (null != divDetailBoxArea.position()) {
                divTop = divDetailBoxArea.position().top;
            }
            try {
                divHSize = containtsBottom - divTop - this.INNER_DIV_H_MARGIN - this.INNER_DIV_H_MARGIN;
                divDetailBoxArea.height(divHSize);
            } catch (e) {
            }
        }
    }
    ,
    //詳細項目選択リスト行選択
    SelectContentList: function (ele) {

        var divListContentArea = $("#" + this.DIV_LIST_CONTENT);
        var preSelect = $(divListContentArea).find("." + this.SELECT_ROW_CSS);
        var listItems = $(divListContentArea).find("." + this.LIST_ROW_CSS);

        var selectRow = $(ele);

        if (0 < preSelect.length) {
            if (0 < selectRow.length
                && preSelect[0] == selectRow[0]) {
                return false;
            }
        }

        if (0 < selectRow.length) {
            $(listItems).removeClass(this.SELECT_ROW_CSS);
            $(selectRow).addClass(this.SELECT_ROW_CSS);
            return true;
        }

        return false;
    }
    ,
    PreLoadCSSImage: function () {
        var objWk = new Object();
        $(objWk).addClass(this.SELECT_ROW_CSS);
        objWk = null;
    }
}