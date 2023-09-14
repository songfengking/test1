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
    public partial class DpfSerial : BaseForm {
        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義
        //部品区分
        const string CONST_PARTS_DPF = "DPF";  //DPF
        const string CONST_PARTS_DOC = "DOC";  //DOC
        const string CONST_PARTS_SCR = "SCR";  //SCR
        const string CONST_PARTS_ACU = "ACU";  //ACU

        //画面mode
        const string CONST_EXEC_NEW = "1";      //新規
        const string CONST_EXEC_UPD = "2";      //更新

        #endregion

        #region 変数定義
        Dictionary<string, object> dicControlValues = new Dictionary<string, object>();
        private Dictionary<string, string> _orgData = new Dictionary<string, string>();
        private static string selectedPartsKbn = PartsKind.PARTS_CD_ENGINE_DPF;
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
        /// <summary>
        /// (メイン)コントロール定義
        /// </summary>
        ControlDefine[] _mainControls = null;
        /// <summary>
        /// (メイン)コントロール定義アクセサ
        /// </summary>
        ControlDefine[] MainControls {
            get {
                return _mainControls;
            }
        }
        /// <summary>
        /// 検索条件定義情報
        /// </summary>
        ControlDefine[] _conditionControls = null;
        /// <summary>
        /// 検索条件定義情報アクセサ
        /// </summary>
        ControlDefine[] ConditionControls {
            get {
                if ( true == ObjectUtils.IsNull( _conditionControls ) ) {
                    _conditionControls = ControlUtils.GetControlDefineArray( typeof( CONDITION ) );
                }
                return _conditionControls;
            }
        }

        /// <summary>
        /// 画面に設定されているトークン参照
        /// </summary>
        public string GetThisToken {
            get {
                return Token;
            }
        }
        #endregion

        #region 検索条件定義
        /// <summary>
        /// 一覧検索条件定義
        /// </summary>
        public class CONDITION {
            /// <summary>型式コード</summary>
            public static readonly ControlDefine modelCdOrg = new ControlDefine( "txtModelCd", "型式コード", "modelCdOrg", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>製品機番</summary>
            public static readonly ControlDefine serialOrg = new ControlDefine( "txtSerial", "製品機番", "serialOrg", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>ステーション</summary>
            public static readonly ControlDefine st = new ControlDefine( "txtST", "ステーション", "st", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>ラインコード</summary>
            public static readonly ControlDefine lineCd = new ControlDefine( "txtLineCd", "ラーンコード", "lineCd", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>部品型式コード</summary>
            public static readonly ControlDefine modelCdDpf = new ControlDefine( "txtDpfModelCd", "部品型式コード", "modelCdDpf", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>部品機番</summary>
            public static readonly ControlDefine serialDpf = new ControlDefine( "txtDpfSerial", "部品機番", "serialDpf", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>製造種別</summary>
            public static readonly ControlDefine productKind = new ControlDefine( "txtProductKind", "製造種別", "productKind", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>IDNO</summary>
            public static readonly ControlDefine idno = new ControlDefine( "txtIdno", "IDNO", "idno", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>取付日</summary>
            public static readonly ControlDefine assemblyDt = new ControlDefine( "cldAssemblyDt", "取付日", "assemblyDt", ControlDefine.BindType.Both, typeof( String ) );
        }
        #endregion

        #region スクリプトイベント
        const string CLOSE_MODAL_DISP = "DpfSerial.CloseModal();";
        #endregion

        #region メソッド
        protected void Page_Load( object sender, EventArgs e ) {
            CurrentForm.RaiseEvent( DoPageLoad );
        }

        /// <summary>
        /// 部品区分の設定
        /// </summary>
        private void CreatePartsKbn() {

            string productKind = StringUtils.ToString( txtProductKind.Text );

            if ( productKind.Equals( ProductKind.Engine ) ) {
                //エンジン選択
                ListItem[] liArr = new ListItem[2];
                liArr[0] = new ListItem( CONST_PARTS_DPF, PartsKind.PARTS_CD_ENGINE_DPF );
                liArr[1] = new ListItem( CONST_PARTS_DOC, PartsKind.PARTS_CD_ENGINE_DOC );

                ControlUtils.SetListControlItems( ddlParts, liArr );

            } else if ( productKind.Equals( ProductKind.Tractor ) ) {
                //トラクタ選択
                ListItem[] liArr = new ListItem[3];
                liArr[0] = new ListItem( CONST_PARTS_DPF, PartsKind.PARTS_CD_ENGINE_DPF );
                liArr[1] = new ListItem( CONST_PARTS_SCR, PartsKind.PARTS_CD_ENGINE_SCR );
                liArr[2] = new ListItem( CONST_PARTS_ACU, PartsKind.PARTS_CD_ENGINE_ACU );

                ControlUtils.SetListControlItems( ddlParts, liArr );
            } else {
                //部品選択
                ListItem[] liArr = new ListItem[4];
                liArr[0] = new ListItem( CONST_PARTS_DPF, PartsKind.PARTS_CD_ENGINE_DPF );
                liArr[1] = new ListItem( CONST_PARTS_DOC, PartsKind.PARTS_CD_ENGINE_DOC );
                liArr[2] = new ListItem( CONST_PARTS_SCR, PartsKind.PARTS_CD_ENGINE_SCR );
                liArr[3] = new ListItem( CONST_PARTS_ACU, PartsKind.PARTS_CD_ENGINE_ACU );

                ControlUtils.SetListControlItems( ddlParts, liArr );
            }

            //            liArr = Common.DataUtils.InsertBlankItem( liArr );  //空白行追加
            //初期値設定
            ddlParts.SelectedValue = PartsKind.PARTS_CD_ENGINE_DPF;
        }
        #endregion
        #region ページイベント

        /// <summary>
        /// ページロード処理
        /// </summary>
        protected override void DoPageLoad() {

            //ベース ページロード処理
            base.DoPageLoad();

            //初期設定
            if ( IsPostBack == false ) {
                Initi();
            }

            //閉じるボタン
            btnClose.Attributes[ControlUtils.ON_CLICK] = CLOSE_MODAL_DISP;

            if ( Request.QueryString["exeKbn"].Equals( CONST_EXEC_NEW ) ) {
                btnUpdate.Text = "登録";
            } else {
                btnUpdate.Text = "更新";
                ddlParts.ReadOnly = true;
                cldAssemblyDt.ReadOnly = true;
            }

        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        private void Initi() {
            //アクセスカウンター登録
            Dao.Com.AccessCounterDao.Entry( base.CurrentPageInfo.pageId );

            InitializeSet();

            //更新権限チェック
            AppPermission.PERMISSION_INFO permMainteInfo = AppPermission.GetTransactionPermission( PageInfo.DpfSerial, LoginInfo.UserInfo );
            if ( permMainteInfo.IsView == false ) {
                PageUtils.RedirectToErrorPage( Page, MsgManager.MESSAGE_ERR_72030, null );
                return;
            } else if ( permMainteInfo.IsEdit == false ) {
                this.btnUpdate.Enabled = false;
            }
        }

        /// <summary>
        /// 初期値設定
        /// </summary>
        public void InitializeSet() {

            cldAssemblyDt.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day );    //初期値
            selectedPartsKbn = PartsKind.PARTS_CD_ENGINE_DPF;

            //対象データチェック
            if ( Request.QueryString["exeKbn"].Equals( CONST_EXEC_UPD ) && StringUtils.IsEmpty( Request.QueryString["subIndex"] ) ) {
                base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62020, "更新対象データ" );
                btnUpdate.Enabled = false;
                return;
            }

            txtExeKbn.Text = Request.QueryString["exeKbn"];
            txtProductKind.Text = Request.QueryString["productKind"];
            txtLineCd.Text = Request.QueryString["lineCd"];
            txtST.Text = Request.QueryString["st"];
            txtIdno.Text = Request.QueryString["idno"];

            //セッションよりデータ取得
            string callerToken = Request.QueryString["subToken"];
            ConditionInfoSessionHandler.ST_CONDITION condMainView = SessionManager.GetConditionInfoHandler( callerToken ).GetCondition( PageInfo.MaintenanceDpfListDetail.pageId );


            int index = NumericUtils.ToInt( Request.QueryString["subIndex"], 0 );

            if ( true == ObjectUtils.IsNotNull( condMainView.ResultData ) && index < condMainView.ResultData.Rows.Count ) {
                DataRow rowMainView = condMainView.ResultData.Rows[index];

                //非表示項目に設定
                if ( Request.QueryString["exeKbn"].Equals( CONST_EXEC_NEW ) ) {
                    if ( ProductKind.Engine.Equals( Request.QueryString["productKind"] ) ) {
                        txtIdno.Text = StringUtils.ToString( rowMainView["ENG_IDNO"] );
                    } else {
                        txtIdno.Text = StringUtils.ToString( rowMainView["TRC_IDNO"] );
                    }
                } else {

                    txtDpfModelCd.Text = DataUtils.GetModelCd( StringUtils.ToString( rowMainView["DPF_MODEL_CD"] ) );
                    txtDpfSerial.Text = StringUtils.ToString( rowMainView["DPF_SERIAL_NO"] );

                    txtModelCd.Text = DataUtils.GetModelCd( StringUtils.ToString( rowMainView["DPF_MODEL_CD"] ) );    //変更前のDPF型式
                    txtSerial.Text = StringUtils.ToString( rowMainView["DPF_SERIAL_NO"] );                          //変更前のDPF機番

                    if ( ProductKind.Engine.Equals( Request.QueryString["productKind"] ) ) {
                        txtIdno.Text = StringUtils.ToString( rowMainView["ENG_IDNO"] );
                    } else {
                        txtIdno.Text = StringUtils.ToString( rowMainView["TRC_IDNO"] );
                    }
                }
            }


            //検索パラメータ作成
            Dictionary<string, object> dicCondition = new Dictionary<string, object>();

            base.GetControlValues( ConditionControls, ref dicCondition );

            //検索時画面情報取得
            Dictionary<string, string> dicIdWithText = new Dictionary<string, string>();
            base.GetControlTexts( ConditionControls, out dicIdWithText );

            //検索結果取得
            //エラー時にも最後まで処理を通すため、インスタンスを生成しておく
            MainViewBusiness.ResultSet result = new MainViewBusiness.ResultSet();
            int maxGridViewCount = WebAppInstance.GetInstance().Config.WebCommonInfo.maxGridViewCount;  //検索上限数

            try {
                result.ListTable = EngineProcessDao.SelectDpfSerialList( dicCondition, maxGridViewCount );

            } catch ( DataAccessException ex ) {
                if ( ex.OracleErrorNumber == DataAccessBase.ORAERROR_TIMEOUT ) {
                    //クエリ発行タイムアウト
                    base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_61910 );
                } else {
                    //タイムアウト以外のException
                    logger.Exception( ex );
                    base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                }
            } catch ( Exception ex ) {
                logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
            } finally {
                //メッセージ設定
                result.Message = null;
                if ( null == result.ListTable || 0 == result.ListTable.Rows.Count ) {
                    //検索結果0件
                    result.Message = new Msg( MsgManager.MESSAGE_WRN_62010, "DPF機番情報" );

                } else if ( ( null != result.ListTable && maxGridViewCount <= result.ListTable.Rows.Count ) ) {
                    //検索件数が上限を上回っている場合には警告メッセージをセット
                    result.Message = new Msg( MsgManager.MESSAGE_WRN_61020 );
                }

            }

            //部品区分
            CreatePartsKbn();

            //更新時は登録データの値を設定
            if ( 0 < result.ListTable.Rows.Count ) {
                if ( Request.QueryString["exeKbn"].Equals( CONST_EXEC_UPD ) ) {
                    ddlParts.SelectedValue = StringUtils.ToString( result.ListTable.Rows[0]["PARTS_KBN"] );
                    cldAssemblyDt.Value = DateUtils.ToDate( result.ListTable.Rows[0]["ASSEMBLY_DT"] );
                }
            }

            //部品名変更後の制御
            ChangePartKbn();

        }

        #endregion

        #region 更新処理
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdate_Click( object sender, EventArgs e ) {

            //更新前入力チェック
            if ( false == CheckInputData() ) {
                return;
            }
            Dictionary<string, object> dicCondition = new Dictionary<string, object>();
            base.GetControlValues( ConditionControls, ref dicCondition );

            //必要？？
            Dictionary<string, string> dicIdWithText = new Dictionary<string, string>();
            base.GetControlTexts( ConditionControls, out dicIdWithText );

            //登録／更新
            int retCnt = 0;
            if ( txtExeKbn.Text == "1" ) {
                //新規登録
                retCnt = EngineProcessDao.InsertDpfSerial( dicCondition, LoginInfo.UserInfo.userId, PageInfo.DpfSerial.pageId, selectedPartsKbn );
            } else {
                //更新
                retCnt = EngineProcessDao.UpdateDpfSerial( dicCondition, LoginInfo.UserInfo.userId, PageInfo.DpfSerial.pageId );
            }

            switch ( retCnt ) {
            case 0:     //正常終了
                if ( txtExeKbn.Text == CONST_EXEC_NEW ) {
                    base.WriteApplicationMessage( MsgManager.MESSAGE_INF_10010 );   //新規
                } else {
                    base.WriteApplicationMessage( MsgManager.MESSAGE_INF_10020 );   //更新
                }
                break;
            case 1:     //一意制約エラー
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_72010 );
                break;
            default:
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_72060, "更新" );
                break;
            }

            //画面項目制御
            if ( retCnt == 0 ) {
                ddlParts.ReadOnly = true;
                txtDpfModelCd.ReadOnly = true;
                txtDpfSerial.ReadOnly = true;
                cldAssemblyDt.ReadOnly = true;
                btnUpdate.Enabled = false;
            }


        }
        /// <summary>
        /// 処理実行前データチェック
        /// </summary>
        /// <returns></returns>
        private bool CheckInputData() {

            CurrentForm.ClearApplicationMessage();

            string strDpfModelCd = StringUtils.ToString( txtDpfModelCd.Text );
            string strDpfSerial = StringUtils.ToString( txtDpfSerial.Text );
            string execKbn = StringUtils.ToString( txtExeKbn.Text );


            //入力チェック
            if ( StringUtils.IsEmpty( strDpfModelCd ) ) {
                base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62030, "DPF型式コード" );
                return false;
            }

            if ( StringUtils.IsEmpty( strDpfSerial ) ) {
                base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62030, "DPF機番" );
                return false;
            }

            DateTime dt;
            if (false == ( DateTime.TryParse( cldAssemblyDt.Text, out dt )) ) {
                base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62050, "取付日" );
                return false;            
            }

            //桁数チェック
            if ( false == ( strDpfModelCd.Trim().Length == 10 ) ) {
                base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62050, "DPF型式コード" );
                return false;
            }

            //桁数チェック
            if ( ddlParts.SelectedValue == PartsKind.PARTS_CD_ENGINE_ACU ) {
                //ACU 17桁
                if ( false == ( strDpfSerial.Trim().Length == 17 ) ) {
                    base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62040, ddlParts.Text, "17" );
                    return false;
                }
            } else {
                //それ以外は 10桁
                if ( false == ( strDpfSerial.Trim().Length == 10 ) ) {
                    base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62040, ddlParts.Text, "10" );
                    return false;
                }
            }

            //部品区部の重複チェック
            if ( execKbn.Equals( "1" ) ) {
                //新規時のみ
                string callerToken = Request.QueryString["subToken"];
                ConditionInfoSessionHandler.ST_CONDITION condMainView = SessionManager.GetConditionInfoHandler( callerToken ).GetCondition( PageInfo.MaintenanceDpfListDetail.pageId );

                DataTable dtMainView = condMainView.ResultData;
                if ( false == ObjectUtils.IsNull(dtMainView) &&  0 < dtMainView.Rows.Count ) {
                    DataRow[] drArray = dtMainView.Select( "PARTS_KBN = '" + ddlParts.Text + "'" );
                    if ( 0 < drArray.Length ) {
                        base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62060, "部品名" );
                        return false;
                    }
                }
            }


            //ユーザ情報チェック
            if ( true == ObjectUtils.IsNull( LoginInfo.UserInfo )
                || true == StringUtils.IsEmpty( LoginInfo.UserInfo.userName ) ) {
                PageUtils.RedirectToErrorPage( Page, MsgManager.MESSAGE_ERR_72020, null );
                return false;
            }

            //更新権限チェック
            AppPermission.PERMISSION_INFO permMainteInfo = AppPermission.GetTransactionPermission( PageInfo.DpfSerial, LoginInfo.UserInfo );
            if ( permMainteInfo.IsEdit == false ) {
                PageUtils.RedirectToErrorPage( Page, MsgManager.MESSAGE_ERR_72030, null );
                return false;
            }



            return true;
        }
        #endregion

        protected void ddlParts_SelectedIndexChanged( object sender, EventArgs e ) {

            selectedPartsKbn = StringUtils.ToString( ddlParts.SelectedValue );

            base.RaiseEvent( ChangePartKbn );
        }
        /// <summary>
        /// 部品名変更の関連項目の制御
        /// </summary>
        private void ChangePartKbn() {

            if ( ddlParts.SelectedValue == PartsKind.PARTS_CD_ENGINE_ACU ) {
                txtDpfSerial.MaxLength = 17;
            } else {
                if ( 17 <= txtDpfSerial.Text.Length ) {
                    string tmp = txtDpfSerial.Text.Substring( 0, 10 );
                    txtDpfSerial.Text = tmp;
                }
                txtDpfSerial.MaxLength = 10;
            }
        }
    }
}