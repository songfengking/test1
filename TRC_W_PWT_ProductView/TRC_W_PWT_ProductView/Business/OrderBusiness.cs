using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using KTFramework.Common;
using KTFramework.Common.Dao;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Dao.Com;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.UI.Pages.Order;
using static TRC_W_PWT_ProductView.Business.DetailPartsViewBusiness;

namespace TRC_W_PWT_ProductView.Business {
    /// <summary>
    /// ビジネスクラス
    /// </summary>
    public class OrderBusiness {
        // ロガー定義
        private static readonly Logger Logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数
        /// <summary>国内国コード</summary>
        private const string KUNI_CODE_KOKUNAI = "      ";
        /// <summary>表示名称</summary>
        private const string DISPLAY_NM = "表示名称";
        /// <summary>ステーション</summary>
        private const string STATION = "ステーション";
        /// <summary>ステーションコード</summary>
        private const string STATION_CODE = "ステーションコード";
        /// <summary>実績日時</summary>
        private const string JISSEKI_DT = "実績日時";
        /// <summary>機番7桁</summary>
        private const int KIBAN7 = 7;
        /// <summary>機番6桁</summary>
        private const int KIBAN6 = 6;
        /// <summary>IDNO7桁</summary>
        private const int IDNO7 = 7;
        /// <summary>TARGET_SAGYO_KEEP</summary>
        private const string TARGET_SAGYO_KEEP = "TARGET_SAGYO_KEEP";
        /// <summary>期</summary>
        private const string PATTERN = "PATTERN";
        /// <summary>総称パターン</summary>
        private const string GENERAL_PATTERN = "GENERAL_PATTERN";
        /// <summary>次稼働日</summary>
        private const int NEXT_WORK_DAY = 0;

        #region 搭載エンジン引当で使用する暗黙の検索条件
        /// <summary>引当完了データ表示限界（台）：ミッション立体出庫してから画面に表示している台数の初期値</summary>
        private const int DISPLAY_KANRYO_NUM = 10;
        /// <summary>ミッション立体倉庫直前のステーション</summary>
        private const string JISSEKI_STATION_PREV = "214001";
        /// <summary>ミッション立体倉庫直後のステーション </summary>
        private const string JISSEKI_STATION_POST = "214012";
        #endregion


        #region 指示レベル
        /// <summary>指示レベル指定なし</summary>
        public const string SHIJI_LEVEL_ALL = "0";
        /// <summary>ID機番検索</summary>
        public const string SHIJI_LEVEL_ID_KIBAN = "0";
        /// <summary>指示レベルトラクタ</summary>
        public const string SHIJI_LEVEL_TRACTOR = "1";
        /// <summary>ミッション投入</summary>
        public const string SHIJI_LEVEL_MISSION_THROW = "1";
        /// <summary>本機確定</summary>
        public const string SHIJI_LEVEL_TRACTOR_FIX = "2";
        /// <summary>指示レベル03エンジン</summary>
        public const string SHIJI_LEVEL_03_ENGINE = "3";
        /// <summary>指示レベル07エンジン</summary>
        public const string SHIJI_LEVEL_07_ENGINE = "7";
        #endregion

        #region 組立パターン
        /// <summary>組立パターン</summary>
        private const string K_PATTERN = "MS_K_PATAN";
        /// <summary>組立パターン</summary>
        private const string K_PATTERN_CODE = "パターンコード";
        /// <summary>組立パターン07搭載</summary>
        private const string K_PATTERN_07_TOUSAI = "14";
        /// <summary>組立パターン03搭載</summary>
        private const string K_PATTERN_03_TOUSAI = "15";
        /// <summary>組立パターン07OEM</summary>
        private const string K_PATTERN_07_OEM = "18";
        /// <summary>組立パターン03OEM</summary>
        private const string K_PATTERN_03_OEM = "19";
        /// <summary>組立パターントラクタ</summary>
        private const string K_PATTERN_TRACTOR = "32";
        #endregion

        #region 検索種別
        /// <summary>検索種別トラクタ</summary>
        private const string SEARCH_SYUBETSU = "searchSyubetsu";
        /// <summary>検索種別トラクタ</summary>
        private const string SEARCH_SYUBETSU_TRACTOR = "1";
        /// <summary>検索種別03エンジン全て</summary>
        private const string SEARCH_SYUBETSU_03_ENGINE_ALL = "2";
        /// <summary>検索種別03エンジン搭載</summary>
        private const string SEARCH_SYUBETSU_03_ENGINE_TOUSAI = "3";
        /// <summary>検索種別03エンジンOEM</summary>
        private const string SEARCH_SYUBETSU_03_ENGINE_OEM = "4";
        /// <summary>検索種別07エンジン全て</summary>
        private const string SEARCH_SYUBETSU_07_ENGINE_ALL = "5";
        /// <summary>検索種別07エンジン搭載</summary>
        private const string SEARCH_SYUBETSU_07_ENGINE_TOUSAI = "6";
        /// <summary>検索種別07エンジンOEM</summary>
        private const string SEARCH_SYUBETSU_07_ENGINE_OEM = "7";
        #endregion

        #region エンジン種別
        /// <summary>エンジン種別全て</summary>
        private const string PATTERN_FLAG_ALL = "1";
        /// <summary>エンジン種別搭載</summary>
        private const string PATTERN_FLAG_TOUSAI = "2";
        /// <summary>エンジン種別OEM</summary>
        private const string PATTERN_FLAG_OEM = "3";
        #endregion

        #region 総称パターン
        /// <summary>総称パターンエンジン</summary>
        private const string SPATTERN_ENGINE = "10";
        /// <summary>総称パターントラクタ</summary>
        private const string SPATTERN_TRACTOR = "30";
        #endregion

        #region 立体倉庫
        /// <summary>全て(表示用)</summary>
        private const string RITTAI_ALL_DISP = "全て";
        /// <summary>全て</summary>
        private const string RITTAI_ALL = "";
        /// <summary>筑波(表示用)</summary>
        private const string RITTAI_TSUKUBA_DISP = "筑波";
        /// <summary>筑波</summary>
        private const string RITTAI_TSUKUBA = "1";
        /// <summary>OEM(表示用)</summary>
        private const string RITTAI_OEM_DISP = "OEM";
        /// <summary>OEM</summary>
        private const string RITTAI_OEM = "2";
        /// <summary>堺(表示用)</summary>
        private const string RITTAI_SAKAI_DISP = "堺";
        /// <summary>堺</summary>
        private const string RITTAI_SAKAI = "3";
        /// <summary>塗装後(表示用)</summary>
        private const string RITTAI_TOSOUGO_DISP = "塗装後";
        /// <summary>塗装後summary>
        private const string RITTAI_TOSOUGO = "4";
        #endregion
        #region 棚
        /// <summary>全て(表示用)</summary>
        private const string RITTAI_STOP_ALL_DISP = "全て";
        /// <summary>全て</summary>
        private const string RITTAI_STOP_ALL = "";
        /// <summary>通常(表示用)</summary>
        private const string RITTAI_STOP_NORMAL_DISP = "通常";
        /// <summary>通常</summary>
        private const string RITTAI_STOP_NORMAL = "0";
        /// <summary>禁止棚(表示用)</summary>
        private const string RITTAI_STOP_STOP_DISP = "禁止棚";
        /// <summary>禁止棚</summary>
        private const string RITTAI_STOP_STOP = "1";
        #endregion
        #region 状態
        /// <summary>全て(表示用)</summary>
        private const string LOCATION_FLAG_ALL_DISP = "全て";
        /// <summary>全て</summary>
        private const string LOCATION_FLAG_ALL = "";
        /// <summary>空き(表示用)</summary>
        private const string LOCATION_FLAG_NOENGINE_DISP = "空き";
        /// <summary>空き</summary>
        private const string LOCATION_FLAG_NOENGINE = "0";
        /// <summary>空き以外(表示用)</summary>
        private const string LOCATION_FLAG_ANYENGINE_DISP = "空き以外";
        /// <summary>空き以外</summary>
        private const string LOCATION_FLAG_ANYENGINE = "99";
        /// <summary>在席(表示用)</summary>
        private const string LOCATION_FLAG_STOCKED_DISP = "在席";
        /// <summary>在席</summary>
        private const string LOCATION_FLAG_STOCKED = "1";
        /// <summary>入庫(表示用)</summary>
        private const string LOCATION_FLAG_ACCEPTED_DISP = "入庫";
        /// <summary>入庫</summary>
        private const string LOCATION_FLAG_ACCEPTED = "2";
        /// <summary>出庫(表示用)</summary>
        private const string LOCATION_FLAG_DELIVERED_DISP = "出庫";
        /// <summary>出庫</summary>
        private const string LOCATION_FLAG_DELIVERED = "3";
        /// <summary>空ﾊﾟﾚ(表示用)</summary>
        private const string LOCATION_FLAG_PALETTE_DISP = "空ﾊﾟﾚ";
        /// <summary>空ﾊﾟﾚ</summary>
        private const string LOCATION_FLAG_PALETTE = "4";
        #endregion
        #region 種別
        /// <summary>全て(表示用)</summary>
        private const string ENGINE_SYUBETSU_ALL_DISP = "全て";
        /// <summary>全て</summary>
        private const string ENGINE_SYUBETSU_ALL = "";
        /// <summary>03(表示用)</summary>
        private const string ENGINE_SYUBETSU_03_DISP = "03";
        /// <summary>03</summary>
        private const string ENGINE_SYUBETSU_03 = "1";
        /// <summary>07(表示用)</summary>
        private const string ENGINE_SYUBETSU_07_DISP = "07";
        /// <summary>07</summary>
        private const string ENGINE_SYUBETSU_07 = "2";
        #endregion
        #region 搭載/OEM
        /// <summary>全て(表示用)</summary>
        private const string ENGINE_TOUSAIOEM_ALL_DISP = "全て";
        /// <summary>全て</summary>
        private const string ENGINE_TOUSAIOEM_ALL = "";
        /// <summary>搭載(表示用)</summary>
        private const string ENGINE_TOUSAIOEM_TOUSAI_DISP = "搭載";
        /// <summary>搭載</summary>
        private const string ENGINE_TOUSAIOEM_TOUSAI = "1";
        /// <summary>OEM(表示用)</summary>
        private const string ENGINE_TOUSAIOEM_OEM_DISP = "OEM";
        /// <summary>OEM</summary>
        private const string ENGINE_TOUSAIOEM_OEM = "2";
        #endregion
        #region 内外作
        /// <summary>全て(表示用)</summary>
        private const string ENGINE_NAIGAISAKU_ALL_DISP = "全て";
        /// <summary>全て</summary>
        private const string ENGINE_NAIGAISAKU_ALL = "";
        /// <summary>内作(表示用)</summary>
        private const string ENGINE_NAIGAISAKU_NAISAKU_DISP = "内作";
        /// <summary>内作</summary>
        private const string ENGINE_NAIGAISAKU_NAISAKU = "0";
        /// <summary>堺(表示用)</summary>
        private const string ENGINE_NAIGAISAKU_SAKAI_DISP = "堺";
        /// <summary>堺</summary>
        private const string ENGINE_NAIGAISAKU_SAKAI = "1";
        #endregion
        #region 運転
        /// <summary>全て(表示用)</summary>
        private const string ENGINE_UNTEN_ALL_DISP = "全て";
        /// <summary>全て</summary>
        private const string ENGINE_UNTEN_ALL = "";
        /// <summary>運転前(表示用)</summary>
        private const string ENGINE_UNTEN_BEFORE_DISP = "運転前";
        /// <summary>運転前</summary>
        private const string ENGINE_UNTEN_BEFORE = "0";
        /// <summary>完了(表示用)</summary>
        private const string ENGINE_UNTEN_AFTER_DISP = "完了";
        /// <summary>完了</summary>
        private const string ENGINE_UNTEN_AFTER = "1";
        #endregion
        #endregion


