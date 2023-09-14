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

namespace TRC_W_PWT_ProductView.UI.MasterForm {
    /// <summary>
    /// マスターページ（サブ画面）
    /// </summary>
    public partial class MasterSub : MasterPage {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

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
            CurrentForm.RaiseEvent( MasterPageLoad );
        }
        #endregion

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
                    //メインページの時には、新規トークンを発行しセッション作成(本来はログイン画面 又は MACsリダイレクトパラメータ有無)
                    if ( PageInfo.MainView.pageId == CurrentPageInfo.pageId ) {
                        string newToken = KTWebUtils.GetCsrfToken();
                        hdnToken.Value = newToken;
                        sesMgr.SetTransInfoSessionHandler( newToken, null );
                    }
                }
            }
        }

        /// <summary>
        /// ロードイベント
        /// </summary>
        private void MasterPageLoad() {

            //タイトル設定
//            this.lblTitle.Text = CurrentPageInfo.title;

            //ユーザ情報を取得し、画面の更新を行う
            //UserInfoDto userInfo = _pdsSessionManager.GetUserInfoSessionHandler().UserInfo;
            //string code = Page.Request.QueryString.Get( PdsWebConsts.REQ_PARAM_NAME_ERROR_CODE );

            //if ( ( true == StringUtils.IsNotEmpty( userInfo.UserName ) ) ) {
            //            //&& ( false == string.IsNullOrEmpty( userInfo.KaName ) )
            //            //&& ( false == string.IsNullOrEmpty( userInfo.LineName ) ) ) {
            //    lblUserName.Text = userInfo.UserName;                                 //ログインユーザ
            //    if ( true == StringUtils.IsNotEmpty( userInfo.KaName ) ) {
            //        lblBelongsTo.Text = userInfo.KaName + " " + userInfo.LineName;    //課・ライン
            //    } else if ( true == StringUtils.IsNotEmpty( userInfo.TorihikisakiName ) ) {
            //        lblBelongsTo.Text = userInfo.TorihikisakiName;                    //取引先名
            //    } else {
            //        lblBelongsTo.Text = "";                                           //ゲスト
            //    }
            //    ktbtnMainMenu.Visible = true;                                         //リンクボタン（メインメニュー）
            //    if ( true == StringUtils.IsNotEmpty(userInfo.UserId) && userInfo.UserId != PdsWebConsts.GUEST_USER_ID ) {
            //        ktbtnManual.Visible = true;                                       //リンクボタン（マニュアル）
            //        ktbtnChangePassword.Visible = true;                               //リンクボタン（パスワード変更）
            //    } else {
            //        ktbtnManual.Visible = true;                                       //リンクボタン（マニュアル）
            //        ktbtnChangePassword.Visible = false;                              //ゲストユーザ リンクボタン（パスワード変更）非表示
            //    }
            //    ktbtnLogout.Visible = true;                                           //リンクボタン（ログアウト）            
            //} else {
            //    //ログイン画面
            //    lblUserName.Text = "";                //ログインユーザ
            //    lblBelongsTo.Text = "";               //課・ライン
            //    ktbtnMainMenu.Visible = false;        //リンクボタン（メインメニュー）
            //    ktbtnManual.Visible = false;          //リンクボタン（マニュアル）
            //    ktbtnChangePassword.Visible = false;  //リンクボタン（パスワード変更）
            //    ktbtnLogout.Visible = false;          //リンクボタン（ログアウト）
            //}

            //Exception発生時(エラー画面内でログアウトをさせる)
            //if ( code != null ) {
            //    ktbtnMainMenu.Visible = false;        //リンクボタン（メインメニュー）
            //    ktbtnManual.Visible = false;          //リンクボタン（マニュアル）
            //    ktbtnChangePassword.Visible = false;  //リンクボタン（パスワード変更）
            //    ktbtnLogout.Visible = false;          //リンクボタン（ログアウト）
            //}
        }

        #endregion

        #region セッション、不正アクセスチェック

        /// <summary>
        /// セッション、不正アクセスチェック 異常時にはエラー画面に遷移
        /// </summary>
        private void CheckAccess() {
            string token = GetThisToken;
            SessionManagerInstance sesMgr = ( (BaseForm)Page ).SessionManager;
            if ( true == StringUtils.IsNotBlank( token ) ) {
                if ( false == sesMgr.IsAlive( token ) ) {
                    //セッションタイムアウト
                    PageUtils.RedirectToErrorPage( Page, MsgManager.MESSAGE_ERR_70110, null );
                }
            } else {

                if ( PageInfo.Error.pageId == CurrentPageInfo.pageId ) {
                    //エラーページの時には、何も行わない
                } else {
                    //不正な遷移
                    PageUtils.RedirectToErrorPage( Page, MsgManager.MESSAGE_ERR_70120, null );
                }
            }
        }

        #endregion

        #region メッセージ欄更新

        /// <summary>
        /// メッセージ欄更新
        /// </summary>
        /// <param name="msgdef">メッセージ定義</param>
        /// <param name="parameters">メッセージパラメータ</param>
        internal void WriteApplicationMessage( MsgDef msgdef, params object[] parameters ) {
            StackFrame stackFrame = WriteMessageUtils.GetCalledStackFrame( MethodBase.GetCurrentMethod() );
            WriteApplicationMessage( stackFrame, msgdef, parameters );
        }

        /// <summary>
        /// メッセージ欄更新
        /// </summary>
        /// <param name="stackFrame">スタックフレーム</param>
        /// <param name="msgdef">メッセージ定義</param>
        /// <param name="parameters">メッセージパラメータ</param>
        internal void WriteApplicationMessage( StackFrame stackFrame, MsgDef msgdef, params object[] parameters ) {
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
    }
}