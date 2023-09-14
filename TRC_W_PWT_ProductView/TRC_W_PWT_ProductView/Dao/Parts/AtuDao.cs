using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using KTFramework.Common.Dao;
using KTFramework.Dao;
using KTFramework.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Dao.Com;
//using TRC_W_PWT_ProductView.Defines;

namespace TRC_W_PWT_ProductView.Dao.Parts {
    /// <summary>
    /// AtuDAO
    /// </summary>
    public class AtuDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "Atu";

        /// <summary>
        /// ATU検索パラメータクラス
        /// </summary>
        public class SearchParameter {

            /// <summary>[共通]型式/機番</summary>
            public List<SerialParam> paramSerialList = null;

            //ATU検索条件
            /// <summary>機種区分</summary>
            public string paramModelType;
            /// <summary>型式コード(前方一致)</summary>
            public string paramModelCd;
            /// <summary>型式名(前方一致)</summary>
            public string paramModelNm;
            /// <summary>IDNO</summary>
            public string paramIdno;
            /// <summary>機番</summary>
            public string paramSerial;
            /// <summary>品番</summary>
            public string paramPartsNum;
            /// <summary>生産指示日(FROM)</summary>
            public DateTime? paramProductInstDtFrom;
            /// <summary>生産指示日(TO)</summary>
            public DateTime? paramProductInstDtTo;
            /// <summary>投入日(FROM)</summary>
            public DateTime? paramThrowDtFrom;
            /// <summary>投入日(TO)</summary>
            public DateTime? paramThrowDtTo;
            /// <summary>完成日(FROM)</summary>
            public DateTime? paramCompletionDtFrom;
            /// <summary>完成日(TO)</summary>
            public DateTime? paramCompletionDtTo;

        }

        /// <summary>
        /// 一覧画面ATU投入順序情報検索
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectAtuList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //ATU検索条件式
            bindParam.Add( "paramModelType", param.paramModelType );                                    //機種区分
            bindParam.Add( "paramModelCd", param.paramModelCd );                                        //生産型式コード(前方一致)
            bindParam.Add( "paramModelNm", param.paramModelNm );                                        //生産型式名(前方一致)
            bindParam.Add( "paramSerial", param.paramSerial );                                          //機番(前方一致)
            //bindParam.Add( "paramIdno", param.paramIdno );                                              //IDNO(前方一致)
            bindParam.Add( "paramPartsNum", param.paramPartsNum );                                      //フルアッシ品番(前方一致)
            if ( null != param.paramProductInstDtFrom ) {
                bindParam.Add( "paramProductInstDtFrom", param.paramProductInstDtFrom.Value );          //生産指示日(FROM)
            }
            if ( null != param.paramProductInstDtTo ) {
                bindParam.Add( "paramProductInstDtTo", param.paramProductInstDtTo.Value );              //生産指示日(TO)
            }
            if ( null != param.paramThrowDtFrom ) {
                bindParam.Add( "paramThrowDtFrom", param.paramThrowDtFrom.Value );                      //投入日(FROM)
            }
            if ( null != param.paramThrowDtTo ) {
                bindParam.Add( "paramThrowDtTo", param.paramThrowDtTo.Value );                          //投入日(TO)
            }
            if ( null != param.paramCompletionDtFrom ) {
                bindParam.Add( "paramCompletionDtFrom", param.paramCompletionDtFrom.Value );            //完成日(FROM)
            }
            if ( null != param.paramCompletionDtTo ) {
                bindParam.Add( "paramCompletionDtTo", param.paramCompletionDtTo.Value );                //完成日(TO)
            }

            //SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectAtuThrowOrderList" );
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
            
            //ロジックでの値割当
            if ( null != resultTable ) {
                foreach ( DataRow row in resultTable.Rows ) {
                    //生産型式コード表記(生産型式でなく型式を表示させる[型式／仕向地変更対応])
                    string modelCd = StringUtils.ToString( row["modelCd"] );
                    row["modelCdStr"] = DataUtils.GetModelCdStr( modelCd );

                    string fullAssyPartsNum = StringUtils.ToString( row["fullAssyPartsNum"] );
                    row["fullAssyPartsNumStr"] = DataUtils.GetModelCdStr( fullAssyPartsNum );

                    string engineModelCd = StringUtils.ToString( row["engineModelCd"] );
                    row["engineModelCdStr"] = DataUtils.GetModelCdStr( engineModelCd );

                    string tractorModelCd = StringUtils.ToString( row["tractorModelCd"] );
                    row["tractorModelCdStr"] = DataUtils.GetModelCdStr( tractorModelCd );
                }

                resultTable.AcceptChanges();
            }

            return resultTable;
        }

        /// <summary>
        /// 一覧画面ATU機番情報検索
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectAtuSerialList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //ATU検索条件式
            bindParam.Add( "paramSerialList", param.paramSerialList );                                        //生産型式コード(前方一致)

            //SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectAtuPartsSerialList" );
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

            return MasterList.SetStationNm( resultTable );
        }


        /// <summary>
        /// 詳細画面ATU機番情報検索
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectAtuSerial( string modelCd, string serial ) {
            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //ATU検索条件式
            bindParam.Add( "paramModelCd", modelCd );                                        //生産型式コード(前方一致)
            bindParam.Add( "paramSerial", serial );                                          //機番(前方一致)

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectAtuPartsSerial" );
            DataTable tblParts = SelectData( statementId, bindParam );

            //ステーション名を割当(stationNmフィールドへセット)
            return MasterList.SetStationNm( tblParts );

        }


        /// <summary>
        /// 一覧画面トルク締付情報検索
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectAtuTorqueList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //ATU検索条件式
            bindParam.Add( "paramSerialList", param.paramSerialList );                                        //生産型式コード(前方一致)

            //SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectTorqueTightenRecordList" );
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

            return MasterList.SetStationNm( resultTable );
        }

        /// <summary>
        /// 詳細画面トルク締付情報検索
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectAtuTorque( string modelCd, string serial ) {
            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //ATU検索条件式
            bindParam.Add( "paramModelCd", modelCd );                                        //生産型式コード(前方一致)
            bindParam.Add( "paramSerial", serial );                                          //機番(前方一致)

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectTorqueTightenRecord" );
            DataTable tblParts = SelectData( statementId, bindParam );

            //ステーション名を割当(stationNmフィールドへセット)
            return MasterList.SetStationNm( tblParts );
            
        }





        /// <summary>
        /// 一覧画面リーク計測情報検索
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectAtuLeakList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //ATU検索条件式
            bindParam.Add( "paramSerialList", param.paramSerialList );                                        //生産型式コード(前方一致)

            //SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectLeakMeasureResultList" );
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

            return MasterList.SetStationNm( resultTable );
        }


        /// <summary>
        /// 詳細画面リーク計測検索
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectAtuLeak( string modelCd, string serial ) {
            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //ATU検索条件式
            bindParam.Add( "paramModelCd", modelCd );                                        //生産型式コード(前方一致)
            bindParam.Add( "paramSerial", serial );                                          //機番(前方一致)

            //SELECT実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectLeakMeasureResult" );
            DataTable tblParts = SelectData( statementId, bindParam );

            //ステーション名を割当(stationNmフィールドへセット)
            return MasterList.SetStationNm( tblParts );

        }


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

    }
}