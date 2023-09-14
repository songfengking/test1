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
    /// 製品種別DAO
    /// </summary>
    public class ProductKindDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "ProductKind";

        /// <summary>
        /// 検索
        /// </summary>
        /// <returns></returns>
        public static DataTable Select() {
            //SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "Select" );
            return GiaDao.GetInstance().Select( statementId );
        }

        /// <summary>
        /// リスト取得
        /// </summary>
        /// <returns>リストアイテム配列</returns>
        public static ListItem[] SelectByList() {
            //SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "Select" );
            DataTable tblResult = GiaDao.GetInstance().Select( statementId );

            return ControlUtils.GetListItems( tblResult, "PRODUCT_KIND_NM", "PRODUCT_KIND_CD" );
        }
    }
}