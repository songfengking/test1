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
using TRC_W_PWT_ProductView.Dao.Process;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.Session;
using TRC_W_PWT_ProductView.UI.Base;
using C1.C1Report;

namespace TRC_W_PWT_ProductView.UI.Pages.ProcessView {
    /// <summary>
    /// (詳細 トラクタ 工程) 電子チェックシート
    /// </summary>
    public partial class EngineELCheckSheet : System.Web.UI.UserControl, Defines.Interface.IDetail {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義

        /// <summary>
        /// 管理ID
        /// </summary>
        const string MANAGE_ID = Defines.Session.DetailFrame.SESSION_PAGE_INFO_DETAIL_KEY;//CurrentUCInfo.pageId
        const string MANAGE_ID2 = "EngineELCheckSheet";
        const string MANAGE_ID_HEADER = "ImgCheckSheetHeader";

        /// <summary>
        /// 詳細画面切替イベント(サムネイル画像クリックでメインエリアに通常サイズ表示を行う)
        /// </summary>
        /// <remarks>パラメータ 差し替え先コントロール URL</remarks>
        const string CHANGE_MAIN_AREA_VIEW = "ELCheckSheetDef.ChangeMainAreaImage('{0}','{1}');";

        /// <summary>
        /// 詳細画面切替イベント(サムネイル画像クリックでメインエリアに通常サイズ表示を行う)
        /// </summary>
        /// <remarks>パラメータ 差し替え先コントロール URL</remarks>
        const string CHANGE_MAIN_AREA_VIEW2 = "ELCheckSheetDef.ChangeMainAreaImage2('{0}','{1}');";

        const string MAIN_VIEW_SELECTED = "ELCheckSheetDef.SelectMainViewRow(this,{0},'{1}');";
        const string LIST_VIEW_SELECTED = "ELCheckSheetDef.SelectListViewRow(this,{0},'{1}');";
        const string NG_LIST_VIEW_SELECTED = "ELCheckSheetDef.SelectNGListViewRow(this,{0},'{1}');";



        /// <summary>
        /// DB取得データ格納先 ディクショナリキー
        /// </summary>
        private string SESSION_PAGE_INFO_DB_KEY = "bindTableData";

        /// <summary>
        /// 画面内コントロール
        /// </summary>
        internal class ViewControl {
            /// <summary>SELECTコマンド送信用ボタン</summary>
            public static readonly ControlDefine SELECT = new ControlDefine( "btnSelect", "BUTTON", "", ControlDefine.BindType.None, typeof( String ) );
        }

        //定数
        /// <summary>検査情報</summary>
        private const Int32 CHK_INFO = 0;
        /// <summary>不具合一覧情報</summary>
        private const Int32 NG_LIST = 1;
        /// <summary>検査画像</summary>
        private const Int32 CHK_IMG = 2;
        /// <summary>不具合画像</summary>
        private const Int32 NG_IMG = 3;
        /// <summary>固定列数</summary>
        private const int FIXED_COLUMNS_NUM = 5;
        /// <summary>順序連番文字数</summary>
        private const int LENGTH_ORDER_NO = 12;
        //GridView選択列制御
        const string GRID_MAIN_VIEW_GROUP_CD = "MainView";

        #region コントロール定義：メインリスト
        /// <summary>
        /// (メインリスト)コントロール定義
        /// </summary>
        public class GRID_MAIN {
            /// <suumary>投入順序連番</summary>
            public static readonly GridViewDefine ORDER_NO = new GridViewDefine( "投入順序連番", "ORDER_NO", typeof( string ), "", false, HorizontalAlign.Center, 110, true, true );
            /// <suumary>ラインコード(非表示)</summary>
            public static readonly GridViewDefine LINE_CD = new GridViewDefine( "ラインコード", "LINE_CD", typeof( string ), "", false, HorizontalAlign.Left, 0, false, true );
            /// <suumary>ライン名</summary>
            public static readonly GridViewDefine LINE_NM = new GridViewDefine( "ライン名", "LINE_NM", typeof( string ), "", false, HorizontalAlign.Left, 250, true, true );
            /// <suumary>検査開始時間</summary>
            public static readonly GridViewDefine INS_START_DT = new GridViewDefine( "検査開始日時", "INS_START_DT", typeof( DateTime ), "{0:yyyy/MM/dd HH:mm:ss}", false, HorizontalAlign.Center, 150, true, true );
            /// <suumary>検査終了時間</summary>
            public static readonly GridViewDefine INS_END_DT = new GridViewDefine( "検査終了日時", "INS_END_DT", typeof( DateTime ), "{0:yyyy/MM/dd HH:mm:ss}", false, HorizontalAlign.Center, 150, true, true );
            /// <suumary>結果</summary>
            public static readonly GridViewDefine PASS_STATUS = new GridViewDefine( "結果", "PASS_STATUS", typeof( string ), "", false, HorizontalAlign.Center, 60, true, true );
            /// <suumary>最終検査工程</summary>
            public static readonly GridViewDefine INS_PROD_PROC = new GridViewDefine( "最終検査工程", "lastProc", typeof( string ), "", false, HorizontalAlign.Left, 140, true, true );
            /// <suumary>検査開始社員</summary>
            public static readonly GridViewDefine INS_START_BY = new GridViewDefine( "検査開始社員", "INS_START_BY", typeof( string ), "", false, HorizontalAlign.Left, 160, true, true );
            /// <suumary>合格判定社員</summary>
            public static readonly GridViewDefine PASS_EMPLOYEE_CD = new GridViewDefine( "合格判定社員", "PASS_EMPLOYEE_CD", typeof( string ), "", false, HorizontalAlign.Left, 160, true, true );
        }

        /// <summary>
        /// (メイングリッド)コントロール定義[トラクタ]
        /// </summary>
        public class GRID_MAIN_TRACTOR {
            /// <suumary>確定順序連番</summary>
            public static readonly GridViewDefine ORDER_NO = new GridViewDefine( "確定順序連番", "DECISION_NO", typeof( string ), "", false, HorizontalAlign.Center, 110, true, true );
            /// <suumary>ラインコード(非表示)</summary>
            public static readonly GridViewDefine LINE_CD = new GridViewDefine( "ラインコード", "LINE_CD", typeof( string ), "", false, HorizontalAlign.Left, 0, false, true );
            /// <suumary>ライン名</summary>
            public static readonly GridViewDefine LINE_NM = new GridViewDefine( "ライン名", "LINE_NM", typeof( string ), "", false, HorizontalAlign.Left, 250, true, true );
            /// <suumary>検査開始時間</summary>
            public static readonly GridViewDefine INS_START_DT = new GridViewDefine( "検査開始日時", "INS_START_DT", typeof( DateTime ), "{0:yyyy/MM/dd HH:mm:ss}", false, HorizontalAlign.Center, 150, true, true );
            /// <suumary>検査終了時間</summary>
            public static readonly GridViewDefine INS_END_DT = new GridViewDefine( "検査終了日時", "INS_END_DT", typeof( DateTime ), "{0:yyyy/MM/dd HH:mm:ss}", false, HorizontalAlign.Center, 150, true, true );
            /// <suumary>結果</summary>
            public static readonly GridViewDefine PASS_STATUS = new GridViewDefine( "結果", "PASS_STATUS", typeof( string ), "", false, HorizontalAlign.Center, 60, true, true );
            /// <suumary>最終検査工程</summary>
            public static readonly GridViewDefine INS_PROD_PROC = new GridViewDefine( "最終検査工程", "lastProc", typeof( string ), "", false, HorizontalAlign.Left, 140, true, true );
            /// <suumary>検査開始社員</summary>
            public static readonly GridViewDefine INS_START_BY = new GridViewDefine( "検査開始社員", "INS_START_BY", typeof( string ), "", false, HorizontalAlign.Left, 160, true, true );
            /// <suumary>合格判定社員</summary>
            public static readonly GridViewDefine PASS_EMPLOYEE_CD = new GridViewDefine( "合格判定社員", "PASS_EMPLOYEE_CD", typeof( string ), "", false, HorizontalAlign.Left, 160, true, true );
        }
        #endregion

