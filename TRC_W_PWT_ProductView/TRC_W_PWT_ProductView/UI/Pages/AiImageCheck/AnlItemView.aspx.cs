
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
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
using TRC_W_PWT_ProductView.UI.Pages.UserControl;

namespace TRC_W_PWT_ProductView.UI.Pages.AiImageCheck {
    public partial class AnlItemView : BaseForm {

        #region 定数

        #region 固定値、文字列
        /// <summary>
        /// 空き(表示用)
        /// </summary>
        private const string FLAG_DISP = "";
        /// <summary>
        /// 利用ツールプルダウンタイプ(検索用)
        /// </summary>
        private const string PULLDOWNTYPE_USINGTOOL = "3";
        /// <summary>
        /// オブジェクトダブルクリック
        /// </summary>
        private const string ON_DBLCLICK = "ondblclick";
        /// <summary>
        /// クエリ条件リスト
        /// </summary>
        private static Dictionary<string, object> dicCondition ;
        /// <summary>
        /// 直行
        /// </summary>
        const int TYOKKO = 1;
        /// <summary>
        /// <summary>メッセージ(Key)</summary>
        /// </summary>
        const string MSG_KEY = "MSG";

        #endregion

        #region 検索条件

        /// <summary>
        /// 検索条件
        /// </summary>
        public class CONDITION {
            /// <summary>
            /// ライン
            /// </summary>
            public static readonly ControlDefine LINE_CD = new ControlDefine( "ddlLineCd", "ライン", "ddlLineCd", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// ステーション
            /// </summary>
            public static readonly ControlDefine STATION = new ControlDefine( "ddlStation", "ステーション", "ddlStation", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 画像解析項目コード
            /// </summary>
            public static readonly ControlDefine ITEM_CD = new ControlDefine( "txtItemCd", "画像解析項目コード", "txtItemCd", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 画像解析項目名
            /// </summary>
            public static readonly ControlDefine ITEM_NM = new ControlDefine( "txtItemNm", "画像解析項目名", "txtItemNm", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 利用ツール
            /// </summary>
            public static readonly ControlDefine USING_TOOL = new ControlDefine( "ddlUsingTool", "利用ツール", "ddlUsingTool", ControlDefine.BindType.Both, typeof( string ) );
        }

        #endregion

        #endregion

        #region グリッドビュー定義

        /// <summary>
        /// グリッドビュー定義
        /// </summary>
        public class GRID_SEARCHDETAIL {
            /// <summary>
            /// ライン
            /// </summary>
            public static readonly GridViewDefine LINE_NM = new GridViewDefine( " ライン ", "LineNm", typeof( string ), "", true, HorizontalAlign.Left, 130, true, true );
            /// <summary>
            /// ステーション
            /// </summary>
            public static readonly GridViewDefine STATION_NM = new GridViewDefine( "ステーション", "StationNm", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
            /// <summary>
            /// 画像解析項目コード
            /// </summary>
            public static readonly GridViewDefine ITEM_CD = new GridViewDefine( "画像解析項目コード", "ItemCd", typeof( string ), "", true, HorizontalAlign.Center, 180, true, true );
            /// <summary>
            /// 画像解析項目名
            /// </summary>
            public static readonly GridViewDefine ITEM_NM = new GridViewDefine( "画像解析項目名", "ItemNm", typeof( string ), "", true, HorizontalAlign.Left, 200, true, true );
            /// <summary>
            /// 改訂番号
            /// </summary>
            public static readonly GridViewDefine REVISION_NO = new GridViewDefine( "改訂番号", "RevisionNo", typeof( string ), "", true, HorizontalAlign.Center,100, true, true );
            /// <summary>
            /// 利用ツール
            /// </summary>
            public static readonly GridViewDefine USING_TOOL = new GridViewDefine( "利用ツール", "UsingTool", typeof( string ), "", true, HorizontalAlign.Left, 100, true, true );
            /// <summary>
            /// ストリーム
            /// </summary>
            public static readonly GridViewDefine STREAM = new GridViewDefine( "ストリーム", "Stream", typeof( string ), "", true, HorizontalAlign.Left, 100, true, true );
            /// <summary>
            /// 利用モデル
            /// </summary>
            public static readonly GridViewDefine USING_MODEL = new GridViewDefine( "利用モデル", "UsingModel", typeof( string ), "", true, HorizontalAlign.Left, 100, true, true );
            /// <summary>
            /// 期待結果
            /// </summary>
            public static readonly GridViewDefine EXPECT_RESULT = new GridViewDefine( "期待結果", "ExpectResult", typeof( string ), "", true, HorizontalAlign.Left, 100, true, true );
            /// <summary>
            /// ラインコード
            /// </summary>
            public static readonly GridViewDefine LINE_CD = new GridViewDefine( "ラインコード", "LineCode", typeof( string ), "", true, HorizontalAlign.Left, 0, false, true );
            /// <summary>
            /// ステーションコード
            /// </summary>
            public static readonly GridViewDefine STATION_CD = new GridViewDefine( "ステーションコード", "StationCode", typeof( string ), "", true, HorizontalAlign.Left, 0, false, true );
            /// <summary>
            /// 利用ツールコード
            /// </summary>
            public static readonly GridViewDefine USING_TOOL_CD = new GridViewDefine( "利用ツールコード", "UsingToolCd", typeof( string ), "", true, HorizontalAlign.Left, 0, false, true );

        }

        /// <summary>
        /// グリッドビューグループ
        /// </summary>
        const string GRID_CORRECTIVE_SEARCH_VIEW_GROUP_CD = "AnlItemView";

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
                    _gridviewDefault = ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHDETAIL ) );
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
            // 検索条件を作成する
            base.GetControlValues( ConditionControls, ref dicCondition );
            base.RaiseEvent( DoSearch );
        }

        /// <summary>
        /// 入力画面の閉じるボタン押下時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnViewRefresh_Click( object sender, EventArgs e ) {
            base.RaiseEvent( DoSearch );
        }

