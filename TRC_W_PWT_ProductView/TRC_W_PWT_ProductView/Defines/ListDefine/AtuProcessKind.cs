using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace TRC_W_PWT_ProductView.Defines.ListDefine {

    /// <summary>
    /// ATU工程区分
    /// </summary>
    internal static class AtuProcessKind {

        /// <summary>ATU工程区分:ATU機番管理</summary>
        public const string ATU_PARTS_SERIAL = "01";
        /// <summary>ATU工程区分:トルク締付履歴</summary>
        public const string ATU_TORQUE_TIGHTENING_RECORD = "02";
        /// <summary>ATU工程区分:リーク計測結果</summary>
        public const string ATU_LEAK_MEASURE_RESULT = "03";

        /// <summary>ATU工程区分:ATU機番管理</summary>
        public static string NAME_ATU_PARTS_SERIAL = "ATU機番管理";
        /// <summary>ATU工程区分:トルク締付履歴</summary>
        public static string NAME_ATU_TORQUE_TIGHTENING_RECORD = "トルク締付履歴";
        /// <summary>ATU工程区分:リーク計測結果</summary>
        public static string NAME_ATU_LEAK_MEASURE_RESULT = "リーク計測結果";
        
        /// <summary>
        /// 検索区分リスト
        /// </summary>
        private static readonly ListItem[] AtuProcessKindList = new ListItem[]
        {
            new ListItem( NAME_ATU_PARTS_SERIAL, ATU_PARTS_SERIAL),
            new ListItem( NAME_ATU_TORQUE_TIGHTENING_RECORD, ATU_TORQUE_TIGHTENING_RECORD ),
            new ListItem( NAME_ATU_LEAK_MEASURE_RESULT, ATU_LEAK_MEASURE_RESULT ),
        };

        /// <summary>
        /// リストを取得する
        /// </summary>
        /// <param name="addBlank">空白行追加有無</param>
        /// <returns>リストアイテム配列</returns>
        internal static ListItem[] GetList( bool addBlank = true ) {

            ListItem[] resultArr = null;
            if ( true == addBlank ) {
                resultArr = Common.DataUtils.InsertBlankItem( AtuProcessKindList );
            } else {
                resultArr = AtuProcessKindList;
            }

            return resultArr;

        }


    }
}