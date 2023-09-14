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
    /// (詳細 エンジン 工程) パワクロ走行検査結果
    /// </summary>
    public partial class PCrawler : System.Web.UI.UserControl, Defines.Interface.IDetail {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 画面コントロール定義
        
        /// <summary>
        /// 管理ID
        /// </summary>
        const string MANAGE_ID = Defines.Session.DetailFrame.SESSION_PAGE_INFO_DETAIL_KEY;//CurrentUCInfo.pageId

        /// <summary>
        /// DB取得データ格納先 ディクショナリキー
        /// </summary>
        private string SESSION_PAGE_INFO_DB_KEY = "bindTableData";

        /// <summary>
        /// ListView選択行制御
        /// </summary>
        const string MAIN_VIEW_SELECTED = "PCrawler.SelectMainViewRow(this,{0},'{1}');";

        /// <summary>
        /// GridVew選択行制御
        /// </summary>
        const string GRID_SUB_VIEW_GROUP_CD = "SubView";

        /// <summary>
        /// (メインリスト)コントロール定義
        /// </summary>
        public class GRID_MAIN {
            /// <summary>TR</summary>
            public static readonly ControlDefine TR_ROW_DATA = new ControlDefine( "trRowData", "TR", "", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>(SELECTコマンド送信用ボタン)</summary>
            public static readonly ControlDefine SELECT = new ControlDefine( "btnSelect", "BUTTON", "", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>検査日時</summary>
            public static readonly ControlDefine INSPECTION_DT = new ControlDefine( "txtInspectionDt", "検査日時", "inspectionDt", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>結果</summary>
            public static readonly ControlDefine RESULT = new ControlDefine( "txtResult", "結果", "result", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>検査員</summary>
            public static readonly ControlDefine EMPLOYEE_CD = new ControlDefine( "txtEmployeeCd", "検査員", "employeeCd", ControlDefine.BindType.Down, typeof( String ) );           
            /// <summary>検査グループ</summary>
            public static readonly ControlDefine INSPECTION_GROUP = new ControlDefine( "txtInspectionGroup", "検査グループ", "inspectionGroup", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>モンロー</summary>
            public static readonly ControlDefine MONROE_RESULT = new ControlDefine( "txtMonroe", "モンロー", "monroeResult", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>PTO</summary>
            public static readonly ControlDefine PTO_RESULT = new ControlDefine( "txtPTO", "PTO", "ptoResult", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>油圧</summary>
            public static readonly ControlDefine HYDRAULIC_RESULT = new ControlDefine( "txtHydraulic", "油圧", "hydraulicResult", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>ライト</summary>
            public static readonly ControlDefine HEADLIGHT_RESULT = new ControlDefine( "txtHeadLight", "ライト", "headlightResult", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>キーストップ</summary>
            public static readonly ControlDefine KEYSTOP_RESULT = new ControlDefine( "txtKeyStop", "キーストップ", "keystopResult", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>ハンドル締付</summary>
            public static readonly ControlDefine STEERING_RESULT = new ControlDefine( "txtSteeringTighten", "ハンドル締付", "steeringResult", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>走行</summary>
            public static readonly ControlDefine SPEED_RESULT = new ControlDefine( "txtSpeed", "走行", "speedResult", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>異音</summary>
            public static readonly ControlDefine NOISE_RESULT = new ControlDefine( "txtAbnormalNoise", "異音", "noiseResult", ControlDefine.BindType.Down, typeof( String ) );
        }

        /// <summary>
        /// (サブリスト)コントロール定義
        /// </summary>
        public class GRID_SUB {
            /// <summary>TR</summary>
            public static readonly ControlDefine TR_ROW_DATA = new ControlDefine( "trRowData", "TR", "", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>検査順序</summary>
            public static readonly ControlDefine INSPECTION_SEQ = new ControlDefine( "ntbInspectionSeq", "検査順序", "inspectionSeq", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>結果</summary>
            public static readonly ControlDefine RESULT = new ControlDefine( "txtResult", "結果", "result", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>2WD/4WD</summary>
            public static readonly ControlDefine STANDARD_WHEEL_DRIVE = new ControlDefine( "txtStandardWheelDrive", "2WD/4WD", "standardWheelDrive", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>副変速</summary>
            public static readonly ControlDefine STANDARD_SUB_TRANSMISSION = new ControlDefine( "txtStandardSubTransmission", "副変速", "standardSubTransmission", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>主変速</summary>
            public static readonly ControlDefine STANDARD_MAIN_TRANSMISSION = new ControlDefine( "txtStandardMainTransmission", "主変速", "standardMainTransmission", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>前進/後進</summary>
            public static readonly ControlDefine STANDARD_FORWARD_REVERSE_KIND = new ControlDefine( "txtStandardForwardReverseKind", "前進/後進", "standardForwardReverseKind", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>前輪左(km/h)</summary>
            public static readonly ControlDefine MEASURE_VALUE_L_FRONT_WHEEL = new ControlDefine( "ntbMeasureValueLFrontWheel", "前輪左(km/h)", "measureValueLFrontWheel", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>前輪右(km/h)</summary>
            public static readonly ControlDefine MEASURE_VALUE_R_FRONT_WHEEL = new ControlDefine( "ntbMeasureValueRFrontWheel", "前輪右(km/h)", "measureValueRFrontWheel", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>後輪左(km/h)</summary>
            public static readonly ControlDefine MEASURE_VALUE_L_REAR_WHEEL = new ControlDefine( "ntbMeasureValueLRearWheel", "後輪左(km/h)", "measureValueLRearWheel", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>後輪右(km/h)</summary>
            public static readonly ControlDefine MEASURE_VALUE_R_REAR_WHEEL = new ControlDefine( "ntbMeasureValueRRearWheel", "後輪右(km/h)", "measureValueRRearWheel", ControlDefine.BindType.Down, typeof( int ) );
        }

                /// <summary>
        /// 一覧表示情報コントロール(左下)
        /// </summary>
        public class GRID_SUB_SEARCH_CONTROLS_L {
        }

        /// <summary>
        /// 一覧表示情報コントロール(右下)
        /// </summary>
        public class GRID_SUB_SEARCH_CONTROLS_R {
        }

        /// <summary>
        /// 検索結果
        /// </summary>
        /// 
        internal class GRID_SUB_COMMON {
            /// <summary>検査順序</summary>
            public static readonly GridViewDefine INSPECTION_SEQ = new GridViewDefine( "検査順序", "inspectionSeq", typeof( string ), "", false, HorizontalAlign.Right, 90, true, true );
            /// <summary>結果</summary>
            public static readonly GridViewDefine RESULT = new GridViewDefine( "結果", "result", typeof( string ), "", false, HorizontalAlign.Center, 70, true, true );
            /// <summary>2WD/4WD</summary>
            public static readonly GridViewDefine STANDARD_WHEEL_DRIVE = new GridViewDefine( "2WD<br />4WD", "standardWheelDrive", typeof( string ), "", false, HorizontalAlign.Center, 70, true, true );
            /// <summary>副変速</summary>
            public static readonly GridViewDefine STANDARD_SUB_TRANSMISSION = new GridViewDefine( "副変速", "standardSubTransmission", typeof( string ), "", false, HorizontalAlign.Center, 70, true, true );
            /// <summary>主変速</summary>
            public static readonly GridViewDefine STANDARD_MAIN_TRANSMISSION = new GridViewDefine( "主変速", "standardMainTransmission", typeof( string ), "", false, HorizontalAlign.Center, 70, true, true );
            /// <summary>前進/後進</summary>
            public static readonly GridViewDefine STANDARD_FORWARD_REVERSE_KIND = new GridViewDefine( "前進<br />後進", "standardForwardReverseKind", typeof( string ), "", false, HorizontalAlign.Center, 70, true, true );
            /// <summary>前輪左(km/h)</summary>
            public static readonly GridViewDefine MEASURE_VALUE_L_FRONT_WHEEL = new GridViewDefine( "前輪左<br />(km/h)", "measureValueLFrontWheel", typeof( string ), "{0:#,0.0}", false, HorizontalAlign.Right, 70, true, true );
            /// <summary>前輪右(km/h)</summary>
            public static readonly GridViewDefine MEASURE_VALUE_R_FRONT_WHEEL = new GridViewDefine( "前輪右<br />(km/h)", "measureValueRFrontWheel", typeof( string ), "{0:#,0.0}", false, HorizontalAlign.Right, 70, true, true );
            /// <summary>後輪左(km/h)</summary>
            public static readonly GridViewDefine MEASURE_VALUE_L_REAR_WHEEL = new GridViewDefine( "後輪左<br />(km/h)", "measureValueLRearWheel", typeof( string ), "{0:#,0.0}", false, HorizontalAlign.Right, 70, true, true );
            /// <summary>後輪右(km/h)</summary>
            public static readonly GridViewDefine MEASURE_VALUE_R_REAR_WHEEL = new GridViewDefine( "後輪右<br />(km/h)", "measureValueRRearWheel", typeof( string ), "{0:#,0.0}", false, HorizontalAlign.Right, 70, true, true );
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
        protected void lstMainListRB_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundMainRBList( sender, e );
        }
        protected void lstMainListLB_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundMainLBList( sender, e );
        }

        /// <summary>
        /// メインリスト選択行変更中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
         protected void lstMainListLB_SelectedIndexChanging( object sender, ListViewSelectEventArgs e ) {
            //処理なし
        }
                protected void lstMainListRB_SelectedIndexChanging( object sender, ListViewSelectEventArgs e ) {
            //処理なし
        }


        /// <summary>
        /// メインリスト選択行変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void lstMainListLB_SelectedIndexChanged( object sender, EventArgs e ) {
            CurrentForm.RaiseEvent( SelectedIndexChangedMainLBList );
        }
        protected void lstMainListRB_SelectedIndexChanged( object sender, EventArgs e ) {
            CurrentForm.RaiseEvent( SelectedIndexChangedMainRBList );
        }


        /// <summary>
        /// サブリスト行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvSubViewRB_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundSubViewLB( sender, e );
        }
        protected void grvSubViewLB_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundSubViewRB( sender, e );
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
            Business.DetailViewBusiness.ResultSetPCrawler resPCrawler = new Business.DetailViewBusiness.ResultSetPCrawler();
            try {
                resPCrawler = Business.DetailViewBusiness.SelectTractorPCrawlerDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial );
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

            //取得データをセッションに格納
            Dictionary<string, object> dicPageControlInfo = new Dictionary<string, object>();
            dicPageControlInfo.Add( SESSION_PAGE_INFO_DB_KEY, resPCrawler );
            CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).SetPageControlInfo( MANAGE_ID, dicPageControlInfo );

            if ( 0 == resPCrawler.MainTable.Rows.Count ) {
                //検索結果0件
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62010 , CurrentUCInfo.title );
                return;
            }

            InitializeValues( resPCrawler );
        }

        #endregion

        /// <summary>
        /// 初期値設定
        /// </summary>
        private void InitializeValues( Business.DetailViewBusiness.ResultSetPCrawler resPCrawler ) {

            //メインリストバインド
            lstMainListLB.DataSource = resPCrawler.MainTable;
            lstMainListLB.DataBind();
            lstMainListLB.SelectedIndex = 0;

            lstMainListRB.DataSource = resPCrawler.MainTable;
            lstMainListRB.DataBind();
            lstMainListRB.SelectedIndex = 0;

            //サブリストバインド
            SelectedIndexChangedMainList( lstMainListLB.SelectedIndex );
        }

        #endregion

        #region リストバインド
        /// <summary>
        /// メインリストバインド処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void ItemDataBoundMainList( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {
                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Item, MainControls, rowBind, ref dicSetValues );

                //検査日時加工/セット
                KTTextBox txtInspectionDt = ( (KTTextBox)e.Item.FindControl( GRID_MAIN.INSPECTION_DT.controlId ) );
                string inspectionDt = DateUtils.ToString( rowBind[GRID_MAIN.INSPECTION_DT.bindField], DateUtils.FormatType.Second );
                txtInspectionDt.Value = inspectionDt;

                //クリックイベントセット
                HtmlTableRow trRow = (HtmlTableRow)e.Item.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
                Button selectBtn = (Button)e.Item.FindControl( GRID_MAIN.SELECT.controlId );
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format( ControlUtils.LIST_VIEW_SELECTED, e.Item.DataItemIndex, selectBtn.UniqueID );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );

            }
        }

        /// <summary>
        /// サブリストバインド処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void ItemDataBoundSubList( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {
                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Item, SubControls, rowBind, ref dicSetValues );
                //クリックイベントセット
                HtmlTableRow trRow = (HtmlTableRow)e.Item.FindControl( GRID_SUB.TR_ROW_DATA.controlId );
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format( ControlUtils.LIST_VIEW_SELECTED, e.Item.DataItemIndex,"" );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );
            }
        }

        /// <summary>
        /// メインリスト選択行変更処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void SelectedIndexChangedMainList( int paramIndex ) {

            int mainIndex = paramIndex;

            Business.DetailViewBusiness.ResultSetPCrawler resPcrawler = new Business.DetailViewBusiness.ResultSetPCrawler();
            Dictionary<string, object> dicPageControlInfo = CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).GetPageControlInfo( MANAGE_ID );
            if ( true == dicPageControlInfo.ContainsKey( SESSION_PAGE_INFO_DB_KEY ) ) {
                resPcrawler = (Business.DetailViewBusiness.ResultSetPCrawler)dicPageControlInfo[SESSION_PAGE_INFO_DB_KEY];
            }


            if ( 0 < resPcrawler.SubTables[mainIndex].Rows.Count ) {
                //一覧表示列の設定
                GridViewDefine[] gridColumns;
                List<GridViewDefine> columns = new List<GridViewDefine>();
                columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_SUB_COMMON ) ) );
                gridColumns = columns.ToArray();

                ConditionInfoSessionHandler.ST_CONDITION cond_sub = new ConditionInfoSessionHandler.ST_CONDITION();
                cond_sub.ResultData = resPcrawler.SubTables[mainIndex];

                //グリッドビューバインド
                GridView frozenGrid = grvSubViewLB;
                if ( 0 < cond_sub.ResultData.Rows.Count ) {

                    //新規バインド
                    ControlUtils.ShowGridViewHeader( grvSubHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, gridColumns, true ), cond_sub, true );
                    ControlUtils.ShowGridViewHeader( grvSubHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, gridColumns, false ), cond_sub, true );
                    ControlUtils.BindGridView_WithTempField( grvSubViewLB, ControlUtils.GetFrozenColumns( frozenGrid, gridColumns, true ), cond_sub.ResultData );
                    ControlUtils.BindGridView_WithTempField( grvSubViewRB, ControlUtils.GetFrozenColumns( frozenGrid, gridColumns, false ), cond_sub.ResultData );

                    //グリッドビュー外のDivサイズ変更
                    SetDivGridSubViewWidth();
                } else {
                    ClearSubGridView();
                }                    
            }
        }
         /// <summary>
        /// グリッドビュー行バインド(左下)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowDataBoundSubViewLB( params object[] parameters ) {
            object sender = parameters[0];
            GridViewRowEventArgs e = (GridViewRowEventArgs)parameters[1];

            if ( e.Row.RowType == DataControlRowType.DataRow ) {
                DataRow row = ( (DataRowView)e.Row.DataItem ).Row;

                Dictionary<string, object> dicControls = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Row, ControlUtils.GetControlDefineArray( typeof( GRID_SUB_SEARCH_CONTROLS_L ) ), row, ref dicControls );

            }
            ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_SUB_VIEW_GROUP_CD );
        }

        /// <summary>
        /// グリッドビュー行バインド(右下)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowDataBoundSubViewRB( params object[] parameters ) {
            object sender = parameters[0];
            GridViewRowEventArgs e = (GridViewRowEventArgs)parameters[1];

            if ( e.Row.RowType == DataControlRowType.DataRow ) {
                DataRow rowData = ( (DataRowView)e.Row.DataItem ).Row;
                Dictionary<string, object> dicControls = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Row, ControlUtils.GetControlDefineArray( typeof( GRID_SUB_SEARCH_CONTROLS_L ) ), rowData, ref dicControls );
            }
            ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_SUB_VIEW_GROUP_CD );
        }

        private void ItemDataBoundMainLBList( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {
                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Item, MainControls, rowBind, ref dicSetValues );

                //検査日時加工/セット
                KTTextBox txtInspectionDt = ( (KTTextBox)e.Item.FindControl( GRID_MAIN.INSPECTION_DT.controlId ) );
                string inspectionDt = DateUtils.ToString( rowBind[GRID_MAIN.INSPECTION_DT.bindField], DateUtils.FormatType.Second );
                txtInspectionDt.Value = inspectionDt;

                //クリックイベントセット
                HtmlTableRow trRow = (HtmlTableRow)e.Item.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
                Button selectBtn = (Button)e.Item.FindControl( GRID_MAIN.SELECT.controlId );
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format( MAIN_VIEW_SELECTED, e.Item.DataItemIndex, selectBtn.UniqueID );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );

            }
        }

        private void SelectedIndexChangedMainLBList() {

            int mainIndex = lstMainListLB.SelectedIndex;

            //選択行背景色変更解除
            foreach ( ListViewDataItem li in lstMainListLB.Items ) {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace( " " + ResourcePath.CSS.ListSelectedRow, "" );
            }

            foreach ( ListViewDataItem li in lstMainListRB.Items ) {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace( " " + ResourcePath.CSS.ListSelectedRow, "" );
            }


            //一覧項目選択済に色変更
            HtmlTableRow trSelectRow = (HtmlTableRow)lstMainListLB.Items[mainIndex].FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
            trSelectRow.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;

            HtmlTableRow trSelectRowRB = (HtmlTableRow)lstMainListRB.Items[mainIndex].FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
            trSelectRowRB.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;

            SelectedIndexChangedMainList( mainIndex );

        }

        private void ItemDataBoundMainRBList( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {
                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Item, MainControls, rowBind, ref dicSetValues );

                //クリックイベントセット
                HtmlTableRow trRow = (HtmlTableRow)e.Item.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
                Button selectBtn = (Button)e.Item.FindControl( GRID_MAIN.SELECT.controlId );
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format( MAIN_VIEW_SELECTED, e.Item.DataItemIndex, selectBtn.UniqueID );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );

            }
        }

        private void SelectedIndexChangedMainRBList() {

            int mainIndex = lstMainListRB.SelectedIndex;

            //選択行背景色変更解除
            foreach ( ListViewDataItem li in lstMainListLB.Items ) {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace( " " + ResourcePath.CSS.ListSelectedRow, "" );
            }

            foreach ( ListViewDataItem li in lstMainListRB.Items ) {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace( " " + ResourcePath.CSS.ListSelectedRow, "" );
            }


            //一覧項目選択済に色変更
            HtmlTableRow trSelectRow = (HtmlTableRow)lstMainListLB.Items[mainIndex].FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
            trSelectRow.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;

            HtmlTableRow trSelectRowRB = (HtmlTableRow)lstMainListRB.Items[mainIndex].FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
            trSelectRowRB.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;

            SelectedIndexChangedMainList( mainIndex );
        }

        #endregion

        #region Grid設定
        /// <summary>
        /// グリッドビュー格納DIVサイズ調整
        /// </summary>
        private void SetDivGridSubViewWidth() {
            SetDivGridViewWidth( grvSubHeaderLT, divGrvSubHeaderLT );
            SetDivGridViewWidth( grvSubHeaderRT, divGrvSubHeaderRT );

            SetDivGridViewWidth( grvSubViewLB, divGrvSubLB );
            SetDivGridViewWidth( grvSubViewRB, divGrvSubRB );
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
        private void ClearSubGridView() {

            //列名非表示 グリッドビュークリア
            ControlUtils.InitializeGridView( grvSubHeaderLT, false );
            ControlUtils.InitializeGridView( grvSubHeaderRT, false );
            ControlUtils.InitializeGridView( grvSubViewLB, false );
            ControlUtils.InitializeGridView( grvSubViewRB, false );

            //グリッドビュー外のDivサイズ変更
            SetDivGridSubViewWidth();

            //GridView非表示
            divGrvSubDisplay.Visible = false;
        }
        #endregion

    }
}