using System;
using System.Data;
using System.Collections.Generic;
using KTFramework.Common.Dao;
using KTFramework.Dao;
using KTFramework.Common;
using System.Reflection;
using TRC_W_PWT_ProductView.Business;
using TRC_W_PWT_ProductView.Defines;

namespace TRC_W_PWT_ProductView.Dao.Com {
    /// <summary>
    /// MS_SAGYO用DAO
    /// </summary>
    public class MsSagyoDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "MsSagyo";

        /// <summary>
        /// MS_SAGYOデータ取得
        /// </summary>
        /// <param name="idno">IDNO</param>
        /// <returns>MS_SAGYOデータ</returns>
        public static DataTable SelectData( string idno ) {
            //SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectData" );

            KTBindParameters param = new KTBindParameters();
            param.Add( "idno", idno );

            return GiaDao.GetInstance().Select( statementId, param );
        }

        /// <summary>
        /// 作業情報取得
        /// </summary>
        /// <param name="idno">IDNO</param>
        /// <param name="kiban">機番</param>
        /// <param name="shijiYM">月度</param>
        /// <param name="maxRecordCount">最大検索件数</param>
        /// <param name="result">結果</param>
        /// <returns>取得結果</returns>
        public static DataTable SelectJunjoListByIdKiban( string idno, string kiban, string shijiYM, int maxRecordCount, ref OrderBusiness.ResultSet result ) {
            KTBindParameters param = new KTBindParameters();
            param.Add( "idno", idno );
            param.Add( "kiban", kiban );
            param.Add( "shijiYM", shijiYM );

            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectJunjoListByIdKiban" );

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

        /// <summary>
        /// 順序リスト2取得
        /// </summary>
        /// <param name="shijiLevel">指示レベル</param>
        /// <param name="idno">IDNO</param>
        /// <param name="kiban">機番</param>
        /// <param name="shijiYM">月度</param>
        /// <param name="katashikiCode">型式コード</param>
        /// <param name="kuniCode">国コード</param>
        /// <param name="katashikiName">型式名</param>
        /// <param name="tokki">特記事項</param>
        /// <param name="targetSagyoKeep">TARGET_SAGYO_KEEP</param>
        /// <param name="pattern">期</param>
        /// <param name="generalPattern">総称パターン</param>
        /// <param name="maxRecordCount">最大検索件数</param>
        /// <param name="result">結果</param>
        /// <returns>取得結果</returns>
        public static DataTable SelectJunjoList2( string shijiLevel, string idno, string kiban, string shijiYM, string katashikiCode, string kuniCode, string katashikiName, string tokki, bool targetSagyoKeep, string pattern, string generalPattern, int maxRecordCount, ref OrderBusiness.ResultSet result ) {
            KTBindParameters param = new KTBindParameters();
            param.Add( "shijiLevel", shijiLevel );
            param.Add( "idno", idno );
            param.Add( "kiban", kiban );
            param.Add( "shijiYM", shijiYM );
            param.Add( "katashikiCode", katashikiCode );
            param.Add( "kuniCode", kuniCode );
            param.Add( "katashikiName", katashikiName );
            param.Add( "tokki", tokki );
            param.Add( "targetSagyoKeep", targetSagyoKeep );
            param.Add( "ki", pattern );
            param.Add( "soshoPattern", generalPattern );

            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectJunjoList_2" );
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

        /// <summary>
        /// 順序リスト取得
        /// </summary>
        /// <param name="shijiLevel">指示レベル</param>
        /// <param name="idno">IDNO</param>
        /// <param name="kiban">機番</param>
        /// <param name="shijiYM">月度</param>
        /// <param name="katashikiCode">型式コード</param>
        /// <param name="kuniCode">国コード</param>
        /// <param name="katashikiName">型式名</param>
        /// <param name="tokki">特記事項</param>
        /// <param name="targetSagyoKeep">TARGET_SAGYO_KEEP</param>
        /// <param name="pattern">期</param>
        /// <param name="generalPattern">総称パターン</param>
        /// <param name="maxRecordCount">最大検索件数</param>
        /// <param name="result">結果</param>
        /// <returns>取得結果</returns>
        public static DataTable SelectJunjoList( string shijiLevel, string idno, string kiban, string shijiYM, string katashikiCode, string kuniCode, string katashikiName, string tokki, bool targetSagyoKeep, string pattern, string generalPattern, int maxRecordCount, ref OrderBusiness.ResultSet result ) {
            KTBindParameters param = new KTBindParameters();
            param.Add( "shijiLevel", shijiLevel );
            param.Add( "idno", idno );
            param.Add( "kiban", kiban );
            param.Add( "shijiYM", shijiYM );
            param.Add( "katashikiCode", katashikiCode );
            param.Add( "kuniCode", kuniCode );
            param.Add( "katashikiName", katashikiName );
            param.Add( "tokki", tokki );
            param.Add( "targetSagyoKeep", targetSagyoKeep );
            param.Add( "ki", pattern );
            param.Add( "soshoPattern", generalPattern );

            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectJunjoList" );
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