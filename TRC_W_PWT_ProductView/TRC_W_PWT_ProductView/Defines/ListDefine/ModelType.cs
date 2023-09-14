using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace TRC_W_PWT_ProductView.Defines.ListDefine {

    /// <summary>
    /// 型式種別
    /// </summary>
    internal static class ModelType {

        /// <summary>0:生産型式</summary>
        public const string Product = "0";
        /// <summary>1:販売型式</summary>
        public const string Sales = "1";

        /// <summary>0:生産型式</summary>
        private static string NAME_PRODUCT = "生産型式";
        /// <summary>1:販売型式</summary>
        private static string NAME_SALES = "販売型式";

        /// <summary>
        /// 型式種別リスト
        /// </summary>
        private static readonly ListItem[] ModelTypeList = new ListItem[]
        {
            new ListItem( NAME_PRODUCT, Product ),
            new ListItem( NAME_SALES, Sales ),
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
            case Product:
            case Sales:
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
            case Product:
                return NAME_PRODUCT;
            case Sales:
                return NAME_SALES;
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
                resultArr = Common.DataUtils.InsertBlankItem( ModelTypeList );
            } else {
                resultArr = ModelTypeList;
            }

            return resultArr;

        }
    }
}
