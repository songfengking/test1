using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using KTFramework.Dao;
using KTFramework.Common;
using KTFramework.Common.Dao;
using TRC_W_PWT_ProductView.Business;
using TRC_W_PWT_ProductView.Defines;

namespace TRC_W_PWT_ProductView.Dao.Com {
	public class AiImageCheckDao : DaoBase {

		/// <summary>
		/// SQLマップネームスペース
		/// </summary>
		private const string SQLMAP_NAMESPACE = "AiImageCheck";

		/// <summary>
		/// 列名：値
		/// </summary>
		public const string COLNAME_VALUE = "VALUE";

		/// <summary>
		/// 列名：表示名
		/// </summary>
		public const string COLNAME_TEXT = "TEXT";

		/// <summary>
		/// ステーションリスト取得  
		/// </summary>
		/// <returns>取得結果</returns>
		public static DataTable SelectStationList( ) {
			string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectStationList" );
			DataTable resultTable = PicDao.GetInstance().Select( statementId );
			return resultTable;
		}

		/// <summary>
		/// ラインリスト取得  
		/// </summary>
		/// <returns>取得結果</returns>
		public static DataTable SelectLineList( ) {
			string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectLineList" );
			DataTable resultTable = PicDao.GetInstance().Select( statementId );
			return resultTable;
		}

		/// <summary>
		/// 入力STATIONに従ってライン取得 
		/// </summary>
		/// <param name="station">STATION</param>
		/// <returns>検索結果詳細DataTable</returns>
		public static DataTable SelectLineListByStation( string station ) {
			KTBindParameters bindParam = new KTBindParameters();

			//パラメータの設定
			bindParam.Add( "station", station );                      //station

			//SQL
			string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectLineListByStation" );

			//検索
			return SelectPicData( statementId, bindParam );
		}

		/// <summary>
		/// 入力LINEに従ってステーションリスト取得   
		/// </summary>
		/// <param name="linecd">LINE</param>
		/// <returns>検索結果詳細DataTable</returns>
		public static DataTable SelectStationListByLine( string linecd ) {
			KTBindParameters bindParam = new KTBindParameters();

			//パラメータの設定
			bindParam.Add( "linecd", linecd );                      //linecd

			//SQL
			string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectStationListByLine" );

			//検索
			return SelectPicData( statementId, bindParam );
		}

		/// <summary>
		/// 入力LINEとSTATIONに従って設備リスト取得   
		/// </summary>
		/// <param name="linecd">LINE</param>
		/// <param name="station">STATION</param>
		/// <returns>検索結果詳細DataTable</returns>
		public static DataTable SelectSetubiByLineStation( string linecd, string station ) {
			KTBindParameters bindParam = new KTBindParameters();

			//パラメータの設定
			bindParam.Add( "linecd", linecd );                      //linecd
			bindParam.Add( "station", station );                    //station

			//SQL
			string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectSetubiByLineStation" );

			//検索
			return SelectPicData( statementId, bindParam );
		}

		/// <summary>
		/// 不良タイプリスト検索処理  
		/// </summary>
		/// <param name="ngFlag">不良タイプフラグ</param>
		/// <returns>検索結果詳細DataTable</returns>
		public static DataTable SelectNgTypeList( string ngFlag ) {
			KTBindParameters bindParam = new KTBindParameters();

			//パラメータの設定
			bindParam.Add( "ngFlag", ngFlag );

			//SQL
			string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectNgTypeList" );

			//検索
			return SelectPicData( statementId, bindParam );
		}

		/// <summary>
		/// 是正タイプリスト検索処理  
		/// </summary>
		/// <param name="correctiveFlag">是正タイプフラグ</param>
		/// <returns>検索結果詳細DataTable</returns>
		public static DataTable SelectCorrectiveTypeList( string correctiveFlag ) {
			KTBindParameters bindParam = new KTBindParameters();

			//パラメータの設定
			bindParam.Add( "correctiveFlag", correctiveFlag );

			//SQL
			string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectCorrectiveTypeList" );

			//検索
			return SelectPicData( statementId, bindParam );
		}

