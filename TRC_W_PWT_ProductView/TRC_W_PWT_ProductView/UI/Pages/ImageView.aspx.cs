using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Drawing;
using KTFramework.Common;
using KTWebInheritance.Common;
using TRC_W_PWT_ProductView.Session;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.UI.Base;
using TRC_W_PWT_ProductView.Defines;

namespace TRC_W_PWT_ProductView.UI.Pages {
    /// <summary>
    /// 画像表示
    /// </summary>
    public partial class ImageView : Page {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 画面初期化処理
        /// <summary>
        /// 画面ロード処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad( EventArgs e ) {
            DoPageLoad();
        }
        #endregion

        #region プロパティ

        #region セッションアクセサー

        /// <summary>セッションマネージャ</summary>
        private SessionManagerInstance _sessionManager = null;
        /// <summary>セッションマネージャ</summary>
        protected internal SessionManagerInstance SessionManager {
            get {

                if ( true == ObjectUtils.IsNull( _sessionManager ) ) {
                    _sessionManager = new SessionManagerInstance( Page.Session );
                }

                return _sessionManager;
            }
        }

        #endregion

        #endregion

        #region イベントメソッド

        /// <summary>
        /// ページロード処理
        /// </summary>
        protected void DoPageLoad() {
            Byte[] byteImage = GetImage();
            if ( true == ObjectUtils.IsNotNull( byteImage ) ) {
                WriteImage( byteImage );
            }
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 画像情報取得
        /// </summary>
        /// <returns></returns>
        private byte[] GetImage() {

            byte[] imageByte = null;

            string token = Page.Request.QueryString.Get( RequestParameter.ImageView.TOKEN );
            string manageId = Page.Request.QueryString.Get( RequestParameter.ImageView.MANAGE_ID );
            string indexStr = Page.Request.QueryString.Get( RequestParameter.ImageView.INDEX );

            int index = NumericUtils.ToInt( indexStr, -1 );

            if ( 0 <= index ) {

                Dictionary<string, byte[]> dicImages = SessionManager.GetImageInfoHandler( token ).GetImages( manageId );
                if ( true == dicImages.ContainsKey( indexStr ) ) {
                    imageByte = dicImages[indexStr];
                }
            }

            return imageByte;
        }

        /// <summary>
        /// 画像出力
        /// </summary>
        /// <param name="imageByte"></param>
        private void WriteImage( byte[] imageByte ) {

            string contentTypeStr = Page.Request.QueryString.Get( RequestParameter.ImageView.CONTENT_TYPE );
            ImageInfoSessionHandler.ContentType contentType = (ImageInfoSessionHandler.ContentType)Enum.Parse( typeof( ImageInfoSessionHandler.ContentType ), contentTypeStr );

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            ms.Write( imageByte, 0, imageByte.Length );
            System.Drawing.Image baseImage = System.Drawing.Image.FromStream( ms );
            ms.Close();
            Size sz = baseImage.Size;

            System.Drawing.Image processImage = null;
            Graphics gra = null;

            //画像表示のみ縮尺を行う
            //※現在マルチページTIFFが無いので全てBITMAPで扱う
            if ( contentType == ImageInfoSessionHandler.ContentType.Bitmap ) {

                string widthStr = Page.Request.QueryString.Get( RequestParameter.ImageView.WIDTH );
                string heightStr = Page.Request.QueryString.Get( RequestParameter.ImageView.HEIGHT );

                int width = NumericUtils.ToInt( widthStr, -1 );
                int height = NumericUtils.ToInt( heightStr, -1 );

                double ratioHeight = 0;
                double ratioWidth = 0;
                double ratioOf = 0;

                if ( 0 >= width ) {
                    width = sz.Width;
                }

                if ( 0 >= height ) {
                    height = sz.Height;
                }

                ratioWidth = (double)width / sz.Width;
                ratioHeight = (double)height / sz.Height;
                if ( ratioHeight > ratioWidth ) {
                    ratioOf = ratioWidth;
                } else {
                    ratioOf = ratioHeight;
                }

                if ( ratioOf < 1 ) {
                    processImage = new Bitmap( Convert.ToInt32( baseImage.Width * ratioOf ), Convert.ToInt32( baseImage.Height * ratioOf ) );
                    gra = Graphics.FromImage( processImage );
                    //縮小比に従って補間方法分岐(25%)
                    if ( ratioOf > 0.25 ) {
                        gra.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
                    } else {
                        gra.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
                    }
                }
            } else if ( contentType == ImageInfoSessionHandler.ContentType.TIFF ) {
                processImage = new Bitmap( Convert.ToInt32( baseImage.Width ), Convert.ToInt32( baseImage.Height ) );
                gra = Graphics.FromImage( processImage );
                gra.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            }

            if ( true == ObjectUtils.IsNotNull( gra ) ) {
                gra.DrawImage( baseImage, 0, 0, processImage.Width, processImage.Height );
                ImageConverter imgConv = new ImageConverter();
                imageByte = (byte[])imgConv.ConvertTo( processImage, typeof( byte[] ) );
            }

            try {
                //Response.ContentType = ImageInfoSessionHandler.GetResponseContent( contentType );
                Response.ContentType = ImageInfoSessionHandler.GetResponseContent( ImageInfoSessionHandler.ContentType.Bitmap );
                Response.Flush();
                Response.BinaryWrite( imageByte );
                Response.End();
            } catch ( System.Threading.ThreadAbortException ) {
                //response.Endで必ず発生する為、正常として扱う
            } catch ( Exception ex ) {
                logger.Exception( ex );
            }
        }

        #region 公開静的メソッド
        /// <summary>
        /// 画像表示画面へのURLを作成します
        /// </summary>
        /// <param name="form">ページ</param>
        /// <param name="manageId">管理ID</param>
        /// <param name="index">インデックス</param>
        /// <param name="width">最大幅</param>
        /// <param name="height">最大高さ</param>
        /// <returns>クライアント用URL</returns>
        public static string GetImageViewUrl( Control setCtrl, string token, string manageId, int index, int width, int height, ImageInfoSessionHandler.ContentType contentType, bool IsClientUrl = false ) {

            string pageUrl = "";
            if ( true == IsClientUrl ) {
                pageUrl = PageInfo.ResolveClientUrl( setCtrl, PageInfo.ImageView );
            } else {
                pageUrl = PageInfo.ImageView.url;
            }

            //[0] URL [1][2] トークン [3][4] 管理ID [5][6] インデックス [7][8] 最大幅 [9][10] 最大高さ [11][12] コンテンツ種別
            string url = string.Format( "{0}?{1}={2}&{3}={4}&{5}={6}&{7}={8}&{9}={10}&{11}={12}"
                                , pageUrl
                                , RequestParameter.ImageView.TOKEN
                                , token
                                , RequestParameter.ImageView.MANAGE_ID
                                , manageId
                                , RequestParameter.ImageView.INDEX
                                , index
                                , RequestParameter.ImageView.WIDTH
                                , width
                                , RequestParameter.ImageView.HEIGHT
                                , height
                                , RequestParameter.ImageView.CONTENT_TYPE
                                , contentType.ToString()
                                );

            return url;

        }
        #endregion

        #endregion
    }
}