using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KTFramework.Dao;
using KTFramework.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Dao.Process;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.UI.Base;
using TRC_W_PWT_ProductView.Business;
using TRC_W_PWT_ProductView.Session;
using TRC_W_PWT_ProductView.Dao.Com;
using System.Data;

namespace TRC_W_PWT_ProductView.UI.Pages.AiImageCheck {
    /// <summary>
    /// 型式紐づけ一覧追加画面
    /// </summary>
    public partial class AnlGroupModelInput : BaseForm {

        #region 定数
        const string CLOSE_MODAL_DISP = "AnlGroupModelInput.CloseModal();";

        private const int CONST_MAX_RECORD = 4000;
        /// <summary>
        /// 一覧検索条件定義
        /// </summary>
        public class CONDITION {
            /// <summary>型式コード</summary>
            public static readonly ControlDefine MODEL_CD = new ControlDefine( "txtModelCd", "型式コード", "txtModelCd", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>型式名</summary>
            public static readonly ControlDefine MODEL_NM = new ControlDefine( "txtModelNm", "型式名", "txtModelNm", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 画像解析グループコード
            /// </summary>
            public static readonly ControlDefine IMG_GROUP_CD = new ControlDefine( "txtImgGroupCd", "画像解析グループコード", "txtImgGroupCd", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 画像解析グループ名
            /// </summary>
            public static readonly ControlDefine IMG_GROUP_NM = new ControlDefine( "txtImgGroupNm", "画像解析グループ名", "txtImgGroupNm", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 未割当
            /// </summary>
            public static readonly ControlDefine CHK_TARGET = new ControlDefine( "chkTarget", "未割当", "chkTarget", ControlDefine.BindType.Both, typeof( Boolean ) );
        }

        /// <summary>
        /// 型式一覧定義
        /// </summary>
        internal class GRID_MODEL {
            /// <summary>
			/// チェックボックス
			/// </summary>
			public static readonly GridViewDefine UPD_CHECK = new GridViewDefine( "ﾁｪｯｸ", "", typeof( string ), "", false, HorizontalAlign.Center, 60, true, false );
            /// <summary>
			/// 型式
			/// </summary>
			public static readonly GridViewDefine MODEL_CD = new GridViewDefine( "型式コード", "ModelCode", typeof( string ), "", true, HorizontalAlign.Center, 120, true, true );
            /// <summary>
            /// 型式名
            /// </summary>
            public static readonly GridViewDefine MODEL_NM = new GridViewDefine( "型式名", "ModelName", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <summary>
            /// 国コード
            /// </summary>
            public static readonly GridViewDefine COUNTRY_CD = new GridViewDefine( "国コード", "CountryCode", typeof( string ), "", true, HorizontalAlign.Left, 90, true, true );
            /// <summary>
            /// 画像解析グループコード
            /// </summary>
            public static GridViewDefine GROUP_CD = new GridViewDefine( "画像解析グループコード", "GroupCode", typeof( string ), "", true, HorizontalAlign.Center, 200, true, true );
            /// <summary>
            /// 画像解析グループ名
            /// </summary>
            public static GridViewDefine GROUP_NM = new GridViewDefine( "画像解析グループ名", "GroupName", typeof( string ), "", true, HorizontalAlign.Left, 200, true, true );
        }


        /// <summary>
        /// 一覧表示情報コントロール(左下)
        /// </summary>
        public class GRID_SEARCH_CONTROLS_L {
            public static readonly ControlDefine UPD_CHECK = new ControlDefine( "chkUpdate", "ﾁｪｯｸ", "", ControlDefine.BindType.None, null );
        }

        #region スクリプト
        /// <summary>
        /// ポストバック後実行スクリプト
        /// </summary>
        private static readonly KeyValuePair<string, string> SCRIPT_AFTER_SELECT = new KeyValuePair<string, string>( "ProcessFilteringView_AfterSelect", "$(function(){ProcessFilteringView.AfterSelect()});" );
        #endregion

        #endregion

        #region 変数定義

        private static Dictionary<string, string> _orgData = new Dictionary<string, string>();
        private static Dictionary<string, string> _resultData = new Dictionary<string, string>();
        private int _execKbn = 1;

        #endregion

        #region プロパティ
        /// <summary>
        /// ロガー
        /// </summary>
        private static readonly Logger Logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );
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
        GridViewDefine[] gridviewDefault
        {
            get
            {
                if ( true == ObjectUtils.IsNull( _gridviewDefault ) ) {
                    _gridviewDefault = ControlUtils.GetGridViewDefineArray( typeof( GRID_MODEL ) );
                }
                return _gridviewDefault;
            }
        }
        /// <summary>
		/// 表示中ページ(ベースフォーム)情報
		/// </summary>
		private BaseForm CurrentForm {
            get {
                return ( (BaseForm)Page );
            }
        }
        #endregion

        #region イベント
        /// <summary>
        /// 初期表示イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load( object sender, EventArgs e ) {
            RaiseEvent( base.DoPageLoad );
            if ( true == IsPostBack ) {
                ControlUtils.ReMakeBoundTemplateFieldCtrl( grvProcessWork, gridviewDefault );
            }
            
        }

        /// <summary>
        /// 検索ボタン押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click( object sender, EventArgs e ) {
            RaiseEvent( DoSearch );
        }

        /// <summary>
        /// 工程作業一覧グリッドビューデータバインドイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvProcessWork_RowDataBound( object sender, GridViewRowEventArgs e ) {
            var gv = (GridView)sender;
            ControlUtils.GridViewRowBound( (GridView)sender, e );
            // 非表示列を設定
            var ht = gridviewDefault.Where( d => StringUtils.IsNotEmpty( d.headerText ) ).Select( d => d.headerText );
            for ( var cIndex = 0; cIndex < gv.Columns.Count; cIndex++ ) {
                if ( false == ht.Contains( gv.Columns[cIndex].HeaderText ) ) {
                    e.Row.Cells[cIndex].Style.Add( HtmlTextWriterStyle.Display, "none" );
                }
            }
        }

