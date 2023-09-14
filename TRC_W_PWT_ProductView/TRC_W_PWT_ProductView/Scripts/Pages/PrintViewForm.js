///////////////////////////////////////////////////////////////////////////////////////////////
// �����ʗp����
///////////////////////////////////////////////////////////////////////////////////////////////
//���C����������̊e��ʔh���Ăяo��(PageLoad)
function MasterMainPageLoaded() {
}
//���C����������̊e��ʔh���Ăяo��(Resize)
function MasterMainPageResized() {
}
//�O���[�o���ϐ�
var zoom = parseInt(0);
var rate = parseInt(1);
var count = parseInt(0);
PrintViewForm = {
    //�萔
    DIV_BODY: "Body"
    ,
    DIV_VIEW: "View"
    ,
    DIV_LIST_AREA: "PrintCheckSheet_divListArea"
    ,
    DIV_BOX_AREA: "PrintCheckSheet_divViewBox"
    ,
    DIV_IMAGE_AREA: "PrintCheckSheet_imgMainArea"
    ,
    BTN_REDUCE: "btnReduct"
    ,
    BTN_EXPAND: "btnExpan"
    ,
    DIV_MARGIN: 4
    ,
    CONST_DEFAULT_WIDTH: 1259
    ,
    CONST_DEFAULT_HEIGHT: 1760
    ,
    CSS_PRINT_SHOW: "print-show"
    ,
    CSS_PRINT_VIEW: "print-div-view"
    ,
    loadPrintDisp: function (isPageLoad) {
        if (0 == isPageLoad) {
            //�y�[�W�ǂݍ��݊������A�e��ʓ�Script�Ăяo��(�Ăяo�������X�N���v�g�����݂��鎞�g�p)
            if (typeof MasterMainPageLoaded == "function") {
                MasterMainPageLoaded();
            }
        }
        //���T�C�Y���s���A�e��ʓ�Script�Ăяo��(�Ăяo�������X�N���v�g�����݂��鎞�g�p)
        if (typeof MasterMainPageResized == "function") {
            MasterMainPageResized();
        }

    }
    ,
    //�g��^�k��
    zoomSize: function (no) {
        if (no == 0) {
            count = 0;
            zoom = 0;
            PrintViewForm.imageSize();
            $("#" + PrintViewForm.BTN_REDUCE).prop("disabled", false);
            $("#" + PrintViewForm.BTN_EXPAND).prop("disabled", false);

        } else {
            count = count + parseInt(no);
            zoom = zoom + parseInt(no);
            if (zoom == 0) {
                zoom = 1;
            }
            if (no == -1) {
                zoom = rate * 0.8;
            } else if (no == 1) {
                zoom = rate * 1.2;
            }
            if (count == -5) {
                $("#" + this.BTN_REDUCE).prop("disabled", true);
            } else if (count == 7) {
                $("#" + this.BTN_EXPAND).prop("disabled", true);
            } else {
                $("#" + this.BTN_REDUCE).prop("disabled", false);
                $("#" + this.BTN_EXPAND).prop("disabled", false);
            }
            rate = zoom;
            PrintViewForm.resizeImage(zoom);
        }
    }
    ,
    //���C���摜�f�t�H���g�T�C�Y
    imageSize: function () {
        rate = (document.body.clientWidth - $("#" + this.DIV_LIST_AREA).width() - 37) / PrintViewForm.CONST_DEFAULT_WIDTH;
        PrintViewForm.resizeImage(rate);
    }
    ,
    //���T�C�Y
    resizeImage: function (rateSize) {

        //�摜
        var imgWidth = PrintViewForm.CONST_DEFAULT_WIDTH * rateSize;
        var imgHeight = PrintViewForm.CONST_DEFAULT_HEIGHT * rateSize;
        var viewImage = $("#" + PrintViewForm.DIV_IMAGE_AREA);

        viewImage.width(imgWidth);
        viewImage.height(imgHeight);


        //�\���G���A
        var consBottom = 0;

        var divBox = $("#" + PrintViewForm.DIV_BOX_AREA);
        var divList = $("#" + PrintViewForm.DIV_LIST_AREA);
        var divWSize = $(window).width() - divList.width() - 12;

        divBox.width(divWSize);
        divBox.height(divList.height());

    }
    ,
    //���
    PrintPreview: function () {

        //����O����
        $("#" + PrintViewForm.DIV_BODY).addClass(PrintViewForm.CSS_PRINT_SHOW);
        $("#" + PrintViewForm.DIV_LIST_AREA).addClass(PrintViewForm.CSS_PRINT_SHOW);
        $("#" + PrintViewForm.DIV_BOX_AREA).addClass(PrintViewForm.CSS_PRINT_SHOW);
        $("#" + PrintViewForm.DIV_VIEW).removeClass(PrintViewForm.CSS_PRINT_VIEW);

        if (window.ActiveXObject == null) {
            // ActiveXObject�ɃA�N�Z�X�ł��Ȃ����̂̓��_���u���E�U�Ƃ��āA
            // �u���E�U�W���̈��
            window.print();
        } else {
            // ActiveXObject�ɃA�N�Z�X�o����ꍇ�AIE11 
            // Windows�@�\�ɃA�N�Z�X���A����v���r���[
        var sWebBrowserCode = '<object width="0" height="0" classid="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2"></object>';
            // insertAdjacentHTML�̗L���`�F�b�N�������s���Ă������A
            // IE10�ȍ~�W�����ڂ̂��ߕs�v�Ƃ���B
        document.body.insertAdjacentHTML('beforeEnd', sWebBrowserCode);
        var objWebBrowser = document.body.lastChild;
        if (objWebBrowser == null) {
            return;
        }

        objWebBrowser.ExecWB(7, 1);
        document.body.removeChild(objWebBrowser);
        }

        //����㏈��
        $("#" + PrintViewForm.DIV_BODY).removeClass(PrintViewForm.CSS_PRINT_SHOW);
        $("#" + PrintViewForm.DIV_LIST_AREA).removeClass(PrintViewForm.CSS_PRINT_SHOW);
        $("#" + PrintViewForm.DIV_BOX_AREA).removeClass(PrintViewForm.CSS_PRINT_SHOW);
        $("#" + PrintViewForm.DIV_VIEW).addClass(PrintViewForm.CSS_PRINT_VIEW);

        }
}
