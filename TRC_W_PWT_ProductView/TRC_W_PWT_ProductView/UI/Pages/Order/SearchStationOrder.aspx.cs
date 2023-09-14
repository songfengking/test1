////////////////////////////////////////////////////////////////////////////////////////////////
//      クボタ筑波工場  TRC
//      概要：トレーサビリティシステム ステーション別通過実績検索
//---------------------------------------------------------------------------
//           Ver 1.44.0.0  :  2021/05/31  大沼 新規作成(java版からリプレース)
//           Ver 1.44.0.1  :  2021/09/28  大沼 表示件数の変更
//           Ver 1.44.1.1  :  2021/10/25  豊島 RowDataBoundのログ出力抑制
////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    public partial class SearchStationOrder : BaseForm {


        #region 定数
        /// <summary>
        /// 検索条件
        /// </summary>
        public class CONDITION {
            /// <summary>
            /// ステーション
            /// </summary>
            public static readonly ControlDefine STATION = new ControlDefine( "ddlStation", "ステーション", "ddlStation", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 照会日
            /// </summary>
            public static readonly ControlDefine JISSEKI_YMD = new ControlDefine( "cldJissekiYmd", "照会日", "cldJissekiYmd", ControlDefine.BindType.Both, typeof( string ) );
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
            public static readonly ControlDefine KATASHIKI_CODE = new ControlDefine( "txtKatashikiCode", "型式コード", "txtKatashikiCode", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 国コード
            /// </summary>
            public static readonly ControlDefine KUNI_CODE = new ControlDefine( "txtKuniCode", "国コード", "txtKuniCode", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 型式名
            /// </summary>
            public static readonly ControlDefine KATASHIKI_NAME = new ControlDefine( "txtKatashikiName", "型式名", "txtKatashikiName", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 特記事項
            /// </summary>
            public static readonly ControlDefine TOKKI = new ControlDefine( "txtTokki", "特記事項", "txtTokki", ControlDefine.BindType.Both, typeof( string ) );
        }

        /// <summary>
        /// グリッドビュー定義
        /// </summary>
        public class GRID_SEARCHSTATIONORDER {
            /// <summary>
            /// NO
            /// </summary>
            public static readonly GridViewDefine DISP_ORDER = new GridViewDefine( "NO", "DISP_ORDER", typeof( string ), "", false, HorizontalAlign.Right, 40, true, true );
            /// <summary>
            /// 実績月度連番
            /// </summary>
            public static readonly GridViewDefine JISSEKI_YM_NUM = new GridViewDefine( "実績月度連番", "MS_J_TUKINO", typeof( string ), "", true, HorizontalAlign.Center, 130, true, true );
            /// <summary>
            /// 実績時刻
            /// </summary>
            public static readonly GridViewDefine JISSEKI_DATE = new GridViewDefine( "実績時刻", "MS_JITU_YMD", typeof( string ), "{0:" + "HH:mm:ss" + "}", true, HorizontalAlign.Center, 90, true, true );
            /// <summary>
            /// IDNO
            /// </summary>
            public static readonly GridViewDefine IDNO = new GridViewDefine( "IDNO", "MS_IDNO", typeof( string ), "", true, HorizontalAlign.Center, 60, true, true );
            /// <summary>
            /// 型式ｺｰﾄﾞ
            /// </summary>
            public static readonly GridViewDefine KATASHIKI_CODE = new GridViewDefine( "型式ｺｰﾄﾞ", "MS_B_KATA_C", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// 国ｺｰﾄﾞ
            /// </summary>
            public static readonly GridViewDefine KUNI_CODE = new GridViewDefine( "国ｺｰﾄﾞ", "MS_B_KUNI_C", typeof( string ), "", true, HorizontalAlign.Left, 100, true, true );
            /// <summary>
            /// 型式名
            /// </summary>
            public static readonly GridViewDefine KATASHIKI_NAME = new GridViewDefine( "型式名", "MS_B_KATA_N", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <summary>
            /// 機番
            /// </summary>
            public static readonly GridViewDefine KIBAN = new GridViewDefine( "機番", "MS_KIBAN", typeof( string ), "", true, HorizontalAlign.Center, 60, true, true );
            /// <summary>
            /// 指示月度連番
            /// </summary>
            public static readonly GridViewDefine SHIJI_YM_NUM = new GridViewDefine( "指示月度連番", "MS_YYMM_NO", typeof( string ), "", true, HorizontalAlign.Center, 130, true, true );
            /// <summary>
            /// 確定月度連番
            /// </summary>
            public static readonly GridViewDefine KAKUTEI_YM_NUM = new GridViewDefine( "確定月度連番", "確定月度連番", typeof( string ), "", true, HorizontalAlign.Center, 130, true, true );
            /// <summary>
            /// 直通
            /// </summary>
            public static readonly GridViewDefine TYOKKO_SIGN = new GridViewDefine( "直通", "直通", typeof( string ), "", true, HorizontalAlign.Center, 60, true, true );
            /// <summary>
            /// 特記事項
            /// </summary>
            public static readonly GridViewDefine TOKKI = new GridViewDefine( "特記事項", "MS_TOKKIJIKOU", typeof( string ), "", true, HorizontalAlign.Left, 90, true, true );
            /// <summary>
            /// 完成予定日
            /// </summary>
            public static readonly GridViewDefine KANSEI_YOTEI_DATE = new GridViewDefine( "完成予定日", "MS_KAN_YYMMDD", typeof( string ), "{0:" + "MM/dd" + "}", true, HorizontalAlign.Center, 120, true, true );
            /// <summary>
            /// E型式ｺｰﾄﾞ
            /// </summary>
            public static readonly GridViewDefine ENGINE_KATASHIKI_CODE = new GridViewDefine( "E型式ｺｰﾄﾞ", "MS_E_KATA_C", typeof( string ), "", true, HorizontalAlign.Center, 120, true, true );
            /// <summary>
            /// E型式名
            /// </summary>
            public static readonly GridViewDefine ENGINE_KATASHIKI_NAME = new GridViewDefine( "E型式名", "MS_E_KATA_N", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <summary>
            /// EIDNO
            /// </summary>
            public static readonly GridViewDefine ENGINE_IDNO = new GridViewDefine( "EIDNO", "MS_ENG_IDNO", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
            /// <summary>
            /// E機番
            /// </summary>
            public static readonly GridViewDefine ENGINE_KIBAN = new GridViewDefine( "E機番", "MS_ENG_KIBAN", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
            /// <summary>
            /// ﾊﾟﾀﾝ
            /// </summary>
            public static readonly GridViewDefine KUMITATE_PATTERN = new GridViewDefine( "ﾊﾟﾀﾝ", "MS_K_PATAN", typeof( string ), "", true, HorizontalAlign.Center, 70, true, true );
            /// <summary>
            /// LVL
            /// </summary>
            public static readonly GridViewDefine SHIJI_LEVEL = new GridViewDefine( "LVL", "MS_SIJI_LVL", typeof( string ), "", true, HorizontalAlign.Center, 60, true, true );
            /// <summary>
            /// 工数ﾗﾝｸ
            /// </summary>
            public static readonly GridViewDefine ENGINE_KOUSU_RANK = new GridViewDefine( "工数ﾗﾝｸ", "RANK", typeof( string ), "", true, HorizontalAlign.Center, 90, true, true );

        }

        /// <summary>
        /// グリッドビューグループ
        /// </summary>
        const string GRID_ORDER_SEARCH_STATION_GROUP_CD = "SearchStationOrder";
        /// <summary>
        /// 直行
        /// </summary>
        const int TYOKKO = 1;
        /// <summary>
        /// ステーション初期値
        /// </summary>
        const string INITIAL_STATION = "214011";

        /// <summary>メッセージ(Key)</summary>
        const string MSG_KEY = "MSG";
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
                if ( true == ObjectUtils.IsNull( _gridviewDefault ) ) {
                    _gridviewDefault = ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHSTATIONORDER ) );
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

        #region グリッドビュー操作イベント
        /// <summary>
        /// グリッドビュー並び替えイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvSearchStationOrder_Sorting( object sender, GridViewSortEventArgs e ) {
            base.RaiseEvent( Sorting, sender, e );
            RestoreMsg();
        }

        /// <summary>
        /// グリッドビューページチェンジイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvSearchStationOrder_PageIndexChanging( object sender, CommandEventArgs e ) {
            base.RaiseEvent( PageIndexChanging, sender, e );
            RestoreMsg();
        }

        /// <summary>
        /// 一覧データバインドイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvSearchStationOrder_RowDataBound( object sender, GridViewRowEventArgs e ) {
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
            GridView frozenGrid = grvSearchStationOrderLB;
            ControlUtils.SetGridViewTemplateField( grvSearchStationOrderLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ) );
            ControlUtils.SetGridViewTemplateField( grvSearchStationOrderRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ) );
            ControlUtils.ReMakeBoundTemplateFieldCtrl( grvSearchStationOrderLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ) );
            ControlUtils.ReMakeBoundTemplateFieldCtrl( grvSearchStationOrderRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ) );
            int resultCnt = 0;
            if ( ObjectUtils.IsNotNull( ConditionInfo.ResultData ) ) {
                //検索結果がnullでない場合
                resultCnt = ConditionInfo.ResultData.Rows.Count;
            }
            ControlUtils.SetGridViewPager( ref pnlPager, grvSearchStationOrderRB, grvSearchStationOrder_PageIndexChanging, resultCnt, grvSearchStationOrderRB.PageIndex );
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
            try {
                // ステーション一覧情報を検索条件：ステーションのリストに設定する
                ControlUtils.SetListControlItems( ddlStation, MasterList.StationList );
                // ステーション初期値を検索条件：ステーションの初期値に設定する
                ddlStation.SelectedValue = INITIAL_STATION;
                // システム日付を検索条件：照会日の初期値に設定する
                String systemDate = DateUtils.ToString( DateTime.Now, DateUtils.DATE_FORMAT_DAY );
                DateTime? defaultDate = DateUtils.ToDateNullable( systemDate, DateUtils.DATE_FORMAT_DAY );
                cldJissekiYmd.Value = defaultDate;
            } catch ( Exception ex ) {
                // 例外ログ、メッセージ表示を実行する
                Logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_84010 );
            }
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
                // ステーション別通過実績検索画面：検索処理を実行する
                result = OrderBusiness.SearchOfSearchStationOrder( dicCondition, GridviewDefault, maxGridViewCount );
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
            if ( true == ObjectUtils.IsNotNull( tblResult ) ) {
                // 検索結果が存在する場合、件数表示、ページャーの設定を行う
                ntbResultCount.Value = tblResult.Rows.Count;
                ControlUtils.SetGridViewPager( ref pnlPager, grvSearchStationOrderRB, grvSearchStationOrder_PageIndexChanging, tblResult.Rows.Count, 0 );
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
            GridView frozenGrid = grvSearchStationOrderLB;
            if ( true == ObjectUtils.IsNotNull( tblResult ) ) {
                if ( 0 < tblResult.Rows.Count ) {
                    var tyokkoDaisu = JudgeTyokko( tblResult );
                    var tyokkoRatio = tyokkoDaisu * 100 / tblResult.Rows.Count;
                    // TemplateFieldの追加
                    grvHeaderRT.Columns.Clear();
                    grvSearchStationOrderRB.Columns.Clear();
                    for ( int idx = frozenGrid.Columns.Count; idx < GridviewDefault.Length; idx++ ) {
                        TemplateField tf = new TemplateField();
                        tf.HeaderText = StringUtils.ToString( GridviewDefault[idx].bindField );
                        grvSearchStationOrderRB.Columns.Add( tf );
                    }
                    // 新規バインド
                    ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), ConditionInfo, true );
                    ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), ConditionInfo, true );
                    ControlUtils.BindGridView_WithTempField( grvSearchStationOrderLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), tblResult );
                    ControlUtils.BindGridView_WithTempField( grvSearchStationOrderRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), tblResult );
                    // GridView表示
                    divGrvDisplay.Visible = true;
                    // グリッドビュー外のDivサイズ変更
                    SetDivGridViewWidth();
                    //メッセージ設定                   
                    if ( null == result.Message ) {
                        //メッセージがない場合
                        //メッセージ設定
                        result.Message = new Msg( MsgManager.MESSAGE_INF_50020, tblResult.Rows.Count, tyokkoDaisu, tyokkoRatio );
                    }
                } else {
                    //一覧初期化
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
        /// 直行判定
        /// </summary>
        /// <param name="tblResult">処理結果</param>
        /// <returns>直行台数</returns>
        private int JudgeTyokko( DataTable tblResult ) {
            //直行台数
            var tyokkoDaisu = 0;
            for ( int idx = 0; idx < tblResult.Rows.Count; idx++ ) {
                //処理結果をループ
                if ( TYOKKO == NumericUtils.ToInt( tblResult.Rows[idx][GRID_SEARCHSTATIONORDER.TYOKKO_SIGN.bindField].ToString() ) ) {
                    //直行台数が1の場合
                    tyokkoDaisu++;
                }
            }
            return tyokkoDaisu;
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
                if ( true == GetColumnIndex( sender, GRID_SEARCHSTATIONORDER.DISP_ORDER, out index ) ) {
                    //NOの場合、連番を振る
                    var data = e.Row.RowIndex + 1 + sender.PageIndex * sender.PageSize;
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = data.ToString();
                }
                if ( true == GetColumnIndex( sender, GRID_SEARCHSTATIONORDER.SHIJI_YM_NUM, out index ) ) {
                    // 指示月度連番の場合、yyyyMM-nnnnn形式に変換する（変換できない場合は空文字）
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHSTATIONORDER.SHIJI_YM_NUM.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.getDisplayYMNum( data );
                }
                if ( true == GetColumnIndex( sender, GRID_SEARCHSTATIONORDER.KAKUTEI_YM_NUM, out index ) ) {
                    // 確定月度連番の場合、yyyyMM-nnnnn形式に変換する（変換できない場合は空文字）
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHSTATIONORDER.KAKUTEI_YM_NUM.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.getDisplayYMNum( data );
                }
                if ( true == GetColumnIndex( sender, GRID_SEARCHSTATIONORDER.JISSEKI_YM_NUM, out index ) ) {
                    // 実績月度連番の場合、yyyyMM-nnnnn形式に変換する（変換できない場合は空文字）
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHSTATIONORDER.JISSEKI_YM_NUM.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.getDisplayYMNum( data );
                }
                if ( true == GetColumnIndex( sender, GRID_SEARCHSTATIONORDER.KATASHIKI_CODE, out index ) ) {
                    // 型式コードの場合、XXXXX-XXXXX形式に変換する（変換できない場合は空文字）
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHSTATIONORDER.KATASHIKI_CODE.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.getDisplayKatasikiCode( data );
                }
                if ( true == GetColumnIndex( sender, GRID_SEARCHSTATIONORDER.ENGINE_KATASHIKI_CODE, out index ) ) {
                    // E型式コードの場合、XXXXX-XXXXX形式に変換する（変換できない場合は空文字）
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHSTATIONORDER.ENGINE_KATASHIKI_CODE.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.getDisplayKatasikiCode( data );
                }
                // 選択行の背景色変更を追加
                ControlUtils.GridViewRowBound( sender, e, GRID_ORDER_SEARCH_STATION_GROUP_CD, ControlUtils.GridRowDoubleClickEvent.None );
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

                // ステーション
                condition = CONDITION.STATION.displayNm;
                value = cond.IdWithText[CONDITION.STATION.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );

                // 照会日
                condition = CONDITION.JISSEKI_YMD.displayNm;
                value = cond.IdWithText[CONDITION.JISSEKI_YMD.controlId];
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

                // 特記事項
                condition = CONDITION.TOKKI.displayNm;
                value = cond.IdWithText[CONDITION.TOKKI.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );

                // Excelダウンロード実行
                Excel.Download( Response, "ステーション実績リスト", GetExcelTable( cond.ResultData ), excelCond );
            } catch ( System.Threading.ThreadAbortException ) {
                // response.Endで必ず発生する為、正常として扱う
            } catch ( Exception ex ) {
                // 例外発生時、ログ出力とメッセージ表示
                Logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80030, "ステーション実績リスト_検索結果" );
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
                new Tuple<string, string>("実績月度連番","MS_J_TUKINO"),
                new Tuple<string, string>("実績時刻","MS_JITU_YMD"),
                new Tuple<string, string>("IDNO","MS_IDNO"),
                new Tuple<string, string>("型式ｺｰﾄﾞ","MS_B_KATA_C"),
                new Tuple<string, string>("国ｺｰﾄﾞ","MS_B_KUNI_C"),
                new Tuple<string, string>("型式名","MS_B_KATA_N"),
                new Tuple<string, string>("機番","MS_KIBAN"),
                new Tuple<string, string>("指示月度連番","MS_YYMM_NO"),
                new Tuple<string, string>("確定月度連番","確定月度連番"),
                new Tuple<string, string>("直通","直通"),
                new Tuple<string, string>("特記事項","MS_TOKKIJIKOU"),
                new Tuple<string, string>("完成予定日","MS_KAN_YYMMDD"),
                new Tuple<string, string>("E型式ｺｰﾄﾞ","MS_E_KATA_C"),
                new Tuple<string, string>("E型式名","MS_E_KATA_N"),
                new Tuple<string, string>("EIDNO","MS_ENG_IDNO"),
                new Tuple<string, string>("E機番","MS_ENG_KIBAN"),
                new Tuple<string, string>("ﾊﾟﾀﾝ","MS_K_PATAN"),
                new Tuple<string, string>("LVL","MS_SIJI_LVL"),
                new Tuple<string, string>("工数ﾗﾝｸ","RANK"),
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
                if ( outputHeaderList.Any( e => e.Item2 == GRID_SEARCHSTATIONORDER.SHIJI_YM_NUM.bindField ) ) {
                    // 指示月度連番の変換を実行
                    rowTo[GRID_SEARCHSTATIONORDER.SHIJI_YM_NUM.bindField] = OrderBusiness.getDisplayYMNum( rowSrc[GRID_SEARCHSTATIONORDER.SHIJI_YM_NUM.bindField].ToString() );
                }
                if ( outputHeaderList.Any( e => e.Item2 == GRID_SEARCHSTATIONORDER.KAKUTEI_YM_NUM.bindField ) ) {
                    // 確定月度連番の変換を実行
                    rowTo[GRID_SEARCHSTATIONORDER.KAKUTEI_YM_NUM.bindField] = OrderBusiness.getDisplayYMNum( rowSrc[GRID_SEARCHSTATIONORDER.KAKUTEI_YM_NUM.bindField].ToString() );
                }
                if ( outputHeaderList.Any( e => e.Item2 == GRID_SEARCHSTATIONORDER.JISSEKI_YM_NUM.bindField ) ) {
                    // 実績月度連番の変換を実行
                    rowTo[GRID_SEARCHSTATIONORDER.JISSEKI_YM_NUM.bindField] = OrderBusiness.getDisplayYMNum( rowSrc[GRID_SEARCHSTATIONORDER.JISSEKI_YM_NUM.bindField].ToString() );
                }
                if ( outputHeaderList.Any( e => e.Item2 == GRID_SEARCHSTATIONORDER.KATASHIKI_CODE.bindField ) ) {
                    // 型式コードの変換を実行
                    rowTo[GRID_SEARCHSTATIONORDER.KATASHIKI_CODE.bindField] = OrderBusiness.getDisplayKatasikiCode( rowSrc[GRID_SEARCHSTATIONORDER.KATASHIKI_CODE.bindField].ToString() );
                }
                if ( outputHeaderList.Any( e => e.Item2 == GRID_SEARCHSTATIONORDER.ENGINE_KATASHIKI_CODE.bindField ) ) {
                    // エンジン型式コードの変換を実行
                    rowTo[GRID_SEARCHSTATIONORDER.ENGINE_KATASHIKI_CODE.bindField] = OrderBusiness.getDisplayKatasikiCode( rowSrc[GRID_SEARCHSTATIONORDER.ENGINE_KATASHIKI_CODE.bindField].ToString() );
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
            ControlUtils.InitializeGridView( grvSearchStationOrderLB, false );
            ControlUtils.InitializeGridView( grvSearchStationOrderRB, false );
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
            int pageSize = grvSearchStationOrderRB.PageSize;
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
            GridView frozenGrid = grvSearchStationOrderLB;
            ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), cond, true );
            ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), cond, true );
            ControlUtils.BindGridView_WithTempField( grvSearchStationOrderLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), ConditionInfo.ResultData );
            ControlUtils.BindGridView_WithTempField( grvSearchStationOrderRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), ConditionInfo.ResultData );
            ControlUtils.GridViewPageIndexChanging( grvSearchStationOrderLB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
            ControlUtils.GridViewPageIndexChanging( grvSearchStationOrderRB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
            ControlUtils.SetGridViewPager( ref pnlPager, grvSearchStationOrderRB, grvSearchStationOrder_PageIndexChanging, ConditionInfo.ResultData.Rows.Count, grvSearchStationOrderRB.PageIndex );
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
            ControlUtils.GridViewSorting( grvSearchStationOrderLB, ref cond, e, true );
            ControlUtils.GridViewSorting( grvSearchStationOrderRB, ref cond, e );
            // 背面ユーザ切替対応
            GridView frozenGrid = grvSearchStationOrderLB;
            ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), cond, true );
            ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), cond, true );
            ControlUtils.BindGridView_WithTempField( grvSearchStationOrderLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), cond.ResultData );
            ControlUtils.BindGridView_WithTempField( grvSearchStationOrderRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), cond.ResultData );
            ControlUtils.SetGridViewPager( ref pnlPager, grvSearchStationOrderRB, grvSearchStationOrder_PageIndexChanging, cond.ResultData.Rows.Count, grvSearchStationOrderRB.PageIndex );
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
            SetDivGridViewWidth( grvSearchStationOrderLB, divGrvLB );
            SetDivGridViewWidth( grvSearchStationOrderRB, divGrvRB );
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
    }
}