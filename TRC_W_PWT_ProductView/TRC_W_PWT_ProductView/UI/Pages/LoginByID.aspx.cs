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
using TRC_W_PWT_ProductView.Business;

namespace TRC_W_PWT_ProductView.UI.Pages {

    /// <summary>
    /// ログイン画面
    /// </summary>
    public partial class LoginByID : BaseForm {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region スクリプトイベント
        /// <summary>
        /// ID/PW 未入力側にフォーカス移動 双方入力済みの時には、ログインボタン押下
        /// </summary>
        const string CHECK_INPUT_LOGIN = "LoginByID.CheckInput();";

        #endregion

        #region イベント

        /// <summary>
        /// ページロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load( object sender, EventArgs e ) {
            base.RaiseEvent( DoPageLoad, false );
        }

        #region ログインボタン選択処理
        /// <summary>
        /// ログインボタン選択処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLogin_Click( object sender, EventArgs e ) {
            base.RaiseEvent( DoLogin );
        }
        #endregion

        #region ゲストログイン
        /// <summary>
        /// ゲストログイン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuestLogin_Click( object sender, EventArgs e ) {
            Login( Defines.User.Guest.GUEST_ID, Defines.User.Guest.GUEST_PW );
        }
        #endregion

        #region パスワード変更処理
        /// <summary>
        /// パスワード変更処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnChangePassword_Click( object sender, EventArgs e ) {
            //画面遷移
            PageUtils.RedirectToTRC( this, CurrentPageInfo.pageId, PageInfo.ChangePassword.url, this.Token, null );
        }
        #endregion

        #endregion

        #region イベントメソッド

        #region ページイベント

        /// <summary>
        /// ページロード処理
        /// </summary>
        protected override void DoPageLoad() {

            //ベース ページロード処理
            base.DoPageLoad();

            //javascriptイベント文字列作成
            string javaScriptEvent = String.Format( CHECK_INPUT_LOGIN );

            //ユーザID、パスワードにjavascriptを設定
            txtUserID.Attributes[ControlUtils.ON_KEY_UP] = javaScriptEvent;
            txtPassword.Attributes[ControlUtils.ON_KEY_UP] = javaScriptEvent;
            
        }

        /// <summary>
        /// ログイン処理
        /// </summary>
        private void DoLogin() {

            string loginId = txtUserID.Value.Trim();
            string password = txtPassword.Value.Trim();

            if ( true == CheckLogin( loginId, password ) ) {
                Login( loginId, password );
            }
        }

        #endregion

        #endregion

        #region メソッド

        #region 画面初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        protected override void Initialize() {
            //ベース処理初期化処理
            base.Initialize();

            //フォーカス設定
            base.ClientFocus = txtUserID.ClientID;
        }
        
        #endregion

        /// <summary>
        /// ログインチェック処理
        /// </summary>
        /// <param name="loginId">ログインID</param>
        /// <param name="password">パスワード</param>
        /// <returns>正否</returns>
        private bool CheckLogin( string loginId, string password ) {
            
            if ( true == StringUtils.IsEmpty( loginId )
                || true == StringUtils.IsEmpty( password ) ) {
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_83010 );
                return false;
            }

            return true;
        }

        /////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ログイン処理
        /// </summary>
        /// <param name="loginId">ログインID</param>
        /// <param name="password">パスワード</param>
        /// <returns>正否</returns>
        /// <remarks>KTServiceによる認証を行います。</remarks>
        private bool Login( string loginId, string password ) {
            
            KTAuthLogin.WSUserInfoDto resultDto = null;

            //WEBサービスによる認証処理
            WebServices webService = new WebServices();
            resultDto = webService.Login( loginId, password, KTWebUtils.GetClientIp( Request ) );  //ユーザID/PWD認証
            if ( true == WebServicesUtils.IsSrvError( resultDto.resultCode ) ) {
                //認証失敗
                string errMsg = WebServicesUtils.GetErrMsg( resultDto.resultCode );

                logger.Error( "{0} 認証失敗 エラーコード:{1} エラーメッセージ:{2}", MethodBase.GetCurrentMethod().Name, resultDto.resultCode, errMsg );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_89001, resultDto.resultCode, errMsg );
                return false;               
            }

            //WEBサービスアクセストークン登録
            webService.SetAccessInfo( resultDto.accessToken, resultDto.terminalCode, resultDto.userId );

            //ログイン情報保持
            UserInfoSessionHandler.ST_USER loginInfo = new UserInfoSessionHandler.ST_USER();
            //loginInfo.UserInfo = resultDto;
            SessionManager.GetUserInfoHandler().SetUserInfo( loginInfo );

            //画面遷移
            PageUtils.RedirectToTRC( this, CurrentPageInfo.pageId, PageInfo.MainView.url, this.Token, null );

            return true;
        }

        #endregion

    }
}