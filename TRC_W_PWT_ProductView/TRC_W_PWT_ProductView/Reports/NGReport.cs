using KTFramework.Common;
using KTFramework.Excel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using TRC_W_PWT_ProductView.Common;

namespace TRC_W_PWT_ProductView.Report {
    public class NGReport {

        #region 定数
        /// <summary>
        /// 出力データソース列定義
        /// </summary>
        protected static class OutputSourceColumn {
            // 順序連番
            public const string SHIJI_TUKI_NO = "shijiTsukiNo";
            // 型式名
            public const string MODEL_NAME = "modelNm";
            // 機番
            public const string SERIAL = "serial6";
            // ピストン出代上限
            public const string PISTON_BUMP_UPPER_LIMIT = "pistonBumpUpperLimit";
            // ピストン出代下限
            public const string PISTON_BUMP_LOWER_LIMIT = "pistonBumpLowerLimit";
            // ピストン出代気筒1
            public const string PISTON01_BUMP = "piston01Bump";
            // ピストン出代気筒2
            public const string PISTON02_BUMP = "piston02Bump";
            // ピストン出代気筒3
            public const string PISTON03_BUMP = "piston03Bump";
            // ピストン出代気筒4
            public const string PISTON04_BUMP = "piston04Bump";
            // ガスケット寸法
            public const string GASKET_SIZE = "gasketSize";
            // 選定ガスケット品番
            public const string SELECTED_GASKET_NUM = "selectedGasketNum";
            // 測定号機
            public const string MEASUREMENT_TERMINAL = "measurementTerminal";
        }

        /// <summary>
        /// ガスケットNGチェックシート定義
        /// </summary>
        protected static class GasketNGCheckSheet {
            // 1レコード当りの必要行数
            public const int NUMBER_OF_ROWS_PER_RECORD = 11;
            // 1シート当りの出力レコード数
            public const int NUMBER_OF_OUTPUTABLE_RECORDS_PER_SHEET = 3;
            // エクセル出力位置
            public static class OutputPosition {
                // 順序連番
                public static Point ShijiTukiNo = new Point( 4, 13 );
                // 機番
                public static Point Serial = new Point( 11, 13 );
                // ピストン出代気筒1
                public static Point Piston01Bump = new Point( 6, 15 );
                // ピストン出代気筒2
                public static Point Piston02Bump = new Point( 8, 15 );
                // ピストン出代気筒3
                public static Point Piston03Bump = new Point( 10, 15 );
                // ピストン出代気筒4
                public static Point Piston04Bump = new Point( 12, 15 );
                // ガスケット寸法
                public static Point GasketSize = new Point( 4, 20 );
                // 選定ガスケット部品番号
                public static Point SelectedGasketNumber = new Point( 11, 20 );
                // 号機
                public static Point MeasurementTerminal = new Point( 6, 22 );
            }
        }

        /// <summary>
        /// 出代NG測定定義
        /// </summary>
        protected static class BumpNGMeasurementSheet {
            // 1レコード当りの必要行数
            public const int NUMBER_OF_ROWS_PER_RECORD = 2;
            // 1シート当りの出力レコード数
            public const int NUMBER_OF_OUTPUTABLE_RECORDS_PER_SHEET = 15;
            /// <summary>
            /// エクセル出力位置
            /// </summary>
            public static class OutputPosition {
                // 号機
                public static Point MeasurementTerminal = new Point( 0, 3 );
                // ピストン出代気筒1
                public static Point Piston01Bump = new Point( 2, 3 );
                // ピストン出代気筒2
                public static Point Piston02Bump = new Point( 4, 3 );
                // ピストン出代気筒3
                public static Point Piston03Bump = new Point( 6, 3 );
                // ピストン出代気筒4
                public static Point Piston04Bump = new Point( 8, 3 );
                // 型式名
                public static Point Model = new Point( 10, 3 );
                // 機番
                public static Point Serial = new Point( 10, 4 );
            }
        }

