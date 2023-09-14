using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
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
using TRC_W_PWT_ProductView.UI.Pages.Maintenance;
using TRC_W_PWT_ProductView.UI.Pages.UserControl;

namespace TRC_W_PWT_ProductView.UI.Pages {

    /// <summary>
    /// チェック対象外リスト明細画面
    /// </summary>
    public partial class MaintenanceNAListDetail : BaseForm {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        const string CONST_IN_PRODUCT = "生産指示(順序)";
        const string CONST_NOT_APPLICABLE = "登録済(チェック対象外)";
        const string NOT_FOUND_DTL_DATA = "重要チェック対象外リスト";
        Dictionary<string, object> dicControlValues = new Dictionary<string, object>();

        #region 検索条件定義
        /// <summary>
        /// 一覧検索条件定義
        /// </summary>
        public class CONDITION {
            /// <summary>型式コード</summary>
            public static readonly ControlDefine MODEL_CD = new ControlDefine( "txtModelCd", "型式コード", "modelCd", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>製品機番</summary>
            public static readonly ControlDefine SERIAL_NO = new ControlDefine( "txtSerial", "製品機番", "serial", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>パターンコード</summary>
            public static readonly ControlDefine PTN_CD = new ControlDefine( "txtPtnCd", "パターンコード", "ptnCd", ControlDefine.BindType.Both, typeof( String ) );
        }
        #endregion
        /// <summary>
        /// 管理ID
        /// </summary>
//        const string MANAGE_ID = Defines.Session.DetailFrame.SESSION_PAGE_INFO_DETAIL_KEY;//CurrentUCInfo.pageId

        #region グリッドビュー定義
        #region ヘッダ
        public class MODEL_INFO_PRODUCT {
            /// <summary>生産型式コード</summary>
            public static readonly ControlDefine MODEL_CD = new ControlDefine( "txtModelCd", "型式コード", "MODEL_CD", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>生産型式名</summary>
            public static readonly ControlDefine MODEL_NM = new ControlDefine( "txtModelNm", "型式名", "MODEL_NM", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>機番</summary>
            public static readonly ControlDefine SERIAL_NO = new ControlDefine( "txtSerial", "製品機番", "SERIAL_NO", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>完成日</summary>
            public static readonly ControlDefine FIN_DT = new ControlDefine( "txtFinDt", "完成日", "FIN_DT", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>パターンコード</summary>
            public static readonly ControlDefine PTN_CD = new ControlDefine( "txtPtnCd", "パターンコード", "PTN_CD", ControlDefine.BindType.Down, typeof( String ) );
        }
        #endregion


        #region 生産中
        /// <summary>
        /// 検索区分：生産中
        /// </summary>
        internal class GRID_IN_PRODUCT {
            /// <summary>型式コード</summary>
            public static readonly GridViewDefine MODEL_CD = new GridViewDefine( "型式コード", "MODEL_CD", typeof( string ), "", true, HorizontalAlign.Center, 140, true );
            /// <summary>型式名</summary>
            public static readonly GridViewDefine MODEL_NM = new GridViewDefine( "型式名", "MODEL_NM", typeof( string ), "", true, HorizontalAlign.Left, 200, true );
            /// <summary>機番</summary>
            public static readonly GridViewDefine SERIAL_NO = new GridViewDefine( "機番", "SERIAL_NO", typeof( string ), "", true, HorizontalAlign.Center, 100, true );
            /// <summary>完成予定日</summary>
            public static readonly GridViewDefine KAN_YO_YM = new GridViewDefine( "完成予定日", "KAN_YO_YM", typeof( string ), "", true, HorizontalAlign.Center, 140, true );
            /// <summary>月度連番</summary>
            public static readonly GridViewDefine DATA_CNT = new GridViewDefine( "ﾁｪｯｸ対象外登録", "DATA_CNT", typeof( string ), "", true, HorizontalAlign.Center, 140, true );

        }

        public class GRID_MAIN {
            /// <summary>TR</summary>
            public static readonly ControlDefine TR_ROW_DATA = new ControlDefine( "trRowData", "TR", "", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>(SELECTコマンド送信用ボタン)</summary>
            public static readonly ControlDefine SELECT = new ControlDefine( "btnSelect", "BUTTON", "", ControlDefine.BindType.None, typeof( String ) );
            /// <suumary>チェック対象外(キー)</summary>
            public static readonly ControlDefine OPE_KEY = new ControlDefine( "txtOpeKey", "チェック対象外(キー)", "OPE_KEY", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>チェック対象外</summary>
            public static readonly ControlDefine PARTS_OPE = new ControlDefine( "txtPartsOpe", "チェック対象外", "PARTS_OPE", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>詳細</summary>
            public static readonly ControlDefine DTL = new ControlDefine( "txtDtl", "詳細", "DTL", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>詳細(キー)</summary>
            public static readonly ControlDefine DTL_KEY = new ControlDefine( "txtDtlKey", "詳細(キー)", "DTL_KEY", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>備考</summary>
            public static readonly ControlDefine NOTES = new ControlDefine( "txtNotes", "備考", "NOTES", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>登録日</summary>
            public static readonly ControlDefine INS_DT = new ControlDefine( "txtInsDt", "登録日", "INS_DT", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>登録者</summary>
            public static readonly ControlDefine INS_BY = new ControlDefine( "txtInsBy", "登録者", "INS_BY", ControlDefine.BindType.Down, typeof( String ) );
        }

       #endregion

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
        /// ヘッダ情報
        /// </summary>
        ControlDefine[] _HeaderControls = null;
        /// <summary>
        /// ヘッダ情報
        /// </summary>
        ControlDefine[] HeaderControls {
            get {
                if ( true == ObjectUtils.IsNull( _HeaderControls ) ) {
                    _HeaderControls = ControlUtils.GetControlDefineArray( typeof( MODEL_INFO_PRODUCT ) );
                }
                return _HeaderControls;
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
        /// <summary>
        /// 表示中ユーザコントロール情報
        /// </summary>
        private PageInfo.ST_PAGE_INFO CurrentUCInfo {
            get {
                return PageInfo.GetUCPageInfo( DetailKeyParam.ProductKind, DetailKeyParam.GroupCd, DetailKeyParam.ClassCd, DetailKeyParam.AssemblyPatternCd );
            }
        }
        /// <summary>
        /// ユーザ情報
        /// </summary>
        private UserInfoSessionHandler.ST_USER _loginInfo;
        /// <summary>
        /// ユーザ情報
        /// </summary>
        public UserInfoSessionHandler.ST_USER LoginInfo {

            get {
                if ( true == ObjectUtils.IsNull( _loginInfo.UserInfo ) ) {
                    SessionManagerInstance sesMgr = CurrentForm.SessionManager;
                    _loginInfo = sesMgr.GetUserInfoHandler().GetUserInfo();
                }

                return _loginInfo;
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

            string productModelCd = "";
            string serial = "";
            string ptnCd = "";

            //一覧表示列の設定
            GridViewDefine[] gridColumns;
            gridColumns = ControlUtils.GetGridViewDefineArray( typeof( MODEL_INFO_PRODUCT ) );

            string coop = StringUtils.Nvl( base.GetTransInfoValue( RequestParameter.DetailFrame.EXTERNAL_COOP ) );
            if ( true == StringUtils.IsNotBlank( coop ) ) {

                //MACs連携
                //現時点では処理なし

            } else {
                //一覧表示
                string callerToken = StringUtils.Nvl( base.GetTransInfoValue( RequestParameter.Common.TOKEN ) );
                string indexStr = StringUtils.Nvl( base.GetTransInfoValue( RequestParameter.DetailFrame.INDEX ) );
                int index = NumericUtils.ToInt( indexStr, -1 );
                //一覧からの遷移
                if ( 0 <= index ) {
 
                    //一覧の検索情報を取得
                    ConditionInfoSessionHandler.ST_CONDITION condMainView = SessionManager.GetConditionInfoHandler( callerToken ).GetCondition( PageInfo.MasterMainteNAList.pageId );

                    //完成日列追加
                    if (false == ( condMainView.ResultData.Columns.Contains( "FIN_DT" ) )) { 
                        condMainView.ResultData.Columns.Add( "FIN_DT" );
                    }

                    if ( true == ObjectUtils.IsNotNull( condMainView.ResultData )
                        && index < condMainView.ResultData.Rows.Count ) {

                        DataRow rowMainView = condMainView.ResultData.Rows[index];

                        ptnCd = StringUtils.ToString( rowMainView["PTN_CD"] );
                        productModelCd = rowMainView[MODEL_INFO_PRODUCT.MODEL_CD.bindField].ToString();
                        serial = rowMainView[MODEL_INFO_PRODUCT.SERIAL_NO.bindField].ToString();

                        //ヘッダデータ取得
                        string strFinDt = DoSearchHeader( productModelCd, serial, ptnCd );
                        rowMainView["FIN_DT"] = strFinDt;

                        base.SetControlValues( HeaderControls, rowMainView, ref dicControlValues );


                        logger.Info( "詳細画面表示(一覧画面から遷移) 型式:{0} 機番:{1}", productModelCd, serial );

                    }
                }
            }

            //初期化、初期値設定
            InitializeValues();

        }
        /// <summary>
        /// 初期化、初期値設定
        /// </summary>
        private void InitializeValues() {

            //■初期値設定
            this.btnInsert.Enabled = false;
            this.btnDelete.Enabled = false;

            //明細データ取得
            DoSearch();

        }

        #endregion

        #endregion
        /// <summary>
        /// 検索ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click( object sender, EventArgs e ) {
            base.RaiseEvent( DoSearch );
        }
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
        protected void grvMainView_PageIndexChanging( object sender, GridViewPageEventArgs e ) {
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

        #region グリッドビューイベント

        /// <summary>
        /// グリッドビュー行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowDataBoundMainView( params object[] parameters ) {

            object sender = parameters[0];
            GridViewRowEventArgs e = (GridViewRowEventArgs)parameters[1];
            if ( e.Row.RowType == DataControlRowType.DataRow ) {

                ControlUtils.GridViewRowBound( (GridView)sender, e, ControlUtils.GridRowDoubleClickEvent.None );
            }
        }

        /// <summary>
        /// グリッドビューページ切替
        /// </summary>
        /// <param name="parameters"></param>
        private void PageIndexChangingMainView( params object[] parameters ) {
            object sender = parameters[0];
            GridViewPageEventArgs e = (GridViewPageEventArgs)parameters[1];
            ControlUtils.GridViewPageIndexChanging( (GridView)sender, ConditionInfo.ResultData, e );
        }

        /// <summary>
        /// グリッドビュー並び替え
        /// </summary>
        /// <param name="parameters"></param>
        private void SortingMainView( params object[] parameters ) {
            object sender = parameters[0];
            GridViewSortEventArgs e = (GridViewSortEventArgs)parameters[1];

            ConditionInfoSessionHandler.ST_CONDITION cond = ConditionInfo;
            ControlUtils.GridViewSorting( (GridView)sender, ref cond, e );
            ConditionInfo = cond;
        }
        #endregion

        #region 検索処理
        /// <summary>
        /// 検索処理(ヘッダー)
        /// </summary>
        private string DoSearchHeader(string modeCd,string serial,string ptnCd) {


            string retDt = "";
            //検索結果取得
            //エラー時にも最後まで処理を通すため、インスタンスを生成しておく
            MainViewBusiness.ResultSet result = new MainViewBusiness.ResultSet();
            DataTable tblResult = null;
            int maxGridViewCount = WebAppInstance.GetInstance().Config.WebCommonInfo.maxGridViewCount;  //検索上限数
            try {
                //ヘッダデータ取得
                tblResult = EngineProcessDao.SelectHeaderInfo( modeCd, serial, ptnCd, maxGridViewCount );

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
                if ( null == tblResult || 0 == tblResult.Rows.Count ) {
                    //検索結果0件
                    result.Message = new Msg( MsgManager.MESSAGE_WRN_61010 );
                    retDt = "";

                } else if ( ( null != tblResult && maxGridViewCount <= tblResult.Rows.Count ) ) {
                    //検索件数が上限を上回っている場合には警告メッセージをセット
                    result.Message = new Msg( MsgManager.MESSAGE_WRN_61020 );
                }else{
                    retDt = StringUtils.ToString( tblResult.Rows[0]["FIN_DT"] );
                }

            }

            return retDt;
        }

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

            //検索結果取得
            //エラー時にも最後まで処理を通すため、インスタンスを生成しておく
            MainViewBusiness.ResultSet result = new MainViewBusiness.ResultSet();
            DataTable tblResult = null;
            int maxGridViewCount = WebAppInstance.GetInstance().Config.WebCommonInfo.maxGridViewCount;  //検索上限数
            try {
                //生産中データ取得
                result.ListTable = EngineProcessDao.SelectNotApplicableDetail( dicCondition, maxGridViewCount );

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
                solidTitleHeader.Visible = true;

                //メッセージ設定
                result.Message = null;
                if ( null == result.ListTable || 0 == result.ListTable.Rows.Count ) {
                    //検索結果0件
                    result.Message = new Msg( MsgManager.MESSAGE_WRN_62010, NOT_FOUND_DTL_DATA );
                    solidTitleHeader.Visible = false;

                } else if ( ( null != result.ListTable && maxGridViewCount <= result.ListTable.Rows.Count ) ) {
                    //検索件数が上限を上回っている場合には警告メッセージをセット
                    result.Message = new Msg( MsgManager.MESSAGE_WRN_61020 );
                }

            }

            ConditionInfoSessionHandler.ST_CONDITION cond = new ConditionInfoSessionHandler.ST_CONDITION();
            tblResult = result.ListTable;
            if ( null != tblResult ) {

                //検索条件/結果インスタンス
                cond.conditionValue = dicCondition;
                cond.IdWithText = dicIdWithText;
                cond.ResultData = tblResult.DefaultView.ToTable();
            } else {
                //タイムアウト等Exception時には、GridViewクリア
            }

            //検索条件をセッションに格納
            ConditionInfo = cond;

            //グリッドビューバインド
            lstMainList.DataSource = tblResult;
            lstMainList.DataBind();

            lstMainList.SelectedIndex = 0;
            SelectedIndexChangedMainList();


            //権限によるボタン制御
            AppPermission.PERMISSION_INFO permMainteInfo = AppPermission.GetTransactionPermission( PageInfo.NACheckList, LoginInfo.UserInfo );
            if ( permMainteInfo.IsView == true ) {

                if ( StringUtils.IsEmpty(txtFinDt.Text)) {
                    this.btnInsert.Enabled = true;

                    if ( lstMainList.Items.Count > 0 ) {
                        this.btnDelete.Enabled = true;
                    }
                }
            }else{
                this.btnInsert.Enabled = false;
                this.btnDelete.Enabled = false;
            }

            //メッセージ表示
            if ( null != result.Message ) {
                base.WriteApplicationMessage( result.Message );
            }

            return;
        }
        #endregion

        /// <summary>
        /// メインリスト行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstMainList_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundMainList( sender, e );
        }

        protected void lstMainList_SelectedIndexChanging( object sender, ListViewSelectEventArgs e ) {

        }

        /// <summary>
        /// メインリスト選択行変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstMainList_SelectedIndexChanged( object sender, EventArgs e ) {
            CurrentForm.RaiseEvent( SelectedIndexChangedMainList );

        }

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

                //クリックイベントセット
                HtmlTableRow trRow = (HtmlTableRow)e.Item.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
                Button selectBtn = (Button)e.Item.FindControl( GRID_MAIN.SELECT.controlId );
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format( ControlUtils.LIST_VIEW_SELECTED, e.Item.DataItemIndex, selectBtn.UniqueID );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );
            }
        }
        #endregion

        #region メインリスト選択行変更
        /// <summary>
        /// メインリスト選択行変更処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void SelectedIndexChangedMainList() {

            int mainIndex = lstMainList.SelectedIndex;

            string modelCd = null;
            string serial = null;
            string opeKey = null;
            string strDtl = null;
            string finDt = null;
            string ptnCd = null;


            //選択行背景色変更解除
            foreach ( ListViewDataItem li in lstMainList.Items ) {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace( " " + ResourcePath.CSS.ListSelectedRow, "" );
            }

            if ( lstMainList.Items.Count > 0 ) { 
                HtmlTableRow trSelectRow = (HtmlTableRow)lstMainList.Items[mainIndex].FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
                trSelectRow.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;

                //選択されたレコードを取得
                TextBox tmpKey = (TextBox)lstMainList.Items[mainIndex].FindControl( GRID_MAIN.OPE_KEY.controlId );
                opeKey = StringUtils.ToString( tmpKey.Text );

                //
                TextBox tmpDtl = (TextBox)lstMainList.Items[mainIndex].FindControl( GRID_MAIN.DTL_KEY.controlId );
                strDtl = StringUtils.ToString( tmpDtl.Text );
            }
            finDt = txtFinDt.Text;

            Dictionary<string, object> dicCondition = new Dictionary<string, object>();
            base.GetControlValues( ConditionControls, ref dicCondition );
            modelCd = DataUtils.GetModelCd( DataUtils.GetDictionaryStringVal( dicCondition, "modelCd" ) );  //型式コード
            serial = DataUtils.GetDictionaryStringVal( dicCondition, "serial" );    //シリアル
            ptnCd = DataUtils.GetDictionaryStringVal( dicCondition, "ptnCd" );      //パターンコード

            //keyの作成
            ListItem[] liArr = new ListItem[6];
            liArr[0] = new ListItem( "exeKbn", "1" );
            liArr[1] = new ListItem( "modelCd", modelCd );
            liArr[2] = new ListItem( "serial", serial );
            liArr[3] = new ListItem( "opr", opeKey );
            liArr[4] = new ListItem( "st", strDtl );
            liArr[5] = new ListItem( "ptnCd", ptnCd );


            //新規ボタン用
            btnInsert.Attributes[ControlUtils.ON_CLICK] = InputModal.CreateDispUrl( this, PageInfo.NACheckList, 10, 10, liArr, "1" );

            //削除ボタン用
            liArr[0] = new ListItem( "exeKbn", "3" );
            btnDelete.Attributes[ControlUtils.ON_CLICK] = InputModal.CreateDispUrl( this, PageInfo.NACheckList, 10, 10, liArr, "3" );

        }

        #endregion

        #region 削除処理
        /// <summary>
        /// 削除ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click( object sender, EventArgs e ) {
            deleteInvalidMsg();
        }
        /// <summary>
        /// 削除不可メッセージ表示
        /// </summary>
        private void deleteInvalidMsg() {
            base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_72050 );        
        }

        #endregion

    }
}