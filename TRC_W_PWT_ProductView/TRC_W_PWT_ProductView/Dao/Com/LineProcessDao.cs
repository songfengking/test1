using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KTFramework.Common.Dao;
using KTFramework.Dao;
using KTFramework.Common;
using System.Data;

namespace TRC_W_PWT_ProductView.Dao.Com {
    public class LineProcessDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "LineProcess";

        #region ライン工程一覧取得
        /// <summary>
        /// ライン工程一覧取得
        /// </summary>
        /// <param name="productKind">製品種別</param>
        /// <param name="assemblyPatternCd">組立パターンコード</param>
        /// <returns>ライン工程リストDataTable</returns>
        public static DataTable SelectLineProcessList( string productKind, string assemblyPatternCd ) {
            // SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectLineProcessList" );
            KTBindParameters param = new KTBindParameters();
            param.Add( "productKind", productKind );
            param.Add( "assemblyPatternCd", assemblyPatternCd );
            return PicDao.GetInstance().Select( statementId, param );
        }
        #endregion

    }
}