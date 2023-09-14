using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KTFramework.Common;
using KTFramework.Dao;
using TRC_W_PWT_ProductView.Business;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.Interface;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.UI.Base;

namespace TRC_W_PWT_ProductView.UI.Pages.ProcessView {
    public partial class GenericDetail : System.Web.UI.UserControl, Defines.Interface.IDetail {
        #region 内部クラス
        /// <summary>
        /// 来歴一覧列定義
        /// </summary>
        internal class HistoryGridColumnDefine {
            /// <summary>表示順</summary>
            public static readonly GridViewDefine DISPLAY_ORDER = new GridViewDefine( "", "DISPLAY_ODR", typeof( int ), "", false, HorizontalAlign.NotSet, 0, true );
            /// <summary>作業コード</summary>
            public static readonly GridViewDefine WORK_CD = new GridViewDefine( "", "WORK_CD", typeof( string ), "", false, HorizontalAlign.NotSet, 0, true );
            /// <summary>作業名</summary>
            public static readonly GridViewDefine WORK_NM = new GridViewDefine( "作業名", "WORK_NM", typeof( string ), "", false, HorizontalAlign.Left, 200, true );
            /// <summary>最終来歴</summary>
            public static readonly GridViewDefine RECORD_NO = new GridViewDefine( "最終来歴", "RECORD_NO", typeof( string ), "", false, HorizontalAlign.Right, 100, true );
            /// <summary>最終結果</summary>
            public static readonly GridViewDefine RESULT_NM = new GridViewDefine( "最終結果", "RESULT_NM", typeof( string ), "", false, HorizontalAlign.Center, 200, true );
        }
        /// <summary>
        /// 測定結果一覧固定列定義
        /// </summary>
        internal class MeasuringResultGridFixColumnDefine {
            /// <summary>来歴</summary>
            public static readonly GridViewDefine RECORD_NO = new GridViewDefine( "来歴", "RECORD_NO", typeof( string ), "", false, HorizontalAlign.Right, 50, true );
            /// <summary>装置No</summary>
            public static readonly GridViewDefine DEVICE_NO = new GridViewDefine( "装置No", "DEVICE_NO", typeof( string ), "", false, HorizontalAlign.Right, 70, true );
            /// <summary>検査測定日時</summary>
            public static readonly GridViewDefine RESULT_DT = new GridViewDefine( "検査測定日時", "RESULT_DT", typeof( DateTime ), $"{{0:{DateUtils.DATE_FORMAT_SECOND}}}", false, HorizontalAlign.Center, 160, true );
        }
        /// <summary>
        /// 画面内コントロール
        /// </summary>
        internal class ViewControl {
            /// <summary>SELECTコマンド送信用ボタン</summary>
            public static readonly ControlDefine SELECT = new ControlDefine( "btnSelect", "BUTTON", "", ControlDefine.BindType.None, typeof( String ) );
        }
        #endregion
        #region 定数
        /// <summary>
        /// DB取得データ格納先 ディクショナリキー
        /// </summary>
        private const string SESSION_PAGE_INFO_DB_KEY = "bindTableData";
        /// <summary>
        /// グリッドビュー選択スクリプト文字列
        /// </summary>
        private const string GRIDVIEW_SELECTED = " GenericDetail.SelectGridViewRow(this,{0},'{1}');";
        #endregion

        #region プロパティ
        /// <summary>
        /// ログ出力機能
        /// </summary>
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        /// <summary>
        /// 詳細用必須引継ぎ情報
        /// </summary>
        public ST_DETAIL_PARAM DetailKeyParam { get; set; }

        /// <summary>
        /// 表示中フォーム
        /// </summary>
        private BaseForm CurrentForm { get { return (BaseForm)Page; } }

        /// <summary>
        /// 表示中ユーザコントロール情報
        /// </summary>
        private PageInfo.ST_PAGE_INFO CurrentUCInfo
        {
            get { return PageInfo.GetUCPageInfo( DetailKeyParam.ProductKind, DetailKeyParam.GroupCd, DetailKeyParam.ClassCd, DetailKeyParam.AssemblyPatternCd ); }
        }

        /// <summary>来歴一覧定義情報</summary>
        GridViewDefine[] _historyGridViewDefine = null;
        /// <summary>来歴一覧定義情報</summary>
        GridViewDefine[] HistoryGridViewDefine
        {
            get
            {
                if ( true == ObjectUtils.IsNull( _historyGridViewDefine ) ) {
                    _historyGridViewDefine = ControlUtils.GetGridViewDefineArray( typeof( HistoryGridColumnDefine ) );
                }
                return _historyGridViewDefine;
            }
        }

