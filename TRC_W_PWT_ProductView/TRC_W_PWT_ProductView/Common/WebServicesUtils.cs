using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using KTFramework.Common;

namespace TRC_W_PWT_ProductView.Common {
    /// <summary>
    /// データ用ユーティリティクラス
    /// </summary>
    public static class WebServicesUtils {

        /// <summary>
        /// WEBサービスPRIFIX長
        /// </summary>
        const int ERR_CD_PRIFIX_LEN = 6;

        /// <summary>
        /// WEBサービスエラーコード、エラーメッセージ
        /// </summary>
        static readonly string[,] WEBSRV_ERR_CODE_MSG = new string[,]{
                { "ES_AUT_9000_0001","アプリケーションを認証できません。" },
                { "ES_AUT_9000_0002","ユーザID、またはパスワードが異なります。" },
                { "ES_AUT_9000_0003","パスワードの有効期限切れです。" },
                { "ES_AUT_9000_0004","権限が取得できません。" },
                { "ES_AUT_9000_0005","端末を認証できません。" },
                { "ES_APP_9002_0001","アクセストークンが発行できません。" },
                { "ES_AUT_9000_9999","異常が発生しました。" },
                { "ES_AUT_9100_0001","旧パスワードが未入力です。" },
                { "ES_AUT_9100_0002","新パスワードが未入力です。" },
                { "ES_AUT_9100_0003","パスワード入力桁数が異常です。(7桁以上、20桁以下)" },
                { "ES_AUT_9100_0004","前回のパスワードから変更されていません。" },
                { "ES_AUT_9100_0005","異常が発生しました。" },
            };

        /////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// WEBサービス正常判定処理
        /// </summary>
        /// <param name="errCd">エラーコード</param>
        /// <returns>true = 正常 / false = 異常</returns>
        public static bool IsSrvNA( string errCd ) {
            //エラーコードPRIFIXがNAまたはWRN以外の場合にはエラー
            if ( true == StringUtils.IsBlank( errCd ) ) {
                return false;
            }
            if ( ERR_CD_PRIFIX_LEN <= errCd.Length && "NA_INF" == errCd.Substring( 0, ERR_CD_PRIFIX_LEN ) ) {
                return true;
            }
            return false;
        }

        /////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// WEBサービス警告判定処理
        /// </summary>
        /// <param name="errCd">エラーコード</param>
        /// <returns>true = 警告 / false = 警告なし</returns>
        public static bool IsSrvWarn( string errCd ) {
            //エラーコードPRIFIXがNAまたはWRN以外の場合にはエラー
            if ( true == StringUtils.IsBlank( errCd ) ) {
                return false;
            }
            if ( ERR_CD_PRIFIX_LEN <= errCd.Length && "NA_WRN" == errCd.Substring( 0, ERR_CD_PRIFIX_LEN ) ) {
                return true;
            }
            return false;
        }

        /////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// WEBサービスエラー判定処理
        /// </summary>
        /// <param name="errCd">エラーコード</param>
        /// <returns>true = 正常 / false = 異常</returns>
        public static bool IsSrvError( string errCd ) {
            //エラーコードPRIFIXがNAまたはWRN以外の場合にはエラー
            if ( true == StringUtils.IsBlank( errCd ) ) {
                return true;
            }
            if ( false == IsSrvNA( errCd ) && false == IsSrvWarn( errCd ) ) {
                return true;
            }
            return false;
        }

        /////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// WEBサービスエラーメッセージ取得
        /// </summary>
        /// <param name="errCd">エラーコード</param>
        /// <returns>エラーメッセージ</returns>
        public static string GetErrMsg( string errCd ) {

            string result = "";

            if ( true == StringUtils.IsBlank( errCd ) ) {
                return "";
            }

            //配列長さ
            int loopMax = WEBSRV_ERR_CODE_MSG.Length / WEBSRV_ERR_CODE_MSG.Rank;
            //該当するメッセージを取得
            for ( int loopCnt = 0; loopCnt < loopMax; loopCnt++ ) {

                if ( WEBSRV_ERR_CODE_MSG[loopCnt, 0] == errCd ) {
                    result = WEBSRV_ERR_CODE_MSG[loopCnt, 1];
                    break;
                }
            }

            return result;
        }
    }
}