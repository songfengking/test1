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
using TRC_W_PWT_ProductView.UI.Base;
using TRC_W_PWT_ProductView.Dao.Com;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.ListDefine;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.Session;

namespace TRC_W_PWT_ProductView.UI.Pages.Kanban {
    public partial class KanbanPickingDetailView : BaseForm {

        #region 定数定義
        /// <summary>
        /// 検索条件
        /// </summary>
        public class CONDITION {
            /// <summary>
            /// ピッキングNo
            /// </summary>
            public static readonly ControlDefine PICKING_LIST_NO = new ControlDefine("hdnPickingNo", "ピッキングNo", "hdnPickingNo", ControlDefine.BindType.Both, typeof(string));
        }
    
        /// <summary>
        /// ピッキング明細一覧定義
        /// </summary>
        internal class GRID_DETAIL_INFO {
            // 2023/03/13 星 変更開始
            /// <summary>No</summary>
            public static readonly GridViewDefine NO = new GridViewDefine("No", "NO", typeof(string), "", true, HorizontalAlign.Center, 50, true, true);
            /// <summary>品番</summary>
            public static readonly GridViewDefine PARTS_NUMBER = new GridViewDefine("品番", "PARTS_NUMBER", typeof(string), "", true, HorizontalAlign.Center, 130, true, true);
            /// <summary>品名</summary>
            public static readonly GridViewDefine MATERIAL_NAME = new GridViewDefine("品名", "MATERIAL_NAME", typeof(string), "", true, HorizontalAlign.Left, 300, true, true);
            // 2022/07/27 大越　修正開始
            // P0677_e-かんばんピッキング システム改善依頼 列ヘッダをピッキング完了時刻に変更
            /// <summary>ピッキング完了時刻</summary>
            public static readonly GridViewDefine END_TIME = new GridViewDefine( "ピッキング完了時刻", "END_TIME", typeof(string), "", true, HorizontalAlign.Center, 180, true, true);
            // 2022/07/27 大越　修正終了
            // 2023/03/13 星 追加開始
            /// <summary>納入指示日</summary>
            public static readonly GridViewDefine DELIVERY_INSTRUCTION_DATE = new GridViewDefine( "納入指示日", "DELIVERY_INSTRUCTION_DATE", typeof( string ), "", true, HorizontalAlign.Center, 180, true, true );
            // 2023/03/13 星 追加終了
            /// <summary>材管大番地</summary>
            public static readonly GridViewDefine ZAIKAN_PRIMARY_LOCATION = new GridViewDefine("材管大番地", "ZAIKAN_PRIMARY_LOCATION", typeof(string), "", true, HorizontalAlign.Center, 130, true, true);
            /// <summary>材管中・小番地</summary>
            public static readonly GridViewDefine ZAIKAN_SEC_TER_LOCATION = new GridViewDefine("材管中・小番地", "ZAIKAN_SEC_TER_LOCATION", typeof(string), "", true, HorizontalAlign.Center, 130, true, true);
            /// <summary>SNP</summary>
            public static readonly GridViewDefine SNP = new GridViewDefine("SNP", "SNP", typeof(string), "", true, HorizontalAlign.Left, 70, true, true);
            // 2022/07/27 大越　修正開始
            // P0677_e-かんばんピッキング システム改善依頼 箱数列を読み込み済箱数/箱数列に変更
            /// <summary>読み込み済箱数/箱数</summary>
            public static readonly GridViewDefine PICKING_BOX_COUNT = new GridViewDefine( "読み込み済箱数/箱数", "PICKING_BOX_COUNT", typeof(string), "", true, HorizontalAlign.Left, 180, true, true);
            // 2022/07/27 大越　修正終了
            /// <summary>ピッキング</summary>
            public static readonly GridViewDefine PICKING_STATUS = new GridViewDefine("ピッキング", "PICKING_STATUS", typeof(string), "", true, HorizontalAlign.Left, 150, true, true);
            // 2022/07/27 大越　追加開始
            // P0677_e-かんばんピッキング システム改善依頼 かんばん照合完了時刻列を追加
            /// <summary>かんばん照合完了時刻</summary>
            public static readonly GridViewDefine KANBAN_COMPARE_COMP_FLG = new GridViewDefine( "かんばん照合完了時刻", "KANBAN_COMPARE_COMP_TIME", typeof( string ), "", true, HorizontalAlign.Center, 180, true, true );
            // 2022/07/27 大越　追加終了
            // 2023/03/13 星 変更終了
        }

