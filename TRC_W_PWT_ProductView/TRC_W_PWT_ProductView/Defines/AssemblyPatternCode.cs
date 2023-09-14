using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KTFramework.Common;

namespace TRC_W_PWT_ProductView.Defines {
    /// <summary>
    /// 組立パターンコード
    /// </summary>
    public class AssemblyPatternCode {
        /// <summary>未定義</summary>
        public const string Undefined = "-1";
        /// <summary>14 :07搭載エンジン</summary>
        public const string InstalledEngine07 = "14 ";
        /// <summary>15 :03搭載エンジン</summary>
        public const string InstalledEngine03 = "15 ";
        /// <summary>18 :07OEMエンジン</summary>
        public const string OemEngine07 = "18 ";
        /// <summary>19 :03OEMエンジン</summary>
        public const string OemEngine03 = "19 ";
        /// <summary>21 :CKDエンジン</summary>
        public const string CkdEngine = "21 ";
        /// <summary>32 :トラクタ</summary>
        public const string Tractor = "32 ";
        /// <summary>41 :CKDトラクタ</summary>
        public const string CkdTractor = "41 ";
        /// <summary>70 :内作ロータリ総称</summary>
        public const string InhouseProductedRotaryGeneral = "70 ";
        /// <summary>71 :内作ロータリ</summary>
        public const string InhouseProductedRotary = "71 ";
        /// <summary>711:内作ロータリ1</summary>
        public const string InhouseProductedRotary1 = "711";
        /// <summary>712:内作ロータリ2</summary>
        public const string InhouseProductedRotary2 = "712";
        /// <summary>713:内作ロータリ3</summary>
        public const string InhouseProductedRotary3 = "713";
        /// <summary>715:内作ロータリ5</summary>
        public const string InhouseProductedRotary5 = "715";
        /// <summary>77 :外作</summary>
        public const string ExternalProductions = "77 ";
        /// <summary>771:ロータリ</summary>
        public const string Rotary = "771";
        /// <summary>773:ユニット</summary>
        public const string Unit = "773";
        /// <summary>774:タイヤ</summary>
        public const string Wheel = "774";
        /// <summary>775:出荷部品</summary>
        public const string BundledItems = "775";
        /// <summary>776:その他</summary>
        public const string Imple = "776";

        /// <summary>不明</summary>
        private const string NAME_UNDEFINED = "不明";
        /// <summary>14 :07搭載エンジン</summary>
        private const string NAME_INSTALLED_ENGINE_07 = "07搭載エンジン";
        /// <summary>15 :03搭載エンジン</summary>
        private const string NAME_INSTALLED_ENGINE_03 = "03搭載エンジン";
        /// <summary>18 :07OEMエンジン</summary>
        private const string NAME_OEM_ENGINE_07 = "07OEMエンジン";
        /// <summary>19 :03OEMエンジン</summary>
        private const string NAME_OEM_ENGINE_03 = "03OEMエンジン";
        /// <summary>21 :CKDエンジン</summary>
        private const string NAME_CKD_ENGINE = "CKDエンジン";
        /// <summary>32 :トラクタ</summary>
        private const string NAME_TRACTOR = "トラクタ";
        /// <summary>41 :CKDトラクタ</summary>
        private const string NAME_CKD_TRACTOR = "CKDトラクタ";
        /// <summary>70 :内作ロータリ総称</summary>
        private const string NAME_INHOUSE_PRODUCTED_ROTARY_GENERAL = "内作ロータリ総称";
        /// <summary>71 :内作ロータリ</summary>
        private const string NAME_INHOUSE_PRODUCTED_ROTARY = "内作ロータリ";
        /// <summary>711:内作ロータリ1</summary>
        private const string NAME_INHOUSE_PRODUCTED_ROTARY1 = "内作ロータリ1";
        /// <summary>712:内作ロータリ2</summary>
        private const string NAME_INHOUSE_PRODUCTED_ROTARY2 = "内作ロータリ2";
        /// <summary>713:内作ロータリ3</summary>
        private const string NAME_INHOUSE_PRODUCTED_ROTARY3 = "内作ロータリ3";
        /// <summary>715:内作ロータリ5</summary>
        private const string NAME_INHOUSE_PRODUCTED_ROTARY5 = "内作ロータリ5";
        /// <summary>77 :外作</summary>
        private const string NAME_EXTERNAL_PRODUCTIONS = "外作";
        /// <summary>771:ロータリ</summary>
        private const string NAME_ROTARY = "ロータリ";
        /// <summary>773:ユニット</summary>
        private const string NAME_UNIT = "ユニット";
        /// <summary>774:タイヤ</summary>
        private const string NAME_WHEEL = "タイヤ";
        /// <summary>775:出荷部品</summary>
        private const string NAME_BUNDLED_ITEMS = "出荷部品";
        /// <summary>776:その他</summary>
        private const string NAME_IMPLE = "その他外作インプル";

