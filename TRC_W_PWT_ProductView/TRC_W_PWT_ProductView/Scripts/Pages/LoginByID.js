///////////////////////////////////////////////////////////////////////////////////////////////
//ログイン画面(LoginByID)用制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
//メイン処理からの各画面派生呼び出し(PageLoad)
function MasterMainPageLoaded() {
}
//メイン処理からの各画面派生呼び出し(Resize)
function MasterMainPageResized() {
}

LoginByID = {
    //定数
    BTN_LOGIN: "MasterBody_btnLogin"
    ,
    TXT_USER_ID: "MasterBody_txtUserID"
    ,
    TXT_PASSWORD: "MasterBody_txtPassword"
    ,
    CheckInput: function (evt) {

        var keyCode = "";
        if (evt) {
            keyCode = evt.keyCode;
        } else {
            keyCode = event.keyCode;
        }

        var id = $("#" + this.TXT_USER_ID).val();
        var pass = $("#" + this.TXT_PASSWORD).val();
        // Enterキー押下時
        if (keyCode == 13) {
            if ("" == id) {
                ControlCommon.SetFocus(this.TXT_USER_ID);
                return false;
            } else if ("" == pass) {
                ControlCommon.SetFocus(this.TXT_PASSWORD);
                return false;
            }
            $("#" + this.BTN_LOGIN).click();
        }
        return true;
    }
}