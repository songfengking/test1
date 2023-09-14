using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using KTFramework.C1Common.Excel;
using C1.C1Excel;
using KTFramework.Common;
using KTFramework.Excel;
using System.Diagnostics.Contracts;
using System.Linq;
using NPOI.SS.UserModel;

namespace TRC_W_PWT_ProductView.Common {

    /// <summary>
    /// エクセル出力クラス
    /// </summary>
    public static class Excel {

        /// <summary>
        /// EXCEL出力処理
        /// </summary>
        /// <param name="response">HttpResponseインスタンス</param>
        /// <param name="excelName">出力ファイル名</param>
        /// <param name="targetTable">出力データ</param>
        /// <param name="searchConditions">検索条件</param>
        public static void Download( HttpResponse response, string fileName, DataTable target, List<ExcelConditionItem> condition ) {
            //単純帳票初期化
            SimpleExcelHandler excel = new SimpleExcelHandler( fileName, target, condition );

            //作成
            excel.Create();

            //ダウンロード
            excel.Download( response );

            return;
        }

        /// <summary>
        /// EXCEL出力処理(KTExcel版)
        /// </summary>
        /// <param name="response">Httpレスポンス</param>
        /// <param name="fileName">ファイル名</param>
        /// <param name="condition">出力データ絞込条件</param>
        /// <param name="target">出力データ</param>
        public static void Download( HttpResponse response, string fileName, Dictionary<string, string> condition, DataTable target ) {

#if DEBUG
            if ( !Path.GetExtension( fileName ).Equals( ".xls" ) ) {
                throw new Exception( "ファイルの拡張子をxlsに変更してください。" );
            }
#endif

            //Excelオブジェクト
            ExcelUtils excel = new ExcelUtils( ExcelUtils.WorkBookType.hssf );

            //Excelテンプレート
            excel.DefaultTemplate( condition, target.Columns.Cast<DataColumn>().Select( x => x.ColumnName ).ToArray(), target );

            //ダウンロード
            excel.Download( response, fileName );

            return;
        }

        /// <summary>
        /// EXCEL出力処理(KTExcel版) 複数シート出力
        /// </summary>
        /// <param name="response">Httpレスポンス</param>
        /// <param name="fileName">ファイル名（{ファイル名}_検索結果_yyyyMMddHHmm.(xlsx|xls)で出力）</param>
        /// <param name="sheetData">各シートのExcelシート出力データ（順序順にシート作成）</param>
        /// <param name="excel2003Flag">Excel2003形式(.xls)で出力するかどうか</param>
        /// <remarks>
        /// ExcelUtilsが複数シートをテンプレートで作成する事に対応していないので作成処理を記述する
        /// </remarks>
        public static void Download( HttpResponse response, string fileName, List<ExcelSheetData> sheetData, bool excel2003Flag = false ) {
            // ExcelUtils.DefaultTemplateに合わせて設定
            // 検索条件部セル書式設定
            var conditionCellStyle = new CellStyleSetting() {
                fontName = "ＭＳ ゴシック",
                fontSize = 11,
            };
            // 検索結果ヘッダ部セル書式設定
            var headerCellStyle = new CellStyleSetting() {
                fontName = "ＭＳ ゴシック",
                fontSize = 11,
                fontColor = IndexedColors.White.Index,
                fillForegroundColor = IndexedColors.Grey40Percent.Index,
                fillPattern = FillPattern.SolidForeground,
            };
            // 検索結果データ部セル書式設定
            var dataCellStyle = new CellStyleSetting() {
                borderLeft = NPOI.SS.UserModel.BorderStyle.Thin,
                borderBottom = NPOI.SS.UserModel.BorderStyle.Thin,
                borderRight = NPOI.SS.UserModel.BorderStyle.Thin,
                borderTop = NPOI.SS.UserModel.BorderStyle.Thin,
                leftBorderColor = IndexedColors.Black.Index,
                bottomBorderColor = IndexedColors.Black.Index,
                rightBorderColor = IndexedColors.Black.Index,
                topBorderColor = IndexedColors.Black.Index,
                fontName = "ＭＳ ゴシック",
                fontSize = 11,
            };
            // 出力年月日
            var outputTime = DateTime.Now;
            // Excelオブジェクト
            var excel = new ExcelUtils( ( ( excel2003Flag == true ) ? ExcelUtils.WorkBookType.hssf : ExcelUtils.WorkBookType.xssf ) );
            for ( int sheetIndex = 0; sheetIndex < sheetData.Count; sheetIndex++ ) {
                // シートを作成し、作成したシートに移動する
                excel.CreateSheet( sheetData[sheetIndex].SheetName );
                excel.GetSheet( sheetIndex );
                // 出力日を出力
                excel.WriteData( "出力日", 0, 0, new List<CellStyleSetting>() { conditionCellStyle } );
                excel.WriteData( outputTime.ToString( DateUtils.DATE_FORMAT_SECOND ), 0, 1, new List<CellStyleSetting>() { conditionCellStyle } );
                // 検索条件を出力
                excel.WriteData( "【検索条件】", 2, 0, new List<CellStyleSetting>() { conditionCellStyle } );
                for ( int condIndex = 0; condIndex < sheetData[sheetIndex].Condition.Count; condIndex++ ) {
                    excel.WriteData( sheetData[sheetIndex].Condition[condIndex].Condition, 3 + condIndex, 0, new List<CellStyleSetting>() { conditionCellStyle } );
                    excel.WriteData( sheetData[sheetIndex].Condition[condIndex].Value, 3 + condIndex, 1, new List<CellStyleSetting>() { conditionCellStyle } );
                }
                // 検索結果ヘッダ部を出力
                // ヘッダ部出力位置
                var headerRowPos = 3 + sheetData[sheetIndex].Condition.Count + 1;
                for ( int headerIndex = 0; headerIndex < sheetData[sheetIndex].TargetData.Columns.Count; headerIndex++ ) {
                    excel.WriteData( sheetData[sheetIndex].TargetData.Columns[headerIndex].Caption, headerRowPos, headerIndex, new List<CellStyleSetting>() { headerCellStyle } );
                }
                // 検索結果データ部を出力
                // ExcelUtils.WriteDataTableでは罫線が出力されない
                excel.WriteDataTable( sheetData[sheetIndex].TargetData, headerRowPos + 1, 0, true, new List<CellStyleSetting>() { dataCellStyle } );
            }
            // ファイル名作成
            var outputFileName = $"{fileName}_検索結果_{outputTime.ToString( DateUtils.DATE_FORMAT_MINITE_NOSEP )}.{( ( excel2003Flag == true ) ? "xls" : "xlsx" )}";
            // ダウンロード
            excel.Download( response, outputFileName );
        }
    }

    /// <summary>
    /// Excelシート出力データ
    /// </summary>
    public class ExcelSheetData {
        /// <summary>
        /// 検索条件
        /// </summary>
        public List<ExcelConditionItem> Condition { get; set; }
        /// <summary>
        /// 表示データ
        /// </summary>
        public DataTable TargetData { get; set; }
        /// <summary>
        /// シート名
        /// </summary>
        public string SheetName { get; set; }
    }
}