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
    /// DPF機番メンテ画面
    /// </summary>
    public partial class MasterMainteDpfList : BaseForm {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 固定値
        /// <summary>
        /// グリッドビューグループ
        /// </summary>
        const string GRID_MAIN_VIEW_GROUP_CD = "MainView";

        const string CONST_IN_PRODUCT = "投入順序";
        const string CONST_REGISTERED = "登録済";
        const string CONST_PARTS = "部品";

        //検索区分
        const string CONST_ORDER_NO = "0";      //投入順序
        const string CONST_DPF_LIST = "1";      //登録済

        //ラインコード
        const string CONST_LINE_OEM = "003060"; //OEM
        const string CONST_LINE_HS = "002150";  //HS

        //ステーション
        const string CONST_ST_OEM = "306310";   //OEM
        const string CONST_ST_HS = "215227";    //HS

        //部品区分
        const string CONST_PARTS_DPF = "DPF";  //DPF
        const string CONST_PARTS_DOC = "DOC";  //DOC
        const string CONST_PARTS_SCR = "SCR";  //SCR
        const string CONST_PARTS_ACU = "ACU";  //ACU

        //検索種別
        const string CONST_SEARCH_DPF = "DPF";  //ACU

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
        /// 検索条件定義情報(投入順序)
        /// </summary>
        ControlDefine[] _conditionControlsJun = null;
        /// <summary>
        /// 検索条件定義情報アクセサ
        /// </summary>
        ControlDefine[] ConditionControlsJun {
            get {
                if ( true == ObjectUtils.IsNull( _conditionControlsJun ) ) {
                    _conditionControlsJun = ControlUtils.GetControlDefineArray( typeof( CONDITION_JUN ) );
                }
                return _conditionControlsJun;
            }
        }

        /// <summary>
        /// 検索条件定義情報(登録済)
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
        public static GridViewDefine[] GridViewResults = null;
        #endregion


        #region 検索条件定義
        /// <summary>
        /// 一覧検索条件定義(投入順序)
        /// </summary>
        public class CONDITION_JUN {
            /// <summary>製品区分</summary>
            public static readonly ControlDefine productKind = new ControlDefine( "rblProductKind", "製品区分", "productKind", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>エンジン種別</summary>
            public static readonly ControlDefine searchKbn = new ControlDefine( "rblSearchKbn", "検索種別", "searchKbn", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>型式コード</summary>
            public static readonly ControlDefine modelCd = new ControlDefine( "txtModelCd", "型式コード", "modelCd", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>型式名</summary>
            public static readonly ControlDefine modelNm = new ControlDefine( "txtModelNm", "型式名", "modelNm", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>製品機番</summary>
            public static readonly ControlDefine serial = new ControlDefine( "txtProductSerial", "機番", "serial", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>IDNO</summary>
            public static readonly ControlDefine idno = new ControlDefine( "txtIDNO", "IDNO", "idno", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>部品区分</summary>
            public static readonly ControlDefine parts = new ControlDefine( "ddlParts", "部品区分", "parts", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>抜取検査</summary>
            public static readonly ControlDefine sample = new ControlDefine( "ddlCheckSample", "抜取検査", "sample", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>登録種別</summary>
            public static readonly ControlDefine regKind = new ControlDefine( "ddlRegKind", "登録種別", "regKind", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>完成予定日(開始)</summary>
            public static readonly ControlDefine dataFrom = new ControlDefine( "cldCompStart", "完成予定日From", "dateFrom", ControlDefine.BindType.Both, typeof( DateTime ) );
            /// <summary>完成予定日(終了)</summary>
            public static readonly ControlDefine dataTo = new ControlDefine( "cldCompEnd", "完成予定日To", "dateTo", ControlDefine.BindType.Both, typeof( DateTime ) );
            /// <summary>範囲(取付日From)</summary>
            public static readonly ControlDefine assemblyStart = new ControlDefine( "cldAssemblyStart", "取付日From", "assemblyStart", ControlDefine.BindType.Both, typeof( DateTime ) );
            /// <summary>範囲(取付日To)</summary>
            public static readonly ControlDefine assemblyEnd = new ControlDefine( "cldAssemblyEnd", "取付日To", "assemblyEnd", ControlDefine.BindType.Both, typeof( DateTime ) );
            /// <summary>ステーション(非表示)</summary>
            public static readonly ControlDefine st = new ControlDefine( "txtST", "ステーション", "st", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>ラインコード(非表示)</summary>
            public static readonly ControlDefine lineCd = new ControlDefine( "txtLineCd", "ラーンコード", "lineCd", ControlDefine.BindType.Both, typeof( String ) );
        }
        /// <summary>
        /// 一覧検索条件定義(登録済)
        /// </summary>
        public class CONDITION {
            /// <summary>製品区分</summary>
            public static readonly ControlDefine productKind = new ControlDefine( "rblProductKind", "製品区分", "productKind", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>エンジン種別</summary>
            public static readonly ControlDefine searchKbn = new ControlDefine( "rblSearchKbn", "検索種別", "searchKbn", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>型式コード</summary>
            public static readonly ControlDefine modelCd = new ControlDefine( "txtModelCd", "型式コード", "modelCd", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>型式名</summary>
            public static readonly ControlDefine modelNm = new ControlDefine( "txtModelNm", "型式名", "modelNm", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>製品機番</summary>
            public static readonly ControlDefine serial = new ControlDefine( "txtProductSerial", "機番", "serial", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>IDNO</summary>
            public static readonly ControlDefine idno = new ControlDefine( "txtIDNO", "IDNO", "idno", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>部品区分</summary>
            public static readonly ControlDefine parts = new ControlDefine( "ddlParts", "部品区分", "parts", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>抜取検査</summary>
            public static readonly ControlDefine sample = new ControlDefine( "ddlCheckSample", "抜取検査", "sample", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>登録種別</summary>
            public static readonly ControlDefine regKind = new ControlDefine( "ddlRegKind", "登録種別", "regKind", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>完成予定日(開始)</summary>
            public static readonly ControlDefine dataFrom = new ControlDefine( "cldCompStart", "完成予定日From", "dateFrom", ControlDefine.BindType.Both, typeof( DateTime ) );
            /// <summary>完成予定日(終了)</summary>
            public static readonly ControlDefine dataTo = new ControlDefine( "cldCompEnd", "完成予定日To", "dateTo", ControlDefine.BindType.Both, typeof( DateTime ) );
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
            public static readonly ControlDefine UPD_CHECK = new ControlDefine( "chkUpdate", "修正", "", ControlDefine.BindType.None, null );
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
            /// <suumary>ライン</summary>
            public static readonly GridViewDefine LINE_CD = new GridViewDefine( "ライン", "LINE_CD", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
            /// <suumary>ステーション</summary>
            public static readonly GridViewDefine ST = new GridViewDefine( "ステーション", "ST", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <suumary>取付日時</summary>
            public static readonly GridViewDefine ATTACHMENT_DT = new GridViewDefine( "取付日時", "ATTACHMENT_DT", typeof( string ), "", true, HorizontalAlign.Center, 160, true, true );
            /// <suumary>抜取検査</summary>
            public static readonly GridViewDefine SAMPLE_CHECK = new GridViewDefine( "抜取検査", "SAMPLE_CHECK", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <suumary>型式コード</summary>
            public static readonly GridViewDefine DPF_MODEL_CD = new GridViewDefine( "部品型式コード", "DPF_MODEL_CD", typeof( string ), "", true, HorizontalAlign.Center, 150, true, true );
            /// <suumary>機番</summary>
            public static readonly GridViewDefine DPF_SERIAL_NO = new GridViewDefine( "部品機番", "DPF_SERIAL_NO", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <suumary>順序連番</summary>
            public static readonly GridViewDefine JUN_NO = new GridViewDefine( "順序連番", "JUN_NO", typeof( string ), "", true, HorizontalAlign.Center, 120, true, true );
            /// <summary>完成予定日</summary>
            public static readonly GridViewDefine KAN_YO_YM = new GridViewDefine( "完成予定日", "KAN_YO_YM", typeof( string ), "", true, HorizontalAlign.Center, 120, true, true );
            /// <summary>規制部品登録</summary>
            public static readonly GridViewDefine DATA_CNT = new GridViewDefine( "部品登録", "DATA_CNT", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <suumary>製品種別</summary>
            public static readonly GridViewDefine PTN_CD = new GridViewDefine( "製品種別", "PTN_CD", typeof( string ), "", true, HorizontalAlign.Center, 0, false, true );

        }

        /// <summary>
        /// トラクタ
        /// </summary>
        internal class GRID_TRACTOR {
            /// <suumary>IDNO</summary>
            public static readonly GridViewDefine TRC_IDNO = new GridViewDefine( "ﾄﾗｸﾀIDNO", "TRC_IDNO", typeof( string ), "", true, HorizontalAlign.Center, 120, true, true );
            /// <suumary>型式コード</summary>
            public static readonly GridViewDefine TRC_MODEL_CD = new GridViewDefine( "ﾄﾗｸﾀ型式", "TRC_MODEL_CD", typeof( string ), "", true, HorizontalAlign.Center, 130, true, true );
            /// <suumary>型式名</summary>
            public static readonly GridViewDefine TRC_MODEL_NM = new GridViewDefine( "ﾄﾗｸﾀ型式名", "TRC_MODEL_NM", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <suumary>国コード</summary>
            public static readonly GridViewDefine TRC_COUNTRY = new GridViewDefine( "ﾄﾗｸﾀ国", "TRC_COUNTRY", typeof( string ), "", true, HorizontalAlign.Center, 90, true, true );
            /// <suumary>機番</summary>
            public static readonly GridViewDefine TRC_SERIAL_NO = new GridViewDefine( "ﾄﾗｸﾀ機番", "TRC_SERIAL_NO", typeof( string ), "", true, HorizontalAlign.Center, 110, true, true );
        }

        /// <summary>
        /// エンジン
        /// </summary>
        internal class GRID_ENGINE {
            /// <suumary>IDNO</summary>
            public static readonly GridViewDefine ENG_IDNO = new GridViewDefine( "ｴﾝｼﾞﾝIDNO", "ENG_IDNO", typeof( string ), "", true, HorizontalAlign.Center, 120, true, true );
            /// <suumary>型式コード</summary>
            public static readonly GridViewDefine ENG_MODEL_CD = new GridViewDefine( "ｴﾝｼﾞﾝ型式", "ENG_MODEL_CD", typeof( string ), "", true, HorizontalAlign.Center, 130, true, true );
            /// <suumary>型式名</summary>
            public static readonly GridViewDefine ENG_MODEL_NM = new GridViewDefine( "ｴﾝｼﾞﾝ型式名", "ENG_MODEL_NM", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <suumary>機番</summary>
            public static readonly GridViewDefine ENG_SERIAL_NO = new GridViewDefine( "ｴﾝｼﾞﾝ機番", "ENG_SERIAL_NO", typeof( string ), "", true, HorizontalAlign.Center, 110, true, true );
        }
        /// <summary>
        /// 共通部
        /// </summary>
        internal class GRID_COMMON_TAIL {
            /// <suumary>更新日時</summary>
            public static readonly GridViewDefine UPDATE_DT = new GridViewDefine( "更新日時", "UPDATE_DT", typeof( string ), "", true, HorizontalAlign.Center, 150, true, true );
            /// <suumary>更新者</summary>
            public static readonly GridViewDefine UPDATE_BY = new GridViewDefine( "更新者", "UPDATE_BY", typeof( string ), "", true, HorizontalAlign.Left, 140, true, true );
        }


        #endregion

        #region スクリプトイベント
        #endregion

        
        #region イベント

        /// <summary>
        /// ページロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load( object sender, EventArgs e ) {
            CurrentForm.RaiseEvent( DoPageLoad, false );
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

            //ベース処理初期化処理
            base.Initialize();

            //アクセスカウンター登録
            Dao.Com.AccessCounterDao.Entry( base.CurrentPageInfo.pageId );

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

            //抜取検査
            CreateCheckSample();

            //登録種別
            CreateRegKind();


            //■初期値設定
            //製品種別 10:エンジン
            rblProductKind.SelectedValue = ProductKind.Engine;
            rblSearchKbn.SelectedValue = CONST_ORDER_NO;

            //部品区分設定
            CreatePartsKbn();

            //日付項目
            //完成予定日From
            cldCompStart.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, 1 );
            //完成予定日to
            cldCompEnd.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth( DateTime.Today.Year, DateTime.Today.Month ) );
            //取付日From
            cldAssemblyStart.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, 1 );
            //取付日to
            cldAssemblyEnd.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth( DateTime.Today.Year, DateTime.Today.Month ) );

            //製品種別設定
            ChangeProductKind();

            //検索区分設定
            ChangeSearchKbn();

            ConditionInfo = new ConditionInfoSessionHandler.ST_CONDITION();

            //件数表示
            ClearGridView();

        }

        /// <summary>
        /// 製品種別リスト作成
        /// </summary>
        private void SetProductKindList() {

            //共通処理で取得
            ControlUtils.SetListControlItems( rblProductKind, Dao.Com.MasterList.ProductKindList );

            //その他(PRODUCT_KIND_CD=76)をリストから削除する
            rblProductKind.Items.RemoveAt( 2 );

            ListItem liArr = new ListItem();
            liArr = new ListItem( CONST_PARTS, CONST_SEARCH_DPF );
            rblProductKind.Items.Add( liArr );

        }

        /// <summary>
        /// 検索区分の設定
        /// </summary>
        private void CreateSearchKbn() {

            ListItem[] liArr = new ListItem[2];
            liArr[0] = new ListItem( CONST_IN_PRODUCT, CONST_ORDER_NO );
            liArr[1] = new ListItem( CONST_REGISTERED, CONST_DPF_LIST );

            ControlUtils.SetListControlItems( rblSearchKbn, liArr );
        }

        /// <summary>
        /// 部品区分の設定
        /// </summary>
        private void CreatePartsKbn() {

            if ( rblProductKind.SelectedValue.Equals( ProductKind.Engine ) ) {
                //エンジン選択
                ListItem[] liArr = new ListItem[2];
                liArr[0] = new ListItem( CONST_PARTS_DPF, PartsKind.PARTS_CD_ENGINE_DPF );
                liArr[1] = new ListItem( CONST_PARTS_DOC, PartsKind.PARTS_CD_ENGINE_DOC );

                ControlUtils.SetListControlItems( ddlParts, liArr );

            } else if ( rblProductKind.SelectedValue.Equals( ProductKind.Tractor ) ) {
                //トラクタ選択
                ListItem[] liArr = new ListItem[3];
                liArr[0] = new ListItem( CONST_PARTS_DPF, PartsKind.PARTS_CD_ENGINE_DPF );
                liArr[1] = new ListItem( CONST_PARTS_SCR, PartsKind.PARTS_CD_ENGINE_SCR );
                liArr[2] = new ListItem( CONST_PARTS_ACU, PartsKind.PARTS_CD_ENGINE_ACU );

                ControlUtils.SetListControlItems( ddlParts, liArr );
            }else{
                //部品選択
                ListItem[] liArr = new ListItem[4];
                liArr[0] = new ListItem( CONST_PARTS_DPF, PartsKind.PARTS_CD_ENGINE_DPF );
                liArr[1] = new ListItem( CONST_PARTS_DOC, PartsKind.PARTS_CD_ENGINE_DOC );
                liArr[2] = new ListItem( CONST_PARTS_SCR, PartsKind.PARTS_CD_ENGINE_SCR );
                liArr[3] = new ListItem( CONST_PARTS_ACU, PartsKind.PARTS_CD_ENGINE_ACU );

                ControlUtils.SetListControlItems( ddlParts, liArr );
            }

        }
        /// <summary>
        /// 抜取検査の設定
        /// </summary>
        private void CreateCheckSample() {

            ListItem[] liArr = new ListItem[2];
            liArr[0] = new ListItem( "対象", "1" );
            liArr[1] = new ListItem( "対象外", "0" );

            liArr = Common.DataUtils.InsertBlankItem( liArr );  //空白行追加
            ControlUtils.SetListControlItems( ddlCheckSample, liArr );
        }

        /// <summary>
        /// 登録種別の設定
        /// </summary>
        private void CreateRegKind() {

            ListItem[] liArr = new ListItem[2];
            liArr[0] = new ListItem( "通常", "0" );
            liArr[1] = new ListItem( "強制", "1" );

            liArr = Common.DataUtils.InsertBlankItem( liArr );  //空白行追加
            ControlUtils.SetListControlItems( ddlRegKind, liArr );
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

            //Gridカラム再作成
            grvMainViewRB.Columns.Clear();
            for ( int idx = frozenGrid.Columns.Count; idx < GridViewResults.Length; idx++ ) {
                TemplateField tf = new TemplateField();
                tf.HeaderText = StringUtils.ToString( GridViewResults[idx].bindField );
                grvMainViewRB.Columns.Add( tf );
            }

            ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, true ), cond, true );
            ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, false ), cond, true );

            ControlUtils.BindGridView_WithTempField( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, true ), ConditionInfo.ResultData );
            ControlUtils.BindGridView_WithTempField( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, false ), ConditionInfo.ResultData );

            ControlUtils.GridViewPageIndexChanging( grvMainViewLB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
            ControlUtils.GridViewPageIndexChanging( grvMainViewRB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );

            ControlUtils.SetGridViewPager( ref pnlPager, grvMainViewRB, grvMainView_PageIndexChanging, ConditionInfo.ResultData.Rows.Count, grvMainViewRB.PageIndex );

            //グリッドビュー表示列情報修正
            SetGridViewColumns();

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
//            ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_MAIN_VIEW_GROUP_CD );
            ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_MAIN_VIEW_GROUP_CD, ControlUtils.GridRowDoubleClickEvent.WindowOpen, PageInfo.ResolveClientUrl( this, PageInfo.MaintenanceDpfListDetail ), base.Token );

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

//            ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_MAIN_VIEW_GROUP_CD );
            ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_MAIN_VIEW_GROUP_CD, ControlUtils.GridRowDoubleClickEvent.WindowOpen, PageInfo.ResolveClientUrl( this, PageInfo.MaintenanceDpfListDetail ), base.Token );

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

            //グリッドビュー表示列情報修正
            SetGridViewColumns();

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

            if ( rblSearchKbn.SelectedValue.Equals( CONST_ORDER_NO ) ) {
                //投入順序
                base.GetControlValues( ConditionControlsJun, ref dicCondition );
            } else {
                //登録済
                base.GetControlValues( ConditionControls, ref dicCondition );
            }

            //検索時画面情報取得
            Dictionary<string, string> dicIdWithText = new Dictionary<string, string>();
            if ( rblSearchKbn.SelectedValue.Equals( CONST_ORDER_NO ) ) {
                //投入順序
                base.GetControlTexts( ConditionControlsJun, out dicIdWithText );
            } else {
                //登録済
                base.GetControlTexts( ConditionControls, out dicIdWithText );
            }

            //一覧表示列の設定
            GridViewDefine[] gridColumns;
            List<GridViewDefine> columns = new List<GridViewDefine>();

            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_COMMON ) ) );
            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_TRACTOR ) ) );
            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_ENGINE ) ) );

            //更新情報
            if ( rblSearchKbn.SelectedValue.Equals( CONST_DPF_LIST ) ) { 
                columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_COMMON_TAIL ) ) );
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
                    //順序選択
                    result.ListTable = TractorProcessDao.SelectSagyoKeepList( dicCondition, maxGridViewCount );
                } else {
                    //登録済選択
                    result.ListTable = EngineProcessDao.SelectDpfSerialList( dicCondition, maxGridViewCount );
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

                //従業員情報取得
                getEmpInfo();

                foreach ( DataRow row in tblResult.Rows ) {
                    if ( rblSearchKbn.SelectedValue.Equals( CONST_DPF_LIST ) ) {
                        //「名称」で上書きする
                        if ( StringUtils.IsNotEmpty( StringUtils.ToString( row["UPDATE_BY"] ) ) ) {
                            if ( _dicEmp.ContainsKey( StringUtils.ToString( row["UPDATE_BY"] ) ) ) {
                                //存在する
                                row["UPDATE_BY"] = _dicEmp[StringUtils.ToString( row["UPDATE_BY"] )];
                            } else {
                                //存在しない
                                row["UPDATE_BY"] = "";
                            }
                        }
                        //DPF型式
                        row["DPF_MODEL_CD"] = DataUtils.GetModelCdStr( StringUtils.ToString( row["DPF_MODEL_CD"] ) );
                    }
                    row["TRC_MODEL_CD"] = DataUtils.GetModelCdStr( StringUtils.ToString( row["TRC_MODEL_CD"] ) );
                    row["ENG_MODEL_CD"] = DataUtils.GetModelCdStr( StringUtils.ToString( row["ENG_MODEL_CD"] ) );

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

                    //TemplateFieldの再作成
                    grvHeaderRT.Columns.Clear();
                    grvMainViewRB.Columns.Clear();
                    for ( int idx = frozenGrid.Columns.Count; idx < GridViewResults.Length; idx++ ) {
                        TemplateField tf = new TemplateField();
                        tf.HeaderText = StringUtils.ToString( GridViewResults[idx].bindField );
                        grvMainViewRB.Columns.Add( tf );
                    }

                    //新規バインド
                    ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, true ),ConditionInfo, true );
                    ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, false ),ConditionInfo, true );
                    ControlUtils.BindGridView_WithTempField( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, true ), tblResult );
                    ControlUtils.BindGridView_WithTempField( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, false ), tblResult );

                    //ページャー作成
                    ControlUtils.SetGridViewPager( ref pnlPager, grvMainViewLB, grvMainView_PageIndexChanging, cond.ResultData.Rows.Count, grvMainViewLB.PageIndex );

                    //グリッドビュー表示列情報修正
                    SetGridViewColumns();

                    //GridView表示
                    divGrvDisplay.Visible = true;

                    //グリッドビュー外のDivサイズ変更
                    SetDivGridViewWidth();
                } else {
                    ClearGridView();
                }
            }

            //項目表示制御
            SetGridViewColumns();

            //メッセージ表示
            if ( null != result.Message ) {
                base.WriteApplicationMessage( result.Message );
            }

            return;
        }
        #endregion

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
            //処理なし
            if ( rblSearchKbn.SelectedValue.Equals( CONST_ORDER_NO ) ) {
                //投入順序での検索時
                GRID_COMMON.ATTACHMENT_DT.visible = false;
                GRID_COMMON.SAMPLE_CHECK.visible = false;
                GRID_COMMON.DPF_MODEL_CD.visible = false;
                GRID_COMMON.DPF_SERIAL_NO.visible = false;

                GRID_COMMON.JUN_NO.visible = true;
                GRID_COMMON.KAN_YO_YM.visible = true;            
            }else{
                //登録済データ検索時
                GRID_COMMON.JUN_NO.visible = false;
                GRID_COMMON.KAN_YO_YM.visible = false;

                GRID_COMMON.ATTACHMENT_DT.visible = true;
                GRID_COMMON.SAMPLE_CHECK.visible = true;
                GRID_COMMON.DPF_MODEL_CD.visible = true;
                GRID_COMMON.DPF_SERIAL_NO.visible = true;
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

        /// <summary>
        /// 検索区分変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblSearchKbn_SelectedIndexChanged( object sender, EventArgs e ) {
            base.RaiseEvent( ChangeSearchKbn );
        }
        /// <summary>
        /// 検索区分変更の関連項目の制御
        /// </summary>
        private void ChangeSearchKbn() {

            //投入順序の場合は登録日使用不可
            if ( rblSearchKbn.SelectedValue.Equals( CONST_ORDER_NO ) ) {
                //取付年月日
                cldAssemblyStart.Value = null;
                cldAssemblyEnd.Value = null;
                cldAssemblyStart.Enabled = false;
                cldAssemblyEnd.Enabled = false;

                //完成予定日
                if ( ObjectUtils.IsNull( cldCompStart.Value ) ) { 
                    cldCompStart.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, 1 );
                }
                if ( ObjectUtils.IsNull( cldCompEnd.Value ) ) {
                    cldCompEnd.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth( DateTime.Today.Year, DateTime.Today.Month ) );
                }
                cldCompStart.Enabled = true;
                cldCompEnd.Enabled = true;

                //部品区分
                ddlParts.Items.Clear();
                ddlParts.Enabled = false;

                //抜取検査
                ddlCheckSample.Items.Clear();
                ddlCheckSample.Enabled = false;

                //登録種別
                ddlRegKind.Items.Clear();
                ddlRegKind.Enabled = false;

            } else {
                //取付年月日
                if ( ObjectUtils.IsNull( cldAssemblyStart.Value ) ) {
                    cldAssemblyStart.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, 1 );
                }
                if ( ObjectUtils.IsNull( cldAssemblyEnd.Value ) ) {
                    cldAssemblyEnd.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth( DateTime.Today.Year, DateTime.Today.Month ) );
                }
                cldAssemblyStart.Enabled = true;
                cldAssemblyEnd.Enabled = true;

                //完成予定日
                cldCompStart.Value = null;
                cldCompEnd.Value = null;
                cldCompStart.Enabled = false;
                cldCompEnd.Enabled = false;

                //部品区分
                CreatePartsKbn();
                ddlParts.SelectedValue = "";
                ddlParts.Enabled = true;

                //抜取検査
                CreateCheckSample();
                ddlCheckSample.SelectedValue = "";
                ddlCheckSample.Enabled = true;

                //登録種別
                CreateRegKind();
                ddlRegKind.SelectedValue = "";
                ddlRegKind.Enabled = true;
            }

            SetGridViewColumns();

        }

        /// <summary>
        /// 製品種別変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblProductKind_SelectedIndexChanged( object sender, EventArgs e ) {
            base.RaiseEvent( ChangeProductKind );
        }

        /// <summary>
        /// 製品種別変更の関連項目の制御
        /// </summary>
        private void ChangeProductKind() {
            if ( rblProductKind.SelectedValue.Equals( ProductKind.Engine ) ) {
                //エンジン選択
                txtST.Text = CONST_ST_OEM;
                txtLineCd.Text = CONST_LINE_OEM;
                rblSearchKbn.Enabled = true;
                txtModelNm.Enabled = true;
                txtIDNO.Enabled = true;

                txtProductSerial.MaxLength = 7;

            } else if ( rblProductKind.SelectedValue.Equals( ProductKind.Tractor ) ) {
                //トラクタ
                txtST.Text = CONST_ST_HS;
                txtLineCd.Text = CONST_LINE_HS;
                rblSearchKbn.Enabled = true;
                txtModelNm.Enabled = true;
                txtIDNO.Enabled = true;

                txtProductSerial.MaxLength = 7;

            } else {
                //DPF
                rblSearchKbn.SelectedValue = CONST_DPF_LIST;
                rblSearchKbn.Enabled = false;

                txtST.Text = "";
                txtLineCd.Text = "";
                txtModelNm.Value = null;
                txtModelNm.Enabled = false;
                txtIDNO.Value = null;
                txtIDNO.Enabled = false;

                //ACUは17桁なのでMAXに合わせておく
                txtProductSerial.MaxLength = 17;

            }

            ChangeSearchKbn();

        }

        protected void ddlParts_SelectedIndexChanged( object sender, EventArgs e ) {
        }

        protected void ddlCheckSample_SelectedIndexChanged( object sender, EventArgs e ) {

        }

        protected void ddlRegKind_SelectedIndexChanged( object sender, EventArgs e ) {

        }
    }
}