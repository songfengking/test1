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
    /// 型式DAO(KATM)
    /// </summary>
    public class ModelDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "Model";

        /// <summary>
        /// 型式名からの型式コード一覧逆引き
        /// </summary>
        /// <param name="modelNm">型式名(前方一致)</param>
        /// <returns>型式一覧</returns>
        public static DataTable GetModelCdListByName(string modelNm) {
            //SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectModelCdListByName" );

            KTBindParameters param = new KTBindParameters();
            param.Add( "modelNm", modelNm );

            return GiaDao.GetInstance().Select( statementId, param );
        }

        /// <summary>
        /// 型式情報取得
        /// </summary>
        /// <param name="modelCd">型式コード</param>
        /// <param name="countryCd">国コード</param>
        /// <returns>型式情報DataTable</returns>
        public static DataTable GetModelInfo( string modelCd, string countryCd ) {
            //SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectModelList" );

            KTBindParameters param = new KTBindParameters();
            param.Add( "modelCd", modelCd );
            param.Add( "countryCd", countryCd );

            return GiaDao.GetInstance().Select( statementId, param );
        }

        /// <summary>
        /// 組立パターン(2桁)取得
        /// </summary>
        /// <param name="modelCd">型式コード</param>
        /// <param name="countryCd">国コード</param>
        /// <returns>組立パターン(2桁)</returns>
        public static string GetAssemblyPatternCd( string modelCd, string countryCd ) {
            string assemblyPatternCd = null;
            DataTable tblModel = GetModelInfo( modelCd, countryCd );
            if ( 1 == tblModel.Rows.Count ) {
                assemblyPatternCd = 
                    StringUtils.ToString( tblModel.Rows[0]["assemblyPatternCd"] ) + 
                    StringUtils.ToString( tblModel.Rows[0]["assemblySubPatternCd"] );
            }
            return assemblyPatternCd;
        }
    }
}