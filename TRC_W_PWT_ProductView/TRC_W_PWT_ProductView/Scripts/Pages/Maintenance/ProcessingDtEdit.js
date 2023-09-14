///////////////////////////////////////////////////////////////////////////////////////////////
//3C加工日修正 入力画面(ProcessingDtEdit)用制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
//メイン処理からの各画面派生呼び出し(PageLoad)
function MasterMainPageLoaded() {
}
//メイン処理からの各画面派生呼び出し(Resize)
function MasterMainPageResized() {
}
ProcessingDtEdit = {

    CONST_FRAME: "ifrmModal"
    ,
    CONST_PARENT_DISP: "dialog"
    ,
    CONST_BUTTON_SEARCH: "MasterBody_btnSearch"
    ,
    CONST_BUTTON_UPDATE: "MasterBodyBottom_btnUpdate"
    ,
    CONST_BUTTON_PROC_DT_ERROR: "MasterBody_btnProcDtError"
    ,
    CONST_BUTTON_PROC_NUM_ERROR: "MasterBody_btnProcNumError"
    ,
    CONST_BUTTON_PROC_LINE_ERROR: "MasterBody_btnProcLineError"
    ,
    CONST_BUTTON_REMARK_ERROR: "MasterBody_btnRemarkError"
    ,
    CONST_BUTTON_CLEAR: "MasterBody_btnClear"
    ,
    CONST_BUTTON_CHECK: "MasterBody_btnInputCheck"
    ,
    TXT_PROC_DT: "MasterBody_txtUpdDt"
    ,
    TXT_PROC_NUM: "MasterBody_txtUpdNum"
    ,
    TXT_PROC_LINE: "MasterBody_txtUpdLine"
    ,
    TXT_REMARK: "MasterBody_txtRemark"
    ,
    //画面CLOSE
    CloseModal: function () {
        try{
            parent.$("#" + this.CONST_PARENT_DISP).dialog('close');
            parent.$("#" + this.CONST_BUTTON_SEARCH).trigger('click');
        } finally {
            parent.$("#" + this.CONST_FRAME).remove();
        }

        return true;
    }
    ,
    //カーソル移動(加工日)
    CheckUpdateDt: function () {

        $("#" + this.CONST_BUTTON_CLEAR).trigger('click');

        //入力データ
        var tmp = $("#" + ProcessingDtEdit.TXT_PROC_DT);

        if (tmp.val().length == 8) {
            var vYear = tmp.val().substr(0, 4);
            var vMonth = tmp.val().substr(4, 2) - 1;  // Javascriptは、0-11で表現
            var vDay = tmp.val().substr(6, 2);

            if (vMonth >= 0 && vMonth <= 11 && vDay >= 1 && vDay <= 31) {
                var vDt = new Date(vYear, vMonth, vDay);

                if (isNaN(vDt)) {
                    $(tmp).val();

                    return false;
                } else if (vDt.getFullYear() == vYear && vDt.getMonth() == vMonth && vDt.getDate() == vDay) {
                    //正常
                    parent.$("#MasterBody_txtparamDt").val(tmp.val());
                } else {
                    $("#" + this.CONST_BUTTON_PROC_DT_ERROR).trigger('click');
                    return false;
                }
            } else {
                $("#" + this.CONST_BUTTON_PROC_DT_ERROR).trigger('click');
                return false;
            }
        } else {
            $("#" + this.CONST_BUTTON_PROC_DT_ERROR).trigger('click');
            return false;
        }
        return true;
    }
    ,
    //カーソル移動(連番)
    CheckUpdateNum: function () {

        $("#" + this.CONST_BUTTON_CLEAR).trigger('click');

        //連番入力チェック
        var tmp = $("#" + ProcessingDtEdit.TXT_PROC_NUM);

        if (tmp.val().length == 0 || tmp.val().length > 3) {
            //桁数チェック
            $("#" + this.CONST_BUTTON_PROC_NUM_ERROR).trigger('click');
            return false;
        } else {
            if (isNaN(tmp.val())) {
                //数字チェック
                $("#" + this.CONST_BUTTON_PROC_NUM_ERROR).trigger('click');
                return false;
            } else {
                //正常
                parent.$("#MasterBody_txtparamNum").val(tmp.val());
            }
        }
        return true;
    }
    ,
    //カーソル移動(加工ライン)
    CheckUpdateLine: function () {

        $("#" + this.CONST_BUTTON_CLEAR).trigger('click');

        //連番入力チェック
        var tmp = $("#" + ProcessingDtEdit.TXT_PROC_LINE);

        if (tmp.val().length == 0 || tmp.val().length > 4) {
            //桁数チェック
            $("#" + this.CONST_BUTTON_PROC_LINE_ERROR).trigger('click');
            return false;
        } else {
            //正常
            parent.$("#MasterBody_txtparamLine").val(tmp.val());
        }
        return true;
    }
    ,
    //カーソル移動
    CheckRemark: function () {

        $("#" + this.CONST_BUTTON_CLEAR).trigger('click');

        //備考入力桁数チェック
        tmp = $("#" + ProcessingDtEdit.TXT_REMARK);

        if (ProcessingDtEdit.CountLength(tmp.val()) > 200) {
            $("#" + this.CONST_BUTTON_REMARK_ERROR).trigger('click');
            return false;
        } else if (tmp.val().length == 0) {
            $("#" + this.CONST_BUTTON_REMARK_ERROR).trigger('click');
            return false;
        } else {
            //正常
            parent.$("#MasterBody_txtparamRemark").val(tmp.val());
        }
        return true;
    }
    ,
    //文字列のバイト数チェック
    CountLength:function(str) { 
        var r = 0; 
        for (var i = 0; i < str.length; i++) {
            var c = str.charCodeAt(i); 
            // Shift_JIS: 0x0 ～ 0x80, 0xa0 , 0xa1 ～ 0xdf , 0xfd ～ 0xff 
            // Unicode : 0x0 ～ 0x80, 0xf8f0, 0xff61 ～ 0xff9f, 0xf8f1 ～ 0xf8f3 
            if ( (c >= 0x0 && c < 0x81) || (c == 0xf8f0) || (c >= 0xff61 && c < 0xffa0) || (c >= 0xf8f1 && c < 0xf8f4)) { 
                r += 1; 
            } else { 
                r += 2; 
            } 
        } 
    return r; 
    }
    ,
    //親画面更新処理実行
    Update3CProcDt: function () {

        try {
            //更新処理
            parent.$("#" + this.CONST_BUTTON_UPDATE).trigger('click');
        } finally {
            //入力画面close
            ProcessingDtEdit.CloseModal();
        }
        
        return true;
    }
    ,
    //半角英数字判定
    inputCheck: function (val) {
        if (val.match(/[^A-Za-z0-9]+/)) {
            //半角英数字以外の文字が存在する場合、エラー
            return false;
        }
        return true;
    }
}
