using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KTWebInheritance;
using KTFramework.Common;
using TRC_W_PWT_ProductView.Report.Utils;

namespace TRC_W_PWT_ProductView.Common {

    /// <summary>
    /// WEBシステム用コンフィグデータ
    /// </summary>
    public class ConfigData : KTWebInheritance.BaseClass.BaseConfig {

        #region 定数
        #region XML設定ファイル
        /// <summary>XML設定ファイル カテゴリー(Common)</summary>
        private const string CATEGORY_SETTINGS = "Common";
        /// <summary>XML設定ファイル セクション(WebCommon)</summary>
        private const string SECTION_WEB_COMMON = "WebCommon";
        /// <summary>XML設定ファイル キー(sessionKeep)</summary>
        private const string KEY_SESSION_KEEP = "sessionKeep";
        /// <summary>XML設定ファイル キー(maxListCount)</summary>
        private const string KEY_MAX_GV_COUNT = "maxGridViewCount";
        /// <summary>XML設定ファイル キー(maxListCount)</summary>
        private const string KEY_PARTS_MAX_GV_COUNT = "partsViewMaxGridViewCount";
        /// <summary>XML設定ファイル キー(maxListCount)</summary>
        private const string KEY_PROCESS_MAX_GV_COUNT = "processViewMaxGridViewCount";
        /// <summary>XML設定ファイル キー(maxListCount)</summary>
        private const string KEY_SEARCHORDER_MAX_GV_COUNT = "searchOrderInfoMaxGridViewCount";

        /// <summary>XML設定ファイル カテゴリー(Application)</summary>
        private const string CATEGORY_SETTINGS_APP = "Application";
        /// <summary>XML設定ファイル セクション(WebCommon)</summary>
        private const string SECTION_APP_PROCESS = "ProcessSetting";
        /// <summary>XML設定ファイル セクション(tractorCamImageStation)</summary>
        private const string SECTION_PROCESS_TRACOTER_CI = "tractorCamImageStation";
        /// <summary>XML設定ファイル セクション(engineCamImageStation)</summary>
        private const string SECTION_PROCESS_ENGINE_CI = "engineCamImageStation";
        /// <summary>XML設定ファイル セクション(engineShipmentPartsPickStation)</summary>
        private const string SECTION_PROCESS_ENGINE_SHIPMENTPARTS = "engineShipmentPartsPickStation";
        /// <summary>XML設定ファイル セクション(tractorAiImageStation)</summary>
        private const string SECTION_PROCESS_TRACOTER_AI_IMAGE = "tractorAiImageStation";
        /// <summary>XML設定ファイル セクション(engineAiImageStation)</summary>
        private const string SECTION_PROCESS_ENGINE_AI_IMAGE = "engineAiImageStation";

        /// <summary>XML設定ファイル セクション(ManualFilePath)</summary>
        private const string SECTION_MANUAL_FILE_PATH = "ManualFilePath";
        /// <summary>XML設定ファイル キー(BasePath)</summary>
        private const string KEY_MANUAL_BASE_PATH = "BasePath";
        /// <summary>XML設定ファイル キー(ユーザーズガイド：XXXXXXXX.pdf)</summary>
        private const string KEY_MANUAL_FILE = "Manual";

        /// <summary>XML設定ファイル セクション(DefaultSetting)</summary>
        private const string SECTION_DEFAULT_SETTING = "DefaultSetting";
        /// <summary>XML設定ファイル キー(defaultUrlNum)</summary>
        private const string KEY_DEFAULT_URL_NUM = "defaultUrlNum";

