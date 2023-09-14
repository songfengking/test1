using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using KTFramework.Common;

namespace TRC_W_PWT_ProductView.Defines.ListDefine {

    /// <summary>
    /// 日付区分
    /// </summary>
    internal static class DateKind {

        /// <summary>00:製品</summary>
        private const string PRODUCT = "00";
        /// <summary>01:部品</summary>
        private const string PARTS = "01";
        /// <summary>02:工程</summary>
        private const string PROCESS = "02";
        /// <summary>10:部品検索</summary>
        private const string PARTS_SEARCH = "10";

        /// <summary>
        /// 加工部品コード
        /// </summary>
        /// <remarks>
        /// GroupCd = 2(部品区分)のみ
        /// </remarks>
        private static readonly string[] PROCESSING_PARTS_CLASS_CD = new string[] {
            //クランクケース
            "06",
            //シリンダヘッド
            "09",
            //クランクシャフト
            "10",
        };

        /// <summary>
        /// 日付区分リストアイテム構造体
        /// </summary>
        internal struct ST_DATE_KIND {
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
            public ST_DATE_KIND( string value, string text ) {
                this.value = value;
                this.text = text;
            }
        }
        
        /// <summary>
        /// 製品小区分
        /// </summary>
        internal static class ProductClass {
            /// <summary>0001:完成予定（生産中）</summary>
            public static readonly ST_DATE_KIND ToBeComplete = new ST_DATE_KIND( PRODUCT + "01", "完成予定（生産中）" );
            /// <summary>0002:完成（出荷を含む）</summary>
            public static readonly ST_DATE_KIND Completed = new ST_DATE_KIND( PRODUCT + "02", "完成（出荷を含む）" );
            /// <summary>0003:出荷</summary>
            public static readonly ST_DATE_KIND Shipped = new ST_DATE_KIND ( PRODUCT + "03", "出荷" );
            /// <summary>0004:在庫（完成予定＋完成－出荷）</summary>
            public static readonly ST_DATE_KIND StockOnly = new ST_DATE_KIND( PRODUCT + "04", "在庫" );

        }
        /// <summary>
        /// 部品小区分
        /// </summary>
        internal static class PartsClass {
            /// <summary>0101:加工</summary>
            public static readonly ST_DATE_KIND Processing = new ST_DATE_KIND( PARTS + "01", "加工" );
            /// <summary>0102:組付</summary>
            public static readonly ST_DATE_KIND Installing =new ST_DATE_KIND( PARTS + "02", "組付" );
        }
        /// <summary>
        /// 工程小区分
        /// </summary>
        internal static class ProcessClass {
            /// <summary>0201:検査・計測</summary>
            public static readonly ST_DATE_KIND InspectionMeasuring = new ST_DATE_KIND( PROCESS + "01", "検査・計測" );
        }

        /// <summary>
        /// 部品検索ATU小区分
        /// </summary>
        internal static class AtuSearchClass {
            /// <summary>1001:生産指示日</summary>
            public static readonly ST_DATE_KIND ProductInst = new ST_DATE_KIND( PARTS_SEARCH + "01", "生産指示日" );
            /// <summary>1002:生産指示日</summary>
            public static readonly ST_DATE_KIND Throw = new ST_DATE_KIND( PARTS_SEARCH + "02", "投入日" );
            /// <summary>1003:完成日</summary>
            public static readonly ST_DATE_KIND Completed = new ST_DATE_KIND( PARTS_SEARCH + "03", "完成日" );
        }

        /// <summary>
        /// トラクタ生産型式検索リスト
        /// </summary>
        private static readonly ListItem[] TractorList = new ListItem[] {
            new ListItem( ProductClass.ToBeComplete.text, ProductClass.ToBeComplete.value ),
            new ListItem( ProductClass.Completed.text, ProductClass.Completed.value ),
            new ListItem( ProductClass.Shipped.text, ProductClass.Shipped.value ),
            new ListItem( ProductClass.StockOnly.text, ProductClass.StockOnly.value ),
        };

        /// <summary>
        /// エンジン生産型式検索リスト
        /// </summary>
        private static readonly ListItem[] EngineList = new ListItem[] {
            new ListItem( ProductClass.ToBeComplete.text, ProductClass.ToBeComplete.value ),
            new ListItem( ProductClass.Completed.text, ProductClass.Completed.value ),
            new ListItem( ProductClass.Shipped.text, ProductClass.Shipped.value ),
            new ListItem( ProductClass.StockOnly.text, ProductClass.StockOnly.value ),
        };

        /// <summary>
        /// ロータリー生産型式検索リスト
        /// </summary>
        private static readonly ListItem[] RotaryList = new ListItem[] {
            new ListItem( ProductClass.Completed.text, ProductClass.Completed.value ),
            new ListItem( ProductClass.Shipped.text, ProductClass.Shipped.value ),
        };

