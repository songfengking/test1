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

namespace TRC_W_PWT_ProductView.UI.Pages.Maintenance {
    public partial class NACheckList : BaseForm {
        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義
        const string CLOSE_MODAL_DISP = "NACheckList.CloseModal();";
        const string CHECK_INPUT_DISP = "return NACheckList.CheckInput();";
        const string KEY_PRESS_EVENT = "onkeypress";

        const string _const_opr_kind = "4";
        const string _const_opr_value = "01";
        const string _const_disp_title = "重要チェック対象外";

        //処理区分
        enum ExecKbn : int {
            SELECT = 0,
            INSERT = 1,
            UPDATE = 2,
            DELETE = 3
        }

        #endregion

        #region 変数定義
        private Dictionary<string, string> _orgData = new Dictionary<string, string>();
        private int _execKbn = 1;
        #endregion
        
        #region 項目定義
        /// <summary>
        /// 検索条件定義
        /// </summary>
        public class CONDITION {
            /// <summary>型式コード</summary>
            public static readonly ControlDefine MODEL_CD = new ControlDefine( "txtModelCd", "型式コード", "modelCd", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>製品機番</summary>
            public static readonly ControlDefine SERIAL_NO = new ControlDefine( "txtSerial", "製品機番", "serial", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>チェック対象外</summary>
            public static readonly ControlDefine CHKNA = new ControlDefine( "ddlChkNA", "チェック対象外", "ChkNA", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>詳細</summary>
            public static readonly ControlDefine DTL = new ControlDefine( "ddlDtl", "詳細", "dtl", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>理由</summary>
            public static readonly ControlDefine REASON = new ControlDefine( "ddlReason", "理由", "reason", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>理由記入欄</summary>
            public static readonly ControlDefine NOTES = new ControlDefine( "txtNotes", "理由記入欄", "notes", ControlDefine.BindType.Both, typeof( String ) );

        }
        #endregion

        #region プロパティ

        /// <summary>
        /// 表示中ページ(ベースフォーム)情報
        /// </summary>
        private BaseForm CurrentForm {
            get {
                return ( ( BaseForm )Page );
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
        protected override void DoPageLoad() {

            //閉じる
            ktbtnClose.Attributes[ControlUtils.ON_CLICK] = CLOSE_MODAL_DISP;
            //理由記入欄
            txtNotes.Attributes[KEY_PRESS_EVENT] = CHECK_INPUT_DISP;
            //実行モード
            this._execKbn = NumericUtils.ToInt( Request.QueryString["exeKbn"] );

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
        public void InitializeSet() {

            //アクセスカウンター登録
            Dao.Com.AccessCounterDao.Entry( base.CurrentPageInfo.pageId );

            //パラメータ取得
            this._orgData.Add( "modelCd", Request.QueryString["modelCd"] );
            this._orgData.Add( "serial", Request.QueryString["serial"] );
            this._orgData.Add( "opr", Request.QueryString["opr"] );
            this._orgData.Add( "st", Request.QueryString["st"] );
            this._orgData.Add( "ptnCd", Request.QueryString["ptnCd"] );

            txtModelCd.Text = this._orgData["modelCd"];
            txtSerial.Text = this._orgData["serial"];

            //検索結果取得
            Business.DetailViewBusiness.ResultSet dtRet = new Business.DetailViewBusiness.ResultSet();

            try {
                //LISTの値取得
                dtRet = EngineProcessDao.SelectNACheckList();

            } catch ( DataAccessException ex ) {
                logger.Exception( ex );
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                return;
            } catch ( Exception ex ) {
                logger.Exception( ex );
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                return;
            } finally {
            }

            if ( 0 == dtRet.MainTable.Rows.Count ) {
                //検索結果0件
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62010, _const_disp_title );
                return;
            }

            txtNotes.ReadOnly = true;
            this.ddlDtl.Enabled = false;

            //初期表示データbind
            InitializeValues( dtRet );


            //処理区分で初期値設定
            if ( this._execKbn.Equals( (int)ExecKbn.INSERT ) ) {
                this.btnRegist.Text = "登録";

            } else if ( this._execKbn.Equals( (int)ExecKbn.DELETE ) ) {
                Initialize_Delete();
                this.btnRegist.Text = "削除";
            }

            ChangeProcess();

            //更新権限チェック
            AppPermission.PERMISSION_INFO permMainteInfo = AppPermission.GetTransactionPermission( PageInfo.NACheckList, LoginInfo.UserInfo );
            if ( permMainteInfo.IsView == false ) {
                PageUtils.RedirectToErrorPage( Page, MsgManager.MESSAGE_ERR_72030, null );
                return;
            } else if ( permMainteInfo.IsEdit == false ) {
                this.btnRegist.Enabled = false;
            }

        }

        /// <summary>
        /// DropDownListの初期値設定
        /// </summary>
        private void InitializeValues( Business.DetailViewBusiness.ResultSet res ) {

            //チェック対象外
            ControlUtils.SetListControlItems( ddlChkNA, GetClassItem( res.MainTable ) );

            //詳細
            ControlUtils.SetListControlItems( ddlDtl, GetClassItem( res.SubTable ) );

            //理由
            if ( this._orgData["ptnCd"].Equals( ProductKind.Engine ) ) {
                //エンジンの場合
                ControlUtils.SetListControlItems( ddlReason, WordList.EngineList );
            } else {
                //トラクタの場合
                ControlUtils.SetListControlItems( ddlReason, WordList.ToractorList );
            }

            ddlReason.SelectedIndex = 0;


        }
        /// <summary>
        /// リストBOX設定
        /// </summary>
        /// <param name="paramDt"></param>
        /// <returns></returns>
        public static ListItem[] GetClassItem( DataTable paramDt ) {
            DataRow[] rowClassArr ;

            rowClassArr = (
                from row in paramDt.AsEnumerable()
                select row
            ).ToArray();            

            return Common.ControlUtils.GetListItems( rowClassArr, "DISP_DATA", "KEY", true );

        }

        /// <summary>
        /// 初期値設定(削除)
        /// </summary>
        private void Initialize_Delete() {

            ddlChkNA.SelectedValue = Request.QueryString["opr"];
            ddlDtl.SelectedValue = Request.QueryString["st"];

            //理由の取得
            DataTable dtData = new DataTable();

            Dictionary<string, string> dicTmp = new Dictionary<string, string>();
            dicTmp.Add( "modelCd", this._orgData["modelCd"] );
            dicTmp.Add( "serial", this._orgData["serial"] );
            dicTmp.Add( "opr", this._orgData["opr"] );
            dicTmp.Add( "st", this._orgData["st"] );
            dicTmp.Add( "ptnCd", this._orgData["ptnCd"] );

            int maxGridViewCount = WebAppInstance.GetInstance().Config.WebCommonInfo.maxGridViewCount;  //検索上限数

            try {
                //SELECT実行
                dtData = EngineProcessDao.SelectNotApplicable1( dicTmp, maxGridViewCount );

            } catch ( DataAccessException ex ) {
                logger.Exception( ex );
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                return;
            } catch ( Exception ex ) {
                logger.Exception( ex );
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                return;
            } finally {
            }

            if ( dtData.Rows.Count > 0 ) {

                string notes = StringUtils.ToString( dtData.Rows[0]["NOTES"] );

                ListItem lst = new ListItem();
                lst = ddlReason.Items.FindByText( notes );
                if ( lst == null ) {
                    //Listに該当がない→その他選択時
                    ddlReason.SelectedValue = WordList.MainteCommon.COM_99.value;
                    txtNotes.Text = notes;
                } else {
                    //Listから選択時
                    int idx = ddlReason.Items.IndexOf( lst );
                    ddlReason.SelectedIndex = idx;                
                }
            }

            //画面コントロール制御
            this.ddlChkNA.Enabled = false;
            this.ddlReason.Enabled = false;

            return;
        }

        #endregion

        #region ボタンイベント
        /// <summary>
        /// 実行ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRegist_Click( object sender, EventArgs e ) {
            base.RaiseEvent( RegisteredData );
        }
        #endregion

        #region DropDownListイベント
        /// <summary>
        /// チェック対象外変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlChkNA_SelectedIndexChanged( object sender, EventArgs e ) {
            base.RaiseEvent( ChangeChkNACtrl );
        }

        /// <summary>
        /// 理由変更時関連項目制御
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlReason_SelectedIndexChanged( object sender, EventArgs e ) {
            base.RaiseEvent( ChangeReason );
        }

        #endregion

        #region 業務処理
        /// <summary>
        /// 理由DropDownList変更時の処理
        /// </summary>
        private void ChangeReason() {
            //「その他」選択時のみ、理由記入欄入力可
            if ( ddlReason.SelectedValue.Equals( WordList.MainteCommon.COM_99.value ) ) {
                txtNotes.ReadOnly = false;
            } else {
                txtNotes.Text = "";
                txtNotes.ReadOnly = true;
            }       
        }

        /// <summary>
        /// チェック対象外DropDownList変更時の処理
        /// </summary>
        private void ChangeChkNACtrl() {
            //関連項目制御
            ChangeProcess();
            //メッセージクリア
            ClearApplicationMessage();        
        }

        /// <summary>
        /// 詳細の制御
        /// </summary>
        private void ChangeProcess() {

            string tmp1 = ddlChkNA.SelectedValue;
            string strOprKind = "";
            string strOprValue = "";

            strOprKind = StringUtils.CutString( tmp1, 3 ).Trim();

            if ( StringUtils.IsNotEmpty( strOprKind ) ) {
                strOprValue = tmp1.Substring(3);
            }

            if ( strOprKind.Equals( _const_opr_kind ) && strOprValue.Equals( _const_opr_value ) ) {
                if ( this._execKbn.Equals( (int)ExecKbn.DELETE ) ) {
                    this.ddlDtl.Enabled = false;
                }else{
                    this.ddlDtl.Enabled = true;
                }
            }else{
                this.ddlDtl.SelectedValue = "";
                this.ddlDtl.Enabled = false;
            }

        }

        /// <summary>
        /// 処理実行前データチェック
        /// </summary>
        /// <returns></returns>
        private bool CheckInputData() {
        
            string strOprKind = "";
            string strOprValue = "";
            string strNotes = "";
            string strStation = "";
            string strPartsNm = "";

            //キー項目の設定
            string strMdlCd = txtModelCd.Text;
            string strSerial = txtSerial.Text;

            strMdlCd = strMdlCd.Replace( "-", "" );

            //チェック対象外
            string tmp1 = ddlChkNA.SelectedValue;

            strOprKind = StringUtils.CutString( tmp1, 3 ).Trim();
            if ( StringUtils.IsNotEmpty( strOprKind ) ) {
                strOprValue = tmp1.Substring( 3, 2 );
            }

            //詳細
            string tmp2 = ddlDtl.SelectedValue;

            if ( strOprKind.Equals( _const_opr_kind ) && strOprValue.Equals( _const_opr_value ) ) {
                if ( StringUtils.IsNotEmpty( tmp2 ) ) {
                    strStation = StringUtils.CutString( tmp2, 6 ).Trim();
                    strPartsNm = tmp2.Substring( 6 );
                }
            } else {
                strStation = null;
                strPartsNm = null;
            }

            //理由記入欄
            strNotes = StringUtils.ToString(txtNotes.Text);

            //入力チェック
            if ( true == StringUtils.IsEmpty( tmp1 ) ) {
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62020, "チェック対象外" );
                SetFocus( ddlChkNA );
                return false;
            }

            //理由の選択チェック
            if ( 0 == ddlReason.Text.Length ) {
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62020, "理由" );
                SetFocus( ddlReason );
                return false;            
            }

            //その他選択時
            if ( ddlReason.SelectedValue.Equals( WordList.MainteCommon.COM_99.value ) ) {
                if( true == StringUtils.IsEmpty( strNotes ) ) {
                    //理由記入欄未入力
                    CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62030, "理由記入欄" );
                    SetFocus( txtNotes );
                    return false;
                } else if ( 100 < strNotes.Length ) {
                    //100文字より入力
                    CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62050, "理由記入欄" );
                    SetFocus( txtNotes );
                    return false;
                } else if ( 0 < strNotes.Length && strNotes.Trim().Equals( "その他" ) ) {
                    //理由記入欄に「その他」と入力
                    CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62050, "理由記入欄" );
                    SetFocus( txtNotes );
                    return false;
                }

            }

