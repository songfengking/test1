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
    /// エンジン部品検索DAO
    /// </summary>
    public class EnginePartsDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "EngineParts";
        /// <summary>
        /// 検索パラメータクラス
        /// </summary>
        public class SearchParameter {
            /// <summary>[共通]型式/機番</summary>
            public List<SerialParam> paramSerialList = null;
            /// <summary>[共通]生産型式コード(前方一致)</summary>
            public string paramProductModelCd = null;
            /// <summary>部品区分コード[クレート/ロプス以外]</summary>
            public string paramPartsCd = null;
            /// <summary>部品品番(前方一致)</summary>
            public string paramPartsNum = null;
            /// <summary>部品機番</summary>
            public string paramPartsSerial = null;
            /// <summary>組付日(FROM)</summary>
            public DateTime? paramInstallDtFrom = null;
            /// <summary>組付日(TO)</summary>
            public DateTime? paramInstallDtTo = null;
            /// <summary>加工日(FROM)…3C用</summary>
            public DateTime? paramProcessDtFrom = null;
            /// <summary>加工日(TO)…3C用</summary>
            public DateTime? paramProcessDtTo = null;
            /// <summary>加工日未設定</summary>
            public bool paramPassedRegist = false;
            /// <summary>[共通]生産型式コードリスト(型式名から検索したリスト)</summary>
            public List<string> paramProductModelCdList = null;
        }

        #region 一覧検索DAO
        /// <summary>
        /// 一覧画面エンジン部品(共通)情報検索(ENGBKJ_F)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  paramSerialList         … 型式/機番リスト
        ///  paramProductModelCd     … 生産型式コード(前方一致)
        ///  paramPartsCd            … 部品区分コード
        ///  paramPartsNum           … 部品品番(前方一致)
        ///  paramPartsSerial        … 部品機番
        ///  paramInstallDtFrom      … 組付日(FROM)
        ///  paramInstallDtTo        … 組付日(TO)
        ///  paramProductModelCdList … 生産型式コードリスト
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectPartsList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //部品検索条件
            bindParam.Add( "paramSerialList", param.paramSerialList );                          //型式/機番リスト
            bindParam.Add( "paramProductModelCd", param.paramProductModelCd );                  //生産型式コード(前方一致)
            bindParam.Add( "paramPartsCd", param.paramPartsCd );                                //部品区分コード
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
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEnginePartsList" );
            DataTable tblParts = SelectData( statementId, bindParam, maxRecordCount );

            //クボタ品番を「XXXXX-XXXXX」に変換
            if ( null != tblParts && 0 < tblParts.Rows.Count ) {
                foreach ( DataRow row in tblParts.Rows ) {
                    row["partsKubotaNum"] = DataUtils.GetModelCdStr( StringUtils.ToString( row["partsKubotaNum"] ) );
                }
            }

            //ステーション名を割当(stationNmフィールドへセット)
            return MasterList.SetStationNm( tblParts );
        }

        /// <summary>
        /// 一覧画面エンジン部品(3C)情報検索(ENGBKJ_F)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  paramSerialList         … 型式/機番リスト
        ///  paramProductModelCd     … 生産型式コード(前方一致)
        ///  paramPartsCd            … 部品区分コード
        ///  paramPartsNum           … 部品品番(前方一致)
        ///  paramPartsSerial        … 部品機番
        ///  paramInstallDtFrom      … 組付日(FROM)
        ///  paramInstallDtTo        … 組付日(TO)
        ///  paramProcessDtFrom      … 加工日(FROM)
        ///  paramProcessDtTo        … 加工日(TO)
        ///  paramPassedRegist       … 加工日未設定(true=999999のものを検索 false=条件指定なし)
        ///  paramProductModelCdList … 生産型式コードリスト
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable Select3cList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //部品検索条件
            bindParam.Add( "paramSerialList", param.paramSerialList );                          //型式/機番リスト
            bindParam.Add( "paramProductModelCd", param.paramProductModelCd );                  //生産型式コード(前方一致)
            bindParam.Add( "paramPartsCd", param.paramPartsCd );                                //部品区分コード
            bindParam.Add( "paramPartsNum", param.paramPartsNum );                              //部品品番(前方一致)
            bindParam.Add( "paramPartsSerial", param.paramPartsSerial );                        //部品機番
            if ( null != param.paramInstallDtFrom ) {
                bindParam.Add( "paramInstallDtFrom", param.paramInstallDtFrom.Value );          //組付日(FROM)
            }
            if ( null != param.paramInstallDtTo ) {
                bindParam.Add( "paramInstallDtTo", param.paramInstallDtTo.Value );              //組付日(TO)
            }
            if ( null != param.paramProcessDtFrom ) {
                bindParam.Add( "paramProcessDtFrom", param.paramProcessDtFrom.Value );          //加工日(FROM)
            }
            if ( null != param.paramProcessDtTo ) {
                bindParam.Add( "paramProcessDtTo", param.paramProcessDtTo.Value );              //加工日(TO)
            }
            bindParam.Add( "paramPassedRegist", param.paramPassedRegist );                      //加工日未設定(999999のもの)
            bindParam.Add( "paramProductModelCdList", param.paramProductModelCdList );          //生産型式コードリスト

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngine3cList" );
            DataTable tblParts = SelectData( statementId, bindParam, maxRecordCount );

            //クボタ品番を「XXXXX-XXXXX」に変換
            if ( null != tblParts && 0 < tblParts.Rows.Count ) {
                foreach ( DataRow row in tblParts.Rows ) {
                    row["partsKubotaNum"] = DataUtils.GetModelCdStr( StringUtils.ToString( row["partsKubotaNum"] ) );
                }
            }

            //修正者の設定
            tblParts = replaceEmpInfo( tblParts, "UPDATE_BY" );

            //ステーション名を割当(stationNmフィールドへセット)
            return MasterList.SetStationNm( tblParts );
        }

        /// <summary>
        /// 一覧画面エンジン部品(基幹部品)情報検索
        /// </summary>
        /// <param name="param"></param>
        /// <param name="maxRecordCount"></param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectEngineCorePartsList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
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
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineCorePartsList" );
            DataTable tblParts = SelectData( statementId, bindParam, maxRecordCount );

            //品番を「XXXXX-XXXXX」に変換
            if ( null != tblParts && 0 < tblParts.Rows.Count ) {
                foreach ( DataRow row in tblParts.Rows ) {
                    row["partsNum"] = DataUtils.GetModelCdStr( StringUtils.ToString( row["partsNum"] ) );
                }
            }

            return MasterList.SetStationNm( tblParts );
        }
        #endregion

        #region 詳細検索DAO
        /// <summary>
        /// 詳細画面エンジン部品(共通)情報検索(ENGBKJ_F)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="lastHistoryDataOnly">true=最新データ取得 false=全来歴データ取得</param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectEnginePartsDetail( string partsCd, string productModelCd, string serial, bool lastHistoryDataOnly ) {
            KTBindParameters bindParam = new KTBindParameters();

            //パラメータの設定
            bindParam.Add( "paramPartsCd", partsCd );                                           //部品区分コード
            bindParam.Add( "paramSerial", new SerialParam( productModelCd, serial ) );          //型式/機番リスト
            bindParam.Add( "lastHistoryDataOnly", lastHistoryDataOnly );                        //最新データのみ取得
            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEnginePartsDetail" );
            DataTable tblParts = SelectData( statementId, bindParam );

            //クボタ品番を「XXXXX-XXXXX」に変換
            if ( null != tblParts && 0 < tblParts.Rows.Count ) {
                foreach ( DataRow row in tblParts.Rows ) {
                    row["partsKubotaNum"] = DataUtils.GetModelCdStr( StringUtils.ToString( row["partsKubotaNum"] ) );
                }
            }

            //ステーション名を割当(stationNmフィールドへセット)
            return MasterList.SetStationNm( tblParts );
        }

        /// <summary>
        /// 詳細画面エンジン部品(ECU)情報検索(ECURET_F)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectEngineEcuDetail( string productModelCd, string serial ) {
            KTBindParameters bindParam = new KTBindParameters();

            //パラメータの設定
            bindParam.Add( "paramSerial", new SerialParam( productModelCd, serial ) );          //型式/機番リスト

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineEcuDetail" );
            DataTable tblParts = SelectData( statementId, bindParam );

            //ステーション名を割当(stationNmフィールドへセット)
            return MasterList.SetStationNm( tblParts );
        }

        /// <summary>
        /// 詳細画面エンジン部品(インジェクター)情報検索(INJCRT_F)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectEngineInjecterDetail( string productModelCd, string serial ) {
            KTBindParameters bindParam = new KTBindParameters();

            //パラメータの設定
            bindParam.Add( "paramSerial", new SerialParam( productModelCd, serial ) );          //型式/機番リスト

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineInjecterDetail" );
            DataTable tblParts = SelectData( statementId, bindParam );

            //ステーション名を割当(stationNmフィールドへセット)
            return MasterList.SetStationNm( tblParts );
        }

        /// <summary>
        /// 詳細画面エンジン部品(DPF)情報検索(TBL_DPF機番情報)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectEngineDpfDetail( string productModelCd, string serial ) {
            KTBindParameters bindParam = new KTBindParameters();

            //パラメータの設定
            bindParam.Add( "paramSerial", new SerialParam( productModelCd, serial ) );          //型式/機番リスト

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineDpfDetail" );
            DataTable tblParts = SelectData( statementId, bindParam );

            //ステーション名を割当(stationNmフィールドへセット)
            return MasterList.SetStationNm( tblParts );
        }
        /// <summary>
        /// エンジン部品(設備情報)情報検索(MS_SETUBI_JYOHO)
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="country">国</param>
        /// <returns>検索結果DataTable</returns>
        public static DataTable SelectSetubiJyoho( string modelCd, string country ) {
            KTBindParameters bindParam = new KTBindParameters();

            //パラメータの設定
            bindParam.Add( "paramModelCd", modelCd );               //生産型式コード
            bindParam.Add( "paramCountry", country.PadRight( 6 ) );   //国
            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineSetubiJyoho" );
            DataTable tblParts = SelectData( statementId, bindParam );

            //ステーション名を割当(stationNmフィールドへセット)
            return tblParts;
        }

        /// <summary>
        /// 一覧画面エンジン機番ラベル印刷情報検索(TT_SQ_PRINTOUT_RECORD)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  paramSerialList         … 型式/機番リスト
        ///  paramProductModelCd     … 生産型式コード(前方一致)
        ///  paramPartsCd            … 部品区分コード
        ///  paramPartsNum           … 部品品番(前方一致)
        ///  paramPartsSerial        … 部品機番
        ///  paramInstallDtFrom      … 組付日(FROM)
        ///  paramInstallDtTo        … 組付日(TO)
        ///  paramProductModelCdList … 生産型式コードリスト
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectSerialPrintList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //部品検索条件
            bindParam.Add( "paramSerialList", param.paramSerialList );                          //型式/機番リスト
            bindParam.Add( "paramProductModelCd", param.paramProductModelCd );                  //生産型式コード(前方一致)
            bindParam.Add( "paramPartsCd", param.paramPartsCd );                                //部品区分コード
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
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectSerialPrintList" );
            DataTable tblParts = SelectData( statementId, bindParam, maxRecordCount );

            //ステーション名を割当(stationNmフィールドへセット)
            return MasterList.SetStationNm( tblParts );
        }

        /// <summary>
        /// 詳細画面エンジン機番ラベル印刷情報検索(TT_SQ_PRINTOUT_RECORD)
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <remarks>
        /// param設定情報
        ///  paramSerialList         … 型式/機番リスト
        ///  paramProductModelCd     … 生産型式コード(前方一致)
        ///  paramPartsCd            … 部品区分コード
        ///  paramPartsNum           … 部品品番(前方一致)
        ///  paramPartsSerial        … 部品機番
        ///  paramInstallDtFrom      … 組付日(FROM)
        ///  paramInstallDtTo        … 組付日(TO)
        ///  paramProductModelCdList … 生産型式コードリスト
        /// </remarks>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectSerialPrint( string productModelCd, string serial ) {
            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //部品検索条件
            bindParam.Add( "paramProductModelCd", productModelCd );                  //生産型式コード(前方一致)
            bindParam.Add( "paramSerial", serial );                                  //機番

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectSerialPrint" );
            DataTable tblParts = SelectData( statementId, bindParam );

            //ステーション名を割当(stationNmフィールドへセット)
            return MasterList.SetStationNm( tblParts );
        }

        /// <summary>
        /// 詳細画面エンジン部品(基幹部品)情報検索
        /// </summary>
        /// <param name="idno">IDNO</param>
        /// <param name="productModelCd">生産型式コード</param>
        /// <returns>検索結果詳細DataTable</returns>
        public static DataTable SelectEngineCorePartsDetail( string partsCd, string productModelCd, string serial ) {
            KTBindParameters bindParam = new KTBindParameters();

            //パラメータの設定
            bindParam.Add( "paramPartsCd", partsCd );                               //部品区分
            bindParam.Add( "paramProductModelCd", productModelCd );                 //型式コード
            bindParam.Add( "paramSerial", DataUtils.GetSerial6( serial ) );         //機番

            //SQL
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectEngineCorePartsDetail" );

            //検索
            DataTable tblParts = SelectData( statementId, bindParam );

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
        /// <summary>
        /// 指定カラムの値(従業員名)変更
        /// </summary>
        /// <param name="dt">名称変更対象DataTable</param>
        /// <param name="strCol">対象カラム</param>
        /// <returns></returns>
        private static DataTable replaceEmpInfo( DataTable dt, string strCol ) {

            Dictionary<String, String> _dicEmp = new Dictionary<String, String>();
            DataTable tmp = new DataTable();

            //従業員情報を取得
            tmp = Business.DetailViewBusiness.SelectEmpInfo( null, null );

            foreach ( DataRow dr in tmp.Rows ) {
                _dicEmp.Add( dr["EMP_NO"].ToString().Trim(), dr["EMP_NM"].ToString().Trim() );
            }

            foreach ( DataRow dtRow in dt.Rows ) {

                //「名称」で上書きする
                if ( StringUtils.IsNotEmpty( StringUtils.ToString( dtRow[strCol] ) ) ) {
                    if ( _dicEmp.ContainsKey( StringUtils.ToString( dtRow[strCol] ) ) ) {
                        //存在する
                        dtRow[strCol] = _dicEmp[StringUtils.ToString( dtRow[strCol] )];
                    } else {
                        //存在しない
                        dtRow[strCol] = "";
                    }
                }
            }

            return dt;
        }

        /// <summary>
        /// クボタ内製ECU検査情報詳細取得
        /// </summary>
        /// <param name="partsNumber">クボタ品番</param>
        /// <returns></returns>
        public static DataTable SelectInhouseEcuInspectionResultDetail( string ecuSerial ) {
            KTBindParameters param = new KTBindParameters();

            //パラメータの設定
            param.Add( "ecuSerial", ecuSerial );

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectInhouseEcuInspectionResultDetail" );
            return SelectData( statementId, param );
        }

        /// <summary>
        /// クボタ内製ECU検査情報CSVデータ取得
        /// </summary>
        /// <param name="partsNumber"></param>
        /// <returns></returns>
        public static Object SelectInhouseEcuInspectionResultData( string ecuSerial ) {
            KTBindParameters param = new KTBindParameters();

            //パラメータの設定
            param.Add( "ecuSerial", ecuSerial );

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectInhouseEcuInspectionResultData" );
            DataTable result = SelectData( statementId, param );
            if ( result != null && result.Rows.Count > 0 ) {
                return result.Rows[0]["inspectionResultData"];
            } else {
                return null;
            }
        }
    }
}