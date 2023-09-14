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

namespace TRC_W_PWT_ProductView.Dao.Process {
    /// <summary>
    /// エンジン工程検索DAO
    /// </summary>
    public class EngineProcessDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "EngineProcess";

        /// <summary>
        /// 検索パラメータクラス
        /// </summary>
        public class SearchParameter {
            /// <summary>[共通]型式/機番</summary>
            public List<SerialParam> paramSerialList = null;
            /// <summary>[共通]生産型式コード(前方一致)</summary>
            public string paramProductModelCd = null;
            /// <summary>部品区分コード[3C精密測定]</summary>
            public string paramPartsCd = null;
            /// <summary>測定日(FROM)</summary>
            public DateTime? paramInspectionDtFrom = null;
            /// <summary>測定日(TO)</summary>
            public DateTime? paramInspectionDtTo = null;
            /// <summary>[共通]生産型式コードリスト(型式名から検索したリスト)</summary>
            public List<string> paramProductModelCdList = null;
        }

        #region 一覧検索DAO
        /// <summary>
        /// 一覧画面エンジン工程(トルク締付)情報検索(TBL_トルク締付履歴)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  paramSerialList         … 型式/機番リスト
        ///  paramProductModelCd     … 生産型式コード(前方一致)
        ///  paramInstallDtFrom      … 測定日(FROM)
        ///  paramInstallDtTo        … 測定日(TO)
        ///  paramProductModelCdList … 生産型式コードリスト
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectTorqueList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //工程検索条件
            bindParam.Add( "paramSerialList", param.paramSerialList );                          //型式/機番リスト
            bindParam.Add( "paramProductModelCd", param.paramProductModelCd );                  //生産型式コード(前方一致)
            if ( null != param.paramInspectionDtFrom ) {
                bindParam.Add( "paramInspectionDtFrom", param.paramInspectionDtFrom.Value );    //測定日(FROM)
            }
            if ( null != param.paramInspectionDtTo ) {
                bindParam.Add( "paramInspectionDtTo", param.paramInspectionDtTo.Value );        //測定日(TO)
            }
            bindParam.Add( "paramProductModelCdList", param.paramProductModelCdList );          //生産型式コードリスト

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineTorqueList" );
            return SelectData( statementId, bindParam, maxRecordCount );
        }

        /// <summary>
        /// 一覧画面エンジン工程(ハーネス検査)情報検索(TBL_ハーネス検査履歴)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  paramSerialList         … 型式/機番リスト
        ///  paramProductModelCd     … 生産型式コード(前方一致)
        ///  paramInstallDtFrom      … 測定日(FROM)
        ///  paramInstallDtTo        … 測定日(TO)
        ///  paramProductModelCdList … 生産型式コードリスト
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectHarnessList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //工程検索条件
            bindParam.Add( "paramSerialList", param.paramSerialList );                          //型式/機番リスト
            bindParam.Add( "paramProductModelCd", param.paramProductModelCd );                  //生産型式コード(前方一致)
            if ( null != param.paramInspectionDtFrom ) {
                bindParam.Add( "paramInspectionDtFrom", param.paramInspectionDtFrom.Value );    //測定日(FROM)
            }
            if ( null != param.paramInspectionDtTo ) {
                bindParam.Add( "paramInspectionDtTo", param.paramInspectionDtTo.Value );        //測定日(TO)
            }
            bindParam.Add( "paramProductModelCdList", param.paramProductModelCdList );          //生産型式コードリスト

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineHarnessList" );
            return SelectData( statementId, bindParam, maxRecordCount );
        }

        /// <summary>
        /// 一覧画面エンジン工程(運転検査(03/07))情報検索(D_HEADER/D_HEADER_07)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  paramSerialList         … 型式/機番リスト
        ///  paramProductModelCd     … 生産型式コード(前方一致)
        ///  paramInstallDtFrom      … 測定日(FROM)
        ///  paramInstallDtTo        … 測定日(TO)
        ///  paramProductModelCdList … 生産型式コードリスト
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectEngineTestList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //工程検索条件
            bindParam.Add( "paramSerialList", param.paramSerialList );                          //型式/機番リスト
            bindParam.Add( "paramProductModelCd", param.paramProductModelCd );                  //生産型式コード(前方一致)
            if ( null != param.paramInspectionDtFrom ) {
                bindParam.Add( "paramInspectionDtFrom", param.paramInspectionDtFrom.Value );    //測定日(FROM)
            }
            if ( null != param.paramInspectionDtTo ) {
                bindParam.Add( "paramInspectionDtTo", param.paramInspectionDtTo.Value );        //測定日(TO)
            }
            bindParam.Add( "paramProductModelCdList", param.paramProductModelCdList );          //生産型式コードリスト

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineTestList" );
            return SelectData( statementId, bindParam, maxRecordCount );
        }

        /// <summary>
        /// 一覧画面エンジン工程(フリクションロス実測)情報検索(TBL_フリクションロス実測7)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  paramSerialList         … 型式/機番リスト
        ///  paramProductModelCd     … 生産型式コード(前方一致)
        ///  paramInstallDtFrom      … 測定日(FROM)
        ///  paramInstallDtTo        … 測定日(TO)
        ///  paramProductModelCdList … 生産型式コードリスト
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectFrictionLossList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //工程検索条件
            bindParam.Add( "paramSerialList", param.paramSerialList );                          //型式/機番リスト
            bindParam.Add( "paramProductModelCd", param.paramProductModelCd );                  //生産型式コード(前方一致)
            if ( null != param.paramInspectionDtFrom ) {
                bindParam.Add( "paramInspectionDtFrom", param.paramInspectionDtFrom.Value );    //測定日(FROM)
            }
            if ( null != param.paramInspectionDtTo ) {
                bindParam.Add( "paramInspectionDtTo", param.paramInspectionDtTo.Value );        //測定日(TO)
            }
            bindParam.Add( "paramProductModelCdList", param.paramProductModelCdList );          //生産型式コードリスト

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineFrictionLossList" );
            return SelectData( statementId, bindParam, maxRecordCount );
        }

        /// <summary>
        /// 一覧画面エンジン工程(シリンダヘッド部品組付履歴)情報検索(TBL_シリンダヘッド部品組付履歴)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  paramSerialList         … 型式/機番リスト
        ///  paramProductModelCd     … 生産型式コード(前方一致)
        ///  paramInstallDtFrom      … 測定日(FROM)
        ///  paramInstallDtTo        … 測定日(TO)
        ///  paramProductModelCdList … 生産型式コードリスト
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectCyhAssemblyList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //工程検索条件
            bindParam.Add( "paramSerialList", param.paramSerialList );                          //型式/機番リスト
            bindParam.Add( "paramProductModelCd", param.paramProductModelCd );                  //生産型式コード(前方一致)
            if ( null != param.paramInspectionDtFrom ) {
                bindParam.Add( "paramInspectionDtFrom", param.paramInspectionDtFrom.Value );    //測定日(FROM)
            }
            if ( null != param.paramInspectionDtTo ) {
                bindParam.Add( "paramInspectionDtTo", param.paramInspectionDtTo.Value );        //測定日(TO)
            }
            bindParam.Add( "paramProductModelCdList", param.paramProductModelCdList );          //生産型式コードリスト

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineCyhAssemblyList" );
            return SelectData( statementId, bindParam, maxRecordCount );
        }

        /// <summary>
        /// 一覧画面エンジン工程(3C精密測定検索)情報検索(TT_SQ_PRECISE_MEASURE_DATA/TT_SQ_3C_DETAIL)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  paramSerialList         … 型式/機番リスト
        ///  paramProductModelCd     … 生産型式コード(前方一致)
        ///  paramPartsCd            … 部品区分コード
        ///  paramInstallDtFrom      … 測定日(FROM)
        ///  paramInstallDtTo        … 測定日(TO)
        ///  paramProductModelCdList … 生産型式コードリスト
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable Select3cInspectionList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //工程検索条件
            bindParam.Add( "paramSerialList", param.paramSerialList );                          //型式/機番リスト
            bindParam.Add( "paramProductModelCd", param.paramProductModelCd );                  //生産型式コード(前方一致)
            bindParam.Add( "paramPartsCd", param.paramPartsCd );                                //部品区分コード
            if ( null != param.paramInspectionDtFrom ) {
                bindParam.Add( "paramInspectionDtFrom", param.paramInspectionDtFrom.Value );    //測定日(FROM)
            }
            if ( null != param.paramInspectionDtTo ) {
                bindParam.Add( "paramInspectionDtTo", param.paramInspectionDtTo.Value );        //測定日(TO)
            }
            bindParam.Add( "paramProductModelCdList", param.paramProductModelCdList );          //生産型式コードリスト

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngine3CInspectionList" );
            return SelectData( statementId, bindParam, maxRecordCount );
        }

        /// <summary>
        /// 一覧画面エンジン工程(燃料噴射時期(03/07))情報検索(TBL噴射計測データ/TBL_07噴射時期計測データ)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  paramSerialList         … 型式/機番リスト
        ///  paramProductModelCd     … 生産型式コード(前方一致)
        ///  paramInstallDtFrom      … 測定日(FROM)
        ///  paramInstallDtTo        … 測定日(TO)
        ///  paramProductModelCdList … 生産型式コードリスト
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectInjectionList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();
            //工程検索条件
            bindParam.Add( "paramSerialList", param.paramSerialList );                          //型式/機番リスト
            bindParam.Add( "paramProductModelCd", param.paramProductModelCd );                  //生産型式コード(前方一致)
            if ( null != param.paramInspectionDtFrom ) {
                bindParam.Add( "paramInspectionDtFrom", param.paramInspectionDtFrom.Value );    //測定日(FROM)
            }
            if ( null != param.paramInspectionDtTo ) {
                bindParam.Add( "paramInspectionDtTo", param.paramInspectionDtTo.Value );        //測定日(TO)
            }
            bindParam.Add( "paramProductModelCdList", param.paramProductModelCdList );          //生産型式コードリスト

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineInjectionList" );
            return SelectData( statementId, bindParam, maxRecordCount );

        }
        /// <summary>
        /// 詳細情報エンジン工程(燃料噴射時期(03))情報検索(TBL噴射計測データ)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  paramList         … 型式/機番リスト
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectInjectionListDetail03( List<SerialParam> paramList, int maxRecordCount = Int32.MaxValue ) {
            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();
            bindParam.Add( "paramList", paramList );    //生産型式、機番

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineInjectionExcelDetail03" );
            return SelectData( statementId, bindParam, maxRecordCount );

        }

        /// <summary>
        /// 詳細情報エンジン工程(燃料噴射時期(07))情報検索(TBL_07噴射時期計測データ)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  paramList         … 型式/機番リスト
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectInjectionListDetail07( List<SerialParam> paramList, int maxRecordCount = Int32.MaxValue ) {
            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();
            bindParam.Add( "paramList", paramList );    //生産型式、機番

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineInjectionExcelDetail07" );
            return SelectData( statementId, bindParam, maxRecordCount );

        }

        /// <summary>
        /// 詳細情報エンジン工程(燃料噴射時期(03))NG情報検索(TBL噴射計測データ)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  paramList         … 型式/機番リスト
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectNgInjectionList( List<SerialParam> paramList, int maxRecordCount = Int32.MaxValue ) {
            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();
            bindParam.Add( "paramList", paramList );    //生産型式、機番

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineInjectionExcelNg" );
            return SelectData( statementId, bindParam, maxRecordCount );

        }

        /// <summary>
        /// 一覧画面エンジン工程(品質画像証跡)情報検索(TT_SQ_CAMERA_IMAGE_STORAGE)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  paramStationCdList      … ステーションコードリスト
        ///  paramSerialList         … 型式/機番リスト
        ///  paramProductModelCd     … 生産型式コード(前方一致)
        ///  paramInstallDtFrom      … 測定日(FROM)
        ///  paramInstallDtTo        … 測定日(TO)
        ///  paramProductModelCdList … 生産型式コードリスト
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectCamImageList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
            //エンジン品質画像証跡用ステーション取得(設定ファイル)
            List<string> stationCdList = WebAppInstance.GetInstance().Config.ApplicationInfo.engineCamImageStationCdList;

            //共通工程呼出
            CommonProcessDao.SearchParameter paramCom = new CommonProcessDao.SearchParameter();
            paramCom.paramStationCdList = stationCdList;                        //ステーションコード
            paramCom.paramSerialList = param.paramSerialList;                   //型式/機番
            paramCom.paramProductModelCd = param.paramProductModelCd;           //生産型式コード
            paramCom.paramInspectionDtFrom = param.paramInspectionDtFrom;       //測定日(FROM)
            paramCom.paramInspectionDtTo = param.paramInspectionDtTo;           //測定日(TO)
            paramCom.paramProductModelCdList = param.paramProductModelCdList;   //生産型式コードリスト
            return CommonProcessDao.SelectCamImageList( paramCom, maxRecordCount );
        }

        /// <summary>
        /// 一覧画面エンジン工程(電子チェックシート)情報検索(TT_SQ_ICS_INS_RESULT)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  paramStationCdList      … ステーションコードリスト
        ///  paramSerialList         … 型式/機番リスト
        ///  paramProductModelCd     … 生産型式コード(前方一致)
        ///  paramInstallDtFrom      … 測定日(FROM)
        ///  paramInstallDtTo        … 測定日(TO)
        ///  paramProductModelCdList … 生産型式コードリスト
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectELCheckList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
            //共通工程呼出
            CommonProcessDao.SearchParameter paramCom = new CommonProcessDao.SearchParameter();
            paramCom.paramSerialList = param.paramSerialList;                       //型式/機番リスト
            paramCom.paramProductModelCd = param.paramProductModelCd;               //生産型式コード(前方一致)
            paramCom.paramInspectionDtFrom = param.paramInspectionDtFrom;           //測定日(FROM)
            paramCom.paramInspectionDtTo = param.paramInspectionDtTo;               //測定日(TO)
            paramCom.paramProductModelCdList = param.paramProductModelCdList;       //生産型式コードリスト
            paramCom.paramProductKindCd = Defines.ListDefine.ProductKind.Engine;    //製品種別
            return CommonProcessDao.SelectImgCheckList( paramCom, maxRecordCount );

        }

        /// <summary>
        /// 一覧画面エンジン工程(AI画像解析)情報検索(TT_SQ_IMG_ANL_RESULT)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  paramStationCdList      … ステーションコードリスト
        ///  paramSerialList         … 型式/機番リスト
        ///  paramProductModelCd     … 生産型式コード(前方一致)
        ///  paramInstallDtFrom      … 測定日(FROM)
        ///  paramInstallDtTo        … 測定日(TO)
        ///  paramProductModelCdList … 生産型式コードリスト
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectAiImageList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
            //エンジンAI画像解析用ステーション取得(設定ファイル)
            List<string> stationCdList = WebAppInstance.GetInstance().Config.ApplicationInfo.engineAiImageStationCdList;

            //共通工程呼出
            CommonProcessDao.SearchParameter paramCom = new CommonProcessDao.SearchParameter();
            paramCom.paramStationCdList = stationCdList;                            //ステーションコード
            paramCom.paramSerialList = param.paramSerialList;                       //型式/機番リスト
            paramCom.paramProductModelCd = param.paramProductModelCd;               //生産型式コード(前方一致)
            paramCom.paramInspectionDtFrom = param.paramInspectionDtFrom;           //測定日(FROM)
            paramCom.paramInspectionDtTo = param.paramInspectionDtTo;               //測定日(TO)
            paramCom.paramProductModelCdList = param.paramProductModelCdList;       //生産型式コードリスト
            return CommonProcessDao.SelectAiImageList( paramCom, maxRecordCount );
        }
        #endregion

        #region 詳細検索DAO

        /// <summary>
        /// 詳細画面エンジン工程(トルク締付)最新来歴情報検索(TBL_トルク締付履歴)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectEngineTorqueLatestHistory( string productModelCd, string serial ) {
            KTBindParameters bindParam = new KTBindParameters();

            //パラメータの設定
            bindParam.Add( "paramSerial", new SerialParam( productModelCd, serial ) );          //型式/機番リスト

            //SQL
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineTorqueLatestHistory" );

            //検索
            return SelectData( statementId, bindParam );
        }

        /// <summary>
        /// 詳細画面エンジン工程(トルク締付)情報検索(TBL_トルク締付履歴)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="partsNm">部品名</param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectEngineTorqueDetail( string productModelCd, string serial, string partsNm ) {
            KTBindParameters bindParam = new KTBindParameters();

            //パラメータの設定
            bindParam.Add( "paramSerial", new SerialParam( productModelCd, serial ) );          //型式/機番リスト
            bindParam.Add( "partsNm", partsNm );                                                //部品名

            //SQL
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineTorqueDetail" );

            //検索
            return SelectData( statementId, bindParam );
        }

        /// <summary>
        /// 詳細画面エンジン工程(トルク締付)情報検索(TBL_トルク締付履歴)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectEngineTorqueDetail( string productModelCd, string serial ) {
            KTBindParameters bindParam = new KTBindParameters();

            //パラメータの設定
            bindParam.Add( "paramSerial", new SerialParam( productModelCd, serial ) );          //型式/機番リスト

            //SQL
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineTorqueDetail" );

            //検索
            return SelectData( statementId, bindParam );
        }

        /// <summary>
        /// 詳細画面エンジン工程(ハーネス検査)情報検索(TBL_ハーネス検査履歴)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectEngineHarnessDetail( string productModelCd, string serial ) {
            KTBindParameters bindParam = new KTBindParameters();

            //パラメータの設定
            bindParam.Add( "paramSerial", new SerialParam( productModelCd, serial ) );          //型式/機番リスト

            //SQL
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineHarnessDetail" );

            //検索
            return SelectData( statementId, bindParam );
        }

        /// <summary>
        /// 詳細画面エンジン工程(エンジン運転検査)情報検索(D_HEADER,D_HEADER_07)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectEngineTestHeader( string assemblyPatternCd, string modelCd, string productModelCd, string serial ) {
            KTBindParameters bindParam = new KTBindParameters();

            //************************************
            // ヘッダ検索
            //************************************
            //パラメータの設定
            List<SerialParam> serialList = new List<SerialParam>();                               //型式/機番リスト
            serialList.Add( new SerialParam( modelCd, serial ) );
            serialList.Add( new SerialParam( productModelCd, serial ) );
            bindParam.Add( "paramSerialList", serialList );

            //03/07エンジンで検索対象を切り替える
            string statementId = null;
            switch ( assemblyPatternCd ) {
            case AssemblyPatternCode.OemEngine03:
            case AssemblyPatternCode.InstalledEngine03:
                //03エンジン…D_HEADER
                statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineTest03Header" );
                break;
            case AssemblyPatternCode.OemEngine07:
            case AssemblyPatternCode.InstalledEngine07:
                //07エンジン…D_HEADER_07
                statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineTest07Header" );
                break;
            default:
                break;
            }

            //検索
            return SelectData( statementId, bindParam );
        }

        /// <summary>
        /// 詳細画面エンジン工程(エンジン運転検査)情報検索(D_DETAIL,D_DETAIL_07)
        /// </summary>
        /// <param name="assemblyPatternCd">組立パターンコード</param>
        /// <param name="idno">IDNO</param>
        /// <param name="inspectionYmdHms">検査時刻</param>
        /// <returns>検索結果詳細DataTable[]</returns>
        public static DataTable SelectEngineTestDetail( string assemblyPatternCd, string idno, string inspectionYmdHms ) {
            KTBindParameters bindParam = new KTBindParameters();

            //************************************
            // 詳細検索
            //************************************
            //パラメータの設定
            bindParam.Clear();
            bindParam.Add( "idno", idno );                 //IDNO
            bindParam.Add( "inspectionYmdHms", inspectionYmdHms ); //検査時刻

            //03/07エンジンで検索対象を切り替える
            string statementId = null;
            switch ( assemblyPatternCd ) {
            case AssemblyPatternCode.OemEngine03:
            case AssemblyPatternCode.InstalledEngine03:
                //03エンジン…D_HEADER
                statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineTest03Detail" );
                break;
            case AssemblyPatternCode.OemEngine07:
            case AssemblyPatternCode.InstalledEngine07:
                //07エンジン…D_HEADER_07
                statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineTest07Detail" );
                break;
            default:
                break;
            }
            //詳細情報検索
            return SelectData( statementId, bindParam );
        }

        /// <summary>
        /// 詳細画面エンジン工程(フリクションロス)情報検索(TBL_フリクションロス実測)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectEngineFrictionLossDetail( string productModelCd, string serial ) {
            KTBindParameters bindParam = new KTBindParameters();

            //パラメータの設定
            bindParam.Add( "paramSerial", new SerialParam( productModelCd, serial ) );          //型式/機番リスト

            //SQL
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineFrictionLossDetail" );

            //検索
            return SelectData( statementId, bindParam );
        }

        /// <summary>
        /// 詳細画面エンジン工程(3C精密測定)情報検索(TT_SQ_PRECISE_MEASURE_DATA,TT_SQ_3C_DETAIL,TBL_TIPS_CODE_NAMES)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectEngine3CInspectionHeader( string productModelCd, string serial, string partsTypeCd ) {
            KTBindParameters bindParam = new KTBindParameters();

            //************************************
            // ヘッダ検索
            //************************************
            //パラメータの設定
            bindParam.Add( "paramSerial", new SerialParam( productModelCd, serial ) );          //型式/機番リスト
            bindParam.Add( "paramPartsCd", partsTypeCd );                                       //部品区分

            //SQL
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngine3CInspectionDetail" );

            //検索
            return SelectData( statementId, bindParam );
        }

        /// <summary>
        /// 詳細画面エンジン工程(3C精密測定)情報検索(TT_SQ_PRECISE_MEASURE_FILE)
        /// </summary>
        /// <param name="dt">ヘッダー情報</param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectEngine3CInspectionDetail( DataTable dt ) {
            KTBindParameters bindParam = new KTBindParameters();

            //変数定義
            DataTable dts = new DataTable();

            //************************************
            // 詳細検索
            //************************************
            //SQL
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngine3CInspectionFile" );

            //詳細情報検索
            bindParam.Add( "partsNm", StringUtils.ToString( dt.Rows[0]["partsNm"] ) );         //部品名
            bindParam.Add( "processYmd", StringUtils.ToString( dt.Rows[0]["processYmd"] ) );   //加工日
            bindParam.Add( "processNum", StringUtils.ToString( dt.Rows[0]["processNum"] ) );   //連番
            bindParam.Add( "partsTypeCd", StringUtils.ToString( dt.Rows[0]["partsTypeCd"] ) ); //部品区分

            //検索
            return SelectData( statementId, bindParam );
        }

        /// <summary>
        /// 詳細画面エンジン工程(燃料噴射時期)情報検索(TBL噴射計測データ/TBL_07噴射時期計測データ)
        /// </summary>
        /// <param name="assemblyPatternCd">組立パターンコード</param>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectEngineFuelInjectionDetail( string assemblyPatternCd, string productModelCd, string serial ) {
            KTBindParameters bindParam = new KTBindParameters();

            //パラメータの設定
            bindParam.Add( "paramSerial", new SerialParam( productModelCd, serial ) );          //型式/機番リスト

            DataTable resultData = null;
            //03/07エンジンで検索対象を切り替える
            string statementId = null;
            switch ( assemblyPatternCd ) {
            case AssemblyPatternCode.OemEngine03:
            case AssemblyPatternCode.InstalledEngine03:
                //03エンジン…TBL噴射計測データ
                statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineFuelInjection03Detail" );
                resultData = SelectData( statementId, bindParam );
                break;
            case AssemblyPatternCode.OemEngine07:
            case AssemblyPatternCode.InstalledEngine07:
                //07エンジン…TBL_07噴射時期計測データ
                statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineFuelInjection07Detail" );
                resultData = SelectData( statementId, bindParam );
                //ステーション名を割当(stationNmフィールドへセット)
                resultData = MasterList.SetStationNm( resultData );
                break;
            default:
                break;
            }
            return resultData;
        }

        /// <summary>
        /// 詳細画面エンジン工程(品質画像証跡)情報検索(TT_SQ_CAMERA_IMAGE_STORAGE)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectCamImageDetail( string productModelCd, string serial ) {
            //エンジン品質画像証跡用ステーション取得(設定ファイル)
            List<string> stationCdList = WebAppInstance.GetInstance().Config.ApplicationInfo.engineCamImageStationCdList;

            //共通工程呼出
            return CommonProcessDao.SelectCamImageDetail( stationCdList, productModelCd, serial );
        }

        #region 電子チェックシート検索DAO
        /// <summary>
        /// 詳細画面エンジン工程(電子チェックシート：検査ヘッダ)情報取得(TT_SQ_ICS_INS_RESULT)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="lineCd">ラインコード</param>
        /// <remarks></remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectELCheckHeader( string productModelCd, string serial, string lineCd = null ) {

            //共通工程呼出
            return CommonProcessDao.SelectImgCheckHeader( productModelCd, serial, Defines.ListDefine.ProductKind.Engine, lineCd );

        }
        #endregion

        #region 出荷部品検索DAO
        /// <summary>
        /// 詳細画面エンジン部品(出荷部品)情報検索(TT_PW_TRC_DETAIL)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectShipmentParts( string productModelCd, string serial ) {
            KTBindParameters bindParam = new KTBindParameters();
            // パラメータの設定
            bindParam.Add( "paramSerial", new SerialParam( productModelCd, serial ) );
            var statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectShipmentParts" );
            return SelectData( statementId, bindParam );
        }

        /// <summary>
        /// 詳細画面エンジン部品(梱包作業指示書)情報検索(TT_DS_PACKING_ORDER)
        /// </summary>
        /// <param name="packingOrderIdno">梱包作業指示書IDNO</param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectPackingOrderSheet( IEnumerable<string> packingOrderIdno ) {
            KTBindParameters bindParam = new KTBindParameters();
            // パラメータの設定
            bindParam.Add( "idNoList", packingOrderIdno.ToList() );
            bindParam.Add( "stationList", WebAppInstance.GetInstance().Config.ApplicationInfo.engineShipmentPartsPickStationList );
            var statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectPackingOrderSheet" );
            return SelectData( statementId, bindParam );
        }
        #endregion

        #region AI画像解析検索DAO
        /// <summary>
        /// 詳細画面エンジン工程(AI画像解析)情報取得(TT_SQ_IMG_ANL_RESULT)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="lineCd">ラインコード</param>
        /// <remarks></remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectEngineAiImageDetail( string productModelCd, string serial ) {
            //エンジンAI画像解析用ステーション取得(設定ファイル)
            List<string> stationCdList = WebAppInstance.GetInstance().Config.ApplicationInfo.engineAiImageStationCdList;

            //共通工程呼出
            return CommonProcessDao.SelectAiImageDetail( stationCdList, productModelCd, serial );
        }
        #endregion

        #endregion

        /// <summary>
        /// データ検索処理
        /// </summary>
        /// <param name="statementId">ステートメントID</param>
        /// <param name="bindParam">パラメータ</param>
        /// <param name="maxRecordCount">最大出力件数</param>
        /// <returns>出力結果DataTable</returns>
        private static DataTable SelectData( string statementId, KTBindParameters bindParam, int maxRecordCount = Int32.MaxValue ) {
            Cursor cursor = CreDao.GetInstance().OpenCursor( statementId, bindParam );
            DataTable resultTable = null;
            try {
                while ( CreDao.GetInstance().Fetch( ref resultTable, ref cursor ) ) {
                    if ( resultTable.Rows.Count >= maxRecordCount ) {
                        break;
                    }
                }
            } finally {
                CreDao.GetInstance().CloseCursor( ref cursor );
            }
            resultTable.AcceptChanges();

            return resultTable;
        }

        #region 更新処理
        /// <summary>
        /// 重要チェック_除外登録(TM_SQ_CRITICAL_CHECK_EXCLUDE)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="partsCd">製品区分</param>
        /// <param name="prosessionDt">(修正後)加工日</param>
        /// <param name="updBy">更新ユーザ</param>
        /// <returns></returns>
        public static int InsertCriticalCheckExclude( string modelCd, string serial, string oprKind, string oprValue,
                                                     string Station, string PartsNm, string notes, string updBy, string updSys ) {
            KTBindParameters bindParam = new KTBindParameters();

            int intRet = 0;
            //************************************
            //パラメータの設定
            //************************************
            bindParam.Add( "paramproductModelCd", modelCd );
            bindParam.Add( "paramserial", serial );
            bindParam.Add( "paramoprKind", oprKind );
            bindParam.Add( "paramoprValue", oprValue );
            bindParam.Add( "paramStation", Station );
            bindParam.Add( "paramPartsNm", PartsNm );
            bindParam.Add( "paramnotes", notes );
            bindParam.Add( "paramupdBy", updBy );
            bindParam.Add( "paramupdSys", updSys );

            //実行SQL
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "InsertCriticalCheckExclude" );

            try {
                //transaction発行
                CreDao.GetInstance().BeginTransaction();

                //更新
                intRet = CreDao.GetInstance().Exec( statementId, bindParam );

                CreDao.GetInstance().CommitTransaction();
            } catch ( DataAccessException ex ) {
                if ( ex.OracleErrorNumber == DataAccessBase.ORAERROR_TIMEOUT ) {
                    //クエリ発行タイムアウト
                } else if ( ex.OracleErrorNumber == 1 ) {
                    //一意制約
                } else {
                    //タイムアウト以外のException                    
                }
                intRet = -1;
            } catch ( Exception ex ) {
                intRet = -1;
            } finally {
                if ( true == CreDao.GetInstance().IsTransaction ) {
                    try {
                        CreDao.GetInstance().RollbackTransaction();
                    } catch {
                        intRet = -1;
                    }
                }
            }

            return intRet;
        }
        /// <summary>
        /// 重要チェック_除外更新(TM_SQ_CRITICAL_CHECK_EXCLUDE)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="partsCd">製品区分</param>
        /// <param name="prosessionDt">(修正後)加工日</param>
        /// <param name="updBy">更新ユーザ</param>
        /// <returns></returns>
        public static int UpdateCriticalCheckExcude( Dictionary<string, string> _orgData, string oprKind, string oprValue, string notes,
                                                     string Station, string PartsNm, string updBy ) {
            KTBindParameters bindParam = new KTBindParameters();

            int intRet = 0;
            //************************************
            //パラメータの設定
            //************************************

            string orgKind = StringUtils.CutString( _orgData["opr"], 3 ).Trim();
            string orgValue = _orgData["opr"].Substring( 3 );
            string orgModelCd = _orgData["modelCd"].Replace( "-", "" );

            string orgSt = null;
            string orgPartsNm = null;

            //除外ステーション、除外部品名
            if ( StringUtils.IsNotEmpty( _orgData["st"] ) ) {
                orgSt = StringUtils.CutString( _orgData["st"], 6 ).Trim();
                orgPartsNm = _orgData["st"].Substring( 6 );
            }



            bindParam.Add( "org_modelCd", orgModelCd );
            bindParam.Add( "org_serial", _orgData["serial"] );
            bindParam.Add( "org_oprKind", orgKind );
            bindParam.Add( "org_oprValue", orgValue );
            bindParam.Add( "org_Station", orgSt );
            bindParam.Add( "org_PartsNm", orgPartsNm );


            bindParam.Add( "paramoprKind", oprKind );
            bindParam.Add( "paramoprValue", oprValue );
            bindParam.Add( "paramStation", Station );
            bindParam.Add( "paramPartsNm", PartsNm );
            bindParam.Add( "paramnotes", notes );
            bindParam.Add( "paramupdBy", updBy );

            //実行SQL
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "UpdateCriticalCheckExcude" );

            try {
                //transaction発行
                CreDao.GetInstance().BeginTransaction();

                //更新
                intRet = CreDao.GetInstance().Exec( statementId, bindParam );

                CreDao.GetInstance().CommitTransaction();
            } catch ( DataAccessException ex ) {
                if ( ex.OracleErrorNumber == DataAccessBase.ORAERROR_TIMEOUT ) {
                    //クエリ発行タイムアウト
                } else {
                    //タイムアウト以外のException                    
                }
                intRet = -1;
            } catch ( Exception ex ) {
                intRet = -1;
            } finally {
                if ( true == CreDao.GetInstance().IsTransaction ) {
                    try {
                        CreDao.GetInstance().RollbackTransaction();
                    } catch {
                        intRet = -1;
                    }
                }
            }

            return intRet;
        }
        /// <summary>
        /// 重要チェック_除外削除(TM_SQ_CRITICAL_CHECK_EXCLUDE)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="partsCd">製品区分</param>
        /// <param name="prosessionDt">(修正後)加工日</param>
        /// <param name="updBy">更新ユーザ</param>
        /// <returns></returns>
        public static int DeleteCriticalCheckExclude( string productModelCd, string serial, string oprKind, string oprValue, string Station, string PartsNm ) {
            KTBindParameters bindParam = new KTBindParameters();

            int intRet = 0;
            string paramSerial = DataUtils.GetSerial6( serial );

            //************************************
            //パラメータの設定
            //************************************

            bindParam.Add( "paramproductModelCd", productModelCd );
            bindParam.Add( "paramserial", paramSerial );
            bindParam.Add( "paramoprKind", oprKind );
            bindParam.Add( "paramoprValue", oprValue );
            bindParam.Add( "paramStation", Station );
            bindParam.Add( "paramPartsNm", PartsNm );

            //実行SQL
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "DeleteCriticalCheckExclude" );

            try {
                //transaction発行
                CreDao.GetInstance().BeginTransaction();

                //更新
                intRet = CreDao.GetInstance().Exec( statementId, bindParam );

                CreDao.GetInstance().CommitTransaction();
            } catch ( DataAccessException ex ) {
                if ( ex.OracleErrorNumber == DataAccessBase.ORAERROR_TIMEOUT ) {
                    //クエリ発行タイムアウト
                } else {
                    //タイムアウト以外のException                    
                }
                intRet = -1;
            } catch ( Exception ex ) {
                intRet = -1;
            } finally {
                if ( true == CreDao.GetInstance().IsTransaction ) {
                    try {
                        CreDao.GetInstance().RollbackTransaction();
                    } catch {
                        intRet = -1;
                    }
                }
            }

            return intRet;
        }
        #endregion

        #region マスタメンテ

        #region チェック対象外リスト
        /// <summary>
        /// 一覧画面エンジン工程(チェック対象外リスト)情報検索(TM_SQ_CRITICAL_CHECK_EXCLUDE)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static Business.DetailViewBusiness.ResultSet SelectNACheckList() {

            Business.DetailViewBusiness.ResultSet resDt = new Business.DetailViewBusiness.ResultSet();

            //パラメータの設定

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectNACheckList" );
            resDt.MainTable = CreDao.GetInstance().Select( statementId, null );

            if ( 0 < resDt.MainTable.Rows.Count ) {
                resDt.SubTable = new DataTable();

                //詳細取得
                statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectNACheckListDtl" );
                resDt.SubTable = CreDao.GetInstance().Select( statementId, null );
            }

            return resDt;

        }

        /// <summary>
        /// 一覧画面：生産中データ検索(MS_SAGYO)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectInProduct( Dictionary<string, object> condition, int maxRecordCount ) {

            string productKindCd = DataUtils.GetDictionaryStringVal( condition, "productKindCd" );                 //製品区分
            string modelCd = DataUtils.GetModelCd( DataUtils.GetDictionaryStringVal( condition, "modelCd" ) ); //型式コード
            string modelNm = DataUtils.GetDictionaryStringVal( condition, "modelNm" );                //型式名
            string serial = DataUtils.GetDictionaryStringVal( condition, "serial" );                   //機番
            DateTime dtCompFrom = DataUtils.GetDictionaryDateVal( condition, "dateCompFrom" );                //完成予定日(FROM)
            DateTime dtCompTo = DataUtils.GetDictionaryDateVal( condition, "dateCompTo" );                    //完成予定日(TO)

            string paramDtCompFrom = "";
            string paramDtCompTo = "";

            //完成予定日変換
            if ( StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( condition, "dateCompFrom" ) ) ) {
                paramDtCompFrom = dtCompFrom.ToShortDateString();
            }
            if ( StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( condition, "dateCompTo" ) ) ) {
                paramDtCompTo = dtCompTo.ToShortDateString();
            }

            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //************************************
            // 詳細検索
            //************************************
            //SQL

            //詳細情報検索
            bindParam.Add( "productKindCd", productKindCd );    //製品種別
            bindParam.Add( "modelCd", modelCd );                //型式コード
            bindParam.Add( "modelNm", modelNm );                //型式名
            bindParam.Add( "serial", serial );                  //機番
            bindParam.Add( "dtCompFrom", paramDtCompFrom );             //From
            bindParam.Add( "dtCompTo", paramDtCompTo );                 //To

            //SELECT実行
            string statementId = "";

            if ( productKindCd.Equals( "30" ) ) {
                //トラクタ
                statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectInProductTrac" );
            } else {
                //エンジン
                statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectInProductEngine" );
            }

            Cursor cursor = CreDao.GetInstance().OpenCursor( statementId, bindParam );
            DataTable resultTable = null;
            try {
                while ( CreDao.GetInstance().Fetch( ref resultTable, ref cursor ) ) {
                    if ( resultTable.Rows.Count >= maxRecordCount ) {
                        break;
                    }
                }
            } finally {
                CreDao.GetInstance().CloseCursor( ref cursor );
            }

            return resultTable;
        }
        /// <summary>
        /// 一覧画面：登録済データ検索(TM_SQ_CRITICAL_CHECK_EXCLUDE)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectNotApplicable( Dictionary<string, object> condition, int maxRecordCount ) {

            string productKindCd = DataUtils.GetDictionaryStringVal( condition, "productKindCd" );                 //製品区分
            string modelCd = DataUtils.GetModelCd( DataUtils.GetDictionaryStringVal( condition, "modelCd" ) ); //型式コード
            string modelNm = DataUtils.GetDictionaryStringVal( condition, "modelNm" );                //型式名
            string serial = DataUtils.GetDictionaryStringVal( condition, "serial" );                  //機番
            DateTime dtFrom = DataUtils.GetDictionaryDateVal( condition, "dateFrom" );                //登録日(FROM)
            DateTime dtTo = DataUtils.GetDictionaryDateVal( condition, "dateTo" );                    //登録日(TO)
            DateTime dtCompFrom = DataUtils.GetDictionaryDateVal( condition, "dateCompFrom" );        //完成予定日(FROM)
            DateTime dtCompTo = DataUtils.GetDictionaryDateVal( condition, "dateCompTo" );            //完成予定日(TO)

            string paramModelCd = null;
            string paramDtCompFrom = "";
            string paramDtCompTo = "";

            //登録日変換
            DateTime? paramDtTo = GetSearchToDate( dtTo );
            DateTime? paramDtFrom = GetSearchFromDate( dtFrom );

            //完成予定日変換
            if ( StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( condition, "dateCompFrom" ) ) ) {
                paramDtCompFrom = dtCompFrom.ToShortDateString();
            }
            if ( StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( condition, "dateCompTo" ) ) ) {
                paramDtCompTo = dtCompTo.ToShortDateString();
            }

            //型式
            paramModelCd = modelCd.Replace( "-", "" );

            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //************************************
            // 詳細検索
            //************************************
            //SQL

            //詳細情報検索
            bindParam.Add( "productKindCd", productKindCd );    //製品種別
            bindParam.Add( "modelCd", paramModelCd );           //型式コード
            bindParam.Add( "modelNm", modelNm );                //型式名
            bindParam.Add( "serial", serial );                  //機番
            bindParam.Add( "dtFrom", paramDtFrom );             //登録日From
            bindParam.Add( "dtTo", paramDtTo );                 //登録日To
            bindParam.Add( "dtCompFrom", paramDtCompFrom );     //完成予定日From
            bindParam.Add( "dtCompTo", paramDtCompTo );         //完成予定日To

            //SELECT実行
            string statementId = "";
            if ( productKindCd.Equals( "30" ) ) {
                //トラクタ
                statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectNotApplicableTrac" );
            } else {
                //エンジン
                statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectNotApplicableEngine" );
            }
            Cursor cursor = CreDao.GetInstance().OpenCursor( statementId, bindParam );
            DataTable resultTable = null;
            try {
                while ( CreDao.GetInstance().Fetch( ref resultTable, ref cursor ) ) {
                    if ( resultTable.Rows.Count >= maxRecordCount ) {
                        break;
                    }
                }
            } finally {
                CreDao.GetInstance().CloseCursor( ref cursor );
            }

            return resultTable;
        }

        /// <summary>
        /// 一覧画面：メモデータ検索(TM_SQ_CRITICAL_CHECK_EXCLUDE)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectNotApplicable1( Dictionary<string, string> _orgData, int maxRecordCount ) {
            KTBindParameters bindParam = new KTBindParameters();

            string orgKind = null;
            string orgValue = null;
            string orgSt = null;
            string orgPartsNm = null;

            //************************************
            //パラメータの設定
            //************************************

            //型式コード
            string orgModelCd = _orgData["modelCd"].Replace( "-", "" );

            //除外部品作業種別、除外部品作業番号
            if ( StringUtils.IsNotEmpty( _orgData["opr"] ) ) {
                orgKind = StringUtils.CutString( _orgData["opr"], 3 ).Trim();
                orgValue = _orgData["opr"].Substring( 3 );
            }

            //除外ステーション、除外部品名
            if ( StringUtils.IsNotEmpty( _orgData["st"] ) ) {
                orgSt = StringUtils.CutString( _orgData["st"], 6 ).Trim();
                orgPartsNm = _orgData["st"].Substring( 6 );
            }

            bindParam.Add( "modelCd", orgModelCd );
            bindParam.Add( "serial", _orgData["serial"] );
            bindParam.Add( "oprKind", orgKind );
            bindParam.Add( "oprValue", orgValue );
            bindParam.Add( "st", orgSt );
            bindParam.Add( "partsNm", orgPartsNm );

            //************************************
            // 詳細検索
            //************************************
            //SQL

            //SELECT実行
            string statementId = "";
            if ( _orgData["ptnCd"].Equals( "30" ) ) {
                //トラクタ
                statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectNotApplicableTrac" );
            } else {
                //エンジン
                statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectNotApplicableEngine" );
            }

            Cursor cursor = CreDao.GetInstance().OpenCursor( statementId, bindParam );
            DataTable resultTable = null;
            try {
                while ( CreDao.GetInstance().Fetch( ref resultTable, ref cursor ) ) {
                    if ( resultTable.Rows.Count >= maxRecordCount ) {
                        break;
                    }
                }
            } finally {
                CreDao.GetInstance().CloseCursor( ref cursor );
            }

            return resultTable;
        }

        /// <summary>
        /// 明細画面：登録済データ検索(TM_SQ_CRITICAL_CHECK_EXCLUDE)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectNotApplicableDetail( Dictionary<string, object> condition, int maxRecordCount ) {

            string productKindCd = DataUtils.GetDictionaryStringVal( condition, "ptnCd" );                 //製品種別
            string modelCd = DataUtils.GetModelCd( DataUtils.GetDictionaryStringVal( condition, "modelCd" ) ); //型式コード
            string modelNm = DataUtils.GetDictionaryStringVal( condition, "modelNm" );                //型式名
            string serial = DataUtils.GetDictionaryStringVal( condition, "serial" );                   //機番
            DateTime dtFrom = DataUtils.GetDictionaryDateVal( condition, "dateFrom" );                //日付(FROM)
            DateTime dtTo = DataUtils.GetDictionaryDateVal( condition, "dateTo" );                    //日付(TO)


            string paramModelCd = null;
            string paramDtFrom = null;
            string paramDtTo = null;

            //型式
            paramModelCd = modelCd.Replace( "-", "" );

            //完成予定日変換
            if ( StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( condition, "dateFrom" ) ) ) {
                paramDtFrom = dtFrom.ToShortDateString();
            }
            if ( StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( condition, "dateTo" ) ) ) {
                paramDtTo = dtTo.ToShortDateString();
            }


            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //************************************
            // 詳細検索
            //************************************
            //SQL

            //詳細情報検索
            bindParam.Add( "productKindCd", productKindCd );    //製品種別
            bindParam.Add( "modelCd", paramModelCd );           //型式コード
            bindParam.Add( "modelNm", modelNm );                //型式名
            bindParam.Add( "serial", serial );                  //機番
            bindParam.Add( "dtFrom", paramDtFrom );             //From
            bindParam.Add( "dtTo", paramDtTo );                 //To

            //SELECT実行
            string statementId = "";
            if ( productKindCd.Equals( "30" ) ) {
                //トラクタ
                statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectNotApplicableDetailTrac" );
            } else {
                //エンジン
                statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectNotApplicableDetailEngine" );
            }
            Cursor cursor = CreDao.GetInstance().OpenCursor( statementId, bindParam );
            DataTable resultTable = null;
            try {
                while ( CreDao.GetInstance().Fetch( ref resultTable, ref cursor ) ) {
                    if ( resultTable.Rows.Count >= maxRecordCount ) {
                        break;
                    }
                }
            } finally {
                CreDao.GetInstance().CloseCursor( ref cursor );
            }

            return resultTable;
        }

        /// <summary>
        /// 一覧画面：完成日取得
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectHeaderInfo( string modelCd, string serial, string ptnCd, int maxRecordCount ) {


            string paramModelCd = null;

            //型式
            paramModelCd = DataUtils.GetModelCd( modelCd );

            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //************************************
            // 詳細検索
            //************************************
            //SQL

            //詳細情報検索
            bindParam.Add( "productKindCd", ptnCd );            //製品種別
            bindParam.Add( "modelCd", paramModelCd );           //型式コード
            bindParam.Add( "serial", serial );                  //機番

            //SELECT実行
            string statementId = "";
            if ( ptnCd.Equals( "30" ) ) {
                //トラクタ
                statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectHeaderInfoTrac" );
            } else {
                //エンジン
                statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectHeaderInfoEngine" );
            }
            Cursor cursor = CreDao.GetInstance().OpenCursor( statementId, bindParam );
            DataTable resultTable = null;
            try {
                while ( CreDao.GetInstance().Fetch( ref resultTable, ref cursor ) ) {
                    if ( resultTable.Rows.Count >= maxRecordCount ) {
                        break;
                    }
                }
            } finally {
                CreDao.GetInstance().CloseCursor( ref cursor );
            }

            return resultTable;
        }
        #endregion

        #region 3C加工日
        /// <summary>
        /// 明細画面：3C詳細テーブル検索(重要部品エンジン組付実績3C詳細テーブル)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable Select3CList( Dictionary<string, object> condition, bool blTargetKind, int maxRecordCount ) {

            string productKind = DataUtils.GetDictionaryStringVal( condition, "productKind" );      //製品区分
            string engineKind = DataUtils.GetDictionaryStringVal( condition, "engineKind" );        //エンジン種別
            DateTime dtFrom = DataUtils.GetDictionaryDateVal( condition, "dateFrom" );              //日付(FROM)
            DateTime dtTo = DataUtils.GetDictionaryDateVal( condition, "dateTo" );                  //日付(TO)
            DateTime dtAssembyFrom = DataUtils.GetDictionaryDateVal( condition, "assemblyStart" );              //日付(FROM)
            DateTime dtAssembyTo = DataUtils.GetDictionaryDateVal( condition, "assemblyEnd" );                  //日付(TO)

            //加工日変換
            DateTime? paramDtFrom = GetSearchFromDate( dtFrom );
            DateTime? paramDtTo = GetSearchToDate( dtTo );

            //取付日変換
            DateTime? paramAssembyFrom = GetSearchFromDate( dtAssembyFrom );
            DateTime? paramAssemblyTo = GetSearchToDate( dtAssembyTo );

            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //************************************
            // 詳細検索
            //************************************
            //SQL

            //詳細情報検索
            bindParam.Add( "productKind", productKind );        //製品区分
            bindParam.Add( "engineKind", engineKind.Trim() );   //エンジン種別
            bindParam.Add( "targetDt", blTargetKind );          //対象区分
            bindParam.Add( "dtFrom", paramDtFrom );             //From
            bindParam.Add( "dtTo", paramDtTo );                 //To
            bindParam.Add( "assemblyFrom", paramAssembyFrom );  //取付日From
            bindParam.Add( "assemblyTo", paramAssemblyTo );     //取付日To


            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "Select3CList" );

            Cursor cursor = CreDao.GetInstance().OpenCursor( statementId, bindParam );
            DataTable resultTable = null;
            try {
                while ( CreDao.GetInstance().Fetch( ref resultTable, ref cursor ) ) {
                    if ( resultTable.Rows.Count >= maxRecordCount ) {
                        break;
                    }
                }
            } finally {
                CreDao.GetInstance().CloseCursor( ref cursor );
            }

            return resultTable;
        }


        /// <summary>
        /// 重要部品エンジン組付実績3C詳細テーブル更新(重要部品エンジン組付実績3C詳細テーブル)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="partsCd">製品区分</param>
        /// <param name="prosessionDt">(修正後)加工日</param>
        /// <param name="updBy">更新ユーザ</param>
        /// <param name="exeCnt">更新件数</param>
        /// <returns></returns>
        public static int Update3CDetail( DataTable dtParam, string updBy, string updSys, int exeCnt ) {

            int intRet = 0;
            int intExec = 0;
            //************************************
            //パラメータの設定
            //************************************

            //実行SQL
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "Update3CDetail" );

            try {
                //transaction発行
                CreDao.GetInstance().BeginTransaction();


                for ( int rowIdx = 0; rowIdx < dtParam.Rows.Count; rowIdx++ ) {
                    KTBindParameters bindParam = new KTBindParameters();

                    bindParam.Add( "paramSt", StringUtils.ToString( dtParam.Rows[rowIdx]["STATION_CD"] ) );
                    bindParam.Add( "paramModelCd", StringUtils.ToString( dtParam.Rows[rowIdx]["MODEL_CD"] ) );
                    bindParam.Add( "paramSerial6", StringUtils.ToString( dtParam.Rows[rowIdx]["SERIAL6"] ) );
                    bindParam.Add( "paramPartsCd", StringUtils.ToString( dtParam.Rows[rowIdx]["CRITICAL_PARTS_CD"] ) );
                    bindParam.Add( "paramProcDt", StringUtils.ToString( dtParam.Rows[rowIdx]["MATERIAL_PROCESSING_DATE"] ) );
                    if ( 1 == exeCnt ) {
                        //単一チェックのときのみ加工連番更新
                        bindParam.Add( "paramProcNum", StringUtils.ToString( dtParam.Rows[rowIdx]["MATERIAL_PROCESSING_NUM"] ) );
                    } else {
                        bindParam.Add( "paramProcNum", null );
                    }
                    bindParam.Add( "paramProcLine", StringUtils.ToString( dtParam.Rows[rowIdx]["PROCESSING_LINE"] ) );
                    bindParam.Add( "paramRemarks", StringUtils.ToString( dtParam.Rows[rowIdx]["REMARKS"] ) );
                    bindParam.Add( "paramupdBy", updBy );
                    bindParam.Add( "paramupdSys", updSys );

                    //更新
                    intExec = CreDao.GetInstance().Exec( statementId, bindParam );
                    intRet++;
                }


                if ( dtParam.Rows.Count.Equals( intRet ) ) {
                    //正常
                    CreDao.GetInstance().CommitTransaction();
                } else {
                    CreDao.GetInstance().RollbackTransaction();
                }

            } catch ( DataAccessException ex ) {
                if ( ex.OracleErrorNumber == DataAccessBase.ORAERROR_TIMEOUT ) {
                    //クエリ発行タイムアウト
                } else if ( ex.OracleErrorNumber == 1 ) {
                    //一意制約
                } else {
                    //タイムアウト以外のException                    
                }
                intRet = -1;
            } catch ( Exception ex ) {
                intRet = -1;
            } finally {
                if ( true == CreDao.GetInstance().IsTransaction ) {
                    try {
                        CreDao.GetInstance().RollbackTransaction();
                    } catch {
                        intRet = -1;
                    }
                }
            }

            return intRet;
        }
        #endregion

        #region DPF機番情報取得
        /// <summary>
        /// 明細画面：DPF機番情報テーブル検索(TBL_DPF機番情報テーブル)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectDpfSerialList( Dictionary<string, object> condition, int maxRecordCount ) {


            string modelCd = DataUtils.GetModelCd( DataUtils.GetDictionaryStringVal( condition, "modelCd" ) );  //型式コード
            string modelNm = DataUtils.GetDictionaryStringVal( condition, "modelNm" );                          //型式名

            string serial = DataUtils.GetDictionaryStringVal( condition, "serial" );     //機番
            string modelCdDpf = DataUtils.GetModelCd( DataUtils.GetDictionaryStringVal( condition, "modelCdDpf" ) );  //DPF型式コード
            string serialDpf = DataUtils.GetDictionaryStringVal( condition, "serialDpf" );                            //DPF機番
            string lineCd = DataUtils.GetModelCd( DataUtils.GetDictionaryStringVal( condition, "lineCd" ) );    //ライン
            string st = DataUtils.GetDictionaryStringVal( condition, "st" );                                    //ステーション
            string productKind = DataUtils.GetDictionaryStringVal( condition, "productKind" );                  //製品種別
            string idno = DataUtils.GetDictionaryStringVal( condition, "idno" );                                //IDNO
            string parts = DataUtils.GetDictionaryStringVal( condition, "parts" );                              //部品区分
            string sample = DataUtils.GetDictionaryStringVal( condition, "sample" );                            //抜取検査
            string regKind = DataUtils.GetDictionaryStringVal( condition, "regKind" );                          //登録種別


            DateTime dtFrom = DataUtils.GetDictionaryDateVal( condition, "dateFrom" );                          //日付(FROM)
            DateTime dtTo = DataUtils.GetDictionaryDateVal( condition, "dateTo" );                              //日付(TO)
            DateTime dtAssembyFrom = DataUtils.GetDictionaryDateVal( condition, "assemblyStart" );              //日付(FROM)
            DateTime dtAssembyTo = DataUtils.GetDictionaryDateVal( condition, "assemblyEnd" );                  //日付(TO)

            //加工日変換
            DateTime? paramDtFrom = GetSearchFromDate( dtFrom );
            DateTime? paramDtTo = GetSearchToDate( dtTo );

            //取付日変換
            DateTime? paramAssembyFrom = GetSearchFromDate( dtAssembyFrom );
            DateTime? paramAssemblyTo = GetSearchToDate( dtAssembyTo );

            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //************************************
            // 詳細検索
            //************************************
            //SQL

            //詳細情報検索

            bindParam.Add( "modelCd", modelCd );                //型式
            bindParam.Add( "modelNm", modelNm );                //型式名

            bindParam.Add( "serial", serial );                  //機番番号
            bindParam.Add( "modelCdDpf", modelCdDpf );          //DPF型式
            bindParam.Add( "serialDpf", serialDpf );            //DPF機番番号
            bindParam.Add( "lineCd", lineCd );                  //ラインコード
            bindParam.Add( "st", st );                          //ステーション
            bindParam.Add( "productKind", productKind );        //製品種別
            bindParam.Add( "idno", idno );                      //機番番号
            bindParam.Add( "parts", parts );                    //部品区分
            bindParam.Add( "sample", sample );                  //抜取検査
            bindParam.Add( "regKind", regKind );                //登録種別
            bindParam.Add( "dtFrom", paramDtFrom );             //From
            bindParam.Add( "dtTo", paramDtTo );                 //To
            bindParam.Add( "assemblyFrom", paramAssembyFrom );  //取付日From
            bindParam.Add( "assemblyTo", paramAssemblyTo );     //取付日To


            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectDpfSerialList" );

            Cursor cursor = CreDao.GetInstance().OpenCursor( statementId, bindParam );
            DataTable resultTable = null;
            try {
                while ( CreDao.GetInstance().Fetch( ref resultTable, ref cursor ) ) {
                    if ( resultTable.Rows.Count >= maxRecordCount ) {
                        break;
                    }
                }
            } finally {
                CreDao.GetInstance().CloseCursor( ref cursor );
            }

            return resultTable;
        }


        /// <summary>
        /// 明細画面：DPF機番情報テーブルデータ存在チェック
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static int SelectDpfSerialCheck( Dictionary<string, object> condition, int maxRecordCount = 1000 ) {

            string modelCdDpf = DataUtils.GetModelCd( DataUtils.GetDictionaryStringVal( condition, "modelCdDpf" ) );  //DPF型式コード
            string serialDpf = DataUtils.GetDictionaryStringVal( condition, "serialDpf" );                            //DPF機番
            string lineCd = DataUtils.GetModelCd( DataUtils.GetDictionaryStringVal( condition, "lineCd" ) );    //ライン
            string st = DataUtils.GetDictionaryStringVal( condition, "st" );                                    //ステーション
            string productKind = DataUtils.GetModelCd( DataUtils.GetDictionaryStringVal( condition, "productKind" ) );    //製品種別

            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //************************************
            // 詳細検索
            //************************************
            //SQL

            //詳細情報検索
            bindParam.Add( "modelCdDpf", modelCdDpf );          //DPF型式
            bindParam.Add( "serialDpf", serialDpf );            //DPF機番番号
            bindParam.Add( "lineCd", lineCd );                  //ラインコード
            bindParam.Add( "st", st );                          //ステーション
            bindParam.Add( "productKind", productKind );        //製品種別

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectDpfSerialList" );

            Cursor cursor = CreDao.GetInstance().OpenCursor( statementId, bindParam );
            DataTable resultTable = null;
            try {
                while ( CreDao.GetInstance().Fetch( ref resultTable, ref cursor ) ) {
                    if ( resultTable.Rows.Count >= maxRecordCount ) {
                        break;
                    }
                }
            } finally {
                CreDao.GetInstance().CloseCursor( ref cursor );
            }

            return resultTable.Rows.Count;
        }

        #endregion

        #region 登録／更新

        /// <summary>
        /// DPF機番情報登録(TBL_DPF機番情報テーブル)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="partsCd">製品区分</param>
        /// <param name="prosessionDt">(修正後)加工日</param>
        /// <param name="updBy">更新ユーザ</param>
        /// <returns></returns>
        public static int InsertDpfSerial( Dictionary<string, object> condition, string updBy, string updSys, string partsKbn ) {

            int intRet = 0;
            int intExec = 0;

            //登録前の存在チェック
            intRet = SelectDpfSerialCheck( condition );
            //既に登録済の場合は処理終了
            if ( 0 < intRet ) {
                intRet = 1;
                return intRet;
            }

            //************************************
            //パラメータの設定
            //************************************

            //実行SQL

            try {
                string modelCd = DataUtils.GetModelCd( DataUtils.GetDictionaryStringVal( condition, "modelCd" ) );  //DPF型式コード(変更前)
                string serial = DataUtils.GetDictionaryStringVal( condition, "serial" );                            //DPF機番(変更前)
                string lineCd = DataUtils.GetModelCd( DataUtils.GetDictionaryStringVal( condition, "lineCd" ) );    //ライン
                string st = DataUtils.GetDictionaryStringVal( condition, "st" );                                    //ステーション
                string modelCdDpf = DataUtils.GetModelCd( DataUtils.GetDictionaryStringVal( condition, "modelCdDpf" ) );  //DPF型式コード(変更後)
                string serialDpf = DataUtils.GetDictionaryStringVal( condition, "serialDpf" );                            //DPF機番(変更後)
                string productKind = DataUtils.GetModelCd( DataUtils.GetDictionaryStringVal( condition, "productKind" ) );    //製品種別
                string idno = DataUtils.GetDictionaryStringVal( condition, "idno" );                                //IDNO
                string parts = partsKbn;                                                                            //部品区分

                DateTime dtAss = DataUtils.GetDictionaryDateVal( condition, "assemblyDt" );
                DateTime dNow = System.DateTime.Now;

                DateTime paramDt = new DateTime( dtAss.Year, dtAss.Month, dtAss.Day, dNow.Hour, dNow.Minute, dNow.Second );

                //transaction発行
                CreDao.GetInstance().BeginTransaction();

                //パラメータの設定
                KTBindParameters bindParam = new KTBindParameters();

                //詳細情報検索
                bindParam.Add( "modelCd", modelCd );                //型式
                bindParam.Add( "serial", serial );                  //機番番号
                bindParam.Add( "lineCd", lineCd );                  //型式
                bindParam.Add( "st", st );                          //機番番号
                bindParam.Add( "modelCdDpf", modelCdDpf );          //DPF型式
                bindParam.Add( "serialDpf", serialDpf );            //DPF機番番号
                bindParam.Add( "productKind", productKind );        //製品種別
                bindParam.Add( "idno", idno );                      //IDNO
                bindParam.Add( "parts", parts );                    //部品区分
                bindParam.Add( "assDt", paramDt );                  //組付日
                bindParam.Add( "updBy", updBy );                    //更新ユーザ
                bindParam.Add( "updSys", updSys );                  //更新PRG

                //更新
                string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "InsertDpfSerial" );
                intExec = CreDao.GetInstance().Exec( statementId, bindParam );

                if ( intExec == 1 ) {
                    //正常
                    CreDao.GetInstance().CommitTransaction();
                } else {
                    CreDao.GetInstance().RollbackTransaction();
                    intRet = 99;
                }

            } catch ( DataAccessException ex ) {
                if ( ex.OracleErrorNumber == DataAccessBase.ORAERROR_TIMEOUT ) {
                    //クエリ発行タイムアウト
                    intRet = 12170;
                } else if ( ex.OracleErrorNumber == 1 ) {
                    //一意制約
                    intRet = 1;
                } else {
                    //タイムアウト以外のException                    
                    intRet = 99;
                }
            } catch ( Exception ex ) {
                intRet = -1;
            } finally {
                if ( true == CreDao.GetInstance().IsTransaction ) {
                    try {
                        CreDao.GetInstance().RollbackTransaction();
                    } catch {
                        intRet = -1;
                    }
                }
            }

            return intRet;
        }

        //DPF機番情報更新
        /// <summary>
        /// DPF機番情報更新(TBL_DPF機番情報テーブル)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="partsCd">製品区分</param>
        /// <param name="prosessionDt">(修正後)加工日</param>
        /// <param name="updBy">更新ユーザ</param>
        /// <returns></returns>
        public static int UpdateDpfSerial( Dictionary<string, object> condition, string updBy, string updSys ) {

            int intRet = 0;
            int intExec = 0;

            //更新前の存在チェック
            intRet = SelectDpfSerialCheck( condition );

            //既に登録済の場合は処理終了
            if ( 0 < intRet ) {
                intRet = 1;
                return intRet;
            }

            //************************************
            //パラメータの設定
            //************************************

            //実行SQL

            try {
                string modelCdOrg = DataUtils.GetModelCd( DataUtils.GetDictionaryStringVal( condition, "modelCdOrg" ) );  //DPF型式コード(変更前)
                string serialOrg = DataUtils.GetDictionaryStringVal( condition, "serialOrg" );                            //DPF機番(変更前)
                string lineCd = DataUtils.GetModelCd( DataUtils.GetDictionaryStringVal( condition, "lineCd" ) );          //ライン
                string st = DataUtils.GetDictionaryStringVal( condition, "st" );                                          //ステーション
                string modelCdDpf = DataUtils.GetModelCd( DataUtils.GetDictionaryStringVal( condition, "modelCdDpf" ) );  //DPF型式コード(変更後)
                string serialDpf = DataUtils.GetDictionaryStringVal( condition, "serialDpf" );                            //DPF機番(変更後)

                //transaction発行
                CreDao.GetInstance().BeginTransaction();

                //パラメータの設定
                KTBindParameters bindParam = new KTBindParameters();

                //詳細情報検索
                bindParam.Add( "modelCdOrg", modelCdOrg );                //型式
                bindParam.Add( "serialOrg", serialOrg );                  //機番番号
                bindParam.Add( "lineCd", lineCd );                  //型式
                bindParam.Add( "st", st );                          //機番番号
                bindParam.Add( "modelCdDpf", modelCdDpf );          //DPF型式
                bindParam.Add( "serialDpf", serialDpf );            //DPF機番番号
                bindParam.Add( "updBy", updBy );                    //更新ユーザ
                bindParam.Add( "updSys", updSys );                  //更新PRG

                //更新
                string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "UpdateDpfSerial" );
                intExec = CreDao.GetInstance().Exec( statementId, bindParam );

                if ( intExec == 1 ) {
                    //正常
                    CreDao.GetInstance().CommitTransaction();
                } else {
                    CreDao.GetInstance().RollbackTransaction();
                    intRet = 99;
                }


            } catch ( DataAccessException ex ) {
                if ( ex.OracleErrorNumber == DataAccessBase.ORAERROR_TIMEOUT ) {
                    //クエリ発行タイムアウト
                    intRet = 12170;
                } else if ( ex.OracleErrorNumber == 1 ) {
                    //一意制約
                    intRet = 1;
                } else {
                    //タイムアウト以外のException                    
                    intRet = 99;
                }
            } catch ( Exception ex ) {
                intRet = -1;
            } finally {
                if ( true == CreDao.GetInstance().IsTransaction ) {
                    try {
                        CreDao.GetInstance().RollbackTransaction();
                    } catch {
                        intRet = -1;
                    }
                }
            }

            return intRet;
        }
        #endregion


        #endregion

        #region 日付変換
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


    }
}