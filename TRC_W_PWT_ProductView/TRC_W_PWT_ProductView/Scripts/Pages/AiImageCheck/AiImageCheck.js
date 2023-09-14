///////////////////////////////////////////////////////////////////////////////////////////////
//AI外観検査画面(AiImageCheck)用制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
AiImageCheck = {

	CONST_PARENT_DISP: "dialog"
	,
	//画面CLOSE
	CloseModal: function () {
		var parentDisp = "#ifrmModal";
		$("#dialog").dialog('close');
		$(parentDisp).remove();
		return true;
	}
	,
	// パラメータ：1.画面ID、2.画面URL、3.任意のkey
	gridDouClickOpen: function (dispID, pageUrl, extentedKey) {
		var encodeUrl = encodeURI(pageUrl);
		AiImageCheck.setDialog(dispID, encodeUrl, extentedKey);
		var tmp = parent.$("#dialog");
		tmp.dialog("open");
		return false;
	}
	,
	//ダイアログ設定
	setDialog: function (dispID, pageUrl, extentedKey) {
		//dialogの設定
		//共通で定義しておき、自動で設定されるようにする
		$("#dialog").dialog({
			width: 1270, height: 1000, title: extentedKey,
		});

		$("#ifrmModal").attr({
			width:  "1250",
			height: "930",
			src: pageUrl
		});
	}
	,
	// パラメータ：1.画面ID、2.画面URL、3.任意のkey
	gridDouClickInputOpen: function (dispID, pageUrl, extentedKey) {
		parent.$("#" + this.CONST_PARENT_DISP).dialog('close');
		var encodeUrl = encodeURI(pageUrl);
		//dialogの設定
		//共通で定義しておき、自動で設定されるようにする
		parent.$("#dialog").dialog({
			width: 720, height: 460, title: extentedKey,
		});

		parent.$("#ifrmModal").attr({
			width: "680",
			height: "380",
			src: pageUrl
		});
		var tmp = parent.$("#dialog");
		tmp.dialog("open");
		return false;
	}
	,
	// パラメータ：1.画面ID、2.画面URL、3.任意のkey
	gridDouClickInputOpen1: function (dispID, pageUrl, extentedKey) {
		//parent.$("#" + this.CONST_PARENT_DISP).dialog('close');
		var encodeUrl = encodeURI(pageUrl);
		//dialogの設定
		//共通で定義しておき、自動で設定されるようにする
		parent.$("#dialog").dialog({
			width: 720, height: 460, title: extentedKey,
		});

		parent.$("#ifrmModal").attr({
			width: "680",
			height: "380",
			src: encodeUrl
		});
		var tmp = parent.$("#dialog");
		tmp.dialog("open");
		return false;
	}
	,
	// パラメータ：1.画面ID、2.画面URL、3.任意のkey
	btnClickBackOpen: function (dispID, pageUrl, extentedKey) {
		parent.$("#" + this.CONST_PARENT_DISP).dialog('close');
		var encodeUrl = encodeURI(pageUrl);
		//dialogの設定
		//共通で定義しておき、自動で設定されるようにする
		parent.$("#dialog").dialog({
			width: 1270, height: 1000, title: extentedKey,
		});

		parent.$("#ifrmModal").attr({
			width: "1250",
			height: "930",
			src: pageUrl
		});
		var tmp = parent.$("#dialog");
		tmp.dialog("open");
		return false;
	}
}
