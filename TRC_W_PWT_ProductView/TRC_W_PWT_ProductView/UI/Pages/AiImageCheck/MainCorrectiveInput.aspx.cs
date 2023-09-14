using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Reflection;
using KTFramework.Common;
using KTFramework.Dao;
using KTWebInheritance.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.UI.Base;
using TRC_W_PWT_ProductView.Session;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.ListDefine;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.Dao.Com;
using TRC_W_PWT_ProductView.Dao.Process;
using TRC_W_PWT_ProductView.Business;
using System.Diagnostics;

namespace TRC_W_PWT_ProductView.UI.Pages.AiImageCheck {
    public partial class MainCorrectiveInput : BaseForm {

        #region 定数定義

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );
        const string CLOSE_MODAL_DISP = "MainCorrectiveInput.CloseModal();";
        const string CHECK_INPUT_DISP = "return MainCorrectiveInput.CheckInput();";
        const string CHECK_INPUT_DISP_BLUR = "return MainCorrectiveInput.CheckInputBlur();";
        const string KEY_PRESS_EVENT = "onkeypress";
        const string BLUR_EVENT = "onblur";

        /// <summary>
        /// 空き(表示用)
        /// </summary>
        const string FLAG_DISP = "";
        /// <summary>
        /// OK(表示用)
        /// </summary>
        const string FLAG_OK_DISP = "OK";
        /// <summary>
        /// NG(表示用)
        /// </summary>
        const string FLAG_NG_DISP = "NG";
        /// <summary>
        /// 不良タイプフラグ
        /// </summary>
        const string NG_FLAG = "1";
        /// <summary>
        /// 是正フラグ
        /// </summary>
        const string CORRECTIVE_FLAG = "2";

        #endregion

        #region 変数定義

        private static Dictionary<string, string> _orgData = new Dictionary<string, string>();
        private static Dictionary<string, string> _resultData = new Dictionary<string, string>();

        #endregion

        #region プロパティ

        /// <summary>
        /// 表示中ページ(ベースフォーム)情報
        /// </summary>
        private BaseForm CurrentForm {
            get {
                return ( (BaseForm)Page );
            }
        }

        /// <summary>
        /// ユーザ情報
        /// </summary>
        private UserInfoSessionHandler.ST_USER _loginInfo;

        /// <summary>
        /// ユーザ情報
        /// </summary>
        public UserInfoSessionHandler.ST_USER LoginInfo {

            get {
                if ( true == ObjectUtils.IsNull( _loginInfo.UserInfo ) ) {
                    SessionManagerInstance sesMgr = CurrentForm.SessionManager;
                    _loginInfo = sesMgr.GetUserInfoHandler().GetUserInfo();
                }

                return _loginInfo;
            }
        }

        #endregion

        #region ページイベント

        protected void Page_Load( object sender, EventArgs e ) {
            base.RaiseEvent( DoPageLoad, false );
        }

        /// <summary>
        /// ページロード処理
        /// </summary>
        protected override void DoPageLoad( ) {
            //keyの作成
            ListItem[] liArr = new ListItem[5];
            liArr[0] = new ListItem( "ddlLineCd", MainCorrectiveList.ddlLineCd );  //ラインコード
            liArr[1] = new ListItem( "ddlStation", MainCorrectiveList.ddlStation ); //ステーション
            liArr[2] = new ListItem( "txtIdno", MainCorrectiveList.txtIdno );  //IDNO
            liArr[3] = new ListItem( "txtMonthlySeqNum", MainCorrectiveList.txtMonthlySeqNum );  //月度連番
            liArr[4] = new ListItem( "txtRecordNo", MainCorrectiveList.txtRecordNo );   //来歴

            //戻る
            ktbtnClose.Attributes[ControlUtils.ON_CLICK] = AiImageCheck.ImageInputModal.ImageCreateDispUrl( this, PageInfo.MainCorrectiveList, 10, 10, liArr, "是正処置入力検査結果一覧画面", "backlist" );

            //是正コメント
            txtCorrectiveComment.Attributes[KEY_PRESS_EVENT] = CHECK_INPUT_DISP;
            txtCorrectiveComment.Attributes[BLUR_EVENT] = CHECK_INPUT_DISP_BLUR;

            //初期設定
            if ( IsPostBack == false ) {
                InitializeSet();
            }
        }

