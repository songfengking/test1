using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KTFramework.Common;
using TRC_W_PWT_ProductView.Business;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Dao.Com;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.Session;
using TRC_W_PWT_ProductView.UI.Base;

namespace TRC_W_PWT_ProductView.UI.Pages.Kanban {
    /// <summary>
    /// ピッキング者選択画面
    /// </summary>
    public partial class KanbanPickingUserSelect : BaseForm {
        #region 定数
        /// <summary>
        /// 処理パラメータ定義
        /// </summary>
        public class CONDITION {
            /// <summary>選択位置</summary>
            public static readonly ControlDefine SELECTED_INDEX = new ControlDefine( "hdnSelectedIndex", "選択位置", "selectedIndex", ControlDefine.BindType.Up, typeof( string ) );
        }

        /// <summary>
        /// ピッキング者一覧定義
        /// </summary>
        internal class GRID_PICKING_USER {
            /// <summary>ユーザID</summary>
            public static readonly GridViewDefine USER_ID = new GridViewDefine( "ユーザID", "USER_ID", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>ユーザ名</summary>
            public static readonly GridViewDefine USER_NM = new GridViewDefine( "ユーザ名", "USER_NM", typeof( string ), "", true, HorizontalAlign.Left, 200, true, true );
        }

        /// <summary>
        /// 親画面更新用スクリプト
        /// </summary>
        private static readonly KeyValuePair<string, string> JS_FUNC_UPDATE = new KeyValuePair<string, string>( "KanbanPickingUserSelect_SelectUserUpdate", "$(function(){KanbanPickingUserSelect.SelectUserUpdate()});" );
        #endregion

