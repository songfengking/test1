using System;
using System.Data;
using System.Collections.Generic;
using KTFramework.Common.Dao;
using KTFramework.Dao;
using KTFramework.Common;
using System.Reflection;
using TRC_W_PWT_ProductView.Defines;

namespace TRC_W_PWT_ProductView.Dao.Com {
    /// <summary>
    /// TBL_TE_立体倉庫ロケーションM用DAO
    /// </summary>
    public class TeRittaiLocationDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "TeRittaiLocation";

        /// <summary>
        /// 立体別在庫検索（1:筑波,2:OEM,3:堺）
        /// </summary>
        /// <returns>立体別在庫検索結果</returns>
        public static DataTable SelectRittaiZaiko() {
            //SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectRittaiZaiko" );
            return GiaDao.GetInstance().Select( statementId );
        }

        /// <summary>
        /// エンジン生産種別在庫検索（1:搭載,2:OEM,3:堺）
        /// </summary>
        /// <returns>エンジン生産種別在庫検索結果</returns>
        public static DataTable SelectSeisanSyubetsuZaiko() {
            //SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectSeisanSyubetsuZaiko" );
            return GiaDao.GetInstance().Select( statementId );
        }

        /// <summary>
        /// 型式別在庫検索
        /// </summary>
        /// <returns>型式別在庫検索結果</returns>
        public static DataTable SelectKatashikiZaiko( string rittaiNum, string stopFlag, string locationFlag, string engineSyubetsu, string tousaiOem, string naigaisaku, string untenFlag, string idno, string kiban, string tokki, string katashikiCode, string katashikiName, ref Business.OrderBusiness.ResultSet result, int maxRecordCount ) {

            KTBindParameters param = new KTBindParameters();
            param.Add( "rittaiNum", rittaiNum );
            param.Add( "stopFlag", stopFlag );
            param.Add( "locationFlag", locationFlag );
            param.Add( "engineSyubetsu", engineSyubetsu );
            param.Add( "tousaiOem", tousaiOem );
            param.Add( "naigaisaku", naigaisaku );
            param.Add( "untenFlag", untenFlag );
            param.Add( "idno", idno );
            param.Add( "kiban", kiban );
            param.Add( "tokki", tokki );
            param.Add( "katashikiCode", katashikiCode );
            param.Add( "katashikiName", katashikiName );

            //SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectKatashikiZaiko" );
            Cursor cursor = GiaDao.GetInstance().OpenCursor( statementId, param );
            DataTable resultTable = null;
            try {
                while ( GiaDao.GetInstance().Fetch( ref resultTable, ref cursor ) ) {
                    if ( resultTable.Rows.Count > maxRecordCount ) {
                        // 表示上限を超えた場合、メッセージを設定し処理を打ち切り
                        result.Message = new Msg( MsgManager.MESSAGE_WRN_61020 );
                        // 表示上限+1になっているので、末尾1件削除
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
        /// 全在庫検索（型式別IDNO一覧用 ソート条件固定検索）
        /// </summary>
        /// <returns>全在庫検索結果</returns>
        public static DataTable SelectRittaiZaikoAllForKatashikiIdnoList( string rittaiNum, string stopFlag, string locationFlag, string engineSyubetsu, string tousaiOem, string naigaisaku, string untenFlag, string idno, string kiban, string tokki, string katashikiCode, string katashikiName ) {

            KTBindParameters param = new KTBindParameters();
            param.Add( "rittaiNum", rittaiNum );
            param.Add( "stopFlag", stopFlag );
            param.Add( "locationFlag", locationFlag );
            param.Add( "engineSyubetsu", engineSyubetsu );
            param.Add( "tousaiOem", tousaiOem );
            param.Add( "naigaisaku", naigaisaku );
            param.Add( "untenFlag", untenFlag );
            param.Add( "idno", idno );
            param.Add( "kiban", kiban );
            param.Add( "tokki", tokki );
            param.Add( "katashikiCode", katashikiCode );
            param.Add( "katashikiName", katashikiName );
            // 型式別IDNO一覧表示用
            param.Add( "searchForKatashikiIdnoListFlag", true );

            //SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectRittaiZaikoAll" );
            return GiaDao.GetInstance().Select( statementId, param );
        }

        /// <summary>
        /// 全在庫検索
        /// </summary>
        /// <returns>全在庫検索結果</returns>
        public static DataTable SelectRittaiZaikoAll( string rittaiNum, string stopFlag, string locationFlag, string engineSyubetsu, string tousaiOem, string naigaisaku, string untenFlag, string idno, string kiban, string tokki, string katashikiCode, string katashikiName, ref Business.OrderBusiness.ResultSet result, int maxRecordCount ) {

            KTBindParameters param = new KTBindParameters();
            param.Add( "rittaiNum", rittaiNum );
            param.Add( "stopFlag", stopFlag );
            param.Add( "locationFlag", locationFlag );
            param.Add( "engineSyubetsu", engineSyubetsu );
            param.Add( "tousaiOem", tousaiOem );
            param.Add( "naigaisaku", naigaisaku );
            param.Add( "untenFlag", untenFlag );
            param.Add( "idno", idno );
            param.Add( "kiban", kiban );
            param.Add( "tokki", tokki );
            param.Add( "katashikiCode", katashikiCode );
            param.Add( "katashikiName", katashikiName );
            // 型式別IDNO一覧表示用
            param.Add( "searchForKatashikiIdnoListFlag", false );

            //SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectRittaiZaikoAll" );
            Cursor cursor = GiaDao.GetInstance().OpenCursor( statementId, param );
            DataTable resultTable = null;
            try {
                while ( GiaDao.GetInstance().Fetch( ref resultTable, ref cursor ) ) {
                    if ( resultTable.Rows.Count > maxRecordCount ) {
                        // 表示上限を超えた場合、メッセージを設定し処理を打ち切り
                        result.Message = new Msg( MsgManager.MESSAGE_WRN_61020 );
                        // 表示上限+1になっているので、末尾1件削除
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
        /// 搭載エンジン検索
        /// </summary>
        /// <returns>搭載エンジン検索結果</returns>
        public static DataTable SelectTousaiEngineList( string tractorKanseiYoteiYMD, string tractorKatashikiCode, string katashikiCode, string katashikiName, string tractorIdno, string tractorKatashikiName, string tractorTokki, int displayKanryoNum, string jissekiStationPrev, string jissekiStationPost, string callEngineWarehouse, ref Business.OrderBusiness.ResultSet result, int maxRecordCount ) {

            KTBindParameters param = new KTBindParameters();
            param.Add( "tractorKanseiYoteiYMD", tractorKanseiYoteiYMD );
            param.Add( "tractorKatashikiCode", tractorKatashikiCode );
            param.Add( "katashikiCode", katashikiCode );
            param.Add( "katashikiName", katashikiName );
            param.Add( "tractorIdno", tractorIdno );
            param.Add( "tractorKatashikiName", tractorKatashikiName );
            param.Add( "tractorTokki", tractorTokki );
            param.Add( "displayKanryoNum", displayKanryoNum );
            param.Add( "jissekiStationPrev", jissekiStationPrev );
            param.Add( "jissekiStationPost", jissekiStationPost );
            param.Add( "callEngineWarehouse", callEngineWarehouse );

            //SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectTousaiEngineList" );
            Cursor cursor = GiaDao.GetInstance().OpenCursor( statementId, param );
            DataTable resultTable = null;
            try {
                while ( GiaDao.GetInstance().Fetch( ref resultTable, ref cursor ) ) {
                    if ( resultTable.Rows.Count > maxRecordCount ) {
                        // 表示上限を超えた場合、メッセージを設定し処理を打ち切り
                        result.Message = new Msg( MsgManager.MESSAGE_WRN_61020 );
                        // 表示上限+1になっているので、末尾1件削除
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