        /// <summary>
        /// (生産型式)工程区分検索リスト
        /// </summary>
        private static readonly ListItem[] ProductModelProcessList = new ListItem[] {
            new ListItem( ProcessClass.InspectionMeasuring.text, ProcessClass.InspectionMeasuring.value ),
        };

        /// <summary>
        /// (生産型式)部品区分(CC/CYH/CS)検索リスト 加工あり
        /// </summary>
        private static readonly ListItem[] ProductModelProcessingPartsList = new ListItem[] {
            new ListItem( PartsClass.Processing.text, PartsClass.Processing.value ),
            new ListItem( PartsClass.Installing.text, PartsClass.Installing.value ),
        };

        /// <summary>
        /// (生産型式)部品区分(CC/CYH/CS 以外)検索リスト 加工なし
        /// </summary>
        private static readonly ListItem[] ProductModelInstallingPartsList = new ListItem[] {
            new ListItem( PartsClass.Installing.text, PartsClass.Installing.value ),
        };

        /// <summary>
        /// 販売型式検索リスト
        /// </summary>
        private static readonly ListItem[] SalesModelList = new ListItem[] {
            new ListItem( ProductClass.Shipped.text, ProductClass.Shipped.value ),
        };

        /// <summary>
        /// ATU検索リスト
        /// </summary>
        private static readonly ListItem[] PartsSearchKindList = new ListItem[] {
            new ListItem( AtuSearchClass.ProductInst.text, AtuSearchClass.ProductInst.value ),
            new ListItem( AtuSearchClass.Throw.text, AtuSearchClass.Throw.value ),
            new ListItem( AtuSearchClass.Completed.text, AtuSearchClass.Completed.value ),
        };

        /// <summary>
        /// リストを取得する
        /// </summary>
        /// <param name="addBlank">空白行追加有無</param>
        /// <returns>型式種別リスト</returns>
        internal static ListItem[] GetList( string modelType, string productKind, string groupCd, string classCd, bool addBlank = false ) {

            ListItem[] resultArr = null;
            //ListItem[] baseArr = null;
            ListItem[] appendArr = null;

            //生産型式
            if ( ModelType.Product == modelType ) {

                //完成予定、完成、出荷をセット
                if ( ProductKind.Tractor == productKind ) {
                    resultArr = TractorList;
                } else if ( ProductKind.Engine == productKind ) {
                    resultArr = EngineList;
                } else if ( ProductKind.Rotary == productKind ) {
                    resultArr = RotaryList;
                }                

                //未選択
                if ( true == StringUtils.IsEmpty( groupCd ) ) {
                    //リスト変更なし

                //工程区分
                } else if ( GroupCd.Process == groupCd ) {
                    appendArr = ProductModelProcessList;

                //部品区分
                } else if ( GroupCd.Parts == groupCd ) {
                    //加工部品
                    if ( 0 <= Array.IndexOf( PROCESSING_PARTS_CLASS_CD, classCd ) ) {
                        appendArr = ProductModelProcessingPartsList;

                    //非加工部品
                    } else {
                        appendArr = ProductModelInstallingPartsList;
                    }
                }

            //販売型式
            } else if ( ModelType.Sales == modelType ) {
                resultArr = SalesModelList;
            }

            //追加リスト要素をコピー
            if ( true == ObjectUtils.IsNotNull( appendArr ) ) {
                Array.Resize( ref resultArr, resultArr.Length + appendArr.Length );
                Array.ConstrainedCopy( appendArr, 0, resultArr, resultArr.Length - appendArr.Length, appendArr.Length );
            }

            //空白行追加
            if ( true == addBlank ) {
                resultArr = Common.DataUtils.InsertBlankItem( resultArr );
            }

            return resultArr;

        }

        /// <summary>
        /// リストを取得する(部品検索用)
        /// </summary>
        /// <param name="addBlank">空白行追加有無</param>
        /// <returns>型式種別リスト</returns>
        internal static ListItem[] GetList( string partsSearchTarget, bool addBlank = false ) {

            ListItem[] resultArr = null;
            //ListItem[] appendArr = null;

            //ATU部品検索
            if ( PartsSearchTarget.Atu == partsSearchTarget ) {
                resultArr = PartsSearchKindList;
            }

            ////追加リスト要素をコピー
            //if ( true == ObjectUtils.IsNotNull( appendArr ) ) {
            //    Array.Resize( ref resultArr, resultArr.Length + appendArr.Length );
            //    Array.ConstrainedCopy( appendArr, 0, resultArr, resultArr.Length - appendArr.Length, appendArr.Length );
            //}

            //空白行追加
            if ( true == addBlank ) {
                resultArr = Common.DataUtils.InsertBlankItem( resultArr );
            }

            return resultArr;

        }
    }
}
