
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls;
using KTFramework.C1Common.Excel;
using KTFramework.Common;
using KTFramework.Dao;
using TRC_W_PWT_ProductView.Business;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Dao.Com;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.Session;
using TRC_W_PWT_ProductView.UI.Base;
using TRC_W_PWT_ProductView.UI.Pages.UserControl;

namespace TRC_W_PWT_ProductView.UI.Pages.AiImageCheck {
	public partial class MainCorrectiveView : BaseForm {

		#region 定数

		#region 固定値、文字列
		/// <summary>
		/// 空き(表示用)
		/// </summary>
		const string FLAG_DISP = "";
		/// <summary>
		/// OK(表示用)
		/// </summary>
		const string FLAG_OK_DISP = "OK";
		/// <summary>
		/// NG(表示用)
		/// </summary>
		const string FLAG_NG_DISP = "NG";
		/// <summary>
		/// 是正処置入力検査結果一覧画面(表示用)
		/// </summary>
		const string FLAG_IMAGE_LIST_DISP = "是正処置入力検査結果一覧画面";
		/// <summary>
		/// オブジェクトダブルクリック
		/// </summary>
		const string ON_DBLCLICK = "ondblclick";
		/// <summary>
		/// 直行
		/// </summary>
		const int TYOKKO = 1;
		/// <summary>
		/// <summary>メッセージ(Key)</summary>
		/// </summary>
		const string MSG_KEY = "MSG";
		/// <summary>
		/// クエリ条件リスト
		/// </summary>
		private static Dictionary<string, object> dicCondition = null;


		#endregion

		#region 検索条件

