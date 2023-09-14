///////////////////////////////////////////////////////////////////////////////////////////////
//スクリプトマネージャ制御及びローディング処理制御
///////////////////////////////////////////////////////////////////////////////////////////////
//スクリプトマネージャ制御
$(document).ready(function () {

    var mngr = Sys.WebForms.PageRequestManager.getInstance();
    mngr.add_endRequest(
        function (sender, args) {

            if (null != args.get_error()) {
                if (null != args._response && null != args._response._timedOut && true == args._response._timedOut) {
                    //処理がタイムアウトしました。
                } else {
                    //処理が正常に完了しませんでした。
                }
            }

            args.set_errorHandled(true);

        }
    );
    mngr.add_initializeRequest(
        function (sender, args) {
            if (true == mngr.get_isInAsyncPostBack()) {
                args.set_cancel(true);
                return false;
            }
        }
    );
    mngr.add_beginRequest(
        function (sender, args) {
            if (false == SubmitControl.IsSubmitEnd(false)) {
                event.cancelBubble = true;
                return false;
            }

            SubmitControl.ShowLoading(SubmitControl.TYPE_TRANSPARENT);

        }
    );
    $("form").submit(function (sender, args) {

        if (false == SubmitControl.IsSubmitEnd(false)) {
            event.cancelBubble = true;
            return false;
        }

        SubmitControl.ShowLoading(SubmitControl.TYPE_DEFAULT);

    })
})

