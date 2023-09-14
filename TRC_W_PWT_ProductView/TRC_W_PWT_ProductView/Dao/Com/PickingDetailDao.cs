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
    /// ピッキング明細情報DAO
    /// </summary>
    public class PickingDetailDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "PickingDetail";

        /// <summary>
        /// ピッキング明細情報取得
        /// </summary>
        /// <param name="pickingNo">ピッキングNo</param>
        /// <returns>DataTable</returns>
        public static DataTable SelectDetailInfo(String pickingNo) {
            try {
                //SQL実行
                string statementId = GetFullStatementIdForLibrary(SQLMAP_NAMESPACE, "SelectPickingDetail");

                KTBindParameters param = new KTBindParameters();
                param.Add("pickingNo", pickingNo);
                DataTable tblResult = GiaDao.GetInstance().Select(statementId, param);

                return tblResult;
            } catch (Exception ex) {
                throw ex;
            }
        }
    }
}