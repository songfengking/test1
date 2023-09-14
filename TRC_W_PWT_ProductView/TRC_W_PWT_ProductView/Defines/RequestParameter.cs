using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TRC_W_PWT_ProductView.Defines {
    /// <summary>
    /// リクエストパラメータ用定義
    /// </summary>
    internal static class RequestParameter {
        /// <summary>
        /// エラー画面引き渡しキー
        /// </summary>
        internal static class Error {
            /// <summary>エラーコード</summary>
            internal const string ERROR_CD = "ErrorCd";
            /// <summary>エラーメッセージ</summary>
            internal const string ERROR_MSG = "ErrorMsg";
        }
        /// <summary>
        /// 画面遷移引き渡しキー
        /// </summary>
        internal static class Common {
            /// <summary>前画面トークン</summary>
            internal const string TOKEN = "Token";
            /// <summary>呼び元ページID</summary>
            internal const string CALLER = "CallerPageID";
        }
        /// <summary>
        /// 詳細外枠画面引き渡しキー
        /// </summary>
        internal static class DetailFrame {
            /// <summary>外部連携</summary>
            internal const string EXTERNAL_COOP = "Coop";
            /// <summary>トークン</summary>
            //internal const string TOKEN = "Token";
            /// <summary>インデックス</summary>
            internal const string INDEX = "Index";
            /// <summary>リスト初期選択グループ項目</summary>
            internal const string GROUP_CD = "GroupCd";
            /// <summary>リスト初期選択区分項目</summary>
            internal const string CLASS_CD = "ClassCd";
            /// <summary>型式コード</summary>
            internal const string MODEL_CD = "ModelCd";
            /// <summary>国コード</summary>
            internal const string COUNTRY_CD = "CountryCd";
            /// <summary>機番</summary>
            internal const string SERIAL = "Serial";
            /// <summary>画面ID</summary>
            internal const string PAGE_ID = "PageID";
        }
        /// <summary>
        /// 詳細外枠画面引き渡しキー(部品)
        /// </summary>
        internal static class DetailPartsFrame {
            /// <summary>外部連携</summary>
            internal const string EXTERNAL_COOP = "Coop";
            /// <summary>インデックス</summary>
            internal const string INDEX = "Index";
            /// <summary>リスト初期選択グループ項目</summary>
            internal const string GROUP_CD = "GroupCd";
            /// <summary>リスト初期選択区分項目</summary>
            internal const string CLASS_CD = "ClassCd";
            /// <summary>型式コード</summary>
            internal const string MODEL_CD = "ModelCd";
            /// <summary>国コード</summary>
            internal const string COUNTRY_CD = "CountryCd";
            /// <summary>機番</summary>
            internal const string SERIAL = "Serial";
            /// <summary>画面ID</summary>
            internal const string PAGE_ID = "PageID";
        }

        /// <summary>
        /// イメージ表示画面引き渡しキー
        /// </summary>
        internal static class ImageView {
            /// <summary>トークン</summary>
            internal const string TOKEN = "Token";
            /// <summary>管理ID</summary>
            internal const string MANAGE_ID = "manageId";
            /// <summary>インデックス</summary>
            internal const string INDEX = "Index";
            /// <summary>幅</summary>
            internal const string WIDTH = "Width";
            /// <summary>高さ</summary>
            internal const string HEIGHT = "Height";
            /// <summary>コンテンツタイプ</summary>
            internal const string CONTENT_TYPE = "ContentType";
        }

        /// <summary>
        /// エラー画面引き渡しキー
        /// </summary>
        internal static class BrowserChangeGuidance {
            /// <summary>エラーコード</summary>
            internal const string URL = "Url";
        }


        /// <summary>
        /// 外部連携 システムコード
        /// </summary>
        internal static class ExternalCoop {
            /// <summary>MACs</summary>
            internal const string MACs = "140010";
        }

        /// <summary>
        /// AD認証チェック処理
        /// </summary>
        internal static class LoginByAD {
            /// <summary>ログオンキー</summary>
            public const string LOGON_KEY = "log";
            /// <summary>リダイレクト先</summary>
            public const string REDIRECT_PAGE_ID = "redirect";
        }
    }
}