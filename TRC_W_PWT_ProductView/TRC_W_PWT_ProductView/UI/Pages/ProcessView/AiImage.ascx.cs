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
using KTWebControl.CustomControls;
using KTWebInheritance.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.ListDefine;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.UI.Base;

namespace TRC_W_PWT_ProductView.UI.Pages.ProcessView {

    /// <summary>
    /// エンジン/トラクタ工程詳細画面:AI画像解析
    /// </summary>
    public partial class AiImage : System.Web.UI.UserControl, Defines.Interface.IDetail {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義

        /// <summary>
        /// 管理ID
        /// </summary>
        const string MANAGE_ID = "AiImage";

        /// <summary>行選択イベント</summary>
        const string MAIN_VIEW_SELECTED = "AiImageDef.SelectMainViewRow(this,'{0}');";

        /// <summary>
        /// 部品組付情報
        /// </summary>
        public class GRID_MAIN {
            /// <summary>TR</summary>
            public static readonly ControlDefine TR_ROW_DATA = new ControlDefine( "trRowData", "TR", "", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>ステーション</summary>
            public static readonly ControlDefine STATION_NM = new ControlDefine( "txtStationNm", "ステーション", "stationNm", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>ステーションコード</summary>
            public static readonly ControlDefine STATION_CD = new ControlDefine( "txtStationCd", "ステーションコード", "stationCd", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>カテゴリ</summary>
            public static readonly ControlDefine CATEGORY = new ControlDefine( "txtCategory", "カテゴリ", "category", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>検査項目</summary>
            public static readonly ControlDefine ANL_ITEM_NM = new ControlDefine( "txtAnlItemNm", "検査項目", "anlItemNm", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>不良タイプ</summary>
            public static readonly ControlDefine NG_TYPE_NM = new ControlDefine( "txtNgTypeNm", "不良タイプ", "ngTypeNm", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>検査結果</summary>
            public static readonly ControlDefine RESULT = new ControlDefine( "txtResult", "検査結果", "result", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>判定日時</summary>
            public static readonly ControlDefine ANL_DATE = new ControlDefine( "txtAnlDate", "判定日時", "anlDate", ControlDefine.BindType.Down, typeof( DateTime ) );
            /// <summary>プレビュー画像パス</summary>
            public static readonly ControlDefine PREVIEW_PATH = new ControlDefine( "txtPreviewPath", "プレビュー画像パス", "previewPath", ControlDefine.BindType.Down, typeof( String ) );
        }

        /// <summary>
        /// 詳細情報
        /// </summary>
        public class GRID_SUB {
            //詳細なし
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

        /// <summary>
        /// 詳細画面切替イベント(サムネイル画像クリックでメインエリアに通常サイズ表示を行う)
        /// </summary>
        /// <remarks>パラメータ 差し替え先コントロール URL</remarks>
        const string CHANGE_MAIN_AREA_VIEW = "AiImageDef.ChangeMainAreaImage('{0}','{1}');";

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
        /// 処理中ステーションコード
        /// </summary>
        private string _currentStationCd;

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
        ControlDefine[] MainControls
        {
            get
            {
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
        ControlDefine[] ChkImgCtrl
        {
            get
            {
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

        /// <summary>
        /// メインリスト行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstMainListLB_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundMainLBList( sender, e );
        }

        /// <summary>
        /// メインリスト行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstMainListRB_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundMainRBList( sender, e );
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
                    tblMain = Business.DetailViewBusiness.SelectEngineAiImageDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                } else if ( DetailKeyParam.ProductKind == ProductKind.Tractor ) {
                    // トラクタ
                    tblMain = Business.DetailViewBusiness.SelectTractorAiImageDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
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
            // データテーブルをステーションコードごとにまとめる
            var results = tblMainData.AsEnumerable().ToList().GroupBy( x => new { station = x["stationCd"] } );

            bool firstItemFlg = true;       // 最初の要素を判定するフラグ
            string firstStationCd = "";     // 先頭のタブのID
            foreach ( var stResults in results ) {
                // ステーションコード取得
                _currentStationCd = StringUtils.ToString( stResults.First()["stationCd"] );

                if ( firstItemFlg == true ) {
                    // 先頭データのステーションコードを保存
                    firstItemFlg = false;
                    firstStationCd = _currentStationCd;
                }

                // 親コントロール
                HtmlGenericControl stationTab = new HtmlGenericControl( "div" );
                stationTab.Attributes.Add( "ID", "tabStation" + _currentStationCd );
                stationTab.Attributes.Add( "class", "tabResult tabDiv" );
                stationTab.Attributes.Add( "style", "overflow: hidden; padding: 3px;" );

                // タブ定義を追加
                CreateTabDefAnlImg( ref stationTab, _currentStationCd );
                CreateTabDefAnlRslt( ref stationTab, _currentStationCd );
                pnlTabDefines.Controls.Add( stationTab );

                // タブ選択イベント
                Page.ClientScript.RegisterStartupScript( this.Page.GetType(), "InitializeTab" + _currentStationCd, "AiImageDef.InitializeTab('" + _currentStationCd + "');", true );

                // タブの属性を設定
                HtmlGenericControl tr = new HtmlGenericControl( "li" );
                tr.Attributes.Add( "class", "tabResult" );

                HtmlGenericControl tabInfo = new HtmlGenericControl( "a" );
                tabInfo.ID = _currentStationCd;
                tabInfo.Attributes.Add( "href", "#tabStation" + _currentStationCd );
                tabInfo.Attributes.Add( "class", "tabStation" + _currentStationCd + " tabColor" );
                tabInfo.Attributes.Add( "OnClick", "AiImageDef.ChangeTab('" + _currentStationCd + "'); return false;" );
                tabInfo.Attributes.Add( "style", "width: 160px;" );
                tabInfo.InnerText = StringUtils.ToString( stResults.First()["stationNm"] );

                // タブ追加
                tr.Controls.Add( tabInfo );
                stationTabs.Controls.Add( tr );

                // グリッドビューに解析結果を表示
                ListView lstMainListLB = (ListView)FindControl( "lstMainListLB" + _currentStationCd );
                ListView lstMainListRB = (ListView)FindControl( "lstMainListRB" + _currentStationCd );
                lstMainListLB.DataSource = stResults.ToArray().CopyToDataTable();
                lstMainListLB.DataBind();
                lstMainListRB.DataSource = stResults.ToArray().CopyToDataTable();
                lstMainListRB.DataBind();

                // カメラ画像設定
                InitializeCheckImage( _currentStationCd );
            }

            // タブ選択イベント
            Page.ClientScript.RegisterStartupScript( this.Page.GetType(), "ChangeTab", "AiImageDef.ChangeTab('" + firstStationCd + "');", true );
        }

        #endregion

        #region リストバインド

        /// <summary>
        /// メインリストバインド処理（左）
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void ItemDataBoundMainLBList( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {
                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Item, MainControls, rowBind, ref dicSetValues );

                //行選択イベントセット
                HtmlTableRow trRow = (HtmlTableRow)e.Item.FindControl( GRID_MAIN.TR_ROW_DATA.controlId + _currentStationCd );
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format( MAIN_VIEW_SELECTED, _currentStationCd );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );
            }
        }

        /// <summary>
        /// メインリストバインド処理（右）
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void ItemDataBoundMainRBList( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {
                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Item, MainControls, rowBind, ref dicSetValues );

                //行選択イベントセット
                HtmlTableRow trRow = (HtmlTableRow)e.Item.FindControl( GRID_MAIN.TR_ROW_DATA.controlId + _currentStationCd );
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format( MAIN_VIEW_SELECTED, _currentStationCd );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );
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
        /// <param name="stationCd"></param>
        private void InitializeCheckImage( string stationCd ) {
            DataTable tblCameraImage = new DataTable();
            tblCameraImage.Columns.Add( "imageData", typeof( byte[] ) );
            tblCameraImage.Columns.Add( "imageFileNm" );
            tblCameraImage.Columns.Add( "takenDate" );

            // IDからリストビューを取得
            ListView lstMainListRB = (ListView)FindControl( "lstMainListRB" + stationCd );
            // ルートディレクトリパス
            string previewRootPath = WebAppInstance.GetInstance().Config.ApplicationInfo.aiimagePreviewBasePath;
            // プレビュー画像パス
            string imageDirPath = @Path.Combine( previewRootPath, StringUtils.ToString( ( (DataTable)lstMainListRB.DataSource ).Rows[0][GRID_MAIN.PREVIEW_PATH.bindField] ) );

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

            // IDからコントロールを取得
            ListView lv = (ListView)FindControl( "lstImageList" + stationCd );
            // データバインド
            lv.DataSource = tblCameraImage;
            lv.DataBind();

            // 表示データ制御
            // IDからコントロールを取得
            Control imgMainArea = FindControl( "imgMainArea" + stationCd );
            if ( 0 < tblCameraImage.Rows.Count ) {
                // 表示データが存在する場合
                imgMainArea.Visible = true;
            } else {
                // 表示データが存在しない場合
                imgMainArea.Visible = false;
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

            CurrentForm.SessionManager.GetImageInfoHandler( CurrentForm.Token ).SetImages( MANAGE_ID + _currentStationCd, dicImages );
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
                Image imgMainArea = (Image)FindControl( "imgMainArea" + _currentStationCd );
                if ( 0 == e.Item.DataItemIndex ) {
                    // 先頭行選択済
                    // メイン画像エリア
                    HtmlGenericControl divCtrl = ( (HtmlGenericControl)e.Item.FindControl( CHK_IMG_LIST.DIV_ROW_DATA.controlId ) );
                    divCtrl.Attributes["class"] = divCtrl.Attributes["class"] + " " + LIST_SELECTED_ITEM_CSS;
                    string urlMainAreaTop = ImageView.GetImageViewUrl( this, CurrentForm.Token, MANAGE_ID + _currentStationCd, e.Item.DataItemIndex, 0, 0
                        , TRC_W_PWT_ProductView.Session.ImageInfoSessionHandler.ContentType.Bitmap, false );
                    imgMainArea.ImageUrl = urlMainAreaTop;

                    // サムネイルエリア 先頭行
                    string urlThumbnail = ImageView.GetImageViewUrl( this, CurrentForm.Token, MANAGE_ID + _currentStationCd, e.Item.DataItemIndex, LIST_MAX_WIDTH, LIST_MAX_HEIGHT
                    , TRC_W_PWT_ProductView.Session.ImageInfoSessionHandler.ContentType.Bitmap, false );
                    imgThumbnail.ImageUrl = urlThumbnail;
                } else {
                    // 先頭行以外はページロード後に画像読み込み
                    string urlThumbnail = ImageView.GetImageViewUrl( this, CurrentForm.Token, MANAGE_ID + _currentStationCd, e.Item.DataItemIndex, LIST_MAX_WIDTH, LIST_MAX_HEIGHT
                        , TRC_W_PWT_ProductView.Session.ImageInfoSessionHandler.ContentType.Bitmap, true );
                    imgThumbnail.ImageUrl = ResourcePath.Image.DummyLoad;
                    imgThumbnail.Attributes[ResourcePath.Image.AttrOriginalSrc] = urlThumbnail;
                }
                // クリックイベントセット
                string urlMainArea = ImageView.GetImageViewUrl( this, CurrentForm.Token, MANAGE_ID + _currentStationCd, e.Item.DataItemIndex, 0, 0
                    , TRC_W_PWT_ProductView.Session.ImageInfoSessionHandler.ContentType.Bitmap, true );
                imgThumbnail.Attributes[ControlUtils.ON_CLICK] = String.Format( CHANGE_MAIN_AREA_VIEW, imgMainArea.ClientID, urlMainArea );

                // ツールチップ設定
                ControlUtils.SetToolTip( e.Item );
            }
        }

        #endregion

        #region タブ定義作成

        /// <summary>
        /// タブ定義作成(検査画像)
        /// </summary>
        /// <param name="parentCtrl"></param>
        /// <param name="stationCd"></param>
        /// <returns></returns>
        private void CreateTabDefAnlImg( ref HtmlGenericControl parentCtrl, string stationCd ) {
            // <div class="div-detail-table-title">■検査画像</div>
            HtmlGenericControl divAnlImg = new HtmlGenericControl( "div" );
            divAnlImg.Attributes.Add( "class", "div-detail-table-title" );
            divAnlImg.InnerText = "■検査画像";
            parentCtrl.Controls.Add( divAnlImg );

            // <div style = "margin-top: 10px;"></div>
            HtmlGenericControl divMarginTop = new HtmlGenericControl( "div" );
            divMarginTop.Attributes.Add( "style", "margin-top: 10px;" );
            parentCtrl.Controls.Add( divMarginTop );

            #region divAnlListArea
            // <div id = "divAnlListArea" class="div-y-scroll-flt" style="width: 232px">
            HtmlGenericControl divAnlListArea = new HtmlGenericControl( "div" );
            divAnlListArea.Attributes.Add( "id", "divAnlListArea" + stationCd );
            divAnlListArea.Attributes.Add( "class", "div-y-scroll-flt" );
            divAnlListArea.Attributes.Add( "style", "width: 232px" );
            // <asp:ListView ID = "lstImageList" runat="server" OnItemDataBound="lstImageList_ItemDataBound">
            ListView lstImageList = new ListView();
            lstImageList.ID = "lstImageList" + stationCd;
            lstImageList.Attributes.Add( "runat", "server" );
            lstImageList.ItemDataBound += lstImageList_ItemDataBound;

            // <LayoutTemplate>
            lstImageList.LayoutTemplate = new LayoutTemplateImageList( stationCd );

            // <ItemTemplate>
            lstImageList.ItemTemplate = new ItemTemplateImageList( stationCd );

            divAnlListArea.Controls.Add( lstImageList );
            #endregion

            #region divAnlViewArea
            // <div id = "divAnlViewArea" >
            HtmlGenericControl divAnlViewArea = new HtmlGenericControl( "div" );
            divAnlViewArea.Attributes.Add( "id", "divAnlViewArea" + stationCd );

            // <div id="divAnlViewBox" class="div-auto-scroll" style="padding: 3px">
            HtmlGenericControl divAnlViewBox = new HtmlGenericControl( "div" );
            divAnlViewBox.Attributes.Add( "id", "divAnlViewBox" + stationCd );
            divAnlViewBox.Attributes.Add( "class", "div-auto-scroll" );
            divAnlViewBox.Attributes.Add( "style", "padding: 3px" );

            // <asp:Image ID = "imgMainArea" runat="server" AlternateText="" />
            Image imgMainArea = new Image();
            imgMainArea.ID = "imgMainArea" + stationCd;
            imgMainArea.Attributes.Add( "runat", "server" );
            imgMainArea.Attributes.Add( "AlternateText", "" );

            divAnlViewBox.Controls.Add( imgMainArea );
            divAnlViewArea.Controls.Add( divAnlViewBox );
            #endregion

            parentCtrl.Controls.Add( divAnlListArea );
            parentCtrl.Controls.Add( divAnlViewArea );
        }

        /// <summary>
        /// タブ定義作成(検査結果)
        /// </summary>
        /// <param name="parentCtrl"></param>
        /// <param name="stationCd"></param>
        /// <returns></returns>
        private void CreateTabDefAnlRslt( ref HtmlGenericControl parentCtrl, string stationCd ) {
            // <div id = "divDetailBodyScroll" class="div-fix-scroll">
            HtmlGenericControl divDetailBodyScroll = new HtmlGenericControl( "div" );
            divDetailBodyScroll.Attributes.Add( "id", "divDetailBodyScroll" + stationCd );
            divDetailBodyScroll.Attributes.Add( "class", "div-fix-scroll" );

            // <div class="div-detail-table-title">■検査結果</div>
            HtmlGenericControl divAnlRslt = new HtmlGenericControl( "div" );
            divAnlRslt.Attributes.Add( "class", "div-detail-table-title" );
            divAnlRslt.InnerText = "■検査結果";
            divDetailBodyScroll.Controls.Add( divAnlRslt );

            // <div style = "margin-top: 10px;" ></ div >
            HtmlGenericControl marginTop = new HtmlGenericControl( "div" );
            marginTop.Attributes.Add( "style", "margin-top: 10px;" );
            divDetailBodyScroll.Controls.Add( marginTop );

            // <div id="divMainListArea" class="div-y-scroll-flt2">
            HtmlGenericControl divMainListArea = new HtmlGenericControl( "div" );
            divMainListArea.Attributes.Add( "id", "divMainListArea" + stationCd );
            divMainListArea.Attributes.Add( "class", "div-y-scroll-flt2" );

            // <table class="table-layout-fix" style="margin-left: 10px">
            HtmlGenericControl table = new HtmlGenericControl( "table" );
            table.Attributes.Add( "class", "table-layout-fix" );
            table.Attributes.Add( "style", "margin-left: 10px" );

            // <tr>
            HtmlTableRow tr1 = new HtmlTableRow();
            // <td>
            HtmlTableCell td1_1 = new HtmlTableCell( "td" );
            // <div id = "divLTMainScroll" class="div-fix-scroll div-left-grid">
            HtmlGenericControl divLTMainScroll = new HtmlGenericControl( "div" );
            divLTMainScroll.Attributes.Add( "id", "divLTMainScroll" + stationCd );
            divLTMainScroll.Attributes.Add( "class", "div-fix-scroll div-left-grid" );
            // <div id = "divMainHeaderLT" runat="server">
            HtmlGenericControl divMainHeaderLT = new HtmlGenericControl( "div" );
            divMainHeaderLT.Attributes.Add( "id", "divMainHeaderLT" + stationCd );
            divMainHeaderLT.Attributes.Add( "runat", "server" );
            // <table id = "solidLTMainHeader" runat="server" class="grid-layout" style="width: 220px;">
            HtmlGenericControl solidLTMainHeader = new HtmlGenericControl( "table" );
            solidLTMainHeader.Attributes.Add( "id", "solidLTMainHeader" + stationCd );
            solidLTMainHeader.Attributes.Add( "runat", "server" );
            solidLTMainHeader.Attributes.Add( "class", "grid-layout" );
            solidLTMainHeader.Attributes.Add( "style", "width: 220px;" );
            // <tr id = "headerLTMainContent" runat="server" class="listview-header_2r ui-state-default">
            HtmlTableRow headerLTMainContent = new HtmlTableRow();
            headerLTMainContent.Attributes.Add( "id", "headerLTMainContent" + stationCd );
            headerLTMainContent.Attributes.Add( "runat", "server" );
            headerLTMainContent.Attributes.Add( "class", "listview-header_2r ui-state-default" );
            // <th id = "StationNm" runat="server" style="width: 220px">ステーション</th>
            HtmlTableCell stNm = new HtmlTableCell( "th" );
            stNm.Attributes.Add( "id", GRID_MAIN.STATION_NM.bindField + stationCd );
            stNm.Attributes.Add( "runat", "server" );
            stNm.Attributes.Add( "style", "width: 220px" );
            stNm.InnerText = "ステーション";
            // <th id = "StationCd" runat="server" style="display: none">ステーションコード</th>
            HtmlTableCell stCd = new HtmlTableCell( "th" );
            stCd.Attributes.Add( "id", GRID_MAIN.STATION_CD.bindField + stationCd );
            stCd.Attributes.Add( "runat", "server" );
            stCd.Attributes.Add( "style", "display: none" );
            stCd.InnerText = "ステーションコード";

            headerLTMainContent.Controls.Add( stNm );
            headerLTMainContent.Controls.Add( stCd );
            solidLTMainHeader.Controls.Add( headerLTMainContent );
            divMainHeaderLT.Controls.Add( solidLTMainHeader );
            divLTMainScroll.Controls.Add( divMainHeaderLT );
            td1_1.Controls.Add( divLTMainScroll );

            // <td>
            HtmlTableCell td1_2 = new HtmlTableCell( "td" );
            // <div id = "divRTMainScroll" class="div-scroll-right-top div-right-grid">
            HtmlGenericControl divRTMainScroll = new HtmlGenericControl( "div" );
            divRTMainScroll.Attributes.Add( "id", "divRTMainScroll" + stationCd );
            divRTMainScroll.Attributes.Add( "class", "div-scroll-right-top div-right-grid" );
            // <div id = "divMainHeaderRT" runat="server">
            HtmlGenericControl divMainHeaderRT = new HtmlGenericControl( "div" );
            divMainHeaderRT.Attributes.Add( "id", "divMainHeaderRT" + stationCd );
            divMainHeaderRT.Attributes.Add( "runat", "server" );
            // <table id = "solidMainRTHeader" runat="server" class="grid-layout" style="width: 1040px;">
            HtmlGenericControl solidMainRTHeader = new HtmlGenericControl( "table" );
            solidMainRTHeader.Attributes.Add( "id", "solidMainRTHeader" + stationCd );
            solidMainRTHeader.Attributes.Add( "runat", "server" );
            solidMainRTHeader.Attributes.Add( "class", "grid-layout" );
            solidMainRTHeader.Attributes.Add( "style", "width: 1040px;" );
            // <tr id = "headerRTMainContent" runat="server" class="listview-header_2r ui-state-default">
            HtmlTableRow headerRTMainContent = new HtmlTableRow();
            headerRTMainContent.Attributes.Add( "id", "headerRTMainContent" + stationCd );
            headerRTMainContent.Attributes.Add( "runat", "server" );
            headerRTMainContent.Attributes.Add( "class", "listview-header_2r ui-state-default" );
            // <th id = "Category" runat="server" style="width: 250px">カテゴリ</th>
            HtmlTableCell category = new HtmlTableCell( "th" );
            category.Attributes.Add( "id", GRID_MAIN.CATEGORY.bindField + stationCd );
            category.Attributes.Add( "runat", "server" );
            category.Attributes.Add( "style", "width: 250px" );
            category.InnerText = "カテゴリ";
            // <th id = "AnlItemNm" runat="server" style="width: 250px">検査項目</th>
            HtmlTableCell anlItemNm = new HtmlTableCell( "th" );
            anlItemNm.Attributes.Add( "id", GRID_MAIN.ANL_ITEM_NM.bindField + stationCd );
            anlItemNm.Attributes.Add( "runat", "server" );
            anlItemNm.Attributes.Add( "style", "width: 250px" );
            anlItemNm.InnerText = "検査項目";
            // <th id = "NgTypeNm" runat="server" style="width: 250px">不良タイプ</th>
            HtmlTableCell ngTypeNm = new HtmlTableCell( "th" );
            ngTypeNm.Attributes.Add( "id", GRID_MAIN.NG_TYPE_NM.bindField + stationCd );
            ngTypeNm.Attributes.Add( "runat", "server" );
            ngTypeNm.Attributes.Add( "style", "width: 250px" );
            ngTypeNm.InnerText = "不良タイプ";
            // <th id = "Result" runat="server" style="width: 90px">検査結果</th>
            HtmlTableCell result = new HtmlTableCell( "th" );
            result.Attributes.Add( "id", GRID_MAIN.RESULT.bindField + stationCd );
            result.Attributes.Add( "runat", "server" );
            result.Attributes.Add( "style", "width: 90px" );
            result.InnerText = "検査結果";
            // <th id = "AnlDate" runat="server" style="width: 200px">判定日時</th>
            HtmlTableCell anlDate = new HtmlTableCell( "th" );
            anlDate.Attributes.Add( "id", GRID_MAIN.ANL_DATE.bindField + stationCd );
            anlDate.Attributes.Add( "runat", "server" );
            anlDate.Attributes.Add( "style", "width: 200px" );
            anlDate.InnerText = "判定日時";
            // <th id = "PreviewPath" runat="server" style="display: none">プレビュー画像パス</th>
            HtmlTableCell previewPath = new HtmlTableCell( "th" );
            previewPath.Attributes.Add( "id", GRID_MAIN.PREVIEW_PATH.bindField + stationCd );
            previewPath.Attributes.Add( "runat", "server" );
            previewPath.Attributes.Add( "style", "display: none" );
            previewPath.InnerText = "プレビュー画像パス";

            headerRTMainContent.Controls.Add( category );
            headerRTMainContent.Controls.Add( anlItemNm );
            headerRTMainContent.Controls.Add( ngTypeNm );
            headerRTMainContent.Controls.Add( result );
            headerRTMainContent.Controls.Add( anlDate );
            headerRTMainContent.Controls.Add( previewPath );
            solidMainRTHeader.Controls.Add( headerRTMainContent );
            divMainHeaderRT.Controls.Add( solidMainRTHeader );
            divRTMainScroll.Controls.Add( divMainHeaderRT );
            td1_2.Controls.Add( divRTMainScroll );

            tr1.Controls.Add( td1_1 );
            tr1.Controls.Add( td1_2 );

            // <tr>
            HtmlTableRow tr2 = new HtmlTableRow();
            // <td>
            HtmlTableCell td2_1 = new HtmlTableCell( "td" );
            // <div id = "divLBMainScroll" class="div-scroll-left-bottom div-left-grid" style="height: 100px">
            HtmlGenericControl divLBMainScroll = new HtmlGenericControl( "div" );
            divLBMainScroll.Attributes.Add( "id", "divLBMainScroll" + stationCd );
            divLBMainScroll.Attributes.Add( "class", "div-scroll-left-bottom div-left-grid" );
            divLBMainScroll.Attributes.Add( "style", "height: 100px" );
            // <div id = "divGrvLBMain" runat="server">
            HtmlGenericControl divGrvLBMain = new HtmlGenericControl( "div" );
            divGrvLBMain.Attributes.Add( "id", "divGrvLBMain" + stationCd );
            divGrvLBMain.Attributes.Add( "runat", "server" );
            // <asp:ListView ID = "lstMainListLB" runat="server" OnItemDataBound="lstMainListLB_ItemDataBound">
            ListView lstMainListLB = new ListView();
            lstMainListLB.ID = "lstMainListLB" + stationCd;
            lstMainListLB.Attributes.Add( "runat", "server" );
            lstMainListLB.ItemDataBound += lstMainListLB_ItemDataBound;
            // <LayoutTemplate>
            lstMainListLB.LayoutTemplate = new LayoutTemplateMainListLB( stationCd );
            // <ItemTemplate>
            lstMainListLB.ItemTemplate = new ItemTemplateMainListLB( stationCd );

            divGrvLBMain.Controls.Add( lstMainListLB );
            divLBMainScroll.Controls.Add( divGrvLBMain );
            td2_1.Controls.Add( divLBMainScroll );

            // <td>
            HtmlTableCell td2_2 = new HtmlTableCell( "td" );
            // <div id = "divRBMainScroll" class="div-visible-scroll div-right-grid" style="height: 100px">
            HtmlGenericControl divRBMainScroll = new HtmlGenericControl( "div" );
            divRBMainScroll.Attributes.Add( "id", "divRBMainScroll" + stationCd );
            divRBMainScroll.Attributes.Add( "class", "div-visible-scroll div-right-grid" );
            divRBMainScroll.Attributes.Add( "style", "height: 100px" );
            // <div id = "divGrvRBMain" runat="server">
            HtmlGenericControl divGrvRBMain = new HtmlGenericControl( "div" );
            divGrvRBMain.Attributes.Add( "id", "divGrvRBMain" + stationCd );
            divGrvRBMain.Attributes.Add( "runat", "server" );
            // <asp:ListView ID = "lstMainListRB" runat="server" OnItemDataBound="lstMainListRB_ItemDataBound"></asp:ListView>
            ListView lstMainListRB = new ListView();
            lstMainListRB.ID = "lstMainListRB" + stationCd;
            lstMainListRB.Attributes.Add( "runat", "server" );
            lstMainListRB.ItemDataBound += lstMainListRB_ItemDataBound;
            // <LayoutTemplate>
            lstMainListRB.LayoutTemplate = new LayoutTemplateMainListRB( stationCd );
            // <ItemTemplate>
            lstMainListRB.ItemTemplate = new ItemTemplateMainListRB( stationCd );

            divGrvRBMain.Controls.Add( lstMainListRB );
            divRBMainScroll.Controls.Add( divGrvRBMain );
            td2_2.Controls.Add( divRBMainScroll );

            tr2.Controls.Add( td2_1 );
            tr2.Controls.Add( td2_2 );

            table.Controls.Add( tr1 );
            table.Controls.Add( tr2 );
            divMainListArea.Controls.Add( table );
            divDetailBodyScroll.Controls.Add( divMainListArea );

            parentCtrl.Controls.Add( divDetailBodyScroll );
        }

        #region テンプレート

        #region lstImageList(検査画像)

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

        #region lstMainListLB(リストビュー左下)

        /// <summary>
        /// LayoutTemplate(リストビュー左下)
        /// </summary>
        private class LayoutTemplateMainListLB : ITemplate {
            /// <summary>ステーションコード</summary>
            string _stationCd = "";

            public LayoutTemplateMainListLB( string stationCd ) {
                _stationCd = stationCd;
            }

            public void InstantiateIn( Control container ) {
                // <table id = "itemPlaceholderContainerLBMain" runat="server" class="grid-layout" style="width: 220px">
                HtmlGenericControl itemPlaceholderContainerLBMain = new HtmlGenericControl( "table" );
                itemPlaceholderContainerLBMain.ID = "itemPlaceholderContainerLBMain" + _stationCd;
                itemPlaceholderContainerLBMain.Attributes.Add( "runat", "server" );
                itemPlaceholderContainerLBMain.Attributes.Add( "class", "grid-layout" );
                itemPlaceholderContainerLBMain.Attributes.Add( "style", "width: 220px" );
                // <tr id = "headerLBMainContent" runat="server" class="listview-header_3r ui-state-default">
                HtmlTableRow headerLBMainContent = new HtmlTableRow();
                headerLBMainContent.ID = "headerLBMainContent" + _stationCd;
                headerLBMainContent.Attributes.Add( "runat", "server" );
                headerLBMainContent.Attributes.Add( "class", "listview-header_3r ui-state-default" );
                // <th id = "StationNm" runat="server" style="width: 220px" />
                HtmlTableCell stNm = new HtmlTableCell( "th" );
                stNm.ID = GRID_MAIN.STATION_NM.bindField + _stationCd;
                stNm.Attributes.Add( "runat", "server" );
                stNm.Attributes.Add( "style", "width: 220px" );
                // <th id = "StationCd" runat="server" style="display: none" />
                HtmlTableCell stCd = new HtmlTableCell( "th" );
                stCd.ID = GRID_MAIN.STATION_CD.bindField + _stationCd;
                stCd.Attributes.Add( "runat", "server" );
                stCd.Attributes.Add( "style", "display: none" );
                // <tr id = "itemPlaceholder" runat="server">
                HtmlTableRow itemPlaceholder = new HtmlTableRow();
                itemPlaceholder.ID = "itemPlaceholder";
                itemPlaceholder.Attributes.Add( "runat", "server" );

                headerLBMainContent.Controls.Add( stNm );
                headerLBMainContent.Controls.Add( stCd );
                itemPlaceholderContainerLBMain.Controls.Add( headerLBMainContent );
                itemPlaceholderContainerLBMain.Controls.Add( itemPlaceholder );

                container.Controls.Add( itemPlaceholderContainerLBMain );
            }
        }

        /// <summary>
        /// ItemTemplate(リストビュー左下)
        /// </summary>
        private class ItemTemplateMainListLB : ITemplate {
            /// <summary>ステーションコード</summary>
            string _stationCd = "";

            public ItemTemplateMainListLB( string stationCd ) {
                _stationCd = stationCd;
            }

            public void InstantiateIn( Control container ) {
                // <tr id = "trRowData" runat="server" class="listview-row ui-widget">
                HtmlTableRow trRowData = new HtmlTableRow();
                trRowData.ID = "trRowData" + _stationCd;
                trRowData.Attributes.Add( "runat", "server" );
                trRowData.Attributes.Add( "class", "listview-row ui-widget" );
                // <td>
                HtmlTableCell td1 = new HtmlTableCell( "td" );
                // <KTCC:KTTextBox ID = "txtStationNm" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                KTTextBox txtStationNm = new KTTextBox();
                txtStationNm.ID = GRID_MAIN.STATION_NM.controlId;
                txtStationNm.Attributes.Add( "runat", "server" );
                txtStationNm.ReadOnly = true;
                txtStationNm.CssClass = "font-default txt-default txt-width-full al-lf";
                // <td>
                HtmlTableCell td2 = new HtmlTableCell( "td" );
                // <KTCC:KTTextBox ID="txtStationCd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                KTTextBox txtStationCd = new KTTextBox();
                txtStationCd.ID = GRID_MAIN.STATION_CD.controlId;
                txtStationCd.Attributes.Add( "runat", "server" );
                txtStationCd.ReadOnly = true;
                txtStationCd.CssClass = "font-default txt-default txt-width-full al-lf";

                td1.Controls.Add( txtStationNm );
                td2.Controls.Add( txtStationCd );
                trRowData.Controls.Add( td1 );
                trRowData.Controls.Add( td2 );

                container.Controls.Add( trRowData );
            }
        }

        #endregion

        #region lstMainListRB(リストビュー右下)

        /// <summary>
        /// LayoutTemplate(リストビュー右下)
        /// </summary>
        private class LayoutTemplateMainListRB : ITemplate {
            /// <summary>ステーションコード</summary>
            string _stationCd = "";

            public LayoutTemplateMainListRB( string stationCd ) {
                _stationCd = stationCd;
            }

            public void InstantiateIn( Control container ) {
                // <table id="itemPlaceholderContainerRBMain" runat="server" class="grid-layout" style="width: 1040px">
                HtmlGenericControl itemPlaceholderContainerRBMain = new HtmlGenericControl( "table" );
                itemPlaceholderContainerRBMain.ID = "itemPlaceholderContainerRBMain" + _stationCd;
                itemPlaceholderContainerRBMain.Attributes.Add( "runat", "server" );
                itemPlaceholderContainerRBMain.Attributes.Add( "class", "grid-layout" );
                itemPlaceholderContainerRBMain.Attributes.Add( "style", "width: 1040px" );
                // <tr id="headerRBMainContent" runat="server" class="listview-header_3r ui-state-default">
                HtmlTableRow headerRBMainContent = new HtmlTableRow();
                headerRBMainContent.ID = "headerRBMainContent" + _stationCd;
                headerRBMainContent.Attributes.Add( "runat", "server" );
                headerRBMainContent.Attributes.Add( "class", "listview-header_3r ui-state-default" );
                // <th id="Category" runat="server" style="width: 250px" />
                HtmlTableCell category = new HtmlTableCell( "th" );
                category.ID = GRID_MAIN.CATEGORY.bindField + _stationCd;
                category.Attributes.Add( "runat", "server" );
                category.Attributes.Add( "style", "width: 250px" );
                // <th id="AnlItemNm" runat="server" style="width: 250px" />
                HtmlTableCell anlItemNm = new HtmlTableCell( "th" );
                anlItemNm.ID = GRID_MAIN.ANL_ITEM_NM.bindField + _stationCd;
                anlItemNm.Attributes.Add( "runat", "server" );
                anlItemNm.Attributes.Add( "style", "width: 250px" );
                // <th id="NgTypeNm" runat="server" style="width: 250px" />
                HtmlTableCell ngTypeNm = new HtmlTableCell( "th" );
                ngTypeNm.ID = GRID_MAIN.NG_TYPE_NM.bindField + _stationCd;
                ngTypeNm.Attributes.Add( "runat", "server" );
                ngTypeNm.Attributes.Add( "style", "width: 250px" );
                // <th id="Result" runat="server" style="width: 90px" />
                HtmlTableCell result = new HtmlTableCell( "th" );
                result.ID = GRID_MAIN.RESULT.bindField + _stationCd;
                result.Attributes.Add( "runat", "server" );
                result.Attributes.Add( "style", "width: 90px" );
                // <th id="AnlDate" runat="server" style="width: 200px" />
                HtmlTableCell anlDate = new HtmlTableCell( "th" );
                anlDate.ID = GRID_MAIN.ANL_DATE.bindField + _stationCd;
                anlDate.Attributes.Add( "runat", "server" );
                anlDate.Attributes.Add( "style", "width: 200px" );
                // <th id="PreviewPath" runat="server" style="display: none" />
                HtmlTableCell previewPath = new HtmlTableCell( "th" );
                previewPath.ID = GRID_MAIN.PREVIEW_PATH.bindField + _stationCd;
                previewPath.Attributes.Add( "runat", "server" );
                previewPath.Attributes.Add( "style", "display: none" );
                // <tr id="itemPlaceholder" runat="server">
                HtmlTableRow itemPlaceholder = new HtmlTableRow();
                itemPlaceholder.ID = "itemPlaceholder";
                itemPlaceholder.Attributes.Add( "runat", "server" );

                headerRBMainContent.Controls.Add( category );
                headerRBMainContent.Controls.Add( anlItemNm );
                headerRBMainContent.Controls.Add( ngTypeNm );
                headerRBMainContent.Controls.Add( result );
                headerRBMainContent.Controls.Add( anlDate );
                headerRBMainContent.Controls.Add( previewPath );
                itemPlaceholderContainerRBMain.Controls.Add( headerRBMainContent );
                itemPlaceholderContainerRBMain.Controls.Add( itemPlaceholder );

                container.Controls.Add( itemPlaceholderContainerRBMain );
            }
        }

        /// <summary>
        /// ItemTemplate(リストビュー右下)
        /// </summary>
        private class ItemTemplateMainListRB : ITemplate {
            /// <summary>ステーションコード</summary>
            string _stationCd = "";

            public ItemTemplateMainListRB( string stationCd ) {
                _stationCd = stationCd;
            }

            public void InstantiateIn( Control container ) {
                // <tr id="trRowData" runat="server" class="listview-row ui-widget">
                HtmlTableRow trRowData = new HtmlTableRow();
                trRowData.ID = "trRowData" + _stationCd;
                trRowData.Attributes.Add( "runat", "server" );
                trRowData.Attributes.Add( "class", "listview-row ui-widget" );
                // <td>
                HtmlTableCell td1 = new HtmlTableCell( "td" );
                // <KTCC:KTTextBox ID="txtCategory" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                KTTextBox txtCategory = new KTTextBox();
                txtCategory.ID = GRID_MAIN.CATEGORY.controlId;
                txtCategory.Attributes.Add( "runat", "server" );
                txtCategory.ReadOnly = true;
                txtCategory.CssClass = "font-default txt-default txt-width-full al-lf";
                td1.Controls.Add( txtCategory );
                // <td>
                HtmlTableCell td2 = new HtmlTableCell( "td" );
                // <KTCC:KTTextBox ID="txtAnlItemNm" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                KTTextBox txtAnlItemNm = new KTTextBox();
                txtAnlItemNm.ID = GRID_MAIN.ANL_ITEM_NM.controlId;
                txtAnlItemNm.Attributes.Add( "runat", "server" );
                txtAnlItemNm.ReadOnly = true;
                txtAnlItemNm.CssClass = "font-default txt-default txt-width-full al-lf";
                td2.Controls.Add( txtAnlItemNm );
                // <td>
                HtmlTableCell td3 = new HtmlTableCell( "td" );
                // <KTCC:KTTextBox ID="txtNgTypeNm" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                KTTextBox txtNgTypeNm = new KTTextBox();
                txtNgTypeNm.ID = GRID_MAIN.NG_TYPE_NM.controlId;
                txtNgTypeNm.Attributes.Add( "runat", "server" );
                txtNgTypeNm.ReadOnly = true;
                txtNgTypeNm.CssClass = "font-default txt-default txt-width-full al-lf";
                td3.Controls.Add( txtNgTypeNm );
                // <td>
                HtmlTableCell td4 = new HtmlTableCell( "td" );
                // <KTCC:KTTextBox ID="txtResult" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                KTTextBox txtResult = new KTTextBox();
                txtResult.ID = GRID_MAIN.RESULT.controlId;
                txtResult.Attributes.Add( "runat", "server" );
                txtResult.ReadOnly = true;
                txtResult.CssClass = "font-default txt-default txt-width-full al-ct";
                td4.Controls.Add( txtResult );
                // <td>
                HtmlTableCell td5 = new HtmlTableCell( "td" );
                // <KTCC:KTTextBox ID="txtAnlDate" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                KTTextBox txtAnlDate = new KTTextBox();
                txtAnlDate.ID = GRID_MAIN.ANL_DATE.controlId;
                txtAnlDate.Attributes.Add( "runat", "server" );
                txtAnlDate.ReadOnly = true;
                txtAnlDate.CssClass = "font-default txt-default txt-width-full al-ct";
                td5.Controls.Add( txtAnlDate );
                // <td>
                HtmlTableCell td6 = new HtmlTableCell( "td" );
                // <KTCC:KTTextBox ID="txtPreviewPath" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                KTTextBox txtPreviewPath = new KTTextBox();
                txtPreviewPath.ID = GRID_MAIN.PREVIEW_PATH.controlId;
                txtPreviewPath.Attributes.Add( "runat", "server" );
                txtPreviewPath.ReadOnly = true;
                txtPreviewPath.CssClass = "font-default txt-default txt-width-full al-ct";
                td6.Controls.Add( txtPreviewPath );

                trRowData.Controls.Add( td1 );
                trRowData.Controls.Add( td2 );
                trRowData.Controls.Add( td3 );
                trRowData.Controls.Add( td4 );
                trRowData.Controls.Add( td5 );
                trRowData.Controls.Add( td6 );

                container.Controls.Add( trRowData );
            }
        }

        #endregion

        #endregion

        #endregion
    }
}