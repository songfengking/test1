KTTimeTextBox =
{

    defaultText: "__:__:__",
    separater: ":",

    isNumber: function (numVal){
        // チェック条件パターン
        var pattern = /^\d*$/;
        // 数値チェック
        return pattern.test(numVal);
    },
    chkRegExp: function (textval) {
        if (0 < textval.length) {
            if (this.isNumber(textval) == false) {
                textval = "";
            } else {
                if (6 == textval.length) {
                    //時分秒に分割
                    var hour = textval.substr(0, 2);
                    var minute = textval.substr(2, 2);
                    var second = textval.substr(4, 2);

                    var hournum = Number(hour);
                    var minutenum = Number(minute);
                    var secondnum = Number(second);

                    //時分秒チェック
                    if (hournum < 0 || hournum > 23) {
                        textval = "";
                    }
                    if (minutenum < 0 || minutenum > 59) {
                        textval = "";
                    }
                    if (secondnum < 0 || secondnum > 59) {
                        textval = "";
                    }
                } else {
                    textval = "";
                }
            }

        }
        return textval;
    },
    chkInputKey: function (ele, max, min) {
        //読取属性/disable制御時は編集不可
        if (ele.readOnly || ele.disabled) {
            return;
        }    
        if (event.ctrlKey) {
            if (0x56 == event.keyCode) {
                event.returnValue = false;
            }
            return;
        }
        var key = event.keyCode;
        var val = new Number(ele.value);
        if (isNaN(val)) {
            val = 0;
        }
        if (0x26 == key) {
            //上キー押下

            if (max >= val + 1) {
                ele.value = val + 1;
            }
            event.returnValue = false;
        } else if (0x28 == key) {
            //下キー押下

            if (min <= val - 1) {
                ele.value = val - 1;
            }
            event.returnValue = false;
        } else if (0x30 <= key && 0x39 >= key) {
            //数字キー押下

            var selected_range = ele.createTextRange();
            var text_base = selected_range.text;
            var len = text_base.length;
            if (0 < len) {
                var range_start = document.selection.createRange();
                range_start.setEndPoint("EndToStart", selected_range);
                var start_point = range_start.text.length;

                var range_end = document.selection.createRange();
                range_end.setEndPoint("StartToEnd", selected_range);
                var end_point = range_end.text.length;

                var text1 = ele.value.substr(0, start_point);
                var text2 = ele.value.substr(len - end_point, end_point);
                val = new Number(text1 + (key - 0x30) + text2);

                if (min <= val && max >= val) {
                    ele.value = val;
                    var range = ele.createTextRange();
                    range.move('character', ele.value.length - end_point);
                    range.select();
                }
                event.returnValue = false;
            }
        } else if (0x6D <= key && 0xBD >= key) {
            //マイナス 処理しない
        } else if (0x6E <= key && 0xBE >= key) {
            //小数点 処理しない
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
        var textval = ele.value;

        if (selected) {
            textval = textval.replace(/:/g, "");
            $(ele).val(textval);

            ele.select();
            return;
        }

        //読取属性/disable制御時は編集不可
        if (ele.readOnly || ele.disabled) {
            return;
        }

        //入力可能文字チェック
        textval = textval.replace(/:/g, "");
        textval = this.chkRegExp(textval);

        if (textval == "") {
            textval = this.defaultText;
        } else {
            //入力時刻分割
            var hour = textval.substr(0, 2);
            var minute = textval.substr(2, 2);
            var second = textval.substr(4, 2);

            textval = "";
            textval += (hour.length == 1 ? "0" : "");
            textval += hour;
            textval += this.separater;
            textval += (minute.length == 1 ? "0" : "");
            textval += minute;
            textval += this.separater;
            textval += (second.length == 1 ? "0" : "");
            textval += second;
        }

        //値設定
        $(ele).val(textval);

        return;
    }
}
