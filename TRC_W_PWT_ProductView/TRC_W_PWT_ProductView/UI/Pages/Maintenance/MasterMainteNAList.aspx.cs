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
using TRC_W_PWT_ProductView.Business;
using TRC_W_PWT_ProductView.Dao.Process;
using TRC_W_PWT_ProductView.UI.Pages.UserControl;

namespace TRC_W_PWT_ProductView.UI.Pages {

    /// <summary>
    /// チェック対象外一覧画面
    /// </summary>
    public partial class MasterMainteNAList : BaseForm {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 固定値
        /// <summary>
        /// グリッドビューグループ
        /// </summary>
        const string GRID_MAIN_VIEW_GROUP_CD = "MainView";

        const string CONST_IN_PRODUCT = "投入順序";
        const string CONST_NOT_APPLICABLE = "登録済(チェック対象外)";

        const string CONST_ORDER_NO = "0";  //投入順序
        const string CONST_NA_LIST = "1";   //登録済(チェック対象外)

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
        GridViewDefine[] gridviewDefault {
            get {
                if ( true == ObjectUtils.IsNull( _gridviewDefault ) ) {
                    _gridviewDefault = ControlUtils.GetGridViewDefineArray( typeof( GRID_COMMON ) );
                }
                return _gridviewDefault;
            }
        }
        #endregion

