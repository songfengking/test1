///////////////////////////////////////////////////////////////////////////////////////////////
// 排ガス機番部品 入力画面(DpfSerial)用制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
//メイン処理からの各画面派生呼び出し(PageLoad)
function MasterMainPageLoaded() {
}
//メイン処理からの各画面派生呼び出し(Resize)
function MasterMainPageResized() {
}
$(document).ready(
    function () {
        // 処理
        DpfSerial.Initialize();
    });
DpfSerial = {

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

        parent.$("#" + DpfSerial.CONST_PARENT_DISP).dialog('close');
        parent.$("#" + DpfSerial.CONST_BUTTON_SEARCH).trigger('click');
        parent.$("#" + DpfSerial.CONST_FRAME).remove();


        return true;
    }
    ,
    //入力チェック
    CheckInput: function (evt) {

        var keyCode = "";
        if (evt) {
            keyCode = evt.keyCode;
        } else {
            keyCode = event.keyCode;
        }

        return true;
    }
    , Initialize: function () {
        //処理なし
    }
}