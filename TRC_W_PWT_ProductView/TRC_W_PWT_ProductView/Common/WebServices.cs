using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using KTFramework.Common;
using TRC_W_PWT_ProductView.KTAuthLogin;
using TRC_W_PWT_ProductView.Defines;

namespace TRC_W_PWT_ProductView.Common {
            
    /// <summary>
    /// Webサービスアクセスクラス
    /// </summary>
    public class WebServices {

        /// <summary>
        /// log4net定義
        /// </summary>
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region メンバ変数
        private String _webServiceUrl = "";
        private String _applicationName = "";
        private String _accessToken = "";
        private String _terminalName = "";
        private String _userId = "";
        #endregion

        /////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WebServices() {

            string webServiceUrl = "";
            string applicationName = "";

            if ( true == GetServiceUrl( out webServiceUrl, out applicationName ) ) {
                this._webServiceUrl = webServiceUrl;
                this._applicationName = applicationName;
            }
        }

        /////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WebServices( String webServiceUrl, String applicationName ) {
            this._webServiceUrl = webServiceUrl;
            this._applicationName = applicationName;
        }

        /////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// WEBサービスアクセス情報設定処理
        /// </summary>
        /// <param name="accessToken">WEBサービス認証時取得トークン</param>
        /// <param name="terminalName">端末名</param>
        /// <param name="userId">ユーザID</param>
        public void SetAccessInfo(String accessToken, String terminalName, String userId) {
            this._accessToken = accessToken;
            this._terminalName = terminalName;
            this._userId = userId;
        }

        /////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// KTServiceのURIを取得する
        /// </summary>
        /// <param name="methodName">WEBサービスの呼び出すメソッド名</param>
        /// <returns>WEBメソッドURI</returns>
        private string GetServiceUri(string methodName) {
            string ktServiceURL = this._webServiceUrl;
            return ktServiceURL + methodName;
        }

        /////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// KTService経由でログインする
        /// </summary>
        /// <param name="loginId">ログインID</param>
        /// <param name="password">パスワード</param>
        /// <param name="ipAddress">IPアドレス</param>
        /// <returns>WEBサービス戻り値</returns>
        public WSUserInfoDto Login(string loginId, string password, string ipAddress) {
            //ログイン認証メソッドインスタンス生成
            ktauthLogin webSvc = new ktauthLogin();
            WSUserInfoDto resultDto = new WSUserInfoDto();

            try {
                webSvc.Url = GetServiceUri("ktauthLogin");  //URI設定
                logger.Info( string.Format( "WEBサービス呼び出し URL:[{0}/authByIDPassword]", webSvc.Url ) );
                resultDto = webSvc.authByIDPassword( this._applicationName, loginId, password, false, true, true, true, ipAddress );
                logger.Info(string.Format("WEBサービス呼び出し成功 RTN:[{0}]", resultDto.resultCode));
                logger.Info(string.Format("WEBサービス戻り値:\r\n[{0}]", GetDtoPropertiesStr(resultDto)));

            } catch (Exception ex) {
                resultDto.resultCode = Defines.MsgManager.MESSAGE_ERR_89000.Code;
                resultDto.resultMessage = Defines.MsgManager.MESSAGE_ERR_89000.ToString();
                logger.Error("WEBサービス呼び出しエラー:ktauthLogin.authGiaMigByIDPassword()", ex);
            }

            return resultDto;
        }

        /////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// KTService経由でログインする
        /// </summary>
        /// <param name="logonId">ログオンID</param>
        /// <param name="ipAddress">IPアドレス</param>
        /// <returns>WEBサービス戻り値</returns>
        public WSUserInfoDto Login( string logonId, string ipAddress ) {
            //ログイン認証メソッドインスタンス生成
            ktauthLogin webSvc = new ktauthLogin();
            WSUserInfoDto resultDto = new WSUserInfoDto();

            try {
                webSvc.Url = GetServiceUri( "ktauthLogin" );  //URI設定
                logger.Info( string.Format( "WEBサービス呼び出し URL:[{0}/authByActiveDirectory]", webSvc.Url ) );
                resultDto = webSvc.authByActiveDirectory( logonId, true, true, ipAddress );
                logger.Info( string.Format( "WEBサービス呼び出し成功 RTN:[{0}]", resultDto.resultCode ) );
                logger.Info( string.Format( "WEBサービス戻り値:\r\n[{0}]", GetDtoPropertiesStr( resultDto ) ) );

            } catch ( Exception ex ) {
                resultDto.resultCode = Defines.MsgManager.MESSAGE_ERR_89000.Code;
                resultDto.resultMessage = Defines.MsgManager.MESSAGE_ERR_89000.ToString();
                logger.Error( "WEBサービス呼び出しエラー:ktauthLogin.authByActiveDirectory()", ex );
            }

            return resultDto;
        }

        /////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// KTService経由でログインする
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public WSUserInfoDto Login( string ipAddress ) {
            //ログイン認証メソッドインスタンス生成
            ktauthLogin webSvc = new ktauthLogin();
            WSUserInfoDto resultDto = new WSUserInfoDto();

            try {
                webSvc.Url = GetServiceUri( "ktauthLogin" );  //URI設定
                logger.Info( string.Format( "WEBサービス呼び出し URL:[{0}/authGiaMigByIPAddress]", webSvc.Url ) );
                resultDto = webSvc.authGiaMigByIPAddress( this._applicationName, ipAddress );
                logger.Info( string.Format( "WEBサービス呼び出し成功 RTN:[{0}]", resultDto.resultCode ) );
                logger.Info( string.Format( "WEBサービス戻り値:\r\n[{0}]", GetDtoPropertiesStr( resultDto ) ) );
            } catch ( Exception ex ) {
                resultDto.resultCode = Defines.MsgManager.MESSAGE_ERR_89000.Code;
                resultDto.resultMessage = Defines.MsgManager.MESSAGE_ERR_89000.ToString();
                logger.Error( "WEBサービス呼び出しエラー:ktauthLogin.authGiaMigByIPAddress()", ex );
            }

            return resultDto;
        }

        /////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// KTServiceからパスワード変更する
        /// </summary>
        public WSChangePwdDto ChangePassword( string loginId, string oldPassword, string newPassword, string ipAddress ) {
            ktauthLogin webSvc = new ktauthLogin();
            WSChangePwdDto resultDto = new WSChangePwdDto();

            try {
                webSvc.Url = GetServiceUri( "ktauthLogin" );  //URI設定
                logger.Info( string.Format( "WEBサービス呼び出し URL:[{0}/ChangePassword]", webSvc.Url ) );
                resultDto = webSvc.changePassword( this._accessToken, loginId, oldPassword, newPassword, false, true, ipAddress );
                logger.Info( string.Format( "WEBサービス呼び出し成功 RTN:[{0}]", resultDto.resultCode ) );
                logger.Info( string.Format( "WEBサービス戻り値:\r\n[{0}]", GetDtoPropertiesStr( resultDto ) ) );
            } catch ( Exception ex ) {
                resultDto.resultCode = Defines.MsgManager.MESSAGE_ERR_89000.Code;
                resultDto.resultMessage = Defines.MsgManager.MESSAGE_ERR_89000.ToString();
                logger.Error( "WEBサービス呼び出しエラー:ktauthLogin.changePassword()", ex );
            }
            return resultDto;
        }

        /////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// KTServiceからログアウトする
        /// </summary>
        public void Logout() {
            ktauthLogin webSvc = new ktauthLogin();

            try {
                webSvc.Url = GetServiceUri( "ktauthLogin" );  //URI設定
                logger.Info( string.Format( "WEBサービス呼び出し URL:[{0}/logout]", webSvc.Url ) );
                webSvc.logout( this._accessToken );
                logger.Info( "WEBサービス呼び出し成功" );
            } catch ( Exception ex ) {
                logger.Error( "WEBサービス呼び出しエラー:ktauthLogin.logout()", ex );
            }
            return;
        }

        /// <summary>
        /// Webサービスプロパティ戻り値取得
        /// </summary>
        /// <param name="result">Webサービス戻り値</param>
        /// <returns>Webサービス問い合わせ結果</returns>
        private static string GetDtoPropertiesStr(object result) {
            StringBuilder param = new StringBuilder("");
            if ( null == result || string.IsNullOrEmpty(result.ToString() )) {
                param.Append("(none)");
                return param.ToString();
            }
            try {
                System.Reflection.PropertyInfo[] fields = result.GetType().GetProperties();
                foreach (System.Reflection.PropertyInfo field in fields) {
                    object val = field.GetValue(result, null);
                    param.Append(field.Name);
                    param.Append("=[");
                    param.Append(null != val ? val.ToString() : "(null)");
                    param.Append("],");
                }
            } catch (Exception) {
                param.Append("(解析失敗)");
            }
            return param.ToString();
        }

        #region APマスタ
        /// <summary>
        /// サービスURL取得
        /// </summary>
        /// <param name="url">サービスURL</param>
        /// <param name="apNm">サービス名</param>
        /// <returns>サービスURL</returns>
        internal static bool GetServiceUrl( out string url , out string apNm) {
            url = "";
            apNm = "";
            bool result = false;

            DataTable tblAuthMaster = Dao.Com.KTAuthDao.SelectAPMaster( WebAppInstance.KTAUTH_AP_MASTER_ID );
            if ( 1 == tblAuthMaster.Rows.Count ) {
                url = StringUtils.ToString( tblAuthMaster.Rows[0]["ServiceUrl"] );
                apNm = StringUtils.ToString( tblAuthMaster.Rows[0]["ApplicationNm"] );
                 
                result = true;
            }

            return result;
        }
        #endregion
    }
}