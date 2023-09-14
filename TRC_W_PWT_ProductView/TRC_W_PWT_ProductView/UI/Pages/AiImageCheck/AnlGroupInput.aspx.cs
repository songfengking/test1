using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Reflection;
using KTFramework.Common;
using KTFramework.Dao;
using KTWebInheritance.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.UI.Base;
using TRC_W_PWT_ProductView.Session;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.ListDefine;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.Dao.Com;
using TRC_W_PWT_ProductView.Dao.Process;
using TRC_W_PWT_ProductView.Business;
using System.Diagnostics;
using KTWebControl.CustomControls;

namespace TRC_W_PWT_ProductView.UI.Pages.AiImageCheck {
	public partial class AnlGroupInput : BaseForm {

		#region 定数定義

		//ロガー定義
		private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );
		const string CLOSE_MODAL_DISP = "AnlGroupInput.CloseModal();";

		/// <summary>
		/// 空き(表示用)
		/// </summary>
		private const string FLAG_DISP = "";
		/// <summary>
		/// 利用ツールプルダウンタイプ
		/// </summary>
		public const string PULLDOWNTYPE_USINGTOOL = "3";
		/// <summary>
		/// 入力不可フラグ
		/// </summary>
		public const string INPUT_DISABLE = "1";
		/// <summary>
		/// 入力可能フラグ
		/// </summary>
		public const string INPUT_ENABLE = "2";
		/// <summary>
		/// 入力必須フラグ
		/// </summary>
		public const string INPUT_NOTNULL = "3";
		/// <summary>
		/// 入力最大桁数
		/// </summary>
		public const int INPUT_NUM = 20;
		/// <summary>
		/// 選択用リスト
		/// </summary>
		public static Dictionary<string, string> dicSelectedRow;
		/// <summary>
		/// クエリ条件リスト
		/// </summary>
		private static Dictionary<string, object> dicCondition;

		//処理ページ区分
		enum ExecKbn : int {
			SELECT = 0,
			INSERT = 1,
			UPDATE = 2,
			DELETE = 3,
			COPY = 5,
			STRUCTURE = 6
		}
		#endregion

		#region 変数定義