		/// <summary>
		/// 検索条件
		/// </summary>
		public class CONDITION {
			/// <summary>
			/// ライン
			/// </summary>
			public static readonly ControlDefine LINE_CD = new ControlDefine( "ddlLineCd", "ライン", "ddlLineCd", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// ステーション
			/// </summary>
			public static readonly ControlDefine STATION = new ControlDefine( "ddlStation", "ステーション", "ddlStation", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 生産型式名
			/// </summary>
			public static readonly ControlDefine MODEL_NM = new ControlDefine( "txtModelNm", "生産型式名", "txtModelNm", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 生産型式コード
			/// </summary>
			public static readonly ControlDefine MODEL_CD = new ControlDefine( "txtModelCd", "生産型式コード", "txtModelCd", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 製品機番
			/// </summary>
			public static readonly ControlDefine KIBAN = new ControlDefine( "txtKiban", "製品機番", "txtKiban", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// IDNO
			/// </summary>
			public static readonly ControlDefine IDNO = new ControlDefine( "txtIdno", "IDNO", "txtIdno", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 月度連番
			/// </summary>
			public static readonly ControlDefine MONTHLYSEQNUM = new ControlDefine( "txtMonthlySeqNum", "月度連番", "txtMonthlySeqNum", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 検査結果
			/// </summary>
			public static readonly ControlDefine ANLRESULT = new ControlDefine( "ddlAnlResult", "検査結果", "ddlAnlResult", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 検査日開始
			/// </summary>
			public static readonly ControlDefine CLDSTART = new ControlDefine( "cldStart", "検査日開始", "cldStart", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 検査日終了
			/// </summary>
			public static readonly ControlDefine CLDEND = new ControlDefine( "cldEnd", "検査日終了", "cldEnd", ControlDefine.BindType.Both, typeof( string ) );
		}

		#endregion

		#endregion

		#region グリッドビュー定義

		/// <summary>
		/// グリッドビュー定義
		/// </summary>
		public class GRID_SEARCHDETAIL {
			/// <summary>
			/// ライン
			/// </summary>
			public static readonly GridViewDefine LINE_NM = new GridViewDefine( "ライン", "LineNm", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// ステーション
			/// </summary>
			public static readonly GridViewDefine STATION_NM = new GridViewDefine( "ステーション", "StationNm", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
			/// <summary>
			/// 生産型式
			/// </summary>
			public static readonly GridViewDefine MODEL_CD = new GridViewDefine( "生産型式", "ModelCd", typeof( string ), "", true, HorizontalAlign.Center, 120, true, true );
			/// <summary>
			/// 生産型式名
			/// </summary>
			public static readonly GridViewDefine MODEL_NM = new GridViewDefine( "生産型式名", "ModelNm", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
			/// <summary>
			/// 製品機番
			/// </summary>
			public static readonly GridViewDefine KIBAN = new GridViewDefine( "製品機番", "Kiban", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
			/// <summary>
			/// IDNO
			/// </summary>
			public static readonly GridViewDefine IDNO = new GridViewDefine( "IDNO", "Idno", typeof( string ), "", true, HorizontalAlign.Center, 90, true, true );
			/// <summary>
			/// 月度連番
			/// </summary>
			public static readonly GridViewDefine MONTHLY_SEQ_NUM = new GridViewDefine( "月度連番", "MonthlySeqNum", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
			/// <summary>
			/// 来歴
			/// </summary>
			public static readonly GridViewDefine RECORD_NO = new GridViewDefine( "来歴", "RecordNo", typeof( string ), "", true, HorizontalAlign.Center, 60, true, true );
			/// <summary>
			/// 検査結果
			/// </summary>
			public static readonly GridViewDefine ANL_RESULT = new GridViewDefine( "検査結果", "AnlReuslt", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
			/// <summary>
			/// 検査日時
			/// </summary>
			public static readonly GridViewDefine ANL_DATE = new GridViewDefine( "検査日時", "AnlDate", typeof( string ), "", true, HorizontalAlign.Center, 200, true, true );
		}

		/// <summary>
		/// グリッドビューグループ
		/// </summary>
		const string GRID_CORRECTIVE_SEARCH_VIEW_GROUP_CD = "MainCorrectiveView";

		#endregion

		#region プロパティ
		/// <summary>
		/// ロガー
		/// </summary>
		private static readonly Logger Logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		/// <summary>
		/// 表示中ページ(ベースフォーム)情報
		/// </summary>
		private BaseForm CurrentForm {
			get {
				return ( (BaseForm)Page );
			}
		}

		/// <summary>
		/// 検索条件定義情報
		/// </summary>
		ControlDefine[] _conditionControls = null;

		/// <summary>
		/// 検索条件定義情報アクセサ
		/// </summary>
		ControlDefine[] ConditionControls {
			get {
				if ( true == ObjectUtils.IsNull( _conditionControls ) ) {
					_conditionControls = ControlUtils.GetControlDefineArray( typeof( CONDITION ) );
				}
				return _conditionControls;
			}
		}

		/// <summary>
		/// 一覧定義情報
		/// </summary>
		GridViewDefine[] _gridviewDefault = null;

		/// <summary>
		/// 一覧定義情報アクセサ
		/// </summary>
		GridViewDefine[] GridviewDefault {
			get {
				if ( true == ObjectUtils.IsNull( _gridviewDefault ) ) {
					_gridviewDefault = ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHDETAIL ) );
				}
				return _gridviewDefault;
			}
		}

		#endregion

		#region イベント

		/// <summary>
		/// 画面ロード時イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load( object sender, EventArgs e ) {
			base.RaiseEvent( DoPageLoad, false );
		}

		/// <summary>
		/// 検索ボタン押下時イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnSearch_Click( object sender, EventArgs e ) {
			// 検索条件を作成するs
			base.GetControlValues( ConditionControls, ref dicCondition );
			base.RaiseEvent( DoSearch );
		}

		/// <summary>
		/// 是正処置入力検査結果一覧画面の閉じるボタン押下時イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnViewRefresh_Click( object sender, EventArgs e ) {
			base.RaiseEvent( DoSearch );
		}

		/// <summary>
		/// グリッドビューページチェンジイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void grvDetailBody_PageIndexChanging( object sender, CommandEventArgs e ) {
			base.RaiseEvent( PageIndexChanging, sender, e );
			RestoreMsg();
		}

		/// <summary>
		/// 一覧データバインドイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void grvSearch_RowDataBound( object sender, GridViewRowEventArgs e ) {
			try {
				ClearApplicationMessage();
				RowDataBound( sender, e );
			} catch ( Exception ex ) {
				//イベント処理中にエラー発生
				Logger.Exception( ex );
				throw ex;
			}
		}

		#endregion

		#region ページ処理

		/// <summary>
		/// ページロード処理
		/// </summary>
		protected override void DoPageLoad( ) {
			// ベースページロード処理
			base.DoPageLoad();
			// グリッドビューの再表示
			GridView frozenGrid = grvDetailBodyLB;
			ControlUtils.SetGridViewTemplateField( grvDetailBodyLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ) );
			ControlUtils.ReMakeBoundTemplateFieldCtrl( grvDetailBodyLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ) );
			int resultCnt = 0;
			if ( ObjectUtils.IsNotNull( ConditionInfo.ResultData ) ) {
				//検索結果がnullでない場合
				resultCnt = ConditionInfo.ResultData.Rows.Count;
			}
			ControlUtils.SetGridViewPager( ref pnlPager, grvDetailBodyLB, grvDetailBody_PageIndexChanging, resultCnt, grvDetailBodyLB.PageIndex );
		}

		/// <summary>
		/// 初期化処理
		/// </summary>
		protected override void Initialize( ) {
			// アクセスカウンター登録
			Dao.Com.AccessCounterDao.Entry( base.CurrentPageInfo.pageId );

			// ベース処理初期化処理
			base.Initialize();

			// セッションをクリアする
			ConditionInfo = new ConditionInfoSessionHandler.ST_CONDITION();

			// 初期処理
			InitializeValues();
		}

		#endregion

		#region 機能別処理

		#region 初期処理

		/// <summary>
		/// 初期処理
		/// </summary>
		private void InitializeValues( ) {
			try {
				// システム検査日を検索条件：検査日の開始の初期値に設定する
				String systemDate = DateUtils.ToString( DateTime.Now, DateUtils.DATE_FORMAT_DAY );
				DateTime? defaultDate = DateUtils.ToDateNullable( systemDate, DateUtils.DATE_FORMAT_DAY );
				//当月月初
				cldStart.Value = defaultDate;

				// システム検査結果を検索条件：検査結果リストの初期値に設定する
				ddlAnlResult.Items.Add( FLAG_DISP );
				ddlAnlResult.Items.Add( FLAG_OK_DISP );
				ddlAnlResult.Items.Add( FLAG_NG_DISP );

				// ライン一覧情報を検索条件：ラインのリストに設定する
				ListItem[] ServiceLineList = ControlUtils.GetListItems( AiImageCheckDao.SelectLineListByStation( ddlStation.SelectedValue ), AiImageCheckDao.COLNAME_TEXT, AiImageCheckDao.COLNAME_VALUE );
				if ( 0 < ServiceLineList.Length ) {
					//検索結果がnullでない場合
					ddlLineCd.Items.Clear();
					ddlLineCd.Items.Add( FLAG_DISP );
					ddlLineCd.Items.AddRange( ServiceLineList );
				}

				// ステーション一覧情報を検索条件：ステーションのリストに設定する
				ListItem[] ServiceStationList = ControlUtils.GetListItems( AiImageCheckDao.SelectStationListByLine( ddlLineCd.SelectedValue ), AiImageCheckDao.COLNAME_TEXT, AiImageCheckDao.COLNAME_VALUE );
				if ( 0 < ServiceStationList.Length ) {
					//検索結果がnullでない場合
					ddlStation.Items.Clear();
					ddlStation.Items.Add( FLAG_DISP );
					ddlStation.Items.AddRange( ServiceStationList );
				}
				// GridView非表示
				divGrvDisplay.Visible = false;

			} catch ( Exception ex ) {
				// 例外ログ、メッセージ表示を実行する
				Logger.Exception( ex );
				base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
			}
		}

		/// <summary>
		/// ステーションリスト選択動作
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ddlLineList_SelectedIndexChanged( object sender, EventArgs e ) {
			if ( true == StringUtils.IsNotEmpty( ddlStation.SelectedValue ) ) {
				base.RaiseEvent( ChangeLineListByStation );
			}
		}

		/// <summary>
		/// ステーション変更
		/// </summary>
		private void ChangeLineListByStation( ) {
			ListItem[] ServiceLineList = ControlUtils.GetListItems( AiImageCheckDao.SelectLineListByStation( ddlStation.SelectedValue ), AiImageCheckDao.COLNAME_TEXT, AiImageCheckDao.COLNAME_VALUE );

			if ( 0 < ServiceLineList.Length ) {
				//検索結果がnullでない場合
				ddlLineCd.SelectedValue = ServiceLineList[0].Value;
			}
		}

		/// <summary>
		/// ラインリスト選択動作
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ddlStationList_SelectedIndexChanged( object sender, EventArgs e ) {
			if ( true == StringUtils.IsNotEmpty( ddlLineCd.SelectedValue ) ) {
				base.RaiseEvent( ChangeStationListByLine );
			}
		}

		/// <summary>
		/// ライン変更
		/// </summary>
		private void ChangeStationListByLine( ) {
			ListItem[] ServiceLineList = ControlUtils.GetListItems( AiImageCheckDao.SelectStationListByLine( ddlLineCd.SelectedValue ), AiImageCheckDao.COLNAME_TEXT, AiImageCheckDao.COLNAME_VALUE );

			if ( 0 < ServiceLineList.Length ) {
				//検索結果がnullでない場合
				ddlStation.Items.Clear();
				ddlStation.Items.Add( FLAG_DISP );
				ddlStation.Items.AddRange( ServiceLineList );
			}
		}

		#endregion

		#region 検索処理

		/// <summary>
		/// 検索処理
		/// </summary>
		private void DoSearch( ) {

			// 必要な検索条件を確認する
			if ( true == StringUtils.IsEmpty( DataUtils.GetDictionaryStringVal( dicCondition, "ddlLineCd" ) ) ) {
				// メッセージが存在する場合、メッセージ表示
				base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62020, "ライン" );
				ClearGridView();
				return;
			} else if ( true == StringUtils.IsEmpty( DataUtils.GetDictionaryStringVal( dicCondition, "ddlStation" ) ) ) {
				// メッセージが存在する場合、メッセージ表示
				base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62020, "ステーション" );
				ClearGridView();
				return;
			};

			// 検索時画面情報を取得する
			Dictionary<string, string> dicIdWithText = new Dictionary<string, string>();
			base.GetControlTexts( ConditionControls, out dicIdWithText );

			// 検索結果取得
			// エラー時にも最後まで処理を通すため、インスタンスを生成しておく
			var result = new AiImageCheckBusiness.ImgCheckResultSet();
			DataTable tblResult = null;
			// 検索上限数を取得する
			int maxGridViewCount = WebAppInstance.GetInstance().Config.WebCommonInfo.maxGridViewCount;
			try {
				//throw new Exception();
				// 検索処理を実行する
				result = AiImageCheckBusiness.SearchOfSearchAnlResult( dicCondition, GridviewDefault, maxGridViewCount );

			} catch ( DataAccessException ex ) {
				if ( ex.OracleErrorNumber == DataAccessBase.ORAERROR_TIMEOUT ) {
					// クエリ発行タイムアウトが発生した場合、エラーメッセージを設定する
					base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_61910 );
				} else {
					// タイムアウト以外のExceptionが発生した場合、エラーメッセージを設定する
					Logger.Exception( ex );
					base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80011, "検索処理で" );
				}
			} catch ( Exception ex ) {
				// Exceptionが発生した場合、エラーメッセージを設定する
				Logger.Exception( ex );
				base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_84010 );
			} finally {
			}

			//---検索結果判定---
			// 処理結果が取得できた時のみ件数・ページャーの設定を実施
			ConditionInfoSessionHandler.ST_CONDITION cond = new ConditionInfoSessionHandler.ST_CONDITION();
			tblResult = result.ListTable;
			if ( true == ObjectUtils.IsNotNull( tblResult ) ) {
				// 検索結果が存在する場合、件数表示、ページャーの設定を行う
				ntbResultCount.Value = tblResult.Rows.Count;
				// ピッキング明細件数に応じてページャー設定
				ControlUtils.SetGridViewPager( ref pnlPager, grvDetailBodyLB, grvDetailBody_PageIndexChanging, tblResult.Rows.Count, 0 );
				// 検索条件/結果インスタンスを保持する
				cond.conditionValue = dicCondition;
				cond.IdWithText = dicIdWithText;
				cond.ResultData = tblResult.DefaultView.ToTable();
			} else {
				// タイムアウト等Exception時には、GridViewクリア
				ClearGridView();
			}

			// 検索条件をセッションに格納する
			ConditionInfo = cond;

			// グリッドビューの表示処理を行う
			GridView frozenGrid = grvDetailBodyLB;
			if ( true == ObjectUtils.IsNotNull( tblResult ) ) {
				if ( 0 < tblResult.Rows.Count ) {
					for ( int idx = frozenGrid.Columns.Count; idx < GridviewDefault.Length; idx++ ) {
						TemplateField tf = new TemplateField();
						tf.HeaderText = StringUtils.ToString( GridviewDefault[idx].bindField );
					}
					// 新規バインド
					ControlUtils.ShowGridViewHeader( grvDetailHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), ConditionInfo, true );
					ControlUtils.BindGridView_WithTempField( grvDetailBodyLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), tblResult );
					// GridView表示
					divGrvDisplay.Visible = true;
					// グリッドビュー外のDivサイズ変更
					SetDivGridViewWidth();
				} else {
					//一覧初期化
					ClearGridView();
				}
			}

