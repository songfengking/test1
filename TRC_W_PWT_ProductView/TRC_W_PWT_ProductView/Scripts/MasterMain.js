///////////////////////////////////////////////////////////////////////////////////////////////
//マスターページメイン(MasterMain)用制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
MasterMain = {
    //定数
    CONTAINTS_AREA_TOP: "containts-area-top"
    ,
    CONTAINTS_AREA: "containts-area"
    ,
    CONTAINTS_AREA_BOTTOM: "containts-area-bottom"
    ,
    LABEL_NOW_DT: "lblNowDateTime"
    ,
    //PAGE_LOADイベント
    PageLoaded: function (isPageLoad) {
        this.DisplayTime();
    }
    ,
    //Mainエリアリサイズ
    Resize: function (isPageLoad) {
        if (false == SubmitControl.IsSubmitEnd(true)) {
            setTimeout
            (
                function () {
                    MasterMain.Resize(isPageLoad);
                }
            ,
            50
            );
            return;
        }

        setTimeout
        (
            function () {
                MasterMain.ResizeContaintsArea();
                if (true == isPageLoad) {
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
        0
        );
        setTimeout
        (
            function () {
                //リサイズ実行時、各画面内Script呼び出し(呼び出したいスクリプトが存在する時使用)
                if (typeof MasterMainPageResized == "function") {
                    MasterMainPageResized();
                }
            }
        ,
        50
        );
    }
    ,
    ResizeContaintsArea: function () {

        //上コンテンツコントロール用DIVを取得
        var topArea = $("#" + MasterMain.CONTAINTS_AREA_TOP);       

        //BOTTOMの位置を取得
        var bottomAreaTop = 0;
        var bottomArea = $("#" + MasterMain.CONTAINTS_AREA_BOTTOM);
        if (null != bottomArea.position()) {
            bottomAreaTop = bottomArea.position().top;
            if (null == bottomAreaTop) {
                bottomAreaTop = 0;
            }
        }

        //コンテンツサイズ
        var contentSize = 0;
        //コンテンツ位置
        var contentTop = 0;

        //メインコンテンツ用DIVサイズ変更
        var containtsArea = $("#" + MasterMain.CONTAINTS_AREA);
        //if (0 < containtsArea.outerHeight(true)) {
        if (0 < containtsArea.length) {
            //コンテンツエリアのDIVを高さ変更する
            if (null != containtsArea.position()) {
                contentTop = containtsArea.position().top;
            }
            try {
                contentSize = bottomAreaTop - contentTop;
                containtsArea.height(contentSize);
            } catch (e) {
            }
        }
    }
    ,
    //リサイズ処理呼び出し
    CallResize: function () {
        var slideTm = 0;
        setTimeout
        (
            function () {
                ResizeContaintsArea();
                $(window).trigger('resize');
            }
            ,
            slideTm
        );
    }
    ,
    DisplayTime: function () {
        //現在日時表示
        setInterval
        (
            function () {
                try {
                    $("#" + MasterMain.LABEL_NOW_DT).text(Common.GetPresentTime());
                } catch (e) {
                    //alert(e.message);
                }
            }
            ,
            1000
        );
    }
}