        #endregion

        #region 初期処理

        /// <summary>
        /// 初期化処理
        /// </summary>
        protected void InitializeSet( ) {
            //アクセスカウンター登録
            Dao.Com.AccessCounterDao.Entry( base.CurrentPageInfo.pageId );

            // 不良タイプリストの初期値に設定する
            ListItem[] ServiceNgTypeList = ControlUtils.GetListItems( AiImageCheckDao.SelectNgTypeList( NG_FLAG));
            if ( 0 < ServiceNgTypeList.Length ) {
                //検索結果がnullでない場合
                ddlNgType.Items.Clear();
                ddlNgType.Items.Add( FLAG_DISP );
                ddlNgType.Items.AddRange( ServiceNgTypeList );
            }

            // 是正タイプリストの初期値に設定する
            ListItem[] ServiceCorrectiveTypeList = ControlUtils.GetListItems( AiImageCheckDao.SelectCorrectiveTypeList( CORRECTIVE_FLAG ) );
            _resultData.Clear();
            for ( int decLoop = 0; decLoop < ServiceCorrectiveTypeList.Length; decLoop++ ) {
                _resultData.Add( ServiceCorrectiveTypeList[decLoop].Value, ServiceCorrectiveTypeList[decLoop].Text.Split( '|' )[1] );
                ServiceCorrectiveTypeList[decLoop].Text = ServiceCorrectiveTypeList[decLoop].Text.Split( '|' )[0];
            }

            if ( 0 < ServiceCorrectiveTypeList.Length ) {
                //検索結果がnullでない場合
                ddlCorrectiveType.Items.Clear();
                ddlCorrectiveType.Items.Add( FLAG_DISP );
                ddlCorrectiveType.Items.AddRange( ServiceCorrectiveTypeList );
            }

            //パラメータ取得
            _orgData.Clear();
            _orgData.Add( "imgAnlItemCd", Request.QueryString["imgAnlItemCd"] );
            _orgData.Add( "revisionNo", Request.QueryString["revisionNo"] );
            _orgData.Add( "anlResult", Request.QueryString["anlResult"] );
            _orgData.Add( "imgAnlItemNm", Request.QueryString["imgAnlItemNm"] );
            _orgData.Add( "correctiveResult", Request.QueryString["correctiveResult"] );
            _orgData.Add( "ngTypeCd", Request.QueryString["ngTypeCd"] );
            _orgData.Add( "correctiveTypeCd", Request.QueryString["correctiveTypeCd"] );
            _orgData.Add( "correctiveComment", Request.QueryString["correctiveComment"] );
            _orgData.Add( "updateBy", Request.QueryString["updateBy"] );
            _orgData.Add( "thisTimeUserName", LoginInfo.UserInfo.userName );
            _orgData.Add( "thisTimeUserID", LoginInfo.UserInfo.userId );
            _orgData.Add( "updateSys", PageInfo.MainCorrectiveInput.pageId );

            //画面の初期化
            lblAnlResult.Text = _orgData["anlResult"].ToString();
            lblImgAnlItemNm.Text = _orgData["imgAnlItemNm"].ToString();
            txtCorrectiveComment.Text = _orgData["correctiveComment"].ToString();
            lblUpdateBy.Text = _orgData["updateBy"].ToString();
            lblThisTimeUpdateBy.Text = _orgData["thisTimeUserName"].ToString();
            lblCorrectiveResult.Text = _orgData["correctiveResult"].ToString();
            ddlNgType.SelectedValue = _orgData["ngTypeCd"].ToString();
            ddlCorrectiveType.SelectedValue = _orgData["correctiveTypeCd"].ToString();
        }

