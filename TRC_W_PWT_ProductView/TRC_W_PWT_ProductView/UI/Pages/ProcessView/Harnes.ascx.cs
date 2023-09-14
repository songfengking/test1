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
    /// (詳細 エンジン 部品) ハーネス検査
    /// </summary>
    public partial class Harnes : System.Web.UI.UserControl, Defines.Interface.IDetail {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義

        const string GRID_MAIN_VIEW_GROUP_CD = "MainView";

        /// <summary>
        /// (メインリスト)コントロール定義
        /// </summary>
        public class GRID_MAIN {
            /// <summary>TR</summary>
            public static readonly ControlDefine TR_ROW_DATA = new ControlDefine( "trRowData", "TR", "", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>検査日時</summary>
            public static readonly ControlDefine INSPECTION_DT = new ControlDefine( "txtInspectionDt", "検査日時", "inspectionDt", ControlDefine.BindType.None, typeof( DateTime ) );
            /// <summary>判定</summary>
            public static readonly ControlDefine RESULT = new ControlDefine( "txtResult", "判定", "result", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>試験連番</summary>
            public static readonly ControlDefine HISTORY_INDEX = new ControlDefine( "ntbHistoryIndex", "試験連番", "historyIndex", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>コモンレール</summary>
            public static readonly ControlDefine RESULT_CRS = new ControlDefine( "txtResultCrs", "コモンレール", "resultCrs", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>インジェクタ</summary>
            public static readonly ControlDefine RESULT_INJ = new ControlDefine( "txtResultInj", "インジェクタ", "resultInj", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>ＤＰＦ</summary>
            public static readonly ControlDefine RESULT_DPF = new ControlDefine( "txtResultDpf", "ＤＰＦ", "resultDpf", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>Ｇセンサ</summary>
            public static readonly ControlDefine RESULT_G = new ControlDefine( "txtResultG", "Ｇセンサ", "resultG", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>レール圧</summary>
            public static readonly ControlDefine RESULT_REIL = new ControlDefine( "txtResultReil", "レール圧", "resultReil", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>エラーコード１</summary>
            public static readonly ControlDefine ERROR_CD_1 = new ControlDefine( "txtErrorCd1", "エラーコード１", "errorCd1", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>エラーコード２</summary>
            public static readonly ControlDefine ERROR_CD_2 = new ControlDefine( "txtErrorCd2", "エラーコード２", "errorCd2", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>エラーコード３</summary>
            public static readonly ControlDefine ERROR_CD_3 = new ControlDefine( "txtErrorCd3", "エラーコード３", "errorCd3", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>エラーコード４</summary>
            public static readonly ControlDefine ERROR_CD_4 = new ControlDefine( "txtErrorCd4", "エラーコード４", "errorCd4", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>エラーコード５</summary>
            public static readonly ControlDefine ERROR_CD_5 = new ControlDefine( "txtErrorCd5", "エラーコード５", "errorCd5", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>エラーコード６</summary>
            public static readonly ControlDefine ERROR_CD_6 = new ControlDefine( "txtErrorCd6", "エラーコード６", "errorCd6", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>エラーコード７</summary>
            public static readonly ControlDefine ERROR_CD_7 = new ControlDefine( "txtErrorCd7", "エラーコード７", "errorCd7", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>エラーコード８</summary>
            public static readonly ControlDefine ERROR_CD_8 = new ControlDefine( "txtErrorCd8", "エラーコード８", "errorCd8", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>エラーコード９</summary>
            public static readonly ControlDefine ERROR_CD_9 = new ControlDefine( "txtErrorCd9", "エラーコード９", "errorCd9", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>エラーコード１０</summary>
            public static readonly ControlDefine ERROR_CD_10 = new ControlDefine( "txtErrorCd10", "エラーコード１０", "errorCd10", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>エラーコード１１</summary>
            public static readonly ControlDefine ERROR_CD_11 = new ControlDefine( "txtErrorCd11", "エラーコード１１", "errorCd11", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>エラーコード１２</summary>
            public static readonly ControlDefine ERROR_CD_12 = new ControlDefine( "txtErrorCd12", "エラーコード１２", "errorCd12", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>エラーコード１３</summary>
            public static readonly ControlDefine ERROR_CD_13 = new ControlDefine( "txtErrorCd13", "エラーコード１３", "errorCd13", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>エラーコード１４</summary>
            public static readonly ControlDefine ERROR_CD_14 = new ControlDefine( "txtErrorCd14", "エラーコード１４", "errorCd14", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>エラーコード１５</summary>
            public static readonly ControlDefine ERROR_CD_15 = new ControlDefine( "txtErrorCd15", "エラーコード１５", "errorCd15", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>エラーコード１６</summary>
            public static readonly ControlDefine ERROR_CD_16 = new ControlDefine( "txtErrorCd16", "エラーコード１６", "errorCd16", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>エラーコード１７</summary>
            public static readonly ControlDefine ERROR_CD_17 = new ControlDefine( "txtErrorCd17", "エラーコード１７", "errorCd17", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>エラーコード１８</summary>
            public static readonly ControlDefine ERROR_CD_18 = new ControlDefine( "txtErrorCd18", "エラーコード１８", "errorCd18", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>エラーコード１９</summary>
            public static readonly ControlDefine ERROR_CD_19 = new ControlDefine( "txtErrorCd19", "エラーコード１９", "errorCd19", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>エラーコード２０</summary>
            public static readonly ControlDefine ERROR_CD_20 = new ControlDefine( "txtErrorCd20", "エラーコード２０", "errorCd20", ControlDefine.BindType.Down, typeof( String ) );
        }

        /// <summary>
        /// (サブリスト)コントロール定義
        /// </summary>
        public class GRID_SUB {
            //なし
        }

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

        /// <summary>
        /// 検索結果(更新時共通部)
        /// </summary>
        /// 
        internal class GRID_MAIN_COMMON {
            /// <summary>検査日時</summary>
            public static readonly GridViewDefine INSPECTION_DT = new GridViewDefine( "検査日時", "inspectionDt", typeof( DateTime ), "{0:yyyy/MM/dd HH:mm:ss}", false, HorizontalAlign.Center, 150, true, true );
            /// <summary>判定</summary>
            public static readonly GridViewDefine RESULT = new GridViewDefine( "判定", "result", typeof( string ), "", false, HorizontalAlign.Center, 70, true, true );
            /// <summary>試験連番</summary>
            public static readonly GridViewDefine HISTORY_INDEX = new GridViewDefine( "試験連番", "historyIndex", typeof( string ), "", false, HorizontalAlign.Right, 80, true, true );
            /// <summary>コモンレール</summary>
            public static readonly GridViewDefine UPPER_LIMIT = new GridViewDefine( "コモンレール", "resultCrs", typeof( string ), "", false, HorizontalAlign.Center, 110, true, true );
            /// <summary>インジェクタ</summary>
            public static readonly GridViewDefine LOWER_LIMIT = new GridViewDefine( "インジェクタ", "resultInj", typeof( string ), "", false, HorizontalAlign.Center, 110, true, true );
            /// <summary>ＤＰＦ</summary>
            public static readonly GridViewDefine MEASURE_VAL_1 = new GridViewDefine( "ＤＰＦ", "resultDpf", typeof( string ), "", false, HorizontalAlign.Center, 110, true, true );
            /// <summary>Ｇセンサ</summary>
            public static readonly GridViewDefine MEASURE_VAL_2 = new GridViewDefine( "Ｇセンサ", "resultG", typeof( string ), "", false, HorizontalAlign.Center, 110, true, true );
            /// <summary>レール圧</summary>
            public static readonly GridViewDefine MEASURE_VAL_3 = new GridViewDefine( "レール圧", "resultReil", typeof( string ), "", false, HorizontalAlign.Center, 110, true, true );
            /// <summary>エラーコード１</summary>
            public static readonly GridViewDefine ERROR_CD_1 = new GridViewDefine( "エラー<br />コード1", "errorCd1", typeof( string ), "", false, HorizontalAlign.Left, 80, true, true );
            /// <summary>エラーコード２</summary>
            public static readonly GridViewDefine ERROR_CD_2 = new GridViewDefine( "エラー<br />コード2", "errorCd2", typeof( string ), "", false, HorizontalAlign.Left, 80, true, true );
            /// <summary>エラーコード３</summary>
            public static readonly GridViewDefine ERROR_CD_3 = new GridViewDefine( "エラー<br />コード3", "errorCd3", typeof( string ), "", false, HorizontalAlign.Left, 80, true, true );
            /// <summary>エラーコード４</summary>
            public static readonly GridViewDefine ERROR_CD_4 = new GridViewDefine( "エラー<br />コード4", "errorCd4", typeof( string ), "", false, HorizontalAlign.Left, 80, true, true );
            /// <summary>エラーコード５</summary>
            public static readonly GridViewDefine ERROR_CD_5 = new GridViewDefine( "エラー<br />コード5", "errorCd5", typeof( string ), "", false, HorizontalAlign.Left, 80, true, true );
            /// <summary>エラーコード６</summary>
            public static readonly GridViewDefine ERROR_CD_6 = new GridViewDefine( "エラー<br />コード6", "errorCd6", typeof( string ), "", false, HorizontalAlign.Left, 80, true, true );
            /// <summary>エラーコード７</summary>
            public static readonly GridViewDefine ERROR_CD_7 = new GridViewDefine( "エラー<br />コード7", "errorCd7", typeof( string ), "", false, HorizontalAlign.Left, 80, true, true );
            /// <summary>エラーコード８</summary>
            public static readonly GridViewDefine ERROR_CD_8 = new GridViewDefine( "エラー<br />コード8", "errorCd8", typeof( string ), "", false, HorizontalAlign.Left, 80, true, true );
            /// <summary>エラーコード９</summary>
            public static readonly GridViewDefine ERROR_CD_9 = new GridViewDefine( "エラー<br />コード9", "errorCd9", typeof( string ), "", false, HorizontalAlign.Left, 80, true, true );
            /// <summary>エラーコード１０</summary>
            public static readonly GridViewDefine ERROR_CD_10 = new GridViewDefine( "エラー<br />コード10", "errorCd10", typeof( string ), "", false, HorizontalAlign.Left, 80, true, true );
            /// <summary>エラーコード１１</summary>
            public static readonly GridViewDefine ERROR_CD_11 = new GridViewDefine( "エラー<br />コード11", "errorCd11", typeof( string ), "", false, HorizontalAlign.Left, 80, true, true );
            /// <summary>エラーコード１２</summary>
            public static readonly GridViewDefine ERROR_CD_12 = new GridViewDefine( "エラー<br />コード12", "errorCd12", typeof( string ), "", false, HorizontalAlign.Left, 80, true, true );
            /// <summary>エラーコード１３</summary>
            public static readonly GridViewDefine ERROR_CD_13 = new GridViewDefine( "エラー<br />コード13", "errorCd13", typeof( string ), "", false, HorizontalAlign.Left, 80, true, true );
            /// <summary>エラーコード１４</summary>
            public static readonly GridViewDefine ERROR_CD_14 = new GridViewDefine( "エラー<br />コード14", "errorCd14", typeof( string ), "", false, HorizontalAlign.Left, 80, true, true );
            /// <summary>エラーコード１５</summary>
            public static readonly GridViewDefine ERROR_CD_15 = new GridViewDefine( "エラー<br />コード15", "errorCd15", typeof( string ), "", false, HorizontalAlign.Left, 80, true, true );
            /// <summary>エラーコード１６</summary>
            public static readonly GridViewDefine ERROR_CD_16 = new GridViewDefine( "エラー<br />コード16", "errorCd16", typeof( string ), "", false, HorizontalAlign.Left, 80, true, true );
            /// <summary>エラーコード１７</summary>
            public static readonly GridViewDefine ERROR_CD_17 = new GridViewDefine( "エラー<br />コード17", "errorCd17", typeof( string ), "", false, HorizontalAlign.Left, 80, true, true );
            /// <summary>エラーコード１８</summary>
            public static readonly GridViewDefine ERROR_CD_18 = new GridViewDefine( "エラー<br />コード18", "errorCd18", typeof( string ), "", false, HorizontalAlign.Left, 80, true, true );
            /// <summary>エラーコード１９</summary>
            public static readonly GridViewDefine ERROR_CD_19 = new GridViewDefine( "エラー<br />コード19", "errorCd19", typeof( string ), "", false, HorizontalAlign.Left, 80, true, true );
            /// <summary>エラーコード２０</summary>
            public static readonly GridViewDefine ERROR_CD_20 = new GridViewDefine( "エラー<br />コード20", "errorCd20", typeof( string ), "", false, HorizontalAlign.Left, 80, true, true );

        
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

        #region 初期化

        /// <summary>
        /// ページロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load( object sender, EventArgs e ) {
            CurrentForm.RaiseEvent( DoPageLoad );
        }

        /// <summary>
        /// 初期値設定
        /// </summary>
        private void InitializeValues( Business.DetailViewBusiness.ResultSet resultSet ) {

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
                    ControlUtils.BindGridView_WithTempField( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, gridColumns, true ), resultSet.MainTable );
                    ControlUtils.BindGridView_WithTempField( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, gridColumns, false ), resultSet.MainTable );

                    //グリッドビュー外のDivサイズ変更
                    SetDivGridViewWidth();
                } else {
                    ClearGridView();
                }                       
            }
        }

        #endregion

        #region イベントメソッド

        #region ページイベント
        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize() {
            //検索結果取得
            Business.DetailViewBusiness.ResultSet resultSet = new Business.DetailViewBusiness.ResultSet();
            try {
                resultSet = Business.DetailViewBusiness.SelectEngineHarnessDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial );
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

        #region リストバインド

        /// <summary>
        /// グリッドビュー行バインドCALL(左下)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMainViewLB_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainViewLB( sender, e );
        }
        /// <summary>
        /// グリッドビュー行バインドCALL(右下)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMainViewRB_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainViewRB( sender, e );
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

                Dictionary<string, object> dicControls = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Row, ControlUtils.GetControlDefineArray( typeof( GRID_SEARCH_CONTROLS_L ) ), row, ref dicControls );

            }
            ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_MAIN_VIEW_GROUP_CD );

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
        #endregion

        #endregion

        #region Grid設定
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
            divGrvDisplay.Visible = false;
        }
        #endregion

    }
}