        /// <summary>
        /// テンプレートシートインデックス
        /// </summary>
        protected static class TemplateSheetIndex {
            // ガスケットNGチェックシート一般 シートインデックス
            public const int GASKET_NG_CHECKSHEET_GENERAL_INDEX = 0;
            // ガスケットNGチェックシート E シートインデックス
            public const int GASKET_NG_CHECKSHEET_E3_INDEX = 1;
            // ガスケットNGチェックシート E2 シートインデックス
            public const int GASKET_NG_CHECKSHEET_E32_INDEX = 2;
            // 出代NG測定 シートインデックス
            public const int BUMP_NG_MEASUREMENT_SHEET_INDEX = 3;
        }

        /// <summary>
        /// 測定日出力フォーマット
        /// </summary>
        public const string OUTPUT_FORMAT_MEASURE_DT = "yyyy年 MM月 dd日";
        /// <summary>
        /// 測定日出力位置（ガスケットNGチェックシート）
        /// </summary>
        public readonly Point OUTPUT_POSITION_GASKET_NG_MEASURE_DT = new Point( 10, 2 );
        /// <summary>
        /// 測定日出力位置（出代NG）
        /// </summary>
        public readonly Point OUTPUT_POSITION_BUMP_NG_MEASURE_DT = new Point( 7, 1 );

        #endregion

        #region 構造体

        /// <summary>
        /// テンプレートシート名
        /// </summary>
        protected struct TemplateSheetName {
            /// <summary>
            /// ガスケットNGチェックシート一般 シート名
            /// </summary>
            public string GasketNGChecksheetGeneralName { get; set; }
            /// <summary>
            /// ガスケットNGチェックシート E シート名
            /// </summary>
            public string GasketNGChecksheetE3Name { get; set; }
            /// <summary>
            /// ガスケットNGチェックシート E2 シート名
            /// </summary>
            public string GasketNGChecksheetE32Name { get; set; }
            /// <summary>
            /// 出代NG測定 シート名
            /// </summary>
            public string BumpNGMeasurementSheetName { get; set; }
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 出力データソース
        /// </summary>
        public DataTable OutputSource { get; set; }

        /// <summary>
        /// テンプレートファイルパス
        /// </summary>
        public string TemplateFilePath { get; set; }

        /// <summary>
        /// Excel出力可否
        /// </summary>
        public bool IsOutputable { get; private set; } = false;

        /// <summary>
        /// 測定日
        /// </summary>
        public String MeasureDay { get; set; }

        #endregion

        #region フィールド