        #endregion

        #region プロパティ
        /// <summary>
        /// ロガー
        /// </summary>
        private static readonly Logger _logger = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 検索条件定義情報
        /// </summary>
        ControlDefine[] _conditionControls = null;
        /// <summary>
        /// 検索条件定義情報アクセサ
        /// </summary>
        ControlDefine[] ConditionControls {
            get {
                if (true == ObjectUtils.IsNull(_conditionControls)) {
                    _conditionControls = ControlUtils.GetControlDefineArray(typeof(CONDITION));
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
                if (true == ObjectUtils.IsNull(_gridviewDefault)) {
                    _gridviewDefault = ControlUtils.GetGridViewDefineArray(typeof(GRID_DETAIL_INFO));
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
            RaiseEvent(DoPageLoad);
        }

        /// <summary>
        /// Excel出力ボタン選択動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcel_Click(object sender, EventArgs e) {
            base.RaiseEvent(DoExcelOutput);
        }

        /// <summary>
        /// グリッドビュー行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvDetailView_RowDataBound(object sender, GridViewRowEventArgs e) {
            RowDataBoundMainView(sender, e);
        }

        /// <summary>
        /// グリッドビューページチェンジイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvDetailView_PageIndexChanging(object sender, CommandEventArgs e) {
            base.RaiseEvent(PageIndexChangingMainView, sender, e);
        }

        /// <summary>
        /// グリッドビュー並び替えイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvDetailView_Sorting(object sender, GridViewSortEventArgs e) {
            base.RaiseEvent(SortingMainView, sender, e);
        }
        #endregion

