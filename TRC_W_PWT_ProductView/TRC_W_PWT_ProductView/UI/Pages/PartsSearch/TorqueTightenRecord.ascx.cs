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

namespace TRC_W_PWT_ProductView.UI.Pages.PartsSearch {
    /// <summary>
    /// (詳細 ATU 工程) トルク締付
    /// </summary>
    public partial class TorqueTightenRecord : System.Web.UI.UserControl, Defines.Interface.IDetailParts {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義

        const string GRID_MAIN_VIEW_GROUP_CD = "MainPartsView";
        
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
        /// SubGridView
        /// </summary>
        /// 
        internal class GRID_MAIN_COMMON {
            /// <summary>部品名</summary>
            public static readonly GridViewDefine PARTS_NM = new GridViewDefine( "工具名", "partsNm", typeof( string ), "", false, HorizontalAlign.Center, 200, true, true );
            /// <summary>登録日時</summary>
            public static readonly GridViewDefine INSPECTION_DT = new GridViewDefine( "通過日", "inspectionDt", typeof( DateTime ), "{0:yyyy/MM/dd HH:mm:ss}", false, HorizontalAlign.Center, 160, true, true );
            /// <summary>ステーション</summary>
            public static readonly GridViewDefine STATION_CD = new GridViewDefine( "ステーション", "stationNm", typeof( string ), "", false, HorizontalAlign.Center, 160, true, true );
            /// <summary>端末名</summary>
            public static readonly GridViewDefine TERMINAL_NM = new GridViewDefine( "端末名", "terminalNm", typeof( string ), "", false, HorizontalAlign.Center, 120, true, true );
            /// <summary>計測通番</summary>
            public static readonly GridViewDefine HISTORY_INDEX = new GridViewDefine( "計測通番", "historyIndex", typeof( string ), "", false, HorizontalAlign.Right, 100, true, true );
            /// <summary>結果</summary>
            public static readonly GridViewDefine RESULT = new GridViewDefine( "結果", "result", typeof( string ), "", false, HorizontalAlign.Center, 80, true, true );
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
                return PageInfo.GetUCPageInfo( DetailPartsKeyParam.SearchTarget, DetailPartsKeyParam.PartsKind, DetailPartsKeyParam.ProcessCd );
            }
        }
        
        /// <summary>
        /// 詳細表示キー情報
        /// </summary>
        private Defines.Interface.ST_DETAIL_PARTS_PARAM _detailPartsKeyParam;
        /// <summary>
        /// 詳細表示キー情報
        /// </summary>
        public Defines.Interface.ST_DETAIL_PARTS_PARAM DetailPartsKeyParam {
            get {
                return _detailPartsKeyParam;
            }
            set {
                _detailPartsKeyParam = value;
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
        /// メインリスト行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMainViewLB_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainViewLB( sender, e );
        }

        protected void grvMainViewRB_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainViewRB( sender, e );
        }
        #endregion

        #region イベントメソッド

        #region ページイベント
        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize() {
            //検索結果取得
            Business.DetailPartsViewBusiness.ResultSet resultSet = new Business.DetailPartsViewBusiness.ResultSet();
            try {
                resultSet = Business.DetailPartsViewBusiness.SelectAtuTorqueDetail( DetailPartsKeyParam.ModelCd, DetailPartsKeyParam.Serial );
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

        /// <summary>
        /// 初期値設定
        /// </summary>
        private void InitializeValues( Business.DetailPartsViewBusiness.ResultSet resultSet ) {
            /*
            //来歴情報セット
            lstMainList.DataSource = resultSet.MainTable;
            lstMainList.DataBind();

            lstMainList.SelectedIndex = 0;
             * */

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