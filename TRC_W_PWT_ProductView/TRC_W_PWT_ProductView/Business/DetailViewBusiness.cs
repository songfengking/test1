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
    public class DetailViewBusiness {
        //ロガー定義
        private static readonly Logger _logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        //定数
        //テーブルINDEX
        /// <summary>電子チェックシート：検査情報、刻印：前車軸</summary>
        private const Int32 TBL_IDX0 = 0;
        /// <summary>電子チェックシート：不具合一覧、刻印：ミッション</summary>
        private const Int32 TBL_IDX1 = 1;
        /// <summary>電子チェックシート：検査画像、刻印：MID</summary>
        private const Int32 TBL_IDX2 = 2;
        /// <summary>電子チェックシート：不具合画像</summary>
        private const Int32 TBL_IDX3 = 3;


        #region 製品検索
        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="productCountryCd">生産国コード</param>
        /// <param name="serial">機番</param>
        /// <returns>製品詳細DataTable(1行のみ)</returns>
        public static DataTable SearchProductDetail( string productModelCd, string productCountryCd, string serial ) {
            DataTable tblResult = null;
            ProductsDao.SearchParameter param = new ProductsDao.SearchParameter();

            _logger.Info( "製品検索条件--->型式={0} 国={1} 機番={2}", productModelCd, productCountryCd, serial );

            //型式から組立パターンを取得
            string assemblyPatternCd = ModelDao.GetAssemblyPatternCd( productModelCd, productCountryCd );
            if ( StringUtils.IsBlank( assemblyPatternCd ) ) {
                _logger.Error( "組立パターン検索失敗" );
                return tblResult;
            }

            //************************************************************************************
            //検索対象テーブル設定(組立パターンから製品区分を判定し、検索対象テーブルを決定する)
            //************************************************************************************
            int searchProductingTractor = 0;        //生産中トラクタ参照
            int searchProductingEngine = 0;         //生産中エンジン参照

            int unionStockProducts = 0;             //在庫製品結合
            int searchStockTractor = 0;             //在庫トラクタ参照
            int searchStockEngine = 0;              //在庫エンジン参照
            int searchStockRotary = 0;              //生産中ロータリー参照

            int unionShippedProducts = 0;           //出荷済製品結合
            int searchShippedTractor = 0;           //出荷済トラクタ参照
            int searchShippedEngine = 0;            //出荷済エンジン参照
            int searchShippedRotary = 0;            //出荷済ロータリー参照

            if ( ProductKind.Tractor == AssemblyPatternCode.GetGeneralPatternCd( assemblyPatternCd ) ) {
                //トラクタ
                searchProductingTractor = 1;        //作業指示保存
                unionStockProducts = 1;             //UNION式を付加
                searchStockTractor = 1;             //BIDM
                unionShippedProducts = 1;           //UNION式を付加
                searchShippedTractor = 1;           //機番情報F
                unionShippedProducts = 1;
            } else if ( ProductKind.Engine == AssemblyPatternCode.GetGeneralPatternCd( assemblyPatternCd ) ) {
                //エンジン
                searchProductingEngine = 1;         //作業指示保存
                unionStockProducts = 1;             //UNION式を付加
                searchStockEngine = 1;              //BIDM
                unionShippedProducts = 1;           //UNION式を付加
                searchShippedEngine = 1;            //機番情報F
            } else if ( ProductKind.Rotary == AssemblyPatternCode.GetGeneralPatternCd( assemblyPatternCd ) ) {
                //ロータリー
                searchStockRotary = 1;              //BIDM
                unionShippedProducts = 1;           //UNION式を付加
                searchShippedRotary = 1;            //機番情報F
            } else {
                //製品ではない(販売型式コード？)
                _logger.Warn( "不正な型式指定 型式={0} 国={1}", productModelCd, productCountryCd );
                return tblResult;
            }
            param.searchProductingTractor = searchProductingTractor;            //生産中トラクタ参照
            param.searchProductingEngine = searchProductingEngine;              //生産中エンジン参照
            param.unionStockProducts = unionStockProducts;                      //在庫製品UNION結合
            param.searchStockTractor = searchStockTractor;                      //在庫トラクタ参照
            param.searchStockEngine = searchStockEngine;                        //在庫エンジン参照
            param.searchStockRotary = searchStockRotary;                        //在庫ロータリー参照
            param.unionShippedProducts = unionShippedProducts;                  //在庫製品UNION結合
            param.searchShippedTractor = searchShippedTractor;                  //在庫トラクタ参照
            param.searchShippedEngine = searchShippedEngine;                    //在庫エンジン参照
            param.searchShippedRotary = searchShippedRotary;                    //在庫ロータリー参照
            param.paramProductStatus = 0;                                      //作業指示保存検索対象:未出荷(仕掛を含む)
            //************************************************************************************
            //部品テーブルの結合有無(クレート/ロプスのみ)
            //************************************************************************************
            //なし(製品のみ抽出)
            param.appendPartsCd = null;

            //************************************************************************************
            //製品情報設定
            //************************************************************************************
            param.paramModelType = ModelType.Product;
            //改造前生産型式で検索
            param.searchFirstProduct = 1;
            param.paramSerialList = new List<SerialParam>();
            param.paramSerialList.Add( new SerialParam( productModelCd, serial ) );    //型式コード、機番

            //************************************************************************************
            //日付範囲設定
            //************************************************************************************
            //なし

            //************************************************************************************
            //部品情報設定(クレート/ロプスのみ)
            //************************************************************************************
            //なし

            //検索実行
            DataTable result = ProductsDao.SelectList( param, 1 );
            if ( null != result && 1 == result.Rows.Count ) {
                //改造(パターン=99)のように不明な総称パターンの場合は組立パターンから逆引きする
                string generalPatternCd = StringUtils.ToString( result.Rows[0]["generalPatternCd"] );
                if ( GeneralPatternCode.Tractor != generalPatternCd && GeneralPatternCode.Engine != generalPatternCd && GeneralPatternCode.Imple != generalPatternCd ) {
                    if ( AssemblyPatternCode.Tractor == assemblyPatternCd ) {
                        result.Rows[0]["generalPatternCd"] = GeneralPatternCode.Tractor;
                    } else if ( AssemblyPatternCode.OemEngine03 == assemblyPatternCd || AssemblyPatternCode.OemEngine07 == assemblyPatternCd || AssemblyPatternCode.InstalledEngine03 == assemblyPatternCd || AssemblyPatternCode.InstalledEngine07 == assemblyPatternCd ) {
                        result.Rows[0]["generalPatternCd"] = GeneralPatternCode.Engine;
                    }
                    result.AcceptChanges();
                }
            }

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

        /// <summary>
        /// [エンジン運転結果]詳細情報格納構造体
        /// </summary>
        [Serializable]
        public struct ResultSetEngineTest {
            /// <summary>メイン情報</summary>
            public DataTable MainTable { get; set; }
            /// <summary>サブ情報</summary>
            public DataTable[] SubTables { get; set; }
        }

        /// <summary>
        /// [パワクロ運転結果]詳細情報格納構造体
        /// </summary>
        [Serializable]
        public struct ResultSetPCrawler {
            /// <summary>メイン情報</summary>
            public DataTable MainTable { get; set; }
            /// <summary>サブ情報</summary>
            public DataTable[] SubTables { get; set; }
        }

        /// <summary>
        /// [工程]詳細情報格納構造体(サブ複数)
        /// </summary>
        [Serializable]
        public struct ResultSetMulti {
            /// <summary>メイン情報</summary>
            public DataTable MainTable { get; set; }
            /// <summary>サブ情報</summary>
            public DataTable[] SubTables { get; set; }
        }

        /// <summary>
        /// [共用工程]詳細情報格納構造体
        /// </summary>
        [Serializable]
        public struct ResultSetGeneric {
            /// <summary>来歴情報</summary>
            public DataTable HistoryInfo { get; set; }
            /// <summary>計測項目情報</summary>
            public DataTable[] MeasuringItemInfo { get; set; }
            /// <summary>計測結果情報</summary>
            public DataTable[] MeasuringResultInfo { get; set; }
        }
        #endregion

        #region 部品詳細検索

        #region トラクタ
        /// <summary>
        /// トラクタ部品[WI-FI ECU]詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 取得結果
        /// MainTable  :0～n件
        /// SubTable   :未使用
        /// </returns>
        public static ResultSet SelectTractorWifiEcuDetail( string productModelCd, string serial ) {
            ResultSet result = new ResultSet();

            _logger.Info( "WiFiECU検索--->型式={0} 機番={1}", productModelCd, serial );

            //WifiECU情報(PAIRRSLT_F)取得
            result.MainTable = TractorPartsDao.SelectTractorWifiEcuDetail( productModelCd, serial );
            //サブ情報(無し)
            result.SubTable = null;

            _logger.Info( "WiFiECU結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// トラクタ部品[クレート]詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="idno">IDNO</param>
        /// <returns>
        /// 取得結果
        /// MainTable  :0～1件
        /// SubTable   :未使用
        /// </returns>
        public static ResultSet SelectTractorCrateDetail( string productModelCd, string idno ) {
            ResultSet result = new ResultSet();

            _logger.Info( "クレート検索--->型式={0} IDNO={1}", productModelCd, idno );

            //クレート情報(TBL_WMSクレート機番)取得
            result.MainTable = TractorPartsDao.SelectTractorCrateDetail( idno, productModelCd );
            //サブ情報(無し)
            result.SubTable = null;

            _logger.Info( "クレート結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// トラクタ部品[ロプス]詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="idno">IDNO</param>
        /// <returns>
        /// 取得結果
        /// MainTable  :0～1件
        /// SubTable   :未使用
        /// </returns>
        public static ResultSet SelectTractorRopsDetail( string productModelCd, string idno ) {
            ResultSet result = new ResultSet();

            _logger.Info( "ロプス検索条件--->型式={0} IDNO={1}", productModelCd, idno );

            //ロプス情報(ロプス機番)取得
            result.MainTable = TractorPartsDao.SelectTractorRopsDetail( idno, productModelCd );
            //サブ情報(無し)
            result.SubTable = null;

            _logger.Info( "ロプス結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// トラクタ部品[銘板ラベル]詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="idno">機番</param>
        /// <returns>
        /// 取得結果
        /// MainTable  :0～1件
        /// SubTable   :未使用
        /// </returns>
        public static ResultSet SelectTractorNameplate( string productModelCd, string serail ) {
            ResultSet result = new ResultSet();

            _logger.Info( "銘板ラベル検索条件--->型式={0} 機番={1}", productModelCd, serail );

            List<SerialParam> serialsList = new List<SerialParam>();
            serialsList.Add( new SerialParam( productModelCd, serail ) );
            TractorPartsDao.SearchParameter param = new TractorPartsDao.SearchParameter();
            param.paramSerialList = serialsList;


            //銘板ラベル情報取得
            result.MainTable = TractorPartsDao.SelectTractorNameplateList( param );
            //サブ情報(無し)
            result.SubTable = null;

            _logger.Info( "銘板ラベル結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// トラクタ部品[ミッション]詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 取得結果
        /// MainTable  :0～1件
        /// SubTable   :未使用
        /// </returns>
        public static ResultSet SelectTractorMissionDetail( string productModelCd, string serial ) {
            ResultSet result = new ResultSet();

            _logger.Info( "ミッション検索条件--->型式={0} 機番={1}", productModelCd, serial );

            //ミッション情報取得
            result.MainTable = TractorPartsDao.SelectTractorMissionDetail( productModelCd, serial );
            //サブ情報(無し)
            result.SubTable = null;

            _logger.Info( "ミッション結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// トラクタ部品[ハウジング]詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 取得結果
        /// MainTable  :0～1件
        /// SubTable   :未使用
        /// </returns>
        public static ResultSet SelectTractorHousingDetail( string productModelCd, string serial ) {
            ResultSet result = new ResultSet();

            _logger.Info( "ハウジング検索条件--->型式={0} 機番={1}", productModelCd, serial );

            //ハウジング情報取得
            result.MainTable = TractorPartsDao.SelectTractorHousingDetail( productModelCd, serial );
            //サブ情報(無し)
            result.SubTable = null;

            _logger.Info( "ハウジング結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// トラクタ部品[基幹部品]詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="idno">機番</param>
        /// <returns>
        /// 取得結果
        /// MainTable  :0～複数件
        /// SubTable   :未使用
        /// </returns>
        public static ResultSet SelectTractorCorePartsDetail( string partsCd, string productModelCd, string serail ) {
            ResultSet result = new ResultSet();

            _logger.Info( "基幹部品検索条件--->部品区分={0} 型式={1} 機番={2}", partsCd, productModelCd, serail );

            //基幹部品情報取得
            result.MainTable = TractorPartsDao.SelectTractorCorePartsDetail( partsCd, productModelCd, serail );
            //サブ情報(無し)
            result.SubTable = null;

            _logger.Info( "基幹部品結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        #endregion

        #region エンジン
        /// <summary>
        /// エンジン部品[サプライポンプ]詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 取得結果
        /// MainTable  :0～n件
        /// SubTable   :未使用
        /// </returns>
        public static ResultSet SelectEngineSupplyPumpDetail( string productModelCd, string serial ) {
            ResultSet result = new ResultSet();
            string partsCd = PartsKind.PARTS_CD_ENGINE_SUPPLYPUMP;  //部品区分

            _logger.Info( "サプライポンプ検索--->型式={0} 機番={1}", productModelCd, serial );

            //エンジン共通情報(ENGBKJ_F)取得(来歴含む)
            result.MainTable = EnginePartsDao.SelectEnginePartsDetail( partsCd, productModelCd, serial, false );
            //詳細情報取得(無し)
            result.SubTable = null;

            _logger.Info( "サプライポンプ結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// エンジン部品[ECU]詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 取得結果
        /// MainTable  :0～1件(最新データ)
        /// SubTable   :0～n件
        /// </returns>
        public static ResultSet SelectEngineEcuDetail( string productModelCd, string serial ) {
            ResultSet result = new ResultSet();
            string partsCd = PartsKind.PARTS_CD_ENGINE_ECU;  //部品区分

            _logger.Info( "ECU検索--->型式={0} 機番={1}", productModelCd, serial );

            //エンジン共通情報(ENGBKJ_F)取得(最新のみ)
            result.MainTable = EnginePartsDao.SelectEnginePartsDetail( partsCd, productModelCd, serial, true );
            //詳細情報(ECURET_F)取得
            result.SubTable = EnginePartsDao.SelectEngineEcuDetail( productModelCd, serial );

            _logger.Info( "ECU結果<---MAIN:{0}件 SUB:{1}件", result.MainTable.Rows.Count, result.SubTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// エンジン部品[インジェクター]詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 取得結果
        /// MainTable  :0～1件(最新データ)
        /// SubTable   :0～n件
        /// </returns>
        public static ResultSet SelectEngineInjecterDetail( string productModelCd, string serial ) {
            ResultSet result = new ResultSet();
            string partsCd = PartsKind.PARTS_CD_ENGINE_INJECTOR;  //部品区分

            _logger.Info( "インジェクタ検索--->型式={0} 機番={1}", productModelCd, serial );

            //エンジン共通情報(ENGBKJ_F)取得(全件)
            result.MainTable = EnginePartsDao.SelectEnginePartsDetail( partsCd, productModelCd, serial, false );
            //詳細情報(INJCRT_F)取得
            result.SubTable = EnginePartsDao.SelectEngineInjecterDetail( productModelCd, serial );

            _logger.Info( "インジェクタ結果<---MAIN:{0}件 SUB:{1}件", result.MainTable.Rows.Count, result.SubTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// エンジン部品[DPF]詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 取得結果
        /// MainTable  :0～n件(最新データ)
        /// SubTable   :未使用
        /// </returns>
        public static ResultSet SelectEngineDpfDetail( string productModelCd, string serial ) {
            ResultSet result = new ResultSet();
            string partsCd = PartsKind.PARTS_CD_ENGINE_DPF;  //部品区分

            _logger.Info( "DPF検索--->型式={0} 機番={1}", productModelCd, serial );

            //エンジン共通情報(ENGBKJ_F)取得(来歴含む)
            result.MainTable = EnginePartsDao.SelectEnginePartsDetail( partsCd, productModelCd, serial, true );
            //詳細情報(無し)
            result.SubTable = null;

            _logger.Info( "DPF結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// エンジン部品[SCR]詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 取得結果
        /// MainTable  :0～n件(最新データ)
        /// SubTable   :未使用
        /// </returns>
        public static ResultSet SelectEngineSrcDetail( string productModelCd, string serial ) {
            ResultSet result = new ResultSet();
            string partsCd = PartsKind.PARTS_CD_ENGINE_SCR;  //部品区分

            _logger.Info( "SCR検索--->型式={0} 機番={1}", productModelCd, serial );

            //エンジン共通情報(ENGBKJ_F)取得(来歴含む)
            result.MainTable = EnginePartsDao.SelectEnginePartsDetail( partsCd, productModelCd, serial, true );
            //詳細情報(無し)
            result.SubTable = null;

            _logger.Info( "SCR結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }
        /// <summary>
        /// エンジン部品[DOC]詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 取得結果
        /// MainTable  :0～n件(最新データ)
        /// SubTable   :未使用
        /// </returns>
        public static ResultSet SelectEngineDocDetail( string productModelCd, string serial ) {
            ResultSet result = new ResultSet();
            string partsCd = PartsKind.PARTS_CD_ENGINE_DOC;  //部品区分

            _logger.Info( "DOC検索--->型式={0} 機番={1}", productModelCd, serial );

            //エンジン共通情報(ENGBKJ_F)取得(来歴含む)
            result.MainTable = EnginePartsDao.SelectEnginePartsDetail( partsCd, productModelCd, serial, true );
            //詳細情報(無し)
            result.SubTable = null;

            _logger.Info( "DOC結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// エンジン部品[ACU]詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 取得結果
        /// MainTable  :0～n件(最新データ)
        /// SubTable   :未使用
        /// </returns>
        public static ResultSet SelectEngineAcuDetail( string productModelCd, string serial ) {
            ResultSet result = new ResultSet();
            string partsCd = PartsKind.PARTS_CD_ENGINE_ACU;  //部品区分

            _logger.Info( "ACU検索--->型式={0} 機番={1}", productModelCd, serial );

            //エンジン共通情報(ENGBKJ_F)取得(来歴含む)
            result.MainTable = EnginePartsDao.SelectEnginePartsDetail( partsCd, productModelCd, serial, true );
            //詳細情報(無し)
            result.SubTable = null;

            _logger.Info( "ACU結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// エンジン部品[3C]詳細検索処理
        /// </summary>
        /// <param name="partsCd">部品区分コード</param>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>部品詳細DataTable(1行)</returns>
        public static ResultSet SelectEngine3cPartsDetail( string partsCd, string productModelCd, string serial ) {
            ResultSet result = new ResultSet();
            EnginePartsDao.SearchParameter param = new EnginePartsDao.SearchParameter();

            _logger.Info( "3C部品検索--->部品区分={0} 型式={1} 機番={2}", partsCd, productModelCd, serial );

            //検索条件設定
            param.paramPartsCd = partsCd;                                                  //部品区分コード
            param.paramSerialList = new List<SerialParam>();
            param.paramSerialList.Add( new SerialParam( productModelCd, serial ) );         //型式/機番

            //ENGBKJ_F検索(最新のみ)…一覧検索処理と共用
            result.MainTable = EnginePartsDao.Select3cList( param, 1 );
            //詳細情報はENGBKJ_FとJOINして取得
            result.SubTable = null;

            _logger.Info( "3C部品結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// エンジン部品[EPR]詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 詳細情報
        /// genericTable  :n件
        /// specificTable :未使用
        /// </returns>
        public static ResultSet SelectEngineEprDetail( string productModelCd, string serial ) {
            ResultSet result = new ResultSet();
            string partsCd = PartsKind.PARTS_CD_ENGINE_EPR;  //部品区分

            _logger.Info( "EPR検索--->型式={0} 機番={1}", productModelCd, serial );

            //エンジン共通情報(ENGBKJ_F)取得(来歴含む)
            result.MainTable = EnginePartsDao.SelectEnginePartsDetail( partsCd, productModelCd, serial, false );
            //詳細情報(無し)
            result.SubTable = null;

            _logger.Info( "EPR結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// エンジン部品[MIXER]詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 詳細情報
        /// genericTable  :n件
        /// specificTable :未使用
        /// </returns>
        public static ResultSet SelectEngineMixerDetail( string productModelCd, string serial ) {
            ResultSet result = new ResultSet();
            string partsCd = PartsKind.PARTS_CD_ENGINE_MIXER;  //部品区分

            _logger.Info( "MIXER検索--->型式={0} 機番={1}", productModelCd, serial );

            //エンジン共通情報(ENGBKJ_F)取得(来歴含む)
            result.MainTable = EnginePartsDao.SelectEnginePartsDetail( partsCd, productModelCd, serial, false );
            //詳細情報(無し)
            result.SubTable = null;

            _logger.Info( "MIXER結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// エンジン機番ラベル印刷詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 取得結果
        /// MainTable  :0～n件(最新データ)
        /// SubTable   :未使用
        /// </returns>
        public static ResultSet SelectSerialPrint( string productModelCd, string serial ) {
            ResultSet result = new ResultSet();

            _logger.Info( "機番ラベル印刷検索--->型式={0} 機番={1}", productModelCd, serial );

            //エンジン機番ラベル印刷取得(来歴含む)
            result.MainTable = EnginePartsDao.SelectSerialPrint( productModelCd, serial );
            //詳細情報(無し)
            result.SubTable = null;

            _logger.Info( "機番ラベル印刷結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// エンジン部品[ラック位置センサ]詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 詳細情報
        /// genericTable  :n件
        /// specificTable :未使用
        /// </returns>
        public static ResultSet SelectEngineRackPositionSensorDetail( string productModelCd, string serial ) {
            ResultSet result = new ResultSet();
            string partsCd = PartsKind.PARTS_CD_ENGINE_RACK_POSITION_SENSOR;  //ラック位置センサ

            _logger.Info( "ラック位置センサ検索--->型式={0} 機番={1}", productModelCd, serial );

            //エンジン共通情報(ENGBKJ_F)取得(来歴含む)
            result.MainTable = EnginePartsDao.SelectEnginePartsDetail( partsCd, productModelCd, serial, false );
            result.SubTable = null;

            _logger.Info( "ラック位置センサ結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// エンジン部品[IPU]詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 取得結果
        /// MainTable  :0～n件
        /// SubTable   :未使用
        /// </returns>
        public static ResultSet SelectEngineIpuDetail( string productModelCd, string serial ) {
            ResultSet result = new ResultSet();
            string partsCd = PartsKind.PARTS_CD_ENGINE_IPU;  //部品区分

            _logger.Info( "IPU検索--->型式={0} 機番={1}", productModelCd, serial );

            //エンジン共通情報(ENGBKJ_F)取得(来歴含む)
            result.MainTable = EnginePartsDao.SelectEnginePartsDetail( partsCd, productModelCd, serial, false );
            //詳細情報取得(無し)
            result.SubTable = null;

            _logger.Info( "IPU検索<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// エンジン部品[EHC]詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 取得結果
        /// MainTable  :0～n件
        /// SubTable   :未使用
        /// </returns>
        public static ResultSet SelectEngineEhcDetail( string productModelCd, string serial ) {
            ResultSet result = new ResultSet();
            string partsCd = PartsKind.PARTS_CD_ENGINE_EHC;  //部品区分

            _logger.Info( "EHC検索--->型式={0} 機番={1}", productModelCd, serial );

            //エンジン共通情報(ENGBKJ_F)取得(来歴含む)
            result.MainTable = EnginePartsDao.SelectEnginePartsDetail( partsCd, productModelCd, serial, false );
            //詳細情報取得(無し)
            result.SubTable = null;

            _logger.Info( "EHC検索<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// エンジン部品[基幹部品]詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="idno">機番</param>
        /// <returns>
        /// 取得結果
        /// MainTable  :0～複数件
        /// SubTable   :未使用
        /// </returns>
        public static ResultSet SelectEngineCorePartsDetail( string partsCd, string productModelCd, string serail ) {
            ResultSet result = new ResultSet();

            _logger.Info( "基幹部品検索条件--->部品区分={0} 型式={1} 機番={2}", partsCd, productModelCd, serail );

            //基幹部品情報取得
            result.MainTable = EnginePartsDao.SelectEngineCorePartsDetail( partsCd, productModelCd, serail );
            //サブ情報(無し)
            result.SubTable = null;

            _logger.Info( "基幹部品結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        #endregion

        #endregion

        #region 工程詳細検索
        #region 共用
        /// <summary>
        /// 工程共用情報取得処理
        /// </summary>
        /// <param name="modelCd">型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="lineCd">ラインコード</param>
        /// <param name="processCd">工程コード</param>
        /// <returns>詳細情報</returns>
        public static ResultSetGeneric SelectProcessGenericDetail( string modelCd, string serial, string lineCd, string processCd ) {
            // 詳細情報を生成
            var result = new ResultSetGeneric();
            // 来歴情報取得
            result.HistoryInfo = CommonProcessDao.SelectNewestProcessWorkHistory( modelCd, serial, lineCd, processCd );
            // 計測項目、計測結果を初期化
            result.MeasuringItemInfo = null;
            result.MeasuringResultInfo = null;
            if ( 0 < result.HistoryInfo.Rows.Count ) {
                result.MeasuringItemInfo = new DataTable[result.HistoryInfo.Rows.Count];
                result.MeasuringResultInfo = new DataTable[result.HistoryInfo.Rows.Count];
            }
            for ( var index = 0; index < result.HistoryInfo.Rows.Count; index++ ) {
                _logger.Info( "工程共用情報[{0}] ModelCd:[{1}] Serial:[{2}] LineCd:[{3}] ProcessCd:[{3}] WorkCd:[{4}]",
                    index,
                    StringUtils.ToString( result.HistoryInfo.Rows[index]["MODEL_CD"] ),
                    StringUtils.ToString( result.HistoryInfo.Rows[index]["SERIAL"] ),
                    StringUtils.ToString( result.HistoryInfo.Rows[index]["LINE_CD"] ),
                    StringUtils.ToString( result.HistoryInfo.Rows[index]["PROCESS_CD"] ),
                    StringUtils.ToString( result.HistoryInfo.Rows[index]["WORK_CD"] ) );
                // ヘッダ情報
                DataTable itemInfo = null;
                // 計測情報
                DataTable resultInfo = null;
                // 計測項目情報取得処理
                itemInfo = CommonProcessDao.SelectMeasuringItemInfo(
                    StringUtils.ToString( result.HistoryInfo.Rows[index]["LINE_CD"] ),
                    StringUtils.ToString( result.HistoryInfo.Rows[index]["PROCESS_CD"] ),
                    StringUtils.ToString( result.HistoryInfo.Rows[index]["WORK_CD"] ) );
                // トレーサビリティ項目コードリストの作成
                var traceabilityItemCdList = itemInfo.AsEnumerable()
                    .Select( x => StringUtils.ToString( x["ATTRIBUTE_CD"] ) )
                    .ToList();
                _logger.Info( "工程共用情報[{0}] TraceabilityItemCdList:[[{1}]]",
                    index, string.Join( "],[", traceabilityItemCdList ) );
                // 作業来歴情報取得処理
                resultInfo = CommonProcessDao.SelectWorkHistoryInfo(
                    StringUtils.ToString( result.HistoryInfo.Rows[index]["MODEL_CD"] ),
                    StringUtils.ToString( result.HistoryInfo.Rows[index]["SERIAL"] ),
                    StringUtils.ToString( result.HistoryInfo.Rows[index]["LINE_CD"] ),
                    StringUtils.ToString( result.HistoryInfo.Rows[index]["PROCESS_CD"] ),
                    StringUtils.ToString( result.HistoryInfo.Rows[index]["WORK_CD"] ),
                    traceabilityItemCdList );
                if ( 0 < itemInfo.Rows.Count ) {
                    result.MeasuringResultInfo[index] = resultInfo;
                }
                result.MeasuringItemInfo[index] = itemInfo;
            }
            return result;
        }
        #endregion

        #region トラクタ
        /// <summary>
        /// パワクロ走行検査詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// 取得結果
        /// MainTable  :0～n件
        /// SubTable   :MainTableレコード数分の配列で、0～n件
        public static ResultSetPCrawler SelectTractorPCrawlerDetail( string productModelCd, string serial ) {
            ResultSetPCrawler result = new ResultSetPCrawler();

            _logger.Info( "パワクロ走行検査検索--->型式={0} 機番={1}", productModelCd, serial );

            //パワクロ走行検査ヘッダ(TT_SQ_POWCLAW_INS_ITEM_RESULT)取得
            result.MainTable = TractorProcessDao.SelectTractorPCrawlerHeader( productModelCd, serial );

            //詳細情報取得(ヘッダ件数分取得)
            result.SubTables = null;
            if ( 0 < result.MainTable.Rows.Count ) {
                result.SubTables = new DataTable[result.MainTable.Rows.Count];
            }
            for ( int index = 0; index < result.MainTable.Rows.Count; index++ ) {
                //詳細情報検索用パラメータ取得
                DateTime inspectionDtFrom = DateUtils.ToDate( result.MainTable.Rows[index]["createDt"] );
                DateTime inspectionDtTo = DateUtils.ToDate( result.MainTable.Rows[index]["inspectionDt"] );

                //詳細取得
                result.SubTables[index] = new DataTable();
                result.SubTables[index] = TractorProcessDao.SelectTractorPCrawlerDetail( productModelCd, serial, inspectionDtFrom, inspectionDtTo );
            }

            _logger.Info( "パワクロ走行検査結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// トラクタチェックシート詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// 取得結果
        /// MainTable  :0～n件
        /// SubTable   :未使用
        public static ResultSet SelectTractorCheckSheetDetail( string productModelCd, string serial ) {
            ResultSet result = new ResultSet();

            _logger.Info( "チェックシート検索--->型式={0} 機番={1}", productModelCd, serial );

            //チェックシート一覧(TBL_チェックシートイメージ)取得
            result.MainTable = TractorProcessDao.SelectTractorCheckSheetDetail( productModelCd, serial );
            //詳細情報取得(無し)
            result.SubTable = null;

            _logger.Info( "チェックシート結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// トラクタ品質画像証跡詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// MainTable  :0～n件
        /// SubTable   :未使用
        public static ResultSet SelectTractorCamImageDetail( string productModelCd, string serial ) {
            ResultSet result = new ResultSet();

            _logger.Info( "品質画像証跡検索--->型式={0} 機番={1}", productModelCd, serial );

            //品質画像証跡(TT_SQ_CAMERA_IMAGE_STORAGE)取得
            result.MainTable = TractorProcessDao.SelectCamImageDetail( productModelCd, serial );
            //詳細情報取得(無し)
            result.SubTable = null;

            _logger.Info( "品質画像証跡結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// トラクタ 電子チェックシートヘッダ検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="serial">ラインコード</param>
        /// 取得結果
        /// MainTable  :0～n件
        /// SubTable   :未使用
        public static ResultSetMulti SelectELCheckSheetHeader( string productModelCd, string serial ) {
            ResultSetMulti result = new ResultSetMulti();

            _logger.Info( "電子チェックシートヘッダ検索--->型式={0} 機番={1}", productModelCd, serial );

            //検査情報ヘッダ
            result.MainTable = TractorProcessDao.SelectELCheckHeader( productModelCd, serial );

            _logger.Info( "電子チェックシートヘッダ結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }
        /// <summary>
        /// トラクタ 電子チェックシート詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="serial">ラインコード</param>
        /// 取得結果
        /// MainTable  :0～n件
        /// SubTable   :未使用
        public static ResultSetMulti SelectELCheckSheetDetail( string productModelCd, string serial, string lineCd ) {
            ResultSetMulti result = new ResultSetMulti();

            _logger.Info( "電子チェックシート検索--->型式={0} 機番={1}", productModelCd, serial );

            //検査情報ヘッダ
            result.MainTable = TractorProcessDao.SelectELCheckHeader( productModelCd, serial, lineCd );

            result.SubTables = null;
            if ( 0 < result.MainTable.Rows.Count ) {
                result.SubTables = new DataTable[4];

                if ( true == StringUtils.IsEmpty( lineCd ) ) {
                    //LINE_CD未設定の場合、MainTableからラインコードを取得
                    lineCd = StringUtils.ToString( result.MainTable.Rows[0]["LINE_CD"] );
                }
            }

            //検査情報
            result.SubTables[TBL_IDX0] = new DataTable();
            result.SubTables[TBL_IDX0] = TractorProcessDao.SelectELCheckInfo( productModelCd, serial, lineCd );

            //不具合一覧取
            result.SubTables[TBL_IDX1] = new DataTable();
            result.SubTables[TBL_IDX1] = TractorProcessDao.SelectNGList( productModelCd, serial, lineCd );

            //検査画像
            result.SubTables[TBL_IDX2] = new DataTable();
            result.SubTables[TBL_IDX2] = TractorProcessDao.SelectChkImg( productModelCd, serial, lineCd );

            //不具合画像
            result.SubTables[TBL_IDX3] = new DataTable();
            result.SubTables[TBL_IDX3] = TractorProcessDao.SelectNGImg( productModelCd, serial, lineCd );

            _logger.Info( "電子チェックシート結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }
        /// <summary>
        /// トラクタ 従業員情報検索処理
        /// </summary>
        /// <param name="dateTime">日付</param>
        /// <param name="userId">従業員No</param>
        /// <param name="syozoku">所属ライン番号</param>
        /// 取得結果
        /// DataTable  :0～n件
        public static DataTable SelectEmpInfo( string strEmp, string strLineCd ) {
            DataTable tblTmp = null;

            _logger.Info( "従業員情報検索--->従業員No={0}", strEmp, "" );

            //従業員情報
            tblTmp = KintaiDao.SelectByList( null, null );

            _logger.Info( "従業員情報結果<---MAIN:{0}件", tblTmp.Rows.Count );
            return tblTmp;
            ;
        }

        /// <summary>
        /// トラクタ 刻印検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="stationCd">ステーションコード</param>
        /// 取得結果
        /// MainTable  :0～n件
        /// SubTable   :未使用
        public static ResultSetMulti SelectPrintSheel( string productModelCd, string serial, string stationCd, string country ) {
            ResultSetMulti result = new ResultSetMulti();

            _logger.Info( "刻印検索--->型式={0} 機番={1} ステーションコード={2}", productModelCd, serial, stationCd );

            //刻印：前車軸フレーム
            //result.MainTable = TractorProcessDao.SelectPrintSheel( productModelCd, serial, stationCd, "02" );

            result.SubTables = new DataTable[3];

            //刻印：前車軸フレーム
            result.SubTables[TBL_IDX0] = new DataTable();
            result.SubTables[TBL_IDX0] = TractorProcessDao.SelectPrintSheel( productModelCd, serial, stationCd, "02" );

            //ダミーでCopyする
            if ( result.SubTables[TBL_IDX0].Rows.Count > 0 ) {
                result.MainTable = result.SubTables[TBL_IDX0].Copy();
            }

            //刻印：ミッション
            result.SubTables[TBL_IDX1] = new DataTable();
            result.SubTables[TBL_IDX1] = TractorProcessDao.SelectPrintSheel( productModelCd, serial, stationCd, "03" );

            //ダミーでCopyする
            if ( result.SubTables[TBL_IDX1].Rows.Count > 0 ) {
                result.MainTable = result.SubTables[TBL_IDX1].Copy();
            }


            //刻印：MID
            result.SubTables[TBL_IDX2] = new DataTable();
            result.SubTables[TBL_IDX2] = TractorProcessDao.SelectPrintSheel( productModelCd, serial, stationCd, "04" );

            //ダミーでCopyする
            if ( result.SubTables[TBL_IDX2].Rows.Count > 0 ) {
                result.MainTable = result.SubTables[TBL_IDX2].Copy();
            }

            _logger.Info( "刻印結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// トラクタ ナットランナー詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// MainTable  :0～n件
        /// SubTable   :未使用
        public static ResultSet SelectTractorNutRunnerDetail( string productModelCd, string serial ) {
            ResultSet result = new ResultSet();

            _logger.Info( "ナットランナー検索--->型式={0} 機番={1}", productModelCd, serial );

            //ナットランナー(TT_SQ_CAMERA_IMAGE_STORAGE)取得
            result.MainTable = TractorProcessDao.SelectNutRunner( productModelCd, serial );

            result.SubTable = null;

            _logger.Info( "ナットランナー結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// トラクタ 光軸検査詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// MainTable  :0～n件
        public static ResultSetMulti SelectOptaxisDetail( string productModelCd, string serial ) {
            ResultSetMulti result = new ResultSetMulti();

            _logger.Info( "光軸検査検索--->型式={0} 機番={1}", productModelCd, serial );

            //光軸検査(TT_SQ_ETR_INS_OPTAXIS_RESULT)取得
            result.MainTable = TractorProcessDao.SelectOptaxis( productModelCd, serial );

            _logger.Info( "光軸検査結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// トラクタ トラクタ走行検査詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// MainTable  :0～n件
        public static ResultSetMulti SelectTractorAllDetail( string productModelCd, string serial ) {
            ResultSetMulti result = new ResultSetMulti();

            _logger.Info( "トラクタ走行検査検索--->型式={0} 機番={1}", productModelCd, serial );

            //トラクタ走行検査(TT_SQ_ETR_INS_ITEM_RESULT)取得
            result.MainTable = TractorProcessDao.SelectTractorAll( productModelCd, serial );

            _logger.Info( "トラクタ走行検査結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// トラクタ 関所ヘッダ検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="lineCd">ラインコード</param>
        public static ResultSetMulti SelectCheckPointHeader( string productModelCd, string serial, string lineCd ) {
            ResultSetMulti result = new ResultSetMulti();

            _logger.Info( "関所ヘッダ検索--->型式={0} 機番={1} ラインコード={2}", productModelCd, serial, lineCd );

            //工程情報ヘッダ
            result.MainTable = TractorProcessDao.SelectCheckPointHeader( productModelCd, serial, lineCd );

            _logger.Info( "関所ヘッダ検索<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// トラクタ 関所詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="stationCd">ステーションコード</param>
        public static ResultSetMulti SelectCheckPoint( string productModelCd, string serial, string stationCd ) {
            ResultSetMulti result = new ResultSetMulti();

            _logger.Info( "工程作業実績検索--->型式={0} 機番={1} ステーションコード={2}", productModelCd, serial, stationCd );

            //検査情報ヘッダ
            result.MainTable = TractorProcessDao.SelectCheckPoint( productModelCd, serial, stationCd );

            _logger.Info( "工程作業実績検索<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// トラクタ イメージチェックシートヘッダ検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// 取得結果
        /// MainTable  :0～n件
        /// SubTable   :未使用
        public static ResultSetMulti SelectTractorImgCheckSheetHeader( string productModelCd, string serial ) {
            ResultSetMulti result = new ResultSetMulti();

            _logger.Info( "[トラクタ]イメージチェックシートヘッダ検索--->型式={0} 機番={1}", productModelCd, serial );

            //検査情報ヘッダ
            result.MainTable = TractorProcessDao.SelectImgCheckHeader( productModelCd, serial );

            _logger.Info( "[トラクタ]イメージチェックシートヘッダ結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// トラクタ 検査ベンチ検査結果詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// 取得結果
        /// MainTable  :0～n件
        /// SubTable   :未使用
        public static ResultSetMulti SelectTractorTestBenchDetail( string productModelCd, string serial ) {
            ResultSetMulti result = new ResultSetMulti();

            _logger.Info( "[トラクタ]検査ベンチ検査結果詳細検索--->型式={0} 機番={1}", productModelCd, serial );

            // 詳細画面トラクタ工程(検査ベンチ)情報取得
            result.MainTable = TractorProcessDao.SelectTestBench( productModelCd, serial );

            _logger.Info( "[トラクタ]検査ベンチ検査結果詳細結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// トラクタ AI画像解析詳細検索処理
        /// </summary>
        /// <param name="assemblyPatternCd">組立パターン</param>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 詳細情報
        /// genericTable  :1件
        /// specificTable :未使用
        /// </returns>
        public static ResultSet SelectTractorAiImageDetail( string productModelCd, string serial ) {
            ResultSet result = new ResultSet();

            _logger.Info( "AI画像解析検索--->型式={0} 機番={1}", productModelCd, serial );

            //詳細情報(TT_SQ_IMG_ANL_RESULT)
            result.MainTable = TractorProcessDao.SelectTractorAiImageDetail( productModelCd, serial );
            //詳細情報(無し)
            result.SubTable = null;

            _logger.Info( "AI画像解析結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }
        #endregion

        #region エンジン
        /// <summary>
        /// エンジン工程[トルク締付]詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 詳細情報
        /// genericTable  :n件
        /// specificTable :未使用
        /// </returns>
        public static ResultSet SelectEngineTorqueDetail( string productModelCd, string serial ) {
            ResultSet result = new ResultSet();

            _logger.Info( "トルク締付検索--->型式={0} 機番={1}", productModelCd, serial );

            //エンジン共通情報(TBL_トルク締付履歴)取得
            result.MainTable = EngineProcessDao.SelectEngineTorqueDetail( productModelCd, serial );
            //詳細情報(無し)
            result.SubTable = null;

            _logger.Info( "トルク締付結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// エンジン工程[トルク締付]詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 詳細情報
        /// genericTable  :n件
        /// specificTable :未使用
        /// </returns>
        public static ResultSetMulti SelectEngineTorqueDetails( string productModelCd, string serial ) {
            ResultSetMulti result = new ResultSetMulti();

            _logger.Info( "トルク締付詳細検索--->型式={0} 機番={1}", productModelCd, serial );

            //エンジン共通情報(TBL_トルク締付履歴)取得
            result.MainTable = EngineProcessDao.SelectEngineTorqueLatestHistory( productModelCd, serial );
            //詳細情報取得(ヘッダ件数分取得)
            result.SubTables = null;
            if ( 0 < result.MainTable.Rows.Count ) {
                result.SubTables = new DataTable[result.MainTable.Rows.Count];
            }
            for ( int index = 0; index < result.MainTable.Rows.Count; index++ ) {
                //詳細情報検索用パラメータ取得
                string partsNm = StringUtils.ToString( result.MainTable.Rows[index]["partsNm"] );

                //詳細取得
                result.SubTables[index] = new DataTable();
                result.SubTables[index] = EngineProcessDao.SelectEngineTorqueDetail( productModelCd, serial, partsNm );
            }

            _logger.Info( "トルク締付詳細結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// エンジン工程[ハーネス検査]詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 詳細情報
        /// genericTable  :n件
        /// specificTable :未使用
        /// </returns>
        public static ResultSet SelectEngineHarnessDetail( string productModelCd, string serial ) {
            ResultSet result = new ResultSet();

            _logger.Info( "ハーネス検索--->型式={0} 機番={1}", productModelCd, serial );

            //エンジン共通情報(TBL_ハーネス検査履歴)取得
            result.MainTable = EngineProcessDao.SelectEngineHarnessDetail( productModelCd, serial );
            //詳細情報(無し)
            result.SubTable = null;

            _logger.Info( "ハーネス結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// エンジン工程[エンジン運転検査]詳細検索処理
        /// </summary>
        /// <param name="assemblyPatternCd">組立パターン</param>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 詳細情報
        /// genericTable  :n件
        /// specificTable :n件
        /// </returns>
        public static ResultSetEngineTest SelectEngineTestDetail( string assemblyPatternCd, string modelCd, string productModelCd, string serial ) {
            ResultSetEngineTest result = new ResultSetEngineTest();

            _logger.Info( "エンジン運転検査検索--->型式={0} 生産型式={1} 機番={2}", modelCd, productModelCd, serial );
            //エンジン共通情報(D_HEADER,D_HEADER_07)取得
            result.MainTable = EngineProcessDao.SelectEngineTestHeader( assemblyPatternCd, modelCd, productModelCd, serial );
            //詳細情報取得(ヘッダ件数分取得)
            result.SubTables = null;
            if ( 0 < result.MainTable.Rows.Count ) {
                result.SubTables = new DataTable[result.MainTable.Rows.Count];
            }
            for ( int index = 0; index < result.MainTable.Rows.Count; index++ ) {
                //詳細情報検索用パラメータ取得
                string idno = StringUtils.ToString( result.MainTable.Rows[index]["idno"] );
                string inspectionYmdHms = StringUtils.ToString( result.MainTable.Rows[index]["inspectionYmdHms"] );

                //詳細取得
                result.SubTables[index] = new DataTable();
                result.SubTables[index] = EngineProcessDao.SelectEngineTestDetail( assemblyPatternCd, idno, inspectionYmdHms );
            }

            _logger.Info( "エンジン運転検査結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// エンジン工程[フリクションロス]詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 詳細情報
        /// genericTable  :n件
        /// specificTable :未使用
        /// </returns>
        public static ResultSet SelectEngineFrictionLossDetail( string productModelCd, string serial ) {
            ResultSet result = new ResultSet();

            _logger.Info( "フリクションロス検索--->型式={0} 機番={1}", productModelCd, serial );

            //エンジン共通情報(TBL_フリクションロス実測)取得
            result.MainTable = EngineProcessDao.SelectEngineFrictionLossDetail( productModelCd, serial );
            //詳細情報(無し)
            result.SubTable = null;

            _logger.Info( "フリクションロス結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// エンジン工程[3C精密測定]詳細検索処理
        /// </summary>
        /// <param name="processCd">工程区分</param>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 詳細情報
        /// genericTable  :1件
        /// specificTable :n件
        /// </returns>
        public static ResultSet SelectEngine3CInspectionDetail( string processCd, string productModelCd, string serial ) {
            ResultSet result = new ResultSet();

            _logger.Info( "3C精密測定データ検索--->型式={0} 機番={1}", productModelCd, serial );

            string partsCd = "";
            switch ( processCd ) {
            case ProcessKind.PROCESS_CD_ENGINE_CYH_INSPECT: //シリンダヘッド精密測定
                partsCd = PartsKind.PARTS_CD_ENGINE_CYH;                             //CYH部品区分コード
                break;
            case ProcessKind.PROCESS_CD_ENGINE_CS_INSPECT:  //クランクシャフト精密測定
                partsCd = PartsKind.PARTS_CD_ENGINE_CS;                              //CS部品区分コード
                break;
            case ProcessKind.PROCESS_CD_ENGINE_CC_INSPECT:  //クランクケース精密測定
                partsCd = PartsKind.PARTS_CD_ENGINE_CC;                              //CC部品区分コード
                break;
            default:
                break;
            }

            //エンジン共通情報(TT_SQ_PRECISE_MEASURE_DATA,TT_SQ_3C_DETAIL,TBL_TIPS_CODE_NAMES)取得
            result.MainTable = EngineProcessDao.SelectEngine3CInspectionHeader( productModelCd, serial, partsCd );
            if ( result.MainTable.Rows.Count > 0 ) {
                //詳細情報(TT_SQ_PRECISE_MEASURE_FILE)
                result.SubTable = EngineProcessDao.SelectEngine3CInspectionDetail( result.MainTable );
            } else {
                //詳細情報(無し)
                result.SubTable = null;
            }

            _logger.Info( "3C精密測定データ結果<---MAIN:{0}件 SUB:{1}件", result.MainTable.Rows.Count, null == result.SubTable ? -1 : result.SubTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// エンジン工程[燃料噴射時期]詳細検索処理
        /// </summary>
        /// <param name="assemblyPatternCd">組立パターン</param>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 詳細情報
        /// genericTable  :1件
        /// specificTable :未使用
        /// </returns>
        public static ResultSet SelectEngineFuelInjectionDetail( string assemblyPatternCd, string productModelCd, string serial ) {
            ResultSet result = new ResultSet();

            _logger.Info( "燃料噴射時期検索--->型式={0} 機番={1}", productModelCd, serial );

            //エンジン共通情報(TBL噴射計測データ／TBL_07噴射時期計測データ)取得
            result.MainTable = EngineProcessDao.SelectEngineFuelInjectionDetail( assemblyPatternCd, productModelCd, serial );
            //詳細情報(無し)
            result.SubTable = null;

            _logger.Info( "燃料噴射時期結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// エンジン品質画像証跡詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>工程詳細DataTable(n行)</returns>
        public static ResultSet SelectEngineCamImageDetail( string productModelCd, string serial ) {
            ResultSet result = new ResultSet();

            _logger.Info( "品質画像証跡検索--->型式={0} 機番={1}", productModelCd, serial );

            //品質画像証跡(TT_SQ_CAMERA_IMAGE_STORAGE)取得
            result.MainTable = EngineProcessDao.SelectCamImageDetail( productModelCd, serial );
            //詳細情報取得(無し)
            result.SubTable = null;

            _logger.Info( "品質画像証跡結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// エンジン 電子チェックシートヘッダ検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="serial">ラインコード</param>
        /// 取得結果
        /// MainTable  :0～n件
        /// SubTable   :未使用
        public static ResultSetMulti SelectEngineELCheckSheetHeader( string productModelCd, string serial ) {
            ResultSetMulti result = new ResultSetMulti();

            _logger.Info( "エンジン電子チェックシートヘッダ検索--->型式={0} 機番={1}", productModelCd, serial );

            //検査情報ヘッダ
            result.MainTable = EngineProcessDao.SelectELCheckHeader( productModelCd, serial );

            _logger.Info( "エンジン電子チェックシートヘッダ結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// エンジン 電子チェックシート詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <param name="serial">ラインコード</param>
        /// <param name="productKindCd">製品種別コード</param>
        /// 取得結果
        /// MainTable  :0～n件
        /// SubTable   :未使用
        public static ResultSetMulti SelectEngineELCheckSheetDetail( string productModelCd, string serial, string lineCd, string productKindCd ) {
            ResultSetMulti result = new ResultSetMulti();
            string logStr = string.Empty;

            //検査情報ヘッダ
            if ( GeneralPatternCode.Tractor == productKindCd ) {
                //トラクタ
                _logger.Info( "トラクタイメージチェックシート検索--->型式={0} 機番={1}", productModelCd, serial );
                logStr = "トラクタイメージチェックシート";
                result.MainTable = TractorProcessDao.SelectImgCheckHeader( productModelCd, serial, lineCd );
            } else {
                //エンジン
                _logger.Info( "エンジン電子チェックシート検索--->型式={0} 機番={1}", productModelCd, serial );
                logStr = "エンジン電子チェックシート";
                result.MainTable = EngineProcessDao.SelectELCheckHeader( productModelCd, serial, lineCd );
            }

            result.SubTables = null;
            if ( 0 < result.MainTable.Rows.Count ) {
                result.SubTables = new DataTable[4];

                if ( true == StringUtils.IsEmpty( lineCd ) ) {
                    //LINE_CD未設定の場合、MainTableからラインコードを取得
                    lineCd = StringUtils.ToString( result.MainTable.Rows[0]["LINE_CD"] );
                }
            }

            //検査情報
            result.SubTables[TBL_IDX0] = new DataTable();
            result.SubTables[TBL_IDX0] = CommonProcessDao.SelectIcsChkInfo( productModelCd, serial, lineCd );

            //不具合一覧取
            result.SubTables[TBL_IDX1] = new DataTable();
            result.SubTables[TBL_IDX1] = CommonProcessDao.SelectIcsNGList( productModelCd, serial, lineCd );

            //検査画像
            result.SubTables[TBL_IDX2] = new DataTable();
            result.SubTables[TBL_IDX2] = CommonProcessDao.SelectIcsChkImg( productModelCd, serial, lineCd );

            //不具合画像
            result.SubTables[TBL_IDX3] = new DataTable();
            result.SubTables[TBL_IDX3] = CommonProcessDao.SelectIcsNGImg( productModelCd, serial, lineCd );

            _logger.Info( "{0}結果<---MAIN:{1}件", logStr, result.MainTable.Rows.Count );
            return result;
        }

        /// <summary>
        /// 出荷部品詳細検索処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 取得結果
        /// MainTable  :0～n件
        /// SubTable   :0～m件
        /// </returns>
        public static ResultSet SelectEngineShipmentPartsDetail( string productModelCd, string serial ) {
            ResultSet result = new ResultSet();
            _logger.Info( "出荷部品検索--->型式={0} 機番={1}", productModelCd, serial );
            result.MainTable = EngineProcessDao.SelectShipmentParts( productModelCd, serial );
            var packingOrderIdno = result.MainTable.AsEnumerable().Select( r => ( (string)r["PARTS_NUM"] ).Split( ' ' ) )
                .Where( data => data.Length == 3 )
                .Select( data => data[1] );
            if ( packingOrderIdno.Count() > 0 ) {
                // 梱包作業指示IDNOが存在する場合
                result.SubTable = EngineProcessDao.SelectPackingOrderSheet( packingOrderIdno );
            }
            _logger.Info( "出荷部品検索<---MAIN:{0}件", result.MainTable.Rows.Count );
            _logger.Info( "出荷部品検索<---SUB:{0}件", result.SubTable?.Rows?.Count ?? 0 );
            return result;
        }

        /// <summary>
        /// エンジン工程[AI画像解析]詳細検索処理
        /// </summary>
        /// <param name="assemblyPatternCd">組立パターン</param>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        /// <returns>
        /// 詳細情報
        /// genericTable  :1件
        /// specificTable :未使用
        /// </returns>
        public static ResultSet SelectEngineAiImageDetail( string productModelCd, string serial ) {
            ResultSet result = new ResultSet();

            _logger.Info( "AI画像解析検索--->型式={0} 機番={1}", productModelCd, serial );

            //詳細情報(TT_SQ_IMG_ANL_RESULT)
            result.MainTable = EngineProcessDao.SelectEngineAiImageDetail( productModelCd, serial );
            //詳細情報(無し)
            result.SubTable = null;

            _logger.Info( "AI画像解析結果<---MAIN:{0}件", result.MainTable.Rows.Count );
            return result;
        }

        #endregion

        #endregion

        #region 製品別通過実績データ取得
        /// <summary>
        /// 製品別通過実績の工程取得
        /// </summary>
        /// <param name="modelCd">型式CD</param>
        /// <param name="serial">機番</param>
        /// <param name="assemblyPatternCd">組立パターン</param>
        /// <returns></returns>
        public static DataTable SelectImaDokoProcess( string modelCd, string serial, string assemblyPatternCd ) {
            DataTable result = new DataTable();

            _logger.Info( "製品別通過実績(工程)検索--->型式={0} 機番={1} アセンブリコード={2}", modelCd, serial, assemblyPatternCd );

            //工程取得
            result = CommonProcessDao.SelectImaDokoProcess( modelCd, serial, assemblyPatternCd );

            _logger.Info( "製品別通過実績(工程)検索結果<---MAIN:{0}件", result.Rows.Count );
            return result;

        }
        /// <summary>
        /// 製品別通過実績の実績取得
        /// </summary>
        /// <param name="modelCd">型式CD</param>
        /// <param name="serial">機番</param>
        /// <param name="assemblyPatternCd">組立パターン</param>
        /// <returns></returns>
        public static DataTable SelectImaDokoResult( string modelCd, string serial, string assemblyPatternCd ) {
            DataTable result = new DataTable();

            _logger.Info( "製品別通過実績(実績)検索--->型式={0} 機番={1} アセンブリコード={2}", modelCd, serial, assemblyPatternCd );

            //実績取得
            result = CommonProcessDao.SelectImaDokoResult( modelCd, serial, assemblyPatternCd );


            _logger.Info( "製品別通過実績(実績)検索結果<---MAIN:{0}件", result.Rows.Count );
            return result;

        }

        #endregion
    }
}