		private static Dictionary<string, object> _orgData = new Dictionary<string, object>();
		private static Dictionary<string, string> _resultData = new Dictionary<string, string>();
		private int _execKbn = 1;

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
			public static readonly ControlDefine JYOHO2 = new ControlDefine( "txtJyoho2", "情報1", "txtJyoho2", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOHO2LBL = new ControlDefine( "lJyoho2", "情報1", "lJyoho2", ControlDefine.BindType.Both, typeof( string ) );
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

		/// <summary>
		/// 入力検索条件
		/// </summary>
		public class GRID_CONDITION {
			/// <summary>
			/// ステーション
			/// </summary>
			public static readonly ControlDefine STATION = new ControlDefine( "txtStationCd", "ステーション", "txtStationCd", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// ライン
			/// </summary>
			public static readonly ControlDefine LINE_CD = new ControlDefine( "txtLineCd", "ライン", "txtLineCd", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 設備
			/// </summary>
			public static readonly ControlDefine SETUBI = new ControlDefine( "txtSetubi", "設備", "txtSetubi", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 画像解析グループコード
			/// </summary>
			public static readonly ControlDefine IMG_GROUP_CD = new ControlDefine( "txtGroupCd", "画像解析グループコード", "txtGroupCd", ControlDefine.BindType.Both, typeof( string ) );
			/// <summary>
			/// 画像解析グループ名
			/// </summary>
			public static readonly ControlDefine IMG_GROUP_NM = new ControlDefine( "txtGroupNm", "画像解析グループ名", "txtGroupNm", ControlDefine.BindType.Both, typeof( string ) );
		}

		/// <summary>
		/// グリッドビュー定義
		/// </summary>
		public class GRID_SEARCHDETAIL {

			/// <summary>
			/// 情報1
			/// </summary>
			public static readonly ControlDefine KOMOKU_1 = new ControlDefine( "KOMOKU_1", "情報1", "KOMOKU_1", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_1 = new ControlDefine( "YUKO_1", "情報1", "YUKO_1", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_1 = new ControlDefine( "SHUBETU_1", "情報1", "SHUBETU_1", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_1 = new ControlDefine( "MEASURE_1", "情報1", "MEASURE_1", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_1 = new ControlDefine( "ZENKETA_1", "情報1", "ZENKETA_1", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_1 = new ControlDefine( "SYOSU_1", "情報1", "SYOSU_1", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_1 = new ControlDefine( "HISSU_1", "情報1", "HISSU_1", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_1 = new ControlDefine( "KAGEN_1", "情報1", "KAGEN_1", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_1 = new ControlDefine( "JYOGEN_1", "情報1", "JYOGEN_1", ControlDefine.BindType.Both, typeof( string ) );

			/// <summary>
			/// 情報2
			/// </summary>
			public static readonly ControlDefine KOMOKU_2 = new ControlDefine( "KOMOKU_2", "情報2", "KOMOKU_2", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_2 = new ControlDefine( "YUKO_2", "情報2", "YUKO_2", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_2 = new ControlDefine( "SHUBETU_2", "情報2", "SHUBETU_2", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_2 = new ControlDefine( "MEASURE_2", "情報2", "MEASURE_2", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_2 = new ControlDefine( "ZENKETA_2", "情報2", "ZENKETA_2", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_2 = new ControlDefine( "SYOSU_2", "情報2", "SYOSU_2", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_2 = new ControlDefine( "HISSU_2", "情報2", "HISSU_2", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_2 = new ControlDefine( "KAGEN_2", "情報2", "KAGEN_2", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_2 = new ControlDefine( "JYOGEN_2", "情報2", "JYOGEN_2", ControlDefine.BindType.Both, typeof( string ) );

			/// <summary>
			/// 情報3
			/// </summary>
			public static readonly ControlDefine KOMOKU_3 = new ControlDefine( "KOMOKU_3", "情報3", "KOMOKU_3", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_3 = new ControlDefine( "YUKO_3", "情報3", "YUKO_3", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_3 = new ControlDefine( "SHUBETU_3", "情報3", "SHUBETU_3", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_3 = new ControlDefine( "MEASURE_3", "情報3", "MEASURE_3", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_3 = new ControlDefine( "ZENKETA_3", "情報3", "ZENKETA_3", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_3 = new ControlDefine( "SYOSU_3", "情報3", "SYOSU_3", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_3 = new ControlDefine( "HISSU_3", "情報3", "HISSU_3", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_3 = new ControlDefine( "KAGEN_3", "情報3", "KAGEN_3", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_3 = new ControlDefine( "JYOGEN_3", "情報3", "JYOGEN_3", ControlDefine.BindType.Both, typeof( string ) );

			/// <summary>
			/// 情報4
			/// </summary>
			public static readonly ControlDefine KOMOKU_4 = new ControlDefine( "KOMOKU_4", "情報4", "KOMOKU_4", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_4 = new ControlDefine( "YUKO_4", "情報4", "YUKO_4", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_4 = new ControlDefine( "SHUBETU_4", "情報4", "SHUBETU_4", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_4 = new ControlDefine( "MEASURE_4", "情報4", "MEASURE_4", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_4 = new ControlDefine( "ZENKETA_4", "情報4", "ZENKETA_4", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_4 = new ControlDefine( "SYOSU_4", "情報4", "SYOSU_4", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_4 = new ControlDefine( "HISSU_4", "情報4", "HISSU_4", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_4 = new ControlDefine( "KAGEN_4", "情報4", "KAGEN_4", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_4 = new ControlDefine( "JYOGEN_4", "情報4", "JYOGEN_4", ControlDefine.BindType.Both, typeof( string ) );

			/// <summary>
			/// 情報5
			/// </summary>
			public static readonly ControlDefine KOMOKU_5 = new ControlDefine( "KOMOKU_5", "情報5", "KOMOKU_5", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_5 = new ControlDefine( "YUKO_5", "情報5", "YUKO_5", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_5 = new ControlDefine( "SHUBETU_5", "情報5", "SHUBETU_5", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_5 = new ControlDefine( "MEASURE_5", "情報5", "MEASURE_5", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_5 = new ControlDefine( "ZENKETA_5", "情報5", "ZENKETA_5", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_5 = new ControlDefine( "SYOSU_5", "情報5", "SYOSU_5", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_5 = new ControlDefine( "HISSU_5", "情報5", "HISSU_5", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_5 = new ControlDefine( "KAGEN_5", "情報5", "KAGEN_5", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_5 = new ControlDefine( "JYOGEN_5", "情報5", "JYOGEN_5", ControlDefine.BindType.Both, typeof( string ) );

			/// <summary>
			/// 情報6
			/// </summary>
			public static readonly ControlDefine KOMOKU_6 = new ControlDefine( "KOMOKU_6", "情報6", "KOMOKU_6", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_6 = new ControlDefine( "YUKO_6", "情報6", "YUKO_6", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_6 = new ControlDefine( "SHUBETU_6", "情報6", "SHUBETU_6", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_6 = new ControlDefine( "MEASURE_6", "情報6", "MEASURE_6", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_6 = new ControlDefine( "ZENKETA_6", "情報6", "ZENKETA_6", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_6 = new ControlDefine( "SYOSU_6", "情報6", "SYOSU_6", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_6 = new ControlDefine( "HISSU_6", "情報6", "HISSU_6", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_6 = new ControlDefine( "KAGEN_6", "情報6", "KAGEN_6", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_6 = new ControlDefine( "JYOGEN_6", "情報6", "JYOGEN_6", ControlDefine.BindType.Both, typeof( string ) );

			/// <summary>
			/// 情報7
			/// </summary>
			public static readonly ControlDefine KOMOKU_7 = new ControlDefine( "KOMOKU_7", "情報7", "KOMOKU_7", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_7 = new ControlDefine( "YUKO_7", "情報7", "YUKO_7", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_7 = new ControlDefine( "SHUBETU_7", "情報7", "SHUBETU_7", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_7 = new ControlDefine( "MEASURE_7", "情報7", "MEASURE_7", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_7 = new ControlDefine( "ZENKETA_7", "情報7", "ZENKETA_7", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_7 = new ControlDefine( "SYOSU_7", "情報7", "SYOSU_7", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_7 = new ControlDefine( "HISSU_7", "情報7", "HISSU_7", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_7 = new ControlDefine( "KAGEN_7", "情報7", "KAGEN_7", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_7 = new ControlDefine( "JYOGEN_7", "情報7", "JYOGEN_7", ControlDefine.BindType.Both, typeof( string ) );

			/// <summary>
			/// 情報8
			/// </summary>
			public static readonly ControlDefine KOMOKU_8 = new ControlDefine( "KOMOKU_8", "情報8", "KOMOKU_8", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_8 = new ControlDefine( "YUKO_8", "情報8", "YUKO_8", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_8 = new ControlDefine( "SHUBETU_8", "情報8", "SHUBETU_8", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_8 = new ControlDefine( "MEASURE_8", "情報8", "MEASURE_8", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_8 = new ControlDefine( "ZENKETA_8", "情報8", "ZENKETA_8", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_8 = new ControlDefine( "SYOSU_8", "情報8", "SYOSU_8", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_8 = new ControlDefine( "HISSU_8", "情報8", "HISSU_8", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_8 = new ControlDefine( "KAGEN_8", "情報8", "KAGEN_8", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_8 = new ControlDefine( "JYOGEN_8", "情報8", "JYOGEN_8", ControlDefine.BindType.Both, typeof( string ) );

			/// <summary>
			/// 情報9
			/// </summary>
			public static readonly ControlDefine KOMOKU_9 = new ControlDefine( "KOMOKU_9", "情報9", "KOMOKU_9", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_9 = new ControlDefine( "YUKO_9", "情報9", "YUKO_9", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_9 = new ControlDefine( "SHUBETU_9", "情報9", "SHUBETU_9", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_9 = new ControlDefine( "MEASURE_9", "情報9", "MEASURE_9", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_9 = new ControlDefine( "ZENKETA_9", "情報9", "ZENKETA_9", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_9 = new ControlDefine( "SYOSU_9", "情報9", "SYOSU_9", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_9 = new ControlDefine( "HISSU_9", "情報9", "HISSU_9", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_9 = new ControlDefine( "KAGEN_9", "情報9", "KAGEN_9", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_9 = new ControlDefine( "JYOGEN_9", "情報9", "JYOGEN_9", ControlDefine.BindType.Both, typeof( string ) );

			/// <summary>
			/// 情報10
			/// </summary>
			public static readonly ControlDefine KOMOKU_10 = new ControlDefine( "KOMOKU_10", "情報10", "KOMOKU_10", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_10 = new ControlDefine( "YUKO_10", "情報10", "YUKO_10", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_10 = new ControlDefine( "SHUBETU_10", "情報10", "SHUBETU_10", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_10 = new ControlDefine( "MEASURE_10", "情報10", "MEASURE_10", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_10 = new ControlDefine( "ZENKETA_10", "情報10", "ZENKETA_10", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_10 = new ControlDefine( "SYOSU_10", "情報10", "SYOSU_10", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_10 = new ControlDefine( "HISSU_10", "情報10", "HISSU_10", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_10 = new ControlDefine( "KAGEN_10", "情報10", "KAGEN_10", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_10 = new ControlDefine( "JYOGEN_10", "情報10", "JYOGEN_10", ControlDefine.BindType.Both, typeof( string ) );

			/// <summary>
			/// 情報11
			/// </summary>
			public static readonly ControlDefine KOMOKU_11 = new ControlDefine( "KOMOKU_11", "情報11", "KOMOKU_11", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_11 = new ControlDefine( "YUKO_11", "情報11", "YUKO_11", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_11 = new ControlDefine( "SHUBETU_11", "情報11", "SHUBETU_11", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_11 = new ControlDefine( "MEASURE_11", "情報11", "MEASURE_11", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_11 = new ControlDefine( "ZENKETA_11", "情報11", "ZENKETA_11", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_11 = new ControlDefine( "SYOSU_11", "情報11", "SYOSU_11", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_11 = new ControlDefine( "HISSU_11", "情報11", "HISSU_11", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_11 = new ControlDefine( "KAGEN_11", "情報11", "KAGEN_11", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_11 = new ControlDefine( "JYOGEN_11", "情報11", "JYOGEN_11", ControlDefine.BindType.Both, typeof( string ) );

			/// <summary>
			/// 情報12
			/// </summary>
			public static readonly ControlDefine KOMOKU_12 = new ControlDefine( "KOMOKU_12", "情報12", "KOMOKU_12", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_12 = new ControlDefine( "YUKO_12", "情報12", "YUKO_12", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_12 = new ControlDefine( "SHUBETU_12", "情報12", "SHUBETU_12", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_12 = new ControlDefine( "MEASURE_12", "情報12", "MEASURE_12", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_12 = new ControlDefine( "ZENKETA_12", "情報12", "ZENKETA_12", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_12 = new ControlDefine( "SYOSU_12", "情報12", "SYOSU_12", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_12 = new ControlDefine( "HISSU_12", "情報12", "HISSU_12", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_12 = new ControlDefine( "KAGEN_12", "情報12", "KAGEN_12", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_12 = new ControlDefine( "JYOGEN_12", "情報12", "JYOGEN_12", ControlDefine.BindType.Both, typeof( string ) );

			/// <summary>
			/// 情報13
			/// </summary>
			public static readonly ControlDefine KOMOKU_13 = new ControlDefine( "KOMOKU_13", "情報13", "KOMOKU_13", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_13 = new ControlDefine( "YUKO_13", "情報13", "YUKO_13", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_13 = new ControlDefine( "SHUBETU_13", "情報13", "SHUBETU_13", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_13 = new ControlDefine( "MEASURE_13", "情報13", "MEASURE_13", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_13 = new ControlDefine( "ZENKETA_13", "情報13", "ZENKETA_13", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_13 = new ControlDefine( "SYOSU_13", "情報13", "SYOSU_13", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_13 = new ControlDefine( "HISSU_13", "情報13", "HISSU_13", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_13 = new ControlDefine( "KAGEN_13", "情報13", "KAGEN_13", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_13 = new ControlDefine( "JYOGEN_13", "情報13", "JYOGEN_13", ControlDefine.BindType.Both, typeof( string ) );

			/// <summary>
			/// 情報14
			/// </summary>
			public static readonly ControlDefine KOMOKU_14 = new ControlDefine( "KOMOKU_14", "情報14", "KOMOKU_14", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_14 = new ControlDefine( "YUKO_14", "情報14", "YUKO_14", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_14 = new ControlDefine( "SHUBETU_14", "情報14", "SHUBETU_14", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_14 = new ControlDefine( "MEASURE_14", "情報14", "MEASURE_14", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_14 = new ControlDefine( "ZENKETA_14", "情報14", "ZENKETA_14", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_14 = new ControlDefine( "SYOSU_14", "情報14", "SYOSU_14", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_14 = new ControlDefine( "HISSU_14", "情報14", "HISSU_14", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_14 = new ControlDefine( "KAGEN_14", "情報14", "KAGEN_14", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_14 = new ControlDefine( "JYOGEN_14", "情報14", "JYOGEN_14", ControlDefine.BindType.Both, typeof( string ) );

			/// <summary>
			/// 情報15
			/// </summary>
			public static readonly ControlDefine KOMOKU_15 = new ControlDefine( "KOMOKU_15", "情報15", "KOMOKU_15", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_15 = new ControlDefine( "YUKO_15", "情報15", "YUKO_15", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_15 = new ControlDefine( "SHUBETU_15", "情報15", "SHUBETU_15", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_15 = new ControlDefine( "MEASURE_15", "情報15", "MEASURE_15", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_15 = new ControlDefine( "ZENKETA_15", "情報15", "ZENKETA_15", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_15 = new ControlDefine( "SYOSU_15", "情報15", "SYOSU_15", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_15 = new ControlDefine( "HISSU_15", "情報15", "HISSU_15", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_15 = new ControlDefine( "KAGEN_15", "情報15", "KAGEN_15", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_15 = new ControlDefine( "JYOGEN_15", "情報15", "JYOGEN_15", ControlDefine.BindType.Both, typeof( string ) );

			/// <summary>
			/// 情報16
			/// </summary>
			public static readonly ControlDefine KOMOKU_16 = new ControlDefine( "KOMOKU_16", "情報16", "KOMOKU_16", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_16 = new ControlDefine( "YUKO_16", "情報16", "YUKO_16", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_16 = new ControlDefine( "SHUBETU_16", "情報16", "SHUBETU_16", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_16 = new ControlDefine( "MEASURE_16", "情報16", "MEASURE_16", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_16 = new ControlDefine( "ZENKETA_16", "情報16", "ZENKETA_16", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_16 = new ControlDefine( "SYOSU_16", "情報16", "SYOSU_16", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_16 = new ControlDefine( "HISSU_16", "情報16", "HISSU_16", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_16 = new ControlDefine( "KAGEN_16", "情報16", "KAGEN_16", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_16 = new ControlDefine( "JYOGEN_16", "情報16", "JYOGEN_16", ControlDefine.BindType.Both, typeof( string ) );

			/// <summary>
			/// 情報17
			/// </summary>
			public static readonly ControlDefine KOMOKU_17 = new ControlDefine( "KOMOKU_17", "情報17", "KOMOKU_17", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_17 = new ControlDefine( "YUKO_17", "情報17", "YUKO_17", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_17 = new ControlDefine( "SHUBETU_17", "情報17", "SHUBETU_17", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_17 = new ControlDefine( "MEASURE_17", "情報17", "MEASURE_17", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_17 = new ControlDefine( "ZENKETA_17", "情報17", "ZENKETA_17", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_17 = new ControlDefine( "SYOSU_17", "情報17", "SYOSU_17", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_17 = new ControlDefine( "HISSU_17", "情報17", "HISSU_17", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_17 = new ControlDefine( "KAGEN_17", "情報17", "KAGEN_17", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_17 = new ControlDefine( "JYOGEN_17", "情報17", "JYOGEN_17", ControlDefine.BindType.Both, typeof( string ) );

			/// <summary>
			/// 情報18
			/// </summary>
			public static readonly ControlDefine KOMOKU_18 = new ControlDefine( "KOMOKU_18", "情報18", "KOMOKU_18", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_18 = new ControlDefine( "YUKO_18", "情報18", "YUKO_18", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_18 = new ControlDefine( "SHUBETU_18", "情報18", "SHUBETU_18", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_18 = new ControlDefine( "MEASURE_18", "情報18", "MEASURE_18", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_18 = new ControlDefine( "ZENKETA_18", "情報18", "ZENKETA_18", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_18 = new ControlDefine( "SYOSU_18", "情報18", "SYOSU_18", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_18 = new ControlDefine( "HISSU_18", "情報18", "HISSU_18", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_18 = new ControlDefine( "KAGEN_18", "情報18", "KAGEN_18", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_18 = new ControlDefine( "JYOGEN_18", "情報18", "JYOGEN_18", ControlDefine.BindType.Both, typeof( string ) );

			/// <summary>
			/// 情報19
			/// </summary>
			public static readonly ControlDefine KOMOKU_19 = new ControlDefine( "KOMOKU_19", "情報19", "KOMOKU_19", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_19 = new ControlDefine( "YUKO_19", "情報19", "YUKO_19", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_19 = new ControlDefine( "SHUBETU_19", "情報19", "SHUBETU_19", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_19 = new ControlDefine( "MEASURE_19", "情報19", "MEASURE_19", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_19 = new ControlDefine( "ZENKETA_19", "情報19", "ZENKETA_19", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_19 = new ControlDefine( "SYOSU_19", "情報19", "SYOSU_19", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_19 = new ControlDefine( "HISSU_19", "情報19", "HISSU_19", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_19 = new ControlDefine( "KAGEN_19", "情報19", "KAGEN_19", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_19 = new ControlDefine( "JYOGEN_19", "情報19", "JYOGEN_19", ControlDefine.BindType.Both, typeof( string ) );

			/// <summary>
			/// 情報20
			/// </summary>
			public static readonly ControlDefine KOMOKU_20 = new ControlDefine( "KOMOKU_20", "情報20", "KOMOKU_20", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_20 = new ControlDefine( "YUKO_20", "情報20", "YUKO_20", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_20 = new ControlDefine( "SHUBETU_20", "情報20", "SHUBETU_20", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_20 = new ControlDefine( "MEASURE_20", "情報20", "MEASURE_20", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_20 = new ControlDefine( "ZENKETA_20", "情報20", "ZENKETA_20", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_20 = new ControlDefine( "SYOSU_20", "情報20", "SYOSU_20", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_20 = new ControlDefine( "HISSU_20", "情報20", "HISSU_20", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_20 = new ControlDefine( "KAGEN_20", "情報20", "KAGEN_20", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_20 = new ControlDefine( "JYOGEN_20", "情報20", "JYOGEN_20", ControlDefine.BindType.Both, typeof( string ) );

			/// <summary>
			/// 情報21
			/// </summary>
			public static readonly ControlDefine KOMOKU_21 = new ControlDefine( "KOMOKU_21", "情報21", "KOMOKU_21", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_21 = new ControlDefine( "YUKO_21", "情報21", "YUKO_21", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_21 = new ControlDefine( "SHUBETU_21", "情報21", "SHUBETU_21", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_21 = new ControlDefine( "MEASURE_21", "情報21", "MEASURE_21", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_21 = new ControlDefine( "ZENKETA_21", "情報21", "ZENKETA_21", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_21 = new ControlDefine( "SYOSU_21", "情報21", "SYOSU_21", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_21 = new ControlDefine( "HISSU_21", "情報21", "HISSU_21", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_21 = new ControlDefine( "KAGEN_21", "情報21", "KAGEN_21", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_21 = new ControlDefine( "JYOGEN_21", "情報21", "JYOGEN_21", ControlDefine.BindType.Both, typeof( string ) );

			/// <summary>
			/// 情報22
			/// </summary>
			public static readonly ControlDefine KOMOKU_22 = new ControlDefine( "KOMOKU_22", "情報22", "KOMOKU_22", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_22 = new ControlDefine( "YUKO_22", "情報22", "YUKO_22", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_22 = new ControlDefine( "SHUBETU_22", "情報22", "SHUBETU_22", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_22 = new ControlDefine( "MEASURE_22", "情報22", "MEASURE_22", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_22 = new ControlDefine( "ZENKETA_22", "情報22", "ZENKETA_22", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_22 = new ControlDefine( "SYOSU_22", "情報22", "SYOSU_22", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_22 = new ControlDefine( "HISSU_22", "情報22", "HISSU_22", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_22 = new ControlDefine( "KAGEN_22", "情報22", "KAGEN_22", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_22 = new ControlDefine( "JYOGEN_22", "情報22", "JYOGEN_22", ControlDefine.BindType.Both, typeof( string ) );

			/// <summary>
			/// 情報23
			/// </summary>
			public static readonly ControlDefine KOMOKU_23 = new ControlDefine( "KOMOKU_23", "情報23", "KOMOKU_23", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_23 = new ControlDefine( "YUKO_23", "情報23", "YUKO_23", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_23 = new ControlDefine( "SHUBETU_23", "情報23", "SHUBETU_23", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_23 = new ControlDefine( "MEASURE_23", "情報23", "MEASURE_23", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_23 = new ControlDefine( "ZENKETA_23", "情報23", "ZENKETA_23", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_23 = new ControlDefine( "SYOSU_23", "情報23", "SYOSU_23", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_23 = new ControlDefine( "HISSU_23", "情報23", "HISSU_23", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_23 = new ControlDefine( "KAGEN_23", "情報23", "KAGEN_23", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_23 = new ControlDefine( "JYOGEN_23", "情報23", "JYOGEN_23", ControlDefine.BindType.Both, typeof( string ) );

			/// <summary>
			/// 情報24
			/// </summary>
			public static readonly ControlDefine KOMOKU_24 = new ControlDefine( "KOMOKU_24", "情報24", "KOMOKU_24", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_24 = new ControlDefine( "YUKO_24", "情報24", "YUKO_24", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_24 = new ControlDefine( "SHUBETU_24", "情報24", "SHUBETU_24", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_24 = new ControlDefine( "MEASURE_24", "情報24", "MEASURE_24", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_24 = new ControlDefine( "ZENKETA_24", "情報24", "ZENKETA_24", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_24 = new ControlDefine( "SYOSU_24", "情報24", "SYOSU_24", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_24 = new ControlDefine( "HISSU_24", "情報24", "HISSU_24", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_24 = new ControlDefine( "KAGEN_24", "情報24", "KAGEN_24", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_24 = new ControlDefine( "JYOGEN_24", "情報24", "JYOGEN_24", ControlDefine.BindType.Both, typeof( string ) );

			/// <summary>
			/// 情報25
			/// </summary>
			public static readonly ControlDefine KOMOKU_25 = new ControlDefine( "KOMOKU_25", "情報25", "KOMOKU_25", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_25 = new ControlDefine( "YUKO_25", "情報25", "YUKO_25", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_25 = new ControlDefine( "SHUBETU_25", "情報25", "SHUBETU_25", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_25 = new ControlDefine( "MEASURE_25", "情報25", "MEASURE_25", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_25 = new ControlDefine( "ZENKETA_25", "情報25", "ZENKETA_25", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_25 = new ControlDefine( "SYOSU_25", "情報25", "SYOSU_25", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_25 = new ControlDefine( "HISSU_25", "情報25", "HISSU_25", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_25 = new ControlDefine( "KAGEN_25", "情報25", "KAGEN_25", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_25 = new ControlDefine( "JYOGEN_25", "情報25", "JYOGEN_25", ControlDefine.BindType.Both, typeof( string ) );

			/// <summary>
			/// 情報26
			/// </summary>
			public static readonly ControlDefine KOMOKU_26 = new ControlDefine( "KOMOKU_26", "情報26", "KOMOKU_26", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_26 = new ControlDefine( "YUKO_26", "情報26", "YUKO_26", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_26 = new ControlDefine( "SHUBETU_26", "情報26", "SHUBETU_26", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_26 = new ControlDefine( "MEASURE_26", "情報26", "MEASURE_26", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_26 = new ControlDefine( "ZENKETA_26", "情報26", "ZENKETA_26", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_26 = new ControlDefine( "SYOSU_26", "情報26", "SYOSU_26", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_26 = new ControlDefine( "HISSU_26", "情報26", "HISSU_26", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_26 = new ControlDefine( "KAGEN_26", "情報26", "KAGEN_26", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_26 = new ControlDefine( "JYOGEN_26", "情報26", "JYOGEN_26", ControlDefine.BindType.Both, typeof( string ) );

			/// <summary>
			/// 情報27
			/// </summary>
			public static readonly ControlDefine KOMOKU_27 = new ControlDefine( "KOMOKU_27", "情報27", "KOMOKU_27", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_27 = new ControlDefine( "YUKO_27", "情報27", "YUKO_27", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_27 = new ControlDefine( "SHUBETU_27", "情報27", "SHUBETU_27", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_27 = new ControlDefine( "MEASURE_27", "情報27", "MEASURE_27", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_27 = new ControlDefine( "ZENKETA_27", "情報27", "ZENKETA_27", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_27 = new ControlDefine( "SYOSU_27", "情報27", "SYOSU_27", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_27 = new ControlDefine( "HISSU_27", "情報27", "HISSU_27", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_27 = new ControlDefine( "KAGEN_27", "情報27", "KAGEN_27", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_27 = new ControlDefine( "JYOGEN_27", "情報27", "JYOGEN_27", ControlDefine.BindType.Both, typeof( string ) );

			/// <summary>
			/// 情報28
			/// </summary>
			public static readonly ControlDefine KOMOKU_28 = new ControlDefine( "KOMOKU_28", "情報28", "KOMOKU_28", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_28 = new ControlDefine( "YUKO_28", "情報28", "YUKO_28", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_28 = new ControlDefine( "SHUBETU_28", "情報28", "SHUBETU_28", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_28 = new ControlDefine( "MEASURE_28", "情報28", "MEASURE_28", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_28 = new ControlDefine( "ZENKETA_28", "情報28", "ZENKETA_28", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_28 = new ControlDefine( "SYOSU_28", "情報28", "SYOSU_28", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_28 = new ControlDefine( "HISSU_28", "情報28", "HISSU_28", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_28 = new ControlDefine( "KAGEN_28", "情報28", "KAGEN_28", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_28 = new ControlDefine( "JYOGEN_28", "情報28", "JYOGEN_28", ControlDefine.BindType.Both, typeof( string ) );

			/// <summary>
			/// 情報29
			/// </summary>
			public static readonly ControlDefine KOMOKU_29 = new ControlDefine( "KOMOKU_29", "情報29", "KOMOKU_29", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine YUKO_29 = new ControlDefine( "YUKO_29", "情報29", "YUKO_29", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine SHUBETU_29 = new ControlDefine( "SHUBETU_29", "情報29", "SHUBETU_29", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine MEASURE_29 = new ControlDefine( "MEASURE_29", "情報29", "MEASURE_29", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine ZENKETA_29 = new ControlDefine( "ZENKETA_29", "情報29", "ZENKETA_29", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine SYOSU_29 = new ControlDefine( "SYOSU_29", "情報29", "SYOSU_29", ControlDefine.BindType.Both, typeof( int ) );
			public static readonly ControlDefine HISSU_29 = new ControlDefine( "HISSU_29", "情報29", "HISSU_29", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine KAGEN_29 = new ControlDefine( "KAGEN_29", "情報29", "KAGEN_29", ControlDefine.BindType.Both, typeof( string ) );
			public static readonly ControlDefine JYOGEN_29 = new ControlDefine( "JYOGEN_29", "情報29", "JYOGEN_29", ControlDefine.BindType.Both, typeof( string ) );
		}

		#endregion

		#region プロパティ

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
		/// マスターページ ボディ部プレースホルダー
		/// </summary>
		Control _masterBodyPlaceHolder = null;
		/// <summary>
		/// マスターページ ボディ部プレースホルダーアクセサー
		/// </summary>
		private Control MasterBodyPlaceHolder {
			get {
				if ( true == ObjectUtils.IsNull( _masterBodyPlaceHolder ) ) {
					_masterBodyPlaceHolder = (ContentPlaceHolder)Master.FindControl( PageControlID.Master.PLACE_HOLDER_MASTER_BODY );
				}

				return _masterBodyPlaceHolder;
			}
		}

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

		#region ページイベント

		protected void Page_Load( object sender, EventArgs e ) {
			base.RaiseEvent( DoPageLoad, false );
		}

		/// <summary>
		/// ページロード処理
		/// </summary>
		protected override void DoPageLoad( ) {

			//初期設定

			//閉じる
			ktbtnClose.Attributes[ControlUtils.ON_CLICK] = CLOSE_MODAL_DISP;

			//実行モード
			this._execKbn = NumericUtils.ToInt( Request.QueryString["exeKbn"] );
			if ( IsPostBack == false ) {
				InitializeSet();
			} else {
				ScriptManager.RegisterClientScriptBlock( this, this.GetType(), "AnlGroupInput_setDisplay", "$(function(){AnlGroupInput.setDisplay()});", true );
			}
		}

		#endregion

		#region 初期処理

		/// <summary>
		/// 初期化処理
		/// </summary>
		public void InitializeSet( ) {
			//アクセスカウンター登録
			Dao.Com.AccessCounterDao.Entry( base.CurrentPageInfo.pageId );

			//辞書変数の初期化
			dicSelectedRow = new Dictionary<string, string>();

			//初期値に設定する
			_orgData.Clear();
			_orgData.Add( "txtLineNm", Request.QueryString["txtLineNm"] );
			_orgData.Add( "txtStationNm", Request.QueryString["txtStationNm"] );
			_orgData.Add( "txtSetubi", Request.QueryString["txtSetubi"] );
			_orgData.Add( "txtGroupCd", Request.QueryString["txtGroupCd"] );
			_orgData.Add( "txtGroupNm", Request.QueryString["txtGroupNm"] );
			_orgData.Add( "txtLineCd", Request.QueryString["txtLineCd"] );
			_orgData.Add( "txtStationCd", Request.QueryString["txtStationCd"] );

			////画面の初期化
			txtLineNm.Text = _orgData["txtLineNm"].ToString();
			txtStationNm.Text = _orgData["txtStationNm"].ToString();
			txtSetubi.Text = _orgData["txtSetubi"].ToString();
			var result = new AiImageCheckBusiness.ImgCheckResultSet();
			DataTable tblResult = null;
			try {
				//throw new Exception();
				// 検索処理を実行する
				result = AiImageCheckBusiness.SelectGroupInputList( _orgData, GridviewDefault );

			} catch ( DataAccessException ex ) {

				if ( ex.OracleErrorNumber == DataAccessBase.ORAERROR_TIMEOUT ) {
					// クエリ発行タイムアウトが発生した場合、エラーメッセージを設定する
					base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_61910 );

				} else {

					// タイムアウト以外のExceptionが発生した場合、エラーメッセージを設定する
					logger.Exception( ex );
					base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80011, "検索処理で" );
				}
			} catch ( Exception ex ) {

				// Exceptionが発生した場合、エラーメッセージを設定する
				logger.Exception( ex );
				base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_84010 );

			} finally {
			}

			//---検索結果判定---
			// 処理結果が取得できた時のみ件数・ページャーの設定を実施
			ConditionInfoSessionHandler.ST_CONDITION cond = new ConditionInfoSessionHandler.ST_CONDITION();
			tblResult = result.ListTable;
			_resultData.Clear();
			if ( true == ObjectUtils.IsNotNull( tblResult ) ) {

				//表示されている列名を動的に収集する
				if ( 0 < tblResult.Rows.Count ) {
					for ( int idx = 0, idy = 1; idx < tblResult.Columns.Count; idx++ ) {
						if ( true == tblResult.Columns[idx].ColumnName.Contains( "YUKO" ) ) {
							if ( true != string.IsNullOrEmpty( tblResult.Rows[0].ItemArray[idx].ToString() ) &&
								true == tblResult.Rows[0].ItemArray[idx].ToString().Equals( "有効" ) ) {
								_resultData.Add( "lJyoho" + idy.ToString(), tblResult.Rows[0].ItemArray[idx - 1].ToString() );
								_resultData.Add( "txtJyoho" + idy.ToString(), tblResult.Rows[0].ItemArray[idx + 8].ToString() + "|" +
									tblResult.Rows[0].ItemArray[idx + 1].ToString() + "|" +
									tblResult.Rows[0].ItemArray[idx + 2].ToString() + "|" +
									tblResult.Rows[0].ItemArray[idx + 3].ToString() + "|" +
									tblResult.Rows[0].ItemArray[idx + 4].ToString() + "|" +
									tblResult.Rows[0].ItemArray[idx + 5].ToString() + "|" +
									tblResult.Rows[0].ItemArray[idx + 6].ToString() + "|" +
									tblResult.Rows[0].ItemArray[idx + 7].ToString() + "|" + "txtJyoho" + ( (int)( idx / 10 ) + 1 ).ToString() );
								hiddenNum.Text = idy.ToString();
								idy++;
							}
						}
					}
				}
			}

			int idt = 0;
			DataTable dt = new DataTable( "testtable" );
			foreach ( KeyValuePair<string, string> item1 in _resultData ) {
				dt.Columns.Add( item1.Key, typeof( String ) );
			}
			DataRow dr = dt.NewRow();
			foreach ( KeyValuePair<string, string> item in _resultData ) {
				if ( true == item.Key.ToString().Contains( "lJyoho" ) ) {
					dr[item.Key.ToString()] = item.Value + ":";
				} else {
					if ( this._execKbn == (int)ExecKbn.INSERT ) {
						//新規
						dr[item.Key.ToString()] = "";
					} else {
						dr[item.Key.ToString()] = item.Value.Split( '|' )[0].ToString();
					}
				}

				idt++;
			}
			Dictionary<string, object> dicControlValues = new Dictionary<string, object>();
			CurrentForm.SetControlValues( ConditionControls, dr, ref dicControlValues );
			ScriptManager.RegisterClientScriptBlock( this, this.GetType(), "AnlGroupInput_setDisplay", "$(function(){AnlGroupInput.setDisplay()});", true );

			//制約
			for ( int idx = 1; idx < _resultData.Count + 1; idx++ ) {
				string sValue = "";
				string sIdx = "txtJyoho" + idx.ToString();
				int conInputNum = 0;
				if ( true == _resultData.TryGetValue( sIdx, out sValue ) ) {
					Control ctrl = MasterBodyPlaceHolder.FindControl( sIdx );
					Type tp = ctrl.GetType();
					if ( tp == typeof( KTTextBox ) ) {
						( (KTTextBox)ctrl ).MaxLength = INPUT_NUM;
						if ( true == int.TryParse( sValue.Split( '|' )[3].Trim(), out conInputNum ) ) {
							if ( conInputNum < INPUT_NUM ) {
								( (KTTextBox)ctrl ).MaxLength = conInputNum;
							}
						}
						string syubetu = sValue.Split( '|' )[1].Trim();
						if ( true == syubetu.Equals( "数値" ) ) {
							( (KTTextBox)ctrl ).InputMode = (KTTextBox.InputModeType)Enum.Parse( typeof( KTTextBox.InputModeType ), "IntNum" );
						} else if ( true == syubetu.Equals( "英数字" ) ) {
							( (KTTextBox)ctrl ).InputMode = (KTTextBox.InputModeType)Enum.Parse( typeof( KTTextBox.InputModeType ), "AlphaNum" );
						}
					}
				}
			}

			if ( this._execKbn == (int)ExecKbn.UPDATE ) {
				//更新
				txtGroupCd.Text = _orgData["txtGroupCd"].ToString();
				txtGroupNm.Value = _orgData["txtGroupNm"].ToString();
			} else if ( this._execKbn == (int)ExecKbn.INSERT ) {
				btnUpdate.Text = "登録";
			} else {
				//コピーして追加
				btnUpdate.Text = "登録";
				txtGroupNm.Value = _orgData["txtGroupNm"].ToString();
			}
		}
		#endregion

		#region ボタンイベント

		/// <summary>
		/// 更新ボタン押下時イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnUpdate_Click( object sender, EventArgs e ) {
			base.RaiseEvent( DoUpdate );
		}

		/// <summary>
		/// 閉じるボタン押下時イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnViewRefresh_Click( object sender, EventArgs e ) {
			//base.RaiseEvent( DoSearch );
		}

		/// <summary>
		/// 更新処理
		/// </summary>
		private void DoUpdate( ) {
			//パラメータの設定
			KTBindParameters bindParam = new KTBindParameters();

			//実行前チェック
			if ( false == CheckInputData( ref bindParam ) ) {
				return;
			}

			//詳細情報データ処理
			for ( int idx = 29; 0 < idx; idx-- ) {
				string sValue = "";
				if ( true == _resultData.TryGetValue( "txtJyoho" + idx.ToString(), out sValue ) ) {
					string newPosition = sValue.Split( '|' )[8].ToString();
					string showPosition = "txtJyoho" + idx.ToString();
					if ( false == newPosition.Equals( showPosition ) ) {
						bindParam[newPosition].Value = bindParam[showPosition].Value;
						bindParam[showPosition].Value = "";
					}
				}
			}
			bindParam.Add( "updateBy", LoginInfo.UserInfo.userId );                          //更新者
			bindParam.Add( "updatePg", PageInfo.AnlGroupInput.pageId );                      //更新Pg
			bindParam.Add( "txtGroupNm", txtGroupNm.Value );
			bindParam.Add( "txtLineCd", _orgData["txtLineCd"].ToString() );
			bindParam.Add( "txtStationCd", _orgData["txtStationCd"].ToString() );
			bindParam.Add( "txtSetubi", _orgData["txtSetubi"].ToString() );

			//データベース処理
			int retCnt = 0;
			if ( this._execKbn == (int)ExecKbn.UPDATE ) {
				bindParam.Add( "txtGroupCd", _orgData["txtGroupCd"].ToString() );

				//更新
				retCnt = AiImageCheckDao.UpdateGroupInput( bindParam );
				if ( retCnt.Equals( 1 ) ) {
					//正常
					CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_INF_10010 );
				} else if ( retCnt.Equals( 0 ) ) {
					//データなし
					CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_72040 );
				} else {
					CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
				}
			} else {
				//登録
				txtGroupCd.Text = AiImageCheckDao.SelectNextGroupCode();
				bindParam.Add( "txtGroupCd", txtGroupCd.Text );
				retCnt = AiImageCheckDao.InsertGroupInput( bindParam );
				if ( retCnt.Equals( 1 ) ) {
					//正常
					CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_INF_10010 );
				} else if ( retCnt.Equals( 0 ) ) {
					//データなし
					CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_72040 );
				} else {
					CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
				}

			}
			btnUpdate.Enabled = false;
		}

		#endregion

		#region 業務処理

		/// <summary>
		/// 処理実行前データチェック
		/// </summary>
		/// <returns></returns>
		private bool CheckInputData( ref KTBindParameters bindParam ) {

			//動的データ処理
			bindParam.Add( "txtJyoho1", txtJyoho1.Value );
			bindParam.Add( "txtJyoho2", txtJyoho2.Value );
			bindParam.Add( "txtJyoho3", txtJyoho3.Value );
			bindParam.Add( "txtJyoho4", txtJyoho4.Value );
			bindParam.Add( "txtJyoho5", txtJyoho5.Value );
			bindParam.Add( "txtJyoho6", txtJyoho6.Value );
			bindParam.Add( "txtJyoho7", txtJyoho7.Value );
			bindParam.Add( "txtJyoho8", txtJyoho8.Value );
			bindParam.Add( "txtJyoho9", txtJyoho9.Value );
			bindParam.Add( "txtJyoho10", txtJyoho10.Value );
			bindParam.Add( "txtJyoho11", txtJyoho11.Value );
			bindParam.Add( "txtJyoho12", txtJyoho12.Value );
			bindParam.Add( "txtJyoho13", txtJyoho13.Value );
			bindParam.Add( "txtJyoho14", txtJyoho14.Value );
			bindParam.Add( "txtJyoho15", txtJyoho15.Value );
			bindParam.Add( "txtJyoho16", txtJyoho16.Value );
			bindParam.Add( "txtJyoho17", txtJyoho17.Value );
			bindParam.Add( "txtJyoho18", txtJyoho18.Value );
			bindParam.Add( "txtJyoho19", txtJyoho19.Value );
			bindParam.Add( "txtJyoho20", txtJyoho20.Value );
			bindParam.Add( "txtJyoho21", txtJyoho21.Value );
			bindParam.Add( "txtJyoho22", txtJyoho22.Value );
			bindParam.Add( "txtJyoho23", txtJyoho23.Value );
			bindParam.Add( "txtJyoho24", txtJyoho24.Value );
			bindParam.Add( "txtJyoho25", txtJyoho25.Value );
			bindParam.Add( "txtJyoho26", txtJyoho26.Value );
			bindParam.Add( "txtJyoho27", txtJyoho27.Value );
			bindParam.Add( "txtJyoho28", txtJyoho28.Value );
			bindParam.Add( "txtJyoho29", txtJyoho29.Value );

			//入力チェック
			if ( true == StringUtils.IsBlank( txtGroupNm.Value ) || true == StringUtils.IsEmpty( txtGroupNm.Value ) || ( 40 < txtGroupNm.Value.Length ) ) {
				CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62030, "画像解析グループ名" );
				SetFocus( txtGroupNm );
				return false;
			}
			for ( int idx = 1; idx < _resultData.Count + 1; idx++ ) {
				string sValue = "";
				if ( true == _resultData.TryGetValue( "txtJyoho" + idx.ToString(), out sValue ) ) {
					int iMaxLength = 0;
					string sCheckValue = bindParam["txtJyoho" + idx.ToString()].Value.ToString().Trim();
					string flagItemCheck = sValue.Split( '|' )[5].ToString().Trim();
					int iByteNum = System.Text.Encoding.Default.GetByteCount( sCheckValue );

					if ( 0 == iByteNum ) {
						// 入力必要チェック
						if ( true == flagItemCheck.Equals( "必須" ) ) {
							CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62030, _resultData["lJyoho" + idx.ToString()].ToString() );
							string colId = "txtJyoho" + idx.ToString();
							ScriptManager.RegisterClientScriptBlock( this, this.GetType(), "return AnlGroupInput_setFocus", "$(function(){AnlGroupInput.setFocus('" + colId + "')});", true );
							return false;
						} 

					} else if ( 0 < iByteNum ) {
						// データバイト数チェック
						if ( INPUT_NUM < iByteNum ) {
							CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62030, _resultData["lJyoho" + idx.ToString()].ToString() );
							string colId = "txtJyoho" + idx.ToString();
							ScriptManager.RegisterClientScriptBlock( this, this.GetType(), "return AnlGroupInput_setFocus", "$(function(){AnlGroupInput.setFocus('" + colId + "')});", true );
							return false;
						} else {
							if ( true == int.TryParse( sValue.Split( '|' )[3].ToString(), out iMaxLength ) ) {
								if ( iMaxLength < iByteNum ) {
									CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62030, _resultData["lJyoho" + idx.ToString()].ToString() );
									string colId = "txtJyoho" + idx.ToString();
									ScriptManager.RegisterClientScriptBlock( this, this.GetType(), "return AnlGroupInput_setFocus", "$(function(){AnlGroupInput.setFocus('" + colId + "')});", true );
									return false;
								}
							}
						}

						// データ型チェック
						flagItemCheck = sValue.Split( '|' )[1].ToString().Trim();
						if ( true == flagItemCheck.Equals( "数値" ) ) {
							int iValue = 0;
							int iMinValue = 0;
							int iMaxValue = 0;
							if ( false == int.TryParse( sCheckValue, out iValue ) ) {
								CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62030, _resultData["lJyoho" + idx.ToString()].ToString() );
								string colId = "txtJyoho" + idx.ToString();
								ScriptManager.RegisterClientScriptBlock( this, this.GetType(), "return AnlGroupInput_setFocus", "$(function(){AnlGroupInput.setFocus('" + colId + "')});", true );
								return false;
							}
							if ( true == int.TryParse( sValue.Split( '|' )[6].ToString(), out iMinValue ) &&
								   true == int.TryParse( sValue.Split( '|' )[7].ToString(), out iMaxValue ) ) {
								if ( false == ( ( iMinValue <= iValue ) && ( iValue <= iMaxValue ) ) ) {
									CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62050, _resultData["lJyoho" + idx.ToString()].ToString() );
									string colId = "txtJyoho" + idx.ToString();
									ScriptManager.RegisterClientScriptBlock( this, this.GetType(), "return AnlGroupInput_setFocus", "$(function(){AnlGroupInput.setFocus('" + colId + "')});", true );
									return false;
								}
							}
						}
					}
				}
			}
			return true;
		}

		#endregion
	}
}