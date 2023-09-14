///////////////////////////////////////////////////////////////////////////////////////////////
//クライアント処理起動制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
//同期、非同期完了イベント
function pageLoad(sender, args) {
    if (typeof CommonPageLoaded == "function") {
        CommonPageSubmitEnd();
    }
}
//初回ロードのみ
$(document).ready(function () {
    if (typeof CommonPageLoaded == "function") {
        CommonPageLoaded(true);
    }
    
});
//共通ページロード完了時処理
function CommonPageSubmitEnd() {

    //ロード完了前呼び出し関数
    //SubmitControl.ClearLoading();

    if (false == SubmitControl.IsSubmitEnd(true)) {
        setTimeout
        (
            function () {
                CommonPageSubmitEnd();
            }
        ,
        50
        );
        return;
    }
    //ロード完了後呼び出し関数
    SubmitControl.ClearLoading();
    ControlCommon.SetInitializeFocus();
}
//共通ページロード初回読み込み完了時処理
function CommonPageLoaded(isFirst) {

    //起動時用スクリプト
    if (true == isFirst) {
    }

    if (false == SubmitControl.IsSubmitEnd(true)) {
        setTimeout
        (
            function () {
                CommonPageLoaded(false);
            }
        ,
        50
        );
        return;
    }
    //以下関数記載
}
