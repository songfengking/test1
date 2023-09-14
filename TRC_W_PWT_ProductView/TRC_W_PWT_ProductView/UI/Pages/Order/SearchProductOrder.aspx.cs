////////////////////////////////////////////////////////////////////////////////////////////////
//      クボタ筑波工場  TRC
//      概要：トレーサビリティシステム 製品別通過実績検索
//---------------------------------------------------------------------------
//           Ver 1.44.0.0  :  2021/05/31  豊島  新規作成(java版からリプレース)
//           Ver 1.44.0.1  :  2021/09/14  豊島  結合テスト不具合対応(検索後のラジオボタン切り替え)
//           Ver 1.44.0.2  :  2021/09/28  豊島  表示件数の変更
//           Ver 1.44.1.1  :  2021/10/25  豊島　RowDataBoundのログ出力抑制
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
using KTFramework.Dao;
using TRC_W_PWT_ProductView.Business;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Dao.Com;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.Session;
using TRC_W_PWT_ProductView.UI.Base;

namespace TRC_W_PWT_ProductView.UI.Pages.Order {
    /// <summary>
    /// 製品別通過実績検索画面
    /// </summary>
    public partial class SearchProductOrder : BaseForm {
        #region 定数

        #region 固定値、文字列
        /// <summary>メッセージ(Key)</summary>
        const string MSG_KEY = "MSG";
        /// <summary>ステーション</summary>
        public const string STATION_NM = "ステーション";
        /// <summary>工程順序</summary>
        public const string PROCESS_SEQ = "工程順序";
        /// <summary>実績連番</summary>
        public const string JISSEKI_SEQ = "実績連番";
        /// <summary>実績日時</summary>
        public const string JISSEKI_DT = "実績日時";
        /// <summary>spanタグ(末尾)</summary>
        private const string END_SPAN = "</span>";
        /// <summary>ハイフン</summary>
        private const string HYPHEN = "-";
        /// <summary>スペース(HTML)</summary>
        private const string SPACE_HTML = "&nbsp;";
        /// <summary>スペース(Char型)</summary>
        private const Char SPACE_CHAR = ' ';
        #endregion

        #region 製品種別
        /// <summary>指定しない</summary>
        public const string SHIJI_LEVEL_ALL = "指定しない";
        /// <summary>トラクタ</summary>
        public const string TRACTOR = "トラクタ";
        /// <summary>03エンジン</summary>
        public const string ENGINE_03 = "03エンジン";
        /// <summary>07エンジン</summary>
        public const string ENGINE_07 = "07エンジン";
        /// <summary>トラクタ(値)</summary>
        public const string TRACTOR_VAL = "1";
        /// <summary>03エンジン(値)</summary>
        public const string ENGINE_03_VAL = "3";
        /// <summary>07エンジン(値)</summary>
        public const string ENGINE_07_VAL = "7";
        #endregion

        #region エンジン種別
        /// <summary>全て</summary>
        public const string PATTERN_FLG_ALL = "全て";
        /// <summary>搭載</summary>
        public const string PATTERN_FLG_TOUSAI = "搭載";
        /// <summary>OEM</summary>
        public const string PATTERN_FLG_OEM = "OEM";
        /// <summary>搭載(値)</summary>
        public const string PATTERN_FLG_TOUSAI_VAL = "2";
        /// <summary>OEM(値)</summary>
        public const string PATTERN_FLG_OEM_VAL = "3";
        #endregion

        #region 組立パターン
        /// <summary>組立パターン07搭載</summary>
        private const string K_PATTERN_07_TOUSAI = "14";
        /// <summary>組立パターン03搭載</summary>
        private const string K_PATTERN_03_TOUSAI = "15";
        /// <summary>組立パターン07OEM</summary>
        private const string K_PATTERN_07_OEM = "18";
        /// <summary>組立パターン03OEM</summary>
        private const string K_PATTERN_03_OEM = "19";
        /// <summary>表示用組立パターン07搭載</summary>
        private const string K_PATTERN_07_TOUSAI_DISP = "07搭載";
        /// <summary>表示用組立パターン03搭載</summary>
        private const string K_PATTERN_03_TOUSAI_DISP = "03搭載";
        /// <summary>表示用組立パターン07OEM</summary>
        private const string K_PATTERN_07_OEM_DISP = "07OEM";
        /// <summary>表示用組立パターン03OEM</summary>
        private const string K_PATTERN_03_OEM_DISP = "03OEM";
        #endregion

        #region CSS指定
        /// <summary>青</summary>
        private const string COLOR_BLUE = "<span class=blue>";
        /// <summary>灰</summary>
        private const string COLOR_GRAY = "<span class=gray>";
        #endregion

