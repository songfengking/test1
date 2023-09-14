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

namespace TRC_W_PWT_ProductView.UI.Pages.PartsView {
    /// <summary>
    /// エンジン部品詳細画面:機番ラベル印刷
    /// </summary>
    public partial class SerialPrint : System.Web.UI.UserControl, Defines.Interface.IDetail {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義

        //GridView選択列制御
        const string GRID_MAIN_VIEW_GROUP_CD = "MainView";

        /// <summary>
        /// 詳細情報
        /// </summary>
        public class GRID_SUB {
            //詳細なし
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
        internal class GRID_MAIN {
            /// <summary>ステーション</summary>
            public static readonly GridViewDefine STATION_NM = new GridViewDefine( "ステーション", "stationNm", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <summary>印字エンジン型式</summary>
            public static readonly GridViewDefine ENGINE_MODEL_CD = new GridViewDefine( "印字型式", "printCont3", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <summary>印字エンジン機番</summary>
            public static readonly GridViewDefine ENGINE_SERIAL = new GridViewDefine( "印字機番", "printCont2", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <summary>刻印文字</summary>
            public static readonly GridViewDefine PRINT_CONT = new GridViewDefine( "刻印文字", "printCont1", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <summary>QRコード</summary>
            public static readonly GridViewDefine QR_CD = new GridViewDefine( "QRコード", "printCont5", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <summary>自動フラグ</summary>
            public static readonly GridViewDefine AUTO_FLAG = new GridViewDefine( "自動フラグ", "autoFlag", typeof( string ), "", true, HorizontalAlign.Center, 120, true, true );
            /// <summary>印刷日時</summary>
            public static readonly GridViewDefine PRINT_DT = new GridViewDefine( "印刷日時", "printDt", typeof( DateTime ), "", true, HorizontalAlign.Center, 160, true, true );
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
        /// コントロール定義
        /// </summary>
        ControlDefine[] _criticalPartsControls = null;
        /// <summary>
        /// コントロール定義アクセサ
        /// </summary>
        ControlDefine[] CriticalPartsControls {
            get {
                if ( true == ObjectUtils.IsNull( _criticalPartsControls ) ) {
                    _criticalPartsControls = ControlUtils.GetControlDefineArray( typeof( GRID_MAIN ) );
                }
                return _criticalPartsControls;
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
        /// 初期化処理
        /// </summary>
        public void Initialize() {

            //検索結果取得
            DataTable tblMain = null;
            try {
                tblMain = Business.DetailViewBusiness.SelectSerialPrint( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
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

            if ( 0 == tblMain.Rows.Count ) {
                //検索結果0件
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62010, CurrentUCInfo.title );
                return;
            }

            InitializeValues( tblMain );
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
        private void InitializeValues( DataTable tblMainData ) {
            //一覧表示列の設定
            GridViewDefine[] gridColumns;
            List<GridViewDefine> columns = new List<GridViewDefine>();
            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_MAIN ) ) );
            gridColumns = columns.ToArray();

            ConditionInfoSessionHandler.ST_CONDITION cond = new ConditionInfoSessionHandler.ST_CONDITION();
            cond.ResultData = tblMainData;

            //グリッドビューバインド
            GridView frozenGrid = grvMainViewLB;
            //新規バインド
            ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, gridColumns, true ), cond, true );
            ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, gridColumns, false ), cond, true );

            if ( 0 < cond.ResultData.Rows.Count ) {
                ControlUtils.BindGridView_WithTempField( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, gridColumns, true ), cond.ResultData );
                ControlUtils.BindGridView_WithTempField( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, gridColumns, false ), cond.ResultData );
                //グリッドビュー外のDivサイズ変更
                SetDivGridViewWidth();
            } else {
                ClearGridView();
            }
        }


        #endregion

        protected void grvMainViewLB_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainViewLB( sender, e );
        }

        protected void grvMainViewRB_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainViewRB( sender, e );
        }

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
        #endregion

    }
}