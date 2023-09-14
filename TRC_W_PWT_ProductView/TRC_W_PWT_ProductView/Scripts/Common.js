///////////////////////////////////////////////////////////////////////////////////////////////
//�N���C�A���g�p���ʊ֐�
///////////////////////////////////////////////////////////////////////////////////////////////
Common = {
    BROWSER_IE8 : "IE8"
    ,
    BROWSER_IE9 : "IE9"
    ,
    //IE9�ȏ�AIE10�ȏ� ���m��
    BROWSER_IE_EDGE : "IE_EDGE"
    ,
    BROWSER_CHROME: "CHROME"
    ,
    BROWSER_NO_SUPPORT: undefined
    ,
    //0���ߏ���
    ToDoubleDigits: function (num) {
        num += "";
        if (1 === num.length) {
            num = "0" + num;
        }
        return num;
    }
    ,
    //���ݎ����擾�����iyyyy/mm/dd hh:mm�`���j
    GetPresentTime: function () {
        //���ݎ����擾
        var datetime = new Date();

        var year = datetime.getFullYear();                           //�N�擾
        var month = this.ToDoubleDigits(datetime.getMonth() + 1);    //���擾�i0���߁j
        var day = this.ToDoubleDigits(datetime.getDate());           //���擾�i0���߁j
        var hours = this.ToDoubleDigits(datetime.getHours());        //���擾�i0���߁j
        var minutes = this.ToDoubleDigits(datetime.getMinutes());    //���擾�i0���߁j
        var seconds = this.ToDoubleDigits(datetime.getSeconds());    //�b�擾�i0���߁j

        //�uyyyy/mm/dd hh:MM:ss�v�`���ɂ���
        return year + "/" + month + "/" + day + " " + hours + ":" + minutes + ":" + seconds;
    }
    ,
    GetBrowserType: function () {

        //�u���E�U���
        var browserType = this.BROWSER_NO_SUPPORT;

        var _ua = (function () {
            return {
                ltIE8: typeof window.addEventListener == "undefined" && typeof document.getElementsByClassName == "undefined",
                ltIE9: document.uniqueID && !window.matchMedia,
                gtIE10: document.uniqueID && document.documentMode >= 10,
                Trident: document.uniqueID,
                Gecko: window.sidebar,
                Presto: window.opera,
                Blink: window.chrome,
                Webkit: !window.chrome && typeof document.webkitIsFullScreen != undefined,
                Touch: typeof document.ontouchstart != "undefined",
                Mobile: typeof window.orientation != "undefined"
            }
        })();

        if (_ua.ltIE8) {
            browserType = this.BROWSER_IE8;
        } else if (_ua.ltIE9) {
            browserType = this.BROWSER_IE9;
        } else if (_ua.gtIE10) {
            browserType = this.BROWSER_IE_EDGE;
        } else if (_ua.Blink) {
            //OPERA�̉\��������(OPERA�̓T�|�[�g�O)
            browserType = this.BROWSER_CHROME
        } else if (_ua.Gecko) {
            //Firefox�̓T�|�[�g�O
        } else if (_ua.Trident && !_ua.ltIE8) {
            browserType = this.BROWSER_IE_EDGE;
        } else if ((_ua.Trident && !_ua.ltIE9) || _ua.gtIE10) {
            browserType = this.BROWSER_IE_EDGE;
        }

        return browserType;
    }
    ,
    IsBlank: function (val) {
        if (null != val && undefined != val && "" != val) {
            return false;
        }

        return true;
    }
    ,
    GetWheelEvent: function () {
        var wheelEvent = "";
        var browserType = Common.GetBrowserType();

        if (Common.BROWSER_IE8 == browserType) {
            wheelEvent = "mousewheel";
        } else if (Common.BROWSER_CHROME == browserType
            || Common.BROWSER_IE9 == browserType
            || Common.BROWSER_IE_EDGE == browserType) {
            wheelEvent = "wheel";
        } else {
            wheelEvent = "DOMMouseScroll";
        }

        return wheelEvent;
    },
    GetWheelDelta: function (e) {
        e.preventDefault();

        var wheelDelta = 0;
        var browserType = Common.GetBrowserType();
        var delta = e.originalEvent.deltaY ? -(e.originalEvent.deltaY) : e.originalEvent.wheelDelta ? e.originalEvent.wheelDelta : -(e.originalEvent.detail);

        if (false == Common.IsBlank(e.originalEvent.deltaY)) {
            wheelDelta = -1 * e.originalEvent.deltaY;
        } else if (false == Common.IsBlank(e.originalEvent.wheelDelta)) {
            wheelDelta = e.originalEvent.wheelDelta;
        } else if (false == Common.IsBlank(e.originalEvent.detail)) {
            wheelDelta = -1 * e.originalEvent.detail;
        }

        return wheelDelta;
    }
}