        #region 検索条件
        /// <summary>
        /// 検索条件
        /// </summary>
        public class CONDITION {
            /// <summary>
            /// 製品種別
            /// </summary>
            public static readonly ControlDefine SHIJI_LEVEL = new ControlDefine( "rblShijiLevel", "製品種別", "rblShijiLevel", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// エンジン種別
            /// </summary>
            public static readonly ControlDefine PATTERN_FLAG = new ControlDefine( "rblPatternFlag", "エンジン種別", "rblPatternFlag", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// IDNO
            /// </summary>
            public static readonly ControlDefine IDNO = new ControlDefine( "txtIdno", "IDNO", "txtIdno", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 機番
            /// </summary>
            public static readonly ControlDefine KIBAN = new ControlDefine( "txtKiban", "機番", "txtKiban", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 型式コード
            /// </summary>
            public static readonly ControlDefine KATASHIKI_CODE = new ControlDefine( "txtKatashikiCd", "型式コード", "txtKatashikiCd", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 国コード
            /// </summary>
            public static readonly ControlDefine KUNI_CODE = new ControlDefine( "txtKuniCd", "国コード", "txtKuniCd", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 型式名
            /// </summary>
            public static readonly ControlDefine KATASHIKI_NAME = new ControlDefine( "txtKatashikiNm", "型式名", "txtKatashikiNm", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 投入順序連番
            /// </summary>
            public static readonly ControlDefine TONYU_YM_NUM = new ControlDefine( "txtTonyuYmNum", "投入順序連番", "txtTonyuYmNum", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 確定順序連番
            /// </summary>
            public static readonly ControlDefine KAKUTEI_YM_NUM = new ControlDefine( "txtKakuteiYmNum", "確定順序連番", "txtKakuteiYmNum", ControlDefine.BindType.Both, typeof( string ) );
        }
        #endregion

        #region グリッドビュー定義
        /// <summary>
        /// グリッドビュー定義
        /// </summary>
        public class GRID_SEARCHPRODUCTORDER {
            /// <summary>
            /// No
            /// </summary>
            public static readonly GridViewDefine DISP_ORDER = new GridViewDefine( "No", "DISP_ORDER", typeof( string ), "", false, HorizontalAlign.Right, 40, true, true );
            /// <summary>
            /// IDNO
            /// </summary>
            public static readonly GridViewDefine IDNO = new GridViewDefine( "IDNO", "IDNO", typeof( string ), "", true, HorizontalAlign.Center, 70, true, true );
            /// <summary>
            /// 型式コード
            /// </summary>
            public static readonly GridViewDefine KATASHIKI_CODE = new GridViewDefine( "型式コード", "型式コード", typeof( string ), "", true, HorizontalAlign.Center, 110, true, true );
            /// <summary>
            /// 国コード
            /// </summary>
            public static readonly GridViewDefine KUNI_CODE = new GridViewDefine( "国コード", "国コード", typeof( string ), "", true, HorizontalAlign.Left, 100, true, true );
            /// <summary>
            /// 型式名
            /// </summary>
            public static readonly GridViewDefine KATASHIKI_NAME = new GridViewDefine( "型式名", "型式名", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <summary>
            /// 機番
            /// </summary>
            public static readonly GridViewDefine KIBAN = new GridViewDefine( "機番", "機番", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// 種別
            /// </summary>
            public static readonly GridViewDefine KUMITATE_PATTERN_NAME = new GridViewDefine( "種別", "組立パターン", typeof( string ), "", true, HorizontalAlign.Center, 60, true, true );
            /// <summary>
            /// 投入順序
            /// </summary>
            public static readonly GridViewDefine TONYU_YM_NUM = new GridViewDefine( "投入順序", "指示月度連番", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// 確定順序
            /// </summary>
            public static readonly GridViewDefine KAKUTEI_YM_NUM = new GridViewDefine( "確定順序", "確定月度連番", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// 出荷
            /// </summary>
            public static readonly GridViewDefine SYUKKA_DATE = new GridViewDefine( "出荷", "出荷年月日", typeof( string ), "", true, HorizontalAlign.Center, 70, true, true );
            /// <summary>
            /// 搭載型式
            /// </summary>
            public static readonly GridViewDefine ENGINE_KATASHIKI_CODE = new GridViewDefine( "搭載型式", "エンジン型式コード", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// 搭載IDNO
            /// </summary>
            public static readonly GridViewDefine ENGINE_IDNO = new GridViewDefine( "搭載IDNO", "エンジンIDNO", typeof( string ), "", true, HorizontalAlign.Center, 90, true, true );
            /// <summary>
            /// 搭載機番
            /// </summary>
            public static readonly GridViewDefine ENGINE_KIBAN = new GridViewDefine( "搭載機番", "エンジン機番", typeof( string ), "", true, HorizontalAlign.Center, 110, true, true );
        }

        /// <summary>
        /// グリッドビューグループ
        /// </summary>
        const string GRID_ORDER_SEARCH_PRODUCT_ORDER_GROUP_CD = "SearchProductOrder";
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
        private BaseForm CurrentForm
        {
            get
            {
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
        ControlDefine[] ConditionControls
        {
            get
            {
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
        GridViewDefine[] GridviewDefault
        {
            get
            {
                if ( true == ObjectUtils.IsNotNull( _gridviewDefault ) ) {
                    // _gridviewDefaultがあれば_gridviewDefaultを返す
                    return _gridviewDefault;
                }
                if ( true == ObjectUtils.IsNotNull( base.ConditionInfo.ResultDefine ) ) {
                    // 検索条件セッション情報があれば検索条件セッション情報を返す
                    _gridviewDefault = base.ConditionInfo.ResultDefine;
                } else {
                    // 一覧定義から_gridviewDefaultを作成する
                    _gridviewDefault = ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHPRODUCTORDER ) );
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
        /// 検索ボタン押下時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click( object sender, EventArgs e ) {
            base.RaiseEvent( DoSearch );
        }

        /// <summary>
        /// Excel出力ボタン押下時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcel_Click( object sender, EventArgs e ) {
            base.RaiseEvent( OutputExcel );
        }

        /// <summary>
        /// 製品種別リスト選択動作
        /// </summary>
        protected void rblShijiLevel_SelectedIndexChanged( object sender, EventArgs e ) {
            base.RaiseEvent( ChangeShijiLevel );
        }

        #region グリッドビュー操作イベント
        /// <summary>
        /// グリッドビュー並び替えイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSearchProductOrder_Sorting( object sender, GridViewSortEventArgs e ) {
            base.RaiseEvent( Sorting, sender, e );
            // メッセージの復元
            RestoreMsg();
        }

        /// <summary>
        /// グリッドビューページチェンジイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSearchProductOrder_PageIndexChanging( object sender, CommandEventArgs e ) {
            base.RaiseEvent( PageIndexChanging, sender, e );
            // メッセージの復元
            RestoreMsg();
        }

        /// <summary>
        /// 一覧データバインドイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSearchProductOrder_RowDataBound( object sender, GridViewRowEventArgs e ) {
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
            // ベースページロード処理
            base.DoPageLoad();
            // グリッドビューの再表示
            GridView frozenGrid = gvSearchProductOrderLB;
            ControlUtils.SetGridViewTemplateField( gvSearchProductOrderLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ) );
            ControlUtils.SetGridViewTemplateField( gvSearchProductOrderRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ) );
            ControlUtils.ReMakeBoundTemplateFieldCtrl( gvSearchProductOrderLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ) );
            ControlUtils.ReMakeBoundTemplateFieldCtrl( gvSearchProductOrderRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ) );
            int resultCnt = 0;
            if ( ObjectUtils.IsNotNull( ConditionInfo.ResultData ) ) {
                resultCnt = ConditionInfo.ResultData.Rows.Count;
            }
            ControlUtils.SetGridViewPager( ref pnlPager, gvSearchProductOrderRB, gvSearchProductOrder_PageIndexChanging, resultCnt, gvSearchProductOrderRB.PageIndex );

            if ( false == IsPostBack ) {
                // 初回表示時のみ検索を行う
                if ( true == ObjectUtils.IsNotNull( base.GetTransInfoValue( RequestParameter.Common.CALLER ) ) ) {
                    // 呼び元ページIDが存在する場合

                    // 比較対象がstringのため、string型に変換して呼び元ページIDを取得
                    string pageIdStr = StringUtils.ToString( base.GetTransInfoValue( RequestParameter.Common.CALLER ) );

                    if ( false == ( pageIdStr == PageInfo.MaintenanceMenu.pageId ) ) {
                        // メンテナンスメニュー画面以外から遷移した場合
                        // 型式コードとして扱う
                        this.txtKatashikiCd.Value = pageIdStr;
                        // 検索
                        DoSearch();
                    }
                }
            }
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
        #endregion

        #region 機能別処理

        #region 初期処理
        /// <summary>
        /// 初期処理
        /// </summary>
        private void InitializeValues() {
            //製品種別.指定しないにチェック
            this.rblShijiLevel.SelectedValue = SHIJI_LEVEL_ALL;
            //エンジン種別.全てにチェック
            this.rblPatternFlag.SelectedValue = PATTERN_FLG_ALL;
            //オプションボタン制御
            ChangeShijiLevel();
        }
        #endregion

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
                result = OrderBusiness.SearchOfSearchProductOrder( ref dicCondition, maxGridViewCount );
                // ラジオボタンの切り替え
                switchRb( dicCondition );
            } catch ( DataAccessException ex ) {
                if ( ex.OracleErrorNumber == DataAccessBase.ORAERROR_TIMEOUT ) {
                    // クエリ発行タイムアウトが発生した場合、エラーメッセージを設定する
                    base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_61910 );
                } else {
                    // タイムアウト以外のExceptionが発生した場合、エラーメッセージを設定する
                    Logger.Exception( ex );
                    base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80011, "検索処理で" );
                }
            } catch ( Exception ex ) {
                // Exceptionが発生した場合、エラーメッセージを設定する
                Logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_84010 );
            } finally {
            }
            ConditionInfoSessionHandler.ST_CONDITION cond = new ConditionInfoSessionHandler.ST_CONDITION();
            tblResult = result.ListTable;
            if ( null != tblResult ) {

                // _gridviewDefaultの再作成
                RecreateGridviewDefault( tblResult, dicCondition );

                // 列定義から列名のキャプションを設定する
                _gridviewDefault.ToList().ForEach( cd => {
                    tblResult.Columns[cd.bindField].Caption = cd.headerText;
                } );

                // 検索結果が存在する場合、件数表示、ページャーの設定を行う
                ntbResultCount.Value = tblResult.Rows.Count;
                ControlUtils.SetGridViewPager( ref pnlPager, gvSearchProductOrderRB, gvSearchProductOrder_PageIndexChanging, tblResult.Rows.Count, 0 );
                // 検索条件/結果インスタンスを保持する
                cond.conditionValue = dicCondition;
                cond.IdWithText = dicIdWithText;
                cond.ResultData = tblResult.DefaultView.ToTable();
                cond.ResultDefine = _gridviewDefault;
            } else {
                // タイムアウト等Exception時には、GridViewクリア
                ClearGridView();
            }
            // 検索条件をセッションに格納する
            ConditionInfo = cond;
            // グリッドビューの表示処理を行う
            GridView frozenGrid = gvSearchProductOrderLB;
            if ( true == ObjectUtils.IsNotNull( tblResult ) ) {
                if ( 0 < tblResult.Rows.Count ) {
                    // TemplateFieldの追加
                    grvHeaderRT.Columns.Clear();
                    gvSearchProductOrderRB.Columns.Clear();
                    for ( int idx = frozenGrid.Columns.Count; idx < GridviewDefault.Length; idx++ ) {
                        TemplateField tf = new TemplateField();
                        tf.HeaderText = StringUtils.ToString( GridviewDefault[idx].bindField );
                        gvSearchProductOrderRB.Columns.Add( tf );
                    }
                    // 新規バインド
                    ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), ConditionInfo, true );
                    ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), ConditionInfo, true );
                    ControlUtils.BindGridView_WithTempField( gvSearchProductOrderLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), tblResult );
                    ControlUtils.BindGridView_WithTempField( gvSearchProductOrderRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), tblResult );
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
        /// 一覧定義情報再作成
        /// </summary>
        private void RecreateGridviewDefault( DataTable tblResult, Dictionary<string, object> dicCondition ) {
            // 検索が成功したら_gridviewDefaultを作り直す
            _gridviewDefault = ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHPRODUCTORDER ) );

