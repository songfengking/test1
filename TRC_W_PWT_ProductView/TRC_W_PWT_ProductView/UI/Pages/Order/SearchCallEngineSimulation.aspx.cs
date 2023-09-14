////////////////////////////////////////////////////////////////////////////////////////////////
//      クボタ筑波工場  TRC
//      概要：トレーサビリティシステム 搭載エンジン引当検索
//---------------------------------------------------------------------------
//           Ver 1.44.0.0  :  2021/06/10  豊島    新規作成(java版からリプレース)
//           Ver 1.44.0.1  :  2021/09/28  豊島    07搭載エンジンの判断をAPコードに変更、表示件数の変更
//           Ver 1.44.1.1  :  2021/10/25  豊島　  RowDataBoundのログ出力抑制
//           Ver 2.01.0.0  :  2022/09/29  久保田　引当先倉庫 絞込機能追加
////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KTFramework.C1Common.Excel;
using KTFramework.Common;
using KTFramework.Common.Dao;
using KTFramework.Dao;
using TRC_W_PWT_ProductView.Business;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Dao.Com;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.ListDefine;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.Session;
using TRC_W_PWT_ProductView.UI.Base;

namespace TRC_W_PWT_ProductView.UI.Pages.Order {
    /// <summary>
    /// 搭載エンジン引当検索画面
    /// </summary>
    public partial class SearchCallEngineSimulation : BaseForm {
        #region 定数

        /// <summary>メッセージ(Key)</summary>
        const string MSG_KEY = "MSG";

        /// <summary>次稼働日</summary>
        private const int NEXT_WORK_DAY = 0;
        /// <summary>基準日</summary>
        private const string BASE_DATA_STR = "2001/01/01";
        /// <summary>07搭載エンジン</summary>
        private const string TOUSAI_ENGINE_07 = "14";


        #region 引当結果
        /// <summary>不可(表示用)</summary>
        private const string HIKIATE_RESULT_NOENGINE_DISP = "不可";
        /// <summary>不可</summary>
        private const int HIKIATE_RESULT_NOENGINE = 0;
        /// <summary>可(表示用)</summary>
        private const string HIKIATE_RESULT_STOCKED_DISP = "可";
        /// <summary>可</summary>
        private const int HIKIATE_RESULT_STOCKED = 1;
        /// <summary>予約(表示用)</summary>
        private const string HIKIATE_RESULT_PREPARED_DISP = "予約";
        /// <summary>予約</summary>
        private const int HIKIATE_RESULT_PREPARED = 2;
        /// <summary>完了(表示用)</summary>
        private const string HIKIATE_RESULT_DELIVERED_DISP = "完了";
        /// <summary>完了</summary>
        private const int HIKIATE_RESULT_DELIVERED = 3;
        #endregion

        #region CSS指定(値先頭の半角スペースが必要)
        /// <summary>不可(背景色)</summary>
        private const string HIKIATE_RESULT_NOENGINE_COLOR = " hikiate-result0";
        /// <summary>可(背景色)</summary>
        private const string HIKIATE_RESULT_STOCKED_COLOR = " hikiate-result1";
        /// <summary>予約(背景色)</summary>
        private const string HIKIATE_RESULT_PREPARED_COLOR = " hikiate-result2";
        /// <summary>完了(背景色)</summary>
        private const string HIKIATE_RESULT_DELIVERED_COLOR = " hikiate-result3";
        /// <summary>07搭載エンジン(背景色)</summary>
        private const string TOUSAI_ENGINE_07_COLOR = " tousai-engine-07";
        #endregion

