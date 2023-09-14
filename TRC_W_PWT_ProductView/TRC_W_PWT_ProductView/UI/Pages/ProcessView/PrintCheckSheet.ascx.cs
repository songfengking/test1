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
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.UI.Base;

namespace TRC_W_PWT_ProductView.UI.Pages {
    public partial class PrintCheckSheet : System.Web.UI.UserControl, Defines.Interface.IDetail {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数
        /// <summary>
        /// 管理ID
        /// </summary>
        const string MANAGE_ID = "PrintCheckSheet";  //CurrentUCInfo.pageId
        const string MANAGE_ID_01 = "PrintCheckSheet01";  //CurrentUCInfo.pageId
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
        /// 表示中ページ情報
        /// </summary>
        private PageInfo.ST_PAGE_INFO CurrentPageInfo
        {
            get
            {
                return CurrentForm.CurrentPageInfo;
            }
        }
        #endregion

        #region
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

        /// <summary>
        /// サムネイル(リスト内イメージ)最大幅
        /// </summary>
        const int LIST_MAX_WIDTH = 200;
        /// <summary>
        /// サムネイル(リスト内イメージ)最大高さ
        /// </summary>
        const int LIST_MAX_HEIGHT = 282;

        /// <summary>
        /// 画像リストテンプレート定義
        /// </summary>
        public class LIST_TEMPLATE_CONTROL {
            /// <summary>DIV</summary>
            public static readonly ControlDefine DIV_ROW_DATA = new ControlDefine( "divRowData", "DIV", "", ControlDefine.BindType.None, typeof( Byte[] ) );
            /// <summary>画像ファイル</summary>
            public static readonly ControlDefine CHECK_SHEET = new ControlDefine( "imgCheckSheet", "画像ファイル", "image", ControlDefine.BindType.None, typeof( Byte[] ) );
            /// <summary>チェックボックス </summary>
            public static readonly ControlDefine CHECK_PRIN = new ControlDefine( "divChkPrint", "チェックボックス", "", ControlDefine.BindType.None, typeof( bool ) );
            /// <summary>印刷ファイル</summary>
            public static readonly ControlDefine PRINT_SHEET = new ControlDefine( "imgCheckSheet", "印刷ファイル", "image", ControlDefine.BindType.None, typeof( Byte[] ) );
        }

        #endregion

        #region スクリプトイベント
        /// <summary>
        /// 詳細画面切替イベント(サムネイル画像クリックでメインエリアに通常サイズ表示を行う)
        /// </summary>
        /// <remarks>パラメータ 差し替え先コントロール URL</remarks>
        const string CHANGE_MAIN_AREA_VIEW = "PrintCheckSheet.ChangeMainAreaImage('{0}','{1}');";

        /// <summary>
        /// サムネイル項目 項目選択済用CSS
        /// </summary>
        const string LIST_SELECTED_ITEM_CSS = "div-item-selected";

        #endregion


        private string localToken = "";


        protected void Page_Load( object sender, EventArgs e ) {
        }