        /// <summary>XML設定ファイル セクション(ReportFilePath)</summary>
        private const string SECTION_REPORT_FILE_PATH = "ReportFilePath";
        /// <summary>XML設定ファイル キー(BasePath)</summary>
        private const string KEY_REPORT_BASE_PATH = "BasePath";
        /// <summary>XML設定ファイル キー(xxxx.xml)</summary>
        private const string KEY_REPORT_EL_CHECK = "ELCheckSheet";
        /// <summary>XML設定ファイル キー(TRCR0002.xml)</summary>
        private const string KEY_REPORT_TRACTOR_IMG_CHECK = "TractorImgCheckSheet";
        /// <summary>XML設定ファイル キー(xxxx.xml)</summary>
        private const string KEY_REPORT_ENGINE_EL_CHECK = "EngineELCheckSheet";
        /// <summary>XML設定ファイル キー(xxxx.xls)</summary>
        private const string KEY_REPORT_NG_REPORT = "NGReport";
        /// <summary>XML設定ファイル キー(xxxx.pdf)</summary>
        private const string KEY_REPORT_ENGINE03_DETAIL = "EngineInjection03Detail";
        /// <summary>XML設定ファイル キー(xxxx.pdf)</summary>
        private const string KEY_REPORT_ENGINE07_DETAIL = "EngineInjection07Detail";

        /// <summary>XML設定ファイル セクション(TempFilePath)</summary>
        private const string SECTION_WORK_FILE_PATH = "TempFilePath";
        /// <summary>XML設定ファイル キー(BasePath)</summary>
        private const string KEY_WORK_BASE_PATH = "BasePath";

        /// <summary>XML設定ファイル セクション(MassDataOutput)</summary>
        private const string SECTION_MASS_DATA_OUTPUT = "MassDataOutput";
        /// <summary>XML設定ファイル キー(SearchPeriod)</summary>
        private const string KEY_SEARCH_PERIOD = "SearchPeriod";

        /// <summary>XML設定ファイル セクション(AiImagePreviewPath)</summary>
        private const string SECTION_AIIMAGE_PREVIEW_PATH = "AiImagePreviewPath";
        /// <summary>XML設定ファイル キー(BasePath)</summary>
        private const string KEY_AIIMAGE_PREVIEW_BASE_PATH = "BasePath";

        #endregion
        #endregion

        #region プロパティ
        /// <summary>アプリケーションWeb基本情報</summary>
        private WebCommonInfoStruct _webCommonInfo;
        /// <summary>アプリケーションWeb基本情報</summary>
        protected internal WebCommonInfoStruct WebCommonInfo { get { return _webCommonInfo; } }

        /// <summary>アプリケーションWeb個別情報</summary>
        private ApplicationInfoStruct _applicationInfo;
        /// <summary>アプリケーションWeb個別情報</summary>
        protected internal ApplicationInfoStruct ApplicationInfo { get { return _applicationInfo; } }
        #endregion

        #region 構造体
        /// <summary>
        /// アプリケーションWeb基本情報構造体
        /// </summary>
        public struct WebCommonInfoStruct {
            /// <summary>
            /// セッション維持自動更新間隔
            /// </summary>
            public int sessionKeep;
            /// <summary>
            /// 一覧最大表示件数
            /// </summary>
            public int maxGridViewCount;
            /// <summary>
            /// 部品検索画面一覧最大表示件数
            /// </summary>
            public int partsViewMaxGridViewCount;
            /// <summary>
            /// 工程検索画面一覧最大表示件数
            /// </summary>
            public int processViewMaxGridViewCount;
            /// <summary>
            /// 順序情報検索画面一覧表示最大検索件数
            /// </summary>
            public int searchOrderInfoMaxGridViewCount;
        }

        public struct ApplicationInfoStruct {
            /// <summary>トラクタ品質画像証跡ステーションリスト</summary>
            public List<string> tractorCamImageStationCdList;
            /// <summary>エンジン品質画像証跡ステーションリスト</summary>
            public List<string> engineCamImageStationCdList;
            /// <summary>出荷部品画像証跡ステーションリスト</summary>
            public List<string> engineShipmentPartsPickStationList;
            /// <summary>トラクタAI画像解析ステーションリスト</summary>
            public List<string> tractorAiImageStationCdList;
            /// <summary>エンジンAI画像解析ステーションリスト</summary>
            public List<string> engineAiImageStationCdList;