        #region コントロール定義：サブリスト
        /// <summary>
        /// (サブリスト)コントロール定義(検査情報)
        /// </summary>
        public class GRID_CHK_INFO {
            /// <summary>TR</summary>
            public static readonly ControlDefine TR_ROW_DATA = new ControlDefine( "trRowData", "TR", "", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>工程</summary>
            public static readonly ControlDefine PROD_PROC = new ControlDefine( "txtProc", "工程", "PROD_PROC", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>No</summary>
            public static readonly ControlDefine GUARANTEE_NO = new ControlDefine( "txtGuaranteeNo", "No", "GUARANTEE_NO", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>区分</summary>
            public static readonly ControlDefine CRIT_KIND = new ControlDefine( "txtCritKind", "区分", "CRIT_KIND", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>保証項目名</summary>
            public static readonly ControlDefine GUARANTEE_ITEM_NM = new ControlDefine( "txtGuaranteeNm", "保証項目名", "GUARANTEE_ITEM_NM", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>入力値１</summary>
            public static readonly ControlDefine INPUT_VALUE1 = new ControlDefine( "txtInput1", "入力値1", "INPUT_VALUE1", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>入力値２</summary>
            public static readonly ControlDefine INPUT_VALUE2 = new ControlDefine( "txtInput2", "入力値2", "INPUT_VALUE2", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>検査結果</summary>
            public static readonly ControlDefine PASS_STATUS = new ControlDefine( "txtPassStatus", "検査結果", "PASS_STATUS", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>検査時刻</summary>
            public static readonly ControlDefine INS_DT = new ControlDefine( "txtInsDt", "検査時刻", "INS_DT", ControlDefine.BindType.Down, typeof( DateTime ) );
            /// <summary>検査社員</summary>
            public static readonly ControlDefine EMPLOYEE_CD = new ControlDefine( "txtEmpCd", "検査社員", "EMPLOYEE_CD", ControlDefine.BindType.Down, typeof( String ) );
        }

        /// <summary>
        /// (サブリスト)コントロール定義(不具合一覧)
        /// </summary>
        public class GRID_NG_LIST {
            /// <summary>TR</summary>
            public static readonly ControlDefine TR_ROW_DATA = new ControlDefine( "trRowData", "TR", "", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>工程</summary>
            public static readonly ControlDefine PROD_PROC = new ControlDefine( "txtProc", "工程", "PROD_PROC", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>No</summary>
            public static readonly ControlDefine GUARANTEE_NO = new ControlDefine( "txtGuaranteeNo", "№", "GUARANTEE_NO", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>履歴№</summary>
            public static readonly ControlDefine RECORD_NO = new ControlDefine( "txtRecNo", "履歴№", "RECORD_NO", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>区分</summary>
            public static readonly ControlDefine CRIT_KIND = new ControlDefine( "txtCritKind", "区分", "CRIT_KIND", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>保証項目名</summary>
            public static readonly ControlDefine GUARANTEE_NM = new ControlDefine( "txtGuarant", "保証項目", "GUARANTEE_NM", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>ステータス</summary>
            public static readonly ControlDefine CONFIRM_STATUS = new ControlDefine( "txtConfStatus", "ステータス", "CONFIRM_STATUS", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>不具合内容</summary>
            public static readonly ControlDefine NG_DTL = new ControlDefine( "txtNGDtl", "不具合内容", "NG_DTL", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>不具合値</summary>
            public static readonly ControlDefine NG_VALUE = new ControlDefine( "txtNGValue", "不具合値", "NG_VALUE", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>不具合記入日時</summary>
            public static readonly ControlDefine NG_UPDATE_DT = new ControlDefine( "txtNGDt", "不具合記入日時", "NG_UPDATE_DT", ControlDefine.BindType.Down, typeof( DateTime ) );
            /// <summary>不具合記入社員</summary>
            public static readonly ControlDefine NG_UPDATE_EMPL_CD = new ControlDefine( "txtNGEmpCd", "不具合記入社員名", "NG_UPDATE_EMPL_CD", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>不具合写真枚数</summary>
            public static readonly ControlDefine NG_IMG_CNT = new ControlDefine( "txtNGImgCnt", "不具合写真枚数", "NG_IMG_CNT", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>手直し有無</summary>
            public static readonly ControlDefine ADJUST_STATUS = new ControlDefine( "txtAdjStaus", "手直し有無", "ADJUST_STATUS", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>手直し内容</summary>
            public static readonly ControlDefine ADJUST_DTL = new ControlDefine( "txtAdjDtl", "手直し内容", "ADJUST_DTL", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>起因ライン</summary>
            public static readonly ControlDefine NG_CAUSED_LINE_CD = new ControlDefine( "txtCauseLine", "起因ライン", "NG_CAUSED_LINE_CD", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>起因ライン特記事項</summary>
            public static readonly ControlDefine NG_CAUSED_LINE = new ControlDefine( "txtCauseLineDtl", "起因ライン特記事項", "NG_CAUSED_LINE", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>手直し対応日時</summary>
            public static readonly ControlDefine ADJUST_DT = new ControlDefine( "txtAdjDt", "手直し対応日時", "ADJUST_DT", ControlDefine.BindType.Down, typeof( DateTime ) );
            /// <summary>手直し対応社員</summary>
            public static readonly ControlDefine ADJUST_EMPL_CD = new ControlDefine( "txtAdjEmpCd", "手直し対応社員名", "ADJUST_EMPL_CD", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>手直し写真枚数</summary>
            public static readonly ControlDefine ADJUST_IMG_CNT = new ControlDefine( "txtAdjImgCnt", "手直し写真枚数", "ADJUST_IMG_CNT", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>確認日時</summary>
            public static readonly ControlDefine CONFIRM_DT = new ControlDefine( "txtConfDt", "確認日時", "CONFIRM_DT", ControlDefine.BindType.Down, typeof( DateTime ) );
            /// <summary>確認社員</summary>
            public static readonly ControlDefine CONFIRM_EMPL_CD = new ControlDefine( "txtConfEmpCd", "確認社員", "CONFIRM_EMPL_CD", ControlDefine.BindType.Down, typeof( String ) );
        }

        /// <summary>
        /// 検査画像　コントロール定義
        /// </summary>
        public class CHK_IMG_LIST {
            /// <summary>DIV</summary>
            public static readonly ControlDefine DIV_ROW_DATA = new ControlDefine( "divRowData", "DIV", "", ControlDefine.BindType.None, typeof( Byte[] ) );
            /// <summary>画像ファイル</summary>
            public static readonly ControlDefine IMAGE_DATA = new ControlDefine( "imgCameraImage", "画像ファイル", "IMAGE_DATA", ControlDefine.BindType.None, typeof( Byte[] ) );
            /// <summary>工程コード</summary>
            public static readonly ControlDefine PROD_PROC_INFO = new ControlDefine( "txtProcInfo", "工程情報", "PROD_PROC_INFO", ControlDefine.BindType.Down, typeof( String ) );
        }

        /// <summary>
        /// 不具合画像　コントロール定義
        /// </summary>
        public class NG_IMG_LIST {
            /// <summary>DIV</summary>
            public static readonly ControlDefine DIV_ROW_DATA = new ControlDefine( "divRowData", "DIV", "", ControlDefine.BindType.None, typeof( Byte[] ) );
            /// <summary>画像ファイル</summary>
            public static readonly ControlDefine IMAGE_DATA = new ControlDefine( "imgCameraImage", "画像ファイル", "IMAGE_DATA", ControlDefine.BindType.None, typeof( Byte[] ) );
            /// <summary>工程情報</summary>
            public static readonly ControlDefine PROD_PROC_INFO = new ControlDefine( "txtProcInfo", "工程情報", "PROD_PROC_INFO", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>不具合内容</summary>
            public static readonly ControlDefine NG_DTL = new ControlDefine( "txtNGDtl", "不具合内容", "NG_DTL", ControlDefine.BindType.Down, typeof( String ) );
        }
        #endregion

        #endregion

        #region CSS

        /// <summary>
        /// サムネイル項目 項目選択済用CSS
        /// </summary>
        const string LIST_SELECTED_ITEM_CSS = "div-item-selected";

        /// <summary>
        /// サムネイル(リスト内イメージ)最大幅
        /// </summary>
        const int LIST_MAX_WIDTH = 200;

        /// <summary>
        /// サムネイル(リスト内イメージ)最大高さ
        /// </summary>
        const int LIST_MAX_HEIGHT = 150;

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
        /// (メイン)コントロール定義
        /// </summary>
        GridViewDefine[] _mainControls = null;
        /// <summary>
        /// (メイン)コントロール定義アクセサ
        /// </summary>
        GridViewDefine[] MainControls {
            get {
                if ( true == ObjectUtils.IsNull( _mainControls ) ) {
                    //_mainControls = ControlUtils.GetControlDefineArray( typeof( GRID_MAIN ) );
                    if ( GeneralPatternCode.Tractor == DetailKeyParam.ProductKind ) {
                        //トラクタイメージチェックシート
                        _mainControls = ControlUtils.GetGridViewDefineArray( typeof( GRID_MAIN_TRACTOR ) );
                    } else {
                        //エンジン電子チェックシート
                        _mainControls = ControlUtils.GetGridViewDefineArray( typeof( GRID_MAIN ) );
                   }
                }
                return _mainControls;
            }
        }

        /// <summary>
        /// (検査情報)コントロール定義
        /// </summary>
        ControlDefine[] _SubChkCtrl = null;
        /// <summary>
        /// (検査情報)コントロール定義アクセサ
        /// </summary>
        ControlDefine[] SubChkCtrl {
            get {
                if ( true == ObjectUtils.IsNull( _SubChkCtrl ) ) {
                    _SubChkCtrl = ControlUtils.GetControlDefineArray( typeof( GRID_CHK_INFO ) );
                }
                return _SubChkCtrl;
            }
        }

        /// <summary>
        /// (不具合一致)コントロール定義
        /// </summary>
        ControlDefine[] _SubNGListCtrl = null;
        /// <summary>
        /// (不具合一致)コントロール定義アクセサ
        /// </summary>
        ControlDefine[] SubNGListCtrl {
            get {
                if ( true == ObjectUtils.IsNull( _SubNGListCtrl ) ) {
                    _SubNGListCtrl = ControlUtils.GetControlDefineArray( typeof( GRID_NG_LIST ) );
                }
                return _SubNGListCtrl;
            }
        }

        /// <summary>
        /// 不具合画像リストテンプレート定義情報
        /// </summary>
        ControlDefine[] _NGImgCtrl = null;
        /// <summary>
        /// 画像リストテンプレート定義情報アクセサ
        /// </summary>
        ControlDefine[] NGImgCtrl {
            get {
                if ( true == ObjectUtils.IsNull( _NGImgCtrl ) ) {
                    _NGImgCtrl = ControlUtils.GetControlDefineArray( typeof( NG_IMG_LIST ) );
                }
                return _NGImgCtrl;
            }
        }
        /// <summary>
        /// 検査画像リストテンプレート定義情報
        /// </summary>
        ControlDefine[] _ChkImgCtrl = null;
        /// <summary>
        /// 画像リストテンプレート定義情報アクセサ
        /// </summary>
        ControlDefine[] ChkImgCtrl {
            get {
                if ( true == ObjectUtils.IsNull( _ChkImgCtrl ) ) {
                    _ChkImgCtrl = ControlUtils.GetControlDefineArray( typeof( CHK_IMG_LIST ) );
                }
                return _ChkImgCtrl;
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
        static Dictionary<String, String> _dicEmp = new Dictionary<String, String>();

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

        #region メインリスト
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
        protected void lstMainListRB_SelectedIndexChanging( object sender, ListViewSelectEventArgs e ) {
            //処理なし
        }
        protected void lstMainListLB_SelectedIndexChanging( object sender, ListViewSelectEventArgs e ) {
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
        #endregion

        #region メイングリッド
        /// <summary>
        /// メイングリッド行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMainViewLB_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainViewLB( sender, e );
        }
        protected void grvMainViewRB_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainViewRB( sender, e );
        }

        /// <summary>
        /// メイングリッド選択行変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMainViewLB_SelectedIndexChanged( object sender, EventArgs e ) {
            CurrentForm.RaiseEvent( SelectedIndexChangedMainLBList );
        }
        protected void grvMainViewRB_SelectedIndexChanged( object sender, EventArgs e ) {
            CurrentForm.RaiseEvent( SelectedIndexChangedMainRBList );
        }
        #endregion

        /// <summary>
        /// 検査情報行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstChkInfo_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundChkInfo( sender, e );
        }
        protected void lstChkInfo2_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundChkInfo2( sender, e );
        }

        /// <summary>
        /// 不具合一覧行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstNG_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundNGList( sender, e );
        }
        protected void lstNGRB_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundNGRBList( sender, e );
        }


        #endregion

        #region イベントメソッド

        #region ページイベント

        /// <summary>
        /// ページロード処理
        /// </summary>
        private void DoPageLoad() {

            if ( true == IsPostBack || true == ScriptManager.GetCurrent( Page ).IsInAsyncPostBack ) {
                //ポストバック時のGridViewの動的コントロールの復元
                SetMainGridViewBind();
            }
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize() {

            //画像エリアを非表示にしておく
            imgMainArea.Visible = false;
            imgMainArea2.Visible = false;

            //検索結果取得
            Business.DetailViewBusiness.ResultSetMulti res = new Business.DetailViewBusiness.ResultSetMulti();
            try {
                //検査情報ヘッダ取得
                if ( GeneralPatternCode.Tractor == DetailKeyParam.ProductKind ) {
                    //[トラクタ]イメージチェックシート
                    res = Business.DetailViewBusiness.SelectTractorImgCheckSheetHeader( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial );
                } else {
                    //[エンジン]電子チェックシート
                    res = Business.DetailViewBusiness.SelectEngineELCheckSheetHeader( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial );
                }
 
                //取得データをセッションに格納
                Dictionary<string, object> dicHeaderInfo = new Dictionary<string, object>();
                dicHeaderInfo.Add( SESSION_PAGE_INFO_DB_KEY, res );
                CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).SetPageControlInfo( MANAGE_ID_HEADER, dicHeaderInfo );

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

            //従業員情報の取得
            getEmpInfo();

            //従業員コードを設定
            res.MainTable = replaceEmpInfo( res.MainTable, "PASS_EMPLOYEE_CD" );
            res.MainTable = replaceEmpInfo( res.MainTable, "INS_START_BY" );

            //メイングリッドビューにデータバインド
            SetMainGridViewBind();

            grvMainViewLB.SelectedIndex = 0;
            grvMainViewRB.SelectedIndex = 0;

            //サブリストバインド
            SelectedIndexChangedMainList( grvMainViewLB.SelectedIndex );

        }
        #endregion

        #endregion

        #region Grid設定
        /// <summary>
        /// メイングリッドビュー バインド設定
        /// </summary>
        private void SetMainGridViewBind() {

            //セッションからヘッダデータを取得
            Dictionary<string, object> dicPageHeaderInfo = CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).GetPageControlInfo( MANAGE_ID_HEADER );
            Business.DetailViewBusiness.ResultSetMulti resultHeader = new Business.DetailViewBusiness.ResultSetMulti();
            if ( true == dicPageHeaderInfo.ContainsKey( SESSION_PAGE_INFO_DB_KEY ) ) {
                resultHeader = (Business.DetailViewBusiness.ResultSetMulti)dicPageHeaderInfo[SESSION_PAGE_INFO_DB_KEY];
            }
            if ( true == ObjectUtils.IsNull( resultHeader.MainTable ) || 0 == resultHeader.MainTable.Rows.Count ) {
                //取得済みのヘッダ情報がない場合は何もしない
                return;
            }

            //一覧をクリア
            grvMainHeaderLT.Columns.Clear();
            grvMainViewLB.Columns.Clear();
            grvMainHeaderRT.Columns.Clear();
            grvMainViewRB.Columns.Clear();

            //一覧表示列の設定
            GridViewDefine[] gridColumns;
            List<GridViewDefine> columns = new List<GridViewDefine>();
            columns.AddRange( MainControls );
            gridColumns = columns.ToArray();

            ConditionInfoSessionHandler.ST_CONDITION cond_main = new ConditionInfoSessionHandler.ST_CONDITION();
            cond_main.ResultData = resultHeader.MainTable;

            for ( int idx = 0; idx < gridColumns.Length; idx++ ) {
                TemplateField tf = new TemplateField();
                tf.HeaderText = StringUtils.ToString( gridColumns[idx].bindField );
                if ( idx <= FIXED_COLUMNS_NUM ) {
                    //固定列
                    grvMainHeaderLT.Columns.Add( tf );
                    grvMainViewLB.Columns.Add( tf );
                } else {
                    grvMainHeaderRT.Columns.Add( tf );
                    grvMainViewRB.Columns.Add( tf );
                }
            }

            /*  SELECTコマンド送信用ボタンが存在すると列表示がヘッダ行とデータ行でずれるため   *
             *  SELECTコマンド送信用ボタンを追加前のカラム定義でバインドする                   */
            //カラム定義を取得
            GridViewDefine[] GridDefineL = ControlUtils.GetFrozenColumns( grvMainViewLB, gridColumns, true );
            GridViewDefine[] GridDefineR = ControlUtils.GetFrozenColumns( grvMainViewLB, gridColumns, false );
            //SELECTコマンド送信用ボタンを追加
            CommandField cf = new CommandField();
            cf.ShowSelectButton = true;
            grvMainHeaderLT.Columns.Add( cf );
            grvMainViewLB.Columns.Add( cf );
            grvMainHeaderRT.Columns.Add( cf );
            grvMainViewRB.Columns.Add( cf );
            //メインリストバインド
            ControlUtils.ShowGridViewHeader( grvMainHeaderLT, GridDefineL, cond_main, false );
            ControlUtils.ShowGridViewHeader( grvMainHeaderRT, GridDefineR, cond_main, false );
            ControlUtils.BindGridView_WithTempField( grvMainViewLB, GridDefineL, cond_main.ResultData );
            ControlUtils.BindGridView_WithTempField( grvMainViewRB, GridDefineR, cond_main.ResultData );

        }

        /// <summary>
        /// グリッドビュー格納DIVサイズ調整
        /// </summary>
        private void SetDivGridViewWidth() {
            SetDivGridViewWidth( grvMainHeaderLT, divMainHeaderLT);
            SetDivGridViewWidth( grvMainHeaderRT, divMainHeaderRT );

            SetDivGridViewWidth( grvMainViewLB, divGrvLBMain );
            SetDivGridViewWidth( grvMainViewRB, divGrvRBMain );
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
            ControlUtils.InitializeGridView( grvMainHeaderLT, false );
            ControlUtils.InitializeGridView( grvMainHeaderRT, false );
            ControlUtils.InitializeGridView( grvMainViewLB, false );
            ControlUtils.InitializeGridView( grvMainViewRB, false );

            //グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();

            //GridView非表示
            divMainListArea.Visible = false;
        }
        #endregion

        #region リストバインド
        /// <summary>
        /// メインリストバインド処理（左）
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void ItemDataBoundMainLBList( params object[] parameters ) {
            //    object sender = parameters[0];
            //    ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            //    if ( e.Item.ItemType == ListViewItemType.DataItem ) {
            //        DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
            //        Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
            //        CurrentForm.SetControlValues( e.Item, MainControls, rowBind, ref dicSetValues );

            //        //クリックイベントセット
            //        HtmlTableRow trRow = (HtmlTableRow)e.Item.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
            //        Button selectBtn = (Button)e.Item.FindControl( GRID_MAIN.SELECT.controlId );
            //        trRow.Attributes[ControlUtils.ON_CLICK] = string.Format( MAIN_VIEW_SELECTED, e.Item.DataItemIndex, selectBtn.UniqueID );

            //        //ツールチップ設定
            //        ControlUtils.SetToolTip( e.Item );

            //    }
        }
        /// <summary>
        /// メインリストバインド処理（右）
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void ItemDataBoundMainRBList( params object[] parameters ) {
            //    object sender = parameters[0];
            //    ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            //    if ( e.Item.ItemType == ListViewItemType.DataItem ) {
            //        DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
            //        Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
            //        CurrentForm.SetControlValues( e.Item, MainControls, rowBind, ref dicSetValues );

            //        //クリックイベントセット
            //        HtmlTableRow trRow = (HtmlTableRow)e.Item.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
            //        Button selectBtn = (Button)e.Item.FindControl( GRID_MAIN.SELECT.controlId );
            //        trRow.Attributes[ControlUtils.ON_CLICK] = string.Format( MAIN_VIEW_SELECTED, e.Item.DataItemIndex, selectBtn.UniqueID );

            //        //ツールチップ設定
            //        ControlUtils.SetToolTip( e.Item );

            //    }
        }

        /// <summary>
        /// グリッドビュー行バインド(左下)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowDataBoundMainViewLB( params object[] parameters ) {
            GridView sender = (GridView)parameters[0];
            GridViewRowEventArgs e = (GridViewRowEventArgs)parameters[1];

            ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_MAIN_VIEW_GROUP_CD );

            // 空文字でない場合、表示するヘッダテキストとして保持する
            var ht = MainControls.Where( d => StringUtils.IsNotEmpty( d.headerText ) ).Select( d => d.headerText );
            for ( var cIndex = 0; cIndex < sender.Columns.Count; cIndex++ ) {
                if ( false == ht.Contains( sender.Columns[cIndex].HeaderText ) ) {
                    // 一覧のヘッダテキストが表示するヘッダテキストに含まれない場合、非表示を設定する
                    e.Row.Cells[cIndex].Style.Add( HtmlTextWriterStyle.Display, "none" );
                }
            }

            if ( e.Row.RowType == DataControlRowType.DataRow ) {
                DataRow row = ( (DataRowView)e.Row.DataItem ).Row;

                Dictionary<string, object> dicControls = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Row, ControlUtils.GetControlDefineArray( typeof( GRID_MAIN ) ), row, ref dicControls );

                //ポストバック追加
                e.Row.Attributes[ControlUtils.ON_CLICK] += Page.ClientScript.GetPostBackClientHyperlink( (GridView)sender, "Select$" + e.Row.RowIndex );
            }

        }

        /// <summary>
        /// グリッドビュー行バインド(右下)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowDataBoundMainViewRB( params object[] parameters ) {
            GridView sender = (GridView)parameters[0];
            GridViewRowEventArgs e = (GridViewRowEventArgs)parameters[1];

            ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_MAIN_VIEW_GROUP_CD );

            // 空文字でない場合、表示するヘッダテキストとして保持する
            var ht = MainControls.Where( d => StringUtils.IsNotEmpty( d.headerText ) ).Select( d => d.headerText );
            for ( var cIndex = 0; cIndex < sender.Columns.Count; cIndex++ ) {
                if ( false == ht.Contains( sender.Columns[cIndex].HeaderText ) ) {
                    // 一覧のヘッダテキストが表示するヘッダテキストに含まれない場合、非表示を設定する
                    e.Row.Cells[cIndex].Style.Add( HtmlTextWriterStyle.Display, "none" );
                }
            }

            if ( e.Row.RowType == DataControlRowType.DataRow ) {
                DataRow row = ( (DataRowView)e.Row.DataItem ).Row;
                Dictionary<string, object> dicControls = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Row, ControlUtils.GetControlDefineArray( typeof( GRID_MAIN ) ), row, ref dicControls );

                //ポストバック追加
                e.Row.Attributes[ControlUtils.ON_CLICK] += Page.ClientScript.GetPostBackClientHyperlink( (GridView)sender, "Select$" + e.Row.RowIndex );
            }

        }
        #endregion

        #region サブリストバインド
        /// <summary>
        /// サブリストバインド処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void ItemDataBoundChkInfo( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];
            HtmlTableRow trRow = new HtmlTableRow();

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {
                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();

                //検査情報
                CurrentForm.SetControlValues( e.Item, SubChkCtrl, rowBind, ref dicSetValues );
                trRow = (HtmlTableRow)e.Item.FindControl( GRID_CHK_INFO.TR_ROW_DATA.controlId );
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format( LIST_VIEW_SELECTED, e.Item.DataItemIndex, "" );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );
            }
        }
        /// <summary>
        /// サブリストバインド処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void ItemDataBoundChkInfo2( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];
            HtmlTableRow trRow = new HtmlTableRow();

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {
                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();

                //検査情報
                CurrentForm.SetControlValues( e.Item, SubChkCtrl, rowBind, ref dicSetValues );
                trRow = (HtmlTableRow)e.Item.FindControl( GRID_CHK_INFO.TR_ROW_DATA.controlId );
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format( LIST_VIEW_SELECTED, e.Item.DataItemIndex, "" );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );
            }

        }

        /// <summary>
        /// サブリストバインド処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void ItemDataBoundNGList( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];
            HtmlTableRow trRow = new HtmlTableRow();

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {
                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();

                //不具合一覧
                CurrentForm.SetControlValues( e.Item, SubNGListCtrl, rowBind, ref dicSetValues );
                trRow = (HtmlTableRow)e.Item.FindControl( GRID_NG_LIST.TR_ROW_DATA.controlId );
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format( NG_LIST_VIEW_SELECTED, e.Item.DataItemIndex, "" );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );
            }
        }
        /// <summary>
        /// サブリストバインド処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void ItemDataBoundNGRBList( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];
            HtmlTableRow trRow = new HtmlTableRow();

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {
                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();

                //検査情報
                CurrentForm.SetControlValues( e.Item, SubNGListCtrl, rowBind, ref dicSetValues );
                trRow = (HtmlTableRow)e.Item.FindControl( GRID_NG_LIST.TR_ROW_DATA.controlId );
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format( NG_LIST_VIEW_SELECTED, e.Item.DataItemIndex, "" );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );
            }
        }
        #endregion

        #region メインリスト選択行変更
        /// <summary>
        /// メインリスト選択行変更処理呼び出し（右）
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void SelectedIndexChangedMainRBList() {

            int selectedIndex = grvMainViewRB.SelectedIndex;
            grvMainViewLB.SelectedIndex = selectedIndex;

            SelectedIndexChangedMainList( selectedIndex );
        }
        /// <summary>
        /// メインリスト選択行変更処理呼び出し（左）
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void SelectedIndexChangedMainLBList() {

            int selectedIndex = grvMainViewLB.SelectedIndex;
            grvMainViewRB.SelectedIndex = selectedIndex;

            SelectedIndexChangedMainList( selectedIndex );

        }
        /// <summary>
        /// メインリスト選択行変更処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void SelectedIndexChangedMainList( int paramIndex ) {

            int mainIndex = paramIndex;

            //検索結果取得
            Business.DetailViewBusiness.ResultSetMulti resultHeader = new Business.DetailViewBusiness.ResultSetMulti();
            Dictionary<string, object> dicPageHeaderInfo = CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).GetPageControlInfo( MANAGE_ID_HEADER );
            if ( true == dicPageHeaderInfo.ContainsKey( SESSION_PAGE_INFO_DB_KEY ) ) {
                resultHeader = (Business.DetailViewBusiness.ResultSetMulti)dicPageHeaderInfo[SESSION_PAGE_INFO_DB_KEY];
            }

            //選択されたレコードのLINE_CDを取得
            string paramLineCd = StringUtils.ToString( resultHeader.MainTable.Rows[paramIndex][GRID_MAIN.LINE_CD.bindField] );

            //検索結果取得
            Business.DetailViewBusiness.ResultSetMulti res = new Business.DetailViewBusiness.ResultSetMulti();

            try {
                res = Business.DetailViewBusiness.SelectEngineELCheckSheetDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial, paramLineCd, DetailKeyParam.ProductKind );
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

            //取得データをセッションに格納
            Dictionary<string, object> dicPageControlInfo = new Dictionary<string, object>();
            dicPageControlInfo.Add( SESSION_PAGE_INFO_DB_KEY, res );
            CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).SetPageControlInfo( MANAGE_ID, dicPageControlInfo );
            CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).SetPageControlInfo( MANAGE_ID2, dicPageControlInfo );

            if ( 0 == res.MainTable.Rows.Count ) {
                //検索結果0件
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62010, CurrentUCInfo.title );
                return;
            }

            ///////////////////////////
            //  タブの設定
            ///////////////////////////
            getEmpInfo();

            //サブリスト初期化
            lstChkInfo.DataSource = null;
            lstChkInfo.DataBind();
            lstChkInfo2.DataSource = null;
            lstChkInfo2.DataBind();

            lstNG.DataSource = null;
            lstNG.DataBind();
            lstNGRB.DataSource = null;
            lstNGRB.DataBind();

            lstChkImageList.DataSource = null;
            lstChkImageList.DataBind();

            lstNGImageList.DataSource = null;
            lstNGImageList.DataBind();

            //検査一覧
            if ( res.SubTables[CHK_INFO].Rows.Count > 0 ) {
                //表示用整形
                res.SubTables[CHK_INFO] = replaceEmpInfo( res.SubTables[CHK_INFO], "EMPLOYEE_CD" );
            }
            lstChkInfo.DataSource = res.SubTables[CHK_INFO];
            lstChkInfo.DataBind();
            lstChkInfo2.DataSource = res.SubTables[CHK_INFO];
            lstChkInfo2.DataBind();

            //不具合一覧
            if ( res.SubTables[NG_LIST].Rows.Count > 0 ) {
                //不具合記入社員
                res.SubTables[NG_LIST] = replaceEmpInfo( res.SubTables[NG_LIST], "NG_UPDATE_EMPL_CD" );
                //手直し対応社員
                res.SubTables[NG_LIST] = replaceEmpInfo( res.SubTables[NG_LIST], "ADJUST_EMPL_CD" );
                //確認社員
                res.SubTables[NG_LIST] = replaceEmpInfo( res.SubTables[NG_LIST], "CONFIRM_EMPL_CD" );

                solidNGListHeaderLT.Visible = true;
                solidNGListHeaderRT.Visible = true;
            } else {
                solidNGListHeaderLT.Visible = false;
                solidNGListHeaderRT.Visible = false;
            }

            lstNG.DataSource = res.SubTables[NG_LIST];
            lstNG.DataBind();
            lstNGRB.DataSource = res.SubTables[NG_LIST];
            lstNGRB.DataBind();

            //検査画像
            InitializeCheckImage( res.SubTables[CHK_IMG] );

            //不具合画像
            InitializeNGImage( res.SubTables[NG_IMG] );

        }

        #endregion

        #region 不具合画像リストバインド

        /// <summary>
        /// イメージリスト行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstNGImageList_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundNGImageList( sender, e );
        }

        /// <summary>
        /// 不具合画像初期値設定
        /// </summary>
        private void InitializeNGImage( DataTable tblCameraImage ) {

            lstNGImageList.DataSource = tblCameraImage;
            lstNGImageList.DataBind();

            //表示データ制御

            if ( tblCameraImage.Rows.Count > 0 ) {
                //表示データが存在する場合
                imgMainArea.Visible = true;
            } else {
                //表示データが存在しない場合
                imgMainArea.Visible = false;
            }

            Dictionary<string, byte[]> dicImages = new Dictionary<string, byte[]>();
            for ( int loopImg = 0; loopImg < tblCameraImage.Rows.Count; loopImg++ ) {
                byte[] byteImage = new byte[0];

                if ( ObjectUtils.IsNotNull( tblCameraImage.Rows[loopImg][NG_IMG_LIST.IMAGE_DATA.bindField] ) ) {
                    byteImage = (byte[])tblCameraImage.Rows[loopImg][NG_IMG_LIST.IMAGE_DATA.bindField];
                }

                dicImages.Add( loopImg.ToString(), byteImage );
            }

            CurrentForm.SessionManager.GetImageInfoHandler( CurrentForm.Token ).SetImages( MANAGE_ID, dicImages );
        }

        /// <summary>
        /// イメージリストバインド処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void ItemDataBoundNGImageList( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {

                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Item, NGImgCtrl, rowBind, ref dicSetValues );