		/// <summary>
		/// 是正結果リスト検索処理  
		/// </summary>
		/// <param name="correctiveFlag">是正タイプフラグ</param>
		/// <returns>検索結果詳細DataTable</returns>
		public static DataTable SelectCorrectiveResultList( string correctiveFlag ) {
			KTBindParameters bindParam = new KTBindParameters();

			//パラメータの設定
			bindParam.Add( "correctiveFlag", correctiveFlag );

			//SQL
			string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectCorrectiveResultList" );

			//検索
			return SelectPicData( statementId, bindParam );
		}

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
		/// 是正処置入力製品検索画面検索取得
		/// </summary>
		/// <param name="lineCd">ライン</param>
		/// <param name="station">ステーション</param>
		/// <param name="modelCd">生産型式コード</param>
		/// <param name="modelNm">生産型式名</param>
		/// <param name="kiban">製品機番</param>
		/// <param name="idno">IDNO</param>
		/// <param name="monthlySeqNum">月度連番</param>
		/// <param name="anlResult">検査結果</param>
		/// <param name="cldStart">検査日開始</param>
		/// <param name="cldEnd">検査日終了</param>
		/// <param name="maxRecordCount">最大検索件数</param>
		/// <param name="result">結果</param>
		/// <returns>取得結果</returns>
		public static DataTable SelectSearchResultList( string lineCd, string station, string modelCd, string modelNm, string kiban, string idno, string monthlySeqNum, string anlResult, string cldStart, string cldEnd, int maxRecordCount, ref AiImageCheckBusiness.ImgCheckResultSet result ) {
			KTBindParameters param = new KTBindParameters();
			param.Add( "lineCd", lineCd );
			param.Add( "station", station );
			param.Add( "modelCd", modelCd );
			param.Add( "modelNm", modelNm );
			param.Add( "kiban", kiban );
			param.Add( "idno", idno );
			param.Add( "monthlySeqNum", monthlySeqNum );
			param.Add( "anlResult", anlResult );
			param.Add( "cldStart", cldStart );
			param.Add( "cldEnd", cldEnd );

			string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectSearchResultList" );
			Cursor cursor = PicDao.GetInstance().OpenCursor( statementId, param );
			DataTable resultTable = null;

			try {
				while ( PicDao.GetInstance().Fetch( ref resultTable, ref cursor ) ) {
					if ( resultTable.Rows.Count > maxRecordCount ) {
						result.Message = new Msg( MsgManager.MESSAGE_WRN_61020 );
						resultTable.Rows[resultTable.Rows.Count - 1].Delete();
						break;
					}
				}
			} finally {
				PicDao.GetInstance().CloseCursor( ref cursor );
			}
			return resultTable;
		}

		/// <summary>
		/// 検査画像ファイルパス取得処理
		/// </summary>
		/// <param name="ddlStation">ステーション</param>
		/// <param name="txtIdno">IDNO</param>
		/// <param name="txtMonthlySeqNum">月度連番s</param>
		/// <param name="txtRecordNo">来歴</param>
		public static DataTable SelectSearchImagePath( string ddlStation, string txtIdno, string txtMonthlySeqNum, string txtRecordNo, ref AiImageCheckBusiness.ImgCheckResultSet result ) {
			KTBindParameters param = new KTBindParameters();
			param.Add( "ddlStation", ddlStation );
			param.Add( "txtIdno", txtIdno );
			param.Add( "txtMonthlySeqNum", txtMonthlySeqNum );
			param.Add( "txtRecordNo", txtRecordNo );

			string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectSearchImagePath" );
			Cursor cursor = PicDao.GetInstance().OpenCursor( statementId, param );
			DataTable resultTable = null;

			try {
				PicDao.GetInstance().Fetch( ref resultTable, ref cursor );
			} finally {
				PicDao.GetInstance().CloseCursor( ref cursor );
			}
			return resultTable;
		}

		/// <summary>
		/// 検査結果取得処理
		/// </summary>
		/// <param name="ddlLineCd">ラインコード</param>
		/// <param name="ddlStation">ステーション</param>
		/// <param name="txtIdno">IDNO</param>
		/// <param name="txtMonthlySeqNum">月度連番s</param>
		/// <param name="txtRecordNo">来歴</param>
		/// <param name="ngFlag">不良タイプフラグ</param>
		/// <param name="correctiveFlag">是正フラグ</param>
		public static DataTable SelectSearchAnlResultList( string ddlLineCd, string ddlStation, string txtIdno, string txtMonthlySeqNum, string txtRecordNo, string ngFlag, string correctiveFlag, ref AiImageCheckBusiness.ImgCheckResultSet result ) {
			KTBindParameters param = new KTBindParameters();
			param.Add( "ddlLineCd", ddlLineCd );
			param.Add( "ddlStation", ddlStation );
			param.Add( "txtIdno", txtIdno );
			param.Add( "txtMonthlySeqNum", txtMonthlySeqNum );
			param.Add( "txtRecordNo", txtRecordNo );
			param.Add( "ngFlag", ngFlag );
			param.Add( "correctiveFlag", correctiveFlag );

			string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectSearchAnlResultList" );
			Cursor cursor = PicDao.GetInstance().OpenCursor( statementId, param );
			DataTable resultTable = null;

			try {
				while ( PicDao.GetInstance().Fetch( ref resultTable, ref cursor ) ) {
				}
			} finally {
				PicDao.GetInstance().CloseCursor( ref cursor );
			}
			return resultTable;
		}