        #region 一覧情報格納構造体
        /// <summary>
        /// 一覧情報格納構造体
        /// </summary>
        [Serializable]
        public struct ResultSet {
            /// <summary>メイン情報</summary>
            public DataTable ListTable { get; set; }
            /// <summary>サブ情報</summary>
            public Msg Message { get; set; }
            /// <summary>サブ情報2</summary>
            /// 使用する側でキャストして使ってください
            public object Object { get; set; }
            /// <summary>立体別在庫数</summary>
            public DataTable ListTableRittaibetsuZaiko { get; set; }
            /// <summary>種別在庫数</summary>
            public DataTable ListTableSyubetsuZaiko { get; set; }
        }
        #endregion

        #region 製品別通過実績検索
        /// <summary>
        /// 製品別通過実績検索：検索処理
        /// </summary>
        /// <param name="dicCondition">検索条件</param>
        /// <param name="maxGridViewCount">検索上限数</param>
        /// <param name="numLimitFlag">件数上限フラグ</param>
        /// <returns>処理結果</returns>
        public static ResultSet SearchOfSearchProductOrder( ref Dictionary<string, object> dicCondition, int maxGridViewCount ) {

            // 処理結果の定義
            ResultSet result = new ResultSet();

            // 処理結果 = 検索条件チェック(ref 検索条件, ref 処理結果)
            result = CheckSearchParams( ref dicCondition, ref result );

            if ( false == (bool)result.Object ) {
                // 処理結果.チェック結果 = false
                return result;
            }

            // 実績検索結果 = 実績検索(検索条件)
            DataTable jissekiList = SearchJisseki( dicCondition );

            if ( jissekiList.Rows.Count <= 0 ) {
                // 実績検索結果の件数 = 0
                // 処理結果.メッセージ = MESSAGE_WRN_61010
                result.Message = new Msg( MsgManager.MESSAGE_WRN_61010 );
                return result;
            }

            // ステーション通過順序結果 = ステーション通過順序検索(検索条件)
            DataTable stationList = SearchStationTsuukaJunjo( dicCondition );

            // 処理結果 = 検索結果表示用リスト整形(実績検索結果、ステーション通過順序結果、表示上限件数、検索条件、ref 処理結果)
            EditDisplayData( jissekiList, stationList, maxGridViewCount, dicCondition, ref result );

            return result;
        }