        /// <summary>
        /// ページロード処理
        /// </summary>
        private void DoPageLoad() {
            Initialize();

        }
        /// <summary>
        /// イメージリスト行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstCheckSheetList_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundCheckSheetList( sender, e );
        }
        #region ページイベント

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize() {

            //画面表示用にリクエストparamからキー値を取得
            localToken = StringUtils.ToString( Request.QueryString["Token"] );
            string modelCd = StringUtils.ToString( Request.QueryString["modelCd"] );
            string serial = StringUtils.ToString( Request.QueryString["serial"] );


            //検索結果取得
            DataTable tblCheckSheet = null;
            try {
                string dispURL = StringUtils.ToString( Request.QueryString["countryCd"] );
                if ( PageInfo.ShipmentParts.pageId.Equals( dispURL ) ) {
                    // 出荷部品の場合、出荷部品のサブ情報（梱包作業指示書取得）を行う
                    tblCheckSheet = Business.DetailViewBusiness.SelectEngineShipmentPartsDetail( modelCd, serial ).SubTable;
                } else {
                    tblCheckSheet = Business.DetailViewBusiness.SelectTractorCheckSheetDetail( modelCd, serial ).MainTable;
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

            if ( 0 == tblCheckSheet.Rows.Count ) {
                //メインイメージ非表示
                imgMainArea.Visible = false;
                return;
            }


            InitializeValues( tblCheckSheet );

            InitializeValues2( tblCheckSheet );
        }

        /// <summary>
        /// 初期値設定
        /// </summary>
        private void InitializeValues( DataTable tblCheckSheet ) {
            TemplateControl tempCtrl = lstCheckSheetList.TemplateControl;
            lstCheckSheetList.DataSource = tblCheckSheet;
            lstCheckSheetList.DataBind();

            Dictionary<string, byte[]> dicImages = new Dictionary<string, byte[]>();
            for ( int loopImg = 0; loopImg < tblCheckSheet.Rows.Count; loopImg++ ) {
                byte[] byteImage = new byte[0];

                if ( ObjectUtils.IsNotNull( tblCheckSheet.Rows[loopImg][LIST_TEMPLATE_CONTROL.CHECK_SHEET.bindField] ) ) {
                    byteImage = (byte[])tblCheckSheet.Rows[loopImg][LIST_TEMPLATE_CONTROL.CHECK_SHEET.bindField];
                }

                dicImages.Add( loopImg.ToString(), byteImage );
            }

            CurrentForm.SessionManager.GetImageInfoHandler( localToken ).SetImages( MANAGE_ID, dicImages );
        }

        #endregion

        #region リストバインド
        /// <summary>
        /// イメージリストバインド処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
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
                    string urlMainAreaTop = ImageView.GetImageViewUrl( this, localToken, MANAGE_ID, e.Item.DataItemIndex, 0, 0
                        , TRC_W_PWT_ProductView.Session.ImageInfoSessionHandler.ContentType.TIFF, false );
                    imgMainArea.ImageUrl = urlMainAreaTop;

                    //サムネイルエリア 先頭行
                    string urlThumbnail = ImageView.GetImageViewUrl( this, localToken, MANAGE_ID, e.Item.DataItemIndex, LIST_MAX_WIDTH, LIST_MAX_HEIGHT
                    , TRC_W_PWT_ProductView.Session.ImageInfoSessionHandler.ContentType.Bitmap, false );
                    imgThumbnail.ImageUrl = urlThumbnail;
                } else {
                    //先頭行以外はページロード後に画像読み込み
                    string urlThumbnail = ImageView.GetImageViewUrl( this, localToken, MANAGE_ID, e.Item.DataItemIndex, LIST_MAX_WIDTH, LIST_MAX_HEIGHT
                        , TRC_W_PWT_ProductView.Session.ImageInfoSessionHandler.ContentType.Bitmap, true );
                    imgThumbnail.ImageUrl = ResourcePath.Image.DummyLoad;
                    imgThumbnail.Attributes[ResourcePath.Image.AttrOriginalSrc] = urlThumbnail;
                }

                //クリックイベントセット
                string urlMainArea = ImageView.GetImageViewUrl( this, localToken, MANAGE_ID, e.Item.DataItemIndex, 0, 0
                    , TRC_W_PWT_ProductView.Session.ImageInfoSessionHandler.ContentType.TIFF, true );
                imgThumbnail.Attributes[ControlUtils.ON_CLICK] = String.Format( CHANGE_MAIN_AREA_VIEW, imgMainArea.ClientID, urlMainArea );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );

            }
        }
        #endregion

        protected void lstPrintList_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundPrintList( sender, e );
        }
        /// <summary>
        /// 初期値設定
        /// </summary>
        private void InitializeValues2( DataTable tblCheckSheet ) {
            TemplateControl tempCtrl = lstPrintList.TemplateControl;
            lstPrintList.DataSource = tblCheckSheet;
            lstPrintList.DataBind();

            Dictionary<string, byte[]> dicImages = new Dictionary<string, byte[]>();
            for ( int loopImg = 0; loopImg < tblCheckSheet.Rows.Count; loopImg++ ) {
                byte[] byteImage = new byte[0];

                if ( ObjectUtils.IsNotNull( tblCheckSheet.Rows[loopImg][LIST_TEMPLATE_CONTROL.CHECK_SHEET.bindField] ) ) {
                    byteImage = (byte[])tblCheckSheet.Rows[loopImg][LIST_TEMPLATE_CONTROL.CHECK_SHEET.bindField];
                }

                dicImages.Add( loopImg.ToString(), byteImage );
            }

            CurrentForm.SessionManager.GetImageInfoHandler( localToken ).SetImages( MANAGE_ID_01, dicImages );
        }
        private void ItemDataBoundPrintList( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {

                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Item, ListTempControls, rowBind, ref dicSetValues );

                //イメージ画像URLセット
                Image imgPrint = ( (Image)e.Item.FindControl( LIST_TEMPLATE_CONTROL.PRINT_SHEET.controlId ) );

                //サムネイルエリア 先頭行
                string urlMainArea = ImageView.GetImageViewUrl( this, localToken, MANAGE_ID_01, e.Item.DataItemIndex, 0, 0
                    , TRC_W_PWT_ProductView.Session.ImageInfoSessionHandler.ContentType.TIFF, false );
                imgPrint.ImageUrl = urlMainArea;

            }
        }
    }
}