			//メッセージ表示
			if ( null != result.Message ) {
				// メッセージが存在する場合、メッセージ表示
				base.WriteApplicationMessage( result.Message );
			}
		}

		#endregion

		#region 一覧行データバインド

		/// <summary>
		/// 一覧行データバインド
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RowDataBound( params object[] parameters ) {
			var sender = (GridView)parameters[0];
			var e = (GridViewRowEventArgs)parameters[1];
			if ( e.Row.RowType == DataControlRowType.DataRow ) {
				// データ行のみバインド処理を行う
				int index = 0;
				if ( true == GetColumnIndex( sender, GRID_SEARCHDETAIL.MONTHLY_SEQ_NUM, out index ) ) {
					// 月度連番の場合、yyyyMM-nnnnn形式に変換する（変換できない場合は空文字）
					var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHDETAIL.MONTHLY_SEQ_NUM.bindField].ToString();
					( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.getDisplayYMNum( data );
				}
				if ( true == GetColumnIndex( sender, GRID_SEARCHDETAIL.MODEL_CD, out index ) ) {
					// 生産型式の場合、XXXXX-XXXXX形式に変換する（変換できない場合は空文字）
					var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHDETAIL.MODEL_CD.bindField].ToString();
					( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.getDisplayKatasikiCode( data );
				}
				// 選択行の背景色変更を追加
				ControlUtils.GridViewRowBound( sender, e, GRID_CORRECTIVE_SEARCH_VIEW_GROUP_CD, ControlUtils.GridRowDoubleClickEvent.None );

				//ダブルクリックイベント
				var ddlLineCd = DataUtils.GetModelCd( DataUtils.GetDictionaryStringVal( dicCondition, "ddlLineCd" ) );  //ラインコード
				var ddlStation = DataUtils.GetModelCd( DataUtils.GetDictionaryStringVal( dicCondition, "ddlStation" ) );  //ステーション
				var txtIdno = ( (Label)e.Row.Cells[5].Controls[0] ).Text;               //IDNO
				var txtMonthlySeqNum = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHDETAIL.MONTHLY_SEQ_NUM.bindField].ToString();      //月度連番
				var txtRecordNo = ( (Label)e.Row.Cells[7].Controls[0] ).Text;           //来歴

				//keyの作成
				ListItem[] liArr = new ListItem[5];
				liArr[0] = new ListItem( "ddlLineCd", ddlLineCd );
				liArr[1] = new ListItem( "ddlStation", ddlStation );
				liArr[2] = new ListItem( "txtIdno", txtIdno );
				liArr[3] = new ListItem( "txtMonthlySeqNum", txtMonthlySeqNum );
				liArr[4] = new ListItem( "txtRecordNo", txtRecordNo );

				//ダブルクリック用
				e.Row.Attributes[ON_DBLCLICK] = AiImageCheck.ImageInputModal.ImageCreateDispUrl( this, PageInfo.MainCorrectiveList, 10, 10, liArr, FLAG_IMAGE_LIST_DISP, "list" );
			}
		}

		#endregion

		#endregion

		#region グリッドビュー操作

		/// <summary>
		/// グリッドビュークリア
		/// </summary>
		private void ClearGridView( ) {
			// 列名非表示 グリッドビュークリア
			ControlUtils.InitializeGridView( grvDetailHeaderLT, false );
			ControlUtils.InitializeGridView( grvDetailBodyLB, false );
			// 件数表示
			ntbResultCount.Value = 0;
			// ページャークリア
			ControlUtils.ClearPager( ref pnlPager );
			// GridView非表示
			divGrvDisplay.Visible = false;
		}

		/// <summary>
		/// グリッドビューページ切替
		/// </summary>
		/// <param name="parameters"></param>
		private void PageIndexChanging( params object[] parameters ) {
			object sender = parameters[0];
			CommandEventArgs e = (CommandEventArgs)parameters[1];
			int newPageIndex = Convert.ToInt32( e.CommandArgument );
			int pageSize = grvDetailBodyLB.PageSize;
			int rowCount = 0;
			if ( true == ObjectUtils.IsNotNull( ConditionInfo.ResultData ) ) {
				rowCount = ConditionInfo.ResultData.Rows.Count;
			}
			int allPages = 0;
			allPages = ConditionInfo.ResultData.Rows.Count / pageSize;
			if ( 0 != rowCount % pageSize ) {
				allPages += 1;
			}
			// ページが無くなっている場合には、先頭ページへ戻す
			if ( allPages <= newPageIndex ) {
				newPageIndex = 0;
			}
			//背面ユーザ切替対応
			ConditionInfoSessionHandler.ST_CONDITION cond = ConditionInfo;
			GridView frozenGrid = grvDetailBodyLB;
			ControlUtils.ShowGridViewHeader( grvDetailHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), cond, true );
			ControlUtils.BindGridView_WithTempField( grvDetailBodyLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), ConditionInfo.ResultData );
			ControlUtils.GridViewPageIndexChanging( grvDetailBodyLB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
			ControlUtils.SetGridViewPager( ref pnlPager, grvDetailBodyLB, grvDetailBody_PageIndexChanging, ConditionInfo.ResultData.Rows.Count, grvDetailBodyLB.PageIndex );
			// グリッドビュー外のDivサイズ変更
			SetDivGridViewWidth();
		}

		/// <summary>
		/// 列番号取得
		/// </summary>
		/// <param name="target">確認対象のグリッドビュー</param>
		/// <param name="def">確認する列定義</param>
		/// <param name="index">列番号</param>
		/// <returns>列定義がグリッドビューに含まれている場合はtrue、そうでなければfalse</returns>
		private bool GetColumnIndex( GridView target, GridViewDefine def, out int index ) {
			// 列番号を初期化
			index = 0;
			foreach ( DataControlField c in target.Columns ) {
				// グリッドビューの列を順次取得する
				if ( c.HeaderText == def.headerText ) {
					// グリッドビューの列のヘッダーテキストと列定義のヘッダーテキストが一致した場合、列が存在するとする
					return true;
				}
				// 列番号を加算する
				index++;
			}
			// すべての列を確認し、存在しなかった場合列が存在しなかったとする
			return false;
		}

		/// <summary>
		/// グリッドビュー外のDivサイズ変更
		/// </summary>
		private void SetDivGridViewWidth( ) {
			SetDivGridViewWidth( grvDetailHeaderLT, divGrvHeaderLT );
			SetDivGridViewWidth( grvDetailBodyLB, divGrvLB );
		}

		/// <summary>
		/// グリッドビュー外のDivサイズ変更
		/// </summary>
		private void SetDivGridViewWidth( GridView grv, System.Web.UI.HtmlControls.HtmlGenericControl div ) {
			// セル幅補正 Padding4px + Border(片側) 1px
			const int CELL_PADDING = 4 * 2 + 1;
			// テーブル幅補正 Border(片側) 1px
			const int OUT_BORDER = 1;
			var visibleColumns = grv.Columns.Cast<DataControlField>().Where( x => x.Visible ).ToList();
			int sumWidth = NumericUtils.ToInt( visibleColumns.Sum( x => x.HeaderStyle.Width.Value ) )
								+ CELL_PADDING * visibleColumns.Count()
								+ ( visibleColumns.Any() ? OUT_BORDER : 0 );
			div.Style["width"] = $"{ sumWidth }px";
		}

		/// <summary>
		/// メッセージの復元
		/// </summary>
		private void RestoreMsg( ) {
			var msg = CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).GetPageControlInfo( MSG_KEY );
			if ( 0 < msg.Count ) {
				// メッセージが設定されていた場合、メッセージ表示
				base.WriteApplicationMessage( (Msg)msg[MSG_KEY] );
			}
		}

		#endregion
	}
}