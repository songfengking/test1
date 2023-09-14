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
    /// AD認証画面
    /// </summary>
    public partial class LoginByAD : BaseForm {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region イベント

        /// <summary>
        /// ページロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load( object sender, EventArgs e ) {
            base.RaiseEvent( DoPageLoad, false );
        }

        #endregion

        #region ページイベント

        /// <summary>
        /// ページロード処理
        /// </summary>
        protected override void DoPageLoad() {

            //ベース ページロード処理
            base.DoPageLoad();
            //画面遷移処理
            RedirectPage();

        }
        #endregion

        #region メソッド

        #region 画面初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        protected override void Initialize() {
            //ベース処理初期化処理
            base.Initialize();
        }

        #endregion

        /// <summary>
        /// 画面遷移処理
        /// </summary>
        private void RedirectPage() {

            UserInfoSessionHandler.ST_USER loginInfo = base.SessionManager.GetUserInfoHandler().GetUserInfo();
            //ログイン済みか否かをチェック
            if ( true == ObjectUtils.IsNotNull( loginInfo.UserInfo )
                && true == StringUtils.IsNotEmpty( loginInfo.UserInfo.userName ) ) {
                //既存のログイン情報を使用する
            } else {

                //WEBサービスによる認証処理
                WebServices webService = new WebServices();
                KTAuthLogin.WSUserInfoDto resultDto = null;

                //パラメータからAD認証情報を取得
                string logonKey = Page.Request.QueryString.Get( RequestParameter.LoginByAD.LOGON_KEY );
                if ( true == StringUtils.IsBlank( logonKey ) ) {
                    resultDto = webService.Login( Defines.User.Guest.GUEST_ID, Defines.User.Guest.GUEST_PW, KTWebUtils.GetClientIp( Request ) );  //ゲスト認証
                } else {
                    resultDto = webService.Login( logonKey, KTWebUtils.GetClientIp( Request ) );  //ユーザAD認証
                }

                if ( true == WebServicesUtils.IsSrvError( resultDto.resultCode ) ) {
                    //認証失敗
                    string errMsg = WebServicesUtils.GetErrMsg( resultDto.resultCode );
                    logger.Error( "{0} 認証失敗 エラーコード:{1} エラーメッセージ:{2}", MethodBase.GetCurrentMethod().Name, resultDto.resultCode, errMsg );
                    PageUtils.RedirectToErrorPage( this, MsgManager.MESSAGE_ERR_89001, resultDto.resultCode, errMsg );
                }

                //ログイン情報保持
                loginInfo = new UserInfoSessionHandler.ST_USER();
                //loginInfo.UserInfo = resultDto;
                SessionManager.GetUserInfoHandler().SetUserInfo( loginInfo );

                //WEBサービスアクセストークン登録
                webService.SetAccessInfo( resultDto.accessToken, resultDto.terminalCode, resultDto.userId );
            }

            string redirectPageID = Page.Request.QueryString.Get( RequestParameter.LoginByAD.REDIRECT_PAGE_ID );
            if ( true == StringUtils.IsBlank( redirectPageID ) ) {
                //画面遷移 検索一覧(初期画面 Guest含む)へリダイレクト
                PageUtils.RedirectToTRC( this, CurrentPageInfo.pageId, PageInfo.MaintenanceMenu.url, this.Token, null );
            } else {
                PageInfo.ST_PAGE_INFO pageInfo = PageUtils.GetPageInfo( redirectPageID );
                if ( StringUtils.IsBlank( pageInfo.url ) ) {
                    //画面遷移 エラー画面へリダイレクト
                    logger.Error( "{0} 遷移先異常 遷移先指定PageID:{1}", MethodBase.GetCurrentMethod().Name, redirectPageID );
                    PageUtils.RedirectToErrorPage( this, MsgManager.MESSAGE_ERR_70140 );
                }

                //画面遷移 指定ページへリダイレクト
                //画面間引き継ぎ情報設定
                Dictionary<string, string> dicTransInfo = new Dictionary<string, string>();
                object[] objParamKeys = ControlUtils.GetStaticDefineArray( typeof( RequestParameter.LoginByAD ), typeof( string ) );
                System.Collections.ArrayList reqParamKeys = new System.Collections.ArrayList( objParamKeys );
                
                foreach ( string reqKey in Page.Request.QueryString.AllKeys ) {
                    //AD専用のパラメータは引継ぎパラメータから除外する
                    if ( true == reqParamKeys.Contains( reqKey ) ) {
                        continue;
                    }

                    dicTransInfo.Add( reqKey, Page.Request.QueryString.Get( reqKey ) );
                }
                PageUtils.RedirectToTRC( this, CurrentPageInfo.pageId, pageInfo.url, this.Token, dicTransInfo );

            }
        }

        #endregion
    }
}