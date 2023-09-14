using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TRC_W_PWT_ProductView.Defines {

    /// <summary>
    /// セッションキー定義情報
    /// </summary>
    internal static class Session {

        #region 固定キー(Token)
        /// <summary>セッションキー：ユーザ情報セッションハンドラ</summary>
        internal const string SESSION_STATE_KEY_USER_INFO = "_sess_st_user_info";

        #endregion

        #region ハンドラ
        /// <summary>セッションキー：ユーザ情報セッションハンドラ</summary>
        internal const string SESSION_KEY_USER_INFO = "_sess_user_info";
        /// <summary>セッションキー：画面間引継情報セッションハンドラ</summary>
        internal const string SESSION_KEY_TRANSITION_INFO = "_sess_transition_info";
        /// <summary>セッションキー：検索条件情報セッションハンドラ</summary>
        internal const string SESSION_KEY_CONDITION_INFO = "_sess_condition_info";
        /// <summary>セッションキー：画面コントロール情報セッションハンドラ</summary>
        internal const string SESSION_KEY_PAGE_CONTROL_INFO = "_sess_page_control_info";
        /// <summary>セッションキー：イメージ情報セッションハンドラ</summary>
        internal const string SESSION_KEY_IMAGE_INFO = "_sess_image_info";
        /// <summary>セッションキー：ViewState情報セッションハンドラ</summary>
        internal const string SESSION_KEY_VIEW_STATE_INFO = "_sess_view_state_info";

        #endregion

        /// <summary>
        /// 詳細外枠画面
        /// </summary>
        internal static class DetailFrame {
            /// <summary>
            /// 型式情報 ディクショナリキー
            /// </summary>
            public const string SESSION_PAGE_INFO_MODEL_INFO_KEY = "modelInfoData";
            /// <summary>
            /// 詳細情報 ディクショナリキー
            /// </summary>
            public const string SESSION_PAGE_INFO_DETAIL_KEY = "detailInfoData";
        }

        /// <summary>
        /// 詳細外枠画面
        /// </summary>
        internal static class DetailPartsFrame {
            /// <summary>
            /// 型式情報 ディクショナリキー
            /// </summary>
            public const string SESSION_PAGE_INFO_MODEL_INFO_KEY = "modelInfoData";
            /// <summary>
            /// 詳細情報 ディクショナリキー
            /// </summary>
            public const string SESSION_PAGE_INFO_DETAIL_KEY = "detailInfoData";
        }

    }
}