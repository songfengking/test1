///////////////////////////////////////////////////////////////////////////////////////////////
//パスワード変更画面(ChangePassword)用制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
//メイン処理からの各画面派生呼び出し(PageLoad)
function MasterMainPageLoaded() {
}
//メイン処理からの各画面派生呼び出し(Resize)
function MasterMainPageResized() {
}

ChangePassword = {
    //定数
    BTN_EDIT: "MasterBody_btnEdit"
    ,
    TXT_USER_ID: "MasterBody_txtUserID"
    ,
    TXT_OLD_PASSWORD: "MasterBody_txtOldPassword"
    ,
    TXT_NEW_PASSWORD: "MasterBody_txtNewPassword"
    ,
    TXT_NEW_PASSWORD_CONFIRM: "MasterBody_txtNewPasswordConfirm"
    ,
    LBL_CHECK_PASSWORD: "MasterBody_lblCheckPassword"
    ,
    CheckInput: function (evt) {

        var keyCode = "";
        if (evt) {
            keyCode = evt.keyCode;
        } else {
            keyCode = event.keyCode;
        }

        var userid = $("#" + this.TXT_USER_ID).val();
        var oldPW = $("#" + this.TXT_OLD_PASSWORD).val();
        var newPW = $("#" + this.TXT_NEW_PASSWORD).val();
        var newPWConf = $("#" + this.TXT_NEW_PASSWORD_CONFIRM).val();
        var lblCheck = $("#" + this.LBL_CHECK_PASSWORD);
        //新パスワードチェック
        //新パスワード再が入力されているか確認
        if ("" != newPWConf) {
            //新パスワードが一致しているか確認
            if (newPW == newPWConf) {
                lblCheck.text("✔");
            } else {
                lblCheck.text("×");
            }
        } else {
            lblCheck.text("");
        }

        // Enterキー押下時
        if (keyCode == 13) {
            if ("" == userid) {
                ControlCommon.SetFocus(this.TXT_USER_ID);
                return false;
            } else if ("" == oldPW) {
                ControlCommon.SetFocus(this.TXT_OLD_PASSWORD);
                return false;
            } else if ("" == newPW) {
                ControlCommon.SetFocus(this.TXT_NEW_PASSWORD);
                return false;
            } else if ("" == newPWConf) {
                ControlCommon.SetFocus(this.TXT_NEW_PASSWORD_CONFIRM);
                return false;
            }
            $("#" + this.BTN_EDIT).click();
        }
        return true;
    }
}