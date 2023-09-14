using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRC_W_PWT_ProductView.Defines {
    /// <summary>
    /// 総称パターンコード
    /// </summary>
    public class GeneralPatternCode {
        /// <summary>不明</summary>
        public const string Undefined = "-1";
        /// <summary>10:エンジン</summary>
        public const string Engine = "10";
        /// <summary>20:CKDエンジン</summary>
        public const string CkdEngine = "20";
        /// <summary>30:トラクタ</summary>
        public const string Tractor = "30";
        /// <summary>40:CKDトラクタ</summary>
        public const string CkdTractor = "40";
        /// <summary>70:内作ロータリ</summary>
        public const string Rotary = "70";
        /// <summary>76:その他インプル</summary>
        public const string Imple = "76";

        /// <summary>不明</summary>
        private const string NAME_UNDEFINED = "不明";
        /// <summary>10:エンジン</summary>
        private const string NAME_ENGINE = "エンジン";
        /// <summary>20:CKDエンジン</summary>
        private const string NAME_CKD_ENGINE = "CKDエンジン";
        /// <summary>30:トラクタ</summary>
        private const string NAME_TRACTOR = "トラクタ";
        /// <summary>40:CKDトラクタ</summary>
        private const string NAME_CKD_TRACTOR = "CKDトラクタ";
        /// <summary>70:内作ロータリ</summary>
        private const string NAME_ROTARY = "内作ロータリ";
        /// <summary>76:その他インプル</summary>
        private const string NAME_IMPLE = "その他インプル";

        /// <summary>
        /// 定義済みかチェックする
        /// </summary>
        /// <param name="code">コード</param>
        /// <returns>true:定義済 false:未定義</returns>
        public static bool IsDefine( string code ) {
            if ( Undefined == GetCode( code ) ) {
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
        public static string GetCode( string value ) {
            switch ( value ) {
            case Engine:
            case CkdEngine:
            case Tractor:
            case CkdTractor:
            case Rotary:
            case Imple:
                return value;
            default:
                //上記以外はすべて不明扱い
                return Undefined;
            }
        }

        /// <summary>
        /// 名称を取得する
        /// </summary>
        /// <param name="code">コード</param>
        /// <returns>コード名</returns>
        public static string GetName( string code ) {
            switch ( GetCode( code ) ) {
            case Engine:
                return NAME_ENGINE;
            case CkdEngine:
                return NAME_CKD_ENGINE;
            case Tractor:
                return NAME_TRACTOR;
            case CkdTractor:
                return NAME_CKD_TRACTOR;
            case Rotary:
                return NAME_ROTARY;
            case Imple:
                return NAME_IMPLE;
            default:
                return NAME_UNDEFINED;
            }
        }
    }
}
