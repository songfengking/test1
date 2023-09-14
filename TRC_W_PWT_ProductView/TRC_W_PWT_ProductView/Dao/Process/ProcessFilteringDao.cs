using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using KTFramework.Common.Dao;
using KTFramework.Dao;

namespace TRC_W_PWT_ProductView.Dao.Process {
    /// <summary>
    /// 工程絞込検索DAO
    /// </summary>
    public class ProcessFilteringDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "ProcessFiltering";

        #region 検索パラメータクラス
        /// <summary>
        /// 工程絞込検索パラメータクラス
        /// </summary>
        public class SearchParameter {
            /// <summary>
            /// ラインコード
            /// </summary>
            public string LineCd { get; set; }
            /// <summary>
            /// 工程名（部分一致）
            /// </summary>
            public string ProcessName { get; set; }
            /// <summary>
            /// 作業名（部分一致）
            /// </summary>
            public string WorkName { get; set; }
        }
        #endregion

        /// <summary>
        /// ライン一覧取得
        /// </summary>
        /// <returns>ライン一覧</returns>
        public static DataTable SelectLineList( string productKnd ) {

            var param = new KTBindParameters();
            param.Add( "productKnd", productKnd );

            // SQL実行
            var statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectLineList" );
            var tblResult = PicDao.GetInstance().Select( statementId, param );
            return tblResult;
        }

        /// <summary>
        /// 工程作業一覧取得
        /// </summary>
        /// <param name="searchParam">検索パラメータ</param>
        /// <returns>工程作業一覧</returns>
        public static DataTable SelectProcessWorkList( SearchParameter searchParam ) {
            // SQL実行
            var statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectProcessWorkList" );
            var param = new KTBindParameters();
            param.Add( "lineCd", searchParam.LineCd );
            param.Add( "processNm", searchParam.ProcessName );
            param.Add( "workNm", searchParam.WorkName );
            var tblResult = PicDao.GetInstance().Select( statementId, param );
            return tblResult;
        }
    }
}