        #region イベントメソッド
        /// <summary>
        /// ページロード処理
        /// </summary>
        protected override void DoPageLoad() {
            // ベースページロード処理
            base.RaiseEvent(base.DoPageLoad, false);

            // 検索結果件数取得
            int resultCnt = 0;
            if (ObjectUtils.IsNotNull(ConditionInfo.ResultData)) {
                resultCnt = ConditionInfo.ResultData.Rows.Count;
            }

            // 検索結果件数に応じてページャー設定
            ControlUtils.SetGridViewPager(ref pnlPager, grvDetailBody, grvDetailView_PageIndexChanging, resultCnt, grvDetailBody.PageIndex);

        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        protected override void Initialize() {
            // アクセスカウンター登録
            Dao.Com.AccessCounterDao.Entry(base.CurrentPageInfo.pageId);
            // ベース処理初期化処理
            base.Initialize();
            // 初期処理
            InitializeValues();
        }

        /// <summary>
        /// 初期処理
        /// </summary>
        private void InitializeValues() {
            try {
                // 引数取得
                string callerPickingno = StringUtils.Nvl(base.GetTransInfoValue(RequestParameter.Common.CALLER));

                // 引数退避
                hdnPickingNo.Value = callerPickingno;
               
                // 検索処理
                base.RaiseEvent(DoSearch);
            } catch (Exception ex) {
                // 例外ログ、メッセージ表示を実行する
                _logger.Exception(ex);
                base.WriteApplicationMessage(MsgManager.MESSAGE_ERR_84010);
            }
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        private void DoSearch() {
            // 検索条件を取得
            Dictionary<string, object> dicCondition = new Dictionary<string, object>();
            base.GetControlValues(ConditionControls, ref dicCondition);

            // 検索結果取得用変数定義
            // エラー時にも最後まで処理を通すため、インスタンスを生成しておく
            var result = new KanbanBusiness.ResultSet();
            DataTable tblResult = null;

            //---検索実行---
            try {
                // ピッキング明細画面：検索処理を実行する
                result = KanbanBusiness.SearchPickingDetail(dicCondition, gridviewDefault);
            } catch (DataAccessException ex) {
                if (ex.OracleErrorNumber == DataAccessBase.ORAERROR_TIMEOUT) {
                    // クエリ発行タイムアウトが発生した場合、エラーメッセージを設定する
                    base.WriteApplicationMessage(MsgManager.MESSAGE_WRN_61910);
                } else {
                    // タイムアウト以外のExceptionが発生した場合、エラーメッセージを設定する
                    _logger.Exception(ex);
                    base.WriteApplicationMessage(MsgManager.MESSAGE_ERR_80010);
                }
            } catch (Exception ex) {
                _logger.Exception(ex);
                base.WriteApplicationMessage(MsgManager.MESSAGE_ERR_84010, "");
            }

            //---検索結果判定---
            // 処理結果が取得できた時のみ件数・ページャーの設定を実施
            // 取得できなかった時は一覧を初期化
            ConditionInfoSessionHandler.ST_CONDITION cond = new ConditionInfoSessionHandler.ST_CONDITION();
            tblResult = result.ListTable;
            if (null != tblResult) {
                // ピッキング明細件数を表示
                ntbResultCount.Value = tblResult.Rows.Count;
                // ピッキング明細件数に応じてページャー設定
                ControlUtils.SetGridViewPager(ref pnlPager, grvDetailBody, grvDetailView_PageIndexChanging, tblResult.Rows.Count, 0);
                // 検索条件/結果インスタンスを保持する
                cond.conditionValue = dicCondition;
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
            GridView frozenGrid = grvDetailBody;
            if (true == ObjectUtils.IsNotNull(tblResult)) {
                if (0 < tblResult.Rows.Count) {
                    // TemplateFieldの追加
                    grvDetailHeader.Columns.Clear();
                    grvDetailBody.Columns.Clear();
                    for (int idx = frozenGrid.Columns.Count; idx < gridviewDefault.Length; idx++) {
                        TemplateField tf = new TemplateField();
                        tf.HeaderText = StringUtils.ToString(gridviewDefault[idx].bindField);
                        grvDetailBody.Columns.Add(tf);
                    }
        
                    // 新規バインド
                    ControlUtils.ShowGridViewHeader(grvDetailHeader, ControlUtils.GetFrozenColumns(frozenGrid, gridviewDefault, true), ConditionInfo, true);
                    ControlUtils.BindGridView_WithTempField(grvDetailBody, ControlUtils.GetFrozenColumns(frozenGrid, gridviewDefault, true), tblResult);
    
                    // GridView表示
                    divGrvDisplay.Visible = true;
                } else {
                    // GridView初期化
                    ClearGridView();
                }
            }
            // メッセージ表示
            if (null != result.Message) {
                // エラーメッセージが存在する場合、メッセージ表示
                base.WriteApplicationMessage(result.Message);
            }
            // Excel出力ボタン活性
            if (null != tblResult && 0 < tblResult.Rows.Count) {
                // 出力対象データありの場合、Excel出力ボタンを有効化する
                this.btnExcel.Visible = true;
            } else {
                // 出力対象データなしの場合、Excel出力ボタンを無効化する
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
            ControlUtils.InitializeGridView(grvDetailHeader, false);
            // 一覧ボディ部初期化
            ControlUtils.InitializeGridView(grvDetailBody, false);
            // 件数表示に0を設定
            ntbResultCount.Value = 0;
            // ページャー初期化
            ControlUtils.ClearPager(ref pnlPager);
            // GridView非表示
            divGrvDisplay.Visible = false;
        }

        /// <summary>
        /// 一覧行データバインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowDataBoundMainView(params object[] parameters) {
            var sender = (GridView)parameters[0];
            var e = (GridViewRowEventArgs)parameters[1];

            // データ行の一部の列にのみフォーマットをかけて表示する
            if (e.Row.RowType == DataControlRowType.DataRow) {
                int index = 0;
                string workCellText;
                DateTime? workCellDatetime;

                // 一覧の品番の表示をフォーマットする
                if (GetColumnIndex(sender, GRID_DETAIL_INFO.PARTS_NUMBER, out index) == true) {
                    workCellText = StringUtils.ToString(((DataRowView)e.Row.DataItem)[GRID_DETAIL_INFO.PARTS_NUMBER.bindField]);
                    workCellText = KanbanBusiness.GetFormattedPartsNumber(workCellText);
                    ((DataRowView)e.Row.DataItem)[GRID_DETAIL_INFO.PARTS_NUMBER.bindField] = workCellText;
                    e.Row.Cells[index].Text = workCellText;
                }

                // 一覧のピッキング完了時刻の表示をフォーマットする
                if (GetColumnIndex(sender, GRID_DETAIL_INFO.END_TIME, out index) == true) {
                    var date = new DateTime();
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_DETAIL_INFO.END_TIME.bindField].ToString();
                    e.Row.Cells[index].Text = ( DateTime.TryParse( data, out date ) ) ? date.ToString( DateUtils.DATE_FORMAT_SECOND ) : string.Empty;
                }

                // 2022/07/27 大越　追加開始
                // P0677_e-かんばんピッキング システム改善依頼 かんばん照合完了時刻列の表示をフォーマットする
                // 一覧のかんばん照合完了時刻の表示をフォーマットする
                if ( GetColumnIndex( sender, GRID_DETAIL_INFO.KANBAN_COMPARE_COMP_FLG, out index ) == true ) {
                    var date = new DateTime();
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_DETAIL_INFO.KANBAN_COMPARE_COMP_FLG.bindField].ToString();
                    e.Row.Cells[index].Text = ( DateTime.TryParse( data, out date ) ) ? date.ToString( DateUtils.DATE_FORMAT_SECOND ) : string.Empty;
                }
                // 2022/07/27 大越　追加終了

                ControlUtils.GridViewRowBound((GridView)sender, e, ControlUtils.GridRowDoubleClickEvent.None);
            }
        }

        /// <summary>
        /// GridViewカラムインデックス取得
        /// </summary>
        /// <param name="target"></param>
        /// <param name="def"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool GetColumnIndex(GridView target, GridViewDefine def, out int index) {
            index = 0;
            foreach (DataControlField c in target.Columns) {
                if (c.HeaderText == def.headerText) {
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
        private void PageIndexChangingMainView(params object[] parameters) {
            object sender = parameters[0];
            // 新しいページNOに応じたデータ表示に切り替える

            // 新しいページNO取得
            CommandEventArgs e = (CommandEventArgs)parameters[1];
            int newPageIndex = Convert.ToInt32(e.CommandArgument);

            // 全体のページ数取得
            int pageSize = grvDetailBody.PageSize;
            int rowCount = 0;
            if (true == ObjectUtils.IsNotNull(ConditionInfo.ResultData)) {
                rowCount = ConditionInfo.ResultData.Rows.Count;
            }
            int allPages = 0;
            allPages = ConditionInfo.ResultData.Rows.Count / pageSize;
            if (0 != rowCount % pageSize) {
                allPages += 1;
            }

            // ページが無くなっている場合には、先頭ページへ戻す
            if (newPageIndex >= allPages) {
                newPageIndex = 0;
            }

            ConditionInfoSessionHandler.ST_CONDITION cond = ConditionInfo;
            GridView frozenGrid = grvDetailBody;

            // GridViewのヘッダー表示
            ControlUtils.ShowGridViewHeader(grvDetailHeader, ControlUtils.GetFrozenColumns(frozenGrid, gridviewDefault, true), cond, true);

            // GridViewにデータ表示
            ControlUtils.BindGridView_WithTempField(grvDetailBody, ControlUtils.GetFrozenColumns(frozenGrid, gridviewDefault, true), ConditionInfo.ResultData);

            // GridViewのページ切替
            ControlUtils.GridViewPageIndexChanging(grvDetailBody, ConditionInfo.ResultData, new GridViewPageEventArgs(newPageIndex));

            // GridViewのページャーを設定
            ControlUtils.SetGridViewPager(ref pnlPager, grvDetailBody, grvDetailView_PageIndexChanging, ConditionInfo.ResultData.Rows.Count, grvDetailBody.PageIndex);

            // GridView外のDivサイズ調整
            SetDivGridViewWidth();
        }

        /// <summary>
        /// 一覧ページ並び替え
        /// </summary>
        /// <param name="parameters"></param>
        private void SortingMainView(params object[] parameters) {
            object sender = parameters[0];
            GridViewSortEventArgs e = (GridViewSortEventArgs)parameters[1];
            // GridViewの内容をソートした後、セッションにソート後の結果を格納する

            // GridViewのソート
            ConditionInfoSessionHandler.ST_CONDITION cond = ConditionInfo;
            ControlUtils.GridViewSorting(grvDetailBody, ref cond, e);

            GridView frozenGrid = grvDetailBody;

            // GridViewのヘッダー表示
            ControlUtils.ShowGridViewHeader(grvDetailHeader, ControlUtils.GetFrozenColumns(frozenGrid, gridviewDefault, true), cond, true);

            // GridViewにデータ表示
            ControlUtils.BindGridView_WithTempField(grvDetailBody, ControlUtils.GetFrozenColumns(frozenGrid, gridviewDefault, true), cond.ResultData);

            // GridViewのページャーを設定
            ControlUtils.SetGridViewPager(ref pnlPager, grvDetailBody, grvDetailView_PageIndexChanging, cond.ResultData.Rows.Count, grvDetailBody.PageIndex);

            ConditionInfo = cond;

            // GridView外のDivサイズ調整
            SetDivGridViewWidth();
        }

        /// <summary>
        /// GridView格納DIVサイズ調整
        /// </summary>
        private void SetDivGridViewWidth() {
            SetDivGridViewWidth(grvDetailHeader, divGrvHeaderLT);

            SetDivGridViewWidth(grvDetailBody, divGrvLB);
        }

        /// <summary>
        /// GridView外のDivサイズ変更
        /// </summary>
        private void SetDivGridViewWidth(GridView grv, System.Web.UI.HtmlControls.HtmlGenericControl div) {

            // セル幅補正 Padding4px + Border(片側) 1px
            const int CELL_PADDING = 4 * 2 + 1;
            // テーブル幅補正 Border(片側) 1px
            const int OUT_BORDER = 1;

            double sumWidth = 0;
            int showColCnt = 0;

            // GridView周りのdivサイズの微調整を行う
            for (int loop = 0; loop < grv.Columns.Count; loop++) {

                if (false == grv.Columns[loop].Visible) {
                    continue;
                }

                sumWidth += grv.Columns[loop].HeaderStyle.Width.Value + CELL_PADDING;       
                showColCnt += 1;
            }

            if (0 < showColCnt) {
                sumWidth += OUT_BORDER;
            }

            div.Style["width"] = Convert.ToInt32(sumWidth).ToString() + "px";
        }
        #endregion

        #region Excel出力処理
        /// <summary>
        /// Excel出力処理
        /// </summary>
        private void DoExcelOutput() {
            try {
                // セッションから検索データの取得
                ConditionInfoSessionHandler.ST_CONDITION cond = base.ConditionInfo;
                if (null == cond.ResultData || 0 == cond.ResultData.Rows.Count) {
                    // 出力対象データなし
                    return;
                }

                // 検索条件出力データ作成
                List<ExcelConditionItem> excelCond = new List<ExcelConditionItem>();

                // Excelダウンロード実行(テーブルはExcel出力用に加工する)
                Excel.Download(Response, "ピッキング明細情報", GetExcelTable(cond.ResultData), excelCond);
            } catch (System.Threading.ThreadAbortException) {
                // response.Endで必ず発生する為、正常として扱う
            } catch (Exception ex) {
                _logger.Exception(ex);
                base.WriteApplicationMessage(MsgManager.MESSAGE_ERR_80030, "ピッキング明細情報_検索結果");
            }
        }

        /// <summary>
        /// Excel出力用データテーブル作成
        /// </summary>
        /// <param name="tblSource">検索結果</param>
        /// <returns>DataTable</returns>
        private DataTable GetExcelTable(DataTable tblSource) {
            // Excel出力テーブル定義作成(一覧表示対象列を出力対象とする)
            DataTable tblResult = new DataTable();
            foreach (DataColumn column in tblSource.Columns) {
                if (false == StringUtils.IsBlank(column.Caption)) {
                    DataColumn colResult = new DataColumn(column.Caption, column.DataType);
                    tblResult.Columns.Add(colResult);
                }
            }

            // 一覧元DataTableの情報をExcel出力用テーブルにコピー
            foreach (DataRow rowSrc in tblSource.Rows) {
                DataRow rowTo = tblResult.NewRow();
                foreach (DataColumn column in tblSource.Columns) {
                    if (false == StringUtils.IsBlank(column.Caption)) {
                        rowTo[column.Caption] = rowSrc[column.ColumnName];
                    }
                }
                tblResult.Rows.Add(rowTo);
            }
            tblResult.AcceptChanges();

            return tblResult;
        }
        #endregion


    }
}