            //登録済チェック
            if ( this._execKbn == (int)ExecKbn.INSERT ) {
                //新規時のみ

                //登録前重複データチェック
                DataTable dtData = new DataTable();
                Dictionary<string, string> dicTmp = new Dictionary<string, string>();
                dicTmp.Add( "modelCd", strMdlCd );
                dicTmp.Add( "serial", strSerial );
                dicTmp.Add( "opr", tmp1 );
                dicTmp.Add( "st", tmp2 );
                dicTmp.Add( "ptnCd", Request.QueryString["ptnCd"] );

                int maxGridViewCount = WebAppInstance.GetInstance().Config.WebCommonInfo.maxGridViewCount;  //検索上限数

                try {
                    //SELECT実行
                    dtData = EngineProcessDao.SelectNotApplicable1( dicTmp, maxGridViewCount );

                } catch ( DataAccessException ex ) {
                    logger.Exception( ex );
                    CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                    return false;
                } catch ( Exception ex ) {
                    logger.Exception( ex );
                    CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                    return false;
                } finally {
                }

                if ( dtData.Rows.Count > 0 ) {
                    //登録済エラー
                    CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_72010 );
                    return false;
                }
            }

            //ユーザ情報チェック
            if ( true == ObjectUtils.IsNull( LoginInfo.UserInfo )
                || true == StringUtils.IsEmpty( LoginInfo.UserInfo.userName ) ) {
                PageUtils.RedirectToErrorPage( Page, MsgManager.MESSAGE_ERR_72020, null );
                return false;
            }

