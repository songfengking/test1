using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KTFramework.Common;
using KTFramework.Dao;
using KTWebControl.CustomControls;
using KTWebInheritance.Common;
using TRC_W_PWT_ProductView.Business;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.ListDefine;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.Session;
using TRC_W_PWT_ProductView.UI.Base;

namespace TRC_W_PWT_ProductView.UI.Pages.AiImageCheck {

    public partial class MainCorrectiveList : BaseForm {

        #region 定数定義

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        /// <summary>
        /// 空き(表示用)
        /// </summary>
        const string FLAG_DISP = "";
        /// <summary>
        /// OK(表示用)
        /// </summary>
        const string FLAG_OK_DISP = "OK";
        /// <summary>
        /// NG(表示用)
        /// </summary>
        const string FLAG_NG_DISP = "NG";
        /// <summary>
        /// 零(表示用)
        /// </summary>
        const string FLAG_ZERO_DISP = "0";
        /// <summary>
        /// 不良タイプフラグ
        /// </summary>
        const string NG_FLAG = "1";
        /// <summary>
        /// 是正フラグ
        /// </summary>
        const string CORRECTIVE_FLAG = "2";
        /// <summary>
        /// 調整フラグ
        /// </summary>
        const int ADJ_FLAG = 1;
        /// <summary>
        /// ZEROフラグ
        /// </summary>
        const int ZERO_FLAG = 0;
        /// <summary>
        /// <summary>メッセージ(Key)</summary>
        /// </summary>
        const string MSG_KEY = "MSG";
        /// <summary>
        /// オブジェクトダブルクリック
        /// </summary>
        const string ON_DBLCLICK = "ondblclick";
        /// <summary>
        /// 管理ID
        /// </summary>
        const string MANAGE_ID = "MainCorrectiveList";
        /// <summary>
        /// 是正処置入力画面(表示用)
        /// </summary>
        const string FLAG_IMAGE_LIST_DISP = "是正処置入力画面";
        /// <summary>行選択イベント</summary>
        const string MAIN_VIEW_SELECTED = "AiImageDef.SelectMainViewRow(this,'{0}');";
        /// <summary>
        /// 詳細画面切替イベント(サムネイル画像クリックでメインエリアに通常サイズ表示を行う)
        /// </summary>
        /// <remarks>パラメータ 差し替え先コントロール URL</remarks>
        const string CHANGE_MAIN_AREA_VIEW = "AiImageDef.ChangeMainAreaImage('{0}','{1}');";
        const string CLOSE_MODAL_DISP = "AiImageDef.DialogCloseModal();";

