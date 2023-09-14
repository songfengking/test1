
KTDatePicker = {
    defaultText: "____/__/__",
    format: "[0-9]{4}/(0[1-9]|1[0-2])/(0[1-9]|[12][0-9]|3[01])",
    separater: "/",
    maxYear: 9999,
    maxMonth: 12,
    maxDay: 31,
    minYear: 1951,
    minMonth: 1,
    minDay: 1,
    keyInput: false,

    chkRegExp: function(textval) {
        if (0 < textval.length) {
            //正規表現による入力文字のチェック
            var match_str = new String(textval);
            var match = match_str.match(this.format);
            if (match) {
                textval = match[0];     //入力可能な文字のみ設定(入力不可の文字の直前まで)
            } else {
                textval = "";
            }
        }
        return textval;
    },
    convDate: function(textval) {
        var date = new Date(textval);

        var year = "";
        var month = "";
        var day = "";
        var restore = false;

        if (isNaN(date)) {
            var list = textval.split(this.separater);
            year = list[0];
            month = list[1];
            day = list[2];
        } else {
            year = date.getFullYear().toString(10);
            month = (date.getMonth() + 1).toString(10);
            day = date.getDate().toString(10);
            restore = true;
        }
        if (day.length > 2 || new Number(day) > this.maxDay) {
            day = this.maxDay.toString();
            restore = true;
        }
        if (new Number(day) < this.minDay) {
            day = this.minDay.toString();
            restore = true;
        }
        if (month.length > 2 || new Number(month) > this.maxMonth) {
            month = this.maxMonth.toString();
            day = this.maxDay.toString();
            restore = true;
        }
        if (new Number(month) < this.minMonth) {
            month = this.minMonth.toString();
            day = this.minDay.toString();
            restore = true;
        }
        if (year.length > 4 || new Number(year) > this.maxYear) {
            year = this.maxYear.toString();
            month = this.maxMonth.toString();
            day = this.maxDay.toString();
            restore = true;
        }
        if (new Number(year) < this.minYear) {
            year = this.minYear.toString();
            month = this.minMonth.toString();
            day = this.minDay.toString();
            restore = true;
        }

        if (restore == true) {
            textval = "";
            textval += (year.length == 1 ? "0" : "");
            textval += (year.length == 2 ? "0" : "");
            textval += (year.length == 3 ? "0" : "");
            textval += year;
            textval += this.separater;
            textval += (month.length == 1 ? "0" : "");
            textval += month;
            textval += this.separater;
            textval += (day.length == 1 ? "0" : "");
            textval += day;
        }

        return textval;
    },
    chkInputKey: function (ele) {

        if (false == KTDatePicker.keyInput) {
            return;
        }

        //読取属性/disable制御時は編集不可
        if (ele.readOnly || ele.disabled) {
            return;
        }
        //CTRLキー押下時は入力許可
        if (event.ctrlKey) {
            return;
        }
        var defaultLen = this.defaultText.length;
        var key = event.keyCode;            //入力キーコード
        var type = -1;                      //編集タイプ 0:年 1:月 2:日 3:年スラッシュ 4:月スラッシュ 5:最後尾 6:全選択
        var textval = "";                   //出力テキスト

        //選択範囲取得

        var selected_range = ele.createTextRange();
        var text_base = selected_range.text;
        var len = text_base.length;
        var range_start = document.selection.createRange();
        range_start.setEndPoint("EndToStart", selected_range);
        var start_point = range_start.text.length;
        var range_end = document.selection.createRange();
        range_end.setEndPoint("StartToEnd", selected_range);
        var end_point = range_end.text.length;
        var text1 = ele.value.substr(0, start_point);               //カーソル位置左側
        var text2 = ele.value.substr(len - end_point, end_point);   //カーソル位置右側
        var selectLen = defaultLen - (text1.length + text2.length); //選択サイズ取得

        var movelen = 0;                                            //編集後カーソル移動位置

        //カーソル位置は終端より前か？

        if (text2.length > 0) {
            //編集タイプ判定

            if (text1.length < 4) {
                type = 0;       //年
            } else if (text1.length >= 5 && text1.length < 7) {
                type = 1;       //月
            } else if (text1.length >= 8 && text1.length < 10) {
                type = 2;       //日
            } else if (text1.length == 4) {
                type = 3;       //年スラッシュ
            } else if (text1.length == 7) {
                type = 4;       //月スラッシュ                
            }
        } else if (text1.length + selectLen == defaultLen) {
            type = 5;
        } else if (selectLen == defaultLen) {
            type = 6;
        }

        if (0x26 == key) {
            //上キー押下

            //編集タイプ別入力値設定

            var num = 0;
            var numText = "";
            switch (type) {
                case 0:         //年
                case 3:
                    textval = "";                                   //前部文字連結

                    num = new Number(text_base.substr(0, 4)) + 1;   //年を数値へ変換
                    if (isNaN(num)) {
                        num = this.minYear;
                    }
                    numText = num.toString();                       //文字列変換
                    for (var i = numText.length; i < 4; i++) {      //ゼロ付加
                        textval += "0";
                    }
                    textval += numText;                             //年設定
                    textval += text_base.substr(4, defaultLen - 4); //後部文字連結

                    movelen = 4;
                    break;
                case 1:
                case 4:
                    textval = text_base.substr(0, 5);               //前部文字連結
                    num = new Number(text_base.substr(5, 2)) + 1;   //月を数値へ変換
                    if (isNaN(num)) {
                        num = this.minMonth;
                    }
                    numText = num.toString();                       //文字列変換
                    for (var i = numText.length; i < 2; i++) {      //ゼロ付加
                        textval += "0";
                    }
                    textval += numText;                             //月設定
                    textval += text_base.substr(7, defaultLen - 7); //後部文字連結


                    movelen = 7;
                    break;
                case 2:
                case 5:
                    textval = text_base.substr(0, 8);               //前部文字連結
                    num = new Number(text_base.substr(8, 2)) + 1;   //日を数値へ変換
                    if (isNaN(num)) {
                        num = this.minDay;
                    }
                    numText = num.toString();                       //文字列変換
                    for (var i = numText.length; i < 2; i++) {      //ゼロ付加
                        textval += "0";
                    }
                    textval += numText;                             //日設定
                    textval += "";                                  //後部文字連結

                    movelen = 10;
                    break;
                default:
                    //無視
                    break;
            }

            if (textval.length > 0) {
                //編集値セット
                var sel = ele.createTextRange();
                sel.text = this.convDate(textval);

                //カーソル位置移動
                sel.move('character', -1 * (defaultLen - movelen));
                sel.select();
            }

            event.returnValue = false;
        } else if (0x28 == key) {
            //下キー押下

            //編集タイプ別入力値設定

            var num = 0;
            var numText = "";
            switch (type) {
                case 0:         //年
                case 3:
                    textval = "";                                   //前部文字連結
                    num = new Number(text_base.substr(0, 4)) - 1;   //年を数値へ変換
                    if (isNaN(num)) {
                        num = this.minYear;
                    }
                    numText = num.toString();                       //文字列変換
                    for (var i = numText.length; i < 4; i++) {      //ゼロ付加
                        textval += "0";
                    }
                    textval += numText;                             //年設定
                    textval += text_base.substr(4, defaultLen - 4); //後部文字連結

                    movelen = 4;
                    break;
                case 1:         //月

                case 4:
                    textval = text_base.substr(0, 5);               //前部文字連結
                    num = new Number(text_base.substr(5, 2)) - 1;   //月を数値へ変換
                    if (isNaN(num)) {
                        num = this.minMonth;
                    }
                    numText = num.toString();                       //文字列変換
                    for (var i = numText.length; i < 2; i++) {      //ゼロ付加
                        textval += "0";
                    }
                    textval += numText;                             //月設定
                    textval += text_base.substr(7, defaultLen - 7); //後部文字連結

                    movelen = 7;
                    break;
                case 2:         //日
                case 5:
                    textval = text_base.substr(0, 8);               //前部文字連結
                    num = new Number(text_base.substr(8, 2)) - 1;   //日を数値へ変換
                    if (isNaN(num)) {
                        num = this.minDay;
                    }
                    numText = num.toString();                       //文字列変換
                    for (var i = numText.length; i < 2; i++) {      //ゼロ付加
                        textval += "0";
                    }
                    textval += numText;                             //日設定
                    textval += "";                                  //後部文字連結

                    movelen = 10;
                    break;
                default:
                    //無視

                    break;
            }

            if (textval.length > 0) {
                //編集値セット
                var sel = ele.createTextRange();
                sel.text = this.convDate(textval);

                //カーソル位置移動
                sel.move('character', -1 * (defaultLen - movelen));
                sel.select();
            }
            event.returnValue = false;
        } else if ((0x30 <= key && 0x39 >= key) || (0x60 <= key && 0x69 >= key)) {
            //数字キー押下

            //キーに対応する数値
            var keyvalue;
            if (0x60 > key) {
                keyvalue = key - 0x30;      //標準数値キー入力
            } else {
                keyvalue = key - 0x60;      //テンキー入力
            }

            //選択範囲チェック
            if (text1.length + text2.length == 0) {
                //全選択
                textval = text1 + keyvalue.toString() + text_base.substr(1, text_base.length - 1);
                movelen = 1;                                        //カーソルを先頭から左1シフト
            } else {
                //選択なし(カーソル位置から1文字設定)
                textval = text1;
                //設定位置の文字チェック
                if (text_base.substr(text1.length, 1) != this.separater) {
                    textval += keyvalue.toString();                             //スラッシュ以外
                } else {
                    textval += text_base.substr(text1.length, 1);               //スラッシュ(置換なし)
                }
                //選択位置以降の文字連結
                if (selectLen == 0) {
                    if (text2.length > 0) {
                        textval += text2.substr(1, text2.length - 1);
                    }
                } else {
                    //範囲選択時の2文字目以降はディフォルト値を設定
                    for (var i = 1; i < selectLen; i++) {
                        if (text_base.substr(text1.length + i, 1) != this.separater) {
                            textval += "_";                                     //スラッシュ以外("_"を設定)
                        } else {
                            textval += text_base.substr(text1.length + i, 1);   //スラッシュ(置換なし)
                        }
                    }
                    if (text2.length > 0) {
                        textval += text2;
                    }
                }
                //カーソル位置設定
                movelen = text1.length + 1;                         //カーソルを左1シフト
                if (text_base.substr(text1.length + 1, 1) == this.separater) {
                    movelen++;                                      //次の文字がスラッシュの場合にはさらに左1シフト
                }
            }

            //編集値セット
            var sel = ele.createTextRange();
            sel.text = textval;

            //カーソル位置移動
            sel.move('character', -1 * (defaultLen - movelen));
            sel.select();
            event.returnValue = false;
        } else if (0xBF == key || 0x6F == key) {
            //スラッシュ(カーソル位置移動)
            if (type == 3) {
                //カーソル位置移動(年スラッシュの次の位置に移動)
                var sel = ele.createTextRange();
                //カーソル位置移動
                sel.move('character', 5);
                sel.select();
            } else if (type == 4) {
                //カーソル位置移動(月スラッシュの次の位置に移動)
                var sel = ele.createTextRange();
                //カーソル位置移動
                sel.move('character', 8);
                sel.select();
            }

            event.returnValue = false;
        } else if (0x08 == key) {
            //backspace(選択範囲をすべて"_"に置換)

            //選択範囲チェック
            if (text1.length + text2.length == 0) {
                //全選択
                textval = this.defaultText;             //初期値を設定
                movelen = 0;                            //カーソルは先頭
            } else if (selectLen == 0) {
                //選択なし(カーソル位置から1文字削除)
                textval = text1.substr(0, text1.length - 1);
                if (text1.length > 0) {
                    if (text_base.substr(text1.length - 1, 1) != this.separater) {
                        textval += "_";
                    } else {
                        textval += text_base.substr(text1.length - 1, 1);   //スラッシュ部は削除しない
                    }
                }
                textval += text2;
                movelen = text1.length - 1;
            } else {
                //選択範囲のテキスト置換
                textval = text1;
                for (var i = 0; i < selectLen; i++) {
                    if (text_base.substr(text1.length + i, 1) != this.separater) {
                        textval += "_";
                    } else {
                        textval += text_base.substr(text1.length + i, 1);   //スラッシュ部は削除しない
                    }
                }
                textval += text2;
                movelen = text1.length;
            }

            //編集値セット
            var sel = ele.createTextRange();
            sel.text = textval;

            //カーソル位置移動
            sel.move('character', -1 * (defaultLen - movelen));
            sel.select();

            event.returnValue = false;
        } else if (0x2E == key) {
            //Delete(選択範囲をすべて"_"に置換)

            //選択範囲チェック
            if (text1.length + text2.length == 0) {
                //全選択
                textval = this.defaultText;             //初期値を設定
                movelen = 0;                            //カーソルは先頭
            } else if (selectLen == 0) {
                //選択なし(カーソル位置から1文字削除)
                textval = text1;
                if (text2.length > 0) {
                    if (text_base.substr(text1.length, 1) != this.separater) {
                        textval += "_";
                    } else {
                        textval += text_base.substr(text1.length, 1);       //スラッシュ部は削除しない

                    }
                }
                textval += text2.substr(1, text2.length - 1);
                movelen = text1.length;
            } else {
                //選択範囲のテキスト置換
                textval = text1;
                for (var i = 0; i < selectLen; i++) {
                    if (text_base.substr(text1.length + i, 1) != this.separater) {
                        textval += "_";
                    } else {
                        textval += text_base.substr(text1.length + i, 1);   //スラッシュ部は削除しない

                    }
                }
                textval += text2;
                movelen = text1.length;
            }

            //編集値セット
            var sel = ele.createTextRange();
            sel.text = textval;

            //カーソル位置移動
            sel.move('character', -1 * (defaultLen - movelen));
            sel.select();

            event.returnValue = false;
        } else if (0x41 <= key && 0x5A >= key ||
                   0x6A <= key && 0x6F >= key ||
                   0xBA <= key) {
            //文字と記号は入力不可
            event.returnValue = false;
        } else {
            //それ以外は無視
        }
        return true;
    },
    setFormat: function (ele, selected) {

        if (false == KTDatePicker.keyInput) {
            if (selected) {
                return;
            }
        }

        //読取属性/disable制御時は編集不可
        if (ele.readOnly || ele.disabled) {
            return;
        }
        var textval = ele.value;
        
        //入力可能文字チェック
        textval = this.chkRegExp(textval);

        //日付オブジェクト作成
        var date;
        date = new Date(textval);
        if (isNaN(date)) {
            textval = this.defaultText;
        } else {
            var year = date.getFullYear().toString(10);
            var month = (date.getMonth() + 1).toString(10);
            var day = date.getDate().toString(10);

            if (day.length > 2 || new Number(day) > this.maxDay) {
                day = this.maxDay.toString();
            }
            if (new Number(day) < this.minDay) {
                day = this.minDay.toString();
            }
            if (month.length > 2 || new Number(month) > this.maxMonth) {
                month = this.maxMonth.toString();
                day = this.maxDay.toString();
            }
            if (new Number(month) < this.minMonth) {
                month = this.minMonth.toString();
                day = this.minDay.toString();
            }
            if (year.length > 4 || new Number(year) > this.maxYear) {
                year = this.maxYear.toString();
                month = this.maxMonth.toString();
                day = this.maxDay.toString();
            }
            if (new Number(year) < this.minYear) {
                year = this.minYear.toString();
                month = this.minMonth.toString();
                day = this.minDay.toString();
            }

            textval = "";
            textval += (year.length == 1 ? "0" : "");
            textval += (year.length == 2 ? "0" : "");
            textval += (year.length == 3 ? "0" : "");
            textval += year;
            textval += this.separater;
            textval += (month.length == 1 ? "0" : "");
            textval += month;
            textval += this.separater;
            textval += (day.length == 1 ? "0" : "");
            textval += day;
        }

        //編集値セット
        //var sel = ele.createTextRange();
        //sel.text = textval;
        $(ele).val(textval);

        //テキスト全選択
        //if (selected) {
        //    ele.select();
        //}
        return;
    }
}