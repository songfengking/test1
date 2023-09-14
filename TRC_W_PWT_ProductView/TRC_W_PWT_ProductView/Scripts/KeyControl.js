///////////////////////////////////////////////////////////////////////////////////////////////
//���͐���֐�
///////////////////////////////////////////////////////////////////////////////////////////////
//�L�[���̓C�x���g����
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

    //[ALT+BkSp]�K��
    if (keyCode == 0x25 && altKey == true) {
        return false;
    }

    //[CTRL+N]�K��
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

    //�e�L�X�g�{�b�N�X�A�p�X���[�h�{�b�N�X�A�t�@�C���G���A��[BkSp]����
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

    //�{�^���A�e�L�X�g�G���A�A�����N�{�^����[Enter]����
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

    //INPUT FILE�̏ꍇ�ɂ̓L�[�R�[�h�ϊ����s��Ȃ�
    if ( tagName == "INPUT"
        && tagType == "FILE") {
        return true;
    }

    //����ȊO�̉ӏ��ł�[BkSp],[F5],[F1],[F3],[F11],[Enter]�L�[�K��
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

//F1�L�[���̓C�x���g����
document.onhelp = function() {
    event.returnValue = false;
    event.cancelBubble = true;
}

//�R���e�L�X�g���j���[�\���K��
document.oncontextmenu = function () {
    return true;
}