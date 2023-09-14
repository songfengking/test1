///////////////////////////////////////////////////////////////////////////////////////////////
//  重要部品チェック対象外リスト 詳細画面(MasterMainteNAListDetail)用制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
//メイン処理からの各画面派生呼び出し(PageLoad)
function MasterMainPageLoaded() {
}
//メイン処理からの各画面派生呼び出し(Resize)
function MasterMainPageResized() {
    MasterMainteNAListDetail.ResizeDivArea();
}
$(document).ready(
    function () {
        // 処理
        MasterMainteNAListDetail.ResizeDivArea();
    });
MasterMainteNAListDetail = {
    //定数
    DIV_BODY: "divDetailBodyScroll"
    ,
    DIV_MAIN_AREA: "divMainListArea"
    ,
    DIV_OUT_SCROLL: "outMainScroll"
    ,
    DIV_MARGIN_6: 6
    ,
    INNER_DIV_MARGIN_2: 2
    ,
    INNER_DIV_MARGIN_3: 3
    ,
    INNER_DIV_MARGIN_15: 15
    ,
    ResizeDivArea: function () {

        //コンテンツタグの位置を取得
        var containtsBottom = 0;
        var containtsWidth = 0;
        var containtsArea = $("#" + MasterMain.CONTAINTS_AREA);
        var containtsBottomArea = $("#" + MasterMain.CONTAINTS_AREA_BOTTOM);
        if (null != containtsArea.position()) {
            containtsBottom = containtsBottomArea.position().top - containtsArea.position().top;
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
        var divParentArea = $("#" + MasterMainteNAListDetail.DIV_BODY);
        var divMainArea = $("#" + MasterMainteNAListDetail.DIV_MAIN_AREA);
        var divOutScroll = $("#" + MasterMainteNAListDetail.DIV_OUT_SCROLL);

        var scroll = divOutScroll.scrollLeft();


        //上位DIVを高さ変更
        if (0 < divParentArea.length) {
            //親DIVを幅変更する
            try {
                divWSize = containtsWidth - this.DIV_MARGIN_6;
                divParentArea.width(divWSize);
            } catch (e) {
            }
            //内部DIVを幅変更する
            try {
                divWSize = divWSize - this.INNER_DIV_MARGIN_2;
                divMainArea.width(divWSize + scroll);
                divOutScroll.width(divWSize);
            } catch (e) {
            }
            //サブ側DIVを高さ変更する
            if (null != divParentArea.position()) {
                divTop = divParentArea.position().top;
            }
            try {
                divHSize = containtsBottom - divTop;
                divMainArea.height(divHSize);
            } catch (e) {
                alert(e.message);
            }
        }
    }
}