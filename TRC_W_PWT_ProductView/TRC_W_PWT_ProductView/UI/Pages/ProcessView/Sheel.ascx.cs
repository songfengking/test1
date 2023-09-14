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
    /// (詳細 トラクタ 工程) 刻印
    /// </summary>
    public partial class Sheel : System.Web.UI.UserControl, Defines.Interface.IDetail {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義
        /// <summary>
        /// ステーションコード
        /// </summary>
        /// 固定としておく
        private const string CONST_STATION_CD = "214302";

        //GridView選択列制御
        const string GRID_MAIN_VIEW_GROUP_CD = "MainView";
        const string GRID_MS_VIEW_GROUP_CD = "MSView";
        const string GRID_MID_VIEW_GROUP_CD = "MIDView";

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
        /// 検索結果(メイン)
        /// </summary>
        /// 
        internal class GRID_MAIN {
            /// <summary>刻印文字</summary>
            public static readonly GridViewDefine CONTENTS_1 = new GridViewDefine( "刻印文字", "CONTENTS_1", typeof( string ), "", false, HorizontalAlign.Center, 250, true, true );
            /// <summary>エラー内容</summary>
            public static readonly GridViewDefine CONTENTS_2 = new GridViewDefine( "エラー内容", "CONTENTS_2", typeof( string ), "", false, HorizontalAlign.Left, 250, true, true );
            /// <summary>PIN区分</summary>
            public static readonly GridViewDefine PIN_KBN = new GridViewDefine( "PIN区分", "PIN_KBN", typeof( string ), "", false, HorizontalAlign.Center, 80, true, true );
            /// <summary>刻印方法</summary>
            public static readonly GridViewDefine AUTO = new GridViewDefine( "刻印方法", "AUTO", typeof( string ), "", false, HorizontalAlign.Center, 160, true, true );
            /// <summary>刻印日時</summary>
            public static readonly GridViewDefine PRINT_DT = new GridViewDefine( "刻印日時", "PRINT_DT", typeof( string ), "{0:yyyy/MM/dd HH:mm:ss}", false, HorizontalAlign.Center, 160, true, true );
        }
        /// <summary>
        /// 検索結果(サブ)
        /// </summary>
        /// 
        internal class GRID_SUB {
            /// <summary>刻印文字</summary>
            public static readonly GridViewDefine CONTENTS_1 = new GridViewDefine( "刻印文字", "CONTENTS_1", typeof( string ), "", false, HorizontalAlign.Center, 250, true, true );
            /// <summary>エラー内容</summary>
            public static readonly GridViewDefine CONTENTS_2 = new GridViewDefine( "エラー内容", "CONTENTS_2", typeof( string ), "", false, HorizontalAlign.Left, 250, true, true );
            /// <summary>刻印方法</summary>
            public static readonly GridViewDefine AUTO = new GridViewDefine( "刻印方法", "AUTO", typeof( string ), "", false, HorizontalAlign.Center, 160, true, true );
            /// <summary>刻印日時</summary>
            public static readonly GridViewDefine PRINT_DT = new GridViewDefine( "刻印日時", "PRINT_DT", typeof( string ), "{0:yyyy/MM/dd HH:mm:ss}", false, HorizontalAlign.Center, 160, true, true );
        }

        #endregion

        #region CSS

        /// <summary>
        /// サムネイル項目 項目選択済用CSS
        /// </summary>
        const string LIST_SELECTED_ITEM_CSS = "div-item-selected";
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
                return PageInfo.GetUCPageInfo( DetailKeyParam.ProductKind, DetailKeyParam.GroupCd, DetailKeyParam.ClassCd, DetailKeyParam.AssemblyPatternCd );
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

        #endregion

        #region イベントメソッド

        #region ページイベント

        /// <summary>
        /// ページロード処理
        /// </summary>
        private void DoPageLoad() {
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize() {

            //検索結果取得
            Business.DetailViewBusiness.ResultSetMulti res = new Business.DetailViewBusiness.ResultSetMulti();
            try {
                //検査情報ヘッダ取得
                res = Business.DetailViewBusiness.SelectPrintSheel( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial, CONST_STATION_CD, DetailKeyParam.CountryCd );

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

            if ( 0 == res.MainTable.Rows.Count ) {
                //検索結果0件
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62010, CurrentUCInfo.title );
                return;
            }

            //初期表示データbind
            InitializeValues( res );
        }

        /// <summary>
        /// 初期値設定
        /// </summary>
        private void InitializeValues( Business.DetailViewBusiness.ResultSetMulti res ) {

            //前車軸フレーム刻印
            //一覧表示列の設定
            GridViewDefine[] gridColumns;
            List<GridViewDefine> columns = new List<GridViewDefine>();
            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_MAIN ) ) );
            gridColumns = columns.ToArray();

            ConditionInfoSessionHandler.ST_CONDITION cond = new ConditionInfoSessionHandler.ST_CONDITION();
            cond.ResultData = res.SubTables[0];

            //グリッドビューバインド
            GridView frozenGrid = grvMainViewLB;
            //新規バインド
            ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, gridColumns, true ), cond, true );
            ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, gridColumns, false ), cond, true );

            if ( 0 < res.SubTables[0].Rows.Count ) {
                ControlUtils.BindGridView_WithTempField( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, gridColumns, true ), res.SubTables[0] );
                ControlUtils.BindGridView_WithTempField( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, gridColumns, false ), res.SubTables[0] );
                //グリッドビュー外のDivサイズ変更
                SetDivGridViewWidth();
            } else {
                ClearGridView();
            }

            //ミッション
            //一覧表示列の設定
            GridViewDefine[] gridColumns_MS;
            List<GridViewDefine> columns_MS = new List<GridViewDefine>();
            columns_MS.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_SUB ) ) );
            gridColumns_MS = columns_MS.ToArray();

            ConditionInfoSessionHandler.ST_CONDITION cond_MS = new ConditionInfoSessionHandler.ST_CONDITION();
            cond_MS.ResultData = res.SubTables[1];

            //グリッドビューバインド
            GridView frozenGrid_MS = grvMainViewLB_MS;

            //新規バインド
            ControlUtils.ShowGridViewHeader( grvHeaderLT_MS, ControlUtils.GetFrozenColumns( frozenGrid_MS, gridColumns_MS, true ), cond_MS, true );
            ControlUtils.ShowGridViewHeader( grvHeaderRT_MS, ControlUtils.GetFrozenColumns( frozenGrid_MS, gridColumns_MS, false ), cond_MS, true );

            if ( 0 < res.SubTables[1].Rows.Count ) {
                ControlUtils.BindGridView_WithTempField( grvMainViewLB_MS, ControlUtils.GetFrozenColumns( frozenGrid_MS, gridColumns_MS, true ), res.SubTables[1] );
                ControlUtils.BindGridView_WithTempField( grvMainViewRB_MS, ControlUtils.GetFrozenColumns( frozenGrid_MS, gridColumns_MS, false ), res.SubTables[1] );
                //グリッドビュー外のDivサイズ変更
                SetDivGridViewWidth_MS();
            }else{
                ClearGridView_MS();

            }

            //MIDケース

            //一覧表示列の設定
            GridViewDefine[] gridColumns_MID;
            List<GridViewDefine> columns_MID = new List<GridViewDefine>();
            columns_MID.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_SUB ) ) );
            gridColumns_MID = columns_MID.ToArray();

            ConditionInfoSessionHandler.ST_CONDITION cond_MID = new ConditionInfoSessionHandler.ST_CONDITION();
            cond_MID.ResultData = res.SubTables[2];

            //グリッドビューバインド
            GridView frozenGrid_MID = grvMainViewLB_MID;

            //新規バインド
            ControlUtils.ShowGridViewHeader( grvHeaderLT_MID, ControlUtils.GetFrozenColumns( frozenGrid_MID, gridColumns_MID, true ), cond_MID, true );
            ControlUtils.ShowGridViewHeader( grvHeaderRT_MID, ControlUtils.GetFrozenColumns( frozenGrid_MID, gridColumns_MID, false ), cond_MID, true );

            if ( 0 < res.SubTables[2].Rows.Count ) {
                ControlUtils.BindGridView_WithTempField( grvMainViewLB_MID, ControlUtils.GetFrozenColumns( frozenGrid_MID, gridColumns_MID, true ), res.SubTables[2] );
                ControlUtils.BindGridView_WithTempField( grvMainViewRB_MID, ControlUtils.GetFrozenColumns( frozenGrid_MID, gridColumns_MID, false ), res.SubTables[2] );
                //グリッドビュー外のDivサイズ変更
                SetDivGridViewWidth_MID();
            } else {
                ClearGridView_MID();
            }

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
        /// <summary>
        /// グリッドビュー行バインド(左下)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowDataBoundMainViewLB_MS( params object[] parameters ) {
            object sender = parameters[0];
            GridViewRowEventArgs e = (GridViewRowEventArgs)parameters[1];

            if ( e.Row.RowType == DataControlRowType.DataRow ) {
                DataRow row = ( (DataRowView)e.Row.DataItem ).Row;

                Dictionary<string, object> dicControls = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Row, ControlUtils.GetControlDefineArray( typeof( GRID_SEARCH_CONTROLS_L ) ), row, ref dicControls );

            }
            ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_MS_VIEW_GROUP_CD );

        }
        /// <summary>
        /// グリッドビュー行バインド(右下)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowDataBoundMainViewRB_MS( params object[] parameters ) {
            object sender = parameters[0];
            GridViewRowEventArgs e = (GridViewRowEventArgs)parameters[1];

            if ( e.Row.RowType == DataControlRowType.DataRow ) {
                DataRow rowData = ( (DataRowView)e.Row.DataItem ).Row;
                Dictionary<string, object> dicControls = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Row, ControlUtils.GetControlDefineArray( typeof( GRID_SEARCH_CONTROLS_R ) ), rowData, ref dicControls );
            }

            ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_MS_VIEW_GROUP_CD );

        }
        /// <summary>
        /// グリッドビュー行バインド(左下)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowDataBoundMainViewLB_MID( params object[] parameters ) {
            object sender = parameters[0];
            GridViewRowEventArgs e = (GridViewRowEventArgs)parameters[1];

            if ( e.Row.RowType == DataControlRowType.DataRow ) {
                DataRow row = ( (DataRowView)e.Row.DataItem ).Row;

                Dictionary<string, object> dicControls = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Row, ControlUtils.GetControlDefineArray( typeof( GRID_SEARCH_CONTROLS_L ) ), row, ref dicControls );

            }
            ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_MID_VIEW_GROUP_CD );

        }
        /// <summary>
        /// グリッドビュー行バインド(右下)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowDataBoundMainViewRB_MID( params object[] parameters ) {
            object sender = parameters[0];
            GridViewRowEventArgs e = (GridViewRowEventArgs)parameters[1];

            if ( e.Row.RowType == DataControlRowType.DataRow ) {
                DataRow rowData = ( (DataRowView)e.Row.DataItem ).Row;
                Dictionary<string, object> dicControls = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Row, ControlUtils.GetControlDefineArray( typeof( GRID_SEARCH_CONTROLS_R ) ), rowData, ref dicControls );
            }

            ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_MID_VIEW_GROUP_CD );

        }
        #endregion

        #endregion

        #region グリッド設定
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
        /// グリッドビュー格納DIVサイズ調整
        /// </summary>
        private void SetDivGridViewWidth_MS() {
            SetDivGridViewWidth( grvHeaderLT_MS, divGrvHeaderLT_MS );
            SetDivGridViewWidth( grvHeaderRT_MS, divGrvHeaderRT_MS );

            SetDivGridViewWidth( grvMainViewLB_MS, divGrvLB_MS );
            SetDivGridViewWidth( grvMainViewRB_MS, divGrvRB_MS );
        }
        /// <summary>
        /// グリッドビュー格納DIVサイズ調整
        /// </summary>
        private void SetDivGridViewWidth_MID() {
            SetDivGridViewWidth( grvHeaderLT_MID, divGrvHeaderLT_MID );
            SetDivGridViewWidth( grvHeaderRT_MID, divGrvHeaderRT_MID );

            SetDivGridViewWidth( grvMainViewLB_MID, divGrvLB_MID );
            SetDivGridViewWidth( grvMainViewRB_MID, divGrvRB_MID );
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
            //ControlUtils.InitializeGridView( grvHeaderLT, false );
            //ControlUtils.InitializeGridView( grvHeaderRT, false );
            ControlUtils.InitializeGridView( grvMainViewLB, false );
            ControlUtils.InitializeGridView( grvMainViewRB, false );

            //グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();

            //GridView非表示
            //divGrvDisplay.Visible = false;
        }
        /// <summary>
        /// グリッドビュークリア
        /// </summary>
        private void ClearGridView_MS() {

            //列名非表示 グリッドビュークリア
            //ControlUtils.InitializeGridView( grvHeaderLT_MS, false );
            //ControlUtils.InitializeGridView( grvHeaderRT_MS, false );
            ControlUtils.InitializeGridView( grvMainViewLB_MS, false );
            ControlUtils.InitializeGridView( grvMainViewRB_MS, false );

            //グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth_MS();

        }
        /// <summary>
        /// グリッドビュークリア
        /// </summary>
        private void ClearGridView_MID() {

            //列名非表示 グリッドビュークリア
            //ControlUtils.InitializeGridView( grvHeaderLT_MID, false );
            //ControlUtils.InitializeGridView( grvHeaderRT_MID, false );
            ControlUtils.InitializeGridView( grvMainViewLB_MID, false );
            ControlUtils.InitializeGridView( grvMainViewRB_MID, false );

            //グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth_MID();

        }

        #endregion

        protected void grvMainViewLB_MS_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainViewLB_MS( sender, e );
        }

        protected void grvMainViewRB_MS_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainViewRB_MS( sender, e );
        }

        protected void grvMainViewLB_MID_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainViewLB_MID( sender, e );
        }

        protected void grvMainViewRB_MID_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainViewRB_MID( sender, e );
        }
    }
}