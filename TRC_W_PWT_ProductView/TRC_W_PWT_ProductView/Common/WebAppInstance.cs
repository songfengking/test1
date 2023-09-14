using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using KTFramework.Common;
using KTFramework.Common.App.WebApp;

namespace TRC_W_PWT_ProductView.Common {

    /// <summary>
    /// Webアプリケーション
    /// </summary>
    public class WebAppInstance : WebAppBase,IDisposable {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        /// <summary>
        /// アプリケーション (TBL_KTAUTHAPマスタ)
        /// </summary>
        public const string KTAUTH_AP_MASTER_ID = "140011";

        /// <summary>ゲストID</summary>
        public const string GUEST_ID = "guest";

        /// <summary>ゲストPW</summary>
        public const string GUEST_PW = "guest";

        /// <summary>ゲスト権限</summary>
        public const string GUEST_AP_AUTH = "GUEST";

        #region 変数

        /// <summary>
        /// 破棄状況
        /// </summary>
        bool _disposed = false;

        #endregion

        #region プロパティ
        /// <summary>設定情報</summary>
        private ConfigData _config;
        /// <summary>設定情報</summary>
        public ConfigData Config { get { return _config; } }

        /// <summary>
        /// 短縮アセンブリVersion
        /// </summary>
        public string ShortAssemblyVer {
            get {
                Version ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                return string.Format("{0}.{1}.{2}", ver.Major, ver.Minor, ver.Build);
            }
        }
        #endregion

        /// <summary>インスタンス取得</summary>
        /// <returns>インスタンス</returns>
        new public static WebAppInstance GetInstance() {
            return (WebAppInstance)WebAppBase.GetInstance();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="args"></param>
        /// <param name="httpApp"></param>
        public WebAppInstance( Assembly assembly, string[] args, HttpApplication httpApp ) : base( assembly, args, httpApp ) {
            try {
                //アプリケーション共通使用のDBリストを取得する
                Dao.Com.MasterList.GetAllMasterList();
            } catch ( Exception ex ) {
                logger.Exception( ex );
            }
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~WebAppInstance() {
            logger.Info( "{0} {1}", "処理実行", MethodBase.GetCurrentMethod().Name );
            Dispose( false );
        }

        /// <summary>
        /// リソース破棄
        /// </summary>
        public void Dispose() {
            logger.Info( "{0} {1}", "処理実行", MethodBase.GetCurrentMethod().Name );
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        /// <summary>
        /// リソース破棄
        /// </summary>
        /// <param name="disposing">true = 明示的な破棄 / false = 暗黙的な破棄</param>
        private void Dispose( bool disposing ) {

            if ( false == _disposed ) {
                if ( true == disposing ) {
                    //終了処理
                    try {
                        FinishProcess();
                    } catch ( Exception ex ) {
                        logger.Exception( ex );
                    }
                }
            }
            _disposed = true;
        }

        /// <summary>
        /// コンフィグデータ読み込み
        /// </summary>
        public void ReadConfig() {

            try {
                _config = new ConfigData( AppConfig );
            } catch ( Exception ex ) {
                logger.Exception( ex );
                throw ex;
            }
        }
    }
}