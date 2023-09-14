using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using KTFramework.Common;

namespace KTWebInheritance.Common {

    public static class KTWebUtils {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        /// <summary>
        /// 画面遷移(Redirect)
        /// </summary>
        /// <param name="page">遷移元ページ</param>
        /// <param name="toUrl">遷移先ページURL</param>
        /// <param name="requestParameters">リクエストパラメータ</param>
        public static void RedirectTo( Page page, string toUrl, Dictionary<string, string> requestParameters ) {
            string path = page.ResolveUrl( toUrl );
            string param = MakeParamString( requestParameters );

            logger.Info( "画面遷移(Redirect):{0} {1}→{2}"
                , page.Title ?? "", page.Request.FilePath, path + param );

            page.Response.Redirect( path + param );
        }

        /// <summary>
        /// 画面遷移(ServerTransfer)
        /// </summary>
        /// <param name="page">遷移元ページ</param>
        /// <param name="toUrl">遷移先ページURL</param>
        /// <param name="requestParameters">リクエストパラメータ</param>
        /// <param name="preserve">RequestFormコレクションのクリア有無</param>
        public static void TransferTo( Page page, string toUrl, Dictionary<string, string> requestParameters, bool preserve = false ) {
            string path = page.ResolveUrl( toUrl );
            string param = MakeParamString( requestParameters );

            logger.Info( "画面遷移(ServerTransfer):{0} {1}→{2}"
                , page.Title ?? "", page.Request.FilePath, path + param );
            page.Server.Transfer( path + param, preserve );
        }

        /// <summary>
        /// 遷移先用リクエストパラメータ作成
        /// </summary>
        /// <param name="requestParameters">リクエストパラメータコレクション</param>
        /// <returns>リクエストパラメータ</returns>
        private static string MakeParamString( Dictionary<string, string> requestParameters ) {

            string param = null;

            if ( null != requestParameters ) {
                foreach ( string key in requestParameters.Keys ) {
                    if ( null == param ) {
                        param = "?";
                    } else {
                        param += "&";
                    }
                    param += key + "=" + HttpUtility.HtmlEncode( requestParameters[key] );
                }
            }

            return param;
        }
        
        /// <summary>
        /// CSRFトークン取得
        /// </summary>
        /// <returns></returns>
        public static string GetCsrfToken() {
            //16桁 32バイト
            const int TOKEN_LEN = 16;

            byte[] tokenByte = new byte[TOKEN_LEN];

            RNGCryptoServiceProvider generator = new RNGCryptoServiceProvider();
            generator.GetBytes( tokenByte );

            StringBuilder buf = new StringBuilder();

            for ( int tokenLoop = 0; tokenLoop < tokenByte.Length; tokenLoop++ ) {
                buf.AppendFormat( "{0:x2}", tokenByte[tokenLoop] );
            }

            return buf.ToString();
        }
        
        /// <summary>
        /// クライアントIPアドレス取得（IPv4）
        /// </summary>
        /// <param name="request">リクエスト</param>
        /// <returns>IPアドレス（IPv4）</returns>
        public static string GetClientIp( HttpRequest request ) {

            //IPv4チェック正規表現
            const string ADDRESS_MATCH = "^\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}$";
            System.Text.RegularExpressions.Regex matchIpv4 = new System.Text.RegularExpressions.Regex( ADDRESS_MATCH );

            //IPアドレス
            string addr = "";
            //IPアドレスリストを取得
            string remoteAddr = request.ServerVariables["REMOTE_ADDR"];

            string xForwardedFor = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            //リバースプロキシを使用することを前提として
            //リクエストの"HTTP_X_FORWARDED_FOR"からクライアントIPを取得する
            if ( false == string.IsNullOrEmpty( xForwardedFor ) ) {
                string[] xForwardedForList = xForwardedFor.Split( ',' );
                if ( xForwardedForList.Length != 0 ) {

                    //格納されたリストの先頭がクライアントIPの為取得
                    if ( true == matchIpv4.IsMatch( xForwardedForList[0].ToString() ) ) {
                        return xForwardedForList[0].ToString();
                    }
                }
            }

            //REMOTE_ADDRがIPv4の場合は、REMOTE_ADDRをリターン
            if ( false == string.IsNullOrEmpty( remoteAddr ) ) {

                if ( true == matchIpv4.IsMatch( remoteAddr ) ) {
                    return remoteAddr;
                }
            }

            //
            string remoteHost = request.ServerVariables["REMOTE_HOST"];
            string remoteHostAddress = "";

            //リモートホスト名が取得できれば優先（取れない時でもIISからIPアドレスが基本的には設定される）
            if ( false == string.IsNullOrEmpty( remoteHost ) ) {
                remoteHostAddress = remoteHost;
            } else {
                remoteHostAddress = remoteAddr;
            }

            IPAddress[] ipAddressArr = Dns.GetHostAddresses( remoteHostAddress );

            if ( null != ipAddressArr ) {
                //リスト分処理を行う
                foreach ( IPAddress address in ipAddressArr ) {
                    // IPv4を取得する
                    if ( AddressFamily.InterNetwork == address.AddressFamily ) {
                        addr = address.ToString();
                        break;
                    }
                }
            }

#if DEBUG
            //本番稼働時に空白文字になることは無いが、デバッグ時に空白文字になることがある為、デバッグ用に実装
            if ( "" == addr ) {
                IPHostEntry iphEntry = Dns.GetHostEntry( request.ServerVariables["REMOTE_ADDR"] );
                //リスト分処理を行う
                foreach ( IPAddress ipadress in iphEntry.AddressList ) {
                    //IPv4を取得する
                    if ( AddressFamily.InterNetwork == ipadress.AddressFamily ) {
                        addr = ipadress.ToString();
                    }
                }
            }

            if ( "" == addr ) {
                addr = "127.0.0.1";
            }
#endif

            return addr;
        }        
    }
}
