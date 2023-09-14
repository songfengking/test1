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
    /// EXCELテンプレートDAO
    /// </summary>
    public class TmCcExcelTemplateDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "TmCcExcelTemplate";

        /// <summary>
        /// EXCELテンプレート取得
        /// </summary>
        /// <returns></returns>
        public static DataRow SelectExcelTemplate(string appId, string excelId) {

            DataRow rowResult = null;
            DataTable tblResult = null;

            // パラメータの設定
            KTBindParameters param = new KTBindParameters();
            param.Add("appId", appId);
            param.Add("excelId", excelId);

            string statementId = GetFullStatementIdForLibrary(SQLMAP_NAMESPACE, "SelectExcelTemplate");
            tblResult = GiaDao.GetInstance().Select(statementId, param);

            if (ObjectUtils.IsNotNull(tblResult) && tblResult.Rows.Count > 0) {
                rowResult = tblResult.Rows[0];
            }
            return rowResult;
        }
    }
}
