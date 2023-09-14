using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using KTFramework.Common.Dao;
using KTFramework.Dao;
using KTFramework.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Dao.Com;
using TRC_W_PWT_ProductView.Defines;
using System.Linq;
using TRC_W_PWT_ProductView.Defines.ProcessViewDefine;
using TRC_W_PWT_ProductView.Defines.ListDefine;
using TRC_W_PWT_ProductView.Report.Dto;

namespace TRC_W_PWT_ProductView.Dao.Process {
    /// <summary>
    /// エンジン工程検索DAO
    /// </summary>
    public class ProcessSearchDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "ProcessSearch";

        #region 工程検索処理

        /// <summary>
        /// エンジン：噴射時期計測03検索
        /// </summary>
        /// <param name="dicCondition"></param>
        /// <param name="forExcelOutput">詳細情報取得有無</param>
        /// <returns></returns>
        public static DataTable SelectEngineInjection03List( Dictionary<string, object> dicCondition, bool forExcelOutput ) {

            //パラメータ作成
            KTBindParameters param = GenerateCommonParameter( dicCondition );
            param.Add( "dtFrom", GetSearchFromDate( DateUtils.ToDate( dicCondition[SearchConditionDefine.CONDITION_ENGINE_INJECTION_03.DATE_FROM.bindField] ) ) );
            param.Add( "dtTo", GetSearchToDate( DateUtils.ToDate( dicCondition[SearchConditionDefine.CONDITION_ENGINE_INJECTION_03.DATE_TO.bindField] ) ) );
            param.Add( "integratedCd", dicCondition[SearchConditionDefine.CONDITION_ENGINE_INJECTION_03.INTEGRATED_CD.bindField]?.ToString() );
            param.Add( "result", dicCondition[SearchConditionDefine.CONDITION_ENGINE_INJECTION_03.RESULT.bindField]?.ToString() );
            if ( NumericUtils.IsInt( dicCondition[SearchConditionDefine.CONDITION_ENGINE_INJECTION_03.MEASUREMENT_TERMINAL.bindField] ) ) {
                param.Add( "measurementTerminal", NumericUtils.ToInt( dicCondition[SearchConditionDefine.CONDITION_ENGINE_INJECTION_03.MEASUREMENT_TERMINAL.bindField] ) );
            }
            //検索実行
            if ( !forExcelOutput ) {
                return SelectData( GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineInjection03List" ), param );
            } else {
                return SelectData( GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineInjection03ForExcel" ), param );
            }
        }

        /// <summary>
        /// エンジン：噴射時期計測07検索
        /// </summary>
        /// <param name="dicCondition"></param>
        /// <param name="forExcelOutput">詳細情報取得有無</param>
        /// <returns></returns>
        public static DataTable SelectEngineInjection07List( Dictionary<string, object> dicCondition, bool forExcelOutput ) {
            //パラメータ作成
            KTBindParameters param = GenerateCommonParameter( dicCondition );
            param.Add( "dtFrom", GetSearchFromDate( DateUtils.ToDate( dicCondition[SearchConditionDefine.CONDITION_ENGINE_INJECTION_07.DATE_FROM.bindField] ) ) );
            param.Add( "dtTo", GetSearchToDate( DateUtils.ToDate( dicCondition[SearchConditionDefine.CONDITION_ENGINE_INJECTION_07.DATE_TO.bindField] ) ) );
            param.Add( "result", dicCondition[SearchConditionDefine.CONDITION_ENGINE_INJECTION_07.RESULT.bindField]?.ToString() );

            //検索実行
            if ( !forExcelOutput ) {
                return SelectData( GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineInjection07List" ), param );
            } else {
                return SelectData( GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineInjection07ForExcel" ), param );
            }
        }

        /// <summary>
        /// エンジン：フリクション検索
        /// </summary>
        /// <param name="dicCondition"></param>
        /// <param name="forExcelOutput">詳細情報取得有無</param>
        /// <returns></returns>
        public static DataTable SelectEngineFrictionList( Dictionary<string, object> dicCondition, bool forExcelOutput ) {
            //パラメータ作成
            KTBindParameters param = GenerateCommonParameter( dicCondition );
            param.Add( "dtFrom", GetSearchFromDate( DateUtils.ToDate( dicCondition[SearchConditionDefine.CONDITION_ENGINE_FRICTION.DATE_FROM.bindField] ) ) );
            param.Add( "dtTo", GetSearchToDate( DateUtils.ToDate( dicCondition[SearchConditionDefine.CONDITION_ENGINE_FRICTION.DATE_TO.bindField] ) ) );
            param.Add( "result", dicCondition[SearchConditionDefine.CONDITION_ENGINE_FRICTION.RESULT.bindField]?.ToString() );

            //検索実行
            if ( !forExcelOutput ) {
                return SelectData( GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineFrictionList" ), param );
            } else {
                return SelectData( GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineFrictionForExcel" ), param );
            }
        }

        /// <summary>
        /// エンジン：エンジン運転測定03検索
        /// </summary>
        /// <param name="dicCondition"></param>
        /// <param name="forExcelOutput">詳細情報取得有無</param>
        /// <returns></returns>
        public static DataTable SelectEngineTest03List( Dictionary<string, object> dicCondition, bool forExcelOutput ) {

            //パラメータ作成
            KTBindParameters param = GenerateCommonParameter( dicCondition );
            param.Add( "dtFrom", GetSearchFromDate( DateUtils.ToDate( dicCondition[SearchConditionDefine.CONDITION_ENGINE_TEST_03.DATE_FROM.bindField] ) ) );
            param.Add( "dtTo", GetSearchToDate( DateUtils.ToDate( dicCondition[SearchConditionDefine.CONDITION_ENGINE_TEST_03.DATE_TO.bindField] ) ) );
            if ( NumericUtils.IsInt( dicCondition[SearchConditionDefine.CONDITION_ENGINE_TEST_03.RESULT.bindField] ) ) {
                param.Add( "result", NumericUtils.ToInt( dicCondition[SearchConditionDefine.CONDITION_ENGINE_TEST_03.RESULT.bindField] ) );
            }

            //検索実行
            if ( !forExcelOutput ) {
                return SelectData( GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineTest03List" ), param );
            } else {
                return SelectData( GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineTest03ForExcel" ), param );
            }
        }

        /// <summary>
        /// エンジン：エンジン運転測定07検索
        /// </summary>
        /// <param name="dicCondition"></param>
        /// <param name="forExcelOutput">詳細情報取得有無</param>
        /// <returns></returns>
        public static DataTable SelectEngineTest07List( Dictionary<string, object> dicCondition, bool forExcelOutput ) {
            //パラメータ作成
            KTBindParameters param = GenerateCommonParameter( dicCondition );
            param.Add( "dtFrom", GetSearchFromDate( DateUtils.ToDate( dicCondition[SearchConditionDefine.CONDITION_ENGINE_TEST_07.DATE_FROM.bindField] ) ) );
            param.Add( "dtTo", GetSearchToDate( DateUtils.ToDate( dicCondition[SearchConditionDefine.CONDITION_ENGINE_TEST_07.DATE_TO.bindField] ) ) );
            if ( NumericUtils.IsInt( dicCondition[SearchConditionDefine.CONDITION_ENGINE_TEST_07.RESULT.bindField] ) ) {
                param.Add( "result", NumericUtils.ToInt( dicCondition[SearchConditionDefine.CONDITION_ENGINE_TEST_07.RESULT.bindField] ) );
            }

            //検索実行
            if ( !forExcelOutput ) {
                return SelectData( GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineTest07List" ), param );
            } else {
                return SelectData( GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineTest07ForExcel" ), param );
            }
        }

        #endregion

        #region 工程別操作ボタン用検索処理

        /// <summary>
        /// エンジン：噴射時期計測03 NGリスト取得
        /// </summary>
        /// <param name="dicCondition">検索条件</param>
        /// <param name="errorMessageList">エラーメッセージ（前方一致）</param>
        /// <returns></returns>
        public static DataTable SelectEngineInjection03NGList( Dictionary<string, object> dicCondition, IEnumerable<string> errorMessages ) {
            //パラメータ作成
            KTBindParameters param = GenerateCommonParameter( dicCondition );
            param.Add( "dtFrom", GetSearchFromDate( DateUtils.ToDate( dicCondition[SearchConditionDefine.CONDITION_ENGINE_INJECTION_03.DATE_FROM.bindField] ) ) );
            param.Add( "dtTo", GetSearchToDate( DateUtils.ToDate( dicCondition[SearchConditionDefine.CONDITION_ENGINE_INJECTION_03.DATE_TO.bindField] ) ) );
            param.Add( "integratedCd", dicCondition[SearchConditionDefine.CONDITION_ENGINE_INJECTION_03.INTEGRATED_CD.bindField]?.ToString() );
            param.Add( "result", dicCondition[SearchConditionDefine.CONDITION_ENGINE_INJECTION_03.RESULT.bindField]?.ToString() );
            param.Add( "errorMessages", errorMessages );
            if ( NumericUtils.IsInt( dicCondition[SearchConditionDefine.CONDITION_ENGINE_INJECTION_03.MEASUREMENT_TERMINAL.bindField] ) ) {
                param.Add( "measurementTerminal", NumericUtils.ToInt( dicCondition[SearchConditionDefine.CONDITION_ENGINE_INJECTION_03.MEASUREMENT_TERMINAL.bindField] ) );
            }

            //検索実行
            return SelectData( GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineInjection03NGList" ), param );
        }

        /// <summary>
        /// エンジン：噴射時期計測03 詳細データ取得（PDF出力用）
        /// </summary>
        /// <param name="measuringDate"></param>
        /// <param name="modelName"></param>
        /// <param name="serial"></param>
        /// <returns></returns>
        public static EngineInjection03DetailDto SelectEngineInjection03DetailForPdfOutput( DateTime measuringDate, string modelName, string serial ) {
            EngineInjection03DetailDto param = new EngineInjection03DetailDto {
                MeasuringDate = measuringDate,
                ModelName = modelName,
                Serial = serial
            };

            return PicDao.GetInstance().Select<EngineInjection03DetailDto>( GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineInjection03DetailForPdfOutput" ), param ).Single();
        }

        /// <summary>
        /// エンジン：噴射時期計測07 詳細データ取得（PDF出力用）
        /// </summary>
        /// <param name="measuringDate"></param>
        /// <param name="modelName"></param>
        /// <param name="serial"></param>
        /// <returns></returns>
        public static EngineInjection07DetailDto SelectEngineInjection07DetailForPdfOutput( DateTime measuringDate, string modelName, string serial ) {
            EngineInjection07DetailDto param = new EngineInjection07DetailDto {
                MeasuringDate = measuringDate,
                ModelName = modelName,
                Serial = serial
            };

            return PicDao.GetInstance().Select<EngineInjection07DetailDto>( GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineInjection07DetailForPdfOutput" ), param ).Single();
        }

        #endregion

        #region Fetch処理

        /// <summary>
        /// データ検索処理
        /// </summary>
        /// <param name="statementId">ステートメントID</param>
        /// <param name="bindParam">パラメータ</param>
        /// <param name="maxRecordCount">最大出力件数</param>
        /// <returns>出力結果DataTable</returns>
        private static DataTable SelectData( string statementId, KTBindParameters bindParam, int? maxRecordCount = null ) {
            //最大件数指定が無い場合、既定設定を採用
            maxRecordCount = maxRecordCount ?? WebAppInstance.GetInstance().Config.WebCommonInfo.processViewMaxGridViewCount;
            Cursor cursor = PicDao.GetInstance().OpenCursor( statementId, bindParam );
            DataTable resultTable = null;
            try {
                while ( PicDao.GetInstance().Fetch( ref resultTable, ref cursor ) ) {
                    if ( resultTable.Rows.Count >= maxRecordCount ) {
                        break;
                    }
                }
            } finally {
                PicDao.GetInstance().CloseCursor( ref cursor );
            }
            resultTable.AcceptChanges();

            return resultTable;
        }
        #endregion

        #region データ変換

        /// <summary>
        /// 日付検索条件用の値に変換します
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static DateTime? GetSearchFromDate( DateTime dt ) {
            DateTime dtMin = new DateTime( 1900, 1, 1 );
            if ( dtMin < dt ) {
                return dt;
            }
            return null;
        }
        /// <summary>
        /// 日付検索条件用の値に変換します
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static DateTime? GetSearchToDate( DateTime dt ) {
            DateTime dtMin = new DateTime( 1900, 1, 1 );
            if ( dtMin < dt ) {
                return dt.AddDays( 1 ).AddMilliseconds( -1 );
            }
            return null;
        }

        #endregion

        #region 共通検索条件バインドパラメータ作成

        /// <summary>
        /// 共通検索条件のバインドパラメータオブジェクト作成
        /// </summary>
        /// <param name="dicCondition"></param>
        /// <returns></returns>
        private static KTBindParameters GenerateCommonParameter( Dictionary<string, object> dicCondition ) {
            return new KTBindParameters {
                { "modelNm", dicCondition[SearchConditionDefine.CONDITION_COMMON.MODEL_NM.bindField]?.ToString() },
                { "modelCd", DataUtils.GetModelCd(dicCondition[SearchConditionDefine.CONDITION_COMMON.MODEL_CD.bindField]?.ToString()) },
                { "serial6", DataUtils.GetSerial6(dicCondition[SearchConditionDefine.CONDITION_COMMON.SERIAL.bindField]?.ToString()) },
                { "idno", dicCondition[SearchConditionDefine.CONDITION_COMMON.IDNO.bindField]?.ToString() },
                { "pinCd", dicCondition[SearchConditionDefine.CONDITION_COMMON.PIN_CD.bindField]?.ToString() }
            };
        }

        #endregion

    }
}