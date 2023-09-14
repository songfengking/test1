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
    /// チェック対象外一覧画面
    /// </summary>
    public partial class MasterMainte3CList : BaseForm {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 固定値
        private const int CONST_MAX_RECORD = 1000;

        /// <summary>
        /// グリッドビューグループ
        /// </summary>
        const string GRID_MAIN_VIEW_GROUP_CD = "MainView";
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
        GridViewDefine[] _gridviewResults = null;
        /// <summary>
        /// 一覧定義情報アクセサ
        /// </summary>
        GridViewDefine[] GridViewResults {
            get {
                if ( true == ObjectUtils.IsNull( _gridviewResults ) ) {
                    _gridviewResults = ControlUtils.GetGridViewDefineArray( typeof( GRID_MAIN ) );
                }
                return _gridviewResults;
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

        #region メンバ変数
        Dictionary<String, String> _dicEmp = new Dictionary<String, String>();
        #endregion


        #region 検索条件定義
        /// <summary>
        /// 一覧検索条件定義
        /// </summary>
        public class CONDITION {
            /// <summary>製品区分</summary>
            public static readonly ControlDefine productKind = new ControlDefine( "ddl3C", "製品区分", "productKind", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>エンジン種別</summary>
            public static readonly ControlDefine engineKind = new ControlDefine( "ddlEngineKind", "エンジン種別", "engineKind", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>検索対象</summary>
            public static readonly ControlDefine targetKind = new ControlDefine( "chkTarget", "検索対象", "targetKind", ControlDefine.BindType.Both, typeof( Boolean ) );
            /// <summary>範囲(開始)</summary>
            public static readonly ControlDefine dataFrom = new ControlDefine( "cldStart", "範囲(開始)", "dateFrom", ControlDefine.BindType.Both, typeof( DateTime ) );
            /// <summary>範囲(終了)</summary>
            public static readonly ControlDefine dataTo = new ControlDefine( "cldEnd", "範囲(終了)", "dateTo", ControlDefine.BindType.Both, typeof( DateTime ) );
            /// <summary>範囲(取付日From)</summary>
            public static readonly ControlDefine assemblyStart = new ControlDefine( "cldAssemblyStart", "取付日From", "assemblyStart", ControlDefine.BindType.Both, typeof( DateTime ) );
            /// <summary>範囲(取付日To)</summary>
            public static readonly ControlDefine assemblyEnd = new ControlDefine( "cldAssemblyEnd", "取付日To", "assemblyEnd", ControlDefine.BindType.Both, typeof( DateTime ) );
        }
        #endregion

        #region グリッドビュー定義
        /// <summary>
        /// 一覧表示情報コントロール(左下)
        /// </summary>
        public class GRID_SEARCH_CONTROLS_L {
            public static readonly ControlDefine UPD_CHECK = new ControlDefine( "chkUpdate", "ﾁｪｯｸ", "", ControlDefine.BindType.None, null );
        }
        /// <summary>チェックボックス</summary>

        /// <summary>
        /// 一覧表示情報コントロール(右下)
        /// </summary>
        public class GRID_SEARCH_CONTROLS_R {
        }
        /// <summary>
        /// 検索結果
        /// </summary>
        internal class GRID_MAIN {

            /// <summary>チェックボックス</summary>
            public static readonly GridViewDefine UPD_CHECK = new GridViewDefine( "ﾁｪｯｸ", "", typeof( string ), "", false, HorizontalAlign.Center, 60, true, false );
            /// <suumary>製品区分</summary>
            public static readonly GridViewDefine PARTS_CD = new GridViewDefine( "製品区分", "PARTS_CD", typeof( string ), "", false, HorizontalAlign.Center, 70, true, true );
            /// <suumary>重要部品品名</summary>
            public static readonly GridViewDefine PARTS_NM = new GridViewDefine( "重要部品品名", "PARTS_NM", typeof( string ), "", true,  HorizontalAlign.Left, 160, true, true );
            /// <suumary>加工付日</summary>
            public static readonly GridViewDefine PROC_DT = new GridViewDefine( "加工日", "PROC_DT", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true ); 
            /// <suumary>取付日</summary>
            public static readonly GridViewDefine NEW_YMD = new GridViewDefine( "取付日", "NEW_YMD", typeof( string ), "", true, HorizontalAlign.Center, 150, true, true );
            /// <suumary>連番</summary>
            public static readonly GridViewDefine PROC_NUM = new GridViewDefine( "連番", "PROC_NUM", typeof( string ), "", true, HorizontalAlign.Center, 65, true, true );
            /// <suumary>型式コード</summary>
            public static readonly GridViewDefine MODEL_CD = new GridViewDefine( "型式コード", "MODEL_CD", typeof( string ), "", true, HorizontalAlign.Center, 130, true, true );
            /// <suumary>型式名</summary>
            public static readonly GridViewDefine MODEL_NM = new GridViewDefine( "型式名", "MODEL_NM", typeof( string ), "", true, HorizontalAlign.Left, 180, true, true );            
            /// <suumary>国コード</summary>
            public static readonly GridViewDefine COUNTRY = new GridViewDefine( "国コード", "COUNTRY", typeof( string ), "", true, HorizontalAlign.Center,100, true, true );
            /// <suumary>機番</summary>
            public static readonly GridViewDefine SERIAL_NO = new GridViewDefine( "機番", "SERIAL_NO", typeof( string ), "", true, HorizontalAlign.Center, 90, true, true );
            /// <suumary>加工ライン</summary>
            public static readonly GridViewDefine PROCESSING_LINE = new GridViewDefine( "加工ライン", "PROCESSING_LINE", typeof( string ), "", true, HorizontalAlign.Center, 90, true, true );
            /// <suumary>修正理由</summary>
            public static readonly GridViewDefine REMARKS = new GridViewDefine( "修正理由", "REMARKS", typeof( string ), "", true, HorizontalAlign.Left, 300, true, true );
            /// <suumary>修正者</summary>
            public static readonly GridViewDefine UPDATE_BY = new GridViewDefine( "修正者", "UPDATE_BY", typeof( string ), "", true, HorizontalAlign.Left, 140, true, true );
            /// <suumary>ステーション</summary>
            public static readonly GridViewDefine ST = new GridViewDefine( "ステーション", "ST", typeof( string ), "", true, HorizontalAlign.Center, 0, false, false );
            /// <suumary>製品区分(CD)</summary>
            public static readonly GridViewDefine PARTS_CD_ORG = new GridViewDefine( "製品区分(CD)", "PARTS_CD_ORG", typeof( string ), "", false, HorizontalAlign.Center, 0, false, false );

        }

        #endregion

        #region スクリプトイベント
        const string CLOSE_MODAL_DISP = "TorqueRelation.CloseModal();";

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
            ControlUtils.SetGridViewTemplateField( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, true ) );
            ControlUtils.SetGridViewTemplateField( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, false ) );

            ControlUtils.ReMakeBoundTemplateFieldCtrl( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, true ) );
            ControlUtils.ReMakeBoundTemplateFieldCtrl( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, false ) );

            int resultCnt = 0;
            if ( ObjectUtils.IsNotNull( ConditionInfo.ResultData ) ) {
                resultCnt = ConditionInfo.ResultData.Rows.Count;
            }

//            ControlUtils.SetGridViewPager( ref pnlPager, grvMainViewRB, grvMainView_PageIndexChanging, resultCnt, grvMainViewRB.PageIndex );

        }

        #endregion

        #endregion

        #region メソッド

        #region 画面初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        protected override void Initialize() {

            //ベース処理初期化処理
            base.Initialize();

            //アクセスカウンター登録
            Dao.Com.AccessCounterDao.Entry( base.CurrentPageInfo.pageId );

            //イベント
            ListItem[] liArr = new ListItem[1];
            liArr[0] = new ListItem( "exeKbn", "2" );
            btnModalDisp.Attributes[ControlUtils.ON_CLICK] = InputModal.CreateDispUrl( this, PageInfo.ProcessingDtEdit, 10, 10, liArr, "2" );
            
            //初期化、初期値設定
            InitializeValues();

        }
        /// <summary>
        /// 初期化、初期値設定
        /// </summary>
        private void InitializeValues() {

            //検索区分
            CreateProductKind();

            //エンジン種別
            CreateEngineKind();

            //当月月初
            cldStart.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, 1 );
            cldAssemblyStart.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, 1 );

            //当月月末
            cldEnd.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth( DateTime.Today.Year, DateTime.Today.Month ) );
            cldAssemblyEnd.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth( DateTime.Today.Year, DateTime.Today.Month ) );

            ConditionInfo = new ConditionInfoSessionHandler.ST_CONDITION();

            //件数表示
            ClearGridView();

            btnModalDisp.Enabled = false;
        }
        /// <summary>
        /// 製品区分の設定
        /// </summary>
        private void CreateProductKind() {

            ListItem[] liArr = new ListItem[3];
            liArr[0] = new ListItem( "CC", PartsKind.PARTS_CD_ENGINE_CC );
            liArr[1] = new ListItem( "CYH", PartsKind.PARTS_CD_ENGINE_CYH );
            liArr[2] = new ListItem( "CS", PartsKind.PARTS_CD_ENGINE_CS );

            ControlUtils.SetListControlItems( ddl3C, liArr );
      
        }

        /// <summary>
        /// エンジン種別の設定
        /// </summary>
        private void CreateEngineKind() {

            ListItem[] liArr = new ListItem[3];
            liArr[0] = new ListItem( "", "" );
            liArr[1] = new ListItem( "03エンジン", "3" );
            liArr[2] = new ListItem( "07エンジン", "7");

            ControlUtils.SetListControlItems( ddlEngineKind, liArr );

        }
        #endregion

        #region グリッドビュー

        /// <summary>
        /// グリッドビュークリア
        /// </summary>
        private void ClearGridView() {

            //列名非表示 グリッドビュークリア
            ControlUtils.InitializeGridView( grvHeaderLT,false );
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

            //ボタンの活性状態切替
            SetButtonPermission();

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

        #region グリッドビューイベント

        /// <summary>
        /// グリッドビューページ切替
        /// </summary>
        /// <param name="parameters"></param>
        private void PageIndexChangingMainView( params object[] parameters ) {
            object sender = parameters[0];
            //GridViewPageEventArgs e = (GridViewPageEventArgs)parameters[1];
            //ControlUtils.GridViewPageIndexChanging( (GridView)sender, ConditionInfo.ResultData, e );

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

            ControlUtils.GridViewPageIndexChanging( grvMainViewLB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
            ControlUtils.GridViewPageIndexChanging( grvMainViewRB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );

//            ControlUtils.SetGridViewPager( ref pnlPager, grvMainViewRB, grvMainView_PageIndexChanging, ConditionInfo.ResultData.Rows.Count, grvMainViewRB.PageIndex );

            //グリッドビュー表示列情報修正
//            SetGridViewColumns();

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

                //ここで画面項目制御
                CheckBox chkDel = (CheckBox)e.Row.FindControl( GRID_SEARCH_CONTROLS_L.UPD_CHECK.controlId );

                Dictionary<string, object> dicControls = new Dictionary<string, object>();
                base.SetControlValues( e.Row, ControlUtils.GetControlDefineArray( typeof( GRID_SEARCH_CONTROLS_L ) ), row, ref dicControls );
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
                base.SetControlValues( e.Row, ControlUtils.GetControlDefineArray( typeof( GRID_SEARCH_CONTROLS_R ) ), rowData, ref dicControls );
            }

            ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_MAIN_VIEW_GROUP_CD );

        }
        /// <summary>
        /// グリッドビュー並び替え
        /// </summary>
        /// <param name="parameters"></param>
        private void SortingMainView( params object[] parameters ) {
            object sender = parameters[0];
            GridViewSortEventArgs e = (GridViewSortEventArgs)parameters[1];

            ConditionInfoSessionHandler.ST_CONDITION cond = ConditionInfo;
            //ControlUtils.GridViewSorting( (GridView)sender, ref cond, e );
            ControlUtils.GridViewSorting( grvMainViewLB, ref cond, e, true );
            ControlUtils.GridViewSorting( grvMainViewRB, ref cond, e );

            //背面ユーザ切替対応
            //ControlUtils.RenameSortedColumnNm( grvHeaderLT, cond );
            //ControlUtils.RenameSortedColumnNm( grvHeaderRT, cond );
            GridView frozenGrid = grvMainViewLB;
            ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, true ), cond, true );
            ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, false ), cond, true );

