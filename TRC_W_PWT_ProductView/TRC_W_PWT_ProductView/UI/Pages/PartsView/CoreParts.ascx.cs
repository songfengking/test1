using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using KTFramework.Common;
using KTFramework.Dao;
using KTWebControl.CustomControls;
using KTWebInheritance.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.ListDefine;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.UI.Base;

namespace TRC_W_PWT_ProductView.UI.Pages.PartsView {
    /// <summary>
    /// エンジン部品詳細画面:基幹部品
    /// </summary>
    public partial class CoreParts : System.Web.UI.UserControl, Defines.Interface.IDetail {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義
        /// <summary>
        /// 部品組付情報
        /// </summary>
        public class GRID_MAIN {
            /// <summary>組付日時(yyyy/MM/dd hh:mm:ss)</summary>
            public static readonly ControlDefine INSTALL_DT = new ControlDefine( "txtInstallDt", "組付日時", "installDt", ControlDefine.BindType.None, typeof( DateTime ) );
            /// <summary>ステーション名</summary>
            public static readonly ControlDefine STATION_NM = new ControlDefine( "txtStationNm", "ステーション名", "stationNm", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>来歴NO</summary>
            public static readonly ControlDefine HISTORY_INDEX = new ControlDefine( "txtHistoryIndex", "来歴No", "historyIndex", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>部品区分名</summary>
            public static readonly ControlDefine PARTS_TYPE_NM = new ControlDefine( "txtPartsTypeNm", "部品区分名", "partsTypeNm", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>品番</summary>
            public static readonly ControlDefine PARTS_NUM = new ControlDefine( "txtPartsNum", "品番", "partsNum", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>部品機番</summary>
            public static readonly ControlDefine PARTS_SERIAL = new ControlDefine( "txtPartsSerial", "部品機番", "partsSerial", ControlDefine.BindType.Down, typeof( String ) );
        }

        /// <summary>
        /// 詳細情報
        /// </summary>
        public class GRID_SUB {
            //詳細なし
        }
        #endregion

        #region プロパティ
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
        /// 表示中ユーザコントロール情報
        /// </summary>
        private PageInfo.ST_PAGE_INFO CurrentUCInfo
        {
            get
            {
                return PageInfo.GetUCPageInfo( DetailKeyParam.ProductKind, DetailKeyParam.GroupCd, DetailKeyParam.ClassCd );
            }
        }

        /// <summary>
        /// コントロール定義
        /// </summary>
        ControlDefine[] _criticalPartsControls = null;
        /// <summary>
        /// コントロール定義アクセサ
        /// </summary>
        ControlDefine[] CriticalPartsControls
        {
            get
            {
                if ( true == ObjectUtils.IsNull( _criticalPartsControls ) ) {
                    _criticalPartsControls = ControlUtils.GetControlDefineArray( typeof( GRID_MAIN ) );
                }
                return _criticalPartsControls;
            }
        }

        /// <summary>
        /// 詳細表示キー情報
        /// </summary>
        private Defines.Interface.ST_DETAIL_PARAM _detailKeyParam;

        /// <summary>
        /// 詳細表示キー情報
        /// </summary>
        public Defines.Interface.ST_DETAIL_PARAM DetailKeyParam
        {
            get
            {
                return _detailKeyParam;
            }
            set
            {
                _detailKeyParam = value;
            }
        }
        #endregion

        #region イベント

        /// <summary>
        /// ページロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load( object sender, EventArgs e ) {
            CurrentForm.RaiseEvent( DoPageLoad );
        }

        #endregion

        #region イベントメソッド

        #region ページイベント

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize() {

            //検索結果取得
            DataTable tblMain = null;
            try {
                if ( DetailKeyParam.ProductKind == ProductKind.Engine ) {
                    // エンジン
                    tblMain = Business.DetailViewBusiness.SelectEngineCorePartsDetail( DetailKeyParam.ClassCd, DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                } else if ( DetailKeyParam.ProductKind == ProductKind.Tractor ) {
                    // トラクタ
                    tblMain = Business.DetailViewBusiness.SelectTractorCorePartsDetail( DetailKeyParam.ClassCd, DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                }
            } catch ( DataAccessException ex ) {
                logger.Exception( ex );
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                return;
            } catch ( Exception ex ) {
                logger.Exception( ex );
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                return;
            } finally {
            }

            if ( 0 == tblMain.Rows.Count ) {
                //検索結果0件
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62010, CurrentUCInfo.title );
                return;
            }

            InitializeValues( tblMain );
        }

        /// <summary>
        /// ページロード処理
        /// </summary>
        private void DoPageLoad() {
        }

        #endregion

        /// <summary>
        /// 初期値設定
        /// </summary>
        private void InitializeValues( DataTable tblMainData ) {
            string prevStationNm = "";
            string prevPartsType = "";
            string prevModelCd = "";
            string prevSerial = "";
            Table mainTablel = null;
            TableRow insRow = null;

            foreach ( DataRow row in tblMainData.Rows ) {
                if ( prevStationNm == StringUtils.ToString( row["stationNm"] ) &&
                    prevPartsType == StringUtils.ToString( row["partsType"] ) &&
                    prevModelCd == StringUtils.ToString( row["productModelCd"] ) &&
                    prevSerial == StringUtils.ToString( row["serial6"] ) ) {
                    // 前回とステーション、部品区分、型式コード、機番が同じなら表示しない
                    continue;
                }

                // テーブル追加
                mainTablel = new Table();
                mainTablel.Attributes.Add( "class", "table-border-layout grid-layout" );
                mainTablel.Style.Add( HtmlTextWriterStyle.Height, "auto" );
                pnlInstallInfo.Controls.Add( mainTablel );

                // テーブルの間隔をあける
                HtmlGenericControl item = new HtmlGenericControl( "div" );
                item.Attributes.Add( "class", "div-detail-table-title" );
                pnlInstallInfo.Controls.Add( item );

                // テーブルにデータを設定
                // 1行目
                insRow = new TableRow();
                insRow.Attributes.Add( "class", "listview-header" );
                insRow.Cells.Add( CreateTableCell( GRID_MAIN.INSTALL_DT.displayNm, true ) );
                insRow.Cells.Add( CreateTableCell( StringUtils.ToString( row["installDt"], "yyyy/MM/dd HH:mm:ss" ) ) );
                insRow.Cells.Add( CreateTableCell( GRID_MAIN.STATION_NM.displayNm, true ) );
                insRow.Cells.Add( CreateTableCell( StringUtils.ToString( row["stationNm"] ) ) );
                mainTablel.Rows.Add( insRow );
                // 2行目
                insRow = new TableRow();
                insRow.Attributes.Add( "class", "listview-header" );
                insRow.Cells.Add( CreateTableCell( GRID_MAIN.HISTORY_INDEX.displayNm, true ) );
                insRow.Cells.Add( CreateTableCell( StringUtils.ToString( row["historyIndex"] ) ) );
                insRow.Cells.Add( CreateTableCell( GRID_MAIN.PARTS_TYPE_NM.displayNm, true ) );
                insRow.Cells.Add( CreateTableCell( StringUtils.ToString( row["partsTypeNm"] ) ) );
                mainTablel.Rows.Add( insRow );
                // 3行目
                insRow = new TableRow();
                insRow.Attributes.Add( "class", "listview-header" );
                insRow.Cells.Add( CreateTableCell( GRID_MAIN.PARTS_NUM.displayNm, true ) );
                insRow.Cells.Add( CreateTableCell( StringUtils.ToString( row["partsNum"] ) ) );
                insRow.Cells.Add( CreateTableCell( GRID_MAIN.PARTS_SERIAL.displayNm, true ) );
                insRow.Cells.Add( CreateTableCell( StringUtils.ToString( row["partsSerial"] ) ) );
                mainTablel.Rows.Add( insRow );

                // ステーション、部品区分、型式コード、機番を保持
                prevStationNm = StringUtils.ToString( row["stationNm"] );
                prevPartsType = StringUtils.ToString( row["partsType"] );
                prevModelCd = StringUtils.ToString( row["productModelCd"] );
                prevSerial = StringUtils.ToString( row["serial6"] );
            }
        }

        /// <summary>
        /// 検査結果セル作成
        /// </summary>
        /// <param name="text">表示文字列</param>
        /// <param name="isItem">項目名を判別するフラグ</param>
        /// <returns>検査結果セル</returns>
        private TableCell CreateTableCell( string text, bool isItem = false ) {
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

            if ( true == isItem ) {
                // 項目名
                ret.Attributes.Add( "class", "ui-state-default detailtable-header" );
            }

            return ret;
        }

        #endregion

    }
}