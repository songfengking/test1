using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TRC_W_PWT_ProductView.Defines {

    /// <summary>
    /// ページコントロール定義情報
    /// </summary>
    internal static class ResourcePath {
        /// <summary>
        /// イメージ
        /// </summary>
        internal static class Image {
            /// <summary>デフォルトローディング画像</summary>
            internal const string DefaultLoading = "~/Images/busy.gif";
            /// <summary>ダミー読み込み画像</summary>
            internal const string DummyLoad = "~/Images/dummyload.gif";
            /// <summary>ロード完了後差し替え元オリジナル画像パス用Attribute属性</summary>
            internal const string AttrOriginalSrc = "data-original";
        }

        /// <summary>
        /// イメージ
        /// </summary>
        internal static class CSS {
            /// <summary>
            /// 一覧項目 (グリッド、リストビュー)項目選択済用CSS
            /// </summary>
            internal　const string ListSelectedRow = "ui-state-highlight";
            /// <summary>
            /// 非表示用CSS
            /// </summary>
            internal const string Invisible = "invisible";
            /// <summary>
            /// 非表示(幅高さ0)用CSS
            /// </summary>
            internal const string SizeZero = "size-zero";
            /// <summary>
            /// ローディングDIV CSS
            /// </summary>
            internal const string LoadingDivBG = "loading-back-ground";
        }
    }
}