using System;
using System.Data;
using System.Collections.Generic;
using KTFramework.Common.Dao;
using KTFramework.Dao;
using KTFramework.Common;
using System.Reflection;
namespace TRC_W_PWT_ProductView.Dao.Com {
    /// <summary>
    /// Imadoko用DAO
    /// </summary>
    public class ImadokoDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "Imadoko";

        /// <summary>
        /// 実績検索結果取得
        /// </summary>
        /// <param name="searchSyubetu">検索種別</param>
        /// <param name="idno">IDNO</param>
        /// <param name="katashikiCode">型式コード</param>
        /// <param name="kuniCode">国コード</param>
        /// <param name="katashikiName">型式名</param>
        /// <param name="kiban">機番</param>
        /// <param name="tonyuYMNum">指示月度連番</param>
        /// <param name="kakuteiYMNum">確定月度連番</param>
        /// <returns>実績検索結果</returns>
        public static DataTable SelectList( string searchSyubetu, string idno, string katashikiCode, string kuniCode, string katashikiName, string kiban, string tonyuYMNum, string kakuteiYMNum ) {
            //SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectList" );

            KTBindParameters param = new KTBindParameters();
            param.Add( "searchSyubetu", searchSyubetu );
            param.Add( "idno", idno );
            param.Add( "katashikiCode", katashikiCode );
            param.Add( "kuniCode", kuniCode );
            param.Add( "katashikiName", katashikiName );
            param.Add( "kiban", kiban );
            param.Add( "tonyuYMNum", tonyuYMNum );
            param.Add( "kakuteiYMNum", kakuteiYMNum );

            return GiaDao.GetInstance().Select( statementId, param );
        }
    }
}