        /// <summary>
        /// パブリック変数
        /// </summary>
        public static string ddlLineCd = "";
        public static string ddlStation = "";
        public static string txtIdno = "";
        public static string txtMonthlySeqNum = "";
        public static string txtRecordNo = "";
        private static Msg errorInfo =  null;

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
                    _gridviewDefault = ControlUtils.GetGridViewDefineArray( typeof( GRID_MAIN ) );
                }
                return _gridviewDefault;
            }
        }

        /// <summary>
        /// グリッドビュー定義
        /// </summary>
        public class GRID_MAIN {
            /// <summary>
            /// 検査結果
            /// </summary>
            public static readonly GridViewDefine ANL_RESULT = new GridViewDefine( "検査結果", "AnlResult", typeof( string ), "", true, HorizontalAlign.Left, 100, true, true );
            /// <summary>
            /// No.
            /// </summary>
            public static readonly GridViewDefine DISPLAY_ORDER = new GridViewDefine( "No.", "DisplayOrder", typeof( string ), "", true, HorizontalAlign.Left, 50, true, true );
            /// <summary>
            /// 検査項目名
            /// </summary>
            public static readonly GridViewDefine IMG_ANL_ITEM_NM = new GridViewDefine( "検査項目名", "ImgAnlItemNm", typeof( string ), "", true, HorizontalAlign.Left, 120, true, true );
            /// <summary>
            /// 是正結果
            /// </summary>
            public static readonly GridViewDefine CORRECTIVE_RESULT = new GridViewDefine( "是正結果", "CorrectiveResult", typeof( string ), "", true, HorizontalAlign.Left, 100, true, true );
            /// <summary>
            /// 不良タイプ
            /// </summary>
            public static readonly GridViewDefine NG_TYPE_NM = new GridViewDefine( "不良タイプ", "NgTypeNm", typeof( string ), "", true, HorizontalAlign.Left, 130, true, true );
            /// <summary>
            /// 是正タイプ
            /// </summary>
            public static readonly GridViewDefine CORRECTIVE_TYPE_NM = new GridViewDefine( "是正タイプ", "CorrectiveTypeNm", typeof( string ), "", true, HorizontalAlign.Left, 130, true, true );
            /// <summary>
            /// 是正コメント
            /// </summary>
            public static readonly GridViewDefine CORRECTIVE_COMMENT = new GridViewDefine( "是正コメント", "CorrectiveComment", typeof( string ), "", true, HorizontalAlign.Left, 240, true, true );
            /// <summary>
            /// 前回更新者
            /// </summary>
            public static readonly GridViewDefine UPDATE_BY = new GridViewDefine( "前回更新者", "UpdateBy", typeof( string ), "", true, HorizontalAlign.Left, 120, true, true );
            /// <summary>
            /// 画像解析項目コード
            /// </summary>
            public static readonly GridViewDefine IMG_ANL_ITEM_CD = new GridViewDefine( "画像解析項目コード", "ImgAnlItemCd", typeof( string ), "", true, HorizontalAlign.Left, 150, true, true );
            /// <summary>
            /// 改定番号
            /// </summary>
            public static readonly GridViewDefine REVISION_NO = new GridViewDefine( "改定番号", "RevisionNo", typeof( string ), "", true, HorizontalAlign.Left, 100, true, true );
            /// <summary>
            /// 不良タイプコード
            /// </summary>
            public static readonly GridViewDefine NG_TYPE_CD = new GridViewDefine( "不良タイプコード", "NgTypeCd", typeof( string ), "", true, HorizontalAlign.Left, 100, true, true );
            /// <summary>
            /// 是正タイプコード
            /// </summary>
            public static readonly GridViewDefine CORRECTIVE_TYPE_CD = new GridViewDefine( "是正タイプコード", "CorrectiveTypeCd", typeof( string ), "", true, HorizontalAlign.Left, 100, true, true );
       }

        /// <summary>
        /// 検査画像　コントロール定義
        /// </summary>
        public class CHK_IMG_LIST {
            /// <summary>DIV</summary>
            public static readonly ControlDefine DIV_ROW_DATA = new ControlDefine( "divRowData", "div", "", ControlDefine.BindType.None, typeof( Byte[] ) );
            /// <summary>画像ファイル</summary>
            public static readonly ControlDefine IMAGE_DATA = new ControlDefine( "imgCameraImage", "画像ファイル", "imageData", ControlDefine.BindType.None, typeof( Byte[] ) );
            /// <summary>工程コード</summary>
            public static readonly ControlDefine IMAGE_FILE_NM = new ControlDefine( "txtFileNm", "画像ファイル名", "imageFileNm", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>画像タイトル</summary>
            public static readonly ControlDefine TAKEN_DATE = new ControlDefine( "txtTakenDate", "撮像日時", "takenDate", ControlDefine.BindType.Down, typeof( DateTime ) );
        }

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

        #region ページイベント

        /// <summary>
        /// ページロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load( object sender, EventArgs e ) {
            CurrentForm.RaiseEvent( DoPageLoad );
        }

        /// <summary>
        /// ページロード処理
        /// </summary>
        private void DoPageLoad( ) {
            //閉じる
            btnClose.Attributes[ControlUtils.ON_CLICK] = CLOSE_MODAL_DISP;
            Initialize();
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize( ) {

            //アクセスカウンター登録
            Dao.Com.AccessCounterDao.Entry( base.CurrentPageInfo.pageId );

            //パラメータ取得
            ddlLineCd = Request.QueryString["ddlLineCd"] ;
            ddlStation = Request.QueryString["ddlStation"] ;
            txtIdno = Request.QueryString["txtIdno"] ;
            txtMonthlySeqNum = Request.QueryString["txtMonthlySeqNum"] ;
            txtRecordNo = Request.QueryString["txtRecordNo"] ;
            
            //検査画像初期値設定
            InitializeValuesImage( ddlStation, txtIdno, txtMonthlySeqNum, txtRecordNo);

            //検査結果初期値設定
            InitializeValuesResult( ddlLineCd, ddlStation, txtIdno, txtMonthlySeqNum, txtRecordNo);
        }

        /// <summary>
        /// 検査画像初期値設定
        /// </summary>
        private void InitializeValuesImage( string ddlStation, string txtIdno, string txtMonthlySeqNum, string txtRecordNo ) {

            //検索結果取得
            // エラー時にも最後まで処理を通すため、インスタンスを生成しておく
            var result = new AiImageCheckBusiness.ImgCheckResultSet();

            //エラー変数初期値設定
            if ( null != errorInfo ) {
                errorInfo = null;
            }

            //検索処理
                try {
                // 検索cファイルパス処理を実行する
                result = AiImageCheckBusiness.SearchOfImagePath( ddlStation, txtIdno, txtMonthlySeqNum, txtRecordNo );
            } catch ( DataAccessException ex ) {
                if ( ex.OracleErrorNumber == DataAccessBase.ORAERROR_TIMEOUT ) {
                    // クエリ発行タイムアウトが発生した場合、エラーメッセージを設定する
                    errorInfo = new Msg( MsgManager.MESSAGE_WRN_61910 );
                } else {
                    // タイムアウト以外のExceptionが発生した場合、エラーメッセージを設定する
                    logger.Exception( ex );
                    errorInfo = new Msg( MsgManager.MESSAGE_ERR_80011, "検索処理で" );
                }
            } catch ( Exception ex ) {
                // Exceptionが発生した場合、エラーメッセージを設定する
                logger.Exception( ex );
                errorInfo = new Msg( MsgManager.MESSAGE_ERR_84010 );
            } finally {
            }

            //検索結果0件
            if ( 0 == result.ListTable.Rows.Count ) {
                // メッセージが存在する場合、メッセージ表示
                errorInfo = result.Message;
                tabStation.Visible = false;
                return;
            }

            // データテーブルまとめる
            string imagePath = result.ListTable.Rows[0].ItemArray[0].ToString();

            //画像パスがNUllまたは空値です
            if ( true == string.IsNullOrEmpty(imagePath) ) {
                // メッセージが存在する場合、メッセージ表示
                errorInfo = new Msg( MsgManager.MESSAGE_WRN_61010 );
                tabStation.Visible = false;
                return;
            }
            
            // カメラ画像設定
            InitializeCheckImage( imagePath );
        }

        /// <summary>
        /// 検査結果初期値設定
        /// </summary>
        private void InitializeValuesResult( string ddlLineCd, string ddlStation, string txtIdno, string txtMonthlySeqNum, string txtRecordNo ) {

            //検索結果取得
            // エラー時にも最後まで処理を通すため、インスタンスを生成しておく
            var result = new AiImageCheckBusiness.ImgCheckResultSet();

            //検索処理
            try {
                // 是正処置入力検査結果検索処理を実行する
                result = AiImageCheckBusiness.SearchOfImageAnlResultList( ddlLineCd, ddlStation, txtIdno, txtMonthlySeqNum, txtRecordNo, NG_FLAG, CORRECTIVE_FLAG );
            } catch ( DataAccessException ex ) {
                if ( ex.OracleErrorNumber == DataAccessBase.ORAERROR_TIMEOUT ) {
                    // クエリ発行タイムアウトが発生した場合、エラーメッセージを設定する
                    base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_61910 );
                } else {
                    // タイムアウト以外のExceptionが発生した場合、エラーメッセージを設定する
                    logger.Exception( ex );
                    base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80011, "検索処理で" );
                }
            } catch ( Exception ex ) {
                // Exceptionが発生した場合、エラーメッセージを設定する
                logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_84010 );
            } finally {
            }

            // グリッドビューの表示処理を行う
            GridView frozenGrid = grvDetailBody;
            if ( true == ObjectUtils.IsNotNull( result.ListTable ) ) {
                if ( 0 < result.ListTable.Rows.Count ) {
                    for ( int idx = frozenGrid.Columns.Count; idx < GridviewDefault.Length; idx++ ) {
                        TemplateField tf = new TemplateField();
                        tf.HeaderText = StringUtils.ToString( GridviewDefault[idx].bindField );
                    }
                    // 新規バインド
                    ControlUtils.ShowGridViewHeader( grvDetailHeader, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), ConditionInfo, true );
                    ControlUtils.BindGridView_WithTempField( grvDetailBody, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), result.ListTable );
                    for ( int idy = 8; idy < GridviewDefault.Length; idy++ ) {
                        grvDetailBody.Columns[idy].Visible = false;
                        grvDetailHeader.Columns[idy].Visible = false;
                    }

                    for ( int idz = 0; idz < grvDetailBody.Rows.Count; idz++ ) {
                        string anlResult = ( (Label)grvDetailBody.Rows[idz].Cells[ZERO_FLAG].Controls[ZERO_FLAG] ).Text;
                        if ( String.Equals( anlResult, FLAG_NG_DISP ) ) {
                            grvDetailBody.Rows[idz].BackColor = System.Drawing.Color.Red;
                            grvDetailBody.Rows[idz].Font.Bold = true;
                        }
                    }
                    // GridView表示
                    divGrvDisplay.Visible = true;
                    // グリッドビュー外のDivサイズ変更
                    SetDivGridViewWidth();
                } else {
                    //一覧初期化
                    ClearGridView();
                }
            }

            //メッセージ表示
            if ( null != result.Message ) {
                // メッセージが存在する場合、メッセージ表示
                base.WriteApplicationMessage( result.Message );
            } else if ( null != errorInfo ) {
                // メッセージが存在する場合、メッセージ表示
                base.WriteApplicationMessage( errorInfo );
            }

        }

        /// <summary>
        /// グリッドビュークリア
        /// </summary>
        private void ClearGridView( ) {
            // 列名非表示 グリッドビュークリア
            ControlUtils.InitializeGridView( grvDetailHeader, false );
            ControlUtils.InitializeGridView( grvDetailBody, false );
            // ページャークリア
            ControlUtils.ClearPager( ref pnlPager );
            // GridView非表示
            divGrvDisplay.Visible = false;
        }

        /// <summary>
        /// グリッドビュー外のDivサイズ変更
        /// </summary>
        private void SetDivGridViewWidth( ) {
            SetDivGridViewWidth( grvDetailHeader, divGrvHeaderLT );
            SetDivGridViewWidth( grvDetailBody, divGrvLB );
            divGrvLB.Style["width"] = $"{ ( divGrvLB.Style["width"] + ADJ_FLAG) }px";
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
        /// グリッドビューページ切替
        /// </summary>
        /// <param name="parameters"></param>
        private void PageIndexChanging( params object[] parameters ) {
            object sender = parameters[0];
            CommandEventArgs e = (CommandEventArgs)parameters[1];
            int newPageIndex = Convert.ToInt32( e.CommandArgument );
            int pageSize = grvDetailBody.PageSize;
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
            if ( allPages <= newPageIndex ) {
                newPageIndex = 0;
            }
            // 背面ユーザ切替対応
            ConditionInfoSessionHandler.ST_CONDITION cond = ConditionInfo;
            GridView frozenGrid = grvDetailBody;
            ControlUtils.ShowGridViewHeader( grvDetailHeader, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), cond, true );
            ControlUtils.BindGridView_WithTempField( grvDetailBody, ControlUtils.GetFrozenColumns( frozenGrid, GridviewDefault, true ), ConditionInfo.ResultData );
            ControlUtils.GridViewPageIndexChanging( grvDetailBody, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
            ControlUtils.SetGridViewPager( ref pnlPager, grvDetailBody, grvDetailBody_PageIndexChanging, ConditionInfo.ResultData.Rows.Count, grvDetailBody.PageIndex );
            // グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();
        }

        /// <summary>
        /// メッセージの復元
        /// </summary>
        private void RestoreMsg( ) {
            var msg = CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).GetPageControlInfo( MSG_KEY );
            if ( 0 < msg.Count ) {
                // メッセージが設定されていた場合、メッセージ表示
                base.WriteApplicationMessage( (Msg)msg[MSG_KEY] );
            }
        }

        /// <summary>
        /// 一覧データバインドイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvSearch_RowDataBound( object sender, GridViewRowEventArgs e ) {
            try {
                ClearApplicationMessage();
                RowDataBound( sender, e );
            } catch ( Exception ex ) {
                //イベント処理中にエラー発生
                logger.Exception( ex );
                throw ex;
            }
        }

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
				// 選択行の背景色変更を追加
				ControlUtils.GridViewRowBound( sender, e, MANAGE_ID, ControlUtils.GridRowDoubleClickEvent.None );

                /// <summary>
                /// 権限による制御
                /// </summary>
                AppPermission.PERMISSION_INFO permAuthInfo = AppPermission.GetTransactionPermission( TRC_W_PWT_ProductView.Defines.PageInfo.MainCorrectiveInput, LoginInfo.UserInfo );

                //権限取得
                if ( permAuthInfo.IsEdit == true ) {
                    ////ダブルクリックイベント
                    var imgAnlItemCd = ( (Label)e.Row.Cells[8].Controls[0] ).Text;               //画像解析項目コード
                    var revisionNo = ( (Label)e.Row.Cells[9].Controls[0] ).Text;                 //改定番号
                    var anlResult = ( (Label)e.Row.Cells[0].Controls[0] ).Text;                  //検査結果
                    var imgAnlItemNm = ( (Label)e.Row.Cells[2].Controls[0] ).Text;               //検査項目名    
                    var correctiveResult = ( (Label)e.Row.Cells[3].Controls[0] ).Text;           //是正結果  
                    var ngTypeCd = ( (Label)e.Row.Cells[10].Controls[0] ).Text;                  //不良タイプコード     
                    var correctiveTypeCd = ( (Label)e.Row.Cells[11].Controls[0] ).Text;          //是正タイプコード   
                    var correctiveComment = ( (Label)e.Row.Cells[6].Controls[0] ).Text;          //是正コメント  
                    var updateBy = ( (Label)e.Row.Cells[7].Controls[0] ).Text;                   //前回更新者

                    //keyの作成
                    ListItem[] liArr = new ListItem[9];
                    liArr[0] = new ListItem( "imgAnlItemCd", imgAnlItemCd );
                    liArr[1] = new ListItem( "revisionNo", revisionNo );
                    liArr[2] = new ListItem( "anlResult", anlResult );
                    liArr[3] = new ListItem( "imgAnlItemNm", imgAnlItemNm );
                    liArr[4] = new ListItem( "correctiveResult", correctiveResult );
                    liArr[5] = new ListItem( "ngTypeCd", ngTypeCd );
                    liArr[6] = new ListItem( "correctiveTypeCd", correctiveTypeCd );
                    liArr[7] = new ListItem( "correctiveComment", correctiveComment );
                    liArr[8] = new ListItem( "updateBy", updateBy );

                    //ダブルクリック用
                    e.Row.Attributes[ON_DBLCLICK] = AiImageCheck.ImageInputModal.ImageCreateDispUrl( this, PageInfo.MainCorrectiveInput, 10, 10, liArr, FLAG_IMAGE_LIST_DISP, "input" );
                }
            }
        }

        #endregion

        #region 検査画像リストバインド

        /// <summary>
        /// イメージリスト行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstImageList_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundImageList( sender, e );
        }

        /// <summary>
        /// カメラ画像設定
        /// </summary>
        /// <param name="imageDirPath"></param>
        private void InitializeCheckImage( string imageDirPath) {
            DataTable tblCameraImage = new DataTable();
            tblCameraImage.Columns.Add( "imageData", typeof( byte[] ) );
            tblCameraImage.Columns.Add( "imageFileNm" );
            tblCameraImage.Columns.Add( "takenDate" );

            // プレビュー画像パス
            if ( ( StringUtils.IsNotEmpty( imageDirPath ) == true ) && ( System.IO.Directory.Exists( imageDirPath ) == true ) ) {
                // パスが登録されていて、フォルダが存在している
                // 画像ファイルをすべて取得
                string[] extensions = new string[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                var imageFiles = Directory.GetFiles( imageDirPath, "*.*" ).Where( c => extensions.Any( extension => c.EndsWith( extension ) ) ).ToArray();

                foreach ( string imageFile in imageFiles ) {
                    // ファイルの内容をすべて読み込む
                    System.IO.FileStream fs = new System.IO.FileStream( imageFile, System.IO.FileMode.Open, System.IO.FileAccess.Read );
                    byte[] bs = new byte[fs.Length];
                    fs.Read( bs, 0, bs.Length );
                    // 閉じる
                    fs.Close();

                    // 行定義
                    DataRow dr = tblCameraImage.NewRow();
                    dr["imageData"] = bs;
                    dr["imageFileNm"] = Path.GetFileName( imageFile );
                    dr["takenDate"] = StringUtils.ToString( File.GetLastWriteTime( imageFile ), DateUtils.DATE_FORMAT_SECOND );

                    // DataTableにデータを追加
                    tblCameraImage.Rows.Add( dr );
                }
            }

            // 画像データバインド
            // <LayoutTemplate>
            lstImageList.LayoutTemplate = new LayoutTemplateImageList( FLAG_ZERO_DISP );
            // <ItemTemplate>
            lstImageList.ItemTemplate = new ItemTemplateImageList( FLAG_ZERO_DISP );
            lstImageList.DataSource = tblCameraImage;
            lstImageList.DataBind();

            // 表示データ制御
            // IDからコントロールを取得
            if ( 0 < tblCameraImage.Rows.Count ) {
                // 表示データが存在する場合
               imgMainArea.Visible = true;
            } else {
                // 表示データが存在しない場合
               imgMainArea.Visible = false;
               tabStation.Visible = false;
               errorInfo = new Msg( MsgManager.MESSAGE_WRN_61010 );
            }

            // 画像データをセッションに保存
            Dictionary<string, byte[]> dicImages = new Dictionary<string, byte[]>();
            for ( int loopImg = 0; loopImg < tblCameraImage.Rows.Count; loopImg++ ) {
                byte[] byteImage = new byte[0];

                if ( ObjectUtils.IsNotNull( tblCameraImage.Rows[loopImg][CHK_IMG_LIST.IMAGE_DATA.bindField] ) ) {
                    byteImage = (byte[])tblCameraImage.Rows[loopImg][CHK_IMG_LIST.IMAGE_DATA.bindField];
                }
                dicImages.Add( loopImg.ToString(), byteImage );
            }
            CurrentForm.SessionManager.GetImageInfoHandler( CurrentForm.Token ).SetImages( MANAGE_ID , dicImages );
        }

        /// <summary>
        /// 検査イメージリストバインド処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void ItemDataBoundImageList( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {

                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Item, ChkImgCtrl, rowBind, ref dicSetValues );

                // イメージ画像URLセット(サムネイル)
                Image imgThumbnail = ( (Image)e.Item.FindControl( CHK_IMG_LIST.IMAGE_DATA.controlId ) );

                // IDからコントロールを取得
                if ( 0 == e.Item.DataItemIndex ) {
                    // 先頭行選択済
                    // メイン画像エリア
                    HtmlGenericControl divCtrl = ( (HtmlGenericControl)e.Item.FindControl( CHK_IMG_LIST.DIV_ROW_DATA.controlId ) );
                    divCtrl.Attributes["class"] = divCtrl.Attributes["class"] + " " + LIST_SELECTED_ITEM_CSS;
					string urlMainAreaTop = ImageView.GetImageViewUrl( this, CurrentForm.Token, MANAGE_ID , e.Item.DataItemIndex, 0, 0
						, TRC_W_PWT_ProductView.Session.ImageInfoSessionHandler.ContentType.Bitmap, false );
					imgMainArea.ImageUrl = urlMainAreaTop;

					// サムネイルエリア 先頭行
					string urlThumbnail = ImageView.GetImageViewUrl( this, CurrentForm.Token, MANAGE_ID , e.Item.DataItemIndex, LIST_MAX_WIDTH, LIST_MAX_HEIGHT
					, TRC_W_PWT_ProductView.Session.ImageInfoSessionHandler.ContentType.Bitmap, false );
					imgThumbnail.ImageUrl = urlThumbnail;
				} else {
					// 先頭行以外はページロード後に画像読み込み
					string urlThumbnail = ImageView.GetImageViewUrl( this, CurrentForm.Token, MANAGE_ID , e.Item.DataItemIndex, LIST_MAX_WIDTH, LIST_MAX_HEIGHT
						, TRC_W_PWT_ProductView.Session.ImageInfoSessionHandler.ContentType.Bitmap, true );
                    imgThumbnail.ImageUrl = urlThumbnail;
                    imgThumbnail.Attributes[ResourcePath.Image.AttrOriginalSrc] = urlThumbnail;
				}
				// クリックイベントセット
				string urlMainArea = ImageView.GetImageViewUrl( this, CurrentForm.Token, MANAGE_ID , e.Item.DataItemIndex, 0, 0
					, TRC_W_PWT_ProductView.Session.ImageInfoSessionHandler.ContentType.Bitmap, true );
				imgThumbnail.Attributes[ControlUtils.ON_CLICK] = String.Format( CHANGE_MAIN_AREA_VIEW, imgMainArea.ClientID, urlMainArea );

				// ツールチップ設定
				ControlUtils.SetToolTip( e.Item );
            }
        }

        /// <summary>
        /// LayoutTemplate(検査画像)
        /// </summary>
        private class LayoutTemplateImageList : ITemplate {
            /// <summary>ステーションコード</summary>
            string _stationCd = "";

            public LayoutTemplateImageList( string stationCd ) {
                _stationCd = stationCd;
            }

            public void InstantiateIn( Control container ) {
                // <div class="" id="itemPlaceholder" runat="server" />
                HtmlGenericControl itemPlaceholder = new HtmlGenericControl( "div" );
                itemPlaceholder.ID = "itemPlaceholder";
                itemPlaceholder.Attributes.Add( "class", "" );
                itemPlaceholder.Attributes.Add( "runat", "server" );

                container.Controls.Add( itemPlaceholder );
            }
        }

        /// <summary>
        /// ItemTemplate(検査画像)
        /// </summary>
        private class ItemTemplateImageList : ITemplate {
            /// <summary>ステーションコード</summary>
            string _stationCd = "";

            public ItemTemplateImageList( string stationCd ) {
                _stationCd = stationCd;
            }

            public void InstantiateIn( Control container ) {
                // <div id = "divRowData" runat="server" class="div-list-view-item" style="width: 208px; height: auto">
                HtmlGenericControl divRowData = new HtmlGenericControl( "div" );
                divRowData.ID = "divRowData";
                divRowData.Attributes.Add( "class", "div-list-view-item" );
                divRowData.Attributes.Add( "style", "width: 208px; height: auto" );
                divRowData.Attributes.Add( "runat", "server" );
                // <table class="table-border-layout" style="margin-left: 0px; margin-right: 1px">
                HtmlGenericControl table = new HtmlGenericControl( "table" );
                table.Attributes.Add( "class", "table-border-layout" );
                table.Attributes.Add( "style", "margin-left: 0px; margin-right: 1px" );
                // <colgroup>
                HtmlGenericControl colgroup = new HtmlGenericControl( "colgroup" );
                // < col style = "width: 202px" />
                HtmlGenericControl col = new HtmlGenericControl( "col" );
                col.Attributes.Add( "style", "width: 202px" );
                colgroup.Controls.Add( col );
                // <tr>
                HtmlTableRow tr1 = new HtmlTableRow();
                // <td>
                HtmlTableCell td1 = new HtmlTableCell( "td" );
                // <div>
                HtmlGenericControl div = new HtmlGenericControl( "div" );
                // < asp:Image ID = "imgCameraImage" runat="server" CssClass="thumbnail-area" />
                Image imgCameraImage = new Image();
                imgCameraImage.ID = CHK_IMG_LIST.IMAGE_DATA.controlId;
                imgCameraImage.Attributes.Add( "runat", "server" );
                imgCameraImage.CssClass = "thumbnail-area";
                div.Controls.Add( imgCameraImage );
                td1.Controls.Add( div );
                tr1.Controls.Add( td1 );
                // <tr>
                HtmlTableRow tr2 = new HtmlTableRow();
                // <td>
                HtmlTableCell td2 = new HtmlTableCell( "td" );
                // <KTCC:KTTextBox ID = "txtFileName" runat="server" ReadOnly="true" CssClass="font-default  txt-width-full al-ct" TextMode="MultiLine"></KTCC:KTTextBox>
                KTTextBox txtFileName = new KTTextBox();
                txtFileName.ID = CHK_IMG_LIST.IMAGE_FILE_NM.controlId;
                txtFileName.Attributes.Add( "runat", "server" );
                txtFileName.ReadOnly = true;
                txtFileName.CssClass = "font-default  txt-width-full al-ct";
                txtFileName.Attributes.Add( "TextMode", "MultiLine" );
                td2.Controls.Add( txtFileName );
                tr2.Controls.Add( td2 );
                // <tr>
                HtmlTableRow tr3 = new HtmlTableRow();
                // <td>
                HtmlTableCell td3 = new HtmlTableCell( "td" );
                // <KTCC:KTTextBox ID = "txtTakenDate" runat="server" ReadOnly="true" CssClass="font-default  txt-width-full al-ct" TextMode="MultiLine"></KTCC:KTTextBox>
                KTTextBox txtTakenDate = new KTTextBox();
                txtTakenDate.ID = CHK_IMG_LIST.TAKEN_DATE.controlId;
                txtTakenDate.Attributes.Add( "runat", "server" );
                txtTakenDate.ReadOnly = true;
                txtTakenDate.CssClass = "font-default  txt-width-full al-ct";
                txtTakenDate.Attributes.Add( "TextMode", "MultiLine" );
                td3.Controls.Add( txtTakenDate );
                tr3.Controls.Add( td3 );

                table.Controls.Add( colgroup );
                table.Controls.Add( tr1 );
                table.Controls.Add( tr2 );
                table.Controls.Add( tr3 );
                divRowData.Controls.Add( table );

                container.Controls.Add( divRowData );
            }
        }

        #endregion

    }
}