        #region プロパティ
        //ロガー定義
        private static readonly Logger _logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        /// <summary>
        /// 処理パラメータ定義情報
        /// </summary>
        ControlDefine[] _conditionControls = null;
        /// <summary>
        /// 処理パラメータ定義情報アクセサ
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
                    _gridviewDefault = ControlUtils.GetGridViewDefineArray( typeof( GRID_PICKING_USER ) );
                }
                return _gridviewDefault;
            }
        }
        #endregion

        #region イベント
        /// <summary>
        /// ページロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load( object sender, EventArgs e ) {
            base.RaiseEvent( DoPageLoad );
        }

        /// <summary>
        /// 一覧行データバインドイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvPickingUserBody_RowDataBound( object sender, GridViewRowEventArgs e ) {
            ControlUtils.GridViewRowBound( (GridView)sender, e );
        }

        /// <summary>
        /// 一覧ページ遷移イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvPickingUserBody_PageIndexChanging( object sender, CommandEventArgs e ) {
            base.RaiseEvent( DoChangePageIndex, sender, e );
        }

        /// <summary>
        /// 一覧ソートイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvPickingUserHeader_Sorting( object sender, GridViewSortEventArgs e ) {
            base.RaiseEvent( DoSort, sender, e );
        }

        /// <summary>
        /// 選択ボタン押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSelect_Click( object sender, EventArgs e ) {
            base.RaiseEvent( SelectUser );
        }
        #endregion

        #region 画面初期化処理
        /// <summary>
        /// 初期処理
        /// </summary>
        protected override void DoPageLoad() {
            // ベース ページロード処理
            base.DoPageLoad();
            int resultCnt = 0;
            if ( ObjectUtils.IsNotNull( ConditionInfo.ResultData ) ) {
                resultCnt = ConditionInfo.ResultData.Rows.Count;
            }
            ControlUtils.SetGridViewPager( ref pnlPager, grvPickingUserBody, grvPickingUserBody_PageIndexChanging, resultCnt, grvPickingUserBody.PageIndex );
        }
        #endregion

        #region 機能別処理
        /// <summary>
        /// 初期処理
        /// </summary>
        protected override void Initialize() {
            // アクセスカウンター登録
            Dao.Com.AccessCounterDao.Entry( base.CurrentPageInfo.pageId );
            // ベース処理初期化処理
            base.Initialize();
            try {
                // ピッキング者一覧を取得する
                var pickingUserData = TmCcAuthUserDao.GetKanbanPickingUserInfo();
                // 結果をセッションに格納
                ConditionInfoSessionHandler.ST_CONDITION cond = new ConditionInfoSessionHandler.ST_CONDITION();
                cond.ResultData = pickingUserData;
                ConditionInfo = cond;
                if ( ConditionInfo.ResultData != null ) {
                    // 結果が存在する場合、件数表示
                    ntbResultCount.Value = pickingUserData.Rows.Count;
                    if ( ConditionInfo.ResultData.Rows.Count > 0 ) {
                        // 結果が1件以上存在する場合、ピッキング者一覧をDataGridViewに設定
                        ControlUtils.ShowGridViewHeader( grvPickingUserHeader, gridviewDefault, ConditionInfo, true );
                        ControlUtils.BindGridView_WithTempField( grvPickingUserBody, gridviewDefault, pickingUserData );
                        ControlUtils.SetGridViewPager( ref pnlPager, grvPickingUserBody, grvPickingUserBody_PageIndexChanging, ConditionInfo.ResultData.Rows.Count, grvPickingUserBody.PageIndex );
                    }
                }
            } catch ( Exception ex ) {
                _logger.Error( "ピッキング者選択画面.初期処理で例外発生：{0}", ex.Message );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_84010 );
            }
        }

        /// <summary>
        /// 「選択」ボタン押下時処理
        /// </summary>
        private void SelectUser() {
            // 検索パラメータ取得
            Dictionary<string, object> dicCondition = new Dictionary<string, object>();
            base.GetControlValues( ConditionControls, ref dicCondition );
            // 選択行チェック
            int selectedIndex = -1;
            if ( false == int.TryParse( DataUtils.GetDictionaryStringVal( dicCondition, ProcessFilteringView.CONDITION.SELECTED_INDEX.bindField ), out selectedIndex ) || -1 == selectedIndex ) {
                // 選択位置が数値変換できない、または0未満の場合
                base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_61080 );
            } else {
                var data = ConditionInfo.ResultData.Rows[grvPickingUserBody.Rows[selectedIndex].DataItemIndex];
                hdnPickingUserId.Value = data.ItemArray[ControlUtils.GetGridViewDefineIndex( gridviewDefault, GRID_PICKING_USER.USER_ID )].ToString().Replace( "&nbsp;", "" );
                hdnPickingUserNm.Value = data.ItemArray[ControlUtils.GetGridViewDefineIndex( gridviewDefault, GRID_PICKING_USER.USER_NM )].ToString().Replace( "&nbsp;", "" );
                ScriptManager.RegisterClientScriptBlock( btnSelect, btnSelect.GetType(), JS_FUNC_UPDATE.Key, JS_FUNC_UPDATE.Value, true );
            }
        }
        #endregion

        #region グリッドビュー操作
        /// <summary>
        /// グリッドビューページ切替
        /// </summary>
        /// <param name="parameters"></param>
        private void DoChangePageIndex( params object[] parameters ) {
            object sender = parameters[0];
            CommandEventArgs e = (CommandEventArgs)parameters[1];
            int newPageIndex = Convert.ToInt32( e.CommandArgument );
            int pageSize = grvPickingUserBody.PageSize;
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
            ControlUtils.ShowGridViewHeader( grvPickingUserHeader, gridviewDefault, ConditionInfo, true );
            ControlUtils.BindGridView_WithTempField( grvPickingUserBody, gridviewDefault, ConditionInfo.ResultData );
            ControlUtils.GridViewPageIndexChanging( grvPickingUserBody, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
            ControlUtils.SetGridViewPager( ref pnlPager, grvPickingUserBody, grvPickingUserBody_PageIndexChanging, ConditionInfo.ResultData.Rows.Count, grvPickingUserBody.PageIndex );
        }

        /// <summary>
        /// グリッドビュー並び替え
        /// </summary>
        /// <param name="parameters"></param>
        private void DoSort( params object[] parameters ) {
            object sender = parameters[0];
            GridViewSortEventArgs e = (GridViewSortEventArgs)parameters[1];
            ConditionInfoSessionHandler.ST_CONDITION cond = ConditionInfo;
            ControlUtils.GridViewSorting( grvPickingUserBody, ref cond, e, false );
            ControlUtils.ShowGridViewHeader( grvPickingUserHeader, gridviewDefault, cond, true );
            ControlUtils.BindGridView_WithTempField( grvPickingUserBody, gridviewDefault, ConditionInfo.ResultData );
            ControlUtils.SetGridViewPager( ref pnlPager, grvPickingUserBody, grvPickingUserBody_PageIndexChanging, ConditionInfo.ResultData.Rows.Count, grvPickingUserBody.PageIndex );
            ConditionInfo = cond;
        }
        #endregion
    }
}