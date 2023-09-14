///////////////////////////////////////////////////////////////////////////////////////////////
// 型式IDNOダイアログ用制御関数
///////////////////////////////////////////////////////////////////////////////////////////////

// メイン処理からの各画面派生呼び出し(PageLoad)
function MasterMainPageLoaded() {
}

// メイン処理からの各画面派生呼び出し(Resize)
function MasterMainPageResized() {
    ProCodeIdnoDialog.Initialize();
}

ProCodeIdnoDialog = {
    // メソッド
    // 初期処理
    Initialize: function () {
        $(window).on("blur", function () { window.focus(); });
    },
    // サイズ変更
    ResizeGridArea: function () {
    },
}