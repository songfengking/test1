using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using KTFramework.Common;
using KTFramework.Common.Dao;
using KTFramework.Dao;

namespace TRC_W_PWT_ProductView.Dao.Com {
    /// <summary>
    /// 汎用テーブル情報DAO
    /// </summary>
    public class GenericTableDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "GenericTable";

        #region 検索パラメータクラス
        /// <summary>
        /// 汎用テーブル検索パラメータクラス
        /// </summary>
        public class SearchParameter {
            /// <summary>
            /// ラインコード
            /// </summary>
            public string LineCd { get; set; }
            /// <summary>
            /// 工程コード
            /// </summary>
            public string ProcessCd { get; set; }
            /// <summary>
            /// 作業コード
            /// </summary>
            public string WorkCd { get; set; }
            /// <summary>
            /// 日付From
            /// </summary>
            public DateTime? DtFrom { get; set; }
            /// <summary>
            /// 日付To
            /// </summary>
            public DateTime? DtTo { get; set; }
            /// <summary>
            /// 型式機番リスト
            /// </summary>
            public List<SerialParam> SerialsList { get; set; }
            /// <summary>
            /// 型式コードリスト
            /// </summary>
            public List<string> ModelCdList { get; set; }
        }
        #endregion

        /// <summary>
        /// 検索対象フラグ取得
        /// </summary>
        /// <param name="searchParam">検索パラメータ</param>
        /// <returns>検索対象フラグ</returns>
        public static int SelectSearchTargetFlag( string lineCd,string processCd ) {
            // SQL実行
            var statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectSearchTargetFlag" );
            var param = new KTBindParameters();
            param.Add( "lineCd", lineCd );
            param.Add( "processCd", processCd );
            return PicDao.GetInstance().Select( statementId, param )
                .AsEnumerable()
                .Select( x => NumericUtils.ToInt( x["SEARCH_TARGET_FLG"] ) )
                .FirstOrDefault();
        }

        /// <summary>
        /// 汎用テーブル情報検索
        /// </summary>
        /// <param name="searchParam">検索パラメータ</param>
        /// <returns>汎用テーブル情報</returns>
        public static DataTable SelectGenericTable( SearchParameter searchParam ) {
            // SQL実行
            var statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectGenericTable" );
            var param = new KTBindParameters();
            param.Add( "lineCd", searchParam.LineCd );
            param.Add( "processCd", searchParam.ProcessCd );
            param.Add( "workCd", searchParam.WorkCd );
            param.Add( "dtFrom", searchParam.DtFrom );
            param.Add( "dtTo", searchParam.DtTo );
            param.Add( "serialsList", searchParam.SerialsList );
            param.Add( "modelCdList", searchParam.ModelCdList );
            var tblResult = PicDao.GetInstance().Select( statementId, param );
            return tblResult;
        }
    }
}