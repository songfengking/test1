using System;
using System.Data;
using System.Collections.Generic;
using KTFramework.Common.Dao;
using KTFramework.Dao;
using KTFramework.Common;
using System.Reflection;
namespace TRC_W_PWT_ProductView.Dao.Com {
    /// <summary>
    /// TBL_ステーション通過順序用DAO
    /// </summary>
    public class StationTsuukaJunjoDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "StationTsuukaJunjo";

        /// <summary>
        /// ステーション通過順序取得
        /// </summary>
        /// <param name="shijiLevel">指示レベル</param>
        /// <returns>ステーション通過順序検索結果</returns>
        public static DataTable SelectList( string shijiLevel ) {
            //SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectList" );
            KTBindParameters param = new KTBindParameters();
            param.Add( "shijiLevel", shijiLevel );
            return GiaDao.GetInstance().Select( statementId, param );
        }
    }
}