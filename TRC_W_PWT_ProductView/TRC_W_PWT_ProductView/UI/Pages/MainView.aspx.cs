using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
using System.Reflection;
using KTFramework.Common;
using KTFramework.Dao;
using KTWebInheritance.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.UI.Base;
using TRC_W_PWT_ProductView.Session;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.ListDefine;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.Business;
using TRC_W_PWT_ProductView.Dao.Process;
using TRC_W_PWT_ProductView.Dao.Com;
using KTFramework.C1Common.Excel;
using TRC_W_PWT_ProductView.SrvCore;

namespace TRC_W_PWT_ProductView.UI.Pages {

    /// <summary>
    /// メイン一覧ページ
    /// </summary>
    public partial class MainView : BaseForm {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義
        /// <summary>
        /// グリッドビューグループ
        /// </summary>
        const string GRID_MAIN_VIEW_GROUP_CD = "MainView";
        #endregion

        #region 検索条件定義
        /// <summary>
        /// 一覧検索条件定義
        /// </summary>
        public class CONDITION {
            /// <summary>検索区分</summary>
            public static readonly ControlDefine SEARCH_TYPE = new ControlDefine( "rblSearchType", "検索区分", "rblSearchType", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>製品種別</summary>
            public static readonly ControlDefine PRODUCT_KIND_CD = new ControlDefine( "rblProductKind", "製品種別", "productKindCd", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>製品型式(型式種別)</summary>
            public static readonly ControlDefine MODEL_TYPE = new ControlDefine( "rblModelType", "製品型式", "modelType", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>型式コード</summary>
            public static readonly ControlDefine MODEL_CD = new ControlDefine( "txtModelCd", "型式コード", "modelCd", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>型式名</summary>
            public static readonly ControlDefine MODEL_NM = new ControlDefine( "txtModelNm", "型式名", "modelNm", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>PINコード(チェックボックス)</summary>
            public static readonly ControlDefine PIN_CD_CHECK = new ControlDefine( "chkPinCd", "PINコード", "pinCdCheck", ControlDefine.BindType.Both, typeof( bool ) );
            /// <summary>PINコード</summary>
            public static readonly ControlDefine PIN_CD = new ControlDefine( "txtPinCd", "PINコード", "pinCd", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>製品機番</summary>
            public static readonly ControlDefine SERIAL = new ControlDefine( "txtProductSerial", "製品機番", "serial", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>IDNO</summary>
            public static readonly ControlDefine IDNO = new ControlDefine( "txtIDNO", "IDNO", "idno", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>工程コード</summary>
            public static readonly ControlDefine PROCESS_CD = new ControlDefine( "hdnProcessCd", "工程コード", "processCd", ControlDefine.BindType.Up, typeof( String ) );
            /// <summary>工程名</summary>
            public static readonly ControlDefine PROCESS_NM = new ControlDefine( "hdnProcessNm", "工程名", "processNm", ControlDefine.BindType.Up, typeof( String ) );
            /// <summary>ラインコード</summary>
            public static readonly ControlDefine LINE_CD = new ControlDefine( "hdnLineCd", "ラインコード", "lineCd", ControlDefine.BindType.Up, typeof( String ) );
            /// <summary>作業コード</summary>
            public static readonly ControlDefine WORK_CD = new ControlDefine( "hdnWorkCd", "作業コード", "workCd", ControlDefine.BindType.Up, typeof( String ) );
            /// <summary>作業名</summary>
            public static readonly ControlDefine WORK_NM = new ControlDefine( "hdnWorkNm", "作業名", "workNm", ControlDefine.BindType.Up, typeof( String ) );
            /// <summary>部品</summary>
            public static readonly ControlDefine PARTS_CD = new ControlDefine( "ddlParts", "部品", "partsCd", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>品番</summary>
            public static readonly ControlDefine PARTS_NUM = new ControlDefine( "txtPartsNo", "品番", "partsNum", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>部品機番</summary>
            public static readonly ControlDefine PARTS_SERIAL = new ControlDefine( "txtPartsSerial", "部品機番", "partsSerial", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>対象</summary>
            public static readonly ControlDefine DATE_KIND_CD = new ControlDefine( "ddlDateKind", "対象", "dateKindCd", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>範囲(開始)</summary>
            public static readonly ControlDefine DATE_FROM = new ControlDefine( "cldStart", "範囲", "dateFrom", ControlDefine.BindType.Both, typeof( DateTime ) );
            /// <summary>範囲(終了)</summary>
            public static readonly ControlDefine DATE_TO = new ControlDefine( "cldEnd", "範囲", "dateTo", ControlDefine.BindType.Both, typeof( DateTime ) );
            /// <summary>検索対象フラグ</summary>
            public static readonly ControlDefine SEARCH_TARGET_FLAG = new ControlDefine( "hdnSearchTargetFlg", "検索対象フラグ", "searchTargetFlg", ControlDefine.BindType.Up, typeof( String ) );
        }
        #endregion

        #region グリッドビュー定義

        /// <summary>
        /// 一覧表示情報コントロール(左下)
        /// </summary>
        public class GRID_SEARCH_CONTROLS_L {
        }
        /// <summary>チェックボックス</summary>

        /// <summary>
        /// 一覧表示情報コントロール(右下)
        /// </summary>
        public class GRID_SEARCH_CONTROLS_R {
        }

        #region 製品
        /// <summary>
        /// 製品：全製品共通検索一覧定義
        /// </summary>
        internal class GRID_PRODUCT_COMMON {
            /// <summary>製品状態(仕掛/在庫/出荷済)</summary>
            public static readonly GridViewDefine PRODUCT_STATUS = new GridViewDefine( "状態", "productStatus", typeof( string ), "", true, HorizontalAlign.Left, 60, true, true );
            /// <summary>組立パターンコード</summary>
            public static readonly GridViewDefine ASSEMBLY_PATTERN_CD = new GridViewDefine( "組立パターン", "assemblyPatternCd", typeof( string ), "", true, HorizontalAlign.Center, 0, false, true );
            /// <summary>組立パターン名</summary>
            public static readonly GridViewDefine ASSEMBLY_PATTERN_NM = new GridViewDefine( "製品種別", "assemblyPatternNm", typeof( string ), "", true, HorizontalAlign.Left, 140, true, true );
            /// <summary>生産型式コード(非表示)</summary>
            public static readonly GridViewDefine PRODUCT_MODEL_CD = new GridViewDefine( "生産型式", "productModelCd", typeof( string ), "", true, HorizontalAlign.Center, 0, false, true );
            /// <summary>型式コード(非表示)</summary>
            public static readonly GridViewDefine MODEL_CD = new GridViewDefine( "生産型式", "modelCd", typeof( string ), "", false, HorizontalAlign.Center, 0, false, true );
            /// <summary>生産型式コード表記</summary>
            public static readonly GridViewDefine PRODUCT_MODEL_CD_STR = new GridViewDefine( "生産型式", "productModelCdStr", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>生産国コード(非表示)</summary>
            public static readonly GridViewDefine PRODUCT_COUNTRY_CD = new GridViewDefine( "生産国", "productCountryCd", typeof( string ), "", true, HorizontalAlign.Center, 0, false, true );
            /// <summary>国コード</summary>
            public static readonly GridViewDefine COUNTRY_CD = new GridViewDefine( "生産国", "countryCd", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
            /// <summary>生産型式名</summary>
            public static readonly GridViewDefine PRODUCT_MODEL_NM = new GridViewDefine( "生産型式名", "productModelNm", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <summary>製品機番</summary>
            public static readonly GridViewDefine SERIAL = new GridViewDefine( "製品機番", "serial", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>PINコード</summary>
            public static readonly GridViewDefine PIN_CD = new GridViewDefine( "PINコード", "pinCd", typeof( string ), "", true, HorizontalAlign.Center, 130, true, true );
            /// <summary>IDNO</summary>
            public static readonly GridViewDefine IDNO = new GridViewDefine( "IDNO", "idno", typeof( string ), "", true, HorizontalAlign.Center, 70, true, true );
            /// <summary>完成予定日</summary>
            public static readonly GridViewDefine PLAN_DT = new GridViewDefine( "完成予定", "planDt", typeof( DateTime ), "{0:yyyy/MM/dd}", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>完成日</summary>
            public static readonly GridViewDefine PRODUCT_DT = new GridViewDefine( "完成日", "productDt", typeof( DateTime ), "{0:yyyy/MM/dd}", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>改造内容</summary>
            public static readonly GridViewDefine PRE_ALTERATION_DETAIL = new GridViewDefine( "改造内容", "preAlterationDetail", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>改造前型式</summary>
            public static readonly GridViewDefine PRE_ALTERATION_MODEL_CD = new GridViewDefine( "改造前型式", "preAlterationModelCd", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>改造前国</summary>
            public static readonly GridViewDefine PRE_ALTERATION_COUNTRY_CD = new GridViewDefine( "改造前国", "preAlterationCountryCd", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
            /// <summary>出荷日</summary>
            public static readonly GridViewDefine SHIPPED_DT = new GridViewDefine( "出荷日", "shippedDt", typeof( DateTime ), "{0:yyyy/MM/dd}", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>出荷伝票NO</summary>
            public static readonly GridViewDefine SHIPPING_VOUCHER_NUM = new GridViewDefine( "出荷伝票NO", "shippingVoucherNum", typeof( string ), "", true, HorizontalAlign.Center, 120, true, true );
            /// <summary>得意先名</summary>
            public static readonly GridViewDefine CUSTOMER_NM = new GridViewDefine( "得意先名", "customerNm", typeof( string ), "", true, HorizontalAlign.Left, 530, true, true );
            /// <summary>納所名</summary>
            public static readonly GridViewDefine DELIVERY_DESTINATION_NM = new GridViewDefine( "納所名", "deliveryDestinationNm", typeof( string ), "", true, HorizontalAlign.Left, 530, true, true );

        }
        /// <summary>
        /// 製品:トラクタ検索一覧定義
        /// </summary>
        internal class GRID_TRACTOR {
            /// <summary>販売型式コード</summary>
            public static readonly GridViewDefine SALES_MODEL_CD = new GridViewDefine( "販売型式", "salesModelCd", typeof( string ), "", true, HorizontalAlign.Center, 0, false, true );
            /// <summary>販売型式コード表記</summary>
            public static readonly GridViewDefine SALES_MODEL_CD_STR = new GridViewDefine( "販売型式", "salesModelCdStr", typeof( string ), "", true, HorizontalAlign.Center, 120, true, true );
            /// <summary>販売型式名</summary>
            public static readonly GridViewDefine SALES_MODEL_NM = new GridViewDefine( "販売型式名", "salesModelNm", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <summary>エンジン型式コード</summary>
            public static readonly GridViewDefine ENGINE_MODEL_CD = new GridViewDefine( "エンジン型式", "engineModelCd", typeof( string ), "", true, HorizontalAlign.Center, 0, false, true );
            /// <summary>エンジン型式コード表記</summary>
            public static readonly GridViewDefine ENGINE_MODEL_CD_STR = new GridViewDefine( "エンジン型式", "engineModelCdStr", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <summary>エンジン型式名</summary>
            public static readonly GridViewDefine ENGINE_MODEL_NM = new GridViewDefine( "エンジン型式名", "engineModelNm", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <summary>エンジン機番</summary>
            public static readonly GridViewDefine ENGINE_SERIAL = new GridViewDefine( "エンジン機番", "engineSerial", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <summary>投入順序連番</summary>
            public static readonly GridViewDefine PRODUCT_ORDER_NUM1 = new GridViewDefine( "投入順序連番", "productOrderNum1", typeof( string ), "", true, HorizontalAlign.Center, 130, true, true );
            /// <summary>確定順序連番</summary>
            public static readonly GridViewDefine PRODUCT_ORDER_NUM2 = new GridViewDefine( "確定順序連番", "productOrderNum2", typeof( string ), "", true, HorizontalAlign.Center, 130, true, true );
        }
        /// <summary>
        /// 製品:エンジン検索一覧定義
        /// </summary>
        internal class GRID_ENGINE {
            /// <summary>本機型式コード</summary>
            public static readonly GridViewDefine TRACTOR_MODEL_CD = new GridViewDefine( "本機型式", "tractorModelCd", typeof( string ), "", true, HorizontalAlign.Center, 0, false, true );
            /// <summary>本機型式コード表記</summary>
            public static readonly GridViewDefine TRACTOR_MODEL_CD_STR = new GridViewDefine( "本機型式", "tractorModelCdStr", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>本機型式名</summary>
            public static readonly GridViewDefine TRACTOR_MODEL_NM = new GridViewDefine( "本機型式名", "tractorModelNm", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <summary>本機機番</summary>
            public static readonly GridViewDefine TRACTOR_SERIAL = new GridViewDefine( "本機機番", "tractorSerial", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>投入順序連番</summary>
            public static readonly GridViewDefine PRODUCT_ORDER_NUM1 = new GridViewDefine( "投入順序連番", "productOrderNum1", typeof( string ), "", true, HorizontalAlign.Center, 130, true, true );
        }
        /// <summary>
        /// 製品:ロータリー検索一覧定義
        /// </summary>
        internal class GRID_ROTARY {
            /// <summary>販売型式コード</summary>
            public static readonly GridViewDefine SALES_MODEL_CD = new GridViewDefine( "販売型式", "salesModelCd", typeof( string ), "", true, HorizontalAlign.Center, 0, false, true );
            /// <summary>販売型式コード表記</summary>
            public static readonly GridViewDefine SALES_MODEL_CD_STR = new GridViewDefine( "販売型式", "salesModelCdStr", typeof( string ), "", true, HorizontalAlign.Center, 120, true, true );
            /// <summary>販売型式名</summary>
            public static readonly GridViewDefine SALES_MODEL_NM = new GridViewDefine( "販売型式名", "salesModelNm", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <summary>本機型式コード</summary>
            public static readonly GridViewDefine TRACTOR_MODEL_CD = new GridViewDefine( "本機型式", "tractorModelCd", typeof( string ), "", true, HorizontalAlign.Center, 0, false, true );
            /// <summary>本機型式コード表記</summary>
            public static readonly GridViewDefine TRACTOR_MODEL_CD_STR = new GridViewDefine( "本機型式", "tractorModelCdStr", typeof( string ), "", true, HorizontalAlign.Center, 120, true, true );
            /// <summary>本機型式名</summary>
            public static readonly GridViewDefine TRACTOR_MODEL_NM = new GridViewDefine( "本機型式名", "tractorModelNm", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <summary>本機機番</summary>
            public static readonly GridViewDefine TRACTOR_SERIAL = new GridViewDefine( "本機機番", "tractorSerial", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
        }
        #endregion

        #region 部品
        /// <summary>
        /// 部品:共通検索一覧定義
        /// </summary>
        internal class GRID_PARTS_COMMON {
            /// <summary>組付日</summary>
            public static readonly GridViewDefine INSTALL_DT = new GridViewDefine( "組付日", "installDt", typeof( DateTime ), "{0:yyyy/MM/dd}", true, HorizontalAlign.Center, 100, true, true );
        }

        /// <summary>
        /// 部品:[共通]基幹部品検索一覧定義
        /// </summary>
        internal class GRID_PARTS_COREPARTS {
            /// <summary>ステーション名</summary>
            public static readonly GridViewDefine STATION_NM = new GridViewDefine( "ステーション", "stationNm", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <summary>部品機番</summary>
            public static readonly GridViewDefine SERIAL = new GridViewDefine( "部品機番", "partsSerial", typeof( string ), "", true, HorizontalAlign.Left, 240, true, true );
            /// <summary>品番</summary>
            public static readonly GridViewDefine PARTS_NUM = new GridViewDefine( "品番", "partsNum", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <summary>来歴No</summary>
            public static readonly GridViewDefine HISTORY_NO = new GridViewDefine( "来歴No", "historyIndex", typeof( string ), "", true, HorizontalAlign.Right, 80, true, true );
        }

        #region トラクタ部品
        /// <summary>
        /// 部品:[トラクタ]クレート検索一覧定義
        /// </summary>
        internal class GRID_PARTS_TRACTOR_CRATE {
            /// <summary>クレート機番</summary>
            public static readonly GridViewDefine SERIAL = new GridViewDefine( "クレート機番", "crateSerial", typeof( string ), "", true, HorizontalAlign.Left, 140, true, true );
        }
        /// <summary>
        /// 部品:[トラクタ]ロプス検索一覧定義
        /// </summary>
        internal class GRID_PARTS_TRACTOR_ROPS {
            /// <summary>ロプス品番</summary>
            public static readonly GridViewDefine PARTS_NUM = new GridViewDefine( "ロプス品番", "ropsModelCd", typeof( string ), "", true, HorizontalAlign.Left, 120, true, true );
            /// <summary>ロプス機番</summary>
            public static readonly GridViewDefine SERIAL = new GridViewDefine( "ロプス機番", "ropsSerial", typeof( string ), "", true, HorizontalAlign.Left, 120, true, true );
        }
        /// <summary>
        /// 部品:[トラクタ]WiFiECU検索一覧定義
        /// </summary>
        internal class GRID_PARTS_TRACTOR_WECU {
            /// <summary>WiFiECU品番</summary>
            public static readonly GridViewDefine HARD_NUM = new GridViewDefine( "WiFiECU品番", "hardNum", typeof( string ), "", true, HorizontalAlign.Left, 140, true, true );
            /// <summary>WiFi ECU機番</summary>
            public static readonly GridViewDefine HARD_SERIAL = new GridViewDefine( "WiFi ECU機番", "hardSerial", typeof( string ), "", true, HorizontalAlign.Left, 140, true, true );
        }
        /// <summary>
        /// 部品:[トラクタ]銘板ラベル検索一覧定義
        /// </summary>
        internal class GRID_PARTS_TRACTOR_NAMEPLATE {
            /// <summary>発行年月日</summary>
            public static readonly GridViewDefine PRINT_DT = new GridViewDefine( "発行年月日", "installDt", typeof( DateTime ), "{0:yyyy/MM/dd}", true, HorizontalAlign.Center, 120, true, true );
            /// <summary>ラベル種別</summary>
            public static readonly GridViewDefine PLATE_TYPE = new GridViewDefine( "ラベル種別", "plateType", typeof( string ), "", true, HorizontalAlign.Left, 120, true, true );
            /// <summary>本機銘板名</summary>
            public static readonly GridViewDefine NAME_PLATE = new GridViewDefine( "銘板名", "namePlateNm", typeof( string ), "", true, HorizontalAlign.Left, 200, true, true );
        }
        /// <summary>
        /// 部品:[トラクタ]ミッション検索一覧定義
        /// </summary>
        internal class GRID_PARTS_TRACTOR_MISSION {
            /// <summary>加工日</summary>
            public static readonly GridViewDefine PROCESSING_YMD = new GridViewDefine( "加工日", "processingYmd", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>加工連番</summary>
            public static readonly GridViewDefine PROCESSING_SEQ = new GridViewDefine( "加工連番", "processingSeq", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
        }
        /// <summary>
        /// 部品:[トラクタ]ハウジング検索一覧定義
        /// </summary>
        internal class GRID_PARTS_TRACTOR_HOUSING {
            /// <summary>加工日</summary>
            public static readonly GridViewDefine PROCESSING_YMD = new GridViewDefine( "加工日", "processingYmd", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>加工連番</summary>
            public static readonly GridViewDefine PROCESSING_SEQ = new GridViewDefine( "加工連番", "processingSeq", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
        }
        #endregion

        #region エンジン部品
        /// <summary>
        /// 部品:[エンジン](全般)検索一覧定義
        /// </summary>
        internal class GRID_ENGINE_PARTS_COM {
            /// <summary>ステーション名</summary>
            public static readonly GridViewDefine STATION_NM = new GridViewDefine( "ステーション", "stationNm", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <summary>部品機番</summary>
            public static readonly GridViewDefine SERIAL = new GridViewDefine( "部品機番", "partsSerial", typeof( string ), "", true, HorizontalAlign.Left, 240, true, true );
        }
        /// <summary>
        /// 部品:[エンジン](全般)検索一覧定義
        /// </summary>
        internal class GRID_ENGINE_PARTS_COM_DETAIL {
            /// <summary>クボタ品番</summary>
            public static readonly GridViewDefine K_NUM = new GridViewDefine( "クボタ品番", "partsKubotaNum", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <summary>メーカー品番</summary>
            public static readonly GridViewDefine M_NUM = new GridViewDefine( "メーカー品番", "partsMakerNum", typeof( string ), "", true, HorizontalAlign.Left, 140, true, true );
            /// <summary>来歴No</summary>
            public static readonly GridViewDefine HISTORY_NO = new GridViewDefine( "来歴No", "historyIndex", typeof( string ), "", true, HorizontalAlign.Right, 80, true, true );
        }


        /// <summary>
        /// 部品:[エンジン](クボタ品番)検索一覧定義
        /// </summary>
        internal class GRID_ENGINE_PARTS_TAIL {
            /// <summary>ATU型式</summary>
            public static readonly GridViewDefine K_NUM = new GridViewDefine( "ATU型式", "partsKubotaNum", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <summary>来歴No</summary>
            public static readonly GridViewDefine HISTORY_NO = new GridViewDefine( "来歴No", "historyIndex", typeof( string ), "", true, HorizontalAlign.Right, 80, true, true );
        }
        /// <summary>
        /// 部品:[エンジン](3C)検索一覧定義
        /// </summary>
        internal class GRID_ENGINE_PARTS_3C {
            /// <summary>ステーション名</summary>
            public static readonly GridViewDefine STATION_NM = new GridViewDefine( "ステーション", "stationNm", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <summary>部品機番</summary>
            public static readonly GridViewDefine SERIAL = new GridViewDefine( "部品機番", "partsSerial", typeof( string ), "", true, HorizontalAlign.Left, 240, true, true );
            /// <summary>クボタ品番</summary>
            public static readonly GridViewDefine K_NUM = new GridViewDefine( "クボタ品番", "partsKubotaNum", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <summary>メーカー品番</summary>
            public static readonly GridViewDefine M_NUM = new GridViewDefine( "メーカー品番", "partsMakerNum", typeof( string ), "", true, HorizontalAlign.Left, 140, true, true );
            /// <summary>来歴No</summary>
            public static readonly GridViewDefine HISTORY_NO = new GridViewDefine( "来歴No", "historyIndex", typeof( int ), "", true, HorizontalAlign.Right, 80, true, true );
            /// <summary>加工日(日付型ではない)</summary>
            public static readonly GridViewDefine PROCESS_DT = new GridViewDefine( "加工日", "processYmd", typeof( string ), "", true, HorizontalAlign.Left, 80, true, true );
            /// <summary>連番</summary>
            public static readonly GridViewDefine PROCESS_NUM = new GridViewDefine( "連番", "processNum", typeof( string ), "{0:yyyy/MM/dd}", true, HorizontalAlign.Left, 80, true, true );
            /// <summary>修正者</summary>
            public static readonly GridViewDefine UPDATE_BY = new GridViewDefine( "修正者", "UPDATE_BY", typeof( string ), "", true, HorizontalAlign.Left, 120, true, true );
        }
        /// <summary>
        /// 部品:[エンジン]機番ラベル印刷検索一覧定義
        /// </summary>
        internal class GRID_ENGINE_SERIALPRINT {
            /// <summary>ステーションコード</summary>
            public static readonly GridViewDefine STATION_NM = new GridViewDefine( "ステーション", "stationNm", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <summary>印字エンジン型式</summary>
            public static readonly GridViewDefine ENGINE_MODEL_CD = new GridViewDefine( "印字型式", "printCont3", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <summary>印字エンジン機番</summary>
            public static readonly GridViewDefine ENGINE_SERIAL = new GridViewDefine( "印字機番", "printCont2", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <summary>刻印文字</summary>
            public static readonly GridViewDefine PRINT_CONT = new GridViewDefine( "刻印文字", "printCont1", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <summary>QRコード</summary>
            public static readonly GridViewDefine QR_CD = new GridViewDefine( "QRコード", "printCont5", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <summary>自動フラグ</summary>
            public static readonly GridViewDefine AUTO_FLAG = new GridViewDefine( "自動フラグ", "autoFlag", typeof( string ), "", true, HorizontalAlign.Center, 120, true, true );
            /// <summary>印刷日時</summary>
            public static readonly GridViewDefine PRINT_DT = new GridViewDefine( "印刷日時", "printDt", typeof( DateTime ), "", true, HorizontalAlign.Center, 160, true, true );
        }
        #endregion

        #endregion

        #region 工程
        /// <summary>
        /// 汎用テーブル検索一覧定義
        /// </summary>
        internal class GRID_PROCESS_GENERIC_TABLE {
            /// <summary>測定日時</summary>
            public static readonly GridViewDefine RESULT_DT = new GridViewDefine( "測定日時", "resultDt", typeof( DateTime ), "{0:yyyy/MM/dd}", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>結果</summary>
            public static readonly GridViewDefine RESULT_NM = new GridViewDefine( "結果", "resultNm", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>測定値1</summary>
            public static readonly GridViewDefine RESULT_VAL_1 = new GridViewDefine( "測定値1", "resultVal1", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>測定値2</summary>
            public static readonly GridViewDefine RESULT_VAL_2 = new GridViewDefine( "測定値2", "resultVal2", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>測定値3</summary>
            public static readonly GridViewDefine RESULT_VAL_3 = new GridViewDefine( "測定値3", "resultVal3", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>測定値4</summary>
            public static readonly GridViewDefine RESULT_VAL_4 = new GridViewDefine( "測定値4", "resultVal4", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>測定値5</summary>
            public static readonly GridViewDefine RESULT_VAL_5 = new GridViewDefine( "測定値5", "resultVal5", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
        }

        /// <summary>
        /// 工程:共通検索一覧定義
        /// </summary>
        internal class GRID_PROCESS_COMMON {
            /// <summary>測定日</summary>
            public static readonly GridViewDefine INSPECTION_DT = new GridViewDefine( "測定日", "inspectionDt", typeof( DateTime ), "{0:yyyy/MM/dd}", true, HorizontalAlign.Center, 100, true, true );
        }

        /// <summary>
        /// 工程:[共通]AI画像解析
        /// </summary>
        internal class GRID_PROCESS_AIIMAGE {
            /// <summary>ステーション名</summary>
            public static readonly GridViewDefine STATION_NM = new GridViewDefine( "ステーション", "stationNm", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <summary>合否</summary>
            public static readonly GridViewDefine RESULT = new GridViewDefine( "合否", "result", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
            /// <summary>画像URL</summary>
            public static readonly GridViewDefine IMAGE_URL = new GridViewDefine( "画像URL", "gDrivePath", typeof( string ), "", true, HorizontalAlign.Center, 100, true, false );
        }

        #region トラクタ工程
        /// <summary>
        /// 工程:[トラクタ](パワクロ走行検査)検索一覧定義
        /// </summary>
        internal class GRID_PROCESS_TRACTOR_PCRAWLER {
            /// <summary>結果</summary>
            public static readonly GridViewDefine RESULT = new GridViewDefine( "結果", "result", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
            /// <summary>来歴No</summary>
            public static readonly GridViewDefine HISTORY_NO = new GridViewDefine( "来歴No", "historyIndex", typeof( int ), "", true, HorizontalAlign.Right, 80, true, true );
        }

        /// <summary>
        /// 工程:[トラクタ](チェックシートイメージ)検索一覧定義
        /// </summary>
        internal class GRID_PROCESS_TRACTOR_CHECKSHEET {
            /// <summary>検査日</summary>
            public static readonly GridViewDefine INSPECTION_DT = new GridViewDefine( "検査日", "inspectionDt", typeof( DateTime ), "{0:yyyy/MM/dd}", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>シート数</summary>
            public static readonly GridViewDefine SHEET_QTY = new GridViewDefine( "シート数", "checkSheetCount", typeof( int ), "", true, HorizontalAlign.Right, 100, true, true );
        }

        /// <summary>
        /// 工程:[トラクタ](品質画像証跡)検索一覧定義
        /// </summary>
        internal class GRID_PROCESS_TRACTOR_CAMIMAGE {
            /// <summary>シート数</summary>
            public static readonly GridViewDefine IMAGE_QTY = new GridViewDefine( "画像数", "imageCount", typeof( int ), "", true, HorizontalAlign.Right, 80, true, true );
        }

        /// <summary>
        /// 工程:[トラクタ](電子チェックシート)検索一覧定義
        /// </summary>
        internal class GRID_PROCESS_TRACTOR_ELCHECK {
            /// <summary>検査日</summary>
            public static readonly GridViewDefine CHK_DT = new GridViewDefine( "検査開始日時", "inspectionDt", typeof( string ), "", true, HorizontalAlign.Center, 160, true, true );
            /// <summary>ライン名</summary>
            public static readonly GridViewDefine LINE_NM = new GridViewDefine( "ライン名", "lineNm", typeof( string ), "", true, HorizontalAlign.Left, 140, true, true );
            /// <summary>最終検査工程</summary>
            public static readonly GridViewDefine LAST_PROC = new GridViewDefine( "最終検査工程", "lastProc", typeof( string ), "", true, HorizontalAlign.Left, 140, true, true );
            /// <summary>結果</summary>
            public static readonly GridViewDefine RESULT = new GridViewDefine( "結果", "result", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
            /// <summary>合格判定社員名</summary>
            public static readonly GridViewDefine JUDGE_EMP_CD = new GridViewDefine( "合格判定社員名", "judgeEmpCd", typeof( string ), "", true, HorizontalAlign.Left, 200, true, true );
        }

        /// <summary>
        /// 工程:[トラクタ](イメージチェックシート)検索一覧定義
        /// </summary>
        internal class GRID_PROCESS_TRACTOR_IMGCHECK {
            /// <summary>検査日</summary>
            public static readonly GridViewDefine CHK_DT = new GridViewDefine( "検査開始日時", "inspectionDt", typeof( string ), "", true, HorizontalAlign.Center, 160, true, true );
            /// <summary>検査開始社員名</summary>
            public static readonly GridViewDefine START_BY = new GridViewDefine( "検査開始社員名", "startBy", typeof( string ), "", true, HorizontalAlign.Left, 200, true, true );
            /// <summary>最終更新社員名</summary>
            public static readonly GridViewDefine UPDATE_BY = new GridViewDefine( "更新社員名", "updateBy", typeof( string ), "", true, HorizontalAlign.Left, 200, true, true );
            /// <summary>ライン名</summary>
            public static readonly GridViewDefine LINE_NM = new GridViewDefine( "ライン名", "lineNm", typeof( string ), "", true, HorizontalAlign.Left, 140, true, true );
            /// <summary>最終検査工程</summary>
            public static readonly GridViewDefine LAST_PROC = new GridViewDefine( "最終検査工程", "lastProc", typeof( string ), "", true, HorizontalAlign.Left, 140, true, true );
            /// <summary>結果</summary>
            public static readonly GridViewDefine RESULT = new GridViewDefine( "結果", "result", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
            /// <summary>合格判定社員名</summary>
            public static readonly GridViewDefine JUDGE_EMP_CD = new GridViewDefine( "合格判定社員名", "judgeEmpCd", typeof( string ), "", true, HorizontalAlign.Left, 200, true, true );
        }

        /// <summary>
        /// 工程:[トラクタ](刻印)検索一覧定義
        /// </summary>
        internal class GRID_PROCESS_PRINT_SHEEL {
            /// <summary>刻印日(前車軸)</summary>
            public static readonly GridViewDefine PRINT_DT = new GridViewDefine( "刻印日(前車軸)", "inspectionDt", typeof( string ), "", true, HorizontalAlign.Center, 160, true, true );
            /// <summary>刻印文字(前車軸)</summary>
            public static readonly GridViewDefine CONTENTS_1 = new GridViewDefine( "刻印文字(前車軸)", "type02_contents_1", typeof( string ), "", true, HorizontalAlign.Center, 180, true, true );
            /// <summary>刻印方法</summary>
            public static readonly GridViewDefine PIN_KBN = new GridViewDefine( "PIN区分", "pin_kbn", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>刻印日(MS)</summary>
            public static readonly GridViewDefine TYPE03_PRINT_DT = new GridViewDefine( "刻印日(MS)", "type03_printDt", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <summary>刻印文字(MS)</summary>
            public static readonly GridViewDefine TYPE03_CONTENTS_1 = new GridViewDefine( "刻印文字(MS)", "type03_contents_1", typeof( string ), "", true, HorizontalAlign.Center, 180, true, true );
            /// <summary>刻印日(MID)</summary>
            public static readonly GridViewDefine TYPE04_PRINT_DT = new GridViewDefine( "刻印日(MID)", "type04_printDt", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <summary>刻印文字(MID)</summary>
            public static readonly GridViewDefine TYPE04_CONTENTS_1 = new GridViewDefine( "刻印文字(MID)", "type04_contents_1", typeof( string ), "", true, HorizontalAlign.Center, 180, true, true );

        }

        /// <summary>
        /// 工程:[トラクタ](ナットランナー)検索一覧定義
        /// </summary>
        internal class GRID_PROCESS_TRACTOR_NUTRUNNER {
            /// <summary>測定日時</summary>
            public static readonly GridViewDefine CHK_DT = new GridViewDefine( "測定日時", "inspectionDt", typeof( DateTime ), "{0:yyyy/MM/dd HH:mm:ss}", true, HorizontalAlign.Center, 160, true, true );
            /// <summary>結果</summary>
            public static readonly GridViewDefine RESULT = new GridViewDefine( "結果", "check_result", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
        }

        /// <summary>
        /// 工程:[トラクタ](トラクタ走行検査)検索一覧定義
        /// </summary>
        internal class GRID_PROCESS_TRACTOR_ALL {
            /// <summary>結果</summary>
            public static readonly GridViewDefine RESULT = new GridViewDefine( "結果", "result", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
        }

        /// <summary>
        /// 工程:[トラクタ](光軸検査)検索一覧定義
        /// </summary>
        internal class GRID_PROCESS_OPTAXIS {
            /// <summary>結果</summary>
            public static readonly GridViewDefine RESULT = new GridViewDefine( "結果", "result", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
        }

        /// <summary>
        /// 工程:[トラクタ](関所)検索一覧定義
        /// </summary>
        internal class GRID_PROCESS_CHKPOINT {
            /// <summary>ワーク工程作業開始時刻</summary>
            public static readonly GridViewDefine WORK_START_DT = new GridViewDefine( "作業開始時刻", "inspectionDt", typeof( DateTime ), "{0:yyyy/MM/dd HH:mm:ss}", true, HorizontalAlign.Center, 160, true, true );
            /// <summary>ワーク工程作業終了時刻</summary>
            public static readonly GridViewDefine WORK_END_DT = new GridViewDefine( "作業終了時刻", "workEndDt", typeof( DateTime ), "{0:yyyy/MM/dd HH:mm:ss}", true, HorizontalAlign.Center, 160, true, true );
        }

        /// <summary>
        /// 工程:[トラクタ](検査ベンチ)検索一覧定義
        /// </summary>
        internal class GRID_PROCESS_TRACTOR_TESTBENCH {
            /// <summary>測定日時</summary>
            public static readonly GridViewDefine CHK_DT = new GridViewDefine( "測定日時", "inspectionDt", typeof( DateTime ), "{0:yyyy/MM/dd HH:mm:ss}", true, HorizontalAlign.Center, 160, true, true );
            /// <summary>結果</summary>
            public static readonly GridViewDefine RESULT = new GridViewDefine( "結果", "result", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
            /// <summary>来歴No</summary>
            public static readonly GridViewDefine HISTORY_NO = new GridViewDefine( "来歴No", "historyNo", typeof( int ), "", true, HorizontalAlign.Right, 80, true, true );
        }
        #endregion

        #region エンジン工程
        /// <summary>
        /// 工程:[エンジン](トルク締付)検索一覧定義
        /// </summary>
        internal class GRID_PROCESS_ENGINE_TORQUE {
            /// <summary>結果</summary>
            public static readonly GridViewDefine RESULT = new GridViewDefine( "結果", "result", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
            /// <summary>部品名</summary>
            public static readonly GridViewDefine PARTS_NM = new GridViewDefine( "部品名", "partsNm", typeof( string ), "", true, HorizontalAlign.Left, 100, true, true );
            /// <summary>端末名</summary>
            public static readonly GridViewDefine TERMINAL_NM = new GridViewDefine( "端末名", "terminalNm", typeof( string ), "", true, HorizontalAlign.Left, 100, true, true );
            /// <summary>来歴No</summary>
            public static readonly GridViewDefine HISTORY_NO = new GridViewDefine( "来歴No", "historyIndex", typeof( int ), "", true, HorizontalAlign.Right, 80, true, true );
        }

        /// <summary>
        /// 工程:[エンジン](ハーネス検査)検索一覧定義
        /// </summary>
        internal class GRID_PROCESS_ENGINE_HARNESS {
            /// <summary>結果</summary>
            public static readonly GridViewDefine RESULT = new GridViewDefine( "結果", "result", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
        }

        /// <summary>
        /// 工程:[エンジン](運転検査)検索一覧定義
        /// </summary>
        internal class GRID_PROCESS_ENGINE_TEST {
            /// <summary>結果</summary>
            public static readonly GridViewDefine RESULT = new GridViewDefine( "結果", "result", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
            /// <summary>来歴No</summary>
            public static readonly GridViewDefine HISTORY_NO = new GridViewDefine( "来歴No", "historyIndex", typeof( int ), "", true, HorizontalAlign.Right, 80, true, true );
        }

        /// <summary>
        /// 工程:[エンジン](フリクションロス)検索一覧定義
        /// </summary>
        internal class GRID_PROCESS_ENGINE_FRICTION {
            /// <summary>結果</summary>
            public static readonly GridViewDefine RESULT = new GridViewDefine( "結果", "result", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
            /// <summary>回転数</summary>
            public static readonly GridViewDefine RPM = new GridViewDefine( "回転数", "rpm", typeof( string ), "", true, HorizontalAlign.Right, 80, true, true );
            /// <summary>来歴No</summary>
            public static readonly GridViewDefine HISTORY_NO = new GridViewDefine( "来歴No", "historyIndex", typeof( int ), "", true, HorizontalAlign.Right, 80, true, true );
        }

        /// <summary>
        /// 工程:[エンジン](シリンダヘッド部品組付履歴)検索一覧定義
        /// </summary>
        internal class GRID_PROCESS_ENGINE_CYH_ASSEMBLY {
            /// <summary>結果</summary>
            public static readonly GridViewDefine RESULT = new GridViewDefine( "結果", "result", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
            /// <summary>来歴No</summary>
            public static readonly GridViewDefine HISTORY_NO = new GridViewDefine( "来歴No", "historyIndex", typeof( int ), "", true, HorizontalAlign.Right, 80, true, true );
        }

        /// <summary>
        /// 工程:[エンジン](3C精密測定)検索一覧定義
        /// </summary>
        internal class GRID_PROCESS_ENGINE_3C_INSPECT {
            /// <summary>結果</summary>
            public static readonly GridViewDefine RESULT = new GridViewDefine( "結果", "result", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
        }

        /// <summary>
        /// 工程:[エンジン](燃料噴射時期)検索一覧定義
        /// </summary>
        internal class GRID_PROCESS_ENGINE_INJECTION {
            /// <summary>測定号機</summary>
            public static readonly GridViewDefine MEASURE_TERMINAL = new GridViewDefine( "測定号機", "measureTerminal", typeof( int ), "", true, HorizontalAlign.Right, 80, true, true );
            /// <summary>NG判定</summary>
            public static readonly GridViewDefine RESULT = new GridViewDefine( "判定", "result", typeof( string ), "", true, HorizontalAlign.Right, 80, true, true );
            /// <summary>来歴No</summary>
            public static readonly GridViewDefine HISTORY_NO = new GridViewDefine( "来歴No", "historyIndex", typeof( int ), "", true, HorizontalAlign.Right, 80, true, true );
        }

        /// <summary>
        /// 工程:[エンジン](品質画像証跡)検索一覧定義
        /// </summary>
        internal class GRID_PROCESS_ENGINE_CAMIMAGE {
            /// <summary>シート数</summary>
            public static readonly GridViewDefine IMAGE_QTY = new GridViewDefine( "画像数", "imageCount", typeof( int ), "", true, HorizontalAlign.Right, 80, true, true );
        }

        /// <summary>
        /// 工程:[エンジン](電子チェックシート)検索一覧定義
        /// </summary>
        internal class GRID_PROCESS_ENGINE_ELCHECK {
            /// <summary>検査日</summary>
            public static readonly GridViewDefine CHK_DT = new GridViewDefine( "検査開始日時", "inspectionDt", typeof( string ), "", true, HorizontalAlign.Center, 160, true, true );
            /// <summary>検査開始社員名</summary>
            public static readonly GridViewDefine START_BY = new GridViewDefine( "検査開始社員名", "startBy", typeof( string ), "", true, HorizontalAlign.Left, 200, true, true );
            /// <summary>最終更新社員名</summary>
            public static readonly GridViewDefine UPDATE_BY = new GridViewDefine( "更新社員名", "updateBy", typeof( string ), "", true, HorizontalAlign.Left, 200, true, true );
            /// <summary>ライン名</summary>
            public static readonly GridViewDefine LINE_NM = new GridViewDefine( "ライン名", "lineNm", typeof( string ), "", true, HorizontalAlign.Left, 140, true, true );
            /// <summary>最終検査工程</summary>
            public static readonly GridViewDefine LAST_PROC = new GridViewDefine( "最終検査工程", "lastProc", typeof( string ), "", true, HorizontalAlign.Left, 140, true, true );
            /// <summary>結果</summary>
            public static readonly GridViewDefine RESULT = new GridViewDefine( "結果", "result", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
            /// <summary>合格判定社員名</summary>
            public static readonly GridViewDefine JUDGE_EMP_CD = new GridViewDefine( "合格判定社員名", "judgeEmpCd", typeof( string ), "", true, HorizontalAlign.Left, 200, true, true );
        }
        #endregion

        #endregion

        #endregion

        #region プロパティ

        /// <summary>
        /// 検索条件定義情報
        /// </summary>
        ControlDefine[] _conditionControls = null;
        /// <summary>
        /// 検索条件定義情報アクセサ
        /// </summary>
        ControlDefine[] ConditionControls
        {
            get
            {
                if ( true == ObjectUtils.IsNull( _conditionControls ) ) {
                    _conditionControls = ControlUtils.GetControlDefineArray( typeof( CONDITION ) );
                }
                return _conditionControls;
            }
        }

        /// <summary>
        /// 一覧定義情報
        /// </summary>
        GridViewDefine[] _gridviewDefault = null;
        /// <summary>
        /// 一覧定義情報アクセサ
        /// </summary>
        GridViewDefine[] gridviewDefault
        {
            get
            {
                if ( true == ObjectUtils.IsNull( _gridviewDefault ) ) {
                    _gridviewDefault = ControlUtils.GetGridViewDefineArray( typeof( GRID_PRODUCT_COMMON ) );
                }
                return _gridviewDefault;
            }
        }

        #endregion

        #region イベント

        /// <summary>
        /// ページロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load( object sender, EventArgs e ) {
            base.RaiseEvent( DoPageLoad, false );
        }

        /// <summary>
        /// 検索区分切替動作(部品検索)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChangePartsSearch_Click( object sender, EventArgs e ) {
            base.RaiseEvent( MoveToPartsSearch );
        }

        /// <summary>
        /// 検索区分切替動作(工程検索)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChangeProcessSearch_Click( object sender, EventArgs e ) {
            base.RaiseEvent( MoveToProcessSearch );
        }

        /// <summary>
        /// 製品種別リスト選択動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblProductKind_SelectedIndexChanged( object sender, EventArgs e ) {
            base.RaiseEvent( ChangeProductKind );
        }

        /// <summary>
        /// 型式区分リスト選択動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblModelType_SelectedIndexChanged( object sender, EventArgs e ) {
            base.RaiseEvent( ChangeModelType );
        }

        /// <summary>
        /// 工程リスト選択動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProcess_SelectedIndexChanged( object sender, EventArgs e ) {
            base.RaiseEvent( ChangeProcess );
        }

        /// <summary>
        /// 部品リスト選択動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlParts_SelectedIndexChanged( object sender, EventArgs e ) {
            base.RaiseEvent( ChangeParts );
        }

        /// <summary>
        /// 検索[Excel出力]ボタン選択動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMassDataOutput_Click( object sender, EventArgs e ) {
            base.RaiseEvent( DoMassDataOutput );
        }

        /// <summary>
        /// 検索ボタン選択動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click( object sender, EventArgs e ) {
            base.RaiseEvent( DoSearch );
        }

        /// <summary>
        /// グリッドビュー行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMainView_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainView( sender, e );
        }

        /// <summary>
        /// グリッドビューページチェンジイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMainView_PageIndexChanging( object sender, CommandEventArgs e ) {
            base.RaiseEvent( PageIndexChangingMainView, sender, e );
        }

        /// <summary>
        /// グリッドビュー並び替えイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMainView_Sorting( object sender, GridViewSortEventArgs e ) {
            base.RaiseEvent( SortingMainView, sender, e );
        }

        protected void btnExcel_Click( object sender, EventArgs e ) {
            base.RaiseEvent( OutputExcel );
        }

        /// <summary>
        /// 工程変更動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnChangeProcess_Click( object sender, EventArgs e ) {
            base.RaiseEvent( ChangeProcess );
        }
        #endregion

        #region イベントメソッド

        #region ページイベント

        /// <summary>
        /// ページロード処理
        /// </summary>
        protected override void DoPageLoad() {
            //ベース ページロード処理
            base.DoPageLoad();

            //動的処理
            GridView frozenGrid = grvMainViewLB;
            ControlUtils.SetGridViewTemplateField( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, gridviewDefault, true ) );
            ControlUtils.SetGridViewTemplateField( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, gridviewDefault, false ) );

            ControlUtils.ReMakeBoundTemplateFieldCtrl( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, gridviewDefault, true ) );
            ControlUtils.ReMakeBoundTemplateFieldCtrl( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, gridviewDefault, false ) );

            int resultCnt = 0;
            if ( ObjectUtils.IsNotNull( ConditionInfo.ResultData ) ) {
                resultCnt = ConditionInfo.ResultData.Rows.Count;
            }

            ControlUtils.SetGridViewPager( ref pnlPager, grvMainViewRB, grvMainView_PageIndexChanging, resultCnt, grvMainViewRB.PageIndex );
            // ページロード後に実行するjavascriptを設定
            ScriptManager.RegisterClientScriptBlock( this, this.GetType(), "MainView_DoAfterLoad", "$(function(){MainView.DoAfterLoad()});", true );
        }

        #endregion

        #region リスト選択処理
        /// <summary>
        /// 製品種別変更
        /// </summary>
        private void ChangeProductKind() {
            if ( ModelType.Product == rblModelType.SelectedValue ) {
                //生産型式の場合には、工程区分/部品区分のリストを取得

                //工程クリア
                ClearSearchProcess();

                //部品区分リスト
                if ( rblProductKind.SelectedValue.Equals( ProductKind.Tractor ) ) {
                    //トラクタ
                    ControlUtils.SetListControlItems( ddlParts,
                        Dao.Com.MasterList.GetTractorItem( rblProductKind.SelectedValue, Defines.ListDefine.GroupCd.Parts ) );
                } else {
                    //それ以外
                    ControlUtils.SetListControlItems( ddlParts,
                        Dao.Com.MasterList.GetClassItem( rblProductKind.SelectedValue, Defines.ListDefine.GroupCd.Parts ) );
                }

            } else {
                //販売型式の場合には、工程区分/部品区分のリストはクリア

                //工程クリア
                ClearSearchProcess();

                //部品区分リスト
                ControlUtils.SetListControlItems( ddlParts, null );
            }

            //部品検索条件 活性変更
            ChangePartsCondition();

            //日付区分リスト変更
            SetDateKindList();
        }

        /// <summary>
        /// 工程クリア
        /// </summary>
        private void ClearSearchProcess() {
            hdnLineCd.Value = "";
            hdnProcessCd.Value = "";
            hdnWorkCd.Value = "";
            hdnSearchTargetFlg.Value = "";
            hdnProcessNm.Value = "";
            hdnWorkNm.Value = "";
            txtProcessNm.Value = "";
            txtWorkNm.Value = "";
        }

        /// <summary>
        /// 型式区分変更
        /// </summary>
        private void ChangeModelType() {
            //製品種別変更
            ChangeProductKind();
        }

        /// <summary>
        /// 工程変更
        /// </summary>
        private void ChangeProcess() {
            //部品リスト未選択に変更
            ddlParts.SelectedValue = null;
            //部品検索条件 活性変更
            ChangePartsCondition();
            //日付区分リスト変更
            SetDateKindList();
        }

        /// <summary>
        /// 部品変更
        /// </summary>
        private void ChangeParts() {
            //工程リスト未選択に変更
            ClearSearchProcess();
            //部品検索条件 活性変更
            ChangePartsCondition();
            //日付区分リスト変更
            SetDateKindList();
        }

        /// <summary>
        /// 検索区分変更(部品検索)
        /// </summary>
        private void MoveToPartsSearch() {
            PageUtils.RedirectToTRC( this.Page, CurrentPageInfo.pageId, PageInfo.MainPartsView.url, this.Token, null );
        }

        /// <summary>
        /// 検索区分変更(工程検索)
        /// </summary>
        private void MoveToProcessSearch() {
            PageUtils.RedirectToTRC( this.Page, CurrentPageInfo.pageId, PageInfo.MainProcessView.url, this.Token, null );
        }

        #endregion

        #region 検索(Excel出力)処理
        /// <summary>
        /// Excel出力(バッチ)ボタン押下処理
        /// </summary>
        private void DoMassDataOutput() {

            // 処理実行中チェック
            DataTable JobControlInfo = JobControlInfoDao.Select( TrcWebConsts.JOB_PARAM_KEY_JOB_NM_MASS_DATA_OUTPUT );

            if ( 0 != JobControlInfo.Rows.Count ) {
                base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_63010 );
                return;
            }

            // 検索パラメータチェック
            var msg = CheckSearchParam();
            if ( null != msg ) {
                // エラーメッセージ有りの場合
                base.WriteApplicationMessage( msg );
                return;
            }
            // ユーザ実行JOB起動要求
            DownLoadMassDataOutput();
        }

        /// <summary>
        /// 検索パラメータチェック
        /// </summary>
        /// <param name="lineCd">ラインコード</param>
        /// <param name="processCd">工程コード</param>
        /// <param name="productKindCd">製品種別</param>
        /// <returns>対象外の場合メッセージ、対象の場合null</returns>
        private Msg CheckSearchParam() {
            Msg ret = null;

            // 検索パラメータ作成
            Dictionary<string, object> dicCondition = new Dictionary<string, object>();
            base.GetControlValues( ConditionControls, ref dicCondition );

            //検索条件取得
            string lineCd = DataUtils.GetDictionaryStringVal( dicCondition, MainView.CONDITION.LINE_CD.bindField );
            string processCd = DataUtils.GetDictionaryStringVal( dicCondition, MainView.CONDITION.PROCESS_CD.bindField );
            string productKindCd = DataUtils.GetDictionaryStringVal( dicCondition, MainView.CONDITION.PRODUCT_KIND_CD.bindField );
            DateTime dateFrom = DataUtils.GetDictionaryDateVal( dicCondition, MainView.CONDITION.DATE_FROM.bindField );
            DateTime dateTo = DataUtils.GetDictionaryDateVal( dicCondition, MainView.CONDITION.DATE_TO.bindField );

            //条件チェック
            if ( true == ObjectUtils.IsNull( dateFrom ) || DateTime.MinValue == dateFrom ) {
                //開始日未入力
                return new Msg( MsgManager.MESSAGE_WRN_61051, "開始日" );
            }
            if ( true == ObjectUtils.IsNull( dateTo ) || DateTime.MinValue == dateTo ) {
                //終了日未入力
                return new Msg( MsgManager.MESSAGE_WRN_61051, "終了日" );
            }
            if ( dateFrom.AddMonths( WebAppInstance.GetInstance().Config.ApplicationInfo.searchPeriod ) < dateTo ) {
                //検索対象期間が既定範囲を超える
                return new Msg( MsgManager.MESSAGE_WRN_61052, WebAppInstance.GetInstance().Config.ApplicationInfo.searchPeriod );
            }

            if ( true == StringUtils.IsNotEmpty( processCd ) ) {
                // 工程コードがNULL以外
                // 2桁工程コードを取得
                processCd = DataUtils.GetInspectionCd( lineCd: lineCd, processCd: processCd, productKind: productKindCd ) ?? processCd;
            }
            switch ( productKindCd ) {
            case ProductKind.Engine:
                // 製品種別がエンジンの場合
                switch ( processCd ) {
                case ProcessKind.PROCESS_CD_ENGINE_CAMIMAGE:
                    // 工程コードが品質画像証跡の場合、対象外とする
                    ret = new Msg( MsgManager.MESSAGE_WRN_61050 );
                    break;
                }
                break;
            case ProductKind.Tractor:
                switch ( processCd ) {
                // 製品種別がトラクタの場合
                case ProcessKind.PROCESS_CD_TRACTOR_CAMIMAGE:
                case ProcessKind.PROCESS_CD_TRACTOR_CHKSHEET:
                case ProcessKind.PROCESS_CD_TRACTOR_OPTAXIS:
                case ProcessKind.PROCESS_CD_TRACTOR_IMGCHECK:
                    // 工程コードが品質画像証跡、チェックシート、光軸検査、イメージチェックシートの場合、対象外とする
                    ret = new Msg( MsgManager.MESSAGE_WRN_61050 );
                    break;
                }
                break;
            }

            return ret;
        }

        /// <summary>
        /// ユーザ実行JOB起動要求
        /// </summary>
        private void DownLoadMassDataOutput() {
            // 起動パラメータ設定
            List<CoreService.JobParameter> jobParam = new List<CoreService.JobParameter>();
            // 起動パラメータ設定
            Dictionary<string, object> dicCondition = new Dictionary<string, object>();
            base.GetControlValues( ConditionControls, ref dicCondition );
            // 製品種別
            jobParam.Add( new CoreService.JobParameter(
                TrcWebConsts.JOB_PARAM_KEY_PRODUCT_KIND_CD, DataUtils.GetDictionaryStringVal( dicCondition, MainView.CONDITION.PRODUCT_KIND_CD.bindField ) ) );
            // 型式種別
            jobParam.Add( new CoreService.JobParameter(
                TrcWebConsts.JOB_PARAM_KEY_MODEL_TYPE, DataUtils.GetDictionaryStringVal( dicCondition, MainView.CONDITION.MODEL_TYPE.bindField ) ) );
            // 型式コード
            jobParam.Add( new CoreService.JobParameter(
                TrcWebConsts.JOB_PARAM_KEY_MODEL_CD, DataUtils.GetDictionaryStringVal( dicCondition, MainView.CONDITION.MODEL_CD.bindField ) ) );
            // 型式名
            jobParam.Add( new CoreService.JobParameter(
                TrcWebConsts.JOB_PARAM_KEY_MODEL_NM, DataUtils.GetDictionaryStringVal( dicCondition, MainView.CONDITION.MODEL_NM.bindField ) ) );
            // PINコード( チェックボックス )
            jobParam.Add( new CoreService.JobParameter(
                TrcWebConsts.JOB_PARAM_KEY_PIN_CD_CHECK, DataUtils.GetDictionaryStringVal( dicCondition, MainView.CONDITION.PIN_CD_CHECK.bindField ) ) );
            // PINコード
            jobParam.Add( new CoreService.JobParameter(
                TrcWebConsts.JOB_PARAM_KEY_PIN_CD, DataUtils.GetDictionaryStringVal( dicCondition, MainView.CONDITION.PIN_CD.bindField ) ) );
            // 製品機番
            jobParam.Add( new CoreService.JobParameter(
                TrcWebConsts.JOB_PARAM_KEY_SERIAL, DataUtils.GetDictionaryStringVal( dicCondition, MainView.CONDITION.SERIAL.bindField ) ) );
            // IDNO
            jobParam.Add( new CoreService.JobParameter(
                TrcWebConsts.JOB_PARAM_KEY_IDNO, DataUtils.GetDictionaryStringVal( dicCondition, MainView.CONDITION.IDNO.bindField ) ) );
            // 工程コード
            jobParam.Add( new CoreService.JobParameter(
                TrcWebConsts.JOB_PARAM_KEY_PROCESS_CD, DataUtils.GetDictionaryStringVal( dicCondition, MainView.CONDITION.PROCESS_CD.bindField ) ) );
            // 工程名
            jobParam.Add( new CoreService.JobParameter(
                TrcWebConsts.JOB_PARAM_KEY_PROCESS_NM, DataUtils.GetDictionaryStringVal( dicCondition, MainView.CONDITION.PROCESS_NM.bindField ) ) );
            // 作業コード
            jobParam.Add( new CoreService.JobParameter(
                TrcWebConsts.JOB_PARAM_KEY_WORK_CD, DataUtils.GetDictionaryStringVal( dicCondition, MainView.CONDITION.WORK_CD.bindField ) ) );
            // 作業名
            jobParam.Add( new CoreService.JobParameter(
                TrcWebConsts.JOB_PARAM_KEY_WORK_NM, DataUtils.GetDictionaryStringVal( dicCondition, MainView.CONDITION.WORK_NM.bindField ) ) );
            // 部品区分
            jobParam.Add( new CoreService.JobParameter(
                TrcWebConsts.JOB_PARAM_KEY_PARTS_CD, DataUtils.GetDictionaryStringVal( dicCondition, MainView.CONDITION.PARTS_CD.bindField ) ) );
            // 品番
            jobParam.Add( new CoreService.JobParameter(
                TrcWebConsts.JOB_PARAM_KEY_PARTS_NUM, DataUtils.GetDictionaryStringVal( dicCondition, MainView.CONDITION.PARTS_NUM.bindField ) ) );
            // 部品機番
            jobParam.Add( new CoreService.JobParameter(
                TrcWebConsts.JOB_PARAM_KEY_PARTS_SERIAL, DataUtils.GetDictionaryStringVal( dicCondition, MainView.CONDITION.PARTS_SERIAL.bindField ) ) );
            // 日付区分
            jobParam.Add( new CoreService.JobParameter(
                TrcWebConsts.JOB_PARAM_KEY_DATE_KIND_CD, DataUtils.GetDictionaryStringVal( dicCondition, MainView.CONDITION.DATE_KIND_CD.bindField ) ) );
            // 日付From
            jobParam.Add( new CoreService.JobParameter(
                TrcWebConsts.JOB_PARAM_KEY_DATE_FROM, DataUtils.GetDictionaryDateVal( dicCondition, MainView.CONDITION.DATE_FROM.bindField ).ToString( DateUtils.DATE_FORMAT_DAY ) ) );
            // 日付To
            jobParam.Add( new CoreService.JobParameter(
                TrcWebConsts.JOB_PARAM_KEY_DATE_TO, DataUtils.GetDictionaryDateVal( dicCondition, MainView.CONDITION.DATE_TO.bindField ).ToString( DateUtils.DATE_FORMAT_DAY ) ) );
            // ラインコード
            jobParam.Add( new CoreService.JobParameter(
                TrcWebConsts.JOB_PARAM_KEY_LINE_CD, DataUtils.GetDictionaryStringVal( dicCondition, MainView.CONDITION.LINE_CD.bindField ) ) );
            // 従業員番号
            jobParam.Add( new CoreService.JobParameter(
                TrcWebConsts.JOB_PARAM_KEY_USER_ID, SessionManager.GetUserInfoHandler().GetUserInfo().UserInfo.userId ) );
            // メールアドレス
            jobParam.Add( new CoreService.JobParameter(
                TrcWebConsts.JOB_PARAM_KEY_MAIL_ADDRESS, SessionManager.GetUserInfoHandler().GetUserInfo().UserInfo.mailAddress ) );
            // ユーザ実行JOB起動要求送信
            C1010RequestDto resultDto = new CoreService().RequestBatchExec2(
                //実行タスクID
                TrcWebConsts.JOB_MASS_DATA_OUTPUT,
                // ログインユーザID
                SessionManager.GetUserInfoHandler().GetUserInfo().UserInfo.userId,
                // ユーザ名
                SessionManager.GetUserInfoHandler().GetUserInfo().UserInfo.userName,
                // メールアドレス
                SessionManager.GetUserInfoHandler().GetUserInfo().UserInfo.mailAddress,
                // 端末IPアドレス
                SessionManager.GetUserInfoHandler().GetUserInfo().UserInfo.ipAddress,
                // JOBネット名
                TrcWebConsts.JOB_PARAM_KEY_JOB_NM_MASS_DATA_OUTPUT,
                // 起動パラメータ         
                jobParam );
            // 要求結果確認
            if ( resultDto.resultCd.IndexOf( "INF" ) >= 0 ) {
                //正常終了
                base.WriteApplicationMessage( MsgManager.MESSAGE_INF_10040, resultDto.resultMessage );
            } else {
                //それ以外
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_72070, resultDto.resultMessage );
            }
        }
        #endregion

        #region 検索処理

        /// <summary>
        /// 検索処理
        /// </summary>
        private void DoSearch() {

            //検索パラメータ作成
            Dictionary<string, object> dicCondition = new Dictionary<string, object>();

            base.GetControlValues( ConditionControls, ref dicCondition );

            //検索時画面情報取得
            Dictionary<string, string> dicIdWithText = new Dictionary<string, string>();
            base.GetControlTexts( ConditionControls, out dicIdWithText );

            //一覧表示列の設定
            string productKindCd = DataUtils.GetDictionaryStringVal( dicCondition, MainView.CONDITION.PRODUCT_KIND_CD.bindField );   //製品区分
            string partsCd = DataUtils.GetDictionaryStringVal( dicCondition, MainView.CONDITION.PARTS_CD.bindField );                //部品区分
            string processCd = DataUtils.GetDictionaryStringVal( dicCondition, MainView.CONDITION.PROCESS_CD.bindField );            //工程区分
            string lineCd = DataUtils.GetDictionaryStringVal( dicCondition, MainView.CONDITION.LINE_CD.bindField );                  //ラインコード

            GridViewDefine[] GridViewResults = GetListColumns( productKindCd, partsCd, processCd, lineCd );

            //検索結果取得
            //エラー時にも最後まで処理を通すため、インスタンスを生成しておく
            MainViewBusiness.ResultSet result = new MainViewBusiness.ResultSet();
            DataTable tblResult = null;
            int maxGridViewCount = WebAppInstance.GetInstance().Config.WebCommonInfo.maxGridViewCount;  //検索上限数
            try {
                result = MainViewBusiness.Search( dicCondition, GridViewResults, maxGridViewCount );
            } catch ( DataAccessException ex ) {
                if ( ex.OracleErrorNumber == DataAccessBase.ORAERROR_TIMEOUT ) {
                    //クエリ発行タイムアウト
                    base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_61910 );
                } else {
                    //タイムアウト以外のException
                    logger.Exception( ex );
                    base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                }
            } catch ( Exception ex ) {
                logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
            } finally {
            }

            ConditionInfoSessionHandler.ST_CONDITION cond = new ConditionInfoSessionHandler.ST_CONDITION();
            tblResult = result.ListTable;
            if ( null != tblResult ) {
                //初期オーダー設定(日付区分に応じて並べ替えを指定)
                tblResult.DefaultView.Sort = GetListInitOrder( dicCondition );

                //件数表示
                ntbResultCount.Value = tblResult.Rows.Count;

                //ページャー設定
                ControlUtils.SetGridViewPager( ref pnlPager, grvMainViewRB, grvMainView_PageIndexChanging, tblResult.Rows.Count, 0 );

                //検索条件/結果インスタンス
                cond.conditionValue = dicCondition;
                cond.IdWithText = dicIdWithText;
                cond.ResultData = tblResult.DefaultView.ToTable();
            } else {
                //タイムアウト等Exception時には、GridViewクリア
                ClearGridView();
            }

            //検索条件をセッションに格納
            ConditionInfo = cond;

            GridView frozenGrid = grvMainViewLB;
            if ( true == ObjectUtils.IsNotNull( tblResult ) ) {
                if ( 0 < tblResult.Rows.Count ) {

                    //TemplateFieldの追加
                    grvHeaderRT.Columns.Clear();
                    grvMainViewRB.Columns.Clear();
                    for ( int idx = frozenGrid.Columns.Count; idx < GridViewResults.Length; idx++ ) {
                        TemplateField tf = new TemplateField();
                        tf.HeaderText = StringUtils.ToString( GridViewResults[idx].bindField );
                        grvMainViewRB.Columns.Add( tf );
                    }

                    //新規バインド
                    GridViewDefine[] tmpGridView = remakeGridView();
                    ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, tmpGridView, true ), ConditionInfo, true );
                    ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, tmpGridView, false ), ConditionInfo, true );
                    ControlUtils.BindGridView_WithTempField( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, tmpGridView, true ), tblResult );
                    ControlUtils.BindGridView_WithTempField( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, tmpGridView, false ), tblResult );

                    //GridView表示
                    divGrvDisplay.Visible = true;

                    //グリッドビュー外のDivサイズ変更
                    SetDivGridViewWidth();

                } else {
                    ClearGridView();
                }
            }

            //メッセージ表示
            if ( null != result.Message ) {
                base.WriteApplicationMessage( result.Message );
            }

            //Excel出力ボタン活性
            if ( null != tblResult && 0 < tblResult.Rows.Count ) {
                this.divGrvCtrlButton.Visible = true;  //出力対象データあり
            } else {
                this.divGrvCtrlButton.Visible = false; //出力対象データなし
            }

            return;
        }

        #endregion

        #region グリッドビューイベント

        /// <summary>
        /// グリッドビュー行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowDataBoundMainView( params object[] parameters ) {
            object sender = parameters[0];
            GridViewRowEventArgs e = (GridViewRowEventArgs)parameters[1];

            if ( e.Row.RowType == DataControlRowType.DataRow ) {
                string productModelCd = StringUtils.ToString( ( (DataRowView)e.Row.DataItem )[GRID_PRODUCT_COMMON.PRODUCT_MODEL_CD.bindField] );
                string serial = StringUtils.ToString( ( (DataRowView)e.Row.DataItem )[GRID_PRODUCT_COMMON.SERIAL.bindField] );

                //エンジン、トラクタのみ詳細画面表示を有効とする
                string productKind = DataUtils.GetDictionaryStringVal( ConditionInfo.conditionValue, MainView.CONDITION.PRODUCT_KIND_CD.bindField );
                if ( ( ProductKind.Engine == productKind || ProductKind.Tractor == productKind ) &&
                    true == StringUtils.IsNotBlank( productModelCd ) &&
                    true == StringUtils.IsNotBlank( serial ) ) {
                    ControlUtils.GridViewRowBound( (GridView)sender, e, ControlUtils.GridRowDoubleClickEvent.WindowOpen, PageInfo.ResolveClientUrl( this, PageInfo.DetailFrame ), base.Token );
                    //ロータリの時には、一覧表示のみとする
                } else {
                    ControlUtils.GridViewRowBound( (GridView)sender, e, ControlUtils.GridRowDoubleClickEvent.None );
                }
            }
        }

        /// <summary>
        /// グリッドビューページ切替
        /// </summary>
        /// <param name="parameters"></param>
        private void PageIndexChangingMainView( params object[] parameters ) {
            object sender = parameters[0];

            CommandEventArgs e = (CommandEventArgs)parameters[1];
            int newPageIndex = Convert.ToInt32( e.CommandArgument );

            int pageSize = grvMainViewRB.PageSize;
            int rowCount = 0;
            if ( true == ObjectUtils.IsNotNull( ConditionInfo.ResultData ) ) {
                rowCount = ConditionInfo.ResultData.Rows.Count;
            }
            int allPages = 0;
            allPages = ConditionInfo.ResultData.Rows.Count / pageSize;
            if ( 0 != rowCount % pageSize ) {
                allPages += 1;
            }
            //ページが無くなっている場合には、先頭ページへ戻す
            if ( newPageIndex >= allPages ) {
                newPageIndex = 0;
            }

            //背面ユーザ切替対応
            ConditionInfoSessionHandler.ST_CONDITION cond = ConditionInfo;
            GridView frozenGrid = grvMainViewLB;

            GridViewDefine[] tmpGridView = remakeGridView();
            ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, tmpGridView, true ), cond, true );
            ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, tmpGridView, false ), cond, true );

            ControlUtils.BindGridView_WithTempField( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, tmpGridView, true ), ConditionInfo.ResultData );
            ControlUtils.BindGridView_WithTempField( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, tmpGridView, false ), ConditionInfo.ResultData );

            ControlUtils.GridViewPageIndexChanging( grvMainViewLB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
            ControlUtils.GridViewPageIndexChanging( grvMainViewRB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );

            ControlUtils.SetGridViewPager( ref pnlPager, grvMainViewRB, grvMainView_PageIndexChanging, ConditionInfo.ResultData.Rows.Count, grvMainViewRB.PageIndex );

            //グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();
        }

        /// <summary>
        /// グリッドビュー並び替え
        /// </summary>
        /// <param name="parameters"></param>
        private void SortingMainView( params object[] parameters ) {
            object sender = parameters[0];
            GridViewSortEventArgs e = (GridViewSortEventArgs)parameters[1];

            ConditionInfoSessionHandler.ST_CONDITION cond = ConditionInfo;
            ControlUtils.GridViewSorting( grvMainViewLB, ref cond, e, true );
            ControlUtils.GridViewSorting( grvMainViewRB, ref cond, e );

            //背面ユーザ切替対応
            GridView frozenGrid = grvMainViewLB;
            GridViewDefine[] tmpGridView = remakeGridView();
            ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, tmpGridView, true ), cond, true );
            ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, tmpGridView, false ), cond, true );

            ControlUtils.BindGridView_WithTempField( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, tmpGridView, true ), cond.ResultData );
            ControlUtils.BindGridView_WithTempField( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, tmpGridView, false ), cond.ResultData );

            ControlUtils.SetGridViewPager( ref pnlPager, grvMainViewRB, grvMainView_PageIndexChanging, cond.ResultData.Rows.Count, grvMainViewRB.PageIndex );

            ConditionInfo = cond;

            //グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();
        }
        #endregion

        #region Excel出力
        /// <summary>
        /// Excel出力処理
        /// </summary>
        private void OutputExcel() {
            //セッションから検索データの取得
            ConditionInfoSessionHandler.ST_CONDITION cond = base.ConditionInfo;
            if ( null == cond.ResultData || 0 == cond.ResultData.Rows.Count ) {
                //出力対象データなし
                return;
            }

            //検索条件出力データ作成
            List<ExcelConditionItem> excelCond = new List<ExcelConditionItem>();
            string condition = "";
            string value = "";

            //製品種別
            condition = CONDITION.PRODUCT_KIND_CD.displayNm;
            value = cond.IdWithText[CONDITION.PRODUCT_KIND_CD.controlId];
            excelCond.Add( new ExcelConditionItem( condition, value ) );

            //製品型式
            condition = CONDITION.MODEL_TYPE.displayNm;
            value = cond.IdWithText[CONDITION.MODEL_TYPE.controlId];
            excelCond.Add( new ExcelConditionItem( condition, value ) );

            //型式コード
            condition = CONDITION.MODEL_CD.displayNm;
            value = cond.IdWithText[CONDITION.MODEL_CD.controlId];
            excelCond.Add( new ExcelConditionItem( condition, value ) );

            //型式名
            condition = CONDITION.MODEL_NM.displayNm;
            value = cond.IdWithText[CONDITION.MODEL_NM.controlId];
            excelCond.Add( new ExcelConditionItem( condition, value ) );

            //製品機番
            condition = CONDITION.SERIAL.displayNm;
            value = cond.IdWithText[CONDITION.SERIAL.controlId];
            excelCond.Add( new ExcelConditionItem( condition, value ) );

            //IDNO
            condition = CONDITION.IDNO.displayNm;
            value = cond.IdWithText[CONDITION.IDNO.controlId];
            excelCond.Add( new ExcelConditionItem( condition, value ) );

            //工程
            condition = CONDITION.PROCESS_CD.displayNm;
            value = cond.IdWithText[CONDITION.PROCESS_CD.controlId];
            excelCond.Add( new ExcelConditionItem( condition, value ) );

            //部品
            condition = CONDITION.PARTS_CD.displayNm;
            value = cond.IdWithText[CONDITION.PARTS_CD.controlId];
            excelCond.Add( new ExcelConditionItem( condition, value ) );

            //品番
            condition = CONDITION.PARTS_NUM.displayNm;
            value = cond.IdWithText[CONDITION.PARTS_NUM.controlId];
            excelCond.Add( new ExcelConditionItem( condition, value ) );

            //機番
            condition = CONDITION.PARTS_SERIAL.displayNm;
            value = cond.IdWithText[CONDITION.PARTS_SERIAL.controlId];
            excelCond.Add( new ExcelConditionItem( condition, value ) );

            //対象(日付区分)
            condition = CONDITION.DATE_KIND_CD.displayNm;
            value = cond.IdWithText[CONDITION.DATE_KIND_CD.controlId];
            excelCond.Add( new ExcelConditionItem( condition, value ) );

            //範囲(日付)
            condition = CONDITION.DATE_FROM.displayNm;
            value = cond.IdWithText[CONDITION.DATE_FROM.controlId] + "～" + cond.IdWithText[CONDITION.DATE_TO.controlId];
            excelCond.Add( new ExcelConditionItem( condition, value ) );

            //Excelダウンロード実行(テーブルはExcel出力用に加工する)
            try {
                Excel.Download( Response, "製品情報", GetExcelTable( cond.ResultData ), excelCond );
            } catch ( System.Threading.ThreadAbortException ) {
                //response.Endで必ず発生する為、正常として扱う
            } catch ( Exception ex ) {
                logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80030, "製品情報_検索結果" );
            }

            return;
        }

        #endregion

        #endregion

        #region メソッド

        #region 画面初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        protected override void Initialize() {

            //アクセスカウンター登録
            Dao.Com.AccessCounterDao.Entry( base.CurrentPageInfo.pageId );

            //ベース処理初期化処理
            base.Initialize();

            //初期化、初期値設定
            InitializeValues();
        }

        /// <summary>
        /// 初期化、初期値設定
        /// </summary>
        private void InitializeValues() {

            //■固定リスト項目

            //製品種別リスト
            ControlUtils.SetListControlItems( rblProductKind, Dao.Com.MasterList.ProductKindList );

            //型式種別リスト
            ControlUtils.SetListControlItems( rblModelType, ModelType.GetList() );

            //■初期値設定

            //製品種別 10:エンジン
            rblProductKind.SelectedValue = ProductKind.Engine;
            //型式種別 0:生産型式
            rblModelType.SelectedValue = ModelType.Product;

            //製品種別に応じたリスト作成
            ChangeProductKind();

            //型式区分、工程部品区分に応じた日付リスト作成
            //SetDateKindList();

            //日付(開始、終了 初期値)
            //当月月初
            cldStart.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, 1 );
            //当月月末
            cldEnd.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth( DateTime.Today.Year, DateTime.Today.Month ) );

            ConditionInfo = new ConditionInfoSessionHandler.ST_CONDITION();

            // 検索[Excel出力]ボタンの表示状態をAction1権限に応じて切り替える
            btnMassDataOutput.Visible = AppPermission.GetTransactionPermission( TRC_W_PWT_ProductView.Defines.PageInfo.MainView, LoginInfo.UserInfo ).IsAction1;

            //グリッドビュー初期化
            ClearGridView();

            //Excel出力ボタン非活性
            this.divGrvCtrlButton.Visible = false;

            // チェックボックス初期値
            chkPinCd.Checked = false;
        }

        #endregion

        #region グリッドビュー

        /// <summary>
        /// グリッドビュークリア
        /// </summary>
        private void ClearGridView() {

            //列名非表示 グリッドビュークリア
            ControlUtils.InitializeGridView( grvHeaderLT, false );
            ControlUtils.InitializeGridView( grvHeaderRT, false );
            ControlUtils.InitializeGridView( grvMainViewLB, false );
            ControlUtils.InitializeGridView( grvMainViewRB, false );

            //グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();

            //件数表示
            ntbResultCount.Value = 0;

            //ページャークリア
            ControlUtils.ClearPager( ref pnlPager );

            //GridView非表示
            divGrvDisplay.Visible = false;

        }

        /// <summary>
        /// グリッドビュー外のDivサイズ変更
        /// </summary>
        private void SetDivGridViewWidth() {

            SetDivGridViewWidth( grvHeaderLT, divGrvHeaderLT );
            SetDivGridViewWidth( grvHeaderRT, divGrvHeaderRT );

            SetDivGridViewWidth( grvMainViewLB, divGrvLB );
            SetDivGridViewWidth( grvMainViewRB, divGrvRB );
        }
        /// <summary>
        /// グリッドビュー外のDivサイズ変更
        /// </summary>
        private void SetDivGridViewWidth( GridView grv, System.Web.UI.HtmlControls.HtmlGenericControl div ) {

            //セル幅補正 Padding4px + Border(片側) 1px
            const int CELL_PADDING = 4 * 2 + 1;
            //テーブル幅補正 Border(片側) 1px
            const int OUT_BORDER = 1;

            var visibleColumns = grv.Columns.Cast<DataControlField>().Where( x => x.Visible ).ToList();
            int sumWidth = NumericUtils.ToInt( visibleColumns.Sum( x => x.HeaderStyle.Width.Value ) )
                                + CELL_PADDING * visibleColumns.Count()
                                + ( visibleColumns.Any() ? OUT_BORDER : 0 );

            div.Style["width"] = $"{ sumWidth }px";

        }

        /// <summary>
        /// 一覧初期ソート
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <returns>DataViewソート文字列</returns>
        private string GetListInitOrder( Dictionary<string, object> condition ) {
            string order = "";
            string orderDtField = "";

            string productKindCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PRODUCT_KIND_CD.bindField );   //製品区分
            string partsCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PARTS_CD.bindField );                //部品区分
            string processCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PROCESS_CD.bindField );            //工程区分
            string dateKindCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.DATE_KIND_CD.bindField );         //日付区分
            string lineCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.LINE_CD.bindField );                  //ラインコード
            // 汎用テーブル使用フラグ
            var genericTableUseFlag = false;
            if ( false == StringUtils.IsEmpty( processCd ) ) {
                // 工程コードがNULLでない場合
                if ( 0 != GenericTableDao.SelectSearchTargetFlag( lineCd, processCd ) ) {
                    // 検索対象フラグONの検索結果が存在する場合、汎用テーブル使用フラグをON
                    genericTableUseFlag = true;
                }
            }

            //トラクタ
            if ( ProductKind.Tractor == productKindCd ) {
                if ( true == genericTableUseFlag ) {
                    // 汎用テーブル使用フラグONの場合
                    orderDtField = GRID_PROCESS_GENERIC_TABLE.RESULT_DT.bindField;
                } else {
                    if ( DateKind.ProductClass.ToBeComplete.value == dateKindCd ) {
                        //完成予定日
                        orderDtField = GRID_PRODUCT_COMMON.PLAN_DT.bindField;
                    } else if ( DateKind.ProductClass.Completed.value == dateKindCd ) {
                        //完成日
                        orderDtField = GRID_PRODUCT_COMMON.PRODUCT_DT.bindField;
                    } else if ( DateKind.ProductClass.Shipped.value == dateKindCd ) {
                        //出荷日
                        orderDtField = GRID_PRODUCT_COMMON.SHIPPED_DT.bindField;
                    } else if ( DateKind.PartsClass.Installing.value == dateKindCd ) {
                        //部品:組付日
                        orderDtField = GRID_PARTS_COMMON.INSTALL_DT.bindField;
                    } else if ( DateKind.ProcessClass.InspectionMeasuring.value == dateKindCd ) {
                        //工程:検査・計測
                        orderDtField = GRID_PROCESS_COMMON.INSPECTION_DT.bindField;
                    } else if ( DateKind.ProductClass.StockOnly.value == dateKindCd ) {
                        //完成予定＋完成日
                        orderDtField = GRID_PRODUCT_COMMON.PRODUCT_DT.bindField;
                    }
                }
            }
            //エンジン
            if ( ProductKind.Engine == productKindCd ) {
                if ( true == genericTableUseFlag ) {
                    // 汎用テーブル使用フラグONの場合
                    orderDtField = GRID_PROCESS_GENERIC_TABLE.RESULT_DT.bindField;
                } else {
                    if ( DateKind.ProductClass.ToBeComplete.value == dateKindCd ) {
                        //完成予定日
                        orderDtField = GRID_PRODUCT_COMMON.PLAN_DT.bindField;
                    } else if ( DateKind.ProductClass.Completed.value == dateKindCd ) {
                        //完成日
                        orderDtField = GRID_PRODUCT_COMMON.PRODUCT_DT.bindField;
                    } else if ( DateKind.ProductClass.Shipped.value == dateKindCd ) {
                        //出荷日
                        orderDtField = GRID_PRODUCT_COMMON.SHIPPED_DT.bindField;
                    } else if ( DateKind.PartsClass.Installing.value == dateKindCd ) {
                        //部品:組付日
                        orderDtField = GRID_PARTS_COMMON.INSTALL_DT.bindField;
                    } else if ( DateKind.PartsClass.Processing.value == dateKindCd ) {
                        //部品:加工日
                        orderDtField = GRID_ENGINE_PARTS_3C.PROCESS_DT.bindField;
                    } else if ( DateKind.ProcessClass.InspectionMeasuring.value == dateKindCd ) {
                        //工程:検査・計測
                        orderDtField = GRID_PROCESS_COMMON.INSPECTION_DT.bindField;
                    } else if ( DateKind.ProductClass.StockOnly.value == dateKindCd ) {
                        //完成予定＋完成日
                        orderDtField = GRID_PRODUCT_COMMON.PRODUCT_DT.bindField;
                    }
                }
            }
            //ロータリー
            if ( ProductKind.Rotary == productKindCd ) {
                if ( DateKind.ProductClass.ToBeComplete.value == dateKindCd ) {
                    //完成予定日
                    orderDtField = GRID_PRODUCT_COMMON.PLAN_DT.bindField;
                } else if ( DateKind.ProductClass.Completed.value == dateKindCd ) {
                    //完成日
                    orderDtField = GRID_PRODUCT_COMMON.PRODUCT_DT.bindField;
                } else if ( DateKind.ProductClass.Shipped.value == dateKindCd ) {
                    //出荷日
                    orderDtField = GRID_PRODUCT_COMMON.SHIPPED_DT.bindField;
                } else if ( DateKind.ProductClass.StockOnly.value == dateKindCd ) {
                    //完成予定＋完成日
                    orderDtField = GRID_PRODUCT_COMMON.PRODUCT_DT.bindField;
                }
            }
            //日付は降順でソート
            if ( false == StringUtils.IsBlank( orderDtField ) ) {
                orderDtField += " desc,";
            }

            //拡張オーダー(結合により型式/機番が複数件となる場合のソート条件)指定
            string orderExtention = "";
            if ( ProductKind.Engine == productKindCd && ProcessKind.PROCESS_CD_ENGINE_TORQUE == processCd ) {
                //エンジン工程:トルク締付
                orderExtention = ",";
                //部品名ごとのオーダを追加
                orderExtention += GRID_PROCESS_ENGINE_TORQUE.PARTS_NM.bindField;
            } else if ( ProductKind.Engine == productKindCd && ProcessKind.PROCESS_CD_ENGINE_FRICTION == processCd ) {
                //エンジン工程：フリクションロス
                orderExtention = ",";
                //回転数ごとのオーダを追加
                orderExtention += GRID_PROCESS_ENGINE_FRICTION.RPM.bindField;
            }

            //日付の後に、型式/機番をソート条件に設定
            order = string.Format( "{0} modelCd, serial {1}", orderDtField, orderExtention );

            return order;
        }
        #endregion

        #region 動的リスト生成

        /// <summary>
        /// 日付区分リスト変更
        /// </summary>
        private void SetDateKindList() {

            //選択項目を退避
            string preSelectedCd = ddlDateKind.SelectedValue;

            string groupCd = "";
            string classCd = "";

            //工程/部品区分(任意排他)の選択項目を取得
            if ( true == StringUtils.IsNotEmpty( hdnProcessCd.Value ) ) {
                groupCd = GroupCd.Process;
                classCd = hdnProcessCd.Value;
            } else if ( true == StringUtils.IsNotEmpty( ddlParts.SelectedValue ) ) {
                groupCd = GroupCd.Parts;
                classCd = ddlParts.SelectedValue;
            }

            //日付区分リスト取得
            ControlUtils.SetListControlItems( ddlDateKind,
                DateKind.GetList( rblModelType.SelectedValue
                                , rblProductKind.SelectedValue
                                , groupCd, classCd, false ) );

            //元選択項目をセット(リストに存在しない場合には、コントロール内で未選択に変更する)
            if ( true == ObjectUtils.IsNotNull( ddlDateKind.Items.FindByValue( preSelectedCd ) ) ) {
                ddlDateKind.SelectedValue = preSelectedCd;
            } else {
                //リストに無い場合(初期化含む)には、デフォルト値をセットする
                //日付区分 0003:出荷
                ddlDateKind.SelectedValue = DateKind.ProductClass.Shipped.value;
            }
        }

        /// <summary>
        /// 部品用検索条件欄の活性変更
        /// </summary>
        private void ChangePartsCondition() {

            //部品条件が選ばれていない時には、品番・部品機番は入力不可
            if ( true == StringUtils.IsEmpty( ddlParts.SelectedValue ) ) {
                //品番 使用不可
                txtPartsNo.Value = null;
                txtPartsNo.Enabled = false;
                //部品機番 使用不可
                txtPartsSerial.Value = null;
                txtPartsSerial.Enabled = false;
            } else {
                txtPartsNo.Enabled = true;
                txtPartsSerial.Enabled = true;
            }
        }

        /// <summary>
        /// 検索時に指定した製品区分、および部品/工程区分から、一覧表示列定義を取得する
        /// </summary>
        /// <param name="productKindCd">製品区分コード</param>
        /// <param name="partsCd">部品区分コード</param>
        /// <param name="processCd">工程区分コード</param>
        /// <param name="lineCd">ラインコード</param>
        /// <returns></returns>
        private GridViewDefine[] GetListColumns( string productKindCd, string partsCd, string processCd, string lineCd ) {
            List<GridViewDefine> columns = new List<GridViewDefine>();
            // 汎用テーブル使用フラグ
            var genericTableUseFlag = false;
            if ( false == StringUtils.IsEmpty( processCd ) ) {
                // 工程コードがNULLでない場合
                if ( 0 != GenericTableDao.SelectSearchTargetFlag( lineCd, processCd ) ) {
                    // 検索対象フラグONの検索結果が存在する場合、汎用テーブル使用フラグをON
                    genericTableUseFlag = true;
                } else {
                    // OFFの場合、工程コードを2桁工程コードに変換する
                    processCd = DataUtils.GetInspectionCd( lineCd: lineCd, processCd: processCd, productKind: productKindCd ) ?? processCd;
                }
            }
            //製品一覧列を取得
            if ( productKindCd == ProductKind.Tractor ) {
                //トラクタ
                columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PRODUCT_COMMON ) ) );
                columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_TRACTOR ) ) );

                if ( false == StringUtils.IsBlank( partsCd ) ) {
                    //トラクタ部品一覧列を取得
                    columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PARTS_COMMON ) ) );
                    switch ( partsCd ) {
                    case PartsKind.PARTS_CD_TRACTOR_CRATE:
                        columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PARTS_TRACTOR_CRATE ) ) );
                        break;
                    case PartsKind.PARTS_CD_TRACTOR_ROPS:
                        columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PARTS_TRACTOR_ROPS ) ) );
                        break;
                    case PartsKind.PARTS_CD_TRACTOR_WECU:
                        columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PARTS_TRACTOR_WECU ) ) );
                        break;
                    case PartsKind.PARTS_CD_TRACTOR_NAMEPLATE:
                        columns.Remove( GRID_PARTS_COMMON.INSTALL_DT );
                        columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PARTS_TRACTOR_NAMEPLATE ) ) );
                        break;
                    case PartsKind.PARTS_CD_TRACTOR_MISSION:
                        columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PARTS_TRACTOR_MISSION ) ) );
                        break;
                    case PartsKind.PARTS_CD_TRACTOR_HOUSING:
                        columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PARTS_TRACTOR_HOUSING ) ) );
                        break;
                    case PartsKind.PARTS_CD_ENGINE_DPF:           //DPF
                    case PartsKind.PARTS_CD_ENGINE_SCR:           //SCR
                    case PartsKind.PARTS_CD_ENGINE_ACU:           //ACU
                        columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_ENGINE_PARTS_COM ) ) );
                        columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_ENGINE_PARTS_TAIL ) ) );
                        break;
                    default:
                        if ( true == partsCd.StartsWith( PartsKind.PARTS_CD_TRACTOR_PREFIX_COREPARTS ) ) {
                            // 基幹部品
                            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PARTS_COREPARTS ) ) );
                        }
                        break;
                    }
                }
                if ( false == StringUtils.IsBlank( processCd ) ) {
                    if ( true == genericTableUseFlag ) {
                        // 汎用テーブル使用フラグがONの場合
                        // 汎用テーブル用カラム追加
                        columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PROCESS_GENERIC_TABLE ) ) );
                    } else {
                        //トラクタ工程一覧列を取得
                        columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PROCESS_COMMON ) ) );
                        switch ( processCd ) {
                        case ProcessKind.PROCESS_CD_TRACTOR_PCRAWER:    //パワクロ走行検査
                            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PROCESS_TRACTOR_PCRAWLER ) ) );
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_CHKSHEET:   //チェックシート
                                                                        //Ver1.2 項目名変更
                            columns.Remove( GRID_PROCESS_COMMON.INSPECTION_DT );
                            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PROCESS_TRACTOR_CHECKSHEET ) ) );
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_CAMIMAGE:   //品質画像証跡
                            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PROCESS_TRACTOR_CAMIMAGE ) ) );
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_ELCHECK:    //電子チェックシート
                                                                        // 電子チェックシートは測定日が不要なため、削除する。
                            columns.Remove( GRID_PROCESS_COMMON.INSPECTION_DT );
                            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PROCESS_TRACTOR_ELCHECK ) ) );
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_SHEEL:        //刻印
                                                                          // 測定日が不要なため、削除する。
                            columns.Remove( GRID_PROCESS_COMMON.INSPECTION_DT );
                            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PROCESS_PRINT_SHEEL ) ) );
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_NUTRUNNER:  //ナットランナー
                                                                        // 測定日の書式が異なるため、共通設定を削除する
                            columns.Remove( GRID_PROCESS_COMMON.INSPECTION_DT );
                            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PROCESS_TRACTOR_NUTRUNNER ) ) );
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_ALL:  //トラクタ走行検査
                            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PROCESS_TRACTOR_ALL ) ) );
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_OPTAXIS:  //光軸検査
                            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PROCESS_OPTAXIS ) ) );
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_CHKPOINT:  //関所
                            columns.Remove( GRID_PROCESS_COMMON.INSPECTION_DT );
                            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PROCESS_CHKPOINT ) ) );
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_IMGCHECK:   //イメージチェックシート
                                                                        // イメージチェックシートは測定日が不要なため、削除する。
                            columns.Remove( GRID_PROCESS_COMMON.INSPECTION_DT );
                            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PROCESS_TRACTOR_IMGCHECK ) ) );
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_TESTBENCH: // 検査ベンチ
                                                                       // 測定日が不要なため、削除する。
                            columns.Remove( GRID_PROCESS_COMMON.INSPECTION_DT );
                            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PROCESS_TRACTOR_TESTBENCH ) ) );
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_AIIMAGE:   // AI画像解析
                            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PROCESS_AIIMAGE ) ) );
                            break;
                        default:
                            break;
                        }
                    }
                }
            } else if ( productKindCd == ProductKind.Engine ) {
                //エンジン
                columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PRODUCT_COMMON ) ) );
                columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_ENGINE ) ) );

                if ( false == StringUtils.IsBlank( partsCd ) ) {
                    //エンジン部品一覧列を取得
                    columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PARTS_COMMON ) ) );
                    switch ( partsCd ) {
                    case PartsKind.PARTS_CD_ENGINE_SUPPLYPUMP:    //サプライポンプ
                    case PartsKind.PARTS_CD_ENGINE_IPU:           //IPU
                    case PartsKind.PARTS_CD_ENGINE_EHC:           //EHC
                    case PartsKind.PARTS_CD_ENGINE_INJECTOR:      //インジェクタ
                    case PartsKind.PARTS_CD_ENGINE_ECU:           //ECU
                    case PartsKind.PARTS_CD_ENGINE_EPR:           //EPR
                    case PartsKind.PARTS_CD_ENGINE_MIXER:         //MIXER
                    case PartsKind.PARTS_CD_ENGINE_RACK_POSITION_SENSOR:  //ラック位置センサ
                        columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_ENGINE_PARTS_COM ) ) );
                        columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_ENGINE_PARTS_COM_DETAIL ) ) );
                        break;
                    case PartsKind.PARTS_CD_ENGINE_DPF:           //DPF
                    case PartsKind.PARTS_CD_ENGINE_SCR:           //SCR
                    case PartsKind.PARTS_CD_ENGINE_DOC:           //DOC
                    case PartsKind.PARTS_CD_ENGINE_ACU:           //ACU
                        columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_ENGINE_PARTS_COM ) ) );
                        columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_ENGINE_PARTS_TAIL ) ) );
                        break;
                    case PartsKind.PARTS_CD_ENGINE_SERIALPRINT:   //SERIALPRINT
                        columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_ENGINE_SERIALPRINT ) ) );
                        break;
                    case PartsKind.PARTS_CD_ENGINE_CC:            //クランクケース
                    case PartsKind.PARTS_CD_ENGINE_CYH:           //シリンダヘッド
                    case PartsKind.PARTS_CD_ENGINE_CS:            //クランクシャフト
                        columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_ENGINE_PARTS_3C ) ) );
                        break;
                    default:
                        if ( true == partsCd.StartsWith( PartsKind.PARTS_CD_TRACTOR_PREFIX_COREPARTS ) ) {
                            // 基幹部品
                            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PARTS_COREPARTS ) ) );
                        }
                        break;
                    }
                }
                if ( false == StringUtils.IsBlank( processCd ) ) {
                    //エンジン工程一覧列を取得
                    if ( true == genericTableUseFlag ) {
                        // 汎用テーブル使用フラグがONの場合
                        // 汎用テーブル用カラム追加
                        columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PROCESS_GENERIC_TABLE ) ) );
                    } else {
                        columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PROCESS_COMMON ) ) );
                        switch ( processCd ) {
                        case ProcessKind.PROCESS_CD_ENGINE_TORQUE:      //トルク締付
                            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PROCESS_ENGINE_TORQUE ) ) );
                            break;
                        case ProcessKind.PROCESS_CD_ENGINE_HARNESS:     //ハーネス検査
                            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PROCESS_ENGINE_HARNESS ) ) );
                            break;
                        case ProcessKind.PROCESS_CD_ENGINE_TEST:        //エンジン運転測定
                            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PROCESS_ENGINE_TEST ) ) );
                            break;
                        case ProcessKind.PROCESS_CD_ENGINE_FRICTION:    //フリクションロス
                            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PROCESS_ENGINE_FRICTION ) ) );
                            break;
                        case ProcessKind.PROCESS_CD_ENGINE_CYH_INSPECT: //シリンダヘッド精密測定
                        case ProcessKind.PROCESS_CD_ENGINE_CS_INSPECT:  //クランクシャフト精密測定
                        case ProcessKind.PROCESS_CD_ENGINE_CC_INSPECT:  //クランクケース精密測定
                            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PROCESS_ENGINE_3C_INSPECT ) ) );
                            break;
                        case ProcessKind.PROCESS_CD_ENGINE_INJECTION:   //噴射時期計測
                            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PROCESS_ENGINE_INJECTION ) ) );
                            break;
                        case ProcessKind.PROCESS_CD_ENGINE_CAMIMAGE:   //品質画像証跡
                            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PROCESS_ENGINE_CAMIMAGE ) ) );
                            break;
                        case ProcessKind.PROCESS_CD_ENGINE_ELCHECK:    //電子チェックシート
                                                                       // 電子チェックシートは測定日が不要なため、削除する。
                            columns.Remove( GRID_PROCESS_COMMON.INSPECTION_DT );
                            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PROCESS_ENGINE_ELCHECK ) ) );
                            break;
                        case ProcessKind.PROCESS_CD_ENGINE_AIIMAGE:   // AI画像解析
                            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PROCESS_AIIMAGE ) ) );
                            break;
                        default:
                            break;
                        }
                    }
                }
            } else if ( productKindCd == ProductKind.Rotary ) {
                //ロータリー
                columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_PRODUCT_COMMON ) ) );
                columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_ROTARY ) ) );
            }

            return columns.ToArray();
        }

        #endregion

        #region Excel出力テーブル作成
        /// <summary>
        /// 一覧結果Excel出力用データテーブルを作成します
        /// </summary>
        /// <param name="gridColumns">一覧表示対象列</param>
        /// <returns>DataTable</returns>
        private DataTable GetExcelTable( DataTable tblSource ) {
            //Excel出力テーブル定義作成(一覧表示対象列を出力対象とする)
            DataTable tblResult = new DataTable();
            foreach ( DataColumn column in tblSource.Columns ) {
                if ( false == StringUtils.IsBlank( column.Caption ) ) {
                    DataColumn colResult = new DataColumn( column.Caption, column.DataType );
                    tblResult.Columns.Add( colResult );
                }
            }

            //一覧元DataTableの情報をExcel出力用テーブルにコピー
            foreach ( DataRow rowSrc in tblSource.Rows ) {
                DataRow rowTo = tblResult.NewRow();
                foreach ( DataColumn column in tblSource.Columns ) {
                    if ( false == StringUtils.IsBlank( column.Caption ) ) {
                        rowTo[column.Caption] = rowSrc[column.ColumnName];
                    }
                }
                tblResult.Rows.Add( rowTo );
            }
            tblResult.AcceptChanges();

            return tblResult;
        }

        #endregion

        #endregion

        #region グリッドビューイベント処理

        protected void grvMainViewLB_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainViewLB( sender, e );
        }

        protected void grvMainViewRB_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainViewRB( sender, e );
        }

        /// <summary>
        /// グリッドビュー行バインド(左下)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowDataBoundMainViewLB( params object[] parameters ) {
            object sender = parameters[0];
            GridViewRowEventArgs e = (GridViewRowEventArgs)parameters[1];

            if ( e.Row.RowType == DataControlRowType.DataRow ) {
                DataRow row = ( (DataRowView)e.Row.DataItem ).Row;
                Dictionary<string, object> dicControls = new Dictionary<string, object>();
                base.SetControlValues( e.Row, ControlUtils.GetControlDefineArray( typeof( GRID_SEARCH_CONTROLS_L ) ), row, ref dicControls );

                string productModelCd = StringUtils.ToString( ( (DataRowView)e.Row.DataItem )[GRID_PRODUCT_COMMON.PRODUCT_MODEL_CD.bindField] );
                string serial = StringUtils.ToString( ( (DataRowView)e.Row.DataItem )[GRID_PRODUCT_COMMON.SERIAL.bindField] );
                //エンジン、トラクタのみ詳細画面表示を有効とする
                string productKind = DataUtils.GetDictionaryStringVal( ConditionInfo.conditionValue, MainView.CONDITION.PRODUCT_KIND_CD.bindField );
                if ( ( ProductKind.Engine == productKind || ProductKind.Tractor == productKind ) &&
                    true == StringUtils.IsNotBlank( productModelCd ) &&
                    true == StringUtils.IsNotBlank( serial ) ) {
                    ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_MAIN_VIEW_GROUP_CD, ControlUtils.GridRowDoubleClickEvent.WindowOpen, PageInfo.ResolveClientUrl( this, PageInfo.DetailFrame ), base.Token );
                    //ロータリの時には、一覧表示のみとする
                } else {
                    ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_MAIN_VIEW_GROUP_CD, ControlUtils.GridRowDoubleClickEvent.None );
                }
            }
        }

        /// <summary>
        /// グリッドビュー行バインド(右下)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowDataBoundMainViewRB( params object[] parameters ) {
            object sender = parameters[0];
            GridViewRowEventArgs e = (GridViewRowEventArgs)parameters[1];

            if ( e.Row.RowType == DataControlRowType.DataRow ) {
                DataRow rowData = ( (DataRowView)e.Row.DataItem ).Row;
                Dictionary<string, object> dicControls = new Dictionary<string, object>();
                base.SetControlValues( e.Row, ControlUtils.GetControlDefineArray( typeof( GRID_SEARCH_CONTROLS_R ) ), rowData, ref dicControls );

                string productModelCd = StringUtils.ToString( ( (DataRowView)e.Row.DataItem )[GRID_PRODUCT_COMMON.PRODUCT_MODEL_CD.bindField] );
                string serial = StringUtils.ToString( ( (DataRowView)e.Row.DataItem )[GRID_PRODUCT_COMMON.SERIAL.bindField] );
                //エンジン、トラクタのみ詳細画面表示を有効とする
                string productKind = DataUtils.GetDictionaryStringVal( ConditionInfo.conditionValue, MainView.CONDITION.PRODUCT_KIND_CD.bindField );
                if ( ( ProductKind.Engine == productKind || ProductKind.Tractor == productKind ) &&
                    true == StringUtils.IsNotBlank( productModelCd ) &&
                    true == StringUtils.IsNotBlank( serial ) ) {
                    ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_MAIN_VIEW_GROUP_CD, ControlUtils.GridRowDoubleClickEvent.WindowOpen, PageInfo.ResolveClientUrl( this, PageInfo.DetailFrame ), base.Token );
                    //ロータリの時には、一覧表示のみとする
                } else {
                    ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_MAIN_VIEW_GROUP_CD, ControlUtils.GridRowDoubleClickEvent.None );
                }

                // 画像URLのリンクを設定
                SetImageUrlLink( e );
            }
        }
        #endregion

        #region
        /// <summary>
        /// GridView再構成
        /// </summary>
        /// <returns></returns>
        private GridViewDefine[] remakeGridView() {

            GridViewDefine[] retGridView = null;

            //一覧表示列の設定
            string productKindCd = DataUtils.GetDictionaryStringVal( ConditionInfo.conditionValue, MainView.CONDITION.PRODUCT_KIND_CD.bindField );   //製品区分
            string partsCd = DataUtils.GetDictionaryStringVal( ConditionInfo.conditionValue, MainView.CONDITION.PARTS_CD.bindField );                //部品区分
            string processCd = DataUtils.GetDictionaryStringVal( ConditionInfo.conditionValue, MainView.CONDITION.PROCESS_CD.bindField );            //工程区分
            string lineCd = DataUtils.GetDictionaryStringVal( ConditionInfo.conditionValue, MainView.CONDITION.LINE_CD.bindField );                  //ラインコード

            retGridView = GetListColumns( productKindCd, partsCd, processCd, lineCd );

            return retGridView;
        }
        #endregion

        #region AI画像解析
        /// <summary>
        /// 画像URLのリンクを設定
        /// </summary>
        /// <param name="e"></param>
        private void SetImageUrlLink( GridViewRowEventArgs e ) {
            //ラインコード、工程コード、製品種別コード取得
            string lineCd = DataUtils.GetDictionaryStringVal( ConditionInfo.conditionValue, MainView.CONDITION.LINE_CD.bindField );
            string processCd = DataUtils.GetDictionaryStringVal( ConditionInfo.conditionValue, MainView.CONDITION.PROCESS_CD.bindField );
            string productKindCd = DataUtils.GetDictionaryStringVal( ConditionInfo.conditionValue, MainView.CONDITION.PRODUCT_KIND_CD.bindField );
            if ( true == StringUtils.IsNotEmpty( processCd ) ) {
                //工程コードがNULL以外
                //2桁工程コードを取得
                processCd = DataUtils.GetInspectionCd( lineCd: lineCd, processCd: processCd, productKind: productKindCd ) ?? processCd;
            }
            if ( ( ( ProductKind.Tractor == productKindCd ) && ( ProcessKind.PROCESS_CD_TRACTOR_AIIMAGE == processCd ) ) ||
                 ( ( ProductKind.Engine == productKindCd ) && ( ProcessKind.PROCESS_CD_ENGINE_AIIMAGE == processCd ) ) ) {
                //AI画像解析で検索されているなら、画像URLを取得
                string url = StringUtils.ToString( ( (DataRowView)e.Row.DataItem )[GRID_PROCESS_AIIMAGE.IMAGE_URL.bindField] );

                if ( StringUtils.IsNotBlank( url ) == true ) {
                    //画像URLが取得できている
                    for ( int cellCount = 0; cellCount < grvMainViewRB.Columns.Count; cellCount++ ) {
                        if ( grvMainViewRB.HeaderRow.Cells[cellCount].Text == GRID_PROCESS_AIIMAGE.IMAGE_URL.headerText ) {
                            //画像URL列のインデックスを取得
                            //リンクボタンをセルに追加
                            HtmlGenericControl linkBtn = new HtmlGenericControl( "a" );
                            linkBtn.Attributes.Add( "href", url );
                            linkBtn.Attributes.Add( "target", "_blank" );
                            linkBtn.InnerText = "Link";
                            e.Row.Cells[cellCount].Controls.Add( linkBtn );

                            break;
                        }
                    }
                }
            }
        }
        #endregion
    }
}