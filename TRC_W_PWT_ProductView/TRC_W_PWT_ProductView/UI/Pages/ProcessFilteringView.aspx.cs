using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KTFramework.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Dao.Process;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.UI.Base;
using System.Data;

namespace TRC_W_PWT_ProductView.UI.Pages {
    /// <summary>
    /// 工程絞込検索画面
    /// </summary>
    public partial class ProcessFilteringView : BaseForm {
        #region 定数
        /// <summary>
        /// 一覧検索条件定義
        /// </summary>
        public class CONDITION {
            /// <summary>ラインコード</summary>
            public static readonly ControlDefine LINE_CD = new ControlDefine( "ddlLineCd", "ラインコード", "lineCd", ControlDefine.BindType.Up, typeof( string ) );
            /// <summary>工程名</summary>
            public static readonly ControlDefine PROCESS_NM = new ControlDefine( "txtProcessName", "作業コード", "processNm", ControlDefine.BindType.Up, typeof( string ) );
            /// <summary>作業名</summary>
            public static readonly ControlDefine WORK_NM = new ControlDefine( "txtWorkName", "作業コード", "workNm", ControlDefine.BindType.Up, typeof( string ) );
            /// <summary>選択位置</summary>
            public static readonly ControlDefine SELECTED_INDEX = new ControlDefine( "hdnSelectedIndex", "選択位置", "selectedIndex", ControlDefine.BindType.Up, typeof( string ) );
        }

        /// <summary>
        /// 工程作業一覧定義
        /// </summary>
        internal class GRID_PROCESS_WORK {
            /// <summary>ライン名</summary>
            public static readonly GridViewDefine LINE_NM = new GridViewDefine( "ライン", "lineNm", typeof( string ), "", false, HorizontalAlign.Left, 200, true );
            /// <summary>工程名</summary>
            public static readonly GridViewDefine PROCESS_NM = new GridViewDefine( "工程", "processNm", typeof( string ), "", false, HorizontalAlign.Left, 400, true );
            /// <summary>作業名</summary>
            public static readonly GridViewDefine WORK_NM = new GridViewDefine( "作業名", "workNm", typeof( string ), "", false, HorizontalAlign.Left, 400, true );
            /// <summary>ライン名</summary>
            public static readonly GridViewDefine LINE_CD = new GridViewDefine( "", "lineCd", typeof( string ), "", false, HorizontalAlign.NotSet, 0, true );
            /// <summary>工程名</summary>
            public static readonly GridViewDefine PROCESS_CD = new GridViewDefine( "", "processCd", typeof( string ), "", false, HorizontalAlign.NotSet, 0, true );
            /// <summary>作業名</summary>
            public static readonly GridViewDefine WORK_CD = new GridViewDefine( "", "workCd", typeof( string ), "", false, HorizontalAlign.NotSet, 0, true );
            /// <summary>作業名</summary>
            public static readonly GridViewDefine SEARCH_TARGET_FLG = new GridViewDefine( "", "searchTargetFlg", typeof( string ), "", false, HorizontalAlign.NotSet, 0, true );
        }
        #region スクリプト
        /// <summary>
        /// ポストバック後実行スクリプト
        /// </summary>
        private static readonly KeyValuePair<string, string> SCRIPT_AFTER_SELECT = new KeyValuePair<string, string>( "ProcessFilteringView_AfterSelect", "$(function(){ProcessFilteringView.AfterSelect()});" );
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
                    _gridviewDefault = ControlUtils.GetGridViewDefineArray( typeof( GRID_PROCESS_WORK ) );
                }
                return _gridviewDefault;
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
        #endregion

        #region メソッド
        /// <summary>
        /// 初期処理
        /// </summary>
        protected override void Initialize() {
            base.Initialize();
            // コントロール初期化
            ddlLineCd.Items.Clear();

            //パラメータ取得
            //製品種別
            string productKind = StringUtils.ToString(Page.Request.QueryString.Get("productKind"));
            
            txtProcessName.Value = string.Empty;
            txtWorkName.Value = string.Empty;

            // ライン一覧取得
            DataTable lineList = ProcessFilteringDao.SelectLineList( productKind );

            if (null == lineList || 0 == lineList.Rows.Count) {
                //ライン一覧が0件の場合、警告メッセージを表示
                base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_61090 );
            }

