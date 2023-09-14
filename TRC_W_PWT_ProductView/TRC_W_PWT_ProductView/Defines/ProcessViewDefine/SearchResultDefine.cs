using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using TRC_W_PWT_ProductView.Defines.TypeDefine;

namespace TRC_W_PWT_ProductView.Defines.ProcessViewDefine {
    public class SearchResultDefine {
        public class GRID_COMMON {
            /// <summary>型式コード(非表示)</summary>
            public static readonly GridViewDefine MODEL_CD = new GridViewDefine( "生産型式", "modelCd", typeof( string ), "", false, HorizontalAlign.Center, 0, false, true );
            /// <summary>型式コード(表記)</summary>
            public static readonly GridViewDefine MODEL_CD_DISP_NM = new GridViewDefine( "生産型式", "modelCdDispNm", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>国コード</summary>
            public static readonly GridViewDefine COUNTRY_CD = new GridViewDefine( "生産国", "countryCd", typeof( string ), "", false, HorizontalAlign.Center, 0, false, true );
            /// <summary>国コード(表記)</summary>
            public static readonly GridViewDefine COUNTRY_CD_DISP_NM = new GridViewDefine( "生産国", "countryCdDispNm", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
            /// <summary>型式名</summary>
            public static readonly GridViewDefine MODEL_NM = new GridViewDefine( "生産型式名", "modelNm", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <summary>6桁機番</summary>
            public static readonly GridViewDefine SERIAL6 = new GridViewDefine( "製品機番", "serial6", typeof( string ), "", false, HorizontalAlign.Center, 0, false, true );
            /// <summary>7桁機番</summary>
            public static readonly GridViewDefine SERIAL7 = new GridViewDefine( "製品機番", "serial7", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>PINコード</summary>
            public static readonly GridViewDefine PIN_CD = new GridViewDefine( "PINコード", "pinCd", typeof( string ), "", true, HorizontalAlign.Center, 130, true, true );
            /// <summary>IDNO</summary>
            public static readonly GridViewDefine IDNO = new GridViewDefine( "IDNO", "idno", typeof( string ), "", true, HorizontalAlign.Center, 70, true, true );
        }

        #region エンジン検索結果定義

        public class GRID_ENGINE_INJECTION_03 {
            /// <summary>測定日時</summary>
            public static readonly GridViewDefine INSPECTION_DT = new GridViewDefine( "測定日時", "inspectionDt", typeof( string ), "{0:yyyy/MM/dd HH:mm:ss}", true, HorizontalAlign.Center, 150, true, true );
            /// <summary>測定号機</summary>
            public static readonly GridViewDefine MEASUREMENT_TERMINAL = new GridViewDefine( "測定号機", "measurementTerminal", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
            /// <summary>統合コード</summary>
            public static readonly GridViewDefine INTEGRATED_CD = new GridViewDefine( "統合コード", "integratedCd", typeof( string ), "", true, HorizontalAlign.Center, 120, true, true );
            /// <summary>結果</summary>
            public static readonly GridViewDefine CC = new GridViewDefine( "CC", "cc", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
            /// <summary>結果</summary>
            public static readonly GridViewDefine FCAM = new GridViewDefine( "FCAM", "fcam", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
            /// <summary>結果</summary>
            public static readonly GridViewDefine FCAM_GEAR = new GridViewDefine( "FCAMギヤ", "fcamGear", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
            /// <summary>結果</summary>
            public static readonly GridViewDefine IN_P = new GridViewDefine( "IN_P", "inP", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
            /// <summary>結果</summary>
            public static readonly GridViewDefine RESULT = new GridViewDefine( "判定", "result", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
        }

        public class GRID_ENGINE_INJECTION_07 {
            /// <summary>測定日時</summary>
            public static readonly GridViewDefine INSPECTION_DT = new GridViewDefine( "測定日時", "inspectionDt", typeof( string ), "{0:yyyy/MM/dd HH:mm:ss}", true, HorizontalAlign.Center, 150, true, true );
            /// <summary>結果</summary>
            public static readonly GridViewDefine RESULT = new GridViewDefine( "判定", "result", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
        }

        public class GRID_ENGINE_FRICTION {
            /// <summary>測定日時</summary>
            public static readonly GridViewDefine INSPECTION_DT = new GridViewDefine( "測定日時", "inspectionDt", typeof( string ), "{0:yyyy/MM/dd HH:mm:ss}", true, HorizontalAlign.Center, 150, true, true );
            /// <summary>結果</summary>
            public static readonly GridViewDefine RPM = new GridViewDefine( "回転数", "rpm", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
            /// <summary>結果</summary>
            public static readonly GridViewDefine RESULT = new GridViewDefine( "判定", "result", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
        }

        public class GRID_ENGINE_TEST_03 {
            /// <summary>測定日時</summary>
            public static readonly GridViewDefine INSPECTION_DT = new GridViewDefine( "測定日時", "inspectionDt", typeof( string ), "{0:yyyy/MM/dd HH:mm:ss}", true, HorizontalAlign.Center, 150, true, true );
            /// <summary>結果</summary>
            public static readonly GridViewDefine RESULT = new GridViewDefine( "判定", "result", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );

        }

        public class GRID_ENGINE_TEST_07 {
            /// <summary>測定日時</summary>
            public static readonly GridViewDefine INSPECTION_DT = new GridViewDefine( "測定日時", "inspectionDt", typeof( string ), "{0:yyyy/MM/dd HH:mm:ss}", true, HorizontalAlign.Center, 150, true, true );
            /// <summary>結果</summary>
            public static readonly GridViewDefine RESULT = new GridViewDefine( "判定", "result", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
        }
        #endregion
    }
}