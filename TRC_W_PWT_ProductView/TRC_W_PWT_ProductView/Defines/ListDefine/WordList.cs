using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace TRC_W_PWT_ProductView.Defines.ListDefine {

    /// <summary>
    /// LIST固定文言定義クラス
    /// </summary>
    internal static class WordList {
        /// <summary>
        /// 構造体
        /// </summary>
        internal struct ST_WORD_LIST {
            /// <summary>
            /// 表示テキスト
            /// </summary>
            public string text;
            /// <summary>
            /// 値(コード)
            /// </summary>
            public string value;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="value"></param>
            /// <param name="text"></param>
            public ST_WORD_LIST( string value, string text ) {
                this.value = value;
                this.text = text;
            }
        }

        /// <summary>
        /// 対象外リスト(エンジン)
        /// </summary>
        internal static class MainteNAListEngine {
            /// <summary>01:排ガス測定エンジン抜取のため</summary>
            public static readonly ST_WORD_LIST ENGINE_01 = new ST_WORD_LIST( "01", "排ガス測定エンジン抜取のため" );
            /// <summary>02:再調整は行わないため、NG搬出</summary>
            public static readonly ST_WORD_LIST ENGINE_02 = new ST_WORD_LIST( "02", "検査課の調整値でエンジン課ETラインで運転確認し、再調整は行わないため、NG搬出する" );
            /// <summary>03:無塗装単体出荷のため</summary>
            public static readonly ST_WORD_LIST ENGINE_03 = new ST_WORD_LIST( "03", "無塗装単体出荷のため" );
            /// <summary>04:登録間違えのため</summary>
            public static readonly ST_WORD_LIST ENGINE_04 = new ST_WORD_LIST( "04", "登録間違えのため" );
        }
        /// <summary>
        /// 対象外リスト(トラクタ)
        /// </summary>
        internal static class MainteNAListToractor {
        }

        /// <summary>
        /// (共通)
        /// </summary>
        internal static class MainteCommon {
            /// <summary>99:その他</summary>
            public static readonly ST_WORD_LIST COM_99 = new ST_WORD_LIST( "COM001", "その他" );
        }

        /// <summary>
        /// エンジンリスト
        /// </summary>
        internal static ListItem[] EngineList = new ListItem[] {
            new ListItem( "", "" ),
            new ListItem( MainteNAListEngine.ENGINE_01.text, MainteNAListEngine.ENGINE_01.value ),
            new ListItem( MainteNAListEngine.ENGINE_02.text, MainteNAListEngine.ENGINE_02.value ),
            new ListItem( MainteNAListEngine.ENGINE_03.text, MainteNAListEngine.ENGINE_03.value ),
            new ListItem( MainteNAListEngine.ENGINE_04.text, MainteNAListEngine.ENGINE_04.value ),
            new ListItem( MainteCommon.COM_99.text, MainteCommon.COM_99.value ),
        };

        /// <summary>
        /// トラクタリスト
        /// </summary>
        internal static ListItem[] ToractorList = new ListItem[] {
            new ListItem( "", "" ),
            new ListItem( MainteCommon.COM_99.text, MainteCommon.COM_99.value ),
        };
    }
}
