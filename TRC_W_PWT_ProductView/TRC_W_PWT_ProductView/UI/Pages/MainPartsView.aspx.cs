using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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
using KTFramework.C1Common.Excel;

namespace TRC_W_PWT_ProductView.UI.Pages {

    /// <summary>
    /// メイン一覧ページ
    /// </summary>
    public partial class MainPartsView : BaseForm {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義
        /// <summary>
        /// グリッドビューグループ
        /// </summary>
        const string GRID_MAIN_VIEW_GROUP_CD = "MainPartsView";
        #endregion

        #region 検索条件定義
        /// <summary>
        /// 一覧検索条件定義
        /// </summary>
        public class CONDITION {
            /// <summary>部品検索対象</summary>
            public static readonly ControlDefine PARTS_SERACH_TARGET = new ControlDefine( "ddlPartsSearchTarget", "部品検索対象", "partsSearchTarget", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>機種区分</summary>
            public static readonly ControlDefine PARTS_KIND = new ControlDefine( "ddlPartsKind", "機種区分", "partsKind", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>型式コード</summary>
            public static readonly ControlDefine MODEL_CD = new ControlDefine( "txtModelCd", "型式コード", "modelCd", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>型式名</summary>
            public static readonly ControlDefine MODEL_NM = new ControlDefine( "txtModelNm", "型式名", "modelNm", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>製品機番</summary>
            public static readonly ControlDefine SERIAL = new ControlDefine( "txtProductSerial", "部品機番", "serial", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>IDNO</summary>
            public static readonly ControlDefine IDNO = new ControlDefine( "txtIDNO", "IDNO", "idno", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>工程</summary>
            public static readonly ControlDefine PROCESS_CD = new ControlDefine( "ddlProcess", "工程", "processCd", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>品番</summary>
            public static readonly ControlDefine PARTS_NUM = new ControlDefine( "txtPartsNo", "フルアッシ品番", "partsNum", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>対象</summary>
            public static readonly ControlDefine DATE_KIND_CD = new ControlDefine( "ddlDateKind", "対象", "dateKindCd", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>範囲(開始)</summary>
            public static readonly ControlDefine DATE_FROM = new ControlDefine( "cldStart", "範囲", "dateFrom", ControlDefine.BindType.Both, typeof( DateTime ) );
            /// <summary>範囲(終了)</summary>
            public static readonly ControlDefine DATE_TO = new ControlDefine( "cldEnd", "範囲", "dateTo", ControlDefine.BindType.Both, typeof( DateTime ) );
        }
        #endregion

        #region グリッドビュー定義

        /// <summary>
        /// 一覧表示情報コントロール(左下)
        /// </summary>
        public class GRID_SEARCH_CONTROLS_L {
        }
        /// <summary>チェックボックス</summary>

        /// <summary>
        /// 一覧表示情報コントロール(右下)
        /// </summary>
        public class GRID_SEARCH_CONTROLS_R {
        }

        #region 部品
        /// <summary>
        /// 部品：全部品共通検索一覧定義
        /// </summary>
        internal class GRID_PARTS_COMMON {

        }

        /// ATU(主)：ATU投入順序検索一覧定義
        /// </summary>
        internal class GRID_ATU {
            /// <summary>部品状態(生産中/完成)</summary>
            public static readonly GridViewDefine PRODUCT_STATUS = new GridViewDefine( "完成状態", "productStatus", typeof( string ), "", true, HorizontalAlign.Center, 90, true, true );
            /// <summary>機種区分</summary>
            public static readonly GridViewDefine MODEL_TYPE = new GridViewDefine( "機種区分", "modelType", typeof( string ), "", true, HorizontalAlign.Center, 90, true, true );
            /// <summary>部品種別</summary>
            public static readonly GridViewDefine STATUS = new GridViewDefine( "部品種別", "status", typeof( string ), "", true, HorizontalAlign.Center, 110, true, true );
            /// <summary>フルアッシ品番</summary>
            public static readonly GridViewDefine FULL_ASSY_PARTS_NUM = new GridViewDefine( "フルアッシ品番", "fullAssyPartsNum", typeof( string ), "", true, HorizontalAlign.Center, 140, false, true );
            /// <summary>フルアッシ品番表記</summary>
            public static readonly GridViewDefine FULL_ASSY_PARTS_NUM_STR = new GridViewDefine( "フルアッシ品番", "fullAssyPartsNumStr", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <summary>ATU型式</summary>
            public static readonly GridViewDefine MODEL_CD = new GridViewDefine( "ATU型式", "modelCd", typeof( string ), "", true, HorizontalAlign.Center, 100, false, true );
            /// <summary>ATU型式表記</summary>
            public static readonly GridViewDefine MODEL_CD_STR = new GridViewDefine( "ATU型式", "modelCdStr", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>ATU型式名</summary>
            public static readonly GridViewDefine MODEL_NM = new GridViewDefine( "ATU型式名", "modelNm", typeof( string ), "", true, HorizontalAlign.Center, 160, true, true );
            /// <summary>機番</summary>
            public static readonly GridViewDefine SERIAL = new GridViewDefine( "機番", "serial", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>生産指示日</summary>
            public static readonly GridViewDefine CREATE_DT = new GridViewDefine( "生産指示日", "createDt", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <summary>投入日</summary>
            public static readonly GridViewDefine THROW_DT = new GridViewDefine( "投入日", "throwDt", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <summary>完成日</summary>
            public static readonly GridViewDefine COMPLETION_DT = new GridViewDefine( "完成日", "completionDt", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <summary>投入順序連番</summary>
            public static readonly GridViewDefine THROW_MONTHLY_SEQUENCE_NUM = new GridViewDefine( "投入順序連番", "throwMonthlySequenceNum", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            ///// <summary>入口</summary>
            //public static readonly GridViewDefine THROW_DESTINATION = new GridViewDefine( "入口", "throwDestination", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <summary>行先</summary>
            public static readonly GridViewDefine COMPLETION_DESTINATION = new GridViewDefine( "行先", "completionDestination", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <summary>出荷日</summary>
            public static readonly GridViewDefine SHIP_DT = new GridViewDefine("出荷日", "shipDt", typeof(string), "", true, HorizontalAlign.Center, 140, true, true);
            /// <summary>エンジン型式</summary>
            public static readonly GridViewDefine ENGINE_MODEL_CD = new GridViewDefine( "エンジン型式", "engineModelCd", typeof( string ), "", true, HorizontalAlign.Center, 140, false, true );
            /// <summary>エンジン型式表記</summary>
            public static readonly GridViewDefine ENGINE_MODEL_CD_STR = new GridViewDefine( "エンジン型式", "engineModelCdStr", typeof( string ), "", true, HorizontalAlign.Center, 140, false, true );
            /// <summary>エンジン型式名</summary>
            public static readonly GridViewDefine ENGINE_MODEL_NM = new GridViewDefine( "エンジン型式名", "engineModelNm", typeof( string ), "", true, HorizontalAlign.Center, 160, false, true );
            /// <summary>エンジン機番</summary>
            public static readonly GridViewDefine ENGINE_SERIAL = new GridViewDefine( "エンジン機番", "engineSerial", typeof( string ), "", true, HorizontalAlign.Center, 150, false, true );
            /// <summary>エンジンIDNO</summary>
            public static readonly GridViewDefine ENGINE_IDNO = new GridViewDefine( "エンジンIDNO", "engineIdno", typeof( string ), "", true, HorizontalAlign.Center, 150, false, true );
            /// <summary>トラクタ型式</summary>
            public static readonly GridViewDefine TRACTOR_MODEL_CD = new GridViewDefine( "トラクタ型式", "tractorModelCd", typeof( string ), "", true, HorizontalAlign.Center, 140, false, true );
            /// <summary>トラクタ型式表記</summary>
            public static readonly GridViewDefine TRACTOR_MODEL_CD_STR = new GridViewDefine( "トラクタ型式", "tractorModelCdStr", typeof( string ), "", true, HorizontalAlign.Center, 140, false, true );
            /// <summary>トラクタ型式名</summary>
            public static readonly GridViewDefine TRACTOR_MODEL_NM = new GridViewDefine( "トラクタ型式名", "tractorModelNm", typeof( string ), "", true, HorizontalAlign.Center, 160, false, true );
            /// <summary>トラクタ機番</summary>
            public static readonly GridViewDefine TRACTOR_SERIAL = new GridViewDefine( "トラクタ機番", "tractorSerial", typeof( string ), "", true, HorizontalAlign.Center, 150, false, true );
            /// <summary>トラクタIDNO</summary>
            public static readonly GridViewDefine TRACTOR_IDNO = new GridViewDefine( "トラクタIDNO", "tractorIdno", typeof( string ), "", true, HorizontalAlign.Center, 150, false, true );
        }

        /// ATU(副)：ATU機番管理定義
        /// </summary>
        internal class GRID_ATU_SERIAL {
            /// <summary>読込日</summary>
            public static readonly GridViewDefine READ_DT = new GridViewDefine( "読込日", "readDt", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <summary>ステーション名</summary>
            public static readonly GridViewDefine STATION_CD = new GridViewDefine( "ステーション", "stationNm", typeof( string ), "", true, HorizontalAlign.Center, 160, true, true );
            /// <summary>アッシ品番</summary>
            public static readonly GridViewDefine ASSY_NUM = new GridViewDefine( "アッシ品番", "assyPartsNum", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>アッシ機番</summary>
            public static readonly GridViewDefine ASSY_SERIAL = new GridViewDefine( "アッシ機番", "assySerial", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>構成品品番1</summary>
            public static readonly GridViewDefine COMPONENT_PARTS_NUM1 = new GridViewDefine( "構成品品番1", "componentPartsNum1", typeof( string ), "", true, HorizontalAlign.Center, 120, true, true );
            /// <summary>構成品機番1</summary>
            public static readonly GridViewDefine COMPONENT_PARTS_SERIAL1 = new GridViewDefine( "構成品機番1", "componentSerial1", typeof( string ), "", true, HorizontalAlign.Center, 120, true, true );
            /// <summary>構成品品番2</summary>
            public static readonly GridViewDefine COMPONENT_PARTS_NUM2 = new GridViewDefine( "構成品品番2", "componentPartsNum2", typeof( string ), "", true, HorizontalAlign.Center, 120, true, true );
            /// <summary>構成品機番2</summary>
            public static readonly GridViewDefine COMPONENT_PARTS_SERIAL2 = new GridViewDefine( "構成品機番2", "componentSerial2", typeof( string ), "", true, HorizontalAlign.Center, 120, true, true );
        }

        /// ATU(副)：トルク締付定義
        /// </summary>
        internal class GRID_ATU_TORQUE {
            /// <summary>作成日時</summary>
            public static readonly GridViewDefine TQ_CREATE_DT = new GridViewDefine("作成日時", "tqCreateDt", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true);
            /// <summary>ステーション</summary>
            public static readonly GridViewDefine STATION_CD = new GridViewDefine( "ステーション", "stationNm", typeof( string ), "", true, HorizontalAlign.Center, 160, true, true );
            /// <summary>端末名</summary>
            public static readonly GridViewDefine TERMINAL_NM = new GridViewDefine( "端末名", "terminalNm", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>結果</summary>
            public static readonly GridViewDefine RESULT = new GridViewDefine( "結果", "result", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
        }

        /// ATU(副)：リーク計測定義
        /// </summary>
        internal class GRID_ATU_LEAK {
            /// <summary>計測日時</summary>
            public static readonly GridViewDefine MEASURE_DT = new GridViewDefine("計測日時", "measureDt", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true);
            /// <summary>ステーション</summary>
            public static readonly GridViewDefine STATION_CD = new GridViewDefine( "ステーション", "stationNm", typeof( string ), "", true, HorizontalAlign.Center, 160, true, true );
            /// <summary>計測器</summary>
            public static readonly GridViewDefine MEASURE_TESTER = new GridViewDefine( "計測器", "measureTester", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <summary>履歴NO</summary>
            public static readonly GridViewDefine RECORD_NUM = new GridViewDefine( "履歴NO", "recordNm", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>総合判定</summary>
            public static readonly GridViewDefine TOTAL_JUDGE = new GridViewDefine( "総合判定", "totalJudge", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
        }

        #endregion

        #endregion

        #region プロパティ

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
        GridViewDefine[] _gridviewDefault = null;
        /// <summary>
        /// 一覧定義情報アクセサ
        /// </summary>
        GridViewDefine[] gridviewDefault {
            get {
                if ( true == ObjectUtils.IsNull( _gridviewDefault ) ) {
                    _gridviewDefault = ControlUtils.GetGridViewDefineArray( typeof( GRID_PARTS_COMMON ) );
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
            base.RaiseEvent( DoPageLoad, false );
        }

        /// <summary>
        /// 検索区分切替動作（製品検索）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChangeProductSearch_Click( object sender, EventArgs e ) {
            base.RaiseEvent( MoveToProductSearch );
        }

        /// <summary>
        /// 検索区分切替動作（工程検索）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChangeProcessSearch_Click( object sender, EventArgs e ) {
            base.RaiseEvent( MoveToProcessSearch );
        }

        /// <summary>
        /// 部品検索対象選択動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPartsSearchTarget_SelectedIndexChanged( object sender, EventArgs e ) {

            base.RaiseEvent( ChangePartsSearchTarget );
        }

        /// <summary>
        /// 部品検索種別選択動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPartsKind_SelectedIndexChanged( object sender, EventArgs e ) {

            base.RaiseEvent( ChangePartsKind );
        }


        /// <summary>
        /// 型式区分リスト選択動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblModelType_SelectedIndexChanged( object sender, EventArgs e ) {
            base.RaiseEvent( ChangeModelType );
        }

        /// <summary>
        /// 工程リスト選択動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProcess_SelectedIndexChanged( object sender, EventArgs e ) {
            base.RaiseEvent( ChangeProcess );
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
        /// グリッドビュー行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMainPartsView_RowDataBound( object sender, GridViewRowEventArgs e ) {
            base.RaiseEvent( RowDataBoundMainPartsView, sender, e );
            RowDataBoundMainPartsView( sender, e );
        }

        /// <summary>
        /// グリッドビューページチェンジイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMainPartsView_PageIndexChanging( object sender, CommandEventArgs e ) {
            base.RaiseEvent( PageIndexChangingMainPartsView, sender, e );
        }

        /// <summary>
        /// グリッドビュー並び替えイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMainPartsView_Sorting( object sender, GridViewSortEventArgs e ) {
            base.RaiseEvent( SortingMainPartsView, sender, e );
        }

        protected void btnExcel_Click( object sender, EventArgs e ) {
            base.RaiseEvent( OutputExcel );
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
            GridView frozenGrid = grvMainPartsViewLB;
            ControlUtils.SetGridViewTemplateField( grvMainPartsViewLB, ControlUtils.GetFrozenColumns( frozenGrid, gridviewDefault, true ) );
            ControlUtils.SetGridViewTemplateField( grvMainPartsViewRB, ControlUtils.GetFrozenColumns( frozenGrid, gridviewDefault, false ) );

            ControlUtils.ReMakeBoundTemplateFieldCtrl( grvMainPartsViewLB, ControlUtils.GetFrozenColumns( frozenGrid, gridviewDefault, true ) );
            ControlUtils.ReMakeBoundTemplateFieldCtrl( grvMainPartsViewRB, ControlUtils.GetFrozenColumns( frozenGrid, gridviewDefault, false ) );

            int resultCnt = 0;
            if ( ObjectUtils.IsNotNull( ConditionInfo.ResultData ) ) {
                resultCnt = ConditionInfo.ResultData.Rows.Count;
            }

            ControlUtils.SetGridViewPager( ref pnlPager, grvMainPartsViewRB, grvMainPartsView_PageIndexChanging, resultCnt, grvMainPartsViewRB.PageIndex );


        }

        #endregion

        protected void KTButton1_Click( object sender, EventArgs e ) {

        }

        /// <summary>
        /// 検索区分変更(製品検索)
        /// </summary>
        private void MoveToProductSearch() {
            PageUtils.RedirectToTRC( this.Page, CurrentPageInfo.pageId, PageInfo.MainView.url, this.Token, null );
        }

        /// <summary>
        /// 検索区分変更(工程検索)
        /// </summary>
        private void MoveToProcessSearch() {
            PageUtils.RedirectToTRC( this.Page, CurrentPageInfo.pageId, PageInfo.MainProcessView.url, this.Token, null );
        }

        /// <summary>
        /// 部品検索対象変更
        /// </summary>
        private void ChangePartsSearchTarget() {

            string partsSearchCd = DataUtils.GetDictionaryStringVal( ConditionInfo.conditionValue, MainPartsView.CONDITION.PARTS_SERACH_TARGET.bindField );  //部品検索区分

            if ( PartsSearchTarget.Atu == ddlPartsSearchTarget.SelectedValue ) {
                ControlUtils.SetListControlItems( ddlPartsKind, PartsSearchKind.GetList( PartsSearchTarget.Atu ) );
            }

            if ( PartsSearchTarget.Atu == ddlPartsSearchTarget.SelectedValue ) {
                //日付リスト変更処理
            }


            //日付区分リスト変更
            SetDateKindList();

        }

        /// <summary>
        /// 部品検索種別変更
        /// </summary>
        private void ChangePartsKind() {
        }

        /// <summary>
        /// 型式区分変更
        /// </summary>
        private void ChangeModelType() {
        }


        /// <summary>
        /// 工程変更
        /// </summary>
        private void ChangeProcess() {
        }

        #endregion

        #region 検索処理

        /// <summary>
        /// 検索処理
        /// </summary>
        private void DoSearch() {

            //チェック処理
            if ( false == ChackCondition() ) {
                return;
            }

            //検索パラメータ作成
            Dictionary<string, object> dicCondition = new Dictionary<string, object>();

            base.GetControlValues( ConditionControls, ref dicCondition );
            KTBindParameters parameters = DataUtils.GetBindParameters( ConditionControls, dicCondition );

            //検索時画面情報取得
            Dictionary<string, string> dicIdWithText = new Dictionary<string, string>();
            base.GetControlTexts( ConditionControls, out dicIdWithText );

            //一覧表示列の設定
            string partsSearchCd = DataUtils.GetDictionaryStringVal( dicCondition, MainPartsView.CONDITION.PARTS_SERACH_TARGET.bindField );       //部品検索区分
            string processCd = DataUtils.GetDictionaryStringVal( dicCondition, MainPartsView.CONDITION.PROCESS_CD.bindField );                    //工程区分

            GridViewDefine[] GridViewResults = GetListColumns( partsSearchCd, processCd );

            //検索結果取得
            //エラー時にも最後まで処理を通すため、インスタンスを生成しておく
            MainPartsViewBusiness.ResultSet result = new MainPartsViewBusiness.ResultSet();
            DataTable tblResult = null;
            int maxGridViewCount = WebAppInstance.GetInstance().Config.WebCommonInfo.partsViewMaxGridViewCount;  //検索上限数
            try {
                result = MainPartsViewBusiness.Search( dicCondition, GridViewResults, maxGridViewCount );
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

            }

            ConditionInfoSessionHandler.ST_CONDITION cond = new ConditionInfoSessionHandler.ST_CONDITION();
            tblResult = result.ListTable;
            if ( null != tblResult ) {
                //初期オーダー設定
                tblResult.DefaultView.Sort = GetListInitOrder( dicCondition );

                //件数表示
                ntbResultCount.Value = tblResult.Rows.Count;

                //ページャー設定
                ControlUtils.SetGridViewPager( ref pnlPager, grvMainPartsViewRB, grvMainPartsView_PageIndexChanging, tblResult.Rows.Count, 0 );

                //検索条件/結果インスタンス
                cond.conditionValue = dicCondition;
                cond.IdWithText = dicIdWithText;
                cond.ResultData = tblResult.DefaultView.ToTable();
            } else {
                //タイムアウト等Exception時には、GridViewクリア
                ClearGridView();
            }

            //検索条件をセッションに格納
            ConditionInfo = cond;

            GridView frozenGrid = grvMainPartsViewLB;
            if ( true == ObjectUtils.IsNotNull( tblResult ) ) {
                if ( 0 < tblResult.Rows.Count ) {

                    //TemplateFieldの追加
                    grvHeaderRT.Columns.Clear();
                    grvMainPartsViewRB.Columns.Clear();
                    for ( int idx = frozenGrid.Columns.Count; idx < GridViewResults.Length; idx++ ) {
                        TemplateField tf = new TemplateField();
                        tf.HeaderText = StringUtils.ToString( GridViewResults[idx].bindField );
                        grvMainPartsViewRB.Columns.Add( tf );
                    }

                    //新規バインド
                    GridViewDefine[] tmpGridView = remakeGridView();
                    ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, tmpGridView, true ), ConditionInfo, true );
                    ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, tmpGridView, false ), ConditionInfo, true );
                    ControlUtils.BindGridView_WithTempField( grvMainPartsViewLB, ControlUtils.GetFrozenColumns( frozenGrid, tmpGridView, true ), tblResult );
                    ControlUtils.BindGridView_WithTempField( grvMainPartsViewRB, ControlUtils.GetFrozenColumns( frozenGrid, tmpGridView, false ), tblResult );

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

            //メッセージ表示
            if ( null != result.Message ) {
                base.WriteApplicationMessage( result.Message );
            }

            //Excel出力ボタン活性
            if ( null != tblResult && 0 < tblResult.Rows.Count ) {
                this.divGrvCtrlButton.Visible = true;  //出力対象データあり
            } else {
                this.divGrvCtrlButton.Visible = false; //出力対象データなし
            }

            return;
        }

        /// <summary>
        /// 検索条件チェック処理
        /// </summary>
        private bool ChackCondition() {

            return true;
        }

        #endregion

        #region グリッドビューイベント

        /// <summary>
        /// グリッドビュー行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowDataBoundMainPartsView( params object[] parameters ) {
            object sender = parameters[0];
            GridViewRowEventArgs e = (GridViewRowEventArgs)parameters[1];

            ControlUtils.GridViewRowBound( (GridView)sender, e, ControlUtils.GridRowDoubleClickEvent.None );
        }

        /// <summary>
        /// グリッドビューページ切替
        /// </summary>
        /// <param name="parameters"></param>
        private void PageIndexChangingMainPartsView( params object[] parameters ) {
            object sender = parameters[0];

            CommandEventArgs e = (CommandEventArgs)parameters[1];
            int newPageIndex = Convert.ToInt32( e.CommandArgument );

            int pageSize = grvMainPartsViewRB.PageSize;
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
            GridView frozenGrid = grvMainPartsViewLB;

            GridViewDefine[] tmpGridView = remakeGridView();
            ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, tmpGridView, true ), cond, true );
            ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, tmpGridView, false ), cond, true );

            ControlUtils.BindGridView_WithTempField( grvMainPartsViewLB, ControlUtils.GetFrozenColumns( frozenGrid, tmpGridView, true ), ConditionInfo.ResultData );
            ControlUtils.BindGridView_WithTempField( grvMainPartsViewRB, ControlUtils.GetFrozenColumns( frozenGrid, tmpGridView, false ), ConditionInfo.ResultData );

            ControlUtils.GridViewPageIndexChanging( grvMainPartsViewLB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
            ControlUtils.GridViewPageIndexChanging( grvMainPartsViewRB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );

            ControlUtils.SetGridViewPager( ref pnlPager, grvMainPartsViewRB, grvMainPartsView_PageIndexChanging, ConditionInfo.ResultData.Rows.Count, grvMainPartsViewRB.PageIndex );

            //グリッドビュー表示列情報修正
            SetGridViewColumns();

            //グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();
        }
        /// <summary>
        /// グリッドビューの列コントロール制御
        /// </summary>
        /// <remarks>定義情報で表示となっている列に対して権限情報でグリッドビューの列情報を動的に変更</remarks>
        private void SetGridViewColumns() {

            //制御なし

        }
        /// <summary>
        /// グリッドビュー並び替え
        /// </summary>
        /// <param name="parameters"></param>
        private void SortingMainPartsView( params object[] parameters ) {
            object sender = parameters[0];
            GridViewSortEventArgs e = (GridViewSortEventArgs)parameters[1];

            ConditionInfoSessionHandler.ST_CONDITION cond = ConditionInfo;
            ControlUtils.GridViewSorting( grvMainPartsViewLB, ref cond, e, true );
            ControlUtils.GridViewSorting( grvMainPartsViewRB, ref cond, e );

            //背面ユーザ切替対応
            GridView frozenGrid = grvMainPartsViewLB;
            GridViewDefine[] tmpGridView = remakeGridView();
            ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, tmpGridView, true ), cond, true );
            ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, tmpGridView, false ), cond, true );

            ControlUtils.BindGridView_WithTempField( grvMainPartsViewLB, ControlUtils.GetFrozenColumns( frozenGrid, tmpGridView, true ), cond.ResultData );
            ControlUtils.BindGridView_WithTempField( grvMainPartsViewRB, ControlUtils.GetFrozenColumns( frozenGrid, tmpGridView, false ), cond.ResultData );

            ControlUtils.SetGridViewPager( ref pnlPager, grvMainPartsViewRB, grvMainPartsView_PageIndexChanging, cond.ResultData.Rows.Count, grvMainPartsViewRB.PageIndex );

            ConditionInfo = cond;

            //グリッドビュー表示列情報修正
            SetGridViewColumns();

            //グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();
        }
        #endregion

        #region Excel出力
        /// <summary>
        /// Excel出力処理
        /// </summary>
        private void OutputExcel() {
            //セッションから検索データの取得
            ConditionInfoSessionHandler.ST_CONDITION cond = base.ConditionInfo;
            if ( null == cond.ResultData || 0 == cond.ResultData.Rows.Count ) {
                //出力対象データなし
                return;
            }

            //検索条件出力データ作成
            List<ExcelConditionItem> excelCond = new List<ExcelConditionItem>();
            string condition = "";
            string value = "";

            //部品
            condition = CONDITION.PARTS_SERACH_TARGET.displayNm;
            value = cond.IdWithText[CONDITION.PARTS_SERACH_TARGET.controlId];
            excelCond.Add(new ExcelConditionItem(condition, value));

            //機種区分
            condition = CONDITION.PARTS_KIND.displayNm;
            value = cond.IdWithText[CONDITION.PARTS_KIND.controlId];
            excelCond.Add(new ExcelConditionItem(condition, value));

            //型式コード
            condition = CONDITION.MODEL_CD.displayNm;
            value = cond.IdWithText[CONDITION.MODEL_CD.controlId];
            excelCond.Add( new ExcelConditionItem( condition, value ) );

            //型式名
            condition = CONDITION.MODEL_NM.displayNm;
            value = cond.IdWithText[CONDITION.MODEL_NM.controlId];
            excelCond.Add( new ExcelConditionItem( condition, value ) );

            //部品機番
            condition = CONDITION.SERIAL.displayNm;
            value = cond.IdWithText[CONDITION.SERIAL.controlId];
            excelCond.Add( new ExcelConditionItem( condition, value ) );

            //工程
            condition = CONDITION.PROCESS_CD.displayNm;
            value = cond.IdWithText[CONDITION.PROCESS_CD.controlId];
            excelCond.Add( new ExcelConditionItem( condition, value ) );

            //品番
            condition = CONDITION.PARTS_NUM.displayNm;
            value = cond.IdWithText[CONDITION.PARTS_NUM.controlId];
            excelCond.Add( new ExcelConditionItem( condition, value ) );

            //対象(日付区分)
            condition = CONDITION.DATE_KIND_CD.displayNm;
            value = cond.IdWithText[CONDITION.DATE_KIND_CD.controlId];
            excelCond.Add( new ExcelConditionItem( condition, value ) );

            //範囲(日付)
            condition = CONDITION.DATE_FROM.displayNm;
            value = cond.IdWithText[CONDITION.DATE_FROM.controlId] + "～" + cond.IdWithText[CONDITION.DATE_TO.controlId];
            excelCond.Add( new ExcelConditionItem( condition, value ) );

            //Excelダウンロード実行(テーブルはExcel出力用に加工する)
            try {
                Excel.Download( Response, "部品情報", GetExcelTable( cond.ResultData ), excelCond );
            } catch ( System.Threading.ThreadAbortException ) {
                //response.Endで必ず発生する為、正常として扱う
            } catch ( Exception ex ) {
                logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80030, "部品情報_検索結果" );
            }

            return;
        }
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

            //初期化、初期値設定
            InitializeValues();
        }

        /// <summary>
        /// 初期化、初期値設定
        /// </summary>
        private void InitializeValues() {

            //■固定リスト項目

            //部品検索対象リスト
            ControlUtils.SetListControlItems( ddlPartsSearchTarget, PartsSearchTarget.GetList() );

            ControlUtils.SetControlValue( lblPartsNum, "フルアッシ品番" );

            //部品検索種別リスト
            ControlUtils.SetListControlItems( ddlPartsKind, PartsSearchKind.GetList( PartsSearchTarget.Atu ) );

            //■初期値設定

            //検索区分
            ddlPartsSearchTarget.SelectedValue = PartsSearchTarget.Atu;

            //ATU工程区分リスト
            ControlUtils.SetListControlItems( ddlProcess, AtuProcessKind.GetList() );

            //型式区分、工程部品区分に応じた日付リスト作成
            SetDateKindList();

            //日付(開始、終了 初期値)
            //当月月初
            cldStart.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, 1 );
            //当月月末
            cldEnd.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth( DateTime.Today.Year, DateTime.Today.Month ) );

            ConditionInfo = new ConditionInfoSessionHandler.ST_CONDITION();

            //グリッドビュー初期化
            ClearGridView();

            //Excel出力ボタン非活性
            this.divGrvCtrlButton.Visible = false;

        }

        #endregion

        #region グリッドビュー

        /// <summary>
        /// グリッドビュークリア
        /// </summary>
        private void ClearGridView() {

            //列名非表示 グリッドビュークリア
            ControlUtils.InitializeGridView( grvHeaderLT, false );
            ControlUtils.InitializeGridView( grvHeaderRT, false );
            ControlUtils.InitializeGridView( grvMainPartsViewLB, false );
            ControlUtils.InitializeGridView( grvMainPartsViewRB, false );

            //グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();

            //件数表示
            ntbResultCount.Value = 0;

            //ページャークリア
            ControlUtils.ClearPager( ref pnlPager );

            //GridView非表示
            divGrvDisplay.Visible = false;

        }

        /// <summary>
        /// グリッドビュー外のDivサイズ変更
        /// </summary>
        private void SetDivGridViewWidth() {

            SetDivGridViewWidth( grvHeaderLT, divGrvHeaderLT );
            SetDivGridViewWidth( grvHeaderRT, divGrvHeaderRT );

            SetDivGridViewWidth( grvMainPartsViewLB, divGrvLB );
            SetDivGridViewWidth( grvMainPartsViewRB, divGrvRB );
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
        /// 一覧初期ソート
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <returns>DataViewソート文字列</returns>
        private string GetListInitOrder( Dictionary<string, object> condition ) {
            string order = "";
            //投入順序連番の昇順に並替
            order = "throwMonthlySequenceNum";

            return order;
        }
        #endregion

        #region 動的リスト生成

        /// <summary>
        /// 日付区分リスト変更
        /// </summary>
        private void SetDateKindList() {

            //選択項目を退避
            string preSelectedCd = ddlDateKind.SelectedValue;

            //日付区分リスト取得
            ControlUtils.SetListControlItems( ddlDateKind,
                DateKind.GetList( ddlPartsSearchTarget.SelectedValue, false ) );

            //元選択項目をセット(リストに存在しない場合には、コントロール内で未選択に変更する)
            if ( true == ObjectUtils.IsNotNull( ddlDateKind.Items.FindByValue( preSelectedCd ) ) ) {
                ddlDateKind.SelectedValue = preSelectedCd;
            } else {
                //リストに無い場合(初期化含む)には、デフォルト値をセットする
                //日付区分 0003:出荷
                ddlDateKind.SelectedValue = DateKind.ProductClass.Shipped.value;
            }
        }


        /// <summary>
        /// 検索時に指定した製品区分、および部品/工程区分から、一覧表示列定義を取得する
        /// </summary>
        /// <param name="productKindCd">製品区分コード</param>
        /// <param name="partsCd">部品区分コード</param>
        /// <param name="processCd">工程区分コード</param>
        /// <returns></returns>
        private GridViewDefine[] GetListColumns( string partsSearchCd, string processCd ) {
            List<GridViewDefine> columns = new List<GridViewDefine>();

            //部品一覧列を取得
            if ( partsSearchCd == PartsSearchTarget.Atu ) {

                columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_ATU ) ) );

                if ( processCd == AtuProcessKind.ATU_PARTS_SERIAL ) {

                    columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_ATU_SERIAL ) ) );

                } else if ( processCd == AtuProcessKind.ATU_TORQUE_TIGHTENING_RECORD ) {

                    columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_ATU_TORQUE ) ) );

                } else if ( processCd == AtuProcessKind.ATU_LEAK_MEASURE_RESULT ) {

                    columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_ATU_LEAK ) ) );

                }
            }

            return columns.ToArray();
        }

        #endregion

        #region Excel出力テーブル作成
        /// <summary>
        /// 一覧結果Excel出力用データテーブルを作成します
        /// </summary>
        /// <param name="gridColumns">一覧表示対象列</param>
        /// <returns>DataTable</returns>
        private DataTable GetExcelTable( DataTable tblSource ) {
            //Excel出力テーブル定義作成(一覧表示対象列を出力対象とする)
            DataTable tblResult = new DataTable();
            foreach ( DataColumn column in tblSource.Columns ) {
                if ( false == StringUtils.IsBlank( column.Caption ) ) {
                    DataColumn colResult = new DataColumn( column.Caption, column.DataType );
                    tblResult.Columns.Add( colResult );
                }
            }

            //一覧元DataTableの情報をExcel出力用テーブルにコピー
            foreach ( DataRow rowSrc in tblSource.Rows ) {
                DataRow rowTo = tblResult.NewRow();
                foreach ( DataColumn column in tblSource.Columns ) {
                    if ( false == StringUtils.IsBlank( column.Caption ) ) {
                        rowTo[column.Caption] = rowSrc[column.ColumnName];
                    }
                }
                tblResult.Rows.Add( rowTo );
            }
            tblResult.AcceptChanges();

            return tblResult;
        }
        #endregion

        protected void grvMainPartsViewLB_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainPartsViewLB( sender, e );
        }

        #endregion

        protected void grvMainPartsViewRB_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainPartsViewRB( sender, e );
        }
        #region グリッドビューイベント処理

        /// <summary>
        /// グリッドビュー行バインド(左下)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowDataBoundMainPartsViewLB( params object[] parameters ) {
            object sender = parameters[0];
            GridViewRowEventArgs e = (GridViewRowEventArgs)parameters[1];

            if ( e.Row.RowType == DataControlRowType.DataRow ) {
                DataRow row = ( (DataRowView)e.Row.DataItem ).Row;
                Dictionary<string, object> dicControls = new Dictionary<string, object>();
                base.SetControlValues( e.Row, ControlUtils.GetControlDefineArray( typeof( GRID_SEARCH_CONTROLS_L ) ), row, ref dicControls );

                //詳細画面表示を有効にする
                ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_MAIN_VIEW_GROUP_CD, ControlUtils.GridRowDoubleClickEvent.WindowOpen, PageInfo.ResolveClientUrl( this, PageInfo.DetailPartsFrame ), base.Token );

            }
        }

        /// <summary>
        /// グリッドビュー行バインド(右下)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowDataBoundMainPartsViewRB( params object[] parameters ) {
            object sender = parameters[0];
            GridViewRowEventArgs e = (GridViewRowEventArgs)parameters[1];

            if ( e.Row.RowType == DataControlRowType.DataRow ) {
                DataRow rowData = ( (DataRowView)e.Row.DataItem ).Row;
                Dictionary<string, object> dicControls = new Dictionary<string, object>();
                base.SetControlValues( e.Row, ControlUtils.GetControlDefineArray( typeof( GRID_SEARCH_CONTROLS_R ) ), rowData, ref dicControls );

                //詳細画面表示を有効にする
                ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_MAIN_VIEW_GROUP_CD, ControlUtils.GridRowDoubleClickEvent.WindowOpen, PageInfo.ResolveClientUrl( this, PageInfo.DetailPartsFrame ), base.Token );

            }


        }
        #endregion
        #region
        /// <summary>
        /// GridView再構成
        /// </summary>
        /// <returns></returns>
        private GridViewDefine[] remakeGridView() {

            GridViewDefine[] retGridView = null;

            ////一覧表示列の設定
            string partsSearchCd = DataUtils.GetDictionaryStringVal( ConditionInfo.conditionValue, MainPartsView.CONDITION.PARTS_SERACH_TARGET.bindField );  //部品検索区分
            string processCd = DataUtils.GetDictionaryStringVal( ConditionInfo.conditionValue, MainPartsView.CONDITION.PROCESS_CD.bindField );               //工程区分

            retGridView = GetListColumns( partsSearchCd, processCd );

            return retGridView;
        }
        #endregion

    }
}