            /// <summary>
            /// マニュアルベースパス
            /// </summary>
            public string manualTemplateBasePath;
            /// <summary>
            /// 再検補充マニュアル
            /// </summary>
            public string manualFile;
            /// <summary>
            /// 初期URL_NUM(TM_CC_AUTH_AP 1 = URL / 2 = TEST_URL)
            /// </summary>
            public string defaultUrlNum;
            /// <summary>
            /// 帳票ベースパス
            /// </summary>
            public string reportTemplateBasePath;
            /// <summary>
            /// トラクタ電子チェックシート
            /// </summary>
            public string repELCheckSheet;
            /// <summary>
            /// トラクタイメージチェックシート
            /// </summary>
            public string repTractorImgCheckSheet;
            /// <summary>
            /// エンジン電子チェックシート
            /// </summary>
            public string repEngineELCheckSheet;
            /// <summary>
            /// NG報告書
            /// </summary>
            public string repNGReport;
            /// <summary>
            /// 噴射時期計測03詳細
            /// </summary>
            public string repEngineInjection03Detail;
            /// <summary>
            /// 噴射時期計測07詳細
            /// </summary>
            public string repEngineInjection07Detail;
            /// <summary>
            /// temporaryパス
            /// </summary>
            public string temporaryBasePath;
            /// <summary>
            /// 大量データ出力：検索対象期間(単位：月)
            /// </summary>
            public int searchPeriod;
            /// <summary>
            /// AI画像解析プレビュー画像ベースパス
            /// </summary>
            public string aiimagePreviewBasePath;
        }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="config">ApplicationConfig.xml用Configurator</param>
        public ConfigData( ConfigXml config ) : base( config ) {
            SetWebCommonInfo();
            SetApplicationInfo();
        }

        #region メソッド
        /// <summary>
        /// アプリケーションWeb基本情報設定処理
        /// </summary>
        protected void SetWebCommonInfo() {
            _webCommonInfo = new WebCommonInfoStruct();
            _webCommonInfo.sessionKeep = NumericUtils.ToInt( Config.Config[CATEGORY_SETTINGS][SECTION_WEB_COMMON][KEY_SESSION_KEEP], 0x00 );
            _webCommonInfo.maxGridViewCount = NumericUtils.ToInt( Config.Config[CATEGORY_SETTINGS][SECTION_WEB_COMMON][KEY_MAX_GV_COUNT], 0x00 );
            _webCommonInfo.partsViewMaxGridViewCount = NumericUtils.ToInt( Config.Config[CATEGORY_SETTINGS][SECTION_WEB_COMMON][KEY_PARTS_MAX_GV_COUNT], 0x00 );
            _webCommonInfo.processViewMaxGridViewCount = NumericUtils.ToInt( Config.Config[CATEGORY_SETTINGS][SECTION_WEB_COMMON][KEY_PROCESS_MAX_GV_COUNT], 0x00 );
            _webCommonInfo.searchOrderInfoMaxGridViewCount = NumericUtils.ToInt( Config.Config[CATEGORY_SETTINGS][SECTION_WEB_COMMON][KEY_SEARCHORDER_MAX_GV_COUNT], 0x00 );
            if ( 0 >= _webCommonInfo.maxGridViewCount ) {
                //0未満の場合には最大値を設定
                _webCommonInfo.maxGridViewCount = Int32.MaxValue;
            }
            if ( 0 >= _webCommonInfo.partsViewMaxGridViewCount ) {
                //0未満の場合には最大値を設定
                _webCommonInfo.partsViewMaxGridViewCount = Int32.MaxValue;
            }
            if ( 0 >= _webCommonInfo.processViewMaxGridViewCount ) {
                //0未満の場合には最大値を設定
                _webCommonInfo.processViewMaxGridViewCount = Int32.MaxValue;
            }
            if ( 0 >= _webCommonInfo.searchOrderInfoMaxGridViewCount ) {
                //0未満の場合には最大値を設定
                _webCommonInfo.processViewMaxGridViewCount = Int32.MaxValue;
            }
        }