        //ロガー定義
        private static readonly Logger _logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );
        //シート名
        private TemplateSheetName _templateSheetName;
        //Excelオブジェクト
        private ExcelUtils _excel;

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NGReport() : this( null, WebAppInstance.GetInstance().Config.ApplicationInfo.reportTemplateBasePath +
            WebAppInstance.GetInstance().Config.ApplicationInfo.repNGReport ) {
            
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputSource">出力データソース</param>
        public NGReport( DataTable outputSource ) : this( outputSource, WebAppInstance.GetInstance().Config.ApplicationInfo.reportTemplateBasePath +
            WebAppInstance.GetInstance().Config.ApplicationInfo.repNGReport ) {

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputSource">出力データソース</param>
        /// <param name="reportingDt">報告日</param>
        public NGReport( DataTable outputSource, DateTime reportingDt ) : this( outputSource, WebAppInstance.GetInstance().Config.ApplicationInfo.reportTemplateBasePath +
            WebAppInstance.GetInstance().Config.ApplicationInfo.repNGReport, reportingDt ) {

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputSource">出力データソース</param>
        /// <param name="templateFilePath">テンプレートファイルパス</param>
        public NGReport( DataTable outputSource, string templateFilePath ) {
            this.OutputSource = outputSource;
            this.TemplateFilePath = templateFilePath;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputSource">出力データソース</param>
        /// <param name="reportingDay">報告日</param>
        /// <param name="templateFilePath">テンプレートファイルパス</param>
        public NGReport( DataTable outputSource, string templateFilePath, DateTime reportingDt ) {
            this.OutputSource = outputSource;
            this.TemplateFilePath = templateFilePath;
            this.MeasureDay = StringUtils.ToString( reportingDt.Date, OUTPUT_FORMAT_MEASURE_DT ).Replace( " 0", "  " );
        }

        /// <summary>
        /// エクセル出力
        /// </summary>
        public void Create( bool removeTemplateSheet = true ) {
            //テンプレートファイルチェック
            if ( string.IsNullOrEmpty( this.TemplateFilePath ) || !File.Exists( this.TemplateFilePath ) ) {
                _logger.Error( $"テンプレートファイルが見つかりません。 ファイルパス：{ this.TemplateFilePath }" );
                throw new FileNotFoundException( $"テンプレートファイルが見つかりません。 ファイルパス：{ this.TemplateFilePath }" );
            }

            //出力データ件数チェック
            if ( this.OutputSource == null || this.OutputSource.Rows.Count == 0 ) {
                _logger.Error( "出力データソースが空です。" );
                throw new InvalidOperationException( "出力データソースが空です。" );
            }

            //出力データソースに列定義が全て含まれているかチェック
            if ( GetConstantValues<string>( typeof( OutputSourceColumn ) )
                    .Except( this.OutputSource.Columns.Cast<DataColumn>().Select( x => x.ColumnName ) ).Any() ) {
                _logger.Error( "Excel出力に必要な列が出力データソースに含まれていません。" );
                throw new InvalidOperationException( "Excel出力に必要な列が出力データソースに含まれていません。" );
            }

            //エクセルオブジェクト生成（xlsフォーマット）
            _excel = new ExcelUtils( TemplateFilePath );

            //テンプレートシート名設定（ファイル削除時に利用）
            SetTemplateFileName();

            //出力印字処理
            if ( CreateGasketNGChecksheetGeneral() |
                 CreateGasketNGChecksheetE3() |
                 CreateGasketNGChecksheetE32() |
                 CreateBumpNGMeasurementSheet() ) {
                
                //テンプレートシート削除
                if ( removeTemplateSheet ) {
                    GetPropertyValues<string>( _templateSheetName, typeof( TemplateSheetName ) ).ForEach( x => {
                        int sheetIndex = GetPrivateFieldValueByReflection<IWorkbook>( _excel, typeof( ExcelUtils ), "_workbook" ).GetSheetIndex( x );
                        GetPrivateFieldValueByReflection<IWorkbook>( _excel, typeof( ExcelUtils ), "_workbook" ).RemoveSheetAt( sheetIndex );
                    } );
                }

                //出力可
                this.IsOutputable = true;
            } else {
                //出力不可
                this.IsOutputable = false;
            }
        }

        /// <summary>
        /// Excelダウンロード処理
        /// </summary>
        /// <param name="httpResponse"></param>
        public void Dowonload( HttpResponse httpResponse ){
            //出力可否チェック
            if( !this.IsOutputable ) {
                throw new InvalidOperationException( "Excel出力を実行できません。" );
            }

            //Excelファイルダウンロード
            try {
                _excel.Download( httpResponse, $"NG報告書_{ DateTime.Now.ToString( "yyyyMMddHHmmss" ) }.xls" );
            } catch ( System.Threading.ThreadAbortException ) {
                //無視
            } catch ( Exception ex ) {
                _logger.Exception( ex );
                throw ex;
            }
        }

        /// <summary>
        /// テンプレートファイル名設定
        /// </summary>
        /// <param name="excel"></param>
        private void SetTemplateFileName() {
            // ガスケットNGチェックシート一般
            _excel.GetSheet( TemplateSheetIndex.GASKET_NG_CHECKSHEET_GENERAL_INDEX );
            _templateSheetName.GasketNGChecksheetGeneralName = GetPrivateFieldValueByReflection<ISheet>( _excel, typeof( ExcelUtils ), "_sheet" ).SheetName;
            // ガスケットNGチェックシートE
            _excel.GetSheet( TemplateSheetIndex.GASKET_NG_CHECKSHEET_E3_INDEX );
            _templateSheetName.GasketNGChecksheetE3Name = GetPrivateFieldValueByReflection<ISheet>( _excel, typeof( ExcelUtils ), "_sheet" ).SheetName;
            // ガスケットNGチェックシートE2
            _excel.GetSheet( TemplateSheetIndex.GASKET_NG_CHECKSHEET_E32_INDEX );
            _templateSheetName.GasketNGChecksheetE32Name = GetPrivateFieldValueByReflection<ISheet>( _excel, typeof( ExcelUtils ), "_sheet" ).SheetName;
            // 出代NG測定
            _excel.GetSheet( TemplateSheetIndex.BUMP_NG_MEASUREMENT_SHEET_INDEX );
            _templateSheetName.BumpNGMeasurementSheetName = GetPrivateFieldValueByReflection<ISheet>( _excel, typeof( ExcelUtils ), "_sheet" ).SheetName;
        }

        /// <summary>
        /// ガスケットNGチェックシート一般
        /// </summary>
        /// <param name="excel"></param>
        private bool CreateGasketNGChecksheetGeneral() {
            //下限：0.5 + 上限：0.6
            var outputTarget = OutputSource.Rows.Cast<DataRow>()
                                                .Where( x => CheckPistonBumpLimit( x, 0.5m, 0.6m ) )
                                                .ToList();
            
            //印字処理
            if ( outputTarget.Count == 0 ) {
                return false;
            } else {
                PrintGasketNGChecksheet( outputTarget, TemplateSheetIndex.GASKET_NG_CHECKSHEET_GENERAL_INDEX );
                return true;
            }
        }

        /// <summary>
        /// ガスケットNGチェックシート　E3
        /// </summary>
        /// <param name="excel"></param>
        private bool CreateGasketNGChecksheetE3() {
            //下限：0.525 + 上限：0.625
            var outputTarget = OutputSource.Rows.Cast<DataRow>()
                                                .Where( x => CheckPistonBumpLimit( x, 0.525m, 0.625m ) )
                                                .ToList();

            //印字処理
            if ( outputTarget.Count == 0 ) {
                return false;
            } else {
                PrintGasketNGChecksheet( outputTarget, TemplateSheetIndex.GASKET_NG_CHECKSHEET_E3_INDEX );
                return true;
            }
        }

        /// <summary>
        /// ガスケットNGチェックシート　E3 (2)
        /// </summary>
        /// <param name="excel"></param>
        private bool CreateGasketNGChecksheetE32() {
            //下限：0.475 + 上限：0.525
            var outputTarget = OutputSource.Rows.Cast<DataRow>()
                                                .Where( x => CheckPistonBumpLimit( x, 0.475m, 0.525m ) )
                                                .ToList();

            //印字処理
            if ( outputTarget.Count == 0 ) {
                return false;
            } else {
                PrintGasketNGChecksheet( outputTarget, TemplateSheetIndex.GASKET_NG_CHECKSHEET_E32_INDEX );
                return true;
            }
        }

        /// <summary>
        /// 出代NG測定
        /// </summary>
        /// <param name="excel"></param>
        private bool CreateBumpNGMeasurementSheet() {
            //上下限その他
            var outputTarget = OutputSource.Rows.Cast<DataRow>()
                                                .Where( x => !CheckPistonBumpLimit( x, 0.5m, 0.6m ) &&
                                                             !CheckPistonBumpLimit( x, 0.525m, 0.625m ) &&
                                                             !CheckPistonBumpLimit( x, 0.475m, 0.525m ) )
                                                .ToList();
            //印字処理
            if ( outputTarget.Count == 0 ) {
                return false;
            } else {
                PrintBumpNGMeasurementSheet( outputTarget, TemplateSheetIndex.BUMP_NG_MEASUREMENT_SHEET_INDEX );
                return true;
            }
        }

        /// <summary>
        /// ピストン出代上下限チェック
        /// </summary>
        /// <param name="row"></param>
        /// <param name="lowerLimit"></param>
        /// <param name="upperLimit"></param>
        /// <returns></returns>
        private bool CheckPistonBumpLimit( DataRow row, decimal lowerLimit, decimal upperLimit ) {
            return NumericUtils.ToDecimal( row[OutputSourceColumn.PISTON_BUMP_LOWER_LIMIT] ) == lowerLimit &&
                   NumericUtils.ToDecimal( row[OutputSourceColumn.PISTON_BUMP_UPPER_LIMIT] ) == upperLimit;
        }

        /// <summary>
        /// ガスケットNGチェックシート印字処理
        /// </summary>
        /// <param name="excel"></param>
        /// <param name="outputTarget"></param>
        /// <param name="templateSheetIndex"></param>
        private void PrintGasketNGChecksheet( IList<DataRow> outputTarget, int templateSheetIndex ) {
            
            //現在の繰り返し回数（シート追加条件用）
            int currentIteration = 0;

            //Excel印字処理
            foreach ( var row in outputTarget ) {
                //テンプレートシートをコピーして、シートを新規追加
                if ( currentIteration % GasketNGCheckSheet.NUMBER_OF_OUTPUTABLE_RECORDS_PER_SHEET == 0 ) {
                    _excel.GetSheet( templateSheetIndex );
                    _excel.CopySheet( templateSheetIndex,
                        $"{ GetPrivateFieldValueByReflection<ISheet>( _excel, typeof( ExcelUtils ), "_sheet" ).SheetName }_{ 1 + currentIteration / GasketNGCheckSheet.NUMBER_OF_OUTPUTABLE_RECORDS_PER_SHEET }" );
                    _excel.GetSheet( GetPrivateFieldValueByReflection<IWorkbook>( _excel, typeof( ExcelUtils ), "_workbook" ).NumberOfSheets - 1 );

                    //測定日
                    WriteDataWithTemplateCellStyle( ref _excel, MeasureDay, OUTPUT_POSITION_GASKET_NG_MEASURE_DT.Y, OUTPUT_POSITION_GASKET_NG_MEASURE_DT.X );
                }

                //行方向オフセット
                int rowOffset = (currentIteration % GasketNGCheckSheet.NUMBER_OF_OUTPUTABLE_RECORDS_PER_SHEET) * GasketNGCheckSheet.NUMBER_OF_ROWS_PER_RECORD;

                //順序連番
                WriteDataWithTemplateCellStyle( ref _excel, row[OutputSourceColumn.SHIJI_TUKI_NO].ToString(),
                    GasketNGCheckSheet.OutputPosition.ShijiTukiNo.Y + rowOffset, GasketNGCheckSheet.OutputPosition.ShijiTukiNo.X );

                //機番
                WriteDataWithTemplateCellStyle( ref _excel, row[OutputSourceColumn.SERIAL].ToString(),
                    GasketNGCheckSheet.OutputPosition.Serial.Y + rowOffset, GasketNGCheckSheet.OutputPosition.Serial.X );

                //ピストン出代1
                WriteDataWithTemplateCellStyle( ref _excel, row[OutputSourceColumn.PISTON01_BUMP].ToString(),
                    GasketNGCheckSheet.OutputPosition.Piston01Bump.Y + rowOffset, GasketNGCheckSheet.OutputPosition.Piston01Bump.X );

                //ピストン出代2
                WriteDataWithTemplateCellStyle( ref _excel, row[OutputSourceColumn.PISTON02_BUMP].ToString(),
                    GasketNGCheckSheet.OutputPosition.Piston02Bump.Y + rowOffset, GasketNGCheckSheet.OutputPosition.Piston02Bump.X );

                //ピストン出代3
                WriteDataWithTemplateCellStyle( ref _excel, row[OutputSourceColumn.PISTON03_BUMP].ToString(),
                    GasketNGCheckSheet.OutputPosition.Piston03Bump.Y + rowOffset, GasketNGCheckSheet.OutputPosition.Piston03Bump.X );

                //ピストン出代4
                WriteDataWithTemplateCellStyle( ref _excel, row[OutputSourceColumn.PISTON04_BUMP].ToString(),
                    GasketNGCheckSheet.OutputPosition.Piston04Bump.Y + rowOffset, GasketNGCheckSheet.OutputPosition.Piston04Bump.X );

                //ガスケット寸法
                WriteDataWithTemplateCellStyle( ref _excel, $"{ row[OutputSourceColumn.GASKET_SIZE].ToString().PadRight( 4 ) }    →     1.  ",
                    GasketNGCheckSheet.OutputPosition.GasketSize.Y + rowOffset, GasketNGCheckSheet.OutputPosition.GasketSize.X );

                //選定ガスケット部品番号
                WriteDataWithTemplateCellStyle( ref _excel, row[OutputSourceColumn.SELECTED_GASKET_NUM].ToString(),
                    GasketNGCheckSheet.OutputPosition.SelectedGasketNumber.Y + rowOffset, GasketNGCheckSheet.OutputPosition.SelectedGasketNumber.X );

                //号機
                WriteDataWithTemplateCellStyle( ref _excel, $"{ row[OutputSourceColumn.MEASUREMENT_TERMINAL] }号機",
                    GasketNGCheckSheet.OutputPosition.MeasurementTerminal.Y + rowOffset, GasketNGCheckSheet.OutputPosition.MeasurementTerminal.X );

                currentIteration++;
            }

        }

        /// <summary>
        /// 出代NG測定印字処理
        /// </summary>
        /// <param name="excel"></param>
        /// <param name="outputTarget"></param>
        /// <param name="templateSheetIndex"></param>
        private void PrintBumpNGMeasurementSheet( IList<DataRow> outputTarget, int templateSheetIndex ) {

            //現在の繰り返し回数（シート追加条件用）
            int currentIteration = 0;

            //Excel印字処理
            foreach ( var row in outputTarget ) {
                //テンプレートシートをコピーして、シートを新規追加
                if ( currentIteration % BumpNGMeasurementSheet.NUMBER_OF_OUTPUTABLE_RECORDS_PER_SHEET == 0 ) {
                    _excel.GetSheet( templateSheetIndex );
                    _excel.CopySheet( templateSheetIndex,
                        $"{ GetPrivateFieldValueByReflection<ISheet>( _excel, typeof( ExcelUtils ), "_sheet" ).SheetName }_{ 1 + currentIteration / BumpNGMeasurementSheet.NUMBER_OF_OUTPUTABLE_RECORDS_PER_SHEET }" );
                    _excel.GetSheet( GetPrivateFieldValueByReflection<IWorkbook>( _excel, typeof( ExcelUtils ), "_workbook" ).NumberOfSheets - 1 );

                    //測定日
                    WriteDataWithTemplateCellStyle( ref _excel, MeasureDay, OUTPUT_POSITION_BUMP_NG_MEASURE_DT.Y, OUTPUT_POSITION_BUMP_NG_MEASURE_DT.X );
                }

                //行方向オフセット
                int rowOffset = (currentIteration % BumpNGMeasurementSheet.NUMBER_OF_OUTPUTABLE_RECORDS_PER_SHEET) * BumpNGMeasurementSheet.NUMBER_OF_ROWS_PER_RECORD;

                //号機
                WriteDataWithTemplateCellStyle( ref _excel, $"{ row[OutputSourceColumn.MEASUREMENT_TERMINAL] }号機",
                    BumpNGMeasurementSheet.OutputPosition.MeasurementTerminal.Y + rowOffset, BumpNGMeasurementSheet.OutputPosition.MeasurementTerminal.X );

                //ピストン出代1
                WriteDataWithTemplateCellStyle( ref _excel, row[OutputSourceColumn.PISTON01_BUMP].ToString(),
                    BumpNGMeasurementSheet.OutputPosition.Piston01Bump.Y + rowOffset, BumpNGMeasurementSheet.OutputPosition.Piston01Bump.X );

                //ピストン出代2
                WriteDataWithTemplateCellStyle( ref _excel, row[OutputSourceColumn.PISTON02_BUMP].ToString(),
                    BumpNGMeasurementSheet.OutputPosition.Piston02Bump.Y + rowOffset, BumpNGMeasurementSheet.OutputPosition.Piston02Bump.X );

                //ピストン出代3
                WriteDataWithTemplateCellStyle( ref _excel, row[OutputSourceColumn.PISTON03_BUMP].ToString(),
                    BumpNGMeasurementSheet.OutputPosition.Piston03Bump.Y + rowOffset, BumpNGMeasurementSheet.OutputPosition.Piston03Bump.X );

                //ピストン出代4
                WriteDataWithTemplateCellStyle( ref _excel, row[OutputSourceColumn.PISTON04_BUMP].ToString(),
                    BumpNGMeasurementSheet.OutputPosition.Piston04Bump.Y + rowOffset, BumpNGMeasurementSheet.OutputPosition.Piston04Bump.X );

                //型式
                WriteDataWithTemplateCellStyle( ref _excel, $"{ row[OutputSourceColumn.MODEL_NAME].ToString().TrimEnd() }",
                    BumpNGMeasurementSheet.OutputPosition.Model.Y + rowOffset, BumpNGMeasurementSheet.OutputPosition.Model.X );
                //機番
                WriteDataWithTemplateCellStyle( ref _excel, $"{ row[OutputSourceColumn.SERIAL] }",
                    BumpNGMeasurementSheet.OutputPosition.Serial.Y + rowOffset, BumpNGMeasurementSheet.OutputPosition.Serial.X );

                currentIteration++;
            }

        }

        /// <summary>
        /// テンプレートのセルスタイルでデータ書き出し
        /// </summary>
        /// <param name="excel">Excelオブジェクト</param>
        /// <param name="target">対象データ</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnIndex">列インデックス</param>
        private void WriteDataWithTemplateCellStyle( ref ExcelUtils excel, object target, int rowIndex, int columnIndex ) {
            excel.WriteData( target, rowIndex, columnIndex,
                GetCellStyle( GetPrivateFieldValueByReflection<ISheet>( excel, typeof( ExcelUtils ), "_sheet" ), rowIndex, columnIndex ) );
        }

        /// <summary>
        /// セルスタイル取得
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnIndex">列インデックス</param>
        /// <returns></returns>
        private ICellStyle GetCellStyle( ISheet sheet, int rowIndex, int columnIndex ) {
            ICell cell = sheet.GetRow( rowIndex ).GetCell( columnIndex );
            return cell.CellStyle;
        } 

        /// <summary>
        /// クラスから定数リスト取得
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        private List<T> GetConstantValues<T>( Type targetType ) {
            FieldInfo[] fields = targetType.GetFields( BindingFlags.Public
                | BindingFlags.Static
                | BindingFlags.FlattenHierarchy );

            return (fields.Where( fi => fi.IsLiteral
                 && !fi.IsInitOnly
                 && fi.FieldType == typeof( T ) ).Select( fi => ( T )fi.GetRawConstantValue() ) ).ToList();
        }

        /// <summary>
        /// クラスからフィールドリスト取得
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        private List<T> GetPropertyValues<T>( object targetInstance, Type targetType ) {
            return targetType.GetProperties( BindingFlags.Instance | BindingFlags.Public )
                             .Where( prop => prop.PropertyType == typeof( T ) )
                             .Select( prop => ( T )prop.GetValue( targetInstance, null ) )
                             .ToList();
        }

        /// <summary>
        /// リフレクションを利用してアクセス（Excelシートオブジェクト取得用）
        /// HACK:privateなフィールドにアクセスしている為、非推奨
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private T GetPrivateFieldValueByReflection<T>( object targetInstance, Type targetType, string fieldName ) {
            return ( T )targetType.GetField( fieldName, 
                BindingFlags.Instance | 
                BindingFlags.Static | 
                BindingFlags.NonPublic ).GetValue( targetInstance );
        }
    }
}