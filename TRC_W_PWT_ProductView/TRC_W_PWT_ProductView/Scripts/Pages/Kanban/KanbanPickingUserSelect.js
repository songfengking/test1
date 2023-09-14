///////////////////////////////////////////////////////////////////////////////////////////////
// ピッキング者検索用制御関数
///////////////////////////////////////////////////////////////////////////////////////////////

// メイン処理からの各画面派生呼び出し(PageLoad)
function MasterMainPageLoaded() {
    KanbanPickingUserSelect.Initialize();
}

// メイン処理からの各画面派生呼び出し(Resize)
function MasterMainPageResized() {
    
}

KanbanPickingUserSelect = {
    // 定数
    // 非表示項目：選択位置
    HIDDENFIELD_SELECTED_INDEX: "MasterBody_hdnSelectedIndex",
    // 非表示ユーザ名
    HIDDENFIELD_USER_NM: "MasterBody_hdnPickingUserNm",
    // 非表示ユーザID
    HIDDENFIELD_USER_ID: "MasterBody_hdnPickingUserId",
    // ユーザ名
    TEXT_USER_NM: "MasterBody_txtPickingUserNm",

    // メソッド
    // 初期処理
    Initialize: function () {
        $(window).on("blur", function () { window.focus(); });
    },
    // サイズ変更
    ResizeGridArea: function () {
    },
    // 選択ボタン押下処理
    SelectUser: function () {
        // 選択行を取得する
        var row = $("." + ControlCommon.SELECT_ROW)[0];
        // 選択位置を、存在しない場合は-1、存在する場合はその値にする
        var selectedIndex = (row == null) ? -1 : row.rowIndex;
        // 非表示項目に選択位置をコピーする
        $("#" + this.HIDDENFIELD_SELECTED_INDEX)[0].value = selectedIndex;
    },
    // 選択ボタン押下後処理
    SelectUserUpdate: function () {
        var userNm = $("#" + this.HIDDENFIELD_USER_NM)[0].value;
        var userId = $("#" + this.HIDDENFIELD_USER_ID)[0].value;
        window.opener.document.getElementById(this.HIDDENFIELD_USER_NM).value = userNm;
        window.opener.document.getElementById(this.HIDDENFIELD_USER_ID).value = userId;
        window.opener.document.getElementById(this.TEXT_USER_NM).value = (userNm == "") ? userId : userNm;
        // 画面を閉じる
        ControlCommon.WindowClose();
    }
}