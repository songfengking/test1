using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TRC_W_PWT_ProductView.Report.Dto {
    /// <summary>
    /// 噴射時期計測データ印字データクラス
    /// </summary>
    public class EngineInjection07DetailDto {
        /// <summary>測定日時</summary>
        public DateTime? MeasuringDate { get; set; }
        /// <summary>エンジン型式名</summary>
        public string ModelName { get; set; }
        /// <summary>エンジン機番</summary>
        public string Serial { get; set; }
        /// <summary>IDNo</summary>
        public string Idno { get; set; }
        /// <summary>噴射ポンプ品番</summary>
        public string PonpuPartNum { get; set; }
        /// <summary>噴射ポンプ機番</summary>
        public string PonpuSerial { get; set; }
        /// <summary>判定(値)</summary>
        public int Result { get; set; }
        /// <summary>判定(OK/NG)</summary>
        public string ResultDisplayString { get; set; }
        /// <summary>ピストン出代(下限)</summary>
        public decimal? PistonBumpLowerLimit { get; set; }
        /// <summary>ピストン出代(上限)</summary>
        public decimal? PistonBumpUpperLimit { get; set; }
        /// <summary>ピストン出代(気筒1)</summary>
        public decimal? PistonBumpCylinder1 { get; set; }
        /// <summary>ピストン出代(気筒2)</summary>
        public decimal? PistonBumpCylinder2 { get; set; }
        /// <summary>ピストン出代(気筒3)</summary>
        public decimal? PistonBumpCylinder3 { get; set; }
        /// <summary>ピストン出代(気筒4)</summary>
        public decimal? PistonBumpCylinder4 { get; set; }
        /// <summary>ピストン出代(平均)</summary>
        public decimal? PistonBumpAverage { get; set; }
        /// <summary>ピストン出代ランク(下限)</summary>
        public decimal? PistonBumpRankLowerLimit { get; set; }
        /// <summary>ピストン出代ランク(上限)</summary>
        public decimal? PistonBumpRankUpperLimit { get; set; }
        /// <summary>ピストン出代ランク</summary>
        public string PistonBumpRank { get; set; }
        /// <summary>燃料噴射時期(下限)</summary>
        public decimal? InjectionTimingLowerLimit { get; set; }
        /// <summary>燃料噴射時期(上限)</summary>
        public decimal? InjectionTimingUpperLimit { get; set; }
        /// <summary>燃料噴射時期(気筒1)</summary>
        public decimal? InjectionTimingCylinder1 { get; set; }
        /// <summary>燃料噴射時期(気筒2)</summary>
        public decimal? InjectionTimingCylinder2 { get; set; }
        /// <summary>燃料噴射時期(気筒3)</summary>
        public decimal? InjectionTimingCylinder3 { get; set; }
        /// <summary>燃料噴射時期(気筒4)</summary>
        public decimal? InjectionTimingCylinder4 { get; set; }
        /// <summary>燃料噴射時期(平均)</summary>
        public decimal? InjectionTimingAverage { get; set; }
        /// <summary>気温</summary>
        public decimal? Temperature { get; set; }
        /// <summary>パルスタイミング角度</summary>
        public decimal? PulseTimingAngle { get; set; }
    }
}