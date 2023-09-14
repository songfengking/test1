using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KTFramework.Common;
using KTFramework.Dao;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.Interface;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.UI.Base;
using static TRC_W_PWT_ProductView.Business.DetailViewBusiness;

namespace TRC_W_PWT_ProductView.UI.Pages.ProcessView {
    /// <summary>
    /// (詳細 エンジン 工程)出荷部品
    /// </summary>
    public partial class ShipmentParts : System.Web.UI.UserControl, Defines.Interface.IDetail {
        #region 内部クラス
        /// <summary>
        /// 出荷部品リスト テンプレート定義
        /// </summary>
        internal class ShipmentPartsGridColumnDefine {
            /// <summary>出荷部品</summary>
            public static readonly GridViewDefine WORK_NM = new GridViewDefine( "出荷部品", "PARTS_NUM", typeof( string ), "", false, HorizontalAlign.Left, 200, true );
        }

        /// <summary>
        /// 画像リストテンプレート定義
        /// </summary>
        public class LIST_TEMPLATE_CONTROL {
            /// <summary>DIV</summary>
            public static readonly ControlDefine DIV_ROW_DATA = new ControlDefine( "divRowData", "DIV", "", ControlDefine.BindType.None, typeof( Byte[] ) );
            /// <summary>画像ファイル</summary>
            public static readonly ControlDefine CHECK_SHEET = new ControlDefine( "imgOrderSheet", "画像ファイル", "IMAGE", ControlDefine.BindType.None, typeof( Byte[] ) );
        }
        #endregion

        #region 定数
        /// <summary>
        /// 管理ID
        /// </summary>
        private const string MANAGE_ID = Defines.Session.DetailFrame.SESSION_PAGE_INFO_DETAIL_KEY;
        /// <summary>
        /// DB取得データ格納先 ディクショナリキー
        /// </summary>
        private const string SESSION_PAGE_INFO_DB_KEY = "bindTableData";
        /// <summary>
        /// サムネイル(リスト内イメージ)最大幅
        /// </summary>
        private const int LIST_MAX_WIDTH = 200;
        /// <summary>
        /// サムネイル(リスト内イメージ)最大高さ
        /// </summary>
        private const int LIST_MAX_HEIGHT = 282;
        /// <summary>
        /// サムネイル項目 項目選択済用CSS
        /// </summary>
        const string LIST_SELECTED_ITEM_CSS = "div-item-selected";
        /// <summary>
        /// 詳細画面切替イベント(サムネイル画像クリックでメインエリアに通常サイズ表示を行う)
        /// </summary>
        /// <remarks>パラメータ 差し替え先コントロール URL</remarks>
        const string CHANGE_MAIN_AREA_VIEW = "ShipmentParts.ChangeMainAreaImage('{0}','{1}');";
        /// <summary>
        /// 印刷画面起動用
        /// </summary>
        /// <remarks>パラメータ URL トークン 型式コード 遷移元画面ID 機番</remarks>
        const string TRANSFER_PRINT_CLICK = "return ControlCommon.WindowOpenChangeDetail('{0}','{1}','{2}','{3}','{4}');";

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

        /// <summary>
        /// 出荷部品一覧定義情報
        /// </summary>
        GridViewDefine[] _shipmentPartsGridViewDefine = null;
        /// <summary>
        /// 出荷部品一覧定義情報
        /// </summary>
        GridViewDefine[] ShipmentPartsGridViewDefine
        {
            get
            {
                if ( true == ObjectUtils.IsNull( _shipmentPartsGridViewDefine ) ) {
                    _shipmentPartsGridViewDefine = ControlUtils.GetGridViewDefineArray( typeof( ShipmentPartsGridColumnDefine ) );
                }
                return _shipmentPartsGridViewDefine;
            }
        }

        /// <summary>
        /// 画像リストテンプレート定義情報
        /// </summary>
        ControlDefine[] _listTempControls = null;
        /// <summary>
        /// 画像リストテンプレート定義情報アクセサ
        /// </summary>
        ControlDefine[] ListTempControls
        {
            get
            {
                if ( true == ObjectUtils.IsNull( _listTempControls ) ) {
                    _listTempControls = ControlUtils.GetControlDefineArray( typeof( LIST_TEMPLATE_CONTROL ) );
                }
                return _listTempControls;
            }
        }
        #endregion

