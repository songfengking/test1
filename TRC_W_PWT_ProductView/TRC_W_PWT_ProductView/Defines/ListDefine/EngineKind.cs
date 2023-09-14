using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace TRC_W_PWT_ProductView.Defines.ListDefine {
    public static class EngineKind {
        /// <summary>03:03エンジン</summary>
        public static readonly string ENGINE_03 = "03";
        /// <summary>07:07エンジン</summary>
        public static readonly string ENGINE_07 = "07";

        /// <summary>03:03エンジン</summary>
        private static readonly string ENGINE_03_DISPLAY_STRING = "03";
        /// <summary>07:07エンジン</summary>
        private static readonly string ENGINE_07_DISPLAY_STRING = "07";

        /// <summary>エンジン種別リスト</summary>
        public static ListItem[] GetList() => new ListItem[] {
            new ListItem( ENGINE_03_DISPLAY_STRING, ENGINE_03 ),
            new ListItem( ENGINE_07_DISPLAY_STRING, ENGINE_07 )
        };
    }
}