		/// <summary>
		/// 更新画像解析結果情報更新
		/// </summary>
		/// <param name="bindParam">更新パラメータ</param>
		/// <returns></returns>
		public static int UpdateAnlResult( KTBindParameters bindParam ) {

			int intRet = 0;

			//実行SQL
			try {
				//更新
				PicDao.GetInstance().BeginTransaction();
				string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "UpdateAnlResult" );
				intRet = PicDao.GetInstance().Exec( statementId, bindParam );
				PicDao.GetInstance().CommitTransaction();

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
				if ( true == PicDao.GetInstance().IsTransaction ) {
					try {
						PicDao.GetInstance().RollbackTransaction();
					} catch {
						intRet = -1;
					}
				}
			}

			return intRet;
		}

		/// <summary>
		/// 更新画像グループ入力更新
		/// </summary>
		/// <param name="bindParam">更新パラメータ</param>
		/// <returns></returns>
		public static int UpdateGroupInput( KTBindParameters bindParam ) {

			int intRet = 0;

			//実行SQL
			try {
				//更新
				PicDao.GetInstance().BeginTransaction();
				string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "UpdateGroupInput" );
				intRet = PicDao.GetInstance().Exec( statementId, bindParam );
				PicDao.GetInstance().CommitTransaction();

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
				if ( true == PicDao.GetInstance().IsTransaction ) {
					try {
						PicDao.GetInstance().RollbackTransaction();
					} catch {
						intRet = -1;
					}
				}
			}

