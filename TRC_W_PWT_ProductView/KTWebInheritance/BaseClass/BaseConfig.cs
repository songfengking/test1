using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KTFramework.Common;

namespace KTWebInheritance.BaseClass {

    /// <summary>
    /// コンフィグファイルアクセスベース
    /// </summary>
    public abstract class BaseConfig {

        #region 定数
        #region XML設定ファイル
        ///// <summary>XML設定ファイル カテゴリー(Settings)</summary>
        //private const string CATEGORY_SETTINGS = "Settings";
        ///// <summary>XML設定ファイル セクション(Common)</summary>
        //private const string SECTION_COMMON = "Common";
        ///// <summary>XML設定ファイル キー(plantCd)</summary>
        //private const string KEY_PLANT_CD = "plantCd";
        ///// <summary>XML設定ファイル キー(plantNm)</summary>
        //private const string KEY_PLANT_NM = "plantNm";
        ///// <summary>XML設定ファイル キー(class)</summary>
        //private const string KEY_CLASS_CD = "class";
        ///// <summary>XML設定ファイル キー(project)</summary>
        //private const string KEY_PROJECT_CD = "project";
        ///// <summary>XML設定ファイル キー(id)</summary>
        //private const string KEY_PROJECT_ID = "id";
        ///// <summary>XML設定ファイル キー(name)</summary>
        //private const string KEY_PROJECT_NM = "name";
        ///// <summary>XML設定ファイル キー(display)</summary>
        //private const string KEY_DISPLAY_NM = "display";
        #endregion
        #endregion

        #region プロパティ        
        /// <summary>Configファイル</summary>
        private ConfigXml _config;
        /// <summary>Configファイル</summary>
        protected internal ConfigXml Config { get { return _config; } }
        ///// <summary>アプリケーション基本情報</summary>
        //private KTApplicationInfoStruct _ktApplicationInfo;
        ///// <summary>アプリケーション基本情報</summary>
        //protected internal KTApplicationInfoStruct KTApplicationInfo { get { return _ktApplicationInfo; } }
        #endregion

        //#region 構造体
        ///// <summary>
        ///// アプリケーション基本情報構造体
        ///// </summary>
        //public struct KTApplicationInfoStruct {
        //    /// <summary>
        //    /// 工場コード
        //    /// </summary>
        //    public string plantCd;
        //    /// <summary>
        //    /// 工場名
        //    /// </summary>
        //    public string plantNm;
        //    /// <summary>
        //    /// クラスコード
        //    /// </summary>
        //    public string classCd;
        //    /// <summary>
        //    /// プロジェクト短縮コード
        //    /// </summary>
        //    public string projectCd;
        //    /// <summary>
        //    /// プロジェクトID
        //    /// </summary>
        //    public string projectId;
        //    /// <summary>
        //    /// プロジェクト名
        //    /// </summary>
        //    public string projectNm;
        //    /// <summary>
        //    /// 表示用プロジェクト名
        //    /// </summary>
        //    public string displayNm;
        //}
        //#endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="config">ApplicationConfig.xml用Configurator</param>
        public BaseConfig( ConfigXml config ) {            
            _config = config;
        }
        #endregion

        //#region メソッド
        ///// <summary>
        ///// アプリケーション基本情報設定処理
        ///// </summary>
        //protected void SetApplicationInfo() {
        //    _ktApplicationInfo = new KTApplicationInfoStruct();
        //    _ktApplicationInfo.plantCd = Config.Config[CATEGORY_SETTINGS][SECTION_COMMON][KEY_PLANT_CD];
        //    _ktApplicationInfo.plantNm = Config.Config[CATEGORY_SETTINGS][SECTION_COMMON][KEY_PLANT_NM];
        //    _ktApplicationInfo.classCd = Config.Config[CATEGORY_SETTINGS][SECTION_COMMON][KEY_CLASS_CD];
        //    _ktApplicationInfo.projectCd = Config.Config[CATEGORY_SETTINGS][SECTION_COMMON][KEY_PROJECT_CD];
        //    _ktApplicationInfo.projectId = Config.Config[CATEGORY_SETTINGS][SECTION_COMMON][KEY_PROJECT_ID];
        //    _ktApplicationInfo.projectNm = Config.Config[CATEGORY_SETTINGS][SECTION_COMMON][KEY_PROJECT_NM];
        //    _ktApplicationInfo.displayNm = Config.Config[CATEGORY_SETTINGS][SECTION_COMMON][KEY_DISPLAY_NM];
        //}
        //#endregion
    }
}
