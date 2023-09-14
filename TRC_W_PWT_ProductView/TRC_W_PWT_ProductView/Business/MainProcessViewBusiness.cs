using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using KTFramework.Common;
using KTFramework.Dao;
using TRC_W_PWT_ProductView.Defines.ListDefine;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Dao.Com;
using TRC_W_PWT_ProductView.Dao.Products;
using TRC_W_PWT_ProductView.Dao.Parts;
using TRC_W_PWT_ProductView.Dao.Process;
using TRC_W_PWT_ProductView.UI.Pages;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.ProcessViewDefine;


namespace TRC_W_PWT_ProductView.Business {
    /// <summary>
    /// 工程検索ビジネスクラス
    /// </summary>
    public class MainProcessViewBusiness {
        //ロガー定義
        private static readonly Logger _logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 一覧情報格納構造体
        /// <summary>
        /// 一覧情報格納構造体
        /// </summary>
        [Serializable]
        public struct ResultSet {
            /// <summary>メイン情報</summary>
            public DataTable ResultTable { get; set; }
            /// <summary>サブ情報</summary>
            public Msg Message { get; set; }
        }
        #endregion

        #region

        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <param name="dicCondition">画面検索条件</param>
        /// <returns>一覧表示DataTable</returns>
        public static ResultSet Search( Dictionary<string, object> dicCondition, bool forExcelOutput = false ) {
            //検索結果
            var result = new ResultSet();

            //製品種別
            string productKind = dicCondition[SearchConditionDefine.CONDITION_COMMON.PRODUCT_KIND.bindField].ToString();
            //工程種別
            string processKind = dicCondition[SearchConditionDefine.CONDITION_COMMON.PROCESS_KIND.bindField].ToString();
            //エンジン種別
            string engineKind = dicCondition[SearchConditionDefine.CONDITION_COMMON.ENGINE_KIND.bindField].ToString();

            if ( productKind.Equals( ProductKind.Engine ) ) {
                switch ( processKind ) {
                    case ProcessKind.PROCESS_CD_ENGINE_INJECTION:
                        if ( engineKind.Equals( EngineKind.ENGINE_03 ) ) {
                            result.ResultTable = ProcessSearchDao.SelectEngineInjection03List( dicCondition, forExcelOutput );
                        } else if( engineKind.Equals( EngineKind.ENGINE_07 ) ) {
                            result.ResultTable = ProcessSearchDao.SelectEngineInjection07List( dicCondition, forExcelOutput );
                        }
                        break;
                    case ProcessKind.PROCESS_CD_ENGINE_FRICTION:
                        result.ResultTable = ProcessSearchDao.SelectEngineFrictionList( dicCondition, forExcelOutput );
                        break;
                    case ProcessKind.PROCESS_CD_ENGINE_TEST:
                        if ( engineKind.Equals( EngineKind.ENGINE_03 ) ) {
                            result.ResultTable = ProcessSearchDao.SelectEngineTest03List( dicCondition, forExcelOutput );
                        } else if ( engineKind.Equals( EngineKind.ENGINE_07 ) ) {
                            result.ResultTable = ProcessSearchDao.SelectEngineTest07List( dicCondition, forExcelOutput );
                        }
                        break;
                    default:
                        break;
                }
            } else if ( productKind.Equals( ProductKind.Tractor ) ) {
                //未実装
                throw new NotImplementedException();
            } else {
                throw new ArgumentException( "製品種別が不正です。" );
            }

            if( result.ResultTable == null || result.ResultTable.Rows.Count == 0 ) {
                result.Message = new Msg( MsgManager.MESSAGE_WRN_61010 );
            }

            return result;
        }

        #endregion
    }
}