SubmitControl = {
    //変数
    //サブミット中判定フラグ
    IsSubmit:false
    ,
    //個別指定ローディング種別(サブミット後に都度初期化)
    LoadingType: ""
    ,
    //定数
    LOAD_CTRL: "divLoadingData_"
    ,
    LOAD_BG: "divLoadingBackGround"
    ,
    LOAD_IMAGE: "imgLoadingImage"
    ,
    LOAD_CHAR: "lblLoadingChar"
    ,
    LOAD_TYPE_ATTR: "LoadingType"
    ,
    LOAD_BG_CSS_ATTR: "LoadingBackGroundCss_"
    ,
    LOAD_IMAGE_SRC_ATTR: "LoadingImageSrc_"
    ,
    LOAD_CHAR_TEXT_ATTR: "LoadingCharText_"
    ,
    LOAD_CHAR_CSS_ATTR: "LoadingCharCss_"
    ,
    CSS_POS_BG: "loading-back-ground"
    ,
    CSS_POS_CONTENT: "loading-content"
    ,
    //ローディング種別定義
    TYPE_DEFAULT: "default"
    ,
    TYPE_TRANSPARENT: "transparent"    
    ,
    TYPE_NONE: "none"
    ,
    TYPE_EXIT: "exit"
    ,
    //画面出力終了判定
    IsSubmitEnd: function ( isReadyStateOnly ) {
        if (window.document.readyState != null && window.document.readyState.toUpperCase() != 'COMPLETE') {
            return false;
        }

        if (true == isReadyStateOnly) {
            return true;
        } else {
            return !SubmitControl.IsSubmit;
        }
    }
    ,
    //ローディングタイプ設定
    SetLoadingType: function ( loadTp ) {
        SubmitControl.LoadingType = loadTp;
    }
    ,
    //ローディングタイプクリア
    ClearLoadingType: function () {
        SubmitControl.LoadingType = "";
    }
    ,
    //ローディング表示
    ShowLoading: function (loadingTypeParam) {

        ControlCommon.KeepFocus();

        //ローディング表示なし
        if (this.TYPE_NONE == this.LoadingType) {
            SubmitControl.ClearLoadingType();
            return;
        }

        SubmitControl.IsSubmit = true;
        
        var loadingType = this.LoadingType;
        if ("" == loadingType) {
            loadingType = loadingTypeParam;
        }

        var scrollH = document.documentElement.scrollLeft || document.body.scrollLeft;
        var scrollV = document.documentElement.scrollTop || document.body.scrollTop;
        
        //背景
        var loadingBg = $("#" + this.LOAD_CTRL + this.LOAD_BG);
        var bgAttr = loadingBg.attr(this.LOAD_BG_CSS_ATTR + loadingType);
        if (undefined != bgAttr && "" != bgAttr) {
            loadingBg.addClass(bgAttr);
            loadingBg[0].style.top = scrollV;
            loadingBg[0].style.left = scrollH;
            loadingBg.show();
        }
        //イメージ
        var loadingImage = $("#" + this.LOAD_CTRL + this.LOAD_IMAGE);
        loadingImage.addClass(this.CSS_POS_CONTENT);
        var imgAttr = loadingImage.attr(this.LOAD_IMAGE_SRC_ATTR + loadingType);
        if (undefined != imgAttr && "" != imgAttr) {
            var imgWk = new Image();
            var unixTs = new Date().getTime();
            imgWk.src = imgAttr + "?uts=" + unixTs;
            loadingImage.attr("src", imgWk.src);
        } else {
            loadingImage.attr("src", "");
        }
        loadingImage.hide();

        //表示文字
        var loadingChar = $("#" + this.LOAD_CTRL + this.LOAD_CHAR);
        loadingChar.addClass(this.CSS_POS_CONTENT);
        var charCssAttr = loadingChar.attr(this.LOAD_CHAR_CSS_ATTR + loadingType);
        if (undefined != charCssAttr && "" != charCssAttr) {
            loadingChar.addClass(charCssAttr);
        }
        var charTxtAttr = loadingChar.attr(this.LOAD_CHAR_TEXT_ATTR + loadingType);
        if (undefined != charTxtAttr && "" != charTxtAttr) {
            loadingChar.text(charTxtAttr);
        }
        loadingChar.hide();

        SubmitControl.SetRelativePosition(loadingImage, loadingChar);
    }
    ,
    //ローディング表示
    SetRelativePosition: function (eleImg, eleChar) {

        if (0 == eleImg.length || 0 == eleChar.length) {
            return;
        }

        if (undefined != eleImg.attr("src") && "" != eleImg.attr("src")) {            
            //if (eleImg[0].complete != true
            //    && (undefined != eleImg[0].readyState && eleImg[0].readyState.toUpperCase() != 'COMPLETE')) {
            if (eleImg[0].complete != true) {
                setTimeout
                    (
                        function () {
                            SubmitControl.SetRelativePosition(eleImg, eleChar);
                        }
                        ,
                        50
                    );
                return;
            }
        }

        var scrollH = document.documentElement.scrollLeft || document.body.scrollLeft;
        var scrollV = document.documentElement.scrollTop || document.body.scrollTop;
        
        var charAreaWidth = 0;
        var charAreaHeight = 0;

        var loadChar = eleChar.text();
        if ("" != loadChar) {
            var fontSize = eleChar.css("font-size").replace('px','');
            charByte = SubmitControl.RetMaxRowByteLength(loadChar);
            charAreaWidth = ((fontSize * (charByte / 2) / 2) - scrollH);
            charAreaHeight = eleChar.height();
        }

        var imageWidth = 0;
        var imageHeight = 0;
        if (undefined != eleImg.attr("src") && "" != eleImg.attr("src")) {
            imageWidth = eleImg.width();
            imageHeight = eleImg.height();

            eleImg[0].style.marginTop = -1 * ((imageHeight / 2) - scrollV) + "px";
            eleImg[0].style.marginLeft = (-1 * charAreaWidth) - (imageWidth / 2) + "px";

            eleImg.show();
        }

        if ("" != loadChar) {
            eleChar[0].style.marginTop = (-1 * (charAreaHeight / 2)) + "px";
            if (0 < imageWidth) {
                eleChar[0].style.marginLeft = (-1 * charAreaWidth) + (imageWidth / 2) + "px";
            } else {
                eleChar[0].style.marginLeft = (-1 * charAreaWidth) + "px";
            }

            eleChar.show();
        }

    }
    ,
    //ローディング解除
    ClearLoading: function () {

        //背景
        var loadingBg = $("#" + this.LOAD_CTRL + this.LOAD_BG);
        loadingBg.removeClass();
        loadingBg.addClass(this.CSS_POS_BG);
        loadingBg.hide();

        //イメージ
        var loadingImage = $("#" + this.LOAD_CTRL + this.LOAD_IMAGE);
        loadingImage.removeClass();
        loadingImage.attr("src", "");
        loadingImage.hide();
        if (0 < loadingImage.length) {
            loadingImage[0].style.marginTop = 0;
            loadingImage[0].style.marginLeft = 0;
        }
        
        //表示文字
        var loadingChar = $("#" + this.LOAD_CTRL + this.LOAD_CHAR);
        loadingChar.removeClass();
        loadingChar.text("");
        loadingChar.hide();
        if (0 < loadingChar.length) {
            loadingChar[0].style.marginTop = 0;
            loadingChar[0].style.marginLeft = 0;
        }

        SubmitControl.ClearLoadingType();
        SubmitControl.IsSubmit = false;

    }
    ,
    //改行単位最長Byte型文字列長変換
    RetMaxRowByteLength:function(str) {
        var maxByte = 0;
        var byte = 0;
        var chr;

        var strArr = str.split("\n");
        var jj = 0;
        for (jj = 0; jj < strArr.length; jj++)
        {
            byte = SubmitControl.RetByteLength(strArr[jj]);
            if (byte > maxByte)
            {
                maxByte = byte;
            }
        }
        return maxByte;
    }
    ,
    //Byte型文字列長変換
    RetByteLength: function (str) {

        var byte = 0;
        var chr;

        var ii = 0;
        for (ii = 0; ii < str.length; ii++) {
            chr = escape(str.charAt(ii));
            if (chr.length < 4) {
                byte++;
            } else {
                byte += 2;
            }
        }
        return byte;
    }
}
