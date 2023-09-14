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
    /// 従業員情報DAO
    /// </summary>
    public class KintaiDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "Kintai";

        /// <summary>
        /// 従業員情報取取得
        /// </summary>
        /// <param name="dateTime">日付</param>
        /// <param name="userId">従業員No</param>
        /// <param name="syozoku">所属ライン番号</param>
        /// <returns>DataTable</returns>
        public static DataTable SelectByList(String userId, String syozoku) {
            //SQL実行
            string statementId = GetFullStatementIdForLibrary(SQLMAP_NAMESPACE, "select");

            KTBindParameters param = new KTBindParameters();
            param.Add("UserId", userId);
            param.Add("Syozoku", syozoku);
            DataTable tblResult = GiaDao.GetInstance().Select(statementId, param);

            return tblResult;
        }
    }
}