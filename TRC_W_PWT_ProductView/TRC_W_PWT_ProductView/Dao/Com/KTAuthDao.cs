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
    /// KTAuth関連DAO
    /// </summary>
    public class KTAuthDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "KTAuth";

        /// <summary>
        /// 検索
        /// </summary>
        /// <returns>検索結果</returns>
        /// <param name="applicationID">ApplicationID</param>
        /// <remarks>
        /// パラメータ条件
        /// [必須]ApplicationID
        /// </remarks>
        public static DataTable SelectAPMaster( string applicationID ) {

            KTBindParameters param = new KTBindParameters();
            param.Add( "ApplicationID", applicationID );

            //SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectAPMaster" );
            return GiaDao.GetInstance().Select(statementId, param);
        }

        /// <summary>
        /// 検索
        /// </summary>
        /// <returns>検索結果</returns>
        /// <param name="LogonID">ログオン認証から受け取ったログオンキー</param>
        /// <remarks>
        /// パラメータ条件
        /// [必須]LogonID
        /// </remarks>
        public static DataTable SelectLogonInfo( string LogonID ) {

            KTBindParameters param = new KTBindParameters();
            param.Add( "LogonID", LogonID );

            //SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectLogonInfo" );
            return GiaDao.GetInstance().Select( statementId, param );
        }
    }
}