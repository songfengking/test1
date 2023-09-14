using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using KTFramework.Common;
using KTWebInheritance.Common;
using TRC_W_PWT_ProductView.UI.Base;
using TRC_W_PWT_ProductView.Session;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Common;
using KTFramework.Web.Client.SvcCore;
using KTFramework.Web.Client.CoreService;
using KTFramework.Web.Client.Service;

namespace TRC_W_PWT_ProductView.UI.MasterForm {
    /// <summary>
    /// マスターページ（メイン画面）
    /// </summary>
    public partial class MasterMain : MasterPage {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義

        #region スクリプトイベント
        /// <summary>
        /// ウィンドウクローズ キー
        /// </summary>
        const string WINDOW_CLOSE_KEY = "WindowClose";

        /// <summary>
        /// ウィンドウクローズ 呼び出しスクリプト
        /// </summary>
        const string WINDOW_CLOSE = "ControlCommon.WindowClose();";

        #endregion

        #endregion

        #region プロパティ

        /// <summary>
        /// 画面に設定されているトークン参照
        /// </summary>
        public string GetThisToken {
            get { return this.hdnToken.Value; }
        }

        /// <summary>
        /// フォーカス
        /// </summary>
        public string ClientFocus {
            get { return this.hdnFocus.Value; }
            set { this.hdnFocus.Value = value; }
        }

        /// <summary>
        /// 表示中ページ(ベースフォーム)情報
        /// </summary>
        private BaseForm CurrentForm {
            get {
                return ( (BaseForm)Page );
            }
        }

        /// <summary>
        /// 表示中ページ情報
        /// </summary>
        private PageInfo.ST_PAGE_INFO CurrentPageInfo {
            get {
                return CurrentForm.CurrentPageInfo;
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
                    //SessionManagerInstance sesMgr = CurrentForm.SessionManager;
                    //_loginInfo = sesMgr.GetUserInfoHandler().GetUserInfo();

                    _loginInfo = CurrentForm.LoginInfo;
                }

                return _loginInfo;
            }
        }

        /// <summary>
        /// ユーザ情報
        /// </summary>
        public void SetUserInfo() {

            if ( true == ObjectUtils.IsNotNull( LoginInfo.UserInfo )
                && true == StringUtils.IsNotEmpty( LoginInfo.UserInfo.userName ) ) {
                lblUserName.Text = LoginInfo.UserInfo.userName;                                              //ログインユーザ
                if ( true == StringUtils.IsNotEmpty( LoginInfo.UserInfo.bumonName ) ) {
                    lblBelongsTo.Text = LoginInfo.UserInfo.bumonName + " " + LoginInfo.UserInfo.lineName;    //課・ライン
                } else if ( true == StringUtils.IsNotEmpty( LoginInfo.UserInfo.companyName ) ) {
                    lblBelongsTo.Text = LoginInfo.UserInfo.companyName;                                      //取引先名
                } else {
                    lblBelongsTo.Text = "";                                                                  //ゲスト
                }

                if ( CurrentPageInfo.pagetype.Equals( PageInfo.PageType.Maintenance ) ) {
                    btnChangeUser.Visible = false;                                                           //ユーザ切替
                    btnMainView.Visible = false;                                                             //リンクボタン（メインメニュー）
                    btnMaintenanceMenu.Visible = false;                                                      //リンクボタン（メニュー）

                } else {
                    btnChangeUser.Visible = true;                                                            //ユーザ切替
                    
                    if ( CurrentPageInfo.pageId.Equals(PageInfo.MainView.pageId ) || CurrentPageInfo.pageId.Equals(PageInfo.MainPartsView.pageId)) {
                        btnMainView.Visible = false;                                                         //リンクボタン（検索画面）
                    }else{
                        btnMainView.Visible = true;                                                          //リンクボタン（検索画面）
                    }

                    //ユーザ権限取得
                    //AppPermission.PERMISSION_INFO permMainteInfo = AppPermission.GetTransactionPermission( PageInfo.MaintenanceMenu, LoginInfo.UserInfo );
                    //btnMaintenanceMenu.Visible = permMainteInfo.IsView;                                          //リンクボタン（メンテナンスメニュー）

                    btnMaintenanceMenu.Visible = true;
                }

                //リンクボタン(検索画面)は無条件で非表示にする
                btnMainView.Visible = false;

            } else {
                //ログイン画面
                lblUserName.Text = "";                //ログインユーザ
                lblBelongsTo.Text = "";               //課・ライン
                btnChangeUser.Visible = true;         //ユーザ切替
                btnMainView.Visible = false;          //リンクボタン（メインメニュー）
                btnMaintenanceMenu.Visible = true;    //リンクボタン（メンテナンス）    tomita
            }

            //詳細画面は、ユーザ切替、メインメニュー、メンテナンスを非表示
            if ( PageInfo.DetailFrame.pageId == CurrentPageInfo.pageId || PageInfo.DetailPartsFrame.pageId == CurrentPageInfo.pageId || PageInfo.KanbanPickingDetailView.pageId == CurrentPageInfo.pageId ) {
                btnChangeUser.Visible = false;        //ユーザ切替
                btnMainView.Visible = false;          //リンクボタン（メインメニュー）
                btnMaintenanceMenu.Visible = false;   //リンクボタン（メンテナンス）
            }

            ////System.Web.HttpBrowserCapabilities browser = Request.Browser;
            //if ( Request.UserAgent.ToUpper().Contains("CHROME") ) {
            //    btnLogout.Visible = false;
            //}
        }

