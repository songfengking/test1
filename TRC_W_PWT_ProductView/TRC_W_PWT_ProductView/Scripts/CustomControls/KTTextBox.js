var KTTextBox =
{
    trim: function(val) {
        var trimval = "";
        if (null != val) {
            trimval = val;

            //先端部のトリム
            while (1) {
                if (trimval.length == 0)
                    break;

                //半角スペースまたは全角スペースが存在するか?
                if (String.fromCharCode(0x20) == trimval.substr(0, 1) || String.fromCharCode(0x3000) == trimval.substr(0, 1))
                    trimval = trimval.substr(1, trimval.length - 1);
                else
                    break;
            }

            //後端部のトリム
            while (1) {
                if (trimval.length == 0)
                    break;

                //半角スペースまたは全角スペースが存在するか?
                if (String.fromCharCode(0x20) == trimval.substr(trimval.length - 1, 1) || String.fromCharCode(0x3000) == trimval.substr(trimval.length - 1, 1))
                    trimval = trimval.substr(0, trimval.length - 1);
                else
                    break;
            }
        }

        return trimval;
    },
    getCharacterLen: function(inChar) {
        // unicode変換
        var c = inChar.charCodeAt(0);

        // unicodeが半角コードの範囲内かチェック
        // 0x0～0x80:半角英数字/記号		0xff61～0xff9f:句読点/半角カナ		0xf8f1～0xf8f3:予約語		0xf8f0:予約語
        if ( (c >= 0x0 && c < 0x81) || (c == 0xf8f0) || (c >= 0xff61 && c < 0xffa0) || (c >= 0xf8f1 && c < 0xf8f4)) {
            // 1byte文字　半角
            return 1;
        } else {
            // 2byte文字　全角
            return 2;
        }
    },
    chkRegExp: function(textval, regExp) {
        if (0 < textval.length) {
            //正規表現による入力文字のチェック
            var match_str = new String(textval);
            var match = match_str.match(regExp);
            if (match) {
                textval = match[0];     //入力可能な文字のみ設定(入力不可の文字の直前まで)
            } else {
                textval = "";
            }
        }
        return textval;
    },
    escapeCode: function(val) {
        return val;
    },
    setFormatAll: function(ele, maxLength) {
        //読取属性/disable制御時は編集不可
        if (ele.readOnly || ele.disabled) {
            return;
        }    
        var textval = ele.value;
        textval = this.trim(textval);
        textval = this.escapeCode(textval);

        //終端の改行コードは削除する
        var lastCode;
        while (textval.length > 0) {
            lastCode = textval.charAt(textval.length - 1);
            if (lastCode == String.fromCharCode(0x0D) || lastCode == String.fromCharCode(0x0A)) {
                textval = textval.substr(0, textval.length - 1);
            }
            else {
                break;
            }
        }

        //最大文字数を超過しているか？
        if (textval.length > parseInt(maxLength)) {
            textval = textval.substr(0, maxLength);        //文字列切り出し
        }
        ele.value = textval;

        return;
    },
    setFormatHalfKana: function (ele, maxLength) {
        //読取属性/disable制御時は編集不可
        if (ele.readOnly || ele.disabled) {
            return;
        }
        var textval = ele.value;
        textval = this.trim(textval);
        textval = this.escapeCode(textval);

        //終端の改行コードは削除する
        var lastCode;
        while (textval.length > 0) {
            lastCode = textval.charAt(textval.length - 1);
            if (lastCode == String.fromCharCode(0x0D) || lastCode == String.fromCharCode(0x0A)) {
                textval = textval.substr(0, textval.length - 1);
            }
            else {
                break;
            }
        }

        //最大文字数を超過しているか？
        if (textval.length > parseInt(maxLength)) {
            textval = textval.substr(0, maxLength);        //文字列切り出し
        }

        //半角カナチェック
        var tempval = "";

        //カナ変換テーブル
        var hankaku = new Array("ｶﾞ", "ｷﾞ", "ｸﾞ", "ｹﾞ", "ｺﾞ", "ｻﾞ", "ｼﾞ", "ｽﾞ", "ｾﾞ", "ｿﾞ", "ﾀﾞ", "ﾁﾞ", "ﾂﾞ", "ﾃﾞ", "ﾄﾞ", "ﾊﾞ", "ﾊﾟ", "ﾋﾞ", "ﾋﾟ", "ﾌﾞ", "ﾌﾟ", "ﾍﾞ", "ﾍﾟ", "ﾎﾞ", "ﾎﾟ", "ｳﾞ", "ｧ",  "ｱ",  "ｨ",  "ｲ",  "ｩ",  "ｳ",  "ｪ",  "ｴ",  "ｫ",  "ｵ",  "ｶ",  "ｷ",  "ｸ",  "ｹ",  "ｺ",  "ｻ",  "ｼ",  "ｽ",  "ｾ",  "ｿ",  "ﾀ",  "ﾁ",  "ｯ",  "ﾂ",  "ﾃ",  "ﾄ",  "ﾅ",  "ﾆ",  "ﾇ",  "ﾈ",  "ﾉ",  "ﾊ",  "ﾋ",  "ﾌ",  "ﾍ",  "ﾎ",  "ﾏ",  "ﾐ",  "ﾑ",  "ﾒ",  "ﾓ",  "ｬ",  "ﾔ",  "ｭ",  "ﾕ",  "ｮ",  "ﾖ",  "ﾗ",  "ﾘ",  "ﾙ",  "ﾚ",  "ﾛ",  "ﾜ",  "ｦ",  "ﾝ",  "｡",  "｢",  "｣",  "､",  "･",  "ｰ",  "ﾞ",  "ﾟ");
        var zenkaku = new Array("ガ", "ギ", "グ", "ゲ", "ゴ", "ザ", "ジ", "ズ", "ゼ", "ゾ", "ダ", "ヂ", "ヅ", "デ", "ド", "バ", "パ", "ビ", "ピ", "ブ", "プ", "ベ", "ペ", "ボ", "ポ", "ヴ", "ァ", "ア", "ィ", "イ", "ゥ", "ウ", "ェ", "エ", "ォ", "オ", "カ", "キ", "ク", "ケ", "コ", "サ", "シ", "ス", "セ", "ソ", "タ", "チ", "ッ", "ツ", "テ", "ト", "ナ", "ニ", "ヌ", "ネ", "ノ", "ハ", "ヒ", "フ", "ヘ", "ホ", "マ", "ミ", "ム", "メ", "モ", "ャ", "ヤ", "ュ", "ユ", "ョ", "ヨ", "ラ", "リ", "ル", "レ", "ロ", "ワ", "ヲ", "ン", "。", "「", "」", "、", "・", "ー", "゛", "゜");
        for (var i = 0; i < textval.length; i++) {
            var char = textval.charAt(i);
            var charIndex = $.inArray(char, zenkaku);
            if(0 <= charIndex) {
                char = hankaku[charIndex];
            }

            // 1byte文字以外は許容しない
            if (1 != this.getCharacterLen(char)) {
                break;
            } else {
                tempval += char;
            }
        }
        ele.value = tempval;

        return;
    },
    setFormatAscii: function (ele, maxLength, regExp) {
        //読取属性/disable制御時は編集不可
        if (ele.readOnly || ele.disabled) {
            return;
        }    
        var textval = ele.value;
        textval = this.trim(textval);
        textval = this.escapeCode(textval);
        textval = this.chkRegExp(textval, regExp);

        //最大文字数を超過しているか？
        if (textval.length > parseInt(maxLength)) {
            textval = textval.substr(0, maxLength);        //文字列切り出し
        }
        ele.value = textval;

        return;
    },
    setFormatInt: function(ele, max, min, regExp) {
        //読取属性/disable制御時は編集不可
        if (ele.readOnly || ele.disabled) {
            return;
        }    
        var textval = ele.value;
        textval = this.trim(textval);
        textval = this.chkRegExp(textval, regExp);
        ele.value = textval;

        //パラメータチェック
        if (0 == textval.length || isNaN(textval) || isNaN(max) || isNaN(min)) {
            return textval;             //パラメータ異常時には変換処理をしない
        }
        var maxVal = new Number(max);   //最大値取得
        var minVal = new Number(min);   //最小値取得
        var val = new Number(textval);  //値取得

        //最大/最小値超過チェック
        if (maxVal < val || minVal > val) {
            val = "";             //最大/最小値超過時はクリア
        }

        ele.value = val.toString(10);
        return;
    },
    setFormatFloat: function(ele, max, min, decLen, regExp) {
        //読取属性/disable制御時は編集不可
        if (ele.readOnly || ele.disabled) {
            return;
        }    
        var textval = ele.value;
        textval = this.trim(textval);
        textval = this.chkRegExp(textval, regExp);
        //パラメータチェック
        if (0 == textval.length || isNaN(textval)) {
            ele.value = "";
            return;             //パラメータ異常時にはクリア
        }
        if (isNaN(max) || isNaN(min) || isNaN(decLen) ) {

            return;
        }
        var maxVal = new Number(max);   //最大値取得
        var minVal = new Number(min);   //最小値取得
        var val = new Number(textval);  //値取得
        var isExponential = false;

        //最大/最小値超過チェック
        if (maxVal < val || minVal > val) {
            ele.value = "";
            return;             //最大/最小値超過時はクリア
        }

        if (0 >= decLen) {
            return;
        }

        var tmpVal = val.toString(10);
        var periodIdx = tmpVal.indexOf(".");
        var decVal = "";

        if (0 <= periodIdx) {
            decVal = tmpVal.substring(periodIdx + 1, tmpVal.length);
        }
        if (decLen < decVal.length) {
            tmpVal = tmpVal.substring(0, tmpVal.length - (decVal.length - decLen));
            decVal = tmpVal.substring(periodIdx + 1, tmpVal.length);
        }

        if (0 < decLen) {

            if (0 > periodIdx) {
                tmpVal += ".";
            }

            for (var loop = decVal.length; loop < decLen; loop++) {
                tmpVal += "0";
            }
        }

        ele.value = tmpVal;

        return;
    },
    setCursor: function (ele) {
        //読取属性/disable制御時は編集不可
        if (ele.readOnly || ele.disabled) {
            return;
        }

        var val = $(ele).val();
        if (0 == val.length) {
            ele.focus();
            ele.select();
        }
    },
    chgUpper: function (ele) {
        var autoUpper = $(ele).attr("AutoUpper");
        if (autoUpper != "TRUE") {
            return;
        }

        var textval = ele.value;
        var upperval = textval.toUpperCase();
        if (textval != upperval) {
            ele.value = upperval;
        }
    }
}
