using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
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
    public partial class TestBench : System.Web.UI.UserControl, Defines.Interface.IDetail {
        #region メンバ変数
        /// <summary>
        /// ログ出力機能
        /// </summary>
        private static readonly Logger _logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );
        #endregion

        #region 定数
        /// <summary>
        /// DB取得データ格納先 ディクショナリキー
        /// </summary>
        private const string SESSION_PAGE_INFO_DB_KEY = "bindTableData";
        /// <summary>
        /// 表示ファイル取得データ格納先 ディクショナリキー
        /// </summary>
        private const string SESSION_PAGE_INFO_DISP_KEY = "bindDispData";
        #region DB関連
        /// <summary>
        /// 列名：ファイル名
        /// </summary>
        private const string COL_NAME_FILE_NAME = "FILE_NAME";
        /// <summary>
        /// 列名：表示フラグ
        /// </summary>
        private const string COL_NAME_DISP_FLAG = "DISPLAY_FLAG";
        /// <summary>
        /// 列名：ファイル一意名
        /// </summary>
        private const string COL_NAME_UNIQUE_NAME = "UNIQUE_NAME";
        /// <summary>
        /// 列名：来歴番号
        /// </summary>
        private const string COL_NAME_HISTORY_NO = "HISTORY_NO";
        /// <summary>
        /// テーブル名：検査結果詳細
        /// </summary>
        private const string TABLE_NAME_INSP_DETAIL = "TT_SF_MAF_BENCH_INSP_DETAIL";
        #endregion
        #endregion

        /// <summary>
        /// 検査結果詳細定義
        /// </summary>
        internal class DetailGridColumnDefine {
            /// <summary>検査No</summary>
            public static readonly GridViewDefine TEST_NO = new GridViewDefine( "検査No", "TEST_NO", typeof( string ), "", false, HorizontalAlign.Right, 0, true );
            /// <summary>ベンチNo</summary>
            public static readonly GridViewDefine BENCH_NO = new GridViewDefine( "ベンチNo", "BENCH_NO", typeof( string ), "", false, HorizontalAlign.Right, 0, true );
            /// <summary>作業名</summary>
            public static readonly GridViewDefine HISTORY_NO = new GridViewDefine( "最終来歴", COL_NAME_HISTORY_NO, typeof( string ), "", false, HorizontalAlign.Right, 100, true );
            /// <summary>最終来歴</summary>
            public static readonly GridViewDefine FILE_NAME = new GridViewDefine( "ファイル名", COL_NAME_FILE_NAME, typeof( string ), "", false, HorizontalAlign.Right, 300, true );
            /// <summary>最終結果</summary>
            public static readonly GridViewDefine UNIQUE_NAME = new GridViewDefine( "", COL_NAME_UNIQUE_NAME, typeof( string ), "", false, HorizontalAlign.Center, 200, false );
        }

        #region プロパティ
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
        private PageInfo.ST_PAGE_INFO CurrentUCInfo { get { return PageInfo.GetUCPageInfo( DetailKeyParam.ProductKind, DetailKeyParam.GroupCd, DetailKeyParam.ClassCd, DetailKeyParam.AssemblyPatternCd ); } }

        /// <summary>検査結果詳細定義情報</summary>
        GridViewDefine[] _detailGridViewDefine = null;
        /// <summary>検査結果詳細定義情報</summary>
        GridViewDefine[] DetailGridViewDefine
        {
            get
            {
                if ( true == ObjectUtils.IsNull( _detailGridViewDefine ) ) {
                    _detailGridViewDefine = ControlUtils.GetGridViewDefineArray( typeof( DetailGridColumnDefine ) );
                }
                return _detailGridViewDefine;
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
        /// 検査結果詳細グリッドビューデータバインドイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvTestResultDetail_RowDataBound( object sender, GridViewRowEventArgs e ) {
            // 検査結果詳細グリッドビューの行数分呼び出されるためRaiseEventはしない
            try {
                if ( e.Row.RowType == DataControlRowType.DataRow ) {
                    // データ部の場合、行番号 + 1を検査NOとする
                    e.Row.Cells[0].Text = ( e.Row.RowIndex + 1 ).ToString();
                    e.Row.Cells[0].HorizontalAlign = DetailGridColumnDefine.TEST_NO.align;
                }
                ControlUtils.GridViewRowBound( (GridView)sender, e );
            } catch ( Exception ex ) {
                // 例外発生時ログのみ出力
                _logger.Exception( ex );
            }
        }

        /// <summary>
        /// 検査結果詳細グリッドビューボタン押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvTestResultDetail_RowCommand( object sender, GridViewCommandEventArgs e ) {
            if ( e.CommandName == "Download" ) {
                // ダウンロードボタンの場合ダウンロード処理実行
                CurrentForm.RaiseEvent( DownloadTestDetail, e );
                return;
            }
        }
        #endregion

        #region 機能別処理
        /// <summary>
        /// ページロード処理
        /// </summary>
        private void DoPageLoad() {
            var pageInfo = CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).GetPageControlInfo( Defines.Session.DetailFrame.SESSION_PAGE_INFO_DETAIL_KEY );
            if ( pageInfo.ContainsKey( SESSION_PAGE_INFO_DISP_KEY ) ) {
                SetDisplayData( (List<DisplayData>)( pageInfo[SESSION_PAGE_INFO_DISP_KEY] ) );
            }
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        void IDetail.Initialize() {
            // 詳細情報を生成
            var detailInfo = new DetailViewBusiness.ResultSetMulti();
            var displayData = new List<DisplayData>();
            try {
                // 計測情報取得
                detailInfo = DetailViewBusiness.SelectTractorTestBenchDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial );
                displayData = GetDisplayData( detailInfo.MainTable );
            } catch ( DataAccessException ex ) {
                // メッセージ表示
                _logger.Exception( ex );
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
            } catch ( Exception ex ) {
                // メッセージ表示
                _logger.Exception( ex );
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
            }
            // セッションに情報を格納
            // 取得データをセッションに格納
            var dicPageControlInfo = new Dictionary<string, object>();
            dicPageControlInfo.Add( SESSION_PAGE_INFO_DB_KEY, detailInfo );
            dicPageControlInfo.Add( SESSION_PAGE_INFO_DISP_KEY, displayData );
            CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).SetPageControlInfo( Defines.Session.DetailFrame.SESSION_PAGE_INFO_DETAIL_KEY, dicPageControlInfo );
            if ( ( detailInfo.MainTable == null ) || ( detailInfo.MainTable.Rows.Count == 0 ) || ( displayData.Count == 0 ) ) {
                // 来歴情報が0件の場合、メッセージ表示
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62010, CurrentUCInfo.title );
                return;
            }
            InitializeValues( detailInfo.MainTable, displayData );
        }

        /// <summary>
        /// 初期値設定
        /// </summary>
        private void InitializeValues( DataTable detailInfo, List<DisplayData> displayData ) {
            // 検査結果詳細をクリア、非表示
            ControlUtils.InitializeGridView( grvTestResultDetail, false );
            grvTestResultDetail.Visible = false;
            if ( null != detailInfo && 0 < detailInfo.Rows.Count ) {
                // 検査結果詳細情報が存在する場合、検査結果詳細一覧に検査結果詳細情報をバインド
                ControlUtils.BindGridView_WithTempField( grvTestResultDetail, DetailGridViewDefine, detailInfo );
                // 選択行インデックスを0に設定
                grvTestResultDetail.SelectedIndex = 0;
                // 検査結果詳細一覧を表示
                grvTestResultDetail.Visible = true;
            }
            // 検査結果を表示
            SetDisplayData( displayData );
        }

        /// <summary>
        /// 検査結果を表示
        /// </summary>
        /// <param name="dataList">表示データ一覧</param>
        private void SetDisplayData( List<DisplayData> dataList ) {
            // 検査結果表示箇所をクリア
            pnlTestResult.Controls.Clear();
            // 前回表示した大項目を初期化
            var prevItem = string.Empty;
            // 現在更新中のテーブルを初期化
            Table currTable = null;
            // 現在更新中の行を初期化
            TableRow currTableRow = null;
            // 表示データを順次展開
            foreach ( var data in dataList ) {
                if ( prevItem != data.Item ) {
                    // 大項目が前回と異なる場合
                    if ( currTable != null && currTableRow != null ) {
                        // 更新中テーブルと更新中行が存在する場合(中途半端に行が埋まっている状態)
                        // 更新中テーブルに更新中行を追加する
                        var emptySubItemCell = CreateTableCell( string.Empty );
                        emptySubItemCell.Attributes.Add( "class", "ui-state-default detailtable-header" );
                        currTableRow.Cells.Add( emptySubItemCell );
                        var emptyValueCell = CreateTableCell( string.Empty );
                        currTableRow.Cells.Add( emptyValueCell );
                        currTable.Rows.Add( currTableRow );
                        currTableRow = null;
                    }
                    // 大項目は■を付与して表示する
                    var item = new HtmlGenericControl( "div" );
                    item.Attributes.Add( "class", "div-detail-table-title" );
                    item.InnerText = "■" + data.Item;
                    pnlTestResult.Controls.Add( item );
                    // 更新中テーブルを作成する
                    currTable = new Table();
                    currTable.Attributes.Add( "class", "table-border-layout grid-layout" );
                    currTable.Style.Add( HtmlTextWriterStyle.Height, "auto" );
                    pnlTestResult.Controls.Add( currTable );
                    // 前回表示した大項目に現在の大項目を設定する
                    prevItem = data.Item;
                }
                if ( currTableRow == null ) {
                    // 更新中行が存在しない場合作成する
                    currTableRow = new TableRow();
                    currTableRow.Attributes.Add( "class", "listview-header" );
                }
                // 小項目を作成する
                var subItemCell = CreateTableCell( data.SubItem );
                subItemCell.Attributes.Add( "class", "ui-state-default detailtable-header" );
                currTableRow.Cells.Add( subItemCell );
                // 値を作成する
                var valueCell = CreateTableCell( data.Value + data.Unit );
                valueCell.HorizontalAlign = HorizontalAlign.Left;
                currTableRow.Cells.Add( valueCell );
                if ( currTableRow.Cells.Count == 4 ) {
                    // 更新中行の要素が4つ(小項目と値のセットが2つ)の場合テーブルに追加して初期化
                    currTable.Rows.Add( currTableRow );
                    currTableRow = null;
                }
            }
        }

        /// <summary>
        /// 検査結果セル作成
        /// </summary>
        /// <param name="text">表示文字列</param>
        /// <returns>検査結果セル</returns>
        private TableCell CreateTableCell( string text ) {
            // セルを初期化
            var ret = new TableCell();
            // 表示文字列を画面表示、ツールチップ表示に設定
            ret.Text = text;
            ret.ToolTip = text;
            // セル幅250px
            ret.Style.Add( HtmlTextWriterStyle.Width, "250px" );
            // はみ出した文字列を省略
            ret.Style.Add( HtmlTextWriterStyle.TextOverflow, "ellipsis" );
            ret.Style.Add( HtmlTextWriterStyle.WhiteSpace, "nowrap" );
            ret.Style.Add( HtmlTextWriterStyle.Overflow, "hidden" );
            ret.Style.Add( "max-width", "250px" );
            return ret;
        }

        /// <summary>
        /// 表示データ一覧作成
        /// </summary>
        /// <param name="result">検索結果</param>
        /// <returns>表示データ一覧</returns>
        private List<DisplayData> GetDisplayData( DataTable result ) {
            // 表示ファイル一意名
            var displayUniqueName = result.AsEnumerable()
                // 表示フラグ = 1
                .Where( dr => StringUtils.Nvl( dr[COL_NAME_DISP_FLAG] ) == "1" )
                // 来歴番号の降順に並び替え
                .OrderByDescending( dr => NumericUtils.ToInt( dr[COL_NAME_HISTORY_NO], 0 ) )
                // ファイルパスを取得
                .Select( dr => dr[COL_NAME_UNIQUE_NAME].ToString() )
                .FirstOrDefault();
            // 表示データ一覧を作成
            var byteData = new CoreService().GetLobData( TABLE_NAME_INSP_DETAIL, displayUniqueName );
            if ( byteData == null ) {
                // 表示データを取得できなかった場合
                return new List<DisplayData>();
            }
            var strData = Encoding.ShiftJIS.GetString( byteData );
            // 表示ファイルを終端まで読み込み、改行で区切った文字列リストを作成
            var ret = strData.Split( new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries )
                // 表示データクラスを作成
                .Select( line => new DisplayData( line ) )
                // リスト化
                .ToList();
            return ret;
        }

        /// <summary>
        /// 検査結果詳細のダウンロード
        /// </summary>
        /// <param name="parameters"></param>
        /// <remarks>対応しているファイルはCSV、JPGのみ</remarks>
        private void DownloadTestDetail( params object[] parameters ) {
            var e = (GridViewCommandEventArgs)parameters[0];
            var rownum = NumericUtils.ToInt( e.CommandArgument, -1 );
            if ( rownum == -1 ) {
                // 選択行が範囲外の場合終了
                return;
            }
            // ファイル名は例外発生時に表示するため先に宣言する
            string fileName = string.Empty;
            try {
                var pageInfo = CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).GetPageControlInfo( Defines.Session.DetailFrame.SESSION_PAGE_INFO_DETAIL_KEY );
                if ( pageInfo.ContainsKey( SESSION_PAGE_INFO_DISP_KEY ) ) {
                    // 検査結果詳細が取得できた場合
                    // 検査結果詳細を取得
                    var dataSource = ( (DetailViewBusiness.ResultSetMulti)pageInfo[SESSION_PAGE_INFO_DB_KEY] ).MainTable;
                    // 固有名、ファイル名を取得
                    var uniqueName = StringUtils.Nvl( dataSource.Rows[rownum][DetailGridColumnDefine.UNIQUE_NAME.bindField] );
                    fileName = StringUtils.Nvl( dataSource.Rows[rownum][DetailGridColumnDefine.FILE_NAME.bindField] );
                    // 出力のMIMEタイプを宣言
                    var contentType = string.Empty;
                    switch ( Path.GetExtension( fileName ).ToLower() ) {
                    case ".csv":
                        // 拡張子がcsvの場合
                        contentType = "text/csv";
                        break;
                    case ".jpg":
                    case ".jpeg":
                        // 拡張子がjpgの場合
                        contentType = "image/jpeg";
                        break;
                    default:
                        // それ以外はテキストファイルとする
                        contentType = "text/plain";
                        break;
                    }
                    // LOBデータを取得
                    var lobData = new CoreService().GetLobData( TABLE_NAME_INSP_DETAIL, uniqueName );
                    // ページに反映
                    Response.ClearContent();
                    Response.AddHeader( "Content-Disposition", string.Format( "attachment;filename={0}", Uri.EscapeUriString( fileName ) ) );
                    Response.ContentType = contentType;
                    Response.OutputStream.Write( lobData, 0, (int)lobData.Length );
                    Response.End();
                }
            } catch ( Exception ex ) {
                // 来歴情報が0件の場合、メッセージ表示
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_71010, fileName );
                _logger.Exception( ex );
            }
        }
        #endregion

        #region 表示データクラス
        /// <summary>
        /// 表示データクラス
        /// </summary>
        /// <remarks>
        /// 表示ファイルの1レコードの構成は以下の通り
        /// [Work|Common|Inspection],[大項目],[Header|Condition|Result],[小項目],[値],[単位]
        /// </remarks>
        private class DisplayData {
            /// <summary>
            /// 大項目種別
            /// </summary>
            public string ItemKind { get; set; }
            /// <summary>
            /// 大項目
            /// </summary>
            public string Item { get; set; }
            /// <summary>
            /// 小項目種別
            /// </summary>
            public string SubItemKind { get; set; }
            /// <summary>
            /// 小項目
            /// </summary>
            public string SubItem { get; set; }
            /// <summary>
            /// 値
            /// </summary>
            public string Value { get; set; }
            /// <summary>
            /// 単位
            /// </summary>
            public string Unit { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="data">表示データ文字列</param>
            public DisplayData( string data ) {
                try {
                    var dList = data.Split( new string[] { "," }, StringSplitOptions.None );
                    ItemKind = ( dList.Length > 0 ) ? dList[0] : string.Empty;
                    Item = ( dList.Length > 1 ) ? dList[1] : string.Empty;
                    SubItemKind = ( dList.Length > 2 ) ? dList[2] : string.Empty;
                    SubItem = ( dList.Length > 3 ) ? dList[3] : string.Empty;
                    Value = ( dList.Length > 4 ) ? dList[4] : string.Empty;
                    Unit = ( dList.Length > 5 ) ? dList[5] : string.Empty;
                } catch ( Exception ex ) {
                    _logger.Exception( ex );
                }
            }
        }
        #endregion
    }
}