using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TRC_W_PWT_ProductView.Defines.TypeDefine;

namespace TRC_W_PWT_ProductView.Defines.ProcessViewDefine {
    public class SearchConditionDefine {
        /// <summary>
        /// 検索条件（共通）
        /// </summary>
        public class CONDITION_COMMON {
            /// <summary>製品種別</summary>
            public static readonly ControlDefine PRODUCT_KIND = new ControlDefine( "rblProductKind", "製品種別", "productKind", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>工程</summary>
            public static readonly ControlDefine PROCESS_KIND = new ControlDefine( "ddlProcessKind", "工程種別", "processKind", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>エンジン種別</summary>
            public static readonly ControlDefine ENGINE_KIND = new ControlDefine( "ddlEngineKind", "エンジン種別", "engineKind", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>型式名</summary>
            public static readonly ControlDefine MODEL_NM = new ControlDefine( "txtModelNm", "型式名", "modelNm", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>型式コード</summary>
            public static readonly ControlDefine MODEL_CD = new ControlDefine( "txtModelCd", "型式コード", "modelCd", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>PINコード</summary>
            public static readonly ControlDefine PIN_CD = new ControlDefine( "txtPinCd", "PINコード", "pinCd", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>製品機番</summary>
            public static readonly ControlDefine SERIAL = new ControlDefine( "txtSerial", "製品機番", "serial", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>IDNO</summary>
            public static readonly ControlDefine IDNO = new ControlDefine( "txtIdno", "IDNO", "idno", ControlDefine.BindType.Both, typeof( string ) );
        }

        #region エンジン検索条件定義
        /// <summary>
        /// エンジン検索条件（噴射時期計測03）
        /// </summary>
        /// <see cref="TRC_W_PWT_ProductView.UI.Pages.UserControl.MainProcessPartialView.divEngineInjection03Condition"/>
        /// <seealso cref="TRC_W_PWT_ProductView.UI.Pages.UserControl.MainProcessPartialView.divEngineInjection03Condition_Init(object, EventArgs)"/>
        public class CONDITION_ENGINE_INJECTION_03 {
            /// <summary>範囲(開始)</summary>
            public static readonly ControlDefine DATE_FROM = new ControlDefine( "engineInjection03_cldStart", "開始日付", "dateFrom", ControlDefine.BindType.Both, typeof( DateTime ) );
            /// <summary>範囲(終了)</summary>
            public static readonly ControlDefine DATE_TO = new ControlDefine( "engineInjection03_cldEnd", "終了日付", "dateTo", ControlDefine.BindType.Both, typeof( DateTime ) );
            /// <summary>統合コード</summary>
            public static readonly ControlDefine INTEGRATED_CD = new ControlDefine( "engineInjection03_txtIntegratedCd", "統合コード", "integratedCd", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>判定・結果</summary>
            public static readonly ControlDefine RESULT = new ControlDefine( "engineInjection03_ddlResultKind", "判定", "result", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>判定・結果</summary>
            public static readonly ControlDefine MEASUREMENT_TERMINAL = new ControlDefine( "engineInjection03_ddlMeasurementTerminalKind", "号機", "measurementTerminal", ControlDefine.BindType.Both, typeof( string ) );    
        }
        
        /// <summary>
        /// エンジン検索条件（噴射時期計測07）
        /// </summary>
        /// <see cref="TRC_W_PWT_ProductView.UI.Pages.UserControl.MainProcessPartialView.divEngineInjection07Condition"/>
        /// <seealso cref="TRC_W_PWT_ProductView.UI.Pages.UserControl.MainProcessPartialView.divEngineInjection07Condition_Init(object, EventArgs)"/>
        public class CONDITION_ENGINE_INJECTION_07 {
            /// <summary>範囲(開始)</summary>
            public static readonly ControlDefine DATE_FROM = new ControlDefine( "engineInjection07_cldStart", "開始日付", "dateFrom", ControlDefine.BindType.Both, typeof( DateTime ) );
            /// <summary>範囲(終了)</summary>
            public static readonly ControlDefine DATE_TO = new ControlDefine( "engineInjection07_cldEnd", "終了日付", "dateTo", ControlDefine.BindType.Both, typeof( DateTime ) );
            /// <summary>判定・結果</summary>
            public static readonly ControlDefine RESULT = new ControlDefine( "engineInjection07_ddlResultKind", "判定", "result", ControlDefine.BindType.Both, typeof( string ) );
        }

        /// <summary>
        /// エンジン検索条件（フリクションロス）
        /// </summary>
        /// <see cref="TRC_W_PWT_ProductView.UI.Pages.UserControl.MainProcessPartialView.divEngineFrictionCondition"/>
        /// <seealso cref="TRC_W_PWT_ProductView.UI.Pages.UserControl.MainProcessPartialView.divEngineFrictionCondition_Init(object, EventArgs)"/>
        public class CONDITION_ENGINE_FRICTION {
            /// <summary>範囲(開始)</summary>
            public static readonly ControlDefine DATE_FROM = new ControlDefine( "engineFriction_cldStart", "開始日付", "dateFrom", ControlDefine.BindType.Both, typeof( DateTime ) );
            /// <summary>範囲(終了)</summary>
            public static readonly ControlDefine DATE_TO = new ControlDefine( "engineFriction_cldEnd", "終了日付", "dateTo", ControlDefine.BindType.Both, typeof( DateTime ) );
            /// <summary>判定・結果</summary>
            public static readonly ControlDefine RESULT = new ControlDefine( "engineFriction_ddlResultKind", "判定", "result", ControlDefine.BindType.Both, typeof( string ) );
        }

        /// <summary>
        /// エンジン検索条件定義（エンジン運転測定）
        /// </summary>
        /// <see cref="TRC_W_PWT_ProductView.UI.Pages.UserControl.MainProcessPartialView.divEngineTest03Condition"/>
        /// <seealso cref="TRC_W_PWT_ProductView.UI.Pages.UserControl.MainProcessPartialView.divEngineTest03Condition_Init(object, EventArgs)"/>
        public class CONDITION_ENGINE_TEST_03 {
            /// <summary>範囲(開始)</summary>
            public static readonly ControlDefine DATE_FROM = new ControlDefine( "engineTest03_cldStart", "開始日付", "dateFrom", ControlDefine.BindType.Both, typeof( DateTime ) );
            /// <summary>範囲(終了)</summary>
            public static readonly ControlDefine DATE_TO = new ControlDefine( "engineTest03_cldEnd", "終了日付", "dateTo", ControlDefine.BindType.Both, typeof( DateTime ) );
            /// <summary>判定・結果</summary>
            public static readonly ControlDefine RESULT = new ControlDefine( "engineTest03_ddlResultKind", "判定", "result", ControlDefine.BindType.Both, typeof( string ) );
        }

        /// <summary>
        /// エンジン検索条件定義（エンジン運転測定）
        /// </summary>
        /// <see cref="TRC_W_PWT_ProductView.UI.Pages.UserControl.MainProcessPartialView.divEngineTest07Condition"/>
        /// <seealso cref="TRC_W_PWT_ProductView.UI.Pages.UserControl.MainProcessPartialView.divEngineTest07Condition_Init(object, EventArgs)"/>
        public class CONDITION_ENGINE_TEST_07 {
            /// <summary>範囲(開始)</summary>
            public static readonly ControlDefine DATE_FROM = new ControlDefine( "engineTest07_cldStart", "開始日付", "dateFrom", ControlDefine.BindType.Both, typeof( DateTime ) );
            /// <summary>範囲(終了)</summary>
            public static readonly ControlDefine DATE_TO = new ControlDefine( "engineTest07_cldEnd", "終了日付", "dateTo", ControlDefine.BindType.Both, typeof( DateTime ) );
            /// <summary>判定・結果</summary>
            public static readonly ControlDefine RESULT = new ControlDefine( "engineTest07_ddlResultKind", "判定", "result", ControlDefine.BindType.Both, typeof( string ) );
        }
        #endregion
    }
}