        #region メンバ変数
        /// <summary>
        /// ロガー定義
        /// </summary>
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );
        #endregion

        #region イベント
        protected void Page_Load( object sender, EventArgs e ) {

        }

        /// <summary>
        /// 出荷部品リスト行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvShipmentParts_RowDataBound( object sender, GridViewRowEventArgs e ) {
            CurrentForm.RaiseEvent( DoShipmemntPartsGridRowDataBound, sender, e );
        }

        /// <summary>
        /// イメージリスト行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstOrderSheetList_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundCheckSheetList( sender, e );
        }
        #endregion

        #region 処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize() {
            ResultSet result;
            try {
                // 出荷部品検索
                result = Business.DetailViewBusiness.SelectEngineShipmentPartsDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial );
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
            // セッションに情報を格納
            // 取得データをセッションに格納
            var dicPageControlInfo = new Dictionary<string, object>();
            dicPageControlInfo.Add( SESSION_PAGE_INFO_DB_KEY, result );
            CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).SetPageControlInfo( Defines.Session.DetailFrame.SESSION_PAGE_INFO_DETAIL_KEY, dicPageControlInfo );
            // 初期値設定処理
            InitializeValues( result );
        }

        /// <summary>
        /// 初期値設定処理
        /// </summary>
        /// <param name="data">詳細情報</param>
        private void InitializeValues( ResultSet data ) {
            // 出荷部品一覧をクリア、非表示
            ControlUtils.InitializeGridView( grvShipmentParts, false );
            grvShipmentParts.Visible = false;
            if ( null != data.MainTable && 0 < data.MainTable.Rows.Count ) {
                // 出荷部品情報が存在する場合、出荷部品一覧に出荷部品情報をバインド
                ControlUtils.BindGridView_WithTempField( grvShipmentParts, ShipmentPartsGridViewDefine, data.MainTable );
                // 選択行インデックスを0に設定
                grvShipmentParts.SelectedIndex = 0;
                // 出荷部品一覧を表示
                grvShipmentParts.Visible = true;
            }
            if ( data.SubTable == null || data.SubTable.Rows.Count == 0 ) {
                // 梱包作業指示書情報が存在しない場合
                // 梱包作業日時が無い旨を表示
                txtPackingOrderTime.Value = "梱包作業指示無し";
                // メインイメージ非表示
                imgMainArea.Visible = false;
                btnPrint.Visible = false;
            } else {
                // 梱包作業指示書情報が存在する場合
                // 梱包作業日時を表示
                var tmpDateStr = DateUtils.ToString( data.SubTable.Rows[0][0], DateUtils.FormatType.Second );
                txtPackingOrderTime.Value = ( StringUtils.IsNotBlank( tmpDateStr ) ) ? tmpDateStr : "梱包実績日時無し";
                if ( data.SubTable.AsEnumerable().Select( dr => dr[LIST_TEMPLATE_CONTROL.CHECK_SHEET.bindField] ).Any( image => image != DBNull.Value ) ) {
                    // 画像リストのデータを設定
                    lstOrderSheetList.DataSource = data.SubTable;
                    lstOrderSheetList.DataBind();
                    // セッションに格納するデータを作成
                    Dictionary<string, byte[]> dicImages = new Dictionary<string, byte[]>();
                    for ( int loopImg = 0; loopImg < data.SubTable.Rows.Count; loopImg++ ) {
                        byte[] byteImage = new byte[0];
                        if ( ObjectUtils.IsNotNull( data.SubTable.Rows[loopImg][LIST_TEMPLATE_CONTROL.CHECK_SHEET.bindField] ) ) {
                            // データが存在する場合バイト列に変換
                            byteImage = (byte[])data.SubTable.Rows[loopImg][LIST_TEMPLATE_CONTROL.CHECK_SHEET.bindField];
                        }
                        dicImages.Add( loopImg.ToString(), byteImage );
                    }
                    // セッションに格納
                    CurrentForm.SessionManager.GetImageInfoHandler( CurrentForm.Token ).SetImages( MANAGE_ID, dicImages );
                    // 印刷ページ用設定
                    // 暫定的に国コードに遷移元画面IDを設定
                    btnPrint.Attributes[ControlUtils.ON_CLICK]
                        = String.Format( TRANSFER_PRINT_CLICK
                        , PageInfo.ResolveClientUrl( this, PageInfo.PreViewForm )
                        , CurrentForm.Token
                        , DetailKeyParam.ProductModelCd
                        , PageInfo.ShipmentParts.pageId
                        , DetailKeyParam.Serial
                    );
                } else {
                    // メインイメージ非表示
                    imgMainArea.Visible = false;
                    btnPrint.Visible = false;
                }
            }
        }

        /// <summary>
        /// 出荷部品リスト行バインド処理
        /// </summary>
        /// <param name="parameters"></param>
        private void DoShipmemntPartsGridRowDataBound( params object[] parameters ) {
            var sender = (GridView)parameters[0];
            var e = (GridViewRowEventArgs)parameters[1];
            ControlUtils.GridViewRowBound( (GridView)sender, e );
            // 空文字でない場合、表示するヘッダテキストとして保持する
            var ht = ShipmentPartsGridViewDefine.Where( d => StringUtils.IsNotEmpty( d.headerText ) ).Select( d => d.headerText );
            for ( var cIndex = 0; cIndex < sender.Columns.Count; cIndex++ ) {
                if ( false == ht.Contains( sender.Columns[cIndex].HeaderText ) ) {
                    // 一覧のヘッダテキストが表示するヘッダテキストに含まれない場合、非表示を設定する
                    e.Row.Cells[cIndex].Style.Add( HtmlTextWriterStyle.Display, "none" );
                }
            }
        }

        /// <summary>
        /// 画像リスト行バインド処理
        /// </summary>
        /// <param name="parameters"></param>
        private void ItemDataBoundCheckSheetList( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];
            if ( e.Item.ItemType == ListViewItemType.DataItem ) {
                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Item, ListTempControls, rowBind, ref dicSetValues );
                //イメージ画像URLセット(サムネイル)
                Image imgThumbnail = ( (Image)e.Item.FindControl( LIST_TEMPLATE_CONTROL.CHECK_SHEET.controlId ) );
                //先頭行選択済
                if ( 0 == e.Item.DataItemIndex ) {
                    //メイン画像エリア
                    HtmlGenericControl divCtrl = ( (HtmlGenericControl)e.Item.FindControl( LIST_TEMPLATE_CONTROL.DIV_ROW_DATA.controlId ) );
                    divCtrl.Attributes["class"] = divCtrl.Attributes["class"] + " " + LIST_SELECTED_ITEM_CSS;
                    string urlMainAreaTop = ImageView.GetImageViewUrl( this, CurrentForm.Token, MANAGE_ID, e.Item.DataItemIndex, 0, 0
                        , TRC_W_PWT_ProductView.Session.ImageInfoSessionHandler.ContentType.TIFF, false );
                    imgMainArea.ImageUrl = urlMainAreaTop;
                    //サムネイルエリア 先頭行
                    string urlThumbnail = ImageView.GetImageViewUrl( this, CurrentForm.Token, MANAGE_ID, e.Item.DataItemIndex, LIST_MAX_WIDTH, LIST_MAX_HEIGHT
                    , TRC_W_PWT_ProductView.Session.ImageInfoSessionHandler.ContentType.Bitmap, false );
                    imgThumbnail.ImageUrl = urlThumbnail;
                } else {
                    //先頭行以外はページロード後に画像読み込み
                    string urlThumbnail = ImageView.GetImageViewUrl( this, CurrentForm.Token, MANAGE_ID, e.Item.DataItemIndex, LIST_MAX_WIDTH, LIST_MAX_HEIGHT
                        , TRC_W_PWT_ProductView.Session.ImageInfoSessionHandler.ContentType.Bitmap, true );
                    imgThumbnail.ImageUrl = ResourcePath.Image.DummyLoad;
                    imgThumbnail.Attributes[ResourcePath.Image.AttrOriginalSrc] = urlThumbnail;
                }
                //クリックイベントセット
                string urlMainArea = ImageView.GetImageViewUrl( this, CurrentForm.Token, MANAGE_ID, e.Item.DataItemIndex, 0, 0
                    , TRC_W_PWT_ProductView.Session.ImageInfoSessionHandler.ContentType.TIFF, true );
                imgThumbnail.Attributes[ControlUtils.ON_CLICK] = String.Format( CHANGE_MAIN_AREA_VIEW, imgMainArea.ClientID, urlMainArea );
                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );
            }
        }
        #endregion
    }
}