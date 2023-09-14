///////////////////////////////////////////////////////////////////////////////////////////////
//�N���C�A���g�p�R���g���[������֐�
///////////////////////////////////////////////////////////////////////////////////////////////
ControlCommon = {
    //�萔
    HDN_FOCUS_ID: "hdnFocus"
    ,
    GRID_ROW: "grid-row"
    ,
    GRID_VIEW: "grid-layout"
    ,
    SELECT_ROW: "ui-state-highlight"
    ,
    LIST_VIEW_ROW: "listview-row"
    ,
    LIST_VIEW_SELECT_CTRL: "btnSelectSubmit"
    ,
    METHOD_TYPE_WINDOW: 1
    ,
    QUERY_STR_TOKEN: "?Token="
    ,
    QUERY_STR_INDEX: "&Index="
    ,
    QUERY_STR_GROUP: "&GroupCd="
    ,
    QUERY_STR_CLASS: "&ClassCd="
    //,
    //QUERY_STR_COOP: "?Coop="
    ,
    QUERY_STR_MODEL_CD: "&ModelCd="
    ,
    QUERY_STR_COUNTRY_CD: "&CountryCd="
    ,
    QUERY_STR_SERIAL: "&Serial="
    ,
    ATTR_GROUP_CD: "group_cd"
    ,
    ATTR_GRID_INDEX: "grid_index"
    ,
    ATTR_DATA_INDEX: "data_index"
    ,
    GRID_VIEW2: "grid-layout2"
    ,
    //�O���b�h�r���[�s�I��
    SelectGridRow: function (ele,index) {

        var gridview = $(ele).closest("." + ControlCommon.GRID_VIEW);
        var gridrows = $(gridview).find("." + ControlCommon.GRID_ROW);
        var selectRow = $(ele);

        if (0 < selectRow.length) {
            $(gridrows).removeClass(ControlCommon.SELECT_ROW);
            $(selectRow).addClass(ControlCommon.SELECT_ROW);
        }
    }
    ,
    //�O���b�h�r���[�s�_�u���N���b�N
    DoubleClickGridRow: function (index, methodType, param1, param2) {
        if (this.METHOD_TYPE_WINDOW == methodType) {
            this.WindowOpen(index, param1, param2);
        }
    },
    //�O���b�h�r���[�s�_�u���N���b�N
    DoubleClickGridRowWithCaller: function (index, methodType, param1, param2, param3) {
        if (this.METHOD_TYPE_WINDOW == methodType) {
            this.WindowOpenWithCaller(index, param1, param2, param3);
        }
    },
    //���X�g�r���[�s�I��
    SelectListViewRow: function (ele,index, selectBtnId) {

        var gridview = $(ele).closest("." + ControlCommon.GRID_VIEW);
        var preSelect = $(gridview).find("." + ControlCommon.SELECT_ROW);
        var gridrows = $(gridview).find("." + ControlCommon.LIST_VIEW_ROW);

        var selectRow = $(ele);

        if (0 < preSelect.length
            && 0 < selectRow.length
            && preSelect[0] == selectRow[0]) {
            return;
        }

        if (0 < selectRow.length) {
            $(gridrows).removeClass(ControlCommon.SELECT_ROW);
            $(selectRow).addClass(ControlCommon.SELECT_ROW);
            
            if (null != selectBtnId && undefined != selectBtnId && "" != selectBtnId) {
                __doPostBack(selectBtnId, '');
            }
        }
    }
    ,
    //�O���b�h�r���[�s�I��
    SelectGridRowSynchro: function (ele, index) {

        var selectRow = $(ele);
        var attrGrpCd = selectRow.attr(this.ATTR_GROUP_CD);
        if (false == Common.IsBlank(attrGrpCd)) {
            var gridIndex = selectRow.attr(this.ATTR_GRID_INDEX);

            var selectRows = $("[" + this.ATTR_GROUP_CD + "='" + attrGrpCd + "']" + "[" + this.ATTR_GRID_INDEX + "='" + gridIndex + "']");

            for (var idx = 0; idx < selectRows.length; idx++) {
                var gridview = $(selectRows[idx]).closest("." + ControlCommon.GRID_VIEW2);
                var gridrows = $(gridview).find("." + ControlCommon.GRID_ROW);
                $(gridrows).removeClass(ControlCommon.SELECT_ROW);
            }
            $(selectRows).addClass(ControlCommon.SELECT_ROW);
        } else {
            var gridview = $(ele).closest("." + ControlCommon.GRID_VIEW2);
            var gridrows = $(gridview).find("." + ControlCommon.GRID_ROW);
            $(gridrows).removeClass(ControlCommon.SELECT_ROW);
            $(selectRow).addClass(ControlCommon.SELECT_ROW);
        }
    }
    ,
    //�E�B���h�E�I�[�v��(�Z�b�V�����C���f�b�N�X�w��n��)
    WindowOpen: function (index, url, token) {
        var unixTs = new Date().getTime();
        var subWindow = window.open(url + this.QUERY_STR_TOKEN + token + this.QUERY_STR_INDEX + index + "&uts=" + unixTs, '_blank');
    }
    ,
    //�E�B���h�E�I�[�v��(�Z�b�V�����C���f�b�N�X�A�Ăяo�����y�[�W�w��n��)
    WindowOpenWithCaller: function (index, url, token, callerPageId) {
        var unixTs = new Date().getTime();
        var subWindow = window.open(url + this.QUERY_STR_TOKEN + token + this.QUERY_STR_INDEX + index + "&uts=" + unixTs + "&CallerPageID=" + callerPageId, '_blank');
    }
    ,
    //�E�B���h�E�I�[�v��(�^���A���A�@�� �w��)
    WindowOpenChangeDetail: function (url, token, modelCd, countryCd, serial) {
        var unixTs = new Date().getTime();
        var subWindow = window.open(url + this.QUERY_STR_TOKEN + token + this.QUERY_STR_MODEL_CD + modelCd + this.QUERY_STR_COUNTRY_CD + countryCd + this.QUERY_STR_SERIAL + serial + "&uts=" + unixTs, '_blank');
        return false;
    }
    ,
    //�E�B���h�E�N���[�Y�O����
    BeforeWindowClose: function () {

        //�I�����[�f�B���O
        SubmitControl.SetLoadingType(SubmitControl.TYPE_EXIT);

        var browserType = Common.GetBrowserType();
        //IE�n(EDGE�͓s�x�m�F�v)
        browserType = Common.BROWSER_IE8;
        if (Common.BROWSER_IE8 == browserType
            || Common.BROWSER_IE9 == browserType
            || Common.BROWSER_IE_EDGE == browserType) {
            //�T�[�o����Z�b�V�����폜��I��
            return true;
        //IE�ȊO(�I���m�F���b�Z�[�W�Ƃ��̓u���E�U�ˑ�)
        } else if (Common.BROWSER_CHROME == browserType
            || Common.BROWSER_NO_SUPPORT == browserType) {
            //���A�E�B���h�E�N���[�Y �Z�b�V�����̓^�C���A�E�g����j��
            this.WindowClose();
            return false;
        }
    }
    ,
    //�E�B���h�E�N���[�Y
    WindowClose: function () {

        var browserType = Common.GetBrowserType();
        //IE�n(EDGE�͓s�x�m�F�v)
        browserType = Common.BROWSER_IE8;
        if (Common.BROWSER_IE8 == browserType
            || Common.BROWSER_IE9 == browserType
            || Common.BROWSER_IE_EDGE == browserType) {
            window.open('about:blank', '_self').close();
        //IE�ȊO(�I���m�F���b�Z�[�W�Ƃ��̓u���E�U�ˑ�)
        } else if (Common.BROWSER_CHROME == browserType
            || Common.BROWSER_NO_SUPPORT == browserType) {
            window.opener = window;
            window.close();
        }
    }
    ,
    //�����t�H�[�J�X�Z�b�g
    SetInitializeFocus: function () {
        var hdnFocus = $("#" + this.HDN_FOCUS_ID);
        if (0 < hdnFocus.length) {
            var focusCtrlId = hdnFocus.val();
            if ("" != focusCtrlId) {
                ControlCommon.SetFocus(focusCtrlId);
            }
        }
    }
    ,
    //�ŏI�t�H�[�J�X�ێ�
    KeepFocus: function () {
        var hdnFocus = $("#" + this.HDN_FOCUS_ID);
        if (0 < hdnFocus.length) {
            var autoFocusElementId = document.activeElement.id;
            if (null != autoFocusElementId && undefined != autoFocusElementId && "" != autoFocusElementId) {
                hdnFocus.val(autoFocusElementId);
            }            
        }
    }
    ,
    //�t�H�[�J�X�Z�b�g
    SetFocus: function( ctrlID ) {
        try {
            setTimeout(function () {
                //�t�H�[�J�X�ݒ�
                $('#' + ctrlID).focus();
                if (document.activeElement.id != ctrlID) {
                    $('#' + ctrlID).focus().focus();
                }
            }, 0);
        } catch (ex) {
            //alert(ex.message);
        }
    }
}