using KTWebInheritance.BaseClass;
using KTFramework.Common;
using System;
using System.Data;
using System.Collections.Generic;

namespace TRC_W_PWT_ProductView.Session {
    /// <summary>
    /// 画面間引継情報セッションハンドラ
    /// </summary>
    [Serializable]
    public class TransInfoSessionHandler : BaseSessionHandler {

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TransInfoSessionHandler(): base() {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="handler">画面間引継ぎ情報</param>
        public TransInfoSessionHandler( TransInfoSessionHandler handler ) {
            SessionInfo = handler.Clone();
        }

        /// <summary>
        /// 画面間引継情報取得
        /// </summary>
        /// <param name="manageId">管理ID</param>
        /// <returns>画面間引継情報</returns>
        public Dictionary<string, object> GetTransInfo( string manageId ) {
            Dictionary<string, object> result = new Dictionary<string, object>();

            object obj = base.Get( manageId );
            if ( ObjectUtils.IsNotNull( obj ) ) {
                result = (Dictionary<string, object>)obj;
            }

            return result;
        }

        /// <summary>
        /// 画面間引継情報格納
        /// </summary>
        /// <param name="manageId">管理ID</param>        
        /// <param name="transInfo">画面間引継情報</param>
        public void SetTransInfo( string manageId, Dictionary<string, object> transInfo ) {
            base.Set( manageId, transInfo );
        }

        /// <summary>
        /// 画面間引継情報削除
        /// </summary>
        /// <param name="manageId">管理ID</param>        
        public void ClearTransInfo( string manageId ) {
            base.Remove( manageId );
        }
    }
}