                //イメージ画像URLセット(サムネイル)
                Image imgThumbnail = ( (Image)e.Item.FindControl( NG_IMG_LIST.IMAGE_DATA.controlId ) );

                //先頭行選択済
                if ( 0 == e.Item.DataItemIndex ) {
                    //メイン画像エリア
                    HtmlGenericControl divCtrl = ( (HtmlGenericControl)e.Item.FindControl( NG_IMG_LIST.DIV_ROW_DATA.controlId ) );
                    divCtrl.Attributes["class"] = divCtrl.Attributes["class"] + " " + LIST_SELECTED_ITEM_CSS;
                    string urlMainAreaTop = ImageView.GetImageViewUrl( this, CurrentForm.Token, MANAGE_ID, e.Item.DataItemIndex, 0, 0
                        , TRC_W_PWT_ProductView.Session.ImageInfoSessionHandler.ContentType.Bitmap, false );
                    imgMainArea.ImageUrl = urlMainAreaTop;

                    //サムネイルエリア 先頭行
                    string urlThumbnail = ImageView.GetImageViewUrl( this, CurrentForm.Token, MANAGE_ID, e.Item.DataItemIndex, LIST_MAX_WIDTH, LIST_MAX_HEIGHT
                    , TRC_W_PWT_ProductView.Session.ImageInfoSessionHandler.ContentType.Bitmap, false );
                    imgThumbnail.ImageUrl = urlThumbnail;
                } else {
                    //先頭行以外はページロード後に画像読み込み
                    string urlThumbnail = ImageView.GetImageViewUrl( this, CurrentForm.Token, MANAGE_ID, e.Item.DataItemIndex, LIST_MAX_WIDTH, LIST_MAX_HEIGHT
                        , TRC_W_PWT_ProductView.Session.ImageInfoSessionHandler.ContentType.Bitmap, true );
                    imgThumbnail.ImageUrl = ResourcePath.Image.DummyLoad;
                    imgThumbnail.Attributes[ResourcePath.Image.AttrOriginalSrc] = urlThumbnail;
                }