//            ControlUtils.SetGridViewPager( ref pnlPager, grvMainViewRB, grvMainView_PageIndexChanging, cond.ResultData.Rows.Count, grvMainViewRB.PageIndex );

            ConditionInfo = cond;

            //グリッドビュー表示列情報修正
//            SetGridViewColumns();

            //グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();

        }
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
            //生産中が選択された場合
            gridColumns = ControlUtils.GetGridViewDefineArray( typeof( GRID_MAIN ) );

            //検索結果取得
            //エラー時にも最後まで処理を通すため、インスタンスを生成しておく
            MainViewBusiness.ResultSet result = new MainViewBusiness.ResultSet();
            DataTable tblResult = null;
//            int maxGridViewCount = WebAppInstance.GetInstance().Config.WebCommonInfo.maxGridViewCount;  //検索上限数
            //作業指示保存が重いため、4000件では耐えれない処理速度のため1000件としておく
            int maxGridViewCount = CONST_MAX_RECORD;

            try {
                result.ListTable = EngineProcessDao.Select3CList( dicCondition, chkTarget.Checked, maxGridViewCount );

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

                foreach ( DataRow row in tblResult.Rows ) {
                    row["MODEL_CD"] = DataUtils.GetModelCdStr( StringUtils.ToString( row["MODEL_CD"] ) );
                }
                if ( tblResult.Rows.Count > 0 ) {
                    getEmpInfo();
                    tblResult = replaceEmpInfo( tblResult, "UPDATE_BY" );
                }
                
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

                    //新規バインド
                    ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, true ),ConditionInfo, true );
                    ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, false ),ConditionInfo, true );
                    ControlUtils.BindGridView_WithTempField( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, true ), tblResult );
                    ControlUtils.BindGridView_WithTempField( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, false ), tblResult );

                    //ページャー作成