        #region 検索条件
        /// <summary>
        /// 検索条件
        /// </summary>
        public class CONDITION {
            /// <summary>
            /// (T)完成予定日
            /// </summary>
            public static readonly ControlDefine T_KANSEI_YOTEI_YMD = new ControlDefine( "cldTKanseiYoteiYmd", "(T)完成予定日", "cldTKanseiYoteiYmd", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// (T)IDNO
            /// </summary>
            public static readonly ControlDefine T_IDNO = new ControlDefine( "txtTIdno", "(T)IDNO", "txtTIdno", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// (T)型式コード
            /// </summary>
            public static readonly ControlDefine T_KATASHIKI_CODE = new ControlDefine( "txtTKatashikiCd", "(T)型式コード", "txtTKatashikiCd", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// (T)型式名
            /// </summary>
            public static readonly ControlDefine T_KATASHIKI_NAME = new ControlDefine( "txtTKatashikiNm", "(T)型式名", "txtTKatashikiNm", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// (T)特記事項
            /// </summary>
            public static readonly ControlDefine T_TOKKI = new ControlDefine( "txtTTokki", "(T)特記事項", "txtTTokki", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// (E)型式コード
            /// </summary>
            public static readonly ControlDefine E_KATASHIKI_CODE = new ControlDefine( "txtEKatashikiCd", "(E)型式コード", "txtEKatashikiCd", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// (E)型式名
            /// </summary>
            public static readonly ControlDefine E_KATASHIKI_NAME = new ControlDefine( "txtEKatashikiNm", "(E)型式名", "txtEKatashikiNm", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 引当先倉庫 絞込
            /// </summary>
            public static readonly ControlDefine CALL_ENGINE_WAREHOUSE = new ControlDefine( "ddlCallEngineWarehouse", "引当先倉庫 絞込", "ddlCallEngineWarehouse", ControlDefine.BindType.Both, typeof( string ) );
        }
        #endregion

        #region グリッドビュー定義
        /// <summary>
        /// グリッドビュー定義
        /// </summary>
        public class GRID_SEARCHCALLENGINESIMULATION {
            /// <summary>
            /// No
            /// </summary>
            public static readonly GridViewDefine DISP_ORDER = new GridViewDefine( "No", "DISP_ORDER", typeof( string ), "", false, HorizontalAlign.Right, 40, true, true );
            /// <summary>
            /// 指示月度連番
            /// </summary>
            public static readonly GridViewDefine T_SHIJI_YM_NUM = new GridViewDefine( "指示月度連番", "指示月度連番", typeof( string ), "", true, HorizontalAlign.Center, 130, true, true );
            /// <summary>
            /// IDNO
            /// </summary>
            public static readonly GridViewDefine T_IDNO = new GridViewDefine( "IDNO", "IDNO", typeof( string ), "", true, HorizontalAlign.Center, 70, true, true );
            /// <summary>
            /// 完成予定日
            /// </summary>
            public static readonly GridViewDefine T_KANSEI_YOTEI_DATE = new GridViewDefine( "完成予定日", "完成予定日", typeof( string ), "{0:" + "MM/dd" + "}", true, HorizontalAlign.Center, 110, true, true );
            /// <summary>
            /// 型式コード
            /// </summary>
            public static readonly GridViewDefine T_KATASHIKI_CODE = new GridViewDefine( "型式コード", "型式コード", typeof( string ), "", true, HorizontalAlign.Center, 110, true, true );
            /// <summary>
            /// 国コード
            /// </summary>
            public static readonly GridViewDefine T_KUNI_CODE = new GridViewDefine( "国コード", "国コード", typeof( string ), "", true, HorizontalAlign.Left, 100, true, true );
            /// <summary>
            /// 型式名
            /// </summary>
            public static readonly GridViewDefine T_KATASHIKI_NAME = new GridViewDefine( "型式名", "型式名", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <summary>
            /// 機番
            /// </summary>
            public static readonly GridViewDefine T_KIBAN = new GridViewDefine( "機番", "機番", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// 特記事項
            /// </summary>
            public static readonly GridViewDefine T_TOKKI = new GridViewDefine( "特記事項", "特記事項", typeof( string ), "", true, HorizontalAlign.Left, 100, true, true );
            /// <summary>
            /// E型式コード
            /// </summary>
            public static readonly GridViewDefine E_KATASHIKI_CODE = new GridViewDefine( "E型式コード", "エンジン型式コード", typeof( string ), "", true, HorizontalAlign.Center, 120, true, true );
            /// <summary>
            /// E型式名
            /// </summary>
            public static readonly GridViewDefine E_KATASHIKI_NAME = new GridViewDefine( "E型式名", "エンジン型式名", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <summary>
            /// E必要数
            /// </summary>
            public static readonly GridViewDefine E_HITSUYO_NUM = new GridViewDefine( "E必要数", "エンジン必要数", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// E在庫数
            /// </summary>
            public static readonly GridViewDefine E_ZAIKO_NUM = new GridViewDefine( "E在庫数", "エンジン在庫数", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// 引当結果
            /// </summary>
            public static readonly GridViewDefine E_HIKIATE_RESULT = new GridViewDefine( "引当結果", "エンジン引当結果", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// 1号機E在庫数
            /// </summary>
            public static readonly GridViewDefine MACHINE_NO_01_STOCK = new GridViewDefine( "1号機E在庫数", "1号機E在庫数", typeof( string ), "", true, HorizontalAlign.Center, 120, true, true );
            /// <summary>
            /// 2号機E在庫数
            /// </summary>
            public static readonly GridViewDefine MACHINE_NO_02_STOCK = new GridViewDefine( "2号機E在庫数", "2号機E在庫数", typeof( string ), "", true, HorizontalAlign.Center, 120, true, true );
            /// <summary>
            /// 引当IDNO
            /// </summary>
            public static readonly GridViewDefine E_IDNO = new GridViewDefine( "引当IDNO", "エンジンIDNO", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// FTS通過日時
            /// </summary>
            public static readonly GridViewDefine MISSHION_JISSEKI_DATE_PREV = new GridViewDefine( "FTS通過日時", "ミッション立体前実績", typeof( string ), "{0:" + "MM/dd HH:mm" + "}", true, HorizontalAlign.Center, 120, true, true );
            /// <summary>
            /// MAR引当日時
            /// </summary>
            public static readonly GridViewDefine MISSHION_JISSEKI_DATE_POST = new GridViewDefine( "MAR引当日時", "ミッション立体後実績", typeof( string ), "{0:" + "MM/dd HH:mm" + "}", true, HorizontalAlign.Center, 120, true, true );
            /// <summary>
            /// APコード
            /// </summary>
            public static readonly GridViewDefine AP_CODE = new GridViewDefine( "APコード", "APコード", typeof( string ), "", true, HorizontalAlign.Center, 120, false, true );

        }

        /// <summary>
        /// グリッドビューグループ
        /// </summary>
        const string GRID_ORDER_SEARCH_CALL_ENGINE_SIMULATION_GROUP_CD = "SearchCallEngineSimulation";
        #endregion
        #endregion

        #region プロパティ
        /// <summary>
        /// ロガー
        /// </summary>
        private static readonly Logger Logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

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
        GridViewDefine[] _gridviewDefault = null;
        /// <summary>
        /// 一覧定義情報アクセサ
        /// </summary>
        GridViewDefine[] GridviewDefault {
            get {
                if ( true == ObjectUtils.IsNull( _gridviewDefault ) ) {
                    _gridviewDefault = ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHCALLENGINESIMULATION ) );
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
        /// 引当先倉庫リスト選択動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCallEngineWarehouse_SelectedIndexChanged( object sender, EventArgs e ) {
            base.RaiseEvent( ChangeCallEngineWarehouse );
        }

        /// <summary>
        /// 検索ボタン押下時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click( object sender, EventArgs e ) {
            // 非表示項目.自動更新にfalseを設定
            hdnAutoSearch.Value = "false";
            base.RaiseEvent( DoSearch );
        }

        /// <summary>
        /// タイマーイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tmrSearch_Tick( object sender, EventArgs e ) {
            base.RaiseEvent( tmrProcess );
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
        protected void gvSearchCallEngineSimulation_Sorting( object sender, GridViewSortEventArgs e ) {
            base.RaiseEvent( Sorting, sender, e );
            // メッセージの復元
            RestoreMsg();
        }

        /// <summary>
        /// グリッドビューページチェンジイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSearchCallEngineSimulation_PageIndexChanging( object sender, CommandEventArgs e ) {
            base.RaiseEvent( PageIndexChanging, sender, e );
            // メッセージの復元
            RestoreMsg();
        }

        /// <summary>
        /// 一覧データバインドイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSearchCallEngineSimulation_RowDataBound( object sender, GridViewRowEventArgs e ) {
            try {
                ClearApplicationMessage();
                RowDataBound( sender, e );
            } catch ( Exception ex ) {
                //イベント処理中にエラー発生
                Logger.Exception( ex );
                throw ex;
            }
        }
        #endregion
        #endregion

        #region ページ処理
        /// <summary>
        /// ページロード処理
        /// </summary>
        protected override void DoPageLoad() {
            // グリッドビューの再表示
            GridView frozenGrid = gvSearchCallEngineSimulationLB;
            ControlUtils.SetGridViewTemplateField( gvSearchCallEngineSimulationLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ) );
            ControlUtils.SetGridViewTemplateField( gvSearchCallEngineSimulationRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ) );
            ControlUtils.ReMakeBoundTemplateFieldCtrl( gvSearchCallEngineSimulationLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ) );
            ControlUtils.ReMakeBoundTemplateFieldCtrl( gvSearchCallEngineSimulationRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ) );
            // ベースページロード処理
            base.DoPageLoad();
            int resultCnt = 0;
            if ( ObjectUtils.IsNotNull( ConditionInfo.ResultData ) ) {
                resultCnt = ConditionInfo.ResultData.Rows.Count;
            }
            ControlUtils.SetGridViewPager( ref pnlPager, gvSearchCallEngineSimulationRB, gvSearchCallEngineSimulation_PageIndexChanging, resultCnt, gvSearchCallEngineSimulationRB.PageIndex );
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
        #region 初期処理
        /// <summary>
        /// 初期処理
        /// </summary>
        private void InitializeValues() {
            // (T)完成予定日 = 1日後の稼働日
#if !DEBUG
            cldTKanseiYoteiYmd.Value = CalendarUtils.GetOffsetWorkday( DateTime.Now, NEXT_WORK_DAY );
#endif

            //引当先倉庫リスト設定
            ControlUtils.SetListControlItems( ddlCallEngineWarehouse, CallEngineWarehouse.GetList() );

            // 初回検索
            DoSearch();
        }
        #endregion

        /// <summary>
        /// 引当先倉庫変更
        /// </summary>
        private void ChangeCallEngineWarehouse() {
        }


        #region 検索処理
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
            var result = new OrderBusiness.ResultSet();
            DataTable tblResult = null;
            // 検索上限数を取得する
            int maxGridViewCount = WebAppInstance.GetInstance().Config.WebCommonInfo.maxGridViewCount;
            try {
                // 処理結果 = 共通処理.検索処理
                result = OrderBusiness.SearchOfSearchCallEngineSimulation( dicCondition, GridviewDefault, maxGridViewCount );
                // タイマー有効化
                tmrSearch.Enabled = true;
            } catch ( DataAccessException ex ) {
                if ( ex.OracleErrorNumber == DataAccessBase.ORAERROR_TIMEOUT ) {
                    // クエリ発行タイムアウトが発生した場合、エラーメッセージを設定する
                    base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_61910 );
                } else {
                    // タイムアウト以外のExceptionが発生した場合、エラーメッセージを設定する
                    Logger.Exception( ex );
                    base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80011, "検索処理で" );
                }
                // タイマー無効化
                tmrSearch.Enabled = false;
            } catch ( Exception ex ) {
                // Exceptionが発生した場合、エラーメッセージを設定する
                Logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_84010 );
                // タイマー無効化
                tmrSearch.Enabled = false;
            } finally {
            }
            ConditionInfoSessionHandler.ST_CONDITION cond = new ConditionInfoSessionHandler.ST_CONDITION();
            tblResult = result.ListTable;
            if ( null != tblResult ) {
                // 検索結果が存在する場合、件数表示、ページャーの設定を行う
                ntbResultCount.Value = tblResult.Rows.Count;
                ControlUtils.SetGridViewPager( ref pnlPager, gvSearchCallEngineSimulationRB, gvSearchCallEngineSimulation_PageIndexChanging, tblResult.Rows.Count, 0 );
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
            GridView frozenGrid = gvSearchCallEngineSimulationLB;
            if ( true == ObjectUtils.IsNotNull( tblResult ) ) {
                if ( 0 < tblResult.Rows.Count ) {
                    // TemplateFieldの追加
                    grvHeaderRT.Columns.Clear();
                    gvSearchCallEngineSimulationRB.Columns.Clear();
                    for ( int idx = frozenGrid.Columns.Count; idx < GridviewDefault.Length; idx++ ) {
                        TemplateField tf = new TemplateField();
                        tf.HeaderText = StringUtils.ToString( GridviewDefault[idx].bindField );
                        gvSearchCallEngineSimulationRB.Columns.Add( tf );
                    }
                    // 新規バインド
                    ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), ConditionInfo, true );
                    ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), ConditionInfo, true );
                    ControlUtils.BindGridView_WithTempField( gvSearchCallEngineSimulationLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), tblResult );
                    ControlUtils.BindGridView_WithTempField( gvSearchCallEngineSimulationRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), tblResult );
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
                // メッセージが存在する場合、メッセージ表示
                base.WriteApplicationMessage( result.Message );

                // メッセージの保存
                Dictionary<string, object> dicMsgInfo = new Dictionary<string, object>();
                dicMsgInfo.Add( MSG_KEY, result.Message );
                CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).SetPageControlInfo( MSG_KEY, dicMsgInfo );
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
        /// タイマー処理
        /// </summary>
        private void tmrProcess() {
            // 非表示項目.自動更新にfalseを設定
            hdnAutoSearch.Value = "false";
            // 現在ページの保存
            string prePageStr = gvSearchCallEngineSimulationRB.PageIndex.ToString();
            // 検索
            DoSearch();

            if ( 0 < ConditionInfo.ResultData.Rows.Count ) {
                // 検索結果がある場合、非表示項目.自動更新にtrueを設定
                hdnAutoSearch.Value = "true";
            }

            // ページ位置を復元
            RePageIndexChanging( prePageStr );
        }
        #endregion

