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

namespace TRC_W_PWT_ProductView.UI.Pages.ProcessView {
    /// <summary>
    /// (詳細 エンジン 工程) 画像証跡
    /// </summary>
    public partial class CheckSheet : System.Web.UI.UserControl, Defines.Interface.IDetail {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義

        /// <summary>
        /// 管理ID
        /// </summary>
        const string MANAGE_ID = Defines.Session.DetailFrame.SESSION_PAGE_INFO_DETAIL_KEY;//CurrentUCInfo.pageId

        #region スクリプトイベント
        /// <summary>
        /// 詳細画面切替イベント(サムネイル画像クリックでメインエリアに通常サイズ表示を行う)
        /// </summary>
        /// <remarks>パラメータ 差し替え先コントロール URL</remarks>
        const string CHANGE_MAIN_AREA_VIEW = "CheckSheet.ChangeMainAreaImage('{0}','{1}');";

        /// <summary>
        /// 印刷画面起動用
        /// </summary>
        /// <remarks>パラメータ URL トークン 型式コード 遷移元画面ID 機番</remarks>
        const string TRANSFER_PRINT_CLICK = "return ControlCommon.WindowOpenChangeDetail('{0}','{1}','{2}','{3}','{4}');";

        #endregion

        #region CSS

        /// <summary>
        /// サムネイル項目 項目選択済用CSS
        /// </summary>
        const string LIST_SELECTED_ITEM_CSS = "div-item-selected";

        #endregion

        /// <summary>
        /// サムネイル(リスト内イメージ)最大幅
        /// </summary>
        const int LIST_MAX_WIDTH = 200;
        /// <summary>
        /// サムネイル(リスト内イメージ)最大高さ
        /// </summary>
        const int LIST_MAX_HEIGHT = 282;
        
        /// <summary>
        /// 画像証跡 画像リストテンプレート定義
        /// </summary>
        public class LIST_TEMPLATE_CONTROL {
            /// <summary>DIV</summary>
            public static readonly ControlDefine DIV_ROW_DATA = new ControlDefine( "divRowData", "DIV", "", ControlDefine.BindType.None, typeof( Byte[] ) );
            /// <summary>画像ファイル</summary>
            public static readonly ControlDefine CHECK_SHEET = new ControlDefine( "imgCheckSheet", "画像ファイル", "image", ControlDefine.BindType.None, typeof( Byte[] ) );
        }

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
        public Defines.Interface.ST_DETAIL_PARAM DetailKeyParam {
            get {
                return _detailKeyParam;
            }
            set {
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
        ControlDefine[] ListTempControls {
            get {
                if ( true == ObjectUtils.IsNull( _listTempControls ) ) {
                    _listTempControls = ControlUtils.GetControlDefineArray( typeof( LIST_TEMPLATE_CONTROL ) );
                }
                return _listTempControls;
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

        /// <summary>
        /// イメージリスト行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstCheckSheetList_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundCheckSheetList( sender, e );
        }

        #endregion

        #region イベントメソッド

        #region ページイベント

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize() {

            //検索結果取得
            DataTable tblCheckSheet = null;
            try {
                tblCheckSheet = Business.DetailViewBusiness.SelectTractorCheckSheetDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
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
                //検索結果0件
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62010, CurrentUCInfo.title );
                //メインイメージ非表示
                imgMainArea.Visible = false;
                btnPrint.Visible = false;
                return;
            }

            InitializeValues( tblCheckSheet );

            //印刷ページ用設定
            //暫定的に国コードに遷移元画面IDを設定
            btnPrint.Attributes[ControlUtils.ON_CLICK]
                = String.Format( TRANSFER_PRINT_CLICK
                , PageInfo.ResolveClientUrl( this, PageInfo.PreViewForm )
                , CurrentForm.Token
                , DetailKeyParam.ProductModelCd
                , PageInfo.CheckSheet.pageId
                , DetailKeyParam.Serial
            );

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
        private void InitializeValues( DataTable tblCheckSheet ) {
            TemplateControl tempCtrl = lstCheckSheetList.TemplateControl;
            lstCheckSheetList.DataSource = tblCheckSheet;
            lstCheckSheetList.DataBind();

            Dictionary<string,byte[]> dicImages = new Dictionary<string,byte[]>();
            for ( int loopImg = 0; loopImg < tblCheckSheet.Rows.Count; loopImg++ ) {
                byte[] byteImage = new byte[0];

                if ( ObjectUtils.IsNotNull( tblCheckSheet.Rows[loopImg][LIST_TEMPLATE_CONTROL.CHECK_SHEET.bindField] ) ) {
                    byteImage = (byte[])tblCheckSheet.Rows[loopImg][LIST_TEMPLATE_CONTROL.CHECK_SHEET.bindField];
                }
                
                dicImages.Add(loopImg.ToString(), byteImage);
            }

            CurrentForm.SessionManager.GetImageInfoHandler( CurrentForm.Token ).SetImages( MANAGE_ID, dicImages );
        }

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

        #endregion

    }
}