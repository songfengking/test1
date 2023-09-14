///////////////////////////////////////////////////////////////////////////////////////////////
//重要チェック対象外リスト 入力画面(NACheckList)用制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
//メイン処理からの各画面派生呼び出し(PageLoad)
function MasterMainPageLoaded() {
}
//メイン処理からの各画面派生呼び出し(Resize)
function MasterMainPageResized() {
}

NACheckList = {

    TXT_NOTES: "MasterBody_txtNotes"
    ,
    CONST_FRAME: "ifrmModal"
    ,
    CONST_PARENT_DISP: "dialog"
    ,
    CONST_BUTTON_SEARCH: "MasterBodyBottom_btnSearch"
    ,
    //画面CLOSE
    CloseModal: function () {

        parent.$("#" + this.CONST_PARENT_DISP).dialog('close');
        parent.$("#" + this.CONST_BUTTON_SEARCH).trigger('click');
        parent.$("#" + this.CONST_FRAME).remove();


        return true;
    }
    ,
    //入力チェック
    // Enterキーは無効とする(replaceする)
    CheckInput: function (evt) {

        var keyCode = "";
        if (evt) {
            keyCode = evt.keyCode;
        } else {
            keyCode = event.keyCode;
        }

        // Enterキー押下時
        if (keyCode == 13) {
            var tmp = $("#" + this.TXT_NOTES);
            $(tmp)[0].value = $(tmp)[0].value.replace("\r\n", "");
            return false;
        }
        return true;
    }

}