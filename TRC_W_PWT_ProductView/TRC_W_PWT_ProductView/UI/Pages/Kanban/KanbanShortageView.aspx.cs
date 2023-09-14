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
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.Session;
using TRC_W_PWT_ProductView.UI.Base;

namespace TRC_W_PWT_ProductView.UI.Pages.Kanban {
    /// <summary>
    /// 未完了ピッキング画面
    /// </summary>
    public partial class KanbanShortageView : BaseForm {
        #region 定数
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
            public static readonly ControlDefine SEND_TIME_FROM = new ControlDefine( "txtSendTimeFrom", "要求時刻From", "txtSendTimeFrom", ControlDefine.BindType.Both, typeof( TimeSpan ) );
            /// <summary>
            /// 要求日付To
            /// </summary>
            public static readonly ControlDefine SEND_DATE_TO = new ControlDefine( "cldSendDateTo", "要求日付To", "cldSendDateTo", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 要求時刻To
            /// </summary>
            public static readonly ControlDefine SEND_TIME_TO = new ControlDefine( "txtSendTimeTo", "要求時刻To", "txtSendTimeTo", ControlDefine.BindType.Both, typeof( TimeSpan ) );
            /// <summary>
            /// エリア
            /// </summary>
            public static readonly ControlDefine AREA_NM = new ControlDefine( "ddlArea", "エリア", "ddlArea", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 材管ロケ大番地
            /// </summary>
            public static readonly ControlDefine ZAIKAN_PRIMARY_LOCATION = new ControlDefine( "txtZaikanPrimaryLocation", "材管ロケ大番地", "txtZaikanPrimaryLocation", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 材管ロケ中番地
            /// </summary>
            public static readonly ControlDefine ZAIKAN_SECONDARY_LOCATION = new ControlDefine( "txtZaikanSecondaryLocation", "材管ロケ中番地", "txtZaikanSecondaryLocation", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 材管ロケ小番地
            /// </summary>
            public static readonly ControlDefine ZAIKAN_TERTIARY_LOCATION = new ControlDefine( "txtZaikanTertiaryLocation", "材管ロケ小番地", "txtZaikanTertiaryLocation", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// ピッキング者ID
            /// </summary>
            public static readonly ControlDefine PICKING_USER_ID = new ControlDefine( "hdnPickingUserId", "ピッキング者", "hdnPickingUserId", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// ピッキング者名
            /// </summary>
            public static readonly ControlDefine PICKING_USER_NM = new ControlDefine( "hdnPickingUserNm", "ピッキング者", "hdnPickingUserNm", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 品番
            /// </summary>
            public static readonly ControlDefine PARTS_NUMBER = new ControlDefine( "txtPartsNumber", "品番", "txtPartsNumber", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// ピッキングNo
            /// </summary>
            public static readonly ControlDefine PICKING_LIST_NO = new ControlDefine( "txtPickingNo", "ピッキングNo", "txtPickingNo", ControlDefine.BindType.Both, typeof( string ) );
        }

        /// <summary>
        /// グリッドビュー定義
        /// </summary>
        public class GRID_KANBANSHORTAGEVIEW {
            /// <summary>
            /// ピッキングNo
            /// </summary>
            public static readonly GridViewDefine PICKING_LIST_NO = new GridViewDefine( "ピッキングNo", "PICKING_LIST_NO", typeof( string ), "", true, HorizontalAlign.Center, 135, true, true );
            /// <summary>
            /// 要求時刻
            /// </summary>
            public static readonly GridViewDefine SEND_TIME = new GridViewDefine( "要求時刻", "SEND_TIME", typeof( string ), "{0:" + DateUtils.DATE_FORMAT_MINITE + "}", true, HorizontalAlign.Center, 150, true, true );
            /// <summary>
            /// エリア
            /// </summary>
            public static readonly GridViewDefine AREA_NM = new GridViewDefine( "エリア", "AREA_NM", typeof( string ), "", true, HorizontalAlign.Left, 180, true, true );
            /// <summary>
            /// ピッキング者
            /// </summary>
            public static readonly GridViewDefine PICKING_USER = new GridViewDefine( "ピッキング者", "PICKING_USER", typeof( string ), "", true, HorizontalAlign.Left, 135, true, true );
            /// <summary>
            /// No
            /// </summary>
            public static readonly GridViewDefine DISP_ORDER = new GridViewDefine( "No", "DISP_ORDER", typeof( string ), "", true, HorizontalAlign.Center, 55, true, true );
            /// <summary>
            /// 品番
            /// </summary>
            public static readonly GridViewDefine PARTS_NUMBER = new GridViewDefine( "品番", "PARTS_NUMBER", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// 品名
            /// </summary>
            public static readonly GridViewDefine MATERIAL_NAME = new GridViewDefine( "品名", "MATERIAL_NAME", typeof( string ), "", true, HorizontalAlign.Left, 200, true, true );
            /// <summary>
            /// 材管大番地
            /// </summary>
            public static readonly GridViewDefine PRIMARY_LOCATION = new GridViewDefine( "材管大番地", "PRIMARY_LOCATION", typeof( string ), "", true, HorizontalAlign.Center, 150, true, true );
            /// <summary>
            /// 材管中・小番地
            /// </summary>
            public static readonly GridViewDefine SECONDARY_TERTIARY_LOCATION = new GridViewDefine( "材管中・小番地", "SECONDARY_TERTIARY_LOCATION", typeof( string ), "", true, HorizontalAlign.Center, 150, true, true );
            /// <summary>
            /// SNP
            /// </summary>
            public static readonly GridViewDefine SNP = new GridViewDefine( "SNP", "SNP", typeof( string ), "", true, HorizontalAlign.Left, 100, true, true );
            /// <summary>
            /// 箱数
            /// </summary>
            public static readonly GridViewDefine PICKING_BOX_COUNT = new GridViewDefine( "箱数", "PICKING_BOX_COUNT", typeof( string ), "", true, HorizontalAlign.Left, 70, true, true );
            /// <summary>
            /// 納入予定（K-SCM）
            /// </summary>
            public static readonly GridViewDefine MIN_DELIVERY_INST_DT = new GridViewDefine( "納入予定（K-SCM）", "MIN_DELIVERY_INST_DT", typeof( string ), "", true, HorizontalAlign.Center, 175, true, true );
            /// <summary>
            /// 納入予定数量
            /// </summary>
            public static readonly GridViewDefine UNPAID_QTY = new GridViewDefine( "納入予定数量", "UNPAID_QTY", typeof( string ), "", true, HorizontalAlign.Left, 175, true, true );

        }

        /// <summary>
        /// グリッドビューグループ
        /// </summary>
        const string GRID_KANBAN_SHORTAGE_VIEW_GROUP_CD = "KanbanShortageView";
        #endregion

        #region プロパティ
        /// <summary>
        /// ロガー
        /// </summary>
        private static readonly Logger Logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

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
        GridViewDefine[] GridviewDefault
        {
            get
            {
                if ( true == ObjectUtils.IsNull( _gridviewDefault ) ) {
                    _gridviewDefault = ControlUtils.GetGridViewDefineArray( typeof( GRID_KANBANSHORTAGEVIEW ) );
                }
                return _gridviewDefault;
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
        /// 検索ボタン押下時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click( object sender, EventArgs e ) {
            base.RaiseEvent( DoSearch );
        }

        /// <summary>
        /// Excel出力ボタン押下時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcel_Click( object sender, EventArgs e ) {
            base.RaiseEvent( OutputExcel );
        }

        #region グリッドビュー操作イベント
        /// <summary>
        /// グリッドビュー並び替えイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvKanbanShortageView_Sorting( object sender, GridViewSortEventArgs e ) {
            base.RaiseEvent( Sorting, sender, e );
        }

        /// <summary>
        /// グリッドビューページチェンジイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvKanbanShortageView_PageIndexChanging( object sender, CommandEventArgs e ) {
            base.RaiseEvent( PageIndexChanging, sender, e );
        }

        /// <summary>
        /// 一覧データバインドイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvKanbanShortageView_RowDataBound( object sender, GridViewRowEventArgs e ) {
            base.RaiseEvent( RowDataBound, sender, e );
        }
        #endregion
        #endregion

        #region ページ処理
        /// <summary>
        /// ページロード処理
        /// </summary>
        protected override void DoPageLoad() {
            // ベースページロード処理
            base.DoPageLoad();
            // グリッドビューの再表示
            GridView frozenGrid = grvKanbanShortageViewLB;
            ControlUtils.SetGridViewTemplateField( grvKanbanShortageViewLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ) );
            ControlUtils.SetGridViewTemplateField( grvKanbanShortageViewRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ) );
            ControlUtils.ReMakeBoundTemplateFieldCtrl( grvKanbanShortageViewLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ) );
            ControlUtils.ReMakeBoundTemplateFieldCtrl( grvKanbanShortageViewRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ) );
            int resultCnt = 0;
            if ( ObjectUtils.IsNotNull( ConditionInfo.ResultData ) ) {
                resultCnt = ConditionInfo.ResultData.Rows.Count;
            }
            ControlUtils.SetGridViewPager( ref pnlPager, grvKanbanShortageViewRB, grvKanbanShortageView_PageIndexChanging, resultCnt, grvKanbanShortageViewRB.PageIndex );
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        protected override void Initialize() {
            // アクセスカウンター登録
            Dao.Com.AccessCounterDao.Entry( base.CurrentPageInfo.pageId );
            // ベース処理初期化処理
            base.Initialize();
            // セッションをクリアする
            ConditionInfo = new ConditionInfoSessionHandler.ST_CONDITION();
            // 初期処理
            InitializeValues();
        }
        #endregion

        #region 機能別処理
        /// <summary>
        /// 初期処理
        /// </summary>
        private void InitializeValues() {
            try {
                // エリア一覧情報を検索条件：エリアのリストに設定する
                ControlUtils.SetListControlItems( ddlArea, MasterList.AreaNameList );
            } catch ( Exception ex ) {
                // 例外ログ、メッセージ表示を実行する
                Logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_84010 );
            }
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        private void DoSearch() {
            // 検索条件を作成する
            Dictionary<string, object> dicCondition = new Dictionary<string, object>();
            base.GetControlValues( ConditionControls, ref dicCondition );
            // 検索時画面情報を取得する
            Dictionary<string, string> dicIdWithText = new Dictionary<string, string>();
            base.GetControlTexts( ConditionControls, out dicIdWithText );
            // 検索結果取得
            // エラー時にも最後まで処理を通すため、インスタンスを生成しておく
            var result = new KanbanBusiness.ResultSet();
            DataTable tblResult = null;
            // 検索上限数を取得する
            int maxGridViewCount = WebAppInstance.GetInstance().Config.WebCommonInfo.maxGridViewCount;
            try {
                // 未完了ピッキング画面：検索処理を実行する
                result = KanbanBusiness.SearchIncompletePicking( dicCondition, GridviewDefault, maxGridViewCount );
            } catch ( DataAccessException ex ) {
                if ( ex.OracleErrorNumber == DataAccessBase.ORAERROR_TIMEOUT ) {
                    // クエリ発行タイムアウトが発生した場合、エラーメッセージを設定する
                    base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_61910 );
                } else {
                    // タイムアウト以外のExceptionが発生した場合、エラーメッセージを設定する
                    Logger.Exception( ex );
                    base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                }
            } catch ( Exception ex ) {
                // Exceptionが発生した場合、エラーメッセージを設定する
                Logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
            } finally {
            }
            ConditionInfoSessionHandler.ST_CONDITION cond = new ConditionInfoSessionHandler.ST_CONDITION();
            tblResult = result.ListTable;
            if ( null != tblResult ) {
                // 検索結果が存在する場合、件数表示、ページャーの設定を行う
                ntbResultCount.Value = tblResult.Rows.Count;
                ControlUtils.SetGridViewPager( ref pnlPager, grvKanbanShortageViewRB, grvKanbanShortageView_PageIndexChanging, tblResult.Rows.Count, 0 );
                // 検索条件/結果インスタンスを保持する
                cond.conditionValue = dicCondition;
                cond.IdWithText = dicIdWithText;
                cond.ResultData = tblResult.DefaultView.ToTable();
            } else {
                // タイムアウト等Exception時には、GridViewクリア
                ClearGridView();
            }
            // 検索条件をセッションに格納する
            ConditionInfo = cond;
            // グリッドビューの表示処理を行う
            GridView frozenGrid = grvKanbanShortageViewLB;
            if ( true == ObjectUtils.IsNotNull( tblResult ) ) {
                if ( 0 < tblResult.Rows.Count ) {
                    // TemplateFieldの追加
                    grvHeaderRT.Columns.Clear();
                    grvKanbanShortageViewRB.Columns.Clear();
                    for ( int idx = frozenGrid.Columns.Count; idx < GridviewDefault.Length; idx++ ) {
                        TemplateField tf = new TemplateField();
                        tf.HeaderText = StringUtils.ToString( GridviewDefault[idx].bindField );
                        grvKanbanShortageViewRB.Columns.Add( tf );
                    }
                    // 新規バインド
                    ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), ConditionInfo, true );
                    ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), ConditionInfo, true );
                    ControlUtils.BindGridView_WithTempField( grvKanbanShortageViewLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), tblResult );
                    ControlUtils.BindGridView_WithTempField( grvKanbanShortageViewRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), tblResult );
                    // GridView表示
                    divGrvDisplay.Visible = true;
                    // グリッドビュー外のDivサイズ変更
                    SetDivGridViewWidth();
                } else {
                    ClearGridView();
                }
            }
            //メッセージ表示
            if ( null != result.Message ) {
                // エラーメッセージが存在する場合、メッセージ表示
                base.WriteApplicationMessage( result.Message );
            }
            //Excel出力ボタン活性
            if ( null != tblResult && 0 < tblResult.Rows.Count ) {
                // 出力対象データありの場合、Excel出力ボタンを表示する
                this.btnExcel.Visible = true;
            } else {
                //出力対象データなしの場合、Excel出力ボタンを非表示にする
                this.btnExcel.Visible = false;
            }
        }

        /// <summary>
        /// 一覧行データバインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowDataBound( params object[] parameters ) {
            var sender = (GridView)parameters[0];
            var e = (GridViewRowEventArgs)parameters[1];
            if ( e.Row.RowType == DataControlRowType.DataRow ) {
                // データ行のみバインド処理を行う
                int index = 0;
                if ( GetColumnIndex( sender, GRID_KANBANSHORTAGEVIEW.SEND_TIME, out index ) == true ) {
                    // 要求時刻列の場合、yyyy/MM/dd HH:mm:ss形式に変換する（変換できない場合は空文字）
                    var date = new DateTime();
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_KANBANSHORTAGEVIEW.SEND_TIME.bindField].ToString();
                    e.Row.Cells[index].Text = ( DateTime.TryParse( data, out date ) ) ? date.ToString( DateUtils.DATE_FORMAT_SECOND ) : string.Empty;
                }
                if ( GetColumnIndex( sender, GRID_KANBANSHORTAGEVIEW.SECONDARY_TERTIARY_LOCATION, out index ) == true ) {
                    // 材管ロケ中番地・小番地の場合
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_KANBANSHORTAGEVIEW.SECONDARY_TERTIARY_LOCATION.bindField].ToString();
                    if(data.Trim().Length == 1 ) {
                        // 前後のスペースを除外した際に連結用の1文字しか残らなかった場合空文字にする
                        e.Row.Cells[index].Text = string.Empty;
                    }
                }
                if ( GetColumnIndex( sender, GRID_KANBANSHORTAGEVIEW.PARTS_NUMBER, out index ) == true ) {
                    // 品番列の場合、品番のフォーマットで取得した結果に変換する
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_KANBANSHORTAGEVIEW.PARTS_NUMBER.bindField].ToString();
                    e.Row.Cells[index].Text = KanbanBusiness.GetFormattedPartsNumber( data );
                }
                if ( GetColumnIndex( sender, GRID_KANBANSHORTAGEVIEW.MIN_DELIVERY_INST_DT, out index ) == true ) {
                    // 納入予定（）K-SCM列の場合、yyyy/MM/dd形式に変換する（変換できない場合は空文字）
                    var date = new DateTime();
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_KANBANSHORTAGEVIEW.MIN_DELIVERY_INST_DT.bindField].ToString();
                    e.Row.Cells[index].Text = ( DateTime.TryParse( data, out date ) ) ? date.ToString( DateUtils.DATE_FORMAT_DAY ) : string.Empty;
                }
                // 選択行の背景色変更を追加
                ControlUtils.GridViewRowBound( sender, e, GRID_KANBAN_SHORTAGE_VIEW_GROUP_CD, ControlUtils.GridRowDoubleClickEvent.None );
            }
        }

        /// <summary>
        /// Excel出力処理
        /// </summary>
        private void OutputExcel() {
            try {
                // セッションから検索データの取得
                ConditionInfoSessionHandler.ST_CONDITION cond = base.ConditionInfo;
                if ( null == cond.ResultData || 0 == cond.ResultData.Rows.Count ) {
                    // 出力対象データなし
                    return;
                }
                // 検索条件出力データ作成
                var excelCond = new List<ControlDefine>() {
                    // 要求日付From
                    CONDITION.SEND_DATE_FROM,
                    // 要求時刻From
                    CONDITION.SEND_TIME_FROM,
                    // 要求日付To
                    CONDITION.SEND_DATE_TO,
                    // 要求時刻To
                    CONDITION.SEND_TIME_TO,
                    // エリア
                    CONDITION.AREA_NM,
                    // 材管ロケ大番地
                    CONDITION.ZAIKAN_PRIMARY_LOCATION,
                    // 材管ロケ中番地
                    CONDITION.ZAIKAN_SECONDARY_LOCATION,
                    // 材管ロケ小番地
                    CONDITION.ZAIKAN_TERTIARY_LOCATION,
                    // ピッキング者名
                    CONDITION.PICKING_USER_NM,
                    // 品番
                    CONDITION.PARTS_NUMBER,
                    // ピッキングNo
                    CONDITION.PICKING_LIST_NO
                }.Select( ctrl => {
                    // 表示名、表示値を取得する
                    var data = new ExcelConditionItem( ctrl.displayNm, cond.IdWithText[ctrl.controlId] );
                    if ( ctrl.bindField == CONDITION.PICKING_USER_NM.bindField && data.Value == string.Empty ) {
                        // ピッキング者名かつ表示値が空の場合、表示名は維持して値をピッキング者IDから取得する
                        data = new ExcelConditionItem( ctrl.displayNm, cond.IdWithText[CONDITION.PICKING_USER_ID.controlId] );
                    }
                    return data;
                } ).ToList();
                // Excelダウンロード実行
                Excel.Download( Response, "未完了ピッキング", cond.ResultData, excelCond );
            } catch ( System.Threading.ThreadAbortException ) {
                // response.Endで必ず発生する為、正常として扱う
            } catch ( Exception ex ) {
                // 例外発生時、ログ出力とメッセージ表示
                Logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80030, "未完了ピッキング_検索結果" );
            }
        }
        #endregion

        #region グリッドビュー操作
        /// <summary>
        /// グリッドビュークリア
        /// </summary>
        private void ClearGridView() {
            // 列名非表示 グリッドビュークリア
            ControlUtils.InitializeGridView( grvHeaderLT, false );
            ControlUtils.InitializeGridView( grvHeaderRT, false );
            ControlUtils.InitializeGridView( grvKanbanShortageViewLB, false );
            ControlUtils.InitializeGridView( grvKanbanShortageViewRB, false );
            // 件数表示
            ntbResultCount.Value = 0;
            // ページャークリア
            ControlUtils.ClearPager( ref pnlPager );
            // GridView非表示
            divGrvDisplay.Visible = false;
        }

        /// <summary>
        /// グリッドビューページ切替
        /// </summary>
        /// <param name="parameters"></param>
        private void PageIndexChanging( params object[] parameters ) {
            object sender = parameters[0];
            CommandEventArgs e = (CommandEventArgs)parameters[1];
            int newPageIndex = Convert.ToInt32( e.CommandArgument );
            int pageSize = grvKanbanShortageViewRB.PageSize;
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
            // 背面ユーザ切替対応
            ConditionInfoSessionHandler.ST_CONDITION cond = ConditionInfo;
            GridView frozenGrid = grvKanbanShortageViewLB;
            ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), cond, true );
            ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), cond, true );
            ControlUtils.BindGridView_WithTempField( grvKanbanShortageViewLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), ConditionInfo.ResultData );
            ControlUtils.BindGridView_WithTempField( grvKanbanShortageViewRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), ConditionInfo.ResultData );
            ControlUtils.GridViewPageIndexChanging( grvKanbanShortageViewLB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
            ControlUtils.GridViewPageIndexChanging( grvKanbanShortageViewRB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
            ControlUtils.SetGridViewPager( ref pnlPager, grvKanbanShortageViewRB, grvKanbanShortageView_PageIndexChanging, ConditionInfo.ResultData.Rows.Count, grvKanbanShortageViewRB.PageIndex );
            // グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();
        }

        /// <summary>
        /// グリッドビュー並び替え
        /// </summary>
        /// <param name="parameters"></param>
        private void Sorting( params object[] parameters ) {
            object sender = parameters[0];
            GridViewSortEventArgs e = (GridViewSortEventArgs)parameters[1];
            ConditionInfoSessionHandler.ST_CONDITION cond = ConditionInfo;
            ControlUtils.GridViewSorting( grvKanbanShortageViewLB, ref cond, e, true );
            ControlUtils.GridViewSorting( grvKanbanShortageViewRB, ref cond, e );
            // 背面ユーザ切替対応
            GridView frozenGrid = grvKanbanShortageViewLB;
            ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), cond, true );
            ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), cond, true );
            ControlUtils.BindGridView_WithTempField( grvKanbanShortageViewLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), cond.ResultData );
            ControlUtils.BindGridView_WithTempField( grvKanbanShortageViewRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), cond.ResultData );
            ControlUtils.SetGridViewPager( ref pnlPager, grvKanbanShortageViewRB, grvKanbanShortageView_PageIndexChanging, cond.ResultData.Rows.Count, grvKanbanShortageViewRB.PageIndex );
            ConditionInfo = cond;
            // グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();
        }

        /// <summary>
        /// 列番号取得
        /// </summary>
        /// <param name="target">確認対象のグリッドビュー</param>
        /// <param name="def">確認する列定義</param>
        /// <param name="index">列番号</param>
        /// <returns>列定義がグリッドビューに含まれている場合はtrue、そうでなければfalse</returns>
        private bool GetColumnIndex( GridView target, GridViewDefine def, out int index ) {
            // 列番号を初期化
            index = 0;
            foreach ( DataControlField c in target.Columns ) {
                // グリッドビューの列を順次取得する
                if ( c.HeaderText == def.headerText ) {
                    // グリッドビューの列のヘッダーテキストと列定義のヘッダーテキストが一致した場合、列が存在するとする
                    return true;
                }
                // 列番号を加算する
                index++;
            }
            // すべての列を確認し、存在しなかった場合列が存在しなかったとする
            return false;
        }

        /// <summary>
        /// グリッドビュー外のDivサイズ変更
        /// </summary>
        private void SetDivGridViewWidth() {
            SetDivGridViewWidth( grvHeaderLT, divGrvHeaderLT );
            SetDivGridViewWidth( grvHeaderRT, divGrvHeaderRT );
            SetDivGridViewWidth( grvKanbanShortageViewLB, divGrvLB );
            SetDivGridViewWidth( grvKanbanShortageViewRB, divGrvRB );
        }
        /// <summary>
        /// グリッドビュー外のDivサイズ変更
        /// </summary>
        private void SetDivGridViewWidth( GridView grv, System.Web.UI.HtmlControls.HtmlGenericControl div ) {
            // セル幅補正 Padding4px + Border(片側) 1px
            const int CELL_PADDING = 4 * 2 + 1;
            // テーブル幅補正 Border(片側) 1px
            const int OUT_BORDER = 1;
            var visibleColumns = grv.Columns.Cast<DataControlField>().Where( x => x.Visible ).ToList();
            int sumWidth = NumericUtils.ToInt( visibleColumns.Sum( x => x.HeaderStyle.Width.Value ) )
                                + CELL_PADDING * visibleColumns.Count()
                                + ( visibleColumns.Any() ? OUT_BORDER : 0 );
            div.Style["width"] = $"{ sumWidth }px";
        }
        #endregion
    }
}