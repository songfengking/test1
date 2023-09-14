using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using KTFramework.Dao;
using KTFramework.Common;
using KTFramework.Common.Dao;

namespace TRC_W_PWT_ProductView.Dao.Com {
    public class WmsDpfDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "WmsDpf";

        /// <summary>
        /// 出荷データ取得
        /// </summary>
        /// <param name="serialList"></param>
        /// <returns></returns>
        public static DataTable SelectShipData(List<SerialParam> serialList) {
            KTBindParameters param = new KTBindParameters();
            param.Add("serialList", serialList);
            string statementId = GetFullStatementIdForLibrary(SQLMAP_NAMESPACE, "SelectShipData");
            DataTable resultTable = GiaDao.GetInstance().Select(statementId, param);
            return resultTable;
        }
    }
}