using KTWebInheritance.BaseClass;
using KTFramework.Common;
using KTFramework.Dao;
using System;
using System.Data;
using System.Collections.Generic;
using TRC_W_PWT_ProductView.Defines.TypeDefine;

namespace TRC_W_PWT_ProductView.Session {
    /// <summary>
    /// 検索条件情報セッションハンドラ
    /// </summary>
    [Serializable]
    public class ConditionInfoSessionHandler : BaseSessionHandler {

        /// <summary>
        /// 検索条件情報格納構造体
        /// </summary>
        [Serializable]
        public struct ST_CONDITION {
            /// <summary>
            /// 検索条件
            /// </summary>
            public Dictionary<string, object> conditionValue;
            /// <summary>
            /// 検索時コントロール表示情報
            /// </summary>
            public Dictionary<string, string> IdWithText;
            /// <summary>
            /// 検索結果定義
            /// </summary>
            public GridViewDefine[] ResultDefine;
            /// <summary>
            /// 検索結果
            /// </summary>
            public DataTable ResultData;
            /// <summary>
            /// ソート列
            /// </summary>
            public string SortExpression;
            /// <summary>
            /// ソート順
            /// </summary>
            public System.Web.UI.WebControls.SortDirection SortDirection;
        }
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ConditionInfoSessionHandler(): base() {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="handler">検索条件情報</param>
        public ConditionInfoSessionHandler( ConditionInfoSessionHandler handler ) {
            SessionInfo = handler.Clone();
        }

        /// <summary>
        /// 検索条件情報取得
        /// </summary>
        /// <param name="manageId">管理ID</param>
        /// <returns>検索条件情報</returns>
        public ST_CONDITION GetCondition( string manageId ) {
            ST_CONDITION result = new ST_CONDITION();

            object obj = base.Get( manageId );
            if ( ObjectUtils.IsNotNull( obj ) ) {
                result = (ST_CONDITION)obj;
            }

            return result;
        }

        /// <summary>
        /// 検索条件情報格納
        /// </summary>
        /// <param name="manageId">管理ID</param>        
        /// <param name="newCond">検索条件情報</param>
        public void SetCondition( string manageId, ST_CONDITION newCond ) {
            base.Set( manageId, newCond );
        }

        /// <summary>
        /// 検索条件情報削除
        /// </summary>
        /// <param name="manageId">管理ID</param>        
        public void ClearCondition( string manageId ) {
            base.Remove( manageId );
        }
    }
}