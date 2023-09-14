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
    /// アクセスカウンターDAO
    /// </summary>
    public class AccessCounterDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "AccessCounter";

        /// <summary>ページID MAX長</summary>
        private const int PAGE_ID_MAX = 20;
        
        /// <summary>
        /// アクセスカウンタ登録 UpdateOrInsert
        /// </summary>
        /// <param name="pageId">表示ページID</param>
        /// <param name="userId">表示ユーザID</param>
        /// <returns>影響件数</returns>
        /// <remarks>
        /// 登録時異常があっても、画面表示はシステムエラーとしない
        /// </remarks>
        public static int Entry( string pageId, string userId = Defines.User.Guest.GUEST_ID  ) {

            string entryDate = DateUtils.ToString( DateTime.Today, DateUtils.FormatType.DayNoSep);
            KTBindParameters bindParam = new KTBindParameters();

            //検索対象テーブル条件
            bindParam.Add( "applicationId", WebAppInstance.KTAUTH_AP_MASTER_ID ); //アプリケーションID
            bindParam.Add( "pageId", StringUtils.CutString( pageId, 20 ));        //ページID
            bindParam.Add( "sumDate", entryDate );                                //集約日
            bindParam.Add( "userId", userId );                                    //ユーザID
            
            int result = 0;
            try {
                GiaDao.GetInstance().BeginTransaction();
                result = Update( bindParam );
                if ( result == 0 ) {
                    result = Insert( bindParam );
                }
                GiaDao.GetInstance().CommitTransaction();
            } catch ( DataAccessException ex ) {
                if ( ex.OracleErrorNumber == DataAccessBase.ORAERROR_TIMEOUT ) {
                    //クエリ発行タイムアウト
                } else {
                    //タイムアウト以外のException                    
                }
                result = -1;
            } catch ( Exception ex ) {
                result = -1;
            } finally {
                if ( true == GiaDao.GetInstance().IsTransaction ) {
                    try {
                        GiaDao.GetInstance().RollbackTransaction();
                    } catch ( Exception ex ) {
                        result = -1;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="param">バインドパラメータ</param>
        /// <returns>影響件数</returns>
        private static int Insert( KTBindParameters param ) {
            //SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "Insert" );
            return GiaDao.GetInstance().Exec(statementId, param);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="param">バインドパラメータ</param>
        /// <returns>影響件数</returns>
        private static int Update( KTBindParameters param ) {
            //SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "Update" );
            return GiaDao.GetInstance().Exec( statementId, param );
        }
    }
}