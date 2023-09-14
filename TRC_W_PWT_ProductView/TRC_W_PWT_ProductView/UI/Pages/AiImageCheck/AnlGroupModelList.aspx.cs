
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
	public partial class AnlGroupModelList : BaseForm {

		#region 定数

		#region 固定値、文字列
		/// <summary>
		/// 空き(表示用)
		/// </summary>
		const string FLAG_DISP = "";
		/// <summary>
		/// ゼロ値(表示用)
		/// </summary>
		const int FLAG_ZERO = 0;
		/// <summary>
		/// いち値(表示用)
		/// </summary>
		const string FLAG_ONE = "1";
		/// <summary>
		/// OK(表示用)
		/// </summary>
		const string FLAG_OK_DISP = "OK";
		/// <summary>
		/// NG(表示用)
		/// </summary>
		const string FLAG_NG_DISP = "NG";
		/// <summary>
		/// 型式紐づけ一覧画面(表示用)
		/// </summary>
		const string FLAG_IMAGE_LIST_DISP = "型式紐づけ一覧画面";
		/// <summary>
		/// オブジェクトダブルクリック
		/// </summary>
		const string ON_DBLCLICK = "ondblclick";
		/// <summary>
		/// メッセージ(Key)
		/// </summary>
		const string MSG_KEY = "MSG";
		
		/// <summary>
		/// △(ASC)
		/// </summary>
		const string ALLOW_UP = " △";
		/// <summary>
		/// ▼(DESC)
		/// </summary>
		const string ALLOW_DOWN = " ▼";

		private const int CONST_MAX_RECORD = 1000;

		#endregion

		#endregion

		#region 検索条件

		/// <summary>
		/// 検索条件
		/// </summary>
		public class CONDITION {

			/// ライン
			/// </summary>
			public static readonly ControlDefine LINE_NM = new ControlDefine( "lLineNm", "ライン", "lLineNm", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine LINE_NM_LBL = new ControlDefine( "txtLineNm", "ライン", "txtLineNm", ControlDefine.BindType.Both, typeof( string ) );

			/// ステーション
			/// </summary>
			public static readonly ControlDefine STATION_NM = new ControlDefine( "lStationNm", "ステーション", "lStationNm", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine STATION_NM_LBL = new ControlDefine( "txtStationNm", "ステーション", "txtStationNm", ControlDefine.BindType.Both, typeof( string ) );

			/// 設備
			/// </summary>
			public static readonly ControlDefine SETUBI = new ControlDefine( "lSetubi", "設備", "lSetubi", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SETUBILBL = new ControlDefine( "txtSetubi", "設備", "txtSetubi", ControlDefine.BindType.Both, typeof( string ) );

			///  画像解析グループコード
			/// </summary>
			public static readonly ControlDefine GROUP_CD = new ControlDefine( "lGroupCd", " 画像解析グループコード", "lGroupCd", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine GROUP_CD_LBL = new ControlDefine( "txtGroupCd", " 画像解析グループコード", "txtGroupCd", ControlDefine.BindType.Both, typeof( string ) );

			/// 画像解析グループ名
			/// </summary>
			public static readonly ControlDefine GROUP_NM = new ControlDefine( "lGroupNm", "画像解析グループ名", "lGroupNm", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine GROUP_NM_LBL = new ControlDefine( "txtGroupNm", "画像解析グループ名", "txtGroupNm", ControlDefine.BindType.Both, typeof( string ) );

			/// 情報1
			/// </summary>
			public static readonly ControlDefine JYOHO1 = new ControlDefine( "txtJyoho1", "情報1", "txtJyoho1", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO1LBL = new ControlDefine( "lJyoho1", "情報1", "lJyoho1", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 情報2
			/// </summary>
			public static readonly ControlDefine JYOHO2 = new ControlDefine( "txtJyoho2", "情報2", "txtJyoho2", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO2LBL = new ControlDefine( "lJyoho2", "情報2", "lJyoho2", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 情報3
			/// </summary>
			public static readonly ControlDefine JYOHO3 = new ControlDefine( "txtJyoho3", "情報3", "txtJyoho3", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO3LBL = new ControlDefine( "lJyoho3", "情報3", "lJyoho3", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 情報4
			/// </summary>
			public static readonly ControlDefine JYOHO4 = new ControlDefine( "txtJyoho4", "情報4", "txtJyoho4", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO4LBL = new ControlDefine( "lJyoho4", "情報4", "lJyoho4", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 情報5
			/// </summary>
			public static readonly ControlDefine JYOHO5 = new ControlDefine( "txtJyoho5", "情報5", "txtJyoho5", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO5LBL = new ControlDefine( "lJyoho5", "情報5", "lJyoho5", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 情報6
			/// </summary>
			public static readonly ControlDefine JYOHO6 = new ControlDefine( "txtJyoho6", "情報6", "txtJyoho6", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO6LBL = new ControlDefine( "lJyoho6", "情報6", "lJyoho6", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 情報7
			/// </summary>
			public static readonly ControlDefine JYOHO7 = new ControlDefine( "txtJyoho7", "情報7", "txtJyoho7", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO7LBL = new ControlDefine( "lJyoho7", "情報7", "lJyoho7", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 情報8
			/// </summary>
			public static readonly ControlDefine JYOHO8 = new ControlDefine( "txtJyoho8", "情報8", "txtJyoho8", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO8LBL = new ControlDefine( "lJyoho8", "情報8", "lJyoho8", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 情報9
			/// </summary>
			public static readonly ControlDefine JYOHO9 = new ControlDefine( "txtJyoho9", "情報9", "txtJyoho9", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO9LBL = new ControlDefine( "lJyoho9", "情報9", "lJyoho9", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 情報10
			/// </summary>
			public static readonly ControlDefine JYOHO10 = new ControlDefine( "txtJyoho10", "情報10", "txtJyoho10", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO10LBL = new ControlDefine( "lJyoho10", "情報10", "lJyoho10", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 情報11
			/// </summary>
			public static readonly ControlDefine JYOHO11 = new ControlDefine( "txtJyoho11", "情報11", "txtJyoho11", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO11LBL = new ControlDefine( "lJyoho11", "情報11", "lJyoho11", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 情報12
			/// </summary>
			public static readonly ControlDefine JYOHO12 = new ControlDefine( "txtJyoh12", "情報12", "txtJyoho12", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO12LBL = new ControlDefine( "lJyoho12", "情報12", "lJyoho12", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 情報13
			/// </summary>
			public static readonly ControlDefine JYOHO13 = new ControlDefine( "txtJyoho13", "情報13", "txtJyoho13", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO13LBL = new ControlDefine( "lJyoho13", "情報13", "lJyoho13", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 情報14
			/// </summary>
			public static readonly ControlDefine JYOHO14 = new ControlDefine( "txtJyoho14", "情報14", "txtJyoho14", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO14LBL = new ControlDefine( "lJyoho14", "情報14", "lJyoho14", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 情報15
			/// </summary>
			public static readonly ControlDefine JYOHO15 = new ControlDefine( "txtJyoho15", "情報15", "txtJyoho15", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO15LBL = new ControlDefine( "lJyoho15", "情報15", "lJyoho15", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 情報16
			/// </summary>
			public static readonly ControlDefine JYOHO16 = new ControlDefine( "txtJyoho16", "情報16", "txtJyoho16", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO16LBL = new ControlDefine( "lJyoho16", "情報16", "lJyoho16", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 情報17
			/// </summary>
			public static readonly ControlDefine JYOHO17 = new ControlDefine( "txtJyoho17", "情報3", "txtJyoho17", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO17LBL = new ControlDefine( "lJyoho17", "情報3", "lJyoho17", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 情報18
			/// </summary>
			public static readonly ControlDefine JYOHO18 = new ControlDefine( "txtJyoho18", "情報18", "txtJyoho18", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO18LBL = new ControlDefine( "lJyoho18", "情報18", "lJyoho18", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 情報19
			/// </summary>
			public static readonly ControlDefine JYOHO19 = new ControlDefine( "txtJyoho19", "情報19", "txtJyoho19", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO19LBL = new ControlDefine( "lJyoho19", "情報19", "lJyoho19", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 情報20
			/// </summary>
			public static readonly ControlDefine JYOHO20 = new ControlDefine( "txtJyoho20", "情報20", "txtJyoho20", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO20LBL = new ControlDefine( "lJyoho20", "情報20", "lJyoho20", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 情報21
			/// </summary>
			public static readonly ControlDefine JYOHO21 = new ControlDefine( "txtJyoho21", "情報21", "txtJyoho21", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO21LBL = new ControlDefine( "lJyoho21", "情報21", "lJyoho21", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 情報22
			/// </summary>
			public static readonly ControlDefine JYOHO22 = new ControlDefine( "txtJyoho22", "情報22", "txtJyoho22", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO22LBL = new ControlDefine( "lJyoho22", "情報22", "lJyoho22", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 情報23
			/// </summary>
			public static readonly ControlDefine JYOHO23 = new ControlDefine( "txtJyoho23", "情報23", "txtJyoho23", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO23LBL = new ControlDefine( "lJyoho23", "情報23", "lJyoho23", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 情報24
			/// </summary>
			public static readonly ControlDefine JYOHO24 = new ControlDefine( "txtJyoho24", "情報24", "txtJyoho24", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO24LBL = new ControlDefine( "lJyoho24", "情報24", "lJyoho24", ControlDefine.BindType.Both, typeof( string ) );
			/// 情報25
			/// </summary>
			public static readonly ControlDefine JYOHO25 = new ControlDefine( "txtJyoho25", "情報25", "txtJyoho25", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO25LBL = new ControlDefine( "lJyoho25", "情報25", "lJyoho25", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 情報26
			/// </summary>
			public static readonly ControlDefine JYOHO26 = new ControlDefine( "txtJyoho26", "情報26", "txtJyoho26", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO26LBL = new ControlDefine( "lJyoho26", "情報26", "lJyoho26", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 情報27
			/// </summary>
			public static readonly ControlDefine JYOHO27 = new ControlDefine( "txtJyoho27", "情報27", "txtJyoho27", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO27LBL = new ControlDefine( "lJyoho27", "情報27", "lJyoho27", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 情報28
			/// </summary>
			public static readonly ControlDefine JYOHO28 = new ControlDefine( "txtJyoho28", "情報28", "txtJyoho28", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO28LBL = new ControlDefine( "lJyoho28", "情報28", "lJyoho28", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 情報29
			/// </summary>
			public static readonly ControlDefine JYOHO29 = new ControlDefine( "txtJyoho29", "情報29", "txtJyoho29", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO29LBL = new ControlDefine( "lJyoho29", "情報29", "lJyoho29", ControlDefine.BindType.Both, typeof( string ) );
		}

		#endregion

		#region 変数
		/// <summary>
		/// クエリ条件リスト
		/// </summary>
		private static Dictionary<string, object> dicCondition;
		/// <summary>
		/// 表示用リスト
		/// </summary>
		private static Dictionary<string, string> dicDataList;
		/// <summary>
		/// 選択用リスト
		/// </summary>
		public static Dictionary<string, string> dicSelectedRow;
		#endregion

		#region グリッドビュー定義
		/// <summary>
		/// 一覧表示情報コントロール(左下)
		/// </summary>
		public class GRID_SEARCH_CONTROLS_L {
			public static readonly ControlDefine UPD_CHECK = new ControlDefine( "chkUpdate", "ﾁｪｯｸ", "check", ControlDefine.BindType.Both, null );
		}

		/// <summary>
		/// 一覧表示情報コントロール(右下)
		/// </summary>
		public class GRID_SEARCH_CONTROLS_R {
		}

		/// <summary>
		/// グリッドビュー定義
		/// </summary>
		public class GRID_MAIN {
			/// <summary>
			/// チェックボックス
			/// </summary>
			public static GridViewDefine UPD_CHECK = new GridViewDefine( "ﾁｪｯｸ", "check", typeof( string ), "", false, HorizontalAlign.Center, 60, true, false );
			/// <summary>
			/// 型式コード
			/// </summary>
			public static GridViewDefine MODEL_CD = new GridViewDefine( "型式コード", "ModelCd", typeof( string ), "", true, HorizontalAlign.Center, 150, true, true );
			/// <summary>
			/// 型式名
			/// </summary>
			public static GridViewDefine MODEL_NM = new GridViewDefine( "型式名", "ModelNM", typeof( string ), "", true, HorizontalAlign.Left, 200, true, true );
			/// <summary>
			/// 国コード
			/// </summary>
			public static GridViewDefine COUNTRY_CD = new GridViewDefine( "国コード", "CountryCd", typeof( string ), "", true, HorizontalAlign.Left, 90, true, true );
			/// <summary>
			/// ラインコード
			/// </summary>
			public static GridViewDefine LINE_CD = new GridViewDefine( "ラインコード", "LineCd", typeof( string ), "", true, HorizontalAlign.Left, 150, false, true );
			/// <summary>
			/// ステーションコード
			/// </summary>
			public static GridViewDefine STATION_CD = new GridViewDefine( "ステーションコード", "StationCd", typeof( string ), "", true, HorizontalAlign.Left, 150, false, true );
			

		}

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
		GridViewDefine[] _gridviewResults = null;

		/// <summary>
		/// 一覧定義情報アクセサ
		/// </summary>
		GridViewDefine[] GridViewResults {
			get {
				if ( true == ObjectUtils.IsNull( _gridviewResults ) ) {
					_gridviewResults = ControlUtils.GetGridViewDefineArray( typeof( GRID_MAIN ) );
				}
				return _gridviewResults;
			}
		}
		/// <summary>
		/// ユーザ情報
		/// </summary>
		private UserInfoSessionHandler.ST_USER _loginInfo;
		/// <summary>
		/// ユーザ情報
		/// </summary>
		public UserInfoSessionHandler.ST_USER LoginInfo {

			get {
				if ( true == ObjectUtils.IsNull( _loginInfo.UserInfo ) ) {
					SessionManagerInstance sesMgr = CurrentForm.SessionManager;
					_loginInfo = sesMgr.GetUserInfoHandler().GetUserInfo();
				}

				return _loginInfo;
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
		/// 削除ボタン押下時イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnDelete_Click( object sender, EventArgs e ) {
			base.RaiseEvent( DoDelete );
		}

		/// <summary>
		/// 閉じるボタン押下時イベント
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
		protected void grvMainViewLB_RowDataBound( object sender, GridViewRowEventArgs e ) {
			if ( e.Row.RowType == DataControlRowType.DataRow ) {

				DataRow row = ( (DataRowView)e.Row.DataItem ).Row;

				//ここで画面項目制御
				CheckBox chkDel = (CheckBox)e.Row.FindControl( GRID_SEARCH_CONTROLS_L.UPD_CHECK.controlId );
				chkDel.Checked = Convert.ToBoolean( row["check"].ToString() );

				Dictionary<string, object> dicControls = new Dictionary<string, object>();
				base.SetControlValues( e.Row, ControlUtils.GetControlDefineArray( typeof( GRID_SEARCH_CONTROLS_L ) ), row, ref dicControls );
			}

			RowDataBoundMainViewLB( sender, e );
		}

		/// <summary>
		/// 一覧データバインドイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void grvMainViewRB_RowDataBound( object sender, GridViewRowEventArgs e ) {

			RowDataBoundMainViewRB( sender, e );
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
			GridView frozenGrid = grvMainViewLB;
			ControlUtils.SetGridViewTemplateField( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, true ) );
			ControlUtils.SetGridViewTemplateField( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, false ) );

			ControlUtils.ReMakeBoundTemplateFieldCtrl( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, true ) );
			ControlUtils.ReMakeBoundTemplateFieldCtrl( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, false ) );
			int resultCnt = 0;

			if ( ObjectUtils.IsNotNull( ConditionInfo.ResultData ) ) {
				//検索結果がnullでない場合
				resultCnt = ConditionInfo.ResultData.Rows.Count;
			}
			//ページャー設定
			//ControlUtils.SetGridViewPager( ref pnlPager, grvDetailBodyRB, grvDetailBody_PageIndexChanging, resultCnt, grvDetailBodyRB.PageIndex );
			//辞書変数の初期化
			dicSelectedRow = new Dictionary<string, string>( AnlGroupModelView.dicSelectedRow );

			//グリッドの初期化
			if ( dicSelectedRow.Count > 0 ) {

				int idx = 0;
				DataTable dt = new DataTable( "testtable" );
				foreach ( KeyValuePair<string, string> item1 in dicSelectedRow ) {
					dt.Columns.Add( item1.Key, typeof( String ) );
				}
				DataRow dr = dt.NewRow();
				foreach ( KeyValuePair<string, string> item in dicSelectedRow ) {
					dr[item.Key.ToString()] = item.Value;
					idx++;
				}
				Dictionary<string, object> dicControlValues = new Dictionary<string, object>();
				//ヘッダ情報部データ設定
				CurrentForm.SetControlValues( ConditionControls, dr, ref dicControlValues );
				//ヘッダ情報部表示データ数計算
				hiddenNum.Text = ( ( idx - 10 ) / 2 ).ToString();
				//ヘッダ情報部データ表示
				ScriptManager.RegisterClientScriptBlock( this, this.GetType(), "AnlGroupList_setDisplay", "$(function(){AnlGroupModelList.setDisplay()});", true );

				ListItem[] liArr = new ListItem[5];
				liArr[0] = new ListItem( "exeKbn", "1" );
				liArr[1] = new ListItem( "groupCd", txtGroupCd.Text );
				liArr[2] = new ListItem( "lineCd", txtLineNm.Text.Split( ':' )[0] );
				liArr[3] = new ListItem( "stationCd", txtStationNm.Text.Split( ':' )[0] );
				liArr[4] = new ListItem( "setubi", txtSetubi.Text );
				btnInsert.Attributes[ControlUtils.ON_CLICK] = AiImageCheck.ImageInputModal.ImageCreateDispUrl( this, PageInfo.AnlGroupModelInput, 10, 10, liArr, "型式紐づけ一覧追加", "list" );
			}
			if ( IsPostBack == false ) {
				// セッションをクリアする
				ConditionInfo = new ConditionInfoSessionHandler.ST_CONDITION();
				//検索処理
				DoSearch();
			}
			
		}

		/// <summary>
		/// 初期化処理
		/// </summary>
		protected override void Initialize( ) {

			// アクセスカウンター登録
			Dao.Com.AccessCounterDao.Entry( base.CurrentPageInfo.pageId );

			// ベース処理初期化処理
			base.Initialize();

			

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

				// 件数初期処理
				ntbResultCount.Value = 0;
				divGrvDisplay.Visible = false;

				//ボタン非活性化
				btnDelete.Enabled = false;
				btnAllCheck.Enabled = false;
				//更新権限チェック
				AppPermission.PERMISSION_INFO permMainteInfo = AppPermission.GetTransactionPermission( PageInfo.AnlGroupModelInput, LoginInfo.UserInfo );
				if ( permMainteInfo.IsEdit == true ) {
					btnInsert.Enabled = true;
				} else {
					btnInsert.Enabled = false;
				}

			} catch ( Exception ex ) {
				// 例外ログ、メッセージ表示を実行する
				Logger.Exception( ex );
				base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
			}
		}

		#endregion

		#region 検索処理

		/// <summary>
		/// 検索処理
		/// </summary>
		private void DoSearch( ) {

			//検索パラメータ作成
			Dictionary<string, object> dicCondition = new Dictionary<string, object>();

			base.GetControlValues( ConditionControls, ref dicCondition );

			//検索時画面情報取得
			Dictionary<string, string> dicIdWithText = new Dictionary<string, string>();
			base.GetControlTexts( ConditionControls, out dicIdWithText );

			//一覧表示列の設定
			GridViewDefine[] gridColumns;
			gridColumns = ControlUtils.GetGridViewDefineArray( typeof( GRID_MAIN ) );

			//検索結果取得
			//エラー時にも最後まで処理を通すため、インスタンスを生成しておく
			AiImageCheckBusiness.ImgCheckResultSet result = new AiImageCheckBusiness.ImgCheckResultSet();
			DataTable tblResult = null;

			//int maxGridViewCount = WebAppInstance.GetInstance().Config.WebCommonInfo.maxGridViewCount;  //検索上限数
			//作業指示保存が重いため、4000件では耐えれない処理速度のため1000件としておく
			int maxGridViewCount = CONST_MAX_RECORD;
			try {
				result = AiImageCheckBusiness.SelectSetubiJyohoList( dicCondition, maxGridViewCount );

			} catch ( DataAccessException ex ) {
				if ( ex.OracleErrorNumber == DataAccessBase.ORAERROR_TIMEOUT ) {
					//クエリ発行タイムアウト
					base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_61910 );
				} else {
					//タイムアウト以外のException
					Logger.Exception( ex );
					base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80011, "検索処理で" );
				}
			} catch ( Exception ex ) {
				Logger.Exception( ex );
				base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_84010 );
			} finally {
				

			}
			ConditionInfoSessionHandler.ST_CONDITION cond = new ConditionInfoSessionHandler.ST_CONDITION();
			tblResult = result.ListTable;
			if ( null != tblResult ) {

				foreach ( DataRow row in tblResult.Rows ) {
					row["ModelCd"] = DataUtils.GetModelCdStr( StringUtils.ToString( row["ModelCd"] ) );
				}

				//件数表示
				ntbResultCount.Value = tblResult.Rows.Count;

				//検索条件/結果インスタンス
				cond.conditionValue = dicCondition;
				cond.IdWithText = dicIdWithText;
				cond.ResultData = tblResult.DefaultView.ToTable();

			} else {
				//タイムアウト等Exception時には、GridViewクリア
				ClearGridView();
			}
			//格納実施
			ConditionInfo = cond;

			//グリッドビューバインド
			GridView frozenGrid = grvMainViewLB;
			if ( true == ObjectUtils.IsNotNull( tblResult ) ) {
				if ( 0 < tblResult.Rows.Count ) {

					//新規バインド
					ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, true ), ConditionInfo, true );
					ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, false ), ConditionInfo, true );
					ControlUtils.BindGridView_WithTempField( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, true ), tblResult );
					ControlUtils.BindGridView_WithTempField( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, false ), tblResult );

					//ページャー作成
					//                  ControlUtils.SetGridViewPager( ref pnlPager, grvMainViewLB, grvMainView_PageIndexChanging, cond.ResultData.Rows.Count, grvMainViewLB.PageIndex );

					//グリッドビュー表示列情報修正
					//                  SetGridViewColumns();

					//GridView表示
					divGrvDisplay.Visible = true;

					//グリッドビュー外のDivサイズ変更
					SetDivGridViewWidth();
				} else {
					ClearGridView();
				}
			}

			//権限によるボタン制御
			AppPermission.PERMISSION_INFO permMainteInfo = AppPermission.GetTransactionPermission( PageInfo.AnlGroupModelInput, LoginInfo.UserInfo );
			if ( permMainteInfo.IsEdit == true ) {
				//更新権限所持＋データが存在する
				if ( tblResult != null && tblResult.Rows.Count > 0 ) {
					btnDelete.Enabled = true;
					btnAllCheck.Enabled = true;
				} else {
					btnDelete.Enabled = false;
					btnAllCheck.Enabled = false;
				}
			} else {
				btnDelete.Enabled = false;
				btnAllCheck.Enabled = false;
			}

			//メッセージ表示
			if ( null != result.Message ) {
				base.WriteApplicationMessage( result.Message );
			}
		}

		#endregion

		#region 削除処理

		/// <summary>
		/// 削除処理
		/// </summary>
		private void DoDelete( ) {
			int exeCnt = 0;
			//更新前権限チェック
			if ( CheckInputData() == false ) {
				return;
			}
			//更新前データ整形
			DataTable dtDelete = new DataTable();
			dtDelete.Columns.Add( "STATION_CD" );
			dtDelete.Columns.Add( "LINE_CD" );
			dtDelete.Columns.Add( "SETUBI" );
			dtDelete.Columns.Add( "MODEL_CD" );
			dtDelete.Columns.Add( "COUNTRY_CD" );
			//セッションから検索データ取得
			ConditionInfoSessionHandler.ST_CONDITION cond = base.ConditionInfo;
			if ( null == cond.ResultData || 0 == cond.ResultData.Rows.Count ) {
				//出力対象データなし
				base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_72040 );
				return;
			}
			foreach ( GridViewRow row in grvMainViewLB.Rows ) {
				CheckBox chkDel = (CheckBox)row.FindControl( GRID_SEARCH_CONTROLS_L.UPD_CHECK.controlId );
				if ( true == chkDel.Checked ) {

					DataRow row2 = dtDelete.NewRow();
					row2["STATION_CD"] = StringUtils.ToString( cond.ResultData.Rows[row.DataItemIndex][GRID_MAIN.STATION_CD.bindField] );
					row2["LINE_CD"] = StringUtils.ToString( cond.ResultData.Rows[row.DataItemIndex][GRID_MAIN.LINE_CD.bindField] );
					row2["SETUBI"] = txtSetubi.Value;
					row2["COUNTRY_CD"] = StringUtils.ToString( cond.ResultData.Rows[row.DataItemIndex][GRID_MAIN.COUNTRY_CD.bindField] );
					row2["MODEL_CD"] = DataUtils.GetModelCd( StringUtils.ToString( cond.ResultData.Rows[row.DataItemIndex][GRID_MAIN.MODEL_CD.bindField] ) );

					dtDelete.Rows.Add( row2 );

					exeCnt++;
				}
			}
			if ( exeCnt.Equals( 0 ) ) {
				//処理対象データなし
				CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62020, "削除対象データ" );
				return;
			}
			//削除処理
			int retCnt = 0;
			//更新
			retCnt = AiImageCheckDao.DeleteSetubiJyohoData( dtDelete, LoginInfo.UserInfo.userId, PageInfo.AnlGroupModelInput.pageId, exeCnt );

			if ( retCnt.Equals( exeCnt ) ) {
				//正常
				CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_INF_10030 );
			} else if ( retCnt != exeCnt ) {
				//処理件数不一致
				CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_72060,"削除" );
				return;
			} else {
				CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
				return;
			}

			//正常終了時、再検索
			DoSearch();

		}

		#endregion

		#region 一覧行データバインド

		/// <summary>
		/// 一覧行データバインド
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RowDataBound( params object[] parameters ) {

		}

		#endregion

		#endregion

		#region グリッドビュー操作

		/// <summary>
		/// グリッドビュークリア
		/// </summary>
		private void ClearGridView( ) {
			//列名非表示 グリッドビュークリア
			ControlUtils.InitializeGridView( grvHeaderLT, false );
			ControlUtils.InitializeGridView( grvHeaderRT, false );
			ControlUtils.InitializeGridView( grvMainViewLB, false );
			ControlUtils.InitializeGridView( grvMainViewRB, false );

			//グリッドビュー外のDivサイズ変更
			SetDivGridViewWidth();

			//件数表示
			ntbResultCount.Value = 0;

			//ページャークリア
			ControlUtils.ClearPager( ref pnlPager );

			//GridView非表示
			divGrvDisplay.Visible = false;

			//ボタンの活性状態切替
			SetButtonPermission();
		}

		/// <summary>
		/// グリッドビューページ切替
		/// </summary>
		/// <param name="parameters"></param>
		private void PageIndexChanging( params object[] parameters ) {
		}

		/// <summary>
		/// グリッドビュー外のDivサイズ変更
		/// </summary>
		private void SetDivGridViewWidth( ) {
			SetDivGridViewWidth( grvHeaderLT, divGrvHeaderLT );
			SetDivGridViewWidth( grvHeaderRT, divGrvHeaderRT );

			SetDivGridViewWidth( grvMainViewLB, divGrvLB );
			SetDivGridViewWidth( grvMainViewRB, divGrvRB );
		}

		/// <summary>
		/// 処理実行前データチェック
		/// </summary>
		/// <returns></returns>
		private bool CheckInputData( ) {

			//ユーザ情報チェック
			if ( true == ObjectUtils.IsNull( LoginInfo.UserInfo )
				|| true == StringUtils.IsEmpty( LoginInfo.UserInfo.userName ) ) {
				PageUtils.RedirectToErrorPage( Page, MsgManager.MESSAGE_ERR_72020, null );
				return false;
			}

			//更新権限チェック
			AppPermission.PERMISSION_INFO permMainteInfo = AppPermission.GetTransactionPermission( PageInfo.AnlGroupModelInput, LoginInfo.UserInfo );
			if ( permMainteInfo.IsEdit == false ) {
				PageUtils.RedirectToErrorPage( Page, MsgManager.MESSAGE_ERR_72030, null );
				return false;
			}

			return true;
		}

		/// <summary>
		/// グリッドビュー外のDivサイズ変更
		/// </summary>
		private void SetDivGridViewWidth( GridView grv, System.Web.UI.HtmlControls.HtmlGenericControl div ) {

			//セル幅補正 Padding4px + Border(片側) 1px
			const int CELL_PADDING = 4 * 2 + 1;
			//テーブル幅補正 Border(片側) 1px
			const int OUT_BORDER = 1;

			double sumWidth = 0;
			int showColCnt = 0;

			for ( int loop = 0; loop < grv.Columns.Count; loop++ ) {

				if ( false == grv.Columns[loop].Visible ) {
					continue;
				}

				sumWidth += grv.Columns[loop].HeaderStyle.Width.Value + CELL_PADDING;
				showColCnt += 1;
			}

			if ( 0 < showColCnt ) {
				sumWidth += OUT_BORDER;
			}

			div.Style["width"] = Convert.ToInt32( sumWidth ).ToString() + "px";
		}

		/// <summary>
		/// グリッドビュー行バインド(左下)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RowDataBoundMainViewLB( params object[] parameters ) {
			object sender = parameters[0];
			GridViewRowEventArgs e = (GridViewRowEventArgs)parameters[1];

			if ( e.Row.RowType == DataControlRowType.DataRow ) {

				DataRow row = ( (DataRowView)e.Row.DataItem ).Row;

				//ここで画面項目制御
				CheckBox chkDel = (CheckBox)e.Row.FindControl( GRID_SEARCH_CONTROLS_L.UPD_CHECK.controlId );

				Dictionary<string, object> dicControls = new Dictionary<string, object>();
				base.SetControlValues( e.Row, ControlUtils.GetControlDefineArray( typeof( GRID_SEARCH_CONTROLS_L ) ), row, ref dicControls );
			}
			ControlUtils.GridViewRowBound( (GridView)sender, e ,"");

		}

		/// <summary>
		/// グリッドビュー行バインド(右下)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RowDataBoundMainViewRB( params object[] parameters ) {
			object sender = parameters[0];
			GridViewRowEventArgs e = (GridViewRowEventArgs)parameters[1];

			if ( e.Row.RowType == DataControlRowType.DataRow ) {
				DataRow rowData = ( (DataRowView)e.Row.DataItem ).Row;
				Dictionary<string, object> dicControls = new Dictionary<string, object>();
				base.SetControlValues( e.Row, ControlUtils.GetControlDefineArray( typeof( GRID_SEARCH_CONTROLS_R ) ), rowData, ref dicControls );
			}

			ControlUtils.GridViewRowBound( (GridView)sender, e );

		}

		/// <summary>
		/// メッセージの復元
		/// </summary>
		private void RestoreMsg( ) {
		}

		#endregion

		#region ボトムボタン表示処理
		/// <summary>
		/// セッションの検索結果、ユーザー権限からボトムボタンを活性
		/// </summary>
		private void SetButtonPermission( ) {

			//初期化

			//セッションから検索結果を取得
			string token = StringUtils.Nvl( base.GetTransInfoValue( RequestParameter.Common.TOKEN ) );
			ConditionInfoSessionHandler.ST_CONDITION cond = base.SessionManager.GetConditionInfoHandler( token ).GetCondition( PageInfo.AnlGroupModelInput.pageId );
			DataTable tblResult = cond.ResultData;

			//ユーザ権限取得
			UserInfoSessionHandler.ST_USER loginInfo = base.SessionManager.GetUserInfoHandler().GetUserInfo();

			//検索結果の有無確認
			if ( null != tblResult && 0 < tblResult.Rows.Count ) {

				if ( true == ObjectUtils.IsNotNull( loginInfo.UserInfo ) ) {
					AppPermission.PERMISSION_INFO permInfo = AppPermission.GetTransactionPermission( PageInfo.AnlGroupModelInput, loginInfo.UserInfo );
					//更新権限があれば更新、削除ボタン活性
					if ( permInfo.IsEdit ) {
					}
				}
			}
		}
		#endregion
	}
}