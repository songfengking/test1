///////////////////////////////////////////////////////////////////////////////////////////////
// 工程絞込検索画面(ProcessFilteringView)用制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
//メイン処理からの各画面派生呼び出し(PageLoad)
function MasterMainPageLoaded() {
}
//メイン処理からの各画面派生呼び出し(Resize)
function MasterMainPageResized() {
    ProcessFilteringView.Initialize();
}

ProcessFilteringView = {
    // 定数
    // 非表示項目：選択位置
    HIDDENFIELD_SELECTED_INDEX: "MasterBody_hdnSelectedIndex",
    // 非表示項目：ラインコード
    HIDDENFIELD_LINE_CD: "MasterBody_hdnLineCd",
    // 非表示項目：工程コード
    HIDDENFIELD_PROCESS_CD: "MasterBody_hdnProcessCd",
    // 非表示項目：工程名
    HIDDENFIELD_PROCESS_NM: "MasterBody_hdnProcessNm",
    // 非表示項目：作業コード
    HIDDENFIELD_WORK_CD: "MasterBody_hdnWorkCd",
    // 非表示項目：作業名
    HIDDENFIELD_WORK_NM: "MasterBody_hdnWorkNm",
    // 非表示項目：検索対象フラグ
    HIDDENFIELD_SEARCH_TARGET_FLG: "MasterBody_hdnSearchTargetFlg",
    // 親画面：ラインコード
    PARENT_LINE_CD: "MasterBody_hdnLineCd",
    // 親画面：工程コード
    PARENT_PORCESS_CD: "MasterBody_hdnProcessCd",
    // 親画面：工程名
    PARENT_PROCESS_NM: "MasterBody_hdnProcessNm",
    // 親画面：作業コード
    PARENT_WORK_CD: "MasterBody_hdnWorkCd",
    // 親画面：作業名
    PARENT_WORK_NM: "MasterBody_hdnWorkNm",
    // 親画面：検索対象フラグ
    PARENT_SEARCH_TARGET_FLG: "MasterBody_hdnSearchTargetFlg",
    // 親画面：工程変更ボタン
    PARENT_CHANGE_PROCESS: "MasterBody_btnChangeProcess",
    
    // メソッド
    // 初期処理
    Initialize: function () {
        $(window).on("blur", function () { window.focus(); });
    },
    // 選択ボタン押下前処理
    BeforeSelect: function () {
        // 工程作業一覧の選択位置を取得
        var selectedIndex = -1;
        var row = $("." + ControlCommon.SELECT_ROW)[0];
        if (row == null) {
            // 選択行が存在しない場合
            selectedIndex = -1;
        } else {
            // 選択行が存在する場合
            // 先頭行が1であるため、0から開始するようにする
            selectedIndex = row.rowIndex - 1;
        }
        // 取得した選択位置を非表示項目：選択位置に設定
        var hdnSelectedIndex = $("#" + this.HIDDENFIELD_SELECTED_INDEX)[0];
        hdnSelectedIndex.value = selectedIndex;
        return true;
    },
    // 選択ボタン押下後処理
    AfterSelect: function () {
        // 親画面に値を設定
        window.opener.document.getElementById(this.PARENT_LINE_CD).value = $("#" + this.HIDDENFIELD_LINE_CD)[0].value;
        window.opener.document.getElementById(this.PARENT_PORCESS_CD).value = $("#" + this.HIDDENFIELD_PROCESS_CD)[0].value;
        window.opener.document.getElementById(this.PARENT_PROCESS_NM).value = $("#" + this.HIDDENFIELD_PROCESS_NM)[0].value;
        window.opener.document.getElementById(this.PARENT_WORK_CD).value = $("#" + this.HIDDENFIELD_WORK_CD)[0].value;
        window.opener.document.getElementById(this.PARENT_WORK_NM).value = $("#" + this.HIDDENFIELD_WORK_NM)[0].value;
        window.opener.document.getElementById(this.PARENT_SEARCH_TARGET_FLG).value = $("#" + this.HIDDENFIELD_SEARCH_TARGET_FLG)[0].value;
        window.opener.MainView.DoAfterLoad();
        window.opener.document.getElementById(this.PARENT_CHANGE_PROCESS).click();
        // 画面を閉じる
        ControlCommon.WindowClose();
    },
}