using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TRC_W_PWT_ProductView.Defines.ListDefine {
    /// <summary>
    /// 部品区分定義クラス
    /// </summary>
    public static class PartsKind {

        /// <summary>部品区分:[トラクタ]WiFi ECU</summary>
        public const string PARTS_CD_TRACTOR_WECU = "T1";
        /// <summary>部品区分:[トラクタ]クレート</summary>
        public const string PARTS_CD_TRACTOR_CRATE = "T2";
        /// <summary>部品区分:[トラクタ]ロプス</summary>
        public const string PARTS_CD_TRACTOR_ROPS = "T3";
        /// <summary>部品区分:[トラクタ]銘板ラベル</summary>
        public const string PARTS_CD_TRACTOR_NAMEPLATE = "T4";
        /// <summary>部品区分:[トラクタ]ミッション</summary>
        public const string PARTS_CD_TRACTOR_MISSION = "T5";
        /// <summary>部品区分:[トラクタ]ハウジング</summary>
        public const string PARTS_CD_TRACTOR_HOUSING = "T6";

        /// <summary>部品区分:[共通]基幹部品(接頭句)</summary>
        public const string PARTS_CD_TRACTOR_PREFIX_COREPARTS = "C";

        /// <summary>部品区分:[エンジン]サプライポンプ[01]</summary>
        public const string PARTS_CD_ENGINE_SUPPLYPUMP = "01";
        /// <summary>部品区分:[エンジン]コモンレール[02]</summary>
        //public const string PARTS_CD_ENGINE_RAIL = "02";
        /// <summary>部品区分:[エンジン]ECU[03]</summary>
        public const string PARTS_CD_ENGINE_ECU = "03";
        /// <summary>部品区分:[エンジン]インジェクタ[04]</summary>
        public const string PARTS_CD_ENGINE_INJECTOR = "04";
        /// <summary>部品区分:[エンジン]DPF[05]</summary>
        public const string PARTS_CD_ENGINE_DPF = "05";
        /// <summary>部品区分:[エンジン]クランクケース[06]</summary>
        public const string PARTS_CD_ENGINE_CC = "06";
        /// <summary>部品区分:[エンジン]EPR[07]</summary>
        public const string PARTS_CD_ENGINE_EPR = "07";
        /// <summary>部品区分:[エンジン]MIXER[08]</summary>
        public const string PARTS_CD_ENGINE_MIXER = "08";
        /// <summary>部品区分:[エンジン]シリンダヘッド[09]</summary>
        public const string PARTS_CD_ENGINE_CYH = "09";
        /// <summary>部品区分:[エンジン]クランクシャフト[10]</summary>
        public const string PARTS_CD_ENGINE_CS = "10";
        /// <summary>部品区分:[エンジン]DOC[11]</summary>
        public const string PARTS_CD_ENGINE_DOC = "11";
        /// <summary>部品区分:[エンジン]SCR[12]</summary>
        public const string PARTS_CD_ENGINE_SCR = "12";
        /// <summary>部品区分:[エンジン]ACU[13]</summary>
        public const string PARTS_CD_ENGINE_ACU = "13";
        /// <summary>部品区分:[エンジン]ラック位置センサ[14]</summary>
        public const string PARTS_CD_ENGINE_RACK_POSITION_SENSOR = "14";
        /// <summary>部品区分:[エンジン]IPU[15]</summary>
        public const string PARTS_CD_ENGINE_IPU = "15";
        /// <summary>部品区分:[エンジン]EHC[16]</summary>
        public const string PARTS_CD_ENGINE_EHC = "16";
        /// <summary>部品区分:[エンジン]SERIALPRINT[99]</summary>
        public const string PARTS_CD_ENGINE_SERIALPRINT = "99";

    }
}