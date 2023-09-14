using KTWebInheritance.BaseClass;
using KTFramework.Common;
using System;
using System.Data;
using System.Collections.Generic;

namespace TRC_W_PWT_ProductView.Session {
    /// <summary>
    /// 画面コントロール情報セッションハンドラ
    /// </summary>
    [Serializable]
    public class PageControlInfoSessionHandler : BaseSessionHandler {

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PageControlInfoSessionHandler(): base() {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dic">画面コントロール情報</param>
        public PageControlInfoSessionHandler( PageControlInfoSessionHandler handler ) {
            SessionInfo = handler.Clone();
        }

        /// <summary>
        /// 画面コントロール情報取得
        /// </summary>
        /// <param name="manageId">管理ID</param>
        /// <returns>画面コントロール情報</returns>
        public Dictionary<string, object> GetPageControlInfo( string manageId ) {
            Dictionary<string, object> result = new Dictionary<string, object>();

            object obj = base.Get( manageId );
            if ( ObjectUtils.IsNotNull( obj ) ) {
                result = (Dictionary<string, object>)obj;
            }

            return result;
        }

        /// <summary>
        /// 画面コントロール情報格納
        /// </summary>
        /// <param name="manageId">管理ID</param>        
        /// <param name="pageControlInfo">画面コントロール情報</param>
        public void SetPageControlInfo( string manageId, Dictionary<string, object> pageControlInfo ) {
            base.Set( manageId, pageControlInfo );
        }

        /// <summary>
        /// 画面コントロール情報削除
        /// </summary>
        /// <param name="manageId">管理ID</param>        
        public void ClearPageControlInfo( string manageId ) {
            base.Remove( manageId );
        }
    }
}