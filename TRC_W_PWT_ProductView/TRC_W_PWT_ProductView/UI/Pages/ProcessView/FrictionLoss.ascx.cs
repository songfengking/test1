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
    /// (詳細 エンジン 工程) フリクションロス
    /// </summary>
    public partial class FrictionLoss : System.Web.UI.UserControl, Defines.Interface.IDetail {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義

        //GridView選択列制御
        const string GRID_MAIN_VIEW_GROUP_CD = "MainView";

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
            /// <summary>計測日時</summary>
            public static readonly GridViewDefine INSPECTION_DT = new GridViewDefine( "計測日時", "inspectionDt", typeof( DateTime ), "{0:yyyy/MM/dd HH:mm:ss}", false, HorizontalAlign.Center, 150, true, true );
            /// <summary>判定</summary>
            public static readonly GridViewDefine MESURE_RESULT = new GridViewDefine( "判定", "mesureResult", typeof( string ), "", false, HorizontalAlign.Center, 70, true, true );
            /// <summary>回転数(rpm)</summary>
            public static readonly GridViewDefine STANDARD_RPM = new GridViewDefine( "回転数<br />(rpm)", "standardRpm", typeof( string ), "", false, HorizontalAlign.Right, 100, true, true );
            /// <summary>最大値(N・m)</summary>
            public static readonly GridViewDefine MESURE_MAX_VAL = new GridViewDefine( "最大値<br />(N・m)", "mesureMaxVal", typeof( string ), "{0:f2}", false, HorizontalAlign.Right, 100, true, true );
            /// <summary>最小値(N・m)</summary>
            public static readonly GridViewDefine MESURE_MIN_VAL = new GridViewDefine( "最小値<br />(N・m)", "mesureMinVal", typeof( string ), "{0:f2}", false, HorizontalAlign.Right, 100, true, true );
            /// <summary>平均値(N・m)</summary>
            public static readonly GridViewDefine MESURE_AVG_VAL = new GridViewDefine( "平均値<br />(N・m)", "mesureAvgVal", typeof( string ), "{0:f2}", false, HorizontalAlign.Right, 100, true, true );
            /// <summary>気温</summary>
            public static readonly GridViewDefine MESURE_TEMPERATURE = new GridViewDefine( "気温", "mesureTemperature", typeof( string ), "{0:f1}", false, HorizontalAlign.Right, 100, true, true );
        }


        /// <summary>
        /// (サブリスト)コントロール定義
        /// </summary>
        public class GRID_SUB {
            //なし
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

        ///// <summary>
        ///// メインリスト行バインド
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
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
            Business.DetailViewBusiness.ResultSet resultSet = new Business.DetailViewBusiness.ResultSet();
            try {
                resultSet = Business.DetailViewBusiness.SelectEngineFrictionLossDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial );
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
        private void InitializeValues( Business.DetailViewBusiness.ResultSet resultSet ) {

            //来歴情報セット
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