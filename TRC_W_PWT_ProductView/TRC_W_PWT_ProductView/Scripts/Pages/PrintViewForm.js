///////////////////////////////////////////////////////////////////////////////////////////////
// 印刷画面用処理
///////////////////////////////////////////////////////////////////////////////////////////////
//メイン処理からの各画面派生呼び出し(PageLoad)
function MasterMainPageLoaded() {
}
//メイン処理からの各画面派生呼び出し(Resize)
function MasterMainPageResized() {
}
//グローバル変数
var zoom = parseInt(0);
var rate = parseInt(1);
var count = parseInt(0);
PrintViewForm = {
    //定数
    DIV_BODY: "Body"
    ,
    DIV_VIEW: "View"
    ,
    DIV_LIST_AREA: "PrintCheckSheet_divListArea"
    ,
    DIV_BOX_AREA: "PrintCheckSheet_divViewBox"
    ,
    DIV_IMAGE_AREA: "PrintCheckSheet_imgMainArea"
    ,
    BTN_REDUCE: "btnReduct"
    ,
    BTN_EXPAND: "btnExpan"
    ,
    DIV_MARGIN: 4
    ,
    CONST_DEFAULT_WIDTH: 1259
    ,
    CONST_DEFAULT_HEIGHT: 1760
    ,
    CSS_PRINT_SHOW: "print-show"
    ,
    CSS_PRINT_VIEW: "print-div-view"
    ,
    loadPrintDisp: function (isPageLoad) {
        if (0 == isPageLoad) {
            //ページ読み込み完了時、各画面内Script呼び出し(呼び出したいスクリプトが存在する時使用)
            if (typeof MasterMainPageLoaded == "function") {
                MasterMainPageLoaded();
            }
        }
        //リサイズ実行時、各画面内Script呼び出し(呼び出したいスクリプトが存在する時使用)
        if (typeof MasterMainPageResized == "function") {
            MasterMainPageResized();
        }

    }
    ,
    //拡大／縮小
    zoomSize: function (no) {
        if (no == 0) {
            count = 0;
            zoom = 0;
            PrintViewForm.imageSize();
            $("#" + PrintViewForm.BTN_REDUCE).prop("disabled", false);
            $("#" + PrintViewForm.BTN_EXPAND).prop("disabled", false);

        } else {
            count = count + parseInt(no);
            zoom = zoom + parseInt(no);
            if (zoom == 0) {
                zoom = 1;
            }
            if (no == -1) {
                zoom = rate * 0.8;
            } else if (no == 1) {
                zoom = rate * 1.2;
            }
            if (count == -5) {
                $("#" + this.BTN_REDUCE).prop("disabled", true);
            } else if (count == 7) {
                $("#" + this.BTN_EXPAND).prop("disabled", true);
            } else {
                $("#" + this.BTN_REDUCE).prop("disabled", false);
                $("#" + this.BTN_EXPAND).prop("disabled", false);
            }
            rate = zoom;
            PrintViewForm.resizeImage(zoom);
        }
    }
    ,
    //メイン画像デフォルトサイズ
    imageSize: function () {
        rate = (document.body.clientWidth - $("#" + this.DIV_LIST_AREA).width() - 37) / PrintViewForm.CONST_DEFAULT_WIDTH;
        PrintViewForm.resizeImage(rate);
    }
    ,
    //リサイズ
    resizeImage: function (rateSize) {

        //画像
        var imgWidth = PrintViewForm.CONST_DEFAULT_WIDTH * rateSize;
        var imgHeight = PrintViewForm.CONST_DEFAULT_HEIGHT * rateSize;
        var viewImage = $("#" + PrintViewForm.DIV_IMAGE_AREA);

        viewImage.width(imgWidth);
        viewImage.height(imgHeight);


        //表示エリア
        var consBottom = 0;

        var divBox = $("#" + PrintViewForm.DIV_BOX_AREA);
        var divList = $("#" + PrintViewForm.DIV_LIST_AREA);
        var divWSize = $(window).width() - divList.width() - 12;

        divBox.width(divWSize);
        divBox.height(divList.height());

    }
    ,
    //印刷
    PrintPreview: function () {

        //印刷前処理
        $("#" + PrintViewForm.DIV_BODY).addClass(PrintViewForm.CSS_PRINT_SHOW);
        $("#" + PrintViewForm.DIV_LIST_AREA).addClass(PrintViewForm.CSS_PRINT_SHOW);
        $("#" + PrintViewForm.DIV_BOX_AREA).addClass(PrintViewForm.CSS_PRINT_SHOW);
        $("#" + PrintViewForm.DIV_VIEW).removeClass(PrintViewForm.CSS_PRINT_VIEW);

        if (window.ActiveXObject == null) {
            // ActiveXObjectにアクセスできないものはモダンブラウザとして、
            // ブラウザ標準の印刷
            window.print();
        } else {
            // ActiveXObjectにアクセス出来る場合、IE11 
            // Windows機能にアクセスし、印刷プレビュー
        var sWebBrowserCode = '<object width="0" height="0" classid="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2"></object>';
            // insertAdjacentHTMLの有無チェックを旧来行っていたが、
            // IE10以降標準搭載のため不要とする。
        document.body.insertAdjacentHTML('beforeEnd', sWebBrowserCode);
        var objWebBrowser = document.body.lastChild;
        if (objWebBrowser == null) {
            return;
        }

        objWebBrowser.ExecWB(7, 1);
        document.body.removeChild(objWebBrowser);
        }

        //印刷後処理
        $("#" + PrintViewForm.DIV_BODY).removeClass(PrintViewForm.CSS_PRINT_SHOW);
        $("#" + PrintViewForm.DIV_LIST_AREA).removeClass(PrintViewForm.CSS_PRINT_SHOW);
        $("#" + PrintViewForm.DIV_BOX_AREA).removeClass(PrintViewForm.CSS_PRINT_SHOW);
        $("#" + PrintViewForm.DIV_VIEW).addClass(PrintViewForm.CSS_PRINT_VIEW);

        }
}
