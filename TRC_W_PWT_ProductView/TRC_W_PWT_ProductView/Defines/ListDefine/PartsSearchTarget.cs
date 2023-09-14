using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace TRC_W_PWT_ProductView.Defines.ListDefine {

    /// <summary>
    /// 部品検索対象
    /// </summary>
    internal static class PartsSearchTarget {

        /// <summary>1:ATU</summary>
        public const string Atu = "01";

        /// <summary>1:ATU</summary>>
        private static string NAME_ATU = "ATU";

        /// <summary>
        /// 部品検索対象リスト
        /// </summary>
        private static readonly ListItem[] PartsSearchTargetList = new ListItem[]
        {
            new ListItem( NAME_ATU, Atu )
        };

        /// <summary>
        /// 定義済みかチェックする
        /// </summary>
        /// <param name="code">コード</param>
        /// <returns>true:定義済 false:未定義</returns>
        internal static bool IsDefine( string code ) {
            if ( null == GetCode( code ) ) {
                return false;
            } else {
                return true;
            }
        }

        /// <summary>
        /// コードを取得する
        /// </summary>
        /// <param name="value">チェック値</param>
        /// <returns>コード</returns>
        internal static string GetCode( string value ) {
            switch ( value ) {
            case Atu:
                return value;
            default:
                //上記以外はすべて対象外扱い
                return "";
            }
        }

        /// <summary>
        /// 名称を取得する
        /// </summary>
        /// <param name="code">コード</param>
        /// <returns>コード名</returns>
        internal static string GetName( string code ) {
            switch ( GetCode( code ) ) {
            case Atu:
                return NAME_ATU;
            default:
                return "";
            }
        }

        /// <summary>
        /// リストを取得する
        /// </summary>
        /// <param name="addBlank">空白行追加有無</param>
        /// <returns>リストアイテム配列</returns>
        internal static ListItem[] GetList( bool addBlank = false ) {

            ListItem[] resultArr = null;
            if ( true == addBlank ) {
                resultArr = Common.DataUtils.InsertBlankItem( PartsSearchTargetList );
            } else {
                resultArr = PartsSearchTargetList;
            }

            return resultArr;

        }
    }
}
