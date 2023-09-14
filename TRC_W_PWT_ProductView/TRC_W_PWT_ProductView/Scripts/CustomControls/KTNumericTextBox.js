var KTNumericTextBox =
{
    chkInputKey: function(ele, max, min) {
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
    setFormat: function(ele, max, min) {
        //読取属性/disable制御時は編集不可
        if (ele.readOnly || ele.disabled) {
            return;
        }    
        var val = new Number(ele.value);
        if (isNaN(val)) {
            ele.value = "";
        } else if (min > val || max < val) {
            ele.value = "";
        }
        return;
    },
    increase: function(ele, max) {
        //読取属性/disable制御時は編集不可
        if (ele.readOnly || ele.disabled) {
            return;
        }    
        var val = new Number(ele.value);
        if (isNaN(val)) {
            val = 0;
        }
        if (max >= val + 1) {
            ele.value = val + 1;
        }
        return;
    },
    decrease: function(ele, min) {
        //読取属性/disable制御時は編集不可
        if (ele.readOnly || ele.disabled) {
            return;
        }    
        var val = new Number(ele.value);
        if (isNaN(val)) {
            val = 0;
        }
        if (min <= val - 1) {
            ele.value = val - 1;
        }
        return;
    }
}
