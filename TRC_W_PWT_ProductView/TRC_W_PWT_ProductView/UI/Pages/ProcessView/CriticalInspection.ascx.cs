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

namespace TRC_W_PWT_ProductView.UI.Pages.ProcessView {
    /// <summary>
    /// (詳細 エンジン 工程) 精密測定データ
    /// </summary>
    public partial class CriticalInspection : System.Web.UI.UserControl, Defines.Interface.IDetail {

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
        const string MAIN_VIEW_SELECTED = "CriticalInspection.SelectMainViewRow(this,{0},'{1}');";

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
            /// <summary>部品名</summary>
            public static readonly ControlDefine PARTS_NM = new ControlDefine( "txtPartsNm", "部品名", "partsNm", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>加工日</summary>
            public static readonly ControlDefine PROCESS_YMD = new ControlDefine( "txtProcessYmd", "加工日", "processYmd", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>連番</summary>
            public static readonly ControlDefine PROCESS_NUM = new ControlDefine( "txtProcessNum", "連番", "processNum", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>品番</summary>
            public static readonly ControlDefine PARTS_NUM = new ControlDefine( "txtPartsNum", "品番", "PartsNum", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>作業区分</summary>
            public static readonly ControlDefine OPERATION_KIND = new ControlDefine( "txtOperationKind", "作業区分", "operationKind", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>納入日時</summary>
            public static readonly ControlDefine DELIVERY_DT = new ControlDefine( "txtDeliveryDt", "納入日時", "deliveryDt", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>結果</summary>
            public static readonly ControlDefine RESULT = new ControlDefine( "txtResult", "結果", "result", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>備考</summary>
            public static readonly ControlDefine NOTES = new ControlDefine( "txtNotes", "備考", "notes", ControlDefine.BindType.Down, typeof( String ) );
        }

        /// <summary>
        /// (サブリスト)コントロール定義
        /// </summary>
        public class GRID_SUB {
            /// <summary>TR</summary>
            public static readonly ControlDefine TR_ROW_DATA = new ControlDefine( "trRowData", "TR", "", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>ファイルNo</summary>
            public static readonly ControlDefine FILE_NUM = new ControlDefine( "ntbFileNum", "ファイルNo", "fileNum", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>ファイル名</summary>
            public static readonly ControlDefine FILE_NM = new ControlDefine( "btnFileNm", "ファイル名", "fileNm", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>ファイルデータ</summary>
            public static readonly ControlDefine FILE_DATA = new ControlDefine( "fileData", "ファイルデータ", "fileData", ControlDefine.BindType.None, typeof( Byte[] ) );
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
        protected void lstMainListLB_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundMainLBList( sender, e );
        }
        protected void lstMainListRB_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundMainRBList( sender, e );
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
        protected void lstSubList_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundSubList( sender, e );
        }

        /// <summary>
        /// サブリスト PDFファイル名ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFileNm_Command( object sender, CommandEventArgs e ) {
            CurrentForm.RaiseEvent( CommandSubListFileNm, sender , e );
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
            Business.DetailViewBusiness.ResultSet res = new Business.DetailViewBusiness.ResultSet();
            try {
                res = Business.DetailViewBusiness.SelectEngine3CInspectionDetail( DetailKeyParam.ClassCd, DetailKeyParam.ProductModelCd, DetailKeyParam.Serial );
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
            dicPageControlInfo.Add( SESSION_PAGE_INFO_DB_KEY, res );
            CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).SetPageControlInfo( MANAGE_ID, dicPageControlInfo );

            if ( 0 == res.MainTable.Rows.Count ) {
                //検索結果0件
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62010 , CurrentUCInfo.title );
                return;
            }

            InitializeValues( res );
        }

        #endregion

        #endregion

        /// <summary>
        /// 初期値設定
        /// </summary>
        private void InitializeValues( Business.DetailViewBusiness.ResultSet res ) {
            //メインリストバインド
            lstMainListLB.DataSource = res.MainTable;
            lstMainListLB.DataBind();
            lstMainListLB.SelectedIndex = 0;

            lstMainListRB.DataSource = res.MainTable;
            lstMainListRB.DataBind();
            lstMainListRB.SelectedIndex = 0;

            //サブリストバインド
            SelectedIndexChangedMainList( lstMainListLB.SelectedIndex );
        }

        #region リストバインド
        /// <summary>
        /// メインリストバインド処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
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
        /// <summary>
        /// メインリストバインド処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void ItemDataBoundMainRBList( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {
                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Item, MainControls, rowBind, ref dicSetValues );

                //納入日時加工/セット
                KTTextBox txtDeliveryDt = ( (KTTextBox)e.Item.FindControl( GRID_MAIN.DELIVERY_DT.controlId ) );
                string deliveryDt = DateUtils.ToString( rowBind[GRID_MAIN.DELIVERY_DT.bindField], DateUtils.FormatType.Second );
                txtDeliveryDt.Value = deliveryDt;

                //クリックイベントセット
                HtmlTableRow trRow = (HtmlTableRow)e.Item.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
                Button selectBtn = (Button)e.Item.FindControl( GRID_MAIN.SELECT.controlId );
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format( MAIN_VIEW_SELECTED, e.Item.DataItemIndex, selectBtn.UniqueID );

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

                //表記をファイル名とし、ファイル名ボタンクリックの引数をインデックスにする
                KTButton btnFileNm = (KTButton)e.Item.FindControl( GRID_SUB.FILE_NM.controlId );
                btnFileNm.Text = rowBind[GRID_SUB.FILE_NM.bindField].ToString();
                btnFileNm.CommandArgument = e.Item.DataItemIndex.ToString();

                //クリックイベントセット
                HtmlTableRow trRow = (HtmlTableRow)e.Item.FindControl( GRID_SUB.TR_ROW_DATA.controlId );
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format( ControlUtils.LIST_VIEW_SELECTED, e.Item.DataItemIndex, "" );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );

            }
        }


        /// <summary>
        /// メイン選択行変更（左）
        /// </summary>
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
        /// <summary>
        /// メイン選択行変更（右）
        /// </summary>
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

        /// <summary>
        /// メインリスト選択行変更処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void SelectedIndexChangedMainList( int paramIndex ) {

            int mainIndex = paramIndex;

            Business.DetailViewBusiness.ResultSet res = new Business.DetailViewBusiness.ResultSet();
            Dictionary<string, object> dicPageControlInfo = CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).GetPageControlInfo( MANAGE_ID );
            if ( true == dicPageControlInfo.ContainsKey( SESSION_PAGE_INFO_DB_KEY ) ) {
                res = (Business.DetailViewBusiness.ResultSet)dicPageControlInfo[SESSION_PAGE_INFO_DB_KEY];
            }

            //サブリスト初期化
            lstSubList.DataSource = null;
            lstSubList.DataBind();
            
            //サブリストバインド
            if ( 0 < res.SubTable.Rows.Count ) {
                    lstSubList.DataSource = res.SubTable;
                    lstSubList.DataBind();
            }

        }

        /// <summary>
        /// サブリスト ファイルダウンロード処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void CommandSubListFileNm( params object[] parameters ) {
            object sender = parameters[0];
            CommandEventArgs e = (CommandEventArgs)parameters[1];

            int index = Convert.ToInt32( e.CommandArgument );

            Business.DetailViewBusiness.ResultSet res = new Business.DetailViewBusiness.ResultSet();
            Dictionary<string, object> dicPageControlInfo = CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).GetPageControlInfo( MANAGE_ID );
            if ( true == dicPageControlInfo.ContainsKey( SESSION_PAGE_INFO_DB_KEY ) ) {
                res = (Business.DetailViewBusiness.ResultSet)dicPageControlInfo[SESSION_PAGE_INFO_DB_KEY];

                string fileNm = res.SubTable.Rows[index][GRID_SUB.FILE_NM.bindField].ToString();
                byte[] blobData = (byte[])res.SubTable.Rows[index][GRID_SUB.FILE_DATA.bindField];
                WebFileUtils.DownloadFile( CurrentForm, blobData, fileNm );
            }
        }
                

        #endregion

    }
}