            //更新権限チェック
            AppPermission.PERMISSION_INFO permMainteInfo = AppPermission.GetTransactionPermission( PageInfo.NACheckList, LoginInfo.UserInfo );
            if ( permMainteInfo.IsEdit == false ) {
                PageUtils.RedirectToErrorPage( Page, MsgManager.MESSAGE_ERR_72030, null );
                return false;
            }

            return true;
        }

        /// <summary>
        /// 登録／削除処理
        /// </summary>
        private void RegisteredData() {
            string strOprKind = "";
            string strOprValue = "";
            string strNotes = "";
            string strStation = "";
            string strPartsNm = "";

            //実行前チェック
            if ( false == CheckInputData() ) {
                return;
            }

            //キー項目の設定
            string strMdlCd = DataUtils.GetModelCd( StringUtils.ToString( txtModelCd.Text ) );
            string strSerial = DataUtils.GetSerial6( StringUtils.ToString( txtSerial.Text ) );

            //チェック対象外
            string tmp1 = ddlChkNA.SelectedValue;
            strOprKind = StringUtils.CutString( tmp1, 3 ).Trim();
            if ( StringUtils.IsNotEmpty( strOprKind ) ) {
                strOprValue = tmp1.Substring( 3, 2 );
            }

            //詳細
            string tmp2 = ddlDtl.SelectedValue;
            if ( strOprKind.Equals( _const_opr_kind ) && strOprValue.Equals( _const_opr_value ) ) {
                if ( StringUtils.IsNotEmpty( tmp2 ) ) {
                    strStation = StringUtils.CutString( tmp2, 6 ).Trim();
                    strPartsNm = tmp2.Substring( 6 );
                }
            } else {
                strStation = null;
                strPartsNm = null;
            }

            //理由記入欄
            if ( ddlReason.SelectedValue.Equals( WordList.MainteCommon.COM_99.value ) ) {
                strNotes = txtNotes.Text;
            } else {
                strNotes = ddlReason.SelectedItem.Text;
            }

            //処理実行
            int result = 0;

            if ( this._execKbn.Equals( (int)ExecKbn.INSERT ) ) {
                //新規
                result = EngineProcessDao.InsertCriticalCheckExclude(
                            strMdlCd, strSerial, strOprKind, strOprValue, strStation, strPartsNm,
                            strNotes, LoginInfo.UserInfo.userId, PageInfo.NACheckList.pageId
                            );

                if ( result.Equals( 1 ) ) {
                    //正常
                    CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_INF_10010 );
                } else if ( result.Equals( 0 ) ) {
                    //データなし
                    CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_72040 );
                } else {
                    CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                }
            } else if ( this._execKbn.Equals( (int)ExecKbn.DELETE ) ) {
                //削除
                result = EngineProcessDao.DeleteCriticalCheckExclude(
                            strMdlCd, strSerial, strOprKind, strOprValue, strStation, strPartsNm );
                if ( result.Equals( 1 ) ) {
                    //正常
                    CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_INF_10030 );
                } else if ( result.Equals( 0 ) ) {
                    //データなし
                    CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_72040 );
                } else {
                    CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                }
            } else {
                //処理なし
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                return;
            }

            //画面CLOSE
            this.btnRegist.Enabled = false;
        }

        #endregion
    }
}