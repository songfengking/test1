using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using KTFramework.Common.Dao;
using KTFramework.Dao;
using TRC_W_PWT_ProductView.Common;

namespace TRC_W_PWT_ProductView.Dao.Com {
    /// <summary>
    /// サーバプリンター情報DAO
    /// </summary>
    public class TmGeServerPrinterDao : DaoBase {
        #region 定数
        /// <summary>
        /// SQLマップネームスペース
        /// </summary>
        private const string SQLMAP_NAMESPACE = "TmGeServerPrinter";
        /// <summary>
        /// 列名：場所
        /// </summary>
        public const string COLNAME_LOCATION = "LOCATION";
        #endregion

        /// <summary>
        /// エリア一覧情報取得
        /// </summary>
        /// <returns>エリア一覧</returns>
        public static DataTable SelectAreaList() {
            // SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, nameof( SelectAreaList ) );
            return GiaDao.GetInstance().Select( statementId );
        }
    }
}