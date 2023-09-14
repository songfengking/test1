using System;
using System.Data;
using System.Collections.Generic;
using KTFramework.Common.Dao;
using KTFramework.Dao;
using KTFramework.Common;
using System.Reflection;
namespace TRC_W_PWT_ProductView.Dao.Com {
    /// <summary>
    /// 7桁機番変換DAO
    /// </summary>
    public class Serial7Dao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "Serial";

        /// <summary>
        /// 7桁機番取得処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial6">6桁機番</param>
        /// <returns></returns>
        public static string SelectSerial7(string productModelCd, string serial6) {
            string serial = serial6;
            //SQL実行
            KTBindParameters param = new KTBindParameters();
            param.Add( "productModelCd", productModelCd );
            param.Add( "serial6", serial6 );
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectSerial" );
            DataTable tblResult = GiaDao.GetInstance().Select( statementId, param );
            if ( 1 == tblResult.Rows.Count ) {
                serial = StringUtils.ToString( tblResult.Rows[0]["serial"] );
            }
            return serial;
        }
    }
}