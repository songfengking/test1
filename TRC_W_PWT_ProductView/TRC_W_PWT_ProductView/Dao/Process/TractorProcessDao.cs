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

namespace TRC_W_PWT_ProductView.Dao.Process {
    /// <summary>
    /// トラクタ工程検索DAO
    /// </summary>
    public class TractorProcessDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "TractorProcess";

        /// <summary>
        /// 検索パラメータクラス
        /// </summary>
        public class SearchParameter {
            /// <summary>[共通]型式/機番</summary>
            public List<SerialParam> paramSerialList = null;
            /// <summary>[共通]生産型式コード(前方一致)</summary>
            public string paramProductModelCd = null;
            /// <summary>測定日(FROM)</summary>
            public DateTime? paramInspectionDtFrom = null;
            /// <summary>測定日(TO)</summary>
            public DateTime? paramInspectionDtTo = null;
            /// <summary>[共通]生産型式コードリスト(型式名から検索したリスト)</summary>
            public List<string> paramProductModelCdList = null;
        }

        #region 一覧検索DAO
        /// <summary>
        /// 一覧画面トラクタ工程(パワクロ走行検査)情報検索(TT_SQ_POWCLAW_INS_ITEM_RESULT)
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
        public static DataTable SelectPCrawlerList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
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
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectTractorPCrawlerList" );
            return SelectData( statementId, bindParam, maxRecordCount );
        }

        /// <summary>
        /// 一覧画面トラクタ工程(チェックシートイメージ)情報検索(TBL_チェックシートイメージ)
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
        public static DataTable SelectCheckSheetList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
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
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectTractorCheckSheetList" );
            return SelectData( statementId, bindParam, maxRecordCount );
        }

        /// <summary>
        /// 一覧画面トラクタ工程(品質画像証跡)情報検索(TT_SQ_CAMERA_IMAGE_STORAGE)
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
            //トラクタ品質画像証跡用ステーション取得(設定ファイル)
            List<string> stationCdList = WebAppInstance.GetInstance().Config.ApplicationInfo.tractorCamImageStationCdList;

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
        /// 一覧画面トラクタ工程(電子チェックシート)情報検索(TT_SQ_SIS_INS_RESULT)
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
        public static DataTable SelectELCheckList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
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
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectTractorELCheckList" );
            DataTable dtRet = SelectData( statementId, bindParam, maxRecordCount );

            if ( null != dtRet && dtRet.Rows.Count > 0 ) {
                foreach ( DataRow row in dtRet.Rows ) {
                    string modelCd = StringUtils.ToString( row["productModelCd"] );
                    string serial = StringUtils.ToString( row["serial6"] );
                    string lineCd = StringUtils.ToString( row["lineCd"] );

                    row["lastProc"] = SelectELCheckLastProc( modelCd, serial, row.Field<string>( "INSPECTIONDT" ), lineCd );
                }
            }

            return dtRet;


        }

        /// <summary>
        /// 一覧画面トラクタ工程(イメージチェックシート)情報検索(TT_SQ_ICS_INS_RESULT)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <param name="productKindCd">製品種別</param>
        /// <remarks>
        /// param設定情報
        ///  paramStationCdList      … ステーションコードリスト
        ///  paramSerialList         … 型式/機番リスト
        ///  paramProductModelCd     … 生産型式コード(前方一致)
        ///  paramInstallDtFrom      … 測定日(FROM)
        ///  paramInstallDtTo        … 測定日(TO)
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectImgCheckList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
            //共通工程呼出
            CommonProcessDao.SearchParameter paramCom = new CommonProcessDao.SearchParameter();
            paramCom.paramSerialList = param.paramSerialList;                       //型式/機番リスト
            paramCom.paramProductModelCd = param.paramProductModelCd;               //生産型式コード(前方一致)
            paramCom.paramInspectionDtFrom = param.paramInspectionDtFrom;           //測定日(FROM)
            paramCom.paramInspectionDtTo = param.paramInspectionDtTo;               //測定日(TO)
            paramCom.paramProductModelCdList = param.paramProductModelCdList;       //生産型式コードリスト
            paramCom.paramProductKindCd = Defines.ListDefine.ProductKind.Tractor;   //製品種別
            return CommonProcessDao.SelectImgCheckList( paramCom, maxRecordCount );

        }

        /// <summary>
        /// 一覧画面トラクタ工程(刻印)情報検索(TT_SQ_PRINTOUT_RECORD)
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
        public static DataTable SelectPrintSheelList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
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
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectPrintSheelList" );

            return SelectData( statementId, bindParam, maxRecordCount );

        }
        /// <summary>
        /// 一覧画面トラクタ工程(ナットランナー)情報検索(TBL_検査記録機種情報)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectNutRunnerList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
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
            string statementId = GetFullStatementIdForLibrary( "EngineProcess", "SelectNutRunnerList" );
            return SelectCreData( statementId, bindParam, maxRecordCount );
        }

        /// <summary>
        /// 一覧画面トラクタ工程(光軸検査)情報検索(TT_SQ_ETR_INS_OPTAXIS_RESULT)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectOptaxisList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
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
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectOptaxisList" );
            return SelectData( statementId, bindParam, maxRecordCount );
        }

        /// <summary>
        /// 一覧画面トラクタ工程(トラクタ走行検査)情報検索(TT_SQ_ETR_INS_ITEM_RESULT)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectTractorAllList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
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
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectTractorAllList" );
            return SelectData( statementId, bindParam, maxRecordCount );
        }

        /// <summary>
        /// 一覧画面トラクタ工程(関所)情報検索_型式機番(TT_SI_PSP_WORK_RESULT)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectWorkResultModelList( SearchParameter param, string lineCd, int maxRecordCount = Int32.MaxValue ) {
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
            bindParam.Add( "lineCd", lineCd );                                                  //ラインコード

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectWorkResultModelList" );
            return SelectData( statementId, bindParam, maxRecordCount );
        }

        /// <summary>
        /// 一覧画面トラクタ工程(関所)情報検索_型式機番(TT_SI_PSP_WORK_RESULT)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectWorkResultList( SearchParameter param, string lineCd, int maxRecordCount = Int32.MaxValue ) {
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
            bindParam.Add( "lineCd", lineCd );                                                  //ラインコード

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectWorkResultList" );
            return SelectData( statementId, bindParam, maxRecordCount );
        }

        /// <summary>
        /// 一覧画面トラクタ工程(検査ベンチ)情報検索_型式機番()
        /// </summary>
        /// <param name="param"></param>
        /// <param name="maxRecordCount"></param>
        /// <returns></returns>
        public static DataTable SelectTestBenchList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
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
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectTestBenchList" );
            return SelectData( statementId, bindParam, maxRecordCount );
        }

        /// <summary>
        /// 一覧画面トラクタ工程(AI画像解析)情報検索(TT_SQ_IMG_ANL_RESULT)
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
            //トラクタAI画像解析用ステーション取得(設定ファイル)
            List<string> stationCdList = WebAppInstance.GetInstance().Config.ApplicationInfo.tractorAiImageStationCdList;

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

        #region 詳細検索
        /// <summary>
        /// 詳細画面トラクタ工程(パワクロ走行検査)ヘッダ情報検索(TT_SQ_POWCLAW_INS_ITEM_RESULT)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectTractorPCrawlerHeader( string productModelCd, string serial ) {
            KTBindParameters bindParam = new KTBindParameters();

            //パラメータの設定
            bindParam.Add( "paramSerial", new SerialParam( productModelCd, serial ) );          //型式/機番リスト

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectTractorPCrawlerHeader" );
            return SelectData( statementId, bindParam );
        }

        /// <summary>
        /// 詳細画面トラクタ工程(パワクロ走行検査)詳細情報検索(TT_SQ_POWCLAW_INS_SPEED_RESULT)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="inspectionDtFrom">検査開始時刻</param>
        /// <param name="inspectionDtTo">検査終了時刻</param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectTractorPCrawlerDetail( string productModelCd, string serial, DateTime inspectionDtFrom, DateTime inspectionDtTo ) {
            KTBindParameters bindParam = new KTBindParameters();

            //パラメータの設定
            bindParam.Add( "paramSerial", new SerialParam( productModelCd, serial ) );          //型式/機番リスト
            bindParam.Add( "paramInspectionDtFrom", inspectionDtFrom );                         //検査開始時刻
            bindParam.Add( "paramInspectionDtTo", inspectionDtTo );                             //検査終了時刻

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectTractorPCrawlerDetail" );
            return SelectData( statementId, bindParam );
        }

        /// <summary>
        /// 詳細画面トラクタ工程(チェックシート)情報検索(TBL_チェックシートイメージ)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectTractorCheckSheetDetail( string productModelCd, string serial ) {
            KTBindParameters bindParam = new KTBindParameters();

            //パラメータの設定
            bindParam.Add( "paramSerial", new SerialParam( productModelCd, serial ) );          //型式/機番リスト

            //SQL
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectTractorCheckSheetDetail" );

            //検索
            return SelectData( statementId, bindParam );
        }

        /// <summary>
        /// 詳細画面トラクタ工程(品質画像証跡)情報検索(TT_SQ_CAMERA_IMAGE_STORAGE)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectCamImageDetail( string productModelCd, string serial ) {
            //トラクタ品質画像証跡用ステーション取得(設定ファイル)
            List<string> stationCdList = WebAppInstance.GetInstance().Config.ApplicationInfo.tractorCamImageStationCdList;

            //共通工程呼出
            return CommonProcessDao.SelectCamImageDetail( stationCdList, productModelCd, serial );
        }
        #endregion

        #region 電子チェックシート検索DAO
        /// <summary>
        /// 詳細画面トラクタ工程(電子チェックシート：検査ヘッダ)情報取得(TT_SQ_SIS_INS_RESULT)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  productModelCd             … 型式
        ///  serial                     … 機番
        ///  lineCd                     … ラインコード
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectELCheckHeader( string productModelCd, string serial, string lineCd = null ) {

            KTBindParameters bindParam = new KTBindParameters();
            string statementId = null;
            DataTable dtRet = new DataTable();

            //************************************
            // ヘッダ検索
            //************************************
            //パラメータの設定
            bindParam.Add( "paramProductModelCd", productModelCd );     //型式
            bindParam.Add( "paramSerial", serial );                     //機番
            bindParam.Add( "paramLineCd", lineCd );                     //ラインコード

            //SELECT実行
            statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectELCheckHeader" );
            dtRet = SelectData( statementId, bindParam );

            if ( null != dtRet && dtRet.Rows.Count > 0 ) {
                foreach ( DataRow row in dtRet.Rows ) {
                    lineCd = StringUtils.ToString( row["LINE_CD"] );
                    row["lastProc"] = SelectELCheckLastProc( productModelCd, serial, row.Field<string>( "INS_START_DT" ), lineCd );
                }
                dtRet.AcceptChanges();
            }

            return dtRet;
        }

        /// <summary>
        /// 詳細画面トラクタ工程(電子チェックシート：検査情報)情報取得(TT_SQ_SIS_INS_RESULT_DTL)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  productModelCd             … 型式
        ///  serial                     … 機番
        ///  lineCd                     … ラインコード
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectELCheckInfo( string productModelCd, string serial, string lineCd ) {

            KTBindParameters bindParam = new KTBindParameters();
            string statementId = null;

            //************************************
            // 検査詳細情報取得
            //************************************
            //パラメータの設定
            bindParam.Add( "paramProductModelCd", productModelCd );     //型式
            bindParam.Add( "paramSerial", serial );                     //機番
            bindParam.Add( "paramLineCd", lineCd );                     //ラインコード

            //SELECT実行
            statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectELCheckInfo" );

            return SelectData( statementId, bindParam );
        }


        /// <summary>
        /// 詳細画面トラクタ工程(電子チェックシート：不具合一覧)情報取得(TT_SQ_SIS_INS_NG)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  productModelCd             … 型式
        ///  serial                     … 機番
        ///  lineCd                     … ラインコード
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectNGList( string productModelCd, string serial, string lineCd ) {

            KTBindParameters bindParam = new KTBindParameters();
            DataTable dtRet = new DataTable();
            string statementId = null;

            //************************************
            // 不具合一覧取得
            //************************************
            //パラメータの設定
            bindParam.Add( "paramProductModelCd", productModelCd );     //型式
            bindParam.Add( "paramSerial", serial );                     //機番
            bindParam.Add( "paramLineCd", lineCd );                     //ラインコード

            //SELECT実行
            statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectNGList" );

            dtRet = SelectData( statementId, bindParam );

            foreach ( DataRow row in dtRet.Rows ) {
                //
                int intGuaranteeNo = NumericUtils.ToInt( row["GUARANTEE_NO"] );
                if ( intGuaranteeNo.Equals( 0 ) ) {
                    row["GUARANTEE_NO"] = "-";
                }
            }


            return dtRet;
        }

        /// <summary>
        /// 詳細画面トラクタ工程(電子チェックシート：不具合画像リスト)情報取得(TT_SQ_SIS_INS_RESULT_IMG)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  productModelCd             … 型式
        ///  serial                     … 機番
        ///  lineCd                     … ラインコード
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectNGImg( string productModelCd, string serial, string lineCd ) {

            KTBindParameters bindParam = new KTBindParameters();
            string statementId = null;

            //************************************
            // ヘッダ検索
            //************************************
            //パラメータの設定
            bindParam.Add( "paramProductModelCd", productModelCd );     //型式
            bindParam.Add( "paramSerial", serial );                     //機番
            bindParam.Add( "paramLineCd", lineCd );                     //ラインコード

            //SELECT実行
            statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectELNGImg" );

            return SelectData( statementId, bindParam );
        }

        /// <summary>
        /// 詳細画面トラクタ工程(電子チェックシート：検査画像リスト)情報取得(TT_SQ_SIS_INS_RESULT_IMG)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  productModelCd             … 型式
        ///  serial                     … 機番
        ///  lineCd                     … ラインコード
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectChkImg( string productModelCd, string serial, string lineCd ) {

            KTBindParameters bindParam = new KTBindParameters();
            string statementId = null;

            //************************************
            // ヘッダ検索
            //************************************
            //パラメータの設定
            bindParam.Add( "paramProductModelCd", productModelCd );     //型式
            bindParam.Add( "paramSerial", serial );                     //機番
            bindParam.Add( "paramLineCd", lineCd );                     //ラインコード

            //SELECT実行
            statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectELChkImg" );

            return SelectData( statementId, bindParam );
        }
        /// <summary>
        /// 詳細画面トラクタ工程(電子チェックシート：最終工程)情報取得
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  productModelCd             … 型式
        ///  serial                     … 機番
        ///  lineCd                     … ラインコード
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static string SelectELCheckLastProc( string productModelCd, string serial, string inspectionDate, string lineCd = null ) {

            KTBindParameters bindParam = new KTBindParameters();
            string statementId = null;
            DataTable dtRet = new DataTable();
            string lastProc = "";

            //************************************
            // ヘッダ検索
            //************************************
            //パラメータの設定
            bindParam.Add( "paramProductModelCd", productModelCd );     //型式
            bindParam.Add( "paramSerial", serial );                     //機番
            bindParam.Add( "paramLineCd", lineCd );                     //ラインコード
            bindParam.Add( "paramInspectDt", DateTime.Parse( inspectionDate ) ); //検査日

            //SELECT実行
            statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectELCheckLastProc" );
            dtRet = SelectData( statementId, bindParam );

            if ( null != dtRet && dtRet.Rows.Count > 0 ) {
                lastProc = StringUtils.ToString( dtRet.Rows[0]["lastProc"] );
            }

            return lastProc;
        }
        /// <summary>
        /// ラインコードと検査開始日から課情報を取得
        /// </summary>
        /// <param name="lineCd"></param>
        /// <param name="inspectionStartDate"></param>
        /// <returns></returns>
        public static DataTable SelectSection( string lineCd, DateTime inspectionStartDate ) {

            KTBindParameters bindParam = new KTBindParameters();
            string statementId = null;

            //パラメータの設定
            bindParam.Add( "lineCd", lineCd );                     //ラインコード
            bindParam.Add( "insStartDate", inspectionStartDate );  //検査開始日

            //SELECT実行
            statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectSection" );

            return SelectData( statementId, bindParam );
        }


        #endregion

        #region イメージチェックシート検索DAO
        /// <summary>
        /// 詳細画面トラクタ工程(イメージチェックシート：検査情報ヘッダ)情報取得(TT_SQ_ICS_INS_RESULT)
        /// </summary>
        /// <param name="productModelCd">型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="lineCd">ラインコード</param>
        /// <remarks></remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectImgCheckHeader( string productModelCd, string serial, string lineCd = null ) {

            //共通工程呼出
            return CommonProcessDao.SelectImgCheckHeader( productModelCd, serial, Defines.ListDefine.ProductKind.Tractor, lineCd );
        }
        #endregion

        #region 刻印
        /// <summary>
        /// 詳細画面トラクタ工程(刻印：ミッションケース刻印)情報取得(TT_SQ_PRINTOUT_RECORD)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  productModelCd             … 型式
        ///  serial                     … 機番
        ///  stationCd                  … ステーションコード
        ///  printKbn                   …印刷区分
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectPrintSheel( string productModelCd, string serial, string stationCd, string printKbn ) {

            KTBindParameters bindParam = new KTBindParameters();
            string statementId = null;

            //************************************
            // ヘッダ検索
            //************************************
            //パラメータの設定
            bindParam.Add( "paramProductModelCd", productModelCd );     //型式
            bindParam.Add( "paramSerial", serial );                     //機番
            //bindParam.Add( "stationCd", stationCd );                    //ステーション(型式-機番で検索可能なため、コメントアウト(2016.01.19))
            bindParam.Add( "printKbn", printKbn );                      //印刷区分

            //SELECT実行
            statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectPrintSheel" );

            return SelectData( statementId, bindParam );
        }
        #endregion

        #region NutRunner
        /// <summary>
        /// 詳細画面トラクタ工程(ナットランナー)情報取得
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  productModelCd             … 型式
        ///  serial                     … 機番
        ///  stationCd                     … ステーションコード
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectNutRunner( string productModelCd, string serial ) {

            KTBindParameters bindParam = new KTBindParameters();
            string statementId = null;

            //************************************
            // ヘッダ検索
            //************************************
            //パラメータの設定
            bindParam.Add( "paramProductModelCd", productModelCd );     //型式
            bindParam.Add( "paramSerial", serial );                     //機番

            //SELECT実行
            statementId = GetFullStatementIdForLibrary( "EngineProcess", "SelectNutRunner" );

            return SelectCreData( statementId, bindParam );
        }
        #endregion

        #region 光軸検査
        /// <summary>
        /// 詳細画面トラクタ工程(光軸検査)情報取得
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  productModelCd             … 型式
        ///  serial                     … 機番
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectOptaxis( string productModelCd, string serial ) {

            KTBindParameters bindParam = new KTBindParameters();
            string statementId = null;

            //パラメータの設定
            bindParam.Add( "paramProductModelCd", productModelCd );     //型式
            bindParam.Add( "paramSerial", serial );                     //機番

            //SELECT実行
            statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectOptaxisDetail" );

            return SelectData( statementId, bindParam );
        }
        #endregion

        #region 関所
        /// <summary>
        /// 詳細画面トラクタ工程(関所：工程ヘッダ)情報取得(TT_SI_PSP_WORK_RESULT)
        /// </summary>
        /// <param name="productModelCd">型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="lineCd">ラインコード</param>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectCheckPointHeader( string productModelCd, string serial, string lineCd ) {

            KTBindParameters bindParam = new KTBindParameters();
            string statementId = null;
            DataTable dtRet = new DataTable();

            //************************************
            // ヘッダ検索
            //************************************
            //パラメータの設定
            bindParam.Add( "paramProductModelCd", productModelCd );     //型式
            bindParam.Add( "paramSerial", serial );                     //機番
            bindParam.Add( "paramLineCd", lineCd );                     //ラインコード

            //SELECT実行
            statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectCheckPointHeader" );
            dtRet = SelectData( statementId, bindParam );

            return dtRet;
        }

        /// <summary>
        /// 詳細画面トラクタ工程(関所：工程作業実績)情報取得(TT_SI_PSP_WORK_RESULT)
        /// </summary>
        /// <param name="productModelCd">型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="stationCd">ステーションコード</param>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectCheckPoint( string productModelCd, string serial, string stationCd ) {

            KTBindParameters bindParam = new KTBindParameters();
            string statementId = null;
            DataTable dtRet = new DataTable();

            //************************************
            // ヘッダ検索
            //************************************
            //パラメータの設定
            bindParam.Add( "paramProductModelCd", productModelCd );     //型式
            bindParam.Add( "paramSerial", serial );                     //機番
            bindParam.Add( "paramStationCd", stationCd );               //ステーションコード

            //SELECT実行
            statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectCheckPoint" );
            dtRet = SelectData( statementId, bindParam );

            return dtRet;
        }

        /// <summary>
        /// 詳細画面トラクタ工程(関所：工程作業実績)情報取得(TT_SI_PSP_WORK_RESULT)
        /// </summary>
        /// <param name="productModelCd">型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="lineCd">ラインコード</param>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectCheckPointForExcel( string productModelCd, string serial, string lineCd ) {

            KTBindParameters bindParam = new KTBindParameters();
            string statementId = null;
            DataTable dtRet = new DataTable();

            //************************************
            // ヘッダ検索
            //************************************
            //パラメータの設定
            bindParam.Add( "paramProductModelCd", productModelCd );     //型式
            bindParam.Add( "paramSerial", serial );                     //機番
            bindParam.Add( "paramLineCd", lineCd );                     //ラインコード

            //SELECT実行
            statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectCheckPointForExcel" );
            dtRet = SelectData( statementId, bindParam );

            return dtRet;
        }
        #endregion

        #region トラクタ走行検査
        /// <summary>
        /// 詳細画面トラクタ工程(トラクタ走行検査)情報取得
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  productModelCd             … 型式
        ///  serial                     … 機番
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectTractorAll( string productModelCd, string serial ) {

            KTBindParameters bindParam = new KTBindParameters();
            string statementId = null;

            //パラメータの設定
            bindParam.Add( "paramProductModelCd", productModelCd );     //型式
            bindParam.Add( "paramSerial", serial );                     //機番

            //SELECT実行
            statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectTractorAllDetail" );

            return PicDao.GetInstance().Select( statementId, bindParam );
        }


        #endregion

        #region DPF機番情報取得
        /// <summary>
        /// 明細画面：DPF機番情報登録用順序テーブル検索(MS_SAGYO_KEEP)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectSagyoKeepList( Dictionary<string, object> condition, int maxRecordCount ) {

            string productKind = DataUtils.GetDictionaryStringVal( condition, "productKind" );
            string modelCd = DataUtils.GetModelCd( DataUtils.GetDictionaryStringVal( condition, "modelCd" ) );  //型式コード
            string modelNm = DataUtils.GetDictionaryStringVal( condition, "modelNm" );                          //型式名
            string serial = DataUtils.GetDictionaryStringVal( condition, "serial" );    //機番
            string idno = DataUtils.GetDictionaryStringVal( condition, "idno" );                                //IDNO
            string lineCd = DataUtils.GetDictionaryStringVal( condition, "lineCd" );                            //ライン
            string st = DataUtils.GetDictionaryStringVal( condition, "st" );                                    //ステーション
            DateTime dtFrom = DataUtils.GetDictionaryDateVal( condition, "dateFrom" );                //日付(FROM)
            DateTime dtTo = DataUtils.GetDictionaryDateVal( condition, "dateTo" );                    //日付(TO)

            /*
            string paramDtFrom = null;
            string paramDtTo = null;

            //完成予定日変換
            if ( StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( condition, "dateFrom" ) ) ) {
                paramDtFrom = dtFrom.ToShortDateString();
            }
            if ( StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( condition, "dateTo" ) ) ) {
                paramDtTo = dtTo.ToShortDateString();
            }
            */

            //加工日変換
            DateTime? paramDtFrom = GetSearchFromDate( dtFrom );
            DateTime? paramDtTo = GetSearchToDate( dtTo );


            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //************************************
            // 詳細検索
            //************************************
            //SQL

            //詳細情報検索
            bindParam.Add( "productKind", productKind );        //製品種別
            bindParam.Add( "modelCd", modelCd );                //型式
            bindParam.Add( "modelNm", modelNm );                //型式名
            bindParam.Add( "serial", serial );                  //機番番号
            bindParam.Add( "idno", idno );                      //IDNO
            bindParam.Add( "lineCd", lineCd );                  //ライン
            bindParam.Add( "st", st );                          //ステーション
            bindParam.Add( "dtCompFrom", paramDtFrom );         //完成予定日From
            bindParam.Add( "dtCompTo", paramDtTo );             //完成予定日To

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectSagyoKeepList" );
            return SelectData( statementId, bindParam, maxRecordCount );
        }

        #endregion

        #region 検査ベンチ
        /// <summary>
        /// 詳細画面トラクタ工程(検査ベンチ)情報取得
        /// </summary>
        /// <param name="productModelCd">型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="lineCd">ラインコード</param>
        /// <remarks></remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectTestBench( string productModelCd, string serial, string lineCd = null ) {

            KTBindParameters bindParam = new KTBindParameters();
            string statementId = null;

            // パラメータの設定
            // 型式
            bindParam.Add( "paramProductModelCd", productModelCd );
            // 機番
            bindParam.Add( "paramSerial", serial );

            // SELECT実行
            statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectTractorTestBenchDetail" );

            return SelectData( statementId, bindParam );
        }
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

        #region AI画像解析
        /// <summary>
        /// 詳細画面トラクタ工程(AI画像解析)情報取得
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  stationCdList              … トラクタAI画像解析用ステーション
        ///  productModelCd             … 型式
        ///  serial                     … 機番
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectTractorAiImageDetail( string productModelCd, string serial ) {
            //トラクタAI画像解析用ステーション取得(設定ファイル)
            List<string> stationCdList = WebAppInstance.GetInstance().Config.ApplicationInfo.tractorAiImageStationCdList;

            //共通工程呼出
            return CommonProcessDao.SelectAiImageDetail( stationCdList, productModelCd, serial );
        }
        #endregion

        /// <summary>
        /// データ検索処理
        /// </summary>
        /// <param name="statementId">ステートメントID</param>
        /// <param name="bindParam">パラメータ</param>
        /// <param name="maxRecordCount">最大出力件数</param>
        /// <returns>出力結果DataTable</returns>
        private static DataTable SelectData( string statementId, KTBindParameters bindParam, int maxRecordCount = Int32.MaxValue ) {
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
        /// <summary>
        /// データ検索処理(Cre)
        /// </summary>
        /// <param name="statementId">ステートメントID</param>
        /// <param name="bindParam">パラメータ</param>
        /// <param name="maxRecordCount">最大出力件数</param>
        /// <returns>出力結果DataTable</returns>
        private static DataTable SelectCreData( string statementId, KTBindParameters bindParam, int maxRecordCount = Int32.MaxValue ) {
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
        /// <summary>
        /// 速度検査情報取得
        /// </summary>
        /// <param name="productModelCd"></param>
        /// <param name="serial"></param>
        /// <returns></returns>
        public static DataTable SelectMaxSpeedInsList( string productModelCd, string serial, string inspectionSeq ) {
            KTBindParameters bindParam = new KTBindParameters();
            string statementId = null;

            //パラメータの設定
            bindParam.Add( "paramProductModelCd", productModelCd );    // 型式
            bindParam.Add( "paramSerial", serial );                    // 機番
            bindParam.Add( "inspectionSeq", inspectionSeq );           // 検査連番

            //SELECT実行
            statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "MaxSpeedInsList" );
            return PicDao.GetInstance().Select( statementId, bindParam );
        }

        /// <summary>
        /// エクセル出力用一覧
        /// </summary>
        /// <param name="productModelCd"></param>
        /// <param name="serial"></param>
        /// <returns></returns>
        public static DataTable SelectExcelList( string productModelCd, string serial ) {
            KTBindParameters bindParam = new KTBindParameters();
            string statementId = null;

            //パラメータの設定
            bindParam.Add( "paramProductModelCd", productModelCd );     //型式
            bindParam.Add( "paramSerial", serial );                     //機番

            //SELECT実行
            statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "ExcelList" );
            return PicDao.GetInstance().Select( statementId, bindParam );
        }
    }
}