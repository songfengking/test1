using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TRC_W_PWT_ProductView.Report.Dto {
    /// <summary>
    /// 噴射時期計測データ印字データクラス
    /// </summary>
    public class EngineInjection03DetailDto {
        /// <summary>測定日時</summary>
        public DateTime? MeasuringDate { get; set; }
        /// <summary>生産型式名</summary>
        public string ModelName { get; set; }
        /// <summary>機番</summary>
        public string Serial { get; set; }
        /// <summary></summary>
        public string EnginePrint { get; set; }
        /// <summary></summary>
        public string Idno { get; set; }
        /// <summary></summary>
        public int Result { get; set; }
        /// <summary></summary>
        public string ResultDisplayString { get; set; }
        /// <summary></summary>
        public int? NumberOfCylinder { get; set; }
        /// <summary></summary>
        public decimal? Stroke { get; set; }
        /// <summary></summary>
        public int? Bore { get; set; }
        /// <summary></summary>
        public string CombustionSystem { get; set; }
        /// <summary></summary>
        public int AdjustAdvance { get; set; }
        /// <summary></summary>
        public string AdjustAdvanceDisplayString { get; set; }
        /// <summary></summary>
        public decimal? LackPosition { get; set; }
        /// <summary></summary>
        public decimal? LackSizeDifference { get; set; }
        /// <summary></summary>
        public string ScrewType { get; set; }
        /// <summary></summary>
        public decimal? InjectionTimingOffset { get; set; }
        /// <summary></summary>
        public int MeasuringTerminal { get; set; }
        /// <summary></summary>
        public int Recess { get; set; }
        /// <summary></summary>
        public string RecessDisplayString { get; set; }
        /// <summary></summary>
        public int MethodCylinder { get; set; }
        /// <summary></summary>
        public string MethodCylinderDisplayString { get; set; }
        /// <summary></summary>
        public int MethodDirection { get; set; }
        /// <summary></summary>
        public string MethodDirectionDisplayString { get; set; }
        /// <summary></summary>
        public decimal? PistonBumpUpperLimit { get; set; }
        /// <summary></summary>
        public decimal? PistonBumpLowerLimit { get; set; }
        /// <summary></summary>
        public decimal? PistonBumpCylinder1 { get; set; }
        /// <summary></summary>
        public decimal? PistonBumpCylinder2 { get; set; }
        /// <summary></summary>
        public decimal? PistonBumpCylinder3 { get; set; }
        /// <summary></summary>
        public decimal? PistonBumpCylinder4 { get; set; }
        /// <summary></summary>
        public decimal? TopClearanceUpperLimit { get; set; }
        /// <summary></summary>
        public decimal? TopClearanceLowerLimit { get; set; }
        /// <summary></summary>
        public decimal? TopClearanceCylinder1 { get; set; }
        /// <summary></summary>
        public decimal? TopClearanceCylinder2 { get; set; }
        /// <summary></summary>
        public decimal? TopClearanceCylinder3 { get; set; }
        /// <summary></summary>
        public decimal? TopClearanceCylinder4 { get; set; }
        /// <summary></summary>
        public decimal? InjectionTimingUpperLimit { get; set; }
        /// <summary></summary>
        public decimal? InjectionTimingLowerLimit { get; set; }
        /// <summary></summary>
        public decimal? InjectionTimingCylinder1 { get; set; }
        /// <summary></summary>
        public decimal? InjectionTimingCylinder2 { get; set; }
        /// <summary></summary>
        public decimal? InjectionTimingCylinder3 { get; set; }
        /// <summary></summary>
        public decimal? InjectionTimingCylinder4 { get; set; }
        /// <summary></summary>
        public decimal? BumpSizeVariableBase { get; set; }
        /// <summary></summary>
        public decimal? BumpSizeError { get; set; }
        /// <summary></summary>
        public string StandardGascketNumber { get; set; }
        /// <summary></summary>
        public decimal? GascketSize { get; set; }
        /// <summary></summary>
        public string SelectedGascketNumber { get; set; }
        /// <summary></summary>
        public decimal? BasePumpShimSize { get; set; }
        /// <summary></summary>
        public decimal? TotalShimSize { get; set; }
        /// <summary></summary>
        public int? NumberOfShim0175{ get; set; }
        /// <summary></summary>
        public int? NumberOfShim0200 { get; set; }
        /// <summary></summary>
        public int? NumberOfShim0250 { get; set; }
        /// <summary></summary>
        public int? NumberOfShim0300 { get; set; }
        /// <summary></summary>
        public int? NumberOfShim0350 { get; set; }
        /// <summary></summary>
        public int? TotalNumberOfShim { get; set; }
        /// <summary></summary>
        public decimal? JigOffset1 { get; set; }
        /// <summary></summary>
        public decimal? JigOffset2 { get; set; }
        /// <summary></summary>
        public decimal? JigOffset3 { get; set; }
        /// <summary></summary>
        public decimal? MeasurementOffset { get; set; }
        /// <summary></summary>
        public decimal? FitOffsetCylinderAverage { get; set; }
        /// <summary></summary>
        public decimal? FitOffsetCylinder1 { get; set; }
        /// <summary></summary>
        public decimal? FitOffsetCylinder2 { get; set; }
        /// <summary></summary>
        public decimal? FitOffsetCylinder3 { get; set; }
        /// <summary></summary>
        public decimal? FitOffsetCylinder4 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringRow1Cylinder1 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringRow1Cylinder2 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringRow1Cylinder3 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringRow1Cylinder4 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringRow2Cylinder1 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringRow2Cylinder2 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringRow2Cylinder3 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringRow2Cylinder4 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringRow3Cylinder1 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringRow3Cylinder2 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringRow3Cylinder3 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringRow3Cylinder4 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringRow4Cylinder1 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringRow4Cylinder2 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringRow4Cylinder3 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringRow4Cylinder4 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringOffset1Cylinder1 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringOffset1Cylinder2 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringOffset1Cylinder3 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringOffset1Cylinder4 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringOffset2Cylinder1 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringOffset2Cylinder2 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringOffset2Cylinder3 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringOffset2Cylinder4 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringOffset3Cylinder1 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringOffset3Cylinder2 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringOffset3Cylinder3 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringOffset3Cylinder4 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringOffset4Cylinder1 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringOffset4Cylinder2 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringOffset4Cylinder3 { get; set; }
        /// <summary></summary>
        public decimal? MeasuringOffset4Cylinder4 { get; set; }
        /// <summary></summary>
        public string Remark { get; set; }
    }
}