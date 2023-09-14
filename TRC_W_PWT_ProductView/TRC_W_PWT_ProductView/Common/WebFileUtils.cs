using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Reflection;
using KTFramework.Common;
using KTWebInheritance.Common;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.UI.Base;

namespace TRC_W_PWT_ProductView.Common {

    /// <summary>
    /// Web用ファイルアクセスユーティリティクラス
    /// </summary>
    public class WebFileUtils {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        /// <summary>
        /// 実ファイルのダウンロードを実行する
        /// </summary>
        /// <param name="page">ページインスタンス</param>
        /// <param name="basePath">対象ディレクトリパス</param>
        /// <param name="fileNm">対象ファイル名</param>
        /// <returns>変換後URL</returns>
        public static void DownloadFile( BaseForm page, string basePath, string fileNm ) {

            string fileFullPath = string.Format( "{0}{1}", basePath, fileNm );

            if ( false == System.IO.File.Exists( fileFullPath ) ) {
                logger.Error( "ファイルダウンロードエラー 指定ファイルが存在しません。ファイル：{0}", fileFullPath );
                page.WriteApplicationMessage( MsgManager.MESSAGE_ERR_71010, fileNm );
                return;
            }

            logger.Info( "ファイルダウンロード 実ファイル読み込み。ファイル：{0}", fileFullPath );
            byte[] binary = System.IO.File.ReadAllBytes( fileFullPath );
            DownloadFile( page, binary, fileNm );
        }

        /// <summary>
        /// ファイルのダウンロードを実行する
        /// </summary>
        /// <param name="page">ページインスタンス</param>
        /// <param name="binary">バイナリデータ</param>
        /// <param name="fileNm">対象ファイル名</param>
        /// <returns>変換後URL</returns>
        public static void DownloadFile( BaseForm page, byte[] binary, string fileNm ) {

            logger.Info( "ファイルダウンロード開始。ファイル名：{0}", fileNm );
            try {
                page.Response.HeaderEncoding = Encoding.ShiftJIS;
                page.Response.ContentEncoding = Encoding.ShiftJIS;
                page.Response.ContentType = "application/octet-stream";
                page.Response.AddHeader( "Content-Disposition", "attachment; filename=" + String.Format( "\"{0}\"", fileNm, Encoding.ShiftJIS ) );
                page.Response.OutputStream.Write( binary, 0, binary.Length );
                page.Response.End();
            } catch ( System.Threading.ThreadAbortException ) {
                //response.Endで必ず発生する為、正常として扱う
            } catch ( Exception ex ) {
                logger.Exception( ex );
                page.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80030, fileNm );
            }
        }
    }
}