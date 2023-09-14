using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using KTFramework.Common.Dao;
using KTFramework.Dao;
using KTFramework.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Dao.Com;

namespace TRC_W_PWT_ProductView.Dao.Parts {
    /// <summary>
    /// トラクタ部品検索DAO
    /// </summary>
    public class TractorPartsDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "TractorParts";

        /// <summary>
        /// 検索パラメータクラス
        /// </summary>
        public class SearchParameter {
            /// <summary>[共通]型式/機番</summary>
            public List<SerialParam> paramSerialList = null;
            /// <summary>[共通]生産型式コード(前方一致)</summary>
            public string paramProductModelCd = null;
            /// <summary>部品区分コード[基幹部品]</summary>
            public string paramPartsCd = null;
            /// <summary>部品品番(前方一致)</summary>
            public string paramPartsNum = null;
            /// <summary>部品機番</summary>
            public string paramPartsSerial = null;
            /// <summary>組付日(FROM)</summary>
            public DateTime? paramInstallDtFrom = null;
            /// <summary>組付日(TO)</summary>
            public DateTime? paramInstallDtTo = null;
            /// <summary>[共通]生産型式コードリスト(型式名から検索したリスト)</summary>
            public List<string> paramProductModelCdList = null;
        }

        #region 一覧検索DAO
        /// <summary>
        /// 一覧画面トラクタ部品(WiFiECU)情報検索(PAIRRSLT_F)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  paramSerialList         … 型式/機番リスト
        ///  paramProductModelCd     … 生産型式コード(前方一致)
        ///  paramPartsNum           … 部品品番(前方一致)
        ///  paramPartsSerial        … 部品機番
        ///  paramInstallDtFrom      … 組付日(FROM)
        ///  paramInstallDtTo        … 組付日(TO)
        ///  paramProductModelCdList … 生産型式コードリスト
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectWiFiEcuList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //部品検索条件
            bindParam.Add( "paramSerialList", param.paramSerialList );                          //型式/機番リスト
            bindParam.Add( "paramProductModelCd", param.paramProductModelCd );                  //生産型式コード(前方一致)
            bindParam.Add( "paramPartsNum", param.paramPartsNum );                              //部品品番(前方一致)
            bindParam.Add( "paramPartsSerial", param.paramPartsSerial );                        //部品機番
            if ( null != param.paramInstallDtFrom ) {
                bindParam.Add( "paramInstallDtFrom", param.paramInstallDtFrom.Value );          //組付日(FROM)
            }
            if ( null != param.paramInstallDtTo ) {
                bindParam.Add( "paramInstallDtTo", param.paramInstallDtTo.Value );              //組付日(TO)
            }
            bindParam.Add( "paramProductModelCdList", param.paramProductModelCdList );          //生産型式コードリスト

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectTractorWifiEcuList" );
            return SelectPicData( statementId, bindParam, maxRecordCount );
        }
        /// <summary>
        /// 詳細画面トラクタ部品(銘板ラベル)情報検索
        /// </summary>
        /// <param name="param"></param>
        /// <param name="maxRecordCount"></param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectTractorNameplateList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
            KTBindParameters bindParam = new KTBindParameters();

            //部品検索条件
            bindParam.Add( "paramSerialList", param.paramSerialList );                          //型式/機番リスト
            bindParam.Add( "paramProductModelCd", param.paramProductModelCd );                  //生産型式コード(前方一致)
            bindParam.Add( "paramProductModelCdList", param.paramProductModelCdList );          //生産型式コードリスト

            if ( null != param.paramInstallDtFrom ) {
                bindParam.Add( "paramInstallDtFrom", param.paramInstallDtFrom.Value );          //組付日(FROM)
            }
            if ( null != param.paramInstallDtTo ) {
                bindParam.Add( "paramInstallDtTo", param.paramInstallDtTo.Value );              //組付日(TO)
            }

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectTractorNameplateList" );
            return SelectPicData( statementId, bindParam, maxRecordCount );
        }
        /// <summary>
        /// 詳細画面トラクタ部品(ミッション)情報検索
        /// </summary>
        /// <param name="param"></param>
        /// <param name="maxRecordCount"></param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectTractorMissionList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
            KTBindParameters bindParam = new KTBindParameters();

            //部品検索条件
            bindParam.Add( "paramSerialList", param.paramSerialList );                      //型式/機番リスト
            bindParam.Add( "paramPartsNum", param.paramPartsNum );                          //部品品番(クボタ品番

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectTractorMissionList" );
            return SelectPicData( statementId, bindParam, maxRecordCount );
        }
        /// <summary>
        /// 詳細画面トラクタ部品(ハウジング)情報検索
        /// </summary>
        /// <param name="param"></param>
        /// <param name="maxRecordCount"></param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectTractorHousingList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
            KTBindParameters bindParam = new KTBindParameters();

            //部品検索条件
            bindParam.Add( "paramSerialList", param.paramSerialList );                      //型式/機番リスト
            bindParam.Add( "paramPartsNum", param.paramPartsNum );                          //部品品番(クボタ品番)

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectTractorHousingList" );
            return SelectPicData( statementId, bindParam, maxRecordCount );
        }
        /// <summary>
        /// 一覧画面トラクタ部品(基幹部品)情報検索
        /// </summary>
        /// <param name="param"></param>
        /// <param name="maxRecordCount"></param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectTractorCorePartsList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
            KTBindParameters bindParam = new KTBindParameters();

            //部品検索条件
            bindParam.Add( "paramPartsCd", param.paramPartsCd );                            //部品区分
            bindParam.Add( "paramSerialList", param.paramSerialList );                      //型式/機番リスト
            bindParam.Add( "paramProductModelCd", param.paramProductModelCd );              //生産型式コード(前方一致)
            bindParam.Add( "paramProductModelCdList", param.paramProductModelCdList );      //生産型式コードリスト
            bindParam.Add( "paramPartsNum", param.paramPartsNum );                          //部品品番(前方一致)
            bindParam.Add( "paramPartsSerial", param.paramPartsSerial );                    //部品機番

            if ( null != param.paramInstallDtFrom ) {
                bindParam.Add( "paramInstallDtFrom", param.paramInstallDtFrom.Value );      //組付日(FROM)
            }
            if ( null != param.paramInstallDtTo ) {
                bindParam.Add( "paramInstallDtTo", param.paramInstallDtTo.Value );          //組付日(TO)
            }

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectTractorCorePartsList" );
            DataTable tblParts = SelectPicData( statementId, bindParam, maxRecordCount );

            //品番を「XXXXX-XXXXX」に変換
            if ( null != tblParts && 0 < tblParts.Rows.Count ) {
                foreach ( DataRow row in tblParts.Rows ) {
                    row["partsNum"] = DataUtils.GetModelCdStr( StringUtils.ToString( row["partsNum"] ) );
                }
            }

            return MasterList.SetStationNm( tblParts );
        }
        /// <summary>
        /// 詳細画面エンジン機番ラベル印刷検索
        /// </summary>
        /// <param name="param"></param>
        /// <param name="maxRecordCount"></param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectEngineSerialPrintList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
            KTBindParameters bindParam = new KTBindParameters();

            //部品検索条件
            bindParam.Add( "paramSerialList", param.paramSerialList );                          //型式/機番リスト
            bindParam.Add( "paramProductModelCd", param.paramProductModelCd );                  //生産型式コード(前方一致)
            bindParam.Add( "paramProductModelCdList", param.paramProductModelCdList );          //生産型式コードリスト

            if ( null != param.paramInstallDtFrom ) {
                bindParam.Add( "paramInstallDtFrom", param.paramInstallDtFrom.Value );          //組付日(FROM)
            }
            if ( null != param.paramInstallDtTo ) {
                bindParam.Add( "paramInstallDtTo", param.paramInstallDtTo.Value );              //組付日(TO)
            }

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineSerialPrintList" );
            return SelectPicData( statementId, bindParam, maxRecordCount );
        }
        #endregion

        #region 詳細検索DAO
        /// <summary>
        /// 詳細画面トラクタ部品(WifiECU)情報検索(PAIRRSLT_F)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectTractorWifiEcuDetail( string productModelCd, string serial ) {
            KTBindParameters bindParam = new KTBindParameters();

            //パラメータの設定
            bindParam.Add( "paramSerial", new SerialParam( productModelCd, serial ) );          //型式/機番リスト

            //SQL
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectTractorWifiEcuDetail" );

            //検索
            return SelectPicData( statementId, bindParam );
        }

        /// <summary>
        /// 詳細画面トラクタ部品(クレート機番)情報検索(TBL_WMSクレート機番)
        /// </summary>
        /// <param name="idno">IDNO</param>
        /// <param name="productModelCd">生産型式コード</param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectTractorCrateDetail( string idno, string productModelCd ) {
            KTBindParameters bindParam = new KTBindParameters();

            //パラメータの設定
            bindParam.Add( "idno", idno );                      //idno
            bindParam.Add( "productModelCd", productModelCd );  //型式コード

            //SQL
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectTractorCrateDetail" );

            //検索
            return SelectGiaData( statementId, bindParam );
        }

        /// <summary>
        /// 詳細画面トラクタ部品(ロプス機番)情報検索(ロプス機番)
        /// </summary>
        /// <param name="idno">IDNO</param>
        /// <param name="productModelCd">生産型式コード</param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectTractorRopsDetail( string idno, string productModelCd ) {
            KTBindParameters bindParam = new KTBindParameters();

            //パラメータの設定
            bindParam.Add( "idno", idno );                      //idno
            bindParam.Add( "productModelCd", productModelCd );  //型式コード

            //SQL
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectTractorRopsDetail" );

            //検索
            return SelectGiaData( statementId, bindParam );
        }

        /// <summary>
        /// 詳細画面トラクタ部品(ミッション)
        /// </summary>
        /// <param name="idno">IDNO</param>
        /// <param name="productModelCd">生産型式コード</param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectTractorMissionDetail( string productModelCd, string serial ) {
            KTBindParameters bindParam = new KTBindParameters();

            //パラメータの設定
            bindParam.Add( "productModelCd", productModelCd );  //型式コード
            bindParam.Add( "serial", serial );                  //機番

            //SQL
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectTractorMissionDetail" );

            //検索
            return SelectPicData( statementId, bindParam );
        }

        /// <summary>
        /// 詳細画面トラクタ部品(ハウジング)
        /// </summary>
        /// <param name="idno">IDNO</param>
        /// <param name="productModelCd">生産型式コード</param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectTractorHousingDetail( string productModelCd, string serial ) {
            KTBindParameters bindParam = new KTBindParameters();

            //パラメータの設定
            bindParam.Add( "productModelCd", productModelCd );  //型式コード
            bindParam.Add( "serial", serial );                  //機番

            //SQL
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectTractorHousingDetail" );

            //検索
            return SelectPicData( statementId, bindParam );
        }

        /// <summary>
        /// 詳細画面トラクタ部品(基幹部品)情報検索
        /// </summary>
        /// <param name="idno">IDNO</param>
        /// <param name="productModelCd">生産型式コード</param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectTractorCorePartsDetail( string partsCd, string productModelCd, string serial ) {
            KTBindParameters bindParam = new KTBindParameters();

            //パラメータの設定
            bindParam.Add( "partsCd", partsCd );                        //部品区分
            bindParam.Add( "productModelCd", productModelCd );          //型式コード
            bindParam.Add( "serial", DataUtils.GetSerial6( serial ) );  //機番

            //SQL
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectTractorCorePartsDetail" );

            //検索
            DataTable tblParts = SelectPicData( statementId, bindParam );

            //品番を「XXXXX-XXXXX」に変換
            if ( null != tblParts && 0 < tblParts.Rows.Count ) {
                foreach ( DataRow row in tblParts.Rows ) {
                    row["partsNum"] = DataUtils.GetModelCdStr( StringUtils.ToString( row["partsNum"] ) );
                }
            }

            return MasterList.SetStationNm( tblParts );
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

        /// <summary>
        /// データ検索処理
        /// </summary>
        /// <param name="statementId">ステートメントID</param>
        /// <param name="bindParam">パラメータ</param>
        /// <param name="maxRecordCount">最大出力件数</param>
        /// <returns>出力結果DataTable</returns>
        private static DataTable SelectGiaData( string statementId, KTBindParameters bindParam, int maxRecordCount = Int32.MaxValue ) {
            Cursor cursor = GiaDao.GetInstance().OpenCursor( statementId, bindParam );
            DataTable resultTable = null;
            try {
                while ( GiaDao.GetInstance().Fetch( ref resultTable, ref cursor ) ) {
                    if ( resultTable.Rows.Count >= maxRecordCount ) {
                        break;
                    }
                }
            } finally {
                GiaDao.GetInstance().CloseCursor( ref cursor );
            }
            resultTable.AcceptChanges();

            return resultTable;
        }
    }
}