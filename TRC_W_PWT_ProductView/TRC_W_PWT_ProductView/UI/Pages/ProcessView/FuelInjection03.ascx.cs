using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using KTFramework.Common;
using KTFramework.Dao;
using KTWebControl.CustomControls;
using KTWebInheritance.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.UI.Base;

namespace TRC_W_PWT_ProductView.UI.Pages.ProcessView {

    /// <summary>
    /// (詳細 エンジン 工程) 噴射時期計測(03エンジン)
    /// </summary>
    public partial class FuelInjection03 : System.Web.UI.UserControl, Defines.Interface.IDetail {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義

        /// <summary>
        /// (メインリスト)コントロール定義
        /// </summary>
        public class GRID_MAIN {
            /// <summary>計測日時</summary>                    
            public static readonly ControlDefine INSPECTION_DT = new ControlDefine( "txtInspectionDt", "計測日時", "inspectionDt", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>エンジン刻印名</summary>                
            public static readonly ControlDefine CARVED_SEAL_NM = new ControlDefine( "txtCarvedSealNm", "エンジン刻印名", "carvedSealNm", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>基準ポンプシム寸法</summary>            
            public static readonly ControlDefine PUMP_SHIM_SIZE = new ControlDefine( "ntbPumpShimSize", "基準ポンプシム寸法", "pumpShimSize", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>気筒数</summary>                        
            public static readonly ControlDefine CYLINDER_QTY = new ControlDefine( "ntbCylinderQty", "気筒数", "cylinderQty", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>ストローク</summary>                    
            public static readonly ControlDefine STROKE = new ControlDefine( "ntbStroke", "ストローク", "stroke", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>ボア</summary>                          
            public static readonly ControlDefine BORE = new ControlDefine( "ntbBore", "ボア", "bore", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>燃焼方式</summary>                      
            public static readonly ControlDefine COMBUSTION = new ControlDefine( "txtCombustion", "燃焼方式", "combustion", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>ピストン出代上限</summary>              
            public static readonly ControlDefine PISTON_BUMP_UPPER_LIMIT = new ControlDefine( "ntbPistonBumpUpperLimit", "ピストン出代上限", "pistonBumpUpperLimit", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>ピストン出代下限</summary>              
            public static readonly ControlDefine PISTON_BUMP_LOWER_LIMIT = new ControlDefine( "ntbPistonBumpLowerLimit", "ピストン出代下限", "pistonBumpLowerLimit", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>トップクリアランス上限</summary>        
            public static readonly ControlDefine TOP_CLEARANCE_UPPER_LIMIT = new ControlDefine( "ntbTopClearanceUpperLimit", "トップクリアランス上限", "topClearanceUpperLimit", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>トップクリアランス下限</summary>        
            public static readonly ControlDefine TOP_CLEARANCE_LOWER_LIMIT = new ControlDefine( "ntbTopClearanceLowerLimit", "トップクリアランス下限", "topClearanceLowerLimit", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>燃料噴射時期上限</summary>              
            public static readonly ControlDefine INJECTION_TIMING_UPPER_LIMIT = new ControlDefine( "ntbInjectionTimingUpperLimit", "燃料噴射時期上限", "injectionTimingUpperLimit", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>燃料噴射時期下限</summary>              
            public static readonly ControlDefine INJECTION_TIMING_LOWER_LIMIT = new ControlDefine( "ntbInjectionTimingLowerLimit", "燃料噴射時期下限", "injectionTimingLowerLimit", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>セットシム量上限</summary>              
            public static readonly ControlDefine SYM_UPPER_LIMIT = new ControlDefine( "ntbSymUpperLimit", "セットシム量上限", "symUpperLimit", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>セットシム量下限</summary>              
            public static readonly ControlDefine SYM_LOWER_LIMIT = new ControlDefine( "ntbSymLowerLimit", "セットシム量下限", "symLowerLimit", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>進角調整</summary>                      
            public static readonly ControlDefine ADVANCE_ADJUST = new ControlDefine( "txtAdvanceAdjust", "進角調整", "advanceAdjust", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>ラック位置</summary>                    
            public static readonly ControlDefine RACK_POSITION = new ControlDefine( "ntbRackPosition", "ラック位置", "rackPosition", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>ラック寸法差</summary>                  
            public static readonly ControlDefine RACK_SIZE_DIFF = new ControlDefine( "ntbRackSizeDiff", "ラック寸法差", "rackSizeDiff", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>ポンプネジ</summary>                    
            public static readonly ControlDefine PUMP_SCREW = new ControlDefine( "txtPumpScrew", "ポンプネジ", "pumpScrew", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>噴射時期補正値</summary>                
            public static readonly ControlDefine INJECTION_TIMING_ADJUST_VAL = new ControlDefine( "ntbInjectionTimingAdjustVal", "噴射時期補正値", "injectionTimingAdjustVal", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定パスフラグ</summary>                
            public static readonly ControlDefine MEASUREMENT_PATH_TYP = new ControlDefine( "txtMeasurementPathTyp", "測定パスフラグ", "measurementPathTyp", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>ガスケット対象フラグ</summary>          
            public static readonly ControlDefine GASKET_TYP = new ControlDefine( "txtGasketTyp", "ガスケット対象フラグ", "gasketTyp", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>標準ガスケット品番</summary>            
            public static readonly ControlDefine GASKET_NUM = new ControlDefine( "txtGasketNum", "標準ガスケット品番", "gasketNum", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>ピストン出代気筒1</summary>             
            public static readonly ControlDefine PISTON_01_BUMP = new ControlDefine( "ntbPiston01Bump", "ピストン出代気筒1", "piston01Bump", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>ピストン出代気筒2</summary>             
            public static readonly ControlDefine PISTON_02_BUMP = new ControlDefine( "ntbPiston02Bump", "ピストン出代気筒2", "piston02Bump", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>ピストン出代気筒3</summary>             
            public static readonly ControlDefine PISTON_03_BUMP = new ControlDefine( "ntbPiston03Bump", "ピストン出代気筒3", "piston03Bump", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>ピストン出代気筒4</summary>             
            public static readonly ControlDefine PISTON_04_BUMP = new ControlDefine( "ntbPiston04Bump", "ピストン出代気筒4", "piston04Bump", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>出代寸法誤差</summary>                  
            public static readonly ControlDefine BUMP_SIZE_VARIANCE = new ControlDefine( "ntbBumpSizeVariance", "出代寸法誤差", "bumpSizeVariance", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>トップクリアランス気筒1</summary>       
            public static readonly ControlDefine TOP_CLEARANCE_PISTON_01 = new ControlDefine( "ntbTopClearancePiston01", "トップクリアランス気筒1", "topClearancePiston01", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>トップクリアランス気筒2</summary>       
            public static readonly ControlDefine TOP_CLEARANCE_PISTON_02 = new ControlDefine( "ntbTopClearancePiston02", "トップクリアランス気筒2", "topClearancePiston02", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>トップクリアランス気筒3</summary>       
            public static readonly ControlDefine TOP_CLEARANCE_PISTON_03 = new ControlDefine( "ntbTopClearancePiston03", "トップクリアランス気筒3", "topClearancePiston03", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>トップクリアランス気筒4</summary>       
            public static readonly ControlDefine TOP_CLEARANCE_PISTON_04 = new ControlDefine( "ntbTopClearancePiston04", "トップクリアランス気筒4", "topClearancePiston04", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>燃料噴射時期気筒1</summary>             
            public static readonly ControlDefine INJECTION_TIMING_PISTON_01 = new ControlDefine( "ntbInjectionTimingPiston01", "燃料噴射時期気筒1", "injectionTimingPiston01", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>燃料噴射時期気筒2</summary>             
            public static readonly ControlDefine INJECTION_TIMING_PISTON_02 = new ControlDefine( "ntbInjectionTimingPiston02", "燃料噴射時期気筒2", "injectionTimingPiston02", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>燃料噴射時期気筒3</summary>             
            public static readonly ControlDefine INJECTION_TIMING_PISTON_03 = new ControlDefine( "ntbInjectionTimingPiston03", "燃料噴射時期気筒3", "injectionTimingPiston03", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>燃料噴射時期気筒4</summary>             
            public static readonly ControlDefine INJECTION_TIMING_PISTON_04 = new ControlDefine( "ntbInjectionTimingPiston04", "燃料噴射時期気筒4", "injectionTimingPiston04", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>シム枚数 0.175mm</summary>                   
            public static readonly ControlDefine SYM_QTY_175 = new ControlDefine( "ntbSymQty175", "シム枚数 0.175mm", "symQty175", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>シム枚数 0.2mm</summary>                     
            public static readonly ControlDefine SYM_QTY_200 = new ControlDefine( "ntbSymQty200", "シム枚数 0.2mm", "symQty200", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>シム枚数 0.25mm</summary>                    
            public static readonly ControlDefine SYM_QTY_250 = new ControlDefine( "ntbSymQty250", "シム枚数 0.25mm", "symQty250", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>シム枚数 0.3mm</summary>                     
            public static readonly ControlDefine SYM_QTY_300 = new ControlDefine( "ntbSymQty300", "シム枚数 0.3mm", "symQty300", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>シム枚数 0.35mm</summary>                    
            public static readonly ControlDefine SYM_QTY_350 = new ControlDefine( "ntbSymQty350", "シム枚数 0.35mm", "symQty350", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>総合判定</summary>                      
            public static readonly ControlDefine RESULT = new ControlDefine( "txtResult", "総合判定", "result", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>選定ガスケット部品番号</summary>        
            public static readonly ControlDefine SELECTED_GASKET_NUM = new ControlDefine( "txtSelectedGasketNum", "選定ガスケット部品番号", "selectedGasketNum", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>ガスケット寸法</summary>                
            public static readonly ControlDefine GASKET_SIZE = new ControlDefine( "ntbGasketSize", "ガスケット寸法", "gasketSize", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定点数</summary>                      
            public static readonly ControlDefine MEASUREMENT_QTY = new ControlDefine( "ntbMeasurementQty", "測定点数", "measurementQty", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>測定値生1気筒1</summary>                
            public static readonly ControlDefine MEASURE_PISTON_01_1 = new ControlDefine( "ntbMeasurePiston01_1", "測定値生1気筒1", "measurePiston01_1", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定値生1気筒2</summary>                
            public static readonly ControlDefine MEASURE_PISTON_01_2 = new ControlDefine( "ntbMeasurePiston01_2", "測定値生1気筒2", "measurePiston01_2", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定値生1気筒3</summary>                
            public static readonly ControlDefine MEASURE_PISTON_01_3 = new ControlDefine( "ntbMeasurePiston01_3", "測定値生1気筒3", "measurePiston01_3", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定値生1気筒4</summary>                
            public static readonly ControlDefine MEASURE_PISTON_01_4 = new ControlDefine( "ntbMeasurePiston01_4", "測定値生1気筒4", "measurePiston01_4", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定値生2気筒1</summary>                
            public static readonly ControlDefine MEASURE_PISTON_02_1 = new ControlDefine( "ntbMeasurePiston02_1", "測定値生2気筒1", "measurePiston02_1", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定値生2気筒2</summary>                
            public static readonly ControlDefine MEASURE_PISTON_02_2 = new ControlDefine( "ntbMeasurePiston02_2", "測定値生2気筒2", "measurePiston02_2", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定値生2気筒3</summary>                
            public static readonly ControlDefine MEASURE_PISTON_02_3 = new ControlDefine( "ntbMeasurePiston02_3", "測定値生2気筒3", "measurePiston02_3", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定値生2気筒4</summary>                
            public static readonly ControlDefine MEASURE_PISTON_02_4 = new ControlDefine( "ntbMeasurePiston02_4", "測定値生2気筒4", "measurePiston02_4", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定値生3気筒1</summary>                
            public static readonly ControlDefine MEASURE_PISTON_03_1 = new ControlDefine( "ntbMeasurePiston03_1", "測定値生3気筒1", "measurePiston03_1", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定値生3気筒2</summary>                
            public static readonly ControlDefine MEASURE_PISTON_03_2 = new ControlDefine( "ntbMeasurePiston03_2", "測定値生3気筒2", "measurePiston03_2", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定値生3気筒3</summary>                
            public static readonly ControlDefine MEASURE_PISTON_03_3 = new ControlDefine( "ntbMeasurePiston03_3", "測定値生3気筒3", "measurePiston03_3", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定値生3気筒4</summary>                
            public static readonly ControlDefine MEASURE_PISTON_03_4 = new ControlDefine( "ntbMeasurePiston03_4", "測定値生3気筒4", "measurePiston03_4", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定値生4気筒1</summary>                
            public static readonly ControlDefine MEASURE_PISTON_04_1 = new ControlDefine( "ntbMeasurePiston04_1", "測定値生4気筒1", "measurePiston04_1", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定値生4気筒2</summary>                
            public static readonly ControlDefine MEASURE_PISTON_04_2 = new ControlDefine( "ntbMeasurePiston04_2", "測定値生4気筒2", "measurePiston04_2", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定値生4気筒3</summary>                
            public static readonly ControlDefine MEASURE_PISTON_04_3 = new ControlDefine( "ntbMeasurePiston04_3", "測定値生4気筒3", "measurePiston04_3", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定値生4気筒4</summary>                
            public static readonly ControlDefine MEASURE_PISTON_04_4 = new ControlDefine( "ntbMeasurePiston04_4", "測定値生4気筒4", "measurePiston04_4", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>治具補正1</summary>                     
            public static readonly ControlDefine JIG_ADJUST_1 = new ControlDefine( "ntbJigAdjust1", "治具補正1", "jigAdjust1", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>治具補正2</summary>                     
            public static readonly ControlDefine JIG_ADJUST_2 = new ControlDefine( "ntbJigAdjust2", "治具補正2", "jigAdjust2", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>治具補正3</summary>                     
            public static readonly ControlDefine JIG_ADJUST_3 = new ControlDefine( "ntbJigAdjust3", "治具補正3", "jigAdjust3", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定補正値1気筒1</summary>              
            public static readonly ControlDefine ADJUST_PISTON_01_1 = new ControlDefine( "ntbAdjustPiston01_1", "測定補正値1気筒1", "adjustPiston01_1", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定補正値1気筒2</summary>              
            public static readonly ControlDefine ADJUST_PISTON_01_2 = new ControlDefine( "ntbAdjustPiston01_2", "測定補正値1気筒2", "adjustPiston01_2", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定補正値1気筒3</summary>              
            public static readonly ControlDefine ADJUST_PISTON_01_3 = new ControlDefine( "ntbAdjustPiston01_3", "測定補正値1気筒3", "adjustPiston01_3", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定補正値1気筒4</summary>              
            public static readonly ControlDefine ADJUST_PISTON_01_4 = new ControlDefine( "ntbAdjustPiston01_4", "測定補正値1気筒4", "adjustPiston01_4", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定補正値2気筒1</summary>              
            public static readonly ControlDefine ADJUST_PISTON_02_1 = new ControlDefine( "ntbAdjustPiston02_1", "測定補正値2気筒1", "adjustPiston02_1", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定補正値2気筒2</summary>              
            public static readonly ControlDefine ADJUST_PISTON_02_2 = new ControlDefine( "ntbAdjustPiston02_2", "測定補正値2気筒2", "adjustPiston02_2", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定補正値2気筒3</summary>              
            public static readonly ControlDefine ADJUST_PISTON_02_3 = new ControlDefine( "ntbAdjustPiston02_3", "測定補正値2気筒3", "adjustPiston02_3", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定補正値2気筒4</summary>              
            public static readonly ControlDefine ADJUST_PISTON_02_4 = new ControlDefine( "ntbAdjustPiston02_4", "測定補正値2気筒4", "adjustPiston02_4", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定補正値3気筒1</summary>              
            public static readonly ControlDefine ADJUST_PISTON_03_1 = new ControlDefine( "ntbAdjustPiston03_1", "測定補正値3気筒1", "adjustPiston03_1", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定補正値3気筒2</summary>              
            public static readonly ControlDefine ADJUST_PISTON_03_2 = new ControlDefine( "ntbAdjustPiston03_2", "測定補正値3気筒2", "adjustPiston03_2", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定補正値3気筒3</summary>              
            public static readonly ControlDefine ADJUST_PISTON_03_3 = new ControlDefine( "ntbAdjustPiston03_3", "測定補正値3気筒3", "adjustPiston03_3", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定補正値3気筒4</summary>              
            public static readonly ControlDefine ADJUST_PISTON_03_4 = new ControlDefine( "ntbAdjustPiston03_4", "測定補正値3気筒4", "adjustPiston03_4", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定補正値4気筒1</summary>              
            public static readonly ControlDefine ADJUST_PISTON_04_1 = new ControlDefine( "ntbAdjustPiston04_1", "測定補正値4気筒1", "adjustPiston04_1", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定補正値4気筒2</summary>              
            public static readonly ControlDefine ADJUST_PISTON_04_2 = new ControlDefine( "ntbAdjustPiston04_2", "測定補正値4気筒2", "adjustPiston04_2", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定補正値4気筒3</summary>              
            public static readonly ControlDefine ADJUST_PISTON_04_3 = new ControlDefine( "ntbAdjustPiston04_3", "測定補正値4気筒3", "adjustPiston04_3", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>測定補正値4気筒4</summary>              
            public static readonly ControlDefine ADJUST_PISTON_04_4 = new ControlDefine( "ntbAdjustPiston04_4", "測定補正値4気筒4", "adjustPiston04_4", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>出代平均値</summary>                    
            public static readonly ControlDefine BUMP_AVE_VAL = new ControlDefine( "ntbBumpAveVal", "出代平均値", "bumpAveVal", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>出代最大値</summary>                    
            public static readonly ControlDefine BUMP_MAX_VAL = new ControlDefine( "ntbBumpMaxVal", "出代最大値", "bumpMaxVal", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>出代最小値</summary>                    
            public static readonly ControlDefine BUMP_MIN_VAL = new ControlDefine( "ntbBumpMinVal", "出代最小値", "bumpMinVal", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>出代寸法バラツキ基準値</summary>        
            public static readonly ControlDefine BUMP_SIZE_BASE_VAL = new ControlDefine( "ntbBumpSizeBaseVal", "出代寸法バラツキ基準値", "bumpSizeBaseVal", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>エラーメッセージ1</summary>             
            public static readonly ControlDefine ERROR_MESSAGE_1 = new ControlDefine( "txtErrorMessage1", "エラーメッセージ1", "errorMessage1", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>エラーメッセージ2</summary>             
            public static readonly ControlDefine ERROR_MESSAGE_2 = new ControlDefine( "txtErrorMessage2", "エラーメッセージ2", "errorMessage2", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>エラーメッセージ3</summary>             
            public static readonly ControlDefine ERROR_MESSAGE_3 = new ControlDefine( "txtErrorMessage3", "エラーメッセージ3", "errorMessage3", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>測定号機</summary>                      
            public static readonly ControlDefine MEASURE_TERMINAL = new ControlDefine( "ntbMeasureTerminal", "測定号機", "measureTerminal", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>測定方法気筒</summary>                  
            public static readonly ControlDefine MEASURE_METHOD_PISTON = new ControlDefine( "txtMeasureMethodPiston", "測定方法気筒", "measureMethodPiston", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>測定方法方向</summary>                  
            public static readonly ControlDefine MEASURE_METHOD_DIRECTION = new ControlDefine( "txtMeasureMethodDirection", "測定方法方向", "measureMethodDirection", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>測定補正値</summary>                    
            public static readonly ControlDefine MEASURE_ADJUST_VAL = new ControlDefine( "ntbMeasureAdjustVal", "測定補正値", "measureAdjustVal", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>燃焼室リセス</summary>                  
            public static readonly ControlDefine COMBUSTION_RECESS = new ControlDefine( "txtCombustionRecess", "燃焼室リセス", "combustionRecess", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>ＦＩＴ補正値1気筒</summary>             
            public static readonly ControlDefine FIT_ADJUST_PISTON_01 = new ControlDefine( "ntbFitAdjustPiston01", "ＦＩＴ補正値1気筒", "fitAdjustPiston01", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>ＦＩＴ補正値2気筒</summary>             
            public static readonly ControlDefine FIT_ADJUST_PISTON_02 = new ControlDefine( "ntbFitAdjustPiston02", "ＦＩＴ補正値2気筒", "fitAdjustPiston02", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>ＦＩＴ補正値3気筒</summary>             
            public static readonly ControlDefine FIT_ADJUST_PISTON_03 = new ControlDefine( "ntbFitAdjustPiston03", "ＦＩＴ補正値3気筒", "fitAdjustPiston03", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>ＦＩＴ補正値4気筒</summary>             
            public static readonly ControlDefine FIT_ADJUST_PISTON_04 = new ControlDefine( "ntbFitAdjustPiston04", "ＦＩＴ補正値4気筒", "fitAdjustPiston04", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>ＦＩＴ補正値平均値</summary>            
            public static readonly ControlDefine FIT_ADJUST_AVE_VAL = new ControlDefine( "ntbFitAdjustAveVal", "ＦＩＴ補正値平均値", "fitAdjustAveVal", ControlDefine.BindType.Down, typeof( Decimal ) );
        }

        /// <summary>
        /// (サブリスト)コントロール定義
        /// </summary>
        public class GRID_SUB {
            //なし
        }
        #endregion

        #region プロパティ
        /// <summary>
        /// 表示中ページ(ベースフォーム)情報
        /// </summary>
        private BaseForm CurrentForm {
            get {
                return ( (BaseForm)Page );
            }
        }

        /// <summary>
        /// 表示中ユーザコントロール情報
        /// </summary>
        private PageInfo.ST_PAGE_INFO CurrentUCInfo {
            get {
                return PageInfo.GetUCPageInfo( DetailKeyParam.ProductKind, DetailKeyParam.GroupCd, DetailKeyParam.ClassCd, DetailKeyParam.AssemblyPatternCd );
            }
        }

        /// <summary>
        /// (メイン)コントロール定義
        /// </summary>
        ControlDefine[] _mainControls = null;
        /// <summary>
        /// (メイン)コントロール定義アクセサ
        /// </summary>
        ControlDefine[] MainControls {
            get {
                if ( true == ObjectUtils.IsNull( _mainControls ) ) {
                    _mainControls = ControlUtils.GetControlDefineArray( typeof( GRID_MAIN ) );
                }
                return _mainControls;
            }
        }

        /// <summary>
        /// (サブ)コントロール定義
        /// </summary>
        ControlDefine[] _subControls = null;
        /// <summary>
        /// (サブ)コントロール定義アクセサ
        /// </summary>
        ControlDefine[] SubControls {
            get {
                if ( true == ObjectUtils.IsNull( _subControls ) ) {
                    _subControls = ControlUtils.GetControlDefineArray( typeof( GRID_SUB ) );
                }
                return _subControls;
            }
        }

        /// <summary>
        /// 詳細表示キー情報
        /// </summary>
        private Defines.Interface.ST_DETAIL_PARAM _detailKeyParam;
        /// <summary>
        /// 詳細表示キー情報
        /// </summary>
        public Defines.Interface.ST_DETAIL_PARAM DetailKeyParam {
            get {
                return _detailKeyParam;
            }
            set {
                _detailKeyParam = value;
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
            CurrentForm.RaiseEvent( DoPageLoad );
        }

        /// <summary>
        /// メインリスト行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstMainList_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundMainList( sender, e );
        }

        #endregion

        #region イベントメソッド

        #region ページイベント
        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize() {
            //検索結果取得
            Business.DetailViewBusiness.ResultSet resultSet = new Business.DetailViewBusiness.ResultSet();
            try {
                resultSet = Business.DetailViewBusiness.SelectEngineFuelInjectionDetail( DetailKeyParam.AssemblyPatternCd, DetailKeyParam.ProductModelCd, DetailKeyParam.Serial );
            } catch ( DataAccessException ex ) {
                logger.Exception( ex );
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                return;
            } catch ( Exception ex ) {
                logger.Exception( ex );
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                return;
            } finally {
            }

            if ( 0 == resultSet.MainTable.Rows.Count ) {
                //検索結果0件
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62010, CurrentUCInfo.title );
                return;
            }

            InitializeValues( resultSet );
        }

        /// <summary>
        /// ページロード処理
        /// </summary>
        private void DoPageLoad() {
        }

        #endregion

        /// <summary>
        /// 初期値設定
        /// </summary>
        private void InitializeValues( Business.DetailViewBusiness.ResultSet resultSet ) {
            //来歴情報セット
            lstMainList.DataSource = resultSet.MainTable;
            lstMainList.DataBind();

            lstMainList.SelectedIndex = 0;
        }
        #endregion

        #region リストバインド
        /// <summary>
        /// メインリストバインド処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void ItemDataBoundMainList( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {
                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                //コントロールへの自動データバインド
                CurrentForm.SetControlValues( e.Item, MainControls, rowBind, ref dicSetValues );

                //コントロールへの独自データバインド
                //計測日時加工/セット
                KTTextBox txtInspectionDt = ( (KTTextBox)e.Item.FindControl( GRID_MAIN.INSPECTION_DT.controlId ) );
                txtInspectionDt.Value = DateUtils.ToString( rowBind[GRID_MAIN.INSPECTION_DT.bindField], DateUtils.FormatType.Second );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );
            }
        }
        #endregion

    }
}