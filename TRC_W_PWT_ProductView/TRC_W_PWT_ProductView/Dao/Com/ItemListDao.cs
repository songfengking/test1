using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using KTFramework.Dao;
using KTFramework.Common;
using KTFramework.Common.Dao;

namespace TRC_W_PWT_ProductView.Dao.Com {
    public class ItemListDao : DaoBase {
        /// <summary>
        /// SQLマップネームスペース
        /// </summary>
        private const string SQLMAP_NAMESPACE = "ItemList";
        /// <summary>
        /// 列名：値
        /// </summary>
        public const string COLNAME_VALUE = "VALUE";
        /// <summary>
        /// 列名：表示名
        /// </summary>
        public const string COLNAME_TEXT = "TEXT";

        /// <summary>
        /// 実績ステーションリスト取得  
        /// </summary>
        /// <returns>取得結果</returns>
        public static DataTable SelectJissekiStationList() {
            string statementId = GetFullStatementIdForLibrary(SQLMAP_NAMESPACE, "SelectJissekiStationList" );
            DataTable resultTable = GiaDao.GetInstance().Select(statementId);
            return resultTable;
        }

        /// <summary>
        /// 作業指示月度リスト取得  
        /// </summary>
        /// <returns>取得結果</returns>
        public static DataTable SelectSagyoGatsudoList() {
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectSagyoGatsudoList" );
            DataTable resultTable = GiaDao.GetInstance().Select( statementId );
            return resultTable;
        }
    }
}