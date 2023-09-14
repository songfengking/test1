using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using KTFramework.Common;
using KTFramework.Dao;
using KTWebControl.CustomControls;
using KTWebInheritance.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.UI.Base;

namespace TRC_W_PWT_ProductView.UI.Pages.UserControl {
    /// <summary>
    /// ローディング中表示コントロール
    /// </summary>
    public partial class LoadingData : System.Web.UI.UserControl {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定義

        #region Attribute キー定義
        /// <summary>
        /// ロード種別
        /// </summary>
        private const string LOAD_TYPE_ATTR = "LoadingType";
        /// <summary>
        /// 背景要素CSS
        /// </summary>
        private const string LOAD_BG_CSS_ATTR = "LoadingBackGroundCss_";
        /// <summary>
        /// ローディング画像 パス
        /// </summary>
        private const string LOAD_IMAGE_SRC_ATTR = "LoadingImageSrc_";
        /// <summary>
        /// 文字列CSS
        /// </summary>
        private const string LOAD_CHAR_CSS_ATTR = "LoadingCharCss_";
        /// <summary>
        /// 表示文字列
        /// </summary>
        private const string LOAD_CHAR_TEXT_ATTR = "LoadingCharText_";

        #endregion

        #region CSS定義
        /// <summary>
        /// 背景要素CSS
        /// </summary>
        private const string LOAD_TYPE_CSS_BG = "loading-bg-";
        /// <summary>
        /// 文字列CSS
        /// </summary>
        private const string LOAD_TYPE_CSS_CHAR = "loading-char-";

        #endregion

        #region 標準ローディング
        /// <summary>
        /// ロード種別 標準
        /// </summary>
        private const string LOAD_TYPE_DEFAULT = "default";
        /// <summary>
        /// ロード種別 標準用 画像パス
        /// </summary>
        private const string LOAD_TYPE_DEFAULT_IMAGE = ResourcePath.Image.DefaultLoading;
        #endregion

        #region 透過ローディング
        /// <summary>
        /// ロード種別 透過
        /// </summary>
        private const string LOAD_TYPE_TRANSPARENT = "transparent";
        #endregion

        #region 画面終了ローディング
        /// <summary>
        /// ロード種別 画面終了
        /// </summary>
        private const string LOAD_TYPE_EXIT = "exit";
        /// <summary>
        /// ロード種別 標準用 画像パス
        /// </summary>
        private const string LOAD_TYPE_EXIT_IMAGE = "";
        #endregion
        
        /// <summary>
        /// ローディング要素構造体
        /// </summary>
        internal struct ST_LOAD {
            /// <summary>
            /// ロード種別
            /// </summary>
            public string loadType;
            /// <summary>
            /// 背景要素CSS
            /// </summary>
            public string backGroundCss;
            /// <summary>
            /// ローディング画像 パス
            /// </summary>
            public string image;
            /// <summary>
            /// 文字列CSS
            /// </summary>
            public string charCss;
            /// <summary>
            /// 表示文字列
            /// </summary>
            public string charText;

            /// <summary>
            /// ローディング要素構造体 コンストラクタ
            /// </summary>
            /// <param name="loadType">ロード種別</param>
            /// <param name="backGroundCss">背景要素CSS</param>
            /// <param name="image">ローディング画像 パス</param>
            /// <param name="charCss">文字列CSS</param>
            /// <param name="charText">表示文字列</param>
            public ST_LOAD( string loadType, string backGroundCss, string image, string charCss, string charText ) {
                this.loadType = loadType;
                this.backGroundCss = backGroundCss;
                this.image = image;
                this.charCss = charCss;
                this.charText = charText;
            }
        }

        /// <summary>
        /// ローディング定義
        /// </summary>
        internal static class LOADINGS_DEFINES {
            /// <summary>標準</summary>
            public static readonly ST_LOAD DEFAULT = new ST_LOAD( LOAD_TYPE_DEFAULT, LOAD_TYPE_DEFAULT, LOAD_TYPE_DEFAULT_IMAGE, LOAD_TYPE_DEFAULT, "読み込み中" );
            /// <summary>透過</summary>
            public static readonly ST_LOAD TRANSPARENT = new ST_LOAD( LOAD_TYPE_TRANSPARENT, LOAD_TYPE_TRANSPARENT, "", "", "" );
            /// <summary>終了</summary>
            public static readonly ST_LOAD EXIT = new ST_LOAD( LOAD_TYPE_EXIT, LOAD_TYPE_EXIT, LOAD_TYPE_EXIT_IMAGE, LOAD_TYPE_EXIT, "画面終了中" );
        }

        #endregion

        #region イベント

        /// <summary>
        /// ページロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load( object sender, EventArgs e ) {

            //初期化
            divLoadingBackGround.Style["class"] = ResourcePath.CSS.LoadingDivBG + " " + ResourcePath.CSS.SizeZero;
            //imgLoadingImage.ImageUrl = ResourcePath.Image.DummyLoad;
            imgLoadingImage.Style["class"] = ResourcePath.CSS.SizeZero;
            lblLoadingChar.Style["class"] = ResourcePath.CSS.Invisible;

            object[] loadDefines = ControlUtils.GetStaticDefineArray( typeof( LOADINGS_DEFINES ), typeof(ST_LOAD) );
            SetAttributes( loadDefines );
        }

        #endregion

        /// <summary>
        /// Attribute要素セット
        /// </summary>
        /// <param name="loadDefines"></param>
        private void SetAttributes( object[] loadDefines ) {
            foreach ( object loadObj in loadDefines ) {
                ST_LOAD loadDef = (ST_LOAD)loadObj;

                divLoadingBackGround.Attributes[LOAD_BG_CSS_ATTR + loadDef.loadType] = loadDef.backGroundCss == "" ? "" : LOAD_TYPE_CSS_BG + loadDef.backGroundCss;
                imgLoadingImage.Attributes[LOAD_IMAGE_SRC_ATTR + loadDef.loadType] = loadDef.image == "" ? "" : ResolveClientUrl( loadDef.image );
                lblLoadingChar.Attributes[LOAD_CHAR_CSS_ATTR + loadDef.loadType] = loadDef.charCss == "" ? "" : LOAD_TYPE_CSS_CHAR + loadDef.charCss;
                lblLoadingChar.Attributes[LOAD_CHAR_TEXT_ATTR + loadDef.loadType] = loadDef.charText;
            }
        }
    }
}