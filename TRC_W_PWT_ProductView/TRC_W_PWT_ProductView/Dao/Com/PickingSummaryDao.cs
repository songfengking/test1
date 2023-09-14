using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KTFramework.Common.Dao;
using KTFramework.Dao;
using KTFramework.Common;

namespace TRC_W_PWT_ProductView.Dao.Com {
    /// <summary>
    /// ピッキング状況情報DAO
    /// </summary>
    public class PickingSummaryDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "PickingSummary";

        /// <summary>
        /// ピッキング状況情報取得
        /// </summary>
        /// <param name="sendFrom">要求日時From</param>
        /// <param name="sendTo">要求日時To</param>
        /// <param name="endFrom">完了日時From</param>
        /// <param name="endTo">完了日時To</param>
        /// <param name="status">状況</param>
        /// <param name="areaName">エリア</param>
        /// <param name="userId">ピッキング者</param>
        /// <param name="pickingNo">ピッキングNo</param>
        /// <param name="partsNumber">品番</param>
        /// <returns>DataTable</returns>
        public static DataTable SelectPickingInfo( DateTime? sendFrom, DateTime? sendTo, DateTime? endFrom, DateTime? endTo, List<string> status, string areaName, string userId, string pickingNo, string partsNumber ) {
            try {
                // SQL実行
                string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectPickingSummary" );

                KTBindParameters param = new KTBindParameters();
                param.Add( "sendFrom", sendFrom );
                param.Add( "sendTo", sendTo );
                param.Add( "endFrom", endFrom );
                param.Add( "endTo", endTo );
                param.Add( "status", status );
                param.Add( "areaName", areaName );
                param.Add( "userId", userId );
                param.Add( "pickingNo", pickingNo );
                param.Add( "partsNumber", partsNumber );
                DataTable tblResult = GiaDao.GetInstance().Select( statementId, param );

                return tblResult;
            } catch ( Exception ex ) {
                throw ex;
            }
        }

        /// <summary>
        /// 未完了ピッキング検索処理
        /// </summary>
        /// <param name="sendFrom">要求日時From</param>
        /// <param name="sendTo">要求日時To</param>
        /// <param name="areaName">エリア</param>
        /// <param name="primaryLocation">材管ロケ大番地</param>
        /// <param name="secondaryLocation">材管ロケ中番地</param>
        /// <param name="tertiaryLocation">材管ロケ小番地</param>
        /// <param name="userId">ピッキング者</param>
        /// <param name="partsNumber">品番</param>
        /// <param name="pickingNo">ピッキングNo</param>
        /// <returns>検索結果</returns>
        public static DataTable SelectIncompletePicking( DateTime? sendFrom, DateTime? sendTo, string areaName, string primaryLocation, string secondaryLocation, string tertiaryLocation, string userId, string partsNumber, string pickingNo ) {
            var param = new KTBindParameters();
            param.Add( nameof( sendFrom ), sendFrom );
            param.Add( nameof( sendTo ), sendTo );
            param.Add( nameof( areaName ), areaName );
            param.Add( nameof( primaryLocation ), primaryLocation );
            param.Add( nameof( secondaryLocation ), secondaryLocation );
            param.Add( nameof( tertiaryLocation ), tertiaryLocation );
            param.Add( nameof( userId ), userId );
            param.Add( nameof( partsNumber ), partsNumber );
            param.Add( nameof( pickingNo ), pickingNo );
            // SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, nameof( SelectIncompletePicking ) );
            return GiaDao.GetInstance().Select( statementId, param );
        }
    }
}