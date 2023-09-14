using System;
using System.Data;
using System.Collections.Generic;
using KTFramework.Common.Dao;
using KTFramework.Dao;
using KTFramework.Common;
using System.Reflection;
namespace TRC_W_PWT_ProductView.Dao.Com {
    /// <summary>
    /// 型式マスタ用DAO
    /// </summary>
    public class KatashikiMasterDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "KatashikiMaster";

        /// <summary>
        /// 型式マスタデータ取得
        /// </summary>
        /// <param name="plantCode">工場</param>
        /// <param name="katashikiCode">型式コード</param>
        /// <returns>型式マスタデータ</returns>
        public static DataTable SelectData( string plantCode, string katashikiCode ) {
            //SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectData" );

            KTBindParameters param = new KTBindParameters();
            param.Add( "plantCode", plantCode );
            param.Add( "katashikiCode", katashikiCode );
            
            return GiaDao.GetInstance().Select( statementId, param );
        }
    }
}