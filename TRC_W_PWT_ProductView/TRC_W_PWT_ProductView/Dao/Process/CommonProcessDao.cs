using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using KTFramework.Common.Dao;
using KTFramework.Dao;
using KTFramework.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Dao.Com;

namespace TRC_W_PWT_ProductView.Dao.Process {
    /// <summary>
    /// トラクタ/エンジン共通工程検索DAO
    /// </summary>
    public class CommonProcessDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "CommonProcess";

        /// <summary>
        /// 検索パラメータクラス
        /// </summary>
        public class SearchParameter {
            /// <summary>ステーションコード トラクタ=212002 エンジン=306205</summary>
            public List<string> paramStationCdList = null;
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
            /// <summary>[共通]製品種別コード</summary>
            public string paramProductKindCd = null;
        }

        #region 一覧検索DAO
        /// <summary>
        /// 一覧画面工程(品質画像証跡)情報検索(TT_SQ_CAMERA_IMAGE_STORAGE)
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
            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //工程検索条件
            bindParam.Add( "paramStationCdList", param.paramStationCdList );                    //対象ステーションリスト
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
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectCamImageList" );
            return SelectPicData( statementId, bindParam, maxRecordCount );
        }

        #region イメージチェックシート
        /// <summary>
        /// 一覧画面工程(イメージチェックシート)情報検索(TT_SQ_ICS_INS_RESULT)
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
        ///  paramProductKindCd      … 製品種別コード
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectImgCheckList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
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
            bindParam.Add( "paramProductKindCd", param.paramProductKindCd );                    //製品種別コード

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectImgCheckList" );
            DataTable dtRet = SelectPicData( statementId, bindParam, maxRecordCount );

            if ( null != dtRet && 0 < dtRet.Rows.Count ) {
                foreach ( DataRow row in dtRet.Rows ) {
                    string modelCd = StringUtils.ToString( row["productModelCd"] );
                    string serial = StringUtils.ToString( row["serial6"] );
                    string lineCd = StringUtils.ToString( row["lineCd"] );

                    row["lastProc"] = SelectImgCheckLastProc( modelCd, serial, row.Field<string>( "INSPECTIONDT" ), lineCd );
                }
            }

            return dtRet;
        }
        #endregion

        #region AI画像解析
        /// <summary>
        /// 一覧画面工程(AI画像解析)情報検索(TT_SQ_IMG_ANL_RESULT)
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
        ///  paramProductKindCd      … 製品種別コード
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectAiImageList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
            // パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            // 工程検索条件
            bindParam.Add( "paramStationCdList", param.paramStationCdList );                    //対象ステーションリスト
            bindParam.Add( "paramSerialList", param.paramSerialList );                          //型式/機番リスト
            bindParam.Add( "paramProductModelCd", param.paramProductModelCd );                  //生産型式コード(前方一致)
            if ( null != param.paramInspectionDtFrom ) {
                bindParam.Add( "paramInspectionDtFrom", param.paramInspectionDtFrom.Value );    //測定日(FROM)
            }
            if ( null != param.paramInspectionDtTo ) {
                bindParam.Add( "paramInspectionDtTo", param.paramInspectionDtTo.Value );        //測定日(TO)
            }
            bindParam.Add( "paramProductModelCdList", param.paramProductModelCdList );          //生産型式コードリスト

            // SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectAiImageList" );
            DataTable dtTemp = SelectPicData( statementId, bindParam, maxRecordCount );
            DataTable dtRet = GetResult( dtTemp );

            if ( ( null != dtRet ) && ( 0 < dtRet.Rows.Count ) ) {
                // データあり
                foreach ( DataRow row in dtRet.Rows ) {
                    if ( ObjectUtils.IsNotNull( row["pathByWork"] ) == true ) {
                        // ワーク別格納先管理T.Gドライブ格納先がNULLでない
                        row["gDrivePath"] = row["pathByWork"];
                    } else if ( ObjectUtils.IsNotNull( row["pathByDay"] ) == true ) {
                        // 日別格納先管理T.Gドライブ格納先がNULLでない
                        row["gDrivePath"] = row["pathByDay"];
                    }
                }
            }

            return MasterList.SetStationNm( dtRet );
        }

        /// <summary>
        /// 一覧画面工程(AI画像解析：合否)情報検索(TT_SQ_IMG_ANL_RESULT)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <remarks></remarks>
        /// <returns>合否</returns>
        public static DataTable GetResult( DataTable tblData ) {
            DataTable dtRet = tblData.Clone();

            if ( ( null != tblData ) && ( 0 < tblData.Rows.Count ) ) {
                // データあり
                // 結果をstationCd、stationCd、serial6でグループ化
                var results = tblData.AsEnumerable().ToList().GroupBy( x => new { stationCd = x["stationCd"], productModelCd = x["productModelCd"], serial6 = x["serial6"] } );

                foreach ( var items in results ) {
                    // グループごとの処理
                    // 合否判定
                    string result = "合格";
                    foreach ( var item in items ) {
                        // 行ごとの処理
                        if ( StringUtils.ToString( item["result"] ) == "0" ) {
                            // NGが存在するので「不合格」を設定して抜ける
                            result = "不合格";
                            break;
                        } else if ( StringUtils.ToString( item["result"] ) == "2" ) {
                            // スキップされた項目があるので「未完了」を設定
                            result = "未完了";
                        }
                    }

                    // 結果をテーブルに追加
                    DataRow temp = items.First();
                    temp["result"] = result;
                    dtRet.ImportRow( temp );
                }
            }

            return dtRet;
        }
        #endregion

        #endregion

        #region 詳細検索DAO
        /// <summary>
        /// 詳細画面工程(品質画像証跡)情報検索(TT_SQ_CAMERA_IMAGE_STORAGE)
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
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectCamImageDetail( List<string> stationCdList, string productModelCd, string serial ) {
            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //工程検索条件
            bindParam.Add( "paramStationCdList", stationCdList );                               //対象ステーションリスト
            bindParam.Add( "paramSerial", new SerialParam( productModelCd, serial ) );            //型式/機番リスト

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectCamImageDetail" );
            return SelectPicData( statementId, bindParam, Int32.MaxValue );
        }

        #region イメージチェックシート詳細検索DAO
        /// <summary>
        /// 詳細画面工程[イメージチェックシート：検査ヘッダ]情報取得(TT_SQ_ICS_INS_RESULT)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="productKindCd">製品種別コード</param>
        /// <param name="lineCd">ラインコード</param>
        /// <remarks></remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectImgCheckHeader( string productModelCd, string serial, string productKindCd, string lineCd = null ) {

            KTBindParameters bindParam = new KTBindParameters();
            string statementId = null;
            DataTable dtRet = new DataTable();

            //************************************
            // ヘッダ検索
            //************************************
            //パラメータの設定
            bindParam.Add( "paramProductModelCd", productModelCd );         //生産型式コード
            bindParam.Add( "paramSerial", serial );                         //機番
            bindParam.Add( "paramProductKindCd", productKindCd );           //製品種別コード
            bindParam.Add( "paramLineCd", lineCd );                         //ラインコード

            //SELECT実行
            statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectImgCheckHeader" );
            dtRet = SelectPicData( statementId, bindParam );

            if ( null != dtRet && 0 < dtRet.Rows.Count ) {
                foreach ( DataRow row in dtRet.Rows ) {
                    lineCd = StringUtils.ToString( row["LINE_CD"] );
                    //最終検査工程取得
                    row["lastProc"] = SelectImgCheckLastProc( productModelCd, serial, row.Field<string>( "INS_START_DT" ), lineCd );
                }
                dtRet.AcceptChanges();
            }

            return dtRet;
        }

        /// <summary>
        /// 詳細画面工程[イメージチェックシート：検査情報]情報取得(TT_SQ_ICS_INS_RESULT_DTL)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="lineCd">ラインコード</param>
        /// <remarks></remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectIcsChkInfo( string productModelCd, string serial, string lineCd ) {

            KTBindParameters bindParam = new KTBindParameters();
            string statementId = null;

            //************************************
            // 検査詳細情報取得
            //************************************
            //パラメータの設定
            bindParam.Add( "paramProductModelCd", productModelCd );         //型式
            bindParam.Add( "paramSerial", serial );                         //機番
            bindParam.Add( "paramLineCd", lineCd );                         //ラインコード

            //SELECT実行
            statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectIcsChkInfo" );

            return SelectPicData( statementId, bindParam );
        }

        /// <summary>
        /// 詳細画面工程[イメージチェックシート：不具合一覧]情報取得(TT_SQ_ICS_INS_NG)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="lineCd">ラインコード</param>
        /// <remarks></remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectIcsNGList( string productModelCd, string serial, string lineCd ) {

            KTBindParameters bindParam = new KTBindParameters();
            DataTable dtRet = new DataTable();
            string statementId = null;

            //************************************
            // 不具合一覧取得
            //************************************
            //パラメータの設定
            bindParam.Add( "paramProductModelCd", productModelCd );         //型式
            bindParam.Add( "paramSerial", serial );                         //機番
            bindParam.Add( "paramLineCd", lineCd );                         //ラインコード

            //SELECT実行
            statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectIcsNGList" );

            dtRet = SelectPicData( statementId, bindParam );

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
        /// 詳細画面工程[イメージチェックシート：不具合画像リスト]情報取得(TT_SQ_ICS_INS_RESULT_IMG)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="lineCd">ラインコード</param>
        /// <remarks></remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectIcsNGImg( string productModelCd, string serial, string lineCd ) {

            KTBindParameters bindParam = new KTBindParameters();
            string statementId = null;

            //************************************
            // 不具合画像取得
            //************************************
            //パラメータの設定
            bindParam.Add( "paramProductModelCd", productModelCd );         //型式
            bindParam.Add( "paramSerial", serial );                         //機番
            bindParam.Add( "paramLineCd", lineCd );                         //ラインコード

            //SELECT実行
            statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectIcsNGImg" );

            return SelectPicData( statementId, bindParam );
        }

        /// <summary>
        /// 詳細画面工程[イメージチェックシート：検査画像リスト]情報取得(TT_SQ_ICS_INS_RESULT_IMG)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="lineCd">ラインコード</param>
        /// <param name="param">検索パラメータ</param>
        /// <remarks></remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectIcsChkImg( string productModelCd, string serial, string lineCd ) {

            KTBindParameters bindParam = new KTBindParameters();
            string statementId = null;

            //************************************
            // 検査画像取得
            //************************************
            //パラメータの設定
            bindParam.Add( "paramProductModelCd", productModelCd );         //型式
            bindParam.Add( "paramSerial", serial );                         //機番
            bindParam.Add( "paramLineCd", lineCd );                         //ラインコード

            //SELECT実行
            statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectIcsChkImg" );

            return SelectPicData( statementId, bindParam );
        }

        /// <summary>
        /// 一覧/詳細画面工程[イメージチェックシート：最終工程]情報取得
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="inspectionDt">検査日</param>
        /// <param name="lineCd">ラインコード</param>
        /// <remarks></remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static string SelectImgCheckLastProc( string productModelCd, string serial, string inspectionDt, string lineCd = null ) {

            KTBindParameters bindParam = new KTBindParameters();
            string statementId = null;
            DataTable dtRet = new DataTable();
            string lastProc = "";

            //************************************
            // 最終検査工程
            //************************************
            //パラメータの設定
            bindParam.Add( "paramProductModelCd", productModelCd );                 //型式
            bindParam.Add( "paramSerial", serial );                                 //機番
            bindParam.Add( "paramLineCd", lineCd );                                 //ラインコード
            bindParam.Add( "paramInspectDt", DateTime.Parse( inspectionDt ) );      //検査日

            //SELECT実行
            statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectImgCheckLastProc" );
            dtRet = SelectPicData( statementId, bindParam );

            if ( null != dtRet && 0 < dtRet.Rows.Count ) {
                lastProc = StringUtils.ToString( dtRet.Rows[0]["lastProc"] );
            }

            return lastProc;
        }

        /// <summary>
        /// ラインコードと検査開始日から課情報を取得
        /// </summary>
        /// <param name="lineCd">ラインコード</param>
        /// <param name="inspectionStartDt">検査開始日</param>
        /// <returns></returns>
        public static DataTable SelectSection( string lineCd, DateTime inspectionStartDt ) {

            KTBindParameters bindParam = new KTBindParameters();
            string statementId = null;

            //パラメータの設定
            bindParam.Add( "lineCd", lineCd );                      //ラインコード
            bindParam.Add( "insStartDate", inspectionStartDt );     //検査開始日

            //SELECT実行
            statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectSection" );

            return SelectPicData( statementId, bindParam );
        }
        #endregion

        /// <summary>
        /// 詳細画面工程(AI画像解析)情報検索(TT_SQ_IMG_ANL_RESULT)
        /// </summary>
        /// <param name="stationCdList">対象ステーションリスト</param>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectAiImageDetail( List<string> stationCdList, string productModelCd, string serial ) {
            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //工程検索条件
            bindParam.Add( "paramStationCdList", stationCdList );                                               //対象ステーションリスト
            bindParam.Add( "paramSerial", new SerialParam( productModelCd, DataUtils.GetSerial6( serial ) ) );  //型式/機番リスト

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectAiImageDetail" );
            DataTable dtRet = SelectPicData( statementId, bindParam, Int32.MaxValue );

            return MasterList.SetStationNm( dtRet );
        }

        #endregion

        #region 製品別通過実績取得

        /// <summary>
        /// 製品別通過実績(工程)
        /// </summary>
        /// <param name="modelCd">型式CD</param>
        /// <param name="serial">機番</param>
        /// <param name="assemblyPatternCd">組立パターン</param>
        /// <returns></returns>
        public static DataTable SelectImaDokoProcess( string modelCd, string serial, string assemblyPatternCd ) {
            DataTable dtRet = new DataTable();

            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //工程検索条件
            bindParam.Add( "modelCd", modelCd );
            bindParam.Add( "serial", serial );
            bindParam.Add( "assemblyPatternCd", assemblyPatternCd );

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectImaDokoProcess" );
            dtRet = SelectPicData( statementId, bindParam, Int32.MaxValue );

            return dtRet;

        }
        /// <summary>
        /// 製品別通過実績(実績)
        /// </summary>
        /// <param name="modelCd">型式CD</param>
        /// <param name="serial">機番</param>
        /// <param name="assemblyPatternCd">組立パターン</param>
        /// <returns></returns>
        public static DataTable SelectImaDokoResult( string modelCd, string serial, string assemblyPatternCd ) {
            DataTable dtRet = new DataTable();

            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //工程検索条件
            bindParam.Add( "modelCd", modelCd );
            bindParam.Add( "serial", serial );
            bindParam.Add( "assemblyPatternCd", assemblyPatternCd );

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectImaDokoResult" );
            dtRet = SelectPicData( statementId, bindParam, Int32.MaxValue );

            return dtRet;
        }

        #endregion

        /// <summary>
        /// データ検索処理
        /// </summary>
        /// <param name="statementId">ステートメントID</param>
        /// <param name="bindParam">パラメータ</param>
        /// <param name="maxRecordCount">最大出力件数</param>
        /// <returns>出力結果DataTable</returns>
        private static DataTable SelectPicData( string statementId, KTBindParameters bindParam, int maxRecordCount = Int32.MaxValue ) {
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

        #region 共用工程詳細
        /// <summary>
        /// 工程作業最新来歴取得処理
        /// </summary>
        /// <param name="modelCd">型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="lineCd">ラインコード</param>
        /// <param name="processCd">工程コード</param>
        /// <returns>工程作業情報</returns>
        public static DataTable SelectNewestProcessWorkHistory( string modelCd, string serial, string lineCd, string processCd ) {
            // パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();
            bindParam.Add( "modelCd", modelCd );
            bindParam.Add( "serial", DataUtils.GetSerial6( serial ) );
            bindParam.Add( "lineCd", lineCd );
            bindParam.Add( "processCd", processCd );
            // SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectNewestProcessWorkHistory" );
            return PicDao.GetInstance().Select( statementId, bindParam );
        }

        /// <summary>
        /// 計測項目情報取得処理
        /// </summary>
        /// <param name="lineCd">ラインコード</param>
        /// <param name="processCd">工程コード</param>
        /// <param name="workCd">作業コード</param>
        /// <returns>計測項目情報</returns>
        public static DataTable SelectMeasuringItemInfo( string lineCd, string processCd, string workCd ) {
            // パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();
            bindParam.Add( "lineCd", lineCd );
            bindParam.Add( "processCd", processCd );
            bindParam.Add( "workCd", workCd );
            // SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectMeasuringItemInfo" );
            return PicDao.GetInstance().Select( statementId, bindParam );
        }

        /// <summary>
        /// 作業来歴情報取得処理
        /// </summary>
        /// <param name="modelCd">型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="lineCd">ラインコード</param>
        /// <param name="processCd">工程コード</param>
        /// <param name="workCd">作業コード</param>
        /// <param name="attributeCdList">トレーサビリティ項目コードリスト</param>
        /// <returns>計測結果情報</returns>
        public static DataTable SelectWorkHistoryInfo( string modelCd, string serial, string lineCd, string processCd, string workCd, List<string> attributeCdList ) {
            // パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();
            bindParam.Add( "modelCd", modelCd );
            bindParam.Add( "serial", DataUtils.GetSerial6( serial ) );
            bindParam.Add( "lineCd", lineCd );
            bindParam.Add( "processCd", processCd );
            bindParam.Add( "workCd", workCd );
            bindParam.Add( "attributeCdList", attributeCdList ?? new List<string>() );
            // SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectWorkHistoryInfo" );
            return PicDao.GetInstance().Select( statementId, bindParam );
        }
        #endregion

        #region トレーサビリティ工程区分マスタ取得
        /// <summary>
        /// トレーサビリティ工程区分マスタ取得
        /// </summary>
        /// <returns>工程区分情報</returns>
        public static DataTable SelectAllProcessClass() {

            // SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectAllProcessClass" );
            return PicDao.GetInstance().Select( statementId );
        }
        #endregion
    }
}