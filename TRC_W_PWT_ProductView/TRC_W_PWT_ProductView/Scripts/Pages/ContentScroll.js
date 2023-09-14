///////////////////////////////////////////////////////////////////////////////////////////////
//詳細画面 固定配置用コントロール制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
//メイン処理からの各画面派生呼び出し(PageLoad)
function DetailFramePageLoaded() {
}
//メイン処理からの各画面派生呼び出し(Resize)
function DetailFramePageResized() {
    ContentScroll.ResizeDivArea();
}

ContentScroll = {
    //定数
    DIV_DETAIL_AREA: "divMainListArea"
    ,
    PARENT_DIV_W_MARGIN: 6
    ,
    INNER_DIV_W_MARGIN: 2
    ,
    INNER_DIV_H_MARGIN: 3
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
        var divDetailArea = $("#" + this.DIV_DETAIL_AREA);

        if (0 < divDetailArea.length) {
            //DIVを幅変更する
            try {
                divWSize = containtsWidth - this.PARENT_DIV_W_MARGIN;
                divDetailArea.width(divWSize);
            } catch (e) {
            }
            //DIVを高さ変更する
            if (null != divDetailArea.position()) {
                divTop = divDetailArea.position().top;
            }
            try {
                divHSize = containtsBottom - divTop - this.INNER_DIV_H_MARGIN;
                divDetailArea.height(divHSize);
            } catch (e) {
            }
        }
    }
}