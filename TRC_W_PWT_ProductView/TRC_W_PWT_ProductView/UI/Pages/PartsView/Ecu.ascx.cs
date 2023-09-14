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
using TRC_W_PWT_ProductView.Session;
using TRC_W_PWT_ProductView.Dao.Parts;

namespace TRC_W_PWT_ProductView.UI.Pages.PartsView {
    /// <summary>
    /// エンジン部品詳細画面:ECU
    /// </summary>
    public partial class Ecu : System.Web.UI.UserControl, Defines.Interface.IDetail {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義

        const string GRID_MAIN_VIEW_GROUP_CD = "MainView";

        /// <summary>
        /// 部品組付情報
        /// </summary>
        public class GRID_MAIN {
            /// <summary>組付日時(yyyy/MM/dd hh:mm:ss)</summary>
            public static readonly ControlDefine INSTALL_DT = new ControlDefine( "txtInstallDt", "組付日時", "installDt", ControlDefine.BindType.None, typeof( DateTime ) );
            /// <summary>ステーション名</summary>
            public static readonly ControlDefine STATION_NM = new ControlDefine( "txtStationNm", "ステーション", "stationNm", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>部品機番</summary>
            public static readonly ControlDefine SERIAL = new ControlDefine( "txtPartsSerial", "部品機番", "partsSerial", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>クボタ品番</summary>
            public static readonly ControlDefine PARTS_KUBOTA_NUM = new ControlDefine( "txtPartsKubotaNum", "クボタ品番", "partsKubotaNum", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>メーカー品番</summary>
            public static readonly ControlDefine PARTS_MAKER_NUM = new ControlDefine( "txtPartsMakerNum", "メーカー品番", "partsMakerNum", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>来歴NO</summary>
            public static readonly ControlDefine HISTORY_INDEX = new ControlDefine( "ntbHistoryIndex", "来歴No", "historyIndex", ControlDefine.BindType.Down, typeof( int ) );
        }

        /// <summary>
        /// 来歴情報(常に全件表示)
        /// </summary>
        public class GRID_SUB {
            /// <summary>TR</summary>
            public static readonly ControlDefine TR_ROW_DATA = new ControlDefine( "trRowData", "TR", "", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>組付日時</summary>
            public static readonly ControlDefine INSPECTION_DT = new ControlDefine( "txtInspectionDt", "組付日時", "inspectionDt", ControlDefine.BindType.None, typeof( DateTime ) );
            /// <summary>ECUエンジン品番</summary>
            public static readonly ControlDefine ECU_NUM = new ControlDefine( "txtEcuEngineNum", "ECU品番", "ecuEngineNum", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>ECUハードウェア品番</summary>
            public static readonly ControlDefine ECU_HW_NUM = new ControlDefine( "txtEcuHardNum", "ECU HW品番", "ecuHardNum", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>ECUソフトウェア品番</summary>
            public static readonly ControlDefine ECU_SW_NUM = new ControlDefine( "txtEcuSoftNum", "ECU SW品番", "ecuSoftNum", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>ECU機番</summary>
            public static readonly ControlDefine ECU_SERIAL = new ControlDefine( "txtEcuSerial", "ECU機番", "ecuSerial", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>デンソーハードウェア品番</summary>
            public static readonly ControlDefine DENSO_HW_NUM = new ControlDefine( "txtDensoHardNum", "DENSO HW品番", "densoHardNum", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>デンソーソフトウェア品番</summary>
            public static readonly ControlDefine DENSO_SW_NUM = new ControlDefine( "txtDensoSoftNum", "DENSO SW品番", "densoSoftNum", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>エンジン刻印名</summary>
            public static readonly ControlDefine ENGINE_PRT = new ControlDefine( "txtEnginePrt", "刻印名", "enginePrt", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>ECU No</summary>
            public static readonly ControlDefine ECU_ID = new ControlDefine( "txtEcuId", "ECU No", "ecuId", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>チェックサム</summary>
            public static readonly ControlDefine CHECKSUM = new ControlDefine( "txtEcuCheckCd", "チェックサム", "ecuCheckCd", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>測定端末名</summary>
            public static readonly ControlDefine TERMINAL = new ControlDefine( "txtTerminalNm", "測定端末", "terminalNm", ControlDefine.BindType.Down, typeof( string ) );
        }

        /// <summary>
        /// 一覧表示情報コントロール(左下)
        /// </summary>
        public class GRID_SEARCH_CONTROLS_L {
        }

        /// <summary>
        /// 一覧表示情報コントロール(右下)
        /// </summary>
        public class GRID_SEARCH_CONTROLS_R {
        }

        /// <summary>
        /// 検索結果(更新時共通部)
        /// </summary>
        /// 
        internal class GRID_MAIN_COMMON {
            /// <summary>組付日時</summary>
            public static readonly GridViewDefine INSPECTION_DT = new GridViewDefine( "組付日時", "inspectionDt", typeof( DateTime ), "{0:yyyy/MM/dd HH:mm:ss}", false, HorizontalAlign.Center, 150, true, true );
            /// <summary>ECUエンジン品番</summary>
            public static readonly GridViewDefine ECU_NUM = new GridViewDefine( "ECU<br />エンジン品番", "ecuEngineNum", typeof( string ), "", false, HorizontalAlign.Center, 120, true, true );
            /// <summary>ECU HW品番</summary>
            public static readonly GridViewDefine ECU_HW_NUM = new GridViewDefine( "ECU<br />HW品番", "ecuHardNum", typeof( string ), "", false, HorizontalAlign.Center, 100, true, true );
            /// <summary>ECU SW品番</summary>
            public static readonly GridViewDefine ECU_SW_NUM = new GridViewDefine( "ECU<br />SW品番", "ecuSoftNum", typeof( string ), "", false, HorizontalAlign.Center, 100, true, true );
            /// <summary>ECU機番</summary>
            public static readonly GridViewDefine ECU_SERIAL = new GridViewDefine( "ECU<br />機番", "ecuSerial", typeof( string ), "", false, HorizontalAlign.Center, 120, true, true );
            /// <summary>DENSO HW品番</summary>
            public static readonly GridViewDefine DENSO_HW_NUM = new GridViewDefine( "DENSO<br />HW品番", "densoHardNum", typeof( string ), "", false, HorizontalAlign.Center, 100, true, true );
            /// <summary>DENSO SW品番</summary>
            public static readonly GridViewDefine DENSO_SW_NUM = new GridViewDefine( "DENSO<br />SW品番", "densoSoftNum", typeof( string ), "", false, HorizontalAlign.Center, 120, true, true );
            /// <summary>刻印名</summary>
            public static readonly GridViewDefine ENGINE_PRT = new GridViewDefine( "刻印名", "enginePrt", typeof( string ), "", false, HorizontalAlign.Center, 140, true, true );
            /// <summary>ECU No</summary>
            public static readonly GridViewDefine ECU_ID = new GridViewDefine( "ECU No", "ecuId", typeof( string ), "", false, HorizontalAlign.Center, 100, true, true );
            /// <summary>チェックサム</summary>
            public static readonly GridViewDefine CHECKSUM = new GridViewDefine( "チェックサム", "ecuCheckCd", typeof( string ), "", false, HorizontalAlign.Center, 120, true, true );
            /// <summary>測定端末</summary>
            public static readonly GridViewDefine TERMINAL = new GridViewDefine( "測定端末", "terminalNm", typeof( string ), "", false, HorizontalAlign.Left, 100, true, true );
            /// <summary>処理判定</summary>
            public static readonly GridViewDefine PROC_JUDGE = new GridViewDefine( "処理<br />判定", "prcJdg", typeof( string ), "", false, HorizontalAlign.Center, 50, true, true );
            /// <summary>OEMのID(ECU読出)</summary>
            public static readonly GridViewDefine OEM_ID_ECU = new GridViewDefine( "OEM ID<br />(ECU読出)", "oemIDecu", typeof( string ), "", false, HorizontalAlign.Center, 100, true, true );
            /// <summary>OEMのID(Revファイル)</summary>
            public static readonly GridViewDefine OEM_ID_REV = new GridViewDefine( "OEM ID<br />(Rev FILE)", "oemIDrev", typeof( string ), "", false, HorizontalAlign.Center, 100, true, true );
        }

        public class GRID_INHOUSE_ECU {
            /// <summary>ECU機種</summary>
            public static readonly ControlDefine MODEL_TYPE = new ControlDefine( "txtModelType", "ECU機種", "modelType", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>ECU品番</summary>
            public static readonly ControlDefine PARTS_NUMBER = new ControlDefine( "txtPartsNumber", "ECU品番", "partsNumber", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>基板シリアルNo</summary>
            public static readonly ControlDefine CIRCUIT_BOARD_SERIAL = new ControlDefine( "txtCircuitBoardSerial", "基板シリアルNo", "circuitBoardSerial", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>コネクタ組付け 締付本数</summary>
            public static readonly ControlDefine CONNECTOR_ASM_NUM = new ControlDefine( "ntbConnectorAsmNum", "コネクタ組付け 締付本数", "connectorAsmNum", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>コネクタ組付け トルク上限値</summary>
            public static readonly ControlDefine CONNECTOR_ASM_MAX = new ControlDefine( "ntbConnectorAsmMax", "コネクタ組付け トルク上限値", "connectorAsmMax", ControlDefine.BindType.Down, typeof( decimal ) );
            /// <summary>コネクタ組付け トルク上限値</summary>
            public static readonly ControlDefine CONNECTOR_ASM_MIN = new ControlDefine( "ntbConnectorAsmMin", "コネクタ組付け トルク下限値", "connectorAsmMin", ControlDefine.BindType.Down, typeof( decimal ) );
            /// <summary>コネクタ組付け トルク実測値1</summary>
            public static readonly ControlDefine CONNECTOR_ASM_MEASUREMENT1 = new ControlDefine( "ntbConnectorAsmMeasurement1", "コネクタ組付け トルク実測値1", "connectorAsmMeasurement1", ControlDefine.BindType.Down, typeof( decimal ) );
            /// <summary>コネクタ組付け トルク実測値2</summary>
            public static readonly ControlDefine CONNECTOR_ASM_MEASUREMENT2 = new ControlDefine( "ntbConnectorAsmMeasurement2", "コネクタ組付け トルク実測値2", "connectorAsmMeasurement2", ControlDefine.BindType.Down, typeof( decimal ) );
            /// <summary>コネクタ組付け トルク実測値3</summary>
            public static readonly ControlDefine CONNECTOR_ASM_MEASUREMENT3 = new ControlDefine( "ntbConnectorAsmMeasurement3", "コネクタ組付け トルク実測値3", "connectorAsmMeasurement3", ControlDefine.BindType.Down, typeof( decimal ) );
            /// <summary>コネクタ組付け トルク実測値4</summary>
            public static readonly ControlDefine CONNECTOR_ASM_MEASUREMENT4 = new ControlDefine( "ntbConnectorAsmMeasurement4", "コネクタ組付け トルク実測値4", "connectorAsmMeasurement4", ControlDefine.BindType.Down, typeof( decimal ) );
            /// <summary>コネクタ組付け 締付結果</summary>
            public static readonly ControlDefine CONNECTOR_ASM_RESULT = new ControlDefine( "txtConnectorAsmResult", "コネクタ組付け 締付結果", "connectorAsmResult", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>防湿剤塗布 目視検査結果</summary>
            public static readonly ControlDefine DESICCANT_COAT_INSPX_RESULT = new ControlDefine( "txtDesiccantCoatInspxResult", "防湿剤塗布 目視検査結果", "desiccantCoatInspxResult", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>コネクタ組付け トルク実測値4</summary>
            public static readonly ControlDefine DESICCANT_COAT_OPERATOR_NUM = new ControlDefine( "txtDesiccantCoatOperator", "防湿剤塗布 目視検査作業者", "desiccantCoatOperator", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>コネクタ組付け 締付結果</summary>
            public static readonly ControlDefine HEAT_RADIAT_COAT_INSPX_RESULT = new ControlDefine( "txtHeatRadiatCoatInspxResult", "放熱剤塗布 塗布欠け検査結果", "heatRadiatCoatInspxResult", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>基板・ケース組付け 締付本数</summary>
            public static readonly ControlDefine CIRCUIT_BOARD_ASM_NUM = new ControlDefine( "ntbCircuitBoardAsmNum", "基板・ケース組付け 締付本数", "circuitBoardAsmNum", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>基板・ケース組付け トルク上限値</summary>
            public static readonly ControlDefine CIRCUIT_BOARD_ASM_MAX = new ControlDefine( "ntbCircuitBoardAsmMax", "基板・ケース組付け トルク上限値", "circuitBoardAsmMax", ControlDefine.BindType.Down, typeof( decimal ) );
            /// <summary>基板・ケース組付け トルク下限値</summary>
            public static readonly ControlDefine CIRCUIT_BOARD_ASM_MIN = new ControlDefine( "ntbCircuitBoardAsmMin", "基板・ケース組付け トルク下限値", "circuitBoardAsmMin", ControlDefine.BindType.Down, typeof( decimal ) );
            /// <summary>基板・ケース組付け トルク実測値1</summary>
            public static readonly ControlDefine CIRCUIT_BOARD_ASM_MEASUREMENT1 = new ControlDefine( "ntbCircuitBoardAsmMeasurement1", "基板・ケース組付け トルク実測値1", "circuitBoardAsmMeasurement1", ControlDefine.BindType.Down, typeof( decimal ) );
            /// <summary>基板・ケース組付け トルク実測値2</summary>
            public static readonly ControlDefine CIRCUIT_BOARD_ASM_MEASUREMENT2 = new ControlDefine( "ntbCircuitBoardAsmMeasurement2", "基板・ケース組付け トルク実測値2", "circuitBoardAsmMeasurement2", ControlDefine.BindType.Down, typeof( decimal ) );
            /// <summary>基板・ケース組付け トルク実測値3</summary>
            public static readonly ControlDefine CIRCUIT_BOARD_ASM_MEASUREMENT3 = new ControlDefine( "ntbCircuitBoardAsmMeasurement3", "基板・ケース組付け トルク実測値3", "circuitBoardAsmMeasurement3", ControlDefine.BindType.Down, typeof( decimal ) );
            /// <summary>基板・ケース組付け トルク実測値4</summary>
            public static readonly ControlDefine CIRCUIT_BOARD_ASM_MEASUREMENT4 = new ControlDefine( "ntbCircuitBoardAsmMeasurement4", "基板・ケース組付け トルク実測値4", "circuitBoardAsmMeasurement4", ControlDefine.BindType.Down, typeof( decimal ) );
            /// <summary>基板・ケース組付け トルク実測値5</summary>
            public static readonly ControlDefine CIRCUIT_BOARD_ASM_MEASUREMENT5 = new ControlDefine( "ntbCircuitBoardAsmMeasurement5", "基板・ケース組付け トルク実測値5", "circuitBoardAsmMeasurement5", ControlDefine.BindType.Down, typeof( decimal ) );
            /// <summary>基板・ケース組付け トルク実測値6</summary>
            public static readonly ControlDefine CIRCUIT_BOARD_ASM_MEASUREMENT6 = new ControlDefine( "ntbCircuitBoardAsmMeasurement6", "基板・ケース組付け トルク実測値6", "circuitBoardAsmMeasurement6", ControlDefine.BindType.Down, typeof( decimal ) );
            /// <summary>基板・ケース組付け 締付結果</summary>
            public static readonly ControlDefine CIRCUIT_BOARD_ASM_RESULT = new ControlDefine( "txtCircuitBoardAsmResult", "基板・ケース組付け 締付結果", "circuitBoardAsmResult", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>シール剤塗布 塗布欠け検査結果</summary>
            public static readonly ControlDefine SEAL_MATERIAL_INSPX_RESULT = new ControlDefine( "txtSealMaterialInspxResult", "シール剤塗布 塗布欠け検査結果", "sealMaterialInspxResult", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>上下ケース組付け 締め付け本数</summary>
            public static readonly ControlDefine CASE_ASM_NUM = new ControlDefine( "ntbCaseAsmNum", "上下ケース組付け 締め付け本数", "caseAsmNum", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>上下ケース組付け トルク上限値</summary>
            public static readonly ControlDefine CASE_ASM_MAX = new ControlDefine( "ntbCaseAsmMax", "上下ケース組付け トルク上限値", "caseAsmMax", ControlDefine.BindType.Down, typeof( decimal ) );
            /// <summary>上下ケース組付け トルク下限値</summary>
            public static readonly ControlDefine CASE_ASM_MIN = new ControlDefine( "ntbCaseAsmMin", "上下ケース組付け トルク下限値", "caseAsmMin", ControlDefine.BindType.Down, typeof( decimal ) );
            /// <summary>上下ケース組付け トルク実測値1</summary>
            public static readonly ControlDefine CASE_ASM_MEASUREMENT1 = new ControlDefine( "ntbCaseAsmMeasurement1", "上下ケース組付け トルク実測値1", "caseAsmMeasurement1", ControlDefine.BindType.Down, typeof( decimal ) );
            /// <summary>上下ケース組付け トルク実測値2</summary>
            public static readonly ControlDefine CASE_ASM_MEASUREMENT2 = new ControlDefine( "ntbCaseAsmMeasurement2", "上下ケース組付け トルク実測値2", "caseAsmMeasurement2", ControlDefine.BindType.Down, typeof( decimal ) );
            /// <summary>上下ケース組付け トルク実測値3</summary>
            public static readonly ControlDefine CASE_ASM_MEASUREMENT3 = new ControlDefine( "ntbCaseAsmMeasurement3", "上下ケース組付け トルク実測値3", "caseAsmMeasurement3", ControlDefine.BindType.Down, typeof( decimal ) );
            /// <summary>上下ケース組付け トルク実測値4</summary>
            public static readonly ControlDefine CASE_ASM_MEASUREMENT4 = new ControlDefine( "ntbCaseAsmMeasurement4", "上下ケース組付け トルク実測値4", "caseAsmMeasurement4", ControlDefine.BindType.Down, typeof( decimal ) );
            /// <summary>上下ケース組付け 締付結果</summary>
            public static readonly ControlDefine CASE_ASM_RESULT = new ControlDefine( "txtCaseAsmResult", "上下ケース組付け 締付結果", "caseAsmResult", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>リークテスト 加圧値</summary>
            public static readonly ControlDefine LEAK_PRESSURE_VALUE = new ControlDefine( "ntbLeakPressureValue", "リークテスト 加圧値", "leakPressureValue", ControlDefine.BindType.Down, typeof( decimal ) );
            /// <summary>リークテスト 漏れ量　上限値</summary>
            public static readonly ControlDefine LEAK_AMOUNT_MAX = new ControlDefine( "ntbLeakAmountMax", "リークテスト 漏れ量　上限値", "leakAmountMax", ControlDefine.BindType.Down, typeof( decimal ) );
            /// <summary>リークテスト 漏れ量　下限値</summary>
            public static readonly ControlDefine LEAK_AMOUNT_MIN = new ControlDefine( "ntbLeakAmountMin", "リークテスト 漏れ量　下限値", "leakAmountMin", ControlDefine.BindType.Down, typeof( decimal ) );
            /// <summary>リークテスト 漏れ量</summary>
            public static readonly ControlDefine LEAK_AMOUNT = new ControlDefine( "ntbLeakAmount", "リークテスト 漏れ量", "leakAmount", ControlDefine.BindType.Down, typeof( decimal ) );
            /// <summary>リークテスト 検査結果</summary>
            public static readonly ControlDefine LEAK_INSPECTION_RESULT = new ControlDefine( "txtLeakInspectionResult", "リークテスト 検査結果", "leakInspectionResult", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>最終検査 INJ1・4ピーク電流補正</summary>
            public static readonly ControlDefine LAST_INSPX_INJ14PEAK_CORRECT = new ControlDefine( "txtLastInspxInj14peakCorrect", "最終検査 INJ1・4ピーク電流補正", "lastInspxInj14peakCorrect", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>最終検査 INJ1・4定電流補正</summary>
            public static readonly ControlDefine LAST_INSPX_INJ14CONST_CORRECT = new ControlDefine( "txtLastInspxInj14constCorrect", "最終検査 INJ1・4定電流補正", "lastInspxInj14constCorrect", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>最終検査 INJ2・3ピーク電流補正</summary>
            public static readonly ControlDefine LAST_INSPX_INJ23PEAK_CORRECT = new ControlDefine( "txtLastInspxInj23peakCorrect", "最終検査 INJ2・3ピーク電流補正", "lastInspxInj23peakCorrect", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>最終検査 INJ2・3定電流補正</summary>
            public static readonly ControlDefine LAST_INSPX_INJ23CONST_CORRECT = new ControlDefine( "txtLastInspxInj23constCorrect", "最終検査 INJ2・3定電流補正", "lastInspxInj23constCorrect", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>最終検査 SCVゲイン補正</summary>
            public static readonly ControlDefine LAST_INSPX_SCV_GAIN_CORRECT = new ControlDefine( "txtLastInspxScvGainCorrect", "最終検査 SCVゲイン補正", "lastInspxScvGainCorrect", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>最終検査 SCVオフセット補正</summary>
            public static readonly ControlDefine LAST_INSPX_SCV_OFFSET_CORRECT = new ControlDefine( "txtLastInspxScvOffsetCorrect", "最終検査 SCVオフセット補正", "lastInspxScvOffsetCorrect", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>最終検査 検査結果</summary>
            public static readonly ControlDefine LAST_INSPX_RESULT = new ControlDefine( "txtLastInspxResult", "最終検査 検査結果", "lastInspxResult", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>最終検査 シリアルNo</summary>
            public static readonly ControlDefine ECU_SERIAL = new ControlDefine( "txtEcuSerial", "最終検査 シリアルNo", "ecuSerial", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>最終検査 シリアルNo（上部表示用）</summary>
            public static readonly ControlDefine ECU_SERIAL_HEADER = new ControlDefine( "txtEcuSerialHeader", "最終検査 シリアルNo", "ecuSerial", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>最終検査 シリアルNo（CSV出力用パラメータ）</summary>
            public static readonly ControlDefine ECU_SERIAL_HIDDEN = new ControlDefine( "hdnEcuSerial", "最終検査 シリアルNo", "ecuSerial", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>完了日時</summary>
            public static readonly ControlDefine COMPLETION_DT = new ControlDefine( "txtCompletionDt", "最終検査 シリアルNo", "completionDt", ControlDefine.BindType.Down, typeof( DateTime ) );
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
                return PageInfo.GetUCPageInfo( DetailKeyParam.ProductKind, DetailKeyParam.GroupCd, DetailKeyParam.ClassCd );
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
        /// (内製ECU)コントロール定義アクセサ
        /// </summary>
        ControlDefine[] InhouseEcuControls => ControlUtils.GetControlDefineArray( typeof( GRID_INHOUSE_ECU ) );

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
        /// バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMainViewLB_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainViewLB( sender, e );
        }

        protected void grvMainViewRB_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainViewRB( sender, e );
        }

        protected void btnOutputCsv_Click( object sender, EventArgs e ) {
            CurrentForm.RaiseEvent( OutputCsv );
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
                resultSet = Business.DetailViewBusiness.SelectEngineEcuDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial );
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

            foreach ( DataRow row in resultSet.MainTable.Rows ) {
                DataTable dtSetubi = EnginePartsDao.SelectSetubiJyoho( DetailKeyParam.ProductModelCd, DetailKeyParam.CountryCd );
                if ( dtSetubi != null && 0 < dtSetubi.Rows.Count ) {
                    DataRow drSetubi = dtSetubi.Rows[0];
                    if ( "0" == StringUtils.ToString( drSetubi["MS_JYOHO_1"] ) ) {
                        row["partsMakerNum"] = null;
                    }

                }
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
            //最終組付情報セット
            if ( 1 == resultSet.MainTable.Rows.Count ) {
                //1行のみ表示
                DataRow row = resultSet.MainTable.Rows[0];

                //コントロールへの自動データバインド
                Dictionary<string, object> dicControlValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues( this, MainControls, row, ref dicControlValues );

                //コントロールへの独自データバインド
                //組付日時加工/セット
                KTTextBox txtInstallDt = ( (KTTextBox)this.tblMain.FindControl( GRID_MAIN.INSTALL_DT.controlId ) );
                txtInstallDt.Value = DateUtils.ToString( row[GRID_MAIN.INSTALL_DT.bindField], DateUtils.FormatType.Second );
            }

            if ( 0 < resultSet.SubTable.Rows.Count ) {
                //一覧表示列の設定
                GridViewDefine[] gridColumns;
                List<GridViewDefine> columns = new List<GridViewDefine>();
                columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_MAIN_COMMON ) ) );
                gridColumns = columns.ToArray();

                ConditionInfoSessionHandler.ST_CONDITION cond_sub = new ConditionInfoSessionHandler.ST_CONDITION();
                cond_sub.ResultData = resultSet.SubTable;

                //グリッドビューバインド
                GridView frozenGrid = grvMainViewLB;
                if ( 0 < resultSet.SubTable.Rows.Count ) {

                    //新規バインド
                    ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, gridColumns, true ), cond_sub, true );
                    ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, gridColumns, false ), cond_sub, true );
                    ControlUtils.BindGridView_WithTempField( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, gridColumns, true ), resultSet.SubTable );
                    ControlUtils.BindGridView_WithTempField( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, gridColumns, false ), resultSet.SubTable );

                    //グリッドビュー外のDivサイズ変更
                    SetDivGridViewWidth();
                } else {
                    ClearGridView();
                }
            }
            //クボタ内製ECU
            DataTable inhouseEcuDetail = EnginePartsDao.SelectInhouseEcuInspectionResultDetail( resultSet.MainTable.Rows[0]["partsSerial"].ToString() );
            if ( inhouseEcuDetail != null && inhouseEcuDetail.Rows.Count > 0 ) {
                divInhouseEcuInfo.Visible = true;

                //コントロールへの自動データバインド
                Dictionary<string, object> dicControlValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues( this, InhouseEcuControls, inhouseEcuDetail.Rows[0], ref dicControlValues );

            } else {
                divInhouseEcuInfo.Visible = false;
            }

        }

        /// <summary>
        /// 検査データCSV出力
        /// </summary>
        private void OutputCsv() {
            //ファイル名
            string fileName = "内製ECU検査データ詳細_" + DateTime.Now.ToString( "yyyyMMddHHmmss" ) + ".csv";
            //BLOBデータ取得
            byte[] blobData = ( byte[] )EnginePartsDao.SelectInhouseEcuInspectionResultData( hdnEcuSerial.Value );

            if ( blobData != null && blobData.Length > 0 ) {
                //ダウンロード実行
                WebFileUtils.DownloadFile( CurrentForm, blobData, fileName );
            }
        }
#endregion

#region リストバインド
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
                CurrentForm.SetControlValues( e.Row, ControlUtils.GetControlDefineArray( typeof( GRID_SEARCH_CONTROLS_L ) ), row, ref dicControls );

            }
            ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_MAIN_VIEW_GROUP_CD );

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
                CurrentForm.SetControlValues( e.Row, ControlUtils.GetControlDefineArray( typeof( GRID_SEARCH_CONTROLS_R ) ), rowData, ref dicControls );
            }

            ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_MAIN_VIEW_GROUP_CD );

        }
#endregion

#region Grid設定
        /// <summary>
        /// グリッドビュー格納DIVサイズ調整
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

            double sumWidth = 0;
            int showColCnt = 0;

            for ( int loop = 0; loop < grv.Columns.Count; loop++ ) {

                if ( false == grv.Columns[loop].Visible ) {
                    continue;
                }

                sumWidth += grv.Columns[loop].HeaderStyle.Width.Value + CELL_PADDING;
                showColCnt += 1;
            }

            if ( 0 < showColCnt ) {
                sumWidth += OUT_BORDER;
            }

            div.Style["width"] = Convert.ToInt32( sumWidth ).ToString() + "px";
        }
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

            //GridView非表示
            divGrvDisplay.Visible = false;
        }
#endregion

    }
}