        /// <summary>
        /// アプリケーションWeb基本情報設定処理
        /// </summary>
        protected void SetApplicationInfo() {
            _applicationInfo = new ApplicationInfoStruct();
            _applicationInfo.tractorCamImageStationCdList = Config.Config[CATEGORY_SETTINGS_APP][SECTION_APP_PROCESS].Get( SECTION_PROCESS_TRACOTER_CI );
            _applicationInfo.engineCamImageStationCdList = Config.Config[CATEGORY_SETTINGS_APP][SECTION_APP_PROCESS].Get( SECTION_PROCESS_ENGINE_CI );
            _applicationInfo.engineShipmentPartsPickStationList = Config.Config[CATEGORY_SETTINGS_APP][SECTION_APP_PROCESS].Get( SECTION_PROCESS_ENGINE_SHIPMENTPARTS );
            _applicationInfo.tractorAiImageStationCdList = Config.Config[CATEGORY_SETTINGS_APP][SECTION_APP_PROCESS].Get( SECTION_PROCESS_TRACOTER_AI_IMAGE );
            _applicationInfo.engineAiImageStationCdList = Config.Config[CATEGORY_SETTINGS_APP][SECTION_APP_PROCESS].Get( SECTION_PROCESS_ENGINE_AI_IMAGE );

            // マニュアルベースパス
            _applicationInfo.manualTemplateBasePath = System.AppDomain.CurrentDomain.BaseDirectory + Config.Config[CATEGORY_SETTINGS_APP][SECTION_MANUAL_FILE_PATH][KEY_MANUAL_BASE_PATH];
            // 排ガス規制部品メンテ　マニュアルファイル
            _applicationInfo.manualFile = Config.Config[CATEGORY_SETTINGS_APP][SECTION_MANUAL_FILE_PATH][KEY_MANUAL_FILE];

            // 初期URL_NUM
            _applicationInfo.defaultUrlNum = Config.Config[CATEGORY_SETTINGS_APP][SECTION_DEFAULT_SETTING][KEY_DEFAULT_URL_NUM];

            // 帳票ファイルベースパス
            _applicationInfo.reportTemplateBasePath = System.AppDomain.CurrentDomain.BaseDirectory + Config.Config[CATEGORY_SETTINGS_APP][SECTION_REPORT_FILE_PATH][KEY_REPORT_BASE_PATH];
            // トラクタ電子チェックシート　帳票ファイル
            _applicationInfo.repELCheckSheet = Config.Config[CATEGORY_SETTINGS_APP][SECTION_REPORT_FILE_PATH][KEY_REPORT_EL_CHECK];
            // トラクタイメージチェックシート　帳票ファイル
            _applicationInfo.repTractorImgCheckSheet = Config.Config[CATEGORY_SETTINGS_APP][SECTION_REPORT_FILE_PATH][KEY_REPORT_TRACTOR_IMG_CHECK];
            // エンジン電子チェックシート　帳票ファイル
            _applicationInfo.repEngineELCheckSheet = Config.Config[CATEGORY_SETTINGS_APP][SECTION_REPORT_FILE_PATH][KEY_REPORT_ENGINE_EL_CHECK];
            // NG報告書　テンプレートファイル
            _applicationInfo.repNGReport = Config.Config[CATEGORY_SETTINGS_APP][SECTION_REPORT_FILE_PATH][KEY_REPORT_NG_REPORT];
            // 噴射時期計測03詳細データ
            _applicationInfo.repEngineInjection03Detail = Config.Config[CATEGORY_SETTINGS_APP][SECTION_REPORT_FILE_PATH][KEY_REPORT_ENGINE03_DETAIL];
            // 噴射時期計測07詳細データ
            _applicationInfo.repEngineInjection07Detail = Config.Config[CATEGORY_SETTINGS_APP][SECTION_REPORT_FILE_PATH][KEY_REPORT_ENGINE07_DETAIL];
            // temporaryベースパス
            _applicationInfo.temporaryBasePath = Config.Config[CATEGORY_SETTINGS_APP][SECTION_WORK_FILE_PATH][KEY_WORK_BASE_PATH];
            // 大量データ出力：検索対象期間
            _applicationInfo.searchPeriod = NumericUtils.ToInt( Config.Config[CATEGORY_SETTINGS_APP][SECTION_MASS_DATA_OUTPUT][KEY_SEARCH_PERIOD] );
            // AI画像解析プレビュー画像ベースパス
            _applicationInfo.aiimagePreviewBasePath = Config.Config[CATEGORY_SETTINGS_APP][SECTION_AIIMAGE_PREVIEW_PATH][KEY_AIIMAGE_PREVIEW_BASE_PATH];

            // PDFフォント設定
            PdfSharp.Fonts.GlobalFontSettings.FontResolver = new JapaneseFontResolver();
        }
        #endregion
    }
}