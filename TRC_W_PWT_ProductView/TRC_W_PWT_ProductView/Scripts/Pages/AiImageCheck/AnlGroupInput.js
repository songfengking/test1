///////////////////////////////////////////////////////////////////////////////////////////////
// 画像解析グループ入力画面(AnlGroupInput)用制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
//メイン処理からの各画面派生呼び出し(PageLoad)
function MasterMainPageLoaded() {
}
//メイン処理からの各画面派生呼び出し(Resize)
function MasterMainPageResized() {
}

AnlGroupInput = {

	TXT_NOTES: "MasterBody_txtNotes"
	,
	CONST_FRAME: "ifrmModal"
	,
	CONST_PARENT_DISP: "dialog"
	,
	CONST_BUTTON_SEARCH: "MasterBody_btnViewRefresh"
	,
	TH_JYOHO: "thJyoho"
	,
	TD_JYOHO: "tdJyoho"
	,
	IDX: "MasterBody_hiddenNum"
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
	,
	RefreshColumn: function (colId, colNm, colVals) {
		var tmpCol = $("#MasterBody_lJyoho" + colId);
		var tmpColVal = $("#MasterBody_txtJyoho" + colId);
		$(tmpCol).text(colNm);
		$(tmpColVal).val(colVals);
		return false;
	}
	,
	setDisplay: function () {
		var idx = $("#" + this.IDX).val();
		for (let i = 1; i <= idx; i++) {
			var thJyoho = $("#" + this.TH_JYOHO + i);
			var tdJyoho = $("#" + this.TD_JYOHO + i);
			thJyoho.removeClass('print-show');
			tdJyoho.removeClass('print-show');
		}
		return false;
	}
	,
	setColNm: function (colId, colNm) {
		var tmpCol = $("#MasterBody_" + colId);
		$(tmpCol).text(colNm + ":");
		return false;
	}
	,
	setFocus: function (colId) {
		var tmpCol = $("#MasterBody_" + colId);
		$(tmpCol).focus(); 
		return false;
	}

}