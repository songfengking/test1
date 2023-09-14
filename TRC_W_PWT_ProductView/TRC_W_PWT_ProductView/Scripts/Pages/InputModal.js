///////////////////////////////////////////////////////////////////////////////////////////////
//メンテナンス系画面(InputModal)用制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
//メイン処理からの各画面派生呼び出し(PageLoad)
function MasterMainPageLoaded() {

    $("#dialog").dialog({
        autoOpen: false,
        resizable: true
    })

    //iframe
    $(".ui-dialog-titlebar-close").hide();
    $("#ifrmArea").append('<iframe id="ifrmModal" frameborder="0"></iframe>');

}
//メンテ画面リサイズ
function MasterMainPageResized() {

    var disp = document.title;
    if (disp.indexOf(InputModal.DISP_TITLE_3C) >= 0) {
        //3Cリサイズ
        MasterMainte3CList.ResizeGridArea();
    } else if (disp.indexOf(InputModal.DISP_TITLE_DPF_DETAIL) >= 0) {
        //DPF詳細リサイズ
        MasterMainteDpfListDetail.ResizeGridArea();
    } else if (disp.indexOf(InputModal.DISP_TITLE_NALIST) >= 0) {
        //重要部品チェック対象外リサイズ
        MasterMainteNAListDetail.ResizeDivArea();

    } else if (disp.indexOf(InputModal.DISP_TITLE_ANL_VIEW) >= 0) {
        //画像解析グループ検索画面 - TRC
        AnlGroupView.Initialize();
        AnlGroupView.ResizeGridArea2();
    } else if (disp.indexOf(InputModal.DISP_TITLE_ANL_GROUPMODEL_VIEW) >= 0) {
        //型式紐づけ検索画面 - TRC
        AnlGroupModelView.Initialize();
        AnlGroupModelView.ResizeGridArea2();
    } else if (disp.indexOf(InputModal.DISP_TITLE_ANL_GROUPMODEL_LIST) >= 0) {
        //型式紐づけ一覧画面
        AnlGroupModelList.Initialize();
        AnlGroupModelList.ResizeGridArea2();
    } else if (disp.indexOf(InputModal.DISP_TITLE_MAIN_VIEW) >= 0) {
        //是正処置入力検査結果一覧画面
        MainCorrectiveView.Initialize();
        MainCorrectiveView.ResizeGridArea2();
    } else if (disp.indexOf(InputModal.DISP_TITLE_ANL_LIST) >= 0) {
        //画像解析グループ構成一覧画面
        AnlGroupList.Initialize();
        AnlGroupList.ResizeGridArea2();
    } else if (disp.indexOf(InputModal.DISP_TITLE_ANL_ITEM_VIEW) >= 0) {
        //画像解析項目検索画面
        AnlItemView.Initialize();
        AnlItemView.ResizeGridArea2();
    }
    
}

InputModal = {

    DISP_ID_NALIST:"NACheckList"
    ,
    DISP_ID_3C: "ProcessingDtEdit"
    ,
    DISP_ID_DPF: "DpfSerial"
    ,
    DISP_TITLE_NALIST:"重要チェック対象外リスト"
    ,
    DISP_TITLE_3C:"3C加工日"
    ,
    DISP_TITLE_DPF: "機番情報"
    ,
    DISP_TITLE_DPF_DETAIL: "排ガス規制部品 詳細"
    ,
    DISP_TITLE_DPF_INPUT: "規制部品 修正"
    ,
    DISP_TITLE_ANL_VIEW: "画像解析グループ検索画面"
    ,
    DISP_TITLE_ANL_LIST: "画像解析グループ項目構成一覧画面"
    ,
    DISP_TITLE_ANL_GROUPMODEL_VIEW: "型式紐づけグループ検索画面"
    ,
    DISP_TITLE_ANL_GROUPMODEL_LIST: "型式紐づけ一覧画面"
    ,
    DISP_TITLE_MAIN_VIEW: "是正処置入力製品検索画面"
    ,
    DISP_TITLE_ANL_ITEM_VIEW:"検査項目マスタ検索画面"
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
    btnOpen: function (dispID, pageUrl, extentedKey) {

        var encodeUrl = encodeURI(pageUrl);

        InputModal.setDialog(dispID, encodeUrl, extentedKey);

        var tmp = parent.$("#dialog");
        tmp.dialog("open");
        return false;
    }
    ,
    //ダイアログ設定
    setDialog: function (dispID, pageUrl, extentedKey) {

        if (dispID == this.DISP_ID_NALIST) {
            //重要チェック対象外リスト登録

            var varTitle = this.DISP_TITLE_NALIST;
            if (extentedKey == "1") {
                varTitle = varTitle + "登録";
            } else if (extentedKey == "2") {
                varTitle = varTitle + "更新";
            } else if (extentedKey == "3") {
                varTitle = varTitle + "削除";
            }

            //dialogの設定
            //共通で定義しておき、自動で設定されるようにする
            $("#dialog").dialog({
                width: 900, height: 370, title: varTitle,
            });

            $("#ifrmModal").attr({
                width:"100%",
                height:"300px",
                src: pageUrl
            });

        } else if (dispID == this.DISP_ID_3C) {
            //3C加工日 修正

            var varTitle = this.DISP_TITLE_3C;
            if (extentedKey == "2") {
                varTitle = varTitle + " 修正";
            }

            //更新パラメータ用テキストの値をリセット
            parent.$("#MasterBody_txtparamDt").val('');
            parent.$("#MasterBody_txtparamNum").val('');
            parent.$("#MasterBody_txtparamLine").val('');
            parent.$("#MasterBody_txtparamRemark").val('');

            //チェック件数取得
            var checkedNum = $("[ID*='chkUpdate'][type='checkbox']:checked");

            if (1 == checkedNum.length) {
                //単一
                pageUrl = pageUrl + "&subIndex=" + 1;
            } else if (1 < checkedNum.length) {
                //複数
                pageUrl = pageUrl + "&subIndex=" + 2;
            } else {
                //チェック無し
                pageUrl = pageUrl + "&subIndex=" + 0;
            }

            //dialogの設定
            $("#dialog").dialog({
                width: 500, height: 390, title: varTitle
            });

            $("#ifrmModal").attr({
                width: "100%", height: "320px", src: pageUrl
            });
        } else if (dispID == this.DISP_ID_DPF) {
            //DPF機番情報メンテ

            var varTitle = this.DISP_TITLE_DPF;
            var subIndex = "";

            var listItems = parent.$("#MasterBody_grvMainViewLB").find(".ui-state-highlight");
            if (0 < listItems.length) {
                subIndex = listItems[0].rowIndex;
            }

            pageUrl = pageUrl + "&subIndex=" + subIndex;
            if (extentedKey == "1") {
                varTitle = varTitle + " 登録";
            } else if (extentedKey == "2") {
                varTitle = varTitle + " 更新";
            }

            //dialogの設定
            $("#dialog").dialog({
                width: 500, height: 350, title: varTitle
            });

            $("#ifrmModal").attr({
                width: "100%", height: "280px", src: pageUrl
            });
        }


    }
}