        /// <summary>
        /// 定義済みかチェックする
        /// </summary>
        /// <param name="patternCd">組立パターン</param>
        /// <param name="patternCdSub">組立パターンサブ</param>
        /// <returns>true:定義済 false:未定義</returns>
        public static bool IsDefine( string patternCd, string patternCdSub ) {
            if ( Undefined == GetCode( patternCd, patternCdSub ) ) {
                return false;
            } else {
                return true;
            }
        }

        /// <summary>
        /// コードを取得する
        /// </summary>
        /// <param name="patternCd">組立パターン</param>
        /// <param name="patternCdSub">組立パターンサブ</param>
        /// <returns>コード</returns>
        public static string GetCode( string patternCd, string patternCdSub ) {
            string value = patternCd + patternCdSub;
            switch ( value ) {
            case InstalledEngine07:
            case InstalledEngine03:
            case OemEngine07:
            case OemEngine03:
            case CkdEngine:
            case Tractor:
            case CkdTractor:
            case InhouseProductedRotaryGeneral:
            case InhouseProductedRotary:
            case InhouseProductedRotary1:
            case InhouseProductedRotary2:
            case InhouseProductedRotary3:
            case InhouseProductedRotary5:
            case ExternalProductions:
            case Rotary:
            case Unit:
            case Wheel:
            case BundledItems:
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
        /// <param name="patternCd">組立パターン</param>
        /// <param name="patternCdSub">組立パターンサブ</param>
        /// <returns>コード名</returns>
        public static string GetName( string patternCd, string patternCdSub ) {
            return GetName( GetCode( patternCd, patternCdSub ) );
        }

        /// <summary>
        /// 名称を取得する
        /// </summary>
        /// <param name="code">コード</param>
        /// <returns>コード名</returns>
        public static string GetName( string code ) {
            switch ( code ) {
            case InstalledEngine07:
                return NAME_INSTALLED_ENGINE_07;
            case InstalledEngine03:
                return NAME_INSTALLED_ENGINE_03;
            case OemEngine07:
                return NAME_OEM_ENGINE_07;
            case OemEngine03:
                return NAME_OEM_ENGINE_03;
            case CkdEngine:
                return NAME_CKD_ENGINE;
            case Tractor:
                return NAME_TRACTOR;
            case CkdTractor:
                return NAME_CKD_TRACTOR;
            case InhouseProductedRotaryGeneral:
                return NAME_INHOUSE_PRODUCTED_ROTARY_GENERAL;
            case InhouseProductedRotary:
                return NAME_INHOUSE_PRODUCTED_ROTARY;
            case InhouseProductedRotary1:
                return NAME_INHOUSE_PRODUCTED_ROTARY1;
            case InhouseProductedRotary2:
                return NAME_INHOUSE_PRODUCTED_ROTARY2;
            case InhouseProductedRotary3:
                return NAME_INHOUSE_PRODUCTED_ROTARY3;
            case InhouseProductedRotary5:
                return NAME_INHOUSE_PRODUCTED_ROTARY5;
            case ExternalProductions:
                return NAME_EXTERNAL_PRODUCTIONS;
            case Rotary:
                return NAME_ROTARY;
            case Unit:
                return NAME_UNIT;
            case Wheel:
                return NAME_WHEEL;
            case BundledItems:
                return NAME_BUNDLED_ITEMS;
            case Imple:
                return NAME_IMPLE;
            default:
                return NAME_UNDEFINED;
            }
        }

        /// <summary>
        /// 組立パターンから総称パターンを取得する
        /// </summary>
        /// <param name="patternCd">組立パターン</param>
        /// <param name="patternCdSub">組立パターンサブ</param>
        /// <returns>総称パターン</returns>
        public static string GetGeneralPatternCd( string patternCd, string patternCdSub ) {
            return GetGeneralPatternCd( GetCode( patternCd, patternCdSub ) );
        }

        /// <summary>
        /// 組立パターンから総称パターンを取得する
        /// </summary>
        /// <param name="assemblyPatternCd">組立パターン+組立パターンサブ</param>
        /// <returns>総称パターン</returns>
        public static string GetGeneralPatternCd( string assemblyPatternCd ) {
            switch ( assemblyPatternCd ) {
            case InstalledEngine03:
            case InstalledEngine07:
            case OemEngine03:
            case OemEngine07:
                return GeneralPatternCode.Engine;
            case CkdEngine:
                return GeneralPatternCode.CkdEngine;
            case Tractor:
                return GeneralPatternCode.Tractor;
            case CkdTractor:
                return GeneralPatternCode.CkdTractor;
            case InhouseProductedRotaryGeneral:
            case InhouseProductedRotary:
            case InhouseProductedRotary1:
            case InhouseProductedRotary2:
            case InhouseProductedRotary3:
            case InhouseProductedRotary5:
                return GeneralPatternCode.Rotary;
            case ExternalProductions:
            case Rotary:
            case Unit:
            case Wheel:
            case BundledItems:
            case Imple:
                return GeneralPatternCode.Imple;
            default:
                return GeneralPatternCode.Undefined;
            }
        }
    }
}