        #region 検索条件定義
        /// <summary>
        /// 一覧検索条件定義
        /// </summary>
        public class CONDITION {
            /// <summary>製品種別</summary>
            public static readonly ControlDefine PRODUCT_KIND_CD = new ControlDefine( "rblProductKind", "製品種別", "productKindCd", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>検索区分</summary>
            public static readonly ControlDefine SEARCH_KBN = new ControlDefine( "rblSearchKbn", "検索区分", "SearchKbn", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>型式コード</summary>
            public static readonly ControlDefine MODEL_CD = new ControlDefine( "txtModelCd", "型式コード", "modelCd", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>型式名</summary>
            public static readonly ControlDefine MODEL_NM = new ControlDefine( "txtModelNm", "型式名", "modelNm", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>製品機番</summary>
            public static readonly ControlDefine SERIAL = new ControlDefine( "txtProductSerial", "製品機番", "serial", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>範囲(完成予定日_開始)</summary>
            public static readonly ControlDefine DATE_COMP_FROM = new ControlDefine( "cldCompStart", "範囲", "dateCompFrom", ControlDefine.BindType.Both, typeof( DateTime ) );
            /// <summary>範囲(完成予定日_終了)</summary>
            public static readonly ControlDefine DATE_COMP_TO = new ControlDefine( "cldCompEnd", "範囲", "dateCompTo", ControlDefine.BindType.Both, typeof( DateTime ) );
            /// <summary>範囲(開始)</summary>
            public static readonly ControlDefine DATE_FROM = new ControlDefine( "cldStart", "範囲", "dateFrom", ControlDefine.BindType.Both, typeof( DateTime ) );
            /// <summary>範囲(終了)</summary>
            public static readonly ControlDefine DATE_TO = new ControlDefine( "cldEnd", "範囲", "dateTo", ControlDefine.BindType.Both, typeof( DateTime ) );
        }
        #endregion

        #region グリッドビュー定義

        /// <summary>
        /// 検索区分：生産中
        /// </summary>
        internal class GRID_IN_PRODUCT {
            /// <summary>順序連番</summary>
            public static readonly GridViewDefine JUN_NO = new GridViewDefine( "投入順序連番", "JUN_NO", typeof( string ), "", true, HorizontalAlign.Center, 140, true );
            /// <summary>型式コード</summary>
            public static readonly GridViewDefine MODEL_CD = new GridViewDefine( "型式コード", "MODEL_CD", typeof( string ), "", true, HorizontalAlign.Center, 140, true );
            /// <summary>型式名</summary>
            public static readonly GridViewDefine MODEL_NM = new GridViewDefine( "型式名", "MODEL_NM", typeof( string ), "", true, HorizontalAlign.Left, 200, true );
            /// <summary>機番</summary>
            public static readonly GridViewDefine SERIAL_NO = new GridViewDefine( "機番", "SERIAL_NO", typeof( string ), "", true, HorizontalAlign.Center, 100, true );
            /// <summary>6桁機番</summary>
            public static readonly GridViewDefine SERIAL6 = new GridViewDefine( "6桁機番", "SERIAL6", typeof( string ), "", true, HorizontalAlign.Center, 0, false );
            /// <summary>完成予定日</summary>
            public static readonly GridViewDefine KAN_YO_YM = new GridViewDefine( "完成予定日", "KAN_YO_YM", typeof( string ), "", true, HorizontalAlign.Center, 140, true );
            /// <summary>ﾁｪｯｸ対象外登録</summary>
            public static readonly GridViewDefine DATA_CNT = new GridViewDefine( "ﾁｪｯｸ対象外登録", "DATA_CNT", typeof( string ), "", true, HorizontalAlign.Center, 140, true );
            /// <summary>パターンコード</summary>
            public static readonly GridViewDefine PTN_CD = new GridViewDefine( "パターンコード", "PTN_CD", typeof( string ), "", true, HorizontalAlign.Center, 0, false );
        }

        /// <summary>
        /// 検索区分：登録済(チェック対象外)
        /// </summary>
        internal class GRID_NOT_APPLICABLE {
            /// <summary>順序連番</summary>
            public static readonly GridViewDefine JUN_NO = new GridViewDefine( "投入順序連番", "JUN_NO", typeof( string ), "", true, HorizontalAlign.Center, 140, true );
            /// <summary>型式コード</summary>
            public static readonly GridViewDefine MODEL_CD = new GridViewDefine( "型式コード", "MODEL_CD", typeof( string ), "", true, HorizontalAlign.Center, 140, true );
            /// <summary>型式名</summary>
            public static readonly GridViewDefine MODEL_NM = new GridViewDefine( "型式名", "MODEL_NM", typeof( string ), "", true, HorizontalAlign.Left, 200, true );
            /// <summary>機番</summary>
            public static readonly GridViewDefine SERIAL_NO = new GridViewDefine( "機番", "SERIAL_NO", typeof( string ), "", true, HorizontalAlign.Center, 100, true );
            /// <summary>6桁機番</summary>
            public static readonly GridViewDefine SERIAL6 = new GridViewDefine( "6桁機番", "SERIAL6", typeof( string ), "", true, HorizontalAlign.Center, 0, false );
            /// <summary>完成予定日</summary>
            public static readonly GridViewDefine KAN_YO_YM = new GridViewDefine( "完成予定日", "KAN_YO_YM", typeof( string ), "", true, HorizontalAlign.Center, 140, true );
            /// <summary>チェック対象外</summary>
            public static readonly GridViewDefine PARTS_OPE = new GridViewDefine( "チェック対象外", "PARTS_OPE", typeof( string ), "", true, HorizontalAlign.Center, 160, true );
            /// <summary>詳細</summary>
            public static readonly GridViewDefine DTL = new GridViewDefine( "詳細", "DTL", typeof( string ), "", true, HorizontalAlign.Center, 250, true );
            /// <summary>備考</summary>
            public static readonly GridViewDefine NOTES = new GridViewDefine( "備考", "NOTES", typeof( string ), "", true, HorizontalAlign.Left, 400, true );
            /// <summary>登録日</summary>
            public static readonly GridViewDefine INS_DT = new GridViewDefine( "登録日", "INS_DT", typeof( string ), "", true, HorizontalAlign.Center, 160, true );
            /// <summary>登録者</summary>
            public static readonly GridViewDefine INS_BY = new GridViewDefine( "登録者", "INS_BY", typeof( string ), "", true, HorizontalAlign.Center, 100, true );
            /// <summary>パターンコード</summary>
            public static readonly GridViewDefine PTN_CD = new GridViewDefine( "パターンコード", "PTN_CD", typeof( string ), "", true, HorizontalAlign.Center, 0, false );
        }
        /// <summary>
        /// 一覧表示情報コントロール(左下)
        /// </summary>
        public class GRID_SEARCH_CONTROLS_L {
        }
        /// <summary>チェックボックス</summary>

        /// <summary>
        /// 一覧表示情報コントロール(右下)
        /// </summary>
        public class GRID_SEARCH_CONTROLS_R {
        }

        /// <summary>
        /// 検索結果(更新時共通部)
        /// </summary>
        /// 
        internal class GRID_COMMON {
            /// <summary>順序連番</summary>
            public static readonly GridViewDefine JUN_NO = new GridViewDefine( "投入順序連番", "JUN_NO", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <summary>型式コード</summary>
            public static readonly GridViewDefine MODEL_CD = new GridViewDefine( "型式コード", "MODEL_CD", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <summary>型式名</summary>
            public static readonly GridViewDefine MODEL_NM = new GridViewDefine( "型式名", "MODEL_NM", typeof( string ), "", true, HorizontalAlign.Left, 200, true, true );
            /// <summary>機番</summary>
            public static readonly GridViewDefine SERIAL_NO = new GridViewDefine( "機番", "SERIAL_NO", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>6桁機番</summary>
            public static readonly GridViewDefine SERIAL6 = new GridViewDefine( "6桁機番", "SERIAL6", typeof( string ), "", true, HorizontalAlign.Center, 0, false, true );
            /// <summary>完成予定日</summary>
            public static readonly GridViewDefine KAN_YO_YM = new GridViewDefine( "完成予定日", "KAN_YO_YM", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
        }

        /// <summary>
        /// 投入順序
        /// </summary>
        internal class GRID_JUN_NO {
            /// <summary>ﾁｪｯｸ対象外登録</summary>
            public static readonly GridViewDefine DATA_CNT = new GridViewDefine( "ﾁｪｯｸ対象外登録", "DATA_CNT", typeof( string ), "", true, HorizontalAlign.Center, 160, true, true );
            /// <summary>パターンコード</summary>
            public static readonly GridViewDefine PTN_CD = new GridViewDefine( "パターンコード", "PTN_CD", typeof( string ), "", true, HorizontalAlign.Center, 0, false, true );
        }

        /// <summary>
        /// 登録済
        /// </summary>
        internal class GRID_REGISTERD {
            /// <summary>チェック対象外</summary>
            public static readonly GridViewDefine PARTS_OPE = new GridViewDefine( "チェック対象外", "PARTS_OPE", typeof( string ), "", true, HorizontalAlign.Center, 160, true, true );
            /// <summary>詳細</summary>
            public static readonly GridViewDefine DTL = new GridViewDefine( "詳細", "DTL", typeof( string ), "", true, HorizontalAlign.Center, 250, true, true );
            /// <summary>備考</summary>
            public static readonly GridViewDefine NOTES = new GridViewDefine( "備考", "NOTES", typeof( string ), "", true, HorizontalAlign.Left, 400, true, true );
            /// <summary>登録日</summary>
            public static readonly GridViewDefine INS_DT = new GridViewDefine( "登録日", "INS_DT", typeof( string ), "", true, HorizontalAlign.Center, 160, true, true );
            /// <summary>登録者</summary>
            public static readonly GridViewDefine INS_BY = new GridViewDefine( "登録者", "INS_BY", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>パターンコード</summary>
            public static readonly GridViewDefine PTN_CD = new GridViewDefine( "パターンコード", "PTN_CD", typeof( string ), "", true, HorizontalAlign.Center, 0, false, true );
        }
        #endregion

        #region メンバ変数
        public static GridViewDefine[] GridViewResults = null;
        #endregion

        #region イベント

        /// <summary>
        /// ページロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load( object sender, EventArgs e ) {
            base.RaiseEvent( DoPageLoad, false );
        }

        #endregion

        #region イベントメソッド

        #region ページイベント

        /// <summary>
        /// ページロード処理
        /// </summary>
        protected override void DoPageLoad() {
            //ベース ページロード処理
            base.DoPageLoad();

            //動的処理
            GridView frozenGrid = grvMainViewLB;
            ControlUtils.SetGridViewTemplateField( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, gridviewDefault, true ) );
            ControlUtils.SetGridViewTemplateField( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, gridviewDefault, false ) );

            ControlUtils.ReMakeBoundTemplateFieldCtrl( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, gridviewDefault, true ) );
            ControlUtils.ReMakeBoundTemplateFieldCtrl( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, gridviewDefault, false ) );

            int resultCnt = 0;
            if ( ObjectUtils.IsNotNull( ConditionInfo.ResultData ) ) {
                resultCnt = ConditionInfo.ResultData.Rows.Count;
            }

            ControlUtils.SetGridViewPager( ref pnlPager, grvMainViewRB, grvMainView_PageIndexChanging, resultCnt, grvMainViewRB.PageIndex );
        }

        #endregion

        #endregion

        #region メソッド

        #region 画面初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        protected override void Initialize() {

            //アクセスカウンター登録
            Dao.Com.AccessCounterDao.Entry( base.CurrentPageInfo.pageId );

            //ベース処理初期化処理
            base.Initialize();

            //初期化、初期値設定
            InitializeValues();

        }
        /// <summary>
        /// 初期化、初期値設定
        /// </summary>
        private void InitializeValues() {

            //■固定リスト項目
            //製品種別リスト
            SetProductKindList();

            //検索区分
            CreateSearchKbn();

            //■初期値設定
            //製品種別 10:エンジン
            rblProductKind.SelectedValue = ProductKind.Engine;
            rblSearchKbn.SelectedValue = CONST_ORDER_NO;

            //当月月初
            cldCompStart.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, 1 );
            //当月月末
            cldCompEnd.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth( DateTime.Today.Year, DateTime.Today.Month ) );

            //登録日
            ChangeSearchKbn();

            ConditionInfo = new ConditionInfoSessionHandler.ST_CONDITION();

            //グリッドビュー初期化
            ClearGridView();

        }
        /// <summary>
        /// 検索区分の設定
        /// </summary>
        private void CreateSearchKbn() {

            ListItem[] liArr = new ListItem[2];
            liArr[0] = new ListItem( CONST_IN_PRODUCT, CONST_ORDER_NO );
            liArr[1] = new ListItem( CONST_NOT_APPLICABLE, CONST_NA_LIST );

            ControlUtils.SetListControlItems( rblSearchKbn, liArr );

       
        }
        /// <summary>
        /// 製品種別設定
        /// </summary>
        private void SetProductKindList() {

            //共通処理で取得
            ControlUtils.SetListControlItems( rblProductKind, Dao.Com.MasterList.ProductKindList );

            //その他(PRODUCT_KIND_CD=76)をリストから削除する
            rblProductKind.Items.RemoveAt( 2 );

        }
        #endregion
        #region グリッドビュー

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

            //件数表示
            ntbResultCount.Value = 0;

            //ページャークリア
            ControlUtils.ClearPager( ref pnlPager );

            //GridView非表示
            divGrvDisplay.Visible = false;

        }

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
        #endregion