//                  ControlUtils.SetGridViewPager( ref pnlPager, grvMainViewLB, grvMainView_PageIndexChanging, cond.ResultData.Rows.Count, grvMainViewLB.PageIndex );

                    //グリッドビュー表示列情報修正
//                  SetGridViewColumns();

                    //GridView表示
                    divGrvDisplay.Visible = true;

                    //グリッドビュー外のDivサイズ変更
                    SetDivGridViewWidth();
                } else {
                    ClearGridView();
                }
            }

            //権限によるボタン制御
            AppPermission.PERMISSION_INFO permMainteInfo = AppPermission.GetTransactionPermission( PageInfo.ProcessingDtEdit, LoginInfo.UserInfo );
            if ( permMainteInfo.IsEdit == true ) {
                //更新権限所持＋データが存在する
                if ( tblResult.Rows.Count > 0 ) {
                    btnModalDisp.Enabled = true;
                }else{
                    btnModalDisp.Enabled = false;
                }
            } else {
                btnModalDisp.Enabled = false;
            }

            //メッセージ表示
            if ( null != result.Message ) {
                base.WriteApplicationMessage( result.Message );
            }

            return;
        }
        #endregion

        /// <summary>
        /// 検索対象変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTarget_CheckedChanged( object sender, EventArgs e ) {
            if ( chkTarget.Checked ) {
                //「999999」のみの場合
                cldStart.Enabled = false;
                cldEnd.Enabled = false;

                cldStart.Value = null;
                cldEnd.Value = null;
            }else{
                //それ以外
                cldStart.Enabled = true;
                cldEnd.Enabled = true;

                cldStart.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, 1 );
                cldEnd.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth( DateTime.Today.Year, DateTime.Today.Month ) );
            }
        }

        /// <summary>
        /// 処理実行前データチェック
        /// </summary>
        /// <returns></returns>
        private bool CheckInputData() {

            //ユーザ情報チェック
            if ( true == ObjectUtils.IsNull( LoginInfo.UserInfo )
                || true == StringUtils.IsEmpty( LoginInfo.UserInfo.userName ) ) {
                PageUtils.RedirectToErrorPage( Page, MsgManager.MESSAGE_ERR_72020, null );
                return false;
            }

            //更新権限チェック
            AppPermission.PERMISSION_INFO permMainteInfo = AppPermission.GetTransactionPermission( PageInfo.ProcessingDtEdit, LoginInfo.UserInfo );
            if ( permMainteInfo.IsEdit == false ) {
                PageUtils.RedirectToErrorPage( Page, MsgManager.MESSAGE_ERR_72030, null );
                return false;
            }

            return true;
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdate_Click( object sender, EventArgs e ) {

            int exeCnt = 0;

            //更新前権限チェック
            if ( CheckInputData() == false ) {
                return;            
            }

            //更新前データ整形
            DataTable dtSelect = new DataTable();
            dtSelect.Columns.Add( "STATION_CD" );
            dtSelect.Columns.Add( "MODEL_CD" );
            dtSelect.Columns.Add( "SERIAL6" );
            dtSelect.Columns.Add( "CRITICAL_PARTS_CD" );
            dtSelect.Columns.Add( "MATERIAL_PROCESSING_DATE" );
            dtSelect.Columns.Add( "MATERIAL_PROCESSING_NUM" );
            dtSelect.Columns.Add( "REMARKS" );
            dtSelect.Columns.Add( "PROCESSING_LINE" );

            //セッションから検索データ取得
            ConditionInfoSessionHandler.ST_CONDITION cond = base.ConditionInfo;
            if ( null == cond.ResultData || 0 == cond.ResultData.Rows.Count ) {
                //出力対象データなし
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_72040 );
                return;
            }

            foreach ( GridViewRow row in grvMainViewLB.Rows ) {
                CheckBox chkDel = (CheckBox)row.FindControl( GRID_SEARCH_CONTROLS_L.UPD_CHECK.controlId );
                if ( true == chkDel.Checked ) {

                    DataRow row2 = dtSelect.NewRow();
                    row2["STATION_CD"] = StringUtils.ToString( cond.ResultData.Rows[row.DataItemIndex][GRID_MAIN.ST.bindField] );
                    row2["MODEL_CD"] = DataUtils.GetModelCd( StringUtils.ToString( cond.ResultData.Rows[row.DataItemIndex][GRID_MAIN.MODEL_CD.bindField] ) );
                    row2["SERIAL6"] = DataUtils.GetSerial6( StringUtils.ToString( cond.ResultData.Rows[row.DataItemIndex][GRID_MAIN.SERIAL_NO.bindField] ) );
                    row2["CRITICAL_PARTS_CD"] = StringUtils.ToString( cond.ResultData.Rows[row.DataItemIndex][GRID_MAIN.PARTS_CD_ORG.bindField] );
                    if( true == StringUtils.IsBlank( txtparamDt.Value ) || 2 > txtparamDt.Value.Length ) {
                        //未入力または空白の場合は更新しない
                        row2["MATERIAL_PROCESSING_DATE"] = null;
                    }else {
                        row2["MATERIAL_PROCESSING_DATE"] = txtparamDt.Value.Substring( 2 );
                    }
                    if( true == StringUtils.IsBlank(txtparamNum.Value) ) {
                        //未入力または空白の場合は更新しない
                        row2["MATERIAL_PROCESSING_NUM"] = null;
                    }else {
                        row2["MATERIAL_PROCESSING_NUM"] = txtparamNum.Value.PadLeft( 3, '0' );
                    }
                    if ( true == StringUtils.IsBlank( txtparamLine.Value ) ) {
                        //未入力または空白の場合は更新しない
                        row2["PROCESSING_LINE"] = null;
                    } else {
                        row2["PROCESSING_LINE"] = txtparamLine.Value;
                    }
                    if ( true == StringUtils.IsEmpty( txtparamRemark.Value ) ) {
                        //未入力の場合は更新しない(備考は空白で更新できるようにスペースは許容する)
                        row2["REMARKS"] = null;
                    } else {
                        row2["REMARKS"] = txtparamRemark.Value;
                    }

                    dtSelect.Rows.Add( row2 );

                    exeCnt++;
                }
            }

            if ( exeCnt.Equals( 0 ) ) {
                //処理対象データなし
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62020,"修正対象データ" );
                return;
            }

            //更新処理
            int result = EngineProcessDao.Update3CDetail( dtSelect, LoginInfo.UserInfo.userId, PageInfo.MasterMainte3CList.pageId, exeCnt );

            if ( result.Equals( exeCnt ) ) {
                //正常
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_INF_10020 );
            } else if ( result.Equals( 0 ) ) {
                //データなし
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_72040 );
                return;
            } else if ( result != exeCnt ) {
                //処理件数不一致
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_72060 );
                return;
            } else {
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                return;
            }

            //正常終了時、再検索
            DoSearch();

        }

        /// <summary>
        /// 画面表示用従業員情報の取得
        /// </summary>
        private void getEmpInfo() {
            DataTable tmp = new DataTable();
            try {
                //従業員情報を取得
                tmp = Business.DetailViewBusiness.SelectEmpInfo( null, null );
                _dicEmp = new Dictionary<string, string>();

                foreach ( DataRow dr in tmp.Rows ) {
                    _dicEmp.Add( dr["EMP_NO"].ToString().Trim(), dr["EMP_NM"].ToString().Trim() );
                }

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

        }
        /// <summary>
        /// 指定カラムの値(従業員名)変更
        /// </summary>
        /// <param name="dt">名称変更対象DataTable</param>
        /// <param name="strCol">対象カラム</param>
        /// <returns></returns>
        private DataTable replaceEmpInfo( DataTable dt, string strCol ) {

            foreach ( DataRow dtRow in dt.Rows ) {

                //「名称」で上書きする
                if ( StringUtils.IsNotEmpty( StringUtils.ToString( dtRow[strCol] ) ) ) {
                    if ( _dicEmp.ContainsKey( StringUtils.ToString(dtRow[strCol]) ) ) {
                        //存在する
                        dtRow[strCol] = _dicEmp[StringUtils.ToString( dtRow[strCol] )];
                    }else{
                        //存在しない
                        dtRow[strCol] = "";
                    }
                }
            }

            return dt;
        }

        protected void grvMainViewLB_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainViewLB( sender, e );
        }
        protected void grvMainViewRB_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainViewRB( sender, e );
        }
        #region グリッドビュー関連処理

        /// <summary>
        /// グリッドビューの列コントロール制御
        /// </summary>
        /// <remarks>定義情報で表示となっている列に対して権限情報でグリッドビューの列情報を動的に変更</remarks>
        private void SetGridViewColumns() {
            //表示専用処理
            GridView frozenGrid = grvMainViewLB;

            AppPermission.PERMISSION_INFO PermissionInfo = AppPermission.GetTransactionPermission( PageInfo.ProcessingDtEdit, LoginInfo.UserInfo );
            if ( false == PermissionInfo.IsEdit ) {
                int delSignL = ControlUtils.GetGridViewDefineIndex( ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, true ), GRID_MAIN.UPD_CHECK );
                if ( 0 <= delSignL ) {
                    grvHeaderLT.Columns[delSignL].Visible = false;
                    grvMainViewLB.Columns[delSignL].Visible = false;
                }
                int delSignR = ControlUtils.GetGridViewDefineIndex( ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, false ), GRID_MAIN.UPD_CHECK );
                if ( 0 <= delSignR ) {
                    grvHeaderRT.Columns[delSignR].Visible = false;
                    grvMainViewRB.Columns[delSignR].Visible = false;
                }
            }
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

        #region ボトムボタン表示処理
        /// <summary>
        /// セッションの検索結果、ユーザー権限からボトムボタンを活性
        /// </summary>
        private void SetButtonPermission() {

            //初期化

            //セッションから検索結果を取得
            string token = StringUtils.Nvl( base.GetTransInfoValue( RequestParameter.Common.TOKEN ) );
            ConditionInfoSessionHandler.ST_CONDITION cond = base.SessionManager.GetConditionInfoHandler( token ).GetCondition( PageInfo.ProcessingDtEdit.pageId );
            DataTable tblResult = cond.ResultData;

            //ユーザ権限取得
            UserInfoSessionHandler.ST_USER loginInfo = base.SessionManager.GetUserInfoHandler().GetUserInfo();

            //検索結果の有無確認
            if ( null != tblResult && 0 < tblResult.Rows.Count ) {

                if ( true == ObjectUtils.IsNotNull( loginInfo.UserInfo ) ) {
                    AppPermission.PERMISSION_INFO permInfo = AppPermission.GetTransactionPermission( PageInfo.ProcessingDtEdit, loginInfo.UserInfo );
                    //更新権限があれば更新、削除ボタン活性
                    if ( permInfo.IsEdit ) {
                    }
                }
            }
        }
        #endregion
    }
}