        /// <summary>測定結果一覧定義情報</summary>
        GridViewDefine[] _measuringResultGridViewDefine = null;
        /// <summary>測定結果一覧定義情報</summary>
        GridViewDefine[] MeasuringResultGridViewDefine
        {
            get
            {
                if ( true == ObjectUtils.IsNull( _measuringResultGridViewDefine ) ) {
                    _measuringResultGridViewDefine = ControlUtils.GetGridViewDefineArray( typeof( MeasuringResultGridFixColumnDefine ) );
                }
                return _measuringResultGridViewDefine;
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
            CurrentForm.RaiseEvent( DoPageLoad );
        }

        /// <summary>
        /// 来歴一覧データバインドイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvHistory_RowDataBound( object sender, GridViewRowEventArgs e ) {
            CurrentForm.RaiseEvent( DoHistoryGridRowDataBound, sender, e );
        }

        /// <summary>
        /// 来歴一覧選択行変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvHistory_SelectedIndexChanged( object sender, EventArgs e ) {
            CurrentForm.RaiseEvent( DoHistoryGridSelectedIndexChanged );
        }
        #endregion

        #region メソッド
        /// <summary>
        /// ページロード処理
        /// </summary>
        private void DoPageLoad() {
        }

        /// <summary>
        /// 来歴一覧データバインド処理
        /// </summary>
        /// <param name="parameters"></param>
        private void DoHistoryGridRowDataBound( params object[] parameters ) {
            var sender = (GridView)parameters[0];
            var e = (GridViewRowEventArgs)parameters[1];
            ControlUtils.GridViewRowBound( (GridView)sender, e );
            // 空文字でない場合、表示するヘッダテキストとして保持する
            var ht = HistoryGridViewDefine.Where( d => StringUtils.IsNotEmpty( d.headerText ) ).Select( d => d.headerText );
            for ( var cIndex = 0; cIndex < sender.Columns.Count; cIndex++ ) {
                if ( false == ht.Contains( sender.Columns[cIndex].HeaderText ) ) {
                    // 一覧のヘッダテキストが表示するヘッダテキストに含まれない場合、非表示を設定する
                    e.Row.Cells[cIndex].Style.Add( HtmlTextWriterStyle.Display, "none" );
                }
            }
            if ( DataControlRowType.Header != e.Row.RowType ) {
                // ヘッダ行でない場合、列クリックイベントに行選択ボタンの発火を設定する
                e.Row.Attributes[ControlUtils.ON_CLICK] = Page.ClientScript.GetPostBackClientHyperlink( (GridView)sender, "Select$" + e.Row.RowIndex );
            }
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize() {
            // 詳細情報を生成
            var detailInfo = new DetailViewBusiness.ResultSetGeneric();
            try {
                // 計測情報取得
                detailInfo = DetailViewBusiness.SelectProcessGenericDetail(
                    DetailKeyParam.ModelCd, DetailKeyParam.Serial, DetailKeyParam.LineCd, DetailKeyParam.ProcessCd );
            } catch ( DataAccessException ex ) {
                // メッセージ表示
                logger.Exception( ex );
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
            } catch ( Exception ex ) {
                // メッセージ表示
                logger.Exception( ex );
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
            }
            // セッションに情報を格納
            // 取得データをセッションに格納
            var dicPageControlInfo = new Dictionary<string, object>();
            dicPageControlInfo.Add( SESSION_PAGE_INFO_DB_KEY, detailInfo );
            CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).SetPageControlInfo( Defines.Session.DetailFrame.SESSION_PAGE_INFO_DETAIL_KEY, dicPageControlInfo );
            if ( null == detailInfo.HistoryInfo || 0 == detailInfo.HistoryInfo.Rows.Count ) {
                // 来歴情報が0件の場合、メッセージ表示
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62010, CurrentUCInfo.title );
                return;
            }
            // 初期値設定処理
            InitializeValues( detailInfo );
        }

        /// <summary>
        /// 初期値設定処理
        /// </summary>
        /// <param name="detailInfo">詳細情報</param>
        private void InitializeValues( DetailViewBusiness.ResultSetGeneric detailInfo ) {
            // 来歴一覧をクリア、非表示
            ControlUtils.InitializeGridView( grvHistory, false );
            grvHistory.Visible = false;
            // 計測結果一覧をクリア、非表示
            ControlUtils.InitializeGridView( grvMeasuringResult, false );
            grvMeasuringResult.Visible = false;
            if ( null != detailInfo.HistoryInfo && 0 < detailInfo.HistoryInfo.Rows.Count ) {
                // 来歴情報が存在する場合、来歴一覧に来歴情報をバインド
                ControlUtils.BindGridView_WithTempField( grvHistory, HistoryGridViewDefine, detailInfo.HistoryInfo );
                // 選択行インデックスを0に設定
                grvHistory.SelectedIndex = 0;
                // 来歴一覧を表示
                grvHistory.Visible = true;
                // 来歴一覧選択行変更処理
                DoHistoryGridSelectedIndexChanged();
            }
        }

        /// <summary>
        /// 来歴一覧選択行変更処理
        /// </summary>
        private void DoHistoryGridSelectedIndexChanged() {
            // 選択行インデックスを取得
            var index = grvHistory.SelectedIndex;
            if ( 0 > index ) {
                // 行が選択されていない場合、処理終了
                return;
            }
            foreach ( GridViewRow r in grvHistory.Rows ) {
                // 選択状態を解除する
                r.CssClass = grvHistory.RowStyle.CssClass;
            }
            // 選択行を選択状態にする
            var selectedRow = grvHistory.Rows[index];
            selectedRow.CssClass = grvHistory.SelectedRowStyle.CssClass;
            // 計測結果表示更新処理
            RefreshMeasuringResult( index );
        }

        /// <summary>
        /// 計測結果表示更新処理
        /// </summary>
        /// <param name="index">作業インデックス</param>
        private void RefreshMeasuringResult( int index ) {
            // セッションデータ取得
            var detailInfo = new Business.DetailViewBusiness.ResultSetGeneric();
            Dictionary<string, object> dicPageControlInfo = CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).GetPageControlInfo( Defines.Session.DetailFrame.SESSION_PAGE_INFO_DETAIL_KEY );
            if ( true == dicPageControlInfo.ContainsKey( SESSION_PAGE_INFO_DB_KEY ) ) {
                detailInfo = (Business.DetailViewBusiness.ResultSetGeneric)dicPageControlInfo[SESSION_PAGE_INFO_DB_KEY];
            }
            if ( null == detailInfo.MeasuringResultInfo[index] || 0 == detailInfo.MeasuringResultInfo[index].Rows.Count ) {
                // 計測結果情報が存在しない場合、処理終了
                return;
            }
            // ヘッダ情報
            var headerInfo = detailInfo.MeasuringItemInfo[index];
            // 計測結果情報
            var resultInfo = detailInfo.MeasuringResultInfo[index];
            // 列定義生成
            var columnDefine = new List<GridViewDefine>();
            // 表示用データテーブルの作成
            var dispDt = resultInfo.Clone();
            foreach ( DataRow colInfo in headerInfo.Rows ) {
                // ヘッダ情報からカラム情報を取り出す
                // ヘッダテキスト
                var headerText = StringUtils.ToString( colInfo["ATTRIBUTE_NM"] );
                // データタイプ
                var dataType = typeof( string );
                // フォーマット形式
                var formatParam = "";
                // 表示位置
                var align = HorizontalAlign.Left;
                // 単位
                var unit = StringUtils.ToString( colInfo["UNIT"] );
                // 列名
                var columnName = StringUtils.ToString( colInfo["ATTRIBUTE_CD"] );
                // データ型区分
                var typeKind = StringUtils.ToString( colInfo["DATA_TYP"] );
                if ( true == StringUtils.IsNotEmpty( unit ) ) {
                    // 単位が設定されている場合、ヘッダテキストに追加
                    headerText += $"({ unit })";
                }
                switch ( typeKind ) {
                case "1":
                    // 数値型の場合
                    dataType = typeof( decimal );
                    align = HorizontalAlign.Right;
                    // 小数部
                    var decimalPart = NumericUtils.ToInt( colInfo["DECIMAL_DGT"] );
                    // フォーマット文字列の設定
                    formatParam = ( 1 > decimalPart ) ? "0" : $"F{decimalPart}";
                    dispDt.Columns[columnName].DataType = typeof( decimal );
                    break;
                case "3":
                    // 日付型の場合
                    dataType = typeof( DateTime );
                    align = HorizontalAlign.Center;
                    formatParam = DateUtils.DATE_FORMAT_SECOND;
                    dispDt.Columns[columnName].DataType = typeof( DateTime );
                    break;
                }
                // フォーマット文字列
                var format = ( StringUtils.IsNotEmpty( formatParam ) ) ? $"{{0:{formatParam}}}" : "";
                // 列定義に追加
                columnDefine.Add( new GridViewDefine( headerText, columnName, dataType, format, true, columnName, align, 240, true, true ) );
            }
            foreach ( DataRow data in resultInfo.Rows ) {
                // 計測結果を表示用データテーブルに反映
                dispDt.ImportRow( data );
            }
            // 列定義に固定列を挿入
            columnDefine.InsertRange( 0, MeasuringResultGridViewDefine );
            // グリッドビューにバインド
            ControlUtils.BindGridView( grvMeasuringResult, columnDefine.ToArray(), dispDt );
            // グリッドビューを表示
            grvMeasuringResult.Visible = true;
        }
        #endregion
    }
}