        /// <summary>
        /// グリッドビューページチェンジイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvDetailBody_PageIndexChanging( object sender, CommandEventArgs e ) {
            base.RaiseEvent( PageIndexChanging, sender, e );
            RestoreMsg();
        }

        /// <summary>
        /// 一覧データバインドイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMainViewLB_RowDataBound( object sender, GridViewRowEventArgs e ) {
            if ( e.Row.RowType == DataControlRowType.DataRow ) {

                DataRow row = ( (DataRowView)e.Row.DataItem ).Row;
               
            }

            RowDataBoundMainViewLB( sender, e );
        }

        /// <summary>
        /// 一覧データバインドイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMainViewRB_RowDataBound( object sender, GridViewRowEventArgs e ) {

            RowDataBoundMainViewRB( sender, e );
        }

        #endregion

        #region ページ処理

        /// <summary>
        /// ページロード処理
        /// </summary>
        protected override void DoPageLoad() {
            // ベースページロード処理
            base.DoPageLoad();
            // グリッドビューの再表示
           GridView frozenGrid = grvMainViewLB;
			ControlUtils.SetGridViewTemplateField( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ) );
			ControlUtils.SetGridViewTemplateField( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ) );

			ControlUtils.ReMakeBoundTemplateFieldCtrl( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ) );
			ControlUtils.ReMakeBoundTemplateFieldCtrl( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ) );
            int resultCnt = 0;
            if ( ObjectUtils.IsNotNull( ConditionInfo.ResultData ) ) {
                //検索結果がnullでない場合
                resultCnt = ConditionInfo.ResultData.Rows.Count;
            }
            ControlUtils.SetGridViewPager( ref pnlPager, grvMainViewRB, grvDetailBody_PageIndexChanging, resultCnt, grvMainViewRB.PageIndex );
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

                // 利用ツールリストの初期値に設定する
                ListItem[] ServiceUsingToolList = ControlUtils.GetListItems( AiImageCheckDao.SelectUsingToolList( PULLDOWNTYPE_USINGTOOL ) );
                if ( 0 < ServiceUsingToolList.Length ) {
                    for ( int decLoop = 0; decLoop < ServiceUsingToolList.Length; decLoop++ ) {                
                        ServiceUsingToolList[decLoop].Text = ServiceUsingToolList[decLoop].Text.Split( '|' )[0];
                    }
                    //検索結果がnullでない場合
                    ddlUsingTool.Items.Clear();
                    ddlUsingTool.Items.Add( FLAG_DISP );
                    ddlUsingTool.Items.AddRange( ServiceUsingToolList );
                }

                // ステーション一覧情報を検索条件：ステーションのリストに設定する
                ddlStation.Items.Add( FLAG_DISP );
                ddlStation.Items.AddRange( MasterList.ServiceStationList );

                // ライン一覧情報を検索条件：ラインのリストに設定する
                ddlLineCd.Items.Add( FLAG_DISP );
                ddlLineCd.Items.AddRange( MasterList.ServiceLineList );

                //新規追加ボタン非活性化
                this.btnModalDisp.Enabled = false;

            } catch ( Exception ex ) {
                // 例外ログ、メッセージ表示を実行する
                Logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
            }
        }

        /// <summary>
        /// ステーションリスト選択動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlLineList_SelectedIndexChanged( object sender, EventArgs e ) {
            if ( true == StringUtils.IsNotEmpty( ddlStation.SelectedValue ) ) {
                base.RaiseEvent( ChangeLineListByStation );
            }
        }

        /// <summary>
        /// ステーション変更
        /// </summary>
        private void ChangeLineListByStation( ) {
            ListItem[] ServiceLineList = ControlUtils.GetListItems( AiImageCheckDao.SelectLineListByStation( ddlStation.SelectedValue ), AiImageCheckDao.COLNAME_TEXT, AiImageCheckDao.COLNAME_VALUE );

            if ( 0 < ServiceLineList.Length ) {
                //検索結果がnullでない場合
                ddlLineCd.SelectedValue = ServiceLineList[0].Value;
            }
        }

        /// <summary>
        /// ラインリスト選択動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlStationList_SelectedIndexChanged( object sender, EventArgs e ) {
            if ( true == StringUtils.IsNotEmpty( ddlLineCd.SelectedValue ) ) {
                base.RaiseEvent( ChangeStationListByLine );
            }
        }

        /// <summary>
        /// ライン変更
        /// </summary>
        private void ChangeStationListByLine( ) {
            ListItem[] ServiceLineList = ControlUtils.GetListItems( AiImageCheckDao.SelectStationListByLine( ddlLineCd.SelectedValue ), AiImageCheckDao.COLNAME_TEXT, AiImageCheckDao.COLNAME_VALUE );

            if ( 0 < ServiceLineList.Length ) {
                //検索結果がnullでない場合
                ddlStation.Items.Clear();
                ddlStation.Items.Add(FLAG_DISP);
                ddlStation.Items.AddRange( ServiceLineList );
            }
        }

        #endregion

        #region 検索処理

        /// <summary>
        /// 検索処理
        /// </summary>
        private void DoSearch() {

            // 必要な検索条件を確認する
            if ( true == StringUtils.IsEmpty( DataUtils.GetDictionaryStringVal( dicCondition, AnlItemView.CONDITION.LINE_CD.bindField ) ) ) {
                // メッセージが存在する場合、メッセージ表示
                base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62020, "ライン" );
                ClearGridView();
                btnModalDisp.Enabled = false;
                return;
            } else if ( true == StringUtils.IsEmpty( DataUtils.GetDictionaryStringVal( dicCondition, AnlItemView.CONDITION.STATION.bindField ) ) ) {
                // メッセージが存在する場合、メッセージ表示
                base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62020, "ステーション" );
                ClearGridView();
                btnModalDisp.Enabled = false;
                return;
            };

            // 検索時画面情報を取得する
            Dictionary<string, string> dicIdWithText = new Dictionary<string, string>();
            base.GetControlTexts( ConditionControls, out dicIdWithText );

            // 検索結果取得
            // エラー時にも最後まで処理を通すため、インスタンスを生成しておく
            var result = new AiImageCheckBusiness.ImgCheckResultSet();
            DataTable tblResult = null;
            // 検索上限数を取得する
            int maxGridViewCount = WebAppInstance.GetInstance().Config.WebCommonInfo.maxGridViewCount;
            try {
                // 検索処理を実行する
                result = AiImageCheckBusiness.SearchOfAnlItemResult( dicCondition, GridviewDefault, maxGridViewCount );
                
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

            //---検索結果判定---
            // 処理結果が取得できた時のみ件数・ページャーの設定を実施
            ConditionInfoSessionHandler.ST_CONDITION cond = new ConditionInfoSessionHandler.ST_CONDITION();
            tblResult = result.ListTable;
            if ( true == ObjectUtils.IsNotNull( tblResult ) ) {
                // 検索結果が存在する場合、件数表示、ページャーの設定を行う
                ntbResultCount.Value = tblResult.Rows.Count;
                // ピッキング明細件数に応じてページャー設定
                ControlUtils.SetGridViewPager( ref pnlPager, grvMainViewRB, grvDetailBody_PageIndexChanging, tblResult.Rows.Count, 0 );
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
            GridView frozenGrid = grvMainViewLB;
            if ( true == ObjectUtils.IsNotNull( tblResult ) ) {
                if ( 0 < tblResult.Rows.Count ) {

                    //新規バインド
                    ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), ConditionInfo, true );
                    ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), ConditionInfo, true );
                    ControlUtils.BindGridView_WithTempField( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), tblResult );
                    ControlUtils.BindGridView_WithTempField( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), tblResult );

                    //ページャー作成
                    ControlUtils.SetGridViewPager( ref pnlPager, grvMainViewLB, grvDetailBody_PageIndexChanging, cond.ResultData.Rows.Count, grvMainViewLB.PageIndex );

                    //グリッドビュー表示列情報修正
                   // SetGridViewColumns();

                    //GridView表示
                    divGrvDisplay.Visible = true;

                    //グリッドビュー外のDivサイズ変更
                    SetDivGridViewWidth();
                } else {
                    ClearGridView();
                }
            }

            //権限によるボタン制御
            AppPermission.PERMISSION_INFO permMainteInfo = AppPermission.GetTransactionPermission( PageInfo.AnlItemInput, LoginInfo.UserInfo );
            if ( permMainteInfo.IsEdit == true ) {
                //更新権限所持
                btnModalDisp.Enabled = true;
                //イベント
                ListItem[] liArr = new ListItem[3];
                liArr[0] = new ListItem( "exeKbn", "1" );
                liArr[1] = new ListItem( "ddlLineCd", DataUtils.GetDictionaryStringVal( dicCondition, "ddlLineCd" ) );
                liArr[2] = new ListItem( "ddlStation", DataUtils.GetDictionaryStringVal( dicCondition, "ddlStation" ) );
                btnModalDisp.Attributes[ControlUtils.ON_CLICK] = AiImageCheck.ImageInputModal.ImageCreateDispUrl( this, PageInfo.AnlItemInput, 10, 10, liArr, "検査項目新規登録", "input" );
                ;
               
            } else {
                btnModalDisp.Enabled = false;
            }

            //メッセージ表示
            if ( null != result.Message ) {
                // メッセージが存在する場合、メッセージ表示
                base.WriteApplicationMessage( result.Message );
            }
        }

        #endregion

        #region 一覧行データバインド

        /// <summary>
		/// グリッドビュー行バインド(左下)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RowDataBoundMainViewLB( params object[] parameters ) {
            object sender = parameters[0];
            GridViewRowEventArgs e = (GridViewRowEventArgs)parameters[1];

            if ( e.Row.RowType == DataControlRowType.DataRow ) {

                // データ行のみバインド処理を行う
                int index = 0;

                // 選択行の背景色変更を追加
                ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_CORRECTIVE_SEARCH_VIEW_GROUP_CD, ControlUtils.GridRowDoubleClickEvent.None );

                /// <summary>
                /// 権限による制御
                /// </summary>
                AppPermission.PERMISSION_INFO permAuthInfo = AppPermission.GetTransactionPermission( TRC_W_PWT_ProductView.Defines.PageInfo.AnlItemInput, LoginInfo.UserInfo );

                //権限取得
                if ( permAuthInfo.IsEdit == true ) {
                    //ダブルクリックイベント
                    Dictionary<string, object> dicCondition = new Dictionary<string, object>();
                    base.GetControlValues( ConditionControls, ref dicCondition );
                    var ddlLineCd = DataUtils.GetDictionaryStringVal( dicCondition, "ddlLineCd" );  //ラインコード
                    var ddlStation = DataUtils.GetDictionaryStringVal( dicCondition, "ddlStation" );  //ステーション
                    var txtItemCode = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHDETAIL.ITEM_CD.bindField].ToString();      //検査項目コード
                    var txtItemNm = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHDETAIL.ITEM_NM.bindField].ToString();      //検査項目名称
                    var txtRevisionNo = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHDETAIL.REVISION_NO.bindField].ToString();      //改訂番号
                    var txtLineNm = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHDETAIL.LINE_NM.bindField].ToString();      //ライン名称
                    var txtStationNm = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHDETAIL.STATION_NM.bindField].ToString();      //ステーション名
                    var txtUsingToolCd = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHDETAIL.USING_TOOL_CD.bindField].ToString();      //利用ツール
                    var txtStream = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHDETAIL.STREAM.bindField].ToString();      //ストリーム
                    var txtUsingModel = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHDETAIL.USING_MODEL.bindField].ToString();      //利用モデル
                    var txtExpectResult = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHDETAIL.EXPECT_RESULT.bindField].ToString();      //期待結果


                    //keyの作成
                    ListItem[] liArr = new ListItem[10];
                    liArr[0] = new ListItem( "exeKbn", "2" );
                    liArr[1] = new ListItem( "ddlLineCd", ddlLineCd );
                    liArr[2] = new ListItem( "ddlStation", ddlStation );
                    liArr[3] = new ListItem( "txtItemCode", txtItemCode );
                    liArr[4] = new ListItem( "txtRevisionNo", txtRevisionNo );
                    liArr[5] = new ListItem( "txtItemNm", txtItemNm );
                    liArr[6] = new ListItem( "txtUsingToolCd", txtUsingToolCd );
                    liArr[7] = new ListItem( "txtStream", txtStream );
                    liArr[8] = new ListItem( "txtUsingModel", txtUsingModel );
                    liArr[9] = new ListItem( "txtExpectResult", txtExpectResult );

                    //ダブルクリック用
                    e.Row.Attributes[ON_DBLCLICK] = AiImageCheck.ImageInputModal.ImageCreateDispUrl( this, PageInfo.AnlItemInput, 10, 10, liArr, "検査項目更新", "input" );
                }

            }
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

                // データ行のみバインド処理を行う
                int index = 0;

                // 選択行の背景色変更を追加
                ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_CORRECTIVE_SEARCH_VIEW_GROUP_CD, ControlUtils.GridRowDoubleClickEvent.None );

                /// <summary>
                /// 権限による制御
                /// </summary>
                AppPermission.PERMISSION_INFO permAuthInfo = AppPermission.GetTransactionPermission( TRC_W_PWT_ProductView.Defines.PageInfo.AnlItemInput, LoginInfo.UserInfo );

                //権限取得
                if ( permAuthInfo.IsEdit == true ) {
                    //ダブルクリックイベント
                    Dictionary<string, object> dicCondition = new Dictionary<string, object>();
                    base.GetControlValues( ConditionControls, ref dicCondition );
                    var ddlLineCd = DataUtils.GetDictionaryStringVal( dicCondition, "ddlLineCd" );  //ラインコード
                    var ddlStation = DataUtils.GetDictionaryStringVal( dicCondition, "ddlStation" );  //ステーション
                    var txtItemCode = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHDETAIL.ITEM_CD.bindField].ToString();      //検査項目コード
                    var txtItemNm = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHDETAIL.ITEM_NM.bindField].ToString();      //検査項目名称
                    var txtRevisionNo = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHDETAIL.REVISION_NO.bindField].ToString();      //改訂番号
                    var txtLineNm = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHDETAIL.LINE_NM.bindField].ToString();      //ライン名称
                    var txtStationNm = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHDETAIL.STATION_NM.bindField].ToString();      //ステーション名
                    var txtUsingToolCd = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHDETAIL.USING_TOOL_CD.bindField].ToString();      //利用ツール
                    var txtStream = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHDETAIL.STREAM.bindField].ToString();      //ストリーム
                    var txtUsingModel = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHDETAIL.USING_MODEL.bindField].ToString();      //利用モデル
                    var txtExpectResult = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHDETAIL.EXPECT_RESULT.bindField].ToString();      //期待結果


                    //keyの作成
                    ListItem[] liArr = new ListItem[10];
                    liArr[0] = new ListItem( "exeKbn", "2" );
                    liArr[1] = new ListItem( "ddlLineCd", ddlLineCd );
                    liArr[2] = new ListItem( "ddlStation", ddlStation );
                    liArr[3] = new ListItem( "txtItemCode", txtItemCode );
                    liArr[4] = new ListItem( "txtRevisionNo", txtRevisionNo );
                    liArr[5] = new ListItem( "txtItemNm", txtItemNm );
                    liArr[6] = new ListItem( "txtUsingToolCd", txtUsingToolCd );
                    liArr[7] = new ListItem( "txtStream", txtStream );
                    liArr[8] = new ListItem( "txtUsingModel", txtUsingModel );
                    liArr[9] = new ListItem( "txtExpectResult", txtExpectResult );

                    //ダブルクリック用
                    e.Row.Attributes[ON_DBLCLICK] = AiImageCheck.ImageInputModal.ImageCreateDispUrl( this, PageInfo.AnlItemInput, 10, 10, liArr, "検査項目更新", "input" );
                }

            }

        }


       

        #endregion

		#endregion

		#region グリッドビュー操作

		/// <summary>
		/// グリッドビュークリア
		/// </summary>
		private void ClearGridView() {
            //列名非表示 グリッドビュークリア
            ControlUtils.InitializeGridView( grvHeaderLT, false );
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
            //btnModalDisp.Enabled = false;
        }

        /// <summary>
        /// グリッドビューページ切替
        /// </summary>
        /// <param name="parameters"></param>
        private void PageIndexChanging( params object[] parameters ) {
            object sender = parameters[0];
            CommandEventArgs e = (CommandEventArgs)parameters[1];
            int newPageIndex = Convert.ToInt32( e.CommandArgument );
            int pageSize = grvMainViewLB.PageSize;
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
            if ( allPages  <= newPageIndex ) {
                newPageIndex = 0;
            }
            // 背面ユーザ切替対応
            ConditionInfoSessionHandler.ST_CONDITION cond = ConditionInfo;
            GridView frozenGrid = grvMainViewLB;
            ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), cond, true );
            //ShowGridViewHeader( grvDetailHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), cond, true );
            ControlUtils.BindGridView_WithTempField( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), ConditionInfo.ResultData );
            ControlUtils.BindGridView_WithTempField( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, false ), ConditionInfo.ResultData );
           
            ControlUtils.GridViewPageIndexChanging( grvMainViewLB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
            ControlUtils.GridViewPageIndexChanging( grvMainViewRB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
            ControlUtils.SetGridViewPager( ref pnlPager, grvMainViewRB, grvDetailBody_PageIndexChanging, ConditionInfo.ResultData.Rows.Count, grvMainViewRB.PageIndex );

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
        private void SetDivGridViewWidth( ) {
            SetDivGridViewWidth( grvHeaderLT, divGrvHeaderLT );
            SetDivGridViewWidth( grvHeaderRT, divGrvHeaderRT );

            SetDivGridViewWidth( grvMainViewLB, divGrvLB );
            SetDivGridViewWidth( grvMainViewRB, divGrvRB );
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