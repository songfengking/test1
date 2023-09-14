using System;
using System.Data;
using System.Collections.Generic;
using KTFramework.Common.Dao;
using KTFramework.Dao;
using KTFramework.Common;
using System.Reflection;
namespace TRC_W_PWT_ProductView.Dao.Com {
    /// <summary>
    /// ジョブ制御情報DAO
    /// </summary>
    public class JobControlInfoDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "JobControlInfo";

        /// <summary>
        /// 実行中ジョブ取得
        /// </summary>
        /// <param name="jobId">ジョブID</param>
        /// <returns></returns>
        public static DataTable Select(string jobId) {
            //SQL実行
            KTBindParameters param = new KTBindParameters();
            param.Add( "jobId", jobId );
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "Select" );
            return GiaDao.GetInstance().Select( statementId, param );
        }
    }
}