        /// <summary>
        /// 選択ボタン押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSelect_Click( object sender, EventArgs e ) {
            RaiseEvent( DoSelect );
        }

        /// <summary>
        /// 選択ボタン押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSelect_DBClick( object sender, EventArgs e ) {
            RaiseEvent( DoSelect );
        }
        #endregion

        #region メソッド
        /// <summary>
        /// 初期処理
        /// </summary>
        protected override void Initialize() {
            base.Initialize();
            //初期設定

            //閉じる
            btnCancel.Attributes[ControlUtils.ON_CLICK] = CLOSE_MODAL_DISP;
            //実行モード
            this._execKbn = NumericUtils.ToInt( Request.QueryString["exeKbn"] );

            //アクセスカウンター登録
            Dao.Com.AccessCounterDao.Entry( base.CurrentPageInfo.pageId );
            _resultData.Clear();

            _orgData.Clear();
            _orgData.Add( "exeKbn", Request.QueryString["exeKbn"] );
            _orgData.Add( "ddlLineCd", Request.QueryString["lineCd"] );
            _orgData.Add( "ddlStation", Request.QueryString["stationCd"] );
            _orgData.Add( "txtGroupCode", Request.QueryString["groupCd"] );
            _orgData.Add( "setubi", Request.QueryString["setubi"] );

            //ユーザ権限取得
            UserInfoSessionHandler.ST_USER loginInfo = base.SessionManager.GetUserInfoHandler().GetUserInfo();
            if ( true == ObjectUtils.IsNotNull( loginInfo.UserInfo ) ) {
                AppPermission.PERMISSION_INFO permInfo = AppPermission.GetTransactionPermission( PageInfo.AnlGroupModelInput, loginInfo.UserInfo );
                //更新権限があれば追加ボタンを活性化
                if ( permInfo.IsEdit ) {
                    btnSelect.Enabled = true;
                } else {
                    btnSelect.Enabled = false;
                }
            }
            btnAllCheck.Enabled = false;
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        private void DoSearch() {
            // 検索パラメータ取得
            Dictionary<string, object> dicCondition = new Dictionary<string, object>();
            base.GetControlValues( ConditionControls, ref dicCondition );
            var modelCd = DataUtils.GetDictionaryStringVal( dicCondition, AnlGroupModelInput.CONDITION.MODEL_CD.bindField );
            var modelNm = DataUtils.GetDictionaryStringVal( dicCondition, AnlGroupModelInput.CONDITION.MODEL_NM.bindField );
            var groupCd = DataUtils.GetDictionaryStringVal( dicCondition, AnlGroupModelInput.CONDITION.IMG_GROUP_CD.bindField );
            var groupNm = DataUtils.GetDictionaryStringVal( dicCondition, AnlGroupModelInput.CONDITION.IMG_GROUP_NM.bindField );
            //一覧表示列の設定
            GridViewDefine[] gridColumns;
            gridColumns = ControlUtils.GetGridViewDefineArray( typeof( GRID_MODEL ) );
            //検索結果取得
            //エラー時にも最後まで処理を通すため、インスタンスを生成しておく
            AiImageCheckBusiness.ImgCheckResultSet result = new AiImageCheckBusiness.ImgCheckResultSet();
            DataTable tblResult = null;

            //int maxGridViewCount = WebAppInstance.GetInstance().Config.WebCommonInfo.maxGridViewCount;  //検索上限数
            //作業指示保存が重いため、4000件では耐えれない処理速度のため1000件としておく
            int maxGridViewCount = CONST_MAX_RECORD;

            try {
                result = AiImageCheckBusiness.SelectModelList( groupCd, groupNm, modelCd, modelNm, chkTarget.Checked, _orgData["txtGroupCode"].ToString(), _orgData["ddlStation"].ToString(), maxGridViewCount );

            } catch ( DataAccessException ex ) {
                if ( ex.OracleErrorNumber == DataAccessBase.ORAERROR_TIMEOUT ) {
                    //クエリ発行タイムアウト
                    base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_61910 );
                } else {
                    //タイムアウト以外のException
                    Logger.Exception( ex );
                    base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80011, "検索処理で" );
                }
            } catch ( Exception ex ) {
                Logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_84010 );
            } finally {

            }
            ConditionInfoSessionHandler.ST_CONDITION cond = new ConditionInfoSessionHandler.ST_CONDITION();
            tblResult = result.ListTable;
            if ( null != tblResult ) {
                foreach ( DataRow row in tblResult.Rows ) {
                    row["ModelCode"] = DataUtils.GetModelCdStr( StringUtils.ToString( row["ModelCode"] ) );
                }

                //検索条件/結果インスタンス
                cond.conditionValue = dicCondition;
                
                cond.ResultData = tblResult.DefaultView.ToTable();
                btnAllCheck.Enabled = true;
            } else {
                //タイムアウト等Exception時には、GridViewクリア
                ClearGridView();
            }
            //格納実施
            ConditionInfo = cond;
            // 型式一覧をDataGridViewに設定
            ControlUtils.BindGridView_WithTempField( grvProcessWork, gridviewDefault, result.ListTable );
            //メッセージ表示
            if ( null != result.Message ) {
                base.WriteApplicationMessage( result.Message );
            }
        }

        /// <summary>
        /// 選択処理
        /// </summary>
        private void DoSelect() {
            int exeCnt = 0;
            //更新前権限チェック
            if ( CheckInputData() == false ) {
                return;
            }
            //更新前データ整形
            DataTable dtUpdate = new DataTable();
            dtUpdate.Columns.Add( "MODEL_CD" );
            dtUpdate.Columns.Add( "COUNTRY_CD" );
            dtUpdate.Columns.Add( "SETUBI" );
            dtUpdate.Columns.Add( "STATION" );
            dtUpdate.Columns.Add( "GROUP_CD" );
            //セッションから検索データ取得
            ConditionInfoSessionHandler.ST_CONDITION cond = base.ConditionInfo;
            if ( null == cond.ResultData || 0 == cond.ResultData.Rows.Count ) {
                //出力対象データなし
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_72040 );
                return;
            }
            foreach ( GridViewRow row in grvProcessWork.Rows ) {
                CheckBox chkDel = (CheckBox)row.FindControl( GRID_SEARCH_CONTROLS_L.UPD_CHECK.controlId );
                if ( true == chkDel.Checked ) {

                    DataRow row2 = dtUpdate.NewRow();
                    row2["MODEL_CD"] = DataUtils.GetModelCd( StringUtils.ToString( cond.ResultData.Rows[row.DataItemIndex][GRID_MODEL.MODEL_CD.bindField] ) );
                    row2["COUNTRY_CD"] = StringUtils.ToString( cond.ResultData.Rows[row.DataItemIndex][GRID_MODEL.COUNTRY_CD.bindField] );
                    row2["SETUBI"] = _orgData["setubi"].ToString();
                    row2["STATION"] = _orgData["ddlStation"].ToString();
                    row2["GROUP_CD"] = _orgData["txtGroupCode"].ToString();
                    dtUpdate.Rows.Add( row2 );

                    exeCnt++;
                }
            }
            if ( exeCnt.Equals( 0 ) ) {
                //処理対象データなし
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62020, "追加対象データ" );
                return;
            }
            //追加処理
            int retCnt = 0;
            //更新
            retCnt = AiImageCheckDao.InsertSetubiJyohoData( dtUpdate, LoginInfo.UserInfo.userId, PageInfo.AnlGroupModelInput.pageId, exeCnt );

            if ( retCnt.Equals( exeCnt ) ) {
                //正常
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_INF_10010 );
            } else if ( retCnt != exeCnt ) {
                //処理件数不一致
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_72060, "追加" );
                return;
            } else {
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                return;
            }

            //正常終了時、再検索
            DoSearch();
        }

        /// <summary>
		/// 処理実行前データチェック
		/// </summary>
		/// <returns></returns>
		private bool CheckInputData( ) {

            //ユーザ情報チェック
            if ( true == ObjectUtils.IsNull( LoginInfo.UserInfo )
                || true == StringUtils.IsEmpty( LoginInfo.UserInfo.userName ) ) {
                PageUtils.RedirectToErrorPage( Page, MsgManager.MESSAGE_ERR_72020, null );
                return false;
            }

            //更新権限チェック
            AppPermission.PERMISSION_INFO permMainteInfo = AppPermission.GetTransactionPermission( PageInfo.AnlGroupModelInput, LoginInfo.UserInfo );
            if ( permMainteInfo.IsEdit == false ) {
                PageUtils.RedirectToErrorPage( Page, MsgManager.MESSAGE_ERR_72030, null );
                return false;
            }

            return true;
        }

        /// <summary>
		/// グリッドビュークリア
		/// </summary>
		private void ClearGridView( ) {
            //列名非表示 グリッドビュークリア
            ControlUtils.InitializeGridView( grvProcessWork, false );
        }
        #endregion
    }
}