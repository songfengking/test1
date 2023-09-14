using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TRC_W_PWT_ProductView.Defines.ListDefine {
    /// <summary>
    /// 工程区分定義クラス
    /// </summary>
    public static class ProcessKind {
        /// <summary>工程区分:[エンジン/トラクタ]共用工程</summary>
        public const string PROCESS_CD_COMMON_PROCESS = "00";

        /// <summary>工程区分:[トラクタ]パワクロ走行検査</summary>
        public const string PROCESS_CD_TRACTOR_PCRAWER = "01";
        /// <summary>工程区分:[トラクタ]チェックシート</summary>
        public const string PROCESS_CD_TRACTOR_CHKSHEET = "03";
        /// <summary>工程区分:[トラクタ]品質画像証跡</summary>
        public const string PROCESS_CD_TRACTOR_CAMIMAGE = "04";
        /// <summary>工程区分:[トラクタ]電子チェックシート</summary>
        public const string PROCESS_CD_TRACTOR_ELCHECK = "05";
        /// <summary>工程区分:[トラクタ]刻印</summary>
        public const string PROCESS_CD_TRACTOR_SHEEL = "06";
        /// <summary>工程区分:[トラクタ]ナットランナー</summary>
        public const string PROCESS_CD_TRACTOR_NUTRUNNER = "07";
        /// <summary>工程区分:[トラクタ]トラクタ走行検査</summary>
        public const string PROCESS_CD_TRACTOR_ALL = "08";
        /// <summary>工程区分:[トラクタ]光軸検査</summary>
        public const string PROCESS_CD_TRACTOR_OPTAXIS = "09";
        /// <summary>工程区分:[トラクタ]関所</summary>
        public const string PROCESS_CD_TRACTOR_CHKPOINT = "10";
        /// <summary>工程区分:[トラクタ]イメージチェックシート</summary>
        public const string PROCESS_CD_TRACTOR_IMGCHECK = "11";
        /// <summary>工程区分:[トラクタ]検査ベンチ</summary>
        public const string PROCESS_CD_TRACTOR_TESTBENCH = "12";
        /// <summary>工程区分:[トラクタ]AI画像解析</summary>
        public const string PROCESS_CD_TRACTOR_AIIMAGE = "13";
        /// <summary>工程区分:[エンジン]トルク締付</summary>
        public const string PROCESS_CD_ENGINE_TORQUE = "01";
        /// <summary>工程区分:[エンジン]ハーネス検査</summary>
        public const string PROCESS_CD_ENGINE_HARNESS = "02";
        /// <summary>工程区分:[エンジン]エンジン運転測定</summary>
        public const string PROCESS_CD_ENGINE_TEST = "03";
        /// <summary>工程区分:[エンジン]フリクションロス</summary>
        public const string PROCESS_CD_ENGINE_FRICTION = "04";
        /// <summary>工程区分:[エンジン]シリンダヘッド組付</summary>
        //public const string PROCESS_CD_ENGINE_CYH_ASSEMBLY = "05";
        /// <summary>工程区分:[エンジン]シリンダヘッド精密測定</summary>
        public const string PROCESS_CD_ENGINE_CYH_INSPECT = "06";
        /// <summary>工程区分:[エンジン]クランクシャフト精密測定</summary>
        public const string PROCESS_CD_ENGINE_CS_INSPECT = "07";
        /// <summary>工程区分:[エンジン]クランクケース精密測定</summary>
        public const string PROCESS_CD_ENGINE_CC_INSPECT = "08";
        /// <summary>工程区分:[エンジン]噴射時期計測</summary>
        public const string PROCESS_CD_ENGINE_INJECTION = "09";
        /// <summary>工程区分:[エンジン]品質画像証跡</summary>
        public const string PROCESS_CD_ENGINE_CAMIMAGE = "10";
        /// <summary>工程区分:[エンジン]電子チェックシート</summary>
        public const string PROCESS_CD_ENGINE_ELCHECK = "11";
        /// <summary>工程区分:[エンジン]出荷部品</summary>
        public const string PROCESS_CD_ENGINE_SHIPMENTPARTS = "12";
        /// <summary>工程区分:[エンジン]AI画像解析</summary>
        public const string PROCESS_CD_ENGINE_AIIMAGE = "13";
    }
}