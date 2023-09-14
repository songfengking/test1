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
using KTFramework.C1Common.Excel;
using TRC_W_PWT_ProductView.Dao.Process;

namespace TRC_W_PWT_ProductView.UI.Pages.ProcessView
{

    /// <summary>
    /// (詳細 トラクタ 工程) トラクタ走行検査
    /// </summary>
    public partial class TractorAll : System.Web.UI.UserControl, Defines.Interface.IDetail
    {

        //ロガー定義
        private static readonly Logger logger = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 管理ID
        /// </summary>
        const string MANAGE_ID = Defines.Session.DetailFrame.SESSION_PAGE_INFO_DETAIL_KEY;//CurrentUCInfo.pageId

        /// <summary>
        /// ListView選択行制御
        /// </summary>
        const string MAIN_VIEW_SELECTED = "TractorAll.SelectMainViewRow(this,{0},'{1}');";

        /// <summary>
        /// DB取得データ格納先 ディクショナリキー
        /// </summary>
        private string SESSION_PAGE_INFO_DB_KEY = "bindTableData";

        #region 定数定義

        /// <summary>
        /// (メインリスト)コントロール定義
        /// </summary>
        public class GRID_MAIN
        {
            /// <summary>TR</summary>
            public static readonly ControlDefine TR_ROW_DATA = new ControlDefine("trRowData", "TR", "", ControlDefine.BindType.None, typeof(String));
            /// <summary>(SELECTコマンド送信用ボタン)</summary>
            public static readonly ControlDefine SELECT = new ControlDefine("btnSelect", "BUTTON", "", ControlDefine.BindType.None, typeof(String));
            /// <summary>型式コード</summary>
            public static readonly ControlDefine MODEL_CD = new ControlDefine("txtModelCd", "型式コード", "modelCd", ControlDefine.BindType.Down, typeof(String));
            /// <summary>国コード</summary>
            public static readonly ControlDefine COUNTRY_CD = new ControlDefine("txtCountryCd", "国コード", "countryCd", ControlDefine.BindType.Down, typeof(String));
            /// <summary>機番</summary>
            public static readonly ControlDefine SERIAL_NUMBER = new ControlDefine("txtSerialNumber", "機番", "serialNumber", ControlDefine.BindType.Down, typeof(String));
            /// <summary>検査連番</summary>
            public static readonly ControlDefine INSPECTION_SEQ = new ControlDefine("txtInspectionSeq", "検査連番", "inspectionSeq", ControlDefine.BindType.Down, typeof(String));
            /// <summary>IDNO</summary>
            public static readonly ControlDefine IDNO = new ControlDefine("txtIdno", "IDNO", "idno", ControlDefine.BindType.Down, typeof(String));
            /// <summary>月度</summary>
            public static readonly ControlDefine MONTH = new ControlDefine("txtMonth", "月度", "month", ControlDefine.BindType.Down, typeof(String));
            /// <summary>月連</summary>
            public static readonly ControlDefine SEQUENCE_NUM = new ControlDefine("txtSeqenceNum", "月連", "seqenceNum", ControlDefine.BindType.Down, typeof(String));
            /// <summary>PIN</summary>
            public static readonly ControlDefine PIN_CD = new ControlDefine("txtPinCd", "PINコード", "pinCd", ControlDefine.BindType.Down, typeof(String));
            /// <summary>全検査完了フラグ</summary>
            public static readonly ControlDefine RESULT = new ControlDefine("txtResult", "全検査完了フラグ", "result", ControlDefine.BindType.Down, typeof(String));
            /// <summary>最終検査従業員番号_検査員</summary>
            public static readonly ControlDefine INSPECTION_EMPL_CD = new ControlDefine("txtInspectionEmplCd", "最終検査従業員番号_検査員", "inspectionEmplCd", ControlDefine.BindType.Down, typeof(String));
            /// <summary>最終検査終了時刻</summary>
            public static readonly ControlDefine INSPECTION_DT = new ControlDefine("txtInspectionDt", "最終検査終了時刻", "inspectionDt", ControlDefine.BindType.Down, typeof(DateTime));
            /// <summary>検査グループ</summary>
            public static readonly ControlDefine INSPECTION_GROUP = new ControlDefine("txtInspectionGroup", "検査グループ", "inspectionGroup", ControlDefine.BindType.Down, typeof(String));
            /// <summary>2WD4WD</summary>
            public static readonly ControlDefine WHEEL_DRIVE = new ControlDefine("txtWheelDrive", "2WD4WD", "wheelDrive", ControlDefine.BindType.Down, typeof(String));
            /// <summary>固縛フック_前</summary>
            public static readonly ControlDefine LASHING_HOOK_F = new ControlDefine("txtLashingHookF", "固縛フック_前", "lashingHookF", ControlDefine.BindType.Down, typeof(String));
            /// <summary>固縛フック_後</summary>
            public static readonly ControlDefine LASHING_HOOK_R = new ControlDefine("txtLashingHookR", "固縛フック_後", "lashingHookR", ControlDefine.BindType.Down, typeof(String));
            /// <summary>治具タイヤ係数_共通_前</summary>
            public static readonly ControlDefine JIG_COM_F = new ControlDefine("txtJigComF", "治具タイヤ係数_共通_前", "jigComF", ControlDefine.BindType.Down, typeof(String));
            /// <summary>治具タイヤ係数_共通_後</summary>
            public static readonly ControlDefine JIG_COM_R = new ControlDefine("txtJigComR", "治具タイヤ係数_共通_後", "jigComR", ControlDefine.BindType.Down, typeof(String));
            /// <summary>最高速検査フラグ</summary>
            public static readonly ControlDefine MAX_SPEED_FLAG = new ControlDefine("txtMaxSpeedFlag", "最高速検査フラグ", "maxSpeedFlag", ControlDefine.BindType.Down, typeof(String));
            /// <summary>最高速度_前_下限</summary>
            public static readonly ControlDefine FRONT_WHEEL_MIN = new ControlDefine("txtFrontWheelMin", "最高速度_前_下限", "frontWheelMin", ControlDefine.BindType.Down, typeof(String));
            /// <summary>最高速度_前_上限</summary>
            public static readonly ControlDefine FRONT_WHEEL_MAX = new ControlDefine("txtFrontWheelMax", "最高速度_前_上限", "frontWheelMax", ControlDefine.BindType.Down, typeof(String));
            /// <summary>最高速度_後_下限</summary>
            public static readonly ControlDefine REAR_WHEEL_MIN = new ControlDefine("txtRearWheelMin", "最高速度_後_下限", "rearWheelMin", ControlDefine.BindType.Down, typeof(String));
            /// <summary>最高速度_後_上限</summary>
            public static readonly ControlDefine REAR_WHEEL_MAX = new ControlDefine("txtRearWheelMax", "最高速度_後_上限", "rearWheelMax", ControlDefine.BindType.Down, typeof(String));
            /// <summary>最高速度到達後計測時間</summary>
            public static readonly ControlDefine RANGE_HOLD_TIME = new ControlDefine("txtRangeHoldTime", "最高速度到達後計測時間", "rangeHoldTime", ControlDefine.BindType.Down, typeof(String));
            /// <summary>最高速度範囲外打ち切り時間</summary>
            public static readonly ControlDefine CLOSE_TIME = new ControlDefine("txtCloseTime", "最高速度範囲外打ち切り時間", "closeTime", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果 最高速検査フラグ しない</summary>
            public static readonly ControlDefine RT_MAX_SPEED_FLAG = new ControlDefine("txtRtMaxSpeedFlag", "結果 最高速検査フラグ", "rtMaxSpeedFlag", ControlDefine.BindType.Down, typeof(String));
            /// <summary>最高速検査従業員番号_検査員</summary>
            public static readonly ControlDefine RT_MAX_SPEED_EMPL_CD = new ControlDefine("txtRtMaxSpeedemplCd", "最高速検査従業員番号_検査員", "rtMaxSpeedemplCd", ControlDefine.BindType.Down, typeof(String));
            /// <summary>最高速検査終了時刻</summary>
            public static readonly ControlDefine RT_MAX_SPEED_END_TIME = new ControlDefine("txtRtMaxSpeedEndTime", "最高速検査終了時刻", "rtMaxSpeedEndTime", ControlDefine.BindType.Down, typeof(DateTime));
            /// <summary>結果_最高速度_前_左_直値</summary>
            public static readonly ControlDefine RT_FRONT_WHEEL_L = new ControlDefine("txtRtFrontWheelL", "結果_最高速度_前_左_直値", "rtFrontWheelL", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_最高速度_前_右_直値</summary>
            public static readonly ControlDefine RT_FRONT_WHEEL_R = new ControlDefine("txtRtFrontWheelR", "結果_最高速度_前_右_直値", "rtFrontWheelR", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_最高速度_後_左_直値</summary>
            public static readonly ControlDefine RT_REAR_WHEEL_L = new ControlDefine("txtRtRearWheelL", "結果_最高速度_後_左_直値", "rtRearWheelL", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_最高速度_後_右_直値</summary>
            public static readonly ControlDefine RT_REAR_WHEEL_R = new ControlDefine("txtRtRearWheelR", "結果_最高速度_後_右_直値", "rtRearWheelR", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_最高速度_前_左_補正値</summary>
            public static readonly ControlDefine RT_FRONT_WHEEL_L_CV = new ControlDefine("txtRtFrontWheelLCv", "結果_最高速度_前_左_補正値", "rtFrontWheelLCv", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_最高速度_前_右_補正値</summary>
            public static readonly ControlDefine RT_FRONT_WHEEL_R_CV = new ControlDefine("txtRtFrontWheelRCv", "結果_最高速度_前_右_補正値", "rtFrontWheelRCv", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_最高速度_後_左_補正値</summary>
            public static readonly ControlDefine RT_REAR_WHEEL_L_CV = new ControlDefine("txtRtRearWheelLCv", "結果_最高速度_後_左_補正値", "rtRearWheelLCv", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_最高速度_後_右_補正値</summary>
            public static readonly ControlDefine RT_REAR_WHEEL_R_CV = new ControlDefine("txtRtRearWheelRCv", "結果_最高速度_後_右_補正値", "rtRearWheelRCv", ControlDefine.BindType.Down, typeof(String));
            /// <summary>騒音検査フラグ</summary>
            public static readonly ControlDefine NOISE_FLAG = new ControlDefine("txtNoiseFlag", "騒音検査フラグ", "noiseFlag", ControlDefine.BindType.Down, typeof(String));
            /// <summary>副変速</summary>
            public static readonly ControlDefine SUB_TRANSMISSION = new ControlDefine("txtSubTransmission", "副変速", "subTransmission", ControlDefine.BindType.Down, typeof(String));
            /// <summary>主変速</summary>
            public static readonly ControlDefine MAIN_TRANSMISSION = new ControlDefine("txtMainTransmission", "主変速", "mainTransmission", ControlDefine.BindType.Down, typeof(String));
            /// <summary>騒音速度_前輪_下限</summary>
            public static readonly ControlDefine NOISE_FRONT_WHEEL_MIN = new ControlDefine("txtNoiseFrontWheelMin", "騒音速度_前輪_下限", "noiseFrontWheelMin", ControlDefine.BindType.Down, typeof(String));
            /// <summary>騒音速度_前輪_上限</summary>
            public static readonly ControlDefine NOISE_FRONT_WHEEL_MAX = new ControlDefine("txtNoiseFrontWheelMax", "騒音速度_前輪_上限", "noiseFrontWheelMax", ControlDefine.BindType.Down, typeof(String));
            /// <summary>騒音速度_後輪_下限</summary>
            public static readonly ControlDefine NOISE_REAR_WHEEL_MIN = new ControlDefine("txtNoiseRearWheelMin", "騒音速度_後輪_下限", "noiseRearWheelMin", ControlDefine.BindType.Down, typeof(String));
            /// <summary>騒音速度_後輪_上限</summary>
            public static readonly ControlDefine NOISE_REAR_WHEEL_MAX = new ControlDefine("txtNoiseRearWheelMax", "騒音速度_後輪_上限", "noiseRearWheelMax", ControlDefine.BindType.Down, typeof(String));
            /// <summary>騒音速度到達後計測時間</summary>
            public static readonly ControlDefine NOISE_RANGE_HOLD_TIME = new ControlDefine("txtNoiseRangeHoldTime", "騒音速度到達後計測時間", "noiseRangeHoldTime", ControlDefine.BindType.Down, typeof(String));
            /// <summary>騒音速度範囲外打ち切り時間</summary>
            public static readonly ControlDefine NOISE_CLOSE_TIME = new ControlDefine("txtNoiseCloseTime", "騒音速度範囲外打ち切り時間", "noiseCloseTime", ControlDefine.BindType.Down, typeof(String));
            /// <summary>騒音判定閾値_上限</summary>
            public static readonly ControlDefine NOISE_THRESHOLD = new ControlDefine("txtNoiseThreshold", "騒音判定閾値_上限", "noiseThreshold", ControlDefine.BindType.Down, typeof(String));
            /// <summary>騒音判定閾値_下限</summary>
            public static readonly ControlDefine NOISE_THRESHOLD_MIN = new ControlDefine( "txtNoiseThresholdMin", "騒音判定閾値_下限", "noiseThresholdMin", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>結果 騒音検査フラグ</summary>
            public static readonly ControlDefine RT_NOISE_FLAG = new ControlDefine("txtRtNoiseFlag", "結果 騒音検査フラグ", "rtNoiseFlag", ControlDefine.BindType.Down, typeof(String));
            /// <summary>騒音検査従業員番号_検査員</summary>
            public static readonly ControlDefine RT_NOISE_EMPL_CD = new ControlDefine("txtRtNoiseEmplCd", "騒音検査従業員番号_検査員", "rtNoiseEmplCd", ControlDefine.BindType.Down, typeof(String));
            /// <summary>騒音検査終了時刻</summary>
            public static readonly ControlDefine RT_NOISE_END_TIME = new ControlDefine("txtRtNoiseEndTime", "騒音検査終了時刻", "rtNoiseEndTime", ControlDefine.BindType.Down, typeof(DateTime));
            /// <summary>結果_騒音_前_左_直値</summary>
            public static readonly ControlDefine RT_NOISE_FRONT_WHEEL_L = new ControlDefine("txtRtNoiseFrontWheelL", "結果_騒音_前_左_直値", "rtNoiseFrontWheelL", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_騒音_前_右_直値</summary>
            public static readonly ControlDefine RT_NOISE_FRONT_WHEEL_R = new ControlDefine("txtRtNoiseFrontWheelR", "結果_騒音_前_右_直値", "rtNoiseFrontWheelR", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_騒音_後_左_直値</summary>
            public static readonly ControlDefine RT_NOISE_REAR_WHEEL_L = new ControlDefine("txtRtNoiseRearWheelL", "結果_騒音_後_左_直値", "rtNoiseRearWheelL", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_騒音_後_右_直値</summary>
            public static readonly ControlDefine RT_NOISE_REAR_WHEEL_R = new ControlDefine("txtRtNoiseRearWheelR", "結果_騒音_後_右_直値", "rtNoiseRearWheelR", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_騒音_前_左_補正値</summary>
            public static readonly ControlDefine RT_NOISE_FRONT_WHEEL_L_CV = new ControlDefine("txtRtNoiseFrontWheelLCv", "結果_騒音_前_左_補正値", "rtNoiseFrontWheelLCv", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_騒音_前_右_補正値</summary>
            public static readonly ControlDefine RT_NOISE_FRONT_WHEEL_R_CV = new ControlDefine("txtRtNoiseFrontWheelRCv", "結果_騒音_前_右_補正値", "rtNoiseFrontWheelRCv", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_騒音_後_左_補正値</summary>
            public static readonly ControlDefine RT_NOISE_REAR_WHEEL_L_CV = new ControlDefine("txtRtNoiseRearWheelLCv", "結果_騒音_後_左_補正値", "rtNoiseRearWheelLCv", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_騒音_後_右_補正値</summary>
            public static readonly ControlDefine RT_NOISE_REAR_WHEEL_R_CV = new ControlDefine("txtRtNoiseRearWheelRCv", "結果_騒音_後_右_補正値", "rtNoiseRearWheelRCv", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_最大騒音値</summary>
            public static readonly ControlDefine RT_NOISE_MAX = new ControlDefine("txtRtNoiseMax", "結果_最大騒音値", "rtNoiseMax", ControlDefine.BindType.Down, typeof(String));
            /// <summary>異音検査フラグ</summary>
            public static readonly ControlDefine ABNORMAL_NOISE_FLAG = new ControlDefine("txtAbnormalNoiseFlag", "異音検査フラグ", "abnormalNoiseFlag", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果 異音検査フラグ</summary>
            public static readonly ControlDefine RT_ABNORMAL_NOISE_FLAG = new ControlDefine("txtRtAbnormalNoiseFlag", "結果 異音検査フラグ", "rtAbnormalNoiseFlag", ControlDefine.BindType.Down, typeof(String));
            /// <summary>異音検査従業員番号_検査員</summary>
            public static readonly ControlDefine RT_ABNORMAL_NOISE_EMPL_CD = new ControlDefine("txtRtAbnormalNoiseEmplCd", "異音検査従業員番号_検査員", "rtAbnormalNoiseEmplCd", ControlDefine.BindType.Down, typeof(String));
            /// <summary>異音検査終了時刻</summary>
            public static readonly ControlDefine RT_ABNORMAL_NOISE_END_TIME = new ControlDefine("txtRtAbnormalNoiseEndTime", "異音検査終了時刻", "rtAbnormalNoiseEndTime", ControlDefine.BindType.Down, typeof(DateTime));
            /// <summary>最大許容重量</summary>
            public static readonly ControlDefine MAX_ALLOW_WEIGHT = new ControlDefine("txtMaxAllowWeight", "最大許容重量", "maxAllowWeight", ControlDefine.BindType.Down, typeof(String));
            /// <summary>ブレーキ検査打ち切り時間</summary>
            public static readonly ControlDefine BRAKING_COM_CLOSE_TIME = new ControlDefine("txtBrakingComCloseTime", "ブレーキ検査打ち切り時間", "brakingComCloseTime", ControlDefine.BindType.Down, typeof(String));
            /// <summary>左右制動力差NG閾値</summary>
            public static readonly ControlDefine BRAKING_FORCE_LRDIFF = new ControlDefine("txtBrakingForceLRDiff", "左右制動力差NG閾値", "brakingForceLRDiff", ControlDefine.BindType.Down, typeof(String));
            /// <summary>主ブレーキ検査フラグ</summary>
            public static readonly ControlDefine BRAKING_B_FLAG = new ControlDefine("txtBrakingBFlag", "主ブレーキ検査フラグ", "brakingBFlag", ControlDefine.BindType.Down, typeof(String));
            /// <summary>踏力_下限</summary>
            public static readonly ControlDefine BRAKING_B_FORCE_MIN = new ControlDefine("txtBrakingBForceMin", "踏力_下限", "brakingBForceMin", ControlDefine.BindType.Down, typeof(String));
            /// <summary>踏力_上限</summary>
            public static readonly ControlDefine BRAKING_B_FORCE_MAX = new ControlDefine("txtBrakingBForceMax", "踏力_上限", "brakingBForceMax", ControlDefine.BindType.Down, typeof(String));
            /// <summary>主ブレーキ判定閾値_下限</summary>
            public static readonly ControlDefine BRAKING_B_THRESHOLD = new ControlDefine("txtBrakingBThreshold", "主ブレーキ判定閾値_下限", "brakingBThreshold", ControlDefine.BindType.Down, typeof(String));
            /// <summary>主ブレーキ判定閾値_上限</summary>
            public static readonly ControlDefine BRAKING_B_THRESHOLD_MAX = new ControlDefine( "txtBrakingBThresholdMax", "主ブレーキ判定閾値_上限", "brakingBThresholdMax", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>駐車ブレーキ検査フラグ</summary>
            public static readonly ControlDefine BRAKING_P_FLAG = new ControlDefine("txtBrakingPFlag", "駐車ブレーキ検査フラグ", "brakingPFlag", ControlDefine.BindType.Down, typeof(String));
            /// <summary>駐車ブレーキ判定閾値_下限</summary>
            public static readonly ControlDefine BRAKING_P_THRESHOLD = new ControlDefine("txtBrakingPThreshold", "駐車ブレーキ判定閾値_下限", "brakingPThreshold", ControlDefine.BindType.Down, typeof(String));
            /// <summary>駐車ブレーキ判定閾値_上限</summary>
            public static readonly ControlDefine BRAKING_P_THRESHOLD_MAX = new ControlDefine( "txtBrakingPThresholdMax", "駐車ブレーキ判定閾値_上限", "brakingPThresholdMax", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>非常ブレーキ検査フラグ</summary>
            public static readonly ControlDefine BRAKING_E_FLAG = new ControlDefine("txtBrakingEFlag", "非常ブレーキ検査フラグ", "brakingEFlag", ControlDefine.BindType.Down, typeof(String));
            /// <summary>非常ブレーキ判定閾値_下限</summary>
            public static readonly ControlDefine BRAKING_E_THRESHOLD = new ControlDefine("txtBrakingEThreshold", "非常ブレーキ判定閾値_下限", "brakingEThreshold", ControlDefine.BindType.Down, typeof(String));
            /// <summary>非常ブレーキ判定閾値_上限</summary>
            public static readonly ControlDefine BRAKING_E_THRESHOLD_MAX = new ControlDefine( "txtBrakingEThresholdMax", "非常ブレーキ判定閾値_上限", "brakingEThresholdMax", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>結果_主B検査フラグ</summary>
            public static readonly ControlDefine RT_BRK_B_FLAG = new ControlDefine("txtRtBrkBFlag", "結果_主B検査フラグ", "rtBrkBFlag", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_主B検査従業員番号_検査員</summary>
            public static readonly ControlDefine RT_BRK_B_EMPL_CD = new ControlDefine("txtRtBrkBEmplCd", "結果_主B検査従業員番号_検査員", "rtBrkBEmplCd", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_主B検査終了時刻</summary>
            public static readonly ControlDefine RT_BRK_B_END_TIME = new ControlDefine("txtRtBrkBEndTime", "結果_主B検査終了時刻", "rtBrkBEndTime", ControlDefine.BindType.Down, typeof(DateTime));
            /// <summary>結果_主B最大制動力_後_左_直値</summary>
            public static readonly ControlDefine RT_BRK_B_MAX_FORCE_REAR_L = new ControlDefine("txtRtBrkBMaxForceRearL", "結果_主B最大制動力_後_左_直値", "rtBrkBMaxForceRearL", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_主B最大制動力_後_右_直値</summary>
            public static readonly ControlDefine RT_BRK_B_MAX_FORCE_REAR_R = new ControlDefine("txtRtBrkBMaxForceRearR", "結果_主B最大制動力_後_右_直値", "rtBrkBMaxForceRearR", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_主B最大制動力_後_左_補正値</summary>
            public static readonly ControlDefine RT_BRK_B_MAX_FORCE_REAR_L_CV = new ControlDefine("txtRtBrkBMaxForceRearLCv", "結果_主B最大制動力_後_左_補正値", "rtBrkBMaxForceRearLCv", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_主B最大制動力_後_右_補正値</summary>
            public static readonly ControlDefine RT_BRK_B_MAX_FORCE_REAR_R_CV = new ControlDefine("txtRtBrkBMaxForceRearRCv", "結果_主B最大制動力_後_右_補正値", "rtBrkBMaxForceRearRCv", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_主B最大制動力_踏力</summary>
            public static readonly ControlDefine RT_BRK_B_MAX_PEDAL_FORCE_MAX = new ControlDefine("txtRtBrkBMaxPedalForceMax", "結果_主B最大制動力_踏力", "rtBrkBMaxPedalForceMax", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_駐B検査フラグ</summary>
            public static readonly ControlDefine RT_BRK_P_FLAG = new ControlDefine("txtRtBrkPFlag", "結果_駐B検査フラグ", "rtBrkPFlag", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_駐B検査従業員番号_検査員</summary>
            public static readonly ControlDefine RT_BRK_P_EMPL_CD = new ControlDefine("txtRtBrkPEmplCd", "結果_駐B検査従業員番号_検査員", "rtBrkPEmplCd", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_駐B検査終了時刻</summary>
            public static readonly ControlDefine RT_BRK_P_END_TIME = new ControlDefine("txtRtBrkPEndTime", "結果_駐B検査終了時刻", "rtBrkPEndTime", ControlDefine.BindType.Down, typeof(DateTime));
            /// <summary>結果_駐B最大制動力_後_左_直値</summary>
            public static readonly ControlDefine RT_BRK_P_MAX_FORCE_REAR_L = new ControlDefine("txtRtBrkPMaxForceRearL", "結果_駐B最大制動力_後_左_直値", "rtBrkPMaxForceRearL", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_駐B最大制動力_後_右_直値</summary>
            public static readonly ControlDefine RT_BRK_P_MAX_FORCE_REAR_R = new ControlDefine("txtRtBrkPMaxForceRearR", "結果_駐B最大制動力_後_右_直値", "rtBrkPMaxForceRearR", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_駐B最大制動力_後_左_補正値</summary>
            public static readonly ControlDefine RT_BRK_P_MAX_FORCE_REAR_L_CV = new ControlDefine("txtRtBrkPMaxForceRearLCv", "結果_駐B最大制動力_後_左_補正値", "rtBrkPMaxForceRearLCv", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_駐B最大制動力_後_右_補正値</summary>
            public static readonly ControlDefine RT_BRK_P_MAX_FORCE_REAR_R_CV = new ControlDefine("txtRtBrkPMaxForceRearRCv", "結果_駐B最大制動力_後_右_補正値", "rtBrkPMaxForceRearRCv", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_非B検査フラグ</summary>
            public static readonly ControlDefine RT_BRK_E_FLAG = new ControlDefine("txtRtBrkEFlag", "結果_非B検査フラグ", "rtBrkEFlag", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_非B検査従業員番号_検査員</summary>
            public static readonly ControlDefine RT_BRK_E_EMPL_CD = new ControlDefine("txtRtBrkEEmplCd", "結果_非B検査従業員番号_検査員", "rtBrkEEmplCd", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_非B検査終了時刻</summary>
            public static readonly ControlDefine RT_BRK_E_END_TIME = new ControlDefine("txtRtBrkEEndTime", "結果_非B検査終了時刻", "rtBrkEEndTime", ControlDefine.BindType.Down, typeof(DateTime));
            /// <summary>結果_非B最大制動力_後_左_直値</summary>
            public static readonly ControlDefine RT_BRK_E_MAX_FORCE_REAR_L = new ControlDefine("txtRtBrkEMaxForceRearL", "結果_非B最大制動力_後_左_直値", "rtBrkEMaxForceRearL", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_非B最大制動力_後_右_直値</summary>
            public static readonly ControlDefine RT_BRK_E_MAX_FORCE_REAR_R = new ControlDefine("txtRtBrkEMaxForceRearR", "結果_非B最大制動力_後_右_直値", "rtBrkEMaxForceRearR", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_非B最大制動力_後_左_補正値</summary>
            public static readonly ControlDefine RT_BRK_E_MAX_FORCE_REAR_L_CV = new ControlDefine("txtRtBrkEMaxForceRearLCv", "結果_非B最大制動力_後_左_補正値", "rtBrkEMaxForceRearLCv", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_非B最大制動力_後_右_補正値</summary>
            public static readonly ControlDefine RT_BRK_E_MAX_FORCE_REAR_R_CV = new ControlDefine("txtRtBrkEMaxForceRearRCv", "結果_非B最大制動力_後_右_補正値", "rtBrkEMaxForceRearRCv", ControlDefine.BindType.Down, typeof(String));
            /// <summary>モンロー検査フラグ</summary>
            public static readonly ControlDefine MONROE_FLAG = new ControlDefine("txtMonroeFlag", "モンロー検査フラグ", "monroeFlag", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果 モンロー検査フラグ</summary>
            public static readonly ControlDefine RT_MONROE_FLAG = new ControlDefine("txtRtMonroeFlag", "結果 モンロー検査フラグ", "rtMonroeFlag", ControlDefine.BindType.Down, typeof(String));
            /// <summary>モンロー検査従業員番号_検査員</summary>
            public static readonly ControlDefine RT_MONROE_EMPL_CD = new ControlDefine("txtRtMonroeEmplCd", "モンロー検査従業員番号_検査員", "rtMonroeEmplCd", ControlDefine.BindType.Down, typeof(String));
            /// <summary>モンロー検査終了時刻</summary>
            public static readonly ControlDefine RT_MONROE_END_TIME = new ControlDefine("txtRtMonroeEndTime", "モンロー検査終了時刻", "rtMonroeEndTime", ControlDefine.BindType.Down, typeof(DateTime));
            /// <summary>PTO検査フラグ</summary>
            public static readonly ControlDefine PTO_FLAG = new ControlDefine("txtPtoFlag", "PTO検査フラグ", "ptoFlag", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果 PTO検査フラグ</summary>
            public static readonly ControlDefine RT_PTO_FLAG = new ControlDefine("txtRtPtoFlag", "結果 PTO検査フラグ", "rtPtoFlag", ControlDefine.BindType.Down, typeof(String));
            /// <summary>PTO検査従業員番号_検査員</summary>
            public static readonly ControlDefine RT_PTO_EMPL_CD = new ControlDefine("txtRtPtoEmplCd", "PTO検査従業員番号_検査員", "rtPtoEmplCd", ControlDefine.BindType.Down, typeof(String));
            /// <summary>PTO検査終了時刻</summary>
            public static readonly ControlDefine RT_PTO_END_TIME = new ControlDefine("txtRtPtoEndTime", "PTO検査終了時刻", "rtPtoEndTime", ControlDefine.BindType.Down, typeof(DateTime));
            /// <summary>油圧検査フラグ</summary>
            public static readonly ControlDefine HYDRAULIC_FLAG = new ControlDefine("txtHydraulicFlag", "油圧検査フラグ", "hydraulicFlag", ControlDefine.BindType.Down, typeof(String));
            /// <summary>油圧検査用錘指示</summary>
            public static readonly ControlDefine HYDRAULIC_WEIGHT = new ControlDefine("txtHydraulicWeight", "油圧検査用錘指示", "hydraulicWeight", ControlDefine.BindType.Down, typeof(String));
            /// <summary>油圧検査用スリング指示</summary>
            public static readonly ControlDefine HYDRAULIC_SLING = new ControlDefine( "txtHydraulicSling", "油圧検査用スリング指示", "hydraulicSling", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>油圧検査用スリング指示</summary>
            public static readonly ControlDefine HYDRAULIC_LOWER_LINK = new ControlDefine( "txtHydraulicLowerLink", "油圧検査用ロアリンクバー", "hydraulicLowerLink", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>結果 油圧検査フラグ</summary>
            public static readonly ControlDefine RT_HYDRAULIC_FLAG = new ControlDefine("txtRtHydraulicFlag", "結果 油圧検査フラグ", "rtHydraulicFlag", ControlDefine.BindType.Down, typeof(String));
            /// <summary>油圧検査従業員番号_検査員</summary>
            public static readonly ControlDefine RT_HYDRAULIC_EMPL_CD = new ControlDefine("txtRtHydraulicEmplCd", "油圧検査従業員番号_検査員", "rtHydraulicEmplCd", ControlDefine.BindType.Down, typeof(String));
            /// <summary>油圧検査終了時刻</summary>
            public static readonly ControlDefine RT_HYDRAULIC_END_TIME = new ControlDefine("txtRtHydraulicEndTime", "油圧検査終了時刻", "rtHydraulicEndTime", ControlDefine.BindType.Down, typeof(DateTime));
            /// <summary>ライト検査フラグ</summary>
            public static readonly ControlDefine HEADLIGHT_FLAG = new ControlDefine("txtHeadlightFlag", "ライト検査フラグ", "headlightFlag", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果 ライト検査フラグ</summary>
            public static readonly ControlDefine RT_HEADLIGHT_FLAG = new ControlDefine("txtRtHeadlightFlag", "結果 ライト検査フラグ", "rtHeadlightFlag", ControlDefine.BindType.Down, typeof(String));
            /// <summary>ライト検査従業員番号_検査員</summary>
            public static readonly ControlDefine RT_HEADLIGHT_EMPL_CD = new ControlDefine("txtRtHeadlightEmplCd", "ライト検査従業員番号_検査員", "rtHeadlightEmplCd", ControlDefine.BindType.Down, typeof(String));
            /// <summary>ライト検査終了時刻</summary>
            public static readonly ControlDefine RT_HEADLIGHT_END_TIME = new ControlDefine("txtRtHeadlightEndTime", "ライト検査終了時刻", "rtHeadlightEndTime", ControlDefine.BindType.Down, typeof(DateTime));
            /// <summary>キーストップ検査フラグ</summary>
            public static readonly ControlDefine KEY_STOP_FLAG = new ControlDefine("txtKeyStopFlag", "キーストップ検査フラグ", "keyStopFlag", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果 キーストップ検査フラグ</summary>
            public static readonly ControlDefine RT_KEY_STOP_FLAG = new ControlDefine("txtRtKeyStopFlag", "結果 キーストップ検査フラグ", "rtKeyStopFlag", ControlDefine.BindType.Down, typeof(String));
            /// <summary>キーストップ検査従業員番号_検査員</summary>
            public static readonly ControlDefine RT_KEY_STOP_EMPL_CD = new ControlDefine("txtRtKeyStopEmplCd", "キーストップ検査従業員番号_検査員", "rtKeyStopEmplCd", ControlDefine.BindType.Down, typeof(String));
            /// <summary>キーストップ検査終了時刻</summary>
            public static readonly ControlDefine RT_KEY_STOP_END_TIME = new ControlDefine("txtRtKeyStopEmplEndTime", "キーストップ検査終了時刻", "rtKeyStopEmplEndTime", ControlDefine.BindType.Down, typeof(DateTime));
            /// <summary>ハンドル締付検査フラグ</summary>
            public static readonly ControlDefine STEERING_TIGHTEN_FLAG = new ControlDefine("txtSteeringTightenFlag", "ハンドル締付検査フラグ", "steeringTightenFlag", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果 ハンドル締付検査フラグ</summary>
            public static readonly ControlDefine RT_STG_TIGHTEN_FLAG = new ControlDefine("txtRtSteeringTightenFlag", "結果 ハンドル締付検査フラグ", "rtSteeringTightenFlag", ControlDefine.BindType.Down, typeof(String));
            /// <summary>ハンドル締付検査従業員番号_検査員</summary>
            public static readonly ControlDefine RT_STG_TIGHTEN_EMPL_CD = new ControlDefine("txtRtSteeringTightenEmplCd", "ハンドル締付検査従業員番号_検査員", "rtSteeringTightenEmplCd", ControlDefine.BindType.Down, typeof(String));
            /// <summary>ハンドル締付検査終了時刻</summary>
            public static readonly ControlDefine RT_STG_TIGHTEN_END_TIME = new ControlDefine("txtRtSteeringTightenEndTime", "ハンドル締付検査終了時刻", "rtSteeringTightenEndTime", ControlDefine.BindType.Down, typeof(DateTime));
            /// <summary>登録日時</summary>
            public static readonly ControlDefine CREATE_DT = new ControlDefine("txtCreateDt", "登録日時", "createDt", ControlDefine.BindType.Down, typeof(DateTime));

            /// <summary>モンロー検査ステーションコード</summary>
            public static readonly ControlDefine MONROE_STATION_CD = new ControlDefine( "txtMonroeStationCd", "モンローステーション", "monroeStationCd", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>PTO検査ステーションコード</summary>
            public static readonly ControlDefine PTO_STATION_CD = new ControlDefine( "txtPtoStationCd", "PTOステーション", "ptoStationCd", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>油圧検査ステーションコード</summary>
            public static readonly ControlDefine HYDRAULIC_STATION_CD = new ControlDefine( "txtHydraulicStationCd", "油圧ステーション", "hydraulicStationCd", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>ヘッドライト検査ステーションコード</summary>
            public static readonly ControlDefine HEADLIGHT_STATION_CD = new ControlDefine( "txtHeadlightStationCd", "ヘッドライトステーション", "headlightStationCd", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>キーストップステーションコード</summary>
            public static readonly ControlDefine KEY_STOP_STATION_CD = new ControlDefine( "txtKeystopStationCd", "キーストップステーション", "keystopStationCd", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>ハンドル締付ステーションコード</summary>
            public static readonly ControlDefine STEERING_TIGHTEN_STATION_CD = new ControlDefine( "txtStgtightenStationCd", "ハンドル締付ステーション", "stgtightenStationCd", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>最高速度検査ステーションコード</summary>
            public static readonly ControlDefine MAX_SPEED_STATION_CD = new ControlDefine( "txtMaxSpeedStationCd", "最高速度ステーション", "maxspeedStationCd", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>電子チェックシート検査ステーションコード</summary>
            public static readonly ControlDefine SIS_STATION_CD = new ControlDefine( "txtSisStationCd", "電子チェックシートステーション", "sisStationCd", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>主B検査ステーションコード</summary>
            public static readonly ControlDefine BRK_B_STATION_CD = new ControlDefine( "txtBrkbStationCd", "主B検査ステーション", "brkbStationCd", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>駐B検査ステーションコード</summary>
            public static readonly ControlDefine BRK_P_STATION_CD = new ControlDefine( "txtBrkpStationCd", "駐B検査ステーション", "brkpStationCd", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>非B検査ステーションコード</summary>
            public static readonly ControlDefine BRK_E_STATION_CD = new ControlDefine( "txtBrkeStationCd", "非B検査ステーション", "brkeStationCd", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>騒音検査ステーションコード</summary>
            public static readonly ControlDefine NOISE_STATION_CD = new ControlDefine( "txtNoiseStationCd", "騒音検査ステーション", "noiseStationCd", ControlDefine.BindType.Down, typeof( string ) );

            /// <summary>最高速検査フラグ</summary>
            public static readonly ControlDefine MAX_SPEED_FLAG_DETAIL = new ControlDefine( "txtMaxSpeedFlagDetail", "最高速検査", "maxSpeedFlagDetail", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>最高速度検査ステーションコード</summary>
            public static readonly ControlDefine MAX_SPEED_STATION_CD_DETAIL = new ControlDefine("txtMaxSpeedStationCdDetail", "最高速度ステーション", "maxspeedStationCdDetail", ControlDefine.BindType.Down, typeof(string));
            /// <summary>結果_最高速度_後_右_補正値</summary>
            public static readonly ControlDefine SPEED_INSPECTION_SEQ = new ControlDefine( "txtSpeedInspectionSeq", "スピード検査", "speedInspectionSeq", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>結果_最高速度_後_右_補正値</summary>
            public static readonly ControlDefine SPEED_INSPECTION_END_DATETIME = new ControlDefine( "txtSpeedInspectionEndDatetime", "検査終了時刻", "speedInspectionEndDatetime", ControlDefine.BindType.Down, typeof( string ) );
        }

        /// <summary>
        /// (速度検査リスト)コントロール定義
        /// </summary>
        public class GRID_SPEED
        {
            /// <summary>最高速検査フラグ</summary>
            //public static readonly ControlDefine MAX_SPEED_FLAG = new ControlDefine("txtMaxSpeedFlag", "最高速検査フラグ", "maxSpeedFlag", ControlDefine.BindType.Down, typeof(String));
            /// <summary>最高速度_前_下限</summary>
            public static readonly ControlDefine FRONT_WHEEL_MIN = new ControlDefine("txtFrontWheelMin", "最高速度_前_下限", "frontWheelMin", ControlDefine.BindType.Down, typeof(String));
            /// <summary>最高速度_前_上限</summary>
            public static readonly ControlDefine FRONT_WHEEL_MAX = new ControlDefine("txtFrontWheelMax", "最高速度_前_上限", "frontWheelMax", ControlDefine.BindType.Down, typeof(String));
            /// <summary>最高速度_後_下限</summary>
            public static readonly ControlDefine REAR_WHEEL_MIN = new ControlDefine("txtRearWheelMin", "最高速度_後_下限", "rearWheelMin", ControlDefine.BindType.Down, typeof(String));
            /// <summary>最高速度_後_上限</summary>
            public static readonly ControlDefine REAR_WHEEL_MAX = new ControlDefine("txtRearWheelMax", "最高速度_後_上限", "rearWheelMax", ControlDefine.BindType.Down, typeof(String));
            /// <summary>最高速度到達後計測時間</summary>
            public static readonly ControlDefine RANGE_HOLD_TIME = new ControlDefine("txtRangeHoldTime", "最高速度到達後計測時間", "rangeHoldTime", ControlDefine.BindType.Down, typeof(String));
            /// <summary>最高速度範囲外打ち切り時間</summary>
            public static readonly ControlDefine CLOSE_TIME = new ControlDefine("txtCloseTime", "最高速度範囲外打ち切り時間", "closeTime", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果 最高速検査フラグ しない</summary>
            public static readonly ControlDefine RT_MAX_SPEED_FLAG = new ControlDefine("txtRtMaxSpeedFlag", "結果 最高速検査フラグ", "rtMaxSpeedFlag", ControlDefine.BindType.Down, typeof(String));
            /// <summary>最高速検査従業員番号_検査員</summary>
            public static readonly ControlDefine RT_MAX_SPEED_EMPL_CD = new ControlDefine("txtRtMaxSpeedemplCd", "最高速検査従業員番号_検査員", "rtMaxSpeedemplCd", ControlDefine.BindType.Down, typeof(String));
            /// <summary>最高速検査終了時刻</summary>
            public static readonly ControlDefine RT_MAX_SPEED_END_TIME = new ControlDefine("txtRtMaxSpeedEndTime", "最高速検査終了時刻", "rtMaxSpeedEndTime", ControlDefine.BindType.Down, typeof(DateTime));
            /// <summary>結果_最高速度_前_左_直値</summary>
            public static readonly ControlDefine RT_FRONT_WHEEL_L = new ControlDefine("txtRtFrontWheelL", "結果_最高速度_前_左_直値", "rtFrontWheelL", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_最高速度_前_右_直値</summary>
            public static readonly ControlDefine RT_FRONT_WHEEL_R = new ControlDefine("txtRtFrontWheelR", "結果_最高速度_前_右_直値", "rtFrontWheelR", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_最高速度_後_左_直値</summary>
            public static readonly ControlDefine RT_REAR_WHEEL_L = new ControlDefine("txtRtRearWheelL", "結果_最高速度_後_左_直値", "rtRearWheelL", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_最高速度_後_右_直値</summary>
            public static readonly ControlDefine RT_REAR_WHEEL_R = new ControlDefine("txtRtRearWheelR", "結果_最高速度_後_右_直値", "rtRearWheelR", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_最高速度_前_左_補正値</summary>
            public static readonly ControlDefine RT_FRONT_WHEEL_L_CV = new ControlDefine("txtRtFrontWheelLCv", "結果_最高速度_前_左_補正値", "rtFrontWheelLCv", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_最高速度_前_右_補正値</summary>
            public static readonly ControlDefine RT_FRONT_WHEEL_R_CV = new ControlDefine("txtRtFrontWheelRCv", "結果_最高速度_前_右_補正値", "rtFrontWheelRCv", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_最高速度_後_左_補正値</summary>
            public static readonly ControlDefine RT_REAR_WHEEL_L_CV = new ControlDefine("txtRtRearWheelLCv", "結果_最高速度_後_左_補正値", "rtRearWheelLCv", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_最高速度_後_右_補正値</summary>
            public static readonly ControlDefine RT_REAR_WHEEL_R_CV = new ControlDefine("txtRtRearWheelRCv", "結果_最高速度_後_右_補正値", "rtRearWheelRCv", ControlDefine.BindType.Down, typeof(String));
            /// <summary>最高速度検査ステーションコード</summary>
            //public static readonly ControlDefine MAX_SPEED_STATION_CD = new ControlDefine("txtMaxSpeedStationCd", "最高速度ステーションコード", "maxspeedStationCd", ControlDefine.BindType.Down, typeof(DateTime));

            /// <summary>結果_最高速度_後_右_補正値</summary>
            public static readonly ControlDefine SPEED_INSPECTION_SEQ = new ControlDefine("txtSpeedInspectionSeq", "スピード検査順序", "speedInspectionSeq", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果_最高速度_後_右_補正値</summary>
            public static readonly ControlDefine SPEED_INSPECTION_END_DATETIME = new ControlDefine("txtSpeedInspectionEndDatetime", "検査終了時刻", "speedInspectionEndDatetime", ControlDefine.BindType.Down, typeof(String));
        }

        #endregion

        #region プロパティ
        /// <summary>
        /// 表示中ページ(ベースフォーム)情報
        /// </summary>
        private BaseForm CurrentForm
        {
            get
            {
                return ((BaseForm)Page);
            }
        }

        /// <summary>
        /// 表示中ユーザコントロール情報
        /// </summary>
        private PageInfo.ST_PAGE_INFO CurrentUCInfo
        {
            get
            {
                return PageInfo.GetUCPageInfo(DetailKeyParam.ProductKind, DetailKeyParam.GroupCd, DetailKeyParam.ClassCd, DetailKeyParam.AssemblyPatternCd);
            }
        }

        /// <summary>
        /// (メイン)コントロール定義
        /// </summary>
        ControlDefine[] _mainControls = null;
        /// <summary>
        /// (メイン)コントロール定義アクセサ
        /// </summary>
        ControlDefine[] MainControls
        {
            get
            {
                if (true == ObjectUtils.IsNull(_mainControls))
                {
                    _mainControls = ControlUtils.GetControlDefineArray(typeof(GRID_MAIN));
                }
                return _mainControls;
            }
        }

        /// <summary>
        /// (速度検査)コントロール定義
        /// </summary>
        ControlDefine[] _speedControls = null;
        /// <summary>
        /// (速度検査)コントロール定義アクセサ
        /// </summary>
        ControlDefine[] SpeedControls
        {
            get
            {
                if (true == ObjectUtils.IsNull(_speedControls))
                {
                    _speedControls = ControlUtils.GetControlDefineArray(typeof(GRID_SPEED));
                }
                return _speedControls;
            }
        }

        /// <summary>
        /// 詳細表示キー情報
        /// </summary>
        private Defines.Interface.ST_DETAIL_PARAM _detailKeyParam;
        /// <summary>
        /// 詳細表示キー情報
        /// </summary>
        public Defines.Interface.ST_DETAIL_PARAM DetailKeyParam
        {
            get
            {
                return _detailKeyParam;
            }
            set
            {
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
        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentForm.RaiseEvent(DoPageLoad);
        }
        #endregion

        #region イベントメソッド

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize()
        {
            //検索結果取得
            Business.DetailViewBusiness.ResultSetMulti resultSet = new Business.DetailViewBusiness.ResultSetMulti();
            try
            {
                resultSet = Business.DetailViewBusiness.SelectTractorAllDetail(DetailKeyParam.ProductModelCd, DetailKeyParam.Serial);
            }
            catch (DataAccessException ex)
            {
                logger.Exception(ex);
                CurrentForm.WriteApplicationMessage(MsgManager.MESSAGE_ERR_80010);
                return;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                CurrentForm.WriteApplicationMessage(MsgManager.MESSAGE_ERR_80010);
                return;
            }
            finally
            {
            }

            //取得データをセッションに格納
            Dictionary<string, object> dicPageControlInfo = new Dictionary<string, object>();
            dicPageControlInfo.Add(SESSION_PAGE_INFO_DB_KEY, resultSet);
            CurrentForm.SessionManager.GetPageControlInfoHandler(CurrentForm.Token).SetPageControlInfo(MANAGE_ID, dicPageControlInfo);

            if (0 == resultSet.MainTable.Rows.Count)
            {
                //検索結果0件
                CurrentForm.WriteApplicationMessage(MsgManager.MESSAGE_WRN_62010, CurrentUCInfo.title);
                return;
            }

            InitializeValues(resultSet);
        }

        /// <summary>
        /// ページロード処理
        /// </summary>
        private void DoPageLoad()
        {
        }

        /// <summary>
        /// 初期値設定
        /// </summary>
        private void InitializeValues(Business.DetailViewBusiness.ResultSetMulti resultSet)
        {
            //来歴情報セット
            lstMainListLB.DataSource = resultSet.MainTable;
            lstMainListLB.DataBind();
            lstMainListLB.SelectedIndex = 0;

            lstMainListRB.DataSource = resultSet.MainTable;
            lstMainListRB.DataBind();
            lstMainListRB.SelectedIndex = 0;

            //選択行背景色変更解除
            foreach (ListViewDataItem li in lstMainListLB.Items)
            {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl(GRID_MAIN.TR_ROW_DATA.controlId);
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace(" " + ResourcePath.CSS.ListSelectedRow, "");
            }

            foreach (ListViewDataItem li in lstMainListRB.Items)
            {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl(GRID_MAIN.TR_ROW_DATA.controlId);
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace(" " + ResourcePath.CSS.ListSelectedRow, "");
            }

            //一覧項目選択済に色変更
            HtmlTableRow trSelectRow = (HtmlTableRow)lstMainListLB.Items[lstMainListLB.SelectedIndex].FindControl(GRID_MAIN.TR_ROW_DATA.controlId);
            trSelectRow.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;

            HtmlTableRow trSelectRowRB = (HtmlTableRow)lstMainListRB.Items[lstMainListLB.SelectedIndex].FindControl(GRID_MAIN.TR_ROW_DATA.controlId);
            trSelectRowRB.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;

            SelectedIndexChangedMainList(0);
        }

        /// <summary>
        /// エクセル出力ボタン処理
        /// </summary>
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            OutputExcel();
        }

        #endregion

        #region Excel処理
        /// <summary>
        /// Excel出力処理
        /// </summary>
        private void OutputExcel()
        {

            // セッションデータ取得
            //Business.DetailViewBusiness.ResultSetMulti res = new Business.DetailViewBusiness.ResultSetMulti();
            //Dictionary<string, object> dicPageControlInfo = CurrentForm.SessionManager.GetPageControlInfoHandler(CurrentForm.Token).GetPageControlInfo(MANAGE_ID);
            //if (true == dicPageControlInfo.ContainsKey(SESSION_PAGE_INFO_DB_KEY))
            //{
            //    res = (Business.DetailViewBusiness.ResultSetMulti)dicPageControlInfo[SESSION_PAGE_INFO_DB_KEY];
            //}

            List<ControlDefine> columns = new List<ControlDefine>();
            columns.AddRange(ControlUtils.GetControlDefineArray(typeof(GRID_MAIN)));
            ControlDefine[] ControlResults = columns.ToArray();

            DataTable list = TractorProcessDao.SelectExcelList( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial );

            // エクセルカラム名表示
            foreach (ControlDefine row in ControlResults)
            {
                foreach ( DataColumn col in list.Columns )
                {
                    if (row.bindField.ToUpper() == col.ColumnName)
                    {
                        col.Caption = row.displayNm;
                        continue;
                    }
                }
            }
            
            //検索条件出力データ作成
            List<ExcelConditionItem> excelCond = new List<ExcelConditionItem>();

            string modelCd = "";
            string serial = "";
            string seq = "";

            foreach (DataRow dataRow in list.Rows) {
                if ( false == modelCd.Equals( "" ) ) {

                    if ( true == modelCd.Equals( StringUtils.ToString( dataRow[GRID_MAIN.MODEL_CD.bindField] ) ) &&
                        true == serial.Equals( StringUtils.ToString( dataRow[GRID_MAIN.SERIAL_NUMBER.bindField] ) ) &&
                        true == seq.Equals( StringUtils.ToString( dataRow[GRID_MAIN.INSPECTION_SEQ.bindField] ) ) ) {

                        modelCd = StringUtils.ToString( dataRow[GRID_MAIN.MODEL_CD.bindField] );
                        serial = StringUtils.ToString( dataRow[GRID_MAIN.SERIAL_NUMBER.bindField] );
                        seq = StringUtils.ToString( dataRow[GRID_MAIN.INSPECTION_SEQ.bindField] );

                        dataRow[GRID_MAIN.MODEL_CD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.COUNTRY_CD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.SERIAL_NUMBER.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.INSPECTION_SEQ.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.IDNO.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.SEQUENCE_NUM.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.PIN_CD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RESULT.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.INSPECTION_EMPL_CD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.INSPECTION_DT.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.INSPECTION_GROUP.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.WHEEL_DRIVE.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.LASHING_HOOK_F.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.LASHING_HOOK_R.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.JIG_COM_F.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.JIG_COM_R.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.NOISE_FLAG.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.SUB_TRANSMISSION.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.MAIN_TRANSMISSION.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.NOISE_FRONT_WHEEL_MIN.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.NOISE_FRONT_WHEEL_MAX.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.NOISE_REAR_WHEEL_MIN.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.NOISE_REAR_WHEEL_MAX.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.NOISE_RANGE_HOLD_TIME.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.NOISE_CLOSE_TIME.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.NOISE_THRESHOLD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.NOISE_THRESHOLD_MIN.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_NOISE_FLAG.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_NOISE_EMPL_CD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_NOISE_END_TIME.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.NOISE_STATION_CD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_NOISE_FRONT_WHEEL_L.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_NOISE_FRONT_WHEEL_R.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_NOISE_REAR_WHEEL_L.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_NOISE_REAR_WHEEL_R.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_NOISE_FRONT_WHEEL_L_CV.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_NOISE_FRONT_WHEEL_R_CV.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_NOISE_REAR_WHEEL_L_CV.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_NOISE_REAR_WHEEL_R_CV.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_NOISE_MAX.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.ABNORMAL_NOISE_FLAG.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_ABNORMAL_NOISE_FLAG.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_ABNORMAL_NOISE_EMPL_CD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_ABNORMAL_NOISE_END_TIME.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.SIS_STATION_CD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.MAX_ALLOW_WEIGHT.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.BRAKING_COM_CLOSE_TIME.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.BRAKING_FORCE_LRDIFF.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.BRAKING_B_FLAG.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.BRAKING_B_FORCE_MIN.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.BRAKING_B_FORCE_MAX.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.BRAKING_B_THRESHOLD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.BRAKING_B_THRESHOLD_MAX.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.BRAKING_P_FLAG.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.BRAKING_P_THRESHOLD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.BRAKING_P_THRESHOLD_MAX.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.BRAKING_E_FLAG.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.BRAKING_E_THRESHOLD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.BRAKING_E_THRESHOLD_MAX.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_BRK_B_FLAG.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_BRK_B_EMPL_CD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.BRK_B_STATION_CD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_BRK_B_MAX_FORCE_REAR_L.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_BRK_B_MAX_FORCE_REAR_R.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_BRK_B_MAX_FORCE_REAR_L_CV.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_BRK_B_MAX_FORCE_REAR_R_CV.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_BRK_B_MAX_PEDAL_FORCE_MAX.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_BRK_P_FLAG.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_BRK_P_EMPL_CD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_BRK_P_END_TIME.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.BRK_P_STATION_CD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_BRK_P_MAX_FORCE_REAR_L.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_BRK_P_MAX_FORCE_REAR_R.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_BRK_P_MAX_FORCE_REAR_L_CV.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_BRK_P_MAX_FORCE_REAR_R_CV.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_BRK_E_FLAG.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_BRK_E_EMPL_CD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_BRK_E_END_TIME.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.BRK_E_STATION_CD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_BRK_E_MAX_FORCE_REAR_L.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_BRK_E_MAX_FORCE_REAR_R.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_BRK_E_MAX_FORCE_REAR_L_CV.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_BRK_E_MAX_FORCE_REAR_R_CV.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.MONROE_FLAG.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_MONROE_FLAG.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_MONROE_EMPL_CD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_MONROE_END_TIME.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.MONROE_STATION_CD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.PTO_FLAG.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_PTO_FLAG.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_PTO_EMPL_CD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_PTO_END_TIME.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.PTO_STATION_CD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.HYDRAULIC_FLAG.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.HYDRAULIC_WEIGHT.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.HYDRAULIC_SLING.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.HYDRAULIC_LOWER_LINK.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_HYDRAULIC_FLAG.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_HYDRAULIC_EMPL_CD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_HYDRAULIC_END_TIME.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.HYDRAULIC_STATION_CD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.HEADLIGHT_FLAG.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_HEADLIGHT_FLAG.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_HEADLIGHT_EMPL_CD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_HEADLIGHT_END_TIME.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.HEADLIGHT_STATION_CD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.KEY_STOP_FLAG.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_KEY_STOP_FLAG.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_KEY_STOP_EMPL_CD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_KEY_STOP_END_TIME.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.KEY_STOP_STATION_CD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.STEERING_TIGHTEN_FLAG.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_STG_TIGHTEN_FLAG.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_STG_TIGHTEN_EMPL_CD.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.RT_STG_TIGHTEN_END_TIME.bindField] = DBNull.Value;
                        dataRow[GRID_MAIN.STEERING_TIGHTEN_STATION_CD.bindField] = DBNull.Value;
                    }

                } else {

                    modelCd = StringUtils.ToString( dataRow[GRID_MAIN.MODEL_CD.bindField] );
                    serial = StringUtils.ToString( dataRow[GRID_MAIN.SERIAL_NUMBER.bindField] );
                    seq = StringUtils.ToString( dataRow[GRID_MAIN.INSPECTION_SEQ.bindField] );
                }
            }
         
            //Excelダウンロード実行(テーブルはExcel出力用に加工する)
            try
            {
                Excel.Download( Page.Response, "検査情報", GetExcelTable( list ), excelCond );
            }
            catch (System.Threading.ThreadAbortException)
            {
                //response.Endで必ず発生する為、正常として扱う
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
            }

            return;
        }

        /// <summary>
        /// 一覧結果Excel出力用データテーブルを作成します
        /// </summary>
        /// <param name="gridColumns">一覧表示対象列</param>
        /// <returns>DataTable</returns>
        private DataTable GetExcelTable(DataTable tblSource)
        {
            //Excel出力テーブル定義作成(一覧表示対象列を出力対象とする)
            DataTable tblResult = new DataTable();
            foreach (DataColumn column in tblSource.Columns)
            {
                if (false == StringUtils.IsBlank(column.Caption))
                {
                    DataColumn colResult = new DataColumn(column.Caption, column.DataType);
                    tblResult.Columns.Add(colResult);
                }
            }

            //一覧元DataTableの情報をExcel出力用テーブルにコピー
            foreach (DataRow rowSrc in tblSource.Rows)
            {
                DataRow rowTo = tblResult.NewRow();
                foreach (DataColumn column in tblSource.Columns)
                {
                    if (false == StringUtils.IsBlank(column.Caption))
                    {
                        rowTo[column.Caption] = rowSrc[column.ColumnName];
                    }
                }
                tblResult.Rows.Add(rowTo);
            }
            tblResult.AcceptChanges();

            return tblResult;
        }

        #endregion

        #region リストビューイベント
        /// <summary>
        /// メインリスト行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstMainListLB_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ItemDataBoundMainLBList(sender, e);
        }
        protected void lstMainListRB_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ItemDataBoundMainRBList(sender, e);
        }

        /// <summary>
        /// メインリスト選択行変更中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstMainListLB_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            //処理なし
        }
        protected void lstMainListRB_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            //処理なし
        }

        /// <summary>
        /// メインリスト選択行変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstMainListLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentForm.RaiseEvent(SelectedIndexChangedMainLBList);
        }
        protected void lstMainListRB_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentForm.RaiseEvent(SelectedIndexChangedMainRBList);
        }

        /// <summary>
        /// メインリストバインド処理（左）
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void SelectedIndexChangedMainLBList()
        {

            int mainIndex = lstMainListLB.SelectedIndex;

            //選択行背景色変更解除
            foreach (ListViewDataItem li in lstMainListLB.Items)
            {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl(GRID_MAIN.TR_ROW_DATA.controlId);
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace(" " + ResourcePath.CSS.ListSelectedRow, "");
            }

            foreach (ListViewDataItem li in lstMainListRB.Items)
            {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl(GRID_MAIN.TR_ROW_DATA.controlId);
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace(" " + ResourcePath.CSS.ListSelectedRow, "");
            }

            //一覧項目選択済に色変更
            HtmlTableRow trSelectRow = (HtmlTableRow)lstMainListLB.Items[mainIndex].FindControl(GRID_MAIN.TR_ROW_DATA.controlId);
            trSelectRow.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;

            HtmlTableRow trSelectRowRB = (HtmlTableRow)lstMainListRB.Items[mainIndex].FindControl(GRID_MAIN.TR_ROW_DATA.controlId);
            trSelectRowRB.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;

            SelectedIndexChangedMainList(mainIndex);
        }

        /// <summary>
        /// メインリストバインド処理（右）
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void SelectedIndexChangedMainRBList()
        {

            int mainIndex = lstMainListRB.SelectedIndex;

            //選択行背景色変更解除
            foreach (ListViewDataItem li in lstMainListLB.Items)
            {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl(GRID_MAIN.TR_ROW_DATA.controlId);
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace(" " + ResourcePath.CSS.ListSelectedRow, "");
            }

            foreach (ListViewDataItem li in lstMainListRB.Items)
            {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl(GRID_MAIN.TR_ROW_DATA.controlId);
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace(" " + ResourcePath.CSS.ListSelectedRow, "");
            }

            //一覧項目選択済に色変更
            HtmlTableRow trSelectRow = (HtmlTableRow)lstMainListLB.Items[mainIndex].FindControl(GRID_MAIN.TR_ROW_DATA.controlId);
            trSelectRow.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;

            HtmlTableRow trSelectRowRB = (HtmlTableRow)lstMainListRB.Items[mainIndex].FindControl(GRID_MAIN.TR_ROW_DATA.controlId);
            trSelectRowRB.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;

            SelectedIndexChangedMainList(mainIndex);
        }

        /// <summary>
        /// 左リストビューデータバインド
        /// </summary>
        /// <param name="parameters"></param>
        private void ItemDataBoundMainLBList(params object[] parameters)
        {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                DataRow rowBind = ((DataRowView)e.Item.DataItem).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues(e.Item, MainControls, rowBind, ref dicSetValues);

                //行クリックイベントセット
                HtmlTableRow trRow = (HtmlTableRow)e.Item.FindControl(GRID_MAIN.TR_ROW_DATA.controlId);
                Button selectBtn = (Button)e.Item.FindControl(GRID_MAIN.SELECT.controlId);
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format(MAIN_VIEW_SELECTED, e.Item.DataItemIndex, selectBtn.UniqueID);

                //ツールチップ設定
                ControlUtils.SetToolTip(e.Item);
            }
        }

        /// <summary>
        /// 右リストビューデータバインド
        /// </summary>
        /// <param name="parameters"></param>
        private void ItemDataBoundMainRBList(params object[] parameters)
        {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                DataRow rowBind = ((DataRowView)e.Item.DataItem).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues(e.Item, MainControls, rowBind, ref dicSetValues);

                //行クリックイベントセット
                HtmlTableRow trRow = (HtmlTableRow)e.Item.FindControl(GRID_MAIN.TR_ROW_DATA.controlId);
                Button selectBtn = (Button)e.Item.FindControl(GRID_MAIN.SELECT.controlId);
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format(MAIN_VIEW_SELECTED, e.Item.DataItemIndex, selectBtn.UniqueID);

                //ツールチップ設定
                ControlUtils.SetToolTip(e.Item);
            }
        }

        /// <summary>
        /// リスト行選択時詳細データ表示処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void SelectedIndexChangedMainList(int paramIndex)
        {

            // セッションデータ取得
            Business.DetailViewBusiness.ResultSetMulti res = new Business.DetailViewBusiness.ResultSetMulti();
            Dictionary<string, object> dicPageControlInfo = CurrentForm.SessionManager.GetPageControlInfoHandler(CurrentForm.Token).GetPageControlInfo(MANAGE_ID);
            if (true == dicPageControlInfo.ContainsKey(SESSION_PAGE_INFO_DB_KEY))
            {
                res = (Business.DetailViewBusiness.ResultSetMulti)dicPageControlInfo[SESSION_PAGE_INFO_DB_KEY];
            }

            // 各項目の値をセット
            setValue(res.MainTable.Rows[paramIndex]);

            DataTable resultSetMaxSpeedList = new DataTable();
            // 速度検査情報取得
            resultSetMaxSpeedList = TractorProcessDao.SelectMaxSpeedInsList( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial, res.MainTable.Rows[paramIndex][GRID_MAIN.INSPECTION_SEQ.bindField].ToString() );

            lstSpeedIns.DataSource = resultSetMaxSpeedList;
            lstSpeedIns.DataBind();
            lstSpeedIns.SelectedIndex = 0;
        }

        /// <summary>
        /// 各項目に値をセットする
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void setValue(DataRow row)
        {

            txtInspectionSeq.Value = row[GRID_MAIN.INSPECTION_SEQ.bindField].ToString();
            txtSeqenceNum.Value = row[GRID_MAIN.SEQUENCE_NUM.bindField].ToString();
            txtPinCd.Value = row[GRID_MAIN.PIN_CD.bindField].ToString();
            txtInspectionDt.Value = row[GRID_MAIN.INSPECTION_DT.bindField].ToString();
            txtInspectionGroup.Value = row[GRID_MAIN.INSPECTION_GROUP.bindField].ToString();
            txtWheelDrive.Value = row[GRID_MAIN.WHEEL_DRIVE.bindField].ToString();
            txtLashingHookF.Value = row[GRID_MAIN.LASHING_HOOK_F.bindField].ToString();
            txtLashingHookR.Value = row[GRID_MAIN.LASHING_HOOK_R.bindField].ToString();
            txtJigComF.Value = row[GRID_MAIN.JIG_COM_F.bindField].ToString();
            txtJigComR.Value = row[GRID_MAIN.JIG_COM_R.bindField].ToString();
            txtNoiseFlag.Value = row[GRID_MAIN.NOISE_FLAG.bindField].ToString();
            txtSubTransmission.Value = row[GRID_MAIN.SUB_TRANSMISSION.bindField].ToString();
            txtMainTransmission.Value = row[GRID_MAIN.MAIN_TRANSMISSION.bindField].ToString();
            txtNoiseFrontWheelMin.Value = row[GRID_MAIN.NOISE_FRONT_WHEEL_MIN.bindField].ToString();
            txtNoiseFrontWheelMax.Value = row[GRID_MAIN.NOISE_FRONT_WHEEL_MAX.bindField].ToString();
            txtNoiseRearWheelMin.Value = row[GRID_MAIN.NOISE_REAR_WHEEL_MIN.bindField].ToString();
            txtNoiseRearWheelMax.Value = row[GRID_MAIN.NOISE_REAR_WHEEL_MAX.bindField].ToString();
            txtNoiseRangeHoldTime.Value = row[GRID_MAIN.NOISE_RANGE_HOLD_TIME.bindField].ToString();
            txtNoiseCloseTime.Value = row[GRID_MAIN.NOISE_CLOSE_TIME.bindField].ToString();
            txtNoiseThreshold.Value = row[GRID_MAIN.NOISE_THRESHOLD.bindField].ToString();
            txtNoiseThresholdMin.Value = row[GRID_MAIN.NOISE_THRESHOLD_MIN.bindField].ToString();
            txtRtNoiseFlag.Value = row[GRID_MAIN.RT_NOISE_FLAG.bindField].ToString();
            //txtRtNoiseEmplCd.Value = row[GRID_MAIN.RT_NOISE_EMPL_CD.bindField].ToString();
            txtRtNoiseEndTime.Value = row[GRID_MAIN.RT_NOISE_END_TIME.bindField].ToString();
            txtRtNoiseFrontWheelL.Value = row[GRID_MAIN.RT_NOISE_FRONT_WHEEL_L.bindField].ToString();
            txtRtNoiseFrontWheelR.Value = row[GRID_MAIN.RT_NOISE_FRONT_WHEEL_R.bindField].ToString();
            txtRtNoiseRearWheelL.Value = row[GRID_MAIN.RT_NOISE_REAR_WHEEL_L.bindField].ToString();
            txtRtNoiseRearWheelR.Value = row[GRID_MAIN.RT_NOISE_REAR_WHEEL_R.bindField].ToString();
            txtRtNoiseFrontWheelLCv.Value = row[GRID_MAIN.RT_NOISE_FRONT_WHEEL_L_CV.bindField].ToString();
            txtRtNoiseFrontWheelRCv.Value = row[GRID_MAIN.RT_NOISE_FRONT_WHEEL_R_CV.bindField].ToString();
            txtRtNoiseRearWheelLCv.Value = row[GRID_MAIN.RT_NOISE_REAR_WHEEL_L_CV.bindField].ToString();
            txtRtNoiseRearWheelRCv.Value = row[GRID_MAIN.RT_NOISE_REAR_WHEEL_R_CV.bindField].ToString();
            txtRtNoiseMax.Value = row[GRID_MAIN.RT_NOISE_MAX.bindField].ToString();
            txtAbnormalNoiseFlag.Value = row[GRID_MAIN.ABNORMAL_NOISE_FLAG.bindField].ToString();
            txtRtAbnormalNoiseFlag.Value = row[GRID_MAIN.RT_ABNORMAL_NOISE_FLAG.bindField].ToString();
            //txtRtAbnormalNoiseEmplCd.Value = row[GRID_MAIN.RT_ABNORMAL_NOISE_EMPL_CD.bindField].ToString();
            txtRtAbnormalNoiseEndTime.Value = row[GRID_MAIN.RT_ABNORMAL_NOISE_END_TIME.bindField].ToString();
            txtMaxAllowWeight.Value = row[GRID_MAIN.MAX_ALLOW_WEIGHT.bindField].ToString();
            txtBrakingComCloseTime.Value = row[GRID_MAIN.BRAKING_COM_CLOSE_TIME.bindField].ToString();
            txtBrakingForceLRDiff.Value = row[GRID_MAIN.BRAKING_FORCE_LRDIFF.bindField].ToString();
            txtBrakingBFlag.Value = row[GRID_MAIN.BRAKING_B_FLAG.bindField].ToString();
            txtBrakingBForceMin.Value = row[GRID_MAIN.BRAKING_B_FORCE_MIN.bindField].ToString();
            txtBrakingBForceMax.Value = row[GRID_MAIN.BRAKING_B_FORCE_MAX.bindField].ToString();
            txtBrakingBThreshold.Value = row[GRID_MAIN.BRAKING_B_THRESHOLD.bindField].ToString();
            txtBrakingBThresholdMax.Value = row[GRID_MAIN.BRAKING_B_THRESHOLD_MAX.bindField].ToString();
            txtBrakingPFlag.Value = row[GRID_MAIN.BRAKING_P_FLAG.bindField].ToString();
            txtBrakingPThreshold.Value = row[GRID_MAIN.BRAKING_P_THRESHOLD.bindField].ToString();
            txtBrakingPThresholdMax.Value = row[GRID_MAIN.BRAKING_P_THRESHOLD_MAX.bindField].ToString();
            txtBrakingEFlag.Value = row[GRID_MAIN.BRAKING_E_FLAG.bindField].ToString();
            txtBrakingEThreshold.Value = row[GRID_MAIN.BRAKING_E_THRESHOLD.bindField].ToString();
            txtBrakingEThresholdMax.Value = row[GRID_MAIN.BRAKING_E_THRESHOLD_MAX.bindField].ToString();
            txtRtBrkBFlag.Value = row[GRID_MAIN.RT_BRK_B_FLAG.bindField].ToString();
            //txtRtBrkBEmplCd.Value = row[GRID_MAIN.RT_BRK_B_EMPL_CD.bindField].ToString();
            txtRtBrkBEndTime.Value = row[GRID_MAIN.RT_BRK_B_END_TIME.bindField].ToString();
            txtRtBrkBMaxForceRearL.Value = row[GRID_MAIN.RT_BRK_B_MAX_FORCE_REAR_L.bindField].ToString();
            txtRtBrkBMaxForceRearR.Value = row[GRID_MAIN.RT_BRK_B_MAX_FORCE_REAR_R.bindField].ToString();
            txtRtBrkBMaxForceRearLCv.Value = row[GRID_MAIN.RT_BRK_B_MAX_FORCE_REAR_L_CV.bindField].ToString();
            txtRtBrkBMaxForceRearRCv.Value = row[GRID_MAIN.RT_BRK_B_MAX_FORCE_REAR_R_CV.bindField].ToString();
            txtRtBrkBMaxPedalForceMax.Value = row[GRID_MAIN.RT_BRK_B_MAX_PEDAL_FORCE_MAX.bindField].ToString();
            txtRtBrkPFlag.Value = row[GRID_MAIN.RT_BRK_P_FLAG.bindField].ToString();
            //txtRtBrkPEmplCd.Value = row[GRID_MAIN.RT_BRK_P_EMPL_CD.bindField].ToString();
            txtRtBrkPEndTime.Value = row[GRID_MAIN.RT_BRK_P_END_TIME.bindField].ToString();
            txtRtBrkPMaxForceRearL.Value = row[GRID_MAIN.RT_BRK_P_MAX_FORCE_REAR_L.bindField].ToString();
            txtRtBrkPMaxForceRearR.Value = row[GRID_MAIN.RT_BRK_P_MAX_FORCE_REAR_R.bindField].ToString();
            txtRtBrkPMaxForceRearLCv.Value = row[GRID_MAIN.RT_BRK_P_MAX_FORCE_REAR_L_CV.bindField].ToString();
            txtRtBrkPMaxForceRearRCv.Value = row[GRID_MAIN.RT_BRK_P_MAX_FORCE_REAR_R_CV.bindField].ToString();
            txtRtBrkEFlag.Value = row[GRID_MAIN.RT_BRK_E_FLAG.bindField].ToString();
            //txtRtBrkEEmplCd.Value = row[GRID_MAIN.RT_BRK_E_EMPL_CD.bindField].ToString();
            txtRtBrkEEndTime.Value = row[GRID_MAIN.RT_BRK_E_END_TIME.bindField].ToString();
            txtRtBrkEMaxForceRearL.Value = row[GRID_MAIN.RT_BRK_E_MAX_FORCE_REAR_L.bindField].ToString();
            txtRtBrkEMaxForceRearR.Value = row[GRID_MAIN.RT_BRK_E_MAX_FORCE_REAR_R.bindField].ToString();
            txtRtBrkEMaxForceRearLCv.Value = row[GRID_MAIN.RT_BRK_E_MAX_FORCE_REAR_L_CV.bindField].ToString();
            txtRtBrkEMaxForceRearRCv.Value = row[GRID_MAIN.RT_BRK_E_MAX_FORCE_REAR_R_CV.bindField].ToString();
            txtMonroeFlag.Value = row[GRID_MAIN.MONROE_FLAG.bindField].ToString();
            txtRtMonroeFlag.Value = row[GRID_MAIN.RT_MONROE_FLAG.bindField].ToString();
            //txtRtMonroeEmplCd.Value = row[GRID_MAIN.RT_MONROE_EMPL_CD.bindField].ToString();
            txtRtMonroeEndTime.Value = row[GRID_MAIN.RT_MONROE_END_TIME.bindField].ToString();
            txtPtoFlag.Value = row[GRID_MAIN.PTO_FLAG.bindField].ToString();
            txtRtPtoFlag.Value = row[GRID_MAIN.RT_PTO_FLAG.bindField].ToString();
            //txtRtPtoEmplCd.Value = row[GRID_MAIN.RT_PTO_EMPL_CD.bindField].ToString();
            txtRtPtoEndTime.Value = row[GRID_MAIN.RT_PTO_END_TIME.bindField].ToString();
            txtHydraulicFlag.Value = row[GRID_MAIN.HYDRAULIC_FLAG.bindField].ToString();
            txtHydraulicWeight.Value = row[GRID_MAIN.HYDRAULIC_WEIGHT.bindField].ToString();
            txtHydraulicSling.Value = row[GRID_MAIN.HYDRAULIC_SLING.bindField].ToString();
            txtHydraulicLowerLink.Value = row[GRID_MAIN.HYDRAULIC_LOWER_LINK.bindField].ToString();
            txtRtHydraulicFlag.Value = row[GRID_MAIN.RT_HYDRAULIC_FLAG.bindField].ToString();
            //txtRtHydraulicEmplCd.Value = row[GRID_MAIN.RT_HYDRAULIC_EMPL_CD.bindField].ToString();
            txtRtHydraulicEndTime.Value = row[GRID_MAIN.RT_HYDRAULIC_END_TIME.bindField].ToString();
            txtHeadlightFlag.Value = row[GRID_MAIN.HEADLIGHT_FLAG.bindField].ToString();
            txtRtHeadlightFlag.Value = row[GRID_MAIN.RT_HEADLIGHT_FLAG.bindField].ToString();
            //txtRtHeadlightEmplCd.Value = row[GRID_MAIN.RT_HEADLIGHT_EMPL_CD.bindField].ToString();
            txtRtHeadlightEndTime.Value = row[GRID_MAIN.RT_HEADLIGHT_END_TIME.bindField].ToString();
            txtKeyStopFlag.Value = row[GRID_MAIN.KEY_STOP_FLAG.bindField].ToString();
            txtRtKeyStopFlag.Value = row[GRID_MAIN.RT_KEY_STOP_FLAG.bindField].ToString();
            //txtRtKeyStopEmplCd.Value = row[GRID_MAIN.RT_KEY_STOP_EMPL_CD.bindField].ToString();
            txtRtKeyStopEmplEndTime.Value = row[GRID_MAIN.RT_KEY_STOP_END_TIME.bindField].ToString();
            txtSteeringTightenFlag.Value = row[GRID_MAIN.STEERING_TIGHTEN_FLAG.bindField].ToString();
            txtRtSteeringTightenFlag.Value = row[GRID_MAIN.RT_STG_TIGHTEN_FLAG.bindField].ToString();
            //txtRtSteeringTightenEmplCd.Value = row[GRID_MAIN.RT_STG_TIGHTEN_EMPL_CD.bindField].ToString();
            txtRtSteeringTightenEndTime.Value = row[GRID_MAIN.RT_STG_TIGHTEN_END_TIME.bindField].ToString();

            txtMonroeStationCd.Value = row[GRID_MAIN.MONROE_STATION_CD.bindField].ToString();
            txtPtoStationCd.Value = row[GRID_MAIN.PTO_STATION_CD.bindField].ToString();
            txtHydraulicStationCd.Value = row[GRID_MAIN.HYDRAULIC_STATION_CD.bindField].ToString();
            txtHeadlightStationCd.Value = row[GRID_MAIN.HEADLIGHT_STATION_CD.bindField].ToString();
            txtKeystopStationCd.Value = row[GRID_MAIN.KEY_STOP_STATION_CD.bindField].ToString();
            txtStgtightenStationCd.Value = row[GRID_MAIN.STEERING_TIGHTEN_STATION_CD.bindField].ToString();
            txtSisStationCd.Value = row[GRID_MAIN.SIS_STATION_CD.bindField].ToString();
            txtBrkbStationCd.Value = row[GRID_MAIN.BRK_B_STATION_CD.bindField].ToString();
            txtBrkpStationCd.Value = row[GRID_MAIN.BRK_P_STATION_CD.bindField].ToString();
            txtBrkeStationCd.Value = row[GRID_MAIN.BRK_E_STATION_CD.bindField].ToString();
            txtNoiseStationCd.Value = row[GRID_MAIN.NOISE_STATION_CD.bindField].ToString();

            txtMaxSpeedFlagDetail.Value = row[GRID_MAIN.MAX_SPEED_FLAG_DETAIL.bindField].ToString();
            txtMaxSpeedStationCdDetail.Value = row[GRID_MAIN.MAX_SPEED_STATION_CD_DETAIL.bindField].ToString();

        }

        #endregion

        protected void lstSpeedIns_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ItemDataBoundSpeedInsList(sender, e);
        }

        /// <summary>
        /// 速度検査ビューデータバインド
        /// </summary>
        /// <param name="parameters"></param>
        private void ItemDataBoundSpeedInsList(params object[] parameters)
        {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                DataRow rowBind = ((DataRowView)e.Item.DataItem).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues(e.Item, SpeedControls, rowBind, ref dicSetValues);

            }
        }
    }
}