        /// <summary>
        /// 製品別通過実績検索：検索条件チェック
        /// </summary>
        /// <param name="dicCondition">検索条件</param>
        /// <param name="result">処理結果</param>
        /// <returns>処理結果</returns>
        public static ResultSet CheckSearchParams( ref Dictionary<string, object> dicCondition, ref ResultSet result ) {

            if ( true == ( StringUtils.IsEmpty( DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.IDNO.bindField ) )
                 && StringUtils.IsEmpty( DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.KIBAN.bindField ) )
                 && StringUtils.IsEmpty( DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.KATASHIKI_CODE.bindField ) )
                 && StringUtils.IsEmpty( DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.KUNI_CODE.bindField ) )
                 && StringUtils.IsEmpty( DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.KATASHIKI_NAME.bindField ) )
                 && StringUtils.IsEmpty( DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.TONYU_YM_NUM.bindField ) )
                 && StringUtils.IsEmpty( DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.KAKUTEI_YM_NUM.bindField ) ) ) ) {
                // ラジオボタン以外のすべての検索条件が""の場合
                // エラーメッセージ：検索条件を入力してください。
                result.Message = new Msg( MsgManager.MESSAGE_WRN_62080, "検索条件" );
                result.Object = false;

                return result;
            }

            // 検索条件構造体に検索種別を追加
            dicCondition.Add( SEARCH_SYUBETSU, "" );
            // 検索条件整形(ref 検索条件構造体)
            SearchConditionFormatOfSearchProductOrder( ref dicCondition );

            if ( true == StringUtils.IsEmpty( dicCondition[SEARCH_SYUBETSU].ToString() ) ) {
                // 検索条件構造体.検索種別が空文字
                // エラーメッセージ：
                // 製品種別を特定できませんでした。指示レベルを指定してください。\n指示レベルを指定しない場合は型式コード(10桁)又はIDNOを指定してください。
                result.Message = new Msg( MsgManager.MESSAGE_ERR_72200 );
                result.Object = false;

                return result;
            }

            result.Object = true;

            return result;
        }

        /// <summary>
        /// 製品別通過実績検索：検索条件整形
        /// </summary>
        /// <param name="dicCondition">検索条件</param>
        public static void SearchConditionFormatOfSearchProductOrder( ref Dictionary<string, object> dicCondition ) {

            // 検索条件構造体.型式コード(検索条件構造体.型式コードの大文字変換とハイフンを削除)
            dicCondition[SearchProductOrder.CONDITION.KATASHIKI_CODE.bindField] = getValueKatasikiCode( DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.KATASHIKI_CODE.bindField ) );

            // 検索種別設定
            SetSearchSyubetu( ref dicCondition );

            if ( true == StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.KATASHIKI_NAME.bindField ) ) ) {
                // 検索条件構造体.型式名 != null
                // 検索条件構造体.型式名(検索条件構造体.型式名を大文字に変換)
                dicCondition[SearchProductOrder.CONDITION.KATASHIKI_NAME.bindField] = DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.KATASHIKI_NAME.bindField ).ToUpper();
            }

            if ( true == StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.KIBAN.bindField ) ) ) {
                // 検索条件構造体.機番 != null
                // 検索条件構造体.機番(検索条件構造体.機番を大文字に変換)
                dicCondition[SearchProductOrder.CONDITION.KIBAN.bindField] = DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.KIBAN.bindField ).ToUpper();
            }

        }

        /// <summary>
        /// 製品別通過実績検索：検索種別設定
        /// </summary>
        /// <param name="dicCondition">検索条件</param>
        public static void SetSearchSyubetu( ref Dictionary<string, object> dicCondition ) {

            string searchSyubetu = "";
            string patternFlag = "";
            // shijiLevel = 検索条件構造体.製品種別
            string shijiLevel = DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.SHIJI_LEVEL.bindField );

            if ( shijiLevel == null || shijiLevel == SearchProductOrder.SHIJI_LEVEL_ALL ) {
                // shijiLevel = null || shijiLevel = 指定しない

                string kumitatePattern = "";

                if ( true == StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.IDNO.bindField ) ) ) {
                    // 検索条件構造体.IDNOが入力あり
                    // 取得結果 = MsSagyoDao.SelectData( 検索条件構造体.IDNO )
                    DataTable result = MsSagyoDao.SelectData( DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.IDNO.bindField ) );

                    if ( 0 < result.Rows.Count && true == ObjectUtils.IsNotNull( result.Rows[0][K_PATTERN] ) ) {
                        // 取得結果 != null
                        // kumitatePattern = 取得結果.組立パターン.trim
                        kumitatePattern = result.Rows[0][K_PATTERN].ToString().Trim();
                    }

                } else if ( true == StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.KATASHIKI_CODE.bindField ) ) ) {
                    // 検索条件構造体.型式コードが入力あり
                    // 取得結果 = KatashikiMasterDao.getData(工場(28), 検索条件構造体.型式コード)
                    DataTable result = KatashikiMasterDao.SelectData( WebAppInstance.GetInstance().PlantCode, DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.KATASHIKI_CODE.bindField ) );

                    if ( 0 < result.Rows.Count && true == ObjectUtils.IsNotNull( result.Rows[0][K_PATTERN_CODE] ) ) {
                        // 取得結果 != null
                        // kumitatePattern = 取得結果.組立パターン.trim
                        kumitatePattern = result.Rows[0][K_PATTERN_CODE].ToString().Trim();
                    }
                }

                if ( true == StringUtils.IsNotEmpty( kumitatePattern ) ) {
                    // kumitatePattern != null

                    if ( kumitatePattern == K_PATTERN_TRACTOR ) {
                        // kumitatePattern == 組立パターントラクタ(32)

                        // shijiLevel = 指示レベルトラクタ(1)
                        shijiLevel = SHIJI_LEVEL_TRACTOR;
                        // searchSyubetu = 検索種別トラクタ(1)
                        searchSyubetu = SEARCH_SYUBETSU_TRACTOR;

                    } else if ( kumitatePattern == K_PATTERN_03_OEM ) {
                        // kumitatePattern == 組立パターン03OEM(19)

                        // shijiLevel = 指示レベル03エンジン(3)
                        shijiLevel = SHIJI_LEVEL_03_ENGINE;
                        // patternFlag = パターンフラグOEM(3)
                        patternFlag = PATTERN_FLAG_OEM;
                        // searchSyubetu = 検索種別03エンジンOEM(4)
                        searchSyubetu = SEARCH_SYUBETSU_03_ENGINE_OEM;

                    } else if ( kumitatePattern == K_PATTERN_03_TOUSAI ) {
                        // kumitatePattern == 組立パターン03搭載(15)

                        // shijiLevel = 指示レベル03エンジン(3)
                        shijiLevel = SHIJI_LEVEL_03_ENGINE;
                        // patternFlag = パターンフラグ搭載(2)
                        patternFlag = PATTERN_FLAG_TOUSAI;
                        // searchSyubetu = 検索種別03エンジン搭載(3)
                        searchSyubetu = SEARCH_SYUBETSU_03_ENGINE_TOUSAI;

                    } else if ( kumitatePattern == K_PATTERN_07_OEM ) {
                        // kumitatePattern == 組立パターン07OEM(18)

                        // shijiLevel = 指示レベル07エンジン(7)
                        shijiLevel = SHIJI_LEVEL_07_ENGINE;
                        // patternFlag = パターンフラグOEM(3)
                        patternFlag = PATTERN_FLAG_OEM;
                        // searchSyubetu = 検索種別07エンジンOEM(7)
                        searchSyubetu = SEARCH_SYUBETSU_07_ENGINE_OEM;

                    } else if ( kumitatePattern == K_PATTERN_07_TOUSAI ) {
                        // kumitatePattern == 組立パターン07搭載( 14 )

                        // shijiLevel = 指示レベル07エンジン(7)
                        shijiLevel = SHIJI_LEVEL_07_ENGINE;
                        // patternFlag = パターンフラグ搭載(2)
                        patternFlag = PATTERN_FLAG_TOUSAI;
                        // searchSyubetu = 検索種別07エンジン搭載(6)
                        searchSyubetu = SEARCH_SYUBETSU_07_ENGINE_TOUSAI;
                    }

                    // 検索条件構造体.製品種別(shijiLevel)
                    dicCondition[SearchProductOrder.CONDITION.SHIJI_LEVEL.bindField] = shijiLevel;
                    // 検索条件構造体.エンジン種別(patternFlag)
                    dicCondition[SearchProductOrder.CONDITION.PATTERN_FLAG.bindField] = patternFlag;
                    // 検索条件構造体.検索種別(searchSyubetu)
                    dicCondition[SEARCH_SYUBETSU] = searchSyubetu;

                }


            } else {
                if ( shijiLevel == SearchProductOrder.TRACTOR ) {
                    // 検索条件構造体.製品種別 =トラクタ

                    // 検索条件構造体.製品種別(トラクタ(1))
                    dicCondition[SearchProductOrder.CONDITION.SHIJI_LEVEL.bindField] = SHIJI_LEVEL_TRACTOR;
                    // searchSyubetu = 検索種別トラクタ(1)
                    searchSyubetu = SEARCH_SYUBETSU_TRACTOR;

                } else if ( shijiLevel == SearchProductOrder.ENGINE_03 ) {
                    // 検索条件構造体.製品種別 = 03エンジン

                    // 検索条件構造体.製品種別(03エンジン(3))
                    dicCondition[SearchProductOrder.CONDITION.SHIJI_LEVEL.bindField] = SHIJI_LEVEL_03_ENGINE;

                    if ( DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.PATTERN_FLAG.bindField ) == SearchProductOrder.PATTERN_FLG_ALL ) {
                        // 検索条件構造体.エンジン種別 = 全て

                        // patternFlag = パターンフラグ全て(1)
                        patternFlag = PATTERN_FLAG_ALL;
                        // searchSyubetu = 検索種別03エンジン全て(2)
                        searchSyubetu = SEARCH_SYUBETSU_03_ENGINE_ALL;

                    } else if ( DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.PATTERN_FLAG.bindField ) == SearchProductOrder.PATTERN_FLG_TOUSAI ) {
                        // 検索条件構造体.エンジン種別 = 搭載

                        // patternFlag = パターンフラグ搭載(2)
                        patternFlag = PATTERN_FLAG_TOUSAI;
                        // searchSyubetu = 検索種別03エンジン搭載(3)
                        searchSyubetu = SEARCH_SYUBETSU_03_ENGINE_TOUSAI;

                    } else if ( DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.PATTERN_FLAG.bindField ) == SearchProductOrder.PATTERN_FLG_OEM ) {
                        // 検索条件構造体.エンジン種別 = OEM

                        // patternFlag = パターンフラグOEM(3)
                        patternFlag = PATTERN_FLAG_OEM;
                        // searchSyubetu = 検索種別03エンジンOEM(4)
                        searchSyubetu = SEARCH_SYUBETSU_03_ENGINE_OEM;
                    }

                } else if ( shijiLevel == SearchProductOrder.ENGINE_07 ) {
                    // 検索条件構造体.製品種別 = 07エンジン

                    // 検索条件構造体.製品種別(07エンジン(7))
                    dicCondition[SearchProductOrder.CONDITION.SHIJI_LEVEL.bindField] = SHIJI_LEVEL_07_ENGINE;

                    if ( DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.PATTERN_FLAG.bindField ) == SearchProductOrder.PATTERN_FLG_ALL ) {
                        // 検索条件構造体.エンジン種別 = 全て(1)

                        // patternFlag = パターンフラグ全て(1)
                        patternFlag = PATTERN_FLAG_ALL;
                        // searchSyubetu = 検索種別07エンジン全て(5)
                        searchSyubetu = SEARCH_SYUBETSU_07_ENGINE_ALL;

                    } else if ( DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.PATTERN_FLAG.bindField ) == SearchProductOrder.PATTERN_FLG_TOUSAI ) {
                        // 検索条件構造体.エンジン種別 = 搭載(2)

                        // patternFlag = パターンフラグ搭載(2)
                        patternFlag = PATTERN_FLAG_TOUSAI;
                        // searchSyubetu = 検索種別07エンジン搭載(6)
                        searchSyubetu = SEARCH_SYUBETSU_07_ENGINE_TOUSAI;

                    } else if ( DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.PATTERN_FLAG.bindField ) == SearchProductOrder.PATTERN_FLG_OEM ) {
                        // 検索条件構造体.エンジン種別 = OEM(3)

                        // patternFlag = パターンフラグOEM(3)
                        patternFlag = PATTERN_FLAG_OEM;
                        // searchSyubetu = 検索種別07エンジンOEM(7)
                        searchSyubetu = SEARCH_SYUBETSU_07_ENGINE_OEM;
                    }
                }
            }

            // 検索条件構造体.エンジン種別設定(patternFlag)
            dicCondition[SearchProductOrder.CONDITION.PATTERN_FLAG.bindField] = patternFlag;
            // 検索条件構造体.検索種別設定(searchSyubetu)
            dicCondition[SEARCH_SYUBETSU] = searchSyubetu;

        }

        /// <summary>
        /// 製品別通過実績検索：実績検索
        /// </summary>
        /// <param name="dicCondition">検索条件</param>
        /// <returns>処理結果</returns>
        public static DataTable SearchJisseki( Dictionary<string, object> dicCondition ) {

            // 検索条件から各検索条件を取得する
            // 検索種別
            string searchSyubetu = dicCondition[SEARCH_SYUBETSU].ToString();
            // IDNO
            string idno = DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.IDNO.bindField );
            // 型式コード
            string katashikiCode = DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.KATASHIKI_CODE.bindField );
            // 国コード
            string kuniCode = DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.KUNI_CODE.bindField );
            // 型式名
            string katashikiName = DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.KATASHIKI_NAME.bindField );
            // 機番
            string kiban = DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.KIBAN.bindField );
            // 指示月度連番
            string tonyuYMNum = DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.TONYU_YM_NUM.bindField );
            // 確定月度連番
            string kakuteiYMNum = DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.KAKUTEI_YM_NUM.bindField );

            // 処理結果 = ImadokoDao.SelectList
            DataTable jissekiList = ImadokoDao.SelectList( searchSyubetu, idno, katashikiCode, kuniCode, katashikiName, kiban, tonyuYMNum, kakuteiYMNum );

            return jissekiList;

        }

        /// <summary>
        /// 製品別通過実績検索：ステーション通過順序検索
        /// </summary>
        /// <param name="dicCondition">検索条件</param>
        /// <returns>処理結果</returns>
        public static DataTable SearchStationTsuukaJunjo( Dictionary<string, object> dicCondition ) {

            // 検索条件から各検索条件を取得する
            // 指示レベル
            string shijiLevel = DataUtils.GetDictionaryStringVal( dicCondition, SearchProductOrder.CONDITION.SHIJI_LEVEL.bindField );

            // 処理結果 = StationTsuukaJunjoDao.SelectList
            DataTable stationList = StationTsuukaJunjoDao.SelectList( shijiLevel );

            return stationList;
        }

        /// <summary>
        /// 製品別通過実績検索：検索結果表示用リスト整形
        /// </summary>
        /// <param name="jissekiList">実績リスト</param>
        /// <param name="stationList">ステーションリスト</param>
        /// <param name="maxGridViewCount">表示件数上限</param>
        /// <param name="dicCondition">検索条件</param>
        /// <param name="result">処理結果</param>
        public static void EditDisplayData( DataTable jissekiList, DataTable stationList, int maxGridViewCount, Dictionary<string, object> dicCondition, ref ResultSet result ) {

            // 表示用検索結果テーブル = 実績検索結果.clone()
            DataTable displayDt = jissekiList.Clone();

            // 追加位置(確定順序のあと)
            int addIdx = displayDt.Columns.IndexOf( SearchProductOrder.GRID_SEARCHPRODUCTORDER.KAKUTEI_YM_NUM.bindField );

            foreach ( DataRow dr in stationList.Rows ) {
                // 表示用検索結果テーブルにステーション通過順序検索結果のカラムを追加
                displayDt.Columns.Add( dr[DISPLAY_NM].ToString() ).SetOrdinal( addIdx );
                addIdx++;
            }

            string prevIdno = "";
            DataRow workDt = null;

            for ( int idx = 0; idx < jissekiList.Rows.Count; idx++ ) {
                // 実績検索結果分

                if ( workDt == null || false == jissekiList.Rows[idx][SearchProductOrder.GRID_SEARCHPRODUCTORDER.IDNO.bindField].Equals( prevIdno ) ) {
                    // workDt = null || 現在実績検索結果.IDNO != prevIdno

                    // workDt = 表示用検索結果テーブル.newRow()
                    workDt = displayDt.NewRow();

                    if ( displayDt.Rows.Count == maxGridViewCount ) {
                        // 表示用検索結果テーブル.count = 表示上限件数
                        // 検索対象が多すぎる為、すべての結果は表示されません。条件を絞り込んでください。
                        result.Message = new Msg( MsgManager.MESSAGE_WRN_61020 );
                        // 連番列の追加
                        displayDt.Columns.Add( SearchProductOrder.GRID_SEARCHPRODUCTORDER.DISP_ORDER.bindField ).SetOrdinal( 0 );
                        // 処理結果.メイン情報へ表示用テーブルを設定
                        result.ListTable = displayDt;
                        return;
                    }

                    foreach ( DataColumn dc in jissekiList.Columns ) {
                        // 実績検索結果カラム分
                        // workDt[現在カラム] = 現在実績検索結果[現在カラム]
                        workDt.SetField( dc.ColumnName, jissekiList.Rows[idx][dc] );
                    }

                    // 表示用検索結果テーブル.Add(workDt)
                    displayDt.Rows.Add( workDt );

                    // prevIdno = 現在実績検索結果.IDNO
                    prevIdno = jissekiList.Rows[idx][SearchProductOrder.GRID_SEARCHPRODUCTORDER.IDNO.bindField].ToString();

                }

                // colNm = ステーション通過順序検索結果から現在実績検索結果のステーションと一致する表示名を抽出
                var colNm = stationList.AsEnumerable().
                            Select( sl => new { Station = StringUtils.Nvl( sl[STATION_CODE] ), DisplayName = StringUtils.Nvl( sl[DISPLAY_NM] ) } ).
                            Where( slo => slo.Station == StringUtils.Nvl( jissekiList.Rows[idx][STATION] ) ).
                            Select( slo => slo.DisplayName ).
                            FirstOrDefault();

                if ( true == StringUtils.IsNotEmpty( colNm ) ) {
                    // colNmが抽出できた場合
                    // workDt[colNm] = 現在実績検索結果[実績日]
                    workDt[colNm] = jissekiList.Rows[idx][JISSEKI_DT].ToString();
                }
            }
            // 連番列の追加
            displayDt.Columns.Add( SearchProductOrder.GRID_SEARCHPRODUCTORDER.DISP_ORDER.bindField ).SetOrdinal( 0 );
            // 処理結果.メイン情報へ表示用テーブルを設定
            result.ListTable = displayDt;
            // "検索台数 : " + dataCount + "台"
            result.Message = new Msg( MsgManager.MESSAGE_INF_50030, result.ListTable.Rows.Count.ToString() );
        }


        #endregion

        #region エンジン立体倉庫在庫検索
        /// <summary>
        /// エンジン立体倉庫在庫検索：検索処理
        /// </summary>
        /// <param name="dicCondition">検索条件</param>
        /// <param name="maxGridViewCount">検索上限数</param>
        /// <param name="searchTarget">検索対象</param>
        /// <returns>処理結果</returns>
        public static ResultSet SearchOfSearchEngineStock( ref Dictionary<string, object> dicCondition, int maxGridViewCount, string searchTarget ) {

            // 処理結果の定義
            ResultSet result = new ResultSet();

            // 検索条件整形(ref 検索条件構造体)
            SearchConditionFormatOfSearchEngineStock( ref dicCondition );

            // 処理結果.立体別在庫数 = 共通処理.立体別在庫検索
            result.ListTableRittaibetsuZaiko = TeRittaiLocationDao.SelectRittaiZaiko();

            // 処理結果.種別在庫数 = 共通処理.エンジン生産種別在庫検索
            result.ListTableSyubetsuZaiko = TeRittaiLocationDao.SelectSeisanSyubetsuZaiko();

            if ( searchTarget == SearchEngineStock.SearchTarget.KATASHIKIBETSU.ToString() ) {
                // SEARCH_TARGET = 型式別(1)
                // 共通処理.型式別在庫検索
                SearchKatashikibetsuZaiko( dicCondition, maxGridViewCount, ref result );

            } else {
                // SEARCH_TARGET = 全在庫(2)
                // 共通処理.全在庫検索
                SelectRittaiZaikoAll( dicCondition, maxGridViewCount, ref result );

                // キャプション設定で失敗するため、取得結果の列定義に棚番を追加する
                result.ListTable.Columns.Add( SearchEngineStock.GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.LOCATION_NAME.bindField ).SetOrdinal( 1 );

                // ソートが動作しないため、号機、連、段、列を結合して棚番のデータに設定する
                var stockKbnIdx = result.ListTable.Columns.IndexOf( SearchEngineStock.GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.STOCK_KBN.bindField );
                var stockRenIdx = result.ListTable.Columns.IndexOf( SearchEngineStock.GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.STOCK_REN.bindField );
                var stockDanIdx = result.ListTable.Columns.IndexOf( SearchEngineStock.GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.STOCK_DAN.bindField );
                var stockRetsuIdx = result.ListTable.Columns.IndexOf( SearchEngineStock.GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.STOCK_RETSU.bindField );
                var tanabanIdx = result.ListTable.Columns.IndexOf( SearchEngineStock.GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.LOCATION_NAME.bindField );
                foreach ( DataRow dr in result.ListTable.Rows ) {
                    var locNm = dr[stockKbnIdx].ToString() + "-";
                    locNm += dr[stockRenIdx].ToString() + "-";
                    locNm += dr[stockDanIdx].ToString() + "-";
                    locNm += dr[stockRetsuIdx].ToString();
                    dr[tanabanIdx] = locNm;
                }
            }

            // 取得結果の列定義にNOを追加する
            result.ListTable.Columns.Add( SearchEngineStock.GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_1_L.DISP_ORDER.bindField ).SetOrdinal( 0 );

            return result;
        }

        /// <summary>
        /// エンジン立体倉庫在庫検索：検索条件整形
        /// </summary>
        /// <param name="dicCondition">検索条件</param>
        public static void SearchConditionFormatOfSearchEngineStock( ref Dictionary<string, object> dicCondition ) {
            // 検索条件構造体.型式コード(検索条件構造体.型式コードの大文字変換とハイフンを削除)
            dicCondition[SearchEngineStock.CONDITION.KATASHIKI_CODE.bindField] = getValueKatasikiCode( DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.KATASHIKI_CODE.bindField ) );

            if ( true == StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.KATASHIKI_NAME.bindField ) ) ) {
                // 検索条件構造体.型式名 != null
                // 検索条件構造体.型式名(検索条件構造体.型式名を大文字に変換)
                dicCondition[SearchEngineStock.CONDITION.KATASHIKI_NAME.bindField] = DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.KATASHIKI_NAME.bindField ).ToUpper();
            }
            if ( true == StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.KIBAN.bindField ) ) ) {
                // 検索条件構造体.機番 != null
                // 検索条件構造体.機番(検索条件構造体.機番を大文字に変換)
                dicCondition[SearchEngineStock.CONDITION.KIBAN.bindField] = DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.KIBAN.bindField ).ToUpper();
            }
        }

        /// <summary>
        /// エンジン立体倉庫在庫検索：型式別在庫検索
        /// </summary>
        /// <param name="dicCondition">検索条件</param>
        /// <param name="maxGridViewCount">検索上限数</param>
        /// <param name="result">処理結果</param>
        public static void SearchKatashikibetsuZaiko( Dictionary<string, object> dicCondition, int maxGridViewCount, ref ResultSet result ) {
            // 立体倉庫
            var rittaiNumVal = DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.RITTAI_NUM.bindField );
            var rittaiNum = convertToValueRittai( rittaiNumVal );
            // 禁止棚フラグ
            var stopFlagVal = DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.STOP_FLAG.bindField );
            var stopFlag = convertToValueRittaiStop( stopFlagVal );
            // ロケーションフラグ
            var locationFlagVal = DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.LOCATION_FLAG.bindField );
            var locationFlag = convertToValueLocation( locationFlagVal );
            // エンジン種別
            var engineSyubetsuVal = DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.ENGINE_SYUBETSU.bindField );
            var engineSyubetsu = convertToValueEngineSyubetsu( engineSyubetsuVal );
            // 搭載OEM
            var tousaiOemVal = DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.TOUSAI_OEM.bindField );
            var tousaiOem = convertToValueTousaiOem( tousaiOemVal );
            // 内外作
            var naigaisakuVal = DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.NAIGAISAKU.bindField );
            var naigaisaku = convertToValueNaigaisaku( naigaisakuVal );
            // 運転フラグ
            var untenFlagVal = DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.UNTEN_FLAG.bindField );
            var untenFlag = convertToValueUnten( untenFlagVal );
            // IDNO
            var idno = DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.IDNO.bindField );
            // 機番
            var kiban = DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.KIBAN.bindField );
            // 特記事項
            var tokki = DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.TOKKI.bindField );
            // 型式コード
            var katashikiCode = DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.KATASHIKI_CODE.bindField );
            // 型式名
            var katashikiName = DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.KATASHIKI_NAME.bindField );

            // 型式別在庫検索を実行する
            result.ListTable = TeRittaiLocationDao.SelectKatashikiZaiko( rittaiNum, stopFlag, locationFlag, engineSyubetsu, tousaiOem, naigaisaku, untenFlag, idno, kiban, tokki, katashikiCode, katashikiName, ref result, maxGridViewCount );

            if ( result.ListTable.Rows.Count == 0 ) {
                // 検索結果が0件の場合、エラーメッセージを設定
                result.Message = new Msg( MsgManager.MESSAGE_WRN_61010 );
            } else {
                if ( null == result.Message ) {
                    // Dao側で警告メッセージを設定していなければ成功メッセージを設定

                    // 検索時刻
                    DateTime dt = DateTime.Now;
                    // 検索時刻 + " 現在"
                    result.Message = new Msg( MsgManager.MESSAGE_INF_50040, dt );
                }
            }
        }

        /// <summary>
        /// エンジン立体倉庫在庫検索：全在庫検索（型式別IDNO一覧用 ソート条件固定検索）
        /// </summary>
        /// <param name="rittaiNum">立体倉庫</param>
        /// <param name="stopFlag">禁止棚フラグ</param>
        /// <param name="locationFlag">ロケーションフラグ</param>
        /// <param name="engineSyubetsu">エンジン種別</param>
        /// <param name="tousaiOem">搭載OEM</param>
        /// <param name="naigaisaku">内外作</param>
        /// <param name="untenFlag">運転フラグ</param>
        /// <param name="idno">IDNO</param>
        /// <param name="kiban">機番</param>
        /// <param name="tokki">特記事項</param>
        /// <param name="katashikiCode">型式コード</param>
        /// <param name="katashikiName">型式名</param>
        /// <returns>処理結果</returns>
        public static DataTable SelectRittaiZaikoAllForKatashikiIdnoList( string rittaiNum, string stopFlag, string locationFlag, string engineSyubetsu, string tousaiOem, string naigaisaku, string untenFlag, string idno, string kiban, string tokki, string katashikiCode, string katashikiName ) {

            // 型式コード(型式コードの大文字変換とハイフンを削除)
            katashikiCode = getValueKatasikiCode( katashikiCode );

            if ( true == StringUtils.IsNotEmpty( katashikiName ) ) {
                // 型式名 != null
                // 型式名(型式名を大文字に変換)
                katashikiName = katashikiName.ToUpper();
            }
            if ( true == StringUtils.IsNotEmpty( kiban ) ) {
                // 機番 != null
                // 機番(検索条件構造体.機番を大文字に変換)
                kiban = kiban.ToUpper();
            }

            // 表示名に変換
            // 立体倉庫
            rittaiNum = convertToValueRittai( rittaiNum );
            // 禁止棚フラグ
            stopFlag = convertToValueRittaiStop( stopFlag );
            // ロケーションフラグ
            locationFlag = convertToValueLocation( locationFlag );
            // エンジン種別
            engineSyubetsu = convertToValueEngineSyubetsu( engineSyubetsu );
            // 搭載OEM
            tousaiOem = convertToValueTousaiOem( tousaiOem );
            // 内外作
            naigaisaku = convertToValueNaigaisaku( naigaisaku );
            // 運転フラグ
            untenFlag = convertToValueUnten( untenFlag );

            // 全在庫検索（型式別IDNO一覧用 ソート条件固定検索）を実行する
            DataTable zenzaikoDtForDialog = TeRittaiLocationDao.SelectRittaiZaikoAllForKatashikiIdnoList( rittaiNum, stopFlag, locationFlag, engineSyubetsu, tousaiOem, naigaisaku, untenFlag, idno, kiban, tokki, katashikiCode, katashikiName );

            // 取得結果の列定義にNOを追加する
            zenzaikoDtForDialog.Columns.Add( SearchStationOrder.GRID_SEARCHSTATIONORDER.DISP_ORDER.bindField ).SetOrdinal( 0 );

            return zenzaikoDtForDialog;
        }

        /// <summary>
        /// エンジン立体倉庫在庫検索：全在庫検索
        /// </summary>
        /// <param name="dicCondition">検索条件</param>
        /// <param name="maxGridViewCount">検索上限数</param>
        /// <param name="result">処理結果</param>
        /// <returns>処理結果</returns>
        public static void SelectRittaiZaikoAll( Dictionary<string, object> dicCondition, int maxGridViewCount, ref ResultSet result ) {
            // 立体倉庫
            var rittaiNumVal = DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.RITTAI_NUM.bindField );
            var rittaiNum = convertToValueRittai( rittaiNumVal );
            // 禁止棚フラグ
            var stopFlagVal = DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.STOP_FLAG.bindField );
            var stopFlag = convertToValueRittaiStop( stopFlagVal );
            // ロケーションフラグ
            var locationFlagVal = DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.LOCATION_FLAG.bindField );
            var locationFlag = convertToValueLocation( locationFlagVal );
            // エンジン種別
            var engineSyubetsuVal = DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.ENGINE_SYUBETSU.bindField );
            var engineSyubetsu = convertToValueEngineSyubetsu( engineSyubetsuVal );
            // 搭載OEM
            var tousaiOemVal = DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.TOUSAI_OEM.bindField );
            var tousaiOem = convertToValueTousaiOem( tousaiOemVal );
            // 内外作
            var naigaisakuVal = DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.NAIGAISAKU.bindField );
            var naigaisaku = convertToValueNaigaisaku( naigaisakuVal );
            // 運転フラグ
            var untenFlagVal = DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.UNTEN_FLAG.bindField );
            var untenFlag = convertToValueUnten( untenFlagVal );
            // IDNO
            var idno = DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.IDNO.bindField );
            // 機番
            var kiban = DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.KIBAN.bindField );
            // 特記事項
            var tokki = DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.TOKKI.bindField );
            // 型式コード
            var katashikiCode = DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.KATASHIKI_CODE.bindField );
            // 型式名
            var katashikiName = DataUtils.GetDictionaryStringVal( dicCondition, SearchEngineStock.CONDITION.KATASHIKI_NAME.bindField );

            // 全在庫検索を実行する
            result.ListTable = TeRittaiLocationDao.SelectRittaiZaikoAll( rittaiNum, stopFlag, locationFlag, engineSyubetsu, tousaiOem, naigaisaku, untenFlag, idno, kiban, tokki, katashikiCode, katashikiName, ref result, maxGridViewCount );

            if ( result.ListTable.Rows.Count == 0 ) {
                // 検索結果が0件の場合、エラーメッセージを設定
                result.Message = new Msg( MsgManager.MESSAGE_WRN_61010 );
            } else {
                if ( null == result.Message ) {
                    // Dao側で警告メッセージを設定していなければ成功メッセージを設定

                    // 検索時刻
                    DateTime dt = DateTime.Now;
                    // 検索時刻 + " 現在"
                    result.Message = new Msg( MsgManager.MESSAGE_INF_50040, dt );
                }
            }
        }
        #endregion


        #region 搭載エンジン引当
        /// <summary>
        /// 搭載エンジン引当画面：検索処理
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="columnsDef">列定義</param>
        /// <param name="maxRecordCount">最大検索件数</param>
        /// <returns>処理結果</returns>
        public static ResultSet SearchOfSearchCallEngineSimulation( Dictionary<string, object> condition, GridViewDefine[] columnsDef, int maxRecordCount ) {
            // 処理結果を初期化する
            var result = new ResultSet();

            string tractorKanseiYoteiYMD = null;
            if ( true == StringUtils.IsEmpty( DataUtils.GetDictionaryStringVal( condition, SearchCallEngineSimulation.CONDITION.T_KANSEI_YOTEI_YMD.bindField ) ) ) {
                //(T)完成予定日が空の場合、1日後の稼働日を設定する
#if !DEBUG
                DateTime dt = CalendarUtils.GetOffsetWorkday( DateTime.Now, NEXT_WORK_DAY );
                tractorKanseiYoteiYMD = dt.ToShortDateString();
#endif
            } else {
                //(T)完成予定日が空でない場合、日付のみを取得
                tractorKanseiYoteiYMD = DataUtils.GetDictionaryDateVal( condition, SearchCallEngineSimulation.CONDITION.T_KANSEI_YOTEI_YMD.bindField ).ToShortDateString();
            }

            // 検索条件.( T )型式コード( 検索条件.( T )型式コードの大文字変換とハイフンを削除 )
            string tractorKatashikiCode = getValueKatasikiCode( DataUtils.GetDictionaryStringVal( condition, SearchCallEngineSimulation.CONDITION.T_KATASHIKI_CODE.bindField ) );
            // 検索条件.( E )型式コード( 検索条件.( E )型式コードの大文字変換とハイフンを削除 )
            string katashikiCode = getValueKatasikiCode( DataUtils.GetDictionaryStringVal( condition, SearchCallEngineSimulation.CONDITION.E_KATASHIKI_CODE.bindField ) );

            string katashikiName = DataUtils.GetDictionaryStringVal( condition, SearchCallEngineSimulation.CONDITION.E_KATASHIKI_NAME.bindField );
            if ( true == StringUtils.IsNotEmpty( katashikiName ) ) {
                //(E)型式名がnullでないときは大文字にする
                katashikiName = DataUtils.GetDictionaryStringVal( condition, SearchCallEngineSimulation.CONDITION.E_KATASHIKI_NAME.bindField ).ToUpper();
            }

            // 検索条件から各検索条件を取得する
            // (T)IDNO
            string tractorIdno = DataUtils.GetDictionaryStringVal( condition, SearchCallEngineSimulation.CONDITION.T_IDNO.bindField );
            // (T)型式名
            string tractorKatashikiName = DataUtils.GetDictionaryStringVal( condition, SearchCallEngineSimulation.CONDITION.T_KATASHIKI_NAME.bindField );
            // (T)特記事項
            string tractorTokki = DataUtils.GetDictionaryStringVal( condition, SearchCallEngineSimulation.CONDITION.T_TOKKI.bindField );
            // 引当先倉庫
            string callEngineWarehouse = DataUtils.GetDictionaryStringVal( condition, SearchCallEngineSimulation.CONDITION.CALL_ENGINE_WAREHOUSE.bindField );

            // 暗黙の検索条件
            // 引当完了データ表示限界（台）：ミッション立体出庫してから画面に表示している台数の初期値：10
            int displayKanryoNum = DISPLAY_KANRYO_NUM;
            // ミッション立体倉庫直前のステーション：214001
            string jissekiStationPrev = JISSEKI_STATION_PREV;
            // ミッション立体倉庫直後のステーション：214012
            string jissekiStationPost = JISSEKI_STATION_POST;

            // 処理結果 = TeRittaiLocationDao.SelectTousaiEngineList
            result.ListTable = TeRittaiLocationDao.SelectTousaiEngineList( tractorKanseiYoteiYMD, tractorKatashikiCode, katashikiCode, katashikiName, tractorIdno, tractorKatashikiName, tractorTokki, displayKanryoNum, jissekiStationPrev, jissekiStationPost, callEngineWarehouse, ref result, maxRecordCount );
            // 取得結果の列定義にNOを追加する
            result.ListTable.Columns.Add( SearchCallEngineSimulation.GRID_SEARCHCALLENGINESIMULATION.DISP_ORDER.bindField ).SetOrdinal( 0 );
            // 列定義から列名のキャプションを設定する
            columnsDef.ToList().ForEach( cd => {
                result.ListTable.Columns[cd.bindField].Caption = cd.headerText;
            } );

            if ( result.ListTable.Rows.Count == 0 ) {
                // 検索結果が0件の場合、エラーメッセージを設定
                result.Message = new Msg( MsgManager.MESSAGE_WRN_61010 );
            } else {
                if ( null == result.Message ) {
                    // Dao側で警告メッセージを設定していなければ成功メッセージを設定

                    // 検索時刻
                    DateTime dt = DateTime.Now;
                    // 検索時刻 + " 現在"
                    result.Message = new Msg( MsgManager.MESSAGE_INF_50040, dt );
                }
            }
            return result;
        }
        #endregion


        #region 型変換
        /// <summary>
        /// yyyyMMnnnnn形式の月度連番をyyyyMM-nnnnn形式にして返す
        /// </summary>
        ///  <param name="ymNum">yyyyMMnnnnn形式の月度連番</param>
        /// <returns>処理結果</returns>
        public static String getDisplayYMNum( String ymNum ) {
            if ( StringUtils.IsBlank( ymNum ) ) {
                return "";
            }
            if ( ymNum.Length <= 6 ) {
                return ymNum;
            } else {
                return ymNum.Substring( 0, 6 ) + "-" + ymNum.Substring( 6 );
            }
        }

        /// yyyy/MM/dd HH:mm:ss形式の値をMM/dd形式にして返す
        /// </summary>
        ///  <param name="data">変換対象データ</param>
        /// <returns>処理結果</returns>
        public static String convertDatetimeToMD( String data, String result = "" ) {
            if ( StringUtils.IsBlank( data ) ) {
                return result;
            } else {
                return data.Substring( 5, 5 );
            }
        }

        /// yyyy/MM/dd HH:mm:ss形式の値をMM/dd HH:mm:ss形式にして返す
        /// </summary>
        ///  <param name="data">変換対象データ</param>
        /// <returns>処理結果</returns>
        public static String convertDatetimeToMDH( String data, String result = "" ) {

            DateTime dt;
            if ( DateTime.TryParse( data, out dt ) ) {
                // 日付チェック
                // 時間の0埋めのためDateTime型へ変換し、フォーマット変換
                DateTime date = DateTime.Parse( data );
                data = date.ToString( DateUtils.DATE_FORMAT_SECOND );
                return data.Substring( 5, 14 );
            } else {
                return result;
            }
        }

        /// yyyy/MM/dd HH:mm:ss形式の値をyy/MM/dd HH:mm:ss形式にして返す
        /// </summary>
        ///  <param name="data">変換対象データ</param>
        /// <returns>処理結果</returns>
        public static String convertDatetimeToYYMDH( String data, String result = "" ) {

            DateTime dt;
            if ( DateTime.TryParse( data, out dt ) ) {
                // 日付チェック
                // 時間の0埋めのためDateTime型へ変換し、フォーマット変換
                DateTime date = DateTime.Parse( data );
                data = date.ToString( DateUtils.DATE_FORMAT_SECOND );
                return data.Substring( 2, 17 );
            } else {
                return result;
            }
        }

        /// <summary>
        /// 9999999999形式の型式コードを99999-99999形式にして返す
        /// </summary>
        ///  <param name="katashikiCode">9999999999形式の型式コード</param>
        /// <returns>処理結果</returns>
        public static String getDisplayKatasikiCode( String katashikiCode ) {
            if ( StringUtils.IsBlank( katashikiCode ) ) {
                return "";
            }
            katashikiCode = katashikiCode.ToUpper();
            if ( katashikiCode.Length <= 5 ) {
                return katashikiCode;
            } else {
                return katashikiCode.Substring( 0, 5 ) + "-" + katashikiCode.Substring( 5 );
            }
        }

        /// <summary>
        /// 99999-99999形式の型式コードを大文字変換し9999999999形式にして返す
        /// </summary>
        ///  <param name="katashikiCode">99999-99999形式の型式コード</param>
        /// <returns>処理結果</returns>
        public static String getValueKatasikiCode( String katashikiCode ) {
            if ( StringUtils.IsBlank( katashikiCode ) ) {
                return "";
            }
            katashikiCode = katashikiCode.Replace( "-", "" );
            return katashikiCode.ToUpper();
        }

        /// データが空であれば"-"にして返す
        /// </summary>
        ///  <param name="data">変換対象データ</param>
        /// <returns>処理結果</returns>
        public static String convertEmptyToHyphen( String data ) {
            if ( StringUtils.IsBlank( data ) ) {
                return "-";
            } else {
                return data;
            }
        }

        /// 基準日より古いか判定する
        /// </summary>
        ///  <param name="checkDt">チェック対象日付</param>
        ///  <param name="baseDt">基準日</param>
        /// <returns>チェック結果</returns>
        public static bool checkOlderBaseDate( String checkDtStr, String baseDtStr ) {
            DateTime checkDt = DateTime.Parse( checkDtStr );
            DateTime baseDt = DateTime.Parse( baseDtStr );
            if ( checkDt <= baseDt ) {
                // 古ければtrueを返す
                return true;
            }
            return false;
        }

        #region 変換(コード値→表示名)
        /// <summary>
        /// 種別変換(コード値→表示名)
        /// </summary>
        /// <param name="data">変換対象データ</param>
        /// <returns>変換結果</returns>
        public static string convertToDisplayEngineSyubetsu( string data ) {
            if ( data == ENGINE_SYUBETSU_03 ) {
                // 種別 == 1
                data = ENGINE_SYUBETSU_03_DISP;
            } else if ( data == ENGINE_SYUBETSU_07 ) {
                // 種別 == 2
                data = ENGINE_SYUBETSU_07_DISP;
            } else {
                // 該当なし
                data = "";
            }
            return data;
        }

        /// <summary>
        /// 搭載/OEM変換(コード値→表示名)
        /// </summary>
        /// <param name="data">変換対象データ</param>
        /// <returns>変換結果</returns>
        public static string convertToDisplayTousaiOem( string data ) {
            if ( data == ENGINE_TOUSAIOEM_TOUSAI ) {
                // 搭載/OEM == 1
                data = ENGINE_TOUSAIOEM_TOUSAI_DISP;
            } else if ( data == ENGINE_TOUSAIOEM_OEM ) {
                // 搭載/OEM == 2
                data = ENGINE_TOUSAIOEM_OEM_DISP;
            } else {
                // 該当なし
                data = "";
            }
            return data;
        }

        /// <summary>
        /// 内外作変換(コード値→表示名)
        /// </summary>
        /// <param name="data">変換対象データ</param>
        /// <returns>変換結果</returns>
        public static string convertToDisplayNaigaisaku( string data ) {
            if ( data == ENGINE_NAIGAISAKU_NAISAKU ) {
                // 内外作 == 0
                data = "";
            } else if ( data == ENGINE_NAIGAISAKU_SAKAI ) {
                // 内外作 == 1
                data = ENGINE_NAIGAISAKU_SAKAI_DISP;
            } else {
                // 該当なし
                data = "";
            }
            return data;
        }

        /// <summary>
        /// 立体倉庫変換(コード値→表示名)
        /// </summary>
        /// <param name="data">変換対象データ</param>
        /// <returns>変換結果</returns>
        public static string convertToDisplayRittai( string data ) {
            if ( data == RITTAI_TSUKUBA ) {
                // 立体倉庫 == 1
                data = RITTAI_TSUKUBA_DISP;
            } else if ( data == RITTAI_OEM ) {
                // 立体倉庫 == 2
                data = RITTAI_OEM_DISP;
            } else if ( data == RITTAI_SAKAI ) {
                // 立体倉庫 == 3
                data = RITTAI_SAKAI_DISP;
            } else if ( data == RITTAI_TOSOUGO ) {
                // 立体倉庫 == 4
                data = RITTAI_TOSOUGO_DISP;
            } else {
                // 該当なし
                data = "";
            }
            return data;
        }

        /// <summary>
        /// 禁止棚変換(コード値→表示名)
        /// </summary>
        /// <param name="data">変換対象データ</param>
        /// <returns>変換結果</returns>
        public static string convertToDisplayRittaiStop( string data ) {
            if ( data == RITTAI_STOP_NORMAL ) {
                // 禁止棚 == 0
                data = "";
            } else if ( data == RITTAI_STOP_STOP ) {
                // 禁止棚 == 1
                data = RITTAI_STOP_STOP_DISP;
            } else {
                // 該当なし
                data = "";
            }
            return data;
        }

        /// <summary>
        /// 状態変換(コード値→表示名)
        /// </summary>
        /// <param name="data">変換対象データ</param>
        /// <returns>変換結果</returns>
        public static string convertToDisplayLocation( string data ) {
            if ( data == LOCATION_FLAG_NOENGINE ) {
                // 状態 == 0
                data = LOCATION_FLAG_NOENGINE_DISP;
            } else if ( data == LOCATION_FLAG_ANYENGINE ) {
                // 状態 == 99
                data = LOCATION_FLAG_ANYENGINE_DISP;
            } else if ( data == LOCATION_FLAG_STOCKED ) {
                // 状態 == 1
                data = LOCATION_FLAG_STOCKED_DISP;
            } else if ( data == LOCATION_FLAG_ACCEPTED ) {
                // 状態 == 2
                data = LOCATION_FLAG_ACCEPTED_DISP;
            } else if ( data == LOCATION_FLAG_DELIVERED ) {
                // 状態 == 3
                data = LOCATION_FLAG_DELIVERED_DISP;
            } else if ( data == LOCATION_FLAG_PALETTE ) {
                // 状態 == 4
                data = LOCATION_FLAG_PALETTE_DISP;
            } else {
                // 該当なし
                data = "";
            }
            return data;
        }

        /// <summary>
        /// 運転変換(コード値→表示名)
        /// </summary>
        /// <param name="data">変換対象データ</param>
        /// <returns>変換結果</returns>
        public static string convertToDisplayUnten( string data ) {
            if ( data == ENGINE_UNTEN_BEFORE ) {
                // 運転 == 0
                data = ENGINE_UNTEN_BEFORE_DISP;
            } else if ( data == ENGINE_UNTEN_AFTER ) {
                // 運転 == 1
                data = ENGINE_UNTEN_AFTER_DISP;
            } else {
                // 該当なし
                data = "";
            }
            return data;
        }
        #endregion

        #region 変換(表示名→コード値)
        /// <summary>
        /// 種別変換(表示名→コード値)
        /// </summary>
        /// <param name="data">変換対象データ</param>
        /// <returns>変換結果</returns>
        public static string convertToValueEngineSyubetsu( string data ) {
            if ( data == ENGINE_SYUBETSU_03_DISP ) {
                // 種別 == 03
                data = ENGINE_SYUBETSU_03;
            } else if ( data == ENGINE_SYUBETSU_07_DISP ) {
                // 種別 == 07
                data = ENGINE_SYUBETSU_07;
            } else {
                // 該当なし
                data = "";
            }
            return data;
        }

        /// <summary>
        /// 搭載/OEM変換(表示名→コード値)
        /// </summary>
        /// <param name="data">変換対象データ</param>
        /// <returns>変換結果</returns>
        public static string convertToValueTousaiOem( string data ) {
            if ( data == ENGINE_TOUSAIOEM_TOUSAI_DISP ) {
                // 搭載/OEM == 搭載
                data = ENGINE_TOUSAIOEM_TOUSAI;
            } else if ( data == ENGINE_TOUSAIOEM_OEM_DISP ) {
                // 搭載/OEM == OEM
                data = ENGINE_TOUSAIOEM_OEM;
            } else {
                // 該当なし
                data = "";
            }
            return data;
        }

        /// <summary>
        /// 内外作変換(表示名→コード値)
        /// </summary>
        /// <param name="data">変換対象データ</param>
        /// <returns>変換結果</returns>
        public static string convertToValueNaigaisaku( string data ) {
            if ( data == ENGINE_NAIGAISAKU_NAISAKU_DISP ) {
                // 内外作 == 内作
                data = ENGINE_NAIGAISAKU_NAISAKU;
            } else if ( data == ENGINE_NAIGAISAKU_SAKAI_DISP ) {
                // 内外作 == 堺
                data = ENGINE_NAIGAISAKU_SAKAI;
            } else {
                // 該当なし
                data = "";
            }
            return data;
        }

        /// <summary>
        /// 立体倉庫変換(表示名→コード値)
        /// </summary>
        /// <param name="data">変換対象データ</param>
        /// <returns>変換結果</returns>
        public static string convertToValueRittai( string data ) {
            if ( data == RITTAI_TSUKUBA_DISP ) {
                // 立体倉庫 == 筑波
                data = RITTAI_TSUKUBA;
            } else if ( data == RITTAI_OEM_DISP ) {
                // 立体倉庫 == OEM
                data = RITTAI_OEM;
            } else if ( data == RITTAI_SAKAI_DISP ) {
                // 立体倉庫 == 堺
                data = RITTAI_SAKAI;
            } else if ( data == RITTAI_TOSOUGO_DISP ) {
                // 立体倉庫 == 塗装後
                data = RITTAI_TOSOUGO;
            } else {
                // 該当なし
                data = "";
            }
            return data;
        }

        /// <summary>
        /// 禁止棚変換(表示名→コード値)
        /// </summary>
        /// <param name="data">変換対象データ</param>
        /// <returns>変換結果</returns>
        public static string convertToValueRittaiStop( string data ) {
            if ( data == RITTAI_STOP_NORMAL_DISP ) {
                // 禁止棚 == 通常
                data = RITTAI_STOP_NORMAL;
            } else if ( data == RITTAI_STOP_STOP_DISP ) {
                // 禁止棚 == 禁止棚
                data = RITTAI_STOP_STOP;
            } else {
                // 該当なし
                data = "";
            }
            return data;
        }

        /// <summary>
        /// 状態変換(表示名→コード値)
        /// </summary>
        /// <param name="data">変換対象データ</param>
        /// <returns>変換結果</returns>
        public static string convertToValueLocation( string data ) {
            if ( data == LOCATION_FLAG_NOENGINE_DISP ) {
                // 状態 == 空き
                data = LOCATION_FLAG_NOENGINE;
            } else if ( data == LOCATION_FLAG_ANYENGINE_DISP ) {
                // 状態 == 空き以外
                data = LOCATION_FLAG_ANYENGINE;
            } else if ( data == LOCATION_FLAG_STOCKED_DISP ) {
                // 状態 == 在席
                data = LOCATION_FLAG_STOCKED;
            } else if ( data == LOCATION_FLAG_ACCEPTED_DISP ) {
                // 状態 == 入庫
                data = LOCATION_FLAG_ACCEPTED;
            } else if ( data == LOCATION_FLAG_DELIVERED_DISP ) {
                // 状態 == 出庫
                data = LOCATION_FLAG_DELIVERED;
            } else if ( data == LOCATION_FLAG_PALETTE_DISP ) {
                // 状態 == 空ﾊﾟﾚ
                data = LOCATION_FLAG_PALETTE;
            } else {
                // 該当なし
                data = "";
            }
            return data;
        }

        /// <summary>
        /// 運転変換(表示名→コード値)
        /// </summary>
        /// <param name="data">変換対象データ</param>
        /// <returns>変換結果</returns>
        public static string convertToValueUnten( string data ) {
            if ( data == ENGINE_UNTEN_BEFORE_DISP ) {
                // 運転 == 運転前
                data = ENGINE_UNTEN_BEFORE;
            } else if ( data == ENGINE_UNTEN_AFTER_DISP ) {
                // 運転 == 完了
                data = ENGINE_UNTEN_AFTER;
            } else {
                // 該当なし
                data = "";
            }
            return data;
        }
        #endregion
        #endregion

        #region ステーション別通過実績検索
        /// <summary>
        /// ステーション別通過実績検索画面：検索処理
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="columnsDef">列定義</param>
        /// <param name="maxRecordCount">最大検索件数</param>
        /// <returns>処理結果</returns>
        public static ResultSet SearchOfSearchStationOrder( Dictionary<string, object> condition, GridViewDefine[] columnsDef, int maxRecordCount ) {
            var result = new ResultSet();

            //型式コードの大文字変換とハイフン削除
            condition[SearchStationOrder.CONDITION.KATASHIKI_CODE.bindField] = getValueKatasikiCode( DataUtils.GetDictionaryStringVal( condition, SearchStationOrder.CONDITION.KATASHIKI_CODE.bindField ) );

            if ( true == StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( condition, SearchStationOrder.CONDITION.KATASHIKI_NAME.bindField ) ) ) {
                //型式名が空でないときは大文字にする
                condition[SearchStationOrder.CONDITION.KATASHIKI_NAME.bindField] = DataUtils.GetDictionaryStringVal( condition, SearchStationOrder.CONDITION.KATASHIKI_NAME.bindField ).ToUpper();
            }

            if ( true == StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( condition, SearchStationOrder.CONDITION.KIBAN.bindField ) ) ) {
                //機番が空でないときは大文字にする
                condition[SearchStationOrder.CONDITION.KIBAN.bindField] = DataUtils.GetDictionaryStringVal( condition, SearchStationOrder.CONDITION.KIBAN.bindField ).ToUpper();
            }

            // ステーション
            var station = DataUtils.GetDictionaryStringVal( condition, SearchStationOrder.CONDITION.STATION.bindField );
            // 照会日
            string jissekiYMD;
            if ( true == StringUtils.IsEmpty( DataUtils.GetDictionaryStringVal( condition, SearchStationOrder.CONDITION.JISSEKI_YMD.bindField ) ) ) {
                //照会日が空の場合、"" を入れる
                jissekiYMD = "";
            } else {
                //照会日が空でない場合、日付のみを取得
                jissekiYMD = DataUtils.GetDictionaryDateVal( condition, SearchStationOrder.CONDITION.JISSEKI_YMD.bindField ).ToShortDateString();
            }

            // IDNO
            var idno = DataUtils.GetDictionaryStringVal( condition, SearchStationOrder.CONDITION.IDNO.bindField );
            // 機番
            var kiban = DataUtils.GetDictionaryStringVal( condition, SearchStationOrder.CONDITION.KIBAN.bindField );
            // 型式コード
            var katashikiCode = DataUtils.GetDictionaryStringVal( condition, SearchStationOrder.CONDITION.KATASHIKI_CODE.bindField );
            //国コード
            var kuniCode = DataUtils.GetDictionaryStringVal( condition, SearchStationOrder.CONDITION.KUNI_CODE.bindField );
            //型式名
            var katashikiName = DataUtils.GetDictionaryStringVal( condition, SearchStationOrder.CONDITION.KATASHIKI_NAME.bindField );
            //特記事項 
            var tokki = DataUtils.GetDictionaryStringVal( condition, SearchStationOrder.CONDITION.TOKKI.bindField );

            // 検索を実行する
            result.ListTable = MsZisekiDao.SelectStationJissekiList( station, jissekiYMD, idno, kiban, katashikiCode, kuniCode, katashikiName, tokki, maxRecordCount, ref result );
            // 取得結果の列定義にNOを追加する
            result.ListTable.Columns.Add( SearchStationOrder.GRID_SEARCHSTATIONORDER.DISP_ORDER.bindField ).SetOrdinal( 0 );

            // 列定義から列名のキャプションを設定する
            columnsDef.ToList().ForEach( cd => {
                result.ListTable.Columns[cd.bindField].Caption = cd.headerText;
            } );

            if ( result.ListTable.Rows.Count == 0 ) {
                // 検索結果が0件の場合、エラーメッセージを設定
                result.Message = new Msg( MsgManager.MESSAGE_WRN_61010 );
            }
            return result;
        }
        #endregion

        #region 順序情報検索
        /// <summary>
        /// 順序情報検索：検索処理
        /// </summary>
        /// <param name="dicCondition">検索条件</param>
        /// <param name="columnsDef">一覧列定義</param>
        /// <param name="maxGridViewCount">検索上限数</param>
        /// <returns>処理結果</returns>
        public static ResultSet SearchOfSearchOrderInfo( ref Dictionary<string, object> dicCondition, GridViewDefine[] columnsDef, int maxGridViewCount ) {
            // 処理結果の定義
            ResultSet result = new ResultSet();
            // 検索条件整形
            SearchConditionFormatOfSearchOrderInfo( ref dicCondition );

            switch ( DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.SHIJI_LEVEL.bindField ) ) {
            case SearchOrderInfo.ID_KIBAN_SEARCH:
                // 製品種別がID/機番検索の場合、作業情報取得
                result = SearchOfSagyoInfo( dicCondition, columnsDef, maxGridViewCount );
                break;
            case SearchOrderInfo.TRACTOR_FIX:
                // 製品種別が本機確定の場合、順序リスト2取得
                result = SearchOfOrderList2( dicCondition, columnsDef, maxGridViewCount );
                break;
            default:
                // 製品種別が上記以外の場合、順序リスト取得
                result = SearchOfOrderList( dicCondition, columnsDef, maxGridViewCount );
                break;
            }
            return result;
        }


        /// <summary>
        /// 順序情報検索：検索条件チェック
        /// </summary>
        /// <param name="dicCondition">検索条件</param>
        /// <param name="result">処理結果</param>
        /// <returns>処理結果</returns>
        public static ResultSet CheckSearchParamsOfSearchOrderInfo( Dictionary<string, object> dicCondition, ResultSet result ) {

            // IDNOの空白を削除
            dicCondition[SearchOrderInfo.CONDITION.IDNO.bindField] = DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.IDNO.bindField ).Trim();
            // 機番の空白を削除
            dicCondition[SearchOrderInfo.CONDITION.KIBAN.bindField] = DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.KIBAN.bindField ).Trim();

            if ( "" == DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.IDNO.bindField ) && "" == DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.KIBAN.bindField ) ) {
                // IDNO、機番がともに空文字の場合
                // エラーメッセージ：検索条件を入力してください。
                result.Message = new Msg( MsgManager.MESSAGE_WRN_62080, "検索条件" );
                result.Object = false;

                return result;
            } else if ( "" != DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.IDNO.bindField ) && IDNO7 > DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.IDNO.bindField ).Length ) {
                // IDNOが7文字未満の場合
                // エラーメッセージ：IDNOは7桁で入力してください。
                result.Message = new Msg( MsgManager.MESSAGE_WRN_62090, "IDNO", "7桁" );
                result.Object = false;

                return result;
            } else if ( "" != DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.KIBAN.bindField ) && KIBAN6 > DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.KIBAN.bindField ).Length ) {
                // 機番が6文字未満の場合
                // エラーメッセージ：機番は6桁又は7桁で入力してください。
                result.Message = new Msg( MsgManager.MESSAGE_WRN_62090, "機番", "6桁又は7桁" );
                result.Object = false;

                return result;
            }

            result.Object = true;

            return result;
        }

        /// <summary>
        /// 順序情報検索：検索条件整形
        /// </summary>
        /// <param name="dicCondition">検索条件</param>
        public static void SearchConditionFormatOfSearchOrderInfo( ref Dictionary<string, object> dicCondition ) {

            // 検索条件構造体にTARGET_SAGYO_KEEPを追加
            dicCondition.Add( TARGET_SAGYO_KEEP, "" );
            // 検索条件構造体に期を追加
            dicCondition.Add( PATTERN, "" );
            // 検索条件構造体に総称パターンを追加
            dicCondition.Add( GENERAL_PATTERN, "" );

            // IDNOの空白を削除
            dicCondition[SearchOrderInfo.CONDITION.IDNO.bindField] = DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.IDNO.bindField ).Trim();
            // 機番の空白を削除
            dicCondition[SearchOrderInfo.CONDITION.KIBAN.bindField] = DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.KIBAN.bindField ).Trim();

            if ( SearchOrderInfo.ID_KIBAN_SEARCH == DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.SHIJI_LEVEL.bindField ) ) {
                // 製品種別がID/機番検索の場合
                if ( true == StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.KIBAN.bindField ) ) ) {
                    // 機番がnullでない場合、機番を大文字に変換
                    dicCondition[SearchOrderInfo.CONDITION.KIBAN.bindField] = DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.KIBAN.bindField ).ToUpper();
                }
                if ( KIBAN7 == DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.KIBAN.bindField ).Length ) {
                    // 機番が7桁の場合、1桁目を削除
                    dicCondition[SearchOrderInfo.CONDITION.KIBAN.bindField] = DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.KIBAN.bindField ).Substring( 1 );
                }
            } else {
                // 二か月前の月度を取得
                String systemDateYM = DateUtils.ToString( DateTime.Now, DateUtils.DATE_FORMAT_MONTH_NOSEP );
                var standardYM = DateUtils.ToDate( systemDateYM, DateUtils.FormatType.MonthNoSep ).AddMonths( -2 ).ToString( DateUtils.DATE_FORMAT_MONTH_NOSEP );

                if ( int.Parse( standardYM ) > int.Parse( DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.SHIJI_YM.bindField ) ) ) {
                    // 月度が二か月前より古い場合
                    dicCondition[TARGET_SAGYO_KEEP] = false;
                } else {
                    dicCondition[TARGET_SAGYO_KEEP] = true;
                }

                // 決算期取得
                dicCondition[PATTERN] = CalendarUtils.GetFiscalYear( DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.SHIJI_YM.bindField ), DateUtils.DATE_FORMAT_MONTH_NOSEP );

                if ( SearchOrderInfo.ENGINE_03 == DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.SHIJI_LEVEL.bindField ) || SearchOrderInfo.ENGINE_07 == DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.SHIJI_LEVEL.bindField ) ) {
                    // 製品種別が03エンジンまたは07エンジンの場合
                    // 検索条件の総称パターンに総称パターンエンジンをセット
                    dicCondition[GENERAL_PATTERN] = SPATTERN_ENGINE;
                } else {
                    // 検索条件の総称パターンに総称パターントラクタをセット
                    dicCondition[GENERAL_PATTERN] = SPATTERN_TRACTOR;
                }

                //型式コードの大文字変換とハイフン削除
                dicCondition[SearchOrderInfo.CONDITION.KATASHIKI_CODE.bindField] = getValueKatasikiCode( DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.KATASHIKI_CODE.bindField ) );

                if ( true == StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.KATASHIKI_NAME.bindField ) ) ) {
                    // 型式名がnullでない場合、型式名を大文字に変換
                    dicCondition[SearchOrderInfo.CONDITION.KATASHIKI_NAME.bindField] = DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.KATASHIKI_NAME.bindField ).ToUpper();
                }
                if ( true == StringUtils.IsNotEmpty( DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.KIBAN.bindField ) ) ) {
                    // 機番がnullでない場合、機番を大文字に変換
                    dicCondition[SearchOrderInfo.CONDITION.KIBAN.bindField] = DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.KIBAN.bindField ).ToUpper();
                }
            }
        }

        /// <summary>
        /// 順序情報検索画面：作業情報取得
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="columnsDef">一覧列定義</param>
        /// <param name="maxRecordCount">最大検索件数</param>
        /// <returns>処理結果</returns>
        public static ResultSet SearchOfSagyoInfo( Dictionary<string, object> dicCondition, GridViewDefine[] columnsDef, int maxRecordCount ) {
            var result = new ResultSet();
            // 検索条件チェック
            result = CheckSearchParamsOfSearchOrderInfo( dicCondition, result );

            if ( false == (bool)result.Object ) {
                // チェック結果がfalseの場合、処理終了
                return result;
            }

            // IDNO
            var idno = DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.IDNO.bindField );
            // 機番
            var kiban = DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.KIBAN.bindField );
            // 月度
            var shijiYM = DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.SHIJI_YM.bindField );

            // 検索を実行する
            result.ListTable = MsSagyoDao.SelectJunjoListByIdKiban( idno, kiban, shijiYM, maxRecordCount, ref result );
            // 取得結果の列定義にNOを追加する
            result.ListTable.Columns.Add( SearchOrderInfo.GRID_SEARCHORDERINFO_ID_KIBAN_L.DISP_ORDER.bindField ).SetOrdinal( 0 );
            // 取得結果の列定義にEパタンを追加する
            result.ListTable.Columns.Add( SearchOrderInfo.GRID_SEARCHORDERINFO_ID_KIBAN_R.ENGINE_KUMITATE_PATTERN.bindField ).SetOrdinal( 18 );

            // 列定義から列名のキャプションを設定する
            columnsDef.ToList().ForEach( cd => {
                result.ListTable.Columns[cd.bindField].Caption = cd.headerText;
            } );

            if ( result.ListTable.Rows.Count == 0 ) {
                // 検索結果が0件の場合、エラーメッセージを設定
                result.Message = new Msg( MsgManager.MESSAGE_WRN_61010 );
            }

            return result;
        }

        /// <summary>
        /// 順序情報検索画面：順序リスト2取得
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="columnsDef">一覧列定義</param>
        /// <param name="maxRecordCount">最大検索件数</param>
        /// <returns>処理結果</returns>
        public static ResultSet SearchOfOrderList2( Dictionary<string, object> dicCondition, GridViewDefine[] columnsDef, int maxRecordCount ) {
            var result = new ResultSet();

            // 指示レベル
            var shijiLevel = SHIJI_LEVEL_TRACTOR_FIX;
            // IDNO
            var idno = DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.IDNO.bindField );
            // 機番
            var kiban = DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.KIBAN.bindField );
            // 月度
            var shijiYM = DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.SHIJI_YM.bindField );
            // 型式コード
            var katashikiCode = DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.KATASHIKI_CODE.bindField );
            // 国コード
            var kuniCode = DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.KUNI_CODE.bindField );
            // 型式名
            var katashikiName = DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.KATASHIKI_NAME.bindField );
            // 特記事項
            var tokki = DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.TOKKI.bindField );
            // TARGET_SAGYO_KEEP
            var targetSagyoKeep = (bool)dicCondition[TARGET_SAGYO_KEEP];
            // 期
            var pattern = dicCondition[PATTERN].ToString();
            // 総称パターン
            var generalPattern = dicCondition[GENERAL_PATTERN].ToString();

            // 検索を実行する
            result.ListTable = MsSagyoDao.SelectJunjoList2( shijiLevel, idno, kiban, shijiYM, katashikiCode, kuniCode, katashikiName, tokki, targetSagyoKeep, pattern, generalPattern, maxRecordCount, ref result );
            // 取得結果の列定義にNOを追加する
            result.ListTable.Columns.Add( SearchOrderInfo.GRID_SEARCHORDERINFO_TRACTOR_FIX_L.DISP_ORDER.bindField ).SetOrdinal( 0 );

            // 列定義から列名のキャプションを設定する
            columnsDef.ToList().ForEach( cd => {
                result.ListTable.Columns[cd.bindField].Caption = cd.headerText;
            } );

            if ( result.ListTable.Rows.Count == 0 ) {
                // 検索結果が0件の場合、エラーメッセージを設定
                result.Message = new Msg( MsgManager.MESSAGE_WRN_61010 );
            }
            return result;
        }

        /// <summary>
        /// 順序情報検索画面：順序リスト取得
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="columnsDef">一覧列定義</param>
        /// <param name="maxRecordCount">最大検索件数</param>
        /// <returns>処理結果</returns>
        public static ResultSet SearchOfOrderList( Dictionary<string, object> dicCondition, GridViewDefine[] columnsDef, int maxRecordCount ) {
            var result = new ResultSet();

            // 指示レベル
            string shijiLevel;
            if ( SearchOrderInfo.ENGINE_03 == DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.SHIJI_LEVEL.bindField ) ) {
                shijiLevel = SHIJI_LEVEL_03_ENGINE;
            } else if ( SearchOrderInfo.ENGINE_07 == DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.SHIJI_LEVEL.bindField ) ) {
                shijiLevel = SHIJI_LEVEL_07_ENGINE;
            } else {
                shijiLevel = SHIJI_LEVEL_MISSION_THROW;
            }
            // IDNO
            var idno = DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.IDNO.bindField );
            // 機番
            var kiban = DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.KIBAN.bindField );
            // 月度
            var shijiYM = DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.SHIJI_YM.bindField );
            // 型式コード
            var katashikiCode = DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.KATASHIKI_CODE.bindField );
            // 国コード
            var kuniCode = DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.KUNI_CODE.bindField );
            // 型式名
            var katashikiName = DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.KATASHIKI_NAME.bindField );
            // 特記事項
            var tokki = DataUtils.GetDictionaryStringVal( dicCondition, SearchOrderInfo.CONDITION.TOKKI.bindField );
            // TARGET_SAGYO_KEEP
            var targetSagyoKeep = (bool)dicCondition[TARGET_SAGYO_KEEP];
            // 期
            var pattern = dicCondition[PATTERN].ToString();
            // 総称パターン
            var generalPattern = dicCondition[GENERAL_PATTERN].ToString();


            // 検索を実行する
            result.ListTable = MsSagyoDao.SelectJunjoList( shijiLevel, idno, kiban, shijiYM, katashikiCode, kuniCode, katashikiName, tokki, targetSagyoKeep, pattern, generalPattern, maxRecordCount, ref result );
            // 取得結果の列定義にNOを追加する
            result.ListTable.Columns.Add( SearchOrderInfo.GRID_SEARCHORDERINFO_OTHER_L.DISP_ORDER.bindField ).SetOrdinal( 0 );

            // 列定義から列名のキャプションを設定する
            columnsDef.ToList().ForEach( cd => {
                result.ListTable.Columns[cd.bindField].Caption = cd.headerText;
            } );

            if ( result.ListTable.Rows.Count == 0 ) {
                // 検索結果が0件の場合、エラーメッセージを設定
                result.Message = new Msg( MsgManager.MESSAGE_WRN_61010 );
            }
            return result;
        }
        #endregion
    }
}