using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.IO;
using KTFramework.Common;
using KTFramework.Dao;
using KTWebControl.CustomControls;
using KTWebInheritance.Common;
using KTFramework.Excel;
using KTFramework.C1Common.Excel;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.UI.Base;
using C1.C1Report;
using TRC_W_PWT_ProductView.Dao.Process;
using C1.C1Excel;

namespace TRC_W_PWT_ProductView.UI.Pages.ProcessView {
    /// <summary>
    /// (詳細 トラクタ 工程) 関所
    /// </summary>
    public partial class CheckPoint : System.Web.UI.UserControl, Defines.Interface.IDetail {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義

        const string MANAGE_ID = "LINE_CD";

        /// <summary>
        /// 工程情報選択イベント
        /// </summary>
        /// <remarks>パラメータ 差し替え先コントロール URL</remarks>
        const string MAIN_VIEW_SELECTED = "CheckPoint.SelectMainViewRow(this,{0},'{1}');";
        /// <summary>
        /// 作業、実績差異有のフラグ
        /// </summary>
        const string CNT_DIFFERENCE_FLG = "1";

        /// <summary>
        /// コントロール定義(工程情報)
        /// </summary>
        public class GRID_STATION {
            /// <summary>TR</summary>
            public static readonly ControlDefine TR_ROW_DATA = new ControlDefine( "trRowData", "TR", "", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>(SELECTコマンド送信用ボタン)</summary>
            public static readonly ControlDefine SELECT = new ControlDefine( "btnSelect", "BUTTON", "", ControlDefine.BindType.None, typeof( String ) );
            /// <suumary>ステーションコード</summary>
            public static readonly ControlDefine STATION_CD = new ControlDefine("txtStationCd", "ステーションコード", "STATION_CD", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>ステーション名</summary>
            public static readonly ControlDefine STATION_NM = new ControlDefine("txtStationNm", "ステーション名", "STATION_NM", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>作業開始時刻</summary>
            public static readonly ControlDefine WORK_START_DT = new ControlDefine("txtWorkStartDt", "作業開始時刻", "WORK_START_DT", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>作業終了時刻</summary>
            public static readonly ControlDefine WORK_END_DT = new ControlDefine("txtWorkEndDt", "作業終了時刻", "WORK_END_DT", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>実績差分フラグ(非表示)</summary>
            public static readonly ControlDefine RESULT_DIFFERENCE_FLG1 = new ControlDefine("txtResultDifferenceFlg1", "実績差分フラグ", "RESULT_DIFFERENCE_FLG", ControlDefine.BindType.Down, typeof(String));
            /// <suumary>関所</summary>
            public static readonly ControlDefine CHECK_POINT = new ControlDefine("txtCheckPoint", "関所", "CHECK_POINT", ControlDefine.BindType.Down, typeof( DateTime ) );
            /// <suumary>次ページ</summary>
            public static readonly ControlDefine PLC_NEXT_PAGE = new ControlDefine("txtPlcNextPage", "次ページ", "PLC_NEXT_PAGE", ControlDefine.BindType.Down, typeof( DateTime ) );
            /// <suumary>前データ</summary>
            public static readonly ControlDefine PLC_PREV = new ControlDefine("txtPlcPrev", "前データ", "PLC_PREV", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>次データ</summary>
            public static readonly ControlDefine PLC_NEXT = new ControlDefine("txtPlcNext", "次データ", "PLC_NEXT", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>歯抜け</summary>
            public static readonly ControlDefine PLC_MISSING = new ControlDefine("txtPlcMissing", "歯抜け", "PLC_MISSING", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>手直し</summary>
            public static readonly ControlDefine PLC_REPAIR = new ControlDefine("txtPlcRepair", "手直し", "PLC_REPAIR", ControlDefine.BindType.Down, typeof(String));
            /// <suumary>実績差分フラグ(非表示)</summary>
            public static readonly ControlDefine RESULT_DIFFERENCE_FLG2 = new ControlDefine("txtResultDifferenceFlg2", "実績差分フラグ", "RESULT_DIFFERENCE_FLG", ControlDefine.BindType.Down, typeof(String));
        }

        /// <summary>
        /// コントロール定義(工程作業実績情報)
        /// </summary>
        public class GRID_WORK {
            /// <summary>TR</summary>
            public static readonly ControlDefine TR_ROW_DATA = new ControlDefine( "trRowData", "TR", "", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>順序</summary>
            public static readonly ControlDefine INSTRUCT_ORDER = new ControlDefine("txtInstructOrder", "順序", "INSTRUCT_ORDER", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>作業内容</summary>
            public static readonly ControlDefine INSTRUCT_CONTENT = new ControlDefine("txtInstructContent", "作業内容", "INSTRUCT_CONTENT", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>工具</summary>
            public static readonly ControlDefine TOOL_NM = new ControlDefine("txtToolNm", "工具", "TOOL_NM", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>指示回数</summary>
            public static readonly ControlDefine INSTRUCT_CNT = new ControlDefine("txtInstructCnt", "指示回数", "INSTRUCT_CNT", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>実績回数</summary>
            public static readonly ControlDefine RESULT_CNT = new ControlDefine("txtResultCnt", "実績回数", "RESULT_CNT", ControlDefine.BindType.Down, typeof( String ) );
        }

        #endregion

        #region CSS

        /// <summary>
        /// サムネイル項目 項目選択済用CSS
        /// </summary>
        const string LIST_SELECTED_ITEM_CSS = "div-item-selected";
        /// <summary>
        /// 作業、実績差異があるセルスタイル
        /// </summary>
        const string CNT_DIFFERENCE_CELL = "cell-cnt-difference";

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
        /// コントロール定義(工程情報)
        /// </summary>
        ControlDefine[] _mainControls = null;
        /// <summary>
        /// コントロール定義(工程情報)アクセサ
        /// </summary>
        ControlDefine[] MainControls {
            get {
                if ( true == ObjectUtils.IsNull( _mainControls ) ) {
                    _mainControls = ControlUtils.GetControlDefineArray( typeof(GRID_STATION) );
                }
                return _mainControls;
            }
        }

        /// <summary>
        /// (コントロール定義(工程作業実績情報)
        /// </summary>
        ControlDefine[] _SubChkCtrl = null;
        /// <summary>
        /// コントロール定義(工程作業実績情報)アクセサ
        /// </summary>
        ControlDefine[] SubChkCtrl {
            get {
                if ( true == ObjectUtils.IsNull( _SubChkCtrl ) ) {
                    _SubChkCtrl = ControlUtils.GetControlDefineArray( typeof(GRID_WORK) );
                }
                return _SubChkCtrl;
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

        #region メンバ変数

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
        /// 工程情報リスト行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void listProcessLB_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundMainLBList( sender, e );
        }
        protected void listProcessRB_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundMainRBList( sender, e );
        }

        /// <summary>
        /// メインリスト選択行変更中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void listProcessLB_SelectedIndexChanging( object sender, ListViewSelectEventArgs e ) {
            //処理なし
        }
        protected void listProcessRB_SelectedIndexChanging( object sender, ListViewSelectEventArgs e ) {
            //処理なし
        }
        /// <summary>
        /// メインリスト選択行変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void listProcessLB_SelectedIndexChanged( object sender, EventArgs e ) {
            CurrentForm.RaiseEvent(SelectedIndexChangedMainLBList);
        }
        protected void listProcessRB_SelectedIndexChanged( object sender, EventArgs e ) {
            CurrentForm.RaiseEvent( SelectedIndexChangedMainRBList );
        }

        /// <summary>
        /// 工程作業実績情報行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void listWork_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundChkInfo( sender, e );
        }

        /// <summary>
        /// エクセル出力ボタン処理
        /// </summary>
        protected void btnOutputExcel_Click(object sender, EventArgs e) {
            CurrentForm.RaiseEvent(OutputExcel);
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
                //工程情報ヘッダ取得
                res = Business.DetailViewBusiness.SelectCheckPointHeader( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial, DetailKeyParam.LineCd);
                //ライン情報をセッションに格納
                Dictionary<string, object> dicLineInfo = new Dictionary<string, object>();
                dicLineInfo.Add(MANAGE_ID, DetailKeyParam.LineCd);
                CurrentForm.SessionManager.GetPageControlInfoHandler(CurrentForm.Token).SetPageControlInfo(MANAGE_ID, dicLineInfo);
            } catch ( KTFramework.Dao.DataAccessException ex ) {
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

            //メインリストバインド
            listProcessLB.DataSource = res.MainTable;
            listProcessLB.DataBind();
            listProcessLB.SelectedIndex = 0;

            listProcessRB.DataSource = res.MainTable;
            listProcessRB.DataBind();
            listProcessRB.SelectedIndex = 0;

            //サブリストバインド
            SelectedIndexChangedMainList(listProcessLB.SelectedIndex);

            //選択行背景色変更解除
            foreach (ListViewDataItem li in listProcessLB.Items) {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl(GRID_STATION.TR_ROW_DATA.controlId);
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace(" " + ResourcePath.CSS.ListSelectedRow, "");
            }

            foreach (ListViewDataItem li in listProcessRB.Items) {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl(GRID_STATION.TR_ROW_DATA.controlId);
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace(" " + ResourcePath.CSS.ListSelectedRow, "");
            }


            //一覧項目選択済に色変更
            HtmlTableRow trSelectRow = (HtmlTableRow)listProcessLB.Items[listProcessLB.SelectedIndex].FindControl(GRID_STATION.TR_ROW_DATA.controlId);
            trSelectRow.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;

            HtmlTableRow trSelectRowRB = (HtmlTableRow)listProcessRB.Items[listProcessRB.SelectedIndex].FindControl(GRID_STATION.TR_ROW_DATA.controlId);
            trSelectRowRB.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;


        }
        #endregion

        #endregion

        #region リストバインド
        /// <summary>
        /// メインリストバインド処理（左）
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void ItemDataBoundMainLBList( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {
                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Item, MainControls, rowBind, ref dicSetValues );

                //クリックイベントセット
                HtmlTableRow trRow = (HtmlTableRow)e.Item.FindControl(GRID_STATION.TR_ROW_DATA.controlId );
                Button selectBtn = (Button)e.Item.FindControl(GRID_STATION.SELECT.controlId );
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format( MAIN_VIEW_SELECTED, e.Item.DataItemIndex, selectBtn.UniqueID );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );

            }
        }
        /// <summary>
        /// メインリストバインド処理（右）
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void ItemDataBoundMainRBList( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {
                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Item, MainControls, rowBind, ref dicSetValues );

                //クリックイベントセット
                HtmlTableRow trRow = (HtmlTableRow)e.Item.FindControl(GRID_STATION.TR_ROW_DATA.controlId );
                Button selectBtn = (Button)e.Item.FindControl(GRID_STATION.SELECT.controlId );
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format( MAIN_VIEW_SELECTED, e.Item.DataItemIndex, selectBtn.UniqueID );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );

            }
        }

        /// <summary>
        /// サブリストバインド処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void ItemDataBoundChkInfo( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {
                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();

                //工程作業実績情報
                CurrentForm.SetControlValues( e.Item, SubChkCtrl, rowBind, ref dicSetValues );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );
            }
        }
        /// <summary>
        /// メインリスト選択行変更処理呼び出し（左）
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void SelectedIndexChangedMainLBList() {

            int mainIndex = listProcessLB.SelectedIndex;

            //選択行背景色変更解除
            foreach (ListViewDataItem li in listProcessLB.Items) {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl(GRID_STATION.TR_ROW_DATA.controlId);
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace(" " + ResourcePath.CSS.ListSelectedRow, "");
            }

            foreach (ListViewDataItem li in listProcessRB.Items) {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl(GRID_STATION.TR_ROW_DATA.controlId);
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace(" " + ResourcePath.CSS.ListSelectedRow, "");
            }


            //一覧項目選択済に色変更
            HtmlTableRow trSelectRow = (HtmlTableRow)listProcessLB.Items[mainIndex].FindControl(GRID_STATION.TR_ROW_DATA.controlId);
            trSelectRow.Attributes["class"] = trSelectRow.Attributes["class"].Replace(" " + CNT_DIFFERENCE_CELL, "");
            trSelectRow.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;

            HtmlTableRow trSelectRowRB = (HtmlTableRow)listProcessRB.Items[mainIndex].FindControl(GRID_STATION.TR_ROW_DATA.controlId);
            trSelectRowRB.Attributes["class"] = trSelectRow.Attributes["class"].Replace(" " + CNT_DIFFERENCE_CELL, "");
            trSelectRowRB.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;

            SelectedIndexChangedMainList(mainIndex);

        }
        /// <summary>
        /// メインリスト選択行変更処理呼び出し（右）
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void SelectedIndexChangedMainRBList() {

            int mainIndex = listProcessRB.SelectedIndex;

            //選択行背景色変更解除
            foreach ( ListViewDataItem li in listProcessLB.Items ) {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl(GRID_STATION.TR_ROW_DATA.controlId );
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace( " " + ResourcePath.CSS.ListSelectedRow, "" );
            }

            foreach ( ListViewDataItem li in listProcessRB.Items ) {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl(GRID_STATION.TR_ROW_DATA.controlId );
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace( " " + ResourcePath.CSS.ListSelectedRow, "" );
            }


            //一覧項目選択済に色変更
            HtmlTableRow trSelectRow = (HtmlTableRow)listProcessLB.Items[mainIndex].FindControl(GRID_STATION.TR_ROW_DATA.controlId );
            trSelectRow.Attributes["class"] = trSelectRow.Attributes["class"].Replace(" " + CNT_DIFFERENCE_CELL, "");
            trSelectRow.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;

            HtmlTableRow trSelectRowRB = (HtmlTableRow)listProcessRB.Items[mainIndex].FindControl(GRID_STATION.TR_ROW_DATA.controlId );
            trSelectRowRB.Attributes["class"] = trSelectRow.Attributes["class"].Replace(" " + CNT_DIFFERENCE_CELL, "");
            trSelectRowRB.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;

            SelectedIndexChangedMainList( mainIndex );
        }
        /// <summary>
        /// メインリスト選択行変更処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void SelectedIndexChangedMainList( int paramIndex ) {

            int mainIndex = paramIndex;

            //選択されたレコードのステーションコードを取得
            TextBox tmpStationCd = (TextBox)listProcessLB.Items[mainIndex].FindControl(GRID_STATION.STATION_CD.controlId );
            string paramStationCd = StringUtils.ToString(tmpStationCd.Text );
            //検索結果取得
            Business.DetailViewBusiness.ResultSetMulti res = new Business.DetailViewBusiness.ResultSetMulti();

            try {
                res = Business.DetailViewBusiness.SelectCheckPoint( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial, paramStationCd);
            } catch ( KTFramework.Dao.DataAccessException ex ) {
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
                //return;
            }

            //工程作業実績情報バインド
            listWork.DataSource = res.MainTable;
            listWork.DataBind();

            //セル背景色変更
            ChangeCellBackColor();
        }
        /// <summary>
        /// 指示実績数差異有データ色付け処理
        /// </summary>
        private void ChangeCellBackColor() {

            //工程情報
            foreach (ListViewDataItem li in listProcessLB.Items) {
                HtmlTableRow trDifferenceRow = (HtmlTableRow)li.FindControl(GRID_STATION.TR_ROW_DATA.controlId);
                var txtResultDifferenceFlg = (KTTextBox)trDifferenceRow.FindControl(GRID_STATION.RESULT_DIFFERENCE_FLG1.controlId);
                string resultDifferenceFlg = StringUtils.ToString(txtResultDifferenceFlg.Text);
                if (resultDifferenceFlg == CNT_DIFFERENCE_FLG && false == trDifferenceRow.Attributes["class"].Contains(" " + ResourcePath.CSS.ListSelectedRow)) {
                    trDifferenceRow.Attributes["class"] = trDifferenceRow.Attributes["class"].Replace(" " + CNT_DIFFERENCE_CELL, "");
                    trDifferenceRow.Attributes["class"] = trDifferenceRow.Attributes["class"] + " " + CNT_DIFFERENCE_CELL;
                }
            }
            foreach (ListViewDataItem li in listProcessRB.Items) {
                HtmlTableRow trDifferenceRow = (HtmlTableRow)li.FindControl(GRID_STATION.TR_ROW_DATA.controlId);
                var txtResultDifferenceFlg = (KTTextBox)trDifferenceRow.FindControl(GRID_STATION.RESULT_DIFFERENCE_FLG2.controlId);
                string resultDifferenceFlg = StringUtils.ToString(txtResultDifferenceFlg.Text);
                if (resultDifferenceFlg == CNT_DIFFERENCE_FLG && false == trDifferenceRow.Attributes["class"].Contains(" " + ResourcePath.CSS.ListSelectedRow)) {
                    trDifferenceRow.Attributes["class"] = trDifferenceRow.Attributes["class"].Replace(" " + CNT_DIFFERENCE_CELL, "");
                    trDifferenceRow.Attributes["class"] = trDifferenceRow.Attributes["class"] + " " + CNT_DIFFERENCE_CELL;
                }
            }

            //工程作業実績情報
            foreach (ListViewDataItem li in listWork.Items) {
                HtmlTableRow trDifferenceRow = (HtmlTableRow)li.FindControl(GRID_WORK.TR_ROW_DATA.controlId);
                var txtInstructCnt = (KTTextBox)trDifferenceRow.FindControl(GRID_WORK.INSTRUCT_CNT.controlId);
                var txtResultCnt = (KTTextBox)trDifferenceRow.FindControl(GRID_WORK.RESULT_CNT.controlId);
                int instructCnt = NumericUtils.ToInt(txtInstructCnt.Value);
                int resultCnt = NumericUtils.ToInt(txtResultCnt.Value);
                if (instructCnt != resultCnt) {
                    trDifferenceRow.Attributes["class"] = trDifferenceRow.Attributes["class"].Replace(" " + CNT_DIFFERENCE_CELL, "");
                    trDifferenceRow.Attributes["class"] = trDifferenceRow.Attributes["class"] + " " + CNT_DIFFERENCE_CELL;
                }
            }
        }

        #endregion

        #region Excel出力
        /// <summary>
        /// Excel出力処理
        /// </summary>
        private void OutputExcel() {

            //EXCELオブジェクト作成
            //KTFramework.Excel.ExcelUtils _excelUtils = null;
            KTFramework.C1Common.Excel.ExcelHandler handler = null;

            //出力ファイル指定
            string templateFilePath = System.AppDomain.CurrentDomain.BaseDirectory + @"TemplateFiles\Excel\関所テンプレート.xls";
            string ountputFileNm = string.Format("関所_{0}.xls", DateUtils.ToString(DateTime.Now, DateUtils.FormatType.SecondNoSep));

            //ライン情報取得
            Dictionary<string, object> dicPageControlInfo = CurrentForm.SessionManager.GetPageControlInfoHandler(CurrentForm.Token).GetPageControlInfo(MANAGE_ID);
            string lineCd = StringUtils.ToString(dicPageControlInfo[MANAGE_ID]); 

            try {
                
                handler = new ExcelHandler(templateFilePath);

                //出力シート選択
                XLSheet xlSheet = handler.Sheets["関所"];

                //日時出力
                xlSheet[0, 1].Value = DateTime.Now.ToString(DateUtils.DATE_FORMAT_SECOND);

                //検索条件作成
                List<string> outputCondition = new List<string>();
                outputCondition.Add(DetailKeyParam.ProductModelCd);   //型式コード
                outputCondition.Add(DetailKeyParam.Serial);           //機番
                outputCondition.Add(DetailKeyParam.Idno);             //IDNO
                outputCondition.Add(lineCd);

                int outputConditionStartRowIndex = 3;
                int outputConditionStartColIndex = 1;
                for (int i = 0; outputCondition.Count > i; i++) {
                    //検索条件をEXCELに書き込み
                    xlSheet[outputConditionStartRowIndex, outputConditionStartColIndex].Value = outputCondition[i];
                    outputConditionStartRowIndex++;
                }

                ////出力データ作成
                DataTable tblOutputData = TractorProcessDao.SelectCheckPointForExcel(DetailKeyParam.ProductModelCd, DetailKeyParam.Serial, lineCd);

                int tmpRowCopyCnt = 0;
                tmpRowCopyCnt = tblOutputData.Rows.Count;

                //データ開始行
                int dataStartIndex = 9;

                //出力 行
                XLRow xlRowTemplate = xlSheet.Rows[dataStartIndex].Clone();
                //データ数のテンプレートコピー
                for (int rowLoop = 1; rowLoop < tmpRowCopyCnt; rowLoop++) {
                    int idxRowTo = dataStartIndex + rowLoop;

                    //行高とスタイルをコピー
                    xlSheet.Rows[idxRowTo].Height = xlRowTemplate.Height;
                    xlSheet.Rows[idxRowTo].Style = xlRowTemplate.Style;
                    //セルの値とスタイルをコピー                     
                    for (int colLoop = 0; colLoop < tblOutputData.Columns.Count; colLoop++) {
                        XLCell xlCopyToCell = xlSheet[idxRowTo, colLoop];
                        XLCell xlCopyFromCell = xlSheet[dataStartIndex, colLoop];
                        xlCopyToCell.SetValue(xlCopyFromCell.Value, xlCopyFromCell.Style);
                    }
                }

                //データ反映
                int rowIndex = dataStartIndex;
                int colIndex = 0;
                if (tblOutputData != null) {
                    foreach (DataRow row in tblOutputData.Rows) {
                        colIndex = 0;
                        foreach (DataColumn col in tblOutputData.Columns) {
                            string castVal = StringUtils.ToString(row[col]);
                            xlSheet[rowIndex, colIndex].Value = castVal;
                            colIndex++;
                        }
                        rowIndex++;
                    }
                }

                // ファイル出力
                handler.Download(Response, ountputFileNm);

            } catch (System.Threading.ThreadAbortException) {
                //response.Endで必ず発生する為、正常として扱う
            } catch (Exception ex) {
                logger.Exception(ex);
                CurrentForm.WriteApplicationMessage(MsgManager.MESSAGE_ERR_80030, "関所情報_検索結果");
            } finally {
                //リソース解放
                handler.Dispose();
                handler = null;
            }
        }

        #region セルスタイル
        /// <summary>
        /// セルスタイル(右揃え)
        /// </summary>
        /// <param name="flgDifference" >指示実績差異フラグ(true:差異有,　false:差異無)</param>
        private static List<CellStyleSetting> GetSameContainerCellStyle_Right(bool flgDifference) {

            List<CellStyleSetting> cellStyleSettingList = new List<CellStyleSetting>();

            CellStyleSetting cellStyleSetting = new CellStyleSetting();
            cellStyleSetting.horizonAlignment = NPOI.SS.UserModel.HorizontalAlignment.Right;
            cellStyleSetting.fillPattern = NPOI.SS.UserModel. FillPattern.SolidForeground;
            cellStyleSetting.borderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleSetting.borderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleSetting.borderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleSetting.borderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleSetting.fontStyle = "text";
            if (flgDifference == true) {
                cellStyleSetting.fillBackgroundColor = NPOI.SS.UserModel.IndexedColors.Yellow.Index;
            }
            cellStyleSettingList.Add(cellStyleSetting);

            return cellStyleSettingList;
        }
        /// <summary>
        /// セルスタイル(左揃え)
        /// </summary>
        /// <param name="flgDifference" >指示実績差異フラグ(true:差異有,　false:差異無)</param>
        private static List<CellStyleSetting> GetSameContainerCellStyle_Left(bool flgDifference) {

            List<CellStyleSetting> cellStyleSettingList = new List<CellStyleSetting>();

            CellStyleSetting cellStyleSetting = new CellStyleSetting();
            cellStyleSetting.horizonAlignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
            cellStyleSetting.fillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
            cellStyleSetting.borderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleSetting.borderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleSetting.borderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleSetting.borderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleSetting.fontStyle = "text";
            if (flgDifference == true) {
                cellStyleSetting.fillBackgroundColor = NPOI.SS.UserModel.IndexedColors.Yellow.Index;
            }
            cellStyleSettingList.Add(cellStyleSetting);

            return cellStyleSettingList;
        }
        /// <summary>
        /// セルスタイル(中央揃え)
        /// </summary>
        /// <param name="flgDifference" >指示実績差異フラグ(true:差異有,　false:差異無)</param>
        private static List<CellStyleSetting> GetSameContainerCellStyle_Center(bool flgDifference) {

            List<CellStyleSetting> cellStyleSettingList = new List<CellStyleSetting>();

            CellStyleSetting cellStyleSetting = new CellStyleSetting();
            cellStyleSetting.horizonAlignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            cellStyleSetting.fillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
            cellStyleSetting.borderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleSetting.borderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleSetting.borderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleSetting.borderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleSetting.fontStyle = "text";
            if (flgDifference == true) {
                cellStyleSetting.fillBackgroundColor = NPOI.SS.UserModel.IndexedColors.Yellow.Index;
            }
            cellStyleSettingList.Add(cellStyleSetting);

            return cellStyleSettingList;
        }
        #endregion

        #endregion


    }
}