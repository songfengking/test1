using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using KTFramework.Common.Dao;
using KTFramework.Dao;
using KTFramework.Common;
using TRC_W_PWT_ProductView.Common;

namespace TRC_W_PWT_ProductView.Dao.Com {
    /// <summary>
    /// 区分グループDAO
    /// </summary>
    public class ClassGroupDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "ClassGroup";

        /// <summary>
        /// 検索
        /// </summary>
        /// <returns>検索結果</returns>
        /// <remarks>
        /// パラメータ条件
        /// [必須]PRODUCT_KIND_CD
        /// [必須]GROUP_CD
        /// </remarks>
        public static DataTable Select( KTBindParameters param ) {
            //SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "Select" );
            return GiaDao.GetInstance().Select( statementId, param );
        }

        /// <summary>
        /// 全検索
        /// </summary>
        /// <returns>検索結果</returns>
        /// <remarks>
        /// DisplayOrderが0以上のデータを取得します
        /// </remarks>
        public static DataTable SelectAll() {
            //SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectAll" );
            return GiaDao.GetInstance().Select( statementId );
        }

        /// <summary>
        /// リスト取得
        /// </summary>
        /// <returns>リストアイテム配列</returns>
        /// <remarks>
        /// パラメータ条件
        /// [必須]PRODUCT_KIND_CD
        /// [必須]GROUP_CD
        /// </remarks>
        public static ListItem[] SelectByList( KTBindParameters param ) {
            //SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "Select" );
            DataTable tblResult = GiaDao.GetInstance().Select( statementId, param );

            return ControlUtils.GetListItems( tblResult, "CLASS_NM", "CLASS_CD" );
        }
    }
}