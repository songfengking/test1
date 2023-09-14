using System;
using System.Data;
using System.Collections.Generic;
using KTFramework.Common.Dao;
using KTFramework.Dao;
using KTFramework.Common;
using System.Reflection;
namespace TRC_W_PWT_ProductView.Dao.Com {
    /// <summary>
    /// MS_STATION用DAO
    /// </summary>
    public class MsStationDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "MsStation";

        /// <summary>
        /// 検索
        /// </summary>
        /// <returns></returns>
        public static DataTable Select() {
            //SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "Select" );
            return PicDao.GetInstance().Select( statementId );
        }
    }
}