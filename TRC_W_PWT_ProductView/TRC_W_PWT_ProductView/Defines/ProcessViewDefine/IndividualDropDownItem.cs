using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace TRC_W_PWT_ProductView.Defines.ProcessViewDefine {
    /// <summary>
    /// 工程固有のドロップダウンアイテムの定義（判定・結果以外のドロップダウン定義）
    /// </summary>
    public static class IndividualDropDownItem {
        /// <summary>
        /// 燃料噴射03
        /// </summary>
        public static class EngineInjection03 {
            public static class MeasurementTerminal {
                /// <summary>NG</summary>
                public const string TERMINAL1 = "1";
                /// <summary>OK</summary>
                public const string TERMINAL2 = "2";
                /// <summary>NG表示文字列</summary>
                private const string TERMINAL1_DISPLAY_STRING = "1号機";
                /// <summary>OK表示文字列</summary>
                private const string TERMINAL2_DISPLAY_STRING = "2号機";
                /// <summary>検査結果リスト</summary>
                public static ListItem[] GetList() => new ListItem[] {
                    new ListItem( string.Empty, string.Empty ),
                    new ListItem( TERMINAL1_DISPLAY_STRING, TERMINAL1 ),
                    new ListItem( TERMINAL2_DISPLAY_STRING, TERMINAL2 )
                };
            }
        }
    }
}