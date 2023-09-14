using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace TRC_W_PWT_ProductView.Defines.ListDefine {

    /// <summary>
    /// 部品検索対象
    /// </summary>
    internal static class PartsSearchKind {

        /// <summary>1:DPF</summary>
        public const string Dpf = "1";
        /// <summary>2:DOC</summary>
        public const string Doc = "3";

        /// <summary>1:ATU部品種別:DPF</summary>
        private static string DPF = "DPF";
        /// <summary>2:ATU部品種別:DOC</summary>
        private static string DOC = "DOC";

        /// <summary>
        /// ATUリスト
        /// </summary>
        private static readonly ListItem[] PartsSearchKindList_Atu = new ListItem[]
        {
            new ListItem( DPF,Dpf ),
            new ListItem( DOC,Doc ),
        };

        /// <summary>
        /// コードを取得する
        /// </summary>
        /// <param name="value">チェック値</param>
        /// <returns>コード</returns>
        internal static string GetCode( string value ) {
            switch ( value ) {
            case Dpf:
            case Doc:
                return value;
            default:
                //上記以外はすべて対象外扱い
                return "";
            }
        }

        /// <summary>
        /// リストを取得する
        /// </summary>
        /// <param name="addBlank">空白行追加有無</param>
        /// <returns>リストアイテム配列</returns>
        internal static ListItem[] GetList( string target, bool addBlank = false ) {

            ListItem[] resultArr = null;

            if ( PartsSearchTarget.Atu == target ) {
                resultArr = Common.DataUtils.InsertBlankItem( PartsSearchKindList_Atu );
            }

            return resultArr;

        }
    }
}