			return intRet;
		}

		/// <summary>
		/// 登録画像グループ入力更新
		/// </summary>
		/// <param name="bindParam">登録パラメータ</param>
		/// <returns></returns>
		public static int InsertGroupInput( KTBindParameters bindParam ) {

			int intRet = 0;

			//実行SQL
			try {
				//更新
				PicDao.GetInstance().BeginTransaction();
				string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "InsertGroupInput" );
				intRet = PicDao.GetInstance().Exec( statementId, bindParam );
				PicDao.GetInstance().CommitTransaction();

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
				if ( true == PicDao.GetInstance().IsTransaction ) {
					try {
						PicDao.GetInstance().RollbackTransaction();
					} catch {
						intRet = -1;
					}
				}
			}

			return intRet;
		}

		/// <summary>
		/// 検査項目マスタ画面検索データ取得
		/// </summary>
		/// <param name="lineCd">ライン</param>
		/// <param name="station">ステーション</param>
		/// <param name="itemCd">画像解析項目コード</param>
		/// <param name="itemNm">画像解析項目名</param>
		/// <param name="usingTool">利用ツール</param>
		/// <param name="usingTool">利用ツールプルダウンタイプ</param>
		/// <param name="maxRecordCount">最大検索件数</param>
		/// <param name="result">結果</param>
		/// <returns>取得結果</returns>
		public static DataTable SelectItemSearchList( string lineCd, string station, string itemCd, string itemNm, string usingTool, string pulldownType, int maxRecordCount, ref AiImageCheckBusiness.ImgCheckResultSet result ) {
			KTBindParameters param = new KTBindParameters();
			param.Add( "lineCd", lineCd );
			param.Add( "station", station );
			param.Add( "itemCd", itemCd );
			param.Add( "itemNm", itemNm );
			param.Add( "usingTool", usingTool );
			param.Add( "pulldownType", pulldownType );

			string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectItemSearchList" );
			Cursor cursor = PicDao.GetInstance().OpenCursor( statementId, param );
			DataTable resultTable = null;

			try {
				while ( PicDao.GetInstance().Fetch( ref resultTable, ref cursor ) ) {
					if ( resultTable.Rows.Count > maxRecordCount ) {
						//  result.Message = new Msg( MsgManager.MESSAGE_WRN_61020 );
						resultTable.Rows[resultTable.Rows.Count - 1].Delete();
						break;
					}
				}
			} finally {
				PicDao.GetInstance().CloseCursor( ref cursor );
			}
			return resultTable;
		}

		/// <summary>
		/// 検査項目コード採番処理  
		/// </summary>
		/// <returns>検査項目コード</returns>
		public static string SelectNextItemCode( ) {
			KTBindParameters bindParam = new KTBindParameters();
			string returnResult = string.Empty;

			//SQL
			string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectNextItemCode" );

			//検索
			DataTable resultTable = PicDao.GetInstance().Select( statementId );

			if ( resultTable.Rows.Count > 0 ) {
				returnResult = "0000000000" + resultTable.Rows[0][0].ToString();
				returnResult = returnResult.Substring( returnResult.Length - 10, 10 );
			}
			return returnResult;
		}

		/// <summary>
		/// グループコード採番処理  
		/// </summary>
		/// <returns>検査項目コード</returns>
		public static string SelectNextGroupCode( ) {
			KTBindParameters bindParam = new KTBindParameters();
			string returnResult = string.Empty;

			//SQL
			string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectNextGroupCode" );

			//検索
			DataTable resultTable = PicDao.GetInstance().Select( statementId );

			if ( resultTable.Rows.Count > 0 ) {
				returnResult = "0000000000" + resultTable.Rows[0][0].ToString();
				returnResult = returnResult.Substring( returnResult.Length - 10, 10 );
			}
			return returnResult;
		}

		/// <summary>
		/// 検査項目マスタ登録
		/// </summary>
		/// <param name="bindParam">更新パラメータ</param>
		/// <returns>更新件数</returns>
		public static int InsertAnlItemData( KTBindParameters bindParam ) {

			int intRet = 0;

			//実行SQL
			try {

				PicDao.GetInstance().BeginTransaction();
				string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "InsertAnlItemData" );
				intRet = PicDao.GetInstance().Exec( statementId, bindParam );
				PicDao.GetInstance().CommitTransaction();

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
				if ( true == PicDao.GetInstance().IsTransaction ) {
					try {
						PicDao.GetInstance().RollbackTransaction();
					} catch {
						intRet = -1;
					}
				}
			}

			return intRet;
		}

		/// <summary>
		/// 利用ツールリスト検索処理  
		/// </summary>
		/// <param name="pullldownType">プルダウンタイプ</param>
		/// <returns>利用ツールDataTable</returns>
		public static DataTable SelectUsingToolList( string pullldownType ) {
			KTBindParameters bindParam = new KTBindParameters();

			//パラメータの設定
			bindParam.Add( "pullldownType", pullldownType );

			//SQL
			string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectUsingToolList" );

			//検索
			return SelectPicData( statementId, bindParam );
		}

		/// <summary>
		/// 画像解析グループ検索処理
		/// </summary>
		/// <param name="station">ステーション</param>
		/// <param name="lineCd">ライン</param>
		/// <param name="setubi">設備</param>
		/// <param name="imgAnlGroupCd">画像解析グループコード</param>
		/// <param name="imgAnlGroupNm">画像解析グループ名</param>
		/// <param name="maxRecordCount">最大検索件数</param>
		/// <param name="result">結果</param>
		/// <returns>取得結果</returns>
		public static DataTable SelectGroupList( string station, string lineCd, string setubi, string imgAnlGroupCd, string imgAnlGroupNm, int maxRecordCount, ref AiImageCheckBusiness.ImgCheckResultSet result ) {
			KTBindParameters param = new KTBindParameters();
			param.Add( "station", station );
			param.Add( "lineCd", lineCd );
			param.Add( "setubi", setubi );
			param.Add( "imgAnlGroupCd", imgAnlGroupCd );
			param.Add( "imgAnlGroupNm", imgAnlGroupNm );

			string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectGroupList" );
			Cursor cursor = PicDao.GetInstance().OpenCursor( statementId, param );
			DataTable resultTable = null;

			try {
				while ( PicDao.GetInstance().Fetch( ref resultTable, ref cursor ) ) {
					if ( resultTable.Rows.Count > maxRecordCount ) {
						result.Message = new Msg( MsgManager.MESSAGE_WRN_61020 );
						resultTable.Rows[resultTable.Rows.Count - 1].Delete();
						break;
					}
				}
			} finally {
				PicDao.GetInstance().CloseCursor( ref cursor );
			}
			return resultTable;
		}

		/// <summary>
		/// 画像解析グループ入力検索処理
		/// </summary>
		/// <param name="station">ステーション</param>
		/// <param name="lineCd">ライン</param>
		/// <param name="setubi">設備</param>
		/// <param name="imgAnlGroupCd">画像解析グループコード</param>
		/// <param name="result">結果</param>
		/// <returns>取得結果</returns>
		public static DataTable SelectGroupInputList( string station, string lineCd, string setubi, string imgAnlGroupCd, ref AiImageCheckBusiness.ImgCheckResultSet result ) {
			KTBindParameters param = new KTBindParameters();
			param.Add( "station", station );
			param.Add( "lineCd", lineCd );
			param.Add( "setubi", setubi );
			param.Add( "imgAnlGroupCd", imgAnlGroupCd );

			string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectGroupInputList" );
			Cursor cursor = PicDao.GetInstance().OpenCursor( statementId, param );
			DataTable resultTable = null;

			try {
				while ( PicDao.GetInstance().Fetch( ref resultTable, ref cursor ) ) {
				}
			} finally {
				PicDao.GetInstance().CloseCursor( ref cursor );
			}
			return resultTable;
		}

		/// <summary>
		/// 型式紐づけデータ登録
		/// </summary>
		/// <param name="dtUpdate">型式紐づけデータ</param>
		/// <param name="updBy">更新者</param>
		/// <param name="updSys">更新プログラム</param>
		/// <param name="exeCnt">更新件数</param>
		/// <returns>更新件数</returns>
		public static int InsertSetubiJyohoData( DataTable dtUpdate, string updBy, string updSys, int exeCnt ) {
			int intRet = 0;
			int intExec = 0;
			//************************************
			//パラメータの設定
			//************************************

			//実行SQL
			string statementId1 = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "UpdateSetubiJyoho" );
			string statementId2 = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "InsertSetubiJyoho" );
			try {
				//transaction発行
				PicDao.GetInstance().BeginTransaction();


				for ( int rowIdx = 0; rowIdx < dtUpdate.Rows.Count; rowIdx++ ) {
					KTBindParameters bindParam = new KTBindParameters();

					bindParam.Add( "paramModelCd", StringUtils.ToString( dtUpdate.Rows[rowIdx]["MODEL_CD"] ) );
					bindParam.Add( "paramCountryCd", StringUtils.ToString( dtUpdate.Rows[rowIdx]["COUNTRY_CD"] ) );
					bindParam.Add( "paramSetubi", StringUtils.ToString( dtUpdate.Rows[rowIdx]["SETUBI"] ) );
					bindParam.Add( "paramGroupCd", StringUtils.ToString( dtUpdate.Rows[rowIdx]["GROUP_CD"] ) );
					bindParam.Add( "paramupdBy", updBy );
					bindParam.Add( "paramupdSys", updSys );

					//更新
					intExec = PicDao.GetInstance().Exec( statementId1, bindParam );
					if ( intExec == 0 ) {
						intExec = PicDao.GetInstance().Exec( statementId2, bindParam );
					}
					intRet = intRet + intExec;
				}


				if ( exeCnt.Equals( intRet ) ) {
					//正常
					PicDao.GetInstance().CommitTransaction();
				} else {
					PicDao.GetInstance().RollbackTransaction();
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
				if ( true == PicDao.GetInstance().IsTransaction ) {
					try {
						PicDao.GetInstance().RollbackTransaction();
					} catch {
						intRet = -1;
					}
				}
			}

			return intRet;
		}

		/// <summary>
		/// 型式紐づけデータ削除
		/// </summary>
		/// <param name="dtUpdate">型式紐づけデータ</param>
		/// <param name="updBy">更新者</param>
		/// <param name="updSys">更新プログラム</param>
		/// <param name="exeCnt">更新件数</param>
		/// <returns>更新件数</returns>
		public static int DeleteSetubiJyohoData( DataTable dtUpdate, string updBy, string updSys, int exeCnt ) {
			int intRet = 0;
			int intExec = 0;
			//************************************
			//パラメータの設定
			//************************************

			//実行SQL
			string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "DeleteSetubiJyoho" );

			try {
				//transaction発行
				PicDao.GetInstance().BeginTransaction();


				for ( int rowIdx = 0; rowIdx < dtUpdate.Rows.Count; rowIdx++ ) {
					KTBindParameters bindParam = new KTBindParameters();

					bindParam.Add( "paramModelCd", StringUtils.ToString( dtUpdate.Rows[rowIdx]["MODEL_CD"] ) );
					bindParam.Add( "paramCountryCd", StringUtils.ToString( dtUpdate.Rows[rowIdx]["COUNTRY_CD"] ) );
					bindParam.Add( "paramSetubi", StringUtils.ToString( dtUpdate.Rows[rowIdx]["SETUBI"] ) );


					//削除
					intExec = PicDao.GetInstance().Exec( statementId, bindParam );

					intRet = intRet + intExec;
				}


				if ( exeCnt.Equals( intRet ) ) {
					//正常
					PicDao.GetInstance().CommitTransaction();
				} else {
					PicDao.GetInstance().RollbackTransaction();
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
				if ( true == PicDao.GetInstance().IsTransaction ) {
					try {
						PicDao.GetInstance().RollbackTransaction();
					} catch {
						intRet = -1;
					}
				}
			}

			return intRet;
		}

		/// <summary>
		/// 型式紐づけ一覧検索処理
		/// </summary>
		/// <param name="station">ステーション</param>
		/// <param name="lineCd">ライン</param>
		/// <param name="setubi">設備</param>
		/// <param name="imgAnlGroupCd">画像解析グループコード</param>
		/// <param name="maxRecordCount">最大検索件数</param>
		/// <param name="result">結果</param>
		/// <returns>取得結果</returns>
		public static DataTable SelectSetubiJyohoList( string station, string lineCd, string setubi, string imgAnlGroupCd, int maxRecordCount, ref AiImageCheckBusiness.ImgCheckResultSet result ) {
			KTBindParameters param = new KTBindParameters();
			param.Add( "station", station );
			param.Add( "lineCd", lineCd );
			param.Add( "setubi", setubi );
			param.Add( "imgAnlGroupCd", imgAnlGroupCd );

			string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectSetubiJyohoList" );
			Cursor cursor = PicDao.GetInstance().OpenCursor( statementId, param );
			DataTable resultTable = null;

			try {
				while ( PicDao.GetInstance().Fetch( ref resultTable, ref cursor ) ) {
					if ( resultTable.Rows.Count > maxRecordCount ) {
						result.Message = new Msg( MsgManager.MESSAGE_WRN_61020 );
						resultTable.Rows[resultTable.Rows.Count - 1].Delete();
						break;
					}
				}
			} finally {
				PicDao.GetInstance().CloseCursor( ref cursor );
			}
			return resultTable;
		}

		/// <summary>
		/// 型式一覧取得処理
		/// </summary>
		/// <param name="txtGroupCd">グループコード</param>
		/// <param name="txtGroupNm">グループ名</param>
		/// <param name="txtModelCd">型式コード</param>
		/// <param name="txtModelNm">型式名</param>
		/// <param name="check">未割当</param>
		/// <param name="txtGroupCdNotExists">除外グループコード</param>
		public static DataTable SelectModelList( string txtGroupCd, string txtGroupNm, string txtModelCd, string txtModelNm, string check, string txtGroupCdNotExists, string txtStation, int maxRecordCount, ref AiImageCheckBusiness.ImgCheckResultSet result ) {
			KTBindParameters param = new KTBindParameters();

			param.Add( "txtGroupCd", txtGroupCd );
			param.Add( "txtGroupNm", txtGroupNm );
			param.Add( "txtModelCd", txtModelCd );
			param.Add( "txtModelNm", txtModelNm );
			param.Add( "check", check );
			param.Add( "txtStation", txtStation );
			param.Add( "txtGroupCdNotExists", txtGroupCdNotExists );

			string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectModelList" );
			Cursor cursor = PicDao.GetInstance().OpenCursor( statementId, param );
			DataTable resultTable = null;
			try {
				while ( PicDao.GetInstance().Fetch( ref resultTable, ref cursor ) ) {
					if ( resultTable.Rows.Count > maxRecordCount ) {
						result.Message = new Msg( MsgManager.MESSAGE_WRN_61020 );
						resultTable.Rows[resultTable.Rows.Count - 1].Delete();
						break;
					}
				}
			} finally {
				PicDao.GetInstance().CloseCursor( ref cursor );
			}
			return resultTable;
		}	

		/// <summary>
		/// 画像解析項目構成一覧検索処理
		/// </summary>
		/// <param name="txtGroupCd">画像解析グループコード</param>
		/// <param name="pulldownType">プルダウンタイプ</param>
		/// <param name="maxRecordCount">最大検索件数</param>
		/// <param name="result">結果</param>
		/// <returns>取得結果</returns>
		public static DataTable SelectGroupAsyList( string txtGroupCd, string pulldownType, int maxRecordCount, ref AiImageCheckBusiness.ImgCheckResultSet result ) {
			KTBindParameters param = new KTBindParameters();
			param.Add( "txtGroupCd", txtGroupCd );
			param.Add( "pulldownType", pulldownType );

			string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectGroupAsyList" );
			Cursor cursor = PicDao.GetInstance().OpenCursor( statementId, param );
			DataTable resultTable = null;

			try {
				while ( PicDao.GetInstance().Fetch( ref resultTable, ref cursor ) ) {
					if ( resultTable.Rows.Count > maxRecordCount ) {
						result.Message = new Msg( MsgManager.MESSAGE_WRN_61020 );
						resultTable.Rows[resultTable.Rows.Count - 1].Delete();
						break;
					}
				}
			} finally {
				PicDao.GetInstance().CloseCursor( ref cursor );
			}
			return resultTable;
		}

		/// <summary>
		/// 画像解析項目構成一覧検索処理
		/// </summary>
		/// <param name="itemCd">画像解析項目コード</param>
		/// <param name="itemNm">画像解析項目名</param>
		/// <param name="usingTool">利用ツール</param>
		/// <param name="pulldownType">プルダウンタイプ</param>
		/// <param name="lineCd">ラインコード</param>
		/// <param name="stationCd">ステーションコード</param>
		/// <param name="groupCd">画像解析グループコード</param>
		/// <param name="maxRecordCount">最大検索件数</param>
		/// <param name="result">結果</param>
		/// <returns>取得結果</returns>
		public static DataTable SelectGroupAsyJyoho( string itemCd, string itemNm, string usingTool, string pulldownType, string lineCd, string stationCd, string groupCd, int maxRecordCount, ref AiImageCheckBusiness.ImgCheckResultSet result ) {
			KTBindParameters param = new KTBindParameters();
			param.Add( "txtItemCd", itemCd );
			param.Add( "txtItemNm", itemNm );
			param.Add( "txtUsingTool", usingTool );
			param.Add( "txtLineCd", lineCd );
			param.Add( "txtStationCd", stationCd );
			param.Add( "pulldownType", pulldownType );
			param.Add( "txtGroupCd", groupCd );

			string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectGroupAsyJyoho" );
			Cursor cursor = PicDao.GetInstance().OpenCursor( statementId, param );
			DataTable resultTable = null;

			try {
				while ( PicDao.GetInstance().Fetch( ref resultTable, ref cursor ) ) {
					if ( resultTable.Rows.Count > maxRecordCount ) {
						result.Message = new Msg( MsgManager.MESSAGE_WRN_61020 );
						resultTable.Rows[resultTable.Rows.Count - 1].Delete();
						break;
					}
				}
			} finally {
				PicDao.GetInstance().CloseCursor( ref cursor );
			}
			return resultTable;
		}

		/// <summary>
		/// 画像解析項目構成データ削除
		/// </summary>
		/// <param name="dtUpdate">型式紐づけデータ</param>
		/// <param name="updBy">更新者</param>
		/// <param name="updSys">更新プログラム</param>
		/// <param name="exeCnt">更新件数</param>
		/// <returns>更新件数</returns>
		public static int DeleteGroupAsyJyohoData( DataTable dtUpdate, string updBy, string updSys, int exeCnt ) {
			int intRet = 0;
			int intExec = 0;
			//************************************
			//パラメータの設定
			//************************************

			//実行SQL
			string statementId1 = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "DeleteGroupAsyJyoho1" );
			string statementId2 = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "DeleteGroupAsyJyoho2" );

			try {
				//transaction発行
				PicDao.GetInstance().BeginTransaction();

				for ( int rowIdx = 0; rowIdx < dtUpdate.Rows.Count; rowIdx++ ) {
					KTBindParameters bindParam = new KTBindParameters();
					bindParam.Add( "paramItemCd", StringUtils.ToString( dtUpdate.Rows[rowIdx]["ITEM_CD"] ) );
					bindParam.Add( "paramGroupCd", StringUtils.ToString( dtUpdate.Rows[rowIdx]["GROUP_CD"] ) );
					bindParam.Add( "paramDisplayOrder", StringUtils.ToString( dtUpdate.Rows[rowIdx]["DISPLAY_ORDER"] ) );
					bindParam.Add( "paramUpdateBy", StringUtils.ToString( dtUpdate.Rows[rowIdx]["UPDATE_BY"] ) );
					bindParam.Add( "paramUpdatePg", StringUtils.ToString( dtUpdate.Rows[rowIdx]["UPDATE_PG"] ) );
					//削除
					intExec = 0;
					intExec = PicDao.GetInstance().Exec( statementId1, bindParam );
					if ( intExec == 1 ) {
						int recNum = PicDao.GetInstance().Exec( statementId2, bindParam );
					}

					intRet = intRet + intExec;
				}

				if ( exeCnt.Equals( intRet ) ) {
					//正常
					PicDao.GetInstance().CommitTransaction();
				} else {
					PicDao.GetInstance().RollbackTransaction();
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
				if ( true == PicDao.GetInstance().IsTransaction ) {
					try {
						PicDao.GetInstance().RollbackTransaction();
					} catch {
						intRet = -1;
					}
				}
			}

			return intRet;
		}

		/// <summary>
		/// 画像解析項目構成データ更新
		/// </summary>
		/// <param name="dtUpdate">型式紐づけデータ</param>
		/// <param name="updBy">更新者</param>
		/// <param name="updSys">更新プログラム</param>
		/// <param name="exeCnt">更新件数</param>
		/// <returns>更新件数</returns>
		public static int UpdateGroupAsyJyohoData( DataTable dtUpdate, string updBy, string updSys, int exeCnt ) {
			int intRet = 0;
			int intExec = 0;
			//************************************
			//パラメータの設定
			//************************************

			//実行SQL
			string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "UpdateGroupAsyJyoho" );

			try {
				//transaction発行
				PicDao.GetInstance().BeginTransaction();

				for ( int rowIdx = 0; rowIdx < dtUpdate.Rows.Count; rowIdx++ ) {
					KTBindParameters bindParam = new KTBindParameters();
					bindParam.Add( "paramItemCd", StringUtils.ToString( dtUpdate.Rows[rowIdx]["ITEM_CD"] ) );
					bindParam.Add( "paramGroupCd", StringUtils.ToString( dtUpdate.Rows[rowIdx]["GROUP_CD"] ) );
					bindParam.Add( "paramDisplayOrder", StringUtils.ToString( dtUpdate.Rows[rowIdx]["DISPLAY_ORDER"] ) );
					bindParam.Add( "paramIndexNo", StringUtils.ToString( dtUpdate.Rows[rowIdx]["INDEX_NO"] ) );
					bindParam.Add( "paramSflag", StringUtils.ToString( dtUpdate.Rows[rowIdx]["SFLAG"] ) );
					bindParam.Add( "paramCflag", StringUtils.ToString( dtUpdate.Rows[rowIdx]["CFLAG"] ) );
					bindParam.Add( "paramUpdateBy", StringUtils.ToString( dtUpdate.Rows[rowIdx]["UPDATE_BY"] ) );
					bindParam.Add( "paramUpdatePg", StringUtils.ToString( dtUpdate.Rows[rowIdx]["UPDATE_PG"] ) );
					//削除
					intExec = 0;
					intExec = PicDao.GetInstance().Exec( statementId, bindParam );
					intRet = intRet + intExec;
				}

				if ( exeCnt.Equals( intRet ) ) {
					//正常
					PicDao.GetInstance().CommitTransaction();
				} else {
					PicDao.GetInstance().RollbackTransaction();
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
				if ( true == PicDao.GetInstance().IsTransaction ) {
					try {
						PicDao.GetInstance().RollbackTransaction();
					} catch {
						intRet = -1;
					}
				}
			}

			return intRet;
		}

		/// <summary>
		/// UsingToolリスト取得 
		/// </summary>
		/// <param name="usingToolCd">UsingToolコード</param>
		/// <returns>検索結果詳細DataTable</returns>
		public static DataTable SelectAnlUsingToolList( string usingToolCd = "" ) {
			KTBindParameters bindParam = new KTBindParameters();

			//パラメータの設定
			bindParam.Add( "usingToolCd", usingToolCd );                      //station

			//SQL
			string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectAnlUsingToolList" );

			//検索
			return SelectPicData( statementId, bindParam );
		}

		/// <summary>
		/// 画像解析項目構成追加画面追加
		/// </summary>
		/// <param name="dtUpdate">型式紐づけデータ</param>
		/// <param name="updBy">更新者</param>
		/// <param name="updSys">更新プログラム</param>
		/// <param name="exeCnt">更新件数</param>
		/// <returns>更新件数</returns>
		public static int InsertGroupAsyJyohoData( DataTable dtUpdate, string updBy, string updSys, int exeCnt ) {
			int intRet = 0;
			int intExec = 0;
			//************************************
			//パラメータの設定
			//************************************

			//実行SQL
			string statementId1 = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "InsertGroupAsyJyoho1" );
			string statementId2 = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "InsertGroupAsyJyoho2" );

			try {
				//transaction発行
				PicDao.GetInstance().BeginTransaction();

				for ( int rowIdx = 0; rowIdx < dtUpdate.Rows.Count; rowIdx++ ) {
					KTBindParameters bindParam = new KTBindParameters();
					bindParam.Add( "paramItemCd", StringUtils.ToString( dtUpdate.Rows[rowIdx]["ITEM_CD"] ) );
					bindParam.Add( "paramGroupCd", StringUtils.ToString( dtUpdate.Rows[rowIdx]["GROUP_CD"] ) );
					bindParam.Add( "paramIndexNo", StringUtils.ToString( dtUpdate.Rows[rowIdx]["INDEX_NO"] ) );
					bindParam.Add( "paramSflag", StringUtils.ToString( dtUpdate.Rows[rowIdx]["SFLAG"] ) );
					bindParam.Add( "paramCflag", StringUtils.ToString( dtUpdate.Rows[rowIdx]["CFLAG"] ) );
					bindParam.Add( "paramUpdateBy", StringUtils.ToString( dtUpdate.Rows[rowIdx]["UPDATE_BY"] ) );
					bindParam.Add( "paramUpdatePg", StringUtils.ToString( dtUpdate.Rows[rowIdx]["UPDATE_PG"] ) );

					//追加
					intExec = 0;
					intExec = PicDao.GetInstance().Exec( statementId1, bindParam );
					if ( intExec == 0 ) {
						intExec = PicDao.GetInstance().Exec( statementId2, bindParam );
					}

					intRet = intRet + intExec;
				}

				if ( exeCnt.Equals( intRet ) ) {
					//追加
					PicDao.GetInstance().CommitTransaction();
				} else {
					PicDao.GetInstance().RollbackTransaction();
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
				if ( true == PicDao.GetInstance().IsTransaction ) {
					try {
						PicDao.GetInstance().RollbackTransaction();
					} catch {
						intRet = -1;
					}
				}
			}

			return intRet;
		}
	}
}