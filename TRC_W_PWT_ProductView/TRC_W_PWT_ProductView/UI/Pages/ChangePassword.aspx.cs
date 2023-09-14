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
    /// パスワード変更画面
    /// </summary>
    public partial class ChangePassword : BaseForm {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region スクリプトイベント
        /// <summary>
        /// ID/旧PW/新PW/新PW確認 未入力側にフォーカス移動 全て入力済みの時には、更新ボタン押下
        /// 新PWと新PW確認の入力値チェック
        /// </summary>
        const string CHECK_INPUT = "ChangePassword.CheckInput();";

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

        #region 更新ボタン選択処理
        /// <summary>
        /// 更新ボタン選択処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click( object sender, EventArgs e ) {
            base.RaiseEvent( DoEdit );
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
            string javaScriptEvent = String.Format( CHECK_INPUT );

            //ユーザID、パスワード(旧、新、新確認)にjavascriptを設定
            txtUserID.Attributes[ControlUtils.ON_KEY_UP] = javaScriptEvent;
            txtOldPassword.Attributes[ControlUtils.ON_KEY_UP] = javaScriptEvent;
            txtNewPassword.Attributes[ControlUtils.ON_KEY_UP] = javaScriptEvent;
            txtNewPasswordConfirm.Attributes[ControlUtils.ON_KEY_UP] = javaScriptEvent;
            
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        private void DoEdit() {

            string loginId = txtUserID.Value.Trim();
            string oldPassword = txtOldPassword.Value.Trim();
            string newPassword = txtNewPassword.Value.Trim();
            string newPasswordConfirm = txtNewPasswordConfirm.Value.Trim();

            if ( true == CheckEdit( loginId, oldPassword, newPassword, newPasswordConfirm ) ) {
                Edit( loginId, oldPassword, newPassword, newPasswordConfirm );
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

            //初期値設定
            //ログイン情報取得
            UserInfoSessionHandler.ST_USER loginInfo = SessionManager.GetUserInfoHandler().GetUserInfo();
            if ( true == ObjectUtils.IsNotNull( loginInfo.UserInfo )
                && true == StringUtils.IsNotBlank( loginInfo.UserInfo.userId ) 
                && ( Defines.User.Guest.GUEST_ID != loginInfo.UserInfo.userId ) ){
                    txtUserID.Value = loginInfo.UserInfo.userId;
                    txtUserID.ReadOnly = true;
            }

            //フォーカス設定
            if ( false == txtUserID.ReadOnly ) {
                base.ClientFocus = txtUserID.ClientID;
            } else {
                base.ClientFocus = txtOldPassword.ClientID;
            }
        }
        
        #endregion

        /// <summary>
        /// 変更入力チェック
        /// </summary>
        /// <param name="loginId">ログインID</param>
        /// <param name="oldPassword">旧パスワード</param>
        /// <param name="newPassword">新パスワード</param>
        /// <param name="newPasswordConfirm">新パスワード確認</param>
        /// <returns>正否</returns>
        private bool CheckEdit( string loginId, string oldPassword, string newPassword, string newPasswordConfirm ) {

            loginId = loginId.Trim();
            oldPassword = oldPassword.Trim();
            newPassword = newPassword.Trim();
            newPasswordConfirm = newPasswordConfirm.Trim();

            //入力チェック
            if ( true == StringUtils.IsEmpty( loginId )
                || true == StringUtils.IsEmpty( oldPassword )
                || true == StringUtils.IsEmpty( newPassword )
                || true == StringUtils.IsEmpty( newPasswordConfirm ) ) {
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_83020 );
                return false;
            }
            //ゲストユーザ規制
            if ( Defines.User.Guest.GUEST_ID == loginId ) {
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_83050 );
                return false;
            }
            //新パスワードチェック
            if ( newPassword != newPasswordConfirm ) {
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_83030 );
                return false;
            }
            //旧新パスワードチェック
            if ( oldPassword == newPassword ) {
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_83040 );
                return false;
            }

            return true;
        }

        /////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// パスワード変更処理
        /// </summary>
        /// <param name="loginId">ログインID</param>
        /// <param name="oldPassword">旧パスワード</param>
        /// <param name="newPassword">新パスワード</param>
        /// <param name="newPasswordConfirm">新パスワード確認</param>
        /// <returns>エラーメッセージ</returns>
        /// <remarks>KTServiceによる認証を行います。</remarks>
        private bool Edit( string loginId, string oldPassword, string newPassword, string newPasswordConfirm ) {

            KTAuthLogin.WSChangePwdDto resultDto = null;

            //WEBサービスによるパスワード変更処理
            WebServices webService = new WebServices();
            resultDto = webService.ChangePassword( loginId, oldPassword ,newPassword, KTWebUtils.GetClientIp( Request ) );  //ユーザID/PWD認証
            if ( true == WebServicesUtils.IsSrvError( resultDto.resultCode ) ) {
                //認証失敗
                string errMsg = WebServicesUtils.GetErrMsg( resultDto.resultCode );

                logger.Error( "{0} パスワード変更失敗 エラーコード:{1} エラーメッセージ:{2}", MethodBase.GetCurrentMethod().Name, resultDto.resultCode, errMsg );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_89001, resultDto.resultCode, errMsg );
                return false;               
            }

            //WEBサービスによるログイン処理
            KTAuthLogin.WSUserInfoDto loginDto = webService.Login( loginId, newPassword, KTWebUtils.GetClientIp( Request ) );  //ユーザID/PWD認証
            if ( true == WebServicesUtils.IsSrvError( loginDto.resultCode ) ) {
                //認証失敗
                string errMsg = WebServicesUtils.GetErrMsg( loginDto.resultCode );

                logger.Error( "{0} パスワード変更後ログイン認証失敗 エラーコード:{1} エラーメッセージ:{2}", MethodBase.GetCurrentMethod().Name, loginDto.resultCode, errMsg );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_89001, loginDto.resultCode, errMsg );
                return false;
            }

            //WEBサービスアクセストークン登録
            webService.SetAccessInfo( loginDto.accessToken, loginDto.terminalCode, loginDto.userId );

            //ログイン情報保持
            UserInfoSessionHandler.ST_USER loginInfo = new UserInfoSessionHandler.ST_USER();
            //loginInfo.UserInfo = loginDto;
            SessionManager.GetUserInfoHandler().SetUserInfo( loginInfo );

            //画面遷移
            PageUtils.RedirectToTRC( this, CurrentPageInfo.pageId, PageInfo.MaintenanceMenu.url, this.Token, null );

            return true;
        }

        #endregion

    }
}