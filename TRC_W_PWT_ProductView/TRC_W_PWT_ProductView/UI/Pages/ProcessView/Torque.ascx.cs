using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using KTFramework.Common;
using KTFramework.Dao;
using KTWebControl.CustomControls;
using KTWebInheritance.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.UI.Base;
using TRC_W_PWT_ProductView.Session;

namespace TRC_W_PWT_ProductView.UI.Pages.ProcessView {
    /// <summary>
    /// (詳細 エンジン 工程) トルク締付
    /// </summary>
    public partial class Torque : System.Web.UI.UserControl, Defines.Interface.IDetail {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義

        /// <summary>
        /// 管理ID
        /// </summary>
        const string MANAGE_ID = Defines.Session.DetailFrame.SESSION_PAGE_INFO_DETAIL_KEY;//CurrentUCInfo.pageId

        /// <summary>
        /// DB取得データ格納先 ディクショナリキー
        /// </summary>
        private string SESSION_PAGE_INFO_DB_KEY = "bindTableData";

        /// <summary>
        /// 選択行制御
        /// </summary>
        const string LIST_VIEW_SELECTED = "Torque.SelectListViewRow(this,{0},'{1}');";

        const string GRID_MAIN_VIEW_GROUP_CD = "MainView";

        #region コントロール定義
        /// <summary>
        /// (メインリスト)コントロール定義
        /// </summary>
        public class GRID_MAIN {
            /// <summary>TR</summary>
            public static readonly ControlDefine TR_ROW_DATA = new ControlDefine( "trRowData", "TR", "", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>(SELECTコマンド送信用ボタン)</summary>
            public static readonly ControlDefine SELECT = new ControlDefine( "btnSelect", "BUTTON", "", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>部品名</summary>
            public static readonly ControlDefine PARTS_NM = new ControlDefine( "txtPartsNm", "部品名", "partsNm", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>計測日時</summary>
            public static readonly ControlDefine INSPECTION_DT = new ControlDefine( "txtInspectionDt", "計測日時", "inspectionDt", ControlDefine.BindType.None, typeof( DateTime ) );
            /// <summary>端末名</summary>
            public static readonly ControlDefine TERMINAL_NM = new ControlDefine( "txtTerminalNm", "端末名", "terminalNm", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>計測通番</summary>
            public static readonly ControlDefine HISTORY_INDEX = new ControlDefine( "ntbHistoryIndex", "最終来歴", "historyIndex", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>結果</summary>
            public static readonly ControlDefine RESULT = new ControlDefine( "txtResult", "結果", "result", ControlDefine.BindType.Down, typeof( String ) );
        }

        /// <summary>
        /// (サブリスト)コントロール定義
        /// </summary>
        public class GRID_SUB {
            /// <summary>部品名</summary>
            public static readonly ControlDefine PARTS_NM = new ControlDefine( "txtPartsNm", "部品名", "partsNm", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>計測日時</summary>
            public static readonly ControlDefine INSPECTION_DT = new ControlDefine( "txtInspectionDt", "計測日時", "inspectionDt", ControlDefine.BindType.None, typeof( DateTime ) );
            /// <summary>端末名</summary>
            public static readonly ControlDefine TERMINAL_NM = new ControlDefine( "txtTerminalNm", "端末名", "terminalNm", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>計測通番</summary>
            public static readonly ControlDefine HISTORY_INDEX = new ControlDefine( "ntbHistoryIndex", "計測通番", "historyIndex", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>[トルク]上限値</summary>
            public static readonly ControlDefine UPPER_LIMIT = new ControlDefine( "ntbUpperLimit", "[トルク]上限値", "upperLimit", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>[トルク]下限値</summary>
            public static readonly ControlDefine LOWER_LIMIT = new ControlDefine( "ntbLowerLimit", "[トルク]下限値", "lowerLimit", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>[トルク]計測値１</summary>
            public static readonly ControlDefine MEASURE_VAL_1 = new ControlDefine( "ntbMeasureVal1", "[トルク]計測値１", "measureVal1", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>[トルク]計測値２</summary>
            public static readonly ControlDefine MEASURE_VAL_2 = new ControlDefine( "ntbMeasureVal2", "[トルク]計測値２", "measureVal2", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>[トルク]計測値３</summary>
            public static readonly ControlDefine MEASURE_VAL_3 = new ControlDefine( "ntbMeasureVal3", "[トルク]計測値３", "measureVal3", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>[トルク]計測値４</summary>
            public static readonly ControlDefine MEASURE_VAL_4 = new ControlDefine( "ntbMeasureVal4", "[トルク]計測値４", "measureVal4", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>[トルク]計測値５</summary>
            public static readonly ControlDefine MEASURE_VAL_5 = new ControlDefine( "ntbMeasureVal5", "[トルク]計測値５", "measureVal5", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>[トルク]計測値６</summary>
            public static readonly ControlDefine MEASURE_VAL_6 = new ControlDefine( "ntbMeasureVal6", "[トルク]計測値６", "measureVal6", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>[トルク]計測値７</summary>
            public static readonly ControlDefine MEASURE_VAL_7 = new ControlDefine( "ntbMeasureVal7", "[トルク]計測値７", "measureVal7", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>[トルク]計測値８</summary>
            public static readonly ControlDefine MEASURE_VAL_8 = new ControlDefine( "ntbMeasureVal8", "[トルク]計測値８", "measureVal8", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>[トルク]計測値９</summary>
            public static readonly ControlDefine MEASURE_VAL_9 = new ControlDefine( "ntbMeasureVal9", "[トルク]計測値９", "measureVal9", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>[トルク]計測値１０</summary>
            public static readonly ControlDefine MEASURE_VAL_10 = new ControlDefine( "ntbMeasureVal10", "[トルク]計測値１０", "measureVal10", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>[角度]上限値</summary>
            public static readonly ControlDefine ANGLE_UPPER_LIMIT = new ControlDefine( "ntbAngleUpperLimit", "[角度]上限値", "angleUpperLimit", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>[角度]下限値</summary>
            public static readonly ControlDefine ANGLE_LOWER_LIMIT = new ControlDefine( "ntbAngleLowerLimit", "[角度]下限値", "angleLowerLimit", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>[角度]計測値１</summary>
            public static readonly ControlDefine ANGLE_MEASURE_VAL_1 = new ControlDefine( "ntbAngleMeasureVal1", "[角度]計測値１", "angleMeasureVal1", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>[角度]計測値２</summary>
            public static readonly ControlDefine ANGLE_MEASURE_VAL_2 = new ControlDefine( "ntbAngleMeasureVal2", "[角度]計測値２", "angleMeasureVal2", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>[角度]計測値３</summary>
            public static readonly ControlDefine ANGLE_MEASURE_VAL_3 = new ControlDefine( "ntbAngleMeasureVal3", "[角度]計測値３", "angleMeasureVal3", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>[角度]計測値４</summary>
            public static readonly ControlDefine ANGLE_MEASURE_VAL_4 = new ControlDefine( "ntbAngleMeasureVal4", "[角度]計測値４", "angleMeasureVal4", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>[角度]計測値５</summary>
            public static readonly ControlDefine ANGLE_MEASURE_VAL_5 = new ControlDefine( "ntbAngleMeasureVal5", "[角度]計測値５", "angleMeasureVal5", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>[角度]計測値６</summary>
            public static readonly ControlDefine ANGLE_MEASURE_VAL_6 = new ControlDefine( "ntbAngleMeasureVal6", "[角度]計測値６", "angleMeasureVal6", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>[角度]計測値７</summary>
            public static readonly ControlDefine ANGLE_MEASURE_VAL_7 = new ControlDefine( "ntbAngleMeasureVal7", "[角度]計測値７", "angleMeasureVal7", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>[角度]計測値８</summary>
            public static readonly ControlDefine ANGLE_MEASURE_VAL_8 = new ControlDefine( "ntbAngleMeasureVal8", "[角度]計測値８", "angleMeasureVal8", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>[角度]計測値９</summary>
            public static readonly ControlDefine ANGLE_MEASURE_VAL_9 = new ControlDefine( "ntbAngleMeasureVal9", "[角度]計測値９", "angleMeasureVal9", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>[角度]計測値１０</summary>
            public static readonly ControlDefine ANGLE_MEASURE_VAL_10 = new ControlDefine( "ntbAngleMeasureVal10", "[角度]計測値１０", "angleMeasureVal10", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>2度締め確認[トルク]上限値</summary>
            public static readonly ControlDefine TWICE_UPPER_LIMIT = new ControlDefine( "ntbTwiceUpperLimit", "2度締め確認[トルク]上限値", "twiceUpperLimit", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>2度締め確認[トルク]下限値</summary>
            public static readonly ControlDefine TWICE_LOWER_LIMIT = new ControlDefine( "ntbTwiceLowerLimit", "2度締め確認[トルク]下限値", "twiceLowerLimit", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>2度締め確認[トルク]計測値１</summary>
            public static readonly ControlDefine TWICE_MEASURE_VAL_1 = new ControlDefine( "ntbTwiceMeasureVal1", "2度締め確認[トルク]計測値１", "twiceMeasureVal1", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>2度締め確認[トルク]計測値２</summary>
            public static readonly ControlDefine TWICE_MEASURE_VAL_2 = new ControlDefine( "ntbTwiceMeasureVal2", "2度締め確認[トルク]計測値２", "twiceMeasureVal2", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>2度締め確認[トルク]計測値３</summary>
            public static readonly ControlDefine TWICE_MEASURE_VAL_3 = new ControlDefine( "ntbTwiceMeasureVal3", "2度締め確認[トルク]計測値３", "twiceMeasureVal3", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>2度締め確認[トルク]計測値４</summary>
            public static readonly ControlDefine TWICE_MEASURE_VAL_4 = new ControlDefine( "ntbTwiceMeasureVal4", "2度締め確認[トルク]計測値４", "twiceMeasureVal4", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>2度締め確認[トルク]計測値５</summary>
            public static readonly ControlDefine TWICE_MEASURE_VAL_5 = new ControlDefine( "ntbTwiceMeasureVal5", "2度締め確認[トルク]計測値５", "twiceMeasureVal5", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>2度締め確認[トルク]計測値６</summary>
            public static readonly ControlDefine TWICE_MEASURE_VAL_6 = new ControlDefine( "ntbTwiceMeasureVal6", "2度締め確認[トルク]計測値６", "twiceMeasureVal6", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>2度締め確認[トルク]計測値７</summary>
            public static readonly ControlDefine TWICE_MEASURE_VAL_7 = new ControlDefine( "ntbTwiceMeasureVal7", "2度締め確認[トルク]計測値７", "twiceMeasureVal7", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>2度締め確認[トルク]計測値８</summary>
            public static readonly ControlDefine TWICE_MEASURE_VAL_8 = new ControlDefine( "ntbTwiceMeasureVal8", "2度締め確認[トルク]計測値８", "twiceMeasureVal8", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>2度締め確認[トルク]計測値９</summary>
            public static readonly ControlDefine TWICE_MEASURE_VAL_9 = new ControlDefine( "ntbTwiceMeasureVal9", "2度締め確認[トルク]計測値９", "twiceMeasureVal9", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>2度締め確認[トルク]計測値１０</summary>
            public static readonly ControlDefine TWICE_MEASURE_VAL_10 = new ControlDefine( "ntbTwiceMeasureVal10", "2度締め確認[トルク]計測値１０", "twiceMeasureVal10", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>2度締め確認[角度]上限値</summary>
            public static readonly ControlDefine TWICE_ANGLE_UPPER_LIMIT = new ControlDefine( "ntbTwiceAngleUpperLimit", "2度締め確認[角度]上限値", "twiceAngleUpperLimit", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>2度締め確認[角度]下限値</summary>
            public static readonly ControlDefine TWICE_ANGLE_LOWER_LIMIT = new ControlDefine( "ntbTwiceAngleLowerLimit", "2度締め確認[角度]下限値", "twiceAngleLowerLimit", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>2度締め確認[角度]計測値１</summary>
            public static readonly ControlDefine TWICE_ANGLE_MEASURE_VAL_1 = new ControlDefine( "ntbTwiceAngleMeasureVal1", "2度締め確認[角度]計測値１", "twiceAngleMeasureVal1", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>2度締め確認[角度]計測値２</summary>
            public static readonly ControlDefine TWICE_ANGLE_MEASURE_VAL_2 = new ControlDefine( "ntbTwiceAngleMeasureVal2", "2度締め確認[角度]計測値２", "twiceAngleMeasureVal2", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>2度締め確認[角度]計測値３</summary>
            public static readonly ControlDefine TWICE_ANGLE_MEASURE_VAL_3 = new ControlDefine( "ntbTwiceAngleMeasureVal3", "2度締め確認[角度]計測値３", "twiceAngleMeasureVal3", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>2度締め確認[角度]計測値４</summary>
            public static readonly ControlDefine TWICE_ANGLE_MEASURE_VAL_4 = new ControlDefine( "ntbTwiceAngleMeasureVal4", "2度締め確認[角度]計測値４", "twiceAngleMeasureVal4", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>2度締め確認[角度]計測値５</summary>
            public static readonly ControlDefine TWICE_ANGLE_MEASURE_VAL_5 = new ControlDefine( "ntbTwiceAngleMeasureVal5", "2度締め確認[角度]計測値５", "twiceAngleMeasureVal5", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>2度締め確認[角度]計測値６</summary>
            public static readonly ControlDefine TWICE_ANGLE_MEASURE_VAL_6 = new ControlDefine( "ntbTwiceAngleMeasureVal6", "2度締め確認[角度]計測値６", "twiceAngleMeasureVal6", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>2度締め確認[角度]計測値７</summary>
            public static readonly ControlDefine TWICE_ANGLE_MEASURE_VAL_7 = new ControlDefine( "ntbTwiceAngleMeasureVal7", "2度締め確認[角度]計測値７", "twiceAngleMeasureVal7", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>2度締め確認[角度]計測値８</summary>
            public static readonly ControlDefine TWICE_ANGLE_MEASURE_VAL_8 = new ControlDefine( "ntbTwiceAngleMeasureVal8", "2度締め確認[角度]計測値８", "twiceAngleMeasureVal8", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>2度締め確認[角度]計測値９</summary>
            public static readonly ControlDefine TWICE_ANGLE_MEASURE_VAL_9 = new ControlDefine( "ntbTwiceAngleMeasureVal9", "2度締め確認[角度]計測値９", "twiceAngleMeasureVal9", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>2度締め確認[角度]計測値１０</summary>
            public static readonly ControlDefine TWICE_ANGLE_MEASURE_VAL_10 = new ControlDefine( "ntbTwiceAngleMeasureVal10", "2度締め確認[角度]計測値１０", "twiceAngleMeasureVal10", ControlDefine.BindType.Down, typeof( int ) );
        }
        #endregion 

        /// <summary>
        /// 一覧表示情報コントロール(左下)
        /// </summary>
        public class GRID_SEARCH_CONTROLS_L {
        }

        /// <summary>
        /// 一覧表示情報コントロール(右下)
        /// </summary>
        public class GRID_SEARCH_CONTROLS_R {
        }

        #region 使用しない
        /// <summary>
        /// SubGridView
        /// </summary>
        /// 
        internal class GRID_MAIN_COMMON {
            /// <summary>部品名</summary>
            public static readonly GridViewDefine PARTS_NM = new GridViewDefine( "部品名", "partsNm", typeof( string ), "", false, HorizontalAlign.Left, 200, true, true );
            /// <summary>登録日時</summary>
            public static readonly GridViewDefine INSPECTION_DT = new GridViewDefine( "登録日時", "inspectionDt", typeof( DateTime ), "{0:yyyy/MM/dd HH:mm:ss}", false, HorizontalAlign.Center, 160, true, true );
            /// <summary>端末名</summary>
            public static readonly GridViewDefine TERMINAL_NM = new GridViewDefine( "端末名", "terminalNm", typeof( string ), "", false, HorizontalAlign.Left, 120, true, true );
            /// <summary>計測通番</summary>
            public static readonly GridViewDefine HISTORY_INDEX = new GridViewDefine( "計測通番", "historyIndex", typeof( string ), "", false, HorizontalAlign.Right, 100, true, true );
            /// <summary>上限値</summary>
            public static readonly GridViewDefine UPPER_LIMIT = new GridViewDefine( "上限値", "upperLimit", typeof( string ), "{0:#,0.00}", false, HorizontalAlign.Right, 80, true, true );
            /// <summary>下限値</summary>
            public static readonly GridViewDefine LOWER_LIMIT = new GridViewDefine( "下限値", "lowerLimit", typeof( string ), "{0:#,0.00}", false, HorizontalAlign.Right, 80, true, true );
            /// <summary>計測値１</summary>
            public static readonly GridViewDefine MEASURE_VAL_1 = new GridViewDefine( "計測値1", "measureVal1", typeof( string ), "{0:#,0.00}", false, HorizontalAlign.Right, 80, true, true );
            /// <summary>計測値２</summary>
            public static readonly GridViewDefine MEASURE_VAL_2 = new GridViewDefine( "計測値2", "measureVal2", typeof( string ), "{0:#,0.00}", false, HorizontalAlign.Right, 80, true, true );
            /// <summary>計測値３</summary>
            public static readonly GridViewDefine MEASURE_VAL_3 = new GridViewDefine( "計測値3", "measureVal3", typeof( string ), "{0:#,0.00}", false, HorizontalAlign.Right, 80, true, true );
            /// <summary>計測値４</summary>
            public static readonly GridViewDefine MEASURE_VAL_4 = new GridViewDefine( "計測値4", "measureVal4", typeof( string ), "{0:#,0.00}", false, HorizontalAlign.Right, 80, true, true );
            /// <summary>計測値５</summary>
            public static readonly GridViewDefine MEASURE_VAL_5 = new GridViewDefine( "計測値5", "measureVal5", typeof( string ), "{0:#,0.00}", false, HorizontalAlign.Right, 80, true, true );
            /// <summary>計測値６</summary>
            public static readonly GridViewDefine MEASURE_VAL_6 = new GridViewDefine( "計測値6", "measureVal6", typeof( string ), "{0:#,0.00}", false, HorizontalAlign.Right, 80, true, true );
            /// <summary>計測値７</summary>
            public static readonly GridViewDefine MEASURE_VAL_7 = new GridViewDefine( "計測値7", "measureVal7", typeof( string ), "{0:#,0.00}", false, HorizontalAlign.Right, 80, true, true );
            /// <summary>計測値８</summary>
            public static readonly GridViewDefine MEASURE_VAL_8 = new GridViewDefine( "計測値8", "measureVal8", typeof( string ), "{0:#,0.00}", false, HorizontalAlign.Right, 80, true, true );
        }
        #endregion

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
        /// 表示中ユーザコントロール情報
        /// </summary>
        private PageInfo.ST_PAGE_INFO CurrentUCInfo {
            get {
                return PageInfo.GetUCPageInfo( DetailKeyParam.ProductKind, DetailKeyParam.GroupCd, DetailKeyParam.ClassCd );
            }
        }

        /// <summary>
        /// (メイン)コントロール定義
        /// </summary>
        ControlDefine[] _mainControls = null;
        /// <summary>
        /// (メイン)コントロール定義アクセサ
        /// </summary>
        ControlDefine[] MainControls {
            get {
                if ( true == ObjectUtils.IsNull( _mainControls ) ) {
                    _mainControls = ControlUtils.GetControlDefineArray( typeof( GRID_MAIN ) );
                }
                return _mainControls;
            }
        }

        /// <summary>
        /// (サブ)コントロール定義
        /// </summary>
        ControlDefine[] _subControls = null;
        /// <summary>
        /// (サブ)コントロール定義アクセサ
        /// </summary>
        ControlDefine[] SubControls {
            get {
                if ( true == ObjectUtils.IsNull( _subControls ) ) {
                    _subControls = ControlUtils.GetControlDefineArray( typeof( GRID_SUB ) );
                }
                return _subControls;
            }
        }

        /// <summary>
        /// 詳細表示キー情報
        /// </summary>
        private Defines.Interface.ST_DETAIL_PARAM _detailKeyParam;
        /// <summary>
        /// 詳細表示キー情報
        /// </summary>
        public Defines.Interface.ST_DETAIL_PARAM DetailKeyParam {
            get {
                return _detailKeyParam;
            }
            set {
                _detailKeyParam = value;
            }
        }
        #endregion

        #region イベント

        /// <summary>
        /// ページロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load( object sender, EventArgs e ) {
            CurrentForm.RaiseEvent( DoPageLoad );
        }

        /// <summary>
        /// メインリスト行バインド（左）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMainViewLB_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainViewLB( sender, e );
        }
        protected void lstMainListLB_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundMainLBList( sender, e );
        }

        /// <summary>
        /// メインリスト行バインド（右）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMainViewRB_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainViewRB( sender, e );
        }
        protected void lstMainListRB_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundMainRBList( sender, e );
        }

        /// <summary>
        /// メインリスト行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstSubList_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundSubList( sender, e );
        }

        /// <summary>
        /// メインリスト選択行変更中（左）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstMainListLB_SelectedIndexChanging( object sender, ListViewSelectEventArgs e ) {
            //処理なし
        }

                /// <summary>
        /// メインリスト選択行変更中（右）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstMainListRB_SelectedIndexChanging( object sender, ListViewSelectEventArgs e ) {
            //処理なし
        }

        /// <summary>
        /// メインリスト選択行変更（左）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstMainListLB_SelectedIndexChanged( object sender, EventArgs e ) {
            CurrentForm.RaiseEvent( SelectedIndexChangedMainLBList );
        }

        /// <summary>
        /// メインリスト選択行変更（右）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstMainListRB_SelectedIndexChanged( object sender, EventArgs e ) {
            CurrentForm.RaiseEvent( SelectedIndexChangedMainRBList );
        }

        #endregion

        #region イベントメソッド

        #region ページイベント
        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize() {
            //検索結果取得
            Business.DetailViewBusiness.ResultSetMulti resultSet = new Business.DetailViewBusiness.ResultSetMulti();
            try {
                resultSet = Business.DetailViewBusiness.SelectEngineTorqueDetails( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial );
            } catch ( DataAccessException ex ) {
                logger.Exception( ex );
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                return;
            } catch ( Exception ex ) {
                logger.Exception( ex );
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                return;
            } finally {
            }

            //取得データをセッションに格納
            Dictionary<string, object> dicPageControlInfo = new Dictionary<string, object>();
            dicPageControlInfo.Add( SESSION_PAGE_INFO_DB_KEY, resultSet );
            CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).SetPageControlInfo( MANAGE_ID, dicPageControlInfo );

            if ( 0 == resultSet.MainTable.Rows.Count ) {
                //検索結果0件
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62010, CurrentUCInfo.title );
                return;
            }

            InitializeValues( resultSet );
        }

        /// <summary>
        /// ページロード処理
        /// </summary>
        private void DoPageLoad() {
        }

        #endregion

        /// <summary>
        /// 初期値設定
        /// </summary>
        private void InitializeValues( Business.DetailViewBusiness.ResultSetMulti resultSet ) {
            /*
            //来歴情報セット
            lstMainList.DataSource = resultSet.MainTable;
            lstMainList.DataBind();

            lstMainList.SelectedIndex = 0;
             * */

            /*
            if ( 0 < resultSet.MainTable.Rows.Count ) {
                //一覧表示列の設定
                GridViewDefine[] gridColumns;
                List<GridViewDefine> columns = new List<GridViewDefine>();
                columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_MAIN_COMMON ) ) );
                gridColumns = columns.ToArray();

                ConditionInfoSessionHandler.ST_CONDITION cond_main = new ConditionInfoSessionHandler.ST_CONDITION();
                cond_main.ResultData = resultSet.MainTable;

                //グリッドビューバインド
                GridView frozenGrid = grvMainViewLB;
                if ( 0 < resultSet.MainTable.Rows.Count ) {

                    //新規バインド
                    ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, gridColumns, true ), cond_main, true );
                    ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, gridColumns, false ), cond_main, true );
                    ControlUtils.BindGridView_WithTempField( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, gridColumns, true ), cond_main.ResultData );
                    ControlUtils.BindGridView_WithTempField( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, gridColumns, false ), cond_main.ResultData );

                    //グリッドビュー外のDivサイズ変更
                    SetDivGridViewWidth();
                } else {
                    ClearGridView();
                }                    
            }
            */

            //来歴情報セット
            lstMainListRB.DataSource = resultSet.MainTable;
            lstMainListRB.DataBind();
            lstMainListRB.SelectedIndex = 0;

            //サブリストバインド
            SelectedIndexChangedMainList( lstMainListRB.SelectedIndex );

        }
        #endregion

        #region リストバインド
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

                Dictionary<string, object> dicControls = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Row, ControlUtils.GetControlDefineArray( typeof( GRID_SEARCH_CONTROLS_L ) ), row, ref dicControls );

            }
            ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_MAIN_VIEW_GROUP_CD );

        }
        private void ItemDataBoundMainLBList( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {
                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Item, MainControls, rowBind, ref dicSetValues );

                //クリックイベントセット
                HtmlTableRow trRow = (HtmlTableRow)e.Item.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
                Button selectBtn = (Button)e.Item.FindControl( GRID_MAIN.SELECT.controlId );
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format( LIST_VIEW_SELECTED, e.Item.DataItemIndex, selectBtn.UniqueID );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );

            }
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
                CurrentForm.SetControlValues( e.Row, ControlUtils.GetControlDefineArray( typeof( GRID_SEARCH_CONTROLS_R ) ), rowData, ref dicControls );
            }

            ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_MAIN_VIEW_GROUP_CD );

        }
        private void ItemDataBoundMainRBList( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {
                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Item, MainControls, rowBind, ref dicSetValues );

                //計測日時
                KTTextBox txtInspectionDt = ( (KTTextBox)e.Item.FindControl( GRID_MAIN.INSPECTION_DT.controlId ) );
                string inspectionDt = DateUtils.ToString( rowBind[GRID_MAIN.INSPECTION_DT.bindField], DateUtils.FormatType.Second );
                txtInspectionDt.Value = inspectionDt;

                //クリックイベントセット
                HtmlTableRow trRow = (HtmlTableRow)e.Item.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
                Button selectBtn = (Button)e.Item.FindControl( GRID_MAIN.SELECT.controlId );
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format( LIST_VIEW_SELECTED, e.Item.DataItemIndex, selectBtn.UniqueID );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );

            }
        }
        #endregion

        #region Grid設定
#if false
        /// <summary>
        /// グリッドビュー格納DIVサイズ調整
        /// </summary>
        private void SetDivGridViewWidth() {
            SetDivGridViewWidth( grvHeaderLT, divGrvHeaderLT );
            SetDivGridViewWidth( grvHeaderRT, divGrvHeaderRT );

            SetDivGridViewWidth( grvMainViewLB, divGrvLB );
            SetDivGridViewWidth( grvMainViewRB, divGrvRB );
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
        /// グリッドビュークリア
        /// </summary>
        private void ClearGridView() {
            //列名非表示 グリッドビュークリア
            ControlUtils.InitializeGridView( grvHeaderLT, false );
            ControlUtils.InitializeGridView( grvHeaderRT, false );
            ControlUtils.InitializeGridView( grvMainViewLB, false );
            ControlUtils.InitializeGridView( grvMainViewRB, false );

            //グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();

            //GridView非表示
            divMainListArea.Visible = false;
        }
#endif
        #endregion

        #region リストバインド(詳細)
        /// <summary>
        /// メインリストバインド処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void ItemDataBoundSubList( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {
                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                //コントロールへの自動データバインド
                CurrentForm.SetControlValues( e.Item, SubControls, rowBind, ref dicSetValues );

                //計測日時
                KTTextBox txtInspectionDt = ( (KTTextBox)e.Item.FindControl( GRID_MAIN.INSPECTION_DT.controlId ) );
                string inspectionDt = DateUtils.ToString( rowBind[GRID_MAIN.INSPECTION_DT.bindField], DateUtils.FormatType.Second );
                txtInspectionDt.Value = inspectionDt;

                //コントロールへの独自データバインド
                int gridColIndex = 30;
                int statusColIndex = 85;
                KTTextBox txt = null;
                for ( int i = 1; i <= 10; i++ ) {
                    if ( "3" == StringUtils.ToString( rowBind["twiceTightStatus" + i] ) ) {
                        //二度締締付状態が「二度締めNG後OK」
                        HtmlTableCell tdTwiceTorque = ( (HtmlTableCell)e.Item.FindControl( "tdTwiceMeasureVal" + i ) );
                        HtmlTableCell tdTwiceAngle = ( (HtmlTableCell)e.Item.FindControl( "tdTwiceAngleMeasureVal" + i ) );
                        if ( ObjectUtils.IsNotNull( tdTwiceTorque ) && ObjectUtils.IsNotNull( tdTwiceAngle ) ) {
                            tdTwiceTorque.Attributes["style"] = tdTwiceTorque.Attributes["style"] + " background-color:#FFFF00;";
                            tdTwiceAngle.Attributes["style"]  = tdTwiceAngle.Attributes["style"]  + " background-color:#FFFF00;";
                        }
                    }
                }

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );
            }
        }
        #endregion

        #region メインリスト選択行変更処理

        /// <summary>
        /// メインリスト選択行変更処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void SelectedIndexChangedMainList( int paramIndex ) {

            int mainIndex = paramIndex;

            Business.DetailViewBusiness.ResultSetMulti res = new Business.DetailViewBusiness.ResultSetMulti();
            Dictionary<string, object> dicPageControlInfo = CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).GetPageControlInfo( MANAGE_ID );
            if ( true == dicPageControlInfo.ContainsKey( SESSION_PAGE_INFO_DB_KEY ) ) {
                res = (Business.DetailViewBusiness.ResultSetMulti)dicPageControlInfo[SESSION_PAGE_INFO_DB_KEY];
            }

            //サブリスト初期化
            lstSubList.DataSource = null;
            lstSubList.DataBind();


            //サブリストバインド
            if ( 0 < res.SubTables.Length
                && 0 < res.SubTables[mainIndex].Rows.Count ) {
                lstSubList.DataSource = res.SubTables[mainIndex];
                lstSubList.DataBind();
            }
        }

        /// <summary>
        /// メインリスト選択行変更処理呼び出し（左）
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void SelectedIndexChangedMainLBList() {

            // 選択行インデックスを取得
            int mainIndex = lstMainListLB.SelectedIndex;

            //選択行背景色変更解除
            foreach ( ListViewDataItem li in lstMainListLB.Items ) {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace( " " + ResourcePath.CSS.ListSelectedRow, "" );
            }

            foreach ( ListViewDataItem li in lstMainListRB.Items ) {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace( " " + ResourcePath.CSS.ListSelectedRow, "" );
            }

            //一覧項目選択済に色変更
            HtmlTableRow trSelectRow = (HtmlTableRow)lstMainListLB.Items[mainIndex].FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
            trSelectRow.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;

            HtmlTableRow trSelectRowRB = (HtmlTableRow)lstMainListRB.Items[mainIndex].FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
            trSelectRowRB.Attributes["class"] = trSelectRowRB.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;

            // 計測結果表示更新処理
            SelectedIndexChangedMainList( mainIndex );

        }

        /// <summary>
        /// メインリスト選択行変更処理呼び出し（右）
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void SelectedIndexChangedMainRBList() {

            // 選択行インデックスを取得
            var mainIndex = lstMainListRB.SelectedIndex;

            //選択行背景色変更解除
            //foreach ( ListViewDataItem li in lstMainListLB.Items ) {
            //    HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
            //    trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace( " " + ResourcePath.CSS.ListSelectedRow, "" );
            //}

            foreach ( ListViewDataItem li in lstMainListRB.Items ) {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace( " " + ResourcePath.CSS.ListSelectedRow, "" );
            }


            //一覧項目選択済に色変更
            //HtmlTableRow trSelectRow = (HtmlTableRow)lstMainListLB.Items[mainIndex].FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
            //trSelectRow.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;

            HtmlTableRow trSelectRowRB = (HtmlTableRow)lstMainListRB.Items[mainIndex].FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
            trSelectRowRB.Attributes["class"] = trSelectRowRB.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;


            // 計測結果表示更新処理
            SelectedIndexChangedMainList( mainIndex );

        }
        #endregion

    }
}