        #endregion


        #region イベント
        /// <summary>
        /// 検索ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click( object sender, EventArgs e ) {
            base.RaiseEvent( DoSearch );
        }


        #region グリッドビューイベント

        /// <summary>
        /// グリッドビュー行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMainView_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainView( sender, e );
        }
        /// <summary>
        /// グリッドビューページチェンジイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMainView_PageIndexChanging( object sender, CommandEventArgs e ) {
            base.RaiseEvent( PageIndexChangingMainView, sender, e );
        }
        /// <summary>
        /// グリッドビュー並び替えイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMainView_Sorting( object sender, GridViewSortEventArgs e ) {
            base.RaiseEvent( SortingMainView, sender, e );
        }

        /// <summary>
        /// グリッドビュー行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowDataBoundMainView( params object[] parameters ) {

            object sender = parameters[0];
            GridViewRowEventArgs e = (GridViewRowEventArgs)parameters[1];
            if ( e.Row.RowType == DataControlRowType.DataRow ) {

                //エンジン、トラクタのみ詳細画面表示を有効とする
//                ControlUtils.GridViewRowBound( (GridView)sender, e, ControlUtils.GridRowDoubleClickEvent.None );

                ControlUtils.GridViewRowBound( (GridView)sender, e, ControlUtils.GridRowDoubleClickEvent.WindowOpen, PageInfo.ResolveClientUrl( this, PageInfo.MaintenanceNAListDetail ), base.Token );
            }
        }

        /// <summary>
        /// グリッドビューページ切替
        /// </summary>
        /// <param name="parameters"></param>
        private void PageIndexChangingMainView( params object[] parameters ) {
            object sender = parameters[0];

            CommandEventArgs e = (CommandEventArgs)parameters[1];
            int newPageIndex = Convert.ToInt32( e.CommandArgument );

            int pageSize = grvMainViewRB.PageSize;
            int rowCount = 0;
            if ( true == ObjectUtils.IsNotNull( ConditionInfo.ResultData ) ) {
                rowCount = ConditionInfo.ResultData.Rows.Count;
            }
            int allPages = 0;
            allPages = ConditionInfo.ResultData.Rows.Count / pageSize;
            if ( 0 != rowCount % pageSize ) {
                allPages += 1;
            }
            //ページが無くなっている場合には、先頭ページへ戻す
            if ( newPageIndex >= allPages ) {
                newPageIndex = 0;
            }

            //背面ユーザ切替対応
            ConditionInfoSessionHandler.ST_CONDITION cond = ConditionInfo;
            GridView frozenGrid = grvMainViewLB;
            ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, true ), cond, true );
            ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, false ), cond, true );

            ControlUtils.BindGridView_WithTempField( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, true ), ConditionInfo.ResultData );
            ControlUtils.BindGridView_WithTempField( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, false ), ConditionInfo.ResultData );

            ControlUtils.GridViewPageIndexChanging( grvMainViewLB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
            ControlUtils.GridViewPageIndexChanging( grvMainViewRB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );

            ControlUtils.SetGridViewPager( ref pnlPager, grvMainViewRB, grvMainView_PageIndexChanging, ConditionInfo.ResultData.Rows.Count, grvMainViewRB.PageIndex );

            //グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();
        }

        /// <summary>
        /// グリッドビュー並び替え
        /// </summary>
        /// <param name="parameters"></param>
        private void SortingMainView( params object[] parameters ) {
            object sender = parameters[0];
            GridViewSortEventArgs e = (GridViewSortEventArgs)parameters[1];

            ConditionInfoSessionHandler.ST_CONDITION cond = ConditionInfo;
            ControlUtils.GridViewSorting( grvMainViewLB, ref cond, e, true );
            ControlUtils.GridViewSorting( grvMainViewRB, ref cond, e );

            //背面ユーザ切替対応
            GridView frozenGrid = grvMainViewLB;
            ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, true ), cond, true );
            ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, false ), cond, true );

            ControlUtils.BindGridView_WithTempField( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, true ), cond.ResultData );
            ControlUtils.BindGridView_WithTempField( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, false ), cond.ResultData );

            ControlUtils.SetGridViewPager( ref pnlPager, grvMainViewRB, grvMainView_PageIndexChanging, cond.ResultData.Rows.Count, grvMainViewRB.PageIndex );

            ConditionInfo = cond;

            //グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();
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
                base.SetControlValues( e.Row, ControlUtils.GetControlDefineArray( typeof( GRID_SEARCH_CONTROLS_L ) ), row, ref dicControls );

            }
            ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_MAIN_VIEW_GROUP_CD, ControlUtils.GridRowDoubleClickEvent.WindowOpen, PageInfo.ResolveClientUrl( this, PageInfo.MaintenanceNAListDetail ), base.Token );

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

            ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_MAIN_VIEW_GROUP_CD, ControlUtils.GridRowDoubleClickEvent.WindowOpen, PageInfo.ResolveClientUrl( this, PageInfo.MaintenanceNAListDetail ), base.Token );

        }
        #endregion

        #endregion

        #region 検索処理

        /// <summary>
        /// 検索処理
        /// </summary>
        private void DoSearch() {

            //検索パラメータ作成
            Dictionary<string, object> dicCondition = new Dictionary<string, object>();

            base.GetControlValues( ConditionControls, ref dicCondition );

            //検索時画面情報取得
            Dictionary<string, string> dicIdWithText = new Dictionary<string, string>();
            base.GetControlTexts( ConditionControls, out dicIdWithText );

            //一覧表示列の設定
            GridViewDefine[] gridColumns;
            List<GridViewDefine> columns = new List<GridViewDefine>();
            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_COMMON ) ) );

            if ( rblSearchKbn.SelectedValue.Equals( CONST_ORDER_NO ) ) {
                //生産中が選択された場合
                columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_JUN_NO ) ) );

            } else {
                //登済済が選択された場合
                columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_REGISTERD ) ) );
            }

            gridColumns = columns.ToArray();
            GridViewResults = gridColumns; 

            //検索結果取得
            //エラー時にも最後まで処理を通すため、インスタンスを生成しておく
            MainViewBusiness.ResultSet result = new MainViewBusiness.ResultSet();
            DataTable tblResult = null;
            int maxGridViewCount = WebAppInstance.GetInstance().Config.WebCommonInfo.maxGridViewCount;  //検索上限数
            try {
                if ( rblSearchKbn.SelectedValue.Equals( CONST_ORDER_NO ) ) {
                    //生産中データ取得
                    result.ListTable = EngineProcessDao.SelectInProduct( dicCondition, maxGridViewCount );
                }else{
                    //チェック対象外データ取得
                    result.ListTable = EngineProcessDao.SelectNotApplicable( dicCondition, maxGridViewCount );
                }

            } catch ( DataAccessException ex ) {
                if ( ex.OracleErrorNumber == DataAccessBase.ORAERROR_TIMEOUT ) {
                    //クエリ発行タイムアウト
                    base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_61910 );
                } else {
                    //タイムアウト以外のException
                    logger.Exception( ex );
                    base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                }
            } catch ( Exception ex ) {
                logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
            } finally {
                //メッセージ設定
                result.Message = null;
                if ( null == result.ListTable || 0 == result.ListTable.Rows.Count ) {
                    //検索結果0件
                    result.Message = new Msg( MsgManager.MESSAGE_WRN_61010 );

                } else if ( ( null != result.ListTable && maxGridViewCount <= result.ListTable.Rows.Count ) ) {
                    //検索件数が上限を上回っている場合には警告メッセージをセット
                    result.Message = new Msg( MsgManager.MESSAGE_WRN_61020 );
                }

            }
            ConditionInfoSessionHandler.ST_CONDITION cond = new ConditionInfoSessionHandler.ST_CONDITION();
            tblResult = result.ListTable;
            if ( null != tblResult ) {
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

                    //TemplateFieldの再作成
                    grvHeaderRT.Columns.Clear();
                    grvMainViewRB.Columns.Clear();
                    for ( int idx = frozenGrid.Columns.Count; idx < GridViewResults.Length; idx++ ) {
                        TemplateField tf = new TemplateField();
                        tf.HeaderText = StringUtils.ToString( GridViewResults[idx].bindField );
                        grvMainViewRB.Columns.Add( tf );
                    }

                    //新規バインド
                    ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, true ), ConditionInfo, true );
                    ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, false ), ConditionInfo, true );
                    ControlUtils.BindGridView_WithTempField( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, true ), tblResult );
                    ControlUtils.BindGridView_WithTempField( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, false ), tblResult );

                    //ページャー作成
                    ControlUtils.SetGridViewPager( ref pnlPager, grvMainViewLB, grvMainView_PageIndexChanging, cond.ResultData.Rows.Count, grvMainViewLB.PageIndex );

                    //GridView表示
                    divGrvDisplay.Visible = true;

                    //グリッドビュー外のDivサイズ変更
                    SetDivGridViewWidth();
                } else {
                    ClearGridView();
                }
            }

            //メッセージ表示
            if ( null != result.Message ) {
                base.WriteApplicationMessage( result.Message );
            }

            return;
        }
        #endregion


        #region リスト選択処理
        /// <summary>
        /// 検索区分変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblSearchKbn_SelectedIndexChanged( object sender, EventArgs e ) {
            base.RaiseEvent( ChangeSearchKbn );
        }
        /// <summary>
        /// 検索区分変更関連項目制御
        /// </summary>
        private void ChangeSearchKbn() {

            //投入順序の場合は登録日使用不可
            if ( rblSearchKbn.SelectedValue.Equals( CONST_ORDER_NO ) ) {
                cldStart.Value = null;
                cldEnd.Value = null;
                cldStart.Enabled = false;
                cldEnd.Enabled = false;
            } else {
                cldStart.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, 1 );
                cldEnd.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth( DateTime.Today.Year, DateTime.Today.Month ) );
                cldStart.Enabled = true;
                cldEnd.Enabled = true;
            }

        }

        #endregion

        protected void grvMainViewLB_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainViewLB( sender, e );
        }

        protected void grvMainViewRB_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainViewRB( sender, e );
        }

    }
}