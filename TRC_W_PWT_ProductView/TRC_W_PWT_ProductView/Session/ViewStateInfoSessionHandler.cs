using KTWebInheritance.BaseClass;
using KTFramework.Common;
using System;
using System.Data;
using System.Collections.Generic;

namespace TRC_W_PWT_ProductView.Session {
    /// <summary>
    /// ViewState情報セッションハンドラ
    /// </summary>
    [Serializable]
    public class ViewStateInfoSessionHandler : BaseSessionHandler {

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ViewStateInfoSessionHandler(): base() {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dic">画面間引継ぎ情報</param>
        public ViewStateInfoSessionHandler( ViewStateInfoSessionHandler handler ) {
            SessionInfo = handler.Clone();
        }

        /// <summary>
        /// ViewState情報取得
        /// </summary>
        /// <param name="manageId">管理ID</param>
        /// <returns>ViewState情報</returns>
        public object GetViewStateInfo( string manageId ) {
            object result = base.Get( manageId );
            return result;
        }

        /// <summary>
        /// ViewState情報格納
        /// </summary>
        /// <param name="manageId">管理ID</param>        
        /// <param name="viewStateInfo">ViewState情報</param>
        public void SetViewStateInfo( string manageId, object viewStateInfo ) {
            base.Set( manageId, viewStateInfo );
        }

        /// <summary>
        /// ViewState情報削除
        /// </summary>
        /// <param name="manageId">管理ID</param>        
        public void ClearViewStateInfo( string manageId ) {
            base.Remove( manageId );
        }
    }
}