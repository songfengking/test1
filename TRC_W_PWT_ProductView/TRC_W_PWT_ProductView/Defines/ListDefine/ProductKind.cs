using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace TRC_W_PWT_ProductView.Defines.ListDefine {

    /// <summary>
    /// 製品種別
    /// </summary>
    internal static class ProductKind {

        /// <summary>10:エンジン</summary>
        public const string Engine = "10";
        /// <summary>30:トラクタ</summary>
        public const string Tractor = "30";
        /// <summary>76:ロータリー</summary>
        public const string Rotary = "76";

        /// <summary>10:エンジン</summary>
        private static string NAME_ENGINE = "エンジン";
        /// <summary>30:トラクタ</summary>
        private static string NAME_TRACTOR = "トラクタ";
        /// <summary>76:ロータリー</summary>
        private static string NAME_ROTARY = "ロータリー";

        /// <summary>
        /// 製品種別リスト
        /// </summary>
        private static readonly ListItem[] ProductKindList = new ListItem[]
        {
            new ListItem( NAME_ENGINE, Engine ),
            new ListItem( NAME_TRACTOR, Tractor ),
            new ListItem( NAME_ROTARY, Rotary ),
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
            case Engine:
            case Tractor:
            case Rotary:
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
            case Engine:
                return NAME_ENGINE;
            case Tractor:
                return NAME_TRACTOR;
            case Rotary:
                return NAME_ROTARY;
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
                resultArr = Common.DataUtils.InsertBlankItem( ProductKindList );
            } else {
                resultArr = ProductKindList;
            }

            return resultArr;

        }
    }
}
