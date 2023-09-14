﻿///////////////////////////////////////////////////////////////////////////////////////////////
//詳細画面 検査ベンチ用コントロール制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
//メイン処理からの各画面派生呼び出し(PageLoad)
function DetailFramePageLoaded() {
    TestBench.Initialize();
}
//メイン処理からの各画面派生呼び出し(Resize)
function DetailFramePageResized() {
    TestBench.ResizeDivArea();
}

TestBench = {
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
    ResizeDivArea: function () {
        // 詳細表示エリアDIVの位置を取得
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
        // Div高さサイズ
        var divHSize = 0;
        // Div幅サイズ
        var divWSize = 0;
        // Div縦位置
        var divTop = 0;
        // Div横位置
        var divLeft = 0;
        // グリッド用DIVサイズ変更
        var divParentArea = $("#" + this.DIV_BODY);
        var divMainArea = $("#" + this.DIV_MAIN_AREA);
        var divSubArea = $("#" + this.DIV_SUB_AREA);
        // 上位DIVを高さ変更
        if (0 < divParentArea.length) {
            // 親DIVを幅変更する
            try {
                divWSize = containtsWidth - this.PARENT_DIV_W_MARGIN;
                divParentArea.width(divWSize);
            } catch (e) {
            }
            // 内部DIVを幅変更する
            try {
                divWSize = divWSize - this.INNER_DIV_W_MARGIN;
                divMainArea.width(divWSize);
                divSubArea.width(divWSize);
            } catch (e) {
            }
            // 親DIVを高さ変更する
            if (null != divParentArea.position()) {
                divTop = divParentArea.position().top;
            }
            try {
                divHSize = containtsBottom - divTop - this.INNER_DIV_MARGIN_3;
                divParentArea.height(divHSize);
            } catch (e) {
            }
            // メイン側DIVを高さ変更する
            if (null != divSubArea.position()) {
                divTop = divSubArea.position().top;
            }
            try {
                divHSize = containtsBottom - divTop - divSubArea.height() - this.INNER_DIV_MARGIN_3;
                divMainArea.height(divHSize);
            } catch (e) {
            }
        }
    }
    ,
    Initialize: function () { }
}