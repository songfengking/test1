using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KTFramework.C1Common.Excel;
using KTFramework.Common;
using KTFramework.Dao;
using TRC_W_PWT_ProductView.Business;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Dao.Com;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.ListDefine;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.Session;
using TRC_W_PWT_ProductView.UI.Base;

namespace TRC_W_PWT_ProductView.UI.Pages.Kanban {
    public partial class KanbanPickingStatusView : BaseForm {
        #region 定数定義
        /// <summary>
        /// 検索条件
        /// </summary>
        public class CONDITION {
            /// <summary>
            /// 要求日付From
            /// </summary>
            public static readonly ControlDefine SEND_DATE_FROM = new ControlDefine( "cldSendDateFrom", "要求日付From", "cldSendDateFrom", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 要求時刻From
            /// </summary>
            public static readonly ControlDefine SEND_TIME_FROM = new ControlDefine( "ttbSendTimeFrom", "要求時刻From", "ttbSendTimeFrom", ControlDefine.BindType.Both, typeof( TimeSpan ) );
            /// <summary>
            /// 要求日付To
            /// </summary>
            public static readonly ControlDefine SEND_DATE_TO = new ControlDefine( "cldSendDateTo", "要求日付To", "cldSendDateTo", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 要求時刻To
            /// </summary>
            public static readonly ControlDefine SEND_TIME_TO = new ControlDefine( "ttbSendTimeTo", "要求時刻To", "ttbSendTimeTo", ControlDefine.BindType.Both, typeof( TimeSpan ) );
            /// <summary>
            /// 完了日付From
            /// </summary>
            public static readonly ControlDefine END_DATE_FROM = new ControlDefine( "cldEndDateFrom", "完了日付From", "cldEndDateFrom", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 完了時刻From
            /// </summary>
            public static readonly ControlDefine END_TIME_FROM = new ControlDefine( "ttbEndTimeFrom", "完了時刻From", "ttbEndTimeFrom", ControlDefine.BindType.Both, typeof( TimeSpan ) );
            /// <summary>
            /// 完了日付To
            /// </summary>
            public static readonly ControlDefine END_DATE_TO = new ControlDefine( "cldEndDateTo", "完了日付To", "cldEndDateTo", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 完了時刻To
            /// </summary>
            public static readonly ControlDefine END_TIME_TO = new ControlDefine( "ttbEndTimeTo", "完了時刻To", "ttbEndTimeTo", ControlDefine.BindType.Both, typeof( TimeSpan ) );
            /// <summary>
            /// 状況
            /// </summary>
            public static readonly ControlDefine STATUS = new ControlDefine( "ddlStatus", "状況", "ddlStatus", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// エリア
            /// </summary>
            public static readonly ControlDefine AREA_NM = new ControlDefine( "ddlArea", "エリア", "ddlArea", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// ピッキング者ID
            /// </summary>
            public static readonly ControlDefine PICKING_USER_ID = new ControlDefine( "hdnPickingUserId", "ピッキング者", "hdnPickingUserId", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// ピッキング者名
            /// </summary>
            public static readonly ControlDefine PICKING_USER_NM = new ControlDefine( "hdnPickingUserNm", "ピッキング者", "hdnPickingUserNm", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// ピッキングNo
            /// </summary>
            public static readonly ControlDefine PICKING_LIST_NO = new ControlDefine( "txtPickingNo", "ピッキングNo", "txtPickingNo", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 品番
            /// </summary>
            public static readonly ControlDefine PARTS_NUMBER = new ControlDefine( "txtPartsNumber", "品番", "txtPartsNumber", ControlDefine.BindType.Both, typeof( string ) );
        }

        /// <summary>
        /// ピッキング状況一覧定義
        /// </summary>
        internal class GRID_PICKING_INFO {
            /// <summary>ピッキングNo</summary>
            public static readonly GridViewDefine PICKING_LIST_NO = new GridViewDefine( "ピッキングNo", "PICKING_LIST_NO", typeof( string ), "", true, HorizontalAlign.Center, 150, true, true );
            /// <summary>要求時刻</summary>
            public static readonly GridViewDefine SEND_TIME = new GridViewDefine( "要求時刻", "SEND_TIME", typeof( string ), "", true, HorizontalAlign.Center, 200, true, true );
            /// <summary>完了時刻</summary>
            public static readonly GridViewDefine END_TIME = new GridViewDefine( "完了時刻", "END_TIME", typeof( string ), "", true, HorizontalAlign.Center, 200, true, true );
            // 2023/03/13 星 追加開始
            public static readonly GridViewDefine DELIVERY_INSTRUCTION_DATE = new GridViewDefine( "納入指示日", "DELIVERY_INSTRUCTION_DATE", typeof( string ), "", true, HorizontalAlign.Center, 200, false, true );
            // 2023/03/13 星 追加終了
            /// <summary>エリア</summary>
            public static readonly GridViewDefine AREA_NM = new GridViewDefine( "エリア", "AREA_NM", typeof( string ), "", true, HorizontalAlign.Left, 250, true, true );
            /// <summary>状況</summary>
            public static readonly GridViewDefine STATUS = new GridViewDefine( "状況", "STATUS", typeof( string ), "", true, HorizontalAlign.Left, 120, true, true );
            /// <summary>ピッキング者</summary>
            public static readonly GridViewDefine PICKING_USER = new GridViewDefine( "ピッキング者", "PICKING_USER", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true ); 
        }

        /// <summary>
        /// 状況名称・コード
        /// </summary>
        private const string StatusComp = "完了";
        private const string StatusNotComp = "完了(未完あり)";
        private const string StatusTouched = "着手中";
        private const string StatusUnTouched = "未着手";
        private const string StatusIdComp = "9";
        private const string StatusIdNotComp = "8";
        private const string StatusIdTouched = "2";
        private const string StatusIdUnTouched0 = "0";
        private const string StatusIdUnTouched1 = "1";
        #endregion

        #region プロパティ
        /// <summary>
        /// ロガー
        /// </summary>
        private static readonly Logger _logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        /// <summary>
        /// 検索条件定義情報
        /// </summary>
        ControlDefine[] _conditionControls = null;
        /// <summary>
        /// 検索条件定義情報アクセサ
        /// </summary>
        ControlDefine[] ConditionControls
        {
            get
            {
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
        GridViewDefine[] gridviewDefault
        {
            get
            {
                if ( true == ObjectUtils.IsNull( _gridviewDefault ) ) {
                    _gridviewDefault = ControlUtils.GetGridViewDefineArray( typeof( GRID_PICKING_INFO ) );
                }
                return _gridviewDefault;
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
            RaiseEvent( DoPageLoad );
        }

        /// <summary>
        /// 検索ボタン選択動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click( object sender, EventArgs e ) {
            base.RaiseEvent( DoSearch );
        }

        /// <summary>
        /// Excel出力ボタン選択動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcel_Click( object sender, EventArgs e ) {
            base.RaiseEvent( DoExcelOutput );
        }

        /// <summary>
        /// GridView行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvPickingView_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainView( sender, e );
        }

        /// <summary>
        /// GridViewページチェンジ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvPickingView_PageIndexChanging( object sender, CommandEventArgs e ) {
            base.RaiseEvent( PageIndexChangingMainView, sender, e );
        }

        /// <summary>
        /// GridView並び替え
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvPickingView_Sorting( object sender, GridViewSortEventArgs e ) {
            base.RaiseEvent( SortingMainView, sender, e );
        }
        #endregion

        #region イベントメソッド
        /// <summary>
        /// ページロード処理
        /// </summary>
        protected override void DoPageLoad() {
            // ベースページロード処理
            base.DoPageLoad();

            // 検索結果件数取得
            int resultCnt = 0;
            if ( ObjectUtils.IsNotNull( ConditionInfo.ResultData ) ) {
                resultCnt = ConditionInfo.ResultData.Rows.Count;
            }

            // 検索結果件数に応じてページャー設定
            ControlUtils.SetGridViewPager( ref pnlPager, grvPickingBody, grvPickingView_PageIndexChanging, resultCnt, grvPickingBody.PageIndex );
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        protected override void Initialize() {
            // アクセスカウンター登録
            Dao.Com.AccessCounterDao.Entry( base.CurrentPageInfo.pageId );

            // ベースの初期化処理
            base.Initialize();
            // セッションをクリアする
            ConditionInfo = new ConditionInfoSessionHandler.ST_CONDITION();

            // 初期処理
            InitializeValues();
        }

        /// <summary>
        /// 初期処理
        /// </summary>
        private void InitializeValues() {
            try {
                // デフォルト値設定
                // 完了日のFromにシステム日付設定
                String systemDate = DateUtils.ToString( DateTime.Now, DateUtils.DATE_FORMAT_DAY );
                DateTime? defaultDate = DateUtils.ToDateNullable( systemDate, DateUtils.DATE_FORMAT_DAY );
                cldSendDateFrom.Value = defaultDate;

                // 状況のリストに値をセット
                ddlStatus.Items.Add( String.Empty );
                ddlStatus.Items.Add( StatusComp );
                ddlStatus.Items.Add( StatusNotComp );
                ddlStatus.Items.Add( StatusTouched );
                ddlStatus.Items.Add( StatusUnTouched );

                // エリア一覧情報を検索条件：エリアのリストに設定する
                ControlUtils.SetListControlItems( ddlArea, MasterList.AreaNameList );
            } catch ( Exception ex ) {
                // 例外ログ、メッセージ表示を実行する
                _logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_84010 );
            }
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        private void DoSearch() {
            // 検索条件を取得
            Dictionary<string, object> dicCondition = new Dictionary<string, object>();
            base.GetControlValues( ConditionControls, ref dicCondition );

            // 検索時画面情報を取得
            Dictionary<string, string> dicIdWithText = new Dictionary<string, string>();
            base.GetControlTexts( ConditionControls, out dicIdWithText );

            // 検索結果取得用変数定義
            // エラー時にも最後まで処理を通すため、インスタンスを生成しておく
            var result = new KanbanBusiness.ResultSet();
            DataTable tblResult = null;

            // 表示上限件数を取得する
            int maxGridViewCount = WebAppInstance.GetInstance().Config.WebCommonInfo.maxGridViewCount;

            //---検索実行---
            try {

                // ピッキング状況画面：検索処理を実行する
                result = KanbanBusiness.SearchPickingStatus( dicCondition, gridviewDefault, maxGridViewCount );

            } catch ( DataAccessException ex ) {
                if ( ex.OracleErrorNumber == DataAccessBase.ORAERROR_TIMEOUT ) {
                    // クエリ発行タイムアウトが発生した場合、エラーメッセージを設定する
                    base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_61910 );
                } else {
                    // タイムアウト以外のExceptionが発生した場合、エラーメッセージを設定する
                    _logger.Exception( ex );
                    base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                }
            } catch ( Exception ex ) {
                _logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_84010, "" );
            }

            // 2023/03/14 星 追加開始
            //品番検索時
            if( txtPartsNumber.Text != "" ) {
                //納入指示日列を追加
                GRID_PICKING_INFO.DELIVERY_INSTRUCTION_DATE.visible = true;
            } else {
                GRID_PICKING_INFO.DELIVERY_INSTRUCTION_DATE.visible = false;
            }

            // 2023/03/14 星 追加終了

            //---検索結果判定---
            // 処理結果が取得できた時のみ件数・ページャーの設定を実施
            // 取得できなかった時は一覧を初期化
            ConditionInfoSessionHandler.ST_CONDITION cond = new ConditionInfoSessionHandler.ST_CONDITION();
            tblResult = result.ListTable;
            if ( ( null != tblResult ) && ( maxGridViewCount >= tblResult.Rows.Count ) ) {
                // ピッキング状況件数を表示
                ntbResultCount.Value = tblResult.Rows.Count;
                // ピッキング状況件数に応じてページャー設定
                ControlUtils.SetGridViewPager( ref pnlPager, grvPickingBody, grvPickingView_PageIndexChanging, tblResult.Rows.Count, 0 );
                // 検索条件/結果インスタンスを保持する
                cond.conditionValue = dicCondition;
                cond.IdWithText = dicIdWithText;
                cond.ResultData = tblResult.DefaultView.ToTable();
            } else {
                // タイムアウト等Exception時には、GridView初期化
                ClearGridView();
            }

            // 検索条件をセッションに格納する
            ConditionInfo = cond;

            //---グリッドビューの表示処理---
            // 処理結果が取得できた時のみ一覧の設定を実施
            // 取得できなかった時は一覧を初期化
            GridView frozenGrid = grvPickingBody;
            if ( true == ObjectUtils.IsNotNull( tblResult ) ) {
                if ( ( 0 < tblResult.Rows.Count ) && ( maxGridViewCount >= tblResult.Rows.Count ) ) {
                    // TemplateFieldの追加
                    grvPickingHeader.Columns.Clear();
                    grvPickingBody.Columns.Clear();
                    for ( int idx = frozenGrid.Columns.Count; idx < gridviewDefault.Length; idx++ ) {
                        TemplateField tf = new TemplateField();
                        tf.HeaderText = StringUtils.ToString( gridviewDefault[idx].bindField );
                        grvPickingBody.Columns.Add( tf );
                    }
                    // 新規バインド
                    ControlUtils.ShowGridViewHeader( grvPickingHeader, ControlUtils.GetFrozenColumns( frozenGrid, gridviewDefault, true ), ConditionInfo, true );
                    ControlUtils.BindGridView_WithTempField( grvPickingBody, ControlUtils.GetFrozenColumns( frozenGrid, gridviewDefault, true ), tblResult );
                    // GridView表示
                    divGrvDisplay.Visible = true;
                } else {
                    // GridView初期化
                    ClearGridView();
                }
            }
            //メッセージ表示
            if ( null != result.Message ) {
                // エラーメッセージが存在する場合、メッセージ表示
                base.WriteApplicationMessage( result.Message );
            }
            //Excel出力ボタン活性
            if ( ( null != tblResult ) && ( 0 < tblResult.Rows.Count ) && ( maxGridViewCount >= tblResult.Rows.Count ) ) {
                // 出力対象データありの場合、Excel出力ボタンを有効化する
                this.btnExcel.Visible = true;
            } else {
                //出力対象データなしの場合、Excel出力ボタンを無効化する
                this.btnExcel.Visible = false;
            }

        }
        #endregion

        #region グリッドビューイベント
        /// <summary>
        /// 一覧初期化
        /// </summary>
        private void ClearGridView() {
            // 一覧ヘッダー部初期化
            ControlUtils.InitializeGridView( grvPickingHeader, false );
            // 一覧ボディ部初期化
            ControlUtils.InitializeGridView( grvPickingBody, false );
            // 件数表示に0を設定
            ntbResultCount.Value = 0;
            // ページャー初期化
            ControlUtils.ClearPager( ref pnlPager );
            // GridView非表示
            divGrvDisplay.Visible = false;
        }

        /// <summary>
        /// 一覧行データバインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowDataBoundMainView( params object[] parameters ) {
            //object sender = parameters[0];
            //GridViewRowEventArgs e = (GridViewRowEventArgs)parameters[1];
            var sender = (GridView)parameters[0];
            var e = (GridViewRowEventArgs)parameters[1];

            // データ行の一部の列にのみフォーマットをかけて表示する
            if ( e.Row.RowType == DataControlRowType.DataRow ) {

                int index = 0;
                string workCellText;
                DateTime? workCellDatetime;

                // 一覧の要求時刻の表示をフォーマットする
                if ( GetColumnIndex( sender, GRID_PICKING_INFO.SEND_TIME, out index ) == true ) {
                    var date = new DateTime();
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_PICKING_INFO.SEND_TIME.bindField].ToString();
                    e.Row.Cells[index].Text = ( DateTime.TryParse( data, out date ) ) ? date.ToString( DateUtils.DATE_FORMAT_SECOND ) : string.Empty;
                }

                // 一覧の完了時刻の表示をフォーマットする
                if ( GetColumnIndex( sender, GRID_PICKING_INFO.END_TIME, out index ) == true ) {
                    var date = new DateTime();
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_PICKING_INFO.END_TIME.bindField].ToString();
                    e.Row.Cells[index].Text = ( DateTime.TryParse( data, out date ) ) ? date.ToString( DateUtils.DATE_FORMAT_SECOND ) : string.Empty;
                }

                // 詳細画面表示を有効にする
                string keyPickingno = StringUtils.ToString( ( (DataRowView)e.Row.DataItem )[GRID_PICKING_INFO.PICKING_LIST_NO.bindField] );
                ControlUtils.GridViewRowBound( (GridView)sender, e, ControlUtils.GridRowDoubleClickEvent.WindowOpen, PageInfo.ResolveClientUrl( this, PageInfo.KanbanPickingDetailView ), base.Token, keyPickingno );
            }
        }

        /// <summary>
        /// GridViewカラムインデックス取得
        /// </summary>
        /// <param name="target"></param>
        /// <param name="def"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool GetColumnIndex( GridView target, GridViewDefine def, out int index ) {
            index = 0;
            foreach ( DataControlField c in target.Columns ) {
                if ( c.HeaderText == def.headerText ) {
                    return true;
                }
                index++;
            }
            return false;
        }

        /// <summary>
        /// 一覧ページ切替
        /// </summary>
        /// <param name="parameters"></param>
        private void PageIndexChangingMainView( params object[] parameters ) {
            object sender = parameters[0];
            // 新しいページNOに応じたデータ表示に切り替える

            // 新しいページNO取得
            CommandEventArgs e = (CommandEventArgs)parameters[1];
            int newPageIndex = Convert.ToInt32( e.CommandArgument );

            // 全体のページ数取得
            int pageSize = grvPickingBody.PageSize;
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
            if ( newPageIndex >= allPages ) {
                newPageIndex = 0;
            }

            ConditionInfoSessionHandler.ST_CONDITION cond = ConditionInfo;
            GridView frozenGrid = grvPickingBody;

            // GridViewのヘッダー表示
            ControlUtils.ShowGridViewHeader( grvPickingHeader, ControlUtils.GetFrozenColumns( frozenGrid, gridviewDefault, true ), cond, true );

            // GridViewにデータ表示
            ControlUtils.BindGridView_WithTempField( grvPickingBody, ControlUtils.GetFrozenColumns( frozenGrid, gridviewDefault, true ), ConditionInfo.ResultData );

            // GridViewのページ切替
            ControlUtils.GridViewPageIndexChanging( grvPickingBody, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );

            // GridViewのページャーを設定
            ControlUtils.SetGridViewPager( ref pnlPager, grvPickingBody, grvPickingView_PageIndexChanging, ConditionInfo.ResultData.Rows.Count, grvPickingBody.PageIndex );

            // GridView外のDivサイズ調整
            SetDivGridViewWidth();
        }

        /// <summary>
        /// 一覧ページ並び替え
        /// </summary>
        /// <param name="parameters"></param>
        private void SortingMainView( params object[] parameters ) {
            object sender = parameters[0];
            GridViewSortEventArgs e = (GridViewSortEventArgs)parameters[1];
            // GridViewの内容をソートした後、セッションにソート後の結果を格納する

            // GridViewのソート
            ConditionInfoSessionHandler.ST_CONDITION cond = ConditionInfo;
            ControlUtils.GridViewSorting( grvPickingBody, ref cond, e );

            GridView frozenGrid = grvPickingBody;

            // GridViewのヘッダー表示
            ControlUtils.ShowGridViewHeader( grvPickingHeader, ControlUtils.GetFrozenColumns( frozenGrid, gridviewDefault, true ), cond, true );

            // GridViewにデータ表示
            ControlUtils.BindGridView_WithTempField( grvPickingBody, ControlUtils.GetFrozenColumns( frozenGrid, gridviewDefault, true ), cond.ResultData );

            // GridViewのページャーを設定
            ControlUtils.SetGridViewPager( ref pnlPager, grvPickingBody, grvPickingView_PageIndexChanging, cond.ResultData.Rows.Count, grvPickingBody.PageIndex );

            ConditionInfo = cond;

            // GridView外のDivサイズ変更
            SetDivGridViewWidth();
        }


        /// <summary>
        /// GridView格納DIVサイズ調整
        /// </summary>
        private void SetDivGridViewWidth() {
            SetDivGridViewWidth( grvPickingHeader, divGrvHeaderLT );

            SetDivGridViewWidth( grvPickingBody, divGrvLB );
        }

        /// <summary>
        /// GridView外のDivサイズ変更
        /// </summary>
        private void SetDivGridViewWidth( GridView grv, System.Web.UI.HtmlControls.HtmlGenericControl div ) {

            // セル幅補正 Padding4px + Border(片側) 1px
            const int CELL_PADDING = 4 * 2 + 1;
            // テーブル幅補正 Border(片側) 1px
            const int OUT_BORDER = 1;

            double sumWidth = 0;
            int showColCnt = 0;

            // GridView周りのdivサイズの微調整を行う
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

        #region Excel出力処理
        /// <summary>
        /// Excel出力処理
        /// </summary>
        private void DoExcelOutput() {
            try {
                // セッションから検索結果の取得
                ConditionInfoSessionHandler.ST_CONDITION cond = base.ConditionInfo;
                if ( null == cond.ResultData || 0 == cond.ResultData.Rows.Count ) {
                    // 出力対象データなし
                    return;
                }

                // 検索条件出力データ作成
                List<ExcelConditionItem> excelCond = new List<ExcelConditionItem>();
                string condition = "";
                string value = "";

                // 要求日(From)
                condition = CONDITION.SEND_DATE_FROM.displayNm;
                value = cond.IdWithText[CONDITION.SEND_DATE_FROM.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );
                // 要求時(From)
                condition = CONDITION.SEND_TIME_FROM.displayNm;
                value = cond.IdWithText[CONDITION.SEND_TIME_FROM.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );
                // 要求日(To)
                condition = CONDITION.SEND_DATE_TO.displayNm;
                value = cond.IdWithText[CONDITION.SEND_DATE_TO.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );
                // 要求時(To)
                condition = CONDITION.SEND_TIME_TO.displayNm;
                value = cond.IdWithText[CONDITION.SEND_TIME_TO.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );

                // 完了日(From)
                condition = CONDITION.END_DATE_FROM.displayNm;
                value = cond.IdWithText[CONDITION.END_DATE_FROM.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );
                // 完了時(From)
                condition = CONDITION.END_TIME_FROM.displayNm;
                value = cond.IdWithText[CONDITION.END_TIME_FROM.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );
                // 完了日(To)
                condition = CONDITION.END_DATE_TO.displayNm;
                value = cond.IdWithText[CONDITION.END_DATE_TO.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );
                // 完了時(To)
                condition = CONDITION.END_TIME_TO.displayNm;
                value = cond.IdWithText[CONDITION.END_TIME_TO.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );

                // 状況
                condition = CONDITION.STATUS.displayNm;
                value = cond.IdWithText[CONDITION.STATUS.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );

                // エリア
                condition = CONDITION.AREA_NM.displayNm;
                value = cond.IdWithText[CONDITION.AREA_NM.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );

                // ピッキング者
                condition = CONDITION.PICKING_USER_NM.displayNm;
                value = cond.IdWithText[CONDITION.PICKING_USER_NM.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );

                // ピッキングNo
                condition = CONDITION.PICKING_LIST_NO.displayNm;
                value = cond.IdWithText[CONDITION.PICKING_LIST_NO.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );

                // 品番
                condition = CONDITION.PARTS_NUMBER.displayNm;
                value = cond.IdWithText[CONDITION.PARTS_NUMBER.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );

                // Excelダウンロード実行(テーブルはExcel出力用に加工する)
                Excel.Download( Response, "ピッキング状況情報", GetExcelTable( cond.ResultData ), excelCond );
            } catch ( System.Threading.ThreadAbortException ) {
                // response.Endで必ず発生する為、正常として扱う
            } catch ( Exception ex ) {
                _logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80030, "ピッキング状況情報_検索結果" );
            }
        }

        /// <summary>
        /// Excel出力用データテーブル作成
        /// </summary>
        /// <param name="tblSource">検索結果</param>
        /// <returns>DataTable</returns>
        private DataTable GetExcelTable( DataTable tblSource ) {
            // Excel出力テーブル定義作成(一覧表示対象列を出力対象とする)
            DataTable tblResult = new DataTable();
            foreach ( DataColumn column in tblSource.Columns ) {
                if ( false == StringUtils.IsBlank( column.Caption ) ) {
                    DataColumn colResult = new DataColumn( column.Caption, column.DataType );
                    tblResult.Columns.Add( colResult );
                }
            }

            // 一覧元DataTableの情報をExcel出力用テーブルにコピー
            foreach ( DataRow rowSrc in tblSource.Rows ) {
                DataRow rowTo = tblResult.NewRow();
                foreach ( DataColumn column in tblSource.Columns ) {
                    if ( false == StringUtils.IsBlank( column.Caption ) ) {
                        rowTo[column.Caption] = rowSrc[column.ColumnName];
                    }
                }
                tblResult.Rows.Add( rowTo );
            }

            // 2023/03/22 星 追加開始
            foreach ( DataColumn column in tblSource.Columns ) {
                if ( txtPartsNumber.Text == "" && column.ToString() == "DELIVERY_INSTRUCTION_DATE" ) {
                    tblResult.Columns.RemoveAt( column.Ordinal );
                }
            }
            // 2023/03/22 星 追加終了

            tblResult.AcceptChanges();

            return tblResult;
        }
        #endregion

    }
}