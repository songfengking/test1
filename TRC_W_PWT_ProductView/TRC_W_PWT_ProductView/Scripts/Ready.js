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
    if (typeof MasterMain.PageLoaded == "function") {
        MasterMain.PageLoaded();
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
    InitializeCalendarControl();
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

//カレンダーコントロール初期化
function InitializeCalendarControl() {

    $.datepicker.regional['ja'] = {
        closeText: '閉じる',
        prevText: '&#x3c;前',
        nextText: '次&#x3e;',
        currentText: '今日',
        monthNames: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'],
        monthNamesShort: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'],
        dayNames: ['日曜日', '月曜日', '火曜日', '水曜日', '木曜日', '金曜日', '土曜日'],
        dayNamesShort: ['日', '月', '火', '水', '木', '金', '土'],
        dayNamesMin: ['日', '月', '火', '水', '木', '金', '土'],
        weekHeader: '週',
        dateFormat: 'yy/mm/dd',
        firstDay: 0,
        isRTL: false,
        showMonthAfterYear: true,
        yearSuffix: '年'
    };
    $.datepicker.setDefaults($.datepicker.regional['ja']);
    $(".cld-ymd" + ":text").datepicker();
}