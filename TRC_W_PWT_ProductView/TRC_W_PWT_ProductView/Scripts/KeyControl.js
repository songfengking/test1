///////////////////////////////////////////////////////////////////////////////////////////////
//入力制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
//キー入力イベント制御
document.onkeydown = function (evt) {

    var ctrlKey = false;
    var shiftKey = false;
    var altKey = false;

    var keyCode = "";

    if (evt) {
        keyCode = evt.keyCode;
        ctrlKey = evt.ctrlKey;
        shiftKey = evt.shiftKey;
        altKey = evt.altKey;
    } else {
        keyCode = event.keyCode;
        ctrlKey = event.ctrlKey;
        shiftKey = event.shiftKey;
        altKey = event.altKey;
    }

    //[ALT+BkSp]規制
    if (keyCode == 0x25 && altKey == true) {
        return false;
    }

    //[CTRL+N]規制
    if (window.event.ctrlKey == true) {
        if (event.keyCode == 78) {
            return false;
        }
    }

    var srcElem = window.event.srcElement;
    var elem = $(srcElem);
    var tagName = elem.prop("tagName").toUpperCase();

    var typeObj = elem.prop("type");
    var tagType = "";
    if (null != typeObj 
        && undefined != typeObj) {
        tagType = elem.prop("type").toUpperCase();
    }

    //テキストボックス、パスワードボックス、ファイルエリアは[BkSp]許可
    if (keyCode == 8) {
        if ( tagName == "INPUT"
            && ( tagType == "TEXT" || tagType == "PASSWORD")
            && false == elem[0].readOnly ) {
            return true;
        } else if ( tagName == "TEXTAREA" 
            && false == elem[0].readOnly ) {
            return true;
        }
    }

    //ボタン、テキストエリア、リンクボタンは[Enter]許可
    if (keyCode == 13) {
        if ( tagName == "INPUT"
            && tagType == "SUBMIT" ) {
            return true;
        } else if ( tagName == "TEXTAREA"
            && false == elem[0].readOnly) {
            return true;
        } else if ( tagName == "A" ) {
            return true;
        }
    }

    //INPUT FILEの場合にはキーコード変換を行わない
    if ( tagName == "INPUT"
        && tagType == "FILE") {
        return true;
    }

    //それ以外の箇所での[BkSp],[F5],[F1],[F3],[F11],[Enter]キー規制
    if (keyCode == 8
        || keyCode == 116
        || keyCode == 112
        || keyCode == 114
        || keyCode == 122
        || keyCode == 13) {

        if (evt) {
            evt.keyCode = 0;
        } else {
            event.keyCode = 0;
        }
        return false;
    }
}

//F1キー入力イベント制御
document.onhelp = function() {
    event.returnValue = false;
    event.cancelBubble = true;
}

//コンテキストメニュー表示規制
document.oncontextmenu = function () {
    return true;
}