            // グリッドビューを一覧定義情報に戻す
            GridView gridView = gvSearchProductOrderLB;
            ControlUtils.SetGridViewTemplateField( gvSearchProductOrderLB, ControlUtils.GetFrozenColumns( gridView, ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHPRODUCTORDER ) ), true ) );
            ControlUtils.SetGridViewTemplateField( gvSearchProductOrderRB, ControlUtils.GetFrozenColumns( gridView, ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHPRODUCTORDER ) ), false ) );

            // 列定義にある列名を保持
            var gridViewheader = _gridviewDefault.Select( d => d.bindField );

            // 表示用テーブルにある列名を取得
            List<string> colHeader = new List<string>();
            foreach ( DataColumn c in tblResult.Columns ) {
                colHeader.Add( c.ColumnName );
            }

            // 表示用テーブルの列名と列定義の列名の差を取り、出荷のまえに埋め込む
            var diff = colHeader.Except( gridViewheader ).Select( h => new GridViewDefine( h, h, typeof( string ), "", false, HorizontalAlign.Center, 180, true, true ) );
            var temp = _gridviewDefault.ToList();
            int LBidx = gvSearchProductOrderLB.Columns.Count;
            int RBidx;
            if ( GetColumnIndex( gvSearchProductOrderRB, GRID_SEARCHPRODUCTORDER.SYUKKA_DATE, out RBidx ) ) {
                temp.InsertRange( LBidx + RBidx, diff );
            }

            // 特定列の非表示処理
            for ( int idx = 0; idx < temp.Count; idx++ ) {
                if ( temp[idx].headerText == STATION_NM ) {
                    // ステーション
                    temp[idx].visible = false;
                }
                if ( temp[idx].headerText == PROCESS_SEQ ) {
                    // 工程順序
                    temp[idx].visible = false;
                }
                if ( temp[idx].headerText == JISSEKI_SEQ ) {
                    // 実績連番
                    temp[idx].visible = false;
                }
                if ( temp[idx].headerText == JISSEKI_DT ) {
                    // 実績日時
                    temp[idx].visible = false;
                }

                if ( DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.SHIJI_LEVEL.bindField ) == TRACTOR_VAL ) {
                    // 検索条件.製品種別でトラクタを選択
                    if ( temp[idx].headerText == GRID_SEARCHPRODUCTORDER.KUMITATE_PATTERN_NAME.headerText ) {
                        // 種別を非表示
                        temp[idx].visible = false;
                    }
                    if ( temp[idx].headerText == GRID_SEARCHPRODUCTORDER.KAKUTEI_YM_NUM.headerText ) {
                        // 確定順序を表示
                        temp[idx].visible = true;
                    }
                    if ( temp[idx].headerText == GRID_SEARCHPRODUCTORDER.ENGINE_KATASHIKI_CODE.headerText ) {
                        // 搭載型式を表示
                        temp[idx].visible = true;
                    }
                    if ( temp[idx].headerText == GRID_SEARCHPRODUCTORDER.ENGINE_IDNO.headerText ) {
                        // 搭載IDNOを表示
                        temp[idx].visible = true;
                    }
                    if ( temp[idx].headerText == GRID_SEARCHPRODUCTORDER.ENGINE_KIBAN.headerText ) {
                        // 搭載機番を表示
                        temp[idx].visible = true;
                    }
                } else {
                    // 検索条件.製品種別でトラクタ以外を選択
                    if ( temp[idx].headerText == GRID_SEARCHPRODUCTORDER.KUMITATE_PATTERN_NAME.headerText ) {
                        // 種別を表示
                        temp[idx].visible = true;
                    }
                    if ( temp[idx].headerText == GRID_SEARCHPRODUCTORDER.KAKUTEI_YM_NUM.headerText ) {
                        // 確定順序を非表示
                        temp[idx].visible = false;
                    }
                    if ( temp[idx].headerText == GRID_SEARCHPRODUCTORDER.ENGINE_KATASHIKI_CODE.headerText ) {
                        // 搭載型式を非表示
                        temp[idx].visible = false;
                    }
                    if ( temp[idx].headerText == GRID_SEARCHPRODUCTORDER.ENGINE_IDNO.headerText ) {
                        // 搭載IDNOを非表示
                        temp[idx].visible = false;
                    }
                    if ( temp[idx].headerText == GRID_SEARCHPRODUCTORDER.ENGINE_KIBAN.headerText ) {
                        // 搭載機番を非表示
                        temp[idx].visible = false;
                    }
                }
            }

            // 定義しなおしたものを_gridviewDefaultに設定
            _gridviewDefault = temp.ToArray();
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
                if ( GetColumnIndex( sender, GRID_SEARCHPRODUCTORDER.DISP_ORDER, out index ) == true ) {
                    //NOの場合、連番を振る
                    var data = e.Row.RowIndex + 1 + sender.PageIndex * sender.PageSize;
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = data.ToString();
                }
                if ( GetColumnIndex( sender, GRID_SEARCHPRODUCTORDER.KATASHIKI_CODE, out index ) == true ) {
                    // 型式コードの場合、XXXXX-XXXXX形式に変換する（変換できない場合は空文字）
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHPRODUCTORDER.KATASHIKI_CODE.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.getDisplayKatasikiCode( data );
                }
                if ( GetColumnIndex( sender, GRID_SEARCHPRODUCTORDER.ENGINE_KATASHIKI_CODE, out index ) == true ) {
                    // エンジン型式コードの場合、XXXXX-XXXXX形式に変換する（変換できない場合は空文字）
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHPRODUCTORDER.ENGINE_KATASHIKI_CODE.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.getDisplayKatasikiCode( data );
                }
                if ( GetColumnIndex( sender, GRID_SEARCHPRODUCTORDER.KUMITATE_PATTERN_NAME, out index ) == true ) {
                    // 種別の場合、該当する文字列に変換する (該当なしの場合は"")
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHPRODUCTORDER.KUMITATE_PATTERN_NAME.bindField].ToString();
                    if ( data == K_PATTERN_07_TOUSAI ) {
                        // 種別 == 組立パターン07搭載( 14 )
                        ( (Label)e.Row.Cells[index].Controls[0] ).Text = K_PATTERN_07_TOUSAI_DISP;
                    } else if ( data == K_PATTERN_03_TOUSAI ) {
                        // 種別 == 組立パターン03搭載(15)
                        ( (Label)e.Row.Cells[index].Controls[0] ).Text = K_PATTERN_03_TOUSAI_DISP;
                    } else if ( data == K_PATTERN_07_OEM ) {
                        // 種別 == 組立パターン07OEM( 18 )
                        ( (Label)e.Row.Cells[index].Controls[0] ).Text = K_PATTERN_07_OEM_DISP;
                    } else if ( data == K_PATTERN_03_OEM ) {
                        // 種別 == 組立パターン03OEM(19)
                        ( (Label)e.Row.Cells[index].Controls[0] ).Text = K_PATTERN_03_OEM_DISP;
                    } else {
                        // 該当なし
                        ( (Label)e.Row.Cells[index].Controls[0] ).Text = "";
                    }
                }
                if ( GetColumnIndex( sender, GRID_SEARCHPRODUCTORDER.TONYU_YM_NUM, out index ) == true ) {
                    // 投入順序の場合、yyyyMM-nnnnn形式に変換する（変換できない場合は空文字）
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHPRODUCTORDER.TONYU_YM_NUM.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.getDisplayYMNum( data );
                }
                if ( GetColumnIndex( sender, GRID_SEARCHPRODUCTORDER.KAKUTEI_YM_NUM, out index ) == true ) {
                    // 確定順序の場合、yyyyMM-nnnnn形式に変換する（変換できない場合は空文字）
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHPRODUCTORDER.KAKUTEI_YM_NUM.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.getDisplayYMNum( data );
                }
                if ( GetColumnIndex( sender, GRID_SEARCHPRODUCTORDER.SYUKKA_DATE, out index ) == true ) {
                    // 出荷の場合、MM/dd形式に変換する(空の場合は"-")
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHPRODUCTORDER.SYUKKA_DATE.bindField].ToString();
                    data = OrderBusiness.convertDatetimeToMD( data, HYPHEN );
                    if ( data == HYPHEN ) {
                        // ハイフンの場合、スタイル変更なし
                        ( (Label)e.Row.Cells[index].Controls[0] ).Text = data;
                    } else {
                        // ハイフンでない場合、スタイル変更(文字色青)
                        ( (Label)e.Row.Cells[index].Controls[0] ).Text = COLOR_BLUE + data + END_SPAN;
                        // ツールチップの再設定
                        e.Row.Cells[index].ToolTip = data;
                    }
                }
                // 追加したステーションを判別する
                var diff = base.ConditionInfo.ResultDefine.Except( ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHPRODUCTORDER ) ) );
                for ( int idx = 0; idx < diff.Count(); idx++ ) {
                    if ( GetColumnIndex( sender, diff.ElementAt( idx ), out index ) == true ) {
                        // 追加したステーションを、MM/dd HH:mm:ss形式に変換する(空の場合は"-")
                        var data = ( (DataRowView)e.Row.DataItem ).Row[diff.ElementAt( idx ).bindField].ToString();
                        data = OrderBusiness.convertDatetimeToMDH( data, HYPHEN );
                        if ( data == HYPHEN ) {
                            // ハイフンの場合、スタイル変更なし
                            ( (Label)e.Row.Cells[index].Controls[0] ).Text = data;
                        } else {
                            // ハイフンでない場合、スタイル変更(文字色青(MM/dd)、灰(HH:mm:ss))
                            // 半角スペースで分割
                            var dataSplit = data.Split( SPACE_CHAR );
                            ( (Label)e.Row.Cells[index].Controls[0] ).Text = COLOR_BLUE + dataSplit[0] + END_SPAN + SPACE_HTML + COLOR_GRAY + dataSplit[1] + END_SPAN;
                            // ツールチップの再設定
                            e.Row.Cells[index].ToolTip = data;
                        }
                    }
                }
                // 選択行の背景色変更を追加
                ControlUtils.GridViewRowBound( sender, e, GRID_ORDER_SEARCH_PRODUCT_ORDER_GROUP_CD, ControlUtils.GridRowDoubleClickEvent.None );

                // ツールチップの再設定
                if ( GetColumnIndex( sender, GRID_SEARCHPRODUCTORDER.SYUKKA_DATE, out index ) == true ) {
                    // 出荷
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHPRODUCTORDER.SYUKKA_DATE.bindField].ToString();
                    data = OrderBusiness.convertDatetimeToMD( data, HYPHEN );
                    e.Row.Cells[index].ToolTip = data;
                }
                for ( int idx = 0; idx < diff.Count(); idx++ ) {
                    if ( GetColumnIndex( sender, diff.ElementAt( idx ), out index ) == true ) {
                        // 追加したステーション
                        var data = ( (DataRowView)e.Row.DataItem ).Row[diff.ElementAt( idx ).bindField].ToString();
                        data = OrderBusiness.convertDatetimeToMDH( data, HYPHEN );
                        e.Row.Cells[index].ToolTip = data;
                    }
                }
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

                // 製品種別
                condition = CONDITION.SHIJI_LEVEL.displayNm;
                value = cond.IdWithText[CONDITION.SHIJI_LEVEL.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );

                // エンジン種別
                condition = CONDITION.PATTERN_FLAG.displayNm;
                value = cond.IdWithText[CONDITION.PATTERN_FLAG.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );

                // IDNO
                condition = CONDITION.IDNO.displayNm;
                value = cond.IdWithText[CONDITION.IDNO.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );

                // 機番
                condition = CONDITION.KIBAN.displayNm;
                value = cond.IdWithText[CONDITION.KIBAN.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );

                // 型式コード
                condition = CONDITION.KATASHIKI_CODE.displayNm;
                value = cond.IdWithText[CONDITION.KATASHIKI_CODE.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );

                // 国コード
                condition = CONDITION.KUNI_CODE.displayNm;
                value = cond.IdWithText[CONDITION.KUNI_CODE.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );

                // 型式名
                condition = CONDITION.KATASHIKI_NAME.displayNm;
                value = cond.IdWithText[CONDITION.KATASHIKI_NAME.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );

                // 投入順序連番
                condition = CONDITION.TONYU_YM_NUM.displayNm;
                value = cond.IdWithText[CONDITION.TONYU_YM_NUM.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );

                // 確定順序連番
                condition = CONDITION.KAKUTEI_YM_NUM.displayNm;
                value = cond.IdWithText[CONDITION.KAKUTEI_YM_NUM.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );

                // Excelダウンロード実行
                Excel.Download( Response, "製品別通過実績", GetExcelTable( cond.ResultData ), excelCond );
            } catch ( System.Threading.ThreadAbortException ) {
                // response.Endで必ず発生する為、正常として扱う
            } catch ( Exception ex ) {
                // 例外発生時、ログ出力とメッセージ表示
                Logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80030, "製品別通過実績_検索結果" );
            }
        }

        /// <summary>
        /// Excel出力用データテーブル作成
        /// </summary>
        /// <param name="tblSource">検索結果</param>
        /// <returns>DataTable</returns>
        private DataTable GetExcelTable( DataTable tblSource ) {
            // 指示レベルを取得
            var sijiLevel = DataUtils.GetDictionaryStringVal( base.ConditionInfo.conditionValue, SearchProductOrder.CONDITION.SHIJI_LEVEL.bindField );
            // 出力対象列(Item1 = 表示用ヘッダ、Item2 = 列名)
            List<Tuple<string, string>> outputHeaderList;
            if ( sijiLevel == OrderBusiness.SHIJI_LEVEL_TRACTOR ) {
                outputHeaderList = new List<Tuple<string, string>>() {
                    new Tuple<string, string>("IDNO","IDNO"),
                    new Tuple<string, string>("型式ｺｰﾄﾞ","型式コード"),
                    new Tuple<string, string>("国ｺｰﾄﾞ","国コード"),
                    new Tuple<string, string>("型式名","型式名"),
                    new Tuple<string, string>("機番","機番"),
                    new Tuple<string, string>("投入順序","指示月度連番"),
                    new Tuple<string, string>("確定順序","確定月度連番"),
                    new Tuple<string, string>("搭載型式","エンジン型式コード"),
                    new Tuple<string, string>("搭載IDNO","エンジンIDNO"),
                    new Tuple<string, string>("搭載機番","エンジン機番"),
                    new Tuple<string, string>("出荷日","出荷年月日"),
                };
            } else {
                outputHeaderList = new List<Tuple<string, string>>() {
                    new Tuple<string, string>("IDNO","IDNO"),
                    new Tuple<string, string>("型式ｺｰﾄﾞ","型式コード"),
                    new Tuple<string, string>("国ｺｰﾄﾞ","国コード"),
                    new Tuple<string, string>("型式名","型式名"),
                    new Tuple<string, string>("機番","機番"),
                    new Tuple<string, string>("種別","組立パターン"),
                    new Tuple<string, string>("投入順序","指示月度連番"),
                    new Tuple<string, string>("出荷日","出荷年月日"),
                };
            }
            // 固定で画面表示する列
            var baseDefine = typeof( GRID_SEARCHPRODUCTORDER ).GetFields().Select( fi => ( (GridViewDefine)fi.GetValue( null ) ).bindField );
            foreach ( var gdElem in GridviewDefault ) {
                // 表示中の列定義を参照する
                if ( ( baseDefine.Contains( gdElem.bindField ) == false ) && ( gdElem.visible == true ) ) {
                    // 表示中の列のうち、固定で画面表示する列以外かつ表示している列の場合
                    // 可変で追加した列として追加する
                    outputHeaderList.Add( new Tuple<string, string>( gdElem.headerText, gdElem.bindField ) );
                }
            }
            // 出力結果テーブル
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
                if ( outputHeaderList.Any( e => e.Item2 == GRID_SEARCHPRODUCTORDER.KATASHIKI_CODE.bindField ) ) {
                    // 型式コードの変換を実行
                    rowTo[GRID_SEARCHPRODUCTORDER.KATASHIKI_CODE.bindField] = OrderBusiness.getDisplayKatasikiCode( rowSrc[GRID_SEARCHPRODUCTORDER.KATASHIKI_CODE.bindField].ToString() );
                }
                if ( outputHeaderList.Any( e => e.Item2 == GRID_SEARCHPRODUCTORDER.KUMITATE_PATTERN_NAME.bindField ) ) {
                    // 種別の変換を実行
                    switch ( rowSrc[GRID_SEARCHPRODUCTORDER.KUMITATE_PATTERN_NAME.bindField].ToString() ) {
                    case K_PATTERN_07_TOUSAI:
                        rowTo[GRID_SEARCHPRODUCTORDER.KUMITATE_PATTERN_NAME.bindField] = K_PATTERN_07_TOUSAI_DISP;
                        break;
                    case K_PATTERN_03_TOUSAI:
                        rowTo[GRID_SEARCHPRODUCTORDER.KUMITATE_PATTERN_NAME.bindField] = K_PATTERN_03_TOUSAI_DISP;
                        break;
                    case K_PATTERN_07_OEM:
                        rowTo[GRID_SEARCHPRODUCTORDER.KUMITATE_PATTERN_NAME.bindField] = K_PATTERN_07_OEM_DISP;
                        break;
                    case K_PATTERN_03_OEM:
                        rowTo[GRID_SEARCHPRODUCTORDER.KUMITATE_PATTERN_NAME.bindField] = K_PATTERN_03_OEM_DISP;
                        break;
                    default:
                        rowTo[GRID_SEARCHPRODUCTORDER.KUMITATE_PATTERN_NAME.bindField] = string.Empty;
                        break;
                    }
                }
                if ( outputHeaderList.Any( e => e.Item2 == GRID_SEARCHPRODUCTORDER.TONYU_YM_NUM.bindField ) ) {
                    // 投入順序の変換を実行
                    rowTo[GRID_SEARCHPRODUCTORDER.TONYU_YM_NUM.bindField] = OrderBusiness.getDisplayYMNum( rowSrc[GRID_SEARCHPRODUCTORDER.TONYU_YM_NUM.bindField].ToString() );
                }
                if ( outputHeaderList.Any( e => e.Item2 == GRID_SEARCHPRODUCTORDER.KAKUTEI_YM_NUM.bindField ) ) {
                    // 確定順序の変換を実行
                    rowTo[GRID_SEARCHPRODUCTORDER.KAKUTEI_YM_NUM.bindField] = OrderBusiness.getDisplayYMNum( rowSrc[GRID_SEARCHPRODUCTORDER.KAKUTEI_YM_NUM.bindField].ToString() );
                }
                tblResult.Rows.Add( rowTo );
            }
            tblResult.AcceptChanges();
            return tblResult;
        }
        #endregion

        #region 画面操作
        /// <summary>
        /// オプションボタン制御
        /// </summary>
        private void ChangeShijiLevel() {
            if ( rblShijiLevel.SelectedValue == SHIJI_LEVEL_ALL ) {
                //製品種別 = 指定しない
                rblPatternFlag.Enabled = false;
                txtKakuteiYmNum.Value = "";
                txtKakuteiYmNum.Enabled = true;
                rblPatternFlag.SelectedValue = PATTERN_FLG_ALL;
            } else if ( rblShijiLevel.SelectedValue == TRACTOR ) {
                //製品種別 = トラクタ
                rblPatternFlag.Enabled = false;
                txtKakuteiYmNum.Enabled = true;
                rblPatternFlag.SelectedValue = PATTERN_FLG_ALL;
            } else if ( rblShijiLevel.SelectedValue == ENGINE_03 ) {
                //製品種別 = 03エンジン
                rblPatternFlag.Enabled = true;
                txtKakuteiYmNum.Value = "";
                txtKakuteiYmNum.Enabled = false;
            } else if ( rblShijiLevel.SelectedValue == ENGINE_07 ) {
                //製品種別 = 07エンジン
                rblPatternFlag.Enabled = true;
                txtKakuteiYmNum.Value = "";
                txtKakuteiYmNum.Enabled = false;
            }
        }

        /// <summary>
        /// ラジオボタンの切り替え
        /// </summary>
        private void switchRb( Dictionary<string, object> dicCondition ) {
            // 製品種別への反映
            string shijiLevel = dicCondition[CONDITION.SHIJI_LEVEL.bindField].ToString();
            if ( shijiLevel == TRACTOR_VAL ) {
                // shijiLevel = 1(トラクタ)
                this.rblShijiLevel.SelectedValue = TRACTOR;
            } else if ( shijiLevel == ENGINE_03_VAL ) {
                // shijiLevel = 3(03エンジン)
                this.rblShijiLevel.SelectedValue = ENGINE_03;
            } else if ( shijiLevel == ENGINE_07_VAL ) {
                // shijiLevel = 7(07エンジン)
                this.rblShijiLevel.SelectedValue = ENGINE_07;
            } else {
                // shijiLevel = それ以外(指定しない)
                this.rblShijiLevel.SelectedValue = SHIJI_LEVEL_ALL;
            }

            // エンジン種別への反映
            string patternFlag = dicCondition[CONDITION.PATTERN_FLAG.bindField].ToString();
            if ( patternFlag == PATTERN_FLG_TOUSAI_VAL ) {
                // patternFlag = 2(搭載)
                this.rblPatternFlag.SelectedValue = PATTERN_FLG_TOUSAI;
            } else if ( patternFlag == PATTERN_FLG_OEM_VAL ) {
                // patternFlag = 3(OEM)
                this.rblPatternFlag.SelectedValue = PATTERN_FLG_OEM;
            } else {
                // patternFlag = それ以外(全て)
                this.rblPatternFlag.SelectedValue = PATTERN_FLG_ALL;
            }

            //オプションボタン制御
            ChangeShijiLevel();
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
            ControlUtils.InitializeGridView( gvSearchProductOrderLB, false );
            ControlUtils.InitializeGridView( gvSearchProductOrderRB, false );
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
            int pageSize = gvSearchProductOrderRB.PageSize;
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
            GridView frozenGrid = gvSearchProductOrderLB;
            ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), cond, true );
            ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), cond, true );
            ControlUtils.BindGridView_WithTempField( gvSearchProductOrderLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), ConditionInfo.ResultData );
            ControlUtils.BindGridView_WithTempField( gvSearchProductOrderRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), ConditionInfo.ResultData );
            ControlUtils.GridViewPageIndexChanging( gvSearchProductOrderLB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
            ControlUtils.GridViewPageIndexChanging( gvSearchProductOrderRB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
            ControlUtils.SetGridViewPager( ref pnlPager, gvSearchProductOrderRB, gvSearchProductOrder_PageIndexChanging, ConditionInfo.ResultData.Rows.Count, gvSearchProductOrderRB.PageIndex );
            // グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();
        }

        /// <summary>
        /// グリッドビュー並び替え
        /// </summary>
        /// <param name="parameters"></param>
        private void Sorting( params object[] parameters ) {
            object sender = parameters[0];
            GridViewSortEventArgs e = (GridViewSortEventArgs)parameters[1];
            ConditionInfoSessionHandler.ST_CONDITION cond = ConditionInfo;
            ControlUtils.GridViewSorting( gvSearchProductOrderLB, ref cond, e, true );
            ControlUtils.GridViewSorting( gvSearchProductOrderRB, ref cond, e );
            // 背面ユーザ切替対応
            GridView frozenGrid = gvSearchProductOrderLB;
            ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), cond, true );
            ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), cond, true );
            ControlUtils.BindGridView_WithTempField( gvSearchProductOrderLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), cond.ResultData );
            ControlUtils.BindGridView_WithTempField( gvSearchProductOrderRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), cond.ResultData );
            ControlUtils.SetGridViewPager( ref pnlPager, gvSearchProductOrderRB, gvSearchProductOrder_PageIndexChanging, cond.ResultData.Rows.Count, gvSearchProductOrderRB.PageIndex );
            ConditionInfo = cond;
            // グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();
        }

        /// <summary>
        /// 列番号取得
        /// </summary>
        /// <param name="target">確認対象のグリッドビュー</param>
        /// <param name="def">確認する列定義</param>
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
            SetDivGridViewWidth( gvSearchProductOrderLB, divGrvLB );
            SetDivGridViewWidth( gvSearchProductOrderRB, divGrvRB );
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
    }
}