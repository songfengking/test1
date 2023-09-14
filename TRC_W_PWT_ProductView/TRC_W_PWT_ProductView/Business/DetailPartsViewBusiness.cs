using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using KTFramework.Common;
using KTFramework.Dao;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.ListDefine;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Dao.Com;
using TRC_W_PWT_ProductView.Dao.Products;
using TRC_W_PWT_ProductView.Dao.Parts;
using TRC_W_PWT_ProductView.Dao.Process;
using TRC_W_PWT_ProductView.UI.Pages;

namespace TRC_W_PWT_ProductView.Business {
    /// <summary>
    /// 製品詳細検索ビジネスクラス
    /// </summary>
    [Serializable]
    public class DetailPartsViewBusiness {
        //ロガー定義
        private static readonly Logger _logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );
        
        #region 部品検索
        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="productCountryCd">生産国コード</param>
        /// <param name="serial">機番</param>
        /// <returns>製品詳細DataTable(1行のみ)</returns>
        public static DataTable SearchPartsDetail( string modelCd, string serial ) {
            //DataTable tblResult = null;
            AtuDao.SearchParameter param = new AtuDao.SearchParameter();

            _logger.Info( "ATU検索条件--->型式={0} 機番={1}", modelCd, serial );

            param.paramModelCd = modelCd;            //型式コード
            param.paramSerial = serial;              //機番

            //************************************************************************************
            //製品情報設定
            //************************************************************************************
            //param.paramModelType = ModelType.Product;
            //param.paramSerialList = new List<SerialParam>();
            //param.paramSerialList.Add( new SerialParam( modelCd, serial ) );    //型式コード、機番

            //************************************************************************************
            //日付範囲設定
            //************************************************************************************
            //なし

            //検索実行
            DataTable result = AtuDao.SelectAtuList( param, 1 );

            return result;
        }
        #endregion

        #region 詳細情報格納構造体
        /// <summary>
        /// [工程/部品共通]詳細情報格納構造体
        /// </summary>
        [Serializable]
        public struct ResultSet {
            /// <summary>メイン情報</summary>
            public DataTable MainTable { get; set; }
            /// <summary>サブ情報</summary>
            public DataTable SubTable { get; set; }
        }

        #endregion



        #region 工程詳細検索

        /// <summary>
        /// ATU工程[ATU機番管理]詳細検索処理
        /// </summary>
        /// <param name="productModelCd">型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 詳細情報
        /// genericTable  :n件
        /// specificTable :未使用
        /// </returns>
        public static ResultSet SelectAtuSerialDetail( string modelCd, string serial ) {
            ResultSet result = new ResultSet();

            _logger.Info( "ATU機番管理検索--->型式={0} 機番={1}", modelCd, serial );

            //エンジン共通情報(TBL_トルク締付履歴)取得
            result.MainTable = AtuDao.SelectAtuSerial( modelCd, serial );
            //詳細情報(無し)
            result.SubTable = null;

            _logger.Info( "ATU機番管理結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// ATU工程[トルク締付]詳細検索処理
        /// </summary>
        /// <param name="productModelCd">型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 詳細情報
        /// genericTable  :n件
        /// specificTable :未使用
        /// </returns>
        public static ResultSet SelectAtuTorqueDetail( string modelCd, string serial ) {
            ResultSet result = new ResultSet();

            _logger.Info( "トルク締付検索--->型式={0} 機番={1}", modelCd, serial );

            //エンジン共通情報(TBL_トルク締付履歴)取得
            result.MainTable = AtuDao.SelectAtuTorque( modelCd, serial );
            //詳細情報(無し)
            result.SubTable = null;

            _logger.Info( "トルク締付結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// ATU工程[リーク計測]詳細検索処理
        /// </summary>
        /// <param name="productModelCd">型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 詳細情報
        /// genericTable  :n件
        /// specificTable :未使用
        /// </returns>
        public static ResultSet SelectAtuLeakDetail( string modelCd, string serial ) {
            ResultSet result = new ResultSet();

            _logger.Info( "リーク計測検索--->型式={0} 機番={1}", modelCd, serial );

            //エンジン共通情報(TBL_トルク締付履歴)取得
            result.MainTable = AtuDao.SelectAtuLeak( modelCd, serial );
            //詳細情報(無し)
            result.SubTable = null;

            _logger.Info( "リーク計測結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        #endregion

    }



}