        #endregion

        #region ボタンイベント

        /// <summary>
        /// 更新ボタン押下時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdate_Click( object sender, EventArgs e ) {
            base.RaiseEvent( DoUpdate );
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        private void DoUpdate( ) {
            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //詳細情報検索
            bindParam.Add( "ddlLineCd", MainCorrectiveList.ddlLineCd );                 //ラインコード
            bindParam.Add( "ddlStation", MainCorrectiveList.ddlStation );               //ステーション
            bindParam.Add( "txtIdno", MainCorrectiveList.txtIdno );                     //IDNO
            bindParam.Add( "txtMonthlySeqNum", MainCorrectiveList.txtMonthlySeqNum );   //月度連番
            bindParam.Add( "txtRecordNo", MainCorrectiveList.txtRecordNo );             //来歴
            bindParam.Add( "txtImgAnlItemCd", _orgData["imgAnlItemCd"].ToString() );    //画像解析項目コード
            bindParam.Add( "txtRevisionNo", _orgData["revisionNo"].ToString() );        //改定番号
            bindParam.Add( "updateBy", _orgData["thisTimeUserID"].ToString() );         //更新者
            bindParam.Add( "correctiveComment", txtCorrectiveComment.Text );            //是正コメント
            bindParam.Add( "correctiveTypeCd", ddlCorrectiveType.SelectedValue );       //是正タイプコード
            bindParam.Add( "ngTypeCd", ddlNgType.SelectedValue );                       //不良タイプコード
            bindParam.Add( "updateSys", _orgData["updateSys"].ToString() );             //更新プログラム

            string anlRet = "";   //検査結果
            if ( lblAnlResult.Text.Equals( "NG" ) ) {
                anlRet = "2";
            } else if ( lblAnlResult.Text.Equals( "OK" ) ) {
                anlRet = "1";
            }

            string corRet = ""; //是正結果
            if ( lblCorrectiveResult.Text.Equals( "" ) ) {
                bindParam.Add( "correctiveResult", "" ); //是正結果
            } else {
                if ( lblCorrectiveResult.Text.Equals( "OK" ) ) {
                    corRet = "1";
                    anlRet = "1";
                } else if ( lblCorrectiveResult.Text.Equals( "NG" ) ) {
                    corRet = "2";
                    anlRet = "2";
                }
                bindParam.Add( "correctiveResult", corRet );
            }
            bindParam.Add( "anlResult", anlRet );

            int retCnt = 0;
            //更新
            retCnt = AiImageCheckDao.UpdateAnlResult( bindParam );
            if ( retCnt.Equals( 1 ) ) {
                //正常
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_INF_10010 );
            } else if ( retCnt.Equals( 0 ) ) {
                //データなし
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_72040 );
            } else {
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
            }
            btnUpdate.Enabled = false;
        }

        #endregion

        #region 業務処理

        /// <summary>
        /// 是正タイプリスト選択動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCorrectiveTypeList_SelectedIndexChanged( object sender, EventArgs e ) {
            lblCorrectiveResult.Text = "";
            if ( ddlCorrectiveType.SelectedValue.Equals( "" ) ) {
                lblCorrectiveResult.Text = "";
                return;
            } else {
                string selVal = _resultData[ddlCorrectiveType.SelectedValue].ToString();
                if ( selVal.Equals( "1" ) ) {
                    lblCorrectiveResult.Text = "OK";
                } else if ( selVal.Equals( "2" ) ) {
                    lblCorrectiveResult.Text = "NG";
                }
            }
        }

        /// <summary>
        /// 是正コメント入力文字数制限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtCorrectiveComment_TextChange( object sender, EventArgs e ) {
            if ( 100 < txtCorrectiveComment.Text.Length ) {
                txtCorrectiveComment.Text = txtCorrectiveComment.Text.Substring(0,100);
            }
        }

        #endregion
    }
}