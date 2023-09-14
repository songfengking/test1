///////////////////////////////////////////////////////////////////////////////////////////////
// 是正処置入力画面(MainCorrectiveInput)用制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
//メイン処理からの各画面派生呼び出し(PageLoad)
function MasterMainPageLoaded() {
}
//メイン処理からの各画面派生呼び出し(Resize)
function MasterMainPageResized() {
}

MainCorrectiveInput = {

    TXT_NOTES: "MasterBody_txtCorrectiveComment"
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
            $(tmp).val($(tmp).val().replace("\r\n", ""));
            return false;
        }

        var tmpTxt = $("#" + this.TXT_NOTES);
        if ($(tmpTxt).val().length > 100) {
            $(tmpTxt).val($(tmpTxt).val().substring(0, 100));
            return false;
        }
        return true;
    }
    ,
    //入力チェック
    CheckInputBlur: function (evt) {

        var tmpTxt = $("#" + this.TXT_NOTES);
        if ($(tmpTxt).val().length > 100) {
            $(tmpTxt).val($(tmpTxt).val().substring(0, 100));
            return false;
        }
        return true;
    }

}