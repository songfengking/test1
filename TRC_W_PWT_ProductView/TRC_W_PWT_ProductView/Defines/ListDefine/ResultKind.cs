using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace TRC_W_PWT_ProductView.Defines.ListDefine {
    public static class ResultKind {
        /// <summary>
        /// 結果種別（基本）
        /// </summary>
        public static class Inspection{
            /// <summary>NG</summary>
            public const string NG = "0";
            /// <summary>OK</summary>
            public const string OK = "1";
            /// <summary>NG表示文字列</summary>
            private const string NG_DISPLAY_STRING = "NG";
            /// <summary>OK表示文字列</summary>
            private const string OK_DISPLAY_STRING = "OK";
            /// <summary>検査結果リスト</summary>
            public static ListItem[] GetList() => new ListItem[] {
                new ListItem( string.Empty, string.Empty ),
                new ListItem( OK_DISPLAY_STRING, OK ),
                new ListItem( NG_DISPLAY_STRING, NG )
            };
        }

        /// <summary>
        /// 結果種別（噴射時期計測 03エンジン用）
        /// </summary>
        public static class EngineInjection03 {
            /// <summary>OK</summary>
            public const string OK = "0";
            /// <summary>NG</summary>
            public const string NG = "1";
            /// <summary>再計測</summary>
            public const string REMEASUREMENT = "2";
            /// <summary>OK表示文字列</summary>
            private const string OK_DISPLAY_STRING = "OK";
            /// <summary>NG表示文字列</summary>
            private const string NG_DISPLAY_STRING = "NG";
            /// <summary>再計測表示文字列</summary>
            private const string REMEASUREMENT_DISPLAY_STRING = "再計測";
            /// <summary>検査結果リスト</summary>
            public static ListItem[] GetList() => new ListItem[] {
                new ListItem( string.Empty, string.Empty ),
                new ListItem( OK_DISPLAY_STRING, OK ),
                new ListItem( NG_DISPLAY_STRING, NG ),
                new ListItem( REMEASUREMENT_DISPLAY_STRING, REMEASUREMENT )
            };
        }

        /// <summary>
        /// 結果種別（エンジン運転場 03・07エンジン共通）
        /// </summary>
        public static class EngineTest {
            /// <summary>OK</summary>
            public const string OK = "1";
            /// <summary>NG</summary>
            public const string NG = "2";
            /// <summary>OK表示文字列</summary>
            private const string OK_DISPLAY_STRING = "OK";
            /// <summary>NG表示文字列</summary>
            private const string NG_DISPLAY_STRING = "NG";
            /// <summary>検査結果リスト</summary>
            public static ListItem[] GetList() => new ListItem[] {
                new ListItem( string.Empty, string.Empty ),
                new ListItem( OK_DISPLAY_STRING, OK ),
                new ListItem( NG_DISPLAY_STRING, NG ),
            };
        }
    }
}