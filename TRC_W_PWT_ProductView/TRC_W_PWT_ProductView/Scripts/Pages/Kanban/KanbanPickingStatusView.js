///////////////////////////////////////////////////////////////////////////////////////////////
// ピッキング状況画面(KanbanPickingStatusView)用制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
// メイン処理からの各画面派生呼び出し(PageLoad)
function MasterMainPageLoaded() {
    KanbanPickingStatusView.Initialize();
}
// メイン処理からの各画面派生呼び出し(Resize)
function MasterMainPageResized() {
    KanbanPickingStatusView.ResizeGridArea2();
}

KanbanPickingStatusView = {
    // 非表示ユーザ名
    HIDDENFIELD_USER_NM: "MasterBody_hdnPickingUserNm",
    // 非表示ユーザID
    HIDDENFIELD_USER_ID: "MasterBody_hdnPickingUserId",
    // ユーザ名
    TEXT_USER_NM: "MasterBody_txtPickingUserNm",

    // 要求日From
    CLD_SENDDATE_FROM: "MasterBody_cldSendDateFrom",
    // 要求時From
    TTB_SENDTIME_FROM: "MasterBody_ttbSendTimeFrom",
    // 要求日To
    CLD_SENDDATE_TO: "MasterBody_cldSendDateTo",
    // 要求時To
    TTB_SENDTIME_TO: "MasterBody_ttbSendTimeTo",

    // 完了日From
    CLD_ENDDATE_FROM: "MasterBody_cldEndDateFrom",
    // 完了時From
    TTB_ENDTIME_FROM: "MasterBody_ttbEndTimeFrom",
    // 完了日To
    CLD_ENDDATE_TO: "MasterBody_cldEndDateTo",
    // 完了時To
    TTB_ENDTIME_TO: "MasterBody_ttbEndTimeTo",

    ResizeGridArea2: function () {
    }
    ,
    Initialize: function () {
        // ピッキング者ロード
        this.LoadPickingUserSelect();
    }
    ,
    // ピッキング者選択表示
    ShowPicikingUserSelect: function () {
        window.open("KanbanPickingUserSelect.aspx", null, "menubar=0,toolbar=0,status=0");
    }
    ,
    // ピッキング者クリア
    ClearPickingUserSelect: function () {
        $("#" + this.HIDDENFIELD_USER_NM)[0].value = "";
        $("#" + this.HIDDENFIELD_USER_ID)[0].value = "";
        $("#" + this.TEXT_USER_NM)[0].value = "";
    }
    ,
    // ピッキング者ロード
    LoadPickingUserSelect: function () {
        // ユーザ名を、非表示ユーザ名が空でない場合は非表示ユーザ名、そうでない場合は非表示ユーザIDの値にする
        var userNm = $("#" + this.HIDDENFIELD_USER_NM)[0].value;
        var userId = $("#" + this.HIDDENFIELD_USER_ID)[0].value;
        $("#" + this.TEXT_USER_NM)[0].value = (userNm != "") ? userNm : userId;
    }
    ,
    // 時刻のみ入力の場合は時刻をクリア
    TimeOnlyInputCheck: function () {

        // 未設定時のフォーマット
        var dateEmptyFormat = "____/__/__";
        var timeEmptyFormat = "__:__:__";

        // 時刻のみ入力されている場合は時刻をクリアする
        // 要求日時From
        if ($("#" + this.CLD_SENDDATE_FROM)[0].value == dateEmptyFormat && $("#" + this.TTB_SENDTIME_FROM)[0].value != timeEmptyFormat) {
            $("#" + this.TTB_SENDTIME_FROM)[0].value = timeEmptyFormat;
        }
        // 要求日時To
        if ($("#" + this.CLD_SENDDATE_TO)[0].value == dateEmptyFormat && $("#" + this.TTB_SENDTIME_TO)[0].value != timeEmptyFormat) {
            $("#" + this.TTB_SENDTIME_TO)[0].value = timeEmptyFormat;
        }
        // 完了日時From
        if ($("#" + this.CLD_ENDDATE_FROM)[0].value == dateEmptyFormat && $("#" + this.TTB_ENDTIME_FROM)[0].value != timeEmptyFormat) {
            $("#" + this.TTB_ENDTIME_FROM)[0].value = timeEmptyFormat;
        }
        // 完了日時To
        if ($("#" + this.CLD_ENDDATE_TO)[0].value == dateEmptyFormat && $("#" + this.TTB_ENDTIME_TO)[0].value != timeEmptyFormat) {
            $("#" + this.TTB_ENDTIME_TO)[0].value = timeEmptyFormat;
        }
    }
    ,
    // -を削除
    DeleteHyphen: function (ele) {
        var textval = ele.value;
        textval = textval.replace(/-/g, '');
        ele.value = textval;
    }
}