            // ライン一覧取得
            ControlUtils.SetListControlItems(ddlLineCd, Common.ControlUtils.GetListItems( lineList, "lineNm", "lineCd", true ) );
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        private void DoSearch() {
            // 検索パラメータ取得
            Dictionary<string, object> dicCondition = new Dictionary<string, object>();
            base.GetControlValues( ConditionControls, ref dicCondition );
            var lineCd = DataUtils.GetDictionaryStringVal( dicCondition, ProcessFilteringView.CONDITION.LINE_CD.bindField );
            var processName = DataUtils.GetDictionaryStringVal( dicCondition, ProcessFilteringView.CONDITION.PROCESS_NM.bindField );
            var workName = DataUtils.GetDictionaryStringVal( dicCondition, ProcessFilteringView.CONDITION.WORK_NM.bindField );
            // 必須項目チェック
            if ( true == StringUtils.IsEmpty( lineCd ) ) {
                // ライン未選択の場合
                base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_61060 );
                return;
            }
            // 工程作業一覧取得
            var tblResult = ProcessFilteringDao.SelectProcessWorkList( new ProcessFilteringDao.SearchParameter() {
                LineCd = lineCd,
                ProcessName = processName,
                WorkName = workName
            } );
            if ( 0 == tblResult.Rows.Count ) {
                // 工程作業一覧の件数が0の場合
                base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_61070 );
                return;
            }
            // 工程作業一覧をDataGridViewに設定
            ControlUtils.BindGridView( grvProcessWork, gridviewDefault, tblResult );
        }

        /// <summary>
        /// 選択処理
        /// </summary>
        private void DoSelect() {
            // 検索パラメータ取得
            Dictionary<string, object> dicCondition = new Dictionary<string, object>();
            base.GetControlValues( ConditionControls, ref dicCondition );
            // 選択行チェック
            int selectedIndex = -1;
            if ( false == int.TryParse( DataUtils.GetDictionaryStringVal( dicCondition, ProcessFilteringView.CONDITION.SELECTED_INDEX.bindField ), out selectedIndex ) || -1 == selectedIndex ) {
                // 選択位置が数値変換できない、または0未満の場合
                base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_61080 );
                return;
            }
            // 検索画面にパラメータを渡すため、非表示項目に選択行の値を設定
            hdnLineCd.Value = grvProcessWork.Rows[selectedIndex].Cells[ControlUtils.GetGridViewDefineIndex( gridviewDefault, GRID_PROCESS_WORK.LINE_CD )].Text.Replace( "&nbsp;", "");
            hdnProcessCd.Value = grvProcessWork.Rows[selectedIndex].Cells[ControlUtils.GetGridViewDefineIndex( gridviewDefault, GRID_PROCESS_WORK.PROCESS_CD )].Text.Replace( "&nbsp;", "" );
            hdnProcessNm.Value = grvProcessWork.Rows[selectedIndex].Cells[ControlUtils.GetGridViewDefineIndex( gridviewDefault, GRID_PROCESS_WORK.PROCESS_NM )].Text.Replace( "&nbsp;", "" );
            hdnWorkCd.Value = grvProcessWork.Rows[selectedIndex].Cells[ControlUtils.GetGridViewDefineIndex( gridviewDefault, GRID_PROCESS_WORK.WORK_CD )].Text.Replace( "&nbsp;", "" );
            hdnWorkNm.Value = grvProcessWork.Rows[selectedIndex].Cells[ControlUtils.GetGridViewDefineIndex( gridviewDefault, GRID_PROCESS_WORK.WORK_NM )].Text.Replace( "&nbsp;", "" );
            hdnSearchTargetFlg.Value = grvProcessWork.Rows[selectedIndex].Cells[ControlUtils.GetGridViewDefineIndex( gridviewDefault, GRID_PROCESS_WORK.SEARCH_TARGET_FLG )].Text.Replace( "&nbsp;", "" );
            // ポストバック後、親画面にパラメータを渡して画面を閉じるメソッドを実行
            ScriptManager.RegisterClientScriptBlock( btnSearch, btnSearch.GetType(), SCRIPT_AFTER_SELECT.Key, SCRIPT_AFTER_SELECT.Value, true );
        }
        #endregion
    }
}