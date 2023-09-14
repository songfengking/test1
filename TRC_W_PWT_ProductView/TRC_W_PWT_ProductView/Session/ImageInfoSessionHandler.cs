using KTWebInheritance.BaseClass;
using KTFramework.Common;
using KTFramework.Dao;
using System;
using System.Data;
using System.Collections.Generic;

namespace TRC_W_PWT_ProductView.Session {
    /// <summary>
    /// イメージ情報セッションハンドラ
    /// </summary>
    [Serializable]
    public class ImageInfoSessionHandler : BaseSessionHandler {

        /// <summary>
        /// コンテンツ種別
        /// </summary>
        public enum ContentType {
            Bitmap = 0,
            TIFF,
        }

        /// <summary>
        /// レスポンス出力時のコンテンツタイプ
        /// </summary>
        /// <param name="contentTp">コンテンツ種別</param>
        /// <returns>レスポンス出力時のコンテンツタイプ</returns>
        public static string GetResponseContent( ContentType contentTp) {

            switch ( contentTp ) {
            case ContentType.Bitmap: {
                return "image/bmp";
                }
            //case ContentType.TIFF: {
            //    return "image/tiff";
            //    }
            default:{
                break; 
                }
            }

            return "";

        }

        /// <summary>
        /// イメージ情報
        /// </summary>
        public Dictionary<string, byte[]> imageValue;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ImageInfoSessionHandler(): base() {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dic">検索条件情報</param>
        public ImageInfoSessionHandler( ImageInfoSessionHandler handler ) {
            SessionInfo = handler.Clone();
        }

        /// <summary>
        /// イメージ情報取得
        /// </summary>
        /// <param name="manageId">管理ID</param>
        /// <returns>検索条件情報</returns>
        public Dictionary<string, byte[]> GetImages( string manageId ) {
            Dictionary<string, byte[]> result = new Dictionary<string, byte[]>();

            object obj = base.Get( manageId );
            if ( ObjectUtils.IsNotNull( obj ) ) {
                result = (Dictionary<string, byte[]>)obj;
            }

            return result;
        }

        /// <summary>
        /// イメージ情報格納
        /// </summary>
        /// <param name="manageId">管理ID</param>        
        /// <param name="imageValue">検索条件情報</param>
        public void SetImages( string manageId, Dictionary<string, byte[]> imageValue ) {
            base.Set( manageId, imageValue );
        }

        /// <summary>
        /// イメージ情報削除
        /// </summary>
        /// <param name="manageId">管理ID</param>        
        public void ClearImages( string manageId ) {
            base.Remove( manageId );
        }
    }
}