using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using KTFramework.Common.Dao;
using KTFramework.Dao;

namespace TRC_W_PWT_ProductView.Dao.Com {
    /// <summary>
    /// ユーザ一覧情報DAO
    /// </summary>
    public class TmCcAuthUserDao : DaoBase {
        #region 定数
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "TmCcAuthUser";
        #endregion

        /// <summary>
        /// 電子かんばんユーザ情報取得
        /// </summary>
        /// <returns></returns>
        public static DataTable GetKanbanPickingUserInfo() {
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, nameof( GetKanbanPickingUserInfo ) );
            return GiaDao.GetInstance().Select( statementId );
        }
    }
}