KTExpander = {
    // クリック
    headerClick: function (expanderContent, imgOpenClose, imgfileOpen, imgfileClose, currentPage) {
        var slideTm = 0;
        var fileName = "";
        //閉じられているか確認
        if ("none" == $("#" + expanderContent).css('display')) {
            fileName = imgfileClose;
            $("#" + expanderContent).slideDown(slideTm);
        } else {
            fileName = imgfileOpen;
            $("#" + expanderContent).slideUp(slideTm);
        }
        //srcのURLを分割
        var urlList = $("#" + imgOpenClose).prop("src").split("/");
        //最後尾のファイル名を置き換える
        urlList[urlList.length - 1] = fileName;
        //srcのURLを更新
        $("#" + imgOpenClose).prop("src", urlList.join("/"));
    },
    // 展開
    Open: function (expanderContent, imgOpenClose, imgfileClose) {
        if ("none" == $("#" + expanderContent).css('display')) {
            // 閉じられている場合に開く処理で必要なパラメータのみを設定してクリック時処理
            headerClick(expanderContent, imgOpenClose, null, imgfileClose, null);
        }
    }
}