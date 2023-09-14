///////////////////////////////////////////////////////////////////////////////////////////////
// 部品なしかんばん画面(KanbanShotageView)用制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
// メイン処理からの各画面派生呼び出し(PageLoad)
function MasterMainPageLoaded() {
    KanbanShotageView.Initialize();
}
// メイン処理からの各画面派生呼び出し(Resize)
function MasterMainPageResized() {
    KanbanShotageView.ResizeGridArea2();
}

KanbanShotageView = {
    // 定数
    MIN_HEIGHT: 70,
    PARENT_DIV_W_MARGIN: 6,
    INNER_DIV_H_MARGIN: 3,
    GRID_LT: "divLTScroll",
    GRID_RT: "divRTScroll",
    GRID_LB: "divLBScroll",
    GRID_RB: "divRBScroll",
    // 非表示ユーザ名
    HIDDENFIELD_USER_NM: "MasterBody_hdnPickingUserNm",
    // 非表示ユーザID
    HIDDENFIELD_USER_ID: "MasterBody_hdnPickingUserId",
    // ユーザ名
    TEXT_USER_NM: "MasterBody_txtPickingUserNm",
    // 要求日From
    CLD_SENDDATE_FROM: "MasterBody_cldSendDateFrom",
    // 要求時From
    TTB_SENDTIME_FROM: "MasterBody_txtSendTimeFrom",
    // 要求日To
    CLD_SENDDATE_TO: "MasterBody_cldSendDateTo",
    // 要求時To
    TTB_SENDTIME_TO: "MasterBody_txtSendTimeTo",

    // メソッド
    ResizeGridArea2: function () {
        // コンテンツタグの位置を取得
        var containtsBottom = 0;
        var containtsArea = $("#" + MasterMain.CONTAINTS_AREA);
        var containtsBottomArea = $("#" + MasterMain.CONTAINTS_AREA_BOTTOM);
        if (null != containtsArea.position()) {
            containtsBottom = containtsBottomArea.position().top - containtsArea.position().top + KanbanShotageView.MIN_HEIGHT;
            if (null == containtsBottom) {
                containtsBottom = 0;
            }
        }
        // ウィンドウ幅
        var windowWidth = $(document).innerWidth();
        // グリッド用DIVサイズ変更
        var gridLT = $("#" + this.GRID_LT);
        var gridRT = $("#" + this.GRID_RT);
        var gridLB = $("#" + this.GRID_LB);
        var gridRB = $("#" + this.GRID_RB);
        // Table Header幅
        var tblHeaders_Width = 0;
        // Div(右)幅
        var divR_Width = 0;
        // Div(右)位置(横)
        var divR_Left = 0;
        // Div(下)位置(縦)
        var divMain_Top = 0;
        // Div(下)高さ
        var divMain_Height = 0;
        if (0 < gridLT.length) {
            // グリッドのDIVを幅変更する
            if (null != gridLT.position()) {
                divR_Left = gridLT.position().left + gridLT.width();
            }
            try {
                tblHeaders_Width = windowWidth - this.PARENT_DIV_W_MARGIN;
                divR_Width = tblHeaders_Width - divR_Left;
                gridRT.width(divR_Width);
                gridRB.width(divR_Width);
            } catch (e) {
            }
            // グリッドのDIVを高さ変更する
            if (null != gridLB.position()) {
                divMain_Top = gridLB.position().top;
            }
            try {
                divMain_Height = containtsBottom - divMain_Top - KanbanShotageView.INNER_DIV_H_MARGIN;
                gridLB.height(divMain_Height);
                gridRB.height(divMain_Height);
            } catch (e) {
            }
        }
    }
    ,
    Initialize: function () {
        $("#" + KanbanShotageView.GRID_RB).scroll(function () {
            var gridRT = $("#" + KanbanShotageView.GRID_RT);
            var gridLB = $("#" + KanbanShotageView.GRID_LB);
            var gridRB = $("#" + KanbanShotageView.GRID_RB);
            if (gridRB.scrollTop() != gridLB.scrollTop()) {
                gridLB.scrollTop(gridRB.scrollTop());
            }
            if (gridRB.scrollLeft() != gridRT.scrollLeft()) {
                gridRT.scrollLeft(gridRB.scrollLeft());
            }
        });
        $("#" + KanbanShotageView.GRID_LB).scroll(function () {
            var gridLB = $("#" + KanbanShotageView.GRID_LB);
            var gridRB = $("#" + KanbanShotageView.GRID_RB);
            if (gridLB.scrollTop() != gridRB.scrollTop()) {
                gridRB.scrollTop(gridLB.scrollTop());
            }
        });
        $("#" + KanbanShotageView.GRID_LB).on(Common.GetWheelEvent(), function (e) {
            var gridLB = $("#" + KanbanShotageView.GRID_LB);
            gridLB.scrollTop(gridLB.scrollTop() - Common.GetWheelDelta(e));
        });
        $("#" + KanbanShotageView.GRID_RB).on(Common.GetWheelEvent(), function (e) {
            var gridRB = $("#" + KanbanShotageView.GRID_RB);
            gridRB.scrollTop(gridRB.scrollTop() - Common.GetWheelDelta(e));
        });
        // ピッキング者ロード
        this.LoadPickingUserSelect();
    }
    ,
    // ピッキング者選択表示
    ShowPicikingUserSelect: function () {
        window.open("KanbanPickingUserSelect.aspx", null, "menubar=0,toolbar=0,status=0");
    }
    ,
    // ピッキング者クリア
    ClearPickingUserSelect: function () {
        $("#" + this.HIDDENFIELD_USER_NM)[0].value = "";
        $("#" + this.HIDDENFIELD_USER_ID)[0].value = "";
        $("#" + this.TEXT_USER_NM)[0].value = "";
    }
    ,
    // ピッキング者ロード
    LoadPickingUserSelect: function () {
        // ユーザ名を、非表示ユーザ名が空でない場合は非表示ユーザ名、そうでない場合は非表示ユーザIDの値にする
        var userNm = $("#" + this.HIDDENFIELD_USER_NM)[0].value;
        var userId = $("#" + this.HIDDENFIELD_USER_ID)[0].value;
        $("#" + this.TEXT_USER_NM)[0].value = (userNm != "") ? userNm : userId;
    }
    ,
    // 時刻のみ入力の場合は時刻をクリア
    TimeOnlyInputCheck: function () {
        // 未設定時のフォーマット
        var dateEmptyFormat = "____/__/__";
        var timeEmptyFormat = "__:__:__";
        // 時刻のみ入力されている場合は時刻をクリアする
        // 要求日時From
        if ($("#" + this.CLD_SENDDATE_FROM)[0].value == dateEmptyFormat && $("#" + this.TTB_SENDTIME_FROM)[0].value != timeEmptyFormat) {
            $("#" + this.TTB_SENDTIME_FROM)[0].value = timeEmptyFormat;
        }
        // 要求日時To
        if ($("#" + this.CLD_SENDDATE_TO)[0].value == dateEmptyFormat && $("#" + this.TTB_SENDTIME_TO)[0].value != timeEmptyFormat) {
            $("#" + this.TTB_SENDTIME_TO)[0].value = timeEmptyFormat;
        }
    }
    ,
    // -を削除
    DeleteHyphen: function (ele) {
        var textval = ele.value;
        textval = textval.replace(/-/g, '');
        ele.value = textval;
    }
}