        #endregion

        #region イベント

        #region ページ初期化処理
        /// <summary>
        /// ページ初期化処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init( object sender, EventArgs e ) {
            Page.PreLoad += master_Page_PreLoad;
        }
        #endregion

        #region ページロード前処理
        /// <summary>
        /// ページロード前処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void master_Page_PreLoad( object sender, EventArgs e ) {
            CurrentForm.RaiseEvent( MasterPagePreLoad, false );
        }
        #endregion

        /// <summary>
        /// ページロード処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load( object sender, EventArgs e ) {
            CurrentForm.RaiseEvent( MasterPageLoad, false );
        }

        /// <summary>
        /// メンテナンスメニューボタン処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMaintenanceMenu_Click( object sender, EventArgs e ) {
            PageUtils.RedirectToTRC( this.Page, CurrentPageInfo.pageId, PageInfo.MaintenanceMenu.url, GetThisToken, null );
        }

        /// <summary>
        /// メインメニューボタン処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMainView_Click( object sender, EventArgs e ) {
            PageUtils.RedirectToTRC( this.Page, CurrentPageInfo.pageId, PageInfo.MainView.url, GetThisToken, null );
        }

        /// <summary>
        /// ユーザ切替ボタン処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnChangeUser_Click( object sender, EventArgs e ) {
            C9910s c9910s = new C9910s();
            Dictionary<string, string> dicTransInfo = new Dictionary<string, string>();

            string urlNum = Page.Request.QueryString.Get( C9910s.ApiasParameter.URL_NUM );
            if ( StringUtils.IsBlank( urlNum ) ) {
                urlNum = WebAppInstance.GetInstance().Config.ApplicationInfo.defaultUrlNum;
                logger.Info( "URL_NUMが未設定な為、デフォルト設定を使用します。 デフォルト値 = {0}", urlNum );
            }

            //const string URL_NUM = "1";
            dicTransInfo.Add( C9910s.ApiasParameter.AP_ID, WebAppInstance.KTAUTH_AP_MASTER_ID );
            dicTransInfo.Add( C9910s.ApiasParameter.URL_NUM, urlNum );
            dicTransInfo.Add( C9910s.ApiasParameter.LOGIN_METHOD, C9910s.LoginMethod.IDPW_ONLY );
            string url = c9910s.GetRedirectUrl( C9910s.ApiasApplicationId.KTAUTH_WEB_AP_MASTER_ID, urlNum );
            if ( StringUtils.IsBlank( url ) ) {
                PageUtils.RedirectToErrorPage( this.Page, MsgManager.MESSAGE_ERR_70140 );
            }
            KTWebUtils.RedirectTo( this.Page, url, dicTransInfo );
            //PageUtils.RedirectToTRC( this.Page, CurrentPageInfo.pageId, PageInfo.LoginByTrc.url, GetThisToken, null );
        }

        /// <summary>
        /// 閉じる(ログアウト)ボタン処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLogout_Click( object sender, EventArgs e ) {
            //セッションクリア
            //CurrentForm.SessionManager.ClearByToken( GetThisToken );
            //KTWebUtils.RedirectTo( this.Page, PageInfo.Top.url, null );
            CurrentForm.Terminate();

            ScriptManager.RegisterClientScriptBlock( this.Page, this.Page.GetType(), WINDOW_CLOSE_KEY, WINDOW_CLOSE, true );
        }

        #endregion

        #region イベントメソッド

        /// <summary>
        /// ロード前イベント
        /// </summary>
        private void MasterPagePreLoad() {

            //セッション、不正遷移チェック
            CheckAccess();

            if ( false == IsPostBack
                && false == ScriptManager.GetCurrent( Page ).IsInAsyncPostBack ) {

                //初回アクセス 且つ トークンなし 且つ トークン発行画面の時に、セッション開始
                string token = GetThisToken;
                SessionManagerInstance sesMgr = CurrentForm.SessionManager;
                if ( true == StringUtils.IsBlank( token ) ) {
                    ////メインページの時には、新規トークンを発行しセッション作成(本来はログイン画面 又は MACsリダイレクトパラメータ有無)
                    //if ( PageInfo.MainView.pageId == CurrentPageInfo.pageId ) {
                    //    SetNewToken();
                    if ( PageInfo.DetailFrame.pageId == CurrentPageInfo.pageId || PageInfo.DetailPartsFrame.pageId == CurrentPageInfo.pageId || PageInfo.KanbanPickingDetailView.pageId == CurrentPageInfo.pageId ) {

                        //詳細外枠の時には、リクエストパラメータを参照する
                        //string coop = Page.Request.QueryString.Get( RequestParameter.DetailFrame.EXTERNAL_COOP );
                        //if ( true == StringUtils.IsNotBlank( coop ) ) {
                        //    //外部連携からの表示
                        //    SetNewToken();
                        //} else {
                        //    //一覧からの表示                            
                        //    hdnToken.Value = Page.Request.QueryString.Get( RequestParameter.DetailFrame.TOKEN );
                        //}

                        //常に新規トークンでセッション分割(常時WindowOpen)
                        SetNewToken();

                        //} else if ( PageInfo.DetailFrame.pageId == CurrentPageInfo.pageId ) {
                    } else {

                        //引き継ぎトークンがあれば、トークンの引き継ぎを行う
                        string preToken = Page.Request.QueryString.Get( RequestParameter.Common.TOKEN );
                        if ( StringUtils.IsNotBlank( preToken ) ) {

                            //ページ種別によってトークン取得を分岐
                            if ( CurrentPageInfo.pagetype.Equals( PageInfo.PageType.Maintenance ) ) {
                                SetNewToken();
                            } else {
                                hdnToken.Value = preToken;
                            }

                        } else {
                            //トークンなし 且つ チェック処理を通過している場合、新規トークンでセッション分割
                            SetNewToken();
                        }
                    }

                    //画面間引き継ぎ情報設定
                    Dictionary<string, object> dicTransInfo = new Dictionary<string, object>();
                    foreach ( string reqKey in Page.Request.QueryString.AllKeys ) {
                        dicTransInfo.Add( reqKey, Page.Request.QueryString.Get( reqKey ) );
                    }

                    sesMgr.GetTransitionInfoHandler( GetThisToken ).SetTransInfo( CurrentPageInfo.pageId, dicTransInfo );

                    SetSessionKeep();

                }
            }
        }

        /// <summary>
        /// ロードイベント
        /// </summary>
        private void MasterPageLoad() {

            //タイトル設定
            this.lblTitle.Text = CurrentPageInfo.title;

            //ユーザ情報を取得し、画面の更新を行う
            SetUserInfo();

        }

        #endregion

        #region セッション、不正アクセスチェック

        /// <summary>
        /// セッション、不正アクセスチェック 異常時にはエラー画面に遷移
        /// </summary>
        private void CheckAccess() {
            string token = "";
            if ( false == IsPostBack
                && false == ScriptManager.GetCurrent( Page ).IsInAsyncPostBack ) {
                //初回遷移
                token = Page.Request.QueryString.Get( RequestParameter.Common.TOKEN );
            } else {
                //ポストバック
                token = GetThisToken;
            }

            SessionManagerInstance sesMgr = CurrentForm.SessionManager;
            if ( true == StringUtils.IsNotBlank( token ) ) {
                if ( false == sesMgr.IsAlive( token ) ) {
                    //セッションタイムアウト
                    PageUtils.RedirectToErrorPage( Page, MsgManager.MESSAGE_ERR_70110, null );
                }
            } else {

                //トークンチェック不要画面
                if ( CurrentPageInfo.pagetype == PageInfo.PageType.MainView ) {
                    ;
                } else {
                
                    //ログイン画面/AD認証画面の時には、新規ログイン可能
                    if ( PageInfo.LoginByTrc.pageId == CurrentPageInfo.pageId ) {
                        //パラメータチェック
                        //内部 OR 外部 ETCETC 
                    } else if ( PageInfo.DetailFrame.pageId == CurrentPageInfo.pageId || PageInfo.DetailPartsFrame.pageId == CurrentPageInfo.pageId ) {
                        //詳細外枠の時には、リクエストパラメータを参照する(MACsリダイレクトパラメータ無しの時には、不可
                        string coop = Page.Request.QueryString.Get( RequestParameter.DetailFrame.EXTERNAL_COOP );
                        if ( true == StringUtils.IsBlank( coop ) ) {
                            PageUtils.RedirectToErrorPage( Page, MsgManager.MESSAGE_ERR_70120, null );
                        }
                    } else if ( PageInfo.Error.pageId == CurrentPageInfo.pageId ) {
                        //エラーページの時には、何も行わない
                    } else {
                        //前画面トークン引き継ぎありなら遷移可
                        string preToken = Page.Request.QueryString.Get( RequestParameter.Common.TOKEN );
                        if ( true == StringUtils.IsNotBlank( preToken ) ) {
                            //エラーとしない
                        } else {

                            string callerID = Page.Request.QueryString.Get( RequestParameter.Common.CALLER );

                            //Top ID、AD認証 エラーページ からの遷移時には引き継ぎトークンなし
                            if ( PageInfo.Top.pageId == callerID
                                || PageInfo.LoginByTrc.pageId == callerID
                                || PageInfo.Error.pageId == callerID ) {
                                //エラーとしない
                            } else {
                                //引き継ぎトークンなしの場合には、エラー
                                PageUtils.RedirectToErrorPage( Page, MsgManager.MESSAGE_ERR_70120, null );
                            }
                        }
                    }
                }

            }
        }

        #endregion

        #region メソッド
        /// <summary>
        /// 新規トークンの発行
        /// </summary>
        private void SetNewToken() {
            string newToken = KTWebUtils.GetCsrfToken();
            hdnToken.Value = newToken;
            SessionManagerInstance sesMgr = CurrentForm.SessionManager;
            sesMgr.GetTransitionInfoHandler( newToken ).SetTransInfo( CurrentPageInfo.pageId, new Dictionary<string, object>() );
        }

        /// <summary>
        /// セッションキープフレームのセット
        /// </summary>
        private void SetSessionKeep() {
            String reqParam = String.Format( "?{0}={1}", Defines.RequestParameter.Common.TOKEN, GetThisToken );
            frmSessionKeep.Attributes["src"] = Page.ResolveUrl(PageInfo.SessionKeep.url) + reqParam;
        }

        #endregion

        #region メッセージ欄更新

        /// <summary>
        /// メッセージ欄更新
        /// </summary>
        /// <param name="msgdef">メッセージ定義</param>
        /// <param name="parameters">メッセージパラメータ</param>
        internal void WriteApplicationMessage( MsgDef msgdef , params object[] parameters ) {
            StackFrame stackFrame = WriteMessageUtils.GetCalledStackFrame( MethodBase.GetCurrentMethod() );
            WriteApplicationMessage( stackFrame, msgdef, parameters );
        }

        /// <summary>
        /// メッセージ欄更新
        /// </summary>
        /// <param name="stackFrame">スタックフレーム</param>
        /// <param name="msgdef">メッセージ定義</param>
        /// <param name="parameters">メッセージパラメータ</param>
        internal void WriteApplicationMessage(StackFrame stackFrame, MsgDef msgdef, params object[] parameters ) {
            WriteMessageUtils.SetApplicationMessage( stackFrame, this.txtApplicationMessage, msgdef, parameters );
        }

        /// <summary>
        /// メッセージ欄更新
        /// </summary>
        /// <param name="msg">メッセージ定義</param>
        internal void WriteApplicationMessage( Msg msg ) {
            StackFrame stackFrame = WriteMessageUtils.GetCalledStackFrame( MethodBase.GetCurrentMethod() );
            WriteApplicationMessage( stackFrame, msg );
        }

        /// <summary>
        /// メッセージ欄更新
        /// </summary>
        /// <param name="stackFrame">スタックフレーム</param>
        /// <param name="msg">メッセージ定義</param>
        internal void WriteApplicationMessage( StackFrame stackFrame, Msg msg ) {
            WriteMessageUtils.SetApplicationMessage( stackFrame, this.txtApplicationMessage, msg );
        }

        /// <summary>
        /// メッセージ欄クリア
        /// </summary>
        internal void ClearApplicationMessage() {
            WriteMessageUtils.ClearApplicationMessage( this.txtApplicationMessage );
        }

        #endregion

        #region 製品別通過実績表示

        /// <summary>
        /// 製品別通過実績データ設定
        /// </summary>
        /// <param name="dt">表示データ</param>
        public void SetImaDokoData( DataTable dt ) {
            txtApplicationMessage.Width = 0;
            txtApplicationMessage.BorderStyle = BorderStyle.None;
            txtApplicationMessage.TextMode = TextBoxMode.SingleLine;
            divGrvMainView.Visible = true;

            grvMainView.DataSource = dt.DefaultView;
            grvMainView.DataBind();
            grvMainView.Width = dt.Columns.Count * 70;

        }

        /// <summary>
        /// 改行処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMainView_RowDataBound( object sender, GridViewRowEventArgs e ) {
            GridViewRow row = e.Row;
            if ( e.Row.RowType == DataControlRowType.DataRow ) {
                for ( int i = 0; i < row.Cells.Count; i++ ) {
//                    row.Cells[i].Text = row.Cells[i].Text.Replace( "\n", "&nbsp;&nbsp;&nbsp;<br/>&nbsp;" );
//                    row.Cells[i].Text = row.Cells[i].Text.Replace( "\n", "<br/>" );
                }
            }

        }

        #endregion

        #region マニュアル
        protected void btnManual_Click( object sender, EventArgs e ) {
            CurrentForm.RaiseEvent( DownLoadManual );
        }
        /// <summary>
        /// マニュアルダウンロード
        /// </summary>
        private void DownLoadManual() {
            //コンフィグ設定読み込み
            ConfigData configData = WebAppInstance.GetInstance().Config;
            
            //この制御をなんとかしたい
            switch (CurrentPageInfo.pagetype){
                //排ガス規制部品
            case PageInfo.PageType.Maintenance:
            case PageInfo.PageType.MainView:
                WebFileUtils.DownloadFile( this.CurrentForm, configData.ApplicationInfo.manualTemplateBasePath, configData.ApplicationInfo.manualFile );
                break;
            //対象外LIST
            //3C加工日
            default:
                break;
            }
        }
        #endregion
    }
}