                //クリックイベントセット
                string urlMainArea = ImageView.GetImageViewUrl( this, CurrentForm.Token, MANAGE_ID, e.Item.DataItemIndex, 0, 0
                    , TRC_W_PWT_ProductView.Session.ImageInfoSessionHandler.ContentType.Bitmap, true );
                imgThumbnail.Attributes[ControlUtils.ON_CLICK] = String.Format( CHANGE_MAIN_AREA_VIEW, imgMainArea.ClientID, urlMainArea );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );

            }
        }
        #endregion

        #region 検査画像リストバインド

        /// <summary>
        /// イメージリスト行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstChkImageList_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundChkImageList( sender, e );
        }

        /// <summary>
        /// カメラ画像初期値設定
        /// </summary>
        private void InitializeCheckImage( DataTable tblCameraImage ) {

            lstChkImageList.DataSource = tblCameraImage;
            lstChkImageList.DataBind();

            //表示データ制御
            if ( tblCameraImage.Rows.Count > 0 ) {
                //表示データが存在する場合
                imgMainArea2.Visible = true;
            } else {
                //表示データが存在しない場合
                imgMainArea2.Visible = false;
            }

            Dictionary<string, byte[]> dicImages = new Dictionary<string, byte[]>();
            for ( int loopImg = 0; loopImg < tblCameraImage.Rows.Count; loopImg++ ) {
                byte[] byteImage = new byte[0];

                if ( ObjectUtils.IsNotNull( tblCameraImage.Rows[loopImg][CHK_IMG_LIST.IMAGE_DATA.bindField] ) ) {
                    byteImage = (byte[])tblCameraImage.Rows[loopImg][CHK_IMG_LIST.IMAGE_DATA.bindField];
                }

                dicImages.Add( loopImg.ToString(), byteImage );
            }

           CurrentForm.SessionManager.GetImageInfoHandler( CurrentForm.Token ).SetImages( MANAGE_ID2, dicImages );
        }

        /// <summary>
        /// 検査イメージリストバインド処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void ItemDataBoundChkImageList( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {

                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Item, ChkImgCtrl, rowBind, ref dicSetValues );

                //イメージ画像URLセット(サムネイル)
                Image imgThumbnail2 = ( (Image)e.Item.FindControl( CHK_IMG_LIST.IMAGE_DATA.controlId ) );

                //先頭行選択済
                if ( 0 == e.Item.DataItemIndex ) {
                    //メイン画像エリア
                    HtmlGenericControl divCtrl = ( (HtmlGenericControl)e.Item.FindControl( CHK_IMG_LIST.DIV_ROW_DATA.controlId ) );
                    divCtrl.Attributes["class"] = divCtrl.Attributes["class"] + " " + LIST_SELECTED_ITEM_CSS;
                    string urlMainAreaTop = ImageView.GetImageViewUrl( this, CurrentForm.Token, MANAGE_ID2, e.Item.DataItemIndex, 0, 0
                        , TRC_W_PWT_ProductView.Session.ImageInfoSessionHandler.ContentType.Bitmap, false );
                    imgMainArea2.ImageUrl = urlMainAreaTop;

                    //サムネイルエリア 先頭行
                    string urlThumbnail = ImageView.GetImageViewUrl( this, CurrentForm.Token, MANAGE_ID2, e.Item.DataItemIndex, LIST_MAX_WIDTH, LIST_MAX_HEIGHT
                    , TRC_W_PWT_ProductView.Session.ImageInfoSessionHandler.ContentType.Bitmap, false );
                    imgThumbnail2.ImageUrl = urlThumbnail;
                } else {
                    //先頭行以外はページロード後に画像読み込み
                    string urlThumbnail = ImageView.GetImageViewUrl( this, CurrentForm.Token, MANAGE_ID2, e.Item.DataItemIndex, LIST_MAX_WIDTH, LIST_MAX_HEIGHT
                        , TRC_W_PWT_ProductView.Session.ImageInfoSessionHandler.ContentType.Bitmap, true );
                    imgThumbnail2.ImageUrl = ResourcePath.Image.DummyLoad;
                    imgThumbnail2.Attributes[ResourcePath.Image.AttrOriginalSrc] = urlThumbnail;
                }
                //クリックイベントセット
                string urlMainArea = ImageView.GetImageViewUrl( this, CurrentForm.Token, MANAGE_ID2, e.Item.DataItemIndex, 0, 0
                    , TRC_W_PWT_ProductView.Session.ImageInfoSessionHandler.ContentType.Bitmap, true );
                imgThumbnail2.Attributes[ControlUtils.ON_CLICK] = String.Format( CHANGE_MAIN_AREA_VIEW2, imgMainArea2.ClientID, urlMainArea );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );
            }
        }
        #endregion

        #region 従業員処理
        /// <summary>
        /// 画面表示用従業員情報の取得
        /// </summary>
        private void getEmpInfo() {
            DataTable tmp = new DataTable();
            try {
                //従業員情報を取得
                tmp = Business.DetailViewBusiness.SelectEmpInfo( null, null );
                _dicEmp = new Dictionary<string,string>();

                foreach ( DataRow dr in tmp.Rows ) {
                    _dicEmp.Add( dr["EMP_NO"].ToString().Trim(), dr["EMP_NO"].ToString().Trim() + ":" + dr["EMP_NM"].ToString().Trim() );
                }

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

        }


        /// <summary>
        /// 指定カラムの値(従業員名)変更
        /// </summary>
        /// <param name="dt">名称変更対象DataTable</param>
        /// <param name="strCol">対象カラム</param>
        /// <returns></returns>
        private  DataTable replaceEmpInfo( DataTable dt,string strCol ) {

            foreach ( DataRow dtRow in dt.Rows ) {

                //対象カラムに値がある場合        
                if ( StringUtils.IsNotEmpty( StringUtils.ToString( dtRow[strCol] ) ) ){
                    string defaultValue = StringUtils.ToString( dtRow[strCol] );
                    if (_dicEmp.TryGetValue( defaultValue, out defaultValue) == true ) {
                        //キーが存在する場合、「コード＋名称」で上書きする
                        dtRow[strCol] = _dicEmp[StringUtils.ToString( dtRow[strCol] )];
                    }
                }
            }

            return dt;
        }
        #endregion

        #region 印刷処理
        /// <summary>
        /// 電子チェックシート帳票出力処理
        /// </summary>
        /// <remarks>レイアウトの項目制御＋帳票マージが必要なため、レイアウトにデータを書き込む方式で処理を作成。
        ///          simpleで単純な帳票の場合はDataSourceにデータをsetする方式で処理を作成すること
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <history>
        /// 2015/11/17  保証項目の改行をspaceに置換する(帳票レイアウトが崩れるため)＋改ページ制御不具合修正
        /// 2016/02/10  表示文字数(byte数)に合わせて枠が拡張されるようにcontrol修正
        /// </history>
        protected void btnPrint_Click( object sender, EventArgs e ) {

            //コンフィグ設定読み込み
            ConfigData configData = WebAppInstance.GetInstance().Config;
            string repFileName = string.Empty;
            string outputFileName = string.Empty;
            if ( GeneralPatternCode.Tractor == DetailKeyParam.ProductKind ) {
                //トラクタイメージチェックシート
                repFileName = configData.ApplicationInfo.repTractorImgCheckSheet;
                outputFileName = "REP_IMGCHK_";
            } else {
                //エンジン電子チェックシート
                repFileName = configData.ApplicationInfo.repEngineELCheckSheet;
                outputFileName = "REP_ELCHK_";
            }
            string repFilePath = configData.ApplicationInfo.reportTemplateBasePath + repFileName;
            string repTemporaryPath = configData.ApplicationInfo.temporaryBasePath;
            string mainFileNm = outputFileName + DateTime.Now.ToString( "yyyyMMddHHmmssfff" ) + ".pdf";  //出力ファイル名

            C1Report report = new C1Report();
            C1Report repNGlist = new C1Report();

            try {

                report.Load( repFilePath, "Result" );

                int selectedIdx = 0;                            //index
                int const_height = 280;                         //枠の高さ(明細)
                string empNm = "";                              //従業員名
                string lineCd = "";                             //ラインコード
                string sectionNm = "";                          //課名
                string strGuaranteeItemNM = "";
                string orderNo = "";                            //順序連番

                //電子チェクシートのセッションから取得
                Business.DetailViewBusiness.ResultSetMulti res = new Business.DetailViewBusiness.ResultSetMulti();
                Dictionary<string, object> dicPageControlInfo = CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).GetPageControlInfo( MANAGE_ID );

                #region 検査情報ヘッダ

                if ( true == dicPageControlInfo.ContainsKey( SESSION_PAGE_INFO_DB_KEY ) ) {
                    res = (Business.DetailViewBusiness.ResultSetMulti)dicPageControlInfo[SESSION_PAGE_INFO_DB_KEY];

                    DataRow row = res.MainTable.Rows[selectedIdx];

                    //ラインコード、ライン名
                    report.Fields["LINE_NM"].Calculated = false;
                    report.Fields["LINE_NM"].Text = StringUtils.ToString( row["LINE_NM"] );
                    lineCd = StringUtils.ToString( row["LINE_NM"] );
                    //順序番号
                    report.Fields["ORDER_NO"].Calculated = false;
                    if ( GeneralPatternCode.Tractor == DetailKeyParam.ProductKind ) {
                        //トラクタイメージチェックシート
                        //確定順序連番
                        orderNo = StringUtils.ToString( row["DECISION_NO"] );
                    } else {
                        //エンジン電子チェックシート
                        //投入順序連番
                        orderNo = StringUtils.ToString( row["ORDER_NO"] );
                    }
                    if( LENGTH_ORDER_NO == orderNo.Length ) {
                        orderNo = orderNo.Substring( 7, 5 );
                    }
                    report.Fields["ORDER_NO"].Text = orderNo;
                    //検査終了日時
                    report.Fields["INS_END_DT"].Calculated = false;
                    //合格日
                    if ( ObjectUtils.IsNotNull( row["INS_END_DT"] ) ) {
                        report.Fields["INS_END_DT"].Text = StringUtils.ToString( row["INS_END_DT"] );

                        //合格印日付
                        DateTime endDt = DateUtils.ToDate( row["INS_END_DT"] );
                        report.Fields["lblPicPassDt"].Calculated = false;
                        report.Fields["lblPicPassDt"].Text = endDt.ToShortDateString();
                        report.Fields["lblPicPassDt"].Font.Bold = true;

                        //課名取得
                        lineCd = lineCd.Substring( 0, 6 );
                        DataTable dtSection = CommonProcessDao.SelectSection(lineCd, DateTime.Parse(row["INS_START_DT"].ToString()));
                        sectionNm = StringUtils.ToString( dtSection.Rows[0]["SECTION_SHORT_NM"] );
                        report.Fields["SECTION_NM"].Calculated = false;
                        report.Fields["SECTION_NM"].Text = sectionNm;
                        report.Fields["SECTION_NM"].Font.Bold = true;

                        //従業員名
                        empNm = "";
                        if ( 0 < _dicEmp.Count ) {
                            string tempNm = StringUtils.ToString( row["PASS_EMPLOYEE_CD"] );
                            if (true == _dicEmp.TryGetValue( tempNm, out tempNm ) ) {
                                empNm = _dicEmp[StringUtils.ToString( row["PASS_EMPLOYEE_CD"] )];
                                empNm = empNm.Substring( empNm.IndexOf( ":" ) + 1 );
                            }else {
                                empNm = StringUtils.ToString( row["PASS_EMPLOYEE_CD"] );
                            }                               
                        }
                        report.Fields["PASS_EMPLOYEE_CD"].Calculated = false;
                        report.Fields["PASS_EMPLOYEE_CD"].Text = empNm;

                    } else {
                        report.Fields["lblPassPicture"].Picture = null;
                    }
                }


                //表示データをセッションから取得
                Dictionary<string, object> dicTmp = CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).GetPageControlInfo( PageInfo.DetailFrame.pageId );
                Dictionary<string, object> dicPrintInfo = (Dictionary<string, object>)dicTmp[Defines.Session.DetailFrame.SESSION_PAGE_INFO_MODEL_INFO_KEY];

                //生産型式
                report.Fields["lblProdModelCd"].Calculated = false;
                report.Fields["lblProdModelCd"].Text = DataUtils.GetModelCdStr( StringUtils.ToString( DetailKeyParam.ProductModelCd ) );
                //IDNO
                report.Fields["lblIDNO"].Calculated = false;
                report.Fields["lblIDNO"].Text = StringUtils.ToString( DetailKeyParam.Idno );
                //機番
                report.Fields["lblSerial"].Calculated = false;
                report.Fields["lblSerial"].Text = StringUtils.ToString( DetailKeyParam.Serial );
                //生産型式名
                report.Fields["lblProdModelNm"].Calculated = false;
                report.Fields["lblProdModelNm"].Text = DataUtils.GetDictionaryStringVal( dicPrintInfo, "productModelNm" );
                //完成予定日
                report.Fields["lblYoteiDt"].Calculated = false;
                if ( StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( dicPrintInfo, "planDt" ) ) ) {
                    DateTime dtPlan = DataUtils.GetDictionaryDateVal( dicPrintInfo, "planDt" );
                    report.Fields["lblYoteiDt"].Text = dtPlan.ToShortDateString();
                }else{
                    report.Fields["lblYoteiDt"].Text = "--------";
                }
                //完成日
                report.Fields["lblKanseiDt"].Calculated = false;
                if ( StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( dicPrintInfo, "productDt" ) ) ) {
                    DateTime dtProduct = DataUtils.GetDictionaryDateVal( dicPrintInfo, "productDt" );
                    report.Fields["lblKanseiDt"].Text = dtProduct.ToShortDateString();
                }else{
                    report.Fields["lblKanseiDt"].Text = "--------";
                }
                #endregion

                if ( true == dicPageControlInfo.ContainsKey( SESSION_PAGE_INFO_DB_KEY ) ) {
                    res = (Business.DetailViewBusiness.ResultSetMulti)dicPageControlInfo[SESSION_PAGE_INFO_DB_KEY];

                    #region 検査情報明細

                    //表示データ加工
                    DataTable dtTmp = new DataTable();
                    dtTmp.Columns.Add( "PROD_PROC" );
                    dtTmp.Columns.Add( "GUARANTEE_NO" );
                    dtTmp.Columns.Add( "CRIT_KIND" );
                    dtTmp.Columns.Add( "GUARANTEE_ITEM_NM" );
                    dtTmp.Columns.Add( "PASS_STATUS" );

                    foreach ( DataRow dtRow in res.SubTables[CHK_INFO].Rows ) {
                        DataRow row = dtTmp.NewRow();
                        row["PROD_PROC"] = " 工程：" + StringUtils.ToString( dtRow[GRID_CHK_INFO.PROD_PROC.bindField] );
                        row["GUARANTEE_NO"] = StringUtils.ToString( dtRow[GRID_CHK_INFO.GUARANTEE_NO.bindField] );
                        row["CRIT_KIND"] = StringUtils.ToString( dtRow[GRID_CHK_INFO.CRIT_KIND.bindField] );
                        strGuaranteeItemNM = StringUtils.ToString( dtRow[GRID_CHK_INFO.GUARANTEE_ITEM_NM.bindField] );
                        string ItemNm = strGuaranteeItemNM.Replace( "\r", "" ).Replace( "\n", " " );
                        //入力値の制御
                        string value1 = StringUtils.ToString( dtRow[GRID_CHK_INFO.INPUT_VALUE1.bindField] );
                        string value2 = StringUtils.ToString( dtRow[GRID_CHK_INFO.INPUT_VALUE2.bindField] );

                        if ( StringUtils.IsNotEmpty( value1 ) | StringUtils.IsNotEmpty( value2 ) ) {
                            if ( StringUtils.IsNotEmpty( value1 ) ) {
                                ItemNm = ItemNm + "【" + value1 + "】";
                            }
                            if ( StringUtils.IsNotEmpty( value2 ) ) {
                                ItemNm = ItemNm + "【" + value2 + "】";
                            }
                        }
                        row["GUARANTEE_ITEM_NM"] = ItemNm;
                        row["PASS_STATUS"] = StringUtils.ToString( dtRow[GRID_CHK_INFO.PASS_STATUS.bindField] );

                        dtTmp.Rows.Add( row );
                    }
                    dtTmp.AcceptChanges();



                    ///////////////////////////
                    //      検査情報
                    ///////////////////////////

                    //検査情報
                    int defTop = 0;
                    int defLeft = 0;
                    int const_left = 5100;
                    int rowCnt = 0;
                    string procTitle = "";
                    bool blItem = false;
                    int pageDefTop = 0;
                    int maxRowCount = 88;

                    C1.C1Report.Section sDtl = report.Sections[C1.C1Report.SectionTypeEnum.Detail];

                    foreach ( DataRow tmpRow in dtTmp.Rows ) {

                        //工程名チェック
                        if ( procTitle.Equals( StringUtils.ToString( tmpRow["PROD_PROC"] ) ) ) {
                            //工程が同じ
                            blItem = true;
                        } else {
                            blItem = false;
                        }
                        procTitle = StringUtils.ToString( tmpRow["PROD_PROC"] );

                        if ( false == ( blItem ) ) {
                            //工程名設定

                            Field titlef = new Field();
                            titlef = sDtl.Fields.Add( "PROD_PROC", procTitle, defLeft, defTop, 4880, const_height );
                            DefineFixValues( titlef, 8 );
                            titlef.Align = FieldAlignEnum.LeftMiddle;
                            titlef.BackColor = System.Drawing.Color.PaleGreen;

                            defTop += 280;
                            rowCnt++;

                            if ( rowCnt % maxRowCount == 0 ) {
                                defLeft = 0;

                                //改ページ挿入
                                CreateBrakeField( sDtl, defTop );
                                defTop += 1;

                                pageDefTop = defTop;
                                rowCnt = 0;
                            } else if ( rowCnt % 44 == 0 ) {
                                defTop = pageDefTop;
                                //defTop = 0;
                                defLeft = const_left;
                            }
                        }

                        //byte数取得
                        string strItem = StringUtils.ToString( tmpRow[GRID_CHK_INFO.GUARANTEE_ITEM_NM.bindField] );
                        int intItem = Encoding.GetEncoding( 932 ).GetByteCount( strItem );
                        int rowRate = intItem / 116;
                        rowRate++;

                        if ( maxRowCount < ( rowCnt + rowRate ) ) {
                            //行数が足りない場合は空白で改行
                            //ダミー設定
                            Field fCtrl = new Field();
                            fCtrl = sDtl.Fields.Add( "rowCtrl", "", defLeft, defTop, 4880, ( maxRowCount - rowCnt ) * const_height );
                            DefineFixValues( fCtrl );

                            //斜線設定
                            Field fSlant = new Field();
                            fSlant = sDtl.Fields.Add( "slant", "", defLeft, defTop, 4880, ( maxRowCount - rowCnt ) * const_height );
                            CreateSlantField( fSlant );

                            defLeft = 0;
                            defTop += 280 * rowRate;

                            //改ページ挿入
                            CreateBrakeField( sDtl, defTop );
                            defTop += 1;

                            pageDefTop = defTop;
                            rowCnt = 0;

                        } else if ( ( defLeft.Equals(0) ) && ( 44 < ( rowCnt + rowRate ) ) ) {
                            //左の行では足りないので、空白で右行へ。
                            Field fLeft = new Field();
                            fLeft = sDtl.Fields.Add( "rowCtrl", "", defLeft, defTop, 4880, ( 44 - rowCnt ) * const_height );
                            DefineFixValues( fLeft );

                            //斜線
                            Field fSlant = new Field();
                            fSlant = sDtl.Fields.Add( "slant", "", defLeft, defTop, 4880, ( 44 - rowCnt ) * const_height );
                            CreateSlantField( fSlant );

                            defTop = pageDefTop;
                            defLeft = const_left;
                            rowCnt = 44;
                        }

                        //保証項目設定
                        Field f = new Field();

                        //GUARANTEE_NO
                        f = sDtl.Fields.Add( "GUARANTEE_NO", StringUtils.ToString( tmpRow[GRID_CHK_INFO.GUARANTEE_NO.bindField] ), defLeft, defTop, 280, const_height * rowRate );
                        DefineFixValues( f );
                        f.Align = FieldAlignEnum.CenterMiddle;

                        //CRIT_KIND
                        f = sDtl.Fields.Add( "CRIT_KIND", StringUtils.ToString( tmpRow[GRID_CHK_INFO.CRIT_KIND.bindField] ), defLeft + 280, defTop, 300, const_height * rowRate );
                        DefineFixValues( f );
                        f.Align = FieldAlignEnum.CenterMiddle;

                        //GUARANTEE_ITEM_NM
                        f = sDtl.Fields.Add( "GUARANTEE_ITEM_NM", StringUtils.ToString( tmpRow[GRID_CHK_INFO.GUARANTEE_ITEM_NM.bindField] ), defLeft + 580, defTop, 4000, const_height * rowRate );
                        DefineFixValues( f );
                        f.Align = FieldAlignEnum.LeftMiddle;
                        f.MarginLeft = 50;

                        //PASS_STATUS
                        f = sDtl.Fields.Add( "PASS_STATUS", StringUtils.ToString( tmpRow[GRID_CHK_INFO.PASS_STATUS.bindField] ), defLeft + 4580, defTop, 300, const_height * rowRate );
                        DefineFixValues( f );
                        f.Align = FieldAlignEnum.CenterMiddle;
                        string strStatus = StringUtils.ToString( tmpRow[GRID_CHK_INFO.PASS_STATUS.bindField] );
                        if ( strStatus.Equals( "NG" ) ) {
                            f.BackColor = System.Drawing.Color.Black;
                            f.ForeColor = System.Drawing.Color.White;
                            f.Font.Bold = true;
                        }

                        defTop += 280 * rowRate;
                        rowCnt = rowCnt + rowRate;

                        //改ページ制御
                        if ( rowCnt % maxRowCount == 0 ) {
                            defLeft = 0;

                            //改ページ挿入
                            CreateBrakeField( sDtl, defTop );
                            defTop += 1;

                            pageDefTop = defTop;
                            rowCnt = 0;

                        } else if ( rowCnt % 44 == 0 ) {
                            defTop = pageDefTop;
                            defLeft = const_left;
                        }
                    }

                    //紙の末端までデータがなかった場合の空行追加処理
                    if ( false == ( rowCnt % maxRowCount == 0 ) ) {

                        if ( rowCnt < 44 ) {
                            //空白枠
                            Field fLeft = new Field();
                            fLeft = sDtl.Fields.Add( "", "", defLeft, defTop, 4880, ( 44 - rowCnt ) * const_height );
                            DefineFixValues( fLeft );

                            //斜線
                            Field fSlantLeft = new Field();
                            fSlantLeft = sDtl.Fields.Add( "slant", "", defLeft, defTop, 4880, ( 44 - rowCnt ) * const_height );
                            CreateSlantField( fSlantLeft );

                            defTop = pageDefTop;
                            defLeft = const_left;
                            rowCnt = 44;
                        }

                        //空白枠
                        Field f = new Field();
                        f = sDtl.Fields.Add( "", "", defLeft, defTop, 4880, ( maxRowCount - rowCnt ) * const_height );
                        DefineFixValues( f );

                        //斜線
                        Field fSlant = new Field();
                        fSlant = sDtl.Fields.Add( "slant", "", defLeft, defTop, 4880, ( maxRowCount - rowCnt ) * const_height );
                        CreateSlantField( fSlant );

                    }
                    #endregion

                    #region 不具合情報
                    ///////////////////////////
                    //      不具合情報
                    ///////////////////////////
                    if ( 0 < res.SubTables[NG_LIST].Rows.Count ) {

                        C1.C1Report.Section sNgDtl = repNGlist.Sections[C1.C1Report.SectionTypeEnum.Detail];

                        DataRow row = res.MainTable.Rows[selectedIdx];

                        //DataTableから表示データを設定する
                        repNGlist.Load( repFilePath, "NGlist" );

                        #region 不具合一覧ヘッダ
                        ////////////////////////////////
                        //  ヘッダ部の設定
                        ////////////////////////////////

                        //ラインコード、ライン名
                        repNGlist.Fields["LINE_NM"].Calculated = false;
                        repNGlist.Fields["LINE_NM"].Text = StringUtils.ToString( row["LINE_NM"] );
                        //順序番号
                        repNGlist.Fields["ORDER_NO"].Calculated = false;
                        repNGlist.Fields["ORDER_NO"].Text = orderNo;
                        //検査終了日時
                        repNGlist.Fields["INS_END_DT"].Calculated = false;
                        //合格日
                        if ( ObjectUtils.IsNotNull( row["INS_END_DT"] ) ) {
                            repNGlist.Fields["INS_END_DT"].Text = StringUtils.ToString( row["INS_END_DT"] );

                            //合格印日付
                            DateTime endDt = DateUtils.ToDate( row["INS_END_DT"] );
                            repNGlist.Fields["lblPicPassDt"].Calculated = false;
                            repNGlist.Fields["lblPicPassDt"].Text = endDt.ToShortDateString();
                            repNGlist.Fields["lblPicPassDt"].Font.Bold = true;

                            //課名
                            repNGlist.Fields["SECTION_NM"].Calculated = false;
                            repNGlist.Fields["SECTION_NM"].Text = sectionNm;
                            repNGlist.Fields["SECTION_NM"].Font.Bold = true;

                            //従業員名
                            empNm = "";
                            if ( 0 < _dicEmp.Count ) {
                                string tempNm = StringUtils.ToString( row["PASS_EMPLOYEE_CD"] );
                                if ( true == _dicEmp.TryGetValue( tempNm, out tempNm ) ) {
                                    empNm = _dicEmp[StringUtils.ToString( row["PASS_EMPLOYEE_CD"] )];
                                    empNm = empNm.Substring( empNm.IndexOf( ":" ) + 1 );
                                } else {
                                    empNm = StringUtils.ToString( row["PASS_EMPLOYEE_CD"] );
                                }
                            }
                            repNGlist.Fields["PASS_EMPLOYEE_CD"].Calculated = false;
                            repNGlist.Fields["PASS_EMPLOYEE_CD"].Text = empNm;

                        } else {
                            repNGlist.Fields["lblPassPicture"].Picture = null;
                        }

                        //表示データをセッションから取得
                        //生産型式
                        repNGlist.Fields["lblProdModelCd"].Calculated = false;
                        repNGlist.Fields["lblProdModelCd"].Text = DataUtils.GetModelCdStr( StringUtils.ToString( DetailKeyParam.ProductModelCd ) );
                        //IDNO
                        repNGlist.Fields["lblIDNO"].Calculated = false;
                        repNGlist.Fields["lblIDNO"].Text = StringUtils.ToString( DetailKeyParam.Idno );
                        //機番
                        repNGlist.Fields["lblSerial"].Calculated = false;
                        repNGlist.Fields["lblSerial"].Text = StringUtils.ToString( DetailKeyParam.Serial );
                        //生産型式名
                        repNGlist.Fields["lblProdModelNm"].Calculated = false;
                        repNGlist.Fields["lblProdModelNm"].Text = DataUtils.GetDictionaryStringVal( dicPrintInfo, "productModelNm" );
                        //完成予定日
                        repNGlist.Fields["lblYoteiDt"].Calculated = false;
                        if ( StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( dicPrintInfo, "planDt" ) ) ) {
                            DateTime dtPlan = DataUtils.GetDictionaryDateVal( dicPrintInfo, "planDt" );
                            repNGlist.Fields["lblYoteiDt"].Text = dtPlan.ToShortDateString();
                        } else {
                            repNGlist.Fields["lblYoteiDt"].Text = "--------";
                        }
                        //完成日
                        repNGlist.Fields["lblKanseiDt"].Calculated = false;
                        if ( StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( dicPrintInfo, "productDt" ) ) ) {
                            DateTime dtProduct = DataUtils.GetDictionaryDateVal( dicPrintInfo, "productDt" );
                            repNGlist.Fields["lblKanseiDt"].Text = dtProduct.ToShortDateString();
                        }else{
                            repNGlist.Fields["lblKanseiDt"].Text = "--------";
                        }

                        #endregion

                        #region 不具合一覧明細
                        ////////////////////////////////
                        //  明細部の設定
                        ////////////////////////////////
                        procTitle = "xxx";
                        defLeft = 0;
                        defTop = 0;
                        rowCnt = 0;
                        long dataCnt = 0;
                        int resultRate = 0;
                        int inputRate = 0;
                        string guaranteeNo = "0";
                        bool blnGuarantee = false;

                        foreach ( DataRow dtRow in res.SubTables[NG_LIST].Rows ) {

                            //default
                            resultRate = 3;

                            //////////////////////////////////////////////////////
                            //  項目設定前の値チェック
                            //////////////////////////////////////////////////////
                            //工程名チェック
                            if ( procTitle.Equals( StringUtils.ToString( dtRow["PROD_PROC"] ) ) ) {
                                //工程が同じ
                                blItem = true;
                            } else {
                                blItem = false;
                            }
                            procTitle = StringUtils.ToString( dtRow["PROD_PROC"] );

                            //保証項目チェック
                            if ( guaranteeNo.Equals( StringUtils.ToString( dtRow[GRID_NG_LIST.GUARANTEE_NO.bindField] ) ) ) {
                                //保証項目が同じ
                                blnGuarantee = true;
                            }else{
                                blnGuarantee = false;
                            }
                            guaranteeNo = StringUtils.ToString( dtRow[GRID_NG_LIST.GUARANTEE_NO.bindField] );

                            //桁数チェック
                            string NGDtl = StringUtils.ToString( dtRow[GRID_NG_LIST.NG_DTL.bindField] );
                            string AdjDtl = StringUtils.ToString( dtRow[GRID_NG_LIST.ADJUST_DTL.bindField] );
                            string inputValue = StringUtils.ToString( dtRow["INPUT_VALUE1"] ) + StringUtils.ToString( dtRow["INPUT_VALUE2"] );

                            //byte数取得
                            int intNGDtl = Encoding.GetEncoding( 932 ).GetByteCount( NGDtl );
                            int intAdjDtl = Encoding.GetEncoding( 932 ).GetByteCount( AdjDtl );
                            int intInputValue = Encoding.GetEncoding( 932 ).GetByteCount( inputValue );

                            //不具合内部品と修正内容は、byte数が多い順に行の幅調整
                            if ( 184 < intAdjDtl ) {
                                //修正内容
                                resultRate = 5;
                            } else if ( 100 < intNGDtl ) {
                                resultRate = 4;
                            } else if ( 144 < intAdjDtl ) {
                                resultRate = 4;
                            }

                            //入力値の行調整
                            if (intInputValue <= 72){
                                //処理なし
                            }else{
                                inputRate = intInputValue / 24;
                                inputRate++;
                                if ( ObjectUtils.IsNotNull( dtRow["INPUT_VALUE2"] ) ) {
                                    //入力値2がある場合は改行するので更に1行追加
                                    inputRate++;
                                }
                            }

                            //入力値の方が大きい場合、入力値の行数を採用する
                            if ( resultRate < inputRate ) {
                                resultRate = inputRate;
                            }

                            if ( false == ( blItem ) ) {
                                //工程名設定

                                Field titlef = new Field();
                                if ( StringUtils.IsNotEmpty( procTitle ) ) {
                                    titlef = sNgDtl.Fields.Add( "PROD_PROC", " 工程：" + procTitle, defLeft, defTop, 9984, const_height );
                                }else{
                                    titlef = sNgDtl.Fields.Add( "PROD_PROC", " 追加不具合", defLeft, defTop, 9984, const_height );
                                }
                                DefineFixValues( titlef, 8 );
                                titlef.Align = FieldAlignEnum.LeftMiddle;
                                titlef.BackColor = System.Drawing.Color.PaleGreen;

                                defTop += 280;
                                rowCnt++;

                                if ( rowCnt % 44 == 0 ) {
                                    //改ページ挿入
                                    CreateBrakeField( sNgDtl, defTop );
                                    defTop = 0;
                                }
                            }

                            //保証項目設定
                            Field f = new Field();

                            if ( false == ( blnGuarantee ) ) {

                                //行数が足りない場合は空白で改行
                                strGuaranteeItemNM = StringUtils.ToString( dtRow[GRID_NG_LIST.GUARANTEE_NM.bindField] ).Replace( "\r", "" ).Replace( "\n", " " );

                                //byte数取得
                                int intItemNm = Encoding.GetEncoding( 932 ).GetByteCount( strGuaranteeItemNM );
                                int ItemRate = intItemNm / 276;
                                ItemRate++;

                                if ( 44 < ( rowCnt + ItemRate ) ) {
                                    //改ページ挿入
                                    CreateBrakeField( sNgDtl, defTop );
                                    defTop += 1;
                                    rowCnt = 0;
                                }

                                //GUARANTEE_NO
                                f = sNgDtl.Fields.Add( "GUARANTEE_NO", StringUtils.ToString( dtRow[GRID_NG_LIST.GUARANTEE_NO.bindField] ), 0, defTop, 280, const_height * ItemRate );
                                DefineFixValues( f );
                                f.Align = FieldAlignEnum.CenterMiddle;

                                //CRIT_KIND
                                f = sNgDtl.Fields.Add( "CRIT_KIND", StringUtils.ToString( dtRow[GRID_NG_LIST.CRIT_KIND.bindField] ), 280, defTop, 300, const_height * ItemRate );
                                DefineFixValues( f );
                                f.Align = FieldAlignEnum.CenterMiddle;

                                //GUARANTEE_ITEM_NM
                                f = sNgDtl.Fields.Add( "GUARANTEE_ITEM_NM", strGuaranteeItemNM, 580, defTop, 9404, const_height * ItemRate );
                                DefineFixValues( f );
                                f.Align = FieldAlignEnum.LeftMiddle;
                                f.MarginLeft = 50;

                                defTop += 280 * ItemRate;
                                rowCnt = rowCnt + ItemRate;

                            }

                            //////////////////////////////////////////////////////
                            //  帳票の枠設定
                            //////////////////////////////////////////////////////

                            //RECORD_NO(title)
                            f = sNgDtl.Fields.Add( "recNm", "連番", 0, defTop, 280, const_height * resultRate );
                            DefineFixValues( f );
                            f.Align = FieldAlignEnum.CenterMiddle;

                            //RECORD_NO
                            f = sNgDtl.Fields.Add( "RECORD_NO", StringUtils.ToString( dtRow[GRID_NG_LIST.RECORD_NO.bindField] ), 280, defTop, 300, const_height * resultRate );
                            DefineFixValues( f );
                            f.Align = FieldAlignEnum.CenterMiddle;

                            //不具合部品枠
                            f = sNgDtl.Fields.Add( "partswaku", "", 580, defTop, 3900, const_height * resultRate );
                            DefineFixValues( f );

                            //不具合入力値
                            string NGValue = StringUtils.ToString( dtRow["INPUT_VALUE1"] )
                                + System.Environment.NewLine + System.Environment.NewLine +
                                StringUtils.ToString( dtRow["INPUT_VALUE2"] );
                            f = sNgDtl.Fields.Add( "NG_VALUE", NGValue, 4480, defTop, 900, const_height * resultRate );
                            DefineFixValues( f );
                            f.Align = FieldAlignEnum.LeftMiddle;
                            f.MarginLeft = 40;

                            //不具合記入者枠
                            f = sNgDtl.Fields.Add( "NG_UPD_WAKU", "", 5380, defTop, 970, const_height * resultRate );
                            DefineFixValues( f );

                            //修正内容
                            f = sNgDtl.Fields.Add( "ADJUST_DTL", StringUtils.ToString( dtRow[GRID_NG_LIST.ADJUST_DTL.bindField] ), 6350, defTop, 1700, const_height * resultRate );
                            DefineFixValues( f );
                            f.Align = FieldAlignEnum.LeftMiddle;
                            f.MarginLeft = 50;

                            //修正者枠
                            f = sNgDtl.Fields.Add( "UPD_WAKU", "", 8050, defTop, 967, const_height * resultRate );
                            DefineFixValues( f );

                            //確認者枠
                            f = sNgDtl.Fields.Add( "CONF_WAKU", "", 9017, defTop, 967, const_height * resultRate );
                            DefineFixValues( f );


                            //////////////////////////////////////////////////////
                            //  表示内容の枠設定
                            //////////////////////////////////////////////////////
                            //内容
                            f = sNgDtl.Fields.Add( "NG_DTL", StringUtils.ToString( dtRow[GRID_NG_LIST.NG_DTL.bindField] ), 580, defTop, 3900, const_height * resultRate );
                            DefineFixValues( f );
                            f.Align = FieldAlignEnum.LeftMiddle;
                            f.MarginLeft = 50;

                            //不具合記入日時
                            f = sNgDtl.Fields.Add( "NG_UPDATE_DT", StringUtils.ToString( dtRow[GRID_NG_LIST.NG_UPDATE_DT.bindField] ), 5380, defTop + 140, 970, const_height );
                            DefineFixValues( f, BorderStyleEnum.Transparent );
                            f.Align = FieldAlignEnum.CenterMiddle;

                            //不具合記入社員
                            empNm = "";
                            if ( StringUtils.IsNotEmpty( StringUtils.ToString( dtRow[GRID_NG_LIST.NG_UPDATE_EMPL_CD.bindField] ) ) ) {
                                empNm = StringUtils.ToString( dtRow[GRID_NG_LIST.NG_UPDATE_EMPL_CD.bindField] );
                                empNm = empNm.Substring( empNm.IndexOf( ":" ) + 1 );
                            }
                            f = sNgDtl.Fields.Add( "NG_UPDATE_EMPL_CD", empNm, 5380, defTop + 440, 970, const_height );
                            DefineFixValues( f, BorderStyleEnum.Transparent );
                            f.Align = FieldAlignEnum.CenterMiddle;
                            f.MarginLeft = 50;


                            //修正日時
                            f = sNgDtl.Fields.Add( "ADJUST_DT", StringUtils.ToString( dtRow[GRID_NG_LIST.ADJUST_DT.bindField] ), 8050, defTop + 140, 967, const_height );
                            DefineFixValues( f, BorderStyleEnum.Transparent );
                            f.Align = FieldAlignEnum.CenterMiddle;

                            //修正社員
                            empNm = "";
                            if ( StringUtils.IsNotEmpty( StringUtils.ToString( dtRow[GRID_NG_LIST.ADJUST_EMPL_CD.bindField] ) ) ) {
                                empNm = StringUtils.ToString( dtRow[GRID_NG_LIST.ADJUST_EMPL_CD.bindField] );
                                empNm = empNm.Substring( empNm.IndexOf( ":" ) + 1 );
                            }
                            f = sNgDtl.Fields.Add( "ADJUST_EMPL_CD", empNm, 8050, defTop + 440, 967, const_height );
                            DefineFixValues( f, BorderStyleEnum.Transparent );
                            f.Align = FieldAlignEnum.CenterMiddle;
                            f.MarginLeft = 50;

                            //確認日時
                            f = sNgDtl.Fields.Add( "CONFIRM_DT", StringUtils.ToString( dtRow[GRID_NG_LIST.CONFIRM_DT.bindField] ), 9017, defTop + 140, 967, const_height );
                            DefineFixValues( f, BorderStyleEnum.Transparent );
                            f.Align = FieldAlignEnum.CenterMiddle;

                            //確認社員
                            empNm = "";
                            if ( StringUtils.IsNotEmpty( StringUtils.ToString( dtRow[GRID_NG_LIST.CONFIRM_EMPL_CD.bindField] ) ) ) {
                                empNm = StringUtils.ToString( dtRow[GRID_NG_LIST.CONFIRM_EMPL_CD.bindField] );
                                empNm = empNm.Substring( empNm.IndexOf( ":" ) + 1 );
                            }
                            f = sNgDtl.Fields.Add( "CONFIRM_EMPL_CD", empNm, 9017, defTop + 440, 967, const_height );
                            DefineFixValues( f, BorderStyleEnum.Transparent );
                            f.Align = FieldAlignEnum.CenterMiddle;
                            f.MarginLeft = 50;

                            rowCnt = rowCnt + resultRate;
                            defTop += 280 * resultRate;
                            dataCnt++;

                            if ( ( rowCnt % 44 == 0 ) || ( 40 <= rowCnt % 44 ) ) {
                                //改ページ挿入
                                if (false == (dataCnt.Equals( res.SubTables[NG_LIST].Rows.Count )) ) {
                                    CreateBrakeField( sNgDtl, defTop );
                                    defTop += 1;
                                }
                                rowCnt = 0;
                            }
                        }

                        //最終行までなかった場合の処理
                        if ( false == ( rowCnt % 44 == 0 ) ) {
                            //空白枠設定
                            Field f = new Field();
                            f = sNgDtl.Fields.Add( "", "", defLeft, defTop, 9984, ( 44 - ( rowCnt % 44 ) ) * const_height );
                            DefineFixValues( f );

                            //斜線設定
                            Field fSlant = new Field();
                            fSlant = sNgDtl.Fields.Add( "slant", "", defLeft, defTop, 9984, ( 44 - ( rowCnt % 44 ) ) * const_height );
                            CreateSlantField( fSlant );


                        }

                        #endregion

                    }
                    #endregion
                }

                if ( false == ( System.IO.Directory.Exists( @repTemporaryPath ) ) ) {
                    //権限がない場合、「アクセスが拒否されました」と権限エラーがでるので、Exceptionに飛びます
                    System.IO.Directory.CreateDirectory( @repTemporaryPath );
                }

                //結果＋不具合を同時に出力
                C1.C1Preview.C1MultiDocument mdoc = new C1.C1Preview.C1MultiDocument();
                mdoc.Items.Add( report );
                if ( 0 < res.SubTables[NG_LIST].Rows.Count ) {
                    mdoc.Items.Add( repNGlist );
                }
                mdoc.Export( @repTemporaryPath + "\\" + mainFileNm );

                //binary変換
                byte[] binary = System.IO.File.ReadAllBytes( @repTemporaryPath + "\\" + mainFileNm );
                //削除
                System.IO.File.Delete( @repTemporaryPath + "\\" + mainFileNm );

                //DL
                WebFileUtils.DownloadFile( CurrentForm, binary, mainFileNm );

            } catch ( System.Threading.ThreadAbortException ) {
                //WebFileUtils.DownloadFile response.Endで必ず発生する為、正常として扱う
            } catch ( Exception ex) {
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_72060,ex.Message );
            } finally {
                //メモリ解放
                report.Dispose();
                repNGlist.Dispose();

            }

        }

        #region 帳票フィールド設定
        /// <summary>
        /// 固定フィールド(サイズ指定、枠線指定なし)
        /// </summary>
        /// <param name="defField"></param>
        /// <param name="fontSize"></param>
        private void DefineFixValues( Field defField, float fontSize = 7 ) {
            DefineFixValues( defField, fontSize, BorderStyleEnum.Solid );
        }
        /// <summary>
        /// 固定フィールド(サイズ指定なし、枠線指定)
        /// </summary>
        /// <param name="defField"></param>
        /// <param name="style"></param>
        private void DefineFixValues( Field defField, BorderStyleEnum style ) {
            DefineFixValues( defField, 7, style );
        }
        /// <summary>
        /// 固定フィールドの設定
        /// </summary>
        /// <param name="defField">フィールド</param>
        /// <param name="fontSize">fontサイズ</param>
        /// <param name="style">枠線</param>
        private void DefineFixValues( Field defField, float fontSize, BorderStyleEnum style ) {

            string const_font_name = "ＭＳ 明朝";           //フォント名

            defField.Calculated = false;
            defField.CanGrow = true;
            defField.Font.Name = const_font_name;
            defField.Font.Size = fontSize;
            defField.BorderStyle = style;
            defField.BorderColor = System.Drawing.Color.Black;
            defField.LineWidth = 3;
        }

        /// <summary>
        /// 改行処理
        /// </summary>
        /// <param name="section"></param>
        /// <param name="top"></param>
        private void CreateBrakeField( C1.C1Report.Section section, int top ) {

            Field brkField = new Field();
            brkField = section.Fields.Add( "Break", "", 0, top, 9980, 280 );
            brkField.Calculated = false;
            brkField.CanGrow = false;
            brkField.ForcePageBreak = ForcePageBreakEnum.Before;
            brkField.Visible = true;
        }

        /// <summary>
        /// 斜線処理
        /// </summary>
        /// <param name="field"></param>
        private void CreateSlantField( Field field ) {
            DefineFixValues( field );

            field.ShapeType = ShapeType.Line;
            field.LineWidth = 5;
            LineShape line = new LineShape();
            line.LineSlant = LineSlantEnum.Down;

            field.Shape = line;

        }
        #endregion

        #endregion

    }
}