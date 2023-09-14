using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using KTFramework.Dao;
using KTFramework.Common;
using KTFramework.Common.Dao;
using TRC_W_PWT_ProductView.Business;
using TRC_W_PWT_ProductView.Defines;

namespace TRC_W_PWT_ProductView.Dao.Com {
    public class MsZisekiDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "MsZiseki";

        /// <summary>
        /// ステーション実績リスト取得
        /// </summary>
        /// <param name="station">ステーション</param>
        /// <param name="jissekiYMD">照会日</param>
        /// <param name="idno">IDNO</param>
        /// <param name="kiban">機番</param>
        /// <param name="katashikiCode">型式コード</param>
        /// <param name="kuniCode">国コード</param>
        /// <param name="katashikiName">型式名</param>
        /// <param name="tokki">特記事項</param>
        /// <param name="maxRecordCount">最大検索件数</param>
        /// <param name="result">結果</param>
        /// <returns>取得結果</returns>
        public static DataTable SelectStationJissekiList( string station, string jissekiYMD, string idno, string kiban, string katashikiCode, string kuniCode, string katashikiName, string tokki, int maxRecordCount, ref OrderBusiness.ResultSet result ) {
            KTBindParameters param = new KTBindParameters();
            param.Add( "station", station );
            param.Add( "jissekiYMD", jissekiYMD );
            param.Add( "idno", idno );
            param.Add( "kiban", kiban );
            param.Add( "katashikiCode", katashikiCode );
            param.Add( "kuniCode", kuniCode );
            param.Add( "katashikiName", katashikiName );
            param.Add( "tokki", tokki );

            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectStationJissekiList" );
            Cursor cursor = GiaDao.GetInstance().OpenCursor( statementId, param );
            DataTable resultTable = null;

            try {
                while ( GiaDao.GetInstance().Fetch( ref resultTable, ref cursor ) ) {
                    if ( resultTable.Rows.Count > maxRecordCount ) {
                        result.Message = new Msg( MsgManager.MESSAGE_WRN_61020 );
                        resultTable.Rows[resultTable.Rows.Count - 1].Delete();
                        break;
                    }
                }
            } finally {
                GiaDao.GetInstance().CloseCursor( ref cursor );
            }          
            return resultTable;
        }
    }
}