        #region 一覧行データバインド
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
                if ( GetColumnIndex( sender, GRID_SEARCHCALLENGINESIMULATION.DISP_ORDER, out index ) == true ) {
                    //NOの場合、連番を振る
                    var data = e.Row.RowIndex + 1 + sender.PageIndex * sender.PageSize;
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = data.ToString();
                }
                if ( GetColumnIndex( sender, GRID_SEARCHCALLENGINESIMULATION.T_SHIJI_YM_NUM, out index ) == true ) {
                    // 指示月度連番の場合、yyyyMM-nnnnn形式に変換する（変換できない場合は空文字）
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHCALLENGINESIMULATION.T_SHIJI_YM_NUM.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.getDisplayYMNum( data );
                }
                if ( GetColumnIndex( sender, GRID_SEARCHCALLENGINESIMULATION.T_KATASHIKI_CODE, out index ) == true ) {
                    // T型式コードの場合、XXXXX-XXXXX形式に変換する（変換できない場合は空文字）
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHCALLENGINESIMULATION.T_KATASHIKI_CODE.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.getDisplayKatasikiCode( data );
                }
                if ( GetColumnIndex( sender, GRID_SEARCHCALLENGINESIMULATION.E_KATASHIKI_CODE, out index ) == true ) {
                    // E型式コードの場合、XXXXX-XXXXX形式に変換する（変換できない場合は空文字）
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHCALLENGINESIMULATION.E_KATASHIKI_CODE.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.getDisplayKatasikiCode( data );
                }
                if ( GetColumnIndex( sender, GRID_SEARCHCALLENGINESIMULATION.MISSHION_JISSEKI_DATE_PREV, out index ) == true ) {
                    // FTS通過日時の場合、2001/01/01より古いならnullを設定する
                    // 現在日とFTS通過日時の差分が2001/01/01より新しいならMM/dd HH:mmに変換(一覧定義のフォーマットで実現)
                    var missionJissekiDatePrev = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHCALLENGINESIMULATION.MISSHION_JISSEKI_DATE_PREV.bindField].ToString();
                    if ( true == StringUtils.IsNotEmpty( missionJissekiDatePrev ) ) {
                        if ( true == OrderBusiness.checkOlderBaseDate( missionJissekiDatePrev, BASE_DATA_STR ) ) {
                            ( (Label)e.Row.Cells[index].Controls[0] ).Text = null;
                        }
                    }
                }
                if ( GetColumnIndex( sender, GRID_SEARCHCALLENGINESIMULATION.MISSHION_JISSEKI_DATE_POST, out index ) == true ) {
                    // MAR引当日時の場合、2001/01/01より古いならnullを設定する
                    // 現在日とMAR引当日時の差分が2001/01/01より新しいならMM/dd HH:mmに変換(一覧定義のフォーマットで実現)
                    var missionJissekiDatePost = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHCALLENGINESIMULATION.MISSHION_JISSEKI_DATE_POST.bindField].ToString();
                    if ( true == StringUtils.IsNotEmpty( missionJissekiDatePost ) ) {
                        if ( true == OrderBusiness.checkOlderBaseDate( missionJissekiDatePost, BASE_DATA_STR ) ) {
                            ( (Label)e.Row.Cells[index].Controls[0] ).Text = null;
                        }
                    }
                }
                // 引当結果
                // 背景色変更のため、もとのCssClassの設定を保持
                string cssClass = gvSearchCallEngineSimulationLB.RowStyle.CssClass;
                if ( GetColumnIndex( sender, GRID_SEARCHCALLENGINESIMULATION.E_HIKIATE_RESULT, out index ) == true ) {
                    // 引当結果の場合、該当する文字列に変換する（変換できない場合は空文字）
                    var data = NumericUtils.ToInt( ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHCALLENGINESIMULATION.E_HIKIATE_RESULT.bindField].ToString() );
                    if ( data == HIKIATE_RESULT_NOENGINE ) {
                        // 引当結果 = 不可(0)
                        ( (Label)e.Row.Cells[index].Controls[0] ).Text = HIKIATE_RESULT_NOENGINE_DISP;
                        // APコードの取得
                        var apCode = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHCALLENGINESIMULATION.AP_CODE.bindField].ToString();
                        if ( apCode == TOUSAI_ENGINE_07 ) {
                            // #FF9900
                            cssClass = cssClass + TOUSAI_ENGINE_07_COLOR;
                        } else {
                            // #FFC0C0
                            cssClass = cssClass + HIKIATE_RESULT_NOENGINE_COLOR;
                        }
                        e.Row.CssClass = cssClass;
                    } else if ( data == HIKIATE_RESULT_STOCKED ) {
                        // 引当結果 = 可(1)
                        ( (Label)e.Row.Cells[index].Controls[0] ).Text = HIKIATE_RESULT_STOCKED_DISP;
                        // APコードの取得
                        var apCode = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHCALLENGINESIMULATION.AP_CODE.bindField].ToString();
                        if ( apCode == TOUSAI_ENGINE_07 ) {
                            // #C0FFA0
                            cssClass = cssClass + HIKIATE_RESULT_STOCKED_COLOR;
                        } else {
                            // #C0FFA0
                            cssClass = cssClass + HIKIATE_RESULT_STOCKED_COLOR;
                        }
                        e.Row.CssClass = cssClass;
                    } else if ( data == HIKIATE_RESULT_PREPARED ) {
                        // 引当結果 = 予約(2)
                        ( (Label)e.Row.Cells[index].Controls[0] ).Text = HIKIATE_RESULT_PREPARED_DISP;
                        // #C0C0FF
                        cssClass = cssClass + HIKIATE_RESULT_PREPARED_COLOR;
                        e.Row.CssClass = cssClass;
                        // E必要数、E在庫数に"-"を設定する
                        if ( GetColumnIndex( sender, GRID_SEARCHCALLENGINESIMULATION.E_HITSUYO_NUM, out index ) == true ) {
                            ( (Label)e.Row.Cells[index].Controls[0] ).Text = "-";
                        }
                        if ( GetColumnIndex( sender, GRID_SEARCHCALLENGINESIMULATION.E_ZAIKO_NUM, out index ) == true ) {
                            ( (Label)e.Row.Cells[index].Controls[0] ).Text = "-";
                        }
                    } else if ( data == HIKIATE_RESULT_DELIVERED ) {
                        // 引当結果 = 完了(3)
                        ( (Label)e.Row.Cells[index].Controls[0] ).Text = HIKIATE_RESULT_DELIVERED_DISP;
                        // #C0C0C0
                        cssClass = cssClass + HIKIATE_RESULT_DELIVERED_COLOR;
                        e.Row.CssClass = cssClass;
                        // E必要数、E在庫数に"-"を設定する
                        if ( GetColumnIndex( sender, GRID_SEARCHCALLENGINESIMULATION.E_HITSUYO_NUM, out index ) == true ) {
                            ( (Label)e.Row.Cells[index].Controls[0] ).Text = "-";
                        }
                        if ( GetColumnIndex( sender, GRID_SEARCHCALLENGINESIMULATION.E_ZAIKO_NUM, out index ) == true ) {
                            ( (Label)e.Row.Cells[index].Controls[0] ).Text = "-";
                        }
                    } else {
                        // 該当なし
                        ( (Label)e.Row.Cells[index].Controls[0] ).Text = "";
                        // E必要数、E在庫数に"-"を設定する
                        if ( GetColumnIndex( sender, GRID_SEARCHCALLENGINESIMULATION.E_HITSUYO_NUM, out index ) == true ) {
                            ( (Label)e.Row.Cells[index].Controls[0] ).Text = "-";
                        }
                        if ( GetColumnIndex( sender, GRID_SEARCHCALLENGINESIMULATION.E_ZAIKO_NUM, out index ) == true ) {
                            ( (Label)e.Row.Cells[index].Controls[0] ).Text = "-";
                        }
                    }
                    // 固定列側にも同じ背景色を設定する
                    gvSearchCallEngineSimulationLB.Rows[e.Row.RowIndex].CssClass = cssClass;
                }

                // 選択行の背景色変更を追加
                // 製品別通過実績検索画面画面への遷移機能を設定
                string keyKatashikiCode = StringUtils.ToString( ( (DataRowView)e.Row.DataItem )[GRID_SEARCHCALLENGINESIMULATION.E_KATASHIKI_CODE.bindField] );
                ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_ORDER_SEARCH_CALL_ENGINE_SIMULATION_GROUP_CD, ControlUtils.GridRowDoubleClickEvent.WindowOpen, PageInfo.ResolveClientUrl( this, PageInfo.SearchProductOrder ), base.Token, keyKatashikiCode );
            }
        }
        #endregion

        #region Excel出力処理
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
                List<ExcelConditionItem> excelCond = new List<ExcelConditionItem>();
                string condition = "";
                string value = "";

                // (T)完成予定日
                condition = CONDITION.T_KANSEI_YOTEI_YMD.displayNm;
                value = cond.IdWithText[CONDITION.T_KANSEI_YOTEI_YMD.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );
                // (T)IDNO
                condition = CONDITION.T_IDNO.displayNm;
                value = cond.IdWithText[CONDITION.T_IDNO.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );
                // (T)型式コード
                condition = CONDITION.T_KATASHIKI_CODE.displayNm;
                value = cond.IdWithText[CONDITION.T_KATASHIKI_CODE.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );
                // (T)型式名
                condition = CONDITION.T_KATASHIKI_NAME.displayNm;
                value = cond.IdWithText[CONDITION.T_KATASHIKI_NAME.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );
                // (T)特記事項
                condition = CONDITION.T_TOKKI.displayNm;
                value = cond.IdWithText[CONDITION.T_TOKKI.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );
                // (E)型式コード
                condition = CONDITION.E_KATASHIKI_CODE.displayNm;
                value = cond.IdWithText[CONDITION.E_KATASHIKI_CODE.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );
                // (E)型式名
                condition = CONDITION.E_KATASHIKI_NAME.displayNm;
                value = cond.IdWithText[CONDITION.E_KATASHIKI_NAME.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );
                // 引当先倉庫 絞込
                condition = CONDITION.CALL_ENGINE_WAREHOUSE.displayNm;
                value = cond.IdWithText[CONDITION.CALL_ENGINE_WAREHOUSE.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );


                // Excelダウンロード実行
                Excel.Download( Response, "搭載エンジン引当", GetExcelTable( cond.ResultData ), excelCond );
            } catch ( System.Threading.ThreadAbortException ) {
                // response.Endで必ず発生する為、正常として扱う
            } catch ( Exception ex ) {
                // 例外発生時、ログ出力とメッセージ表示
                Logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80030, "搭載エンジン引当_検索結果" );
            }
        }

        /// <summary>
        /// Excel出力用データテーブル作成
        /// </summary>
        /// <param name="tblSource">検索結果</param>
        /// <returns>DataTable</returns>
        private DataTable GetExcelTable( DataTable tblSource ) {
            // 出力対象列(Item1 = 表示用ヘッダ、Item2 = 列名)
            var outputHeaderList = new List<Tuple<string, string>>() {
                new Tuple<string, string>("指示月度連番","指示月度連番"),
                new Tuple<string, string>("IDNO","IDNO"),
                new Tuple<string, string>("完成予定日","完成予定日"),
                new Tuple<string, string>("型式ｺｰﾄﾞ","型式コード"),
                new Tuple<string, string>("国ｺｰﾄﾞ","国コード"),
                new Tuple<string, string>("型式名","型式名"),
                new Tuple<string, string>("機番","機番"),
                new Tuple<string, string>("特記事項","特記事項"),
                new Tuple<string, string>("E型式ｺｰﾄﾞ","エンジン型式コード"),
                new Tuple<string, string>("E型式名","エンジン型式名"),
                new Tuple<string, string>("E必要数","エンジン必要数"),
                new Tuple<string, string>("E在庫数","エンジン在庫数"),
                new Tuple<string, string>("引当結果","エンジン引当結果"),
                new Tuple<string, string>("1号機E在庫数","1号機E在庫数"),
                new Tuple<string, string>("2号機E在庫数","2号機E在庫数"),
            };
            // Excel出力テーブル定義作成(一覧表示対象列を出力対象とする)
            DataTable tblResult = new DataTable();
            // 出力結果テーブルの列を作成する
            // 表示列の定義を取得
            foreach ( var outputHeader in outputHeaderList ) {
                // 出力対象列の列名に一致する表示列の型を取得し、出力結果テーブルの列として作成する
                var newCol = new DataColumn( outputHeader.Item2 );
                // 表示用ヘッダを設定する
                newCol.Caption = outputHeader.Item1;
                tblResult.Columns.Add( newCol );
            }

            // 一覧元DataTableの情報をExcel出力用テーブルにコピー
            foreach ( DataRow rowSrc in tblSource.Rows ) {
                DataRow rowTo = tblResult.NewRow();
                foreach ( var outputHeader in outputHeaderList ) {
                    rowTo[outputHeader.Item2] = rowSrc[outputHeader.Item2];
                }
                if ( outputHeaderList.Any( e => e.Item2 == GRID_SEARCHCALLENGINESIMULATION.T_SHIJI_YM_NUM.bindField ) ) {
                    // 指示月度連番の変換を実行
                    rowTo[GRID_SEARCHCALLENGINESIMULATION.T_SHIJI_YM_NUM.bindField] = OrderBusiness.getDisplayYMNum( rowSrc[GRID_SEARCHCALLENGINESIMULATION.T_SHIJI_YM_NUM.bindField].ToString() );
                }
                if ( outputHeaderList.Any( e => e.Item2 == GRID_SEARCHCALLENGINESIMULATION.T_KATASHIKI_CODE.bindField ) ) {
                    // 型式コードの変換を実行
                    rowTo[GRID_SEARCHCALLENGINESIMULATION.T_KATASHIKI_CODE.bindField] = OrderBusiness.getDisplayKatasikiCode( rowSrc[GRID_SEARCHCALLENGINESIMULATION.T_KATASHIKI_CODE.bindField].ToString() );
                }
                if ( outputHeaderList.Any( e => e.Item2 == GRID_SEARCHCALLENGINESIMULATION.E_KATASHIKI_CODE.bindField ) ) {
                    // エンジン型式コードの変換を実行
                    rowTo[GRID_SEARCHCALLENGINESIMULATION.E_KATASHIKI_CODE.bindField] = OrderBusiness.getDisplayKatasikiCode( rowSrc[GRID_SEARCHCALLENGINESIMULATION.E_KATASHIKI_CODE.bindField].ToString() );
                }
                if ( outputHeaderList.Any( e => e.Item2 == GRID_SEARCHCALLENGINESIMULATION.E_HIKIATE_RESULT.bindField ) ) {
                    // 引当結果の変換を実行
                    switch ( NumericUtils.ToInt( rowSrc[GRID_SEARCHCALLENGINESIMULATION.E_HIKIATE_RESULT.bindField] ) ) {
                    case HIKIATE_RESULT_NOENGINE:
                        rowTo[GRID_SEARCHCALLENGINESIMULATION.E_HIKIATE_RESULT.bindField] = HIKIATE_RESULT_NOENGINE_DISP;
                        break;
                    case HIKIATE_RESULT_STOCKED:
                        rowTo[GRID_SEARCHCALLENGINESIMULATION.E_HIKIATE_RESULT.bindField] = HIKIATE_RESULT_STOCKED_DISP;
                        break;
                    case HIKIATE_RESULT_PREPARED:
                        rowTo[GRID_SEARCHCALLENGINESIMULATION.E_HIKIATE_RESULT.bindField] = HIKIATE_RESULT_PREPARED_DISP;
                        break;
                    case HIKIATE_RESULT_DELIVERED:
                        rowTo[GRID_SEARCHCALLENGINESIMULATION.E_HIKIATE_RESULT.bindField] = HIKIATE_RESULT_DELIVERED_DISP;
                        break;
                    default:
                        rowTo[GRID_SEARCHCALLENGINESIMULATION.E_HIKIATE_RESULT.bindField] = string.Empty;
                        break;
                    }
                }
                if ( outputHeaderList.Any( e => e.Item2 == GRID_SEARCHCALLENGINESIMULATION.MACHINE_NO_01_STOCK.bindField ) ) {
                    // 1号機E在庫数の変換を実行
                    rowTo[GRID_SEARCHCALLENGINESIMULATION.MACHINE_NO_01_STOCK.bindField] = OrderBusiness.getDisplayKatasikiCode( rowSrc[GRID_SEARCHCALLENGINESIMULATION.MACHINE_NO_01_STOCK.bindField].ToString() );
                }
                if ( outputHeaderList.Any( e => e.Item2 == GRID_SEARCHCALLENGINESIMULATION.MACHINE_NO_02_STOCK.bindField ) ) {
                    // 2号機E在庫数の変換を実行
                    rowTo[GRID_SEARCHCALLENGINESIMULATION.MACHINE_NO_02_STOCK.bindField] = OrderBusiness.getDisplayKatasikiCode( rowSrc[GRID_SEARCHCALLENGINESIMULATION.MACHINE_NO_02_STOCK.bindField].ToString() );
                }
                tblResult.Rows.Add( rowTo );
            }
            tblResult.AcceptChanges();

            return tblResult;
        }
        #endregion
        #endregion

        #region グリッドビュー操作
        /// <summary>
        /// グリッドビュークリア
        /// </summary>
        private void ClearGridView() {
            // 列名非表示 グリッドビュークリア
            ControlUtils.InitializeGridView( grvHeaderLT, false );
            ControlUtils.InitializeGridView( grvHeaderRT, false );
            ControlUtils.InitializeGridView( gvSearchCallEngineSimulationLB, false );
            ControlUtils.InitializeGridView( gvSearchCallEngineSimulationRB, false );
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
            int pageSize = gvSearchCallEngineSimulationRB.PageSize;
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
            GridView frozenGrid = gvSearchCallEngineSimulationLB;
            ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), cond, true );
            ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), cond, true );
            ControlUtils.BindGridView_WithTempField( gvSearchCallEngineSimulationLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), ConditionInfo.ResultData );
            ControlUtils.BindGridView_WithTempField( gvSearchCallEngineSimulationRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), ConditionInfo.ResultData );
            ControlUtils.GridViewPageIndexChanging( gvSearchCallEngineSimulationLB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
            ControlUtils.GridViewPageIndexChanging( gvSearchCallEngineSimulationRB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
            ControlUtils.SetGridViewPager( ref pnlPager, gvSearchCallEngineSimulationRB, gvSearchCallEngineSimulation_PageIndexChanging, ConditionInfo.ResultData.Rows.Count, gvSearchCallEngineSimulationRB.PageIndex );
            // グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();
        }

        /// <summary>
        /// 復元専用グリッドビューページ切替
        /// </summary>
        /// <param name="parameters"></param>
        private void RePageIndexChanging( string pageIdx ) {
            int newPageIndex = NumericUtils.ToInt( pageIdx );
            int pageSize = gvSearchCallEngineSimulationRB.PageSize;
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
            GridView frozenGrid = gvSearchCallEngineSimulationLB;
            ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), cond, true );
            ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), cond, true );
            ControlUtils.BindGridView_WithTempField( gvSearchCallEngineSimulationLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), ConditionInfo.ResultData );
            ControlUtils.BindGridView_WithTempField( gvSearchCallEngineSimulationRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), ConditionInfo.ResultData );
            ControlUtils.GridViewPageIndexChanging( gvSearchCallEngineSimulationLB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
            ControlUtils.GridViewPageIndexChanging( gvSearchCallEngineSimulationRB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
            ControlUtils.SetGridViewPager( ref pnlPager, gvSearchCallEngineSimulationRB, gvSearchCallEngineSimulation_PageIndexChanging, ConditionInfo.ResultData.Rows.Count, gvSearchCallEngineSimulationRB.PageIndex );
            // グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();

            // メッセージの復元
            RestoreMsg();
        }

        /// <summary>
        /// グリッドビュー並び替え
        /// </summary>
        /// <param name="parameters"></param>
        private void Sorting( params object[] parameters ) {
            object sender = parameters[0];
            GridViewSortEventArgs e = (GridViewSortEventArgs)parameters[1];
            ConditionInfoSessionHandler.ST_CONDITION cond = ConditionInfo;
            ControlUtils.GridViewSorting( gvSearchCallEngineSimulationLB, ref cond, e, true );
            ControlUtils.GridViewSorting( gvSearchCallEngineSimulationRB, ref cond, e );
            // 背面ユーザ切替対応
            GridView frozenGrid = gvSearchCallEngineSimulationLB;
            ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), cond, true );
            ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), cond, true );
            ControlUtils.BindGridView_WithTempField( gvSearchCallEngineSimulationLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), cond.ResultData );
            ControlUtils.BindGridView_WithTempField( gvSearchCallEngineSimulationRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), cond.ResultData );
            ControlUtils.SetGridViewPager( ref pnlPager, gvSearchCallEngineSimulationRB, gvSearchCallEngineSimulation_PageIndexChanging, cond.ResultData.Rows.Count, gvSearchCallEngineSimulationRB.PageIndex );
            ConditionInfo = cond;
            // グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();
        }

        /// <summary>
        /// 列番号取得
        /// </summary>
        /// <param name="target">確認対象のグリッドビュー</param>
        /// <param name="def">確認する列定義</param>gvSearchCallEngineSimulation
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
            SetDivGridViewWidth( gvSearchCallEngineSimulationLB, divGrvLB );
            SetDivGridViewWidth( gvSearchCallEngineSimulationRB, divGrvRB );
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

        /// <summary>
        /// メッセージの復元
        /// </summary>
        private void RestoreMsg() {
            var msg = CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).GetPageControlInfo( MSG_KEY );
            if ( 0 < msg.Count ) {
                // メッセージが設定されていた場合、メッセージ表示
                base.WriteApplicationMessage( (Msg)msg[MSG_KEY] );
            }
        }
     }
}